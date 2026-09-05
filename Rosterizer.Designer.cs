namespace Rosterizer
{
  partial class Rosterizer
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
      DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
      DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
      tableLayoutPanel1 = new TableLayoutPanel();
      rosterGridView = new DataGridView();
      LName = new DataGridViewTextBoxColumn();
      NName = new DataGridViewTextBoxColumn();
      Status = new DataGridViewTextBoxColumn();
      SoldierClass = new DataGridViewTextBoxColumn();
      Def = new DataGridViewTextBoxColumn();
      HP = new DataGridViewTextBoxColumn();
      Mob = new DataGridViewTextBoxColumn();
      Will = new DataGridViewTextBoxColumn();
      Aim = new DataGridViewTextBoxColumn();
      XP = new DataGridViewTextBoxColumn();
      Rank = new DataGridViewTextBoxColumn();
      tableLayoutPanel3 = new TableLayoutPanel();
      shivCheckbox = new CheckBox();
      shownSoldierLabel = new MaterialSkin.Controls.MaterialLabel();
      perkComboBox3 = new MaterialSkin.Controls.MaterialComboBox();
      perkComboBox2 = new MaterialSkin.Controls.MaterialComboBox();
      blueCheckbox = new CheckBox();
      woundedCheckbox = new CheckBox();
      deadCheckbox = new CheckBox();
      perkComboBox1 = new MaterialSkin.Controls.MaterialComboBox();
      totalSoldierLabel = new MaterialSkin.Controls.MaterialLabel();
      tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)rosterGridView).BeginInit();
      tableLayoutPanel3.SuspendLayout();
      SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.ColumnCount = 2;
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 328F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
      tableLayoutPanel1.Controls.Add(rosterGridView, 1, 0);
      tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
      tableLayoutPanel1.Dock = DockStyle.Fill;
      tableLayoutPanel1.Location = new Point(0, 0);
      tableLayoutPanel1.Margin = new Padding(0);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.RowCount = 1;
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
      tableLayoutPanel1.Size = new Size(1264, 681);
      tableLayoutPanel1.TabIndex = 2;
      // 
      // rosterGridView
      // 
      rosterGridView.AllowUserToAddRows = false;
      rosterGridView.AllowUserToDeleteRows = false;
      rosterGridView.AllowUserToResizeColumns = false;
      rosterGridView.AllowUserToResizeRows = false;
      rosterGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      rosterGridView.BackgroundColor = SystemColors.ControlDarkDark;
      rosterGridView.BorderStyle = BorderStyle.Fixed3D;
      rosterGridView.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
      rosterGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      rosterGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = Color.FromArgb(144, 144, 176);
      dataGridViewCellStyle1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
      dataGridViewCellStyle1.ForeColor = SystemColors.ControlLight;
      dataGridViewCellStyle1.SelectionBackColor = SystemColors.ControlDark;
      dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
      dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
      rosterGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      rosterGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      rosterGridView.Columns.AddRange(new DataGridViewColumn[] { LName, NName, Status, SoldierClass, Def, HP, Mob, Will, Aim, XP, Rank });
      dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle13.BackColor = SystemColors.ControlLight;
      dataGridViewCellStyle13.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
      dataGridViewCellStyle13.ForeColor = SystemColors.ControlText;
      dataGridViewCellStyle13.Padding = new Padding(4, 0, 4, 0);
      dataGridViewCellStyle13.SelectionBackColor = SystemColors.ControlLight;
      dataGridViewCellStyle13.SelectionForeColor = SystemColors.ControlText;
      dataGridViewCellStyle13.WrapMode = DataGridViewTriState.False;
      rosterGridView.DefaultCellStyle = dataGridViewCellStyle13;
      rosterGridView.Dock = DockStyle.Fill;
      rosterGridView.EnableHeadersVisualStyles = false;
      rosterGridView.GridColor = SystemColors.GrayText;
      rosterGridView.ImeMode = ImeMode.NoControl;
      rosterGridView.Location = new Point(328, 0);
      rosterGridView.Margin = new Padding(0);
      rosterGridView.Name = "rosterGridView";
      rosterGridView.ReadOnly = true;
      rosterGridView.RowHeadersVisible = false;
      rosterGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      rosterGridView.RowTemplate.ReadOnly = true;
      rosterGridView.ScrollBars = ScrollBars.Vertical;
      rosterGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
      rosterGridView.ShowEditingIcon = false;
      rosterGridView.ShowRowErrors = false;
      rosterGridView.Size = new Size(936, 681);
      rosterGridView.TabIndex = 5;
      rosterGridView.CellPainting += rosterGridView_CellPainting;
      // 
      // LName
      // 
      LName.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      LName.DefaultCellStyle = dataGridViewCellStyle2;
      LName.Frozen = true;
      LName.HeaderText = "Name";
      LName.Name = "LName";
      LName.ReadOnly = true;
      LName.Resizable = DataGridViewTriState.False;
      LName.Width = 220;
      // 
      // NName
      // 
      NName.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      NName.DefaultCellStyle = dataGridViewCellStyle3;
      NName.Frozen = true;
      NName.HeaderText = "Nickname";
      NName.Name = "NName";
      NName.ReadOnly = true;
      NName.Resizable = DataGridViewTriState.False;
      NName.Width = 220;
      // 
      // Status
      // 
      Status.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
      Status.DefaultCellStyle = dataGridViewCellStyle4;
      Status.Frozen = true;
      Status.HeaderText = "Status";
      Status.MinimumWidth = 85;
      Status.Name = "Status";
      Status.ReadOnly = true;
      Status.Resizable = DataGridViewTriState.False;
      Status.Width = 85;
      // 
      // SoldierClass
      // 
      SoldierClass.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
      SoldierClass.DefaultCellStyle = dataGridViewCellStyle5;
      SoldierClass.HeaderText = "Class";
      SoldierClass.Name = "SoldierClass";
      SoldierClass.ReadOnly = true;
      SoldierClass.Width = 80;
      // 
      // Def
      // 
      Def.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      Def.DefaultCellStyle = dataGridViewCellStyle6;
      Def.HeaderText = "Def";
      Def.MinimumWidth = 40;
      Def.Name = "Def";
      Def.ReadOnly = true;
      Def.Resizable = DataGridViewTriState.False;
      Def.Width = 40;
      // 
      // HP
      // 
      HP.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      HP.DefaultCellStyle = dataGridViewCellStyle7;
      HP.HeaderText = "HP";
      HP.MinimumWidth = 40;
      HP.Name = "HP";
      HP.ReadOnly = true;
      HP.Resizable = DataGridViewTriState.False;
      HP.Width = 40;
      // 
      // Mob
      // 
      Mob.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
      Mob.DefaultCellStyle = dataGridViewCellStyle8;
      Mob.HeaderText = "Mob";
      Mob.MinimumWidth = 40;
      Mob.Name = "Mob";
      Mob.ReadOnly = true;
      Mob.Resizable = DataGridViewTriState.False;
      Mob.Width = 40;
      // 
      // Will
      // 
      Will.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
      Will.DefaultCellStyle = dataGridViewCellStyle9;
      Will.HeaderText = "Will";
      Will.MinimumWidth = 40;
      Will.Name = "Will";
      Will.ReadOnly = true;
      Will.Resizable = DataGridViewTriState.False;
      Will.Width = 40;
      // 
      // Aim
      // 
      Aim.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
      Aim.DefaultCellStyle = dataGridViewCellStyle10;
      Aim.HeaderText = "Aim";
      Aim.MinimumWidth = 40;
      Aim.Name = "Aim";
      Aim.ReadOnly = true;
      Aim.Resizable = DataGridViewTriState.False;
      Aim.Width = 40;
      // 
      // XP
      // 
      XP.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
      XP.DefaultCellStyle = dataGridViewCellStyle11;
      XP.HeaderText = "EXP";
      XP.MinimumWidth = 75;
      XP.Name = "XP";
      XP.ReadOnly = true;
      XP.Resizable = DataGridViewTriState.False;
      XP.Width = 75;
      // 
      // Rank
      // 
      Rank.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter;
      Rank.DefaultCellStyle = dataGridViewCellStyle12;
      Rank.HeaderText = "Rank";
      Rank.Name = "Rank";
      Rank.ReadOnly = true;
      Rank.Resizable = DataGridViewTriState.False;
      // 
      // tableLayoutPanel3
      // 
      tableLayoutPanel3.BackColor = SystemColors.ControlLight;
      tableLayoutPanel3.ColumnCount = 2;
      tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel3.Controls.Add(shivCheckbox, 1, 14);
      tableLayoutPanel3.Controls.Add(shownSoldierLabel, 1, 0);
      tableLayoutPanel3.Controls.Add(perkComboBox3, 0, 5);
      tableLayoutPanel3.Controls.Add(perkComboBox2, 0, 3);
      tableLayoutPanel3.Controls.Add(blueCheckbox, 1, 13);
      tableLayoutPanel3.Controls.Add(woundedCheckbox, 0, 13);
      tableLayoutPanel3.Controls.Add(deadCheckbox, 0, 14);
      tableLayoutPanel3.Controls.Add(perkComboBox1, 0, 1);
      tableLayoutPanel3.Controls.Add(totalSoldierLabel, 0, 0);
      tableLayoutPanel3.Dock = DockStyle.Fill;
      tableLayoutPanel3.ForeColor = SystemColors.ControlText;
      tableLayoutPanel3.Location = new Point(0, 0);
      tableLayoutPanel3.Margin = new Padding(0);
      tableLayoutPanel3.Name = "tableLayoutPanel3";
      tableLayoutPanel3.RowCount = 16;
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 401F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel3.Size = new Size(328, 681);
      tableLayoutPanel3.TabIndex = 4;
      // 
      // shivCheckbox
      // 
      shivCheckbox.AutoSize = true;
      shivCheckbox.Dock = DockStyle.Fill;
      shivCheckbox.FlatStyle = FlatStyle.Flat;
      shivCheckbox.Location = new Point(164, 653);
      shivCheckbox.Margin = new Padding(0);
      shivCheckbox.Name = "shivCheckbox";
      shivCheckbox.Padding = new Padding(10, 0, 0, 0);
      shivCheckbox.Size = new Size(164, 20);
      shivCheckbox.TabIndex = 12;
      shivCheckbox.Text = "Shiv";
      shivCheckbox.UseVisualStyleBackColor = true;
      shivCheckbox.CheckedChanged += shivCheckBox_CheckedChanged;
      // 
      // shownSoldierLabel
      // 
      shownSoldierLabel.AutoSize = true;
      shownSoldierLabel.Depth = 0;
      shownSoldierLabel.Dock = DockStyle.Fill;
      shownSoldierLabel.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
      shownSoldierLabel.FontType = MaterialSkin.MaterialSkinManager.fontType.Caption;
      shownSoldierLabel.Location = new Point(168, 0);
      shownSoldierLabel.Margin = new Padding(4, 0, 4, 0);
      shownSoldierLabel.MouseState = MaterialSkin.MouseState.HOVER;
      shownSoldierLabel.Name = "shownSoldierLabel";
      shownSoldierLabel.Padding = new Padding(0, 0, 20, 0);
      shownSoldierLabel.Size = new Size(156, 22);
      shownSoldierLabel.TabIndex = 10;
      shownSoldierLabel.Text = "Shown: 0";
      shownSoldierLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // perkComboBox3
      // 
      perkComboBox3.AutoResize = false;
      perkComboBox3.BackColor = Color.FromArgb(255, 255, 255);
      tableLayoutPanel3.SetColumnSpan(perkComboBox3, 2);
      perkComboBox3.Depth = 0;
      perkComboBox3.Dock = DockStyle.Fill;
      perkComboBox3.DrawMode = DrawMode.OwnerDrawVariable;
      perkComboBox3.DropDownHeight = 466;
      perkComboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
      perkComboBox3.DropDownWidth = 121;
      perkComboBox3.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      perkComboBox3.ForeColor = Color.FromArgb(222, 0, 0, 0);
      perkComboBox3.FormattingEnabled = true;
      perkComboBox3.IntegralHeight = false;
      perkComboBox3.ItemHeight = 29;
      perkComboBox3.Location = new Point(4, 98);
      perkComboBox3.Margin = new Padding(4, 0, 4, 0);
      perkComboBox3.MaxDropDownItems = 16;
      perkComboBox3.MouseState = MaterialSkin.MouseState.OUT;
      perkComboBox3.Name = "perkComboBox3";
      perkComboBox3.Size = new Size(320, 35);
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
      perkComboBox2.DropDownHeight = 495;
      perkComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
      perkComboBox2.DropDownWidth = 121;
      perkComboBox2.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      perkComboBox2.ForeColor = Color.FromArgb(222, 0, 0, 0);
      perkComboBox2.FormattingEnabled = true;
      perkComboBox2.IntegralHeight = false;
      perkComboBox2.ItemHeight = 29;
      perkComboBox2.Location = new Point(4, 60);
      perkComboBox2.Margin = new Padding(4, 0, 4, 0);
      perkComboBox2.MaxDropDownItems = 17;
      perkComboBox2.MouseState = MaterialSkin.MouseState.OUT;
      perkComboBox2.Name = "perkComboBox2";
      perkComboBox2.Size = new Size(320, 35);
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
      blueCheckbox.Dock = DockStyle.Fill;
      blueCheckbox.FlatStyle = FlatStyle.Flat;
      blueCheckbox.Location = new Point(164, 633);
      blueCheckbox.Margin = new Padding(0);
      blueCheckbox.Name = "blueCheckbox";
      blueCheckbox.Padding = new Padding(10, 0, 0, 0);
      blueCheckbox.Size = new Size(164, 20);
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
      woundedCheckbox.Dock = DockStyle.Fill;
      woundedCheckbox.FlatStyle = FlatStyle.Flat;
      woundedCheckbox.Location = new Point(0, 633);
      woundedCheckbox.Margin = new Padding(0);
      woundedCheckbox.Name = "woundedCheckbox";
      woundedCheckbox.Padding = new Padding(10, 0, 0, 0);
      woundedCheckbox.Size = new Size(164, 20);
      woundedCheckbox.TabIndex = 4;
      woundedCheckbox.Text = "Wounded";
      woundedCheckbox.UseVisualStyleBackColor = true;
      woundedCheckbox.CheckedChanged += woundedCheckbox_CheckedChanged;
      // 
      // deadCheckbox
      // 
      deadCheckbox.AutoSize = true;
      deadCheckbox.Dock = DockStyle.Fill;
      deadCheckbox.FlatStyle = FlatStyle.Flat;
      deadCheckbox.Location = new Point(0, 653);
      deadCheckbox.Margin = new Padding(0);
      deadCheckbox.Name = "deadCheckbox";
      deadCheckbox.Padding = new Padding(10, 0, 0, 0);
      deadCheckbox.Size = new Size(164, 20);
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
      perkComboBox1.DropDownHeight = 524;
      perkComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
      perkComboBox1.DropDownWidth = 121;
      perkComboBox1.FlatStyle = FlatStyle.Flat;
      perkComboBox1.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      perkComboBox1.ForeColor = Color.FromArgb(222, 0, 0, 0);
      perkComboBox1.IntegralHeight = false;
      perkComboBox1.ItemHeight = 29;
      perkComboBox1.Location = new Point(4, 22);
      perkComboBox1.Margin = new Padding(4, 0, 4, 0);
      perkComboBox1.MaxDropDownItems = 18;
      perkComboBox1.MouseState = MaterialSkin.MouseState.OUT;
      perkComboBox1.Name = "perkComboBox1";
      perkComboBox1.Size = new Size(320, 35);
      perkComboBox1.StartIndex = 0;
      perkComboBox1.TabIndex = 6;
      perkComboBox1.UseTallSize = false;
      perkComboBox1.DrawItem += perkComboBox1_DrawItem;
      perkComboBox1.SelectedIndexChanged += perkComboBox1_SelectedIndexChanged;
      perkComboBox1.KeyDown += perkComboBox1_KeyDown;
      // 
      // totalSoldierLabel
      // 
      totalSoldierLabel.AutoSize = true;
      totalSoldierLabel.Depth = 0;
      totalSoldierLabel.Dock = DockStyle.Fill;
      totalSoldierLabel.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
      totalSoldierLabel.FontType = MaterialSkin.MaterialSkinManager.fontType.Caption;
      totalSoldierLabel.Location = new Point(4, 0);
      totalSoldierLabel.Margin = new Padding(4, 0, 4, 0);
      totalSoldierLabel.MouseState = MaterialSkin.MouseState.HOVER;
      totalSoldierLabel.Name = "totalSoldierLabel";
      totalSoldierLabel.Size = new Size(156, 22);
      totalSoldierLabel.TabIndex = 9;
      totalSoldierLabel.Text = "Total: 0";
      totalSoldierLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // Rosterizer
      // 
      AllowDrop = true;
      AutoScaleDimensions = new SizeF(7F, 14F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = SystemColors.ControlDarkDark;
      BackgroundImageLayout = ImageLayout.None;
      ClientSize = new Size(1264, 681);
      Controls.Add(tableLayoutPanel1);
      DoubleBuffered = true;
      Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      Margin = new Padding(0);
      MaximumSize = new Size(1280, 720);
      MinimizeBox = false;
      MinimumSize = new Size(1280, 720);
      Name = "Rosterizer";
      SizeGripStyle = SizeGripStyle.Hide;
      StartPosition = FormStartPosition.CenterScreen;
      DragDrop += Rosterizer_DragDrop;
      tableLayoutPanel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)rosterGridView).EndInit();
      tableLayoutPanel3.ResumeLayout(false);
      tableLayoutPanel3.PerformLayout();
      ResumeLayout(false);
    }

    #endregion
    private DataGridViewTextBoxColumn lNameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn nNameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn rankDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn xpDataGridViewTextBoxColumn;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel3;
    private CheckBox blueCheckbox;
    private CheckBox woundedCheckbox;
    private CheckBox deadCheckbox;
    private MaterialSkin.Controls.MaterialComboBox perkComboBox1;
    private MaterialSkin.Controls.MaterialComboBox perkComboBox2;
    private MaterialSkin.Controls.MaterialComboBox perkComboBox3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private MaterialSkin.Controls.MaterialLabel shownSoldierLabel;
    private MaterialSkin.Controls.MaterialLabel totalSoldierLabel;
    private DataGridView rosterGridView;
    private TabPage rosterTab;
    private TabPage perksTab;
    private TabPage itemsTab;
    private CheckBox shivCheckbox;
    private BindingSource jsonRootBindingSource;
    private DataGridViewTextBoxColumn LName;
    private DataGridViewTextBoxColumn NName;
    private DataGridViewTextBoxColumn Status;
    private DataGridViewTextBoxColumn SoldierClass;
    private DataGridViewTextBoxColumn Def;
    private DataGridViewTextBoxColumn HP;
    private DataGridViewTextBoxColumn Mob;
    private DataGridViewTextBoxColumn Will;
    private DataGridViewTextBoxColumn Aim;
    private DataGridViewTextBoxColumn XP;
    private DataGridViewTextBoxColumn Rank;
  }
}
