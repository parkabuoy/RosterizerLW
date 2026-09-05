using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;


namespace Rosterizer
{
  public partial class Rosterizer : Form
  {

    static readonly Color deadBg = Color.Coral;
    static readonly Color deadFg = Color.DarkRed;
    static readonly Color woundBg = Color.FromArgb(255, 221, 221, 181);
    static readonly Color blueshirtBg = Color.FromArgb(255, 125, 180, 221);
    static readonly Color shivBg = Color.FromArgb(255, 157, 157, 157);
    static readonly Color maxBg = Color.FromArgb(255, 162, 198, 162);
    static readonly Color hiBg = Color.FromArgb(255, 181, 221, 181);
    static readonly Color loBg = Color.FromArgb(255, 227, 194, 194);
    static readonly Color minBg = Color.FromArgb(255, 215, 168, 168);
    
    static readonly double indPct = 1.1;
    static readonly double indPct2 = 1.05;
    static readonly double indPct3 = 1.45;
    static readonly int minHiLoListed = 6;

    bool maxHp = false, hiHp = false, loHp = false, minHp = false;
    bool maxMob = false, hiMob = false, loMob = false, minMob = false;
    bool maxAim = false, hiAim = false, loAim = false, minAim = false;
    bool maxWill = false, hiWill = false, loWill = false, minWill = false;
    bool maxDef = false, hiDef = false, loDef = false, minDef = false;
    public Rosterizer()
    {
      InitializeComponent();
      Text = $"{ConsoleApp.SaveFile.Name}  -  {(ConsoleApp.SaveParsed.Header.Save_description ?? new()).Str}";
      perkComboBox1.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x)]);
      perkComboBox2.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x)]);
      perkComboBox3.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x)]);

      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    public void ListRoster(string[]? boolFilters = null, string[]? perkFilter = null)
    {
      rosterGridView.Rows.Clear();

      List<Soldier> filteredSoldiers = [];
      int filteredSoldiersCount = 0;

      foreach (Soldier s in ConsoleApp.Roster)
      {
        bool?[] filterPass = [];
        perkFilter ??= [];

        if (boolFilters is not null && boolFilters.Length > 0)
        {
          filterPass = new bool?[boolFilters.Length];
          for (int i = 0; i < boolFilters.Length; i++)
          {
            switch (boolFilters[i])
            {
              case "wounded":
                filterPass[i] = !s.IsWounded;
                break;
              case "blue":
                filterPass[i] = !s.IsBlueshirt;
                break;
              case "dead":
                filterPass[i] = !s.IsDead;
                break;
              case "shiv":
                filterPass[i] = !s.IsShiv;
                break;
            }
          }
        }

        if (filterPass.Length > 0 && filterPass.Any(x => x == false)) continue;
        if (perkFilter.Any(x => !string.IsNullOrWhiteSpace(x)))
        {
          filterPass = new bool?[perkFilter.Length];

          for (int i = 0; i < 3; i++)
          {
            if (i == 0 && perkComboBox1.SelectedIndex == -1) filterPass[i] = true;
            else if (i == 1 && perkComboBox2.SelectedIndex == -1) filterPass[i] = true;
            else if (i == 2 && perkComboBox3.SelectedIndex == -1) filterPass[i] = true;
            
            
            bool perkFilterPass = false;
            foreach (string? sPerkName in s.Perks.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x)))
            {
              if (sPerkName == perkFilter[i])
              {
                perkFilterPass = true;
                break;
              }
            }
            filterPass[i] = perkFilterPass;
          }
        }

        if (filterPass.Any(x => x == true))
        {
          filteredSoldiersCount++;
          filteredSoldiers.Add(s);
        }
      }

      int filteredSoldierIndex = 0;

      maxHp = hiHp = loHp = minHp = false;
      maxMob = hiMob = loMob = minMob = false;
      maxAim = hiAim = loAim = minAim = false;
      maxWill = hiWill = loWill = minWill = false;
      maxDef = hiDef = loDef = minDef = false;

      foreach (Soldier f in filteredSoldiers)
      {
        if (filteredSoldiers.Count > 2)
        {
          maxHp = f.Stats.HP == filteredSoldiers.Max(x => x.Stats.HP);
          maxMob = f.Stats.Mobility == filteredSoldiers.Max(x => x.Stats.Mobility);
          maxAim = f.Stats.Aim == filteredSoldiers.Max(x => x.Stats.Aim);
          maxWill = f.Stats.Will == filteredSoldiers.Max(x => x.Stats.Will);
          maxDef = f.Stats.Defense == filteredSoldiers.Max(x => x.Stats.Defense);

          minHp = !maxHp && f.Stats.HP == filteredSoldiers.Min(x => x.Stats.HP);
          minMob = !maxMob && f.Stats.Mobility == filteredSoldiers.Min(x => x.Stats.Mobility);
          minAim = !maxAim && f.Stats.Aim == filteredSoldiers.Min(x => x.Stats.Aim);
          minWill = !maxWill && f.Stats.Will == filteredSoldiers.Min(x => x.Stats.Will);
          minDef = !maxDef && f.Stats.Defense == filteredSoldiers.Min(x => x.Stats.Defense);

          if (filteredSoldiers.Count >= minHiLoListed)
          {
            hiHp = !minHp && !maxHp && f.Stats.HP * indPct >= filteredSoldiers.Max(x => x.Stats.HP);
            hiMob = !minMob && !maxMob && f.Stats.Mobility * indPct >= filteredSoldiers.Max(x => x.Stats.Mobility);
            hiAim = !minAim && !maxAim && f.Stats.Aim * indPct2 >= filteredSoldiers.Max(x => x.Stats.Aim);
            hiWill = !minWill && !maxWill && f.Stats.Will * indPct2 >= filteredSoldiers.Max(x => x.Stats.Will);
            hiDef = !minDef && !maxDef && f.Stats.Defense * indPct3 >= filteredSoldiers.Max(x => x.Stats.Defense);

            loHp = !hiHp && !minHp && !maxHp && filteredSoldiers.Min(x => x.Stats.HP) * indPct >= f.Stats.HP;
            loMob = !hiMob && !minMob && !maxMob && filteredSoldiers.Min(x => x.Stats.Mobility) * indPct >= f.Stats.Mobility;
            loAim = !hiAim && !minAim && !maxAim && filteredSoldiers.Min(x => x.Stats.Aim) * indPct2 >= f.Stats.Aim;
            loWill = !hiWill && !minWill && !maxWill && filteredSoldiers.Min(x => x.Stats.Will) * indPct2 >= f.Stats.Will;
            loDef = !hiDef && !minDef && !maxDef && filteredSoldiers.Min(x => x.Stats.Defense) * indPct3 >= f.Stats.Defense;
          }
        }

        object[] dgvrVals =
        {
          f.LName,
          f.NName,
          f.Status == "Healing" ? ((f.FatigueHrs / 24) > 0 ? $"{f.FatigueHrs / 24}d " : "") + $"{f.FatigueHrs % 24}h" : f.Status,
          f.Class,
          f.Stats.Defense,
          f.Stats.HP,
          f.Stats.Mobility,
          f.Stats.Will,
          f.Stats.Aim,
          f.Xp,
          f.IsShiv ? "" : f.Rank.ToString(),
        };

        rosterGridView.Rows.Add(dgvrVals);
        if (f.Status == "Healing")        

        if (maxAim) rosterGridView.Rows[filteredSoldierIndex].Cells["Aim"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
        else if (minAim) rosterGridView.Rows[filteredSoldierIndex].Cells["Aim"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
        else if (hiAim) rosterGridView.Rows[filteredSoldierIndex].Cells["Aim"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
        else if (loAim) rosterGridView.Rows[filteredSoldierIndex].Cells["Aim"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };
        
        if (maxMob) rosterGridView.Rows[filteredSoldierIndex].Cells["Mob"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
        else if (minMob) rosterGridView.Rows[filteredSoldierIndex].Cells["Mob"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
        else if (hiMob) rosterGridView.Rows[filteredSoldierIndex].Cells["Mob"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
        else if (loMob) rosterGridView.Rows[filteredSoldierIndex].Cells["Mob"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };
        
        if (maxHp) rosterGridView.Rows[filteredSoldierIndex].Cells["HP"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
        else if (minHp) rosterGridView.Rows[filteredSoldierIndex].Cells["HP"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
        else if (hiHp) rosterGridView.Rows[filteredSoldierIndex].Cells["HP"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
        else if (loHp) rosterGridView.Rows[filteredSoldierIndex].Cells["HP"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };
        
        if (maxWill) rosterGridView.Rows[filteredSoldierIndex].Cells["Will"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
        else if (minWill) rosterGridView.Rows[filteredSoldierIndex].Cells["Will"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
        else if (hiWill) rosterGridView.Rows[filteredSoldierIndex].Cells["Will"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
        else if (loWill) rosterGridView.Rows[filteredSoldierIndex].Cells["Will"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };
        
        if (maxDef) rosterGridView.Rows[filteredSoldierIndex].Cells["Def"].Style = new() { BackColor = maxBg, SelectionBackColor = maxBg };
        else if (minDef) rosterGridView.Rows[filteredSoldierIndex].Cells["Def"].Style = new() { BackColor = minBg, SelectionBackColor = minBg };
        else if (hiDef) rosterGridView.Rows[filteredSoldierIndex].Cells["Def"].Style = new() { BackColor = hiBg, SelectionBackColor = hiBg };
        else if (loDef) rosterGridView.Rows[filteredSoldierIndex].Cells["Def"].Style = new() { BackColor = loBg, SelectionBackColor = loBg };
        
        if (f.IsShiv) rosterGridView.Rows[filteredSoldierIndex].DefaultCellStyle = new() { BackColor = shivBg, SelectionBackColor = shivBg };
        else if (f.IsDead) rosterGridView.Rows[filteredSoldierIndex].DefaultCellStyle = new() { BackColor = deadBg, ForeColor = deadFg, SelectionBackColor = deadBg };
        else if (f.IsBlueshirt) rosterGridView.Rows[filteredSoldierIndex].DefaultCellStyle = new() { BackColor = blueshirtBg, SelectionBackColor = blueshirtBg };
        //else if (f.IsWounded) rosterGridView.Rows[filteredSoldierIndex].DefaultCellStyle = new() { BackColor = woundBg, SelectionBackColor = woundBg };
        else if (f.IsWounded) rosterGridView.Rows[filteredSoldierIndex].Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg };

        filteredSoldierIndex++;
      }

      totalSoldierLabel.Text = $"Total: {ConsoleApp.Roster.Count}";
      shownSoldierLabel.Text = $"Match: {filteredSoldiers.Count}";
      shownSoldierLabel.BackColor = filteredSoldiers.Count == 0 ? Color.Coral : Color.Transparent;
    }

    private void deadCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void woundedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void blueCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void shivCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private string[] GetFilters()
    {
      return [(deadCheckbox.Checked ? "" : "dead"), (woundedCheckbox.Checked ? "" : "wounded"), (blueCheckbox.Checked ? "" : "blue"), (shivCheckbox.Checked ? "" : "shiv")];
    }

    private string[] GetPerkFilters()
    {
      perkComboBox2.Visible = perkComboBox1.SelectedIndex != -1;
      perkComboBox3.Visible = perkComboBox2.Visible && perkComboBox2.SelectedIndex != -1;

      return [(string)(perkComboBox1.SelectedItem ?? ""), (string)(perkComboBox2.SelectedItem ?? ""), (string)(perkComboBox3.SelectedItem ?? "")];
    }

    private void perkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      perkComboBox2.Items.Clear();
      perkComboBox3.Items.Clear();

      perkComboBox2.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x).Where(x => x != (perkComboBox1.SelectedItem ?? "").ToString())]);
      perkComboBox3.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x).Where(x => x != (perkComboBox1.SelectedItem ?? "").ToString())]);

      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      perkComboBox3.Items.Clear();

      perkComboBox3.Items.AddRange([.. ConsoleApp.PerkNames.Where(x => x != (perkComboBox2.SelectedItem ?? "").ToString() && x != (perkComboBox1.SelectedItem ?? "").ToString())]);

      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox3_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        perkComboBox1.SelectedIndex = -1;
        ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

    private void perkComboBox2_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        perkComboBox2.SelectedIndex = -1;
        ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

    private void perkComboBox3_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        perkComboBox3.SelectedIndex = -1;
        ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

    private void Rosterizer_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        foreach (string file in (e.Data.GetData(DataFormats.FileDrop) as string[])) 
        {
          int x = 2; 
        }
      }
    }
    private void rosterGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {

      if (e.RowIndex == -1)
      {
        e.PaintBackground(e.CellBounds, false);
        TextRenderer.DrawText(e.Graphics, string.Format("{0}", e.FormattedValue),
            e.CellStyle.Font, e.CellBounds, e.CellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        e.Handled = true;
      }
    }
  }
}
