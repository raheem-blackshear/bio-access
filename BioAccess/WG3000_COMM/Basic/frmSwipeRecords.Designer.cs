namespace WG3000_COMM.Basic
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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmSwipeRecords
    {
        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnDelete;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnFindOption;
        private ToolStripButton btnPrint;
        private ToolStripComboBox cboEnd;
        private ToolStripComboBox cboStart;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private dfrmSwipeRecordsFindOption dfrmFindOption;
        private DataGridView dgvSwipeRecords;
        private ToolStripDateTime dtpDateFrom;
        private ToolStripDateTime dtpDateTo;
        private ToolStripDateTime dtpTimeFrom;
        private ToolStripDateTime dtpTimeTo;
        private DataView dvFloor;
        private DataGridViewTextBoxColumn f_CardNO;
        private ToolStripMenuItem loadAllToolStripMenuItem;
        private ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;
        private ToolStripMenuItem saveLayoutToolStripMenuItem;
        private DataTable table;
        private System.Windows.Forms.Timer timer1;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip3;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
        private ToolStripLabel toolStripLabel4;
        private ToolStripLabel toolStripLabel5;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator1;
        private UserControlFind userControlFind1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSwipeRecords));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dgvSwipeRecords = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReadDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Pass = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_RecordAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.f_CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cboStart = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cboEnd = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.btnFindOption = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSwipeRecords)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.f_ConsumerNO,
            this.f_ConsumerName,
            this.f_DepartmentName,
            this.f_ReadDate,
            this.f_Addr,
            this.f_Pass,
            this.f_Desc,
            this.f_RecordAll});
            this.dgvSwipeRecords.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.dgvSwipeRecords, "dgvSwipeRecords");
            this.dgvSwipeRecords.EnableHeadersVisualStyles = false;
            this.dgvSwipeRecords.Name = "dgvSwipeRecords";
            this.dgvSwipeRecords.ReadOnly = true;
            this.dgvSwipeRecords.RowHeadersVisible = false;
            this.dgvSwipeRecords.RowTemplate.Height = 23;
            this.dgvSwipeRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSwipeRecords.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSwipeRecords_CellFormatting);
            this.dgvSwipeRecords.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSwipeRecords_Scroll);
            this.dgvSwipeRecords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSwipeRecords_MouseDoubleClick);
            // 
            // f_RecID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
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
            // f_DepartmentName
            // 
            resources.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
            this.f_DepartmentName.Name = "f_DepartmentName";
            this.f_DepartmentName.ReadOnly = true;
            // 
            // f_ReadDate
            // 
            resources.ApplyResources(this.f_ReadDate, "f_ReadDate");
            this.f_ReadDate.Name = "f_ReadDate";
            this.f_ReadDate.ReadOnly = true;
            // 
            // f_Addr
            // 
            resources.ApplyResources(this.f_Addr, "f_Addr");
            this.f_Addr.Name = "f_Addr";
            this.f_Addr.ReadOnly = true;
            // 
            // f_Pass
            // 
            resources.ApplyResources(this.f_Pass, "f_Pass");
            this.f_Pass.Name = "f_Pass";
            this.f_Pass.ReadOnly = true;
            this.f_Pass.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_Pass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_Desc
            // 
            this.f_Desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_Desc, "f_Desc");
            this.f_Desc.Name = "f_Desc";
            this.f_Desc.ReadOnly = true;
            // 
            // f_RecordAll
            // 
            resources.ApplyResources(this.f_RecordAll, "f_RecordAll");
            this.f_RecordAll.Name = "f_RecordAll";
            this.f_RecordAll.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.saveLayoutToolStripMenuItem,
            this.restoreDefaultLayoutToolStripMenuItem,
            this.loadAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
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
            // loadAllToolStripMenuItem
            // 
            this.loadAllToolStripMenuItem.Name = "loadAllToolStripMenuItem";
            resources.ApplyResources(this.loadAllToolStripMenuItem, "loadAllToolStripMenuItem");
            this.loadAllToolStripMenuItem.Click += new System.EventHandler(this.loadAllToolStripMenuItem_Click);
            // 
            // f_CardNO
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_CardNO.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_CardNO, "f_CardNO");
            this.f_CardNO.Name = "f_CardNO";
            this.f_CardNO.ReadOnly = true;
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip3.BackgroundImage = global::Properties.Resources.pTools_second_title;
            resources.ApplyResources(this.toolStrip3, "toolStrip3");
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.cboStart,
            this.toolStripLabel3,
            this.cboEnd,
            this.toolStripSeparator1,
            this.toolStripLabel4,
            this.toolStripLabel5});
            this.toolStrip3.Name = "toolStrip3";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // cboStart
            // 
            this.cboStart.Items.AddRange(new object[] {
            resources.GetString("cboStart.Items"),
            resources.GetString("cboStart.Items1")});
            this.cboStart.Name = "cboStart";
            resources.ApplyResources(this.cboStart, "cboStart");
            this.cboStart.SelectedIndexChanged += new System.EventHandler(this.cboStart_SelectedIndexChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // cboEnd
            // 
            this.cboEnd.Items.AddRange(new object[] {
            resources.GetString("cboEnd.Items"),
            resources.GetString("cboEnd.Items1")});
            this.cboEnd.Name = "cboEnd";
            resources.ApplyResources(this.cboEnd, "cboEnd");
            this.cboEnd.SelectedIndexChanged += new System.EventHandler(this.cboEnd_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.toolStripLabel4.Name = "toolStripLabel4";
            resources.ApplyResources(this.toolStripLabel4, "toolStripLabel4");
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.toolStripLabel5.Name = "toolStripLabel5";
            resources.ApplyResources(this.toolStripLabel5, "toolStripLabel5");
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pTools_first_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrint,
            this.btnExportToExcel,
            this.btnFindOption,
            this.btnDelete});
            this.toolStrip1.Name = "toolStrip1";
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
            // btnFindOption
            // 
            this.btnFindOption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.btnFindOption.Image = global::Properties.Resources.icon_query_option;
            resources.ApplyResources(this.btnFindOption, "btnFindOption");
            this.btnFindOption.Name = "btnFindOption";
            this.btnFindOption.Click += new System.EventHandler(this.btnFindOption_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = global::Properties.Resources.icon_delete;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // userControlFind1
            // 
            resources.ApplyResources(this.userControlFind1, "userControlFind1");
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.ForeColor = System.Drawing.Color.White;
            this.userControlFind1.Name = "userControlFind1";
            // 
            // frmSwipeRecords
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvSwipeRecords);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmSwipeRecords";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSwipeRecords_FormClosing);
            this.Load += new System.EventHandler(this.frmSwipeRecords_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSwipeRecords_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSwipeRecords)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private DataGridViewTextBoxColumn f_RecID;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_DepartmentName;
        private DataGridViewTextBoxColumn f_ReadDate;
        private DataGridViewTextBoxColumn f_Addr;
        private DataGridViewCheckBoxColumn f_Pass;
        private DataGridViewTextBoxColumn f_Desc;
        private DataGridViewTextBoxColumn f_RecordAll;
    }
}

