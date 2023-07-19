namespace WG3000_COMM.ExtendFunc.Patrol
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

    public partial class frmPatrolReport : frmBioAccess
    {
        private ArrayList arrColsName = new ArrayList();
        private ArrayList arrColsShow = new ArrayList();
        private bool bFirstQuery = true;
        private bool bLoadedFinished;
        private bool bLogCreateReport;
        private dfrmPatrolReportFindOption dfrmFindOption;
        private string dgvSql = "";
        private DataSet dsDefaultStyle = new DataSet("DGV_STILE");
        private int MaxRecord = 0x3e8;
        private int recIdMax;
        private int startRecordIndex;
        private DataTable table;
        private UserControlFind userControlFind1;

        public frmPatrolReport()
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
            string oldValue = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO ";
            string str3 = oldValue;
            str3 = str3 + " FROM t_b_Consumer WHERE (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ";
            if (findConsumerID > 0)
            {
                str3 = oldValue;
                str3 = str3 + " FROM t_b_Consumer WHERE ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
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
                str3 = str3 + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) ";
            }
            else if (findName != "")
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName))) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) ";
            }
            else if (findCard > 0L)
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_CardNO ={0:d} ", findCard) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
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
                        if (form.Name == "dfrmPatrolReportCreate")
                        {
                            return;
                        }
                    }
                }
                using (dfrmPatrolReportCreate create = new dfrmPatrolReportCreate())
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
        }

        private void btnFindOption_Click(object sender, EventArgs e)
        {
            if (this.dfrmFindOption == null)
            {
                this.dfrmFindOption = new dfrmPatrolReportFindOption();
                this.dfrmFindOption.Owner = this;
            }
            this.dfrmFindOption.Show();
        }

        private void btnPatrolRoute_Click(object sender, EventArgs e)
        {
            using (frmPatrolRoute route = new frmPatrolRoute())
            {
                route.ShowDialog();
            }
        }

        private void btnPatrolSetup_Click(object sender, EventArgs e)
        {
            using (dfrmPatrolSetup setup = new dfrmPatrolSetup())
            {
                setup.ShowDialog();
            }
        }

        private void btnPatrolTask_Click(object sender, EventArgs e)
        {
            using (frmPatrolTaskData data = new frmPatrolTaskData())
            {
                data.ShowDialog();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvMain, this.Text);
        }

        public void btnQuery_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.getLogCreateReport();
            if (!this.bFirstQuery && !this.bLogCreateReport)
            {
                XMessageBox.Show(this, CommonStr.strGetRecordsBeforeCreateReport + "\r\n\r\n" + CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.bFirstQuery = false;
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
                string strBaseInfo = " SELECT t_d_PatrolDetailData.f_RecID, t_b_Group.f_GroupName, ";
                strBaseInfo = ((strBaseInfo + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, ") + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, " + " t_d_PatrolDetailData.f_PlanPatrolTime AS f_patroldate, ") + " t_d_PatrolDetailData.f_PlanPatrolTime, " + " t_d_PatrolDetailData.f_RealPatrolTime, ";
                if (wgAppConfig.IsAccessDB)
                {
                    strBaseInfo = strBaseInfo + string.Format("IIF(f_EventDesc=0, {0} ,IIF(f_EventDesc=1,{1}, IIF(f_EventDesc=2, {2}, IIF(f_EventDesc=3, {3}, IIF(f_EventDesc=4, {4},''))))) AS  [f_EventDesc] ,  ", new object[] { wgTools.PrepareStr(this.getEventDescStr(0)), wgTools.PrepareStr(this.getEventDescStr(1)), wgTools.PrepareStr(this.getEventDescStr(2)), wgTools.PrepareStr(this.getEventDescStr(3)), wgTools.PrepareStr(this.getEventDescStr(4)) });
                }
                else
                {
                    strBaseInfo = strBaseInfo + string.Format("CASE WHEN f_EventDesc=0 THEN {0} ELSE ( CASE WHEN f_EventDesc=1 THEN {1} ELSE ( CASE WHEN f_EventDesc=2 THEN {2} ELSE (CASE WHEN f_EventDesc=3 THEN {3} ELSE (CASE WHEN f_EventDesc=4 THEN {4} ELSE '' END) END) END) END) END AS  [f_EventDesc] ,  ", new object[] { wgTools.PrepareStr(this.getEventDescStr(0)), wgTools.PrepareStr(this.getEventDescStr(1)), wgTools.PrepareStr(this.getEventDescStr(2)), wgTools.PrepareStr(this.getEventDescStr(3)), wgTools.PrepareStr(this.getEventDescStr(4)) });
                }
                strBaseInfo = (strBaseInfo + " f_RouteName, ") + " f_ReaderName, " + " '' as f_Description";
                string strsql = this.getSqlFindNormal(strBaseInfo, "t_d_PatrolDetailData", this.getSqlOfDateTime("t_d_PatrolDetailData.f_patroldate"), groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                if (flag)
                {
                    flag = false;
                    strsql = strsql + " WHERE " + str2;
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
            using (frmPatrolStatistics statistics = new frmPatrolStatistics())
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
            string oldValue = " SELECT   f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO ";
            string str3 = oldValue;
            str3 = str3 + " FROM t_b_Consumer WHERE ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
            if (findConsumerID > 0)
            {
                str3 = oldValue;
                str3 = str3 + " FROM t_b_Consumer WHERE ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) " + string.Format(" AND t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
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
                str3 = str3 + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers )) ) ";
            }
            else if (findName != "")
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName))) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
            }
            else if (findCard > 0L)
            {
                str3 = oldValue;
                str3 = (str3 + " FROM t_b_Consumer  ") + string.Format(" WHERE f_CardNO ={0:d} ", findCard) + " AND ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ";
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
            str3 = str3 + " FROM t_b_Consumer  " + string.Format(" WHERE f_ConsumerID IN ({0}) AND {1} ", selected.selectedUsers, "   ( (t_b_Consumer.f_ConsumerID IN (SELECT t_d_PatrolUsers.f_ConsumerID FROM t_d_PatrolUsers ))  ) ");
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
                        if (form.Name == "dfrmPatrolReportCreate")
                        {
                            return;
                        }
                    }
                }
                using (dfrmPatrolReportCreate create = new dfrmPatrolReportCreate())
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
                    for (int i = 0; (i < dt.Columns.Count) && (i < this.dgvMain.Columns.Count); i++)
                    {
                        this.dgvMain.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
                        this.dgvMain.Columns[i].Name = dt.Columns[i].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_PatrolDate", wgTools.DisplayFormat_DateYMDWeek);
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_PlanPatrolTime", "HH:mm");
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_RealPatrolTime", "HH:mm:ss");
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
            this.Refresh();
            this.userControlFind1.btnQuery.PerformClick();
        }

        public string getEventDescStr(int code)
        {
            switch (code)
            {
                case 0:
                    return CommonStr.strPatrolEventRest;

                case 1:
                    return CommonStr.strPatrolEventNormal;

                case 2:
                    return CommonStr.strPatrolEventEarly;

                case 3:
                    return CommonStr.strPatrolEventLate;

                case 4:
                    return CommonStr.strPatrolEventAbsence;
            }
            return "";
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
                string cmdText = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
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
            string cmdText = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
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

        private string getSqlFindNormal(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
        {
            string str = "";
            try
            {
                string str2 = "";
                if (!string.IsNullOrEmpty(strTimeCon))
                {
                    str2 = str2 + string.Format("AND {0}", strTimeCon);
                }
                if (findConsumerID > 0)
                {
                    str2 = str2 + string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
                    return (strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID))  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  )  ", fromMainDt, str2));
                }
                if (!string.IsNullOrEmpty(findName))
                {
                    str2 = str2 + string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                if (findCard > 0L)
                {
                    str2 = str2 + string.Format(" AND t_b_Consumer.f_CardNO ={0:d} ", findCard);
                }
                if (groupMinNO > 0)
                {
                    if (groupMinNO >= groupMaxNO)
                    {
                        return (strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID)) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO)));
                    }
                    return (strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID)) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO)));
                }
                str = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID)) LEFT JOIN t_d_PatrolRouteList on ( t_d_PatrolRouteList.f_RouteID = {0}.f_RouteID)) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
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
                strSql = strSql + string.Format(" AND t_d_PatrolDetailData.f_RecID > {0:d}", this.recIdMax);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE t_d_PatrolDetailData.f_RecID > {0:d}", this.recIdMax);
            }
            strSql = strSql + " ORDER BY t_d_PatrolDetailData.f_RecID ";
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
            string funName = "mnuPatrolDetailData";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnCreateReport.Visible = false;
            }
        }

        private void loadStyle()
        {
            this.dgvMain.AutoGenerateColumns = false;
            this.arrColsName.Clear();
            this.arrColsShow.Clear();
            for (int i = 0; i < this.dgvMain.ColumnCount; i++)
            {
                this.arrColsName.Add(this.dgvMain.Columns[i].HeaderText);
                this.arrColsShow.Add(this.dgvMain.Columns[i].Visible);
            }
            string str = "";
            string str2 = "";
            for (int j = 0; j < this.arrColsName.Count; j++)
            {
                if (str != "")
                {
                    str = str + ",";
                    str2 = str2 + ",";
                }
                str = str + this.arrColsName[j];
                str2 = str2 + this.arrColsShow[j].ToString();
            }
            string keyVal = wgAppConfig.GetKeyVal(base.Name + "-" + this.dgvMain.Tag);
            if (keyVal != "")
            {
                string[] strArray = keyVal.Split(new char[] { ';' });
                if (((strArray.Length == 2) && (str == strArray[0])) && (str2 != strArray[1]))
                {
                    string[] strArray2 = strArray[1].Split(new char[] { ',' });
                    if (strArray2.Length == this.arrColsName.Count)
                    {
                        this.arrColsShow.Clear();
                        for (int k = 0; k < this.dgvMain.ColumnCount; k++)
                        {
                            this.dgvMain.Columns[k].Visible = bool.Parse(strArray2[k]);
                            this.arrColsShow.Add(this.dgvMain.Columns[k].Visible);
                        }
                    }
                }
            }
            wgAppConfig.ReadGVStyle(this, this.dgvMain);
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

