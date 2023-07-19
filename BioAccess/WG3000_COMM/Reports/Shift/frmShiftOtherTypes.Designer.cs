namespace WG3000_COMM.Reports.Shift
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

    public partial class frmShiftOtherTypes
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShiftOtherTypes));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_ShiftID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReadTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_bOvertimeShift = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_OnDuty1t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OffDuty1t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OnDuty2t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OffDuty2t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OnDuty3t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OffDuty3t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OnDuty4t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OffDuty4t = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ShiftID,
            this.f_ShiftName,
            this.f_ReadTimes,
            this.f_bOvertimeShift,
            this.f_OnDuty1t,
            this.f_OffDuty1t,
            this.f_OnDuty2t,
            this.f_OffDuty2t,
            this.f_OnDuty3t,
            this.f_OffDuty3t,
            this.f_OnDuty4t,
            this.f_OffDuty4t});
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.DoubleClick += new System.EventHandler(this.dgvControlSegs_DoubleClick);
            // 
            // f_ShiftID
            // 
            resources.ApplyResources(this.f_ShiftID, "f_ShiftID");
            this.f_ShiftID.Name = "f_ShiftID";
            this.f_ShiftID.ReadOnly = true;
            // 
            // f_ShiftName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ShiftName.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_ShiftName, "f_ShiftName");
            this.f_ShiftName.Name = "f_ShiftName";
            this.f_ShiftName.ReadOnly = true;
            this.f_ShiftName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_ShiftName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_ReadTimes
            // 
            resources.ApplyResources(this.f_ReadTimes, "f_ReadTimes");
            this.f_ReadTimes.Name = "f_ReadTimes";
            this.f_ReadTimes.ReadOnly = true;
            this.f_ReadTimes.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_ReadTimes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_bOvertimeShift
            // 
            resources.ApplyResources(this.f_bOvertimeShift, "f_bOvertimeShift");
            this.f_bOvertimeShift.Name = "f_bOvertimeShift";
            this.f_bOvertimeShift.ReadOnly = true;
            // 
            // f_OnDuty1t
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_OnDuty1t.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_OnDuty1t, "f_OnDuty1t");
            this.f_OnDuty1t.Name = "f_OnDuty1t";
            this.f_OnDuty1t.ReadOnly = true;
            this.f_OnDuty1t.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_OnDuty1t.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_OffDuty1t
            // 
            resources.ApplyResources(this.f_OffDuty1t, "f_OffDuty1t");
            this.f_OffDuty1t.Name = "f_OffDuty1t";
            this.f_OffDuty1t.ReadOnly = true;
            this.f_OffDuty1t.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_OffDuty1t.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_OnDuty2t
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_OnDuty2t.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_OnDuty2t, "f_OnDuty2t");
            this.f_OnDuty2t.Name = "f_OnDuty2t";
            this.f_OnDuty2t.ReadOnly = true;
            this.f_OnDuty2t.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_OnDuty2t.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_OffDuty2t
            // 
            resources.ApplyResources(this.f_OffDuty2t, "f_OffDuty2t");
            this.f_OffDuty2t.Name = "f_OffDuty2t";
            this.f_OffDuty2t.ReadOnly = true;
            // 
            // f_OnDuty3t
            // 
            resources.ApplyResources(this.f_OnDuty3t, "f_OnDuty3t");
            this.f_OnDuty3t.Name = "f_OnDuty3t";
            this.f_OnDuty3t.ReadOnly = true;
            this.f_OnDuty3t.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_OnDuty3t.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_OffDuty3t
            // 
            resources.ApplyResources(this.f_OffDuty3t, "f_OffDuty3t");
            this.f_OffDuty3t.Name = "f_OffDuty3t";
            this.f_OffDuty3t.ReadOnly = true;
            // 
            // f_OnDuty4t
            // 
            resources.ApplyResources(this.f_OnDuty4t, "f_OnDuty4t");
            this.f_OnDuty4t.Name = "f_OnDuty4t";
            this.f_OnDuty4t.ReadOnly = true;
            // 
            // f_OffDuty4t
            // 
            resources.ApplyResources(this.f_OffDuty4t, "f_OffDuty4t");
            this.f_OffDuty4t.Name = "f_OffDuty4t";
            this.f_OffDuty4t.ReadOnly = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.BackgroundImage = global::Properties.Resources.pTools_second_title;
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
            this.btnExportToExcel});
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
            // frmShiftOtherTypes
            // 
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmShiftOtherTypes";
            this.Load += new System.EventHandler(this.frmShiftOtherTypes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnPrint;
        private DataGridView dgvMain;
        private DataGridViewCheckBoxColumn f_bOvertimeShift;
        private DataGridViewTextBoxColumn f_OffDuty1t;
        private DataGridViewTextBoxColumn f_OffDuty2t;
        private DataGridViewTextBoxColumn f_OffDuty3t;
        private DataGridViewTextBoxColumn f_OffDuty4t;
        private DataGridViewTextBoxColumn f_OnDuty1t;
        private DataGridViewTextBoxColumn f_OnDuty2t;
        private DataGridViewTextBoxColumn f_OnDuty3t;
        private DataGridViewTextBoxColumn f_OnDuty4t;
        private DataGridViewTextBoxColumn f_ReadTimes;
        private DataGridViewTextBoxColumn f_ShiftID;
        private DataGridViewTextBoxColumn f_ShiftName;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripLabel toolStripLabel1;
    }
}

