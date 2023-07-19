namespace WG3000_COMM.ExtendFunc.Patrol
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmRouteEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmRouteEdit));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbof_RouteID = new System.Windows.Forms.ComboBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCopyFromOtherRoute = new System.Windows.Forms.ImageButton();
            this.btnStartTimeUpdate = new System.Windows.Forms.ImageButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutoAdd = new System.Windows.Forms.CheckBox();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.label45 = new System.Windows.Forms.Label();
            this.dateBeginHMS1 = new System.Windows.Forms.DateTimePicker();
            this.nudMinute = new System.Windows.Forms.NumericUpDown();
            this.dgvSelected = new System.Windows.Forms.DataGridView();
            this.NextDay = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SN2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label10 = new System.Windows.Forms.Label();
            this.dgvOptional = new System.Windows.Forms.DataGridView();
            this.f_NextDay1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_patroltime1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Sn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteAllReaders = new System.Windows.Forms.ImageButton();
            this.Label11 = new System.Windows.Forms.Label();
            this.btnDeleteOneReader = new System.Windows.Forms.ImageButton();
            this.btnAddAllReaders = new System.Windows.Forms.ImageButton();
            this.btnAddOneReader = new System.Windows.Forms.ImageButton();
            this.cmdCancel = new System.Windows.Forms.ImageButton();
            this.cmdOK = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptional)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            // 
            // cbof_RouteID
            // 
            this.cbof_RouteID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_RouteID, "cbof_RouteID");
            this.cbof_RouteID.Name = "cbof_RouteID";
            // 
            // Label8
            // 
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label8, "Label8");
            this.Label8.Name = "Label8";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label2.Name = "label2";
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnCopyFromOtherRoute);
            this.tabPage1.Controls.Add(this.btnStartTimeUpdate);
            this.tabPage1.Controls.Add(this.radioButton2);
            this.tabPage1.Controls.Add(this.radioButton1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.chkAutoAdd);
            this.tabPage1.Controls.Add(this.dtpTime);
            this.tabPage1.Controls.Add(this.label45);
            this.tabPage1.Controls.Add(this.dateBeginHMS1);
            this.tabPage1.Controls.Add(this.nudMinute);
            this.tabPage1.Controls.Add(this.dgvSelected);
            this.tabPage1.Controls.Add(this.Label10);
            this.tabPage1.Controls.Add(this.dgvOptional);
            this.tabPage1.Controls.Add(this.btnDeleteAllReaders);
            this.tabPage1.Controls.Add(this.Label11);
            this.tabPage1.Controls.Add(this.btnDeleteOneReader);
            this.tabPage1.Controls.Add(this.btnAddAllReaders);
            this.tabPage1.Controls.Add(this.btnAddOneReader);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // btnCopyFromOtherRoute
            // 
            this.btnCopyFromOtherRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnCopyFromOtherRoute, "btnCopyFromOtherRoute");
            this.btnCopyFromOtherRoute.Focusable = true;
            this.btnCopyFromOtherRoute.ForeColor = System.Drawing.Color.White;
            this.btnCopyFromOtherRoute.Name = "btnCopyFromOtherRoute";
            this.btnCopyFromOtherRoute.Toggle = false;
            this.btnCopyFromOtherRoute.UseVisualStyleBackColor = false;
            this.btnCopyFromOtherRoute.Click += new System.EventHandler(this.btnCopyFromOtherRoute_Click);
            // 
            // btnStartTimeUpdate
            // 
            this.btnStartTimeUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnStartTimeUpdate, "btnStartTimeUpdate");
            this.btnStartTimeUpdate.Focusable = true;
            this.btnStartTimeUpdate.ForeColor = System.Drawing.Color.White;
            this.btnStartTimeUpdate.Name = "btnStartTimeUpdate";
            this.btnStartTimeUpdate.Toggle = false;
            this.btnStartTimeUpdate.UseVisualStyleBackColor = false;
            this.btnStartTimeUpdate.Click += new System.EventHandler(this.btnStartTimeUpdate_Click);
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // chkAutoAdd
            // 
            resources.ApplyResources(this.chkAutoAdd, "chkAutoAdd");
            this.chkAutoAdd.Checked = true;
            this.chkAutoAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkAutoAdd.Name = "chkAutoAdd";
            this.chkAutoAdd.UseVisualStyleBackColor = true;
            // 
            // dtpTime
            // 
            resources.ApplyResources(this.dtpTime, "dtpTime");
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Value = new System.DateTime(2012, 6, 12, 0, 0, 0, 0);
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.BackColor = System.Drawing.Color.Transparent;
            this.label45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label45.Name = "label45";
            // 
            // dateBeginHMS1
            // 
            resources.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
            this.dateBeginHMS1.Name = "dateBeginHMS1";
            this.dateBeginHMS1.ShowUpDown = true;
            this.dateBeginHMS1.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // nudMinute
            // 
            resources.ApplyResources(this.nudMinute, "nudMinute");
            this.nudMinute.Maximum = new decimal(new int[] {
            2400,
            0,
            0,
            0});
            this.nudMinute.Name = "nudMinute";
            this.nudMinute.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // dgvSelected
            // 
            this.dgvSelected.AllowUserToAddRows = false;
            this.dgvSelected.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvSelected, "dgvSelected");
            this.dgvSelected.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NextDay,
            this.Cost,
            this.f_SN2,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelected.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelected.EnableHeadersVisualStyles = false;
            this.dgvSelected.Name = "dgvSelected";
            this.dgvSelected.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelected.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelected.RowTemplate.Height = 23;
            this.dgvSelected.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelected.DoubleClick += new System.EventHandler(this.btnDeleteOneReader_Click);
            // 
            // NextDay
            // 
            this.NextDay.Frozen = true;
            resources.ApplyResources(this.NextDay, "NextDay");
            this.NextDay.Name = "NextDay";
            this.NextDay.ReadOnly = true;
            this.NextDay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Cost
            // 
            resources.ApplyResources(this.Cost, "Cost");
            this.Cost.Name = "Cost";
            this.Cost.ReadOnly = true;
            this.Cost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_SN2
            // 
            resources.ApplyResources(this.f_SN2, "f_SN2");
            this.f_SN2.Name = "f_SN2";
            this.f_SN2.ReadOnly = true;
            this.f_SN2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Label10
            // 
            this.Label10.BackColor = System.Drawing.Color.Transparent;
            this.Label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label10, "Label10");
            this.Label10.Name = "Label10";
            // 
            // dgvOptional
            // 
            this.dgvOptional.AllowUserToAddRows = false;
            this.dgvOptional.AllowUserToDeleteRows = false;
            this.dgvOptional.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvOptional, "dgvOptional");
            this.dgvOptional.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOptional.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvOptional.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOptional.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_NextDay1,
            this.f_patroltime1,
            this.f_Sn,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.f_Selected});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOptional.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvOptional.EnableHeadersVisualStyles = false;
            this.dgvOptional.Name = "dgvOptional";
            this.dgvOptional.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOptional.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvOptional.RowTemplate.Height = 23;
            this.dgvOptional.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOptional.DoubleClick += new System.EventHandler(this.btnAddOneReader_Click);
            // 
            // f_NextDay1
            // 
            resources.ApplyResources(this.f_NextDay1, "f_NextDay1");
            this.f_NextDay1.Name = "f_NextDay1";
            this.f_NextDay1.ReadOnly = true;
            // 
            // f_patroltime1
            // 
            resources.ApplyResources(this.f_patroltime1, "f_patroltime1");
            this.f_patroltime1.Name = "f_patroltime1";
            this.f_patroltime1.ReadOnly = true;
            // 
            // f_Sn
            // 
            resources.ApplyResources(this.f_Sn, "f_Sn");
            this.f_Sn.Name = "f_Sn";
            this.f_Sn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // f_Selected
            // 
            resources.ApplyResources(this.f_Selected, "f_Selected");
            this.f_Selected.Name = "f_Selected";
            this.f_Selected.ReadOnly = true;
            // 
            // btnDeleteAllReaders
            // 
            this.btnDeleteAllReaders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteAllReaders, "btnDeleteAllReaders");
            this.btnDeleteAllReaders.Focusable = true;
            this.btnDeleteAllReaders.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAllReaders.Name = "btnDeleteAllReaders";
            this.btnDeleteAllReaders.Toggle = false;
            this.btnDeleteAllReaders.UseVisualStyleBackColor = false;
            this.btnDeleteAllReaders.Click += new System.EventHandler(this.btnDeleteAllReaders_Click);
            // 
            // Label11
            // 
            this.Label11.BackColor = System.Drawing.Color.Transparent;
            this.Label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label11, "Label11");
            this.Label11.Name = "Label11";
            // 
            // btnDeleteOneReader
            // 
            this.btnDeleteOneReader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteOneReader, "btnDeleteOneReader");
            this.btnDeleteOneReader.Focusable = true;
            this.btnDeleteOneReader.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOneReader.Name = "btnDeleteOneReader";
            this.btnDeleteOneReader.Toggle = false;
            this.btnDeleteOneReader.UseVisualStyleBackColor = false;
            this.btnDeleteOneReader.Click += new System.EventHandler(this.btnDeleteOneReader_Click);
            // 
            // btnAddAllReaders
            // 
            this.btnAddAllReaders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAllReaders, "btnAddAllReaders");
            this.btnAddAllReaders.Focusable = true;
            this.btnAddAllReaders.ForeColor = System.Drawing.Color.White;
            this.btnAddAllReaders.Name = "btnAddAllReaders";
            this.btnAddAllReaders.Toggle = false;
            this.btnAddAllReaders.UseVisualStyleBackColor = false;
            this.btnAddAllReaders.Click += new System.EventHandler(this.btnAddAllReaders_Click);
            // 
            // btnAddOneReader
            // 
            this.btnAddOneReader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneReader, "btnAddOneReader");
            this.btnAddOneReader.Focusable = true;
            this.btnAddOneReader.ForeColor = System.Drawing.Color.White;
            this.btnAddOneReader.Name = "btnAddOneReader";
            this.btnAddOneReader.Toggle = false;
            this.btnAddOneReader.UseVisualStyleBackColor = false;
            this.btnAddOneReader.Click += new System.EventHandler(this.btnAddOneReader_Click);
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdCancel.Focusable = true;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Toggle = false;
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            resources.ApplyResources(this.cmdOK, "cmdOK");
            this.cmdOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdOK.Focusable = true;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Toggle = false;
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdOK);
            this.panelBottomBanner.Controls.Add(this.cmdCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmRouteEdit
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cbof_RouteID);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.MinimizeBox = false;
            this.Name = "dfrmRouteEdit";
            this.Load += new System.EventHandler(this.dfrmMealOption_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptional)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnAddAllReaders;
        internal ImageButton btnAddOneReader;
        internal ImageButton btnCopyFromOtherRoute;
        internal ImageButton btnDeleteAllReaders;
        internal ImageButton btnDeleteOneReader;
        internal ImageButton btnStartTimeUpdate;
        internal ComboBox cbof_RouteID;
        private CheckBox chkAutoAdd;
        internal ImageButton cmdCancel;
        internal ImageButton cmdOK;
        private DataGridViewTextBoxColumn Cost;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DateTimePicker dateBeginHMS1;
        private DataGridView dgvOptional;
        private DataGridView dgvSelected;
        private DateTimePicker dtpTime;
        private DataGridViewTextBoxColumn f_NextDay1;
        private DataGridViewTextBoxColumn f_patroltime1;
        private DataGridViewTextBoxColumn f_Selected;
        private DataGridViewTextBoxColumn f_Sn;
        private DataGridViewTextBoxColumn f_SN2;
        private Label label1;
        internal Label Label10;
        internal Label Label11;
        private Label label2;
        internal Label label3;
        private Label label45;
        internal Label Label8;
        private DataGridViewCheckBoxColumn NextDay;
        private NumericUpDown nudMinute;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        internal TextBox txtName;
        private Panel panelBottomBanner;
    }
}

