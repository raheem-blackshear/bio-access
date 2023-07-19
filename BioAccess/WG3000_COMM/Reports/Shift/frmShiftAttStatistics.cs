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

    public partial class frmShiftAttStatistics : frmBioAccess
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

        public frmShiftAttStatistics()
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
                    string strsql = wgAppConfig.getSqlFindNormal((((((((((((((((((((((" SELECT t_d_shift_AttStatistic.f_RecID, t_b_Group.f_GroupName, " + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, ") + "CASE WHEN CONVERT(decimal(10,1),[f_DayShouldWork]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_DayShouldWork]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_DayShouldWork]        ) END) ELSE ' ' END  [f_DayShouldWork]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_DayRealWork]         ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_DayRealWork]         ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_DayRealWork]          ) END) ELSE ' ' END  [f_DayRealWork]         ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_LateMinutes]         ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LateMinutes]         ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LateMinutes]          ) END) ELSE ' ' END  [f_LateMinutes]         ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_LateCount]           ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LateCount]           ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LateCount]            ) END) ELSE ' ' END  [f_LateCount]           ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyMinutes]   ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyMinutes]   ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LeaveEarlyMinutes]    ) END) ELSE ' ' END  [f_LeaveEarlyMinutes]   ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyCount]     ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_LeaveEarlyCount]     ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_LeaveEarlyCount]      ) END) ELSE ' ' END  [f_LeaveEarlyCount]     ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_OvertimeHours]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_OvertimeHours]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_OvertimeHours]        ) END) ELSE ' ' END  [f_OvertimeHours]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_AbsenceDays]         ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_AbsenceDays]         ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_AbsenceDays]          ) END) ELSE ' ' END  [f_AbsenceDays]         ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_NotReadCardCount]    ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_NotReadCardCount]    ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_NotReadCardCount]     ) END) ELSE ' ' END  [f_NotReadCardCount]    ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_ManualReadTimesCount]) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_ManualReadTimesCount]) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_ManualReadTimesCount] ) END) ELSE ' ' END  [f_ManualReadTimesCount],  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType1]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType1]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType1]         ) END) ELSE ' ' END  [f_SpecialType1]        ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType2]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType2]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType2]         ) END) ELSE ' ' END  [f_SpecialType2]        ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType3]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType3]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType3]         ) END) ELSE ' ' END  [f_SpecialType3]        ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType4]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType4]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType4]         ) END) ELSE ' ' END  [f_SpecialType4]        ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType5]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType5]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType5]         ) END) ELSE ' ' END  [f_SpecialType5]        ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType6]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType6]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType6]         ) END) ELSE ' ' END  [f_SpecialType6]        ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType7]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType7]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType7]         ) END) ELSE ' ' END  [f_SpecialType7]        ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType8]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType8]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType8]         ) END) ELSE ' ' END  [f_SpecialType8]        ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType9]        ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType9]        ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType9]         ) END) ELSE ' ' END  [f_SpecialType9]        ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType10]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType10]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType10]        ) END) ELSE ' ' END  [f_SpecialType10]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType11]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType11]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType11]        ) END) ELSE ' ' END  [f_SpecialType11]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType12]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType12]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType12]        ) END) ELSE ' ' END  [f_SpecialType12]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType13]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType13]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType13]        ) END) ELSE ' ' END  [f_SpecialType13]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType14]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType14]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType14]        ) END) ELSE ' ' END  [f_SpecialType14]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType15]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType15]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType15]        ) END) ELSE ' ' END  [f_SpecialType15]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType16]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType16]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType16]        ) END) ELSE ' ' END  [f_SpecialType16]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType17]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType17]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType17]        ) END) ELSE ' ' END  [f_SpecialType17]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType18]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType18]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType18]        ) END) ELSE ' ' END  [f_SpecialType18]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType19]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType19]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType19]        ) END) ELSE ' ' END  [f_SpecialType19]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType20]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType20]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType20]        ) END) ELSE ' ' END  [f_SpecialType20]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType21]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType21]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType21]        ) END) ELSE ' ' END  [f_SpecialType21]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType22]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType22]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType22]        ) END) ELSE ' ' END  [f_SpecialType22]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType23]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType23]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType23]        ) END) ELSE ' ' END  [f_SpecialType23]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType24]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType24]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType24]        ) END) ELSE ' ' END  [f_SpecialType24]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType25]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType25]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType25]        ) END) ELSE ' ' END  [f_SpecialType25]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType26]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType26]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType26]        ) END) ELSE ' ' END  [f_SpecialType26]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType27]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType27]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType27]        ) END) ELSE ' ' END  [f_SpecialType27]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType28]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType28]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType28]        ) END) ELSE ' ' END  [f_SpecialType28]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType29]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType29]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType29]        ) END) ELSE ' ' END  [f_SpecialType29]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType30]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType30]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType30]        ) END) ELSE ' ' END  [f_SpecialType30]       ,  ") + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType31]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType31]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType31]        ) END) ELSE ' ' END  [f_SpecialType31]       ,  " + "CASE WHEN CONVERT(decimal(10,1),[f_SpecialType32]       ) >0 THEN (CASE WHEN CONVERT(decimal(10,1),[f_SpecialType32]       ) <1 THEN  '0.5' ELSE  CONVERT(varchar(6),[f_SpecialType32]        ) END) ELSE ' ' END  [f_SpecialType32]          ", "t_d_shift_AttStatistic", "", groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                    string str4 = "";
                    if (sender == this.cmdQueryNormalShift)
                    {
                        str4 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =0) ";
                    }
                    else if (sender == this.cmdQueryOtherShift)
                    {
                        str4 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =1) ";
                    }
                    if (!string.IsNullOrEmpty(str4))
                    {
                        if (strsql.IndexOf(" WHERE ") > 0)
                        {
                            strsql = strsql + str4;
                        }
                        else
                        {
                            strsql = strsql + " WHERE (1>0) " + str4;
                        }
                    }
                    this.reloadData(strsql);
                }
            }
        }

        private void btnQuery_Click_Acc(object sender, EventArgs e)
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
                string strsql = wgAppConfig.getSqlFindNormal((((((((((((((((((((((" SELECT t_d_shift_AttStatistic.f_RecID, t_b_Group.f_GroupName, " + "       t_b_Consumer.f_ConsumerNO AS f_ConsumerNO, " + " t_b_Consumer.f_ConsumerName AS f_ConsumerName, ") + "IIF((IIF(ISNULL([f_DayShouldWork]),0,[f_DayShouldWork])       ) >0 ,CSTR(t_d_shift_AttStatistic.[f_DayShouldWork]        ) , ' ') AS  [f_DayShouldWork]       ,  " + "IIF((IIF(ISNULL([f_DayRealWork]         ),0,[f_DayRealWork] )         ) >0 ,IIF((IIF(ISNULL([f_DayRealWork]         ),0,[f_DayRealWork]          )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_DayRealWork]          ) ) , ' ') AS  [f_DayRealWork]         ,  ") + "IIF((IIF(ISNULL([f_LateMinutes]         ),0,[f_LateMinutes]          )) >0 ,IIF((IIF(ISNULL([f_LateMinutes]         ),0,[f_LateMinutes]          )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LateMinutes]          ) ) , ' ') AS  [f_LateMinutes]         ,  " + "IIF((IIF(ISNULL([f_LateCount]           ),0,[f_LateCount]            )) >0 ,IIF((IIF(ISNULL([f_LateCount]           ),0,[f_LateCount]            )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LateCount]            ) ) , ' ') AS  [f_LateCount]           ,  ") + "IIF((IIF(ISNULL([f_LeaveEarlyMinutes]   ),0,[f_LeaveEarlyMinutes]    )) >0 ,IIF((IIF(ISNULL([f_LeaveEarlyMinutes]   ),0,[f_LeaveEarlyMinutes]    )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LeaveEarlyMinutes]    ) ) , ' ') AS  [f_LeaveEarlyMinutes]   ,  " + "IIF((IIF(ISNULL([f_LeaveEarlyCount]     ),0,[f_LeaveEarlyCount]      )) >0 ,IIF((IIF(ISNULL([f_LeaveEarlyCount]     ),0,[f_LeaveEarlyCount]      )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_LeaveEarlyCount]      ) ) , ' ') AS  [f_LeaveEarlyCount]     ,  ") + "IIF((IIF(ISNULL([f_OvertimeHours]       ),0,[f_OvertimeHours]        )) >0 ,IIF((IIF(ISNULL([f_OvertimeHours]       ),0,[f_OvertimeHours]        )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_OvertimeHours]        ) ) , ' ') AS  [f_OvertimeHours]       ,  " + "IIF((IIF(ISNULL([f_AbsenceDays]         ),0,[f_AbsenceDays]          )) >0 ,IIF((IIF(ISNULL([f_AbsenceDays]         ),0,[f_AbsenceDays]          )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_AbsenceDays]          ) ) , ' ') AS  [f_AbsenceDays]         ,  ") + "IIF((IIF(ISNULL([f_NotReadCardCount]    ),0,[f_NotReadCardCount]     )) >0 ,IIF((IIF(ISNULL([f_NotReadCardCount]    ),0,[f_NotReadCardCount]     )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_NotReadCardCount]     ) ) , ' ') AS  [f_NotReadCardCount]    ,  " + "IIF((IIF(ISNULL([f_ManualReadTimesCount]),0,[f_ManualReadTimesCount] )) >0 ,IIF((IIF(ISNULL([f_ManualReadTimesCount]),0,[f_ManualReadTimesCount] )) <1 , '0.5', CSTR(t_d_shift_AttStatistic.[f_ManualReadTimesCount] ) ) , ' ') AS  [f_ManualReadTimesCount],  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType1]    ),0, [f_SpecialType1] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType1]    ),0, [f_SpecialType1] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType1]         ) )) , ' ') AS  [f_SpecialType1]        ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType2]    ),0, [f_SpecialType2] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType2]    ),0, [f_SpecialType2] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType2]         ) )) , ' ') AS  [f_SpecialType2]        ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType3]    ),0, [f_SpecialType3] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType3]    ),0, [f_SpecialType3] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType3]         ) )) , ' ') AS  [f_SpecialType3]        ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType4]    ),0, [f_SpecialType4] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType4]    ),0, [f_SpecialType4] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType4]         ) )) , ' ') AS  [f_SpecialType4]        ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType5]    ),0, [f_SpecialType5] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType5]    ),0, [f_SpecialType5] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType5]         ) )) , ' ') AS  [f_SpecialType5]        ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType6]    ),0, [f_SpecialType6] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType6]    ),0, [f_SpecialType6] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType6]         ) )) , ' ') AS  [f_SpecialType6]        ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType7]    ),0, [f_SpecialType7] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType7]    ),0, [f_SpecialType7] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType7]         ) )) , ' ') AS  [f_SpecialType7]        ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType8]    ),0, [f_SpecialType8] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType8]    ),0, [f_SpecialType8] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType8]         ) )) , ' ') AS  [f_SpecialType8]        ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType9]    ),0, [f_SpecialType9] )    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType9]    ),0, [f_SpecialType9] )    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType9]         ) )) , ' ') AS  [f_SpecialType9]        ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType10]   ),0, [f_SpecialType10])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType10]   ),0, [f_SpecialType10])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType10]        ) )) , ' ') AS  [f_SpecialType10]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType11]   ),0, [f_SpecialType11])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType11]   ),0, [f_SpecialType11])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType11]        ) )) , ' ') AS  [f_SpecialType11]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType12]   ),0, [f_SpecialType12])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType12]   ),0, [f_SpecialType12])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType12]        ) )) , ' ') AS  [f_SpecialType12]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType13]   ),0, [f_SpecialType13])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType13]   ),0, [f_SpecialType13])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType13]        ) )) , ' ') AS  [f_SpecialType13]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType14]   ),0, [f_SpecialType14])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType14]   ),0, [f_SpecialType14])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType14]        ) )) , ' ') AS  [f_SpecialType14]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType15]   ),0, [f_SpecialType15])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType15]   ),0, [f_SpecialType15])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType15]        ) )) , ' ') AS  [f_SpecialType15]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType16]   ),0, [f_SpecialType16])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType16]   ),0, [f_SpecialType16])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType16]        ) )) , ' ') AS  [f_SpecialType16]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType17]   ),0, [f_SpecialType17])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType17]   ),0, [f_SpecialType17])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType17]        ) )) , ' ') AS  [f_SpecialType17]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType18]   ),0, [f_SpecialType18])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType18]   ),0, [f_SpecialType18])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType18]        ) )) , ' ') AS  [f_SpecialType18]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType19]   ),0, [f_SpecialType19])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType19]   ),0, [f_SpecialType19])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType19]        ) )) , ' ') AS  [f_SpecialType19]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType20]   ),0, [f_SpecialType20])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType20]   ),0, [f_SpecialType20])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType20]        ) )) , ' ') AS  [f_SpecialType20]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType21]   ),0, [f_SpecialType21])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType21]   ),0, [f_SpecialType21])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType21]        ) )) , ' ') AS  [f_SpecialType21]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType22]   ),0, [f_SpecialType22])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType22]   ),0, [f_SpecialType22])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType22]        ) )) , ' ') AS  [f_SpecialType22]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType23]   ),0, [f_SpecialType23])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType23]   ),0, [f_SpecialType23])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType23]        ) )) , ' ') AS  [f_SpecialType23]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType24]   ),0, [f_SpecialType24])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType24]   ),0, [f_SpecialType24])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType24]        ) )) , ' ') AS  [f_SpecialType24]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType25]   ),0, [f_SpecialType25])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType25]   ),0, [f_SpecialType25])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType25]        ) )) , ' ') AS  [f_SpecialType25]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType26]   ),0, [f_SpecialType26])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType26]   ),0, [f_SpecialType26])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType26]        ) )) , ' ') AS  [f_SpecialType26]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType27]   ),0, [f_SpecialType27])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType27]   ),0, [f_SpecialType27])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType27]        ) )) , ' ') AS  [f_SpecialType27]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType28]   ),0, [f_SpecialType28])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType28]   ),0, [f_SpecialType28])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType28]        ) )) , ' ') AS  [f_SpecialType28]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType29]   ),0, [f_SpecialType29])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType29]   ),0, [f_SpecialType29])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType29]        ) )) , ' ') AS  [f_SpecialType29]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType30]   ),0, [f_SpecialType30])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType30]   ),0, [f_SpecialType30])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType30]        ) )) , ' ') AS  [f_SpecialType30]       ,  ") + "IIF(CDbl(IIF(ISNULL([f_SpecialType31]   ),0, [f_SpecialType31])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType31]   ),0, [f_SpecialType31])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType31]        ) )) , ' ') AS  [f_SpecialType31]       ,  " + "IIF(CDbl(IIF(ISNULL([f_SpecialType32]   ),0, [f_SpecialType32])    ) >0 ,(IIF(CDbl(IIF(ISNULL([f_SpecialType32]   ),0, [f_SpecialType32])    ) <1 , '0.5', (t_d_shift_AttStatistic.[f_SpecialType32]        ) )) , ' ') AS  [f_SpecialType32]          ", "t_d_shift_AttStatistic", "", groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                string str4 = "";
                if (sender == this.cmdQueryNormalShift)
                {
                    str4 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =0) ";
                }
                else if (sender == this.cmdQueryOtherShift)
                {
                    str4 = " AND t_d_shift_AttStatistic.f_ConsumerID IN (SELECT aaa.f_ConsumerID FROM t_b_Consumer aaa WHERE aaa.f_ShiftEnabled =1) ";
                }
                if (!string.IsNullOrEmpty(str4))
                {
                    if (strsql.IndexOf(" WHERE ") > 0)
                    {
                        strsql = strsql + str4;
                    }
                    else
                    {
                        strsql = strsql + " WHERE (1>0) " + str4;
                    }
                }
                this.reloadData(strsql);
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
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.cmdQueryNormalShift.Visible = true;
                this.cmdQueryOtherShift.Visible = true;
            }
            this.Refresh();
            this.userControlFind1.btnQuery.PerformClick();
        }

        public string getlocalizedHolidayType(string type)
        {
            string strBusinessTrip = "";
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    return type;
                }
                strBusinessTrip = type;
                if (((type == "出差") || (type == "出差")) || (type == "Business Trip"))
                {
                    strBusinessTrip = CommonStr.strBusinessTrip;
                }
                if (((type == "病假") || (type == "病假")) || (type == "Sick Leave"))
                {
                    strBusinessTrip = CommonStr.strSickLeave;
                }
                if ((!(type == "事假") && !(type == "事假")) && !(type == "Private Leave"))
                {
                    return strBusinessTrip;
                }
                return CommonStr.strPrivateLeave;
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
            return strBusinessTrip;
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
            string cmdText = "SELECT * FROM t_a_Attendence WHERE [f_NO]=15 ";
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

        private string getSqlOfDateTime(string colNameOfDate)
        {
            return " 1>0 ";
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
                strSql = strSql + string.Format(" AND t_d_shift_AttStatistic.f_RecID > {0:d}", this.recIdMax);
            }
            else
            {
                strSql = strSql + string.Format(" WHERE t_d_shift_AttStatistic.f_RecID > {0:d}", this.recIdMax);
            }
            strSql = strSql + " ORDER BY t_d_shift_AttStatistic.f_RecID ";
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
            if (wgAppConfig.IsAccessDB)
            {
                this.loadStyle_Acc();
            }
            else
            {
                if (this.OnlyTwoTimesSpecial())
                {
                    this.dgvMain.Columns[10].HeaderText = CommonStr.strWorkHour;
                }
                if (this.OnlyOnDutySpecial())
                {
                    this.dgvMain.Columns[8].Visible = false;
                    this.dgvMain.Columns[9].Visible = false;
                    this.dgvMain.Columns[10].Visible = false;
                    this.dgvMain.Columns[12].Visible = false;
                }
                this.dgvMain.AutoGenerateColumns = false;
                string cmdText = " SELECT * FROM t_a_HolidayType ORDER BY f_NO ";
                int num = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        string str2;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            str2 = "f_SpecialType" + (num + 1);
                            this.dc = new DataGridViewTextBoxColumn();
                            this.dc.HeaderText = this.getlocalizedHolidayType(reader["f_HolidayType"].ToString()) + "\r\n(" + CommonStr.strDay + ")";
                            this.dc.DataPropertyName = str2;
                            this.dc.Width = 0x2d;
                            this.dc.ReadOnly = true;
                            this.dc.Visible = true;
                            this.dgvMain.Columns.Add(this.dc);
                            num++;
                        }
                        reader.Close();
                        while (num < 0x20)
                        {
                            str2 = "f_SpecialType" + (num + 1);
                            this.dc = new DataGridViewTextBoxColumn();
                            this.dc.HeaderText = str2;
                            this.dc.DataPropertyName = str2;
                            this.dc.Width = 0x2d;
                            this.dc.ReadOnly = true;
                            this.dc.Visible = false;
                            this.dgvMain.Columns.Add(this.dc);
                            num++;
                        }
                        wgAppConfig.ReadGVStyle(this, this.dgvMain);
                    }
                }
            }
        }

        private void loadStyle_Acc()
        {
            if (this.OnlyTwoTimesSpecial())
            {
                this.dgvMain.Columns[10].HeaderText = CommonStr.strWorkHour;
            }
            if (this.OnlyOnDutySpecial())
            {
                this.dgvMain.Columns[8].Visible = false;
                this.dgvMain.Columns[9].Visible = false;
                this.dgvMain.Columns[10].Visible = false;
                this.dgvMain.Columns[12].Visible = false;
            }
            this.dgvMain.AutoGenerateColumns = false;
            string cmdText = " SELECT * FROM t_a_HolidayType ORDER BY f_NO ";
            int num = 0;
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    string str2;
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        str2 = "f_SpecialType" + (num + 1);
                        this.dc = new DataGridViewTextBoxColumn();
                        this.dc.HeaderText = this.getlocalizedHolidayType(reader["f_HolidayType"].ToString()) + "\r\n(" + CommonStr.strDay + ")";
                        this.dc.DataPropertyName = str2;
                        this.dc.Width = 0x2d;
                        this.dc.ReadOnly = true;
                        this.dc.Visible = true;
                        this.dgvMain.Columns.Add(this.dc);
                        num++;
                    }
                    reader.Close();
                    while (num < 0x20)
                    {
                        str2 = "f_SpecialType" + (num + 1);
                        this.dc = new DataGridViewTextBoxColumn();
                        this.dc.HeaderText = str2;
                        this.dc.DataPropertyName = str2;
                        this.dc.Width = 0x2d;
                        this.dc.ReadOnly = true;
                        this.dc.Visible = false;
                        this.dgvMain.Columns.Add(this.dc);
                        num++;
                    }
                    wgAppConfig.ReadGVStyle(this, this.dgvMain);
                }
            }
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

