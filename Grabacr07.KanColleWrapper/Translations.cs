using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Grabacr07.KanColleWrapper.Internal;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

namespace Grabacr07.KanColleWrapper
{
	public class Translations : NotificationObject
	{
		private XDocument ShipsXML;
		private XDocument ShipTypesXML;
		private XDocument EquipmentXML;
		private XDocument OperationsXML;
		private XDocument QuestsXML;
		private string CurrentCulture;

		public bool EnableTranslations { get; set; }
		public bool EnableAddUntranslated { get; set; }
		public bool EnableStringSubmission { get; set; }
		
		#region EquipmentVersion 変更通知プロパティ

		private string _EquipmentVersion;

		public string EquipmentVersion
		{
			get { return _EquipmentVersion; }
			set
			{
				if (_EquipmentVersion != value)
				{
					_EquipmentVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region OperationsVersion 変更通知プロパティ

		private string _OperationsVersion;

		public string OperationsVersion
		{
			get { return _OperationsVersion; }
			set
			{
				if (_OperationsVersion != value)
				{
					_OperationsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region QuestsVersion 変更通知プロパティ

		private string _QuestsVersion;

		public string QuestsVersion
		{
			get { return _QuestsVersion; }
			set
			{
				if (_QuestsVersion != value)
				{
					_QuestsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipsVersion 変更通知プロパティ

		private string _ShipsVersion;

		public string ShipsVersion
		{
			get { return _ShipsVersion; }
			set
			{
				if (_ShipsVersion != value)
				{
					_ShipsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipTypesVersion 変更通知プロパティ

		private string _ShipTypesVersion;

		public string ShipTypesVersion
		{
			get { return _ShipTypesVersion; }
			set
			{
				if (_ShipTypesVersion != value)
				{
					_ShipTypesVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		internal Translations()
		{
			try
			{
				if (File.Exists("Translations\\Ships.xml")) ShipsXML = XDocument.Load("Translations\\Ships.xml");
				if (File.Exists("Translations\\ShipTypes.xml")) ShipTypesXML = XDocument.Load("Translations\\ShipTypes.xml");
				if (File.Exists("Translations\\Equipment.xml")) EquipmentXML = XDocument.Load("Translations\\Equipment.xml");
				if (File.Exists("Translations\\Operations.xml")) OperationsXML = XDocument.Load("Translations\\Operations.xml");
				if (File.Exists("Translations\\Quests.xml")) QuestsXML = XDocument.Load("Translations\\Quests.xml");

				GetVersions();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		public void ChangeCulture(string Culture)
		{
			CurrentCulture = Culture == "en-US" || Culture == "en" ? "" : (Culture + "\\");

			ShipsXML = null;
			ShipTypesXML = null;
			EquipmentXML = null;
			OperationsXML = null;
			QuestsXML = null;

			if (!EnableTranslations || CurrentCulture == "ja-JP")
				return;

			try
			{
				if (!Directory.Exists("Translations")) Directory.CreateDirectory("Translations");
				if (!Directory.Exists("Translations\\" + CurrentCulture)) Directory.CreateDirectory("Translations\\" + CurrentCulture);
				if (File.Exists("Translations\\" + CurrentCulture + "Ships.xml")) ShipsXML = XDocument.Load("Translations\\" + CurrentCulture + "Ships.xml");
				if (File.Exists("Translations\\" + CurrentCulture + "ShipTypes.xml")) ShipTypesXML = XDocument.Load("Translations\\" + CurrentCulture + "ShipTypes.xml");
				if (File.Exists("Translations\\" + CurrentCulture + "Equipment.xml")) EquipmentXML = XDocument.Load("Translations\\" + CurrentCulture + "Equipment.xml");
				if (File.Exists("Translations\\" + CurrentCulture + "Operations.xml")) OperationsXML = XDocument.Load("Translations\\" + CurrentCulture + "Operations.xml");
				if (File.Exists("Translations\\" + CurrentCulture + "Quests.xml")) QuestsXML = XDocument.Load("Translations\\" + CurrentCulture + "Quests.xml");

				GetVersions();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		private void GetVersions()
		{
			if (ShipsXML != null)
				ShipsVersion = ShipsXML.Root.Attribute("Version").Value;
			else
				ShipsVersion = "0.0.0";
			if (ShipTypesXML != null)
				ShipTypesVersion = ShipTypesXML.Root.Attribute("Version").Value;
			else
				ShipTypesVersion = "0.0.0";
			if (EquipmentXML != null)
				EquipmentVersion = EquipmentXML.Root.Attribute("Version").Value;
			else
				EquipmentVersion = "0.0.0";
			if (OperationsXML != null)
				OperationsVersion = OperationsXML.Root.Attribute("Version").Value;
			else
				OperationsVersion = "0.0.0";
			if (QuestsXML != null)
				QuestsVersion = QuestsXML.Root.Attribute("Version").Value;
			else
				QuestsVersion = "0.0.0";
		}

		private IEnumerable<XElement> GetTranslationList(TranslationType Type)
		{
			switch(Type)
			{
				case TranslationType.Ships:
					if (ShipsXML != null) 
						return ShipsXML.Descendants("Ship");
					break;
				case TranslationType.ShipTypes:
					if (ShipTypesXML != null) 
						return ShipTypesXML.Descendants("Type");
					break;
				case TranslationType.Equipment:
					if (EquipmentXML != null) 
						return EquipmentXML.Descendants("Item");
					break;
				case TranslationType.OperationMaps:
					if (OperationsXML != null) 
						return OperationsXML.Descendants("Map");
					break;
				case TranslationType.OperationSortie:
					if (OperationsXML != null) 
						return OperationsXML.Descendants("Sortie");
					break;
				case TranslationType.Quests:
				case TranslationType.QuestTitle:
				case TranslationType.QuestDetail:
					if (QuestsXML != null) 
						return QuestsXML.Descendants("Quest");
					break;
			}

			return null;
		}

		public string GetTranslation(string JPString, TranslationType Type, Object RawData, int ID = -1)
		{
			if (!EnableTranslations || CurrentCulture == "ja-JP")
				return JPString;

			try
			{
				IEnumerable<XElement> TranslationList = GetTranslationList(Type);

				if (TranslationList == null)
				{
					AddTranslation(RawData, Type);
					return JPString;
				}

				string JPChildElement = "JP-Name";
				string TRChildElement = "TR-Name";

				if (Type == TranslationType.QuestDetail)
				{
					JPChildElement = "JP-Detail";
					TRChildElement = "TR-Detail";
				}

				IEnumerable<XElement> FoundTranslation = TranslationList.Where(b => b.Element(JPChildElement).Value.Equals(JPString));

				foreach (XElement el in FoundTranslation)
				{
					Int32 parsedID;
					bool ValidID = false;

					if (ID >= 0 && el.Element("ID") != null)
						ValidID = Int32.TryParse(el.Element("ID").Value, out parsedID);

// #if DEBUG
// 					if (ID >= 0 && el.Element("ID") != null && Convert.ToInt32(el.Element("ID").Value) == ID)
// 						Debug.WriteLine(string.Format("Translation: {0,-20} {1,-20} {2}", JPString, el.Element(TRChildElement).Value, ID));
// #endif
					// Match by ID if an ID is present.
					if (ID >= 0 && el.Element("ID") != null && ValidID && Convert.ToInt32(el.Element("ID").Value) == ID)
						return el.Element(TRChildElement).Value;
					// If an ID is present, but matching by ID has failed: check if we already have an entry with this ID; if not, submit it.
					else if (ID >= 0)
					{
						IEnumerable<XElement> FoundID = TranslationList.Where(b => b.Element("ID").Value.Equals(ID.ToString()));
						if (!FoundID.Any())
						{
							SubmitStrings(RawData, Type);
							return el.Element(TRChildElement).Value;
						}
					}
					else if (ID < 0)
						return el.Element(TRChildElement).Value;

				}
#if DEBUG
				Debug.WriteLine(string.Format("Can't find Translation: {0,-20} {1}", JPString, ID));
#endif
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}

			AddTranslation(RawData, Type);

			return JPString;
		}

		public void AddTranslation(Object RawData, TranslationType Type)
		{
			if (RawData == null || !EnableAddUntranslated)
				return;

			try
			{
				switch (Type)
				{
					case TranslationType.Ships:
						if (ShipsXML == null)
						{
							ShipsXML = new XDocument();
							ShipsXML.Add(new XElement("Ships"));
							ShipsXML.Root.SetAttributeValue("Version", "0.0.0");
							ShipsVersion = "0.0.0";
						}

						kcsapi_mst_ship ShipData = RawData as kcsapi_mst_ship;

						if (ShipData == null)
							return;

						ShipsXML.Root.Add(new XElement("Ship",
							new XElement("JP-Name", ShipData.api_name),
							new XElement("TR-Name", ShipData.api_name)
						));

						ShipsXML.Save("Translations\\" + CurrentCulture + "Ships.xml");
						break;

					case TranslationType.ShipTypes:
						if (ShipTypesXML == null)
						{
							ShipTypesXML = new XDocument();
							ShipTypesXML.Add(new XElement("ShipTypes"));
							ShipTypesXML.Root.SetAttributeValue("Version", "0.0.0");
							ShipTypesVersion = "0.0.0";
						}

						kcsapi_mst_stype TypeData = RawData as kcsapi_mst_stype;

						if (TypeData == null)
							return;

						ShipTypesXML.Root.Add(new XElement("Type",
							new XElement("ID", TypeData.api_id),
							new XElement("JP-Name", TypeData.api_name),
							new XElement("TR-Name", TypeData.api_name)
							));

						ShipTypesXML.Save("Translations\\" + CurrentCulture + "ShipTypes.xml");
						break;

					case TranslationType.Equipment:
						if (EquipmentXML == null)
						{
							EquipmentXML = new XDocument();
							EquipmentXML.Add(new XElement("Equipment"));
							EquipmentXML.Root.SetAttributeValue("Version", "0.0.0");
							EquipmentVersion = "0.0.0";
						}

						kcsapi_mst_slotitem EqiupData = RawData as kcsapi_mst_slotitem;

						if (EqiupData == null)
							return;

						EquipmentXML.Root.Add(new XElement("Item",
							new XElement("JP-Name", EqiupData.api_name),
							new XElement("TR-Name", EqiupData.api_name)
							));

						EquipmentXML.Save("Translations\\" + CurrentCulture + "Equipment.xml");
						break;

					case TranslationType.OperationMaps:
					case TranslationType.OperationSortie:
						if (OperationsXML == null)
						{
							OperationsXML = new XDocument();
							OperationsXML.Add(new XElement("Operations"));
							OperationsXML.Root.SetAttributeValue("Version", "0.0.0");
							OperationsVersion = "0.0.0";
						}

						kcsapi_battleresult OperationsData = RawData as kcsapi_battleresult;

						if (OperationsData == null)
							return;

						if (Type == TranslationType.OperationMaps)
						{
							OperationsXML.Root.Add(new XElement("Map",
								new XElement("JP-Name", OperationsData.api_quest_name),
								new XElement("TR-Name", OperationsData.api_quest_name)
								));
						}
						else
						{
							OperationsXML.Root.Add(new XElement("Sortie",
								new XElement("JP-Name", OperationsData.api_enemy_info.api_deck_name),
								new XElement("TR-Name", OperationsData.api_enemy_info.api_deck_name)
								));
						}

						OperationsXML.Save("Translations\\" + CurrentCulture + "Operations.xml");
						break;

					case TranslationType.Quests:
					case TranslationType.QuestTitle:
					case TranslationType.QuestDetail:
						if (QuestsXML == null)
						{
							QuestsXML = new XDocument();
							QuestsXML.Add(new XElement("Quests"));
							QuestsXML.Root.SetAttributeValue("Version", "0.0.0");
							QuestsVersion = "0.0.0";
						}

						kcsapi_quest QuestData = RawData as kcsapi_quest;

						if (QuestData == null)
							return;

						IEnumerable<XElement> FoundTranslationDetail = QuestsXML.Descendants("Quest").Where(b => b.Element("JP-Detail").Value.Equals(QuestData.api_detail));
						IEnumerable<XElement> FoundTranslationTitle = QuestsXML.Descendants("Quest").Where(b => b.Element("JP-Name").Value.Equals(QuestData.api_title));

						// Check the current list for any errors and fix them before writing a whole new element.
						if (Type == TranslationType.QuestTitle && FoundTranslationDetail != null && FoundTranslationDetail.Any())
						{
							// The title is wrong, but the detail is right. Fix the title.
							foreach (XElement el in FoundTranslationDetail)
								el.Element("JP-Name").Value = QuestData.api_title;
						}
						else if (Type == TranslationType.QuestDetail && FoundTranslationTitle != null && FoundTranslationTitle.Any())
						{
							// We found an existing detail, the title must be broken. Fix it.
							foreach (XElement el in FoundTranslationTitle)
								el.Element("JP-Detail").Value = QuestData.api_detail;
						}
						else
						{
							// The quest doesn't exist at all. Add it.
							QuestsXML.Root.Add(new XElement("Quest",
								new XElement("ID", QuestData.api_no),
								new XElement("JP-Name", QuestData.api_title),
								new XElement("TR-Name", QuestData.api_title),
								new XElement("JP-Detail", QuestData.api_detail),
								new XElement("TR-Detail", QuestData.api_detail)
								));
						}

						QuestsXML.Save("Translations\\" + CurrentCulture + "Quests.xml");
						SubmitStrings(RawData, Type);
						break;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		// Submits missing translations to a remote server.
		public void SubmitStrings(Object RawData, TranslationType Type)
		{
			// Do not submit if the submission option is disabled
			if (!EnableStringSubmission)
				return;

			System.Collections.Specialized.NameValueCollection NewStrings = new System.Collections.Specialized.NameValueCollection();
			switch (Type)
			{
				case TranslationType.Quests:
				case TranslationType.QuestTitle:
				case TranslationType.QuestDetail:
					kcsapi_quest QuestData = RawData as kcsapi_quest;
					if (QuestData == null)
						return;
					NewStrings.Add("type", "Quest");
					NewStrings.Add("ID", QuestData.api_no.ToString());
					NewStrings.Add("Title", QuestData.api_title);
					NewStrings.Add("Detail", QuestData.api_detail);
					break;
			}

			using(WebClient client = new WebClient())
			{
				try
				{
					byte[] responsebytes = client.UploadValues("http://kcv.koumakan.jp/kcv/addstrings", "POST", NewStrings);
					string responsebody = Encoding.UTF8.GetString(responsebytes);
				}
				catch (Exception ex)
				{
					if (ex is WebException || ex is ArgumentNullException) { }
					else { throw; }
				}
			}
		}

	}
}
