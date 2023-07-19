namespace WG3000_COMM.ExtendFunc.Meal
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

    public partial class dfrmMealSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmMealSetup));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvSelected = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label10 = new System.Windows.Forms.Label();
            this.dgvOptional = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteAllReaders = new System.Windows.Forms.ImageButton();
            this.Label11 = new System.Windows.Forms.Label();
            this.btnDeleteOneReader = new System.Windows.Forms.ImageButton();
            this.btnAddAllReaders = new System.Windows.Forms.ImageButton();
            this.btnAddOneReader = new System.Windows.Forms.ImageButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkAllowableSwipe = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudRuleSeconds = new System.Windows.Forms.NumericUpDown();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnOption3 = new System.Windows.Forms.ImageButton();
            this.btnOption2 = new System.Windows.Forms.ImageButton();
            this.btnOption1 = new System.Windows.Forms.ImageButton();
            this.btnOption0 = new System.Windows.Forms.ImageButton();
            this.chkOtherMeal = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS4 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS4 = new System.Windows.Forms.DateTimePicker();
            this.nudOther = new System.Windows.Forms.NumericUpDown();
            this.lblOther = new System.Windows.Forms.Label();
            this.chkEveningMeal = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS3 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS3 = new System.Windows.Forms.DateTimePicker();
            this.nudEvening = new System.Windows.Forms.NumericUpDown();
            this.lblEvening = new System.Windows.Forms.Label();
            this.chkLunchMeal = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS2 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS2 = new System.Windows.Forms.DateTimePicker();
            this.nudLunch = new System.Windows.Forms.NumericUpDown();
            this.lblLunch = new System.Windows.Forms.Label();
            this.chkMorningMeal = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateEndHMS1 = new System.Windows.Forms.DateTimePicker();
            this.nudMorning = new System.Windows.Forms.NumericUpDown();
            this.lblMorning = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.ImageButton();
            this.btnDel = new System.Windows.Forms.ImageButton();
            this.btnAdd = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptional)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleSeconds)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOther)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEvening)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLunch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMorning)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage1, "tabPage1");
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
            // dgvSelected
            // 
            this.dgvSelected.AllowUserToAddRows = false;
            this.dgvSelected.AllowUserToDeleteRows = false;
            this.dgvSelected.AllowUserToOrderColumns = true;
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
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.chkAllowableSwipe);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.nudRuleSeconds);
            this.tabPage2.Controls.Add(this.radioButton3);
            this.tabPage2.Controls.Add(this.radioButton2);
            this.tabPage2.Controls.Add(this.radioButton1);
            this.tabPage2.ForeColor = System.Drawing.Color.White;
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkAllowableSwipe
            // 
            resources.ApplyResources(this.chkAllowableSwipe, "chkAllowableSwipe");
            this.chkAllowableSwipe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkAllowableSwipe.Name = "chkAllowableSwipe";
            this.chkAllowableSwipe.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // nudRuleSeconds
            // 
            resources.ApplyResources(this.nudRuleSeconds, "nudRuleSeconds");
            this.nudRuleSeconds.Name = "nudRuleSeconds";
            this.nudRuleSeconds.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
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
            // tabPage3
            // 
            this.tabPage3.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.btnOption3);
            this.tabPage3.Controls.Add(this.btnOption2);
            this.tabPage3.Controls.Add(this.btnOption1);
            this.tabPage3.Controls.Add(this.btnOption0);
            this.tabPage3.Controls.Add(this.chkOtherMeal);
            this.tabPage3.Controls.Add(this.dateBeginHMS4);
            this.tabPage3.Controls.Add(this.dateEndHMS4);
            this.tabPage3.Controls.Add(this.nudOther);
            this.tabPage3.Controls.Add(this.lblOther);
            this.tabPage3.Controls.Add(this.chkEveningMeal);
            this.tabPage3.Controls.Add(this.dateBeginHMS3);
            this.tabPage3.Controls.Add(this.dateEndHMS3);
            this.tabPage3.Controls.Add(this.nudEvening);
            this.tabPage3.Controls.Add(this.lblEvening);
            this.tabPage3.Controls.Add(this.chkLunchMeal);
            this.tabPage3.Controls.Add(this.dateBeginHMS2);
            this.tabPage3.Controls.Add(this.dateEndHMS2);
            this.tabPage3.Controls.Add(this.nudLunch);
            this.tabPage3.Controls.Add(this.lblLunch);
            this.tabPage3.Controls.Add(this.chkMorningMeal);
            this.tabPage3.Controls.Add(this.dateBeginHMS1);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.dateEndHMS1);
            this.tabPage3.Controls.Add(this.nudMorning);
            this.tabPage3.Controls.Add(this.lblMorning);
            this.tabPage3.Controls.Add(this.label85);
            this.tabPage3.Controls.Add(this.btnEdit);
            this.tabPage3.Controls.Add(this.btnDel);
            this.tabPage3.Controls.Add(this.btnAdd);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnOption3
            // 
            this.btnOption3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOption3, "btnOption3");
            this.btnOption3.Focusable = true;
            this.btnOption3.ForeColor = System.Drawing.Color.White;
            this.btnOption3.Name = "btnOption3";
            this.btnOption3.Toggle = false;
            this.btnOption3.UseVisualStyleBackColor = false;
            this.btnOption3.Click += new System.EventHandler(this.btnOption3_Click);
            // 
            // btnOption2
            // 
            this.btnOption2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOption2, "btnOption2");
            this.btnOption2.Focusable = true;
            this.btnOption2.ForeColor = System.Drawing.Color.White;
            this.btnOption2.Name = "btnOption2";
            this.btnOption2.Toggle = false;
            this.btnOption2.UseVisualStyleBackColor = false;
            this.btnOption2.Click += new System.EventHandler(this.btnOption2_Click);
            // 
            // btnOption1
            // 
            this.btnOption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOption1, "btnOption1");
            this.btnOption1.Focusable = true;
            this.btnOption1.ForeColor = System.Drawing.Color.White;
            this.btnOption1.Name = "btnOption1";
            this.btnOption1.Toggle = false;
            this.btnOption1.UseVisualStyleBackColor = false;
            this.btnOption1.Click += new System.EventHandler(this.btnOption1_Click);
            // 
            // btnOption0
            // 
            this.btnOption0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOption0, "btnOption0");
            this.btnOption0.Focusable = true;
            this.btnOption0.ForeColor = System.Drawing.Color.White;
            this.btnOption0.Name = "btnOption0";
            this.btnOption0.Toggle = false;
            this.btnOption0.UseVisualStyleBackColor = false;
            this.btnOption0.Click += new System.EventHandler(this.btnOption0_Click);
            // 
            // chkOtherMeal
            // 
            resources.ApplyResources(this.chkOtherMeal, "chkOtherMeal");
            this.chkOtherMeal.Checked = true;
            this.chkOtherMeal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOtherMeal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkOtherMeal.Name = "chkOtherMeal";
            this.chkOtherMeal.UseVisualStyleBackColor = true;
            this.chkOtherMeal.CheckedChanged += new System.EventHandler(this.chkMeal_CheckedChanged);
            // 
            // dateBeginHMS4
            // 
            resources.ApplyResources(this.dateBeginHMS4, "dateBeginHMS4");
            this.dateBeginHMS4.Name = "dateBeginHMS4";
            this.dateBeginHMS4.ShowUpDown = true;
            this.dateBeginHMS4.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateEndHMS4
            // 
            resources.ApplyResources(this.dateEndHMS4, "dateEndHMS4");
            this.dateEndHMS4.Name = "dateEndHMS4";
            this.dateEndHMS4.ShowUpDown = true;
            this.dateEndHMS4.Value = new System.DateTime(2010, 1, 1, 23, 59, 0, 0);
            // 
            // nudOther
            // 
            this.nudOther.DecimalPlaces = 2;
            resources.ApplyResources(this.nudOther, "nudOther");
            this.nudOther.Name = "nudOther";
            // 
            // lblOther
            // 
            resources.ApplyResources(this.lblOther, "lblOther");
            this.lblOther.ForeColor = System.Drawing.Color.White;
            this.lblOther.Name = "lblOther";
            // 
            // chkEveningMeal
            // 
            resources.ApplyResources(this.chkEveningMeal, "chkEveningMeal");
            this.chkEveningMeal.Checked = true;
            this.chkEveningMeal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEveningMeal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkEveningMeal.Name = "chkEveningMeal";
            this.chkEveningMeal.UseVisualStyleBackColor = true;
            this.chkEveningMeal.CheckedChanged += new System.EventHandler(this.chkMeal_CheckedChanged);
            // 
            // dateBeginHMS3
            // 
            resources.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
            this.dateBeginHMS3.Name = "dateBeginHMS3";
            this.dateBeginHMS3.ShowUpDown = true;
            this.dateBeginHMS3.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateEndHMS3
            // 
            resources.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
            this.dateEndHMS3.Name = "dateEndHMS3";
            this.dateEndHMS3.ShowUpDown = true;
            this.dateEndHMS3.Value = new System.DateTime(2010, 1, 1, 23, 59, 0, 0);
            // 
            // nudEvening
            // 
            this.nudEvening.DecimalPlaces = 2;
            resources.ApplyResources(this.nudEvening, "nudEvening");
            this.nudEvening.Name = "nudEvening";
            // 
            // lblEvening
            // 
            resources.ApplyResources(this.lblEvening, "lblEvening");
            this.lblEvening.ForeColor = System.Drawing.Color.White;
            this.lblEvening.Name = "lblEvening";
            // 
            // chkLunchMeal
            // 
            resources.ApplyResources(this.chkLunchMeal, "chkLunchMeal");
            this.chkLunchMeal.Checked = true;
            this.chkLunchMeal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLunchMeal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkLunchMeal.Name = "chkLunchMeal";
            this.chkLunchMeal.UseVisualStyleBackColor = true;
            this.chkLunchMeal.CheckedChanged += new System.EventHandler(this.chkMeal_CheckedChanged);
            // 
            // dateBeginHMS2
            // 
            resources.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
            this.dateBeginHMS2.Name = "dateBeginHMS2";
            this.dateBeginHMS2.ShowUpDown = true;
            this.dateBeginHMS2.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateEndHMS2
            // 
            resources.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
            this.dateEndHMS2.Name = "dateEndHMS2";
            this.dateEndHMS2.ShowUpDown = true;
            this.dateEndHMS2.Value = new System.DateTime(2010, 1, 1, 23, 59, 0, 0);
            // 
            // nudLunch
            // 
            this.nudLunch.DecimalPlaces = 2;
            resources.ApplyResources(this.nudLunch, "nudLunch");
            this.nudLunch.Name = "nudLunch";
            // 
            // lblLunch
            // 
            resources.ApplyResources(this.lblLunch, "lblLunch");
            this.lblLunch.ForeColor = System.Drawing.Color.White;
            this.lblLunch.Name = "lblLunch";
            // 
            // chkMorningMeal
            // 
            resources.ApplyResources(this.chkMorningMeal, "chkMorningMeal");
            this.chkMorningMeal.Checked = true;
            this.chkMorningMeal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMorningMeal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkMorningMeal.Name = "chkMorningMeal";
            this.chkMorningMeal.UseVisualStyleBackColor = true;
            this.chkMorningMeal.CheckedChanged += new System.EventHandler(this.chkMeal_CheckedChanged);
            // 
            // dateBeginHMS1
            // 
            resources.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
            this.dateBeginHMS1.Name = "dateBeginHMS1";
            this.dateBeginHMS1.ShowUpDown = true;
            this.dateBeginHMS1.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // dateEndHMS1
            // 
            resources.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
            this.dateEndHMS1.Name = "dateEndHMS1";
            this.dateEndHMS1.ShowUpDown = true;
            this.dateEndHMS1.Value = new System.DateTime(2010, 1, 1, 23, 59, 0, 0);
            // 
            // nudMorning
            // 
            this.nudMorning.DecimalPlaces = 2;
            resources.ApplyResources(this.nudMorning, "nudMorning");
            this.nudMorning.Name = "nudMorning";
            // 
            // lblMorning
            // 
            resources.ApplyResources(this.lblMorning, "lblMorning");
            this.lblMorning.ForeColor = System.Drawing.Color.White;
            this.lblMorning.Name = "lblMorning";
            // 
            // label85
            // 
            this.label85.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.label85, "label85");
            this.label85.Name = "label85";
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Focusable = true;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Toggle = false;
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDel, "btnDel");
            this.btnDel.Focusable = true;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            this.btnDel.Name = "btnDel";
            this.btnDel.Toggle = false;
            this.btnDel.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Focusable = true;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Toggle = false;
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmMealSetup
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmMealSetup";
            this.Load += new System.EventHandler(this.dfrmMealSetup_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptional)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleSeconds)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOther)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEvening)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLunch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMorning)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageButton btnAdd;
        internal ImageButton btnAddAllReaders;
        internal ImageButton btnAddOneReader;
        internal ImageButton btnCancel;
        private ImageButton btnDel;
        internal ImageButton btnDeleteAllReaders;
        internal ImageButton btnDeleteOneReader;
        private ImageButton btnEdit;
        internal ImageButton btnOK;
        private ImageButton btnOption0;
        private ImageButton btnOption1;
        private ImageButton btnOption2;
        private ImageButton btnOption3;
        private CheckBox chkAllowableSwipe;
        private CheckBox chkEveningMeal;
        private CheckBox chkLunchMeal;
        private CheckBox chkMorningMeal;
        private CheckBox chkOtherMeal;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DateTimePicker dateBeginHMS1;
        private DateTimePicker dateBeginHMS2;
        private DateTimePicker dateBeginHMS3;
        private DateTimePicker dateBeginHMS4;
        private DateTimePicker dateEndHMS1;
        private DateTimePicker dateEndHMS2;
        private DateTimePicker dateEndHMS3;
        private DateTimePicker dateEndHMS4;
        private DataGridView dgvOptional;
        private DataGridView dgvSelected;
        private DataGridViewTextBoxColumn f_Selected;
        private Label label1;
        internal Label Label10;
        internal Label Label11;
        private Label label3;
        private Label label85;
        private Label lblEvening;
        private Label lblLunch;
        private Label lblMorning;
        private Label lblOther;
        private NumericUpDown nudEvening;
        private NumericUpDown nudLunch;
        private NumericUpDown nudMorning;
        private NumericUpDown nudOther;
        private NumericUpDown nudRuleSeconds;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Panel panelBottomBanner;
    }
}

