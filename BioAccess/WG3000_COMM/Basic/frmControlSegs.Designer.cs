namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmControlSegs
    {
        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnHolidayControl;
        private ToolStripButton btnPrint;
        private IContainer components = null;
        private DataGridView dgvControlSegs;
        private DataTable dt;
        private DataView dv;
        private DataGridViewTextBoxColumn f_BeginHMS1A;
        private DataGridViewTextBoxColumn f_BeginHMS2A;
        private DataGridViewTextBoxColumn f_BeginHMS3A;
        private DataGridViewTextBoxColumn f_BeginYMD;
        private DataGridViewCheckBoxColumn f_ControlByHoliday;
        private DataGridViewTextBoxColumn f_ControlSegID;
        private DataGridViewTextBoxColumn f_ControlSegIDBak;
        private DataGridViewTextBoxColumn f_ControlSegIDLinked;
        private DataGridViewTextBoxColumn f_EndHMS1A;
        private DataGridViewTextBoxColumn f_EndHMS2A;
        private DataGridViewTextBoxColumn f_EndHMS3A;
        private DataGridViewTextBoxColumn f_EndYMD;
        private DataGridViewCheckBoxColumn f_Friday;
        private DataGridViewCheckBoxColumn f_Monday;
        private DataGridViewCheckBoxColumn f_Saturday;
        private DataGridViewCheckBoxColumn f_Sunday;
        private DataGridViewCheckBoxColumn f_Thursday;
        private DataGridViewCheckBoxColumn f_Tuesday;
        private DataGridViewCheckBoxColumn f_Wednesday;
        private bool firstShow = true;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripLabel toolStripLabel1;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmControlSegs));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvControlSegs = new System.Windows.Forms.DataGridView();
            this.f_ControlSegIDBak = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Monday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Tuesday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Wednesday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Thursday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Friday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Saturday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Sunday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_BeginHMS1A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_EndHMS1A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_BeginHMS2A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_EndHMS2A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_BeginHMS3A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_EndHMS3A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegIDLinked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_BeginYMD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_EndYMD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlByHoliday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.btnHolidayControl = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvControlSegs)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvControlSegs
            // 
            this.dgvControlSegs.AllowUserToAddRows = false;
            this.dgvControlSegs.AllowUserToDeleteRows = false;
            this.dgvControlSegs.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControlSegs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvControlSegs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvControlSegs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ControlSegIDBak,
            this.f_ControlSegID,
            this.f_Monday,
            this.f_Tuesday,
            this.f_Wednesday,
            this.f_Thursday,
            this.f_Friday,
            this.f_Saturday,
            this.f_Sunday,
            this.f_BeginHMS1A,
            this.f_EndHMS1A,
            this.f_BeginHMS2A,
            this.f_EndHMS2A,
            this.f_BeginHMS3A,
            this.f_EndHMS3A,
            this.f_ControlSegIDLinked,
            this.f_BeginYMD,
            this.f_EndYMD,
            this.f_ControlByHoliday});
            resources.ApplyResources(this.dgvControlSegs, "dgvControlSegs");
            this.dgvControlSegs.EnableHeadersVisualStyles = false;
            this.dgvControlSegs.Name = "dgvControlSegs";
            this.dgvControlSegs.ReadOnly = true;
            this.dgvControlSegs.RowTemplate.Height = 23;
            this.dgvControlSegs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvControlSegs.DoubleClick += new System.EventHandler(this.dgvControlSegs_DoubleClick);
            // 
            // f_ControlSegIDBak
            // 
            resources.ApplyResources(this.f_ControlSegIDBak, "f_ControlSegIDBak");
            this.f_ControlSegIDBak.Name = "f_ControlSegIDBak";
            this.f_ControlSegIDBak.ReadOnly = true;
            // 
            // f_ControlSegID
            // 
            resources.ApplyResources(this.f_ControlSegID, "f_ControlSegID");
            this.f_ControlSegID.Name = "f_ControlSegID";
            this.f_ControlSegID.ReadOnly = true;
            // 
            // f_Monday
            // 
            resources.ApplyResources(this.f_Monday, "f_Monday");
            this.f_Monday.Name = "f_Monday";
            this.f_Monday.ReadOnly = true;
            // 
            // f_Tuesday
            // 
            resources.ApplyResources(this.f_Tuesday, "f_Tuesday");
            this.f_Tuesday.Name = "f_Tuesday";
            this.f_Tuesday.ReadOnly = true;
            // 
            // f_Wednesday
            // 
            resources.ApplyResources(this.f_Wednesday, "f_Wednesday");
            this.f_Wednesday.Name = "f_Wednesday";
            this.f_Wednesday.ReadOnly = true;
            // 
            // f_Thursday
            // 
            resources.ApplyResources(this.f_Thursday, "f_Thursday");
            this.f_Thursday.Name = "f_Thursday";
            this.f_Thursday.ReadOnly = true;
            // 
            // f_Friday
            // 
            resources.ApplyResources(this.f_Friday, "f_Friday");
            this.f_Friday.Name = "f_Friday";
            this.f_Friday.ReadOnly = true;
            // 
            // f_Saturday
            // 
            resources.ApplyResources(this.f_Saturday, "f_Saturday");
            this.f_Saturday.Name = "f_Saturday";
            this.f_Saturday.ReadOnly = true;
            // 
            // f_Sunday
            // 
            resources.ApplyResources(this.f_Sunday, "f_Sunday");
            this.f_Sunday.Name = "f_Sunday";
            this.f_Sunday.ReadOnly = true;
            // 
            // f_BeginHMS1A
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_BeginHMS1A.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_BeginHMS1A, "f_BeginHMS1A");
            this.f_BeginHMS1A.Name = "f_BeginHMS1A";
            this.f_BeginHMS1A.ReadOnly = true;
            this.f_BeginHMS1A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_BeginHMS1A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_EndHMS1A
            // 
            resources.ApplyResources(this.f_EndHMS1A, "f_EndHMS1A");
            this.f_EndHMS1A.Name = "f_EndHMS1A";
            this.f_EndHMS1A.ReadOnly = true;
            this.f_EndHMS1A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_EndHMS1A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_BeginHMS2A
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_BeginHMS2A.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_BeginHMS2A, "f_BeginHMS2A");
            this.f_BeginHMS2A.Name = "f_BeginHMS2A";
            this.f_BeginHMS2A.ReadOnly = true;
            this.f_BeginHMS2A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_BeginHMS2A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_EndHMS2A
            // 
            resources.ApplyResources(this.f_EndHMS2A, "f_EndHMS2A");
            this.f_EndHMS2A.Name = "f_EndHMS2A";
            this.f_EndHMS2A.ReadOnly = true;
            this.f_EndHMS2A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_EndHMS2A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_BeginHMS3A
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_BeginHMS3A.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_BeginHMS3A, "f_BeginHMS3A");
            this.f_BeginHMS3A.Name = "f_BeginHMS3A";
            this.f_BeginHMS3A.ReadOnly = true;
            this.f_BeginHMS3A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_BeginHMS3A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_EndHMS3A
            // 
            resources.ApplyResources(this.f_EndHMS3A, "f_EndHMS3A");
            this.f_EndHMS3A.Name = "f_EndHMS3A";
            this.f_EndHMS3A.ReadOnly = true;
            this.f_EndHMS3A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_EndHMS3A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_ControlSegIDLinked
            // 
            resources.ApplyResources(this.f_ControlSegIDLinked, "f_ControlSegIDLinked");
            this.f_ControlSegIDLinked.Name = "f_ControlSegIDLinked";
            this.f_ControlSegIDLinked.ReadOnly = true;
            // 
            // f_BeginYMD
            // 
            resources.ApplyResources(this.f_BeginYMD, "f_BeginYMD");
            this.f_BeginYMD.Name = "f_BeginYMD";
            this.f_BeginYMD.ReadOnly = true;
            // 
            // f_EndYMD
            // 
            resources.ApplyResources(this.f_EndYMD, "f_EndYMD");
            this.f_EndYMD.Name = "f_EndYMD";
            this.f_EndYMD.ReadOnly = true;
            // 
            // f_ControlByHoliday
            // 
            resources.ApplyResources(this.f_ControlByHoliday, "f_ControlByHoliday");
            this.f_ControlByHoliday.Name = "f_ControlByHoliday";
            this.f_ControlByHoliday.ReadOnly = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.BackgroundImage = global::Properties.Resources.pTools_first_title;
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
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
            this.btnPrint,
            this.btnExportToExcel,
            this.btnHolidayControl});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_new;
            resources.ApplyResources(this.btnAdd, "btnAdd");
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
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::Properties.Resources.icon_print;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportToExcel.Image = global::Properties.Resources.icon_export_excel;
            resources.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // btnHolidayControl
            // 
            resources.ApplyResources(this.btnHolidayControl, "btnHolidayControl");
            this.btnHolidayControl.ForeColor = System.Drawing.Color.White;
            this.btnHolidayControl.Image = global::Properties.Resources.icon_add_child;
            this.btnHolidayControl.Name = "btnHolidayControl";
            this.btnHolidayControl.Click += new System.EventHandler(this.btnHolidayControl_Click);
            // 
            // frmControlSegs
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dgvControlSegs);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmControlSegs";
            this.Load += new System.EventHandler(this.frmControlSegs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvControlSegs)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

