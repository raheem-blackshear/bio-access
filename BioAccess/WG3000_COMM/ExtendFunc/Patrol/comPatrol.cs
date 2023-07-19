namespace WG3000_COMM.ExtendFunc.Patrol
{
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;

    public class comPatrol : Component
    {
        public bool bStopCreate;
        private SqlCommand cmd;
        private SqlConnection cn;
        private IContainer components;
        private SqlDataAdapter daCardRecord;
        private DataColumn dc;
        private DataSet dsAtt;
        private DataTable dtCardRecord;
        private DataTable dtReport;
        private DataTable dtShiftWork;
        private DataTable dtValidCardRecord;
        private const int EER_SQL_RUNFAIL = -999;
        private const int ERR_FAIL = -1;
        private const int ERR_NONE = 0;
        public string errInfo;
        private short tNotPatrolTimeout;
        private int tOnTimePatrolTimeout;
        private int tPatrolEventDescAbsence;
        private int tPatrolEventDescEarly;
        private int tPatrolEventDescLate;
        private int tPatrolEventDescNormal;
        private int tTwoReadMintime;

        public comPatrol()
        {
            this.errInfo = "";
            this.tTwoReadMintime = 60;
            this.tPatrolEventDescNormal = 1;
            this.tPatrolEventDescEarly = 2;
            this.tPatrolEventDescLate = 3;
            this.tPatrolEventDescAbsence = 4;
            this.tNotPatrolTimeout = 30;
            this.tOnTimePatrolTimeout = 10;
            this.dsAtt = new DataSet();
            this.InitializeComponent();
        }

        public comPatrol(IContainer container)
        {
            this.errInfo = "";
            this.tTwoReadMintime = 60;
            this.tPatrolEventDescNormal = 1;
            this.tPatrolEventDescEarly = 2;
            this.tPatrolEventDescLate = 3;
            this.tPatrolEventDescAbsence = 4;
            this.tNotPatrolTimeout = 30;
            this.tOnTimePatrolTimeout = 10;
            this.dsAtt = new DataSet();
            container.Add(this);
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string errDesc(int errno)
        {
            switch (errno)
            {
                case -999:
                    return CommonStr.strSqlRunFail;

                case -1:
                    return CommonStr.strFailed;
            }
            return CommonStr.strUnknown;
        }

        public void getPatrolParam()
        {
            this.tOnTimePatrolTimeout = short.Parse(wgAppConfig.getSystemParamByNO(0x1c));
            this.tNotPatrolTimeout = short.Parse(wgAppConfig.getSystemParamByNO(0x1b));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public int logCreateReport(DateTime startDateTime, DateTime endDateTime, string groupName, string totalConsumer)
        {
            int num = -1;
            try
            {
                string str4 = CommonStr.strPatrolCreateLog + "  [" + CommonStr.strOperateDate + DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + "]";
                string str5 = str4 + ";  " + CommonStr.strFrom + Strings.Format(startDateTime, wgTools.DisplayFormat_DateYMD) + CommonStr.strTo + Strings.Format(endDateTime, wgTools.DisplayFormat_DateYMD);
                string str = str5 + ";   " + wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartment) + ":" + groupName + "            " + CommonStr.strUser + " (" + totalConsumer + ")";
                string str2 = Strings.Format(startDateTime, "yyyy-MM-dd") + "--" + Strings.Format(endDateTime, "yyyy-MM-dd");
                wgAppConfig.runUpdateSql((("UPDATE t_a_SystemParam " + " SET [f_Value]=" + this.PrepareStr(str2)) + " , [f_Notes] = " + this.PrepareStr(str)) + " WHERE [f_NO]= 29 ");
                num = 0;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        private string PrepareStr(object obj)
        {
            return wgTools.PrepareStr(obj);
        }

        private string PrepareStr(object obj, bool bDate, string dateFormat)
        {
            return wgTools.PrepareStr(obj, bDate, dateFormat);
        }

        private string SetObjToStr(object obj)
        {
            return wgTools.SetObjToStr(obj);
        }

        public int shift_arrange_delete(int consumerId, DateTime dateStart, DateTime dateEnd)
        {
            string str;
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            bool flag = false;
            object[] objArray = new object[0x25];
            this.errInfo = "";
            int num = -1;
            if (consumerId == 0)
            {
                str = " DELETE FROM t_d_PatrolPlanData ";
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new SqlCommand(str, this.cn);
                if (this.cmd.ExecuteNonQuery() < 0)
                {
                    num = -999;
                    this.errInfo = str;
                    return num;
                }
                return 0;
            }
            DateTime time2 = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time3 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time2 <= time3)
            {
                try
                {
                    string str2 = "";
                    DateTime expression = time2;
                    do
                    {
                        if (str2 != Strings.Format(expression, "yyyy-MM"))
                        {
                            str2 = Strings.Format(expression, "yyyy-MM");
                            str = " SELECT * FROM t_d_PatrolPlanData ";
                            str = (str + " WHERE f_ConsumerID = " + consumerId) + " AND f_DateYM = " + this.PrepareStr(str2);
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new SqlCommand(str, this.cn);
                            SqlDataReader reader = this.cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                flag = true;
                                for (int i = 0; i <= (reader.FieldCount - 1); i++)
                                {
                                    objArray[i] = reader[i];
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                            reader.Close();
                        }
                        do
                        {
                            objArray[2 + expression.Day] = -1;
                            expression = expression.AddDays(1.0);
                        }
                        while (!(str2 != Strings.Format(expression, "yyyy-MM")) && (expression <= time3));
                        if (flag)
                        {
                            int num4;
                            bool flag2 = true;
                            for (num4 = 1; num4 <= 0x1f; num4++)
                            {
                                if (Convert.ToInt32(objArray[2 + num4]) > -1)
                                {
                                    flag2 = false;
                                    break;
                                }
                            }
                            if (flag2)
                            {
                                str = "  DELETE FROM t_d_PatrolPlanData ";
                                str = str + " WHERE f_RecID = " + objArray[0];
                            }
                            else
                            {
                                str = "  UPDATE t_d_PatrolPlanData SET ";
                                num4 = 1;
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, " f_RouteID_", num4.ToString().PadLeft(2, '0'), " = ", objArray[2 + num4] });
                                for (num4 = 2; num4 <= 0x1f; num4++)
                                {
                                    object obj3 = str;
                                    str = string.Concat(new object[] { obj3, " , f_RouteID_", num4.ToString().PadLeft(2, '0'), " = ", objArray[2 + num4] });
                                }
                                str = ((str + " , f_LogDate  = " + this.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss")) + " , f_Notes  = " + this.PrepareStr("")) + " WHERE f_RecID = " + objArray[0];
                            }
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new SqlCommand(str, this.cn);
                            if (this.cmd.ExecuteNonQuery() <= 0)
                            {
                                num = -999;
                                this.errInfo = str;
                                return num;
                            }
                        }
                    }
                    while (expression <= time3);
                    num = 0;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                finally
                {
                    if (this.cn.State == ConnectionState.Open)
                    {
                        this.cn.Close();
                    }
                }
            }
            return num;
        }

        public int shift_arrange_update(int consumerId, DateTime dateShift, int shiftID)
        {
            int num = 0;
            int[] shiftRule = new int[1];
            try
            {
                shiftRule[0] = shiftID;
                num = this.shift_arrangeByRule(consumerId, dateShift, dateShift, 1, shiftRule);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_arrangeByRule(int consumerId, DateTime dateStart, DateTime dateEnd, int ruleLen, int[] shiftRule)
        {
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            bool flag = false;
            object[] objArray = new object[0x25];
            this.errInfo = "";
            int num = -1;
            DateTime time2 = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time3 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time2 <= time3)
            {
                try
                {
                    string str2 = "";
                    int index = 0;
                    DateTime expression = time2;
                    do
                    {
                        string str;
                        if (str2 != Strings.Format(expression, "yyyy-MM"))
                        {
                            str2 = Strings.Format(expression, "yyyy-MM");
                            str = " SELECT * FROM t_d_PatrolPlanData ";
                            str = (str + " WHERE f_ConsumerID = " + consumerId) + " AND f_DateYM = " + this.PrepareStr(str2);
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new SqlCommand(str, this.cn);
                            SqlDataReader reader = this.cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                flag = true;
                                for (int i = 0; i <= (reader.FieldCount - 1); i++)
                                {
                                    objArray[i] = reader[i];
                                }
                            }
                            else
                            {
                                int num6 = DateTime.DaysInMonth(expression.Year, expression.Month);
                                flag = false;
                                objArray[1] = consumerId;
                                objArray[2] = this.PrepareStr(str2);
                                for (int j = 1; j <= 0x1f; j++)
                                {
                                    if (j <= num6)
                                    {
                                        objArray[j + 2] = -1;
                                    }
                                    else
                                    {
                                        objArray[j + 2] = -2;
                                    }
                                }
                            }
                            reader.Close();
                        }
                        do
                        {
                            objArray[2 + expression.Day] = shiftRule[index];
                            index++;
                            if (index >= ruleLen)
                            {
                                index = 0;
                            }
                            expression = expression.AddDays(1.0);
                        }
                        while (!(str2 != Strings.Format(expression, "yyyy-MM")) && (expression <= time3));
                        if (flag)
                        {
                            str = "  UPDATE t_d_PatrolPlanData SET ";
                            int num7 = 1;
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, " f_RouteID_", num7.ToString().PadLeft(2, '0'), " = ", objArray[2 + num7] });
                            for (num7 = 2; num7 <= 0x1f; num7++)
                            {
                                object obj3 = str;
                                str = string.Concat(new object[] { obj3, " , f_RouteID_", num7.ToString().PadLeft(2, '0'), " = ", objArray[2 + num7] });
                            }
                            str = ((str + " , f_LogDate  = " + this.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss")) + " , f_Notes  = " + this.PrepareStr("")) + " WHERE f_RecID = " + objArray[0];
                        }
                        else
                        {
                            int num8;
                            str = "  INSERT INTO t_d_PatrolPlanData  ";
                            str = str + " ( f_ConsumerID , f_DateYM  ";
                            for (num8 = 1; num8 <= 0x1f; num8++)
                            {
                                str = str + " , f_RouteID_" + num8.ToString().PadLeft(2, '0');
                            }
                            str = ((str + " , f_Notes   " + " ) ") + " Values ( " + objArray[1]) + " , " + objArray[2];
                            for (num8 = 1; num8 <= 0x1f; num8++)
                            {
                                str = str + " , " + objArray[2 + num8];
                            }
                            str = (str + "  , " + this.PrepareStr("")) + " ) ";
                        }
                        if (this.cn.State != ConnectionState.Open)
                        {
                            this.cn.Open();
                        }
                        this.cmd = new SqlCommand(str, this.cn);
                        if (this.cmd.ExecuteNonQuery() <= 0)
                        {
                            num = -999;
                            this.errInfo = str;
                            return num;
                        }
                    }
                    while (expression <= time3);
                    num = 0;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                finally
                {
                    if (this.cn.State == ConnectionState.Open)
                    {
                        this.cn.Close();
                    }
                }
            }
            return num;
        }

        public int shift_AttReport_Create(out DataTable dtAttReport)
        {
            this.dtReport = new DataTable("t_d_AttReport");
            int num = -1;
            dtAttReport = null;
            try
            {
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ConsumerID";
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_PatrolDateStart";
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_PatrolDateEnd";
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_TotalLate";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LateMinutes";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_TotalEarly";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Decimal");
                this.dc.ColumnName = "f_TotalAbsence";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_TotalNormal";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                dtAttReport = this.dtReport.Copy();
                num = 0;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_AttReport_Fill(DataTable dtAttReport, DataTable dtShiftWorkSchedule)
        {
            this.errInfo = "";
            int num = -1;
            try
            {
                DataRow row = null;
                DataRow row2 = null;
                int num3 = -1;
                for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                {
                    if (this.bStopCreate)
                    {
                        return num;
                    }
                    if (row == null)
                    {
                        row = dtAttReport.NewRow();
                    }
                    row2 = dtShiftWorkSchedule.Rows[i];
                    row["f_ConsumerID"] = Convert.ToInt32(row2["f_ConsumerID"]);
                    if (this.SetObjToStr(row2["f_EventDesc"]) == this.tPatrolEventDescEarly.ToString())
                    {
                        row["f_TotalEarly"] = Convert.ToInt32(row["f_TotalEarly"]) + 1;
                    }
                    else if (this.SetObjToStr(row2["f_EventDesc"]) == this.tPatrolEventDescLate.ToString())
                    {
                        row["f_TotalLate"] = Convert.ToInt32(row["f_TotalLate"]) + 1;
                    }
                    else if (this.SetObjToStr(row2["f_EventDesc"]) == this.tPatrolEventDescAbsence.ToString())
                    {
                        row["f_TotalAbsence"] = Convert.ToInt32(row["f_TotalAbsence"]) + 1;
                    }
                    else if (this.SetObjToStr(row2["f_EventDesc"]) == this.tPatrolEventDescNormal.ToString())
                    {
                        row["f_TotalNormal"] = Convert.ToInt32(row["f_TotalNormal"]) + 1;
                    }
                    if (num3 < 0)
                    {
                        num3 = (int) row2["f_ConsumerID"];
                    }
                    if (num3 != ((int) row2["f_ConsumerID"]))
                    {
                        dtAttReport.Rows.Add(row);
                        row = dtAttReport.NewRow();
                        num3 = (int) row2["f_ConsumerID"];
                    }
                }
                if (num3 > 0)
                {
                    dtAttReport.Rows.Add(row);
                    row = dtAttReport.NewRow();
                }
                num = 0;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_AttReport_writetodb(DataTable dtAttReport)
        {
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            string str = "";
            this.cmd = new SqlCommand();
            bool flag = true;
            this.errInfo = "";
            int num2 = -1;
            try
            {
                if (dtAttReport.Rows.Count > 0)
                {
                    this.cmd.Connection = this.cn;
                    this.cmd.CommandType = CommandType.Text;
                    for (int i = 0; i <= (dtAttReport.Rows.Count - 1); i++)
                    {
                        if (this.bStopCreate)
                        {
                            return num2;
                        }
                        DataRow row = dtAttReport.Rows[i];
                        str = " INSERT INTO t_d_PatrolStatistic ";
                        str = str + " ( f_ConsumerID, f_PatrolDateStart, f_PatrolDateEnd ";
                        str = str + " , f_TotalLate, f_TotalEarly, f_TotalAbsence, f_TotalNormal ";
                        str = str + " ) ";
                        str = str + " Values ( " + row["f_ConsumerID"];
                        str = str + "," + this.PrepareStr(row["f_PatrolDateStart"], true, "yyyy-MM-dd");
                        str = str + "," + this.PrepareStr(row["f_PatrolDateEnd"], true, "yyyy-MM-dd");
                        str = str + "," + row["f_TotalLate"];
                        str = str + "," + row["f_TotalEarly"];
                        str = str + "," + row["f_TotalAbsence"];
                        str = str + "," + row["f_TotalNormal"];
                        str = str + ") ";
                        if (this.cn.State == ConnectionState.Closed)
                        {
                            this.cn.Open();
                        }
                        this.cmd.CommandText = str;
                        if (this.cmd.ExecuteNonQuery() <= 0)
                        {
                            this.errInfo = str;
                            flag = false;
                            break;
                        }
                    }
                }
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
                if (flag)
                {
                    num2 = 0;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString() + "\r\n" + str, new object[] { EventLogEntryType.Error });
            }
            finally
            {
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
            }
            return num2;
        }

        public int shift_AttStatistic_cleardb()
        {
            this.errInfo = "";
            int num = -1;
            try
            {
                if (wgAppConfig.runUpdateSql("Delete From t_d_PatrolStatistic") >= 0)
                {
                    num = 0;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_work_schedule_analyst(DataTable dtShiftWorkSchedule)
        {
            this.getPatrolParam();
            this.errInfo = "";
            int num = -1;
            try
            {
                for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                {
                    if (this.bStopCreate)
                    {
                        return num;
                    }
                    object expression = dtShiftWorkSchedule.Rows[i]["f_PlanPatrolTime"];
                    if (!Information.IsDBNull(expression))
                    {
                        if (Information.IsDBNull(dtShiftWorkSchedule.Rows[i]["f_RealPatrolTime"]))
                        {
                            dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescAbsence;
                        }
                        else
                        {
                            object obj3 = dtShiftWorkSchedule.Rows[i]["f_RealPatrolTime"];
                            TimeSpan span = Convert.ToDateTime(expression).Subtract(Convert.ToDateTime(obj3));
                            if (Math.Abs(span.TotalMinutes) <= this.tOnTimePatrolTimeout)
                            {
                                dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescNormal;
                            }
                            else if (span.TotalMinutes > this.tOnTimePatrolTimeout)
                            {
                                dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescEarly;
                            }
                            else if (span.TotalMinutes < -this.tOnTimePatrolTimeout)
                            {
                                dtShiftWorkSchedule.Rows[i]["f_EventDesc"] = this.tPatrolEventDescLate;
                            }
                        }
                    }
                }
                num = 0;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_work_schedule_cleardb()
        {
            this.errInfo = "";
            int num = -1;
            try
            {
                if (wgAppConfig.runUpdateSql("Delete From t_d_PatrolDetailData") >= 0)
                {
                    num = 0;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_work_schedule_create(out DataTable dtShiftWorkSchedule)
        {
            this.dtShiftWork = new DataTable("t_d_PatrolPlanWork");
            int num = -1;
            dtShiftWorkSchedule = null;
            try
            {
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ConsumerID";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_PatrolDate";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_RouteID";
                this.dc.DefaultValue = -1;
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ReaderID";
                this.dc.DefaultValue = 0;
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_PlanPatrolTime";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_RealPatrolTime";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_EventDesc";
                this.dc.DefaultValue = 0;
                this.dtShiftWork.Columns.Add(this.dc);
                dtShiftWorkSchedule = this.dtShiftWork.Copy();
                num = 0;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_work_schedule_fill(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd, ref int bNotArranged)
        {
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            bool flag = false;
            object[] objArray = new object[0x25];
            this.errInfo = "";
            int num = -1;
            DateTime time2 = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time3 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time2 <= time3)
            {
                try
                {
                    string str2 = "";
                    DateTime expression = time2;
                    do
                    {
                        string str;
                        SqlDataReader reader;
                        if (str2 != Strings.Format(expression, "yyyy-MM"))
                        {
                            str2 = Strings.Format(expression, "yyyy-MM");
                            str = " SELECT * FROM t_d_PatrolPlanData ";
                            str = (str + " WHERE f_ConsumerID = " + consumerid) + " AND f_DateYM = " + this.PrepareStr(str2);
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new SqlCommand(str, this.cn);
                            reader = this.cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                flag = true;
                                for (int i = 0; i <= (reader.FieldCount - 1); i++)
                                {
                                    objArray[i] = reader[i];
                                }
                            }
                            else
                            {
                                bNotArranged |= 1;
                                flag = false;
                                objArray[0] = -1;
                            }
                            reader.Close();
                        }
                        do
                        {
                            if (flag)
                            {
                                int num2 = Convert.ToInt32(objArray[2 + expression.Day]);
                                if (num2 > 0)
                                {
                                    str = "SELECT * FROM t_d_PatrolRouteDetail WHERE f_RouteID = " + num2 + " ORDER BY f_Sn ";
                                    this.cmd = new SqlCommand(str, this.cn);
                                    if (this.cn.State != ConnectionState.Open)
                                    {
                                        this.cn.Open();
                                    }
                                    reader = this.cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        DataRow row = dtShiftWorkSchedule.NewRow();
                                        row[0] = consumerid;
                                        row[1] = expression;
                                        row["f_RouteID"] = reader["f_RouteID"];
                                        row["f_ReaderID"] = reader["f_ReaderID"];
                                        if (((int) reader["f_NextDay"]) > 0)
                                        {
                                            row["f_PlanPatrolTime"] = Convert.ToDateTime(string.Concat(new object[] { Strings.Format(expression.AddDays(1.0), "yyyy-MM-dd"), " ", reader["f_patroltime"], ":00" }));
                                        }
                                        else
                                        {
                                            row["f_PlanPatrolTime"] = Convert.ToDateTime(string.Concat(new object[] { Strings.Format(expression, "yyyy-MM-dd"), " ", reader["f_patroltime"], ":00" }));
                                        }
                                        dtShiftWorkSchedule.Rows.Add(row);
                                    }
                                    reader.Close();
                                }
                            }
                            expression = expression.AddDays(1.0);
                        }
                        while (!(str2 != Strings.Format(expression, "yyyy-MM")) && (expression <= time3));
                    }
                    while (expression <= time3);
                    num = 0;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                finally
                {
                    if (this.cn.State == ConnectionState.Open)
                    {
                        this.cn.Close();
                    }
                }
            }
            return num;
        }

        public int shift_work_schedule_updatebyReadcard(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
        {
            this.getPatrolParam();
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.dsAtt = new DataSet();
            this.errInfo = "";
            int num = -1;
            DateTime time = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time <= time2)
            {
                try
                {
                    string str = "SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, 0 as f_used  FROM t_d_SwipeRecord, t_b_Reader,t_b_Reader4Patrol ";
                    str = ((((str + " , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  f_ConsumerID = " + consumerid) + 
                        " AND ([f_ReadDate]>= " + this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00") + ") ") + 
                        " AND ([f_ReadDate]<= " + this.PrepareStr(dateEnd.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + 
                        " AND (t_d_SwipeRecord.f_ReaderID = t_b_Reader.f_ReaderID) ") + 
                        " AND t_b_Reader.f_ReaderID = t_b_Reader4Patrol.f_ReaderID " + 
                        " ORDER BY f_ReadDate ASC, f_RecID ASC ";
                    this.cmd = new SqlCommand();
                    this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
                    this.cmd.Connection = this.cn;
                    this.cmd.CommandText = str;
                    this.cmd.CommandType = CommandType.Text;
                    this.daCardRecord = new SqlDataAdapter(this.cmd);
                    this.daCardRecord.Fill(this.dsAtt, "CardRecord");
                    this.dtCardRecord = this.dsAtt.Tables["CardRecord"];
                    this.daCardRecord = new SqlDataAdapter("SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_DutyOnOff,0 as f_used FROM t_d_SwipeRecord, t_b_Reader , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  1<0  ", this.cn);
                    this.daCardRecord.Fill(this.dsAtt, "ValidCardRecord");
                    this.dtValidCardRecord = this.dsAtt.Tables["ValidCardRecord"];
                    if (this.dtCardRecord.Rows.Count > 0)
                    {
                        object[] array = new object[(this.dtCardRecord.Columns.Count - 1) + 1];
                        DateTime time4 = Convert.ToDateTime(this.dtCardRecord.Rows[0]["f_ReadDate"]);
                        int num2 = Convert.ToInt32(this.dtCardRecord.Rows[0]["f_ReaderID"]);
                        DataRow row = this.dtValidCardRecord.NewRow();
                        this.dtCardRecord.Rows[0].ItemArray.CopyTo(array, 0);
                        row.ItemArray = array;
                        this.dtValidCardRecord.Rows.Add(row);
                        time4 = Convert.ToDateTime(row["f_ReadDate"]);
                        num2 = Convert.ToInt32(row["f_ReaderID"]);
                        for (int j = 1; j <= (this.dtCardRecord.Rows.Count - 1); j++)
                        {
                            if (this.bStopCreate)
                            {
                                return num;
                            }
                            DateTime time3 = Convert.ToDateTime(this.dtCardRecord.Rows[j]["f_ReadDate"]);
                            TimeSpan span = time3.Subtract(time4);
                            int num3 = Convert.ToInt32(this.dtCardRecord.Rows[j]["f_ReaderID"]);
                            if ((span.TotalSeconds > this.tTwoReadMintime) || (num3 != num2))
                            {
                                time4 = time3;
                                num2 = num3;
                                row = this.dtValidCardRecord.NewRow();
                                this.dtCardRecord.Rows[j].ItemArray.CopyTo(array, 0);
                                row.ItemArray = array;
                                this.dtValidCardRecord.Rows.Add(row);
                            }
                        }
                    }
                    int num6 = 0;
                    for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                    {
                        if (this.bStopCreate)
                        {
                            return num;
                        }
                        object expression = dtShiftWorkSchedule.Rows[i]["f_PlanPatrolTime"];
                        if (Information.IsDBNull(expression))
                        {
                            continue;
                        }
                        bool flag = false;
                        int num7 = num6;
                    Label_03DB:
                        if (num6 < this.dtValidCardRecord.Rows.Count)
                        {
                            object obj3 = this.dtValidCardRecord.Rows[num6]["f_ReadDate"];
                            TimeSpan span2 = Convert.ToDateTime(expression).Subtract(Convert.ToDateTime(obj3));
                            if (span2.TotalMinutes > this.tNotPatrolTimeout)
                            {
                                num6++;
                                num7 = num6;
                                goto Label_03DB;
                            }
                            if (span2.TotalMinutes >= -this.tNotPatrolTimeout)
                            {
                                if ((wgTools.SetObjToStr(this.dtValidCardRecord.Rows[num6]["f_used"]) != "1") && (((int) dtShiftWorkSchedule.Rows[i]["f_ReaderID"]) == ((int) this.dtValidCardRecord.Rows[num6]["f_ReaderID"])))
                                {
                                    dtShiftWorkSchedule.Rows[i]["f_RealPatrolTime"] = obj3;
                                    flag = true;
                                    this.dtValidCardRecord.Rows[num6]["f_used"] = 1;
                                }
                                else
                                {
                                    num6++;
                                    goto Label_03DB;
                                }
                            }
                        }
                        if (!flag)
                        {
                            num6 = num7;
                        }
                    }
                    num = 0;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                finally
                {
                    if (this.cn.State == ConnectionState.Open)
                    {
                        this.cn.Close();
                    }
                }
            }
            return num;
        }

        public int shift_work_schedule_writetodb(DataTable dtShiftWorkSchedule)
        {
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.cmd = new SqlCommand();
            string str = "";
            bool flag = true;
            this.errInfo = "";
            int num2 = -1;
            try
            {
                if (dtShiftWorkSchedule.Rows.Count > 0)
                {
                    using (this.cmd = new SqlCommand())
                    {
                        this.cmd.Connection = this.cn;
                        this.cmd.CommandType = CommandType.Text;
                        for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                        {
                            if (this.bStopCreate)
                            {
                                return num2;
                            }
                            DataRow row = dtShiftWorkSchedule.Rows[i];
                            str = " INSERT INTO t_d_PatrolDetailData ";
                            str = str + " ( f_ConsumerID, f_PatrolDate, f_RouteID, f_ReaderID, f_PlanPatrolTime, f_RealPatrolTime, f_EventDesc";
                            str = str + " ) ";
                            str = str + " Values ( " + row["f_ConsumerID"];
                            str = str + "," + this.PrepareStr(row["f_PatrolDate"], true, "yyyy-MM-dd");
                            str = str + "," + row["f_RouteID"];
                            str = str + "," + row["f_ReaderID"];
                            str = str + "," + this.PrepareStr(row["f_PlanPatrolTime"], true, "yyyy-MM-dd HH:mm:ss");
                            str = str + "," + this.PrepareStr(row["f_RealPatrolTime"], true, "yyyy-MM-dd HH:mm:ss");
                            str = str + "," + this.PrepareStr(row["f_EventDesc"]);
                            str = str + ") ";
                            if (this.cn.State == ConnectionState.Closed)
                            {
                                this.cn.Open();
                            }
                            this.cmd.CommandText = str;
                            if (this.cmd.ExecuteNonQuery() <= 0)
                            {
                                this.errInfo = str;
                                flag = false;
                                goto Label_01FA;
                            }
                        }
                    }
                }
            Label_01FA:
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
                if (flag)
                {
                    num2 = 0;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString() + "\r\n" + str, new object[] { EventLogEntryType.Error });
            }
            finally
            {
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
            }
            return num2;
        }

        public int tm(object dt)
        {
            int num = 0;
            try
            {
                num = int.Parse(Strings.Format((DateTime) dt, "HHmmss"));
            }
            catch (Exception)
            {
            }
            return num;
        }
    }
}

