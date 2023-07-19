namespace WG3000_COMM.ExtendFunc.Elevator
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmElevatorGroup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmElevatorGroup));
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.cbof_ZoneID = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.dgvSelectedDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeProfile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ZoneID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelAllDoors = new System.Windows.Forms.ImageButton();
            this.btnDelOneDoor = new System.Windows.Forms.ImageButton();
            this.btnAddOneDoor = new System.Windows.Forms.ImageButton();
            this.btnAddAllDoors = new System.Windows.Forms.ImageButton();
            this.lblOptional = new System.Windows.Forms.Label();
            this.lblSeleted = new System.Windows.Forms.Label();
            this.nudGroupToAdd = new System.Windows.Forms.NumericUpDown();
            this.lblControlTimeSeg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupToAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Image = global::Properties.Resources.Rec1Pass;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(587, 550);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(164, 27);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Confirm";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(791, 550);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(164, 27);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "Cancel";
            this.btnExit.Toggle = false;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cbof_ZoneID
            // 
            this.cbof_ZoneID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ZoneID.FormattingEnabled = true;
            this.cbof_ZoneID.Location = new System.Drawing.Point(64, 16);
            this.cbof_ZoneID.Name = "cbof_ZoneID";
            this.cbof_ZoneID.Size = new System.Drawing.Size(275, 20);
            this.cbof_ZoneID.TabIndex = 6;
            this.cbof_ZoneID.Visible = false;
            this.cbof_ZoneID.SelectedIndexChanged += new System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label25.Location = new System.Drawing.Point(7, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(35, 12);
            this.label25.TabIndex = 21;
            this.label25.Text = "Zone:";
            this.label25.Visible = false;
            // 
            // dgvSelectedDoors
            // 
            this.dgvSelectedDoors.AllowUserToAddRows = false;
            this.dgvSelectedDoors.AllowUserToDeleteRows = false;
            this.dgvSelectedDoors.AllowUserToOrderColumns = true;
            this.dgvSelectedDoors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSelectedDoors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(125)))), ((int)(((byte)(156)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.f_Selected2,
            this.dataGridViewTextBoxColumn9,
            this.Column1,
            this.TimeProfile,
            this.f_ControlSegName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
            this.dgvSelectedDoors.Location = new System.Drawing.Point(504, 42);
            this.dgvSelectedDoors.Name = "dgvSelectedDoors";
            this.dgvSelectedDoors.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedDoors.RowHeadersWidth = 20;
            this.dgvSelectedDoors.RowTemplate.Height = 23;
            this.dgvSelectedDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedDoors.Size = new System.Drawing.Size(454, 478);
            this.dgvSelectedDoors.TabIndex = 3;
            this.dgvSelectedDoors.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSelectedDoors_CellFormatting);
            this.dgvSelectedDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "DoorID";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // f_Selected2
            // 
            this.f_Selected2.HeaderText = "Group #";
            this.f_Selected2.Name = "f_Selected2";
            this.f_Selected2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Selected";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 280;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "ZoneID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // TimeProfile
            // 
            this.TimeProfile.HeaderText = "GroupNO";
            this.TimeProfile.Name = "TimeProfile";
            this.TimeProfile.ReadOnly = true;
            this.TimeProfile.Visible = false;
            this.TimeProfile.Width = 50;
            // 
            // f_ControlSegName
            // 
            this.f_ControlSegName.HeaderText = "Desc";
            this.f_ControlSegName.Name = "f_ControlSegName";
            this.f_ControlSegName.ReadOnly = true;
            this.f_ControlSegName.Visible = false;
            // 
            // dgvDoors
            // 
            this.dgvDoors.AllowUserToAddRows = false;
            this.dgvDoors.AllowUserToDeleteRows = false;
            this.dgvDoors.AllowUserToOrderColumns = true;
            this.dgvDoors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvDoors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(125)))), ((int)(((byte)(156)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6,
            this.f_Selected,
            this.dataGridViewTextBoxColumn7,
            this.f_ZoneID,
            this.Column2,
            this.f_ControlSegName1});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDoors.EnableHeadersVisualStyles = false;
            this.dgvDoors.Location = new System.Drawing.Point(8, 42);
            this.dgvDoors.Name = "dgvDoors";
            this.dgvDoors.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDoors.RowHeadersWidth = 20;
            this.dgvDoors.RowTemplate.Height = 23;
            this.dgvDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDoors.Size = new System.Drawing.Size(331, 478);
            this.dgvDoors.TabIndex = 0;
            this.dgvDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "DoorID";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // f_Selected
            // 
            this.f_Selected.HeaderText = "Selected";
            this.f_Selected.Name = "f_Selected";
            this.f_Selected.ReadOnly = true;
            this.f_Selected.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Optional";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 300;
            // 
            // f_ZoneID
            // 
            this.f_ZoneID.HeaderText = "ZoneID";
            this.f_ZoneID.Name = "f_ZoneID";
            this.f_ZoneID.ReadOnly = true;
            this.f_ZoneID.Visible = false;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "TimeProfile";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // f_ControlSegName1
            // 
            this.f_ControlSegName1.HeaderText = "Desc";
            this.f_ControlSegName1.Name = "f_ControlSegName1";
            this.f_ControlSegName1.ReadOnly = true;
            this.f_ControlSegName1.Visible = false;
            // 
            // btnDelAllDoors
            // 
            this.btnDelAllDoors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnDelAllDoors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelAllDoors.Focusable = true;
            this.btnDelAllDoors.ForeColor = System.Drawing.Color.White;
            this.btnDelAllDoors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDelAllDoors.Location = new System.Drawing.Point(340, 356);
            this.btnDelAllDoors.Name = "btnDelAllDoors";
            this.btnDelAllDoors.Size = new System.Drawing.Size(164, 27);
            this.btnDelAllDoors.TabIndex = 5;
            this.btnDelAllDoors.Text = "<<";
            this.btnDelAllDoors.Toggle = false;
            this.btnDelAllDoors.UseVisualStyleBackColor = false;
            this.btnDelAllDoors.Click += new System.EventHandler(this.btnDelAllDoors_Click);
            // 
            // btnDelOneDoor
            // 
            this.btnDelOneDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnDelOneDoor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelOneDoor.Focusable = true;
            this.btnDelOneDoor.ForeColor = System.Drawing.Color.White;
            this.btnDelOneDoor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDelOneDoor.Location = new System.Drawing.Point(340, 318);
            this.btnDelOneDoor.Name = "btnDelOneDoor";
            this.btnDelOneDoor.Size = new System.Drawing.Size(164, 27);
            this.btnDelOneDoor.TabIndex = 4;
            this.btnDelOneDoor.Text = "<";
            this.btnDelOneDoor.Toggle = false;
            this.btnDelOneDoor.UseVisualStyleBackColor = false;
            this.btnDelOneDoor.Click += new System.EventHandler(this.btnDelOneDoor_Click);
            // 
            // btnAddOneDoor
            // 
            this.btnAddOneDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnAddOneDoor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddOneDoor.Focusable = true;
            this.btnAddOneDoor.ForeColor = System.Drawing.Color.White;
            this.btnAddOneDoor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddOneDoor.Location = new System.Drawing.Point(340, 237);
            this.btnAddOneDoor.Name = "btnAddOneDoor";
            this.btnAddOneDoor.Size = new System.Drawing.Size(164, 27);
            this.btnAddOneDoor.TabIndex = 1;
            this.btnAddOneDoor.Text = ">";
            this.btnAddOneDoor.Toggle = false;
            this.btnAddOneDoor.UseVisualStyleBackColor = false;
            this.btnAddOneDoor.Click += new System.EventHandler(this.btnAddOneDoor_Click);
            // 
            // btnAddAllDoors
            // 
            this.btnAddAllDoors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnAddAllDoors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAllDoors.Focusable = true;
            this.btnAddAllDoors.ForeColor = System.Drawing.Color.White;
            this.btnAddAllDoors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddAllDoors.Location = new System.Drawing.Point(340, 199);
            this.btnAddAllDoors.Name = "btnAddAllDoors";
            this.btnAddAllDoors.Size = new System.Drawing.Size(164, 27);
            this.btnAddAllDoors.TabIndex = 2;
            this.btnAddAllDoors.Text = ">>";
            this.btnAddAllDoors.Toggle = false;
            this.btnAddAllDoors.UseVisualStyleBackColor = false;
            this.btnAddAllDoors.Click += new System.EventHandler(this.btnAddAllDoors_Click);
            // 
            // lblOptional
            // 
            this.lblOptional.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOptional.AutoSize = true;
            this.lblOptional.BackColor = System.Drawing.Color.Transparent;
            this.lblOptional.ForeColor = System.Drawing.Color.White;
            this.lblOptional.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblOptional.Location = new System.Drawing.Point(345, 49);
            this.lblOptional.Name = "lblOptional";
            this.lblOptional.Size = new System.Drawing.Size(23, 12);
            this.lblOptional.TabIndex = 24;
            this.lblOptional.Text = "---";
            // 
            // lblSeleted
            // 
            this.lblSeleted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSeleted.AutoSize = true;
            this.lblSeleted.BackColor = System.Drawing.Color.Transparent;
            this.lblSeleted.ForeColor = System.Drawing.Color.White;
            this.lblSeleted.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSeleted.Location = new System.Drawing.Point(475, 49);
            this.lblSeleted.Name = "lblSeleted";
            this.lblSeleted.Size = new System.Drawing.Size(23, 12);
            this.lblSeleted.TabIndex = 25;
            this.lblSeleted.Text = "---";
            // 
            // nudGroupToAdd
            // 
            this.nudGroupToAdd.BackColor = System.Drawing.Color.White;
            this.nudGroupToAdd.Location = new System.Drawing.Point(391, 169);
            this.nudGroupToAdd.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudGroupToAdd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGroupToAdd.Name = "nudGroupToAdd";
            this.nudGroupToAdd.ReadOnly = true;
            this.nudGroupToAdd.Size = new System.Drawing.Size(72, 21);
            this.nudGroupToAdd.TabIndex = 33;
            this.nudGroupToAdd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblControlTimeSeg
            // 
            this.lblControlTimeSeg.ForeColor = System.Drawing.Color.White;
            this.lblControlTimeSeg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblControlTimeSeg.Location = new System.Drawing.Point(371, 142);
            this.lblControlTimeSeg.Name = "lblControlTimeSeg";
            this.lblControlTimeSeg.Size = new System.Drawing.Size(104, 24);
            this.lblControlTimeSeg.TabIndex = 34;
            this.lblControlTimeSeg.Text = "Selected Group #";
            this.lblControlTimeSeg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(503, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 12);
            this.label1.TabIndex = 35;
            this.label1.Text = "Those controllers whose group # are same control the same.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dfrmElevatorGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(967, 606);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudGroupToAdd);
            this.Controls.Add(this.lblControlTimeSeg);
            this.Controls.Add(this.lblSeleted);
            this.Controls.Add(this.lblOptional);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.cbof_ZoneID);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.dgvSelectedDoors);
            this.Controls.Add(this.dgvDoors);
            this.Controls.Add(this.btnAddAllDoors);
            this.Controls.Add(this.btnDelAllDoors);
            this.Controls.Add(this.btnAddOneDoor);
            this.Controls.Add(this.btnDelOneDoor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "dfrmElevatorGroup";
            this.Text = "Group";
            this.Load += new System.EventHandler(this.dfrmPrivilegeSingle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupToAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton btnAddAllDoors;
        private ImageButton btnAddOneDoor;
        private ImageButton btnDelAllDoors;
        private ImageButton btnDelOneDoor;
        private ImageButton btnExit;
        private ImageButton btnOK;
        private ComboBox cbof_ZoneID;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridView dgvDoors;
        private DataGridView dgvSelectedDoors;
        private DataGridViewTextBoxColumn f_ControlSegName;
        private DataGridViewTextBoxColumn f_ControlSegName1;
        private DataGridViewTextBoxColumn f_Selected;
        private DataGridViewTextBoxColumn f_Selected2;
        private DataGridViewTextBoxColumn f_ZoneID;
        internal Label label1;
        private Label label25;
        internal Label lblControlTimeSeg;
        private Label lblOptional;
        private Label lblSeleted;
        internal NumericUpDown nudGroupToAdd;
        private DataGridViewTextBoxColumn TimeProfile;
    }
}

