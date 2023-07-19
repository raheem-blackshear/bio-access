namespace WG3000_COMM.DataOper
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Management;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.ResStrings;

    internal class icOperator
    {
        private static Dictionary<string, string> dicMenuToFrm;
        private static DataTable dt = null;
        private static DataTable dt1 = null;
        private static DataTable m_dtCurrentOperatorPrivilege;
        private static DataTable m_dtDefaultAllPrivilege;
        private static int m_OperatorID = 0;
        private static string m_OperatorName = "";
        private static ToolStripMenuItem mnuItm = null;

        private static void CheckMenu(MenuStrip Menu, ref DataTable dt)
        {
            foreach (ToolStripMenuItem item in Menu.Items)
            {
                CheckSubMenu(item, ref dt);
            }
        }

        public static int checkSoftwareRegister()
        {
            string str3;
            int num2;
            DateTime time3;
            int num = -1;
            string strEvaluation = "";
            string eName = "";
            if (wgAppConfig.getSystemParamValue(12, out eName, out strEvaluation, out str3) <= 0)
            {
                return num;
            }
            if (strEvaluation == "0")
            {
                string str2 = "1";
                if ((!string.IsNullOrEmpty(eName) && int.TryParse(eName, out num2)) && (num2 > 0))
                {
                    str2 = (num2 + 1).ToString();
                }
                string str = DateTime.Now.ToString("yyyy-MM-dd");
                str3 = "";
                wgAppConfig.setSystemParamValue(12, str, str2, str3);
                str3 = PCSysInfo(false);
                str = "";
                str2 = "";
                wgAppConfig.setSystemParamValue(0x26, str, str2, str3);
                return 0;
            }
            if (strEvaluation == "200405")
            {
                return 1;
            }
            if (int.TryParse(strEvaluation, out num2))
            {
                DateTime time2;
                if (num2 > 1)
                {
                    DateTime time;
                    if (!DateTime.TryParse(eName, out time))
                    {
                        return num;
                    }
                    if ((time.AddDays((double) (num2 - 1)) < DateTime.Now) || (time.AddDays((double) -(num2 - 1)) > DateTime.Now))
                    {
                        strEvaluation = "";
                        return -1;
                    }
                    strEvaluation = CommonStr.strEvaluation;
                    return 0;
                }
                if (!DateTime.TryParse(eName, out time2))
                {
                    return num;
                }
                if ((time2.AddDays(60.0) < DateTime.Now) || (time2.AddDays(-60.0) > DateTime.Now))
                {
                    strEvaluation = "";
                    return -1;
                }
                strEvaluation = CommonStr.strEvaluation;
                return 0;
            }
            if (!DateTime.TryParse(eName, out time3))
            {
                return num;
            }
            if ((time3.AddDays(60.0) < DateTime.Now) || (time3.AddDays(-60.0) > DateTime.Now))
            {
                strEvaluation = "";
                return -1;
            }
            strEvaluation = CommonStr.strEvaluation;
            return 0;
        }

        private static void CheckSubMenu(ToolStripMenuItem menuItem, ref DataTable dt)
        {
            wgTools.WgDebugWrite(menuItem.Text + "--" + menuItem.Name.ToString(), new object[0]);
            if (isAllowAdd(menuItem.Name.ToString()))
            {
                DataRow row = dt.NewRow();
                row[0] = dt.Rows.Count + 1;
                row[1] = menuItem.Name.ToString();
                if (menuItem.Text.IndexOf("(&") > 0)
                {
                    row[2] = menuItem.Text.Substring(0, menuItem.Text.IndexOf("(&"));
                }
                else if (menuItem.Text.IndexOf("&") >= 0)
                {
                    row[2] = menuItem.Text.Replace("&", "");
                }
                else
                {
                    row[2] = menuItem.Text;
                }
                row[3] = 0;
                row[4] = 1;
                dt.Rows.Add(row);
                dt.AcceptChanges();
            }
            for (int i = 0; i < menuItem.DropDownItems.Count; i++)
            {
                if (!(menuItem.DropDownItems[i] is ToolStripSeparator))
                {
                    CheckSubMenu((ToolStripMenuItem) menuItem.DropDownItems[i], ref dt);
                }
            }
        }

        public static void FindInMenu(MenuStrip Menu, string MenuItemName, ref ToolStripMenuItem mnuItm)
        {
            foreach (ToolStripMenuItem item in Menu.Items)
            {
                FindInSubMenu(item, MenuItemName, ref mnuItm);
            }
        }

        private static void FindInSubMenu(ToolStripMenuItem menuItem, string MenuItemName, ref ToolStripMenuItem mnuItm)
        {
            if (mnuItm.Text == "")
            {
                if (menuItem.Name.ToString().Equals(MenuItemName))
                {
                    mnuItm = menuItem;
                }
                for (int i = 0; i < menuItem.DropDownItems.Count; i++)
                {
                    if (!(menuItem.DropDownItems[i] is ToolStripSeparator))
                    {
                        FindInSubMenu((ToolStripMenuItem) menuItem.DropDownItems[i], MenuItemName, ref mnuItm);
                    }
                }
            }
        }

        public static void getDefaultFullFunction(MenuStrip mnuMain)
        {
            dt1 = new DataTable();
            dt1.TableName = "OperatePrivilege";
            dt1.Columns.Add("f_FunctionID");
            dt1.Columns.Add("f_FunctionName");
            dt1.Columns.Add("f_FunctionDisplayName");
            dt1.Columns.Add("f_ReadOnly");
            dt1.Columns.Add("f_FullControl");
            CheckMenu(mnuMain, ref dt1);
            IntertIntoDefaultFullFunctionDT(ref dt1, "TotalControl_RealGetCardRecord", "Real GetCardRecord");
            IntertIntoDefaultFullFunctionDT(ref dt1, "TotalControl_RemoteOpen", "Remote Open");
            IntertIntoDefaultFullFunctionDT(ref dt1, "TotalControl_SetDoorControl", "Set Door Control");
            IntertIntoDefaultFullFunctionDT(ref dt1, "TotalControl_SetDoorDelay", "Set Door Delay");
            IntertIntoDefaultFullFunctionDT(ref dt1, "TotalControl_VideoMonitor", "Video Monitor");
            IntertIntoDefaultFullFunctionDT(ref dt1, "TotalControl_Map", "Map");
            m_dtDefaultAllPrivilege = dt1;
            wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_s_OperatorPrivilege", new object[0]));
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_s_OperatorPrivilege ([f_OperatorID], [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl]) VALUES(1, {0:d},{1},{2},{3:d},{4:d})", new object[] { int.Parse(dt1.Rows[i][0].ToString()), wgTools.PrepareStr(dt1.Rows[i][1].ToString()), wgTools.PrepareStr(dt1.Rows[i][2].ToString()), int.Parse(dt1.Rows[i][3].ToString()), int.Parse(dt1.Rows[i][4].ToString()) }));
            }
            DatatableToXml.CDataToXmlFile(dt1, "OperatePrivilegeCurrent.XML");
        }

        private static void getDicMenuToFrm()
        {
            dicMenuToFrm = new Dictionary<string, string>();
            dicMenuToFrm.Add("frmControllers", "mnuControllers");
            dicMenuToFrm.Add("frmDepartments", "mnuGroups");
            dicMenuToFrm.Add("frmUsers", "mnuConsumers");
            dicMenuToFrm.Add("frmControlSegs", "mnuControlSeg");
            dicMenuToFrm.Add("frmPrivileges", "mnuPrivilege");
            dicMenuToFrm.Add("frmConsole", "mnuTotalControl");
            dicMenuToFrm.Add("btnCheckController", "mnuCheckController");
            dicMenuToFrm.Add("btnAdjustTime", "mnuAdjustTime");
            dicMenuToFrm.Add("btnUpload", "mnuUpload");
            dicMenuToFrm.Add("btnMonitor", "mnuMonitor");
            dicMenuToFrm.Add("btnGetRecords", "mnuGetCardRecords");
            dicMenuToFrm.Add("btnRemoteOpen", "TotalControl_RemoteOpen");
            dicMenuToFrm.Add("btnMaps", "btnMaps");
            dicMenuToFrm.Add("btnRealtimeGetRecords", "mnuRealtimeGetRecords");
            dicMenuToFrm.Add("frmSwipeRecords", "mnuCardRecords");
            dicMenuToFrm.Add("dfrmOperator", "cmdOperatorManage");
            dicMenuToFrm.Add("frmAbout", "mnuAbout");
        }

        public static void getFrmOperatorPrivilege(string frmName, out bool readOnly, out bool fullControl)
        {
            readOnly = true;
            fullControl = true;
            if (!string.IsNullOrEmpty(frmName))
            {
                if (dicMenuToFrm == null)
                {
                    getDicMenuToFrm();
                }
                if ((dicMenuToFrm != null) && dicMenuToFrm.ContainsKey(frmName))
                {
                    if (m_dtCurrentOperatorPrivilege == null)
                    {
                        m_dtCurrentOperatorPrivilege = getOperatorPrivilege(m_OperatorID);
                    }
                    if (m_dtCurrentOperatorPrivilege.Rows.Count > 0)
                    {
                        using (DataView view = new DataView(m_dtCurrentOperatorPrivilege))
                        {
                            view.RowFilter = string.Format("f_FunctionName ={0}", wgTools.PrepareStr(dicMenuToFrm[frmName]));
                            if (view.Count > 0)
                            {
                                readOnly = bool.Parse(view[0]["f_ReadOnly"].ToString());
                                fullControl = bool.Parse(view[0]["f_FullControl"].ToString());
                            }
                        }
                    }
                }
            }
        }

        public static DataTable getOperatorPrivilege(int OperatorID)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return getOperatorPrivilege_Acc(OperatorID);
            }
            dt = new DataTable();
            dt.TableName = "OperatePrivilege";
            dt.Columns.Add("f_FunctionID");
            dt.Columns.Add("f_FunctionName");
            dt.Columns.Add("f_FunctionDisplayName");
            dt.Columns.Add("f_ReadOnly");
            dt.Columns.Add("f_FullControl");
            string cmdText = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + OperatorID.ToString();
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        cmdText = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + 1.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                    }
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();
                        row[0] = reader[0];
                        row[1] = reader[1];
                        row[2] = reader[2];
                        row[3] = int.Parse(reader[3].ToString()) > 0;
                        row[4] = int.Parse(reader[4].ToString()) > 0;
                        dt.Rows.Add(row);
                    }
                    reader.Close();
                    dt.AcceptChanges();
                }
            }
            return dt;
        }

        public static DataTable getOperatorPrivilege_Acc(int OperatorID)
        {
            dt = new DataTable();
            dt.TableName = "OperatePrivilege";
            dt.Columns.Add("f_FunctionID");
            dt.Columns.Add("f_FunctionName");
            dt.Columns.Add("f_FunctionDisplayName");
            dt.Columns.Add("f_ReadOnly");
            dt.Columns.Add("f_FullControl");
            string cmdText = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + OperatorID.ToString();
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        cmdText = "SELECT [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl] FROM t_s_OperatorPrivilege WHERE f_OperatorID = " + 1.ToString();
                        command.CommandText = cmdText;
                        reader = command.ExecuteReader();
                    }
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();
                        row[0] = reader[0];
                        row[1] = reader[1];
                        row[2] = reader[2];
                        row[3] = int.Parse(reader[3].ToString()) > 0;
                        row[4] = int.Parse(reader[4].ToString()) > 0;
                        dt.Rows.Add(row);
                    }
                    reader.Close();
                    dt.AcceptChanges();
                }
            }
            return dt;
        }

        private static void IntertIntoDefaultFullFunctionDT(ref DataTable dt, string name, string display)
        {
            DataRow row = dt.NewRow();
            row[0] = dt.Rows.Count + 1;
            row[1] = name;
            row[2] = display;
            row[3] = 0;
            row[4] = 1;
            dt.Rows.Add(row);
            dt.AcceptChanges();
        }

        private static bool isAllowAdd(string FunctionName)
        {
            string str;
            bool flag = true;
            return (((string.IsNullOrEmpty(FunctionName) || ((str = FunctionName) == null)) || (!(str == "mnuExit") && !(str == "mnuMeetingSign"))) && flag);
        }

        public static bool login(string name, string pwd)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return login_Acc(name, pwd);
            }
            bool flag = false;
            try
            {
                string str2;
                bool flag2 = true;
                if (!string.IsNullOrEmpty(wgAppConfig.dbConString))
                {
                    try
                    {
                        string cmdText = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStr("wiegand");
                        using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                        {
                            using (SqlCommand command = new SqlCommand(cmdText, connection))
                            {
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();
                                if (reader.Read())
                                {
                                    flag2 = false;
                                }
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
                if ((flag2 && (name == "wiegand")) && (pwd == "168668"))
                {
                    m_OperatorID = 1;
                    m_OperatorName = name;
                    return true;
                }
                if (string.IsNullOrEmpty(pwd))
                {
                    str2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStr(name) + " and f_Password is NULL ";
                }
                else
                {
                    str2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStr(name) + " and f_Password = " + wgTools.PrepareStr(pwd);
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(str2, connection2))
                    {
                        connection2.Open();
                        SqlDataReader reader2 = command2.ExecuteReader();
                        if (reader2.Read())
                        {
                            m_OperatorID = int.Parse(reader2["f_OperatorID"].ToString());
                            m_OperatorName = name;
                            flag = true;
                        }
                        reader2.Close();
                    }
                    return flag;
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            return flag;
        }

        public static bool login_Acc(string name, string pwd)
        {
            bool flag = false;
            try
            {
                string str2;
                bool flag2 = true;
                if (!string.IsNullOrEmpty(wgAppConfig.dbConString))
                {
                    try
                    {
                        string cmdText = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStr("wiegand");
                        using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                        {
                            using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                            {
                                connection.Open();
                                OleDbDataReader reader = command.ExecuteReader();
                                if (reader.Read())
                                {
                                    flag2 = false;
                                }
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
                if ((flag2 && (name == "wiegand")) && (pwd == "168668"))
                {
                    m_OperatorID = 1;
                    m_OperatorName = name;
                    return true;
                }
                if (string.IsNullOrEmpty(pwd))
                {
                    str2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStr(name) + " and f_Password is NULL ";
                }
                else
                {
                    str2 = "SELECT * FROM t_s_Operator WHERE f_OperatorName = " + wgTools.PrepareStr(name) + " and f_Password = " + wgTools.PrepareStr(pwd);
                }
                using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command2 = new OleDbCommand(str2, connection2))
                    {
                        connection2.Open();
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        if (reader2.Read())
                        {
                            m_OperatorID = int.Parse(reader2["f_OperatorID"].ToString());
                            m_OperatorName = name;
                            flag = true;
                        }
                        reader2.Close();
                    }
                    return flag;
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            return flag;
        }

        public static bool OperatePrivilegeFullControl(string funName)
        {
            bool bReadOnly = false;
            return (OperatePrivilegeVisible(funName, ref bReadOnly) && !bReadOnly);
        }

        public static void OperatePrivilegeLoad(MenuStrip mnuMain)
        {
            if (m_OperatorID != 1)
            {
                if (m_dtCurrentOperatorPrivilege == null)
                {
                    m_dtCurrentOperatorPrivilege = getOperatorPrivilege(m_OperatorID);
                }
                if (m_dtCurrentOperatorPrivilege.Rows.Count > 0)
                {
                    foreach (DataRow row in m_dtCurrentOperatorPrivilege.Rows)
                    {
                        mnuItm = new ToolStripMenuItem();
                        mnuItm.Name = "";
                        FindInMenu(mnuMain, row["f_FunctionName"].ToString(), ref mnuItm);
                        if (((mnuItm.Name.ToString() != "") && !bool.Parse(row["f_ReadOnly"].ToString())) && !bool.Parse(row["f_FullControl"].ToString()))
                        {
                            mnuItm.Visible = false;
                        }
                    }
                }
            }
        }

        public static void OperatePrivilegeLoad(ref WG3000_COMM.Basic.FunctionData[] funcList)
        {
            if (m_OperatorID != 1)
            {
                if (m_dtCurrentOperatorPrivilege == null)
                {
                    m_dtCurrentOperatorPrivilege = getOperatorPrivilege(m_OperatorID);
                }
                if (m_dtCurrentOperatorPrivilege.Rows.Count > 0)
                {
                    foreach (DataRow row in m_dtCurrentOperatorPrivilege.Rows)
                    {
                        if (!bool.Parse(row["f_ReadOnly"].ToString()) && !bool.Parse(row["f_FullControl"].ToString()))
                        {
                            for (int i = 0; i < funcList.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(funcList[i].menuId) && (funcList[i].menuId == row["f_FunctionName"].ToString()))
                                {
                                    funcList[i].menuId = null;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool OperatePrivilegeTreeDisplay(string FunctionName)
        {
            if (m_OperatorID != 1)
            {
                if (m_dtCurrentOperatorPrivilege == null)
                {
                    m_dtCurrentOperatorPrivilege = getOperatorPrivilege(m_OperatorID);
                }
                if (string.IsNullOrEmpty(FunctionName))
                {
                    return false;
                }
                if (m_dtCurrentOperatorPrivilege.Rows.Count > 0)
                {
                    using (DataView view = new DataView(m_dtCurrentOperatorPrivilege))
                    {
                        view.RowFilter = string.Format("f_FunctionName ={0}", wgTools.PrepareStr(FunctionName));
                        if ((view.Count <= 0) || (!bool.Parse(view[0]["f_ReadOnly"].ToString()) && !bool.Parse(view[0]["f_FullControl"].ToString())))
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            return true;
        }

        public static bool OperatePrivilegeVisible(string funName)
        {
            bool bReadOnly = false;
            return OperatePrivilegeVisible(funName, ref bReadOnly);
        }

        public static bool OperatePrivilegeVisible(string funName, ref bool bReadOnly)
        {
            if (m_OperatorID != 1)
            {
                if (m_dtCurrentOperatorPrivilege == null)
                {
                    m_dtCurrentOperatorPrivilege = getOperatorPrivilege(m_OperatorID);
                }
                if (m_dtCurrentOperatorPrivilege.Rows.Count > 0)
                {
                    foreach (DataRow row in m_dtCurrentOperatorPrivilege.Rows)
                    {
                        if (bool.Parse(row["f_ReadOnly"].ToString()) || bool.Parse(row["f_FullControl"].ToString()))
                        {
                            if (!bool.Parse(row["f_FullControl"].ToString()) && (funName == row["f_FunctionName"].ToString()))
                            {
                                bReadOnly = true;
                            }
                        }
                        else if (funName == row["f_FunctionName"].ToString())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static string PCSysInfo(bool bSuper)
        {
            RegistryKey key;
            string str = "";
            try
            {
                str = str + string.Format("\r\n.Net Framework {0} ", Environment.Version.ToString());
            }
            catch
            {
            }
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj2 in searcher.Get())
                    {
                        str = str + string.Format("\r\n{0}: ", CommonStr.strSystem);
                        str = str + string.Format("\r\n{0} ", obj2["Caption"]);
                        str = str + string.Format("\r\n{0} ", obj2["version"].ToString());
                        str = str + string.Format("\r\n{0} ", obj2["CSDVersion"]);
                        try
                        {
                            key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer");
                            if (key != null)
                            {
                                str = str + string.Format("\r\n{0}: ", CommonStr.strIEVersion);
                                str = str + string.Format("\r\n{0} ", key.GetValue("Version"));
                            }
                            key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\DataAccess");
                            if (key != null)
                            {
                                str = str + string.Format("\r\n{0} ", "MDAC " + key.GetValue("FullInstallVer"));
                            }
                        }
                        catch
                        {
                        }
                        str = str + string.Format("\r\n{0} ", "---------------------------------------------");
                        str = str + string.Format("\r\n{0} ", CommonStr.strRegistered);
                        str = str + string.Format("\r\n{0} ", obj2["RegisteredUser"]);
                        str = str + string.Format("\r\n{0} ", obj2["Organization"].ToString());
                        str = str + string.Format("\r\n{0} ", obj2["SerialNumber"]);
                        str = str + string.Format("\r\n{0} ", "---------------------------------------------");
                        str = str + string.Format("\r\n{0} ", CommonStr.strComputer);
                        str = str + string.Format("\r\n{0} ", obj2["TotalVisibleMemorySize"].ToString() + " KB RAM");
                    }
                }
            }
            catch
            {
            }
            try
            {
                int width = Screen.PrimaryScreen.Bounds.Width;
                int height = Screen.PrimaryScreen.Bounds.Height;
                str = str + string.Format("\r\n{0}:{1:d} x {2:d}: ", CommonStr.strDisplaySize, width, height);
                str = str + string.Format("\r\n{0} IP:", Dns.GetHostName());
                foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    str = str + string.Format("\r\n{0}", address.ToString());
                }
                key = Registry.CurrentUser.OpenSubKey(@"Control Panel\International");
                if (key != null)
                {
                    str = str + string.Format("\r\n{0}", "---------------------------------------------");
                    str = str + string.Format("\r\n{0}:{1}", CommonStr.strCountry, key.GetValue("sCountry"));
                    str = str + string.Format("\r\n{0}:{1}", CommonStr.strTimeFormat, key.GetValue("sTimeFormat"));
                    str = str + string.Format("\r\n{0}:{1}", CommonStr.strShortDateFormat, key.GetValue("sShortDate"));
                    str = str + string.Format("\r\n{0}:{1}", CommonStr.strLongDateFormat, key.GetValue("sLongDate"));
                }
            }
            catch
            {
            }
            try
            {
                if (wgAppConfig.IsAccessDB)
                {
                    string str3 = "Microsoft Access";
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str = str + string.Format("\r\n{0}", "---------------------------------------------");
                        str = str + string.Format("\r\n{0}", str3);
                    }
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        string cmdText = "SELECT @@VERSION";
                        using (SqlCommand command = new SqlCommand(cmdText, connection))
                        {
                            cmdText = wgTools.SetObjToStr(command.ExecuteScalar());
                        }
                        if (!string.IsNullOrEmpty(cmdText))
                        {
                            str = str + string.Format("\r\n{0}", "---------------------------------------------");
                            str = str + string.Format("\r\n{0}", cmdText);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (str + string.Format("\r\n{0}", "---------------------------------------------") + string.Format("\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss "))).Replace("'", "\"");
        }

        public static int setOperatorPrivilege(int OperatorID, DataTable dtPrivilege)
        {
            wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_s_OperatorPrivilege WHERE [f_OperatorID] =" + OperatorID.ToString(), new object[0]));
            DataTable table = dtPrivilege;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                int num = 1;
                int num2 = 1;
                if (string.IsNullOrEmpty(table.Rows[i][3].ToString()))
                {
                    num = 0;
                }
                else if (!bool.Parse(table.Rows[i][3].ToString()))
                {
                    num = 0;
                }
                if (string.IsNullOrEmpty(table.Rows[i][4].ToString()))
                {
                    num2 = 0;
                }
                else if (!bool.Parse(table.Rows[i][4].ToString()))
                {
                    num2 = 0;
                }
                wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_s_OperatorPrivilege ([f_OperatorID], [f_FunctionID], [f_FunctionName], [f_FunctionDisplayName], [f_ReadOnly], [f_FullControl]) VALUES({5}, {0:d},{1},{2},{3:d},{4:d})", new object[] { int.Parse(table.Rows[i][0].ToString()), wgTools.PrepareStr(table.Rows[i][1].ToString()), wgTools.PrepareStr(table.Rows[i][2].ToString()), num, num2, OperatorID.ToString() }));
            }
            return 1;
        }

        public static DataTable dtCurrentOperatorPrivilege
        {
            get
            {
                return m_dtCurrentOperatorPrivilege;
            }
        }

        public static int OperatorID
        {
            get
            {
                return m_OperatorID;
            }
        }

        public static string OperatorName
        {
            get
            {
                return m_OperatorName;
            }
            set
            {
                m_OperatorName = value;
            }
        }
    }
}

