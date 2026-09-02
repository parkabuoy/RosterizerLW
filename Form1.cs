using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Forms.VisualStyles;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Documents;


namespace Rosterizer
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      fileInfoStatusLabel.Text = $"{(Program.SaveParsed.Header.Save_description ?? new()).Str} - File: {Program.SaveFile.Name}";
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
      perkComboBox1.Items.AddRange([.. Program.PerkNames]);
      perkComboBox2.Items.AddRange([.. Program.PerkNames]);
      perkComboBox3.Items.AddRange([.. Program.PerkNames]);
    }

    public void ListRoster(string[]? filters = null, Dictionary<string, int>? statFilter = null, string[]? perkFilter = null, string order = "default")
    {
      rosterListView.Items.Clear();

      foreach (Soldier s in Program.Roster.OrderBy(x => x.Xp).ThenBy(x => x.Stats.Mobility).ThenBy(x => x.Stats.Aim))
      {
        bool isWounded = s.Status.ToLower() == "healing";
        bool isBlueshirt = s.Rank <= 2;
        bool isDead = s.Status.ToLower() == "dead";
        bool?[] filterPass = [];

        if (filters is not null && filters.Length > 0)
        {
          if (filters.Any(x => x == "wounded") && isWounded) continue;
          if (filters.Any(x => x == "blue") && isBlueshirt) continue;
          if (filters.Any(x => x == "dead") && isDead) continue;
        }

        if (statFilter is not null && statFilter.Count > 0)
        {
          int stat = 0;
          if (statFilter.TryGetValue("hp", out stat) && s.Stats.HP < stat) continue;
          if (statFilter.TryGetValue("aim", out stat) && s.Stats.Aim < stat) continue;
          if (statFilter.TryGetValue("mobility", out stat) && s.Stats.Mobility < stat) continue;
          if (statFilter.TryGetValue("will", out stat) && s.Stats.Will < stat) continue;
          if (statFilter.TryGetValue("defense", out stat) && s.Stats.Defense < stat) continue;
        }

        if (perkFilter.Any(x => !string.IsNullOrWhiteSpace(x)))
        {
          filterPass = new bool?[perkFilter.Length];
          for (int i = 0; i < perkFilter.Length; i++)
          {
            switch (i)
            {
              case 0: 
                if (perkComboBox1.SelectedIndex == -1) continue;
                break;
              case 1:
                if (perkComboBox2.SelectedIndex == -1) continue;
                break;
              case 2:
                if (perkComboBox3.SelectedIndex == -1) continue;
                break;
            }

            if (string.IsNullOrWhiteSpace(perkFilter[i])) continue;
            else
            {
              foreach (string sPerkName in s.Perks.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x)))
              {
                if (sPerkName == perkFilter[i]) 
                { 
                  filterPass[i] = true;
                  break; 
                }
                else filterPass[i] = false;
              }
            }
          }
        }

        if (filterPass.Length > 0 && filterPass.Any(x => x == false)) continue;

        Color bg = Color.White;
        Color fg = Color.Black;
        Font fnt = new("Tahoma", 8);

        if (isBlueshirt) { bg = Color.LightBlue; fg = Color.Blue; }
        ;
        if (isDead) { bg = Color.LightCoral; fg = Color.Red; fnt = new(fnt, FontStyle.Italic); }
        ;
        if (isWounded) { bg = Color.LightYellow; fg = Color.DarkOrange; }
        ;

        ListViewItem lvi = new();
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.LName, BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.NName, BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Status, BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Rank.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Xp.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Stats.HP.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Stats.Aim.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Stats.Mobility.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Stats.Will.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = s.Stats.Defense.ToString(), BackColor = bg, ForeColor = fg, Font = fnt });
        rosterListView.Items.Add(lvi);
      }

    }


    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void deadCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void woundedCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void blueCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private string[] GetFilters()
    {
      return [(deadCheckbox.Checked ? "" : "dead"), (woundedCheckbox.Checked ? "" : "wounded"), (blueCheckbox.Checked ? "" : "blue")];
    }

    private string[] GetPerkFilters()
    {
      perkComboBox2.Visible = perkComboBox1.SelectedIndex != -1;
      perkComboBox3.Visible = perkComboBox2.Visible && perkComboBox2.SelectedIndex != -1;

      return [(string)(perkComboBox1.SelectedItem ?? ""), (string)(perkComboBox2.SelectedItem ?? ""), (string)(perkComboBox3.SelectedItem ?? "")];
    }

    private void toolStripStatusLabel1_Click(object sender, EventArgs e)
    {

    }

    private void perkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox3_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
    }

    private void perkComboBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        perkComboBox1.SelectedIndex = -1;
        ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

    private void perkComboBox2_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        perkComboBox2.SelectedIndex = -1;
        ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

    private void perkComboBox3_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        perkComboBox3.SelectedIndex = -1;
        ListRoster(filters: GetFilters(), perkFilter: GetPerkFilters());
      }
    }

  }
}
