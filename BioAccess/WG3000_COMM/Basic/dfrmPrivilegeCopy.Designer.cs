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

    partial class dfrmPrivilegeCopy
    {
        private ImageButton btnAddAllUsers;
        private ImageButton btnAddOneUser;
        private ImageButton btnAddOneUser4Copy;
        private ImageButton btnAddPass;
        private ImageButton btnDelAllUsers;
        private ImageButton btnDeleteOneUser4Copy;
        private ImageButton btnDelOneUser;
        private ImageButton button1;
        private DataGridViewTextBoxColumn CardNO;
        private ComboBox cbof_GroupID;
        private IContainer components;
        private DataGridViewTextBoxColumn ConsumerID;
        private DataGridViewTextBoxColumn ConsumerName;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private dfrmFind dfrmFind1;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private DataGridView dgvSelectedUsers;
        private DataGridView dgvSelectedUsers4Copy;
        private DataGridView dgvUsers;
        private DataTable dt;
        private DataTable dt4copy;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvSelected;
        private DataGridViewTextBoxColumn f_GroupID;
        private DataGridViewTextBoxColumn f_SelectedGroup;
        private DataGridViewCheckBoxColumn f_SelectedUsers;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label lblWait;
        private ProgressBar progressBar1;
        private ToolTip toolTip1;
        private DataGridViewTextBoxColumn UserID;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmPrivilegeCopy));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnAddPass = new System.Windows.Forms.ImageButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteOneUser4Copy = new System.Windows.Forms.ImageButton();
            this.btnAddOneUser4Copy = new System.Windows.Forms.ImageButton();
            this.dgvSelectedUsers4Copy = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lblWait = new System.Windows.Forms.Label();
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
            this.ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_GroupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SelectedUsers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnDelAllUsers = new System.Windows.Forms.ImageButton();
            this.btnDelOneUser = new System.Windows.Forms.ImageButton();
            this.btnAddOneUser = new System.Windows.Forms.ImageButton();
            this.button1 = new System.Windows.Forms.ImageButton();
            this.btnAddAllUsers = new System.Windows.Forms.ImageButton();
            this.cbof_GroupID = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers4Copy)).BeginInit();
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
            // dgvSelectedUsers4Copy
            // 
            this.dgvSelectedUsers4Copy.AllowUserToAddRows = false;
            this.dgvSelectedUsers4Copy.AllowUserToDeleteRows = false;
            this.dgvSelectedUsers4Copy.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelectedUsers4Copy, "dgvSelectedUsers4Copy");
            this.dgvSelectedUsers4Copy.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers4Copy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelectedUsers4Copy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedUsers4Copy.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewCheckBoxColumn2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedUsers4Copy.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedUsers4Copy.EnableHeadersVisualStyles = false;
            this.dgvSelectedUsers4Copy.Name = "dgvSelectedUsers4Copy";
            this.dgvSelectedUsers4Copy.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers4Copy.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSelectedUsers4Copy.RowTemplate.Height = 23;
            this.dgvSelectedUsers4Copy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle2;
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
            // dataGridViewCheckBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.ReadOnly = true;
            // 
            // lblWait
            // 
            resources.ApplyResources(this.lblWait, "lblWait");
            this.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWait.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblWait.Name = "lblWait";
            // 
            // dgvSelectedUsers
            // 
            this.dgvSelectedUsers.AllowUserToAddRows = false;
            this.dgvSelectedUsers.AllowUserToDeleteRows = false;
            this.dgvSelectedUsers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
            this.dgvSelectedUsers.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.f_SelectedGroup,
            this.dataGridViewCheckBoxColumn1});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
            this.dgvSelectedUsers.Name = "dgvSelectedUsers";
            this.dgvSelectedUsers.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
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
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle6;
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsumerID,
            this.UserID,
            this.ConsumerName,
            this.CardNO,
            this.f_GroupID,
            this.f_SelectedUsers});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.UserID.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.UserID, "UserID");
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            // 
            // ConsumerName
            // 
            resources.ApplyResources(this.ConsumerName, "ConsumerName");
            this.ConsumerName.Name = "ConsumerName";
            this.ConsumerName.ReadOnly = true;
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
            // cbof_GroupID
            // 
            this.cbof_GroupID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
            this.cbof_GroupID.FormattingEnabled = true;
            this.cbof_GroupID.Name = "cbof_GroupID";
            this.cbof_GroupID.SelectedIndexChanged += new System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label4.Name = "label4";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnAddPass);
            this.panelBottomBanner.Controls.Add(this.button1);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmPrivilegeCopy
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDeleteOneUser4Copy);
            this.Controls.Add(this.btnAddOneUser4Copy);
            this.Controls.Add(this.dgvSelectedUsers4Copy);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.dgvSelectedUsers);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.btnDelAllUsers);
            this.Controls.Add(this.btnDelOneUser);
            this.Controls.Add(this.btnAddOneUser);
            this.Controls.Add(this.btnAddAllUsers);
            this.Controls.Add(this.cbof_GroupID);
            this.Controls.Add(this.label4);
            this.MinimizeBox = false;
            this.Name = "dfrmPrivilegeCopy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilegeCopy_FormClosing);
            this.Load += new System.EventHandler(this.dfrmPrivilegeCopy_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmPrivilegeCopy_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers4Copy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

