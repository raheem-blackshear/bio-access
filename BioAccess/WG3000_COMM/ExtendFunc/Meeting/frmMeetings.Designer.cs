namespace WG3000_COMM.ExtendFunc.Meeting
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmMeetings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeetings));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.MeetingNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeetingName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeetingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddress = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.btnStat = new System.Windows.Forms.ToolStripButton();
            this.btnRealtimeSign = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToOrderColumns = true;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MeetingNO,
            this.MeetingName,
            this.MeetingTime,
            this.Addr,
            this.Content,
            this.Notes});
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.MultiSelect = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.DoubleClick += new System.EventHandler(this.dgvMain_DoubleClick);
            // 
            // MeetingNO
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.MeetingNO.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.MeetingNO, "MeetingNO");
            this.MeetingNO.Name = "MeetingNO";
            this.MeetingNO.ReadOnly = true;
            // 
            // MeetingName
            // 
            resources.ApplyResources(this.MeetingName, "MeetingName");
            this.MeetingName.Name = "MeetingName";
            this.MeetingName.ReadOnly = true;
            // 
            // MeetingTime
            // 
            resources.ApplyResources(this.MeetingTime, "MeetingTime");
            this.MeetingTime.Name = "MeetingTime";
            this.MeetingTime.ReadOnly = true;
            // 
            // Addr
            // 
            resources.ApplyResources(this.Addr, "Addr");
            this.Addr.Name = "Addr";
            this.Addr.ReadOnly = true;
            // 
            // Content
            // 
            this.Content.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Content, "Content");
            this.Content.Name = "Content";
            this.Content.ReadOnly = true;
            // 
            // Notes
            // 
            this.Notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Notes, "Notes");
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddress,
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.btnPrint,
            this.btnExport,
            this.btnStat,
            this.btnRealtimeSign});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddress
            // 
            this.btnAddress.ForeColor = System.Drawing.Color.White;
            this.btnAddress.Image = global::Properties.Resources.icon_meeting_addr_setup;
            resources.ApplyResources(this.btnAddress, "btnAddress");
            this.btnAddress.Name = "btnAddress";
            this.btnAddress.Click += new System.EventHandler(this.btnAddress_Click);
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
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
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
            // btnExport
            // 
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Image = global::Properties.Resources.icon_export_excel;
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnStat
            // 
            this.btnStat.ForeColor = System.Drawing.Color.White;
            this.btnStat.Image = global::Properties.Resources.icon_meeting_start;
            resources.ApplyResources(this.btnStat, "btnStat");
            this.btnStat.Name = "btnStat";
            this.btnStat.Click += new System.EventHandler(this.btnStat_Click);
            // 
            // btnRealtimeSign
            // 
            this.btnRealtimeSign.ForeColor = System.Drawing.Color.White;
            this.btnRealtimeSign.Image = global::Properties.Resources.icon_meeting_realtime_sign;
            resources.ApplyResources(this.btnRealtimeSign, "btnRealtimeSign");
            this.btnRealtimeSign.Name = "btnRealtimeSign";
            this.btnRealtimeSign.Click += new System.EventHandler(this.btnRealtimeSign_Click);
            // 
            // frmMeetings
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmMeetings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMeetings_FormClosing);
            this.Load += new System.EventHandler(this.frmMeetings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMeetings_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewTextBoxColumn Addr;
        private ToolStripButton btnAdd;
        private ToolStripButton btnAddress;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnExport;
        private ToolStripButton btnPrint;
        private ToolStripButton btnRealtimeSign;
        private ToolStripButton btnStat;
        private DataGridViewTextBoxColumn Content;
        private DataGridView dgvMain;
        private DataGridViewTextBoxColumn MeetingName;
        private DataGridViewTextBoxColumn MeetingNO;
        private DataGridViewTextBoxColumn MeetingTime;
        private DataGridViewTextBoxColumn Notes;
        private ToolStrip toolStrip1;
    }
}

