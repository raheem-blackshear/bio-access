namespace WG3000_COMM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Media;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;

    public partial class frmProductFormat : frmBioAccess
    {
        private bool bStopRemoteEvalator;
        private string lastControllerInfo = "";
        private long lastErrController;

        public frmProductFormat()
        {
            this.InitializeComponent();
        }

        private void autoFormatLogEntry(icController control)
        {
            this.txtRunInfo.AppendText("格式化: " + control.ControllerSN.ToString() + "\r\n");
            wgAppConfig.wgLogWithoutDB("格式化: " + control.ControllerSN.ToString(), EventLogEntryType.Warning, null);
            wgLogProduct("格式化: " + control.ControllerSN.ToString(), "BioAccessFormat");
            this.label12.Visible = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            icController controller = new icController();
            try
            {
                Thread.Sleep(300);
            Label_0025:
                if (!this.checkBox1.Checked)
                {
                    try
                    {
                        controller.ControllerSN = -1;
                        controller.runinfo.Clear();
                        controller.GetControllerRunInformationIPNoTries();
                        base.Invoke(new dispDoorStatusByIPComm(this.dispDoorStatusByIPCommEntry), new object[] { controller });
                        if (((this.chkAutoFormat.Checked && (controller.runinfo.wgcticks > 0)) && ((Math.Abs(DateTime.Now.Subtract(controller.runinfo.dtNow).TotalMinutes) < 5.0) && (controller.runinfo.reservedBytes[0] == 0))) && (controller.runinfo.appError == 0))
                        {
                            controller.ControllerSN = (int) controller.runinfo.CurrentControllerSN;
                            if (this.lastErrController == controller.ControllerSN)
                            {
                                goto Label_0025;
                            }
                            int num = 0;
                            int num2 = 0;
                            int num3 = 0;
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
                            if (num != num2)
                            {
                                if (this.lastErrController != controller.ControllerSN)
                                {
                                    this.lastErrController = controller.ControllerSN;
                                    string str = string.Format("SN{3} 有故障: 通信丢包\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, controller.ControllerSN }) + "\r\n";
                                    base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str });
                                }
                            }
                            else
                            {
                                wgUdpComm.triesTotal = 0L;
                                wgTools.WriteLine("control.Test1024 Start");
                                int page = 0;
                                string str2 = "";
                                if (controller.test1024Write() < 0)
                                {
                                    str2 = str2 + "大数据包写入失败\r\n";
                                }
                                int num5 = controller.test1024Read(100, ref page);
                                if (num5 < 0)
                                {
                                    str2 = str2 + "大数据包读取失败: " + num5.ToString() + "\r\n";
                                }
                                if (wgUdpComm.triesTotal > 0L)
                                {
                                    str2 = str2 + "测试中重试次数 = " + wgUdpComm.triesTotal.ToString() + "\r\n";
                                }
                                if (str2 != "")
                                {
                                    if (this.lastErrController != controller.ControllerSN)
                                    {
                                        this.lastErrController = controller.ControllerSN;
                                        string str3 = "SN" + controller.ControllerSN.ToString() + "通信有故障: : " + str2 + "\r\n";
                                        base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str3 });
                                    }
                                }
                                else
                                {
                                    string str4 = "";
                                    base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str4 });
                                    this.lastErrController = 0L;
                                    byte[] data = new byte[0x480];
                                    data[0x403] = 0xa5;
                                    data[0x402] = 0xa5;
                                    data[0x401] = 0xa5;
                                    data[0x400] = 0xa5;
                                    controller.UpdateConfigureSuperIP(data);
                                    base.Invoke(new autoFormatLog(this.autoFormatLogEntry), new object[] { controller });
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                        goto Label_03C6;
                    }
                }
                Thread.Sleep(300);
                goto Label_0025;
            }
            catch (Exception)
            {
            }
        Label_03C6:
            controller.Dispose();
        }

        private void btnAdjustTime_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(this.txtSN.Text, out num))
            {
                icController controller = new icController();
                try
                {
                    controller.ControllerSN = num;
                    controller.AdjustTimeIP(DateTime.Now, 0);
                    wgLogProduct("校准时间: " + controller.ControllerSN.ToString(), "BioAccessFormat");
                }
                catch (Exception)
                {
                }
                controller.Dispose();
            }
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            icController controller = new icController();
            try
            {
                controller.ControllerSN = -1;
                controller.runinfo.Clear();
                controller.GetControllerRunInformationIPNoTries();
                if (controller.runinfo.wgcticks > 0)
                {
                    controller.ControllerSN = (int) controller.runinfo.CurrentControllerSN;
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
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
                    if (num != num2)
                    {
                        string str = string.Format("SN{3} 有故障: 通信丢包\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, controller.ControllerSN }) + "\r\n";
                        base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str });
                    }
                    else
                    {
                        wgUdpComm.triesTotal = 0L;
                        wgTools.WriteLine("control.Test1024 Start");
                        int page = 0;
                        string str2 = "";
                        if (controller.test1024Write() < 0)
                        {
                            str2 = str2 + "大数据包写入失败\r\n";
                        }
                        int num5 = controller.test1024Read(100, ref page);
                        if (num5 < 0)
                        {
                            str2 = str2 + "大数据包读取失败: " + num5.ToString() + "\r\n";
                        }
                        if (wgUdpComm.triesTotal > 0L)
                        {
                            str2 = str2 + "测试中重试次数 = " + wgUdpComm.triesTotal.ToString() + "\r\n";
                        }
                        if (str2 != "")
                        {
                            string str3 = "SN" + controller.ControllerSN.ToString() + "通信有故障: : " + str2 + "\r\n";
                            base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str3 });
                        }
                        else
                        {
                            string str4 = string.Format("SN{3} 通信正常\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, controller.ControllerSN }) + "\r\n";
                            base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str4 });
                            str4 = "";
                            base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str4 });
                            str4 = "";
                            base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str4 });
                            str2 = "大数据包测试成功(测试100次)";
                            str4 = controller.ControllerSN.ToString() + ": " + str2 + "\r\n";
                            base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str4 });
                            this.lastErrController = 0L;
                        }
                    }
                }
                else
                {
                    string str5 = "通信不上";
                    base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str5 });
                    str5 = "";
                    base.Invoke(new pingErrLog(this.pingErrLogEntry), new object[] { str5 });
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
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.bStopRemoteEvalator = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.txtTime.Text = "";
            this.txtSN.Text = "";
            this.txtRunInfo.Text = "";
            this.lastErrController = 0L;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = -1;
                byte[] data = new byte[0x480];
                data[0x403] = 0xa5;
                data[0x402] = 0xa5;
                data[0x401] = 0xa5;
                data[0x400] = 0xa5;
                controller.UpdateConfigureSuperIP(data);
                this.label12.Visible = true;
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button57_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = -1;
                controller.GetControllerRunInformationIP();
                if (controller.runinfo.wgcticks <= 0)
                {
                    this.txtRunInfo.AppendText("???控制器未连接\r\n");
                    SystemSounds.Hand.Play();
                }
                else
                {
                    if (this.checkBox116.Checked)
                    {
                        MjUserInfo mjrc = new MjUserInfo();
                        mjrc.IsActivated = true;
                        mjrc.password = 0;
                        mjrc.ymdStart = DateTime.Parse("2010-1-1");
                        mjrc.ymdEnd = DateTime.Parse("2029-12-31");
                        mjrc.ControlSegIndexSet(1, 1);
                        mjrc.ControlSegIndexSet(2, 1);
                        mjrc.ControlSegIndexSet(3, 1);
                        mjrc.ControlSegIndexSet(4, 1);
                        icPrivilege privilege = new icPrivilege();
                        try
                        {
                            string text = this.textBox32.Text;
                            if (!string.IsNullOrEmpty(text))
                            {
                                string[] strArray = text.Split(new char[] { ',' });
                                if (strArray.Length > 0)
                                {
                                    for (int i = 0; i < strArray.Length; i++)
                                    {
                                        uint num;
                                        if (uint.TryParse(strArray[i].Trim(), NumberStyles.Integer, null, out num) && (num > 0))
                                        {
                                            mjrc.cardID = num;
                                            privilege.AddPrivilegeOfOneCardIP(-1, "", 0xea60, mjrc);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        privilege.Dispose();
                    }
                    if (this.checkBox117.Checked)
                    {
                        wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
                        mjconf.RestoreDefault();
                        mjconf.Ext_doorSet(0, 0);
                        mjconf.Ext_doorSet(1, 1);
                        mjconf.Ext_doorSet(2, 2);
                        mjconf.Ext_doorSet(3, 3);
                        mjconf.Ext_controlSet(0, 4);
                        mjconf.Ext_controlSet(1, 4);
                        mjconf.Ext_controlSet(2, 4);
                        mjconf.Ext_controlSet(3, 4);
                        mjconf.Ext_warnSignalEnabled2Set(0, 2);
                        mjconf.Ext_warnSignalEnabled2Set(1, 2);
                        mjconf.Ext_warnSignalEnabled2Set(2, 2);
                        mjconf.Ext_warnSignalEnabled2Set(3, 2);
                        controller.UpdateConfigureIP(mjconf);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                controller.Dispose();
            }
        }

        private void button70_Click(object sender, EventArgs e)
        {
            this.button70.Enabled = false;
            this.button72.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            icController controller = new icController();
            try
            {
                try
                {
                    this.bStopRemoteEvalator = false;
                    uint operatorId = 0;
                    ulong maxValue = ulong.MaxValue;
                    controller.ControllerSN = int.Parse(this.txtSN.Text);
                    int num3 = 0;
                    int num4 = 1;
                    if (sender == this.button72)
                    {
                        num3 = 20;
                        if ((((int) this.numericUpDown2.Value) >= 0x15) && (((int) this.numericUpDown2.Value) <= 40))
                        {
                            num4 = ((int) this.numericUpDown2.Value) - num3;
                        }
                    }
                    else if ((((int) this.numericUpDown1.Value) >= 1) && (((int) this.numericUpDown1.Value) <= 20))
                    {
                        num4 = ((int) this.numericUpDown1.Value) - num3;
                    }
                    int num5 = int.Parse(this.numericUpDown22.Value.ToString());
                    while (num4 <= 20)
                    {
                        if (this.bStopRemoteEvalator)
                        {
                            goto Label_01B0;
                        }
                        if (this.optNO.Checked)
                        {
                            controller.RemoteOpenFoorIP(num4 + num3, operatorId, maxValue);
                        }
                        else
                        {
                            controller.RemoteOpenFoorIP((num4 + 40) + num3, operatorId, maxValue);
                        }
                        this.lblFloor.Text = (num4 + num3).ToString();
                        Application.DoEvents();
                        for (int i = 0; i < num5; i += 300)
                        {
                            if (this.bStopRemoteEvalator)
                            {
                                break;
                            }
                            Application.DoEvents();
                            Thread.Sleep(300);
                        }
                        num4++;
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
                XMessageBox.Show(exception.ToString());
            }
            finally
            {
                controller.Dispose();
            }
        Label_01B0:
            this.button70.Enabled = true;
            this.button72.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.btnFormat.Visible = this.checkBox1.Checked;
            if (this.checkBox1.Checked)
            {
                this.btnConnected.BackColor = Color.Red;
                this.btnConnected.Visible = true;
            }
        }

        private void dispDoorStatusByIPCommEntry(icController control)
        {
            if (control.runinfo.wgcticks <= 0)
            {
                this.btnConnected.BackColor = Color.Red;
                this.btnConnected.Visible = true;
            }
            else
            {
                if (string.Compare(this.txtSN.Text, control.runinfo.CurrentControllerSN.ToString()) != 0)
                {
                    this.txtSN.Text = control.runinfo.CurrentControllerSN.ToString();
                }
                this.txtTime.Text = control.runinfo.dtNow.ToString("yyyy-MM-dd HH:mm:ss");
                bool flag = false;
                if (Math.Abs(DateTime.Now.Subtract(control.runinfo.dtNow).TotalMinutes) >= 5.0)
                {
                    this.txtTime.BackColor = Color.Red;
                    this.label8.Text = "时钟: 有问题";
                    flag = true;
                }
                else
                {
                    this.txtTime.BackColor = Color.White;
                    this.label8.Text = "时钟:";
                }
                if (control.runinfo.reservedBytes[0] > 0)
                {
                    this.btnConnected.BackColor = Color.Yellow;
                    if (this.label3.Text != control.runinfo.reservedBytes[0].ToString())
                    {
                        this.label3.Text = control.runinfo.reservedBytes[0].ToString();
                        this.lblFailDetail.Text = icDesc.failedPinDesc(control.runinfo.reservedBytes[0]);
                    }
                }
                else if (!string.IsNullOrEmpty(this.label3.Text))
                {
                    this.label3.Text = "";
                    this.lblFailDetail.Text = "";
                }
                if (control.runinfo.appError > 0)
                {
                    this.btnConnected.BackColor = Color.Yellow;
                    if (this.label4.Text != control.runinfo.appError.ToString())
                    {
                        this.label4.Text = control.runinfo.appError.ToString();
                    }
                }
                else if (!string.IsNullOrEmpty(this.label4.Text))
                {
                    this.label4.Text = "";
                }
                if (((control.runinfo.reservedBytes[0] <= 0) && (control.runinfo.appError == 0)) && !flag)
                {
                    this.btnConnected.BackColor = Color.Green;
                }
                if (control.ControllerDriverMainVer.ToString() != this.label7.Text)
                {
                    this.label7.Text = control.runinfo.driverVersion;
                }
                this.btnConnected.Visible = !this.btnConnected.Visible;
                if (this.lastControllerInfo != (control.runinfo.CurrentControllerSN.ToString() + control.runinfo.reservedBytes[0].ToString()))
                {
                    this.lastControllerInfo = control.runinfo.CurrentControllerSN.ToString() + control.runinfo.reservedBytes[0].ToString();
                    string str = "";
                    if (control.runinfo.reservedBytes[0] == 0)
                    {
                        str = str + string.Format("管脚没问题\t", new object[0]);
                    }
                    else
                    {
                        str = str + string.Format("failedPin 问题管脚号: {0}\r\n\t", control.runinfo.reservedBytes[0]) + icDesc.failedPinDesc(control.runinfo.reservedBytes[0]);
                        if ((control.runinfo.reservedBytes[1] & 240) == 0)
                        {
                            str = str + string.Format("\tfailedPinDesc 问题管脚PORT号: G{0:X}\r\n", control.runinfo.reservedBytes[1]);
                        }
                        else
                        {
                            str = str + string.Format("\tfailedPinDesc 问题管脚PORT号: {0:X2}\r\n", control.runinfo.reservedBytes[1]);
                        }
                        str = str + string.Format("\tfailedPinDiffPortType 问题管脚PORT类: {0:X2}\r\n", control.runinfo.reservedBytes[2]);
                        string str2 = "";
                        switch ((control.runinfo.reservedBytes[2] >> 4))
                        {
                            case 1:
                                str2 = "初始默认就有问题";
                                break;

                            case 2:
                                str2 = "管脚高平设置时 就有问题";
                                break;

                            case 3:
                                str2 = "管脚高平设置时 此脚 就有问题";
                                break;

                            case 4:
                                str2 = "管脚低平设置时 就有问题";
                                break;

                            case 5:
                                str2 = "管脚低平设置时 此脚 就有问题";
                                break;
                        }
                        if ((control.runinfo.reservedBytes[2] & 15) == 0)
                        {
                            str = str + string.Format("\t产生问题的另一端口PORT= PORTG\r\n", new object[0]);
                        }
                        else
                        {
                            str = str + string.Format("\t产生问题的另一端口PORT: PORT{0:X}\r\n", control.runinfo.reservedBytes[2] & 15);
                        }
                        if (str2 != "")
                        {
                            str = str + str2 + "\r\n";
                        }
                        str = str + string.Format("\tfailedPinDiff 存在不同: {0:X2}\r\n", control.runinfo.reservedBytes[3]);
                    }
                    wgLogProduct(this.lastControllerInfo + ":" + str + string.Format("\t所有数据: {0}", control.runinfo.BytesDataStr), "BioAccessProduct");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmProductFormat_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void frmProductFormat_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " V" + Application.ProductVersion;
            this.panel1.Visible = false;
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void pingErrLogEntry(string ErrInfo)
        {
            if (string.IsNullOrEmpty(ErrInfo))
            {
                this.lblCommLose.Visible = false;
            }
            else
            {
                this.btnConnected.BackColor = Color.Yellow;
                this.lblCommLose.Visible = true;
                this.txtRunInfo.AppendText(ErrInfo);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.lblTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.timer1.Enabled = true;
        }

        public static void wgLogProduct(string strMsg, string filename)
        {
            try
            {
                string str = string.Concat(new object[] { icOperator.OperatorID, ".", icOperator.OperatorName, ".", strMsg });
                str = DateTime.Now.ToString("yyyy-MM-dd H-mm-ss") + "\t" + str;
                using (StreamWriter writer = new StreamWriter(Application.StartupPath + @"\" + filename + ".log", true))
                {
                    writer.WriteLine(str);
                }
            }
            catch (Exception)
            {
            }
        }

        private delegate void autoFormatLog(icController control);

        private delegate void dispDoorStatusByIPComm(icController control);

        private delegate void pingErrLog(string ErrInfo);
    }
}

