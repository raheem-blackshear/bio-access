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

    public partial class dfrmHolidaySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmHolidaySet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnAddHoliday = new System.Windows.Forms.ImageButton();
            this.btnDelHoliday = new System.Windows.Forms.ImageButton();
            this.btnAddNeedWork = new System.Windows.Forms.ImageButton();
            this.btnDelNeedWork = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.optSunWork2 = new System.Windows.Forms.RadioButton();
            this.optSunWork0 = new System.Windows.Forms.RadioButton();
            this.optSunWork1 = new System.Windows.Forms.RadioButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.optSatWork2 = new System.Windows.Forms.RadioButton();
            this.optSatWork0 = new System.Windows.Forms.RadioButton();
            this.optSatWork1 = new System.Windows.Forms.RadioButton();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvMain2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain2)).BeginInit();
            this.SuspendLayout();
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
            // btnAddNeedWork
            // 
            this.btnAddNeedWork.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddNeedWork, "btnAddNeedWork");
            this.btnAddNeedWork.Focusable = true;
            this.btnAddNeedWork.ForeColor = System.Drawing.Color.White;
            this.btnAddNeedWork.Name = "btnAddNeedWork";
            this.btnAddNeedWork.Toggle = false;
            this.btnAddNeedWork.UseVisualStyleBackColor = false;
            this.btnAddNeedWork.Click += new System.EventHandler(this.btnAddNeedWork_Click);
            // 
            // btnDelNeedWork
            // 
            this.btnDelNeedWork.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelNeedWork, "btnDelNeedWork");
            this.btnDelNeedWork.Focusable = true;
            this.btnDelNeedWork.ForeColor = System.Drawing.Color.White;
            this.btnDelNeedWork.Name = "btnDelNeedWork";
            this.btnDelNeedWork.Toggle = false;
            this.btnDelNeedWork.UseVisualStyleBackColor = false;
            this.btnDelNeedWork.Click += new System.EventHandler(this.btnDelNeedWork_Click);
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
            // optSunWork2
            // 
            resources.ApplyResources(this.optSunWork2, "optSunWork2");
            this.optSunWork2.Name = "optSunWork2";
            // 
            // optSunWork0
            // 
            this.optSunWork0.Checked = true;
            resources.ApplyResources(this.optSunWork0, "optSunWork0");
            this.optSunWork0.Name = "optSunWork0";
            this.optSunWork0.TabStop = true;
            // 
            // optSunWork1
            // 
            resources.ApplyResources(this.optSunWork1, "optSunWork1");
            this.optSunWork1.Name = "optSunWork1";
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.optSatWork2);
            this.GroupBox1.Controls.Add(this.optSatWork0);
            this.GroupBox1.Controls.Add(this.optSatWork1);
            this.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // optSatWork2
            // 
            resources.ApplyResources(this.optSatWork2, "optSatWork2");
            this.optSatWork2.Name = "optSatWork2";
            // 
            // optSatWork0
            // 
            this.optSatWork0.Checked = true;
            resources.ApplyResources(this.optSatWork0, "optSatWork0");
            this.optSatWork0.Name = "optSatWork0";
            this.optSatWork0.TabStop = true;
            // 
            // optSatWork1
            // 
            resources.ApplyResources(this.optSatWork1, "optSatWork1");
            this.optSatWork1.Name = "optSatWork1";
            // 
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.Controls.Add(this.optSunWork2);
            this.GroupBox2.Controls.Add(this.optSunWork0);
            this.GroupBox2.Controls.Add(this.optSunWork1);
            this.GroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.GroupBox2, "GroupBox2");
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.TabStop = false;
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
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_Name,
            this.f_from,
            this.From1,
            this.f_to,
            this.To1,
            this.f_Note,
            this.f_No,
            this.f_Value});
            this.dgvMain.EnableHeadersVisualStyles = false;
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // f_Name
            // 
            resources.ApplyResources(this.f_Name, "f_Name");
            this.f_Name.Name = "f_Name";
            this.f_Name.ReadOnly = true;
            // 
            // f_from
            // 
            resources.ApplyResources(this.f_from, "f_from");
            this.f_from.Name = "f_from";
            this.f_from.ReadOnly = true;
            // 
            // From1
            // 
            resources.ApplyResources(this.From1, "From1");
            this.From1.Name = "From1";
            this.From1.ReadOnly = true;
            // 
            // f_to
            // 
            resources.ApplyResources(this.f_to, "f_to");
            this.f_to.Name = "f_to";
            this.f_to.ReadOnly = true;
            // 
            // To1
            // 
            resources.ApplyResources(this.To1, "To1");
            this.To1.Name = "To1";
            this.To1.ReadOnly = true;
            // 
            // f_Note
            // 
            resources.ApplyResources(this.f_Note, "f_Note");
            this.f_Note.Name = "f_Note";
            this.f_Note.ReadOnly = true;
            // 
            // f_No
            // 
            resources.ApplyResources(this.f_No, "f_No");
            this.f_No.Name = "f_No";
            this.f_No.ReadOnly = true;
            // 
            // f_Value
            // 
            resources.ApplyResources(this.f_Value, "f_Value");
            this.f_Value.Name = "f_Value";
            this.f_Value.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label2.Name = "label2";
            // 
            // dgvMain2
            // 
            this.dgvMain2.AllowUserToAddRows = false;
            this.dgvMain2.AllowUserToDeleteRows = false;
            this.dgvMain2.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMain2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.From2,
            this.dataGridViewTextBoxColumn3,
            this.To2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dgvMain2.EnableHeadersVisualStyles = false;
            resources.ApplyResources(this.dgvMain2, "dgvMain2");
            this.dgvMain2.Name = "dgvMain2";
            this.dgvMain2.ReadOnly = true;
            this.dgvMain2.RowTemplate.Height = 23;
            this.dgvMain2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            // From2
            // 
            resources.ApplyResources(this.From2, "From2");
            this.From2.Name = "From2";
            this.From2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // To2
            // 
            resources.ApplyResources(this.To2, "To2");
            this.To2.Name = "To2";
            this.To2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dfrmHolidaySet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dgvMain2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.btnAddHoliday);
            this.Controls.Add(this.btnDelHoliday);
            this.Controls.Add(this.btnAddNeedWork);
            this.Controls.Add(this.btnDelNeedWork);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmHolidaySet";
            this.Load += new System.EventHandler(this.dfrmHolidaySet_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnAddHoliday;
        internal ImageButton btnAddNeedWork;
        internal ImageButton btnCancel;
        internal ImageButton btnDelHoliday;
        internal ImageButton btnDelNeedWork;
        internal ImageButton btnOK;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridView dgvMain;
        private DataGridView dgvMain2;
        private DataGridViewTextBoxColumn f_from;
        private DataGridViewTextBoxColumn f_Name;
        private DataGridViewTextBoxColumn f_No;
        private DataGridViewTextBoxColumn f_Note;
        private DataGridViewTextBoxColumn f_to;
        private DataGridViewTextBoxColumn f_Value;
        private DataGridViewTextBoxColumn From1;
        private DataGridViewTextBoxColumn From2;
        internal GroupBox GroupBox1;
        internal GroupBox GroupBox2;
        private Label label1;
        private Label label2;
        internal RadioButton optSatWork0;
        internal RadioButton optSatWork1;
        internal RadioButton optSatWork2;
        internal RadioButton optSunWork0;
        internal RadioButton optSunWork1;
        internal RadioButton optSunWork2;
        private DataGridViewTextBoxColumn To1;
        private DataGridViewTextBoxColumn To2;
    }
}

