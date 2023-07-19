namespace WG3000_COMM.DataOper
{
    using System;
    using System.Collections;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Net;
    using WG3000_COMM.Core;

    internal class icControllerConfigureFromDB //TODOJ Controller configure - very important
    {
        public static int getControllerConfigureFromDBByControllerID(int ControllerID, ref wgMjControllerConfigure controlConfigure, ref wgMjControllerTaskList controlTaskList, ref wgMjControllerHolidaysList controlHolidayList)
        {
            DbConnection connection;
            DbCommand command;
            if (wgAppConfig.IsAccessDB)
            {
                connection = new OleDbConnection(wgAppConfig.dbConString);
                command = new OleDbCommand("", connection as OleDbConnection);
            }
            else
            {
                connection = new SqlConnection(wgAppConfig.dbConString);
                command = new SqlCommand("", connection as SqlConnection);
            }
            int num = ControllerID;
            if (num <= 0)
            {
                return -1;
            }
            int controllerSN = 0;
            int num3 = 0;
            connection.Open();
            string str = " SELECT *, f_ZoneNO  FROM t_b_Controller LEFT JOIN t_b_Controller_Zone ON t_b_Controller_Zone.f_ZoneID = t_b_Controller.f_ZoneID WHERE f_ControllerID =  " + num.ToString();
            command.CommandText = str;
            command.CommandText = str;
            DbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                controllerSN = (int) reader["f_ControllerSN"];
                if (!string.IsNullOrEmpty(wgTools.SetObjToStr(reader["f_ZoneNO"])))
                {
                    num3 = int.Parse(wgTools.SetObjToStr(reader["f_ZoneNO"]));
                }
            }
            reader.Close();
            str = " SELECT * from t_b_door where [f_controllerID]= " + ControllerID.ToString() + " order by [f_DoorNO] ASC";
            command.CommandText = str;
            reader = command.ExecuteReader();
            int doorNO = 0;
            while (reader.Read())
            {
                doorNO++;
                controlConfigure.DoorControlSet(doorNO, (int) reader["f_DoorControl"]);
                controlConfigure.DoorDelaySet(doorNO, (int) reader["f_DoorDelay"]);
                if (wgAppConfig.getParamValBoolByNO(0x86))
                {
                    controlConfigure.MorecardNeedCardsSet(doorNO, (int) reader["f_MoreCards_Total"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 1, (int) reader["f_MoreCards_Grp1"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 2, (int) reader["f_MoreCards_Grp2"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 3, (int) reader["f_MoreCards_Grp3"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 4, (int) reader["f_MoreCards_Grp4"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 5, (int) reader["f_MoreCards_Grp5"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 6, (int) reader["f_MoreCards_Grp6"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 7, (int) reader["f_MoreCards_Grp7"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 8, (int) reader["f_MoreCards_Grp8"]);
                }
                else
                {
                    controlConfigure.MorecardNeedCardsSet(doorNO, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 1, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 2, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 3, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 4, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 5, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 6, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 7, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 8, 0);
                }
                if ((((((int) reader["f_MoreCards_Grp1"]) > 0) || (((int) reader["f_MoreCards_Grp2"]) > 0)) || ((((int) reader["f_MoreCards_Grp3"]) > 0) || (((int) reader["f_MoreCards_Grp4"]) > 0))) || (((((int) reader["f_MoreCards_Grp5"]) > 0) || (((int) reader["f_MoreCards_Grp6"]) > 0)) || ((((int) reader["f_MoreCards_Grp7"]) > 0) || (((int) reader["f_MoreCards_Grp8"]) > 0))))
                {
                    controlConfigure.MorecardSequenceInputSet(doorNO, (((int) reader["f_MoreCards_Option"]) & 0x10) > 0);
                }
                else
                {
                    controlConfigure.MorecardSequenceInputSet(doorNO, false);
                }
                controlConfigure.MorecardSingleGroupEnableSet(doorNO, (((int) reader["f_MoreCards_Option"]) & 8) > 0);
                controlConfigure.MorecardSingleGroupStartNOSet(doorNO, (((int) reader["f_MoreCards_Option"]) & 7) + 1);
                controlConfigure.DoorDisableTimesegMinSet(doorNO, 0);
            }
            reader.Close();
            str = " SELECT * from t_b_reader where [f_controllerID]= " + ControllerID.ToString() + " order by [f_ReaderNO] ASC";
            command.CommandText = str;
            reader = command.ExecuteReader();
            int readerNO = 0;
            bool flag = wgAppConfig.getParamValBoolByNO(0x7b);
            while (reader.Read())
            {
                readerNO++;
                if (flag)
                {
                    controlConfigure.ReaderAuthModeSet(readerNO, (int)reader["f_AuthenticationMode"]);
                    controlConfigure.InputCardNOOpenSet(readerNO, (int) reader["f_InputCardno_Enabled"]);
                }
                else
                {
                    controlConfigure.ReaderAuthModeSet(readerNO, 0);
                    controlConfigure.InputCardNOOpenSet(readerNO, 0);
                }
            }
            reader.Close();
            int num6 = 0;
            if (wgAppConfig.getParamValBoolByNO(0x92))
            {
                string eName = "";
                string str3 = "";
                string notes = "";
                wgAppConfig.getSystemParamValue(0x92, out eName, out str3, out notes);
                if (!string.IsNullOrEmpty(wgTools.SetObjToStr(notes)))
                {
                    str = string.Format(" SELECT * from t_b_door where [f_controllerID]= {0} AND [f_DoorID] IN ({1}) order by [f_DoorNO] ASC", ControllerID.ToString(), notes);
                    command.CommandText = str;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        num6 |= ((int) 1) << (int.Parse(wgTools.SetObjToStr(reader["f_DoorNO"])) - 1);
                    }
                    reader.Close();
                }
            }
            controlConfigure.lockSwitchOption = num6;
            controlConfigure.swipeGap = int.Parse("0" + wgAppConfig.getSystemParamByNO(0x93));
            str = " SELECT * from t_b_Controller where [f_controllerID]= " + ControllerID;
            command.CommandText = str;
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                controlConfigure.DoorInterlockSet(1, 0);
                controlConfigure.DoorInterlockSet(2, 0);
                controlConfigure.DoorInterlockSet(3, 0);
                controlConfigure.DoorInterlockSet(4, 0);
                if (wgAppConfig.getParamValBoolByNO(0x85))
                {
                    switch (((int) reader["f_InterLock"]))
                    {
                        case 1:
                            controlConfigure.DoorInterlockSet(1, 0x31);
                            controlConfigure.DoorInterlockSet(2, 0x32);
                            break;

                        case 2:
                            controlConfigure.DoorInterlockSet(3, 0xc4);
                            controlConfigure.DoorInterlockSet(4, 0xc8);
                            break;

                        case 3:
                            controlConfigure.DoorInterlockSet(1, 0x31);
                            controlConfigure.DoorInterlockSet(2, 0x32);
                            controlConfigure.DoorInterlockSet(3, 0xc4);
                            controlConfigure.DoorInterlockSet(4, 0xc8);
                            break;

                        case 4:
                            controlConfigure.DoorInterlockSet(1, 0x71);
                            controlConfigure.DoorInterlockSet(2, 0x72);
                            controlConfigure.DoorInterlockSet(3, 0x74);
                            break;

                        case 8:
                            controlConfigure.DoorInterlockSet(1, 0xf1);
                            controlConfigure.DoorInterlockSet(2, 0xf2);
                            controlConfigure.DoorInterlockSet(3, 0xf4);
                            controlConfigure.DoorInterlockSet(4, 0xf8);
                            break;
                    }
                }
                if (wgAppConfig.getParamValBoolByNO(0x84))
                {
                    controlConfigure.antiback = ((int) reader["f_AntiBack"]) % 10;
                    controlConfigure.indoorPersonsMax = (((int) reader["f_AntiBack"]) - controlConfigure.antiback) / 10;
                }
                else
                {
                    controlConfigure.antiback = 0;
                    controlConfigure.indoorPersonsMax = 0;
                }
                controlConfigure.moreCardRead4Reader = (int) reader["f_MoreCards_GoInOut"];
                int num8 = int.Parse(wgAppConfig.getSystemParamByNO(40));
                controlConfigure.doorOpenTimeout = num8;
                string[] strArray = wgTools.SetObjToStr(reader["f_PeripheralControl"]).Split(new char[] { ',' });
                if (!wgAppConfig.getParamValBoolByNO(0x7c) || (strArray.Length != 0x1b))
                {
                    strArray = "126,30,30,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,10,10,0,0,0,0".Split(new char[] { ',' });
                }
                int[] numArray = new int[4];
                int[] numArray2 = new int[4];
                int[] numArray3 = new int[4];
                int[] numArray4 = new int[4];
                decimal[] numArray5 = new decimal[4];
                int[] numArray6 = new int[4];
                int index = 0;
                int num9 = int.Parse(strArray[index++]);
                int num10 = int.Parse(strArray[index++]);
                int num11 = int.Parse(strArray[index++]);
                numArray[0] = int.Parse(strArray[index++]);
                numArray[1] = int.Parse(strArray[index++]);
                numArray[2] = int.Parse(strArray[index++]);
                numArray[3] = int.Parse(strArray[index++]);
                numArray2[0] = int.Parse(strArray[index++]);
                numArray2[1] = int.Parse(strArray[index++]);
                numArray2[2] = int.Parse(strArray[index++]);
                numArray2[3] = int.Parse(strArray[index++]);
                numArray3[0] = int.Parse(strArray[index++]);
                numArray3[1] = int.Parse(strArray[index++]);
                numArray3[2] = int.Parse(strArray[index++]);
                numArray3[3] = int.Parse(strArray[index++]);
                numArray4[0] = int.Parse(strArray[index++]);
                numArray4[1] = int.Parse(strArray[index++]);
                numArray4[2] = int.Parse(strArray[index++]);
                numArray4[3] = int.Parse(strArray[index++]);
                numArray5[0] = decimal.Parse(strArray[index++]);
                numArray5[1] = decimal.Parse(strArray[index++]);
                numArray5[2] = decimal.Parse(strArray[index++]);
                numArray5[3] = decimal.Parse(strArray[index++]);
                numArray6[0] = int.Parse(strArray[index++]);
                numArray6[1] = int.Parse(strArray[index++]);
                numArray6[2] = int.Parse(strArray[index++]);
                numArray6[3] = int.Parse(strArray[index++]);
                controlConfigure.ext_AlarmControlMode = num9;
                controlConfigure.ext_SetAlarmOnDelay = num10;
                controlConfigure.ext_SetAlarmOffDelay = num11;
                int alarm = int.Parse(wgAppConfig.getSystemParamByNO(0x7c));
                for (index = 0; index < 4; index++)
                {
                    if (alarm > 0)
                    {
                        controlConfigure.Ext_doorSet(index, numArray[index]);
                        controlConfigure.Ext_controlSet(index, numArray2[index]);
                        controlConfigure.Ext_warnSignalEnabledSet(index, numArray3[index]);
                        controlConfigure.Ext_warnSignalEnabled2Set(index, numArray4[index]);
                        controlConfigure.Ext_timeoutSet(index, (int) numArray5[index]);
                    }
                    else
                    {
                        controlConfigure.Ext_doorSet(index, 0);
                        controlConfigure.Ext_controlSet(index, 0);
                        controlConfigure.Ext_warnSignalEnabledSet(index, 0);
                        controlConfigure.Ext_warnSignalEnabled2Set(index, 0);
                        controlConfigure.Ext_timeoutSet(index, 0);
                    }
                }
                int num13 = 0;
                num13 += (((int) reader["f_ForceWarn"]) > 0) ? 1 : 0;
                num13 += (((int) reader["f_DoorOpenTooLong"]) > 0) ? 2 : 0;
                num13 += (((int) reader["f_DoorInvalidOpen"]) > 0) ? 4 : 0;
                num13 += (((int) reader["f_DoorTamperWarn"]) > 0) ? 8 : 0;
                num13 += (((int) reader["f_InvalidCardWarn"]) > 0) ? 0x10 : 0;
                num13 += 0x20;
                if ((((numArray[0] == 0x10) && (numArray6[0] > 0)) || ((numArray[1] == 0x10) && (numArray6[1] > 0))) || (((numArray[2] == 0x10) && (numArray6[2] > 0)) || ((numArray[3] == 0x10) && (numArray6[3] > 0))))
                {
                    num13 += 0x40;
                }
                else
                {
                    controlConfigure.ext_Alarm_Status = 0;
                }
                if (wgAppConfig.getParamValBoolByNO(0x8d))//ActivateWarnForceWithCard
                {
                    num13 += 0x80;
                }
                if (!wgAppConfig.getParamValBoolByNO(0x7c))//ActivatePeripheralControl
                {
                    num13 = 0;
                }
                controlConfigure.warnSetup = num13;
                controlConfigure.xpPassword = int.Parse(wgAppConfig.getSystemParamByNO(0x18));
                if (!wgAppConfig.getParamValBoolByNO(0x7c) || !wgAppConfig.getParamValBoolByNO(60))
                {
                    controlConfigure.fire_broadcast_receive = 0;
                    controlConfigure.fire_broadcast_send = 0;
                }
                else
                {
                    controlConfigure.fire_broadcast_receive = 15;
                    controlConfigure.fire_broadcast_send = 1;
                    if ((wgAppConfig.getSystemParamByNO(60) == "2") && ((num3 > 0) & (num3 < 0xfd)))
                    {
                        controlConfigure.fire_broadcast_send = num3 + 1;
                    }
                }
                if ((!wgAppConfig.getParamValBoolByNO(0x85) || !wgAppConfig.getParamValBoolByNO(0x3d)) || (wgMjController.GetControllerType(controllerSN) == 1))
                {
                    controlConfigure.interlock_broadcast_receive = 0;
                    controlConfigure.interlock_broadcast_send = 0;
                }
                else
                {
                    controlConfigure.interlock_broadcast_receive = 5;
                    controlConfigure.interlock_broadcast_send = 1;
                    if ((wgAppConfig.getSystemParamByNO(0x3d) == "2") && ((num3 > 0) & (num3 < 0xfd)))
                    {
                        controlConfigure.interlock_broadcast_send = num3 + 1;
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(0x84) || !wgAppConfig.getParamValBoolByNO(0x3e))
                {
                    controlConfigure.antiback_broadcast_send = 0;
                    if (controlConfigure.indoorPersonsMax > 0)
                    {
                        controlConfigure.antiback_broadcast_send = 0xfe;
                    }
                }
                else
                {
                    controlConfigure.antiback_broadcast_send = 1;
                    if ((wgAppConfig.getSystemParamByNO(0x3e) == "2") && ((num3 > 0) & (num3 < 0xfd)))
                    {
                        controlConfigure.antiback_broadcast_send = num3 + 1;
                    }
                }
                controlConfigure.receventWarn = (num13 > 0) ? 1 : 0;
                controlConfigure.receventPB = wgAppConfig.getParamValBoolByNO(0x65) ? 1 : 0;
                controlConfigure.receventDS = wgAppConfig.getParamValBoolByNO(0x66) ? 1 : 0;

                /* A50 Specific WiFi Communication Parameters */
                controlConfigure.setWifiSsid(wgTools.SetObjToStr(reader["f_ControllerWiFiSSID"]));
                controlConfigure.setWifiKey(wgTools.SetObjToStr(reader["f_ControllerWiFiKey"]));
                controlConfigure.setWiFiEnable((int)reader["f_ControllerWiFiEnable"] > 0);

                string ip = wgTools.SetObjToStr(reader["f_ControllerWiFiIP"]);
                byte wifi_channel = controlConfigure.wifi_channel;
                controlConfigure.wifi_channel = 1;
                controlConfigure.ip = (ip == "" ? IPAddress.Parse("0.0.0.0") : IPAddress.Parse(ip));
                ip = wgTools.SetObjToStr(reader["f_ControllerWiFiMask"]);
                controlConfigure.mask = (ip == "" ? IPAddress.Parse("0.0.0.0") : IPAddress.Parse(ip));
                ip = wgTools.SetObjToStr(reader["f_ControllerWiFiGateway"]);
                controlConfigure.gateway = (ip == "" ? IPAddress.Parse("0.0.0.0") : IPAddress.Parse(ip));
                controlConfigure.port = ((int)reader["f_ControllerWiFiPort"]);
                controlConfigure.wifi_channel = wifi_channel;
                controlConfigure.setRtCamera((int)reader["f_RtCamera"]);
                controlConfigure.setTouchSensor((int)reader["f_TouchSensor"] > 0);
                controlConfigure.setM1Card((int)reader["f_M1Card"] > 0);
                controlConfigure.setVolume((int)reader["f_Volume"]);
                controlConfigure.setFpIdent((int)reader["f_FpIdent"] > 0);
            }
            reader.Close();
            int pwdNO = 0;
            while (pwdNO < 0x10)
            {
                pwdNO++;
                controlConfigure.SuperpasswordSet(pwdNO, 0xffff);
            }
            if (flag)
            {
                str = " SELECT f_ReaderNO  from t_b_Reader  ";
                str = (str + " where [t_b_Reader].[f_ControllerID] = " + ControllerID.ToString()) + " order by [f_ReaderNO] ASC";
                command.CommandText = str;
                ArrayList list = new ArrayList();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["f_ReaderNO"]);
                }
                reader.Close();
                str = " SELECT f_Password,t_b_Reader.f_ReaderNO,t_b_ReaderPassword.f_BAll,t_b_ReaderPassword.f_ReaderID   from t_b_ReaderPassword LEFT JOIN  t_b_Reader ON t_b_ReaderPassword.f_ReaderID = t_b_Reader.f_ReaderID ";
                str = str + " where [f_BAll] = 1 Or [t_b_Reader].[f_ControllerID] = " + ControllerID.ToString();
                command.CommandText = str;
                reader = command.ExecuteReader();
                int[] numArray7 = new int[] { 1, 1, 1, 1 };
                while (reader.Read())
                {
                    if (((int) reader["f_BAll"]) == 1)
                    {
                        if (numArray7[0] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(numArray7[0]++, (int) reader["f_Password"]);
                        }
                        if (numArray7[1] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(4 + numArray7[1]++, (int) reader["f_Password"]);
                        }
                        if (numArray7[2] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(8 + numArray7[2]++, (int) reader["f_Password"]);
                        }
                        if (numArray7[3] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(12 + numArray7[3]++, (int) reader["f_Password"]);
                        }
                    }
                    else
                    {
                        pwdNO = list.IndexOf(reader["f_ReaderNO"]);
                        if (numArray7[pwdNO] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(numArray7[pwdNO] + (pwdNO * 4), (int) reader["f_Password"]);
                            numArray7[pwdNO]++;
                        }
                    }
                }
                reader.Close();
            }
            controlConfigure.FirstCardInfoSet(1, 0);
            controlConfigure.FirstCardInfoSet(2, 0);
            controlConfigure.FirstCardInfoSet(3, 0);
            controlConfigure.FirstCardInfoSet(4, 0);
            controlTaskList = new wgMjControllerTaskList();
            if (wgAppConfig.getParamValBoolByNO(0x87))
            {
                str = " SELECT  f_FirstCard_Enabled,f_DoorNO ";
                str = ((((str + ", f_FirstCard_BeginHMS") + ", f_FirstCard_BeginControl " + ", f_FirstCard_EndHMS ") + ", f_FirstCard_EndControl" + ", f_FirstCard_Weekday ") + " FROM  t_b_door Where f_FirstCard_Enabled> 0 AND [f_ControllerID] = " + ControllerID.ToString()) + " ORDER BY f_DoorNO ";
                command.CommandText = str;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MjControlTaskItem mjCI = new MjControlTaskItem();
                    mjCI.ymdStart = DateTime.Parse("2010-1-1");
                    mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                    mjCI.hms = DateTime.Parse(reader["f_FirstCard_BeginHMS"].ToString());
                    mjCI.weekdayControl = (byte) ((int) reader["f_FirstCard_Weekday"]);
                    switch (((int) reader["f_FirstCard_BeginControl"]))
                    {
                        case 0:
                            mjCI.paramValue = 0x13;
                            break;

                        case 1:
                            mjCI.paramValue = 0x11;
                            break;

                        case 2:
                            mjCI.paramValue = 0x12;
                            break;

                        case 3:
                            mjCI.paramValue = 20;
                            break;

                        default:
                            mjCI.paramValue = 0;
                            break;
                    }
                    mjCI.paramLoc = (180 + ((byte) reader["f_DoorNO"])) - 1;
                    if (controlTaskList.AddItem(mjCI) < 0)
                    {
                        wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                    }
                    mjCI = new MjControlTaskItem();
                    mjCI.ymdStart = DateTime.Parse("2010-1-1");
                    mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                    mjCI.hms = DateTime.Parse(reader["f_FirstCard_EndHMS"].ToString());
                    mjCI.weekdayControl = (byte) ((int) reader["f_FirstCard_Weekday"]);
                    switch (((int) reader["f_FirstCard_EndControl"]))
                    {
                        case 0:
                            mjCI.paramValue = 0;
                            break;

                        case 1:
                            mjCI.paramValue = 0;
                            break;

                        case 2:
                            mjCI.paramValue = 0;
                            break;

                        case 3:
                            mjCI.paramValue = 4;
                            mjCI.paramValue = (byte) (mjCI.paramValue + 0x10);
                            break;

                        default:
                            mjCI.paramValue = 0;
                            break;
                    }
                    mjCI.paramLoc = (180 + ((byte) reader["f_DoorNO"])) - 1;
                    if (controlTaskList.AddItem(mjCI) < 0)
                    {
                        wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                    }
                    mjCI = new MjControlTaskItem();
                    mjCI.ymdStart = DateTime.Parse("2010-1-1");
                    mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                    mjCI.hms = DateTime.Parse(reader["f_FirstCard_EndHMS"].ToString());
                    mjCI.weekdayControl = (byte) ((int) reader["f_FirstCard_Weekday"]);
                    switch (((int) reader["f_FirstCard_EndControl"]))
                    {
                        case 0:
                            mjCI.paramValue = 3;
                            break;

                        case 1:
                            mjCI.paramValue = 1;
                            break;

                        case 2:
                            mjCI.paramValue = 2;
                            break;

                        case 3:
                            mjCI.paramValue = 3;
                            break;

                        default:
                            mjCI.paramValue = 3;
                            break;
                    }
                    mjCI.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                    if (controlTaskList.AddItem(mjCI) < 0)
                    {
                        wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                    }
                }
                reader.Close();
            }
            if (wgAppConfig.getParamValBoolByNO(0x83))
            {
                str = " SELECT t_b_ControllerTaskList.*,t_b_Door.f_DoorNO, t_b_Door.f_ControllerID FROM t_b_ControllerTaskList ";
                str = str + " LEFT JOIN t_b_Door ON t_b_ControllerTaskList.f_DoorID = t_b_Door.f_DoorID  where t_b_ControllerTaskList.[f_DoorID]=0 OR [f_controllerID]= " + ControllerID.ToString();
                command.CommandText = str;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int num16;
                    MjControlTaskItem item3;
                    int num17;
                    MjControlTaskItem item4;
                    int num18;
                    MjControlTaskItem item5;
                    int num21;
                    int num22;
                    MjControlTaskItem mjc = new MjControlTaskItem();
                    mjc.ymdStart = (DateTime) reader["f_BeginYMD"];
                    mjc.ymdEnd = (DateTime) reader["f_EndYMD"];
                    mjc.hms = (DateTime) reader["f_OperateTime"];
                    int num15 = 0;
                    num15 = (num15 * 2) + ((byte) reader["f_Sunday"]);
                    num15 = (num15 * 2) + ((byte) reader["f_Saturday"]);
                    num15 = (num15 * 2) + ((byte) reader["f_Friday"]);
                    num15 = (num15 * 2) + ((byte) reader["f_Thursday"]);
                    num15 = (num15 * 2) + ((byte) reader["f_Wednesday"]);
                    num15 = (num15 * 2) + ((byte) reader["f_Tuesday"]);
                    num15 = (num15 * 2) + ((byte) reader["f_Monday"]);
                    mjc.weekdayControl = (byte) num15;
                    mjc.paramLoc = 0;
                    if (((int) reader["f_DoorID"]) != 0)
                    {
                        goto Label_170E;
                    }
                    switch (((int) reader["f_DoorControl"]))
                    {
                        case 0:
                            mjc.paramValue = 3;
                            num16 = 0;
                            goto Label_1491;

                        case 1:
                            mjc.paramValue = 1;
                            num17 = 0;
                            goto Label_14DA;

                        case 2:
                            mjc.paramValue = 2;
                            num18 = 0;
                            goto Label_1523;

                        case 3:
                        case 4:
                        {
                            mjc.paramValue = 0;
                            if (((int) reader["f_DoorControl"]) == 3)
                            {
                                mjc.paramValue = 2;
                            }
                            for (int i = 0; i < wgMjController.GetControllerType(controllerSN); i++)
                            {
                                MjControlTaskItem item6 = new MjControlTaskItem();
                                item6.CopyFrom(mjc);
                                item6.paramLoc = 0x100 + i;
                                controlTaskList.AddItem(item6);
                            }
                            continue;
                        }
                        case 5:
                        case 6:
                        case 7:
                        {
                            mjc.paramValue = 0;
                            if ((((int) reader["f_DoorControl"]) == 7) || (((int) reader["f_DoorControl"]) == 6))
                            {
                                mjc.paramValue = 1;
                            }
                            for (int j = 0; j < 4; j++)
                            {
                                MjControlTaskItem item7 = new MjControlTaskItem();
                                item7.CopyFrom(mjc);
                                if (((wgMjController.GetControllerType(controllerSN) != 4) && (((int) reader["f_DoorControl"]) == 6)) && ((j == 1) || (j == 3)))
                                {
                                    item7.paramValue = 0;
                                }
                                item7.paramLoc = 0x26 + j;
                                controlTaskList.AddItem(item7);
                            }
                            continue;
                        }
                        case 8:
                        case 9:
                            mjc.paramValue = 0;
                            num21 = 0;
                            goto Label_169C;

                        case 10:
                            mjc.paramValue = 0;
                            num22 = 0;
                            goto Label_16D6;

                        default:
                            goto Label_16F9;
                    }
                Label_1465:
                    item3 = new MjControlTaskItem();
                    item3.CopyFrom(mjc);
                    item3.paramLoc = 0x1a + num16;
                    controlTaskList.AddItem(item3);
                    num16++;
                Label_1491:
                    if (num16 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_1465;
                    }
                    continue;
                Label_14AE:
                    item4 = new MjControlTaskItem();
                    item4.CopyFrom(mjc);
                    item4.paramLoc = 0x1a + num17;
                    controlTaskList.AddItem(item4);
                    num17++;
                Label_14DA:
                    if (num17 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_14AE;
                    }
                    continue;
                Label_14F7:
                    item5 = new MjControlTaskItem();
                    item5.CopyFrom(mjc);
                    item5.paramLoc = 0x1a + num18;
                    controlTaskList.AddItem(item5);
                    num18++;
                Label_1523:
                    if (num18 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_14F7;
                    }
                    continue;
                Label_1647:
                    if (((int) reader["f_DoorControl"]) == 8)
                    {
                        mjc.paramValue = (byte) controlConfigure.MorecardNeedCardsGet(num21 + 1);
                    }
                    MjControlTaskItem item8 = new MjControlTaskItem();
                    item8.CopyFrom(mjc);
                    item8.paramLoc = 0xb8 + num21;
                    controlTaskList.AddItem(item8);
                    num21++;
                Label_169C:
                    if (num21 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_1647;
                    }
                    continue;
                Label_16B9:
                    mjc.paramValue = (byte) (mjc.paramValue + ((byte) (((int) 1) << num22)));
                    num22++;
                Label_16D6:
                    if (num22 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_16B9;
                    }
                    mjc.paramLoc = 0x37;
                    controlTaskList.AddItem(mjc);
                    continue;
                Label_16F9:
                    mjc.paramValue = 0;
                    mjc.paramLoc = 0;
                    continue;
                Label_170E:
                    switch (((int) reader["f_DoorControl"]))
                    {
                        case 0:
                        {
                            mjc.paramValue = 3;
                            mjc.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                            if (controlTaskList.AddItem(mjc) < 0)
                            {
                                wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                            }
                            continue;
                        }
                        case 1:
                        {
                            mjc.paramValue = 1;
                            mjc.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                            if (controlTaskList.AddItem(mjc) < 0)
                            {
                                wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                            }
                            continue;
                        }
                        case 2:
                        {
                            mjc.paramValue = 2;
                            mjc.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                            if (controlTaskList.AddItem(mjc) < 0)
                            {
                                wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                            }
                            continue;
                        }
                        case 3:
                        case 4:
                        {
                            mjc.paramValue = 0;
                            if (((int) reader["f_DoorControl"]) == 3)
                            {
                                mjc.paramValue = 2;
                            }
                            mjc.paramLoc = (0x100 + ((byte) reader["f_DoorNO"])) - 1;
                            controlTaskList.AddItem(mjc);
                            continue;
                        }
                        case 5:
                        case 6:
                        case 7:
                        {
                            mjc.paramValue = 0;
                            if ((((int) reader["f_DoorControl"]) == 7) || (((int) reader["f_DoorControl"]) == 6))
                            {
                                mjc.paramValue = 1;
                            }
                            if (wgMjController.GetControllerType(controllerSN) == 4)
                            {
                                mjc.paramLoc = (0x26 + ((byte) reader["f_DoorNO"])) - 1;
                                controlTaskList.AddItem(mjc);
                            }
                            else if (((byte) reader["f_DoorNO"]) <= 2)
                            {
                                mjc.paramLoc = 0x26 + ((((byte) reader["f_DoorNO"]) - 1) * 2);
                                MjControlTaskItem item9 = new MjControlTaskItem();
                                item9.CopyFrom(mjc);
                                controlTaskList.AddItem(item9);
                                if (((int) reader["f_DoorControl"]) == 6)
                                {
                                    mjc.paramValue = 0;
                                }
                                mjc.paramLoc = (0x26 + ((((byte) reader["f_DoorNO"]) - 1) * 2)) + 1;
                                controlTaskList.AddItem(mjc);
                            }
                            continue;
                        }
                        case 8:
                        case 9:
                        {
                            mjc.paramValue = 0;
                            if (((int) reader["f_DoorControl"]) == 8)
                            {
                                mjc.paramValue = (byte) controlConfigure.MorecardNeedCardsGet((byte) reader["f_DoorNO"]);
                            }
                            mjc.paramLoc = (0xb8 + ((byte) reader["f_DoorNO"])) - 1;
                            controlTaskList.AddItem(mjc);
                            continue;
                        }
                        case 10:
                        {
                            mjc.paramValue = (byte) (((int) 1) << (((byte) reader["f_DoorNO"]) - 1));
                            mjc.paramLoc = 0x37;
                            controlTaskList.AddItem(mjc);
                            continue;
                        }
                    }
                    mjc.paramValue = 0;
                    mjc.paramLoc = 0;
                }
                reader.Close();
            }
            controlConfigure.controlTaskList_enabled = (controlTaskList.taskCount > 0) ? 1 : 0;
            if (wgAppConfig.getParamValBoolByNO(0x79))
            {
                str = " SELECT * FROM t_b_ControlHolidays ";
                command.CommandText = str;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MjControlHolidayTime mjCHT = new MjControlHolidayTime();
                    mjCHT.dtStart = (DateTime) reader["f_BeginYMDHMS"];
                    mjCHT.dtEnd = (DateTime) reader["f_EndYMDHMS"];
                    mjCHT.bForceWork = ((int) reader["f_forceWork"]) == 1;
                    controlHolidayList.AddItem(mjCHT);
                }
                reader.Close();
            }
            if (controlHolidayList.holidayCount > 0)
            {
                controlConfigure.holidayControl = 1;
            }
            else
            {
                controlConfigure.holidayControl = 0;
            }
            if (wgAppConfig.getParamValBoolByNO(0x90) && wgMjController.IsElevator(controllerSN))
            {
                int num23 = 0;
                str = " SELECT * FROM t_b_Floor WHERE [f_ControllerID] = " + ControllerID.ToString();
                command.CommandText = str;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (((int) reader["f_FloorNO"]) > 0)
                    {
                        if (((int) reader["f_FloorNO"]) <= 20)
                        {
                            num23 |= 1;
                        }
                        else if (((int) reader["f_FloorNO"]) <= 40)
                        {
                            num23 |= 2;
                        }
                    }
                }
                reader.Close();
                try
                {
                    int num24 = int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90));
                    if (num24 > 3)
                    {
                        controlConfigure.elevatorSingleDelay = (float) (((num24 >> 8) & 0xff) / 10M);
                        controlConfigure.elevatorMultioutputDelay = (float) (((num24 >> 0x10) & 0xff) / 10M);
                    }
                    else
                    {
                        controlConfigure.elevatorSingleDelay = 0.4f;
                        controlConfigure.elevatorMultioutputDelay = 5f;
                    }
                }
                catch (Exception)
                {
                }
            }
            return 1;
        }

        private static int getControllerConfigureFromDBByControllerID_Acc(int ControllerID, ref wgMjControllerConfigure controlConfigure, ref wgMjControllerTaskList controlTaskList, ref wgMjControllerHolidaysList controlHolidayList)
        {
            OleDbCommand command = null;
            OleDbConnection connection = null;
            int num = ControllerID;
            if (num <= 0)
            {
                return -1;
            }
            connection = new OleDbConnection(wgAppConfig.dbConString);
            int controllerSN = 0;
            int num3 = 0;
            connection.Open();
            string cmdText = " SELECT *, f_ZoneNO  FROM t_b_Controller LEFT JOIN t_b_Controller_Zone ON t_b_Controller_Zone.f_ZoneID = t_b_Controller.f_ZoneID WHERE f_ControllerID =  " + num.ToString();
            command = new OleDbCommand(cmdText, connection);
            command.CommandText = cmdText;
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                controllerSN = (int) reader["f_ControllerSN"];
                if (!string.IsNullOrEmpty(wgTools.SetObjToStr(reader["f_ZoneNO"])))
                {
                    num3 = int.Parse(wgTools.SetObjToStr(reader["f_ZoneNO"]));
                }
            }
            reader.Close();
            cmdText = " SELECT * from t_b_door where [f_controllerID]= " + ControllerID.ToString() + " order by [f_DoorNO] ASC";
            command.CommandText = cmdText;
            reader = command.ExecuteReader();
            int doorNO = 0;
            while (reader.Read())
            {
                doorNO++;
                controlConfigure.DoorControlSet(doorNO, (int) reader["f_DoorControl"]);
                controlConfigure.DoorDelaySet(doorNO, (int) reader["f_DoorDelay"]);
                if (wgAppConfig.getParamValBoolByNO(0x86))
                {
                    controlConfigure.MorecardNeedCardsSet(doorNO, (int) reader["f_MoreCards_Total"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 1, (int) reader["f_MoreCards_Grp1"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 2, (int) reader["f_MoreCards_Grp2"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 3, (int) reader["f_MoreCards_Grp3"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 4, (int) reader["f_MoreCards_Grp4"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 5, (int) reader["f_MoreCards_Grp5"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 6, (int) reader["f_MoreCards_Grp6"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 7, (int) reader["f_MoreCards_Grp7"]);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 8, (int) reader["f_MoreCards_Grp8"]);
                }
                else
                {
                    controlConfigure.MorecardNeedCardsSet(doorNO, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 1, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 2, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 3, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 4, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 5, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 6, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 7, 0);
                    controlConfigure.MorecardGroupNeedCardsSet(doorNO, 8, 0);
                }
                if ((((((int) reader["f_MoreCards_Grp1"]) > 0) || (((int) reader["f_MoreCards_Grp2"]) > 0)) || ((((int) reader["f_MoreCards_Grp3"]) > 0) || (((int) reader["f_MoreCards_Grp4"]) > 0))) || (((((int) reader["f_MoreCards_Grp5"]) > 0) || (((int) reader["f_MoreCards_Grp6"]) > 0)) || ((((int) reader["f_MoreCards_Grp7"]) > 0) || (((int) reader["f_MoreCards_Grp8"]) > 0))))
                {
                    controlConfigure.MorecardSequenceInputSet(doorNO, (((int) reader["f_MoreCards_Option"]) & 0x10) > 0);
                }
                else
                {
                    controlConfigure.MorecardSequenceInputSet(doorNO, false);
                }
                controlConfigure.MorecardSingleGroupEnableSet(doorNO, (((int) reader["f_MoreCards_Option"]) & 8) > 0);
                controlConfigure.MorecardSingleGroupStartNOSet(doorNO, (((int) reader["f_MoreCards_Option"]) & 7) + 1);
                controlConfigure.DoorDisableTimesegMinSet(doorNO, 0);
            }
            reader.Close();
            cmdText = " SELECT * from t_b_reader where [f_controllerID]= " + ControllerID.ToString() + " order by [f_ReaderNO] ASC";
            command.CommandText = cmdText;
            reader = command.ExecuteReader();
            int readerNO = 0;
            bool flag = wgAppConfig.getParamValBoolByNO(0x7b);
            while (reader.Read())
            {
                readerNO++;
                if (flag)
                {
                    controlConfigure.ReaderAuthModeSet(readerNO, (int)reader["f_AuthenticationMode"]);
                    controlConfigure.InputCardNOOpenSet(readerNO, (int) reader["f_InputCardno_Enabled"]);
                }
                else
                {
                    controlConfigure.ReaderAuthModeSet(readerNO, 0);
                    controlConfigure.InputCardNOOpenSet(readerNO, 0);
                }
            }
            reader.Close();
            int num6 = 0;
            if (wgAppConfig.getParamValBoolByNO(0x92))
            {
                string eName = "";
                string str3 = "";
                string notes = "";
                wgAppConfig.getSystemParamValue(0x92, out eName, out str3, out notes);
                if (!string.IsNullOrEmpty(wgTools.SetObjToStr(notes)))
                {
                    cmdText = string.Format(" SELECT * from t_b_door where [f_controllerID]= {0} AND [f_DoorID] IN ({1}) order by [f_DoorNO] ASC", ControllerID.ToString(), notes);
                    command.CommandText = cmdText;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        num6 |= ((int) 1) << (int.Parse(wgTools.SetObjToStr(reader["f_DoorNO"])) - 1);
                    }
                    reader.Close();
                }
            }
            controlConfigure.lockSwitchOption = num6;
            controlConfigure.swipeGap = int.Parse("0" + wgAppConfig.getSystemParamByNO(0x93));
            cmdText = " SELECT * from t_b_Controller where [f_controllerID]= " + ControllerID;
            command.CommandText = cmdText;
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                controlConfigure.DoorInterlockSet(1, 0);
                controlConfigure.DoorInterlockSet(2, 0);
                controlConfigure.DoorInterlockSet(3, 0);
                controlConfigure.DoorInterlockSet(4, 0);
                if (wgAppConfig.getParamValBoolByNO(0x85))
                {
                    switch (((int) reader["f_InterLock"]))
                    {
                        case 1:
                            controlConfigure.DoorInterlockSet(1, 0x31);
                            controlConfigure.DoorInterlockSet(2, 50);
                            break;

                        case 2:
                            controlConfigure.DoorInterlockSet(3, 0xc4);
                            controlConfigure.DoorInterlockSet(4, 200);
                            break;

                        case 3:
                            controlConfigure.DoorInterlockSet(1, 0x31);
                            controlConfigure.DoorInterlockSet(2, 50);
                            controlConfigure.DoorInterlockSet(3, 0xc4);
                            controlConfigure.DoorInterlockSet(4, 200);
                            break;

                        case 4:
                            controlConfigure.DoorInterlockSet(1, 0x71);
                            controlConfigure.DoorInterlockSet(2, 0x72);
                            controlConfigure.DoorInterlockSet(3, 0x74);
                            break;

                        case 8:
                            controlConfigure.DoorInterlockSet(1, 0xf1);
                            controlConfigure.DoorInterlockSet(2, 0xf2);
                            controlConfigure.DoorInterlockSet(3, 0xf4);
                            controlConfigure.DoorInterlockSet(4, 0xf8);
                            break;
                    }
                }
                if (wgAppConfig.getParamValBoolByNO(0x84))
                {
                    controlConfigure.antiback = ((int) reader["f_AntiBack"]) % 10;
                    controlConfigure.indoorPersonsMax = (((int) reader["f_AntiBack"]) - controlConfigure.antiback) / 10;
                }
                else
                {
                    controlConfigure.antiback = 0;
                    controlConfigure.indoorPersonsMax = 0;
                }
                controlConfigure.moreCardRead4Reader = (int) reader["f_MoreCards_GoInOut"];
                int num8 = int.Parse(wgAppConfig.getSystemParamByNO(40));
                controlConfigure.doorOpenTimeout = num8;
                string[] strArray = wgTools.SetObjToStr(reader["f_PeripheralControl"]).Split(new char[] { ',' });
                if (!wgAppConfig.getParamValBoolByNO(0x7c) || (strArray.Length != 0x1b))
                {
                    strArray = "126,30,30,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,10,10,0,0,0,0".Split(new char[] { ',' });
                }
                int[] numArray = new int[4];
                int[] numArray2 = new int[4];
                int[] numArray3 = new int[4];
                int[] numArray4 = new int[4];
                decimal[] numArray5 = new decimal[4];
                int[] numArray6 = new int[4];
                int index = 0;
                int num9 = int.Parse(strArray[index++]);
                int num10 = int.Parse(strArray[index++]);
                int num11 = int.Parse(strArray[index++]);
                numArray[0] = int.Parse(strArray[index++]);
                numArray[1] = int.Parse(strArray[index++]);
                numArray[2] = int.Parse(strArray[index++]);
                numArray[3] = int.Parse(strArray[index++]);
                numArray2[0] = int.Parse(strArray[index++]);
                numArray2[1] = int.Parse(strArray[index++]);
                numArray2[2] = int.Parse(strArray[index++]);
                numArray2[3] = int.Parse(strArray[index++]);
                numArray3[0] = int.Parse(strArray[index++]);
                numArray3[1] = int.Parse(strArray[index++]);
                numArray3[2] = int.Parse(strArray[index++]);
                numArray3[3] = int.Parse(strArray[index++]);
                numArray4[0] = int.Parse(strArray[index++]);
                numArray4[1] = int.Parse(strArray[index++]);
                numArray4[2] = int.Parse(strArray[index++]);
                numArray4[3] = int.Parse(strArray[index++]);
                numArray5[0] = decimal.Parse(strArray[index++]);
                numArray5[1] = decimal.Parse(strArray[index++]);
                numArray5[2] = decimal.Parse(strArray[index++]);
                numArray5[3] = decimal.Parse(strArray[index++]);
                numArray6[0] = int.Parse(strArray[index++]);
                numArray6[1] = int.Parse(strArray[index++]);
                numArray6[2] = int.Parse(strArray[index++]);
                numArray6[3] = int.Parse(strArray[index++]);
                controlConfigure.ext_AlarmControlMode = num9;
                controlConfigure.ext_SetAlarmOnDelay = num10;
                controlConfigure.ext_SetAlarmOffDelay = num11;
                for (index = 0; index < 4; index++)
                {
                    if (numArray6[index] > 0)
                    {
                        controlConfigure.Ext_doorSet(index, numArray[index]);
                        controlConfigure.Ext_controlSet(index, numArray2[index]);
                        controlConfigure.Ext_warnSignalEnabledSet(index, numArray3[index]);
                        controlConfigure.Ext_warnSignalEnabled2Set(index, numArray4[index]);
                        controlConfigure.Ext_timeoutSet(index, (int) numArray5[index]);
                    }
                    else
                    {
                        controlConfigure.Ext_doorSet(index, 0);
                        controlConfigure.Ext_controlSet(index, 0);
                        controlConfigure.Ext_warnSignalEnabledSet(index, 0);
                        controlConfigure.Ext_warnSignalEnabled2Set(index, 0);
                        controlConfigure.Ext_timeoutSet(index, 0);
                    }
                }
                int num13 = 0;
                num13 += (((int) reader["f_ForceWarn"]) > 0) ? 1 : 0;
                num13 += (((int) reader["f_DoorOpenTooLong"]) > 0) ? 2 : 0;
                num13 += (((int) reader["f_DoorInvalidOpen"]) > 0) ? 4 : 0;
                num13 += (((int)reader["f_DoorTamperWarn"]) > 0) ? 8 : 0;
                num13 += (((int) reader["f_InvalidCardWarn"]) > 0) ? 0x10 : 0;
                num13 += 0x20;
                if ((((numArray[0] == 0x10) && (numArray6[0] > 0)) || ((numArray[1] == 0x10) && (numArray6[1] > 0))) || (((numArray[2] == 0x10) && (numArray6[2] > 0)) || ((numArray[3] == 0x10) && (numArray6[3] > 0))))
                {
                    num13 += 0x40;
                }
                else
                {
                    controlConfigure.ext_Alarm_Status = 0;
                }
                if (wgAppConfig.getParamValBoolByNO(0x8d))
                {
                    num13 += 0x80;
                }
                if (!wgAppConfig.getParamValBoolByNO(0x7c))
                {
                    num13 = 0;
                }
                controlConfigure.warnSetup = num13;
                controlConfigure.xpPassword = int.Parse(wgAppConfig.getSystemParamByNO(0x18));
                if (!wgAppConfig.getParamValBoolByNO(0x7c) || !wgAppConfig.getParamValBoolByNO(60))
                {
                    controlConfigure.fire_broadcast_receive = 0;
                    controlConfigure.fire_broadcast_send = 0;
                }
                else
                {
                    controlConfigure.fire_broadcast_receive = 15;
                    controlConfigure.fire_broadcast_send = 1;
                    if ((wgAppConfig.getSystemParamByNO(60) == "2") && ((num3 > 0) & (num3 < 0xfd)))
                    {
                        controlConfigure.fire_broadcast_send = num3 + 1;
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(0x85) || !wgAppConfig.getParamValBoolByNO(0x3d))
                {
                    controlConfigure.interlock_broadcast_receive = 0;
                    controlConfigure.interlock_broadcast_send = 0;
                }
                else
                {
                    controlConfigure.interlock_broadcast_receive = 5;
                    controlConfigure.interlock_broadcast_send = 1;
                    if ((wgAppConfig.getSystemParamByNO(0x3d) == "2") && ((num3 > 0) & (num3 < 0xfd)))
                    {
                        controlConfigure.interlock_broadcast_send = num3 + 1;
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(0x84) || !wgAppConfig.getParamValBoolByNO(0x3e))
                {
                    controlConfigure.antiback_broadcast_send = 0;
                }
                else
                {
                    controlConfigure.antiback_broadcast_send = 1;
                    if ((wgAppConfig.getSystemParamByNO(0x3e) == "2") && ((num3 > 0) & (num3 < 0xfd)))
                    {
                        controlConfigure.antiback_broadcast_send = num3 + 1;
                    }
                }
                controlConfigure.receventWarn = (num13 > 0) ? 1 : 0;
                controlConfigure.receventPB = wgAppConfig.getParamValBoolByNO(0x65) ? 1 : 0;
                controlConfigure.receventDS = wgAppConfig.getParamValBoolByNO(0x66) ? 1 : 0;
            }
            reader.Close();
            int pwdNO = 0;
            while (pwdNO < 0x10)
            {
                pwdNO++;
                controlConfigure.SuperpasswordSet(pwdNO, 0xffff);
            }
            if (flag)
            {
                cmdText = " SELECT f_ReaderNO  from t_b_Reader  ";
                cmdText = (cmdText + " where [t_b_Reader].[f_ControllerID] = " + ControllerID.ToString()) + " order by [f_ReaderNO] ASC";
                command.CommandText = cmdText;
                ArrayList list = new ArrayList();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["f_ReaderNO"]);
                }
                reader.Close();
                cmdText = " SELECT f_Password,t_b_Reader.f_ReaderNO,t_b_ReaderPassword.f_BAll,t_b_ReaderPassword.f_ReaderID   from t_b_ReaderPassword LEFT JOIN  t_b_Reader ON t_b_ReaderPassword.f_ReaderID = t_b_Reader.f_ReaderID ";
                cmdText = cmdText + " where [f_BAll] = 1 Or [t_b_Reader].[f_ControllerID] = " + ControllerID.ToString();
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
                int[] numArray7 = new int[] { 1, 1, 1, 1 };
                while (reader.Read())
                {
                    if (((int) reader["f_BAll"]) == 1)
                    {
                        if (numArray7[0] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(numArray7[0]++, (int) reader["f_Password"]);
                        }
                        if (numArray7[1] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(4 + numArray7[1]++, (int) reader["f_Password"]);
                        }
                        if (numArray7[2] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(8 + numArray7[2]++, (int) reader["f_Password"]);
                        }
                        if (numArray7[3] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(12 + numArray7[3]++, (int) reader["f_Password"]);
                        }
                    }
                    else
                    {
                        pwdNO = list.IndexOf(reader["f_ReaderNO"]);
                        if (numArray7[pwdNO] <= 4)
                        {
                            controlConfigure.SuperpasswordSet(numArray7[pwdNO] + (pwdNO * 4), (int) reader["f_Password"]);
                            numArray7[pwdNO]++;
                        }
                    }
                }
                reader.Close();
            }
            controlConfigure.FirstCardInfoSet(1, 0);
            controlConfigure.FirstCardInfoSet(2, 0);
            controlConfigure.FirstCardInfoSet(3, 0);
            controlConfigure.FirstCardInfoSet(4, 0);
            controlTaskList = new wgMjControllerTaskList();
            if (wgAppConfig.getParamValBoolByNO(0x87))
            {
                cmdText = " SELECT  f_FirstCard_Enabled,f_DoorNO ";
                cmdText = ((((cmdText + ", f_FirstCard_BeginHMS") + ", f_FirstCard_BeginControl " + ", f_FirstCard_EndHMS ") + ", f_FirstCard_EndControl" + ", f_FirstCard_Weekday ") + " FROM  t_b_door Where f_FirstCard_Enabled> 0 AND [f_ControllerID] = " + ControllerID.ToString()) + " ORDER BY f_DoorNO ";
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MjControlTaskItem mjCI = new MjControlTaskItem();
                    mjCI.ymdStart = DateTime.Parse("2010-1-1");
                    mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                    mjCI.hms = DateTime.Parse(reader["f_FirstCard_BeginHMS"].ToString());
                    mjCI.weekdayControl = (byte) ((int) reader["f_FirstCard_Weekday"]);
                    switch (((int) reader["f_FirstCard_BeginControl"]))
                    {
                        case 0:
                            mjCI.paramValue = 0x13;
                            break;

                        case 1:
                            mjCI.paramValue = 0x11;
                            break;

                        case 2:
                            mjCI.paramValue = 0x12;
                            break;

                        case 3:
                            mjCI.paramValue = 0x14;
                            break;

                        default:
                            mjCI.paramValue = 0;
                            break;
                    }
                    mjCI.paramLoc = (0xb4 + ((byte) reader["f_DoorNO"])) - 1;
                    if (controlTaskList.AddItem(mjCI) < 0)
                    {
                        wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                    }
                    mjCI = new MjControlTaskItem();
                    mjCI.ymdStart = DateTime.Parse("2010-1-1");
                    mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                    mjCI.hms = DateTime.Parse(reader["f_FirstCard_EndHMS"].ToString());
                    mjCI.weekdayControl = (byte) ((int) reader["f_FirstCard_Weekday"]);
                    switch (((int) reader["f_FirstCard_EndControl"]))
                    {
                        case 0:
                            mjCI.paramValue = 0;
                            break;

                        case 1:
                            mjCI.paramValue = 0;
                            break;

                        case 2:
                            mjCI.paramValue = 0;
                            break;

                        case 3:
                            mjCI.paramValue = 4;
                            mjCI.paramValue = (byte) (mjCI.paramValue + 0x10);
                            break;

                        default:
                            mjCI.paramValue = 0;
                            break;
                    }
                    mjCI.paramLoc = (0xb4 + ((byte) reader["f_DoorNO"])) - 1;
                    if (controlTaskList.AddItem(mjCI) < 0)
                    {
                        wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                    }
                    mjCI = new MjControlTaskItem();
                    mjCI.ymdStart = DateTime.Parse("2010-1-1");
                    mjCI.ymdEnd = DateTime.Parse("2029-12-31");
                    mjCI.hms = DateTime.Parse(reader["f_FirstCard_EndHMS"].ToString());
                    mjCI.weekdayControl = (byte) ((int) reader["f_FirstCard_Weekday"]);
                    switch (((int) reader["f_FirstCard_EndControl"]))
                    {
                        case 0:
                            mjCI.paramValue = 3;
                            break;

                        case 1:
                            mjCI.paramValue = 1;
                            break;

                        case 2:
                            mjCI.paramValue = 2;
                            break;

                        case 3:
                            mjCI.paramValue = 3;
                            break;

                        default:
                            mjCI.paramValue = 3;
                            break;
                    }
                    mjCI.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                    if (controlTaskList.AddItem(mjCI) < 0)
                    {
                        wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                    }
                }
                reader.Close();
            }
            if (wgAppConfig.getParamValBoolByNO(0x83))
            {
                cmdText = " SELECT t_b_ControllerTaskList.*,t_b_Door.f_DoorNO, t_b_Door.f_ControllerID FROM t_b_ControllerTaskList ";
                cmdText = cmdText + " LEFT JOIN t_b_Door ON t_b_ControllerTaskList.f_DoorID = t_b_Door.f_DoorID  where t_b_ControllerTaskList.[f_DoorID]=0 OR [f_controllerID]= " + ControllerID.ToString();
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int num16;
                    MjControlTaskItem item3;
                    int num17;
                    MjControlTaskItem item4;
                    int num18;
                    MjControlTaskItem item5;
                    int num21;
                    int num22;
                    MjControlTaskItem mjc = new MjControlTaskItem();
                    mjc.ymdStart = (DateTime) reader["f_BeginYMD"];
                    mjc.ymdEnd = (DateTime) reader["f_EndYMD"];
                    mjc.hms = (DateTime) reader["f_OperateTime"];
                    int num15 = 0;
                    num15 = (num15 * 2) + ((byte)reader["f_Monday"]);
                    num15 = (num15 * 2) + ((byte)reader["f_Tuesday"]);
                    num15 = (num15 * 2) + ((byte)reader["f_Wednesday"]);
                    num15 = (num15 * 2) + ((byte)reader["f_Thursday"]);
                    num15 = (num15 * 2) + ((byte)reader["f_Friday"]);
                    num15 = (num15 * 2) + ((byte)reader["f_Saturday"]);
                    num15 = (num15 * 2) + ((byte)reader["f_Sunday"]);
                    mjc.weekdayControl = (byte) num15;
                    mjc.paramLoc = 0;
                    if (((int) reader["f_DoorID"]) != 0)
                    {
                        goto Label_16BD;
                    }
                    switch (((int) reader["f_DoorControl"]))
                    {
                        case 0:
                            mjc.paramValue = 3;
                            num16 = 0;
                            goto Label_1440;

                        case 1:
                            mjc.paramValue = 1;
                            num17 = 0;
                            goto Label_1489;

                        case 2:
                            mjc.paramValue = 2;
                            num18 = 0;
                            goto Label_14D2;

                        case 3:
                        case 4:
                        {
                            mjc.paramValue = 0;
                            if (((int) reader["f_DoorControl"]) == 3)
                            {
                                mjc.paramValue = 2;
                            }
                            for (int i = 0; i < wgMjController.GetControllerType(controllerSN); i++)
                            {
                                MjControlTaskItem item6 = new MjControlTaskItem();
                                item6.CopyFrom(mjc);
                                item6.paramLoc = 0x100 + i;
                                controlTaskList.AddItem(item6);
                            }
                            continue;
                        }
                        case 5:
                        case 6:
                        case 7:
                        {
                            mjc.paramValue = 0;
                            if ((((int) reader["f_DoorControl"]) == 7) || (((int) reader["f_DoorControl"]) == 6))
                            {
                                mjc.paramValue = 1;
                            }
                            for (int j = 0; j < 4; j++)
                            {
                                MjControlTaskItem item7 = new MjControlTaskItem();
                                item7.CopyFrom(mjc);
                                if (((wgMjController.GetControllerType(controllerSN) != 4) && (((int) reader["f_DoorControl"]) == 6)) && ((j == 1) || (j == 3)))
                                {
                                    item7.paramValue = 0;
                                }
                                item7.paramLoc = 0x26 + j;
                                controlTaskList.AddItem(item7);
                            }
                            continue;
                        }
                        case 8:
                        case 9:
                            mjc.paramValue = 0;
                            num21 = 0;
                            goto Label_164B;

                        case 10:
                            mjc.paramValue = 0;
                            num22 = 0;
                            goto Label_1685;

                        default:
                            goto Label_16A8;
                    }
                Label_1414:
                    item3 = new MjControlTaskItem();
                    item3.CopyFrom(mjc);
                    item3.paramLoc = 0x1a + num16;
                    controlTaskList.AddItem(item3);
                    num16++;
                Label_1440:
                    if (num16 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_1414;
                    }
                    continue;
                Label_145D:
                    item4 = new MjControlTaskItem();
                    item4.CopyFrom(mjc);
                    item4.paramLoc = 0x1a + num17;
                    controlTaskList.AddItem(item4);
                    num17++;
                Label_1489:
                    if (num17 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_145D;
                    }
                    continue;
                Label_14A6:
                    item5 = new MjControlTaskItem();
                    item5.CopyFrom(mjc);
                    item5.paramLoc = 0x1a + num18;
                    controlTaskList.AddItem(item5);
                    num18++;
                Label_14D2:
                    if (num18 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_14A6;
                    }
                    continue;
                Label_15F6:
                    if (((int) reader["f_DoorControl"]) == 8)
                    {
                        mjc.paramValue = (byte) controlConfigure.MorecardNeedCardsGet(num21 + 1);
                    }
                    MjControlTaskItem item8 = new MjControlTaskItem();
                    item8.CopyFrom(mjc);
                    item8.paramLoc = 0xb8 + num21;
                    controlTaskList.AddItem(item8);
                    num21++;
                Label_164B:
                    if (num21 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_15F6;
                    }
                    continue;
                Label_1668:
                    mjc.paramValue = (byte) (mjc.paramValue + ((byte) (((int) 1) << num22)));
                    num22++;
                Label_1685:
                    if (num22 < wgMjController.GetControllerType(controllerSN))
                    {
                        goto Label_1668;
                    }
                    mjc.paramLoc = 0x37;
                    controlTaskList.AddItem(mjc);
                    continue;
                Label_16A8:
                    mjc.paramValue = 0;
                    mjc.paramLoc = 0;
                    continue;
                Label_16BD:
                    switch (((int) reader["f_DoorControl"]))
                    {
                        case 0:
                        {
                            mjc.paramValue = 3;
                            mjc.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                            if (controlTaskList.AddItem(mjc) < 0)
                            {
                                wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                            }
                            continue;
                        }
                        case 1:
                        {
                            mjc.paramValue = 1;
                            mjc.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                            if (controlTaskList.AddItem(mjc) < 0)
                            {
                                wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                            }
                            continue;
                        }
                        case 2:
                        {
                            mjc.paramValue = 2;
                            mjc.paramLoc = (0x1a + ((byte) reader["f_DoorNO"])) - 1;
                            if (controlTaskList.AddItem(mjc) < 0)
                            {
                                wgTools.WgDebugWrite("controlTaskList.add(mjCI)", new object[0]);
                            }
                            continue;
                        }
                        case 3:
                        case 4:
                        {
                            mjc.paramValue = 0;
                            if (((int) reader["f_DoorControl"]) == 3)
                            {
                                mjc.paramValue = 2;
                            }
                            mjc.paramLoc = (0x100 + ((byte) reader["f_DoorNO"])) - 1;
                            controlTaskList.AddItem(mjc);
                            continue;
                        }
                        case 5:
                        case 6:
                        case 7:
                        {
                            mjc.paramValue = 0;
                            if ((((int) reader["f_DoorControl"]) == 7) || (((int) reader["f_DoorControl"]) == 6))
                            {
                                mjc.paramValue = 1;
                            }
                            if (wgMjController.GetControllerType(controllerSN) == 4)
                            {
                                mjc.paramLoc = (0x26 + ((byte) reader["f_DoorNO"])) - 1;
                                controlTaskList.AddItem(mjc);
                            }
                            else if (((byte) reader["f_DoorNO"]) <= 2)
                            {
                                mjc.paramLoc = 0x26 + ((((byte) reader["f_DoorNO"]) - 1) * 2);
                                MjControlTaskItem item9 = new MjControlTaskItem();
                                item9.CopyFrom(mjc);
                                controlTaskList.AddItem(item9);
                                if (((int) reader["f_DoorControl"]) == 6)
                                {
                                    mjc.paramValue = 0;
                                }
                                mjc.paramLoc = (0x26 + ((((byte) reader["f_DoorNO"]) - 1) * 2)) + 1;
                                controlTaskList.AddItem(mjc);
                            }
                            continue;
                        }
                        case 8:
                        case 9:
                        {
                            mjc.paramValue = 0;
                            if (((int) reader["f_DoorControl"]) == 8)
                            {
                                mjc.paramValue = (byte) controlConfigure.MorecardNeedCardsGet((byte) reader["f_DoorNO"]);
                            }
                            mjc.paramLoc = (0xb8 + ((byte) reader["f_DoorNO"])) - 1;
                            controlTaskList.AddItem(mjc);
                            continue;
                        }
                        case 10:
                        {
                            mjc.paramValue = (byte) (((int) 1) << (((byte) reader["f_DoorNO"]) - 1));
                            mjc.paramLoc = 0x37;
                            controlTaskList.AddItem(mjc);
                            continue;
                        }
                    }
                    mjc.paramValue = 0;
                    mjc.paramLoc = 0;
                }
                reader.Close();
            }
            controlConfigure.controlTaskList_enabled = (controlTaskList.taskCount > 0) ? 1 : 0;
            if (wgAppConfig.getParamValBoolByNO(0x79))
            {
                cmdText = " SELECT * FROM t_b_ControlHolidays ";
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MjControlHolidayTime mjCHT = new MjControlHolidayTime();
                    mjCHT.dtStart = (DateTime) reader["f_BeginYMDHMS"];
                    mjCHT.dtEnd = (DateTime) reader["f_EndYMDHMS"];
                    mjCHT.bForceWork = ((int) reader["f_forceWork"]) == 1;
                    controlHolidayList.AddItem(mjCHT);
                }
                reader.Close();
            }
            if (controlHolidayList.holidayCount > 0)
            {
                controlConfigure.holidayControl = 1;
            }
            else
            {
                controlConfigure.holidayControl = 0;
            }
            if (wgAppConfig.getParamValBoolByNO(0x90) && wgMjController.IsElevator(controllerSN))
            {
                int num23 = 0;
                cmdText = " SELECT * FROM t_b_Floor WHERE [f_ControllerID] = " + ControllerID.ToString();
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (((int) reader["f_FloorNO"]) > 0)
                    {
                        if (((int) reader["f_FloorNO"]) <= 20)
                        {
                            num23 |= 1;
                        }
                        else if (((int) reader["f_FloorNO"]) <= 40)
                        {
                            num23 |= 2;
                        }
                    }
                }
                reader.Close();
                try
                {
                    int num24 = int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90));
                    if (num24 > 3)
                    {
                        controlConfigure.elevatorSingleDelay = (float) (((num24 >> 8) & 0xff) / 10M);
                        controlConfigure.elevatorMultioutputDelay = (float) (((num24 >> 0x10) & 0xff) / 10M);
                    }
                    else
                    {
                        controlConfigure.elevatorSingleDelay = 0.4f;
                        controlConfigure.elevatorMultioutputDelay = 5f;
                    }
                }
                catch (Exception)
                {
                }
            }
            return 1;
        }
    }
}

