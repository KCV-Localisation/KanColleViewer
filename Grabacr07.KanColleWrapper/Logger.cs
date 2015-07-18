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
		private bool waitingForShip;
		private int dockid;
		private readonly int[] shipmats;
		private readonly string LogTimestampFormat = "yyyy-MM-dd HH:mm:ss";

		public bool EnableLogging { get; set; }

		// ReSharper disable once AssignNullToNotNullAttribute
		public static string LogFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");

		public enum LogType
		{
			BuildItem,
			BuildShip,
			ShipDrop,
			Materials
		};

		public struct LogTypeInfo
		{
			public string Parameters;
			public string FileName;

			public LogTypeInfo(string parameters, string fileName)
			{
				this.Parameters = parameters;
				this.FileName = fileName;
			}
		}

		public static Dictionary<LogType, LogTypeInfo> LogParameters =
			new Dictionary<LogType, LogTypeInfo>
			{
				{
					LogType.BuildItem, new LogTypeInfo("Date,Result,Secretary,Secretary level,Fuel,Ammo,Steel,Bauxite",
													   "BuildItemLog.csv")
				},
				{
					LogType.BuildShip, new LogTypeInfo("Date,Result,Secretary,Secretary level,Fuel,Ammo,Steel,Bauxite,# of Build Materials",
													   "BuildShipLog.csv")
				},
				{ 
					LogType.ShipDrop, new LogTypeInfo("Date,Result,Operation,Enemy Fleet,Rank", 
													   "ShipDropLog.csv") 
				},
				{ 
					LogType.Materials, new LogTypeInfo("Date,Fuel,Ammunition,Steel,Bauxite,DevKits,Buckets,Flamethrowers",
													   "MaterialsLog.csv") 
				},
			};

		internal Logger(KanColleProxy proxy)
		{
			this.EnableLogging = KanColleClient.Current.Settings.EnableLogging;

			this.shipmats = new int[5];

			// ちょっと考えなおす
			proxy.api_req_kousyou_createitem.TryParse<kcsapi_createitem>().Subscribe(x => this.CreateItem(x.Data, x.Request));
			proxy.api_req_kousyou_createship.TryParse<kcsapi_createship>().Subscribe(x => this.CreateShip(x.Request));
			proxy.api_get_member_kdock.TryParse<kcsapi_kdock[]>().Subscribe(x => this.KDock(x.Data));
			proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => this.BattleResult(x.Data));
			proxy.api_req_combined_battle_battleresult.TryParse<kcsapi_combined_battle_battleresult>().Subscribe(x => this.BattleResult(x.Data));
			proxy.api_get_member_material.TryParse<kcsapi_material[]>().Subscribe(x => this.MaterialsHistory(x.Data));
			proxy.api_req_hokyu_charge.TryParse<kcsapi_charge>().Subscribe(x => this.MaterialsHistory(x.Data.api_material));
			proxy.api_req_kousyou_destroyship.TryParse<kcsapi_destroyship>().Subscribe(x => this.MaterialsHistory(x.Data.api_material));
		}
		
		private void CreateItem(kcsapi_createitem item, NameValueCollection req)
		{
			try
			{
				this.Log(LogType.BuildItem,
						 item.api_create_flag == 1 ? KanColleClient.Current.Master.SlotItems[item.api_slot_item.api_slotitem_id].Name : "Penguin", //Result
						 KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info.Name, //Secretary
						 KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Level, //Secretary Level
						 req["api_item1"], //Fuel
						 req["api_item2"], //Ammo
						 req["api_item3"], //Steel
						 req["api_item4"] //Bauxite
					);
			}
			catch (Exception)
			{
				// ignored
			}
		}

		private void CreateShip(NameValueCollection req)
		{
			this.waitingForShip = true;
			this.dockid = Int32.Parse(req["api_kdock_id"]);
			this.shipmats[0] = Int32.Parse(req["api_item1"]);
			this.shipmats[1] = Int32.Parse(req["api_item2"]);
			this.shipmats[2] = Int32.Parse(req["api_item3"]);
			this.shipmats[3] = Int32.Parse(req["api_item4"]);
			this.shipmats[4] = Int32.Parse(req["api_item5"]);
		}

		private void KDock(kcsapi_kdock[] docks)
		{
			try
			{
				foreach (var dock in docks.Where(dock => this.waitingForShip && dock.api_id == this.dockid))
				{
					this.Log(LogType.BuildShip,
							 KanColleClient.Current.Master.Ships[dock.api_created_ship_id].Name, //Result
							 KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info.Name, //Secretary
							 KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Level, //Secretary Level
							 this.shipmats[0], //Fuel
							 this.shipmats[1], //Ammo
							 this.shipmats[2], //Steel
							 this.shipmats[3], //Bauxite
							 this.shipmats[4] //Materials
						);

					this.waitingForShip = false;
				}
			}
			catch (Exception)
			{
				this.waitingForShip = false;
			}
		}

		private void BattleResult(kcsapi_battleresult br)
		{
			try
			{
				if (br.api_get_ship == null)
					return;

				this.Log(LogType.ShipDrop,
						 KanColleClient.Current.Translations.GetTranslation(br.api_get_ship.api_ship_name, TranslationType.Ships, br), //Result
						 KanColleClient.Current.Translations.GetTranslation(br.api_quest_name, TranslationType.OperationMaps, br), //Operation
						 KanColleClient.Current.Translations.GetTranslation(br.api_enemy_info.api_deck_name, TranslationType.OperationSortie, br), //Enemy Fleet
						 br.api_win_rank //Rank
					);
			}
			catch (Exception)
			{
				// ignored
			}
		}

		private void BattleResult(kcsapi_combined_battle_battleresult br)
		{
			try
			{
				if (br.api_get_ship == null)
					return;

				this.Log(LogType.ShipDrop,
						 KanColleClient.Current.Translations.GetTranslation(br.api_get_ship.api_ship_name, TranslationType.Ships, br), //Result
						 KanColleClient.Current.Translations.GetTranslation(br.api_quest_name, TranslationType.OperationMaps, br), //Operation
						 KanColleClient.Current.Translations.GetTranslation(br.api_enemy_info.api_deck_name, TranslationType.OperationSortie, br), //Enemy Fleet
						 br.api_win_rank //Rank
					);
			}
			catch (Exception)
			{
				// ignored
			}
		}

		private void MaterialsHistory(kcsapi_material[] source)
		{
			if (source == null || source.Length != 7)
				return;

			this.Log(LogType.Materials,
				source[0].api_value, source[1].api_value, source[2].api_value, source[3].api_value, source[6].api_value, source[5].api_value, source[4].api_value);
		}

		private void MaterialsHistory(int[] source)
		{
			if (source == null || source.Length != 4)
				return;

			this.Log(LogType.Materials,
				source[0], source[1], source[2], source[3], 
				KanColleClient.Current.Homeport.Materials.DevelopmentMaterials, 
				KanColleClient.Current.Homeport.Materials.InstantRepairMaterials, 
				KanColleClient.Current.Homeport.Materials.InstantBuildMaterials);
		}

		private void Log(LogType type, params object[] args)
		{
			if (!this.EnableLogging) return;

			string logPath = this.CreateLogFile(type);

			if (String.IsNullOrEmpty(logPath))
				return;

			using (var w = File.AppendText(logPath))
			{
				w.WriteLine(DateTime.Now.ToString(this.LogTimestampFormat) + "," + string.Join(",", args));
			}
		}

		private string CreateLogFile(LogType type)
		{
			try
			{
				var info = LogParameters[type];
				string fullPath = Path.Combine(LogFolder, info.FileName);

				if (!Directory.Exists(LogFolder))
					Directory.CreateDirectory(LogFolder);

				if (!File.Exists(fullPath))
					File.WriteAllText(fullPath, info.Parameters + Environment.NewLine, new UTF8Encoding(true));

				return fullPath;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}

