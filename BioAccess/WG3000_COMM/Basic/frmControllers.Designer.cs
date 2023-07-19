namespace WG3000_COMM.Basic
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
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

    partial class frmControllers
    {
        private ToolStripMenuItem batchUpdateSelectToolStripMenuItem;
        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnPrint;
        private ToolStripButton btnSearchController;
        private ToolStripComboBox cboZone;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private dfrmFind dfrmFind1;
        private DataGridView dgvControllers;
        private DataTable dtController;
        private DataView dv;
        private DataGridViewTextBoxColumn f_ControllerID;
        private DataGridViewTextBoxColumn f_ControllerNO;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_DoorNames;
        private DataGridViewCheckBoxColumn f_Enabled;
        private DataGridViewTextBoxColumn f_IP;
        private DataGridViewTextBoxColumn f_Note;
        private DataGridViewTextBoxColumn f_PORT;
        private DataGridViewTextBoxColumn f_ZoneName;
        private ToolStrip toolStrip1;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.dtController != null)
                {
                    this.dtController.Dispose();
                }
                if (this.dv != null)
                {
                    this.dv.Dispose();
                }
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmControllers));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.batchUpdateSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvControllers = new System.Windows.Forms.DataGridView();
            this.f_ControllerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ZoneName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSearchController = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.cboZone = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvControllers)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchUpdateSelectToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // batchUpdateSelectToolStripMenuItem
            // 
            this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
            resources.ApplyResources(this.batchUpdateSelectToolStripMenuItem, "batchUpdateSelectToolStripMenuItem");
            this.batchUpdateSelectToolStripMenuItem.Click += new System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
            // 
            // dgvControllers
            // 
            this.dgvControllers.AllowUserToAddRows = false;
            this.dgvControllers.AllowUserToDeleteRows = false;
            this.dgvControllers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvControllers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvControllers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ControllerID,
            this.f_ControllerNO,
            this.f_ControllerSN,
            this.f_Enabled,
            this.f_IP,
            this.f_PORT,
            this.f_ZoneName,
            this.f_Note,
            this.f_DoorNames});
            resources.ApplyResources(this.dgvControllers, "dgvControllers");
            this.dgvControllers.EnableHeadersVisualStyles = false;
            this.dgvControllers.Name = "dgvControllers";
            this.dgvControllers.ReadOnly = true;
            this.dgvControllers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(150)))), ((int)(((byte)(177)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvControllers.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvControllers.RowTemplate.Height = 23;
            this.dgvControllers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvControllers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvControllers_MouseDoubleClick);
            // 
            // f_ControllerID
            // 
            resources.ApplyResources(this.f_ControllerID, "f_ControllerID");
            this.f_ControllerID.Name = "f_ControllerID";
            this.f_ControllerID.ReadOnly = true;
            // 
            // f_ControllerNO
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ControllerNO.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_ControllerNO, "f_ControllerNO");
            this.f_ControllerNO.Name = "f_ControllerNO";
            this.f_ControllerNO.ReadOnly = true;
            // 
            // f_ControllerSN
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            // 
            // f_Enabled
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.NullValue = false;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.f_Enabled.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.f_Enabled, "f_Enabled");
            this.f_Enabled.Name = "f_Enabled";
            this.f_Enabled.ReadOnly = true;
            this.f_Enabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_Enabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_IP
            // 
            resources.ApplyResources(this.f_IP, "f_IP");
            this.f_IP.Name = "f_IP";
            this.f_IP.ReadOnly = true;
            // 
            // f_PORT
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_PORT.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.f_PORT, "f_PORT");
            this.f_PORT.Name = "f_PORT";
            this.f_PORT.ReadOnly = true;
            // 
            // f_ZoneName
            // 
            resources.ApplyResources(this.f_ZoneName, "f_ZoneName");
            this.f_ZoneName.Name = "f_ZoneName";
            this.f_ZoneName.ReadOnly = true;
            // 
            // f_Note
            // 
            resources.ApplyResources(this.f_Note, "f_Note");
            this.f_Note.Name = "f_Note";
            this.f_Note.ReadOnly = true;
            // 
            // f_DoorNames
            // 
            this.f_DoorNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_DoorNames, "f_DoorNames");
            this.f_DoorNames.Name = "f_DoorNames";
            this.f_DoorNames.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSearchController,
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.btnPrint,
            this.btnExportToExcel,
            this.cboZone});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnSearchController
            // 
            this.btnSearchController.ForeColor = System.Drawing.Color.White;
            this.btnSearchController.Image = global::Properties.Resources.icon_search;
            resources.ApplyResources(this.btnSearchController, "btnSearchController");
            this.btnSearchController.Name = "btnSearchController";
            this.btnSearchController.Click += new System.EventHandler(this.btnSearchController_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_new1;
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
            // cboZone
            // 
            this.cboZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboZone, "cboZone");
            this.cboZone.Name = "cboZone";
            this.cboZone.SelectedIndexChanged += new System.EventHandler(this.cboZone_SelectedIndexChanged);
            // 
            // frmControllers
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvControllers);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmControllers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmControllers_FormClosing);
            this.Load += new System.EventHandler(this.frmControllers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmControllers_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvControllers)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

