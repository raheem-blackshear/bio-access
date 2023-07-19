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

    public partial class frmShiftOtherData : frmBioAccess
    {
        private bool bLoadedFinished;
        private string dgvSql = "";
        private int MaxRecord = 0x3e8;
        private int recIdMax;
        private int startRecordIndex;
        private DataTable table;
        private UserControlFind userControlFind1;

        public frmShiftOtherData()
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
            using (dfrmAutoShift shift = new dfrmAutoShift())
            {
                shift.ShowDialog();
                this.btnQuery_Click(sender, null);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string str = string.Format("{0}", this.btnClear.Text);
            if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int num;
                if (wgAppConfig.IsAccessDB)
                {
                    using (comShift_Acc acc = new comShift_Acc())
                    {
                        num = acc.shift_arrange_delete(0, DateTime.Parse("2000-1-1"), DateTime.Parse("2050-12-31"));
                        if (num == 0)
                        {
                            XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.btnQuery_Click(sender, null);
                        }
                        else
                        {
                            XMessageBox.Show(this, acc.errDesc(num) + "\r\n\r\n" + acc.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        return;
                    }
                }
                using (comShift shift = new comShift())
                {
                    num = shift.shift_arrange_delete(0, DateTime.Parse("2000-1-1"), DateTime.Parse("2050-12-31"));
                    if (num == 0)
                    {
                        XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.btnQuery_Click(sender, null);
                    }
                    else
                    {
                        XMessageBox.Show(this, shift.errDesc(num) + "\r\n\r\n" + shift.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (dfrmShiftDelete delete = new dfrmShiftDelete())
            {
                delete.ShowDialog(this);
                this.btnQuery_Click(sender, null);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (((this.dgvMain.SelectedCells.Count > 0) && (this.dgvMain.SelectedCells[0].ColumnIndex >= 5)) && (this.dgvMain.SelectedCells[0].RowIndex >= 0))
                    {
                        DataGridViewCell cell = this.dgvMain.SelectedCells[0];
                        DataGridViewRow row = this.dgvMain.Rows[this.dgvMain.SelectedCells[0].RowIndex];
                        if (cell.Value != DBNull.Value)
                        {
                            using (dfrmShiftEdit edit = new dfrmShiftEdit())
                            {
                                if (edit.ShowDialog() == DialogResult.OK)
                                {
                                    int shiftid = edit.shiftid;
                                    if (wgAppConfig.IsAccessDB)
                                    {
                                        using (comShift_Acc acc = new comShift_Acc())
                                        {
                                            DateTime dateShift = Convert.ToDateTime(string.Concat(new object[] { row.Cells["f_DateYM"].Value, "-", cell.ColumnIndex - 4, " 12:00:00" }));
                                            long num2 = acc.shift_arrange_update(Convert.ToInt32(row.Cells["f_ConsumerID"].Value), dateShift, shiftid);
                                            if (num2 == 0L)
                                            {
                                                if (shiftid == 0)
                                                {
                                                    cell.Value = "*";
                                                }
                                                else
                                                {
                                                    cell.Value = shiftid;
                                                }
                                            }
                                            return;
                                        }
                                    }
                                    using (comShift shift = new comShift())
                                    {
                                        DateTime time2 = Convert.ToDateTime(string.Concat(new object[] { row.Cells["f_DateYM"].Value, "-", cell.ColumnIndex - 4, " 12:00:00" }));
                                        long num3 = shift.shift_arrange_update(Convert.ToInt32(row.Cells["f_ConsumerID"].Value), time2, shiftid);
                                        if (num3 == 0L)
                                        {
                                            if (shiftid == 0)
                                            {
                                                cell.Value = "*";
                                            }
                                            else
                                            {
                                                cell.Value = shiftid;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        XMessageBox.Show(this, CommonStr.strNeedToSelectDate, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            finally
            {
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
            string str3;
            Cursor.Current = Cursors.WaitCursor;
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            if (wgAppConfig.IsAccessDB)
            {
                str3 = " SELECT t_d_ShiftData.f_RecID, t_b_Group.f_GroupName, ";
                str3 = (str3 + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, ") + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, " + " t_d_ShiftData.f_DateYM, ";
                for (int i = 1; i < 0x1f; i++)
                {
                    str3 = str3 + string.Format(" CSTR(t_d_ShiftData.f_ShiftID_{0:d2}) as f_ShiftID_{0:d2}, ", i);
                }
                str3 = str3 + "CSTR(t_d_ShiftData.f_ShiftID_31) as f_ShiftID_31 " + " ,t_b_Consumer.f_ConsumerID  ";
            }
            else
            {
                str3 = " SELECT t_d_ShiftData.f_RecID, t_b_Group.f_GroupName, ";
                str3 = (str3 + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, ") + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, " + " t_d_ShiftData.f_DateYM, ";
                for (int j = 1; j < 0x1f; j++)
                {
                    str3 = str3 + string.Format(" CONVERT(nvarchar(3),  t_d_ShiftData.f_ShiftID_{0:d2}) as f_ShiftID_{0:d2}, ", j);
                }
                str3 = str3 + "CONVERT(nvarchar(3),  t_d_ShiftData.f_ShiftID_31) as f_ShiftID_31 " + " ,t_b_Consumer.f_ConsumerID  ";
            }
            string strsql = wgAppConfig.getSqlFindNormal(str3, "t_d_ShiftData", this.getSqlOfDateTime("t_d_ShiftData.f_DateYM"), groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
            this.reloadData(strsql);
        }

        private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.btnEdit.Visible)
            {
                this.btnEdit.PerformClick();
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
                        cell.ReadOnly = true;
                        cell.Style.BackColor = SystemPens.InactiveBorder.Color;
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
                    for (int i = 0; i < dt.Columns.Count; i++)
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

        private void frmShiftOtherData_Load(object sender, EventArgs e)
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
            str = "  (" + colNameOfDate + " >= " + wgTools.PrepareStr(this.dtpDateFrom.Value.ToString("yyyy-MM")) + ")";
            if (str != "")
            {
                str = str + " AND ";
            }
            string str2 = str;
            return (str2 + "  (" + colNameOfDate + " < " + wgTools.PrepareStr(this.dtpDateTo.Value.AddMonths(1).ToString("yyyy-MM")) + ")");
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
                strSql = strSql + string.Format(" AND t_d_ShiftData.f_RecID > {0:d}", this.recIdMax);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE t_d_ShiftData.f_RecID > {0:d}", this.recIdMax);
            }
            strSql = strSql + " ORDER BY t_d_ShiftData.f_RecID ";
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

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuShiftArrange";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnClear.Visible = false;
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

