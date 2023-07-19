namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Threading;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;
    using System.Runtime.InteropServices;

    internal class icSwipeRecord : wgMjControllerSwipeOperate
    {
        private icController control = new icController();
        private static string gYMDHMSFormat = "yyyy-MM-dd HH:mm:ss";

        public icSwipeRecord()
        {
            base.Clear();
        }

        public static int AddNewSwipe_SynConsumerID(ref MjRec mjrec)
        {
            mjrec.loadPhotoFromDevice();
            int num = -9;
            if (wgAppConfig.IsAccessDB)
            {
                num = AddNewSwipe_SynConsumerID_Acc(mjrec);
            }
            else
            {
                string cmdText = "";
                try
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command = new SqlCommand(cmdText, connection))
                        {
                            connection.Open();
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            command.CommandType = CommandType.Text;
                            command.Connection = connection;
                            cmdText = "SELECT f_RecID FROM t_d_SwipeRecord ORDER BY f_RecID DESC";
                            command.CommandText = cmdText;
                            int num3 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            cmdText = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_ConsumerNO, f_Character, f_InOut, f_VerifMode, f_Status, f_RecOption, " +
                            	" f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_Photo, f_RecordAll) values (" +
							    wgTools.PrepareStr(mjrec.ReadDate, true, gYMDHMSFormat) + "," + 
                                mjrec.UserID.ToString() + "," + 
							    (mjrec.IsPassed ? "1" : "0") + "," + 
							    (mjrec.IsEnterIn ? "1" : "0") + "," + 
                                mjrec.verifMode.ToString() + ", " +
							    mjrec.bytStatus.ToString() + "," + 
							    mjrec.bytRecOption.ToString() + "," + 
							    mjrec.ControllerSN.ToString() + ",0" + "," + 
							    mjrec.ReaderNo.ToString() + "," +
                                mjrec.IndexInDataFlash.ToString() + ",";
                            if (mjrec.PhotoData == null)
                                cmdText += " NULL, ";
                            else
                                cmdText += "@photo, ";
                            cmdText += wgTools.PrepareStr(mjrec.ToStringRaw()) + ")" + ";";
                            command.CommandText = cmdText;
                            if (mjrec.PhotoData != null)
                            {
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@photo", mjrec.PhotoData);
                            }
                            if (command.ExecuteNonQuery() > 0)
                            {
                                cmdText = " UPDATE t_d_SwipeRecord   ";
                                cmdText = (((cmdText + " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ") + 
								    " FROM   t_d_SwipeRecord,t_b_Consumer " + 
                                    " WHERE  t_d_SwipeRecord.f_ConsumerNO = t_b_Consumer.f_ConsumerNO  ") + 
								    " AND  t_d_SwipeRecord.f_RecID >" + num3.ToString()) + 
								    " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
                                command.CommandText = cmdText;
                                int num2 = command.ExecuteNonQuery();
                                /*
                                cmdText = " UPDATE t_d_SwipeRecord   ";
                                cmdText = (((cmdText + " SET t_d_SwipeRecord.f_ConsumerNO=t_b_IDCard_Lost.f_ConsumerNO ") + 
								    " FROM   t_d_SwipeRecord,t_b_IDCard_Lost " + 
                                    " WHERE  t_d_SwipeRecord.f_ConsumerNO = t_b_IDCard_Lost.f_ConsumerNO  ") +
								    " AND  t_d_SwipeRecord.f_RecID >" + num3.ToString()) + 
								    " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
                                command.CommandText = cmdText;
                                num2 = command.ExecuteNonQuery();*/
                                cmdText = "UPDATE a  SET a.f_ReaderID=b.f_ReaderID ";
                                cmdText = ((((cmdText + " FROM t_d_SwipeRecord a ") + 
								    " INNER JOIN  t_b_Reader b  INNER JOIN t_b_Controller c ON c.f_ControllerID = b.f_ControllerID AND c.f_ControllerSN = " + 
								    mjrec.ControllerSN.ToString() + " ") + " ON  a.f_ReaderNO = b.f_ReaderNO AND a.f_ReaderNO =" + 
								    mjrec.ReaderNo.ToString()) + " WHERE a.f_RecID >" + num3.ToString()) + 
								    " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
                                command.CommandText = cmdText;
                                num2 = command.ExecuteNonQuery();
                                num = 1;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (num >= 0)
            {
                mjrec.RecID = ReadSwipeRecIDMax();
            }
            return num;
        }

        public static int AddNewSwipe_SynConsumerID_Acc(MjRec mjrec)
        {
            int num = -9;
            string cmdText = "";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        command.CommandType = CommandType.Text;
                        command.Connection = connection;
                        cmdText = "SELECT f_RecID FROM t_d_SwipeRecord ORDER BY f_RecID DESC";
                        command.CommandText = cmdText;
                        int num3 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                        cmdText = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_ConsumerNO, f_Character, f_InOut, f_VerifMode, f_Status, f_RecOption, ";
                        cmdText = (((((((((((cmdText + " f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_Photo, f_RecordAll) values (" + 
							wgTools.PrepareStr(mjrec.ReadDate, true, gYMDHMSFormat)) + "," + 
                            mjrec.UserID.ToString()) + "," + 
							(mjrec.IsPassed ? "1" : "0")) + "," + 
							(mjrec.IsEnterIn ? "1" : "0")) + "," + 
                            mjrec.verifMode.ToString() + ", " +
							mjrec.bytStatus.ToString()) + "," + 
							mjrec.bytRecOption.ToString()) + "," + 
							mjrec.ControllerSN.ToString()) + ",0") + "," + 
							mjrec.ReaderNo.ToString()) + "," + 
							mjrec.IndexInDataFlash.ToString()) + "," + 
                            "@photo, " +
                            wgTools.PrepareStr(mjrec.ToStringRaw())) + ")" + ";";
                        command.CommandText = cmdText;
                        command.Parameters.Clear();
                        if (mjrec.PhotoData == null)
                            command.Parameters.AddWithValue("@photo", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@photo", mjrec.PhotoData);
                        if (command.ExecuteNonQuery() > 0)
                        {
                            cmdText = " UPDATE t_d_SwipeRecord   ";
                            cmdText = ((cmdText + " INNER JOIN t_b_Consumer " + 
                                " ON (  t_d_SwipeRecord.f_ConsumerNO = t_b_Consumer.f_ConsumerNO  ") + 
								" AND  t_d_SwipeRecord.f_RecID >" + num3.ToString()) + 
								" AND (  (((f_RecOption / 2) mod 2) =0) OR ( (((f_RecOption/2) Mod 4) = 3) and ((((f_Status/128) Mod 2)=0) and (((f_Status/16) MOD 2)=0)))) " + 
								") SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ";
                            command.CommandText = cmdText;
                            int num2 = command.ExecuteNonQuery();
                            /*
                            cmdText = " UPDATE t_d_SwipeRecord   ";
                            cmdText = ((cmdText + " INNER JOIN t_b_IDCard_Lost " + 
                                " ON (  t_d_SwipeRecord.f_ConsumerNO = t_b_IDCard_Lost.f_ConsumerNO  ") +
								" AND  t_d_SwipeRecord.f_RecID >" + num3.ToString()) + 
								" AND (  (((f_RecOption / 2) mod 2) =0) OR ( (((f_RecOption/2) Mod 4) = 3) and ((((f_Status/128) Mod 2)=0) and (((f_Status/16) MOD 2)=0)))) " + 
								" ) SET t_d_SwipeRecord.f_ConsumerNO=t_b_IDCard_Lost.f_ConsumerNO ";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();*/
                            cmdText = "UPDATE t_d_SwipeRecord a  ";
                            cmdText = ((((cmdText + "  ") + " INNER JOIN ( t_b_Reader b  INNER JOIN t_b_Controller c ON ( c.f_ControllerID = b.f_ControllerID AND c.f_ControllerSN = " + mjrec.ControllerSN.ToString() + " ") + 
								" )) ON ( a.f_ReaderNO = b.f_ReaderNO AND a.f_ReaderNO =" + mjrec.ReaderNo.ToString()) + 
								" AND a.f_RecID >" + num3.ToString()) + 
								" AND (  (((f_RecOption / 2) mod 2) =0) OR ( (((f_RecOption/2) Mod 4) = 3) and ((((f_Status/128) Mod 2)=0) and (((f_Status/16) MOD 2)=0)))) " + 
								" ) SET a.f_ReaderID=b.f_ReaderID ";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            num = 1;
                        }
                    }
                    return num;
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public int GetSwipeRecords(int ControllerSN, string IP, int Port, string DoorName)
        {
            int num4;
            if (wgAppConfig.IsAccessDB)
            {
                return this.GetSwipeRecords_Acc(ControllerSN, IP, Port, DoorName);
            }
            wgTools.WriteLine("getSwipeRecords Start");
            this.control.ControllerSN = ControllerSN;
            this.control.IP = IP;
            this.control.PORT = Port;
            base.ControllerSN = ControllerSN;
            if (this.control.GetControllerRunInformationIP() < 0)
            {
                return -13;
            }
            if (this.control.runinfo.getNewRecordsNum() == 0)
            {
                base.lastRecordFlashIndex = (int) this.control.runinfo.lastGetRecordIndex;
                return 0;
            }
            if (base.wgudp == null)
            {
                base.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint)ControllerSN,
                getSwipeStartAddr(ControllerSN), getSwipeStartAddr(ControllerSN) + 0x3ff);
            byte[] cmd = tssi_flash_query.ToBytes(base.wgudp.udpPort);
            if (cmd == null)
            {
                return -12;
            }
            recv = null;
            if (base.wgudp.udp_get(cmd, 300, tssi_flash_query.xid, IP, Port, ref recv) < 0)
            {
                return -13;
            }
            wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
            int num2 = 0x1000;
            int num5 = 0;
            string cmdText = "SELECT f_RecID FROM t_d_SwipeRecord ORDER BY f_RecID DESC";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    num4 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                }
            }
            int indexInDataFlash = 0;
            int lastGetRecordIndex = (int) this.control.runinfo.lastGetRecordIndex;
            int[] numArray = new int[4];
            cmdText = " select f_ReaderID   ";
            cmdText = ((cmdText + " FROM   t_b_Reader, t_b_Controller " + " WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID ") + " AND  t_b_Controller.f_ControllerSN = " + ControllerSN.ToString()) + " ORDER BY f_ReaderNO ASC";
            int index = 0;
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    connection2.Open();
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        if (index >= 4)
                        {
                            break;
                        }
                        numArray[index] = (int) reader[0];
                        index++;
                    }
                    reader.Close();
                }
            }
            lastGetRecordIndex = (int) (this.control.runinfo.swipeEndIndex - this.control.runinfo.getNewRecordsNum());
            if (lastGetRecordIndex > 0)
            {
                uint swipeLoc = base.GetSwipeLoc(ControllerSN, (uint)lastGetRecordIndex);
                tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) ControllerSN, (uint) (swipeLoc - (swipeLoc % 0x400)), (uint) (((swipeLoc - (swipeLoc % 0x400)) + 0x400) - 1));
                cmd = tssi_flash_query.ToBytes(base.wgudp.udpPort);
                if (cmd == null)
                {
                    return -12;
                }
                recv = null;
                if (base.wgudp.udp_get(cmd, 300, tssi_flash_query.xid, IP, Port, ref recv) < 0)
                {
                    return -13;
                }
            }
            cmdText = "";
            uint iStartFlashAddr = tssi_flash_query.iStartFlashAddr;
            bool flag = false;
            uint num10 = 0;
            wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 0x400));
            int num11 = lastGetRecordIndex - (lastGetRecordIndex % (int)getSwipeRecordsMaxLen(ControllerSN));
            SqlConnection connection3 = new SqlConnection(wgAppConfig.dbConString);
            SqlCommand command3 = new SqlCommand(cmdText, connection3);
            connection3.Open();
        Label_0350:
            if (!wgMjControllerSwipeOperate.bStopGetRecord && (recv != null))
            {
                WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH(recv);
                MjRec rec = null;
                uint loc = 0;
                loc = (uint) (base.GetSwipeIndex(ControllerSN, tssi_flash.iStartFlashAddr) + num11);
                for (uint i = 0; i < 0x400; i += 0x10)
                {
                    rec = new MjRec(tssi_flash.ucData, i, tssi_flash.iDevSnFrom, loc, true);
                    if ((rec.UserID == uint.MaxValue) || (((rec.bytRecOption == 0) || 
                        (rec.bytRecOption == 0xff)) && (rec.UserID == 0)))
                    {
                        if (this.control.runinfo.swipeEndIndex <= loc)
                        {
                            break;
                        }
                        num10++;
                        loc++;
                    }
                    else
                    {
                        if ((num5 > 0) || (rec.IndexInDataFlash >= lastGetRecordIndex))
                        {
                            rec.loadPhotoFromDevice(IP, Port);
                            cmdText = " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_ConsumerNO, f_Character, f_InOut, f_VerifMode, f_Status, f_RecOption, " + 
								" f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_Photo, f_RecordAll) values (" + 
								wgTools.PrepareStr(rec.ReadDate, true, gYMDHMSFormat) + "," + 
                                rec.UserID.ToString() + "," + 
								(rec.IsPassed ? "1" : "0") + "," + 
								(rec.IsEnterIn ? "1" : "0") + "," + 
                                rec.verifMode.ToString() + ", " + 
								rec.bytStatus.ToString() + "," + 
								rec.bytRecOption.ToString() + "," + 
								rec.ControllerSN.ToString() + "," + 
								numArray[rec.ReaderNo - 1].ToString() + "," + 
								rec.ReaderNo.ToString() + "," + 
								rec.IndexInDataFlash.ToString() + ",";
                            if (rec.PhotoData == null)
                                cmdText += " NULL, ";
                            else
                                cmdText += "@photo, ";
                            cmdText += wgTools.PrepareStr(rec.ToStringRaw()) + ")" + ";";
                            command3.CommandText = cmdText;
                            if (rec.PhotoData != null)
                            {
                                command3.Parameters.Clear();
                                command3.Parameters.AddWithValue("@photo", rec.PhotoData);
                            }
                            index = command3.ExecuteNonQuery();
                            cmdText = "";
                            num5++;
                            indexInDataFlash = (int) rec.IndexInDataFlash;
                        }
                        loc++;
                    }
                }
                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}--[{2:d}]", DoorName, CommonStr.strGotRecords, num5));
                if (this.control.runinfo.swipeEndIndex <= loc)
                {
                    flag = true;
                }
                else if (!flag)
                {
                    tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) ControllerSN, tssi_flash_query.iStartFlashAddr + 0x400, ((tssi_flash_query.iStartFlashAddr + 0x400) + 0x400) - 1);
                    if (tssi_flash_query.iStartFlashAddr > getSwipeEndAddr(ControllerSN))
                    {
                        tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint)ControllerSN, getSwipeStartAddr(ControllerSN), getSwipeStartAddr(ControllerSN) + 0x3ff);
                        num11 += (int)getSwipeRecordsMaxLen(ControllerSN);
                    }
                    if (tssi_flash_query.iStartFlashAddr != iStartFlashAddr)
                    {
                        tssi_flash_query.GetNewXid();
                        cmd = tssi_flash_query.ToBytes(base.wgudp.udpPort);
                        if (((cmd != null) && (base.wgudp.udp_get(cmd, 300, tssi_flash_query.xid, IP, Port, ref recv) >= 0)) && (--num2 > 0))
                        {
                            goto Label_0350;
                        }
                    }
                }
            }
            connection3.Close();
            wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", tssi_flash_query.iStartFlashAddr / 0x400));
            wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num5));
            if (num10 > 0)
            {
                wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num10));
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}--[{2:d}]", DoorName, CommonStr.strWritingRecordsToDB, num5));
            cmdText = " UPDATE t_d_SwipeRecord   ";
            cmdText = (((cmdText + " SET t_d_SwipeRecord.f_ConsumerID=t_b_Consumer.f_ConsumerID ") +
                " FROM   t_d_SwipeRecord,t_b_Consumer " + " WHERE  t_d_SwipeRecord.f_ConsumerNO = t_b_Consumer.f_ConsumerNO  ") +
                " AND  t_d_SwipeRecord.f_RecID >" + num4.ToString()) +
                " AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
            using (SqlConnection connection5 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command5 = new SqlCommand(cmdText, connection5))
                {
                    connection5.Open();
                    command5.CommandTimeout = (num5 / 250) + 30;
                    index = command5.ExecuteNonQuery();
                }
            }
            /*
            cmdText = " UPDATE t_d_SwipeRecord   ";
            cmdText = (((cmdText + " SET t_d_SwipeRecord.f_ConsumerNO=t_b_Consumer.f_ConsumerNO ") +
                " FROM   t_d_SwipeRecord,t_b_IDCard_Lost,t_b_Consumer " + " WHERE  t_d_SwipeRecord.f_CardNO = t_b_IDCard_Lost.f_CardNO ") + 
				" AND  t_d_SwipeRecord.f_RecID >" + num4.ToString()) + 
				" AND (((f_RecOption & 2) =0) OR ((f_RecOption & (2+4)) = (2+4) and ((f_Status & (128+16))=0))) ";
            using (SqlConnection connection6 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command6 = new SqlCommand(cmdText, connection6))
                {
                    connection6.Open();
                    command6.CommandTimeout = (num5 / 250) + 30;
                    index = command6.ExecuteNonQuery();
                }
            }*/
            wgTools.WriteLine("Syn Data Info");
            if (num5 > 0)
            {
                if (this.control.GetControllerRunInformationIP() < 0)
                {
                    return -13;
                }
                if ((indexInDataFlash % getSwipeRecordsMaxLen(ControllerSN)) >= (this.control.runinfo.swipeEndIndex % getSwipeRecordsMaxLen(ControllerSN)))
                {
                    if (this.control.runinfo.swipeEndIndex > getSwipeRecordsMaxLen(ControllerSN))
                    {
                        lastGetRecordIndex = (((int)(this.control.runinfo.swipeEndIndex - (this.control.runinfo.swipeEndIndex % (int)getSwipeRecordsMaxLen(ControllerSN)))) - (int)getSwipeRecordsMaxLen(ControllerSN)) + (indexInDataFlash % (int)getSwipeRecordsMaxLen(ControllerSN));
                    }
                    else
                    {
                        lastGetRecordIndex = 0;
                    }
                }
                else
                {
                    lastGetRecordIndex = ((int)(this.control.runinfo.swipeEndIndex - (this.control.runinfo.swipeEndIndex % (int)getSwipeRecordsMaxLen(ControllerSN)))) + (indexInDataFlash % (int)getSwipeRecordsMaxLen(ControllerSN));
                }
                this.control.UpdateLastGetRecordLocationIP((uint) (lastGetRecordIndex + 1));
                base.lastRecordFlashIndex = lastGetRecordIndex + 1;
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus("");
            return num5;
        }

        public int GetSwipeRecords_Acc(int ControllerSN, string IP, int Port, string DoorName)
        {
            int num4;
            wgTools.WriteLine("getSwipeRecords_Acc Start");
            this.control.ControllerSN = ControllerSN;
            this.control.IP = IP;
            this.control.PORT = Port;
            base.ControllerSN = ControllerSN;
            if (this.control.GetControllerRunInformationIP() < 0)
            {
                return -13;
            }
            if (this.control.runinfo.getNewRecordsNum() == 0)
            {
                base.lastRecordFlashIndex = (int) this.control.runinfo.lastGetRecordIndex;
                return 0;
            }
            if (base.wgudp == null)
            {
                base.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint)ControllerSN, getSwipeStartAddr(ControllerSN), getSwipeStartAddr(ControllerSN) + 0x3ff);
            byte[] cmd = tssi_flash_query.ToBytes(base.wgudp.udpPort);
            if (cmd == null)
            {
                return -12;
            }
            recv = null;
            if (base.wgudp.udp_get(cmd, 300, tssi_flash_query.xid, IP, Port, ref recv) < 0)
            {
                return -13;
            }
            wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
            int num2 = 0x1000;
            int num5 = 0;
            string cmdText = "SELECT f_RecID FROM t_d_SwipeRecord ORDER BY f_RecID DESC";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    num4 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                }
            }
            int indexInDataFlash = 0;
            int lastGetRecordIndex = (int) this.control.runinfo.lastGetRecordIndex;
            int[] numArray = new int[4];
            cmdText = " select f_ReaderID   ";
            cmdText = ((cmdText + " FROM   t_b_Reader, t_b_Controller " + " WHERE t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID ") + " AND  t_b_Controller.f_ControllerSN = " + ControllerSN.ToString()) + " ORDER BY f_ReaderNO ASC";
            int index = 0;
            using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command2 = new OleDbCommand(cmdText, connection2))
                {
                    connection2.Open();
                    OleDbDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        if (index >= 4)
                        {
                            break;
                        }
                        numArray[index] = (int) reader[0];
                        index++;
                    }
                    reader.Close();
                }
            }
            lastGetRecordIndex = (int) (this.control.runinfo.swipeEndIndex - this.control.runinfo.getNewRecordsNum());
            if (lastGetRecordIndex > 0)
            {
                uint swipeLoc = base.GetSwipeLoc(ControllerSN, (uint)lastGetRecordIndex);
                tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) ControllerSN, (uint) (swipeLoc - (swipeLoc % 0x400)), (uint) (((swipeLoc - (swipeLoc % 0x400)) + 0x400) - 1));
                cmd = tssi_flash_query.ToBytes(base.wgudp.udpPort);
                if (cmd == null)
                {
                    return -12;
                }
                recv = null;
                if (base.wgudp.udp_get(cmd, 300, tssi_flash_query.xid, IP, Port, ref recv) < 0)
                {
                    return -13;
                }
            }
            cmdText = "";
            uint iStartFlashAddr = tssi_flash_query.iStartFlashAddr;
            bool flag = false;
            uint num10 = 0;
            wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 0x400));
            int num11 = lastGetRecordIndex - (lastGetRecordIndex % (int)getSwipeRecordsMaxLen(ControllerSN));
            OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString);
            OleDbCommand command3 = new OleDbCommand(cmdText, connection3);
            connection3.Open();
        Label_035B:
            if (!wgMjControllerSwipeOperate.bStopGetRecord && (recv != null))
            {
                WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH(recv);
                MjRec rec = null;
                uint loc = 0;
                loc = (uint) (base.GetSwipeIndex(ControllerSN, tssi_flash.iStartFlashAddr) + num11);
                for (uint i = 0; i < 0x400; i += 0x10)
                {
                    rec = new MjRec(tssi_flash.ucData, i, tssi_flash.iDevSnFrom, loc, true);
                    if ((rec.UserID == uint.MaxValue) || (((rec.bytRecOption == 0) ||
                        (rec.bytRecOption == 0xff)) && (rec.UserID == 0)))
                    {
                        if (this.control.runinfo.swipeEndIndex <= loc)
                        {
                            break;
                        }
                        num10++;
                        loc++;
                    }
                    else
                    {
                        if ((num5 > 0) || (rec.IndexInDataFlash >= lastGetRecordIndex))
                        {
                            rec.loadPhotoFromDevice(IP, Port);
                            cmdText = "";
                            cmdText = ((((((((((((cmdText + " INSERT INTO t_d_SwipeRecord (f_ReadDate, f_ConsumerNO, f_Character, f_InOut, f_VerifMode, f_Status, f_RecOption, ") + 
								" f_ControllerSN, f_ReaderID, f_ReaderNO, f_RecordFlashLoc, f_Photo, f_RecordAll) values (" + 
								wgTools.PrepareStr(rec.ReadDate, true, gYMDHMSFormat)) + "," + 
                                rec.UserID.ToString()) + "," + 
								(rec.IsPassed ? "1" : "0")) + "," + 
								(rec.IsEnterIn ? "1" : "0")) + "," + 
                                rec.verifMode.ToString() + ", " + 
								rec.bytStatus.ToString()) + "," + 
								rec.bytRecOption.ToString()) + "," + 
								rec.ControllerSN.ToString()) + "," + 
								numArray[rec.ReaderNo - 1].ToString()) + "," + 
								rec.ReaderNo.ToString()) + "," + 
								rec.IndexInDataFlash.ToString()) + "," + 
                                "@photo, " +
                                wgTools.PrepareStr(rec.ToStringRaw())) + ")";
                            command3.CommandText = cmdText;
                            command3.Parameters.Clear();
                            if (rec.PhotoData == null)
                                command3.Parameters.AddWithValue("@photo", DBNull.Value);
                            else
                                command3.Parameters.AddWithValue("@photo", rec.PhotoData);
                            index = command3.ExecuteNonQuery();
                            cmdText = "";
                            num5++;
                            indexInDataFlash = (int) rec.IndexInDataFlash;
                        }
                        loc++;
                    }
                }
                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}--[{2:d}]", DoorName, CommonStr.strGotRecords, num5));
                if (this.control.runinfo.swipeEndIndex <= loc)
                {
                    flag = true;
                }
                else if (!flag)
                {
                    tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) ControllerSN, tssi_flash_query.iStartFlashAddr + 0x400, ((tssi_flash_query.iStartFlashAddr + 0x400) + 0x400) - 1);
                    if (tssi_flash_query.iStartFlashAddr > getSwipeEndAddr(ControllerSN))
                    {
                        tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint)ControllerSN, getSwipeStartAddr(ControllerSN), getSwipeStartAddr(ControllerSN) + 0x3ff);
                        num11 += (int)getSwipeRecordsMaxLen(ControllerSN);
                    }
                    if (tssi_flash_query.iStartFlashAddr != iStartFlashAddr)
                    {
                        tssi_flash_query.GetNewXid();
                        cmd = tssi_flash_query.ToBytes(base.wgudp.udpPort);
                        if (((cmd != null) && (base.wgudp.udp_get(cmd, 300, tssi_flash_query.xid, IP, Port, ref recv) >= 0)) && (--num2 > 0))
                        {
                            goto Label_035B;
                        }
                    }
                }
            }
            connection3.Close();
            wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", tssi_flash_query.iStartFlashAddr / 0x400));
            wgTools.WriteLine(string.Format("Got Records:\t Count={0:d}", num5));
            if (num10 > 0)
            {
                wgAppConfig.wgLog(string.Format("Got Records:\t invalidRecCount={0:d}", num10));
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}--[{2:d}]", DoorName, CommonStr.strWritingRecordsToDB, num5));
            cmdText = " UPDATE t_d_SwipeRecord   ";
            cmdText = (((cmdText + " INNER JOIN   t_b_Consumer " + 
                " ON  (t_d_SwipeRecord.f_ConsumerNO = t_b_Consumer.f_ConsumerNO  ") + 
				" AND  t_d_SwipeRecord.f_RecID >" + num4.ToString()) + 
				" AND (  (((f_RecOption / 2) mod 2) =0) OR ( (((f_RecOption/2) Mod 4) = 3) and ((((f_Status/128) Mod 2)=0) and (((f_Status/16) MOD 2)=0)))) ") + " )" + 
				" SET t_d_SwipeRecord.f_ConsumerID=  t_b_Consumer.f_ConsumerID ";
            using (OleDbConnection connection4 = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command4 = new OleDbCommand(cmdText, connection4))
                {
                    connection4.Open();
                    command4.CommandTimeout = (num5 / 250) + 30;
                    index = command4.ExecuteNonQuery();
                }
            }
            /*
            cmdText = " UPDATE t_d_SwipeRecord   ";
            cmdText = (((cmdText + " INNER JOIN   t_b_IDCard_Lost " + 
                " ON (  t_d_SwipeRecord.f_ConsumerNO = t_b_IDCard_Lost.f_ConsumerNO  ") +
				" AND  t_d_SwipeRecord.f_RecID >" + num4.ToString()) + 
				" AND (  (((f_RecOption / 2) mod 2) =0) OR ( (((f_RecOption/2) Mod 4) = 3) and ((((f_Status/128) Mod 2)=0) and (((f_Status/16) MOD 2)=0)))) ") + " )" + 
				" SET t_d_SwipeRecord.f_ConsumerNO=t_b_IDCard_Lost.f_ConsumerNO ";
            using (OleDbConnection connection5 = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command5 = new OleDbCommand(cmdText, connection5))
                {
                    connection5.Open();
                    command5.CommandTimeout = (num5 / 250) + 30;
                    index = command5.ExecuteNonQuery();
                }
            }*/
            wgTools.WriteLine("Syn Data Info");
            if (num5 > 0)
            {
                if (this.control.GetControllerRunInformationIP() < 0)
                {
                    return -13;
                }
                if ((indexInDataFlash % getSwipeRecordsMaxLen(ControllerSN)) >= (this.control.runinfo.swipeEndIndex % getSwipeRecordsMaxLen(ControllerSN)))
                {
                    if (this.control.runinfo.swipeEndIndex > getSwipeRecordsMaxLen(ControllerSN))
                    {
                        lastGetRecordIndex = (((int)(this.control.runinfo.swipeEndIndex - (this.control.runinfo.swipeEndIndex % (int)getSwipeRecordsMaxLen(ControllerSN)))) - (int)getSwipeRecordsMaxLen(ControllerSN)) + (indexInDataFlash % (int)getSwipeRecordsMaxLen(ControllerSN));
                    }
                    else
                    {
                        lastGetRecordIndex = 0;
                    }
                }
                else
                {
                    lastGetRecordIndex = ((int)(this.control.runinfo.swipeEndIndex - (this.control.runinfo.swipeEndIndex % (int)getSwipeRecordsMaxLen(ControllerSN)))) + (indexInDataFlash % (int)getSwipeRecordsMaxLen(ControllerSN));
                }
                this.control.UpdateLastGetRecordLocationIP((uint) (lastGetRecordIndex + 1));
                base.lastRecordFlashIndex = lastGetRecordIndex + 1;
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus("");
            return num5;
        }

        public int GetSwipeRecordsByDoorName(string DoorName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.GetSwipeRecordsByDoorName_Acc(DoorName);
            }
            int num = -1;
            string cmdText = " SELECT f_ControllerSN, f_IP, f_Port";
            cmdText = cmdText + " FROM t_b_Controller a, t_b_Door b WHERE a.f_ControllerID = b.f_ControllerID AND b.f_DoorName =  " + wgTools.PrepareStr(DoorName);
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = this.GetSwipeRecords((int) reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int) reader["f_Port"], DoorName);
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public int GetSwipeRecordsByDoorName_Acc(string DoorName)
        {
            int num = -1;
            string cmdText = " SELECT f_ControllerSN, f_IP, f_Port";
            cmdText = cmdText + " FROM t_b_Controller a, t_b_Door b WHERE a.f_ControllerID = b.f_ControllerID AND b.f_DoorName =  " + wgTools.PrepareStr(DoorName);
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = this.GetSwipeRecords((int) reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int) reader["f_Port"], DoorName);
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public static int ReadSwipeRecIDMax()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return ReadSwipeRecIDMax_Acc();
            }
            int num = 0;
            string cmdText = "SELECT f_RecID FROM t_d_SwipeRecord ORDER BY f_RecID DESC";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    num = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                }
            }
            return num;
        }

        public static int ReadSwipeRecIDMax_Acc()
        {
            int num = 0;
            string cmdText = "SELECT f_RecID FROM t_d_SwipeRecord ORDER BY f_RecID DESC";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    num = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                }
            }
            return num;
        }
    }
}

