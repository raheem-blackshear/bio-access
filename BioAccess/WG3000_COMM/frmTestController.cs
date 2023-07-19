namespace WG3000_COMM
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.Sql;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Media;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;

    partial class frmTestController : Form
    {
        private icController control4Tcp;
        private icController control4Udp;
        private wgMjControllerHolidaysList controlHolidayList;
        private wgMjControllerTaskList controlTaskList;
        private icControllerTimeSegList controlTimeSegList;
        private string defaultIP = "";
        private int defaultPORT = 0xea60;
        private int defaultSN = 0x17e6c642;
        private dfrmFind dfrmFind1;
        private dfrmNetControllerConfig dfrmNetControllerConfig1;
        private DataSet DS;
        private DataTable dtPrivilege;
        private DataTable dtPrivilege1;
        private DataView dv;
        private frmProductFormat frmProductFormat1;
        private frmTestController frmTestController1;
        private FileStream fs;
        private FileStream fsRd;
        private string lastRecordStr = "";
        private OleDbDataAdapter MyCommand;
        private OleDbConnection MyConnection;
        private string newSoundFile = "invalidCard.WAV";
        private SoundPlayer player;
        private byte[] webStringOther = new byte[0x1000];

        public frmTestController()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
            icController controller = null;
            try
            {
                controller = new icController();
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                mjconf.RestoreDefault();
                controller.UpdateConfigureIP(mjconf);
            }
            catch
            {
            }
            finally
            {
                if (controller != null)
                {
                    controller.Dispose();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.controlTaskList != null)
            {
                this.controlTaskList.Clear();
            }
            this.listBox1.Items.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (this.controlTaskList != null)
            {
                icController controller = new icController();
                try
                {
                    controller.ControllerSN = this.defaultSN;
                    controller.IP = this.defaultIP;
                    controller.PORT = this.defaultPORT;
                    if (controller.UpdateControlTaskListIP(this.controlTaskList) > 0)
                    {
                        controller.RenewControlTaskListIP();
                    }
                }
                catch (Exception)
                {
                }
                controller.Dispose();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY();
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.defaultSN,
                    (uint)MjControlTaskItem.getFlashStartAddr(defaultSN),
                    (uint)((MjControlTaskItem.getFlashStartAddr(defaultSN) + 0x400) - 1));
                byte[] cmd = tssi_flash_query.ToBytes(comm.udpPort);
                byte[] recv = null;
                if (comm.udp_get(cmd, 300, tssi_flash_query.xid, this.defaultIP, this.defaultPORT, ref recv) >= 0)
                {
                    if (recv != null)
                    {
                        string text = BitConverter.ToString(recv);
                        this.txtInfo.AppendText(text);
                        this.txtInfo.AppendText("\r\n");
                    }
                    wgTools.WriteLine(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                DateTime dateTimeNew = DateTime.Parse(string.Format("{0} {1}", this.dateTimePicker4.Value.ToString("yyyy-MM-dd"), this.dateTimePicker5.Value.ToString("HH:mm:ss")));
                controller.AdjustTimeIP(dateTimeNew, 0);
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                if (controller.AdjustTimeIP(DateTime.Now, 0) <= 0)
                {
                    this.failsound();
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                this.txtInfo.AppendText(DateTime.Now.ToString("当前电脑时间: yyyy-MM-dd HH:mm:ss.ffff\r\n"));
                controller.GetControllerRunInformationIP();
                this.txtInfo.AppendText(controller.runinfo.dtNow.ToString("控制器时间: yyyy-MM-dd HH:mm:ss\r\n") + "[星期" + controller.runinfo.weekday.ToString() + "]");
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                uint operatorId = 0;
                ulong maxValue = ulong.MaxValue;
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                if (!string.IsNullOrEmpty(this.textBox18.Text))
                {
                    operatorId = uint.Parse(this.textBox18.Text);
                }
                if (!string.IsNullOrEmpty(this.textBox19.Text))
                {
                    maxValue = ulong.Parse(this.textBox19.Text);
                }
                controller.RemoteOpenDoorIP(int.Parse(this.textBox20.Text), operatorId, maxValue);
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x23;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.txtSN.Value;
            packet.iCallReturn = 0;
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                byte[] cmd = packet.ToBytes(comm.udpPort);
                byte[] recv = null;
                comm.udp_get(cmd, 300, packet.xid, this.defaultIP, this.defaultPORT, ref recv);
                MjRegisterCardsParam param = new MjRegisterCardsParam();
                this.textBox1.AppendText(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                this.textBox1.AppendText("\r\n");
                this.Refresh();
                if (recv != null)
                {
                    string text = BitConverter.ToString(recv);
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                    param.updateParam(recv, 20);
                    text = "";
                    text = ((text + string.Format("权限起始页 = 0x{0:X8}[{0:d}]\r\n", param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) + string.Format("存储新有序权限的4K的页面 = 0x{0:X8}[{0:d}]\r\n", param.newPrivilegePage4KAddr)) + string.Format("自由的记录页面(无序的) = 0x{0:X8} [{0:d}]\r\n", param.freeNewPrivilegePageAddr) + string.Format("是否有序的(自由页面中) = {0:d}\r\n", param.bOrderInfreePrivilegePage)) + string.Format("总权限数 = {0:d}\r\n", param.totalPrivilegeCount) + string.Format("已删除的权限数 = {0:d}\r\n", param.deletedPrivilegeCount);
                    this.txtInfo.AppendText(text);
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (this.controlTimeSegList != null)
            {
                icController controller = new icController();
                try
                {
                    controller.ControllerSN = this.defaultSN;
                    controller.IP = this.defaultIP;
                    controller.PORT = this.defaultPORT;
                    controller.UpdateControlTimeSegListIP(this.controlTimeSegList);
                }
                catch (Exception)
                {
                }
                controller.Dispose();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (this.controlTimeSegList == null)
            {
                this.controlTimeSegList = new icControllerTimeSegList();
            }
            MjControlTimeSeg mjControlTimeSeg = new MjControlTimeSeg();
            mjControlTimeSeg.ymdStart = this.dateTimePicker7.Value;
            mjControlTimeSeg.ymdEnd = this.dateTimePicker6.Value;
            mjControlTimeSeg.SegIndex = byte.Parse(this.comboBox57.Text.ToString());
            mjControlTimeSeg.TotalLimittedAccess = (byte) this.numericUpDown10.Value;
            mjControlTimeSeg.LimittedMode = this.checkBox105.Checked ? 1 : 0;
            mjControlTimeSeg.nextSeg = byte.Parse(this.comboBox58.Text.ToString());
            mjControlTimeSeg.weekdayControl = 0;
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox98.Checked ? ((byte) 1) : ((byte) 0)));
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox99.Checked ? ((byte) 2) : ((byte) 0)));
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox100.Checked ? ((byte) 4) : ((byte) 0)));
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox101.Checked ? ((byte) 8) : ((byte) 0)));
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox102.Checked ? ((byte) 0x10) : ((byte) 0)));
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox103.Checked ? ((byte) 0x20) : ((byte) 0)));
            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + (this.checkBox104.Checked ? ((byte) 0x40) : ((byte) 0)));
            mjControlTimeSeg.hmsStart1 = this.dateTimePicker8.Value;
            mjControlTimeSeg.hmsEnd1 = this.dateTimePicker9.Value;
            mjControlTimeSeg.hmsStart2 = this.dateTimePicker10.Value;
            mjControlTimeSeg.hmsEnd2 = this.dateTimePicker11.Value;
            mjControlTimeSeg.hmsStart3 = this.dateTimePicker12.Value;
            mjControlTimeSeg.hmsEnd3 = this.dateTimePicker13.Value;
            mjControlTimeSeg.LimittedAccess1 = (byte) this.numericUpDown7.Value;
            mjControlTimeSeg.LimittedAccess2 = (byte) this.numericUpDown8.Value;
            mjControlTimeSeg.LimittedAccess3 = (byte) this.numericUpDown9.Value;
            mjControlTimeSeg.ControlByHoliday = this.checkBox127.Checked ? ((byte) 1) : ((byte) 0);
            this.listBox2.Items.Add(BitConverter.ToString(mjControlTimeSeg.ToBytes()));
            this.controlTimeSegList.AddItem(mjControlTimeSeg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            if (this.txtSN.Value == this.txtSN.Maximum)
            {
                packet.iDevSnTo = uint.MaxValue;
            }
            else
            {
                packet.iDevSnTo = (uint) this.txtSN.Value;
            }
            packet.iCallReturn = 0;
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                byte[] cmd = packet.ToBytes(comm.udpPort);
                byte[] recv = null;
                comm.udp_get(cmd, 300, packet.xid, this.defaultIP, this.defaultPORT, ref recv);
                this.txtInfo.AppendText(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                this.txtInfo.AppendText("\r\n");
                this.Refresh();
                if (recv != null)
                {
                    string[] strArray;
                    string text = BitConverter.ToString(recv);
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                    this.dgvControlConfure.Rows.Clear();
                    wgMjControllerConfigure configure = new wgMjControllerConfigure();
                    for (int i = 20; i < recv.Length; i++)
                    {
                        if ((i - 20) >= configure.param.Length)
                        {
                            break;
                        }
                        strArray = new string[] { (recv[i] == configure.param[i - 20]) ? " " : "X", string.Format("{0:d3}[0x{1:X2}]", i - 20, (i - 20) + 0x10), string.Format("0x{0:X2}", recv[i]), ((i - 20) < configure.param.Length) ? string.Format("0x{0:X2}", configure.param[i - 20]) : "", ((i - 20) < configure.paramDesc.Length) ? configure.paramDesc[i - 20] : "" };
                        this.dgvControlConfure.Rows.Add(strArray);
                    }
                    for (int j = 0x1f0; j < 510; j++)
                    {
                        strArray = new string[] { (recv[j + 20] == 0) ? " " : "X", string.Format("{0:d3}[0x{1:X2}]", j, j + 0x10), string.Format("0x{0:X2}", recv[j + 20]), string.Format("0x{0:X2}", 0), "--" };
                        this.dgvControlConfure.Rows.Add(strArray);
                    }
                    for (int k = 510; k < 0x200; k++)
                    {
                        strArray = new string[] { (recv[k + 20] == 0) ? " " : "X", string.Format("{0:d3}[0x{1:X2}]", k, k + 0x10), string.Format("0x{0:X2}", recv[k + 20]), string.Format("0x{0:X2}", 0), "CRC_CHECK" };
                        this.dgvControlConfure.Rows.Add(strArray);
                    }
                    for (int m = 0x200; m < 0x210; m++)
                    {
                        strArray = new string[] { (recv[m + 20] == 0xff) ? " " : "X", string.Format("{0:d3}[0x{1:X2}]", m, m + 0x10), string.Format("0x{0:X2}", recv[m + 20]), string.Format("0x{0:X2}", 0xff), "特殊卡_" + ((m >= 520) ? "2" : "1") };
                        this.dgvControlConfure.Rows.Add(strArray);
                    }
                    string[] strArray2 = new string[] { "DHCP_IP1", "DHCP_IP2", "DHCP_IP3", "DHCP_IP4", "DHCP_Mask1", "DHCP_Mask2", "DHCP_Mask3", "DHCP_Mask4", "DHCP_Gateway1", "DHCP_Gateway2", "DHCP_Gateway3", "DHCP_Gateway4" };
                    int[] numArray = new int[] { 0xc0, 0xa8, 0, 0, 0xff, 0xff, 0xff, 0, 0xc0, 0xa8, 0, 0 };
                    for (int n = 0x180; n < 0x18c; n++)
                    {
                        strArray = new string[] { (recv[n + 20] == numArray[n - 0x180]) ? " " : "X", string.Format("{0:d3}", n), string.Format("0x{0:X2}", recv[n + 20]), string.Format("0x{0:X2}", numArray[n - 0x180]), strArray2[n - 0x180] };
                        this.dgvControlConfure.Rows.Add(strArray);
                    }
                    this.tabControl1.SelectedTab = this.tabPage4;
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (this.controlTimeSegList != null)
            {
                this.controlTimeSegList.Clear();
                this.listBox2.Items.Clear();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int gate = wgTools.gate;
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                wgTools.gate = int.Parse(WGPacket.Dpt("texCPBcVbYG8tfyU+wnXCl/Ea+TvHb316FPly/KlaYQ="));
                controller.GetControllerRunInformationIP();
                wgTools.gate = gate;
                this.lblWrongProductCode.Visible = false;
                if (controller.runinfo.wgcticks > 0)
                {
                    try
                    {
                        this.txtInfo.AppendText("\r\n电脑时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "[星期" + DateTime.Now.DayOfWeek.ToString() + "]");
                        this.txtInfo.AppendText("\r\n控制器SN: " + controller.runinfo.CurrentControllerSN.ToString());
                        this.txtInfo.AppendText("\r\n驱动版本: " + controller.runinfo.driverVersion);
                        this.txtInfo.AppendText("\r\n产品类型: " + controller.ControllerProductTypeCode.ToString());
                        if (wgTools.gate != controller.ControllerProductTypeCode)
                        {
                            this.lblWrongProductCode.Visible = true;
                        }
                        this.txtInfo.AppendText("\r\n控制器的系统CTICKS: " + controller.runinfo.wgcticks.ToString() + string.Format("[/20={0:d} 秒]", controller.runinfo.wgcticks / 20));
                        this.txtInfo.AppendText("\r\n控制器的实时时钟: " + controller.runinfo.dtNow.ToString(wgTools.YMDHMSFormat) + "[星期" + controller.runinfo.weekday.ToString() + "]");
                        this.txtInfo.AppendText("\r\n控制器的上电时间: " + controller.runinfo.dtPowerOn.ToString(wgTools.YMDHMSFormat));
                        this.txtInfo.AppendText(string.Format("\r\n1号读头上的合法验证次数: {0:d}\r\n", controller.runinfo.ReaderValidSwipeGet(1)));
                        this.txtInfo.AppendText(string.Format("2号读头上的合法验证次数: {0:d}\r\n", controller.runinfo.ReaderValidSwipeGet(2)));
                        this.txtInfo.AppendText(string.Format("3号读头上的合法验证次数: {0:d}\r\n", controller.runinfo.ReaderValidSwipeGet(3)));
                        this.txtInfo.AppendText(string.Format("4号读头上的合法验证次数: {0:d}\r\n", controller.runinfo.ReaderValidSwipeGet(4)));
                        this.txtInfo.AppendText(string.Format("故障号: {0:d}\r\n", controller.runinfo.appError));
                        this.txtInfo.AppendText(string.Format("报警代号--1号门: {0:d}\r\n", controller.runinfo.WarnInfo(1)));
                        this.txtInfo.AppendText(string.Format("报警代号--2号门: {0:d}\r\n", controller.runinfo.WarnInfo(2)));
                        this.txtInfo.AppendText(string.Format("报警代号--3号门: {0:d}\r\n", controller.runinfo.WarnInfo(3)));
                        this.txtInfo.AppendText(string.Format("报警代号--4号门: {0:d}\r\n", controller.runinfo.WarnInfo(4)));
                        this.txtInfo.AppendText(string.Format("新记录数: {0:d}\r\n", controller.runinfo.getNewRecordsNum()));
                        this.txtInfo.AppendText(string.Format("刷卡终止索引: {0:d}\r\n", controller.runinfo.swipeEndIndex));
                        this.txtInfo.AppendText(string.Format("刷卡存储的起始地址: {0:d}\r\n", controller.runinfo.swipeStartAddr));
                        this.txtInfo.AppendText(string.Format("已提取记录索引: {0}\r\n", controller.runinfo.lastGetRecordIndex));
                        this.txtInfo.AppendText(string.Format("注册数量: {0}\r\n", controller.runinfo.regUserMaxCount));
                        this.txtInfo.AppendText(string.Format("已删除注册数量: {0}\r\n", controller.runinfo.regUserCount));
                        this.txtInfo.AppendText(string.Format("门内人数: {0}\r\n", controller.runinfo.totalPerson4AntibackShare));
                        this.txtInfo.AppendText(string.Format("1号按钮状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 1) > 0) ? "     开门动作" : "--"));
                        this.txtInfo.AppendText(string.Format("2号按钮状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 2) > 0) ? "     开门动作" : "--"));
                        this.txtInfo.AppendText(string.Format("3号按钮状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 4) > 0) ? "     开门动作" : "--"));
                        this.txtInfo.AppendText(string.Format("4号按钮状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 8) > 0) ? "     开门动作" : "--"));
                        this.txtInfo.AppendText(string.Format("1号门磁状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 0x10) > 0) ? "     门开" : "门关"));
                        this.txtInfo.AppendText(string.Format("2号门磁状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 0x20) > 0) ? "     门开" : "门关"));
                        this.txtInfo.AppendText(string.Format("3号门磁状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 0x40) > 0) ? "     门开" : "门关"));
                        this.txtInfo.AppendText(string.Format("4号门磁状态: {0}\r\n", ((controller.runinfo.pbdsStatus & 0x80) > 0) ? "     门开" : "门关"));
                        this.txtInfo.AppendText(string.Format("1号  继电器: {0}\r\n", ((controller.runinfo.lockStatus & 1) > 0) ? "     动作开" : "锁门"));
                        this.txtInfo.AppendText(string.Format("2号  继电器: {0}\r\n", ((controller.runinfo.lockStatus & 2) > 0) ? "     动作开" : "锁门"));
                        this.txtInfo.AppendText(string.Format("3号  继电器: {0}\r\n", ((controller.runinfo.lockStatus & 4) > 0) ? "     动作开" : "锁门"));
                        this.txtInfo.AppendText(string.Format("4号  继电器: {0}\r\n", ((controller.runinfo.lockStatus & 8) > 0) ? "     动作开" : "锁门"));
                        if (controller.runinfo.reservedBytes[0] == 0)
                        {
                            this.txtInfo.AppendText(string.Format("管脚没问题\r\n", new object[0]));
                        }
                        else
                        {
                            this.txtInfo.AppendText(string.Format("failedPin 问题管脚号: {0}\r\n", controller.runinfo.reservedBytes[0]));
                            this.txtInfo.AppendText(icDesc.failedPinDesc(controller.runinfo.reservedBytes[0]));
                            if ((controller.runinfo.reservedBytes[1] & 240) == 0)
                            {
                                this.txtInfo.AppendText(string.Format("failedPinDesc 问题管脚PORT号: G{0:X}\r\n", controller.runinfo.reservedBytes[1]));
                            }
                            else
                            {
                                this.txtInfo.AppendText(string.Format("failedPinDesc 问题管脚PORT号: {0:X2}\r\n", controller.runinfo.reservedBytes[1]));
                            }
                            this.txtInfo.AppendText(string.Format("failedPinDiffPortType 问题管脚PORT类: {0:X2}\r\n", controller.runinfo.reservedBytes[2]));
                            string str = "";
                            switch ((controller.runinfo.reservedBytes[2] >> 4))
                            {
                                case 1:
                                    str = "初始默认就有问题";
                                    break;

                                case 2:
                                    str = "管脚高平设置时 就有问题";
                                    break;

                                case 3:
                                    str = "管脚高平设置时 此脚 就有问题";
                                    break;

                                case 4:
                                    str = "管脚低平设置时 就有问题";
                                    break;

                                case 5:
                                    str = "管脚低平设置时 此脚 就有问题";
                                    break;
                            }
                            if ((controller.runinfo.reservedBytes[2] & 15) == 0)
                            {
                                this.txtInfo.AppendText(string.Format("产生问题的另一端口PORT= PORTG\r\n", new object[0]));
                            }
                            else
                            {
                                this.txtInfo.AppendText(string.Format("产生问题的另一端口PORT: PORT{0:X}\r\n", controller.runinfo.reservedBytes[2] & 15));
                            }
                            if (str != "")
                            {
                                this.txtInfo.AppendText(str + "\r\n");
                            }
                            this.txtInfo.AppendText(string.Format("failedPinDiff 存在不同: {0:X2}\r\n", controller.runinfo.reservedBytes[3]));
                        }
                        this.txtInfo.AppendText("启动时的PORT值 progPORT_InputVal:\r\n");
                        this.txtInfo.AppendText(string.Format("0x{0:X2},0x{1:X2},0x{2:X2},0x{3:X2},0x{4:X2},0x{5:X2},0x{6:X2}\r\n", new object[] { controller.runinfo.reservedBytes[4], controller.runinfo.reservedBytes[5], controller.runinfo.reservedBytes[6], controller.runinfo.reservedBytes[7], controller.runinfo.reservedBytes[8], controller.runinfo.reservedBytes[9], controller.runinfo.reservedBytes[10] }));
                        this.txtInfo.AppendText(string.Format("PORTA= {0:X2}\r\n", controller.runinfo.reservedBytes[4]));
                        this.txtInfo.AppendText(string.Format("PORTB= {0:X2}\r\n", controller.runinfo.reservedBytes[5]));
                        this.txtInfo.AppendText(string.Format("PORTC= {0:X2}\r\n", controller.runinfo.reservedBytes[6]));
                        this.txtInfo.AppendText(string.Format("PORTD= {0:X2}\r\n", controller.runinfo.reservedBytes[7]));
                        this.txtInfo.AppendText(string.Format("PORTE= {0:X2}\r\n", controller.runinfo.reservedBytes[8]));
                        this.txtInfo.AppendText(string.Format("PORTF= {0:X2}\r\n", controller.runinfo.reservedBytes[9]));
                        this.txtInfo.AppendText(string.Format("PORTG= {0:X2}\r\n", controller.runinfo.reservedBytes[10]));
                        this.txtInfo.AppendText("当前的PORT值 progPORTDATA_R:\r\n");
                        this.txtInfo.AppendText(string.Format("PORTA= {0:X2}\r\n", controller.runinfo.reservedBytes[11]));
                        this.txtInfo.AppendText(string.Format("PORTB= {0:X2}\r\n", controller.runinfo.reservedBytes[12]));
                        this.txtInfo.AppendText(string.Format("PORTC= {0:X2}\r\n", controller.runinfo.reservedBytes[13]));
                        this.txtInfo.AppendText(string.Format("PORTD= {0:X2}\r\n", controller.runinfo.reservedBytes[14]));
                        this.txtInfo.AppendText(string.Format("PORTE= {0:X2}\r\n", controller.runinfo.reservedBytes[15]));
                        this.txtInfo.AppendText(string.Format("PORTF= {0:X2}\r\n", controller.runinfo.reservedBytes[0x10]));
                        this.txtInfo.AppendText(string.Format("PORTG= {0:X2}\r\n", controller.runinfo.reservedBytes[0x11]));
                        this.txtInfo.AppendText(string.Format("门磁状态的8-15bit位[火警/强制锁门]: {0:X2}\r\n", controller.runinfo.pbdsStatusHigh));
                        this.txtInfo.AppendText(string.Format("强制锁门: {0}\r\n", controller.runinfo.ForceLockIsActive ? "     动作" : "未动作"));
                        this.txtInfo.AppendText(string.Format("火警: {0}\r\n", controller.runinfo.FireIsActive ? "     动作" : "未动作"));
                        this.txtInfo.AppendText(string.Format("所有数据: {0}\r\n", controller.runinfo.BytesDataStr));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
            wgTools.gate = gate;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.GetControllerRunInformationIP();
                if (controller.runinfo.wgcticks > 0)
                {
                    for (int i = 0; i < 0x10; i++)
                    {
                        this.txtInfo.AppendText(string.Format("FRam[{0:d}] = {1:d}\r\n", i, controller.runinfo.LngFRam(i)));
                    }
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.UpdateFRamIP((uint) this.numericUpDown13.Value, (uint) this.numericUpDown14.Value);
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button24_Click(object sender, EventArgs e)
        {
        }

        private void button25_Click(object sender, EventArgs e)
        {
            WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH();
            tssi_flash.type = 0x21;
            tssi_flash.code = 0x30;
            tssi_flash.iDevSnFrom = 0;
            tssi_flash.iDevSnTo = uint.MaxValue;
            tssi_flash.iCallReturn = 0;
            tssi_flash.ucData = new byte[0x400];
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                tssi_flash.iStartFlashAddr = (uint) (this.nudStartPage.Value * 1024M);
                tssi_flash.iEndFlashAddr = (tssi_flash.iStartFlashAddr + 0x400) - 1;
                for (int i = 0; i < 0x400; i++)
                {
                    tssi_flash.ucData[i] = 0xff;
                }
                for (int j = 0; j < this.nudDatalen.Value; j++)
                {
                    tssi_flash.ucData[j] = (byte) (this.nudValue.Value * 17M);
                }
                byte[] recv = null;
                this.txtInfo.AppendText("button25 Start: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "\r\n");
                while (tssi_flash.iStartFlashAddr <= ((uint) (this.nudEndPage.Value * 1024M)))
                {
                    int num3 = comm.udp_get(tssi_flash.ToBytes(comm.udpPort), 300, tssi_flash.xid, this.defaultIP, this.defaultPORT, ref recv);
                    if (num3 < 0)
                    {
                        this.txtInfo.AppendText("button25 Err Ret=: " + num3.ToString() + "\r\n");
                        break;
                    }
                    tssi_flash.iStartFlashAddr += 0x400;
                    this.label107.Text = (tssi_flash.iStartFlashAddr / 0x400).ToString();
                }
                this.txtInfo.AppendText("button25 End: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "\r\n");
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY();
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.txtSN.Value, (uint) (this.nudStartPage.Value * 1024M), (uint) ((this.nudStartPage.Value * 1024M) + 1024M - 1));
                byte[] cmd = tssi_flash_query.ToBytes(comm.udpPort);
                byte[] recv = null;
                if (comm.udp_get(cmd, 300, tssi_flash_query.xid, this.defaultIP, this.defaultPORT, ref recv) < 0)
                {
                    comm.Dispose();
                    return;
                }
                if (recv != null)
                {
                    string text = BitConverter.ToString(recv);
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                }
                wgTools.WriteLine(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这是发行版本软件");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这是发行版本软件");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            this.dfrmNetControllerConfig1 = new dfrmNetControllerConfig();
            this.dfrmNetControllerConfig1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
            if (this.checkBox58.Checked)
            {
                mjconf.DoorControlSet(1, this.comboBox53.SelectedIndex);
                mjconf.DoorControlSet(2, this.comboBox54.SelectedIndex);
                mjconf.DoorControlSet(3, this.comboBox55.SelectedIndex);
                mjconf.DoorControlSet(4, this.comboBox56.SelectedIndex);
            }
            if (this.checkBox23.Checked)
            {
                mjconf.ReaderAuthModeSet(1, this.checkBox1.Checked ? 1 : 0);
                mjconf.ReaderAuthModeSet(2, this.checkBox2.Checked ? 1 : 0);
                mjconf.ReaderAuthModeSet(3, this.checkBox3.Checked ? 1 : 0);
                mjconf.ReaderAuthModeSet(4, this.checkBox4.Checked ? 1 : 0);
            }
            if (this.checkBox21.Checked)
            {
                int num = 0;
                mjconf.DoorInterlockSet(1, num);
                mjconf.DoorInterlockSet(2, num);
                mjconf.DoorInterlockSet(3, num);
                mjconf.DoorInterlockSet(4, num);
                if ((this.checkBox5.Checked || this.checkBox6.Checked) || (this.checkBox7.Checked || this.checkBox8.Checked))
                {
                    num = 1;
                    if (this.checkBox5.Checked)
                    {
                        num += 0x10;
                    }
                    if (this.checkBox6.Checked)
                    {
                        num += 0x20;
                    }
                    if (this.checkBox7.Checked)
                    {
                        num += 0x40;
                    }
                    if (this.checkBox8.Checked)
                    {
                        num += 0x80;
                    }
                    mjconf.DoorInterlockSet(1, num);
                }
                if ((this.checkBox9.Checked || this.checkBox10.Checked) || (this.checkBox11.Checked || this.checkBox12.Checked))
                {
                    num = 2;
                    if (this.checkBox9.Checked)
                    {
                        num += 0x10;
                    }
                    if (this.checkBox10.Checked)
                    {
                        num += 0x20;
                    }
                    if (this.checkBox11.Checked)
                    {
                        num += 0x40;
                    }
                    if (this.checkBox12.Checked)
                    {
                        num += 0x80;
                    }
                    mjconf.DoorInterlockSet(2, num);
                }
                if ((this.checkBox13.Checked || this.checkBox14.Checked) || (this.checkBox15.Checked || this.checkBox16.Checked))
                {
                    num = 4;
                    if (this.checkBox13.Checked)
                    {
                        num += 0x10;
                    }
                    if (this.checkBox14.Checked)
                    {
                        num += 0x20;
                    }
                    if (this.checkBox15.Checked)
                    {
                        num += 0x40;
                    }
                    if (this.checkBox16.Checked)
                    {
                        num += 0x80;
                    }
                    mjconf.DoorInterlockSet(3, num);
                }
                if ((this.checkBox17.Checked || this.checkBox18.Checked) || (this.checkBox19.Checked || this.checkBox20.Checked))
                {
                    num = 8;
                    if (this.checkBox17.Checked)
                    {
                        num += 0x10;
                    }
                    if (this.checkBox18.Checked)
                    {
                        num += 0x20;
                    }
                    if (this.checkBox19.Checked)
                    {
                        num += 0x40;
                    }
                    if (this.checkBox20.Checked)
                    {
                        num += 0x80;
                    }
                    mjconf.DoorInterlockSet(4, num);
                }
            }
            if (this.checkBox22.Checked)
            {
                if (this.radioButton1.Checked)
                {
                    mjconf.antiback = 0;
                }
                else if (this.radioButton2.Checked)
                {
                    mjconf.antiback = 1;
                }
                else if (this.radioButton3.Checked)
                {
                    mjconf.antiback = 2;
                }
                else if (this.radioButton4.Checked)
                {
                    mjconf.antiback = 3;
                }
                else if (this.radioButton5.Checked)
                {
                    mjconf.antiback = 4;
                }
                mjconf.autiback_allow_firstout_enable = this.radioButton28.Checked ? 0xa5 : 0;
            }
            if (this.checkBox110.Checked)
            {
                mjconf.indoorPersonsMax = (int) this.numericUpDown11.Value;
                mjconf.indoorPersonsMin = (int) this.numericUpDown12.Value;
            }
            if (this.checkBox30.Checked)
            {
                int num2 = 0;
                num2 += this.checkBox31.Checked ? 1 : 0;
                num2 += (this.checkBox32.Checked ? 1 : 0) << 1;
                num2 += (this.checkBox33.Checked ? 1 : 0) << 2;
                num2 += (this.checkBox34.Checked ? 1 : 0) << 3;
                mjconf.moreCardRead4Reader = num2;
                mjconf.MorecardNeedCardsSet(1, int.Parse(this.comboBox9.Text));
                mjconf.MorecardNeedCardsSet(2, int.Parse(this.comboBox10.Text));
                mjconf.MorecardNeedCardsSet(3, int.Parse(this.comboBox11.Text));
                mjconf.MorecardNeedCardsSet(4, int.Parse(this.comboBox12.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 1, int.Parse(this.comboBox13.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 1, int.Parse(this.comboBox14.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 1, int.Parse(this.comboBox15.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 1, int.Parse(this.comboBox16.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 2, int.Parse(this.comboBox17.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 2, int.Parse(this.comboBox18.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 2, int.Parse(this.comboBox19.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 2, int.Parse(this.comboBox20.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 3, int.Parse(this.comboBox21.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 3, int.Parse(this.comboBox22.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 3, int.Parse(this.comboBox23.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 3, int.Parse(this.comboBox24.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 4, int.Parse(this.comboBox25.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 4, int.Parse(this.comboBox26.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 4, int.Parse(this.comboBox27.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 4, int.Parse(this.comboBox28.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 5, int.Parse(this.comboBox29.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 5, int.Parse(this.comboBox30.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 5, int.Parse(this.comboBox31.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 5, int.Parse(this.comboBox32.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 6, int.Parse(this.comboBox33.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 6, int.Parse(this.comboBox34.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 6, int.Parse(this.comboBox35.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 6, int.Parse(this.comboBox36.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 7, int.Parse(this.comboBox37.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 7, int.Parse(this.comboBox38.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 7, int.Parse(this.comboBox39.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 7, int.Parse(this.comboBox40.Text));
                mjconf.MorecardGroupNeedCardsSet(1, 8, int.Parse(this.comboBox41.Text));
                mjconf.MorecardGroupNeedCardsSet(2, 8, int.Parse(this.comboBox42.Text));
                mjconf.MorecardGroupNeedCardsSet(3, 8, int.Parse(this.comboBox43.Text));
                mjconf.MorecardGroupNeedCardsSet(4, 8, int.Parse(this.comboBox44.Text));
                mjconf.MorecardSequenceInputSet(1, this.checkBox35.Checked);
                mjconf.MorecardSequenceInputSet(2, this.checkBox36.Checked);
                mjconf.MorecardSequenceInputSet(3, this.checkBox37.Checked);
                mjconf.MorecardSequenceInputSet(4, this.checkBox38.Checked);
                mjconf.MorecardSingleGroupEnableSet(1, this.checkBox39.Checked);
                mjconf.MorecardSingleGroupEnableSet(2, this.checkBox40.Checked);
                mjconf.MorecardSingleGroupEnableSet(3, this.checkBox41.Checked);
                mjconf.MorecardSingleGroupEnableSet(4, this.checkBox42.Checked);
                mjconf.MorecardSingleGroupStartNOSet(1, int.Parse(this.comboBox45.Text));
                mjconf.MorecardSingleGroupStartNOSet(2, int.Parse(this.comboBox46.Text));
                mjconf.MorecardSingleGroupStartNOSet(3, int.Parse(this.comboBox47.Text));
                mjconf.MorecardSingleGroupStartNOSet(4, int.Parse(this.comboBox48.Text));
            }
            if (this.checkBox50.Checked)
            {
                mjconf.controlTaskList_enabled = this.radioButton7.Checked ? 1 : 0;
            }
            if (this.checkBox51.Checked)
            {
                mjconf.FirstCardInfoSet(1, (this.comboBox49.SelectedIndex > 0) ? (this.comboBox49.SelectedIndex + 0x10) : 0);
                mjconf.FirstCardInfoSet(2, (this.comboBox50.SelectedIndex > 0) ? (this.comboBox50.SelectedIndex + 0x10) : 0);
                mjconf.FirstCardInfoSet(3, (this.comboBox51.SelectedIndex > 0) ? (this.comboBox51.SelectedIndex + 0x10) : 0);
                mjconf.FirstCardInfoSet(4, (this.comboBox52.SelectedIndex > 0) ? (this.comboBox52.SelectedIndex + 0x10) : 0);
            }
            if (this.checkBox52.Checked)
            {
                int num3 = 0;
                num3 += this.checkBox53.Checked ? 1 : 0;
                num3 += this.checkBox54.Checked ? 2 : 0;
                num3 += this.checkBox55.Checked ? 4 : 0;
                num3 += this.checkBox56.Checked ? 8 : 0;
                mjconf.lockSwitchOption = num3;
            }
            if (this.checkBox57.Checked)
            {
                mjconf.receventPB = this.radioButton9.Checked ? 1 : 0;
                mjconf.receventDS = this.radioButton9.Checked ? 1 : 0;
            }
            if (this.checkBox59.Checked)
            {
                mjconf.SuperpasswordSet(1, string.IsNullOrEmpty(this.textBox1.Text) ? 0xffff : int.Parse(this.textBox1.Text));
                mjconf.SuperpasswordSet(2, string.IsNullOrEmpty(this.textBox2.Text) ? 0xffff : int.Parse(this.textBox2.Text));
                mjconf.SuperpasswordSet(3, string.IsNullOrEmpty(this.textBox3.Text) ? 0xffff : int.Parse(this.textBox3.Text));
                mjconf.SuperpasswordSet(4, string.IsNullOrEmpty(this.textBox4.Text) ? 0xffff : int.Parse(this.textBox4.Text));
                mjconf.SuperpasswordSet(5, string.IsNullOrEmpty(this.textBox5.Text) ? 0xffff : int.Parse(this.textBox5.Text));
                mjconf.SuperpasswordSet(6, string.IsNullOrEmpty(this.textBox6.Text) ? 0xffff : int.Parse(this.textBox6.Text));
                mjconf.SuperpasswordSet(7, string.IsNullOrEmpty(this.textBox7.Text) ? 0xffff : int.Parse(this.textBox7.Text));
                mjconf.SuperpasswordSet(8, string.IsNullOrEmpty(this.textBox8.Text) ? 0xffff : int.Parse(this.textBox8.Text));
                mjconf.SuperpasswordSet(9, string.IsNullOrEmpty(this.textBox9.Text) ? 0xffff : int.Parse(this.textBox9.Text));
                mjconf.SuperpasswordSet(10, string.IsNullOrEmpty(this.textBox10.Text) ? 0xffff : int.Parse(this.textBox10.Text));
                mjconf.SuperpasswordSet(11, string.IsNullOrEmpty(this.textBox11.Text) ? 0xffff : int.Parse(this.textBox11.Text));
                mjconf.SuperpasswordSet(12, string.IsNullOrEmpty(this.textBox12.Text) ? 0xffff : int.Parse(this.textBox12.Text));
                mjconf.SuperpasswordSet(13, string.IsNullOrEmpty(this.textBox13.Text) ? 0xffff : int.Parse(this.textBox13.Text));
                mjconf.SuperpasswordSet(14, string.IsNullOrEmpty(this.textBox14.Text) ? 0xffff : int.Parse(this.textBox14.Text));
                mjconf.SuperpasswordSet(15, string.IsNullOrEmpty(this.textBox15.Text) ? 0xffff : int.Parse(this.textBox15.Text));
                mjconf.SuperpasswordSet(0x10, string.IsNullOrEmpty(this.textBox16.Text) ? 0xffff : int.Parse(this.textBox16.Text));
            }
            if (this.checkBox60.Checked)
            {
                int num4 = 0;
                num4 += this.checkBox61.Checked ? 1 : 0;
                num4 += this.checkBox62.Checked ? 2 : 0;
                num4 += this.checkBox63.Checked ? 4 : 0;
                num4 += this.checkBox64.Checked ? 8 : 0;
                num4 += this.checkBox65.Checked ? 0x10 : 0;
                num4 += this.checkBox66.Checked ? 0x20 : 0;
                num4 += this.checkBox67.Checked ? 0x40 : 0;
                num4 += this.checkBox68.Checked ? 0x80 : 0;
                mjconf.warnSetup = num4;
                if (!string.IsNullOrEmpty(this.textBox17.Text))
                {
                    mjconf.xpPassword = int.Parse(this.textBox17.Text);
                }
            }
            if (this.checkBox69.Checked)
            {
                mjconf.receventWarn = this.checkBox70.Checked ? 1 : 0;
            }
            if (this.checkBox71.Checked)
            {
                int extPortNum = 0;
                if (this.radioButton19.Checked)
                {
                    extPortNum = 0;
                }
                if (this.radioButton20.Checked)
                {
                    extPortNum = 1;
                }
                if (this.radioButton21.Checked)
                {
                    extPortNum = 2;
                }
                if (this.radioButton22.Checked)
                {
                    extPortNum = 3;
                }
                int num6 = 0;
                if (this.radioButton10.Checked)
                {
                    num6 = 0;
                }
                if (this.radioButton11.Checked)
                {
                    num6 = 1;
                }
                if (this.radioButton12.Checked)
                {
                    num6 = 2;
                }
                if (this.radioButton13.Checked)
                {
                    num6 = 3;
                }
                if (this.radioButton25.Checked)
                {
                    num6 = 0x10;
                }
                mjconf.Ext_doorSet(extPortNum, num6);
                int num7 = 0;
                if (this.radioButton23.Checked)
                {
                    num7 = 0;
                }
                if (this.radioButton14.Checked)
                {
                    num7 = 1;
                }
                if (this.radioButton15.Checked)
                {
                    num7 = 2;
                }
                if (this.radioButton16.Checked)
                {
                    num7 = 3;
                }
                if (this.radioButton17.Checked)
                {
                    num7 = 4;
                }
                if (this.radioButton18.Checked)
                {
                    num7 = 5;
                }
                mjconf.Ext_controlSet(extPortNum, num7);
                if (!this.radioButton25.Checked)
                {
                    int num8 = 0;
                    if (this.checkBox84.Checked)
                    {
                        num8 |= 1;
                    }
                    if (this.checkBox85.Checked)
                    {
                        num8 |= 2;
                    }
                    if (this.checkBox86.Checked)
                    {
                        num8 |= 4;
                    }
                    if (this.checkBox87.Checked)
                    {
                        num8 |= 8;
                    }
                    if (this.checkBox88.Checked)
                    {
                        num8 |= 0x10;
                    }
                    if (this.checkBox89.Checked)
                    {
                        num8 |= 0x20;
                    }
                    if (this.checkBox90.Checked)
                    {
                        num8 |= 0x40;
                    }
                    mjconf.Ext_warnSignalEnabledSet(extPortNum, num8);
                    int num9 = 0;
                    if (this.checkBox76.Checked)
                    {
                        num9 |= 1;
                    }
                    if (this.checkBox77.Checked)
                    {
                        num9 |= 2;
                    }
                    if (this.checkBox78.Checked)
                    {
                        num9 |= 4;
                    }
                    if (this.checkBox79.Checked)
                    {
                        num9 |= 8;
                    }
                    if (this.checkBox80.Checked)
                    {
                        num9 |= 0x10;
                    }
                    if (this.checkBox81.Checked)
                    {
                        num9 |= 0x20;
                    }
                    if (this.checkBox82.Checked)
                    {
                        num9 |= 0x40;
                    }
                    if (this.checkBox83.Checked)
                    {
                        num9 |= 0x80;
                    }
                    mjconf.Ext_warnSignalEnabled2Set(extPortNum, num9);
                }
                else
                {
                    mjconf.ext_SetAlarmOnDelay = (int) this.numericUpDown4.Value;
                    mjconf.ext_SetAlarmOffDelay = (int) this.numericUpDown5.Value;
                    int num10 = 0;
                    num10 += this.checkBox91.Checked ? 2 : 0;
                    num10 += this.checkBox92.Checked ? 4 : 0;
                    num10 += this.checkBox93.Checked ? 8 : 0;
                    num10 += this.checkBox94.Checked ? 0x10 : 0;
                    num10 += this.checkBox95.Checked ? 0x20 : 0;
                    num10 += this.checkBox96.Checked ? 0x40 : 0;
                    mjconf.ext_AlarmControlMode = num10;
                }
                mjconf.Ext_timeoutSet(extPortNum, (int) this.numericUpDown3.Value);
            }
            if (this.checkBox106.Checked)
            {
                mjconf.swipeOrderMode = 0;
                if (!this.checkBox107.Checked)
                {
                    if (this.checkBox109.Checked)
                    {
                        mjconf.swipeOrderMode |= 2;
                    }
                    else
                    {
                        mjconf.swipeOrderMode |= this.checkBox108.Checked ? 1 : 0;
                    }
                }
            }
            if (this.checkBox112.Checked)
            {
                mjconf.dataServerIP = IPAddress.Parse(this.textBox22.Text);
                mjconf.dataServerPort = int.Parse(this.textBox23.Text);
            }
            if (this.checkBox119.Checked)
            {
                mjconf.swipeGap = (int) this.numericUpDown16.Value;
            }
            if (this.checkBox121.Checked)
            {
                mjconf.pcControlSwipeTimeout = (int) this.numericUpDown18.Value;
            }
            if (this.checkBox122.Checked)
            {
                mjconf.dhcpEnable = this.checkBox123.Checked ? 0xa5 : 0;
            }
            if (this.checkBox124.Checked)
            {
                mjconf.auto_negotiation_enable = this.checkBox125.Checked ? 0xa5 : 0;
            }
            if (this.checkBox126.Checked)
            {
                mjconf.check_controller_online_timeout = (int) this.numericUpDown19.Value;
            }
            if (this.checkBox132.Checked)
            {
                mjconf.elevatorSingleDelay = (float) this.numericUpDown21.Value;
                mjconf.elevatorMultioutputDelay = (float) this.numericUpDown20.Value;
            }
            if (this.checkBox134.Checked)
            {
                if ((string.IsNullOrEmpty(this.textBox27.Text) || string.IsNullOrEmpty(this.textBox28.Text)) || string.IsNullOrEmpty(this.textBox33.Text))
                {
                    mjconf.custom_cardformat_totalbits = 0;
                    mjconf.custom_cardformat_startloc = 0;
                    mjconf.custom_cardformat_validbits = 0;
                    mjconf.custom_cardformat_sumcheck = 0;
                }
                else
                {
                    mjconf.custom_cardformat_totalbits = int.Parse(this.textBox33.Text);
                    mjconf.custom_cardformat_startloc = int.Parse(this.textBox28.Text);
                    mjconf.custom_cardformat_validbits = int.Parse(this.textBox27.Text);
                    mjconf.custom_cardformat_sumcheck = ((mjconf.custom_cardformat_totalbits + mjconf.custom_cardformat_startloc) + mjconf.custom_cardformat_validbits) & 0xff;
                }
            }
            if (this.checkBox136.Checked)
            {
                if (this.radioButton29.Checked)
                {
                    mjconf.fire_broadcast_receive = 0;
                }
                else if (this.radioButton30.Checked)
                {
                    mjconf.fire_broadcast_receive = 0x80;
                }
                else if (this.radioButton31.Checked)
                {
                    if (this.numericUpDown28.Value < 127M)
                    {
                        mjconf.fire_broadcast_receive = (int) this.numericUpDown28.Value;
                    }
                    else
                    {
                        mjconf.fire_broadcast_receive = 0x7e;
                    }
                }
                if (this.radioButton32.Checked)
                {
                    mjconf.fire_broadcast_send = 0;
                }
                else if (this.radioButton33.Checked)
                {
                    mjconf.fire_broadcast_send = (int) this.numericUpDown29.Value;
                }
            }
            if (this.checkBox137.Checked)
            {
                if (this.radioButton36.Checked)
                {
                    mjconf.interlock_broadcast_receive = 0;
                }
                else if (this.radioButton37.Checked)
                {
                    if (this.numericUpDown28.Value < 253M)
                    {
                        mjconf.interlock_broadcast_receive = (int) this.numericUpDown31.Value;
                    }
                    else
                    {
                        mjconf.interlock_broadcast_send = 0xfd;
                    }
                }
                if (this.radioButton34.Checked)
                {
                    mjconf.interlock_broadcast_send = 0;
                }
                else if (this.radioButton35.Checked)
                {
                    mjconf.interlock_broadcast_send = (int) this.numericUpDown30.Value;
                }
            }
            if (this.checkBox138.Checked)
            {
                if (this.radioButton38.Checked)
                {
                    mjconf.antiback_broadcast_send = 0;
                }
                else if (this.radioButton39.Checked)
                {
                    mjconf.antiback_broadcast_send = (int) this.numericUpDown32.Value;
                }
            }
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.UpdateConfigureIP(mjconf);
                if (this.checkBox50.Checked && this.radioButton7.Checked)
                {
                    controller.RenewControlTaskListIP();
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button30_Click(object sender, EventArgs e)
        {
        }

        private void button31_Click(object sender, EventArgs e)
        {
        }

        private void button32_Click(object sender, EventArgs e)
        {
            Interaction.Shell("SqlConfig.exe UPDATE", AppWinStyle.Hide, false, -1);
            Thread.Sleep(0x1388);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            DataTable dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
            this.DisplayData(dataSources);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            Form owner = base.Owner;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                byte[] data = new byte[0x480];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }
                char[] chArray = this.txtCommPassword.Text.PadRight(0x10, '\0').ToCharArray();
                int index = 0x10;
                for (int j = 0; (j < 0x10) && (j < chArray.Length); j++)
                {
                    data[index] = (byte) (chArray[j] & '\x00ff');
                    data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                    index++;
                }
                controller.UpdateConfigureCPUSuperIP(data, this.txtOldCommPassword.Text);
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCommPassword.Text))
            {
                wgAppConfig.UpdateKeyVal("CommCurrent", "");
            }
            else
            {
                this.txtCommPassword.Text = this.txtCommPassword.Text.Trim();
                if (string.IsNullOrEmpty(this.txtCommPassword.Text))
                {
                    wgAppConfig.UpdateKeyVal("CommCurrent", "");
                }
                else
                {
                    wgAppConfig.UpdateKeyVal("CommCurrent", WGPacket.Ept(this.txtCommPassword.Text));
                }
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (this.timer1.Enabled)
            {
                this.timer1.Enabled = false;
            }
            if (this.control4Tcp == null)
            {
                this.control4Tcp = new icController();
            }
            try
            {
                this.control4Tcp.ControllerSN = this.defaultSN;
                this.control4Tcp.IP = this.defaultIP;
                this.control4Tcp.PORT = this.defaultPORT;
                this.control4Tcp.GetControllerRunInformationIP_TCP(this.textBox21.Text);
                if (this.control4Tcp.runinfo.wgcticks > 0)
                {
                    this.label113.Text = this.control4Tcp.runinfo.dtNow.ToString(wgTools.YMDHMSFormat);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            if (this.control4Tcp != null)
            {
                this.control4Tcp.TCP_Close();
                this.control4Tcp = null;
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                this.frmTestController1 = new frmTestController();
                this.frmTestController1.Text = this.frmTestController1.Text + ((i + 2)).ToString();
                this.frmTestController1.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            uint num;
            MjUserInfo mjrc = new MjUserInfo();
            if (uint.TryParse(this.txtCardNO.Text, NumberStyles.Integer, null, out num))
            {
                mjrc.cardID = num;
                if (this.checkBox25.Checked)
                {
                    mjrc.IsActivated = true;
                }
                if (this.checkBox24.Checked)
                {
                    mjrc.IsDeleted = true;
                }
                if (this.checkBox111.Checked)
                {
                    mjrc.IsSuperCard = true;
                }
                mjrc.password = uint.Parse(this.txtPassword.Text);
                mjrc.ymdStart = this.dtpActivate.Value;
                mjrc.ymdEnd = this.dtpDeactivate.Value;
                mjrc.ControlSegIndexSet(1, byte.Parse(this.comboBox1.Text));
                mjrc.ControlSegIndexSet(2, byte.Parse(this.comboBox2.Text));
                mjrc.ControlSegIndexSet(3, byte.Parse(this.comboBox3.Text));
                mjrc.ControlSegIndexSet(4, byte.Parse(this.comboBox4.Text));
                mjrc.FirstCardSet(1, this.checkBox26.Checked);
                mjrc.FirstCardSet(2, this.checkBox27.Checked);
                mjrc.FirstCardSet(3, this.checkBox28.Checked);
                mjrc.FirstCardSet(4, this.checkBox29.Checked);
                mjrc.MoreCardGroupIndexSet(1, byte.Parse(this.comboBox5.Text));
                mjrc.MoreCardGroupIndexSet(2, byte.Parse(this.comboBox6.Text));
                mjrc.MoreCardGroupIndexSet(3, byte.Parse(this.comboBox7.Text));
                mjrc.MoreCardGroupIndexSet(4, byte.Parse(this.comboBox8.Text));
                if (this.radioButton24.Checked)
                {
                    mjrc.maxSwipe = ((int) this.numericUpDown6.Value) & 0x1ff;
                }
                else if (this.radioButton26.Checked)
                {
                    mjrc.hmsEnd = this.dateTimePicker14.Value;
                }
                if (this.checkBox128.Checked && wgMjController.IsElevator((int) this.txtSN.Value))
                {
                    mjrc.AllowFloors = 0L;
                    mjrc.AllowFloors |= this.checkBox129.Checked ? ((ulong) 0x10000000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox141.Checked ? ((ulong) 1L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox142.Checked ? ((ulong) 2L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox143.Checked ? ((ulong) 4L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox144.Checked ? ((ulong) 8L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox145.Checked ? ((ulong) 0x10L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox146.Checked ? ((ulong) 0x20L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox147.Checked ? ((ulong) 0x40L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox148.Checked ? ((ulong) 0x80L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox149.Checked ? ((ulong) 0x100L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox150.Checked ? ((ulong) 0x200L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox151.Checked ? ((ulong) 0x400L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox152.Checked ? ((ulong) 0x800L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox153.Checked ? ((ulong) 0x1000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox154.Checked ? ((ulong) 0x2000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox155.Checked ? ((ulong) 0x4000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox156.Checked ? ((ulong) 0x8000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox157.Checked ? ((ulong) 0x10000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox158.Checked ? ((ulong) 0x20000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox159.Checked ? ((ulong) 0x40000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox160.Checked ? ((ulong) 0x80000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox161.Checked ? ((ulong) 0x100000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox162.Checked ? ((ulong) 0x200000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox163.Checked ? ((ulong) 0x400000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox164.Checked ? ((ulong) 0x800000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox165.Checked ? ((ulong) 0x1000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox166.Checked ? ((ulong) 0x2000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox167.Checked ? ((ulong) 0x4000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox168.Checked ? ((ulong) 0x8000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox169.Checked ? ((ulong) 0x10000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox170.Checked ? ((ulong) 0x20000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox171.Checked ? ((ulong) 0x40000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox172.Checked ? 0x80000000 : 0;
                    mjrc.AllowFloors |= this.checkBox173.Checked ? ((ulong) 0x100000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox174.Checked ? ((ulong) 0x200000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox175.Checked ? ((ulong) 0x400000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox176.Checked ? ((ulong) 0x800000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox177.Checked ? ((ulong) 0x1000000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox178.Checked ? ((ulong) 0x2000000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox179.Checked ? ((ulong) 0x4000000000L) : ((ulong) 0L);
                    mjrc.AllowFloors |= this.checkBox180.Checked ? ((ulong) 0x8000000000L) : ((ulong) 0L);
                    if ((mjrc.AllowFloors & ((ulong) 0x10000000000L)) == 0L)
                    {
                        bool flag = false;
                        for (int i = 0; i < 40; i++)
                        {
                            if ((mjrc.AllowFloors & (((ulong) 1L) << i)) > 0L)
                            {
                                if (flag)
                                {
                                    mjrc.AllowFloors |= this.checkBox129.Checked ? ((ulong) 0x10000000000L) : ((ulong) 0L);
                                    break;
                                }
                                flag = true;
                            }
                        }
                    }
                }
                wgMjControllerPrivilege privilege = new wgMjControllerPrivilege();
                try
                {
                    if (this.checkBox120.Checked)
                    {
                        if (privilege.AddPrivilegeOfOneCardIP((int) this.txtSN.Value, this.txtControllerIP.Text, (int) this.nudPort.Value, mjrc) < 0)
                        {
                            MessageBox.Show("failed\r\n");
                            return;
                        }
                    }
                    else if (privilege.AddPrivilegeOfOneCardIP((int) this.txtSN.Value, "", 0xea60, mjrc) < 0)
                    {
                        MessageBox.Show("failed\r\n");
                        return;
                    }
                    if (this.checkBox97.Checked)
                    {
                        for (int j = 0; j < 0xdab; j++)
                        {
                            mjrc.cardID++;
                            if (this.checkBox120.Checked)
                            {
                                if (privilege.AddPrivilegeOfOneCardIP((int) this.txtSN.Value, this.txtControllerIP.Text, (int) this.nudPort.Value, mjrc) >= 0)
                                {
                                    continue;
                                }
                                MessageBox.Show("failed\r\n");
                                return;
                            }
                            if (privilege.AddPrivilegeOfOneCardIP((int) this.txtSN.Value, "", 0xea60, mjrc) < 0)
                            {
                                MessageBox.Show("failed\r\n");
                                return;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    privilege.Dispose();
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (this.control4Tcp == null)
            {
                this.control4Tcp = new icController();
            }
            icController controller = this.control4Tcp;
            controller.ControllerSN = this.defaultSN;
            controller.IP = this.defaultIP;
            controller.PORT = this.defaultPORT;
            controller.TCP_Open(this.textBox21.Text);
            controller.AdjustTimeIP_TCP(DateTime.Now);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            if (this.control4Tcp == null)
            {
                this.control4Tcp = new icController();
            }
            icController controller = this.control4Tcp;
            controller.ControllerSN = this.defaultSN;
            controller.IP = this.defaultIP;
            controller.PORT = this.defaultPORT;
            controller.TCP_Open(this.textBox21.Text);
            uint operatorId = 0;
            uint maxValue = uint.MaxValue;
            controller.ControllerSN = this.defaultSN;
            controller.IP = this.defaultIP;
            controller.PORT = this.defaultPORT;
            if (!string.IsNullOrEmpty(this.textBox18.Text))
            {
                operatorId = uint.Parse(this.textBox18.Text);
            }
            if (!string.IsNullOrEmpty(this.textBox19.Text))
            {
                maxValue = uint.Parse(this.textBox19.Text);
            }
            controller.RemoteOpenDoorIP_TCP(int.Parse(this.textBox20.Text), operatorId, maxValue);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            if (this.control4Tcp == null)
            {
                this.control4Tcp = new icController();
            }
            icController controller = this.control4Tcp;
            controller.ControllerSN = this.defaultSN;
            controller.IP = this.defaultIP;
            controller.PORT = this.defaultPORT;
            controller.TCP_Open(this.textBox21.Text);
            string str = controller.GetProductInfoIP_TCP();
            if (!string.IsNullOrEmpty(str))
            {
                this.label113.Text = str;
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (this.timer1.Enabled)
            {
                this.timer1.Enabled = false;
            }
            if (this.control4Udp == null)
            {
                this.control4Udp = new icController();
                this.lastRecordStr = "";
            }
            icController controller = this.control4Udp;
            controller.ControllerSN = this.defaultSN;
            controller.IP = this.defaultIP;
            controller.PORT = this.defaultPORT;
            controller.GetControllerRunInformationIP();
            if (controller.runinfo.wgcticks > 0)
            {
                this.label114.Text = controller.runinfo.dtNow.ToString(wgTools.YMDHMSFormat);
                if (controller.runinfo.newSwipes[0].ToStringRaw() != this.lastRecordStr)
                {
                    this.lastRecordStr = controller.runinfo.newSwipes[0].ToStringRaw();
                    wgMjControllerSwipeRecord record = controller.runinfo.newSwipes[0];
                    this.txtInfo.AppendText(record.ToDisplaySimpleInfo(true) + "\r\n");
                }
            }
            this.timer1.Enabled = true;
        }

        private void button44_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                string compactInfo = "";
                string desc = "";
                if (!string.IsNullOrEmpty(controller.GetProductInfoIP(ref compactInfo, ref desc)))
                {
                    this.txtInfo.AppendText(desc + "\r\n");
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            this.txtInfo.Text = "";
        }

        private void button47_Click(object sender, EventArgs e)
        {
            string text = "CREATE PARTITION FUNCTION [RangePrivilegePF1](int) AS RANGE LEFT FOR VALUES (";
            string str2 = "CREATE PARTITION SCHEME [RangePrivilegePS1] AS PARTITION [RangePrivilegePF1] TO (";
            for (int i = 1; i < 0x3e7; i++)
            {
                text = text + string.Format("N'{0:d}',", i);
                str2 = str2 + string.Format("[PRIMARY],", new object[0]);
            }
            text = text + string.Format("N'{0:d}')", 0x3e7);
            str2 = str2 + string.Format("[PRIMARY],[PRIMARY])", new object[0]);
            this.txtInfo.AppendText(text);
            this.txtInfo.AppendText("\r\n");
            this.txtInfo.AppendText(str2);
            this.txtInfo.AppendText("\r\n");
        }

        private void button48_Click(object sender, EventArgs e)
        {
        }

        private void button49_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.RebootControllerIP();
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = 26M;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                byte[] data = new byte[0x800];
                long result = 0L;
                if (long.TryParse(this.textBox26.Text, NumberStyles.AllowHexSpecifier, null, out result))
                {
                    int num2;
                    int num3 = (int) (((ulong) (result << 0x10)) & 0xffff0000L);
                    if (int.TryParse(this.textBox24.Text, out num2) || int.TryParse(this.textBox24.Text.Trim().ToUpper().Replace("0X", ""), NumberStyles.AllowHexSpecifier, null, out num2))
                    {
                        wgTools.CommPStr = "";
                        if (controller.GetControlDataIP(num2, num3 + int.Parse(this.textBox25.Text), ref data) > 0)
                        {
                            string str = BitConverter.ToString(data);
                            this.txtInfo.AppendText(str.Substring(0x54, int.Parse(this.textBox25.Text) * 3));
                            this.txtInfo.AppendText("\r\n");
                        }
                        wgTools.CommPStr = wgAppConfig.GetKeyVal("CommCurrent");
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

        private void button51_Click(object sender, EventArgs e)
        {
            this.frmProductFormat1 = new frmProductFormat();
            this.frmProductFormat1.Show();
        }

        private void button52_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这是发行版本软件");
        }

        private void button53_Click(object sender, EventArgs e)
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x23;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.txtSN.Value;
            packet.iCallReturn = 0;
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                byte[] cmd = packet.ToBytes(comm.udpPort);
                if (cmd != null)
                {
                    byte[] recv = null;
                    int num = comm.udp_get(cmd, 300, packet.xid, this.defaultIP, this.defaultPORT, ref recv);
                    MjRegisterCardsParam param = new MjRegisterCardsParam();
                    this.textBox1.AppendText(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                    this.textBox1.AppendText("\r\n");
                    this.Refresh();
                    if (recv != null)
                    {
                        long num3;
                        string str = BitConverter.ToString(recv);
                        param.updateParam(recv, 20);
                        str = "";
                        str = ((str + string.Format("权限起始页 = 0x{0:X8}[{0:d}]\r\n", param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) + string.Format("存储新有序权限的4K的页面 = 0x{0:X8}[{0:d}]\r\n", param.newPrivilegePage4KAddr)) + string.Format("自由的记录页面(无序的) = 0x{0:X8} [{0:d}]\r\n", param.freeNewPrivilegePageAddr) + string.Format("是否有序的(自由页面中) = {0:d}\r\n", param.bOrderInfreePrivilegePage)) + string.Format("总权限数 = {0:d}\r\n", param.totalPrivilegeCount) + string.Format("已删除的权限数 = {0:d}\r\n", param.deletedPrivilegeCount);
                        long num2 = (long) this.numericUpDown15.Value;
                        if (((param.newPrivilegePage4KAddr - param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) / 8) > num2)
                        {
                            num3 = param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR + (num2 * 8L);
                        }
                        else
                        {
                            num2 -= (param.newPrivilegePage4KAddr - param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) / 8;
                            num3 = (param.newPrivilegePage4KAddr + 0x1000) + (num2 * 8L);
                        }
                        WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY();
                        tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.txtSN.Value, (uint) num3, (uint) ((num3 + 0x400L) - 1L));
                        byte[] buffer3 = tssi_flash_query.ToBytes(comm.udpPort);
                        if (cmd != null)
                        {
                            byte[] buffer4 = null;
                            if (comm.udp_get(buffer3, 300, tssi_flash_query.xid, this.defaultIP, this.defaultPORT, ref buffer4) >= 0)
                            {
                                MjUserInfo card = new MjUserInfo();
                                string strRegisterCardAll = "";
                                if (buffer4 != null)
                                {
                                    string text = BitConverter.ToString(buffer4);
                                    card.cardID = (uint) (((buffer4[0x1c] | (buffer4[0x1d] << 8)) | (buffer4[30] << 0x10)) | (buffer4[0x1f] << 0x18));
                                    text = BitConverter.ToString(buffer4).Replace("-", "").Substring(0x38, 0x10) + "[" + card.cardID.ToString() + "]";
                                    strRegisterCardAll = BitConverter.ToString(buffer4).Replace("-", "").Substring(0x38, 0x10);
                                    this.txtInfo.AppendText(text);
                                    this.txtInfo.AppendText("\r\n");
                                }
                                num3 = (num3 * 2L) + 0x194000L;
                                tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.txtSN.Value, (uint) num3, (uint) ((num3 + 0x400L) - 1L));
                                buffer3 = tssi_flash_query.ToBytes(comm.udpPort);
                                if (buffer3 != null)
                                {
                                    buffer4 = null;
                                    if ((comm.udp_get(buffer3, 300, tssi_flash_query.xid, null, 0xea60, ref buffer4) >= 0) && (buffer4 != null))
                                    {
                                        try
                                        {
                                            string str4 = BitConverter.ToString(buffer4).Replace("-", "").Substring(0x38, 0x20);
                                            this.txtInfo.AppendText(str4);
                                            strRegisterCardAll = strRegisterCardAll + BitConverter.ToString(buffer4).Replace("-", "").Substring(0x38, 0x20);
                                            card.get(strRegisterCardAll);
                                            if (wgMjController.IsElevator((int) this.txtSN.Value))
                                            {
                                                string str5 = "允许到达的楼层: ";
                                                for (int i = 0; i < 40; i++)
                                                {
                                                    if ((card.AllowFloors & (((ulong) 1L) << i)) > 0L)
                                                    {
                                                        str5 = str5 + ((i + 1)).ToString() + ",";
                                                    }
                                                }
                                                if ((card.AllowFloors & ((ulong) 0x10000000000L)) > 0L)
                                                {
                                                    str5 = str5 + "\r\n<多层...>";
                                                }
                                                this.txtInfo.AppendText("\r\n");
                                                this.txtInfo.AppendText(str5);
                                            }
                                            this.txtInfo.AppendText("\r\n");
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                comm.Dispose();
            }
        }

        private void button54_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                string compactInfo = "";
                string desc = "";
                string productInfoIP = controller.GetProductInfoIP(ref compactInfo, ref desc);
                if (!string.IsNullOrEmpty(productInfoIP))
                {
                    this.txtInfo.AppendText(productInfoIP + "\r\n");
                    this.txtInfo.AppendText("compactInfo \r\n");
                    this.txtInfo.AppendText(compactInfo + "\r\n");
                    this.txtInfo.AppendText("descInfo\r\n");
                    this.txtInfo.AppendText(desc + "\r\n");
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button55_Click(object sender, EventArgs e)
        {
            int gate = wgTools.gate;
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.ControllerSN = -1;
                wgTools.gate = int.Parse(WGPacket.Dpt("SYP2ipKrbEk9QZIr9LfOoZehvmmtcOURHRu+FOBrU1Q="));
                controller.GetControllerRunInformationIP();
                wgTools.gate = gate;
                if (controller.runinfo.wgcticks <= 0)
                {
                    this.txtInfo.AppendText("???控制器未连接\r\n");
                    this.txtSN.Value = 0M;
                }
                else if (controller.runinfo.CurrentControllerSN == uint.MaxValue)
                {
                    this.txtSN.Value = 999999999M;
                }
                else
                {
                    this.txtSN.Value = controller.runinfo.CurrentControllerSN;
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
            wgTools.gate = gate;
        }

        private void button56_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = -1;
                controller.GetControllerRunInformationIP();
                if (controller.runinfo.wgcticks <= 0)
                {
                    this.txtInfo.AppendText("???控制器未连接\r\n");
                }
                else
                {
                    if (controller.runinfo.CurrentControllerSN != uint.MaxValue)
                    {
                        this.nudNewSN.Value = controller.runinfo.CurrentControllerSN;
                    }
                }
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
                    this.txtInfo.AppendText("???控制器未连接\r\n");
                    SystemSounds.Hand.Play();
                }
                else
                {
                    if (this.checkBox116.Checked)
                    {
                        MjUserInfo mjrc = new MjUserInfo();
                        mjrc.IsActivated = true;
                        mjrc.password = uint.Parse(this.txtPassword.Text);
                        mjrc.ymdStart = this.dtpActivate.Value;
                        mjrc.ymdEnd = this.dtpDeactivate.Value;
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
            controller.Dispose();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[0x480];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
            int index = 0x20;
            for (int j = 0; j < 0x40; j++)
            {
                data[index] = 0x98;
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
            }
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.UpdateConfigureCPUSuperIP(data, "");
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button59_Click(object sender, EventArgs e)
        {
            wgUdpComm.CommTimeoutMsMin = (int) this.numericUpDown17.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.numericUpDown2.Value = 1M;
        }

        private void button60_Click(object sender, EventArgs e)
        {
            this.numericUpDown17.Value = wgUdpComm.CommTimeoutMsMin;
        }

        private void button61_Click(object sender, EventArgs e)
        {
        }

        private void button61_Click_1(object sender, EventArgs e)
        {
            if (this.controlHolidayList == null)
            {
                this.controlHolidayList = new wgMjControllerHolidaysList();
            }
            MjControlHolidayTime mjCHT = new MjControlHolidayTime();
            mjCHT.dtStart = DateTime.Parse(this.dateTimePicker17.Value.ToString("yyyy-MM-dd ") + this.dateTimePicker15.Value.ToString("HH:mm"));
            mjCHT.dtEnd = DateTime.Parse(this.dateTimePicker16.Value.ToString("yyyy-MM-dd ") + this.dateTimePicker18.Value.ToString("HH:mm"));
            this.listBox3.Items.Add(BitConverter.ToString(mjCHT.ToBytes()));
            this.controlHolidayList.AddItem(mjCHT);
        }

        private void button62_Click(object sender, EventArgs e)
        {
            if (this.controlHolidayList != null)
            {
                this.controlHolidayList.Clear();
                this.listBox3.Items.Clear();
            }
        }

        private void button63_Click(object sender, EventArgs e)
        {
            if (this.controlHolidayList != null)
            {
                icController controller = new icController();
                try
                {
                    controller.ControllerSN = this.defaultSN;
                    controller.IP = this.defaultIP;
                    controller.PORT = this.defaultPORT;
                    if (controller.UpdateHolidayListIP(this.controlHolidayList.ToByte()) > 0)
                    {
                        this.txtInfo.AppendText("上传假期 OK\r\n");
                    }
                    else
                    {
                        this.txtInfo.AppendText("上传假期 Failed\r\n");
                    }
                    wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
                    mjconf.holidayControl = (this.controlHolidayList.holidayCount > 0) ? ((byte) 1) : ((byte) 0);
                    controller.UpdateConfigureIP(mjconf);
                }
                catch (Exception)
                {
                }
                controller.Dispose();
            }
        }

        private void button65_Click(object sender, EventArgs e)
        {
            if (this.controlTaskList != null)
            {
                icController controller = new icController();
                try
                {
                    controller.ControllerSN = this.defaultSN;
                    controller.IP = this.defaultIP;
                    controller.PORT = this.defaultPORT;
                    wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
                    mjconf.FirstCardInfoSet(1, 0);
                    mjconf.FirstCardInfoSet(2, 0);
                    mjconf.FirstCardInfoSet(3, 0);
                    mjconf.FirstCardInfoSet(4, 0);
                    if (this.controlTaskList.taskCount > 0)
                    {
                        mjconf.controlTaskList_enabled = 1;
                    }
                    if ((controller.UpdateConfigureIP(mjconf) > 0) && (controller.UpdateControlTaskListIP(this.controlTaskList) > 0))
                    {
                        controller.RenewControlTaskListIP();
                    }
                }
                catch (Exception)
                {
                }
                controller.Dispose();
            }
        }

        private void button66_Click(object sender, EventArgs e)
        {
            this.listBox4.Items.Clear();
            if (this.controlTaskList != null)
            {
                this.controlTaskList.Clear();
            }
        }

        private void button67_Click(object sender, EventArgs e)
        {
            if (this.controlTaskList == null)
            {
                this.controlTaskList = new wgMjControllerTaskList();
            }
            if (((this.cboDoors.SelectedIndex < 0) || (this.cboBeginControlStatus.SelectedIndex < 0)) || (this.cboEndControlStatus.SelectedIndex < 0))
            {
                MessageBox.Show("请先选择各参数");
            }
            else
            {
                MjControlTaskItem mjCI = new MjControlTaskItem();
                mjCI.ymdStart = DateTime.Parse("2010-1-1");
                mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                mjCI.hms = this.dateBeginHMS1.Value;
                mjCI.weekdayControl = 0;
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkMonday.Checked ? ((byte) 1) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkTuesday.Checked ? ((byte) 2) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkWednesday.Checked ? ((byte) 4) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkThursday.Checked ? ((byte) 8) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkFriday.Checked ? ((byte) 0x10) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkSaturday.Checked ? ((byte) 0x20) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkSunday.Checked ? ((byte) 0x40) : ((byte) 0)));
                switch (this.cboBeginControlStatus.SelectedIndex)
                {
                    case 0:
                        mjCI.paramValue = 0x13;
                        break;

                    case 1:
                        mjCI.paramValue = 0x11;
                        break;

                    case 2:
                        mjCI.paramValue = 0x12;
                        break;

                    case 3:
                        mjCI.paramValue = 20;
                        break;

                    default:
                        mjCI.paramValue = 0;
                        break;
                }
                mjCI.paramLoc = 180 + this.cboDoors.SelectedIndex;
                if (this.controlTaskList.AddItem(mjCI) < 0)
                {
                    wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                }
                else
                {
                    this.listBox4.Items.Add(BitConverter.ToString(mjCI.ToBytes()));
                }
                mjCI = new MjControlTaskItem();
                mjCI.ymdStart = DateTime.Parse("2010-1-1");
                mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                mjCI.hms = this.dateEndHMS1.Value;
                mjCI.weekdayControl = 0;
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkMonday.Checked ? ((byte) 1) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkTuesday.Checked ? ((byte) 2) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkWednesday.Checked ? ((byte) 4) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkThursday.Checked ? ((byte) 8) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkFriday.Checked ? ((byte) 0x10) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkSaturday.Checked ? ((byte) 0x20) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkSunday.Checked ? ((byte) 0x40) : ((byte) 0)));
                switch (this.cboEndControlStatus.SelectedIndex)
                {
                    case 0:
                        mjCI.paramValue = 0;
                        break;

                    case 1:
                        mjCI.paramValue = 0;
                        break;

                    case 2:
                        mjCI.paramValue = 0;
                        break;

                    case 3:
                        mjCI.paramValue = 20;
                        break;

                    default:
                        mjCI.paramValue = 0;
                        break;
                }
                mjCI.paramLoc = 180 + this.cboDoors.SelectedIndex;
                if (this.controlTaskList.AddItem(mjCI) < 0)
                {
                    wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                }
                else
                {
                    this.listBox4.Items.Add(BitConverter.ToString(mjCI.ToBytes()));
                }
                mjCI = new MjControlTaskItem();
                mjCI.ymdStart = DateTime.Parse("2010-1-1");
                mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                mjCI.hms = this.dateEndHMS1.Value;
                mjCI.weekdayControl = 0;
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkMonday.Checked ? ((byte) 1) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkTuesday.Checked ? ((byte) 2) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkWednesday.Checked ? ((byte) 4) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkThursday.Checked ? ((byte) 8) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkFriday.Checked ? ((byte) 0x10) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkSaturday.Checked ? ((byte) 0x20) : ((byte) 0)));
                mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.chkSunday.Checked ? ((byte) 0x40) : ((byte) 0)));
                switch (this.cboEndControlStatus.SelectedIndex)
                {
                    case 0:
                        mjCI.paramValue = 3;
                        break;

                    case 1:
                        mjCI.paramValue = 1;
                        break;

                    case 2:
                        mjCI.paramValue = 2;
                        break;

                    case 3:
                        mjCI.paramValue = 3;
                        break;

                    default:
                        mjCI.paramValue = 3;
                        break;
                }
                mjCI.paramLoc = 0x1a + this.cboDoors.SelectedIndex;
                if (this.controlTaskList.AddItem(mjCI) < 0)
                {
                    wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                }
                else
                {
                    this.listBox4.Items.Add(BitConverter.ToString(mjCI.ToBytes()));
                }
            }
        }

        private void button68_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgMjControllerPrivilege privilege = new wgMjControllerPrivilege();
            try
            {
                privilege.AllowDownload();
                if (this.dtPrivilege != null)
                {
                    this.dtPrivilege.Rows.Clear();
                    this.dtPrivilege.Dispose();
                    this.dtPrivilege = null;
                    GC.Collect();
                }
                this.dtPrivilege = new DataTable("Privilege");
                this.dtPrivilege.Columns.Add("f_ConsumerID", System.Type.GetType("System.UInt32"));
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
                this.dtPrivilege.Columns.Add("f_ControlSegID5", System.Type.GetType("System.Byte"));
                this.dtPrivilege.Columns["f_ControlSegID5"].DefaultValue = 0;
                DataTable tbFpTempl = null;
                if (privilege.DownloadIP(this.defaultSN, this.defaultIP, this.defaultPORT, "", ref this.dtPrivilege, ref tbFpTempl) > 0)
                {
                    if (this.dtPrivilege.Rows.Count >= 0)
                    {
                        this.dataGridView1.DataSource = this.dtPrivilege;
                    }
                    else
                    {
                        this.txtInfo.AppendText("failed\r\n");
                    }
                }
                else
                {
                    this.txtInfo.AppendText("failed\r\n");
                }
            }
            catch (Exception)
            {
            }
            privilege.Dispose();
            Cursor.Current = Cursors.Default;
        }

        private void button69_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                uint operatorId = 0;
                ulong maxValue = ulong.MaxValue;
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                if (!string.IsNullOrEmpty(this.textBox18.Text))
                {
                    operatorId = uint.Parse(this.textBox18.Text);
                }
                if (!string.IsNullOrEmpty(this.textBox19.Text))
                {
                    maxValue = ulong.Parse(this.textBox19.Text);
                }
                if (sender == this.button69)
                {
                    controller.RemoteOpenFoorIP(int.Parse(this.textBox31.Text), operatorId, maxValue);
                }
                else
                {
                    controller.RemoteOpenFoorIP(int.Parse(this.textBox31.Text) + 40, operatorId, maxValue);
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.numericUpDown2.Value = 2M;
        }

        private void button70_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                uint operatorId = 0;
                ulong maxValue = ulong.MaxValue;
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                if (!string.IsNullOrEmpty(this.textBox18.Text))
                {
                    operatorId = uint.Parse(this.textBox18.Text);
                }
                if (!string.IsNullOrEmpty(this.textBox19.Text))
                {
                    maxValue = ulong.Parse(this.textBox19.Text);
                }
                int num3 = 0;
                if (sender == this.button72)
                {
                    num3 = 20;
                }
                int millisecondsTimeout = int.Parse(this.numericUpDown22.Value.ToString());
                for (int i = 1; i <= 20; i++)
                {
                    if (this.checkBox130.Checked)
                    {
                        controller.RemoteOpenFoorIP(i + num3, operatorId, maxValue);
                    }
                    if (this.checkBox131.Checked)
                    {
                        controller.RemoteOpenFoorIP((i + 40) + num3, operatorId, maxValue);
                    }
                    Thread.Sleep(millisecondsTimeout);
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button71_Click(object sender, EventArgs e)
        {
            this.txt02e2.Visible = false;
            this.button71.Visible = false;
            this.label109.Visible = false;
            this.checkBox118.Visible = false;
            this.button52.Visible = false;
            this.checkBox113.Visible = false;
            this.checkBox115.Visible = false;
            this.checkBox116.Visible = false;
            this.checkBox117.Visible = false;
            this.checkBox135.Visible = false;
            this.button57.Visible = false;
        }

        private void button73_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                byte[] data = new byte[0x480];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }
                int num3 = 80;
                if (!this.checkBox133.Checked)
                {
                    num3 = 0;
                }
                else
                {
                    num3 = (int) this.numericUpDown23.Value;
                }
                int index = 0x60;
                data[index] = (byte) (num3 & 0xff);
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
                data[index] = (byte) (num3 >> 8);
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                int num4 = 0x3000;
                if (this.comboBox59.SelectedIndex == 0)
                {
                    num4 = 0x2000;
                }
                else if (this.comboBox59.SelectedIndex == 1)
                {
                    num4 = 0x3000;
                }
                else if (this.comboBox59.SelectedIndex == 2)
                {
                    num4 = 0x38000;
                }
                else
                {
                    num4 = 0x3000;
                }
                BitConverter.GetBytes(num4);
                index = 100;
                data[index] = (byte) (num4 & 0xff);
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
                data[index] = (byte) (num4 >> 8);
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
                data[index] = (byte) (num4 >> 0x10);
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
                data[index] = (byte) (num4 >> 0x18);
                data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                controller.UpdateConfigureCPUSuperIP(data, "");
                WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH();
                tssi_flash.type = 0x21;
                tssi_flash.code = 0x30;
                tssi_flash.iDevSnFrom = 0;
                tssi_flash.iDevSnTo = (uint) this.txtSN.Value;
                tssi_flash.iCallReturn = 0;
                tssi_flash.ucData = new byte[0x400];
                using (wgUdpComm comm = new wgUdpComm())
                {
                    try
                    {
                        Thread.Sleep(300);
                        tssi_flash.iStartFlashAddr = 0x7f2000;
                        tssi_flash.iEndFlashAddr = 0x7f2fff;
                        for (int j = 0; j < 0x400; j++)
                        {
                            tssi_flash.ucData[j] = 0xff;
                        }
                        byte[] recv = null;
                        while (tssi_flash.iStartFlashAddr <= tssi_flash.iEndFlashAddr)
                        {
                            for (int k = 0; k < 0x400; k++)
                            {
                                tssi_flash.ucData[k] = this.webStringOther[(int) ((IntPtr) ((tssi_flash.iStartFlashAddr - 0x7f2000) + k))];
                            }
                            int num7 = comm.udp_get(tssi_flash.ToBytes(comm.udpPort), 300, tssi_flash.xid, this.defaultIP, this.defaultPORT, ref recv);
                            if (num7 < 0)
                            {
                                this.txtInfo.AppendText("button25 Err Ret=: " + num7.ToString() + "\r\n");
                                break;
                            }
                            tssi_flash.iStartFlashAddr += 0x400;
                            this.label107.Text = (tssi_flash.iStartFlashAddr / 0x400).ToString();
                        }
                        this.txtInfo.AppendText("button25 End: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "\r\n");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button74_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能停用, 由软件自身完成. 2012-3-28_14:36:20");
        }

        private void button76_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string fileName;
                this.openFileDialog1.Filter = " (*.xls)|*.xls| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                try
                {
                    this.openFileDialog1.InitialDirectory = @".\REPORT";
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
                this.openFileDialog1.Title = sender.ToString();
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                }
                else
                {
                    return;
                }
                this.MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
                this.DS = new DataSet();
                this.MyConnection.Open();
                this.MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                this.MyConnection.Close();
                string str2 = "public$";
                try
                {
                    string str3;
                    this.MyCommand = new OleDbDataAdapter("select * from [" + str2 + "A1:J65535]", this.MyConnection);
                    this.MyCommand.Fill(this.DS, "webInfo");
                    this.dataGridView2.DataSource = this.DS.Tables["webInfo"];
                    this.dv = new DataView(this.DS.Tables["webInfo"]);
                    int[] numArray = new int[this.dv.Count];
                    int num = 0x2000;
                    string text = "";
                    string str5 = "";
                    for (int i = 0; i <= (this.dv.Count - 1); i++)
                    {
                        numArray[i] = num;
                        text = text + string.Format("0x{0:X},", num);
                        str5 = str5 + string.Format("#define {0} ((char const*)(webStringLoc[{1:d}]))\r\n", wgTools.SetObjToStr(this.dv[i][1]).Trim(), i);
                        str3 = wgTools.SetObjToStr(this.dv[i][2]).Trim();
                        this.txtInfo.AppendText("0x");
                        byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer2 = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), bytes);
                        num = (num + buffer2.Length) + 1;
                        this.txtInfo.AppendText(BitConverter.ToString(buffer2).Replace("-", ",0x"));
                        this.txtInfo.AppendText(",0x00,");
                        this.txtInfo.AppendText("\r\n");
                    }
                    this.txtInfo.AppendText("0x00");
                    this.txtInfo.AppendText("\r\n");
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                    this.txtInfo.AppendText(str5);
                    string path = @"D:\works2011\ARM\StellarisWare6852\boards\BioAccess-9506WEB\Project\Debug\Exe\BioAccess.bin";
                    string str7 = @"D:\works2011\ARM\StellarisWare6852\boards\BioAccess-9506WEB\Project\Debug\Exe\BioAccessnew.bin";
                    BinaryWriter writer = null;
                    BinaryReader reader = null;
                    this.fsRd = new FileStream(path, FileMode.Open);
                    reader = new BinaryReader(this.fsRd);
                    this.fs = new FileStream(str7, FileMode.Create);
                    writer = new BinaryWriter(this.fs);
                    int num3 = 0;
                    while (num3 < 0x2000)
                    {
                        writer.Write(reader.ReadByte());
                        num3++;
                    }
                    num = 0x2400;
                    for (int j = 0; j <= (this.dv.Count - 1); j++)
                    {
                        writer.Write(BitConverter.GetBytes(num));
                        num3 += 4;
                        str3 = wgTools.SetObjToStr(this.dv[j][2]).Trim();
                        byte[] buffer3 = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer4 = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), buffer3);
                        num = (num + buffer4.Length) + 1;
                    }
                    byte num5 = 0;
                    while (num3 < 0x2400)
                    {
                        writer.Write(num5);
                        num3++;
                    }
                    for (int k = 0; k <= (this.dv.Count - 1); k++)
                    {
                        str3 = wgTools.SetObjToStr(this.dv[k][2]).Trim();
                        byte[] buffer5 = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), buffer5);
                        writer.Write(buffer);
                        writer.Write(num5);
                        num3 = (num3 + buffer.Length) + 1;
                    }
                    reader.BaseStream.Position = num3;
                    while (reader.BaseStream.Position < 0x3000L)
                    {
                        writer.Write(reader.ReadByte());
                    }
                    num3 = 0x3000;
                    num = 0x3400;
                    for (int m = 0; m <= (this.dv.Count - 1); m++)
                    {
                        writer.Write(BitConverter.GetBytes(num));
                        num3 += 4;
                        str3 = wgTools.SetObjToStr(this.dv[m][3]).Trim();
                        byte[] buffer7 = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer8 = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), buffer7);
                        num = (num + buffer8.Length) + 1;
                    }
                    while (num3 < 0x3400)
                    {
                        writer.Write(num5);
                        num3++;
                    }
                    for (int n = 0; n <= (this.dv.Count - 1); n++)
                    {
                        str3 = wgTools.SetObjToStr(this.dv[n][3]).Trim();
                        byte[] buffer9 = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer10 = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), buffer9);
                        writer.Write(buffer10);
                        writer.Write(num5);
                        num3 = (num3 + buffer10.Length) + 1;
                    }
                    reader.BaseStream.Position = num3;
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        writer.Write(reader.ReadByte());
                    }
                    reader.Close();
                    writer.Close();
                    this.fs.Dispose();
                    this.fsRd.Dispose();
                    for (int num9 = 0; num9 < this.webStringOther.Length; num9++)
                    {
                        this.webStringOther[num9] = num5;
                    }
                    num3 = 0x38000;
                    num = 0x38400;
                    for (int num10 = 0; num10 <= (this.dv.Count - 1); num10++)
                    {
                        this.webStringOther[num3 - 0x38000] = (byte) (num & 0xff);
                        this.webStringOther[(num3 - 0x38000) + 1] = (byte) (num >> 8);
                        this.webStringOther[(num3 - 0x38000) + 2] = (byte) (num >> 0x10);
                        this.webStringOther[(num3 - 0x38000) + 3] = (byte) (num >> 0x18);
                        num3 += 4;
                        str3 = wgTools.SetObjToStr(this.dv[num10][3]).Trim();
                        byte[] buffer11 = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer12 = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), buffer11);
                        num = (num + buffer12.Length) + 1;
                    }
                    num3 = 0x38400;
                    for (int num11 = 0; num11 <= (this.dv.Count - 1); num11++)
                    {
                        str3 = wgTools.SetObjToStr(this.dv[num11][3]).Trim();
                        byte[] buffer13 = Encoding.GetEncoding("gb2312").GetBytes(str3);
                        byte[] buffer14 = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("utf-8"), buffer13);
                        for (int num12 = 0; num12 < buffer14.Length; num12++)
                        {
                            this.webStringOther[(num3 - 0x38000) + num12] = buffer14[num12];
                        }
                        num3 = (num3 + buffer14.Length) + 1;
                    }
                    this.webStringOther[0x409] = 0;
                    this.dv.Dispose();
                    this.MyCommand.Dispose();
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
                this.DS.Dispose();
                this.MyConnection.Dispose();
            }
            catch (Exception exception3)
            {
                wgTools.WgDebugWrite(exception3.ToString(), new object[0]);
            }
            finally
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
                Cursor.Current = Cursors.Default;
            }
        }

        private void button77_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                wgMjControllerSwipeRecord swipe = null;
                controller.GetSingleSwipeRecord((int) this.numericUpDown24.Value, ref swipe);
                if (swipe != null)
                {
                    this.txtInfo.AppendText(swipe.ToDisplaySimpleInfo(true) + "\r\n");
                }
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button78_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgMjControllerPrivilege privilege = new wgMjControllerPrivilege();
            try
            {
                privilege.AllowDownload();
                if (this.dtPrivilege1 != null)
                {
                    this.dtPrivilege1.Rows.Clear();
                    this.dtPrivilege1.Dispose();
                    this.dtPrivilege1 = null;
                    GC.Collect();
                }
                this.dtPrivilege1 = new DataTable("Privilege");
                this.dtPrivilege1.Columns.Add("f_ConsumerID", System.Type.GetType("System.UInt32"));
                this.dtPrivilege1.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
                this.dtPrivilege1.Columns.Add("f_BeginYMD", System.Type.GetType("System.DateTime"));
                this.dtPrivilege1.Columns.Add("f_EndYMD", System.Type.GetType("System.DateTime"));
                this.dtPrivilege1.Columns.Add("f_PIN", System.Type.GetType("System.String"));
                this.dtPrivilege1.Columns.Add("f_ControlSegID1", System.Type.GetType("System.Byte"));
                this.dtPrivilege1.Columns["f_ControlSegID1"].DefaultValue = 0;
                this.dtPrivilege1.Columns.Add("f_ControlSegID2", System.Type.GetType("System.Byte"));
                this.dtPrivilege1.Columns["f_ControlSegID2"].DefaultValue = 0;
                this.dtPrivilege1.Columns.Add("f_ControlSegID3", System.Type.GetType("System.Byte"));
                this.dtPrivilege1.Columns["f_ControlSegID3"].DefaultValue = 0;
                this.dtPrivilege1.Columns.Add("f_ControlSegID4", System.Type.GetType("System.Byte"));
                this.dtPrivilege1.Columns["f_ControlSegID4"].DefaultValue = 0;
                this.dtPrivilege1.Columns.Add("f_ControlSegID5", System.Type.GetType("System.Byte"));
                this.dtPrivilege1.Columns["f_ControlSegID5"].DefaultValue = 0;
                this.dtPrivilege1.Columns.Add("f_ConsumerName", System.Type.GetType("System.String"));
                this.dtPrivilege1.Columns.Add("f_IsDeleted", System.Type.GetType("System.UInt32"));
                DataTable tbFpTempl = null;
                if (privilege.DownloadIP(this.defaultSN, this.defaultIP, this.defaultPORT, "INCLUDEDELETED", ref this.dtPrivilege1, ref tbFpTempl) <= 0)
                {
                    this.txtInfo.AppendText("failed\r\n");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                privilege.Dispose();
            }
            Cursor.Current = Cursors.Default;
            if ((this.dtPrivilege1 != null) && (this.dtPrivilege1.Rows.Count >= 0))
            {
                this.dtPrivilege1.Columns.Remove("f_BeginYMD");
                this.dtPrivilege1.Columns.Remove("f_EndYMD");
                this.dtPrivilege1.Columns.Remove("f_PIN");
                this.dtPrivilege1.Columns.Remove("f_ControlSegID1");
                this.dtPrivilege1.Columns.Remove("f_ControlSegID2");
                this.dtPrivilege1.Columns.Remove("f_ControlSegID3");
                this.dtPrivilege1.Columns.Remove("f_ControlSegID4");
                this.dtPrivilege1.Columns.Remove("f_ControlSegID5");
                this.dtPrivilege1.AcceptChanges();
                DataView view = new DataView(this.dtPrivilege1);
                this.dataGridView3.DataSource = view;
            }
        }

        private void button79_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcel(this.dataGridView3, this.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.numericUpDown2.Value = 3M;
        }

        private void button80_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox34.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void button82_Click(object sender, EventArgs e)
        {
            this.getResult();
        }

        private void button83_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.txtInfo.AppendText(now.ToString("hh:mm:ss.ffff") + "\r\n");
            icController controller = new icController();
            try
            {
                controller.ControllerSN = -1;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
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
                string text = string.Format("SN{3} : 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, controller.ControllerSN }) + "\r\n";
                this.txtInfo.AppendText(text);
                wgUdpComm.triesTotal = 0L;
                wgTools.WriteLine("control.Test1024 Start");
                int page = 0;
                string str2 = "";
                if (controller.test1024Write() < 0)
                {
                    str2 = str2 + "大数据包写入失败\r\n";
                }
                int num5 = controller.test1024Read(200, ref page);
                if (num5 < 0)
                {
                    str2 = str2 + "大数据包读取失败: " + num5.ToString() + "\r\n";
                }
                if (wgUdpComm.triesTotal > 0L)
                {
                    str2 = str2 + "测试中重试次数 = " + wgUdpComm.triesTotal.ToString() + "\r\n";
                }
                if (str2 == "")
                {
                    str2 = "大数据包测试成功(测试200次)";
                }
                this.txtInfo.AppendText(controller.ControllerSN.ToString() + ": " + str2 + "\r\n");
            }
            catch (Exception)
            {
            }
            controller.Dispose();
            DateTime time2 = DateTime.Now;
            TimeSpan span = (TimeSpan) (time2 - now);
            this.txtInfo.AppendText(string.Format("{0} [{1}秒] \r\n", time2.ToString("hh:mm:ss.ffff"), span.TotalSeconds));
        }

        private void button84_Click(object sender, EventArgs e)
        {
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.WarnResetIP();
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button85_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgMjControllerPrivilege privilege = new wgMjControllerPrivilege();
            try
            {
                privilege.AllowDownload();
                if (this.dtPrivilege != null)
                {
                    this.dtPrivilege.Rows.Clear();
                    this.dtPrivilege.Dispose();
                    this.dtPrivilege = null;
                    GC.Collect();
                }
                this.dtPrivilege = new DataTable("Privilege");
                this.dtPrivilege.Columns.Add("f_ConsumerID", System.Type.GetType("System.UInt32"));
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
                this.dtPrivilege.Columns.Add("f_ControlSegID5", System.Type.GetType("System.Byte"));
                this.dtPrivilege.Columns["f_ControlSegID5"].DefaultValue = 0;
                this.txtInfo.AppendText("开始读取");
                string str = DateTime.Now.ToString("yyyy-MM-dd hhmmssffff");
                for (int i = 0; i < 10; i++)
                {
                    this.dtPrivilege.Clear();
                    DataTable tbFpTempl = null;
                    if (privilege.DownloadIP(this.defaultSN, this.defaultIP, this.defaultPORT, "", ref this.dtPrivilege, ref tbFpTempl) > 0)
                    {
                        this.dtPrivilege.WriteXml(string.Format(@"logtemp\{0}_{1}.xml", str, i));
                        if (this.dtPrivilege.Rows.Count >= 0)
                        {
                            this.dataGridView1.DataSource = this.dtPrivilege;
                        }
                        else
                        {
                            this.txtInfo.AppendText("failed\r\n");
                        }
                    }
                    else
                    {
                        this.txtInfo.AppendText("failed\r\n");
                    }
                }
                this.txtInfo.AppendText("读取完成, 开始比较");
                for (int j = 0; j < 9; j++)
                {
                    if (!this.FileCompare(string.Format(@"logtemp\{0}_{1}.xml", str, j), string.Format(@"logtemp\{0}_{1}.xml", str, j + 1)))
                    {
                        this.txtInfo.AppendText("比较有错的第一个文件:  " + string.Format(@"logtemp\{0}_{1}.xml", str, j));
                    }
                }
                this.txtInfo.AppendText("比较完成");
            }
            catch (Exception)
            {
                this.txtInfo.AppendText("读取出错");
            }
            finally
            {
                if (privilege != null)
                {
                    privilege.Dispose();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void button86_Click(object sender, EventArgs e)
        {
            DirectoryInfo info = new DirectoryInfo(this.textBox34.Text);
            foreach (FileInfo info2 in info.GetFiles())
            {
                Compress(info2);
            }
        }

        private void button87_Click(object sender, EventArgs e)
        {
            DirectoryInfo info = new DirectoryInfo(this.textBox34.Text);
            foreach (FileInfo info2 in info.GetFiles("*.gz"))
            {
                Decompress(info2);
            }
        }

        private void button88_Click(object sender, EventArgs e)
        {
            IPAddress address = IPAddress.Parse("192.168.168.153");
            IPAddress address2 = IPAddress.Parse("255.255.255.0");
            IPAddress address3 = IPAddress.Parse("192.168.168.254");
            int ipstart = 0x99;
            int ipend = 0xfd;
            int cmdOption = 0x12;
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.AutoIPSetIP(cmdOption, address.ToString(), address2.ToString(), address3.ToString(), ipstart, ipend);
            }
            catch (Exception)
            {
            }
            controller.Dispose();
        }

        private void button89_Click(object sender, EventArgs e)
        {
            WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY();
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);
                int num2 = (int) this.numericUpDown27.Value;
                int num3 = 0;
                while (++num3 <= num2)
                {
                    tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.txtSN.Value, (uint) (this.numericUpDown25.Value * 1024M), (uint) ((this.numericUpDown25.Value * 1024M) + 1024M - 1));
                    byte[] cmd = tssi_flash_query.ToBytes(comm.udpPort);
                    byte[] recv = null;
                    int num = comm.udp_get(cmd, 300, tssi_flash_query.xid, this.defaultIP, this.defaultPORT, ref recv);
                    this.txtInfo.AppendText(num3 + "  ----");
                    if (num < 0)
                    {
                        this.txtInfo.AppendText(this.numericUpDown25.Value + "  failed????????????????");
                        this.txtInfo.AppendText("\r\n");
                        comm.Dispose();
                        return;
                    }
                    if (recv != null)
                    {
                        BitConverter.ToString(recv);
                        this.txtInfo.AppendText(this.numericUpDown25.Value + "  OK");
                        this.txtInfo.AppendText("\r\n");
                    }
                    tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.txtSN.Value, (uint) (this.numericUpDown26.Value * 1024M), (uint) ((this.numericUpDown26.Value * 1024M) + 1024M - 1));
                    cmd = tssi_flash_query.ToBytes(comm.udpPort);
                    recv = null;
                    num = comm.udp_get(cmd, 300, tssi_flash_query.xid, this.defaultIP, this.defaultPORT, ref recv);
                    this.txtInfo.AppendText(num3 + "  ----");
                    if (num < 0)
                    {
                        this.txtInfo.AppendText(this.numericUpDown26.Value + "  failed????????????????");
                        comm.Dispose();
                        return;
                    }
                    if (recv != null)
                    {
                        BitConverter.ToString(recv);
                        this.txtInfo.AppendText(this.numericUpDown26.Value + "  OK");
                        this.txtInfo.AppendText("\r\n");
                    }
                }
                wgTools.WriteLine(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.controlTaskList == null)
            {
                this.controlTaskList = new wgMjControllerTaskList();
            }
            MjControlTaskItem mjCI = new MjControlTaskItem();
            mjCI.ymdStart = this.dateTimePicker1.Value;
            mjCI.ymdEnd = this.dateTimePicker2.Value;
            mjCI.hms = this.dateTimePicker3.Value;
            mjCI.weekdayControl = 0;
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox43.Checked ? ((byte) 1) : ((byte) 0)));
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox44.Checked ? ((byte) 2) : ((byte) 0)));
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox45.Checked ? ((byte) 4) : ((byte) 0)));
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox46.Checked ? ((byte) 8) : ((byte) 0)));
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox47.Checked ? ((byte) 0x10) : ((byte) 0)));
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox48.Checked ? ((byte) 0x20) : ((byte) 0)));
            mjCI.weekdayControl = (byte) (mjCI.weekdayControl + (this.checkBox49.Checked ? ((byte) 0x40) : ((byte) 0)));
            mjCI.paramLoc = (int) this.numericUpDown1.Value;
            mjCI.paramValue = (byte) this.numericUpDown2.Value;
            this.listBox1.Items.Add(BitConverter.ToString(mjCI.ToBytes()));
            this.controlTaskList.AddItem(mjCI);
        }

        private void button90_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[0x480];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
            ulong maxValue = ulong.MaxValue;
            ulong result = ulong.MaxValue;
            ulong.TryParse(this.textBox37.Text, out maxValue);
            ulong.TryParse(this.textBox38.Text, out result);
            if (maxValue == 0L)
            {
                maxValue = ulong.MaxValue;
            }
            if (result == 0L)
            {
                result = ulong.MaxValue;
            }
            int index = 160;
            data[index] = (byte) (maxValue & ((ulong) 0xffL));
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 8);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 0x10);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 0x18);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 0x20);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 40);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 0x30);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (maxValue >> 0x38);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result & ((ulong) 0xffL));
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 8);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 0x10);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 0x18);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 0x20);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 40);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 0x30);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            data[index] = (byte) (result >> 0x38);
            data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            icController controller = new icController();
            try
            {
                controller.ControllerSN = this.defaultSN;
                controller.IP = this.defaultIP;
                controller.PORT = this.defaultPORT;
                controller.UpdateConfigureCPUSuperIP(data, "");
            }
            catch (Exception)
            {
            }
            controller.Dispose();
            this.txtInfo.AppendText((sender as Button).Text + "  OK");
            this.txtInfo.AppendText("\r\n");
        }

        private void button91_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string fileName;
                this.openFileDialog1.Filter = " (*.bin)|*.bin| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                try
                {
                    this.openFileDialog1.InitialDirectory = @".\REPORT";
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
                this.openFileDialog1.Title = sender.ToString();
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                }
                else
                {
                    return;
                }
                int num = int.Parse(this.comboBox60.Text);
                string path = fileName;
                string str3 = fileName + ".Txt";
                StreamWriter writer = null;
                BinaryReader reader = null;
                this.fsRd = new FileStream(path, FileMode.Open);
                reader = new BinaryReader(this.fsRd);
                this.fs = new FileStream(str3, FileMode.Create);
                writer = new StreamWriter(this.fs);
                int num2 = 0;
                int num3 = 0;
                long num4 = 0L;
                string str4 = "";
                int num5 = 0;
                writer.Write("#ifndef WEB_PAGE_0x2000 \r\n#define WEB_PAGE_0x2000 \r\n#pragma location = 0x2000\r\nconst  int chsStringLoc[]= { //实际数据 0x2400开始\r\n");
                while (num2 < (reader.BaseStream.Length / 2L))
                {
                    num4 += reader.ReadByte() << (8 * num3);
                    num3++;
                    if (num3 == num)
                    {
                        if (num5 > 0)
                        {
                            writer.Write(", ");
                            if ((num5 % 8) == 0)
                            {
                                writer.Write("\r\n");
                            }
                        }
                        str4 = string.Format("0x{0:X2}", num4);
                        if (str4.Length > 10)
                        {
                            str4 = str4.Replace("FFFFFFFF", "");
                        }
                        writer.Write(str4);
                        num5++;
                        num4 = 0L;
                        num3 = 0;
                    }
                    num2++;
                }
                writer.Write("};\r\n\r\n#pragma location = 0x3000\r\nconst  int enStringLoc[]= { //实际数据 0x3400开始\r\n");
                num5 = 0;
                while (num2 < reader.BaseStream.Length)
                {
                    num4 += reader.ReadByte() << (8 * num3);
                    num3++;
                    if (num3 == num)
                    {
                        if (num5 > 0)
                        {
                            writer.Write(", ");
                            if ((num5 % 8) == 0)
                            {
                                writer.Write("\r\n");
                            }
                        }
                        str4 = string.Format("0x{0:X2}", num4);
                        if (str4.Length > 10)
                        {
                            str4 = str4.Replace("FFFFFFFF", "");
                        }
                        writer.Write(str4);
                        num5++;
                        num3 = 0;
                        num4 = 0L;
                    }
                    num2++;
                }
                writer.Write("};\r\n\r\n#endif\r\n");
                reader.Close();
                writer.Close();
                this.fs.Dispose();
                this.fsRd.Dispose();
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            finally
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
                Cursor.Current = Cursors.Default;
            }
        }

        private void button92_Click(object sender, EventArgs e)
        {
            wgUdpComm comm = new wgUdpComm();
            try
            {
                byte[] destinationArray = new byte[] { 
                    0x19, 80, 0, 0, 0xb1, 0x98, 0xa7, 0x19, 1, 0, 0x70, 0, 0x20, 0x10, 1, 1, 
                    0x20, 0x29, 0x12, 0x31, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                 };
                byte[] recv = null;
                Array.Copy(BitConverter.GetBytes(this.defaultSN), 0, destinationArray, 4, 4);
                DateTime now = DateTime.Now;
                DateTime time2 = DateTime.Now;
                int num = 0xf423f;
                this.txtInfo.AppendText(string.Format("\r\n开始发出第一个:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                for (int i = 0; i < 0x2710; i++)
                {
                    now = DateTime.Now;
                    Array.Copy(BitConverter.GetBytes(num), 0, destinationArray, 8, 4);
                    num--;
                    comm.udp_get(destinationArray, 300, 0, this.defaultIP, this.defaultPORT, ref recv);
                    if (recv != null)
                    {
                        time2 = DateTime.Now;
                    }
                }
                this.txtInfo.AppendText(string.Format("\r\n开始发出:\t{0}", now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                this.txtInfo.AppendText(string.Format("\r\n接收时间:\t{0}", time2.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button93_Click(object sender, EventArgs e)
        {
            wgUdpComm comm = new wgUdpComm();
            try
            {
                byte[] destinationArray = new byte[] { 
                    0x19, 0x54, 0, 0, 0xb1, 0x98, 0xa7, 0x19, 0x55, 170, 170, 0x55, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                 };
                byte[] recv = null;
                Array.Copy(BitConverter.GetBytes(this.defaultSN), 0, destinationArray, 4, 4);
                comm.udp_get(destinationArray, 300, 0, this.defaultIP, this.defaultPORT, ref recv);
                this.txtInfo.AppendText(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                this.Refresh();
                if (recv != null)
                {
                    string text = BitConverter.ToString(recv);
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                    this.txtInfo.AppendText(string.Format("\r\n接收时间:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button94_Click(object sender, EventArgs e)
        {
            wgUdpComm comm = new wgUdpComm();
            try
            {
                int num;
                byte[] destinationArray = new byte[] { 
                    0x19, 0x40, 0, 0, 0xb1, 0x98, 0xa7, 0x19, 1, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                 };
                byte[] recv = null;
                if (!string.IsNullOrEmpty(this.textBox39.Text) && int.TryParse(this.textBox39.Text, NumberStyles.AllowHexSpecifier, null, out num))
                {
                    destinationArray[0] = (byte) num;
                }
                this.txtInfo.AppendText(string.Format("\r\n远程开门:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                Array.Copy(BitConverter.GetBytes(this.defaultSN), 0, destinationArray, 4, 4);
                comm.udp_get(destinationArray, 300, 0, this.defaultIP, this.defaultPORT, ref recv);
                this.txtInfo.AppendText(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                this.Refresh();
                if (recv != null)
                {
                    string text = BitConverter.ToString(recv);
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                    this.txtInfo.AppendText(string.Format("\r\n接收时间:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button95_Click(object sender, EventArgs e)
        {
            wgUdpComm comm = new wgUdpComm();
            try
            {
                byte[] destinationArray = new byte[] { 
                    0x19, 0x56, 0, 0, 0xb1, 0x98, 0xa7, 0x19, 1, 0, 0x70, 0, 0x20, 0x10, 1, 1, 
                    0x20, 0x29, 0x12, 0x31, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                 };
                byte[] recv = null;
                Array.Copy(BitConverter.GetBytes(this.defaultSN), 0, destinationArray, 4, 4);
                DateTime now = DateTime.Now;
                DateTime time2 = DateTime.Now;
                int num = 0xf423f;
                this.txtInfo.AppendText(string.Format("\r\n开始发出第一个:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                for (int i = 0; i < 0x2710; i++)
                {
                    now = DateTime.Now;
                    Array.Copy(BitConverter.GetBytes(num), 0, destinationArray, 8, 4);
                    num--;
                    comm.udp_get(destinationArray, 300, 0, this.defaultIP, this.defaultPORT, ref recv);
                    if (recv != null)
                    {
                        time2 = DateTime.Now;
                    }
                }
                this.txtInfo.AppendText(string.Format("\r\n开始发出:\t{0}", now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                this.txtInfo.AppendText(string.Format("\r\n接收时间:\t{0}", time2.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void button96_Click(object sender, EventArgs e)
        {
            wgUdpComm comm = new wgUdpComm();
            try
            {
                int num;
                byte[] destinationArray = new byte[] { 
                    0x1a, 0x40, 0, 0, 0xb1, 0x98, 0xa7, 0x19, 1, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                 };
                byte[] recv = null;
                this.txtInfo.AppendText(string.Format("\r\n远程开门:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                Array.Copy(BitConverter.GetBytes(this.defaultSN), 0, destinationArray, 4, 4);
                if (!string.IsNullOrEmpty(this.textBox39.Text) && int.TryParse(this.textBox39.Text, NumberStyles.AllowHexSpecifier, null, out num))
                {
                    destinationArray[0] = (byte) (num + 1);
                }
                byte[] buffer3 = new byte[destinationArray.Length - 4];
                Array.Copy(destinationArray, 4, buffer3, 0, buffer3.Length);
                ushort num2 = this.calCRC((uint) buffer3.Length, ref buffer3);
                destinationArray[2] = (byte) (num2 & 0xff);
                destinationArray[3] = (byte) ((num2 >> 8) & 0xff);
                comm.udp_get(destinationArray, 300, 0, this.defaultIP, this.defaultPORT, ref recv);
                this.txtInfo.AppendText(string.Format("\r\n开始发出:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                this.Refresh();
                if (recv != null)
                {
                    string text = BitConverter.ToString(recv);
                    this.txtInfo.AppendText(text);
                    this.txtInfo.AppendText("\r\n");
                    this.txtInfo.AppendText(string.Format("\r\n接收时间:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff")));
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private ushort calCRC(uint len, ref byte[] data)
        {
            uint num3 = 0xc78c;
            for (uint i = 0; i < len; i++)
            {
                num3 ^= (uint) (data[i] << 8);
                for (uint j = 0; j < 8; j++)
                {
                    if ((num3 & 0x8000) > 0)
                    {
                        num3 = (num3 << 1) ^ 0x1021;
                    }
                    else
                    {
                        num3 = num3 << 1;
                    }
                }
            }
            return (ushort) (num3 & 0xffff);
        }

        private void checkBox120_CheckedChanged(object sender, EventArgs e)
        {
            this.grpbIP.Visible = this.checkBox120.Checked;
            if (this.checkBox120.Checked)
            {
                this.defaultIP = this.txtControllerIP.Text;
                this.defaultPORT = (int) this.nudPort.Value;
            }
            else
            {
                this.defaultIP = "";
                this.defaultPORT = 0xea60;
            }
        }

        private void checkBox139_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void comboBox60_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public static void Compress(FileInfo fi)
        {
            using (FileStream stream = fi.OpenRead())
            {
                if (((System.IO.File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden) & (fi.Extension != ".gz"))
                {
                    using (FileStream stream2 = System.IO.File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream stream3 = new GZipStream(stream2, CompressionMode.Compress))
                        {
                            CopyStream(stream, stream3);
                        }
                    }
                }
            }
        }

        private string ConertStr(string str, string From, string To)
        {
            byte[] bytes = Encoding.GetEncoding(From).GetBytes(str);
            bytes = Encoding.Convert(Encoding.GetEncoding(From), Encoding.GetEncoding(To), bytes);
            return Encoding.GetEncoding(To).GetString(bytes);
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[0x8000];
            while (true)
            {
                int count = input.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    return;
                }
                output.Write(buffer, 0, count);
            }
        }

        public static void Decompress(FileInfo fi)
        {
            using (FileStream stream = fi.OpenRead())
            {
                string fullName = fi.FullName;
                using (FileStream stream2 = System.IO.File.Create(fullName.Remove(fullName.Length - fi.Extension.Length)))
                {
                    using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        CopyStream(stream3, stream2);
                    }
                }
            }
        }

        private void dgvControlConfure_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string str = this.dgvControlConfure[0, e.RowIndex].Value as string;
            if (!string.IsNullOrEmpty(str) && str.Equals("X"))
            {
                this.dgvControlConfure[0, e.RowIndex].Style.BackColor = Color.Red;
                this.dgvControlConfure[2, e.RowIndex].Style.BackColor = Color.Red;
            }
        }

        private void DisplayData(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    wgTools.WgDebugWrite("{0} = {1}", new object[] { column.ColumnName, row[column] });
                }
                wgTools.WgDebugWrite("============================", new object[0]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.control4Tcp != null))
            {
                this.control4Tcp.Dispose();
            }
            if (disposing && (this.control4Udp != null))
            {
                this.control4Udp.Dispose();
            }
            if (disposing && (this.fs != null))
            {
                this.fs.Dispose();
            }
            if (disposing && (this.fsRd != null))
            {
                this.fsRd.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void failsound()
        {
            try
            {
                this.player.Play();
            }
            catch (Exception)
            {
            }
        }

        private bool FileCompare(string file1, string file2)
        {
            int num = 0;
            int num2 = 0;
            if (file1 == file2)
            {
                return true;
            }
            using (FileStream stream = new FileStream(file1, FileMode.Open))
            {
                using (FileStream stream2 = new FileStream(file2, FileMode.Open))
                {
                    if (stream.Length == stream2.Length)
                    {
                        do
                        {
                            num = stream.ReadByte();
                            num2 = stream2.ReadByte();
                            if (num != num2)
                            {
                                goto Label_0062;
                            }
                        }
                        while (num != -1);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        Label_0062:
            return ((num - num2) == 0);
        }

        private void frmTestController_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
            wgAppConfig.UpdateKeyVal("PRODUCT_SPECIAL_CARDS", this.textBox32.Text);
        }

        private void frmTestController_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (((keyCode != Keys.Escape) && (keyCode == Keys.F12)) && this.checkBox115.Checked)
            {
                this.button28_Click(null, null);
            }
            if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
            {
                if (this.dfrmFind1 == null)
                {
                    this.dfrmFind1 = new dfrmFind();
                }
                this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
            }
        }

        private void frmTestController_Load(object sender, EventArgs e)
        {
            this.player = new SoundPlayer();
            this.player.SoundLocation = this.newSoundFile;
            if (this.Text == "Test Controller Center")
            {
                dfrmInputNewName name = new dfrmInputNewName();
                try
                {
                    name.setPasswordChar('@');
                    if (((name.ShowDialog(this) == DialogResult.OK) && (name.strNewName.Length >= ("wg".Length + 1))) && ((name.strNewName.Substring(0, "wg".Length) == "wg") && (name.strNewName.Substring("wg".Length, DateTime.Now.Hour.ToString().Length) == DateTime.Now.Hour.ToString())))
                    {
                        this.numericUpDown17.Value = wgUdpComm.CommTimeoutMsMin;
                        this.panel1.Visible = false;
                        if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_CARDS")))
                        {
                            this.textBox32.Text = wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_CARDS");
                        }
                        return;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    name.Dispose();
                }
                base.Close();
            }
        }

        private void getControlSetByUser(ref icController icCon)
        {
            icCon.ControllerSN = (int) this.txtSN.Value;
            if (this.checkBox120.Checked)
            {
                icCon.IP = this.txtControllerIP.Text;
                icCon.PORT = (int) this.nudPort.Value;
            }
            else
            {
                icCon.IP = "";
                icCon.PORT = 0xea60;
            }
        }

        private void getResult()
        {
            string text = this.textBox34.Text;
            string hasValue = this.textBox35.Text;
            string str3 = this.textBox36.Text;
            text = text.TrimEnd(new char[] { '\\' }) + @"\";
            if (Directory.Exists(text))
            {
                DirectoryInfo dI = new DirectoryInfo(text);
                str3 = this.result(dI, hasValue, str3, text);
            }
            MessageBox.Show("结果为(用|分割)：" + str3);
        }

        private void nudPort_ValueChanged(object sender, EventArgs e)
        {
            if (this.checkBox120.Checked)
            {
                this.defaultIP = this.txtControllerIP.Text;
                this.defaultPORT = (int) this.nudPort.Value;
            }
            else
            {
                this.defaultIP = "";
                this.defaultPORT = 0xea60;
            }
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
        }

        public void onlyProduce()
        {
            foreach (TabPage page in this.tabControl1.TabPages)
            {
                if (page.Name != "tabPage13")
                {
                    this.tabControl1.TabPages.Remove(page);
                }
            }
            this.checkBox120.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = false;
            this.button49.Visible = false;
            this.button1.Visible = false;
            this.button29.Visible = false;
            this.button34.Visible = false;
            this.button31.Visible = false;
            this.button32.Visible = false;
            this.button51.Visible = false;
            this.label114.Visible = false;
            this.button43.Visible = false;
            this.button24.Visible = false;
            this.button27.Visible = false;
            this.button56.Visible = false;
            this.label111.Visible = false;
            this.txtOldCommPassword.Visible = false;
            this.label110.Visible = false;
            this.txtCommPassword.Visible = false;
            this.button35.Visible = false;
            this.button36.Visible = false;
            this.button54.Visible = false;
            this.button44.Visible = false;
        }

        private string result(DirectoryInfo DI, string hasValue, string value, string path)
        {
            foreach (DirectoryInfo info in DI.GetDirectories())
            {
                value = value + this.result(info, hasValue, value, path);
            }
            ArrayList list = new ArrayList();
            foreach (FileInfo info2 in DI.GetFiles())
            {
                if (info2.Name.Contains(hasValue))
                {
                    list.Add(info2.FullName);
                }
            }
            foreach (string str in list)
            {
                System.IO.File.Move(str, str.Replace(@"\" + hasValue, @"\" + value));
            }
            return ("替换文件数:" + list.Count.ToString());
        }

        private void tabPage13_Click(object sender, EventArgs e)
        {
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            long num;
            if (long.TryParse(this.textBox29.Text.Trim().ToUpper().Replace("0X", ""), NumberStyles.AllowHexSpecifier, null, out num))
            {
                this.textBox30.Text = num.ToString();
            }
            else
            {
                this.textBox30.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.button43.PerformClick();
        }

        private void txtControllerIP_TextChanged(object sender, EventArgs e)
        {
            if (this.checkBox120.Checked)
            {
                this.defaultIP = this.txtControllerIP.Text;
                this.defaultPORT = (int) this.nudPort.Value;
            }
            else
            {
                this.defaultIP = "";
                this.defaultPORT = 0xea60;
            }
        }

        private void txtSN_ValueChanged(object sender, EventArgs e)
        {
            this.defaultSN = (int) this.txtSN.Value;
        }
    }
}

