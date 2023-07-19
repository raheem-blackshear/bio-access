namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmShiftNormalOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmShiftNormalOption));
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.chkInvalidSwipe = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboLeaveAbsenceTimeout = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkOnlyTwoTimes = new System.Windows.Forms.CheckBox();
            this.chkOnlyOnDuty = new System.Windows.Forms.CheckBox();
            this.dtpOffduty0 = new System.Windows.Forms.DateTimePicker();
            this.Label7 = new System.Windows.Forms.Label();
            this.chkEarliest = new System.Windows.Forms.CheckBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkInvalidSwipe
            // 
            resources.ApplyResources(this.chkInvalidSwipe, "chkInvalidSwipe");
            this.chkInvalidSwipe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkInvalidSwipe.Name = "chkInvalidSwipe";
            this.chkInvalidSwipe.UseVisualStyleBackColor = true;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboLeaveAbsenceTimeout);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkOnlyTwoTimes);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cboLeaveAbsenceTimeout
            // 
            this.cboLeaveAbsenceTimeout.DisplayMember = "8.0";
            this.cboLeaveAbsenceTimeout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLeaveAbsenceTimeout.FormattingEnabled = true;
            resources.ApplyResources(this.cboLeaveAbsenceTimeout, "cboLeaveAbsenceTimeout");
            this.cboLeaveAbsenceTimeout.Name = "cboLeaveAbsenceTimeout";
            this.cboLeaveAbsenceTimeout.ValueMember = "8.0";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkOnlyTwoTimes
            // 
            resources.ApplyResources(this.chkOnlyTwoTimes, "chkOnlyTwoTimes");
            this.chkOnlyTwoTimes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkOnlyTwoTimes.Name = "chkOnlyTwoTimes";
            this.chkOnlyTwoTimes.UseVisualStyleBackColor = true;
            this.chkOnlyTwoTimes.CheckedChanged += new System.EventHandler(this.chkOnlyTwoTimes_CheckedChanged);
            // 
            // chkOnlyOnDuty
            // 
            resources.ApplyResources(this.chkOnlyOnDuty, "chkOnlyOnDuty");
            this.chkOnlyOnDuty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkOnlyOnDuty.Name = "chkOnlyOnDuty";
            this.chkOnlyOnDuty.UseVisualStyleBackColor = true;
            this.chkOnlyOnDuty.CheckedChanged += new System.EventHandler(this.chkOnlyOnDuty_CheckedChanged);
            // 
            // dtpOffduty0
            // 
            resources.ApplyResources(this.dtpOffduty0, "dtpOffduty0");
            this.dtpOffduty0.Name = "dtpOffduty0";
            this.dtpOffduty0.ShowUpDown = true;
            this.dtpOffduty0.Value = new System.DateTime(2004, 7, 18, 0, 0, 0, 0);
            // 
            // Label7
            // 
            this.Label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.Name = "Label7";
            // 
            // chkEarliest
            // 
            resources.ApplyResources(this.chkEarliest, "chkEarliest");
            this.chkEarliest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkEarliest.Name = "chkEarliest";
            this.chkEarliest.UseVisualStyleBackColor = true;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnOK);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmShiftNormalOption
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkOnlyOnDuty);
            this.Controls.Add(this.chkInvalidSwipe);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkEarliest);
            this.Controls.Add(this.dtpOffduty0);
            this.Controls.Add(this.Label7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmShiftNormalOption";
            this.Load += new System.EventHandler(this.dfrmShiftNormalOption_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        private ComboBox cboLeaveAbsenceTimeout;
        private CheckBox chkEarliest;
        private CheckBox chkInvalidSwipe;
        private CheckBox chkOnlyOnDuty;
        private CheckBox chkOnlyTwoTimes;
        internal DateTimePicker dtpOffduty0;
        private GroupBox groupBox1;
        internal Label label1;
        internal Label Label7;
        private Panel panelBottomBanner;
    }
}

