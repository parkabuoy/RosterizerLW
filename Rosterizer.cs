using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO.Hashing;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RosterizerLW.Properties;

namespace RosterizerLW
{
  public partial class Rosterizer : Form
  {
    // muted color scheme
    static bool darkMode = true;

    static readonly DataGridViewCellBorderStyle rosterCellBorders = DataGridViewCellBorderStyle.None;
    static readonly DataGridViewCellBorderStyle soldierCellBorders = DataGridViewCellBorderStyle.SingleHorizontal;
    static readonly DataGridViewCellBorderStyle checklistCellBorders = DataGridViewCellBorderStyle.SingleHorizontal;
    static readonly DataGridViewCellBorderStyle shortlistGridCellBorders = DataGridViewCellBorderStyle.None;
    static DataGridViewCellStyle hotStyle = new();
    static readonly bool LongWoundBgStyle = false;
    static readonly bool StatWoundBgStyle = false;

    private Stopwatch timer2 = new();
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private bool timerRunning = false;

    public static AppConfig _AppConfig;
    public static bool _HasArgs;
    public static bool _ValidArgs;
    public static List<string> _Args;
    public static List<string> ConsoleErrors;
    public static List<string> ConsoleMsgs;
    public static bool FromSquadTab = false;
    public static List<Soldier> TabFlipList = [];
    public static string TabFlipFilterLabelText = "";
    public static int TabFlipVscroll = 0;
    public static List<Soldier> Roster = [];
    public static List<Soldier> Shortlist = [];
    //public static FileInfo SaveFile;
    public static JsonRoot SaveParsed;
    public static DataTable PerkList;
    public static DataTable ChecklistPerksDatatable;
    public static DataTable AbilitiesDatatable;
    public static List<string> PerkNames;
    public static Dictionary<string, int> SquadPerks = [];
    public static Dictionary<string, int> ListedRosterPerks = [];
    public static Dictionary<string, int> AllRosterPerks = [];

    public static int RecoverableHrs = 8;
    public static int BlueshirtLvl = 3;
    public static long[] XpLvls = [120, 350, 700, 1200, 2000, 3000, 4200]; // xp levels per DefaultGameCore.ini ~ln. 900
    public static int MinSquadSize = 6;
    public static int MaxSquadSize = 16;
    public static int SquadSize = 8;
    public static int CurrentShortlistSize = 0;
    public static int Score = 0;
    public static int CurrentSquadSize = 0;
    public static List<List<long>> SquadStats = [];
    public static List<List<long>> RosterStats = [];
    public static Dictionary<string, int> ChecklistPerks = [];
    public static bool ChecklistPass = false;
    public static long ChecklistPassTime = 0;
    public static List<List<int>> SortArray = [[]];
    public static bool? ChatGemActivated = false;

    public static int[] DefaultSortArray = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0];
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
    bool SquadPerksByName = true;

    Perk[] RosterPerkTree = [];
    // path from which we'll load the save (or blank to load AutoSavePath below
    static string overrideSavePath = "..\\..\\..\\saveBackup\\save43";
    // xcom save dir from which the most recent save file will be selected
    string AutoSavePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\XCOM - Enemy Within\\XComGame\\SaveData";
    static string OutputDir = "..\\..\\..\\output\\"; // the output dir
    static string BackupDir = "..\\..\\..\\saveBackup\\"; // path where saves will be backed up
    static string PerkListPath = "..\\..\\..\\csv\\Long War ID reference - Perks.csv";
    static string AbilitiesListPath = "..\\..\\..\\csv\\Long War ID reference - Abilities.csv";
    static string PerkChecklistPath = "..\\..\\..\\csv\\Checklist - Perks.csv";
    public static FileInfo SaveFile;
    public static string SoldierSelectedIdXP = "";

    // ------------------------------------------------------------------------------------------------------------------------------------------------------------
    string saveFilenameFull = "";
    string saveFilename = "";
    string jsonFilenameFull = "";
    string saveNameRegex = "^save\\d{1,3}\\Z"; // regex: starts with "save", has 1-3 numbers after it, then ends
    string todayBackupDir = Path.Combine(BackupDir, $"{DateTime.Now:yyyyMMdd}");
    string todayOutputDir = Path.Combine(OutputDir, $"{DateTime.Now:yyyyMMdd}");

    string execTime = $"{DateTime.Now:yyyyMMdd.HHmm}";

    FileInfo x2jFile = new(AppConfig.Xcom2JsonPath);
    UInt64 hash = Crc64.HashToUInt64(File.ReadAllBytes(AppConfig.Xcom2JsonPath));

    // import seven-segment font
    [DllImport("gdi32.dll")]
    private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
        IntPtr pdv, [In] ref uint pcFonts);
    private PrivateFontCollection fonts = new();

    Font SevenSegmentFont;


    public Rosterizer(AppConfig config)
    {
      InitializeComponent();

      byte[] fontData2 = Resources.sony_7_segment;
      IntPtr fontPtr2 = Marshal.AllocCoTaskMem(fontData2.Length);
      Marshal.Copy(fontData2, 0, fontPtr2, fontData2.Length);
      uint dummy2 = 0;
      fonts.AddMemoryFont(fontPtr2, Resources.sony_7_segment.Length);
      AddFontMemResourceEx(fontPtr2, (uint)Resources.sony_7_segment.Length, IntPtr.Zero, ref dummy2);
      Marshal.FreeCoTaskMem(fontPtr2);

      SevenSegmentFont = new Font(fonts.Families[0], 10.0F, FontStyle.Bold);

      if (!x2jFile.Exists || hash != AppConfig.Xcom2JsonHash) ShowError("invalid xcom2json exe!", x2jFile.FullName);

      SaveFile ??= new(overrideSavePath);
      // build datatable out of csv file (directly copied from swf's id reference sheets)
      PerkList ??= ConvertCSVtoDataTable(PerkListPath);
      ChecklistPerksDatatable ??= ConvertCSVtoDataTable(PerkChecklistPath);
      AbilitiesDatatable ??= ConvertCSVtoDataTable(AbilitiesListPath);

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
      Process.Start(new ProcessStartInfo()
      {
        FileName = "cmd",
        Arguments = $"/C {AppConfig.Xcom2JsonPath} -o \"{jsonFilenameFull}\" \"{saveFilenameFull}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
      })?.WaitForExit();

      // if parsing failed, cry
      if (!File.Exists(jsonFilenameFull)) ShowError("json parsing failure!", saveFilenameFull);

      PopulatePerkList();

      string rawJson = File.ReadAllText(jsonFilenameFull);
      SaveParsed = JsonConvert.DeserializeObject<JsonRoot>(rawJson) ?? new() { Actor_table = [], Checkpoints = [], Header = new() };
      Roster = [.. InitializeRoster(SaveParsed).OrderByDescending(x => x.Xp)];

      DefaultSorting();
      //SetRosterPerks();
      squadGridView.Size = new Size(squadGridView.Width, 0);

      // set colors
      SetColor();

      scoreLabel.Font = Font;
      scoreLabel.ForeColor = SystemColors.GrayText;
      scoreLabel.BackColor = AppConfig.WindowBg;

      scorePointsLabel.Font = Font;
      scorePointsLabel.ForeColor = SystemColors.GrayText;
      scorePointsLabel.BackColor = AppConfig.WindowBg;

      Text = $"{SaveFile?.Name}  -  {(SaveParsed.Header.Save_description ?? new()).Str}";

      minLvlCombx.Items.Clear();
      minLvlCombx.Items.AddRange([0, 1, 2, 3, 4, 5, 6, 7]);
      minLvlCombx.SelectedItem = BlueshirtLvl;

      squadSizeCombx.Items.Clear();
      squadSizeCombx.Items.AddRange([6, 7, 8, 9, 10, 11, 12]);
      squadSizeCombx.SelectedItem = 8;

      squadSizeCombx.SelectedItem = SquadSize;
      treeDataGrid.CellBorderStyle = PerkTreeByName ? DataGridViewCellBorderStyle.SingleHorizontal : DataGridViewCellBorderStyle.Single;

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
      SquadStats = [];
      tableLayoutPanel1.RowStyles[0].Height = 3;
      tableLayoutPanel1.SetRow(squadGridView, 1);
      tableLayoutPanel1.SetRow(rosterGridView, 0);
      tableLayoutPanel1.SetRowSpan(rosterGridView, 2);
    }

    public void SetColor()
    {
      Application.SetColorMode(AppConfig.DarkMode ? SystemColorMode.Dark : SystemColorMode.Classic);

      rosterGridView.ColumnHeadersDefaultCellStyle.BackColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = AppConfig.RosterHeaderBg;
      rosterGridView.ColumnHeadersDefaultCellStyle.ForeColor = rosterGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = AppConfig.RosterHeaderFg;
      rosterGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

      tableLayoutPanel3.BackColor = AppConfig.WindowBg;
      shivCheckbox.ForeColor = deadCheckbox.ForeColor = fatiguedCheckbox.ForeColor = woundedCheckbox.ForeColor = AppConfig.WindowFg;

      rosterGridView.DefaultCellStyle.BackColor = AppConfig.GridCellBg;
      rosterGridView.DefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      rosterGridView.DefaultCellStyle.SelectionBackColor = AppConfig.GridCellBg;
      rosterGridView.DefaultCellStyle.SelectionForeColor = AppConfig.GridCellFg;
      rosterGridView.RowHeadersDefaultCellStyle.BackColor = AppConfig.GridCellBg;
      rosterGridView.RowHeadersDefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      rosterGridView.RowHeadersDefaultCellStyle.SelectionBackColor = AppConfig.RosterSelectedBookendBg;
      rosterGridView.RowHeadersDefaultCellStyle.SelectionForeColor = AppConfig.WindowTitleFg;
      rosterGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      rosterGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      rosterGridView.RowHeadersDefaultCellStyle.Padding = Padding.Empty;
      rosterGridView.RowHeadersWidth = 15;
      rosterGridView.BackgroundColor = AppConfig.GridBg;
      rosterGridView.GridColor = AppConfig.GridBg;
      rosterGridView.CellBorderStyle = rosterCellBorders;

      squadGridView.RowHeadersDefaultCellStyle.BackColor = AppConfig.GridCellBg;
      squadGridView.RowHeadersDefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      squadGridView.RowHeadersDefaultCellStyle.SelectionBackColor = AppConfig.RosterSelectedBookendBg;
      squadGridView.RowHeadersDefaultCellStyle.SelectionForeColor = AppConfig.WindowTitleFg;
      squadGridView.DefaultCellStyle.BackColor = AppConfig.GridCellBg;
      squadGridView.DefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      squadGridView.DefaultCellStyle.SelectionBackColor = AppConfig.GridCellBg;
      squadGridView.DefaultCellStyle.SelectionForeColor = AppConfig.GridCellFg;
      squadGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      squadGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      squadGridView.RowHeadersDefaultCellStyle.Padding = Padding.Empty;
      squadGridView.CellBorderStyle = shortlistGridCellBorders;
      squadGridView.RowHeadersWidth = 15;
      squadGridView.BackgroundColor = AppConfig.GridBg;

      squadPerkList.DefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      squadPerkList.DefaultCellStyle.BackColor = AppConfig.GridCellBg;
      squadPerkList.DefaultCellStyle.SelectionForeColor = AppConfig.GridCellFg;
      squadPerkList.DefaultCellStyle.SelectionBackColor = AppConfig.GridCellBg;
      squadPerkList.BackgroundColor = AppConfig.GridBg;
      squadPerkList.CellBorderStyle = SquadPerksByName ? DataGridViewCellBorderStyle.SingleHorizontal : DataGridViewCellBorderStyle.Single;

      soldierPerksGridView.DefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      soldierPerksGridView.DefaultCellStyle.BackColor = AppConfig.GridCellBg;
      soldierPerksGridView.DefaultCellStyle.SelectionForeColor = AppConfig.GridCellFg;
      soldierPerksGridView.DefaultCellStyle.SelectionBackColor = AppConfig.GridCellBg;
      soldierPerksGridView.CellBorderStyle = soldierCellBorders;
      soldierPerksGridView.BackgroundColor = AppConfig.GridBg;

      checklistGridView.DefaultCellStyle.SelectionBackColor = AppConfig.GridCellBg;
      checklistGridView.DefaultCellStyle.SelectionForeColor = AppConfig.GridCellFg;
      checklistGridView.DefaultCellStyle.BackColor = AppConfig.GridCellBg;
      checklistGridView.DefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      checklistGridView.CellBorderStyle = checklistCellBorders;
      checklistGridView.BackgroundColor = AppConfig.GridBg;

      tabPage9.BackColor = AppConfig.WindowBg;
      panel1.BackColor = AppConfig.WindowBg;
      panel1.ForeColor = AppConfig.WindowFg;

      treeDataGrid.DefaultCellStyle.ForeColor = AppConfig.GridCellFg;
      treeDataGrid.DefaultCellStyle.BackColor = AppConfig.GridCellBg;
      treeDataGrid.DefaultCellStyle.SelectionForeColor = AppConfig.GridCellFg;
      treeDataGrid.DefaultCellStyle.SelectionBackColor = AppConfig.GridCellBg;
      treeDataGrid.BackgroundColor = AppConfig.GridBg;
      treeDataGrid.CellBorderStyle = PerkTreeByName ? DataGridViewCellBorderStyle.SingleHorizontal : DataGridViewCellBorderStyle.Single;

      perkFilterTextbox.BackColor = AppConfig.WindowBg;
      perkFilterTextbox.ForeColor = AppConfig.WindowFg;
    }

    public static void DefaultSorting()
    {
      SortArray.Clear();
      SortArray.Add([.. DefaultSortArray]);
    }

    public static void DoSorting(int colIndex)
    {
      if (colIndex < 0 || colIndex > 11) return;
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

    public void ToggleInShortlist(object sender, bool? toTop = null)
    {
      var point = ((DataGridView)sender).PointToClient(Cursor.Position);
      var info = ((DataGridView)sender).HitTest(point.X, point.Y);

      if (info.RowIndex == -1) return;
      if (((DataGridView)sender).Rows[info.RowIndex] is not null)
      {

        Soldier? s = Roster.FirstOrDefault(x =>
          ((((DataGridView)sender).Rows[info.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Contains(x.LName)
          && (long)((((DataGridView)sender).Rows[info.RowIndex].Cells[14].Value ?? "")) == x.Id);
        if (s is not null)
        {
          if (!s.IsDead && (!s.IsWounded || s.HoursOut <= RecoverableHrs))
          {
            int vscroll = ((DataGridView)sender).FirstDisplayedScrollingRowIndex;

            if (s?.InShortlist == true)
            {
              if (s.InSquad == true)
              {
                List<long> thisSoldierStats = [];
                for (int j = 0; j < 5; j++)
                {
                  _ = Int64.TryParse((((DataGridView)sender).Rows[info.RowIndex].Cells[j].Value ?? "0").ToString() ?? "0", out long thisStat);
                  thisSoldierStats.Add(thisStat);
                }

                for (int i = 0; i < SquadStats.Count; i++)
                {
                  if (SquadStats[i][0] == thisSoldierStats[0]
                    && SquadStats[i][1] == thisSoldierStats[1]
                    && SquadStats[i][2] == thisSoldierStats[2]
                    && SquadStats[i][3] == thisSoldierStats[3]
                    && SquadStats[i][4] == thisSoldierStats[4]
                  )
                  {
                    SquadStats.RemoveAt(i);
                    break;
                  }
                }
              }

              if (CurrentShortlistSize == 1) HideSquadGrid();
              else tableLayoutPanel1.RowStyles[0].Height -= 25;

              if (((DataGridView)sender).Name == "squadGridView")
              {
                if (s.InSquad) s.InSquad = false;

                if (squadGridView.Rows.Count > 1)
                {
                  squadGridView.Rows.RemoveAt(info.RowIndex);
                }
                else squadGridView.Rows.Clear();
              }
              else
              {
                for (int i = 0; i < squadGridView.Rows.Count; i++)
                {
                  if (((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(s.LName) && (long)((squadGridView.Rows[i].Cells[14].Value ?? "") ?? "") == s.Id)
                  {
                    squadGridView.Rows.RemoveAt(i);
                    break;
                  }
                }
              }

              CurrentShortlistSize--;
              Roster.Where(x => x.LName == s.LName && x.Xp == s.Xp).ToList().ForEach(x => { x.InShortlist = false; x.InSquad = false; });
              Shortlist.Remove(s);

              int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

              if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
            }
            else if (CurrentShortlistSize < MaxSquadSize && s?.InShortlist == false)
            {
              if (CurrentShortlistSize == 0) ShowSquadGrid();
              s.InSquad = Shortlist.Count < (int?)(squadSizeCombx.SelectedItem);
              s.InShortlist = true;
              tableLayoutPanel1.RowStyles[0].Height += 25;

              bool debugVals = false;

              object[] dgvrVals =
              [
                s.LName.Contains("TamTam") ? $"{s.LName} ♥♥" : s.LName.Contains("kaBuoy") ? $"{s.LName}   >:3" : s.LName,
                debugVals ? $"s{s.InShortlist}:q{s.InSquad}" : s.NName,
                (s.IsFatigued || s.IsWounded) && !s.IsDead ? ((s.HoursOut / 24) > 0 ? $"{s.HoursOut / 24}d " : "") + $"{s.HoursOut % 24}h" : s.Status,
                s.Class,
                s.RankName,
                s.Stats.Defense,
                s.Stats.HP,
                s.Stats.Mobility,
                $"{(s.Stats.Will == 99999 ? string.Empty : s.Stats.Will)}",
                s.Stats.Aim,
                s.Xp,
                $"{(s.ToNext == 99999 ? string.Empty : s.ToNext)}",
                s.RankId,
                s.RosterNumber,
                s.Id,
                s.HasChecklistPerk,
                !(s.IsWounded && s.HoursOut > RecoverableHrs) && !s.IsDead,
                s.Perks.Any(x => x.Type == -1),
                s.InShortlist,
                s.InSquad
              ];
              squadGridView.Rows.Add(dgvrVals);

              CurrentShortlistSize++;
              Shortlist.Add(s);
              Roster.Where(x => x.LName == s.LName && x.Id == s.Id).ToList().ForEach(x => { x.InShortlist = s.InShortlist; x.InSquad = s.InSquad; });

              if ((bool?)dgvrVals[18] == true)
              {
                List<long> thisSoldierStats = [];

                long thisStat = 0;
                for (int j = 0; j < 5; j++)
                {
                  _ = Int64.TryParse((((DataGridView)sender).Rows[info.RowIndex].Cells[j + 5].Value ?? "0").ToString() ?? "0", out thisStat);
                  thisSoldierStats.Add(thisStat);
                }

                bool wasFound = false;
                for (int i = 0; i < SquadStats.Count; i++)
                {
                  if (SquadStats[i][0] == thisSoldierStats[0]
                    && SquadStats[i][1] == thisSoldierStats[1]
                    && SquadStats[i][2] == thisSoldierStats[2]
                    && SquadStats[i][3] == thisSoldierStats[3]
                    && SquadStats[i][4] == thisSoldierStats[4]
                  )
                  {
                    wasFound = true;
                    break;
                  }
                }

                if (!wasFound) SquadStats.Add(thisSoldierStats);
              }

              int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

              if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
            }

            // switch RosterNumber to squad count
            for (int i = 0; i < squadGridView.Rows.Count; i++) squadGridView.Rows[i].Cells[12].Value = (i + 1);

            if (vscroll > 0 && ((DataGridView)sender).Rows.Count >= vscroll - 1) ((DataGridView)sender).FirstDisplayedScrollingRowIndex = vscroll;
          }
        }

        if (CurrentShortlistSize == 0)
        {
          tabControl1.SelectedIndex = 0;
        }
        else if (tabControl1.SelectedIndex == 1)
        {
          tabControl1_TabIndexChanged(null, null); // todo: not this
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
        int enabled = Int32.Parse(ChecklistPerksDatatable.Rows[i]["Enabled"].ToString() ?? "0");

        if (enabled > 0)
        {
          string perkName = ChecklistPerksDatatable.Rows[i]["Name"].ToString() ?? "";
          if (!string.IsNullOrWhiteSpace(perkName)) ChecklistPerks.Add(perkName, enabled);
        }
      }

      for (int i = 0; i < AbilitiesDatatable.Rows.Count; i++)
      {
        int enabled = Int32.Parse(AbilitiesDatatable.Rows[i]["Enabled"].ToString() ?? "0");
        string abilityName = AbilitiesDatatable.Rows[i]["Name"].ToString() ?? "";

      }
    }

    public void PopulateChecklist()
    {
      checklistGridView.Rows.Clear();

      checklistGridView.Rows.Add(["Checklist", "", ""]);
      checklistGridView.Rows[0].Frozen = true;
      checklistGridView.Rows[0].DefaultCellStyle = new() { BackColor = AppConfig.RosterHeaderBg, ForeColor = AppConfig.RosterHeaderFg, SelectionBackColor = AppConfig.RosterHeaderBg, SelectionForeColor = AppConfig.RosterHeaderFg };
      checklistGridView.Rows[0].Cells[0].Style = new(checklistGridView.Rows[0].DefaultCellStyle) { Alignment = DataGridViewContentAlignment.MiddleCenter, Padding = new(20, 0, 0, 0) };
      checklistGridView.Rows[0].Height = checklistGridView.Rows[0].Height + 4;
      checklistGridView.Rows[0].DividerHeight += 4;

      ChecklistPerks.ToList().ForEach(x => checklistGridView.Rows.Add(x.Key, 0, $"/ {x.Value}"));

      int checklistOk = 0;
      foreach (DataGridViewRow cr in checklistGridView.Rows)
      {
        if (cr.Index == 0) continue;
        bool thisOk = false;
        foreach (Soldier s in Shortlist)
        {
          bool inActualSquad = false;
          for (int i = 0; i < (int?)(squadSizeCombx.SelectedItem); i++)
          {
            if (i < Shortlist.Count)
            {
              if (((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(s.LName))
              {
                inActualSquad = true;
                break;
              }
            }
          }

          if (inActualSquad)
          {
            if (s.Perks.Select(x => x.Name).Contains(cr.Cells[0].Value))
            {
              int curVal = (int)((cr.Cells[1].Value) ?? 0);
              cr.Cells[1].Value = ++curVal;
              if (curVal >= Int32.Parse((cr.Cells[2].Value?.ToString()?.Substring(2, 1) ?? "0")))
              {
                checklistOk++;
                thisOk = true;
                break;
              }
            }
          }
        }

        cr.Cells[0].Style = cr.Cells[1].Style = cr.Cells[2].Style = thisOk
          ? new() { BackColor = AppConfig.HiBg, SelectionBackColor = AppConfig.HiBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg }
          : new() { BackColor = AppConfig.MinBg, SelectionBackColor = AppConfig.MinBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg };
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
      treeDataGrid.Rows[0].DefaultCellStyle = new() { BackColor = AppConfig.RosterHeaderBg, ForeColor = AppConfig.RosterHeaderFg, SelectionBackColor = AppConfig.RosterHeaderBg, SelectionForeColor = AppConfig.RosterHeaderFg };
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
          BackColor = AppConfig.TreeTabUnselectedBg,
          ForeColor = AppConfig.TreeHeaderFg,
          SelectionBackColor = AppConfig.TreeTabUnselectedBg,
          SelectionForeColor = AppConfig.TreeHeaderFg
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
      if (perkFilterTextbox.TextLength > 45) perkFilterTextbox.Text = string.Concat("~", perkFilterTextbox.Text.AsSpan(perkFilterTextbox.TextLength - 45, 45));
    }

    public void ListRoster(List<Soldier>? rosterIn = null, bool squadUpdate = true)
    {
      if (squadUpdate)
      {
        squadPerkList.Rows.Clear();
        SquadPerks.Clear();
      }

      DataGridViewRow? selectedRow = rosterGridView.Rows.Count > 0 ? rosterGridView.SelectedRows[0] : null;
      int selectedRowIndex = -1;

      rosterIn ??= Roster;
      rosterGridView.Rows.Clear();

      List<Soldier> filteredSoldiers = [];
      RosterStats = [];
      int shivCount = 0;
      CurrentSquadSize = 0;

      foreach (Soldier s in Roster)
      {
        if (squadUpdate && Shortlist.Contains(s))
        {
          for (int i = 0; i < squadGridView.Rows.Count; i++)
          {
            if (((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(s.LName) && Int64.Parse((squadGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "") == s.Xp)
            {
              if (i < (int?)(squadSizeCombx.SelectedItem))
              {
                s.InSquad = i < (int?)(squadSizeCombx.SelectedItem);
                CurrentSquadSize++;
                s.Perks.ForEach(x => { if (!SquadPerks.TryAdd(x.Name ?? "", 1)) SquadPerks[x.Name ?? ""]++; });
              }

              if (s.IsShiv) shivCount++;
            }
          }
        }
      }
      if (squadUpdate)
      {
        if (SquadPerks.Count > 0)
        {
          squadPerkList.Rows.Add($"Soldiers: {CurrentSquadSize}{(shivCount > 0 ? $" Shivs: {shivCount}" : "")}");
          squadPerkList.Rows[0].Height = squadPerkList.Rows[0].Height + 4;
          squadPerkList.Rows[0].DividerHeight += 4;
          squadPerkList.Rows[0].Frozen = true;
          squadPerkList.Rows[0].Cells[0].Style = squadPerkList.Rows[0].Cells[1].Style = new()
          {
            BackColor = AppConfig.SquadHeaderBg,
            ForeColor = AppConfig.WindowTitleFg,
            SelectionBackColor = AppConfig.SquadHeaderBg,
            SelectionForeColor = AppConfig.WindowTitleFg,
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            Font = new(Font, FontStyle.Regular),
            Padding = new(20, 0, 0, 0)
          };

          foreach (var p in SquadPerksByName ? SquadPerks.OrderBy(x => x.Key) : SquadPerks.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
          {
            squadPerkList.Rows.Add([p.Key, p.Value]);
          }
        }
        else
        {
          squadPerkList.Rows.Add($"No squad selected");
          squadPerkList.Rows[0].Height = squadPerkList.Rows[0].Height + 4;
          squadPerkList.Rows[0].DividerHeight += 4;
          squadPerkList.Rows[0].Frozen = true;
          squadPerkList.Rows[0].Cells[0].Style = squadPerkList.Rows[0].Cells[1].Style = new()
          {
            BackColor = AppConfig.SquadHeaderBg,
            ForeColor = AppConfig.WindowTitleFg,
            SelectionBackColor = AppConfig.SquadHeaderBg,
            SelectionForeColor = AppConfig.WindowTitleFg,
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            Font = new(Font, FontStyle.Italic),
            Padding = new(25, 0, 0, 0)
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
        RosterStats.AddRange([s.Stats.Defense, s.Stats.HP, s.Stats.Mobility, s.Stats.Will, s.Stats.Aim]);
      }

      int filteredSoldierIndex = 0;

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
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderBy(x => x.Xp)] : [.. filteredSoldiers.OrderByDescending(x => x.Xp)];
              break;
            case 5:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Defense)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Defense)];
              break;
            case 6:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.HP)] : [.. filteredSoldiers.OrderBy(x => x.Stats.HP)];
              break;
            case 7:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Mobility)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Mobility)];
              break;
            case 8:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Will)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Will)];
              break;
            case 9:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.Stats.Aim)] : [.. filteredSoldiers.OrderBy(x => x.Stats.Aim)];
              break;
            case 10:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderBy(x => x.Xp)] : [.. filteredSoldiers.OrderByDescending(x => x.Xp)];
              break;
            case 11:
              filteredSoldiers = isInverted ? [.. filteredSoldiers.OrderByDescending(x => x.ToNext)] : [.. filteredSoldiers.OrderBy(x => x.ToNext)];
              break;
          }
        }
      }

      foreach (Soldier f in filteredSoldiers)
      {
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

        bool debugVals = false;

        object[] dgvrVals =
        {
          f.LName,
          debugVals ? $"s{f.InShortlist}:q{f.InSquad}" : f.NName,
          (f.IsFatigued || f.IsWounded) && !f.IsDead ? ((f.HoursOut / 24) > 0 ? $"{f.HoursOut / 24}d " : "") + $"{f.HoursOut % 24}h" : f.Status,
          f.Class,
          f.RankName,
          f.Stats.Defense,
          f.Stats.HP,
          f.Stats.Mobility,
          $"{(f.Stats.Will == 99999 ? string.Empty : f.Stats.Will)}",
          f.Stats.Aim,
          f.Xp,
          $"{(f.ToNext == 99999 ? string.Empty : f.ToNext)}",
          filteredSoldierIndex + 1,
          f.RankId,
          f.Id,
          f.HasChecklistPerk,
          !(f.IsWounded && f.HoursOut > RecoverableHrs) && !f.IsDead,
          f.Perks.Any(x => x.Type == -1),
          f.InShortlist,
          f.InSquad
        };

        rosterGridView.Rows.Add(dgvrVals);
        if (selectedRow is not null && ((selectedRow.Cells[0].Value ?? "").ToString() ?? "").Contains(f.LName) && Int64.Parse(((selectedRow.Cells[10].Value ?? "").ToString() ?? "")) == f.Xp)
          selectedRowIndex = filteredSoldierIndex;
        DataGridViewRow thisRow = rosterGridView.Rows[filteredSoldierIndex];

        if (!f.IsShiv && f.IsWounded && f.HoursOut > RecoverableHrs)
        {
          thisRow.Cells["Aim"].Style =
          thisRow.Cells["Mob"].Style =
          thisRow.Cells["HP"].Style =
          thisRow.Cells["Will"].Style =
          thisRow.Cells["Def"].Style = StatWoundBgStyle
            ? new() { BackColor = AppConfig.WoundBg, SelectionBackColor = AppConfig.WoundBg, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg, Font = new(Font, FontStyle.Regular) }
            : new() { BackColor = AppConfig.GridCellBg, SelectionBackColor = AppConfig.GridCellBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["SoldierClass"].Style =
          thisRow.Cells["RankName"].Style =
          thisRow.Cells["XP"].Style =
          thisRow.Cells["Next"].Style = LongWoundBgStyle
            ? new() { BackColor = AppConfig.WoundBg, SelectionBackColor = AppConfig.WoundBg, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg, Font = new(Font, FontStyle.Regular) }
            : new() { BackColor = AppConfig.GridCellBg, SelectionBackColor = AppConfig.GridCellBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["Number"].Style =
            new() { BackColor = rosterGridView.BackgroundColor, SelectionBackColor = rosterGridView.BackgroundColor, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg, Font = SmallFont };
        }
        else if (f.IsShiv)
        {
          thisRow.Cells["Aim"].Style =
          thisRow.Cells["Mob"].Style =
          thisRow.Cells["HP"].Style =
          thisRow.Cells["Will"].Style =
          thisRow.Cells["Def"].Style =
            new() { BackColor = AppConfig.ShivBg, SelectionBackColor = AppConfig.ShivBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["Number"].Style =
            new() { BackColor = rosterGridView.BackgroundColor, SelectionBackColor = rosterGridView.BackgroundColor, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg, Font = SmallFont };
        }
        else
        {
          thisRow.Cells["SoldierClass"].Style =
          thisRow.Cells["RankName"].Style =
          thisRow.Cells["XP"].Style =
          thisRow.Cells["Next"].Style =
            new() { BackColor = AppConfig.GridCellBg, SelectionBackColor = AppConfig.GridCellBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg, Font = new(Font, FontStyle.Regular) };

          thisRow.Cells["Number"].Style =
            new() { BackColor = rosterGridView.BackgroundColor, SelectionBackColor = rosterGridView.BackgroundColor, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg, Font = SmallFont };

          if (f.ToNext < AppConfig.ToNextXPHiValue) thisRow.Cells[11].Style = new() { BackColor = AppConfig.ToNextXPHi, SelectionBackColor = AppConfig.ToNextXPHi };
          else if (f.ToNext < AppConfig.ToNextXPLoValue) thisRow.Cells[11].Style = new() { BackColor = AppConfig.ToNextXPLo, SelectionBackColor = AppConfig.ToNextXPLo };
        }

        if (f.IsShiv) thisRow.DefaultCellStyle = new() { BackColor = AppConfig.ShivBg, SelectionBackColor = AppConfig.ShivBg };
        else if (f.IsDead) thisRow.Cells["Status"].Style = new() { BackColor = AppConfig.DeadBg, SelectionBackColor = AppConfig.DeadBg, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg };

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
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = AppConfig.WoundBg, SelectionBackColor = AppConfig.WoundBg, Font = new(Font, FontStyle.Bold), ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = AppConfig.FatigueBg, SelectionBackColor = AppConfig.FatigueBg, Font = new(Font, FontStyle.Bold), ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg };
          }
          else
          {
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = AppConfig.WoundBg, SelectionBackColor = AppConfig.WoundBg, ForeColor = AppConfig.DeadFg, SelectionForeColor = AppConfig.DeadFg };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = AppConfig.FatigueBg, SelectionBackColor = AppConfig.FatigueBg, ForeColor = AppConfig.GridCellFg, SelectionForeColor = AppConfig.GridCellFg };
          }
        }

        filteredSoldierIndex++;
        thisRow.Selected = (((thisRow.Cells["Id"].Value ?? "").ToString() == SoldierSelectedIdXP.Split(',').First()) && ((thisRow.Cells["XP"].Value ?? "").ToString() == SoldierSelectedIdXP.Split(',').Last()));
      }

      for (int i = 0; i < rosterGridView.RowCount; i++)
      {
        for (int j = 5; j <= 9; j++)
        {
          List<long> statHeirarchy = [];
          Color bgColor = Color.Blue;

          switch (j)
          {
            case 5:
              statHeirarchy = [.. RosterStats.Select(x => x[0]).OrderByDescending(x => x).Distinct()];
              break;
            case 6:
              statHeirarchy = [.. RosterStats.Select(x => x[1]).OrderByDescending(x => x)];
              break;
            case 7:
              statHeirarchy = [.. RosterStats.Select(x => x[2]).OrderByDescending(x => x)];
              break;
            case 8:
              statHeirarchy = [.. RosterStats.Select(x => x[3]).OrderByDescending(x => x).Distinct()];
              break;
            case 9:
              statHeirarchy = [.. RosterStats.Select(x => x[4]).OrderByDescending(x => x).Distinct()];
              break;
          }

          _ = Int64.TryParse((rosterGridView.Rows[i].Cells[j].Value ?? "-999").ToString() ?? "-999", out long thisStat);

          int heirarchySize = statHeirarchy.Count;
          int heirarchySplit = heirarchySize >= 15 ? 7 : heirarchySize >= 5 ? 5 : heirarchySize >= 3 ? 3 : 1;
          int heirarchyStep = heirarchySize / heirarchySplit;

          if (heirarchySplit == 7)
          {
            if (thisStat >= statHeirarchy[1])
            {
              bgColor = AppConfig.MaxBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 2])
            {
              bgColor = AppConfig.HiBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 3])
            {
              bgColor = AppConfig.GridCellBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 4])
            {
              bgColor = AppConfig.GridCellBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 5])
            {
              bgColor = AppConfig.GridCellBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 6])
            {
              bgColor = AppConfig.LoBg;
            }
            else bgColor = AppConfig.MinBg;
          }
          else if (heirarchySplit == 5)
          {
            if (thisStat >= statHeirarchy[1])
            {
              bgColor = AppConfig.MaxBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 2])
            {
              bgColor = AppConfig.HiBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 3])
            {
              bgColor = AppConfig.GridCellBg;
            }
            else if (thisStat >= statHeirarchy[heirarchyStep * 4])
            {
              bgColor = AppConfig.LoBg;
            }
            else bgColor = AppConfig.MinBg;
          }
          else if (heirarchySplit == 3)
          {
            if (thisStat >= statHeirarchy[0])
            {
              bgColor = AppConfig.HiBg;
            }
            else if (thisStat >= statHeirarchy[1])
            {
              bgColor = AppConfig.GridCellBg;
            }
            else bgColor = AppConfig.LoBg;
          }
          else bgColor = AppConfig.GridCellBg;

          rosterGridView.Rows[i].Cells[j].Style = new() { BackColor = bgColor, SelectionBackColor = bgColor };
        }
      }

      PopulateChecklist();
      if (selectedRowIndex >= 0) PopupateSoldierPerks(selectedRowIndex);
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

    // this sets the title bar colors
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private void Rosterizer_Load(object sender, EventArgs e)
    {
      int trueValue = 0x01;

      int formHeaderBackgroundColorValue = (AppConfig.WindowTitleBg.B << 16) | (AppConfig.WindowTitleBg.G << 8) | AppConfig.WindowTitleBg.R;
      int formHeaderTextColorValue = (AppConfig.WindowTitleFg.B << 16) | (AppConfig.WindowTitleFg.G << 8) | AppConfig.WindowTitleFg.R;

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

    static List<Soldier> InitializeRoster(JsonRoot saveJson)
    {
      List<Soldier> roster = [];
      int rosterCount = 0;

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
          string soldierClass = (((JObject)((classProp.Properties?.First(x => x.Name == "strName").Value ?? ""))).First?.First?.ToString() ?? "").Trim("{}".ToCharArray());

          if (soldierProp?.Properties is not null)
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
              Class = soldierClass,
              // status is in the parent entity
              Status = ((entity.Properties.First(x => x.Name == "m_eStatus").Value ?? "").ToString() ?? "").TrimStart("eStatus_".ToCharArray()),
              IsDead = false,
              IsBlueshirt = false,
              HoursOut = Int64.Parse((fatigueProp?.Value ?? "").ToString() ?? ""),
              IsShiv = false,
              IsWounded = false,
              IsFatigued = false,
              InShortlist = false,
              InSquad = false,
              HasChecklistPerk = false,
              Score = 0,
              Luck = 0,
              RosterNumber = ++rosterCount,
              RandomPerks = [],
              Abilities = []
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

            int aIndex = 0;
            charProp?.Properties?[4].Int_values?.ForEach(x =>
            {
              if (x > 0 && AbilitiesDatatable.Rows[aIndex]["Enabled"].ToString() == "1" && !string.IsNullOrWhiteSpace(AbilitiesDatatable.Rows[aIndex]["Name"].ToString()))
              {
                thisSoldier.Abilities.Add(new() { Id = aIndex, Name = AbilitiesDatatable.Rows[aIndex]["Name"].ToString() ?? "" });
              }
              aIndex++;
            });

            // get soldier's random perk tree (spoilers)
            bool isThisSoldier = false;
            bool breakLoop = false;
            foreach (CheckpointTable e2 in ((saveJson).Checkpoints[0].Checkpoint_table ?? []).Where(x => x.Name == "Command1.TheWorld:PersistentLevel.RPCheckpoint_0"))
            {
              foreach (var p2 in e2.Properties?.Where(x => x.Name == "arrSoldierStorage") ?? [])
              {
                foreach (var p3 in p2.Structs ?? [])
                {
                  foreach (XStruct p4 in p3)
                  {
                    if (p4.Name == "SoldierID")
                    {
                      if (Int64.Parse(p4.Value.ToString() ?? "-1") == thisSoldier.Id)
                      {
                        isThisSoldier = true;
                      }
                    }

                    if (isThisSoldier && p4.Name == "RandomTree")
                    {
                      thisSoldier.RandomPerks = p4.Elements ?? [];
                      breakLoop = true;
                    }

                    if (breakLoop) break;
                  }
                  if (breakLoop) break;
                }
                if (breakLoop) break;
              }
              if (breakLoop) break;
            }

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

            List<long> unpickedPerkIds = [];
            if (thisSoldier.RandomPerks.Count == 21)
            {
              for (int i = 2; i <= thisSoldier.RankId; i++)
              {
                List<long> unpickedLvl = [];
                bool pickedPerk = false;

                // these are the tree levels, three per level
                switch (i)
                {
                  case 2:
                    unpickedLvl = thisSoldier.RandomPerks[3..6];
                    break;
                  case 3:
                    unpickedLvl = thisSoldier.RandomPerks[6..9];
                    break;
                  case 4:
                    unpickedLvl = thisSoldier.RandomPerks[9..12];
                    break;
                  case 5:
                    unpickedLvl = thisSoldier.RandomPerks[12..15];
                    break;
                  case 6:
                    unpickedLvl = thisSoldier.RandomPerks[15..18];
                    break;
                  case 7:
                    unpickedLvl = thisSoldier.RandomPerks[18..21];
                    break;
                }

                for (int j = 0; j < 3; j++)
                {
                  if (thisSoldier.Perks.Select(x => x.Id).Contains(unpickedLvl[j])) pickedPerk = true;
                }

                if (pickedPerk) continue;
                else unpickedPerkIds.AddRange(unpickedLvl);
              }
            }
            for (int i = 0; i < unpickedPerkIds.Count; i++)
            {
              if (unpickedPerkIds[i] > 0)
              {
                foreach (DataRow row in PerkList.Rows)
                {
                  if (Int64.Parse(row["Enabled"].ToString() ?? "-1") > 0 && Int32.Parse(row["ID"].ToString() ?? "") == unpickedPerkIds[i])
                  {
                    thisSoldier.Perks.Add(new()
                    {
                      Id = unpickedPerkIds[i],
                      Name = row["Name"].ToString() ?? "",
                      Type = -1
                    });
                    break;
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
                (Math.Abs(thisSoldier.Stats.Defense * 180))
              + (thisSoldier.Stats.HP * 150)
              + (thisSoldier.Stats.Mobility * 80)
              + (thisSoldier.Stats.Will * 18)
              + (thisSoldier.Stats.Aim * 29);

            if (thisSoldier.IsDead || thisSoldier.IsFatigued) thisSoldier.Score = 0;
            Score += (int)thisSoldier.Score;
            thisSoldier.Luck *= (DateTime.Now.Microsecond % 102);

            // add the soldier to the roster
            roster.Add(thisSoldier);
          }
        }
      }

      // add 0-soldier perks to roster perks list
      //foreach (DataRow row in PerkList.Rows) if (row["Enabled"].ToString() == "1") ListedRosterPerks.TryAdd(row["Name"].ToString() ?? "", 0);
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
        Soldier? s = Roster.FirstOrDefault(x => ((r.Cells[0].Value ?? "").ToString() ?? "").Contains(x.LName) && long.Parse((r.Cells[10].Value ?? "0").ToString() ?? "0") == x.Xp);
        soldierPerksGridView.Rows.Add($"{s?.LName} - {s?.RankName}");
        soldierPerksGridView.Rows[0].Cells[0].Style = new()
        {
          BackColor = AppConfig.SoldierTabUnselectedBg,
          ForeColor = AppConfig.WindowTitleFg,
          SelectionBackColor = AppConfig.SoldierTabUnselectedBg,
          SelectionForeColor = AppConfig.WindowTitleFg,
          Font = new(Font, FontStyle.Regular),
          Alignment = DataGridViewContentAlignment.MiddleCenter
        };
        soldierPerksGridView.Rows[0].Height = soldierPerksGridView.Rows[0].Height + 4;
        soldierPerksGridView.Rows[0].DividerHeight += 4;

        s?.Perks = [.. s.Perks.OrderBy(x => x.Type == -1).ThenBy(x => x.Name)];
        for (int i = 0; i < s?.Perks.Count; i++)
        {
          Perk p = s.Perks[i];
          soldierPerksGridView.Rows.Add([p.Type == -1 ? ($"({p.Name ?? ""})") : (p.Name ?? "")]);
          if (p.Type == -1)
          {
            soldierPerksGridView.Rows[i + 1].Cells[0].Style = new DataGridViewCellStyle(soldierPerksGridView.DefaultCellStyle)
            {
              Font = new(Font, FontStyle.Italic),
            };
          }
        }

        s?.Abilities = [.. s.Abilities.OrderBy(x => x.Name)];
        for (int i = 0; i < s?.Abilities.Count; i++)
        {
          soldierPerksGridView.Rows[soldierPerksGridView.Rows.Add(s.Abilities[i].Name)].Cells[0].Style = new()
          {
            BackColor = AppConfig.SoldierHeaderBg,
            ForeColor = AppConfig.SoldierHeaderFg,
            SelectionBackColor = AppConfig.SoldierHeaderBg,
            SelectionForeColor = AppConfig.SoldierHeaderFg,
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

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// action bindings
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    private void deadCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

      if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
    }

    private void woundedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

      if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
    }

    private void shivCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

      if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
    }
    private void fatiguedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

      if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
    }

    private void squadAndRosterGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      bool isSquadGrid = (((DataGridView)sender).Name == "squadGridView");
      e.Graphics?.InterpolationMode = InterpolationMode.Bilinear;
      e.Graphics?.PixelOffsetMode = PixelOffsetMode.HighSpeed;
      e.Graphics?.SmoothingMode = SmoothingMode.AntiAlias;

      if (!isSquadGrid)
      {
        if (e.RowIndex == -1 && e.ColumnIndex == -1)
        {
          e.Graphics?.FillRectangle(new SolidBrush(ChatGemActivated is null ? Color.Black : AppConfig.GridBg), new(e.ClipBounds.X, e.ClipBounds.Y, e.ClipBounds.Width, e.ClipBounds.Height));
          TextRenderer.DrawText(
            e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
            string.Format("{0}", "♦"),
            new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Point, 0),
            new Rectangle(e.ClipBounds.X + 1, e.ClipBounds.Y - 2, e.ClipBounds.Width, e.ClipBounds.Height),
            ChatGemActivated is null ? AppConfig.ChatGemPerfect : ChatGemActivated == true ? AppConfig.ChatGemActive : AppConfig.ChatGemIdle,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding
          );
          e.Handled = true;
        }
      }

      if (e.ColumnIndex == 12)
      {
        Color bgColor = Color.Black;
        Color fgColor = AppConfig.GridFg;

        if (isSquadGrid)
        {
          if (e.RowIndex + 1 <= (int?)(squadSizeCombx.SelectedItem))
          {
            bgColor = AppConfig.InSquadBookendsBg;
            fgColor = AppConfig.SquadHeaderFg;
          }
          else bgColor = fgColor = AppConfig.GridBg;
        }
        else bgColor = AppConfig.GridBg;

        e.Graphics?.FillRectangle(new SolidBrush(bgColor), new(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height));
        e.Graphics?.DrawLine(new Pen(AppConfig.StatLineFg, 1), new Point(e.CellBounds.X, e.CellBounds.Y), new Point(e.CellBounds.Left, e.CellBounds.Bottom));
        TextRenderer.DrawText(
          e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
          string.Format("{0}", e.FormattedValue),
          SmallFont,
          e.CellBounds,
          fgColor,
          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding
        );

        e.Handled = true;
      }
      else if (e.RowIndex >= 0)
      {
        if (e.ColumnIndex == 0)
        {
          if (((DataGridView)sender).Name == "squadGridView" && e.RowIndex < (int?)(squadSizeCombx.SelectedItem))
            e.Graphics?.FillRectangle(new SolidBrush(AppConfig.InSquadBookendsBg), new(e.CellBounds.X - 15, e.CellBounds.Y, 15, 24));

          bool hasChecklistPerk = (bool?)((DataGridView)sender).Rows[e.RowIndex].Cells[15].Value == true;
          bool hasUnselectedPerk = (bool?)((DataGridView)sender).Rows[e.RowIndex].Cells[17].Value == true;

          // draw dot indicating soldier has checklist perk 
          if (hasChecklistPerk)
          {
            Rectangle r = new(e.CellBounds.X - 15, e.CellBounds.Y + 6, 11, 11);
            Rectangle c = new(e.CellBounds.X - 13, e.CellBounds.Y + 8, 9, 9);

            e.Graphics?.DrawEllipse(new Pen(AppConfig.ClassColorShadow, 2), c);
            e.Graphics?.FillEllipse(new SolidBrush(AppConfig.ChecklistedPerkBg), r);
          }

          // draw dot indicating has unselected perks
          if (hasUnselectedPerk)
          {
            Rectangle r = new(e.CellBounds.X - 12, e.CellBounds.Y + 9, 5, 5);
            Rectangle r2 = new(e.CellBounds.X - 12, e.CellBounds.Y + 9, 5, 5);

            e.PaintBackground(e.CellBounds, false);
            e.Graphics?.DrawEllipse(new Pen(AppConfig.UnselectedSoldierPerkFg, 2), r2);
            e.Graphics?.FillEllipse(new SolidBrush(AppConfig.UnselectedSoldierPerkBg), r);
          }
        }
        // draw shortlist/squad indicator in roster view
        else if (e.ColumnIndex == 2 && ((DataGridView)sender).Name == "rosterGridView")
        {
          e.PaintBackground(e.CellBounds, false);

          bool inShortlist = (bool?)((DataGridView)sender).Rows[e.RowIndex].Cells[18].Value == true;
          bool inSquad = (bool?)((DataGridView)sender).Rows[e.RowIndex].Cells[19].Value == true;
          bool isWoobie = (bool?)((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Contains('♥') == true;

          if (inShortlist)
          {
            Rectangle r = new(e.CellBounds.X + 4, e.CellBounds.Y + 7, 10, 10);
            Rectangle r2 = new(e.CellBounds.X + 4, e.CellBounds.Y + 7, 10, 10);

            e.PaintBackground(e.CellBounds, false);
            e.Graphics?.DrawRectangle(new Pen(Color.Black, 2), r2);
            e.Graphics?.FillRectangle(new SolidBrush(AppConfig.HiBg), r);
            if (!inSquad)
            {
              TextRenderer.DrawText(
                e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
                isWoobie ? "♥" : "□",
                isWoobie ? Font : SmallFont,
                new Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 6, 12, 12),
                isWoobie ? AppConfig.WoobieBg : ((DataGridView)sender).DefaultCellStyle.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
          }

          if (inSquad && inShortlist)
          {
            Rectangle r = new(e.CellBounds.X + 4, e.CellBounds.Y + 5, 6, 6);
            Rectangle c = new(e.CellBounds.X + 4, e.CellBounds.Y + 7, 6, 6);

            e.Graphics?.DrawRectangle(new Pen(((DataGridView)sender).DefaultCellStyle.ForeColor, 2), c);
            e.Graphics?.FillRectangle(new SolidBrush(AppConfig.WindowTitleBg), r);
          }

          TextRenderer.DrawText(
            e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
            string.Format("{0}", e.FormattedValue),
            Font,
            e.CellBounds,
            ((DataGridView)sender).DefaultCellStyle.SelectionForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);

          e.Handled = true;
        }
        // draw dot for class
        else if (e.ColumnIndex == 3)
        {
          Color classColor = ((e.FormattedValue ?? "").ToString() ?? "").ToLower() switch
          {
            "assault" => AppConfig.AssaultClassColor,
            "engineer" => AppConfig.EngineerClassColor,
            "gunner" => AppConfig.GunnerClassColor,
            "infantry" => AppConfig.InfantryClassColor,
            "medic" => AppConfig.MedicClassColor,
            "rocketeer" => AppConfig.RocketeerClassColor,
            "scout" => AppConfig.ScoutClassColor,
            "sniper" => AppConfig.SniperClassColor,
            _ => AppConfig.ShivClassColor,
          };

          // define locations of graphics
          Rectangle dotRect = new(e.CellBounds.X + 2, e.CellBounds.Y + 5, 12, 12);
          Rectangle outlineRect = new(e.CellBounds.X + 4, e.CellBounds.Y + 7, 10, 10);
          Rectangle textRect = new(e.CellBounds.X + 8, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);

          e.PaintBackground(e.CellBounds, false);                                               // draw cell background
          e.Graphics?.DrawEllipse(new Pen(AppConfig.ClassColorShadow, 4F), outlineRect);                   // draw shadow
          e.Graphics?.FillEllipse(new SolidBrush(Color.FromArgb(255, 180, 180, 180)), dotRect);  // draw dot bg
          e.Graphics?.FillEllipse(new SolidBrush(classColor), dotRect);                          // draw dot
          TextRenderer.DrawText(                                                                // draw class name text
            e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
            string.Format("{0}", e.FormattedValue),
            Font,
            textRect,
            ((DataGridView)sender).DefaultCellStyle.SelectionForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
          e.Handled = true;                                                                     // mark as handled so the default draw doesn't happen
        }

        if (e.ColumnIndex > 4 && e.ColumnIndex < 10)
        {
          if (isSquadGrid)
          {
            List<long> statHeirarchy = [];
            Color bgColor = Color.Blue;

            switch (e.ColumnIndex)
            {
              case 5:
                statHeirarchy = [.. SquadStats.Select(x => x[0]).OrderByDescending(x => x)];
                break;
              case 6:
                statHeirarchy = [.. SquadStats.Select(x => x[1]).OrderByDescending(x => x)];
                break;
              case 7:
                statHeirarchy = [.. SquadStats.Select(x => x[2]).OrderByDescending(x => x)];
                break;
              case 8:
                statHeirarchy = [.. SquadStats.Select(x => x[3]).OrderByDescending(x => x)];
                break;
              case 9:
                statHeirarchy = [.. SquadStats.Select(x => x[4]).OrderByDescending(x => x)];
                break;
            }

            _ = Int64.TryParse((((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "0").ToString() ?? "0", out long thisStat);

            if (statHeirarchy.Count > 6 && thisStat >= statHeirarchy[1]) bgColor = AppConfig.HiBg;
            else if (statHeirarchy.Count > 6 && thisStat <= statHeirarchy[6]) bgColor = AppConfig.LoBg;
            else bgColor = AppConfig.GridCellBg;

            e.PaintBackground(e.CellBounds, false);
            e.Graphics?.FillRectangle(new SolidBrush(bgColor), e.CellBounds);

            TextRenderer.DrawText(
                        e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
                        string.Format("{0}", e.FormattedValue),
                        Font,
                        e.CellBounds,
                        ((DataGridView)sender).DefaultCellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
            e.Handled = true;
          }
        }

        if (e.ColumnIndex == 5 || e.ColumnIndex == 10)
        {
          Color lineColor = Color.FromArgb(100, 0, 0, 0);

          if (!e.Handled) e.PaintBackground(e.CellBounds, false);

          e.Graphics?.DrawLine(new Pen(lineColor, 1), new Point(e.CellBounds.X, e.CellBounds.Y), new Point(e.CellBounds.Left, e.CellBounds.Bottom));

          if (!e.Handled)
          {
            TextRenderer.DrawText(
              e.Graphics ?? ((DataGridView)sender).CreateGraphics(),
              string.Format("{0}", e.FormattedValue),
              Font,
              e.CellBounds,
              ((DataGridView)sender).DefaultCellStyle.SelectionForeColor,
              TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
            e.Handled = true;
          }
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
          int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

          if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
        }
        else ToggleInShortlist(sender);
      }
      else if (e.Button == MouseButtons.Left)
      {
        if (e.RowIndex == -1)
        {
          if (e.ColumnIndex > -1 && e.ColumnIndex < 12)
          {
            DoSorting(e.ColumnIndex);
            if (tabControl1.SelectedIndex == 1)
            {
              ListRoster([.. Shortlist.OrderBy(x => x.InSquad)]);
            }
            else
            {
              int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

              if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
            }
          }
        }
        else PopupateSoldierPerks(e.RowIndex, (DataGridView)sender);
      }
    }

    private void tabControl1_TabIndexChanged(object? sender = null, EventArgs? e = null)
    {
      hotStyle = new()
      {
        ForeColor = AppConfig.GridCellFg,
        SelectionForeColor = AppConfig.GridCellFg,
        BackColor = AppConfig.GridCellBg,
        SelectionBackColor = AppConfig.GridCellBg
      };

      if (sender is null || ((TabControl)sender).SelectedIndex == 1)
      {
        if (Shortlist.Count > 0)
        {
          if (!FromSquadTab) TabFlipVscroll = rosterGridView.FirstDisplayedScrollingRowIndex;
          FromSquadTab = true;
          TabFlipList = RosterPerkTree.Length == 0 ? RosterTree1.SubRoster : RosterPerkTree.Length == 1 ? RosterTree2.SubRoster : RosterPerkTree.Length == 2 ? RosterTree3.SubRoster : RosterTree4.SubRoster;
          TabFlipFilterLabelText = perkFilterTextbox.Text;
          perkFilterTextbox.Text = "";
          ListRoster([.. Shortlist.OrderBy(x => x.InSquad)]);
        }
      }
      else
      {
        if (FromSquadTab)
        {
          switch (RosterPerkTree.Length)
          {
            case 0:
              SetPerkTree(RosterTree1);
              break;
            case 1:
              SetPerkTree(RosterTree2);
              break;
            case 2:
              SetPerkTree(RosterTree3);
              break;
            case 3:
              SetPerkTree(RosterTree4);
              break;
          }

          ListRoster(TabFlipList);
          if (TabFlipVscroll > 0 && rosterGridView.Rows.Count >= TabFlipVscroll - 1) rosterGridView.FirstDisplayedScrollingRowIndex = TabFlipVscroll;
          else TabFlipVscroll = rosterGridView.FirstDisplayedScrollingRowIndex;
        }

        FromSquadTab = false;
      }
    }

    private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
    {
      e.Graphics.InterpolationMode = InterpolationMode.Bilinear;
      e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

      Rectangle rec = tabControl1.ClientRectangle;
      StringFormat StrFormat = new()
      {
        LineAlignment = StringAlignment.Center,
        Alignment = StringAlignment.Center
      };

      SolidBrush backColor = new(AppConfig.WindowBg);
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
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.TreeHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.TreeHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.TreeTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.TreeHeaderFg), tabTextArea, StrFormat);
          }
        }
        else if (i == 1) // squad
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.SquadHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.SquadHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.SquadTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.SquadHeaderFg), tabTextArea, StrFormat);
          }
        }
        else if (i == 2) // soldier
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.SoldierHeaderBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.SoldierHeaderFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.SoldierTabUnselectedBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.SoldierHeaderFg), tabTextArea, StrFormat);
          }
        }
        else if (i == 3) // checklist
        {
          if (ChecklistPass)
          {
            if (bSelected)
            {
              e.Graphics.FillRectangle(new SolidBrush(AppConfig.ChecklistGoodHeaderBg), recBounds);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.ChecklistGoodHeaderFg), tabTextArea, StrFormat);
            }
            else
            {
              e.Graphics.FillRectangle(new SolidBrush(AppConfig.ChecklistGoodTabUnselectedBg), recBounds);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.ChecklistGoodHeaderFg), tabTextArea, StrFormat);
            }
          }
          else
          {
            if (bSelected)
            {
              e.Graphics.FillRectangle(new SolidBrush(AppConfig.ChecklistBadHeaderBg), recBounds);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.ChecklistBadHeaderFg), tabTextArea, StrFormat);
            }
            else
            {
              e.Graphics.FillRectangle(new SolidBrush(AppConfig.ChecklistBadTabUnselectedBg), recBounds);
              e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.ChecklistBadHeaderFg), tabTextArea, StrFormat);
            }
          }
        }
        else if (i == 4) // options
        {
          if (bSelected)
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.WindowBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.WindowFg), tabTextArea, StrFormat);
          }
          else
          {
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.WindowBg), recBounds);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, new(e.Font ?? Font, FontStyle.Regular), new SolidBrush(AppConfig.WindowFg), tabTextArea, StrFormat);
          }
        }
      }
    }

    private void rosterGridView_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left) tabControl1.SelectedIndex = 2;
    }

    private void checklistGridView_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        if (e.RowIndex > 0) SearchPerkTree((DataGridView)sender, e.RowIndex, true);
      }
      else if (e.Button == MouseButtons.Right)
      {
        SetPerkTree(RosterTree1);
        ListRoster();
      }
    }

    private void timerStartStopButton_Click(object sender, EventArgs e)
    {

      timerLabel.Font = SevenSegmentFont;
      timerLabel.ForeColor = Color.Red;
      timer1.Interval = 10;
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
      timerLabel.Font = SevenSegmentFont;
      timerLabel.ForeColor = AppConfig.WindowFg;

      ChecklistPassTime = 0;
      Shortlist = [];
      foreach (Soldier s in Roster)
      {
        s.InShortlist = false;
        s.InSquad = false;
      }

      CurrentShortlistSize = 0;
      tabControl1.SelectedIndex = 0;
      HideSquadGrid();
      ListRoster();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      timerLabel.Text = $"{elapsedTime:h\\:mm\\:ss\\:ff}";

      if (ChecklistPass)
      {
        if (ChecklistPassTime == 0) ChecklistPassTime = timer2.ElapsedMilliseconds / 1000;

        if (DateTime.Now.Millisecond < 500)
        {
          timerLabel.Text = "";
        }
        else timerLabel.Text = $"{elapsedTime:h\\:mm\\:ss\\:ff}";

        timerRunning = false;
        timer2.Stop();
        timer2.Reset();
        timerStartStopButton.Text = "Start";
        return;
      }

      if (timerRunning)
      {
        elapsedTime = timer2.Elapsed;

        if (Math.Floor(elapsedTime.TotalSeconds) % 16 < 12)
        {
          if (Math.Floor(elapsedTime.TotalMilliseconds) % 13 == 0)
          {
            Score -= ((SquadSize - CurrentSquadSize) * 27) + 423;
          }
        }
        else Score -= (SquadSize - CurrentSquadSize);

        scorePointsLabel.Text = Score.ToString();
      }
      else if (!timerRunning)
      {
        if (DateTime.Now.Millisecond < 500)
        {
          timerLabel.Text = "";
        }
        else timerLabel.Text = $"{elapsedTime:h\\:mm\\:ss\\:ff}";
      }
    }

    private void minLvlCombx_ValueChanged(object sender, EventArgs e)
    {
      BlueshirtLvl = (int)(((ComboBox)sender).SelectedItem ?? 0);
      Roster.ForEach(x => x.IsBlueshirt = x.RankId <= BlueshirtLvl);
      if (tabControl1.SelectedIndex == 4) // options
      {
        int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

        if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
      }
    }

    private void squadSizeCombx_ValueChanged(object sender, EventArgs e)
    {
      SquadSize = (int)(((ComboBox)sender).SelectedItem ?? 8);
      Roster.ForEach(x => x.InShortlist = false);
      Shortlist = [];
      int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

      if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
    }

    private void squadPerkList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        SearchPerkTree((DataGridView)sender, e.RowIndex, true);
      }
      else if (e.Button == MouseButtons.Middle)
      {
        SquadPerksByName = !SquadPerksByName;
        squadPerkList.CellBorderStyle = SquadPerksByName ? DataGridViewCellBorderStyle.SingleHorizontal : DataGridViewCellBorderStyle.Single;

        tabControl1_TabIndexChanged(); // todo: not this
      }
    }

    private void squadPerkList_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        tabControl1_TabIndexChanged(); // todo: not this
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
        SetPerkTree(RosterTree1);
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
      string clicked = (((sender.Rows[clickedIndex].Cells[0].Value ?? "").ToString() ?? "").TrimStart('(').TrimEnd(')') ?? "");
      string soldier = sender.Name == "soldierPerksGridView" ? (sender.Rows[0].Cells[0].Value ?? "").ToString() ?? "" : "";
      if (!string.IsNullOrWhiteSpace(soldier)) soldier = soldier[..(soldier.IndexOf('-') - 1)];
      int vscroll = sender.FirstDisplayedScrollingRowIndex;

      bool found = false;

      for (int i = 0; i < AbilitiesDatatable.Rows.Count; i++)
      {
        found = AbilitiesDatatable.Rows[i]["Name"].ToString() == clicked;
        if (found) break;
      }

      for (int i = 0; i < PerkList.Rows.Count; i++)
      {
        found = PerkList.Rows[i]["Name"].ToString() == clicked;
        if (found) break;
      }

      if (!found) return;

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
            treeDataGrid.Rows[0].DefaultCellStyle = new() { BackColor = AppConfig.TreeTabUnselectedBg, ForeColor = AppConfig.TreeHeaderFg, SelectionBackColor = AppConfig.TreeTabUnselectedBg, SelectionForeColor = AppConfig.TreeHeaderFg };
            treeDataGrid.Rows[0].Height = treeDataGrid.Rows[0].Height + 4;
            treeDataGrid.Rows[0].DividerHeight += 4;

            ListRoster(RosterTree1.SubRoster);
            return;

          case 1:
            SetPerkTree(RosterTree2);
            for (int i = 2; i <= 3; i++) if (((treeDataGrid.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains('\\')) treeDataGrid.Rows.RemoveAt(i);

            for (int i = 0; i <= 1; i++)
            {
              treeDataGrid.Rows[i].DefaultCellStyle = new() { BackColor = AppConfig.TreeTabUnselectedBg, ForeColor = AppConfig.TreeHeaderFg, SelectionBackColor = AppConfig.TreeTabUnselectedBg, SelectionForeColor = AppConfig.TreeHeaderFg };
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
              treeDataGrid.Rows[i].DefaultCellStyle = new() { BackColor = AppConfig.TreeTabUnselectedBg, ForeColor = AppConfig.TreeHeaderFg, SelectionBackColor = AppConfig.TreeTabUnselectedBg, SelectionForeColor = AppConfig.TreeHeaderFg };
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
              treeDataGrid.Rows[i].DefaultCellStyle = new() { BackColor = AppConfig.TreeTabUnselectedBg, ForeColor = AppConfig.TreeHeaderFg, SelectionBackColor = AppConfig.TreeTabUnselectedBg, SelectionForeColor = AppConfig.TreeHeaderFg };
              treeDataGrid.Rows[i].Frozen = true;
            }
            treeDataGrid.Rows[3].Height = treeDataGrid.Rows[3].Height + 4;
            treeDataGrid.Rows[3].DividerHeight += 4;

            ListRoster(RosterTree4.SubRoster);
            return;
        }
      }
      int perkTreeLvl = singleLvl ? 0 : RosterPerkTree.Length;
      if ((sender.Name == "squadPerkList" && (clicked.StartsWith("Soldiers:") || clicked.StartsWith("No squad"))) || (sender.Name == "soldierPerksGridView" && clickedIndex == 0)) return;


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
              break;
            }
          }
          break;
        case 2:
          foreach (var b in RosterTree1.PerkBranches.OrderBy(x => x.Key.Name))
          {
            bool doBreak = false;
            if (b.Value.PerkTree.SequenceEqual(RosterPerkTree.Take(1)))
            {
              foreach (var c in b.Value.PerkBranches.OrderBy(x => x.Key.Name))
              {
                if (c.Value.PerkTree.SequenceEqual(RosterPerkTree))
                {
                  RosterTree4 = c.Value.PerkBranches.First(x => x.Value.PerkTree.Last().Name == clicked || x.Key.Name == clicked).Value;
                  SetPerkTree(RosterTree4);
                  doBreak = true;
                }
                if (doBreak) break;
              }
            }
            if (doBreak) break;
          }
          break;
        case 3:
          foreach (var b in RosterTree1.PerkBranches.OrderBy(x => x.Key.Name))
          {
            bool doBreak = false;
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
                      doBreak = true;
                    }
                    if (doBreak) break;
                  }
                }
                if (doBreak) break;
              }
            }
            if (doBreak) break;
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
      if (vscroll > 0 && sender.Rows.Count > vscroll && !sender.Rows[vscroll].Frozen) sender.FirstDisplayedScrollingRowIndex = vscroll;
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
        treeDataGrid.CellBorderStyle = PerkTreeByName ? DataGridViewCellBorderStyle.SingleHorizontal : DataGridViewCellBorderStyle.Single;
        int vscroll2 = rosterGridView.FirstDisplayedScrollingRowIndex;

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

        if (vscroll2 > 0 && rosterGridView.Rows.Count >= vscroll2 - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll2;
      }
    }

    private void TreeSearch(string searchTerms)
    {
      if (!string.IsNullOrWhiteSpace(searchTerms))
      {
        RelatedPerk RosterTreeFiltered = new()
        {
          PerkTree = [],
          PerkBranches = [],
          SubRoster = []
        }; ;

        searchTerms.Replace('/', '\\').Split('\\').ToList().ForEach(searchTerm =>
        {
          switch (RosterPerkTree.Length)
          {
            case 0:
              RosterTreeFiltered.PerkTree = RosterTree1.PerkTree;
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
              RosterTreeFiltered.PerkTree = RosterTree2.PerkTree;
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
              RosterTreeFiltered.PerkTree = RosterTree3.PerkTree;
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
              RosterTreeFiltered.PerkTree = RosterTree4.PerkTree;
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
        });

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
      }
    }

    private void rosterGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1 && e.ColumnIndex == -1) ((DataGridView)sender).Cursor = Cursors.Hand;
      if (e.RowIndex >= 0)
      {
        hotStyle = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style;
        Soldier? s = null;
        bool onShortlist = false;
        for (int i = 0; i < squadGridView.Rows.Count; i++)
        {
          s = Shortlist.FirstOrDefault(x => ((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "")
              .Contains(((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()))
            && Int64.Parse(((squadGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "")) == Int64.Parse((((DataGridView)sender).Rows[e.RowIndex].Cells[10].Value ?? "").ToString() ?? ""), null);
          if (s is not null)
          {
            bool isWoobie = ((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Contains('♥');

            ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
            {
              ForeColor = isWoobie ? AppConfig.WoobieFg : AppConfig.RosterSquadHotTrackFg,
              SelectionForeColor = isWoobie ? AppConfig.WoobieFg : AppConfig.RosterSquadHotTrackFg,
              BackColor = AppConfig.RosterSquadHotTrackBg,
              SelectionBackColor = AppConfig.RosterSquadHotTrackBg,
              Font = isWoobie ? new Font(Font, FontStyle.Bold) : Font
            };
            onShortlist = true;
            break;
          }
        }

        if (!onShortlist)
        {
          if ((bool)(((DataGridView)sender).Rows[e.RowIndex].Cells["IsAvailable"].Value ?? false) || Shortlist.Count == MaxSquadSize)
          {
            ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = new()
            {
              ForeColor = AppConfig.RosterHotTrackFg,
              SelectionForeColor = AppConfig.RosterHotTrackFg,
              BackColor = AppConfig.RosterHotTrackBg,
              SelectionBackColor = AppConfig.RosterHotTrackBg
            };
          }
          else ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = new()
          {
            ForeColor = AppConfig.GridCellFg,
            SelectionForeColor = AppConfig.GridCellFg,
            BackColor = AppConfig.WoundBg,
            SelectionBackColor = AppConfig.WoundBg
          };
        }
      }
    }
    private void rosterGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1) return;
      // hot-track roster
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      for (int i = 0; i < ((DataGridView)sender).Rows.Count; i++)
      {
        ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = hotStyle;
      }
    }

    private void squadGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        hotStyle = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
        Cursor cu = Cursors.Default;
        Soldier? s = null;
        for (int i = 0; i < rosterGridView.Rows.Count; i++)
        {
          s = Shortlist.FirstOrDefault(x => ((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray())
            .Contains(((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()))
            && Int64.Parse(((rosterGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "-1")) == Int64.Parse((((DataGridView)sender).Rows[e.RowIndex].Cells[10].Value ?? "").ToString() ?? "-1"), null);
          if (s is not null)
          {
            bool isWoobie = ((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Contains('♥');

            ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
            {
              ForeColor = isWoobie ? AppConfig.WoobieFg : AppConfig.RosterSquadHotTrackFg,
              SelectionForeColor = isWoobie ? AppConfig.WoobieFg : AppConfig.RosterSquadHotTrackFg,
              BackColor = AppConfig.RosterSquadHotTrackBg,
              SelectionBackColor = AppConfig.RosterSquadHotTrackBg,
              Font = isWoobie ? new Font(Font, FontStyle.Bold) : Font
            };
            break;
          }
        }
      }
      else ((DataGridView)sender).Cursor = Cursors.Default;
    }
    private void squadGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1) return;
      // hot-track roster
      for (int i = 0; i < rosterGridView.Rows.Count; i++)
      {
        rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      for (int i = 0; i < ((DataGridView)sender).Rows.Count; i++)
      {
        ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = hotStyle;
      }
    }
    private void treeDataGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track perk list

      List<Tuple<string, long>> perkedSquadSoldiers = [];
      List<Tuple<string, long>> perkedRosterSoldiers = [];

      hotStyle = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style;

      for (int i = 0; i < Shortlist.Count; i++)
        if (Shortlist[i].Perks.Select(x => x.Name).Contains(((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""))
          perkedSquadSoldiers.Add(new(Shortlist[i].LName, Shortlist[i].Xp));

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new((((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""),
          Int64.Parse(((squadGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
          {
            ForeColor = AppConfig.WindowTitleFg,
            SelectionForeColor = AppConfig.WindowTitleFg,
            BackColor = AppConfig.WindowTitleBg,
            SelectionBackColor = AppConfig.WindowTitleBg
          };
        else squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }

      List<Soldier> filteredRoster = RosterPerkTree.Length == 0 ? RosterTree1.SubRoster : RosterPerkTree.Length == 1 ? RosterTree2.SubRoster : RosterPerkTree.Length == 2 ? RosterTree3.SubRoster : RosterTree4.SubRoster;

      for (int i = 0; i < filteredRoster.Count; i++)
        if (filteredRoster[i].Perks.Select(x => x.Name).Contains(((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""))
          perkedRosterSoldiers.Add(new(filteredRoster[i].LName, filteredRoster[i].Xp));

      int lastIndex = rosterGridView.FirstDisplayedScrollingRowIndex + 26 - Shortlist.Count;

      for (int i = rosterGridView.FirstDisplayedScrollingRowIndex; i < lastIndex && i < rosterGridView.RowCount; i++)
      {
        for (int j = 0; j < perkedRosterSoldiers.Count; j++)
        {
          if (rosterGridView.Rows.Count > 0)
            if (((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()).Contains(perkedRosterSoldiers[j].Item1)
              && Int64.Parse((rosterGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "") == perkedRosterSoldiers[j].Item2)
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.WindowTitleFg,
                SelectionForeColor = AppConfig.WindowTitleFg,
                BackColor = AppConfig.WindowTitleBg,
                SelectionBackColor = AppConfig.WindowTitleBg
              };
              break;
            }
            else
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.GridCellFg,
                SelectionForeColor = AppConfig.GridCellFg,
                BackColor = AppConfig.GridCellBg,
                SelectionBackColor = AppConfig.GridCellBg
              };
            }
        }
      }

      if (((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Trim().StartsWith('\\')) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = new()
      {
        ForeColor = AppConfig.WindowTitleFg,
        SelectionForeColor = AppConfig.WindowTitleFg,
        BackColor = AppConfig.WindowTitleBg,
        SelectionBackColor = AppConfig.WindowTitleBg
      };
    }

    private void treeDataGrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      //// hot-track tree perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new() { BackColor = AppConfig.GridCellBg, SelectionBackColor = AppConfig.GridCellBg };
      }
      for (int i = 0; i < rosterGridView.Rows.Count; i++)
      {
        rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      if (((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").Trim().StartsWith('\\')) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = hotStyle;

    }

    private void squadPerkList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track squad perk list
      List<Tuple<string, long>> perkedSquadSoldiers = [];
      List<Tuple<string, long>> perkedRosterSoldiers = [];

      hotStyle = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style;

      for (int i = 0; i < Shortlist.Count; i++)
        if (Shortlist[i].Perks.Select(x => x.Name).Contains((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
          perkedSquadSoldiers.Add(new(Shortlist[i].LName, Shortlist[i].Xp));

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new((((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""),
            Int64.Parse(((squadGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
          {
            ForeColor = AppConfig.WindowTitleFg,
            SelectionForeColor = AppConfig.WindowTitleFg,
            BackColor = AppConfig.WindowTitleBg,
            SelectionBackColor = AppConfig.WindowTitleBg
          };
        else squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }

      List<Soldier> filteredRoster = RosterPerkTree.Length == 0 ? RosterTree1.SubRoster : RosterPerkTree.Length == 1 ? RosterTree2.SubRoster : RosterPerkTree.Length == 2 ? RosterTree3.SubRoster : RosterTree4.SubRoster;

      for (int i = 0; i < filteredRoster.Count; i++)
        if (filteredRoster[i].Perks.Select(x => x.Name).Contains((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
          perkedRosterSoldiers.Add(new(filteredRoster[i].LName, filteredRoster[i].Xp));

      int lastIndex = rosterGridView.FirstDisplayedScrollingRowIndex + 26 - Shortlist.Count;

      for (int i = rosterGridView.FirstDisplayedScrollingRowIndex; i < lastIndex && i < rosterGridView.RowCount; i++)
      {
        for (int j = 0; j < perkedRosterSoldiers.Count; j++)
        {
          if (rosterGridView.Rows.Count > 0)
            if (((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()).Contains(perkedRosterSoldiers[j].Item1)
              && Int64.Parse((rosterGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "") == perkedRosterSoldiers[j].Item2)
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.WindowTitleFg,
                SelectionForeColor = AppConfig.WindowTitleFg,
                BackColor = AppConfig.WindowTitleBg,
                SelectionBackColor = AppConfig.WindowTitleBg
              };
              break;
            }
            else
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.GridCellFg,
                SelectionForeColor = AppConfig.GridCellFg,
                BackColor = AppConfig.GridCellBg,
                SelectionBackColor = AppConfig.GridCellBg
              };
            }
        }
      }

      if (e.RowIndex == 0) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = new()
      {
        ForeColor = AppConfig.WindowTitleFg,
        SelectionForeColor = AppConfig.WindowTitleFg,
        BackColor = AppConfig.WindowTitleBg,
        SelectionBackColor = AppConfig.WindowTitleBg
      };
    }

    private void squadPerkList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new() { BackColor = AppConfig.GridCellBg, SelectionBackColor = AppConfig.GridCellBg };
      }
      for (int i = 0; i < rosterGridView.Rows.Count; i++)
      {
        rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      if (e.RowIndex == 0) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = hotStyle;
    }

    private void soldierPerksGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      List<Tuple<string, long>> perkedSquadSoldiers = [];
      List<Tuple<string, long>> perkedRosterSoldiers = [];

      string cellValue = (((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "";

      for (int i = 0; i < Shortlist.Count; i++)
        if (Shortlist[i].Perks.Select(x => x.Name).Contains(cellValue.TrimStart('(').TrimEnd(')')))
          perkedSquadSoldiers.Add(new(Shortlist[i].LName, Shortlist[i].Xp));

      hotStyle = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style;

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new((((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""),
            Int64.Parse(((squadGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
          {
            ForeColor = AppConfig.WindowTitleFg,
            SelectionForeColor = AppConfig.WindowTitleFg,
            BackColor = AppConfig.WindowTitleBg,
            SelectionBackColor = AppConfig.WindowTitleBg
          };
        else squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }

      List<Soldier> filteredRoster = RosterPerkTree.Length == 0 ? RosterTree1.SubRoster : RosterPerkTree.Length == 1 ? RosterTree2.SubRoster : RosterPerkTree.Length == 2 ? RosterTree3.SubRoster : RosterTree4.SubRoster;

      for (int i = 0; i < filteredRoster.Count; i++)
        if (filteredRoster[i].Perks.Select(x => x.Name).Contains(cellValue.TrimStart('(').TrimEnd(')')))
          perkedRosterSoldiers.Add(new(filteredRoster[i].LName, filteredRoster[i].Xp));

      int lastIndex = rosterGridView.FirstDisplayedScrollingRowIndex + 26 - Shortlist.Count;

      for (int i = rosterGridView.FirstDisplayedScrollingRowIndex; i < lastIndex && i < rosterGridView.RowCount; i++)
      {
        for (int j = 0; j < perkedRosterSoldiers.Count; j++)
        {
          if (rosterGridView.Rows.Count > 0)
            if (((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()).Contains(perkedRosterSoldiers[j].Item1)
              && Int64.Parse((rosterGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "") == perkedRosterSoldiers[j].Item2)
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.WindowTitleFg,
                SelectionForeColor = AppConfig.WindowTitleFg,
                BackColor = AppConfig.WindowTitleBg,
                SelectionBackColor = AppConfig.WindowTitleBg
              };
              break;
            }
            else
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.GridCellFg,
                SelectionForeColor = AppConfig.GridCellFg,
                BackColor = AppConfig.GridCellBg,
                SelectionBackColor = AppConfig.GridCellBg
              };
            }
        }
      }


      if (e.RowIndex == 0) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = new()
      {
        ForeColor = AppConfig.WindowTitleFg,
        SelectionForeColor = AppConfig.WindowTitleFg,
        BackColor = AppConfig.WindowTitleBg,
        SelectionBackColor = AppConfig.WindowTitleBg
      };
    }

    private void soldierPerksGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      // hot-track soldier perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      for (int i = 0; i < rosterGridView.Rows.Count; i++)
      {
        rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      if (e.RowIndex == 0 || ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style.BackColor == AppConfig.ChecklistGoodTabUnselectedBg) return;
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = hotStyle;
    }

    private void checklistGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == 0) return;
      // hot-track soldier perk list
      List<Tuple<string, long>> perkedSquadSoldiers = [];
      List<Tuple<string, long>> perkedRosterSoldiers = [];

      string cellValue = (((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "";

      for (int i = 0; i < Shortlist.Count; i++)
        if (Shortlist[i].Perks.Select(x => x.Name).Contains(cellValue.TrimStart('(').TrimEnd(')')))
          perkedSquadSoldiers.Add(new(Shortlist[i].LName, Shortlist[i].Xp));

      hotStyle = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style;

      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        if (perkedSquadSoldiers.Contains(new((((squadGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""),
            Int64.Parse(((squadGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "")))))
          squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
          {
            ForeColor = AppConfig.WindowTitleFg,
            SelectionForeColor = AppConfig.WindowTitleFg,
            BackColor = AppConfig.WindowTitleBg,
            SelectionBackColor = AppConfig.WindowTitleBg
          };
        else squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }

      List<Soldier> filteredRoster = RosterPerkTree.Length == 0 ? RosterTree1.SubRoster : RosterPerkTree.Length == 1 ? RosterTree2.SubRoster : RosterPerkTree.Length == 2 ? RosterTree3.SubRoster : RosterTree4.SubRoster;

      for (int i = 0; i < filteredRoster.Count; i++)
        if (filteredRoster[i].Perks.Select(x => x.Name).Contains(((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? "").TrimEnd(" ♥".ToCharArray()) ?? ""))
          perkedRosterSoldiers.Add(new(filteredRoster[i].LName, filteredRoster[i].Xp));

      int lastIndex = rosterGridView.FirstDisplayedScrollingRowIndex + 26 - Shortlist.Count;

      for (int i = rosterGridView.FirstDisplayedScrollingRowIndex; i < lastIndex && i < rosterGridView.RowCount; i++)
      {
        for (int j = 0; j < perkedRosterSoldiers.Count; j++)
        {
          if (rosterGridView.Rows.Count > 0)
            if (((rosterGridView.Rows[i].Cells[0].Value ?? "").ToString() ?? "").Contains(perkedRosterSoldiers[j].Item1)
              && Int64.Parse((rosterGridView.Rows[i].Cells[10].Value ?? "").ToString() ?? "") == perkedRosterSoldiers[j].Item2)
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.WindowTitleFg,
                SelectionForeColor = AppConfig.WindowTitleFg,
                BackColor = AppConfig.WindowTitleBg,
                SelectionBackColor = AppConfig.WindowTitleBg
              };
              break;
            }
            else
            {
              rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
              {
                ForeColor = AppConfig.GridCellFg,
                SelectionForeColor = AppConfig.GridCellFg,
                BackColor = AppConfig.GridCellBg,
                SelectionBackColor = AppConfig.GridCellBg
              };
            }
        }
      }
      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[2].Style = new()
      {
        ForeColor = AppConfig.WindowTitleFg,
        SelectionForeColor = AppConfig.WindowTitleFg,
        BackColor = AppConfig.WindowTitleBg,
        SelectionBackColor = AppConfig.WindowTitleBg
      };
    }

    private void checklistGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == 0) return;

      // hot-track soldier perk list
      for (int i = 0; i < squadGridView.Rows.Count; i++)
      {
        squadGridView.Rows[i].Cells[0].Style = squadGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }
      for (int i = 0; i < rosterGridView.Rows.Count; i++)
      {
        rosterGridView.Rows[i].Cells[0].Style = rosterGridView.Rows[i].Cells[1].Style = new()
        {
          ForeColor = AppConfig.GridCellFg,
          SelectionForeColor = AppConfig.GridCellFg,
          BackColor = AppConfig.GridCellBg,
          SelectionBackColor = AppConfig.GridCellBg
        };
      }

      ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Style = ((DataGridView)sender).Rows[e.RowIndex].Cells[2].Style = hotStyle;
    }

    private void squadGridView_MouseWheel(object sender, MouseEventArgs e)
    {
      var point = ((DataGridView)sender).PointToClient(Cursor.Position);
      var info = ((DataGridView)sender).HitTest(point.X, point.Y);

      if (info.RowIndex == -1) return;

      DataGridView dgv = ((DataGridView)sender);

      int totalRows = dgv.Rows.Count;
      DataGridViewRow selectedRow = dgv.Rows[info.RowIndex];

      string lname = (((DataGridView)sender).Rows[info.RowIndex].Cells[0].Value ?? "").ToString() ?? "";
      long xp = Int64.Parse((((DataGridView)sender).Rows[info.RowIndex].Cells[10].Value ?? "0").ToString() ?? "0");

      if (e.Delta > 0 && info.RowIndex > 0 && info.ColumnIndex >= 0)
      {
        dgv.Rows.Remove(selectedRow);
        dgv.Rows.Insert(info.RowIndex - 1, selectedRow);
        if (info.RowIndex > 0) dgv.Rows[info.RowIndex - 1].Cells[0].Style = dgv.Rows[info.RowIndex - 1].Cells[1].Style = hotStyle;
        dgv.ClearSelection();
        if (info.RowIndex > 0) dgv.Rows[info.RowIndex - 1].Cells[info.ColumnIndex].Selected = true;
        for (int i = 0; i < totalRows; i++) dgv.Rows[i].Cells[12].Value = i + 1;
        if (info.RowIndex == (int?)(squadSizeCombx.SelectedItem) || info.RowIndex == (int?)(squadSizeCombx.SelectedItem) + 1)
        {
          PopulateChecklist();
        }
      }
      else if (e.Delta < 0 && info.RowIndex < totalRows - 1 && info.ColumnIndex >= 0)
      {
        dgv.Rows.Remove(selectedRow);
        dgv.Rows.Insert(info.RowIndex + 1, selectedRow);
        if (info.RowIndex > 0) dgv.Rows[info.RowIndex - 1].Cells[0].Style = dgv.Rows[info.RowIndex - 1].Cells[1].Style = hotStyle;
        dgv.ClearSelection();
        if (info.RowIndex > 0) dgv.Rows[info.RowIndex - 1].Cells[info.ColumnIndex].Selected = true;
        for (int i = 0; i < totalRows; i++) dgv.Rows[i].Cells[12].Value = i + 1;
        if (info.RowIndex == (int?)(squadSizeCombx.SelectedItem) || info.RowIndex == (int?)(squadSizeCombx.SelectedItem) - 1)
        {
          PopulateChecklist();
        }
      }

      int vscroll = rosterGridView.FirstDisplayedScrollingRowIndex;

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

      if (vscroll > 0 && rosterGridView.Rows.Count >= vscroll - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll;
    }

    private void squadGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left) PopupateSoldierPerks(e.RowIndex, (DataGridView)sender);
      else if (e.Button == MouseButtons.Right) ToggleInShortlist(sender);
      else if (e.Button == MouseButtons.Middle)
      {
        if (e.ColumnIndex >= 0)
        {
          var point = ((DataGridView)sender).PointToClient(Cursor.Position);
          var info = ((DataGridView)sender).HitTest(point.X, point.Y);
          if (info.RowIndex > 0)
          {
            DataGridView dgv = ((DataGridView)sender);
            DataGridViewRow selectedRow = dgv.Rows[info.RowIndex];
            dgv.Rows.Remove(selectedRow);
            dgv.Rows.Insert(0, selectedRow);
            if (dgv.Rows.Count >= (int?)(squadSizeCombx.SelectedItem)) dgv.Rows[(int)(squadSizeCombx.SelectedItem)].Cells[19].Value = false;
            dgv.Rows[0].Cells[0].Style = dgv.Rows[0].Cells[1].Style = hotStyle;
            dgv.Rows[0].Cells[(dgv.SelectedCells[0].OwningColumn ?? new()).Index].Selected = true;
            for (int i = 0; i < dgv.Rows.Count; i++) dgv.Rows[i].Cells[12].Value = i + 1;
          }
        }

        int vscroll = rosterGridView.FirstDisplayedScrollingRowIndex;

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

        if (vscroll > 0 && rosterGridView.Rows.Count >= vscroll - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll;
      }

      PopulateChecklist();
    }

    private void timerLabel_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics?.InterpolationMode = InterpolationMode.Bilinear;
      e.Graphics?.PixelOffsetMode = PixelOffsetMode.HighSpeed;
      e.Graphics?.SmoothingMode = SmoothingMode.AntiAlias;
      e.Graphics?.FillRoundedRectangle(new SolidBrush(Color.Black), new(e.ClipRectangle.X, e.ClipRectangle.Y - 2, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 6), new(4, 4));
      TextRenderer.DrawText(
            e.Graphics ?? ((System.Windows.Forms.Label)sender).CreateGraphics(),
            ((System.Windows.Forms.Label)sender).Text,
            SevenSegmentFont,
            new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 4, e.ClipRectangle.Height - 4),
            Color.Red,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }

    private void rosterGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex == -1 && e.RowIndex == -1)
      {
        if (ChatGemActivated is null)
        {
          scoreLabel.Text = "";
          scoreLabel.Visible = false;
          ChatGemActivated = false;
        }

        ChatGemActivated = !ChatGemActivated;

        if (DateTime.Now.Millisecond % 40 == 0)
        {
          scoreLabel.Text = "PERFECT GEM ACTIVATED";
          scoreLabel.ForeColor = AppConfig.ChatGemPerfect;
          scoreLabel.BackColor = Color.Black;
          scoreLabel.TextAlign = ContentAlignment.MiddleCenter;
          scoreLabel.Visible = true;
          ChatGemActivated = null;
        }

        int vscroll = rosterGridView.FirstDisplayedScrollingRowIndex;
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

        if (vscroll > 0 && rosterGridView.Rows.Count >= vscroll - 1) rosterGridView.FirstDisplayedScrollingRowIndex = vscroll;
      }
    }

    private void squadGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
      Rectangle firstTwo = new(e.RowBounds.X,
        e.RowBounds.Y,
         ((DataGridView)sender).Rows[e.RowIndex].HeaderCell.Size.Width +
          ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Size.Width +
          ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Size.Width,
        e.RowBounds.Height
      );

      Rectangle lastThree = new(e.RowBounds.Width -
          ((DataGridView)sender).Rows[e.RowIndex].Cells[10].Size.Width -
          ((DataGridView)sender).Rows[e.RowIndex].Cells[11].Size.Width -
          ((DataGridView)sender).Rows[e.RowIndex].Cells[12].Size.Width,
        e.RowBounds.Y,
        ((DataGridView)sender).Rows[e.RowIndex].Cells[10].Size.Width +
          ((DataGridView)sender).Rows[e.RowIndex].Cells[11].Size.Width +
          ((DataGridView)sender).Rows[e.RowIndex].Cells[12].Size.Width,
        e.RowBounds.Height
      );

      if (e.RowIndex >= (int?)squadSizeCombx.SelectedItem)
      {
        e.Graphics?.FillRectangle(new SolidBrush(AppConfig.NonSquadShortlistOverlay), firstTwo);
        e.Graphics?.FillRectangle(new SolidBrush(AppConfig.NonSquadShortlistOverlay), lastThree);

        if (e.RowIndex == (int?)squadSizeCombx.SelectedItem)
        {
          e.Graphics?.DrawLine(new(AppConfig.GridBg, 3F), new Point(e.RowBounds.X, e.RowBounds.Y), new Point(e.RowBounds.X + e.RowBounds.Width, e.RowBounds.Y));
        }
      }
      else
      {
        Color rowOverlay = e.RowIndex switch
        {
          0 => AppConfig.Row1ShortlistOverlay,
          1 => AppConfig.Row2ShortlistOverlay,
          2 => AppConfig.Row3ShortlistOverlay,
          3 => AppConfig.Row4ShortlistOverlay,
          4 => AppConfig.Row5ShortlistOverlay,
          5 => AppConfig.Row6ShortlistOverlay,
          6 => AppConfig.Row7ShortlistOverlay,
          7 => AppConfig.Row8ShortlistOverlay,
          8 => AppConfig.Row9ShortlistOverlay,
          9 => AppConfig.Row10ShortlistOverlay,
          10 => AppConfig.Row11ShortlistOverlay,
          11 => AppConfig.Row12ShortlistOverlay,
          12 => AppConfig.Row13ShortlistOverlay,
          13 => AppConfig.Row14ShortlistOverlay,
          14 => AppConfig.Row15ShortlistOverlay,
          15 => AppConfig.Row16ShortlistOverlay,
          _ => AppConfig.NonSquadShortlistOverlay
        };

        e.Graphics?.FillRectangle(new SolidBrush(rowOverlay), firstTwo);
        e.Graphics?.FillRectangle(new SolidBrush(rowOverlay), lastThree);
      }
    }

    private void rosterGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
      if (e.RowIndex % 2 == 0)
      {
        Rectangle firstTwo = new(e.RowBounds.X,
        e.RowBounds.Y,
         ((DataGridView)sender).Rows[e.RowIndex].HeaderCell.Size.Width +
          ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Size.Width +
          ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Size.Width,
        e.RowBounds.Height
        );

        Rectangle lastThree = new(e.RowBounds.Width -
            ((DataGridView)sender).Rows[e.RowIndex].Cells[10].Size.Width -
            ((DataGridView)sender).Rows[e.RowIndex].Cells[11].Size.Width -
            ((DataGridView)sender).Rows[e.RowIndex].Cells[12].Size.Width,
          e.RowBounds.Y,
          ((DataGridView)sender).Rows[e.RowIndex].Cells[10].Size.Width +
            ((DataGridView)sender).Rows[e.RowIndex].Cells[11].Size.Width +
            ((DataGridView)sender).Rows[e.RowIndex].Cells[12].Size.Width,
          e.RowBounds.Height
        );

        Color rowOverlay = AppConfig.RowZebraOverlay;

        e.Graphics?.FillRectangle(new SolidBrush(rowOverlay), firstTwo);
        e.Graphics?.FillRectangle(new SolidBrush(rowOverlay), lastThree);
      }
    }

    private void treeDataGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
      if (!PerkTreeByName) e.Graphics?.FillRectangle(new SolidBrush(Color.FromArgb(32, AppConfig.TreeHeaderBg)), new(e.RowBounds.Width -
          ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Size.Width,
        e.RowBounds.Y,
          ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Size.Width,
        e.RowBounds.Height
      ));
      if (e.RowIndex % 2 == 0) e.Graphics?.FillRectangle(new SolidBrush(AppConfig.RowZebraOverlay), e.RowBounds);

      if (ChecklistPerks.ContainsKey((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
      {
        e.Graphics?.FillRectangle(new SolidBrush(Color.FromArgb(96, AppConfig.ChecklistedPerkBg)), e.RowBounds);
      }
    }

    private void squadPerkList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
      if (!SquadPerksByName) e.Graphics?.FillRectangle(new SolidBrush(Color.FromArgb(32, AppConfig.SquadHeaderBg)), new(e.RowBounds.Width -
          ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Size.Width,
        e.RowBounds.Y,
          ((DataGridView)sender).Rows[e.RowIndex].Cells[1].Size.Width,
        e.RowBounds.Height
      ));
      if (e.RowIndex % 2 == 0) e.Graphics?.FillRectangle(new SolidBrush(AppConfig.RowZebraOverlay), e.RowBounds);

      if (ChecklistPerks.ContainsKey((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
      {
        e.Graphics?.FillRectangle(new SolidBrush(Color.FromArgb(96, AppConfig.ChecklistedPerkBg)), e.RowBounds);
      }
    }

    private void soldierPerksGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
      if (e.RowIndex % 2 == 0) e.Graphics?.FillRectangle(new SolidBrush(AppConfig.RowZebraOverlay), e.RowBounds);

      if (ChecklistPerks.ContainsKey((((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value ?? "").ToString() ?? ""))
      {
        e.Graphics?.FillRectangle(new SolidBrush(Color.FromArgb(96, AppConfig.ChecklistedPerkBg)), e.RowBounds);
      }    }
  }
}
