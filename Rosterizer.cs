using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace RosterizerLW
{
  public partial class Rosterizer : Form
  {
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

    static readonly double indPct = 1.1;
    static readonly double indPct2 = 1.05;
    static readonly double indPct3 = 1.45;
    static readonly int minHiLoListSize = 7;
    static readonly int minMinMaxListSize = 4;

    bool maxHp = false, hiHp = false, loHp = false, minHp = false;
    bool maxMob = false, hiMob = false, loMob = false, minMob = false;
    bool maxAim = false, hiAim = false, loAim = false, minAim = false;
    bool maxWill = false, hiWill = false, loWill = false, minWill = false;
    bool maxDef = false, hiDef = false, loDef = false, minDef = false;
    public Rosterizer()
    {
      InitializeComponent();

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

      Text = $"{ConsoleApp.SaveFile.Name}  -  {(ConsoleApp.SaveParsed.Header.Save_description ?? new()).Str}";

      this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
      this.tabControl1.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);
      ListRoster();
      ResetRosterPerkList();
      PopupateSoldierPerks(0);
    }

    public void ResetChecklist()
    {
      checklistGridView.Rows.Clear();
      ConsoleApp.ChecklistPerks.ToList().ForEach(x => checklistGridView.Rows.Add(x.Key, x.Value));

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

      ConsoleApp.ChecklistPass = checklistOk == ConsoleApp.ChecklistPerks.Count;
      string checkMark = ConsoleApp.ChecklistPass ? "✓" : "ⅹ";
      (tabControl1.TabPages[3] ?? new()).Text = $"{checklistOk}/{ConsoleApp.ChecklistPerks.Count} {checkMark}";
    }

    public void ListRoster(bool squadOnly = false)
    {
      rosterGridView.Rows.Clear();
      squadPerkList.Rows.Clear();
      ConsoleApp.SquadPerks.Clear();

      List<Soldier> filteredSoldiers = [];

      foreach (Soldier s in ConsoleApp.Roster)
      {
        if (!shivCheckbox.Checked && s.IsShiv) continue;
        if (!woundedCheckbox.Checked && s.IsWounded && s.HoursOut > ConsoleApp.RecoverableHrs) continue;
        if (s.IsBlueshirt && !s.IsShiv) continue;
        if (!deadCheckbox.Checked && s.IsDead) continue;
        if (!fatiguedCheckbox.Checked && s.IsFatigued && s.HoursOut > ConsoleApp.RecoverableHrs) continue;

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
            if (ConsoleApp.SquadPerks.TryAdd(p.Name, 1)) continue;
            else ConsoleApp.SquadPerks[p.Name]++;
          }
        }
      }

      foreach (var p in ConsoleApp.SquadPerks) squadPerkList.Rows.Add([p.Key, p.Value]);

      int filteredSoldierIndex = 0;

      maxHp = hiHp = loHp = minHp = false;
      maxMob = hiMob = loMob = minMob = false;
      maxAim = hiAim = loAim = minAim = false;
      maxWill = hiWill = loWill = minWill = false;
      maxDef = hiDef = loDef = minDef = false;

      int inSquad = filteredSoldiers.Where(x => x.InSquad).Count();

      RosterSort thisSort = ConsoleApp.Sorting[0];
      RosterSort lastSort = ConsoleApp.Sorting[1];
      bool invertSort = false;

      // do this better
      //for (int i = ConsoleApp.Sorting.Length - 1; i > 0; i--)
      for (int i = 1; i < ConsoleApp.Sorting.Length; i++)
      {
        if (ConsoleApp.Sorting[i] == ConsoleApp.Sorting[i - 1])
        {
          invertSort = true;
          i++;
        }
        switch (thisSort)
        {
          case RosterSort.LName:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderByDescending(x => x.LName)] : [.. filteredSoldiers.OrderBy(x => x.LName)];
            break;
          case RosterSort.NName:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderByDescending(x => x.NName)] : [.. filteredSoldiers.OrderBy(x => x.NName)];
            break;
          case RosterSort.Status:
            if (invertSort) filteredSoldiers = [.. filteredSoldiers.OrderByDescending(x => x.Status).ThenByDescending(x => x.IsWounded).ThenByDescending(x => x.HoursOut)];
            else filteredSoldiers = [.. filteredSoldiers.OrderBy(x => x.Status).ThenBy(x => x.IsWounded).ThenBy(x => x.HoursOut)];
            break;
          case RosterSort.Class:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderByDescending(x => x.Class)] : [.. filteredSoldiers.OrderBy(x => x.Class)];
            break;
          case RosterSort.Def:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.Stats.Defense)] : [.. filteredSoldiers.OrderByDescending(x => x.Stats.Defense)];
            break;
          case RosterSort.HP:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.Stats.HP)] : [.. filteredSoldiers.OrderByDescending(x => x.Stats.HP)];
            break;
          case RosterSort.Mob:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.Stats.Mobility)] : [.. filteredSoldiers.OrderByDescending(x => x.Stats.Mobility)];
            break;
          case RosterSort.Will:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.Stats.Will)] : [.. filteredSoldiers.OrderByDescending(x => x.Stats.Will)];
            break;
          case RosterSort.Aim:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.Stats.Aim)] : [.. filteredSoldiers.OrderByDescending(x => x.Stats.Aim)];
            break;
          case RosterSort.Xp:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.Xp)] : [.. filteredSoldiers.OrderByDescending(x => x.Xp)];
            break;
          case RosterSort.Rank:
            filteredSoldiers = invertSort ? [.. filteredSoldiers.OrderBy(x => x.RankId)] : [.. filteredSoldiers.OrderByDescending(x => x.RankId)];
            break;
        }
        lastSort = thisSort;
        invertSort = false;
      }

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
            hiHp = !minHp && !maxHp && f.Stats.HP * indPct >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.HP);
            hiMob = !minMob && !maxMob && f.Stats.Mobility * indPct >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Mobility);
            hiAim = !minAim && !maxAim && f.Stats.Aim * indPct2 >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Aim);
            hiWill = !minWill && !maxWill && f.Stats.Will * indPct2 >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Will);
            hiDef = !minDef && !maxDef && f.Stats.Defense * indPct3 >= filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Max(x => x.Stats.Defense);

            loHp = !hiHp && !minHp && !maxHp && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.HP) * indPct >= f.Stats.HP;
            loMob = !hiMob && !minMob && !maxMob && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Mobility) * indPct >= f.Stats.Mobility;
            loAim = !hiAim && !minAim && !maxAim && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Aim) * indPct2 >= f.Stats.Aim;
            loWill = !hiWill && !minWill && !maxWill && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Will) * indPct2 >= f.Stats.Will;
            loDef = !hiDef && !minDef && !maxDef && filteredSoldiers.Where(x => !x.IsShiv && !x.IsDead && !x.IsWounded).Min(x => x.Stats.Defense) * indPct3 >= f.Stats.Defense;
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

        long toNext = f.RankId > 0 && f.RankId < ConsoleApp.XpLvls.Length ? ConsoleApp.XpLvls[f.RankId] - f.Xp : 0;

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
          toNext,
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
        else if (!f.IsDead && ((!f.IsWounded && !f.IsFatigued) || f.HoursOut <= ConsoleApp.RecoverableHrs))
        {
          foreach (Perk p in f.Perks)
          {
            if (ConsoleApp.ChecklistPerks.ContainsKey(p.Name ?? ""))
            {
              f.HasChecklistPerk = true;
              break;
            }
          }
        }

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

        if (f.IsShiv) thisRow.DefaultCellStyle = new() { BackColor = shivBg, SelectionBackColor = shivBg };
        else if (f.IsDead) thisRow.DefaultCellStyle = new() { BackColor = deadBg, ForeColor = deadFg, SelectionBackColor = deadBg };

        if (f.LName == "TamTam") thisRow.Cells["LName"].Value = $"TamTam ♥";
        if (f.LName == "ParkaBuoy") thisRow.Cells["LName"].Value = $"ParkaBuoy {new string(' ', DateTime.Now.Second % 5)}{PokemonPicker(f.InSquad ? 1 : 0)}";
        if (!f.IsWounded && !f.IsBlueshirt && !f.IsDead && !f.IsFatigued && !f.InSquad && DateTime.Now.Microsecond % 100 == 0 && DateTime.Now.Millisecond % 50 == 0)
        {
          thisRow.DefaultCellStyle = new()
          {
            ForeColor = Color.FromArgb(180, 0, 0, 0),
            BackColor = Color.FromArgb(255, 255, 215, 0),
            SelectionBackColor = Color.FromArgb(255, 255, 215, 0),
            Font = new(Font, FontStyle.Bold),
            WrapMode = DataGridViewTriState.True
          };
          thisRow.Cells["LName"].Value = $"*~･ﾟ✧~  {thisRow.Cells["LName"].Value}  ~✧･ﾟ~*";
        }

        if (!f.IsDead)
        {
          if (f.HoursOut <= ConsoleApp.RecoverableHrs)
          {
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg, Font = new(Font, FontStyle.Bold) };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg, Font = new(Font, FontStyle.Bold) };
          }
          else
          {
            if (f.IsWounded) thisRow.Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg };
            else if (f.IsFatigued) thisRow.Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg };
          }
        }

        filteredSoldierIndex++;
      }

      ResetChecklist();
    }

    private void ResetRosterPerkList()
    {
      ConsoleApp.RosterPerks.Clear();
      rosterPerkList.Rows.Clear();

      rosterPerkList.Rows.Add("[ANY]", 0);
      rosterPerkList.Rows[0].Height = rosterPerkList.Rows[0].Height + 4;
      rosterPerkList.Rows[0].DividerHeight += 4;

      foreach (Soldier thisSoldier in ConsoleApp.Roster)
      {
        if (!thisSoldier.IsDead && (!thisSoldier.IsWounded || thisSoldier.HoursOut <= ConsoleApp.RecoverableHrs) && !thisSoldier.IsBlueshirt)
        {
          foreach (Perk p in thisSoldier.Perks)
          {
            if (ConsoleApp.RosterPerks.TryAdd(p.Name ?? "", 1)) continue;
            else ConsoleApp.RosterPerks[p.Name ?? ""]++;
          }
        }
      }

      foreach (string p in ConsoleApp.PerkNames.OrderBy(x => x))
      {
        ConsoleApp.RosterPerks.TryGetValue(p, out int perkCount);
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

      // don't draw zeroes for nextlvl (shiv/msgt)
      else if (e.RowIndex >= 0 && e.ColumnIndex == 10 && ((long?)e.Value ?? 0) == 0)
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
            Soldier? s = ConsoleApp.Roster.FirstOrDefault(x => ((r.Cells["LName"].Value ?? "").ToString() ?? "").Contains(x.LName));
            if (s is not null && !s.IsDead && (!s.IsWounded || s.HoursOut <= ConsoleApp.RecoverableHrs))
            {
              s?.InSquad = !s.InSquad;
              ListRoster();
            }
          }
        }
        else
        {
          ConsoleApp.Sorting = ConsoleApp.DefaultSorting;
          ListRoster();
        }
      }
      else if (e.Button == MouseButtons.Left)
      {
        if (e.RowIndex == -1)
        {
          switch (e.ColumnIndex)
          {
            case 0:
              AddSort(RosterSort.LName);
              break;
            case 1:
              AddSort(RosterSort.NName);
              break;
            case 2:
              AddSort(RosterSort.Status);
              break;
            case 3:
              AddSort(RosterSort.Class);
              break;
            case 4:
              AddSort(RosterSort.Def);
              break;
            case 5:
              AddSort(RosterSort.HP);
              break;
            case 6:
              AddSort(RosterSort.Mob);
              break;
            case 7:
              AddSort(RosterSort.Will);
              break;
            case 8:
              AddSort(RosterSort.Aim);
              break;
            case 9:
              AddSort(RosterSort.Xp);
              break;
            case 10:
              AddSort(RosterSort.Xp);
              return;
            case 11:
              AddSort(RosterSort.Rank);
              break;
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
        Soldier? s = ConsoleApp.Roster.FirstOrDefault(x => ((r.Cells["LName"].Value ?? "").ToString() ?? "").Contains(x.LName));
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

    private void AddSort(RosterSort sort)
    {
      ConsoleApp.Sorting = [.. ConsoleApp.Sorting.Prepend(sort)];
      if (ConsoleApp.Sorting.Length > 5) ConsoleApp.Sorting = [.. ConsoleApp.Sorting.SkipLast(1)];
    }

    private void rosterPerkList_SelectionChanged(object sender, EventArgs e)
    {
      if (rosterPerkList.SelectedRows.Count > 0 && rosterPerkList.SelectedRows[0].Index > 0)
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
          ConsoleApp.NavFromChecklist = true;

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

      if (ConsoleApp.NavFromChecklist)
      {
        ConsoleApp.NavFromChecklist = false;
        tabControl1.SelectedIndex = 3;
      }
      else ListRoster();
    }

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
          if (ConsoleApp.ChecklistPass)
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
            Rectangle r = new Rectangle(recBounds.X + 2, recBounds.Y + 2, recBounds.Width - 4, recBounds.Height - 4);

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
      if (tabControl1.SelectedIndex == 0)
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
        ConsoleApp.NavFromChecklist = true;
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

      ConsoleApp.ChecklistPass = false;
      timerLabel.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      timerLabel.ForeColor = windowTitleFg;
      foreach (Soldier s in ConsoleApp.Roster) s.InSquad = false;
      tabControl1.SelectedIndex = 0;
      ResetRosterPerkList();
      ListRoster();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (ConsoleApp.ChecklistPass)
      {
        timerLabel.Font = new Font("Courier New", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        timerLabel.ForeColor = maxBg;
        timerRunning = false;
        timer2.Stop();
        timer2.Reset();
        timerStartStopButton.Text = "Start";
      }
      else
        if (timerRunning)
        {
          timerLabel.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
          timerLabel.ForeColor = windowTitleFg;
          elapsedTime = timer2.Elapsed;
        }
      timerLabel.Text = elapsedTime.ToString("G").Substring(0, 14);
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
      ConsoleApp.BlueshirtLvl = ((TrackBar)sender).Value;
      ConsoleApp.Roster.ForEach(x => x.IsBlueshirt = x.RankId <= ConsoleApp.BlueshirtLvl);
      ListRoster();
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
  }
}
