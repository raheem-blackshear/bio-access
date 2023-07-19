namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
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

    partial class dfrmPrivilege
    {
        private ImageButton btnAddAllDoors;
        private ImageButton btnAddAllUsers;
        private ImageButton btnAddOneDoor;
        private ImageButton btnAddOneUser;
        private ImageButton btnAddPass;
        private ImageButton btnAddPassAndUpload;
        private ImageButton btnDelAllDoors;
        private ImageButton btnDelAllUsers;
        private ImageButton btnDeletePass;
        private ImageButton btnDeletePassAndUpload;
        private ImageButton btnDelOneDoor;
        private ImageButton btnDelOneUser;
        private ImageButton btnExit;
        private DataGridViewTextBoxColumn CardNO;
        private ComboBox cbof_ControlSegID;
        private ComboBox cbof_GroupID;
        private ComboBox cbof_ZoneID;
        private DataGridViewTextBoxColumn Column1;
        private IContainer components;
        private DataGridViewTextBoxColumn ConsumerID;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private dfrmFind dfrmFind1;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private DataGridView dgvDoors;
        private DataGridView dgvSelectedDoors;
        private DataGridView dgvSelectedUsers;
        private DataGridView dgvUsers;
        private DataTable dt;
        private DataTable dtDoorTmpSelected;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvDoorTmpSelected;
        private DataView dvSelected;
        private DataView dvSelectedControllerID;
        private DataView dvtmp;
        private DataGridViewTextBoxColumn f_GroupID;
        private DataGridViewTextBoxColumn f_Selected;
        private DataGridViewTextBoxColumn f_Selected2;
        private DataGridViewTextBoxColumn f_SelectedGroup;
        private DataGridViewCheckBoxColumn f_SelectedUsers;
        private DataGridViewTextBoxColumn f_ZoneID;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label25;
        private Label label3;
        private Label label4;
        private Label lblWait;
        private ProgressBar progressBar1;
        private ToolTip toolTip1;
        private DataGridViewTextBoxColumn UserID;
        private DataGridViewTextBoxColumn UserName;

        protected override void Dispose(bool disposing)
        {
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

        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmPrivilege));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnDeletePass = new System.Windows.Forms.ImageButton();
            this.btnAddPass = new System.Windows.Forms.ImageButton();
            this.btnDeletePassAndUpload = new System.Windows.Forms.ImageButton();
            this.btnAddPassAndUpload = new System.Windows.Forms.ImageButton();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbof_ZoneID = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.dgvSelectedDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ZoneID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelAllDoors = new System.Windows.Forms.ImageButton();
            this.btnDelOneDoor = new System.Windows.Forms.ImageButton();
            this.btnAddOneDoor = new System.Windows.Forms.ImageButton();
            this.btnAddAllDoors = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbof_ControlSegID = new System.Windows.Forms.ComboBox();
            this.cbof_GroupID = new System.Windows.Forms.ComboBox();
            this.lblWait = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvSelectedUsers = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SelectedGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_GroupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SelectedUsers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnDelAllUsers = new System.Windows.Forms.ImageButton();
            this.btnDelOneUser = new System.Windows.Forms.ImageButton();
            this.btnAddOneUser = new System.Windows.Forms.ImageButton();
            this.btnAddAllUsers = new System.Windows.Forms.ImageButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
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
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnDeletePass
            // 
            resources.ApplyResources(this.btnDeletePass, "btnDeletePass");
            this.btnDeletePass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnDeletePass.Focusable = true;
            this.btnDeletePass.ForeColor = System.Drawing.Color.White;
            this.btnDeletePass.Image = global::Properties.Resources.Rec2NoPass;
            this.btnDeletePass.Name = "btnDeletePass";
            this.btnDeletePass.Toggle = false;
            this.toolTip1.SetToolTip(this.btnDeletePass, resources.GetString("btnDeletePass.ToolTip"));
            this.btnDeletePass.UseVisualStyleBackColor = false;
            this.btnDeletePass.Click += new System.EventHandler(this.btnDeletePass_Click);
            // 
            // btnAddPass
            // 
            resources.ApplyResources(this.btnAddPass, "btnAddPass");
            this.btnAddPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnAddPass.Focusable = true;
            this.btnAddPass.ForeColor = System.Drawing.Color.White;
            this.btnAddPass.Image = global::Properties.Resources.Rec1Pass;
            this.btnAddPass.Name = "btnAddPass";
            this.btnAddPass.Toggle = false;
            this.toolTip1.SetToolTip(this.btnAddPass, resources.GetString("btnAddPass.ToolTip"));
            this.btnAddPass.UseVisualStyleBackColor = false;
            this.btnAddPass.Click += new System.EventHandler(this.btnAddPass_Click);
            // 
            // btnDeletePassAndUpload
            // 
            resources.ApplyResources(this.btnDeletePassAndUpload, "btnDeletePassAndUpload");
            this.btnDeletePassAndUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnDeletePassAndUpload.Focusable = true;
            this.btnDeletePassAndUpload.ForeColor = System.Drawing.Color.White;
            this.btnDeletePassAndUpload.Image = global::Properties.Resources.wg16UploadNoPass;
            this.btnDeletePassAndUpload.Name = "btnDeletePassAndUpload";
            this.btnDeletePassAndUpload.Toggle = false;
            this.toolTip1.SetToolTip(this.btnDeletePassAndUpload, resources.GetString("btnDeletePassAndUpload.ToolTip"));
            this.btnDeletePassAndUpload.UseVisualStyleBackColor = false;
            this.btnDeletePassAndUpload.Click += new System.EventHandler(this.btnDeletePassAndUpload_Click);
            // 
            // btnAddPassAndUpload
            // 
            resources.ApplyResources(this.btnAddPassAndUpload, "btnAddPassAndUpload");
            this.btnAddPassAndUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnAddPassAndUpload.Focusable = true;
            this.btnAddPassAndUpload.ForeColor = System.Drawing.Color.White;
            this.btnAddPassAndUpload.Image = global::Properties.Resources.wg16UploadPass;
            this.btnAddPassAndUpload.Name = "btnAddPassAndUpload";
            this.btnAddPassAndUpload.Toggle = false;
            this.toolTip1.SetToolTip(this.btnAddPassAndUpload, resources.GetString("btnAddPassAndUpload.ToolTip"));
            this.btnAddPassAndUpload.UseVisualStyleBackColor = false;
            this.btnAddPassAndUpload.Click += new System.EventHandler(this.btnAddPassAndUpload_Click);
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
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.cbof_ZoneID);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.dgvSelectedDoors);
            this.groupBox2.Controls.Add(this.dgvDoors);
            this.groupBox2.Controls.Add(this.btnDelAllDoors);
            this.groupBox2.Controls.Add(this.btnDelOneDoor);
            this.groupBox2.Controls.Add(this.btnAddOneDoor);
            this.groupBox2.Controls.Add(this.btnAddAllDoors);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbof_ZoneID
            // 
            this.cbof_ZoneID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ZoneID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
            this.cbof_ZoneID.Name = "cbof_ZoneID";
            this.cbof_ZoneID.SelectedIndexChanged += new System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label25.Name = "label25";
            // 
            // dgvSelectedDoors
            // 
            this.dgvSelectedDoors.AllowUserToAddRows = false;
            this.dgvSelectedDoors.AllowUserToDeleteRows = false;
            this.dgvSelectedDoors.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
            this.dgvSelectedDoors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.f_Selected2,
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
            this.dgvSelectedDoors.Name = "dgvSelectedDoors";
            this.dgvSelectedDoors.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedDoors.RowTemplate.Height = 23;
            this.dgvSelectedDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn8
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // f_Selected2
            // 
            resources.ApplyResources(this.f_Selected2, "f_Selected2");
            this.f_Selected2.Name = "f_Selected2";
            this.f_Selected2.ReadOnly = true;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // dgvDoors
            // 
            this.dgvDoors.AllowUserToAddRows = false;
            this.dgvDoors.AllowUserToDeleteRows = false;
            this.dgvDoors.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvDoors, "dgvDoors");
            this.dgvDoors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.f_Selected,
            this.f_ZoneID});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDoors.EnableHeadersVisualStyles = false;
            this.dgvDoors.Name = "dgvDoors";
            this.dgvDoors.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDoors.RowTemplate.Height = 23;
            this.dgvDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDoors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDoors_KeyDown);
            this.dgvDoors.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseClick);
            this.dgvDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
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
            // f_ZoneID
            // 
            resources.ApplyResources(this.f_ZoneID, "f_ZoneID");
            this.f_ZoneID.Name = "f_ZoneID";
            this.f_ZoneID.ReadOnly = true;
            // 
            // btnDelAllDoors
            // 
            this.btnDelAllDoors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
            this.btnDelAllDoors.Focusable = true;
            this.btnDelAllDoors.ForeColor = System.Drawing.Color.White;
            this.btnDelAllDoors.Name = "btnDelAllDoors";
            this.btnDelAllDoors.Toggle = false;
            this.btnDelAllDoors.UseVisualStyleBackColor = false;
            this.btnDelAllDoors.Click += new System.EventHandler(this.btnDelAllDoors_Click);
            // 
            // btnDelOneDoor
            // 
            this.btnDelOneDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
            this.btnDelOneDoor.Focusable = true;
            this.btnDelOneDoor.ForeColor = System.Drawing.Color.White;
            this.btnDelOneDoor.Name = "btnDelOneDoor";
            this.btnDelOneDoor.Toggle = false;
            this.btnDelOneDoor.UseVisualStyleBackColor = false;
            this.btnDelOneDoor.Click += new System.EventHandler(this.btnDelOneDoor_Click);
            // 
            // btnAddOneDoor
            // 
            this.btnAddOneDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
            this.btnAddOneDoor.Focusable = true;
            this.btnAddOneDoor.ForeColor = System.Drawing.Color.White;
            this.btnAddOneDoor.Name = "btnAddOneDoor";
            this.btnAddOneDoor.Toggle = false;
            this.btnAddOneDoor.UseVisualStyleBackColor = false;
            this.btnAddOneDoor.Click += new System.EventHandler(this.btnAddOneDoor_Click);
            // 
            // btnAddAllDoors
            // 
            this.btnAddAllDoors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
            this.btnAddAllDoors.Focusable = true;
            this.btnAddAllDoors.ForeColor = System.Drawing.Color.White;
            this.btnAddAllDoors.Name = "btnAddAllDoors";
            this.btnAddAllDoors.Toggle = false;
            this.btnAddAllDoors.UseVisualStyleBackColor = false;
            this.btnAddAllDoors.Click += new System.EventHandler(this.btnAddAllDoors_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cbof_ControlSegID);
            this.groupBox1.Controls.Add(this.cbof_GroupID);
            this.groupBox1.Controls.Add(this.lblWait);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dgvSelectedUsers);
            this.groupBox1.Controls.Add(this.dgvUsers);
            this.groupBox1.Controls.Add(this.btnDelAllUsers);
            this.groupBox1.Controls.Add(this.btnDelOneUser);
            this.groupBox1.Controls.Add(this.btnAddOneUser);
            this.groupBox1.Controls.Add(this.btnAddAllUsers);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbof_ControlSegID
            // 
            this.cbof_ControlSegID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ControlSegID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
            this.cbof_ControlSegID.Name = "cbof_ControlSegID";
            // 
            // cbof_GroupID
            // 
            this.cbof_GroupID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_GroupID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
            this.cbof_GroupID.Name = "cbof_GroupID";
            this.cbof_GroupID.SelectedIndexChanged += new System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
            // 
            // lblWait
            // 
            resources.ApplyResources(this.lblWait, "lblWait");
            this.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWait.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblWait.Name = "lblWait";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // dgvSelectedUsers
            // 
            this.dgvSelectedUsers.AllowUserToAddRows = false;
            this.dgvSelectedUsers.AllowUserToDeleteRows = false;
            this.dgvSelectedUsers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
            this.dgvSelectedUsers.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.f_SelectedGroup,
            this.dataGridViewCheckBoxColumn1});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
            this.dgvSelectedUsers.Name = "dgvSelectedUsers";
            this.dgvSelectedUsers.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvSelectedUsers.RowTemplate.Height = 23;
            this.dgvSelectedUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSelectedUsers_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle8;
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
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // f_SelectedGroup
            // 
            resources.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
            this.f_SelectedGroup.Name = "f_SelectedGroup";
            this.f_SelectedGroup.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvUsers, "dgvUsers");
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsumerID,
            this.UserID,
            this.UserName,
            this.CardNO,
            this.f_GroupID,
            this.f_SelectedUsers});
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvUsers_KeyDown);
            this.dgvUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseDoubleClick);
            // 
            // ConsumerID
            // 
            resources.ApplyResources(this.ConsumerID, "ConsumerID");
            this.ConsumerID.Name = "ConsumerID";
            this.ConsumerID.ReadOnly = true;
            // 
            // UserID
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.UserID.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.UserID, "UserID");
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            // 
            // UserName
            // 
            resources.ApplyResources(this.UserName, "UserName");
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // CardNO
            // 
            resources.ApplyResources(this.CardNO, "CardNO");
            this.CardNO.Name = "CardNO";
            this.CardNO.ReadOnly = true;
            // 
            // f_GroupID
            // 
            resources.ApplyResources(this.f_GroupID, "f_GroupID");
            this.f_GroupID.Name = "f_GroupID";
            this.f_GroupID.ReadOnly = true;
            // 
            // f_SelectedUsers
            // 
            resources.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
            this.f_SelectedUsers.Name = "f_SelectedUsers";
            this.f_SelectedUsers.ReadOnly = true;
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label4.Name = "label4";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnAddPass);
            this.panelBottomBanner.Controls.Add(this.btnDeletePass);
            this.panelBottomBanner.Controls.Add(this.btnAddPassAndUpload);
            this.panelBottomBanner.Controls.Add(this.btnDeletePassAndUpload);
            this.panelBottomBanner.Controls.Add(this.btnExit);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmPrivilege
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.MinimizeBox = false;
            this.Name = "dfrmPrivilege";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilege_FormClosing);
            this.Load += new System.EventHandler(this.dfrmPrivilege_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmPrivilege_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel panelBottomBanner;
    }
}

