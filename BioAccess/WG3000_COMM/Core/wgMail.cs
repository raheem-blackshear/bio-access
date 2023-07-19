namespace WG3000_COMM.Core
{
    using jmail;
    using System;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Management;
    using System.Net;
    using System.Net.Mail;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ResStrings;

    internal class wgMail
    {
        public static bool bSendingMail;

        private static bool sendMail(string strInfo, string mailSubject)
        {
            if (sendMail2010(strInfo, mailSubject))
            {
                return true;
            }
            try
            {
                jmail.Message message;
                try
                {
                    message = new MessageClass();
                }
                catch
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "regsvr32";
                        process.StartInfo.Arguments = "/s \"" + Application.StartupPath + "\\jmail.dll\"";
                        process.Start();
                        process.WaitForExit();
                        message = new MessageClass();
                    }
                }
                message.Logging = true;
                message.Charset = "GB2312";
                message.ContentType = "text/plain";
                message.AddRecipient("mail20050530@126.com", null, null);
                message.AddRecipientBCC("ccmail20050530@126.com", null);
                message.AddRecipientBCC("ccmail20050530@21cn.com", null);
                message.AddRecipientBCC("mail20050530@21cn.com", null);
                message.From = "ccmail20050530@126.com";
                message.MailServerUserName = "ccmail20050530@126.com";
                message.MailServerPassWord = "CCMAIL20055678";
                message.Subject = mailSubject;
                message.Body = strInfo + "\r\nsendMail";
                message.Priority = 1;
                message.Send("smtp.126.com", false);
                message.Close();
                return true;
            }
            catch
            {
            }
            return sendMailBackup(strInfo, mailSubject);
        }

        private static bool sendMail2010(string strInfo, string mailSubject)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.BodyEncoding = Encoding.GetEncoding("gb2312");
                    message.SubjectEncoding = Encoding.UTF8;
                    message.To.Add("mail20050530@126.com");
                    message.Bcc.Add("ccmail20050530@126.com");
                    message.Bcc.Add("ccmail20050530@21cn.com");
                    message.Bcc.Add("mail20050530@21cn.com");
                    message.From = new MailAddress("ccmail20050530@126.com");
                    message.Priority = MailPriority.High;
                    message.Subject = mailSubject;
                    message.Body = strInfo + "\r\nsendMail2010";
                    SmtpClient client = new SmtpClient("smtp.126.com");
                    client.Credentials = new NetworkCredential("ccmail20050530", "CCMAIL20055678");
                    client.Send(message);
                    client = null;
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static bool sendMailBackup(string strInfo, string mailSubject)
        {
            try
            {
                jmail.Message message;
                try
                {
                    message = new MessageClass();
                }
                catch
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "regsvr32";
                        process.StartInfo.Arguments = "/s \"" + Application.StartupPath + "\\jmail.dll\"";
                        process.Start();
                        process.WaitForExit();
                        message = new MessageClass();
                    }
                }
                message.Logging = true;
                message.Charset = "GB2312";
                message.ContentType = "text/plain";
                message.Logging = true;
                message.Charset = "GB2312";
                message.ContentType = "text/plain";
                message.AddRecipient("mail20050530@126.com", null, null);
                message.AddRecipientBCC("ccmail20050530@126.com", null);
                message.AddRecipientBCC("ccmail20050530@21cn.com", null);
                message.AddRecipientBCC("mail20050530@21cn.com", null);
                message.From = "ccmail20050530@21cn.com";
                message.MailServerUserName = "ccmail20050530@21cn.com";
                message.MailServerPassWord = "CCMAIL20055678";
                message.Subject = mailSubject;
                message.Body = strInfo + "\r\nsendMailBackup";
                message.Priority = 1;
                message.Send("smtp.21cn.com", false);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static void sendMailOnce()
        {
            string str2;
            string strInfo = sysInfo4Mail(out str2);
            int num = 0;
            bSendingMail = true;
            while (num < 3)
            {
                if (sendMail(strInfo, str2))
                {
                    bSendingMail = false;
                    return;
                }
                Thread.Sleep(0x1d4c0);
                num++;
            }
            bSendingMail = false;
        }

        private static string sysInfo4Mail(out string subject)
        {
            if (wgAppConfig.IsAccessDB)
            {
                return sysInfo4Mail_Acc(out subject);
            }
            string str = "";
            string str2 = "Mail Subject";
            try
            {
                string str4;
                string str6;
                SqlDataReader reader;
                string str8;
                string str9;
                str = str + "\r\n【软件版本】：V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
                string str3 = wgAppConfig.getSystemParamByNO(0x31);
                str = str + "\r\n【起始日期】：";
                if (!string.IsNullOrEmpty(str3))
                {
                    str = str + DateTime.Parse(str3).ToString("yyyy-MM-dd");
                }
                str = str + "\r\n【硬件版本】：";
                wgAppConfig.getSystemParamValue(0x30, out str6, out str6, out str4);
                if (!string.IsNullOrEmpty(str4) && (str4.IndexOf("\r\n") >= 0))
                {
                    str4 = str4.Substring(str4.IndexOf("\r\n") + "\r\n".Length);
                }
                string str7 = "";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            str7 = str7 + wgTools.SetObjToStr(reader[0]);
                            if (str4.IndexOf(reader[0].ToString() + ",VER=") >= 0)
                            {
                                string str5 = str4.Substring(str4.IndexOf(reader[0].ToString() + ",VER=") + (reader[0].ToString() + ",VER=").Length);
                                if (str5.Length > 0)
                                {
                                    str5 = str5.Substring(0, str5.IndexOf(","));
                                    str7 = str7 + "(v" + str5 + ");";
                                }
                                else
                                {
                                    str7 = str7 + "(v  )";
                                }
                            }
                            else
                            {
                                str7 = str7 + "(v  )";
                            }
                        }
                        reader.Close();
                    }
                }
                if (!string.IsNullOrEmpty(str7))
                {
                    str = str + str7;
                }
                str = str + "\r\n";
                str = str + "\r\n【使用者公司全称】：";
                wgAppConfig.getSystemParamValue(0x24, out str6, out str8, out str9);
                if (!string.IsNullOrEmpty(str8))
                {
                    str = str + str8;
                }
                if (icOperator.checkSoftwareRegister() > 0)
                {
                    str = str + "\r\n" + CommonStr.strAlreadyRegistered;
                    str2 = str8 + "[" + CommonStr.strAlreadyRegistered + "]";
                    str = str + "\r\n【施工和承建公司名称】：" + str9;
                }
                else
                {
                    str = str + "\r\n" + CommonStr.strUnRegistered;
                    str2 = str8 + "[" + CommonStr.strUnRegistered + "]";
                    str = str + "\r\n【施工和承建公司名称】：" + str9;
                }
                str2 = wgAppConfig.ProductTypeOfApp + "2012_" + str2 + "_V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand("", connection2))
                    {
                        connection2.Open();
                        command2.CommandText = " SELECT COUNT(*)  from t_b_door where f_doorEnabled=1";
                        reader = command2.ExecuteReader();
                        if (reader.Read())
                        {
                            str = str + "\r\n【门数】：" + reader[0].ToString();
                        }
                        reader.Close();
                    }
                }
                using (SqlConnection connection3 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command3 = new SqlCommand("", connection3))
                    {
                        connection3.Open();
                        command3.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
                        reader = command3.ExecuteReader();
                        str7 = "";
                        while (reader.Read())
                        {
                            str7 = str7 + "\r\n" + reader[0].ToString();
                        }
                        reader.Close();
                    }
                }
                if (!string.IsNullOrEmpty(str7))
                {
                    str = str + "\r\n【控制器序列号S/N】：" + str7;
                }
                str = str + "\r\n【其他信息】：";
                using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command4 = new SqlCommand("", connection4))
                    {
                        connection4.Open();
                        command4.CommandText = "SELECT count(*) FROM t_b_Consumer ";
                        reader = command4.ExecuteReader();
                        if (reader.Read())
                        {
                            str = str + "\r\n【注册人数】：" + reader[0].ToString();
                        }
                        reader.Close();
                    }
                }
                using (SqlConnection connection5 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command5 = new SqlCommand("", connection5))
                    {
                        connection5.Open();
                        command5.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam WHERE f_NO>100 ORDER BY f_No Asc ";
                        reader = command5.ExecuteReader();
                        if (reader.HasRows)
                        {
                            string str10 = "";
                            str = str + "\r\n【启用功能(No,Val)】：\r\n";
                            while (reader.Read())
                            {
                                string str12 = str;
                                str = str12 + " (" + reader["f_No"].ToString() + "," + wgTools.SetObjToStr(reader["f_Value"]) + ")";
                                if (wgTools.SetObjToStr(reader["f_Value"]) == "1")
                                {
                                    str10 = str10 + reader["f_No"].ToString() + ";";
                                }
                            }
                            if (!string.IsNullOrEmpty(str10))
                            {
                                str = str + "\r\n【已启用】：" + str10;
                            }
                        }
                        reader.Close();
                    }
                }
                using (SqlConnection connection6 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command6 = new SqlCommand("", connection6))
                    {
                        connection6.Open();
                        command6.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam  WHERE f_NO<100  ORDER BY f_No Asc ";
                        reader = command6.ExecuteReader();
                        if (reader.HasRows)
                        {
                            str = str + "\r\n【参数值(No,Val)】：\r\n";
                            while (reader.Read())
                            {
                                string str13 = str;
                                str = str13 + " (" + reader["f_No"].ToString() + "," + wgTools.SetObjToStr(reader["f_Value"]) + ")";
                            }
                        }
                        reader.Close();
                    }
                }
                if (!string.IsNullOrEmpty(str4))
                {
                    str = str + "\r\n【DrvInfo】：\r\n";
                    str = str + str4;
                }
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj2 in searcher.Get())
                    {
                        str = str + string.Format("\r\n【{0}】： ", CommonStr.strSystem);
                        str = str + string.Format("\r\n{0} ", obj2["Caption"]);
                        str = str + string.Format("\r\n{0} ", obj2["version"].ToString());
                        str = str + string.Format("\r\n{0} ", obj2["CSDVersion"]);
                    }
                }
                str = str + string.Format("\r\n【数据库版本】： ", new object[0]);
                string str11 = "SELECT @@VERSION";
                using (SqlConnection connection7 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command7 = new SqlCommand("", connection7))
                    {
                        connection7.Open();
                        command7.CommandText = str11;
                        str11 = wgTools.SetObjToStr(command7.ExecuteScalar());
                    }
                }
                if (!string.IsNullOrEmpty(str11))
                {
                    str = str + string.Format("\r\n{0}", str11);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            str = (str + "\r\n---------------------------------------------" + "\r\n") + "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
            subject = str2;
            return str;
        }

        private static string sysInfo4Mail_Acc(out string subject)
        {
            string str = "";
            string str2 = "Mail Subject";
            try
            {
                string str4;
                string str6;
                OleDbDataReader reader;
                string str8;
                string str9;
                str = str + "\r\n【软件版本】：V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
                string str3 = wgAppConfig.getSystemParamByNO(0x31);
                str = str + "\r\n【起始日期】：";
                if (!string.IsNullOrEmpty(str3))
                {
                    str = str + DateTime.Parse(str3).ToString("yyyy-MM-dd");
                }
                str = str + "\r\n【硬件版本】：";
                wgAppConfig.getSystemParamValue(0x30, out str6, out str6, out str4);
                if (!string.IsNullOrEmpty(str4) && (str4.IndexOf("\r\n") >= 0))
                {
                    str4 = str4.Substring(str4.IndexOf("\r\n") + "\r\n".Length);
                }
                string str7 = "";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            str7 = str7 + wgTools.SetObjToStr(reader[0]);
                            if (str4.IndexOf(reader[0].ToString() + ",VER=") >= 0)
                            {
                                string str5 = str4.Substring(str4.IndexOf(reader[0].ToString() + ",VER=") + (reader[0].ToString() + ",VER=").Length);
                                if (str5.Length > 0)
                                {
                                    str5 = str5.Substring(0, str5.IndexOf(","));
                                    str7 = str7 + "(v" + str5 + ");";
                                }
                                else
                                {
                                    str7 = str7 + "(v  )";
                                }
                            }
                            else
                            {
                                str7 = str7 + "(v  )";
                            }
                        }
                        reader.Close();
                    }
                }
                if (!string.IsNullOrEmpty(str7))
                {
                    str = str + str7;
                }
                str = str + "\r\n";
                str = str + "\r\n【使用者公司全称】：";
                wgAppConfig.getSystemParamValue(0x24, out str6, out str8, out str9);
                if (!string.IsNullOrEmpty(str8))
                {
                    str = str + str8;
                }
                if (icOperator.checkSoftwareRegister() > 0)
                {
                    str = str + "\r\n" + CommonStr.strAlreadyRegistered;
                    str2 = str8 + "[" + CommonStr.strAlreadyRegistered + "]";
                    str = str + "\r\n【施工和承建公司名称】：" + str9;
                }
                else
                {
                    str = str + "\r\n" + CommonStr.strUnRegistered;
                    str2 = str8 + "[" + CommonStr.strUnRegistered + "]";
                    str = str + "\r\n【施工和承建公司名称】：" + str9;
                }
                str2 = wgAppConfig.ProductTypeOfApp + "2012_" + str2 + "_V" + Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
                using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command2 = new OleDbCommand("", connection2))
                    {
                        connection2.Open();
                        command2.CommandText = " SELECT COUNT(*)  from t_b_door where f_doorEnabled=1";
                        reader = command2.ExecuteReader();
                        if (reader.Read())
                        {
                            str = str + "\r\n【门数】：" + reader[0].ToString();
                        }
                        reader.Close();
                    }
                }
                using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command3 = new OleDbCommand("", connection3))
                    {
                        connection3.Open();
                        command3.CommandText = "SELECT f_ControllerSN FROM t_b_Controller ";
                        reader = command3.ExecuteReader();
                        str7 = "";
                        while (reader.Read())
                        {
                            str7 = str7 + "\r\n" + reader[0].ToString();
                        }
                        reader.Close();
                    }
                }
                if (!string.IsNullOrEmpty(str7))
                {
                    str = str + "\r\n【控制器序列号S/N】：" + str7;
                }
                str = str + "\r\n【其他信息】：";
                using (OleDbConnection connection4 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command4 = new OleDbCommand("", connection4))
                    {
                        connection4.Open();
                        command4.CommandText = "SELECT count(*) FROM t_b_Consumer ";
                        reader = command4.ExecuteReader();
                        if (reader.Read())
                        {
                            str = str + "\r\n【注册人数】：" + reader[0].ToString();
                        }
                        reader.Close();
                    }
                }
                using (OleDbConnection connection5 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command5 = new OleDbCommand("", connection5))
                    {
                        connection5.Open();
                        command5.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam WHERE f_NO>100 ORDER BY f_No Asc ";
                        reader = command5.ExecuteReader();
                        if (reader.HasRows)
                        {
                            string str10 = "";
                            str = str + "\r\n【启用功能(No,Val)】：\r\n";
                            while (reader.Read())
                            {
                                string str12 = str;
                                str = str12 + " (" + reader["f_No"].ToString() + "," + wgTools.SetObjToStr(reader["f_Value"]) + ")";
                                if (wgTools.SetObjToStr(reader["f_Value"]) == "1")
                                {
                                    str10 = str10 + reader["f_No"].ToString() + ";";
                                }
                            }
                            if (!string.IsNullOrEmpty(str10))
                            {
                                str = str + "\r\n【已启用】：" + str10;
                            }
                        }
                        reader.Close();
                    }
                }
                using (OleDbConnection connection6 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command6 = new OleDbCommand("", connection6))
                    {
                        connection6.Open();
                        command6.CommandText = "SELECT f_No, f_Value FROM t_a_SystemParam  WHERE f_NO<100  ORDER BY f_No Asc ";
                        reader = command6.ExecuteReader();
                        if (reader.HasRows)
                        {
                            str = str + "\r\n【参数值(No,Val)】：\r\n";
                            while (reader.Read())
                            {
                                string str13 = str;
                                str = str13 + " (" + reader["f_No"].ToString() + "," + wgTools.SetObjToStr(reader["f_Value"]) + ")";
                            }
                        }
                        reader.Close();
                    }
                }
                if (!string.IsNullOrEmpty(str4))
                {
                    str = str + "\r\n【DrvInfo】：\r\n";
                    str = str + str4;
                }
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj2 in searcher.Get())
                    {
                        str = str + string.Format("\r\n【{0}】： ", CommonStr.strSystem);
                        str = str + string.Format("\r\n{0} ", obj2["Caption"]);
                        str = str + string.Format("\r\n{0} ", obj2["version"].ToString());
                        str = str + string.Format("\r\n{0} ", obj2["CSDVersion"]);
                    }
                }
                str = str + string.Format("\r\n【数据库版本】： ", new object[0]);
                string str11 = "Microsoft Access";
                if (!string.IsNullOrEmpty(str11))
                {
                    str = str + string.Format("\r\n{0}", str11);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            str = (str + "\r\n---------------------------------------------" + "\r\n") + "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
            subject = str2;
            return str;
        }
    }
}

