namespace WG3000_COMM.Reports.Shift
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
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmShiftAttStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShiftAttStatistics));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DayShouldWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DayRealWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_LateMinutes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_LateCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_LeaveEarlyMinutes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_LeaveEarlyCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OvertimeHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_AbsenceDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_NotReadCardCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ManualReadTimesCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdQueryNormalShift = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdQueryOtherShift = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.lblLog = new System.Windows.Forms.ToolStripLabel();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFindSecond();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToOrderColumns = true;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_RecID,
            this.f_DepartmentName,
            this.f_ConsumerNO,
            this.f_ConsumerName,
            this.f_DayShouldWork,
            this.f_DayRealWork,
            this.f_LateMinutes,
            this.f_LateCount,
            this.f_LeaveEarlyMinutes,
            this.f_LeaveEarlyCount,
            this.f_OvertimeHours,
            this.f_AbsenceDays,
            this.f_NotReadCardCount,
            this.f_ManualReadTimesCount});
            this.dgvMain.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
            // 
            // f_RecID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
            // 
            // f_DepartmentName
            // 
            resources.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
            this.f_DepartmentName.Name = "f_DepartmentName";
            this.f_DepartmentName.ReadOnly = true;
            // 
            // f_ConsumerNO
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle3;
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
            // f_DayShouldWork
            // 
            dataGridViewCellStyle4.NullValue = null;
            this.f_DayShouldWork.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_DayShouldWork, "f_DayShouldWork");
            this.f_DayShouldWork.Name = "f_DayShouldWork";
            this.f_DayShouldWork.ReadOnly = true;
            // 
            // f_DayRealWork
            // 
            resources.ApplyResources(this.f_DayRealWork, "f_DayRealWork");
            this.f_DayRealWork.Name = "f_DayRealWork";
            this.f_DayRealWork.ReadOnly = true;
            // 
            // f_LateMinutes
            // 
            resources.ApplyResources(this.f_LateMinutes, "f_LateMinutes");
            this.f_LateMinutes.Name = "f_LateMinutes";
            this.f_LateMinutes.ReadOnly = true;
            // 
            // f_LateCount
            // 
            resources.ApplyResources(this.f_LateCount, "f_LateCount");
            this.f_LateCount.Name = "f_LateCount";
            this.f_LateCount.ReadOnly = true;
            // 
            // f_LeaveEarlyMinutes
            // 
            resources.ApplyResources(this.f_LeaveEarlyMinutes, "f_LeaveEarlyMinutes");
            this.f_LeaveEarlyMinutes.Name = "f_LeaveEarlyMinutes";
            this.f_LeaveEarlyMinutes.ReadOnly = true;
            // 
            // f_LeaveEarlyCount
            // 
            resources.ApplyResources(this.f_LeaveEarlyCount, "f_LeaveEarlyCount");
            this.f_LeaveEarlyCount.Name = "f_LeaveEarlyCount";
            this.f_LeaveEarlyCount.ReadOnly = true;
            // 
            // f_OvertimeHours
            // 
            resources.ApplyResources(this.f_OvertimeHours, "f_OvertimeHours");
            this.f_OvertimeHours.Name = "f_OvertimeHours";
            this.f_OvertimeHours.ReadOnly = true;
            // 
            // f_AbsenceDays
            // 
            resources.ApplyResources(this.f_AbsenceDays, "f_AbsenceDays");
            this.f_AbsenceDays.Name = "f_AbsenceDays";
            this.f_AbsenceDays.ReadOnly = true;
            // 
            // f_NotReadCardCount
            // 
            resources.ApplyResources(this.f_NotReadCardCount, "f_NotReadCardCount");
            this.f_NotReadCardCount.Name = "f_NotReadCardCount";
            this.f_NotReadCardCount.ReadOnly = true;
            // 
            // f_ManualReadTimesCount
            // 
            resources.ApplyResources(this.f_ManualReadTimesCount, "f_ManualReadTimesCount");
            this.f_ManualReadTimesCount.Name = "f_ManualReadTimesCount";
            this.f_ManualReadTimesCount.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem,
            this.restoreDefaultLayoutToolStripMenuItem,
            this.cmdQueryNormalShift,
            this.cmdQueryOtherShift,
            this.displayAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            resources.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // restoreDefaultLayoutToolStripMenuItem
            // 
            this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
            resources.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
            this.restoreDefaultLayoutToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click);
            // 
            // cmdQueryNormalShift
            // 
            this.cmdQueryNormalShift.Name = "cmdQueryNormalShift";
            resources.ApplyResources(this.cmdQueryNormalShift, "cmdQueryNormalShift");
            this.cmdQueryNormalShift.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // cmdQueryOtherShift
            // 
            this.cmdQueryOtherShift.Name = "cmdQueryOtherShift";
            resources.ApplyResources(this.cmdQueryOtherShift, "cmdQueryOtherShift");
            this.cmdQueryOtherShift.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // displayAllToolStripMenuItem
            // 
            this.displayAllToolStripMenuItem.Name = "displayAllToolStripMenuItem";
            resources.ApplyResources(this.displayAllToolStripMenuItem, "displayAllToolStripMenuItem");
            this.displayAllToolStripMenuItem.Click += new System.EventHandler(this.displayAllToolStripMenuItem_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.BackgroundImage = global::Properties.Resources.pTools_third_title;
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLog});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // lblLog
            // 
            this.lblLog.ForeColor = System.Drawing.Color.White;
            this.lblLog.Name = "lblLog";
            resources.ApplyResources(this.lblLog, "lblLog");
            // 
            // userControlFind1
            // 
            resources.ApplyResources(this.userControlFind1, "userControlFind1");
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.ForeColor = System.Drawing.Color.White;
            this.userControlFind1.Name = "userControlFind1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrint,
            this.btnExportToExcel,
            this.btnExit});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::Properties.Resources.icon_print;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportToExcel.Image = global::Properties.Resources.icon_export_excel;
            resources.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Image = global::Properties.Resources.icon_close;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmShiftAttStatistics
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmShiftAttStatistics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
            this.Load += new System.EventHandler(this.frmShiftAttStatistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnExit;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnPrint;
        private ToolStripMenuItem cmdQueryNormalShift;
        private ToolStripMenuItem cmdQueryOtherShift;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridViewTextBoxColumn dc;
        private DataGridView dgvMain;
        private ToolStripMenuItem displayAllToolStripMenuItem;
        private DataGridViewTextBoxColumn f_AbsenceDays;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_DayRealWork;
        private DataGridViewTextBoxColumn f_DayShouldWork;
        private DataGridViewTextBoxColumn f_DepartmentName;
        private DataGridViewTextBoxColumn f_LateCount;
        private DataGridViewTextBoxColumn f_LateMinutes;
        private DataGridViewTextBoxColumn f_LeaveEarlyCount;
        private DataGridViewTextBoxColumn f_LeaveEarlyMinutes;
        private DataGridViewTextBoxColumn f_ManualReadTimesCount;
        private DataGridViewTextBoxColumn f_NotReadCardCount;
        private DataGridViewTextBoxColumn f_OvertimeHours;
        private DataGridViewTextBoxColumn f_RecID;
        private ToolStripLabel lblLog;
        private DateTime logDateEnd;
        private DateTime logDateStart;
        private ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;
        private ToolStripMenuItem saveLayoutToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
    }
}

