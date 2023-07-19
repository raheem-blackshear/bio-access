namespace WG3000_COMM.ExtendFunc.Patrol
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
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmPatrolStatistics : frmBioAccess
    {
        private bool bLoadedFinished;
        private bool bLogCreateReport;
        private string dgvSql = "";
        private DataSet dsDefaultStyle = new DataSet("DGV_STILE");
        private int MaxRecord = 0x3e8;
        private int recIdMax;
        private int startRecordIndex;
        private DataTable table;
        private UserControlFindSecond userControlFind1;

        public frmPatrolStatistics()
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcelSpecial(ref this.dgvMain, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvMain, this.Text);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.getLogCreateReport();
            if (!this.bLogCreateReport)
            {
                XMessageBox.Show(this, CommonStr.strCreateInAdvance, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string str3 = " SELECT t_d_PatrolStatistic.f_RecID, t_b_Group.f_GroupName, ";
                str3 = str3 + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, ";
                if (wgAppConfig.IsAccessDB)
                {
                    str3 = (str3 + "IIF((IIF(ISNULL([f_TotalNormal]           ),0,[f_TotalNormal]            )) >0 ,IIF((IIF(ISNULL([f_TotalNormal]           ),0,[f_TotalNormal]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalNormal]            ) ) , ' ') AS  [f_TotalNormal]           ,  " + "IIF((IIF(ISNULL([f_TotalEarly]           ),0,[f_TotalEarly]            )) >0 ,IIF((IIF(ISNULL([f_TotalEarly]           ),0,[f_TotalEarly]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalEarly]            ) ) , ' ') AS  [f_TotalEarly]           ,  ") + "IIF((IIF(ISNULL([f_TotalLate]           ),0,[f_TotalLate]            )) >0 ,IIF((IIF(ISNULL([f_TotalLate]           ),0,[f_TotalLate]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalLate]            ) ) , ' ') AS  [f_TotalLate]           ,  " + "IIF((IIF(ISNULL([f_TotalAbsence]           ),0,[f_TotalAbsence]            )) >0 ,IIF((IIF(ISNULL([f_TotalAbsence]           ),0,[f_TotalAbsence]            )) <1 , '0.5', CSTR(t_d_PatrolStatistic.[f_TotalAbsence]            ) ) , ' ') AS  [f_TotalAbsence]           ,  ";
                }
                else
                {
                    str3 = (str3 + "CASE WHEN CONVERT(decimal(10,1),[f_TotalNormal]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalNormal]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalNormal]      ) END) ELSE ' ' END  [f_TotalNormal]     ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_TotalEarly]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalEarly]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalEarly]      ) END) ELSE ' ' END  [f_TotalEarly]     ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_TotalLate]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalLate]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalLate]      ) END) ELSE ' ' END  [f_TotalLate]     ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_TotalAbsence]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_TotalAbsence]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_TotalAbsence]      ) END) ELSE ' ' END  [f_TotalAbsence]     ,  ";
                }
                string strsql = wgAppConfig.getSqlFindNormal(str3 + " f_PatrolDateStart, " + " f_PatrolDateEnd ", "t_d_PatrolStatistic", "", groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                this.reloadData(strsql);
            }
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
                    }
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

        private void frmShiftAttStatistics_Load(object sender, EventArgs e)
        {
            this.f_DepartmentName.HeaderText = wgAppConfig.ReplaceFloorRomm(this.f_DepartmentName.HeaderText);
            this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.userControlFind1.toolStripLabel2.Visible = false;
            this.userControlFind1.txtFindCardID.Visible = false;
            this.saveDefaultStyle();
            this.loadStyle();
            Cursor.Current = Cursors.WaitCursor;
            this.getLogCreateReport();
            bool bLogCreateReport = this.bLogCreateReport;
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
                string cmdText = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (new SqlDataAdapter(command))
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
        }

        public void getLogCreateReport_Acc()
        {
            this.bLogCreateReport = false;
            string cmdText = "SELECT * FROM  t_a_SystemParam WHERE [f_NO]=29 ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (new OleDbDataAdapter(command))
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
                strSql = strSql + string.Format(" AND t_d_PatrolStatistic.f_RecID > {0:d}", this.recIdMax);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE t_d_PatrolStatistic.f_RecID > {0:d}", this.recIdMax);
            }
            strSql = strSql + " ORDER BY t_d_PatrolStatistic.f_RecID ";
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
                    goto Label_0186;
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
        Label_0186:
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

        private void loadStyle()
        {
            this.dgvMain.AutoGenerateColumns = false;
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
            this.loadDefaultStyle();
            this.loadStyle();
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

