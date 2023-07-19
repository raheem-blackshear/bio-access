namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmHolidayAdd
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmHolidayAdd));
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.Label4 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.cboStart = new System.Windows.Forms.ComboBox();
            this.cboEnd = new System.Windows.Forms.ComboBox();
            this.txtf_Notes = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.txtHolidayName = new System.Windows.Forms.TextBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Label4
            // 
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.Name = "Label4";
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Value = new System.DateTime(2004, 7, 19, 0, 0, 0, 0);
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // Label5
            // 
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // Label6
            // 
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Name = "Label6";
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Value = new System.DateTime(2004, 7, 19, 0, 0, 0, 0);
            // 
            // cboStart
            // 
            this.cboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboStart, "cboStart");
            this.cboStart.Name = "cboStart";
            // 
            // cboEnd
            // 
            this.cboEnd.DisplayMember = "f_GroupName";
            this.cboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboEnd, "cboEnd");
            this.cboEnd.Name = "cboEnd";
            this.cboEnd.ValueMember = "f_GroupID";
            // 
            // txtf_Notes
            // 
            resources.ApplyResources(this.txtf_Notes, "txtf_Notes");
            this.txtf_Notes.Name = "txtf_Notes";
            // 
            // Label7
            // 
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.Name = "Label7";
            // 
            // txtHolidayName
            // 
            resources.ApplyResources(this.txtHolidayName, "txtHolidayName");
            this.txtHolidayName.Name = "txtHolidayName";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmHolidayAdd
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.txtHolidayName);
            this.Controls.Add(this.txtf_Notes);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.cboStart);
            this.Controls.Add(this.cboEnd);
            this.Controls.Add(this.Label7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmHolidayAdd";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.dfrmLeave_Load);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        internal ComboBox cboEnd;
        internal ComboBox cboStart;
        internal DateTimePicker dtpEndDate;
        internal DateTimePicker dtpStartDate;
        internal Label Label4;
        internal Label Label5;
        internal Label Label6;
        internal Label Label7;
        internal TextBox txtf_Notes;
        internal TextBox txtHolidayName;
        private Panel panelBottomBanner;
    }
}

