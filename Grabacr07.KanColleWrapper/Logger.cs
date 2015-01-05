using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Internal;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

namespace Grabacr07.KanColleWrapper
{
    public class Logger : NotificationObject
    {
        public bool EnableLogging { get; set; }

        private bool waitingForShip;
        private int dockid;
        private readonly BuildShip.Recipe recipe;

        protected class LogItem
        {
            protected readonly string LogTimestampFormat = "yyyy-MM-dd HH:mm";
            public LogItem()
            {
                Date = DateTime.Now;
            }

            public DateTime Date { get; }
            
            /// <summary>
            /// Create a CSV serialization of the current class.
            /// </summary>
            /// <returns>A csv string with serialized items. </returns>
            public virtual string ToCsv()
            {
                PropertyInfo[] properties = this.GetType().GetProperties();
                var sb = new StringBuilder();
                sb.Append(this.Date.ToString(LogTimestampFormat)).Append(',');
                foreach (var prp in properties)
                {
                    if (prp.CanRead && prp.Name != "Date")
                    {
                        sb.Append(prp.GetValue(this)).Append(',');
                    }
                }
                sb.Length--;
                return sb.ToString();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public virtual string CsvTitle()
            {
                PropertyInfo[] properties = this.GetType().GetProperties();
                var sb = new StringBuilder();
                sb.Append("Date").Append(',');
                foreach (var prp in properties)
                {
                    if (prp.Name != "Date") { sb.Append(prp.Name).Append(','); }
                }
                sb.Length--;
                return sb.ToString();
            }
            protected Object GetPropertyByString(string s)
            {
                Queue<string> q = new Queue<string>(s.Split('.'));
                return GetPropertyByQueue(q, this); ;
            }
            private Object GetPropertyByQueue(Queue<string> s, Object p)
            {
                Object o = p.GetType().GetProperty(s.Dequeue()).GetValue(p);
                if (s.IsEmpty()) return o;
                return GetPropertyByQueue(s, o);
            }
        }

        protected class Craft : LogItem
        {
            public class Recipe
            {
                public int Fuel { get; set; }
                public int Ammo { get; set; }
                public int Steel { get; set; }
                public int Bauxite { get; set; }
            }
            public Recipe CraftRecipe { get; set; }
            public ShipInfo Secretary { get; set; }
        }
        protected class BuildItem : Craft
        {
            public string Result { get; set; }
            public override string ToCsv()
            {    
                var sb = new StringBuilder();
                //This is to support templating on semi-arbitrary data for logging
                //TODO: Define this globally or from file
                sb.Append(this.Date.ToString(LogTimestampFormat)).Append(',');
                string[] items = { "Result", "Secretary.ShipType.Name", "CraftRecipe.Fuel", "CraftRecipe.Ammo", "CraftRecipe.Steel", "CraftRecipe.Bauxite" };
                foreach (var item in items)
                {
                    sb.Append(this.GetPropertyByString(item)).Append(',');
                }
                sb.Length--;
                return sb.ToString();
            }
            public override string CsvTitle()
            {
                return "Date,Result,Secretary,Fuel,Ammo,Steel,Bauxite";
            }

        }

        protected class BuildShip : Craft
        {
            public class Recipe : Craft.Recipe
            { public int Materials { get; set; } };
            public ShipInfo Result { get; set; }
            public override string ToCsv()
            {
                var sb = new StringBuilder();
                //This is to support templating on semi-arbitrary data for logging
                //TODO: Define this globally or from file
                sb.Append(this.Date.ToString(LogTimestampFormat)).Append(',');
                string[] items = { "Result.Name", "CraftRecipe.Fuel", "CraftRecipe.Ammo", "CraftRecipe.Steel", "CraftRecipe.Bauxite", "CraftRecipe.Materials" };
                foreach (var item in items)
                {
                    sb.Append(this.GetPropertyByString(item)).Append(',');
                }
                sb.Length--;
                return sb.ToString();
            }
            public override string CsvTitle()
            {
                return "Date,Result,Fuel,Ammo,Steel,Bauxite,# of Build Materials";
            }
        }

        protected class ShipDrop : LogItem
        {
            public string Result { get; set; }
            public string Operation { get; set; }
            public string EnemyFleet { get; set; }
            public string Rank { get; set; }
        }
        protected class Resources : LogItem
        {
            public int Fuel { get; set; }
            public int Ammo { get; set; }
            public int Steel { get; set; }
            public int Bauxite { get; set; }
            public int Materials { get; set; }
            public int Buckets { get; set; }
            public int Flamethrowers { get; set; }
        }

        internal Logger(KanColleProxy proxy)
        {
            recipe = new BuildShip.Recipe();
            // ちょっと考えなおす
            proxy.api_req_kousyou_createitem.TryParse<kcsapi_createitem>().Subscribe(x => this.CreateItem(x.Data, x.Request));
            proxy.api_req_kousyou_createship.TryParse<kcsapi_createship>().Subscribe(x => this.CreateShip(x.Request));
            proxy.api_get_member_kdock.TryParse<kcsapi_kdock[]>().Subscribe(x => this.KDock(x.Data));
            proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => this.BattleResult(x.Data));
            proxy.api_get_member_material.TryParse<kcsapi_material[]>().Subscribe(x => this.MaterialsHistory(x.Data));
            proxy.api_req_hokyu_charge.TryParse<kcsapi_charge>().Subscribe(x => this.MaterialsHistory(x.Data.api_material));
            proxy.api_req_kousyou_destroyship.TryParse<kcsapi_destroyship>().Subscribe(x => this.MaterialsHistory(x.Data.api_material));
        }

        private void CreateItem(kcsapi_createitem item, NameValueCollection req)
        {
            var logitem = new BuildItem
            {
                Result = item.api_create_flag == 1 ? KanColleClient.Current.Master.SlotItems[item.api_slot_item.api_slotitem_id].Name : "Penguin",
                Secretary = KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info,
                CraftRecipe = new Craft.Recipe
                {
                    Fuel = Int32.Parse(req["api_item1"]),
                    Ammo = Int32.Parse(req["api_item2"]),
                    Steel = Int32.Parse(req["api_item3"]),
                    Bauxite = Int32.Parse(req["api_item4"]),
                }
            };
            Log(logitem);
        }

        private void CreateShip(NameValueCollection req)
        {
            this.waitingForShip = true;
            this.dockid = Int32.Parse(req["api_kdock_id"]);

            this.recipe.Fuel = Int32.Parse(req["api_item1"]);
            this.recipe.Ammo = Int32.Parse(req["api_item2"]);
            this.recipe.Steel = Int32.Parse(req["api_item3"]);
            this.recipe.Bauxite = Int32.Parse(req["api_item4"]);
            this.recipe.Materials = Int32.Parse(req["api_item5"]);
        }

        private void KDock(kcsapi_kdock[] docks)
        {
            foreach (var dock in docks.Where(dock => this.waitingForShip && dock.api_id == this.dockid))
            {
                var logitem = new BuildShip
                {
                    Result = KanColleClient.Current.Master.Ships[dock.api_created_ship_id],
                    Secretary = KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info,
                    CraftRecipe = this.recipe
                };
                Log(logitem);
                this.waitingForShip = false;
            }
        }

        private void BattleResult(kcsapi_battleresult br)
        {
            if (br.api_get_ship == null)
                return;
            var logitem = new ShipDrop
            {
                Result = KanColleClient.Current.Translations.GetTranslation(br.api_get_ship.api_ship_name, TranslationType.Ships, br),
                Operation = KanColleClient.Current.Translations.GetTranslation(br.api_quest_name, TranslationType.OperationMaps, br),
                EnemyFleet = KanColleClient.Current.Translations.GetTranslation(br.api_enemy_info.api_deck_name, TranslationType.OperationSortie, br),
                Rank = br.api_win_rank
            };
            Log(logitem);
        }

        private void MaterialsHistory(kcsapi_material[] source)
		{
			if (source == null || source.Length != 7)
				return;

            var logitem = new Resources
            {
                Fuel = source[0].api_value,
                Ammo = source[1].api_value,
                Steel = source[2].api_value,
                Bauxite = source[3].api_value,
                Materials = source[6].api_value,
                Buckets = source[5].api_value,
                Flamethrowers = source[4].api_value
            };
			Log(logitem);
		}

		private void MaterialsHistory(int[] source)
		{
			if (source == null || source.Length != 4)
				return;

            var logitem = new Resources
            {
                Fuel = source[0],
                Ammo = source[1],
                Steel = source[2],
                Bauxite = source[3],
                Materials = KanColleClient.Current.Homeport.Materials.DevelopmentMaterials,
                Buckets = KanColleClient.Current.Homeport.Materials.InstantRepairMaterials,
                Flamethrowers = KanColleClient.Current.Homeport.Materials.InstantBuildMaterials
            };

            Log(logitem);
 		}

protected virtual void Log(LogItem item)
        {
            if (!this.EnableLogging) return;

            try
            {
                string mainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

                string logpath;
                //TODO: Make this into a list, addable by function. Possibly set default filename in object
                switch (item.GetType().Name)
                {
                    case "BuildItem":
                        logpath = mainFolder + "\\ItemBuildLog.csv";
                        break;
                    case "BuildShip":
                        logpath = mainFolder + "\\ShipBuildLog.csv";
                        break;
                    case "ShipDrop":
                        logpath = mainFolder + "\\DropLog.csv";
                        break;
                    case "Resources":
                        logpath = mainFolder + "\\MaterialsLog.csv";
                        break;
                    default:
                        return;
                }
                if (!File.Exists(logpath))
                {
                    using (var w = File.AppendText(logpath))
                    {
                        w.WriteLine(item.CsvTitle());
                    }
                }
                using (var w = File.AppendText(logpath))
                {
                    w.WriteLine(item.ToCsv());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
