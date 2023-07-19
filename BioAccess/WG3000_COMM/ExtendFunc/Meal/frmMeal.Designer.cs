namespace WG3000_COMM.ExtendFunc.Meal
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmMeal : frmBioAccess
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeal));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dgvSwipeRecords = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReadDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MealName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReaderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnMealSetup = new System.Windows.Forms.ToolStripButton();
            this.btnCreateReport = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvSubtotal = new System.Windows.Forms.DataGridView();
            this.f_ReaderID2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Morning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lunch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Evening = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Other = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSwipeRecords)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubtotal)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dgvSwipeRecords
            // 
            this.dgvSwipeRecords.AllowUserToAddRows = false;
            this.dgvSwipeRecords.AllowUserToDeleteRows = false;
            this.dgvSwipeRecords.AllowUserToOrderColumns = true;
            this.dgvSwipeRecords.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSwipeRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSwipeRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSwipeRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_RecID,
            this.f_ConsumerID,
            this.f_DepartmentName,
            this.f_ConsumerNO,
            this.f_ConsumerName,
            this.f_ReadDate,
            this.MealName,
            this.f_Cost,
            this.f_Addr,
            this.f_ReaderID});
            resources.ApplyResources(this.dgvSwipeRecords, "dgvSwipeRecords");
            this.dgvSwipeRecords.EnableHeadersVisualStyles = false;
            this.dgvSwipeRecords.Name = "dgvSwipeRecords";
            this.dgvSwipeRecords.ReadOnly = true;
            this.dgvSwipeRecords.RowHeadersVisible = false;
            this.dgvSwipeRecords.RowTemplate.Height = 23;
            this.dgvSwipeRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSwipeRecords.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSwipeRecords_CellFormatting);
            this.dgvSwipeRecords.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSwipeRecords_Scroll);
            // 
            // f_RecID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
            // 
            // f_ConsumerID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ConsumerID.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
            this.f_ConsumerID.Name = "f_ConsumerID";
            this.f_ConsumerID.ReadOnly = true;
            // 
            // f_DepartmentName
            // 
            resources.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
            this.f_DepartmentName.Name = "f_DepartmentName";
            this.f_DepartmentName.ReadOnly = true;
            // 
            // f_ConsumerNO
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
            this.f_ConsumerNO.Name = "f_ConsumerNO";
            this.f_ConsumerNO.ReadOnly = true;
            // 
            // f_ConsumerName
            // 
            resources.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
            this.f_ConsumerName.Name = "f_ConsumerName";
            this.f_ConsumerName.ReadOnly = true;
            // 
            // f_ReadDate
            // 
            resources.ApplyResources(this.f_ReadDate, "f_ReadDate");
            this.f_ReadDate.Name = "f_ReadDate";
            this.f_ReadDate.ReadOnly = true;
            // 
            // MealName
            // 
            resources.ApplyResources(this.MealName, "MealName");
            this.MealName.Name = "MealName";
            this.MealName.ReadOnly = true;
            // 
            // f_Cost
            // 
            resources.ApplyResources(this.f_Cost, "f_Cost");
            this.f_Cost.Name = "f_Cost";
            this.f_Cost.ReadOnly = true;
            // 
            // f_Addr
            // 
            resources.ApplyResources(this.f_Addr, "f_Addr");
            this.f_Addr.Name = "f_Addr";
            this.f_Addr.ReadOnly = true;
            // 
            // f_ReaderID
            // 
            resources.ApplyResources(this.f_ReaderID, "f_ReaderID");
            this.f_ReaderID.Name = "f_ReaderID";
            this.f_ReaderID.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pTools_first_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMealSetup,
            this.btnCreateReport,
            this.btnPrint,
            this.btnExportToExcel});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnMealSetup
            // 
            this.btnMealSetup.ForeColor = System.Drawing.Color.White;
            this.btnMealSetup.Image = global::Properties.Resources.icon_meal_setup;
            resources.ApplyResources(this.btnMealSetup, "btnMealSetup");
            this.btnMealSetup.Name = "btnMealSetup";
            this.btnMealSetup.Click += new System.EventHandler(this.btnMealSetup_Click);
            // 
            // btnCreateReport
            // 
            this.btnCreateReport.ForeColor = System.Drawing.Color.White;
            this.btnCreateReport.Image = global::Properties.Resources.icon_create;
            resources.ApplyResources(this.btnCreateReport, "btnCreateReport");
            this.btnCreateReport.Name = "btnCreateReport";
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.btnPrint.Image = global::Properties.Resources.icon_print;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.btnExportToExcel.Image = global::Properties.Resources.icon_export_excel;
            resources.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // userControlFind1
            // 
            resources.ApplyResources(this.userControlFind1, "userControlFind1");
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.ForeColor = System.Drawing.Color.White;
            this.userControlFind1.Name = "userControlFind1";
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
            this.tabPage1.Controls.Add(this.dgvSwipeRecords);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.tabPage2.Controls.Add(this.dgvSubtotal);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvSubtotal
            // 
            this.dgvSubtotal.AllowUserToAddRows = false;
            this.dgvSubtotal.AllowUserToDeleteRows = false;
            this.dgvSubtotal.AllowUserToOrderColumns = true;
            this.dgvSubtotal.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSubtotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSubtotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSubtotal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ReaderID2,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            resources.ApplyResources(this.dgvSubtotal, "dgvSubtotal");
            this.dgvSubtotal.EnableHeadersVisualStyles = false;
            this.dgvSubtotal.Name = "dgvSubtotal";
            this.dgvSubtotal.ReadOnly = true;
            this.dgvSubtotal.RowHeadersVisible = false;
            this.dgvSubtotal.RowTemplate.Height = 23;
            this.dgvSubtotal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // f_ReaderID2
            // 
            resources.ApplyResources(this.f_ReaderID2, "f_ReaderID2");
            this.f_ReaderID2.Name = "f_ReaderID2";
            this.f_ReaderID2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.tabPage3.Controls.Add(this.dgvStatistics);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvStatistics
            // 
            this.dgvStatistics.AllowUserToAddRows = false;
            this.dgvStatistics.AllowUserToDeleteRows = false;
            this.dgvStatistics.AllowUserToOrderColumns = true;
            this.dgvStatistics.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStatistics.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.f_DepartmentName2,
            this.f_ConsumerNO2,
            this.dataGridViewTextBoxColumn6,
            this.Morning,
            this.Lunch,
            this.Evening,
            this.Other,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11});
            resources.ApplyResources(this.dgvStatistics, "dgvStatistics");
            this.dgvStatistics.EnableHeadersVisualStyles = false;
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.ReadOnly = true;
            this.dgvStatistics.RowHeadersVisible = false;
            this.dgvStatistics.RowTemplate.Height = 23;
            this.dgvStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // f_DepartmentName2
            // 
            resources.ApplyResources(this.f_DepartmentName2, "f_DepartmentName2");
            this.f_DepartmentName2.Name = "f_DepartmentName2";
            this.f_DepartmentName2.ReadOnly = true;
            // 
            // f_ConsumerNO2
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ConsumerNO2.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.f_ConsumerNO2, "f_ConsumerNO2");
            this.f_ConsumerNO2.Name = "f_ConsumerNO2";
            this.f_ConsumerNO2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // Morning
            // 
            resources.ApplyResources(this.Morning, "Morning");
            this.Morning.Name = "Morning";
            this.Morning.ReadOnly = true;
            // 
            // Lunch
            // 
            resources.ApplyResources(this.Lunch, "Lunch");
            this.Lunch.Name = "Lunch";
            this.Lunch.ReadOnly = true;
            // 
            // Evening
            // 
            resources.ApplyResources(this.Evening, "Evening");
            this.Evening.Name = "Evening";
            this.Evening.ReadOnly = true;
            // 
            // Other
            // 
            resources.ApplyResources(this.Other, "Other");
            this.Other.Name = "Other";
            this.Other.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // ProgressBar1
            // 
            resources.ApplyResources(this.ProgressBar1, "ProgressBar1");
            this.ProgressBar1.Name = "ProgressBar1";
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip3.BackgroundImage = global::Properties.Resources.pTools_second_title;
            resources.ApplyResources(this.toolStrip3, "toolStrip3");
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripLabel3});
            this.toolStrip3.Name = "toolStrip3";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // frmMeal
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.icon_meal_setup;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmMeal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSwipeRecords_FormClosing);
            this.Load += new System.EventHandler(this.frmSwipeRecords_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSwipeRecords_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSwipeRecords)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubtotal)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnCreateReport;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnMealSetup;
        private ToolStripButton btnPrint;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridView dgvStatistics;
        private DataGridView dgvSubtotal;
        private DataGridView dgvSwipeRecords;
        private ToolStripDateTime dtpDateFrom;
        private ToolStripDateTime dtpDateTo;
        private DataGridViewTextBoxColumn Evening;
        private DataGridViewTextBoxColumn f_Addr;
        private DataGridViewTextBoxColumn f_ConsumerID;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_ConsumerNO2;
        private DataGridViewTextBoxColumn f_Cost;
        private DataGridViewTextBoxColumn f_DepartmentName;
        private DataGridViewTextBoxColumn f_DepartmentName2;
        private DataGridViewTextBoxColumn f_ReadDate;
        private DataGridViewTextBoxColumn f_ReaderID;
        private DataGridViewTextBoxColumn f_ReaderID2;
        private DataGridViewTextBoxColumn f_RecID;
        private DataGridViewTextBoxColumn Lunch;
        private DataGridViewTextBoxColumn MealName;
        private DataGridViewTextBoxColumn Morning;
        private DataGridViewTextBoxColumn Other;
        private ProgressBar ProgressBar1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private System.Windows.Forms.Timer timer1;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip3;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
    }
}

