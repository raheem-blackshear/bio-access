namespace WG3000_COMM.Core
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Threading;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ResStrings;

    public class MjRec : wgMjControllerSwipeRecord
    {
        private static icController control4GetDetailedRecord = new icController();
        private string m_Address;
        private DateTime m_beginYMD;
        private string m_consumerName;
        private string m_deptName;
        private DateTime m_endYMD;
        public const int RecordSizeInDb = 0x30;
        private int rec_id = 0;

        private byte[] photoData = null;
        private const uint PhotoMaxSize = 0x2000;//8K
        private const uint ChunkSize = 0x400;
        

        public MjRec()
        {
            this.m_beginYMD = DateTime.Parse("2000-1-1");
            this.m_endYMD = DateTime.Parse("2000-1-1");
        }

        public MjRec(string strRecordAll, bool loadPhoto) : base(strRecordAll)
        {
            this.m_beginYMD = DateTime.Parse("2000-1-1");
            this.m_endYMD = DateTime.Parse("2000-1-1");
        }

        public MjRec(byte[] rec, uint startIndex, bool loadPhoto) : base(rec, startIndex)
        {
            this.m_beginYMD = DateTime.Parse("2000-1-1");
            this.m_endYMD = DateTime.Parse("2000-1-1");
        }

        public MjRec(byte[] rec, uint startIndex, uint ControllerSN, uint loc, bool loadPhoto) 
            : base(rec, startIndex, ControllerSN, loc)
        {
            this.m_beginYMD = DateTime.Parse("2000-1-1");
            this.m_endYMD = DateTime.Parse("2000-1-1");
        }

        public string GetDetailedRecord(icController current_control, uint RecControllerSN)
        {
            string strRecordWarnFire = "";
            if ((base.eventCategory == 1) || (base.eventCategory == 0))
            {
                switch (base.SwipeStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSwipe);
                        goto Label_0477;

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSwipeOpen);
                        goto Label_0477;

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSwipeClose);
                        goto Label_0477;

                    case 0x84:
                    case 0x85:
                    case 0x86:
                    case 0x87:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessPCControl);
                        goto Label_0477;

                    case 0x90:
                    case 0x91:
                    case 0x92:
                    case 0x93:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessNOPRIVILEGE);
                        goto Label_0477;

                    case 160:
                    case 0xa1:
                    case 0xa2:
                    case 0xa3:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessERRPASSWORD);
                        goto Label_0477;

                    case 0xc4:
                    case 0xc5:
                    case 0xc6:
                    case 0xc7:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_ANTIBACK);
                        goto Label_0477;

                    case 200:
                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_MORECARD);
                        goto Label_0477;

                    case 0xcc:
                    case 0xcd:
                    case 0xce:
                    case 0xcf:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_FIRSTCARD);
                        goto Label_0477;

                    case 0xd0:
                    case 0xd1:
                    case 210:
                    case 0xd3:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessDOORNC);
                        goto Label_0477;

                    case 0xd4:
                    case 0xd5:
                    case 0xd6:
                    case 0xd7:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_INTERLOCK);
                        goto Label_0477;

                    case 0xd8:
                    case 0xd9:
                    case 0xda:
                    case 0xdb:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_LIMITEDTIMES);
                        goto Label_0477;

                    case 220:
                    case 0xdd:
                    case 0xde:
                    case 0xdf:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR);
                        goto Label_0477;

                    case 0xe0:
                    case 0xe1:
                    case 0xe2:
                    case 0xe3:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessINVALIDTIMEZONE);
                        goto Label_0477;

                    case 0xe4:
                    case 0xe5:
                    case 230:
                    case 0xe7:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_INORDER);
                        goto Label_0477;

                    case 0xe8:
                    case 0xe9:
                    case 0xea:
                    case 0xeb:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_SWIPEGAPLIMIT);
                        goto Label_0477;

                    case 0xec:
                    case 0xed:
                    case 0xee:
                    case 0xef:
                        strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessVM);
                        goto Label_0477;
                }
                strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccess);
            }
        Label_0477:
            if ((base.eventCategory == 3) || (base.eventCategory == 2))
            {
                if (base.IsPassed)
                {
                    if (wgMjController.IsElevator((int) base.ControllerSN))
                    {
                        if (base.currentSwipeTimes >= 0x80)
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSwipeWithCount4MultiFloor1);
                        }
                        else
                        {
                            string floorName = control4GetDetailedRecord.GetFloorName(base.floorNo);
                            if (!string.IsNullOrEmpty(floorName))
                            {
                                floorName = " [" + floorName + "]";
                            }
                            strRecordWarnFire = string.Format("{0}{1}{2}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSwipeWithCount4Floor, base.currentSwipeTimes) + floorName;
                        }
                    }
                    else
                    {
                        strRecordWarnFire = string.Format("{0}{1}{2}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSwipeWithCount, base.currentSwipeTimes);
                    }
                }
                else
                {
                    strRecordWarnFire = string.Format("{0}{1}{2}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, base.currentSwipeTimes);
                }
            }
            if ((base.eventCategory == 4) || (base.eventCategory == 5))
            {
                switch (base.UserID)
                {
                    case 0:
                        if (base.SwipeStatus != 0)
                        {
                            if ((base.SwipeStatus & 130) == 130)
                            {
                                strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordPowerOn);
                            }
                            else if ((base.SwipeStatus & 160) == 160)
                            {
                                strRecordWarnFire = string.Format("{0}{1}-{2}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordReset, "LDO");
                            }
                            else if ((base.SwipeStatus & 0x90) == 0x90)
                            {
                                strRecordWarnFire = string.Format("{0}{1}-{2}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordReset, "SW");
                            }
                            else if ((base.SwipeStatus & 0x88) == 0x88)
                            {
                                strRecordWarnFire = string.Format("{0}{1}-{2}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordReset, "WDT");
                            }
                            else if ((base.SwipeStatus & 0x84) == 0x84)
                            {
                                strRecordWarnFire = string.Format("{0}{1}-{2}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordReset, "BOR");
                            }
                            else if ((base.SwipeStatus & 0x81) == 0x81)
                            {
                                strRecordWarnFire = string.Format("{0}{1}-{2}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordReset, "EXT");
                            }
                        }
                        else
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordPowerOn);
                        }
                        if (!string.IsNullOrEmpty(strRecordWarnFire) && (RecControllerSN > 0))
                        {
                            strRecordWarnFire = string.Format("{0}[{1}]", strRecordWarnFire, RecControllerSN.ToString());
                        }
                        break;

                    case 1:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordPushButton);
                        }
                        break;

                    case 2:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordPushButtonOpen);
                        }
                        break;

                    case 3:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.SwipeStatus + 1), CommonStr.strRecordPushButtonClose);
                        }
                        break;

                    case 4:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName((base.SwipeStatus & 3) + 1), CommonStr.strRecordPushButtonInvalid_Disable);
                        }
                        break;

                    case 5:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName((base.SwipeStatus & 3) + 1), CommonStr.strRecordPushButtonInvalid_ForcedLock);
                        }
                        break;

                    case 6:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName((base.SwipeStatus & 3) + 1), CommonStr.strRecordPushButtonInvalid_NotOnLine);
                        }
                        break;

                    case 7:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName((base.SwipeStatus & 3) + 1), CommonStr.strRecordPushButtonInvalid_INTERLOCK);
                        }
                        break;

                    case 8:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordDoorOpen);
                        }
                        break;

                    case 9:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordDoorClosed);
                        }
                        break;

                    case 10:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSuperPasswordDoorOpen);
                        }
                        break;

                    case 11:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSuperPasswordOpen);
                        }
                        break;

                    case 12:
                        if ((base.SwipeStatus >= 0) && (base.SwipeStatus <= 3))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetReaderName(base.ReaderNo), CommonStr.strRecordSuperPasswordClose);
                        }
                        break;

                    case 0x51:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordThreat);
                        }
                        break;

                    case 0x52:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordThreatOpen);
                        }
                        break;

                    case 0x53:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordThreatClose);
                        }
                        break;

                    case 0x54:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordWarnLeftOpen);
                        }
                        break;

                    case 0x55:
                        if ((base.SwipeStatus >= 0x80) && (base.SwipeStatus <= 0x83))
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordWarnOpenByForce);
                        }
                        break;

                    case 0x56:
                        if (base.SwipeStatus == 0x80)
                        {
                            strRecordWarnFire = CommonStr.strRecordWarnFire;
                            if (current_control != null)
                            {
                                strRecordWarnFire = control4GetDetailedRecord.GetDoorName(1);
                                if (wgMjController.GetControllerType((int) base.ControllerSN) == 2)
                                {
                                    strRecordWarnFire = strRecordWarnFire + "," + control4GetDetailedRecord.GetDoorName(2);
                                }
                                if (wgMjController.GetControllerType((int) base.ControllerSN) == 4)
                                {
                                    strRecordWarnFire = strRecordWarnFire + string.Format(",{0},{1},{2}", control4GetDetailedRecord.GetDoorName(2), control4GetDetailedRecord.GetDoorName(3), control4GetDetailedRecord.GetDoorName(4));
                                }
                                strRecordWarnFire = strRecordWarnFire + CommonStr.strRecordWarnFire;
                            }
                        }
                        break;

                    case 0x57:
                        if (base.SwipeStatus == 0x80)
                        {
                            strRecordWarnFire = CommonStr.strRecordWarnCloseByForce;
                            if (current_control != null)
                            {
                                strRecordWarnFire = control4GetDetailedRecord.GetDoorName(1);
                                if (wgMjController.GetControllerType((int) base.ControllerSN) == 2)
                                {
                                    strRecordWarnFire = strRecordWarnFire + "," + control4GetDetailedRecord.GetDoorName(2);
                                }
                                if (wgMjController.GetControllerType((int) base.ControllerSN) == 4)
                                {
                                    strRecordWarnFire = strRecordWarnFire + string.Format(",{0},{1},{2}", control4GetDetailedRecord.GetDoorName(2), control4GetDetailedRecord.GetDoorName(3), control4GetDetailedRecord.GetDoorName(4));
                                }
                                strRecordWarnFire = strRecordWarnFire + CommonStr.strRecordWarnCloseByForce;
                            }
                        }
                        break;

                    case 0x58:
                        if (base.SwipeStatus == 0x80)
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(1), CommonStr.strRecordWarnGuardAgainstTheft);
                        }
                        break;

                    case 0x59:
                        if (base.SwipeStatus == 0x80)
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(1), CommonStr.strRecordWarn24Hour);
                        }
                        break;

                    case 90:
                        if (base.SwipeStatus == 0x80)
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(1), CommonStr.strRecordWarnEmergencyCall);
                        }
                        break;
                    case 0x61:
                        if (base.SwipeStatus == 0x80)
                        {
                            strRecordWarnFire = string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(1), CommonStr.strRecordWarnTamper);
                        }
                        break;
                }
            }
            if (base.eventCategory == 6)
            {
                switch (base.SwipeStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        return string.Format("{0}{1}{2}", control4GetDetailedRecord.GetDoorName(base.DoorNo), 
                            CommonStr.strRecordRemoteOpenDoor_ByUSBReader,
                            base.UserID.ToString());

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                        return string.Format("{0}{1}", control4GetDetailedRecord.GetDoorName(base.DoorNo), CommonStr.strRecordRemoteOpenDoor);
                }
            }
            return strRecordWarnFire;
        }

        public void GetUserInfoFromDB()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.GetUserInfoFromDB_Acc();
            }
            else
            {
                string cmdText = " SELECT  f_ConsumerName,  f_GroupName, f_BeginYMD, f_EndYMD ";
                cmdText = (cmdText + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ") +
                    " WHERE  t_b_Consumer.f_ConsumerNO = '" + base.UserID.ToString() + "'";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.m_consumerName = reader["f_ConsumerName"] as string;
                            this.m_deptName = reader["f_GroupName"] as string;
                            DateTime.TryParse(reader["f_BeginYMD"].ToString(), out this.m_beginYMD);
                            DateTime.TryParse(reader["f_EndYMD"].ToString(), out this.m_endYMD);
                        }
                        else
                        {
                            this.m_consumerName = "";
                            this.m_deptName = "";
                        }
                        reader.Close();
                    }
                }
            }
        }

        public void GetUserInfoFromDB_Acc()
        {
            string cmdText = " SELECT  f_ConsumerName,  f_GroupName, f_BeginYMD, f_EndYMD ";
            cmdText = (cmdText + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ") + 
                " WHERE  t_b_Consumer.f_ConsumerNO = '" + base.UserID.ToString() + "'";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        this.m_consumerName = reader["f_ConsumerName"] as string;
                        this.m_deptName = reader["f_GroupName"] as string;
                        DateTime.TryParse(reader["f_BeginYMD"].ToString(), out this.m_beginYMD);
                        DateTime.TryParse(reader["f_EndYMD"].ToString(), out this.m_endYMD);
                    }
                    else
                    {
                        this.m_consumerName = "";
                        this.m_deptName = "";
                    }
                    reader.Close();
                }
            }
        }

        public string ToDisplayDetail()
        {
            string str = "";
            if (base.IsSwipeRecord)
            {
                str = str + string.Format("{0}: \t{1:d}\r\n", CommonStr.strUserID, base.UserID);
                if (this.m_consumerName == null)
                {
                    this.GetUserInfoFromDB();
                }
                str = str + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName) + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartment), this.m_deptName);
            }
            else if (base.IsRemoteOpen)
            {
                if (base.SwipeStatus < 4)
                {
                    str = str + string.Format("{0}: \t{1:d}\r\n", CommonStr.strUserID, base.UserID);
                    if (this.m_consumerName == null)
                    {
                        this.GetUserInfoFromDB();
                    }
                    str = str + string.Format("{0}: \t{1}\r\n", CommonStr.strName, this.m_consumerName) + string.Format("{0}: \t{1}\r\n", wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartment), this.m_deptName);
                }
                else if ((base.SwipeStatus < 20) && (base.SwipeStatus >= 0x10))
                {
                    str = str + string.Format("{0}: \t{1:d}\r\n", CommonStr.strUserID, base.UserID);
                }
            }
            return ((str + string.Format("{0}: \t{1}\r\n", CommonStr.strReadDate, base.ReadDate.ToString(wgTools.DisplayFormat_DateYMDHMSWeek))) + 
                string.Format("{0}: \t{1}\r\n", CommonStr.strAddr, string.IsNullOrEmpty(this.m_Address) ? base.ControllerSN.ToString() : this.m_Address) + 
                string.Format("{0}: \t{1}\r\n", CommonStr.strVerifMode, getVerifModeString()) +
                string.Format("{0}: \t{1}\r\n", CommonStr.strSwipeStatus, this.GetDetailedRecord(null, base.ControllerSN)));
        }

        public string ToDisplayInfo()
        {
            string str = "";
            if (base.IsSwipeRecord)
            {
                str = str + string.Format("{0:d}-", base.UserID);
                if (this.m_consumerName == null)
                {
                    this.GetUserInfoFromDB();
                }
                str = str + string.Format("{0}-", this.m_consumerName) + string.Format("{0}-", this.m_deptName);
            }
            return ((str + string.Format("{0}-", base.ReadDate.ToString(wgTools.DisplayFormat_DateYMDHMSWeek))) + 
                string.Format("{0}-", string.IsNullOrEmpty(this.m_Address) ? base.ControllerSN.ToString() : this.m_Address) + 
                string.Format("{0}-", getVerifModeString()) +
                string.Format("{0}", this.GetDetailedRecord(null, base.ControllerSN)));
        }

        public bool loadPhotoFromDB(int rec_id)
        {
            DataTable table = new DataTable();
            DataView view = new DataView(table);
            
            if (rec_id == 0)
                return false;

            string str = "SELECT f_Photo FROM t_d_SwipeRecord WHERE f_RecID = " + rec_id.ToString();
            try
            {
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(str, connection))
                        {
                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                            {
                                adapter.Fill(table);
                            }
                        }
                        goto l_fill;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(str, connection2))
                    {
                        using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                        {
                            adapter2.Fill(table);
                        }
                    }
                }
            l_fill:
                if (view.Count == 0)
                    return false;

                photoData = (byte[])view[0]["f_Photo"];
            }
            catch (Exception e)
            {
                wgTools.WgDebugWrite(e.ToString(), new object[0]);
            }
            return true;
        }

        public bool loadPhotoFromDevice(string ip, int port)
        {
            if (PhotoID == uint.MaxValue || !IsSwipeRecord)
                return false;

            try
            {
                wgUdpComm wgudp = new wgUdpComm();
                Thread.Sleep(300);

                // fetch photo data
                byte[] recv = null;

                WGPacketNameQuery query = new WGPacketNameQuery(0x20, 0xe0, ControllerSN, PhotoID);
                if (wgudp.udp_get(query.ToBytes(wgudp.udpPort), 300,
                        query.xid, ip, port, ref recv) < 0 || recv == null ||
                    !query.get(recv, 0x14) ||
                    query.tag == 0)
                    return false;

                uint photoSize = query.tag;
                photoData = new byte[photoSize];
                query = new WGPacketNameQuery(0x20, 0xf0, ControllerSN, 0);
                WGPacketWith1024 packet = new WGPacketWith1024();
                for (uint i = 0; i < (photoSize + ChunkSize - 1) / ChunkSize; i++)
                {
                    query.tag = i;
                    query.GetNewXid();
                    if (wgudp.udp_get(query.ToBytes(wgudp.udpPort), 300,
                            query.xid, ip, port, ref recv) < 0 || recv == null ||
                        !packet.get(recv, 0x14))
                        return false;

                    Array.Copy(packet.ucData, 0, photoData, i * ChunkSize,
                        photoSize - i * ChunkSize >= ChunkSize ? ChunkSize : photoSize - i * ChunkSize);
                }
            }
            catch (Exception e)
            {
                wgTools.WgDebugWrite(e.ToString(), new object[0]);
            }
            return true;
        }

        public bool loadPhotoFromDevice()
        {
            if (PhotoID == uint.MaxValue || !IsSwipeRecord)
                return false;

            DataTable table = new DataTable();
            DataView view = new DataView(table);

            string str = "SELECT f_IP, f_PORT FROM t_b_Controller WHERE f_ControllerSN = " + ControllerSN.ToString();
            try
            {
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(str, connection))
                        {
                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                            {
                                adapter.Fill(table);
                            }
                        }
                        goto l_fill;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(str, connection2))
                    {
                        using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                        {
                            adapter2.Fill(table);
                        }
                    }
                }
            l_fill:
                if (view.Count == 0)
                    return false;

                string ip = wgTools.SetObjToStr(view[0]["f_IP"]);
                int port = (int)view[0]["f_Port"];

                loadPhotoFromDevice(ip, port);
            }
            catch (Exception e)
            {
                wgTools.WgDebugWrite(e.ToString(), new object[0]);
            }
            return true;
        }

        public string getVerifModeString()
        {
            string modeStr = "";

            switch ((VerifMode)base.verifMode)
            {
                case VerifMode.VerifFinger:
                    modeStr = CommonStr.strVerifFinger; break;
                case VerifMode.VerifCard:
                    modeStr = CommonStr.strVerifCard; break;
                case VerifMode.VerifPassword:
                    modeStr = CommonStr.strVerifPassword; break;
                case VerifMode.VerifFace:
                    modeStr = CommonStr.strVerifFace; break;
                default:
                    modeStr = CommonStr.strVerifNone; break;
            }
            return modeStr;
        }

        public string address
        {
            get
            {
                return this.m_Address;
            }
            set
            {
                this.m_Address = value;
            }
        }

        public DateTime beginYMD
        {
            get
            {
                return this.m_beginYMD;
            }
        }

        public string consumerName
        {
            get
            {
                return this.m_consumerName;
            }
        }

        public DateTime endYMD
        {
            get
            {
                return this.m_endYMD;
            }
        }

        public string groupname
        {
            get
            {
                return this.m_deptName;
            }
        }

        public byte[] PhotoData
        {
            get
            {
                return photoData;
            }
        }

        public int RecID
        {
            get { return rec_id; }
            set { rec_id = value; }
        }

        public enum VerifMode
        {
            VerifNone,
            VerifFinger,
            VerifCard,
            VerifPassword,
            VerifFace,
        }
    }
}
