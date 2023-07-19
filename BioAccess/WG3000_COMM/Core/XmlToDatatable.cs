namespace WG3000_COMM.Core
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;

    public class XmlToDatatable
    {
        private static StringReader StrStream;

        public static DataSet CXmlFileToDataSet(string xmlFilePath)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                string filename = xmlFilePath;
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    using (DataSet set = new DataSet())
                    {
                        using (StrStream = new StringReader(document.InnerXml))
                        {
                            using (XmlTextReader reader = new XmlTextReader(StrStream))
                            {
                                set.ReadXml(reader);
                                return set;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        public static DataSet CXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                try
                {
                    if (xmlStr.Substring(0, 2) == "?<")
                    {
                        xmlStr = xmlStr.Substring(1);
                    }
                    using (DataSet set = new DataSet())
                    {
                        using (StrStream = new StringReader(xmlStr))
                        {
                            using (XmlTextReader reader = new XmlTextReader(StrStream))
                            {
                                set.ReadXml(reader);
                                return set;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        public static DataTable CXmlToDataTable(string xmlFilePath)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[0];
        }

        public static DataTable CXmlToDataTable(string xmlFilePath, int tableIndex)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[tableIndex];
        }

        public static DataTable CXmlToDatatTable(string xmlStr)
        {
            return CXmlToDataSet(xmlStr).Tables[0];
        }

        public static DataTable CXmlToDatatTable(string xmlStr, int tableIndex)
        {
            return CXmlToDataSet(xmlStr).Tables[tableIndex];
        }
    }
}

