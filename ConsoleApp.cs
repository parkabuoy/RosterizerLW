using System.Data;
using System.Diagnostics;
using System.IO.Hashing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RosterizerLW
{
  public static class ConsoleApp
  {
    public static AppProperties _AppProperties;
    public static AppConfig _AppConfig;
    public static bool _HasArgs;
    public static bool _ValidArgs;
    public static List<string> _Args;
    public static List<string> ConsoleErrors;
    public static List<string> ConsoleMsgs;
    public static List<Soldier> Roster;
    public static FileInfo SaveFile;
    public static JsonRoot SaveParsed;
    public static DataTable PerkList;
    public static DataTable ChecklistPerksDatatable;
    public static List<string> PerkNames;
    public static List<string> SelectedSoldierPerks = [];
    public static Dictionary<string, int> SquadPerks = [];
    public static Dictionary<string, int> RosterPerks = [];
    public static int RecoverableHrs = 8;
    public static RosterSort[] DefaultSorting = [RosterSort.Rank, RosterSort.Xp];
    public static RosterSort[] Sorting = DefaultSorting;
    public static int BlueshirtLvl = 1;
    public static long[] XpLvls = [120, 350, 700, 1200, 2000, 3000, 4200]; // xp levels per DefaultGameCore.ini ~ln. 900
    public static Dictionary<string, int> ChecklistPerks = [];
    public static bool ChecklistPass = false;
    public static bool NavFromChecklist = false;

    [STAThread]
    static void Main()
   {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      //ApplicationConfiguration.Initialize();

      // --------------------------------------------------------------------------------------------------------------------------------------------- DA CONFIG ZONE
      // *not the only config zone apparently
      string outputDir = "..\\..\\..\\output\\"; // the output dir
      string backupDir = "..\\..\\..\\saveBackup\\"; // path where saves will be backed up
      string x2jPath = "..\\..\\..\\exe\\xcom2json.exe"; // the path to xcom2json.exe, CRC 

      UInt64 x2jHash = 550290084522337840; // ensure the expected xcom2json version

      // your xcom save directory
      string savePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\XCOM - Enemy Within\\XComGame\\SaveData";

      // how many of the most recent saves will be backed up on running
      int savesToBackup = 3;
      // ------------------------------------------------------------------------------------------------------------------------------------------------------------

      string saveFilenameFull = "";
      string saveFilename = "";
      string jsonFilenameFull = "";
      string saveNameRegex = "^save\\d{1,3}\\Z"; // regex: starts with "save", has 1-3 numbers after it, then ends
      string todayBackupDir = Path.Combine(backupDir, $"{DateTime.Now:yyyyMMdd}");
      string todayOutputDir = Path.Combine(outputDir, $"{DateTime.Now:yyyyMMdd}");

      string execTime = $"{DateTime.Now:yyyyMMdd.HHmm}";

      FileInfo x2jFile = new(x2jPath);
      UInt64 hash = Crc64.HashToUInt64(File.ReadAllBytes(x2jPath));

      if (!x2jFile.Exists || hash != x2jHash) ShowError("invalid xcom2json exe!", x2jFile.FullName);

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
      SaveFile ??= new("..\\..\\..\\saveBackup\\save43");
      // build datatable out of csv file (directly copied from swf's id reference sheets)
      PerkList ??= ConvertCSVtoDataTable("..\\..\\..\\csv\\Long War ID reference - Perks.csv");
      ChecklistPerksDatatable ??= ConvertCSVtoDataTable("..\\..\\..\\csv\\Checklist - Perks.csv");

      //BackupFile(SaveFile);
      /////}
      /////else ShowError("dir/file not found:", savePath);

      saveFilenameFull = (SaveFile ?? new("")).FullName;
      saveFilename = (SaveFile ?? new("")).Name;

      // build full filename of output json
      if (!Directory.Exists(todayOutputDir)) Directory.CreateDirectory(todayOutputDir);
      jsonFilenameFull = Path.Combine(todayOutputDir, $"{execTime}.{saveFilename}.json");

      // run xcom2json exe on save file
      Process.Start("cmd", $"/C {x2jPath} -o \"{jsonFilenameFull}\" \"{saveFilenameFull}\"").WaitForExit();

      // if parsing failed, cry
      if (!File.Exists(jsonFilenameFull)) ShowError("json parsing failure!", saveFilenameFull);

      string outputLedger = "";
      string fullOutputPath = Path.Combine(todayOutputDir, $"{execTime}.{saveFilename}.tsv");

      PerkNames ??= [];

      for (int i = 0; i < PerkList.Rows.Count; i++)
      {
        if (PerkList.Rows[i]["Enabled"].ToString() == "1")
        {
          string perkName = PerkList.Rows[i]["Name"].ToString() ?? "";
          if (!string.IsNullOrWhiteSpace(perkName)) PerkNames.Add(perkName);
        }
      }

      for (int i = 0; i < ChecklistPerksDatatable.Rows.Count; i++)
      {
        if (ChecklistPerksDatatable.Rows[i]["Enabled"].ToString() == "1")
        {
          string perkName = ChecklistPerksDatatable.Rows[i]["Name"].ToString() ?? "";
          if (!string.IsNullOrWhiteSpace(perkName)) ChecklistPerks.Add(perkName,0);
        }
      }

      string rawJson = File.ReadAllText(jsonFilenameFull);
      SaveParsed = JsonConvert.DeserializeObject<JsonRoot>(rawJson) ?? new() { Actor_table = [], Checkpoints = [], Header = new() };
      Roster = [.. FillRoster(SaveParsed).OrderByDescending(x => x.Xp)];
      SelectedSoldierPerks = [];

      // output the results
      //OutputTSV(fullOutputPath, Roster);

      //Console.WriteLine();
      //Console.WriteLine($"Output saved to: ");
      //Console.WriteLine(fullOutputPath);
      //Console.WriteLine("Press any key to exit...");
      //Console.ReadKey();
      //Environment.Exit(0);

      // back files up
      void BackupFile(FileInfo saveFile)
      {
        if (!Directory.Exists(todayBackupDir)) Directory.CreateDirectory(todayBackupDir);
        saveFile.CopyTo(Path.Combine(todayBackupDir, $"{execTime}.{saveFile.Name}"), overwrite: true);
      }

    // do this with an enum
    static string RankMap(long rankId)
    {
        return rankId switch
      {
        -1 => "SHIV",
        0 => "SQ",
        1 => "SPC",
        2 => "LCPL",
        3 => "CPL",
        4 => "SGT",
        5 => "TSGT",
        6 => "GSGT",
        7 => "MSGT",
        _ => "",
      };
    }

    void OutputTSV(string outPath, List<Soldier> roster)
      {
        string perkNames = "";

        // build a tab-separated string of all perk names by iterating through perk reference table
        for (int i = 0; i < PerkList.Rows.Count; i++) perkNames += $"\t{PerkList.Rows[i]["Name"]}";

        // write header row
        Show(string.Join("\t", ["id"
          ,"lName"
          ,"nName"
          ,"rank"
          ,"status"
          ,"MOB"
          ,"HP"
          ,"DEF"
          ,"WILL"
          ,"AIM"
          ,perkNames[0..]
        ]));

        // build soldier lines - iterate through roster
        foreach (Soldier thisSoldier in roster.Where(x => x.Status != "Dead" && !string.IsNullOrEmpty(x.LName)))
        {
          string perkFlags = "";

          // iterate through perk ref list and set this soldier's value for each perk
          for (int i = 0; i < PerkList.Rows.Count; i++)
          {
            perkFlags += "\t";
            foreach (Perk thisPerk in thisSoldier.Perks.Where(x => x.Id == Int64.Parse(PerkList.Rows[i]["ID"].ToString() ?? ""))) perkFlags += thisPerk.Type;
          }

          // write the line for this soldier
          Show(string.Join("\t", [
            thisSoldier.Id
            ,thisSoldier.LName
            ,thisSoldier.NName
            ,thisSoldier.RankId
            ,thisSoldier.Status
            ,thisSoldier.Stats.Mobility
            ,thisSoldier.Stats.HP
            ,thisSoldier.Stats.Defense
            ,thisSoldier.Stats.Will
            ,thisSoldier.Stats.Aim
            ,perkFlags[1..]
          ]));
        }

        // write file out
        File.WriteAllText(outPath, outputLedger);
      }

      // deserialize a string property
      static string StringProp(Property prop, string name)
      {
        foreach (Property stringProp in (prop.Properties ?? []).Where(x => x.Name == name))
        {
          return (JsonConvert.DeserializeObject<XValue>((stringProp.Value ?? "").ToString() ?? "") ?? new XValue()).Str ?? "";
        }

        return "";
      }

      // deserialize a long property
      static long? LongProp(Property prop, string name)
      {
        if ((prop.Properties ?? []).Exists(x => x.Name == name))
          return (long)((prop.Properties ?? []).First(x => x.Name == name).Value ?? "-1");
        else return null;
      }

      // output to console and global string so it can be saved to a file
      void Show(string line)
      {
        ConsoleMsgs ??= [];
        ConsoleMsgs.Add(line);
        outputLedger += line + Environment.NewLine;
      }

      // output an error message
      void ShowError(string msg1, string msg2, bool anyKey = true)
      {
        ConsoleErrors ??= [];
        ConsoleErrors.Add(msg1);
        ConsoleErrors.Add(msg2);
      }

      // converts a csv to a data table :)
      static DataTable ConvertCSVtoDataTable(string strFilePath)
      {
        DataTable dt = new();
        using (StreamReader sr = new(strFilePath))
        {
          string[] headers = (sr.ReadLine() ?? "").Split(',');
          foreach (string header in headers)
          {
            dt.Columns.Add(header);
          }
          while (!sr.EndOfStream)
          {
            string[] rows = (sr.ReadLine() ?? "").Split(',');
            DataRow dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
              dr[i] = rows[i];
            }
            dt.Rows.Add(dr);
          }
        }
        return dt;
      }

      static List<Soldier> FillRoster(JsonRoot saveJson)
      {
        List<Soldier> roster = [];

        //----------------------------------------------------------------------------- mapping
        // in parsed json, step through the parts which relate to soldiers
        foreach (CheckpointTable entity in ((saveJson).Checkpoints[0].Checkpoint_table ?? []).Where(x => x.Class_name == "XComStrategyGame.XGStrategySoldier"))
        {
          if (entity.Properties is not null)
          {
            // get soldier/character property arrays
            // properties both contain values (name, etc) and lists of more properties
            Property soldierProp = entity.Properties.Where(x => x.Name == "m_kSoldier").First();
            Property charProp = entity.Properties.Where(x => x.Name == "m_kChar").First();
            Property classProp = ((soldierProp ?? new()).Properties ?? []).Where(x => x.Name == "kClass").First();
            Property fatigueProp = entity.Properties.Where(x => x.Name == "m_iTurnsOut").First();

            if (soldierProp.Properties is not null)
            {
              // build soldier
              Soldier thisSoldier = new()
              {
                Id = (long)(soldierProp.Properties.First(x => x.Name == "iID").Value ?? -1),
                Perks = [],
                Stats = new(),
                // use helpers to make this a little cleaner
                LName = StringProp(soldierProp, "strLastName"),
                NName = StringProp(soldierProp, "strNickName"),
                FName = StringProp(soldierProp, "strFirstName"),
                RankId = LongProp(soldierProp, "iRank").GetValueOrDefault(),
                RankName = "",
                Xp = LongProp(soldierProp, "iXP").GetValueOrDefault(),
                Class = ((JObject)(classProp.Properties.First(x => x.Name == "strName").Value)).First.First.ToString().Trim("{}".ToCharArray()),
                // status is in the parent entity
                Status = ((entity.Properties.First(x => x.Name == "m_eStatus").Value ?? "").ToString() ?? "").TrimStart("eStatus_".ToCharArray()),
                IsDead = false,
                IsBlueshirt = false,
                HoursOut = ((long)fatigueProp.Value),
                IsShiv = false,
                IsWounded = false,
                IsFatigued = false,
                InSquad = false,
                HasChecklistPerk = false,
              };
              thisSoldier.IsDead = thisSoldier.Status == "Dead";
              thisSoldier.IsShiv = thisSoldier.RankId == -1;
              thisSoldier.IsWounded = (entity.Properties.FirstOrDefault(x => x.Name == "m_eStatus" && (string?)x.Value == "eStatus_Healing", new()).Number ?? -1) == 0;
              thisSoldier.IsFatigued = thisSoldier.HoursOut > 0 && !thisSoldier.IsWounded;
              thisSoldier.IsBlueshirt = !thisSoldier.IsShiv && thisSoldier.RankId <= BlueshirtLvl;
              thisSoldier.RankName = RankMap(thisSoldier.RankId);

              // get the perks taken
              // these are stored as an array of integers in aUpgrades, 176 of them (one per perk)
              int[] aUpgrades = [];
              if ((charProp.Properties ?? []).Any(x => x.Name == "aUpgrades"))
              {
                // get the upgrades array and walk through it
                aUpgrades = [.. (charProp.Properties ?? []).First(x => x.Name == "aUpgrades").Int_values ?? []];
                for (int i = 0; i < aUpgrades.Length; i++)
                {
                  // if the array value is 0, they don't have that perk
                  if (aUpgrades[i] > 0)
                  {
                    // step through the perk reference sheet until we find the perk which matches this index in the aUpgrades array
                    foreach (DataRow row in PerkList.Rows)
                    {
                      if (row["Enabled"].ToString() == "1" && Int32.Parse(row["ID"].ToString() ?? "") == i)
                      {
                        thisSoldier.Perks.Add(new()
                        {
                          Id = i,
                          Name = row["Name"].ToString() ?? "",
                          // 1 is a chosen perk, 2 and 3 are something else apparently. medals?
                          Type = aUpgrades[i]
                        });

                        break;
                      }
                    }
                  }
                }
              }

              // get stats
              int[] aStats = [];
              if ((charProp.Properties ?? []).Any(x => x.Name == "aStats"))
              {
                // int values stored in an array whose index corresponds to hp, will, etc
                aStats = [.. (charProp.Properties ?? []).First(x => x.Name == "aStats").Int_values ?? []];
                for (int statIndex = 0; statIndex < aStats.Length; statIndex++)
                {
                  switch (statIndex)
                  {
                    case 0:
                      thisSoldier.Stats.HP = aStats[statIndex];
                      break;
                    case 1:
                      thisSoldier.Stats.Aim = aStats[statIndex];
                      break;
                    case 2:
                      thisSoldier.Stats.Defense = aStats[statIndex];
                      break;
                    case 3:
                      thisSoldier.Stats.Mobility = aStats[statIndex];
                      break;
                    case 7:
                      thisSoldier.Stats.Will = aStats[statIndex];
                      break;
                  }
                }
              }

              // add the soldier to the roster
              roster.Add(thisSoldier);
            }
          }
        }
        
        // add 0-soldier perks to roster perks list
        foreach (DataRow row in PerkList.Rows) if (row["Enabled"].ToString() == "1") RosterPerks.TryAdd(row["Name"].ToString() ?? "", 0);
        return roster;
      }
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
        int k = 0;
      }
    }
    public static void Reinitialize(FileInfo saveInput)
    {
      SaveFile = saveInput;
      Roster = [];
      Main();
    }
  }
}
