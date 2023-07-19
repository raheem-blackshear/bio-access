namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using WG3000_COMM.Core;

    public class icController : wgMjController
    {
        private SqlCommand cm;
        private OleDbCommand cm_Acc;
        private const int DoorMax = 4;
        public const int DUTY_OFF = 1;
        public const int DUTY_ON = 2;
        public const int DUTY_ONOFF = 3;
        private bool m_Active = true;
        private int m_ControllerID;
        private int m_ControllerNO;
        private bool[] m_doorActive = new bool[] { true, true, true, true };
        private int[] m_doorControl = new int[] { 3, 3, 3, 3 };
        private int[] m_doorDelay = new int[] { 3, 3, 3, 3 };
        private string[] m_doorName = new string[] { "", "", "", "" };
        private string[] m_floorName = new string[] { 
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "", "", "", "", "", "", "", ""
        };
        private string m_Note = "";
        private bool[] m_readerAsAttendActive = new bool[] { true, true, true, true };
        private int[] m_readerAsAttendControl = new int[] { 3, 3, 3, 3 };
        private string[] m_readerName = new string[] { "", "", "", "" };
        private int[] m_readerAuthenticationMode = new int[] {0, 0, 0, 0};
        private ControllerRunInformation m_runinfo = new ControllerRunInformation();
        private int m_ZoneID;
        private const int ReaderMax = 4;
        private bool[] rt_camera = new bool[] { false, false, false, false };
        private bool _touch_sensor = false;
        private bool _m1_card = false;
        private byte _volume = 5;
        private bool _fp_ident = false;
        private bool _face_ident = false;

        public int AddIntoDB()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.AddIntoDB_Acc();
            }
            int num = -9;
            try
            {
                string cmdText = "BEGIN TRANSACTION";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (this.cm = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        this.cm.ExecuteNonQuery();
                        try
                        {
                            int num2;
                            string str2 = "";
                            for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                            {
                                str2 = str2 + this.m_doorName[num2] + ";   ";
                            }
                            cmdText = "INSERT INTO t_b_Controller " +
                                "(f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_Note, f_DoorNames, f_ZoneID, " +
                                "f_ControllerWiFiSSID, f_ControllerWiFiKey, f_ControllerWiFiEnable, f_ControllerWiFiIP, " +
                                "f_ControllerWiFiMask, f_ControllerWiFiGateway, f_ControllerWiFiPort, f_RtCamera, f_TouchSensor, f_M1Card, f_Volume, f_FpIdent, f_FaceIdent) values (";
                            cmdText = cmdText + this.m_ControllerNO.ToString() + ", " + base.ControllerSN.ToString() + ", " +
                                (this.m_Active ? "1" : "0") + ", " + wgTools.PrepareStr(base.IP) + ", " +
                                base.PORT.ToString() + ", " + wgTools.PrepareStr(this.m_Note) + ", " +
                                wgTools.PrepareStr(str2) + ", " + this.m_ZoneID.ToString() + ", " +
                                wgTools.PrepareStr(base.ssid_wifi) + ", " +
                                wgTools.PrepareStr(base.key_wifi) + ", " +
                                (base.enable_wifi ? "1" : "0") + ", " +
                                wgTools.PrepareStr(base.ip_wifi) + ", " +
                                wgTools.PrepareStr(base.mask_wifi) + ", " +
                                wgTools.PrepareStr(base.gateway_wifi) + ", " +
                                base.port_wifi.ToString() + ", " +
                                getRtCamera().ToString() + ", " +
                                (touch_sensor ? "1" : "0") + ", " +
                                (m1_card ? "1" : "0") + ", " +
                                volume.ToString() + ", " +
                                (fp_ident ? "1" : "0") + ", " +
                                (face_ident ? "1" : "0")  + ")";
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                            cmdText = "SELECT f_ControllerID from [t_b_Controller] where f_ControllerNo =" + this.m_ControllerNO.ToString();
                            this.cm.CommandText = cmdText;
                            this.m_ControllerID = int.Parse("0" + wgTools.SetObjToStr(this.cm.ExecuteScalar()));
                            cmdText = " DELETE FROM [t_b_Door] ";
                            cmdText = cmdText + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                            for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                            {
                                cmdText = " DELETE FROM [t_b_Door] ";
                                cmdText = cmdText + " WHERE [f_DoorName] = " + wgTools.PrepareStr(this.m_doorName[num2]);
                                this.cm.CommandText = cmdText;
                                this.cm.ExecuteNonQuery();
                                cmdText = " INSERT INTO [t_b_Door] ";
                                cmdText = (((((((cmdText + "([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_doorName[num2])) + " , " + this.m_doorControl[num2].ToString()) + " , " + this.m_doorDelay[num2].ToString()) + " , " + (this.m_doorActive[num2] ? "1" : "0")) + ")";
                                this.cm.CommandText = cmdText;
                                this.cm.ExecuteNonQuery();
                            }
                            cmdText = " DELETE FROM [t_b_Reader] ";
                            cmdText = cmdText + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                            for (num2 = 0; num2 < wgMjController.GetControllerReaderNum(base.ControllerSN); num2++)
                            {
                                cmdText = " INSERT INTO [t_b_Reader] ";
                                cmdText = (((((((cmdText + "([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_AuthenticationMode], [f_Attend],[f_DutyOnOff])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_readerName[num2])) + " , 0") + " , " + (this.m_readerAsAttendActive[num2] ? "1" : "0")) + " , " + this.m_readerAsAttendControl[num2].ToString()) + ")";
                                this.cm.CommandText = cmdText;
                                this.cm.ExecuteNonQuery();
                                if (wgMjController.IsElevator(base.ControllerSN))
                                {
                                    break;
                                }
                            }
                            cmdText = "COMMIT TRANSACTION";
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public int AddIntoDB_Acc()
        {
            int num = -9;
            try
            {
                string cmdText = "BEGIN TRANSACTION";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (this.cm_Acc = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        this.cm_Acc.ExecuteNonQuery();
                        try
                        {
                            int num2;
                            string str2 = "";
                            for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                            {
                                str2 = str2 + this.m_doorName[num2] + ";   ";
                            }
                            cmdText = " INSERT INTO t_b_Controller " +
                                "(f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_Note, f_DoorNames, f_ZoneID, " +
                                "f_ControllerWiFiSSID, f_ControllerWiFiKey, f_ControllerWiFiEnable, f_ControllerWiFiIP, " +
                                "f_ControllerWiFiMask, f_ControllerWiFiGateway, f_ControllerWiFiPort, f_RtCamera, f_TouchSensor, f_M1Card, f_Volume, f_FpIdent, f_FaceIdent) values (";
                            cmdText = cmdText + this.m_ControllerNO.ToString() + ", " + base.ControllerSN.ToString() + ", " + 
                                (this.m_Active ? "1" : "0") + ", " + wgTools.PrepareStr(base.IP) + ", " + 
                                base.PORT.ToString() + ", " + wgTools.PrepareStr(this.m_Note) + ", " + 
                                wgTools.PrepareStr(str2) + ", " + this.m_ZoneID.ToString() + ", " +
                                wgTools.PrepareStr(base.ssid_wifi) + ", " +
                                wgTools.PrepareStr(base.key_wifi) + ", " +
                                (base.enable_wifi ? "1" : "0") + ", " +
                                wgTools.PrepareStr(base.ip_wifi) + ", " + 
                                wgTools.PrepareStr(base.mask_wifi) + ", " +
                                wgTools.PrepareStr(base.gateway_wifi) + ", " +
                                base.port_wifi.ToString() + ", " +
                                getRtCamera().ToString() + ", " +
                                (touch_sensor ? "1" : "0") + ", " +
                                (m1_card ? "1" : "0") + ", " +
                                volume.ToString() + ", " +
                                (fp_ident ? "1" : "0") + ", " +
                                (face_ident ? "1" : "0") + ")";
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                            cmdText = "SELECT f_ControllerID from [t_b_Controller] where f_ControllerNo =" + this.m_ControllerNO.ToString();
                            this.cm_Acc.CommandText = cmdText;
                            this.m_ControllerID = int.Parse("0" + wgTools.SetObjToStr(this.cm_Acc.ExecuteScalar()));
                            cmdText = " DELETE FROM [t_b_Door] ";
                            cmdText = cmdText + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                            for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                            {
                                cmdText = " DELETE FROM [t_b_Door] ";
                                cmdText = cmdText + " WHERE [f_DoorName] = " + wgTools.PrepareStr(this.m_doorName[num2]);
                                this.cm_Acc.CommandText = cmdText;
                                this.cm_Acc.ExecuteNonQuery();
                                cmdText = " INSERT INTO [t_b_Door] ";
                                cmdText = (((((((cmdText + "([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_doorName[num2])) + " , " + this.m_doorControl[num2].ToString()) + " , " + this.m_doorDelay[num2].ToString()) + " , " + (this.m_doorActive[num2] ? "1" : "0")) + ")";
                                this.cm_Acc.CommandText = cmdText;
                                this.cm_Acc.ExecuteNonQuery();
                            }
                            cmdText = " DELETE FROM [t_b_Reader] ";
                            cmdText = cmdText + " WHERE [f_ControllerID] = " + this.m_ControllerID.ToString();
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                            for (num2 = 0; num2 < wgMjController.GetControllerReaderNum(base.ControllerSN); num2++)
                            {
                                cmdText = " INSERT INTO [t_b_Reader] ";
                                cmdText = (((((((cmdText + "([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_AuthenticationMode], [f_Attend],[f_DutyOnOff])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_readerName[num2])) + " , 0") + " , " + (this.m_readerAsAttendActive[num2] ? "1" : "0")) + " , " + this.m_readerAsAttendControl[num2].ToString()) + ")";
                                this.cm_Acc.CommandText = cmdText;
                                this.cm_Acc.ExecuteNonQuery();
                                if (wgMjController.IsElevator(base.ControllerSN))
                                {
                                    break;
                                }
                            }
                            cmdText = "COMMIT TRANSACTION";
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public static int DeleteControllerFromDB(int ControllerID)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return DeleteControllerFromDB_Acc(ControllerID);
            }
            int num = ControllerID;
            if (num > 0)
            {
                string cmdText = "BEGIN TRANSACTION";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        try
                        {
                            cmdText = " DELETE FROM t_b_ElevatorGroup ";
                            cmdText = (cmdText + " WHERE ") + " t_b_ElevatorGroup.f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE  FROM t_b_UserFloor ";
                            cmdText = (cmdText + " WHERE f_floorID IN  ") + " (SELECT f_floorID FROM t_b_Floor WHERE t_b_Floor.f_ControllerID =  " + num.ToString() + ")";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM t_b_Floor ";
                            cmdText = cmdText + " WHERE t_b_Floor.f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "DELETE FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "DELETE  FROM t_d_Privilege WHERE f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            command.ExecuteNonQuery();
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception exception)
                        {
                            wgAppConfig.wgLog(exception.ToString());
                            cmdText = "ROLLBACK TRANSACTION";
                            if (command.Connection.State != ConnectionState.Open)
                            {
                                command.Connection.Open();
                            }
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            throw;
                        }
                    }
                }
            }
            return 1;
        }

        public static int DeleteControllerFromDB_Acc(int ControllerID)
        {
            int num = ControllerID;
            if (num > 0)
            {
                string cmdText = "BEGIN TRANSACTION";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        try
                        {
                            cmdText = " DELETE FROM t_b_ElevatorGroup ";
                            cmdText = (cmdText + " WHERE ") + " t_b_ElevatorGroup.f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE  FROM t_b_UserFloor ";
                            cmdText = (cmdText + " WHERE f_floorID IN  ") + " (SELECT f_floorID FROM t_b_Floor WHERE t_b_Floor.f_ControllerID =  " + num.ToString() + ")";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM t_b_Floor ";
                            cmdText = cmdText + " WHERE t_b_Floor.f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "DELETE FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "DELETE  FROM t_d_Privilege WHERE f_ControllerID =  " + num.ToString();
                            command.CommandText = cmdText;
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            command.ExecuteNonQuery();
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception exception)
                        {
                            wgAppConfig.wgLog(exception.ToString());
                            cmdText = "ROLLBACK TRANSACTION";
                            if (command.Connection.State != ConnectionState.Open)
                            {
                                command.Connection.Open();
                            }
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            throw;
                        }
                    }
                }
            }
            return 1;
        }

        public int DirectSetDoorControlIP(string DoorName, int doorControl)
        {
            int index = 0;
            while (index < 4)
            {
                if (this.m_doorName[index] == DoorName)
                {
                    break;
                }
                index++;
            }
            if (index >= 4)
            {
                return -1;
            }
            wgMjControllerConfigure mjconf = new wgMjControllerConfigure();
            mjconf.DoorControlSet(index + 1, doorControl);
            return base.UpdateConfigureIP(mjconf);
        }

        public int GetControllerRunInformationIP()
        {
            return this.GetControllerRunInformationIP("");
        }

        public int GetControllerRunInformationIP(string PCIPAddr)
        {
            byte[] recvInfo = null;
            if ((base.GetMjControllerRunInformationIP(ref recvInfo, PCIPAddr) != 1) || (recvInfo == null))
            {
                return -1;
            }
            if (base.ControllerSN != -1)
            {
                this.m_runinfo.update(recvInfo, 20, (uint) base.ControllerSN);
            }
            else
            {
                uint controllerSN = (uint) (((recvInfo[8] + (recvInfo[9] << 8)) + (recvInfo[10] << 0x10)) + (recvInfo[11] << 0x18));
                this.m_runinfo.update(recvInfo, 20, controllerSN);
            }
            for (int i = 0; i < 10; i++)
            {
                if (this.m_runinfo.newSwipes[i].IndexInDataFlash == uint.MaxValue)
                {
                    break;
                }
            }
            return 1;
        }

        public int GetControllerRunInformationIP_TCP(string strIP)
        {
            byte[] recvInfo = null;
            if ((base.GetMjControllerRunInformationIP_TCP(strIP, ref recvInfo) != 1) || (recvInfo == null))
            {
                return -1;
            }
            this.m_runinfo.update(recvInfo, 20, (uint) base.ControllerSN);
            for (int i = 0; i < 10; i++)
            {
                if (this.m_runinfo.newSwipes[i].IndexInDataFlash == uint.MaxValue)
                {
                    break;
                }
            }
            return 1;
        }

        public int GetControllerRunInformationIPNoTries()
        {
            byte[] recvInfo = null;
            if ((base.GetMjControllerRunInformationIPNoTries(ref recvInfo) != 1) || (recvInfo == null))
            {
                return -1;
            }
            if (base.ControllerSN != -1)
            {
                this.m_runinfo.update(recvInfo, 20, (uint) base.ControllerSN);
            }
            else
            {
                uint controllerSN = (uint) (((recvInfo[8] + (recvInfo[9] << 8)) + (recvInfo[10] << 0x10)) + (recvInfo[11] << 0x18));
                this.m_runinfo.update(recvInfo, 20, controllerSN);
            }
            for (int i = 0; i < 10; i++)
            {
                if (this.m_runinfo.newSwipes[i].IndexInDataFlash == uint.MaxValue)
                {
                    break;
                }
            }
            return 1;
        }

        public int GetControlTaskListIP(ref wgMjControllerTaskList controlTaskList)
        {
            try
            {
                byte[] controlTaskListData = null;
                if ((base.GetControlTaskListIP(ref controlTaskListData) == 1) && (controlTaskListData != null))
                {
                    controlTaskList = new wgMjControllerTaskList(controlTaskListData);
                    return 1;
                }
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public bool GetDoorActive(int doorNO)
        {
            return (((doorNO > 0) && (doorNO <= 4)) && this.m_doorActive[doorNO - 1]);
        }

        public int GetDoorControl(int doorNO)
        {
            if ((doorNO > 0) && (doorNO <= 4))
            {
                return this.m_doorControl[doorNO - 1];
            }
            return 0;
        }

        public int GetDoorDelay(int doorNO)
        {
            if ((doorNO > 0) && (doorNO <= 4))
            {
                return this.m_doorDelay[doorNO - 1];
            }
            return 0;
        }

        public string GetDoorName(int doorNO)
        {
            if ((doorNO > 0) && (doorNO <= 4))
            {
                return wgTools.SetObjToStr(this.m_doorName[doorNO - 1]);
            }
            return "";
        }

        public string GetDoorNameByReaderNO(int readerNO)
        {
            int num = 0;
            if (wgMjController.GetControllerType(base.ControllerSN) == 4)
            {
                num = readerNO;
            }
            else
            {
                num = (readerNO + 1) >> 1;
            }
            if ((num > 0) && (num <= 4))
            {
                return wgTools.SetObjToStr(this.m_doorName[num - 1]);
            }
            return "";
        }

        public int GetDoorNO(string doorName)
        {
            int index = 0;
            index = 0;
            while (index < 4)
            {
                if (this.m_doorName[index] == doorName)
                {
                    break;
                }
                index++;
            }
            if (index == 4)
            {
                return 1;
            }
            index++;
            return index;
        }

        public string GetFloorName(int floorNO)
        {
            if ((floorNO > 0) && (floorNO <= this.m_floorName.Length))
            {
                return wgTools.SetObjToStr(this.m_floorName[floorNO - 1]);
            }
            return "";
        }

        public int GetInfoFromDBByControllerID(int ControllerID)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.GetInfoFromDBByControllerID_Acc(ControllerID);
            }
            int num = ControllerID;
            if (num > 0)
            {
                this.m_ControllerID = num;
                string cmdText = " SELECT * ";
                cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.m_ControllerNO = (int) reader["f_ControllerNO"];
                            base.ControllerSN = (int) reader["f_ControllerSN"];
                            this.m_Active = int.Parse(reader["f_Enabled"].ToString()) > 0;
                            base.IP = wgTools.SetObjToStr(reader["f_IP"]);
                            base.PORT = (int) reader["f_PORT"];
                            this.m_Note = wgTools.SetObjToStr(reader["f_Note"]);
                            this.m_ZoneID = (int) reader["f_ZoneID"];
                            base.ssid_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiSSID"]);
                            base.key_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiKey"]);
                            base.enable_wifi = int.Parse(reader["f_ControllerWiFiEnable"].ToString()) > 0;
                            base.ip_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiIP"]);
                            base.mask_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiMask"]);
                            base.gateway_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiGateway"]);
                            base.port_wifi = (int)reader["f_ControllerWiFiPort"];
                            setRtCamera((int)reader["f_RtCamera"]);
                            touch_sensor = int.Parse(reader["f_TouchSensor"].ToString()) > 0;
                            m1_card = int.Parse(reader["f_M1Card"].ToString()) > 0;
                            volume = (byte)(int)reader["f_Volume"];
                            fp_ident = int.Parse(reader["f_FpIdent"].ToString()) > 0;
                            face_ident = int.Parse(reader["f_FaceIdent"].ToString()) > 0;
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int index = int.Parse(reader["f_DoorNO"].ToString()) - 1;
                            this.m_doorName[index] = (string) reader["f_DoorName"];
                            this.m_doorControl[index] = int.Parse(reader["f_DoorControl"].ToString());
                            this.m_doorDelay[index] = int.Parse(reader["f_DoorDelay"].ToString());
                            this.m_doorActive[index] = int.Parse(reader["f_DoorEnabled"].ToString()) > 0;
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Reader WHERE f_ControllerID =  " + num.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int num3 = int.Parse(reader["f_ReaderNo"].ToString()) - 1;
                            this.m_readerName[num3] = (string) reader["f_ReaderName"];
                            this.m_readerAuthenticationMode[num3] = int.Parse(reader["f_AuthenticationMode"].ToString());
                            this.m_readerAsAttendActive[num3] = int.Parse(reader["f_Attend"].ToString()) > 0;
                            this.m_readerAsAttendControl[num3] = int.Parse(reader["f_DutyOnOff"].ToString());
                        }
                        reader.Close();
                        cmdText = "  SELECT t_b_Reader.f_ReaderName, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
                        cmdText = ((cmdText + "   t_b_Door.f_DoorName, " + "   t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName, t_b_Door.f_ControllerID  ") + "    FROM t_b_Floor , t_b_Door, t_b_Controller, t_b_Reader " + "   where t_b_Floor.f_DoorID = t_b_Door.f_DoorID and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID and t_b_Reader.f_ControllerID = t_b_Floor.f_ControllerID ") + " and  t_b_Floor.f_ControllerID =  " + num.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int num4 = int.Parse(reader["f_floorNO"].ToString()) - 1;
                            this.m_floorName[num4] = (string) reader["f_floorFullName"];
                        }
                        reader.Close();
                    }
                }
            }
            return 1;
        }

        public int GetInfoFromDBByControllerID_Acc(int ControllerID)
        {
            int num = ControllerID;
            if (num > 0)
            {
                this.m_ControllerID = num;
                string cmdText = " SELECT * ";
                cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + num.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.m_ControllerNO = (int) reader["f_ControllerNO"];
                            base.ControllerSN = (int) reader["f_ControllerSN"];
                            this.m_Active = int.Parse(reader["f_Enabled"].ToString()) > 0;
                            base.IP = wgTools.SetObjToStr(reader["f_IP"]);
                            base.PORT = (int) reader["f_PORT"];
                            this.m_Note = wgTools.SetObjToStr(reader["f_Note"]);
                            this.m_ZoneID = (int) reader["f_ZoneID"];
                            base.ssid_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiSSID"]);
                            base.key_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiKey"]);
                            base.enable_wifi = int.Parse(reader["f_ControllerWiFiEnable"].ToString()) > 0;
                            base.ip_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiIP"]);
                            base.mask_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiMask"]);
                            base.gateway_wifi = wgTools.SetObjToStr(reader["f_ControllerWiFiGateway"]);
                            base.port_wifi = (int)reader["f_ControllerWiFiPort"];
                            setRtCamera((int)reader["f_RtCamera"]);
                            touch_sensor = int.Parse(reader["f_TouchSensor"].ToString()) > 0;
                            m1_card = int.Parse(reader["f_M1Card"].ToString()) > 0;
                            volume = (byte)(int)reader["f_Volume"];
                            fp_ident = int.Parse(reader["f_FpIdent"].ToString()) > 0;
                            face_ident = int.Parse(reader["f_FaceIdent"].ToString()) > 0;
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Door WHERE f_ControllerID =  " + num.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int index = int.Parse(reader["f_DoorNO"].ToString()) - 1;
                            this.m_doorName[index] = (string) reader["f_DoorName"];
                            this.m_doorControl[index] = int.Parse(reader["f_DoorControl"].ToString());
                            this.m_doorDelay[index] = int.Parse(reader["f_DoorDelay"].ToString());
                            this.m_doorActive[index] = int.Parse(reader["f_DoorEnabled"].ToString()) > 0;
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Reader WHERE f_ControllerID =  " + num.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int num3 = int.Parse(reader["f_ReaderNo"].ToString()) - 1;
                            this.m_readerName[num3] = (string) reader["f_ReaderName"];
                            this.m_readerAuthenticationMode[num3] = int.Parse(reader["f_AuthenticationMode"].ToString());
                            this.m_readerAsAttendActive[num3] = int.Parse(reader["f_Attend"].ToString()) > 0;
                            this.m_readerAsAttendControl[num3] = int.Parse(reader["f_DutyOnOff"].ToString());
                        }
                        reader.Close();
                        cmdText = "  SELECT t_b_Reader.f_ReaderName, t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
                        cmdText = ((cmdText + "   t_b_Door.f_DoorName, " + "   t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName, t_b_Door.f_ControllerID  ") + "    FROM t_b_Floor , t_b_Door, t_b_Controller, t_b_Reader " + "   where t_b_Floor.f_DoorID = t_b_Door.f_DoorID and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID and t_b_Reader.f_ControllerID = t_b_Floor.f_ControllerID ") + " and  t_b_Floor.f_ControllerID =  " + num.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int num4 = int.Parse(reader["f_floorNO"].ToString()) - 1;
                            this.m_floorName[num4] = (string)reader["f_floorFullName"];
                        }
                        reader.Close();
                    }
                }
            }
            return 1;
        }

        public int GetInfoFromDBByControllerSN(int ControllerSN)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.GetInfoFromDBByControllerSN_Acc(ControllerSN);
            }
            int num = ControllerSN;
            if (num > 0)
            {
                ControllerSN = num;
                string cmdText = " SELECT * ";
                cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerSN =  " + num.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.m_ControllerNO = (int) reader["f_ControllerNO"];
                            ControllerSN = (int) reader["f_ControllerSN"];
                            this.m_ControllerID = (int) reader["f_ControllerID"];
                            this.m_Active = int.Parse(reader["f_Enabled"].ToString()) > 0;
                            base.IP = wgTools.SetObjToStr(reader["f_IP"]);
                            base.PORT = (int) reader["f_PORT"];
                            this.m_Note = wgTools.SetObjToStr(reader["f_Note"]);
                            this.m_ZoneID = (int) reader["f_ZoneID"];
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int index = int.Parse(reader["f_DoorNO"].ToString()) - 1;
                            this.m_doorName[index] = (string) reader["f_DoorName"];
                            this.m_doorControl[index] = int.Parse(reader["f_DoorControl"].ToString());
                            this.m_doorDelay[index] = int.Parse(reader["f_DoorDelay"].ToString());
                            this.m_doorActive[index] = int.Parse(reader["f_DoorEnabled"].ToString()) > 0;
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int num3 = int.Parse(reader["f_ReaderNo"].ToString()) - 1;
                            this.m_readerName[num3] = (string) reader["f_ReaderName"];
                            this.m_readerAuthenticationMode[num3] = int.Parse(reader["f_AuthenticationMode"].ToString());
                            this.m_readerAsAttendActive[num3] = int.Parse(reader["f_Attend"].ToString()) > 0;
                            this.m_readerAsAttendControl[num3] = int.Parse(reader["f_DutyOnOff"].ToString());
                        }
                        reader.Close();
                    }
                }
            }
            return 1;
        }

        public int GetInfoFromDBByControllerSN_Acc(int ControllerSN)
        {
            int num = ControllerSN;
            if (num > 0)
            {
                ControllerSN = num;
                string cmdText = " SELECT * ";
                cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerSN =  " + num.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.m_ControllerNO = (int) reader["f_ControllerNO"];
                            ControllerSN = (int) reader["f_ControllerSN"];
                            this.m_ControllerID = (int) reader["f_ControllerID"];
                            this.m_Active = int.Parse(reader["f_Enabled"].ToString()) > 0;
                            base.IP = wgTools.SetObjToStr(reader["f_IP"]);
                            base.PORT = (int) reader["f_PORT"];
                            this.m_Note = wgTools.SetObjToStr(reader["f_Note"]);
                            this.m_ZoneID = (int) reader["f_ZoneID"];
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int index = int.Parse(reader["f_DoorNO"].ToString()) - 1;
                            this.m_doorName[index] = (string) reader["f_DoorName"];
                            this.m_doorControl[index] = int.Parse(reader["f_DoorControl"].ToString());
                            this.m_doorDelay[index] = int.Parse(reader["f_DoorDelay"].ToString());
                            this.m_doorActive[index] = int.Parse(reader["f_DoorEnabled"].ToString()) > 0;
                        }
                        reader.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int num3 = int.Parse(reader["f_ReaderNo"].ToString()) - 1;
                            this.m_readerName[num3] = (string) reader["f_ReaderName"];
                            this.m_readerAuthenticationMode[num3] = int.Parse(reader["f_AuthenticationMode"].ToString());
                            this.m_readerAsAttendActive[num3] = int.Parse(reader["f_Attend"].ToString()) > 0;
                            this.m_readerAsAttendControl[num3] = int.Parse(reader["f_DutyOnOff"].ToString());
                        }
                        reader.Close();
                    }
                }
            }
            return 1;
        }

        public int GetInfoFromDBByDoorName(string DoorName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.GetInfoFromDBByDoorName_Acc(DoorName);
            }
            int controllerID = 0;
            string cmdText = " SELECT f_ControllerID ";
            cmdText = cmdText + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStr(DoorName);
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    controllerID = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                }
            }
            if (controllerID > 0)
            {
                this.GetInfoFromDBByControllerID(controllerID);
            }
            return 1;
        }

        public int GetInfoFromDBByDoorName_Acc(string DoorName)
        {
            int controllerID = 0;
            string cmdText = " SELECT f_ControllerID ";
            cmdText = cmdText + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStr(DoorName);
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    controllerID = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                }
            }
            if (controllerID > 0)
            {
                this.GetInfoFromDBByControllerID(controllerID);
            }
            return 1;
        }

        public static int GetMaxControllerNO()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return GetMaxControllerNO_Acc();
            }
            try
            {
                string cmdText = "SELECT MAX(f_ControllerNO) from t_b_Controller";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        return int.Parse(command.ExecuteScalar().ToString());
                    }
                }
            }
            catch
            {
            }
            return 0;
        }

        public static int GetMaxControllerNO_Acc()
        {
            try
            {
                string cmdText = "select max(CLNG(0 & [f_ControllerNO])) from t_b_Controller";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        return int.Parse(command.ExecuteScalar().ToString());
                    }
                }
            }
            catch
            {
            }
            return 0;
        }

        public bool GetReaderAsAttendActive(int readerNO)
        {
            return (((readerNO > 0) && (readerNO <= 4)) && this.m_readerAsAttendActive[readerNO - 1]);
        }

        public int GetReaderAsAttendControl(int readerNO)
        {
            if ((readerNO > 0) && (readerNO <= 4))
            {
                return this.m_readerAsAttendControl[readerNO - 1];
            }
            return 0;
        }

        public string GetReaderName(int readerNO)
        {
            if ((readerNO > 0) && (readerNO <= 4))
            {
                return wgTools.SetObjToStr(this.m_readerName[readerNO - 1]);
            }
            return "";
        }

        public static bool IsExisted2NO(int ControllerNO, int ControllerIDExclude)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return IsExisted2NO_Acc(ControllerNO, ControllerIDExclude);
            }
            bool flag = false;
            try
            {
                string cmdText = string.Format("SELECT count(*) from [t_b_Controller] WHERE   [f_ControllerID]<> {0:d} AND [f_ControllerNO] ={1:d} ", ControllerIDExclude, ControllerNO);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse(command.ExecuteScalar().ToString()) > 0)
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
            }
            catch
            {
            }
            return flag;
        }

        public static bool IsExisted2NO_Acc(int ControllerNO, int ControllerIDExclude)
        {
            bool flag = false;
            try
            {
                string cmdText = string.Format("SELECT count(*) from [t_b_Controller] WHERE   [f_ControllerID]<> {0:d} AND [f_ControllerNO] ={1:d} ", ControllerIDExclude, ControllerNO);
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse(command.ExecuteScalar().ToString()) > 0)
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
            }
            catch
            {
            }
            return flag;
        }

        public static bool IsExisted2SN(int SN, int ControllerIDExclude)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return IsExisted2SN_Acc(SN, ControllerIDExclude);
            }
            bool flag = false;
            try
            {
                string cmdText = string.Format("SELECT count(*) from [t_b_Controller] WHERE [f_ControllerID]<> {0:d} AND [f_ControllerSN] ={1:d} ", ControllerIDExclude, SN);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse(command.ExecuteScalar().ToString()) > 0)
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
            }
            catch
            {
            }
            return flag;
        }

        public static bool IsExisted2SN_Acc(int SN, int ControllerIDExclude)
        {
            bool flag = false;
            try
            {
                string cmdText = string.Format("SELECT count(*) from [t_b_Controller] WHERE [f_ControllerID]<> {0:d} AND [f_ControllerSN] ={1:d} ", ControllerIDExclude, SN);
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse(command.ExecuteScalar().ToString()) > 0)
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
            }
            catch
            {
            }
            return flag;
        }

        public int RemoteOpenDoorIP(string DoorName)
        {
            int index = 0;
            while (index < 4)
            {
                if (this.m_doorName[index] == DoorName)
                {
                    break;
                }
                index++;
            }
            if (index >= 4)
            {
                return -1;
            }
            return base.RemoteOpenDoorIP(index + 1, (uint) icOperator.OperatorID, ulong.MaxValue);
        }

        public int RemoteOpenDoorIP(string DoorName, uint operatorId, ulong operatorCardNO)
        {
            int index = 0;
            while (index < 4)
            {
                if (this.m_doorName[index] == DoorName)
                {
                    break;
                }
                index++;
            }
            if (index >= 4)
            {
                return -1;
            }
            return base.RemoteOpenDoorIP(index + 1, operatorId, operatorCardNO);
        }

        public void SetDoorActive(int doorNO, bool active)
        {
            if ((doorNO > 0) && (doorNO <= 4))
            {
                this.m_doorActive[doorNO - 1] = active;
            }
        }

        public void SetDoorControl(int doorNO, int doorControl)
        {
            if (((doorNO > 0) && (doorNO <= 4)) && ((doorControl >= 0) && (doorControl <= 3)))
            {
                this.m_doorControl[doorNO - 1] = doorControl;
            }
        }

        public void SetDoorDelay(int doorNO, int doorDelay)
        {
            if ((doorNO > 0) && (doorNO <= 4))
            {
                this.m_doorDelay[doorNO - 1] = doorDelay;
            }
        }

        public void SetDoorName(int doorNO, string doorName)
        {
            if ((doorNO > 0) && (doorNO <= 4))
            {
                this.m_doorName[doorNO - 1] = doorName;
            }
        }

        public void SetReaderAsAttendActive(int readerNO, bool active)
        {
            if ((readerNO > 0) && (readerNO <= 4))
            {
                this.m_readerAsAttendActive[readerNO - 1] = active;
            }
        }

        public void SetReaderAsAttendControl(int readerNO, int AttendControl)
        {
            if (((readerNO > 0) && (readerNO <= 4)) && ((AttendControl >= 0) && (AttendControl <= 3)))
            {
                this.m_readerAsAttendControl[readerNO - 1] = AttendControl;
            }
        }

        public void SetReaderName(int readerNO, string readerName)
        {
            if ((readerNO > 0) && (readerNO <= 4))
            {
                this.m_readerName[readerNO - 1] = readerName;
            }
        }

        public static string StrDelFirstSame(string mainInfo, string deletedInfo)
        {
            if ((!string.IsNullOrEmpty(mainInfo) && !string.IsNullOrEmpty(deletedInfo)) && (mainInfo.IndexOf(deletedInfo) == 0))
            {
                return mainInfo.Substring(deletedInfo.Length);
            }
            return mainInfo;
        }

        public static string StrReplaceFirstSame(string mainInfo, string oldInfo, string newInfo)
        {
            if (((!string.IsNullOrEmpty(mainInfo) && !string.IsNullOrEmpty(oldInfo)) && !string.IsNullOrEmpty(newInfo)) && (mainInfo.IndexOf(oldInfo) == 0))
            {
                return (newInfo + mainInfo.Substring(oldInfo.Length));
            }
            return mainInfo;
        }

        public int UpdateControlTaskListIP(wgMjControllerTaskList controlTaskList)
        {
            return base.UpdateControlTaskListIP(controlTaskList.ToByte());
        }

        public int UpdateControlTimeSegListIP(icControllerTimeSegList controlTimeSegList)
        {
            return base.UpdateControlTimeSegListIP(controlTimeSegList.ToByte());
        }

        public int UpdateIntoDB(bool ControllerTypeChanged)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.UpdateIntoDB_Acc(ControllerTypeChanged);
            }
            int num = -9;
            try
            {
                string cmdText = "BEGIN TRANSACTION";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (this.cm = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        this.cm.ExecuteNonQuery();
                        try
                        {
                            int num2;
                            string str2 = "";
                            for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                            {
                                str2 = str2 + this.m_doorName[num2] + ";   ";
                            }
                            cmdText = "UPDATE t_b_Controller ";
                            cmdText = (cmdText + string.Format(" SET  f_ControllerNO={0}, " +
                                "f_ControllerSN={1}, " +
                                "f_Enabled={2}, " +
                                "f_IP={3}, " +
                                "f_PORT={4}, " +
                                "f_Note={5}, " +
                                "f_DoorNames={6}, " +
                                "f_ZoneID={7}, " +
                                "f_ControllerWiFiSSID={8}, " +
                                "f_ControllerWiFiKey={9}, " +
                                "f_ControllerWiFiEnable={10}, " +
                                "f_ControllerWiFiIP={11}, " +
                                "f_ControllerWiFiMask={12}, " +
                                "f_ControllerWiFiGateway={13}, " +
                                "f_ControllerWiFiPort={14}, " +
                                "f_RtCamera={15}, " +
                                "f_TouchSensor={16}, " +
                                "f_M1Card={17}, " +
                                "f_Volume={18}, " +
                                "f_FpIdent={19}, " +
                                "f_FaceIdent={20}",
                                new object[] { this.m_ControllerNO.ToString(),
                                    base.ControllerSN.ToString(), 
                                    this.m_Active ? "1" : "0", 
                                    wgTools.PrepareStr(base.IP), 
                                    base.PORT.ToString(),
                                    wgTools.PrepareStr(this.m_Note),
                                    wgTools.PrepareStr(str2),
                                    this.m_ZoneID.ToString(),
                                    wgTools.PrepareStr(base.ssid_wifi),
                                    wgTools.PrepareStr(base.key_wifi),
                                    (base.enable_wifi ? "1" : "0"),
                                    wgTools.PrepareStr(base.ip_wifi),
                                    wgTools.PrepareStr(base.mask_wifi),
                                    wgTools.PrepareStr(base.gateway_wifi),
                                    base.port_wifi.ToString(),
                                    getRtCamera().ToString(),
                                    (touch_sensor ? "1" : "0"),
                                    (m1_card ? "1" : "0"),
                                    volume.ToString(),
                                    (fp_ident ? "1" : "0"),
                                    (face_ident ? "1" : "0")
                                })) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                            if (ControllerTypeChanged)
                            {
                                cmdText = "DELETE FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                                this.cm.CommandText = cmdText;
                                this.cm.ExecuteNonQuery();
                                cmdText = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                                this.cm.CommandText = cmdText;
                                this.cm.ExecuteNonQuery();
                                for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                                {
                                    cmdText = " INSERT INTO [t_b_Door] ";
                                    cmdText = (((((((cmdText + "([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_doorName[num2])) + " , " + this.m_doorControl[num2].ToString()) + " , " + this.m_doorDelay[num2].ToString()) + " , " + (this.m_doorActive[num2] ? "1" : "0")) + ")";
                                    this.cm.CommandText = cmdText;
                                    this.cm.ExecuteNonQuery();
                                }
                                for (num2 = 0; num2 < wgMjController.GetControllerReaderNum(base.ControllerSN); num2++)
                                {
                                    cmdText = " INSERT INTO [t_b_Reader] ";
                                    cmdText = (((((((cmdText + "([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_AuthenticationMode], [f_Attend],[f_DutyOnOff])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_readerName[num2])) + " , 0") + " , " + (this.m_readerAsAttendActive[num2] ? "1" : "0")) + " , " + this.m_readerAsAttendControl[num2].ToString()) + ")";
                                    this.cm.CommandText = cmdText;
                                    this.cm.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                num2 = 0;
                                while (num2 < wgMjController.GetControllerType(base.ControllerSN))
                                {
                                    cmdText = " UPDATE [t_b_Door] SET ";
                                    cmdText = ((cmdText + string.Format(" [f_DoorName]={0}, [f_DoorControl]={1}, [f_DoorDelay]={2}, [f_DoorEnabled]={3} ", new object[] { wgTools.PrepareStr(this.m_doorName[num2]), this.m_doorControl[num2].ToString(), this.m_doorDelay[num2].ToString(), this.m_doorActive[num2] ? "1" : "0" })) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString()) + "  AND [f_DoorNO]=" + ((num2 + 1)).ToString();
                                    this.cm.CommandText = cmdText;
                                    this.cm.ExecuteNonQuery();
                                    num2++;
                                }
                                for (num2 = 0; num2 < wgMjController.GetControllerReaderNum(base.ControllerSN); num2++)
                                {
                                    cmdText = " UPDATE [t_b_Reader] SET ";
                                    cmdText = ((cmdText + string.Format(" [f_ReaderName]={0}, [f_Attend]={1},[f_DutyOnOff]={2}", wgTools.PrepareStr(this.m_readerName[num2]), this.m_readerAsAttendActive[num2] ? "1" : "0", this.m_readerAsAttendControl[num2].ToString())) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString()) + "  AND [f_ReaderNo]=" + ((num2 + 1)).ToString();
                                    this.cm.CommandText = cmdText;
                                    this.cm.ExecuteNonQuery();
                                }
                            }
                            cmdText = "COMMIT TRANSACTION";
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            this.cm.CommandText = cmdText;
                            this.cm.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public int UpdateIntoDB_Acc(bool ControllerTypeChanged)
        {
            int num = -9;
            try
            {
                string cmdText = "BEGIN TRANSACTION";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (this.cm_Acc = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        this.cm_Acc.ExecuteNonQuery();
                        try
                        {
                            int num2;
                            string str2 = "";
                            for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                            {
                                str2 = str2 + this.m_doorName[num2] + ";   ";
                            }
                            cmdText = "UPDATE t_b_Controller ";
                            cmdText = (cmdText + string.Format(" SET  f_ControllerNO={0}, " +
                                "f_ControllerSN={1}, " +
                                "f_Enabled={2}, " +
                                "f_IP={3}, " +
                                "f_PORT={4}, " +
                                "f_Note={5}, " +
                                "f_DoorNames={6}, " +
                                "f_ZoneID={7}, " +
                                "f_ControllerWiFiSSID={8}, " +
                                "f_ControllerWiFiKey={9}, " +
                                "f_ControllerWiFiEnable={10}, " +
                                "f_ControllerWiFiIP={11}, " +
                                "f_ControllerWiFiMask={12}, " +
                                "f_ControllerWiFiGateway={13}, " +
                                "f_ControllerWiFiPort={14}, " +
                                "f_RtCamera={15}, " +
                                "f_TouchSensor={16}, " + 
                                "f_M1Card={17}, " +
                                "f_Volume={18}, " +
                                "f_FpIdent={19}, " +
                                "f_FaceIdent={20}",
                                new object[] { this.m_ControllerNO.ToString(),
                                    base.ControllerSN.ToString(), 
                                    this.m_Active ? "1" : "0", 
                                    wgTools.PrepareStr(base.IP), 
                                    base.PORT.ToString(),
                                    wgTools.PrepareStr(this.m_Note),
                                    wgTools.PrepareStr(str2),
                                    this.m_ZoneID.ToString(),
                                    wgTools.PrepareStr(base.ssid_wifi),
                                    wgTools.PrepareStr(base.key_wifi),
                                    (base.enable_wifi ? "1" : "0"),
                                    wgTools.PrepareStr(base.ip_wifi),
                                    wgTools.PrepareStr(base.mask_wifi),
                                    wgTools.PrepareStr(base.gateway_wifi),
                                    base.port_wifi.ToString(),
                                    getRtCamera().ToString(),
                                    (touch_sensor ? "1" : "0"),
                                    (m1_card ? "1" : "0"),
                                    volume.ToString(),
                                    (fp_ident ? "1" : "0"),
                                    (face_ident ? "1" : "0")
                                })) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString();

                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                            if (ControllerTypeChanged)
                            {
                                cmdText = "DELETE FROM t_b_Reader WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                                this.cm_Acc.CommandText = cmdText;
                                this.cm_Acc.ExecuteNonQuery();
                                cmdText = "DELETE  FROM t_b_Door WHERE f_ControllerID =  " + this.m_ControllerID.ToString();
                                this.cm_Acc.CommandText = cmdText;
                                this.cm_Acc.ExecuteNonQuery();
                                for (num2 = 0; num2 < wgMjController.GetControllerType(base.ControllerSN); num2++)
                                {
                                    cmdText = " INSERT INTO [t_b_Door] ";
                                    cmdText = (((((((cmdText + "([f_ControllerID], [f_DoorNO], [f_DoorName], [f_DoorControl], [f_DoorDelay], [f_DoorEnabled])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_doorName[num2])) + " , " + this.m_doorControl[num2].ToString()) + " , " + this.m_doorDelay[num2].ToString()) + " , " + (this.m_doorActive[num2] ? "1" : "0")) + ")";
                                    this.cm_Acc.CommandText = cmdText;
                                    this.cm_Acc.ExecuteNonQuery();
                                }
                                for (num2 = 0; num2 < wgMjController.GetControllerReaderNum(base.ControllerSN); num2++)
                                {
                                    cmdText = " INSERT INTO [t_b_Reader] ";
                                    cmdText = (((((((cmdText + "([f_ControllerID], [f_ReaderNo], [f_ReaderName], [f_AuthenticationMode], [f_Attend],[f_DutyOnOff])") + " Values(" + this.m_ControllerID.ToString()) + " , " + ((num2 + 1)).ToString()) + " , " + wgTools.PrepareStr(this.m_readerName[num2])) + " , 0") + " , " + (this.m_readerAsAttendActive[num2] ? "1" : "0")) + " , " + this.m_readerAsAttendControl[num2].ToString()) + ")";
                                    this.cm_Acc.CommandText = cmdText;
                                    this.cm_Acc.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                num2 = 0;
                                while (num2 < wgMjController.GetControllerType(base.ControllerSN))
                                {
                                    cmdText = " UPDATE [t_b_Door] SET ";
                                    cmdText = ((cmdText + string.Format(" [f_DoorName]={0}, [f_DoorControl]={1}, [f_DoorDelay]={2}, [f_DoorEnabled]={3} ", new object[] { wgTools.PrepareStr(this.m_doorName[num2]), this.m_doorControl[num2].ToString(), this.m_doorDelay[num2].ToString(), this.m_doorActive[num2] ? "1" : "0" })) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString()) + "  AND [f_DoorNO]=" + ((num2 + 1)).ToString();
                                    this.cm_Acc.CommandText = cmdText;
                                    this.cm_Acc.ExecuteNonQuery();
                                    num2++;
                                }
                                for (num2 = 0; num2 < wgMjController.GetControllerReaderNum(base.ControllerSN); num2++)
                                {
                                    cmdText = " UPDATE [t_b_Reader] SET ";
                                    cmdText = ((cmdText + string.Format(" [f_ReaderName]={0}, [f_Attend]={1},[f_DutyOnOff]={2}", wgTools.PrepareStr(this.m_readerName[num2]), this.m_readerAsAttendActive[num2] ? "1" : "0", this.m_readerAsAttendControl[num2].ToString())) + "  WHERE [f_ControllerID]=" + this.m_ControllerID.ToString()) + "  AND [f_ReaderNo]=" + ((num2 + 1)).ToString();
                                    this.cm_Acc.CommandText = cmdText;
                                    this.cm_Acc.ExecuteNonQuery();
                                }
                            }
                            cmdText = "COMMIT TRANSACTION";
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            this.cm_Acc.CommandText = cmdText;
                            this.cm_Acc.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public bool Active
        {
            get
            {
                return this.m_Active;
            }
            set
            {
                this.m_Active = value;
            }
        }

        public int ControllerID
        {
            get
            {
                return this.m_ControllerID;
            }
            set
            {
                if (this.m_ControllerID >= 0)
                {
                    this.m_ControllerID = value;
                }
            }
        }

        public int ControllerNO
        {
            get
            {
                return this.m_ControllerNO;
            }
            set
            {
                if (this.m_ControllerNO >= 0)
                {
                    this.m_ControllerNO = value;
                }
            }
        }

        public string Note
        {
            get
            {
                return this.m_Note;
            }
            set
            {
                this.m_Note = value;
            }
        }

        public ControllerRunInformation runinfo
        {
            get
            {
                return this.m_runinfo;
            }
        }

        public int ZoneID
        {
            get
            {
                return this.m_ZoneID;
            }
            set
            {
                this.m_ZoneID = value;
            }
        }

        private int getRtCamera()
        {
            int stat = 0;
            for (int door = 0; door < DoorMax; door++)
                if (rt_camera[door])
                    stat += (1 << door);
            return stat;
        }

        private void setRtCamera(int stat)
        {
            for (int door = 0; door < DoorMax; door++)
            {
                rt_camera[door] = ((stat & 1) != 0);
                stat >>= 1;
            }
        }

        public void setCameraBit(int door, bool camera)
        {
            rt_camera[door - 1] = camera;
        }

        public bool getCameraBit(int door)
        {
            return rt_camera[door - 1];
        }

        public bool touch_sensor
        {
            get { return _touch_sensor; }
            set { _touch_sensor = value; }
        }

        public bool m1_card
        {
            get { return _m1_card; }
            set { _m1_card = value; }
        }

        public byte volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public bool fp_ident
        {
            get { return _fp_ident; }
            set { _fp_ident = value; }
        }

        public bool face_ident
        {
            get { return _face_ident; }
            set { _face_ident = value; }
        }
    }
}

