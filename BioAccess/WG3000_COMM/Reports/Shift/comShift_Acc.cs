namespace WG3000_COMM.Reports.Shift
{
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;

    public class comShift_Acc : Component
    {
        public bool bStopCreate;
        private OleDbCommand cmd;
        private OleDbConnection cn;
        private Container components;
        private OleDbDataAdapter daCardRecord;
        private OleDbDataAdapter daHolidayType;
        private OleDbDataAdapter daLeave;
        private DataColumn dc;
        private DataSet dsAtt;
        private DataTable dtCardRecord;
        private DataTable dtHolidayType;
        private DataTable dtLeave;
        private DataTable dtReport;
        private DataTable dtReport1;
        private DataTable dtShiftWork;
        private DataTable dtValidCardRecord;
        private const int EER_INVILID_SHIFTID = -6;
        private const int EER_SQL_RUNFAIL = -999;
        private const int EER_TIMEOVERLAPPED = -7;
        private const int ERR_DAYDIFF = -5;
        private const int ERR_FAIL = -1;
        private const int ERR_ID = -2;
        private const int ERR_NONE = 0;
        private const int ERR_READTIMES = -3;
        private const int ERR_TIMEDIFF = -4;
        public string errInfo;
        private int minShifDiffByMinute;
        private DateTime realOffduty1;
        private DateTime realOffduty2;
        private DateTime realOffduty3;
        private DateTime realOffduty4;
        private DateTime realOnduty1;
        private DateTime realOnduty2;
        private DateTime realOnduty3;
        private DateTime realOnduty4;
        private string strTemp;
        private int tAheadMinutes;
        private int tAheadMinutesOnDutyFirst;
        private int tDelayMinutes;
        private decimal tLateAbsenceDay;
        private int tLateTimeout;
        private decimal tLeaveAbsenceDay;
        private int tLeaveTimeout;
        private int tOvertimeMinutes;
        private int tOvertimeTimeout;
        private int tTwoReadMintime;

        public comShift_Acc()
        {
            this.strTemp = "";
            this.errInfo = "";
            this.tTwoReadMintime = 60;
            this.tAheadMinutesOnDutyFirst = 120;
            this.tAheadMinutes = 30;
            this.tDelayMinutes = 30;
            this.tOvertimeMinutes = 480;
            this.minShifDiffByMinute = 5;
            this.dsAtt = new DataSet();
            this.InitializeComponent();
        }

        public comShift_Acc(IContainer Container) : this()
        {
            Container.Add(this);
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
                case -7:
                    return CommonStr.strTimeOverlapped;

                case -6:
                    return CommonStr.strArrangedInvalidShiftID;

                case -5:
                    return CommonStr.strShiftTimeOver24;

                case -4:
                    return CommonStr.strErrTimeDiff;

                case -3:
                    return CommonStr.strInvalidReadTimes;

                case -2:
                    return CommonStr.strInvalidShiftID;

                case -1:
                    return CommonStr.strFailed;

                case -999:
                    return CommonStr.strSqlRunFail;
            }
            return CommonStr.strUnknown;
        }

        public void getAttendenceParam()
        {
            this.tLateAbsenceDay = 0.5M;
            this.tLeaveAbsenceDay = 0.5M;
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            string cmdText = "SELECT * FROM t_a_Shift_Attendence";
            try
            {
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(cmdText, this.cn);
                OleDbDataReader reader = this.cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (((int) reader["f_No"]) == 1)
                    {
                        this.tLateTimeout = Convert.ToInt32(reader["f_Value"]);
                    }
                    else
                    {
                        if (((int) reader["f_No"]) == 4)
                        {
                            this.tLeaveTimeout = Convert.ToInt32(reader["f_Value"]);
                            continue;
                        }
                        if (((int) reader["f_No"]) == 7)
                        {
                            this.tOvertimeTimeout = Convert.ToInt32(reader["f_Value"]);
                            continue;
                        }
                        if (((int) reader["f_No"]) == 0x11)
                        {
                            this.tAheadMinutesOnDutyFirst = Convert.ToInt32(reader["f_Value"]);
                            continue;
                        }
                        if (((int) reader["f_No"]) == 0x12)
                        {
                            this.tAheadMinutes = Convert.ToInt32(reader["f_Value"]);
                            continue;
                        }
                        if (((int) reader["f_No"]) == 0x13)
                        {
                            this.tDelayMinutes = Convert.ToInt32(reader["f_Value"]);
                            continue;
                        }
                        if (((int) reader["f_No"]) == 20)
                        {
                            this.tOvertimeMinutes = Convert.ToInt32(reader["f_Value"]);
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            finally
            {
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void localizedHoliday(DataTable dt)
        {
            try
            {
                for (int i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_Saturday") == 0)
                    {
                        dt.Rows[i]["f_Name"] = CommonStr.strHoliday_Saturday;
                    }
                    else if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_Sunday") == 0)
                    {
                        dt.Rows[i]["f_Name"] = CommonStr.strHoliday_Sunday;
                    }
                    else if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_AM") == 0)
                    {
                        dt.Rows[i]["f_Name"] = CommonStr.strHoliday_AM;
                    }
                    else if (string.Compare("strHoliday_" + dt.Rows[i]["f_EName"], "strHoliday_PM") == 0)
                    {
                        dt.Rows[i]["f_Name"] = CommonStr.strHoliday_PM;
                    }
                    if (!Information.IsDBNull(dt.Rows[i]["f_Value1"]) && (((dt.Rows[i]["f_Value1"].ToString() == "A.M.") || (dt.Rows[i]["f_Value1"].ToString() == "上午")) || (dt.Rows[i]["f_Value1"].ToString() == "上午")))
                    {
                        dt.Rows[i]["f_Value1"] = CommonStr.strHoliday_AM;
                    }
                    if (!Information.IsDBNull(dt.Rows[i]["f_Value3"]) && (((dt.Rows[i]["f_Value3"].ToString() == "A.M.") || (dt.Rows[i]["f_Value3"].ToString() == "上午")) || (dt.Rows[i]["f_Value3"].ToString() == "上午")))
                    {
                        dt.Rows[i]["f_Value3"] = CommonStr.strHoliday_AM;
                    }
                    if (!Information.IsDBNull(dt.Rows[i]["f_Value1"]) && (((dt.Rows[i]["f_Value1"].ToString() == "P.M.") || (dt.Rows[i]["f_Value1"].ToString() == "下午")) || (dt.Rows[i]["f_Value1"].ToString() == "下午")))
                    {
                        dt.Rows[i]["f_Value1"] = CommonStr.strHoliday_PM;
                    }
                    if (!Information.IsDBNull(dt.Rows[i]["f_Value3"]) && (((dt.Rows[i]["f_Value3"].ToString() == "P.M.") || (dt.Rows[i]["f_Value3"].ToString() == "下午")) || (dt.Rows[i]["f_Value3"].ToString() == "下午")))
                    {
                        dt.Rows[i]["f_Value3"] = CommonStr.strHoliday_PM;
                    }
                }
                dt.AcceptChanges();
            }
            catch (Exception)
            {
            }
        }

        public static void localizedHolidayType(DataTable dt)
        {
            try
            {
                for (int i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (((dt.Rows[i]["f_HolidayType"].ToString() == "出差") || (dt.Rows[i]["f_HolidayType"].ToString() == "出差")) || (dt.Rows[i]["f_HolidayType"].ToString() == "Business Trip"))
                    {
                        dt.Rows[i]["f_HolidayType"] = CommonStr.strBusinessTrip;
                    }
                    if (((dt.Rows[i]["f_HolidayType"].ToString() == "病假") || (dt.Rows[i]["f_HolidayType"].ToString() == "病假")) || (dt.Rows[i]["f_HolidayType"].ToString() == "Sick Leave"))
                    {
                        dt.Rows[i]["f_HolidayType"] = CommonStr.strSickLeave;
                    }
                    if (((dt.Rows[i]["f_HolidayType"].ToString() == "事假") || (dt.Rows[i]["f_HolidayType"].ToString() == "事假")) || (dt.Rows[i]["f_HolidayType"].ToString() == "Private Leave"))
                    {
                        dt.Rows[i]["f_HolidayType"] = CommonStr.strPrivateLeave;
                    }
                }
                dt.AcceptChanges();
            }
            catch (Exception)
            {
            }
        }

        public int logCreateReport(DateTime startDateTime, DateTime endDateTime, string groupName, string totalConsumer)
        {
            int num = -1;
            try
            {
                string str4 = CommonStr.strCreateLog + "  [" + CommonStr.strOperateDate + DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + "]";
                string str5 = str4 + ";  " + CommonStr.strFrom + Strings.Format(startDateTime, wgTools.DisplayFormat_DateYMD) + CommonStr.strTo + Strings.Format(endDateTime, wgTools.DisplayFormat_DateYMD);
                string str = str5 + ";   " + wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartment) + ":" + groupName + "            " + CommonStr.strUser + " (" + totalConsumer + ")";
                string str2 = Strings.Format(startDateTime, "yyyy-MM-dd") + "--" + Strings.Format(endDateTime, "yyyy-MM-dd");
                wgAppConfig.runUpdateSql((("UPDATE t_a_Attendence " + " SET [f_Value]=" + this.PrepareStr(str2)) + " , [f_Notes] = " + this.PrepareStr(str)) + " WHERE [f_NO]= 15 ");
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

        public int shift_add(int id, string name, int readtimes, DateTime onduty1, DateTime offduty1, DateTime onduty2, DateTime offduty2, DateTime onduty3, DateTime offduty3, DateTime onduty4, DateTime offduty4, int bOvertimeShift)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            int num = -1;
            this.errInfo = "";
            try
            {
                num = this.shift_checkvalid(id, name, readtimes, onduty1, offduty1, onduty2, offduty2, onduty3, offduty3, onduty4, offduty4);
                if (num != 0)
                {
                    return num;
                }
                string cmdText = " SELECT count(*) FROM t_b_ShiftSet WHERE f_ShiftID = " + id;
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(cmdText, this.cn);
                int num2 = Convert.ToInt32(this.cmd.ExecuteScalar());
                this.cn.Close();
                if (num2 > 0)
                {
                    num = -2;
                    this.errInfo = id.ToString();
                    return num;
                }
                cmdText = " INSERT INTO t_b_ShiftSet ( f_ShiftID, f_ShiftName, f_ReadTimes,  f_OnDuty1, f_OffDuty1, f_OnDuty2, f_OffDuty2, f_OnDuty3, f_OffDuty3, f_OnDuty4, f_OffDuty4,f_bOvertimeShift, f_Notes)";
                cmdText = ((((cmdText + " VALUES ( " + id) + " , " + this.PrepareStr(name)) + " , " + readtimes) + " , " + this.PrepareStr(this.realOnduty1, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(this.realOffduty1, true, "yyyy-MM-dd HH:mm:ss");
                if (readtimes > 2)
                {
                    cmdText = (cmdText + " , " + this.PrepareStr(this.realOnduty2, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(this.realOffduty2, true, "yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    cmdText = (cmdText + " , " + this.PrepareStr("")) + " , " + this.PrepareStr("");
                }
                if (readtimes > 4)
                {
                    cmdText = (cmdText + " , " + this.PrepareStr(this.realOnduty3, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(this.realOffduty3, true, "yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    cmdText = (cmdText + " , " + this.PrepareStr("")) + " , " + this.PrepareStr("");
                }
                if (readtimes > 6)
                {
                    cmdText = (cmdText + " , " + this.PrepareStr(this.realOnduty4, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(this.realOffduty4, true, "yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    cmdText = (cmdText + " , " + this.PrepareStr("")) + " , " + this.PrepareStr("");
                }
                cmdText = ((cmdText + " , " + bOvertimeShift) + " , " + this.PrepareStr("")) + " )";
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(cmdText, this.cn);
                num2 = this.cmd.ExecuteNonQuery();
                this.cn.Close();
                if (num2 > 0)
                {
                    return 0;
                }
                return -1;
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
            return num;
        }

        public int shift_arrange_delete(int consumerId, DateTime dateStart, DateTime dateEnd)
        {
            string str;
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            bool flag = false;
            object[] objArray = new object[0x25];
            this.errInfo = "";
            int num = -1;
            if (consumerId == 0)
            {
                str = " DELETE FROM t_d_ShiftData ";
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(str, this.cn);
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
                            str = " SELECT * FROM t_d_ShiftData ";
                            str = (str + " WHERE f_ConsumerID = " + consumerId) + " AND f_DateYM = " + this.PrepareStr(str2);
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new OleDbCommand(str, this.cn);
                            OleDbDataReader reader = this.cmd.ExecuteReader();
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
                                str = "  DELETE FROM t_d_ShiftData ";
                                str = str + " WHERE f_RecID = " + objArray[0];
                            }
                            else
                            {
                                str = "  UPDATE t_d_ShiftData SET ";
                                num4 = 1;
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, " f_ShiftID_", num4.ToString().PadLeft(2, '0'), " = ", objArray[2 + num4] });
                                for (num4 = 2; num4 <= 0x1f; num4++)
                                {
                                    object obj3 = str;
                                    str = string.Concat(new object[] { obj3, " , f_ShiftID_", num4.ToString().PadLeft(2, '0'), " = ", objArray[2 + num4] });
                                }
                                str = ((str + " , f_LogDate  = " + this.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss")) + " , f_Notes  = " + this.PrepareStr("")) + " WHERE f_RecID = " + objArray[0];
                            }
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new OleDbCommand(str, this.cn);
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
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
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
                            str = " SELECT * FROM t_d_ShiftData ";
                            str = (str + " WHERE f_ConsumerID = " + consumerId) + " AND f_DateYM = " + this.PrepareStr(str2);
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new OleDbCommand(str, this.cn);
                            OleDbDataReader reader = this.cmd.ExecuteReader();
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
                            str = "  UPDATE t_d_ShiftData SET ";
                            int num7 = 1;
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, " f_ShiftID_", num7.ToString().PadLeft(2, '0'), " = ", objArray[2 + num7] });
                            for (num7 = 2; num7 <= 0x1f; num7++)
                            {
                                object obj3 = str;
                                str = string.Concat(new object[] { obj3, " , f_ShiftID_", num7.ToString().PadLeft(2, '0'), " = ", objArray[2 + num7] });
                            }
                            str = ((str + " , f_LogDate  = " + this.PrepareStr(DateTime.Now, true, "yyyy-MM-dd HH:mm:ss")) + " , f_Notes  = " + this.PrepareStr("")) + " WHERE f_RecID = " + objArray[0];
                        }
                        else
                        {
                            int num8;
                            str = "  INSERT INTO t_d_ShiftData  ";
                            str = str + " ( f_ConsumerID , f_DateYM  ";
                            for (num8 = 1; num8 <= 0x1f; num8++)
                            {
                                str = str + " , f_ShiftID_" + num8.ToString().PadLeft(2, '0');
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
                        this.cmd = new OleDbCommand(str, this.cn);
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

        public int shift_AttReport_cleardb()
        {
            this.errInfo = "";
            int num = -1;
            try
            {
                if (wgAppConfig.runUpdateSql("Delete From t_d_shift_AttReport") >= 0)
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
                this.dc.ColumnName = "f_ShiftDate";
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ShiftID";
                this.dc.DefaultValue = -1;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_Readtimes";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                for (int i = 1; i <= 4; i++)
                {
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.DateTime");
                    this.dc.ColumnName = "f_OnDuty" + i;
                    this.dtReport.Columns.Add(this.dc);
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.String");
                    this.dc.ColumnName = "f_OnDuty" + i + "AttDesc";
                    this.dtReport.Columns.Add(this.dc);
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.String");
                    this.dc.ColumnName = "f_OnDuty" + i + "CardRecordDesc";
                    this.dtReport.Columns.Add(this.dc);
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.DateTime");
                    this.dc.ColumnName = "f_OffDuty" + i;
                    this.dtReport.Columns.Add(this.dc);
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.String");
                    this.dc.ColumnName = "f_OffDuty" + i + "AttDesc";
                    this.dtReport.Columns.Add(this.dc);
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.String");
                    this.dc.ColumnName = "f_OffDuty" + i + "CardRecordDesc";
                    this.dtReport.Columns.Add(this.dc);
                }
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LateMinutes";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LeaveEarlyMinutes";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Decimal");
                this.dc.ColumnName = "f_OvertimeHours";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Decimal");
                this.dc.ColumnName = "f_AbsenceDays";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_NotReadCardCount";
                this.dc.DefaultValue = 0;
                this.dtReport.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_bOvertimeShift";
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
                    DataRow row2 = dtShiftWorkSchedule.Rows[i];
                    int num3 = Convert.ToInt32(row2["f_Readtimes"]);
                    if (Convert.ToInt32(row2["f_ShiftID"]) <= 0)
                    {
                        row["f_ConsumerID"] = row2["f_ConsumerID"];
                        row["f_ShiftDate"] = row2["f_ShiftDate"];
                        row["f_ShiftID"] = row2["f_ShiftID"];
                        row["f_Readtimes"] = row2["f_Readtimes"];
                        for (int j = 1; j <= 4; j++)
                        {
                            row["f_OnDuty" + j + "AttDesc"] = row2["f_AttDesc"];
                            row["f_OffDuty" + j + "AttDesc"] = row2["f_AttDesc"];
                        }
                        dtAttReport.Rows.Add(row);
                        row = dtAttReport.NewRow();
                    }
                    else
                    {
                        int num4 = Convert.ToInt32(row2["f_Timeseg"]);
                        if (num4 == 0)
                        {
                            row["f_LateMinutes"] = 0;
                            row["f_LeaveEarlyMinutes"] = 0;
                            row["f_OvertimeHours"] = 0;
                            row["f_AbsenceDays"] = 0;
                            row["f_NotReadCardCount"] = 0;
                            row["f_ConsumerID"] = row2["f_ConsumerID"];
                            row["f_ShiftDate"] = row2["f_ShiftDate"];
                            row["f_ShiftID"] = row2["f_ShiftID"];
                            row["f_Readtimes"] = row2["f_Readtimes"];
                            row["f_bOvertimeShift"] = row2["f_bOvertimeShift"];
                        }
                        if ((num4 & 1) == 0)
                        {
                            row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "AttDesc"] = this.SetObjToStr(row2["f_AttDesc"]);
                            row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "CardRecordDesc"] = this.SetObjToStr(row2["f_CardRecordDesc"]);
                            row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)] = row2["f_WorkTime"];
                        }
                        else
                        {
                            row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "AttDesc"] = this.SetObjToStr(row2["f_AttDesc"]);
                            row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "CardRecordDesc"] = this.SetObjToStr(row2["f_CardRecordDesc"]);
                            row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)] = row2["f_WorkTime"];
                        }
                        if (this.SetObjToStr(row2["f_AttDesc"]) == CommonStr.strLateness)
                        {
                            row["f_LateMinutes"] = Convert.ToDecimal(row["f_LateMinutes"], CultureInfo.InvariantCulture) + Convert.ToDecimal(row2["f_Duration"], CultureInfo.InvariantCulture);
                        }
                        else if (this.SetObjToStr(row2["f_AttDesc"]) == CommonStr.strLeaveEarly)
                        {
                            row["f_LeaveEarlyMinutes"] = Convert.ToDecimal(row["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture) + Convert.ToDecimal(row2["f_Duration"], CultureInfo.InvariantCulture);
                        }
                        else if (this.SetObjToStr(row2["f_AttDesc"]) == CommonStr.strAbsence)
                        {
                            if ((num4 & 1) == 0)
                            {
                                row["f_AbsenceDays"] = Convert.ToDecimal(row["f_AbsenceDays"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                            }
                            else
                            {
                                row["f_AbsenceDays"] = Convert.ToDecimal(row["f_AbsenceDays"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
                            }
                        }
                        else if (this.SetObjToStr(row2["f_AttDesc"]) == CommonStr.strOvertime)
                        {
                            if (Convert.ToInt32(row["f_bOvertimeShift"]) > 0)
                            {
                                if (Convert.ToInt32(row["f_bOvertimeShift"]) == 1)
                                {
                                    if ((((num4 & 1) != 0) && !(this.SetObjToStr(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]) == "")) && !(this.SetObjToStr(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]) == ""))
                                    {
                                        int num6 = (int) (Convert.ToDateTime(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]).Subtract(Convert.ToDateTime(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)])).TotalMinutes / 30.0);
                                        row["f_OvertimeHours"] = Convert.ToDecimal(row["f_OvertimeHours"], CultureInfo.InvariantCulture) + (num6 / 2.0M);
                                    }
                                }
                                else if (((Convert.ToInt32(row["f_bOvertimeShift"]) == 2) && ((num4 & 1) != 0)) && ((this.SetObjToStr(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]) != "") && (this.SetObjToStr(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]) != "")))
                                {
                                    int num7;
                                    object obj2 = dtShiftWorkSchedule.Rows[i - 1]["f_PlanTime"];
                                    if (Convert.ToDateTime(obj2).Subtract(Convert.ToDateTime(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)])).TotalMinutes < 0.0)
                                    {
                                        num7 = (int) (Convert.ToDateTime(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]).Subtract(Convert.ToDateTime(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)])).TotalMinutes / 30.0);
                                    }
                                    else
                                    {
                                        num7 = (int) (Convert.ToDateTime(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]).Subtract(Convert.ToDateTime(obj2)).TotalMinutes / 30.0);
                                    }
                                    row["f_OvertimeHours"] = Convert.ToDecimal(row["f_OvertimeHours"], CultureInfo.InvariantCulture) + (num7 / 2.0M);
                                }
                            }
                            else
                            {
                                row["f_OvertimeHours"] = Convert.ToDecimal(row["f_OvertimeHours"], CultureInfo.InvariantCulture) + (Conversion.Int((int) (Convert.ToInt32(row2["f_Duration"]) / 30)) / 2.0M);
                            }
                        }
                        else if (this.SetObjToStr(row2["f_AttDesc"]) == CommonStr.strNotReadCard)
                        {
                            row["f_NotReadCardCount"] = Convert.ToInt32(row["f_NotReadCardCount"]) + 1;
                        }
                        if ((num4 + 1) == num3)
                        {
                            if (Convert.ToDecimal(row["f_AbsenceDays"], CultureInfo.InvariantCulture) > 1M)
                            {
                                row["f_AbsenceDays"] = 1;
                            }
                            dtAttReport.Rows.Add(row);
                            row = dtAttReport.NewRow();
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

        public int shift_AttReport_writetodb(DataTable dtAttReport)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            string str = "";
            this.cmd = new OleDbCommand();
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
                        str = " INSERT INTO t_d_Shift_AttReport ";
                        str = str + " ( f_ConsumerID, f_shiftDate, f_ShiftID, f_ReadTimes ";
                        str = str + " , f_OnDuty1, f_OnDuty1AttDesc, f_OnDuty1CardRecordDesc ";
                        str = str + " , f_OffDuty1, f_OffDuty1AttDesc, f_OffDuty1CardRecordDesc ";
                        str = str + " , f_OnDuty2, f_OnDuty2AttDesc, f_OnDuty2CardRecordDesc ";
                        str = str + " , f_OffDuty2, f_OffDuty2AttDesc, f_OffDuty2CardRecordDesc ";
                        str = str + " , f_OnDuty3, f_OnDuty3AttDesc, f_OnDuty3CardRecordDesc ";
                        str = str + " , f_OffDuty3, f_OffDuty3AttDesc, f_OffDuty3CardRecordDesc ";
                        str = str + " , f_OnDuty4, f_OnDuty4AttDesc, f_OnDuty4CardRecordDesc ";
                        str = str + " , f_OffDuty4, f_OffDuty4AttDesc, f_OffDuty4CardRecordDesc ";
                        str = str + " , f_LateMinutes, f_LeaveEarlyMinutes, f_OvertimeHours, f_AbsenceDays ";
                        str = str + " , f_NotReadCardCount, f_bOvertimeShift ";
                        str = str + " ) ";
                        str = str + " Values ( " + row["f_ConsumerID"];
                        str = str + "," + this.PrepareStr(row["f_shiftDate"], true, "yyyy-MM-dd");
                        str = str + "," + row["f_ShiftID"];
                        str = str + "," + row["f_ReadTimes"];
                        str = str + "," + this.PrepareStr(row["f_OnDuty1"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OnDuty1AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty1CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty1"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OffDuty1AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty1CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty2"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OnDuty2AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty2CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty2"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OffDuty2AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty2CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty3"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OnDuty3AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty3CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty3"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OffDuty3AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty3CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty4"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OnDuty4AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OnDuty4CardRecordDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty4"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_OffDuty4AttDesc"]);
                        str = str + "," + this.PrepareStr(row["f_OffDuty4CardRecordDesc"]);
                        str = str + "," + row["f_LateMinutes"];
                        str = str + "," + row["f_LeaveEarlyMinutes"];
                        str = str + "," + row["f_OvertimeHours"];
                        str = str + "," + row["f_AbsenceDays"];
                        str = str + "," + row["f_NotReadCardCount"];
                        str = str + "," + row["f_bOvertimeShift"];
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
                if (wgAppConfig.runUpdateSql("Delete From t_d_shift_AttStatistic") >= 0)
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

        public int shift_AttStatistic_Create(out DataTable dtAttStatistic)
        {
            this.dtReport1 = new DataTable("t_d_AttStatistic");
            int num = -1;
            dtAttStatistic = null;
            try
            {
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ConsumerID";
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_AttDateStart";
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_AttDateEnd";
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_DayShouldWork";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_DayRealWork";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LateMinutes";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LateCount";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LeaveEarlyMinutes";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_LeaveEarlyCount";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Decimal");
                this.dc.ColumnName = "f_OvertimeHours";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Decimal");
                this.dc.ColumnName = "f_AbsenceDays";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_NotReadCardCount";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ManualReadTimesCount";
                this.dc.DefaultValue = 0;
                this.dtReport1.Columns.Add(this.dc);
                for (int i = 1; i <= 0x20; i++)
                {
                    this.dc = new DataColumn();
                    this.dc.DataType = Type.GetType("System.String");
                    this.dc.ColumnName = "f_SpecialType" + i;
                    this.dc.DefaultValue = "";
                    this.dtReport1.Columns.Add(this.dc);
                }
                dtAttStatistic = this.dtReport1.Copy();
                num = 0;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return num;
        }

        public int shift_AttStatistic_Fill(DataTable dtAttStatistic, DataTable dtAttReport)
        {
            this.errInfo = "";
            int num = -1;
            try
            {
                DataRow drAttStatistic = null;
                this.cn = new OleDbConnection(wgAppConfig.dbConString);
                this.dsAtt = new DataSet();
                this.daHolidayType = new OleDbDataAdapter("SELECT * FROM t_a_HolidayType", this.cn);
                this.daHolidayType.Fill(this.dsAtt, "HolidayType");
                this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
                localizedHolidayType(this.dtHolidayType);
                bool flag2 = false;
                for (int i = 0; i <= (dtAttReport.Rows.Count - 1); i++)
                {
                    int num4;
                    string str;
                    flag2 = false;
                    if (this.bStopCreate)
                    {
                        return num;
                    }
                    if (drAttStatistic == null)
                    {
                        drAttStatistic = dtAttStatistic.NewRow();
                    }
                    DataRow row = dtAttReport.Rows[i];
                    int num3 = Convert.ToInt32(row["f_Readtimes"]);
                    if (i == 0)
                    {
                        drAttStatistic["f_ConsumerID"] = row["f_ConsumerID"];
                        drAttStatistic["f_AttDateStart"] = row["f_ShiftDate"];
                    }
                    Convert.ToDateTime(row["f_ShiftDate"]);
                    drAttStatistic["f_LateMinutes"] = Convert.ToInt32(drAttStatistic["f_LateMinutes"], CultureInfo.InvariantCulture) + Convert.ToInt32(row["f_LateMinutes"], CultureInfo.InvariantCulture);
                    drAttStatistic["f_LeaveEarlyMinutes"] = Convert.ToInt32(drAttStatistic["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture) + Convert.ToInt32(row["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture);
                    drAttStatistic["f_AbsenceDays"] = Convert.ToDecimal(drAttStatistic["f_AbsenceDays"], CultureInfo.InvariantCulture) + Convert.ToDecimal(row["f_AbsenceDays"], CultureInfo.InvariantCulture);
                    drAttStatistic["f_OvertimeHours"] = Convert.ToDecimal(drAttStatistic["f_OvertimeHours"], CultureInfo.InvariantCulture) + Convert.ToDecimal(row["f_OvertimeHours"], CultureInfo.InvariantCulture);
                    drAttStatistic["f_NotReadCardCount"] = Convert.ToInt32(drAttStatistic["f_NotReadCardCount"]) + Convert.ToInt32(row["f_NotReadCardCount"]);
                    if (Convert.ToInt32(row["f_ShiftID"]) > 0)
                    {
                        drAttStatistic["f_DayShouldWork"] = Convert.ToInt32(drAttStatistic["f_DayShouldWork"]) + 1;
                        if ((((this.SetObjToStr(row["f_OnDuty1"]) != "") && (this.SetObjToStr(row["f_OffDuty1"]) != "")) && ((Convert.ToInt32(row["f_LateMinutes"], CultureInfo.InvariantCulture) == 0) && (Convert.ToInt32(row["f_LeaveEarlyMinutes"], CultureInfo.InvariantCulture) == 0))) && ((Convert.ToInt32(row["f_NotReadCardCount"]) == 0) && (Convert.ToDecimal(row["f_AbsenceDays"], CultureInfo.InvariantCulture) == 0M)))
                        {
                            flag2 = true;
                        }
                    }
                    if (CommonStr.strOvertime == this.SetObjToStr(row["f_OnDuty" + (Conversion.Int(0) + 1) + "AttDesc"]))
                    {
                        num4 = 1;
                        while (num4 <= num3)
                        {
                            str = this.SetObjToStr(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]);
                            if ("" == str)
                            {
                                flag2 = false;
                                break;
                            }
                            str = this.SetObjToStr(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1)]);
                            if ("" == str)
                            {
                                flag2 = false;
                                break;
                            }
                            num4 += 2;
                        }
                    }
                    if (num3 == 0)
                    {
                        flag2 = false;
                    }
                    for (num4 = 1; num4 <= num3; num4 += 2)
                    {
                        if (this.bStopCreate)
                        {
                            return num;
                        }
                        str = this.SetObjToStr(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "CardRecordDesc"]);
                        if (str != "")
                        {
                            if (str == CommonStr.strSignIn)
                            {
                                drAttStatistic["f_ManualReadTimesCount"] = Convert.ToInt32(drAttStatistic["f_ManualReadTimesCount"]) + 1;
                            }
                            else if (CommonStr.strOvertime != str)
                            {
                                flag2 = false;
                            }
                        }
                        str = this.SetObjToStr(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "CardRecordDesc"]);
                        if (str != "")
                        {
                            if (str == CommonStr.strSignIn)
                            {
                                drAttStatistic["f_ManualReadTimesCount"] = Convert.ToInt32(drAttStatistic["f_ManualReadTimesCount"]) + 1;
                            }
                            else if (CommonStr.strOvertime != str)
                            {
                                flag2 = false;
                            }
                        }
                        str = this.SetObjToStr(row["f_OnDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "AttDesc"]);
                        if (str != "")
                        {
                            if (str == CommonStr.strLateness)
                            {
                                drAttStatistic["f_LateCount"] = Convert.ToInt32(drAttStatistic["f_LateCount"]) + 1;
                            }
                            else if (str == CommonStr.strLeaveEarly)
                            {
                                drAttStatistic["f_LeaveEarlyCount"] = Convert.ToInt32(drAttStatistic["f_LeaveEarlyCount"]) + 1;
                            }
                        }
                        str = this.SetObjToStr(row["f_OffDuty" + (Conversion.Int((int) (num4 / 2)) + 1) + "AttDesc"]);
                        if (str != "")
                        {
                            if (str == CommonStr.strLateness)
                            {
                                drAttStatistic["f_LateCount"] = Convert.ToInt32(drAttStatistic["f_LateCount"]) + 1;
                            }
                            else if (str == CommonStr.strLeaveEarly)
                            {
                                drAttStatistic["f_LeaveEarlyCount"] = Convert.ToInt32(drAttStatistic["f_LeaveEarlyCount"]) + 1;
                            }
                        }
                    }
                    if (flag2)
                    {
                        drAttStatistic["f_DayRealWork"] = Convert.ToInt32(drAttStatistic["f_DayRealWork"]) + 1;
                    }
                    if (i == (dtAttReport.Rows.Count - 1))
                    {
                        drAttStatistic["f_AttDateEnd"] = row["f_ShiftDate"];
                        this.shift_AttStatistic_updatebyLeave(drAttStatistic);
                        dtAttStatistic.Rows.Add(drAttStatistic);
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

        public int shift_AttStatistic_updatebyLeave(DataRow drAttStatistic)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            DateTime now = DateTime.Now;
            this.dsAtt = new DataSet();
            this.errInfo = "";
            int num = -1;
            try
            {
                DataRow row = drAttStatistic;
                DateTime time = DateTime.Parse(Strings.Format(row["f_AttDateStart"], "yyyy-MM-dd 12:00:00"));
                DateTime time2 = DateTime.Parse(Strings.Format(row["f_AttDateEnd"], "yyyy-MM-dd 12:00:00"));
                int num2 = Convert.ToInt32(row["f_ConsumerID"]);
                if (time > time2)
                {
                    return num;
                }
                string selectCommandText = "SELECT * FROM t_d_Leave ";
                selectCommandText = (selectCommandText + " WHERE f_ConsumerID = " + num2) + " ORDER BY f_Value,f_Value1  ";
                this.daLeave = new OleDbDataAdapter(selectCommandText, this.cn);
                this.daLeave.Fill(this.dsAtt, "Leave");
                this.dtLeave = this.dsAtt.Tables["Leave"];
                if (this.dtLeave.Rows.Count <= 0)
                {
                    return 0;
                }
                int num3 = 0;
                time = DateTime.Parse(Strings.Format(row["f_AttDateStart"], "yyyy-MM-dd 08:00:00"));
                time2 = DateTime.Parse(Strings.Format(row["f_AttDateEnd"], "yyyy-MM-dd 20:00:00"));
                object obj2 = time;
                this.daHolidayType.Fill(this.dsAtt, "HolidayType");
                this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
                localizedHolidayType(this.dtHolidayType);
            Label_0193:
                if (Convert.ToDateTime(obj2) > time2)
                {
                    goto Label_0452;
                }
                num3 = 0;
            Label_01A8:
                if (num3 < this.dtLeave.Rows.Count)
                {
                    string str2 = Convert.ToString(this.dtLeave.Rows[num3]["f_HolidayType"]);
                    this.strTemp = Convert.ToString(this.dtLeave.Rows[num3]["f_Value"]);
                    this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num3]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                    DateTime time3 = DateTime.Parse(this.strTemp);
                    this.strTemp = Convert.ToString(this.dtLeave.Rows[num3]["f_Value2"]);
                    this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num3]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                    DateTime time4 = DateTime.Parse(this.strTemp);
                    if ((Convert.ToDateTime(obj2) >= time3) && (Convert.ToDateTime(obj2) <= time4))
                    {
                        bool flag = false;
                        for (int i = 0; i <= (this.dtHolidayType.Rows.Count - 1); i++)
                        {
                            if (str2 == this.dtHolidayType.Rows[i][1].ToString())
                            {
                                if (row["f_SpecialType" + (i + 1)].ToString() == "")
                                {
                                    row["f_SpecialType" + (i + 1)] = 0;
                                }
                                row["f_SpecialType" + (i + 1)] = Convert.ToDecimal(row["f_SpecialType" + (i + 1)], CultureInfo.InvariantCulture) + 0.5M;
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            goto Label_042D;
                        }
                        num3++;
                        goto Label_01A8;
                    }
                    if (Convert.ToDateTime(obj2) >= time3)
                    {
                        num3++;
                        goto Label_01A8;
                    }
                }
            Label_042D:
                obj2 = Convert.ToDateTime(obj2).AddHours(12.0);
                goto Label_0193;
            Label_0452:
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
            return num;
        }

        public int shift_AttStatistic_writetodb(DataTable dtAttStatistic)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            string str = "";
            this.cmd = new OleDbCommand();
            bool flag = true;
            this.errInfo = "";
            int num2 = -1;
            try
            {
                if (dtAttStatistic.Rows.Count > 0)
                {
                    this.cmd.Connection = this.cn;
                    this.cmd.CommandType = CommandType.Text;
                    for (int i = 0; i <= (dtAttStatistic.Rows.Count - 1); i++)
                    {
                        if (this.bStopCreate)
                        {
                            return num2;
                        }
                        DataRow row = dtAttStatistic.Rows[i];
                        str = " INSERT INTO t_d_Shift_AttStatistic ";
                        str = str + " ( f_ConsumerID ";
                        str = str + " , f_AttDateStart, f_AttDateEnd, f_DayShouldWork ";
                        str = str + " , f_DayRealWork";
                        str = str + " , f_LateMinutes,f_LateCount ";
                        str = str + " , f_LeaveEarlyMinutes,f_LeaveEarlyCount ";
                        str = str + " , f_OvertimeHours ";
                        str = str + " , f_AbsenceDays ";
                        str = str + " , f_NotReadCardCount, f_ManualReadTimesCount ";
                        int num4 = 1;
                        while (num4 <= 0x20)
                        {
                            str = str + " , f_SpecialType" + num4.ToString();
                            num4++;
                        }
                        str = str + " )";
                        str = str + " Values ( " + row["f_ConsumerID"];
                        str = str + "," + this.PrepareStr(row["f_AttDateStart"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + this.PrepareStr(row["f_AttDateEnd"], true, "yyyy-MM-dd HH:mm:ss");
                        str = str + "," + row["f_DayShouldWork"];
                        str = str + "," + row["f_DayRealWork"];
                        str = str + "," + row["f_LateMinutes"];
                        str = str + "," + row["f_LateCount"];
                        str = str + "," + row["f_LeaveEarlyMinutes"];
                        str = str + "," + row["f_LeaveEarlyCount"];
                        str = str + "," + row["f_OvertimeHours"];
                        str = str + "," + row["f_AbsenceDays"];
                        str = str + "," + row["f_NotReadCardCount"];
                        str = str + "," + row["f_ManualReadTimesCount"];
                        for (num4 = 1; num4 <= 0x20; num4++)
                        {
                            str = str + " ," + this.PrepareStr(row["f_SpecialType" + num4.ToString()]);
                        }
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

        public int shift_checkvalid(int id, string name, int readtimes, object onduty1, object offduty1, object onduty2, object offduty2, object onduty3, object offduty3, object onduty4, object offduty4)
        {
            int num = -1;
            this.errInfo = "";
            try
            {
                if (id > 0x63)
                {
                    num = -2;
                    this.errInfo = id.ToString();
                    return num;
                }
                if (((readtimes != 2) && (readtimes != 4)) && ((readtimes != 6) && (readtimes != 8)))
                {
                    num = -3;
                    this.errInfo = num.ToString();
                    return num;
                }
                int num2 = 0;
                int totalMinutes = 0;
                this.realOnduty1 = DateTime.Parse(Strings.Format(onduty1, "2000-1-1 HH:mm:ss"));
                if (this.tm(onduty1) > this.tm(offduty1))
                {
                    num2++;
                }
                this.realOffduty1 = DateTime.Parse(Strings.Format(offduty1, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOffduty1.Subtract(this.realOnduty1).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                if (readtimes <= 2)
                {
                    return 0;
                }
                if (this.tm(offduty1) > this.tm(onduty2))
                {
                    num2++;
                }
                this.realOnduty2 = DateTime.Parse(Strings.Format(onduty2, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOnduty2.Subtract(this.realOffduty1).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                if (this.tm(onduty2) > this.tm(offduty2))
                {
                    num2++;
                }
                this.realOffduty2 = DateTime.Parse(Strings.Format(offduty2, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOffduty2.Subtract(this.realOnduty2).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                totalMinutes = (int) this.realOffduty2.Subtract(this.realOnduty1).TotalMinutes;
                if (totalMinutes >= 0x5a0)
                {
                    return -5;
                }
                if (readtimes <= 4)
                {
                    return 0;
                }
                if (this.tm(offduty2) > this.tm(onduty3))
                {
                    num2++;
                }
                this.realOnduty3 = DateTime.Parse(Strings.Format(onduty3, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOnduty3.Subtract(this.realOffduty2).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                if (this.tm(onduty3) > this.tm(offduty3))
                {
                    num2++;
                }
                this.realOffduty3 = DateTime.Parse(Strings.Format(offduty3, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOffduty3.Subtract(this.realOnduty3).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                totalMinutes = (int) this.realOffduty3.Subtract(this.realOnduty1).TotalMinutes;
                if (totalMinutes >= 0x5a0)
                {
                    return -5;
                }
                if (readtimes <= 6)
                {
                    return 0;
                }
                if (this.tm(offduty3) > this.tm(onduty4))
                {
                    num2++;
                }
                this.realOnduty4 = DateTime.Parse(Strings.Format(onduty4, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOnduty4.Subtract(this.realOffduty3).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                if (this.tm(onduty4) > this.tm(offduty4))
                {
                    num2++;
                }
                this.realOffduty4 = DateTime.Parse(Strings.Format(offduty4, "2000-1-1 HH:mm:ss")).AddDays((double) num2);
                totalMinutes = (int) this.realOffduty4.Subtract(this.realOnduty4).TotalMinutes;
                if (totalMinutes < this.minShifDiffByMinute)
                {
                    return -4;
                }
                totalMinutes = (int) this.realOffduty4.Subtract(this.realOnduty1).TotalMinutes;
                if (totalMinutes >= 0x5a0)
                {
                    return -5;
                }
                if (readtimes <= 8)
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

        public int shift_delete(int id)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            int num = -1;
            try
            {
                string cmdText = " DELETE FROM t_b_ShiftSet WHERE f_ShiftID = " + id;
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(cmdText, this.cn);
                this.cmd.ExecuteNonQuery();
                this.cn.Close();
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
            return num;
        }

        public int shift_rule_checkValid(int ruleLen, int[] shiftRule)
        {
            object[,] objArray;
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            this.errInfo = "";
            int num = -1;
            if (ruleLen > 1)
            {
                objArray = new object[ruleLen + 1, 13];
            }
            else
            {
                return 0;
            }
            num = -1;
            try
            {
                int num2;
                for (num2 = 0; num2 <= (ruleLen - 1); num2++)
                {
                    if (this.bStopCreate)
                    {
                        return num;
                    }
                    string cmdText = " SELECT * FROM t_b_ShiftSet WHERE f_ShiftID = " + shiftRule[num2];
                    if (this.cn.State != ConnectionState.Open)
                    {
                        this.cn.Open();
                    }
                    this.cmd = new OleDbCommand(cmdText, this.cn);
                    OleDbDataReader reader = this.cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        for (int i = 0; i <= (reader.FieldCount - 1); i++)
                        {
                            objArray[num2, i] = reader[i];
                        }
                    }
                    else
                    {
                        objArray[num2, 0] = 0;
                        if (shiftRule[num2] != 0)
                        {
                            num = -6;
                            this.errInfo = shiftRule[num2].ToString();
                            return num;
                        }
                    }
                    reader.Close();
                }
                this.cn.Close();
                for (num2 = 1; num2 <= (ruleLen - 1); num2++)
                {
                    if (this.bStopCreate)
                    {
                        return num;
                    }
                    if ((Convert.ToInt32(objArray[num2 - 1, 0]) != 0) && (Convert.ToInt32(objArray[num2, 0]) != 0))
                    {
                        int num4 = Convert.ToInt32(objArray[num2 - 1, 2]);
                        DateTime time2 = Convert.ToDateTime(objArray[num2 - 1, 2 + num4]);
                        Convert.ToInt32(objArray[num2, 2]);
                        DateTime expression = Convert.ToDateTime(objArray[num2, 3]).AddDays(1.0);
                        if ((expression <= time2) || (expression.Subtract(time2).TotalMinutes < this.minShifDiffByMinute))
                        {
                            num = -7;
                            this.errInfo = string.Concat(new object[] { CommonStr.strShift, shiftRule[num2 - 1], CommonStr.strLastOffDuty, Strings.Format(time2, "HH:mm"), ", " });
                            object errInfo = this.errInfo;
                            this.errInfo = string.Concat(new object[] { errInfo, CommonStr.strShift, shiftRule[num2], CommonStr.strFirstOnDuty, Strings.Format(expression, "HH:mm"), CommonStr.strTimeOverlapped });
                            return num;
                        }
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
            return num;
        }

        public int shift_update(int id, string name, int readtimes, object onduty1, object offduty1, object onduty2, object offduty2, object onduty3, object offduty3, object onduty4, object offduty4, int bOvertimeShift)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            int num = -1;
            this.errInfo = "";
            try
            {
                num = this.shift_checkvalid(id, name, readtimes, onduty1, offduty1, onduty2, offduty2, onduty3, offduty3, onduty4, offduty4);
                if (num != 0)
                {
                    return num;
                }
                string cmdText = " SELECT count(*) FROM t_b_ShiftSet WHERE f_ShiftID = " + id;
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(cmdText, this.cn);
                int num2 = Convert.ToInt32(this.cmd.ExecuteScalar());
                this.cn.Close();
                if (num2 == 0)
                {
                    num = -2;
                    this.errInfo = id.ToString();
                    return num;
                }
                cmdText = " UPDATE t_b_ShiftSet Set ";
                cmdText = (((cmdText + " f_ShiftName = " + this.PrepareStr(name)) + " , f_ReadTimes = " + readtimes) + " , f_OnDuty1 = " + this.PrepareStr(this.realOnduty1, true, "yyyy-MM-dd HH:mm:ss")) + " , f_OffDuty1 = " + this.PrepareStr(this.realOffduty1, true, "yyyy-MM-dd HH:mm:ss");
                if (readtimes > 2)
                {
                    cmdText = (cmdText + " , f_OnDuty2 = " + this.PrepareStr(this.realOnduty2, true, "yyyy-MM-dd HH:mm:ss")) + " , f_OffDuty2 = " + this.PrepareStr(this.realOffduty2, true, "yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    cmdText = (cmdText + " , f_OnDuty2 = " + this.PrepareStr("")) + " , f_OffDuty2 = " + this.PrepareStr("");
                }
                if (readtimes > 4)
                {
                    cmdText = (cmdText + " , f_OnDuty3 = " + this.PrepareStr(this.realOnduty3, true, "yyyy-MM-dd HH:mm:ss")) + " , f_OffDuty3 =" + this.PrepareStr(this.realOffduty3, true, "yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    cmdText = (cmdText + " , f_OnDuty3 = " + this.PrepareStr("")) + " , f_OffDuty3 =" + this.PrepareStr("");
                }
                if (readtimes > 6)
                {
                    cmdText = (cmdText + " , f_OnDuty4  = " + this.PrepareStr(this.realOnduty4, true, "yyyy-MM-dd HH:mm:ss")) + " , f_OffDuty4 = " + this.PrepareStr(this.realOffduty4, true, "yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    cmdText = (cmdText + " , f_OnDuty4  = " + this.PrepareStr("")) + " , f_OffDuty4 = " + this.PrepareStr("");
                }
                cmdText = ((cmdText + " ,f_bOvertimeShift = " + bOvertimeShift) + " ,f_Notes = " + this.PrepareStr("")) + " WHERE  f_ShiftID = " + id;
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                this.cmd = new OleDbCommand(cmdText, this.cn);
                num2 = this.cmd.ExecuteNonQuery();
                this.cn.Close();
                if (num2 > 0)
                {
                    return 0;
                }
                return -1;
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
            return num;
        }

        public int shift_work_schedule_analyst(DataTable dtShiftWorkSchedule)
        {
            this.errInfo = "";
            int num = -1;
            try
            {
                int num6 = 0;
                for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                {
                    if (this.bStopCreate)
                    {
                        return num;
                    }
                    object expression = dtShiftWorkSchedule.Rows[i]["f_PlanTime"];
                    if (!Information.IsDBNull(expression))
                    {
                        int num4 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]);
                        int num3 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_ReadTimes"]);
                        int num5 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]) & 1;
                        if (Information.IsDBNull(dtShiftWorkSchedule.Rows[i]["f_WorkTime"]))
                        {
                            dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strNotReadCard;
                        }
                        else
                        {
                            object obj3 = dtShiftWorkSchedule.Rows[i]["f_WorkTime"];
                            TimeSpan span = Convert.ToDateTime(expression).Subtract(Convert.ToDateTime(obj3));
                            dtShiftWorkSchedule.Rows[i]["f_Duration"] = span.Duration().TotalMinutes;
                            if ((num5 == 0) && (span.TotalMinutes < -this.tLateTimeout))
                            {
                                dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strLateness;
                            }
                            if (num5 == 1)
                            {
                                if (span.TotalMinutes > this.tLeaveTimeout)
                                {
                                    dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strLeaveEarly;
                                }
                                else if ((num4 == (num3 - 1)) && (span.TotalMinutes <= -this.tOvertimeTimeout))
                                {
                                    dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strOvertime;
                                }
                            }
                        }
                        if (num4 == 0)
                        {
                            num6 = i;
                        }
                        if (num4 == (num3 - 1))
                        {
                            int num7;
                            if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_bOvertimeShift"]) == "1")
                            {
                                num7 = num6;
                                while (num7 <= i)
                                {
                                    if ((this.SetObjToStr(dtShiftWorkSchedule.Rows[num7]["f_CardRecordDesc"]) == CommonStr.strSignIn) || (this.SetObjToStr(dtShiftWorkSchedule.Rows[num7]["f_CardRecordDesc"]) == ""))
                                    {
                                        dtShiftWorkSchedule.Rows[num7]["f_AttDesc"] = CommonStr.strOvertime;
                                    }
                                    num7++;
                                }
                                continue;
                            }
                            if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_bOvertimeShift"]) == "2")
                            {
                                if (((this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"]) == CommonStr.strNotReadCard) && (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) == "")) && ((this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_AttDesc"]) == CommonStr.strNotReadCard) && (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"]) == "")))
                                {
                                    dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"] = "";
                                    dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = "";
                                }
                                else if ((this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) == CommonStr.strSignIn) || (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) == ""))
                                {
                                    if ((this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"]) != CommonStr.strNotReadCard) || (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_CardRecordDesc"]) != ""))
                                    {
                                        dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"] = CommonStr.strOvertime;
                                    }
                                    else if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"]) == CommonStr.strNotReadCard)
                                    {
                                        dtShiftWorkSchedule.Rows[i - 1]["f_AttDesc"] = "";
                                    }
                                    if ((this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_AttDesc"]) != CommonStr.strNotReadCard) || (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"]) != ""))
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = CommonStr.strOvertime;
                                    }
                                    else if (this.SetObjToStr(dtShiftWorkSchedule.Rows[i]["f_AttDesc"]) == CommonStr.strNotReadCard)
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = "";
                                    }
                                }
                                continue;
                            }
                            bool flag2 = false;
                            num7 = num6;
                            while (num7 <= i)
                            {
                                if ((this.SetObjToStr(dtShiftWorkSchedule.Rows[num7]["f_AttDesc"]) != CommonStr.strNotReadCard) || (this.SetObjToStr(dtShiftWorkSchedule.Rows[num7]["f_CardRecordDesc"]) != ""))
                                {
                                    flag2 = true;
                                    break;
                                }
                                num7++;
                            }
                            if (!flag2)
                            {
                                for (num7 = num6; num7 <= i; num7++)
                                {
                                    dtShiftWorkSchedule.Rows[num7]["f_AttDesc"] = CommonStr.strAbsence;
                                }
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
                if (wgAppConfig.runUpdateSql("Delete From t_d_Shift_Work_Schedule") >= 0)
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
            this.dtShiftWork = new DataTable("t_d_ShiftWork");
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
                this.dc.ColumnName = "f_ShiftDate";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_ShiftID";
                this.dc.DefaultValue = -1;
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_Readtimes";
                this.dc.DefaultValue = 0;
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_PlanTime";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_TimeSeg";
                this.dc.DefaultValue = 0;
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_WorkTime";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.String");
                this.dc.ColumnName = "f_AttDesc";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.String");
                this.dc.ColumnName = "f_CardRecordDesc";
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_Duration";
                this.dc.DefaultValue = 0;
                this.dtShiftWork.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Int32");
                this.dc.ColumnName = "f_bOvertimeShift";
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
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
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
                        OleDbDataReader reader;
                        if (str2 != Strings.Format(expression, "yyyy-MM"))
                        {
                            str2 = Strings.Format(expression, "yyyy-MM");
                            str = " SELECT * FROM t_d_ShiftData ";
                            str = (str + " WHERE f_ConsumerID = " + consumerid) + " AND f_DateYM = " + this.PrepareStr(str2);
                            if (this.cn.State != ConnectionState.Open)
                            {
                                this.cn.Open();
                            }
                            this.cmd = new OleDbCommand(str, this.cn);
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
                            DataRow row;
                            if (!flag)
                            {
                                row = dtShiftWorkSchedule.NewRow();
                                row[0] = consumerid;
                                row[1] = expression;
                                row["f_ShiftID"] = -1;
                                row["f_Readtimes"] = 0;
                                row["f_Duration"] = 0;
                                dtShiftWorkSchedule.Rows.Add(row);
                            }
                            else
                            {
                                int num2 = Convert.ToInt32(objArray[2 + expression.Day]);
                                if (num2 <= 0)
                                {
                                    row = dtShiftWorkSchedule.NewRow();
                                    row[0] = consumerid;
                                    row[1] = expression;
                                    row[2] = num2;
                                    row["f_Readtimes"] = 0;
                                    row["f_Duration"] = 0;
                                    switch (num2)
                                    {
                                        case 0:
                                            row["f_AttDesc"] = CommonStr.strRest;
                                            break;

                                        case -1:
                                            bNotArranged |= 2;
                                            break;
                                    }
                                    dtShiftWorkSchedule.Rows.Add(row);
                                }
                                else
                                {
                                    str = "SELECT * FROM t_b_ShiftSet WHERE f_ShiftID = " + num2;
                                    this.cmd = new OleDbCommand(str, this.cn);
                                    if (this.cn.State != ConnectionState.Open)
                                    {
                                        this.cn.Open();
                                    }
                                    reader = this.cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        for (int j = 1; j <= Convert.ToInt32(reader["f_ReadTimes"]); j++)
                                        {
                                            row = dtShiftWorkSchedule.NewRow();
                                            row["f_Readtimes"] = 0;
                                            row[0] = consumerid;
                                            row[1] = expression;
                                            row[2] = num2;
                                            row["f_Readtimes"] = reader["f_ReadTimes"];
                                            row["f_PlanTime"] = Convert.ToDateTime(Strings.Format(expression.AddDays((double) (Convert.ToInt32(Strings.Format(reader[j + 2], "dd")) - 1)), "yyyy-MM-dd") + Strings.Format(reader[j + 2], " HH:mm:ss"));
                                            row["f_TimeSeg"] = j - 1;
                                            row["f_Duration"] = 0;
                                            row["f_bOvertimeShift"] = reader["f_bOvertimeShift"];
                                            dtShiftWorkSchedule.Rows.Add(row);
                                        }
                                    }
                                    else
                                    {
                                        row = dtShiftWorkSchedule.NewRow();
                                        row[0] = consumerid;
                                        row[1] = expression;
                                        row[2] = num2;
                                        row[3] = -1;
                                        row["f_Readtimes"] = -1;
                                        row["f_Duration"] = 0;
                                        dtShiftWorkSchedule.Rows.Add(row);
                                        bNotArranged |= 4;
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

        public int shift_work_schedule_updatebyLeave(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            DateTime now = DateTime.Now;
            this.dsAtt = new DataSet();
            this.errInfo = "";
            int num = -1;
            DateTime time = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time <= time2)
            {
                try
                {
                    string selectCommandText = "SELECT * FROM t_d_Leave ";
                    selectCommandText = (selectCommandText + " WHERE f_ConsumerID = " + consumerid) + " ORDER BY f_Value,f_Value1  ";
                    this.daLeave = new OleDbDataAdapter(selectCommandText, this.cn);
                    this.daLeave.Fill(this.dsAtt, "Leave");
                    this.dtLeave = this.dsAtt.Tables["Leave"];
                    if (this.dtLeave.Rows.Count <= 0)
                    {
                        return 0;
                    }
                    for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                    {
                        if (this.bStopCreate)
                        {
                            return num;
                        }
                        object expression = dtShiftWorkSchedule.Rows[i]["f_PlanTime"];
                        if (Information.IsDBNull(expression))
                        {
                            int num3 = 0;
                            expression = dtShiftWorkSchedule.Rows[i]["f_shiftDate"];
                            while (true)
                            {
                                DateTime time3;
                                DateTime time4;
                                if (num3 >= this.dtLeave.Rows.Count)
                                {
                                    continue;
                                }
                                string str2 = Convert.ToString(this.dtLeave.Rows[num3]["f_HolidayType"]);
                                this.strTemp = Convert.ToString(this.dtLeave.Rows[num3]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num3]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime.TryParse(this.strTemp, out time3);
                                this.strTemp = Convert.ToString(this.dtLeave.Rows[num3]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num3]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime.TryParse(this.strTemp, out time4);
                                if ((Convert.ToDateTime(expression) >= time3) && (Convert.ToDateTime(expression) <= time4))
                                {
                                    dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = str2;
                                    dtShiftWorkSchedule.Rows[i]["f_AttDesc"] = str2;
                                    continue;
                                }
                                num3++;
                            }
                        }
                        Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]);
                        Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_ReadTimes"]);
                        int num4 = 0;
                        while (true)
                        {
                            DateTime time5;
                            DateTime time6;
                            if (num4 >= this.dtLeave.Rows.Count)
                            {
                                break;
                            }
                            string str3 = Convert.ToString(this.dtLeave.Rows[num4]["f_HolidayType"]);
                            this.strTemp = Convert.ToString(this.dtLeave.Rows[num4]["f_Value"]);
                            this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num4]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                            DateTime.TryParse(this.strTemp, out time5);
                            this.strTemp = Convert.ToString(this.dtLeave.Rows[num4]["f_Value2"]);
                            this.strTemp = this.strTemp + " " + ((this.dtLeave.Rows[num4]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                            DateTime.TryParse(this.strTemp, out time6);
                            if ((Convert.ToDateTime(expression) >= time5) && (Convert.ToDateTime(expression) <= time6))
                            {
                                dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = expression;
                                dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = str3;
                                break;
                            }
                            num4++;
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

        public int shift_work_schedule_updatebyManualReadcard(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            this.dsAtt = new DataSet();
            this.errInfo = "";
            int num = -1;
            DateTime time = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time <= time2)
            {
                try
                {
                    string selectCommandText = "SELECT t_d_ManualCardRecord.f_ConsumerID,  t_d_ManualCardRecord.f_ReadDate, t_d_ManualCardRecord.f_DutyOnOff FROM t_d_ManualCardRecord ";
                    selectCommandText = (((selectCommandText + " WHERE f_ConsumerID = " + consumerid) + " AND ([f_ReadDate]>= " + this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00") + ") ") + " AND ([f_ReadDate]<= " + this.PrepareStr(dateEnd.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + " ORDER BY f_ReadDate ASC ";
                    this.daCardRecord = new OleDbDataAdapter(selectCommandText, this.cn);
                    this.daCardRecord.Fill(this.dsAtt, "CardRecord");
                    DataTable table = this.dsAtt.Tables["CardRecord"];
                    this.daCardRecord = new OleDbDataAdapter("SELECT t_d_ManualCardRecord.f_ConsumerID,  t_d_ManualCardRecord.f_ReadDate, t_d_ManualCardRecord.f_DutyOnOff FROM t_d_ManualCardRecord WHERE 1<0 ", this.cn);
                    this.daCardRecord.Fill(this.dsAtt, "ValidCardRecord");
                    DataTable table2 = this.dsAtt.Tables["ValidCardRecord"];
                    if (table.Rows.Count > 0)
                    {
                        object[] array = new object[(table.Columns.Count - 1) + 1];
                        DateTime time4 = Convert.ToDateTime(table.Rows[0]["f_ReadDate"]);
                        DataRow row = table2.NewRow();
                        table.Rows[0].ItemArray.CopyTo(array, 0);
                        row.ItemArray = array;
                        table2.Rows.Add(row);
                        time4 = Convert.ToDateTime(row["f_ReadDate"]);
                        for (int j = 1; j <= (table.Rows.Count - 1); j++)
                        {
                            if (this.bStopCreate)
                            {
                                return num;
                            }
                            DateTime time3 = Convert.ToDateTime(table.Rows[j]["f_ReadDate"]);
                            if (time3.Subtract(time4).TotalSeconds > this.tTwoReadMintime)
                            {
                                time4 = time3;
                                row = table2.NewRow();
                                table.Rows[j].ItemArray.CopyTo(array, 0);
                                row.ItemArray = array;
                                table2.Rows.Add(row);
                            }
                        }
                    }
                    int num4 = 0;
                    for (int i = 0; i <= (dtShiftWorkSchedule.Rows.Count - 1); i++)
                    {
                        if (this.bStopCreate)
                        {
                            return num;
                        }
                        object expression = dtShiftWorkSchedule.Rows[i]["f_PlanTime"];
                        if (Information.IsDBNull(expression))
                        {
                            continue;
                        }
                        //bool flag = false;
                        int num6 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]);
                        int num5 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_ReadTimes"]);
                    Label_031F:
                        if (num4 < table2.Rows.Count)
                        {
                            object obj3 = table2.Rows[num4]["f_ReadDate"];
                            TimeSpan span2 = Convert.ToDateTime(expression).Subtract(Convert.ToDateTime(obj3));
                            if (num6 == 0)
                            {
                                if (span2.TotalMinutes > this.tAheadMinutesOnDutyFirst)
                                {
                                    num4++;
                                }
                                else
                                {
                                    if (span2.TotalMinutes > -this.tDelayMinutes)
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                        dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = CommonStr.strSignIn;
                                        num4++;
                                        //flag = true;
                                    }
                                    continue;
                                }
                            }
                            if (num6 == (num5 - 1))
                            {
                                if (span2.TotalMinutes < -this.tOvertimeMinutes)
                                {
                                    continue;
                                }
                                if (span2.TotalMinutes > this.tAheadMinutes)
                                {
                                    num4++;
                                }
                                else if (span2.TotalMinutes > -this.tDelayMinutes)
                                {
                                    dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                    dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = CommonStr.strSignIn;
                                    num4++;
                                    //flag = true;
                                }
                                else
                                {
                                    if ((((i + 1) < (dtShiftWorkSchedule.Rows.Count - 1)) && !Information.IsDBNull(dtShiftWorkSchedule.Rows[i + 1]["f_PlanTime"])) && (Convert.ToDateTime(dtShiftWorkSchedule.Rows[i + 1]["f_PlanTime"]).AddMinutes((double) -this.tAheadMinutesOnDutyFirst) < Convert.ToDateTime(obj3)))
                                    {
                                        continue;
                                    }
                                    dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                    dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = CommonStr.strSignIn;
                                    num4++;
                                    //flag = true;
                                }
                            }
                            if ((num6 > 0) && (num6 < (num5 - 1)))
                            {
                                if (span2.TotalMinutes <= this.tAheadMinutes)
                                {
                                    if (span2.TotalMinutes >= -this.tDelayMinutes)
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                        dtShiftWorkSchedule.Rows[i]["f_CardRecordDesc"] = CommonStr.strSignIn;
                                        num4++;
                                        //flag = true;
                                        if (((num6 & 1) != 0) && (span2.TotalMinutes > 0.0))
                                        {
                                            goto Label_031F;
                                        }
                                    }
                                    continue;
                                }
                                num4++;
                            }
                            goto Label_031F;
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

        public int shift_work_schedule_updatebyReadcard(int consumerid, DataTable dtShiftWorkSchedule, DateTime dateStart, DateTime dateEnd)
        {
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            this.dsAtt = new DataSet();
            this.errInfo = "";
            int num = -1;
            DateTime time = DateTime.Parse(Strings.Format(dateStart, "yyyy-MM-dd 12:00:00"));
            DateTime time2 = DateTime.Parse(Strings.Format(dateEnd, "yyyy-MM-dd 12:00:00"));
            if (time <= time2)
            {
                try
                {
                    string str = "SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_DutyOnOff  FROM t_d_SwipeRecord, t_b_Reader ";
                    str = (((str + " WHERE f_ConsumerID = " + consumerid) + 
                        " AND ([f_ReadDate]>= " + this.PrepareStr(dateStart, true, "yyyy-MM-dd 00:00:00") + ") ") + 
                        " AND ([f_ReadDate]<= " + this.PrepareStr(dateEnd.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + 
                        " AND (t_d_SwipeRecord.f_ReaderID = t_b_Reader.f_ReaderID) " + 
                        " AND t_b_Reader.f_Attend = 1 ";
                    if (wgAppConfig.getSystemParamByNO(0x36) == "1")
                    {
                        str = str + " AND f_Character >= 1 ";
                    }
                    str = str + " ORDER BY f_ReadDate ASC, f_RecID ASC ";
                    this.cmd = new OleDbCommand();
                    this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
                    this.cmd.Connection = this.cn;
                    this.cmd.CommandText = str;
                    this.cmd.CommandType = CommandType.Text;
                    this.daCardRecord = new OleDbDataAdapter(this.cmd);
                    this.daCardRecord.Fill(this.dsAtt, "CardRecord");
                    this.dtCardRecord = this.dsAtt.Tables["CardRecord"];
                    this.daCardRecord = new OleDbDataAdapter("SELECT t_d_SwipeRecord.f_ConsumerID, t_d_SwipeRecord.f_ReaderID, t_d_SwipeRecord.f_ReadDate, t_b_Reader.f_DutyOnOff FROM t_d_SwipeRecord, t_b_Reader WHERE 1<0  ", this.cn);
                    this.daCardRecord.Fill(this.dsAtt, "ValidCardRecord");
                    this.dtValidCardRecord = this.dsAtt.Tables["ValidCardRecord"];
                    if (this.dtCardRecord.Rows.Count > 0)
                    {
                        object[] array = new object[(this.dtCardRecord.Columns.Count - 1) + 1];
                        DateTime time4 = Convert.ToDateTime(this.dtCardRecord.Rows[0]["f_ReadDate"]);
                        int num2 = Convert.ToInt32(this.dtCardRecord.Rows[0]["f_DutyOnOff"]);
                        DataRow row = this.dtValidCardRecord.NewRow();
                        this.dtCardRecord.Rows[0].ItemArray.CopyTo(array, 0);
                        row.ItemArray = array;
                        this.dtValidCardRecord.Rows.Add(row);
                        time4 = Convert.ToDateTime(row["f_ReadDate"]);
                        num2 = Convert.ToInt32(row["f_DutyOnOff"]);
                        for (int j = 1; j <= (this.dtCardRecord.Rows.Count - 1); j++)
                        {
                            if (this.bStopCreate)
                            {
                                return num;
                            }
                            DateTime time3 = Convert.ToDateTime(this.dtCardRecord.Rows[j]["f_ReadDate"]);
                            TimeSpan span = time3.Subtract(time4);
                            int num3 = Convert.ToInt32(this.dtCardRecord.Rows[j]["f_DutyOnOff"]);
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
                        object expression = dtShiftWorkSchedule.Rows[i]["f_PlanTime"];
                        if (Information.IsDBNull(expression))
                        {
                            continue;
                        }
                        //bool flag = false;
                        int num8 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_TimeSeg"]);
                        int num7 = Convert.ToInt32(dtShiftWorkSchedule.Rows[i]["f_ReadTimes"]);
                    Label_042F:
                        if (num6 < this.dtValidCardRecord.Rows.Count)
                        {
                            object obj3 = this.dtValidCardRecord.Rows[num6]["f_ReadDate"];
                            TimeSpan span2 = Convert.ToDateTime(expression).Subtract(Convert.ToDateTime(obj3));
                            if (num8 == 0)
                            {
                                if (span2.TotalMinutes > this.tAheadMinutesOnDutyFirst)
                                {
                                    num6++;
                                }
                                else
                                {
                                    if (span2.TotalMinutes <= -this.tDelayMinutes)
                                    {
                                        continue;
                                    }
                                    if ((Convert.ToInt32(this.dtValidCardRecord.Rows[num6]["f_DutyOnOff"]) & 1) == 1)
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                        //flag = true;
                                        continue;
                                    }
                                    num6++;
                                }
                            }
                            if (num8 == (num7 - 1))
                            {
                                if (span2.TotalMinutes < -this.tOvertimeMinutes)
                                {
                                    continue;
                                }
                                if (span2.TotalMinutes > this.tAheadMinutes)
                                {
                                    num6++;
                                }
                                else if (span2.TotalMinutes > -this.tDelayMinutes)
                                {
                                    if ((Convert.ToInt32(this.dtValidCardRecord.Rows[num6]["f_DutyOnOff"]) & 2) == 2)
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                        num6++;
                                        //flag = true;
                                    }
                                    else
                                    {
                                        num6++;
                                    }
                                }
                                else
                                {
                                    if (((((i + 1) < (dtShiftWorkSchedule.Rows.Count - 1)) && !Information.IsDBNull(dtShiftWorkSchedule.Rows[i + 1]["f_PlanTime"])) && (Convert.ToDateTime(dtShiftWorkSchedule.Rows[i + 1]["f_PlanTime"]).AddMinutes((double) -this.tAheadMinutesOnDutyFirst) < Convert.ToDateTime(obj3))) || (span2.TotalMinutes < -this.tOvertimeMinutes))
                                    {
                                        continue;
                                    }
                                    if ((Convert.ToInt32(this.dtValidCardRecord.Rows[num6]["f_DutyOnOff"]) & 2) == 2)
                                    {
                                        dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                        num6++;
                                        //flag = true;
                                    }
                                    else
                                    {
                                        num6++;
                                    }
                                }
                            }
                            if ((num8 > 0) && (num8 < (num7 - 1)))
                            {
                                if (span2.TotalMinutes <= this.tAheadMinutes)
                                {
                                    if (span2.TotalMinutes < -this.tDelayMinutes)
                                    {
                                        continue;
                                    }
                                    if ((num8 & 1) == 0)
                                    {
                                        if ((Convert.ToInt32(this.dtValidCardRecord.Rows[num6]["f_DutyOnOff"]) & 1) == 1)
                                        {
                                            dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                            num6++;
                                            //flag = true;
                                            continue;
                                        }
                                        num6++;
                                    }
                                    else
                                    {
                                        if ((Convert.ToInt32(this.dtValidCardRecord.Rows[num6]["f_DutyOnOff"]) & 2) == 2)
                                        {
                                            dtShiftWorkSchedule.Rows[i]["f_WorkTime"] = obj3;
                                            num6++;
                                            //flag = true;
                                            if (span2.TotalMinutes > 0.0)
                                            {
                                                goto Label_042F;
                                            }
                                            continue;
                                        }
                                        num6++;
                                    }
                                }
                                else
                                {
                                    num6++;
                                }
                            }
                            goto Label_042F;
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
            this.cn = new OleDbConnection(wgAppConfig.dbConString);
            this.cmd = new OleDbCommand();
            string str = "";
            bool flag = true;
            this.errInfo = "";
            int num2 = -1;
            try
            {
                if (dtShiftWorkSchedule.Rows.Count > 0)
                {
                    using (this.cmd = new OleDbCommand())
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
                            str = " INSERT INTO t_d_Shift_Work_Schedule ";
                            str = str + " ( f_ConsumerID, f_shiftDate, f_ShiftID, f_ReadTimes, f_PlanTime, f_TimeSeg, f_WorkTime, f_AttDesc";
                            str = str + " , f_CardRecordDesc, f_Duration, f_bOvertimeShift";
                            str = str + " ) ";
                            str = str + " Values ( " + row["f_ConsumerID"];
                            str = str + "," + this.PrepareStr(row["f_shiftDate"], true, "yyyy-MM-dd");
                            str = str + "," + row["f_ShiftID"];
                            str = str + "," + row["f_ReadTimes"];
                            str = str + "," + this.PrepareStr(row["f_PlanTime"], true, "yyyy-MM-dd HH:mm:ss");
                            str = str + "," + row["f_TimeSeg"];
                            str = str + "," + this.PrepareStr(row["f_WorkTime"], true, "yyyy-MM-dd HH:mm:ss");
                            str = str + "," + this.PrepareStr(row["f_AttDesc"]);
                            str = str + "," + this.PrepareStr(row["f_CardRecordDesc"]);
                            str = str + "," + row["f_Duration"];
                            str = str + "," + row["f_bOvertimeShift"];
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
                                goto Label_026C;
                            }
                        }
                    }
                }
            Label_026C:
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

