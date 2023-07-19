namespace WG3000_COMM.ExtendFunc
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

    public partial class dfrmControllerWarnSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerWarnSet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblThreat = new System.Windows.Forms.Label();
            this.lblSec1 = new System.Windows.Forms.Label();
            this.lblSec2 = new System.Windows.Forms.Label();
            this.lblAlarmTime = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.nudOpenDoorTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblOpenDoorTimeout = new System.Windows.Forms.Label();
            this.lblThreatPassword = new System.Windows.Forms.Label();
            this.btnChangeThreatPassword = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnExtension = new System.Windows.Forms.ImageButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.f_ControllerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_InterLock123 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_InterLock34 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_ForcedOpen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_InterLock1234 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_DoorTamperWarn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Doors = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkActiveFireSignalShare = new System.Windows.Forms.CheckBox();
            this.chkGrouped = new System.Windows.Forms.CheckBox();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpenDoorTimeout)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.lblThreat);
            this.groupBox6.Controls.Add(this.lblSec1);
            this.groupBox6.Controls.Add(this.lblSec2);
            this.groupBox6.Controls.Add(this.lblAlarmTime);
            this.groupBox6.Controls.Add(this.nudDelay);
            this.groupBox6.Controls.Add(this.nudOpenDoorTimeout);
            this.groupBox6.Controls.Add(this.lblOpenDoorTimeout);
            this.groupBox6.Controls.Add(this.lblThreatPassword);
            this.groupBox6.Controls.Add(this.btnChangeThreatPassword);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // lblThreat
            // 
            resources.ApplyResources(this.lblThreat, "lblThreat");
            this.lblThreat.BackColor = System.Drawing.Color.Transparent;
            this.lblThreat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblThreat.Name = "lblThreat";
            // 
            // lblSec1
            // 
            resources.ApplyResources(this.lblSec1, "lblSec1");
            this.lblSec1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblSec1.Name = "lblSec1";
            // 
            // lblSec2
            // 
            resources.ApplyResources(this.lblSec2, "lblSec2");
            this.lblSec2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblSec2.Name = "lblSec2";
            // 
            // lblAlarmTime
            // 
            this.lblAlarmTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblAlarmTime, "lblAlarmTime");
            this.lblAlarmTime.Name = "lblAlarmTime";
            // 
            // nudDelay
            // 
            resources.ApplyResources(this.nudDelay, "nudDelay");
            this.nudDelay.Maximum = new decimal(new int[] {
            6553,
            0,
            0,
            0});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nudDelay_KeyPress);
            this.nudDelay.Leave += new System.EventHandler(this.nudDelay_Leave);
            // 
            // nudOpenDoorTimeout
            // 
            resources.ApplyResources(this.nudOpenDoorTimeout, "nudOpenDoorTimeout");
            this.nudOpenDoorTimeout.BackColor = System.Drawing.Color.White;
            this.nudOpenDoorTimeout.Maximum = new decimal(new int[] {
            650,
            0,
            0,
            0});
            this.nudOpenDoorTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOpenDoorTimeout.Name = "nudOpenDoorTimeout";
            this.nudOpenDoorTimeout.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lblOpenDoorTimeout
            // 
            resources.ApplyResources(this.lblOpenDoorTimeout, "lblOpenDoorTimeout");
            this.lblOpenDoorTimeout.BackColor = System.Drawing.Color.Transparent;
            this.lblOpenDoorTimeout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblOpenDoorTimeout.Name = "lblOpenDoorTimeout";
            // 
            // lblThreatPassword
            // 
            resources.ApplyResources(this.lblThreatPassword, "lblThreatPassword");
            this.lblThreatPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblThreatPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblThreatPassword.Name = "lblThreatPassword";
            // 
            // btnChangeThreatPassword
            // 
            resources.ApplyResources(this.btnChangeThreatPassword, "btnChangeThreatPassword");
            this.btnChangeThreatPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnChangeThreatPassword.Focusable = true;
            this.btnChangeThreatPassword.ForeColor = System.Drawing.Color.White;
            this.btnChangeThreatPassword.Name = "btnChangeThreatPassword";
            this.btnChangeThreatPassword.Toggle = false;
            this.btnChangeThreatPassword.UseVisualStyleBackColor = false;
            this.btnChangeThreatPassword.Click += new System.EventHandler(this.btnChangeThreatPassword_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExtension
            // 
            resources.ApplyResources(this.btnExtension, "btnExtension");
            this.btnExtension.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnExtension.Focusable = true;
            this.btnExtension.ForeColor = System.Drawing.Color.White;
            this.btnExtension.Name = "btnExtension";
            this.btnExtension.Toggle = false;
            this.btnExtension.UseVisualStyleBackColor = false;
            this.btnExtension.Click += new System.EventHandler(this.btnExtension_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ControllerID,
            this.f_ControllerSN,
            this.f_InterLock123,
            this.f_InterLock34,
            this.f_ForcedOpen,
            this.f_InterLock1234,
            this.f_DoorTamperWarn,
            this.f_Doors});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // f_ControllerID
            // 
            resources.ApplyResources(this.f_ControllerID, "f_ControllerID");
            this.f_ControllerID.Name = "f_ControllerID";
            this.f_ControllerID.ReadOnly = true;
            // 
            // f_ControllerSN
            // 
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            // 
            // f_InterLock123
            // 
            resources.ApplyResources(this.f_InterLock123, "f_InterLock123");
            this.f_InterLock123.Name = "f_InterLock123";
            // 
            // f_InterLock34
            // 
            resources.ApplyResources(this.f_InterLock34, "f_InterLock34");
            this.f_InterLock34.Name = "f_InterLock34";
            // 
            // f_ForcedOpen
            // 
            resources.ApplyResources(this.f_ForcedOpen, "f_ForcedOpen");
            this.f_ForcedOpen.Name = "f_ForcedOpen";
            this.f_ForcedOpen.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_ForcedOpen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_InterLock1234
            // 
            resources.ApplyResources(this.f_InterLock1234, "f_InterLock1234");
            this.f_InterLock1234.Name = "f_InterLock1234";
            // 
            // f_DoorTamperWarn
            // 
            resources.ApplyResources(this.f_DoorTamperWarn, "f_DoorTamperWarn");
            this.f_DoorTamperWarn.Name = "f_DoorTamperWarn";
            this.f_DoorTamperWarn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_DoorTamperWarn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_Doors
            // 
            this.f_Doors.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_Doors, "f_Doors");
            this.f_Doors.Name = "f_Doors";
            this.f_Doors.ReadOnly = true;
            // 
            // chkActiveFireSignalShare
            // 
            resources.ApplyResources(this.chkActiveFireSignalShare, "chkActiveFireSignalShare");
            this.chkActiveFireSignalShare.BackColor = System.Drawing.Color.Transparent;
            this.chkActiveFireSignalShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkActiveFireSignalShare.Name = "chkActiveFireSignalShare";
            this.chkActiveFireSignalShare.UseVisualStyleBackColor = false;
            this.chkActiveFireSignalShare.CheckedChanged += new System.EventHandler(this.chkActiveFireSignalShare_CheckedChanged);
            // 
            // chkGrouped
            // 
            resources.ApplyResources(this.chkGrouped, "chkGrouped");
            this.chkGrouped.BackColor = System.Drawing.Color.Transparent;
            this.chkGrouped.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkGrouped.Name = "chkGrouped";
            this.chkGrouped.UseVisualStyleBackColor = false;
            // 
            // dfrmControllerWarnSet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.btnExtension);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.chkActiveFireSignalShare);
            this.Controls.Add(this.chkGrouped);
            this.MinimizeBox = false;
            this.Name = "dfrmControllerWarnSet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerWarnSet_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerInterLock_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControllerWarnSet_KeyDown);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpenDoorTimeout)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton btnExtension;
        internal CheckBox chkActiveFireSignalShare;
        internal CheckBox chkGrouped;
        private DataGridView dataGridView1;
        private Panel panelBottomBanner;
        private ImageButton btnCancel;
        private ImageButton btnOK;
        internal GroupBox groupBox6;
        private Label lblThreat;
        private Label lblSec1;
        private Label lblSec2;
        private Label lblAlarmTime;
        private NumericUpDown nudDelay;
        private NumericUpDown nudOpenDoorTimeout;
        private Label lblOpenDoorTimeout;
        private Label lblThreatPassword;
        private ImageButton btnChangeThreatPassword;
        private DataGridViewTextBoxColumn f_ControllerID;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewCheckBoxColumn f_InterLock123;
        private DataGridViewCheckBoxColumn f_InterLock34;
        private DataGridViewCheckBoxColumn f_ForcedOpen;
        private DataGridViewCheckBoxColumn f_InterLock1234;
        private DataGridViewCheckBoxColumn f_DoorTamperWarn;
        private DataGridViewTextBoxColumn f_Doors;
    }
}

