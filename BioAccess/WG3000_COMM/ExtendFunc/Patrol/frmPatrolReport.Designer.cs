﻿namespace WG3000_COMM.ExtendFunc.Patrol
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
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmPatrolReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatrolReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnSelectColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdCreateWithSomeConsumer = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftDateShort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OnDuty1Short = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OffDuty1Short = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Desc1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Desc2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Desc3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.lblLog = new System.Windows.Forms.ToolStripLabel();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPatrolSetup = new System.Windows.Forms.ToolStripButton();
            this.btnPatrolRoute = new System.Windows.Forms.ToolStripButton();
            this.btnPatrolTask = new System.Windows.Forms.ToolStripButton();
            this.btnCreateReport = new System.Windows.Forms.ToolStripButton();
            this.btnStatistics = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.btnFindOption = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectColumns,
            this.saveLayoutToolStripMenuItem,
            this.restoreDefaultLayoutToolStripMenuItem,
            this.cmdCreateWithSomeConsumer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // btnSelectColumns
            // 
            this.btnSelectColumns.Name = "btnSelectColumns";
            resources.ApplyResources(this.btnSelectColumns, "btnSelectColumns");
            this.btnSelectColumns.Click += new System.EventHandler(this.btnSelectColumns_Click);
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
            // cmdCreateWithSomeConsumer
            // 
            this.cmdCreateWithSomeConsumer.Name = "cmdCreateWithSomeConsumer";
            resources.ApplyResources(this.cmdCreateWithSomeConsumer, "cmdCreateWithSomeConsumer");
            this.cmdCreateWithSomeConsumer.Click += new System.EventHandler(this.cmdCreateWithSomeConsumer_Click);
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
            this.f_ShiftDateShort,
            this.f_OnDuty1Short,
            this.f_OffDuty1Short,
            this.f_Desc1,
            this.f_Desc2,
            this.f_Desc3});
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
            // f_ShiftDateShort
            // 
            resources.ApplyResources(this.f_ShiftDateShort, "f_ShiftDateShort");
            this.f_ShiftDateShort.Name = "f_ShiftDateShort";
            this.f_ShiftDateShort.ReadOnly = true;
            // 
            // f_OnDuty1Short
            // 
            resources.ApplyResources(this.f_OnDuty1Short, "f_OnDuty1Short");
            this.f_OnDuty1Short.Name = "f_OnDuty1Short";
            this.f_OnDuty1Short.ReadOnly = true;
            // 
            // f_OffDuty1Short
            // 
            resources.ApplyResources(this.f_OffDuty1Short, "f_OffDuty1Short");
            this.f_OffDuty1Short.Name = "f_OffDuty1Short";
            this.f_OffDuty1Short.ReadOnly = true;
            // 
            // f_Desc1
            // 
            resources.ApplyResources(this.f_Desc1, "f_Desc1");
            this.f_Desc1.Name = "f_Desc1";
            this.f_Desc1.ReadOnly = true;
            // 
            // f_Desc2
            // 
            resources.ApplyResources(this.f_Desc2, "f_Desc2");
            this.f_Desc2.Name = "f_Desc2";
            this.f_Desc2.ReadOnly = true;
            // 
            // f_Desc3
            // 
            this.f_Desc3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_Desc3, "f_Desc3");
            this.f_Desc3.Name = "f_Desc3";
            this.f_Desc3.ReadOnly = true;
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
            this.userControlFind1.Name = "userControlFind1";
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
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPatrolSetup,
            this.btnPatrolRoute,
            this.btnPatrolTask,
            this.btnCreateReport,
            this.btnStatistics,
            this.btnPrint,
            this.btnExportToExcel,
            this.btnFindOption});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnPatrolSetup
            // 
            this.btnPatrolSetup.ForeColor = System.Drawing.Color.White;
            this.btnPatrolSetup.Image = global::Properties.Resources.icon_patrol;
            resources.ApplyResources(this.btnPatrolSetup, "btnPatrolSetup");
            this.btnPatrolSetup.Name = "btnPatrolSetup";
            this.btnPatrolSetup.Click += new System.EventHandler(this.btnPatrolSetup_Click);
            // 
            // btnPatrolRoute
            // 
            this.btnPatrolRoute.ForeColor = System.Drawing.Color.White;
            this.btnPatrolRoute.Image = global::Properties.Resources.icon_patrol_route;
            resources.ApplyResources(this.btnPatrolRoute, "btnPatrolRoute");
            this.btnPatrolRoute.Name = "btnPatrolRoute";
            this.btnPatrolRoute.Click += new System.EventHandler(this.btnPatrolRoute_Click);
            // 
            // btnPatrolTask
            // 
            this.btnPatrolTask.ForeColor = System.Drawing.Color.White;
            this.btnPatrolTask.Image = global::Properties.Resources.icon_task;
            resources.ApplyResources(this.btnPatrolTask, "btnPatrolTask");
            this.btnPatrolTask.Name = "btnPatrolTask";
            this.btnPatrolTask.Click += new System.EventHandler(this.btnPatrolTask_Click);
            // 
            // btnCreateReport
            // 
            this.btnCreateReport.ForeColor = System.Drawing.Color.White;
            this.btnCreateReport.Image = global::Properties.Resources.icon_create;
            resources.ApplyResources(this.btnCreateReport, "btnCreateReport");
            this.btnCreateReport.Name = "btnCreateReport";
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.ForeColor = System.Drawing.Color.White;
            this.btnStatistics.Image = global::Properties.Resources.icon_statistics;
            resources.ApplyResources(this.btnStatistics, "btnStatistics");
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
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
            // btnFindOption
            // 
            this.btnFindOption.ForeColor = System.Drawing.Color.White;
            this.btnFindOption.Image = global::Properties.Resources.icon_query_option;
            resources.ApplyResources(this.btnFindOption, "btnFindOption");
            this.btnFindOption.Name = "btnFindOption";
            this.btnFindOption.Click += new System.EventHandler(this.btnFindOption_Click);
            // 
            // frmPatrolReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmPatrolReport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
            this.Load += new System.EventHandler(this.frmSwipeRecords_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnCreateReport;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnFindOption;
        private ToolStripButton btnPatrolRoute;
        private ToolStripButton btnPatrolSetup;
        private ToolStripButton btnPatrolTask;
        private ToolStripButton btnPrint;
        private ToolStripMenuItem btnSelectColumns;
        private ToolStripButton btnStatistics;
        private ToolStripMenuItem cmdCreateWithSomeConsumer;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridView dgvMain;
        private ToolStripDateTime dtpDateFrom;
        private ToolStripDateTime dtpDateTo;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_DepartmentName;
        private DataGridViewTextBoxColumn f_Desc1;
        private DataGridViewTextBoxColumn f_Desc2;
        private DataGridViewTextBoxColumn f_Desc3;
        private DataGridViewTextBoxColumn f_OffDuty1Short;
        private DataGridViewTextBoxColumn f_OnDuty1Short;
        private DataGridViewTextBoxColumn f_RecID;
        private DataGridViewTextBoxColumn f_ShiftDateShort;
        private ToolStripLabel lblLog;
        private DateTime logDateEnd;
        private DateTime logDateStart;
        private ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;
        private ToolStripMenuItem saveLayoutToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStrip toolStrip3;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
    }
}

