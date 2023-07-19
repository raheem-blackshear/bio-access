namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmReadCardID : frmBioAccess
    {
        public bool bAutoAddBySwiping;
        private bool bDisposeWatching = false;
        private string inputCard = "";
        private int inputMode;
        private const int Mode_ControllerReader = 2;
        private const int Mode_USBReader = 1;
        private int selectedControllerSN;
        private int selectedDoorNO;
        public WatchingService watching;

        public dfrmReadCardID()
        {
            this.InitializeComponent();
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
            txtCardID.Text = "";
            panelBottomBanner.Visible = false;
            panel1.Visible = true;
            if (!this.optController.Checked || !string.IsNullOrEmpty(this.cboDoors.Text))
            {
                if (this.optUSBReader.Checked)
                {
                    this.inputMode = 1;
                }
                else
                {
                    this.inputMode = 2;
                    this.controllerReaderInput();
                }
                groupBox1.Visible = false;
                this.groupBox2.Location = new Point(this.groupBox1.Location.X, this.groupBox1.Location.Y);
                this.groupBox2.Visible = true;
                if (wgAppConfig.IsChineseSet(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("Language"))))
                {
                    base.Size = new Size(base.Size.Width, 160);
                }
                else
                {
                    base.Size = new Size(base.Size.Width, 160);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            this.groupBox2.Visible = false;
            base.Size = new Size(base.Size.Width, 210);
            if (this.optController.Checked)
            {
                try
                {
                    this.watching.EventHandler -= new OnEventHandler(this.evtNewInfoCallBack);
                }
                catch
                {
                }
            }

            panelBottomBanner.Visible = true;
            panel1.Visible = false;
        }

        private void controllerReaderInput()
        {
            if (this.watching == null)
            {
                if (this.frmCall == null)
                {
                    this.watching = new WatchingService();
                }
                else
                {
                    (this.frmCall as frmUsers).startWatch();
                    this.watching = (this.frmCall as frmUsers).watching;
                }
            }
            this.watching.EventHandler += new OnEventHandler(this.evtNewInfoCallBack);
            Dictionary<int, icController> dictionary = new Dictionary<int, icController>();
            this.control = new icController();
            this.control.GetInfoFromDBByDoorName(this.cboDoors.Text);
            this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoors.Text);
            this.selectedDoorNO = int.Parse(this.dvDoors4Watching[0]["f_DoorNO"].ToString());
            this.selectedControllerSN = this.control.ControllerSN;
            dictionary.Add(this.control.ControllerSN, this.control);
            this.watching.WatchingController = dictionary;
        }

        private void dfrmReadCardID_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.watching != null)
            {
                this.watching.EventHandler -= new OnEventHandler(this.evtNewInfoCallBack);
                if (this.frmCall == null)
                {
                    this.watching.StopWatch();
                }
            }
            try
            {
                if (this.dfrmWait1 != null)
                {
                    this.dfrmWait1.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void dfrmReadCardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.inputMode == 1)
            {
                foreach (object obj2 in base.Controls)
                {
                    try
                    {
                        (obj2 as Control).ImeMode = ImeMode.Off;
                    }
                    catch
                    {
                    }
                }
                if (((!e.Control && !e.Alt) && (!e.Shift && (e.KeyValue >= 0x30))) && (e.KeyValue <= 0x39))
                {
                    if (this.inputCard.Length == 0)
                    {
                        this.timer1.Interval = 500;
                        this.timer1.Enabled = true;
                    }
                    this.inputCard = this.inputCard + ((e.KeyValue - 0x30)).ToString();
                }
            }
            else
            {
                if ((e.KeyValue == 0x43) && e.Control)
                    Clipboard.SetDataObject(txtCardID.Text, false);
            }
        }

        private void dfrmReadCardID_Load(object sender, EventArgs e)
        {
            this.loadDoorData();
            if (this.bAutoAddBySwiping && (this.dvDoors.Count > 0))
            {
                int num = -1;
                bool flag = true;
                for (int i = 0; i < this.dvDoors.Count; i++)
                {
                    if (num == -1)
                    {
                        num = (int) this.dvDoors[i]["f_ControllerSN"];
                    }
                    else if (num != ((int) this.dvDoors[i]["f_ControllerSN"]))
                    {
                        flag = false;
                        break;
                    }
                }
                this.optController.Checked = true;
                if (flag)
                {
                    this.btnNext.PerformClick();
                }
            }
        }

        private void evtNewInfoCallBack(string text)
        {
            wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
            txtCardID.Invoke(new txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { text });
        }

        private void loadDoorData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadDoorData_Acc();
            }
            else
            {
                string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
                cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            this.dt = new DataTable();
                            this.dvDoors = new DataView(this.dt);
                            this.dvDoors4Watching = new DataView(this.dt);
                            adapter.Fill(this.dt);
                            new icControllerZone().getAllowedControllers(ref this.dt);
                            try
                            {
                                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                            }
                            catch (Exception exception)
                            {
                                wgAppConfig.wgLog(exception.ToString());
                            }
                            this.cboDoors.Items.Clear();
                            if (this.dvDoors.Count > 0)
                            {
                                for (int i = 0; i < this.dvDoors.Count; i++)
                                {
                                    this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
                                }
                                if (this.cboDoors.Items.Count > 0)
                                {
                                    this.cboDoors.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void loadDoorData_Acc()
        {
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
            cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        this.dt = new DataTable();
                        this.dvDoors = new DataView(this.dt);
                        this.dvDoors4Watching = new DataView(this.dt);
                        adapter.Fill(this.dt);
                        new icControllerZone().getAllowedControllers(ref this.dt);
                        try
                        {
                            this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                        }
                        catch (Exception exception)
                        {
                            wgAppConfig.wgLog(exception.ToString());
                        }
                        this.cboDoors.Items.Clear();
                        if (this.dvDoors.Count > 0)
                        {
                            for (int i = 0; i < this.dvDoors.Count; i++)
                            {
                                this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
                            }
                            if (this.cboDoors.Items.Count > 0)
                            {
                                this.cboDoors.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
        }

        private void optController_CheckedChanged(object sender, EventArgs e)
        {
            this.cboDoors.Enabled = this.optController.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            if (this.inputCard.Length >= 8)
                txtCardID.Text = this.inputCard;
            this.inputCard = "";
        }

        private void txtInfoUpdateEntry(object info)
        {
            string info_ = info as string;
            ulong cardID = 0;
            if (!string.IsNullOrEmpty(info_) && info_.Length < 0x30)
            {
                byte[] cardID_ = new byte[0x8];
                uint controllerSN = Convert.ToUInt32(info_.Substring(0, 8), 0x10);
                for (int i = 0; i < 8; i++)
                    cardID_[7-i] = Convert.ToByte(info_.Substring(8 + (i * 2), 2), 0x10);
                cardID = BitConverter.ToUInt64(cardID_, 0);
                if ((controllerSN > 0) && (controllerSN == this.selectedControllerSN))
                    txtCardID.Text = cardID.ToString();
            }
        }

        private int usbReaderInput()
        {
            if (txtCardID.Text == "")
                return -1;

            Application.DoEvents();
            return 1;
        }

        private delegate void txtInfoUpdate(object info);

        public string getCardID()
        {
            return txtCardID.Text;
        }
    }
}

