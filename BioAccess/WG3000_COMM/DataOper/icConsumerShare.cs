namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Threading;
    using WG3000_COMM.Core;

    internal class icConsumerShare
    {
        private static DataTable dtLastLoad;
        private static DataTable dtUser = null;
        private static int iSelectedMax = 0x70000000;
        private static int iSelectedMin = 0x70000000;
        private static string lastLoadUsers = "";
        private static int m_iSelectedCurrentNoneMax = 0x70000000;

        public static DataTable getDt()
        {
            return dtLastLoad;
        }

        public static string getOptionalRowfilter()
        {
            return string.Format("( f_Selected <={0:d} )", m_iSelectedCurrentNoneMax);
        }

        public static string getSelectedRowfilter()
        {
            return string.Format(" ( f_Selected >{0:d} )", m_iSelectedCurrentNoneMax);
        }

        public static int getSelectedValue()
        {
            return (m_iSelectedCurrentNoneMax + 1);
        }

        public static string getUpdateLog()
        {
            return wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(50));
        }

        public static void loadUserData()
        {
            wgTools.WriteLine("loadUserData Start");
            Thread.Sleep(100);
            if ((!string.IsNullOrEmpty(lastLoadUsers) && (lastLoadUsers == getUpdateLog())) && ((dtLastLoad != null) && ((iSelectedMax + 0x3e8) < 0x7fffffff)))
            {
                selectNoneUsers();
                dtLastLoad.AcceptChanges();
                wgTools.WriteLine("return dtLastLoad");
                return;
            }
            iSelectedMin = 0x70000000;
            iSelectedMax = 0x70000000;
            m_iSelectedCurrentNoneMax = 0x70000000;
            string cmdText = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID, f_DoorEnabled ";
            cmdText = string.Format(" SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, {0:d} as f_Selected, f_GroupID, f_DoorEnabled  ", iSelectedMin) + " FROM t_b_Consumer " + " ORDER BY f_ConsumerNO ASC ";
            dtUser = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            adapter.Fill(dtUser);
                        }
                    }
                    goto Label_017A;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        adapter2.Fill(dtUser);
                    }
                }
            }
        Label_017A:
            wgTools.WriteLine("da.Fill End");
            try
            {
                dtUser.PrimaryKey = new DataColumn[] { dtUser.Columns[0] };
            }
            catch (Exception)
            {
                throw;
            }
            lastLoadUsers = getUpdateLog();
            dtLastLoad = dtUser;
        }

        public static void selectAllUsers()
        {
            iSelectedMin--;
            m_iSelectedCurrentNoneMax = iSelectedMin;
        }

        public static void selectNoneUsers()
        {
            iSelectedMax++;
            m_iSelectedCurrentNoneMax = iSelectedMax;
        }

        public static void setUpdateLog()
        {
            wgAppConfig.setSystemParamValue(50, null, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), null);
        }

        public static int iSelectedCurrentNoneMax
        {
            get
            {
                return m_iSelectedCurrentNoneMax;
            }
        }
    }
}

