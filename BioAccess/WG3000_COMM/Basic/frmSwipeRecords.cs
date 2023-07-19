namespace WG3000_COMM.Basic
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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmSwipeRecords : frmBioAccess
    {
        private bool bLoadedFinished;
        private string dgvSql = "";
        private DataSet dsDefaultStyle = new DataSet("DGV_STILE");
        private int MaxRecord = 0x3e8;
        private int recIdMin;
        private int startRecordIndex;
        public string strFindOption = "";

        public frmSwipeRecords()
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as ToolStripButton).Text;
                name.label1.Text = CommonStr.strSelectMaxRecID;
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    int num;
                    if (int.TryParse(name.strNewName, out num))
                    {
                        string strSql = "DELETE FROM t_d_SwipeRecord  WHERE f_RecID < " + num.ToString();
                        int num2 = wgAppConfig.runUpdateSql(strSql);
                        wgAppConfig.wgLog("strsql =" + strSql);
                        wgAppConfig.wgLog("Deleted Records' Count =" + num2.ToString());
                        XMessageBox.Show(CommonStr.strDeletedSwipeRecordCount + num2.ToString());
                    }
                    else
                    {
                        XMessageBox.Show(CommonStr.strNumericWrong);
                    }
                }
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in this.dgvSwipeRecords.Columns)
            {
                if (column.Name.Equals("f_Desc"))
                {
                    foreach (DataGridViewRow row in (IEnumerable) this.dgvSwipeRecords.Rows)
                    {
                        DataGridViewCell cell = row.Cells[column.Index];
                        if ((cell.Value != null) && ((cell.Value as string) == " "))
                        {
                            string str = row.Cells[column.Index + 1].Value as string;
                            MjRec rec = new MjRec(str.PadLeft(0x24, '0'), false);
                            cell.Value = rec.GetDetailedRecord(null, 0);
                            if (rec.floorNo > 0)
                            {
                                this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", row.Cells["f_ReaderName"].Value, rec.floorNo);
                                if (this.dvFloor.Count >= 1)
                                {
                                    object obj2 = cell.Value;
                                    cell.Value = string.Concat(new object[] { obj2, " [", this.dvFloor[0]["f_floorFullName"].ToString(), "]" });
                                }
                            }
                        }
                    }
                }
            }
            wgAppConfig.exportToExcel(this.dgvSwipeRecords, this.Text);
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvSwipeRecords, this.Text);
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
            string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_ConsumerNO,  ";
            strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerName, ") + 
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
            string strsql = wgAppConfig.getSqlFindSwipeRecord(strBaseInfo, "t_d_SwipeRecord", strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
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
            string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_ConsumerNO,  ";
            strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerName, ") + 
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
            string strsql = wgAppConfig.getSqlFindSwipeRecord(strBaseInfo, "t_d_SwipeRecord", strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
            this.reloadData(strsql);
        }

        private void cboEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboEnd.SelectedIndex == 1)
            {
                this.dtpDateTo.Enabled = true;
            }
            else
            {
                this.dtpDateTo.Enabled = false;
            }
        }

        private void cboStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboStart.SelectedIndex == 1)
            {
                this.dtpDateFrom.Enabled = true;
            }
            else
            {
                this.dtpDateFrom.Enabled = false;
            }
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
                            MjRec rec = new MjRec(str2.PadLeft(0x30, '0'), false);
                            string verifMode = rec.getVerifModeString();
                            string detail = rec.GetDetailedRecord(null, 0);
                            e.Value = (rec.verifMode == 0) ? detail : "[" + verifMode + "] " + detail;
                            if (rec.floorNo > 0)
                            {
                                this.dvFloor.RowFilter = string.Format("f_ReaderName = '{0}' AND f_floorNO = {1} ", this.dgvSwipeRecords["f_ReaderName", e.RowIndex].Value, rec.floorNo);
                                if (this.dvFloor.Count >= 1)
                                {
                                    object obj2 = e.Value;
                                    e.Value = string.Concat(new object[] { obj2, " [", this.dvFloor[0]["f_floorFullName"].ToString(), "]" });
                                }
                            }
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
            this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
            this.saveDefaultStyle();
            this.loadStyle();
            this.loadFloorInfo();
            this.dtpDateFrom = new ToolStripDateTime();
            this.dtpDateTo = new ToolStripDateTime();
            this.dtpTimeFrom = new ToolStripDateTime();
            this.dtpTimeFrom.SetTimeFormat();
            this.dtpTimeFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            this.dtpTimeTo = new ToolStripDateTime();
            this.dtpTimeTo.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            this.dtpTimeTo.SetTimeFormat();
            this.toolStrip3.Items.Clear();
            this.toolStrip3.Items.AddRange(new ToolStripItem[] { this.toolStripLabel2, this.cboStart, this.dtpDateFrom, this.toolStripLabel3, this.cboEnd, this.dtpDateTo, this.toolStripSeparator1, this.toolStripLabel4, this.dtpTimeFrom, this.toolStripLabel5, this.dtpTimeTo });
            this.dtpDateFrom.BoxWidth = 120;
            this.dtpDateTo.BoxWidth = 120;
            this.dtpTimeFrom.BoxWidth = 0x3e;
            this.dtpTimeTo.BoxWidth = 0x3e;
            this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRomm(this.f_DepartmentName.HeaderText);
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            if (this.cboStart.Items.Count > 0)
            {
                this.cboStart.SelectedIndex = 0;
            }
            this.dtpDateFrom.Enabled = false;
            if (this.cboEnd.Items.Count > 0)
            {
                this.cboEnd.SelectedIndex = 0;
            }
            this.dtpDateTo.Enabled = false;
            this.dtpDateFrom.BoxWidth = 150;
            this.dtpDateTo.BoxWidth = 150;
            wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            if (!wgAppConfig.getParamValBoolByNO(0x8f))
            {
                Cursor.Current = Cursors.WaitCursor;
                this.timer1.Enabled = true;
                this.userControlFind1.btnQuery.PerformClick();
            }
        }

        private string getSqlOfDateTime()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getSqlOfDateTime_Acc();
            }
            string str = "";
            if (this.cboStart.SelectedIndex == 1)
            {
                str = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
            }
            if (this.cboEnd.SelectedIndex == 1)
            {
                if (str != "")
                {
                    str = str + " AND ";
                }
                str = str + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
            }
            if (this.dtpTimeFrom.Value.ToString("HH:mm") != "00:00")
            {
                if (str != "")
                {
                    str = str + " AND ";
                }
                if (this.dtpTimeFrom.Value.ToString("mm") == "00")
                {
                    str = str + " DATEPART(hh, [f_ReadDate]) >= " + this.dtpTimeFrom.Value.ToString("HH");
                }
                else
                {
                    string str2 = (str + " ( ") + " DATEPART(hh, [f_ReadDate]) > " + this.dtpTimeFrom.Value.ToString("HH");
                    str = (str2 + " OR (DATEPART(hh, [f_ReadDate]) = " + this.dtpTimeFrom.Value.ToString("HH") + " AND (DATEPART(mi, [f_ReadDate]) >= " + this.dtpTimeFrom.Value.ToString("mm") + "))") + " ) ";
                }
            }
            if (!(this.dtpTimeTo.Value.ToString("HH:mm") != "23:59"))
            {
                return str;
            }
            if (str != "")
            {
                str = str + " AND ";
            }
            if (this.dtpTimeTo.Value.ToString("mm") == "59")
            {
                return (str + " DATEPART(hh, [f_ReadDate]) <= " + this.dtpTimeTo.Value.ToString("HH"));
            }
            string str3 = (str + " ( ") + " DATEPART(hh, [f_ReadDate]) < " + this.dtpTimeTo.Value.ToString("HH");
            return ((str3 + " OR (DATEPART(hh, [f_ReadDate]) = " + this.dtpTimeTo.Value.ToString("HH") + " AND (DATEPART(mi, [f_ReadDate]) <= " + this.dtpTimeTo.Value.ToString("mm") + "))") + " ) ");
        }

        private string getSqlOfDateTime_Acc()
        {
            string str = "";
            if (this.cboStart.SelectedIndex == 1)
            {
                str = "  ([f_ReadDate]>= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 00:00:00") + ")";
            }
            if (this.cboEnd.SelectedIndex == 1)
            {
                if (str != "")
                {
                    str = str + " AND ";
                }
                str = str + "  ([f_ReadDate]<= " + wgTools.PrepareStr(this.dtpDateTo.Value.ToString(wgTools.YMDHMSFormat), true, "yyyy-MM-dd 23:59:59") + ")";
            }
            if (this.dtpTimeFrom.Value.ToString("HH:mm") != "00:00")
            {
                if (str != "")
                {
                    str = str + " AND ";
                }
                if (this.dtpTimeFrom.Value.ToString("mm") == "00")
                {
                    str = str + " Hour([f_ReadDate]) >= " + this.dtpTimeFrom.Value.ToString("HH");
                }
                else
                {
                    string str2 = (str + " ( ") + " HOUR( [f_ReadDate]) > " + this.dtpTimeFrom.Value.ToString("HH");
                    str = (str2 + " OR (HOUR([f_ReadDate]) = " + this.dtpTimeFrom.Value.ToString("HH") + " AND (Minute( [f_ReadDate]) >= " + this.dtpTimeFrom.Value.ToString("mm") + "))") + " ) ";
                }
            }
            if (!(this.dtpTimeTo.Value.ToString("HH:mm") != "23:59"))
            {
                return str;
            }
            if (str != "")
            {
                str = str + " AND ";
            }
            if (this.dtpTimeTo.Value.ToString("mm") == "59")
            {
                return (str + " HOUR( [f_ReadDate]) <= " + this.dtpTimeTo.Value.ToString("HH"));
            }
            string str3 = (str + " ( ") + " HOUR([f_ReadDate]) < " + this.dtpTimeTo.Value.ToString("HH");
            return ((str3 + " OR (Hour( [f_ReadDate]) = " + this.dtpTimeTo.Value.ToString("HH") + " AND (Minute( [f_ReadDate]) <= " + this.dtpTimeTo.Value.ToString("mm") + "))") + " ) ");
        }

        private void loadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.bLoadedFinished)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.startRecordIndex <= this.dgvSwipeRecords.Rows.Count)
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
                    wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvSwipeRecords.Rows.Count.ToString() + "#");
                }
            }
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

        private void loadFloorInfo()
        {
            string cmdText = " SELECT a.f_floorID,  c.f_DoorName + '.' + a.f_floorName as f_floorFullName , 0 as f_Selected, b.f_ZoneID, 0 as f_TimeProfile, b.f_ControllerID, b.f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_floor a, t_b_Controller b,t_b_Door c WHERE c.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID and a.f_DoorID = c.f_DoorID " + " ORDER BY  (  c.f_DoorName + '.' + a.f_floorName ) ";
            cmdText = "  SELECT t_b_Reader.f_ReaderName, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
            cmdText = (cmdText + "   t_b_Door.f_DoorName, " + "   t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName, t_b_Door.f_ControllerID  ") + "    FROM t_b_Floor , t_b_Door, t_b_Controller, t_b_Reader " + "   where t_b_Floor.f_DoorID = t_b_Door.f_DoorID and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID and t_b_Reader.f_ControllerID = t_b_Floor.f_ControllerID ";
            DataTable table = new DataTable();
            new DataView(table);
            this.dvFloor = new DataView(table);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(table);
                        }
                    }
                    goto Label_0110;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(table);
                    }
                }
            }
        Label_0110:
            try
            {
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string strNewName = "";
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = sender.ToString();
                name.label1.Text = CommonStr.strCardID;
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    strNewName = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                string str3 = "";
                bool flag = false;
                this.userControlFind1.txtFindCardID.Text = "";
                this.userControlFind1.txtFindName.Text = "";
                if ((this.dfrmFindOption != null) && this.dfrmFindOption.Visible)
                {
                    flag = true;
                    str3 = " (t_d_SwipeRecord.f_ReaderID IN ( " + this.dfrmFindOption.getStrSql() + " )) ";
                }
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string strBaseInfo = " SELECT t_d_SwipeRecord.f_RecID, t_d_SwipeRecord.f_ConsumerNO,  ";
                strBaseInfo = (strBaseInfo + "        t_b_Consumer.f_ConsumerName, ") + 
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
                    strTimeCon = strTimeCon + string.Format(" AND {0} ", str3);
                }
                if (strNewName.IndexOf("%") < 0)
                {
                    strNewName = string.Format("%{0}%", strNewName);
                }
                if (wgAppConfig.IsAccessDB)
                {
                    strTimeCon = strTimeCon + string.Format(" AND CSTR(t_d_SwipeRecord.f_ConsumerNO) like {0} ",
                        wgTools.PrepareStr(strNewName));
                }
                else
                {
                    strTimeCon = strTimeCon + string.Format(" AND t_d_SwipeRecord.f_ConsumerNO like {0} ", 
                        wgTools.PrepareStr(strNewName));
                }
                string strsql = wgAppConfig.getSqlFindSwipeRecord(strBaseInfo, "t_d_SwipeRecord", 
                    strTimeCon, groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                this.reloadData(strsql);
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

        private void dgvSwipeRecords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (dgvSwipeRecords.SelectedRows.Count > 0)
                {
                    int recID = (int)dgvSwipeRecords.SelectedRows[0].Cells[0].Value;
                    MjRec rec = new MjRec();
                    rec.loadPhotoFromDB(recID);
                    dfrmRecordDetail detail = new dfrmRecordDetail(
                        recID.ToString(),
                        dgvSwipeRecords.SelectedRows[0].Cells[1].Value.ToString(),
                        dgvSwipeRecords.SelectedRows[0].Cells[2].Value.ToString(),
                        dgvSwipeRecords.SelectedRows[0].Cells[3].Value.ToString(),
                        dgvSwipeRecords.SelectedRows[0].Cells[4].Value.ToString(),
                        dgvSwipeRecords.SelectedRows[0].Cells[5].Value.ToString(),
                        dgvSwipeRecords.SelectedRows[0].Cells[6].Value.ToString(),
                        rec.PhotoData);
                    detail.Owner = this;
                    detail.ShowDialog();
                }
            }
            catch
            {
            }
        }
    }
}

