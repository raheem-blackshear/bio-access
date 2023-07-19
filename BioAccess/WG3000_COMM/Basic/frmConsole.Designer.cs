namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Media;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ExtendFunc;
    using WG3000_COMM.ExtendFunc.Map;
    using WG3000_COMM.ExtendFunc.PCCheck;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmConsole
    {
        private ToolStripButton btnCheck;
        private ToolStripButton btnClearRunInfo;
        private ToolStripButton btnEventLogError;
        private ToolStripButton btnEventLogInfo;
        private ToolStripButton btnEventLogWarn;
        private ToolStripButton btnGetRecords;
        private Button btnHideTools;
        private ToolStripButton btnMaps;
        private ToolStripButton btnRealtimeGetRecords;
        private ToolStripButton btnRemoteOpen;
        private ToolStripButton btnSelectAll;
        private ToolStripButton btnServer;
        private ToolStripButton btnSetTime;
        private ToolStripButton btnStopMonitor;
        private ToolStripButton btnStopOthers;
        private ToolStripButton btnUpload;
        private ToolStripButton btnWarnExisted;
        private ToolStripComboBox cboZone;
        private CheckBox chkDisplayNewestSwipe;
        private CheckBox chkNeedCheckLosePacket;
        private ToolStripMenuItem clearRunInfoToolStripMenuItem;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1Doors;
        private ContextMenuStrip contextMenuStrip2RunInfo;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private dfrmFind dfrmFind1;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private DataGridView dgvRunInfo;
        private ToolStripMenuItem displayMoreSwipesToolStripMenuItem;
        private DataTable dt;
        private DateTime dtlstDoorViewChange = DateTime.Now;
        private DataTable dtReader;
        private DataView dv;
        private DataView dvDoors;
        private DataView dvDoors4Check;
        private DataView dvDoors4Watching;
        private DataGridViewImageColumn f_Category;
        private DataGridViewTextBoxColumn f_Desc;
        private DataGridViewTextBoxColumn f_Detail;
        private DataGridViewTextBoxColumn f_Info;
        private DataGridViewTextBoxColumn f_MjRecStr;
        private DataGridViewTextBoxColumn f_RecID;
        private DataGridViewTextBoxColumn f_Time;
        public dfrmPCCheckAccess frm4PCCheckAccess;
        private dfrmLocate frm4ShowLocate;
        private dfrmPersonsInside frm4ShowPersonsInside;
        private frmMaps frmMaps1;
        private frmWatchingMoreRecords frmMoreRecords;
        private GroupBox grpDetail;
        private GroupBox grpTool;
        private ImageList imgDoor2;
        private ListView listViewNotDisplay = new ListView();
        private ToolStripMenuItem locateToolStripMenuItem;
        public ListView lstDoors;
        private ToolStripMenuItem mnuCheck;
        private ToolStripMenuItem mnuWarnOutputReset;
        private ToolStripMenuItem personInsideToolStripMenuItem;
        private PictureBox pictureBox1;
        private SoundPlayer player;
        private ToolStripMenuItem resetPersonInsideToolStripMenuItem;
        private RichTextBox richTxtInfo;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private StepOfRealtimeGetReocrds stepOfRealtimeGetRecords;
        private DataTable tbRunInfoLog;
        private System.Windows.Forms.Timer timerUpdateDoorInfo;
        private System.Windows.Forms.Timer timerWarn;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolTip toolTip1;
        private TextBox txtInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.control4btnServer != null))
            {
                this.control4btnServer.Dispose();
            }
            if (disposing && (this.control4Check != null))
            {
                this.control4Check.Dispose();
            }
            if (disposing && (this.control4Realtime != null))
            {
                this.control4Realtime.Dispose();
            }
            if (disposing && (this.control4uploadPrivilege != null))
            {
                this.control4uploadPrivilege.Dispose();
            }
            if (disposing && (this.control4getRecordsFromController != null))
            {
                this.control4getRecordsFromController.Dispose();
            }
            if (disposing && (this.pr4uploadPrivilege != null))
            {
                this.pr4uploadPrivilege.Dispose();
            }
            if (disposing && (this.swipe4GetRecords != null))
            {
                this.swipe4GetRecords.Dispose();
            }
            if (disposing && (this.watching != null))
            {
                this.watching.Dispose();
            }
            if (disposing && (this.dfrmWait1 != null))
            {
                this.dfrmWait1.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsole));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpTool = new System.Windows.Forms.GroupBox();
            this.btnHideTools = new System.Windows.Forms.Button();
            this.chkDisplayNewestSwipe = new System.Windows.Forms.CheckBox();
            this.chkNeedCheckLosePacket = new System.Windows.Forms.CheckBox();
            this.lstDoors = new System.Windows.Forms.ListView();
            this.contextMenuStrip1Doors = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWarnOutputReset = new System.Windows.Forms.ToolStripMenuItem();
            this.locateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personInsideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetPersonInsideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvRunInfo = new System.Windows.Forms.DataGridView();
            this.f_Category = new System.Windows.Forms.DataGridViewImageColumn();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Info = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Detail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MjRecStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2RunInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearRunInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayMoreSwipesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTxtInfo = new System.Windows.Forms.RichTextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnEventLogInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEventLogWarn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEventLogError = new System.Windows.Forms.ToolStripButton();
            this.timerUpdateDoorInfo = new System.Windows.Forms.Timer(this.components);
            this.bkUploadAndGetRecords = new System.ComponentModel.BackgroundWorker();
            this.bkDispDoorStatus = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnWarnExisted = new System.Windows.Forms.ToolStripButton();
            this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.btnServer = new System.Windows.Forms.ToolStripButton();
            this.btnStopOthers = new System.Windows.Forms.ToolStripButton();
            this.btnCheck = new System.Windows.Forms.ToolStripButton();
            this.btnSetTime = new System.Windows.Forms.ToolStripButton();
            this.btnUpload = new System.Windows.Forms.ToolStripButton();
            this.btnGetRecords = new System.Windows.Forms.ToolStripButton();
            this.btnRealtimeGetRecords = new System.Windows.Forms.ToolStripButton();
            this.btnRemoteOpen = new System.Windows.Forms.ToolStripButton();
            this.btnClearRunInfo = new System.Windows.Forms.ToolStripButton();
            this.btnMaps = new System.Windows.Forms.ToolStripButton();
            this.cboZone = new System.Windows.Forms.ToolStripComboBox();
            this.btnStopMonitor = new System.Windows.Forms.ToolStripButton();
            this.timerWarn = new System.Windows.Forms.Timer(this.components);
            this.bkRealtimeGetRecords = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpTool.SuspendLayout();
            this.contextMenuStrip1Doors.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRunInfo)).BeginInit();
            this.contextMenuStrip2RunInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(231)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpTool);
            this.splitContainer1.Panel1.Controls.Add(this.lstDoors);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // grpTool
            // 
            this.grpTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.grpTool.Controls.Add(this.btnHideTools);
            this.grpTool.Controls.Add(this.chkDisplayNewestSwipe);
            this.grpTool.Controls.Add(this.chkNeedCheckLosePacket);
            this.grpTool.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpTool, "grpTool");
            this.grpTool.Name = "grpTool";
            this.grpTool.TabStop = false;
            // 
            // btnHideTools
            // 
            this.btnHideTools.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btnHideTools, "btnHideTools");
            this.btnHideTools.Name = "btnHideTools";
            this.btnHideTools.UseVisualStyleBackColor = true;
            this.btnHideTools.Click += new System.EventHandler(this.btnHideTools_Click);
            // 
            // chkDisplayNewestSwipe
            // 
            resources.ApplyResources(this.chkDisplayNewestSwipe, "chkDisplayNewestSwipe");
            this.chkDisplayNewestSwipe.Name = "chkDisplayNewestSwipe";
            this.chkDisplayNewestSwipe.UseVisualStyleBackColor = true;
            // 
            // chkNeedCheckLosePacket
            // 
            resources.ApplyResources(this.chkNeedCheckLosePacket, "chkNeedCheckLosePacket");
            this.chkNeedCheckLosePacket.Name = "chkNeedCheckLosePacket";
            this.chkNeedCheckLosePacket.UseVisualStyleBackColor = true;
            // 
            // lstDoors
            // 
            this.lstDoors.BackColor = System.Drawing.SystemColors.Window;
            this.lstDoors.BackgroundImageTiled = true;
            this.lstDoors.ContextMenuStrip = this.contextMenuStrip1Doors;
            resources.ApplyResources(this.lstDoors, "lstDoors");
            this.lstDoors.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lstDoors.Name = "lstDoors";
            this.toolTip1.SetToolTip(this.lstDoors, resources.GetString("lstDoors.ToolTip"));
            this.lstDoors.UseCompatibleStateImageBehavior = false;
            this.lstDoors.SelectedIndexChanged += new System.EventHandler(this.lstDoors_SelectedIndexChanged);
            this.lstDoors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmConsole_KeyDown);
            this.lstDoors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmConsole_MouseClick);
            // 
            // contextMenuStrip1Doors
            // 
            this.contextMenuStrip1Doors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCheck,
            this.mnuWarnOutputReset,
            this.locateToolStripMenuItem,
            this.personInsideToolStripMenuItem,
            this.resetPersonInsideToolStripMenuItem});
            this.contextMenuStrip1Doors.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1Doors, "contextMenuStrip1Doors");
            // 
            // mnuCheck
            // 
            this.mnuCheck.Name = "mnuCheck";
            resources.ApplyResources(this.mnuCheck, "mnuCheck");
            this.mnuCheck.Click += new System.EventHandler(this.mnuCheck_Click);
            // 
            // mnuWarnOutputReset
            // 
            this.mnuWarnOutputReset.Name = "mnuWarnOutputReset";
            resources.ApplyResources(this.mnuWarnOutputReset, "mnuWarnOutputReset");
            this.mnuWarnOutputReset.Click += new System.EventHandler(this.mnuWarnReset_Click);
            // 
            // locateToolStripMenuItem
            // 
            this.locateToolStripMenuItem.Name = "locateToolStripMenuItem";
            resources.ApplyResources(this.locateToolStripMenuItem, "locateToolStripMenuItem");
            this.locateToolStripMenuItem.Click += new System.EventHandler(this.locateToolStripMenuItem_Click);
            // 
            // personInsideToolStripMenuItem
            // 
            this.personInsideToolStripMenuItem.Name = "personInsideToolStripMenuItem";
            resources.ApplyResources(this.personInsideToolStripMenuItem, "personInsideToolStripMenuItem");
            this.personInsideToolStripMenuItem.Click += new System.EventHandler(this.personInsideToolStripMenuItem_Click);
            // 
            // resetPersonInsideToolStripMenuItem
            // 
            this.resetPersonInsideToolStripMenuItem.Name = "resetPersonInsideToolStripMenuItem";
            resources.ApplyResources(this.resetPersonInsideToolStripMenuItem, "resetPersonInsideToolStripMenuItem");
            this.resetPersonInsideToolStripMenuItem.Click += new System.EventHandler(this.resetPersonInsideToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(231)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvRunInfo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(102)))), ((int)(((byte)(131)))));
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer2.Panel2.Controls.Add(this.grpDetail);
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip2);
            // 
            // dgvRunInfo
            // 
            this.dgvRunInfo.AllowUserToAddRows = false;
            this.dgvRunInfo.AllowUserToDeleteRows = false;
            this.dgvRunInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRunInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRunInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRunInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_Category,
            this.f_RecID,
            this.f_Time,
            this.f_Desc,
            this.f_Info,
            this.f_Detail,
            this.f_MjRecStr});
            this.dgvRunInfo.ContextMenuStrip = this.contextMenuStrip2RunInfo;
            resources.ApplyResources(this.dgvRunInfo, "dgvRunInfo");
            this.dgvRunInfo.EnableHeadersVisualStyles = false;
            this.dgvRunInfo.MultiSelect = false;
            this.dgvRunInfo.Name = "dgvRunInfo";
            this.dgvRunInfo.ReadOnly = true;
            this.dgvRunInfo.RowHeadersVisible = false;
            this.dgvRunInfo.RowTemplate.Height = 23;
            this.dgvRunInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.dgvRunInfo, resources.GetString("dgvRunInfo.ToolTip"));
            this.dgvRunInfo.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRunInfo_CellFormatting);
            this.dgvRunInfo.SelectionChanged += new System.EventHandler(this.dgvRunInfo_SelectionChanged);
            // 
            // f_Category
            // 
            resources.ApplyResources(this.f_Category, "f_Category");
            this.f_Category.Name = "f_Category";
            this.f_Category.ReadOnly = true;
            this.f_Category.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_Category.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_RecID
            // 
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
            // 
            // f_Time
            // 
            resources.ApplyResources(this.f_Time, "f_Time");
            this.f_Time.Name = "f_Time";
            this.f_Time.ReadOnly = true;
            // 
            // f_Desc
            // 
            resources.ApplyResources(this.f_Desc, "f_Desc");
            this.f_Desc.Name = "f_Desc";
            this.f_Desc.ReadOnly = true;
            // 
            // f_Info
            // 
            this.f_Info.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_Info, "f_Info");
            this.f_Info.Name = "f_Info";
            this.f_Info.ReadOnly = true;
            // 
            // f_Detail
            // 
            resources.ApplyResources(this.f_Detail, "f_Detail");
            this.f_Detail.Name = "f_Detail";
            this.f_Detail.ReadOnly = true;
            // 
            // f_MjRecStr
            // 
            resources.ApplyResources(this.f_MjRecStr, "f_MjRecStr");
            this.f_MjRecStr.Name = "f_MjRecStr";
            this.f_MjRecStr.ReadOnly = true;
            // 
            // contextMenuStrip2RunInfo
            // 
            this.contextMenuStrip2RunInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearRunInfoToolStripMenuItem,
            this.displayMoreSwipesToolStripMenuItem});
            this.contextMenuStrip2RunInfo.Name = "contextMenuStrip2RunInfo";
            resources.ApplyResources(this.contextMenuStrip2RunInfo, "contextMenuStrip2RunInfo");
            // 
            // clearRunInfoToolStripMenuItem
            // 
            this.clearRunInfoToolStripMenuItem.Name = "clearRunInfoToolStripMenuItem";
            resources.ApplyResources(this.clearRunInfoToolStripMenuItem, "clearRunInfoToolStripMenuItem");
            this.clearRunInfoToolStripMenuItem.Click += new System.EventHandler(this.clearRunInfoToolStripMenuItem_Click);
            // 
            // displayMoreSwipesToolStripMenuItem
            // 
            this.displayMoreSwipesToolStripMenuItem.Name = "displayMoreSwipesToolStripMenuItem";
            resources.ApplyResources(this.displayMoreSwipesToolStripMenuItem, "displayMoreSwipesToolStripMenuItem");
            this.displayMoreSwipesToolStripMenuItem.Click += new System.EventHandler(this.displayMoreSwipesToolStripMenuItem_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            resources.ApplyResources(this.dataGridView2, "dataGridView2");
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 23;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // grpDetail
            // 
            resources.ApplyResources(this.grpDetail, "grpDetail");
            this.grpDetail.BackColor = System.Drawing.SystemColors.Window;
            this.grpDetail.Controls.Add(this.pictureBox1);
            this.grpDetail.Controls.Add(this.richTxtInfo);
            this.grpDetail.Controls.Add(this.txtInfo);
            this.grpDetail.ForeColor = System.Drawing.Color.Transparent;
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.TabStop = false;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // richTxtInfo
            // 
            resources.ApplyResources(this.richTxtInfo, "richTxtInfo");
            this.richTxtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTxtInfo.Name = "richTxtInfo";
            // 
            // txtInfo
            // 
            resources.ApplyResources(this.txtInfo, "txtInfo");
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtInfo.Name = "txtInfo";
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEventLogInfo,
            this.toolStripSeparator1,
            this.btnEventLogWarn,
            this.toolStripSeparator2,
            this.btnEventLogError});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // btnEventLogInfo
            // 
            this.btnEventLogInfo.Checked = true;
            this.btnEventLogInfo.CheckOnClick = true;
            this.btnEventLogInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnEventLogInfo.Image = global::Properties.Resources.eventlogInfo;
            resources.ApplyResources(this.btnEventLogInfo, "btnEventLogInfo");
            this.btnEventLogInfo.Name = "btnEventLogInfo";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnEventLogWarn
            // 
            this.btnEventLogWarn.Checked = true;
            this.btnEventLogWarn.CheckOnClick = true;
            this.btnEventLogWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnEventLogWarn.Image = global::Properties.Resources.eventlogWarn;
            resources.ApplyResources(this.btnEventLogWarn, "btnEventLogWarn");
            this.btnEventLogWarn.Name = "btnEventLogWarn";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnEventLogError
            // 
            this.btnEventLogError.Checked = true;
            this.btnEventLogError.CheckOnClick = true;
            this.btnEventLogError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnEventLogError.Image = global::Properties.Resources.eventlogError;
            resources.ApplyResources(this.btnEventLogError, "btnEventLogError");
            this.btnEventLogError.Name = "btnEventLogError";
            // 
            // timerUpdateDoorInfo
            // 
            this.timerUpdateDoorInfo.Interval = 200;
            this.timerUpdateDoorInfo.Tick += new System.EventHandler(this.timerUpdateDoorInfo_Tick);
            // 
            // bkUploadAndGetRecords
            // 
            this.bkUploadAndGetRecords.WorkerSupportsCancellation = true;
            this.bkUploadAndGetRecords.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkUploadAndGetRecords_DoWork);
            this.bkUploadAndGetRecords.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkUploadAndGetRecords_RunWorkerCompleted);
            // 
            // bkDispDoorStatus
            // 
            this.bkDispDoorStatus.WorkerSupportsCancellation = true;
            this.bkDispDoorStatus.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkDispDoorStatus_DoWork);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnWarnExisted,
            this.btnSelectAll,
            this.btnServer,
            this.btnStopOthers,
            this.btnCheck,
            this.btnSetTime,
            this.btnUpload,
            this.btnGetRecords,
            this.btnRealtimeGetRecords,
            this.btnRemoteOpen,
            this.btnClearRunInfo,
            this.btnMaps,
            this.cboZone,
            this.btnStopMonitor});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnWarnExisted
            // 
            this.btnWarnExisted.BackColor = System.Drawing.Color.Red;
            this.btnWarnExisted.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnWarnExisted.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnWarnExisted, "btnWarnExisted");
            this.btnWarnExisted.Name = "btnWarnExisted";
            this.btnWarnExisted.Click += new System.EventHandler(this.btnWarnExisted_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.ForeColor = System.Drawing.Color.White;
            this.btnSelectAll.Image = global::Properties.Resources.icon_select_all;
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnServer
            // 
            this.btnServer.BackColor = System.Drawing.Color.Transparent;
            this.btnServer.ForeColor = System.Drawing.Color.White;
            this.btnServer.Image = global::Properties.Resources.icon_monitor;
            resources.ApplyResources(this.btnServer, "btnServer");
            this.btnServer.Name = "btnServer";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnStopOthers
            // 
            this.btnStopOthers.ForeColor = System.Drawing.Color.White;
            this.btnStopOthers.Image = global::Properties.Resources.icon_stop;
            resources.ApplyResources(this.btnStopOthers, "btnStopOthers");
            this.btnStopOthers.Name = "btnStopOthers";
            this.btnStopOthers.Click += new System.EventHandler(this.btnStopOthers_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.ForeColor = System.Drawing.Color.White;
            this.btnCheck.Image = global::Properties.Resources.icon_check;
            resources.ApplyResources(this.btnCheck, "btnCheck");
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.ForeColor = System.Drawing.Color.White;
            this.btnSetTime.Image = global::Properties.Resources.icon_timezone;
            resources.ApplyResources(this.btnSetTime, "btnSetTime");
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.ForeColor = System.Drawing.Color.White;
            this.btnUpload.Image = global::Properties.Resources.icon_upload;
            resources.ApplyResources(this.btnUpload, "btnUpload");
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnGetRecords
            // 
            this.btnGetRecords.ForeColor = System.Drawing.Color.White;
            this.btnGetRecords.Image = global::Properties.Resources.icon_get_rec;
            resources.ApplyResources(this.btnGetRecords, "btnGetRecords");
            this.btnGetRecords.Name = "btnGetRecords";
            this.btnGetRecords.Click += new System.EventHandler(this.btnGetRecords_Click);
            // 
            // btnRealtimeGetRecords
            // 
            this.btnRealtimeGetRecords.ForeColor = System.Drawing.Color.White;
            this.btnRealtimeGetRecords.Image = global::Properties.Resources.icon_realtime_getrecords;
            resources.ApplyResources(this.btnRealtimeGetRecords, "btnRealtimeGetRecords");
            this.btnRealtimeGetRecords.Name = "btnRealtimeGetRecords";
            this.btnRealtimeGetRecords.Click += new System.EventHandler(this.btnRealtimeGetRecords_Click);
            // 
            // btnRemoteOpen
            // 
            this.btnRemoteOpen.ForeColor = System.Drawing.Color.White;
            this.btnRemoteOpen.Image = global::Properties.Resources.icon_open_door;
            resources.ApplyResources(this.btnRemoteOpen, "btnRemoteOpen");
            this.btnRemoteOpen.Name = "btnRemoteOpen";
            this.btnRemoteOpen.Click += new System.EventHandler(this.btnRemoteOpen_Click);
            // 
            // btnClearRunInfo
            // 
            this.btnClearRunInfo.ForeColor = System.Drawing.Color.White;
            this.btnClearRunInfo.Image = global::Properties.Resources.icon_clear_info;
            resources.ApplyResources(this.btnClearRunInfo, "btnClearRunInfo");
            this.btnClearRunInfo.Name = "btnClearRunInfo";
            this.btnClearRunInfo.Click += new System.EventHandler(this.btnClearRunInfo_Click);
            // 
            // btnMaps
            // 
            this.btnMaps.BackColor = System.Drawing.Color.Transparent;
            this.btnMaps.ForeColor = System.Drawing.Color.White;
            this.btnMaps.Image = global::Properties.Resources.icon_maps;
            resources.ApplyResources(this.btnMaps, "btnMaps");
            this.btnMaps.Name = "btnMaps";
            this.btnMaps.Click += new System.EventHandler(this.btnMaps_Click);
            // 
            // cboZone
            // 
            this.cboZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboZone, "cboZone");
            this.cboZone.Name = "cboZone";
            this.cboZone.SelectedIndexChanged += new System.EventHandler(this.cboZone_SelectedIndexChanged);
            // 
            // btnStopMonitor
            // 
            this.btnStopMonitor.ForeColor = System.Drawing.Color.White;
            this.btnStopMonitor.Image = global::Properties.Resources.icon_stop;
            resources.ApplyResources(this.btnStopMonitor, "btnStopMonitor");
            this.btnStopMonitor.Name = "btnStopMonitor";
            this.btnStopMonitor.Click += new System.EventHandler(this.btnStopMonitor_Click);
            // 
            // timerWarn
            // 
            this.timerWarn.Interval = 500;
            this.timerWarn.Tick += new System.EventHandler(this.timerWarn_Tick);
            // 
            // bkRealtimeGetRecords
            // 
            this.bkRealtimeGetRecords.WorkerSupportsCancellation = true;
            this.bkRealtimeGetRecords.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkRealtimeGetRecords_DoWork);
            this.bkRealtimeGetRecords.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkRealtimeGetRecords_RunWorkerCompleted);
            // 
            // frmConsole
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmConsole";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConsole_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConsole_FormClosed);
            this.Load += new System.EventHandler(this.frmConsole_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmConsole_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmConsole_MouseClick);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.grpTool.ResumeLayout(false);
            this.grpTool.PerformLayout();
            this.contextMenuStrip1Doors.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRunInfo)).EndInit();
            this.contextMenuStrip2RunInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

