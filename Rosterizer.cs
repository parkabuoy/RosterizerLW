using System.Data;
using System.Runtime.InteropServices;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Rosterizer
{
  public partial class Rosterizer : Form
  {
    // muted color scheme
    static readonly Color deadBg = Color.FromArgb(144, 87, 97);
    static readonly Color deadFg = Color.FromArgb(255, 255, 255);
    static readonly Color woundBg = Color.FromArgb(182, 145, 152);
    static readonly Color fatigueBg = Color.FromArgb(194, 194, 194);
    static readonly Color blueshirtBg = Color.FromArgb(137, 144, 177);
    static readonly Color shivBg = Color.FromArgb(157, 157, 157);
    static readonly Color maxBg = Color.FromArgb(137, 177, 170);
    static readonly Color hiBg = Color.FromArgb(172, 200, 195);
    static readonly Color loBg = Color.FromArgb(205, 180, 185);
    static readonly Color minBg = Color.FromArgb(184, 148, 155);
    static readonly Color headerBg = Color.FromArgb(58, 92, 109);
    static readonly Color headerFg = Color.FromArgb(222, 222, 222);
    static readonly Color windowTitleBg = Color.FromArgb(69, 111, 132);
    static readonly Color windowTitleFg = Color.FromArgb(222, 222, 222);
    static readonly Color windowBg = Color.FromArgb(222, 222, 222);

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
      Text = $"{ConsoleApp.SaveFile.Name}  -  {(ConsoleApp.SaveParsed.Header.Save_description ?? new()).Str}";
      perkComboBox4.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x)]);
      rosterGridView.ColumnHeadersDefaultCellStyle.BackColor = headerBg;
      rosterGridView.ColumnHeadersDefaultCellStyle.ForeColor = headerFg;
      tableLayoutPanel3.BackColor = windowBg;
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

        if (!shivCheckbox.Checked && s.IsShiv) continue;
        if (!woundedCheckbox.Checked && s.IsWounded && s.HoursOut > 8) continue;
        if (!blueCheckbox.Checked && s.IsBlueshirt) continue;
        if (!deadCheckbox.Checked && s.IsDead) continue;
        if (!fatiguedCheckbox.Checked && s.IsFatigued && s.HoursOut > 8) continue;

        // this is dumb, redo this
        if (perkFilter.Any(x => !string.IsNullOrWhiteSpace(x)))
        {
          filterPass = new bool?[perkFilter.Length];

          for (int i = 0; i < 3; i++)
          {
            if (string.IsNullOrWhiteSpace(perkFilter[i])) continue;

            if (i == 0 && perkComboBox4.SelectedIndex == -1) filterPass[i] = true;
            else if (i == 1 && perkComboBox5.SelectedIndex == -1) filterPass[i] = true;
            else if (i == 2 && perkComboBox6.SelectedIndex == -1) filterPass[i] = true;

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

        if (filterPass.Any(x => x == false)) continue;

        filteredSoldiersCount++;
        filteredSoldiers.Add(s);
      }

      int filteredSoldierIndex = 0;

      maxHp = hiHp = loHp = minHp = false;
      maxMob = hiMob = loMob = minMob = false;
      maxAim = hiAim = loAim = minAim = false;
      maxWill = hiWill = loWill = minWill = false;
      maxDef = hiDef = loDef = minDef = false;

      foreach (Soldier f in filteredSoldiers)
      {
        if (filteredSoldiers.Count >= minMinMaxListSize)
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

          if (filteredSoldiers.Count >= minHiLoListSize)
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
          f.IsFatigued || f.IsWounded ? ((f.HoursOut / 24) > 0 ? $"{f.HoursOut / 24}d " : "") + $"{f.HoursOut % 24}h" : f.Status,
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

        if (f.LName == "TamTam") rosterGridView.Rows[filteredSoldierIndex].Cells["LName"].Value = $"TamTam ♥";
        if (f.LName == "ParkaBuoy") rosterGridView.Rows[filteredSoldierIndex].Cells["LName"].Value = $"Parkaboy {new string(' ', DateTime.Now.Second % 6)}{PokemonPicker()}";
        if (!f.IsWounded && !f.IsBlueshirt && !f.IsDead && DateTime.Now.Microsecond % 100 == 0 && DateTime.Now.Millisecond % 20 == 0)
        {
          rosterGridView.Rows[filteredSoldierIndex].DefaultCellStyle = new()
          {
            ForeColor = Color.FromArgb(180, 0, 0, 0),
            BackColor = Color.FromArgb(255, 255, 215, 0),
            SelectionBackColor = Color.FromArgb(255, 255, 215, 0),
            Font = new(Font, FontStyle.Underline)
          };
          rosterGridView.Rows[filteredSoldierIndex].Cells["LName"].Value = $"*~･ﾟ✧~  {rosterGridView.Rows[filteredSoldierIndex].Cells["LName"].Value}  ~✧･ﾟ~*";
        }

        if (f.HoursOut <= 8)
        {
          if (f.IsWounded) rosterGridView.Rows[filteredSoldierIndex].Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg, Font = new(Font, FontStyle.Bold) };
          else if (f.IsFatigued) rosterGridView.Rows[filteredSoldierIndex].Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg, Font = new(Font, FontStyle.Bold) };
        }
        else
        {
          if (f.IsWounded) rosterGridView.Rows[filteredSoldierIndex].Cells["Status"].Style = new() { BackColor = woundBg, SelectionBackColor = woundBg };
          else if (f.IsFatigued) rosterGridView.Rows[filteredSoldierIndex].Cells["Status"].Style = new() { BackColor = fatigueBg, SelectionBackColor = fatigueBg };
        }

        filteredSoldierIndex++;
      }

      totalSoldierLabel.Text = $"Total: {ConsoleApp.Roster.Count}";
      shownSoldierLabel.Text = $"Match: {filteredSoldiers.Count}";
    }

    private static string PokemonPicker()
    {
      switch (DateTime.Now.Millisecond % 30)
      {
        case 1: return "ฅ(^•ﻌ•^ฅ)";
        case 2: return "ʕ •ᴥ•ʔ";
        case 3: return "ヽ༼ຈل͜ຈ༽ﾉ";
        case 4: return "(´･ω･`)";
        case 5: return "♪┏(・o･)┛♪";
        case 6: return ">:3c";
        case 7: return "(ﾉ◕ヮ◕)ﾉ*:･ ﾟ✧";
        case 8: return "=＾● ⋏ ●＾=";
        case 9: return "༼◥▶ل͜◀◤༽";
        case 10: return "ლↂ‿‿ↂლ";
        case 11: return "⊙︿⊙";
        case 12: return "（＞д＜）ง ▬ι═══ﺤ";
        case 13: return "↜(╰ •ω•)╯";
        default: return "";
      }
    }

    // this sets the title bar colors
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private void Rosterizer_Load(object sender, EventArgs e)
    {
      int formHeaderBackgroundColorValue = (windowTitleBg.B << 16) | (windowTitleBg.G << 8) | windowTitleBg.R;
      int formHeaderTextColorValue = (windowTitleFg.B << 16) | (windowTitleFg.G << 8) | windowTitleFg.R;

      DwmSetWindowAttribute(this.Handle, 35, ref formHeaderBackgroundColorValue, Marshal.SizeOf(formHeaderBackgroundColorValue));
      DwmSetWindowAttribute(this.Handle, 36, ref formHeaderTextColorValue, Marshal.SizeOf(formHeaderTextColorValue));
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
    private void fatiguedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private string[] GetFilters()
    {
      return [(deadCheckbox.Checked ? "" : "dead"), (woundedCheckbox.Checked ? "" : "wounded"), (blueCheckbox.Checked ? "" : "blue"), (shivCheckbox.Checked ? "" : "shiv"), (fatiguedCheckbox.Checked ? "" : "fatigued")];
    }

    private string[] GetPerkFilters()
    {
      perkComboBox5.Visible = perkComboBox4.SelectedIndex != -1;
      perkComboBox6.Visible = perkComboBox5.Visible && perkComboBox5.SelectedIndex != -1;

      return [(string)(perkComboBox4.SelectedItem ?? ""), (string)(perkComboBox5.SelectedItem ?? ""), (string)(perkComboBox6.SelectedItem ?? "")];
    }

    private void perkComboBox4_SelectedIndexChanged(object sender, EventArgs e)
    {
      perkComboBox5.SelectedIndex = -1;
      perkComboBox5.Items.Clear();
      perkComboBox5.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x).Where(x => x != (perkComboBox4.SelectedItem ?? "").ToString())]);

      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox5_SelectedIndexChanged(object sender, EventArgs e)
    {
      perkComboBox6.SelectedIndex = -1;
      perkComboBox6.Items.Clear();
      perkComboBox6.Items.AddRange([.. ConsoleApp.PerkNames.Where(x => x != (perkComboBox5.SelectedItem ?? "").ToString() && x != (perkComboBox4.SelectedItem ?? "").ToString())]);

      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox6_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox4_DrawItem(object sender, DrawItemEventArgs e)
    {
      using (Pen p = new(SystemColors.ControlDark, 1))
      {
        //e.Graphics.DrawRectangle(p, e.Bounds);
        e.DrawBackground();
        if (e.Index > -1) TextRenderer.DrawText(e.Graphics, (string?)((ComboBox)sender).Items[e.Index], e.Font, e.Bounds, e.ForeColor, e.BackColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
      }
    }
    private void perkComboBox5_DrawItem(object sender, DrawItemEventArgs e)
    {
      using (Pen p = new(SystemColors.ControlDark, 1))
      {
        //e.Graphics.DrawRectangle(p, e.Bounds);
        e.DrawBackground();
        if (e.Index > -1) TextRenderer.DrawText(e.Graphics, (string?)((ComboBox)sender).Items[e.Index], e.Font, e.Bounds, e.ForeColor, e.BackColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
      }
    }
    private void perkComboBox6_DrawItem(object sender, DrawItemEventArgs e)
    {
      using (Pen p = new(SystemColors.ControlDark, 1))
      {
        //e.Graphics.DrawRectangle(p, e.Bounds);
        e.DrawBackground();
        if (e.Index > -1) TextRenderer.DrawText(e.Graphics, (string?)((ComboBox)sender).Items[e.Index], e.Font, e.Bounds, e.ForeColor, e.BackColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
      }
    }
    private void rosterGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {

      if (e.RowIndex == -1)
      {
        e.PaintBackground(e.CellBounds, false);
        TextRenderer.DrawText(e.Graphics, string.Format("{0}", e.FormattedValue), (e.CellStyle ?? new()).Font, e.CellBounds, e.CellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        e.Handled = true;
      }
    }

    private void perkComboBox4_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        perkComboBox4.SelectedIndex = -1;
        perkComboBox4.Items.Clear();
        perkComboBox4.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x)]);

        ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }
    private void perkComboBox5_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        perkComboBox5.SelectedIndex = -1;
        perkComboBox5.Items.Clear();
        perkComboBox5.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x).Where(x => x != (perkComboBox4.SelectedItem ?? "").ToString())]);

        ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }
    private void perkComboBox6_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        perkComboBox6.SelectedIndex = -1;
        perkComboBox6.Items.Clear();
        perkComboBox6.Items.AddRange([.. ConsoleApp.PerkNames.OrderBy(x => x).Where(x => x != (perkComboBox4.SelectedItem ?? "").ToString() && x != (perkComboBox5.SelectedItem ?? "").ToString())]);

        ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

    private void Rosterizer_DragDrop(object sender, DragEventArgs e)
    {
      try
      {
        if ((e.Data ?? throw new("No file data")).GetDataPresent(DataFormats.FileDrop))
        {
          if (e.Data.GetDataPresent(DataFormats.FileDrop))
          {
            foreach (string file in (e.Data.GetData(DataFormats.FileDrop) as string[] ?? []))
            {
              string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
              perkComboBox4.SelectedIndex = -1;
              ConsoleApp.Reinitialize(new((files ?? [])[0]));
              ListRoster(boolFilters: GetFilters(), perkFilter: GetPerkFilters());
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (MessageBox.Show(ex.Message) == DialogResult.OK) Application.Exit();

      }
    }

    private void Rosterizer_DragEnter(object sender, DragEventArgs e)
    {
      try
      {
        if ((e.Data ?? throw new("No file data")).GetDataPresent(DataFormats.FileDrop))
        {
          foreach (string file in (e.Data.GetData(DataFormats.FileDrop) as string[] ?? []))
          {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
              e.Effect = DragDropEffects.Copy;
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (MessageBox.Show(ex.Message) == DialogResult.OK) Application.Exit();

      }
    }
  }
}
