namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using WG3000_COMM.Core;

    public class icControllerTimeSegList : wgMjControllerTimeSegList
    {
        public icControllerTimeSegList()
        {
            base.Clear();
        }

        public void fillByDB()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.fillByDB_Acc();
            }
            else
            {
                base.Clear();
                string cmdText = " SELECT * FROM t_b_ControlSeg  ";
                bool flag = wgAppConfig.getParamValBoolByNO(0x88);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (((int) reader["f_ControlSegID"]) <= 0xfe)
                            {
                                MjControlTimeSeg mjControlTimeSeg = new MjControlTimeSeg();
                                mjControlTimeSeg.SegIndex = (byte) ((int) reader["f_ControlSegID"]);
                                mjControlTimeSeg.weekdayControl = 0;
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Monday"].ToString() == "1") ? ((byte) 1) : ((byte) 0)));
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Tuesday"].ToString() == "1") ? ((byte) 2) : ((byte) 0)));
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Wednesday"].ToString() == "1") ? ((byte) 4) : ((byte) 0)));
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Thursday"].ToString() == "1") ? ((byte) 8) : ((byte) 0)));
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Friday"].ToString() == "1") ? ((byte) 0x10) : ((byte) 0)));
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Saturday"].ToString() == "1") ? ((byte) 0x20) : ((byte) 0)));
                                mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Sunday"].ToString() == "1") ? ((byte) 0x40) : ((byte) 0)));
                                mjControlTimeSeg.hmsStart1 = DateTime.Parse(reader["f_BeginHMS1"].ToString());
                                mjControlTimeSeg.hmsStart2 = DateTime.Parse(reader["f_BeginHMS2"].ToString());
                                mjControlTimeSeg.hmsStart3 = DateTime.Parse(reader["f_BeginHMS3"].ToString());
                                mjControlTimeSeg.hmsEnd1 = DateTime.Parse(reader["f_EndHMS1"].ToString());
                                mjControlTimeSeg.hmsEnd2 = DateTime.Parse(reader["f_EndHMS2"].ToString());
                                mjControlTimeSeg.hmsEnd3 = DateTime.Parse(reader["f_EndHMS3"].ToString());
                                mjControlTimeSeg.ymdStart = DateTime.Parse(reader["f_BeginYMD"].ToString());
                                mjControlTimeSeg.ymdEnd = DateTime.Parse(reader["f_EndYMD"].ToString());
                                if (flag)
                                {
                                    mjControlTimeSeg.LimittedMode = int.Parse(reader["f_ReaderCount"].ToString());
                                    mjControlTimeSeg.TotalLimittedAccess = ((int) reader["f_LimitedTimesOfDay"]) & 0xff;
                                    mjControlTimeSeg.MonthLimittedAccess = (((int) reader["f_LimitedTimesOfDay"]) >> 8) & 0xff;
                                    mjControlTimeSeg.LimittedAccess1 = (int) reader["f_LimitedTimesOfHMS1"];
                                    mjControlTimeSeg.LimittedAccess2 = (int) reader["f_LimitedTimesOfHMS2"];
                                    mjControlTimeSeg.LimittedAccess3 = (int) reader["f_LimitedTimesOfHMS3"];
                                }
                                mjControlTimeSeg.nextSeg = (byte) ((int) reader["f_ControlSegIDLinked"]);
                                mjControlTimeSeg.ControlByHoliday = (byte) ((int) reader["f_ControlByHoliday"]);
                                if (base.AddItem(mjControlTimeSeg) != 1)
                                {
                                    break;
                                }
                            }
                        }
                        reader.Close();
                    }
                }
            }
        }

        public void fillByDB_Acc()
        {
            base.Clear();
            string cmdText = " SELECT * FROM t_b_ControlSeg  ";
            bool flag = wgAppConfig.getParamValBoolByNO(0x88);
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (((int) reader["f_ControlSegID"]) <= 0xfe)
                        {
                            MjControlTimeSeg mjControlTimeSeg = new MjControlTimeSeg();
                            mjControlTimeSeg.SegIndex = (byte) ((int) reader["f_ControlSegID"]);
                            mjControlTimeSeg.weekdayControl = 0;
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Monday"].ToString() == "1") ? ((byte) 1) : ((byte) 0)));
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Tuesday"].ToString() == "1") ? ((byte) 2) : ((byte) 0)));
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Wednesday"].ToString() == "1") ? ((byte) 4) : ((byte) 0)));
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Thursday"].ToString() == "1") ? ((byte) 8) : ((byte) 0)));
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Friday"].ToString() == "1") ? ((byte) 0x10) : ((byte) 0)));
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Saturday"].ToString() == "1") ? ((byte) 0x20) : ((byte) 0)));
                            mjControlTimeSeg.weekdayControl = (byte) (mjControlTimeSeg.weekdayControl + ((reader["f_Sunday"].ToString() == "1") ? ((byte) 0x40) : ((byte) 0)));
                            mjControlTimeSeg.hmsStart1 = DateTime.Parse(reader["f_BeginHMS1"].ToString());
                            mjControlTimeSeg.hmsStart2 = DateTime.Parse(reader["f_BeginHMS2"].ToString());
                            mjControlTimeSeg.hmsStart3 = DateTime.Parse(reader["f_BeginHMS3"].ToString());
                            mjControlTimeSeg.hmsEnd1 = DateTime.Parse(reader["f_EndHMS1"].ToString());
                            mjControlTimeSeg.hmsEnd2 = DateTime.Parse(reader["f_EndHMS2"].ToString());
                            mjControlTimeSeg.hmsEnd3 = DateTime.Parse(reader["f_EndHMS3"].ToString());
                            mjControlTimeSeg.ymdStart = DateTime.Parse(reader["f_BeginYMD"].ToString());
                            mjControlTimeSeg.ymdEnd = DateTime.Parse(reader["f_EndYMD"].ToString());
                            if (flag)
                            {
                                mjControlTimeSeg.LimittedMode = int.Parse(reader["f_ReaderCount"].ToString());
                                mjControlTimeSeg.TotalLimittedAccess = ((int) reader["f_LimitedTimesOfDay"]) & 0xff;
                                mjControlTimeSeg.MonthLimittedAccess = (((int) reader["f_LimitedTimesOfDay"]) >> 8) & 0xff;
                                mjControlTimeSeg.LimittedAccess1 = (int) reader["f_LimitedTimesOfHMS1"];
                                mjControlTimeSeg.LimittedAccess2 = (int) reader["f_LimitedTimesOfHMS2"];
                                mjControlTimeSeg.LimittedAccess3 = (int) reader["f_LimitedTimesOfHMS3"];
                            }
                            mjControlTimeSeg.nextSeg = (byte) ((int) reader["f_ControlSegIDLinked"]);
                            mjControlTimeSeg.ControlByHoliday = (byte) ((int) reader["f_ControlByHoliday"]);
                            if (base.AddItem(mjControlTimeSeg) != 1)
                            {
                                break;
                            }
                        }
                    }
                    reader.Close();
                }
            }
        }
    }
}

