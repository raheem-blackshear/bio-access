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
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMeetingStatDetail : frmBioAccess
    {
        private ArrayList arrControllerID = new ArrayList();
        private long[,] arrMeetingNum = new long[7, 6];
        private ArrayList arrSignedCardNo = new ArrayList();
        private ArrayList arrSignedSeat = new ArrayList();
        private ArrayList arrSignedUser = new ArrayList();
        public string curMeetingNo = "";
        private dfrmFind dfrmFind1 = new dfrmFind();
        private DataSet ds = new DataSet();
        private DataView dvAbsent;
        private DataView dvInFact;
        private DataView dvLate;
        private DataView dvLeave;
        private DataView dvShould;
        private long lngDealtRecordID = -1L;
        private string meetingAdr = "";
        private string meetingName = "";
        private string queryReaderStr = "";
        public int selectedPage = -1;

        public dfrmMeetingStatDetail()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.Visible)
            {
                wgAppConfig.exportToExcel(this.dgvMain, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
            }
            else
            {
                wgAppConfig.exportToExcel(this.dgvStat, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            try
            {
                using (dfrmManualSign sign = new dfrmManualSign())
                {
                    sign.curMeetingNo = this.curMeetingNo;
                    sign.curMode = "Leave";
                    sign.Text = this.btnLeave.Text;
                    if (sign.ShowDialog(this) == DialogResult.OK)
                    {
                        int selectedIndex = this.tabControl1.SelectedIndex;
                        this.fillMeetingNum();
                        this.tabControl1.SelectedIndex = 2;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnManualSign_Click(object sender, EventArgs e)
        {
            try
            {
                using (dfrmManualSign sign = new dfrmManualSign())
                {
                    sign.curMeetingNo = this.curMeetingNo;
                    sign.Text = this.btnManualSign.Text;
                    if (sign.ShowDialog(this) == DialogResult.OK)
                    {
                        int selectedIndex = this.tabControl1.SelectedIndex;
                        this.fillMeetingNum();
                        this.tabControl1.SelectedIndex = selectedIndex;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvMain.Visible)
                {
                    wgAppConfig.printdgv(this.dgvMain, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
                }
                else
                {
                    wgAppConfig.printdgv(this.dgvStat, this.Text + " ( " + this.tabControl1.SelectedTab.Text + " )");
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRecreate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                wgAppConfig.runUpdateSql((" UPDATE t_d_MeetingConsumer " + " SET [f_SignRealTime] = NULL " + " ,[f_RecID] = 0 ") + " WHERE f_SignWay = 0 AND  f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo));
                this.lngDealtRecordID = -1L;
                this.fillMeetingRecord(this.curMeetingNo);
                int selectedIndex = this.tabControl1.SelectedIndex;
                this.fillMeetingNum();
                this.tabControl1.SelectedIndex = selectedIndex;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.Cursor = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.tabControl1.SelectedIndex;
            this.fillMeetingNum();
            this.tabControl1.SelectedIndex = selectedIndex;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void dfrmMeetingStatDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmMeetingStatDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                    }
                    this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmStd_Load(object sender, EventArgs e)
        {
            bool bReadOnly = false;
            string funName = "mnuMeeting";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnManualSign.Enabled = false;
                this.btnLeave.Enabled = false;
            }
            try
            {
                if (this.curMeetingNo == "")
                {
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
                }
                TabPage selectedTab = this.tabControl1.SelectedTab;
                this.fillMeetingNum();
                if (!string.IsNullOrEmpty(this.meetingName))
                {
                    this.Text = this.Text + "[" + this.meetingName + "]";
                }
                this.tabControl1.SelectedTab = selectedTab;
                this.tabControl1_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
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
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), connection).ExecuteReader();
                    DateTime now = DateTime.Now;
                    if (reader.Read())
                    {
                        now = (DateTime) reader["f_MeetingDateTime"];
                        this.meetingName = reader["f_MeetingName"].ToString();
                    }
                    reader.Close();
                    int num = 0;
                    while (num <= 5)
                    {
                        this.arrMeetingNum[0, num] = 0L;
                        this.arrMeetingNum[1, num] = 0L;
                        this.arrMeetingNum[2, num] = 0L;
                        this.arrMeetingNum[3, num] = 0L;
                        this.arrMeetingNum[4, num] = 0L;
                        this.arrMeetingNum[5, num] = 0L;
                        this.arrMeetingNum[6, num] = 0L;
                        num++;
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT  a.f_RecID,b.f_ConsumerName, '' as f_MeetingIdentityStr, a.f_Seat, a.f_SignRealTime,'' as f_SignWayStr, a.f_SignWay, a.f_MeetingIdentity  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), connection);
                    this.ds.Clear();
                    adapter.Fill(this.ds, "t_d_MeetingConsumer");
                    DataView view = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    int num2 = 0;
                    while (num2 <= (view.Count - 1))
                    {
                        this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_MeetingIdentityStr"] = frmMeetings.getStrMeetingIdentity(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_MeetingIdentity"].ToString()));
                        this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_SignWayStr"] = frmMeetings.getStrSignWay(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_SignWay"].ToString()));
                        num2++;
                    }
                    num2 = 0;
                    while (num2 <= 5)
                    {
                        view.RowFilter = " f_MeetingIdentity = " + num2;
                        if (view.Count > 0)
                        {
                            this.arrMeetingNum[num2, 0] = view.Count;
                            view.RowFilter = " f_MeetingIdentity = " + num2 + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
                            this.arrMeetingNum[num2, 1] = view.Count;
                            view.RowFilter = " f_MeetingIdentity = " + num2 + " AND (f_SignWay = 2) ";
                            this.arrMeetingNum[num2, 2] = view.Count;
                            this.arrMeetingNum[num2, 3] = Math.Max((long) 0L, (long) ((this.arrMeetingNum[num2, 0] - this.arrMeetingNum[num2, 1]) - this.arrMeetingNum[num2, 2]));
                            view.RowFilter = string.Concat(new object[] { " f_MeetingIdentity = ", num2, " AND  ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1))  AND f_SignRealTime > ", Strings.Format(now, "#yyyy-MM-dd HH:mm:ss#") });
                            this.arrMeetingNum[num2, 4] = view.Count;
                            if (this.arrMeetingNum[num2, 0] > 0L)
                            {
                                this.arrMeetingNum[num2, 5] = (this.arrMeetingNum[num2, 1] * 0x3e8L) / this.arrMeetingNum[num2, 0];
                            }
                        }
                        this.arrMeetingNum[6, 0] += this.arrMeetingNum[num2, 0];
                        this.arrMeetingNum[6, 1] += this.arrMeetingNum[num2, 1];
                        this.arrMeetingNum[6, 2] += this.arrMeetingNum[num2, 2];
                        this.arrMeetingNum[6, 3] += this.arrMeetingNum[num2, 3];
                        this.arrMeetingNum[6, 4] += this.arrMeetingNum[num2, 4];
                        this.arrMeetingNum[6, 5] += this.arrMeetingNum[num2, 5];
                        num2++;
                    }
                    if (this.arrMeetingNum[6, 0] > 0L)
                    {
                        this.arrMeetingNum[6, 5] = (this.arrMeetingNum[6, 1] * 0x3e8L) / this.arrMeetingNum[6, 0];
                    }
                    this.dvShould = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    this.dvInFact = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    this.dvLeave = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    this.dvAbsent = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    this.dvLate = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                    this.dvShould.RowFilter = "";
                    this.dvInFact.RowFilter = "( f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1) ";
                    this.dvLeave.RowFilter = " f_SignWay = 2 ";
                    this.dvAbsent.RowFilter = " f_SignWay =0 AND f_RecID <=0  ";
                    this.dvLate.RowFilter = " ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) AND f_SignRealTime > " + Strings.Format(now, "#yyyy-MM-dd HH:mm:ss#");
                    DataTable table = new DataTable("Stat");
                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingIdentity";
                    table.Columns.Add(column);
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingShould";
                    table.Columns.Add(column);
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingInFact";
                    table.Columns.Add(column);
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingLeave";
                    table.Columns.Add(column);
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingAbsent";
                    table.Columns.Add(column);
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingLate";
                    table.Columns.Add(column);
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "strMeetingRatio";
                    table.Columns.Add(column);
                    for (num2 = 0; num2 <= 6; num2++)
                    {
                        DataRow row = table.NewRow();
                        if (num2 == 6)
                        {
                            row[0] = CommonStr.strMeetingSubTotal;
                        }
                        else
                        {
                            row[0] = frmMeetings.getStrMeetingIdentity((long) num2);
                        }
                        num = 0;
                        while (num <= 4)
                        {
                            row[num + 1] = this.arrMeetingNum[num2, num];
                            if (row[num + 1].ToString() == "0")
                            {
                                row[num + 1] = "";
                            }
                            num++;
                        }
                        row[6] = (this.arrMeetingNum[num2, 5] / 10L) + "%";
                        if (string.IsNullOrEmpty(row[1].ToString()))
                        {
                            row[6] = "";
                        }
                        table.Rows.Add(row);
                    }
                    DataTable table2 = this.ds.Tables["t_d_MeetingConsumer"];
                    this.dgvMain.AutoGenerateColumns = false;
                    num = 0;
                    while ((num < table2.Columns.Count) && (num < this.dgvMain.ColumnCount))
                    {
                        this.dgvMain.Columns[num].DataPropertyName = table2.Columns[num].ColumnName;
                        num++;
                    }
                    this.dgvMain.DataSource = this.dvShould;
                    this.dgvMain.DefaultCellStyle.ForeColor = Color.Black;
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_SignRealTime", wgTools.DisplayFormat_DateYMDHMSWeek);
                    this.dgvStat.AutoGenerateColumns = false;
                    for (num = 0; (num < table.Columns.Count) && (num < this.dgvStat.ColumnCount); num++)
                    {
                        this.dgvStat.Columns[num].DataPropertyName = table.Columns[num].ColumnName;
                    }
                    this.dgvStat.DataSource = table;
                    this.dgvStat.DefaultCellStyle.ForeColor = Color.Black;
                    this.tabControl1.SelectedIndex = 5;
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
                int num2;
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                OleDbDataReader reader = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), connection).ExecuteReader();
                DateTime now = DateTime.Now;
                if (reader.Read())
                {
                    now = (DateTime) reader["f_MeetingDateTime"];
                    this.meetingName = reader["f_MeetingName"].ToString();
                }
                reader.Close();
                int num = 0;
                while (num <= 5)
                {
                    this.arrMeetingNum[0, num] = 0L;
                    this.arrMeetingNum[1, num] = 0L;
                    this.arrMeetingNum[2, num] = 0L;
                    this.arrMeetingNum[3, num] = 0L;
                    this.arrMeetingNum[4, num] = 0L;
                    this.arrMeetingNum[5, num] = 0L;
                    this.arrMeetingNum[6, num] = 0L;
                    num++;
                }
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT  a.f_RecID,b.f_ConsumerName, '' as f_MeetingIdentityStr, a.f_Seat, a.f_SignRealTime,'' as f_SignWayStr, a.f_SignWay, a.f_MeetingIdentity  FROM t_d_MeetingConsumer a, t_b_Consumer b WHERE a.f_ConsumerID=b.f_ConsumerID and a.f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo), connection);
                this.ds.Clear();
                adapter.Fill(this.ds, "t_d_MeetingConsumer");
                DataView view = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                for (num2 = 0; num2 <= (view.Count - 1); num2++)
                {
                    this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_MeetingIdentityStr"] = frmMeetings.getStrMeetingIdentity(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_MeetingIdentity"].ToString()));
                    this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_SignWayStr"] = frmMeetings.getStrSignWay(long.Parse(this.ds.Tables["t_d_MeetingConsumer"].Rows[num2]["f_SignWay"].ToString()));
                }
                for (num2 = 0; num2 <= 5; num2++)
                {
                    view.RowFilter = " f_MeetingIdentity = " + num2;
                    if (view.Count > 0)
                    {
                        this.arrMeetingNum[num2, 0] = view.Count;
                        view.RowFilter = " f_MeetingIdentity = " + num2 + " AND ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) ";
                        this.arrMeetingNum[num2, 1] = view.Count;
                        view.RowFilter = " f_MeetingIdentity = " + num2 + " AND (f_SignWay = 2) ";
                        this.arrMeetingNum[num2, 2] = view.Count;
                        this.arrMeetingNum[num2, 3] = Math.Max((long) 0L, (long) ((this.arrMeetingNum[num2, 0] - this.arrMeetingNum[num2, 1]) - this.arrMeetingNum[num2, 2]));
                        view.RowFilter = string.Concat(new object[] { " f_MeetingIdentity = ", num2, " AND  ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1))  AND f_SignRealTime > ", Strings.Format(now, "#yyyy-MM-dd HH:mm:ss#") });
                        this.arrMeetingNum[num2, 4] = view.Count;
                        if (this.arrMeetingNum[num2, 0] > 0L)
                        {
                            this.arrMeetingNum[num2, 5] = (this.arrMeetingNum[num2, 1] * 0x3e8L) / this.arrMeetingNum[num2, 0];
                        }
                    }
                    this.arrMeetingNum[6, 0] += this.arrMeetingNum[num2, 0];
                    this.arrMeetingNum[6, 1] += this.arrMeetingNum[num2, 1];
                    this.arrMeetingNum[6, 2] += this.arrMeetingNum[num2, 2];
                    this.arrMeetingNum[6, 3] += this.arrMeetingNum[num2, 3];
                    this.arrMeetingNum[6, 4] += this.arrMeetingNum[num2, 4];
                    this.arrMeetingNum[6, 5] += this.arrMeetingNum[num2, 5];
                }
                if (this.arrMeetingNum[6, 0] > 0L)
                {
                    this.arrMeetingNum[6, 5] = (this.arrMeetingNum[6, 1] * 0x3e8L) / this.arrMeetingNum[6, 0];
                }
                this.dvShould = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                this.dvInFact = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                this.dvLeave = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                this.dvAbsent = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                this.dvLate = new DataView(this.ds.Tables["t_d_MeetingConsumer"]);
                this.dvShould.RowFilter = "";
                this.dvInFact.RowFilter = "( f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1) ";
                this.dvLeave.RowFilter = " f_SignWay = 2 ";
                this.dvAbsent.RowFilter = " f_SignWay =0 AND f_RecID <=0  ";
                this.dvLate.RowFilter = " ((f_SignWay =0 AND f_RecID >0 ) OR (f_SignWay = 1)) AND f_SignRealTime > " + Strings.Format(now, "#yyyy-MM-dd HH:mm:ss#");
                DataTable table = new DataTable("Stat");
                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingIdentity";
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingShould";
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingInFact";
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingLeave";
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingAbsent";
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingLate";
                table.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "strMeetingRatio";
                table.Columns.Add(column);
                for (num2 = 0; num2 <= 6; num2++)
                {
                    DataRow row = table.NewRow();
                    if (num2 == 6)
                    {
                        row[0] = CommonStr.strMeetingSubTotal;
                    }
                    else
                    {
                        row[0] = frmMeetings.getStrMeetingIdentity((long) num2);
                    }
                    num = 0;
                    while (num <= 4)
                    {
                        row[num + 1] = this.arrMeetingNum[num2, num];
                        if (row[num + 1].ToString() == "0")
                        {
                            row[num + 1] = "";
                        }
                        num++;
                    }
                    row[6] = (this.arrMeetingNum[num2, 5] / 10L) + "%";
                    if (string.IsNullOrEmpty(row[1].ToString()))
                    {
                        row[6] = "";
                    }
                    table.Rows.Add(row);
                }
                DataTable table2 = this.ds.Tables["t_d_MeetingConsumer"];
                this.dgvMain.AutoGenerateColumns = false;
                for (num = 0; (num < table2.Columns.Count) && (num < this.dgvMain.ColumnCount); num++)
                {
                    this.dgvMain.Columns[num].DataPropertyName = table2.Columns[num].ColumnName;
                }
                this.dgvMain.DataSource = this.dvShould;
                this.dgvMain.DefaultCellStyle.ForeColor = Color.Black;
                wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_SignRealTime", wgTools.DisplayFormat_DateYMDHMSWeek);
                this.dgvStat.AutoGenerateColumns = false;
                for (num = 0; (num < table.Columns.Count) && (num < this.dgvStat.ColumnCount); num++)
                {
                    this.dgvStat.Columns[num].DataPropertyName = table.Columns[num].ColumnName;
                }
                this.dgvStat.DataSource = table;
                this.dgvStat.DefaultCellStyle.ForeColor = Color.Black;
                this.tabControl1.SelectedIndex = 5;
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
                    string str3 = " SELECT t_d_SwipeRecord.f_RecID,   ";
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

        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    this.dgvMain.DataSource = this.dvShould;
                    break;

                case 1:
                    this.dgvMain.DataSource = this.dvInFact;
                    break;

                case 2:
                    this.dgvMain.DataSource = this.dvLeave;
                    break;

                case 3:
                    this.dgvMain.DataSource = this.dvAbsent;
                    break;

                case 4:
                    this.dgvMain.DataSource = this.dvLate;
                    break;
            }
            if ((this.tabControl1.SelectedIndex >= 0) && (this.tabControl1.SelectedIndex <= 4))
            {
                this.dgvStat.Visible = false;
                this.dgvMain.Visible = true;
            }
            else
            {
                this.dgvStat.Visible = true;
                this.dgvMain.Visible = false;
            }
            this.dgvMain.Dock = DockStyle.Fill;
            this.dgvStat.Dock = DockStyle.Fill;
        }
    }
}

