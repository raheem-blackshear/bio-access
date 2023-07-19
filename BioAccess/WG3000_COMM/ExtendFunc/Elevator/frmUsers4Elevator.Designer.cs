namespace WG3000_COMM.ExtendFunc.Elevator
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
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmUsers4Elevator
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUsers4Elevator));
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attend = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Shift = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Door = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Deptname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.floorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeProfile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MoreFloor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FloorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnBatchUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnEditPrivilege = new System.Windows.Forms.ToolStripButton();
            this.btnAutoAdd = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.batchUpdateSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToOrderColumns = true;
            this.dgvUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(125)))), ((int)(((byte)(156)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUsers.ColumnHeadersHeight = 20;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsumerID,
            this.ConsumerNO,
            this.ConsumerName,
            this.CardNO,
            this.Attend,
            this.Shift,
            this.Door,
            this.Start,
            this.End,
            this.Deptname,
            this.floorName,
            this.TimeProfile,
            this.MoreFloor,
            this.FloorID});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Location = new System.Drawing.Point(0, 82);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersWidth = 20;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(948, 414);
            this.dgvUsers.TabIndex = 1;
            this.dgvUsers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvUsers_CellFormatting);
            this.dgvUsers.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvUsers_Scroll);
            this.dgvUsers.DoubleClick += new System.EventHandler(this.dgvUsers_DoubleClick);
            this.dgvUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUsers_KeyDown);
            // 
            // ConsumerID
            // 
            this.ConsumerID.HeaderText = "ConsumerID";
            this.ConsumerID.Name = "ConsumerID";
            this.ConsumerID.ReadOnly = true;
            this.ConsumerID.Visible = false;
            // 
            // ConsumerNO
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ConsumerNO.DefaultCellStyle = dataGridViewCellStyle2;
            this.ConsumerNO.HeaderText = "User ID";
            this.ConsumerNO.Name = "ConsumerNO";
            this.ConsumerNO.ReadOnly = true;
            // 
            // ConsumerName
            // 
            this.ConsumerName.HeaderText = "User Name";
            this.ConsumerName.Name = "ConsumerName";
            this.ConsumerName.ReadOnly = true;
            // 
            // CardNO
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.CardNO.DefaultCellStyle = dataGridViewCellStyle3;
            this.CardNO.HeaderText = "Card NO";
            this.CardNO.Name = "CardNO";
            this.CardNO.ReadOnly = true;
            // 
            // Attend
            // 
            this.Attend.HeaderText = "Attendence";
            this.Attend.Name = "Attend";
            this.Attend.ReadOnly = true;
            this.Attend.Visible = false;
            this.Attend.Width = 80;
            // 
            // Shift
            // 
            this.Shift.HeaderText = "Other Shift";
            this.Shift.Name = "Shift";
            this.Shift.ReadOnly = true;
            this.Shift.Visible = false;
            this.Shift.Width = 80;
            // 
            // Door
            // 
            this.Door.HeaderText = "Access Control";
            this.Door.Name = "Door";
            this.Door.ReadOnly = true;
            this.Door.Visible = false;
            this.Door.Width = 110;
            // 
            // Start
            // 
            this.Start.HeaderText = "Active Date";
            this.Start.Name = "Start";
            this.Start.ReadOnly = true;
            this.Start.Visible = false;
            // 
            // End
            // 
            this.End.HeaderText = "Deactive Date";
            this.End.Name = "End";
            this.End.ReadOnly = true;
            this.End.Visible = false;
            this.End.Width = 120;
            // 
            // Deptname
            // 
            this.Deptname.HeaderText = "Department";
            this.Deptname.Name = "Deptname";
            this.Deptname.ReadOnly = true;
            this.Deptname.Width = 200;
            // 
            // floorName
            // 
            this.floorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.floorName.HeaderText = "Privilege";
            this.floorName.Name = "floorName";
            this.floorName.ReadOnly = true;
            // 
            // TimeProfile
            // 
            this.TimeProfile.HeaderText = "Time Profile";
            this.TimeProfile.Name = "TimeProfile";
            this.TimeProfile.ReadOnly = true;
            // 
            // MoreFloor
            // 
            this.MoreFloor.HeaderText = "MoreFloor";
            this.MoreFloor.Name = "MoreFloor";
            this.MoreFloor.ReadOnly = true;
            this.MoreFloor.Visible = false;
            // 
            // FloorID
            // 
            this.FloorID.HeaderText = "Floor ID";
            this.FloorID.Name = "FloorID";
            this.FloorID.ReadOnly = true;
            this.FloorID.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "add.png");
            this.imageList1.Images.SetKeyName(1, "edit.png");
            this.imageList1.Images.SetKeyName(2, "delete.png");
            this.imageList1.Images.SetKeyName(3, "cancel.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBatchUpdate,
            this.btnEditPrivilege,
            this.btnAutoAdd,
            this.btnPrint,
            this.btnExport,
            this.btnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(948, 44);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnBatchUpdate
            // 
            this.btnBatchUpdate.ForeColor = System.Drawing.Color.White;
            this.btnBatchUpdate.Image = global::Properties.Resources.pTools_Edit_Batch;
            this.btnBatchUpdate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnBatchUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBatchUpdate.Name = "btnBatchUpdate";
            this.btnBatchUpdate.Size = new System.Drawing.Size(64, 41);
            this.btnBatchUpdate.Text = "Configure";
            this.btnBatchUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnBatchUpdate.Click += new System.EventHandler(this.btnBatchUpdate_Click);
            // 
            // btnEditPrivilege
            // 
            this.btnEditPrivilege.ForeColor = System.Drawing.Color.White;
            this.btnEditPrivilege.Image = global::Properties.Resources.pTools_EditPrivielge;
            this.btnEditPrivilege.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnEditPrivilege.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPrivilege.Name = "btnEditPrivilege";
            this.btnEditPrivilege.Size = new System.Drawing.Size(57, 41);
            this.btnEditPrivilege.Text = "Privilege";
            this.btnEditPrivilege.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditPrivilege.Click += new System.EventHandler(this.btnEditPrivilege_Click);
            // 
            // btnAutoAdd
            // 
            this.btnAutoAdd.ForeColor = System.Drawing.Color.White;
            this.btnAutoAdd.Image = global::Properties.Resources.pTools_Add_Auto;
            this.btnAutoAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAutoAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAutoAdd.Name = "btnAutoAdd";
            this.btnAutoAdd.Size = new System.Drawing.Size(45, 41);
            this.btnAutoAdd.Text = "Group";
            this.btnAutoAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAutoAdd.Visible = false;
            this.btnAutoAdd.Click += new System.EventHandler(this.btnAutoAdd_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::Properties.Resources.pTools_Print;
            this.btnPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(37, 41);
            this.btnPrint.Text = "Print";
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Image = global::Properties.Resources.pTools_ExportToExcel;
            this.btnExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(92, 41);
            this.btnExport.Text = "Export To Excel";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Image = global::Properties.Resources.pTools_Maps_Close;
            this.btnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(40, 41);
            this.btnExit.Text = "Close";
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchUpdateSelectToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(194, 26);
            // 
            // batchUpdateSelectToolStripMenuItem
            // 
            this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
            this.batchUpdateSelectToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.batchUpdateSelectToolStripMenuItem.Text = "Batch Update Select";
            this.batchUpdateSelectToolStripMenuItem.Click += new System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
            // 
            // userControlFind1
            // 
            this.userControlFind1.AutoSize = true;
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.userControlFind1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlFind1.Location = new System.Drawing.Point(0, 44);
            this.userControlFind1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userControlFind1.Name = "userControlFind1";
            this.userControlFind1.Size = new System.Drawing.Size(948, 38);
            this.userControlFind1.TabIndex = 2;
            // 
            // frmUsers4Elevator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(948, 496);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmUsers4Elevator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "One To More Management";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUsers4Elevator_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUsers_FormClosed);
            this.Load += new System.EventHandler(this.frmUsers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUsers_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewCheckBoxColumn Attend;
        private BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem batchUpdateSelectToolStripMenuItem;
        private ToolStripButton btnAutoAdd;
        private ToolStripButton btnBatchUpdate;
        private ToolStripButton btnEditPrivilege;
        private ToolStripButton btnExit;
        private ToolStripButton btnExport;
        private ToolStripButton btnPrint;
        private DataGridViewTextBoxColumn CardNO;
        private DataGridViewTextBoxColumn ConsumerID;
        private DataGridViewTextBoxColumn ConsumerName;
        private DataGridViewTextBoxColumn ConsumerNO;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridViewTextBoxColumn Deptname;
        private DataGridView dgvUsers;
        private DataGridViewCheckBoxColumn Door;
        private DataGridViewTextBoxColumn End;
        private DataGridViewTextBoxColumn FloorID;
        private DataGridViewTextBoxColumn floorName;
        private ImageList imageList1;
        private DataGridViewTextBoxColumn MoreFloor;
        private OpenFileDialog openFileDialog1;
        private DataGridViewCheckBoxColumn Shift;
        private DataGridViewTextBoxColumn Start;
        private DataGridViewTextBoxColumn TimeProfile;
        private ToolStrip toolStrip1;
    }
}

