namespace WG3000_COMM.Core
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class DatatableToXml
    {
        private static MemoryStream ms;

        public static string CDataToXml(DataSet ds)
        {
            return CDataToXml(ds, -1);
        }

        public static string CDataToXml(DataTable dt)
        {
            if (dt != null)
            {
                try
                {
                    using (ms = new MemoryStream())
                    {
                        using (XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode))
                        {
                            dt.WriteXml(writer);
                            int length = (int) ms.Length;
                            byte[] buffer = new byte[length];
                            ms.Seek(0L, SeekOrigin.Begin);
                            ms.Read(buffer, 0, length);
                            UnicodeEncoding encoding = new UnicodeEncoding();
                            return encoding.GetString(buffer).Trim();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return "";
        }

        public static string CDataToXml(DataView dv)
        {
            return CDataToXml(dv.Table);
        }

        public static string CDataToXml(DataSet ds, int tableIndex)
        {
            if (tableIndex != -1)
            {
                return CDataToXml(ds.Tables[tableIndex]);
            }
            return CDataToXml(ds.Tables[0]);
        }

        public static bool CDataToXmlFile(DataSet ds, string xmlFilePath)
        {
            return CDataToXmlFile(ds, -1, xmlFilePath);
        }

        public static bool CDataToXmlFile(DataTable dt, string xmlFilePath)
        {
            if ((dt != null) && !string.IsNullOrEmpty(xmlFilePath))
            {
                string path = xmlFilePath;
                try
                {
                    using (ms = new MemoryStream())
                    {
                        using (XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode))
                        {
                            dt.WriteXml(writer);
                            int length = (int) ms.Length;
                            byte[] buffer = new byte[length];
                            ms.Seek(0L, SeekOrigin.Begin);
                            ms.Read(buffer, 0, length);
                            UnicodeEncoding encoding = new UnicodeEncoding();
                            using (StreamWriter writer2 = new StreamWriter(path))
                            {
                                writer2.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                                writer2.WriteLine(encoding.GetString(buffer).Trim());
                                return true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        public static bool CDataToXmlFile(DataView dv, string xmlFilePath)
        {
            return CDataToXmlFile(dv.Table, xmlFilePath);
        }

        public static bool CDataToXmlFile(DataSet ds, int tableIndex, string xmlFilePath)
        {
            if (tableIndex != -1)
            {
                return CDataToXmlFile(ds.Tables[tableIndex], xmlFilePath);
            }
            return CDataToXmlFile(ds.Tables[0], xmlFilePath);
        }
    }
}

