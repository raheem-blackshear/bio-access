namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmRegister : frmBioAccess
    {
        public dfrmRegister()
        {
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.txtRegisterCode.Text.Trim();
                if (string.IsNullOrEmpty(str))
                {
                    XMessageBox.Show(CommonStr.strInputRegisterSN, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (string.IsNullOrEmpty(this.txtCompanyName.Text.Trim()))
                {
                    XMessageBox.Show(CommonStr.strInputCompanyName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (string.IsNullOrEmpty(this.txtBuildingCompanyName.Text.Trim()))
                {
                    XMessageBox.Show(CommonStr.strInputBuildingCompanyName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string str3;
                    string str4;
                    string str5;
                    if (str == "2004")
                    {
                        wgAppConfig.getSystemParamValue(12, out str3, out str4, out str5);
                        wgAppConfig.setSystemParamValue(12, str3, "200405", str5);
                        wgAppConfig.setSystemParamValue(0x24, "", this.txtCompanyName.Text, this.txtBuildingCompanyName.Text);
                        if (wgAppConfig.GetKeyVal("rgtries") != "")
                        {
                            wgAppConfig.UpdateKeyVal("rgtries", 1.ToString());
                        }
                        this.sendRegisterInfo();
                        XMessageBox.Show(CommonStr.strRegisterSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                    else
                    {
                        string s = "";
                        if ((str.Length >= 6) && (str.Substring(0, 4) == "2006"))
                        {
                            s = str.Substring(4, 2);
                        }
                        int result = 0;
                        int.TryParse(s, out result);
                        if (result >= 1)
                        {
                            result *= 30;
                            wgAppConfig.getSystemParamValue(12, out str3, out str4, out str5);
                            str4 = (result + 1).ToString();
                            str3 = DateTime.Now.ToString("yyyy-MM-dd");
                            wgAppConfig.setSystemParamValue(12, str3, str4, str5);
                            wgAppConfig.setSystemParamValue(0x24, "", this.txtCompanyName.Text, this.txtBuildingCompanyName.Text);
                            if (wgAppConfig.GetKeyVal("rgtries") != "")
                            {
                                wgAppConfig.UpdateKeyVal("rgtries", 1.ToString());
                            }
                            this.sendRegisterInfo();
                            XMessageBox.Show(CommonStr.strRegisterSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            base.DialogResult = DialogResult.OK;
                            base.Close();
                        }
                        else
                        {
                            XMessageBox.Show(CommonStr.strRegisterSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void sendRegisterInfo()
        {
            new Thread(new ThreadStart(wgMail.sendMailOnce)).Start();
        }
    }
}

