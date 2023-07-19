namespace WG3000_COMM.Basic
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

    public partial class dfrmLogQuery : frmBioAccess
    {
        private BackgroundWorker backgroundWorker1;
        private bool bLoadedFinished;
        private string dgvSql = "";
        private int MaxRecord = 0x3e8;
        private int recIdMin;
        private int startRecordIndex;

        public dfrmLogQuery()
        {
            this.InitializeComponent();
            this.backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.btnClose.DialogResult = DialogResult.Cancel;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void dfrmLogQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmLogQuery_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmLogQuery_Load(object sender, EventArgs e)
        {
            string strsql = " SELECT f_RecID,f_LogDateTime,  f_EventType, f_EventDesc From t_s_wgLog  ";
            this.dgvMain.AutoGenerateColumns = false;
            this.reloadData(strsql);
        }

        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvMain_Click(object sender, EventArgs e)
        {
        }

        private void dgvMain_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvMain.SelectedRows.Count <= 0)
                {
                    if (this.dgvMain.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    int rowIndex = this.dgvMain.SelectedCells[0].RowIndex;
                }
                else
                {
                    int index = this.dgvMain.SelectedRows[0].Index;
                }
                int num = 0;
                DataGridView dgvMain = this.dgvMain;
                if (dgvMain.Rows.Count > 0)
                {
                    num = dgvMain.CurrentCell.RowIndex;
                }
                string data = dgvMain.Rows[num].Cells["f_EventDesc"].Value.ToString();
                Clipboard.SetDataObject(data, false);
                XMessageBox.Show(data, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
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
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_LogDateTime", wgTools.DisplayFormat_DateYMDHMSWeek);
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

        private DataTable loadDataRecords(int startIndex, int maxRecords, string strSql)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("load LogQuery Start");
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
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            this.dt = new DataTable();
                            wgTools.WriteLine("da.Fill start");
                            adapter.Fill(this.dt);
                            if (this.dt.Rows.Count > 0)
                            {
                                this.recIdMin = int.Parse(this.dt.Rows[this.dt.Rows.Count - 1][0].ToString());
                            }
                        }
                    }
                    goto Label_0236;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(strSql, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        this.dt = new DataTable();
                        wgTools.WriteLine("da.Fill start");
                        adapter2.Fill(this.dt);
                        if (this.dt.Rows.Count > 0)
                        {
                            this.recIdMin = int.Parse(this.dt.Rows[this.dt.Rows.Count - 1][0].ToString());
                        }
                    }
                }
            }
        Label_0236:
            wgTools.WriteLine("da.Fill End " + startIndex.ToString());
            wgTools.WriteLine(this.Text + "  load LogQuery End");
            Cursor.Current = Cursors.Default;
            return this.dt;
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
    }
}

