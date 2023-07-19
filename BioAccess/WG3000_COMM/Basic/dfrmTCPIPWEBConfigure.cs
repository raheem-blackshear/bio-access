namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmTCPIPWEBConfigure : frmBioAccess
    {
        public string strGateway = "";
        public string strIP = "";
        public string strMac = "";
        public string strMask = "";
        public string strPCAddress = "";
        public string strSearchedIP = "";
        public string strSearchedMask = "";
        public string strSN = "";
        public string strTCPPort = "";
        private const int webStringCount = 0x9a;

        public dfrmTCPIPWEBConfigure()
        {
            this.InitializeComponent();
        }

        private void btnDownloadUsers_Click(object sender, EventArgs e)
        {
            try
            {
                int controllerSN = int.Parse(this.txtf_ControllerSN.Text);
                Cursor.Current = Cursors.WaitCursor;
                using (wgMjControllerPrivilege privilege = new wgMjControllerPrivilege())
                {
                    privilege.AllowDownload();
                    if (this.dtPrivilege != null)
                    {
                        this.dtPrivilege.Rows.Clear();
                        this.dtPrivilege.Dispose();
                        this.dtPrivilege = null;
                        GC.Collect();
                    }
                    if (this.dtPrivilege == null)
                    {
                        this.dtPrivilege = new DataTable(wgAppConfig.dbWEBUserName);
                        this.dtPrivilege.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                        this.dtPrivilege.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
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
                        this.dtPrivilege.Columns.Add("f_IsDeleted", System.Type.GetType("System.UInt32"));
                    }
                    DataTable tbFpTempl = null;
                    if (privilege.DownloadIP(controllerSN, null, 0xea60, "INCLUDEDELETED", ref this.dtPrivilege, ref tbFpTempl, this.strPCAddress) > 0)
                    {
                        if (this.dtPrivilege.Rows.Count >= 0)
                        {
                            this.dtPrivilege.Columns.Remove("f_BeginYMD");
                            this.dtPrivilege.Columns.Remove("f_EndYMD");
                            this.dtPrivilege.Columns.Remove("f_PIN");
                            this.dtPrivilege.Columns.Remove("f_ControlSegID1");
                            this.dtPrivilege.Columns.Remove("f_ControlSegID2");
                            this.dtPrivilege.Columns.Remove("f_ControlSegID3");
                            this.dtPrivilege.Columns.Remove("f_ControlSegID4");
                            //this.dtPrivilege.Columns.Remove("f_ControlSegID5");
                            this.dtPrivilege.AcceptChanges();
                            this.dv = new DataView(this.dtPrivilege);
                            this.dv.RowFilter = "f_IsDeleted = 1";
                            if (this.dv.Count > 0)
                            {
                                for (int i = this.dv.Count - 1; i >= 0; i--)
                                {
                                    this.dv.Delete(i);
                                }
                            }
                            this.dtPrivilege.AcceptChanges();
                            this.dtPrivilege.Columns.Remove("f_IsDeleted");
                            this.dtPrivilege.AcceptChanges();
                            string path = wgAppConfig.Path4Doc() + wgAppConfig.dbWEBUserName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                            using (StringWriter writer = new StringWriter())
                            {
                                this.dtPrivilege.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                                using (StreamWriter writer2 = new StreamWriter(path, false))
                                {
                                    writer2.Write(writer.ToString());
                                }
                            }
                            XMessageBox.Show((sender as Button).Text + "\r\n\r\n" + path);
                        }
                        else
                        {
                            XMessageBox.Show((sender as Button).Text + " " + controllerSN.ToString() + " " + CommonStr.strFailed);
                        }
                    }
                    else
                    {
                        XMessageBox.Show((sender as Button).Text + " " + controllerSN.ToString() + " " + CommonStr.strFailed);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnEditUsers_Click(object sender, EventArgs e)
        {
            using (dfrmEditUserFile file = new dfrmEditUserFile())
            {
                file.ShowDialog();
            }
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
            if (this.chkEditIP.Checked)
            {
                if (string.IsNullOrEmpty(this.txtf_IP.Text))
                {
                    XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", "");
                if (!this.isIPAddress(this.txtf_IP.Text))
                {
                    XMessageBox.Show(this, this.txtf_IP.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.txtf_mask.Text = this.txtf_mask.Text.Replace(" ", "");
                if (!this.isIPAddress(this.txtf_mask.Text))
                {
                    XMessageBox.Show(this, this.txtf_mask.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.txtf_gateway.Text = this.txtf_gateway.Text.Replace(" ", "");
                if (!string.IsNullOrEmpty(this.txtf_gateway.Text) && !this.isIPAddress(this.txtf_gateway.Text))
                {
                    XMessageBox.Show(this, this.txtf_gateway.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (this.chkUpdateWebSet.Checked && ((this.nudHttpPort.Value == 60000M) || (this.nudHttpPort.Value == this.nudPort.Value)))
            {
                XMessageBox.Show(this, this.lblHttpPort.Text + "  " + CommonStr.strHttpWEBWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (this.chkUpdateWebSet.Checked && (this.cboLanguage.SelectedIndex == 2))
                {
                    if (string.IsNullOrEmpty(this.txtSelectedFileName.Text))
                    {
                        XMessageBox.Show(CommonStr.strTranslateFileSelect);
                        return;
                    }
                    if (this.dtWebString == null)
                    {
                        bool flag = false;
                        string text = this.txtSelectedFileName.Text;
                        if (System.IO.File.Exists(text))
                        {
                            try
                            {
                                this.tb1 = new DataTable();
                                this.tb1.TableName = "WEBString";
                                this.tb1.Columns.Add("f_NO");
                                this.tb1.Columns.Add("f_Name");
                                this.tb1.Columns.Add("f_Value");
                                this.tb1.Columns.Add("f_CName");
                                this.tb1.ReadXml(text);
                                this.tb1.AcceptChanges();
                                if (this.tb1.Rows.Count == 0x9a)
                                {
                                    bool flag2 = true;
                                    for (int i = 0; i < this.tb1.Rows.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(this.tb1.Rows[i]["f_Value"].ToString()))
                                        {
                                            flag2 = false;
                                            XMessageBox.Show(string.Format("{0} {1}", this.tb1.Rows[i]["f_NO"].ToString(), CommonStr.strTranslateValueInvavid));
                                            return;
                                        }
                                    }
                                    if (flag2)
                                    {
                                        flag = true;
                                        this.dtWebString = this.tb1.Copy();
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        if (!flag)
                        {
                            XMessageBox.Show(CommonStr.strTranslateFileInvalid);
                            return;
                        }
                    }
                }
                if (this.chkAutoUploadWEBUsers.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtUsersFile.Text))
                    {
                        XMessageBox.Show(CommonStr.strUserFileSelect);
                        return;
                    }
                    if (!System.IO.File.Exists(this.txtUsersFile.Text))
                    {
                        XMessageBox.Show(CommonStr.strUserFileSelect);
                        return;
                    }
                }
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

        private void btnOption_Click(object sender, EventArgs e)
        {
            this.btnOption.Enabled = false;
            this.lblPort.Visible = true;
            this.nudPort.Visible = true;
        }

        private void btnOptionWEB_Click(object sender, EventArgs e)
        {
            this.btnOptionWEB.Enabled = false;
            this.lblHttpPort.Visible = true;
            this.nudHttpPort.Visible = true;
            this.label6.Visible = true;
            this.cboDateFormat.Visible = true;
        }

        private void btnOtherLanguage_Click(object sender, EventArgs e)
        {
            using (dfrmTranslate translate = new dfrmTranslate())
            {
                translate.ShowDialog();
            }
        }

        private void btnRestoreNameAndPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (XMessageBox.Show(CommonStr.strRebootController4Restore, (sender as Button).Text, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (icController controller = new icController())
                    {
                        controller.ControllerSN = int.Parse(this.txtf_ControllerSN.Text);
                        controller.UpdateFRamIP(0x10000001, 0);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName;
                this.openFileDialog1.Filter = " (*.xml)|*.xml| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                this.openFileDialog1.Title = (sender as Button).Text;
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                }
                else
                {
                    return;
                }
                bool flag = false;
                string path = fileName;
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        this.tb2 = new DataTable();
                        this.tb2.TableName = "WEBString";
                        this.tb2.Columns.Add("f_NO");
                        this.tb2.Columns.Add("f_Name");
                        this.tb2.Columns.Add("f_Value");
                        this.tb2.Columns.Add("f_CName");
                        this.tb2.ReadXml(path);
                        this.tb2.AcceptChanges();
                        if (this.tb2.Rows.Count == 0x9a)
                        {
                            bool flag2 = true;
                            for (int i = 0; i < this.tb2.Rows.Count; i++)
                            {
                                if (string.IsNullOrEmpty(this.tb2.Rows[i]["f_Value"].ToString()))
                                {
                                    flag2 = false;
                                    XMessageBox.Show(string.Format(CommonStr.strTranslateValueInvavid, this.tb2.Rows[i]["f_NO"].ToString()));
                                    return;
                                }
                            }
                            if (flag2)
                            {
                                flag = true;
                                this.dtWebString = this.tb2.Copy();
                            }
                        }
                    }
                    if (!flag)
                    {
                        XMessageBox.Show(CommonStr.strTranslateFileInvalid);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                this.txtSelectedFileName.Text = fileName;
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
        }

        private void btnSelectUserFile_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName;
                this.openFileDialog1.Filter = " (*.xml)|*.xml| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                this.openFileDialog1.Title = (sender as Button).Text;
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                }
                else
                {
                    return;
                }
                bool flag = false;
                string path = fileName;
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        this.tb = new DataTable();
                        this.tb.TableName = wgAppConfig.dbWEBUserName;
						this.tb.Columns.Add("f_ConsumerNO");
                        this.tb.Columns.Add("f_CardNO");
                        this.tb.Columns.Add("f_ConsumerName");
                        this.tb.ReadXml(path);
                        this.tb.AcceptChanges();
                        flag = true;
                        this.dtUsers = this.tb.Copy();
                    }
                    if (!flag)
                    {
                        XMessageBox.Show(CommonStr.strUsersFileInvalid);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                this.txtUsersFile.Text = fileName;
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
        }

        private void btnTryWEB_Click(object sender, EventArgs e)
        {
            this.tryWEB_ByARP();
        }

        private void btnuploadUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUsersFile.Text))
            {
                XMessageBox.Show(CommonStr.strUserFileSelect);
            }
            else if (!System.IO.File.Exists(this.txtUsersFile.Text))
            {
                XMessageBox.Show(CommonStr.strUserFileSelect);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                string lang = "utf-8";
                if (this.ipweb_uploadusers(this.txtUsersFile.Text, int.Parse(this.txtf_ControllerSN.Text), lang) > 0)
                {
                    Cursor.Current = Cursors.Default;
                    wgAppConfig.wgLog((sender as Button).Text + "  SN=" + this.strSN);
                    XMessageBox.Show((sender as Button).Text + "  SN=" + this.strSN + " " + CommonStr.strSuccessfully);
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnSelectFile.Visible = this.cboLanguage.SelectedIndex == 2;
            this.btnOtherLanguage.Visible = this.cboLanguage.SelectedIndex == 2;
            this.txtSelectedFileName.Visible = this.cboLanguage.SelectedIndex == 2;
        }

        private void chkEditIP_CheckedChanged(object sender, EventArgs e)
        {
            this.grpIP.Enabled = this.chkEditIP.Checked;
        }

        private void chkUpdateSpecialCard_CheckedChanged(object sender, EventArgs e)
        {
            this.grpSpecialCards.Visible = this.chkUpdateSpecialCard.Checked;
            this.grpSpecialCards.Enabled = this.chkUpdateSpecialCard.Checked;
        }

        private void chkUpdateSuperCard_CheckedChanged(object sender, EventArgs e)
        {
            this.grpSuperCards.Enabled = this.chkUpdateSuperCard.Checked;
        }

        private void chkUpdateWebSet_CheckedChanged(object sender, EventArgs e)
        {
            this.grpWEBEnabled.Enabled = this.chkUpdateWebSet.Checked;
            this.grpWEB.Enabled = this.grpWEBEnabled.Enabled && this.optWEBEnabled.Checked;
        }

        private bool CommunicteSocketTcpIsValid(string ipdest, int port)
        {
            Socket socket = null;
            bool flag = false;
            try
            {
                IPAddress address = IPAddress.Parse(ipdest);
                int num = port;
                IPEndPoint remoteEP = new IPEndPoint(address, num);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 0x3e8;
                socket.ReceiveTimeout = 0x3e8;
                socket.Connect(remoteEP);
                if (socket.Connected)
                {
                    flag = true;
                }
                socket.Close();
                socket = null;
            }
            catch
            {
                if (socket != null)
                {
                    socket.Close();
                }
                return flag;
            }
            return flag;
        }

        private void dfrmTCPIPConfigure_Load(object sender, EventArgs e)
        {
            this.txtSuperCard1.Mask = "9999999999";
            this.txtSuperCard2.Mask = "9999999999";
            this.txtf_ControllerSN.Text = this.strSN;
            this.txtf_MACAddr.Text = this.strMac;
            this.txtf_IP.Text = this.strIP;
            this.txtf_mask.Text = this.strMask;
            this.txtf_gateway.Text = this.strGateway;
            if (string.IsNullOrEmpty(this.strTCPPort))
            {
                this.strTCPPort = 0xea60.ToString();
            }
            else if ((int.Parse(this.strTCPPort) < this.nudPort.Minimum) || (int.Parse(this.strTCPPort) >= 0xffff))
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

        private void dfrmTCPIPWEBConfigure_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        this.funCtrlShiftQ();
                    }
                }
            }
            catch
            {
            }
        }

        private void funCtrlShiftQ()
        {
            if (this.btnRestoreNameAndPassword.Visible)
            {
                this.chkUpdateSpecialCard.Visible = true;
            }
            this.chkAutoUploadWEBUsers.Visible = true;
            this.btnRestoreNameAndPassword.Visible = true;
        }

        private void getMaskGateway(string pcIPAddress, ref string mask, ref string gateway)
        {
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties iPProperties = interface2.GetIPProperties();
                UnicastIPAddressInformationCollection unicastAddresses = iPProperties.UnicastAddresses;
                if (unicastAddresses.Count > 0)
                {
                    Console.WriteLine(interface2.Description);
                    foreach (UnicastIPAddressInformation information in unicastAddresses)
                    {
                        if ((!information.Address.IsIPv6LinkLocal && (information.Address.ToString() != "127.0.0.1")) && (information.Address.ToString() == pcIPAddress))
                        {
                            mask = information.IPv4Mask.ToString();
                            if (iPProperties.GatewayAddresses.Count > 0)
                            {
                                gateway = iPProperties.GatewayAddresses[0].Address.ToString();
                            }
                            break;
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        private int IPLng(IPAddress ip)
        {
            byte[] addressBytes = new byte[4];
            addressBytes = ip.GetAddressBytes();
            return ((((addressBytes[3] << 0x18) + (addressBytes[2] << 0x10)) + (addressBytes[1] << 8)) + addressBytes[0]);
        }

        private int ipweb_uploadusers(string userFile, int controllerSN, string lang)
        {
            string path = userFile;
            if (System.IO.File.Exists(path))
            {
                this.tb3 = new DataTable();
                this.tb3.TableName = wgAppConfig.dbWEBUserName;
				this.tb3.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                this.tb3.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
                this.tb3.Columns.Add("f_ConsumerName");
                this.tb3.ReadXml(path);
                this.tb3.AcceptChanges();
                this.dv = new DataView(this.tb3);
                this.dv.Sort = "f_ConsumerNO ASC";
                int num = controllerSN;
                try
                {
                    using (wgMjControllerPrivilege privilege = new wgMjControllerPrivilege())
                    {
                        privilege.AllowUpload();
                        if (this.dtPrivilege != null)
                        {
                            this.dtPrivilege.Rows.Clear();
                            this.dtPrivilege.Dispose();
                            this.dtPrivilege = null;
                            GC.Collect();
                        }
                        if (tbFpTempl != null)
                        {
                            tbFpTempl.Clear();
                            tbFpTempl.Dispose();
                            tbFpTempl = null;
                            GC.Collect();
                        }
                        if (tbFaceTempl != null)
                        {
                            tbFaceTempl.Clear();
                            tbFaceTempl.Dispose();
                            tbFaceTempl = null;
                            GC.Collect();
                        }
                        this.dtPrivilege = new DataTable("Privilege");
                        this.dtPrivilege.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
						this.dtPrivilege.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
                        this.dtPrivilege.Columns.Add("f_ConsumerName", System.Type.GetType("System.String"));
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

                        tbFpTempl = new DataTable("FpTempl");
                        tbFpTempl.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                        tbFpTempl.Columns.Add("f_Finger", System.Type.GetType("System.Int32"));
                        tbFpTempl.Columns.Add("f_Templ", System.Type.GetType("System.Byte[]"));
                        tbFpTempl.Columns.Add("f_Duress", System.Type.GetType("System.Int32"));

                        tbFaceTempl = new DataTable("FaceTempl");
                        tbFaceTempl.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                        tbFaceTempl.Columns.Add("f_Templ", System.Type.GetType("System.Byte[]"));

                        uint num2 = 0;
                        for (int i = 0; i < this.dv.Count; i++)
                        {
                            DataRow row = this.dtPrivilege.NewRow();
                            row["f_ConsumerNO"] = (uint) this.dv[i]["f_ConsumerNO"];
							row["f_CardNO"] = (uint) this.dv[i]["f_CardNO"];
                            row["f_ConsumerName"] = this.dv[i]["f_ConsumerName"];
                            row["f_BeginYMD"] = DateTime.Parse("2011-1-1");
                            row["f_EndYMD"] = DateTime.Parse("2029-12-31");
                            row["f_PIN"] = 0;
                            row["f_ControlSegID1"] = 1;
                            row["f_ControlSegID2"] = 1;
                            row["f_ControlSegID3"] = 1;
                            row["f_ControlSegID4"] = 1;
                            //row["f_ControlSegID5"] = 1;
                            row["f_ConsumerName"] = this.dv[i]["f_ConsumerName"];
                            if (((uint) row["f_ConsumerNO"]) <= num2)
                            {
                                XMessageBox.Show(CommonStr.strFailed);
                                return 0;
                            }
                            num2 = (uint) row["f_ConsumerNO"];
                            this.dtPrivilege.Rows.Add(row);
                        }
                        this.dtPrivilege.AcceptChanges();
                        privilege.bAllowUploadUserName = true;
                        int ret;
                        if ((ret = privilege.UploadIP(num, null, 0xea60, "DOOR NAME", this.dtPrivilege, tbFpTempl, tbFaceTempl, this.strPCAddress)) < 0)
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
                        return 1;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            return 0;
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

        private void optWEBEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.grpWEB.Enabled = this.optWEBEnabled.Checked;
        }

        private void tryWEB_ByARP()
        {
            Cursor.Current = Cursors.WaitCursor;
            this.btnTryWEB.Enabled = false;
            icController controller = new icController();
            try
            {
                string ipdest = "";
                controller.ControllerSN = int.Parse(this.strSN);
                if (controller.GetControllerRunInformationIP(this.strPCAddress) <= 0)
                {
                    XMessageBox.Show(string.Format("{0} {1} {2}", CommonStr.strController, this.strSN, CommonStr.strCommFail));
                }
                else
                {
                    wgMjControllerConfigure controlConfigure = null;
                    controller.GetConfigureIP(ref controlConfigure);
                    if (controlConfigure != null)
                    {
                        ipdest = controlConfigure.ip.ToString();
                    }
                    bool flag = false;
                    if (((ipdest != "192.168.0.0") && (ipdest != "192.168.168.0")) && ((ipdest != "255.255.255.255") && (ipdest != "")))
                    {
                        controller.IP = ipdest;
                        if (controller.GetControllerRunInformationIP(this.strPCAddress) > 0)
                        {
                            flag = true;
                        }
                        controller.IP = "";
                    }
                    if (!flag)
                    {
                        IPAddress ip = IPAddress.Parse(this.strPCAddress);
                        byte[] address = new byte[4];
                        address = ip.GetAddressBytes();
                        if (address[3] != 0x7b)
                        {
                            address[3] = 0x7b;
                            ip = new IPAddress(address);
                        }
                        int num = -1;
                        byte[] pMacAddr = new byte[6];
                        uint length = (uint) pMacAddr.Length;
                        num = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ip), this.IPLng(IPAddress.Parse(this.strPCAddress)), pMacAddr, ref length);
                        if (num == 0)
                        {
                            address[3] = (byte) (address[3] + 1);
                            while (address[3] != 0x7b)
                            {
                                if ((address[3] == 0) || (address[3] == 0xff))
                                {
                                    address[3] = (byte) (address[3] + 1);
                                }
                                else
                                {
                                    ip = new IPAddress(address);
                                    length = (uint) pMacAddr.Length;
                                    num = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ip), this.IPLng(IPAddress.Parse(this.strPCAddress)), pMacAddr, ref length);
                                    if (num != 0)
                                    {
                                        break;
                                    }
                                    address[3] = (byte) (address[3] + 1);
                                }
                            }
                        }
                        if (num != 0)
                        {
                            byte[] data = new byte[0x480];
                            for (int i = 0; i < data.Length; i++)
                            {
                                data[i] = 0;
                            }
                            int num5 = 80;
                            int num6 = 0x3000;
                            if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
                            {
                                num6 = 0x2000;
                            }
                            int index = 100;
                            data[index] = (byte) (num6 & 0xff);
                            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                            index++;
                            data[index] = (byte) (num6 >> 8);
                            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                            index++;
                            data[index] = (byte) (num6 >> 0x10);
                            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                            index++;
                            data[index] = (byte) (num6 >> 0x18);
                            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                            index = 0x60;
                            data[index] = (byte) (num5 & 0xff);
                            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                            index++;
                            data[index] = (byte) (num5 >> 8);
                            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                            controller.UpdateConfigureCPUSuperIP(data, "", this.strPCAddress);
                            string mask = "";
                            string gateway = "";
                            this.getMaskGateway(this.strPCAddress, ref mask, ref gateway);
                            controller.NetIPConfigure(controller.ControllerSN.ToString(), controlConfigure.MACAddr, ip.ToString(), mask, gateway, 0xea60.ToString(), this.strPCAddress);
                            Thread.Sleep(0x7d0);
                        }
                        int num7 = 3;
                        length = (uint) pMacAddr.Length;
                        num = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ip), this.IPLng(IPAddress.Parse(this.strPCAddress)), pMacAddr, ref length);
                        while ((num != 0) && (num7-- > 0))
                        {
                            Thread.Sleep(500);
                            length = (uint) pMacAddr.Length;
                            num = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ip), this.IPLng(IPAddress.Parse(this.strPCAddress)), pMacAddr, ref length);
                        }
                        if (num == 0)
                        {
                            ipdest = ip.ToString();
                            flag = true;
                        }
                    }
                    if (flag && !this.CommunicteSocketTcpIsValid(ipdest, 80))
                    {
                        byte[] buffer4 = new byte[0x480];
                        for (int j = 0; j < buffer4.Length; j++)
                        {
                            buffer4[j] = 0;
                        }
                        int num10 = 80;
                        int num11 = 0x3000;
                        if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
                        {
                            num11 = 0x2000;
                        }
                        int num9 = 100;
                        buffer4[num9] = (byte) (num11 & 0xff);
                        buffer4[0x400 + (num9 >> 3)] = (byte) (buffer4[0x400 + (num9 >> 3)] | ((byte) (((int) 1) << (num9 & 7))));
                        num9++;
                        buffer4[num9] = (byte) (num11 >> 8);
                        buffer4[0x400 + (num9 >> 3)] = (byte) (buffer4[0x400 + (num9 >> 3)] | ((byte) (((int) 1) << (num9 & 7))));
                        num9++;
                        buffer4[num9] = (byte) (num11 >> 0x10);
                        buffer4[0x400 + (num9 >> 3)] = (byte) (buffer4[0x400 + (num9 >> 3)] | ((byte) (((int) 1) << (num9 & 7))));
                        num9++;
                        buffer4[num9] = (byte) (num11 >> 0x18);
                        buffer4[0x400 + (num9 >> 3)] = (byte) (buffer4[0x400 + (num9 >> 3)] | ((byte) (((int) 1) << (num9 & 7))));
                        num9 = 0x60;
                        buffer4[num9] = (byte) (num10 & 0xff);
                        buffer4[0x400 + (num9 >> 3)] = (byte) (buffer4[0x400 + (num9 >> 3)] | ((byte) (((int) 1) << (num9 & 7))));
                        num9++;
                        buffer4[num9] = (byte) (num10 >> 8);
                        buffer4[0x400 + (num9 >> 3)] = (byte) (buffer4[0x400 + (num9 >> 3)] | ((byte) (((int) 1) << (num9 & 7))));
                        controller.UpdateConfigureCPUSuperIP(buffer4, "", this.strPCAddress);
                        controller.RebootControllerIP(this.strPCAddress);
                        Thread.Sleep(0x7d0);
                    }
                    if (flag)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "HTTP://" + ipdest;
                        startInfo.UseShellExecute = true;
                        Process.Start(startInfo);
                    }
                    else
                    {
                        XMessageBox.Show(string.Format("{0} {1} {2}", CommonStr.strController, this.strSN, CommonStr.strFailed));
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                if (controller != null)
                {
                    controller.Dispose();
                }
                Cursor.Current = Cursors.Default;
                this.btnTryWEB.Enabled = true;
            }
        }

        private void txtSuperCard1_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtSuperCard1);
        }

        private void txtSuperCard1_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtSuperCard1);
        }

        private void txtSuperCard2_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtSuperCard2);
        }

        private void txtSuperCard2_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtSuperCard2);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = btnCancel.DialogResult;
            Close();
        }
    }
}

