namespace WG3000_COMM.CoreAA
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal class wgToolsAA
    {
        private static string defaultCustConfigzhCHS = "<NewDataSet>\r\n  <xs:schema id=\"NewDataSet\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n    <xs:element name=\"NewDataSet\" msdata:IsDataSet=\"true\" msdata:MainDataTable=\"appSettings\" msdata:UseCurrentLocale=\"true\">\r\n      <xs:complexType>\r\n        <xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">\r\n          <xs:element name=\"appSettings\">\r\n            <xs:complexType>\r\n              <xs:sequence>\r\n                <xs:element name=\"key\" type=\"xs:string\" minOccurs=\"0\" />\r\n                <xs:element name=\"value\" type=\"xs:string\" minOccurs=\"0\" />\r\n              </xs:sequence>\r\n            </xs:complexType>\r\n          </xs:element>\r\n        </xs:choice>\r\n      </xs:complexType>\r\n    </xs:element>\r\n  </xs:schema>\r\n  <appSettings>\r\n    <key>dbConnection</key>\r\n    <value/>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>Language</key>\r\n    <value>zh-CHS</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>autologinName</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>autologinPassword</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>rgtries</key>\r\n    <value>1</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>NewSoftwareVersionInfo</key>\r\n    <value>1.0.2</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>RunTimes</key>\r\n    <value></value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>NewSoftwareSpecialVersionInfo</key>\r\n    <value>1.0.2</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>CommCurrent</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>RunTimeAt</key>\r\n    <value>0</value>\r\n  </appSettings>\r\n</NewDataSet>";
        public static DateTime dtLast = DateTime.Now;
        public static bool gADCT = true;
        public const string gc_EventSourceName = "BioAccess";
        private static string m_CultureInfoStr = "";
        private static string m_dbConString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ADCT3000;Data Source=(local) ";
        private static string m_MSGTITLE = "";
        private static string m_YMDFormat = "yyyy-MM-dd";
        private static string m_YMDHMSFormat = "yyyy-MM-dd HH:mm:ss";
        private const string n3k_cust = @"\BioAccess.xml";
        private static int tryCreateCnt = 0;

        private wgToolsAA()
        {
        }

        public static void CardIDInput(ref MaskedTextBox mtb)
        {
            if (mtb.Text.Length != mtb.Text.Trim().Length)
            {
                mtb.Text = mtb.Text.Trim();
            }
            else if ((mtb.Text.Length == 0) && (mtb.SelectionStart != 0))
            {
                mtb.SelectionStart = 0;
            }
            if (mtb.Text.Length > 0)
            {
                if (mtb.Text.IndexOf(" ") > 0)
                {
                    mtb.Text = mtb.Text.Replace(" ", "");
                }
                if ((mtb.Text.Length > 9) && (long.Parse(mtb.Text) >= 0xffffffffL))
                {
                    mtb.Text = mtb.Text.Substring(0, mtb.Text.Length - 1);
                }
            }
        }

        public static int cmpProductVersion(string newVersion, string productVersion)
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
                writeLine("cmpProductVersion" + exception.ToString());
            }
            return -2;
        }

        public static bool CreateCustXml()
        {
            string startupPath = Application.StartupPath;
            string path = startupPath + @"\BioAccess.xml";
            if (File.Exists(path))
            {
                tryCreateCnt = 0;
                return true;
            }
            if (tryCreateCnt <= 5)
            {
                string str3 = startupPath + @"\photo\BioAccess.xmlAA";
                string defaultCustConfigzhCHS = wgToolsAA.defaultCustConfigzhCHS;
                if (!IsChineseSet(Thread.CurrentThread.CurrentUICulture.Name))
                {
                    defaultCustConfigzhCHS = defaultCustConfigzhCHS.Replace("zh-CHS", "en");
                }
                if (File.Exists(str3))
                {
                    using (StreamReader reader = new StreamReader(str3))
                    {
                        string str5 = reader.ReadToEnd();
                        if (str5.Length > 0x3e8)
                        {
                            defaultCustConfigzhCHS = str5;
                        }
                    }
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.WriteLine(defaultCustConfigzhCHS);
                }
                tryCreateCnt++;
                if (File.Exists(path))
                {
                    tryCreateCnt = 0;
                    return true;
                }
            }
            return false;
        }

        public static void deselectObject(DataGridView dgv)
        {
            try
            {
                int rowIndex;
                DataRow row;
                if (dgv.SelectedRows.Count <= 0)
                {
                    if (dgv.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgv.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgv.SelectedRows[0].Index;
                }
                DataTable table = ((DataView) dgv.DataSource).Table;
                if (dgv.SelectedRows.Count > 0)
                {
                    int count = dgv.SelectedRows.Count;
                    int[] numArray = new int[count];
                    for (int i = 0; i < dgv.SelectedRows.Count; i++)
                    {
                        numArray[i] = (int) dgv.SelectedRows[i].Cells[0].Value;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        int key = numArray[j];
                        row = table.Rows.Find(key);
                        if (row != null)
                        {
                            row["f_Selected"] = 0;
                        }
                    }
                }
                else
                {
                    int num6 = (int) dgv.Rows[rowIndex].Cells[0].Value;
                    row = table.Rows.Find(num6);
                    if (row != null)
                    {
                        row["f_Selected"] = 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool FileIsExisted(string strFileName)
        {
            bool flag = false;
            try
            {
                if (string.IsNullOrEmpty(strFileName))
                {
                    return flag;
                }
                if (string.IsNullOrEmpty(strFileName.Trim()))
                {
                    return flag;
                }
                string fileName = strFileName.Trim();
                if (((fileName.Length > 2) && !(fileName.Substring(0, 2) == @"\")) && (fileName.IndexOf(":") <= 0))
                {
                    fileName = Application.StartupPath + @"\" + fileName;
                }
                FileInfo info = new FileInfo(fileName);
                if (!info.Exists)
                {
                    return flag;
                }
                if ((info.Extension.ToUpper() == ".JPG") || (info.Extension.ToUpper() == ".BMP"))
                {
                    if (info.Length > 0x800L)
                    {
                        flag = true;
                    }
                    return flag;
                }
                if (info.Extension.ToUpper() == ".MP4")
                {
                    if (info.Length > 0x2800L)
                    {
                        flag = true;
                    }
                    return flag;
                }
                if (info.Length > 0L)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        public static void fillDGVData(ref DataGridView dgv, string strSql)
        {
            SqlCommand selectCommand = new SqlCommand();
            SqlConnection connection = new SqlConnection(dbConString);
            selectCommand = new SqlCommand(strSql, connection);
            new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter = new SqlDataAdapter(selectCommand);
            DataTable table = new DataTable();
            DataView view = new DataView(table);
            adapter.Fill(table);
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = view;
            for (int i = 0; i < view.Table.Columns.Count; i++)
            {
                dgv.Columns[i].DataPropertyName = view.Table.Columns[i].ColumnName;
            }
        }

        public static void GetAppIcon(ref Icon appicon)
        {
            try
            {
                appicon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
            }
        }

        public static string GetKeyVal(string key)
        {
            string str = "";
            try
            {
                string path = Application.StartupPath + @"\BioAccess.xml";
                if (!File.Exists(path))
                {
                    CreateCustXml();
                }
                if (File.Exists(path))
                {
                    DataTable table = new DataTable();
                    table.TableName = "appSettings";
                    table.Columns.Add("key");
                    table.Columns.Add("value");
                    table.ReadXml(path);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["key"].ToString() == key)
                        {
                            return row["value"].ToString();
                        }
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
            return str;
        }

        private static string getSystemParam(int parNo, string parName)
        {
            string str = "";
            try
            {
                string str2;
                if (string.IsNullOrEmpty(parName))
                {
                    str2 = "SELECT f_Value FROM t_a_SystemParam WHERE f_NO=" + parNo.ToString();
                }
                else
                {
                    str2 = "SELECT f_Value FROM t_a_SystemParam WHERE f_Name=" + PrepareStr(parName);
                }
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    str = SetObjToStr(new SqlCommand(str2, connection).ExecuteScalar());
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
            return str;
        }

        public static string getSystemParamByName(string parName)
        {
            return getSystemParam(-1, parName);
        }

        public static string getSystemParamByNO(int parNo)
        {
            return getSystemParam(parNo, "");
        }

        public static int getSystemParamValue(int NO, out string EName, out string value, out string notes)
        {
            SqlConnection connection = new SqlConnection(dbConString);
            int num = -9;
            EName = null;
            value = null;
            notes = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlDataReader reader = new SqlCommand("SELECT * FROM t_a_SystemParam WHERE f_NO = " + NO.ToString(), connection).ExecuteReader();
                if (reader.Read())
                {
                    EName = reader["f_EName"] as string;
                    value = reader["f_Value"] as string;
                    notes = reader["f_Notes"] as string;
                    num = 1;
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return num;
        }

        private static bool i2cchecktime(int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            return ((((Second <= 0x3b) && (Minute <= 0x3b)) && ((Hour <= 0x17) && (Day <= 0x1f))) && (((Day != 0) && (Month <= 12)) && (((Month != 0) && (Year <= 0x63)) && (Year != 0))));
        }

        public static void InsertKeyVal(string key, string value)
        {
            bool flag = false;
            try
            {
                string path = Application.StartupPath + @"\BioAccess.xml";
                if (File.Exists(path))
                {
                    DataTable table = new DataTable();
                    table.TableName = "appSettings";
                    table.Columns.Add("key");
                    table.Columns.Add("value");
                    table.ReadXml(path);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["key"].ToString() == key)
                        {
                            return;
                        }
                    }
                    if (!flag)
                    {
                        DataRow row2 = table.NewRow();
                        row2["key"] = key;
                        row2["value"] = value;
                        table.Rows.Add(row2);
                        table.AcceptChanges();
                    }
                    StringWriter writer = new StringWriter();
                    writer = new StringWriter();
                    table.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                    using (StreamWriter writer2 = new StreamWriter(path, false))
                    {
                        writer2.Write(writer.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
        }

        public static bool IsChineseSet(string cultureInfo)
        {
            bool flag = false;
            try
            {
                if (string.IsNullOrEmpty(cultureInfo) || (!(cultureInfo == "zh") && (cultureInfo.IndexOf("zh-") != 0)))
                {
                    return flag;
                }
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static uint msDateToWgDate(DateTime dt)
        {
            int num = (dt.Year % 100) << 9;
            num += dt.Month << 5;
            num += dt.Day;
            num += (dt.Second >> 1) << 0x10;
            num += dt.Minute << 0x15;
            num += dt.Hour << 0x1b;
            return (uint) num;
        }

        public static uint msDateToWgDateYMD(DateTime dt)
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

        public static void ReadGVStyle(Form form, DataGridView dgv)
        {
            try
            {
                string str = Application.StartupPath + @"\XML\";
                string path = str + form.Name + "_" + dgv.Name + ".xml";
                if (CultureInfoStr == "")
                {
                    path = string.Format("{0}{1}_{2}.xml", str, form.Name, dgv.Name);
                }
                else
                {
                    path = string.Format("{0}{1}_{2}.{3}.xml", new object[] { str, form.Name, dgv.Name, CultureInfoStr });
                }
                if (File.Exists(path))
                {
                    DataTable table = new DataTable();
                    table.TableName = dgv.Name;
                    table.Columns.Add("colName");
                    table.Columns.Add("colHeader");
                    table.Columns.Add("colWidth");
                    table.Columns.Add("colVisable");
                    table.Columns.Add("colDisplayIndex");
                    table.ReadXml(path);
                    foreach (DataRow row in table.Rows)
                    {
                        dgv.Columns[row["colName"].ToString()].HeaderText = row["colHeader"].ToString();
                        dgv.Columns[row["colName"].ToString()].Width = int.Parse(row["colWidth"].ToString());
                        dgv.Columns[row["colName"].ToString()].Visible = bool.Parse(row["colVisable"].ToString());
                        dgv.Columns[row["colName"].ToString()].DisplayIndex = int.Parse(row["colDisplayIndex"].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
        }

        public static string ReadTextFile(string path)
        {
            using (StreamReader reader = new StreamReader(path, new UnicodeEncoding()))
            {
                return reader.ReadToEnd();
            }
        }

        public static int runUpdateSql(string strSql)
        {
            int num = -1;
            if (!string.IsNullOrEmpty(strSql))
            {
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    num = new SqlCommand(strSql, connection).ExecuteNonQuery();
                    connection.Close();
                }
            }
            return num;
        }

        public static void SaveDGVStyle(Form form, DataGridView dgv)
        {
            try
            {
                string str2;
                string str = Application.StartupPath + @"\XML\";
                if (CultureInfoStr == "")
                {
                    str2 = string.Format("{0}{1}_{2}.xml", str, form.Name, dgv.Name);
                }
                else
                {
                    str2 = string.Format("{0}{1}_{2}.{3}.xml", new object[] { str, form.Name, dgv.Name, CultureInfoStr });
                }
                DataSet set = new DataSet("DGV_STILE");
                DataTable table = new DataTable();
                set.Tables.Add(table);
                table.TableName = dgv.Name;
                table.Columns.Add("colName");
                table.Columns.Add("colHeader");
                table.Columns.Add("colWidth");
                table.Columns.Add("colVisable");
                table.Columns.Add("colDisplayIndex");
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    DataRow row = table.NewRow();
                    row["colName"] = column.Name;
                    row["colHeader"] = column.HeaderText;
                    row["colWidth"] = column.Width;
                    row["colVisable"] = column.Visible;
                    row["colDisplayIndex"] = column.DisplayIndex;
                    table.Rows.Add(row);
                    table.AcceptChanges();
                }
                StringWriter writer = new StringWriter();
                writer = new StringWriter();
                table.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                using (StreamWriter writer2 = new StreamWriter(str2, false))
                {
                    writer2.Write(writer.ToString());
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
        }

        public static void selectObject(DataGridView dgv)
        {
            selectObject(dgv, "", "");
        }

        public static void selectObject(DataGridView dgv, string secondField, string val)
        {
            try
            {
                int rowIndex;
                DataRow row;
                if (dgv.SelectedRows.Count <= 0)
                {
                    if (dgv.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgv.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgv.SelectedRows[0].Index;
                }
                DataTable table = ((DataView) dgv.DataSource).Table;
                if (dgv.SelectedRows.Count > 0)
                {
                    int count = dgv.SelectedRows.Count;
                    int[] numArray = new int[count];
                    for (int i = 0; i < dgv.SelectedRows.Count; i++)
                    {
                        numArray[i] = (int) dgv.SelectedRows[i].Cells[0].Value;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        int key = numArray[j];
                        row = table.Rows.Find(key);
                        if (row != null)
                        {
                            row["f_Selected"] = 1;
                            if (secondField != "")
                            {
                                row[secondField] = val;
                            }
                        }
                    }
                }
                else
                {
                    int num6 = (int) dgv.Rows[rowIndex].Cells[0].Value;
                    row = table.Rows.Find(num6);
                    if (row != null)
                    {
                        row["f_Selected"] = 1;
                        if (secondField != "")
                        {
                            row[secondField] = val;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
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

        public static int setSystemParamValue(int NO, string EName, string value, string notes)
        {
            SqlConnection connection = new SqlConnection(dbConString);
            int num = -9;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string str = "UPDATE t_a_SystemParam SET [f_Value] = " + PrepareStr(value);
                if (!string.IsNullOrEmpty(EName))
                {
                    str = str + ", [f_EName] = " + PrepareStr(EName);
                }
                if (!string.IsNullOrEmpty(notes))
                {
                    str = str + ", [f_Notes] = " + PrepareStr(notes);
                }
                new SqlCommand(str + " WHERE f_NO = " + NO.ToString(), connection).ExecuteNonQuery();
                num = 1;
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return num;
        }

        public static void ShowMyImage(string fileToDisplay, ref Image img)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileToDisplay) && FileIsExisted(fileToDisplay))
                {
                    FileStream stream = new FileStream(fileToDisplay, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int) stream.Length);
                    stream.Close();
                    MemoryStream stream2 = new MemoryStream(buffer);
                    img = Image.FromStream(stream2);
                }
            }
            catch
            {
            }
        }

        public static int strToByteArr(string str, ref byte[] bytArr, int len)
        {
            int num = -1;
            try
            {
                int length = len;
                if (length < ((str.Length + 1) >> 1))
                {
                    length = (str.Length + 1) >> 1;
                }
                if (length < bytArr.Length)
                {
                    length = bytArr.Length;
                }
                if (length <= 0)
                {
                    return num;
                }
                for (int i = 0; i < length; i++)
                {
                    bytArr[i] = byte.Parse(str.Substring(1 + (2 * i), 2), NumberStyles.AllowHexSpecifier);
                }
                num = 0;
            }
            catch (Exception)
            {
                throw;
            }
            return num;
        }

        public static void UpdateKeyVal(string key, string value)
        {
            bool flag = false;
            try
            {
                string path = Application.StartupPath + @"\BioAccess.xml";
                if (!File.Exists(path))
                {
                    CreateCustXml();
                }
                if (File.Exists(path))
                {
                    DataTable table = new DataTable();
                    table.TableName = "appSettings";
                    table.Columns.Add("key");
                    table.Columns.Add("value");
                    table.ReadXml(path);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["key"].ToString() == key)
                        {
                            if (value == row["value"].ToString())
                            {
                                return;
                            }
                            row["value"] = value;
                            table.AcceptChanges();
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        DataRow row2 = table.NewRow();
                        row2["key"] = key;
                        row2["value"] = value;
                        table.Rows.Add(row2);
                        table.AcceptChanges();
                    }
                    StringWriter writer = new StringWriter();
                    writer = new StringWriter();
                    table.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                    using (StreamWriter writer2 = new StreamWriter(path, false))
                    {
                        writer2.Write(writer.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
        }

        public static DateTime wgDateToMsDate(byte[] dts, int StartIndex)
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

        public static void wgDebugWrite(string info)
        {
            wgLogWithoutDB(info, EventLogEntryType.Information, null);
        }

        public static void wgDebugWrite(string info, params object[] args)
        {
            try
            {
            }
            catch
            {
            }
        }

        public static void wgDebugWrite(string strMsg, EventLogEntryType entryType)
        {
            wgLogWithoutDB(strMsg, entryType, null);
        }

        public static void wgLogWithoutDB(string strMsg, EventLogEntryType entryType, byte[] rawData)
        {
            try
            {
                string str = strMsg;
                str = DateTime.Now.ToString("yyyy-MM-dd H-mm-ss") + "\t" + str;
                if (rawData != null)
                {
                    str = str + "\t:" + Encoding.ASCII.GetString(rawData);
                }
                using (StreamWriter writer = new StreamWriter(Application.StartupPath + @"\BioAccess.log", true))
                {
                    writer.WriteLine(str);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void writeLine(string info)
        {
            dtLast = DateTime.Now;
        }

        public static string CultureInfoStr
        {
            get
            {
                return m_CultureInfoStr;
            }
            set
            {
                if (value != null)
                {
                    m_CultureInfoStr = value;
                }
            }
        }

        public static string dbConString
        {
            get
            {
                return m_dbConString;
            }
            set
            {
                m_dbConString = value;
            }
        }

        public static bool IsSqlServer
        {
            get
            {
                return true;
            }
        }

        public static string MSGTITLE
        {
            get
            {
                return m_MSGTITLE;
            }
        }

        public static string YMDFormat
        {
            get
            {
                return m_YMDFormat;
            }
            set
            {
                m_YMDFormat = value;
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
    }
}

