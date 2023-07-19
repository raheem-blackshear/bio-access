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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmControlSeg
    {
        private ComboBox cbof_ControlSegID;
        private ComboBox cbof_ControlSegIDLinked;
        private CheckBox chkf_ReaderCount;
        private CheckBox chkFriday;
        private CheckBox chkMonday;
        private CheckBox chkNotAllowInHolidays;
        private CheckBox chkSaturday;
        private CheckBox chkSunday;
        private CheckBox chkThursday;
        private CheckBox chkTuesday;
        private CheckBox chkWednesday;
        internal ImageButton cmdCancel;
        internal ImageButton cmdOK;
        private IContainer components = null;
        public int curControlSegID;
        private DateTimePicker dateBeginHMS1;
        private DateTimePicker dateBeginHMS2;
        private DateTimePicker dateBeginHMS3;
        private DateTimePicker dateEndHMS1;
        private DateTimePicker dateEndHMS2;
        private DateTimePicker dateEndHMS3;
        private DateTimePicker dtpBegin;
        private DateTimePicker dtpEnd;
        private GroupBox groupBox1;
        private GroupBox groupBox10;
        private GroupBox groupBox11;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label81;
        private Label label82;
        private Label label83;
        private Label label84;
        private Label label85;
        private Label label86;
        private Label label87;
        private Label label88;
        private Label label89;
        private Label label90;
        private Label label91;
        private Label label92;
        private Label label93;
        private Label label94;
        private NumericUpDown nudf_LimitedTimesOfDay;
        private NumericUpDown nudf_LimitedTimesOfHMS1;
        private NumericUpDown nudf_LimitedTimesOfHMS2;
        private NumericUpDown nudf_LimitedTimesOfHMS3;
        private NumericUpDown nudf_LimitedTimesOfMonth;
        private RadioButton optControllerCount;
        private RadioButton optReaderCount;
        internal TextBox txtf_ControlSegName;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControlSeg));
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.cmdOK = new System.Windows.Forms.ImageButton();
            this.cmdCancel = new System.Windows.Forms.ImageButton();
            this.chkNotAllowInHolidays = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtf_ControlSegName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudf_LimitedTimesOfMonth = new System.Windows.Forms.NumericUpDown();
            this.optReaderCount = new System.Windows.Forms.RadioButton();
            this.optControllerCount = new System.Windows.Forms.RadioButton();
            this.nudf_LimitedTimesOfDay = new System.Windows.Forms.NumericUpDown();
            this.nudf_LimitedTimesOfHMS3 = new System.Windows.Forms.NumericUpDown();
            this.label93 = new System.Windows.Forms.Label();
            this.nudf_LimitedTimesOfHMS2 = new System.Windows.Forms.NumericUpDown();
            this.label92 = new System.Windows.Forms.Label();
            this.nudf_LimitedTimesOfHMS1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label94 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.chkf_ReaderCount = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label84 = new System.Windows.Forms.Label();
            this.cbof_ControlSegIDLinked = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.label82 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label89 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.dateBeginHMS3 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS3 = new System.Windows.Forms.DateTimePicker();
            this.label87 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.dateBeginHMS2 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS2 = new System.Windows.Forms.DateTimePicker();
            this.label86 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.dateEndHMS1 = new System.Windows.Forms.DateTimePicker();
            this.dateBeginHMS1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkMonday = new System.Windows.Forms.CheckBox();
            this.chkSunday = new System.Windows.Forms.CheckBox();
            this.chkTuesday = new System.Windows.Forms.CheckBox();
            this.chkSaturday = new System.Windows.Forms.CheckBox();
            this.chkWednesday = new System.Windows.Forms.CheckBox();
            this.chkFriday = new System.Windows.Forms.CheckBox();
            this.chkThursday = new System.Windows.Forms.CheckBox();
            this.label83 = new System.Windows.Forms.Label();
            this.cbof_ControlSegID = new System.Windows.Forms.ComboBox();
            this.panelBottomBanner.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfHMS3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfHMS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfHMS1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdOK);
            this.panelBottomBanner.Controls.Add(this.cmdCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // cmdOK
            // 
            resources.ApplyResources(this.cmdOK, "cmdOK");
            this.cmdOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdOK.Focusable = true;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Toggle = false;
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdCancel.Focusable = true;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Toggle = false;
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chkNotAllowInHolidays
            // 
            resources.ApplyResources(this.chkNotAllowInHolidays, "chkNotAllowInHolidays");
            this.chkNotAllowInHolidays.Checked = true;
            this.chkNotAllowInHolidays.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotAllowInHolidays.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkNotAllowInHolidays.Name = "chkNotAllowInHolidays";
            this.chkNotAllowInHolidays.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // txtf_ControlSegName
            // 
            resources.ApplyResources(this.txtf_ControlSegName, "txtf_ControlSegName");
            this.txtf_ControlSegName.Name = "txtf_ControlSegName";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfMonth);
            this.groupBox3.Controls.Add(this.optReaderCount);
            this.groupBox3.Controls.Add(this.optControllerCount);
            this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfDay);
            this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfHMS3);
            this.groupBox3.Controls.Add(this.label93);
            this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfHMS2);
            this.groupBox3.Controls.Add(this.label92);
            this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfHMS1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label94);
            this.groupBox3.Controls.Add(this.label91);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // nudf_LimitedTimesOfMonth
            // 
            resources.ApplyResources(this.nudf_LimitedTimesOfMonth, "nudf_LimitedTimesOfMonth");
            this.nudf_LimitedTimesOfMonth.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.nudf_LimitedTimesOfMonth.Name = "nudf_LimitedTimesOfMonth";
            this.nudf_LimitedTimesOfMonth.ReadOnly = true;
            // 
            // optReaderCount
            // 
            resources.ApplyResources(this.optReaderCount, "optReaderCount");
            this.optReaderCount.Name = "optReaderCount";
            this.optReaderCount.UseVisualStyleBackColor = true;
            // 
            // optControllerCount
            // 
            resources.ApplyResources(this.optControllerCount, "optControllerCount");
            this.optControllerCount.Checked = true;
            this.optControllerCount.Name = "optControllerCount";
            this.optControllerCount.TabStop = true;
            this.optControllerCount.UseVisualStyleBackColor = true;
            // 
            // nudf_LimitedTimesOfDay
            // 
            resources.ApplyResources(this.nudf_LimitedTimesOfDay, "nudf_LimitedTimesOfDay");
            this.nudf_LimitedTimesOfDay.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.nudf_LimitedTimesOfDay.Name = "nudf_LimitedTimesOfDay";
            this.nudf_LimitedTimesOfDay.ReadOnly = true;
            // 
            // nudf_LimitedTimesOfHMS3
            // 
            resources.ApplyResources(this.nudf_LimitedTimesOfHMS3, "nudf_LimitedTimesOfHMS3");
            this.nudf_LimitedTimesOfHMS3.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudf_LimitedTimesOfHMS3.Name = "nudf_LimitedTimesOfHMS3";
            this.nudf_LimitedTimesOfHMS3.ReadOnly = true;
            // 
            // label93
            // 
            resources.ApplyResources(this.label93, "label93");
            this.label93.Name = "label93";
            // 
            // nudf_LimitedTimesOfHMS2
            // 
            resources.ApplyResources(this.nudf_LimitedTimesOfHMS2, "nudf_LimitedTimesOfHMS2");
            this.nudf_LimitedTimesOfHMS2.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudf_LimitedTimesOfHMS2.Name = "nudf_LimitedTimesOfHMS2";
            this.nudf_LimitedTimesOfHMS2.ReadOnly = true;
            // 
            // label92
            // 
            resources.ApplyResources(this.label92, "label92");
            this.label92.Name = "label92";
            // 
            // nudf_LimitedTimesOfHMS1
            // 
            resources.ApplyResources(this.nudf_LimitedTimesOfHMS1, "nudf_LimitedTimesOfHMS1");
            this.nudf_LimitedTimesOfHMS1.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudf_LimitedTimesOfHMS1.Name = "nudf_LimitedTimesOfHMS1";
            this.nudf_LimitedTimesOfHMS1.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label94
            // 
            resources.ApplyResources(this.label94, "label94");
            this.label94.Name = "label94";
            // 
            // label91
            // 
            resources.ApplyResources(this.label91, "label91");
            this.label91.Name = "label91";
            // 
            // chkf_ReaderCount
            // 
            resources.ApplyResources(this.chkf_ReaderCount, "chkf_ReaderCount");
            this.chkf_ReaderCount.Name = "chkf_ReaderCount";
            this.chkf_ReaderCount.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label84);
            this.groupBox2.Controls.Add(this.cbof_ControlSegIDLinked);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label84
            // 
            resources.ApplyResources(this.label84, "label84");
            this.label84.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label84.Name = "label84";
            // 
            // cbof_ControlSegIDLinked
            // 
            this.cbof_ControlSegIDLinked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ControlSegIDLinked.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ControlSegIDLinked, "cbof_ControlSegIDLinked");
            this.cbof_ControlSegIDLinked.Name = "cbof_ControlSegIDLinked";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dtpBegin);
            this.groupBox1.Controls.Add(this.label82);
            this.groupBox1.Controls.Add(this.label81);
            this.groupBox1.Controls.Add(this.dtpEnd);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dtpBegin
            // 
            resources.ApplyResources(this.dtpBegin, "dtpBegin");
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Value = new System.DateTime(2010, 1, 1, 18, 18, 0, 0);
            // 
            // label82
            // 
            resources.ApplyResources(this.label82, "label82");
            this.label82.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label82.Name = "label82";
            // 
            // label81
            // 
            resources.ApplyResources(this.label81, "label81");
            this.label81.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label81.Name = "label81";
            // 
            // dtpEnd
            // 
            resources.ApplyResources(this.dtpEnd, "dtpEnd");
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Value = new System.DateTime(2029, 12, 31, 14, 44, 0, 0);
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.Transparent;
            this.groupBox11.Controls.Add(this.label89);
            this.groupBox11.Controls.Add(this.label90);
            this.groupBox11.Controls.Add(this.dateBeginHMS3);
            this.groupBox11.Controls.Add(this.dateEndHMS3);
            this.groupBox11.Controls.Add(this.label87);
            this.groupBox11.Controls.Add(this.label88);
            this.groupBox11.Controls.Add(this.dateBeginHMS2);
            this.groupBox11.Controls.Add(this.dateEndHMS2);
            this.groupBox11.Controls.Add(this.label86);
            this.groupBox11.Controls.Add(this.label85);
            this.groupBox11.Controls.Add(this.dateEndHMS1);
            this.groupBox11.Controls.Add(this.dateBeginHMS1);
            this.groupBox11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // label89
            // 
            resources.ApplyResources(this.label89, "label89");
            this.label89.Name = "label89";
            // 
            // label90
            // 
            resources.ApplyResources(this.label90, "label90");
            this.label90.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label90.Name = "label90";
            // 
            // dateBeginHMS3
            // 
            resources.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
            this.dateBeginHMS3.Name = "dateBeginHMS3";
            this.dateBeginHMS3.ShowUpDown = true;
            this.dateBeginHMS3.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateEndHMS3
            // 
            resources.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
            this.dateEndHMS3.Name = "dateEndHMS3";
            this.dateEndHMS3.ShowUpDown = true;
            this.dateEndHMS3.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // label87
            // 
            resources.ApplyResources(this.label87, "label87");
            this.label87.Name = "label87";
            // 
            // label88
            // 
            resources.ApplyResources(this.label88, "label88");
            this.label88.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label88.Name = "label88";
            // 
            // dateBeginHMS2
            // 
            resources.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
            this.dateBeginHMS2.Name = "dateBeginHMS2";
            this.dateBeginHMS2.ShowUpDown = true;
            this.dateBeginHMS2.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateEndHMS2
            // 
            resources.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
            this.dateEndHMS2.Name = "dateEndHMS2";
            this.dateEndHMS2.ShowUpDown = true;
            this.dateEndHMS2.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // label86
            // 
            resources.ApplyResources(this.label86, "label86");
            this.label86.Name = "label86";
            // 
            // label85
            // 
            resources.ApplyResources(this.label85, "label85");
            this.label85.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label85.Name = "label85";
            // 
            // dateEndHMS1
            // 
            resources.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
            this.dateEndHMS1.Name = "dateEndHMS1";
            this.dateEndHMS1.ShowUpDown = true;
            this.dateEndHMS1.Value = new System.DateTime(2010, 1, 1, 23, 59, 0, 0);
            // 
            // dateBeginHMS1
            // 
            resources.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
            this.dateBeginHMS1.Name = "dateBeginHMS1";
            this.dateBeginHMS1.ShowUpDown = true;
            this.dateBeginHMS1.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.Transparent;
            this.groupBox10.Controls.Add(this.chkMonday);
            this.groupBox10.Controls.Add(this.chkSunday);
            this.groupBox10.Controls.Add(this.chkTuesday);
            this.groupBox10.Controls.Add(this.chkSaturday);
            this.groupBox10.Controls.Add(this.chkWednesday);
            this.groupBox10.Controls.Add(this.chkFriday);
            this.groupBox10.Controls.Add(this.chkThursday);
            this.groupBox10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // chkMonday
            // 
            resources.ApplyResources(this.chkMonday, "chkMonday");
            this.chkMonday.Checked = true;
            this.chkMonday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMonday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkMonday.Name = "chkMonday";
            this.chkMonday.UseVisualStyleBackColor = true;
            // 
            // chkSunday
            // 
            resources.ApplyResources(this.chkSunday, "chkSunday");
            this.chkSunday.Checked = true;
            this.chkSunday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSunday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.UseVisualStyleBackColor = true;
            // 
            // chkTuesday
            // 
            resources.ApplyResources(this.chkTuesday, "chkTuesday");
            this.chkTuesday.Checked = true;
            this.chkTuesday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTuesday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkTuesday.Name = "chkTuesday";
            this.chkTuesday.UseVisualStyleBackColor = true;
            // 
            // chkSaturday
            // 
            resources.ApplyResources(this.chkSaturday, "chkSaturday");
            this.chkSaturday.Checked = true;
            this.chkSaturday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaturday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.UseVisualStyleBackColor = true;
            // 
            // chkWednesday
            // 
            resources.ApplyResources(this.chkWednesday, "chkWednesday");
            this.chkWednesday.Checked = true;
            this.chkWednesday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWednesday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkWednesday.Name = "chkWednesday";
            this.chkWednesday.UseVisualStyleBackColor = true;
            // 
            // chkFriday
            // 
            resources.ApplyResources(this.chkFriday, "chkFriday");
            this.chkFriday.Checked = true;
            this.chkFriday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFriday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkFriday.Name = "chkFriday";
            this.chkFriday.UseVisualStyleBackColor = true;
            // 
            // chkThursday
            // 
            resources.ApplyResources(this.chkThursday, "chkThursday");
            this.chkThursday.Checked = true;
            this.chkThursday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThursday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkThursday.Name = "chkThursday";
            this.chkThursday.UseVisualStyleBackColor = true;
            // 
            // label83
            // 
            resources.ApplyResources(this.label83, "label83");
            this.label83.BackColor = System.Drawing.Color.Transparent;
            this.label83.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label83.Name = "label83";
            // 
            // cbof_ControlSegID
            // 
            this.cbof_ControlSegID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ControlSegID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
            this.cbof_ControlSegID.Name = "cbof_ControlSegID";
            // 
            // dfrmControlSeg
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkNotAllowInHolidays);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtf_ControlSegName);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chkf_ReaderCount);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.label83);
            this.Controls.Add(this.cbof_ControlSegID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmControlSeg";
            this.Load += new System.EventHandler(this.dfrmControlSeg_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControlSeg_KeyDown);
            this.panelBottomBanner.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfHMS3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfHMS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudf_LimitedTimesOfHMS1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

