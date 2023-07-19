using System;
using System.Collections;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
    public partial class dfrmController : frmBioAccess
    {
        [DllImport("kernel32")]
        public static extern int GetTickCount();

        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        public bool bEditZone;
        private icController m_Controller;
        private int m_ControllerID;
        private bool m_ControllerTypeChanged;
        private bool m_OperateNew = true;
        private wgUdpComm wgudp;
        private Point prevDoorReaderLoc;
        private Size prevWndSize;

        public dfrmController()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int num;
            string str;
            this.mtxtbControllerNO.Text = this.mtxtbControllerNO.Text.Replace(" ", "");
            this.mtxtbControllerSN.Text = this.mtxtbControllerSN.Text.Replace(" ", "");
            if (!int.TryParse(this.mtxtbControllerNO.Text, out num))
            {
                XMessageBox.Show(this, CommonStr.strIDWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if ((int.Parse(this.mtxtbControllerNO.Text) > 0x186a0) || (int.Parse(this.mtxtbControllerNO.Text) < 0))
            {
                XMessageBox.Show(this, CommonStr.strIDWrong + ", <1000000 , >0", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!int.TryParse(this.mtxtbControllerSN.Text, out num))
            {
                XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text)) == 0)
            {
                XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.optIPLarge.Checked && string.IsNullOrEmpty(this.txtControllerIP.Text))
            {
                XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (chkWiFiComm.Checked)
            {
                if (string.IsNullOrEmpty(txtWiFiSSID.Text))
                {
                    XMessageBox.Show(this, CommonStr.strWiFiSsidWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (string.IsNullOrEmpty(txtWiFiIP.Text) ||
                    string.IsNullOrEmpty(txtWiFiMask.Text))
                {
                    XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (this.m_OperateNew)
            {
                if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), 0))
                {
                    XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (icController.IsExisted2NO(int.Parse(this.mtxtbControllerNO.Text), 0))
                {
                    XMessageBox.Show(this, CommonStr.strControllerNOAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), this.m_ControllerID))
                {
                    XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (icController.IsExisted2NO(int.Parse(this.mtxtbControllerNO.Text), this.m_ControllerID))
                {
                    XMessageBox.Show(this, CommonStr.strControllerNOAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (this.optIPLarge.Checked)
            {
                this.txtControllerIP.Text = this.txtControllerIP.Text.Replace(" ", "");
                if (string.IsNullOrEmpty(this.txtControllerIP.Text))
                {
                    XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (chkWiFiComm.Checked)
            {
                txtWiFiSSID.Text = txtWiFiSSID.Text.Replace(" ", "");
                txtWiFiIP.Text = txtWiFiIP.Text.Replace(" ", "");
                txtWiFiMask.Text = txtWiFiMask.Text.Replace(" ", "");
                if (string.IsNullOrEmpty(this.txtWiFiSSID.Text))
                {
                    XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            int controllerType = wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text));
            if (controllerType > 0)
            {
                switch (controllerType)
                {
                    case 1:
                        this.tabControl1.Controls.Remove(this.tabPage1);
                        this.tabPage1.Dispose();
                        this.tabControl1.Controls.Remove(this.tabPage2);
                        this.tabPage2.Dispose();
                        if ((int.Parse(this.mtxtbControllerSN.Text) < 0xa21fe80) || (int.Parse(this.mtxtbControllerSN.Text) > 0xaba94ff))
                        {
                            goto Label_050C;
                        }
                        this.label43.Text = CommonStr.strElevatorName;
                        this.tabPage3.Text = CommonStr.strElevatorController;
                        this.txtReaderName1A.Text = CommonStr.strElevator;
                        if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) != 2)
                        {
                            if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 3)
                            {
                                this.tabPage3.Text = CommonStr.strElevatorController3;
                                this.txtReaderName1A.Text = CommonStr.strElevator3;
                            }
                            break;
                        }
                        this.tabPage3.Text = CommonStr.strElevatorController2;
                        this.txtReaderName1A.Text = CommonStr.strElevator2;
                        break;

                    case 2:
                        this.tabControl1.Controls.Remove(this.tabPage1);
                        this.tabPage1.Dispose();
                        this.tabControl1.Controls.Remove(this.tabPage3);
                        this.tabPage3.Dispose();
                        goto Label_050C;

                    case 4:
                        this.tabControl1.Controls.Remove(this.tabPage2);
                        this.tabPage2.Dispose();
                        this.tabControl1.Controls.Remove(this.tabPage3);
                        this.tabPage3.Dispose();
                        goto Label_050C;

                    default:
                        this.tabControl1.Controls.Remove(this.tabPage2);
                        this.tabPage2.Dispose();
                        this.tabControl1.Controls.Remove(this.tabPage3);
                        this.tabPage3.Dispose();
                        goto Label_050C;
                }
                this.label36.Visible = false;
                this.label37.Visible = false;
                this.label38.Visible = false;
                this.label42.Visible = false;
                this.label44.Visible = false;
                this.groupBox22.Visible = false;
                this.nudDoorDelay1A.Visible = false;
                this.txtReaderName2A.Visible = false;
                this.chkAttend2A.Visible = false;
            }
        Label_050C:
            if (this.m_OperateNew || (wgMjController.GetControllerType(this.m_Controller.ControllerSN) != wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text))))
            {
                this.m_ControllerTypeChanged = true;
                switch (controllerType)
                {
                    case 1:
                        this.txtDoorName1A.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName1A.Text;
                        goto Label_13D5;

                    case 2:
                        this.txtDoorName1B.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName1B.Text;
                        this.txtDoorName2B.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName2B.Text;
                        goto Label_13D5;
                }
            }
            else
            {
                this.m_ControllerTypeChanged = false;
                switch (controllerType)
                {
                    case 1:
                        this.chkDoorActive1A.Checked = this.m_Controller.GetDoorActive(1);
                        this.optOnline1A.Checked = this.m_Controller.GetDoorControl(1) == 3;
                        this.optNO1A.Checked = this.m_Controller.GetDoorControl(1) == 1;
                        this.optNC1A.Checked = this.m_Controller.GetDoorControl(1) == 2;
                        this.nudDoorDelay1A.Value = this.m_Controller.GetDoorDelay(1);
                        this.txtReaderName1A.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(1), this.m_Controller.GetDoorName(1) + "-");
                        this.chkAttend1A.Checked = this.m_Controller.GetReaderAsAttendActive(1);
                        this.optDutyOnOff1A.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 3;
                        this.optDutyOn1A.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 2;
                        this.optDutyOff1A.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 1;
                        this.txtReaderName2A.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(2), this.m_Controller.GetDoorName(1) + "-");
                        this.chkAttend2A.Checked = this.m_Controller.GetReaderAsAttendActive(2);
                        this.optDutyOnOff2A.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 3;
                        this.optDutyOn2A.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 2;
                        this.optDutyOff2A.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 1;
                        if (this.m_Controller.ControllerSN == int.Parse(this.mtxtbControllerSN.Text))
                        {
                            this.txtDoorName1A.Text = this.m_Controller.GetDoorName(1);
                        }
                        else
                        {
                            this.txtDoorName1A.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(1), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                        }
                        goto Label_13D5;

                    case 2:
                        this.chkDoorActive1B.Checked = this.m_Controller.GetDoorActive(1);
                        this.optOnline1B.Checked = this.m_Controller.GetDoorControl(1) == 3;
                        this.optNO1B.Checked = this.m_Controller.GetDoorControl(1) == 1;
                        this.optNC1B.Checked = this.m_Controller.GetDoorControl(1) == 2;
                        this.nudDoorDelay1B.Value = this.m_Controller.GetDoorDelay(1);
                        this.txtReaderName1B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(1), this.m_Controller.GetDoorName(1) + "-");
                        this.chkAttend1B.Checked = this.m_Controller.GetReaderAsAttendActive(1);
                        this.optDutyOnOff1B.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 3;
                        this.optDutyOn1B.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 2;
                        this.optDutyOff1B.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 1;
                        this.txtReaderName2B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(2), this.m_Controller.GetDoorName(1) + "-");
                        this.chkAttend2B.Checked = this.m_Controller.GetReaderAsAttendActive(2);
                        this.optDutyOnOff2B.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 3;
                        this.optDutyOn2B.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 2;
                        this.optDutyOff2B.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 1;
                        this.chkDoorActive2B.Checked = this.m_Controller.GetDoorActive(2);
                        this.optOnline2B.Checked = this.m_Controller.GetDoorControl(2) == 3;
                        this.optNO2B.Checked = this.m_Controller.GetDoorControl(2) == 1;
                        this.optNC2B.Checked = this.m_Controller.GetDoorControl(2) == 2;
                        this.nudDoorDelay2B.Value = this.m_Controller.GetDoorDelay(2);
                        this.txtReaderName3B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(3), this.m_Controller.GetDoorName(2) + "-");
                        this.chkAttend3B.Checked = this.m_Controller.GetReaderAsAttendActive(3);
                        this.optDutyOnOff3B.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 3;
                        this.optDutyOn3B.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 2;
                        this.optDutyOff3B.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 1;
                        this.txtReaderName4B.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(4), this.m_Controller.GetDoorName(2) + "-");
                        this.chkAttend4B.Checked = this.m_Controller.GetReaderAsAttendActive(4);
                        this.optDutyOnOff4B.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 3;
                        this.optDutyOn4B.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 2;
                        this.optDutyOff4B.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 1;
                        if (this.m_Controller.ControllerNO == int.Parse(this.mtxtbControllerNO.Text))
                        {
                            this.txtDoorName1B.Text = this.m_Controller.GetDoorName(1);
                            this.txtDoorName2B.Text = this.m_Controller.GetDoorName(2);
                        }
                        else
                        {
                            this.txtDoorName1B.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(1), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                            this.txtDoorName2B.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(2), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                        }
                        goto Label_13D5;
                }
                this.chkDoorActive1D.Checked = this.m_Controller.GetDoorActive(1);
                this.optOnline1D.Checked = this.m_Controller.GetDoorControl(1) == 3;
                this.optNO1D.Checked = this.m_Controller.GetDoorControl(1) == 1;
                this.optNC1D.Checked = this.m_Controller.GetDoorControl(1) == 2;
                this.nudDoorDelay1D.Value = this.m_Controller.GetDoorDelay(1);
                this.chkDoorActive2D.Checked = this.m_Controller.GetDoorActive(2);
                this.optOnline2D.Checked = this.m_Controller.GetDoorControl(2) == 3;
                this.optNO2D.Checked = this.m_Controller.GetDoorControl(2) == 1;
                this.optNC2D.Checked = this.m_Controller.GetDoorControl(2) == 2;
                this.nudDoorDelay2D.Value = this.m_Controller.GetDoorDelay(2);
                this.chkDoorActive3D.Checked = this.m_Controller.GetDoorActive(3);
                this.optOnline3D.Checked = this.m_Controller.GetDoorControl(3) == 3;
                this.optNO3D.Checked = this.m_Controller.GetDoorControl(3) == 1;
                this.optNC3D.Checked = this.m_Controller.GetDoorControl(3) == 2;
                this.nudDoorDelay3D.Value = this.m_Controller.GetDoorDelay(3);
                this.chkDoorActive4D.Checked = this.m_Controller.GetDoorActive(4);
                this.optOnline4D.Checked = this.m_Controller.GetDoorControl(4) == 3;
                this.optNO4D.Checked = this.m_Controller.GetDoorControl(4) == 1;
                this.optNC4D.Checked = this.m_Controller.GetDoorControl(4) == 2;
                this.nudDoorDelay4D.Value = this.m_Controller.GetDoorDelay(4);
                this.txtReaderName1D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(1), this.m_Controller.GetDoorName(1) + "-");
                this.chkAttend1D.Checked = this.m_Controller.GetReaderAsAttendActive(1);
                this.optDutyOnOff1D.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 3;
                this.optDutyOn1D.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 2;
                this.optDutyOff1D.Checked = this.m_Controller.GetReaderAsAttendControl(1) == 1;
                this.txtReaderName2D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(2), this.m_Controller.GetDoorName(2) + "-");
                this.chkAttend2D.Checked = this.m_Controller.GetReaderAsAttendActive(2);
                this.optDutyOnOff2D.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 3;
                this.optDutyOn2D.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 2;
                this.optDutyOff2D.Checked = this.m_Controller.GetReaderAsAttendControl(2) == 1;
                this.txtReaderName3D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(3), this.m_Controller.GetDoorName(3) + "-");
                this.chkAttend3D.Checked = this.m_Controller.GetReaderAsAttendActive(3);
                this.optDutyOnOff3D.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 3;
                this.optDutyOn3D.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 2;
                this.optDutyOff3D.Checked = this.m_Controller.GetReaderAsAttendControl(3) == 1;
                this.txtReaderName4D.Text = icController.StrDelFirstSame(this.m_Controller.GetReaderName(4), this.m_Controller.GetDoorName(4) + "-");
                this.chkAttend4D.Checked = this.m_Controller.GetReaderAsAttendActive(4);
                this.optDutyOnOff4D.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 3;
                this.optDutyOn4D.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 2;
                this.optDutyOff4D.Checked = this.m_Controller.GetReaderAsAttendControl(4) == 1;
                if (this.m_Controller.ControllerNO != int.Parse(this.mtxtbControllerNO.Text))
                {
                    this.txtDoorName1D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(1), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                    this.txtDoorName2D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(2), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                    this.txtDoorName3D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(3), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                    this.txtDoorName4D.Text = icController.StrReplaceFirstSame(this.m_Controller.GetDoorName(4), this.m_Controller.ControllerNO.ToString() + "-", this.mtxtbControllerNO.Text + "-");
                }
                else
                {
                    this.txtDoorName1D.Text = this.m_Controller.GetDoorName(1);
                    this.txtDoorName2D.Text = this.m_Controller.GetDoorName(2);
                    this.txtDoorName3D.Text = this.m_Controller.GetDoorName(3);
                    this.txtDoorName4D.Text = this.m_Controller.GetDoorName(4);
                }
                goto Label_13D5;
            }
            this.txtDoorName1D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName1D.Text;
            this.txtDoorName2D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName2D.Text;
            this.txtDoorName3D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName3D.Text;
            this.txtDoorName4D.Text = "m" + this.mtxtbControllerNO.Text.PadLeft(3, '0') + "-" + this.txtDoorName4D.Text;
        Label_13D5:
            str = "Select * from  [t_b_Reader] where NOT (f_DutyOnOff =3)";
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        bool flag = false;
                        if (reader.Read())
                        {
                            flag = true;
                        }
                        reader.Close();
                        if (flag && (controllerType > 0))
                        {
                            switch (controllerType)
                            {
                                case 1:
                                    this.label33.Visible = true;
                                    this.groupBox20.Visible = true;
                                    this.groupBox14.Visible = true;
                                    goto Label_1631;

                                case 2:
                                    this.label39.Visible = true;
                                    this.groupBox16.Visible = true;
                                    this.groupBox17.Visible = true;
                                    this.groupBox18.Visible = true;
                                    this.gpbAttend1B.Visible = true;
                                    goto Label_1631;

                                case 3:
                                    goto Label_1631;

                                case 4:
                                    this.label13.Visible = true;
                                    this.groupBox8.Visible = true;
                                    this.groupBox9.Visible = true;
                                    this.groupBox10.Visible = true;
                                    this.groupBox12.Visible = true;
                                    goto Label_1631;
                            }
                        }
                    }
                    goto Label_1631;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();
                    bool flag2 = false;
                    if (reader2.Read())
                    {
                        flag2 = true;
                    }
                    reader2.Close();
                    if (flag2 && (controllerType > 0))
                    {
                        switch (controllerType)
                        {
                            case 1:
                                this.label33.Visible = true;
                                this.groupBox20.Visible = true;
                                this.groupBox14.Visible = true;
                                goto Label_1631;

                            case 2:
                                this.label39.Visible = true;
                                this.groupBox16.Visible = true;
                                this.groupBox17.Visible = true;
                                this.groupBox18.Visible = true;
                                this.gpbAttend1B.Visible = true;
                                goto Label_1631;

                            case 3:
                                goto Label_1631;

                            case 4:
                                this.label13.Visible = true;
                                this.groupBox8.Visible = true;
                                this.groupBox9.Visible = true;
                                this.groupBox10.Visible = true;
                                this.groupBox12.Visible = true;
                                goto Label_1631;
                        }
                    }
                }
            }
        Label_1631:
            this.panelBottomBanner.Visible = false;
            prevDoorReaderLoc = this.grpbDoorReader.Location;
            this.grpbDoorReader.Location = new Point(2, 5);
            prevWndSize = base.Size;
            base.Size = new Size(base.Size.Width, this.grpbDoorReader.Height + 87);
            this.panel1.Visible = true;
            this.panel1.Location = new Point(0, this.grpbDoorReader.Location.Y + this.grpbDoorReader.Height + 10);
            this.panel1.Width = base.Size.Width;
            base.AcceptButton = this.btnOK;
            this.grpbDoorReader.Visible = true;
        }

        public void btnOK_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            string str;
            this.mtxtbControllerNO.Text = this.mtxtbControllerNO.Text.Replace(" ", "");
            this.mtxtbControllerSN.Text = this.mtxtbControllerSN.Text.Replace(" ", "");
            if (!int.TryParse(this.mtxtbControllerNO.Text, out num))
            {
                XMessageBox.Show(this, CommonStr.strIDWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if ((int.Parse(this.mtxtbControllerNO.Text) > 0x186a0) || (int.Parse(this.mtxtbControllerNO.Text) < 0))
            {
                XMessageBox.Show(this, CommonStr.strIDWrong + ", <=100000 , >0", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!int.TryParse(this.mtxtbControllerSN.Text, out num))
            {
                XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text)) == 0)
            {
                XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.optIPLarge.Checked && string.IsNullOrEmpty(this.txtControllerIP.Text))
            {
                XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (chkWiFiComm.Checked && (string.IsNullOrEmpty(txtWiFiSSID.Text) ||
                string.IsNullOrEmpty(txtWiFiIP.Text) ||
                string.IsNullOrEmpty(txtWiFiMask.Text)))
            {
                XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.m_OperateNew)
            {
                if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), 0))
                {
                    XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (icController.IsExisted2SN(int.Parse(this.mtxtbControllerSN.Text), this.m_ControllerID))
            {
                XMessageBox.Show(this, CommonStr.strSNAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.m_OperateNew && (this.m_Controller == null))
            {
                this.m_Controller = new icController();
            }
            icController controller = this.m_Controller;
            controller.ControllerNO = int.Parse(this.mtxtbControllerNO.Text);
            controller.ControllerSN = int.Parse(this.mtxtbControllerSN.Text);
            controller.Note = this.txtNote.Text.ToString();
            controller.Active = this.chkControllerActive.Checked;
            controller.IP = "";
            controller.PORT = 0xea60;
            if (this.cbof_Zone.SelectedIndex < 0)
            {
                controller.ZoneID = 0;
            }
            else
            {
                controller.ZoneID = (int)this.arrZoneID[this.cbof_Zone.SelectedIndex];
            }
            if (this.optIPLarge.Checked)
            {
                controller.IP = this.txtControllerIP.Text;
                controller.PORT = (int)this.nudPort.Value;
            }

            // WiFi settings
            controller.ssid_wifi = txtWiFiSSID.Text;
            controller.key_wifi = txtWiFiKey.Text;
            controller.enable_wifi = chkWiFiComm.Checked;
            controller.ip_wifi = txtWiFiIP.Text;
            controller.mask_wifi = txtWiFiMask.Text;
            controller.gateway_wifi = txtWiFiGateway.Text;
            controller.port_wifi = (int)nudWiFiPort.Value;

            controller.touch_sensor = chkUseTouchSensor.Checked;
            controller.m1_card = chkUseM1Card.Checked;
            controller.volume = (byte)nudVolume.Value;
            controller.fp_ident = chkFpIdent.Checked;
            controller.face_ident = chkFaceIdent.Checked;

            switch (wgMjController.GetControllerType(controller.ControllerSN))
            {
                case 1:
                    controller.SetDoorName(1, this.txtDoorName1A.Text);
                    controller.SetDoorActive(1, this.chkDoorActive1A.Checked);
                    controller.setCameraBit(1, chkCamera1_1.Checked);
                    if (!this.optOnline1A.Checked)
                    {
                        if (this.optNO1A.Checked)
                        {
                            controller.SetDoorControl(1, 1);
                        }
                        else if (this.optNC1A.Checked)
                        {
                            controller.SetDoorControl(1, 2);
                        }
                        break;
                    }
                    controller.SetDoorControl(1, 3);
                    break;

                case 2:
                    controller.SetDoorName(1, this.txtDoorName1B.Text);
                    controller.SetDoorName(2, this.txtDoorName2B.Text);
                    controller.SetDoorActive(1, this.chkDoorActive1B.Checked);
                    controller.SetDoorActive(2, this.chkDoorActive2B.Checked);
                    if (!this.optOnline1B.Checked)
                    {
                        if (this.optNO1B.Checked)
                        {
                            controller.SetDoorControl(1, 1);
                        }
                        else if (this.optNC1B.Checked)
                        {
                            controller.SetDoorControl(1, 2);
                        }
                    }
                    else
                    {
                        controller.SetDoorControl(1, 3);
                    }
                    if (this.optOnline2B.Checked)
                    {
                        controller.SetDoorControl(2, 3);
                    }
                    else if (this.optNO2B.Checked)
                    {
                        controller.SetDoorControl(2, 1);
                    }
                    else if (this.optNC2B.Checked)
                    {
                        controller.SetDoorControl(2, 2);
                    }
                    controller.SetDoorDelay(1, (int)this.nudDoorDelay1B.Value);
                    controller.SetDoorDelay(2, (int)this.nudDoorDelay2B.Value);
                    controller.SetReaderName(1, string.Format("{0}-{1}", this.txtDoorName1B.Text, this.txtReaderName1B.Text));
                    controller.SetReaderName(2, string.Format("{0}-{1}", this.txtDoorName1B.Text, this.txtReaderName2B.Text));
                    controller.SetReaderName(3, string.Format("{0}-{1}", this.txtDoorName2B.Text, this.txtReaderName3B.Text));
                    controller.SetReaderName(4, string.Format("{0}-{1}", this.txtDoorName2B.Text, this.txtReaderName4B.Text));
                    controller.SetReaderAsAttendActive(1, this.chkAttend1B.Checked);
                    controller.SetReaderAsAttendActive(2, this.chkAttend2B.Checked);
                    controller.SetReaderAsAttendActive(3, this.chkAttend3B.Checked);
                    controller.SetReaderAsAttendActive(4, this.chkAttend4B.Checked);
                    controller.setCameraBit(1, chkCamera2_1.Checked);
                    controller.setCameraBit(2, chkCamera2_2.Checked);

                    if (this.optDutyOnOff1B.Checked)
                    {
                        controller.SetReaderAsAttendControl(1, 3);
                    }
                    else if (this.optDutyOn1B.Checked)
                    {
                        controller.SetReaderAsAttendControl(1, 2);
                    }
                    else if (this.optDutyOff1B.Checked)
                    {
                        controller.SetReaderAsAttendControl(1, 1);
                    }
                    if (this.optDutyOnOff2B.Checked)
                    {
                        controller.SetReaderAsAttendControl(2, 3);
                    }
                    else if (this.optDutyOn2B.Checked)
                    {
                        controller.SetReaderAsAttendControl(2, 2);
                    }
                    else if (this.optDutyOff2B.Checked)
                    {
                        controller.SetReaderAsAttendControl(2, 1);
                    }
                    if (this.optDutyOnOff3B.Checked)
                    {
                        controller.SetReaderAsAttendControl(3, 3);
                    }
                    else if (this.optDutyOn3B.Checked)
                    {
                        controller.SetReaderAsAttendControl(3, 2);
                    }
                    else if (this.optDutyOff3B.Checked)
                    {
                        controller.SetReaderAsAttendControl(3, 1);
                    }
                    if (this.optDutyOnOff4B.Checked)
                    {
                        controller.SetReaderAsAttendControl(4, 3);
                    }
                    else if (this.optDutyOn4B.Checked)
                    {
                        controller.SetReaderAsAttendControl(4, 2);
                    }
                    else if (this.optDutyOff4B.Checked)
                    {
                        controller.SetReaderAsAttendControl(4, 1);
                    }
                    goto Label_0B34;

                default:
                    controller.SetDoorName(1, this.txtDoorName1D.Text);
                    controller.SetDoorName(2, this.txtDoorName2D.Text);
                    controller.SetDoorName(3, this.txtDoorName3D.Text);
                    controller.SetDoorName(4, this.txtDoorName4D.Text);
                    controller.SetDoorActive(1, this.chkDoorActive1D.Checked);
                    controller.SetDoorActive(2, this.chkDoorActive2D.Checked);
                    controller.SetDoorActive(3, this.chkDoorActive3D.Checked);
                    controller.SetDoorActive(4, this.chkDoorActive4D.Checked);
                    if (this.optOnline1D.Checked)
                    {
                        controller.SetDoorControl(1, 3);
                    }
                    else if (this.optNO1D.Checked)
                    {
                        controller.SetDoorControl(1, 1);
                    }
                    else if (this.optNC1D.Checked)
                    {
                        controller.SetDoorControl(1, 2);
                    }
                    if (this.optOnline2D.Checked)
                    {
                        controller.SetDoorControl(2, 3);
                    }
                    else if (this.optNO2D.Checked)
                    {
                        controller.SetDoorControl(2, 1);
                    }
                    else if (this.optNC2D.Checked)
                    {
                        controller.SetDoorControl(2, 2);
                    }
                    if (this.optOnline3D.Checked)
                    {
                        controller.SetDoorControl(3, 3);
                    }
                    else if (this.optNO3D.Checked)
                    {
                        controller.SetDoorControl(3, 1);
                    }
                    else if (this.optNC3D.Checked)
                    {
                        controller.SetDoorControl(3, 2);
                    }
                    if (this.optOnline4D.Checked)
                    {
                        controller.SetDoorControl(4, 3);
                    }
                    else if (this.optNO4D.Checked)
                    {
                        controller.SetDoorControl(4, 1);
                    }
                    else if (this.optNC4D.Checked)
                    {
                        controller.SetDoorControl(4, 2);
                    }
                    controller.SetDoorDelay(1, (int)this.nudDoorDelay1D.Value);
                    controller.SetDoorDelay(2, (int)this.nudDoorDelay2D.Value);
                    controller.SetDoorDelay(3, (int)this.nudDoorDelay3D.Value);
                    controller.SetDoorDelay(4, (int)this.nudDoorDelay4D.Value);
                    controller.SetReaderName(1, string.Format("{0}-{1}", this.txtDoorName1D.Text, this.txtReaderName1D.Text));
                    controller.SetReaderName(2, string.Format("{0}-{1}", this.txtDoorName2D.Text, this.txtReaderName2D.Text));
                    controller.SetReaderName(3, string.Format("{0}-{1}", this.txtDoorName3D.Text, this.txtReaderName3D.Text));
                    controller.SetReaderName(4, string.Format("{0}-{1}", this.txtDoorName4D.Text, this.txtReaderName4D.Text));
                    controller.SetReaderAsAttendActive(1, this.chkAttend1D.Checked);
                    controller.SetReaderAsAttendActive(2, this.chkAttend2D.Checked);
                    controller.SetReaderAsAttendActive(3, this.chkAttend3D.Checked);
                    controller.SetReaderAsAttendActive(4, this.chkAttend4D.Checked);
                    controller.setCameraBit(1, chkCamera4_1.Checked);
                    controller.setCameraBit(2, chkCamera4_2.Checked);
                    controller.setCameraBit(3, chkCamera4_3.Checked);
                    controller.setCameraBit(4, chkCamera4_4.Checked);

                    if (this.optDutyOnOff1D.Checked)
                    {
                        controller.SetReaderAsAttendControl(1, 3);
                    }
                    else if (this.optDutyOn1D.Checked)
                    {
                        controller.SetReaderAsAttendControl(1, 2);
                    }
                    else if (this.optDutyOff1D.Checked)
                    {
                        controller.SetReaderAsAttendControl(1, 1);
                    }
                    if (this.optDutyOnOff2D.Checked)
                    {
                        controller.SetReaderAsAttendControl(2, 3);
                    }
                    else if (this.optDutyOn2D.Checked)
                    {
                        controller.SetReaderAsAttendControl(2, 2);
                    }
                    else if (this.optDutyOff2D.Checked)
                    {
                        controller.SetReaderAsAttendControl(2, 1);
                    }
                    if (this.optDutyOnOff3D.Checked)
                    {
                        controller.SetReaderAsAttendControl(3, 3);
                    }
                    else if (this.optDutyOn3D.Checked)
                    {
                        controller.SetReaderAsAttendControl(3, 2);
                    }
                    else if (this.optDutyOff3D.Checked)
                    {
                        controller.SetReaderAsAttendControl(3, 1);
                    }
                    if (this.optDutyOnOff4D.Checked)
                    {
                        controller.SetReaderAsAttendControl(4, 3);
                    }
                    else if (this.optDutyOn4D.Checked)
                    {
                        controller.SetReaderAsAttendControl(4, 2);
                    }
                    else if (this.optDutyOff4D.Checked)
                    {
                        controller.SetReaderAsAttendControl(4, 1);
                    }
                    goto Label_0B34;
            }
            controller.SetDoorDelay(1, (int)this.nudDoorDelay1A.Value);
            controller.SetReaderName(1, string.Format("{0}-{1}", this.txtDoorName1A.Text, this.txtReaderName1A.Text));
            controller.SetReaderName(2, string.Format("{0}-{1}", this.txtDoorName1A.Text, this.txtReaderName2A.Text));
            controller.SetReaderAsAttendActive(1, this.chkAttend1A.Checked);
            controller.SetReaderAsAttendActive(2, this.chkAttend2A.Checked);
            if (this.optDutyOnOff1A.Checked)
            {
                controller.SetReaderAsAttendControl(1, 3);
            }
            else if (this.optDutyOn1A.Checked)
            {
                controller.SetReaderAsAttendControl(1, 2);
            }
            else if (this.optDutyOff1A.Checked)
            {
                controller.SetReaderAsAttendControl(1, 1);
            }
            if (this.optDutyOnOff2A.Checked)
            {
                controller.SetReaderAsAttendControl(2, 3);
            }
            else if (this.optDutyOn2A.Checked)
            {
                controller.SetReaderAsAttendControl(2, 2);
            }
            else if (this.optDutyOff2A.Checked)
            {
                controller.SetReaderAsAttendControl(2, 1);
            }
        Label_0B34:
            if (this.m_OperateNew)
            {
                num2 = controller.AddIntoDB();
                str = CommonStr.strAddController + ":(" + controller.ControllerNO.ToString() + ")" + controller.ControllerSN.ToString();
            }
            else
            {
                num2 = controller.UpdateIntoDB(this.m_ControllerTypeChanged);
                str = CommonStr.strUpdateController + ":(" + controller.ControllerNO.ToString() + ")" + controller.ControllerSN.ToString();
            }
            if (num2 >= 0)
            {
                if (sender != null)
                {
                    wgAppConfig.wgLog(str);
                    base.Close();
                }
                else
                {
                    this.txtDoorName1A.Text = "1" + CommonStr.strDoorNO;
                    this.txtDoorName1B.Text = "1" + CommonStr.strDoorNO;
                    this.txtDoorName2B.Text = "2" + CommonStr.strDoorNO;
                    this.txtDoorName1D.Text = "1" + CommonStr.strDoorNO;
                    this.txtDoorName2D.Text = "2" + CommonStr.strDoorNO;
                    this.txtDoorName3D.Text = "3" + CommonStr.strDoorNO;
                    this.txtDoorName4D.Text = "4" + CommonStr.strDoorNO;
                }
            }
            else
            {
                XMessageBox.Show(this, CommonStr.strValWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                wgTools.WgDebugWrite("Controller Ret=" + num2.ToString(), new object[0]);
            }
        }

        private void btnZoneManage_Click(object sender, EventArgs e)
        {
            using (frmZones zones = new frmZones())
            {
                zones.ShowDialog(this);
            }
            this.bEditZone = true;
            this.loadZoneInfo();
            if (this.m_Controller == null)
            {
                if (this.cbof_Zone.Items.Count > 0)
                {
                    this.cbof_Zone.SelectedIndex = 0;
                }
            }
            else if (this.m_Controller.ZoneID > 0)
            {
                if (this.cbof_Zone.Items.Count > 0)
                {
                    this.cbof_Zone.SelectedIndex = 0;
                }
                for (int i = 0; i < this.cbof_Zone.Items.Count; i++)
                {
                    if (((int)this.arrZoneID[i]) == this.m_Controller.ZoneID)
                    {
                        this.cbof_Zone.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void dfrmController_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    int controllerType = wgMjController.GetControllerType(int.Parse(this.mtxtbControllerSN.Text));
                    if (controllerType > 0)
                    {
                        switch (controllerType)
                        {
                            case 1:
                                this.label33.Visible = true;
                                this.groupBox20.Visible = true;
                                this.groupBox14.Visible = true;
                                return;

                            case 2:
                                this.label39.Visible = true;
                                this.groupBox16.Visible = true;
                                this.groupBox17.Visible = true;
                                this.groupBox18.Visible = true;
                                this.gpbAttend1B.Visible = true;
                                return;

                            case 3:
                                return;

                            case 4:
                                this.label13.Visible = true;
                                this.groupBox8.Visible = true;
                                this.groupBox9.Visible = true;
                                this.groupBox10.Visible = true;
                                this.groupBox12.Visible = true;
                                return;
                        }
                    }
                }
            }
        }

        private void adjustControls(int controllerSN)
        {
            wgMjController.DeviceType deviceType = wgMjController.getDeviceType(controllerSN);
            switch (deviceType)
            {
                case wgMjController.DeviceType.A30_AC://A30 online
                    chkWiFiComm.Enabled = false;
                    grpWiFi.Enabled = false;
                    chkCamera4_1.Visible = false;
                    chkCamera4_2.Visible = false;
                    chkCamera4_3.Visible = false;
                    chkCamera4_4.Visible = false;
                    chkCamera2_1.Visible = false;
                    chkCamera2_2.Visible = false;
                    chkCamera1_1.Visible = false;
                    chkUseM1Card.Enabled = false;
                    chkFaceIdent.Enabled = false;
                    nudVolume.Maximum = 7;
                    break;

                case wgMjController.DeviceType.A50_AC://A50 online
                    chkWiFiComm.Enabled = true;
                    grpWiFi.Enabled = true;
                    chkCamera4_1.Visible = true;
                    chkCamera4_2.Visible = true;
                    chkCamera4_3.Visible = true;
                    chkCamera4_4.Visible = true;
                    chkCamera2_1.Visible = true;
                    chkCamera2_2.Visible = true;
                    chkCamera1_1.Visible = true;
                    chkUseM1Card.Enabled = true;
                    if (m_Controller == null || !m_Controller.enable_wifi)
                    {
                        chkWiFiComm.Checked = false;
                        grpWiFi.Enabled = false;
                    }
                    else
                    {
                        chkWiFiComm.Checked = true;
                        grpWiFi.Enabled = true;
                    }
                    if (m_Controller != null)
                    {
                        txtWiFiSSID.Text = m_Controller.ssid_wifi;
                        txtWiFiKey.Text = m_Controller.key_wifi;
                        txtWiFiIP.Text = m_Controller.ip_wifi;
                        txtWiFiMask.Text = m_Controller.mask_wifi;
                        txtWiFiGateway.Text = m_Controller.gateway_wifi;
                        if (m_Controller.port_wifi > 0)
                            nudWiFiPort.Value = m_Controller.port_wifi;
                    }
                    
                    chkCamera4_1.Checked = (m_Controller != null) && m_Controller.getCameraBit(1);
                    chkCamera4_2.Checked = (m_Controller != null) && m_Controller.getCameraBit(2);
                    chkCamera4_3.Checked = (m_Controller != null) && m_Controller.getCameraBit(3);
                    chkCamera4_4.Checked = (m_Controller != null) && m_Controller.getCameraBit(4);
                    chkCamera2_1.Checked = (m_Controller != null) && m_Controller.getCameraBit(1);
                    chkCamera2_2.Checked = (m_Controller != null) && m_Controller.getCameraBit(2);
                    chkCamera1_1.Checked = (m_Controller != null) && m_Controller.getCameraBit(1);
                    chkFaceIdent.Enabled = false;
                    nudVolume.Maximum = 7;
                    break;

                case wgMjController.DeviceType.F300_AC://F300AC online
                    chkWiFiComm.Enabled = true;
                    grpWiFi.Enabled = true;
                    chkCamera4_1.Visible = true;
                    chkCamera4_2.Visible = true;
                    chkCamera4_3.Visible = true;
                    chkCamera4_4.Visible = true;
                    chkCamera2_1.Visible = true;
                    chkCamera2_2.Visible = true;
                    chkCamera1_1.Visible = true;
                    chkUseM1Card.Enabled = true;
                    if (m_Controller == null || !m_Controller.enable_wifi)
                    {
                        chkWiFiComm.Checked = false;
                        grpWiFi.Enabled = false;
                    }
                    else
                    {
                        chkWiFiComm.Checked = true;
                        grpWiFi.Enabled = true;
                    }
                    if (m_Controller != null)
                    {
                        txtWiFiSSID.Text = m_Controller.ssid_wifi;
                        txtWiFiKey.Text = m_Controller.key_wifi;
                        txtWiFiIP.Text = m_Controller.ip_wifi;
                        txtWiFiMask.Text = m_Controller.mask_wifi;
                        txtWiFiGateway.Text = m_Controller.gateway_wifi;
                        if (m_Controller.port_wifi > 0)
                            nudWiFiPort.Value = m_Controller.port_wifi;
                    }

                    chkCamera4_1.Checked = (m_Controller != null) && m_Controller.getCameraBit(1);
                    chkCamera4_2.Checked = (m_Controller != null) && m_Controller.getCameraBit(2);
                    chkCamera4_3.Checked = (m_Controller != null) && m_Controller.getCameraBit(3);
                    chkCamera4_4.Checked = (m_Controller != null) && m_Controller.getCameraBit(4);
                    chkCamera2_1.Checked = (m_Controller != null) && m_Controller.getCameraBit(1);
                    chkCamera2_2.Checked = (m_Controller != null) && m_Controller.getCameraBit(2);
                    chkCamera1_1.Checked = (m_Controller != null) && m_Controller.getCameraBit(1);
                    chkFpIdent.Enabled = false;
                    chkUseTouchSensor.Enabled = false;
                    nudVolume.Maximum = 10;
                    break;

                default://S8000
                    chkWiFiComm.Enabled = false;
                    grpWiFi.Enabled = false;
                    chkCamera4_1.Visible = false;
                    chkCamera4_2.Visible = false;
                    chkCamera4_3.Visible = false;
                    chkCamera4_4.Visible = false;
                    chkCamera2_1.Visible = false;
                    chkCamera2_2.Visible = false;
                    chkCamera1_1.Visible = false;
                    chkUseM1Card.Enabled = false;
                    chkFpIdent.Enabled = false;
                    chkFaceIdent.Enabled = false;
                    chkUseTouchSensor.Enabled = false;
                    break;
            }
        }


        private void dfrmController_Load(object sender, EventArgs e)
        {
            base.Visible = false;
            this.grpbDoorReader.Visible = false;
            this.loadZoneInfo();
            this.mtxtbControllerNO.Mask = "99990";
            this.mtxtbControllerSN.Mask = "000000000";
            if (this.m_OperateNew)
            {
                this.mtxtbControllerNO.Text = (icController.GetMaxControllerNO() + 1).ToString();
                adjustControls(0);
            }
            else
            {
                this.m_Controller = new icController();
                this.m_Controller.GetInfoFromDBByControllerID(this.m_ControllerID);
                this.m_Controller.ControllerID = this.m_ControllerID;
                this.mtxtbControllerNO.Text = this.m_Controller.ControllerNO.ToString();
                this.mtxtbControllerSN.Text = this.m_Controller.ControllerSN.ToString();
                this.txtNote.Text = this.m_Controller.Note.ToString();
                this.chkControllerActive.Checked = this.m_Controller.Active;
                if (this.m_Controller.IP == "")
                {
                    this.optIPSmall.Checked = true;
                }
                else
                {
                    this.optIPLarge.Checked = true;
                    this.txtControllerIP.Text = this.m_Controller.IP;
                    this.nudPort.Value = this.m_Controller.PORT;
                }

                adjustControls(m_Controller.ControllerSN);

                chkUseTouchSensor.Checked = m_Controller.touch_sensor;
                chkUseM1Card.Checked = m_Controller.m1_card;
                nudVolume.Value = m_Controller.volume;
                chkFpIdent.Checked = m_Controller.fp_ident;
                chkFaceIdent.Checked = m_Controller.face_ident;

                if (this.m_Controller.ZoneID > 0)
                {
                    if (this.cbof_Zone.Items.Count > 0)
                    {
                        this.cbof_Zone.SelectedIndex = 0;
                    }
                    for (int i = 0; i < this.cbof_Zone.Items.Count; i++)
                    {
                        if (((int)this.arrZoneID[i]) == this.m_Controller.ZoneID)
                        {
                            this.cbof_Zone.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            this.grpbIP.Visible = this.optIPLarge.Checked;
            base.Visible = true;
            this.mtxtbControllerSN.Focus();
            this.btnZoneManage.Visible = false;
            bool bReadOnly = false;
            string funName = "btnZoneManage";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && !bReadOnly)
            {
                this.btnZoneManage.Visible = true;
            }
            this.tabPage1.BackColor = this.BackColor;
            this.tabPage2.BackColor = this.BackColor;
            this.tabPage3.BackColor = this.BackColor;
            this.tabPage4.BackColor = this.BackColor;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.m_Controller != null))
            {
                this.m_Controller.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadZoneInfo()
        {
            new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
            int count = this.arrZoneID.Count;
            this.cbof_Zone.Items.Clear();
            for (count = 0; count < this.arrZoneID.Count; count++)
            {
                this.cbof_Zone.Items.Add(this.arrZoneName[count].ToString());
            }
            if (this.cbof_Zone.Items.Count > 0)
            {
                this.cbof_Zone.SelectedIndex = 0;
            }
            bool flag = true;
            this.label25.Visible = flag;
            this.cbof_Zone.Visible = flag;
            this.btnZoneManage.Visible = flag;
        }

        private void mtxtbControllerNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            SNInput(ref this.mtxtbControllerNO);
        }

        private void mtxtbControllerNO_KeyUp(object sender, KeyEventArgs e)
        {
            SNInput(ref this.mtxtbControllerNO);
        }

        private void mtxtbControllerSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            SNInput(ref this.mtxtbControllerSN);
        }

        private void mtxtbControllerSN_KeyUp(object sender, KeyEventArgs e)
        {
            SNInput(ref this.mtxtbControllerSN);
        }

        private void optIPLarge_CheckedChanged(object sender, EventArgs e)
        {
            this.grpbIP.Visible = this.optIPLarge.Checked;
        }

        public static void SNInput(ref MaskedTextBox mtb)
        {
            if (mtb.Text.Length != mtb.Text.Trim().Length)
            {
                mtb.Text = mtb.Text.Trim();
            }
            else if ((mtb.Text.Length == 0) && (mtb.SelectionStart != 0))
            {
                mtb.SelectionStart = 0;
            }
            if (mtb.Text.Length > 0)
            {
                if (mtb.Text.IndexOf(" ") > 0)
                {
                    mtb.Text = mtb.Text.Replace(" ", "");
                }
                if ((mtb.Text.Length > 9) && (long.Parse(mtb.Text) >= 0xffffffffL))
                {
                    mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
                }
            }
        }

        public int ControllerID
        {
            get
            {
                return this.m_ControllerID;
            }
            set
            {
                this.m_ControllerID = value;
            }
        }

        public bool OperateNew
        {
            get
            {
                return this.m_OperateNew;
            }
            set
            {
                this.m_OperateNew = value;
            }
        }

        private void chkWiFiComm_CheckedChanged(object sender, EventArgs e)
        {
            grpWiFi.Enabled = chkWiFiComm.Checked;
        }

        bool getHandleState(uint reader)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            WGPacketNameQuery connectQuery = new WGPacketNameQuery(0x24, 0x60, (uint)m_Controller.ControllerSN, reader);
            byte[] recv = null;

            if (wgudp.udp_get(connectQuery.ToBytes(wgudp.udpPort), 1000,
                    connectQuery.xid, m_Controller.IP, m_Controller.PORT, ref recv) < 0 ||
                    recv == null || !connectQuery.get(recv, 0x14))
                return false;

            return (connectQuery.tag == 1);
        }

        private bool startHandleReader(uint reader)
        {
            if (wgudp == null)
            {
                wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            
            WGPacketNameQuery connectQuery = new WGPacketNameQuery(0x24, 0x50, (uint)m_Controller.ControllerSN, reader);
            byte[] recv = null;

            if (wgudp.udp_get(connectQuery.ToBytes(wgudp.udpPort), 1000,
                    connectQuery.xid, m_Controller.IP, m_Controller.PORT, ref recv) < 0 ||
                    recv == null || !connectQuery.get(recv, 0x14))
                return false;

            return (connectQuery.tag == 1);
        }

        private bool handleReader(uint reader, bool connect)
        {
            bool success = false;
            if (!connect)
                reader += 4; // Connect:1, 2, 3, 4; Disconnect:5, 6, 7, 8
            if (startHandleReader(reader))
            {
                int startTime = GetTickCount();
                while (true)
                {
                    if (getHandleState(reader))
                    {
                        success = true;
                        break;
                    }

                    if (GetTickCount() - startTime > 6000)
                        break;

                    Thread.Sleep(50);
                }
            }
            XMessageBox.Show(this, success ? CommonStr.strHandleSuccess : CommonStr.strHandleFailure,
                wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return success;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnConnect1_1_Click(object sender, EventArgs e)
        {
            enableConnectButtons(2, false);
            handleReader(0, true);
            enableConnectButtons(2, true);
        }

        private void btnConnect1_2_Click(object sender, EventArgs e)
        {
            bool state = (btnConnect1_2.Text == CommonStr.strConnect);
            enableConnectButtons(2, false);
            if (handleReader(1, state))
            {
                state = !state;
                if (state)
                    btnConnect1_2.Text = CommonStr.strConnect;
                else
                    btnConnect1_2.Text = CommonStr.strDisconnect;
            }
            enableConnectButtons(2, true);
        }

        private void enableConnectButtons(int count, bool enabled)
        {
            if (count == 2)
            {
                btnConnect1_1.Enabled = enabled;
                btnConnect1_2.Enabled = enabled;
            }
            else if (count == 4)
            {
                btnConnect4_1.Enabled = enabled;
                btnConnect4_2.Enabled = enabled;
                btnConnect4_3.Enabled = enabled;
                btnConnect4_4.Enabled = enabled;
            }
        }

        private void btnConnect4_1_Click(object sender, EventArgs e)
        {
            enableConnectButtons(4, false);
            handleReader(0, true);
            enableConnectButtons(4, true);
        }

        private void btnConnect4_2_Click(object sender, EventArgs e)
        {
            bool state = (btnConnect4_2.Text == CommonStr.strConnect);
            enableConnectButtons(4, false);
            if (handleReader(1, state))
            {
                state = !state;
                if (state)
                    btnConnect4_2.Text = CommonStr.strConnect;
                else
                    btnConnect4_2.Text = CommonStr.strDisconnect;
            }
            enableConnectButtons(4, true);
        }

        private void btnConnect4_3_Click(object sender, EventArgs e)
        {
            bool state = (btnConnect4_3.Text == CommonStr.strConnect);
            enableConnectButtons(4, false);
            if (handleReader(2, state))
            {
                state = !state;
                if (state)
                    btnConnect4_3.Text = CommonStr.strConnect;
                else
                    btnConnect4_3.Text = CommonStr.strDisconnect;
            }
            enableConnectButtons(4, true);
        }

        private void btnConnect4_4_Click(object sender, EventArgs e)
        {
            bool state = (btnConnect4_4.Text == CommonStr.strConnect);
            enableConnectButtons(4, false);
            if (handleReader(3, state))
            {
                state = !state;
                if (state)
                    btnConnect4_4.Text = CommonStr.strConnect;
                else
                    btnConnect4_4.Text = CommonStr.strDisconnect;
            }
            enableConnectButtons(4, true);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.panelBottomBanner.Visible = true;
            this.grpbDoorReader.Location = prevDoorReaderLoc;
            base.Size = prevWndSize;
            this.panel1.Visible = false;
            base.AcceptButton = btnNext;
        }

        private void mtxtbControllerSN_TextChanged(object sender, EventArgs e)
        {
            int controllerSN = 0;
            if (mtxtbControllerSN.Text != "")
            {
                controllerSN = Convert.ToInt32(mtxtbControllerSN.Text);
                adjustControls(controllerSN);
            }
        }

        private void btnConnect2_1_Click(object sender, EventArgs e)
        {
            btnConnect4_1_Click(sender, e);
        }

        private void btnConnect2_2_Click(object sender, EventArgs e)
        {
            btnConnect4_2_Click(sender, e);
        }

        private void btnConnect2_3_Click(object sender, EventArgs e)
        {
            btnConnect4_3_Click(sender, e);
        }

        private void btnConnect2_4_Click(object sender, EventArgs e)
        {
            btnConnect4_4_Click(sender, e);
        }
    }
}
