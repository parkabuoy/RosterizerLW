using System.Data;
using System.Diagnostics;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RosterizerLW
{
  public partial class Rosterizer : Form
  {
    // muted color scheme
    static readonly Color deadBg = Color.FromArgb(144, 87, 97);
    static readonly Color deadFg = Color.FromArgb(255, 255, 255);
    static readonly Color woundBg = Color.FromArgb(182, 145, 152);
    static readonly Color woundStatBg = Color.FromArgb(212, 165, 172);
    static readonly Color fatigueBg = Color.FromArgb(194, 194, 194);
    static readonly Color shivBg = Color.FromArgb(157, 157, 157);
    static readonly Color maxBg = Color.FromArgb(137, 177, 170);
    static readonly Color hiBg = Color.FromArgb(172, 200, 195);
    static readonly Color loBg = Color.FromArgb(205, 180, 185);
    static readonly Color minBg = Color.FromArgb(184, 148, 155);
    static readonly Color rosterHeaderBg = Color.FromArgb(58, 92, 109);
    static readonly Color rosterTabUnselectedBg = Color.FromArgb(118, 152, 169);
    static readonly Color rosterHeaderFg = Color.FromArgb(222, 222, 222);
    static readonly Color squadHeaderBg = Color.FromArgb(134, 134, 134);
    static readonly Color squadTabUnselectedBg = Color.FromArgb(164, 164, 164);
    static readonly Color squadHeaderFg = Color.FromArgb(222, 222, 222);
    static readonly Color soldierHeaderBg = Color.FromArgb(122, 114, 140);
    static readonly Color soldierTabUnselectedBg = Color.FromArgb(153, 143, 176);
    static readonly Color soldierHeaderFg = Color.FromArgb(222, 222, 222);
    static readonly Color checklistBadHeaderBg = Color.FromArgb(204, 153, 60);
    static readonly Color checklistBadTabUnselectedBg = Color.FromArgb(255, 192, 76);
    static readonly Color checklistBadHeaderFg = Color.FromArgb(102, 76, 30);
    static readonly Color checklistBadTabHighlight = Color.FromArgb(246, 178, 101);
    static readonly Color checklistGoodHeaderBg = Color.FromArgb(95, 143, 110);
    static readonly Color checklistGoodTabUnselectedBg = Color.FromArgb(143, 176, 153);
    static readonly Color checklistGoodHeaderFg = Color.FromArgb(222, 255, 222);
    static readonly Color squadRowBg = Color.FromArgb(194, 194, 194);
    static readonly Color squadRowFg = Color.FromArgb(0, 0, 0);
    static readonly Color soldierRowBg = Color.FromArgb(199, 158, 182);
    static readonly Color soldierRowFg = Color.FromArgb(30, 30, 30);
    static readonly Color windowTitleBg = Color.FromArgb(69, 111, 132);
    static readonly Color windowTitleFg = Color.FromArgb(222, 222, 222);
    static readonly Color windowBg = Color.FromArgb(222, 222, 222);
    static readonly Color windowFg = Color.FromArgb(0, 0, 0);
    static readonly Color gridCellBg = Color.FromArgb(222, 222, 222);
    static readonly Color gridCellFg = Color.FromArgb(0, 0, 0);
    static readonly Color gridSelectedCellBg = Color.FromArgb(118, 152, 169);
    static readonly Color gridSelectedCellFg = Color.FromArgb(0, 0, 0);
    static readonly bool darkMode = false;
    static readonly DataGridViewCellBorderStyle rosterCellBorders = DataGridViewCellBorderStyle.Raised;
    static readonly DataGridViewCellBorderStyle squadCellBorders = DataGridViewCellBorderStyle.Raised;
    static readonly DataGridViewCellBorderStyle soldierCellBorders = DataGridViewCellBorderStyle.Raised;

    /*
        // muted color scheme
    static readonly Color deadBg = Color.FromArgb(144, 87, 97);
    static readonly Color deadFg = Color.FromArgb(255, 255, 255);
    static readonly Color woundBg = Color.FromArgb(182, 145, 152);
    static readonly Color fatigueBg = Color.FromArgb(194, 194, 194);
    static readonly Color shivBg = Color.FromArgb(157, 157, 157);
    static readonly Color maxBg = Color.FromArgb(137, 177, 170);
    static readonly Color hiBg = Color.FromArgb(172, 200, 195);
    static readonly Color loBg = Color.FromArgb(205, 180, 185);
    static readonly Color minBg = Color.FromArgb(184, 148, 155);
    static readonly Color rosterHeaderBg = Color.FromArgb(58, 92, 109);
    static readonly Color rosterTabUnselectedBg = Color.FromArgb(118, 152, 169);
    static readonly Color rosterHeaderFg = Color.FromArgb(222, 222, 222);
    static readonly Color squadHeaderBg = Color.FromArgb(134, 134, 134);
    static readonly Color squadTabUnselectedBg = Color.FromArgb(164, 164, 164);
    static readonly Color squadHeaderFg = Color.FromArgb(222, 222, 222);
    static readonly Color soldierHeaderBg = Color.FromArgb(122, 114, 140);
    static readonly Color soldierTabUnselectedBg = Color.FromArgb(153, 143, 176);
    static readonly Color soldierHeaderFg = Color.FromArgb(222, 222, 222);
    static readonly Color checklistBadHeaderBg = Color.FromArgb(204, 153, 60);
    static readonly Color checklistBadTabUnselectedBg = Color.FromArgb(255, 192, 76);
    static readonly Color checklistBadHeaderFg = Color.FromArgb(102, 76, 30);
    static readonly Color checklistBadTabHighlight = Color.FromArgb(246, 178, 101);
    static readonly Color checklistGoodHeaderBg = Color.FromArgb(95, 143, 110);
    static readonly Color checklistGoodTabUnselectedBg = Color.FromArgb(143, 176, 153);
    static readonly Color checklistGoodHeaderFg = Color.FromArgb(222, 255, 222);
    static readonly Color squadRowBg = Color.FromArgb(194, 194, 194);
    static readonly Color squadRowFg = Color.FromArgb(0, 0, 0);
    static readonly Color soldierRowBg = Color.FromArgb(199, 158, 182);
    static readonly Color soldierRowFg = Color.FromArgb(30, 30, 30);
    static readonly Color windowTitleBg = Color.FromArgb(69, 111, 132);
    static readonly Color windowTitleFg = Color.FromArgb(222, 222, 222);
    static readonly Color windowBg = Color.FromArgb(222, 222, 222);
    static readonly Color windowFg = Color.FromArgb(0, 0, 0);
    static readonly Color gridCellBg = Color.FromArgb(222, 222, 222);
    static readonly Color gridCellFg = Color.FromArgb(0, 0, 0);
    static readonly Color gridSelectedCellBg = Color.FromArgb(118, 152, 169);
    static readonly Color gridSelectedCellFg = Color.FromArgb(0, 0, 0);
    static readonly bool darkMode = false;
    static readonly DataGridViewCellBorderStyle rosterCellBorders = DataGridViewCellBorderStyle.Raised;
    static readonly DataGridViewCellBorderStyle squadCellBorders = DataGridViewCellBorderStyle.Raised;
    static readonly DataGridViewCellBorderStyle soldierCellBorders = DataGridViewCellBorderStyle.Raised;
    */
    private Stopwatch timer2 = new();
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private bool timerRunning = false;

    // these change the relative count of high/low numbers highlighted (other than min/max) for the various stats (multiple since i.e. percentage spread on def will be lower than aim/will)
    // -- or to put it another way, too much green/red? make these lower
    static  double indPctMobWill = 0.22;
    static  double indPctAim = 0.3;
    static  double indPctHp = 0.5;
    static double indPctDef = 0.9;

    static readonly int minHiLoListSize = 7;
    static readonly int minMinMaxListSize = 4;

    bool maxHp = false, hiHp = false, loHp = false, minHp = false;
    bool maxMob = false, hiMob = false, loMob = false, minMob = false;
    bool maxAim = false, hiAim = false, loAim = false, minAim = false;
    bool maxWill = false, hiWill = false, loWill = false, minWill = false;
    bool maxDef = false, hiDef = false, loDef = false, minDef = false;

    public static AppConfig _AppConfig;
    public static bool _HasArgs;
    public static bool _ValidArgs;
    public static List<string> _Args;
    public static List<string> ConsoleErrors;
    public static List<string> ConsoleMsgs;
    public static List<Soldier> Roster;
    //public static FileInfo SaveFile;
    public static JsonRoot SaveParsed;
    public static DataTable PerkList;
    public static DataTable ChecklistPerksDatatable;
    public static List<string> PerkNames;
    public static List<string> SelectedSoldierPerks = [];
    public static Dictionary<string, int> SquadPerks = [];
    public static Dictionary<string, int> RosterPerks = [];
    public static int RecoverableHrs = 8;
    public static int BlueshirtLvl = 2;
    public static long[] XpLvls = [120, 350, 700, 1200, 2000, 3000, 4200]; // xp levels per DefaultGameCore.ini ~ln. 900
    public static Dictionary<string, int> ChecklistPerks = [];
    public static bool ChecklistPass = false;
    public static long ChecklistPassTime = 0;
    public static bool NavFromChecklist = false;

    public static List<List<int>> SortArray = [[]];
    public static int[] DefaultSortArray = [0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0];
    public static int MaxSortDepth = 5;
    static string overrideSavePath = "..\\..\\..\\saveBackup\\save43";
    static string outputDir = "..\\..\\..\\output\\"; // the output dir
    static string backupDir = "..\\..\\..\\saveBackup\\"; // path where saves will be backed up
    static string x2jPath = "..\\..\\..\\exe\\xcom2json.exe"; // the path to xcom2json.exe, CRC 
    static string perkListPath = "..\\..\\..\\csv\\Long War ID reference - Perks.csv";
    static string perkChecklistPath = "..\\..\\..\\csv\\Checklist - Perks.csv";
    public static FileInfo saveFile;

    UInt64 x2jHash = 550290084522337840; // ensure the expected xcom2json version

    // xcom save dir from which the most recent save file will be selected
    string autoSavePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\XCOM - Enemy Within\\XComGame\\SaveData";

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

    public Rosterizer()
    {
      InitializeComponent();

      if (!x2jFile.Exists || hash != x2jHash) ShowError("invalid xcom2json exe!", x2jFile.FullName);

      saveFile ??= new(overrideSavePath);
      // build datatable out of csv file (directly copied from swf's id reference sheets)
      PerkList ??= ConvertCSVtoDataTable(perkListPath);
      ChecklistPerksDatatable ??= ConvertCSVtoDataTable(perkChecklistPath);

      if (!saveFile.Exists)
      {
        if (Directory.Exists(autoSavePath))
        {
          foreach (FileInfo saveFile in new DirectoryInfo(autoSavePath)
            .GetFiles()
            .Where(x => Regex.IsMatch(x.Name, saveNameRegex)) // match "save[123]" regex
            .OrderByDescending(x => x.LastWriteTime)) // get the most recent files
          {
            Rosterizer.saveFile ??= saveFile; // pluck first one (most recent) for analysis (assign to it if it's null)
            break; // only want one
          }
        }
        else ShowError("dir/file not found:", autoSavePath);
      }

      BackupFile(saveFile); // back files up

      saveFilenameFull = (saveFile ?? new("")).FullName;
      saveFilename = (saveFile ?? new("")).Name;

      // build full filename of output json
      if (!Directory.Exists(todayOutputDir)) Directory.CreateDirectory(todayOutputDir);

      jsonFilenameFull = Path.Combine(todayOutputDir, $"{execTime}.{saveFilename}.json");

      // run xcom2json exe on save file
      Process.Start("cmd", $"/C {x2jPath} -o \"{jsonFilenameFull}\" \"{saveFilenameFull}\"").WaitForExit();

      // if parsing failed, cry
      if (!File.Exists(jsonFilenameFull)) ShowError("json parsing failure!", saveFilenameFull);

      PopulatePerkList();

      string rawJson = File.ReadAllText(jsonFilenameFull);
      SaveParsed = JsonConvert.DeserializeObject<JsonRoot>(rawJson) ?? new() { Actor_table = [], Checkpoints = [], Header = new() };
      Roster = [.. FillRoster(SaveParsed).OrderByDescending(x => x.Xp)];
      SelectedSoldierPerks = [];

      DefaultSorting();

      // set colors
      rosterGridView.ColumnHeadersDefaultCellStyle.BackColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = rosterHeaderBg;
      rosterGridView.ColumnHeadersDefaultCellStyle.ForeColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = rosterHeaderFg;
      tableLayoutPanel3.BackColor = windowBg;
      shivCheckbox.ForeColor = deadCheckbox.ForeColor = fatiguedCheckbox.ForeColor = woundedCheckbox.ForeColor = windowFg;
      rosterGridView.DefaultCellStyle.BackColor = gridCellBg;
      rosterGridView.DefaultCellStyle.ForeColor = gridCellFg;
      rosterGridView.DefaultCellStyle.SelectionBackColor = gridSelectedCellBg;
      rosterGridView.DefaultCellStyle.SelectionForeColor = gridSelectedCellFg;
      rosterGridView.CellBorderStyle = rosterCellBorders;
      squadPerkList.DefaultCellStyle.ForeColor = squadPerkList.DefaultCellStyle.SelectionForeColor = squadRowFg;
      squadPerkList.DefaultCellStyle.BackColor = squadPerkList.DefaultCellStyle.SelectionBackColor = squadRowBg;
      squadPerkList.CellBorderStyle = squadCellBorders;
      soldierPerksGridView.DefaultCellStyle.ForeColor = soldierPerksGridView.DefaultCellStyle.SelectionForeColor = gridCellFg;
      soldierPerksGridView.DefaultCellStyle.BackColor = soldierPerksGridView.DefaultCellStyle.SelectionBackColor = gridCellBg;
      soldierPerksGridView.CellBorderStyle = soldierCellBorders;
      rosterPerkList.DefaultCellStyle.BackColor = rosterGridView.DefaultCellStyle.SelectionBackColor = gridCellBg;
      rosterPerkList.DefaultCellStyle.ForeColor = rosterGridView.DefaultCellStyle.SelectionForeColor = gridCellFg;
      rosterPerkList.DefaultCellStyle.SelectionBackColor = gridSelectedCellBg;
      rosterPerkList.DefaultCellStyle.SelectionForeColor = gridSelectedCellFg;
      rosterPerkList.CellBorderStyle = rosterCellBorders;
      checklistGridView.DefaultCellStyle.SelectionBackColor = gridCellBg;
      checklistGridView.DefaultCellStyle.SelectionForeColor = gridCellFg;
      checklistGridView.DefaultCellStyle.BackColor = gridCellBg;
      checklistGridView.DefaultCellStyle.ForeColor = gridCellFg;
      checklistGridView.CellBorderStyle = rosterCellBorders;

      rosterPerkList.AutoGenerateColumns = false;

      Text = $"{saveFile?.Name}  -  {(SaveParsed.Header.Save_description ?? new()).Str}";

      trackBar1.Value = BlueshirtLvl;
      ResetRosterPerkList();
      PopupateSoldierPerks(0);

    }

    public void DefaultSorting()
    {
      SortArray.Clear();
      SortArray.Add([.. DefaultSortArray]);
    }

    // back files up
    public void BackupFile(FileInfo saveFile)
    {
      if (!Directory.Exists(todayBackupDir)) Directory.CreateDirectory(todayBackupDir);
      saveFile.CopyTo(Path.Combine(todayBackupDir, $"{execTime}.{saveFile.Name}"), overwrite: true);
    }

    public void PopulatePerkList()
    {
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
          if (!string.IsNullOrWhiteSpace(perkName)) ChecklistPerks.Add(perkName, 0);
        }
      }
    }

    public void ResetChecklist()
    {
      checklistGridView.Rows.Clear();
      ChecklistPerks.ToList().ForEach(x => checklistGridView.Rows.Add(x.Key, x.Value));

      int checklistOk = 0;
      foreach (DataGridViewRow cr in checklistGridView.Rows)
      {
        bool thisOk = false;
        foreach (DataGridViewRow sr in squadPerkList.Rows)
        {
          if (((sr.Cells[0].Value ?? "").ToString() ?? "") == ((cr.Cells[0].Value ?? "").ToString() ?? ""))
          {
            cr.Cells[1].Value = sr.Cells[1].Value;
            checklistOk++;
            thisOk = true;
            break;
          }
        }
        cr.Cells[1].Style = thisOk
          ? new() { BackColor = hiBg, SelectionBackColor = hiBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg }
          : new() { BackColor = loBg, SelectionBackColor = loBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg };
      }

      ChecklistPass = checklistOk == ChecklistPerks.Count;
      string checkMark = ChecklistPass ? "✓" : "ⅹ";
      (tabControl1.TabPages[3] ?? new()).Text = $"{checklistOk}/{ChecklistPerks.Count} {checkMark}";

      
    }

    public void ListRoster(bool squadOnly = false)
    {
      rosterGridView.Rows.Clear();
      squadPerkList.Rows.Clear();
      SquadPerks.Clear();

      List<Soldier> filteredSoldiers = [];

      foreach (Soldier s in Roster)
      {
        if (!shivCheckbox.Checked && s.IsShiv) continue;
        if (!woundedCheckbox.Checked && s.IsWounded && s.HoursOut > RecoverableHrs) continue;
        if (s.IsBlueshirt && !s.IsShiv) continue;
        if (!deadCheckbox.Checked && s.IsDead) continue;
        if (!fatiguedCheckbox.Checked && s.IsFatigued && s.HoursOut > RecoverableHrs) continue;

        if (rosterPerkList.SelectedRows.Count > 0 && (rosterPerkList.SelectedRows[0].Cells[0].Value ?? "").ToString() != "[ANY]")
        {
          bool[] l = new bool[rosterPerkList.SelectedRows.Count];
          for (int i = 0; i < rosterPerkList.SelectedRows.Count; i++)
          {
            bool? filterpass2 = null;
            for (int j = 0; j < s.Perks.Count; j++)
            {
              if (s.Perks[j].Name == (rosterPerkList.SelectedRows[i].Cells[0].Value ?? "").ToString())
              {
                filterpass2 = true;
                break;
              }
            }

            l[i] = filterpass2 ?? false;
          }

          if (!l.All(x => x) && !s.InSquad) continue;
        }

        filteredSoldiers.Add(s);

        if (s.InSquad)
        {
          foreach (Perk p in s.Perks)
          {
            if (SquadPerks.TryAdd(p.Name, 1)) continue;
            else SquadPerks[p.Name]++;
          }
        }
      }

      foreach (var p in SquadPerks) squadPerkList.Rows.Add([p.Key, p.Value]);

      int filteredSoldierIndex = 0;

      maxHp = hiHp = loHp = minHp = false;
      maxMob = hiMob = loMob = minMob = false;
      maxAim = hiAim = loAim = minAim = false;
      maxWill = hiWill = loWill = minWill = false;
      maxDef = hiDef = loDef = minDef = false;

      int inSquad = filteredSoldiers.Where(x => x.InSquad).Count();

      for (int i = 0; i < SortArray.Count; i++) // iterate through sorting arrays
      {
        for (int j = 0; j < SortArray[i].Count; j++) // iterate through columns to sort
        {
          if (SortArray[i][j] == 0) continue; // not sorting by this if it's 0

          bool isInverted = SortArray[i][j] < 0;

          switch (j)
          {
            case 0:
              filteredSoldiers = [.. (isInverted ? filteredSoldiers.OrderByDescending(x => x.LName) : filteredSoldiers.OrderBy(x => x.LName))];
              break;
            case 1:
              filteredSoldiers = [.. (isInverted ? filteredSoldiers.OrderBy(x => x.IsShiv).ThenByDescending(x => x.NName) : filteredSoldiers.OrderBy(x => x.IsShiv).ThenBy(x => x.NName))];
              break;
            case 2:
              filteredSoldiers = isInverted
                ? [.. filteredSoldiers.OrderBy(x => x.IsFatigued).ThenBy(x => x.HoursOut).ThenBy(x => x.IsDead)]
                : [.. filteredSoldiers.OrderByDescending(x => x.IsDead).ThenByDescending(x => x.IsFatigued).ThenByDescending(x => x.HoursOut)];
              break;
            case 3:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Class)] : [.. filteredSoldiers.OrderBy(x => x.Class)];
              break;
            case 4:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Defense)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Defense)];
              break;
            case 5:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.HP)] : [.. filteredSoldiers.OrderBy(x => x.Stats.HP)];
              break;
            case 6:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Mobility)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Mobility)];
              break;
            case 7:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Will)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Will)];
              break;
            case 8:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Aim)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Aim)];
              break;
            case 9:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderBy(x => x.Xp)] : [.. filteredSoldiers.OrderByDescending(x => x.Xp)];
              break;
            case 10:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.ToNext)] : [.. filteredSoldiers.OrderBy(x => x.ToNext)];
              break;
            case 11:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderBy(x => x.RankId)] : [.. filteredSoldiers.OrderByDescending(x => x.RankId)];
              break;
          }
        }
      }

      double hiLo =      1 - (1 / (double)filteredSoldiers.Count) + indPctMobWill; 
      double hiLoHigh =  1 - (1 / (double)filteredSoldiers.Count) + indPctAim;
      double hiLolow =   1 - (1 / (double)filteredSoldiers.Count) + indPctHp;
      double hiLoLower = 1 - (1 / (double)filteredSoldiers.Count) + indPctDef;

      foreach (Soldier f in filteredSoldiers.OrderByDescending(x => x.InSquad))
      {
        if (filteredSoldiers.Count >= minMinMaxListSize && !f.IsShiv && !f.IsDead && !f.IsWounded)
        {
          maxHp = f.Stats.HP == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.HP);
          maxMob = f.Stats.Mobility == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Mobility);
          maxAim = f.Stats.Aim == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Aim);
          maxWill = f.Stats.Will == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Will);
          maxDef = f.Stats.Defense == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Defense);

          minHp = !maxHp && f.Stats.HP == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.HP);
          minMob = !maxMob && f.Stats.Mobility == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Mobility);
          minAim = !maxAim && f.Stats.Aim == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Aim);
          minWill = !maxWill && f.Stats.Will == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Will);
          minDef = !maxDef && f.Stats.Defense == filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Defense);

          if (filteredSoldiers.Count >= minHiLoListSize)
          {
            hiHp = !minHp && !maxHp && f.Stats.HP * hiLolow >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.HP);
            hiMob = !minMob && !maxMob && f.Stats.Mobility * hiLo >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Mobility);
            hiAim = !minAim && !maxAim && f.Stats.Aim * hiLoHigh >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Aim);
            hiWill = !minWill && !maxWill && f.Stats.Will * hiLo >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Will);
            hiDef = !minDef && !maxDef && f.Stats.Defense * hiLoLower >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Defense);

            loHp = !hiHp && !minHp && !maxHp && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.HP) * indPctMobWill >= f.Stats.HP;
            loMob = !hiMob && !minMob && !maxMob && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Mobility) * indPctMobWill >= f.Stats.Mobility;
            loAim = !hiAim && !minAim && !maxAim && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Aim) * indPctAim >= f.Stats.Aim;
            loWill = !hiWill && !minWill && !maxWill && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Will) * indPctAim >= f.Stats.Will;
            loDef = !hiDef && !minDef && !maxDef && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Defense) * indPctHp >= f.Stats.Defense;
          }
        }
        else
        {
          maxHp = hiHp = loHp = minHp = false;
          maxMob = hiMob = loMob = minMob = false;
          maxAim = hiAim = loAim = minAim = false;
          maxWill = hiWill = loWill = minWill = false;
          maxDef = hiDef = loDef = minDef = false;
        }

        object[] dgvrVals =
        {
          f.LName,
          f.NName,
          (f.IsFatigued || f.IsWounded) && !f.IsDead ? ((f.HoursOut / 24) > 0 ? $"{f.HoursOut / 24}d " : "") + $"{f.HoursOut % 24}h" : f.Status,
          f.Class,
          f.Stats.Defense,
          f.Stats.HP,
          f.Stats.Mobility,
          f.Stats.Will,
          f.Stats.Aim,
          f.Xp,
          f.ToNext,
          f.RankName,
          f.RankId,
          f.Id,
        };

        rosterGridView.Rows.Add(dgvrVals);
        DataGridViewRow thisRow = rosterGridView.Rows[filteredSoldierIndex];

        if (f.InSquad)
        {
          thisRow.Frozen = true;
          thisRow.DefaultCellStyle = new() { BackColor = squadRowBg, SelectionBackColor = squadRowBg, ForeColor = squadRowFg, SelectionForeColor = squadRowFg, };

          thisRow.Cells["Aim"].Style =
            thisRow.Cells["Mob"].Style =
            thisRow.Cells["HP"].Style =
            thisRow.Cells["Will"].Style =
            thisRow.Cells["Def"].Style
            = new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg };

          if (inSquad-- == 1)
          {
            thisRow.Height += 4;
            thisRow.DividerHeight = 4;
          }
        }
        else if (!f.IsDead && ((!f.IsWounded && !f.IsFatigued) || f.HoursOut <= RecoverableHrs))
        {
          foreach (Perk p in f.Perks)
          {
            if (ChecklistPerks.ContainsKey(p.Name ?? ""))
            {
              f.HasChecklistPerk = true;
              break;
            }
          }
        }
        if (f.IsWounded || f.IsShiv)
        {
          thisRow.Cells["Aim"].Style =
          thisRow.Cells["Mob"].Style =
          thisRow.Cells["HP"].Style =
          thisRow.Cells["Will"].Style =
          thisRow.Cells["Def"].Style =
            //new() { BackColor = woundStatBg, SelectionBackColor = woundStatBg, ForeColor = deadFg, SelectionForeColor = deadFg };
            new() { ForeColor = f.IsShiv ? gridCellFg : squadHeaderBg, SelectionForeColor = f.IsShiv ? gridCellFg : squadHeaderBg, Font = new(Font, FontStyle.Italic) };
        }
        else 
        { 
          if (maxAim) thisRow.Cells["Aim"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
          else if (minAim) thisRow.Cells["Aim"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
          else if (hiAim) thisRow.Cells["Aim"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
          else if (loAim) thisRow.Cells["Aim"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };

          if (maxMob) thisRow.Cells["Mob"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
          else if (minMob) thisRow.Cells["Mob"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
          else if (hiMob) thisRow.Cells["Mob"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
          else if (loMob) thisRow.Cells["Mob"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };

          if (maxHp) thisRow.Cells["HP"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
          else if (minHp) thisRow.Cells["HP"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
          else if (hiHp) thisRow.Cells["HP"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
          else if (loHp) thisRow.Cells["HP"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };

          if (maxWill) thisRow.Cells["Will"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
          else if (minWill) thisRow.Cells["Will"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
          else if (hiWill) thisRow.Cells["Will"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
          else if (loWill) thisRow.Cells["Will"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };

          if (maxDef) thisRow.Cells["Def"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
          else if (minDef) thisRow.Cells["Def"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
          else if (hiDef) thisRow.Cells["Def"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
          else if (loDef) thisRow.Cells["Def"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };        
        }

        if (f.IsShiv) thisRow.DefaultCellStyle = new() { BackColor = shivBg, SelectionBackColor = shivBg };
        else if (f.IsDead) thisRow.DefaultCellStyle = new() { BackColor = deadBg, ForeColor = deadFg, SelectionBackColor = deadBg, SelectionForeColor = deadFg };

        if (f.LName == "TamTam") thisRow.Cells["LName"].Value = $"TamTam ♥";
        if (f.LName == "ParkaBuoy") thisRow.Cells["LName"].Value = $"ParkaBuoy {new string(' ', DateTime.Now.Second % 5)}{PokemonPicker(f.InSquad ? 1 : 0)}";

        if (f.Luck == 7777)
        {
          thisRow.DefaultCellStyle = new()
          {
            ForeColor = Color.FromArgb(180, 0, 0, 0),
            BackColor = Color.FromArgb(255, 255, 215, 0),
            SelectionBackColor = Color.FromArgb(255, 255, 215, 0),
            Font = new(Font, FontStyle.Bold),
            WrapMode = DataGridViewTriState.True
          };
          thisRow.Cells["LName"].Style = new()
          {
            ForeColor = Color.FromArgb(180, 0, 0, 0),
            BackColor = Color.FromArgb(255, 255, 215, 0),
            SelectionBackColor = Color.FromArgb(255, 255, 215, 0),
            Font = new(Font, FontStyle.Bold),
            WrapMode = DataGridViewTriState.True,
            Alignment = DataGridViewContentAlignment.MiddleRight
          };
          thisRow.Cells["LName"].Value = $"*~･ﾟ✧~     {thisRow.Cells["LName"].Value}";
          thisRow.Cells["NName"].Value = $"{thisRow.Cells["NName"].Value}     ~✧･ﾟ~*";
        }

        if (!f.IsDead)
        {
          if (f.HoursOut <= RecoverableHrs)
          {
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg, Font = new(Font, FontStyle.Bold), ForeColor = deadFg, SelectionForeColor = deadFg };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg, Font = new(Font, FontStyle.Bold) };
          }
          else
          {
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg, ForeColor = deadFg, SelectionForeColor = deadFg };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg };
          }
        }

        filteredSoldierIndex++;
      }

      ResetChecklist();
    }

    private void ResetRosterPerkList()
    {
      RosterPerks.Clear();
      rosterPerkList.Rows.Clear();

      rosterPerkList.Rows.Add("[ANY]", 0);
      rosterPerkList.Rows[0].Height = rosterPerkList.Rows[0].Height + 4;
      rosterPerkList.Rows[0].DividerHeight += 4;

      foreach (Soldier thisSoldier in Roster)
      {
        if (thisSoldier.IsDead || (thisSoldier.IsWounded && thisSoldier.HoursOut > RecoverableHrs) || thisSoldier.IsBlueshirt) continue;
        else
        {
          foreach (Perk p in thisSoldier.Perks)
          {
            if (RosterPerks.TryAdd(p.Name ?? "", 1)) continue;
            else RosterPerks[p.Name ?? ""]++;
          }
        }
      }

      foreach (string p in PerkNames.OrderBy(x => x))
      {
        RosterPerks.TryGetValue(p, out int perkCount);
        if (!nonRosterPerksCheckbox.Checked && perkCount == 0) continue;
        rosterPerkList.Rows.Add([p, perkCount]);
      }
    }

    private static string PokemonPicker(int i)
    {
      if (i == 1)
      {
        return (DateTime.Now.Millisecond % 4) switch
        {
          1 => "༼◥▶ل͜◀◤༽",
          2 => "♪┏(・o･)┛♪",
          _ => ""
        };
      }
      else
      {
        return (DateTime.Now.Millisecond % 23) switch
        {
          1 => "ฅ(^•ﻌ•^ฅ)",
          2 => "ʕ •ᴥ•ʔ",
          3 => "ヽ༼ຈل͜ຈ༽ﾉ",
          4 => "(´･ω･`)",
          6 => ">:3c",
          7 => "(ﾉ◕ヮ◕)ﾉ*:･ ﾟ✧",
          8 => "=＾● ⋏ ●＾=",
          10 => "ლↂ‿‿ↂლ",
          11 => "⊙︿⊙",
          12 => "（＞д＜）ง ▬ι═══ﺤ",
          13 => "↜(╰ •ω•)╯",
          _ => ""
        };
      }
    }

    // this sets the title bar colors
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private void Rosterizer_Load(object sender, EventArgs e)
    {
      int trueValue = 0x01;

      if (darkMode) Application.SetColorMode(SystemColorMode.Dark);
      else Application.SetColorMode(SystemColorMode.Classic);

      int formHeaderBackgroundColorValue = (windowTitleBg.B << 16) | (windowTitleBg.G << 8) | windowTitleBg.R;
      int formHeaderTextColorValue = (windowTitleFg.B << 16) | (windowTitleFg.G << 8) | windowTitleFg.R;

      DwmSetWindowAttribute(Handle, 20, ref trueValue, Marshal.SizeOf(trueValue));
      DwmSetWindowAttribute(Handle, 35, ref formHeaderBackgroundColorValue, Marshal.SizeOf(formHeaderBackgroundColorValue));
      DwmSetWindowAttribute(Handle, 36, ref formHeaderTextColorValue, Marshal.SizeOf(formHeaderTextColorValue));

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

    void ShowError(string msg1, string msg2, bool anyKey = true)
    {
      if (MessageBox.Show(msg1 + Environment.NewLine + msg2) == DialogResult.OK)
      {
        //todo:log
        Environment.Exit(0);
      }
    }

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
              ToNext = 0,
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
              Score = 0,
              Luck = 0
            };
            thisSoldier.IsDead = thisSoldier.Status == "Dead";
            thisSoldier.IsShiv = thisSoldier.RankId == -1;
            thisSoldier.IsWounded = (entity.Properties.FirstOrDefault(x => x.Name == "m_eStatus" && (string?)x.Value == "eStatus_Healing", new()).Number ?? -1) == 0;
            thisSoldier.IsFatigued = thisSoldier.HoursOut > 0 && !thisSoldier.IsWounded;
            thisSoldier.IsBlueshirt = !thisSoldier.IsShiv && thisSoldier.RankId <= BlueshirtLvl;
            thisSoldier.RankName = RankMap(thisSoldier.RankId);
            thisSoldier.ToNext = thisSoldier.IsShiv || thisSoldier.RankId == 7 ? 99999 : thisSoldier.RankId > 0 && thisSoldier.RankId < XpLvls.Length ? XpLvls[thisSoldier.RankId] - thisSoldier.Xp : 0;
            if (thisSoldier.IsShiv) thisSoldier.Class = "Shiv";
            
            if (thisSoldier.IsShiv || thisSoldier.IsWounded || thisSoldier.IsDead) thisSoldier.Luck = 0;
            else thisSoldier.Luck = DateTime.Now.Microsecond % 100;
            
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

            thisSoldier.Score =
                (thisSoldier.Stats.Defense * 17)
              + (thisSoldier.Stats.HP * 22)
              + (thisSoldier.Stats.Mobility * 9)
              + (thisSoldier.Stats.Will * 8)
              + (thisSoldier.Stats.Aim * 5);

            if (thisSoldier.IsFatigued) thisSoldier.Score = (long)(thisSoldier.Score * 0.75);

            thisSoldier.Luck *= DateTime.Now.Microsecond % 100;

            // add the soldier to the roster
            roster.Add(thisSoldier);
          }
        }
      }

      // add 0-soldier perks to roster perks list
      foreach (DataRow row in PerkList.Rows) if (row["Enabled"].ToString() == "1") RosterPerks.TryAdd(row["Name"].ToString() ?? "", 0);
      return roster;
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

    private void deadCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster();
    }

    private void woundedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster();
    }

    private void shivCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster();
    }
    private void fatiguedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster();
    }

    private void rosterGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (e.RowIndex == -1)
      {
        e.PaintBackground(e.CellBounds, false);
        TextRenderer.DrawText(e.Graphics, string.Format("{0}", e.FormattedValue), (e.CellStyle ?? new()).Font, e.CellBounds, e.CellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        e.Handled = true;
      }

      // don't draw zeroes for tonext (shiv/msgt)
      else if (e.RowIndex >= 0 && e.ColumnIndex == 10 && ((long?)e.Value ?? 0) == 99999)
      {
        e.PaintBackground(e.CellBounds, false);
        e.Handled = true;
      }
    }



    private void rosterGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        if (e.RowIndex >= 0)
        {
          DataGridViewRow r = rosterGridView.Rows[e.RowIndex];
          if (r is not null)
          {
            Soldier? s = Roster.FirstOrDefault(x => ((r.Cells["LName"].Value ?? "").ToString() ?? "").Contains(x.LName));
            if (s is not null && !s.IsDead && (!s.IsWounded || s.HoursOut <= RecoverableHrs))
            {
              s?.InSquad = !s.InSquad;
              ListRoster();
            }
          }
        }
        else
        {
          DefaultSorting();
          ListRoster();
        }
      }
      else if (e.Button == MouseButtons.Left)
      {
        if (e.RowIndex == -1)
        {
          if (SortArray.Count() > 5) SortArray = [.. SortArray.Take(MaxSortDepth)];

          // looks like this
          // 0 [0,0,0,0,0,0,0,0,0,1,0,0] - sort by third-to-last column (XP)
          // 1 [0,-1,0,0,0,0,0,0,0,0,0,0] - sort by second column ascending
          // etc
          // iterate through superarray, i.e. previous sorts, up to MaxSortDepth
          bool wasInvert = false;
          for (int i = 0; i < SortArray.Count(); i++)
          {
            bool isInvert = false;
            // iterate through subarrays, each with current sorting
            for (int j = 0; j < SortArray[i].Count(); j++)
            {
              if (e.ColumnIndex == j)
              {
                if (SortArray[i][j] != 0)
                {
                  SortArray[i][j] *= -1;
                  isInvert = true;
                }
              }
            }
            wasInvert = isInvert;
          }
          if (!wasInvert)
          {
            int[] newSort = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            newSort[e.ColumnIndex] = 1;
            SortArray.Add([.. newSort]);
          }

          ListRoster();
        }
        else PopupateSoldierPerks(e.RowIndex);
      }
    }

    private void PopupateSoldierPerks(int rosterRowIndex)
    {
      DataGridViewRow r = rosterGridView.Rows[rosterRowIndex];
      if (r is not null)
      {
        soldierPerksGridView.Rows.Clear();
        Soldier? s = Roster.FirstOrDefault(x => ((r.Cells["LName"].Value ?? "").ToString() ?? "").Contains(x.LName));
        soldierPerksGridView.Rows.Add($"{s.LName} - {s.RankName}");
        soldierPerksGridView.Rows[0].Cells[0].Style = new()
        {
          BackColor = soldierRowBg,
          ForeColor = soldierRowFg,
          SelectionBackColor = soldierRowBg,
          SelectionForeColor = soldierRowFg,
          Font = new(Font, FontStyle.Regular),
          Alignment = DataGridViewContentAlignment.MiddleCenter
        };
        foreach (Perk p in s.Perks.OrderBy(x => x.Name)) soldierPerksGridView.Rows.Add([p.Name ?? ""]);
      }
    }

    private void rosterPerkList_SelectionChanged(object sender, EventArgs e)
    {
      if (!NavFromChecklist && rosterPerkList.SelectedRows.Count > 0 && rosterPerkList.SelectedRows[0].Index > 0)
      {
        if (rosterPerkList.Rows[0].Selected) rosterPerkList.Rows[0].Selected = false;
        ListRoster();
      }
    }

    private void rosterPerkList_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = true;
    }

    private void rosterPerkList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (rosterPerkList.SelectedRows.Count > 0) ListRoster();
    }

    private void RosterSearch(string searchTerm)
    {
      if (!string.IsNullOrWhiteSpace(searchTerm))
      {
        if (tabControl1.SelectedIndex == 3)
        {
          NavFromChecklist = true;

          tabControl1.SelectedIndex = 0;
          foreach (DataGridViewRow row in rosterPerkList.Rows)
          {
            if (((row.Cells[0].Value ?? "").ToString() ?? "") == searchTerm)
            {
              rosterGridView.SuspendLayout();
              rosterGridView.ClearSelection();
              row.Cells[0].Selected = true;
              rosterGridView.ResumeLayout();
              break;
            }
          }
          RosterSearch(searchTerm);
        }

        if (tabControl1.SelectedIndex == 0)
        {
          ResetRosterPerkList();
          List<Tuple<string, int>> passed = [];
          foreach (string s in searchTerm.Split('/'))
          {
            for (int i = 0; i < rosterPerkList.Rows.Count; i++)
            {
              if (((rosterPerkList.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(s, StringComparison.OrdinalIgnoreCase))
              {
                passed.Add(new((rosterPerkList.Rows[i].Cells[0].Value ?? "").ToString() ?? "", (Int32.Parse((rosterPerkList.Rows[i].Cells[1].Value ?? 0).ToString() ?? ""))));
              }
            }
          }

          if (passed.Count > 0)
          {
            rosterPerkList.Rows.Clear();
            rosterPerkList.Rows.Add("[ANY]", 0);
            rosterPerkList.Rows[0].Frozen = true;
            passed.ForEach(x => rosterPerkList.Rows.Add([x.Item1, x.Item2]));

            rosterPerkList.Rows[0].Cells[0].Selected = false;
            rosterPerkList.Rows[1].Cells[0].Selected = true;
          }
        }
      }

      if (NavFromChecklist)
      {
        NavFromChecklist = false;
        tabControl1.SelectedIndex = 3;
      }
      else ListRoster();
    }

    private static long CalculateScore()
    {
      long score = 0;
      long baseScore = 101933;

      Roster.ForEach(x => score += x.InSquad ? x.Score : -204);

      if (ChecklistPassTime < 180) return baseScore - score;
      if (ChecklistPassTime < 240) return baseScore - score - 19320;
      if (ChecklistPassTime < 360) return baseScore - score - 29203;
      if (ChecklistPassTime < 600) return baseScore - score - 39548;
      if (ChecklistPassTime < 1200) return baseScore - score - 83920;
      else return 0;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// action bindings
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        RosterSearch(((TextBox)sender).Text);
        searchTextBox.Text = "";
        e.Handled = true;
        return;
      }
    }

    private void rosterPerkList_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right) ResetRosterPerkList();
    }

    private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
    {
      Rectangle rec = tabControl1.ClientRectangle;
      StringFormat StrFormat = new()
      {
        LineAlignment = StringAlignment.Center,
        Alignment = StringAlignment.Center
      };

      SolidBrush backColor = new(windowBg);
      SolidBrush fontColor;
      e.Graphics.FillRectangle(backColor, rec);

      Font fntTab = new(e.Font, FontStyle.Bold);
      Brush bshBack = new SolidBrush(windowTitleBg);

      for (int i = 0; i < tabControl1.TabPages.Count; i++)
      {
        bool bSelected = (tabControl1.SelectedIndex == i);
        Rectangle recBounds = tabControl1.GetTabRect(i);
        RectangleF tabTextArea = (RectangleF)tabControl1.GetTabRect(i);

        if (i == 0) // roster
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(rosterHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(rosterHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(rosterTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(rosterHeaderFg), tabTextArea, StrFormat);
          }
        }
        else if (i == 1) // squad
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(squadHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(squadHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(squadTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(squadHeaderFg), tabTextArea, StrFormat);
          }
        }
        else if (i == 2) // soldier
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(soldierHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(soldierHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(soldierTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(soldierHeaderFg), tabTextArea, StrFormat);
          }
        }
        else if (i == 3) // checklist
        {
          if (ChecklistPass)
          {
            if (bSelected)
            {
              e.Graphics.FillRectangle(new SolidBrush(checklistGoodHeaderBg), recBounds);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(checklistGoodHeaderFg), tabTextArea, StrFormat);
            }
            else
            {
              e.Graphics.FillRectangle(new SolidBrush(checklistGoodTabUnselectedBg), recBounds);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(checklistGoodHeaderFg), tabTextArea, StrFormat);
            }
          }
          else
          {
            Rectangle r = new(recBounds.X + 2, recBounds.Y + 2, recBounds.Width - 4, recBounds.Height - 4);

            if (bSelected)
            {
              e.Graphics.FillRectangle(new SolidBrush(checklistBadHeaderBg), r);
              e.Graphics.DrawRectangle(new Pen(checklistBadTabHighlight, 2), r);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(soldierHeaderFg), tabTextArea, StrFormat);
            }
            else
            {
              e.Graphics.FillRectangle(new SolidBrush(checklistBadTabUnselectedBg), r);
              e.Graphics.DrawRectangle(new Pen(checklistBadTabHighlight, 4), r);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(checklistBadHeaderFg), tabTextArea, StrFormat);
            }
          }
        }
        else if (i == 4) // options
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(windowBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(windowFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(windowBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(windowFg), tabTextArea, StrFormat);
          }
        }
        else
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(bshBack, recBounds);
            fontColor = new SolidBrush(windowTitleFg);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, fntTab, fontColor, tabTextArea, StrFormat);
          }
          else
          {
            fontColor = new SolidBrush(windowFg);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, fntTab, fontColor, tabTextArea, StrFormat);
          }
        }
      }
    }

    private void rosterGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        tabControl1.SelectedIndex = 2;
      }
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (tabControl1.SelectedIndex == 0 && !NavFromChecklist)
      {
        ResetRosterPerkList();
        ListRoster();
      }

      if (!timer2.IsRunning && tabControl1.SelectedIndex == 3) timerStartStopButton_Click(new(), new());
    }

    private void checklistGridView_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        NavFromChecklist = true;
        RosterSearch((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "");
      }

    }

    private void timerStartStopButton_Click(object sender, EventArgs e)
    {
      timer1.Start();
      if (!timerRunning)
      {
        timer2.Start();
        timerRunning = true;
        timerStartStopButton.Text = "Stop";
      }
      else
      {
        timer2.Stop();
        timerLabel.Text = "";
        timerRunning = false;
        timerStartStopButton.Text = "Start";
      }
    }

    private void timerResetButton_Click(object sender, EventArgs e)
    {
      timer2.Stop();
      timer2.Reset();
      timerRunning = false;
      timerStartStopButton.Text = "Start";

      elapsedTime = TimeSpan.Zero;

      ChecklistPass = false;
      timerLabel.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      timerLabel.ForeColor = windowTitleFg;
      scoreLabel.Text = "";
      ChecklistPassTime = 0;
      foreach (Soldier s in Roster) s.InSquad = false;
      tabControl1.SelectedIndex = 0;
      ResetRosterPerkList();
      ListRoster();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      timerLabel.Text = elapsedTime.ToString("G")[..14];
      if (ChecklistPass)
      {
        if (ChecklistPassTime == 0) ChecklistPassTime = timer2.ElapsedMilliseconds / 1000;
        scoreLabel.Font = timerLabel.Font = new Font("Courier New", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        timerLabel.ForeColor = scoreLabel.ForeColor = maxBg;
        if (DateTime.Now.Millisecond < 500) timerLabel.Text = "";
        else scoreLabel.Text = $"Score: {CalculateScore()}";
        timerRunning = false;
        timer2.Stop();
        timer2.Reset();
        timerStartStopButton.Text = "Start";
      }
      else if (timerRunning)
      {
        timerLabel.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        timerLabel.ForeColor = windowTitleFg;
        elapsedTime = timer2.Elapsed;
      }
    }

    private void rosterPerkList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (e.RowIndex < 1 || e.ColumnIndex < 0) return;
      if (e.RowIndex == 0)
      {
        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
      }
      else
      {
        e.AdvancedBorderStyle.Top = rosterPerkList.AdvancedCellBorderStyle.Top;
      }
      if (e.ColumnIndex == 0 && ((int?)((DataGridView)sender).Rows[e.RowIndex].Cells[1].Value == 0))
      {
        e.PaintBackground(e.CellBounds, false);
        TextRenderer.DrawText(e.Graphics, string.Format("{0}", e.FormattedValue), new((e.CellStyle).Font, FontStyle.Italic), e.CellBounds, soldierHeaderBg);
      }
      if (e.ColumnIndex == 1 && (int?)e.Value == 0)
      {
        e.PaintBackground(e.CellBounds, false);
        e.Handled = true;
      }
    }

    private void trackBar1_ValueChanged(object sender, EventArgs e)
    {
      BlueshirtLvl = ((TrackBar)sender).Value;
      Roster.ForEach(x => x.IsBlueshirt = x.RankId <= BlueshirtLvl);
      ListRoster();
    }
  }
}
