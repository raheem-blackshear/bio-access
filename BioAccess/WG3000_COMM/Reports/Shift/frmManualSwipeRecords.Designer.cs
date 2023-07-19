﻿namespace WG3000_COMM.Reports.Shift
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
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmManualSwipeRecords
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManualSwipeRecords));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_ManualCardRecordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReadDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
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
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ManualCardRecordID,
            this.f_DepartmentName,
            this.f_ConsumerNO,
            this.f_ConsumerName,
            this.f_ReadDate,
            this.f_Notes});
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMain_CellFormatting);
            this.dgvMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
            // 
            // f_ManualCardRecordID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ManualCardRecordID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_ManualCardRecordID, "f_ManualCardRecordID");
            this.f_ManualCardRecordID.Name = "f_ManualCardRecordID";
            this.f_ManualCardRecordID.ReadOnly = true;
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
            // f_ReadDate
            // 
            resources.ApplyResources(this.f_ReadDate, "f_ReadDate");
            this.f_ReadDate.Name = "f_ReadDate";
            this.f_ReadDate.ReadOnly = true;
            // 
            // f_Notes
            // 
            resources.ApplyResources(this.f_Notes, "f_Notes");
            this.f_Notes.Name = "f_Notes";
            this.f_Notes.ReadOnly = true;
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
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.btnPrint,
            this.btnExportToExcel});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_new;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Image = global::Properties.Resources.icon_edit;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = global::Properties.Resources.icon_delete;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            // frmManualSwipeRecords
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmManualSwipeRecords";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
            this.Load += new System.EventHandler(this.frmSwipeRecords_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnPrint;
        private DataGridView dgvMain;
        private ToolStripDateTime dtpDateFrom;
        private ToolStripDateTime dtpDateTo;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_DepartmentName;
        private DataGridViewTextBoxColumn f_ManualCardRecordID;
        private DataGridViewTextBoxColumn f_Notes;
        private DataGridViewTextBoxColumn f_ReadDate;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip3;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
    }
}

