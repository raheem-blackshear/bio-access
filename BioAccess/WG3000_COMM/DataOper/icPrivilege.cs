namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Collections;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;

    internal class icPrivilege : wgMjControllerPrivilege
    {
        private const uint CARDNOMAX = 0xfffffffe;
        private DataColumn dc;
        private DataTable dtPrivilege;
        private DataTable tbFpTempl;
        private DataTable tbFaceTempl;
        private DataTable dtUserFloor;
        private DataTable dtUserFloorCnt;
        private DataView dvUserFloorCnt;
        private int m_ConsumersTotal;
        private int m_PrivilegeTotal;
        private int m_ValidPrivilegeTotal;

        public icPrivilege()
        {
            base.AllowUpload();
            base.bAllowUploadUserName = false;
            int result = 0;
            try
            {
                int.TryParse(wgAppConfig.GetKeyVal("AllowUploadUserName"), out result);
            }
            catch (Exception)
            {
            }
            if (result > 0)
            {
                base.bAllowUploadUserName = true;
            }
            if (this.dtPrivilege == null)
            {
                this.dtPrivilege = new DataTable("Privilege");
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.UInt32");
                this.dc.ColumnName = "f_ConsumerID";
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.UInt32");
                this.dc.ColumnName = "f_ConsumerNO";
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.String");
                this.dc.ColumnName = "f_CardNO";
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_BeginYMD";
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.DateTime");
                this.dc.ColumnName = "f_EndYMD";
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.String");
                this.dc.ColumnName = "f_PIN";
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_ControlSegID1";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_ControlSegID2";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_ControlSegID3";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_ControlSegID4";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_DoorFirstCard_1";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_DoorFirstCard_2";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_DoorFirstCard_3";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_DoorFirstCard_4";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_MoreCards_GrpID_1";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_MoreCards_GrpID_2";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_MoreCards_GrpID_3";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_MoreCards_GrpID_4";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.UInt32");
                this.dc.ColumnName = "f_MaxSwipe";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.Byte");
                this.dc.ColumnName = "f_IsSuperCard";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.UInt64");
                this.dc.ColumnName = "f_AllowFloors";
                this.dc.DefaultValue = 0;
                this.dtPrivilege.Columns.Add(this.dc);
                this.dc = new DataColumn();
                this.dc.DataType = Type.GetType("System.String");
                this.dc.ColumnName = "f_ConsumerName";
                this.dtPrivilege.Columns.Add(this.dc);
            }

            if (tbFpTempl == null)
            {
                tbFpTempl = new DataTable("FpTempl");
                tbFpTempl.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                tbFpTempl.Columns.Add("f_Finger", System.Type.GetType("System.Int32"));
                tbFpTempl.Columns.Add("f_Templ", System.Type.GetType("System.Byte[]"));
                tbFpTempl.Columns.Add("f_Duress", System.Type.GetType("System.Int32"));
            }

            if (tbFaceTempl == null)
            {
                tbFaceTempl = new DataTable("FaceTempl");
                tbFaceTempl.Columns.Add("f_ConsumerNO", System.Type.GetType("System.UInt32"));
                tbFaceTempl.Columns.Add("f_Templ", System.Type.GetType("System.Byte[]"));
            }
        }

        public int AddPrivilegeOfOneCardByDB(int ControllerID, int ConsumerID)
        {
            if (wgAppConfig.IsAccessDB)
                return this.AddPrivilegeOfOneCardByDB_Acc(ControllerID, ConsumerID);

            int num = -1;
            if (wgMjController.isS8000DC(ControllerID))
            {
                string cmdText = " SELECT f_ConsumerNO, f_CardNO, f_GroupID FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjRegisterCard mjrc = new MjRegisterCard();
                uint result = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        int groupID = -1;
                        byte seg, doorID;

                        if (reader.Read())
                        {
                            uint.TryParse(reader["f_CardNO"].ToString(), out result);
                            groupID = (int)reader["f_GroupID"];
                        }
                        reader.Close();
                        if (result <= 0L || result > 0xfffffffeL)
                            return 1;

                        //////////////////////////////////////////////////////////////////////////
                        // PC Check Group
                        ArrayList doorList = new ArrayList();
                        ArrayList doorNoList = new ArrayList();

                        if (wgAppConfig.getParamValBoolByNO(0x89))
                        {
                            command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                            reader = command.ExecuteReader();
                            if (!reader.Read())
                            {
                                reader.Close();
                                goto l_ExitPCCheck;
                            }
                            reader.Close();
                            command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string doors = reader["f_SoundFileName"].ToString();
                                if (doors == "")
                                {
                                    for (doorID = 1; doorID <= 4; doorID++)
                                        doorNoList.Add(doorID);
                                    reader.Close();
                                    goto l_ExitPCCheck;
                                }
                                int comma;
                                while (true)
                                {
                                    comma = doors.IndexOf(",");
                                    if (comma != -1)
                                    {
                                        doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                        doors = doors.Substring(comma + 1);
                                    }
                                    else
                                    {
                                        doorList.Add(Convert.ToInt32(doors));
                                        break;
                                    }
                                }
                            }
                            reader.Close();
                            if (doorList.Count > 0)
                            {
                                foreach (int e in doorList)
                                {
                                    command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                        " AND f_ControllerID = " + ControllerID.ToString();
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        doorNoList.Add((byte)reader["f_DoorNO"]);
                                    reader.Close();
                                }
                            }
                        }
                    l_ExitPCCheck:
                        cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID, " +
                            "t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL AND f_ControllerID =  " + ControllerID.ToString() + " AND f_CardNO =  " + result.ToString() + 
                            " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            mjrc.CardID = (uint)uint.Parse(reader["f_CardNO"].ToString());
                            mjrc.userID = uint.Parse(reader["f_ConsumerNO"].ToString());
                            mjrc.Password = uint.Parse(reader["f_PIN"].ToString());
                            mjrc.ymdStart = (DateTime)reader["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader["f_EndYMD"];

                            doorID = byte.Parse(reader["f_DoorNO"].ToString());
                            seg = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                            mjrc.ControlSegIndexSet(doorID, seg);
                            if (doorNoList.Count > 0)
                            {
                                foreach (byte e in doorNoList)
                                {
                                    if (e == doorID)
                                    {
                                        mjrc.ControlSegIndexSet(e, (byte)(seg | 0x80));
                                        break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from ( t_d_doorFirstCardUsers a inner join t_b_door b on (a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte)reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader.Close();
                        cmdText = ((" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from t_d_doorMoreCardsUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte)reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader.Close();
                    }
                }
                if (mjrc.CardID > 0)
                    num = this.AddPrivilegeOfOneCardByDB(ControllerID, mjrc);
            }
            else
            {
                string cmdText = " SELECT f_ConsumerNO, f_GroupID ";
                cmdText = cmdText + " FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjUserInfo mjrc = new MjUserInfo();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        int userID = 0, groupID = -1;
                        byte seg, doorID;

                        if (reader.Read())
                        {
                            userID = int.Parse(reader["f_ConsumerNO"].ToString());
                            groupID = (int)reader["f_GroupID"];
                        }
                        reader.Close();
                        if (userID == 0)
                            return 1;

                        //////////////////////////////////////////////////////////////////////////
                        // PC Check Group
                        ArrayList doorList = new ArrayList();
                        ArrayList doorNoList = new ArrayList();

                        if (wgAppConfig.getParamValBoolByNO(0x89))
                        {
                            command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                            reader = command.ExecuteReader();
                            if (!reader.Read())
                            {
                                reader.Close();
                                goto l_ExitPCCheck1;
                            }
                            reader.Close();
                            command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string doors = reader["f_SoundFileName"].ToString();
                                if (doors == "")
                                {
                                    for (doorID = 1; doorID <= 4; doorID++)
                                        doorNoList.Add(doorID);
                                    reader.Close();
                                    goto l_ExitPCCheck1;
                                }
                                int comma;
                                while (true)
                                {
                                    comma = doors.IndexOf(",");
                                    if (comma != -1)
                                    {
                                        doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                        doors = doors.Substring(comma + 1);
                                    }
                                    else
                                    {
                                        doorList.Add(Convert.ToInt32(doors));
                                        break;
                                    }
                                }
                            }
                            reader.Close();
                            if (doorList.Count > 0)
                            {
                                foreach (int e in doorList)
                                {
                                    command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                        " AND f_ControllerID = " + ControllerID.ToString();
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        doorNoList.Add((byte)reader["f_DoorNO"]);
                                    reader.Close();
                                }
                            }
                        }
                    l_ExitPCCheck1:
                        cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID, " +
                            "t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL AND f_ControllerID = " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerNO = '" + userID.ToString() + "' " + 
                            "AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
					    string card, pin;
                        while (reader.Read())
                        {
                            mjrc.userID = uint.Parse(reader["f_ConsumerNO"].ToString());
						    card = reader["f_CardNO"].ToString();
						    mjrc.cardID = (card == "") ? 0 : long.Parse(card);
                            pin = reader["f_PIN"].ToString();
                            mjrc.password = (pin == "") ? 0 : uint.Parse(pin);
                            mjrc.ymdStart = (DateTime) reader["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime) reader["f_EndYMD"];

                            doorID = byte.Parse(reader["f_DoorNO"].ToString());
                            seg = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                            mjrc.ControlSegIndexSet(doorID, seg);
                            if (doorNoList.Count > 0)
                            {
                                foreach (byte e in doorNoList)
                                {
                                    if (e == doorID)
                                    {
                                        mjrc.ControlSegIndexSet(e, (byte)(seg | 0x80));
                                        break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from ( t_d_doorFirstCardUsers a inner join t_b_door b on (a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte) reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader.Close();
                        cmdText = ((" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from t_d_doorMoreCardsUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte) reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte) ((int) reader["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte) ((int) reader["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte) ((int) reader["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte) ((int) reader["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader.Close();

                        if (mjrc.cardID != 0) mjrc.hasCard();
                        if (mjrc.password != 0) mjrc.hasPassword();
                        mjrc.accessAllowed();

                        // Load fingerprint templates
                        command.CommandText = " SELECT f_Finger, f_Templ, f_Duress FROM t_d_FpTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
                        reader = command.ExecuteReader(CommandBehavior.Default);
                        while (reader.Read())
                        {
                            MjFpTempl e = new MjFpTempl(
                                ConsumerID,
                                (int)reader["f_Finger"],
                                (byte[])reader["f_Templ"],
                                (int)reader["f_Duress"] > 0);
                            mjrc.fpTemplList.Add(e);
                            mjrc.hasFingerprint((byte)e.finger);
                            if (e.duress) mjrc.hasDuress((byte)e.finger);
                        }
                        reader.Close();

                        // Load fingerprint templates
                        command.CommandText = " SELECT f_Templ FROM t_d_FaceTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
                        reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                        {
                            mjrc.faceTempl = new MjFaceTempl(
                                ConsumerID,
                                (byte[])reader["f_Templ"]);
                            mjrc.hasFace();
                        }
                        reader.Close();
                    }
                }
                if (mjrc.userID > 0)
                    num = this.AddPrivilegeOfOneCardByDB(ControllerID, mjrc);
            }
            return num;
        }

        public int AddPrivilegeOfOneCardByDB(int ControllerID, MjUserInfo mjrc)
        {
            if (wgAppConfig.IsAccessDB)
                return this.AddPrivilegeOfOneCardByDB_Acc(ControllerID, mjrc);
            
			int num = -1;
            string cmdText = " SELECT * ";
            cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = base.AddPrivilegeOfOneCardIP((int) reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int) reader["f_PORT"], mjrc);
                    }
                    reader.Close();
                }
            }
            return num;
        }
		
        public int AddPrivilegeOfOneCardByDB(int ControllerID, MjRegisterCard mjrc)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.AddPrivilegeOfOneCardByDB_Acc(ControllerID, mjrc);
            }
            int num = -1;
            string cmdText = " SELECT * ";
            cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = base.AddPrivilegeOfOneCardIP((int) reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int) reader["f_PORT"], mjrc);
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public int AddPrivilegeOfOneCardByDB_Acc(int ControllerID, int ConsumerID)
        {
            int num = -1;
            if (wgMjController.isS8000DC(ControllerID))
            {
                string cmdText = " SELECT f_ConsumerNO, f_GroupID, f_CardNO ";
                cmdText = cmdText + " FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjRegisterCard mjrc = new MjRegisterCard();
                uint result = 0;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        int groupID = -1;
                        byte seg, doorID;

                        if (reader.Read())
                        {
                            uint.TryParse(reader["f_CardNO"].ToString(), out result);
                            groupID = (int)reader["f_GroupID"];
                        }
                        reader.Close();
                        if (result <= 0L || result > 0xfffffffeL)
                            return 1;

                        //////////////////////////////////////////////////////////////////////////
                        // PC Check Group
                        ArrayList doorList = new ArrayList();
                        ArrayList doorNoList = new ArrayList();

                        if (wgAppConfig.getParamValBoolByNO(0x89))
                        {
                            command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                            reader = command.ExecuteReader();
                            if (!reader.Read())
                            {
                                reader.Close();
                                goto l_ExitPCCheck;
                            }
                            reader.Close();
                            command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string doors = reader["f_SoundFileName"].ToString();
                                if (doors == "")
                                {
                                    for (doorID = 1; doorID <= 4; doorID++)
                                        doorNoList.Add(doorID);
                                    reader.Close();
                                    goto l_ExitPCCheck;
                                }
                                int comma;
                                while (true)
                                {
                                    comma = doors.IndexOf(",");
                                    if (comma != -1)
                                    {
                                        doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                        doors = doors.Substring(comma + 1);
                                    }
                                    else
                                    {
                                        doorList.Add(Convert.ToInt32(doors));
                                        break;
                                    }
                                }
                            }
                            reader.Close();
                            if (doorList.Count > 0)
                            {
                                foreach (int e in doorList)
                                {
                                    command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                        " AND f_ControllerID = " + ControllerID.ToString();
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        doorNoList.Add((byte)reader["f_DoorNO"]);
                                    reader.Close();
                                }
                            }
                        }
                    l_ExitPCCheck:
                        cmdText = " SELECT t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID, " +
                            "t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL AND f_ControllerID = " + ControllerID.ToString() + " AND f_CardNO = " + result.ToString() + 
                            " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        string card, pin;
                        while (reader.Read())
                        {
                            mjrc.userID = uint.Parse(reader["f_ConsumerNO"].ToString());
                            card = reader["f_CardNO"].ToString();
                            mjrc.CardID = (card == "") ? 0 : uint.Parse(card);
                            pin = reader["f_PIN"].ToString();
                            mjrc.Password = (pin == "") ? 0 : uint.Parse(pin);
                            mjrc.ymdStart = (DateTime)reader["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader["f_EndYMD"];

                            doorID = byte.Parse(reader["f_DoorNO"].ToString());
                            seg = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                            mjrc.ControlSegIndexSet(doorID, seg);
                            if (doorNoList.Count > 0)
                            {
                                foreach (byte e in doorNoList)
                                {
                                    if (e == doorID)
                                    {
                                        mjrc.ControlSegIndexSet(e, (byte)(seg | 0x80));
                                        break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from (t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID =  {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte)reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from (t_d_doorMoreCardsUsers a inner join t_b_door b on (a.f_doorid = b.f_doorid and f_ControllerID = {0})) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte)reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader.Close();
                    }
                }
                if (mjrc.CardID > 0)
                    num = this.AddPrivilegeOfOneCardByDB(ControllerID, mjrc);
            }
            else
            {
                string cmdText = " SELECT f_ConsumerNO, f_GroupID ";
                cmdText = cmdText + " FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjUserInfo mjrc = new MjUserInfo();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        int userID = 0, groupID = -1;
                        byte seg, doorID;

                        if (reader.Read())
                        {
                            userID = int.Parse(reader["f_ConsumerNO"].ToString());
                            groupID = (int)reader["f_GroupID"];
                        }
                        reader.Close();
                        if (userID == 0)
                            return 1;

                        //////////////////////////////////////////////////////////////////////////
                        // PC Check Group
                        ArrayList doorList = new ArrayList();
                        ArrayList doorNoList = new ArrayList();

                        if (wgAppConfig.getParamValBoolByNO(0x89))
                        {
                            command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                            reader = command.ExecuteReader();
                            if (!reader.Read())
                            {
                                reader.Close();
                                goto l_ExitPCCheck1;
                            }
                            reader.Close();
                            command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string doors = reader["f_SoundFileName"].ToString();
                                if (doors == "")
                                {
                                    for (doorID = 1; doorID <= 4; doorID++)
                                        doorNoList.Add(doorID);
                                    reader.Close();
                                    goto l_ExitPCCheck1;
                                }
                                int comma;
                                while (true)
                                {
                                    comma = doors.IndexOf(",");
                                    if (comma != -1)
                                    {
                                        doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                        doors = doors.Substring(comma + 1);
                                    }
                                    else
                                    {
                                        doorList.Add(Convert.ToInt32(doors));
                                        break;
                                    }
                                }
                            }
                            reader.Close();
                            if (doorList.Count > 0)
                            {
                                foreach (int e in doorList)
                                {
                                    command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                        " AND f_ControllerID = " + ControllerID.ToString();
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        doorNoList.Add((byte)reader["f_DoorNO"]);
                                    reader.Close();
                                }
                            }
                        }
                    l_ExitPCCheck1:
                        cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID, " +
                            "t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL AND f_ControllerID = " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerNO = '" + userID.ToString() + "' " +
                            " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        string card, pin;
                        while (reader.Read())
                        {
                            mjrc.userID = uint.Parse(reader["f_ConsumerNO"].ToString());
                            card = reader["f_CardNO"].ToString();
                            mjrc.cardID = (card == "") ? 0 : long.Parse(card);
                            pin = reader["f_PIN"].ToString();
                            mjrc.password = (pin == "") ? 0 : uint.Parse(pin);
                            mjrc.ymdStart = (DateTime)reader["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader["f_EndYMD"];

                            doorID = byte.Parse(reader["f_DoorNO"].ToString());
                            seg = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                            mjrc.ControlSegIndexSet(doorID, seg);
                            if (doorNoList.Count > 0)
                            {
                                foreach (byte e in doorNoList)
                                {
                                    if (e == doorID)
                                    {
                                        mjrc.ControlSegIndexSet(e, (byte)(seg | 0x80));
                                        break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from (t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID =  {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte)reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from (t_d_doorMoreCardsUsers a inner join t_b_door b on (a.f_doorid = b.f_doorid and f_ControllerID = {0})) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((byte)reader["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader.Close();

                        if (mjrc.cardID != 0) mjrc.hasCard();
                        if (mjrc.password != 0) mjrc.hasPassword();
                        mjrc.accessAllowed();

                        // Load fingerprint templates
                        command.CommandText = " SELECT f_Finger, f_Templ, f_Duress FROM t_d_FpTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
                        reader = command.ExecuteReader(CommandBehavior.Default);

                        while (reader.Read())
                        {
                            MjFpTempl e = new MjFpTempl(
                                ConsumerID,
                                (int)reader["f_Finger"],
                                (byte[])reader["f_Templ"],
                                (int)reader["f_Duress"] > 0);
                            mjrc.fpTemplList.Add(e);
                            mjrc.hasFingerprint((byte)e.finger);
                            if (e.duress) mjrc.hasDuress((byte)e.finger);
                        }
                        reader.Close();

                        // Load face template
                        command.CommandText = " SELECT f_Templ FROM t_d_FaceTempl WHERE f_ConsumerID = " + ConsumerID.ToString();
                        reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                        {
                            mjrc.faceTempl = new MjFaceTempl(
                                ConsumerID,
                                (byte[])reader["f_Templ"]);
                            mjrc.hasFace();
                        }
                        reader.Close();
                    }
                }
                if (mjrc.userID > 0)
                    num = this.AddPrivilegeOfOneCardByDB(ControllerID, mjrc);
            }
            return num;
        }

        public int AddPrivilegeOfOneCardByDB_Acc(int ControllerID, MjUserInfo mjrc)
        {
            int num = -1;
            string cmdText = " SELECT * ";
            cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = base.AddPrivilegeOfOneCardIP((int) reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int) reader["f_PORT"], mjrc);
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public int AddPrivilegeOfOneCardByDB_Acc(int ControllerID, MjRegisterCard mjrc)
        {
            int num = -1;
            string cmdText = " SELECT * ";
            cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = base.AddPrivilegeOfOneCardIP((int)reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int)reader["f_PORT"], mjrc);
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public int DelPrivilegeOfOneCardByDB(int ControllerID, int ConsumerID)
        {
            if (wgAppConfig.IsAccessDB)
                return this.DelPrivilegeOfOneCardByDB_Acc(ControllerID, ConsumerID);

            int num = -1;
            if (wgMjController.isS8000DC(ControllerID))
            {
                string cmdText = " SELECT f_ConsumerNO, f_CardNO FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjRegisterCard mjrc = new MjRegisterCard();
                uint result = 0, uid = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            uint.TryParse(reader["f_CardNO"].ToString(), out result);
                            uid = uint.Parse(reader["f_ConsumerNO"].ToString());
                        }
                        reader.Close();
                    }
                }
                if (result <= 0 || result > 0xfffffffeL || uid == 0)
                    return 1;

                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID, " +
                    " t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL AND f_ControllerID = " + ControllerID.ToString() + " AND f_CardNO = " + result.ToString() + 
                    " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        SqlDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            mjrc.CardID = uint.Parse(reader2["f_CardNO"].ToString());
                            mjrc.userID = uint.Parse(reader2["f_ConsumerNO"].ToString());
                            mjrc.Password = uint.Parse(reader2["f_PIN"].ToString());
                            mjrc.ymdStart = (DateTime)reader2["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader2["f_EndYMD"];
                            switch (int.Parse(reader2["f_DoorNO"].ToString()))
                            {
                                case 1:
                                    mjrc.ControlSegIndexSet(1, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 2:
                                    mjrc.ControlSegIndexSet(2, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 3:
                                    mjrc.ControlSegIndexSet(3, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 4:
                                    mjrc.ControlSegIndexSet(4, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = ((" SELECT a.f_ConsumerID, b.f_DoorNO from t_d_doorFirstCardUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = ((" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from t_d_doorMoreCardsUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = " SELECT * FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        this.dtPrivilege.NewRow();
                        if (reader2.Read())
                        {
                            if (mjrc.CardID > 0)
                                num = base.AddPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], mjrc);
                            else
                                num = base.DelPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], result);
                        }
                        reader2.Close();
                    }
                }
            }
            else
            {
                string cmdText = " SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjUserInfo mjrc = new MjUserInfo();
                uint uid = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                            uid = uint.Parse(reader["f_ConsumerNO"].ToString());
                        reader.Close();
                    }
                }
                if (uid == 0)
                    return 1;

                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID, " +
                    " t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL AND f_ControllerID = " + ControllerID.ToString() + " AND f_ConsumerNO = '" + uid.ToString() + 
                    "' AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        SqlDataReader reader2 = command2.ExecuteReader();
                        string card;
                        while (reader2.Read())
                        {
                            mjrc.userID = uint.Parse(reader2["f_ConsumerNO"].ToString());
                            card = reader2["f_CardNO"].ToString();
                            mjrc.cardID = (card == "") ? 0 : long.Parse(card);
                            mjrc.password = uint.Parse(reader2["f_PIN"].ToString());
                            mjrc.ymdStart = (DateTime)reader2["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader2["f_EndYMD"];
                            switch (int.Parse(reader2["f_DoorNO"].ToString()))
                            {
                                case 1:
                                    mjrc.ControlSegIndexSet(1, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 2:
                                    mjrc.ControlSegIndexSet(2, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 3:
                                    mjrc.ControlSegIndexSet(3, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 4:
                                    mjrc.ControlSegIndexSet(4, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = ((" SELECT a.f_ConsumerID, b.f_DoorNO from t_d_doorFirstCardUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = ((" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from t_d_doorMoreCardsUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = " SELECT * FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        this.dtPrivilege.NewRow();
                        if (reader2.Read())
                        {
                            if (mjrc.userID > 0)
                                num = base.AddPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], mjrc);
                            else
                                num = base.DelPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], uid);
                        }
                        reader2.Close();
                    }
                }
            }
            return num;
        }

        public int DelPrivilegeOfOneCardByDB_Acc(int ControllerID, int ConsumerID)
        {
            int num = -1;

            if (wgMjController.isS8000DC(ControllerID))
            {
                string cmdText = " SELECT f_ConsumerNO, f_CardNO FROM t_b_Consumer WHERE f_ConsumerID = " + ConsumerID.ToString();
                MjRegisterCard mjrc = new MjRegisterCard();
                uint result = 0, uid = 0;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            uint.TryParse(reader["f_CardNO"].ToString(), out result);
                            uid = uint.Parse(reader["f_ConsumerNO"].ToString());
                        }
                        reader.Close();
                    }
                }
                if (result <= 0 || result > 0xfffffffeL || uid == 0)
                    return 1;

                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID," +
                    " t_b_Consumer.f_ConsumerName FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL AND f_ControllerID = " + ControllerID.ToString() + " AND f_CardNO =  " + result.ToString() + 
                    " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
                using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command2 = new OleDbCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            mjrc.CardID = uint.Parse(reader2["f_CardNO"].ToString());
                            mjrc.userID = uint.Parse(reader2["f_ConsumerNO"].ToString());
                            mjrc.Password = uint.Parse(reader2["f_PIN"].ToString());
                            mjrc.ymdStart = (DateTime)reader2["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader2["f_EndYMD"];
                            switch (int.Parse(reader2["f_DoorNO"].ToString()))
                            {
                                case 1:
                                    mjrc.ControlSegIndexSet(1, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 2:
                                    mjrc.ControlSegIndexSet(2, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 3:
                                    mjrc.ControlSegIndexSet(3, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 4:
                                    mjrc.ControlSegIndexSet(4, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from (t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, a.f_MoreCards_GrpID, b.f_DoorNO from ( t_d_doorMoreCardsUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0})) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        this.dtPrivilege.NewRow();
                        if (reader2.Read())
                        {
                            if (mjrc.CardID > 0)
                                num = base.AddPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], mjrc);
                            else
                                num = base.DelPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], result);
                        }
                        reader2.Close();
                    }
                }
            }
            else
            {
                string cmdText = " SELECT f_ConsumerNO FROM t_b_Consumer WHERE f_ConsumerID =  " + ConsumerID.ToString();
                MjUserInfo mjrc = new MjUserInfo();
                uint uid = 0;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                            uid = uint.Parse(reader["f_ConsumerNO"].ToString());
                        reader.Close();
                    }
                }
                if (uid == 0)
                    return 1;
                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID ";
                cmdText = ((((cmdText + " , t_b_Consumer.f_ConsumerName ") + " FROM t_b_Consumer ,t_d_Privilege " + " WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL ") + " AND f_ControllerID =  " + ControllerID.ToString()) + " AND f_ConsumerNO = '" + uid.ToString()) + "' AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
                using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command2 = new OleDbCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        string card;
                        while (reader2.Read())
                        {
                            mjrc.userID = uint.Parse(reader2["f_ConsumerNO"].ToString());
                            card = reader2["f_CardNO"].ToString();
                            mjrc.cardID = (card == "") ? 0 : long.Parse(card);
                            mjrc.password = uint.Parse(reader2["f_PIN"].ToString());
                            mjrc.ymdStart = (DateTime)reader2["f_BeginYMD"];
                            mjrc.ymdEnd = (DateTime)reader2["f_EndYMD"];
                            switch (int.Parse(reader2["f_DoorNO"].ToString()))
                            {
                                case 1:
                                    mjrc.ControlSegIndexSet(1, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 2:
                                    mjrc.ControlSegIndexSet(2, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 3:
                                    mjrc.ControlSegIndexSet(3, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;

                                case 4:
                                    mjrc.ControlSegIndexSet(4, (byte)(((int)reader2["f_ControlSegID"]) & 0xff));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from (t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.FirstCardSet(1, true);
                                    break;

                                case 2:
                                    mjrc.FirstCardSet(2, true);
                                    break;

                                case 3:
                                    mjrc.FirstCardSet(3, true);
                                    break;

                                case 4:
                                    mjrc.FirstCardSet(4, true);
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = (string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from ( t_d_doorMoreCardsUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0})) ", ControllerID.ToString()) + " WHERE a.f_ConsumerID =  " + ConsumerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            switch (((byte)reader2["f_DoorNO"]))
                            {
                                case 1:
                                    mjrc.MoreCardGroupIndexSet(1, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 2:
                                    mjrc.MoreCardGroupIndexSet(2, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 3:
                                    mjrc.MoreCardGroupIndexSet(3, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;

                                case 4:
                                    mjrc.MoreCardGroupIndexSet(4, (byte)((int)reader2["f_MoreCards_GrpID"]));
                                    break;
                            }
                        }
                        reader2.Close();
                        cmdText = " SELECT * ";
                        cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerID =  " + ControllerID.ToString();
                        command2.CommandText = cmdText;
                        reader2 = command2.ExecuteReader();
                        this.dtPrivilege.NewRow();
                        if (reader2.Read())
                        {
                            if (mjrc.userID > 0)
                                num = base.AddPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], mjrc);
                            else
                                num = base.DelPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], uid);
                        }
                        reader2.Close();
                    }
                }
            }
            return num;
        }

        protected override void DisplayProcessInfo(string info, int infoCode, int specialInfo)
        {
            switch (infoCode)
            {
                case 0x186a1:
                    wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", info, CommonStr.strUploadPreparing));
                    return;

                case 0x186a2:
                    wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, CommonStr.strUploadingPrivileges, specialInfo));
                    return;

                case 0x186a3:
                    wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, CommonStr.strUploadedPrivileges, specialInfo));
                    return;

                case -100001:
                    wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", info, wgTools.gADCT ? CommonStr.strUploadFail_200K : CommonStr.strUploadFail_40K, specialInfo));
                    return;
            }
            wgAppRunInfo.raiseAppRunInfoCommStatus(info);
        }

        public int getControllerIDByDoorName(string DoorName)
        {
            if (wgAppConfig.IsAccessDB)
                return this.getControllerIDByDoorName_Acc(DoorName);
            
            int num = 0;
            string cmdText = " SELECT f_ControllerID ";
            cmdText = cmdText + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStr(DoorName);
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

        public int getControllerIDByDoorName_Acc(string DoorName)
        {
            int num = 0;
            string cmdText = " SELECT f_ControllerID ";
            cmdText = cmdText + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStr(DoorName);
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

        public int getControllerSNByDoorName(string DoorName)
        {
            if (wgAppConfig.IsAccessDB)
                return this.getControllerSNByDoorName_Acc(DoorName);
            
            int num = 0;
            string cmdText = " SELECT f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Door a,t_b_Controller b WHERE a.f_ControllerID = b.f_ControllerID AND f_DoorName =  " + wgTools.PrepareStr(DoorName);
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

        public int getControllerSNByDoorName_Acc(string DoorName)
        {
            int num = 0;
            string cmdText = " SELECT f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Door a,t_b_Controller b WHERE a.f_ControllerID = b.f_ControllerID AND f_DoorName =  " + wgTools.PrepareStr(DoorName);
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

        public int getControllerSNByID(int ControllerID)
        {
            if (wgAppConfig.IsAccessDB)
                return this.getControllerSNByID_Acc(ControllerID);
            
            int num = 0;
            string cmdText = " SELECT f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Controller b WHERE  b.f_ControllerID =  " + ControllerID;
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

        public int getControllerSNByID_Acc(int ControllerID)
        {
            int num = 0;
            string cmdText = " SELECT f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Controller b WHERE  b.f_ControllerID =  " + ControllerID;
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

        #region Elevator region
        public int getElevatorPrivilegeByID(int ControllerID)
        {
            if (wgAppConfig.IsAccessDB)
                return this.getElevatorPrivilegeByID_Acc(ControllerID);
            
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
            
            wgTools.WriteLine("getPrivilegeByID Start");
            this.dtPrivilege.Rows.Clear();
            string cmdText = string.Format(" SELECT a.f_ConsumerID, COUNT(a.f_FloorID) as cnt FROM t_b_UserFloor a\r\nWHERE a.f_FloorID IN \r\n(SELECT b.f_floorid from t_b_floor b where b.[f_ControllerID]={0} or b.[f_ControllerID] in \r\n (select c.f_ControllerID from t_b_ElevatorGroup c where c.f_ElevatorGroupNO in \r\n   (select d.f_ElevatorGroupNO from t_b_ElevatorGroup d where d.f_ControllerID = {0})))\r\nGROUP BY a.f_ConsumerID ", ControllerID);
            this.dtUserFloorCnt = new DataTable();
            this.dvUserFloorCnt = new DataView(this.dtUserFloorCnt);
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        adapter.Fill(this.dtUserFloorCnt);
                    }
                }
            }
            cmdText = string.Format("SELECT b.f_ConsumerNO, b.f_CardNO, b.f_ConsumerID, b.f_BeginYMD, b.f_EndYMD, b.f_PIN, a.f_ControlSegID, b.f_DoorEnabled, c.f_floorNO\r\nFROM [t_b_UserFloor] a\r\nINNER JOIN t_b_Consumer b ON  a.f_ConsumerID = b.f_ConsumerID AND b.f_ConsumerNO IS NOT NULL\r\nINNER JOIN t_b_Floor c ON a.f_FloorID = c.f_FloorID\r\nWHERE f_ControllerID = {0}\r\nORDER BY f_ConsumerNO", ControllerID);
            this.dtUserFloor = new DataTable();
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        adapter2.Fill(this.dtUserFloor);
                    }
                }
            }
            DataRow row = this.dtPrivilege.NewRow();
			row["f_ConsumerNO"] = 0;
            row["f_CardNO"] = 0;
            row["f_ControlSegID1"] = 0;
            row["f_ControlSegID2"] = 0;
            row["f_ControlSegID3"] = 0;
            row["f_ControlSegID4"] = 0;
            row["f_MoreCards_GrpID_3"] = 0;
            row["f_MoreCards_GrpID_4"] = 0;
            row["f_MoreCards_GrpID_2"] = 0;
            row["f_DoorFirstCard_2"] = 0;
            row["f_DoorFirstCard_3"] = 0;
            row["f_DoorFirstCard_4"] = 0;
            row["f_PIN"] = 0;
            int num = 0;
            for (int i = 0; i < this.dtUserFloor.Rows.Count; i++)
            {
                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                {
                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                }
                DataRow row2 = this.dtUserFloor.Rows[i];
                if ((uint)row2["f_ConsumerNO"] > 0 && (int.Parse(wgTools.SetObjToStr(row2["f_DoorEnabled"])) == 1))
                {
                    if (((uint) row["f_ConsumerNO"]) != ((uint) row2["f_ConsumerNO"]))
                    {
                        if (((uint) row["f_ConsumerNO"]) > 0)
                        {
                            this.dtPrivilege.Rows.Add(row);
                            row = this.dtPrivilege.NewRow();
                            row["f_ControlSegID1"] = 0;
                            row["f_ControlSegID2"] = 0;
                            row["f_ControlSegID3"] = 0;
                            row["f_ControlSegID4"] = 0;
                            row["f_MoreCards_GrpID_3"] = 0;
                            row["f_MoreCards_GrpID_4"] = 0;
                            row["f_MoreCards_GrpID_2"] = 0;
                            row["f_DoorFirstCard_2"] = 0;
                            row["f_DoorFirstCard_3"] = 0;
                            row["f_DoorFirstCard_4"] = 0;
                            row["f_PIN"] = 0;
                        }
						row["f_ConsumerNO"] = (uint)row2["f_ConsumerNO"];
                        row["f_CardNO"] = row2["f_CardNO"].ToString();
                        row["f_ConsumerID"] = (uint) ((int) row2["f_ConsumerID"]);
                        row["f_BeginYMD"] = row2["f_BeginYMD"];
                        row["f_EndYMD"] = row2["f_EndYMD"];
                        row["f_PIN"] = row2["f_PIN"];
                        this.dvUserFloorCnt.RowFilter = "f_ConsumerID = " + ((uint) ((int) row2["f_ConsumerID"]));
                        if ((this.dvUserFloorCnt.Count >= 1) && (((int) this.dvUserFloorCnt[0]["cnt"]) >= 2))
                        {
                            row["f_AllowFloors"] = ((ulong) row["f_AllowFloors"]) | 0x10000000000L;
                        }
                        row["f_ControlSegID1"] = row2["f_ControlSegID"];
                    }
                    int num2 = int.Parse(row2["f_floorNO"].ToString());
                    if ((num2 > 0) && (num2 <= 40))
                    {
						row["f_AllowFloors"] = ((uint)row["f_AllowFloors"]) | (uint)(1 << (num2 - 1));
                    }
                    num++;
                }
            }
            if (((uint) row["f_ConsumerNO"]) > 0)
                this.dtPrivilege.Rows.Add(row);
            this.dtPrivilege.AcceptChanges();
            this.m_PrivilegeTotal = num;
            this.m_ValidPrivilegeTotal = num;
            this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
            wgTools.WriteLine("getElevatorPrivilegeByID End");
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
            {
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
            }
            return 1;
        }

        public int getElevatorPrivilegeByID_Acc(int ControllerID)
        {
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
            {
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
            }
            wgTools.WriteLine("getPrivilegeByID Start");
            this.dtPrivilege.Rows.Clear();
            string cmdText = string.Format(" SELECT a.f_ConsumerID, COUNT(a.f_FloorID) as cnt FROM t_b_UserFloor a\r\nWHERE a.f_FloorID IN \r\n(SELECT b.f_floorid from t_b_floor b where b.[f_ControllerID]={0} or b.[f_ControllerID] in \r\n (select c.f_ControllerID from t_b_ElevatorGroup c where c.f_ElevatorGroupNO in \r\n   (select d.f_ElevatorGroupNO from t_b_ElevatorGroup d where d.f_ControllerID = {0})))\r\nGROUP BY a.f_ConsumerID ", ControllerID);
            this.dtUserFloorCnt = new DataTable();
            this.dvUserFloorCnt = new DataView(this.dtUserFloorCnt);
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        adapter.Fill(this.dtUserFloorCnt);
                    }
                }
            }
            cmdText = string.Format("SELECT b.f_ConsumerNO, b.f_CardNO, b.f_ConsumerID, b.f_BeginYMD, b.f_EndYMD, b.f_PIN, a.f_ControlSegID, b.f_DoorEnabled, c.f_floorNO\r\nFROM (([t_b_UserFloor] a\r\nINNER JOIN t_b_Consumer b ON ( a.f_ConsumerID = b.f_ConsumerID AND b.f_ConsumerNO IS NOT NULL ))\r\nINNER JOIN t_b_Floor c ON a.f_FloorID = c.f_FloorID )\r\nWHERE f_ControllerID = {0}\r\nORDER BY f_ConsumerNO", ControllerID);
            this.dtUserFloor = new DataTable();
            using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command2 = new OleDbCommand(cmdText, connection2))
                {
                    using (OleDbDataAdapter adapter2 = new OleDbDataAdapter(command2))
                    {
                        command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        adapter2.Fill(this.dtUserFloor);
                    }
                }
            }
            DataRow row = this.dtPrivilege.NewRow();
			row["f_ConsumerNO"] = 0;
            row["f_CardNO"] = 0;
            row["f_ControlSegID1"] = 0;
            row["f_ControlSegID2"] = 0;
            row["f_ControlSegID3"] = 0;
            row["f_ControlSegID4"] = 0;
//            row["f_ControlSegID5"] = 0;
            row["f_MoreCards_GrpID_3"] = 0;
            row["f_MoreCards_GrpID_4"] = 0;
            row["f_MoreCards_GrpID_2"] = 0;
            row["f_DoorFirstCard_2"] = 0;
            row["f_DoorFirstCard_3"] = 0;
            row["f_DoorFirstCard_4"] = 0;
            row["f_PIN"] = 0;
            int num = 0;
            for (int i = 0; i < this.dtUserFloor.Rows.Count; i++)
            {
                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                {
                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                }
                DataRow row2 = this.dtUserFloor.Rows[i];
                if (uint.Parse(row2["f_ConsumerNO"].ToString()) > 0 && int.Parse(wgTools.SetObjToStr(row2["f_DoorEnabled"])) == 1)
                {
                    if (uint.Parse(row["f_ConsumerNO"].ToString()) != uint.Parse(row2["f_ConsumerNO"].ToString()))
                    {
                        if (uint.Parse(row["f_ConsumerNO"].ToString()) > 0)
                        {
                            this.dtPrivilege.Rows.Add(row);
                            row = this.dtPrivilege.NewRow();
                            row["f_ControlSegID1"] = 0;
                            row["f_ControlSegID2"] = 0;
                            row["f_ControlSegID3"] = 0;
                            row["f_ControlSegID4"] = 0;
//                            row["f_ControlSegID5"] = 0;
                            row["f_MoreCards_GrpID_3"] = 0;
                            row["f_MoreCards_GrpID_4"] = 0;
                            row["f_MoreCards_GrpID_2"] = 0;
                            row["f_DoorFirstCard_2"] = 0;
                            row["f_DoorFirstCard_3"] = 0;
                            row["f_DoorFirstCard_4"] = 0;
                            row["f_PIN"] = 0;
                        }
						row["f_ConsumerNO"] = uint.Parse(row2["f_ConsumerNO"].ToString());
                        row["f_CardNO"] = row2["f_CardNO"].ToString();
                        row["f_ConsumerID"] = (uint) ((int) row2["f_ConsumerID"]);
                        row["f_BeginYMD"] = row2["f_BeginYMD"];
                        row["f_EndYMD"] = row2["f_EndYMD"];
                        row["f_PIN"] = row2["f_PIN"];
                        this.dvUserFloorCnt.RowFilter = "f_ConsumerID = " + ((uint) ((int) row2["f_ConsumerID"]));
                        if ((this.dvUserFloorCnt.Count >= 1) && (((int) this.dvUserFloorCnt[0]["cnt"]) >= 2))
                        {
                            row["f_AllowFloors"] = ((ulong) row["f_AllowFloors"]) | 0x10000000000L;
                        }
                        row["f_ControlSegID1"] = row2["f_ControlSegID"];
                    }
                    int num2 = int.Parse(row2["f_floorNO"].ToString());
                    if ((num2 > 0) && (num2 <= 40))
                    {
						row["f_AllowFloors"] = ((uint)row["f_AllowFloors"]) | (uint)(1 << (num2 - 1));
                    }
                    num++;
                }
            }
            if (uint.Parse(row["f_ConsumerNO"].ToString()) > 0)
                this.dtPrivilege.Rows.Add(row);
            this.dtPrivilege.AcceptChanges();
            this.m_PrivilegeTotal = num;
            this.m_ValidPrivilegeTotal = num;
            this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
            wgTools.WriteLine("getElevatorPrivilegeByID End");
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
            {
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
            }
            return 1;
        }
        #endregion

        public void getPrivilegeByDoorName(string DoorName)
        {
            if (wgAppConfig.IsAccessDB)
                this.getPrivilegeByDoorName_Acc(DoorName);
            else
            {
                int controllerID = 0;
                string cmdText = " SELECT f_ControllerID ";
                cmdText = cmdText + " FROM t_b_Door WHERE f_DoorName =  " + wgTools.PrepareStr(DoorName);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        controllerID = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                        if (controllerID > 0)
                        {
                            this.getPrivilegeByID(controllerID);
                        }
                    }
                }
            }
        }

        public void getPrivilegeByDoorName_Acc(string DoorName)
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
                    if (controllerID > 0)
                    {
                        this.getPrivilegeByID(controllerID);
                    }
                }
            }
        }

        public int getPrivilegeByID(int ControllerID)
        {
            if (wgAppConfig.IsAccessDB)
                return this.getPrivilegeByID_Acc(ControllerID);

            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

            if (wgMjController.IsElevator(this.getControllerSNByID(ControllerID)))
                return this.getElevatorPrivilegeByID(ControllerID);

            wgTools.WriteLine("getPrivilegeByID Start");
            this.dtPrivilege.Rows.Clear();
            string cmdText = "";
            DataRow row = this.dtPrivilege.NewRow();
            int num = 0;
            int num2 = 0;
            if (wgMjController.isS8000DC(ControllerID))
            {
                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled, " +
                                " t_b_Consumer.f_ConsumerName FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID WHERE f_ControllerID = " + ControllerID.ToString() + " ORDER BY f_CardNO ";
                row["f_CardNO"] = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (DataView view = new DataView(this.dtPrivilege))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                num++;
                                if (((!(reader["f_CardNO"] is DBNull) &&
                                    (((uint)reader["f_CardNO"]) >= 0L)) &&
                                    ((((uint)reader["f_CardNO"]) <= 0xfffffffeL) && 
                                    !(reader["f_DoorEnabled"] is DBNull))) && 
                                    (int.Parse(wgTools.SetObjToStr(reader["f_DoorEnabled"])) == 1))
                                {
                                    num2++;
                                    if ((uint)row["f_CardNO"] != (uint)reader["f_CardNO"])
                                    {
                                        if ((uint)row["f_CardNO"] > 0)
                                        {
                                            this.dtPrivilege.Rows.Add(row);
                                            row = this.dtPrivilege.NewRow();
                                        }
                                        row["f_ConsumerNO"] = uint.Parse(reader["f_ConsumerNO"].ToString());
                                        row["f_CardNO"] = (uint)reader["f_CardNO"];
                                        row["f_ConsumerID"] = (uint)((int)reader["f_ConsumerID"]);
                                        row["f_BeginYMD"] = reader["f_BeginYMD"];
                                        row["f_EndYMD"] = reader["f_EndYMD"];
                                        row["f_PIN"] = reader["f_PIN"];
                                        row["f_ConsumerName"] = reader["f_ConsumerName"];
                                    }
                                    switch (int.Parse(reader["f_DoorNO"].ToString()))
                                    {
                                        case 1:
                                            row["f_ControlSegID1"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 2:
                                            row["f_ControlSegID2"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 3:
                                            row["f_ControlSegID3"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 4:
                                            row["f_ControlSegID4"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;
                                    }
                                }
                            }
                            if (((uint)row["f_CardNO"]) > 0)
                                this.dtPrivilege.Rows.Add(row);
                            reader.Close();
                            
                            //////////////////////////////////////////////////////////////////////////
                            // PC Check Group
                            if (wgAppConfig.getParamValBoolByNO(0x89))
                            {
                                int userID, groupID;
                                byte seg;
                                ArrayList doorList = new ArrayList();
                                ArrayList doorNoList = new ArrayList();
                                for (int i = 0; i < dtPrivilege.Rows.Count; i++)
                                {
                                    doorList.Clear();
                                    doorNoList.Clear();

                                    userID = int.Parse(dtPrivilege.Rows[i]["f_ConsumerNO"].ToString());
                                    command.CommandText = " SELECT f_ConsumerNO, f_GroupID FROM t_b_Consumer WHERE f_ConsumerNO = '" + userID.ToString() + "'";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        groupID = (int)reader["f_GroupID"];
                                    else
                                        groupID = -1;
                                    reader.Close();
                                    command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                                    reader = command.ExecuteReader();
                                    if (!reader.Read())
                                    {
                                        reader.Close();
                                        continue;
                                    }
                                    reader.Close();
                                    command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        string doors = reader["f_SoundFileName"].ToString();
                                        if (doors == "")
                                        {
                                            for (byte doorID = 1; doorID <= 4; doorID++)
                                                doorNoList.Add(doorID);
                                            reader.Close();
                                            goto l_ExitPCCheck;
                                        }
                                        int comma;
                                        while (true)
                                        {
                                            comma = doors.IndexOf(",");
                                            if (comma != -1)
                                            {
                                                doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                                doors = doors.Substring(comma + 1);
                                            }
                                            else
                                            {
                                                doorList.Add(Convert.ToInt32(doors));
                                                break;
                                            }
                                        }
                                    }
                                    reader.Close();
                                    if (doorList.Count > 0)
                                    {
                                        foreach (int e in doorList)
                                        {
                                            command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                                " AND f_ControllerID = " + ControllerID.ToString();
                                            reader = command.ExecuteReader();
                                            if (reader.Read())
                                                doorNoList.Add((byte)reader["f_DoorNO"]);
                                            reader.Close();
                                        }
                                    }
                                l_ExitPCCheck:
                                    if (doorNoList.Count > 0)
                                    {
                                        foreach (byte e in doorNoList)
                                        {
                                            switch (e)
                                            {
                                                case 1:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID1"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID1"] = (seg | 0x80);
                                                    break;
                                                case 2:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID2"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID2"] = (seg | 0x80);
                                                    break;
                                                case 3:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID3"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID3"] = (seg | 0x80);
                                                    break;
                                                case 4:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID4"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID4"] = (seg | 0x80);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            //////////////////////////////////////////////////////////////////////////

                            this.dtPrivilege.AcceptChanges();

                            this.m_PrivilegeTotal = num;
                            this.m_ValidPrivilegeTotal = num;
                            this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
                            cmdText = " SELECT a.f_ConsumerID, b.f_DoorNO from t_d_doorFirstCardUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_DoorFirstCard_1"] = 1;
                                            break;

                                        case 2:
                                            view[0]["f_DoorFirstCard_2"] = 1;
                                            break;

                                        case 3:
                                            view[0]["f_DoorFirstCard_3"] = 1;
                                            break;

                                        case 4:
                                            view[0]["f_DoorFirstCard_4"] = 1;
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            cmdText = " SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from t_d_doorMoreCardsUsers a inner join t_b_door b on a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString() + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                {
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                }
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_MoreCards_GrpID_1"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 2:
                                            view[0]["f_MoreCards_GrpID_2"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 3:
                                            view[0]["f_MoreCards_GrpID_3"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 4:
                                            view[0]["f_MoreCards_GrpID_4"] = reader["f_MoreCards_GrpID"];
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            this.dtPrivilege.AcceptChanges();
                        }
                    }
                }
            }
            else
            {
                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled, " +
                                " t_b_Consumer.f_ConsumerName FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID WHERE f_ControllerID = " + ControllerID.ToString() + " ORDER BY f_ConsumerNO ";
                row["f_ConsumerNO"] = 0;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (DataView view = new DataView(this.dtPrivilege))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                num++;
                                if (uint.Parse(reader["f_ConsumerNO"].ToString()) > 0 && int.Parse(wgTools.SetObjToStr(reader["f_DoorEnabled"])) == 1)
                                {
                                    num2++;
                                    if ((uint)row["f_ConsumerNO"] != uint.Parse(reader["f_ConsumerNO"].ToString()))
                                    {
                                        if ((uint)row["f_ConsumerNO"] > 0)
                                        {
                                            this.dtPrivilege.Rows.Add(row);
                                            row = this.dtPrivilege.NewRow();
                                        }
                                        row["f_ConsumerNO"] = uint.Parse(reader["f_ConsumerNO"].ToString());
                                        row["f_CardNO"] = reader["f_CardNO"].ToString();
                                        row["f_ConsumerID"] = (uint)((int)reader["f_ConsumerID"]);
                                        row["f_BeginYMD"] = reader["f_BeginYMD"];
                                        row["f_EndYMD"] = reader["f_EndYMD"];
                                        row["f_PIN"] = reader["f_PIN"];
                                        row["f_ConsumerName"] = reader["f_ConsumerName"];
                                    }
                                    switch (int.Parse(reader["f_DoorNO"].ToString()))
                                    {
                                        case 1:
                                            row["f_ControlSegID1"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 2:
                                            row["f_ControlSegID2"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 3:
                                            row["f_ControlSegID3"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 4:
                                            row["f_ControlSegID4"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;
                                    }
                                }
                            }
                            if (uint.Parse(row["f_ConsumerNO"].ToString()) > 0)
                                this.dtPrivilege.Rows.Add(row);
                            reader.Close();

                            //////////////////////////////////////////////////////////////////////////
                            // PC Check Group
                            if (wgAppConfig.getParamValBoolByNO(0x89))
                            {
                                int userID, groupID;
                                byte seg;
                                ArrayList doorList = new ArrayList();
                                ArrayList doorNoList = new ArrayList();
                                for (int i = 0; i < dtPrivilege.Rows.Count; i++)
                                {
                                    doorList.Clear();
                                    doorNoList.Clear();

                                    userID = int.Parse(dtPrivilege.Rows[i]["f_ConsumerNO"].ToString());
                                    command.CommandText = " SELECT f_ConsumerNO, f_GroupID FROM t_b_Consumer WHERE f_ConsumerNO = '" + userID.ToString() + "'";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        groupID = (int)reader["f_GroupID"];
                                    else
                                        groupID = -1;
                                    reader.Close();
                                    command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                                    reader = command.ExecuteReader();
                                    if (!reader.Read())
                                    {
                                        reader.Close();
                                        continue;
                                    }
                                    reader.Close();
                                    command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        string doors = reader["f_SoundFileName"].ToString();
                                        if (doors == "")
                                        {
                                            for (byte doorID = 1; doorID <= 4; doorID++)
                                                doorNoList.Add(doorID);
                                            reader.Close();
                                            goto l_ExitPCCheck1;
                                        }
                                        int comma;
                                        while (true)
                                        {
                                            comma = doors.IndexOf(",");
                                            if (comma != -1)
                                            {
                                                doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                                doors = doors.Substring(comma + 1);
                                            }
                                            else
                                            {
                                                doorList.Add(Convert.ToInt32(doors));
                                                break;
                                            }
                                        }
                                    }
                                    reader.Close();
                                    if (doorList.Count > 0)
                                    {
                                        foreach (int e in doorList)
                                        {
                                            command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                                " AND f_ControllerID = " + ControllerID.ToString();
                                            reader = command.ExecuteReader();
                                            if (reader.Read())
                                                doorNoList.Add((byte)reader["f_DoorNO"]);
                                            reader.Close();
                                        }
                                    }
                                l_ExitPCCheck1:
                                    if (doorNoList.Count > 0)
                                    {
                                        foreach (byte e in doorNoList)
                                        {
                                            switch (e)
                                            {
                                                case 1:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID1"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID1"] = (seg | 0x80);
                                                    break;
                                                case 2:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID2"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID2"] = (seg | 0x80);
                                                    break;
                                                case 3:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID3"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID3"] = (seg | 0x80);
                                                    break;
                                                case 4:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID4"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID4"] = (seg | 0x80);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            //////////////////////////////////////////////////////////////////////////

                            this.dtPrivilege.AcceptChanges();

                            this.m_PrivilegeTotal = num;
                            this.m_ValidPrivilegeTotal = num;
                            this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
                            cmdText = " SELECT a.f_ConsumerID, b.f_DoorNO from (t_d_doorFirstCardUsers a inner join t_b_door b on (a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString() + ")) ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_DoorFirstCard_1"] = 1;
                                            break;

                                        case 2:
                                            view[0]["f_DoorFirstCard_2"] = 1;
                                            break;

                                        case 3:
                                            view[0]["f_DoorFirstCard_3"] = 1;
                                            break;

                                        case 4:
                                            view[0]["f_DoorFirstCard_4"] = 1;
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            cmdText = " SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from (t_d_doorMoreCardsUsers a inner join t_b_door b on (a.f_doorid = b.f_doorid and f_ControllerID =  " + ControllerID.ToString() + ")) ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                {
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                }
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_MoreCards_GrpID_1"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 2:
                                            view[0]["f_MoreCards_GrpID_2"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 3:
                                            view[0]["f_MoreCards_GrpID_3"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 4:
                                            view[0]["f_MoreCards_GrpID_4"] = reader["f_MoreCards_GrpID"];
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            this.dtPrivilege.AcceptChanges();
                        }
                    }
                }

                tbFpTempl.Rows.Clear();
                cmdText = "SELECT t_b_Consumer.f_ConsumerNO, t_d_FpTempl.f_Finger, t_d_FpTempl.f_Templ, t_d_FpTempl.f_Duress " +
                    "FROM ((t_d_FpTempl INNER JOIN t_d_Privilege ON t_d_FpTempl.f_ConsumerID = t_d_Privilege.f_ConsumerID) " +
                    "INNER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_FpTempl.f_ConsumerID) " +
                    "WHERE t_d_Privilege.f_ControllerID = " + ControllerID.ToString() + " ORDER BY t_b_Consumer.f_ConsumerNO ASC";
                string c = "", consumerNO;
                int f = -1, finger;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

                            consumerNO = wgTools.SetObjToStr(reader["f_ConsumerNO"]);
                            finger = Convert.ToInt32(reader["f_Finger"].ToString());
                            if (consumerNO != c || finger != f)
                            {
                                row = tbFpTempl.NewRow();
                                row["f_ConsumerNO"] = consumerNO;
                                row["f_Finger"] = finger;
                                row["f_Templ"] = reader["f_Templ"];
                                row["f_Duress"] = reader["f_Duress"];
                                tbFpTempl.Rows.Add(row);
                                c = consumerNO; f = finger;
                            }
                        }
                        reader.Close();
                        tbFpTempl.AcceptChanges();
                    }
                }

                tbFaceTempl.Rows.Clear();
                cmdText = "SELECT t_b_Consumer.f_ConsumerNO, t_d_FaceTempl.f_Templ " +
                    "FROM ((t_d_FaceTempl INNER JOIN t_d_Privilege ON t_d_FaceTempl.f_ConsumerID = t_d_Privilege.f_ConsumerID) " +
                    "INNER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_FaceTempl.f_ConsumerID) " +
                    "WHERE t_d_Privilege.f_ControllerID = " + ControllerID.ToString() + " ORDER BY t_b_Consumer.f_ConsumerNO ASC";
                c = "";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

                            consumerNO = wgTools.SetObjToStr(reader["f_ConsumerNO"]);
                            if (consumerNO != c)
                            {
                                row = tbFaceTempl.NewRow();
                                row["f_ConsumerNO"] = consumerNO;
                                row["f_Templ"] = reader["f_Templ"];
                                tbFaceTempl.Rows.Add(row);
                                c = consumerNO;
                            }
                        }
                        reader.Close();
                        tbFaceTempl.AcceptChanges();
                    }
                }
            }

            wgTools.WriteLine("getPrivilegeByID End");
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

            return 1;
        }

        public int getPrivilegeByID_Acc(int ControllerID)
        {
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

            if (wgMjController.IsElevator(this.getControllerSNByID(ControllerID)))
                return this.getElevatorPrivilegeByID(ControllerID);

            wgTools.WriteLine("getPrivilegeByID Start");
            this.dtPrivilege.Rows.Clear();
            string cmdText = "";
            DataRow row = this.dtPrivilege.NewRow();
            int num = 0;
            int num2 = 0;
            if (wgMjController.isS8000DC(ControllerID))
            {
                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled, " +
                    " t_b_Consumer.f_ConsumerName FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID WHERE f_ControllerID = " + ControllerID.ToString() + " ORDER BY f_CardNO ";
                row["f_CardNO"] = 0;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (DataView view = new DataView(this.dtPrivilege))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            connection.Open();
                            OleDbDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                num++;
                                if (((!(reader["f_CardNO"] is DBNull) &&
                                    (uint.Parse(reader["f_CardNO"].ToString()) >= 0L)) &&
                                    ((uint.Parse(reader["f_CardNO"].ToString()) <= 0xfffffffeL) &&
                                    !(reader["f_DoorEnabled"] is DBNull))) &&
                                    (int.Parse(wgTools.SetObjToStr(reader["f_DoorEnabled"])) == 1))
                                {
                                    num2++;
                                    if (uint.Parse(row["f_CardNO"].ToString()) != ((uint)uint.Parse(reader["f_CardNO"].ToString())))
                                    {
                                        if (uint.Parse(row["f_CardNO"].ToString()) > 0)
                                        {
                                            this.dtPrivilege.Rows.Add(row);
                                            row = this.dtPrivilege.NewRow();
                                        }
                                        row["f_ConsumerNO"] = uint.Parse(reader["f_ConsumerNO"].ToString());
                                        row["f_CardNO"] = (uint)uint.Parse(reader["f_CardNO"].ToString());
                                        row["f_ConsumerID"] = (uint)((int)reader["f_ConsumerID"]);
                                        row["f_BeginYMD"] = reader["f_BeginYMD"];
                                        row["f_EndYMD"] = reader["f_EndYMD"];
                                        row["f_PIN"] = reader["f_PIN"];
                                        row["f_ConsumerName"] = reader["f_ConsumerName"];
                                    }
                                    switch (int.Parse(reader["f_DoorNO"].ToString()))
                                    {
                                        case 1:
                                            row["f_ControlSegID1"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 2:
                                            row["f_ControlSegID2"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 3:
                                            row["f_ControlSegID3"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 4:
                                            row["f_ControlSegID4"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;
                                    }
                                }
                            }
                            if (uint.Parse(row["f_CardNO"].ToString()) > 0)
                                this.dtPrivilege.Rows.Add(row);
                            reader.Close();

                            //////////////////////////////////////////////////////////////////////////
                            // PC Check Group
                            if (wgAppConfig.getParamValBoolByNO(0x89))
                            {
                                int userID, groupID;
                                byte seg;
                                ArrayList doorList = new ArrayList();
                                ArrayList doorNoList = new ArrayList();
                                for (int i = 0; i < dtPrivilege.Rows.Count; i++)
                                {
                                    doorList.Clear();
                                    doorNoList.Clear();

                                    userID = int.Parse(dtPrivilege.Rows[i]["f_ConsumerNO"].ToString());
                                    command.CommandText = " SELECT f_ConsumerNO, f_GroupID FROM t_b_Consumer WHERE f_ConsumerNO = '" + userID.ToString() + "'";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        groupID = (int)reader["f_GroupID"];
                                    else
                                        groupID = -1;
                                    reader.Close();
                                    command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                                    reader = command.ExecuteReader();
                                    if (!reader.Read())
                                    {
                                        reader.Close();
                                        continue;
                                    }
                                    reader.Close();
                                    command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        string doors = reader["f_SoundFileName"].ToString();
                                        if (doors == "")
                                        {
                                            for (byte doorID = 1; doorID <= 4; doorID++)
                                                doorNoList.Add(doorID);
                                            reader.Close();
                                            goto l_ExitPCCheck;
                                        }
                                        int comma;
                                        while (true)
                                        {
                                            comma = doors.IndexOf(",");
                                            if (comma != -1)
                                            {
                                                doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                                doors = doors.Substring(comma + 1);
                                            }
                                            else
                                            {
                                                doorList.Add(Convert.ToInt32(doors));
                                                break;
                                            }
                                        }
                                    }
                                    reader.Close();
                                    if (doorList.Count > 0)
                                    {
                                        foreach (int e in doorList)
                                        {
                                            command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                                " AND f_ControllerID = " + ControllerID.ToString();
                                            reader = command.ExecuteReader();
                                            if (reader.Read())
                                                doorNoList.Add((byte)reader["f_DoorNO"]);
                                            reader.Close();
                                        }
                                    }
                                l_ExitPCCheck:
                                    if (doorNoList.Count > 0)
                                    {
                                        foreach (byte e in doorNoList)
                                        {
                                            switch (e)
                                            {
                                                case 1:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID1"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID1"] = (seg | 0x80);
                                                    break;
                                                case 2:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID2"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID2"] = (seg | 0x80);
                                                    break;
                                                case 3:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID3"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID3"] = (seg | 0x80);
                                                    break;
                                                case 4:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID4"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID4"] = (seg | 0x80);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            //////////////////////////////////////////////////////////////////////////

                            this.dtPrivilege.AcceptChanges();

                            this.m_PrivilegeTotal = num;
                            this.m_ValidPrivilegeTotal = num;
                            this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
                            cmdText = string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from ( t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_DoorFirstCard_1"] = 1;
                                            break;

                                        case 2:
                                            view[0]["f_DoorFirstCard_2"] = 1;
                                            break;

                                        case 3:
                                            view[0]["f_DoorFirstCard_3"] = 1;
                                            break;

                                        case 4:
                                            view[0]["f_DoorFirstCard_4"] = 1;
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            cmdText = string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from ( t_d_doorMoreCardsUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID =  {0} ))", ControllerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_MoreCards_GrpID_1"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 2:
                                            view[0]["f_MoreCards_GrpID_2"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 3:
                                            view[0]["f_MoreCards_GrpID_3"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 4:
                                            view[0]["f_MoreCards_GrpID_4"] = reader["f_MoreCards_GrpID"];
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            this.dtPrivilege.AcceptChanges();
                        }
                    }
                }
            }
            else
            {
                cmdText = " SELECT t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_CardNO, t_b_Consumer.f_ConsumerID, t_b_Consumer.f_BeginYMD, t_b_Consumer.f_EndYMD, t_b_Consumer.f_PIN, t_d_Privilege.f_ControlSegID, t_d_Privilege.f_DoorNO, t_d_Privilege.f_ControllerID,t_b_Consumer.f_DoorEnabled, " +
                    " t_b_Consumer.f_ConsumerName FROM t_d_Privilege LEFT OUTER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID WHERE f_ControllerID = " + ControllerID.ToString() + " ORDER BY f_ConsumerNO ";
                row["f_ConsumerNO"] = 0;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (DataView view = new DataView(this.dtPrivilege))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            connection.Open();
                            OleDbDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

                                num++;
                                if (uint.Parse(reader["f_ConsumerNO"].ToString()) > 0 && int.Parse(wgTools.SetObjToStr(reader["f_DoorEnabled"])) == 1)
                                {
                                    num2++;
                                    if ((uint)row["f_ConsumerNO"] != uint.Parse(reader["f_ConsumerNO"].ToString()))
                                    {
                                        if ((uint)row["f_ConsumerNO"] > 0)
                                        {
                                            this.dtPrivilege.Rows.Add(row);
                                            row = this.dtPrivilege.NewRow();
                                        }
                                        row["f_ConsumerNO"] = uint.Parse(reader["f_ConsumerNO"].ToString());
                                        row["f_CardNO"] = reader["f_CardNO"].ToString();
                                        row["f_ConsumerID"] = (uint)((int)reader["f_ConsumerID"]);
                                        row["f_BeginYMD"] = reader["f_BeginYMD"];
                                        row["f_EndYMD"] = reader["f_EndYMD"];
                                        row["f_PIN"] = reader["f_PIN"];
                                        row["f_ConsumerName"] = reader["f_ConsumerName"];
                                    }
                                    switch (int.Parse(reader["f_DoorNO"].ToString()))
                                    {
                                        case 1:
                                            row["f_ControlSegID1"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 2:
                                            row["f_ControlSegID2"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 3:
                                            row["f_ControlSegID3"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;

                                        case 4:
                                            row["f_ControlSegID4"] = (byte)(((int)reader["f_ControlSegID"]) & 0x7f);
                                            break;
                                    }
                                }
                            }
                            if (uint.Parse(row["f_ConsumerNO"].ToString()) > 0)
                                this.dtPrivilege.Rows.Add(row);
                            reader.Close();

                            //////////////////////////////////////////////////////////////////////////
                            // PC Check Group
                            if (wgAppConfig.getParamValBoolByNO(0x89))
                            {
                                int userID, groupID;
                                byte seg;
                                ArrayList doorList = new ArrayList();
                                ArrayList doorNoList = new ArrayList();
                                for (int i = 0; i < dtPrivilege.Rows.Count; i++)
                                {
                                    doorList.Clear();
                                    doorNoList.Clear();

                                    userID = int.Parse(dtPrivilege.Rows[i]["f_ConsumerNO"].ToString());
                                    command.CommandText = " SELECT f_ConsumerNO, f_GroupID FROM t_b_Consumer WHERE f_ConsumerNO = '" + userID.ToString() + "'";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                        groupID = (int)reader["f_GroupID"];
                                    else
                                        groupID = -1;
                                    reader.Close();
                                    command.CommandText = " SELECT * FROM t_b_Group4PCCheckAccess WHERE f_CheckAccessActive = 1 AND f_GroupID = " + groupID.ToString();
                                    reader = command.ExecuteReader();
                                    if (!reader.Read())
                                    {
                                        reader.Close();
                                        continue;
                                    }
                                    reader.Close();
                                    command.CommandText = " SELECT f_SoundFileName FROM t_b_Group4PCCheckAccess WHERE f_GroupType = 1";
                                    reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        string doors = reader["f_SoundFileName"].ToString();
                                        if (doors == "")
                                        {
                                            for (byte doorID = 1; doorID <= 4; doorID++)
                                                doorNoList.Add(doorID);
                                            reader.Close();
                                            goto l_ExitPCCheck1;
                                        }
                                        int comma;
                                        while (true)
                                        {
                                            comma = doors.IndexOf(",");
                                            if (comma != -1)
                                            {
                                                doorList.Add(Convert.ToInt32(doors.Substring(0, comma)));
                                                doors = doors.Substring(comma + 1);
                                            }
                                            else
                                            {
                                                doorList.Add(Convert.ToInt32(doors));
                                                break;
                                            }
                                        }
                                    }
                                    reader.Close();
                                    if (doorList.Count > 0)
                                    {
                                        foreach (int e in doorList)
                                        {
                                            command.CommandText = " SELECT f_DoorNO FROM t_b_Door WHERE f_DoorID = " + e.ToString() +
                                                " AND f_ControllerID = " + ControllerID.ToString();
                                            reader = command.ExecuteReader();
                                            if (reader.Read())
                                                doorNoList.Add((byte)reader["f_DoorNO"]);
                                            reader.Close();
                                        }
                                    }
                                l_ExitPCCheck1:
                                    if (doorNoList.Count > 0)
                                    {
                                        foreach (byte e in doorNoList)
                                        {
                                            switch (e)
                                            {
                                                case 1:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID1"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID1"] = (seg | 0x80);
                                                    break;
                                                case 2:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID2"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID2"] = (seg | 0x80);
                                                    break;
                                                case 3:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID3"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID3"] = (seg | 0x80);
                                                    break;
                                                case 4:
                                                    seg = (byte)dtPrivilege.Rows[i]["f_ControlSegID4"];
                                                    dtPrivilege.Rows[i]["f_ControlSegID4"] = (seg | 0x80);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            //////////////////////////////////////////////////////////////////////////

                            this.dtPrivilege.AcceptChanges();

                            this.m_PrivilegeTotal = num;
                            this.m_ValidPrivilegeTotal = num;
                            this.m_ConsumersTotal = this.dtPrivilege.Rows.Count;
                            cmdText = string.Format(" SELECT a.f_ConsumerID, b.f_DoorNO from ( t_d_doorFirstCardUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID = {0} )) ", ControllerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_DoorFirstCard_1"] = 1;
                                            break;

                                        case 2:
                                            view[0]["f_DoorFirstCard_2"] = 1;
                                            break;

                                        case 3:
                                            view[0]["f_DoorFirstCard_3"] = 1;
                                            break;

                                        case 4:
                                            view[0]["f_DoorFirstCard_4"] = 1;
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            cmdText = string.Format(" SELECT a.f_ConsumerID,a.f_MoreCards_GrpID, b.f_DoorNO from ( t_d_doorMoreCardsUsers a inner join t_b_door b on ( a.f_doorid = b.f_doorid and f_ControllerID =  {0} ))", ControllerID.ToString()) + " ORDER BY f_ConsumerID ASC, f_DoorNO ASC ";
                            command.CommandText = cmdText;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                    return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;
                                
                                view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"].ToString();
                                if (view.Count == 1)
                                {
                                    switch (((byte)reader["f_DoorNO"]))
                                    {
                                        case 1:
                                            view[0]["f_MoreCards_GrpID_1"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 2:
                                            view[0]["f_MoreCards_GrpID_2"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 3:
                                            view[0]["f_MoreCards_GrpID_3"] = reader["f_MoreCards_GrpID"];
                                            break;

                                        case 4:
                                            view[0]["f_MoreCards_GrpID_4"] = reader["f_MoreCards_GrpID"];
                                            break;
                                    }
                                }
                            }
                            reader.Close();
                            this.dtPrivilege.AcceptChanges();
                        }
                    }
                }

                tbFpTempl.Rows.Clear();
                cmdText = "SELECT t_b_Consumer.f_ConsumerNO, t_d_FpTempl.f_Finger, t_d_FpTempl.f_Templ, t_d_FpTempl.f_Duress " +
                    "FROM ((t_d_FpTempl INNER JOIN t_d_Privilege ON t_d_FpTempl.f_ConsumerID = t_d_Privilege.f_ConsumerID) " +
                    "INNER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_FpTempl.f_ConsumerID) " +
                    "WHERE t_d_Privilege.f_ControllerID = " + ControllerID.ToString() + " ORDER BY t_b_Consumer.f_ConsumerNO ASC";
                string c = "", consumerNO;
                int f = -1, finger;
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

                            consumerNO = wgTools.SetObjToStr(reader["f_ConsumerNO"]);
                            finger = Convert.ToInt32(reader["f_Finger"].ToString());
                            if (consumerNO != c || finger != f)
                            {
                                row = tbFpTempl.NewRow();
                                row["f_ConsumerNO"] = consumerNO;
                                row["f_Finger"] = finger;
                                row["f_Templ"] = reader["f_Templ"];
                                row["f_Duress"] = reader["f_Duress"];
                                tbFpTempl.Rows.Add(row);
                                c = consumerNO; f = finger;
                            }
                        }
                        reader.Close();
                        tbFpTempl.AcceptChanges();
                    }
                }

                tbFaceTempl.Rows.Clear();
                cmdText = "SELECT t_b_Consumer.f_ConsumerNO, t_d_FaceTempl.f_Templ " +
                    "FROM ((t_d_FaceTempl INNER JOIN t_d_Privilege ON t_d_FaceTempl.f_ConsumerID = t_d_Privilege.f_ConsumerID) " +
                    "INNER JOIN t_b_Consumer ON t_b_Consumer.f_ConsumerID = t_d_FaceTempl.f_ConsumerID) " +
                    "WHERE t_d_Privilege.f_ControllerID = " + ControllerID.ToString() + " ORDER BY t_b_Consumer.f_ConsumerNO ASC";
                c = "";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

                            consumerNO = wgTools.SetObjToStr(reader["f_ConsumerNO"]);
                            if (consumerNO != c)
                            {
                                row = tbFaceTempl.NewRow();
                                row["f_ConsumerNO"] = consumerNO;
                                row["f_Templ"] = reader["f_Templ"];
                                tbFaceTempl.Rows.Add(row);
                                c = consumerNO;
                            }
                        }
                        reader.Close();
                        tbFaceTempl.AcceptChanges();
                    }
                }
            }

            wgTools.WriteLine("getPrivilegeByID End");
            if (wgMjControllerPrivilege.bStopUploadPrivilege)
                return wgGlobal.ERR_PRIVILEGES_STOPUPLOAD;

            return 1;
        }

        public void getPrivilegeBySN(int ControllerSN)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.getPrivilegeBySN_Acc(ControllerSN);
            }
            else
            {
                int controllerID = 0;
                string cmdText = " SELECT f_ControllerID ";
                cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerSN =  " + ControllerSN.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        controllerID = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                        if (controllerID > 0)
                        {
                            this.getPrivilegeByID(controllerID);
                        }
                    }
                }
            }
        }

        public void getPrivilegeBySN_Acc(int ControllerSN)
        {
            int controllerID = 0;
            string cmdText = " SELECT f_ControllerID ";
            cmdText = cmdText + " FROM t_b_Controller WHERE f_ControllerSN =  " + ControllerSN.ToString();
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    controllerID = int.Parse("0" + wgTools.SetObjToStr(command.ExecuteScalar()));
                    if (controllerID > 0)
                    {
                        this.getPrivilegeByID(controllerID);
                    }
                }
            }
        }

        public static int getPrivilegeNumInDBByID(int ControllerID)
        {
            if (wgAppConfig.IsAccessDB)
                return getPrivilegeNumInDBByID_Acc(ControllerID);
            
            int num = 0;
            string cmdText = " SELECT COUNT(DISTINCT t_b_Consumer.f_ConsumerID) ";
            if (wgMjController.isS8000DC(ControllerID))
                cmdText += " FROM t_b_Consumer ,t_d_Privilege " + " WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL " + " AND f_ControllerID =  " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
            else
                cmdText += " FROM t_b_Consumer ,t_d_Privilege " + " WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL " + " AND f_ControllerID =  " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = int.Parse(reader[0].ToString());
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public static int getPrivilegeNumInDBByID_Acc(int ControllerID)
        {
            int num = 0;
            string cmdText = " SELECT COUNT(DISTINCT t_b_Consumer.f_ConsumerID) ";
            if (wgMjController.isS8000DC(ControllerID))
                cmdText += " FROM t_b_Consumer ,t_d_Privilege " + " WHERE t_b_Consumer.f_DoorEnabled=1 AND f_CardNO IS NOT NULL " + " AND f_ControllerID =  " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
            else
                cmdText += " FROM t_b_Consumer ,t_d_Privilege " + " WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL " + " AND f_ControllerID =  " + ControllerID.ToString() + " AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = int.Parse(reader[0].ToString());
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public int upload(int ControllerSN, string DoorName)
        {
            if (wgMjController.isS8000DC(ControllerSN))
                return base.UploadIP(ControllerSN, "", 0xea60, DoorName, this.dtPrivilege);
            else
                return base.UploadIP(ControllerSN, "", 0xea60, DoorName, this.dtPrivilege, tbFpTempl, tbFaceTempl);
        }

        public int upload(int ControllerSN, string IP, string DoorName)
        {
            if (wgMjController.isS8000DC(ControllerSN))
                return base.UploadIP(ControllerSN, IP, 0xea60, DoorName, this.dtPrivilege);
            else
                return base.UploadIP(ControllerSN, IP, 0xea60, DoorName, this.dtPrivilege, tbFpTempl, tbFaceTempl);
        }

        public int upload(int ControllerSN, string IP, int Port, string DoorName)
        {
            if (wgMjController.isS8000DC(ControllerSN))
                return base.UploadIP(ControllerSN, IP, Port, DoorName, this.dtPrivilege);
            else
                return base.UploadIP(ControllerSN, IP, Port, DoorName, this.dtPrivilege, tbFpTempl, tbFaceTempl);
        }

        public int ConsumersTotal
        {
            get
            {
                return this.m_ConsumersTotal;
            }
        }

        public int PrivilegTotal
        {
            get
            {
                return this.m_PrivilegeTotal;
            }
        }

        public int ValidPrivilege
        {
            get
            {
                return this.m_ValidPrivilegeTotal;
            }
        }
    }
}

