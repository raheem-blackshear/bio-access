namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmLocate : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private bool bEdit = false;
        private bool bStarting = true;
        private icController controller4Locate = new icController();
        private dfrmFind dfrmFind1;
        private DataSet ds = new DataSet("ReaderAndCardRecordtable");
        private DataView dv;
        private DataView dvSelected;
        private string m_strGroupName;
        private string m_strUsers;
        private string strGroupFilter = "";
        private string strInOutInfo;
        private string strUserId = "000";

        public dfrmLocate()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            e.Result = this.loadUserData4BackWork();
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
            }
            else if (e.Error != null)
            {
                wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
            }
            else
            {
                this.loadUserData4BackWorkComplete(e.Result as DataTable);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            this.strInOutInfo = "";
            try
            {
                int num;
                string str2;
                int index = 0;
                DataRow[] rowArray = new DataRow[] { this.ds.Tables["cardrecord"].NewRow(), this.ds.Tables["cardrecord"].NewRow() };
                string cmdText = " SELECT * FROM t_d_SwipeRecord WHERE f_Character >0 AND f_ConsumerID = " + this.strUserId + " ORDER BY f_ReadDate DESC ";
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            connection.Open();
                            command.CommandTimeout = 180;
                            OleDbDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (((DateTime) reader["f_ReadDate"]) <= DateTime.Now.AddDays(2.0))
                                {
                                    num = 0;
                                    while (num <= (reader.FieldCount - 1))
                                    {
                                        rowArray[index][num] = reader[num];
                                        num++;
                                    }
                                    index++;
                                    if (index >= 2)
                                    {
                                        break;
                                    }
                                }
                            }
                            reader.Close();
                        }
                        goto Label_01FE;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        command2.CommandTimeout = 180;
                        SqlDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            if (((DateTime) reader2["f_ReadDate"]) <= DateTime.Now.AddDays(2.0))
                            {
                                for (num = 0; num <= (reader2.FieldCount - 1); num++)
                                {
                                    rowArray[index][num] = reader2[num];
                                }
                                index++;
                                if (index >= 2)
                                {
                                    break;
                                }
                            }
                        }
                        reader2.Close();
                    }
                }
            Label_01FE:
                str2 = "";
                str2 = this.m_strUsers + "\r\n";
                if (index > 0)
                {
                    TimeSpan span;
                    if ((((index == 2) && (((int) rowArray[0]["f_ControllerSN"]) == ((int) rowArray[1]["f_ControllerSN"]))) && ((((DateTime) rowArray[0]["f_ReadDate"]) > ((DateTime) rowArray[1]["f_ReadDate"])) && (rowArray[0]["f_InOut"].ToString() == "0"))) && (rowArray[1]["f_InOut"].ToString() == "1"))
                    {
                        this.controller4Locate.GetInfoFromDBByControllerSN((int) rowArray[1]["f_ControllerSN"]);
                        str2 = (str2 + ((DateTime) rowArray[1]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + "   {0}  " + this.controller4Locate.GetDoorNameByReaderNO((byte) rowArray[1]["f_ReaderNO"])) + "\r\n";
                        this.controller4Locate.GetInfoFromDBByControllerSN((int) rowArray[0]["f_ControllerSN"]);
                        str2 = (str2 + ((DateTime) rowArray[0]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + "   {1}  " + this.controller4Locate.GetDoorNameByReaderNO((byte) rowArray[0]["f_ReaderNO"])) + "\r\n" + "{2}:  ";
                        span = ((DateTime) rowArray[0]["f_ReadDate"]).Subtract((DateTime) rowArray[1]["f_ReadDate"]);
                        if (span.TotalDays >= 1.0)
                        {
                            str2 = str2 + ((int) span.TotalDays) + " {9}, ";
                        }
                        if (span.Hours > 0)
                        {
                            str2 = str2 + span.Hours + " {3}, ";
                        }
                        if (span.Minutes > 0)
                        {
                            str2 = str2 + span.Minutes + " {4} ";
                        }
                    }
                    else
                    {
                        this.controller4Locate.GetInfoFromDBByControllerSN((int) rowArray[0]["f_ControllerSN"]);
                        if (rowArray[0]["f_InOut"].ToString() == "0")
                        {
                            str2 = ((str2 + "{5}" + "\r\n") + ((DateTime) rowArray[0]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + "   {1}    " + this.controller4Locate.GetDoorNameByReaderNO((byte) rowArray[0]["f_ReaderNO"])) + "\r\n" + "{2}:  ";
                        }
                        else
                        {
                            str2 = ((str2 + ((DateTime) rowArray[0]["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + "   {0}    " + this.controller4Locate.GetDoorNameByReaderNO((byte) rowArray[0]["f_ReaderNO"])) + "\r\n" + "{6}") + "\r\n" + "{2}:  ";
                            span = DateTime.Now.Subtract((DateTime) rowArray[0]["f_ReadDate"]);
                            if (span.TotalSeconds < 0.0)
                            {
                                str2 = str2 + "[{7}] ";
                            }
                            else
                            {
                                if (span.TotalDays >= 1.0)
                                {
                                    str2 = str2 + ((int) span.TotalDays) + " {9}, ";
                                }
                                if (span.Hours > 0)
                                {
                                    str2 = str2 + span.Hours + " {3}, ";
                                }
                                if (span.Minutes > 0)
                                {
                                    str2 = str2 + span.Minutes + " {4} ";
                                }
                            }
                        }
                    }
                }
                else
                {
                    str2 = str2 + "{8}";
                }
                this.strInOutInfo = str2;
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.txtLocate.Text = string.Format(this.strInOutInfo, new object[] { CommonStr.strEnterInto, CommonStr.strGoOff, CommonStr.strStay, CommonStr.strHour, CommonStr.strMinutes, CommonStr.strEnterWithoutSwiping, CommonStr.strDontGoOff, CommonStr.strLaterThanNow, CommonStr.strNoSwiping, CommonStr.strDay });
            this.timer1.Enabled = false;
            Cursor.Current = Cursors.Default;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            int rowIndex;
            DataGridView dgvUsers = this.dgvUsers;
            if (dgvUsers.SelectedRows.Count <= 0)
            {
                if (dgvUsers.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = dgvUsers.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = dgvUsers.SelectedRows[0].Index;
            }
            DataTable table = ((DataView) dgvUsers.DataSource).Table;
            int key = (int) dgvUsers.Rows[rowIndex].Cells[0].Value;
            DataRow row = table.Rows.Find(key);
            if (row != null)
            {
                this.strUserId = row["f_ConsumerID"].ToString();
                this.m_strGroupName = this.cbof_GroupID.Text;
                if (this.m_strGroupName == CommonStr.strAll)
                {
                    this.m_strGroupName = "";
                }
                this.m_strUsers = row["f_ConsumerName"].ToString();
                if (!this.backgroundWorker2.IsBusy)
                {
                    this.backgroundWorker2.RunWorkerAsync();
                    this.timer1.Enabled = true;
                }
            }
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    dataSource.RowFilter = icConsumerShare.getOptionalRowfilter();
                    this.strGroupFilter = "";
                }
                else
                {
                    dataSource.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
                    num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num2);
                            this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num2);
                        }
                        else
                        {
                            dataSource.RowFilter = "f_Selected = 0 ";
                            string str = "";
                            for (int i = 0; i < this.arrGroupNO.Count; i++)
                            {
                                if ((((int) this.arrGroupNO[i]) <= num3) && (((int) this.arrGroupNO[i]) >= num))
                                {
                                    if (str == "")
                                    {
                                        str = str + string.Format(" f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                    }
                                    else
                                    {
                                        str = str + string.Format(" OR f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                    }
                                }
                            }
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", str);
                            this.strGroupFilter = string.Format("  {0} ", str);
                        }
                    }
                    dataSource.RowFilter = string.Format("(f_DoorEnabled > 0) AND {0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                if (string.IsNullOrEmpty(this.strGroupFilter))
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
            }
        }

        private void dfrmLocate_KeyDown(object sender, KeyEventArgs e)
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
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void dfrmPrivilegeCopy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmPrivilegeCopy_Load(object sender, EventArgs e)
        {
            try
            {
                new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
                for (int i = 0; i < this.arrGroupID.Count; i++)
                {
                    if ((i == 0) && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
                    {
                        this.cbof_GroupID.Items.Add(CommonStr.strAll);
                    }
                    else
                    {
                        this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
                    }
                }
                if (this.cbof_GroupID.Items.Count > 0)
                {
                    this.cbof_GroupID.SelectedIndex = 0;
                }
                this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
        {
            this.dfrmLocate_KeyDown(this.dgvUsers, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.controller4Locate != null))
            {
                this.controller4Locate.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private DataTable loadUserData4BackWork()
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadUserData Start");
            icConsumerShare.loadUserData();
            this.ds = new DataSet("ReaderAndCardRecordtable");
            string cmdText = " SELECT * FROM t_d_SwipeRecord WHERE 1<0 ";
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.ds, "cardrecord");
                        }
                    }
                    goto Label_00E2;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.ds, "cardrecord");
                    }
                }
            }
        Label_00E2:
            return icConsumerShare.getDt();
        }

        private void loadUserData4BackWorkComplete(DataTable dtUser)
        {
            this.dv = new DataView(dtUser);
            this.dvSelected = new DataView(dtUser);
            this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
            this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dgvUsers.ColumnCount); i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
            }
            this.cbof_GroupID_SelectedIndexChanged(null, null);
            wgTools.WriteLine("loadUserData End");
            Cursor.Current = Cursors.Default;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.bStarting)
                {
                    if (this.backgroundWorker2.IsBusy)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                    }
                    else if ((this.progressBar1.Value != 0) && (this.progressBar1.Value != this.progressBar1.Maximum))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }
                else if (this.dgvUsers.DataSource == null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
                else
                {
                    this.timer1.Enabled = false;
                    Cursor.Current = Cursors.Default;
                    this.lblWait.Visible = false;
                    this.btnQuery.Enabled = true;
                    this.cbof_GroupID.Enabled = true;
                    this.bStarting = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void txtLocate_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (this.bEdit)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }
    }
}

