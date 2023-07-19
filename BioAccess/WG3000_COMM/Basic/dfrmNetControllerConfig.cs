namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmNetControllerConfig : frmBioAccess
    {
        private bool bAdjustTime;
        private bool bAutoUploadUsers;
        private bool bFirstShowInfo = true;
        private bool bInput5678;
        private bool bIPAndWEBConfigure;
        private bool bOption;
        private bool bOptionWeb;
        private bool bUpdateIPConfigure;
        private bool bUpdateSpecialCard_IPWEB;
        private bool bUpdateSuperCard_IPWEB;
        private bool bUpdateWEBConfigure;
        private bool bWEBEnabled;
        private bool bWebOnlyQuery;
        private int commPort = 0xea60;
        private int HttpPort = 80;
        private string SpecialCard1_IPWEB = "";
        private string SpecialCard2_IPWEB = "";
        private string strControllers = "";
        private string strGateway_IPWEB = "";
        private string strIP_IPWEB = "";
        private string strNETMASK_IPWEB = "";
        private string strSelectedFile1 = "";
        private string strSelectedFile2 = "";
        private string strWEBLanguage1 = "";
        private string strWEBLanguage2 = "";
        private string superCard1_IPWEB = "";
        private string superCard2_IPWEB = "";
        private int webDateFormat;

        public dfrmNetControllerConfig()
        {
            this.InitializeComponent();
        }

        public void AddDiscoveryEntry(object o, object pcIP)
        {
            byte[] pkt = (byte[]) o;
            string str = (string) pcIP;
            wgMjControllerConfigure configure = new wgMjControllerConfigure(pkt, 20);
            string str2 = "";
            if ((configure.webPort == 0) || (configure.webPort == 0xffff))
            {
                str2 = string.Format("{0},{1}", configure.webDeviceName, CommonStr.strWEBDisabled);
            }
            else
            {
                str2 = string.Format("{0},{1},{2},{3}", new object[] { configure.webDeviceName, configure.webLanguage, (wgAppConfig.CultureInfoStr == "zh-CHS") ? configure.webDateDisplayFormatCHS : configure.webDateDisplayFormat, configure.webPort.ToString() });
            }
            int num4 = this.dgvFoundControllers.Rows.Count + 1;
            str2 = string.Format("#{0}", num4.ToString().PadLeft(2, '0')) + str2;
            string[] strArray2 = new string[10];
            int num5 = this.dgvFoundControllers.Rows.Count + 1;
            strArray2[0] = num5.ToString().PadLeft(4, '0');
            strArray2[1] = configure.controllerSN.ToString();
            strArray2[2] = configure.ip.ToString();
            strArray2[3] = configure.mask.ToString();
            strArray2[4] = configure.gateway.ToString();
            strArray2[5] = configure.port.ToString();
            strArray2[6] = configure.MACAddr;
            strArray2[7] = str;
            strArray2[8] = str2;
            strArray2[9] = configure.wifi_channel.ToString();
            string[] values = strArray2;
            if (this.dgvFoundControllers.Rows.Count > 0)
            {
                for (int j = 0; j < this.dgvFoundControllers.Rows.Count; j++)
                {
                    if ((this.dgvFoundControllers.Rows[j].Cells[1].Value.ToString() == values[1]) && (this.dgvFoundControllers.Rows[j].Cells[7].Value.ToString() == values[7]))
                    {
                        return;
                    }
                }
            }
            for (int i = 0; i < values.Length; i++)
            {
                this.strControllers = this.strControllers + values[i] + ",";
            }
            this.dgvFoundControllers.Rows.Add(values);
        }

        private void addSelectedToSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                string str = "";
                int num2 = 0;
                for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
                {
                    int sN = int.Parse(this.dgvFoundControllers.SelectedRows[i].Cells[1].Value.ToString());
                    if ((sN != -1) && !icController.IsExisted2SN(sN, 0))
                    {
                        str = str + sN.ToString() + ",";
                        num2++;
                        this.lblSearchNow.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + sN.ToString();
                        this.toolStripStatusLabel2.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + sN.ToString();
                        using (dfrmController controller = new dfrmController())
                        {
                            controller.OperateNew = true;
                            controller.WindowState = FormWindowState.Minimized;
                            controller.Show();
                            controller.mtxtbControllerSN.Text = sN.ToString();
                            controller.btnNext.PerformClick();
                            controller.btnOK.PerformClick();
                            Application.DoEvents();
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddController, num2, str), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void btnAddToSystem_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                string str = "";
                int num2 = 0;
                for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
                {
                    if (this.dgvFoundControllers.Rows[i].Selected)
                    {
                        int sN = int.Parse(this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString());
                        if ((sN != -1) && !icController.IsExisted2SN(sN, 0))
                        {
                            str = str + sN.ToString() + ",";
                            num2++;
                            this.lblSearchNow.Text = this.dgvFoundControllers.Rows[i].Cells[0].Value.ToString() + "-" + sN.ToString();
                            this.toolStripStatusLabel2.Text = this.dgvFoundControllers.Rows[i].Cells[0].Value.ToString() + "-" + sN.ToString();
                            using (dfrmController controller = new dfrmController())
                            {
                                controller.OperateNew = true;
                                controller.WindowState = FormWindowState.Minimized;
                                controller.Show();
                                controller.mtxtbControllerSN.Text = sN.ToString();
                                controller.btnNext.PerformClick();
                                controller.btnOK.PerformClick();
                                Application.DoEvents();
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddController, num2, str), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectController);
            }
            else
            {
                using (dfrmTCPIPConfigure configure = new dfrmTCPIPConfigure())
                {
                    DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
                    configure.strSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
                    configure.strMac = dataGridViewRow.Cells["f_MACAddr"].Value.ToString();
                    configure.strIP = dataGridViewRow.Cells["f_IP"].Value.ToString();
                    configure.strMask = dataGridViewRow.Cells["f_Mask"].Value.ToString();
                    configure.strGateway = dataGridViewRow.Cells["f_Gateway"].Value.ToString();
                    configure.strTCPPort = dataGridViewRow.Cells["f_PORT"].Value.ToString();
                    string pCIPAddr = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
                    bool wifi_channel = (Convert.ToInt32(dataGridViewRow.Cells["f_WifiChannel"].Value.ToString()) != 0);
                    if (configure.ShowDialog(this) == DialogResult.OK)
                    {
                        string strSN = configure.strSN;
                        string strMac = configure.strMac;
                        string strIP = configure.strIP;
                        string strMask = configure.strMask;
                        string strGateway = configure.strGateway;
                        string strTCPPort = configure.strTCPPort;
                        string text = configure.Text;
                        this.Refresh();
                        Cursor.Current = Cursors.WaitCursor;
                        this.IPConfigureCPU(strSN, strMac, strIP, strMask, strGateway, strTCPPort, pCIPAddr, wifi_channel);
                        wgAppConfig.wgLog(text + "  SN=" + strSN + ", Mac=" + strMac + ",IP =" + strIP + ",Mask=" + strMask + ",Gateway=" + strGateway + ", Port = " + strTCPPort + ", PC IPAddr=" + pCIPAddr);
                        if (this.chkSearchAgain.Checked)
                        {
                            Thread.Sleep(0x1388);
                            this.btnSearch.PerformClick();
                        }
                        else
                        {
                            this.dgvFoundControllers.Rows.Remove(dataGridViewRow);
                        }
                    }
                }
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnDefault.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
            {
                string strSN = "-1";
                string strMac = "";
                string strIP = "192.168.0.0";
                string strMask = "255.255.255.0";
                string strGateway = "";
                string strTCPPort = "60000";
                string text = this.btnDefault.Text;
                this.IPConfigureCPU(strSN, strMac, strIP, strMask, strGateway, strTCPPort, "", false);
                wgAppConfig.wgLog(text + "  SN=" + strSN + ", Mac=" + strMac + ",IP =" + strIP + ",Mask=" + strMask + ",Gateway=" + strGateway + ", Port = " + strTCPPort + ", PC IPAddr=");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnIPAndWebConfigure_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectController);
            }
            else
            {
                string strSN;
                string strMac;
                string strIP;
                string strMask;
                string strGateway;
                string strTCPPort;
                string text;
                string str8;
                using (dfrmTCPIPWEBConfigure configure = new dfrmTCPIPWEBConfigure())
                {
                    DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
                    configure.strSN = row.Cells["f_ControllerSN"].Value.ToString();
                    configure.strMac = row.Cells["f_MACAddr"].Value.ToString();
                    configure.strIP = row.Cells["f_IP"].Value.ToString();
                    configure.strMask = row.Cells["f_Mask"].Value.ToString();
                    configure.strGateway = row.Cells["f_Gateway"].Value.ToString();
                    configure.strTCPPort = row.Cells["f_PORT"].Value.ToString();
                    str8 = row.Cells["f_PCIPAddr"].Value.ToString();
                    configure.strPCAddress = str8;
                    configure.strSearchedIP = row.Cells["f_IP"].Value.ToString();
                    configure.strSearchedMask = row.Cells["f_Mask"].Value.ToString();
                    if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
                    {
                        configure.cboLanguage.SelectedIndex = 0;
                        configure.cboLanguage2.SelectedIndex = 0;
                    }
                    else
                    {
                        configure.cboLanguage.SelectedIndex = 1;
                        configure.cboLanguage2.SelectedIndex = 1;
                    }
                    configure.cboDateFormat.SelectedIndex = 0;
                    if (this.bIPAndWEBConfigure)
                    {
                        configure.chkEditIP.Checked = this.bUpdateIPConfigure;
                        configure.grpIP.Enabled = this.bUpdateIPConfigure;
                        this.bOption = this.bOption || (this.commPort != 0xea60);
                        configure.btnOption.Enabled = !this.bOption;
                        configure.lblPort.Visible = this.bOption;
                        configure.nudPort.Visible = this.bOption;
                        configure.nudPort.Value = decimal.Parse(this.commPort.ToString());
                        configure.chkUpdateWebSet.Checked = this.bUpdateWEBConfigure;
                        configure.grpWEBEnabled.Enabled = this.bUpdateWEBConfigure;
                        configure.grpWEB.Enabled = this.bUpdateWEBConfigure && this.bWEBEnabled;
                        configure.optWEBEnabled.Checked = this.bWEBEnabled;
                        configure.cboLanguage.SelectedIndex = int.Parse(this.strWEBLanguage1);
                        configure.txtSelectedFileName.Text = this.strSelectedFile1;
                        this.bOptionWeb = this.bOptionWeb || (this.HttpPort != 80);
                        configure.btnOptionWEB.Enabled = !this.bOptionWeb;
                        configure.lblHttpPort.Visible = this.bOptionWeb;
                        configure.nudHttpPort.Visible = this.bOptionWeb;
                        configure.nudHttpPort.Value = decimal.Parse(this.HttpPort.ToString());
                        configure.cboDateFormat.SelectedIndex = this.webDateFormat;
                        configure.chkAdjustTime.Checked = this.bAdjustTime;
                        configure.chkWebOnlyQuery.Checked = this.bWebOnlyQuery;
                        configure.chkUpdateSuperCard.Checked = this.bUpdateSuperCard_IPWEB;
                        configure.grpSuperCards.Enabled = this.bUpdateSuperCard_IPWEB;
                        configure.txtSuperCard1.Text = this.superCard1_IPWEB;
                        configure.txtSuperCard2.Text = this.superCard2_IPWEB;
                        configure.chkUpdateSpecialCard.Checked = this.bUpdateSpecialCard_IPWEB;
                        configure.chkUpdateSpecialCard.Visible = this.bUpdateSpecialCard_IPWEB;
                        configure.grpSpecialCards.Enabled = this.bUpdateSpecialCard_IPWEB;
                        configure.grpSpecialCards.Visible = this.bUpdateSpecialCard_IPWEB;
                        configure.txtSpecialCard1.Text = this.SpecialCard1_IPWEB;
                        configure.txtSpecialCard2.Text = this.SpecialCard2_IPWEB;
                        configure.cboLanguage2.SelectedIndex = int.Parse(this.strWEBLanguage2);
                        configure.txtUsersFile.Text = this.strSelectedFile2;
                        configure.chkAutoUploadWEBUsers.Checked = this.bAutoUploadUsers;
                        configure.chkAutoUploadWEBUsers.Visible = this.bAutoUploadUsers;
                        if (configure.strIP == "192.168.0.0")
                        {
                            configure.strIP = this.strIP_IPWEB;
                            configure.strMask = this.strNETMASK_IPWEB;
                            configure.strGateway = this.strGateway_IPWEB;
                        }
                    }
                    if (configure.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                    this.bIPAndWEBConfigure = true;
                    this.bUpdateIPConfigure = configure.chkEditIP.Checked;
                    this.commPort = int.Parse(configure.nudPort.Value.ToString());
                    this.bOption = this.commPort != 0xea60;
                    this.bUpdateWEBConfigure = configure.chkUpdateWebSet.Checked;
                    this.bWEBEnabled = configure.optWEBEnabled.Checked;
                    this.strWEBLanguage1 = configure.cboLanguage.SelectedIndex.ToString();
                    this.strSelectedFile1 = configure.txtSelectedFileName.Text;
                    this.HttpPort = int.Parse(configure.nudHttpPort.Value.ToString());
                    this.bOptionWeb = this.HttpPort != 80;
                    this.bAdjustTime = configure.chkAdjustTime.Checked;
                    this.webDateFormat = configure.cboDateFormat.SelectedIndex;
                    this.bWebOnlyQuery = configure.chkWebOnlyQuery.Checked;
                    this.bUpdateSuperCard_IPWEB = configure.chkUpdateSuperCard.Checked;
                    this.superCard1_IPWEB = configure.txtSuperCard1.Text;
                    this.superCard2_IPWEB = configure.txtSuperCard2.Text;
                    this.bUpdateSpecialCard_IPWEB = configure.chkUpdateSpecialCard.Checked;
                    this.SpecialCard1_IPWEB = configure.txtSpecialCard1.Text;
                    this.SpecialCard2_IPWEB = configure.txtSpecialCard2.Text;
                    this.strWEBLanguage2 = configure.cboLanguage2.SelectedIndex.ToString();
                    this.strSelectedFile2 = configure.txtUsersFile.Text;
                    this.bAutoUploadUsers = configure.chkAutoUploadWEBUsers.Checked;
                    this.strIP_IPWEB = configure.strIP;
                    this.strNETMASK_IPWEB = configure.strMask;
                    this.strGateway_IPWEB = configure.strGateway;
                    if (configure.dtWebString != null)
                    {
                        this.dtWebStringAdvanced_IPWEB = configure.dtWebString.Copy();
                    }
                    else
                    {
                        this.dtWebStringAdvanced_IPWEB = null;
                    }
                    wgAppConfig.wgLog((sender as Button).Text + "  SN=" + row.Cells["f_ControllerSN"].Value.ToString());
                    strSN = configure.strSN;
                    strMac = configure.strMac;
                    strIP = configure.strIP;
                    strMask = configure.strMask;
                    strGateway = configure.strGateway;
                    strTCPPort = configure.strTCPPort;
                    text = configure.Text;
                }
                try
                {
                    this.Refresh();
                    if (string.IsNullOrEmpty(strSN))
                    {
                        return;
                    }
                    Cursor.Current = Cursors.WaitCursor;
                    if ((this.bUpdateWEBConfigure || this.bUpdateSpecialCard_IPWEB) || this.bUpdateSuperCard_IPWEB)
                    {
                        this.ipweb_webSet();
                    }
                    if (this.bAdjustTime)
                    {
                        using (icController controller = new icController())
                        {
                            controller.ControllerSN = int.Parse(strSN);
                            if (controller.AdjustTimeIP(DateTime.Now, 0, str8) < 0)
                            {
                                XMessageBox.Show(CommonStr.strAdjustTime + " " + CommonStr.strFailed);
                                return;
                            }
                            wgAppConfig.wgLog(strSN + " " + string.Format("{0}:{1}", CommonStr.strAdjustTimeOK, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                    }
                    if (this.bAutoUploadUsers)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        DataGridViewRow row2 = this.dgvFoundControllers.SelectedRows[0];
                        string lang = "utf-8";
                        this.ipweb_uploadusers(this.strSelectedFile2, int.Parse(row2.Cells["f_ControllerSN"].Value.ToString()), lang);
                    }
                    if (this.bUpdateIPConfigure)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        this.IPConfigureCPU(strSN, strMac, strIP, strMask, strGateway, strTCPPort, str8, false);
                        wgAppConfig.wgLog(text + "  SN=" + strSN + ", Mac=" + strMac + ",IP =" + strIP + ",Mask=" + strMask + ",Gateway=" + strGateway + ", Port = " + strTCPPort + ", PC IPAddr=" + str8);
                    }
                    else if (this.bUpdateWEBConfigure)
                    {
                        using (icController controller2 = new icController())
                        {
                            controller2.ControllerSN = int.Parse(strSN);
                            controller2.RebootControllerIP();
                        }
                    }
                }
                catch (Exception)
                {
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            uint maxValue = uint.MaxValue;
            Cursor.Current = Cursors.WaitCursor;
            this.lblCount.Text = "0";
            this.toolStripStatusLabel1.Text = "0";
            this.dgvFoundControllers.Rows.Clear();
            this.btnConfigure.Enabled = false;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            string str = "";
            this.btnSearch_Click_short(sender, e);
            this.btnSearch.Enabled = false;
            Thread.Sleep(100);
            this.Refresh();
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = maxValue;
            packet.iCallReturn = 0;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (WGPacket.bCommP)
            {
                WGPacket.bCommP = false;
                string commPStr = wgTools.CommPStr;
                wgTools.CommPStr = "";
                foreach (NetworkInterface interface2 in allNetworkInterfaces)
                {
                    if ((interface2.NetworkInterfaceType != NetworkInterfaceType.Loopback) && (interface2.OperationalStatus == OperationalStatus.Up))
                    {
                        UnicastIPAddressInformationCollection unicastAddresses = interface2.GetIPProperties().UnicastAddresses;
                        if (unicastAddresses.Count > 0)
                        {
                            Console.WriteLine(interface2.Description);
                            foreach (UnicastIPAddressInformation information in unicastAddresses)
                            {
                                if (((information.Address.AddressFamily == AddressFamily.InterNetwork) && !information.Address.IsIPv6LinkLocal) && (information.Address.ToString() != "127.0.0.1"))
                                {
                                    Console.WriteLine("  IP ............................. : {0}", information.Address.ToString());
                                    this.wgudp = new wgUdpComm(information.Address);
                                    Thread.Sleep(300);
                                    byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
                                    if (cmd == null)
                                    {
                                        return;
                                    }
                                    byte[] recv = null;
                                    this.wgudp.udp_get(cmd, 300, 0, null, 0xea60, ref recv);
                                    if (recv != null)
                                    {
                                        long num6 = DateTime.Now.Ticks + 0x3d0900L;
                                        this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { recv, information.Address.ToString() });
                                        while (DateTime.Now.Ticks < num6)
                                        {
                                            if (this.wgudp.PacketCount > 0)
                                            {
                                                while (this.wgudp.PacketCount > 0)
                                                {
                                                    recv = this.wgudp.GetPacket();
                                                    this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { recv, information.Address.ToString() });
                                                }
                                                num6 = DateTime.Now.Ticks + 0x3d0900L;
                                            }
                                            else
                                            {
                                                Thread.Sleep(100);
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }
                wgTools.CommPStr = commPStr;
                WGPacket.bCommP = true;
            }
            else
            {
                foreach (NetworkInterface interface3 in allNetworkInterfaces)
                {
                    if ((interface3.NetworkInterfaceType != NetworkInterfaceType.Loopback) && (interface3.OperationalStatus == OperationalStatus.Up))
                    {
                        num2++;
                        if (interface3.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                            num3++;
                        UnicastIPAddressInformationCollection informations2 = interface3.GetIPProperties().UnicastAddresses;
                        if (informations2.Count > 0)
                        {
                            Console.WriteLine(interface3.Description);
                            bool flag = true;
                            foreach (UnicastIPAddressInformation information2 in informations2)
                            {
                                if (((information2.Address.AddressFamily == AddressFamily.InterNetwork) && !information2.Address.IsIPv6LinkLocal) && (information2.Address.ToString() != "127.0.0.1"))
                                {
                                    if (flag)
                                    {
                                        num4++;
                                        flag = false;
                                    }
                                    str = str + string.Format("{0}, ", information2.Address.ToString());
                                    Console.WriteLine("  IP ............................. : {0}", information2.Address.ToString());
                                    this.wgudp = new wgUdpComm(information2.Address);
                                    Thread.Sleep(300);
                                    byte[] buffer3 = packet.ToBytes(this.wgudp.udpPort);
                                    if (buffer3 == null)
                                    {
                                        return;
                                    }
                                    byte[] buffer4 = null;
                                    this.wgudp.udp_get(buffer3, 500, uint.MaxValue, null, 0xea60, ref buffer4);
                                    if (buffer4 != null)
                                    {
                                        Thread.Sleep(300);
                                        long ticks = DateTime.Now.Ticks;
                                        long num8 = ticks + 0x3d0900L;
                                        int num9 = 0;
                                        this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { buffer4, information2.Address.ToString() });
                                        num9++;
                                        while (DateTime.Now.Ticks < num8)
                                        {
                                            if (this.wgudp.PacketCount > 0)
                                            {
                                                while (this.wgudp.PacketCount > 0)
                                                {
                                                    buffer4 = this.wgudp.GetPacket();
                                                    this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { buffer4, information2.Address.ToString() });
                                                    num9++;
                                                }
                                                num8 = DateTime.Now.Ticks + 0x3d0900L;
                                                long num13 = (DateTime.Now.Ticks - ticks) / 0x2710L;
                                                wgTools.WgDebugWrite(string.Format("搜索到控制器数={0}:所花时间={1}ms", num9.ToString(), num13.ToString()), new object[0]);
                                            }
                                            else
                                            {
                                                Thread.Sleep(100);
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            if (this.dgvFoundControllers.Rows.Count > 0)
            {
                this.dgvFoundControllers.Sort(this.dgvFoundControllers.Columns[1], ListSortDirection.Ascending);
                for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
                {
                    int num14 = i + 1;
                    this.dgvFoundControllers.Rows[i].Cells[0].Value = num14.ToString().PadLeft(4, '0');
                }
            }
            this.btnSearch.Enabled = true;
            wgAppConfig.wgLog(string.Format("{0} Count = {1:d}  : {2}", this.Text, this.dgvFoundControllers.Rows.Count, this.strControllers));
            wgAppConfig.wgLog(string.Format("{0}: Up Adapter Count = {1:d}; Up Adapter Wireless Count = {2:d}; Up Adapter With IPV4 Count = {3:d}; All IP : {4}", new object[] { string.Empty.PadLeft(this.Text.Length * 2, ' '), num2, num3, num4, str }));
            if (this.dgvFoundControllers.Rows.Count > 0)
            {
                this.btnConfigure.Enabled = true;
            }
            else
            {
                string text = CommonStr.strNoControllerInfo2Base;
                if (num2 == 0)
                {
                    text = CommonStr.strNoControllerInfo3PCNotConnected;
                }
                else if ((num2 >= 2) && (num3 >= 1))
                {
                    text = CommonStr.strNoControllerInfo1;
                }
                if (this.bFirstShowInfo)
                {
                    this.bFirstShowInfo = false;
                    XMessageBox.Show(text);
                }
            }
            this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
            this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click_short(object sender, EventArgs e)
        {
            uint maxValue = uint.MaxValue;
            Cursor.Current = Cursors.WaitCursor;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            string str = "";
            this.btnSearch.Enabled = false;
            Thread.Sleep(100);
            this.Refresh();
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x40;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = maxValue;
            packet.iCallReturn = 0;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (WGPacket.bCommP)
            {
                WGPacket.bCommP = false;
                string commPStr = wgTools.CommPStr;
                wgTools.CommPStr = "";
                foreach (NetworkInterface interface2 in allNetworkInterfaces)
                {
                    if ((interface2.NetworkInterfaceType != NetworkInterfaceType.Loopback) && (interface2.OperationalStatus == OperationalStatus.Up))
                    {
                        UnicastIPAddressInformationCollection unicastAddresses = interface2.GetIPProperties().UnicastAddresses;
                        if (unicastAddresses.Count > 0)
                        {
                            Console.WriteLine(interface2.Description);
                            foreach (UnicastIPAddressInformation information in unicastAddresses)
                            {
                                if (((information.Address.AddressFamily == AddressFamily.InterNetwork) && !information.Address.IsIPv6LinkLocal) && (information.Address.ToString() != "127.0.0.1"))
                                {
                                    Console.WriteLine("  IP ............................. : {0}", information.Address.ToString());
                                    this.wgudp = new wgUdpComm(information.Address);
                                    Thread.Sleep(300);
                                    byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
                                    if (cmd == null)
                                    {
                                        return;
                                    }
                                    byte[] recv = null;
                                    this.wgudp.udp_get(cmd, 300, 0, null, 0xea60, ref recv);
                                    if (recv != null)
                                    {
                                        long num6 = DateTime.Now.Ticks + 0x3d0900L;
                                        this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { recv, information.Address.ToString() });
                                        while (DateTime.Now.Ticks < num6)
                                        {
                                            if (this.wgudp.PacketCount > 0)
                                            {
                                                while (this.wgudp.PacketCount > 0)
                                                {
                                                    recv = this.wgudp.GetPacket();
                                                    this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { recv, information.Address.ToString() });
                                                }
                                                num6 = DateTime.Now.Ticks + 0x3d0900L;
                                            }
                                            else
                                            {
                                                Thread.Sleep(100);
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }
                wgTools.CommPStr = commPStr;
                WGPacket.bCommP = true;
            }
            else // bug fixed at 130610
            {
                foreach (NetworkInterface interface3 in allNetworkInterfaces)
                {
                    if ((interface3.NetworkInterfaceType != NetworkInterfaceType.Loopback) && (interface3.OperationalStatus == OperationalStatus.Up))
                    {
                        num2++;
                        if (interface3.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                            num3++;
                        UnicastIPAddressInformationCollection informations2 = interface3.GetIPProperties().UnicastAddresses;
                        if (informations2.Count > 0)
                        {
                            Console.WriteLine(interface3.Description);
                            bool flag = true;
                            foreach (UnicastIPAddressInformation information2 in informations2)
                            {
                                if (((information2.Address.AddressFamily == AddressFamily.InterNetwork) && !information2.Address.IsIPv6LinkLocal) && (information2.Address.ToString() != "127.0.0.1"))
                                {
                                    if (flag)
                                    {
                                        num4++;
                                        flag = false;
                                    }
                                    str = str + string.Format("{0}, ", information2.Address.ToString());
                                    Console.WriteLine("  IP ............................. : {0}", information2.Address.ToString());
                                    this.wgudp = new wgUdpComm(information2.Address);
                                    Thread.Sleep(300);
                                    byte[] buffer3 = packet.ToBytes(this.wgudp.udpPort);
                                    if (buffer3 == null)
                                    {
                                        return;
                                    }
                                    byte[] buffer4 = null;
                                    this.wgudp.udp_get(buffer3, 500, uint.MaxValue, null, 0xea60, ref buffer4);
                                    if (buffer4 != null)
                                    {
                                        Thread.Sleep(300);
                                        long ticks = DateTime.Now.Ticks;
                                        long num8 = ticks + 0x3d0900L;
                                        int num9 = 0;
                                        this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { buffer4, information2.Address.ToString() });
                                        num9++;
                                        while (DateTime.Now.Ticks < num8)
                                        {
                                            if (this.wgudp.PacketCount > 0)
                                            {
                                                while (this.wgudp.PacketCount > 0)
                                                {
                                                    buffer4 = this.wgudp.GetPacket();
                                                    this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { buffer4, information2.Address.ToString() });
                                                    num9++;
                                                }
                                                num8 = DateTime.Now.Ticks + 0x3d0900L;
                                                long num12 = (DateTime.Now.Ticks - ticks) / 0x2710L;
                                                wgTools.WgDebugWrite(string.Format("搜索到控制器数={0}:所花时间={1}ms", num9.ToString(), num12.ToString()), new object[0]);
                                            }
                                            else
                                            {
                                                Thread.Sleep(100);
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
            this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
        }

        private void clearSwipesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (icOperator.OperatorID != 1)
            {
                XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!this.bInput5678)
                {
                    using (dfrmInputNewName name = new dfrmInputNewName())
                    {
                        name.setPasswordChar('*');
                        if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "5678"))
                        {
                            return;
                        }
                    }
                }
                this.bInput5678 = true;
                if (this.dgvFoundControllers.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
                    string s = row.Cells["f_ControllerSN"].Value.ToString();
                    string ipString = row.Cells["f_PCIPAddr"].Value.ToString();
                    string ipAddr = "";
                    if (XMessageBox.Show(this, sender.ToString() + " " + s + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
                    {
                        IPAddress address;
                        int ipPort = 0xea60;
                        if (this.wgudp != null)
                        {
                            this.wgudp = null;
                        }
                        if (IPAddress.TryParse(ipString, out address))
                        {
                            this.wgudp = new wgUdpComm(IPAddress.Parse(ipString));
                        }
                        else
                        {
                            this.wgudp = new wgUdpComm();
                        }
                        Thread.Sleep(300);
                        WGPacketWith1152 with = new WGPacketWith1152();
                        with.type = 0x25;
                        with.code = 0x20;
                        with.iDevSnFrom = 0;
                        with.iCallReturn = 0;
                        WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH();
                        tssi_flash.type = 0x21;
                        tssi_flash.code = 0x30;
                        tssi_flash.iDevSnFrom = 0;
                        tssi_flash.iDevSnTo = uint.Parse(s);
                        tssi_flash.iCallReturn = 0;
                        tssi_flash.ucData = new byte[0x400];
                        try
                        {
                            Thread.Sleep(300);
                            tssi_flash.iStartFlashAddr = 0x4c9000;
                            tssi_flash.iEndFlashAddr = (tssi_flash.iStartFlashAddr + 0x400) - 1;
                            for (int i = 0; i < 0x400; i++)
                            {
                                tssi_flash.ucData[i] = 0xff;
                            }
                            byte[] recv = null;
                            while (tssi_flash.iStartFlashAddr <= 0x4cb000)
                            {
                                if (this.wgudp.udp_get(tssi_flash.ToBytes(this.wgudp.udpPort), 1000, tssi_flash.xid, ipAddr, ipPort, ref recv) < 0)
                                {
                                    break;
                                }
                                tssi_flash.iStartFlashAddr += 0x400;
                            }
                            using (icController controller = new icController())
                            {
                                controller.ControllerSN = int.Parse(row.Cells["f_ControllerSN"].Value.ToString());
                                if (controller.RestoreAllSwipeInTheControllersIP() > 0)
                                {
                                    controller.UpdateFRamIP(9, 0);
                                    controller.RebootControllerIP();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        wgAppConfig.wgLog(sender.ToString() + "  SN=" + s);
                    }
                }
                else
                {
                    XMessageBox.Show(CommonStr.strSelectController);
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dgvFoundControllers.Rows.Clear();
            this.lblCount.Text = "0";
            this.toolStripStatusLabel1.Text = "0";
        }

        private void communicationTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.dgvFoundControllers.Rows.Count > 0) && (this.dgvFoundControllers.SelectedRows.Count <= 0))
            {
                XMessageBox.Show(CommonStr.strSelectController);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                using (icController controller = new icController())
                {
                    controller.ControllerSN = -1;
                    if (this.dgvFoundControllers.Rows.Count > 0)
                    {
                        DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
                        controller.ControllerSN = int.Parse(row.Cells["f_ControllerSN"].Value.ToString());
                    }
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    wgTools.WriteLine("control.SpecialPingIP Start");
                    for (int i = 0; i < 200; i++)
                    {
                        num++;
                        if (controller.SpecialPingIP() == 1)
                        {
                            num2++;
                        }
                        else
                        {
                            num3++;
                        }
                    }
                    wgTools.WriteLine("control.SpecialPingIP End");
                    wgUdpComm.triesTotal = 0L;
                    wgTools.WriteLine("control.Test1024 Start");
                    int page = 0;
                    string str = "";
                    if (controller.test1024Write() < 0)
                    {
                        str = str + CommonStr.strCommLargePacketWriteFailed + "\r\n";
                    }
                    int num5 = controller.test1024Read(100, ref page);
                    if (num5 < 0)
                    {
                        str = str + CommonStr.strCommLargePacketReadFailed + num5.ToString() + "\r\n";
                    }
                    if (wgUdpComm.triesTotal > 0L)
                    {
                        string str3 = str;
                        str = str3 + CommonStr.strCommLargePacketTryTimes + " = " + wgUdpComm.triesTotal.ToString() + "\r\n";
                    }
                    wgTools.WriteLine("control.Test1024 End");
                    if (num3 == 0)
                    {
                        if (str == "")
                        {
                            wgAppConfig.wgLog(string.Concat(new object[] { sender.ToString(), "  SN=", controller.ControllerSN, " ", CommonStr.strCommOK }));
                            XMessageBox.Show(string.Format("{0}: {1} -- {2}", controller.ControllerSN, sender.ToString(), CommonStr.strCommOK));
                        }
                        else
                        {
                            wgAppConfig.wgLog(string.Concat(new object[] { sender.ToString(), "  SN=", controller.ControllerSN, " ", CommonStr.strCommLose, " ", str }));
                            XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n\r\n{3}", new object[] { controller.ControllerSN, sender.ToString(), CommonStr.strCommLose, str }), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        string str2 = string.Format(" {0}: {1}={2}, {3}={4}, {5} = {6}", new object[] { CommonStr.strCommPacket, CommonStr.strCommPacketSent, num, CommonStr.strCommPacketReceived, num2, CommonStr.strCommPacketLost, num3 }) + "\r\n";
                        wgAppConfig.wgLog(string.Concat(new object[] { sender.ToString(), "  SN=", controller.ControllerSN, " ", CommonStr.strCommLose, " ", str2, str }));
                        XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n\r\n{3}\r\n{4}", new object[] { controller.ControllerSN, sender.ToString(), CommonStr.strCommLose, str2, str }), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void dfrmNetControllerConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmNetControllerConfig_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                    }
                    this.dfrmFind1.setObjtoFind(this.dgvFoundControllers, null);
                }
                if (e.Control && (e.KeyValue == 0x43))
                {
                    string data = "";
                    for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
                    {
                        for (int j = 0; j < this.dgvFoundControllers.ColumnCount; j++)
                        {
                            data = data + this.dgvFoundControllers.Rows[i].Cells[j].Value.ToString() + "\t";
                        }
                        data = data + "\r\n";
                    }
                    Clipboard.SetDataObject(data, false);
                }
                if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    this.funCtrlShiftQ();
                }
                if ((e.Control && e.Shift) && (e.KeyValue == 0x54))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        this.FuncControlShiftT();
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void dfrmNetControllerConfig_Load(object sender, EventArgs e)
        {
            this.btnConfigure.Enabled = false;
            this.btnSearch.PerformClick();
        }

        private void dgvFoundControllers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.btnConfigure.Enabled)
            {
                this.btnConfigure.PerformClick();
            }
        }

        public static string encDbConnection(string strDbConnection)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(strDbConnection))
                {
                    str = "ENC" + WGPacket.Ept(strDbConnection);
                }
            }
            catch
            {
            }
            return str;
        }

        private void findF3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dfrmFind1 == null)
                {
                    this.dfrmFind1 = new dfrmFind();
                }
                this.dfrmFind1.setObjtoFind(this.dgvFoundControllers, null);
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void formatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectController);
            }
            else
            {
                using (icController controller = new icController())
                {
                    DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
                    controller.ControllerSN = int.Parse(row.Cells["f_ControllerSN"].Value.ToString());
                    byte[] data = new byte[0x480];
                    data[0x403] = 0xa5;
                    data[0x402] = 0xa5;
                    data[0x401] = 0xa5;
                    data[0x400] = 0xa5;
                    controller.UpdateConfigureSuperIP(data);
                    wgAppConfig.wgLog(sender.ToString() + "  SN=" + controller.ControllerSN);
                    XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[] { controller.ControllerSN, sender.ToString(), CommonStr.strSuccessfully, CommonStr.strRebootController }));
                }
            }
        }

        private void FuncControlShiftT()
        {
            uint result = 0;
            uint num2 = 0;
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = "Start";
                name.label1.Text = CommonStr.strControllerSN;
                name.strNewName = "100190001";
                if (((name.ShowDialog(this) != DialogResult.OK) || !uint.TryParse(name.strNewName, out result)) || (wgMjController.GetControllerType((int) result) == 0))
                {
                    return;
                }
            }
            this.dgvFoundControllers.Rows.Clear();
            this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
            this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
            Cursor.Current = Cursors.WaitCursor;
            this.btnConfigure.Enabled = false;
            this.btnSearch.Enabled = false;
            Thread.Sleep(100);
            this.Refresh();
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iCallReturn = 0;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            num2 = (result + 100) - 1;
            this.wgudp = null;
            result = (result % 0x5f5e100) + 0x5f5e100;
            uint num3 = result % 0x5f5e100;
            num2 = (num2 % 0x5f5e100) + 0x5f5e100;
            while ((result <= num2) || (num2 < 0x17d78400))
            {
                if (result > num2)
                {
                    if (result < 0xbebc200)
                    {
                        result = num3 + 0xbebc200;
                        num2 = (num2 % 0x5f5e100) + 0xbebc200;
                    }
                    else
                    {
                        result = num3 + 0x17d78400;
                        num2 = (num2 % 0x5f5e100) + 0x17d78400;
                    }
                }
                if (((num2 - result) % 5) == 0)
                {
                    this.lblSearchNow.Text = result.ToString();
                    this.toolStripStatusLabel2.Text = result.ToString();
                    this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
                    this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
                    this.Refresh();
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;
                }
                packet.iDevSnTo = result;
                result++;
                if (WGPacket.bCommP)
                {
                    WGPacket.bCommP = false;
                    string commPStr = wgTools.CommPStr;
                    wgTools.CommPStr = "";
                    foreach (NetworkInterface interface2 in allNetworkInterfaces)
                    {
                        UnicastIPAddressInformationCollection unicastAddresses = interface2.GetIPProperties().UnicastAddresses;
                        if (unicastAddresses.Count > 0)
                        {
                            Console.WriteLine(interface2.Description);
                            foreach (UnicastIPAddressInformation information in unicastAddresses)
                            {
                                if (!information.Address.IsIPv6LinkLocal && (information.Address.ToString() != "127.0.0.1"))
                                {
                                    Console.WriteLine("  IP ............................. : {0}", information.Address.ToString());
                                    if (this.wgudp == null)
                                    {
                                        this.wgudp = new wgUdpComm(information.Address);
                                        Thread.Sleep(300);
                                    }
                                    else if (this.wgudp.localIP.ToString() != information.Address.ToString())
                                    {
                                        this.wgudp = new wgUdpComm(information.Address);
                                        Thread.Sleep(300);
                                    }
                                    byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
                                    if (cmd == null)
                                    {
                                        return;
                                    }
                                    byte[] recv = null;
                                    this.wgudp.udp_get(cmd, 300, 0, null, 0xea60, ref recv);
                                    if ((recv != null) && !this.isExisted(packet.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
                                    {
                                        this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { recv, information.Address.ToString() });
                                        long ticks = DateTime.Now.Ticks;
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                    wgTools.CommPStr = commPStr;
                    WGPacket.bCommP = true;
                }
                foreach (NetworkInterface interface3 in allNetworkInterfaces)
                {
                    UnicastIPAddressInformationCollection informations2 = interface3.GetIPProperties().UnicastAddresses;
                    if (informations2.Count > 0)
                    {
                        Console.WriteLine(interface3.Description);
                        foreach (UnicastIPAddressInformation information2 in informations2)
                        {
                            if (!information2.Address.IsIPv6LinkLocal && (information2.Address.ToString() != "127.0.0.1"))
                            {
                                Console.WriteLine("  IP ............................. : {0}", information2.Address.ToString());
                                if (this.wgudp == null)
                                {
                                    this.wgudp = new wgUdpComm(information2.Address);
                                    Thread.Sleep(300);
                                }
                                else if (this.wgudp.localIP.ToString() != information2.Address.ToString())
                                {
                                    this.wgudp = new wgUdpComm(information2.Address);
                                    Thread.Sleep(300);
                                }
                                byte[] buffer3 = packet.ToBytes(this.wgudp.udpPort);
                                if (buffer3 == null)
                                {
                                    return;
                                }
                                byte[] buffer4 = null;
                                this.wgudp.udp_get(buffer3, 300, uint.MaxValue, null, 0xea60, ref buffer4);
                                if ((buffer4 != null) && !this.isExisted(packet.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
                                {
                                    this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { buffer4, information2.Address.ToString() });
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            this.btnSearch.Enabled = true;
            if (this.dgvFoundControllers.Rows.Count > 0)
            {
                this.btnConfigure.Enabled = true;
            }
            this.lblSearchNow.Text = num2.ToString();
            this.toolStripStatusLabel2.Text = num2.ToString();
            this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
            this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
            Cursor.Current = Cursors.Default;
        }

        private void funCtrlShiftQ()
        {
            string strNewName = null;
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.setPasswordChar('*');
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    strNewName = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                strNewName = strNewName.ToUpper();
                int num = (DateTime.Now.Month + DateTime.Now.Day) + DateTime.Now.Hour;
                if (strNewName == ("WGTEST" + num.ToString()))
                {
                    this.dfrmTest = new frmTestController();
                    this.dfrmTest.Show();
                }
                else
                {
                    switch (strNewName)
                    {
                        case "5678":
                            this.btnDefault.Visible = true;
                            this.restoreDefaultParamToolStripMenuItem.Visible = true;
                            this.restoreAllSwipesToolStripMenuItem.Visible = true;
                            this.clearSwipesToolStripMenuItem.Visible = true;
                            return;

                        case "IP":
                            this.btnDefault.Visible = true;
                            return;

                        case "WEB":
                            this.btnIPAndWebConfigure.Visible = true;
                            return;

                        case "PARAM":
                            this.restoreDefaultParamToolStripMenuItem.Visible = true;
                            return;

                        case "RECORD":
                            this.restoreAllSwipesToolStripMenuItem.Visible = true;
                            return;

                        case "FORMAT5678":
                            this.formatToolStripMenuItem.Visible = true;
                            return;

                        case "CSN":
                        case "CSQ":
                            this.dfrmTest = new frmTestController();
                            this.dfrmTest.Show();
                            return;

                        case "P":
                            this.frmProductFormat1 = new frmProductFormat();
                            this.frmProductFormat1.Show();
                            return;

                        case "ENC":
                            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("dbConnection")))
                            {
                                wgAppConfig.UpdateKeyVal("dbConnection", encDbConnection(wgAppConfig.dbConString));
                                wgAppConfig.runUpdateSql("Delete From t_s_wglog");
                                FileInfo info = new FileInfo(Application.StartupPath + @"\BioAccess.log");
                                if (info.Exists)
                                {
                                    info.Delete();
                                }
                                wgAppConfig.wgLog("Encrypt DB Connection");
                                XMessageBox.Show("OK");
                                return;
                            }
                            return;

                        case "DES":
                            XMessageBox.Show(wgAppConfig.dbConString);
                            return;
                    }
                }
            }
        }

        private void IPConfigure(string strSN, string strMac, string strIP, string strMask, string strGateway, string strTCPPort, string PCIPAddr)
        {
            IPAddress address;
            if (this.wgudp != null)
            {
                this.wgudp = null;
            }
            if (IPAddress.TryParse(PCIPAddr, out address))
            {
                this.wgudp = new wgUdpComm(IPAddress.Parse(PCIPAddr));
            }
            else
            {
                this.wgudp = new wgUdpComm();
            }
            Thread.Sleep(300);
            WGPacketWith1152 with = new WGPacketWith1152();
            with.type = 0x24;
            with.code = 0x20;
            with.iDevSnFrom = 0;
            if (int.Parse(strSN) == -1)
            {
                with.iDevSnTo = uint.MaxValue;
            }
            else
            {
                with.iDevSnTo = uint.Parse(strSN);
            }
            with.iCallReturn = 0;
            int index = 0x74;
            IPAddress.Parse(strIP).GetAddressBytes().CopyTo(with.ucData, index);
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 120;
            IPAddress.Parse(strMask).GetAddressBytes().CopyTo(with.ucData, index);
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 0x7c;
            IPAddress.Parse(strGateway).GetAddressBytes().CopyTo(with.ucData, index);
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 0x80;
            with.ucData[index] = (byte) (int.Parse(strTCPPort) & 0xff);
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with.ucData[index] = (byte) ((int.Parse(strTCPPort) >> 8) & 0xff);
            with.ucData[0x400 + (index >> 3)] = (byte) (with.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            byte[] cmd = with.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WgDebugWrite("Err: IP Configure", new object[0]);
            }
            else
            {
                byte[] recv = null;
                this.wgudp.udp_get(cmd, 1000, 0x7fffffff, null, 0xea60, ref recv);
            }
        }

        private void IPConfigureCPU(string strSN, string strMac, 
            string strIP, string strMask, string strGateway, string strTCPPort, string PCIPAddr, bool wifi_channel)
        {
            IPAddress address;
            if (this.wgudp != null)
            {
                this.wgudp = null;
            }
            if (IPAddress.TryParse(PCIPAddr, out address))
            {
                this.wgudp = new wgUdpComm(IPAddress.Parse(PCIPAddr));
            }
            else
            {
                this.wgudp = new wgUdpComm();
            }
            Thread.Sleep(300);
            WGPacketWith1152 with = new WGPacketWith1152();
            with.type = 0x25;
            with.code = 0x20;
            with.iDevSnFrom = 0;
            with.iCallReturn = 0;
            if (int.Parse(strSN) == -1)
            {
                with.iDevSnTo = uint.MaxValue;
            }
            else
            {
                with.iDevSnTo = uint.Parse(strSN);
            }
            byte[] buffer = new byte[0x480];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
            WGPacketWith1152 with2 = new WGPacketWith1152();
            int index = !wifi_channel ? 0x74 : 0x124;
            IPAddress.Parse(strIP).GetAddressBytes().CopyTo(with2.ucData, index);
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = !wifi_channel ? 0x78 : 0x128;
            IPAddress.Parse(strMask).GetAddressBytes().CopyTo(with2.ucData, index);
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = !wifi_channel ? 0x7c : 0x12c;
            if (string.IsNullOrEmpty(strGateway))
            {
                with2.ucData[index] = 0;
                with2.ucData[index + 1] = 0;
                with2.ucData[index + 2] = 0;
                with2.ucData[index + 3] = 0;
            }
            else
            {
                IPAddress.Parse(strGateway).GetAddressBytes().CopyTo(with2.ucData, index);
            }
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = !wifi_channel ? 0x80 : 0x130;
            with2.ucData[index] = (byte) (int.Parse(strTCPPort) & 0xff);
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            with2.ucData[index] = (byte) ((int.Parse(strTCPPort) >> 8) & 0xff);
            with2.ucData[0x400 + (index >> 3)] = (byte) (with2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 0;
            for (int j = 0; j < 0x0e; j++)
            {
                buffer[index] = (byte)(with2.ucData[(!wifi_channel ? 0x74 : 0x124) + j] & 0xff);
                buffer[0x400 + (index >> 3)] = (byte) (buffer[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
            }
            buffer.CopyTo(with.ucData, 0);
            byte[] cmd = with.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WgDebugWrite("Err: IP Configure", new object[0]);
            }
            else
            {
                byte[] recv = null;
                this.wgudp.udp_get(cmd, 1000, 0x7fffffff, null, 0xea60, ref recv);
            }
        }

        private int ipweb_uploadusers(string userFile, int controllerSN, string lang)
        {
            string path = userFile;
            if (!System.IO.File.Exists(path))
            {
                return 0;
            }
            this.tb = new DataTable();
            this.tb.TableName = wgAppConfig.dbWEBUserName;
			this.tb.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
            this.tb.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
            this.tb.Columns.Add("f_ConsumerName");
            this.tb.ReadXml(path);
            this.tb.AcceptChanges();
            this.dv = new DataView(this.tb);
            this.dv.Sort = "f_ConsumerNO ASC";
            string pCIPAddr = null;
            using (wgMjControllerPrivilege privilege = new wgMjControllerPrivilege())
            {
                privilege.AllowUpload();
                if (this.dtPrivilege != null)
                {
                    dtPrivilege.Rows.Clear();
                    dtPrivilege.Dispose();
                    dtPrivilege = null;
                    GC.Collect();
                }
                if (tbFpTempl != null)
                {
                    tbFpTempl.Rows.Clear();
                    tbFpTempl.Dispose();
                    tbFpTempl = null;
                    GC.Collect();
                }
                if (tbFaceTempl != null)
                {
                    tbFaceTempl.Rows.Clear();
                    tbFaceTempl.Dispose();
                    tbFaceTempl = null;
                    GC.Collect();
                }
                this.dtPrivilege = new DataTable("Privilege");
                this.dtPrivilege.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                this.dtPrivilege.Columns.Add("f_CardNO", System.Type.GetType("System.String"));
                this.dtPrivilege.Columns.Add("f_BeginYMD", System.Type.GetType("System.DateTime"));
                this.dtPrivilege.Columns.Add("f_EndYMD", System.Type.GetType("System.DateTime"));
                this.dtPrivilege.Columns.Add("f_PIN", System.Type.GetType("System.String"));
                this.dtPrivilege.Columns.Add("f_ControlSegID1", System.Type.GetType("System.Byte"));
                this.dtPrivilege.Columns["f_ControlSegID1"].DefaultValue = 0;
                this.dtPrivilege.Columns.Add("f_ControlSegID2", System.Type.GetType("System.Byte"));
                this.dtPrivilege.Columns["f_ControlSegID2"].DefaultValue = 0;
                this.dtPrivilege.Columns.Add("f_ControlSegID3", System.Type.GetType("System.Byte"));
                this.dtPrivilege.Columns["f_ControlSegID3"].DefaultValue = 0;
                this.dtPrivilege.Columns.Add("f_ControlSegID4", System.Type.GetType("System.Byte"));
                this.dtPrivilege.Columns["f_ControlSegID4"].DefaultValue = 0;
                //this.dtPrivilege.Columns.Add("f_ControlSegID5", System.Type.GetType("System.Byte"));
                //this.dtPrivilege.Columns["f_ControlSegID5"].DefaultValue = 0;
                this.dtPrivilege.Columns.Add("f_ConsumerName", System.Type.GetType("System.String"));

                tbFpTempl = new DataTable("FpTempl");
                tbFpTempl.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                tbFpTempl.Columns.Add("f_Finger", System.Type.GetType("System.Int32"));
                tbFpTempl.Columns.Add("f_Templ", System.Type.GetType("System.Byte[]"));
                tbFpTempl.Columns.Add("f_Duress", System.Type.GetType("System.Int32"));

                tbFaceTempl = new DataTable("FaceTempl");
                tbFaceTempl.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                tbFaceTempl.Columns.Add("f_Templ", System.Type.GetType("System.Byte[]"));

                uint num = 0;
                for (int i = 0; i < this.dv.Count; i++)
                {
                    DataRow row = this.dtPrivilege.NewRow();
					row["f_ConsumerNO"] = (uint) this.dv[i]["f_ConsumerNO"];
                    row["f_CardNO"] = (uint) this.dv[i]["f_CardNO"];
                    row["f_BeginYMD"] = DateTime.Parse("2011-1-1");
                    row["f_EndYMD"] = DateTime.Parse("2029-12-31");
                    row["f_PIN"] = 0;
                    row["f_ControlSegID1"] = 1;
                    row["f_ControlSegID2"] = 1;
                    row["f_ControlSegID3"] = 1;
                    row["f_ControlSegID4"] = 1;
                    //row["f_ControlSegID5"] = 1;
                    row["f_ConsumerName"] = this.dv[i]["f_ConsumerName"];
                    if (((uint) row["f_ConsumerNO"]) <= num)
                    {
                        XMessageBox.Show(CommonStr.strFailed);
                        return 0;
                    }
                    num = (uint) row["f_ConsumerNO"];
                    this.dtPrivilege.Rows.Add(row);
                }
                this.dtPrivilege.AcceptChanges();
                privilege.bAllowUploadUserName = true;
                int ret;
                if ((ret = privilege.UploadIP(controllerSN, null, 0xea60, "DOOR NAME", this.dtPrivilege, tbFpTempl, tbFaceTempl, pCIPAddr)) < 0)
                {
                    switch (ret)
                    {
                        case wgTools.ErrorCode.ERR_DB_IS_FULL:
                        case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                        case wgTools.ErrorCode.ERR_FAIL:
                            break;
                        default:
                            ret = -1;
                            break;
                    }
                    return ret;
                }
            }
            return 1;
        }

        private void ipweb_webSet()
        {
            int num3;
            DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
            int num = int.Parse(row.Cells["f_ControllerSN"].Value.ToString());
            string pCIPAddr = row.Cells["f_PCIPAddr"].Value.ToString();
            byte[] data = new byte[0x480];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
            if (this.bUpdateSuperCard_IPWEB)
            {
                ulong maxValue = ulong.MaxValue;
                ulong result = ulong.MaxValue;
                ulong.TryParse(this.superCard1_IPWEB, out maxValue);
                ulong.TryParse(this.superCard2_IPWEB, out result);
                if (maxValue == 0L)
                {
                    maxValue = ulong.MaxValue;
                }
                if (result == 0L)
                {
                    result = ulong.MaxValue;
                }
                wgAppConfig.wgLog("  SN=" + num.ToString() + string.Format("  Super Card1={0},Card2={1}", maxValue.ToString(), result.ToString()));
                num3 = 0x90;
                data[num3] = (byte) (maxValue & ((ulong) 0xffL));
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 8);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 0x10);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 0x18);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 0x20);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 40);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 0x30);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (maxValue >> 0x38);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result & ((ulong) 0xffL));
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 8);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 0x10);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 0x18);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 0x20);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 40);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 0x30);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (result >> 0x38);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
            }
            if (this.bUpdateSpecialCard_IPWEB)
            {
                ulong num6 = ulong.MaxValue;
                ulong num7 = ulong.MaxValue;
                ulong.TryParse(this.SpecialCard1_IPWEB, out num6);
                ulong.TryParse(this.SpecialCard2_IPWEB, out num7);
                wgAppConfig.wgLog("  SN=" + num.ToString() + string.Format("  Special Card1={0},Card2={1}", num6.ToString(), num7.ToString()));
                if (num6 == 0L)
                {
                    num6 = ulong.MaxValue;
                }
                if (num7 == 0L)
                {
                    num7 = ulong.MaxValue;
                }
                num3 = 160;
                data[num3] = (byte) (num6 & ((ulong) 0xffL));
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 8);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 0x10);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 0x18);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 0x20);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 40);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 0x30);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num6 >> 0x38);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 & ((ulong) 0xffL));
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 8);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 0x10);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 0x18);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 0x20);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 40);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 0x30);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
                data[num3] = (byte) (num7 >> 0x38);
                data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
                num3++;
            }
            if (!this.bUpdateWEBConfigure)
            {
                goto Label_0A99;
            }
            int httpPort = 80;
            int num9 = 0x3000;
            if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
            {
                num9 = 0x2000;
            }
            if (!this.bWEBEnabled)
            {
                httpPort = 0;
            }
            else
            {
                httpPort = this.HttpPort;
                switch (int.Parse(this.strWEBLanguage1))
                {
                    case 0:
                        num9 = 0x2000;
                        goto Label_08CD;

                    case 1:
                        num9 = 0x3000;
                        goto Label_08CD;

                    case 2:
                        num9 = 0x38000;
                        goto Label_08CD;
                }
                num9 = 0x3000;
            }
        Label_08CD:
            num3 = 100;
            data[num3] = (byte) (num9 & 0xff);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3++;
            data[num3] = (byte) (num9 >> 8);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3++;
            data[num3] = (byte) (num9 >> 0x10);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3++;
            data[num3] = (byte) (num9 >> 0x18);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3 = 0x60;
            data[num3] = (byte) (httpPort & 0xff);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3++;
            data[num3] = (byte) (httpPort >> 8);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3 = 0x62;
            data[num3] = (byte) (this.webDateFormat & 0xff);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
            num3 = 0x63;
            data[num3] = (byte) ((this.bWebOnlyQuery ? 0xa5 : 0) & 0xff);
            data[0x400 + (num3 >> 3)] = (byte) (data[0x400 + (num3 >> 3)] | ((byte) (((int) 1) << (num3 & 7))));
        Label_0A99:
            if ((this.bUpdateSuperCard_IPWEB || this.bUpdateWEBConfigure) || this.bUpdateSpecialCard_IPWEB)
            {
                using (icController controller = new icController())
                {
                    controller.ControllerSN = num;
                    controller.UpdateConfigureCPUSuperIP(data, "", pCIPAddr);
                    wgAppConfig.wgLog("  SN=" + controller.ControllerSN + string.Format(" WEB Language={0}", this.strWEBLanguage1.ToString()));
                }
            }
            wgAppConfig.wgLog(this.btnIPAndWebConfigure.Text + "  SN=" + num);
            if ((this.bUpdateWEBConfigure && this.bWEBEnabled) && (int.Parse(this.strWEBLanguage1) == 2))
            {
                string str2;
                byte[] buffer2 = new byte[0x1000];
                int num10 = 0;
                byte num11 = 0;
                int num12 = 0x2000;
                for (int j = 0; j < buffer2.Length; j++)
                {
                    buffer2[j] = num11;
                }
                num10 = 0x38000;
                num12 = 0x38400;
                this.dv = new DataView(this.dtWebStringAdvanced_IPWEB);
                string name = this.dv[0][2].ToString();
                for (int k = 0; k <= (this.dv.Count - 1); k++)
                {
                    buffer2[num10 - 0x38000] = (byte) (num12 & 0xff);
                    buffer2[(num10 - 0x38000) + 1] = (byte) (num12 >> 8);
                    buffer2[(num10 - 0x38000) + 2] = (byte) (num12 >> 0x10);
                    buffer2[(num10 - 0x38000) + 3] = (byte) (num12 >> 0x18);
                    num10 += 4;
                    str2 = wgTools.SetObjToStr(this.dv[k][2]).Trim();
                    byte[] bytes = Encoding.GetEncoding(name).GetBytes(str2);
                    num12 = (num12 + bytes.Length) + 1;
                }
                num10 = 0x38400;
                for (int m = 0; m <= (this.dv.Count - 1); m++)
                {
                    str2 = wgTools.SetObjToStr(this.dv[m][2]).Trim();
                    byte[] buffer4 = Encoding.GetEncoding(name).GetBytes(str2);
                    for (int n = 0; n < buffer4.Length; n++)
                    {
                        buffer2[(num10 - 0x38000) + n] = buffer4[n];
                    }
                    num10 = (num10 + buffer4.Length) + 1;
                }
                wgUdpComm comm = null;
                try
                {
                    IPAddress address;
                    WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH();
                    tssi_flash.type = 0x21;
                    tssi_flash.code = 0x30;
                    tssi_flash.iDevSnFrom = 0;
                    tssi_flash.iDevSnTo = (uint) num;
                    tssi_flash.iCallReturn = 0;
                    tssi_flash.ucData = new byte[0x400];
                    if (IPAddress.TryParse(pCIPAddr, out address))
                    {
                        comm = new wgUdpComm(IPAddress.Parse(pCIPAddr));
                    }
                    else
                    {
                        comm = new wgUdpComm();
                    }
                    Thread.Sleep(300);
                    tssi_flash.iStartFlashAddr = 0x7f2000;
                    tssi_flash.iEndFlashAddr = 0x7f2fff;
                    for (int num17 = 0; num17 < 0x400; num17++)
                    {
                        tssi_flash.ucData[num17] = 0xff;
                    }
                    byte[] recv = null;
                    while (tssi_flash.iStartFlashAddr <= tssi_flash.iEndFlashAddr)
                    {
                        for (int num18 = 0; num18 < 0x400; num18++)
                        {
                            tssi_flash.ucData[num18] = buffer2[(int) ((IntPtr) ((tssi_flash.iStartFlashAddr - 0x7f2000) + num18))];
                        }
                        comm.udp_get_notries(tssi_flash.ToBytes(comm.udpPort), 1000, tssi_flash.xid, null, 0xea60, ref recv);
                        if (recv == null)
                        {
                            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                        }
                        tssi_flash.iStartFlashAddr += 0x400;
                    }
                    comm.Close();
                    wgAppConfig.wgLog(this.btnIPAndWebConfigure.Text + "  SN=" + num.ToString() + "  OtherLanguage");
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (comm != null)
                    {
                        comm.Dispose();
                    }
                }
            }
        }

        public bool isExisted(string sn, string ip)
        {
            bool flag = false;
            try
            {
                if (this.dgvFoundControllers.Rows.Count <= 0)
                {
                    return flag;
                }
                for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
                {
                    if ((sn == this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString()) && (ip == this.dgvFoundControllers.Rows[i].Cells[7].Value.ToString()))
                    {
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
            return flag;
        }

        private void restoreAllSwipesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectController);
            }
            else
            {
                using (icController controller = new icController())
                {
                    DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
                    controller.ControllerSN = int.Parse(row.Cells["f_ControllerSN"].Value.ToString());
                    controller.RestoreAllSwipeInTheControllersIP();
                    wgAppConfig.wgLog(sender.ToString() + "  SN=" + controller.ControllerSN);
                    XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[] { controller.ControllerSN, sender.ToString(), CommonStr.strSuccessfully, CommonStr.strRebootController }));
                }
            }
        }

        private void restoreDefaultIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnDefault_Click(sender, e);
        }

        private void restoreDefaultParamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectController);
            }
            else
            {
                using (icController controller = new icController())
                {
                    DataGridViewRow row = this.dgvFoundControllers.SelectedRows[0];
                    controller.ControllerSN = int.Parse(row.Cells["f_ControllerSN"].Value.ToString());
                    wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
                    mjconf.RestoreDefault();
                    controller.UpdateConfigureIP(mjconf);
                    wgAppConfig.wgLog(sender.ToString() + "  SN=" + controller.ControllerSN);
                    XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[] { controller.ControllerSN, sender.ToString(), CommonStr.strSuccessfully, CommonStr.strRebootController }));
                }
            }
        }

        private void search100FromTheSpecialSNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uint result = 0;
            uint num2 = 0;
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as ToolStripItem).Text;
                name.label1.Text = CommonStr.strControllerSN;
                if (((name.ShowDialog(this) != DialogResult.OK) || !uint.TryParse(name.strNewName, out result)) || (wgMjController.GetControllerType((int) result) == 0))
                {
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            this.btnConfigure.Enabled = false;
            this.btnSearch.Enabled = false;
            Thread.Sleep(100);
            this.Refresh();
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iCallReturn = 0;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            num2 = (result + 100) - 1;
            if (sender == this.searchSpecialSNToolStripMenuItem)
            {
                num2 = (result + 1) - 1;
            }
            this.wgudp = null;
            while (result <= num2)
            {
                if (((num2 - result) % 5) == 0)
                {
                    this.lblSearchNow.Text = result.ToString();
                    this.toolStripStatusLabel2.Text = result.ToString();
                    this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
                    this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
                    this.Refresh();
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;
                }
                packet.iDevSnTo = result;
                result++;
                if (WGPacket.bCommP)
                {
                    WGPacket.bCommP = false;
                    string commPStr = wgTools.CommPStr;
                    wgTools.CommPStr = "";
                    foreach (NetworkInterface interface2 in allNetworkInterfaces)
                    {
                        UnicastIPAddressInformationCollection unicastAddresses = interface2.GetIPProperties().UnicastAddresses;
                        if (unicastAddresses.Count > 0)
                        {
                            Console.WriteLine(interface2.Description);
                            foreach (UnicastIPAddressInformation information in unicastAddresses)
                            {
                                if (!information.Address.IsIPv6LinkLocal && (information.Address.ToString() != "127.0.0.1"))
                                {
                                    Console.WriteLine("  IP ............................. : {0}", information.Address.ToString());
                                    if (this.wgudp == null)
                                    {
                                        this.wgudp = new wgUdpComm(information.Address);
                                        Thread.Sleep(300);
                                    }
                                    else if (this.wgudp.localIP.ToString() != information.Address.ToString())
                                    {
                                        this.wgudp = new wgUdpComm(information.Address);
                                        Thread.Sleep(300);
                                    }
                                    byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
                                    if (cmd == null)
                                    {
                                        return;
                                    }
                                    byte[] recv = null;
                                    this.wgudp.udp_get(cmd, 300, 0, null, 0xea60, ref recv);
                                    if ((recv != null) && !this.isExisted(packet.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
                                    {
                                        this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { recv, information.Address.ToString() });
                                        long ticks = DateTime.Now.Ticks;
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                    wgTools.CommPStr = commPStr;
                    WGPacket.bCommP = true;
                }
                foreach (NetworkInterface interface3 in allNetworkInterfaces)
                {
                    UnicastIPAddressInformationCollection informations2 = interface3.GetIPProperties().UnicastAddresses;
                    if (informations2.Count > 0)
                    {
                        Console.WriteLine(interface3.Description);
                        foreach (UnicastIPAddressInformation information2 in informations2)
                        {
                            if (!information2.Address.IsIPv6LinkLocal && (information2.Address.ToString() != "127.0.0.1"))
                            {
                                Console.WriteLine("  IP ............................. : {0}", information2.Address.ToString());
                                if (this.wgudp == null)
                                {
                                    this.wgudp = new wgUdpComm(information2.Address);
                                    Thread.Sleep(300);
                                }
                                else if (this.wgudp.localIP.ToString() != information2.Address.ToString())
                                {
                                    this.wgudp = new wgUdpComm(information2.Address);
                                    Thread.Sleep(300);
                                }
                                byte[] buffer3 = packet.ToBytes(this.wgudp.udpPort);
                                if (buffer3 == null)
                                {
                                    return;
                                }
                                byte[] buffer4 = null;
                                this.wgudp.udp_get(buffer3, 300, uint.MaxValue, null, 0xea60, ref buffer4);
                                if ((buffer4 != null) && !this.isExisted(packet.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
                                {
                                    this.dgvFoundControllers.Invoke(new AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[] { buffer4, information2.Address.ToString() });
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            this.btnSearch.Enabled = true;
            if (this.dgvFoundControllers.Rows.Count > 0)
            {
                this.btnConfigure.Enabled = true;
            }
            this.lblSearchNow.Text = num2.ToString();
            this.toolStripStatusLabel2.Text = num2.ToString();
            this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
            this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
            Cursor.Current = Cursors.Default;
        }

        public delegate void AddTolstDiscoveredDevices(object o, object pcIP);

        public delegate void AsyncCallback(IAsyncResult ar);
    }
}

