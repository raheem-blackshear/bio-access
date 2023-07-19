using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
    public partial class dfrmUpgradeFirmware : frmBioAccess
    {
        [DllImport("kernel32")]
        public static extern int GetTickCount();

        private ArrayList cnt_serials = new ArrayList();
        private ArrayList cnt_ips = new ArrayList();
        private ArrayList cnt_ports = new ArrayList();
        private int serial;
        private string ip;
        private int port;
        private const uint FlashBlockSize = 0x1000;

        public dfrmUpgradeFirmware()
        {
            InitializeComponent();
        }

        private void fillControllerList()
        {
            DataTable tbControllers = new DataTable();
            DataView view = new DataView(tbControllers);

            string str;
            if (wgAppConfig.IsAccessDB)
            {
                str = "SELECT f_controllerNO, f_ControllerSN, f_IP, f_PORT FROM t_b_Controller";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(tbControllers);
                        }
                    }
                    goto l_fill;
                }
            }
            str = "SELECT f_controllerNO, f_ControllerSN, f_IP, f_PORT FROM t_b_Controller";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(tbControllers);
                    }
                }
            }
        l_fill:
            int no, serial_, port_;
            string ip_;

            for (int i = 0; i < view.Count; i++)
            {
                no = (int)view[i]["f_ControllerNO"];
                serial_ = (int)view[i]["f_ControllerSN"];
                ip_ = wgTools.SetObjToStr(view[i]["f_IP"]);
                port_ = (int)view[i]["f_Port"];
                cnt_serials.Add(serial_);
                cnt_ips.Add(ip_);
                cnt_ports.Add(port_);

                cmbController.Items.Add(no.ToString() + " : SN" + serial_.ToString());
            }
            if (cmbController.Items.Count != 0)
                cmbController.SelectedIndex = 0;
        }

        private void btnSelectFirmware_Click(object sender, EventArgs e)
        {
            comdlgOpen.Title = CommonStr.strSelectFirmware;//"请选择升级固件";
            comdlgOpen.Filter = CommonStr.strFirmwareFilter;// "固件(*.bin)|*.bin";
            comdlgOpen.DefaultExt = "bin";
            comdlgOpen.FileName = "";
            if (comdlgOpen.ShowDialog() == DialogResult.OK && comdlgOpen.FileName.Length > 0)
            {
                txtFirmware.Text = comdlgOpen.FileName;
            }
        }

        private void upgradeFirmware(byte[] data, uint size)
        {
            if (size == 0)
                return;

            lblStatus.Text = CommonStr.strUpgradingFirmware;

            WGPacketSSI_FLASH packet = new WGPacketSSI_FLASH();
            packet.type = 0x21;
            packet.code = 0x30;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint)serial;
            packet.iCallReturn = 0;
            wgUdpComm comm = new wgUdpComm();
            try
            {
                Thread.Sleep(300);

                byte[] recv = null;

                uint fwStartAddr = wgMjController.getFirmwareStartAddr(serial);
                uint fwEndAddr = fwStartAddr + size - 1;
                bool success = true;
                uint step = size / 100;
                packet.iStartFlashAddr = fwStartAddr;
                while (packet.iStartFlashAddr <= fwEndAddr)
                {
                    packet.iEndFlashAddr = packet.iStartFlashAddr + 0x3ff;
                    if (packet.iEndFlashAddr > fwEndAddr)
                        packet.iEndFlashAddr = fwEndAddr;
                    Application.DoEvents();
                    Array.Copy(data, packet.iStartFlashAddr - fwStartAddr,
                        packet.ucData, 0, packet.iEndFlashAddr - packet.iStartFlashAddr + 1);
                    comm.udp_get(packet.ToBytes(comm.udpPort), 3000, packet.xid, ip, port, ref recv);
                    if (recv == null || recv[0x10] == 0)
                    {
                        success = false;
                        XMessageBox.Show(this, CommonStr.strCommError/*"传送数据错误！"*/, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        lblStatus.Text = CommonStr.strCommError;
                        Application.DoEvents();
                        break;
                    }
                    packet.iStartFlashAddr += 0x400;
                    progUpgrading.Value = (int)((packet.iEndFlashAddr - fwStartAddr) / step);
                }
                if (success)
                {
                    progUpgrading.Value = 100;
                    WGPacketSSI_FLASH_QUERY writePacket = new WGPacketSSI_FLASH_QUERY();
                    writePacket.type = 0x20;
                    writePacket.code = 0xd0;
                    writePacket.iDevSnFrom = 0;
                    writePacket.iDevSnTo = (uint)serial;
                    writePacket.iStartFlashAddr = fwStartAddr;
                    writePacket.iEndFlashAddr = fwEndAddr;
                    writePacket.iCallReturn = 0;
                    comm.udp_get(writePacket.ToBytes(comm.udpPort), 1000, writePacket.xid, ip, port, ref recv);
                    if (recv != null)
                    {
                        lblStatus.Text = CommonStr.strDontPoweroff;//"开始写入固件，请不要拔掉机器的电源！"
                        int startTime = GetTickCount(), wait;
                        switch (wgMjController.getDeviceType(serial))
                        {
                            case wgMjController.DeviceType.A30_AC:
                                wait = 10000; break;
                            case wgMjController.DeviceType.A50_AC:
                                wait = 60000; break;
                            case wgMjController.DeviceType.F300_AC:
                                wait = 7000; break;
                            default:
                                wait = 0; break;
                        }
                        while (GetTickCount() - startTime < wait)
                        {
                            Application.DoEvents();
                            Thread.Sleep(50);
                        }
                        lblStatus.Text = CommonStr.strUpgradeSuccess;//"升级固件成功！";
                        Application.DoEvents();
                        XMessageBox.Show(this, CommonStr.strUpgradeSuccess, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        lblStatus.Text = CommonStr.strUpgradeFailed;//"写入固件错误！";
                        Application.DoEvents();
                        XMessageBox.Show(this, CommonStr.strUpgradeFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception)
            {
            }
            comm.Dispose();
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            btnUpgrade.Enabled = false;
            btnClose.Enabled = false;
            cmbController.Enabled = false;
            txtFirmware.Enabled = false;
            btnSelectFirmware.Enabled = false;

            int index = cmbController.SelectedIndex;
            if (index < 0)
            {
                lblStatus.Text = CommonStr.strSelectController;
            }
            else if (txtFirmware.Text == "")
            {
                lblStatus.Text = CommonStr.strSelectFirmware;
            }
            else
            {
                serial = (int)cnt_serials[index];
                port = (int)cnt_ports[index];
                ip = (string)cnt_ips[index];
                
                uint fwMaxSize = wgMjController.getFirmwareMaxSize(serial);
                uint fwMagic = wgMjController.getFirmwareMagic(serial);

                try
                {
                    using (FileStream fs = new FileStream(txtFirmware.Text, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader r = new BinaryReader(fs))
                        {
                            uint fw_size = (uint)fs.Length;
                            if (fw_size > fwMaxSize + 4 || fw_size < 4)
                            {
                                XMessageBox.Show(this, CommonStr.strInvalidSize/*"固件大小不合适！"*/, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else if (r.ReadUInt32() != fwMagic)
                            {
                                XMessageBox.Show(this, CommonStr.strInvalidFirmware/*"固件不符合！"*/, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                fw_size -= sizeof(uint);
                                uint size = (fw_size + FlashBlockSize - 1) / FlashBlockSize * FlashBlockSize;
                                byte[] data = new byte[size];
                                Array.Clear(data, 0, (int)size);
                                r.Read(data, 0, (int)fw_size);
                                upgradeFirmware(data, size);
                            }
                        }
                    }
                }
                catch (System.Exception)
                {

                }
            }

            btnUpgrade.Enabled = true;
            btnClose.Enabled = true;
            cmbController.Enabled = true;
            txtFirmware.Enabled = true;
            btnSelectFirmware.Enabled = true;
        }

        private void dfrmUpgradeFirmware_Load(object sender, EventArgs e)
        {
            fillControllerList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void dfrmUpgradeFirmware_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !btnClose.Enabled;
        }
    }
}
