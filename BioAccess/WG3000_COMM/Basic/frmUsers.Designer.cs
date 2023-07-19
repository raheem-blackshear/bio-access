namespace WG3000_COMM.Basic
{
    using WG3000_COMM.Core;
    //using WG3000_COMM.DataOper;
    //using WG3000_COMM.Properties;
    //using WG3000_COMM.ResStrings;

    partial class frmUsers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

//         /// <summary>
//         /// Clean up any resources being used.
//         /// </summary>
//         /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//         protected override void Dispose(bool disposing)
//         {
//             if (disposing && (components != null))
//             {
//                 components.Dispose();
//             }
//             base.Dispose(disposing);
//         }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUsers));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.batchUpdateSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnAutoAdd = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.btnImportFromExcel = new System.Windows.Forms.ToolStripButton();
            this.btnRegisterLostCard = new System.Windows.Forms.ToolStripButton();
            this.btnBatchUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnEditPrivilege = new System.Windows.Forms.ToolStripButton();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            this.ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fingerprint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Face = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attend = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Shift = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Door = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Deptname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvUsers, "dgvUsers");
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConsumerID,
            this.ConsumerNO,
            this.ConsumerName,
            this.Fingerprint,
            this.Face,
            this.CardNO,
            this.PIN,
            this.Attend,
            this.Shift,
            this.Door,
            this.Start,
            this.End,
            this.Deptname});
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowTemplate.Height = 23;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvUsers_Scroll);
            this.dgvUsers.DoubleClick += new System.EventHandler(this.dgvUsers_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.batchUpdateSelectToolStripMenuItem,
            this.importFromExcelToolStripMenuItem,
            this.displayAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // batchUpdateSelectToolStripMenuItem
            // 
            this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
            resources.ApplyResources(this.batchUpdateSelectToolStripMenuItem, "batchUpdateSelectToolStripMenuItem");
            this.batchUpdateSelectToolStripMenuItem.Click += new System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
            // 
            // importFromExcelToolStripMenuItem
            // 
            this.importFromExcelToolStripMenuItem.Name = "importFromExcelToolStripMenuItem";
            resources.ApplyResources(this.importFromExcelToolStripMenuItem, "importFromExcelToolStripMenuItem");
            this.importFromExcelToolStripMenuItem.Click += new System.EventHandler(this.btnImportFromExcel_Click);
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
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.btnAutoAdd,
            this.btnPrint,
            this.btnExport,
            this.btnImportFromExcel,
            this.btnRegisterLostCard,
            this.btnBatchUpdate,
            this.btnEditPrivilege});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_new;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Image = global::Properties.Resources.icon_edit;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = global::Properties.Resources.icon_delete;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAutoAdd
            // 
            this.btnAutoAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoAdd.ForeColor = System.Drawing.Color.White;
            this.btnAutoAdd.Image = global::Properties.Resources.icon_auto_add;
            resources.ApplyResources(this.btnAutoAdd, "btnAutoAdd");
            this.btnAutoAdd.Name = "btnAutoAdd";
            this.btnAutoAdd.Click += new System.EventHandler(this.btnAutoAdd_Click);
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
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.ForeColor = System.Drawing.Color.White;
            this.btnImportFromExcel.Image = global::Properties.Resources.icon_import;
            resources.ApplyResources(this.btnImportFromExcel, "btnImportFromExcel");
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Click += new System.EventHandler(this.btnImportFromExcel_Click);
            // 
            // btnRegisterLostCard
            // 
            this.btnRegisterLostCard.ForeColor = System.Drawing.Color.White;
            this.btnRegisterLostCard.Image = global::Properties.Resources.icon_card_lost;
            resources.ApplyResources(this.btnRegisterLostCard, "btnRegisterLostCard");
            this.btnRegisterLostCard.Name = "btnRegisterLostCard";
            this.btnRegisterLostCard.Click += new System.EventHandler(this.btnRegisterLostCard_Click);
            // 
            // btnBatchUpdate
            // 
            this.btnBatchUpdate.ForeColor = System.Drawing.Color.White;
            this.btnBatchUpdate.Image = global::Properties.Resources.icon_edit_batch;
            resources.ApplyResources(this.btnBatchUpdate, "btnBatchUpdate");
            this.btnBatchUpdate.Name = "btnBatchUpdate";
            this.btnBatchUpdate.Click += new System.EventHandler(this.btnBatchUpdate_Click);
            // 
            // btnEditPrivilege
            // 
            this.btnEditPrivilege.ForeColor = System.Drawing.Color.White;
            this.btnEditPrivilege.Image = global::Properties.Resources.Icon_Privilege;
            resources.ApplyResources(this.btnEditPrivilege, "btnEditPrivilege");
            this.btnEditPrivilege.Name = "btnEditPrivilege";
            this.btnEditPrivilege.Click += new System.EventHandler(this.btnEditPrivilege_Click);
            // 
            // userControlFind1
            // 
            resources.ApplyResources(this.userControlFind1, "userControlFind1");
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.Name = "userControlFind1";
            // 
            // ConsumerID
            // 
            resources.ApplyResources(this.ConsumerID, "ConsumerID");
            this.ConsumerID.Name = "ConsumerID";
            this.ConsumerID.ReadOnly = true;
            // 
            // ConsumerNO
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ConsumerNO.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.ConsumerNO, "ConsumerNO");
            this.ConsumerNO.Name = "ConsumerNO";
            this.ConsumerNO.ReadOnly = true;
            // 
            // ConsumerName
            // 
            resources.ApplyResources(this.ConsumerName, "ConsumerName");
            this.ConsumerName.Name = "ConsumerName";
            this.ConsumerName.ReadOnly = true;
            // 
            // Fingerprint
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Fingerprint.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Fingerprint, "Fingerprint");
            this.Fingerprint.Name = "Fingerprint";
            this.Fingerprint.ReadOnly = true;
            this.Fingerprint.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Fingerprint.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Face
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Face.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.Face, "Face");
            this.Face.Name = "Face";
            this.Face.ReadOnly = true;
            this.Face.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CardNO
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.CardNO.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.CardNO, "CardNO");
            this.CardNO.Name = "CardNO";
            this.CardNO.ReadOnly = true;
            // 
            // PIN
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.PIN.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.PIN, "PIN");
            this.PIN.Name = "PIN";
            this.PIN.ReadOnly = true;
            // 
            // Attend
            // 
            resources.ApplyResources(this.Attend, "Attend");
            this.Attend.Name = "Attend";
            this.Attend.ReadOnly = true;
            // 
            // Shift
            // 
            resources.ApplyResources(this.Shift, "Shift");
            this.Shift.Name = "Shift";
            this.Shift.ReadOnly = true;
            // 
            // Door
            // 
            resources.ApplyResources(this.Door, "Door");
            this.Door.Name = "Door";
            this.Door.ReadOnly = true;
            // 
            // Start
            // 
            resources.ApplyResources(this.Start, "Start");
            this.Start.Name = "Start";
            this.Start.ReadOnly = true;
            // 
            // End
            // 
            resources.ApplyResources(this.End, "End");
            this.End.Name = "End";
            this.End.ReadOnly = true;
            // 
            // Deptname
            // 
            this.Deptname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Deptname, "Deptname");
            this.Deptname.Name = "Deptname";
            this.Deptname.ReadOnly = true;
            // 
            // frmUsers
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmUsers";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUsers_FormClosed);
            this.Load += new System.EventHandler(this.frmUsers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUsers_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAutoAdd;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripButton btnImportFromExcel;
        private System.Windows.Forms.ToolStripButton btnRegisterLostCard;
        private System.Windows.Forms.ToolStripButton btnBatchUpdate;
        private System.Windows.Forms.ToolStripButton btnEditPrivilege;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayAllToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private UserControlFind userControlFind1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsumerNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fingerprint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Face;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PIN;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Attend;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Shift;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Door;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn End;
        private System.Windows.Forms.DataGridViewTextBoxColumn Deptname;
    }
}