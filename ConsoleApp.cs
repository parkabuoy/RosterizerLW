using System.Data;
using System.Diagnostics;
using System.IO.Hashing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RosterizerLW
{
  public static class ConsoleApp
  {
    //public static AppProperties _AppProperties;
    //public static AppConfig _AppConfig;
    //public static bool _HasArgs;
    //public static bool _ValidArgs;
    //public static List<string> _Args;
    //public static List<string> ConsoleErrors;
    //public static List<string> ConsoleMsgs;
    //public static List<Soldier> Roster;
    //public static FileInfo SaveFile;
    //public static JsonRoot SaveParsed;
    //public static DataTable PerkList;
    //public static DataTable ChecklistPerksDatatable;
    //public static List<string> PerkNames;
    //public static List<string> SelectedSoldierPerks = [];
    //public static Dictionary<string, int> SquadPerks = [];
    //public static Dictionary<string, int> RosterPerks = [];
    //public static int RecoverableHrs = 8;
    //public static RosterSort[] DefaultSorting = [RosterSort.Rank, RosterSort.Xp];
    //public static RosterSort[] Sorting = DefaultSorting;
    //public static int BlueshirtLvl = 1;
    //public static long[] XpLvls = [120, 350, 700, 1200, 2000, 3000, 4200]; // xp levels per DefaultGameCore.ini ~ln. 900
    //public static Dictionary<string, int> ChecklistPerks = [];
    //public static bool ChecklistPass = false;
    //public static bool NavFromChecklist = false;

    [STAThread]
    static void Main()
   {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      //ApplicationConfiguration.Initialize();

      // --------------------------------------------------------------------------------------------------------------------------------------------- DA CONFIG ZONE
      // *not the only config zone apparently
      //string outputDir = "..\\..\\..\\output\\"; // the output dir
      //string backupDir = "..\\..\\..\\saveBackup\\"; // path where saves will be backed up
      //string x2jPath = "..\\..\\..\\exe\\xcom2json.exe"; // the path to xcom2json.exe, CRC 
      //
      //UInt64 x2jHash = 550290084522337840; // ensure the expected xcom2json version
      //
      //// your xcom save directory
      //string savePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\XCOM - Enemy Within\\XComGame\\SaveData";
      //
      //// how many of the most recent saves will be backed up on running
      //int savesToBackup = 3;
      //// ------------------------------------------------------------------------------------------------------------------------------------------------------------
      //
      //string saveFilenameFull = "";
      //string saveFilename = "";
      //string jsonFilenameFull = "";
      //string saveNameRegex = "^save\\d{1,3}\\Z"; // regex: starts with "save", has 1-3 numbers after it, then ends
      //string todayBackupDir = Path.Combine(backupDir, $"{DateTime.Now:yyyyMMdd}");
      //string todayOutputDir = Path.Combine(outputDir, $"{DateTime.Now:yyyyMMdd}");
      //
      //string execTime = $"{DateTime.Now:yyyyMMdd.HHmm}";
      //
      //FileInfo x2jFile = new(x2jPath);
      //UInt64 hash = Crc64.HashToUInt64(File.ReadAllBytes(x2jPath));


      //Console.WriteLine("Save directory:");
      //Console.WriteLine(savePath);
      //Console.Write("Use this dir? y/n: ");

      // if not 'y', asks for a file/directory
      //if (Console.ReadKey().KeyChar != 'y')
      //{
      //  Console.WriteLine();
      //  Console.WriteLine("Input save dir/file:");
      //  savePath = (Console.ReadLine() ?? "").Replace("\"", "");
      //}

      /////if (Directory.Exists(savePath))
      /////{
      /////  foreach (FileInfo saveFile in new DirectoryInfo(savePath)
      /////    .GetFiles()
      /////    .Where(x => Regex.IsMatch(x.Name, saveNameRegex)) // match "save[123]" regex
      /////    .OrderByDescending(x => x.LastWriteTime) // get the most recent files
      /////    .Take(savesToBackup)) // only a few
      /////  {
      /////    saveForAnalysis ??= saveFile; // pluck first one (most recent) for analysis (assign to it if it's null)
      /////    BackupFile(saveFile); // back files up
      /////  }
      /////}
      /////else if (File.Exists(savePath))
      /////{
      //saveForAnalysis = new(savePath);
      

      // output the results
      //OutputTSV(fullOutputPath, Roster);

      //Console.WriteLine();
      //Console.WriteLine($"Output saved to: ");
      //Console.WriteLine(fullOutputPath);
      //Console.WriteLine("Press any key to exit...");
      //Console.ReadKey();
      //Environment.Exit(0);

      try
      {
        if (Application.OpenForms.Count >= 1)
        {
          for (int i = 0; i < Application.OpenForms.Count; i++)
          {
            Application.OpenForms[i].Close();
          }
        }
        Application.Run(new Rosterizer());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
