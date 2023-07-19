namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmUserBatchUpdate
    {
        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        internal ComboBox cbof_GroupID;
        internal ComboBox cbof_GroupNew;
        internal CheckBox chk1;
        internal CheckBox chk2;
        internal CheckBox chk3;
        internal CheckBox chk4;
        internal CheckBox chk5;
        internal CheckBox chk6;
        internal CheckBox chkIncludeAllBranch;
        private IContainer components = null;
        internal DateTimePicker dtpBegin;
        internal DateTimePicker dtpEnd;
        internal GroupBox GroupBox1;
        internal GroupBox GroupBox2;
        internal GroupBox GroupBox3;
        internal GroupBox GroupBox4;
        internal Label Label1;
        internal Label Label3;
        internal Label Label5;
        internal RadioButton opt1a;
        internal RadioButton opt1b;
        internal RadioButton opt2a;
        internal RadioButton opt2b;
        internal RadioButton opt3a;
        internal RadioButton opt3b;
        private MaskedTextBox txtf_PIN;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmUserBatchUpdate));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.opt1a = new System.Windows.Forms.RadioButton();
            this.opt1b = new System.Windows.Forms.RadioButton();
            this.chk1 = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.cbof_GroupID = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.chk2 = new System.Windows.Forms.CheckBox();
            this.chk3 = new System.Windows.Forms.CheckBox();
            this.chk4 = new System.Windows.Forms.CheckBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.opt2a = new System.Windows.Forms.RadioButton();
            this.opt2b = new System.Windows.Forms.RadioButton();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.opt3a = new System.Windows.Forms.RadioButton();
            this.opt3b = new System.Windows.Forms.RadioButton();
            this.cbof_GroupNew = new System.Windows.Forms.ComboBox();
            this.chk5 = new System.Windows.Forms.CheckBox();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtf_PIN = new System.Windows.Forms.MaskedTextBox();
            this.chk6 = new System.Windows.Forms.CheckBox();
            this.chkIncludeAllBranch = new System.Windows.Forms.CheckBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.opt1a);
            this.GroupBox1.Controls.Add(this.opt1b);
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // opt1a
            // 
            this.opt1a.Checked = true;
            resources.ApplyResources(this.opt1a, "opt1a");
            this.opt1a.Name = "opt1a";
            this.opt1a.TabStop = true;
            // 
            // opt1b
            // 
            resources.ApplyResources(this.opt1b, "opt1b");
            this.opt1b.Name = "opt1b";
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
            // cbof_GroupID
            // 
            this.cbof_GroupID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
            this.cbof_GroupID.Name = "cbof_GroupID";
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.Name = "Label3";
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
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.Controls.Add(this.opt2a);
            this.GroupBox2.Controls.Add(this.opt2b);
            resources.ApplyResources(this.GroupBox2, "GroupBox2");
            this.GroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.TabStop = false;
            // 
            // opt2a
            // 
            this.opt2a.Checked = true;
            resources.ApplyResources(this.opt2a, "opt2a");
            this.opt2a.Name = "opt2a";
            this.opt2a.TabStop = true;
            // 
            // opt2b
            // 
            resources.ApplyResources(this.opt2b, "opt2b");
            this.opt2b.Name = "opt2b";
            // 
            // GroupBox3
            // 
            this.GroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox3.Controls.Add(this.opt3a);
            this.GroupBox3.Controls.Add(this.opt3b);
            resources.ApplyResources(this.GroupBox3, "GroupBox3");
            this.GroupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.TabStop = false;
            // 
            // opt3a
            // 
            this.opt3a.Checked = true;
            resources.ApplyResources(this.opt3a, "opt3a");
            this.opt3a.Name = "opt3a";
            this.opt3a.TabStop = true;
            // 
            // opt3b
            // 
            resources.ApplyResources(this.opt3b, "opt3b");
            this.opt3b.Name = "opt3b";
            // 
            // cbof_GroupNew
            // 
            this.cbof_GroupNew.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_GroupNew, "cbof_GroupNew");
            this.cbof_GroupNew.Name = "cbof_GroupNew";
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
            // dtpEnd
            // 
            resources.ApplyResources(this.dtpEnd, "dtpEnd");
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Value = new System.DateTime(2029, 12, 31, 0, 0, 0, 0);
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // GroupBox4
            // 
            this.GroupBox4.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox4.Controls.Add(this.dtpBegin);
            this.GroupBox4.Controls.Add(this.Label5);
            this.GroupBox4.Controls.Add(this.dtpEnd);
            this.GroupBox4.Controls.Add(this.Label1);
            resources.ApplyResources(this.GroupBox4, "GroupBox4");
            this.GroupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.TabStop = false;
            // 
            // dtpBegin
            // 
            resources.ApplyResources(this.dtpBegin, "dtpBegin");
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // txtf_PIN
            // 
            resources.ApplyResources(this.txtf_PIN, "txtf_PIN");
            this.txtf_PIN.Name = "txtf_PIN";
            // 
            // chk6
            // 
            this.chk6.BackColor = System.Drawing.Color.Transparent;
            this.chk6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chk6, "chk6");
            this.chk6.Name = "chk6";
            this.chk6.UseVisualStyleBackColor = false;
            // 
            // chkIncludeAllBranch
            // 
            resources.ApplyResources(this.chkIncludeAllBranch, "chkIncludeAllBranch");
            this.chkIncludeAllBranch.BackColor = System.Drawing.Color.Transparent;
            this.chkIncludeAllBranch.Checked = true;
            this.chkIncludeAllBranch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeAllBranch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkIncludeAllBranch.Name = "chkIncludeAllBranch";
            this.chkIncludeAllBranch.UseVisualStyleBackColor = false;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnOK);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmUserBatchUpdate
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkIncludeAllBranch);
            this.Controls.Add(this.chk6);
            this.Controls.Add(this.txtf_PIN);
            this.Controls.Add(this.cbof_GroupID);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.chk1);
            this.Controls.Add(this.chk3);
            this.Controls.Add(this.chk4);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.cbof_GroupNew);
            this.Controls.Add(this.chk5);
            this.Controls.Add(this.chk2);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmUserBatchUpdate";
            this.Load += new System.EventHandler(this.dfrmUserBatchUpdate_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmUserBatchUpdate_KeyDown);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

