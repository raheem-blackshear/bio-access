namespace WG3000_COMM.Basic
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
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmPrivileges
    {
        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnEdit;
        private ToolStripButton btnEditSinglePrivilege;
        private ToolStripButton btnExport;
        private ToolStripButton btnPrint;
        private ToolStripButton btnPrivilegeCopy;
        private ToolStripMenuItem cardNOFuzzyQueryToolStripMenuItem;
        private ToolStripComboBox cboDoor;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridView dgvPrivileges;
        private ToolStripMenuItem displayAllToolStripMenuItem;
        private DataTable dt;
        private DataTable dtData;
        private DataView dvDoors;
        private DataView dvDoors4Watching;
        private DataGridViewTextBoxColumn f_CardNO;
        private DataGridViewTextBoxColumn f_ConsumerID;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_ControlSeg;
        private DataGridViewTextBoxColumn f_ControlSegName;
        private DataGridViewTextBoxColumn f_DoorName;
        private DataGridViewTextBoxColumn f_RecID;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripButton2;
        private ToolStripLabel toolStripLabel1;
        private UserControlFind userControlFind1;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrivileges));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPrivileges = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cardNOFuzzyQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cboDoor = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.btnPrivilegeCopy = new System.Windows.Forms.ToolStripButton();
            this.btnEditSinglePrivilege = new System.Windows.Forms.ToolStripButton();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrivileges)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPrivileges
            // 
            this.dgvPrivileges.AllowUserToAddRows = false;
            this.dgvPrivileges.AllowUserToDeleteRows = false;
            this.dgvPrivileges.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrivileges.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPrivileges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPrivileges.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_RecID,
            this.f_DoorName,
            this.f_ConsumerNO,
            this.f_ConsumerName,
            this.f_CardNO,
            this.f_ControlSeg,
            this.f_ControlSegName,
            this.f_ConsumerID});
            this.dgvPrivileges.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.dgvPrivileges, "dgvPrivileges");
            this.dgvPrivileges.EnableHeadersVisualStyles = false;
            this.dgvPrivileges.Name = "dgvPrivileges";
            this.dgvPrivileges.ReadOnly = true;
            this.dgvPrivileges.RowTemplate.Height = 23;
            this.dgvPrivileges.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrivileges.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPrivileges_CellFormatting);
            this.dgvPrivileges.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvPrivileges_Scroll);
            // 
            // f_RecID
            // 
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
            // 
            // f_DoorName
            // 
            resources.ApplyResources(this.f_DoorName, "f_DoorName");
            this.f_DoorName.Name = "f_DoorName";
            this.f_DoorName.ReadOnly = true;
            // 
            // f_ConsumerNO
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
            this.f_ConsumerNO.Name = "f_ConsumerNO";
            this.f_ConsumerNO.ReadOnly = true;
            // 
            // f_ConsumerName
            // 
            resources.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
            this.f_ConsumerName.Name = "f_ConsumerName";
            this.f_ConsumerName.ReadOnly = true;
            // 
            // f_CardNO
            // 
            resources.ApplyResources(this.f_CardNO, "f_CardNO");
            this.f_CardNO.Name = "f_CardNO";
            this.f_CardNO.ReadOnly = true;
            // 
            // f_ControlSeg
            // 
            resources.ApplyResources(this.f_ControlSeg, "f_ControlSeg");
            this.f_ControlSeg.Name = "f_ControlSeg";
            this.f_ControlSeg.ReadOnly = true;
            // 
            // f_ControlSegName
            // 
            this.f_ControlSegName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_ControlSegName, "f_ControlSegName");
            this.f_ControlSegName.Name = "f_ControlSegName";
            this.f_ControlSegName.ReadOnly = true;
            // 
            // f_ConsumerID
            // 
            resources.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
            this.f_ConsumerID.Name = "f_ConsumerID";
            this.f_ConsumerID.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cardNOFuzzyQueryToolStripMenuItem,
            this.displayAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // cardNOFuzzyQueryToolStripMenuItem
            // 
            this.cardNOFuzzyQueryToolStripMenuItem.Name = "cardNOFuzzyQueryToolStripMenuItem";
            resources.ApplyResources(this.cardNOFuzzyQueryToolStripMenuItem, "cardNOFuzzyQueryToolStripMenuItem");
            this.cardNOFuzzyQueryToolStripMenuItem.Click += new System.EventHandler(this.cardNOFuzzyQueryToolStripMenuItem_Click);
            // 
            // displayAllToolStripMenuItem
            // 
            this.displayAllToolStripMenuItem.Name = "displayAllToolStripMenuItem";
            resources.ApplyResources(this.displayAllToolStripMenuItem, "displayAllToolStripMenuItem");
            this.displayAllToolStripMenuItem.Click += new System.EventHandler(this.displayAllToolStripMenuItem_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.BackgroundImage = global::Properties.Resources.pTools_second_title;
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cboDoor});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel1.Name = "toolStripLabel1";
            // 
            // cboDoor
            // 
            this.cboDoor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDoor.Name = "cboDoor";
            resources.ApplyResources(this.cboDoor, "cboDoor");
            this.cboDoor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDoor_KeyPress);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEdit,
            this.btnPrint,
            this.btnExport,
            this.toolStripButton2,
            this.btnPrivilegeCopy,
            this.btnEditSinglePrivilege});
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btnEdit
            // 
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Image = global::Properties.Resources.icon_change_privilege;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::Properties.Resources.icon_print;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Image = global::Properties.Resources.icon_export_excel;
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.ForeColor = System.Drawing.Color.White;
            this.toolStripButton2.Image = global::Properties.Resources.icon_create;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnPrivilegeCopy
            // 
            this.btnPrivilegeCopy.ForeColor = System.Drawing.Color.White;
            this.btnPrivilegeCopy.Image = global::Properties.Resources.icon_copy_privilege;
            resources.ApplyResources(this.btnPrivilegeCopy, "btnPrivilegeCopy");
            this.btnPrivilegeCopy.Name = "btnPrivilegeCopy";
            this.btnPrivilegeCopy.Click += new System.EventHandler(this.btnPrivilegeCopy_Click);
            // 
            // btnEditSinglePrivilege
            // 
            this.btnEditSinglePrivilege.ForeColor = System.Drawing.Color.White;
            this.btnEditSinglePrivilege.Image = global::Properties.Resources.icon_edit_privilege;
            resources.ApplyResources(this.btnEditSinglePrivilege, "btnEditSinglePrivilege");
            this.btnEditSinglePrivilege.Name = "btnEditSinglePrivilege";
            this.btnEditSinglePrivilege.Click += new System.EventHandler(this.btnEditSinglePrivilege_Click);
            // 
            // userControlFind1
            // 
            resources.ApplyResources(this.userControlFind1, "userControlFind1");
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.Name = "userControlFind1";
            // 
            // frmPrivileges
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvPrivileges);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.Name = "frmPrivileges";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrivileges_FormClosing);
            this.Load += new System.EventHandler(this.frmPrivileges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrivileges)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

