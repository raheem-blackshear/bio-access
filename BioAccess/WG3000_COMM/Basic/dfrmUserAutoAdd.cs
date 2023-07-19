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

    public partial class dfrmUserAutoAdd : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        public bool bAutoAddBySwiping;
        private bool bDisposeWatching = false;
        private string inputCard = "";
        private int inputMode;
        private const int Mode_ControllerReader = 2;
        private const int Mode_ManualInput = 3;
        private const int Mode_USBReader = 1;
        private int selectedControllerSN;
        private int selectedDoorNO;
        public WatchingService watching;

        public dfrmUserAutoAdd()
        {
            this.InitializeComponent();
        }

        private int _manualInput()
        {
            int num = -1;
            if (string.IsNullOrEmpty(this.txtStartNO.Text) || string.IsNullOrEmpty(this.txtEndNO.Text))
            {
                XMessageBox.Show(this, CommonStr.strCheckCard, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return num;
            }
            long num2 = long.Parse(this.txtStartNO.Text);
            long num3 = long.Parse(this.txtEndNO.Text);
            if ((num2 <= 0L) || (num3 <= 0L))
            {
                XMessageBox.Show(this, CommonStr.strCheckCard, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return num;
            }
            if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSureAutoAddCard + ": {0:d}--{1:d} [{2:d}] ?", num2, num3, (((num3 - num2) + 1L) > 0L) ? ((num3 - num2) + 1L) : 1L), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
            {
                return num;
            }
            icConsumer consumer = new icConsumer();
            int groupID = int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString());
            string startcaption = "";
            if (this.chkOption.Checked && (this.txtNOStartCaption.Text.Trim().Length > 0))
            {
                startcaption = this.txtNOStartCaption.Text;
            }
            long num7 = consumer.ConsumerNONext(startcaption);
            if (num7 < 0L)
            {
                XMessageBox.Show(this, wgAppConfig.ReplaceWorkNO(CommonStr.strAutoAddCardErrConsumerNO), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return num;
            }
            int num8 = 0;
            if (((num3 - num2) + 1L) > 10L)
            {
                this.dfrmWait1.Show();
                this.dfrmWait1.Refresh();
            }
            for (long i = num2; i <= num3; i += 1L)
            {
                string str2;
                if (string.IsNullOrEmpty(startcaption))
                {
                    str2 = num7.ToString();
                }
                else if (this.chkConst.Checked && ((this.nudNOLength.Value - this.txtNOStartCaption.Text.Length) > 0M))
                {
                    str2 = string.Format("{0}{1}", startcaption, num7.ToString().PadLeft(((int) this.nudNOLength.Value) - this.txtNOStartCaption.Text.Length, '0'));
                }
                else
                {
                    str2 = string.Format("{0}{1}", startcaption, num7.ToString());
                }
                if (consumer.addNew(str2.ToString(), "N" + i.ToString(), groupID, 
                        1, 0, 1, DateTime.Now, DateTime.Parse("2029-12-31"), "", i, null, null) >= 0)
                {
                    num7 += 1L;
                    num8++;
                }
                if ((num8 % 100) == 0)
                {
                    wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num8));
                    Application.DoEvents();
                }
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num8));
            this.dfrmWait1.Hide();
            this.dfrmWait1.Refresh();
            Application.DoEvents();
            XMessageBox.Show(this, CommonStr.strAutoAddCard + "\r\n\r\n" + num8.ToString(), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDirectGetFromtheController_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.watching != null)
                {
                    this.watching.EventHandler -= new OnEventHandler(this.evtNewInfoCallBack);
                    this.watching.StopWatch();
                }
                this.watching = null;
            }
            catch (Exception)
            {
            }
            this.btnDirectGetFromtheController.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            using (icController controller = new icController())
            {
                using (wgMjControllerPrivilege privilege = new wgMjControllerPrivilege())
                {
                    controller.GetInfoFromDBByDoorName(this.cboDoors.Text);
                    privilege.AllowDownload();
                    if (this.dtPrivilege != null)
                    {
                        this.dtPrivilege.Rows.Clear();
                        this.dtPrivilege.Dispose();
                        this.dtPrivilege = null;
                        GC.Collect();
                    }
                    this.dtPrivilege = new DataTable("Privilege");
                    this.dtPrivilege.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                    this.dtPrivilege.Columns.Add("f_CardNO", System.Type.GetType("System.string"));
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
                    this.label3.Text = this.btnDirectGetFromtheController.Text + " ...";
                    wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Start");
                    this.Refresh();
                    DataTable tbFpTempl = null;
                    if (privilege.DownloadIP(controller.ControllerSN, controller.IP, controller.PORT, "", ref this.dtPrivilege, ref tbFpTempl) > 0)
                    {
                        if (this.dtPrivilege.Rows.Count >= 0)
                        {
                            this.lblCount.Text = (this.lstSwipe.Items.Count + this.dtPrivilege.Rows.Count).ToString();
                            wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Complete");
                            this.label3.Text = CommonStr.strSuccessfully;
                            this.Refresh();
                            for (int i = 0; i < this.dtPrivilege.Rows.Count; i++)
                            {
                                if (string.IsNullOrEmpty(wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])))
                                {
                                    this.lstSwipe.Items.Add(this.dtPrivilege.Rows[i]["f_ConsumerNO"].ToString());
                                }
                                else
                                {
                                    this.lstSwipe.Items.Add(string.Format("{0}_{1}", this.dtPrivilege.Rows[i]["f_ConsumerNO"].ToString(), wgTools.SetObjToStr(this.dtPrivilege.Rows[i]["f_ConsumerName"])));
                                }
                            }
                        }
                        else
                        {
                            this.label3.Text = CommonStr.strCommFail;
                            wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Failed");
                        }
                    }
                    else
                    {
                        this.label3.Text = CommonStr.strCommFail;
                        wgAppConfig.wgLog(this.btnDirectGetFromtheController.Text + " Failed");
                    }
                    Cursor.Current = Cursors.Default;
                    this.btnDirectGetFromtheController.Enabled = true;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.lstSwipe.Items.Clear();
            this.lblInfo.Text = "";
            this.lblCount.Text = "";
            panelBottomBanner.Visible = false;
            panel1.Visible = true;
            try
            {
                string keyVal = wgAppConfig.GetKeyVal("UserAutoAddSet");
                if (!string.IsNullOrEmpty(keyVal) && (keyVal.IndexOf(",") > 0))
                {
                    string s = keyVal.Substring(0, keyVal.IndexOf(","));
                    string valA = keyVal.Substring(keyVal.IndexOf(",") + 1);
                    if (int.Parse(s) > 0)
                    {
                        this.chkConst.Checked = true;
                        this.nudNOLength.Value = int.Parse(s);
                        this.nudNOLength.Enabled = true;
                    }
                    this.txtNOStartCaption.Text = wgTools.SetObjToStr(valA);
                    //this.chkOption.Checked = true;
                    //this.groupBox4.Visible = true;
                }
            }
            catch (Exception)
            {
            }
            if (!this.optController.Checked || !string.IsNullOrEmpty(this.cboDoors.Text))
            {
                if (this.optManualInput.Checked)
                {
                    this.label3.Visible = false;
                    this.groupBox3.Visible = true;
                    this.inputMode = 3;
                }
                else
                {
                    this.label3.Visible = true;
                    this.groupBox3.Visible = false;
                    if (this.optUSBReader.Checked)
                    {
                        this.inputMode = 1;
                    }
                    else
                    {
                        this.inputMode = 2;
                        this.controllerReaderInput();
                    }
                }
                new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
                int count = this.arrGroupID.Count;
                for (count = 0; count < this.arrGroupID.Count; count++)
                {
                    this.cbof_GroupID.Items.Add(this.arrGroupName[count].ToString());
                }
                if (this.cbof_GroupID.Items.Count > 0)
                {
                    this.cbof_GroupID.SelectedIndex = 0;
                }
                this.groupBox2.Location = new Point(this.groupBox1.Location.X, this.groupBox1.Location.Y);
                this.groupBox2.Visible = true;
                if (wgAppConfig.IsChineseSet(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("Language"))))
                {
                    base.Size = new Size(base.Size.Width, 360);
                }
                else
                {
                    base.Size = new Size(base.Size.Width, 380);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int num = -1;
            Cursor.Current = Cursors.WaitCursor;
            if (this.inputMode == 3)
            {
                num = this._manualInput();
            }
            else if ((this.inputMode == 1) || (this.inputMode == 2))
            {
                num = this.usbReaderInput();
            }
            Cursor.Current = Cursors.Default;
            if (num >= 0)
            {
                try
                {
                    string keyVal = "";
                    keyVal = wgAppConfig.GetKeyVal("UserAutoAddSet");
                    if (this.chkOption.Checked)
                    {
                        if (this.chkConst.Checked)
                        {
                            keyVal = this.nudNOLength.Value.ToString();
                        }
                        else
                        {
                            keyVal = "0";
                        }
                        keyVal = keyVal + "," + this.txtNOStartCaption.Text;
                    }
                    wgAppConfig.UpdateKeyVal("UserAutoAddSet", keyVal);
                }
                catch (Exception)
                {
                }
                base.DialogResult = DialogResult.OK;
                icConsumerShare.setUpdateLog();
                base.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.groupBox2.Visible = false;
            base.Size = new Size(base.Size.Width, 0x10a);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.nudNOLength.Enabled = this.chkConst.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox4.Visible = this.chkOption.Checked;
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

        private void dfrmUserAutoAdd_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dfrmUserAutoAdd_KeyDown(object sender, KeyEventArgs e)
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
                if (((2 == this.inputMode) && (e.KeyValue == 0x51)) && (e.Control && e.Shift))
                {
                    this.btnDirectGetFromtheController.Visible = true;
                }
                if ((e.KeyValue == 0x43) && e.Control)
                {
                    string data = "";
                    for (int i = 0; i < this.lstSwipe.Items.Count; i++)
                    {
                        data = data + this.lstSwipe.Items[i].ToString() + "\r\n";
                    }
                    Clipboard.SetDataObject(data, false);
                }
            }
        }

        private void dfrmUserAutoAdd_Load(object sender, EventArgs e)
        {
            this.txtStartNO.Mask = "9999999999";
            this.txtEndNO.Mask = "9999999999";
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.chkOption.Text = wgAppConfig.ReplaceWorkNO(this.chkOption.Text);
            this.txtNOStartCaption.Text = wgAppConfig.ReplaceWorkNO(this.txtNOStartCaption.Text);
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
            this.lstSwipe.Invoke(new txtInfoUpdate(this.txtInfoUpdateEntry), new object[] { text });
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
            {
                bool flag = false;
                foreach (object obj2 in this.lstSwipe.Items)
                {
                    if ((obj2 as string) == this.inputCard)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this.lstSwipe.Items.Add(this.inputCard);
                    this.lblInfo.Text = this.inputCard;
                }
                else
                {
                    this.lblInfo.Text = this.inputCard + CommonStr.strCardNOIsAdded;
                }
                this.lblCount.Text = this.lstSwipe.Items.Count.ToString();
            }
            this.inputCard = "";
        }

        private void txtEndNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtEndNO);
        }

        private void txtEndNO_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtEndNO);
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
                {
                    bool flag = false;
                    string item = cardID.ToString();
                    foreach (object obj2 in this.lstSwipe.Items)
                    {
                        if ((obj2 as string) == item)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        this.lstSwipe.Items.Add(item);
                        this.lblInfo.Text = item;
                    }
                    else
                    {
                        this.lblInfo.Text = item + CommonStr.strCardNOIsAdded;
                    }
                    this.lblCount.Text = this.lstSwipe.Items.Count.ToString();
                }
            }
        }

        private void txtStartNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtStartNO);
        }

        private void txtStartNO_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtStartNO);
        }

        private int usbReaderInput()
        {
            int num = -1;
            if (this.lstSwipe.Items.Count <= 0)
            {
                return num;
            }
            icConsumer consumer = new icConsumer();
            int groupID = int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString());
            string startcaption = "";
            if (this.chkOption.Checked && (this.txtNOStartCaption.Text.Trim().Length > 0))
            {
                startcaption = this.txtNOStartCaption.Text;
            }
            long num5 = consumer.ConsumerNONext(startcaption);
            int num6 = 0;
            if (num5 < 0L)
            {
                XMessageBox.Show(this, wgAppConfig.ReplaceWorkNO(CommonStr.strAutoAddCardErrConsumerNO), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return num;
            }
            if (this.lstSwipe.Items.Count > 10)
            {
                this.dfrmWait1.Show();
                this.dfrmWait1.Refresh();
            }
            for (int i = 0; i < this.lstSwipe.Items.Count; i++)
            {
                int num2;
                long num3;
                if (this.lstSwipe.Items[i].ToString().IndexOf("_") <= 0)
                {
                    string str2;
                    num3 = long.Parse(this.lstSwipe.Items[i].ToString());
                    if (string.IsNullOrEmpty(startcaption))
                    {
                        str2 = num5.ToString();
                    }
                    else if (this.chkConst.Checked && ((this.nudNOLength.Value - this.txtNOStartCaption.Text.Length) > 0M))
                    {
                        str2 = string.Format("{0}{1}", startcaption, num5.ToString().PadLeft(((int) this.nudNOLength.Value) - this.txtNOStartCaption.Text.Length, '0'));
                    }
                    else
                    {
                        str2 = string.Format("{0}{1}", startcaption, num5.ToString());
                    }
                    num2 = consumer.addNew(str2.ToString(), "N" + num3.ToString(), groupID,
                        1, 0, 1, DateTime.Now, DateTime.Parse("2029-12-31"), "", num3, null, null);
                }
                else
                {
                    num3 = long.Parse(this.lstSwipe.Items[i].ToString().Substring(0, this.lstSwipe.Items[i].ToString().IndexOf("_")));
                    string str3 = this.lstSwipe.Items[i].ToString().Substring(this.lstSwipe.Items[i].ToString().IndexOf("_") + 1);
                    if (string.IsNullOrEmpty(str3))
                    {
                        str3 = "N" + num3.ToString();
                    }
                    num2 = consumer.addNew(num5.ToString(), str3, groupID, 1, 0, 1, 
                        DateTime.Now, DateTime.Parse("2029-12-31"), "", num3, null, null);
                }
                if (num2 >= 0)
                {
                    num5 += 1L;
                    num6++;
                }
                if ((num6 % 100) == 0)
                {
                    wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num6));
                    Application.DoEvents();
                }
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strAutoAddCard, num6));
            this.dfrmWait1.Hide();
            this.dfrmWait1.Refresh();
            Application.DoEvents();
            XMessageBox.Show(this, CommonStr.strAutoAddCard + "\r\n\r\n" + num6.ToString(), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return 1;
        }

        private delegate void txtInfoUpdate(object info);
    }
}

