namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControllerTaskList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerTaskList));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvTaskList = new System.Windows.Forms.DataGridView();
            this.f_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OperateHMS1A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Monday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Tuesday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Wednesday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Thursday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Friday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Saturday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_Sunday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_AdaptTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorControlDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorControl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cboAccessMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDoors = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox49 = new System.Windows.Forms.CheckBox();
            this.checkBox48 = new System.Windows.Forms.CheckBox();
            this.checkBox47 = new System.Windows.Forms.CheckBox();
            this.checkBox46 = new System.Windows.Forms.CheckBox();
            this.checkBox45 = new System.Windows.Forms.CheckBox();
            this.checkBox44 = new System.Windows.Forms.CheckBox();
            this.checkBox43 = new System.Windows.Forms.CheckBox();
            this.label45 = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.btnEdit = new System.Windows.Forms.ImageButton();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnDel = new System.Windows.Forms.ImageButton();
            this.btnAdd = new System.Windows.Forms.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // dgvTaskList
            // 
            this.dgvTaskList.AllowUserToAddRows = false;
            this.dgvTaskList.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvTaskList, "dgvTaskList");
            this.dgvTaskList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTaskList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTaskList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTaskList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ID,
            this.f_From,
            this.f_To,
            this.f_OperateHMS1A,
            this.f_Monday,
            this.f_Tuesday,
            this.f_Wednesday,
            this.f_Thursday,
            this.f_Friday,
            this.f_Saturday,
            this.f_Sunday,
            this.f_AdaptTo,
            this.f_DoorControlDesc,
            this.f_Note,
            this.f_DoorControl,
            this.f_DoorID});
            this.dgvTaskList.EnableHeadersVisualStyles = false;
            this.dgvTaskList.Name = "dgvTaskList";
            this.dgvTaskList.ReadOnly = true;
            this.dgvTaskList.RowTemplate.Height = 23;
            this.dgvTaskList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTaskList.DoubleClick += new System.EventHandler(this.dgvTaskList_DoubleClick);
            // 
            // f_ID
            // 
            resources.ApplyResources(this.f_ID, "f_ID");
            this.f_ID.Name = "f_ID";
            this.f_ID.ReadOnly = true;
            // 
            // f_From
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_From.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_From, "f_From");
            this.f_From.Name = "f_From";
            this.f_From.ReadOnly = true;
            this.f_From.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_To
            // 
            resources.ApplyResources(this.f_To, "f_To");
            this.f_To.Name = "f_To";
            this.f_To.ReadOnly = true;
            this.f_To.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_To.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_OperateHMS1A
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_OperateHMS1A.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_OperateHMS1A, "f_OperateHMS1A");
            this.f_OperateHMS1A.Name = "f_OperateHMS1A";
            this.f_OperateHMS1A.ReadOnly = true;
            this.f_OperateHMS1A.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_OperateHMS1A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // f_AdaptTo
            // 
            resources.ApplyResources(this.f_AdaptTo, "f_AdaptTo");
            this.f_AdaptTo.Name = "f_AdaptTo";
            this.f_AdaptTo.ReadOnly = true;
            // 
            // f_DoorControlDesc
            // 
            resources.ApplyResources(this.f_DoorControlDesc, "f_DoorControlDesc");
            this.f_DoorControlDesc.Name = "f_DoorControlDesc";
            this.f_DoorControlDesc.ReadOnly = true;
            // 
            // f_Note
            // 
            this.f_Note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_Note, "f_Note");
            this.f_Note.Name = "f_Note";
            this.f_Note.ReadOnly = true;
            // 
            // f_DoorControl
            // 
            resources.ApplyResources(this.f_DoorControl, "f_DoorControl");
            this.f_DoorControl.Name = "f_DoorControl";
            this.f_DoorControl.ReadOnly = true;
            // 
            // f_DoorID
            // 
            resources.ApplyResources(this.f_DoorID, "f_DoorID");
            this.f_DoorID.Name = "f_DoorID";
            this.f_DoorID.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label2.Name = "label2";
            // 
            // cboAccessMethod
            // 
            this.cboAccessMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAccessMethod.FormattingEnabled = true;
            this.cboAccessMethod.Items.AddRange(new object[] {
            resources.GetString("cboAccessMethod.Items"),
            resources.GetString("cboAccessMethod.Items1"),
            resources.GetString("cboAccessMethod.Items2"),
            resources.GetString("cboAccessMethod.Items3"),
            resources.GetString("cboAccessMethod.Items4")});
            resources.ApplyResources(this.cboAccessMethod, "cboAccessMethod");
            this.cboAccessMethod.Name = "cboAccessMethod";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // cboDoors
            // 
            this.cboDoors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDoors.FormattingEnabled = true;
            resources.ApplyResources(this.cboDoors, "cboDoors");
            this.cboDoors.Name = "cboDoors";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.checkBox49);
            this.groupBox2.Controls.Add(this.checkBox48);
            this.groupBox2.Controls.Add(this.checkBox47);
            this.groupBox2.Controls.Add(this.checkBox46);
            this.groupBox2.Controls.Add(this.checkBox45);
            this.groupBox2.Controls.Add(this.checkBox44);
            this.groupBox2.Controls.Add(this.checkBox43);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBox49
            // 
            resources.ApplyResources(this.checkBox49, "checkBox49");
            this.checkBox49.Checked = true;
            this.checkBox49.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox49.Name = "checkBox49";
            this.checkBox49.UseVisualStyleBackColor = true;
            // 
            // checkBox48
            // 
            resources.ApplyResources(this.checkBox48, "checkBox48");
            this.checkBox48.Checked = true;
            this.checkBox48.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox48.Name = "checkBox48";
            this.checkBox48.UseVisualStyleBackColor = true;
            // 
            // checkBox47
            // 
            resources.ApplyResources(this.checkBox47, "checkBox47");
            this.checkBox47.Checked = true;
            this.checkBox47.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox47.Name = "checkBox47";
            this.checkBox47.UseVisualStyleBackColor = true;
            // 
            // checkBox46
            // 
            resources.ApplyResources(this.checkBox46, "checkBox46");
            this.checkBox46.Checked = true;
            this.checkBox46.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox46.Name = "checkBox46";
            this.checkBox46.UseVisualStyleBackColor = true;
            // 
            // checkBox45
            // 
            resources.ApplyResources(this.checkBox45, "checkBox45");
            this.checkBox45.Checked = true;
            this.checkBox45.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox45.Name = "checkBox45";
            this.checkBox45.UseVisualStyleBackColor = true;
            // 
            // checkBox44
            // 
            resources.ApplyResources(this.checkBox44, "checkBox44");
            this.checkBox44.Checked = true;
            this.checkBox44.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox44.Name = "checkBox44";
            this.checkBox44.UseVisualStyleBackColor = true;
            // 
            // checkBox43
            // 
            resources.ApplyResources(this.checkBox43, "checkBox43");
            this.checkBox43.Checked = true;
            this.checkBox43.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox43.Name = "checkBox43";
            this.checkBox43.UseVisualStyleBackColor = true;
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.BackColor = System.Drawing.Color.Transparent;
            this.label45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label45.Name = "label45";
            // 
            // dtpTime
            // 
            resources.ApplyResources(this.dtpTime, "dtpTime");
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Value = new System.DateTime(2011, 11, 30, 0, 0, 0, 0);
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.BackColor = System.Drawing.Color.Transparent;
            this.label43.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label43.Name = "label43";
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.BackColor = System.Drawing.Color.Transparent;
            this.label44.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label44.Name = "label44";
            // 
            // dtpEnd
            // 
            resources.ApplyResources(this.dtpEnd, "dtpEnd");
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Value = new System.DateTime(2029, 12, 31, 14, 44, 0, 0);
            // 
            // dtpBegin
            // 
            resources.ApplyResources(this.dtpBegin, "dtpBegin");
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Value = new System.DateTime(2010, 1, 1, 18, 18, 0, 0);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Focusable = true;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Toggle = false;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDel, "btnDel");
            this.btnDel.Focusable = true;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            this.btnDel.Name = "btnDel";
            this.btnDel.Toggle = false;
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Focusable = true;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Toggle = false;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dfrmControllerTaskList
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.dgvTaskList);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboAccessMethod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboDoors);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpBegin);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.MinimizeBox = false;
            this.Name = "dfrmControllerTaskList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerTaskList_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerTaskList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControllerTaskList_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ImageButton btnAdd;
        private ImageButton btnClose;
        private ImageButton btnDel;
        private ImageButton btnEdit;
        private ComboBox cboAccessMethod;
        private ComboBox cboDoors;
        private CheckBox checkBox43;
        private CheckBox checkBox44;
        private CheckBox checkBox45;
        private CheckBox checkBox46;
        private CheckBox checkBox47;
        private CheckBox checkBox48;
        private CheckBox checkBox49;
        private DataGridView dgvTaskList;
        private DateTimePicker dtpBegin;
        private DateTimePicker dtpEnd;
        private DateTimePicker dtpTime;
        private DataGridViewTextBoxColumn f_AdaptTo;
        private DataGridViewTextBoxColumn f_DoorControl;
        private DataGridViewTextBoxColumn f_DoorControlDesc;
        private DataGridViewTextBoxColumn f_DoorID;
        private DataGridViewCheckBoxColumn f_Friday;
        private DataGridViewTextBoxColumn f_From;
        private DataGridViewTextBoxColumn f_ID;
        private DataGridViewCheckBoxColumn f_Monday;
        private DataGridViewTextBoxColumn f_Note;
        private DataGridViewTextBoxColumn f_OperateHMS1A;
        private DataGridViewCheckBoxColumn f_Saturday;
        private DataGridViewCheckBoxColumn f_Sunday;
        private DataGridViewCheckBoxColumn f_Thursday;
        private DataGridViewTextBoxColumn f_To;
        private DataGridViewCheckBoxColumn f_Tuesday;
        private DataGridViewCheckBoxColumn f_Wednesday;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label43;
        private Label label44;
        private Label label45;
        private TextBox textBox1;
    }
}

