namespace WG3000_COMM.Reports.Shift
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

    public partial class frmShiftAttReport : frmBioAccess
    {
        private ArrayList arrColsName = new ArrayList();
        private ArrayList arrColsShow = new ArrayList();
        private bool bLoadedFinished;
        private bool bLogCreateReport;
        private dfrmShiftAttReportFindOption dfrmFindOption;
        private string dgvSql = "";
        private DataSet dsDefaultStyle = new DataSet("DGV_STILE");
        private int MaxRecord = 0x3e8;
        private int recIdMax;
        private int startRecordIndex;
        private DataTable table;
        private UserControlFind userControlFind1;

        public frmShiftAttReport()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            int startIndex = (int) ((object[]) e.Argument)[0];
            int maxRecords = (int) ((object[]) e.Argument)[1];
            string strSql = (string) ((object[]) e.Argument)[2];
            e.Result = this.loadDataRecords(startIndex, maxRecords, strSql);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                XMessageBox.Show(CommonStr.strOperationCanceled);
            }
            else if (e.Error != null)
            {
                XMessageBox.Show(string.Format("An error occurred: {0}", e.Error.Message));
            }
            else
            {
                if ((e.Result as DataTable).Rows.Count < this.MaxRecord)
                {
                    this.bLoadedFinished = true;
                }
                this.fillDgv(e.Result as DataTable);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvMain.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
            }
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            if (this.dtpDateFrom.Value > this.dtpDateTo.Value)
            {
                XMessageBox.Show(this, CommonStr.strWrongPeriod, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string str = string.Format("{0}\r\n{1} {2} {3} {4}", new object[] { this.btnCreateReport.Text, this.toolStripLabel2.Text, this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD), this.toolStripLabel3.Text, this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD) });
            if (XMessageBox.Show(string.Format(CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string oldValue = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled ";
            string str3 = oldValue;
            str3 = str3 + " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) ";
            if (findConsumerID > 0)
            {
                str3 = oldValue;
                str3 = str3 + " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
            }
            else if (groupMinNO > 0)
            {
                str3 = oldValue;
                if (groupMinNO >= groupMaxNO)
                {
                    str3 = str3 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO);
                }
                else
                {
                    str3 = (str3 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ") + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO);
                }
                if (findName != "")
                {
                    str3 = str3 + string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                else if (findCard > 0L)
                {
                    str3 = str3 + string.Format(" AND f_CardNO ={0:d} ", findCard);
                }
                str3 = str3 + " AND (f_AttendEnabled >0 ) ";
            }
            else if (findName != "")
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName))) + " AND (f_AttendEnabled >0 ) ";
            }
            else if (findCard > 0L)
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_CardNO ={0:d} ", findCard) + " AND (f_AttendEnabled >0 ) ";
            }
            bool flag = false;
            int num6 = 0;
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str3.Replace(oldValue, " SELECT  COUNT(*) "), connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            flag = true;
                            num6 = Convert.ToInt32(reader[0]);
                        }
                        reader.Close();
                    }
                    goto Label_03A6;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str3.Replace(oldValue, " SELECT  COUNT(*) "), connection2))
                {
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.Read())
                    {
                        flag = true;
                        num6 = Convert.ToInt32(reader2[0]);
                    }
                    reader2.Close();
                }
            }
        Label_03A6:
            if (flag)
            {
                if (base.OwnedForms.Length > 0)
                {
                    foreach (Form form in base.OwnedForms)
                    {
                        if (form.Name == "dfrmShiftAttReportCreate")
                        {
                            return;
                        }
                    }
                }
                using (dfrmShiftAttReportCreate create = new dfrmShiftAttReportCreate())
                {
                    create.totalConsumer = num6;
                    create.dtBegin = this.dtpDateFrom.Value;
                    create.dtEnd = this.dtpDateTo.Value;
                    create.strConsumerSql = str3;
                    create.groupName = this.userControlFind1.cboFindDept.Text;
                    create.TopMost = true;
                    create.ShowDialog(this);
                    this.btnQuery_Click(null, null);
                }
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
        }

        private void btnFindOption_Click(object sender, EventArgs e)
        {
            if (this.dfrmFindOption == null)
            {
                this.dfrmFindOption = new dfrmShiftAttReportFindOption();
                this.dfrmFindOption.Owner = this;
            }
            this.dfrmFindOption.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvMain, this.Text);
        }

        public void btnQuery_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnQuery_Click_Acc(sender, e);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                this.getLogCreateReport();
                if (!this.bLogCreateReport)
                {
                    XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    int groupMinNO = 0;
                    int groupIDOfMinNO = 0;
                    int groupMaxNO = 0;
                    string findName = "";
                    long findCard = 0L;
                    int findConsumerID = 0;
                    string str2 = "";
                    bool flag = false;
                    if ((this.dfrmFindOption != null) && this.dfrmFindOption.Visible)
                    {
                        flag = true;
                        str2 = this.dfrmFindOption.getStrSql();
                    }
                    this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                    string strsql = wgAppConfig.getSqlFindNormal(((((((((((((" SELECT t_d_shift_AttReport.f_RecID, t_b_Group.f_GroupName, " + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, ") + " t_d_shift_AttReport.f_ShiftDate AS f_ShiftDateShort, " + " t_d_shift_AttReport.f_ShiftID, ") + " t_d_shift_AttReport.f_ReadTimes, " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty1,108) , '') AS f_OnDuty1Short,  ") + "       ISNULL(t_d_shift_AttReport.f_OnDuty1AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty1CardRecordDesc, '') AS f_Desc1,  " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty1,108) , '') AS f_OffDuty1Short,            ") + "       ISNULL(t_d_shift_AttReport.f_OffDuty1AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OffDuty1CardRecordDesc, '') AS f_Desc2,           " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty2,108) , '') AS f_OnDuty2Short,        ") + "       ISNULL(t_d_shift_AttReport.f_OnDuty2AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty2CardRecordDesc, '') AS f_Desc3,            " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty2,108) , '') AS f_OffDuty2Short, ") + "       ISNULL(t_d_shift_AttReport.f_OffDuty2AttDesc, '')+ ISNULL(t_d_shift_AttReport.f_OffDuty2CardRecordDesc, '') AS f_Desc4,           " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty3,108) , '') AS f_OnDuty3Short,              ") + "       ISNULL(t_d_shift_AttReport.f_OnDuty3AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty3CardRecordDesc, '') AS f_Desc5,            " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty3,108) , '') AS f_OffDuty3Short,       ") + "       ISNULL(t_d_shift_AttReport.f_OffDuty3AttDesc, '')+ ISNULL(t_d_shift_AttReport.f_OffDuty3CardRecordDesc, '') AS f_Desc6,           " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OnDuty4,108) , '') AS f_OnDuty4Short,   ") + "       ISNULL(t_d_shift_AttReport.f_OnDuty4AttDesc, '') + ISNULL(t_d_shift_AttReport.f_OnDuty4CardRecordDesc, '') AS f_Desc7,            " + "       ISNULL(CONVERT(char(8), t_d_shift_AttReport.f_OffDuty4,108) , '') AS f_OffDuty4Short,  ") + "       ISNULL(t_d_shift_AttReport.f_OffDuty4AttDesc, '')+ ISNULL(t_d_shift_AttReport.f_OffDuty4CardRecordDesc, '') AS f_Desc8, " + "CASE WHEN [f_LateMinutes]>0 THEN (CASE WHEN [f_LateMinutes]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_LateMinutes]) END ) ELSE ' ' END [f_LateMinutes], ") + "CASE WHEN [f_LeaveEarlyMinutes]>0 THEN (CASE WHEN [f_LeaveEarlyMinutes]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_LeaveEarlyMinutes]) END ) ELSE ' ' END [f_LeaveEarlyMinutes], " + "CASE WHEN [f_OvertimeHours]>0 THEN (CASE WHEN [f_OvertimeHours]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_OvertimeHours]) END ) ELSE ' ' END [f_OvertimeHours], ") + "CASE WHEN [f_AbsenceDays]>0 THEN (CASE WHEN [f_AbsenceDays]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_AbsenceDays]) END ) ELSE ' ' END [f_AbsenceDays], " + "CASE WHEN [f_NotReadCardCount]>0 THEN (CASE WHEN [f_NotReadCardCount]<1 THEN '0.5' ELSE CONVERT(varchar(6),[f_NotReadCardCount]) END ) ELSE ' ' END [f_NotReadCardCount] ", "t_d_shift_AttReport", this.getSqlOfDateTime("t_d_shift_AttReport.f_ShiftDate"), groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                    if (flag)
                    {
                        flag = false;
                        strsql = strsql + " WHERE " + str2;
                    }
                    string str5 = "";
                    if (sender == this.cmdQueryNormalShift)
                    {
                        str5 = " AND t_d_shift_AttReport.f_ShiftID IS NULL ";
                    }
                    else if (sender == this.cmdQueryOtherShift)
                    {
                        str5 = " AND t_d_shift_AttReport.f_ShiftID IS NOT NULL ";
                    }
                    if (!string.IsNullOrEmpty(str5))
                    {
                        if (strsql.IndexOf(" WHERE ") > 0)
                        {
                            strsql = strsql + str5;
                        }
                        else
                        {
                            strsql = strsql + " WHERE (1>0) " + str5;
                        }
                    }
                    this.reloadData(strsql);
                }
            }
        }

        public void btnQuery_Click_Acc(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.getLogCreateReport();
            if (!this.bLogCreateReport)
            {
                XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                string str2 = "";
                bool flag = false;
                if ((this.dfrmFindOption != null) && this.dfrmFindOption.Visible)
                {
                    flag = true;
                    str2 = this.dfrmFindOption.getStrSql();
                }
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string strsql = wgAppConfig.getSqlFindNormal(((((((((((((" SELECT t_d_shift_AttReport.f_RecID, t_b_Group.f_GroupName, " + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, ") + " t_d_shift_AttReport.f_ShiftDate AS f_ShiftDateShort, " + " t_d_shift_AttReport.f_ShiftID, ") + " t_d_shift_AttReport.f_ReadTimes, " + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty1", "f_OnDuty1Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty1AttDesc", "f_OnDuty1CardRecordDesc", "f_Desc1") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty1", "f_OffDuty1Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty1AttDesc", "f_OffDuty1CardRecordDesc", "f_Desc2") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty2", "f_OnDuty2Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty2AttDesc", "f_OnDuty2CardRecordDesc", "f_Desc3") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty2", "f_OffDuty2Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty2AttDesc", "f_OffDuty2CardRecordDesc", "f_Desc4") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty3", "f_OnDuty3Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty3AttDesc", "f_OnDuty3CardRecordDesc", "f_Desc5") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty3", "f_OffDuty3Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty3AttDesc", "f_OffDuty3CardRecordDesc", "f_Desc6") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OnDuty4", "f_OnDuty4Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OnDuty4AttDesc", "f_OnDuty4CardRecordDesc", "f_Desc7") + string.Format("       IIF(IsDate(t_d_shift_AttReport.{0}),Format(t_d_shift_AttReport.{0},'hh:nn:ss'),'')  AS {1},  ", "f_OffDuty4", "f_OffDuty4Short")) + string.Format("       IIF(ISNULL(t_d_shift_AttReport.{0}), '',t_d_shift_AttReport.{0}) + IIF(ISNULL(t_d_shift_AttReport.{1}), '',t_d_shift_AttReport.{1}) AS {2},  ", "f_OffDuty4AttDesc", "f_OffDuty4CardRecordDesc", "f_Desc8") + "IIF ([f_LateMinutes]>0, IIF ([f_LateMinutes]<1, '0.5', CSTR([f_LateMinutes])), ' ') AS [f_LateMinutes], ") + "IIF ([f_LeaveEarlyMinutes]>0 ,  IIF ([f_LeaveEarlyMinutes]<1 , '0.5', CSTR([f_LeaveEarlyMinutes])), ' ') AS [f_LeaveEarlyMinutes], " + "IIF ([f_OvertimeHours]>0,  IIF ([f_OvertimeHours]<1,  '0.5', CSTR([f_OvertimeHours])), ' ') AS [f_OvertimeHours], ") + "IIF ([f_AbsenceDays]>0,  IIF ([f_AbsenceDays]<1, '0.5', CSTR([f_AbsenceDays])), ' ') AS [f_AbsenceDays], " + "IIF ([f_NotReadCardCount]>0, IIF ([f_NotReadCardCount]<1, '0.5', CSTR([f_NotReadCardCount])), ' ') AS [f_NotReadCardCount] ", "t_d_shift_AttReport", this.getSqlOfDateTime("t_d_shift_AttReport.f_ShiftDate"), groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                if (flag)
                {
                    flag = false;
                    strsql = strsql + " WHERE " + str2;
                }
                string str5 = "";
                if (sender == this.cmdQueryNormalShift)
                {
                    str5 = " AND t_d_shift_AttReport.f_ShiftID IS NULL ";
                }
                else if (sender == this.cmdQueryOtherShift)
                {
                    str5 = " AND t_d_shift_AttReport.f_ShiftID IS NOT NULL ";
                }
                if (!string.IsNullOrEmpty(str5))
                {
                    if (strsql.IndexOf(" WHERE ") > 0)
                    {
                        strsql = strsql + str5;
                    }
                    else
                    {
                        strsql = strsql + " WHERE (1>0) " + str5;
                    }
                }
                this.reloadData(strsql);
            }
        }

        private void btnSelectColumns_Click(object sender, EventArgs e)
        {
            using (dfrmSelectColumnsShow show = new dfrmSelectColumnsShow())
            {
                for (int i = 1; i < this.arrColsName.Count; i++)
                {
                    show.chkListColumns.Items.Add(this.arrColsName[i]);
                    show.chkListColumns.SetItemChecked(i - 1, bool.Parse(this.arrColsShow[i].ToString()));
                }
                if (show.ShowDialog(this) == DialogResult.OK)
                {
                    this.arrColsShow.Clear();
                    this.arrColsShow.Add(this.dgvMain.Columns[0].Visible);
                    for (int j = 1; j < this.dgvMain.ColumnCount; j++)
                    {
                        this.dgvMain.Columns[j].Visible = show.chkListColumns.GetItemChecked(j - 1);
                        this.arrColsShow.Add(this.dgvMain.Columns[j].Visible);
                    }
                    this.saveColumns();
                }
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            using (frmShiftAttStatistics statistics = new frmShiftAttStatistics())
            {
                statistics.ShowDialog(this);
            }
        }

        private void cmdCreateWithSomeConsumer_Click(object sender, EventArgs e)
        {
            if (this.dtpDateFrom.Value > this.dtpDateTo.Value)
            {
                return;
            }
            string str = string.Format("{0}\r\n{1} {2} {3} {4}", new object[] { this.btnCreateReport.Text, this.toolStripLabel2.Text, this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD), this.toolStripLabel3.Text, this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD) });
            if (XMessageBox.Show(string.Format(CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string oldValue = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled ";
            string str3 = oldValue;
            str3 = str3 + " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) ";
            if (findConsumerID > 0)
            {
                str3 = oldValue;
                str3 = str3 + " FROM t_b_Consumer WHERE (f_AttendEnabled >0 ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
            }
            else if (groupMinNO > 0)
            {
                str3 = oldValue;
                if (groupMinNO >= groupMaxNO)
                {
                    str3 = str3 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO);
                }
                else
                {
                    str3 = (str3 + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ") + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO);
                }
                if (findName != "")
                {
                    str3 = str3 + string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                else if (findCard > 0L)
                {
                    str3 = str3 + string.Format(" AND f_CardNO ={0:d} ", findCard);
                }
                str3 = str3 + " AND (f_AttendEnabled >0 ) ";
            }
            else if (findName != "")
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName))) + " AND (f_AttendEnabled >0 ) ";
            }
            else if (findCard > 0L)
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_CardNO ={0:d} ", findCard) + " AND (f_AttendEnabled >0 ) ";
            }
            dfrmUserSelected selected = new dfrmUserSelected();
            if (selected.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            if (string.IsNullOrEmpty(selected.selectedUsers))
            {
                return;
            }
            str3 = oldValue;
            str3 = str3 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerID IN ({0}) AND {1} ", selected.selectedUsers, "   (f_AttendEnabled >0 ) ");
            bool flag = false;
            int num6 = 0;
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str3.Replace(oldValue, " SELECT  COUNT(*) "), connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            flag = true;
                            num6 = Convert.ToInt32(reader[0]);
                        }
                        reader.Close();
                    }
                    goto Label_03FB;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str3.Replace(oldValue, " SELECT  COUNT(*) "), connection2))
                {
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.Read())
                    {
                        flag = true;
                        num6 = Convert.ToInt32(reader2[0]);
                    }
                    reader2.Close();
                }
            }
        Label_03FB:
            if (flag)
            {
                if (base.OwnedForms.Length > 0)
                {
                    foreach (Form form in base.OwnedForms)
                    {
                        if (form.Name == "dfrmShiftAttReportCreate")
                        {
                            return;
                        }
                    }
                }
                using (dfrmShiftAttReportCreate create = new dfrmShiftAttReportCreate())
                {
                    create.totalConsumer = num6;
                    create.dtBegin = this.dtpDateFrom.Value;
                    create.dtEnd = this.dtpDateTo.Value;
                    create.strConsumerSql = str3;
                    create.groupName = this.userControlFind1.cboFindDept.Text;
                    create.TopMost = true;
                    create.ShowDialog(this);
                    this.btnQuery_Click(null, null);
                }
            }
        }

        private void dgvMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void dgvMain_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.bLoadedFinished && (e.ScrollOrientation == ScrollOrientation.VerticalScroll))
            {
                wgTools.WriteLine(e.OldValue.ToString());
                wgTools.WriteLine(e.NewValue.ToString());
                if ((e.NewValue > e.OldValue) && (((e.NewValue + 100) > this.dgvMain.Rows.Count) || ((e.NewValue + (this.dgvMain.Rows.Count / 10)) > this.dgvMain.Rows.Count)))
                {
                    if (this.startRecordIndex <= this.dgvMain.Rows.Count)
                    {
                        if (!this.backgroundWorker1.IsBusy)
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                        }
                    }
                    else
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvMain.Rows.Count.ToString() + "#");
                    }
                }
            }
        }

        private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.bLoadedFinished)
            {
                if (!this.bLogCreateReport)
                {
                    XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (this.startRecordIndex <= this.dgvMain.Rows.Count)
                    {
                        if (!this.backgroundWorker1.IsBusy)
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.bLoadedFinished = true;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 0x5f5e100, this.dgvSql });
                        }
                    }
                    else
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvMain.Rows.Count.ToString() + "#");
                    }
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

        private void fillDgv(DataTable dt)
        {
            try
            {
                if (this.dgvMain.DataSource == null)
                {
                    this.dgvMain.DataSource = dt;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        this.dgvMain.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
                        this.dgvMain.Columns[i].Name = dt.Columns[i].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_ShiftDateShort", wgTools.DisplayFormat_DateYMDWeek);
                    wgAppConfig.ReadGVStyle(this, this.dgvMain);
                    if ((this.startRecordIndex == 0) && (dt.Rows.Count >= this.MaxRecord))
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                    }
                }
                else if (dt.Rows.Count > 0)
                {
                    int firstDisplayedScrollingRowIndex = this.dgvMain.FirstDisplayedScrollingRowIndex;
                    (this.dgvMain.DataSource as DataTable).Merge(dt);
                    if (firstDisplayedScrollingRowIndex >= 0)
                    {
                        this.dgvMain.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmShiftAttReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFindOption != null)
            {
                this.dfrmFindOption.Close();
            }
        }

        private void frmSwipeRecords_Load(object sender, EventArgs e)
        {
            this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRomm(this.f_DepartmentName.HeaderText);
            this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
            this.loadOperatorPrivilege();
            this.dtpDateFrom = new ToolStripDateTime();
            this.dtpDateTo = new ToolStripDateTime();
            this.toolStrip3.Items.Clear();
            this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2, this.dtpDateFrom, this.toolStripLabel3, this.dtpDateTo });
            this.dtpDateFrom.BoxWidth = 120;
            this.dtpDateTo.BoxWidth = 120;
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.dtpDateFrom.Enabled = true;
            this.dtpDateTo.Enabled = true;
            this.userControlFind1.toolStripLabel2.Visible = false;
            this.userControlFind1.txtFindCardID.Visible = false;
            this.saveDefaultStyle();
            this.loadStyle();
            Cursor.Current = Cursors.WaitCursor;
            this.getLogCreateReport();
            if (this.bLogCreateReport)
            {
                this.dtpDateFrom.Value = this.logDateStart;
                this.dtpDateTo.Value = this.logDateEnd;
            }
            else
            {
                this.dtpDateTo.Value = DateTime.Now.Date;
                this.dtpDateFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            }
            this.dtpDateFrom.BoxWidth = 150;
            this.dtpDateTo.BoxWidth = 150;
            wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.cmdQueryNormalShift.Visible = true;
                this.cmdQueryOtherShift.Visible = true;
            }
            this.Refresh();
            this.userControlFind1.btnQuery.PerformClick();
        }

        public void getLogCreateReport()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.getLogCreateReport_Acc();
            }
            else
            {
                this.bLogCreateReport = false;
                string cmdText = "SELECT * FROM t_a_Attendence WHERE [f_NO]=15 ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read() && (wgTools.SetObjToStr(reader["f_Notes"]) != ""))
                        {
                            this.bLogCreateReport = true;
                            this.logDateStart = DateTime.Parse(wgTools.SetObjToStr(reader["f_Value"]).Substring(0, 10));
                            this.logDateEnd = DateTime.Parse(wgTools.SetObjToStr(reader["f_Value"]).Substring(12, 10));
                            this.lblLog.Text = reader["f_Notes"].ToString();
                        }
                        reader.Close();
                    }
                }
            }
        }

        public void getLogCreateReport_Acc()
        {
            this.bLogCreateReport = false;
            string cmdText = "SELECT * FROM t_a_Attendence WHERE [f_NO]=15 ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read() && (wgTools.SetObjToStr(reader["f_Notes"]) != ""))
                    {
                        this.bLogCreateReport = true;
                        this.logDateStart = DateTime.Parse(wgTools.SetObjToStr(reader["f_Value"]).Substring(0, 10));
                        this.logDateEnd = DateTime.Parse(wgTools.SetObjToStr(reader["f_Value"]).Substring(12, 10));
                        this.lblLog.Text = reader["f_Notes"].ToString();
                    }
                    reader.Close();
                }
            }
        }

        private string getSqlOfDateTime(string colNameOfDate)
        {
            string str = "";
            str = "  (" + colNameOfDate + " >= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
            if (str != "")
            {
                str = str + " AND ";
            }
            string str2 = str;
            return (str2 + "  (" + colNameOfDate + " <= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")");
        }

        private DataTable loadDataRecords(int startIndex, int maxRecords, string strSql)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine(this.Text + " loadDataRecords Start");
            if (strSql.ToUpper().IndexOf("SELECT ") > 0)
            {
                strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            }
            if (startIndex == 0)
            {
                this.recIdMax = -2147483648;
            }
            else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
            {
                strSql = strSql + string.Format(" AND t_d_shift_AttReport.f_RecID > {0:d}", this.recIdMax);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE t_d_shift_AttReport.f_RecID > {0:d}", this.recIdMax);
            }
            strSql = strSql + " ORDER BY t_d_shift_AttReport.f_RecID ";
            this.table = new DataTable();
            wgTools.WriteLine("da.Fill start");
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.table);
                        }
                    }
                    goto Label_0190;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(strSql, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.table);
                    }
                }
            }
        Label_0190:
            if (this.table.Rows.Count > 0)
            {
                this.recIdMax = int.Parse(this.table.Rows[this.table.Rows.Count - 1][0].ToString());
            }
            wgTools.WriteLine("da.Fill End " + startIndex.ToString());
            Cursor.Current = Cursors.Default;
            wgTools.WriteLine(this.Text + "  loadRecords End");
            return this.table;
        }

        private void loadDefaultStyle()
        {
            DataTable table = this.dsDefaultStyle.Tables[this.dgvMain.Name];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                this.dgvMain.Columns[i].Name = table.Rows[i]["colName"].ToString();
                this.dgvMain.Columns[i].HeaderText = table.Rows[i]["colHeader"].ToString();
                this.dgvMain.Columns[i].Width = int.Parse(table.Rows[i]["colWidth"].ToString());
                this.dgvMain.Columns[i].Visible = bool.Parse(table.Rows[i]["colVisable"].ToString());
                this.dgvMain.Columns[i].DisplayIndex = int.Parse(table.Rows[i]["colDisplayIndex"].ToString());
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuAttendenceData";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnCreateReport.Visible = false;
            }
        }

        private void loadStyle()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadStyle_Acc();
            }
            else
            {
                if (this.OnlyTwoTimesSpecial())
                {
                    this.dgvMain.Columns[0x19].HeaderText = CommonStr.strWorkHour;
                }
                if (!wgAppConfig.getParamValBoolByNO(0x71))
                {
                    int result = 2;
                    string cmdText = "SELECT f_Value FROM t_a_Attendence WHERE f_No =14";
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command = new SqlCommand(cmdText, connection))
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                int.TryParse(wgTools.SetObjToStr(reader[0]), out result);
                            }
                            reader.Close();
                        }
                    }
                    this.dgvMain.Columns[5].Visible = false;
                    this.dgvMain.Columns[6].Visible = false;
                    if (result == 4)
                    {
                        this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
                        this.dgvMain.Columns[8].HeaderText = CommonStr.strDutyDesc1;
                        this.dgvMain.Columns[9].HeaderText = CommonStr.strAMOffDuty;
                        this.dgvMain.Columns[10].HeaderText = CommonStr.strDutyDesc2;
                        this.dgvMain.Columns[11].HeaderText = CommonStr.strPMOnDuty;
                        this.dgvMain.Columns[12].HeaderText = CommonStr.strDutyDesc3;
                        this.dgvMain.Columns[13].HeaderText = CommonStr.strPMOffDuty;
                        this.dgvMain.Columns[14].HeaderText = CommonStr.strDutyDesc4;
                        for (int k = 15; k < 0x17; k++)
                        {
                            this.dgvMain.Columns[k].Visible = false;
                        }
                        if (this.OnlyOnDutySpecial())
                        {
                            this.dgvMain.Columns[9].Visible = false;
                            this.dgvMain.Columns[10].Visible = false;
                            this.dgvMain.Columns[13].Visible = false;
                            this.dgvMain.Columns[14].Visible = false;
                            this.dgvMain.Columns[0x18].Visible = false;
                            this.dgvMain.Columns[0x19].Visible = false;
                            this.dgvMain.Columns[0x1b].Visible = false;
                        }
                    }
                    else
                    {
                        this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
                        this.dgvMain.Columns[8].HeaderText = CommonStr.strOnDutyDesc;
                        this.dgvMain.Columns[9].HeaderText = CommonStr.strPMOffDuty;
                        this.dgvMain.Columns[10].HeaderText = CommonStr.strOffDutyDesc;
                        for (int m = 11; m < 0x17; m++)
                        {
                            this.dgvMain.Columns[m].Visible = false;
                        }
                        if (this.OnlyOnDutySpecial())
                        {
                            this.dgvMain.Columns[9].Visible = false;
                            this.dgvMain.Columns[10].Visible = false;
                            this.dgvMain.Columns[0x18].Visible = false;
                            this.dgvMain.Columns[0x19].Visible = false;
                            this.dgvMain.Columns[0x1b].Visible = false;
                        }
                    }
                }
                this.dgvMain.AutoGenerateColumns = false;
                this.arrColsName.Clear();
                this.arrColsShow.Clear();
                for (int i = 0; i < this.dgvMain.ColumnCount; i++)
                {
                    this.arrColsName.Add(this.dgvMain.Columns[i].HeaderText);
                    this.arrColsShow.Add(this.dgvMain.Columns[i].Visible);
                }
                string str2 = "";
                string str3 = "";
                for (int j = 0; j < this.arrColsName.Count; j++)
                {
                    if (str2 != "")
                    {
                        str2 = str2 + ",";
                        str3 = str3 + ",";
                    }
                    str2 = str2 + this.arrColsName[j];
                    str3 = str3 + this.arrColsShow[j].ToString();
                }
                string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
                if (keyVal != "")
                {
                    string[] strArray = keyVal.Split(new char[] { ';' });
                    if (((strArray.Length == 2) && (str2 == strArray[0])) && (str3 != strArray[1]))
                    {
                        string[] strArray2 = strArray[1].Split(new char[] { ',' });
                        if (strArray2.Length == this.arrColsName.Count)
                        {
                            this.arrColsShow.Clear();
                            for (int n = 0; n < this.dgvMain.ColumnCount; n++)
                            {
                                this.dgvMain.Columns[n].Visible = bool.Parse(strArray2[n]);
                                this.arrColsShow.Add(this.dgvMain.Columns[n].Visible);
                            }
                        }
                    }
                }
                wgAppConfig.ReadGVStyle(this, this.dgvMain);
            }
        }

        private void loadStyle_Acc()
        {
            if (this.OnlyTwoTimesSpecial())
            {
                this.dgvMain.Columns[0x19].HeaderText = CommonStr.strWorkHour;
            }
            if (!wgAppConfig.getParamValBoolByNO(0x71))
            {
                int result = 2;
                string cmdText = "SELECT f_Value FROM t_a_Attendence WHERE f_No =14";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            int.TryParse(wgTools.SetObjToStr(reader[0]), out result);
                        }
                        reader.Close();
                    }
                }
                this.dgvMain.Columns[5].Visible = false;
                this.dgvMain.Columns[6].Visible = false;
                if (result == 4)
                {
                    this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
                    this.dgvMain.Columns[8].HeaderText = CommonStr.strDutyDesc1;
                    this.dgvMain.Columns[9].HeaderText = CommonStr.strAMOffDuty;
                    this.dgvMain.Columns[10].HeaderText = CommonStr.strDutyDesc2;
                    this.dgvMain.Columns[11].HeaderText = CommonStr.strPMOnDuty;
                    this.dgvMain.Columns[12].HeaderText = CommonStr.strDutyDesc3;
                    this.dgvMain.Columns[13].HeaderText = CommonStr.strPMOffDuty;
                    this.dgvMain.Columns[14].HeaderText = CommonStr.strDutyDesc4;
                    for (int k = 15; k < 0x17; k++)
                    {
                        this.dgvMain.Columns[k].Visible = false;
                    }
                    if (this.OnlyOnDutySpecial())
                    {
                        this.dgvMain.Columns[9].Visible = false;
                        this.dgvMain.Columns[10].Visible = false;
                        this.dgvMain.Columns[13].Visible = false;
                        this.dgvMain.Columns[14].Visible = false;
                        this.dgvMain.Columns[0x18].Visible = false;
                        this.dgvMain.Columns[0x19].Visible = false;
                        this.dgvMain.Columns[0x1b].Visible = false;
                    }
                }
                else
                {
                    this.dgvMain.Columns[7].HeaderText = CommonStr.strAMOnDuty;
                    this.dgvMain.Columns[8].HeaderText = CommonStr.strOnDutyDesc;
                    this.dgvMain.Columns[9].HeaderText = CommonStr.strPMOffDuty;
                    this.dgvMain.Columns[10].HeaderText = CommonStr.strOffDutyDesc;
                    for (int m = 11; m < 0x17; m++)
                    {
                        this.dgvMain.Columns[m].Visible = false;
                    }
                    if (this.OnlyOnDutySpecial())
                    {
                        this.dgvMain.Columns[9].Visible = false;
                        this.dgvMain.Columns[10].Visible = false;
                        this.dgvMain.Columns[0x18].Visible = false;
                        this.dgvMain.Columns[0x19].Visible = false;
                        this.dgvMain.Columns[0x1b].Visible = false;
                    }
                }
            }
            this.dgvMain.AutoGenerateColumns = false;
            this.arrColsName.Clear();
            this.arrColsShow.Clear();
            for (int i = 0; i < this.dgvMain.ColumnCount; i++)
            {
                this.arrColsName.Add(this.dgvMain.Columns[i].HeaderText);
                this.arrColsShow.Add(this.dgvMain.Columns[i].Visible);
            }
            string str2 = "";
            string str3 = "";
            for (int j = 0; j < this.arrColsName.Count; j++)
            {
                if (str2 != "")
                {
                    str2 = str2 + ",";
                    str3 = str3 + ",";
                }
                str2 = str2 + this.arrColsName[j];
                str3 = str3 + this.arrColsShow[j].ToString();
            }
            string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
            if (keyVal != "")
            {
                string[] strArray = keyVal.Split(new char[] { ';' });
                if (((strArray.Length == 2) && (str2 == strArray[0])) && (str3 != strArray[1]))
                {
                    string[] strArray2 = strArray[1].Split(new char[] { ',' });
                    if (strArray2.Length == this.arrColsName.Count)
                    {
                        this.arrColsShow.Clear();
                        for (int n = 0; n < this.dgvMain.ColumnCount; n++)
                        {
                            this.dgvMain.Columns[n].Visible = bool.Parse(strArray2[n]);
                            this.arrColsShow.Add(this.dgvMain.Columns[n].Visible);
                        }
                    }
                }
            }
            wgAppConfig.ReadGVStyle(this, this.dgvMain);
        }

        private bool OnlyOnDutySpecial()
        {
            bool flag2;
            if (wgAppConfig.IsAccessDB)
            {
                return this.OnlyOnDutySpecial_Acc();
            }
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                return false;
            }
            string cmdText = "SELECT * FROM t_a_Attendence";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (((int) reader["f_No"]) == 14)
                        {
                            Convert.ToInt32(reader["f_Value"]);
                        }
                    }
                    reader.Close();
                    flag2 = wgAppConfig.getSystemParamByNO(0x3b).ToString() == "1";
                }
            }
            return flag2;
        }

        private bool OnlyOnDutySpecial_Acc()
        {
            bool flag2;
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                return false;
            }
            string cmdText = "SELECT * FROM t_a_Attendence";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (((int) reader["f_No"]) == 14)
                        {
                            Convert.ToInt32(reader["f_Value"]);
                        }
                    }
                    reader.Close();
                    flag2 = wgAppConfig.getSystemParamByNO(0x3b).ToString() == "1";
                }
            }
            return flag2;
        }

        private bool OnlyTwoTimesSpecial()
        {
            bool flag2;
            if (wgAppConfig.IsAccessDB)
            {
                return this.OnlyTwoTimesSpecial_Acc();
            }
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                return false;
            }
            string cmdText = "SELECT * FROM t_a_Attendence";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    int num = 2;
                    while (reader.Read())
                    {
                        if (((int) reader["f_No"]) == 14)
                        {
                            num = Convert.ToInt32(reader["f_Value"]);
                        }
                    }
                    reader.Close();
                    if (num == 4)
                    {
                        return false;
                    }
                    flag2 = wgAppConfig.getSystemParamByNO(0x39).ToString() == "1";
                }
            }
            return flag2;
        }

        private bool OnlyTwoTimesSpecial_Acc()
        {
            bool flag2;
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                return false;
            }
            string cmdText = "SELECT * FROM t_a_Attendence";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    int num = 2;
                    while (reader.Read())
                    {
                        if (((int) reader["f_No"]) == 14)
                        {
                            num = Convert.ToInt32(reader["f_Value"]);
                        }
                    }
                    reader.Close();
                    if (num == 4)
                    {
                        return false;
                    }
                    flag2 = wgAppConfig.getSystemParamByNO(0x39).ToString() == "1";
                }
            }
            return flag2;
        }

        private void reloadData(string strsql)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.bLoadedFinished = false;
                this.startRecordIndex = 0;
                this.MaxRecord = 0x3e8;
                if (!string.IsNullOrEmpty(strsql))
                {
                    this.dgvSql = strsql;
                }
                this.dgvMain.DataSource = null;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wgAppConfig.RestoreGVStyle(this, this.dgvMain);
            wgAppConfig.UpdateKeyVal(base.Name + "-" + this.dgvMain.Tag, "");
            this.loadDefaultStyle();
            this.loadStyle();
        }

        private void saveColumns()
        {
            string str = "";
            string str2 = "";
            for (int i = 0; i < this.arrColsName.Count; i++)
            {
                if (str != "")
                {
                    str = str + ",";
                    str2 = str2 + ",";
                }
                str = str + this.arrColsName[i];
                str2 = str2 + this.arrColsShow[i].ToString();
            }
            wgAppConfig.InsertKeyVal(base.Name + "-" + this.dgvMain.Tag, str + ";" + str2);
            wgAppConfig.UpdateKeyVal(base.Name + "-" + this.dgvMain.Tag, str + ";" + str2);
        }

        private void saveDefaultStyle()
        {
            DataTable table = new DataTable();
            this.dsDefaultStyle.Tables.Add(table);
            table.TableName = this.dgvMain.Name;
            table.Columns.Add("colName");
            table.Columns.Add("colHeader");
            table.Columns.Add("colWidth");
            table.Columns.Add("colVisable");
            table.Columns.Add("colDisplayIndex");
            for (int i = 0; i < this.dgvMain.ColumnCount; i++)
            {
                DataGridViewColumn column = this.dgvMain.Columns[i];
                DataRow row = table.NewRow();
                row["colName"] = column.Name;
                row["colHeader"] = column.HeaderText;
                row["colWidth"] = column.Width;
                row["colVisable"] = column.Visible;
                row["colDisplayIndex"] = column.DisplayIndex;
                table.Rows.Add(row);
                table.AcceptChanges();
            }
        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wgAppConfig.SaveDGVStyle(this, this.dgvMain);
            XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
        }

        public class ToolStripDateTime : ToolStripControlHost
        {
            private static DateTimePicker dtp;

            public ToolStripDateTime() : base(dtp = new DateTimePicker())
            {
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && (dtp != null))
                {
                    dtp.Dispose();
                }
                base.Dispose(disposing);
            }

            public void SetTimeFormat()
            {
                DateTimePicker control = base.Control as DateTimePicker;
                control.CustomFormat = "HH;mm";
                control.Format = DateTimePickerFormat.Custom;
                control.ShowUpDown = true;
            }

            public int BoxWidth
            {
                get
                {
                    return (base.Control as DateTimePicker).Size.Width;
                }
                set
                {
                    base.Control.Size = new Size(new Point(value, base.Control.Size.Height));
                    (base.Control as DateTimePicker).Size = new Size(new Point(value, base.Control.Size.Height));
                }
            }

            public DateTimePicker DateTimeControl
            {
                get
                {
                    return (base.Control as DateTimePicker);
                }
            }

            public DateTime Value
            {
                get
                {
                    return (base.Control as DateTimePicker).Value;
                }
                set
                {
                    DateTime time;
                    if ((DateTime.TryParse(value.ToString(), out time) && (time >= (base.Control as DateTimePicker).MinDate)) && (time <= (base.Control as DateTimePicker).MaxDate))
                    {
                        (base.Control as DateTimePicker).Value = time;
                    }
                }
            }
        }
    }
}

