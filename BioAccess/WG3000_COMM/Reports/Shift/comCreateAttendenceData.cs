namespace WG3000_COMM.Reports.Shift
{
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;

    public class comCreateAttendenceData : Component
    {
        public bool bChooseOnlyOnDuty;
        public bool bChooseTwoTimes;
        public bool bEarliestAsOnDuty;
        public bool bStopCreate;
        private SqlCommand cmd;
        private SqlCommand cmdConsumer;
        private SqlConnection cn;
        private SqlConnection cnConsumer;
        private Container components;
        public string consumerName;
        private SqlDataAdapter daAttendenceData;
        private SqlDataAdapter daCardRecord;
        private SqlDataAdapter daHoliday;
        private SqlDataAdapter daHolidayType;
        private SqlDataAdapter daLeave;
        private SqlDataAdapter daManualCardRecord;
        private SqlDataAdapter daNoCardRecord;
        private const string DEFAULT_ALLOWOFFDUTYTIME = "23:59:59";
        private const string DEFAULT_ALLOWONDUTYTIME = "00:00:00";
        private const string DEFAULT_ALLOWONDUTYTIME2 = "00:00";
        private DataSet dsAtt;
        private DataTable dtAttendenceData;
        private DataTable dtCardRecord;
        private DataTable dtCardRecord1;
        private DataTable dtCardRecord2;
        private DataTable dtHoliday;
        private DataTable dtHolidayType;
        private DataTable dtLeave;
        private DataView dvCardRecord;
        private DataView dvHoliday;
        private DataView dvLeave;
        public DateTime endDateTime;
        private int gProcVal;
        public string groupName;
        private Thread mainThread;
        public decimal needDutyHour;
        private int normalDay;
        private const int REST_AM = 2;
        private const int REST_ONE_DAY = 0;
        private const int REST_PM = 1;
        public DateTime startDateTime;
        private string strAllowOffdutyTime;
        public string strAllowOndutyTime;
        public string strConsumerSql;
        private string strTemp;
        private decimal tLateAbsenceDay;
        private int tlateAbsenceTimeout;
        private int tLateTimeout;
        private decimal tLeaveAbsenceDay;
        private int tLeaveAbsenceTimeout;
        private int tLeaveTimeout;
        private DateTime tOffduty0;
        private DateTime tOffduty1;
        private DateTime tOffduty2;
        private DateTime tOnduty0;
        private DateTime tOnduty1;
        private DateTime tOnduty2;
        private int tOvertimeTimeout;
        private int tReadCardTimes;
        private int tTwoReadMintime;
        public int userId;
        private const int WORK_AM = 1;
        private const int WORK_ONE_DAY = 3;
        private const int WORK_PM = 2;

        public event CreateCompleteEventHandler CreateCompleteEvent;

        public event DealingNumEventHandler DealingNumEvent;

        public comCreateAttendenceData()
        {
            this.strAllowOndutyTime = "00:00:00";
            this.strAllowOffdutyTime = "23:59:59";
            this.needDutyHour = 8.0M;
            this.strTemp = "";
            this.tReadCardTimes = 2;
            this.tTwoReadMintime = 60;
            this.InitializeComponent();
        }

        public comCreateAttendenceData(IContainer Container) : this()
        {
            Container.Add(this);
        }

        public void _clearAttendenceData()
        {
            string cmdText = "delete from t_d_AttendenceData";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void _clearAttStatistic()
        {
            string cmdText = "delete from t_d_AttStatistic";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
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

        private void getAttendenceParam()
        {
            string cmdText = "SELECT * FROM t_a_Attendence";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (((int) reader["f_No"]) == 1)
                        {
                            this.tLateTimeout = Convert.ToInt32(reader["f_Value"]);
                        }
                        else
                        {
                            if (((int) reader["f_No"]) == 2)
                            {
                                this.tlateAbsenceTimeout = Convert.ToInt32(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 3)
                            {
                                this.tLateAbsenceDay = Convert.ToDecimal(reader["f_Value"], CultureInfo.InvariantCulture);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 4)
                            {
                                this.tLeaveTimeout = Convert.ToInt32(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 5)
                            {
                                this.tLeaveAbsenceTimeout = Convert.ToInt32(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 6)
                            {
                                this.tLeaveAbsenceDay = Convert.ToDecimal(reader["f_Value"], CultureInfo.InvariantCulture);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 7)
                            {
                                this.tOvertimeTimeout = Convert.ToInt32(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 8)
                            {
                                this.tOnduty0 = Convert.ToDateTime(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 9)
                            {
                                this.tOffduty0 = Convert.ToDateTime(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 10)
                            {
                                this.tOnduty1 = Convert.ToDateTime(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 11)
                            {
                                this.tOffduty1 = Convert.ToDateTime(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 12)
                            {
                                this.tOnduty2 = Convert.ToDateTime(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 13)
                            {
                                this.tOffduty2 = Convert.ToDateTime(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 14)
                            {
                                this.tReadCardTimes = Convert.ToInt32(reader["f_Value"]);
                                continue;
                            }
                            if (((int) reader["f_No"]) == 0x10)
                            {
                                this.tTwoReadMintime = Convert.ToInt32(reader["f_Value"]);
                            }
                        }
                    }
                    reader.Close();
                    this.strAllowOndutyTime = wgAppConfig.getSystemParamByNO(0x37).ToString();
                    this.bEarliestAsOnDuty = wgAppConfig.getSystemParamByNO(0x38).ToString() == "1";
                    this.bChooseTwoTimes = wgAppConfig.getSystemParamByNO(0x39).ToString() == "1";
                    this.needDutyHour = decimal.Parse(wgAppConfig.getSystemParamByNO(0x3a).ToString());
                    this.bChooseOnlyOnDuty = wgAppConfig.getSystemParamByNO(0x3b).ToString() == "1";
                }
            }
        }

        public string getDecimalStr(object obj)
        {
            string str = "";
            try
            {
                str = ((decimal) obj).ToString("0.0", CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        private bool InformationIsDate(string str)
        {
            DateTime time;
            return DateTime.TryParse(str, out time);
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

        public void localizedHolidayType(DataTable dt)
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

        private void logCreateReport()
        {
        }

        public void make()
        {
            this.getAttendenceParam();
            if (("00:00:00" != this.strAllowOndutyTime) && ("00:00" != this.strAllowOndutyTime))
            {
                if (this.InformationIsDate("2000-1-1 " + this.strAllowOndutyTime))
                {
                    this.strAllowOndutyTime = Strings.Format(DateTime.Parse("2000-1-1 " + this.strAllowOndutyTime), "H:mm:ss");
                    this.normalDay = 1;
                    DateTime expression = Convert.ToDateTime(Strings.Format(DateTime.Now, "yyyy-MM-dd") + " " + this.strAllowOndutyTime).AddMilliseconds(-1.0);
                    this.strAllowOffdutyTime = Strings.Format(expression, "H:mm:ss");
                }
                else
                {
                    this.strAllowOndutyTime = "00:00:00";
                }
            }
            if (this.tReadCardTimes != 4)
            {
                if (this.bChooseOnlyOnDuty)
                {
                    this.make4OneTime();
                }
                else
                {
                    this.make4TwoTimes();
                }
            }
            else if (this.bChooseOnlyOnDuty)
            {
                this.make4FourTimesOnlyDuty();
            }
            else
            {
                this.make4FourTimes();
            }
        }

        private void make4FourTimes()
        {
            this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.dtCardRecord1 = new DataTable();
            this.dsAtt = new DataSet("Attendance");
            this.dsAtt.Clear();
            this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
            this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
            this.daHolidayType = new SqlDataAdapter("SELECT * FROM t_a_HolidayType", this.cn);
            this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
            this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
            this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
            this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
            this.dtCardRecord1.Clear();
            this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
            this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
            this.getAttendenceParam();
            this._clearAttendenceData();
            this._clearAttStatistic();
            this.daHoliday.Fill(this.dsAtt, "Holiday");
            this.dtHoliday = this.dsAtt.Tables["Holiday"];
            this.localizedHoliday(this.dtHoliday);
            this.dvHoliday = new DataView(this.dtHoliday);
            this.dvHoliday.RowFilter = "";
            this.dvHoliday.Sort = " f_NO ASC ";
            this.daLeave.Fill(this.dsAtt, "Leave");
            this.dtLeave = this.dsAtt.Tables["Leave"];
            this.dvLeave = new DataView(this.dtLeave);
            this.dvLeave.RowFilter = "";
            this.dvLeave.Sort = " f_NO ASC ";
            this.daHolidayType.Fill(this.dsAtt, "HolidayType");
            this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
            this.localizedHolidayType(this.dtHolidayType);
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
            }
            else
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
            }
            this.cnConsumer.Open();
            SqlDataReader reader = this.cmdConsumer.ExecuteReader();
            int num = 0;
            try
            {
                int num2 = 0;
                while (reader.Read())
                {
                    num = (int) reader["f_ConsumerID"];
                    num2++;
                    string selectCommandText = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
                    selectCommandText = ((((selectCommandText + " FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON ( t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ) ") + 
                        " WHERE f_ConsumerID=" + num.ToString()) + 
                        " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + 
                        " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + 
                        " AND t_b_Reader.f_Attend = 1 ";
                    if (wgAppConfig.getSystemParamByNO(0x36) == "1")
                    {
                        selectCommandText = selectCommandText + " AND f_Character >= 1 ";
                    }
                    selectCommandText = selectCommandText + " ORDER BY f_ReadDate ASC ";
                    this.daCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    selectCommandText = "SELECT f_ReadDate,f_Character ";
                    selectCommandText = ((((selectCommandText + string.Format(",{0} as f_Type", this.PrepareStr(CommonStr.strSignIn)) + " FROM t_d_ManualCardRecord  ") + " WHERE f_ConsumerID=" + num.ToString()) + " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + " ORDER BY f_ReadDate ASC ";
                    this.daManualCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    decimal[] numArray = new decimal[0x20];
                    DataRow row = null;
                    if (this.DealingNumEvent != null)
                    {
                        this.DealingNumEvent(num2);
                    }
                    this.gProcVal = num2 + 1;
                    if (this.bStopCreate)
                    {
                        return;
                    }
                    DateTime time3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    DateTime time4 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
                    DateTime expression = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    decimal num9 = 0M;
                    decimal num10 = 0M;
                    int num11 = 0;
                    int num12 = 0;
                    int num13 = 0;
                    int index = 0;
                    while (index <= (numArray.Length - 1))
                    {
                        numArray[index] = 0M;
                        index++;
                    }
                    this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
                    this.dsAtt.Tables["AllCardRecords"].Clear();
                    this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.dvCardRecord = new DataView(this.dtCardRecord1);
                    this.dvCardRecord.RowFilter = "";
                    this.dvCardRecord.Sort = " f_ReadDate ASC ";
                    int num14 = 0;
                    while (this.dvCardRecord.Count > (num14 + 1))
                    {
                        DateTime time11 = (DateTime) this.dvCardRecord[num14 + 1][0];
                        if (time11.Subtract((DateTime) this.dvCardRecord[num14][0]).TotalSeconds < this.tTwoReadMintime)
                        {
                            this.dvCardRecord[num14 + 1].Delete();
                        }
                        else
                        {
                            num14++;
                        }
                    }
                    while (expression <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
                    {
                        DateTime time2;
                        row = this.dtAttendenceData.NewRow();
                        row["f_ConsumerID"] = num;
                        row["f_AttDate"] = expression;
                        row["f_LateTime"] = 0;
                        row["f_LeaveEarlyTime"] = 0;
                        row["f_OvertimeTime"] = 0;
                        row["f_AbsenceDay"] = 0;
                        bool flag2 = true;
                        bool flag3 = true;
                        this.dvCardRecord.RowFilter = " f_ReadDate >= #" + expression.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(expression.AddDays((double) this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
                        this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
                        if (this.dvCardRecord.Count > 0)
                        {
                            int num4 = 0;
                            while (num4 <= (this.dvCardRecord.Count - 1))
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (string.Compare(Strings.Format(time2, "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty1, "HH:mm:ss")) <= 0)
                                    {
                                        if (this.bEarliestAsOnDuty)
                                        {
                                            if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                            {
                                                row["f_Onduty1"] = time2;
                                                row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                        }
                                        else
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        }
                                        if (this.dvCardRecord.Count != 4)
                                        {
                                            goto Label_0A65;
                                        }
                                        num4++;
                                    }
                                    else if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                    {
                                        if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            num4++;
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = CommonStr.strLateness;
                                            num4++;
                                        }
                                        else
                                        {
                                            if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty1.AddMinutes((double) -this.tLeaveTimeout), "HH:mm:ss")) > 0)
                                            {
                                                row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                            }
                                            else
                                            {
                                                row["f_Onduty1"] = time2;
                                                row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                            }
                                            if (this.dvCardRecord.Count == 4)
                                            {
                                                num4++;
                                            }
                                        }
                                    }
                                }
                                else if (!(this.SetObjToStr(row["f_Onduty1"]) != ""))
                                {
                                    row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                }
                                break;
                            Label_0A65:
                                num4++;
                            }
                            int num15 = num4;
                            num4 = num15;
                            while (num4 <= (this.dvCardRecord.Count - 1))
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (string.Compare(Strings.Format(time2, "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                {
                                    if ((string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) <= 0) || (this.dvCardRecord.Count == 4))
                                    {
                                        row["f_Offduty1"] = time2;
                                        row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        if (string.Compare(Strings.Format(time2.AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                        {
                                            if (string.Compare(Strings.Format(time2.AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                            }
                                            else
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                            }
                                        }
                                        if (this.dvCardRecord.Count != 4)
                                        {
                                            goto Label_0EE6;
                                        }
                                        num4++;
                                    }
                                    else
                                    {
                                        if (this.SetObjToStr(row["f_Offduty1"]) == "")
                                        {
                                            if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) >= 0)
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strNotReadCard;
                                            }
                                            else if (((num4 + 1) <= (this.dvCardRecord.Count - 1)) && (string.Compare(Strings.Format(this.dvCardRecord[num4 + 1]["f_ReadDate"], "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0))
                                            {
                                                row["f_Offduty1"] = time2;
                                                row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                            else if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strNotReadCard;
                                            }
                                            else
                                            {
                                                row["f_Offduty1"] = time2;
                                                row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                        }
                                        else if (((num4 + 1) <= (this.dvCardRecord.Count - 1)) && (string.Compare(Strings.Format(this.dvCardRecord[num4 + 1]["f_ReadDate"], "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0))
                                        {
                                            if (this.SetObjToStr(row["f_Offduty1Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0)
                                            {
                                                row["f_Offduty1"] = time2;
                                                row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                            else if ((this.SetObjToStr(row["f_Offduty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                            {
                                                row["f_Offduty1"] = time2;
                                                row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                        }
                                        if (this.dvCardRecord.Count == 4)
                                        {
                                            num4++;
                                        }
                                    }
                                }
                                else if (this.SetObjToStr(row["f_Offduty1"]) == "")
                                {
                                    row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                }
                                break;
                            Label_0EE6:
                                num4++;
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == this.SetObjToStr(row["f_Onduty1"])) && ((this.SetObjToStr(row["f_Offduty1Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0) || (this.SetObjToStr(row["f_Offduty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)))
                            {
                                row["f_Offduty1"] = DBNull.Value;
                                row["f_Offduty1Desc"] = CommonStr.strNotReadCard;
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == "") && (this.SetObjToStr(row["f_Onduty1"]) == ""))
                            {
                                row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == "") && (this.SetObjToStr(row["f_Offduty1Desc"]) == ""))
                            {
                                row["f_Offduty1Desc"] = CommonStr.strNotReadCard;
                            }
                            if ((this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0))
                            {
                                row["f_OnDuty1Desc"] = "";
                            }
                            num15 = num4;
                            num4 = num15;
                            while (num4 <= (this.dvCardRecord.Count - 1))
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (Strings.Format(time2, "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) <= 0)
                                    {
                                        row["f_Onduty2"] = time2;
                                        row["f_Onduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        if (this.dvCardRecord.Count != 4)
                                        {
                                            goto Label_1376;
                                        }
                                        num4++;
                                    }
                                    else
                                    {
                                        if (this.SetObjToStr(row["f_Onduty2"]) != "")
                                        {
                                            if (!(this.SetObjToStr(row["f_Offduty1"]) == this.SetObjToStr(row["f_Onduty2"])))
                                            {
                                                break;
                                            }
                                            row["f_Onduty2"] = DBNull.Value;
                                            row["f_Onduty2Desc"] = "";
                                        }
                                        if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                        {
                                            row["f_Onduty2"] = time2;
                                            row["f_Onduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                        {
                                            row["f_Onduty2"] = time2;
                                            row["f_Onduty2Desc"] = CommonStr.strLateness;
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty2.AddMinutes((double) -this.tLeaveTimeout), "HH:mm:ss")) > 0)
                                        {
                                            row["f_Onduty2Desc"] = CommonStr.strNotReadCard;
                                        }
                                        else
                                        {
                                            row["f_Onduty2"] = time2;
                                            row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                        }
                                        if (this.dvCardRecord.Count == 4)
                                        {
                                            num4++;
                                        }
                                    }
                                }
                                else if ((this.SetObjToStr(row["f_Onduty2Desc"]) == "") && (this.SetObjToStr(row["f_Onduty2"]) == ""))
                                {
                                    row["f_Onduty2Desc"] = CommonStr.strNotReadCard;
                                }
                                break;
                            Label_1376:
                                num4++;
                            }
                            num15 = num4;
                            for (num4 = num15; num4 <= (this.dvCardRecord.Count - 1); num4++)
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (Strings.Format(time2, "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) <= 0)
                                    {
                                        row["f_Offduty2"] = time2;
                                        row["f_Offduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        if (string.Compare(Strings.Format(time2.AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) < 0)
                                        {
                                            if (string.Compare(Strings.Format(time2.AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) < 0)
                                            {
                                                row["f_Offduty2Desc"] = CommonStr.strAbsence;
                                            }
                                            else
                                            {
                                                row["f_Offduty2Desc"] = CommonStr.strLeaveEarly;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        row["f_Offduty2"] = time2;
                                        row["f_Offduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                    }
                                }
                                else
                                {
                                    row["f_Offduty2"] = time2;
                                    row["f_Offduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                }
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == this.SetObjToStr(row["f_Onduty2"])) && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                            {
                                row["f_Onduty2"] = DBNull.Value;
                                row["f_Onduty2Desc"] = "";
                            }
                            if ((this.SetObjToStr(row["f_Offduty2"]) == this.SetObjToStr(row["f_Onduty2"])) && ((this.SetObjToStr(row["f_Offduty2Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0) || (this.SetObjToStr(row["f_Offduty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0)))
                            {
                                row["f_Offduty2"] = DBNull.Value;
                                row["f_Offduty2Desc"] = CommonStr.strNotReadCard;
                            }
                            if ((this.SetObjToStr(row["f_Offduty2"]) == "") && (this.SetObjToStr(row["f_Onduty2"]) == ""))
                            {
                                row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                row["f_Offduty2Desc"] = CommonStr.strAbsence;
                            }
                            if ((this.SetObjToStr(row["f_Offduty2"]) == "") && (this.SetObjToStr(row["f_Offduty2Desc"]) == ""))
                            {
                                row["f_Offduty2Desc"] = CommonStr.strNotReadCard;
                            }
                            if ((this.SetObjToStr(row["f_OnDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty2Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0))
                            {
                                row["f_OnDuty2Desc"] = "";
                            }
                        }
                        else
                        {
                            row["f_OnDuty1Desc"] = CommonStr.strAbsence;
                            row["f_OffDuty1Desc"] = CommonStr.strAbsence;
                            row["f_OnDuty2Desc"] = CommonStr.strAbsence;
                            row["f_OffDuty2Desc"] = CommonStr.strAbsence;
                        }
                        int num16 = 3;
                        this.dvHoliday.RowFilter = " f_NO =1 ";
                        if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Saturday))
                        {
                            num16 = 0;
                        }
                        else
                        {
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num16 = 1;
                            }
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num16 = 2;
                            }
                            this.dvHoliday.RowFilter = " f_NO =2 ";
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Sunday))
                            {
                                num16 = 0;
                            }
                            else
                            {
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num16 = 1;
                                }
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num16 = 2;
                                }
                                this.dvHoliday.RowFilter = " f_TYPE =2 ";
                                for (int i = 0; i <= (this.dvHoliday.Count - 1); i++)
                                {
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                    DateTime time5 = DateTime.Parse(this.strTemp);
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value2"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                    DateTime time6 = DateTime.Parse(this.strTemp);
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num16 = 0;
                                        break;
                                    }
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time6))
                                    {
                                        num16 = 2;
                                    }
                                    if ((time5 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num16 = 1;
                                    }
                                }
                            }
                        }
                        if (num16 != 3)
                        {
                            this.dvHoliday.RowFilter = " f_TYPE =3 ";
                            for (int j = 0; j <= (this.dvHoliday.Count - 1); j++)
                            {
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime time7 = DateTime.Parse(this.strTemp);
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime time8 = DateTime.Parse(this.strTemp);
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    num16 = 3;
                                    break;
                                }
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time8))
                                {
                                    if (num16 == 2)
                                    {
                                        num16 = 3;
                                    }
                                    else
                                    {
                                        num16 = 1;
                                    }
                                }
                                if ((time7 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    if (num16 == 1)
                                    {
                                        num16 = 3;
                                    }
                                    else
                                    {
                                        num16 = 2;
                                    }
                                }
                            }
                        }
                        switch (num16)
                        {
                            case 0:
                                if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty2Desc"] = CommonStr.strRest;
                                }
                                row["f_OnDuty1Desc"] = CommonStr.strRest;
                                row["f_OffDuty1Desc"] = CommonStr.strRest;
                                row["f_OnDuty2Desc"] = CommonStr.strRest;
                                row["f_OffDuty2Desc"] = CommonStr.strRest;
                                if (((this.SetObjToStr(row["f_Onduty1"]) != "") || (this.SetObjToStr(row["f_Offduty1"]) != "")) || ((this.SetObjToStr(row["f_Onduty2"]) != "") || (this.SetObjToStr(row["f_Offduty2"]) != "")))
                                {
                                    if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                        row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                        if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                        else
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                    }
                                    if ((this.SetObjToStr(row["f_Onduty2"]) != "") && (this.SetObjToStr(row["f_Offduty2"]) != ""))
                                    {
                                        row["f_OnDuty2Desc"] = CommonStr.strOvertime;
                                        row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                        if (Strings.Format(row["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                        else
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                    }
                                }
                                flag2 = false;
                                flag3 = false;
                                break;

                            case 1:
                            {
                                if ((this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty2Desc"] = CommonStr.strRest;
                                }
                                bool flag4 = false;
                                if (((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    if (this.SetObjToStr(row["f_OffDuty2"]) != "")
                                    {
                                        row["f_OffDuty1"] = row["f_OffDuty2"];
                                        row["f_OffDuty1Desc"] = "";
                                        row["f_OffDuty2"] = DBNull.Value;
                                        row["f_OnDuty2"] = DBNull.Value;
                                        flag4 = true;
                                    }
                                    else if (this.SetObjToStr(row["f_OnDuty2"]) != "")
                                    {
                                        row["f_OffDuty1"] = row["f_OnDuty2"];
                                        row["f_OffDuty1Desc"] = "";
                                        row["f_OnDuty2"] = DBNull.Value;
                                        row["f_OffDuty2"] = DBNull.Value;
                                        flag4 = true;
                                    }
                                    if (flag4)
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty1"]) == "")
                                        {
                                            row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                            row["f_Offduty1Desc"] = DBNull.Value;
                                        }
                                        else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                        {
                                            if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                            }
                                            else
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                            }
                                        }
                                        else
                                        {
                                            row["f_Offduty1Desc"] = DBNull.Value;
                                        }
                                    }
                                }
                                row["f_OnDuty2Desc"] = CommonStr.strRest;
                                row["f_OffDuty2Desc"] = CommonStr.strRest;
                                if ((this.SetObjToStr(row["f_Onduty2"]) != "") && (this.SetObjToStr(row["f_Offduty2"]) != ""))
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strOvertime;
                                    row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                    if (Strings.Format(row["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                    else
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                }
                                else
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strRest;
                                    row["f_OffDuty2Desc"] = CommonStr.strRest;
                                }
                                break;
                            }
                            case 2:
                                if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                row["f_OnDuty1Desc"] = CommonStr.strRest;
                                row["f_OffDuty1Desc"] = CommonStr.strRest;
                                if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence)
                                {
                                    if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                    {
                                        row["f_OnDuty2"] = row["f_OnDuty1"];
                                        row["f_OnDuty1"] = DBNull.Value;
                                        row["f_OffDuty1"] = DBNull.Value;
                                    }
                                    else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                    {
                                        row["f_OnDuty2"] = row["f_OffDuty1"];
                                        row["f_OffDuty1"] = DBNull.Value;
                                    }
                                }
                                if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                    row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                    if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                    else
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                }
                                else
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                break;

                            default:
                                if (num16 == 2)
                                {
                                    if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strRest;
                                    }
                                    if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                    {
                                        row["f_OffDuty1Desc"] = CommonStr.strRest;
                                    }
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                    if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence)
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                        {
                                            row["f_OnDuty2"] = row["f_OnDuty1"];
                                            row["f_OnDuty1"] = DBNull.Value;
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                        else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                        {
                                            row["f_OnDuty2"] = row["f_OffDuty1"];
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                    }
                                    if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                        row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                        if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                        else
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                    }
                                    else
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strRest;
                                        row["f_OffDuty1Desc"] = CommonStr.strRest;
                                    }
                                }
                                else if (((num16 == 3) && (this.SetObjToStr(row["f_Onduty2"]) != "")) && (this.SetObjToStr(row["f_Offduty2"]) != ""))
                                {
                                    if (string.Compare(Strings.Format(row["f_Offduty2"], "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                    {
                                        if ((string.Compare(Strings.Format(row["f_Offduty2"], "HH:mm:ss"), Strings.Format(this.tOffduty2.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss")) >= 0) && (string.Compare(Strings.Format(this.tOffduty2.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) >= 0))
                                        {
                                            row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                            row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                        }
                                    }
                                    else
                                    {
                                        row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                        row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                    }
                                }
                                break;
                        }
                        if (this.dvLeave.Count > 0)
                        {
                            string str2 = "";
                            string str3 = "";
                            string str4 = "";
                            num16 = 3;
                            for (int k = 0; k <= (this.dvLeave.Count - 1); k++)
                            {
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime time9 = DateTime.Parse(this.strTemp);
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime time10 = DateTime.Parse(this.strTemp);
                                str2 = Convert.ToString(this.dvLeave[k]["f_HolidayType"]);
                                if ((time9 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time10))
                                {
                                    str3 = str2;
                                    str4 = str2;
                                    num16 = 0;
                                    break;
                                }
                                if ((time9 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time10))
                                {
                                    str3 = str2;
                                    if (num16 == 1)
                                    {
                                        num16 = 0;
                                        break;
                                    }
                                    num16 = 2;
                                }
                                if ((time9 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time10))
                                {
                                    str4 = str2;
                                    if (num16 == 2)
                                    {
                                        num16 = 0;
                                        break;
                                    }
                                    num16 = 1;
                                }
                            }
                            bool flag5 = false;
                            switch (num16)
                            {
                                case 0:
                                    row["f_OnDuty1Desc"] = str3;
                                    row["f_OnDuty2Desc"] = str4;
                                    row["f_OffDuty1Desc"] = str3;
                                    row["f_OffDuty2Desc"] = str4;
                                    row["f_OnDuty1"] = DBNull.Value;
                                    row["f_OnDuty2"] = DBNull.Value;
                                    row["f_OffDuty1"] = DBNull.Value;
                                    row["f_OffDuty2"] = DBNull.Value;
                                    break;

                                case 1:
                                    if (((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                    {
                                        if (this.SetObjToStr(row["f_OffDuty2"]) != "")
                                        {
                                            row["f_OffDuty1"] = row["f_OffDuty2"];
                                            flag5 = true;
                                            row["f_OffDuty2"] = DBNull.Value;
                                            row["f_OnDuty2"] = DBNull.Value;
                                        }
                                        else if (this.SetObjToStr(row["f_OnDuty2"]) != "")
                                        {
                                            row["f_OffDuty1"] = row["f_OnDuty2"];
                                            flag5 = true;
                                            row["f_OnDuty2"] = DBNull.Value;
                                        }
                                        if (flag5)
                                        {
                                            if (this.SetObjToStr(row["f_OnDuty1"]) == "")
                                            {
                                                row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                                row["f_Offduty1Desc"] = DBNull.Value;
                                            }
                                            else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                            {
                                                if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                                {
                                                    row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                                }
                                                else
                                                {
                                                    row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                                }
                                            }
                                            else
                                            {
                                                row["f_Offduty1Desc"] = DBNull.Value;
                                            }
                                        }
                                    }
                                    row["f_OnDuty2Desc"] = str4;
                                    row["f_OffDuty2Desc"] = str4;
                                    row["f_OnDuty2"] = DBNull.Value;
                                    row["f_OffDuty2"] = DBNull.Value;
                                    break;

                                case 2:
                                    if (((this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strLateness)) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence))
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                        {
                                            row["f_OnDuty2"] = row["f_OnDuty1"];
                                            flag5 = true;
                                            row["f_OnDuty1"] = DBNull.Value;
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                        else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                        {
                                            flag5 = true;
                                            row["f_OnDuty2"] = row["f_OffDuty1"];
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                        if (flag5)
                                        {
                                            if (this.SetObjToStr(row["f_OffDuty2"]) == "")
                                            {
                                                row["f_Offduty2Desc"] = CommonStr.strNotReadCard;
                                                row["f_Onduty2Desc"] = DBNull.Value;
                                            }
                                            else
                                            {
                                                time2 = Convert.ToDateTime(row["f_OnDuty2"]);
                                                if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                                {
                                                    row["f_Onduty2"] = time2;
                                                    row["f_Onduty2Desc"] = DBNull.Value;
                                                }
                                                else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                                {
                                                    row["f_Onduty2"] = time2;
                                                    row["f_Onduty2Desc"] = CommonStr.strLateness;
                                                }
                                                else
                                                {
                                                    row["f_Onduty2"] = time2;
                                                    row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                                }
                                            }
                                        }
                                    }
                                    row["f_OnDuty1Desc"] = str3;
                                    row["f_OffDuty1Desc"] = str3;
                                    row["f_OnDuty1"] = DBNull.Value;
                                    row["f_OffDuty1"] = DBNull.Value;
                                    break;
                            }
                        }
                        if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strLateness)
                        {
                            row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty1, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strLateness)
                        {
                            row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_OnDuty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
                        {
                            row["f_LeaveEarlyTime"] = Convert.ToInt32(row["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty1, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strLeaveEarly)
                        {
                            row["f_LeaveEarlyTime"] = Convert.ToInt32(row["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OffDuty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if ((this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                            flag3 = false;
                        }
                        else
                        {
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                                flag3 = false;
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
                                flag3 = false;
                            }
                        }
                        if ((this.SetObjToStr(row["f_OnDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                            flag3 = false;
                        }
                        else
                        {
                            if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                                flag3 = false;
                            }
                            if (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
                                flag3 = false;
                            }
                        }
                        if ((Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1M) && (num16 != 3))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0M;
                        }
                        selectCommandText = "";
                        selectCommandText = " INSERT INTO t_d_AttendenceData ";
                        using (SqlCommand command = new SqlCommand((((((((((((((((((selectCommandText + " ([f_ConsumerID], [f_AttDate], ") + "[f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc], " + "[f_Onduty2], [f_Onduty2Desc],[f_Offduty2], [f_Offduty2Desc]  ") + ", [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]  " + " ) ") + " VALUES ( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(row["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1Desc"])) + " , " + this.PrepareStr(row["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Offduty1Desc"])) + " , " + this.PrepareStr(row["f_Onduty2"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty2Desc"])) + " , " + this.PrepareStr(row["f_Offduty2"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Offduty2Desc"])) + " , " + row["f_LateTime"]) + " , " + row["f_LeaveEarlyTime"]) + " , " + this.getDecimalStr(row["f_OvertimeTime"])) + " , " + this.getDecimalStr(row["f_AbsenceDay"])) + " ) ", this.cn))
                        {
                            if (this.cn.State == ConnectionState.Closed)
                            {
                                this.cn.Open();
                            }
                            command.ExecuteNonQuery();
                        }
                        if (flag2)
                        {
                            num5++;
                        }
                        string str5 = "";
                        for (num14 = 0; num14 <= 3; num14++)
                        {
                            switch (num14)
                            {
                                case 0:
                                    str5 = this.SetObjToStr(row["f_OnDuty1Desc"]);
                                    break;

                                case 1:
                                    str5 = this.SetObjToStr(row["f_OnDuty2Desc"]);
                                    break;

                                case 2:
                                    str5 = this.SetObjToStr(row["f_OffDuty1Desc"]);
                                    break;

                                case 3:
                                    str5 = this.SetObjToStr(row["f_OffDuty2Desc"]);
                                    break;
                            }
                            if (str5 == CommonStr.strLateness)
                            {
                                num7++;
                                flag3 = false;
                            }
                            else if (str5 == CommonStr.strLeaveEarly)
                            {
                                num8++;
                                flag3 = false;
                            }
                            else if (str5 == CommonStr.strNotReadCard)
                            {
                                num11++;
                                flag3 = false;
                            }
                            else
                            {
                                str5.IndexOf(CommonStr.strNotReadCard);
                                index = 0;
                                while (index <= (this.dtHolidayType.Rows.Count - 1))
                                {
                                    if (index >= numArray.Length)
                                    {
                                        break;
                                    }
                                    if (str5 == this.dtHolidayType.Rows[index][1].ToString())
                                    {
                                        flag3 = false;
                                        numArray[index] += 0.25M;
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                        if (((this.SetObjToStr(row["f_OnDuty1"]) == "") && (this.SetObjToStr(row["f_OffDuty1"]) == "")) && ((this.SetObjToStr(row["f_OnDuty2"]) == "") && (this.SetObjToStr(row["f_OffDuty2"]) == "")))
                        {
                            flag3 = false;
                        }
                        num9 += Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture);
                        num10 += Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture);
                        num12 += Convert.ToInt32(row["f_LateTime"]);
                        num13 += Convert.ToInt32(row["f_LeaveEarlyTime"]);
                        if (((Convert.ToInt32(row["f_LateTime"]) != 0) || (Convert.ToInt32(row["f_LeaveEarlyTime"]) != 0)) || (Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0M))
                        {
                            flag3 = false;
                        }
                        if (flag3)
                        {
                            num6++;
                        }
                        expression = expression.AddDays(1.0);
                        Application.DoEvents();
                    }
                    this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
                    selectCommandText = "";
                    selectCommandText = " Insert Into t_d_AttStatistic ";
                    selectCommandText = (selectCommandText + " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd] ") + " , [f_DayShouldWork],  [f_DayRealWork]" + " , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
                    index = 1;
                    while (index <= 0x20)
                    {
                        object obj2 = selectCommandText;
                        selectCommandText = string.Concat(new object[] { obj2, " , [f_SpecialType", index, "]" });
                        index++;
                    }
                    selectCommandText = (((((((((((selectCommandText + ", f_LateMinutes" + ", f_LeaveEarlyMinutes") + ", f_ManualReadTimesCount" + " ) ") + " Values( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(time3, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(time4, true, "yyyy-MM-dd HH:mm:ss")) + " , " + num5) + " , " + num6) + " , " + num7) + " , " + num8) + " , " + this.getDecimalStr(num10)) + " , " + this.getDecimalStr(num9)) + " , " + num11;
                    for (index = 0; index <= 0x1f; index++)
                    {
                        selectCommandText = selectCommandText + " , " + this.PrepareStr(this.getDecimalStr(numArray[index]));
                    }
                    using (SqlCommand command2 = new SqlCommand((((selectCommandText + ", " + num12) + ", " + num13) + ", " + this.dvCardRecord.Count) + " )", this.cn))
                    {
                        if (this.cn.State == ConnectionState.Closed)
                        {
                            this.cn.Open();
                        }
                        command2.ExecuteNonQuery();
                        continue;
                    }
                }
                reader.Close();
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
                this.shiftAttReportImportFromAttendenceData();
                this.shiftAttStatisticImportFromAttStatistic();
                this.logCreateReport();
                if (this.CreateCompleteEvent != null)
                {
                    this.CreateCompleteEvent(true, "");
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                try
                {
                    if (this.CreateCompleteEvent != null)
                    {
                        this.CreateCompleteEvent(false, exception.ToString());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void make4FourTimesOnlyDuty()
        {
            this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.dtCardRecord1 = new DataTable();
            this.dsAtt = new DataSet("Attendance");
            this.dsAtt.Clear();
            this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
            this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
            this.daHolidayType = new SqlDataAdapter("SELECT * FROM t_a_HolidayType", this.cn);
            this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
            this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
            this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
            this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
            this.dtCardRecord1.Clear();
            this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
            this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
            this.getAttendenceParam();
            this._clearAttendenceData();
            this._clearAttStatistic();
            this.daHoliday.Fill(this.dsAtt, "Holiday");
            this.dtHoliday = this.dsAtt.Tables["Holiday"];
            this.localizedHoliday(this.dtHoliday);
            this.dvHoliday = new DataView(this.dtHoliday);
            this.dvHoliday.RowFilter = "";
            this.dvHoliday.Sort = " f_NO ASC ";
            this.daLeave.Fill(this.dsAtt, "Leave");
            this.dtLeave = this.dsAtt.Tables["Leave"];
            this.dvLeave = new DataView(this.dtLeave);
            this.dvLeave.RowFilter = "";
            this.dvLeave.Sort = " f_NO ASC ";
            this.daHolidayType.Fill(this.dsAtt, "HolidayType");
            this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
            this.localizedHolidayType(this.dtHolidayType);
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
            }
            else
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
            }
            this.cnConsumer.Open();
            SqlDataReader reader = this.cmdConsumer.ExecuteReader();
            int num = 0;
            try
            {
                int num2 = 0;
                while (reader.Read())
                {
                    num = (int) reader["f_ConsumerID"];
                    num2++;
                    string selectCommandText = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
                    selectCommandText = ((((selectCommandText + " FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON ( t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ) ") + 
                        " WHERE f_ConsumerID=" + num.ToString()) + 
                        " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + 
                        " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + 
                        " AND t_b_Reader.f_Attend = 1 ";
                    if (wgAppConfig.getSystemParamByNO(0x36) == "1")
                    {
                        selectCommandText = selectCommandText + " AND f_Character >= 1 ";
                    }
                    selectCommandText = selectCommandText + " ORDER BY f_ReadDate ASC ";
                    this.daCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    selectCommandText = "SELECT f_ReadDate,f_Character ";
                    selectCommandText = ((((selectCommandText + string.Format(",{0} as f_Type", this.PrepareStr(CommonStr.strSignIn)) + " FROM t_d_ManualCardRecord  ") + " WHERE f_ConsumerID=" + num.ToString()) + " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + " ORDER BY f_ReadDate ASC ";
                    this.daManualCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    decimal[] numArray = new decimal[0x20];
                    DataRow row = null;
                    if (this.DealingNumEvent != null)
                    {
                        this.DealingNumEvent(num2);
                    }
                    this.gProcVal = num2 + 1;
                    if (this.bStopCreate)
                    {
                        return;
                    }
                    DateTime time3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    DateTime time4 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
                    DateTime expression = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    decimal num9 = 0M;
                    decimal num10 = 0M;
                    int num11 = 0;
                    int num12 = 0;
                    int num13 = 0;
                    int index = 0;
                    while (index <= (numArray.Length - 1))
                    {
                        numArray[index] = 0M;
                        index++;
                    }
                    this.dtCardRecord1 = this.dsAtt.Tables["AllCardRecords"];
                    this.dsAtt.Tables["AllCardRecords"].Clear();
                    this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.dvCardRecord = new DataView(this.dtCardRecord1);
                    this.dvCardRecord.RowFilter = "";
                    this.dvCardRecord.Sort = " f_ReadDate ASC ";
                    int num14 = 0;
                    while (this.dvCardRecord.Count > (num14 + 1))
                    {
                        DateTime time11 = (DateTime) this.dvCardRecord[num14 + 1][0];
                        if (time11.Subtract((DateTime) this.dvCardRecord[num14][0]).TotalSeconds < this.tTwoReadMintime)
                        {
                            this.dvCardRecord[num14 + 1].Delete();
                        }
                        else
                        {
                            num14++;
                        }
                    }
                    while (expression <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
                    {
                        DateTime time2;
                        row = this.dtAttendenceData.NewRow();
                        row["f_ConsumerID"] = num;
                        row["f_AttDate"] = expression;
                        row["f_LateTime"] = 0;
                        row["f_LeaveEarlyTime"] = 0;
                        row["f_OvertimeTime"] = 0;
                        row["f_AbsenceDay"] = 0;
                        bool flag2 = true;
                        bool flag3 = true;
                        this.dvCardRecord.RowFilter = " f_ReadDate >= #" + expression.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(expression.AddDays((double) this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
                        this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
                        if (this.dvCardRecord.Count > 0)
                        {
                            int num4 = 0;
                            while (num4 <= (this.dvCardRecord.Count - 1))
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (string.Compare(Strings.Format(time2, "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty1, "HH:mm:ss")) <= 0)
                                    {
                                        if (this.bEarliestAsOnDuty)
                                        {
                                            if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                            {
                                                row["f_Onduty1"] = time2;
                                                row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                        }
                                        else
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        }
                                        goto Label_0A56;
                                    }
                                    if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                    {
                                        if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            num4++;
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty1.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = CommonStr.strLateness;
                                            num4++;
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty1.AddMinutes((double) -this.tLeaveTimeout), "HH:mm:ss")) > 0)
                                        {
                                            row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                        }
                                        else
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                        }
                                    }
                                }
                                else if (!(this.SetObjToStr(row["f_Onduty1"]) != ""))
                                {
                                    row["f_Onduty1"] = time2;
                                    row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                }
                                break;
                            Label_0A56:
                                num4++;
                            }
                            int num15 = num4;
                            if ((this.SetObjToStr(row["f_Offduty1"]) == "") && (this.SetObjToStr(row["f_Onduty1"]) == ""))
                            {
                                row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                            }
                            num15 = num4;
                            for (num4 = num15; num4 <= (this.dvCardRecord.Count - 1); num4++)
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (Strings.Format(time2, "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) <= 0)
                                    {
                                        continue;
                                    }
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2, "HH:mm:ss")) <= 0)
                                    {
                                        row["f_Onduty2"] = time2;
                                        row["f_Onduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        continue;
                                    }
                                    if (this.SetObjToStr(row["f_Onduty2"]) == "")
                                    {
                                        if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                        {
                                            row["f_Onduty2"] = time2;
                                            row["f_Onduty2Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                        {
                                            row["f_Onduty2"] = time2;
                                            row["f_Onduty2Desc"] = CommonStr.strLateness;
                                        }
                                        else
                                        {
                                            row["f_Onduty2"] = time2;
                                            row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                        }
                                    }
                                }
                                else if (!(this.SetObjToStr(row["f_Onduty2"]) != ""))
                                {
                                    row["f_Onduty2"] = time2;
                                    row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                }
                                break;
                            }
                            if ((this.SetObjToStr(row["f_Offduty2"]) == "") && (this.SetObjToStr(row["f_Onduty2"]) == ""))
                            {
                                row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                row["f_Offduty2Desc"] = CommonStr.strAbsence;
                            }
                        }
                        else
                        {
                            row["f_OnDuty1Desc"] = CommonStr.strAbsence;
                            row["f_OffDuty1Desc"] = CommonStr.strAbsence;
                            row["f_OnDuty2Desc"] = CommonStr.strAbsence;
                            row["f_OffDuty2Desc"] = CommonStr.strAbsence;
                        }
                        int num16 = 3;
                        this.dvHoliday.RowFilter = " f_NO =1 ";
                        if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Saturday))
                        {
                            num16 = 0;
                        }
                        else
                        {
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num16 = 1;
                            }
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num16 = 2;
                            }
                            this.dvHoliday.RowFilter = " f_NO =2 ";
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Sunday))
                            {
                                num16 = 0;
                            }
                            else
                            {
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num16 = 1;
                                }
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num16 = 2;
                                }
                                this.dvHoliday.RowFilter = " f_TYPE =2 ";
                                for (int i = 0; i <= (this.dvHoliday.Count - 1); i++)
                                {
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                    DateTime time5 = DateTime.Parse(this.strTemp);
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value2"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                    DateTime time6 = DateTime.Parse(this.strTemp);
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num16 = 0;
                                        break;
                                    }
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time6))
                                    {
                                        num16 = 2;
                                    }
                                    if ((time5 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num16 = 1;
                                    }
                                }
                            }
                        }
                        if (num16 != 3)
                        {
                            this.dvHoliday.RowFilter = " f_TYPE =3 ";
                            for (int j = 0; j <= (this.dvHoliday.Count - 1); j++)
                            {
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime time7 = DateTime.Parse(this.strTemp);
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime time8 = DateTime.Parse(this.strTemp);
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    num16 = 3;
                                    break;
                                }
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time8))
                                {
                                    if (num16 == 2)
                                    {
                                        num16 = 3;
                                    }
                                    else
                                    {
                                        num16 = 1;
                                    }
                                }
                                if ((time7 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    if (num16 == 1)
                                    {
                                        num16 = 3;
                                    }
                                    else
                                    {
                                        num16 = 2;
                                    }
                                }
                            }
                        }
                        switch (num16)
                        {
                            case 0:
                                if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty2Desc"] = CommonStr.strRest;
                                }
                                row["f_OnDuty1Desc"] = CommonStr.strRest;
                                row["f_OffDuty1Desc"] = CommonStr.strRest;
                                row["f_OnDuty2Desc"] = CommonStr.strRest;
                                row["f_OffDuty2Desc"] = CommonStr.strRest;
                                if (((this.SetObjToStr(row["f_Onduty1"]) != "") || (this.SetObjToStr(row["f_Offduty1"]) != "")) || ((this.SetObjToStr(row["f_Onduty2"]) != "") || (this.SetObjToStr(row["f_Offduty2"]) != "")))
                                {
                                    if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                        row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                        if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                        else
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                    }
                                    if ((this.SetObjToStr(row["f_Onduty2"]) != "") && (this.SetObjToStr(row["f_Offduty2"]) != ""))
                                    {
                                        row["f_OnDuty2Desc"] = CommonStr.strOvertime;
                                        row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                        if (Strings.Format(row["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                        else
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                    }
                                }
                                flag2 = false;
                                flag3 = false;
                                break;

                            case 1:
                            {
                                if ((this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty2Desc"] = CommonStr.strRest;
                                }
                                bool flag4 = false;
                                if (((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    if (this.SetObjToStr(row["f_OffDuty2"]) != "")
                                    {
                                        row["f_OffDuty1"] = row["f_OffDuty2"];
                                        row["f_OffDuty1Desc"] = "";
                                        row["f_OffDuty2"] = DBNull.Value;
                                        row["f_OnDuty2"] = DBNull.Value;
                                        flag4 = true;
                                    }
                                    else if (this.SetObjToStr(row["f_OnDuty2"]) != "")
                                    {
                                        row["f_OffDuty1"] = row["f_OnDuty2"];
                                        row["f_OffDuty1Desc"] = "";
                                        row["f_OnDuty2"] = DBNull.Value;
                                        row["f_OffDuty2"] = DBNull.Value;
                                        flag4 = true;
                                    }
                                    if (flag4)
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty1"]) == "")
                                        {
                                            row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                            row["f_Offduty1Desc"] = DBNull.Value;
                                        }
                                        else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                        {
                                            if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                            }
                                            else
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                            }
                                        }
                                        else
                                        {
                                            row["f_Offduty1Desc"] = DBNull.Value;
                                        }
                                    }
                                }
                                row["f_OnDuty2Desc"] = CommonStr.strRest;
                                row["f_OffDuty2Desc"] = CommonStr.strRest;
                                if ((this.SetObjToStr(row["f_Onduty2"]) != "") && (this.SetObjToStr(row["f_Offduty2"]) != ""))
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strOvertime;
                                    row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                    if (Strings.Format(row["f_Offduty2"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                    else
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty2"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                }
                                else
                                {
                                    row["f_OnDuty2Desc"] = CommonStr.strRest;
                                    row["f_OffDuty2Desc"] = CommonStr.strRest;
                                }
                                break;
                            }
                            case 2:
                                if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                row["f_OnDuty1Desc"] = CommonStr.strRest;
                                row["f_OffDuty1Desc"] = CommonStr.strRest;
                                if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence)
                                {
                                    if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                    {
                                        row["f_OnDuty2"] = row["f_OnDuty1"];
                                        row["f_OnDuty1"] = DBNull.Value;
                                        row["f_OffDuty1"] = DBNull.Value;
                                    }
                                    else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                    {
                                        row["f_OnDuty2"] = row["f_OffDuty1"];
                                        row["f_OffDuty1"] = DBNull.Value;
                                    }
                                }
                                if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                    row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                    if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                    else
                                    {
                                        row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                    }
                                }
                                else
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                break;

                            default:
                                if (num16 == 2)
                                {
                                    if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strRest;
                                    }
                                    if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                    {
                                        row["f_OffDuty1Desc"] = CommonStr.strRest;
                                    }
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                    if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence)
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                        {
                                            row["f_OnDuty2"] = row["f_OnDuty1"];
                                            row["f_OnDuty1"] = DBNull.Value;
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                        else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                        {
                                            row["f_OnDuty2"] = row["f_OffDuty1"];
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                    }
                                    if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                        row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                        if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                        else
                                        {
                                            row["f_OvertimeTime"] = Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture) + (Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M);
                                        }
                                    }
                                    else
                                    {
                                        row["f_OnDuty1Desc"] = CommonStr.strRest;
                                        row["f_OffDuty1Desc"] = CommonStr.strRest;
                                    }
                                }
                                else if (((num16 == 3) && (this.SetObjToStr(row["f_Onduty2"]) != "")) && (this.SetObjToStr(row["f_Offduty2"]) != ""))
                                {
                                    if (string.Compare(Strings.Format(row["f_Offduty2"], "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                    {
                                        if ((string.Compare(Strings.Format(row["f_Offduty2"], "HH:mm:ss"), Strings.Format(this.tOffduty2.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss")) >= 0) && (string.Compare(Strings.Format(this.tOffduty2.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty2, "HH:mm:ss")) >= 0))
                                        {
                                            row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                            row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                        }
                                    }
                                    else
                                    {
                                        row["f_OffDuty2Desc"] = CommonStr.strOvertime;
                                        row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty2, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty2"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                    }
                                }
                                break;
                        }
                        if (this.dvLeave.Count > 0)
                        {
                            string str2 = "";
                            string str3 = "";
                            string str4 = "";
                            num16 = 3;
                            for (int k = 0; k <= (this.dvLeave.Count - 1); k++)
                            {
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime time9 = DateTime.Parse(this.strTemp);
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime time10 = DateTime.Parse(this.strTemp);
                                str2 = Convert.ToString(this.dvLeave[k]["f_HolidayType"]);
                                if ((time9 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time10))
                                {
                                    str3 = str2;
                                    str4 = str2;
                                    num16 = 0;
                                    break;
                                }
                                if ((time9 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time10))
                                {
                                    str3 = str2;
                                    if (num16 == 1)
                                    {
                                        num16 = 0;
                                        break;
                                    }
                                    num16 = 2;
                                }
                                if ((time9 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time10))
                                {
                                    str4 = str2;
                                    if (num16 == 2)
                                    {
                                        num16 = 0;
                                        break;
                                    }
                                    num16 = 1;
                                }
                            }
                            bool flag5 = false;
                            switch (num16)
                            {
                                case 0:
                                    row["f_OnDuty1Desc"] = str3;
                                    row["f_OnDuty2Desc"] = str4;
                                    row["f_OffDuty1Desc"] = str3;
                                    row["f_OffDuty2Desc"] = str4;
                                    row["f_OnDuty1"] = DBNull.Value;
                                    row["f_OnDuty2"] = DBNull.Value;
                                    row["f_OffDuty1"] = DBNull.Value;
                                    row["f_OffDuty2"] = DBNull.Value;
                                    break;

                                case 1:
                                    if (((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                    {
                                        if (this.SetObjToStr(row["f_OffDuty2"]) != "")
                                        {
                                            row["f_OffDuty1"] = row["f_OffDuty2"];
                                            flag5 = true;
                                            row["f_OffDuty2"] = DBNull.Value;
                                            row["f_OnDuty2"] = DBNull.Value;
                                        }
                                        else if (this.SetObjToStr(row["f_OnDuty2"]) != "")
                                        {
                                            row["f_OffDuty1"] = row["f_OnDuty2"];
                                            flag5 = true;
                                            row["f_OnDuty2"] = DBNull.Value;
                                        }
                                        if (flag5)
                                        {
                                            if (this.SetObjToStr(row["f_OnDuty1"]) == "")
                                            {
                                                row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                                row["f_Offduty1Desc"] = DBNull.Value;
                                            }
                                            else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                            {
                                                if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_OffDuty1"]).AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty1, "HH:mm:ss")) < 0)
                                                {
                                                    row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                                }
                                                else
                                                {
                                                    row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                                }
                                            }
                                            else
                                            {
                                                row["f_Offduty1Desc"] = DBNull.Value;
                                            }
                                        }
                                    }
                                    row["f_OnDuty2Desc"] = str4;
                                    row["f_OffDuty2Desc"] = str4;
                                    row["f_OnDuty2"] = DBNull.Value;
                                    row["f_OffDuty2"] = DBNull.Value;
                                    break;

                                case 2:
                                    if (((this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strLateness)) || (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence))
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                        {
                                            row["f_OnDuty2"] = row["f_OnDuty1"];
                                            flag5 = true;
                                            row["f_OnDuty1"] = DBNull.Value;
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                        else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                        {
                                            flag5 = true;
                                            row["f_OnDuty2"] = row["f_OffDuty1"];
                                            row["f_OffDuty1"] = DBNull.Value;
                                        }
                                        if (flag5)
                                        {
                                            if (this.SetObjToStr(row["f_OffDuty2"]) == "")
                                            {
                                                row["f_Offduty2Desc"] = CommonStr.strNotReadCard;
                                                row["f_Onduty2Desc"] = DBNull.Value;
                                            }
                                            else
                                            {
                                                time2 = Convert.ToDateTime(row["f_OnDuty2"]);
                                                if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                                {
                                                    row["f_Onduty2"] = time2;
                                                    row["f_Onduty2Desc"] = DBNull.Value;
                                                }
                                                else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty2.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                                {
                                                    row["f_Onduty2"] = time2;
                                                    row["f_Onduty2Desc"] = CommonStr.strLateness;
                                                }
                                                else
                                                {
                                                    row["f_Onduty2"] = time2;
                                                    row["f_Onduty2Desc"] = CommonStr.strAbsence;
                                                }
                                            }
                                        }
                                    }
                                    row["f_OnDuty1Desc"] = str3;
                                    row["f_OffDuty1Desc"] = str3;
                                    row["f_OnDuty1"] = DBNull.Value;
                                    row["f_OffDuty1"] = DBNull.Value;
                                    break;
                            }
                        }
                        if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strLateness)
                        {
                            row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty1, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strLateness)
                        {
                            row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty2, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_OnDuty2"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
                        {
                            row["f_LeaveEarlyTime"] = Convert.ToInt32(row["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty1, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strLeaveEarly)
                        {
                            row["f_LeaveEarlyTime"] = Convert.ToInt32(row["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OffDuty2"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty2, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                        }
                        if ((this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                            flag3 = false;
                        }
                        else
                        {
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                                flag3 = false;
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
                                flag3 = false;
                            }
                        }
                        if ((this.SetObjToStr(row["f_OnDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty2Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                            flag3 = false;
                        }
                        else
                        {
                            if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                                flag3 = false;
                            }
                            if (this.SetObjToStr(row["f_OffDuty2Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
                                flag3 = false;
                            }
                        }
                        if ((Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1M) && (num16 != 3))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0M;
                        }
                        selectCommandText = "";
                        selectCommandText = " INSERT INTO t_d_AttendenceData ";
                        using (SqlCommand command = new SqlCommand((((((((((((((((((selectCommandText + " ([f_ConsumerID], [f_AttDate], ") + "[f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc], " + "[f_Onduty2], [f_Onduty2Desc],[f_Offduty2], [f_Offduty2Desc]  ") + ", [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]  " + " ) ") + " VALUES ( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(row["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1Desc"])) + " , " + this.PrepareStr(row["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Offduty1Desc"])) + " , " + this.PrepareStr(row["f_Onduty2"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty2Desc"])) + " , " + this.PrepareStr(row["f_Offduty2"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Offduty2Desc"])) + " , " + row["f_LateTime"]) + " , " + row["f_LeaveEarlyTime"]) + " , " + this.getDecimalStr(row["f_OvertimeTime"])) + " , " + this.getDecimalStr(row["f_AbsenceDay"])) + " ) ", this.cn))
                        {
                            if (this.cn.State == ConnectionState.Closed)
                            {
                                this.cn.Open();
                            }
                            command.ExecuteNonQuery();
                        }
                        if (flag2)
                        {
                            num5++;
                        }
                        string str5 = "";
                        for (num14 = 0; num14 <= 3; num14++)
                        {
                            switch (num14)
                            {
                                case 0:
                                    str5 = this.SetObjToStr(row["f_OnDuty1Desc"]);
                                    break;

                                case 1:
                                    str5 = this.SetObjToStr(row["f_OnDuty2Desc"]);
                                    break;

                                case 2:
                                    str5 = this.SetObjToStr(row["f_OffDuty1Desc"]);
                                    break;

                                case 3:
                                    str5 = this.SetObjToStr(row["f_OffDuty2Desc"]);
                                    break;
                            }
                            if (str5 == CommonStr.strLateness)
                            {
                                num7++;
                                flag3 = false;
                            }
                            else if (str5 == CommonStr.strLeaveEarly)
                            {
                                num8++;
                                flag3 = false;
                            }
                            else if (str5 == CommonStr.strNotReadCard)
                            {
                                num11++;
                                flag3 = false;
                            }
                            else
                            {
                                str5.IndexOf(CommonStr.strNotReadCard);
                                index = 0;
                                while (index <= (this.dtHolidayType.Rows.Count - 1))
                                {
                                    if (index >= numArray.Length)
                                    {
                                        break;
                                    }
                                    if (str5 == this.dtHolidayType.Rows[index][1].ToString())
                                    {
                                        flag3 = false;
                                        numArray[index] += 0.25M;
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                        if (((this.SetObjToStr(row["f_OnDuty1"]) == "") && (this.SetObjToStr(row["f_OffDuty1"]) == "")) && ((this.SetObjToStr(row["f_OnDuty2"]) == "") && (this.SetObjToStr(row["f_OffDuty2"]) == "")))
                        {
                            flag3 = false;
                        }
                        num9 += Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture);
                        num10 += Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture);
                        num12 += Convert.ToInt32(row["f_LateTime"]);
                        num13 += Convert.ToInt32(row["f_LeaveEarlyTime"]);
                        if (((Convert.ToInt32(row["f_LateTime"]) != 0) || (Convert.ToInt32(row["f_LeaveEarlyTime"]) != 0)) || (Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0M))
                        {
                            flag3 = false;
                        }
                        if (flag3)
                        {
                            num6++;
                        }
                        expression = expression.AddDays(1.0);
                        Application.DoEvents();
                    }
                    this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
                    selectCommandText = "";
                    selectCommandText = " Insert Into t_d_AttStatistic ";
                    selectCommandText = (selectCommandText + " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd] ") + " , [f_DayShouldWork],  [f_DayRealWork]" + " , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
                    index = 1;
                    while (index <= 0x20)
                    {
                        object obj2 = selectCommandText;
                        selectCommandText = string.Concat(new object[] { obj2, " , [f_SpecialType", index, "]" });
                        index++;
                    }
                    selectCommandText = (((((((((((selectCommandText + ", f_LateMinutes" + ", f_LeaveEarlyMinutes") + ", f_ManualReadTimesCount" + " ) ") + " Values( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(time3, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(time4, true, "yyyy-MM-dd HH:mm:ss")) + " , " + num5) + " , " + num6) + " , " + num7) + " , " + num8) + " , " + this.getDecimalStr(num10)) + " , " + this.getDecimalStr(num9)) + " , " + num11;
                    for (index = 0; index <= 0x1f; index++)
                    {
                        selectCommandText = selectCommandText + " , " + this.PrepareStr(this.getDecimalStr(numArray[index]));
                    }
                    using (SqlCommand command2 = new SqlCommand((((selectCommandText + ", " + num12) + ", " + num13) + ", " + this.dvCardRecord.Count) + " )", this.cn))
                    {
                        if (this.cn.State == ConnectionState.Closed)
                        {
                            this.cn.Open();
                        }
                        command2.ExecuteNonQuery();
                        continue;
                    }
                }
                reader.Close();
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
                this.shiftAttReportImportFromAttendenceData();
                this.shiftAttStatisticImportFromAttStatistic();
                this.logCreateReport();
                if (this.CreateCompleteEvent != null)
                {
                    this.CreateCompleteEvent(true, "");
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                try
                {
                    if (this.CreateCompleteEvent != null)
                    {
                        this.CreateCompleteEvent(false, exception.ToString());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void make4OneTime()
        {
            this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.dtCardRecord2 = new DataTable();
            this.dsAtt = new DataSet("Attendance");
            this.dsAtt.Clear();
            this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
            this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
            this.daHolidayType = new SqlDataAdapter("SELECT * FROM t_a_HolidayType", this.cn);
            this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
            this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
            this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
            this.dtCardRecord2 = this.dsAtt.Tables["AllCardRecords"];
            this.dtCardRecord2.Clear();
            this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
            this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
            this.getAttendenceParam();
            this._clearAttendenceData();
            this._clearAttStatistic();
            this.daHoliday.Fill(this.dsAtt, "Holiday");
            this.dtHoliday = this.dsAtt.Tables["Holiday"];
            this.localizedHoliday(this.dtHoliday);
            this.dvHoliday = new DataView(this.dtHoliday);
            this.dvHoliday.RowFilter = "";
            this.dvHoliday.Sort = " f_NO ASC ";
            this.daLeave.Fill(this.dsAtt, "Leave");
            this.dtLeave = this.dsAtt.Tables["Leave"];
            this.dvLeave = new DataView(this.dtLeave);
            this.dvLeave.RowFilter = "";
            this.dvLeave.Sort = " f_NO ASC ";
            this.daHolidayType.Fill(this.dsAtt, "HolidayType");
            this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
            this.localizedHolidayType(this.dtHolidayType);
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
            }
            else
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
            }
            this.cnConsumer.Open();
            SqlDataReader reader = this.cmdConsumer.ExecuteReader();
            int num = 0;
            try
            {
                int num2 = 0;
                while (reader.Read())
                {
                    num = (int) reader["f_ConsumerID"];
                    num2++;
                    string selectCommandText = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
                    selectCommandText = ((((selectCommandText + " FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON ( t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ) ") + 
                        " WHERE f_ConsumerID=" + num.ToString()) + 
                        " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + 
                        " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + 
                        " AND t_b_Reader.f_Attend = 1 ";
                    if (wgAppConfig.getSystemParamByNO(0x36) == "1")
                    {
                        selectCommandText = selectCommandText + " AND f_Character >= 1 ";
                    }
                    selectCommandText = selectCommandText + " ORDER BY f_ReadDate ASC ";
                    this.daCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    selectCommandText = "SELECT f_ReadDate,f_Character ";
                    selectCommandText = ((((selectCommandText + string.Format(", {0} as f_Type", wgTools.PrepareStr(CommonStr.strSignIn)) + " FROM t_d_ManualCardRecord  ") + " WHERE f_ConsumerID=" + num.ToString()) + " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + " ORDER BY f_ReadDate ASC ";
                    this.daManualCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    decimal[] numArray = new decimal[0x20];
                    DataRow row = null;
                    if (this.DealingNumEvent != null)
                    {
                        this.DealingNumEvent(num2);
                    }
                    this.gProcVal = num2 + 1;
                    if (this.bStopCreate)
                    {
                        return;
                    }
                    DateTime time3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    DateTime time4 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
                    DateTime expression = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    decimal num8 = 0M;
                    decimal num9 = 0M;
                    int num10 = 0;
                    int num11 = 0;
                    int num12 = 0;
                    int index = 0;
                    while (index <= (numArray.Length - 1))
                    {
                        numArray[index] = 0M;
                        index++;
                    }
                    this.dtCardRecord2 = this.dsAtt.Tables["AllCardRecords"];
                    this.dsAtt.Tables["AllCardRecords"].Clear();
                    this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.dvCardRecord = new DataView(this.dtCardRecord2);
                    this.dvCardRecord.RowFilter = "";
                    this.dvCardRecord.Sort = " f_ReadDate ASC ";
                    int num13 = 0;
                    while (this.dvCardRecord.Count > (num13 + 1))
                    {
                        DateTime time11 = (DateTime) this.dvCardRecord[num13 + 1][0];
                        if (time11.Subtract((DateTime) this.dvCardRecord[num13][0]).TotalSeconds < this.tTwoReadMintime)
                        {
                            this.dvCardRecord[num13 + 1].Delete();
                        }
                        else
                        {
                            num13++;
                        }
                    }
                    while (expression <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
                    {
                        row = this.dtAttendenceData.NewRow();
                        row["f_ConsumerID"] = num;
                        row["f_AttDate"] = expression;
                        row["f_LateTime"] = 0;
                        row["f_LeaveEarlyTime"] = 0;
                        row["f_OvertimeTime"] = 0;
                        row["f_AbsenceDay"] = 0;
                        bool flag2 = true;
                        bool flag3 = true;
                        bool flag4 = false;
                        this.dvCardRecord.RowFilter = "  f_ReadDate >= #" + expression.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(expression.AddDays((double) this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
                        this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
                        if (this.dvCardRecord.Count > 0)
                        {
                            DateTime time2 = Convert.ToDateTime(this.dvCardRecord[0]["f_ReadDate"]);
                            row["f_Onduty1"] = time2;
                            row["f_Onduty1Desc"] = this.dvCardRecord[0]["f_Type"];
                            if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) > 0)
                            {
                                if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                {
                                    row["f_Onduty1Desc"] = CommonStr.strLateness;
                                }
                                else
                                {
                                    row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                }
                            }
                        }
                        else
                        {
                            row["f_OnDuty1Desc"] = CommonStr.strAbsence;
                        }
                        int num14 = 3;
                        this.dvHoliday.RowFilter = " f_NO =1 ";
                        if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Saturday))
                        {
                            num14 = 0;
                        }
                        else
                        {
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num14 = 1;
                            }
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num14 = 2;
                            }
                            this.dvHoliday.RowFilter = " f_NO =2 ";
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Sunday))
                            {
                                num14 = 0;
                            }
                            else
                            {
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num14 = 1;
                                }
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num14 = 2;
                                }
                                this.dvHoliday.RowFilter = " f_TYPE =2 ";
                                for (int i = 0; i <= (this.dvHoliday.Count - 1); i++)
                                {
                                    DateTime time5;
                                    DateTime time6;
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                    DateTime.TryParse(this.strTemp, out time5);
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value2"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                    DateTime.TryParse(this.strTemp, out time6);
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num14 = 0;
                                        break;
                                    }
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time6))
                                    {
                                        num14 = 2;
                                    }
                                    if ((time5 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num14 = 1;
                                    }
                                }
                            }
                        }
                        if (num14 != 3)
                        {
                            this.dvHoliday.RowFilter = " f_TYPE =3 ";
                            for (int j = 0; j <= (this.dvHoliday.Count - 1); j++)
                            {
                                DateTime time7;
                                DateTime time8;
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime.TryParse(this.strTemp, out time7);
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime.TryParse(this.strTemp, out time8);
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    num14 = 3;
                                    break;
                                }
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time8))
                                {
                                    if (num14 == 2)
                                    {
                                        num14 = 3;
                                    }
                                    else
                                    {
                                        num14 = 1;
                                    }
                                }
                                if ((time7 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    if (num14 == 1)
                                    {
                                        num14 = 3;
                                    }
                                    else
                                    {
                                        num14 = 2;
                                    }
                                }
                            }
                        }
                        switch (num14)
                        {
                            case 0:
                                row["f_Onduty1"] = DBNull.Value;
                                row["f_OnDuty1Desc"] = CommonStr.strRest;
                                flag2 = false;
                                flag3 = false;
                                break;

                            case 1:
                                break;

                            default:
                                if ((num14 == 2) && (this.SetObjToStr(row["f_Onduty1"]) != ""))
                                {
                                    if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tlateAbsenceTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                    {
                                        flag4 = true;
                                        row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                    }
                                    else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tLateTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                    {
                                        flag4 = true;
                                        row["f_Onduty1Desc"] = CommonStr.strLateness;
                                    }
                                    else
                                    {
                                        row["f_Onduty1Desc"] = "";
                                    }
                                }
                                break;
                        }
                        if (this.dvLeave.Count > 0)
                        {
                            DateTime now = DateTime.Now;
                            DateTime result = DateTime.Now;
                            string str2 = "";
                            string str3 = "";
                            num14 = 3;
                            for (int k = 0; k <= (this.dvLeave.Count - 1); k++)
                            {
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime.TryParse(this.strTemp, out now);
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime.TryParse(this.strTemp, out result);
                                str3 = Convert.ToString(this.dvLeave[k]["f_HolidayType"]);
                                if ((now <= expression) && (Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= result))
                                {
                                    str2 = str3;
                                    num14 = 0;
                                    break;
                                }
                                if ((now <= expression) && (Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= result))
                                {
                                    str2 = str3;
                                    if (num14 == 1)
                                    {
                                        num14 = 0;
                                        break;
                                    }
                                    num14 = 2;
                                }
                                if ((now <= Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= result))
                                {
                                    if (num14 == 2)
                                    {
                                        num14 = 0;
                                        break;
                                    }
                                    num14 = 1;
                                }
                            }
                            switch (num14)
                            {
                                case 0:
                                    row["f_OnDuty1Desc"] = str2;
                                    row["f_OnDuty1"] = DBNull.Value;
                                    break;

                                case 1:
                                    break;

                                default:
                                    if ((num14 == 2) && (this.SetObjToStr(row["f_Onduty1"]) != ""))
                                    {
                                        row["f_OnDuty1Desc"] = str2;
                                        if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tlateAbsenceTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                        {
                                            flag4 = true;
                                            row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                        }
                                        else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tLateTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                        {
                                            flag4 = true;
                                            row["f_Onduty1Desc"] = CommonStr.strLateness;
                                        }
                                    }
                                    break;
                            }
                        }
                        if (this.bChooseTwoTimes)
                        {
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strLateness)
                            {
                                row["f_OnDuty1Desc"] = "";
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
                            {
                                row["f_OffDuty1Desc"] = "";
                            }
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strOvertime)
                            {
                                row["f_OnDuty1Desc"] = "";
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strOvertime)
                            {
                                row["f_OffDuty1Desc"] = "";
                            }
                            row["f_OvertimeTime"] = 0;
                            if ((this.SetObjToStr(row["f_OnDuty1"]) != "") && (this.SetObjToStr(row["f_OffDuty1"]) != ""))
                            {
                                row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OnDuty1"], "yyyy-MM-dd HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "yyyy-MM-dd HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                            }
                            if (((decimal) row["f_OvertimeTime"]) >= this.needDutyHour)
                            {
                                flag3 = true;
                            }
                            else
                            {
                                flag3 = false;
                            }
                        }
                        if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strLateness)
                        {
                            if (flag4)
                            {
                                row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "13:30:00")), DateTime.Parse(Strings.Format(row["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                            }
                            else
                            {
                                row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                            }
                        }
                        if (this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)
                        {
                            row["f_AbsenceDay"] = this.tLateAbsenceDay + this.tLeaveAbsenceDay;
                            flag3 = false;
                        }
                        if ((Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1M) && (num14 != 3))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0M;
                        }
                        selectCommandText = "";
                        selectCommandText = " INSERT INTO t_d_AttendenceData ";
                        using (SqlCommand command = new SqlCommand(((((((((((((selectCommandText + " ([f_ConsumerID], [f_AttDate], " + "[f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc]") + ", [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]  " + " ) ") + " VALUES ( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(row["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1Desc"])) + " , " + this.PrepareStr(row["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Offduty1Desc"])) + " , " + row["f_LateTime"]) + " , " + row["f_LeaveEarlyTime"]) + " , " + this.getDecimalStr(row["f_OvertimeTime"])) + " , " + this.getDecimalStr(row["f_AbsenceDay"])) + " ) ", this.cn))
                        {
                            if (this.cn.State == ConnectionState.Closed)
                            {
                                this.cn.Open();
                            }
                            command.ExecuteNonQuery();
                        }
                        if (flag2)
                        {
                            num4++;
                        }
                        string str4 = "";
                        for (num13 = 0; num13 <= 1; num13++)
                        {
                            switch (num13)
                            {
                                case 0:
                                    str4 = this.SetObjToStr(row["f_OnDuty1Desc"]);
                                    break;

                                case 1:
                                    str4 = this.SetObjToStr(row["f_OffDuty1Desc"]);
                                    break;
                            }
                            if (str4 == CommonStr.strLateness)
                            {
                                num6++;
                                flag3 = false;
                            }
                            else if (str4 == CommonStr.strLeaveEarly)
                            {
                                num7++;
                                flag3 = false;
                            }
                            else if (str4 == CommonStr.strNotReadCard)
                            {
                                num10++;
                                flag3 = false;
                            }
                            else
                            {
                                index = 0;
                                while (index <= (this.dtHolidayType.Rows.Count - 1))
                                {
                                    if (index >= numArray.Length)
                                    {
                                        break;
                                    }
                                    if (str4 == Convert.ToString(this.dtHolidayType.Rows[index][1]))
                                    {
                                        flag3 = false;
                                        if (this.bChooseOnlyOnDuty)
                                        {
                                            numArray[index] += 1.0M;
                                        }
                                        else
                                        {
                                            numArray[index] += 0.5M;
                                        }
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                        if ((this.SetObjToStr(row["f_OnDuty1"]) == "") && (this.SetObjToStr(row["f_OffDuty1"]) == ""))
                        {
                            flag3 = false;
                        }
                        num8 += Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture);
                        num9 += Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture);
                        num11 += Convert.ToInt32(row["f_LateTime"]);
                        num12 += Convert.ToInt32(row["f_LeaveEarlyTime"]);
                        if (((Convert.ToInt32(row["f_LateTime"]) != 0) || (Convert.ToInt32(row["f_LeaveEarlyTime"]) != 0)) || (Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0M))
                        {
                            flag3 = false;
                        }
                        if (flag3)
                        {
                            num5++;
                        }
                        expression = expression.AddDays(1.0);
                        Application.DoEvents();
                    }
                    this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
                    selectCommandText = "";
                    selectCommandText = " Insert Into t_d_AttStatistic ";
                    selectCommandText = (selectCommandText + " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd] ") + " , [f_DayShouldWork],  [f_DayRealWork]" + " , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
                    index = 1;
                    while (index <= 0x20)
                    {
                        object obj2 = selectCommandText;
                        selectCommandText = string.Concat(new object[] { obj2, " , [f_SpecialType", index, "]" });
                        index++;
                    }
                    selectCommandText = (((((((((((selectCommandText + ", f_LateMinutes" + ", f_LeaveEarlyMinutes") + ", f_ManualReadTimesCount" + " ) ") + " Values( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(time3, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(time4, true, "yyyy-MM-dd HH:mm:ss")) + " , " + num4) + " , " + num5) + " , " + num6) + " , " + num7) + " , " + this.getDecimalStr(num9)) + " , " + this.getDecimalStr(num8)) + " , " + num10;
                    for (index = 0; index <= 0x1f; index++)
                    {
                        selectCommandText = selectCommandText + " , " + this.PrepareStr(this.getDecimalStr(numArray[index]));
                    }
                    using (SqlCommand command2 = new SqlCommand((((selectCommandText + ", " + num11) + ", " + num12) + ", " + this.dvCardRecord.Count) + " )", this.cn))
                    {
                        if (this.cn.State == ConnectionState.Closed)
                        {
                            this.cn.Open();
                        }
                        command2.ExecuteNonQuery();
                        continue;
                    }
                }
                reader.Close();
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
                this.shiftAttReportImportFromAttendenceData();
                this.shiftAttStatisticImportFromAttStatistic();
                this.logCreateReport();
                if (this.CreateCompleteEvent != null)
                {
                    this.CreateCompleteEvent(true, "");
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                try
                {
                    if (this.CreateCompleteEvent != null)
                    {
                        this.CreateCompleteEvent(false, exception.ToString());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void make4TwoTimes()
        {
            this.cnConsumer = new SqlConnection(wgAppConfig.dbConString);
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.dtCardRecord = new DataTable();
            this.dsAtt = new DataSet("Attendance");
            this.dsAtt.Clear();
            this.daAttendenceData = new SqlDataAdapter("SELECT * FROM t_d_AttendenceData WHERE 1<0", this.cn);
            this.daHoliday = new SqlDataAdapter("SELECT * FROM t_a_Holiday ORDER BY  f_NO ASC", this.cn);
            this.daHolidayType = new SqlDataAdapter("SELECT * FROM t_a_HolidayType", this.cn);
            this.daLeave = new SqlDataAdapter("SELECT * FROM t_d_Leave", this.cn);
            this.daNoCardRecord = new SqlDataAdapter("SELECT f_ReadDate,f_Character,'' as f_Type  FROM t_d_ManualCardRecord Where 1<0 ", this.cn);
            this.daNoCardRecord.Fill(this.dsAtt, "AllCardRecords");
            this.dtCardRecord = this.dsAtt.Tables["AllCardRecords"];
            this.dtCardRecord.Clear();
            this.daAttendenceData.Fill(this.dsAtt, "AttendenceData");
            this.dtAttendenceData = this.dsAtt.Tables["AttendenceData"];
            this.getAttendenceParam();
            this._clearAttendenceData();
            this._clearAttStatistic();
            this.daHoliday.Fill(this.dsAtt, "Holiday");
            this.dtHoliday = this.dsAtt.Tables["Holiday"];
            this.localizedHoliday(this.dtHoliday);
            this.dvHoliday = new DataView(this.dtHoliday);
            this.dvHoliday.RowFilter = "";
            this.dvHoliday.Sort = " f_NO ASC ";
            this.daLeave.Fill(this.dsAtt, "Leave");
            this.dtLeave = this.dsAtt.Tables["Leave"];
            this.dvLeave = new DataView(this.dtLeave);
            this.dvLeave.RowFilter = "";
            this.dvLeave.Sort = " f_NO ASC ";
            this.daHolidayType.Fill(this.dsAtt, "HolidayType");
            this.dtHolidayType = this.dsAtt.Tables["HolidayType"];
            this.localizedHolidayType(this.dtHolidayType);
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 AND f_ShiftEnabled =0) ", this.cnConsumer);
            }
            else
            {
                this.cmdConsumer = new SqlCommand(this.strConsumerSql + "AND (f_AttendEnabled >0 ) ", this.cnConsumer);
            }
            this.cnConsumer.Open();
            SqlDataReader reader = this.cmdConsumer.ExecuteReader();
            int num = 0;
            try
            {
                int num2 = 0;
                while (reader.Read())
                {
                    num = (int) reader["f_ConsumerID"];
                    num2++;
                    string selectCommandText = "SELECT f_ReadDate,f_Character,'' as f_Type  ";
                    selectCommandText = ((((selectCommandText + " FROM t_d_SwipeRecord INNER JOIN t_b_Reader ON t_b_Reader.f_Attend=1 AND t_d_SwipeRecord.f_ReaderID =t_b_Reader.f_ReaderID ") + " WHERE f_ConsumerID=" + num.ToString()) + 
                        " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + 
                        " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + 
                        " AND t_b_Reader.f_Attend = 1 ";
                    if (wgAppConfig.getSystemParamByNO(0x36) == "1")
                    {
                        selectCommandText = selectCommandText + " AND f_Character >= 1 ";
                    }
                    selectCommandText = selectCommandText + " ORDER BY f_ReadDate ASC ";
                    this.daCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    selectCommandText = "SELECT f_ReadDate,f_Character ";
                    selectCommandText = ((((selectCommandText + string.Format(", {0} as f_Type", wgTools.PrepareStr(CommonStr.strSignIn)) + " FROM t_d_ManualCardRecord  ") + " WHERE f_ConsumerID=" + num.ToString()) + " AND ([f_ReadDate]>= " + this.PrepareStr(this.startDateTime, true, "yyyy-MM-dd 00:00:00") + ") ") + " AND ([f_ReadDate]<= " + this.PrepareStr(this.endDateTime.AddDays(1.0), true, "yyyy-MM-dd 23:59:59") + ") ") + " ORDER BY f_ReadDate ASC ";
                    this.daManualCardRecord = new SqlDataAdapter(selectCommandText, this.cn);
                    decimal[] numArray = new decimal[0x20];
                    DataRow row = null;
                    if (this.DealingNumEvent != null)
                    {
                        this.DealingNumEvent(num2);
                    }
                    this.gProcVal = num2 + 1;
                    if (this.bStopCreate)
                    {
                        return;
                    }
                    DateTime time3 = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    DateTime time4 = DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime));
                    DateTime expression = DateTime.Parse(Strings.Format(this.startDateTime, "yyyy-MM-dd " + this.strAllowOndutyTime));
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    decimal num9 = 0M;
                    decimal num10 = 0M;
                    int num11 = 0;
                    int num12 = 0;
                    int num13 = 0;
                    int index = 0;
                    while (index <= (numArray.Length - 1))
                    {
                        numArray[index] = 0M;
                        index++;
                    }
                    this.dtCardRecord = this.dsAtt.Tables["AllCardRecords"];
                    this.dsAtt.Tables["AllCardRecords"].Clear();
                    this.daCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.daManualCardRecord.Fill(this.dsAtt, "AllCardRecords");
                    this.dvCardRecord = new DataView(this.dtCardRecord);
                    this.dvCardRecord.RowFilter = "";
                    this.dvCardRecord.Sort = " f_ReadDate ASC ";
                    int num14 = 0;
                    while (this.dvCardRecord.Count > (num14 + 1))
                    {
                        DateTime time11 = (DateTime) this.dvCardRecord[num14 + 1][0];
                        if (time11.Subtract((DateTime) this.dvCardRecord[num14][0]).TotalSeconds < this.tTwoReadMintime)
                        {
                            this.dvCardRecord[num14 + 1].Delete();
                        }
                        else
                        {
                            num14++;
                        }
                    }
                    while (expression <= DateTime.Parse(Strings.Format(this.endDateTime.AddDays((double) this.normalDay), "yyyy-MM-dd " + this.strAllowOffdutyTime)))
                    {
                        row = this.dtAttendenceData.NewRow();
                        row["f_ConsumerID"] = num;
                        row["f_AttDate"] = expression;
                        row["f_LateTime"] = 0;
                        row["f_LeaveEarlyTime"] = 0;
                        row["f_OvertimeTime"] = 0;
                        row["f_AbsenceDay"] = 0;
                        bool flag2 = true;
                        bool flag3 = true;
                        bool flag4 = false;
                        bool flag5 = false;
                        this.dvCardRecord.RowFilter = "  f_ReadDate >= #" + expression.ToString("yyyy-MM-dd HH:mm:ss") + "# and f_ReadDate<= " + Strings.Format(expression.AddDays((double) this.normalDay), "#yyyy-MM-dd " + this.strAllowOffdutyTime + "#");
                        this.dvLeave.RowFilter = " f_ConsumerID = " + num.ToString();
                        if (this.dvCardRecord.Count > 0)
                        {
                            DateTime time2;
                            int num4 = 0;
                            while (num4 <= (this.dvCardRecord.Count - 1))
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (string.Compare(Strings.Format(time2, "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty0, "HH:mm:ss")) <= 0)
                                    {
                                        if (this.bEarliestAsOnDuty || this.bChooseTwoTimes)
                                        {
                                            if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                            {
                                                row["f_Onduty1"] = time2;
                                                row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                            }
                                        }
                                        else
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        }
                                        goto Label_0A4D;
                                    }
                                    if (this.SetObjToStr(row["f_Onduty1"]) == "")
                                    {
                                        if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double) this.tLateTimeout), "HH:mm:ss")) <= 0)
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOnduty0.AddMinutes((double) this.tlateAbsenceTimeout), "HH:mm:ss")) < 0)
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = CommonStr.strLateness;
                                        }
                                        else if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double) -this.tLeaveTimeout), "HH:mm:ss")) > 0)
                                        {
                                            row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                        }
                                        else
                                        {
                                            row["f_Onduty1"] = time2;
                                            row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                        }
                                    }
                                }
                                else if (!(this.SetObjToStr(row["f_Onduty1"]) != ""))
                                {
                                    row["f_Onduty1Desc"] = CommonStr.strNotReadCard;
                                }
                                break;
                            Label_0A4D:
                                num4++;
                            }
                            while (num4 <= (this.dvCardRecord.Count - 1))
                            {
                                time2 = Convert.ToDateTime(this.dvCardRecord[num4]["f_ReadDate"]);
                                if (string.Compare(Strings.Format(time2, "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                {
                                    if (string.Compare(Strings.Format(time2, "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) <= 0)
                                    {
                                        row["f_Offduty1"] = time2;
                                        row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                        if (string.Compare(Strings.Format(time2.AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) < 0)
                                        {
                                            if (string.Compare(Strings.Format(time2.AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) < 0)
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                            }
                                            else
                                            {
                                                row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        row["f_Offduty1"] = time2;
                                        row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                    }
                                }
                                else
                                {
                                    row["f_Offduty1"] = time2;
                                    row["f_Offduty1Desc"] = this.dvCardRecord[num4]["f_Type"];
                                }
                                num4++;
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == this.SetObjToStr(row["f_Onduty1"])) && ((this.SetObjToStr(row["f_Offduty1Desc"]).IndexOf(CommonStr.strLeaveEarly) >= 0) || (this.SetObjToStr(row["f_Offduty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0)))
                            {
                                row["f_Offduty1"] = DBNull.Value;
                                row["f_Offduty1Desc"] = CommonStr.strNotReadCard;
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == "") && (this.SetObjToStr(row["f_Onduty1"]) == ""))
                            {
                                row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                row["f_Offduty1Desc"] = CommonStr.strAbsence;
                            }
                            if ((this.SetObjToStr(row["f_Offduty1"]) == "") && (this.SetObjToStr(row["f_Offduty1Desc"]) == ""))
                            {
                                row["f_Offduty1Desc"] = CommonStr.strNotReadCard;
                            }
                            if ((this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0))
                            {
                                row["f_OnDuty1Desc"] = "";
                            }
                        }
                        else
                        {
                            row["f_OnDuty1Desc"] = CommonStr.strAbsence;
                            row["f_OffDuty1Desc"] = CommonStr.strAbsence;
                        }
                        int num15 = 3;
                        this.dvHoliday.RowFilter = " f_NO =1 ";
                        if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Saturday))
                        {
                            num15 = 0;
                        }
                        else
                        {
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num15 = 1;
                            }
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Saturday))
                            {
                                num15 = 2;
                            }
                            this.dvHoliday.RowFilter = " f_NO =2 ";
                            if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 0) && (expression.DayOfWeek == DayOfWeek.Sunday))
                            {
                                num15 = 0;
                            }
                            else
                            {
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 1) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num15 = 1;
                                }
                                if ((Convert.ToInt32(this.dvHoliday[0]["f_Value"]) == 2) && (expression.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    num15 = 2;
                                }
                                this.dvHoliday.RowFilter = " f_TYPE =2 ";
                                for (int i = 0; i <= (this.dvHoliday.Count - 1); i++)
                                {
                                    DateTime time5;
                                    DateTime time6;
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                    DateTime.TryParse(this.strTemp, out time5);
                                    this.strTemp = Convert.ToString(this.dvHoliday[i]["f_Value2"]);
                                    this.strTemp = this.strTemp + " " + ((this.dvHoliday[i]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                    DateTime.TryParse(this.strTemp, out time6);
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num15 = 0;
                                        break;
                                    }
                                    if ((time5 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time6))
                                    {
                                        num15 = 2;
                                    }
                                    if ((time5 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time6))
                                    {
                                        num15 = 1;
                                    }
                                }
                            }
                        }
                        if (num15 != 3)
                        {
                            this.dvHoliday.RowFilter = " f_TYPE =3 ";
                            for (int j = 0; j <= (this.dvHoliday.Count - 1); j++)
                            {
                                DateTime time7;
                                DateTime time8;
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime.TryParse(this.strTemp, out time7);
                                this.strTemp = Convert.ToString(this.dvHoliday[j]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvHoliday[j]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime.TryParse(this.strTemp, out time8);
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    num15 = 3;
                                    break;
                                }
                                if ((time7 <= expression) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= time8))
                                {
                                    if (num15 == 2)
                                    {
                                        num15 = 3;
                                    }
                                    else
                                    {
                                        num15 = 1;
                                    }
                                }
                                if ((time7 <= DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (DateTime.Parse(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= time8))
                                {
                                    if (num15 == 1)
                                    {
                                        num15 = 3;
                                    }
                                    else
                                    {
                                        num15 = 2;
                                    }
                                }
                            }
                        }
                        switch (num15)
                        {
                            case 0:
                                if ((this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strRest;
                                }
                                if ((this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strNotReadCard) || (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence))
                                {
                                    row["f_OffDuty1Desc"] = CommonStr.strRest;
                                }
                                row["f_OnDuty1Desc"] = CommonStr.strRest;
                                row["f_OffDuty1Desc"] = CommonStr.strRest;
                                if ((this.SetObjToStr(row["f_Onduty1"]) != "") && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                {
                                    row["f_OnDuty1Desc"] = CommonStr.strOvertime;
                                    row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                    if (string.Compare(Strings.Format(row["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                    {
                                        row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-1 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                    }
                                    else
                                    {
                                        row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_Onduty1"], "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                    }
                                }
                                flag2 = false;
                                flag3 = false;
                                break;

                            case 1:
                                if (this.SetObjToStr(row["f_Offduty1"]) != "")
                                {
                                    if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Offduty1"]).AddMinutes((double) this.tLeaveTimeout), "HH:mm:ss"), "12:00:00") < 0)
                                    {
                                        if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Offduty1"]).AddMinutes((double) this.tLeaveAbsenceTimeout), "HH:mm:ss"), "12:00:00") < 0)
                                        {
                                            flag5 = true;
                                            row["f_Offduty1Desc"] = CommonStr.strAbsence;
                                        }
                                        else
                                        {
                                            flag5 = true;
                                            row["f_Offduty1Desc"] = CommonStr.strLeaveEarly;
                                        }
                                    }
                                    else
                                    {
                                        row["f_Offduty1Desc"] = "";
                                    }
                                }
                                if (this.SetObjToStr(row["f_Offduty1"]) != "")
                                {
                                    if (Strings.Format(row["f_Offduty1"], "yyyy-MM-dd") == Strings.Format(expression, "yyyy-MM-dd"))
                                    {
                                        if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Offduty1"]).AddMinutes((double) -this.tOvertimeTimeout), "HH:mm:ss"), "12:00:00") >= 0)
                                        {
                                            row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                            row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse("2000-1-1 12:00:00"), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-1 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                        }
                                    }
                                    else
                                    {
                                        row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                        row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 12:00:00")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                    }
                                }
                                break;

                            case 2:
                                if (this.SetObjToStr(row["f_Onduty1"]) != "")
                                {
                                    if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tlateAbsenceTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                    {
                                        flag4 = true;
                                        row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                    }
                                    else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tLateTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                    {
                                        flag4 = true;
                                        row["f_Onduty1Desc"] = CommonStr.strLateness;
                                    }
                                    else
                                    {
                                        row["f_Onduty1Desc"] = "";
                                    }
                                    if (this.SetObjToStr(row["f_Offduty1"]) != "")
                                    {
                                        if (string.Compare(Strings.Format(row["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                        {
                                            if ((string.Compare(Strings.Format(row["f_Offduty1"], "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss")) >= 0) && (string.Compare(Strings.Format(this.tOffduty0.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) >= 0))
                                            {
                                                row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                                row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                            }
                                        }
                                        else
                                        {
                                            row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                            row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                        }
                                    }
                                }
                                break;

                            default:
                                if (num15 == 2)
                                {
                                    if (this.SetObjToStr(row["f_Onduty1"]) != "")
                                    {
                                        if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tlateAbsenceTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                        {
                                            flag4 = true;
                                            row["f_Onduty1Desc"] = CommonStr.strAbsence;
                                        }
                                        else if (string.Compare(Strings.Format(Convert.ToDateTime(row["f_Onduty1"]).AddMinutes((double) -this.tLateTimeout), "HH:mm:ss"), "13:30:00") > 0)
                                        {
                                            flag4 = true;
                                            row["f_Onduty1Desc"] = CommonStr.strLateness;
                                        }
                                        else
                                        {
                                            row["f_Onduty1Desc"] = "";
                                        }
                                        if (this.SetObjToStr(row["f_Offduty1"]) != "")
                                        {
                                            if (string.Compare(Strings.Format(row["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                            {
                                                if ((string.Compare(Strings.Format(row["f_Offduty1"], "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss")) >= 0) && (string.Compare(Strings.Format(this.tOffduty0.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) >= 0))
                                                {
                                                    row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                                    row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                                }
                                            }
                                            else
                                            {
                                                row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                                row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                            }
                                        }
                                    }
                                }
                                else if ((num15 == 3) && (this.SetObjToStr(row["f_Offduty1"]) != ""))
                                {
                                    if (string.Compare(Strings.Format(row["f_Offduty1"], "yyyy-MM-dd"), Strings.Format(expression, "yyyy-MM-dd")) == 0)
                                    {
                                        if ((string.Compare(Strings.Format(row["f_Offduty1"], "HH:mm:ss"), Strings.Format(this.tOffduty0.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss")) >= 0) && (string.Compare(Strings.Format(this.tOffduty0.AddMinutes((double) this.tOvertimeTimeout), "HH:mm:ss"), Strings.Format(this.tOffduty0, "HH:mm:ss")) >= 0))
                                        {
                                            row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                            row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                        }
                                    }
                                    else
                                    {
                                        row["f_OffDuty1Desc"] = CommonStr.strOvertime;
                                        row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOffduty0, "2000-1-1 HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "2000-1-2 HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                                    }
                                }
                                break;
                        }
                        if (this.dvLeave.Count > 0)
                        {
                            DateTime now = DateTime.Now;
                            DateTime result = DateTime.Now;
                            string str2 = "";
                            string str3 = "";
                            string str4 = "";
                            num15 = 3;
                            for (int k = 0; k <= (this.dvLeave.Count - 1); k++)
                            {
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value1"].ToString() == CommonStr.strAM) ? "00:00:00" : "12:00:00");
                                DateTime.TryParse(this.strTemp, out now);
                                this.strTemp = Convert.ToString(this.dvLeave[k]["f_Value2"]);
                                this.strTemp = this.strTemp + " " + ((this.dvLeave[k]["f_Value3"].ToString() == CommonStr.strAM) ? "12:00:00" : "23:59:59");
                                DateTime.TryParse(this.strTemp, out result);
                                str4 = Convert.ToString(this.dvLeave[k]["f_HolidayType"]);
                                if ((now <= expression) && (Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= result))
                                {
                                    str2 = str4;
                                    str3 = str4;
                                    num15 = 0;
                                    break;
                                }
                                if ((now <= expression) && (Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 12:00:00")) <= result))
                                {
                                    str2 = str4;
                                    if (num15 == 1)
                                    {
                                        num15 = 0;
                                        break;
                                    }
                                    num15 = 2;
                                }
                                if ((now <= Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 12:00:01"))) && (Convert.ToDateTime(Strings.Format(expression, "yyyy-MM-dd 23:59:59")) <= result))
                                {
                                    str3 = str4;
                                    if (num15 == 2)
                                    {
                                        num15 = 0;
                                        break;
                                    }
                                    num15 = 1;
                                }
                            }
                            switch (num15)
                            {
                                case 0:
                                    row["f_OnDuty1Desc"] = str2;
                                    row["f_OffDuty1Desc"] = str3;
                                    row["f_OnDuty1"] = DBNull.Value;
                                    row["f_OffDuty1"] = DBNull.Value;
                                    break;

                                case 1:
                                    row["f_OffDuty1Desc"] = str3;
                                    row["f_OffDuty1"] = DBNull.Value;
                                    break;

                                default:
                                    if (num15 == 2)
                                    {
                                        if (this.SetObjToStr(row["f_OnDuty2Desc"]) == CommonStr.strNotReadCard)
                                        {
                                            if (this.SetObjToStr(row["f_OnDuty1"]) != "")
                                            {
                                                row["f_OnDuty2"] = row["f_OnDuty1"];
                                                row["f_OnDuty1"] = DBNull.Value;
                                                row["f_OffDuty1"] = DBNull.Value;
                                            }
                                            else if (this.SetObjToStr(row["f_OffDuty1"]) != "")
                                            {
                                                row["f_OnDuty2"] = row["f_OffDuty1"];
                                                row["f_OffDuty1"] = DBNull.Value;
                                            }
                                        }
                                        row["f_OnDuty1Desc"] = str2;
                                        row["f_OnDuty1"] = DBNull.Value;
                                    }
                                    break;
                            }
                        }
                        if (this.bChooseTwoTimes)
                        {
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strLateness)
                            {
                                row["f_OnDuty1Desc"] = "";
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
                            {
                                row["f_OffDuty1Desc"] = "";
                            }
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strOvertime)
                            {
                                row["f_OnDuty1Desc"] = "";
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strOvertime)
                            {
                                row["f_OffDuty1Desc"] = "";
                            }
                            row["f_OvertimeTime"] = 0;
                            if ((this.SetObjToStr(row["f_OnDuty1"]) != "") && (this.SetObjToStr(row["f_OffDuty1"]) != ""))
                            {
                                row["f_OvertimeTime"] = Conversion.Int((long) (DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OnDuty1"], "yyyy-MM-dd HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_Offduty1"], "yyyy-MM-dd HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) / 30L)) / 2.0M;
                            }
                            if (((decimal) row["f_OvertimeTime"]) >= this.needDutyHour)
                            {
                                flag3 = true;
                            }
                            else
                            {
                                flag3 = false;
                            }
                        }
                        if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strLateness)
                        {
                            if (flag4)
                            {
                                row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "13:30:00")), DateTime.Parse(Strings.Format(row["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                            }
                            else
                            {
                                row["f_LateTime"] = Convert.ToInt32(row["f_LateTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(this.tOnduty0, "HH:mm:ss")), DateTime.Parse(Strings.Format(row["f_OnDuty1"], "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                            }
                        }
                        if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strLeaveEarly)
                        {
                            if (flag5)
                            {
                                row["f_LeaveEarlyTime"] = Convert.ToInt32(row["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty0, "12:00:00")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                            }
                            else
                            {
                                row["f_LeaveEarlyTime"] = Convert.ToInt32(row["f_LeaveEarlyTime"]) + DateAndTime.DateDiff(DateInterval.Minute, DateTime.Parse(Strings.Format(row["f_OffDuty1"], "HH:mm:ss")), DateTime.Parse(Strings.Format(this.tOffduty0, "HH:mm:ss")), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
                            }
                        }
                        if ((this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0) && (this.SetObjToStr(row["f_OffDuty1Desc"]).IndexOf(CommonStr.strAbsence) >= 0))
                        {
                            row["f_AbsenceDay"] = this.tLateAbsenceDay + this.tLeaveAbsenceDay;
                            flag3 = false;
                        }
                        else
                        {
                            if (this.SetObjToStr(row["f_OnDuty1Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLateAbsenceDay;
                                flag3 = false;
                            }
                            if (this.SetObjToStr(row["f_OffDuty1Desc"]) == CommonStr.strAbsence)
                            {
                                row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) + this.tLeaveAbsenceDay;
                                flag3 = false;
                            }
                        }
                        if (this.SetObjToStr(row["f_OffDuty1"]) == "")
                        {
                            row["f_OvertimeTime"] = 0;
                        }
                        if ((Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) >= 1M) && (num15 != 3))
                        {
                            row["f_AbsenceDay"] = Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) / 2.0M;
                        }
                        selectCommandText = "";
                        selectCommandText = " INSERT INTO t_d_AttendenceData ";
                        using (SqlCommand command = new SqlCommand(((((((((((((selectCommandText + " ([f_ConsumerID], [f_AttDate], " + "[f_Onduty1],[f_Onduty1Desc], [f_Offduty1], [f_Offduty1Desc]") + ", [f_LateTime], [f_LeaveEarlyTime],[f_OvertimeTime], [f_AbsenceDay]  " + " ) ") + " VALUES ( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(row["f_AttDate"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Onduty1Desc"])) + " , " + this.PrepareStr(row["f_Offduty1"], true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(row["f_Offduty1Desc"])) + " , " + row["f_LateTime"]) + " , " + row["f_LeaveEarlyTime"]) + " , " + this.getDecimalStr(row["f_OvertimeTime"])) + " , " + this.getDecimalStr(row["f_AbsenceDay"])) + " ) ", this.cn))
                        {
                            if (this.cn.State == ConnectionState.Closed)
                            {
                                this.cn.Open();
                            }
                            command.ExecuteNonQuery();
                        }
                        if (flag2)
                        {
                            num5++;
                        }
                        string str5 = "";
                        for (num14 = 0; num14 <= 1; num14++)
                        {
                            switch (num14)
                            {
                                case 0:
                                    str5 = this.SetObjToStr(row["f_OnDuty1Desc"]);
                                    break;

                                case 1:
                                    str5 = this.SetObjToStr(row["f_OffDuty1Desc"]);
                                    break;
                            }
                            if (str5 == CommonStr.strLateness)
                            {
                                num7++;
                                flag3 = false;
                            }
                            else if (str5 == CommonStr.strLeaveEarly)
                            {
                                num8++;
                                flag3 = false;
                            }
                            else if (str5 == CommonStr.strNotReadCard)
                            {
                                num11++;
                                flag3 = false;
                            }
                            else
                            {
                                index = 0;
                                while (index <= (this.dtHolidayType.Rows.Count - 1))
                                {
                                    if (index >= numArray.Length)
                                    {
                                        break;
                                    }
                                    if (str5 == Convert.ToString(this.dtHolidayType.Rows[index][1]))
                                    {
                                        flag3 = false;
                                        numArray[index] += 0.5M;
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                        if ((this.SetObjToStr(row["f_OnDuty1"]) == "") && (this.SetObjToStr(row["f_OffDuty1"]) == ""))
                        {
                            flag3 = false;
                        }
                        num9 += Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture);
                        num10 += Convert.ToDecimal(row["f_OvertimeTime"], CultureInfo.InvariantCulture);
                        num12 += Convert.ToInt32(row["f_LateTime"]);
                        num13 += Convert.ToInt32(row["f_LeaveEarlyTime"]);
                        if (((Convert.ToInt32(row["f_LateTime"]) != 0) || (Convert.ToInt32(row["f_LeaveEarlyTime"]) != 0)) || (Convert.ToDecimal(row["f_AbsenceDay"], CultureInfo.InvariantCulture) != 0M))
                        {
                            flag3 = false;
                        }
                        if (flag3)
                        {
                            num6++;
                        }
                        expression = expression.AddDays(1.0);
                        Application.DoEvents();
                    }
                    this.dvCardRecord.RowFilter = string.Format("f_Type ={0}", this.PrepareStr(CommonStr.strSignIn));
                    selectCommandText = "";
                    selectCommandText = " Insert Into t_d_AttStatistic ";
                    selectCommandText = (selectCommandText + " ( [f_ConsumerID], [f_AttDateStart], [f_AttDateEnd] ") + " , [f_DayShouldWork],  [f_DayRealWork]" + " , [f_TotalLate],  [f_TotalLeaveEarly],[f_TotalOvertime], [f_TotalAbsenceDay], [f_TotalNotReadCard]";
                    index = 1;
                    while (index <= 0x20)
                    {
                        object obj2 = selectCommandText;
                        selectCommandText = string.Concat(new object[] { obj2, " , [f_SpecialType", index, "]" });
                        index++;
                    }
                    selectCommandText = (((((((((((selectCommandText + ", f_LateMinutes" + ", f_LeaveEarlyMinutes") + ", f_ManualReadTimesCount" + " ) ") + " Values( " + row["f_ConsumerID"]) + " , " + this.PrepareStr(time3, true, "yyyy-MM-dd HH:mm:ss")) + " , " + this.PrepareStr(time4, true, "yyyy-MM-dd HH:mm:ss")) + " , " + num5) + " , " + num6) + " , " + num7) + " , " + num8) + " , " + this.getDecimalStr(num10)) + " , " + this.getDecimalStr(num9)) + " , " + num11;
                    for (index = 0; index <= 0x1f; index++)
                    {
                        selectCommandText = selectCommandText + " , " + this.PrepareStr(this.getDecimalStr(numArray[index]));
                    }
                    using (SqlCommand command2 = new SqlCommand((((selectCommandText + ", " + num12) + ", " + num13) + ", " + this.dvCardRecord.Count) + " )", this.cn))
                    {
                        if (this.cn.State == ConnectionState.Closed)
                        {
                            this.cn.Open();
                        }
                        command2.ExecuteNonQuery();
                        continue;
                    }
                }
                reader.Close();
                if (this.cn.State != ConnectionState.Closed)
                {
                    this.cn.Close();
                }
                this.shiftAttReportImportFromAttendenceData();
                this.shiftAttStatisticImportFromAttStatistic();
                this.logCreateReport();
                if (this.CreateCompleteEvent != null)
                {
                    this.CreateCompleteEvent(true, "");
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                try
                {
                    if (this.CreateCompleteEvent != null)
                    {
                        this.CreateCompleteEvent(false, exception.ToString());
                    }
                }
                catch (Exception)
                {
                }
            }
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

        public int shiftAttReportImportFromAttendenceData()
        {
            int num = 0;
            int num2 = 0;
            string cmdText = "SELECT * FROM t_d_AttendenceData  ORDER BY f_RecID ";
            this.dtAttendenceData = new DataTable("AttendenceData");
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(this.dtAttendenceData);
                    }
                }
            }
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            this.cmd = new SqlCommand(cmdText, this.cn);
            try
            {
                if (this.dtAttendenceData.Rows.Count > 0)
                {
                    for (int i = 0; i <= (this.dtAttendenceData.Rows.Count - 1); i++)
                    {
                        if (this.DealingNumEvent != null)
                        {
                            this.DealingNumEvent(i);
                        }
                        num2 = 0;
                        DataRow row = this.dtAttendenceData.Rows[i];
                        if (this.SetObjToStr(row["f_OnDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
                        {
                            num2++;
                        }
                        if (this.SetObjToStr(row["f_OffDuty1Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
                        {
                            num2++;
                        }
                        if (this.SetObjToStr(row["f_OnDuty2Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
                        {
                            num2++;
                        }
                        if (this.SetObjToStr(row["f_OffDuty2Desc"]).IndexOf(CommonStr.strNotReadCard) >= 0)
                        {
                            num2++;
                        }
                        cmdText = " INSERT INTO t_d_Shift_AttReport ";
                        cmdText = ((((((((((((((((((((((((((((((((((((((((cmdText + " ( f_ConsumerID, f_shiftDate, f_ShiftID, f_ReadTimes " + " , f_OnDuty1, f_OnDuty1AttDesc, f_OnDuty1CardRecordDesc ") + " , f_OffDuty1, f_OffDuty1AttDesc, f_OffDuty1CardRecordDesc " + " , f_OnDuty2, f_OnDuty2AttDesc, f_OnDuty2CardRecordDesc ") + " , f_OffDuty2, f_OffDuty2AttDesc, f_OffDuty2CardRecordDesc " + " , f_OnDuty3, f_OnDuty3AttDesc, f_OnDuty3CardRecordDesc ") + " , f_OffDuty3, f_OffDuty3AttDesc, f_OffDuty3CardRecordDesc " + " , f_OnDuty4, f_OnDuty4AttDesc, f_OnDuty4CardRecordDesc ") + " , f_OffDuty4, f_OffDuty4AttDesc, f_OffDuty4CardRecordDesc " + " , f_LateMinutes, f_LeaveEarlyMinutes, f_OvertimeHours, f_AbsenceDays ") + " , f_NotReadCardCount, f_bOvertimeShift " + " ) ") + " Values ( " + row["f_ConsumerID"]) + "," + this.PrepareStr(row["f_AttDate"], true, "yyyy-MM-dd")) + "," + this.PrepareStr("")) + "," + this.tReadCardTimes) + "," + this.PrepareStr(row["f_OnDuty1"], true, "yyyy-MM-dd HH:mm:ss")) + "," + this.PrepareStr(row["f_OnDuty1Desc"])) + "," + this.PrepareStr("")) + "," + this.PrepareStr(row["f_OffDuty1"], true, "yyyy-MM-dd HH:mm:ss")) + "," + this.PrepareStr(row["f_OffDuty1Desc"])) + "," + this.PrepareStr("")) + "," + this.PrepareStr(row["f_OnDuty2"], true, "yyyy-MM-dd HH:mm:ss")) + "," + this.PrepareStr(row["f_OnDuty2Desc"])) + "," + this.PrepareStr("")) + "," + this.PrepareStr(row["f_OffDuty2"], true, "yyyy-MM-dd HH:mm:ss")) + "," + this.PrepareStr(row["f_OffDuty2Desc"])) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + this.PrepareStr("")) + "," + row["f_LateTime"]) + "," + row["f_LeaveEarlyTime"]) + "," + this.getDecimalStr(row["f_OvertimeTime"])) + "," + this.getDecimalStr(row["f_AbsenceDay"])) + "," + num2) + "," + this.PrepareStr("")) + ") ";
                        if (this.cn.State == ConnectionState.Closed)
                        {
                            this.cn.Open();
                        }
                        this.cmd.CommandText = cmdText;
                        if (this.cmd.ExecuteNonQuery() <= 0)
                        {
                            return num;
                        }
                    }
                }
                return num;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (this.cn != null)
                {
                    this.cn.Dispose();
                }
                if (this.cmd != null)
                {
                    this.cmd.Dispose();
                }
            }
            return num;
        }

        public int shiftAttStatisticImportFromAttStatistic()
        {
            int num = 0;
            string cmdText = "SELECT * FROM t_d_AttStatistic  ORDER BY f_RecID ";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        using (DataTable table = new DataTable("AttStatistic"))
                        {
                            adapter.Fill(table);
                            if (table.Rows.Count > 0)
                            {
                                command.Connection = connection;
                                command.CommandType = CommandType.Text;
                                for (int i = 0; i <= (table.Rows.Count - 1); i++)
                                {
                                    if (this.DealingNumEvent != null)
                                    {
                                        this.DealingNumEvent(i);
                                    }
                                    DataRow row = table.Rows[i];
                                    cmdText = " INSERT INTO t_d_Shift_AttStatistic ";
                                    cmdText = (((cmdText + " ( f_ConsumerID " + " , f_AttDateStart, f_AttDateEnd, f_DayShouldWork ") + " , f_DayRealWork" + " , f_LateMinutes,f_LateCount ") + " , f_LeaveEarlyMinutes,f_LeaveEarlyCount " + " , f_OvertimeHours ") + " , f_AbsenceDays " + " , f_NotReadCardCount, f_ManualReadTimesCount ";
                                    for (int j = 1; j <= 0x20; j++)
                                    {
                                        cmdText = cmdText + " , f_SpecialType" + j.ToString();
                                    }
                                    cmdText = (((((((((((((cmdText + " )") + " Values ( " + row["f_ConsumerID"]) + "," + this.PrepareStr(row["f_AttDateStart"], true, "yyyy-MM-dd HH:mm:ss")) + "," + this.PrepareStr(row["f_AttDateEnd"], true, "yyyy-MM-dd HH:mm:ss")) + "," + row["f_DayShouldWork"]) + "," + row["f_DayRealWork"]) + "," + row["f_LateMinutes"]) + "," + row["f_TotalLate"]) + "," + row["f_LeaveEarlyMinutes"]) + "," + row["f_TotalLeaveEarly"]) + "," + this.getDecimalStr(row["f_TotalOvertime"])) + "," + this.getDecimalStr(row["f_TotalAbsenceDay"])) + "," + row["f_TotalNotReadCard"]) + "," + row["f_ManualReadTimesCount"];
                                    for (int k = 1; k <= 0x20; k++)
                                    {
                                        cmdText = cmdText + " ," + this.PrepareStr(row["f_SpecialType" + k.ToString()]);
                                    }
                                    cmdText = cmdText + ") ";
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    command.CommandText = cmdText;
                                    if (command.ExecuteNonQuery() <= 0)
                                    {
                                        return num;
                                    }
                                }
                            }
                            return num;
                        }
                    }
                }
            }
        }

        public void startCreate()
        {
            this.mainThread = new Thread(new ThreadStart(this.make));
            this.mainThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            this.mainThread.Start();
        }

        public delegate void CreateCompleteEventHandler(bool bCreated, string strDesc);

        public delegate void DealingNumEventHandler(int num);
    }
}

