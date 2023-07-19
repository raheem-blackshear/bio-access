namespace WG3000_COMM.ExtendFunc.Meeting
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMeetingStatDetail
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
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmMeetingStatDetail));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.btnLeave = new System.Windows.Forms.ImageButton();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnManualSign = new System.Windows.Forms.ImageButton();
            this.btnRecreate = new System.Windows.Forms.ImageButton();
            this.btnPrint = new System.Windows.Forms.ImageButton();
            this.btnExport = new System.Windows.Forms.ImageButton();
            this.btnRefresh = new System.Windows.Forms.ImageButton();
            this.grpExt = new System.Windows.Forms.GroupBox();
            this.dgvStat = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InFact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_ManualCardRecordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Identity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SeatNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SignRealTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.grpExt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            resources.ApplyResources(this.tabPage5, "tabPage5");
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            resources.ApplyResources(this.tabPage6, "tabPage6");
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // btnLeave
            // 
            this.btnLeave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnLeave, "btnLeave");
            this.btnLeave.Focusable = true;
            this.btnLeave.ForeColor = System.Drawing.Color.White;
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Toggle = false;
            this.btnLeave.UseVisualStyleBackColor = false;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.Toggle = false;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnManualSign
            // 
            this.btnManualSign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnManualSign, "btnManualSign");
            this.btnManualSign.Focusable = true;
            this.btnManualSign.ForeColor = System.Drawing.Color.White;
            this.btnManualSign.Name = "btnManualSign";
            this.btnManualSign.Toggle = false;
            this.btnManualSign.UseVisualStyleBackColor = false;
            this.btnManualSign.Click += new System.EventHandler(this.btnManualSign_Click);
            // 
            // btnRecreate
            // 
            this.btnRecreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRecreate, "btnRecreate");
            this.btnRecreate.Focusable = true;
            this.btnRecreate.ForeColor = System.Drawing.Color.White;
            this.btnRecreate.Name = "btnRecreate";
            this.btnRecreate.Toggle = false;
            this.btnRecreate.UseVisualStyleBackColor = false;
            this.btnRecreate.Click += new System.EventHandler(this.btnRecreate_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Focusable = true;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Toggle = false;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Focusable = true;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.Toggle = false;
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Focusable = true;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Toggle = false;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grpExt
            // 
            resources.ApplyResources(this.grpExt, "grpExt");
            this.grpExt.BackColor = System.Drawing.Color.Transparent;
            this.grpExt.Controls.Add(this.dgvStat);
            this.grpExt.Controls.Add(this.dgvMain);
            this.grpExt.ForeColor = System.Drawing.Color.White;
            this.grpExt.Name = "grpExt";
            this.grpExt.TabStop = false;
            // 
            // dgvStat
            // 
            this.dgvStat.AllowUserToAddRows = false;
            this.dgvStat.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvStat, "dgvStat");
            this.dgvStat.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.InFact,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgvStat.EnableHeadersVisualStyles = false;
            this.dgvStat.Name = "dgvStat";
            this.dgvStat.ReadOnly = true;
            this.dgvStat.RowHeadersVisible = false;
            this.dgvStat.RowTemplate.Height = 23;
            this.dgvStat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            // InFact
            // 
            resources.ApplyResources(this.InFact, "InFact");
            this.InFact.Name = "InFact";
            this.InFact.ReadOnly = true;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ManualCardRecordID,
            this.f_ConsumerName,
            this.f_Identity,
            this.f_SeatNO,
            this.f_SignRealTime,
            this.f_Notes});
            this.dgvMain.EnableHeadersVisualStyles = false;
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // f_ManualCardRecordID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ManualCardRecordID.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_ManualCardRecordID, "f_ManualCardRecordID");
            this.f_ManualCardRecordID.Name = "f_ManualCardRecordID";
            this.f_ManualCardRecordID.ReadOnly = true;
            // 
            // f_ConsumerName
            // 
            resources.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
            this.f_ConsumerName.Name = "f_ConsumerName";
            this.f_ConsumerName.ReadOnly = true;
            // 
            // f_Identity
            // 
            resources.ApplyResources(this.f_Identity, "f_Identity");
            this.f_Identity.Name = "f_Identity";
            this.f_Identity.ReadOnly = true;
            // 
            // f_SeatNO
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_SeatNO.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_SeatNO, "f_SeatNO");
            this.f_SeatNO.Name = "f_SeatNO";
            this.f_SeatNO.ReadOnly = true;
            // 
            // f_SignRealTime
            // 
            resources.ApplyResources(this.f_SignRealTime, "f_SignRealTime");
            this.f_SignRealTime.Name = "f_SignRealTime";
            this.f_SignRealTime.ReadOnly = true;
            // 
            // f_Notes
            // 
            resources.ApplyResources(this.f_Notes, "f_Notes");
            this.f_Notes.Name = "f_Notes";
            this.f_Notes.ReadOnly = true;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnExit);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmMeetingStatDetail
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnManualSign);
            this.Controls.Add(this.btnRecreate);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.grpExt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmMeetingStatDetail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmMeetingStatDetail_FormClosing);
            this.Load += new System.EventHandler(this.dfrmStd_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmMeetingStatDetail_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.grpExt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal ImageButton btnExit;
        internal ImageButton btnExport;
        internal ImageButton btnLeave;
        internal ImageButton btnManualSign;
        internal ImageButton btnPrint;
        internal ImageButton btnRecreate;
        internal ImageButton btnRefresh;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView dgvMain;
        private DataGridView dgvStat;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_Identity;
        private DataGridViewTextBoxColumn f_ManualCardRecordID;
        private DataGridViewTextBoxColumn f_Notes;
        private DataGridViewTextBoxColumn f_SeatNO;
        private DataGridViewTextBoxColumn f_SignRealTime;
        private GroupBox grpExt;
        private DataGridViewTextBoxColumn InFact;
        private DateTime signEndtime;
        private DateTime signStarttime;
        public TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private Panel panelBottomBanner;
    }
}

