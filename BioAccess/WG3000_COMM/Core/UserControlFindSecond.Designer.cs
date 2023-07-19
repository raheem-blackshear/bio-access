namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    partial class UserControlFindSecond : UserControl
    {
        private BackgroundWorker backgroundWorker1;
        public ToolStripButton btnClear;
        public ToolStripButton btnQuery;
        public ToolStripComboBox cboFindDept;
        private ComboBox cboUsers;
        private IContainer components;
        private DataView dv;
        private static DataView dvLastLoad;
        private DataTable tb;
        private System.Windows.Forms.Timer timer1;
        private ToolStrip toolFindUsers;
        private ToolStripLabel toolStripLabel1;
        public ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
        public ToolStripTextBox txtFindCardID;
        public ToolStripTextBox txtFindName;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlFindSecond));
            this.toolFindUsers = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtFindName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtFindCardID = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cboFindDept = new System.Windows.Forms.ToolStripComboBox();
            this.btnQuery = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolFindUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolFindUsers
            // 
            resources.ApplyResources(this.toolFindUsers, "toolFindUsers");
            this.toolFindUsers.BackgroundImage = global::Properties.Resources.pTools_third_title;
            this.toolFindUsers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtFindName,
            this.toolStripLabel2,
            this.txtFindCardID,
            this.toolStripLabel3,
            this.cboFindDept,
            this.btnQuery,
            this.btnClear});
            this.toolFindUsers.Name = "toolFindUsers";
            // 
            // toolStripLabel1
            // 
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel1.Name = "toolStripLabel1";
            // 
            // txtFindName
            // 
            this.txtFindName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFindName.Name = "txtFindName";
            resources.ApplyResources(this.txtFindName, "txtFindName");
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // txtFindCardID
            // 
            this.txtFindCardID.Name = "txtFindCardID";
            resources.ApplyResources(this.txtFindCardID, "txtFindCardID");
            this.txtFindCardID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFindCardID_KeyPress);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // cboFindDept
            // 
            this.cboFindDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFindDept.Name = "cboFindDept";
            resources.ApplyResources(this.cboFindDept, "cboFindDept");
            this.cboFindDept.SelectedIndexChanged += new System.EventHandler(this.cboFindDept_SelectedIndexChanged);
            // 
            // btnQuery
            // 
            this.btnQuery.ForeColor = System.Drawing.Color.White;
            this.btnQuery.Image = global::Properties.Resources.pTools_Query;
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnClear
            // 
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Image = global::Properties.Resources.pTools_Clear_Condition;
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // cboUsers
            // 
            this.cboUsers.DropDownWidth = 200;
            this.cboUsers.FormattingEnabled = true;
            resources.ApplyResources(this.cboUsers, "cboUsers");
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.DropDown += new System.EventHandler(this.cboUsers_DropDown);
            this.cboUsers.DropDownClosed += new System.EventHandler(this.cboUsers_DropDownClosed);
            this.cboUsers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboUsers_KeyPress);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UserControlFindSecond
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Properties.Resources.pTools_third_title;
            this.Controls.Add(this.cboUsers);
            this.Controls.Add(this.toolFindUsers);
            this.DoubleBuffered = true;
            this.Name = "UserControlFindSecond";
            this.Load += new System.EventHandler(this.UserControlFind_Load);
            this.toolFindUsers.ResumeLayout(false);
            this.toolFindUsers.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

