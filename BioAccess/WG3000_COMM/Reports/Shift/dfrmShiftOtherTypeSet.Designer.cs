namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmShiftOtherTypeSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmShiftOtherTypeSet));
            this.cmdCancel = new System.Windows.Forms.ImageButton();
            this.cmdOK = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.chkBOvertimeShift1 = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS1 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS1 = new System.Windows.Forms.DateTimePicker();
            this.Label7 = new System.Windows.Forms.Label();
            this.chkBOvertimeShift = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbof_ShiftID = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.cbof_Readtimes = new System.Windows.Forms.ComboBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBOvertimeShift2 = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS2 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkBOvertimeShift3 = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS3 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS3 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkBOvertimeShift4 = new System.Windows.Forms.CheckBox();
            this.dateBeginHMS4 = new System.Windows.Forms.DateTimePicker();
            this.dateEndHMS4 = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
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
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.Label6);
            this.groupBox1.Controls.Add(this.chkBOvertimeShift1);
            this.groupBox1.Controls.Add(this.dateBeginHMS1);
            this.groupBox1.Controls.Add(this.dateEndHMS1);
            this.groupBox1.Controls.Add(this.Label7);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // Label6
            // 
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Name = "Label6";
            // 
            // chkBOvertimeShift1
            // 
            resources.ApplyResources(this.chkBOvertimeShift1, "chkBOvertimeShift1");
            this.chkBOvertimeShift1.Name = "chkBOvertimeShift1";
            // 
            // dateBeginHMS1
            // 
            resources.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
            this.dateBeginHMS1.Name = "dateBeginHMS1";
            this.dateBeginHMS1.ShowUpDown = true;
            this.dateBeginHMS1.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // dateEndHMS1
            // 
            resources.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
            this.dateEndHMS1.Name = "dateEndHMS1";
            this.dateEndHMS1.ShowUpDown = true;
            this.dateEndHMS1.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // Label7
            // 
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.Name = "Label7";
            // 
            // chkBOvertimeShift
            // 
            this.chkBOvertimeShift.BackColor = System.Drawing.Color.Transparent;
            this.chkBOvertimeShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chkBOvertimeShift, "chkBOvertimeShift");
            this.chkBOvertimeShift.Name = "chkBOvertimeShift";
            this.chkBOvertimeShift.UseVisualStyleBackColor = false;
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            // 
            // cbof_ShiftID
            // 
            this.cbof_ShiftID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_ShiftID, "cbof_ShiftID");
            this.cbof_ShiftID.Name = "cbof_ShiftID";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // Label8
            // 
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label8, "Label8");
            this.Label8.Name = "Label8";
            // 
            // cbof_Readtimes
            // 
            this.cbof_Readtimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_Readtimes, "cbof_Readtimes");
            this.cbof_Readtimes.Name = "cbof_Readtimes";
            this.cbof_Readtimes.SelectedIndexChanged += new System.EventHandler(this.cbof_Readtimes_SelectedIndexChanged);
            // 
            // Label11
            // 
            this.Label11.BackColor = System.Drawing.Color.Transparent;
            this.Label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label11, "Label11");
            this.Label11.Name = "Label11";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chkBOvertimeShift2);
            this.groupBox2.Controls.Add(this.dateBeginHMS2);
            this.groupBox2.Controls.Add(this.dateEndHMS2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkBOvertimeShift2
            // 
            resources.ApplyResources(this.chkBOvertimeShift2, "chkBOvertimeShift2");
            this.chkBOvertimeShift2.Name = "chkBOvertimeShift2";
            // 
            // dateBeginHMS2
            // 
            resources.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
            this.dateBeginHMS2.Name = "dateBeginHMS2";
            this.dateBeginHMS2.ShowUpDown = true;
            this.dateBeginHMS2.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // dateEndHMS2
            // 
            resources.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
            this.dateEndHMS2.Name = "dateEndHMS2";
            this.dateEndHMS2.ShowUpDown = true;
            this.dateEndHMS2.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.chkBOvertimeShift3);
            this.groupBox3.Controls.Add(this.dateBeginHMS3);
            this.groupBox3.Controls.Add(this.dateEndHMS3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkBOvertimeShift3
            // 
            resources.ApplyResources(this.chkBOvertimeShift3, "chkBOvertimeShift3");
            this.chkBOvertimeShift3.Name = "chkBOvertimeShift3";
            // 
            // dateBeginHMS3
            // 
            resources.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
            this.dateBeginHMS3.Name = "dateBeginHMS3";
            this.dateBeginHMS3.ShowUpDown = true;
            this.dateBeginHMS3.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // dateEndHMS3
            // 
            resources.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
            this.dateEndHMS3.Name = "dateEndHMS3";
            this.dateEndHMS3.ShowUpDown = true;
            this.dateEndHMS3.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.chkBOvertimeShift4);
            this.groupBox4.Controls.Add(this.dateBeginHMS4);
            this.groupBox4.Controls.Add(this.dateEndHMS4);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // chkBOvertimeShift4
            // 
            resources.ApplyResources(this.chkBOvertimeShift4, "chkBOvertimeShift4");
            this.chkBOvertimeShift4.Name = "chkBOvertimeShift4";
            // 
            // dateBeginHMS4
            // 
            resources.ApplyResources(this.dateBeginHMS4, "dateBeginHMS4");
            this.dateBeginHMS4.Name = "dateBeginHMS4";
            this.dateBeginHMS4.ShowUpDown = true;
            this.dateBeginHMS4.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // dateEndHMS4
            // 
            resources.ApplyResources(this.dateEndHMS4, "dateEndHMS4");
            this.dateEndHMS4.Name = "dateEndHMS4";
            this.dateEndHMS4.ShowUpDown = true;
            this.dateEndHMS4.Value = new System.DateTime(2010, 2, 28, 0, 0, 0, 0);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdCancel);
            this.panelBottomBanner.Controls.Add(this.cmdOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmShiftOtherTypeSet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkBOvertimeShift);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cbof_ShiftID);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.cbof_Readtimes);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmShiftOtherTypeSet";
            this.Load += new System.EventHandler(this.dfrmShiftOtherTypeSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ComboBox cbof_Readtimes;
        internal ComboBox cbof_ShiftID;
        internal CheckBox chkBOvertimeShift;
        internal CheckBox chkBOvertimeShift1;
        internal CheckBox chkBOvertimeShift2;
        internal CheckBox chkBOvertimeShift3;
        internal CheckBox chkBOvertimeShift4;
        internal ImageButton cmdCancel;
        internal ImageButton cmdOK;
        internal DateTimePicker dateBeginHMS1;
        internal DateTimePicker dateBeginHMS2;
        internal DateTimePicker dateBeginHMS3;
        internal DateTimePicker dateBeginHMS4;
        internal DateTimePicker dateEndHMS1;
        internal DateTimePicker dateEndHMS2;
        internal DateTimePicker dateEndHMS3;
        internal DateTimePicker dateEndHMS4;
        internal GroupBox groupBox1;
        internal GroupBox groupBox2;
        internal GroupBox groupBox3;
        internal GroupBox groupBox4;
        internal Label Label1;
        internal Label label10;
        internal Label Label11;
        internal Label label2;
        internal Label label3;
        internal Label label4;
        internal Label label5;
        internal Label Label6;
        internal Label Label7;
        internal Label Label8;
        internal Label label9;
        internal TextBox txtName;
        private Panel panelBottomBanner;
    }
}

