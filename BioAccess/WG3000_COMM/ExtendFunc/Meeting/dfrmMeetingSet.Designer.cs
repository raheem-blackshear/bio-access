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
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMeetingSet : frmBioAccess
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmMeetingSet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtf_Notes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtf_Content = new System.Windows.Forms.TextBox();
            this.cbof_MeetingAdr = new System.Windows.Forms.ComboBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.btnAddMeetingAdr = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSeat = new System.Windows.Forms.TextBox();
            this.lblWait = new System.Windows.Forms.Label();
            this.cboIdentity = new System.Windows.Forms.ComboBox();
            this.lblControlTimeSeg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvSelectedUsers = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Identity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdentityStr2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MoreCards_GrpID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_SelectedGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Identity1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdentityStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeatNO1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_SelectedUsers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_GroupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelAllUsers = new System.Windows.Forms.ImageButton();
            this.btnDelOneUser = new System.Windows.Forms.ImageButton();
            this.btnAdd = new System.Windows.Forms.ImageButton();
            this.btnAddAll = new System.Windows.Forms.ImageButton();
            this.Label9 = new System.Windows.Forms.Label();
            this.cbof_GroupID = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtf_MeetingNo = new System.Windows.Forms.TextBox();
            this.lblMeetingName = new System.Windows.Forms.Label();
            this.txtf_MeetingName = new System.Windows.Forms.TextBox();
            this.lblMeetingAddr = new System.Windows.Forms.Label();
            this.lblMeetingDateTime = new System.Windows.Forms.Label();
            this.dtpMeetingDate = new System.Windows.Forms.DateTimePicker();
            this.dtpMeetingTime = new System.Windows.Forms.DateTimePicker();
            this.lblSignBegin = new System.Windows.Forms.Label();
            this.lblSignEnd = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.btnCreateInfo = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
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
            // txtf_Notes
            // 
            resources.ApplyResources(this.txtf_Notes, "txtf_Notes");
            this.txtf_Notes.Name = "txtf_Notes";
            // 
            // lblNotes
            // 
            this.lblNotes.BackColor = System.Drawing.Color.Transparent;
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblNotes, "lblNotes");
            this.lblNotes.Name = "lblNotes";
            // 
            // txtf_Content
            // 
            resources.ApplyResources(this.txtf_Content, "txtf_Content");
            this.txtf_Content.Name = "txtf_Content";
            // 
            // cbof_MeetingAdr
            // 
            this.cbof_MeetingAdr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_MeetingAdr, "cbof_MeetingAdr");
            this.cbof_MeetingAdr.Name = "cbof_MeetingAdr";
            // 
            // lblContent
            // 
            this.lblContent.BackColor = System.Drawing.Color.Transparent;
            this.lblContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblContent, "lblContent");
            this.lblContent.Name = "lblContent";
            // 
            // btnAddMeetingAdr
            // 
            this.btnAddMeetingAdr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddMeetingAdr, "btnAddMeetingAdr");
            this.btnAddMeetingAdr.Focusable = true;
            this.btnAddMeetingAdr.ForeColor = System.Drawing.Color.White;
            this.btnAddMeetingAdr.Name = "btnAddMeetingAdr";
            this.btnAddMeetingAdr.Toggle = false;
            this.btnAddMeetingAdr.UseVisualStyleBackColor = false;
            this.btnAddMeetingAdr.Click += new System.EventHandler(this.btnAddMeetingAdr_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.txtSeat);
            this.groupBox1.Controls.Add(this.lblWait);
            this.groupBox1.Controls.Add(this.cboIdentity);
            this.groupBox1.Controls.Add(this.lblControlTimeSeg);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dgvSelectedUsers);
            this.groupBox1.Controls.Add(this.dgvUsers);
            this.groupBox1.Controls.Add(this.btnDelAllUsers);
            this.groupBox1.Controls.Add(this.btnDelOneUser);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnAddAll);
            this.groupBox1.Controls.Add(this.Label9);
            this.groupBox1.Controls.Add(this.cbof_GroupID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtSeat
            // 
            this.txtSeat.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtSeat, "txtSeat");
            this.txtSeat.Name = "txtSeat";
            // 
            // lblWait
            // 
            resources.ApplyResources(this.lblWait, "lblWait");
            this.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWait.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblWait.Name = "lblWait";
            // 
            // cboIdentity
            // 
            this.cboIdentity.BackColor = System.Drawing.Color.White;
            this.cboIdentity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboIdentity, "cboIdentity");
            this.cboIdentity.Items.AddRange(new object[] {
            resources.GetString("cboIdentity.Items"),
            resources.GetString("cboIdentity.Items1"),
            resources.GetString("cboIdentity.Items2"),
            resources.GetString("cboIdentity.Items3"),
            resources.GetString("cboIdentity.Items4"),
            resources.GetString("cboIdentity.Items5")});
            this.cboIdentity.Name = "cboIdentity";
            // 
            // lblControlTimeSeg
            // 
            this.lblControlTimeSeg.BackColor = System.Drawing.Color.Transparent;
            this.lblControlTimeSeg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblControlTimeSeg, "lblControlTimeSeg");
            this.lblControlTimeSeg.Name = "lblControlTimeSeg";
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
            this.dgvSelectedUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Identity,
            this.IdentityStr2,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.f_MoreCards_GrpID,
            this.dataGridViewCheckBoxColumn1,
            this.f_SelectedGroup});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
            this.dgvSelectedUsers.Name = "dgvSelectedUsers";
            this.dgvSelectedUsers.ReadOnly = true;
            this.dgvSelectedUsers.RowTemplate.Height = 23;
            this.dgvSelectedUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedUsers.DoubleClick += new System.EventHandler(this.btnDelOneUser_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // Identity
            // 
            resources.ApplyResources(this.Identity, "Identity");
            this.Identity.Name = "Identity";
            this.Identity.ReadOnly = true;
            // 
            // IdentityStr2
            // 
            resources.ApplyResources(this.IdentityStr2, "IdentityStr2");
            this.IdentityStr2.Name = "IdentityStr2";
            this.IdentityStr2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
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
            // f_MoreCards_GrpID
            // 
            resources.ApplyResources(this.f_MoreCards_GrpID, "f_MoreCards_GrpID");
            this.f_MoreCards_GrpID.Name = "f_MoreCards_GrpID";
            this.f_MoreCards_GrpID.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // f_SelectedGroup
            // 
            resources.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
            this.f_SelectedGroup.Name = "f_SelectedGroup";
            this.f_SelectedGroup.ReadOnly = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvUsers, "dgvUsers");
            this.dgvUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsumerID,
            this.Identity1,
            this.IdentityStr,
            this.UserID,
            this.ConsumerName,
            this.CardNO,
            this.SeatNO1,
            this.f_SelectedUsers,
            this.f_GroupID});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.DoubleClick += new System.EventHandler(this.btnAdd_Click);
            // 
            // ConsumerID
            // 
            resources.ApplyResources(this.ConsumerID, "ConsumerID");
            this.ConsumerID.Name = "ConsumerID";
            this.ConsumerID.ReadOnly = true;
            // 
            // Identity1
            // 
            resources.ApplyResources(this.Identity1, "Identity1");
            this.Identity1.Name = "Identity1";
            this.Identity1.ReadOnly = true;
            // 
            // IdentityStr
            // 
            resources.ApplyResources(this.IdentityStr, "IdentityStr");
            this.IdentityStr.Name = "IdentityStr";
            this.IdentityStr.ReadOnly = true;
            // 
            // UserID
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.UserID.DefaultCellStyle = dataGridViewCellStyle5;
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
            // SeatNO1
            // 
            resources.ApplyResources(this.SeatNO1, "SeatNO1");
            this.SeatNO1.Name = "SeatNO1";
            this.SeatNO1.ReadOnly = true;
            // 
            // f_SelectedUsers
            // 
            resources.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
            this.f_SelectedUsers.Name = "f_SelectedUsers";
            this.f_SelectedUsers.ReadOnly = true;
            // 
            // f_GroupID
            // 
            resources.ApplyResources(this.f_GroupID, "f_GroupID");
            this.f_GroupID.Name = "f_GroupID";
            this.f_GroupID.ReadOnly = true;
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
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Focusable = true;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Toggle = false;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAll, "btnAddAll");
            this.btnAddAll.Focusable = true;
            this.btnAddAll.ForeColor = System.Drawing.Color.White;
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Toggle = false;
            this.btnAddAll.UseVisualStyleBackColor = false;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // Label9
            // 
            this.Label9.BackColor = System.Drawing.Color.Transparent;
            this.Label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label9, "Label9");
            this.Label9.Name = "Label9";
            // 
            // cbof_GroupID
            // 
            this.cbof_GroupID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_GroupID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
            this.cbof_GroupID.Name = "cbof_GroupID";
            this.cbof_GroupID.SelectedIndexChanged += new System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label4.Name = "label4";
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
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // txtf_MeetingNo
            // 
            resources.ApplyResources(this.txtf_MeetingNo, "txtf_MeetingNo");
            this.txtf_MeetingNo.Name = "txtf_MeetingNo";
            this.txtf_MeetingNo.ReadOnly = true;
            // 
            // lblMeetingName
            // 
            this.lblMeetingName.BackColor = System.Drawing.Color.Transparent;
            this.lblMeetingName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblMeetingName, "lblMeetingName");
            this.lblMeetingName.Name = "lblMeetingName";
            // 
            // txtf_MeetingName
            // 
            resources.ApplyResources(this.txtf_MeetingName, "txtf_MeetingName");
            this.txtf_MeetingName.Name = "txtf_MeetingName";
            // 
            // lblMeetingAddr
            // 
            this.lblMeetingAddr.BackColor = System.Drawing.Color.Transparent;
            this.lblMeetingAddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblMeetingAddr, "lblMeetingAddr");
            this.lblMeetingAddr.Name = "lblMeetingAddr";
            // 
            // lblMeetingDateTime
            // 
            this.lblMeetingDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblMeetingDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblMeetingDateTime, "lblMeetingDateTime");
            this.lblMeetingDateTime.Name = "lblMeetingDateTime";
            // 
            // dtpMeetingDate
            // 
            resources.ApplyResources(this.dtpMeetingDate, "dtpMeetingDate");
            this.dtpMeetingDate.Name = "dtpMeetingDate";
            this.dtpMeetingDate.Value = new System.DateTime(2008, 2, 21, 0, 0, 0, 0);
            // 
            // dtpMeetingTime
            // 
            resources.ApplyResources(this.dtpMeetingTime, "dtpMeetingTime");
            this.dtpMeetingTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpMeetingTime.Name = "dtpMeetingTime";
            this.dtpMeetingTime.ShowUpDown = true;
            this.dtpMeetingTime.Value = new System.DateTime(2008, 2, 21, 0, 0, 0, 0);
            // 
            // lblSignBegin
            // 
            this.lblSignBegin.BackColor = System.Drawing.Color.Transparent;
            this.lblSignBegin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblSignBegin, "lblSignBegin");
            this.lblSignBegin.Name = "lblSignBegin";
            // 
            // lblSignEnd
            // 
            this.lblSignEnd.BackColor = System.Drawing.Color.Transparent;
            this.lblSignEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblSignEnd, "lblSignEnd");
            this.lblSignEnd.Name = "lblSignEnd";
            // 
            // dtpStartTime
            // 
            resources.ApplyResources(this.dtpStartTime, "dtpStartTime");
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Value = new System.DateTime(2003, 3, 10, 0, 0, 0, 0);
            // 
            // dtpEndTime
            // 
            resources.ApplyResources(this.dtpEndTime, "dtpEndTime");
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Value = new System.DateTime(2003, 3, 10, 0, 0, 0, 0);
            // 
            // btnCreateInfo
            // 
            resources.ApplyResources(this.btnCreateInfo, "btnCreateInfo");
            this.btnCreateInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCreateInfo.Focusable = true;
            this.btnCreateInfo.ForeColor = System.Drawing.Color.White;
            this.btnCreateInfo.Name = "btnCreateInfo";
            this.btnCreateInfo.Toggle = false;
            this.btnCreateInfo.UseVisualStyleBackColor = false;
            this.btnCreateInfo.Click += new System.EventHandler(this.btnCreateInfo_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCreateInfo);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmMeetingSet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.txtf_Notes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtf_Content);
            this.Controls.Add(this.cbof_MeetingAdr);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.btnAddMeetingAdr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMeetingName);
            this.Controls.Add(this.txtf_MeetingName);
            this.Controls.Add(this.lblMeetingAddr);
            this.Controls.Add(this.lblMeetingDateTime);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.dtpMeetingDate);
            this.Controls.Add(this.txtf_MeetingNo);
            this.Controls.Add(this.dtpMeetingTime);
            this.Controls.Add(this.lblSignBegin);
            this.Controls.Add(this.dtpEndTime);
            this.Controls.Add(this.lblSignEnd);
            this.Controls.Add(this.dtpStartTime);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmMeetingSet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmMeetingSet_FormClosing);
            this.Load += new System.EventHandler(this.dfrmMeetingSet_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmMeetingSet_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ImageButton btnAdd;
        private ImageButton btnAddAll;
        internal ImageButton btnAddMeetingAdr;
        internal ImageButton btnCancel;
        internal ImageButton btnCreateInfo;
        private ImageButton btnDelAllUsers;
        private ImageButton btnDelOneUser;
        internal ImageButton btnOK;
        private DataGridViewTextBoxColumn CardNO;
        private ComboBox cbof_GroupID;
        internal ComboBox cbof_MeetingAdr;
        internal ComboBox cboIdentity;
        private DataGridViewTextBoxColumn ConsumerID;
        private DataGridViewTextBoxColumn ConsumerName;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView dgvSelectedUsers;
        private DataGridView dgvUsers;
        internal DateTimePicker dtpEndTime;
        internal DateTimePicker dtpMeetingDate;
        internal DateTimePicker dtpMeetingTime;
        internal DateTimePicker dtpStartTime;
        private DataGridViewTextBoxColumn f_GroupID;
        private DataGridViewTextBoxColumn f_MoreCards_GrpID;
        private DataGridViewTextBoxColumn f_SelectedGroup;
        private DataGridViewCheckBoxColumn f_SelectedUsers;
        private System.Windows.Forms.GroupBox groupBox1;
        private DataGridViewTextBoxColumn Identity;
        private DataGridViewTextBoxColumn Identity1;
        private DataGridViewTextBoxColumn IdentityStr;
        private DataGridViewTextBoxColumn IdentityStr2;
        internal Label Label1;
        private Label label3;
        private Label label4;
        internal Label Label9;
        internal Label lblContent;
        internal Label lblControlTimeSeg;
        internal Label lblMeetingAddr;
        internal Label lblMeetingDateTime;
        internal Label lblMeetingName;
        internal Label lblNotes;
        internal Label lblSignBegin;
        internal Label lblSignEnd;
        private Label lblWait;
        private DataGridViewTextBoxColumn SeatNO1;
        private System.Windows.Forms.Timer timer1;
        internal TextBox txtf_Content;
        internal TextBox txtf_MeetingName;
        internal TextBox txtf_MeetingNo;
        internal TextBox txtf_Notes;
        internal TextBox txtSeat;
        private DataGridViewTextBoxColumn UserID;
        private Panel panelBottomBanner;
    }
}

