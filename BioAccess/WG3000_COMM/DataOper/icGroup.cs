namespace WG3000_COMM.DataOper
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using WG3000_COMM.Core;

    internal class icGroup
    {
        public int addNew(string GroupName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.addNew_Acc(GroupName);
            }
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string str = " INSERT INTO t_b_Group (f_GroupName) values (";
                    str = str + wgTools.PrepareStr(GroupName) + ")";
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        str = "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName ASC ";
                        command.CommandText = str;
                        SqlDataReader reader = command.ExecuteReader();
                        for (int i = 1; reader.Read(); i++)
                        {
                            wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + i.ToString() + " WHERE  f_GroupID= " + reader[0].ToString());
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

        public int addNew_Acc(string GroupName)
        {
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string str = " INSERT INTO t_b_Group (f_GroupName) values (";
                    str = str + wgTools.PrepareStr(GroupName) + ")";
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        str = "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName ASC ";
                        command.CommandText = str;
                        OleDbDataReader reader = command.ExecuteReader();
                        for (int i = 1; reader.Read(); i++)
                        {
                            wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + i.ToString() + " WHERE  f_GroupID= " + reader[0].ToString());
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

        public int addNew4BatchExcel(string GroupName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.addNew4BatchExcel_Acc(GroupName);
            }
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string str = " INSERT INTO t_b_Group (f_GroupName) values (";
                    str = str + wgTools.PrepareStr(GroupName) + ")";
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
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

        public int addNew4BatchExcel_Acc(string GroupName)
        {
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string str = " INSERT INTO t_b_Group (f_GroupName) values (";
                    str = str + wgTools.PrepareStr(GroupName) + ")";
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
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

        public bool checkExisted(string GroupNewName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.checkExisted_Acc(GroupNewName);
            }
            bool flag = false;
            if (GroupNewName != null)
            {
                if (GroupNewName == "")
                {
                    return flag;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
                        str = (str + " WHERE (f_GroupName = ") + wgTools.PrepareStr(GroupNewName) + " ) ";
                        using (SqlCommand command = new SqlCommand("", connection))
                        {
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

        public bool checkExisted_Acc(string GroupNewName)
        {
            bool flag = false;
            if (GroupNewName != null)
            {
                if (GroupNewName == "")
                {
                    return flag;
                }
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
                        str = (str + " WHERE (f_GroupName = ") + wgTools.PrepareStr(GroupNewName) + " ) ";
                        using (OleDbCommand command = new OleDbCommand("", connection))
                        {
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

        public int delete(string GroupName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.delete_Acc(GroupName);
            }
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string str = " DELETE FROM t_b_Group WHERE (f_GroupName = ";
                    str = ((str + wgTools.PrepareStr(GroupName) + " ) ") + " or (f_GroupName like " + wgTools.PrepareStr(GroupName + @"\%")) + " ) ";
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
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

        public int delete_Acc(string GroupName)
        {
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string str = " DELETE FROM t_b_Group WHERE (f_GroupName = ";
                    str = ((str + wgTools.PrepareStr(GroupName) + " ) ") + " or (f_GroupName like " + wgTools.PrepareStr(GroupName + @"\%")) + " ) ";
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
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

        public int getGroup(ref ArrayList arrGroupName, ref ArrayList arrGroupID, ref ArrayList arrGroupNO)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getGroup_Acc(ref arrGroupName, ref arrGroupID, ref arrGroupNO);
            }
            int num = -9;
            try
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    arrGroupName.Clear();
                    arrGroupID.Clear();
                    arrGroupNO.Clear();
                    ArrayList list = new ArrayList();
                    using (SqlCommand command = new SqlCommand("SELECT f_GroupName from t_b_Group,t_b_Group4Operator WHERE t_b_Group4Operator.f_GroupID = t_b_Group.f_GroupID  AND t_b_Group4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_GroupName ASC", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(reader[0]);
                        }
                        reader.Close();
                        if (list.Count == 0)
                        {
                            arrGroupName.Add("");
                            arrGroupID.Add(0);
                            arrGroupNO.Add(0);
                        }
                        string str = "SELECT f_GroupID,f_GroupName, f_GroupNO from [t_b_Group]  ";
                        str = str + "  ORDER BY f_GroupName ASC";
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
                                arrGroupID.Add(reader[0]);
                                arrGroupName.Add(reader[1]);
                                arrGroupNO.Add(reader[2]);
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

        public int getGroup_Acc(ref ArrayList arrGroupName, ref ArrayList arrGroupID, ref ArrayList arrGroupNO)
        {
            int num = -9;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    arrGroupName.Clear();
                    arrGroupID.Clear();
                    arrGroupNO.Clear();
                    ArrayList list = new ArrayList();
                    using (OleDbCommand command = new OleDbCommand("SELECT f_GroupName from t_b_Group,t_b_Group4Operator WHERE t_b_Group4Operator.f_GroupID = t_b_Group.f_GroupID  AND t_b_Group4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_GroupName ASC", connection))
                    {
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(reader[0]);
                        }
                        reader.Close();
                        if (list.Count == 0)
                        {
                            arrGroupName.Add("");
                            arrGroupID.Add(0);
                            arrGroupNO.Add(0);
                        }
                        string str = "SELECT f_GroupID,f_GroupName, f_GroupNO from [t_b_Group]  ";
                        str = str + "  ORDER BY f_GroupName ASC";
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
                                arrGroupID.Add(reader[0]);
                                arrGroupName.Add(reader[1]);
                                arrGroupNO.Add(reader[2]);
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

        public static int getGroupChildMaxNo(string groupName, ArrayList arrGroupName, ArrayList arrGroupNO)
        {
            int num = 0;
            try
            {
                string str = groupName + @"\";
                for (int i = 0; i < arrGroupName.Count; i++)
                {
                    if ((arrGroupName[i].ToString().IndexOf(str) == 0) && (int.Parse(arrGroupNO[i].ToString()) > num))
                    {
                        num = int.Parse(arrGroupNO[i].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return num;
        }

        public int getGroupID(string GroupNewName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.getGroupID_Acc(GroupNewName);
            }
            int num = -1;
            if (GroupNewName != null)
            {
                if (GroupNewName == "")
                {
                    return num;
                }
                try
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
                        str = (str + " WHERE (f_GroupName = ") + wgTools.PrepareStr(GroupNewName) + " ) ";
                        using (SqlCommand command = new SqlCommand("", connection))
                        {
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

        public int getGroupID_Acc(string GroupNewName)
        {
            int num = -1;
            if (GroupNewName != null)
            {
                if (GroupNewName == "")
                {
                    return num;
                }
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
                        str = (str + " WHERE (f_GroupName = ") + wgTools.PrepareStr(GroupNewName) + " ) ";
                        using (OleDbCommand command = new OleDbCommand("", connection))
                        {
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

        public int Update(string GroupName, string GroupNewName)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.Update_Acc(GroupName, GroupNewName);
            }
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
                        str = (((str + " WHERE (f_GroupName = ") + wgTools.PrepareStr(GroupName) + " ) ") + " or (f_GroupName like " + wgTools.PrepareStr(GroupName + @"\%")) + " )  ORDER BY f_GroupName ASC";
                        command.CommandText = str;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            wgAppConfig.runUpdateSql(("UPDATE t_b_Group SET f_GroupName= " + wgTools.PrepareStr(GroupNewName + reader[1].ToString().Substring(GroupName.Length))) + " WHERE  f_GroupID= " + reader[0].ToString());
                        }
                        reader.Close();
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

        public int Update_Acc(string GroupName, string GroupNewName)
        {
            int num = -9;
            if (GroupName == null)
            {
                return -201;
            }
            if (GroupName == "")
            {
                return -201;
            }
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("", connection))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string str = "SELECT f_GroupID,f_GroupName from [t_b_Group]  ";
                        str = (((str + " WHERE (f_GroupName = ") + wgTools.PrepareStr(GroupName) + " ) ") + " or (f_GroupName like " + wgTools.PrepareStr(GroupName + @"\%")) + " )  ORDER BY f_GroupName ASC";
                        command.CommandText = str;
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            wgAppConfig.runUpdateSql(("UPDATE t_b_Group SET f_GroupName= " + wgTools.PrepareStr(GroupNewName + reader[1].ToString().Substring(GroupName.Length))) + " WHERE  f_GroupID= " + reader[0].ToString());
                        }
                        reader.Close();
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

        public int updateGroupNO()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.updateGroupNO_Acc();
            }
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    command.CommandText = "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName ASC ";
                    SqlDataReader reader = command.ExecuteReader();
                    for (int i = 1; reader.Read(); i++)
                    {
                        wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + i.ToString() + " WHERE  f_GroupID= " + reader[0].ToString());
                    }
                    reader.Close();
                }
            }
            return 1;
        }

        public int updateGroupNO_Acc()
        {
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (OleDbCommand command = new OleDbCommand("", connection))
                {
                    command.CommandText = "SELECT f_GroupID from [t_b_Group]  ORDER BY f_GroupName ASC ";
                    OleDbDataReader reader = command.ExecuteReader();
                    for (int i = 1; reader.Read(); i++)
                    {
                        wgAppConfig.runUpdateSql("UPDATE t_b_Group SET f_GroupNO= " + i.ToString() + " WHERE  f_GroupID= " + reader[0].ToString());
                    }
                    reader.Close();
                }
            }
            return 1;
        }
    }
}

