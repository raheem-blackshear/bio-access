using System.Data;
namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Collections;
    using System.Collections.Generic;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;

    internal class icConsumer
    {
        private const int ConsumerNOMinLen = 10;
        public int gConsumerID;
        public string gYMDFormat = "yyyy-MM-dd";

        public int addNew(string newConsumerNO, string ConsumerName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.addNew_Acc(newConsumerNO, ConsumerName);
            }
            int num = -9;
            if (newConsumerNO == null)
            {
                return -401;
            }
            if (newConsumerNO == "")
            {
                return -401;
            }
            if (ConsumerName == null)
            {
                return -201;
            }
            if (ConsumerName == "")
            {
                return -201;
            }
            string str2 = Convert.ToInt32(newConsumerNO).ToString();// newConsumerNO.PadLeft(10, ' ');
            try
            {
                string str = "";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(str, connection))
                    {
                        connection.Open();
                        if (str2 != "")
                        {
                            command.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + wgTools.PrepareStr(str2);
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        string cmdText = "BEGIN TRANSACTION";
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();
                        try
                        {
                            cmdText = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_BeginYMD ) values (";
                            cmdText = ((cmdText + wgTools.PrepareStr(str2) + ",") + wgTools.PrepareStr(ConsumerName) + ",") + wgTools.PrepareStr("2012-01-01", true, this.gYMDFormat) + ")";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStr(str2);
                            command.CommandText = cmdText;
                            int num2 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            this.gConsumerID = num2;
                            cmdText = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
                            cmdText = cmdText + num2.ToString() + ")";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNew(string newConsumerNO, string ConsumerName, long CardNO, int password, int deptID)
        {
            if (wgAppConfig.IsAccessDB)
                return this.addNew_Acc(newConsumerNO, ConsumerName, CardNO, password, deptID);

            int num = -9;
            if (newConsumerNO == null)
                return -401;

            if (newConsumerNO == "")
                return -401;

            if (ConsumerName == null)
                return -201;

            if (ConsumerName == "")
                return -201;

            string str2 = Convert.ToInt32(newConsumerNO).ToString();// newConsumerNO.PadLeft(10, ' ');
            try
            {
                string str = "";
                int num2;

                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(str, connection2))
                    {
                        connection2.Open();
                        if (str2 != "")
                        {
                            command2.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + wgTools.PrepareStr(str2);
                            SqlDataReader reader = command2.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        if (CardNO > 0L)
                        {
                            command2.CommandText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                            if (int.Parse("0" + wgTools.SetObjToStr(command2.ExecuteScalar())) > 0)
                            {
                                return -103;
                            }
                            str = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                            command2.CommandText = str;
                            num2 = command2.ExecuteNonQuery();
                        }
                        if (password > 0)
                        {
                            command2.CommandText = "SELECT f_ConsumerID From t_b_Consumer WHERE [f_PIN]= " + password.ToString();
                            if (int.Parse("0" + wgTools.SetObjToStr(command2.ExecuteScalar())) > 0)
                                return -103;
                        }
                        command2.CommandText = "BEGIN TRANSACTION";
                        command2.ExecuteNonQuery();
                        try
                        {
                            str = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_CardNO, f_PIN, f_BeginYMD) values (";
                            str = str + wgTools.PrepareStr(str2) + "," + 
                                wgTools.PrepareStr(ConsumerName) + "," +
                                wgTools.PrepareStr(deptID) + "," + 
                                ((CardNO > 0L) ? CardNO.ToString() : "NULL") + "," +
                                ((password > 0) ? password.ToString() : "NULL") + "," + 
                                wgTools.PrepareStr("2012-01-01", true, this.gYMDFormat) + ")";
                            command2.CommandText = str;
                            num2 = command2.ExecuteNonQuery();
                            str = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStr(str2);
                            command2.CommandText = str;
                            int num3 = int.Parse("0" + wgTools.SetObjToStr(command2.ExecuteScalar()));
                            this.gConsumerID = num3;
                            str = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
                            str = str + num3.ToString() + ")";
                            command2.CommandText = str;
                            num2 = command2.ExecuteNonQuery();
                            int num4 = 0;
                            if (CardNO > 0L)
                            {
                                str = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                                command2.CommandText = str;
                                SqlDataReader reader = command2.ExecuteReader();
                                while (reader.Read())
                                {
                                    num4++;
                                    if (num4 > 1)
                                    {
                                        break;
                                    }
                                }
                                reader.Close();
                                if (num4 <= 1)
                                {
                                    str = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                    command2.CommandText = str;
                                    reader = command2.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        num4++;
                                        if (num4 > 1)
                                        {
                                            break;
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            if (num4 > 1)
                            {
                                str = "ROLLBACK TRANSACTION";
                                command2.CommandText = str;
                                command2.ExecuteNonQuery();
                                return num;
                            }
                            str = "COMMIT TRANSACTION";
                            command2.CommandText = str;
                            command2.ExecuteNonQuery();
                            return 1;
                        }
                        catch (Exception)
                        {
                            str = "ROLLBACK TRANSACTION";
                            command2.CommandText = str;
                            command2.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNew(string newConsumerNO, string ConsumerName, int GroupID,
            byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, DateTime BeginYMD, DateTime EndYMD,
            string PIN, long CardNO, List<MjFpTempl> fp_templ, MjFaceTempl face_templ)
        {
            if (wgAppConfig.IsAccessDB)
                return this.addNew_Acc(newConsumerNO, ConsumerName, GroupID, AttendEnabled,
                    ShiftEnabled, DoorEnabled, BeginYMD, EndYMD, PIN, CardNO, fp_templ, face_templ);

            int num = -9;
            if (newConsumerNO == null)
                return -401;
            
            if (newConsumerNO == "")
                return -401;
            
            if (ConsumerName == null)
                return -201;
            
            if (ConsumerName == "")
                return -201;
            
            string cmdText = "";
            string str2 = Convert.ToInt32(newConsumerNO).ToString();// newConsumerNO.PadLeft(10, ' ');
            try
            {
                byte num4 = ShiftEnabled;
                if (AttendEnabled == 0)
                {
                    num4 = 0;
                }
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        int num2;
                        connection.Open();
                        if (str2 != "")
                        {
                            command.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + wgTools.PrepareStr(str2);
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        if (PIN != "")
                        {
                            cmdText = "SELECT f_PIN FROM t_b_Consumer WHERE f_PIN = " + PIN;
                            command.CommandText = cmdText;
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -501;
                            }
                            reader.Close();
                        }
                        if (CardNO > 0L)
                        {
                            cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                            command.CommandText = cmdText;
                            if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                            {
                                return -103;
                            }
                            cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                        }
                        cmdText = "BEGIN TRANSACTION";
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();
                        wgTools.WriteLine("BEGIN TRANSACTION End: ");
                        try
                        {
                            cmdText = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD,f_EndYMD,f_PIN, f_CardNO) values (";
                            cmdText = ((((((((((cmdText + wgTools.PrepareStr(str2)) + "," + wgTools.PrepareStr(ConsumerName)) + "," + GroupID.ToString()) + "," + AttendEnabled.ToString()) + "," + num4.ToString()) + "," + DoorEnabled.ToString()) + "," + wgTools.PrepareStr(BeginYMD, true, this.gYMDFormat)) + "," + wgTools.PrepareStr(EndYMD, true, this.gYMDFormat)) + "," + (PIN != "" ? PIN : "NULL")) + "," + ((CardNO > 0L) ? CardNO.ToString() : "NULL")) + ")";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            wgTools.WriteLine("INSERT INTO t_b_Consumer End: ");
                            cmdText = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStr(str2);
                            command.CommandText = cmdText;
                            int num3 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            wgTools.WriteLine("SELECT f_ConsumerID End: ");
                            this.gConsumerID = num3;

                            cmdText = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
                            cmdText = cmdText + num3.ToString() + ")";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            wgTools.WriteLine("INSERT INTO t_b_Consumer_Other End: ");
                            int num5 = 0;
                            if (CardNO > 0L)
                            {
                                cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                                command.CommandText = cmdText;
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    num5++;
                                    if (num5 > 1)
                                    {
                                        break;
                                    }
                                }
                                reader.Close();
                                if (num5 <= 1)
                                {
                                    cmdText = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                    command.CommandText = cmdText;
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        num5++;
                                        if (num5 > 1)
                                        {
                                            break;
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            if (num5 > 1)
                            {
                                cmdText = "ROLLBACK TRANSACTION";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = "COMMIT TRANSACTION";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();

                                // Insert fingerprint templates
                                if (fp_templ != null)
                                {
                                    foreach (MjFpTempl e in fp_templ)
                                    {
                                        cmdText = " INSERT INTO t_d_FpTempl (f_ConsumerID, f_Finger, f_Templ, f_Duress) VALUES (" +
                                            num3.ToString() + ", " + e.finger.ToString() + ", @templ, " + (e.duress ? "1" : "0") + ")";
                                        command.CommandText = cmdText;
                                        command.Parameters.Clear();
                                        command.Parameters.AddWithValue("@templ", e.data);
                                        command.ExecuteNonQuery();
                                    }
                                }

                                // Insert face template
                                if (face_templ != null)
                                {
                                    cmdText = " INSERT INTO t_d_FaceTempl (f_ConsumerID, f_Templ) VALUES (" +
                                        num3.ToString() + ", @templ)";
                                    command.CommandText = cmdText;
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@templ", face_templ.data);
                                    command.ExecuteNonQuery();

									cmdText = " UPDATE t_d_FaceTemplChanged SET f_Changed = 1";
									command.CommandText = cmdText;
									command.ExecuteNonQuery();
                                }
                                num = 1;
                            }
                            wgTools.WriteLine("COMMIT TRANSACTION End: ");
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine("ROLLBACK TRANSACTION End: ");
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNew_Acc(string newConsumerNO, string ConsumerName)
        {
            int num = -9;
            if (newConsumerNO == null)
                return -401;

            if (newConsumerNO == "")
                return -401;

            if (ConsumerName == null)
                return -201;

            if (ConsumerName == "")
                return -201;

            string str2 = Convert.ToInt32(newConsumerNO).ToString();//newConsumerNO.PadLeft(10, ' ');
            try
            {
                string cmdText = "";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (str2 != "")
                        {
                            command.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + wgTools.PrepareStr(str2);
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        command.CommandText = "BEGIN TRANSACTION";
                        command.ExecuteNonQuery();
                        try
                        {
                            cmdText = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_BeginYMD ) values (";
                            cmdText = ((cmdText + wgTools.PrepareStr(str2) + ",") + wgTools.PrepareStr(ConsumerName) + ",") + wgTools.PrepareStr("2012-01-01", true, this.gYMDFormat) + ")";
                            command.CommandText = cmdText;
                            cmdText = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStr(str2);
                            command.CommandText = cmdText;
                            int num2 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            this.gConsumerID = num2;
                            cmdText = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
                            cmdText = cmdText + num2.ToString() + ")";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNew_Acc(string newConsumerNO, string ConsumerName, long CardNO, int password, int deptID)
        {
            int num = -9;
            if (newConsumerNO == null)
                return -401;

            if (newConsumerNO == "")
                return -401;

            if (ConsumerName == null)
                return -201;

            if (ConsumerName == "")
                return -201;

            string str2 = Convert.ToInt32(newConsumerNO).ToString();// newConsumerNO.PadLeft(10, ' ');
            try
            {
                string str = "";
                int num2;

                using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command2 = new OleDbCommand(str, connection2))
                    {
                        connection2.Open();
                        if (str2 != "")
                        {
                            command2.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + wgTools.PrepareStr(str2);
                            OleDbDataReader reader = command2.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        if (CardNO > 0L)
                        {
                            command2.CommandText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                            if (int.Parse("0" + wgTools.SetObjToStr(command2.ExecuteScalar())) > 0)
                                return -103;

                            command2.CommandText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                            num2 = command2.ExecuteNonQuery();
                        }
                        if (password > 0)
                        {
                            command2.CommandText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_PIN]= " + password.ToString();
                            if (int.Parse("0" + wgTools.SetObjToStr(command2.ExecuteScalar())) > 0)
                                return -103;
                        }
                        command2.CommandText = "BEGIN TRANSACTION";
                        command2.ExecuteNonQuery();
                        try
                        {
                            str = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_CardNO, f_PIN, f_BeginYMD) values (";
                            str = str + wgTools.PrepareStr(str2) + "," +
                                wgTools.PrepareStr(ConsumerName) + "," +
                                wgTools.PrepareStr(deptID) + "," +
                                ((CardNO > 0L) ? CardNO.ToString() : "NULL") + "," +
                                ((password > 0) ? password.ToString() : "NULL") + "," +
                                wgTools.PrepareStr("2012-01-01", true, this.gYMDFormat) + ")";
                            command2.CommandText = str;
                            num2 = command2.ExecuteNonQuery();
                            str = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStr(str2);
                            command2.CommandText = str;
                            int num3 = int.Parse("0" + wgTools.SetObjToStr(command2.ExecuteScalar()));
                            this.gConsumerID = num3;
                            str = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
                            str = str + num3.ToString() + ")";
                            command2.CommandText = str;
                            num2 = command2.ExecuteNonQuery();
                            int num4 = 0;
                            if (CardNO > 0L)
                            {
                                str = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                                command2.CommandText = str;
                                OleDbDataReader reader = command2.ExecuteReader();
                                while (reader.Read())
                                {
                                    num4++;
                                    if (num4 > 1)
                                    {
                                        break;
                                    }
                                }
                                reader.Close();
                                if (num4 <= 1)
                                {
                                    str = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                    command2.CommandText = str;
                                    reader = command2.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        num4++;
                                        if (num4 > 1)
                                        {
                                            break;
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            if (num4 > 1)
                            {
                                str = "ROLLBACK TRANSACTION";
                                command2.CommandText = str;
                                command2.ExecuteNonQuery();
                                return num;
                            }
                            str = "COMMIT TRANSACTION";
                            command2.CommandText = str;
                            command2.ExecuteNonQuery();
                            return 1;
                        }
                        catch (Exception)
                        {
                            str = "ROLLBACK TRANSACTION";
                            command2.CommandText = str;
                            command2.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNew_Acc(string newConsumerNO, string ConsumerName, int GroupID, 
            byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled,
            DateTime BeginYMD, DateTime EndYMD, string PIN, long CardNO, List<MjFpTempl> fp_templ, MjFaceTempl face_templ)
        {
            int num = -9;
            if (newConsumerNO == null)
                return -401;
            
            if (newConsumerNO == "")
                return -401;
            
            if (ConsumerName == null)
                return -201;
            
            if (ConsumerName == "")
                return -201;
            
            string cmdText = "";
            string str2 = Convert.ToInt32(newConsumerNO).ToString();// newConsumerNO.PadLeft(10, ' ');
            try
            {
                byte num4 = ShiftEnabled;
                if (AttendEnabled == 0)
                {
                    num4 = 0;
                }
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        int num2;
                        connection.Open();
                        if (str2 != "")
                        {
                            command.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + wgTools.PrepareStr(str2);
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        if (PIN != "")
                        {
                            cmdText = "SELECT f_PIN FROM t_b_Consumer WHERE f_PIN = " + PIN;
                            command.CommandText = cmdText;
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -501;
                            }
                            reader.Close();
                        }
                        if (CardNO > 0L)
                        {
                            cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                            command.CommandText = cmdText;
                            if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                            {
                                return -103;
                            }
                            cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                        }
                        cmdText = "BEGIN TRANSACTION";
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();
                        wgTools.WriteLine("BEGIN TRANSACTION End: ");
                        try
                        {
                            cmdText = " INSERT INTO t_b_Consumer (f_ConsumerNO, f_ConsumerName, f_GroupID, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled,f_BeginYMD,f_EndYMD,f_PIN, f_CardNO) values (";
                            cmdText = ((((((((((cmdText + wgTools.PrepareStr(str2)) + "," + wgTools.PrepareStr(ConsumerName)) + "," + GroupID.ToString()) + "," + AttendEnabled.ToString()) + "," + num4.ToString()) + "," + DoorEnabled.ToString()) + "," + wgTools.PrepareStr(BeginYMD, true, this.gYMDFormat)) + "," + wgTools.PrepareStr(EndYMD, true, this.gYMDFormat)) + "," + (PIN != "" ? PIN : "NULL")) + "," + ((CardNO > 0L) ? CardNO.ToString() : "NULL")) + ")";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            wgTools.WriteLine("INSERT INTO t_b_Consumer End: ");
                            cmdText = "SELECT f_ConsumerID from [t_b_Consumer] where f_ConsumerNo =" + wgTools.PrepareStr(str2);
                            command.CommandText = cmdText;
                            int num3 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            wgTools.WriteLine("SELECT f_ConsumerID End: ");
                            this.gConsumerID = num3;

                            /* Insert fingerprint templates */
                            if (fp_templ != null)
                            {
                                foreach (MjFpTempl e in fp_templ)
                                {
                                    cmdText = "INSERT INTO t_d_FpTempl (f_ConsumerID, f_Finger, f_Templ, f_Duress) VALUES (" +
                                        num3.ToString() + ", " + e.finger.ToString() + ", @templ, " + (e.duress ? "'1'" : "'0'") + ")";
                                    command.CommandText = cmdText;
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@templ", e.data);
                                    command.ExecuteNonQuery();
                                }
                            }

                            /* Insert fingerprint templates */
                            if (face_templ != null)
                            {
                                cmdText = "INSERT INTO t_d_FaceTempl (f_ConsumerID, f_Templ) VALUES (" +
                                    num3.ToString() + ", @templ)";
                                command.CommandText = cmdText;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@templ", face_templ.data);
                                command.ExecuteNonQuery();

								cmdText = " UPDATE t_d_FaceTemplChanged SET f_Changed = 1";
								command.CommandText = cmdText;
								command.ExecuteNonQuery();
                            }

                            cmdText = " INSERT INTO t_b_Consumer_Other (f_ConsumerID) values (";
                            cmdText = cmdText + num3.ToString() + ")";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            wgTools.WriteLine("INSERT INTO t_b_Consumer_Other End: ");
                            int num5 = 0;
                            if (CardNO > 0L)
                            {
                                cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                                command.CommandText = cmdText;
                                OleDbDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    num5++;
                                    if (num5 > 1)
                                    {
                                        break;
                                    }
                                }
                                reader.Close();
                                if (num5 <= 1)
                                {
                                    cmdText = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                    command.CommandText = cmdText;
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        num5++;
                                        if (num5 > 1)
                                        {
                                            break;
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            if (num5 > 1)
                            {
                                cmdText = "ROLLBACK TRANSACTION";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = "COMMIT TRANSACTION";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                                num = 1;
                            }
                            wgTools.WriteLine("COMMIT TRANSACTION End: ");
                        }
                        catch
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine("ROLLBACK TRANSACTION End: ");
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNewCard(int ConsumerID, long CardNO)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.addNewCard_Acc(ConsumerID, CardNO);
            }
            int num = -9;
            try
            {
                string cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return -103;
                        }
                        cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                        command.CommandText = cmdText;
                        int num2 = command.ExecuteNonQuery();
                        cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                        command.CommandText = cmdText;
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return -103;
                        }
                        cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                        command.CommandText = cmdText;
                        num2 = command.ExecuteNonQuery();
                        cmdText = ("UPDATE t_b_Consumer SET  [f_CardNO]= " + CardNO.ToString()) + " WHERE f_ConsumerID = " + ConsumerID.ToString();
                        command.CommandText = cmdText;
                        if (command.ExecuteNonQuery() == 1)
                        {
                            num = 1;
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int addNewCard_Acc(int ConsumerID, long CardNO)
        {
            int num = -9;
            try
            {
                string cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return -103;
                        }
                        cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                        command.CommandText = cmdText;
                        int num2 = command.ExecuteNonQuery();
                        cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                        command.CommandText = cmdText;
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return -103;
                        }
                        cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                        command.CommandText = cmdText;
                        num2 = command.ExecuteNonQuery();
                        cmdText = ("UPDATE t_b_Consumer SET  [f_CardNO]= " + CardNO.ToString()) + " WHERE f_ConsumerID = " + ConsumerID.ToString();
                        command.CommandText = cmdText;
                        if (command.ExecuteNonQuery() == 1)
                        {
                            num = 1;
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public long ConsumerNONext()
        {
            return this.ConsumerNONext("");
        }

        public long ConsumerNONext(string startcaption)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.ConsumerNONext_Acc(startcaption);
            }
            long num = 0L;
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT [f_ConsumerNO] FROM [t_b_Consumer] ";
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            long result = -1L;
                            string s = wgTools.SetObjToStr(reader[0]);
                            if (s == "")
                            {
                                result = 0L;
                            }
                            else
                            {
                                long.TryParse(s, out result);
                                if (((result <= 0L) && !string.IsNullOrEmpty(startcaption)) && ((s.IndexOf(startcaption) == 0) && s.StartsWith(startcaption)))
                                {
                                    s = s.Substring(startcaption.Length);
                                    if (s == "")
                                    {
                                        result = 0L;
                                    }
                                    else
                                    {
                                        long.TryParse(s, out result);
                                    }
                                }
                            }
                            if (num < result)
                            {
                                num = result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return (num + 1L);
        }

        public long ConsumerNONext_Acc(string startcaption)
        {
            long num = 0L;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT [f_ConsumerNO] FROM [t_b_Consumer] ";
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            long result = -1L;
                            string s = wgTools.SetObjToStr(reader[0]);
                            if (s == "")
                            {
                                result = 0L;
                            }
                            else
                            {
                                long.TryParse(s, out result);
                                if (((result <= 0L) && !string.IsNullOrEmpty(startcaption)) && ((s.IndexOf(startcaption) == 0) && s.StartsWith(startcaption)))
                                {
                                    s = s.Substring(startcaption.Length);
                                    if (s == "")
                                    {
                                        result = 0L;
                                    }
                                    else
                                    {
                                        long.TryParse(s, out result);
                                    }
                                }
                            }
                            if (num < result)
                            {
                                num = result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return (num + 1L);
        }

        public long ConsumerNONextWithSpace()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.ConsumerNONextWithSpace_Acc();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT max(Right('                    ' + [f_ConsumerNO],20)) FROM [t_b_Consumer] ";
                        long result = -1L;
                        string s = wgTools.SetObjToStr(command.ExecuteScalar());
                        if (s == "")
                        {
                            result = 1L;
                        }
                        else
                        {
                            long.TryParse(s, out result);
                            if (result > 0L)
                            {
                                result += 1L;
                            }
                        }
                        if (result > 0L)
                        {
                            command.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStr(result.ToString());
                            if (wgTools.SetObjToStr(command.ExecuteScalar()) == "")
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1L;
        }

        public long ConsumerNONextWithSpace_Acc()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT max(Right('                    ' + [f_ConsumerNO],20)) FROM [t_b_Consumer] ";
                        long result = -1L;
                        string s = wgTools.SetObjToStr(command.ExecuteScalar());
                        if (s == "")
                        {
                            result = 1L;
                        }
                        else
                        {
                            long.TryParse(s, out result);
                            if (result > 0L)
                            {
                                result += 1L;
                            }
                        }
                        if (result > 0L)
                        {
                            command.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStr(result.ToString());
                            if (wgTools.SetObjToStr(command.ExecuteScalar()) == "")
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1L;
        }

        public long ConsumerNONextWithZero()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.ConsumerNONextWithZero_Acc();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT max(Right('00000000000000000000' + RTRIM(LTRIM([f_ConsumerNO])),20)) FROM [t_b_Consumer] ";
                        long result = -1L;
                        string s = wgTools.SetObjToStr(command.ExecuteScalar());
                        if (s == "")
                        {
                            result = 1L;
                        }
                        else
                        {
                            long.TryParse(s, out result);
                            if (result > 0L)
                            {
                                result += 1L;
                            }
                        }
                        if (result > 0L)
                        {
                            command.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStr(result.ToString());
                            if (wgTools.SetObjToStr(command.ExecuteScalar()) == "")
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1L;
        }

        public long ConsumerNONextWithZero_Acc()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT max(Right('00000000000000000000' + RTRIM(LTRIM([f_ConsumerNO])),20)) FROM [t_b_Consumer] ";
                        long result = -1L;
                        string s = wgTools.SetObjToStr(command.ExecuteScalar());
                        if (s == "")
                        {
                            result = 1L;
                        }
                        else
                        {
                            long.TryParse(s, out result);
                            if (result > 0L)
                            {
                                result += 1L;
                            }
                        }
                        if (result > 0L)
                        {
                            command.CommandText = "SELECT ([f_ConsumerNO]) FROM [t_b_Consumer] WHERE f_ConsumerNO=" + wgTools.PrepareStr(result.ToString());
                            if (wgTools.SetObjToStr(command.ExecuteScalar()) == "")
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1L;
        }

        public int deleteAllUser()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.deleteAllUser_Acc();
            }
            int num = -9;
            try
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
                            cmdText = " DELETE  FROM t_b_UserFloor ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE  FROM t_d_ShiftData ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_Leave ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_ManualCardRecord ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_d_Privilege] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_b_IDCard_Lost] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_b_Consumer_Other] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_d_FpTempl] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            command.CommandText = " DELETE FROM [t_d_FaceTempl] ";
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_b_Consumer] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int deleteAllUser_Acc()
        {
            int num = -9;
            try
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
                            cmdText = " DELETE  FROM t_b_UserFloor ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE  FROM t_d_ShiftData ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_Leave ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_ManualCardRecord ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_d_Privilege] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_b_IDCard_Lost] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_b_Consumer_Other] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_d_FpTempl] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            command.CommandText = " DELETE FROM [t_d_FaceTempl] ";
                            command.ExecuteNonQuery();
                            cmdText = " DELETE FROM [t_b_Consumer] ";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            num = 1;
                        }
                        catch (Exception)
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int deleteUser(int ConsumerID)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.deleteUser_Acc(ConsumerID);
            }
            int num = -9;
            try
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
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_b_UserFloor ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_ShiftData ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_Leave ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_ManualCardRecord ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_d_Privilege] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_b_IDCard_Lost] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_b_Consumer_Other] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = "DELETE FROM [t_d_FpTempl] WHERE [f_ConsumerID] = " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = "DELETE FROM [t_d_FaceTempl] WHERE [f_ConsumerID] = " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_b_Consumer] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            num = 1;
                        }
                        catch (Exception exception)
                        {
                            this.wgDebugWrite(exception.ToString());
                            this.wgDebugWrite(cmdText);
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception2)
            {
                this.wgDebugWrite(exception2.ToString());
            }
            return num;
        }

        public int deleteUser_Acc(int ConsumerID)
        {
            int num = -9;
            try
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
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_b_UserFloor ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_ShiftData ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_Leave ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE  FROM t_d_ManualCardRecord ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_d_Privilege] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_b_IDCard_Lost] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_b_Consumer_Other] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = "DELETE FROM [t_d_FpTempl] WHERE [f_ConsumerID] = " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = "DELETE FROM [t_d_FaceTempl] WHERE [f_ConsumerID] = " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = " DELETE FROM [t_b_Consumer] ";
                            cmdText = cmdText + "  WHERE [f_ConsumerID]=" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                            num = 1;
                        }
                        catch (Exception exception)
                        {
                            this.wgDebugWrite(exception.ToString());
                            this.wgDebugWrite(cmdText);
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            wgTools.WriteLine(cmdText);
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception2)
            {
                this.wgDebugWrite(exception2.ToString());
            }
            return num;
        }

        public int editUser(int ConsumerID, string ConsumerNO, string ConsumerName, int GroupID,
            byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, 
            DateTime BeginYMD, DateTime EndYMD, string PIN, long CardNO, List<MjFpTempl> fp_templ, MjFaceTempl face_templ)
        {
            if (wgAppConfig.IsAccessDB)
                return this.editUser_Acc(ConsumerID, ConsumerNO, ConsumerName, GroupID,
                    AttendEnabled, ShiftEnabled, DoorEnabled, BeginYMD, EndYMD, PIN, CardNO, fp_templ, face_templ);
            
            int num = -9;
            if (ConsumerNO == null)
                return -401;
            
            if (ConsumerNO == "")
                return -401;
            
            if (ConsumerName == null)
                return -201;
            
            if (ConsumerName == "")
                return -201;
            
            string cmdText = "";
            string str2 = Convert.ToInt32(ConsumerNO).ToString();// ConsumerNO.PadLeft(10, ' ');
            try
            {
                byte num3 = ShiftEnabled;
                if (AttendEnabled == 0)
                {
                    num3 = 0;
                }
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        int num2;
                        connection.Open();
                        if (str2 != "")
                        {
                            command.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + 
                                wgTools.PrepareStr(str2) + " AND f_ConsumerID <> " + ConsumerID.ToString();
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        if (PIN != "")
                        {
                            cmdText = "SELECT f_PIN FROM t_b_Consumer WHERE f_PIN = " + PIN + 
                                " AND f_ConsumerID <> " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -501;
                            }
                            reader.Close();
                        }
                        if (CardNO > 0L)
                        {
                            cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                            command.CommandText = cmdText;
                            num2 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            if (num2 > 0)
                            {
                                if (ConsumerID != num2)
                                {
                                    return -103;
                                }
                            }
                            else
                            {
                                cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                command.CommandText = cmdText;
                                num2 = command.ExecuteNonQuery();
                            }
                        }
                        cmdText = "BEGIN TRANSACTION";
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();
                        try
                        {
                            cmdText = "UPDATE t_b_Consumer SET f_ConsumerNO =" + wgTools.PrepareStr(str2) + 
                                ", f_ConsumerName = " + wgTools.PrepareStr(ConsumerName) + 
                                ", f_GroupID = " + GroupID.ToString() + 
                                ", f_AttendEnabled = " + AttendEnabled.ToString() + 
                                ", f_ShiftEnabled = " + num3.ToString() + 
                                ", f_DoorEnabled= " + DoorEnabled.ToString() + 
                                ", f_BeginYMD=" + wgTools.PrepareStr(BeginYMD, true, this.gYMDFormat) + 
                                ", f_EndYMD=" + wgTools.PrepareStr(EndYMD, true, this.gYMDFormat) + 
                                ", f_PIN=" + (PIN != "" ? PIN : "NULL") + 
                                ", f_CardNO = " + ((CardNO > 0L) ? CardNO.ToString() : "NULL") + 
                                " WHERE f_ConsumerID =" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();

                            int num4 = 0;
                            if (CardNO > 0L)
                            {
                                cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                                command.CommandText = cmdText;
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    num4++;
                                    if (num4 > 1)
                                    {
                                        break;
                                    }
                                }
                                reader.Close();
                                if (num4 <= 1)
                                {
                                    cmdText = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                    command.CommandText = cmdText;
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        num4++;
                                        if (num4 > 1)
                                        {
                                            break;
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            if (num4 > 1)
                            {
                                cmdText = "ROLLBACK TRANSACTION";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                                return num;
                            }
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            
							// Update fingerprint templates
							cmdText = "DELETE FROM t_d_FpTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
							command.CommandText = cmdText;
							command.ExecuteNonQuery();
                            if (fp_templ != null)
                            {
                                foreach (MjFpTempl e in fp_templ)
                                {
                                    cmdText = "INSERT INTO t_d_FpTempl (f_ConsumerID, f_Finger, f_Templ, f_Duress) VALUES (" +
                                        ConsumerID.ToString() + ", " + e.finger.ToString() + ", @templ, " + (e.duress ? "'1'" : "'0'") + ")";
                                    command.CommandText = cmdText;
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@templ", e.data);
                                    command.ExecuteNonQuery();
                                }
                            }

                            // Update face template
							cmdText = "DELETE FROM t_d_FaceTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
							command.CommandText = cmdText;
							command.ExecuteNonQuery();
                            if (face_templ != null)
                            {
                                cmdText = "INSERT INTO t_d_FaceTempl (f_ConsumerID, f_Templ) VALUES (" +
                                    ConsumerID.ToString() + ", @templ)";
                                command.CommandText = cmdText;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@templ", face_templ.data);
                                command.ExecuteNonQuery();
                            }
							
							// Mark changed
							cmdText = " UPDATE t_d_FaceTemplChanged SET f_Changed = 1";
							command.CommandText = cmdText;
							command.ExecuteNonQuery();

                            return 1;
                        }
                        catch
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int editUser_Acc(int ConsumerID, string ConsumerNO, string ConsumerName, int GroupID,
                byte AttendEnabled, byte ShiftEnabled, byte DoorEnabled, 
                DateTime BeginYMD, DateTime EndYMD, string PIN, long CardNO, List<MjFpTempl> fp_templ, MjFaceTempl face_templ)
        {
            int num = -9;
            if (ConsumerNO == null)
                return -401;
            
            if (ConsumerNO == "")
                return -401;
            
            if (ConsumerName == null)
                return -201;
            
            if (ConsumerName == "")
                return -201;
            
            string cmdText = "";
            string str2 = Convert.ToInt32(ConsumerNO).ToString();// ConsumerNO.PadLeft(10, ' ');
            try
            {
                byte num3 = ShiftEnabled;
                if (AttendEnabled == 0)
                {
                    num3 = 0;
                }
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        int num2;
                        connection.Open();
                        if (str2 != "")
                        {
                            command.CommandText = "SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerNO = " + 
                                wgTools.PrepareStr(str2) + "AND f_ConsumerID <> " + ConsumerID.ToString();
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -401;
                            }
                            reader.Close();
                        }
                        if (PIN != "")
                        {
                            cmdText = "SELECT f_PIN FROM t_b_Consumer WHERE f_PIN = " + PIN +
                                " AND f_ConsumerID <> " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Close();
                                return -501;
                            }
                            reader.Close();
                        }
                        if (CardNO > 0L)
                        {
                            cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                            command.CommandText = cmdText;
                            num2 = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                            if (num2 > 0)
                            {
                                if (ConsumerID != num2)
                                {
                                    return -103;
                                }
                            }
                            else
                            {
                                cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                command.CommandText = cmdText;
                                num2 = command.ExecuteNonQuery();
                            }
                        }
                        cmdText = "BEGIN TRANSACTION";
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();
                        try
                        {
                            cmdText = "UPDATE t_b_Consumer SET f_ConsumerNO =" + wgTools.PrepareStr(str2) + 
                                ", f_ConsumerName =" + wgTools.PrepareStr(ConsumerName) + 
                                ", f_GroupID = " + GroupID.ToString() + 
                                ", f_AttendEnabled = " + AttendEnabled.ToString() + 
                                ", f_ShiftEnabled = " + num3.ToString() + 
                                ", f_DoorEnabled = " + DoorEnabled.ToString() + 
                                ", f_BeginYMD = " + wgTools.PrepareStr(BeginYMD, true, this.gYMDFormat) + 
                                ", f_EndYMD = " + wgTools.PrepareStr(EndYMD, true, this.gYMDFormat) + 
                                ", f_PIN=" + (PIN != "" ? PIN : "NULL") + 
                                ", f_CardNO = " + ((CardNO > 0L) ? CardNO.ToString() : "NULL") +
                                " WHERE f_ConsumerID =" + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();

                            // Update fingerprint templates
                            if (fp_templ != null)
                            {
                                cmdText = "DELETE FROM t_d_FpTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();

                                foreach (MjFpTempl e in fp_templ)
                                {
                                    cmdText = "INSERT INTO t_d_FpTempl (f_ConsumerID, f_Finger, f_Templ, f_Duress) VALUES (" +
                                        ConsumerID.ToString() + ", " + e.finger.ToString() + ", @templ, " + (e.duress ? "'1'" : "'0'") + ")";
                                    command.CommandText = cmdText;
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@templ", e.data);
                                    command.ExecuteNonQuery();
                                }
                            }

                            // Update face template
                            cmdText = "DELETE FROM t_d_FaceTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            if (face_templ != null)
                            {
                                cmdText = "INSERT INTO t_d_FaceTempl (f_ConsumerID, f_Templ) VALUES (" +
                                    ConsumerID.ToString() + ", @templ)";
                                command.CommandText = cmdText;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@templ", face_templ.data);
                                command.ExecuteNonQuery();
                            }

							// Mark changed
							cmdText = " UPDATE t_d_FaceTemplChanged SET f_Changed = 1";
							command.CommandText = cmdText;
							command.ExecuteNonQuery();

                            int num4 = 0;
                            if (CardNO > 0L)
                            {
                                cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                                command.CommandText = cmdText;
                                OleDbDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    num4++;
                                    if (num4 > 1)
                                    {
                                        break;
                                    }
                                }
                                reader.Close();
                                if (num4 <= 1)
                                {
                                    cmdText = "SELECT f_ConsumerID FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + CardNO.ToString();
                                    command.CommandText = cmdText;
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        num4++;
                                        if (num4 > 1)
                                        {
                                            break;
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            if (num4 > 1)
                            {
                                cmdText = "ROLLBACK TRANSACTION";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                                return num;
                            }
                            cmdText = "COMMIT TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            return 1;
                        }
                        catch
                        {
                            cmdText = "ROLLBACK TRANSACTION";
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                        }
                        return num;
                    }
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int editUserOtherInfo(int ConsumerID, string txtf_Title, string txtf_Culture, string txtf_Hometown, string txtf_Birthday, string txtf_Marriage, string txtf_JoinDate, string txtf_LeaveDate, string txtf_CertificateType, string txtf_CertificateID, string txtf_SocialInsuranceNo, string txtf_Addr, string txtf_Postcode, string txtf_Sex, string txtf_Nationality, string txtf_Religion, string txtf_EnglishName, string txtf_Mobile, string txtf_HomePhone, string txtf_Telephone, string txtf_Email, string txtf_Political, string txtf_CorporationName, string txtf_TechGrade, string txtf_Note)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.editUserOtherInfo_Acc(ConsumerID, txtf_Title, txtf_Culture, txtf_Hometown, txtf_Birthday, txtf_Marriage, txtf_JoinDate, txtf_LeaveDate, txtf_CertificateType, txtf_CertificateID, txtf_SocialInsuranceNo, txtf_Addr, txtf_Postcode, txtf_Sex, txtf_Nationality, txtf_Religion, txtf_EnglishName, txtf_Mobile, txtf_HomePhone, txtf_Telephone, txtf_Email, txtf_Political, txtf_CorporationName, txtf_TechGrade, txtf_Note);
            }
            int num = -9;
            try
            {
                string cmdText = " UPDATE t_b_Consumer_Other  SET  ";
                cmdText = ((((((((((((((((((((((((cmdText + "   f_Title                 = " + wgTools.PrepareStr(txtf_Title)) + "  , f_Culture               = " + wgTools.PrepareStr(txtf_Culture)) + "  , f_Hometown              = " + wgTools.PrepareStr(txtf_Hometown)) + "  , f_Birthday              = " + wgTools.PrepareStr(txtf_Birthday)) + "  , f_Marriage              = " + wgTools.PrepareStr(txtf_Marriage)) + "  , f_JoinDate              = " + wgTools.PrepareStr(txtf_JoinDate)) + "  , f_LeaveDate             = " + wgTools.PrepareStr(txtf_LeaveDate)) + "  , f_CertificateType       = " + wgTools.PrepareStr(txtf_CertificateType)) + "  , f_CertificateID         = " + wgTools.PrepareStr(txtf_CertificateID)) + "  , f_SocialInsuranceNo     = " + wgTools.PrepareStr(txtf_SocialInsuranceNo)) + "  , f_Addr                  = " + wgTools.PrepareStr(txtf_Addr)) + "  , f_Postcode              = " + wgTools.PrepareStr(txtf_Postcode)) + "  , f_Sex                   = " + wgTools.PrepareStr(txtf_Sex)) + "  , f_Nationality           = " + wgTools.PrepareStr(txtf_Nationality)) + "  , f_Religion              = " + wgTools.PrepareStr(txtf_Religion)) + "  , f_EnglishName           = " + wgTools.PrepareStr(txtf_EnglishName)) + "  , f_Mobile                = " + wgTools.PrepareStr(txtf_Mobile)) + "  , f_HomePhone             = " + wgTools.PrepareStr(txtf_HomePhone)) + "  , f_Telephone             = " + wgTools.PrepareStr(txtf_Telephone)) + "  , f_Email                 = " + wgTools.PrepareStr(txtf_Email)) + "  , f_Political             = " + wgTools.PrepareStr(txtf_Political)) + "  , f_CorporationName       = " + wgTools.PrepareStr(txtf_CorporationName)) + "  , f_TechGrade             = " + wgTools.PrepareStr(txtf_TechGrade)) + "  , f_Note                  = " + wgTools.PrepareStr(txtf_Note)) + " WHERE f_ConsumerID =" + ConsumerID.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int editUserOtherInfo_Acc(int ConsumerID, string txtf_Title, string txtf_Culture, string txtf_Hometown, string txtf_Birthday, string txtf_Marriage, string txtf_JoinDate, string txtf_LeaveDate, string txtf_CertificateType, string txtf_CertificateID, string txtf_SocialInsuranceNo, string txtf_Addr, string txtf_Postcode, string txtf_Sex, string txtf_Nationality, string txtf_Religion, string txtf_EnglishName, string txtf_Mobile, string txtf_HomePhone, string txtf_Telephone, string txtf_Email, string txtf_Political, string txtf_CorporationName, string txtf_TechGrade, string txtf_Note)
        {
            int num = -9;
            try
            {
                string cmdText = " UPDATE t_b_Consumer_Other  SET  ";
                cmdText = ((((((((((((((((((((((((cmdText + "   f_Title                 = " + wgTools.PrepareStr(txtf_Title)) + "  , f_Culture               = " + wgTools.PrepareStr(txtf_Culture)) + "  , f_Hometown              = " + wgTools.PrepareStr(txtf_Hometown)) + "  , f_Birthday              = " + wgTools.PrepareStr(txtf_Birthday)) + "  , f_Marriage              = " + wgTools.PrepareStr(txtf_Marriage)) + "  , f_JoinDate              = " + wgTools.PrepareStr(txtf_JoinDate)) + "  , f_LeaveDate             = " + wgTools.PrepareStr(txtf_LeaveDate)) + "  , f_CertificateType       = " + wgTools.PrepareStr(txtf_CertificateType)) + "  , f_CertificateID         = " + wgTools.PrepareStr(txtf_CertificateID)) + "  , f_SocialInsuranceNo     = " + wgTools.PrepareStr(txtf_SocialInsuranceNo)) + "  , f_Addr                  = " + wgTools.PrepareStr(txtf_Addr)) + "  , f_Postcode              = " + wgTools.PrepareStr(txtf_Postcode)) + "  , f_Sex                   = " + wgTools.PrepareStr(txtf_Sex)) + "  , f_Nationality           = " + wgTools.PrepareStr(txtf_Nationality)) + "  , f_Religion              = " + wgTools.PrepareStr(txtf_Religion)) + "  , f_EnglishName           = " + wgTools.PrepareStr(txtf_EnglishName)) + "  , f_Mobile                = " + wgTools.PrepareStr(txtf_Mobile)) + "  , f_HomePhone             = " + wgTools.PrepareStr(txtf_HomePhone)) + "  , f_Telephone             = " + wgTools.PrepareStr(txtf_Telephone)) + "  , f_Email                 = " + wgTools.PrepareStr(txtf_Email)) + "  , f_Political             = " + wgTools.PrepareStr(txtf_Political)) + "  , f_CorporationName       = " + wgTools.PrepareStr(txtf_CorporationName)) + "  , f_TechGrade             = " + wgTools.PrepareStr(txtf_TechGrade)) + "  , f_Note                  = " + wgTools.PrepareStr(txtf_Note)) + " WHERE f_ConsumerID =" + ConsumerID.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public static string getErrInfo(int ErrNO)
        {
            string str = "";
            switch (ErrNO)
            {
                case -104:
                    return CommonStr.strCardNotExisted;

                case -103:
                    return CommonStr.strCardAlreadyUsed;

                case -9:
                    return (CommonStr.strOperateFailed + "  (E=" + ErrNO.ToString() + ")");

                case 0:
                    return str;

                case -901:
                    return CommonStr.strDBConNotCreate;

                case -401:
                    return CommonStr.strConsumerNOWrong;

                case -501:
                    return CommonStr.strConsumerWrongPin;

                case -201:
                    return CommonStr.strConsumerNameWrong;
            }
            if (ErrNO < 0)
            {
                str = CommonStr.strOperateFailed + "  (E=" + ErrNO.ToString() + ")";
            }
            return str;
        }

        public bool isExisted(long CardNO)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.isExisted_Acc(CardNO);
            }
            bool flag = false;
            try
            {
                string cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return true;
                        }
                    }
                    return flag;
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return flag;
        }

        public bool isExisted_Acc(long CardNO)
        {
            bool flag = false;
            try
            {
                string cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_CardNO]= " + CardNO.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return true;
                        }
                    }
                    return flag;
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return flag;
        }

        public bool isExistPassword(int password)
        {
            if (wgAppConfig.IsAccessDB)
                return this.isExistPassword_Acc(password);

            bool flag = false;
            try
            {
                string cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE [f_PIN]= " + password.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return true;
                        }
                    }
                    return flag;
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return flag;
        }

        public bool isExistPassword_Acc(int password)
        {
            bool flag = false;
            try
            {
                string cmdText = "SELECT f_ConsumerID From t_b_Consumer WHERE  [f_PIN]= " + password.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar())) > 0)
                        {
                            return true;
                        }
                    }
                    return flag;
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return flag;
        }

        public int registerLostCard(int ConsumerID, long NewCardNO)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.registerLostCard_Acc(ConsumerID, NewCardNO);
            }
            int num = -9;
            try
            {
                string cmdText = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.CommandText = cmdText;
                        long num2 = long.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                        if (num2 <= 0L)
                        {
                            num = -104;
                        }
                        else
                        {
                            cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num2.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES( ";
                            cmdText = (cmdText + ConsumerID.ToString() + "," + num2.ToString()) + ")";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                        }
                        if (NewCardNO <= 0L)
                        {
                            cmdText = "Update t_b_Consumer SET f_CardNO = NULL WHERE [f_ConsumerID]= " + ConsumerID.ToString();
                        }
                        else
                        {
                            cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + NewCardNO.ToString();
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            cmdText = "Update t_b_Consumer SET f_CardNO = " + NewCardNO.ToString() + " WHERE [f_ConsumerID]= " + ConsumerID.ToString();
                        }
                        command.CommandText = cmdText;
                        num2 = command.ExecuteNonQuery();
                        if (num2 >= 0L)
                        {
                            num = (int) num2;
                        }
                    }
                    return num;
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public int registerLostCard_Acc(int ConsumerID, long NewCardNO)
        {
            int num = -9;
            try
            {
                string cmdText = "SELECT f_CardNO FROM t_b_Consumer WHERE [f_ConsumerID]= " + ConsumerID.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.CommandText = cmdText;
                        long num2 = long.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                        if (num2 <= 0L)
                        {
                            num = -104;
                        }
                        else
                        {
                            cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + num2.ToString();
                            command.CommandText = cmdText;
                            command.ExecuteNonQuery();
                            cmdText = "INSERT INTO t_b_IDCard_Lost ([f_ConsumerID], [f_CardNO]) VALUES( ";
                            cmdText = (cmdText + ConsumerID.ToString() + "," + num2.ToString()) + ")";
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                        }
                        if (NewCardNO <= 0L)
                        {
                            cmdText = "Update t_b_Consumer SET f_CardNO = NULL WHERE [f_ConsumerID]= " + ConsumerID.ToString();
                        }
                        else
                        {
                            cmdText = "DELETE FROM t_b_IDCard_Lost WHERE [f_CardNO]= " + NewCardNO.ToString();
                            command.CommandText = cmdText;
                            num2 = command.ExecuteNonQuery();
                            cmdText = "Update t_b_Consumer SET f_CardNO = " + NewCardNO.ToString() + " WHERE [f_ConsumerID]= " + ConsumerID.ToString();
                        }
                        command.CommandText = cmdText;
                        num2 = command.ExecuteNonQuery();
                        if (num2 >= 0L)
                        {
                            num = (int) num2;
                        }
                    }
                    return num;
                }
            }
            catch (Exception exception)
            {
                this.wgDebugWrite(exception.ToString());
            }
            return num;
        }

        public void wgDebugWrite(string info)
        {
        }
    }
}

