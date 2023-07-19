namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmOperatePrivilege
    {
        private ImageButton btnCancel;
        private ImageButton btnFullControlAllOn;
        private ImageButton btnFullControlOff;
        private ImageButton btnOK;
        private ImageButton btnReadAllOff;
        private ImageButton btnReadAllOn;
        private IContainer components = null;
        private DataGridView dgvOperatePrivilege;
        private DataView dv;
        private DataGridViewTextBoxColumn f_DisplayID;
        private DataGridViewCheckBoxColumn f_FullControl;
        private DataGridViewTextBoxColumn f_FunctionDisplayName;
        private DataGridViewTextBoxColumn f_FunctionID;
        private DataGridViewTextBoxColumn f_FunctionName;
        private DataGridViewCheckBoxColumn f_ReadOnly;
        private DataTable tb;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmOperatePrivilege));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvOperatePrivilege = new System.Windows.Forms.DataGridView();
            this.f_FunctionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_FunctionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_FunctionDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReadOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_FullControl = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_DisplayID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReadAllOff = new System.Windows.Forms.ImageButton();
            this.btnFullControlOff = new System.Windows.Forms.ImageButton();
            this.btnReadAllOn = new System.Windows.Forms.ImageButton();
            this.btnFullControlAllOn = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperatePrivilege)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOperatePrivilege
            // 
            this.dgvOperatePrivilege.AllowUserToAddRows = false;
            this.dgvOperatePrivilege.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvOperatePrivilege, "dgvOperatePrivilege");
            this.dgvOperatePrivilege.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOperatePrivilege.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOperatePrivilege.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOperatePrivilege.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_FunctionID,
            this.f_FunctionName,
            this.f_FunctionDisplayName,
            this.f_ReadOnly,
            this.f_FullControl,
            this.f_DisplayID});
            this.dgvOperatePrivilege.EnableHeadersVisualStyles = false;
            this.dgvOperatePrivilege.Name = "dgvOperatePrivilege";
            this.dgvOperatePrivilege.RowHeadersVisible = false;
            this.dgvOperatePrivilege.RowTemplate.Height = 23;
            this.dgvOperatePrivilege.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvOperatePrivilege_CellFormatting);
            // 
            // f_FunctionID
            // 
            resources.ApplyResources(this.f_FunctionID, "f_FunctionID");
            this.f_FunctionID.Name = "f_FunctionID";
            // 
            // f_FunctionName
            // 
            resources.ApplyResources(this.f_FunctionName, "f_FunctionName");
            this.f_FunctionName.Name = "f_FunctionName";
            // 
            // f_FunctionDisplayName
            // 
            resources.ApplyResources(this.f_FunctionDisplayName, "f_FunctionDisplayName");
            this.f_FunctionDisplayName.Name = "f_FunctionDisplayName";
            this.f_FunctionDisplayName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_ReadOnly
            // 
            resources.ApplyResources(this.f_ReadOnly, "f_ReadOnly");
            this.f_ReadOnly.Name = "f_ReadOnly";
            // 
            // f_FullControl
            // 
            resources.ApplyResources(this.f_FullControl, "f_FullControl");
            this.f_FullControl.Name = "f_FullControl";
            // 
            // f_DisplayID
            // 
            resources.ApplyResources(this.f_DisplayID, "f_DisplayID");
            this.f_DisplayID.Name = "f_DisplayID";
            // 
            // btnReadAllOff
            // 
            this.btnReadAllOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnReadAllOff, "btnReadAllOff");
            this.btnReadAllOff.Focusable = true;
            this.btnReadAllOff.ForeColor = System.Drawing.Color.White;
            this.btnReadAllOff.Name = "btnReadAllOff";
            this.btnReadAllOff.Toggle = false;
            this.btnReadAllOff.UseVisualStyleBackColor = false;
            this.btnReadAllOff.Click += new System.EventHandler(this.btnReadAllOff_Click);
            // 
            // btnFullControlOff
            // 
            this.btnFullControlOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnFullControlOff, "btnFullControlOff");
            this.btnFullControlOff.Focusable = true;
            this.btnFullControlOff.ForeColor = System.Drawing.Color.White;
            this.btnFullControlOff.Name = "btnFullControlOff";
            this.btnFullControlOff.Toggle = false;
            this.btnFullControlOff.UseVisualStyleBackColor = false;
            this.btnFullControlOff.Click += new System.EventHandler(this.btnFullControlOff_Click);
            // 
            // btnReadAllOn
            // 
            this.btnReadAllOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnReadAllOn, "btnReadAllOn");
            this.btnReadAllOn.Focusable = true;
            this.btnReadAllOn.ForeColor = System.Drawing.Color.White;
            this.btnReadAllOn.Name = "btnReadAllOn";
            this.btnReadAllOn.Toggle = false;
            this.btnReadAllOn.UseVisualStyleBackColor = false;
            this.btnReadAllOn.Click += new System.EventHandler(this.btnReadAllOn_Click);
            // 
            // btnFullControlAllOn
            // 
            this.btnFullControlAllOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnFullControlAllOn, "btnFullControlAllOn");
            this.btnFullControlAllOn.Focusable = true;
            this.btnFullControlAllOn.ForeColor = System.Drawing.Color.White;
            this.btnFullControlAllOn.Name = "btnFullControlAllOn";
            this.btnFullControlAllOn.Toggle = false;
            this.btnFullControlAllOn.UseVisualStyleBackColor = false;
            this.btnFullControlAllOn.Click += new System.EventHandler(this.btnFullControlAllOn_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dfrmOperatePrivilege
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnFullControlAllOn);
            this.Controls.Add(this.btnReadAllOn);
            this.Controls.Add(this.dgvOperatePrivilege);
            this.Controls.Add(this.btnFullControlOff);
            this.Controls.Add(this.btnReadAllOff);
            this.MinimizeBox = false;
            this.Name = "dfrmOperatePrivilege";
            this.Load += new System.EventHandler(this.dfrmOperatePrivilege_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperatePrivilege)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

