namespace WG3000_COMM.Core
{
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WG3000_COMM.ResStrings;

    public class wgRunInfoLog
    {
        public static int logRecEventMode = 0;
        public static DataTable m_dt;

        public static void addEvent(InfoRow newInfo)
        {
            if (m_dt != null)
            {
                m_dt.AcceptChanges();
                DataRow row = m_dt.NewRow();
                row[0] = newInfo.category;
                row[1] = m_dt.Rows.Count + 1;
                row[2] = DateTime.Now.ToString("HH:mm:ss");
                row[3] = newInfo.desc;
                row[4] = newInfo.information;
                row[5] = newInfo.detail;
                row[6] = newInfo.MjRecStr;
                object[] args = new object[] { (m_dt.Rows.Count + 1).ToString(), newInfo.desc, newInfo.information, newInfo.detail, newInfo.MjRecStr };
                wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", args));
                m_dt.Rows.Add(row);
                m_dt.AcceptChanges();
            }
        }

        public static void addEventNotConnect(int ControllerSN, string IP, ListViewItem itm)
        {
            if (itm != null)
            {
                InfoRow newInfo = new InfoRow();
                newInfo.category = 0x65;
                newInfo.desc = itm.Text;
                newInfo.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[] { CommonStr.strCommFail, CommonStr.strControllerSN, ControllerSN, IP });
                newInfo.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[] { itm.Text, CommonStr.strCommFail, CommonStr.strControllerSN, ControllerSN, IP });
                addEvent(newInfo);
                itm.ImageIndex = 3;
            }
        }

        public static void addEventNotConnect(int ControllerSN, string IP, ListViewItem itm, int errorCode)
        {
            if (itm != null)
            {
                InfoRow newInfo = new InfoRow();
                newInfo.category = 0x65;
                newInfo.desc = itm.Text;
                string info;
                switch (errorCode)
                {
                    case wgTools.ErrorCode.ERR_DB_IS_FULL:
                    case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                    case wgTools.ErrorCode.ERR_FAIL:
                        info = wgGlobal.getErrorString(errorCode);
                        break;
                    default:
                        if (errorCode <= wgTools.ErrorCode.ERR_UNKNOWN)
                        {
                            int err = wgTools.ErrorCode.ERR_UNKNOWN - errorCode;
                            int finger = err >> 28;
                            int uid = err & 0xFFFFFFF;
                            info = wgGlobal.getErrorString(wgTools.ErrorCode.ERR_FINGER_DOUBLED) + " " +
                                CommonStr.strUserID + ":" + uid.ToString() + " " +
                                CommonStr.strVerifFinger + ":" + (finger + 1).ToString();
                        }
                        else
                        {
                            info = CommonStr.strCommFail;
                        }
                        itm.ImageIndex = 3;
                        break;
                }
                newInfo.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[] { info, CommonStr.strControllerSN, ControllerSN, IP });
                newInfo.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[] { itm.Text, info, CommonStr.strControllerSN, ControllerSN, IP });
                addEvent(newInfo);
            }
        }

        public static void addEventSpecial1(InfoRow newInfo)
        {
            if (m_dt != null)
            {
                DataRow row = m_dt.NewRow();
                row[0] = newInfo.category;
                row[1] = m_dt.Rows.Count + 1;
                row[2] = DateTime.Now.ToString("HH:mm:ss");
                row[3] = newInfo.desc;
                row[4] = newInfo.information;
                row[5] = newInfo.detail;
                row[6] = newInfo.MjRecStr;
                wgTools.WriteLine("wgRunInfoLog.addEventSpecial1   dr[6]");
                object[] args = new object[] { (m_dt.Rows.Count + 1).ToString(), newInfo.desc, newInfo.information, newInfo.detail, newInfo.MjRecStr };
                wgAppConfig.wgLog(string.Format("{0},{1},{2},{3},{4}", args));
                m_dt.Rows.Add(row);
            }
        }

        public static void addEventSpecial2()
        {
            if (m_dt != null)
            {
                m_dt.AcceptChanges();
            }
        }

        public static void init(out DataTable dt)
        {
            dt = new DataTable();
            dt.TableName = "runInfolog";
            dt.Columns.Add("f_Category");
            dt.Columns.Add("f_RecID", System.Type.GetType("System.UInt32"));
            dt.Columns.Add("f_Time");
            dt.Columns.Add("f_Desc");
            dt.Columns.Add("f_Info");
            dt.Columns.Add("f_Detail");
            dt.Columns.Add("f_MjRecStr");
            dt.AcceptChanges();
            m_dt = dt;
            try
            {
                logRecEventMode = 0;
                int.TryParse(wgAppConfig.GetKeyVal("logRecEventMode"), out logRecEventMode);
            }
            catch (Exception)
            {
            }
        }
    }
}

