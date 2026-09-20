using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO.Hashing;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RosterizerLW
{
  public partial class Rosterizer : Form
  {
    // muted color scheme
    static bool darkMode = false;
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
    static readonly Color checklistBadHeaderBg = Color.FromArgb(184, 148, 155);
    static readonly Color checklistBadTabUnselectedBg = Color.FromArgb(184, 148, 155);
    static readonly Color checklistBadHeaderFg = Color.FromArgb(222, 255, 222);
    static readonly Color checklistBadTabHighlight = Color.FromArgb(255, 159, 159);
    static readonly Color checklistGoodHeaderBg = Color.FromArgb(95, 143, 110);
    static readonly Color checklistGoodTabUnselectedBg = Color.FromArgb(143, 176, 153);
    static readonly Color checklistGoodHeaderFg = Color.FromArgb(222, 255, 222);
    static readonly Color squadRowBg = Color.FromArgb(194, 194, 194);
    static readonly Color squadRowFg = Color.FromArgb(0, 0, 0);
    static readonly Color soldierRowFg = Color.FromArgb(30, 30, 30);
    static readonly Color windowTitleBg = Color.FromArgb(69, 111, 132);
    static readonly Color windowTitleFg = Color.FromArgb(222, 222, 222);
    static readonly Color windowBg = Color.FromArgb(222, 222, 222);
    static readonly Color windowFg = Color.FromArgb(0, 0, 0);
    static readonly Color gridCellBg = Color.FromArgb(222, 222, 222);
    static readonly Color gridCellFg = Color.FromArgb(0, 0, 0);
    static readonly Color gridBg = SystemColors.ControlDark;

    static readonly Color gridSelectedCellBg = Color.FromArgb(118, 152, 169);
    static readonly Color gridSelectedCellFg = Color.FromArgb(0, 0, 0);
    static readonly Color treeHeaderBg = Color.FromArgb(106, 149, 150);
    static readonly Color treeTabUnselectedBg = Color.FromArgb(150, 180, 181);
    static readonly Color treeHeaderFg = Color.FromArgb(222, 255, 222);
    static readonly DataGridViewCellBorderStyle rosterCellBorders = DataGridViewCellBorderStyle.Raised;
    static readonly DataGridViewCellBorderStyle squadCellBorders = DataGridViewCellBorderStyle.RaisedHorizontal;
    static readonly DataGridViewCellBorderStyle soldierCellBorders = DataGridViewCellBorderStyle.RaisedHorizontal;
    static readonly DataGridViewCellBorderStyle checklistCellBorders = DataGridViewCellBorderStyle.Single;
    static readonly DataGridViewCellBorderStyle treeCellBorders = DataGridViewCellBorderStyle.RaisedHorizontal;
    static readonly bool LongWoundBgStyle = false;
    static readonly bool StatWoundBgStyle = false;

    static readonly Color darkDeadFg = SystemColors.ControlText;
    static readonly Color darkFatigueBg = SystemColors.ControlDark;
    static readonly Color darkShivBg = SystemColors.ControlLight;
    static readonly Color darkDeadBg = Color.FromArgb(99, 38, 44);
    static readonly Color darkWoundBg = Color.FromArgb(130, 81, 86);
    static readonly Color darkMaxBg = Color.FromArgb(46, 92, 107);
    static readonly Color darkHiBg = Color.FromArgb(87, 124, 136);
    static readonly Color darkLoBg = Color.FromArgb(136, 99, 87);
    static readonly Color darkMinBg = Color.FromArgb(97, 71, 62);
    static readonly Color darkRosterHeaderBg = SystemColors.InactiveCaption;
    static readonly Color darkRosterTabUnselectedBg = SystemColors.InactiveCaption;
    static readonly Color darkRosterHeaderFg = SystemColors.ControlText;
    static readonly Color darkSquadHeaderBg = SystemColors.ControlDarkDark;
    static readonly Color darkSquadTabUnselectedBg = SystemColors.ControlDarkDark;
    static readonly Color darkSquadHeaderFg = SystemColors.ControlText;
    static readonly Color darkSoldierHeaderBg = SystemColors.Control;
    static readonly Color darkSoldierTabUnselectedBg = SystemColors.ControlLight;
    static readonly Color darkSoldierHeaderFg = SystemColors.ControlText;
    static readonly Color darkChecklistBadHeaderBg = SystemColors.InactiveCaption;
    static readonly Color darkChecklistBadTabUnselectedBg = SystemColors.InactiveCaption;
    static readonly Color darkChecklistBadHeaderFg = SystemColors.ControlText;
    static readonly Color darkChecklistBadTabHighlight = SystemColors.ActiveCaption;
    static readonly Color darkChecklistGoodHeaderBg = SystemColors.HotTrack;
    static readonly Color darkChecklistGoodTabUnselectedBg = SystemColors.ActiveCaption;
    static readonly Color darkChecklistGoodHeaderFg = SystemColors.ControlText;
    static readonly Color darkSquadRowBg = SystemColors.ControlDark;
    static readonly Color darkSquadRowFg = SystemColors.ControlText;
    static readonly Color darkSoldierRowFg = SystemColors.ControlText;
    static readonly Color darkWindowTitleBg = SystemColors.HotTrack;
    static readonly Color darkWindowTitleFg = SystemColors.ControlText;
    static readonly Color darkWindowBg = SystemColors.ControlLightLight;
    static readonly Color darkWindowFg = SystemColors.ControlText;
    static readonly Color darkGridCellBg = SystemColors.Control;
    static readonly Color darkGridCellFg = SystemColors.ControlText;
    static readonly Color darkGridSelectedCellBg = SystemColors.ActiveCaption;
    static readonly Color darkGridSelectedCellFg = SystemColors.InactiveCaptionText;
    static readonly Color darkTreeHeaderBg = Color.FromArgb(46, 92, 107);
    static readonly Color darkTreeTabUnselectedBg = Color.FromArgb(87, 124, 136);
    static readonly Color darkTreeHeaderFg = SystemColors.ControlText;
    static readonly DataGridViewCellBorderStyle darkRosterCellBorders = DataGridViewCellBorderStyle.Single;
    static readonly DataGridViewCellBorderStyle darkSquadCellBorders = DataGridViewCellBorderStyle.Single;
    static readonly DataGridViewCellBorderStyle darkSoldierCellBorders = DataGridViewCellBorderStyle.Single;
    static readonly DataGridViewCellBorderStyle darkChecklistCellBorders = DataGridViewCellBorderStyle.Single;
    static readonly DataGridViewCellBorderStyle darkTreeCellBorders = DataGridViewCellBorderStyle.Single;

    static readonly bool darkLongWoundBgStyle = false;
    static readonly bool darkStatWoundBgStyle = false;

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
        static readonly Color checklistBadHeaderBg = Color.FromArgb(95, 143, 110);
        static readonly Color checklistBadTabUnselectedBg = Color.FromArgb(143, 176, 153);
        static readonly Color checklistBadHeaderFg = Color.FromArgb(222, 255, 222);
        static readonly Color checklistBadTabHighlight = Color.FromArgb(184, 148, 155);
        static readonly Color checklistGoodHeaderBg = Color.FromArgb(95, 143, 110);
        static readonly Color checklistGoodTabUnselectedBg = Color.FromArgb(143, 176, 153);
        static readonly Color checklistGoodHeaderFg = Color.FromArgb(222, 255, 222);
        static readonly Color squadRowBg = Color.FromArgb(194, 194, 194);
        static readonly Color squadRowFg = Color.FromArgb(0, 0, 0);
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
        static readonly DataGridViewCellBorderStyle squadCellBorders = DataGridViewCellBorderStyle.Sunken;
        static readonly DataGridViewCellBorderStyle soldierCellBorders = DataGridViewCellBorderStyle.Raised;
        static readonly DataGridViewCellBorderStyle checklistCellBorders = DataGridViewCellBorderStyle.Sunken;
        static readonly bool LongWoundBgStyle = false;
        static readonly bool StatWoundBgStyle = false;
    */

    private Stopwatch timer2 = new();
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private bool timerRunning = false;

    // these change the relative count of high/low numbers highlighted (other than min/max) for the various stats (multiple since i.e. percentage spread on def will be lower than aim/will)
    // -- or to put it another way, too much green/red? make these lower
    static double indPctMobWill = 0.22;
    static double indPctAim = 0.26;
    static double indPctHp = 0.5;
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
    public static List<Soldier> Roster = [];
    public static List<Soldier> Squad = [];
    //public static FileInfo SaveFile;
    public static JsonRoot SaveParsed;
    public static DataTable PerkList;
    public static DataTable ChecklistPerksDatatable;
    public static List<string> PerkNames;
    public static Dictionary<string, int> SquadPerks = [];
    public static Dictionary<string, int> ListedRosterPerks = [];
    public static Dictionary<string, int> AllRosterPerks = [];

    public static int RecoverableHrs = 8;
    public static int BlueshirtLvl = 3;
    public static long[] XpLvls = [120, 350, 700, 1200, 2000, 3000, 4200]; // xp levels per DefaultGameCore.ini ~ln. 900
    public static int MinSquadSize = 6;
    public static int MaxSquadSize = 10;
    public static int SquadSize = 8;
    public static int CurrentSquadSize = 0;
    public static Dictionary<string, int> ChecklistPerks = [];
    public static bool ChecklistPass = false;
    public static long ChecklistPassTime = 0;
    public static List<List<int>> SortArray = [[]];
    public static int[] DefaultSortArray = [0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0];
    public static int MaxSortDepth = 5;
    public static Font SmallFont = new("Tahoma", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
    RelatedPerk RosterTree0 = new()
    {
      PerkTree = [],
      PerkBranches = [],
      SubRoster = Roster
    };

    RelatedPerk RosterTree1 = new()
    {
      PerkTree = [],
      PerkBranches = [],
      SubRoster = Roster
    };
    RelatedPerk RosterTree2 = new()
    {
      PerkTree = [],
      PerkBranches = [],
      SubRoster = []
    };
    RelatedPerk RosterTree3 = new()
    {
      PerkTree = [],
      PerkBranches = [],
      SubRoster = []
    };
    RelatedPerk RosterTree4 = new()
    {
      PerkTree = [],
      PerkBranches = [],
      SubRoster = []
    };
    bool PerkTreeByName = false;
    Perk[] RosterPerkTree = [];
    // path from which we'll load the save (or blank to load AutoSavePath below
    static string overrideSavePath = "..\\..\\..\\saveBackup\\save43";
    // xcom save dir from which the most recent save file will be selected
    string AutoSavePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\XCOM - Enemy Within\\XComGame\\SaveData";
    static string OutputDir = "..\\..\\..\\output\\"; // the output dir
    static string BackupDir = "..\\..\\..\\saveBackup\\"; // path where saves will be backed up
    static string X2jPath = "..\\..\\..\\exe\\xcom2json.exe"; // the path to xcom2json.exe, CRC 
    static string PerkListPath = "..\\..\\..\\csv\\Long War ID reference - Perks.csv";
    static string PerkChecklistPath = "..\\..\\..\\csv\\Checklist - Perks.csv";
    public static FileInfo SaveFile;
    public static string SoldierSelectedIdXP = "";
    public static long number_of_times_we_have_called_this_bullshit = 0;

    UInt64 x2jHash = 550290084522337840; // ensure the expected xcom2json version


    // ------------------------------------------------------------------------------------------------------------------------------------------------------------
    string saveFilenameFull = "";
    string saveFilename = "";
    string jsonFilenameFull = "";
    string saveNameRegex = "^save\\d{1,3}\\Z"; // regex: starts with "save", has 1-3 numbers after it, then ends
    string todayBackupDir = Path.Combine(BackupDir, $"{DateTime.Now:yyyyMMdd}");
    string todayOutputDir = Path.Combine(OutputDir, $"{DateTime.Now:yyyyMMdd}");

    string execTime = $"{DateTime.Now:yyyyMMdd.HHmm}";

    FileInfo x2jFile = new(X2jPath);
    UInt64 hash = Crc64.HashToUInt64(File.ReadAllBytes(X2jPath));

    public Rosterizer()
    {
      InitializeComponent();

      if (!x2jFile.Exists || hash != x2jHash) ShowError("invalid xcom2json exe!", x2jFile.FullName);

      SaveFile ??= new(overrideSavePath);
      // build datatable out of csv file (directly copied from swf's id reference sheets)
      PerkList ??= ConvertCSVtoDataTable(PerkListPath);
      ChecklistPerksDatatable ??= ConvertCSVtoDataTable(PerkChecklistPath);

      if (!SaveFile.Exists)
      {
        if (Directory.Exists(AutoSavePath))
        {
          foreach (FileInfo saveFile in new DirectoryInfo(AutoSavePath)
            .GetFiles()
            .Where(x => Regex.IsMatch(x.Name, saveNameRegex)) // match "save[123]" regex
            .OrderByDescending(x => x.LastWriteTime)) // get the most recent files
          {
            Rosterizer.SaveFile ??= saveFile; // pluck first one (most recent) for analysis (assign to it if it's null)
            break; // only want one
          }
        }
        else ShowError("dir/file not found:", AutoSavePath);
      }

      BackupFile(SaveFile); // back files up

      saveFilenameFull = (SaveFile ?? new("")).FullName;
      saveFilename = (SaveFile ?? new("")).Name;

      // build full filename of output json
      if (!Directory.Exists(todayOutputDir)) Directory.CreateDirectory(todayOutputDir);

      jsonFilenameFull = Path.Combine(todayOutputDir, $"{execTime}.{saveFilename}.json");

      // run xcom2json exe on save file
      Process.Start("cmd", $"/C {X2jPath} -o \"{jsonFilenameFull}\" \"{saveFilenameFull}\"").WaitForExit();

      // if parsing failed, cry
      if (!File.Exists(jsonFilenameFull)) ShowError("json parsing failure!", saveFilenameFull);

      PopulatePerkList();

      string rawJson = File.ReadAllText(jsonFilenameFull);
      SaveParsed = JsonConvert.DeserializeObject<JsonRoot>(rawJson) ?? new() { Actor_table = [], Checkpoints = [], Header = new() };
      Roster = [.. FillRoster(SaveParsed).OrderByDescending(x => x.Xp)];

      DefaultSorting();
      //SetRosterPerks();
      squadGridView.Size = new Size(squadGridView.Width, 0);

      // set colors
      SetColor();

      Text = $"{SaveFile?.Name}  -  {(SaveParsed.Header.Save_description ?? new()).Str}";

      trackBar1.Value = BlueshirtLvl;

      trackBar2.Minimum = MinSquadSize;
      trackBar2.Maximum = MaxSquadSize;
      trackBar2.Value = SquadSize;
      treeDataGrid.CellBorderStyle = PerkTreeByName ? DataGridViewCellBorderStyle.Single : DataGridViewCellBorderStyle.Raised;

      HideSquadGrid();
      BuildPerkTree();
      SetPerkTree(RosterTree1);

      timerStartStopButton_Click(new(), new()); // don't think about it
    }

    public void ShowSquadGrid()
    {
      squadGridView.Visible = true;
      tableLayoutPanel1.SetRow(squadGridView, 0);
      tableLayoutPanel1.SetRow(rosterGridView, 1);
      tableLayoutPanel1.SetRowSpan(rosterGridView, 1);

    }

    public void HideSquadGrid()
    {
      squadGridView.Visible = false;
      squadGridView.Rows.Clear();
      tableLayoutPanel1.RowStyles[0].Height = 3;
      tableLayoutPanel1.SetRow(squadGridView, 1);
      tableLayoutPanel1.SetRow(rosterGridView, 0);
      tableLayoutPanel1.SetRowSpan(rosterGridView, 2);
    }

    public void SetColor()
    {
      if (darkMode)
      {
        Application.SetColorMode(SystemColorMode.Dark);

        rosterGridView.ColumnHeadersDefaultCellStyle.BackColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = darkRosterHeaderBg;
        rosterGridView.ColumnHeadersDefaultCellStyle.ForeColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = darkRosterHeaderFg;
        tableLayoutPanel3.BackColor = darkWindowBg;
        shivCheckbox.ForeColor = deadCheckbox.ForeColor = fatiguedCheckbox.ForeColor = woundedCheckbox.ForeColor = darkWindowFg;
        rosterGridView.DefaultCellStyle.BackColor = darkGridCellBg;
        rosterGridView.DefaultCellStyle.ForeColor = darkGridCellFg;
        rosterGridView.DefaultCellStyle.SelectionBackColor = darkSoldierTabUnselectedBg;
        rosterGridView.DefaultCellStyle.SelectionForeColor = darkGridSelectedCellFg;
        rosterGridView.CellBorderStyle = darkRosterCellBorders;
        squadGridView.DefaultCellStyle.BackColor = darkGridCellBg;
        squadGridView.DefaultCellStyle.ForeColor = darkGridCellFg;
        squadGridView.DefaultCellStyle.SelectionBackColor = darkSoldierTabUnselectedBg;
        squadGridView.DefaultCellStyle.SelectionForeColor = darkGridSelectedCellFg;
        squadGridView.CellBorderStyle = darkRosterCellBorders;
        squadPerkList.DefaultCellStyle.ForeColor = darkSquadRowFg;
        squadPerkList.DefaultCellStyle.BackColor = darkSquadRowBg;
        squadPerkList.DefaultCellStyle.SelectionForeColor = darkSquadRowFg;
        squadPerkList.DefaultCellStyle.SelectionBackColor = darkSquadTabUnselectedBg;
        squadPerkList.CellBorderStyle = darkSquadCellBorders;
        soldierPerksGridView.DefaultCellStyle.ForeColor = darkGridCellFg;
        soldierPerksGridView.DefaultCellStyle.BackColor = darkGridCellBg;
        soldierPerksGridView.DefaultCellStyle.SelectionForeColor = darkSoldierRowFg;
        soldierPerksGridView.DefaultCellStyle.SelectionBackColor = darkSoldierTabUnselectedBg;
        soldierPerksGridView.CellBorderStyle = darkSoldierCellBorders;
        //rosterPerkList.DefaultCellStyle.BackColor = darkGridCellBg;
        //rosterPerkList.DefaultCellStyle.ForeColor = darkGridCellFg;
        //rosterPerkList.DefaultCellStyle.SelectionBackColor = darkGridSelectedCellBg;
        //rosterPerkList.DefaultCellStyle.SelectionForeColor = darkGridSelectedCellFg;
        //rosterPerkList.CellBorderStyle = darkRosterCellBorders;
        checklistGridView.DefaultCellStyle.SelectionBackColor = darkGridCellBg;
        checklistGridView.DefaultCellStyle.SelectionForeColor = darkGridCellFg;
        checklistGridView.DefaultCellStyle.BackColor = darkGridCellBg;
        checklistGridView.DefaultCellStyle.ForeColor = darkGridCellFg;
        checklistGridView.CellBorderStyle = darkChecklistCellBorders;
        panel1.BackColor = darkWindowBg;
        panel1.ForeColor = darkWindowFg;

        treeDataGrid.DefaultCellStyle.ForeColor = darkGridCellFg;
        treeDataGrid.DefaultCellStyle.BackColor = darkGridCellBg;
        treeDataGrid.DefaultCellStyle.SelectionForeColor = darkGridCellFg;
        treeDataGrid.DefaultCellStyle.SelectionBackColor = darkGridCellBg;

      }
      else
      {
        Application.SetColorMode(SystemColorMode.Classic);

        rosterGridView.ColumnHeadersDefaultCellStyle.BackColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = rosterHeaderBg;
        rosterGridView.ColumnHeadersDefaultCellStyle.ForeColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = rosterHeaderFg;
        tableLayoutPanel3.BackColor = windowBg;
        shivCheckbox.ForeColor = deadCheckbox.ForeColor = fatiguedCheckbox.ForeColor = woundedCheckbox.ForeColor = windowFg;
        rosterGridView.DefaultCellStyle.BackColor = gridCellBg;
        rosterGridView.DefaultCellStyle.ForeColor = gridCellFg;
        rosterGridView.DefaultCellStyle.SelectionBackColor = gridCellBg;
        rosterGridView.DefaultCellStyle.SelectionForeColor = gridCellFg;
        rosterGridView.RowHeadersDefaultCellStyle.BackColor = gridCellBg;
        rosterGridView.RowHeadersDefaultCellStyle.ForeColor = gridCellFg;
        rosterGridView.RowHeadersDefaultCellStyle.SelectionBackColor = soldierTabUnselectedBg;
        rosterGridView.RowHeadersDefaultCellStyle.SelectionForeColor = soldierHeaderFg;
        rosterGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        rosterGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        rosterGridView.RowHeadersDefaultCellStyle.Padding = Padding.Empty;
        rosterGridView.RowHeadersWidth = 15;
        rosterGridView.BackgroundColor = gridBg;
        rosterGridView.GridColor = gridBg;
        squadGridView.RowHeadersDefaultCellStyle.BackColor = gridCellBg;
        squadGridView.RowHeadersDefaultCellStyle.ForeColor = gridCellFg;
        squadGridView.RowHeadersDefaultCellStyle.SelectionBackColor = gridCellBg;
        squadGridView.RowHeadersDefaultCellStyle.SelectionForeColor = gridCellFg;
        squadGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        squadGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        squadGridView.RowHeadersDefaultCellStyle.Padding = Padding.Empty;
        squadGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

        squadGridView.RowHeadersWidth = 15;
        rosterGridView.CellBorderStyle = rosterCellBorders;
        squadPerkList.DefaultCellStyle.ForeColor = squadRowFg;
        squadPerkList.DefaultCellStyle.BackColor = squadRowBg;
        squadPerkList.DefaultCellStyle.SelectionForeColor = squadRowFg;
        squadPerkList.DefaultCellStyle.SelectionBackColor = squadTabUnselectedBg;
        squadPerkList.CellBorderStyle = squadCellBorders;
        soldierPerksGridView.DefaultCellStyle.ForeColor = gridCellFg;
        soldierPerksGridView.DefaultCellStyle.BackColor = gridCellBg;
        soldierPerksGridView.DefaultCellStyle.SelectionForeColor = soldierRowFg;
        soldierPerksGridView.DefaultCellStyle.SelectionBackColor = soldierTabUnselectedBg;
        soldierPerksGridView.CellBorderStyle = soldierCellBorders;
        //rosterPerkList.DefaultCellStyle.BackColor = gridCellBg;
        //rosterPerkList.DefaultCellStyle.ForeColor = gridCellFg;
        //rosterPerkList.DefaultCellStyle.SelectionBackColor = gridSelectedCellBg;
        //rosterPerkList.DefaultCellStyle.SelectionForeColor = gridSelectedCellFg;
        //rosterPerkList.CellBorderStyle = rosterCellBorders;
        checklistGridView.DefaultCellStyle.SelectionBackColor = gridCellBg;
        checklistGridView.DefaultCellStyle.SelectionForeColor = gridCellFg;
        checklistGridView.DefaultCellStyle.BackColor = gridCellBg;
        checklistGridView.DefaultCellStyle.ForeColor = gridCellFg;
        checklistGridView.CellBorderStyle = checklistCellBorders;
        tabPage9.BackColor = windowBg;
        panel1.BackColor = windowBg;
        panel1.ForeColor = windowFg;

        treeDataGrid.DefaultCellStyle.ForeColor = gridCellFg;
        treeDataGrid.DefaultCellStyle.BackColor = gridCellBg;
        treeDataGrid.DefaultCellStyle.SelectionForeColor = gridCellFg;
        treeDataGrid.DefaultCellStyle.SelectionBackColor = gridCellBg;

        perkFilterTextbox.BackColor = windowBg;
        perkFilterTextbox.ForeColor = windowFg;
      }

    }

    public static void DefaultSorting()
    {
      SortArray.Clear();
      SortArray.Add([.. DefaultSortArray]);
    }

    public static void DoSorting(int colIndex)
    {
      if (colIndex < 0 || colIndex > 11)
      {
        return;
      }

      if (SortArray.Count > MaxSortDepth) SortArray = [.. SortArray.TakeLast(MaxSortDepth)];

      // looks like this
      // 0 [0,0,0,0,0,0,0,0,0,1,0,0] - sort by third-to-last column (XP)
      // 1 [0,-1,0,0,0,0,0,0,0,0,0,0] - sort by second column ascending
      // etc
      // iterate through superarray, i.e. previous sorts, up to MaxSortDepth
      bool wasInvert = false;
      for (int i = 0; i < SortArray.Count; i++)
      {
        bool isInvert = false;
        // iterate through subarrays, each with current sorting
        for (int j = 0; j < SortArray[i].Count; j++)
        {
          if (colIndex == j)
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
        newSort[colIndex] = 1;
        SortArray.Add([.. newSort]);
      }
    }

    public void ToggleInSquad(object sender, int rowIndex)
    {
      if (((DataGridView)sender).Rows[rowIndex] is not null)
      {

        Soldier? s = Roster.FirstOrDefault(x =>
          ((((DataGridView)sender).Rows[rowIndex].Cells[0].Value ?? "").ToString() ?? "").Contains(x.LName)
          && (long)((((DataGridView)sender).Rows[rowIndex].Cells[14].Value ?? "")) == x.Id);
        if (s is not null)
        {
          if (!s.IsDead && (!s.IsWounded || s.HoursOut <= RecoverableHrs))
          {
            int vscroll = ((DataGridView)sender).FirstDisplayedScrollingRowIndex;

            if (s?.InSquad == true)
            {
              if (CurrentSquadSize == 1) HideSquadGrid();
              else tableLayoutPanel1.RowStyles[0].Height -= 25;

              if (((DataGridView)sender).Name == "squadGridView")
              {
                if (squadGridView.Rows.Count > 1) squadGridView.Rows.RemoveAt(rowIndex);
                else squadGridView.Rows.Clear();
              }
              else
              {
                for (int i = 0; i < squadGridView.Rows.Count; i++)
                {
                  if (((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "") == s.LName && (long)((squadGridView.Rows[i].Cells[14].Value ?? "") ?? "") == s.Id)
                  {
                    squadGridView.Rows.RemoveAt(i);
                    break;
                  }
                }
              }

              CurrentSquadSize--;
              Roster.Where(x => x.LName == s.LName).ToList().ForEach(x => x.InSquad = false);
              Squad.Remove(s);

              switch (RosterPerkTree.Length)
              {
                case 0:
                  ListRoster(RosterTree1.SubRoster);
                  break;
                case 1:
                  ListRoster(RosterTree2.SubRoster);
                  break;
                case 2:
                  ListRoster(RosterTree3.SubRoster);
                  break;
                case 3:
                  ListRoster(RosterTree4.SubRoster);
                  break;
              }
            }
            else if (CurrentSquadSize < trackBar2.Value && s?.InSquad == false)
            {
              if (CurrentSquadSize == 0) ShowSquadGrid();

              tableLayoutPanel1.RowStyles[0].Height += 25;
              object[] dgvrVals =
              [
                s.LName,
                s.NName,
                (s.IsFatigued || s.IsWounded) && !s.IsDead ? ((s.HoursOut / 24) > 0 ? $"{s.HoursOut / 24}d " : "") + $"{s.HoursOut % 24}h" : s.Status,
                s.Class,
                s.Stats.Defense,
                s.Stats.HP,
                s.Stats.Mobility,
                $"{(s.Stats.Will == 99999 ? string.Empty : s.Stats.Will)}",
                s.Stats.Aim,
                s.Xp,
                $"{(s.ToNext == 99999 ? string.Empty : s.ToNext)}",
                s.RankName,
                s.RankId,
                s.RosterNumber,
                s.Id,
                s.HasChecklistPerk,
                !(s.IsWounded && s.HoursOut > RecoverableHrs) && !s.IsDead
              ];
              squadGridView.Rows.Add(dgvrVals);
              //rosterGridView.Rows.RemoveAt(rowIndex);

              CurrentSquadSize++;
              s.InSquad = true;
              Squad.Add(s);
              Roster.Where(x => x.LName == s.LName && x.Id == s.Id).ToList().ForEach(x => x.InSquad = true);
              switch (RosterPerkTree.Length)
              {
                case 0:
                  ListRoster(RosterTree1.SubRoster);
                  break;
                case 1:
                  ListRoster(RosterTree2.SubRoster);
                  break;
                case 2:
                  ListRoster(RosterTree3.SubRoster);
                  break;
                case 3:
                  ListRoster(RosterTree4.SubRoster);
                  break;
              }
            }

            // switch RosterNumber to squad count
            for (int i = 0; i < squadGridView.Rows.Count; i++) squadGridView.Rows[i].Cells[12].Value = (i + 1);

            if (vscroll > 0 && ((DataGridView)sender).Rows.Count >= vscroll - 1) ((DataGridView)sender).FirstDisplayedScrollingRowIndex = vscroll;
          }
        }
      }
    }

    // back files up
    public void BackupFile(FileInfo saveFile)
    {
      if (!Directory.Exists(todayBackupDir)) Directory.CreateDirectory(todayBackupDir);
      saveFile.CopyTo(Path.Combine(todayBackupDir, $"{execTime}.{saveFile.Name}"), overwrite: true);
    }

    public static void PopulatePerkList()
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
        foreach (Soldier s in Squad)
        {
          if (s.Perks.Select(x => x.Name).Contains(cr.Cells[0].Value))
          {
            cr.Cells[1].Value = (int)(cr.Cells[1].Value ?? 0) + 1;
            checklistOk++;
            thisOk = true;
            break;
          }
        }

        cr.Cells[1].Style = thisOk
          ? new() { BackColor = hiBg, SelectionBackColor = hiBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg }
          : new() { BackColor = minBg, SelectionBackColor = minBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg };
      }

      ChecklistPass = checklistOk == ChecklistPerks.Count;
      string checkMark = ChecklistPass ? "✓" : "ⅹ";
      (tabControl1.TabPages[3] ?? new()).Text = $"{checklistOk}/{ChecklistPerks.Count} {checkMark}";
    }

    public void SetPerkTree(RelatedPerk perkIn)
    {
      if (perkIn.PerkBranches.Count == 0 || perkIn.SubRoster.Count == 0) return;

      ListRoster(perkIn.SubRoster);
      RosterPerkTree = perkIn.PerkTree;

      perkFilterTextbox.TextAlign = HorizontalAlignment.Left;
      perkFilterTextbox.Text = "\\";
      treeDataGrid.Rows.Clear();

      treeDataGrid.Rows.Add("\\", "");
      treeDataGrid.Rows[0].DefaultCellStyle = new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg };
      treeDataGrid.Rows[0].Frozen = true;

      for (int i = 0; i <= RosterPerkTree.Length; i++)
      {
        if (i < RosterPerkTree.Length)
        {
          perkFilterTextbox.Text += $" {RosterPerkTree[i].Name} \\";
          treeDataGrid.Rows.Add($"{new string(' ', (i * 2) + 2)}\\ {RosterPerkTree[i].Name}", "");
        }

        treeDataGrid.Rows[i].Cells[0].Style = treeDataGrid.Rows[i].Cells[1].Style = new()
        {
          BackColor = treeTabUnselectedBg,
          ForeColor = treeHeaderFg,
          SelectionBackColor = treeTabUnselectedBg,
          SelectionForeColor = treeHeaderFg
        };
        treeDataGrid.Rows[i].Frozen = true;
      }

      treeDataGrid.Rows[RosterPerkTree.Length].Height = treeDataGrid.Rows[RosterPerkTree.Length].Height + 4;
      treeDataGrid.Rows[RosterPerkTree.Length].DividerHeight += 4;


      foreach (var p in PerkTreeByName
        ? perkIn.PerkBranches.Where(x => x.Value.SubRoster.Count > 0).OrderBy(x => x.Key.Name).ToDictionary()
        : perkIn.PerkBranches.Where(x => x.Value.SubRoster.Count > 0).OrderByDescending(x => x.Value.SubRoster.Count).ThenBy(x => x.Key.Name).ToDictionary()
      )
      {
        bool doSkip = false;
        foreach (DataGridViewRow q in treeDataGrid.Rows)
        {
          if ((q.Cells[0].Value ?? "").ToString() == p.Key.Name) doSkip = true;
        }
        if (doSkip) continue;

        treeDataGrid.Rows.Add([p.Key.Name ?? "", p.Value.SubRoster.Count]);
      }
      if (perkFilterTextbox.TextLength > 45) perkFilterTextbox.Text = "~" + perkFilterTextbox.Text.Substring(perkFilterTextbox.TextLength - 45, 45);
      //perkFilterTextbox.Text = perkFilterTextbox.Text.TrimEnd('\\');
    }

    public void ListRoster(List<Soldier>? rosterIn = null, bool squadUpdate = true)
    {
      if (squadUpdate)
      {
        squadPerkList.Rows.Clear();
        SquadPerks.Clear();
      }

      rosterIn ??= Roster;
      rosterGridView.Rows.Clear();

      List<Soldier> filteredSoldiers = [];
      int shivCount = 0;

      foreach (Soldier s in Roster)
      {
        if (squadUpdate && Squad.Contains(s))
        {
          s.Perks.ForEach(x => { if (!SquadPerks.TryAdd(x.Name ?? "", 1)) SquadPerks[x.Name ?? ""]++; });
          if (s.IsShiv) shivCount++;
        }
      }

      if (squadUpdate && SquadPerks.Count > 0)
      {
        squadPerkList.Rows.Add($"Soldiers: {CurrentSquadSize}, Shivs: {shivCount}", "");
        squadPerkList.Rows[0].Height = squadPerkList.Rows[0].Height + 4;
        squadPerkList.Rows[0].DividerHeight += 4;
        squadPerkList.Rows[0].Frozen = true;
        squadPerkList.Rows[0].Cells[0].Style = new()
        {
          BackColor = squadTabUnselectedBg,
          ForeColor = gridCellFg,
          SelectionBackColor = squadTabUnselectedBg,
          SelectionForeColor = gridCellFg,
          Alignment = DataGridViewContentAlignment.MiddleCenter,
          Font = new(Font, FontStyle.Regular),
        };

        foreach (var p in SquadPerks)
        {
          if (squadPerkList.Rows.Count > 1 && p.Key.CompareTo((squadPerkList.Rows[1].Cells[0].Value ?? "").ToString()) <= 0) squadPerkList.Rows.Insert(1, [p.Key, p.Value]);
          else squadPerkList.Rows.Add([p.Key, p.Value]);

          if (ChecklistPerks.ContainsKey(p.Key)) squadPerkList.Rows[^1].Cells[0].Style = squadPerkList.Rows[^1].Cells[1].Style = new()
          {
            BackColor = checklistGoodTabUnselectedBg,
            ForeColor = checklistGoodHeaderFg,
            SelectionBackColor = checklistGoodTabUnselectedBg,
            SelectionForeColor = checklistGoodHeaderFg,
            Font = new(Font, FontStyle.Regular),
          };
        }
      }
      foreach (Soldier s in rosterIn)
      {
        if (!shivCheckbox.Checked && s.IsShiv) continue;
        if (!woundedCheckbox.Checked && s.IsWounded && s.HoursOut > RecoverableHrs) continue;
        if (s.IsBlueshirt && !s.IsShiv) continue;
        if (!deadCheckbox.Checked && s.IsDead) continue;
        if (!fatiguedCheckbox.Checked && s.IsFatigued && s.HoursOut > RecoverableHrs) continue;

        filteredSoldiers.Add(s);
      }

      int filteredSoldierIndex = 0;

      maxHp = hiHp = loHp = minHp = false;
      maxMob = hiMob = loMob = minMob = false;
      maxAim = hiAim = loAim = minAim = false;
      maxWill = hiWill = loWill = minWill = false;
      maxDef = hiDef = loDef = minDef = false;

      int inSquad = Squad.Count();
      //filteredSoldiers.InsertRange(0, Squad);

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
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderBy(x => x.Xp)] : [.. filteredSoldiers.OrderByDescending(x => x.Xp)];
              break;
          }
        }
      }

      double hiLo = 1 - (1 / (double)filteredSoldiers.Count) + indPctMobWill;
      double hiLoHigh = 1 - (1 / (double)filteredSoldiers.Count) + indPctAim;
      double hiLolow = 1 - (1 / (double)filteredSoldiers.Count) + indPctHp;
      double hiLoLower = 1 - (1 / (double)filteredSoldiers.Count) + indPctDef;

      foreach (Soldier f in filteredSoldiers)
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

        if (!f.IsDead && (!f.IsWounded || f.HoursOut <= RecoverableHrs))
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

        object[] dgvrVals =
        {
          f.LName,
          f.NName,
          (f.IsFatigued || f.IsWounded) && !f.IsDead ? ((f.HoursOut / 24) > 0 ? $"{f.HoursOut / 24}d " : "") + $"{f.HoursOut % 24}h" : f.Status,
          f.Class,
          f.Stats.Defense,
          f.Stats.HP,
          f.Stats.Mobility,
          $"{(f.Stats.Will == 99999 ? string.Empty : f.Stats.Will)}",
          f.Stats.Aim,
          f.Xp,
          $"{(f.ToNext == 99999 ? string.Empty : f.ToNext)}",
          f.RankName,
          filteredSoldierIndex + 1,
          f.RankId,
          f.Id,
          f.HasChecklistPerk,
          !(f.IsWounded && f.HoursOut > RecoverableHrs) && !f.IsDead
        };

        rosterGridView.Rows.Add(dgvrVals);
        DataGridViewRow thisRow = rosterGridView.Rows[filteredSoldierIndex];
        if (f.InSquad) thisRow.Cells[0].Style = thisRow.Cells[1].Style = new() { BackColor = squadRowBg, SelectionBackColor = squadRowBg, ForeColor = squadRowFg, SelectionForeColor = squadRowFg, };

        if (!f.IsShiv && f.IsWounded && f.HoursOut > RecoverableHrs)
        {
          thisRow.Cells["Aim"].Style =
          thisRow.Cells["Mob"].Style =
          thisRow.Cells["HP"].Style =
          thisRow.Cells["Will"].Style =
          thisRow.Cells["Def"].Style = StatWoundBgStyle
            ? new() { BackColor = woundBg, SelectionBackColor = woundBg, ForeColor = deadFg, SelectionForeColor = deadFg, Font = new(Font, FontStyle.Regular) }
            : new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["SoldierClass"].Style =
          thisRow.Cells["RankName"].Style =
          thisRow.Cells["XP"].Style =
          thisRow.Cells["Next"].Style = LongWoundBgStyle
            ? new() { BackColor = woundBg, SelectionBackColor = woundBg, ForeColor = deadFg, SelectionForeColor = deadFg, Font = new(Font, FontStyle.Regular) }
            : new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["Number"].Style =
            new() { BackColor = rosterGridView.BackgroundColor, SelectionBackColor = rosterGridView.BackgroundColor, ForeColor = deadFg, SelectionForeColor = deadFg, Font = SmallFont };
        }
        else if (f.IsShiv)
        {
          thisRow.Cells["Aim"].Style =
          thisRow.Cells["Mob"].Style =
          thisRow.Cells["HP"].Style =
          thisRow.Cells["Will"].Style =
          thisRow.Cells["Def"].Style =
            new() { BackColor = shivBg, SelectionBackColor = shivBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["Number"].Style =
            new() { BackColor = rosterGridView.BackgroundColor, SelectionBackColor = rosterGridView.BackgroundColor, ForeColor = deadFg, SelectionForeColor = deadFg, Font = SmallFont };
        }
        else
        {
          thisRow.Cells["Aim"].Style = new()
          {
            BackColor = maxAim ? maxBg : minAim ? minBg : hiAim ? hiBg : loAim ? loBg : gridCellBg,
            SelectionBackColor = maxAim ? maxBg : minAim ? minBg : hiAim ? hiBg : loAim ? loBg : gridCellBg,
            ForeColor = gridCellFg,
            SelectionForeColor = gridCellFg,
          };
          thisRow.Cells["Mob"].Style = new()
          {
            BackColor = maxMob ? maxBg : minMob ? minBg : hiMob ? hiBg : loMob ? loBg : gridCellBg,
            SelectionBackColor = maxMob ? maxBg : minMob ? minBg : hiMob ? hiBg : loMob ? loBg : gridCellBg,
            ForeColor = gridCellFg,
            SelectionForeColor = gridCellFg,
          };
          thisRow.Cells["HP"].Style = new()
          {
            BackColor = maxHp ? maxBg : minHp ? minBg : hiHp ? hiBg : loHp ? loBg : gridCellBg,
            SelectionBackColor = maxHp ? maxBg : minHp ? minBg : hiHp ? hiBg : loHp ? loBg : gridCellBg,
            ForeColor = gridCellFg,
            SelectionForeColor = gridCellFg,
          };
          thisRow.Cells["Will"].Style = new()
          {
            BackColor = maxWill ? maxBg : minWill ? minBg : hiWill ? hiBg : loWill ? loBg : gridCellBg,
            SelectionBackColor = maxWill ? maxBg : minWill ? minBg : hiWill ? hiBg : loWill ? loBg : gridCellBg,
            ForeColor = gridCellFg,
            SelectionForeColor = gridCellFg,
          };
          thisRow.Cells["Def"].Style = new()
          {
            BackColor = maxDef ? maxBg : minDef ? minBg : hiDef ? hiBg : loDef ? loBg : gridCellBg,
            SelectionBackColor = maxDef ? maxBg : minDef ? minBg : hiDef ? hiBg : loDef ? loBg : gridCellBg,
            ForeColor = gridCellFg,
            SelectionForeColor = gridCellFg,
          };
          thisRow.Cells["SoldierClass"].Style =
          thisRow.Cells["RankName"].Style =
          thisRow.Cells["XP"].Style =
          thisRow.Cells["Next"].Style =
            new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["Number"].Style =
            new() { BackColor = rosterGridView.BackgroundColor, SelectionBackColor = rosterGridView.BackgroundColor, ForeColor = deadFg, SelectionForeColor = deadFg, Font = SmallFont };
        }

        if (f.IsShiv) thisRow.DefaultCellStyle = new() { BackColor = shivBg, SelectionBackColor = shivBg };
        else if (f.IsDead) thisRow.Cells["Status"].Style = new() { BackColor = deadBg, SelectionBackColor = deadBg, ForeColor = deadFg, SelectionForeColor = deadFg };

        if (f.LName == "TamTam") thisRow.Cells["LName"].Value = $"TamTam ♥";

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
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg, Font = new(Font, FontStyle.Bold), ForeColor = gridCellFg, SelectionForeColor = gridCellFg };
          }
          else
          {
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg, ForeColor = deadFg, SelectionForeColor = deadFg };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg, ForeColor = gridCellFg, SelectionForeColor = gridCellFg };
          }
        }

        filteredSoldierIndex++;
        thisRow.Selected = (((thisRow.Cells["Id"].Value ?? "").ToString() == SoldierSelectedIdXP.Split(',').First()) && ((thisRow.Cells["XP"].Value ?? "").ToString() == SoldierSelectedIdXP.Split(',').Last()));
      }

      ResetChecklist();
      if (rosterGridView.SelectedRows.Count > 0) PopupateSoldierPerks(rosterGridView.SelectedRows?[0].Index ?? 0);
      else PopupateSoldierPerks(0);
    }

    private void BuildPerkTree()
    {
      RosterTree0.SubRoster = Roster;

      for (int i = 0; i < PerkList.Rows.Count; i++)
      {
        if (PerkList.Rows[i]["Enabled"].ToString() == "1")
        {
          Perk perk1 = new() { Id = (Int64.Parse(PerkList.Rows[i]["Id"].ToString() ?? "")), Name = PerkList.Rows[i]["Name"].ToString(), Type = 0 };

          RelatedPerk rp1 = new()
          {
            PerkTree = [perk1],
            PerkBranches = [],
            SubRoster = [.. Roster.Where(x =>
              x.Perks.Select(x => x.Name).Contains(perk1.Name)
              && !(x.IsDead && !deadCheckbox.Checked)
              && !(x.IsWounded && !woundedCheckbox.Checked)
              && !(x.IsShiv && !shivCheckbox.Checked)
              && !(x.IsFatigued && !fatiguedCheckbox.Checked)
              && !x.IsBlueshirt
            )]
          };
          RosterTree0.PerkBranches.Add(perk1, rp1);

          foreach (Soldier soldier1 in rp1.SubRoster)
          {
            foreach (Perk perk2 in soldier1.Perks.Where(x => x.Name != perk1.Name))
            {

              if (rp1.PerkBranches.ContainsKey(perk2)) continue;
              else
              {
                RelatedPerk rp2 = new()
                {
                  PerkTree = [perk1, perk2],
                  PerkBranches = [],
                  SubRoster = [.. rp1.SubRoster.Where(x
                    => x.Perks.Select(x => x.Name).Contains(perk1.Name)
                    && x.Perks.Select(x => x.Name).Contains(perk2.Name)
                  )]
                };
                rp1.PerkBranches.Add(perk2, rp2);

                foreach (Soldier soldier2 in rp2.SubRoster)
                {
                  foreach (Perk perk3 in soldier2.Perks.Where(x => x.Name != perk1.Name && x.Name != perk2.Name))
                  {
                    if (rp2.PerkBranches.ContainsKey(perk3)) continue;
                    else
                    {
                      RelatedPerk rp3 = new()
                      {
                        PerkTree = [perk1, perk2, perk3],
                        PerkBranches = [],
                        SubRoster = [.. rp2.SubRoster.Where(x
                          => x.Perks.Select(x => x.Name).Contains(perk1.Name)
                          && x.Perks.Select(x => x.Name).Contains(perk2.Name)
                          && x.Perks.Select(x => x.Name).Contains(perk3.Name)
                        )]
                      };
                      rp2.PerkBranches.Add(perk3, rp3);

                      foreach (Soldier soldier3 in rp3.SubRoster)
                      {
                        foreach (Perk perk4 in soldier3.Perks.Where(x => x.Name != perk1.Name && x.Name != perk2.Name && x.Name != perk3.Name))
                        {
                          if (rp3.PerkBranches.ContainsKey(perk4)) continue;
                          else rp3.PerkBranches.Add(perk4, new() { PerkTree = [], PerkBranches = [], SubRoster = [] });
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      RosterTree1 = RosterTree0;
    }

    //private void ResetRosterPerkList()
    //{
    //  rosterPerkList.Rows.Clear();
    //
    //  rosterPerkList.Rows.Add("~\\", 0);
    //  rosterPerkList.Rows[0].Height = rosterPerkList.Rows[0].Height + 4;
    //  rosterPerkList.Rows[0].DividerHeight += 4;
    //  rosterPerkList.Rows[0].Frozen = true;
    //  ListedRosterPerks = AllRosterPerks;
    //
    //  foreach (string p in PerkNames.OrderBy(x => x))
    //  {
    //    ListedRosterPerks.TryGetValue(p, out int perkCount);
    //    if (!nonRosterPerksCheckbox.Checked && perkCount == 0) continue;
    //    rosterPerkList.Rows.Add([p, perkCount]);
    //  }
    //}

    // this sets the title bar colors
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private void Rosterizer_Load(object sender, EventArgs e)
    {
      int trueValue = 0x01;

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
      int rosterCount = 0;
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
              Luck = 0,
              RosterNumber = ++rosterCount
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
            else thisSoldier.Luck = DateTime.Now.Microsecond;

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
                    thisSoldier.Stats.Will = thisSoldier.IsShiv ? 99999 : aStats[statIndex];
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

            thisSoldier.Luck *= (DateTime.Now.Microsecond % 102);

            // add the soldier to the roster
            roster.Add(thisSoldier);
          }
        }
      }

      // add 0-soldier perks to roster perks list
      foreach (DataRow row in PerkList.Rows) if (row["Enabled"].ToString() == "1") ListedRosterPerks.TryAdd(row["Name"].ToString() ?? "", 0);
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

    private void PopupateSoldierPerks(int rosterRowIndex, DataGridView? sender = null)
    {
      bool fromSquad = false;

      if (sender is not null && sender.Name == "squadGridView") fromSquad = true;
      else if (rosterRowIndex >= rosterGridView.RowCount) return;

      DataGridViewRow r;
      r = fromSquad ? squadGridView.Rows[rosterRowIndex] : rosterGridView.Rows[rosterRowIndex];

      if (r is not null)
      {
        soldierPerksGridView.Rows.Clear();
        Soldier? s = Roster.FirstOrDefault(x => ((r.Cells[0].Value ?? "").ToString() ?? "").Contains(x.LName));
        soldierPerksGridView.Rows.Add($"{s.LName} - {s.RankName}");
        soldierPerksGridView.Rows[0].Cells[0].Style = new()
        {
          BackColor = soldierTabUnselectedBg,
          ForeColor = soldierRowFg,
          SelectionBackColor = soldierTabUnselectedBg,
          SelectionForeColor = soldierRowFg,
          Font = new(Font, FontStyle.Regular),
          Alignment = DataGridViewContentAlignment.MiddleCenter
        };

        s.Perks = s.Perks.OrderBy(x => x.Name).ToList();
        for (int i = 0; i < s.Perks.Count; i++)
        {
          soldierPerksGridView.Rows.Add([s.Perks[i].Name ?? ""]);
          if (ChecklistPerks.ContainsKey(s.Perks[i].Name ?? "")) soldierPerksGridView.Rows[i + 1].Cells[0].Style = new()
          {
            BackColor = checklistGoodTabUnselectedBg,
            ForeColor = checklistGoodHeaderFg,
            SelectionBackColor = soldierTabUnselectedBg,
            SelectionForeColor = soldierRowFg,
            Font = new(Font, FontStyle.Regular),
          };
        }
        if (fromSquad)
        {
          SoldierSelectedIdXP = $"{squadGridView.Rows[rosterRowIndex].Cells[12].Value},{squadGridView.Rows[rosterRowIndex].Cells[8].Value}";
          squadGridView.Rows[rosterRowIndex].Selected = true;
        }
        else
        {
          SoldierSelectedIdXP = $"{rosterGridView.Rows[rosterRowIndex].Cells["Id"].Value},{rosterGridView.Rows[rosterRowIndex].Cells["XP"].Value}";
          rosterGridView.Rows[rosterRowIndex].Selected = true;
        }
      }
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

    private void deadCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      switch (RosterPerkTree.Length)
      {
        case 0:
          ListRoster(RosterTree1.SubRoster);
          break;
        case 1:
          ListRoster(RosterTree2.SubRoster);
          break;
        case 2:
          ListRoster(RosterTree3.SubRoster);
          break;
        case 3:
          ListRoster(RosterTree4.SubRoster);
          break;
      }
    }

    private void woundedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      switch (RosterPerkTree.Length)
      {
        case 0:
          ListRoster(RosterTree1.SubRoster);
          break;
        case 1:
          ListRoster(RosterTree2.SubRoster);
          break;
        case 2:
          ListRoster(RosterTree3.SubRoster);
          break;
        case 3:
          ListRoster(RosterTree4.SubRoster);
          break;
      }
    }

    private void shivCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      switch (RosterPerkTree.Length)
      {
        case 0:
          ListRoster(RosterTree1.SubRoster);
          break;
        case 1:
          ListRoster(RosterTree2.SubRoster);
          break;
        case 2:
          ListRoster(RosterTree3.SubRoster);
          break;
        case 3:
          ListRoster(RosterTree4.SubRoster);
          break;
      }
    }
    private void fatiguedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      switch (RosterPerkTree.Length)
      {
        case 0:
          ListRoster(RosterTree1.SubRoster);
          break;
        case 1:
          ListRoster(RosterTree2.SubRoster);
          break;
        case 2:
          ListRoster(RosterTree3.SubRoster);
          break;
        case 3:
          ListRoster(RosterTree4.SubRoster);
          break;
      }
    }

    private void squadAndRosterGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (e.ColumnIndex == 12)
      {
        e.Graphics.FillRectangle(new SolidBrush(rosterGridView.BackgroundColor), new(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height));

        TextRenderer.DrawText(
          e.Graphics,
          string.Format("{0}", e.FormattedValue),
          SmallFont,
          e.CellBounds,
          rosterGridView.DefaultCellStyle.BackColor,
          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding
        );
        e.Handled = true;
      }
      else if (e.RowIndex >= 0)
      {
        // draw dot indicating soldier has checklist perk 
        if (e.ColumnIndex == 0)
        {
          if ((bool?)((DataGridView)sender).Rows[e.RowIndex].Cells[15].Value == true)
          {
            Rectangle r = new(e.CellBounds.X - 11, e.CellBounds.Y + 10, 5, 5);

            e.PaintBackground(e.CellBounds, false);
            e.Graphics.DrawEllipse(new Pen(checklistGoodTabUnselectedBg, 5), r);
            TextRenderer.DrawText(
              e.Graphics,
              string.Format("{0}", e.FormattedValue),
              Font,
              e.CellBounds,
              ((DataGridView)sender).DefaultCellStyle.SelectionForeColor,
              TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding
            );
            e.Handled = true;
          }
        }
        else if (e.ColumnIndex == 3)
        {
          Color classColor = Color.FromArgb(100, 0,0,0);

          switch (((e.FormattedValue ?? "").ToString() ?? "").ToLower())
          {
            case "assault":
              classColor = Color.FromArgb(100, 222, 33, 33);
              break;
            case "engineer":
              classColor = Color.FromArgb(74, 245, 115, 131);
              break;
            case "gunner":
              classColor = Color.FromArgb(174, 171, 212, 141);
              break;
            case "infantry":
              classColor = Color.FromArgb(100, 99, 99, 173);
              break;
            case "medic":
              classColor = Color.FromArgb(200, 255, 255, 255);
              break;
            case "rocketeer":
              classColor = Color.FromArgb(100, 80, 180, 180);
              break;
            case "scout":
              classColor = Color.FromArgb(100, 98, 140, 112);
              break;
            case "shiv":
              classColor = Color.FromArgb(100, 0, 0, 0);
              break;
            case "sniper":
              classColor = Color.FromArgb(11, 55, 0, 55);
              break;

          }

        Rectangle r = new(e.CellBounds.X + 16, e.CellBounds.Y + 12, 46, 1);

          e.PaintBackground(e.CellBounds, false);
          e.Graphics.DrawRectangle(new Pen(classColor, 10), r);
          e.Handled = true;
          TextRenderer.DrawText(
            e.Graphics,
            string.Format("{0}", e.FormattedValue),
            Font,
            e.CellBounds,
            ((DataGridView)sender).DefaultCellStyle.SelectionForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
        }
      }

    }

    private void rosterGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        // reset sorting
        if (e.RowIndex == -1)
        {
          DefaultSorting();
          switch (RosterPerkTree.Length)
          {
            case 0:
              ListRoster(RosterTree1.SubRoster);
              break;
            case 1:
              ListRoster(RosterTree2.SubRoster);
              break;
            case 2:
              ListRoster(RosterTree3.SubRoster);
              break;
            case 3:
              ListRoster(RosterTree4.SubRoster);
              break;
          }
        }
        else
        {
          ToggleInSquad(sender, e.RowIndex);
          if ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["IsAvailable"].Value == true) ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
        }
      }
      else if (e.Button == MouseButtons.Left)
      {
        if (e.RowIndex == -1)
        {
          if (e.ColumnIndex > -1 && e.ColumnIndex < 12)
          {
            DoSorting(e.ColumnIndex);
            switch (RosterPerkTree.Length)
            {
              case 0:
                ListRoster(RosterTree1.SubRoster);
                break;
              case 1:
                ListRoster(RosterTree2.SubRoster);
                break;
              case 2:
                ListRoster(RosterTree3.SubRoster);
                break;
              case 3:
                ListRoster(RosterTree4.SubRoster);
                break;
            }
          }
        }
        else PopupateSoldierPerks(e.RowIndex);
      }
    }

    private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
    {
      tabControl1.SuspendLayout();
      Rectangle rec = tabControl1.ClientRectangle;
      StringFormat StrFormat = new()
      {
        LineAlignment = StringAlignment.Center,
        Alignment = StringAlignment.Center
      };

      SolidBrush backColor = new(windowBg);
      SolidBrush fontColor;
      e.Graphics.FillRectangle(backColor, rec);

      for (int i = 0; i < tabControl1.TabPages.Count; i++)
      {
        bool bSelected = (tabControl1.SelectedIndex == i);
        Rectangle recBounds = tabControl1.GetTabRect(i);
        RectangleF tabTextArea = (RectangleF)tabControl1.GetTabRect(i);

        if (i == 0) // tree
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(treeHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(treeHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(treeTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(treeHeaderFg), tabTextArea, StrFormat);
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
              e.Graphics.DrawRectangle(new Pen(checklistBadTabHighlight, 3), r);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font, FontStyle.Regular), new SolidBrush(checklistBadHeaderFg), tabTextArea, StrFormat);
            }
            else
            {
              e.Graphics.FillRectangle(new SolidBrush(checklistBadTabUnselectedBg), r);
              e.Graphics.DrawRectangle(new Pen(checklistBadTabHighlight, 3), r);
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
      }
      tabControl1.ResumeLayout();
    }

    private void rosterGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        tabControl1.SelectedIndex = 2;
      }
    }

    private void checklistGridView_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        SearchPerkTree((DataGridView)sender, e.RowIndex, true);
      }
      else if (e.Button == MouseButtons.Right)
      {
        SetPerkTree(RosterTree0);
        ListRoster();
      }
    }

    private void timerStartStopButton_Click(object sender, EventArgs e)
    {
      timer1.Interval = 500;
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
      timerLabel.ForeColor = windowFg;
      scoreLabel.Text = "";
      ChecklistPassTime = 0;
      Squad = [];
      foreach (Soldier s in Roster) s.InSquad = false;

      CurrentSquadSize = 0;
      tabControl1.SelectedIndex = 0;
      HideSquadGrid();
      ListRoster();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (DateTime.Now.Second % 60 == 58) timerLabel.Text = "!slow";
      else timerLabel.Text = elapsedTime.ToString("G")[..10];
      
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
        timerLabel.ForeColor = windowFg;
        elapsedTime = timer2.Elapsed;
      }
    }

    private void trackBar1_ValueChanged(object sender, EventArgs e)
    {
      BlueshirtLvl = ((TrackBar)sender).Value;
      Roster.ForEach(x => x.IsBlueshirt = x.RankId <= BlueshirtLvl);
      if (tabControl1.SelectedIndex == 4) // options
      {
        switch (RosterPerkTree.Length)
        {
          case 0:
            ListRoster(RosterTree1.SubRoster);
            break;
          case 1:
            ListRoster(RosterTree2.SubRoster);
            break;
          case 2:
            ListRoster(RosterTree3.SubRoster);
            break;
          case 3:
            ListRoster(RosterTree4.SubRoster);
            break;
        }
      }
    }

    private void trackBar2_ValueChanged(object sender, EventArgs e)
    {
      SquadSize = ((TrackBar)sender).Value;
      Roster.ForEach(x => x.InSquad = false);
      Squad = [];
      switch (RosterPerkTree.Length)
      {
        case 0:
          ListRoster(RosterTree1.SubRoster);
          break;
        case 1:
          ListRoster(RosterTree2.SubRoster);
          break;
        case 2:
          ListRoster(RosterTree3.SubRoster);
          break;
        case 3:
          ListRoster(RosterTree4.SubRoster);
          break;
      }
    }

    private void squadPerkList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        SearchPerkTree((DataGridView)sender, e.RowIndex, true);
      }
    }

    private void squadPerkList_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        ListRoster();
      }
    }

    private void soldierPerksGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        Tuple<string, long> selectedSoldier = new("", 0);

        // get soldier in focus
        for (int i = 0; i < rosterGridView.Rows.Count; i++)
        {
          if (((((DataGridView)sender).Rows[0].Cells[0].Value ?? "").ToString() ?? "").Contains(((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "")))
          {
            selectedSoldier = new(((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? ""), (long)(rosterGridView.Rows[i].Cells[9].Value ?? -1));
            break;
          }
        }

        // filter perk tree to selected perk
        SearchPerkTree((DataGridView)sender, e.RowIndex, true);

        // reset selection to focused soldier
        for (int i = 0; i < rosterGridView.Rows.Count; i++)
        {
          if (((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "") == selectedSoldier.Item1
            && ((long)(rosterGridView.Rows[i].Cells[9].Value ?? -2)) == selectedSoldier.Item2)
          {
            rosterGridView.Select();
            rosterGridView.ClearSelection();
            rosterGridView.Rows[i].Selected = true;
            break;
          }
        }
      }
      else if (e.Button == MouseButtons.Right)
      {
        ListRoster();
      }
    }

    private void darkCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      darkMode = darkCheckbox.Checked;
      SetColor();
    }

    private void SearchPerkTree(DataGridView sender, int clickedIndex, bool singleLvl = false)
    {
      int treeIndex = 0;
      string clicked = (sender.Rows[clickedIndex].Cells[0].Value ?? "").ToString() ?? "";
      string soldier = sender.Name == "soldierPerksGridView" ? (sender.Rows[0].Cells[0].Value ?? "").ToString() ?? "" : "";
      if (!string.IsNullOrWhiteSpace(soldier)) soldier = soldier[..(soldier.IndexOf('-') - 1)];
      int vscroll = sender.FirstDisplayedScrollingRowIndex;

      for (int i = 0; i < treeDataGrid.Rows.Count; i++)
      {
        if (((treeDataGrid.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(clicked))
        {
          treeIndex = i;
          break;
        }
      }

      if (clicked.Trim().StartsWith('\\'))
      {
        switch (treeIndex)
        {
          case 0:
            SetPerkTree(RosterTree1);
            for (int i = 1; i <= 3; i++) if (((treeDataGrid.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains('\\')) treeDataGrid.Rows.RemoveAt(i);

            treeDataGrid.Rows[0].Frozen = true;
            treeDataGrid.Rows[0].DefaultCellStyle = new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg };
            treeDataGrid.Rows[0].Height = treeDataGrid.Rows[0].Height + 4;
            treeDataGrid.Rows[0].DividerHeight += 4;


            ListRoster(RosterTree1.SubRoster);
            return;

          case 1:
            SetPerkTree(RosterTree2);
            for (int i = 2; i <= 3; i++) if (((treeDataGrid.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains('\\')) treeDataGrid.Rows.RemoveAt(i);

            for (int i = 0; i <= 1; i++)
            {
              treeDataGrid.Rows[i].DefaultCellStyle = new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg };
              treeDataGrid.Rows[i].Frozen = true;
            }
            treeDataGrid.Rows[1].Height = treeDataGrid.Rows[1].Height + 4;
            treeDataGrid.Rows[1].DividerHeight += 4;

            ListRoster(RosterTree2.SubRoster);

            return;

          case 2:
            SetPerkTree(RosterTree3);
            if (((treeDataGrid.Rows[3].Cells[0].Value ?? "").ToString() ?? "").Contains('\\')) treeDataGrid.Rows.RemoveAt(3);

            for (int i = 0; i <= 2; i++)
            {
              treeDataGrid.Rows[i].DefaultCellStyle = new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg };
              treeDataGrid.Rows[i].Frozen = true;
            }
            treeDataGrid.Rows[2].Height = treeDataGrid.Rows[2].Height + 4;
            treeDataGrid.Rows[2].DividerHeight += 4;

            ListRoster(RosterTree3.SubRoster);
            return;

          case 3:
            SetPerkTree(RosterTree4);
            for (int i = 0; i <= 3; i++)
            {
              treeDataGrid.Rows[i].DefaultCellStyle = new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg };
              treeDataGrid.Rows[i].Frozen = true;
            }
            treeDataGrid.Rows[3].Height = treeDataGrid.Rows[3].Height + 4;
            treeDataGrid.Rows[3].DividerHeight += 4;

            ListRoster(RosterTree4.SubRoster);
            return;
        }
      }
      int perkTreeLvl = singleLvl ? 0 : RosterPerkTree.Length;
      if ((sender.Name == "squadPerkList" && clicked.StartsWith("Soldiers:")) || (sender.Name == "soldierPerksGridView" && clickedIndex == 0)) return;

      switch (perkTreeLvl)
      {
        case 0:
          RosterTree2 = RosterTree1.PerkBranches.First(x => x.Key.Name == clicked).Value;
          SetPerkTree(RosterTree2);
          break;
        case 1:
          foreach (var b in RosterTree1.PerkBranches)
          {
            if (b.Value.PerkTree == RosterPerkTree)
            {
              RosterTree3 = b.Value.PerkBranches.First(x => x.Value.PerkTree.Last().Name == clicked || x.Key.Name == clicked).Value;
              SetPerkTree(RosterTree3);
            }
          }
          break;
        case 2:
          foreach (var b in RosterTree1.PerkBranches.OrderBy(x => x.Key.Name))
          {
            if (b.Value.PerkTree.SequenceEqual(RosterPerkTree.Take(1)))
            {
              foreach (var c in b.Value.PerkBranches.OrderBy(x => x.Key.Name))
              {
                if (c.Value.PerkTree.SequenceEqual(RosterPerkTree))
                {
                  RosterTree4 = c.Value.PerkBranches.First(x => x.Value.PerkTree.Last().Name == clicked || x.Key.Name == clicked).Value;
                  SetPerkTree(RosterTree4);
                }
              }
            }
          }
          break;
        case 3:
          foreach (var b in RosterTree1.PerkBranches.OrderBy(x => x.Key.Name))
          {
            if (b.Value.PerkTree.SequenceEqual(RosterPerkTree.Take(1)))
            {
              foreach (var c in b.Value.PerkBranches.OrderBy(x => x.Key.Name))
              {
                if (c.Value.PerkTree.SequenceEqual(RosterPerkTree.Take(2)))
                {
                  foreach (var d in c.Value.PerkBranches.OrderBy(x => x.Key.Name))
                  {
                    if (d.Value.PerkTree == RosterPerkTree)
                    {
                      SetPerkTree(d.Value.PerkBranches.First(x => x.Value.PerkTree.Last().Name == clicked || x.Key.Name == clicked).Value);
                    }
                  }
                }
              }
            }
          }
          break;
      }

      if (sender.Name == "soldierPerksGridView" && !string.IsNullOrWhiteSpace(soldier))
      {
        Tuple<string, long> selectedSoldier = new("", 0);
        for (int i = 0; i < rosterGridView.Rows.Count; i++)
        {
          if (((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(soldier))
          {
            PopupateSoldierPerks(i);
            break;
          }
        }
      }
      if (vscroll > 0 && sender.Rows.Count >= vscroll && !sender.Rows[vscroll].Frozen) sender.FirstDisplayedScrollingRowIndex = vscroll;
    }

    private void treeDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        SearchPerkTree((DataGridView)sender, e.RowIndex);
      }
    }

    private void treeDataGrid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        switch (RosterPerkTree.Length)
        {
          case 1:
            SetPerkTree(RosterTree1);
            break;
          case 2:
            SetPerkTree(RosterTree2);
            break;
          case 3:
            SetPerkTree(RosterTree3);
            break;
        }
      }

      if (e.Button == MouseButtons.Middle)
      {
        PerkTreeByName = !PerkTreeByName;
        treeDataGrid.CellBorderStyle = PerkTreeByName ? DataGridViewCellBorderStyle.Single : DataGridViewCellBorderStyle.Raised;
        switch (RosterPerkTree.Length)
        {
          case 0:
            SetPerkTree(RosterTree1);
            ListRoster(RosterTree1.SubRoster);
            break;
          case 1:
            SetPerkTree(RosterTree2);
            ListRoster(RosterTree2.SubRoster);
            break;
          case 2:
            SetPerkTree(RosterTree3);
            ListRoster(RosterTree3.SubRoster);
            break;
          case 3:
            SetPerkTree(RosterTree4);
            ListRoster(RosterTree4.SubRoster);
            break;
        }
      }
    }

    private void TreeSearch(string searchTerm)
    {
      if (!string.IsNullOrWhiteSpace(searchTerm))
      {
        RelatedPerk RosterTreeFiltered = new()
        {
          PerkTree = [],
          PerkBranches = [],
          SubRoster = []
        }; ;

        switch (RosterPerkTree.Length)
        {
          case 0:
            foreach (var perkBranch in RosterTree1.PerkBranches)
            {
              if ((perkBranch.Key.Name ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
              {
                RosterTreeFiltered.PerkBranches.Add(perkBranch.Key, perkBranch.Value);
                foreach (Soldier s in perkBranch.Value.SubRoster) if (!RosterTreeFiltered.SubRoster.Contains(s)) RosterTreeFiltered.SubRoster.Add(s);
              }
            }
            break;
          case 1:
            foreach (var perkBranch in RosterTree2.PerkBranches)
            {
              if ((perkBranch.Key.Name ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
              {
                RosterTreeFiltered.PerkBranches.Add(perkBranch.Key, perkBranch.Value);
                foreach (Soldier s in perkBranch.Value.SubRoster) if (!RosterTreeFiltered.SubRoster.Contains(s)) RosterTreeFiltered.SubRoster.Add(s);
              }
            }
            break;
          case 2:
            foreach (var perkBranch in RosterTree3.PerkBranches)
            {
              if ((perkBranch.Key.Name ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
              {
                RosterTreeFiltered.PerkBranches.Add(perkBranch.Key, perkBranch.Value);
                foreach (Soldier s in perkBranch.Value.SubRoster) if (!RosterTreeFiltered.SubRoster.Contains(s)) RosterTreeFiltered.SubRoster.Add(s);
              }
            }
            break;
          case 3:
            foreach (var perkBranch in RosterTree4.PerkBranches)
            {
              if ((perkBranch.Key.Name ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
              {
                RosterTreeFiltered.PerkBranches.Add(perkBranch.Key, perkBranch.Value);
                foreach (Soldier s in perkBranch.Value.SubRoster) if (!RosterTreeFiltered.SubRoster.Contains(s)) RosterTreeFiltered.SubRoster.Add(s);

              }
            }
            break;
        }

        SetPerkTree(RosterTreeFiltered);
      }
    }

    private void treeSearchTextbox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        TreeSearch(((TextBox)sender).Text);
        ((TextBox)sender).Text = "";
        e.Handled = true;
        return;
      }
    }



    private void rosterGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // set 'no' cursor for soldiers which can't be added to squad
      if (e.RowIndex >= 0) ((DataGridView)sender).Cursor = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["IsAvailable"].Value) ? Cursors.Default : Cursors.No;
      else ((DataGridView)sender).Cursor = Cursors.Default;
    }

    private void treeDataGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track perk list
      ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle = (((DataGridView)sender).Rows[e.RowIndex].Selected == true) ?
          new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg }
        : new() { ForeColor = treeHeaderFg, SelectionForeColor = treeHeaderFg, BackColor = treeHeaderBg, SelectionBackColor = treeHeaderBg };
    }

    private void treeDataGrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track squad perk list
      ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle = (((DataGridView)sender).Rows[e.RowIndex].Selected == true
        || ((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Trim().StartsWith('\\')) ?
            new() { BackColor = treeTabUnselectedBg, ForeColor = treeHeaderFg, SelectionBackColor = treeTabUnselectedBg, SelectionForeColor = treeHeaderFg }
          : new() { ForeColor = gridCellFg, SelectionForeColor = gridCellFg, BackColor = gridCellBg, SelectionBackColor = gridCellBg };
    }

    private void squadPerkList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track squad perk list
      List<Tuple<string, long>> perkedSquadSoldiers = [];

      for (int i = 0; i < Squad.Count; i++)
        if (Squad[i].Perks.Select(x => x.Name).Contains((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
          perkedSquadSoldiers.Add(new(Squad[i].LName, Squad[i].Xp));

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new(((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? ""), Int64.Parse(((squadGridView.Rows[i].Cells[9].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = new()
          {
            ForeColor = deadFg,
            SelectionForeColor = deadFg,
            BackColor = squadHeaderBg,
            SelectionBackColor = squadHeaderBg
          };
        else squadGridView.Rows[i].Cells[0].Style = new()
        {
          ForeColor = gridCellFg,
          SelectionForeColor = gridCellFg,
          BackColor = gridCellBg,
          SelectionBackColor = gridCellBg
        };
      }

      if (e.RowIndex == 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style.BackColor == checklistGoodTabUnselectedBg) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = new()
      {
        ForeColor = deadFg,
        SelectionForeColor = deadFg,
        BackColor = squadHeaderBg,
        SelectionBackColor = squadHeaderBg
      };
    }

    private void squadPerkList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg };
      }
      if (e.RowIndex == 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style.BackColor == checklistGoodTabUnselectedBg) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = new()
      {
        ForeColor = gridCellFg,
        SelectionForeColor = gridCellFg,
        BackColor = squadRowBg,
        SelectionBackColor = squadRowBg
      };
    }

    private void soldierPerkList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      List<Tuple<string, long>> perkedSquadSoldiers = [];

      for (int i = 0; i < Squad.Count; i++)
        if (Squad[i].Perks.Select(x => x.Name).Contains((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
          perkedSquadSoldiers.Add(new(Squad[i].LName, Squad[i].Xp));

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new(((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? ""), Int64.Parse(((squadGridView.Rows[i].Cells[9].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = new()
          {
            ForeColor = deadFg,
            SelectionForeColor = deadFg,
            BackColor = squadHeaderBg,
            SelectionBackColor = squadHeaderBg
          };
        else squadGridView.Rows[i].Cells[0].Style = new()
        {
          ForeColor = gridCellFg,
          SelectionForeColor = gridCellFg,
          BackColor = gridCellBg,
          SelectionBackColor = gridCellBg
        };
      }

      if (e.RowIndex == 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style.BackColor == checklistGoodTabUnselectedBg) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = new()
      {
        ForeColor = deadFg,
        SelectionForeColor = deadFg,
        BackColor = squadHeaderBg,
        SelectionBackColor = squadHeaderBg
      };
    }

    private void soldierPerkList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg };
      }

      if (e.RowIndex == 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style.BackColor == checklistGoodTabUnselectedBg) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = new()
      {
        ForeColor = gridCellFg,
        SelectionForeColor = gridCellFg,
        BackColor = gridCellBg,
        SelectionBackColor = gridCellBg
      };
    }

    private void checklistGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      List<Tuple<string, long>> perkedSquadSoldiers = [];

      for (int i = 0; i < Squad.Count; i++)
        if (Squad[i].Perks.Select(x => x.Name).Contains((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
          perkedSquadSoldiers.Add(new(Squad[i].LName, Squad[i].Xp));

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new(((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? ""), Int64.Parse(((squadGridView.Rows[i].Cells[9].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = new()
          {
            ForeColor = deadFg,
            SelectionForeColor = deadFg,
            BackColor = squadHeaderBg,
            SelectionBackColor = squadHeaderBg
          };
        else squadGridView.Rows[i].Cells[0].Style = new()
        {
          ForeColor = gridCellFg,
          SelectionForeColor = gridCellFg,
          BackColor = gridCellBg,
          SelectionBackColor = gridCellBg
        };
      }

      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = new()
      {
        ForeColor = deadFg,
        SelectionForeColor = deadFg,
        BackColor = squadHeaderBg,
        SelectionBackColor = squadHeaderBg
      };
    }

    private void checklistGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = new() { BackColor = gridCellBg, SelectionBackColor = gridCellBg };
      }

      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = new()
      {
        ForeColor = gridCellFg,
        SelectionForeColor = gridCellFg,
        BackColor = gridCellBg,
        SelectionBackColor = gridCellBg
      };
    }

    private void squadGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        PopupateSoldierPerks(e.RowIndex, (DataGridView)sender);
      }
      else if (e.Button == MouseButtons.Right)
      {
        int vscroll = ((DataGridView)sender).FirstDisplayedScrollingRowIndex;
        ToggleInSquad(sender, e.RowIndex);
        if (vscroll > 0 && ((DataGridView)sender).Rows.Count >= vscroll - 1) ((DataGridView)sender).FirstDisplayedScrollingRowIndex = vscroll;
      }
    }

    private void squadGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        tabControl1.SelectedIndex = 2;
      }
    }
  }
}
