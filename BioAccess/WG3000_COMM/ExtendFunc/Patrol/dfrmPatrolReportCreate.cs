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

    public partial class dfrmPatrolReportCreate : frmBioAccess
    {
        private comPatrol comPatrolWork = new comPatrol();
        private comPatrol_Acc comPatrolWork_Acc = new comPatrol_Acc();
        private DataTable dtAttReport;
        private DataTable dtShiftWorkSchedule;
        public string groupName = "";
        public string strConsumerSql = "";
        public int totalConsumer;

        public dfrmPatrolReportCreate()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            DateTime dateStart = (DateTime) ((object[]) e.Argument)[0];
            DateTime dateEnd = (DateTime) ((object[]) e.Argument)[1];
            string strSql = (string) ((object[]) e.Argument)[2];
            e.Result = this.ReportCreate(dateStart, dateEnd, strSql);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            this.lblInfo.Text = e.ProgressPercentage.ToString();
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
                wgAppRunInfo.raiseAppRunInfoLoadNums(CommonStr.strSuccessfully);
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (this.comPatrolWork != null)
            {
                this.comPatrolWork.bStopCreate = true;
            }
            if (this.comPatrolWork_Acc != null)
            {
                this.comPatrolWork_Acc.bStopCreate = true;
            }
            this.backgroundWorker1.CancelAsync();
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void dfrmShiftAttReportCreate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                if (this.comPatrolWork != null)
                {
                    this.comPatrolWork.bStopCreate = true;
                }
                if (this.comPatrolWork_Acc != null)
                {
                    this.comPatrolWork_Acc.bStopCreate = true;
                }
                this.backgroundWorker1.CancelAsync();
            }
        }

        private void dfrmShiftAttReportCreate_Load(object sender, EventArgs e)
        {
            this.progressBar1.Maximum = this.totalConsumer;
            this.label1.Text = ("[ " + this.totalConsumer.ToString() + " ]").PadLeft("[ 200000 ]".Length, ' ');
            this.StartCreate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private int ReportCreate(DateTime dateStart, DateTime dateEnd, string strSql)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.ReportCreate_Acc(dateStart, dateEnd, strSql);
            }
            int num = -1;
            num = 0;
            try
            {
                if (num == 0)
                {
                    num = this.comPatrolWork.shift_work_schedule_cleardb();
                }
                if (num == 0)
                {
                    num = this.comPatrolWork.shift_AttStatistic_cleardb();
                }
                DateTime startDate = dateStart;
                DateTime endDate = dateEnd;
                if (num == 0)
                {
                    this.comPatrolWork.getPatrolParam();
                }
                int bNotArrange = 0;
                int percentProgress = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(strSql, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        int currentConsumerID = 0;
                        Cursor.Current = Cursors.WaitCursor;
                        while (reader.Read())
                        {
                            if (this.comPatrolWork_Acc.bStopCreate)
                            {
                                return num;
                            }
                            currentConsumerID = (int) reader["f_ConsumerID"];
                            percentProgress++;
                            this.backgroundWorker1.ReportProgress(percentProgress);
                            num = this.ShiftOtherDeal(currentConsumerID, this.comPatrolWork, startDate, endDate, ref bNotArrange);
                        }
                        reader.Close();
                        if (num == 0)
                        {
                            num = this.comPatrolWork.logCreateReport(startDate, endDate, this.groupName, this.totalConsumer.ToString());
                        }
                    }
                    return num;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        private int ReportCreate_Acc(DateTime dateStart, DateTime dateEnd, string strSql)
        {
            int num = -1;
            num = 0;
            try
            {
                if (num == 0)
                {
                    num = this.comPatrolWork_Acc.shift_work_schedule_cleardb();
                }
                if (num == 0)
                {
                    num = this.comPatrolWork_Acc.shift_AttStatistic_cleardb();
                }
                DateTime startDate = dateStart;
                DateTime endDate = dateEnd;
                if (num == 0)
                {
                    this.comPatrolWork_Acc.getPatrolParam();
                }
                int bNotArrange = 0;
                int percentProgress = 0;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        int currentConsumerID = 0;
                        Cursor.Current = Cursors.WaitCursor;
                        while (reader.Read())
                        {
                            if (this.comPatrolWork_Acc.bStopCreate)
                            {
                                return num;
                            }
                            currentConsumerID = (int) reader["f_ConsumerID"];
                            percentProgress++;
                            this.backgroundWorker1.ReportProgress(percentProgress);
                            num = this.ShiftOtherDeal_Acc(currentConsumerID, this.comPatrolWork_Acc, startDate, endDate, ref bNotArrange);
                        }
                        reader.Close();
                        if (num == 0)
                        {
                            num = this.comPatrolWork_Acc.logCreateReport(startDate, endDate, this.groupName, this.totalConsumer.ToString());
                        }
                    }
                    return num;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        private int ShiftNornalDeal(int ConsumerID, DateTime dateStart, DateTime dateEnd)
        {
            return -1;
        }

        private int ShiftOtherDeal(int currentConsumerID, comPatrol comPatrolWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
        {
            int num = 0;
            int num2 = 1;
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtShiftWorkSchedule == null)
                {
                    num = comPatrolWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
                }
                else
                {
                    this.dtShiftWorkSchedule.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
            }
            wgTools.WriteLine(num2++.ToString());
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_writetodb(this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtAttReport == null)
                {
                    num = comPatrolWork.shift_AttReport_Create(out this.dtAttReport);
                }
                else
                {
                    this.dtAttReport.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_AttReport_writetodb(this.dtAttReport);
            }
            return num;
        }

        private int ShiftOtherDeal_Acc(int currentConsumerID, comPatrol_Acc comPatrolWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
        {
            int num = 0;
            int num2 = 1;
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtShiftWorkSchedule == null)
                {
                    num = comPatrolWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
                }
                else
                {
                    this.dtShiftWorkSchedule.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
            }
            wgTools.WriteLine(num2++.ToString());
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_work_schedule_writetodb(this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtAttReport == null)
                {
                    num = comPatrolWork.shift_AttReport_Create(out this.dtAttReport);
                }
                else
                {
                    this.dtAttReport.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comPatrolWork.shift_AttReport_writetodb(this.dtAttReport);
            }
            return num;
        }

        private void StartCreate()
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.dtBegin, this.dtEnd, this.strConsumerSql });
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan span = DateTime.Now.Subtract(this.startTime);
                string str = string.Concat(new object[] { span.Hours, ":", span.Minutes, ":", span.Seconds });
                this.lblRuntime.Text = str;
            }
            catch
            {
            }
        }
    }
}

