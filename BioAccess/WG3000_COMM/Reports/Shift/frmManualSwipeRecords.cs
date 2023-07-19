namespace WG3000_COMM.Reports.Shift
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
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmManualSwipeRecords : frmBioAccess
    {
        private bool bLoadedFinished;
        private string dgvSql = "";
        private int MaxRecord = 0x3e8;
        private int recIdMax;
        private int startRecordIndex;
        private DataTable table;
        private UserControlFind userControlFind1;

        public frmManualSwipeRecords()
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmManualSwipeRecordsAdd add = new dfrmManualSwipeRecordsAdd())
            {
                add.ShowDialog();
                this.btnQuery_Click(sender, null);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.RowCount > 0)
            {
                int index;
                if (this.dgvMain.SelectedRows.Count <= 1)
                {
                    index = this.dgvMain.SelectedRows[0].Index;
                    if (XMessageBox.Show(this, CommonStr.strDelete + " " + this.dgvMain[0, index].Value.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else if (XMessageBox.Show(this, CommonStr.strDeleteSelected + " " + this.dgvMain.SelectedRows.Count.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                {
                    return;
                }
                int firstDisplayedScrollingRowIndex = this.dgvMain.FirstDisplayedScrollingRowIndex;
                if (this.dgvMain.SelectedRows.Count <= 1)
                {
                    index = this.dgvMain.SelectedRows[0].Index;
                    wgAppConfig.runUpdateSql(" DELETE FROM t_d_ManualCardRecord WHERE [f_ManualCardRecordID]= " + this.dgvMain[0, index].Value.ToString());
                }
                else
                {
                    foreach (DataGridViewRow row in this.dgvMain.SelectedRows)
                    {
                        wgAppConfig.runUpdateSql(" DELETE FROM t_d_ManualCardRecord WHERE [f_ManualCardRecordID]= " + row.Cells[0].Value.ToString());
                    }
                }
                this.btnQuery_Click(sender, null);
            }
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
            if (wgAppConfig.IsAccessDB)
            {
                this.btnQuery_Click_Acc(sender, e);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string strsql = wgAppConfig.getSqlFindNormal((" SELECT t_d_ManualCardRecord.f_ManualCardRecordID, t_b_Group.f_GroupName, " + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, f_ReadDate,") + " t_d_ManualCardRecord.f_Note, " + " t_b_Consumer.f_ConsumerID  ", "t_d_ManualCardRecord", this.getSqlOfDateTime("t_d_ManualCardRecord.f_ReadDate"), groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                this.reloadData(strsql);
            }
        }

        private void btnQuery_Click_Acc(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string strsql = wgAppConfig.getSqlFindNormal((" SELECT t_d_ManualCardRecord.f_ManualCardRecordID, t_b_Group.f_GroupName, " + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, f_ReadDate,") + " t_d_ManualCardRecord.f_Note, " + " t_b_Consumer.f_ConsumerID  ", "t_d_ManualCardRecord", this.getSqlOfDateTime("t_d_ManualCardRecord.f_ReadDate"), groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
            this.reloadData(strsql);
        }

        private void btnTypeSetup_Click(object sender, EventArgs e)
        {
            using (dfrmHolidayType type = new dfrmHolidayType())
            {
                type.ShowDialog(this);
                this.btnQuery_Click(sender, null);
            }
        }

        private void dgvMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex >= 5) && (e.ColumnIndex < this.dgvMain.Columns.Count))
            {
                object obj1 = e.Value;
                DataGridViewCell cell = this.dgvMain[e.ColumnIndex, e.RowIndex];
                string str = this.dgvMain[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    if (str == "0")
                    {
                        str = "*";
                        e.Value = str;
                        cell.Value = e.Value;
                    }
                    else if (str == "-1")
                    {
                        e.Value = "-";
                        cell.Value = e.Value;
                    }
                    else if (str == "-2")
                    {
                        e.Value = DBNull.Value;
                        cell.Value = e.Value;
                    }
                }
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
                    for (int i = 0; i < this.dgvMain.ColumnCount; i++)
                    {
                        this.dgvMain.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
                        this.dgvMain.Columns[i].Name = dt.Columns[i].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_ReadDate", wgTools.DisplayFormat_DateYMDHMSWeek);
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
            this.userControlFind1.toolStripLabel2.Visible = false;
            this.userControlFind1.txtFindCardID.Visible = false;
            this.dtpDateFrom.Enabled = true;
            this.dtpDateTo.Enabled = true;
            this.loadStyle();
            Cursor.Current = Cursors.WaitCursor;
            this.dtpDateTo.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-12-31"));
            this.dtpDateFrom.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01"));
            this.dtpDateFrom.BoxWidth = 150;
            this.dtpDateTo.BoxWidth = 150;
            wgAppConfig.setDisplayFormatDate(this.dtpDateFrom.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpDateTo.DateTimeControl, wgTools.DisplayFormat_DateYMDWeek);
            this.Refresh();
            this.userControlFind1.btnQuery.PerformClick();
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
                strSql = strSql + string.Format(" AND f_ManualCardRecordID > {0:d}", this.recIdMax);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE f_ManualCardRecordID > {0:d}", this.recIdMax);
            }
            strSql = strSql + " ORDER BY f_ManualCardRecordID ";
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

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuManualCardRecord";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnDelete.Visible = false;
                this.btnEdit.Visible = false;
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

