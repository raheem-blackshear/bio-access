namespace WG3000_COMM.Core
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public class wgTools
    {
        public const int COMM_UDP_PORT = 0xea60;
        private static DateTime dtLast = DateTime.Now;
        public const int ERR_PRIVILEGES_OVER200K = -100001;
        public const int ERR_PRIVILEGES_STOPUPLOAD = -100002;
        public const int ERR_SWIPERECORD_STOPGET = -200002;
        public static bool IsSqlServer = true;
        private static string m_CommPasswordStr = "";
        private static string m_DisplayFormat_DateYMD = "yyyy-MM-dd";
        private static string m_DisplayFormat_DateYMDHMS = "yyyy-MM-dd HH:mm:ss";
        private static string m_DisplayFormat_DateYMDHMSWeek = "yyyy-MM-dd HH:mm:ss dddd";
        private static string m_DisplayFormat_DateYMDWeek = "yyyy-MM-dd dddd";
        private static int m_gPTC = 0;
        private static string m_MSGTITLE = "";
        private static string m_YMDHMSFormat = "yyyy-MM-dd HH:mm:ss";
        public const int PRIVILEGES_UPLOADED = 0x186a3;
        public const int PRIVILEGES_UPLOADING = 0x186a2;
        public const int PRIVILEGES_UPLOADPREPARING = 0x186a1;

        private wgTools()
        {
        }

        public static int CmpProductVersion(string newVersion, string productVersion)
        {
            try
            {
                if (SetObjToStr(newVersion) == SetObjToStr(productVersion))
                {
                    return 0;
                }
                if (SetObjToStr(newVersion) != "")
                {
                    if (SetObjToStr(productVersion) == "")
                    {
                        return 1;
                    }
                    if (newVersion == productVersion)
                    {
                        return 0;
                    }
                    string[] strArray = newVersion.Split(new char[] { '.' });
                    string[] strArray2 = productVersion.Split(new char[] { '.' });
                    for (int i = 0; i < Math.Min(strArray.Length, strArray2.Length); i++)
                    {
                        if (int.Parse(strArray[i]) > int.Parse(strArray2[i]))
                        {
                            return 1;
                        }
                        if (int.Parse(strArray[i]) < int.Parse(strArray2[i]))
                        {
                            return -1;
                        }
                    }
                    if (strArray.Length == strArray2.Length)
                    {
                        return 0;
                    }
                    if (strArray.Length > strArray2.Length)
                    {
                        return 1;
                    }
                }
                return -1;
            }
            catch (Exception exception)
            {
                WriteLine("cmpProductVersion" + exception.ToString());
            }
            return -2;
        }

        public static void Compress(FileInfo fi)
        {
            using (FileStream stream = fi.OpenRead())
            {
                if (((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden) & (fi.Extension != ".gz"))
                {
                    using (FileStream stream2 = File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream stream3 = new GZipStream(stream2, CompressionMode.Compress))
                        {
                            CopyStream(stream, stream3);
                        }
                    }
                }
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[0x8000];
            while (true)
            {
                int count = input.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    return;
                }
                output.Write(buffer, 0, count);
            }
        }

        public static void Decompress(FileInfo fi)
        {
            using (FileStream stream = fi.OpenRead())
            {
                string fullName = fi.FullName;
                using (FileStream stream2 = File.Create(fullName.Remove(fullName.Length - fi.Extension.Length)))
                {
                    using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        CopyStream(stream3, stream2);
                    }
                }
            }
        }

        private static bool i2cchecktime(int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            return ((((Second <= 0x3b) && (Minute <= 0x3b)) && ((Hour <= 0x17) && (Day <= 0x1f))) && (((Day != 0) && (Month <= 12)) && (((Month != 0) && (Year <= 0x63)) && (Year != 0))));
        }

        public static bool IsAllFF(byte[] arrbyt, int startIndex, int len)
        {
            if ((startIndex >= arrbyt.Length) || ((startIndex + len) > arrbyt.Length))
            {
                return false;
            }
            for (int i = startIndex; i < (startIndex + len); i++)
            {
                if (arrbyt[i] != 0xff)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidDateTimeFormat(string dateTimeFormat)
        {
            bool flag = false;
            try
            {
                DateTime time;
                if (string.IsNullOrEmpty(dateTimeFormat))
                {
                    return true;
                }
                if (DateTime.TryParse(DateTime.Now.ToString(dateTimeFormat), out time))
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        internal static uint MsDateToWgDate(DateTime dt)
        {
            int num = (dt.Year % 100) << 9;
            num += dt.Month << 5;
            num += dt.Day;
            num += (dt.Second >> 1) << 0x10;
            num += dt.Minute << 0x15;
            num += dt.Hour << 0x1b;
            return (uint) num;
        }

        internal static uint MsDateToWgDateHMS(DateTime dt)
        {
            int num = 0;
            num += dt.Second >> 1;
            num += dt.Minute << 5;
            num += dt.Hour << 11;
            return (uint) num;
        }

        internal static uint MsDateToWgDateYMD(DateTime dt)
        {
            int num = (dt.Year % 100) << 9;
            num += dt.Month << 5;
            num += dt.Day;
            return (uint) num;
        }

        public static string PrepareStr(object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            if (obj == DBNull.Value)
            {
                return "NULL";
            }
            if (obj.ToString().Trim() == "")
            {
                return "NULL";
            }
            string str = obj.ToString();
            if (IsSqlServer)
            {
                str = obj.ToString().Replace("'", "''");
                return ("'" + str + "'");
            }
            str = obj.ToString().Replace("'", "''");
            return ("'" + str + "'");
        }

        public static string PrepareStr(object obj, bool bDate, string dateFormat)
        {
            if (obj == null)
            {
                return "NULL";
            }
            if (obj == DBNull.Value)
            {
                return "NULL";
            }
            if (obj.ToString().Trim() == "")
            {
                return "NULL";
            }
            string str = obj.ToString();
            bool flag = false;
            flag = bDate;
            string format = dateFormat;
            if (flag)
            {
                if (IsSqlServer)
                {
                    if (format == "")
                    {
                        return ("CONVERT( datetime, '" + str.Trim() + "')");
                    }
                    return ("CONVERT( datetime, '" + DateTime.Parse(obj.ToString()).ToString(format) + "')");
                }
                if (format == "")
                {
                    return ("#" + str.Trim() + "#");
                }
                return ("#" + DateTime.Parse(obj.ToString()).ToString(format) + "#");
            }
            if (!IsSqlServer)
            {
                return ("'" + str + "'");
            }
            if (format == "")
            {
                str = obj.ToString().Replace("'", "''");
                return ("'" + str.Trim() + "'");
            }
            return ("'" + DateTime.Parse(obj.ToString()).ToString(format) + "'");
        }

        public static string ReadTextFile(string path)
        {
            using (StreamReader reader = new StreamReader(path, new UnicodeEncoding()))
            {
                return reader.ReadToEnd();
            }
        }

        public static string SetObjToStr(object valA)
        {
            string str = "";
            try
            {
                if ((valA != null) && (DBNull.Value != valA))
                {
                    str = valA.ToString().Trim();
                }
            }
            catch (Exception)
            {
            }
            return str;
        }

        public static long SignedCard(long CardNo)
        {
            if (CardNo >= 0xffffffffL)
            {
                return (CardNo - 0x100000000L);
            }
            return CardNo;
        }

        public static long UnsignedCard(long CardNo)
        {
            if (CardNo < 0L)
            {
                return (CardNo + 0x100000000L);
            }
            return CardNo;
        }

        internal static DateTime WgDateToMsDate(byte[] dts, int StartIndex)
        {
            ushort num = BitConverter.ToUInt16(dts, StartIndex);
            ushort num2 = BitConverter.ToUInt16(dts, StartIndex + 2);
            int second = (num2 & 0x1f) << 1;
            int minute = (num2 >> 5) & 0x3f;
            int hour = num2 >> 11;
            int day = num & 0x1f;
            int month = (num >> 5) & 15;
            int year = num >> 9;
            try
            {
                if (i2cchecktime(year, month, day, hour, minute, second))
                {
                    return new DateTime(year + 0x7d0, month, day, hour, minute, second);
                }
                return new DateTime(0x7d9, 1, 1, 0, 0, 0);
            }
            catch (Exception)
            {
                return new DateTime(0x7d9, 1, 1, 0, 0, 0);
            }
        }

        public static void WgDebugWrite(string info, params object[] args)
        {
            try
            {
            }
            catch
            {
            }
        }

        public static void WriteLine(string info)
        {
            dtLast = DateTime.Now;
        }

        public static string CommPStr
        {
            get
            {
                return m_CommPasswordStr;
            }
            set
            {
                if (value != null)
                {
                    m_CommPasswordStr = value;
                }
            }
        }

        public static string DisplayFormat_DateYMD
        {
            get
            {
                return m_DisplayFormat_DateYMD;
            }
            set
            {
                m_DisplayFormat_DateYMD = value;
            }
        }

        public static string DisplayFormat_DateYMDHMS
        {
            get
            {
                return m_DisplayFormat_DateYMDHMS;
            }
            set
            {
                m_DisplayFormat_DateYMDHMS = value;
            }
        }

        public static string DisplayFormat_DateYMDHMSWeek
        {
            get
            {
                return m_DisplayFormat_DateYMDHMSWeek;
            }
            set
            {
                m_DisplayFormat_DateYMDHMSWeek = value;
            }
        }

        public static string DisplayFormat_DateYMDWeek
        {
            get
            {
                return m_DisplayFormat_DateYMDWeek;
            }
            set
            {
                m_DisplayFormat_DateYMDWeek = value;
            }
        }

        public static bool gADCT
        {
            get
            {
                return (gPTC_internal == 1);
            }
        }

        internal static bool gADCT_internal
        {
            get
            {
                return (gPTC_internal == 1);
            }
        }

        public static int gate
        {
            get
            {
                return m_gPTC;
            }
            set
            {
                m_gPTC = value;
            }
        }

        public static string gPTC
        {
            set
            {
                try
                {
                    m_gPTC = int.Parse(WGPacket.Dpt(value));
                }
                catch
                {
                }
            }
        }

        internal static int gPTC_internal
        {
            get
            {
                return m_gPTC;
            }
        }

        public static string MSGTITLE
        {
            get
            {
                return m_MSGTITLE;
            }
        }

        public static string YMDHMSFormat
        {
            get
            {
                return m_YMDHMSFormat;
            }
            set
            {
                m_YMDHMSFormat = value;
            }
        }

        public static class ErrorCode
        {
            public const int ERR_SUCCESS = 0;
            public const int ERR_PARAM = -1;
            public const int ERR_NOT_ENROLLED_POS = -2;
            public const int ERR_BAD_FINGER = -3;
            public const int ERR_MERGE = -4;
            public const int ERR_IDENTIFY = -5;
            public const int ERR_VERIFY = -6;
            public const int ERR_SENSOR = -7;
            public const int ERR_NOT_PRESSED = -8;
            public const int DEV_ERR = -10;
            public const int ERR_NO_FACE = -11;

            public const int ERR_FAIL = -20;	/* compensate macro */
            public const int ERR_CAP_TIMEOUT = -21;
            public const int ERR_NTH_ERR = -22;
            public const int ERR_FINGER_DOUBLED = -23;
            public const int ERR_ENROLLED_POS = -24;
            public const int ERR_INVALID_ID = -25;
            public const int ERR_NOT_ENROLLED_ID = -26;
            public const int ERR_FULL_ID_FP = -27;
            public const int ERR_INVALID_POS = -28;
            public const int ERR_DB_IS_FULL = -29;
            public const int ERR_CARD_DOUBLED = -30;
            public const int ERR_TIMEOUT = -31;
            public const int ERR_INVALID_PARAMETER = -32;
            public const int ERR_LOG_NOLOG = -40;		// no log to get
            public const int ERR_LOG_END = -41;		// log read all
            public const int ERR_FACE_DOUBLED = -42;

            public const int ERR_FW_CANT_OPEN = -50;
            public const int ERR_FW_TO0_LARGE = -51;
            public const int ERR_FW_BAD_FILE = -52;

			public const int ERR_OUT_OF_MEMORY = -60;
			public const int ERR_GRAY_NOT_CREATED = -61;
			public const int ERR_FACE_NOT_DETECTED = -62;
			public const int ERR_EYES_NOT_DETECTED = -63;
			public const int ERR_DETAILS_NOT_DETECTED = -64;
			public const int ERR_FEATURES_NOT_EXTRACTED = -65;
			public const int ERR_TEMPLATE_NOT_EXTRACTED = -66;
			public const int ERR_DONT_MOVE_FACE = -67;

            public const int ERR_CANT_OPEN_COMM = -100;
            public const int COMM_SESSION_SEND_ERROR = -101;
            public const int COMM_SESSION_RECV_ERROR = -102;
            public const int ERR_CANT_OPEN_COMM_BUSY = -103;
            public const int ERR_UNKNOWN = -104;
        }
    }
}

