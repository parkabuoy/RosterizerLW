namespace Rosterizer
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      soldierBindingSource1 = new BindingSource(components);
      tableLayoutPanel1 = new TableLayoutPanel();
      tableLayoutPanel3 = new TableLayoutPanel();
      perkComboBox3 = new MaterialSkin.Controls.MaterialComboBox();
      perkComboBox2 = new MaterialSkin.Controls.MaterialComboBox();
      blueCheckbox = new CheckBox();
      woundedCheckbox = new CheckBox();
      deadCheckbox = new CheckBox();
      perkComboBox1 = new MaterialSkin.Controls.MaterialComboBox();
      tableLayoutPanel2 = new TableLayoutPanel();
      rosterListView = new MaterialSkin.Controls.MaterialListView();
      Id = new ColumnHeader();
      LName = new ColumnHeader();
      NName = new ColumnHeader();
      Status = new ColumnHeader();
      Rank = new ColumnHeader();
      XP = new ColumnHeader();
      HP = new ColumnHeader();
      Aim = new ColumnHeader();
      Mob = new ColumnHeader();
      Will = new ColumnHeader();
      Def = new ColumnHeader();
      statusStrip1 = new StatusStrip();
      fileInfoStatusLabel = new ToolStripStatusLabel();
      ((System.ComponentModel.ISupportInitialize)soldierBindingSource1).BeginInit();
      tableLayoutPanel1.SuspendLayout();
      tableLayoutPanel3.SuspendLayout();
      tableLayoutPanel2.SuspendLayout();
      statusStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // soldierBindingSource1
      // 
      soldierBindingSource1.DataSource = typeof(Soldier);
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.ColumnCount = 2;
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 218F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
      tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
      tableLayoutPanel1.Dock = DockStyle.Fill;
      tableLayoutPanel1.Location = new Point(0, 0);
      tableLayoutPanel1.Margin = new Padding(0);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.RowCount = 1;
      tableLayoutPanel1.RowStyles.Add(new RowStyle());
      tableLayoutPanel1.Size = new Size(1264, 681);
      tableLayoutPanel1.TabIndex = 2;
      // 
      // tableLayoutPanel3
      // 
      tableLayoutPanel3.ColumnCount = 2;
      tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
      tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      tableLayoutPanel3.Controls.Add(perkComboBox3, 0, 10);
      tableLayoutPanel3.Controls.Add(perkComboBox2, 0, 8);
      tableLayoutPanel3.Controls.Add(blueCheckbox, 0, 13);
      tableLayoutPanel3.Controls.Add(woundedCheckbox, 0, 12);
      tableLayoutPanel3.Controls.Add(deadCheckbox, 0, 14);
      tableLayoutPanel3.Controls.Add(perkComboBox1, 0, 6);
      tableLayoutPanel3.Dock = DockStyle.Fill;
      tableLayoutPanel3.Location = new Point(0, 0);
      tableLayoutPanel3.Margin = new Padding(0);
      tableLayoutPanel3.Name = "tableLayoutPanel3";
      tableLayoutPanel3.RowCount = 16;
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 380F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.Size = new Size(218, 681);
      tableLayoutPanel3.TabIndex = 4;
      // 
      // perkComboBox3
      // 
      perkComboBox3.AutoResize = false;
      perkComboBox3.BackColor = Color.FromArgb(255, 255, 255);
      tableLayoutPanel3.SetColumnSpan(perkComboBox3, 2);
      perkComboBox3.Depth = 0;
      perkComboBox3.Dock = DockStyle.Fill;
      perkComboBox3.DrawMode = DrawMode.OwnerDrawVariable;
      perkComboBox3.DropDownHeight = 118;
      perkComboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
      perkComboBox3.DropDownWidth = 121;
      perkComboBox3.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      perkComboBox3.ForeColor = Color.FromArgb(222, 0, 0, 0);
      perkComboBox3.FormattingEnabled = true;
      perkComboBox3.IntegralHeight = false;
      perkComboBox3.ItemHeight = 29;
      perkComboBox3.Location = new Point(0, 200);
      perkComboBox3.Margin = new Padding(0);
      perkComboBox3.MaxDropDownItems = 4;
      perkComboBox3.MouseState = MaterialSkin.MouseState.OUT;
      perkComboBox3.Name = "perkComboBox3";
      perkComboBox3.Size = new Size(218, 35);
      perkComboBox3.StartIndex = 0;
      perkComboBox3.TabIndex = 8;
      perkComboBox3.UseTallSize = false;
      perkComboBox3.Visible = false;
      perkComboBox3.SelectedIndexChanged += perkComboBox3_SelectedIndexChanged;
      perkComboBox3.KeyDown += perkComboBox3_KeyDown;
      // 
      // perkComboBox2
      // 
      perkComboBox2.AutoResize = false;
      perkComboBox2.BackColor = Color.FromArgb(255, 255, 255);
      tableLayoutPanel3.SetColumnSpan(perkComboBox2, 2);
      perkComboBox2.Depth = 0;
      perkComboBox2.Dock = DockStyle.Fill;
      perkComboBox2.DrawMode = DrawMode.OwnerDrawVariable;
      perkComboBox2.DropDownHeight = 118;
      perkComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
      perkComboBox2.DropDownWidth = 121;
      perkComboBox2.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      perkComboBox2.ForeColor = Color.FromArgb(222, 0, 0, 0);
      perkComboBox2.FormattingEnabled = true;
      perkComboBox2.IntegralHeight = false;
      perkComboBox2.ItemHeight = 29;
      perkComboBox2.Location = new Point(0, 160);
      perkComboBox2.Margin = new Padding(0);
      perkComboBox2.MaxDropDownItems = 4;
      perkComboBox2.MouseState = MaterialSkin.MouseState.OUT;
      perkComboBox2.Name = "perkComboBox2";
      perkComboBox2.Size = new Size(218, 35);
      perkComboBox2.StartIndex = 0;
      perkComboBox2.TabIndex = 7;
      perkComboBox2.UseTallSize = false;
      perkComboBox2.Visible = false;
      perkComboBox2.SelectedIndexChanged += perkComboBox2_SelectedIndexChanged;
      perkComboBox2.KeyDown += perkComboBox2_KeyDown;
      // 
      // blueCheckbox
      // 
      blueCheckbox.AutoSize = true;
      tableLayoutPanel3.SetColumnSpan(blueCheckbox, 2);
      blueCheckbox.Dock = DockStyle.Fill;
      blueCheckbox.Location = new Point(0, 620);
      blueCheckbox.Margin = new Padding(0);
      blueCheckbox.Name = "blueCheckbox";
      blueCheckbox.Padding = new Padding(10, 0, 0, 0);
      blueCheckbox.Size = new Size(218, 20);
      blueCheckbox.TabIndex = 3;
      blueCheckbox.Text = "Blueshirt";
      blueCheckbox.UseVisualStyleBackColor = true;
      blueCheckbox.CheckedChanged += blueCheckbox_CheckedChanged;
      // 
      // woundedCheckbox
      // 
      woundedCheckbox.AutoSize = true;
      woundedCheckbox.Checked = true;
      woundedCheckbox.CheckState = CheckState.Checked;
      tableLayoutPanel3.SetColumnSpan(woundedCheckbox, 2);
      woundedCheckbox.Dock = DockStyle.Fill;
      woundedCheckbox.Location = new Point(0, 600);
      woundedCheckbox.Margin = new Padding(0);
      woundedCheckbox.Name = "woundedCheckbox";
      woundedCheckbox.Padding = new Padding(10, 0, 0, 0);
      woundedCheckbox.Size = new Size(218, 20);
      woundedCheckbox.TabIndex = 4;
      woundedCheckbox.Text = "Wounded";
      woundedCheckbox.UseVisualStyleBackColor = true;
      woundedCheckbox.CheckedChanged += woundedCheckbox_CheckedChanged;
      // 
      // deadCheckbox
      // 
      deadCheckbox.AutoSize = true;
      tableLayoutPanel3.SetColumnSpan(deadCheckbox, 2);
      deadCheckbox.Dock = DockStyle.Fill;
      deadCheckbox.Location = new Point(0, 640);
      deadCheckbox.Margin = new Padding(0);
      deadCheckbox.Name = "deadCheckbox";
      deadCheckbox.Padding = new Padding(10, 0, 0, 0);
      deadCheckbox.Size = new Size(218, 20);
      deadCheckbox.TabIndex = 5;
      deadCheckbox.Text = "Dead";
      deadCheckbox.UseVisualStyleBackColor = true;
      deadCheckbox.CheckedChanged += deadCheckbox_CheckedChanged;
      // 
      // perkComboBox1
      // 
      perkComboBox1.AutoResize = false;
      perkComboBox1.BackColor = Color.White;
      tableLayoutPanel3.SetColumnSpan(perkComboBox1, 2);
      perkComboBox1.Depth = 0;
      perkComboBox1.Dock = DockStyle.Fill;
      perkComboBox1.DrawMode = DrawMode.OwnerDrawVariable;
      perkComboBox1.DropDownHeight = 118;
      perkComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
      perkComboBox1.DropDownWidth = 121;
      perkComboBox1.FlatStyle = FlatStyle.Flat;
      perkComboBox1.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      perkComboBox1.ForeColor = Color.FromArgb(222, 0, 0, 0);
      perkComboBox1.IntegralHeight = false;
      perkComboBox1.ItemHeight = 29;
      perkComboBox1.Location = new Point(0, 120);
      perkComboBox1.Margin = new Padding(0);
      perkComboBox1.MaxDropDownItems = 4;
      perkComboBox1.MouseState = MaterialSkin.MouseState.OUT;
      perkComboBox1.Name = "perkComboBox1";
      perkComboBox1.Size = new Size(218, 35);
      perkComboBox1.StartIndex = 0;
      perkComboBox1.TabIndex = 6;
      perkComboBox1.UseTallSize = false;
      perkComboBox1.SelectedIndexChanged += perkComboBox1_SelectedIndexChanged;
      perkComboBox1.KeyDown += perkComboBox1_KeyDown;
      // 
      // tableLayoutPanel2
      // 
      tableLayoutPanel2.ColumnCount = 1;
      tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
      tableLayoutPanel2.Controls.Add(rosterListView, 0, 0);
      tableLayoutPanel2.Dock = DockStyle.Fill;
      tableLayoutPanel2.Location = new Point(221, 3);
      tableLayoutPanel2.Name = "tableLayoutPanel2";
      tableLayoutPanel2.RowCount = 1;
      tableLayoutPanel2.RowStyles.Add(new RowStyle());
      tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel2.Size = new Size(1040, 675);
      tableLayoutPanel2.TabIndex = 3;
      // 
      // Id
      // 
      Id.Text = "Id";
      Id.Width = 0;
      // 
      // LName
      // 
      LName.Text = "Name";
      LName.Width = 170;
      // 
      // NName
      // 
      NName.Text = "Nickname";
      NName.Width = 170;
      // 
      // Status
      // 
      Status.Text = "Status";
      Status.TextAlign = HorizontalAlignment.Center;
      Status.Width = 100;
      // 
      // Rank
      // 
      Rank.Text = "Rank";
      Rank.TextAlign = HorizontalAlignment.Center;
      Rank.Width = 80;
      // 
      // XP
      // 
      XP.Text = "XP";
      XP.TextAlign = HorizontalAlignment.Right;
      XP.Width = 100;
      // 
      // HP
      // 
      HP.Text = "HP";
      HP.TextAlign = HorizontalAlignment.Right;
      HP.Width = 80;
      // 
      // Aim
      // 
      Aim.Text = "Aim";
      Aim.TextAlign = HorizontalAlignment.Right;
      Aim.Width = 80;
      // 
      // Mob
      // 
      Mob.Text = "Mob";
      Mob.TextAlign = HorizontalAlignment.Right;
      Mob.Width = 80;
      // 
      // Will
      // 
      Will.Text = "Will";
      Will.TextAlign = HorizontalAlignment.Right;
      Will.Width = 80;
      // 
      // Def
      // 
      Def.Text = "Def";
      Def.TextAlign = HorizontalAlignment.Right;
      Def.Width = 80;
      // 
      // rosterListView
      // 
      rosterListView.Activation = ItemActivation.OneClick;
      rosterListView.Alignment = ListViewAlignment.Default;
      rosterListView.AutoSizeTable = false;
      rosterListView.BackColor = Color.FromArgb(255, 255, 255);
      rosterListView.BorderStyle = BorderStyle.None;
      rosterListView.Columns.AddRange(new ColumnHeader[] { Id, LName, NName, Status, Rank, XP, HP, Aim, Mob, Will, Def });
      rosterListView.Depth = 0;
      rosterListView.Dock = DockStyle.Fill;
      rosterListView.Font = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
      rosterListView.FullRowSelect = true;
      rosterListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      rosterListView.ImeMode = ImeMode.NoControl;
      rosterListView.LabelWrap = false;
      rosterListView.Location = new Point(0, 0);
      rosterListView.Margin = new Padding(0);
      rosterListView.MinimumSize = new Size(200, 100);
      rosterListView.MouseLocation = new Point(-1, -1);
      rosterListView.MouseState = MaterialSkin.MouseState.OUT;
      rosterListView.MultiSelect = false;
      rosterListView.Name = "rosterListView";
      rosterListView.OwnerDraw = true;
      rosterListView.RightToLeft = RightToLeft.No;
      rosterListView.Size = new Size(1040, 675);
      rosterListView.Sorting = SortOrder.Ascending;
      rosterListView.TabIndex = 3;
      rosterListView.UseCompatibleStateImageBehavior = false;
      rosterListView.View = View.Details;
      // 
      // statusStrip1
      // 
      statusStrip1.Items.AddRange(new ToolStripItem[] { fileInfoStatusLabel });
      statusStrip1.Location = new Point(0, 659);
      statusStrip1.Name = "statusStrip1";
      statusStrip1.Size = new Size(1264, 22);
      statusStrip1.TabIndex = 3;
      statusStrip1.Text = "statusStrip1";
      // 
      // fileInfoStatusLabel
      // 
      fileInfoStatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
      fileInfoStatusLabel.Name = "fileInfoStatusLabel";
      fileInfoStatusLabel.Size = new Size(1249, 17);
      fileInfoStatusLabel.Spring = true;
      fileInfoStatusLabel.Text = "fileInfo";
      fileInfoStatusLabel.TextAlign = ContentAlignment.MiddleRight;
      fileInfoStatusLabel.TextDirection = ToolStripTextDirection.Horizontal;
      fileInfoStatusLabel.Click += toolStripStatusLabel1_Click;
      // 
      // Form1
      // 
      AllowDrop = true;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BackgroundImageLayout = ImageLayout.None;
      ClientSize = new Size(1264, 681);
      Controls.Add(statusStrip1);
      Controls.Add(tableLayoutPanel1);
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      MaximumSize = new Size(1280, 720);
      MinimumSize = new Size(1280, 720);
      Name = "Form1";
      StartPosition = FormStartPosition.CenterScreen;
      ((System.ComponentModel.ISupportInitialize)soldierBindingSource1).EndInit();
      tableLayoutPanel1.ResumeLayout(false);
      tableLayoutPanel3.ResumeLayout(false);
      tableLayoutPanel3.PerformLayout();
      tableLayoutPanel2.ResumeLayout(false);
      statusStrip1.ResumeLayout(false);
      statusStrip1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
    private BindingSource soldierBindingSource1;
    private DataGridViewTextBoxColumn lNameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn nNameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn rankDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn xpDataGridViewTextBoxColumn;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;
    private MaterialSkin.Controls.MaterialListView rosterListView;
    private ColumnHeader LName;
    private ColumnHeader NName;
    private ColumnHeader Status;
    private ColumnHeader Rank;
    private ColumnHeader XP;
    private ColumnHeader HP;
    private ColumnHeader Aim;
    private ColumnHeader Mob;
    private ColumnHeader Will;
    private ColumnHeader Def;
    private TableLayoutPanel tableLayoutPanel3;
    private StatusStrip statusStrip1;
    private CheckBox blueCheckbox;
    private CheckBox woundedCheckbox;
    private CheckBox deadCheckbox;
    private ColumnHeader Id;
    private ToolStripStatusLabel fileInfoStatusLabel;
    private MaterialSkin.Controls.MaterialComboBox perkComboBox1;
    private MaterialSkin.Controls.MaterialComboBox perkComboBox2;
    private MaterialSkin.Controls.MaterialComboBox perkComboBox3;
  }
}
