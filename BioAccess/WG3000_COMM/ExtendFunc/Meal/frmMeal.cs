namespace WG3000_COMM.ExtendFunc.Meal
{
    using System;
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

    public partial class frmMeal : frmBioAccess
    {
        private bool bLoadedFinished;
        private dfrmSwipeRecordsFindOption dfrmFindOption;
        private string dgvSql = "";
        private DataSet ds;
        private DataSet dsDefaultStyle = new DataSet("DGV_STILE");
        private DataView dv;
        private DataView dvConsumerStatistics = new DataView();
        private DataView dvReaderStatistics;
        private int MaxRecord = 0x3e8;
        private string oldStatTitle = "";
        private int recIdMin;
        private int startRecordIndex;
        public string strFindOption = "";
        private DataTable table;
        private UserControlFind userControlFind1;

        public frmMeal()
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
            e.Result = this.loadSwipeRecords(startIndex, maxRecords, strSql);
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
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvSwipeRecords.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
            }
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.startDeal();
            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 1)
            {
                wgAppConfig.exportToExcel(this.dgvSubtotal, this.Text + " [" + this.tabPage2.Text + "]");
            }
            else if (this.tabControl1.SelectedIndex == 2)
            {
                wgAppConfig.exportToExcel(this.dgvStatistics, this.Text + " [" + this.tabPage3.Text + "]");
            }
            else
            {
                wgAppConfig.exportToExcel(this.dgvSwipeRecords, this.Text + " [" + this.tabPage1.Text + "]");
            }
        }

        private void btnFindOption_Click(object sender, EventArgs e)
        {
            if (this.dfrmFindOption == null)
            {
                this.dfrmFindOption = new dfrmSwipeRecordsFindOption();
                this.dfrmFindOption.Owner = this;
            }
            this.dfrmFindOption.Show();
        }

        private void btnMealSetup_Click(object sender, EventArgs e)
        {
            using (dfrmMealSetup setup = new dfrmMealSetup())
            {
                setup.ShowDialog();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 1)
            {
                wgAppConfig.printdgv(this.dgvSubtotal, this.Text + " [" + this.tabPage2.Text + "]");
            }
            else if (this.tabControl1.SelectedIndex == 2)
            {
                wgAppConfig.printdgv(this.dgvStatistics, this.Text + " [" + this.tabPage3.Text + "]");
            }
            else
            {
                wgAppConfig.printdgv(this.dgvSwipeRecords, this.Text + " [" + this.tabPage1.Text + "]");
            }
        }

        public void btnQuery_Click(object sender, EventArgs e)
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
                str2 = " (t_d_SwipeRecord.f_ReaderID IN ( " + this.dfrmFindOption.getStrSql() + " )) ";
            }
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID,  ";
            strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, ") + 
                "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName, " + 
                "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
            string strTimeCon = " ( 1>0 ) ";
            if (this.getSqlOfDateTime() != "")
            {
                strTimeCon = strTimeCon + string.Format(" AND {0} ", this.getSqlOfDateTime());
            }
            if (flag)
            {
                flag = false;
                strTimeCon = strTimeCon + string.Format(" AND {0} ", str2);
            }
            string strsql = wgAppConfig.getSqlFindSwipeRecord(strBaseInfo, "t_d_SwipeRecord", 
                strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
            this.reloadData(strsql);
        }

        public void btnQuery_Click_Acc(object sender, EventArgs e)
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
                str2 = " (t_d_SwipeRecord.f_ReaderID IN ( " + this.dfrmFindOption.getStrSql() + " )) ";
            }
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID,  ";
            strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, ") + 
                "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName, " + 
                "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
            string strTimeCon = " ( 1>0 ) ";
            if (this.getSqlOfDateTime() != "")
            {
                strTimeCon = strTimeCon + string.Format(" AND {0} ", this.getSqlOfDateTime());
            }
            if (flag)
            {
                flag = false;
                strTimeCon = strTimeCon + string.Format(" AND {0} ", str2);
            }
            string strsql = wgAppConfig.getSqlFindSwipeRecord(strBaseInfo, "t_d_SwipeRecord", 
                strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
            this.reloadData(strsql);
        }

        private void dgvSwipeRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (((e.ColumnIndex >= 0) && (e.ColumnIndex < this.dgvSwipeRecords.Columns.Count)) && this.dgvSwipeRecords.Columns[e.ColumnIndex].Name.Equals("f_Desc"))
            {
                switch ((e.Value as string))
                {
                    case null:
                    case " ":
                    {
                        DataGridViewCell cell = this.dgvSwipeRecords[e.ColumnIndex, e.RowIndex];
                        string str2 = this.dgvSwipeRecords[e.ColumnIndex + 1, e.RowIndex].Value as string;
                        if (string.IsNullOrEmpty(str2))
                        {
                            e.Value = "";
                            cell.Value = "";
                        }
                        else
                        {
                            e.Value = new MjRec(str2.PadLeft(0x30, '0'), false).GetDetailedRecord(null, 0);
                            cell.Value = e.Value;
                        }
                        break;
                    }
                }
            }
        }

        private void dgvSwipeRecords_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.bLoadedFinished && (e.ScrollOrientation == ScrollOrientation.VerticalScroll))
            {
                wgTools.WriteLine(e.OldValue.ToString());
                wgTools.WriteLine(e.NewValue.ToString());
                if ((e.NewValue > e.OldValue) && (((e.NewValue + 100) > this.dgvSwipeRecords.Rows.Count) || ((e.NewValue + (this.dgvSwipeRecords.Rows.Count / 10)) > this.dgvSwipeRecords.Rows.Count)))
                {
                    if (this.startRecordIndex <= this.dgvSwipeRecords.Rows.Count)
                    {
                        if (!this.backgroundWorker1.IsBusy)
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                        }
                    }
                    else
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvSwipeRecords.Rows.Count.ToString() + "#");
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
                if (this.dgvSwipeRecords.DataSource == null)
                {
                    this.dgvSwipeRecords.DataSource = dt;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        this.dgvSwipeRecords.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
                        this.dgvSwipeRecords.Columns[i].Name = dt.Columns[i].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(this.dgvSwipeRecords, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
                    wgAppConfig.ReadGVStyle(this, this.dgvSwipeRecords);
                    if ((this.startRecordIndex == 0) && (dt.Rows.Count >= this.MaxRecord))
                    {
                        this.startRecordIndex += this.MaxRecord;
                        wgTools.WgDebugWrite("First 1000", new object[0]);
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                    }
                }
                else if (dt.Rows.Count > 0)
                {
                    int firstDisplayedScrollingRowIndex = this.dgvSwipeRecords.FirstDisplayedScrollingRowIndex;
                    (this.dgvSwipeRecords.DataSource as DataTable).Merge(dt);
                    if (firstDisplayedScrollingRowIndex > 0)
                    {
                        this.dgvSwipeRecords.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmSwipeRecords_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFindOption != null)
            {
                this.dfrmFindOption.Close();
            }
        }

        private void frmSwipeRecords_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void frmSwipeRecords_Load(object sender, EventArgs e)
        {
            this.oldStatTitle = this.tabPage3.Text;
            this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRomm(this.f_DepartmentName.HeaderText);
            this.f_DepartmentName2.HeaderText = wgAppConfig.ReplaceFloorRomm(this.f_DepartmentName2.HeaderText);
            this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
            this.f_ConsumerNO2.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO2.HeaderText);
            this.loadOperatorPrivilege();
            this.saveDefaultStyle();
            this.loadStyle();
            this.dtpDateFrom = new ToolStripDateTime();
            this.dtpDateTo = new ToolStripDateTime();
            this.dtpDateTo.Value = DateTime.Now.Date;
            this.dtpDateFrom.Value = DateTime.Now.Date;
            this.toolStrip3.Items.Clear();
            this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2, this.dtpDateFrom, this.toolStripLabel3, this.dtpDateTo });
            this.userControlFind1.toolStripLabel2.Visible = false;
            this.userControlFind1.txtFindCardID.Visible = false;
            this.dtpDateFrom.BoxWidth = 120;
            this.dtpDateTo.BoxWidth = 120;
            this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRomm(this.f_DepartmentName.HeaderText);
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.dtpDateFrom.BoxWidth = 150;
            this.dtpDateTo.BoxWidth = 150;
            wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            this.userControlFind1.btnQuery.Visible = false;
            this.bLoadedFinished = true;
        }

        private string getQueryConsumerConditionStr(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
        {
            string str = "";
            try
            {
                string str2 = " WHERE (1>0) ";
                if (findConsumerID > 0)
                {
                    return (strBaseInfo + string.Format("  FROM t_b_Consumer  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) WHERE   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID));
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
                        str = strBaseInfo + string.Format("  FROM t_b_Consumer  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {0} )  ", string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
                    }
                    else
                    {
                        str = strBaseInfo + string.Format("  FROM t_b_Consumer  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {0} )  ", string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
                    }
                }
                else
                {
                    str = strBaseInfo + string.Format("  FROM t_b_Consumer  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  )  ", new object[0]);
                }
                str = str + str2;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        private string getSqlFindSwipeRecord4Meal(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
        {
            string str = "";
            try
            {
                string str2 = "";
                string str3 = " WHERE (1>0) ";
                if (!string.IsNullOrEmpty(strTimeCon))
                {
                    str3 = str3 + string.Format("AND {0}", strTimeCon);
                }
                if (findConsumerID > 0)
                {
                    str2 = str2 + string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
                    str = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN  t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2);
                    return (str + str3);
                }
                if (!string.IsNullOrEmpty(findName))
                {
                    str3 = str3 + string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                if (findCard > 0L)
                {
                    str3 = str3 + string.Format(" AND {0}.f_CardNO ={1:d} ", fromMainDt, findCard);
                }
                if (groupMinNO > 0)
                {
                    if (groupMinNO >= groupMaxNO)
                    {
                        str = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
                    }
                    else
                    {
                        str = strBaseInfo + string.Format(" FROM (((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
                    }
                }
                else
                {
                    str = strBaseInfo + string.Format(" FROM ((({0} INNER JOIN t_b_Consumer ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID))  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) )  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2);
                }
                str = str + str3;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        private string getSqlOfDateTime()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getSqlOfDateTime_Acc();
            }
            string str = "";
            str = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
            if (str != "")
            {
                str = str + " AND ";
            }
            return (str + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")");
        }

        private string getSqlOfDateTime_Acc()
        {
            string str = "";
            str = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
            if (str != "")
            {
                str = str + " AND ";
            }
            return (str + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")");
        }

        private void loadDefaultStyle()
        {
            DataTable table = this.dsDefaultStyle.Tables[this.dgvSwipeRecords.Name];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                this.dgvSwipeRecords.Columns[i].Name = table.Rows[i]["colName"].ToString();
                this.dgvSwipeRecords.Columns[i].HeaderText = table.Rows[i]["colHeader"].ToString();
                this.dgvSwipeRecords.Columns[i].Width = int.Parse(table.Rows[i]["colWidth"].ToString());
                this.dgvSwipeRecords.Columns[i].Visible = bool.Parse(table.Rows[i]["colVisable"].ToString());
                this.dgvSwipeRecords.Columns[i].DisplayIndex = int.Parse(table.Rows[i]["colDisplayIndex"].ToString());
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuConstMeal";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnMealSetup.Visible = false;
            }
        }

        private void loadStyle()
        {
            this.dgvSwipeRecords.AutoGenerateColumns = false;
            wgAppConfig.ReadGVStyle(this, this.dgvSwipeRecords);
        }

        private DataTable loadSwipeRecords(int startIndex, int maxRecords, string strSql)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadSwipeRecords Start");
            if (strSql.ToUpper().IndexOf("SELECT ") > 0)
            {
                strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            }
            if (startIndex == 0)
            {
                this.recIdMin = 0x7fffffff;
            }
            else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
            {
                strSql = strSql + string.Format(" AND f_RecID < {0:d}", this.recIdMin);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE f_RecID < {0:d}", this.recIdMin);
            }
            strSql = strSql + " ORDER BY f_RecID DESC ";
            this.table = new DataTable();
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
                    goto Label_017B;
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
        Label_017B:
            if (this.table.Rows.Count > 0)
            {
                this.recIdMin = int.Parse(this.table.Rows[this.table.Rows.Count - 1][0].ToString());
            }
            wgTools.WriteLine("da.Fill End " + startIndex.ToString());
            Cursor.Current = Cursors.Default;
            return this.table;
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
                this.dgvSwipeRecords.DataSource = null;
                this.timer1.Enabled = true;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        private void restoreDefaultLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wgAppConfig.RestoreGVStyle(this, this.dgvSwipeRecords);
            this.loadDefaultStyle();
            this.loadStyle();
        }

        private void saveDefaultStyle()
        {
            DataTable table = new DataTable();
            this.dsDefaultStyle.Tables.Add(table);
            table.TableName = this.dgvSwipeRecords.Name;
            table.Columns.Add("colName");
            table.Columns.Add("colHeader");
            table.Columns.Add("colWidth");
            table.Columns.Add("colVisable");
            table.Columns.Add("colDisplayIndex");
            for (int i = 0; i < this.dgvSwipeRecords.ColumnCount; i++)
            {
                DataGridViewColumn column = this.dgvSwipeRecords.Columns[i];
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
            wgAppConfig.SaveDGVStyle(this, this.dgvSwipeRecords);
            XMessageBox.Show(sender.ToString() + " " + CommonStr.strSuccessfully);
        }

        public void startDeal()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.startDeal_Acc();
            }
            else
            {
                this.btnCreateReport.Enabled = false;
                Cursor current = Cursor.Current;
                string str = "";
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                Cursor.Current = Cursors.WaitCursor;
                this.btnCreateReport.Enabled = false;
                try
                {
                    int num;
                    int num11;
                    string str9;
                    int groupMinNO = 0;
                    int groupIDOfMinNO = 0;
                    int groupMaxNO = 0;
                    string findName = "";
                    long findCard = 0L;
                    int findConsumerID = 0;
                    this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                    string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID,  ";
                    strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, ") + 
                        "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName, t_d_SwipeRecord.f_ReaderID, t_b_Consumer.f_ConsumerID, " +
                        "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
                    string strTimeCon = " ( 1>0 ) ";
                    if (this.getSqlOfDateTime() != "")
                    {
                        strTimeCon = strTimeCon + string.Format(" AND {0} ", this.getSqlOfDateTime());
                    }
                    strTimeCon = strTimeCon + " AND  ([f_ReadDate]<= " + wgTools.PrepareStr(DateTime.Now.AddDays(1.0).ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
                    string cmdText = this.getSqlFindSwipeRecord4Meal(strBaseInfo, "t_d_SwipeRecord",
                        strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                    this.tabPage3.Text = this.oldStatTitle + string.Format("({0} {1} {2})", 
                        this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD), 
                        this.toolStripLabel3.Text.Replace(":", ""), 
                        this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD));
                    this.ProgressBar1.Value = 0;
                    this.ProgressBar1.Maximum = 100;
                    this.ProgressBar1.Value = 30;
                    this.ds = new DataSet("inout");
                    SqlCommand selectCommand = new SqlCommand(cmdText);
                    selectCommand.Connection = connection;
                    selectCommand.CommandTimeout = 180;
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    this.ProgressBar1.Value = 40;
                    adapter.Fill(this.ds, "t_d_SwipeRecord");
                    this.ProgressBar1.Value = 60;
                    cmdText = " SELECT  f_RecID, 0 as f_ConsumerID, '' AS f_GroupName, '' as f_ConsumerNO,  '' AS f_ConsumerName,t_d_SwipeRecord.f_ReadDate , '' as f_MealName, 0.01 as f_Cost, '' as [f_ReaderName],f_ReaderID  ";
                    SqlCommand command = new SqlCommand(cmdText + " FROM t_d_SwipeRecord " + " WHERE 1<0 ", connection);
                    adapter.SelectCommand = command;
                    adapter.Fill(this.ds, "MealReport");
                    cmdText = "  SELECT  t_d_Reader4Meal.*  ";
                    command = new SqlCommand(cmdText + " FROM  t_b_Reader,t_d_Reader4Meal, t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID " + " ORDER BY  t_b_Reader.f_ReaderID  ", connection);
                    adapter.SelectCommand = command;
                    adapter.Fill(this.ds, "t_d_Reader4Meal");
                    DataView view = new DataView(this.ds.Tables["t_d_Reader4Meal"]);
                    cmdText = "  SELECT  t_b_Reader.f_ReaderID , t_b_Reader.[f_ReaderName], 0 as f_CostCount, 0.01 as f_CostTotal4Reader  ";
                    command = new SqlCommand(cmdText + " FROM  t_b_Reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID " + " ORDER BY  t_b_Reader.f_ReaderID  ", connection);
                    adapter.SelectCommand = command;
                    adapter.Fill(this.ds, "ReaderStatistics");
                    DataTable table = this.ds.Tables["ReaderStatistics"];
                    this.dvReaderStatistics = new DataView(this.ds.Tables["ReaderStatistics"]);
                    for (num = 0; num <= (this.dvReaderStatistics.Count - 1); num++)
                    {
                        this.dvReaderStatistics[num]["f_CostTotal4Reader"] = 0;
                    }
                    table.AcceptChanges();
                    this.ProgressBar1.Value = 70;
                    if (this.dvReaderStatistics.Count > 0)
                    {
                        str = str + "  f_ReaderID IN ( " + this.dvReaderStatistics[0]["f_ReaderID"];
                        for (int i = 1; i <= (this.dvReaderStatistics.Count - 1); i++)
                        {
                            str = str + "," + this.dvReaderStatistics[i]["f_ReaderID"];
                        }
                        str = str + ")";
                    }
                    else
                    {
                        str = str + " 1<0 ";
                    }
                    strBaseInfo = " SELECT    f_ConsumerID, f_GroupName,  f_ConsumerNO, f_ConsumerName ";
                    strBaseInfo = strBaseInfo + " , 0 as f_CostMorningCount, 0 as f_CostLunchCount, 0 as f_CostEveningCount ,0 as f_CostOtherCount, 0 as f_CostTotalCount, 0.01 as f_CostTotal,  0.01 as f_CostMorning, 0.01 as f_CostLunch, 0.01 as f_CostEvening ,0.01 as f_CostOther ";
                    command = new SqlCommand(this.getQueryConsumerConditionStr(strBaseInfo, "", strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID), connection);
                    adapter.SelectCommand = command;
                    adapter.Fill(this.ds, "ConsumerStatistics");
                    DataTable table2 = this.ds.Tables["ConsumerStatistics"];
                    this.dvConsumerStatistics = new DataView(this.ds.Tables["ConsumerStatistics"]);
                    for (num = 0; num <= (this.dvConsumerStatistics.Count - 1); num++)
                    {
                        table2.Rows[num]["f_CostMorning"] = 0;
                        table2.Rows[num]["f_CostLunch"] = 0;
                        table2.Rows[num]["f_CostEvening"] = 0;
                        table2.Rows[num]["f_CostOther"] = 0;
                        table2.Rows[num]["f_CostTotal"] = 0;
                    }
                    table2.AcceptChanges();
                    this.ProgressBar1.Value = 80;
                    DataTable table3 = this.ds.Tables["MealReport"];
                    DataView view2 = new DataView(this.ds.Tables["t_d_SwipeRecord"]);
                    new SqlDataAdapter("SELECT * from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) ", connection).Fill(this.ds, "t_b_reader");
                    DataTable table4 = this.ds.Tables["t_b_reader"];
                    new DataView(table4);
                    int num8 = 0;
                    int num9 = 0;
                    SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString);
                    selectCommand = new SqlCommand("SELECT * from t_b_MealSetup WHERE f_ID=1 ", connection2);
                    if (connection2.State != ConnectionState.Open)
                    {
                        connection2.Open();
                    }
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) == 1)
                        {
                            num9 = 1;
                        }
                        else if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) == 2)
                        {
                            num9 = 2;
                            try
                            {
                                num8 = (int) decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                            }
                            catch (Exception exception)
                            {
                                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                            }
                        }
                        else
                        {
                            num9 = 0;
                            num8 = 0;
                        }
                    }
                    reader.Close();
                    selectCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
                    if (connection2.State != ConnectionState.Open)
                    {
                        connection2.Open();
                    }
                    reader = selectCommand.ExecuteReader();
                    int[] numArray = new int[4];
                    string[] strArray = new string[4];
                    string[] strArray2 = new string[4];
                    DateTime[] timeArray = new DateTime[4];
                    DateTime[] timeArray2 = new DateTime[4];
                    decimal[] numArray2 = new decimal[4];
                    int index = 0;
                    while (index <= (numArray.Length - 1))
                    {
                        numArray[index] = 0;
                        timeArray[index] = DateTime.Parse("00:00");
                        timeArray2[index] = DateTime.Parse("00:00");
                        numArray2[index] = 0M;
                        index++;
                    }
                    strArray[0] = CommonStr.strMealName0;
                    strArray[1] = CommonStr.strMealName1;
                    strArray[2] = CommonStr.strMealName2;
                    strArray[3] = CommonStr.strMealName3;
                    strArray2[0] = "f_CostMorning";
                    strArray2[1] = "f_CostLunch";
                    strArray2[2] = "f_CostEvening";
                    strArray2[3] = "f_CostOther";
                    while (reader.Read())
                    {
                        if (((((int) reader["f_ID"]) == 2) || (((int) reader["f_ID"]) == 3)) || ((((int) reader["f_ID"]) == 4) || (((int) reader["f_ID"]) == 5)))
                        {
                            if (((int) reader["f_Value"]) > 0)
                            {
                                index = ((int) reader["f_ID"]) - 2;
                                numArray[index] = 1;
                                timeArray[index] = (DateTime) reader["f_BeginHMS"];
                                timeArray[index] = timeArray[index].AddSeconds((double) -timeArray[index].Second);
                                timeArray2[index] = (DateTime) reader["f_EndHMS"];
                                timeArray2[index] = timeArray2[index].AddSeconds((double) (0x3b - timeArray2[index].Second));
                                numArray2[index] = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                            }
                        }
                        else if ((((int) reader["f_ID"]) == 6) && (((int) reader["f_Value"]) > 0))
                        {
                            str = str + " AND f_Character=1 ";
                        }
                    }
                    reader.Close();
                    view2.Sort = "f_ReadDate ASC";
                    this.ProgressBar1.Value = 0;
                    this.dvConsumerStatistics = new DataView(this.ds.Tables["ConsumerStatistics"]);
                    this.ProgressBar1.Maximum = Math.Max(0, this.dvConsumerStatistics.Count);
                    for (num11 = 0; num11 <= (this.dvConsumerStatistics.Count - 1); num11++)
                    {
                        this.ProgressBar1.Value = num11;
                        view2.RowFilter = str + " AND  f_ConsumerID = " + this.dvConsumerStatistics[num11]["f_ConsumerID"];
                        if (view2.Count > 0)
                        {
                            bool flag = false;
                            string str6 = "";
                            DateTime time2 = DateTime.Parse("2100-1-1");
                            string str7 = "";
                            int num13 = -1;
                            string str8 = "";
                            for (int j = 0; j <= (view2.Count - 1); j++)
                            {
                                DateTime time = (DateTime) view2[j]["f_ReadDate"];
                                flag = false;
                                index = 0;
                                while (index <= (numArray.Length - 1))
                                {
                                    if (numArray[index] > 0)
                                    {
                                        if (index < 3)
                                        {
                                            if ((string.Compare(time.ToString("HH:mm"), timeArray[index].ToString("HH:mm")) < 0) || (string.Compare(time.ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) > 0))
                                            {
                                                goto Label_0B93;
                                            }
                                            flag = true;
                                            str6 = strArray[index];
                                            str8 = strArray2[index];
                                            break;
                                        }
                                        if (string.Compare(timeArray[index].ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) < 0)
                                        {
                                            if ((string.Compare(time.ToString("HH:mm"), timeArray[index].ToString("HH:mm")) < 0) || (string.Compare(time.ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) > 0))
                                            {
                                                goto Label_0B93;
                                            }
                                            flag = true;
                                            str6 = strArray[index];
                                            str8 = strArray2[index];
                                            break;
                                        }
                                        if ((string.Compare(time.ToString("HH:mm"), timeArray[index].ToString("HH:mm")) >= 0) || (string.Compare(time.ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) <= 0))
                                        {
                                            flag = true;
                                            str6 = strArray[index];
                                            str8 = strArray2[index];
                                            break;
                                        }
                                    }
                                Label_0B93:
                                    index++;
                                }
                                if (flag)
                                {
                                    bool flag2 = true;
                                    TimeSpan span = Convert.ToDateTime(view2[j]["f_ReadDate"]).Subtract(time2);
                                    if (((num9 == 1) && (str7 == str6)) && (Math.Abs(span.TotalHours) < 12.0))
                                    {
                                        flag2 = false;
                                    }
                                    if (((num9 == 2) && (str7 == str6)) && ((Math.Abs(span.TotalSeconds) < num8) && (num13 == ((int) view2[j]["f_ReaderID"]))))
                                    {
                                        flag2 = false;
                                    }
                                    if (flag2)
                                    {
                                        str7 = str6;
                                        time2 = (DateTime) view2[j]["f_ReadDate"];
                                        num13 = (int) view2[j]["f_ReaderID"];
                                        DataRow row = table3.NewRow();
                                        row["f_RecID"] = view2[j]["f_RecID"];
                                        row["f_GroupName"] = view2[j]["f_GroupName"];
                                        row["f_ConsumerNO"] = view2[j]["f_ConsumerNO"];
                                        row["f_ConsumerID"] = view2[j]["f_ConsumerID"];
                                        row["f_ConsumerName"] = view2[j]["f_ConsumerName"];
                                        row["f_ReaderName"] = view2[j]["f_ReaderName"];
                                        row["f_ReaderID"] = view2[j]["f_ReaderID"];
                                        row["f_ReadDate"] = view2[j]["f_ReadDate"];
                                        row["f_MealName"] = str6;
                                        row["f_Cost"] = numArray2[index];
                                        if (!string.IsNullOrEmpty(str8))
                                        {
                                            view.RowFilter = string.Format("{0}>=0 AND f_ReaderID ={1} ", str8, row["f_ReaderID"].ToString());
                                            if (view.Count > 0)
                                            {
                                                row["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(view[0][str8]));
                                            }
                                        }
                                        table3.Rows.Add(row);
                                    }
                                }
                            }
                            table3.AcceptChanges();
                        }
                    }
                    this.dv = new DataView(this.ds.Tables["MealReport"]);
                    int num15 = 0;
                    decimal num16 = 0M;
                    this.dv.RowFilter = "";
                    if (this.dv.Count > 0)
                    {
                        this.ProgressBar1.Value = 0;
                        this.ProgressBar1.Maximum = Math.Max(0, table.Rows.Count);
                        for (int k = 0; k <= (table.Rows.Count - 1); k++)
                        {
                            this.ProgressBar1.Value = k;
                            str9 = "f_ReaderID = " + ((int) table.Rows[k]["f_ReaderID"]);
                            this.dv.RowFilter = str9;
                            if (this.dv.Count > 0)
                            {
                                if (this.dv.Count > 0)
                                {
                                    table.Rows[k]["f_CostCount"] = ((int) table.Rows[k]["f_CostCount"]) + this.dv.Count;
                                    num15 += this.dv.Count;
                                }
                                for (int m = 0; m <= (this.dv.Count - 1); m++)
                                {
                                    table.Rows[k]["f_CostTotal4Reader"] = ((decimal) table.Rows[k]["f_CostTotal4Reader"]) + ((decimal) this.dv[m]["f_Cost"]);
                                    num16 += (decimal) this.dv[m]["f_Cost"];
                                }
                            }
                        }
                    }
                    DataRow row2 = table.NewRow();
                    row2["f_ReaderName"] = CommonStr.strMealTotal;
                    row2["f_CostCount"] = num15;
                    row2["f_CostTotal4Reader"] = num16;
                    table.Rows.Add(row2);
                    table.AcceptChanges();
                    this.dv.RowFilter = "";
                    this.dv.RowFilter = "";
                    if (this.dv.Count > 0)
                    {
                        this.ProgressBar1.Value = 0;
                        this.ProgressBar1.Maximum = Math.Max(0, table2.Rows.Count);
                        for (num11 = 0; num11 <= (table2.Rows.Count - 1); num11++)
                        {
                            this.ProgressBar1.Value = num11;
                            str9 = "f_ConsumerID = " + table2.Rows[num11]["f_ConsumerID"];
                            this.dv.RowFilter = str9;
                            if (this.dv.Count > 0)
                            {
                                for (index = 0; index <= (numArray.Length - 1); index++)
                                {
                                    if (numArray[index] > 0)
                                    {
                                        this.dv.RowFilter = str9 + " AND f_MealName= " + wgTools.PrepareStr(strArray[index]);
                                        table2.Rows[num11][strArray2[index] + "Count"] = ((int) table2.Rows[num11][strArray2[index] + "Count"]) + this.dv.Count;
                                        table2.Rows[num11]["f_CostTotalCount"] = ((int) table2.Rows[num11]["f_CostTotalCount"]) + this.dv.Count;
                                        for (int n = 0; n <= (this.dv.Count - 1); n++)
                                        {
                                            table2.Rows[num11][strArray2[index]] = ((decimal) table2.Rows[num11][strArray2[index]]) + ((decimal) this.dv[n]["f_Cost"]);
                                            table2.Rows[num11]["f_CostTotal"] = ((decimal) table2.Rows[num11]["f_CostTotal"]) + ((decimal) this.dv[n]["f_Cost"]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    table2.AcceptChanges();
                    DataRow row3 = table2.NewRow();
                    row3["f_GroupName"] = "==========";
                    row3["f_ConsumerNO"] = "==========";
                    row3["f_ConsumerName"] = CommonStr.strMealTotal;
                    index = 4;
                    while (index <= 13)
                    {
                        row3[index] = 0;
                        index++;
                    }
                    for (num11 = 0; num11 <= (table2.Rows.Count - 1); num11++)
                    {
                        this.ProgressBar1.Value = num11;
                        for (index = 4; index <= 13; index++)
                        {
                            if (row3[index].GetType().Name.ToString() == "Decimal")
                            {
                                row3[index] = ((decimal) row3[index]) + ((decimal) table2.Rows[num11][index]);
                            }
                            else
                            {
                                row3[index] = ((int) row3[index]) + ((int) table2.Rows[num11][index]);
                            }
                        }
                    }
                    table2.Rows.Add(row3);
                    this.ProgressBar1.Value = 0;
                    this.dgvSwipeRecords.AutoGenerateColumns = false;
                    this.dgvSubtotal.AutoGenerateColumns = false;
                    this.dgvStatistics.AutoGenerateColumns = false;
                    this.dgvSwipeRecords.DataSource = this.ds.Tables["MealReport"];
                    DataTable table5 = this.ds.Tables["MealReport"];
                    for (num = 0; (num < this.dgvSwipeRecords.ColumnCount) && (num < table5.Columns.Count); num++)
                    {
                        this.dgvSwipeRecords.Columns[num].DataPropertyName = table5.Columns[num].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(this.dgvSwipeRecords, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
                    this.dgvSubtotal.DataSource = table;
                    table5 = table;
                    for (num = 0; (num < this.dgvSubtotal.ColumnCount) && (num < table5.Columns.Count); num++)
                    {
                        this.dgvSubtotal.Columns[num].DataPropertyName = table5.Columns[num].ColumnName;
                    }
                    this.dgvStatistics.DataSource = table2;
                    table5 = table2;
                    for (num = 0; (num < this.dgvStatistics.ColumnCount) && (num < table5.Columns.Count); num++)
                    {
                        this.dgvStatistics.Columns[num].DataPropertyName = table5.Columns[num].ColumnName;
                    }
                    this.dgvSwipeRecords.DefaultCellStyle.ForeColor = Color.Black;
                    this.dgvSubtotal.DefaultCellStyle.ForeColor = Color.Black;
                    this.dgvStatistics.DefaultCellStyle.ForeColor = Color.Black;
                    if (this.dgvSwipeRecords.Rows.Count <= 0)
                    {
                        this.btnPrint.Enabled = false;
                        this.btnExportToExcel.Enabled = false;
                        XMessageBox.Show(CommonStr.strMealNoRecords);
                    }
                    else
                    {
                        this.btnPrint.Enabled = true;
                        this.btnExportToExcel.Enabled = true;
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
                finally
                {
                    this.btnCreateReport.Enabled = true;
                    Cursor.Current = current;
                }
            }
        }

        public void startDeal_Acc()
        {
            this.btnCreateReport.Enabled = false;
            Cursor current = Cursor.Current;
            string str = "";
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            Cursor.Current = Cursors.WaitCursor;
            this.btnCreateReport.Enabled = false;
            try
            {
                int num;
                int num11;
                string str9;
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID,   ";
                strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, ") + 
                    "        t_b_Group.f_GroupName, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_ReaderName, t_d_SwipeRecord.f_ReaderID, t_b_Consumer.f_ConsumerID, " +
                    "        t_d_SwipeRecord.f_Character, ' ' as f_Desc, t_d_SwipeRecord.f_RecordAll ";
                string strTimeCon = " ( 1>0 ) ";
                if (this.getSqlOfDateTime() != "")
                {
                    strTimeCon = strTimeCon + string.Format(" AND {0} ", this.getSqlOfDateTime());
                }
                strTimeCon = strTimeCon + " AND  ([f_ReadDate]<= " + wgTools.PrepareStr(DateTime.Now.AddDays(1.0).ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
                string cmdText = this.getSqlFindSwipeRecord4Meal(strBaseInfo, "t_d_SwipeRecord",
                    strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                this.tabPage3.Text = this.oldStatTitle + string.Format("({0} {1} {2})", this.dtpDateFrom.Value.ToString(wgTools.DisplayFormat_DateYMD), this.toolStripLabel3.Text.Replace(":", ""), this.dtpDateTo.Value.ToString(wgTools.DisplayFormat_DateYMD));
                this.ProgressBar1.Value = 0;
                this.ProgressBar1.Maximum = 100;
                this.ProgressBar1.Value = 30;
                this.ds = new DataSet("inout");
                OleDbCommand selectCommand = new OleDbCommand(cmdText);
                selectCommand.Connection = connection;
                selectCommand.CommandTimeout = 180;
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                this.ProgressBar1.Value = 40;
                adapter.Fill(this.ds, "t_d_SwipeRecord");
                this.ProgressBar1.Value = 60;
                cmdText = " SELECT  f_RecID, 0 as f_ConsumerID, '' AS f_GroupName, '' as f_ConsumerNO,  '' AS f_ConsumerName,t_d_SwipeRecord.f_ReadDate , '' as f_MealName, 0.01 as f_Cost, '' as [f_ReaderName],f_ReaderID  ";
                OleDbCommand command = new OleDbCommand(cmdText + " FROM t_d_SwipeRecord " + " WHERE 1<0 ", connection);
                adapter.SelectCommand = command;
                adapter.Fill(this.ds, "MealReport");
                cmdText = "  SELECT  t_d_Reader4Meal.*  ";
                command = new OleDbCommand(cmdText + " FROM  t_b_Reader,t_d_Reader4Meal  , t_b_Controller WHERE  ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID " + " ORDER BY  t_b_Reader.f_ReaderID  ", connection);
                adapter.SelectCommand = command;
                adapter.Fill(this.ds, "t_d_Reader4Meal");
                DataView view = new DataView(this.ds.Tables["t_d_Reader4Meal"]);
                cmdText = "  SELECT  t_b_Reader.f_ReaderID , t_b_Reader.[f_ReaderName], 0 as f_CostCount, 0.01 as f_CostTotal4Reader  ";
                command = new OleDbCommand(cmdText + " FROM  t_b_Reader,t_d_Reader4Meal , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND t_b_Reader.f_ReaderID = t_d_Reader4Meal.f_ReaderID " + " ORDER BY  t_b_Reader.f_ReaderID  ", connection);
                adapter.SelectCommand = command;
                adapter.Fill(this.ds, "ReaderStatistics");
                DataTable table = this.ds.Tables["ReaderStatistics"];
                this.dvReaderStatistics = new DataView(this.ds.Tables["ReaderStatistics"]);
                for (num = 0; num <= (this.dvReaderStatistics.Count - 1); num++)
                {
                    this.dvReaderStatistics[num]["f_CostTotal4Reader"] = 0;
                }
                table.AcceptChanges();
                this.ProgressBar1.Value = 70;
                if (this.dvReaderStatistics.Count > 0)
                {
                    str = str + "  f_ReaderID IN ( " + this.dvReaderStatistics[0]["f_ReaderID"];
                    for (int i = 1; i <= (this.dvReaderStatistics.Count - 1); i++)
                    {
                        str = str + "," + this.dvReaderStatistics[i]["f_ReaderID"];
                    }
                    str = str + ")";
                }
                else
                {
                    str = str + " 1<0 ";
                }
                strBaseInfo = " SELECT    f_ConsumerID, f_GroupName,  f_ConsumerNO, f_ConsumerName ";
                strBaseInfo = strBaseInfo + " , 0 as f_CostMorningCount, 0 as f_CostLunchCount, 0 as f_CostEveningCount ,0 as f_CostOtherCount, 0 as f_CostTotalCount, 0.01 as f_CostTotal,  0.01 as f_CostMorning, 0.01 as f_CostLunch, 0.01 as f_CostEvening ,0.01 as f_CostOther ";
                command = new OleDbCommand(this.getQueryConsumerConditionStr(strBaseInfo, "", strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID), connection);
                adapter.SelectCommand = command;
                adapter.Fill(this.ds, "ConsumerStatistics");
                DataTable table2 = this.ds.Tables["ConsumerStatistics"];
                this.dvConsumerStatistics = new DataView(this.ds.Tables["ConsumerStatistics"]);
                for (num = 0; num <= (this.dvConsumerStatistics.Count - 1); num++)
                {
                    table2.Rows[num]["f_CostMorning"] = 0;
                    table2.Rows[num]["f_CostLunch"] = 0;
                    table2.Rows[num]["f_CostEvening"] = 0;
                    table2.Rows[num]["f_CostOther"] = 0;
                    table2.Rows[num]["f_CostTotal"] = 0;
                }
                table2.AcceptChanges();
                this.ProgressBar1.Value = 80;
                DataTable table3 = this.ds.Tables["MealReport"];
                DataView view2 = new DataView(this.ds.Tables["t_d_SwipeRecord"]);
                new OleDbDataAdapter("SELECT * from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) ", connection).Fill(this.ds, "t_b_reader");
                DataTable table4 = this.ds.Tables["t_b_reader"];
                new DataView(table4);
                int num8 = 0;
                int num9 = 0;
                OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString);
                selectCommand = new OleDbCommand("SELECT * from t_b_MealSetup WHERE f_ID=1 ", connection2);
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                OleDbDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) == 1)
                    {
                        num9 = 1;
                    }
                    else if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) == 2)
                    {
                        num9 = 2;
                        try
                        {
                            num8 = (int) decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                        }
                        catch (Exception exception)
                        {
                            wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                        }
                    }
                    else
                    {
                        num9 = 0;
                        num8 = 0;
                    }
                }
                reader.Close();
                selectCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                reader = selectCommand.ExecuteReader();
                int[] numArray = new int[4];
                string[] strArray = new string[4];
                string[] strArray2 = new string[4];
                DateTime[] timeArray = new DateTime[4];
                DateTime[] timeArray2 = new DateTime[4];
                decimal[] numArray2 = new decimal[4];
                int index = 0;
                while (index <= (numArray.Length - 1))
                {
                    numArray[index] = 0;
                    timeArray[index] = DateTime.Parse("00:00");
                    timeArray2[index] = DateTime.Parse("00:00");
                    numArray2[index] = 0M;
                    index++;
                }
                strArray[0] = CommonStr.strMealName0;
                strArray[1] = CommonStr.strMealName1;
                strArray[2] = CommonStr.strMealName2;
                strArray[3] = CommonStr.strMealName3;
                strArray2[0] = "f_CostMorning";
                strArray2[1] = "f_CostLunch";
                strArray2[2] = "f_CostEvening";
                strArray2[3] = "f_CostOther";
                while (reader.Read())
                {
                    if (((((int) reader["f_ID"]) == 2) || (((int) reader["f_ID"]) == 3)) || ((((int) reader["f_ID"]) == 4) || (((int) reader["f_ID"]) == 5)))
                    {
                        if (((int) reader["f_Value"]) > 0)
                        {
                            index = ((int) reader["f_ID"]) - 2;
                            numArray[index] = 1;
                            timeArray[index] = (DateTime) reader["f_BeginHMS"];
                            timeArray[index] = timeArray[index].AddSeconds((double) -timeArray[index].Second);
                            timeArray2[index] = (DateTime) reader["f_EndHMS"];
                            timeArray2[index] = timeArray2[index].AddSeconds((double) (0x3b - timeArray2[index].Second));
                            numArray2[index] = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                        }
                    }
                    else if ((((int) reader["f_ID"]) == 6) && (((int) reader["f_Value"]) > 0))
                    {
                        str = str + " AND f_Character=1 ";
                    }
                }
                view2.Sort = "f_ReadDate ASC";
                this.ProgressBar1.Value = 0;
                this.dvConsumerStatistics = new DataView(this.ds.Tables["ConsumerStatistics"]);
                this.ProgressBar1.Maximum = Math.Max(0, this.dvConsumerStatistics.Count);
                for (num11 = 0; num11 <= (this.dvConsumerStatistics.Count - 1); num11++)
                {
                    this.ProgressBar1.Value = num11;
                    view2.RowFilter = str + " AND  f_ConsumerID = " + this.dvConsumerStatistics[num11]["f_ConsumerID"];
                    if (view2.Count > 0)
                    {
                        bool flag = false;
                        string str6 = "";
                        DateTime time2 = DateTime.Parse("2100-1-1");
                        string str7 = "";
                        int num13 = -1;
                        string str8 = "";
                        for (int j = 0; j <= (view2.Count - 1); j++)
                        {
                            DateTime time = (DateTime) view2[j]["f_ReadDate"];
                            flag = false;
                            index = 0;
                            while (index <= (numArray.Length - 1))
                            {
                                if (numArray[index] > 0)
                                {
                                    if (index < 3)
                                    {
                                        if ((string.Compare(time.ToString("HH:mm"), timeArray[index].ToString("HH:mm")) < 0) || (string.Compare(time.ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) > 0))
                                        {
                                            goto Label_0B7E;
                                        }
                                        flag = true;
                                        str6 = strArray[index];
                                        str8 = strArray2[index];
                                        break;
                                    }
                                    if (string.Compare(timeArray[index].ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) < 0)
                                    {
                                        if ((string.Compare(time.ToString("HH:mm"), timeArray[index].ToString("HH:mm")) < 0) || (string.Compare(time.ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) > 0))
                                        {
                                            goto Label_0B7E;
                                        }
                                        flag = true;
                                        str6 = strArray[index];
                                        str8 = strArray2[index];
                                        break;
                                    }
                                    if ((string.Compare(time.ToString("HH:mm"), timeArray[index].ToString("HH:mm")) >= 0) || (string.Compare(time.ToString("HH:mm"), timeArray2[index].ToString("HH:mm")) <= 0))
                                    {
                                        flag = true;
                                        str6 = strArray[index];
                                        str8 = strArray2[index];
                                        break;
                                    }
                                }
                            Label_0B7E:
                                index++;
                            }
                            if (flag)
                            {
                                bool flag2 = true;
                                TimeSpan span = Convert.ToDateTime(view2[j]["f_ReadDate"]).Subtract(time2);
                                if (((num9 == 1) && (str7 == str6)) && (Math.Abs(span.TotalHours) < 12.0))
                                {
                                    flag2 = false;
                                }
                                if (((num9 == 2) && (str7 == str6)) && ((Math.Abs(span.TotalSeconds) < num8) && (num13 == ((int) view2[j]["f_ReaderID"]))))
                                {
                                    flag2 = false;
                                }
                                if (flag2)
                                {
                                    str7 = str6;
                                    time2 = (DateTime) view2[j]["f_ReadDate"];
                                    num13 = (int) view2[j]["f_ReaderID"];
                                    DataRow row = table3.NewRow();
                                    row["f_RecID"] = view2[j]["f_RecID"];
                                    row["f_GroupName"] = view2[j]["f_GroupName"];
                                    row["f_ConsumerNO"] = view2[j]["f_ConsumerNO"];
                                    row["f_ConsumerID"] = view2[j]["f_ConsumerID"];
                                    row["f_ConsumerName"] = view2[j]["f_ConsumerName"];
                                    row["f_ReaderName"] = view2[j]["f_ReaderName"];
                                    row["f_ReaderID"] = view2[j]["f_ReaderID"];
                                    row["f_ReadDate"] = view2[j]["f_ReadDate"];
                                    row["f_MealName"] = str6;
                                    row["f_Cost"] = numArray2[index];
                                    if (!string.IsNullOrEmpty(str8))
                                    {
                                        view.RowFilter = string.Format("{0}>=0 AND f_ReaderID ={1} ", str8, row["f_ReaderID"].ToString());
                                        if (view.Count > 0)
                                        {
                                            row["f_Cost"] = decimal.Parse(wgTools.SetObjToStr(view[0][str8]));
                                        }
                                    }
                                    table3.Rows.Add(row);
                                }
                            }
                        }
                        table3.AcceptChanges();
                    }
                }
                this.dv = new DataView(this.ds.Tables["MealReport"]);
                int num15 = 0;
                decimal num16 = 0M;
                this.dv.RowFilter = "";
                if (this.dv.Count > 0)
                {
                    this.ProgressBar1.Value = 0;
                    this.ProgressBar1.Maximum = Math.Max(0, table.Rows.Count);
                    for (int k = 0; k <= (table.Rows.Count - 1); k++)
                    {
                        this.ProgressBar1.Value = k;
                        str9 = "f_ReaderID = " + ((int) table.Rows[k]["f_ReaderID"]);
                        this.dv.RowFilter = str9;
                        if (this.dv.Count > 0)
                        {
                            if (this.dv.Count > 0)
                            {
                                table.Rows[k]["f_CostCount"] = ((int) table.Rows[k]["f_CostCount"]) + this.dv.Count;
                                num15 += this.dv.Count;
                            }
                            for (int m = 0; m <= (this.dv.Count - 1); m++)
                            {
                                table.Rows[k]["f_CostTotal4Reader"] = ((decimal) table.Rows[k]["f_CostTotal4Reader"]) + ((decimal) this.dv[m]["f_Cost"]);
                                num16 += (decimal) this.dv[m]["f_Cost"];
                            }
                        }
                    }
                }
                DataRow row2 = table.NewRow();
                row2["f_ReaderName"] = CommonStr.strMealTotal;
                row2["f_CostCount"] = num15;
                row2["f_CostTotal4Reader"] = num16;
                table.Rows.Add(row2);
                table.AcceptChanges();
                this.dv.RowFilter = "";
                this.dv.RowFilter = "";
                if (this.dv.Count > 0)
                {
                    this.ProgressBar1.Value = 0;
                    this.ProgressBar1.Maximum = Math.Max(0, table2.Rows.Count);
                    for (num11 = 0; num11 <= (table2.Rows.Count - 1); num11++)
                    {
                        this.ProgressBar1.Value = num11;
                        str9 = "f_ConsumerID = " + table2.Rows[num11]["f_ConsumerID"];
                        this.dv.RowFilter = str9;
                        if (this.dv.Count > 0)
                        {
                            for (index = 0; index <= (numArray.Length - 1); index++)
                            {
                                if (numArray[index] > 0)
                                {
                                    this.dv.RowFilter = str9 + " AND f_MealName= " + wgTools.PrepareStr(strArray[index]);
                                    table2.Rows[num11][strArray2[index] + "Count"] = ((int) table2.Rows[num11][strArray2[index] + "Count"]) + this.dv.Count;
                                    table2.Rows[num11]["f_CostTotalCount"] = ((int) table2.Rows[num11]["f_CostTotalCount"]) + this.dv.Count;
                                    for (int n = 0; n <= (this.dv.Count - 1); n++)
                                    {
                                        table2.Rows[num11][strArray2[index]] = ((decimal) table2.Rows[num11][strArray2[index]]) + ((decimal) this.dv[n]["f_Cost"]);
                                        table2.Rows[num11]["f_CostTotal"] = ((decimal) table2.Rows[num11]["f_CostTotal"]) + ((decimal) this.dv[n]["f_Cost"]);
                                    }
                                }
                            }
                        }
                    }
                }
                table2.AcceptChanges();
                DataRow row3 = table2.NewRow();
                row3["f_GroupName"] = "==========";
                row3["f_ConsumerNO"] = "==========";
                row3["f_ConsumerName"] = CommonStr.strMealTotal;
                index = 4;
                while (index <= 13)
                {
                    row3[index] = 0;
                    index++;
                }
                for (num11 = 0; num11 <= (table2.Rows.Count - 1); num11++)
                {
                    this.ProgressBar1.Value = num11;
                    for (index = 4; index <= 13; index++)
                    {
                        if (row3[index].GetType().Name.ToString() == "Decimal")
                        {
                            row3[index] = ((decimal) row3[index]) + ((decimal) table2.Rows[num11][index]);
                        }
                        else
                        {
                            row3[index] = ((int) row3[index]) + ((int) table2.Rows[num11][index]);
                        }
                    }
                }
                table2.Rows.Add(row3);
                this.ProgressBar1.Value = 0;
                this.dgvSwipeRecords.AutoGenerateColumns = false;
                this.dgvSubtotal.AutoGenerateColumns = false;
                this.dgvStatistics.AutoGenerateColumns = false;
                this.dgvSwipeRecords.DataSource = this.ds.Tables["MealReport"];
                DataTable table5 = this.ds.Tables["MealReport"];
                for (num = 0; (num < this.dgvSwipeRecords.ColumnCount) && (num < table5.Columns.Count); num++)
                {
                    this.dgvSwipeRecords.Columns[num].DataPropertyName = table5.Columns[num].ColumnName;
                }
                wgAppConfig.setDisplayFormatDate(this.dgvSwipeRecords, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
                this.dgvSubtotal.DataSource = table;
                table5 = table;
                for (num = 0; (num < this.dgvSubtotal.ColumnCount) && (num < table5.Columns.Count); num++)
                {
                    this.dgvSubtotal.Columns[num].DataPropertyName = table5.Columns[num].ColumnName;
                }
                this.dgvStatistics.DataSource = table2;
                table5 = table2;
                for (num = 0; (num < this.dgvStatistics.ColumnCount) && (num < table5.Columns.Count); num++)
                {
                    this.dgvStatistics.Columns[num].DataPropertyName = table5.Columns[num].ColumnName;
                }
                this.dgvSwipeRecords.DefaultCellStyle.ForeColor = Color.Black;
                this.dgvSubtotal.DefaultCellStyle.ForeColor = Color.Black;
                this.dgvStatistics.DefaultCellStyle.ForeColor = Color.Black;
                if (this.dgvSwipeRecords.Rows.Count <= 0)
                {
                    this.btnPrint.Enabled = false;
                    this.btnExportToExcel.Enabled = false;
                    XMessageBox.Show(CommonStr.strMealNoRecords);
                }
                else
                {
                    this.btnPrint.Enabled = true;
                    this.btnExportToExcel.Enabled = true;
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            finally
            {
                this.btnCreateReport.Enabled = true;
                Cursor.Current = current;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.dgvSwipeRecords.DataSource == null)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                this.timer1.Enabled = false;
            }
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
                    (base.Control as DateTimePicker).Value = value;
                }
            }
        }
    }
}

