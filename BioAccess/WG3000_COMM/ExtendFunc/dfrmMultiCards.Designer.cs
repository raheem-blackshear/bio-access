namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMultiCards
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
        /// 
        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmMultiCards));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpOptInOut = new System.Windows.Forms.GroupBox();
            this.chkReaderIn = new System.Windows.Forms.CheckBox();
            this.chkReaderOut = new System.Windows.Forms.CheckBox();
            this.grpOption = new System.Windows.Forms.GroupBox();
            this.nudGrpStartOfSingle = new System.Windows.Forms.NumericUpDown();
            this.chkReadByOrder = new System.Windows.Forms.CheckBox();
            this.chkSingleGroup = new System.Windows.Forms.CheckBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.grpNeeded = new System.Windows.Forms.GroupBox();
            this.nudGrp3 = new System.Windows.Forms.NumericUpDown();
            this.Label10 = new System.Windows.Forms.Label();
            this.nudGrp8 = new System.Windows.Forms.NumericUpDown();
            this.Label9 = new System.Windows.Forms.Label();
            this.nudGrp6 = new System.Windows.Forms.NumericUpDown();
            this.Label8 = new System.Windows.Forms.Label();
            this.nudGrp5 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudGrp4 = new System.Windows.Forms.NumericUpDown();
            this.Label6 = new System.Windows.Forms.Label();
            this.nudGrp2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudGrp1 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudGrp7 = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.nudTotal = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblWait = new System.Windows.Forms.Label();
            this.nudGroupToAdd = new System.Windows.Forms.NumericUpDown();
            this.lblControlTimeSeg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvSelectedUsers = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MoreCards_GrpID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_SelectedGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MoreCards_GrpID_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SelectedUsers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_GroupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelAllUsers = new System.Windows.Forms.ImageButton();
            this.btnDelOneUser = new System.Windows.Forms.ImageButton();
            this.btnAddOneUser = new System.Windows.Forms.ImageButton();
            this.btnAddAllUsers = new System.Windows.Forms.ImageButton();
            this.cbof_GroupID = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grpOptInOut.SuspendLayout();
            this.grpOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrpStartOfSingle)).BeginInit();
            this.grpNeeded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupToAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // grpOptInOut
            // 
            this.grpOptInOut.BackColor = System.Drawing.Color.Transparent;
            this.grpOptInOut.Controls.Add(this.chkReaderIn);
            this.grpOptInOut.Controls.Add(this.chkReaderOut);
            this.grpOptInOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.grpOptInOut, "grpOptInOut");
            this.grpOptInOut.Name = "grpOptInOut";
            this.grpOptInOut.TabStop = false;
            // 
            // chkReaderIn
            // 
            this.chkReaderIn.Checked = true;
            this.chkReaderIn.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkReaderIn, "chkReaderIn");
            this.chkReaderIn.Name = "chkReaderIn";
            // 
            // chkReaderOut
            // 
            resources.ApplyResources(this.chkReaderOut, "chkReaderOut");
            this.chkReaderOut.Name = "chkReaderOut";
            // 
            // grpOption
            // 
            this.grpOption.BackColor = System.Drawing.Color.Transparent;
            this.grpOption.Controls.Add(this.nudGrpStartOfSingle);
            this.grpOption.Controls.Add(this.chkReadByOrder);
            this.grpOption.Controls.Add(this.chkSingleGroup);
            this.grpOption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.grpOption, "grpOption");
            this.grpOption.Name = "grpOption";
            this.grpOption.TabStop = false;
            // 
            // nudGrpStartOfSingle
            // 
            this.nudGrpStartOfSingle.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrpStartOfSingle, "nudGrpStartOfSingle");
            this.nudGrpStartOfSingle.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudGrpStartOfSingle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGrpStartOfSingle.Name = "nudGrpStartOfSingle";
            this.nudGrpStartOfSingle.ReadOnly = true;
            this.nudGrpStartOfSingle.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // chkReadByOrder
            // 
            resources.ApplyResources(this.chkReadByOrder, "chkReadByOrder");
            this.chkReadByOrder.Name = "chkReadByOrder";
            // 
            // chkSingleGroup
            // 
            resources.ApplyResources(this.chkSingleGroup, "chkSingleGroup");
            this.chkSingleGroup.Name = "chkSingleGroup";
            this.chkSingleGroup.CheckedChanged += new System.EventHandler(this.chkSingleGroup_CheckedChanged);
            // 
            // chkActive
            // 
            resources.ApplyResources(this.chkActive, "chkActive");
            this.chkActive.BackColor = System.Drawing.Color.Transparent;
            this.chkActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkActive.Name = "chkActive";
            this.chkActive.UseVisualStyleBackColor = false;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // grpNeeded
            // 
            this.grpNeeded.BackColor = System.Drawing.Color.Transparent;
            this.grpNeeded.Controls.Add(this.nudGrp3);
            this.grpNeeded.Controls.Add(this.Label10);
            this.grpNeeded.Controls.Add(this.nudGrp8);
            this.grpNeeded.Controls.Add(this.Label9);
            this.grpNeeded.Controls.Add(this.nudGrp6);
            this.grpNeeded.Controls.Add(this.Label8);
            this.grpNeeded.Controls.Add(this.nudGrp5);
            this.grpNeeded.Controls.Add(this.label1);
            this.grpNeeded.Controls.Add(this.nudGrp4);
            this.grpNeeded.Controls.Add(this.Label6);
            this.grpNeeded.Controls.Add(this.nudGrp2);
            this.grpNeeded.Controls.Add(this.label2);
            this.grpNeeded.Controls.Add(this.nudGrp1);
            this.grpNeeded.Controls.Add(this.label11);
            this.grpNeeded.Controls.Add(this.label12);
            this.grpNeeded.Controls.Add(this.nudGrp7);
            this.grpNeeded.Controls.Add(this.label13);
            this.grpNeeded.Controls.Add(this.nudTotal);
            this.grpNeeded.Controls.Add(this.label14);
            this.grpNeeded.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.grpNeeded, "grpNeeded");
            this.grpNeeded.Name = "grpNeeded";
            this.grpNeeded.TabStop = false;
            // 
            // nudGrp3
            // 
            this.nudGrp3.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp3, "nudGrp3");
            this.nudGrp3.Name = "nudGrp3";
            this.nudGrp3.ReadOnly = true;
            this.nudGrp3.ValueChanged += new System.EventHandler(this.nudGrp3_ValueChanged);
            // 
            // Label10
            // 
            resources.ApplyResources(this.Label10, "Label10");
            this.Label10.Name = "Label10";
            // 
            // nudGrp8
            // 
            this.nudGrp8.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp8, "nudGrp8");
            this.nudGrp8.Name = "nudGrp8";
            this.nudGrp8.ReadOnly = true;
            this.nudGrp8.ValueChanged += new System.EventHandler(this.nudGrp8_ValueChanged);
            // 
            // Label9
            // 
            resources.ApplyResources(this.Label9, "Label9");
            this.Label9.Name = "Label9";
            // 
            // nudGrp6
            // 
            this.nudGrp6.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp6, "nudGrp6");
            this.nudGrp6.Name = "nudGrp6";
            this.nudGrp6.ReadOnly = true;
            this.nudGrp6.ValueChanged += new System.EventHandler(this.nudGrp6_ValueChanged);
            // 
            // Label8
            // 
            resources.ApplyResources(this.Label8, "Label8");
            this.Label8.Name = "Label8";
            // 
            // nudGrp5
            // 
            this.nudGrp5.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp5, "nudGrp5");
            this.nudGrp5.Name = "nudGrp5";
            this.nudGrp5.ReadOnly = true;
            this.nudGrp5.ValueChanged += new System.EventHandler(this.nudGrp5_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // nudGrp4
            // 
            this.nudGrp4.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp4, "nudGrp4");
            this.nudGrp4.Name = "nudGrp4";
            this.nudGrp4.ReadOnly = true;
            this.nudGrp4.ValueChanged += new System.EventHandler(this.nudGrp4_ValueChanged);
            // 
            // Label6
            // 
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Name = "Label6";
            // 
            // nudGrp2
            // 
            this.nudGrp2.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp2, "nudGrp2");
            this.nudGrp2.Name = "nudGrp2";
            this.nudGrp2.ReadOnly = true;
            this.nudGrp2.ValueChanged += new System.EventHandler(this.nudGrp2_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // nudGrp1
            // 
            this.nudGrp1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp1, "nudGrp1");
            this.nudGrp1.Name = "nudGrp1";
            this.nudGrp1.ReadOnly = true;
            this.nudGrp1.ValueChanged += new System.EventHandler(this.nudGrp1_ValueChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // nudGrp7
            // 
            this.nudGrp7.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGrp7, "nudGrp7");
            this.nudGrp7.Name = "nudGrp7";
            this.nudGrp7.ReadOnly = true;
            this.nudGrp7.ValueChanged += new System.EventHandler(this.nudGrp7_ValueChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // nudTotal
            // 
            this.nudTotal.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudTotal, "nudTotal");
            this.nudTotal.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudTotal.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudTotal.Name = "nudTotal";
            this.nudTotal.ReadOnly = true;
            this.nudTotal.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudTotal.ValueChanged += new System.EventHandler(this.nudTotal_ValueChanged);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblWait);
            this.groupBox1.Controls.Add(this.nudGroupToAdd);
            this.groupBox1.Controls.Add(this.lblControlTimeSeg);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dgvSelectedUsers);
            this.groupBox1.Controls.Add(this.dgvUsers);
            this.groupBox1.Controls.Add(this.btnDelAllUsers);
            this.groupBox1.Controls.Add(this.btnDelOneUser);
            this.groupBox1.Controls.Add(this.btnAddOneUser);
            this.groupBox1.Controls.Add(this.btnAddAllUsers);
            this.groupBox1.Controls.Add(this.cbof_GroupID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblWait
            // 
            resources.ApplyResources(this.lblWait, "lblWait");
            this.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWait.Name = "lblWait";
            // 
            // nudGroupToAdd
            // 
            this.nudGroupToAdd.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudGroupToAdd, "nudGroupToAdd");
            this.nudGroupToAdd.Maximum = new decimal(new int[] {
            8,
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
            this.nudGroupToAdd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblControlTimeSeg
            // 
            this.lblControlTimeSeg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblControlTimeSeg, "lblControlTimeSeg");
            this.lblControlTimeSeg.Name = "lblControlTimeSeg";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // dgvSelectedUsers
            // 
            this.dgvSelectedUsers.AllowUserToAddRows = false;
            this.dgvSelectedUsers.AllowUserToDeleteRows = false;
            this.dgvSelectedUsers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
            this.dgvSelectedUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.f_MoreCards_GrpID,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewCheckBoxColumn1,
            this.f_SelectedGroup});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
            this.dgvSelectedUsers.Name = "dgvSelectedUsers";
            this.dgvSelectedUsers.ReadOnly = true;
            this.dgvSelectedUsers.RowTemplate.Height = 23;
            this.dgvSelectedUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedUsers.DoubleClick += new System.EventHandler(this.btnDelOneUser_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // f_MoreCards_GrpID
            // 
            resources.ApplyResources(this.f_MoreCards_GrpID, "f_MoreCards_GrpID");
            this.f_MoreCards_GrpID.Name = "f_MoreCards_GrpID";
            this.f_MoreCards_GrpID.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // f_SelectedGroup
            // 
            resources.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
            this.f_SelectedGroup.Name = "f_SelectedGroup";
            this.f_SelectedGroup.ReadOnly = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvUsers, "dgvUsers");
            this.dgvUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsumerID,
            this.f_MoreCards_GrpID_1,
            this.UserID,
            this.ConsumerName,
            this.CardNO,
            this.f_SelectedUsers,
            this.f_GroupID});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.DoubleClick += new System.EventHandler(this.btnAddOneUser_Click);
            // 
            // ConsumerID
            // 
            resources.ApplyResources(this.ConsumerID, "ConsumerID");
            this.ConsumerID.Name = "ConsumerID";
            this.ConsumerID.ReadOnly = true;
            // 
            // f_MoreCards_GrpID_1
            // 
            resources.ApplyResources(this.f_MoreCards_GrpID_1, "f_MoreCards_GrpID_1");
            this.f_MoreCards_GrpID_1.Name = "f_MoreCards_GrpID_1";
            this.f_MoreCards_GrpID_1.ReadOnly = true;
            // 
            // UserID
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.UserID.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.UserID, "UserID");
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            // 
            // ConsumerName
            // 
            resources.ApplyResources(this.ConsumerName, "ConsumerName");
            this.ConsumerName.Name = "ConsumerName";
            this.ConsumerName.ReadOnly = true;
            // 
            // CardNO
            // 
            resources.ApplyResources(this.CardNO, "CardNO");
            this.CardNO.Name = "CardNO";
            this.CardNO.ReadOnly = true;
            // 
            // f_SelectedUsers
            // 
            resources.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
            this.f_SelectedUsers.Name = "f_SelectedUsers";
            this.f_SelectedUsers.ReadOnly = true;
            // 
            // f_GroupID
            // 
            resources.ApplyResources(this.f_GroupID, "f_GroupID");
            this.f_GroupID.Name = "f_GroupID";
            this.f_GroupID.ReadOnly = true;
            // 
            // btnDelAllUsers
            // 
            this.btnDelAllUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
            this.btnDelAllUsers.Focusable = true;
            this.btnDelAllUsers.ForeColor = System.Drawing.Color.White;
            this.btnDelAllUsers.Name = "btnDelAllUsers";
            this.btnDelAllUsers.Toggle = false;
            this.btnDelAllUsers.UseVisualStyleBackColor = false;
            this.btnDelAllUsers.Click += new System.EventHandler(this.btnDelAllUsers_Click);
            // 
            // btnDelOneUser
            // 
            this.btnDelOneUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
            this.btnDelOneUser.Focusable = true;
            this.btnDelOneUser.ForeColor = System.Drawing.Color.White;
            this.btnDelOneUser.Name = "btnDelOneUser";
            this.btnDelOneUser.Toggle = false;
            this.btnDelOneUser.UseVisualStyleBackColor = false;
            this.btnDelOneUser.Click += new System.EventHandler(this.btnDelOneUser_Click);
            // 
            // btnAddOneUser
            // 
            this.btnAddOneUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
            this.btnAddOneUser.Focusable = true;
            this.btnAddOneUser.ForeColor = System.Drawing.Color.White;
            this.btnAddOneUser.Name = "btnAddOneUser";
            this.btnAddOneUser.Toggle = false;
            this.btnAddOneUser.UseVisualStyleBackColor = false;
            this.btnAddOneUser.Click += new System.EventHandler(this.btnAddOneUser_Click);
            // 
            // btnAddAllUsers
            // 
            this.btnAddAllUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
            this.btnAddAllUsers.Focusable = true;
            this.btnAddAllUsers.ForeColor = System.Drawing.Color.White;
            this.btnAddAllUsers.Name = "btnAddAllUsers";
            this.btnAddAllUsers.Toggle = false;
            this.btnAddAllUsers.UseVisualStyleBackColor = false;
            this.btnAddAllUsers.Click += new System.EventHandler(this.btnAddAllUsers_Click);
            // 
            // cbof_GroupID
            // 
            this.cbof_GroupID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_GroupID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
            this.cbof_GroupID.Name = "cbof_GroupID";
            this.cbof_GroupID.SelectedIndexChanged += new System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label4.Name = "label4";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // dfrmMultiCards
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grpOptInOut);
            this.Controls.Add(this.grpOption);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.grpNeeded);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "dfrmMultiCards";
            this.Load += new System.EventHandler(this.dfrmMultiCards_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmMultiCards_KeyDown);
            this.grpOptInOut.ResumeLayout(false);
            this.grpOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGrpStartOfSingle)).EndInit();
            this.grpNeeded.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrp7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupToAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ImageButton btnAddAllUsers;
        private ImageButton btnAddOneUser;
        internal ImageButton btnCancel;
        private ImageButton btnDelAllUsers;
        private ImageButton btnDelOneUser;
        internal ImageButton btnOK;
        private DataGridViewTextBoxColumn CardNO;
        private ComboBox cbof_GroupID;
        internal CheckBox chkActive;
        internal CheckBox chkReadByOrder;
        internal CheckBox chkReaderIn;
        internal CheckBox chkReaderOut;
        internal CheckBox chkSingleGroup;
        private DataGridViewTextBoxColumn ConsumerID;
        private DataGridViewTextBoxColumn ConsumerName;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView dgvSelectedUsers;
        private DataGridView dgvUsers;
        private DataGridViewTextBoxColumn f_GroupID;
        private DataGridViewTextBoxColumn f_MoreCards_GrpID;
        private DataGridViewTextBoxColumn f_MoreCards_GrpID_1;
        private DataGridViewTextBoxColumn f_SelectedGroup;
        private DataGridViewCheckBoxColumn f_SelectedUsers;
        private GroupBox groupBox1;
        internal GroupBox grpNeeded;
        internal GroupBox grpOptInOut;
        internal GroupBox grpOption;
        internal Label label1;
        internal Label Label10;
        internal Label label11;
        internal Label label12;
        internal Label label13;
        internal Label label14;
        internal Label label2;
        private Label label3;
        private Label label4;
        internal Label Label6;
        internal Label Label8;
        internal Label Label9;
        internal Label lblControlTimeSeg;
        private Label lblWait;
        internal NumericUpDown nudGroupToAdd;
        internal NumericUpDown nudGrp1;
        internal NumericUpDown nudGrp2;
        internal NumericUpDown nudGrp3;
        internal NumericUpDown nudGrp4;
        internal NumericUpDown nudGrp5;
        internal NumericUpDown nudGrp6;
        internal NumericUpDown nudGrp7;
        internal NumericUpDown nudGrp8;
        internal NumericUpDown nudGrpStartOfSingle;
        internal NumericUpDown nudTotal;
        private System.Windows.Forms.Timer timer1;
        private DataGridViewTextBoxColumn UserID;
    }
}

