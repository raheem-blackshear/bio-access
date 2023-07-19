namespace WG3000_COMM.ExtendFunc.Meeting
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmMeetingSign : frmBioAccess
    {
        private ArrayList arrControllerID = new ArrayList();
        private long[,] arrMeetingNum = new long[5, 5];
        private ArrayList arrSignedCardNo = new ArrayList();
        private ArrayList arrSignedSeat = new ArrayList();
        private ArrayList arrSignedUser = new ArrayList();
        private int cntTimer2;
        public string curMeetingNo = "";
        private DataSet ds = new DataSet();
        private frmConsole frmWatch;
        private int lastinfoRowsCount;
        public long lngDealtRecordID = -1L;
        private string meetingAdr = "";
        private string queryReaderStr = "";
        private Thread startSlowThread;

        public dfrmMeetingSign()
        {
            this.InitializeComponent();
        }

        private void _loadPhoto(string strCardno, ref PictureBox pic)
        {
            try
            {
                string str;
                if (strCardno.Trim() == "")
                {
                    str = null;
                }
                else
                {
                    str = wgAppConfig.getPhotoFileName(long.Parse(strCardno.Trim()));
                }
                Image img = pic.Image;
                wgAppConfig.ShowMyImage(str, ref img);
                pic.Image = img;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                dfrmManualSign sign = new dfrmManualSign();
                MethodInvoker method = new MethodInvoker(this.updateDisplay_Invoker);
                sign.curMeetingNo = this.curMeetingNo;
                if (sign.ShowDialog(this) == DialogResult.OK)
                {
                    this.fillMeetingNum();
                    base.BeginInvoke(method);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                base.TopMost = false;
                dfrmMeetingStatDetail detail = new dfrmMeetingStatDetail();
                MethodInvoker method = new MethodInvoker(this.updateDisplay_Invoker);
                detail.curMeetingNo = this.curMeetingNo;
                if (sender == this.Button1)
                {
                    detail.tabControl1.SelectedTab = detail.tabControl1.TabPages[0];
                }
                if (sender == this.Button2)
                {
                    detail.tabControl1.SelectedTab = detail.tabControl1.TabPages[1];
                }
                if (sender == this.Button3)
                {
                    detail.tabControl1.SelectedTab = detail.tabControl1.TabPages[2];
                }
                if (sender == this.Button4)
                {
                    detail.tabControl1.SelectedTab = detail.tabControl1.TabPages[3];
                }
                if (sender == this.Button5)
                {
                    detail.tabControl1.SelectedTab = detail.tabControl1.TabPages[5];
                }
                if (sender == this.Button7)
                {
                    detail.tabControl1.SelectedTab = detail.tabControl1.TabPages[5];
                }
                detail.ShowDialog(this);
                this.fillMeetingNum();
                base.BeginInvoke(method);
                base.TopMost = true;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (dfrmInterfaceLock @lock = new dfrmInterfaceLock())
            {
                @lock.txtOperatorName.Text = icOperator.OperatorName;
                @lock.StartPosition = FormStartPosition.CenterScreen;
                @lock.ShowDialog(this);
            }
        }

        private void dfrmMeetingSign_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.frmWatch != null)
                {
                    this.frmWatch.Close();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmMeetingSign_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.frmWatch != null)
                {
                    this.frmWatch.Close();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            try
            {
                if ((this.startSlowThread != null) && this.startSlowThread.IsAlive)
                {
                    this.startSlowThread.Interrupt();
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            wgAppConfig.DisposeImage(this.picSwipe1.Image);
            wgAppConfig.DisposeImage(this.picSwipe2.Image);
            wgAppConfig.DisposeImage(this.picSwipe3.Image);
            wgAppConfig.DisposeImage(this.picSwipe4.Image);
            wgAppConfig.DisposeImage(this.picSwipe5.Image);
            wgAppConfig.DisposeImage(this.picSwipe6.Image);
        }

        private void dfrmMeetingSign_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmMeetingSign_Load_Acc(sender, e);
            }
            else
            {
                try
                {
                    if (this.curMeetingNo == "")
                    {
                        base.Close();
                    }
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), connection).ExecuteReader();
                    if (reader.Read())
                    {
                        this.lblMeetingName.Text = wgTools.SetObjToStr(reader["f_MeetingName"]);
                        this.signStarttime = DateTime.Parse(Strings.Format(reader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
                        this.signEndtime = DateTime.Parse(Strings.Format(reader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
                        this.meetingAdr = wgTools.SetObjToStr(reader["f_MeetingAdr"]);
                    }
                    reader.Close();
                    connection.Close();
                    if (this.lblMeetingName.Text == "")
                    {
                        base.Close();
                    }
                    Application.DoEvents();
                    this.startSlowThread = new Thread(new ThreadStart(this.startSlow));
                    this.startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
                    this.startSlowThread.IsBackground = true;
                    this.startSlowThread.Start();
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        private void dfrmMeetingSign_Load_Acc(object sender, EventArgs e)
        {
            try
            {
                if (this.curMeetingNo == "")
                {
                    base.Close();
                }
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                OleDbDataReader reader = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), connection).ExecuteReader();
                if (reader.Read())
                {
                    this.lblMeetingName.Text = wgTools.SetObjToStr(reader["f_MeetingName"]);
                    this.signStarttime = DateTime.Parse(Strings.Format(reader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
                    this.signEndtime = DateTime.Parse(Strings.Format(reader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
                    this.meetingAdr = wgTools.SetObjToStr(reader["f_MeetingAdr"]);
                }
                reader.Close();
                connection.Close();
                if (this.lblMeetingName.Text == "")
                {
                    base.Close();
                }
                Application.DoEvents();
                this.startSlowThread = new Thread(new ThreadStart(this.startSlow));
                this.startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
                this.startSlowThread.IsBackground = true;
                this.startSlowThread.Start();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmMeetingSign_SizeChanged(object sender, EventArgs e)
        {
            GroupBox[] boxArray = new GroupBox[] { this.GroupBox2, this.GroupBox3, this.GroupBox4, this.GroupBox6, this.GroupBox7 };
            for (int i = 0; i < 5; i++)
            {
                boxArray[i].Size = new Size((this.flowLayoutPanel1.Width / 5) - 8, this.flowLayoutPanel1.Height - 0x12);
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

        public void fillMeetingNum()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.fillMeetingNum_Acc();
            }
            else
            {
                try
                {
                    SqlConnection selectConnection = new SqlConnection(wgAppConfig.dbConString);
                    if (selectConnection.State == ConnectionState.Closed)
                    {
                        selectConnection.Open();
                    }
                    for (int i = 0; i <= 4; i++)
                    {
                        this.arrMeetingNum[0, i] = 0L;
                        this.arrMeetingNum[1, i] = 0L;
                        this.arrMeetingNum[2, i] = 0L;
                        this.arrMeetingNum[3, i] = 0L;
                        this.arrMeetingNum[4, i] = 0L;
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT  a.*  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), selectConnection);
                    this.ds.Clear();
                    adapter.Fill(this.ds, "t_d_MeetingConsumer");
                    DataView view = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    for (int j = 0; j <= 3; j++)
                    {
                        view.RowFilter = " f_MeetingIdentity = " + j;
                        if (view.Count > 0)
                        {
                            this.arrMeetingNum[j, 0] = view.Count;
                            view.RowFilter = " f_MeetingIdentity = " + j + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
                            this.arrMeetingNum[j, 1] = view.Count;
                            view.RowFilter = " f_MeetingIdentity = " + j + " AND (f_SignWay = 2) ";
                            this.arrMeetingNum[j, 2] = view.Count;
                            this.arrMeetingNum[j, 3] = Math.Max((long) 0L, (long) ((this.arrMeetingNum[j, 0] - this.arrMeetingNum[j, 1]) - this.arrMeetingNum[j, 2]));
                            if (this.arrMeetingNum[j, 0] > 0L)
                            {
                                this.arrMeetingNum[j, 4] = (this.arrMeetingNum[j, 1] * 0x3e8L) / this.arrMeetingNum[j, 0];
                            }
                        }
                        this.arrMeetingNum[4, 0] += this.arrMeetingNum[j, 0];
                        this.arrMeetingNum[4, 1] += this.arrMeetingNum[j, 1];
                        this.arrMeetingNum[4, 2] += this.arrMeetingNum[j, 2];
                        this.arrMeetingNum[4, 3] += this.arrMeetingNum[j, 3];
                    }
                    if (this.arrMeetingNum[4, 0] > 0L)
                    {
                        this.arrMeetingNum[4, 4] = (this.arrMeetingNum[4, 1] * 0x3e8L) / this.arrMeetingNum[4, 0];
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        public void fillMeetingNum_Acc()
        {
            try
            {
                OleDbConnection selectConnection = new OleDbConnection(wgAppConfig.dbConString);
                if (selectConnection.State == ConnectionState.Closed)
                {
                    selectConnection.Open();
                }
                for (int i = 0; i <= 4; i++)
                {
                    this.arrMeetingNum[0, i] = 0L;
                    this.arrMeetingNum[1, i] = 0L;
                    this.arrMeetingNum[2, i] = 0L;
                    this.arrMeetingNum[3, i] = 0L;
                    this.arrMeetingNum[4, i] = 0L;
                }
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT  a.*  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), selectConnection);
                this.ds.Clear();
                adapter.Fill(this.ds, "t_d_MeetingConsumer");
                DataView view = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                for (int j = 0; j <= 3; j++)
                {
                    view.RowFilter = " f_MeetingIdentity = " + j;
                    if (view.Count > 0)
                    {
                        this.arrMeetingNum[j, 0] = view.Count;
                        view.RowFilter = " f_MeetingIdentity = " + j + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
                        this.arrMeetingNum[j, 1] = view.Count;
                        view.RowFilter = " f_MeetingIdentity = " + j + " AND (f_SignWay = 2) ";
                        this.arrMeetingNum[j, 2] = view.Count;
                        this.arrMeetingNum[j, 3] = Math.Max((long) 0L, (long) ((this.arrMeetingNum[j, 0] - this.arrMeetingNum[j, 1]) - this.arrMeetingNum[j, 2]));
                        if (this.arrMeetingNum[j, 0] > 0L)
                        {
                            this.arrMeetingNum[j, 4] = (this.arrMeetingNum[j, 1] * 0x3e8L) / this.arrMeetingNum[j, 0];
                        }
                    }
                    this.arrMeetingNum[4, 0] += this.arrMeetingNum[j, 0];
                    this.arrMeetingNum[4, 1] += this.arrMeetingNum[j, 1];
                    this.arrMeetingNum[4, 2] += this.arrMeetingNum[j, 2];
                    this.arrMeetingNum[4, 3] += this.arrMeetingNum[j, 3];
                }
                if (this.arrMeetingNum[4, 0] > 0L)
                {
                    this.arrMeetingNum[4, 4] = (this.arrMeetingNum[4, 1] * 0x3e8L) / this.arrMeetingNum[4, 0];
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        public void fillMeetingRecord(string MeetingNo)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.fillMeetingRecord_Acc(MeetingNo);
            }
            else
            {
                try
                {
                    string str;
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(MeetingNo), connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        this.signStarttime = (DateTime) reader["f_SignStartTime"];
                        this.signEndtime = (DateTime) reader["f_SignEndTime"];
                        this.meetingAdr = wgTools.SetObjToStr(reader["f_MeetingAdr"]);
                    }
                    reader.Close();
                    if ((this.lngDealtRecordID == -1L) && (this.meetingAdr != ""))
                    {
                        this.queryReaderStr = "";
                        str = "Select t_b_reader.* from t_b_reader,t_d_MeetingAdr  ";
                        command = new SqlCommand((str + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID ") + " AND t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.meetingAdr), connection);
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (this.queryReaderStr == "")
                                {
                                    this.queryReaderStr = " f_ReaderID IN ( " + reader["f_ReaderID"];
                                }
                                else
                                {
                                    this.queryReaderStr = this.queryReaderStr + " , " + reader["f_ReaderID"];
                                }
                                if (this.arrControllerID.IndexOf(reader["f_ControllerID"]) < 0)
                                {
                                    this.arrControllerID.Add(reader["f_ControllerID"]);
                                }
                            }
                            this.queryReaderStr = this.queryReaderStr + ")";
                        }
                        reader.Close();
                    }
                    if (this.lngDealtRecordID == -1L)
                    {
                        this.lngDealtRecordID = 0L;
                    }
                    string str2 = "";
                    str2 = (str2 + " ([f_ReadDate]>= " + wgTools.PrepareStr(this.signStarttime, true, "yyyy-MM-dd H:mm:ss") + ")") + " AND ([f_ReadDate]<= " + wgTools.PrepareStr(this.signEndtime, true, "yyyy-MM-dd H:mm:ss") + ")";
                    string str3 = " SELECT t_d_SwipeRecord.f_RecID,  ";
                    str3 = (((str3 + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, " + 
                        "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, ") + 
                        "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity, t_d_SwipeRecord.f_ConsumerID " +
                        string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + 
                        wgTools.PrepareStr(MeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0])) + 
                        " WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID.ToString()) + 
                        " AND  t_d_SwipeRecord.f_ConsumerID IN (SELECT f_ConsumerID FROM t_d_MeetingConsumer WHERE f_SignWay=0 AND f_RecID =0 AND  f_MeetingNO = " + 
                        wgTools.PrepareStr(MeetingNo) + " )  ";
                    if (this.queryReaderStr != "")
                    {
                        str3 = str3 + " AND " + this.queryReaderStr;
                    }
                    command = new SqlCommand(str3 + " AND " + str2, connection);
                    reader = command.ExecuteReader();
                    ArrayList list = new ArrayList();
                    ArrayList list2 = new ArrayList();
                    ArrayList list3 = new ArrayList();
                    if (reader.HasRows)
                    {
                        int index = -1;
                        while (reader.Read())
                        {
                            index = list.IndexOf(reader["f_ConsumerID"]);
                            if (index < 0)
                            {
                                list.Add(reader["f_ConsumerID"]);
                                list2.Add((DateTime) reader["f_ReadDate"]);
                                list3.Add(reader["f_RecID"]);
                            }
                            else if (((DateTime) list2[index]) > ((DateTime) reader["f_ReadDate"]))
                            {
                                list2[index] = (DateTime) reader["f_ReadDate"];
                                list3[index] = reader["f_RecID"];
                            }
                        }
                    }
                    reader.Close();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            str = " UPDATE t_d_MeetingConsumer ";
                            object obj2 = (str + " SET [f_SignRealTime] = " + wgTools.PrepareStr((DateTime) list2[i], true, "yyyy-MM-dd H:mm:ss")) + " ,[f_RecID] = " + list3[i];
                            str = string.Concat(new object[] { obj2, " WHERE f_ConsumerID = ", list[i], " AND  f_MeetingNO = ", wgTools.PrepareStr(MeetingNo) });
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                    str = "SELECT f_RecID from t_d_SwipeRecord ORDER BY f_RecID DESC ";
                    reader = new SqlCommand(str, connection).ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        this.lngDealtRecordID = long.Parse(reader["f_RecID"].ToString());
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        public void fillMeetingRecord_Acc(string MeetingNo)
        {
            try
            {
                string str;
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                OleDbCommand command = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(MeetingNo), connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    this.signStarttime = (DateTime) reader["f_SignStartTime"];
                    this.signEndtime = (DateTime) reader["f_SignEndTime"];
                    this.meetingAdr = wgTools.SetObjToStr(reader["f_MeetingAdr"]);
                }
                reader.Close();
                if ((this.lngDealtRecordID == -1L) && (this.meetingAdr != ""))
                {
                    this.queryReaderStr = "";
                    str = "Select t_b_reader.* from t_b_reader,t_d_MeetingAdr  ";
                    reader = new OleDbCommand((str + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID ") + " AND t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.meetingAdr), connection).ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (this.queryReaderStr == "")
                            {
                                this.queryReaderStr = " f_ReaderID IN ( " + reader["f_ReaderID"];
                            }
                            else
                            {
                                this.queryReaderStr = this.queryReaderStr + " , " + reader["f_ReaderID"];
                            }
                            if (this.arrControllerID.IndexOf(reader["f_ControllerID"]) < 0)
                            {
                                this.arrControllerID.Add(reader["f_ControllerID"]);
                            }
                        }
                        this.queryReaderStr = this.queryReaderStr + ")";
                    }
                    reader.Close();
                }
                if (this.lngDealtRecordID == -1L)
                {
                    this.lngDealtRecordID = 0L;
                }
                string str2 = "";
                str2 = (str2 + " ([f_ReadDate]>= " + wgTools.PrepareStr(this.signStarttime, true, "yyyy-MM-dd H:mm:ss") + ")") + " AND ([f_ReadDate]<= " + wgTools.PrepareStr(this.signEndtime, true, "yyyy-MM-dd H:mm:ss") + ")";
                string str3 = " SELECT t_d_SwipeRecord.f_RecID,  ";
                str3 = (((str3 + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, " + 
                    "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, ") + 
                    "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity, t_d_SwipeRecord.f_ConsumerID " + 
                    string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + 
                    wgTools.PrepareStr(MeetingNo) + ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0])) + 
                    " WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID.ToString()) + 
                    " AND  t_d_SwipeRecord.f_ConsumerID IN (SELECT f_ConsumerID FROM t_d_MeetingConsumer WHERE f_SignWay=0 AND f_RecID =0 AND  f_MeetingNO = " + 
                    wgTools.PrepareStr(MeetingNo) + " )  ";
                if (this.queryReaderStr != "")
                {
                    str3 = str3 + " AND " + this.queryReaderStr;
                }
                command = new OleDbCommand(str3 + " AND " + str2, connection);
                reader = command.ExecuteReader();
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                ArrayList list3 = new ArrayList();
                if (reader.HasRows)
                {
                    int index = -1;
                    while (reader.Read())
                    {
                        index = list.IndexOf(reader["f_ConsumerID"]);
                        if (index < 0)
                        {
                            list.Add(reader["f_ConsumerID"]);
                            list2.Add((DateTime) reader["f_ReadDate"]);
                            list3.Add(reader["f_RecID"]);
                        }
                        else if (((DateTime) list2[index]) > ((DateTime) reader["f_ReadDate"]))
                        {
                            list2[index] = (DateTime) reader["f_ReadDate"];
                            list3[index] = reader["f_RecID"];
                        }
                    }
                }
                reader.Close();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        str = " UPDATE t_d_MeetingConsumer ";
                        object obj2 = (str + " SET [f_SignRealTime] = " + wgTools.PrepareStr((DateTime) list2[i], true, "yyyy-MM-dd H:mm:ss")) + " ,[f_RecID] = " + list3[i];
                        str = string.Concat(new object[] { obj2, " WHERE f_ConsumerID = ", list[i], " AND  f_MeetingNO = ", wgTools.PrepareStr(MeetingNo) });
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
                str = "SELECT f_RecID from t_d_SwipeRecord ORDER BY f_RecID DESC ";
                reader = new OleDbCommand(str, connection).ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    this.lngDealtRecordID = long.Parse(reader["f_RecID"].ToString());
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        public void getNewMeetingRecord()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.getNewMeetingRecord_Acc();
            }
            else
            {
                try
                {
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    string str2 = " SELECT t_d_SwipeRecord.f_RecID,   ";
                    str2 = ((str2 + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, " + 
                        "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, ") + 
                        "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity " +
                        string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " +
                        wgTools.PrepareStr(this.curMeetingNo) + 
                        ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0])) + 
                        " WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID.ToString();
                    if (this.queryReaderStr != "")
                    {
                        str2 = str2 + " AND " + this.queryReaderStr;
                    }
                    string cmdText = str2;
                    SqlDataReader reader = new SqlCommand(cmdText, connection).ExecuteReader();
                    bool flag = false;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string str3 = wgTools.SetObjToStr(reader["f_ConsumerName"]);
                            MjRec rec = new MjRec(reader["f_RecordAll"].ToString(), false);
                            if (rec.IsSwipeRecord)
                            {
                                flag = true;
                                if (str3 != "")
                                {
                                    if (wgTools.SetObjToStr(reader["f_MeetingIdentity"]) != "")
                                    {
                                        str3 = str3 + "." + frmMeetings.getStrMeetingIdentity(long.Parse(reader["f_MeetingIdentity"].ToString()));
                                    }
                                    this.arrSignedUser.Add(str3);
                                    this.arrSignedSeat.Add(wgTools.SetObjToStr(reader["f_Seat"]));
                                    this.arrSignedCardNo.Add(wgTools.SetObjToStr(reader["f_ConsumerNO"]));
                                }
                                else
                                {
                                    str3 = wgTools.SetObjToStr(reader["f_CardNO"]);
                                    this.arrSignedUser.Add(str3);
                                    this.arrSignedSeat.Add("!!!");
                                    this.arrSignedCardNo.Add(wgTools.SetObjToStr(reader["f_ConsumerNO"]));
                                }
                            }
                        }
                        if (this.arrSignedUser.Count > 100)
                        {
                            while (this.arrSignedUser.Count > 50)
                            {
                                this.arrSignedUser.RemoveAt(0);
                                this.arrSignedSeat.RemoveAt(0);
                                this.arrSignedCardNo.RemoveAt(0);
                            }
                        }
                    }
                    reader.Close();
                    connection.Close();
                    if (flag)
                    {
                        int count;
                        if (this.arrSignedUser.Count > 0)
                        {
                            count = this.arrSignedUser.Count;
                            this.txtSwipeUser1.Text = this.arrSignedUser[count - 1].ToString();
                            this.txtSwipeSeat1.Text = this.arrSignedSeat[count - 1].ToString();
                            this._loadPhoto(this.arrSignedCardNo[count - 1].ToString(), ref this.picSwipe1);
                        }
                        if (this.arrSignedUser.Count > 1)
                        {
                            count = this.arrSignedUser.Count;
                            this.txtSwipeUser2.Text = this.arrSignedUser[count - 2].ToString();
                            this.txtSwipeSeat2.Text = this.arrSignedSeat[count - 2].ToString();
                            this._loadPhoto(this.arrSignedCardNo[count - 2].ToString(), ref this.picSwipe2);
                        }
                        if (this.arrSignedUser.Count > 2)
                        {
                            count = this.arrSignedUser.Count;
                            this.txtSwipeUser3.Text = this.arrSignedUser[count - 3].ToString();
                            this.txtSwipeSeat3.Text = this.arrSignedSeat[count - 3].ToString();
                            this._loadPhoto(this.arrSignedCardNo[count - 3].ToString(), ref this.picSwipe3);
                        }
                        if (this.arrSignedUser.Count > 3)
                        {
                            count = this.arrSignedUser.Count;
                            this.txtSwipeUser4.Text = this.arrSignedUser[count - 4].ToString();
                            this.txtSwipeSeat4.Text = this.arrSignedSeat[count - 4].ToString();
                            this._loadPhoto(this.arrSignedCardNo[count - 4].ToString(), ref this.picSwipe4);
                        }
                        if (this.arrSignedUser.Count > 4)
                        {
                            count = this.arrSignedUser.Count;
                            this.txtSwipeUser5.Text = this.arrSignedUser[count - 5].ToString();
                            this.txtSwipeSeat5.Text = this.arrSignedSeat[count - 5].ToString();
                            this._loadPhoto(this.arrSignedCardNo[count - 5].ToString(), ref this.picSwipe5);
                        }
                        if (this.arrSignedUser.Count > 5)
                        {
                            count = this.arrSignedUser.Count;
                            this.txtSwipeUser6.Text = this.arrSignedUser[count - 6].ToString();
                            this.txtSwipeSeat6.Text = this.arrSignedSeat[count - 6].ToString();
                            this._loadPhoto(this.arrSignedCardNo[count - 6].ToString(), ref this.picSwipe6);
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        public void getNewMeetingRecord_Acc()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string str2 = " SELECT t_d_SwipeRecord.f_RecID,   ";
                str2 = ((str2 + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, " +
                    "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, ") + 
                    "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll, t_d_MeetingConsumer.f_Seat, t_d_MeetingConsumer.f_MeetingIdentity " + 
                    string.Format(" FROM ((t_b_Consumer INNER JOIN t_d_SwipeRecord ON ( t_b_Consumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID)) LEFT JOIN  t_d_MeetingConsumer on ( t_d_MeetingConsumer.f_ConsumerID = t_d_SwipeRecord.f_ConsumerID AND  f_MeetingNO = " + 
                    wgTools.PrepareStr(this.curMeetingNo) + 
                    ") ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", new object[0])) + 
                    " WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID.ToString();
                if (this.queryReaderStr != "")
                {
                    str2 = str2 + " AND " + this.queryReaderStr;
                }
                string cmdText = str2;
                OleDbDataReader reader = new OleDbCommand(cmdText, connection).ExecuteReader();
                bool flag = false;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string str3 = wgTools.SetObjToStr(reader["f_ConsumerName"]);
                        MjRec rec = new MjRec(reader["f_RecordAll"].ToString(), false);
                        if (rec.IsSwipeRecord)
                        {
                            flag = true;
                            if (str3 != "")
                            {
                                if (wgTools.SetObjToStr(reader["f_MeetingIdentity"]) != "")
                                {
                                    str3 = str3 + "." + frmMeetings.getStrMeetingIdentity(long.Parse(reader["f_MeetingIdentity"].ToString()));
                                }
                                this.arrSignedUser.Add(str3);
                                this.arrSignedSeat.Add(wgTools.SetObjToStr(reader["f_Seat"]));
                                this.arrSignedCardNo.Add(wgTools.SetObjToStr(reader["f_ConsumerNO"]));
                            }
                            else
                            {
                                str3 = wgTools.SetObjToStr(reader["f_CardNO"]);
                                this.arrSignedUser.Add(str3);
                                this.arrSignedSeat.Add("!!!");
                                this.arrSignedCardNo.Add(wgTools.SetObjToStr(reader["f_ConsumerNO"]));
                            }
                        }
                    }
                    if (this.arrSignedUser.Count > 100)
                    {
                        while (this.arrSignedUser.Count > 50)
                        {
                            this.arrSignedUser.RemoveAt(0);
                            this.arrSignedSeat.RemoveAt(0);
                            this.arrSignedCardNo.RemoveAt(0);
                        }
                    }
                }
                reader.Close();
                connection.Close();
                if (flag)
                {
                    int count;
                    if (this.arrSignedUser.Count > 0)
                    {
                        count = this.arrSignedUser.Count;
                        this.txtSwipeUser1.Text = this.arrSignedUser[count - 1].ToString();
                        this.txtSwipeSeat1.Text = this.arrSignedSeat[count - 1].ToString();
                        this._loadPhoto(this.arrSignedCardNo[count - 1].ToString(), ref this.picSwipe1);
                    }
                    if (this.arrSignedUser.Count > 1)
                    {
                        count = this.arrSignedUser.Count;
                        this.txtSwipeUser2.Text = this.arrSignedUser[count - 2].ToString();
                        this.txtSwipeSeat2.Text = this.arrSignedSeat[count - 2].ToString();
                        this._loadPhoto(this.arrSignedCardNo[count - 2].ToString(), ref this.picSwipe2);
                    }
                    if (this.arrSignedUser.Count > 2)
                    {
                        count = this.arrSignedUser.Count;
                        this.txtSwipeUser3.Text = this.arrSignedUser[count - 3].ToString();
                        this.txtSwipeSeat3.Text = this.arrSignedSeat[count - 3].ToString();
                        this._loadPhoto(this.arrSignedCardNo[count - 3].ToString(), ref this.picSwipe3);
                    }
                    if (this.arrSignedUser.Count > 3)
                    {
                        count = this.arrSignedUser.Count;
                        this.txtSwipeUser4.Text = this.arrSignedUser[count - 4].ToString();
                        this.txtSwipeSeat4.Text = this.arrSignedSeat[count - 4].ToString();
                        this._loadPhoto(this.arrSignedCardNo[count - 4].ToString(), ref this.picSwipe4);
                    }
                    if (this.arrSignedUser.Count > 4)
                    {
                        count = this.arrSignedUser.Count;
                        this.txtSwipeUser5.Text = this.arrSignedUser[count - 5].ToString();
                        this.txtSwipeSeat5.Text = this.arrSignedSeat[count - 5].ToString();
                        this._loadPhoto(this.arrSignedCardNo[count - 5].ToString(), ref this.picSwipe5);
                    }
                    if (this.arrSignedUser.Count > 5)
                    {
                        count = this.arrSignedUser.Count;
                        this.txtSwipeUser6.Text = this.arrSignedUser[count - 6].ToString();
                        this.txtSwipeSeat6.Text = this.arrSignedSeat[count - 6].ToString();
                        this._loadPhoto(this.arrSignedCardNo[count - 6].ToString(), ref this.picSwipe6);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void GroupBox1_SizeChanged(object sender, EventArgs e)
        {
        }

        public void startSlow()
        {
            MethodInvoker method = new MethodInvoker(this.startSlow_Invoker);
            try
            {
                Application.DoEvents();
                Thread.Sleep(0x3e8);
                this.fillMeetingRecord(this.curMeetingNo);
                this.fillMeetingNum();
                base.BeginInvoke(method);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        public void startSlow_Invoker()
        {
            try
            {
                int num = 0;
                this.txtA0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtA1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtA2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtA3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtA4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 1;
                this.txtB0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtB1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtB2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtB3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtB4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 2;
                this.txtC0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtC1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtC2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtC3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtC4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 3;
                this.txtD0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtD1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtD2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtD3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtD4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 4;
                this.txtE0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtE1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtE2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtE3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtE4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                if (this.frmWatch == null)
                {
                    this.frmWatch = new frmConsole();
                    this.frmWatch.arrSelectDoors4Sign = (ArrayList) this.arrControllerID.Clone();
                    this.frmWatch.WindowState = FormWindowState.Minimized;
                    this.frmWatch.Show();
                    this.frmWatch.Visible = false;
                    this.frmWatch.directToRealtimeGet();
                }
                foreach (object obj2 in this.GroupBox5.Controls)
                {
                    if ((obj2.GetType().Name.ToString() == "TextBox") && (((TextBox) obj2).Text == "0"))
                    {
                        ((TextBox) obj2).Text = "";
                    }
                }
                if (string.IsNullOrEmpty(this.txtA0.Text))
                {
                    this.txtA4.Text = "";
                }
                if (string.IsNullOrEmpty(this.txtB0.Text))
                {
                    this.txtB4.Text = "";
                }
                if (string.IsNullOrEmpty(this.txtC0.Text))
                {
                    this.txtC4.Text = "";
                }
                if (string.IsNullOrEmpty(this.txtD0.Text))
                {
                    this.txtD4.Text = "";
                }
                if (string.IsNullOrEmpty(this.txtE0.Text))
                {
                    this.txtE4.Text = "";
                }
                this.Timer2.Enabled = true;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.TimerStartSlow.Enabled = false;
            Cursor.Current = Cursors.Default;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.Timer1.Enabled = false;
            try
            {
                this.lblTime.Text = Strings.Format(DateTime.Now, "HH:mm:ss");
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.Timer1.Enabled = true;
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            this.Timer2.Enabled = false;
            try
            {
                this.cntTimer2++;
                if ((this.frmWatch.getAllInfoRowsCount() == this.lastinfoRowsCount) && (this.cntTimer2 <= (600 / this.Timer2.Interval)))
                {
                    goto Label_01F5;
                }
                this.cntTimer2 = 0;
                this.lastinfoRowsCount = this.frmWatch.getAllInfoRowsCount();
                MethodInvoker method = new MethodInvoker(this.updateDisplay_Invoker);
                object valA = null;
                string cmdText = "SELECT f_RecID from t_d_SwipeRecord WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID;
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            connection.Open();
                            valA = command.ExecuteScalar();
                            connection.Close();
                        }
                        goto Label_0117;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        valA = command2.ExecuteScalar();
                        connection2.Close();
                    }
                }
            Label_0117:
                if (wgTools.SetObjToStr(valA) != "")
                {
                    this.getNewMeetingRecord();
                    this.fillMeetingRecord(this.curMeetingNo);
                    this.fillMeetingNum();
                    base.BeginInvoke(method);
                }
                else
                {
                    try
                    {
                        if (this.frmWatch == null)
                        {
                            goto Label_01F5;
                        }
                        ListView lstDoors = this.frmWatch.lstDoors;
                        bool flag = false;
                        for (int i = 0; i <= (lstDoors.Items.Count - 1); i++)
                        {
                            if (lstDoors.Items[i].ImageIndex == 3)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            this.btnErrConnect.Visible = flag ^ this.btnErrConnect.Visible;
                        }
                        else
                        {
                            this.btnErrConnect.Visible = flag;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        Label_01F5:
            this.Timer2.Enabled = true;
        }

        private void Timer2_Tick_Acc(object sender, EventArgs e)
        {
            this.Timer2.Enabled = false;
            try
            {
                MethodInvoker method = new MethodInvoker(this.updateDisplay_Invoker);
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                OleDbCommand command = new OleDbCommand("SELECT f_RecID from t_d_SwipeRecord WHERE t_d_SwipeRecord.f_RecID > " + this.lngDealtRecordID, connection);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                object valA = command.ExecuteScalar();
                connection.Close();
                if (wgTools.SetObjToStr(valA) != "")
                {
                    this.getNewMeetingRecord();
                    this.fillMeetingRecord(this.curMeetingNo);
                    this.fillMeetingNum();
                    base.BeginInvoke(method);
                }
                else
                {
                    try
                    {
                        if (this.frmWatch != null)
                        {
                            ListView lstDoors = this.frmWatch.lstDoors;
                            bool flag = false;
                            for (int i = 0; i <= (lstDoors.Items.Count - 1); i++)
                            {
                                if (lstDoors.Items[i].ImageIndex == 3)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                this.btnErrConnect.Visible = flag ^ this.btnErrConnect.Visible;
                            }
                            else
                            {
                                this.btnErrConnect.Visible = flag;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.Timer2.Enabled = true;
        }

        private void TimerStartSlow_Tick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        public void updateDisplay_Invoker()
        {
            try
            {
                int num = 0;
                this.txtA0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtA1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtA2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtA3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtA4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 1;
                this.txtB0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtB1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtB2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtB3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtB4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 2;
                this.txtC0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtC1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtC2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtC3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtC4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 3;
                this.txtD0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtD1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtD2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtD3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtD4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                num = 4;
                this.txtE0.Text = this.arrMeetingNum[num, 0].ToString();
                this.txtE1.Text = this.arrMeetingNum[num, 1].ToString();
                this.txtE2.Text = this.arrMeetingNum[num, 2].ToString();
                this.txtE3.Text = this.arrMeetingNum[num, 3].ToString();
                this.txtE4.Text = ((this.arrMeetingNum[num, 4] / 10L)).ToString() + "%";
                foreach (object obj2 in this.GroupBox5.Controls)
                {
                    if ((obj2.GetType().Name.ToString() == "TextBox") && (((TextBox) obj2).Text == "0"))
                    {
                        ((TextBox) obj2).Text = "";
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }
    }
}

