using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

namespace Logger
{
	public abstract class LoggerBase : NotificationObject
	{
		#region Text 変更通知プロパティ

		private string _Text;

		public string Text
		{
			get { return this._Text; }
			set
			{
				if (this._Text != value)
				{
					this._Text = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Enabled 変更通知プロパティ

		private bool _Enabled;

		public bool Enabled
		{
			get { return this._Enabled; }
			set
			{
				if (this._Enabled != value)
				{
					this._Enabled = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public readonly string LogTimestampFormat = "yyyy-MM-dd HH:mm:ss";

		public void Log(string filename, string format, params object[] args)
		{
			try
			{
				string mainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
				using (var w = File.AppendText(mainFolder + "\\" + filename))
				{
					w.WriteLine(format, args);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}

	public class ItemLog : LoggerBase
	{
		public ItemLog(KanColleProxy proxy)
		{
			proxy.api_req_kousyou_createitem.TryParse<kcsapi_createitem>().Subscribe(x => this.CreateItem(x.Data, x.Request));
			this.Text = "Development";
			this.Enabled = true;
		}

		private void CreateItem(kcsapi_createitem item, NameValueCollection req)
		{
			if (!this.Enabled) return;
			Log("DevelopmentLog.csv", "{0},{1},{2},{3},{4},{5},{6}",
				DateTime.Now.ToString(this.LogTimestampFormat),
				item.api_create_flag == 1 ? KanColleClient.Current.Master.SlotItems[item.api_slot_item.api_slotitem_id].Name : "Penguin",
				KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info.ShipType.Name,
				req["api_item1"], req["api_item2"], req["api_item3"], req["api_item4"]);
		}
	}
}
