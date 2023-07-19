namespace WG3000_COMM.ExtendFunc.Meeting
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmMeetingAdr
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
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmMeetingAdr));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvSelected = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelectReader = new System.Windows.Forms.ImageButton();
            this.Label11 = new System.Windows.Forms.Label();
            this.lstMeetingAdr = new System.Windows.Forms.ListBox();
            this.btnAddMeetingAdr = new System.Windows.Forms.ImageButton();
            this.btnDeleteMeetingAdr = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // GroupBox1
            // 
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Controls.Add(this.dgvSelected);
            this.GroupBox1.Controls.Add(this.btnSelectReader);
            this.GroupBox1.Controls.Add(this.Label11);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // dgvSelected
            // 
            this.dgvSelected.AllowUserToAddRows = false;
            this.dgvSelected.AllowUserToDeleteRows = false;
            this.dgvSelected.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelected, "dgvSelected");
            this.dgvSelected.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelected.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelected.EnableHeadersVisualStyles = false;
            this.dgvSelected.Name = "dgvSelected";
            this.dgvSelected.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelected.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelected.RowTemplate.Height = 23;
            this.dgvSelected.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // btnSelectReader
            // 
            this.btnSelectReader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnSelectReader, "btnSelectReader");
            this.btnSelectReader.Focusable = true;
            this.btnSelectReader.ForeColor = System.Drawing.Color.White;
            this.btnSelectReader.Name = "btnSelectReader";
            this.btnSelectReader.Toggle = false;
            this.btnSelectReader.UseVisualStyleBackColor = false;
            this.btnSelectReader.Click += new System.EventHandler(this.btnSelectReader_Click);
            // 
            // Label11
            // 
            this.Label11.BackColor = System.Drawing.Color.Transparent;
            this.Label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label11, "Label11");
            this.Label11.Name = "Label11";
            // 
            // lstMeetingAdr
            // 
            resources.ApplyResources(this.lstMeetingAdr, "lstMeetingAdr");
            this.lstMeetingAdr.Name = "lstMeetingAdr";
            this.lstMeetingAdr.SelectedIndexChanged += new System.EventHandler(this.lstMeetingAdr_SelectedIndexChanged);
            this.lstMeetingAdr.DoubleClick += new System.EventHandler(this.lstMeetingAdr_DoubleClick);
            // 
            // btnAddMeetingAdr
            // 
            this.btnAddMeetingAdr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddMeetingAdr, "btnAddMeetingAdr");
            this.btnAddMeetingAdr.Focusable = true;
            this.btnAddMeetingAdr.ForeColor = System.Drawing.Color.White;
            this.btnAddMeetingAdr.Name = "btnAddMeetingAdr";
            this.btnAddMeetingAdr.Toggle = false;
            this.btnAddMeetingAdr.UseVisualStyleBackColor = false;
            this.btnAddMeetingAdr.Click += new System.EventHandler(this.btnAddMeetingAdr_Click);
            // 
            // btnDeleteMeetingAdr
            // 
            this.btnDeleteMeetingAdr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteMeetingAdr, "btnDeleteMeetingAdr");
            this.btnDeleteMeetingAdr.Focusable = true;
            this.btnDeleteMeetingAdr.ForeColor = System.Drawing.Color.White;
            this.btnDeleteMeetingAdr.Name = "btnDeleteMeetingAdr";
            this.btnDeleteMeetingAdr.Toggle = false;
            this.btnDeleteMeetingAdr.UseVisualStyleBackColor = false;
            this.btnDeleteMeetingAdr.Click += new System.EventHandler(this.btnDeleteMeetingAdr_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmMeetingAdr
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.btnDeleteMeetingAdr);
            this.Controls.Add(this.btnAddMeetingAdr);
            this.Controls.Add(this.lstMeetingAdr);
            this.Controls.Add(this.GroupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmMeetingAdr";
            this.Load += new System.EventHandler(this.dfrmMeetingAdr_Load);
            this.GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal ImageButton btnAddMeetingAdr;
        internal ImageButton btnCancel;
        internal ImageButton btnDeleteMeetingAdr;
        internal ImageButton btnSelectReader;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridView dgvSelected;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal Label Label11;
        internal ListBox lstMeetingAdr;
        private Panel panelBottomBanner;
    }
}

