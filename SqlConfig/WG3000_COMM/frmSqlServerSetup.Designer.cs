namespace WG3000_COMM
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class frmSqlServerSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSqlServerSetup));
            this.Label2 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPasswd = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.optWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.optSqlAuthentication = new System.Windows.Forms.RadioButton();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtDBFile = new System.Windows.Forms.TextBox();
            this.txtDBLogFile = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.cboDBs = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.chkSelectDirectory = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblRunInfo = new System.Windows.Forms.Label();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.btnUpgradeMsAccessToSqlServer = new System.Windows.Forms.ImageButton();
            this.btnAdvanced = new System.Windows.Forms.ImageButton();
            this.btnUseMsAccessDatabase = new System.Windows.Forms.ImageButton();
            this.btnBackupDB = new System.Windows.Forms.ImageButton();
            this.btnRestoreDB = new System.Windows.Forms.ImageButton();
            this.btnCheckDBVersion = new System.Windows.Forms.ImageButton();
            this.btnConfirm = new System.Windows.Forms.ImageButton();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnCreateDB = new System.Windows.Forms.ImageButton();
            this.cmdConnectTest = new System.Windows.Forms.ImageButton();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Name = "Label2";
            // 
            // txtServer
            // 
            resources.ApplyResources(this.txtServer, "txtServer");
            this.txtServer.Name = "txtServer";
            // 
            // txtPasswd
            // 
            resources.ApplyResources(this.txtPasswd, "txtPasswd");
            this.txtPasswd.Name = "txtPasswd";
            // 
            // txtUsername
            // 
            resources.ApplyResources(this.txtUsername, "txtUsername");
            this.txtUsername.Name = "txtUsername";
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.Name = "Label3";
            // 
            // Label4
            // 
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.Name = "Label4";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // optWindowsAuthentication
            // 
            this.optWindowsAuthentication.BackColor = System.Drawing.Color.Transparent;
            this.optWindowsAuthentication.Checked = true;
            this.optWindowsAuthentication.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.optWindowsAuthentication, "optWindowsAuthentication");
            this.optWindowsAuthentication.Name = "optWindowsAuthentication";
            this.optWindowsAuthentication.TabStop = true;
            this.optWindowsAuthentication.UseVisualStyleBackColor = false;
            // 
            // optSqlAuthentication
            // 
            this.optSqlAuthentication.BackColor = System.Drawing.Color.Transparent;
            this.optSqlAuthentication.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.optSqlAuthentication, "optSqlAuthentication");
            this.optSqlAuthentication.Name = "optSqlAuthentication";
            this.optSqlAuthentication.TabStop = true;
            this.optSqlAuthentication.UseVisualStyleBackColor = false;
            this.optSqlAuthentication.CheckedChanged += new System.EventHandler(this.optSqlAuthentication_CheckedChanged);
            // 
            // Label5
            // 
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // Label6
            // 
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Name = "Label6";
            // 
            // txtDBName
            // 
            resources.ApplyResources(this.txtDBName, "txtDBName");
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.TextChanged += new System.EventHandler(this.txtDBName_TextChanged);
            // 
            // txtDBFile
            // 
            resources.ApplyResources(this.txtDBFile, "txtDBFile");
            this.txtDBFile.Name = "txtDBFile";
            this.txtDBFile.ReadOnly = true;
            // 
            // txtDBLogFile
            // 
            resources.ApplyResources(this.txtDBLogFile, "txtDBLogFile");
            this.txtDBLogFile.Name = "txtDBLogFile";
            this.txtDBLogFile.ReadOnly = true;
            // 
            // Label7
            // 
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label7.Name = "Label7";
            // 
            // cboDBs
            // 
            this.cboDBs.FormattingEnabled = true;
            resources.ApplyResources(this.cboDBs, "cboDBs");
            this.cboDBs.Name = "cboDBs";
            this.cboDBs.SelectedIndexChanged += new System.EventHandler(this.cboDBs_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chkSelectDirectory
            // 
            resources.ApplyResources(this.chkSelectDirectory, "chkSelectDirectory");
            this.chkSelectDirectory.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectDirectory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkSelectDirectory.Name = "chkSelectDirectory";
            this.chkSelectDirectory.UseVisualStyleBackColor = false;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // lblRunInfo
            // 
            this.lblRunInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblRunInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblRunInfo, "lblRunInfo");
            this.lblRunInfo.Name = "lblRunInfo";
            // 
            // GroupBox3
            // 
            resources.ApplyResources(this.GroupBox3, "GroupBox3");
            this.GroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.TabStop = false;
            // 
            // GroupBox1
            // 
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // GroupBox2
            // 
            resources.ApplyResources(this.GroupBox2, "GroupBox2");
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.ForeColor = System.Drawing.Color.White;
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.TabStop = false;
            // 
            // btnUpgradeMsAccessToSqlServer
            // 
            this.btnUpgradeMsAccessToSqlServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnUpgradeMsAccessToSqlServer, "btnUpgradeMsAccessToSqlServer");
            this.btnUpgradeMsAccessToSqlServer.Focusable = true;
            this.btnUpgradeMsAccessToSqlServer.ForeColor = System.Drawing.Color.White;
            this.btnUpgradeMsAccessToSqlServer.Name = "btnUpgradeMsAccessToSqlServer";
            this.btnUpgradeMsAccessToSqlServer.UseVisualStyleBackColor = false;
            this.btnUpgradeMsAccessToSqlServer.Click += new System.EventHandler(this.btnUpgradeMsAccessToSqlServer_Click);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAdvanced, "btnAdvanced");
            this.btnAdvanced.Focusable = true;
            this.btnAdvanced.ForeColor = System.Drawing.Color.White;
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.UseVisualStyleBackColor = false;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // btnUseMsAccessDatabase
            // 
            this.btnUseMsAccessDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnUseMsAccessDatabase, "btnUseMsAccessDatabase");
            this.btnUseMsAccessDatabase.Focusable = true;
            this.btnUseMsAccessDatabase.ForeColor = System.Drawing.Color.White;
            this.btnUseMsAccessDatabase.Name = "btnUseMsAccessDatabase";
            this.btnUseMsAccessDatabase.UseVisualStyleBackColor = false;
            this.btnUseMsAccessDatabase.Click += new System.EventHandler(this.btnUseMsAccessDatabase_Click);
            // 
            // btnBackupDB
            // 
            this.btnBackupDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnBackupDB, "btnBackupDB");
            this.btnBackupDB.Focusable = true;
            this.btnBackupDB.ForeColor = System.Drawing.Color.White;
            this.btnBackupDB.Name = "btnBackupDB";
            this.btnBackupDB.UseVisualStyleBackColor = false;
            this.btnBackupDB.Click += new System.EventHandler(this.btnBackupDB_Click);
            // 
            // btnRestoreDB
            // 
            this.btnRestoreDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRestoreDB, "btnRestoreDB");
            this.btnRestoreDB.Focusable = true;
            this.btnRestoreDB.ForeColor = System.Drawing.Color.White;
            this.btnRestoreDB.Name = "btnRestoreDB";
            this.btnRestoreDB.UseVisualStyleBackColor = false;
            this.btnRestoreDB.Click += new System.EventHandler(this.btnRestoreDB_Click);
            // 
            // btnCheckDBVersion
            // 
            this.btnCheckDBVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnCheckDBVersion, "btnCheckDBVersion");
            this.btnCheckDBVersion.Focusable = true;
            this.btnCheckDBVersion.ForeColor = System.Drawing.Color.White;
            this.btnCheckDBVersion.Name = "btnCheckDBVersion";
            this.btnCheckDBVersion.UseVisualStyleBackColor = false;
            this.btnCheckDBVersion.Click += new System.EventHandler(this.btnCheckDBVersion_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.Focusable = true;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCreateDB
            // 
            this.btnCreateDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnCreateDB, "btnCreateDB");
            this.btnCreateDB.Focusable = true;
            this.btnCreateDB.ForeColor = System.Drawing.Color.White;
            this.btnCreateDB.Name = "btnCreateDB";
            this.btnCreateDB.UseVisualStyleBackColor = false;
            this.btnCreateDB.Click += new System.EventHandler(this.btnCreateDB_Click);
            // 
            // cmdConnectTest
            // 
            this.cmdConnectTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.cmdConnectTest, "cmdConnectTest");
            this.cmdConnectTest.Focusable = true;
            this.cmdConnectTest.ForeColor = System.Drawing.Color.White;
            this.cmdConnectTest.Name = "cmdConnectTest";
            this.cmdConnectTest.UseVisualStyleBackColor = false;
            this.cmdConnectTest.Click += new System.EventHandler(this.cmdConnectTest_Click);
            // 
            // frmSqlServerSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(231)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblRunInfo);
            this.Controls.Add(this.btnUpgradeMsAccessToSqlServer);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnAdvanced);
            this.Controls.Add(this.btnUseMsAccessDatabase);
            this.Controls.Add(this.chkSelectDirectory);
            this.Controls.Add(this.btnBackupDB);
            this.Controls.Add(this.btnRestoreDB);
            this.Controls.Add(this.cboDBs);
            this.Controls.Add(this.btnCheckDBVersion);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.txtDBLogFile);
            this.Controls.Add(this.txtDBFile);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCreateDB);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.optSqlAuthentication);
            this.Controls.Add(this.optWindowsAuthentication);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPasswd);
            this.Controls.Add(this.cmdConnectTest);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSqlServerSetup";
            this.Load += new System.EventHandler(this.frmSqlServerSetup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSqlServerSetup_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.ImageButton cmdConnectTest;
        private System.Windows.Forms.TextBox txtPasswd;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.RadioButton optWindowsAuthentication;
        private System.Windows.Forms.RadioButton optSqlAuthentication;
        private System.Windows.Forms.ImageButton btnCreateDB;
        private System.Windows.Forms.ImageButton btnExit;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtDBFile;
        private System.Windows.Forms.TextBox txtDBLogFile;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.ImageButton btnConfirm;
        private System.Windows.Forms.ImageButton btnCheckDBVersion;
        private System.Windows.Forms.ComboBox cboDBs;
        private System.Windows.Forms.ImageButton btnRestoreDB;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ImageButton btnBackupDB;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chkSelectDirectory;
        private System.Windows.Forms.ImageButton btnUseMsAccessDatabase;
        private System.Windows.Forms.ImageButton btnAdvanced;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ImageButton btnUpgradeMsAccessToSqlServer;
        private System.Windows.Forms.Label lblRunInfo;
        private GroupBox GroupBox3;
        private GroupBox GroupBox1;
        private GroupBox GroupBox2;
    }
}