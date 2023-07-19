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
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControllerTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerTask));
            this.cmdCancel = new System.Windows.Forms.ImageButton();
            this.cmdOK = new System.Windows.Forms.ImageButton();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboAccessMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDoors = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
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
            this.chk5 = new System.Windows.Forms.CheckBox();
            this.chk3 = new System.Windows.Forms.CheckBox();
            this.chk4 = new System.Windows.Forms.CheckBox();
            this.chk1 = new System.Windows.Forms.CheckBox();
            this.chk2 = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtTaskIDs = new System.Windows.Forms.TextBox();
            this.lblTaskID = new System.Windows.Forms.Label();
            this.chk6 = new System.Windows.Forms.CheckBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            // txtNote
            // 
            resources.ApplyResources(this.txtNote, "txtNote");
            this.txtNote.Name = "txtNote";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
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
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.checkBox49);
            this.groupBox3.Controls.Add(this.checkBox48);
            this.groupBox3.Controls.Add(this.checkBox47);
            this.groupBox3.Controls.Add(this.checkBox46);
            this.groupBox3.Controls.Add(this.checkBox45);
            this.groupBox3.Controls.Add(this.checkBox44);
            this.groupBox3.Controls.Add(this.checkBox43);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // checkBox49
            // 
            resources.ApplyResources(this.checkBox49, "checkBox49");
            this.checkBox49.Checked = true;
            this.checkBox49.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox49.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox49.Name = "checkBox49";
            this.checkBox49.UseVisualStyleBackColor = true;
            // 
            // checkBox48
            // 
            resources.ApplyResources(this.checkBox48, "checkBox48");
            this.checkBox48.Checked = true;
            this.checkBox48.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox48.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox48.Name = "checkBox48";
            this.checkBox48.UseVisualStyleBackColor = true;
            // 
            // checkBox47
            // 
            resources.ApplyResources(this.checkBox47, "checkBox47");
            this.checkBox47.Checked = true;
            this.checkBox47.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox47.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox47.Name = "checkBox47";
            this.checkBox47.UseVisualStyleBackColor = true;
            // 
            // checkBox46
            // 
            resources.ApplyResources(this.checkBox46, "checkBox46");
            this.checkBox46.Checked = true;
            this.checkBox46.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox46.Name = "checkBox46";
            this.checkBox46.UseVisualStyleBackColor = true;
            // 
            // checkBox45
            // 
            resources.ApplyResources(this.checkBox45, "checkBox45");
            this.checkBox45.Checked = true;
            this.checkBox45.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox45.Name = "checkBox45";
            this.checkBox45.UseVisualStyleBackColor = true;
            // 
            // checkBox44
            // 
            resources.ApplyResources(this.checkBox44, "checkBox44");
            this.checkBox44.Checked = true;
            this.checkBox44.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox44.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox44.Name = "checkBox44";
            this.checkBox44.UseVisualStyleBackColor = true;
            // 
            // checkBox43
            // 
            resources.ApplyResources(this.checkBox43, "checkBox43");
            this.checkBox43.Checked = true;
            this.checkBox43.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox43.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
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
            // chk5
            // 
            this.chk5.BackColor = System.Drawing.Color.Transparent;
            this.chk5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk5, "chk5");
            this.chk5.Name = "chk5";
            this.chk5.UseVisualStyleBackColor = false;
            this.chk5.CheckedChanged += new System.EventHandler(this.chk5_CheckedChanged);
            // 
            // chk3
            // 
            this.chk3.BackColor = System.Drawing.Color.Transparent;
            this.chk3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk3, "chk3");
            this.chk3.Name = "chk3";
            this.chk3.UseVisualStyleBackColor = false;
            this.chk3.CheckedChanged += new System.EventHandler(this.chk3_CheckedChanged);
            // 
            // chk4
            // 
            this.chk4.BackColor = System.Drawing.Color.Transparent;
            this.chk4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk4, "chk4");
            this.chk4.Name = "chk4";
            this.chk4.UseVisualStyleBackColor = false;
            this.chk4.CheckedChanged += new System.EventHandler(this.chk4_CheckedChanged);
            // 
            // chk1
            // 
            this.chk1.BackColor = System.Drawing.Color.Transparent;
            this.chk1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk1, "chk1");
            this.chk1.Name = "chk1";
            this.chk1.UseVisualStyleBackColor = false;
            this.chk1.CheckedChanged += new System.EventHandler(this.chk1_CheckedChanged);
            // 
            // chk2
            // 
            this.chk2.BackColor = System.Drawing.Color.Transparent;
            this.chk2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk2, "chk2");
            this.chk2.Name = "chk2";
            this.chk2.UseVisualStyleBackColor = false;
            this.chk2.CheckedChanged += new System.EventHandler(this.chk2_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.txtNote);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dtpBegin);
            this.groupBox1.Controls.Add(this.dtpEnd);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dtpTime);
            this.groupBox2.Controls.Add(this.label45);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.cboAccessMethod);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.cboDoors);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // txtTaskIDs
            // 
            resources.ApplyResources(this.txtTaskIDs, "txtTaskIDs");
            this.txtTaskIDs.Name = "txtTaskIDs";
            this.txtTaskIDs.ReadOnly = true;
            // 
            // lblTaskID
            // 
            resources.ApplyResources(this.lblTaskID, "lblTaskID");
            this.lblTaskID.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblTaskID.Name = "lblTaskID";
            // 
            // chk6
            // 
            this.chk6.BackColor = System.Drawing.Color.Transparent;
            this.chk6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk6, "chk6");
            this.chk6.Name = "chk6";
            this.chk6.UseVisualStyleBackColor = false;
            this.chk6.CheckedChanged += new System.EventHandler(this.chk6_CheckedChanged);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdCancel);
            this.panelBottomBanner.Controls.Add(this.cmdOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmControllerTask
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.txtTaskIDs);
            this.Controls.Add(this.lblTaskID);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.chk6);
            this.Controls.Add(this.chk5);
            this.Controls.Add(this.chk3);
            this.Controls.Add(this.chk1);
            this.Controls.Add(this.chk2);
            this.Controls.Add(this.chk4);
            this.MinimizeBox = false;
            this.Name = "dfrmControllerTask";
            this.Load += new System.EventHandler(this.dfrmControllerTask_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cboAccessMethod;
        private ComboBox cboDoors;
        private CheckBox checkBox43;
        private CheckBox checkBox44;
        private CheckBox checkBox45;
        private CheckBox checkBox46;
        private CheckBox checkBox47;
        private CheckBox checkBox48;
        private CheckBox checkBox49;
        internal CheckBox chk1;
        internal CheckBox chk2;
        internal CheckBox chk3;
        internal CheckBox chk4;
        internal CheckBox chk5;
        internal CheckBox chk6;
        internal ImageButton cmdCancel;
        internal ImageButton cmdOK;
        private DataTable dtDoors;
        private DateTimePicker dtpBegin;
        private DateTimePicker dtpEnd;
        private DateTimePicker dtpTime;
        internal GroupBox groupBox1;
        internal GroupBox groupBox2;
        internal GroupBox groupBox3;
        internal GroupBox groupBox4;
        internal GroupBox groupBox5;
        internal GroupBox groupBox6;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label43;
        private Label label44;
        private Label label45;
        private Label lblTaskID;
        private TextBox txtNote;
        public TextBox txtTaskIDs;
        private Panel panelBottomBanner;
    }
}

