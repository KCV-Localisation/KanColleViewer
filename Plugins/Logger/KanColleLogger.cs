using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;

namespace Logger
{
	[Export(typeof(IToolPlugin))]
	[ExportMetadata("Title", "KanColleLogger")]
	[ExportMetadata("Description", "シンプルな回数カウント機能を提供します。")]
	[ExportMetadata("Version", "1.0")]
	[ExportMetadata("Author", "@Xiatian")]
	public class KanColleCounter : IToolPlugin
	{
		private readonly LoggerViewModel viewmodel = new LoggerViewModel
		{
			Loggers = new ObservableCollection<LoggerBase>
			{
				new ItemLog(KanColleClient.Current.Proxy),
			}
		};

		public string ToolName
		{
			get { return "Logger"; }
		}

		public object GetSettingsView()
		{
			return null;
		}

		public object GetToolView()
		{
			return new LoggerView { DataContext = this.viewmodel, };
		}
	}
}
