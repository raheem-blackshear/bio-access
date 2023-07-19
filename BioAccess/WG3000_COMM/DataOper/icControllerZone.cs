namespace WG3000_COMM.DataOper
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using WG3000_COMM.Core;

    internal class icControllerZone
    {
        public int addNew(string ZoneName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.addNew_Acc(ZoneName);
            }
            int num = -9;
            if (ZoneName == null)
            {
                return -201;
            }
            if (ZoneName == "")
            {
                return -201;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = " INSERT INTO t_b_Controller_Zone (f_ZoneName) values (";
                        str = str + wgTools.PrepareStr(ZoneName) + ")";
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        str = "SELECT f_ZoneID from [t_b_Controller_Zone]  ORDER BY f_ZoneName ASC ";
                        command.CommandText = str;
                        SqlDataReader reader = command.ExecuteReader();
                        for (int i = 1; reader.Read(); i++)
                        {
                            wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneNO= " + i.ToString() + " WHERE  f_ZoneID= " + reader[0].ToString());
                        }
                        reader.Close();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int addNew_Acc(string ZoneName)
        {
            int num = -9;
            if (ZoneName == null)
            {
                return -201;
            }
            if (ZoneName == "")
            {
                return -201;
            }
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = " INSERT INTO t_b_Controller_Zone (f_ZoneName) values (";
                        str = str + wgTools.PrepareStr(ZoneName) + ")";
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        str = "SELECT f_ZoneID from [t_b_Controller_Zone]  ORDER BY f_ZoneName ASC ";
                        command.CommandText = str;
                        OleDbDataReader reader = command.ExecuteReader();
                        for (int i = 1; reader.Read(); i++)
                        {
                            wgAppConfig.runUpdateSql("UPDATE t_b_Controller_Zone SET f_ZoneNO= " + i.ToString() + " WHERE  f_ZoneID= " + reader[0].ToString());
                        }
                        reader.Close();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public bool checkExisted(string ZoneNewName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.checkExisted_Acc(ZoneNewName);
            }
            bool flag = false;
            if (ZoneNewName != null)
            {
                if (ZoneNewName == "")
                {
                    return flag;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command = new SqlCommand("", connection))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            string str = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
                            str = (str + " WHERE (f_ZoneName = ") + wgTools.PrepareStr(ZoneNewName) + " ) ";
                            command.CommandText = str;
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                flag = true;
                            }
                            reader.Close();
                        }
                        return flag;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            return flag;
        }

        public bool checkExisted_Acc(string ZoneNewName)
        {
            bool flag = false;
            if (ZoneNewName != null)
            {
                if (ZoneNewName == "")
                {
                    return flag;
                }
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand("", connection))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            string str = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
                            str = (str + " WHERE (f_ZoneName = ") + wgTools.PrepareStr(ZoneNewName) + " ) ";
                            command.CommandText = str;
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                flag = true;
                            }
                            reader.Close();
                        }
                        return flag;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            return flag;
        }

        public int delete(string ZoneName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.delete_Acc(ZoneName);
            }
            int num = -9;
            if (ZoneName == null)
            {
                return -201;
            }
            if (ZoneName == "")
            {
                return -201;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = " DELETE FROM t_b_Controller_Zone WHERE (f_ZoneName = ";
                        str = ((str + wgTools.PrepareStr(ZoneName) + " ) ") + " or (f_ZoneName like " + wgTools.PrepareStr(ZoneName + @"\%")) + " ) ";
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int delete_Acc(string ZoneName)
        {
            int num = -9;
            if (ZoneName == null)
            {
                return -201;
            }
            if (ZoneName == "")
            {
                return -201;
            }
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = " DELETE FROM t_b_Controller_Zone WHERE (f_ZoneName = ";
                        str = ((str + wgTools.PrepareStr(ZoneName) + " ) ") + " or (f_ZoneName like " + wgTools.PrepareStr(ZoneName + @"\%")) + " ) ";
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int getAllowedControllers(ref DataTable dtController)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getAllowedControllers_Acc(ref dtController);
            }
            int num = -9;
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        command.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName ASC";
                        SqlDataReader reader = command.ExecuteReader();
                        ArrayList list = new ArrayList();
                        while (reader.Read())
                        {
                            list.Add(reader[0]);
                        }
                        reader.Close();
                        if (list.Count == 0)
                        {
                            return 1;
                        }
                        string str = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
                        using (SqlDataAdapter adapter = new SqlDataAdapter(str + "  ORDER BY f_ZoneName ASC", connection))
                        {
                            using (DataTable table = new DataTable("Zones"))
                            {
                                adapter.Fill(table);
                                using (DataView view = new DataView(table))
                                {
                                    bool flag = true;
                                    string str2 = "";
                                    int num2 = 0;
                                    int result = 0;
                                    while (num2 < dtController.Rows.Count)
                                    {
                                        DataRow row = dtController.Rows[num2];
                                        flag = false;
                                        if (int.TryParse(row["f_ZoneID"].ToString(), out result))
                                        {
                                            view.RowFilter = "f_ZoneID = " + row["f_ZoneID"].ToString();
                                            if (view.Count > 0)
                                            {
                                                str2 = (string) view[0]["f_ZoneName"];
                                                for (int i = 0; i < list.Count; i++)
                                                {
                                                    if ((str2 == list[i].ToString()) || (str2.IndexOf(list[i].ToString() + @"\") == 0))
                                                    {
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!flag)
                                        {
                                            dtController.Rows.Remove(row);
                                            dtController.AcceptChanges();
                                        }
                                        else
                                        {
                                            num2++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int getAllowedControllers_Acc(ref DataTable dtController)
        {
            int num = -9;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        command.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName ASC";
                        OleDbDataReader reader = command.ExecuteReader();
                        ArrayList list = new ArrayList();
                        while (reader.Read())
                        {
                            list.Add(reader[0]);
                        }
                        reader.Close();
                        if (list.Count == 0)
                        {
                            return 1;
                        }
                        string str = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(str + "  ORDER BY f_ZoneName ASC", connection))
                        {
                            using (DataTable table = new DataTable("Zones"))
                            {
                                adapter.Fill(table);
                                using (DataView view = new DataView(table))
                                {
                                    bool flag = true;
                                    string str2 = "";
                                    int num2 = 0;
                                    int result = 0;
                                    while (num2 < dtController.Rows.Count)
                                    {
                                        DataRow row = dtController.Rows[num2];
                                        flag = false;
                                        if (int.TryParse(row["f_ZoneID"].ToString(), out result))
                                        {
                                            view.RowFilter = "f_ZoneID = " + row["f_ZoneID"].ToString();
                                            if (view.Count > 0)
                                            {
                                                str2 = (string) view[0]["f_ZoneName"];
                                                for (int i = 0; i < list.Count; i++)
                                                {
                                                    if ((str2 == list[i].ToString()) || (str2.IndexOf(list[i].ToString() + @"\") == 0))
                                                    {
                                                        flag = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!flag)
                                        {
                                            dtController.Rows.Remove(row);
                                            dtController.AcceptChanges();
                                        }
                                        else
                                        {
                                            num2++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int getZone(ref ArrayList arrZoneName, ref ArrayList arrZoneID, ref ArrayList arrZoneNO)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getZone_Acc(ref arrZoneName, ref arrZoneID, ref arrZoneNO);
            }
            int num = -9;
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        arrZoneName.Clear();
                        arrZoneID.Clear();
                        arrZoneNO.Clear();
                        command.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName ASC";
                        SqlDataReader reader = command.ExecuteReader();
                        ArrayList list = new ArrayList();
                        while (reader.Read())
                        {
                            list.Add(reader[0]);
                        }
                        reader.Close();
                        if (list.Count == 0)
                        {
                            arrZoneName.Add("");
                            arrZoneID.Add(0);
                            arrZoneNO.Add(0);
                        }
                        string str = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
                        str = str + "  ORDER BY f_ZoneName ASC";
                        command.CommandText = str;
                        reader = command.ExecuteReader();
                        bool flag = true;
                        string str2 = "";
                        while (reader.Read())
                        {
                            if (list.Count > 0)
                            {
                                flag = false;
                            }
                            for (int i = 0; i < list.Count; i++)
                            {
                                str2 = (string) reader[1];
                                if ((str2 == list[i].ToString()) || (str2.IndexOf(list[i].ToString() + @"\") == 0))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                arrZoneID.Add(reader[0]);
                                arrZoneName.Add(reader[1]);
                                arrZoneNO.Add(reader[2]);
                            }
                        }
                        reader.Close();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int getZone_Acc(ref ArrayList arrZoneName, ref ArrayList arrZoneID, ref ArrayList arrZoneNO)
        {
            int num = -9;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        arrZoneName.Clear();
                        arrZoneID.Clear();
                        arrZoneNO.Clear();
                        command.CommandText = "SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName ASC";
                        OleDbDataReader reader = command.ExecuteReader();
                        ArrayList list = new ArrayList();
                        while (reader.Read())
                        {
                            list.Add(reader[0]);
                        }
                        reader.Close();
                        if (list.Count == 0)
                        {
                            arrZoneName.Add("");
                            arrZoneID.Add(0);
                            arrZoneNO.Add(0);
                        }
                        string str = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
                        str = str + "  ORDER BY f_ZoneName ASC";
                        command.CommandText = str;
                        reader = command.ExecuteReader();
                        bool flag = true;
                        string str2 = "";
                        while (reader.Read())
                        {
                            if (list.Count > 0)
                            {
                                flag = false;
                            }
                            for (int i = 0; i < list.Count; i++)
                            {
                                str2 = (string) reader[1];
                                if ((str2 == list[i].ToString()) || (str2.IndexOf(list[i].ToString() + @"\") == 0))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                arrZoneID.Add(reader[0]);
                                arrZoneName.Add(reader[1]);
                                arrZoneNO.Add(reader[2]);
                            }
                        }
                        reader.Close();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public static int getZoneChildMaxNo(string ZoneName, ArrayList arrZoneName, ArrayList arrZoneNO)
        {
            int num = 0;
            try
            {
                string str = ZoneName + @"\";
                for (int i = 0; i < arrZoneName.Count; i++)
                {
                    if ((arrZoneName[i].ToString().IndexOf(str) == 0) && (int.Parse(arrZoneNO[i].ToString()) > num))
                    {
                        num = int.Parse(arrZoneNO[i].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return num;
        }

        public int getZoneID(string ZoneNewName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getZoneID_Acc(ZoneNewName);
            }
            int num = -1;
            if (ZoneNewName != null)
            {
                if (ZoneNewName == "")
                {
                    return num;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command = new SqlCommand("", connection))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            string str = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
                            str = (str + " WHERE (f_ZoneName = ") + wgTools.PrepareStr(ZoneNewName) + " ) ";
                            command.CommandText = str;
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                num = (int) reader[0];
                            }
                            reader.Close();
                        }
                        return num;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            return num;
        }

        public int getZoneID_Acc(string ZoneNewName)
        {
            int num = -1;
            if (ZoneNewName != null)
            {
                if (ZoneNewName == "")
                {
                    return num;
                }
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand("", connection))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            string str = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
                            str = (str + " WHERE (f_ZoneName = ") + wgTools.PrepareStr(ZoneNewName) + " ) ";
                            command.CommandText = str;
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                num = (int) reader[0];
                            }
                            reader.Close();
                        }
                        return num;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            return num;
        }

        public int Update(string ZoneName, string ZoneNewName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.Update_Acc(ZoneName, ZoneNewName);
            }
            int num = -9;
            if (ZoneName == null)
            {
                return -201;
            }
            if (ZoneName == "")
            {
                return -201;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
                        str = (((str + " WHERE (f_ZoneName = ") + wgTools.PrepareStr(ZoneName) + " ) ") + " or (f_ZoneName like " + wgTools.PrepareStr(ZoneName + @"\%")) + " )  ORDER BY f_ZoneName ASC";
                        command.CommandText = str;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            wgAppConfig.runUpdateSql(("UPDATE t_b_Controller_Zone SET f_ZoneName= " + wgTools.PrepareStr(ZoneNewName + reader[1].ToString().Substring(ZoneName.Length))) + " WHERE  f_ZoneID= " + reader[0].ToString());
                        }
                        reader.Close();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }

        public int Update_Acc(string ZoneName, string ZoneNewName)
        {
            int num = -9;
            if (ZoneName == null)
            {
                return -201;
            }
            if (ZoneName == "")
            {
                return -201;
            }
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_ZoneID,f_ZoneName from [t_b_Controller_Zone]  ";
                        str = (((str + " WHERE (f_ZoneName = ") + wgTools.PrepareStr(ZoneName) + " ) ") + " or (f_ZoneName like " + wgTools.PrepareStr(ZoneName + @"\%")) + " )  ORDER BY f_ZoneName ASC";
                        command.CommandText = str;
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            wgAppConfig.runUpdateSql(("UPDATE t_b_Controller_Zone SET f_ZoneName= " + wgTools.PrepareStr(ZoneNewName + reader[1].ToString().Substring(ZoneName.Length))) + " WHERE  f_ZoneID= " + reader[0].ToString());
                        }
                        reader.Close();
                    }
                }
                num = 1;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return num;
        }
    }
}

