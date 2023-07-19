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

    public partial class dfrmControlHolidaySet
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
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControlHolidaySet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvNeedWork = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddNeedWorkDay = new System.Windows.Forms.ImageButton();
            this.btnDelNeedWorkDay = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddHoliday = new System.Windows.Forms.ImageButton();
            this.btnDelHoliday = new System.Windows.Forms.ImageButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNeedWork)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.dgvNeedWork);
            this.groupBox2.Controls.Add(this.btnAddNeedWorkDay);
            this.groupBox2.Controls.Add(this.btnDelNeedWorkDay);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // dgvNeedWork
            // 
            this.dgvNeedWork.AllowUserToAddRows = false;
            this.dgvNeedWork.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvNeedWork, "dgvNeedWork");
            this.dgvNeedWork.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNeedWork.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNeedWork.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvNeedWork.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvNeedWork.EnableHeadersVisualStyles = false;
            this.dgvNeedWork.Name = "dgvNeedWork";
            this.dgvNeedWork.ReadOnly = true;
            this.dgvNeedWork.RowTemplate.Height = 23;
            this.dgvNeedWork.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
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
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // btnAddNeedWorkDay
            // 
            this.btnAddNeedWorkDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddNeedWorkDay, "btnAddNeedWorkDay");
            this.btnAddNeedWorkDay.Focusable = true;
            this.btnAddNeedWorkDay.ForeColor = System.Drawing.Color.White;
            this.btnAddNeedWorkDay.Name = "btnAddNeedWorkDay";
            this.btnAddNeedWorkDay.Toggle = false;
            this.btnAddNeedWorkDay.UseVisualStyleBackColor = false;
            this.btnAddNeedWorkDay.Click += new System.EventHandler(this.btnAddNeedWorkDay_Click);
            // 
            // btnDelNeedWorkDay
            // 
            this.btnDelNeedWorkDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelNeedWorkDay, "btnDelNeedWorkDay");
            this.btnDelNeedWorkDay.Focusable = true;
            this.btnDelNeedWorkDay.ForeColor = System.Drawing.Color.White;
            this.btnDelNeedWorkDay.Name = "btnDelNeedWorkDay";
            this.btnDelNeedWorkDay.Toggle = false;
            this.btnDelNeedWorkDay.UseVisualStyleBackColor = false;
            this.btnDelNeedWorkDay.Click += new System.EventHandler(this.btnDelNeedWorkDay_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.dgvMain);
            this.groupBox1.Controls.Add(this.btnAddHoliday);
            this.groupBox1.Controls.Add(this.btnDelHoliday);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_No,
            this.f_from,
            this.f_to,
            this.f_Note});
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // f_No
            // 
            resources.ApplyResources(this.f_No, "f_No");
            this.f_No.Name = "f_No";
            this.f_No.ReadOnly = true;
            // 
            // f_from
            // 
            resources.ApplyResources(this.f_from, "f_from");
            this.f_from.Name = "f_from";
            this.f_from.ReadOnly = true;
            // 
            // f_to
            // 
            resources.ApplyResources(this.f_to, "f_to");
            this.f_to.Name = "f_to";
            this.f_to.ReadOnly = true;
            // 
            // f_Note
            // 
            this.f_Note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_Note, "f_Note");
            this.f_Note.Name = "f_Note";
            this.f_Note.ReadOnly = true;
            // 
            // btnAddHoliday
            // 
            this.btnAddHoliday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddHoliday, "btnAddHoliday");
            this.btnAddHoliday.Focusable = true;
            this.btnAddHoliday.ForeColor = System.Drawing.Color.White;
            this.btnAddHoliday.Name = "btnAddHoliday";
            this.btnAddHoliday.Toggle = false;
            this.btnAddHoliday.UseVisualStyleBackColor = false;
            this.btnAddHoliday.Click += new System.EventHandler(this.btnAddHoliday_Click);
            // 
            // btnDelHoliday
            // 
            this.btnDelHoliday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelHoliday, "btnDelHoliday");
            this.btnDelHoliday.Focusable = true;
            this.btnDelHoliday.ForeColor = System.Drawing.Color.White;
            this.btnDelHoliday.Name = "btnDelHoliday";
            this.btnDelHoliday.Toggle = false;
            this.btnDelHoliday.UseVisualStyleBackColor = false;
            this.btnDelHoliday.Click += new System.EventHandler(this.btnDelHoliday_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
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
            // dfrmControlHolidaySet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmControlHolidaySet";
            this.Load += new System.EventHandler(this.dfrmHolidaySet_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNeedWork)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnAddHoliday;
        internal ImageButton btnAddNeedWorkDay;
        internal ImageButton btnCancel;
        internal ImageButton btnDelHoliday;
        internal ImageButton btnDelNeedWorkDay;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView dgvMain;
        private DataGridView dgvNeedWork;
        private DataGridViewTextBoxColumn f_from;
        private DataGridViewTextBoxColumn f_No;
        private DataGridViewTextBoxColumn f_Note;
        private DataGridViewTextBoxColumn f_to;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
    }
}

