using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
    public partial class dfrmRegisterFingerprint : frmBioAccess
    {
        [DllImport("kernel32")]
        public static extern int GetTickCount();

        private const int ImageWidth = 256;
        private const int ImageHeight = 256;
        private const int TimeoutOnce = 7000;
        private const uint ChunkSize = 0x400;
        private const int FpCount = 3;

        private wgUdpComm wgudp;
        private ArrayList cnt_serials = new ArrayList();
        private ArrayList cnt_ips = new ArrayList();
        private ArrayList cnt_ports = new ArrayList();
        private int serial;
        private string ip;
        private int port;
        private List<MjFpTempl> _fp_templ = new List<MjFpTempl>();

        public dfrmRegisterFingerprint(List<MjFpTempl> t)
        {
            InitializeComponent();

            // Fingerprint templates
            foreach (MjFpTempl e in t)
                _fp_templ.Add(new MjFpTempl(e));
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
                if (wgMjController.isFingerprintFeasible(serial_))
                {
                    cnt_serials.Add(serial_);
                    cnt_ips.Add(ip_);
                    cnt_ports.Add(port_);

                    cmbController.Items.Add(no.ToString() + " : SN" + serial_.ToString());
                }
            }
            if (view.Count != 0)
                cmbController.SelectedIndex = 0;
        }

        private void setEnableButtons(int finger, bool scan)
        {
            switch (finger)
            {
                case 0:
                    btnScan1.Enabled = (cmbController.Items.Count > 0) && scan;
                    btnDelete1.Enabled = !scan;
                    break;

                case 1:
                    btnScan2.Enabled = (cmbController.Items.Count > 0) && scan;
                    btnDelete2.Enabled = !scan;
                    break;
                case 2:
                    btnScan3.Enabled = (cmbController.Items.Count > 0) && scan;
                    btnDelete3.Enabled = !scan;
                    break;
            }
        }

        private void setDuressCheck(int finger, bool state)
        {
            switch (finger)
            {
                case 0: chkDuress1.Checked = state; break;
                case 1: chkDuress2.Checked = state; break;
                case 2: chkDuress3.Checked = state; break;
            }
        }

        private void dfrmRegisterFingerprint_Load(object sender, EventArgs e)
        {
            fillControllerList();

            // Uncheck all duresses
            for (int i = 0; i < FpCount; i++)
            {
                setDuressCheck(i, false);
                setEnableButtons(i, true);
            }

            // Render current states
            foreach (MjFpTempl t in _fp_templ)
            {
                setDuressCheck(t.finger, t.duress);
                setEnableButtons(t.finger, false);
                PictureBox pic = getPicBox(t.finger);
                if (pic != null)
                    pic.Image = global::Properties.Resources.finger;
            }
        }

        private bool getDuressCheck(int finger)
        {
            bool duress = false;
            switch (finger)
            {
                case 0: duress = chkDuress1.Checked; break;
                case 1: duress = chkDuress2.Checked; break;
                case 2: duress = chkDuress3.Checked; break;
            }
            return duress;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (MjFpTempl t in _fp_templ)
                t.duress = getDuressCheck(t.finger);

            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        /************************************************************************/
        /* Enroll Fingerprint                                                   */
        /************************************************************************/
        private int startEnroll(int finger)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            // send fp enroll start
            WGPacketFpQuery fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdStartEnrollFp, (uint)serial, 0, (uint)finger, 0);
            byte[] recv = null;

            if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                    fpQuery.xid, ip, port, ref recv) < 0 || recv == null)
                return wgTools.ErrorCode.ERR_FAIL;

            return wgTools.ErrorCode.ERR_SUCCESS;
        }

        private int doCaptureImage()
        {
            WGPacketFpQuery fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdCaptureFp, (uint)serial, 0, 0, 0);
            byte[] recv = null;

            if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                    fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                !fpQuery.get(recv, 0x14))
                return wgTools.ErrorCode.ERR_FAIL;

            return (int)fpQuery.tag;
        }

        private int captureImage(int timeout)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            
            if (timeout != 0)
            {
                int startTime = GetTickCount();
                while (true)
                {
                    int ret = doCaptureImage();

                    if (ret != wgTools.ErrorCode.ERR_NOT_PRESSED)
                        return ret;

                    if (GetTickCount() - startTime > timeout)
                        return wgTools.ErrorCode.ERR_CAP_TIMEOUT;

                    Thread.Sleep(50);
                }
            }

            return doCaptureImage();
        }

        private int getImage256(int finger)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            WGPacketFpQuery fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdGetImage256, (uint)serial, 0, 0, 0);
            byte[] recv = null;

            if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                !fpQuery.get(recv, 0x14))
                return wgTools.ErrorCode.ERR_FAIL;

            if (fpQuery.tag != wgTools.ErrorCode.ERR_SUCCESS)
                return (int)fpQuery.tag;

            // fetch image data
            fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdFetchImage256, (uint)serial, 0, 0, 0);
            WGPacketWith1024 packet = new WGPacketWith1024();
            byte[] imageData = new byte[ImageWidth * ImageHeight];
            for (uint i = 0; i < ImageWidth * ImageHeight / ChunkSize; i++)
            {
                fpQuery.index = i;
                fpQuery.GetNewXid();
                if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                    fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                    !packet.get(recv, 0x14))
                    return wgTools.ErrorCode.ERR_FAIL;

                Array.Copy(packet.ucData, 0, imageData, i * ChunkSize, ChunkSize);
            }
            drawFinger(finger, imageData);
            return wgTools.ErrorCode.ERR_SUCCESS;
        }

        private PictureBox getPicBox(int finger)
        {
            PictureBox pic = null;

            switch (finger)
            {
                case 0: pic = picFinger1; break;
                case 1: pic = picFinger2; break;
                case 2: pic = picFinger3; break;
            }

            return pic;
        }

        private void clearImage(int finger)
        {
            PictureBox pic = getPicBox(finger);
            if (pic != null)
                pic.Image = null;
        }

        private void drawFinger(int finger, byte[] data)
        {
            int x, y;
            Color newColor;
            Bitmap bmpFinger = null;
            int dx, dy;
            PictureBox pic = getPicBox(finger);

            if (pic != null)
            {
                dx = (pic.Width - ImageWidth) / 2;
                dy = (pic.Height - ImageHeight) / 2;
                try
                {
                    bmpFinger = new Bitmap(pic.Width, pic.Height);

                    for (x = 0; x < ImageWidth; x++)
                    {
                        for (y = 0; y < ImageHeight; y++)
                        {
                            newColor = Color.FromArgb(255, data[y * ImageWidth + x],
                                                           data[y * ImageWidth + x],
                                                           data[y * ImageWidth + x]);
                            bmpFinger.SetPixel(dx + x, y + dy/*(pic.Height - y - dy - 1)*/, newColor);
                        }
                    }
                }
                catch (System.Exception)
                {
                    XMessageBox.Show(this, CommonStr.strCheckPixelFormat, wgTools.MSGTITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                pic.Image = bmpFinger;
            }
        }

        private int enrollNth(int turn)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            WGPacketFpQuery fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdEnrollNthFp, (uint)serial, 0, (uint)turn, 0);
            byte[] recv = null;

            if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                !fpQuery.get(recv, 0x14))
                return wgTools.ErrorCode.ERR_FAIL;

            return (int)fpQuery.tag;
        }

        private int pressedFinger()
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            WGPacketFpQuery fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdPressedFp, (uint)serial, 0, 0, 0);
            byte[] recv = null;

            if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                !fpQuery.get(recv, 0x14))
                return wgTools.ErrorCode.ERR_FAIL;

            return (int)fpQuery.tag;
        }

        private int endEnroll(int finger, bool duress)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            WGPacketFpQuery fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdEndEnrollFp, (uint)serial, 0, 0, 0);
            byte[] recv = null;

            if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                    fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                !fpQuery.get(recv, 0x14))
                return wgTools.ErrorCode.ERR_FAIL;

            if (fpQuery.tag != wgTools.ErrorCode.ERR_SUCCESS)
                return (int)fpQuery.tag;

            // fetch fingerprint template
            fpQuery = new WGPacketFpQuery((byte)CmdCode.FpEnrollTypeCode,
                (byte)CmdCode.CmdFetchFpTempl, (uint)serial, 0, 0, 0);
            WGPacketWith1024 packet = new WGPacketWith1024();
            byte[] data = new byte[MjFpTempl.Length];
            for (uint i = 0; i < 2; i++)
            {
                fpQuery.index = i;
                fpQuery.GetNewXid();
                if (wgudp.udp_get(fpQuery.ToBytes(wgudp.udpPort), 1000,
                        fpQuery.xid, ip, port, ref recv) < 0 || recv == null ||
                    !packet.get(recv, 0x14))
                    return wgTools.ErrorCode.ERR_FAIL;

                uint len = (i != MjFpTempl.Length / ChunkSize) ? ChunkSize : (uint)MjFpTempl.Length % ChunkSize;

                Array.Copy(packet.ucData, 0, data, i * ChunkSize, len);
            }
            MjFpTempl templ = new MjFpTempl(0, finger, data, duress);
            _fp_templ.Add(templ);

            return wgTools.ErrorCode.ERR_SUCCESS;
        }

        private void report_result(int error)
        {
            lblResult.Text = wgGlobal.getErrorString(error);
            Application.DoEvents();
        }

        private bool getDuress(int finger)
        {
            bool duress = false;
            switch (finger)
            {
                case 0: duress = chkDuress1.Checked; break;
                case 1: duress = chkDuress2.Checked; break;
                case 2: duress = chkDuress3.Checked; break;
            }
            return duress;
        }

        private void scanFinger(int finger)
        {
            int res, index = cmbController.SelectedIndex;
            serial = (int)cnt_serials[index];
            port = (int)cnt_ports[index];
            ip = (string)cnt_ips[index];

            res = startEnroll(finger);
            if (res != wgTools.ErrorCode.ERR_SUCCESS)
                goto l_error;

            for (int i = 0; i <= 2; i++)
            {
                lblResult.Text = CommonStr.strPlaceFinger + (i + 1).ToString();
                Application.DoEvents();
                res = captureImage(TimeoutOnce);
                if (res != wgTools.ErrorCode.ERR_SUCCESS)
                    goto l_error;

                res = getImage256(finger);
                if (res != wgTools.ErrorCode.ERR_SUCCESS)
                    goto l_error;

                res = enrollNth(i + 1);
                if (res != wgTools.ErrorCode.ERR_SUCCESS)
                    goto l_error;

                // wait till take fingerprint off.
                if (i < 2)
                {
                    lblResult.Text = CommonStr.strTakeoffFinger;
                    Application.DoEvents();
                    int tick = GetTickCount();
                    while (true)
                    {
                        res = pressedFinger();
                        if (res != wgTools.ErrorCode.ERR_SUCCESS)
                            break;

                        // if user does not take finger off for 1 minutes.
                        if (GetTickCount() - tick > 60 * 1000)
                            return;
                    }

                }
            }
            res = endEnroll(finger, getDuress(finger));
            if (res == wgTools.ErrorCode.ERR_SUCCESS)
            {
                lblResult.Text = CommonStr.strFpEnrolledSuccessfully;
                setEnableButtons(finger, false);
                return;
            }
l_error:
            report_result(res);
        }
        
        private void delFinger(int finger)
        {
            MjFpTempl templ = _fp_templ.Find(delegate(MjFpTempl bk) { return bk.finger == finger; });
            if (_fp_templ.Remove(templ))
            {
                lblResult.Text = CommonStr.strFpRemovedSuccessfully;
                setEnableButtons(finger, true);
                setDuressCheck(finger, false);
            }
            else
            {
                lblResult.Text = CommonStr.strFailed;
            }
        }

        private void btnScan1_Click(object sender, EventArgs e)
        {
            scanFinger(0);
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            delFinger(0);
            clearImage(0);
        }

        private void btnScan2_Click(object sender, EventArgs e)
        {
            scanFinger(1);
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            delFinger(1);
            clearImage(1);
        }

        private void btnScan3_Click(object sender, EventArgs e)
        {
            scanFinger(2);
        }

        private void btnDelete3_Click(object sender, EventArgs e)
        {
            delFinger(2);
            clearImage(2);
        }

        private enum CmdCode
        {
            FpEnrollTypeCode = 0x26,
            CmdStartEnrollFp = 0x10,
            CmdCaptureFp = 0x20,
            CmdGetImage256 = 0x30,
            CmdFetchImage256 = 0x40,
            CmdEnrollNthFp = 0x50,
            CmdPressedFp = 0x60,
            CmdEndEnrollFp = 0x70,
            CmdFetchFpTempl = 0x80,
        }

        public List<MjFpTempl> fp_templ
        {
            get { return _fp_templ; }
            set { _fp_templ = value; }
        }
    }
}
