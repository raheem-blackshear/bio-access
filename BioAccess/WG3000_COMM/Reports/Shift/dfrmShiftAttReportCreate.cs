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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftAttReportCreate : frmBioAccess
    {
        private comShift comShiftWork = new comShift();
        private comShift_Acc comShiftWork_Acc = new comShift_Acc();
        private DataTable dtAttReport;
        private DataTable dtAttStatistic;
        public string groupName = "";
        public string strConsumerSql = "";
        public int totalConsumer;
        private DataTable dtShiftWorkSchedule;

        public dfrmShiftAttReportCreate()
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
            if (this.comShiftWork != null)
            {
                this.comShiftWork.bStopCreate = true;
            }
            if (this.comShiftWork_Acc != null)
            {
                this.comShiftWork_Acc.bStopCreate = true;
            }
            this.backgroundWorker1.CancelAsync();
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void dfrmShiftAttReportCreate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                if (this.comShiftWork != null)
                {
                    this.comShiftWork.bStopCreate = true;
                }
                if (this.comShiftWork_Acc != null)
                {
                    this.comShiftWork_Acc.bStopCreate = true;
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
                    num = this.comShiftWork.shift_work_schedule_cleardb();
                }
                if (num == 0)
                {
                    num = this.comShiftWork.shift_AttReport_cleardb();
                }
                if (num == 0)
                {
                    num = this.comShiftWork.shift_AttStatistic_cleardb();
                }
                DateTime startDate = dateStart;
                DateTime endDate = dateEnd;
                if (num == 0)
                {
                    this.comShiftWork.getAttendenceParam();
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
                        bool flag = wgAppConfig.getParamValBoolByNO(0x71);
                        while (reader.Read())
                        {
                            if (this.comShiftWork.bStopCreate)
                            {
                                return num;
                            }
                            currentConsumerID = (int) reader["f_ConsumerID"];
                            percentProgress++;
                            this.backgroundWorker1.ReportProgress(percentProgress);
                            if (flag && (((byte) reader["f_ShiftEnabled"]) > 0))
                            {
                                num = this.ShiftOtherDeal(currentConsumerID, this.comShiftWork, startDate, endDate, ref bNotArrange);
                                if (num == 0)
                                {
                                    continue;
                                }
                                if ((bNotArrange & 1) > 0)
                                {
                                    XMessageBox.Show(string.Concat(new object[] { reader["f_ConsumerName"], "\r\n\r\n", CommonStr.strNotArrange, "\r\n", CommonStr.strReArrange }));
                                }
                                else if ((bNotArrange & 2) > 0)
                                {
                                    XMessageBox.Show(string.Concat(new object[] { reader["f_ConsumerName"], "\r\n\r\n", CommonStr.strNotArrange, "\r\n", CommonStr.strReArrange }));
                                }
                                else if ((bNotArrange & 4) > 0)
                                {
                                    XMessageBox.Show(string.Concat(new object[] { reader["f_ConsumerName"], "\r\n\r\n", CommonStr.strArrangeShiftIDNotExisted, "\r\n", CommonStr.strReArrange }));
                                }
                                break;
                            }
                            using (comCreateAttendenceData data = new comCreateAttendenceData())
                            {
                                data.startDateTime = dateStart;
                                data.endDateTime = dateEnd;
                                data.strConsumerSql = strSql + "AND f_ConsumerID = " + currentConsumerID.ToString();
                                data.make();
                                continue;
                            }
                        }
                        reader.Close();
                        if (num == 0)
                        {
                            num = this.comShiftWork.logCreateReport(startDate, endDate, this.groupName, this.totalConsumer.ToString());
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
                    num = this.comShiftWork_Acc.shift_work_schedule_cleardb();
                }
                if (num == 0)
                {
                    num = this.comShiftWork_Acc.shift_AttReport_cleardb();
                }
                if (num == 0)
                {
                    num = this.comShiftWork_Acc.shift_AttStatistic_cleardb();
                }
                DateTime startDate = dateStart;
                DateTime endDate = dateEnd;
                if (num == 0)
                {
                    this.comShiftWork_Acc.getAttendenceParam();
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
                        bool flag = wgAppConfig.getParamValBoolByNO(0x71);
                        while (reader.Read())
                        {
                            if (this.comShiftWork_Acc.bStopCreate)
                            {
                                return num;
                            }
                            currentConsumerID = (int) reader["f_ConsumerID"];
                            percentProgress++;
                            this.backgroundWorker1.ReportProgress(percentProgress);
                            if (flag && (((byte) reader["f_ShiftEnabled"]) > 0))
                            {
                                num = this.ShiftOtherDeal_Acc(currentConsumerID, this.comShiftWork_Acc, startDate, endDate, ref bNotArrange);
                                if (num == 0)
                                {
                                    continue;
                                }
                                if ((bNotArrange & 1) > 0)
                                {
                                    XMessageBox.Show(string.Concat(new object[] { reader["f_ConsumerName"], "\r\n\r\n", CommonStr.strNotArrange, "\r\n", CommonStr.strReArrange }));
                                }
                                else if ((bNotArrange & 2) > 0)
                                {
                                    XMessageBox.Show(string.Concat(new object[] { reader["f_ConsumerName"], "\r\n\r\n", CommonStr.strNotArrange, "\r\n", CommonStr.strReArrange }));
                                }
                                else if ((bNotArrange & 4) > 0)
                                {
                                    XMessageBox.Show(string.Concat(new object[] { reader["f_ConsumerName"], "\r\n\r\n", CommonStr.strArrangeShiftIDNotExisted, "\r\n", CommonStr.strReArrange }));
                                }
                                break;
                            }
                            using (comCreateAttendenceData_Acc acc = new comCreateAttendenceData_Acc())
                            {
                                acc.startDateTime = dateStart;
                                acc.endDateTime = dateEnd;
                                acc.strConsumerSql = strSql + "AND f_ConsumerID = " + currentConsumerID.ToString();
                                acc.make();
                                continue;
                            }
                        }
                        reader.Close();
                        if (num == 0)
                        {
                            num = this.comShiftWork_Acc.logCreateReport(startDate, endDate, this.groupName, this.totalConsumer.ToString());
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

        private int ShiftOtherDeal(int currentConsumerID, comShift comShiftWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
        {
            int num = 0;
            int num2 = 1;
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtShiftWorkSchedule == null)
                {
                    num = comShiftWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
                }
                else
                {
                    this.dtShiftWorkSchedule.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
            }
            wgTools.WriteLine(num2++.ToString());
            if ((num == 0) && (bNotArrange != 0))
            {
                int num1 = bNotArrange & 1;
                int num19 = bNotArrange & 2;
                int num20 = bNotArrange & 4;
                num = -1;
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_updatebyManualReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_updatebyLeave(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtAttReport == null)
                {
                    num = comShiftWork.shift_AttReport_Create(out this.dtAttReport);
                }
                else
                {
                    this.dtAttReport.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttReport_writetodb(this.dtAttReport);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtAttStatistic == null)
                {
                    num = comShiftWork.shift_AttStatistic_Create(out this.dtAttStatistic);
                }
                else
                {
                    this.dtAttStatistic.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttStatistic_Fill(this.dtAttStatistic, this.dtAttReport);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttStatistic_writetodb(this.dtAttStatistic);
            }
            wgTools.WriteLine(num2++.ToString());
            wgTools.WriteLine(num2++.ToString());
            return num;
        }

        private int ShiftOtherDeal_Acc(int currentConsumerID, comShift_Acc comShiftWork, DateTime startDate, DateTime endDate, ref int bNotArrange)
        {
            int num = 0;
            int num2 = 1;
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtShiftWorkSchedule == null)
                {
                    num = comShiftWork.shift_work_schedule_create(out this.dtShiftWorkSchedule);
                }
                else
                {
                    this.dtShiftWorkSchedule.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_fill(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate, ref bNotArrange);
            }
            wgTools.WriteLine(num2++.ToString());
            if ((num == 0) && (bNotArrange != 0))
            {
                int num1 = bNotArrange & 1;
                int num19 = bNotArrange & 2;
                int num20 = bNotArrange & 4;
                num = -1;
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_updatebyReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_updatebyManualReadcard(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_updatebyLeave(currentConsumerID, this.dtShiftWorkSchedule, startDate, endDate);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_work_schedule_analyst(this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtAttReport == null)
                {
                    num = comShiftWork.shift_AttReport_Create(out this.dtAttReport);
                }
                else
                {
                    this.dtAttReport.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttReport_Fill(this.dtAttReport, this.dtShiftWorkSchedule);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttReport_writetodb(this.dtAttReport);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                if (this.dtAttStatistic == null)
                {
                    num = comShiftWork.shift_AttStatistic_Create(out this.dtAttStatistic);
                }
                else
                {
                    this.dtAttStatistic.Rows.Clear();
                }
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttStatistic_Fill(this.dtAttStatistic, this.dtAttReport);
            }
            wgTools.WriteLine(num2++.ToString());
            if (num == 0)
            {
                num = comShiftWork.shift_AttStatistic_writetodb(this.dtAttStatistic);
            }
            wgTools.WriteLine(num2++.ToString());
            wgTools.WriteLine(num2++.ToString());
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

