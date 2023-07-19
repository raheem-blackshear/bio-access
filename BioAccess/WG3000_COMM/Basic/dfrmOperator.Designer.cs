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

    partial class dfrmOperator
    {
        private ToolStripButton btnAdd;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnEditDepartment;
        private ToolStripButton btnEditPrivilege;
        private ToolStripButton btnEditZones;
        private ToolStripButton btnSetPassword;
        private IContainer components = null;
        private DataGridView dgvOperators;
        private DataView dv;
        private DataGridViewTextBoxColumn f_OperatorID;
        private DataGridViewTextBoxColumn f_OperatorName;
        private DataTable table;
        private ToolStrip toolStrip1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmOperator));
            this.dgvOperators = new System.Windows.Forms.DataGridView();
            this.f_OperatorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OperatorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnSetPassword = new System.Windows.Forms.ToolStripButton();
            this.btnEditPrivilege = new System.Windows.Forms.ToolStripButton();
            this.btnEditDepartment = new System.Windows.Forms.ToolStripButton();
            this.btnEditZones = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperators)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOperators
            // 
            this.dgvOperators.AllowUserToAddRows = false;
            this.dgvOperators.AllowUserToDeleteRows = false;
            this.dgvOperators.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOperators.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOperators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOperators.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_OperatorID,
            this.f_OperatorName});
            resources.ApplyResources(this.dgvOperators, "dgvOperators");
            this.dgvOperators.EnableHeadersVisualStyles = false;
            this.dgvOperators.MultiSelect = false;
            this.dgvOperators.Name = "dgvOperators";
            this.dgvOperators.ReadOnly = true;
            this.dgvOperators.RowTemplate.Height = 23;
            this.dgvOperators.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // f_OperatorID
            // 
            resources.ApplyResources(this.f_OperatorID, "f_OperatorID");
            this.f_OperatorID.Name = "f_OperatorID";
            this.f_OperatorID.ReadOnly = true;
            // 
            // f_OperatorName
            // 
            this.f_OperatorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_OperatorName, "f_OperatorName");
            this.f_OperatorName.Name = "f_OperatorName";
            this.f_OperatorName.ReadOnly = true;
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
            this.btnSetPassword,
            this.btnEditPrivilege,
            this.btnEditDepartment,
            this.btnEditZones});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_new;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Image = global::Properties.Resources.icon_edit;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = global::Properties.Resources.icon_delete;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSetPassword
            // 
            resources.ApplyResources(this.btnSetPassword, "btnSetPassword");
            this.btnSetPassword.ForeColor = System.Drawing.Color.White;
            this.btnSetPassword.Image = global::Properties.Resources.icon_operator_setpassword;
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            // 
            // btnEditPrivilege
            // 
            resources.ApplyResources(this.btnEditPrivilege, "btnEditPrivilege");
            this.btnEditPrivilege.ForeColor = System.Drawing.Color.White;
            this.btnEditPrivilege.Image = global::Properties.Resources.icon_change_privilege;
            this.btnEditPrivilege.Name = "btnEditPrivilege";
            this.btnEditPrivilege.Click += new System.EventHandler(this.btnEditPrivilege_Click);
            // 
            // btnEditDepartment
            // 
            resources.ApplyResources(this.btnEditDepartment, "btnEditDepartment");
            this.btnEditDepartment.ForeColor = System.Drawing.Color.White;
            this.btnEditDepartment.Image = global::Properties.Resources.icon_operator_group;
            this.btnEditDepartment.Name = "btnEditDepartment";
            this.btnEditDepartment.Click += new System.EventHandler(this.btnEditDepartment_Click);
            // 
            // btnEditZones
            // 
            resources.ApplyResources(this.btnEditZones, "btnEditZones");
            this.btnEditZones.ForeColor = System.Drawing.Color.White;
            this.btnEditZones.Image = global::Properties.Resources.icon_operator_zone;
            this.btnEditZones.Name = "btnEditZones";
            this.btnEditZones.Click += new System.EventHandler(this.btnEditZones_Click);
            // 
            // dfrmOperator
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dgvOperators);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "dfrmOperator";
            this.Load += new System.EventHandler(this.dfrmOperator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperators)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

