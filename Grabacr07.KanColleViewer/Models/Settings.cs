using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grabacr07.KanColleViewer.Models.Data.Xml;
using Grabacr07.KanColleWrapper;
using Livet;

namespace Grabacr07.KanColleViewer.Models
{
	[Serializable]
	public class Settings : NotificationObject
	{
		#region static members

		private static readonly string filePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"grabacr.net",
			"KanColleViewer",
			"Settings.xml");

		private static readonly string CurrentSettingsVersion = "1.8";

		public static Settings Current { get; set; }

		public static void Load()
		{
			try
			{
				Current = filePath.ReadXml<Settings>();
				if (Current.SettingsVersion != CurrentSettingsVersion)
					Current = GetInitialSettings();
			}
			catch (Exception ex)
			{
				Current = GetInitialSettings();
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}

		public static Settings GetInitialSettings()
		{
			return new Settings
			{
				SettingsVersion = CurrentSettingsVersion,
				ScreenshotFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
				ScreenshotFilename = "KanColle-{0:d04}.png",
				ScreenshotImageFormat = SupportedImageFormat.Png,
				CanDisplayBuildingShipName = false,
				EnableLogging = false,
				EnableTranslations = true,
				EnableAddUntranslated = true,
				EnableStringSubmission = true,
				EnableCriticalNotify = true,
				EnableCriticalAccent = true,
				EnableUpdateNotification = true,
				EnableUpdateTransOnStart = true,
				ShipCatalog_SaveFilters = false,
				ShipCatalog_LevelFilter_Level2OrMore = true,
				ShipCatalog_LockFilter_Locked = true,
				ShipCatalog_SpeedFilter_Both = true,
				ShipCatalog_RemodelFilter_Both = true,
				ShipCatalog_ModernFilter_Both = true,
				ShipCatalog_ShowMoreStats = true,
				SlotItemCatalog_ShowStats = true,
				NotifyBuildingCompleted = true,
				NotifyRepairingCompleted = true,
				NotifyExpeditionReturned = true,
				FlashQuality = "High",
				FlashWindow = "Opaque",
				CustomSoundVolume = 50,
				KanColleClientSettings = new KanColleClientSettings(),
				Culture = "en",
				OrientationHorizontal = false,
			};
		}

		#endregion

		#region SettingsVersion 変更通知プロパティ
		
		private string _SettingsVersion;

		public string SettingsVersion
		{
			get { return this._SettingsVersion; }
			set
			{
				if (this._SettingsVersion != value)
				{
					this._SettingsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region ScreenshotFolder 変更通知プロパティ

		private string _ScreenshotFolder;

		/// <summary>
		/// スクリーンショットの保存先フォルダーを取得または設定します。
		/// </summary>
		public string ScreenshotFolder
		{
			get { return this._ScreenshotFolder; }
			set
			{
				if (this._ScreenshotFolder != value)
				{
					this._ScreenshotFolder = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ScreenshotFilename 変更通知プロパティ

		private string _ScreenshotFilename;

		/// <summary>
		/// スクリーンショットのファイル名を取得または設定します。
		/// </summary>
		public string ScreenshotFilename
		{
			get { return this._ScreenshotFilename; }
			set
			{
				if (this._ScreenshotFilename != value)
				{
					this._ScreenshotFilename = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ScreenshotImageFormat 変更通知プロパティ

		private SupportedImageFormat _ScreenshotImageFormat;

		/// <summary>
		/// スクリーンショットのイメージ形式を取得または設定します。
		/// </summary>
		public SupportedImageFormat ScreenshotImageFormat
		{
			get
			{
				switch (this._ScreenshotImageFormat)
				{
					case SupportedImageFormat.Png:
					case SupportedImageFormat.Jpeg:
						break;
					default:
						this._ScreenshotImageFormat = SupportedImageFormat.Png;
						break;
				}
				return this._ScreenshotImageFormat;
			}
			set
			{
				if (this._ScreenshotImageFormat != value)
				{
					this._ScreenshotImageFormat = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanDisplayBuildingShipName 変更通知プロパティ

		private bool _CanDisplayBuildingShipName;

		/// <summary>
		/// 建造中の艦の名前を表示するかどうかを示す値を取得または設定します。
		/// </summary>
		public bool CanDisplayBuildingShipName
		{
			get { return this._CanDisplayBuildingShipName; }
			set
			{
				if (this._CanDisplayBuildingShipName != value)
				{
					this._CanDisplayBuildingShipName = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyBuildingComplete 変更通知プロパティ

		private bool _NotifyBuildingCompleted;

		public bool NotifyBuildingCompleted
		{
			get { return this._NotifyBuildingCompleted; }
			set
			{
				if (this._NotifyBuildingCompleted != value)
				{
					this._NotifyBuildingCompleted = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyExpeditionReturned 変更通知プロパティ

		private bool _NotifyExpeditionReturned;

		public bool NotifyExpeditionReturned
		{
			get { return this._NotifyExpeditionReturned; }
			set
			{
				if (this._NotifyExpeditionReturned != value)
				{
					this._NotifyExpeditionReturned = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyRepairingCompleted 変更通知プロパティ

		private bool _NotifyRepairingCompleted;

		public bool NotifyRepairingCompleted
		{
			get { return this._NotifyRepairingCompleted; }
			set
			{
				if (this._NotifyRepairingCompleted != value)
				{
					this._NotifyRepairingCompleted = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyFleetRejuvenated 変更通知プロパティ

		private bool _NotifyFleetRejuvenated;

		public bool NotifyFleetRejuvenated
		{
			get { return this._NotifyFleetRejuvenated; }
			set
			{
				if (this._NotifyFleetRejuvenated != value)
				{
					this._NotifyFleetRejuvenated = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ProxySettings 変更通知プロパティ

		private ProxySettings _ProxySettings;

		public ProxySettings ProxySettings
		{
			get { return this._ProxySettings ?? (this._ProxySettings = new ProxySettings()); }
			set
			{
				if (this._ProxySettings != value)
				{
					this._ProxySettings = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#region old properties

		public bool EnableProxy
		{
			get { return this.ProxySettings.IsEnabled; }
			set { this.ProxySettings.IsEnabled = value; }
		}

		public bool EnableSSLProxy
		{
			get { return this.ProxySettings.IsEnabledOnSSL; }
			set { this.ProxySettings.IsEnabledOnSSL = value; }
		}

		public string ProxyHost
		{
			get { return this.ProxySettings.Host; }
			set { this.ProxySettings.Host = value; }
		}
		
		public ushort ProxyPort
		{
			get { return this.ProxySettings.Port; }
			set { this.ProxySettings.Port = value; }
		}

		#endregion

		#endregion

		#region TopMost 変更通知プロパティ

		private bool _TopMost;

		/// <summary>
		/// メイン ウィンドウを常に最前面に表示するかどうかを示す値を取得または設定します。
		/// </summary>
		public bool TopMost
		{
			get { return this._TopMost; }
			set
			{
				if (this._TopMost != value)
				{
					this._TopMost = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Culture 変更通知プロパティ

		private string _Culture;

		/// <summary>
		/// カルチャを取得または設定します。
		/// </summary>
		public string Culture
		{
			get { return this._Culture; }
			set
			{
				if (this._Culture != value)
				{
					this._Culture = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region BrowserZoomFactor 変更通知プロパティ

		private int _BrowserZoomFactorPercentage = 100;
		private double? _BrowserZoomFactor;

		/// <summary>
		/// ブラウザーの拡大率 (パーセンテージ) を取得または設定します。
		/// </summary>
		public int BrowserZoomFactorPercentage
		{
			get { return this._BrowserZoomFactorPercentage; }
			set { this._BrowserZoomFactorPercentage = value; }
		}

		/// <summary>
		/// ブラウザーの拡大率を取得または設定します。
		/// </summary>
		[XmlIgnore]
		public double BrowserZoomFactor
		{
			get { return this._BrowserZoomFactor ?? (this._BrowserZoomFactor = this.BrowserZoomFactorPercentage / 100.0).Value; }
			set
			{
				if (this._BrowserZoomFactor != value)
				{
					this._BrowserZoomFactor = value;
					this._BrowserZoomFactorPercentage = (int)(value * 100);
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region KanColleClientSettings 変更通知プロパティ

		public KanColleClientSettings KanColleClientSettings
		{
			get { return KanColleClient.Current.Settings; }
			set
			{
				if (KanColleClient.Current.Settings != value)
				{
					KanColleClient.Current.Settings = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		

		#region EnableTranslations 変更通知プロパティ

		private bool _EnableTranslations;

		public bool EnableTranslations
		{ 
			get { return this._EnableTranslations; }
			set
			{
				if (this._EnableTranslations != value)
				{
					this._EnableTranslations = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnableAddUntranslated 変更通知プロパティ

		private bool _EnableAddUntranslated;

		public bool EnableAddUntranslated
		{
			get { return this._EnableAddUntranslated; }
			set
			{
				if (this._EnableAddUntranslated != value)
				{
					this._EnableAddUntranslated = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnableStringSubmission 変更通知プロパティ

		private bool _EnableStringSubmission;

		public bool EnableStringSubmission
		{
			get { return this._EnableStringSubmission; }
			set
			{
				if (this._EnableStringSubmission != value)
				{
					this._EnableStringSubmission = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnableCriticalNotify 変更通知プロパティ

		private bool _EnableCriticalNotify;

		public bool EnableCriticalNotify
		{
			get { return this._EnableCriticalNotify; }
			set
			{
				if (this._EnableCriticalNotify != value)
				{
					this._EnableCriticalNotify = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableCriticalAccent 変更通知プロパティ

		private bool _EnableCriticalAccent;

		public bool EnableCriticalAccent
		{
			get { return this._EnableCriticalAccent; }
			set
			{
				if (this._EnableCriticalAccent != value)
				{
					this._EnableCriticalAccent = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableUpdateNotification 変更通知プロパティ

		private bool _EnableUpdateNotification;

		public bool EnableUpdateNotification
		{
			get { return this._EnableUpdateNotification; }
			set
			{
				if (this._EnableUpdateNotification != value)
				{
					this._EnableUpdateNotification = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableUpdateTransOnStart 変更通知プロパティ

		private bool _EnableUpdateTransOnStart;

		public bool EnableUpdateTransOnStart
		{
			get { return this._EnableUpdateTransOnStart; }
			set
			{
				if (this._EnableUpdateTransOnStart != value)
				{
					this._EnableUpdateTransOnStart = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_SaveFilters 変更通知プロパティ

		private bool _ShipCatalog_SaveFilters;

		public bool ShipCatalog_SaveFilters
		{
			get { return this._ShipCatalog_SaveFilters; }
			set
			{
				if (this._ShipCatalog_SaveFilters != value)
				{
					this._ShipCatalog_SaveFilters = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_LevelFilter_Both 変更通知プロパティ

		private bool _ShipCatalog_LevelFilter_Both;

		public bool ShipCatalog_LevelFilter_Both
		{
			get { return this._ShipCatalog_LevelFilter_Both; }
			set
			{
				if (this._ShipCatalog_LevelFilter_Both != value)
				{
					this._ShipCatalog_LevelFilter_Both = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_LevelFilter_Level2OrMore 変更通知プロパティ

		private bool _ShipCatalog_LevelFilter_Level2OrMore;

		public bool ShipCatalog_LevelFilter_Level2OrMore
		{
			get { return this._ShipCatalog_LevelFilter_Level2OrMore; }
			set
			{
				if (this._ShipCatalog_LevelFilter_Level2OrMore != value)
				{
					this._ShipCatalog_LevelFilter_Level2OrMore = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_LevelFilter_Level1 変更通知プロパティ

		private bool _ShipCatalog_LevelFilter_Level1;

		public bool ShipCatalog_LevelFilter_Level1
		{
			get { return this._ShipCatalog_LevelFilter_Level1; }
			set
			{
				if (this._ShipCatalog_LevelFilter_Level1 != value)
				{
					this._ShipCatalog_LevelFilter_Level1 = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_LockFilter_Both 変更通知プロパティ

		private bool _ShipCatalog_LockFilter_Both;

		public bool ShipCatalog_LockFilter_Both
		{
			get { return this._ShipCatalog_LockFilter_Both; }
			set
			{
				if (this._ShipCatalog_LockFilter_Both != value)
				{
					this._ShipCatalog_LockFilter_Both = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_LockFilter_Locked 変更通知プロパティ

		private bool _ShipCatalog_LockFilter_Locked;

		public bool ShipCatalog_LockFilter_Locked
		{
			get { return this._ShipCatalog_LockFilter_Locked; }
			set
			{
				if (this._ShipCatalog_LockFilter_Locked != value)
				{
					this._ShipCatalog_LockFilter_Locked = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_LockFilter_Unlocked 変更通知プロパティ

		private bool _ShipCatalog_LockFilter_Unlocked;

		public bool ShipCatalog_LockFilter_Unlocked
		{
			get { return this._ShipCatalog_LockFilter_Unlocked; }
			set
			{
				if (this._ShipCatalog_LockFilter_Unlocked != value)
				{
					this._ShipCatalog_LockFilter_Unlocked = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_ExpeditionFilter_WithoutExpedition 変更通知プロパティ

		private bool _ShipCatalog_ExpeditionFilter_WithoutExpedition;

		public bool ShipCatalog_ExpeditionFilter_WithoutExpedition
		{
			get { return this._ShipCatalog_ExpeditionFilter_WithoutExpedition; }
			set
			{
				if (this._ShipCatalog_ExpeditionFilter_WithoutExpedition != value)
				{
					this._ShipCatalog_ExpeditionFilter_WithoutExpedition = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_SpeedFilter_Both 変更通知プロパティ

		private bool _ShipCatalog_SpeedFilter_Both;

		public bool ShipCatalog_SpeedFilter_Both
		{
			get { return this._ShipCatalog_SpeedFilter_Both; }
			set
			{
				if (this._ShipCatalog_SpeedFilter_Both != value)
				{
					this._ShipCatalog_SpeedFilter_Both = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_SpeedFilter_Fast 変更通知プロパティ

		private bool _ShipCatalog_SpeedFilter_Fast;

		public bool ShipCatalog_SpeedFilter_Fast
		{
			get { return this._ShipCatalog_SpeedFilter_Fast; }
			set
			{
				if (this._ShipCatalog_SpeedFilter_Fast != value)
				{
					this._ShipCatalog_SpeedFilter_Fast = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_SpeedFilter_Low 変更通知プロパティ

		private bool _ShipCatalog_SpeedFilter_Low;

		public bool ShipCatalog_SpeedFilter_Low
		{
			get { return this._ShipCatalog_SpeedFilter_Low; }
			set
			{
				if (this._ShipCatalog_SpeedFilter_Low != value)
				{
					this._ShipCatalog_SpeedFilter_Low = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_RemodelFilter_Both 変更通知プロパティ

		private bool _ShipCatalog_RemodelFilter_Both;

		public bool ShipCatalog_RemodelFilter_Both
		{
			get { return this._ShipCatalog_RemodelFilter_Both; }
			set
			{
				if (this._ShipCatalog_RemodelFilter_Both != value)
				{
					this._ShipCatalog_RemodelFilter_Both = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_RemodelFilter_AlreadyRemodeling 変更通知プロパティ

		private bool _ShipCatalog_RemodelFilter_AlreadyRemodeling;

		public bool ShipCatalog_RemodelFilter_AlreadyRemodeling
		{
			get { return this._ShipCatalog_RemodelFilter_AlreadyRemodeling; }
			set
			{
				if (this._ShipCatalog_RemodelFilter_AlreadyRemodeling != value)
				{
					this._ShipCatalog_RemodelFilter_AlreadyRemodeling = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_RemodelFilter_NotAlreadyRemodeling 変更通知プロパティ

		private bool _ShipCatalog_RemodelFilter_NotAlreadyRemodeling;

		public bool ShipCatalog_RemodelFilter_NotAlreadyRemodeling
		{
			get { return this._ShipCatalog_RemodelFilter_NotAlreadyRemodeling; }
			set
			{
				if (this._ShipCatalog_RemodelFilter_NotAlreadyRemodeling != value)
				{
					this._ShipCatalog_RemodelFilter_NotAlreadyRemodeling = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_ModernFilter_Both 変更通知プロパティ

		private bool _ShipCatalog_ModernFilter_Both;

		public bool ShipCatalog_ModernFilter_Both
		{
			get { return this._ShipCatalog_ModernFilter_Both; }
			set
			{
				if (this._ShipCatalog_ModernFilter_Both != value)
				{
					this._ShipCatalog_ModernFilter_Both = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_ModernFilter_NotMaxModernized 変更通知プロパティ

		private bool _ShipCatalog_ModernFilter_NotMaxModernized;

		public bool ShipCatalog_ModernFilter_NotMaxModernized
		{
			get { return this._ShipCatalog_ModernFilter_NotMaxModernized; }
			set
			{
				if (this._ShipCatalog_ModernFilter_NotMaxModernized != value)
				{
					this._ShipCatalog_ModernFilter_NotMaxModernized = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_ModernFilter_MaxModernized 変更通知プロパティ

		private bool _ShipCatalog_ModernFilter_MaxModernized;

		public bool ShipCatalog_ModernFilter_MaxModernized
		{
			get { return this._ShipCatalog_ModernFilter_MaxModernized; }
			set
			{
				if (this._ShipCatalog_ModernFilter_MaxModernized != value)
				{
					this._ShipCatalog_ModernFilter_MaxModernized = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipCatalog_ShowMoreStats 変更通知プロパティ

		private bool _ShipCatalog_ShowMoreStats;

		public bool ShipCatalog_ShowMoreStats
		{
			get { return this._ShipCatalog_ShowMoreStats; }
			set
			{
				if (this._ShipCatalog_ShowMoreStats != value)
				{
					this._ShipCatalog_ShowMoreStats = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region SlotItemCatalog_ShowStats 変更通知プロパティ

		private bool _SlotItemCatalog_ShowStats;

		public bool SlotItemCatalog_ShowStats
		{
			get { return this._SlotItemCatalog_ShowStats; }
			set
			{
				if (this._SlotItemCatalog_ShowStats != value)
				{
					this._SlotItemCatalog_ShowStats = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region FlashQuality 変更通知プロパティ

		private string _FlashQuality;

		public string FlashQuality
		{
			get { return this._FlashQuality; }
			set
			{
				if (this._FlashQuality != value)
				{
					this._FlashQuality = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region FlashWindow 変更通知プロパティ

		private string _FlashWindow;

		public string FlashWindow
		{
			get { return this._FlashWindow; }
			set
			{
				if (this._FlashWindow != value)
				{
					this._FlashWindow = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region CustomSoundVolume 変更通知プロパティ

		private int _CustomSoundVolume;

		public int CustomSoundVolume
		{
			get { return this._CustomSoundVolume; }
			set
			{
				if (this._CustomSoundVolume != value)
				{
					this._CustomSoundVolume = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnableLogging 変更通知プロパティ

		private bool _EnableLogging;

		public bool EnableLogging
		{
			get { return this._EnableLogging; }
			set 
			{ 
				if (this._EnableLogging != value)
				{
					this._EnableLogging = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region OrientationHorizontal 変更通知プロパティ

		private bool _OrientationHorizontal;

		public bool OrientationHorizontal
		{
			get { return this._OrientationHorizontal; }
			set
			{
				if (this._OrientationHorizontal != value)
				{
					this._OrientationHorizontal = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		public void Save()
		{
			if (!this.ShipCatalog_SaveFilters)
			{
				this.ShipCatalog_LevelFilter_Level2OrMore = true;
				this.ShipCatalog_LevelFilter_Level1 = this.ShipCatalog_LevelFilter_Both = false;

				this.ShipCatalog_LockFilter_Locked = true;
				this.ShipCatalog_LockFilter_Both = this.ShipCatalog_LockFilter_Unlocked = false;

				this.ShipCatalog_ExpeditionFilter_WithoutExpedition = false;

				this.ShipCatalog_SpeedFilter_Both = true;
				this.ShipCatalog_SpeedFilter_Fast = this.ShipCatalog_SpeedFilter_Low = false;

				this.ShipCatalog_RemodelFilter_Both = true;
				this.ShipCatalog_RemodelFilter_AlreadyRemodeling = this.ShipCatalog_RemodelFilter_NotAlreadyRemodeling = false;

				this.ShipCatalog_ModernFilter_Both = true;
				this.ShipCatalog_ModernFilter_MaxModernized = this.ShipCatalog_ModernFilter_NotMaxModernized = false;
			}

			try
			{
				this.WriteXml(filePath);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}
	}
}
