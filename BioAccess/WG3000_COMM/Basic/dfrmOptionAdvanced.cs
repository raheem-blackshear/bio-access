namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmOptionAdvanced : frmBioAccess
    {
        public dfrmOptionAdvanced()
        {
            this.InitializeComponent();
            this.cmdCancel.DialogResult = DialogResult.Cancel;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPhotoDirectory.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void chkValidSwipeGap_CheckedChanged(object sender, EventArgs e)
        {
            this.nudValidSwipeGap.Enabled = this.chkValidSwipeGap.Checked;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPhotoDirectory.Text.Trim()) && (this.txtPhotoDirectory.Text.Trim().Length != Encoding.GetEncoding("utf-8").GetBytes(this.txtPhotoDirectory.Text.Trim()).Length))
            {
                XMessageBox.Show(this, CommonStr.strInvalidPhotoDirectory, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                wgAppConfig.UpdateKeyVal("AllowUploadUserName", this.chkAllowUploadUserName.Checked ? "1" : "0");
                wgAppConfig.setSystemParamValue(0x29, this.txtPhotoDirectory.Text.Trim());
                if (this.chkValidSwipeGap.Visible)
                {
                    int num = 0;
                    if (this.chkValidSwipeGap.Checked)
                    {
                        num = (int) this.nudValidSwipeGap.Value;
                    }
                    if ((num & 1) > 0)
                    {
                        num++;
                    }
                    wgAppConfig.setSystemParamValue(0x93, num.ToString());
                }
                base.Close();
            }
        }

        private void dfrmOptionAdvanced_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.funcCtrlShiftQ();
                }
            }
        }

        private void dfrmOptionAdvanced_Load(object sender, EventArgs e)
        {
            if (icOperator.OperatorID != 1)
            {
                XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.chkAllowUploadUserName.Checked = wgAppConfig.GetKeyVal("AllowUploadUserName") == "1";
                this.txtPhotoDirectory.Text = wgAppConfig.getSystemParamByNO(0x29);
                this.chkValidSwipeGap.Visible = wgAppConfig.getParamValBoolByNO(0x93);
                this.chkValidSwipeGap.Checked = wgAppConfig.getParamValBoolByNO(0x93);
                this.nudValidSwipeGap.Visible = wgAppConfig.getParamValBoolByNO(0x93);
                this.nudValidSwipeGap.Enabled = false;
                if (this.chkValidSwipeGap.Checked)
                {
                    this.nudValidSwipeGap.Value = int.Parse(wgAppConfig.getSystemParamByNO(0x93));
                    this.nudValidSwipeGap.Enabled = true;
                }
            }
        }

        private void funcCtrlShiftQ()
        {
            this.txtPhotoDirectory.ReadOnly = false;
            this.chkValidSwipeGap.Visible = true;
            this.nudValidSwipeGap.Visible = true;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = cmdCancel.DialogResult;
            Close();
        }
    }
}

