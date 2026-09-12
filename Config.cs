using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;

namespace RosterizerLW
{
  [SupportedOSPlatform("windows")] // shut up warning CA1416
  #pragma warning disable CS8602 
  public class AppConfig
  {
    public static string Xcom2JsonPath { get; set; }
    public static string OutputPath { get; set; }
    public static string BackupPath { get; set; }
    public static Int64 Xcom2JsonHash { get; set; }
    public static string XcomSavePath { get; set; }
    public static int SavesToBackup { get; set; }
    public XmlDocument XMLConfig { get; set; }
    public XmlNodeList appNode { get; set; }
    public XmlNodeList versionNode { get; set; }

    public void GetConfigData(string configFilename)
    {

      XMLConfig = new XmlDocument();
      XMLConfig.Load(configFilename);

      XmlNodeList appNode = XMLConfig.GetElementsByTagName("AppName");
      XmlNodeList versionNode = XMLConfig.GetElementsByTagName("AppVersion");

      Xcom2JsonPath = GetElementInnerXml("Xcom2JsonPath", typeof(string));
      OutputPath = GetElementInnerXml("OutputPath", typeof(string));
      BackupPath = GetElementInnerXml("BackupPath", typeof(string));
      Xcom2JsonHash = GetElementInnerXml("Xcom2JsonHash", typeof(Int64));
      XcomSavePath = GetElementInnerXml("XcomSavePath", typeof(string));
      SavesToBackup = GetElementInnerXml("SavesToBackup", typeof(int));
    }

    public dynamic GetElementInnerXml(string tagName, Type type)
    {
        string innerXml = (XMLConfig.GetElementsByTagName(tagName)[0]).InnerXml;
        if (type == typeof(bool)) { return Convert.ToBoolean(innerXml); }
        else if (type == typeof(double)) { return Convert.ToDouble(innerXml); }
        else if (type == typeof(int)) { return Convert.ToInt32(innerXml); }
        else { return innerXml; }
    }
  }
}
