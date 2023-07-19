namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmTCPIPWEBConfigure
    {
        private Button btnCancel;
        private Button btnDownloadUsers;
        private Button btnEditUsers;
        private Button btnOK;
        public Button btnOption;
        public Button btnOptionWEB;
        private Button btnOtherLanguage;
        private Button btnRestoreNameAndPassword;
        private Button btnSelectFile;
        private Button btnSelectUserFile;
        private Button btnTryWEB;
        private Button btnuploadUser;
        public ComboBox cboDateFormat;
        public ComboBox cboLanguage;
        public ComboBox cboLanguage2;
        public CheckBox chkAdjustTime;
        public CheckBox chkAutoUploadWEBUsers;
        public CheckBox chkEditIP;
        public CheckBox chkUpdateSpecialCard;
        public CheckBox chkUpdateSuperCard;
        public CheckBox chkUpdateWebSet;
        public CheckBox chkWebOnlyQuery;
        private IContainer components = null;
        private DataGridView dataGridView3;
        private DataTable dtPrivilege;
        private DataTable tbFpTempl;
        private DataTable tbFaceTempl;
        public DataTable dtUsers;
        public DataTable dtWebString;
        private DataView dv;
        public GroupBox grpIP;
        public GroupBox grpSpecialCards;
        public GroupBox grpSuperCards;
        public GroupBox grpWEB;
        public GroupBox grpWEBEnabled;
        private GroupBox grpWEBUsers;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        public Label label6;
        private Label label8;
        private Label label9;
        public Label lblHttpPort;
        public Label lblPort;
        public NumericUpDown nudHttpPort;
        public NumericUpDown nudPort;
        private OpenFileDialog openFileDialog1;
        public RadioButton optWEBDisable;
        public RadioButton optWEBEnabled;
        private DataTable tb;
        private DataTable tb1;
        private DataTable tb2;
        private DataTable tb3;
        private TextBox txtf_ControllerSN;
        private TextBox txtf_gateway;
        private TextBox txtf_IP;
        private TextBox txtf_MACAddr;
        private TextBox txtf_mask;
        public TextBox txtSelectedFileName;
        public MaskedTextBox txtSpecialCard1;
        public MaskedTextBox txtSpecialCard2;
        public MaskedTextBox txtSuperCard1;
        public MaskedTextBox txtSuperCard2;
        public TextBox txtUsersFile;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmTCPIPWEBConfigure));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpWEBEnabled = new System.Windows.Forms.GroupBox();
            this.optWEBEnabled = new System.Windows.Forms.RadioButton();
            this.optWEBDisable = new System.Windows.Forms.RadioButton();
            this.grpWEBUsers = new System.Windows.Forms.GroupBox();
            this.txtUsersFile = new System.Windows.Forms.TextBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.chkAutoUploadWEBUsers = new System.Windows.Forms.CheckBox();
            this.cboLanguage2 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnEditUsers = new System.Windows.Forms.Button();
            this.btnDownloadUsers = new System.Windows.Forms.Button();
            this.btnSelectUserFile = new System.Windows.Forms.Button();
            this.btnuploadUser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtf_ControllerSN = new System.Windows.Forms.TextBox();
            this.txtf_MACAddr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtf_IP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtf_mask = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtf_gateway = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnOption = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.grpIP = new System.Windows.Forms.GroupBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.chkEditIP = new System.Windows.Forms.CheckBox();
            this.grpWEB = new System.Windows.Forms.GroupBox();
            this.chkWebOnlyQuery = new System.Windows.Forms.CheckBox();
            this.cboDateFormat = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOptionWEB = new System.Windows.Forms.Button();
            this.lblHttpPort = new System.Windows.Forms.Label();
            this.nudHttpPort = new System.Windows.Forms.NumericUpDown();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnOtherLanguage = new System.Windows.Forms.Button();
            this.txtSelectedFileName = new System.Windows.Forms.TextBox();
            this.chkUpdateWebSet = new System.Windows.Forms.CheckBox();
            this.chkUpdateSuperCard = new System.Windows.Forms.CheckBox();
            this.grpSuperCards = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSuperCard2 = new System.Windows.Forms.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSuperCard1 = new System.Windows.Forms.MaskedTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkAdjustTime = new System.Windows.Forms.CheckBox();
            this.btnTryWEB = new System.Windows.Forms.Button();
            this.btnRestoreNameAndPassword = new System.Windows.Forms.Button();
            this.chkUpdateSpecialCard = new System.Windows.Forms.CheckBox();
            this.grpSpecialCards = new System.Windows.Forms.GroupBox();
            this.txtSpecialCard2 = new System.Windows.Forms.MaskedTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSpecialCard1 = new System.Windows.Forms.MaskedTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.grpWEBEnabled.SuspendLayout();
            this.grpWEBUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.grpIP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.grpWEB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHttpPort)).BeginInit();
            this.grpSuperCards.SuspendLayout();
            this.grpSpecialCards.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpWEBEnabled
            // 
            this.grpWEBEnabled.Controls.Add(this.optWEBEnabled);
            this.grpWEBEnabled.Controls.Add(this.optWEBDisable);
            resources.ApplyResources(this.grpWEBEnabled, "grpWEBEnabled");
            this.grpWEBEnabled.ForeColor = System.Drawing.Color.White;
            this.grpWEBEnabled.Name = "grpWEBEnabled";
            this.grpWEBEnabled.TabStop = false;
            // 
            // optWEBEnabled
            // 
            resources.ApplyResources(this.optWEBEnabled, "optWEBEnabled");
            this.optWEBEnabled.Checked = true;
            this.optWEBEnabled.ForeColor = System.Drawing.Color.White;
            this.optWEBEnabled.Name = "optWEBEnabled";
            this.optWEBEnabled.TabStop = true;
            this.optWEBEnabled.UseVisualStyleBackColor = true;
            this.optWEBEnabled.CheckedChanged += new System.EventHandler(this.optWEBEnabled_CheckedChanged);
            // 
            // optWEBDisable
            // 
            resources.ApplyResources(this.optWEBDisable, "optWEBDisable");
            this.optWEBDisable.ForeColor = System.Drawing.Color.White;
            this.optWEBDisable.Name = "optWEBDisable";
            this.optWEBDisable.UseVisualStyleBackColor = true;
            // 
            // grpWEBUsers
            // 
            this.grpWEBUsers.Controls.Add(this.txtUsersFile);
            this.grpWEBUsers.Controls.Add(this.dataGridView3);
            this.grpWEBUsers.Controls.Add(this.chkAutoUploadWEBUsers);
            this.grpWEBUsers.Controls.Add(this.cboLanguage2);
            this.grpWEBUsers.Controls.Add(this.label12);
            this.grpWEBUsers.Controls.Add(this.btnEditUsers);
            this.grpWEBUsers.Controls.Add(this.btnDownloadUsers);
            this.grpWEBUsers.Controls.Add(this.btnSelectUserFile);
            this.grpWEBUsers.Controls.Add(this.btnuploadUser);
            this.grpWEBUsers.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpWEBUsers, "grpWEBUsers");
            this.grpWEBUsers.Name = "grpWEBUsers";
            this.grpWEBUsers.TabStop = false;
            // 
            // txtUsersFile
            // 
            resources.ApplyResources(this.txtUsersFile, "txtUsersFile");
            this.txtUsersFile.Name = "txtUsersFile";
            this.txtUsersFile.ReadOnly = true;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridView3, "dataGridView3");
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            // 
            // chkAutoUploadWEBUsers
            // 
            resources.ApplyResources(this.chkAutoUploadWEBUsers, "chkAutoUploadWEBUsers");
            this.chkAutoUploadWEBUsers.ForeColor = System.Drawing.Color.White;
            this.chkAutoUploadWEBUsers.Name = "chkAutoUploadWEBUsers";
            this.chkAutoUploadWEBUsers.UseVisualStyleBackColor = true;
            // 
            // cboLanguage2
            // 
            this.cboLanguage2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage2.FormattingEnabled = true;
            this.cboLanguage2.Items.AddRange(new object[] {
            resources.GetString("cboLanguage2.Items"),
            resources.GetString("cboLanguage2.Items1"),
            resources.GetString("cboLanguage2.Items2")});
            resources.ApplyResources(this.cboLanguage2, "cboLanguage2");
            this.cboLanguage2.Name = "cboLanguage2";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Name = "label12";
            // 
            // btnEditUsers
            // 
            this.btnEditUsers.BackColor = System.Drawing.Color.Transparent;
            this.btnEditUsers.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnEditUsers, "btnEditUsers");
            this.btnEditUsers.ForeColor = System.Drawing.Color.White;
            this.btnEditUsers.Name = "btnEditUsers";
            this.btnEditUsers.UseVisualStyleBackColor = false;
            this.btnEditUsers.Click += new System.EventHandler(this.btnEditUsers_Click);
            // 
            // btnDownloadUsers
            // 
            this.btnDownloadUsers.BackColor = System.Drawing.Color.Transparent;
            this.btnDownloadUsers.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnDownloadUsers, "btnDownloadUsers");
            this.btnDownloadUsers.ForeColor = System.Drawing.Color.White;
            this.btnDownloadUsers.Name = "btnDownloadUsers";
            this.btnDownloadUsers.UseVisualStyleBackColor = false;
            this.btnDownloadUsers.Click += new System.EventHandler(this.btnDownloadUsers_Click);
            // 
            // btnSelectUserFile
            // 
            this.btnSelectUserFile.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectUserFile.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnSelectUserFile, "btnSelectUserFile");
            this.btnSelectUserFile.ForeColor = System.Drawing.Color.White;
            this.btnSelectUserFile.Name = "btnSelectUserFile";
            this.btnSelectUserFile.UseVisualStyleBackColor = false;
            this.btnSelectUserFile.Click += new System.EventHandler(this.btnSelectUserFile_Click);
            // 
            // btnuploadUser
            // 
            this.btnuploadUser.BackColor = System.Drawing.Color.Transparent;
            this.btnuploadUser.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnuploadUser, "btnuploadUser");
            this.btnuploadUser.ForeColor = System.Drawing.Color.White;
            this.btnuploadUser.Name = "btnuploadUser";
            this.btnuploadUser.UseVisualStyleBackColor = false;
            this.btnuploadUser.Click += new System.EventHandler(this.btnuploadUser_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // txtf_ControllerSN
            // 
            resources.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
            this.txtf_ControllerSN.Name = "txtf_ControllerSN";
            this.txtf_ControllerSN.ReadOnly = true;
            this.txtf_ControllerSN.TabStop = false;
            // 
            // txtf_MACAddr
            // 
            resources.ApplyResources(this.txtf_MACAddr, "txtf_MACAddr");
            this.txtf_MACAddr.Name = "txtf_MACAddr";
            this.txtf_MACAddr.ReadOnly = true;
            this.txtf_MACAddr.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // txtf_IP
            // 
            resources.ApplyResources(this.txtf_IP, "txtf_IP");
            this.txtf_IP.Name = "txtf_IP";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // txtf_mask
            // 
            resources.ApplyResources(this.txtf_mask, "txtf_mask");
            this.txtf_mask.Name = "txtf_mask";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // txtf_gateway
            // 
            resources.ApplyResources(this.txtf_gateway, "txtf_gateway");
            this.txtf_gateway.Name = "txtf_gateway";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnOption
            // 
            this.btnOption.BackColor = System.Drawing.Color.Transparent;
            this.btnOption.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnOption, "btnOption");
            this.btnOption.ForeColor = System.Drawing.Color.White;
            this.btnOption.Name = "btnOption";
            this.btnOption.TabStop = false;
            this.btnOption.UseVisualStyleBackColor = false;
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // lblPort
            // 
            resources.ApplyResources(this.lblPort, "lblPort");
            this.lblPort.ForeColor = System.Drawing.Color.White;
            this.lblPort.Name = "lblPort";
            // 
            // grpIP
            // 
            this.grpIP.BackColor = System.Drawing.Color.Transparent;
            this.grpIP.Controls.Add(this.nudPort);
            this.grpIP.Controls.Add(this.lblPort);
            this.grpIP.Controls.Add(this.label3);
            this.grpIP.Controls.Add(this.txtf_gateway);
            this.grpIP.Controls.Add(this.label5);
            this.grpIP.Controls.Add(this.txtf_IP);
            this.grpIP.Controls.Add(this.txtf_mask);
            this.grpIP.Controls.Add(this.label4);
            this.grpIP.Controls.Add(this.btnOption);
            resources.ApplyResources(this.grpIP, "grpIP");
            this.grpIP.Name = "grpIP";
            this.grpIP.TabStop = false;
            // 
            // nudPort
            // 
            resources.ApplyResources(this.nudPort, "nudPort");
            this.nudPort.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.TabStop = false;
            this.nudPort.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // chkEditIP
            // 
            resources.ApplyResources(this.chkEditIP, "chkEditIP");
            this.chkEditIP.ForeColor = System.Drawing.Color.White;
            this.chkEditIP.Name = "chkEditIP";
            this.chkEditIP.UseVisualStyleBackColor = true;
            this.chkEditIP.CheckedChanged += new System.EventHandler(this.chkEditIP_CheckedChanged);
            // 
            // grpWEB
            // 
            this.grpWEB.Controls.Add(this.chkWebOnlyQuery);
            this.grpWEB.Controls.Add(this.cboDateFormat);
            this.grpWEB.Controls.Add(this.label6);
            this.grpWEB.Controls.Add(this.btnOptionWEB);
            this.grpWEB.Controls.Add(this.lblHttpPort);
            this.grpWEB.Controls.Add(this.nudHttpPort);
            this.grpWEB.Controls.Add(this.cboLanguage);
            this.grpWEB.Controls.Add(this.label8);
            this.grpWEB.Controls.Add(this.btnSelectFile);
            this.grpWEB.Controls.Add(this.btnOtherLanguage);
            this.grpWEB.Controls.Add(this.txtSelectedFileName);
            resources.ApplyResources(this.grpWEB, "grpWEB");
            this.grpWEB.Name = "grpWEB";
            this.grpWEB.TabStop = false;
            // 
            // chkWebOnlyQuery
            // 
            resources.ApplyResources(this.chkWebOnlyQuery, "chkWebOnlyQuery");
            this.chkWebOnlyQuery.ForeColor = System.Drawing.Color.White;
            this.chkWebOnlyQuery.Name = "chkWebOnlyQuery";
            this.chkWebOnlyQuery.UseVisualStyleBackColor = true;
            // 
            // cboDateFormat
            // 
            this.cboDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDateFormat.FormattingEnabled = true;
            this.cboDateFormat.Items.AddRange(new object[] {
            resources.GetString("cboDateFormat.Items"),
            resources.GetString("cboDateFormat.Items1"),
            resources.GetString("cboDateFormat.Items2"),
            resources.GetString("cboDateFormat.Items3"),
            resources.GetString("cboDateFormat.Items4"),
            resources.GetString("cboDateFormat.Items5")});
            resources.ApplyResources(this.cboDateFormat, "cboDateFormat");
            this.cboDateFormat.Name = "cboDateFormat";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // btnOptionWEB
            // 
            this.btnOptionWEB.BackColor = System.Drawing.Color.Transparent;
            this.btnOptionWEB.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnOptionWEB, "btnOptionWEB");
            this.btnOptionWEB.ForeColor = System.Drawing.Color.White;
            this.btnOptionWEB.Name = "btnOptionWEB";
            this.btnOptionWEB.TabStop = true;
            this.btnOptionWEB.UseVisualStyleBackColor = false;
            this.btnOptionWEB.Click += new System.EventHandler(this.btnOptionWEB_Click);
            // 
            // lblHttpPort
            // 
            resources.ApplyResources(this.lblHttpPort, "lblHttpPort");
            this.lblHttpPort.BackColor = System.Drawing.Color.Transparent;
            this.lblHttpPort.ForeColor = System.Drawing.Color.White;
            this.lblHttpPort.Name = "lblHttpPort";
            // 
            // nudHttpPort
            // 
            resources.ApplyResources(this.nudHttpPort, "nudHttpPort");
            this.nudHttpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudHttpPort.Name = "nudHttpPort";
            this.nudHttpPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Items.AddRange(new object[] {
            resources.GetString("cboLanguage.Items"),
            resources.GetString("cboLanguage.Items1"),
            resources.GetString("cboLanguage.Items2")});
            resources.ApplyResources(this.cboLanguage, "cboLanguage");
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Name = "label8";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectFile.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnSelectFile, "btnSelectFile");
            this.btnSelectFile.ForeColor = System.Drawing.Color.White;
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.UseVisualStyleBackColor = false;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnOtherLanguage
            // 
            this.btnOtherLanguage.BackColor = System.Drawing.Color.Transparent;
            this.btnOtherLanguage.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnOtherLanguage, "btnOtherLanguage");
            this.btnOtherLanguage.ForeColor = System.Drawing.Color.White;
            this.btnOtherLanguage.Name = "btnOtherLanguage";
            this.btnOtherLanguage.UseVisualStyleBackColor = false;
            this.btnOtherLanguage.Click += new System.EventHandler(this.btnOtherLanguage_Click);
            // 
            // txtSelectedFileName
            // 
            resources.ApplyResources(this.txtSelectedFileName, "txtSelectedFileName");
            this.txtSelectedFileName.Name = "txtSelectedFileName";
            this.txtSelectedFileName.ReadOnly = true;
            // 
            // chkUpdateWebSet
            // 
            resources.ApplyResources(this.chkUpdateWebSet, "chkUpdateWebSet");
            this.chkUpdateWebSet.ForeColor = System.Drawing.Color.White;
            this.chkUpdateWebSet.Name = "chkUpdateWebSet";
            this.chkUpdateWebSet.UseVisualStyleBackColor = true;
            this.chkUpdateWebSet.CheckedChanged += new System.EventHandler(this.chkUpdateWebSet_CheckedChanged);
            // 
            // chkUpdateSuperCard
            // 
            resources.ApplyResources(this.chkUpdateSuperCard, "chkUpdateSuperCard");
            this.chkUpdateSuperCard.ForeColor = System.Drawing.Color.White;
            this.chkUpdateSuperCard.Name = "chkUpdateSuperCard";
            this.chkUpdateSuperCard.UseVisualStyleBackColor = true;
            this.chkUpdateSuperCard.CheckedChanged += new System.EventHandler(this.chkUpdateSuperCard_CheckedChanged);
            // 
            // grpSuperCards
            // 
            this.grpSuperCards.Controls.Add(this.label9);
            this.grpSuperCards.Controls.Add(this.txtSuperCard2);
            this.grpSuperCards.Controls.Add(this.label10);
            this.grpSuperCards.Controls.Add(this.txtSuperCard1);
            this.grpSuperCards.Controls.Add(this.label11);
            resources.ApplyResources(this.grpSuperCards, "grpSuperCards");
            this.grpSuperCards.ForeColor = System.Drawing.Color.White;
            this.grpSuperCards.Name = "grpSuperCards";
            this.grpSuperCards.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Name = "label9";
            // 
            // txtSuperCard2
            // 
            resources.ApplyResources(this.txtSuperCard2, "txtSuperCard2");
            this.txtSuperCard2.Name = "txtSuperCard2";
            this.txtSuperCard2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuperCard2_KeyPress);
            this.txtSuperCard2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSuperCard2_KeyUp);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Name = "label10";
            // 
            // txtSuperCard1
            // 
            resources.ApplyResources(this.txtSuperCard1, "txtSuperCard1");
            this.txtSuperCard1.Name = "txtSuperCard1";
            this.txtSuperCard1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuperCard1_KeyPress);
            this.txtSuperCard1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSuperCard1_KeyUp);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Name = "label11";
            // 
            // chkAdjustTime
            // 
            resources.ApplyResources(this.chkAdjustTime, "chkAdjustTime");
            this.chkAdjustTime.ForeColor = System.Drawing.Color.White;
            this.chkAdjustTime.Name = "chkAdjustTime";
            this.chkAdjustTime.UseVisualStyleBackColor = true;
            // 
            // btnTryWEB
            // 
            this.btnTryWEB.BackColor = System.Drawing.Color.Transparent;
            this.btnTryWEB.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnTryWEB, "btnTryWEB");
            this.btnTryWEB.ForeColor = System.Drawing.Color.White;
            this.btnTryWEB.Image = global::Properties.Resources.web;
            this.btnTryWEB.Name = "btnTryWEB";
            this.btnTryWEB.UseVisualStyleBackColor = false;
            this.btnTryWEB.Click += new System.EventHandler(this.btnTryWEB_Click);
            // 
            // btnRestoreNameAndPassword
            // 
            this.btnRestoreNameAndPassword.BackColor = System.Drawing.Color.Transparent;
            this.btnRestoreNameAndPassword.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnRestoreNameAndPassword, "btnRestoreNameAndPassword");
            this.btnRestoreNameAndPassword.ForeColor = System.Drawing.Color.White;
            this.btnRestoreNameAndPassword.Name = "btnRestoreNameAndPassword";
            this.btnRestoreNameAndPassword.UseVisualStyleBackColor = false;
            this.btnRestoreNameAndPassword.Click += new System.EventHandler(this.btnRestoreNameAndPassword_Click);
            // 
            // chkUpdateSpecialCard
            // 
            resources.ApplyResources(this.chkUpdateSpecialCard, "chkUpdateSpecialCard");
            this.chkUpdateSpecialCard.ForeColor = System.Drawing.Color.White;
            this.chkUpdateSpecialCard.Name = "chkUpdateSpecialCard";
            this.chkUpdateSpecialCard.UseVisualStyleBackColor = true;
            this.chkUpdateSpecialCard.CheckedChanged += new System.EventHandler(this.chkUpdateSpecialCard_CheckedChanged);
            // 
            // grpSpecialCards
            // 
            this.grpSpecialCards.Controls.Add(this.txtSpecialCard2);
            this.grpSpecialCards.Controls.Add(this.label13);
            this.grpSpecialCards.Controls.Add(this.txtSpecialCard1);
            this.grpSpecialCards.Controls.Add(this.label14);
            resources.ApplyResources(this.grpSpecialCards, "grpSpecialCards");
            this.grpSpecialCards.ForeColor = System.Drawing.Color.White;
            this.grpSpecialCards.Name = "grpSpecialCards";
            this.grpSpecialCards.TabStop = false;
            // 
            // txtSpecialCard2
            // 
            resources.ApplyResources(this.txtSpecialCard2, "txtSpecialCard2");
            this.txtSpecialCard2.Name = "txtSpecialCard2";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Name = "label13";
            // 
            // txtSpecialCard1
            // 
            resources.ApplyResources(this.txtSpecialCard1, "txtSpecialCard1");
            this.txtSpecialCard1.Name = "txtSpecialCard1";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Name = "label14";
            // 
            // dfrmTCPIPWEBConfigure
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.chkUpdateSpecialCard);
            this.Controls.Add(this.grpSpecialCards);
            this.Controls.Add(this.btnRestoreNameAndPassword);
            this.Controls.Add(this.btnTryWEB);
            this.Controls.Add(this.chkAdjustTime);
            this.Controls.Add(this.grpWEBEnabled);
            this.Controls.Add(this.grpWEBUsers);
            this.Controls.Add(this.chkUpdateSuperCard);
            this.Controls.Add(this.grpSuperCards);
            this.Controls.Add(this.chkUpdateWebSet);
            this.Controls.Add(this.grpWEB);
            this.Controls.Add(this.chkEditIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grpIP);
            this.Controls.Add(this.txtf_ControllerSN);
            this.Controls.Add(this.txtf_MACAddr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmTCPIPWEBConfigure";
            this.Load += new System.EventHandler(this.dfrmTCPIPConfigure_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmTCPIPWEBConfigure_KeyDown);
            this.grpWEBEnabled.ResumeLayout(false);
            this.grpWEBEnabled.PerformLayout();
            this.grpWEBUsers.ResumeLayout(false);
            this.grpWEBUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.grpIP.ResumeLayout(false);
            this.grpIP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.grpWEB.ResumeLayout(false);
            this.grpWEB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHttpPort)).EndInit();
            this.grpSuperCards.ResumeLayout(false);
            this.grpSuperCards.PerformLayout();
            this.grpSpecialCards.ResumeLayout(false);
            this.grpSpecialCards.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

