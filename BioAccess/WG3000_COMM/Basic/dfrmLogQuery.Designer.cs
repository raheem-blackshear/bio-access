namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmLogQuery
    {
        private ImageButton btnClose;
        private IContainer components = null;
        private dfrmFind dfrmFind1;
        private DataGridView dgvMain;
        private DataTable dt;
        private DataGridViewTextBoxColumn f_EventDesc;
        private DataGridViewTextBoxColumn f_EventType;
        private DataGridViewTextBoxColumn f_LogDateTime;
        private DataGridViewTextBoxColumn f_RecID;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmLogQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_LogDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_EventType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_EventDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_RecID,
            this.f_LogDateTime,
            this.f_EventType,
            this.f_EventDesc});
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
            this.dgvMain.DoubleClick += new System.EventHandler(this.dgvMain_DoubleClick);
            // 
            // f_RecID
            // 
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
            // 
            // f_LogDateTime
            // 
            resources.ApplyResources(this.f_LogDateTime, "f_LogDateTime");
            this.f_LogDateTime.Name = "f_LogDateTime";
            this.f_LogDateTime.ReadOnly = true;
            // 
            // f_EventType
            // 
            resources.ApplyResources(this.f_EventType, "f_EventType");
            this.f_EventType.Name = "f_EventType";
            this.f_EventType.ReadOnly = true;
            // 
            // f_EventDesc
            // 
            this.f_EventDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_EventDesc, "f_EventDesc");
            this.f_EventDesc.Name = "f_EventDesc";
            this.f_EventDesc.ReadOnly = true;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnClose);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmLogQuery
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.dgvMain);
            this.MinimizeBox = false;
            this.Name = "dfrmLogQuery";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmLogQuery_FormClosing);
            this.Load += new System.EventHandler(this.dfrmLogQuery_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmLogQuery_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel panelBottomBanner;
    }
}

