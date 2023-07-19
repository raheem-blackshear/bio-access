namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmTCPIPConfigure : frmBioAccess
    {
        public string strGateway = "";
        public string strIP = "";
        public string strMac = "";
        public string strMask = "";
        public string strSN = "";
        public string strTCPPort = "";

        public dfrmTCPIPConfigure()
        {
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.txtf_ControllerSN.ReadOnly)
            {
                int num;
                this.txtf_ControllerSN.Text = this.txtf_ControllerSN.Text.Trim();
                if (!int.TryParse(this.txtf_ControllerSN.Text, out num))
                {
                    XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (wgMjController.GetControllerType(int.Parse(this.txtf_ControllerSN.Text)) == 0)
                {
                    XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (string.IsNullOrEmpty(this.txtf_IP.Text))
            {
                XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", "");
                if (!this.isIPAddress(this.txtf_IP.Text))
                {
                    XMessageBox.Show(this, this.txtf_IP.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.txtf_mask.Text = this.txtf_mask.Text.Replace(" ", "");
                    if (!this.isIPAddress(this.txtf_mask.Text))
                    {
                        XMessageBox.Show(this, this.txtf_mask.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        this.txtf_gateway.Text = this.txtf_gateway.Text.Replace(" ", "");
                        if (!string.IsNullOrEmpty(this.txtf_gateway.Text) && !this.isIPAddress(this.txtf_gateway.Text))
                        {
                            XMessageBox.Show(this, this.txtf_gateway.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            this.strSN = this.txtf_ControllerSN.Text;
                            this.strMac = this.txtf_MACAddr.Text;
                            this.strIP = this.txtf_IP.Text;
                            this.strMask = this.txtf_mask.Text;
                            this.strGateway = this.txtf_gateway.Text;
                            this.strTCPPort = this.nudPort.Value.ToString();
                            base.DialogResult = DialogResult.OK;
                            base.Close();
                        }
                    }
                }
            }
        }

        private void dfrmTCPIPConfigure_Load(object sender, EventArgs e)
        {
            this.txtf_ControllerSN.Text = this.strSN;
            this.txtf_MACAddr.Text = this.strMac;
            this.txtf_IP.Text = this.strIP;
            this.txtf_mask.Text = this.strMask;
            this.txtf_gateway.Text = this.strGateway;
            if ((int.Parse(this.strTCPPort) < this.nudPort.Minimum) || (int.Parse(this.strTCPPort) >= 0xffff))
            {
                this.strTCPPort = 0xea60.ToString();
            }
            this.nudPort.Value = int.Parse(this.strTCPPort);
            if (this.txtf_IP.Text == "255.255.255.255")
            {
                this.txtf_IP.Text = "192.168.0.0";
            }
            if (this.txtf_mask.Text == "255.255.255.255")
            {
                this.txtf_mask.Text = "255.255.255.0";
            }
            if (this.txtf_gateway.Text == "255.255.255.255")
            {
                this.txtf_gateway.Text = "";
            }
            if (this.txtf_gateway.Text == "0.0.0.0")
            {
                this.txtf_gateway.Text = "";
            }
        }

        public bool isIPAddress(string ipstr)
        {
            bool flag = false;
            try
            {
                if (string.IsNullOrEmpty(ipstr))
                {
                    return flag;
                }
                string[] strArray = ipstr.Split(new char[] { '.' });
                if (strArray.Length != 4)
                {
                    return flag;
                }
                flag = true;
                for (int i = 0; i <= 3; i++)
                {
                    int num;
                    if (!int.TryParse(strArray[i], out num))
                    {
                        flag = false;
                        break;
                    }
                    if ((num < 0) || (num > 0xff))
                    {
                        flag = false;
                        break;
                    }
                }
                if (int.Parse(strArray[0]) == 0)
                {
                    return false;
                }
                if (int.Parse(strArray[3]) == 0xff)
                {
                    flag = false;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = btnCancel.DialogResult;
            Close();
        }
    }
}

