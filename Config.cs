using System.Runtime.Versioning;
using System.Xml;
using Microsoft.VisualBasic.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RosterizerLW
{
  [SupportedOSPlatform("windows")] // shut up warning CA1416
  #pragma warning disable CS8602 
  public class AppConfig
  {
    public XmlDocument XMLConfig { get; set; }
    public XmlNodeList appNode { get; set; }
    public XmlNodeList versionNode { get; set; }
    public static string Xcom2JsonPath { get; set; }
    public static string OutputPath { get; set; }
    public static string BackupPath { get; set; }
    public static ulong Xcom2JsonHash { get; set; }
    public static string XcomSavePath { get; set; }
    public static int SavesToBackup { get; set; }
    public static Color DeadBg { get; set; }
    public static Color DeadFg { get; set; }
    public static Color WoundBg { get; set; }
    public static Color FatigueBg { get; set; }
    public static Color ShivBg { get; set; }
    public static Color MaxBg { get; set; }
    public static Color HiBg { get; set; }
    public static Color LoBg { get; set; }
    public static Color MinBg { get; set; }
    public static Color RosterHeaderBg { get; set; }
    public static Color RosterTabUnselectedBg { get; set; }
    public static Color RosterHeaderFg { get; set; }
    public static Color RosterSquadHotTrackBg { get; set; }
    public static Color RosterSquadHotTrackFg { get; set; }
    public static Color RosterSelectedBookendBg { get; set; }
    public static Color WoobieBg { get; set; }
    public static Color WoobieFg { get; set; }
    public static Color WoobieFg2 { get; set; }
    public static Color SquadHeaderBg { get; set; }
    public static Color SquadTabUnselectedBg { get; set; }
    public static Color SquadHeaderFg { get; set; }
    public static Color SoldierHeaderBg { get; set; }
    public static Color SoldierTabUnselectedBg { get; set; }
    public static Color SoldierHeaderFg { get; set; }
    public static Color ChecklistBadHeaderBg { get; set; }
    public static Color ChecklistBadTabUnselectedBg { get; set; }
    public static Color ChecklistBadHeaderFg { get; set; }
    public static Color ChecklistBadTabHighlight { get; set; }
    public static Color ChecklistGoodHeaderBg { get; set; }
    public static Color ChecklistGoodTabUnselectedBg { get; set; }
    public static Color ChecklistGoodHeaderFg { get; set; }
    public static Color SquadRowBg { get; set; }
    public static Color SquadRowFg { get; set; }
    public static Color SoldierRowFg { get; set; }
    public static Color WindowTitleBg { get; set; }
    public static Color WindowTitleFg { get; set; }
    public static Color WindowBg { get; set; }
    public static Color WindowFg { get; set; }
    public static Color GridCellBg { get; set; }
    public static Color GridCellFg { get; set; }
    public static Color GridBg { get; set; }
    public static Color ChecklistedPerkBg { get; set; }
    public static Color InSquadBookendsBg { get; set; }
    public static Color AssaultClassColor { get; set; }
    public static Color EngineerClassColor { get; set; }
    public static Color GunnerClassColor { get; set; }
    public static Color InfantryClassColor { get; set; }
    public static Color MedicClassColor { get; set; }
    public static Color RocketeerClassColor { get; set; }
    public static Color ScoutClassColor { get; set; }
    public static Color ShivClassColor { get; set; }
    public static Color SniperClassColor { get; set; }
    public static Color ClassColorShadow { get; set; }
    public static Color GridSelectedCellBg { get; set; }
    public static Color GridSelectedCellFg { get; set; }
    public static Color TreeHeaderBg { get; set; }
    public static Color TreeTabUnselectedBg { get; set; }
    public static Color TreeHeaderFg { get; set; }
    public static Color UnselectedSoldierPerkBg { get; set; }
    public static Color StatLineFg { get; set; }
    public static Color ChatGemIdle { get; set; }
    public static Color ChatGemActive { get; set; }
    public static Color ChatGemPerfect { get; set; }

    public void GetConfigData(string configFilename)
    {

      XMLConfig = new XmlDocument();
      XMLConfig.Load(configFilename);

      XmlNodeList appNode = XMLConfig.GetElementsByTagName("AppName");
      XmlNodeList versionNode = XMLConfig.GetElementsByTagName("AppVersion");

      Xcom2JsonPath = GetElementInnerXml("Xcom2JsonPath", typeof(string));
      OutputPath = GetElementInnerXml("OutputPath", typeof(string));
      BackupPath = GetElementInnerXml("BackupPath", typeof(string));
      Xcom2JsonHash = GetElementInnerXml("Xcom2JsonCRC64", typeof(ulong));
      XcomSavePath = GetElementInnerXml("XcomSavePath", typeof(string));
      SavesToBackup = GetElementInnerXml("SavesToBackup", typeof(int));
      DeadBg  = GetElementInnerXml("DeadBg", typeof(Color));
      DeadFg  = GetElementInnerXml("DeadFg", typeof(Color));
      WoundBg  = GetElementInnerXml("WoundBg", typeof(Color));
      FatigueBg  = GetElementInnerXml("FatigueBg", typeof(Color));
      ShivBg  = GetElementInnerXml("ShivBg", typeof(Color));
      MaxBg  = GetElementInnerXml("MaxBg", typeof(Color));
      HiBg  = GetElementInnerXml("HiBg", typeof(Color));
      LoBg  = GetElementInnerXml("LoBg", typeof(Color));
      MinBg  = GetElementInnerXml("MinBg", typeof(Color));
      RosterHeaderBg  = GetElementInnerXml("RosterHeaderBg", typeof(Color));
      RosterTabUnselectedBg  = GetElementInnerXml("RosterTabUnselectedBg", typeof(Color));
      RosterHeaderFg  = GetElementInnerXml("RosterHeaderFg", typeof(Color));
      RosterSquadHotTrackBg  = GetElementInnerXml("RosterSquadHotTrackBg", typeof(Color));
      RosterSquadHotTrackFg  = GetElementInnerXml("RosterSquadHotTrackFg", typeof(Color));
      RosterSelectedBookendBg  = GetElementInnerXml("RosterSelectedBookendBg", typeof(Color));
      WoobieBg  = GetElementInnerXml("WoobieBg", typeof(Color));
      WoobieFg  = GetElementInnerXml("WoobieFg", typeof(Color));
      WoobieFg2  = GetElementInnerXml("WoobieFg2", typeof(Color));
      SquadHeaderBg  = GetElementInnerXml("SquadHeaderBg", typeof(Color));
      SquadTabUnselectedBg  = GetElementInnerXml("SquadTabUnselectedBg", typeof(Color));
      SquadHeaderFg  = GetElementInnerXml("SquadHeaderFg", typeof(Color));
      SoldierHeaderBg  = GetElementInnerXml("SoldierHeaderBg", typeof(Color));
      SoldierTabUnselectedBg  = GetElementInnerXml("SoldierTabUnselectedBg", typeof(Color));
      SoldierHeaderFg  = GetElementInnerXml("SoldierHeaderFg", typeof(Color));
      ChecklistBadHeaderBg  = GetElementInnerXml("ChecklistBadHeaderBg", typeof(Color));
      ChecklistBadTabUnselectedBg  = GetElementInnerXml("ChecklistBadTabUnselectedBg", typeof(Color));
      ChecklistBadHeaderFg  = GetElementInnerXml("ChecklistBadHeaderFg", typeof(Color));
      ChecklistBadTabHighlight  = GetElementInnerXml("ChecklistBadTabHighlight", typeof(Color));
      ChecklistGoodHeaderBg  = GetElementInnerXml("ChecklistGoodHeaderBg", typeof(Color));
      ChecklistGoodTabUnselectedBg  = GetElementInnerXml("ChecklistGoodTabUnselectedBg", typeof(Color));
      ChecklistGoodHeaderFg  = GetElementInnerXml("ChecklistGoodHeaderFg", typeof(Color));
      SquadRowBg  = GetElementInnerXml("SquadRowBg", typeof(Color));
      SquadRowFg  = GetElementInnerXml("SquadRowFg", typeof(Color));
      SoldierRowFg  = GetElementInnerXml("SoldierRowFg", typeof(Color));
      WindowTitleBg  = GetElementInnerXml("WindowTitleBg", typeof(Color));
      WindowTitleFg  = GetElementInnerXml("WindowTitleFg", typeof(Color));
      WindowBg  = GetElementInnerXml("WindowBg", typeof(Color));
      WindowFg  = GetElementInnerXml("WindowFg", typeof(Color));
      GridCellBg  = GetElementInnerXml("GridCellBg", typeof(Color));
      GridCellFg  = GetElementInnerXml("GridCellFg", typeof(Color));
      GridBg  = GetElementInnerXml("GridBg", typeof(Color));
      ChecklistedPerkBg  = GetElementInnerXml("ChecklistedPerkBg", typeof(Color));
      InSquadBookendsBg  = GetElementInnerXml("InSquadBookendsBg", typeof(Color));
      AssaultClassColor  = GetElementInnerXml("AssaultClassColor", typeof(Color));
      EngineerClassColor  = GetElementInnerXml("EngineerClassColor", typeof(Color));
      GunnerClassColor  = GetElementInnerXml("GunnerClassColor", typeof(Color));
      InfantryClassColor  = GetElementInnerXml("InfantryClassColor", typeof(Color));
      MedicClassColor  = GetElementInnerXml("MedicClassColor", typeof(Color));
      RocketeerClassColor  = GetElementInnerXml("RocketeerClassColor", typeof(Color));
      ScoutClassColor  = GetElementInnerXml("ScoutClassColor", typeof(Color));
      ShivClassColor  = GetElementInnerXml("ShivClassColor", typeof(Color));
      SniperClassColor  = GetElementInnerXml("SniperClassColor", typeof(Color));
      ClassColorShadow  = GetElementInnerXml("ClassColorShadow", typeof(Color));
      GridSelectedCellBg  = GetElementInnerXml("GridSelectedCellBg", typeof(Color));
      GridSelectedCellFg  = GetElementInnerXml("GridSelectedCellFg", typeof(Color));
      TreeHeaderBg  = GetElementInnerXml("TreeHeaderBg", typeof(Color));
      TreeTabUnselectedBg  = GetElementInnerXml("TreeTabUnselectedBg", typeof(Color));
      TreeHeaderFg  = GetElementInnerXml("TreeHeaderFg", typeof(Color));
      UnselectedSoldierPerkBg  = GetElementInnerXml("UnselectedSoldierPerkBg", typeof(Color));
      StatLineFg  = GetElementInnerXml("StatLineFg", typeof(Color));
      ChatGemIdle  = GetElementInnerXml("ChatGemIdle", typeof(Color));
      ChatGemActive  = GetElementInnerXml("ChatGemActive", typeof(Color));
      ChatGemPerfect = GetElementInnerXml("ChatGemPerfect", typeof(Color));

    }

    public dynamic GetElementInnerXml(string tagName, Type type)
    {
      string innerXml = (XMLConfig.GetElementsByTagName(tagName)[0]).InnerXml;
      if (type == typeof(bool)) { return Convert.ToBoolean(innerXml); }
      else if (type == typeof(double)) { return Convert.ToDouble(innerXml); }
      else if (type == typeof(int)) { return Convert.ToInt32(innerXml); }
      else if (type == typeof(ulong)) { return Convert.ToUInt64(innerXml); }
      else if (type == typeof(Color)) { return ColorTranslator.FromHtml(innerXml); }
      else { return innerXml; }
    }
  }
}
