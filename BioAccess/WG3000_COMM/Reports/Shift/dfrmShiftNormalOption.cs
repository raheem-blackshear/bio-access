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

    public partial class dfrmShiftNormalOption : frmBioAccess
    {
        public dfrmShiftNormalOption()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                wgAppConfig.setSystemParamValue(0x37, "", this.dtpOffduty0.Value.ToString("HH:mm"), "");
                wgAppConfig.setSystemParamValueBool(0x38, this.chkEarliest.Checked);
                wgAppConfig.setSystemParamValueBool(0x39, this.chkOnlyTwoTimes.Checked);
                wgAppConfig.setSystemParamValue(0x3a, "", this.cboLeaveAbsenceTimeout.Text, "");
                wgAppConfig.setSystemParamValueBool(0x36, this.chkInvalidSwipe.Checked);
                wgAppConfig.setSystemParamValueBool(0x3b, this.chkOnlyOnDuty.Checked);
            }
            catch
            {
            }
            base.Close();
        }

        private void chkOnlyOnDuty_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkOnlyOnDuty.Checked)
            {
                this.chkOnlyTwoTimes.Checked = false;
            }
        }

        private void chkOnlyTwoTimes_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkOnlyTwoTimes.Checked)
            {
                this.chkOnlyOnDuty.Checked = false;
            }
        }

        private void dfrmShiftNormalOption_Load(object sender, EventArgs e)
        {
            this.dtpOffduty0.CustomFormat = "HH:mm";
            this.dtpOffduty0.Format = DateTimePickerFormat.Custom;
            this.dtpOffduty0.Value = DateTime.Parse("00:00:00");
            this.cboLeaveAbsenceTimeout.Items.Clear();
            double num = 0.0;
            for (int i = 0; i < 0x30; i++)
            {
                this.cboLeaveAbsenceTimeout.Items.Add(num.ToString("F1", CultureInfo.InvariantCulture));
                num += 0.5;
            }
            this.cboLeaveAbsenceTimeout.Text = "8.0";
            try
            {
                this.dtpOffduty0.Value = DateTime.Parse("2011-1-1 " + wgAppConfig.getSystemParamByNO(0x37).ToString());
            }
            catch
            {
            }
            try
            {
                this.chkEarliest.Checked = wgAppConfig.getSystemParamByNO(0x38).ToString() == "1";
            }
            catch
            {
            }
            try
            {
                this.chkOnlyTwoTimes.Checked = wgAppConfig.getSystemParamByNO(0x39).ToString() == "1";
            }
            catch
            {
            }
            try
            {
                this.cboLeaveAbsenceTimeout.Text = wgAppConfig.getSystemParamByNO(0x3a).ToString();
            }
            catch
            {
            }
            try
            {
                this.chkInvalidSwipe.Checked = wgAppConfig.getSystemParamByNO(0x36).ToString() == "1";
            }
            catch
            {
            }
            try
            {
                this.chkOnlyOnDuty.Checked = wgAppConfig.getSystemParamByNO(0x3b).ToString() == "1";
            }
            catch
            {
            }
            this.loadOperatorPrivilege();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuShiftNormalConfigure";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnOK.Visible = false;
            }
        }
    }
}

