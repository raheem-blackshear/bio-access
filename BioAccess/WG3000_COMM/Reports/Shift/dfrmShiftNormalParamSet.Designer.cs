namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftNormalParamSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmShiftNormalParamSet));
            this.btnOption = new System.Windows.Forms.ImageButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.cboLeaveAbsenceDay = new System.Windows.Forms.ComboBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.nudOvertimeTimeout = new System.Windows.Forms.NumericUpDown();
            this.cboLateAbsenceDay = new System.Windows.Forms.ComboBox();
            this.nudLeaveAbsenceTimeout = new System.Windows.Forms.NumericUpDown();
            this.nudLeaveTimeout = new System.Windows.Forms.NumericUpDown();
            this.nudLateAbsenceTimeout = new System.Windows.Forms.NumericUpDown();
            this.nudLateTimeout = new System.Windows.Forms.NumericUpDown();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.dtpOffduty0 = new System.Windows.Forms.DateTimePicker();
            this.dtpOnduty0 = new System.Windows.Forms.DateTimePicker();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.grpbTwoTimes = new System.Windows.Forms.GroupBox();
            this.optReadCardTwoTimes = new System.Windows.Forms.RadioButton();
            this.optReadCardFourTimes = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.dtpOnduty2 = new System.Windows.Forms.DateTimePicker();
            this.Label11 = new System.Windows.Forms.Label();
            this.grpbFourtimes = new System.Windows.Forms.GroupBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.dtpOnduty1 = new System.Windows.Forms.DateTimePicker();
            this.dtpOffduty1 = new System.Windows.Forms.DateTimePicker();
            this.Label9 = new System.Windows.Forms.Label();
            this.dtpOffduty2 = new System.Windows.Forms.DateTimePicker();
            this.Label10 = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOvertimeTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeaveAbsenceTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeaveTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLateAbsenceTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLateTimeout)).BeginInit();
            this.grpbTwoTimes.SuspendLayout();
            this.grpbFourtimes.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOption
            // 
            resources.ApplyResources(this.btnOption, "btnOption");
            this.btnOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOption.Focusable = true;
            this.btnOption.ForeColor = System.Drawing.Color.White;
            this.btnOption.Name = "btnOption";
            this.btnOption.Toggle = false;
            this.btnOption.UseVisualStyleBackColor = false;
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.cboLeaveAbsenceDay);
            this.GroupBox1.Controls.Add(this.Label16);
            this.GroupBox1.Controls.Add(this.Label15);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.Label12);
            this.GroupBox1.Controls.Add(this.nudOvertimeTimeout);
            this.GroupBox1.Controls.Add(this.cboLateAbsenceDay);
            this.GroupBox1.Controls.Add(this.nudLeaveAbsenceTimeout);
            this.GroupBox1.Controls.Add(this.nudLeaveTimeout);
            this.GroupBox1.Controls.Add(this.nudLateAbsenceTimeout);
            this.GroupBox1.Controls.Add(this.nudLateTimeout);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label14);
            this.GroupBox1.Controls.Add(this.Label17);
            this.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // cboLeaveAbsenceDay
            // 
            this.cboLeaveAbsenceDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboLeaveAbsenceDay, "cboLeaveAbsenceDay");
            this.cboLeaveAbsenceDay.Items.AddRange(new object[] {
            resources.GetString("cboLeaveAbsenceDay.Items"),
            resources.GetString("cboLeaveAbsenceDay.Items1"),
            resources.GetString("cboLeaveAbsenceDay.Items2")});
            this.cboLeaveAbsenceDay.Name = "cboLeaveAbsenceDay";
            // 
            // Label16
            // 
            resources.ApplyResources(this.Label16, "Label16");
            this.Label16.Name = "Label16";
            // 
            // Label15
            // 
            resources.ApplyResources(this.Label15, "Label15");
            this.Label15.Name = "Label15";
            // 
            // Label13
            // 
            resources.ApplyResources(this.Label13, "Label13");
            this.Label13.Name = "Label13";
            // 
            // Label12
            // 
            resources.ApplyResources(this.Label12, "Label12");
            this.Label12.Name = "Label12";
            // 
            // nudOvertimeTimeout
            // 
            this.nudOvertimeTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudOvertimeTimeout, "nudOvertimeTimeout");
            this.nudOvertimeTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudOvertimeTimeout.Name = "nudOvertimeTimeout";
            this.nudOvertimeTimeout.ReadOnly = true;
            this.nudOvertimeTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // cboLateAbsenceDay
            // 
            this.cboLateAbsenceDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboLateAbsenceDay, "cboLateAbsenceDay");
            this.cboLateAbsenceDay.Items.AddRange(new object[] {
            resources.GetString("cboLateAbsenceDay.Items"),
            resources.GetString("cboLateAbsenceDay.Items1"),
            resources.GetString("cboLateAbsenceDay.Items2")});
            this.cboLateAbsenceDay.Name = "cboLateAbsenceDay";
            // 
            // nudLeaveAbsenceTimeout
            // 
            this.nudLeaveAbsenceTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudLeaveAbsenceTimeout, "nudLeaveAbsenceTimeout");
            this.nudLeaveAbsenceTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudLeaveAbsenceTimeout.Name = "nudLeaveAbsenceTimeout";
            this.nudLeaveAbsenceTimeout.ReadOnly = true;
            this.nudLeaveAbsenceTimeout.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // nudLeaveTimeout
            // 
            this.nudLeaveTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudLeaveTimeout, "nudLeaveTimeout");
            this.nudLeaveTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudLeaveTimeout.Name = "nudLeaveTimeout";
            this.nudLeaveTimeout.ReadOnly = true;
            this.nudLeaveTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudLateAbsenceTimeout
            // 
            this.nudLateAbsenceTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudLateAbsenceTimeout, "nudLateAbsenceTimeout");
            this.nudLateAbsenceTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudLateAbsenceTimeout.Name = "nudLateAbsenceTimeout";
            this.nudLateAbsenceTimeout.ReadOnly = true;
            this.nudLateAbsenceTimeout.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // nudLateTimeout
            // 
            this.nudLateTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudLateTimeout, "nudLateTimeout");
            this.nudLateTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudLateTimeout.Name = "nudLateTimeout";
            this.nudLateTimeout.ReadOnly = true;
            this.nudLateTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // Label2
            // 
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Name = "Label2";
            // 
            // Label3
            // 
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.Name = "Label3";
            // 
            // Label4
            // 
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.Name = "Label4";
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // Label14
            // 
            resources.ApplyResources(this.Label14, "Label14");
            this.Label14.Name = "Label14";
            // 
            // Label17
            // 
            resources.ApplyResources(this.Label17, "Label17");
            this.Label17.Name = "Label17";
            // 
            // dtpOffduty0
            // 
            resources.ApplyResources(this.dtpOffduty0, "dtpOffduty0");
            this.dtpOffduty0.Name = "dtpOffduty0";
            this.dtpOffduty0.ShowUpDown = true;
            this.dtpOffduty0.Value = new System.DateTime(2004, 7, 18, 17, 30, 0, 0);
            // 
            // dtpOnduty0
            // 
            resources.ApplyResources(this.dtpOnduty0, "dtpOnduty0");
            this.dtpOnduty0.Name = "dtpOnduty0";
            this.dtpOnduty0.ShowUpDown = true;
            this.dtpOnduty0.Value = new System.DateTime(2004, 7, 18, 8, 30, 0, 0);
            // 
            // Label7
            // 
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.Name = "Label7";
            // 
            // Label6
            // 
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Name = "Label6";
            // 
            // grpbTwoTimes
            // 
            this.grpbTwoTimes.Controls.Add(this.Label6);
            this.grpbTwoTimes.Controls.Add(this.dtpOnduty0);
            this.grpbTwoTimes.Controls.Add(this.dtpOffduty0);
            this.grpbTwoTimes.Controls.Add(this.Label7);
            resources.ApplyResources(this.grpbTwoTimes, "grpbTwoTimes");
            this.grpbTwoTimes.Name = "grpbTwoTimes";
            this.grpbTwoTimes.TabStop = false;
            // 
            // optReadCardTwoTimes
            // 
            this.optReadCardTwoTimes.Checked = true;
            resources.ApplyResources(this.optReadCardTwoTimes, "optReadCardTwoTimes");
            this.optReadCardTwoTimes.Name = "optReadCardTwoTimes";
            this.optReadCardTwoTimes.TabStop = true;
            this.optReadCardTwoTimes.CheckedChanged += new System.EventHandler(this.optReadCardTwoTimes_CheckedChanged);
            // 
            // optReadCardFourTimes
            // 
            resources.ApplyResources(this.optReadCardFourTimes, "optReadCardFourTimes");
            this.optReadCardFourTimes.Name = "optReadCardFourTimes";
            this.optReadCardFourTimes.CheckedChanged += new System.EventHandler(this.optReadCardFourTimes_CheckedChanged);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dtpOnduty2
            // 
            resources.ApplyResources(this.dtpOnduty2, "dtpOnduty2");
            this.dtpOnduty2.Name = "dtpOnduty2";
            this.dtpOnduty2.ShowUpDown = true;
            this.dtpOnduty2.Value = new System.DateTime(2004, 7, 18, 13, 30, 0, 0);
            // 
            // Label11
            // 
            resources.ApplyResources(this.Label11, "Label11");
            this.Label11.Name = "Label11";
            // 
            // grpbFourtimes
            // 
            this.grpbFourtimes.Controls.Add(this.Label8);
            this.grpbFourtimes.Controls.Add(this.dtpOnduty1);
            this.grpbFourtimes.Controls.Add(this.dtpOffduty1);
            this.grpbFourtimes.Controls.Add(this.Label9);
            this.grpbFourtimes.Controls.Add(this.dtpOffduty2);
            this.grpbFourtimes.Controls.Add(this.Label10);
            this.grpbFourtimes.Controls.Add(this.Label11);
            this.grpbFourtimes.Controls.Add(this.dtpOnduty2);
            resources.ApplyResources(this.grpbFourtimes, "grpbFourtimes");
            this.grpbFourtimes.Name = "grpbFourtimes";
            this.grpbFourtimes.TabStop = false;
            // 
            // Label8
            // 
            resources.ApplyResources(this.Label8, "Label8");
            this.Label8.Name = "Label8";
            // 
            // dtpOnduty1
            // 
            resources.ApplyResources(this.dtpOnduty1, "dtpOnduty1");
            this.dtpOnduty1.Name = "dtpOnduty1";
            this.dtpOnduty1.ShowUpDown = true;
            this.dtpOnduty1.Value = new System.DateTime(2004, 7, 18, 8, 30, 0, 0);
            // 
            // dtpOffduty1
            // 
            resources.ApplyResources(this.dtpOffduty1, "dtpOffduty1");
            this.dtpOffduty1.Name = "dtpOffduty1";
            this.dtpOffduty1.ShowUpDown = true;
            this.dtpOffduty1.Value = new System.DateTime(2004, 7, 18, 12, 0, 0, 0);
            // 
            // Label9
            // 
            resources.ApplyResources(this.Label9, "Label9");
            this.Label9.Name = "Label9";
            // 
            // dtpOffduty2
            // 
            resources.ApplyResources(this.dtpOffduty2, "dtpOffduty2");
            this.dtpOffduty2.Name = "dtpOffduty2";
            this.dtpOffduty2.ShowUpDown = true;
            this.dtpOffduty2.Value = new System.DateTime(2004, 7, 18, 17, 30, 0, 0);
            // 
            // Label10
            // 
            resources.ApplyResources(this.Label10, "Label10");
            this.Label10.Name = "Label10";
            // 
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.Controls.Add(this.grpbFourtimes);
            this.GroupBox2.Controls.Add(this.grpbTwoTimes);
            this.GroupBox2.Controls.Add(this.optReadCardTwoTimes);
            this.GroupBox2.Controls.Add(this.optReadCardFourTimes);
            this.GroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.GroupBox2, "GroupBox2");
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.TabStop = false;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnOK);
            this.panelBottomBanner.Controls.Add(this.btnOption);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmShiftNormalParamSet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.GroupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmShiftNormalParamSet";
            this.Load += new System.EventHandler(this.dfrmShiftNormalParamSet_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmShiftNormalParamSet_KeyDown);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOvertimeTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeaveAbsenceTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeaveTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLateAbsenceTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLateTimeout)).EndInit();
            this.grpbTwoTimes.ResumeLayout(false);
            this.grpbFourtimes.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        internal ImageButton btnOption;
        internal ComboBox cboLateAbsenceDay;
        internal ComboBox cboLeaveAbsenceDay;
        internal DateTimePicker dtpOffduty0;
        internal DateTimePicker dtpOffduty1;
        internal DateTimePicker dtpOffduty2;
        internal DateTimePicker dtpOnduty0;
        internal DateTimePicker dtpOnduty1;
        internal DateTimePicker dtpOnduty2;
        internal GroupBox GroupBox1;
        internal GroupBox GroupBox2;
        internal GroupBox grpbFourtimes;
        internal GroupBox grpbTwoTimes;
        internal Label Label1;
        internal Label Label10;
        internal Label Label11;
        internal Label Label12;
        internal Label Label13;
        internal Label Label14;
        internal Label Label15;
        internal Label Label16;
        internal Label Label17;
        internal Label Label2;
        internal Label Label3;
        internal Label Label4;
        internal Label Label5;
        internal Label Label6;
        internal Label Label7;
        internal Label Label8;
        internal Label Label9;
        internal NumericUpDown nudLateAbsenceTimeout;
        internal NumericUpDown nudLateTimeout;
        internal NumericUpDown nudLeaveAbsenceTimeout;
        internal NumericUpDown nudLeaveTimeout;
        internal NumericUpDown nudOvertimeTimeout;
        internal RadioButton optReadCardFourTimes;
        internal RadioButton optReadCardTwoTimes;
        private Panel panelBottomBanner;
    }
}

