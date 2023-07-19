namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmPrivilegeDoorCopy
    {
        private ImageButton btnAddAllUsers;
        private ImageButton btnAddOneUser;
        private ImageButton btnAddOneUser4Copy;
        private ImageButton btnAddPass;
        private ImageButton btnDelAllUsers;
        private ImageButton btnDeleteOneUser4Copy;
        private ImageButton btnDelOneUser;
        private ImageButton button1;
        private ComboBox cbof_ZoneID;
        private IContainer components;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private dfrmFind dfrmFind1;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private DataGridView dgvDoors;
        private DataGridView dgvSelectedDoors;
        private DataGridView dgvSelectedDoors4Copy;
        private DataTable dt;
        private DataTable dt4copy;
        private DataView dv;
        private DataView dvSelected;
        private DataView dvSelectedNone;
        private DataView dvtmp;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_DoorID;
        private DataGridViewTextBoxColumn f_DoorName;
        private DataGridViewTextBoxColumn f_DoorNo;
        private Label label1;
        private Label label2;
        private Label label25;
        private Label lblWait;
        private ProgressBar progressBar1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolTip toolTip1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmPrivilegeDoorCopy));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnAddPass = new System.Windows.Forms.ImageButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteOneUser4Copy = new System.Windows.Forms.ImageButton();
            this.btnAddOneUser4Copy = new System.Windows.Forms.ImageButton();
            this.lblWait = new System.Windows.Forms.Label();
            this.btnDelAllUsers = new System.Windows.Forms.ImageButton();
            this.btnDelOneUser = new System.Windows.Forms.ImageButton();
            this.btnAddOneUser = new System.Windows.Forms.ImageButton();
            this.button1 = new System.Windows.Forms.ImageButton();
            this.btnAddAllUsers = new System.Windows.Forms.ImageButton();
            this.dgvDoors = new System.Windows.Forms.DataGridView();
            this.f_DoorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSelectedDoors4Copy = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSelectedDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbof_ZoneID = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors4Copy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // btnAddPass
            // 
            resources.ApplyResources(this.btnAddPass, "btnAddPass");
            this.btnAddPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnAddPass.Focusable = true;
            this.btnAddPass.ForeColor = System.Drawing.Color.White;
            this.btnAddPass.Image = global::Properties.Resources.Rec1Pass;
            this.btnAddPass.Name = "btnAddPass";
            this.btnAddPass.TabStop = false;
            this.btnAddPass.Toggle = false;
            this.btnAddPass.UseVisualStyleBackColor = false;
            this.btnAddPass.Click += new System.EventHandler(this.btnAddPass_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // btnDeleteOneUser4Copy
            // 
            this.btnDeleteOneUser4Copy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteOneUser4Copy, "btnDeleteOneUser4Copy");
            this.btnDeleteOneUser4Copy.Focusable = true;
            this.btnDeleteOneUser4Copy.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOneUser4Copy.Name = "btnDeleteOneUser4Copy";
            this.btnDeleteOneUser4Copy.TabStop = false;
            this.btnDeleteOneUser4Copy.Toggle = false;
            this.btnDeleteOneUser4Copy.UseVisualStyleBackColor = false;
            this.btnDeleteOneUser4Copy.Click += new System.EventHandler(this.btnDeleteOneUser4Copy_Click);
            // 
            // btnAddOneUser4Copy
            // 
            this.btnAddOneUser4Copy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneUser4Copy, "btnAddOneUser4Copy");
            this.btnAddOneUser4Copy.Focusable = true;
            this.btnAddOneUser4Copy.ForeColor = System.Drawing.Color.White;
            this.btnAddOneUser4Copy.Name = "btnAddOneUser4Copy";
            this.btnAddOneUser4Copy.Toggle = false;
            this.btnAddOneUser4Copy.UseVisualStyleBackColor = false;
            this.btnAddOneUser4Copy.Click += new System.EventHandler(this.btnAddOneUser4Copy_Click);
            // 
            // lblWait
            // 
            resources.ApplyResources(this.lblWait, "lblWait");
            this.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWait.ForeColor = System.Drawing.Color.White;
            this.lblWait.Name = "lblWait";
            // 
            // btnDelAllUsers
            // 
            this.btnDelAllUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
            this.btnDelAllUsers.Focusable = true;
            this.btnDelAllUsers.ForeColor = System.Drawing.Color.White;
            this.btnDelAllUsers.Name = "btnDelAllUsers";
            this.btnDelAllUsers.Toggle = false;
            this.btnDelAllUsers.UseVisualStyleBackColor = false;
            this.btnDelAllUsers.Click += new System.EventHandler(this.btnDelAllUsers_Click);
            // 
            // btnDelOneUser
            // 
            this.btnDelOneUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
            this.btnDelOneUser.Focusable = true;
            this.btnDelOneUser.ForeColor = System.Drawing.Color.White;
            this.btnDelOneUser.Name = "btnDelOneUser";
            this.btnDelOneUser.Toggle = false;
            this.btnDelOneUser.UseVisualStyleBackColor = false;
            this.btnDelOneUser.Click += new System.EventHandler(this.btnDelOneUser_Click);
            // 
            // btnAddOneUser
            // 
            this.btnAddOneUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
            this.btnAddOneUser.Focusable = true;
            this.btnAddOneUser.ForeColor = System.Drawing.Color.White;
            this.btnAddOneUser.Name = "btnAddOneUser";
            this.btnAddOneUser.Toggle = false;
            this.btnAddOneUser.UseVisualStyleBackColor = false;
            this.btnAddOneUser.Click += new System.EventHandler(this.btnAddOneUser_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.button1.Focusable = true;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.Toggle = false;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAddAllUsers
            // 
            this.btnAddAllUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
            this.btnAddAllUsers.Focusable = true;
            this.btnAddAllUsers.ForeColor = System.Drawing.Color.White;
            this.btnAddAllUsers.Name = "btnAddAllUsers";
            this.btnAddAllUsers.Toggle = false;
            this.btnAddAllUsers.UseVisualStyleBackColor = false;
            this.btnAddAllUsers.Click += new System.EventHandler(this.btnAddAllUsers_Click);
            // 
            // dgvDoors
            // 
            this.dgvDoors.AllowUserToAddRows = false;
            this.dgvDoors.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvDoors, "dgvDoors");
            this.dgvDoors.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_DoorID,
            this.f_ControllerSN,
            this.f_DoorNo,
            this.f_DoorName});
            this.dgvDoors.EnableHeadersVisualStyles = false;
            this.dgvDoors.Name = "dgvDoors";
            this.dgvDoors.ReadOnly = true;
            this.dgvDoors.RowTemplate.Height = 23;
            this.dgvDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDoors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDoors_KeyDown);
            this.dgvDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
            // 
            // f_DoorID
            // 
            resources.ApplyResources(this.f_DoorID, "f_DoorID");
            this.f_DoorID.Name = "f_DoorID";
            this.f_DoorID.ReadOnly = true;
            // 
            // f_ControllerSN
            // 
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            // 
            // f_DoorNo
            // 
            resources.ApplyResources(this.f_DoorNo, "f_DoorNo");
            this.f_DoorNo.Name = "f_DoorNo";
            this.f_DoorNo.ReadOnly = true;
            // 
            // f_DoorName
            // 
            this.f_DoorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_DoorName, "f_DoorName");
            this.f_DoorName.Name = "f_DoorName";
            this.f_DoorName.ReadOnly = true;
            // 
            // dgvSelectedDoors4Copy
            // 
            this.dgvSelectedDoors4Copy.AllowUserToAddRows = false;
            this.dgvSelectedDoors4Copy.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvSelectedDoors4Copy, "dgvSelectedDoors4Copy");
            this.dgvSelectedDoors4Copy.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors4Copy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelectedDoors4Copy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedDoors4Copy.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13});
            this.dgvSelectedDoors4Copy.EnableHeadersVisualStyles = false;
            this.dgvSelectedDoors4Copy.Name = "dgvSelectedDoors4Copy";
            this.dgvSelectedDoors4Copy.ReadOnly = true;
            this.dgvSelectedDoors4Copy.RowTemplate.Height = 23;
            this.dgvSelectedDoors4Copy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            // dataGridViewTextBoxColumn12
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // dgvSelectedDoors
            // 
            this.dgvSelectedDoors.AllowUserToAddRows = false;
            this.dgvSelectedDoors.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
            this.dgvSelectedDoors.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn17});
            this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
            this.dgvSelectedDoors.Name = "dgvSelectedDoors";
            this.dgvSelectedDoors.ReadOnly = true;
            this.dgvSelectedDoors.RowTemplate.Height = 23;
            this.dgvSelectedDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn14
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn16
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            // 
            // cbof_ZoneID
            // 
            this.cbof_ZoneID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ZoneID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
            this.cbof_ZoneID.Name = "cbof_ZoneID";
            this.cbof_ZoneID.SelectedIndexChanged += new System.EventHandler(this.cbof_ZoneID_SelectedIndexChanged);
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label25.Name = "label25";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(92)))), ((int)(((byte)(120)))));
            this.statusStrip1.BackgroundImage = global::Properties.Resources.pMain_bottom;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.BackgroundImage = global::Properties.Resources.pMain_bottom;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel2.BackgroundImage = global::Properties.Resources.pMain_bottom;
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Spring = true;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.button1);
            this.panelBottomBanner.Controls.Add(this.btnAddPass);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmPrivilegeDoorCopy
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbof_ZoneID);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.dgvSelectedDoors);
            this.Controls.Add(this.dgvSelectedDoors4Copy);
            this.Controls.Add(this.dgvDoors);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDeleteOneUser4Copy);
            this.Controls.Add(this.btnAddOneUser4Copy);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.btnDelAllUsers);
            this.Controls.Add(this.btnDelOneUser);
            this.Controls.Add(this.btnAddOneUser);
            this.Controls.Add(this.btnAddAllUsers);
            this.MinimizeBox = false;
            this.Name = "dfrmPrivilegeDoorCopy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilegeCopy_FormClosing);
            this.Load += new System.EventHandler(this.dfrmPrivilegeCopy_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmPrivilegeCopy_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors4Copy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

