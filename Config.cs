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

    public void GetConfigData()
    {
      Xcom2JsonPath = GetElementInnerXml("Xcom2JsonPath", typeof(string));
      OutputPath = GetElementInnerXml("OutputPath", typeof(string));
      BackupPath = GetElementInnerXml("BackupPath", typeof(string));
      Xcom2JsonHash = GetElementInnerXml("Xcom2JsonHash", typeof(Int64));
      XcomSavePath = GetElementInnerXml("XcomSavePath", typeof(string));
      SavesToBackup = GetElementInnerXml("SavesToBackup", typeof(int));
    }

    public dynamic GetElementInnerXml(string tagName, Type type)
    {
      try
      {
        string innerXml = (ConsoleApp._AppProperties.xmlConfig.GetElementsByTagName(tagName)[0]).InnerXml;

        if (type == typeof(bool)) { return Convert.ToBoolean(innerXml); }
        else if (type == typeof(double)) { return Convert.ToDouble(innerXml); }
        else if (type == typeof(int)) { return Convert.ToInt32(innerXml); }
        else { return innerXml; }
      }
      catch (Exception ex)
      {
        ConsoleApp._AppProperties.Logger.WriteException(ConsoleApp._AppProperties, ex, ConsoleApp._AppProperties.ShortTimeStamp(), $"Error loading {type.Name} \'{tagName}\' from config XML. Application could not run",
            true, typeof(AppConfig).Name, new StackTrace().GetFrame(0).GetMethod().Name);
        throw;
      }
    }
  }

  public class AppProperties
  {
    public string AppName;
    public string AppVersion;
    public string AppClass;
    public string AppMethod;
    public XmlDocument xmlConfig;
    public ILogger Logger { get; set; }

    public AppProperties(string filename)
    {
      this.xmlConfig = new XmlDocument();
      this.xmlConfig.Load(@filename);

      XmlNodeList AppNode = this.xmlConfig.GetElementsByTagName("AppName");
      XmlNodeList VersionNode = this.xmlConfig.GetElementsByTagName("AppVersion");

      this.AppName = AppNode[0].InnerXml;
      this.AppVersion = VersionNode[0].InnerXml;
      this.AppClass = "";
      this.AppMethod = "";
      this.Logger = new TextLogger(Assembly.GetExecutingAssembly().GetName().Name + "_Errors.txt");
    }

    public static string AppDate()
    {
      return string.Format("{0:yyyy-MM-dd}", DateTime.Now);
    }

    public static string AppTime()
    {
      return string.Format("{0:hh-mm-ss}", DateTime.Now);
    }

    /* Returns a ISO Timestamp without milliseconds
     *Format: "yyyy-mm-dd hh:mm:ss"*/
    public string ShortTimeStamp()
    {
      return string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
    }

    /* Returns a ISO Timestamp with milliseconds
    *Format: "yyyy-mm-dd hh:mm:ss.mmm"*/
    public string LongTimeStamp()
    {
      return string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", DateTime.Now);
    }


    public interface ILogger
    {
      void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message);
      void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical);
      void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical, string appClass, string appMethod);
    }

    public class TextLogger : ILogger
    {
      private readonly string _errorLogFileName;

      public TextLogger(string errorLogFileName)
      {
        _errorLogFileName = errorLogFileName;
      }

      public void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message)
      {
        WriteException(myConfig, ex, timeStamp, message, false, myConfig.AppClass, myConfig.AppMethod);

      }

      public void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical)
      {
        WriteException(myConfig, ex, timeStamp, message, isCritical, myConfig.AppClass, myConfig.AppMethod);
      }

      public void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical, string appClass, string appMethod)
      {
        StringBuilder builder;

        builder = new StringBuilder();

        builder.Append("AppName: ").Append(myConfig.AppName).Append("\r\n");
        builder.Append("AppVersion: ").Append(myConfig.AppVersion).Append("\r\n");
        builder.Append("AppClass: ").Append(appClass).Append("\r\n");
        builder.Append("AppMethod: ").Append(appMethod).Append("\r\n");
        builder.Append("ErrorTimestamp: ").Append(timeStamp).Append("\r\n");
        builder.Append("ExceptionMessage: ").Append(ex.Message).Append("\r\n");
        builder.Append("AppErrorMessage: ").Append(message).Append("\r\n");
        builder.Append("IsCritical: ").Append(isCritical).Append("\r\n").Append("\r\n");

        File.AppendAllText(_errorLogFileName, builder.ToString());
      }
    }
  }

  public interface ILogger
  {
    void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message);
    void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical);
    void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical, string appClass, string appMethod);
  }

  public class TextLogger : ILogger
  {
    private readonly string _errorLogFileName;

    public TextLogger(string errorLogFileName)
    {
      _errorLogFileName = errorLogFileName;
    }

    public void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message)
    {
      WriteException(myConfig, ex, timeStamp, message, false, myConfig.AppClass, myConfig.AppMethod);

    }

    public void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical)
    {
      WriteException(myConfig, ex, timeStamp, message, isCritical, myConfig.AppClass, myConfig.AppMethod);
    }

    public void WriteException(AppProperties myConfig, Exception ex, string timeStamp, string message, bool isCritical, string appClass, string appMethod)
    {
      StringBuilder builder;

      builder = new StringBuilder();

      builder.Append("AppName: ").Append(myConfig.AppName).Append("\r\n");
      builder.Append("AppVersion: ").Append(myConfig.AppVersion).Append("\r\n");
      builder.Append("AppClass: ").Append(appClass).Append("\r\n");
      builder.Append("AppMethod: ").Append(appMethod).Append("\r\n");
      builder.Append("ErrorTimestamp: ").Append(timeStamp).Append("\r\n");
      builder.Append("ExceptionMessage: ").Append(ex.Message).Append("\r\n");
      builder.Append("AppErrorMessage: ").Append(message).Append("\r\n");
      builder.Append("IsCritical: ").Append(isCritical).Append("\r\n").Append("\r\n");

      File.AppendAllText(_errorLogFileName, builder.ToString());
    }
  }
}
