namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ResStrings;

    internal class wgAppConfig
    {
        private static ArrayList arrPhotoFileFullNames = new ArrayList();
        public static bool bFloorRoomManager = false;
        private static Icon currenAppIcon = null;
        public static int dbCommandTimeout = 600;
        public const int dbControllerUserDefaultPassword = 0;
        private static string defaultCustConfigzhCHS = "<NewDataSet>\r\n  <xs:schema id=\"NewDataSet\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n    <xs:element name=\"NewDataSet\" msdata:IsDataSet=\"true\" msdata:MainDataTable=\"appSettings\" msdata:UseCurrentLocale=\"true\">\r\n      <xs:complexType>\r\n        <xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">\r\n          <xs:element name=\"appSettings\">\r\n            <xs:complexType>\r\n              <xs:sequence>\r\n                <xs:element name=\"key\" type=\"xs:string\" minOccurs=\"0\" />\r\n                <xs:element name=\"value\" type=\"xs:string\" minOccurs=\"0\" />\r\n              </xs:sequence>\r\n            </xs:complexType>\r\n          </xs:element>\r\n        </xs:choice>\r\n      </xs:complexType>\r\n    </xs:element>\r\n  </xs:schema>\r\n  <appSettings>\r\n    <key>dbConnection</key>\r\n    <value/>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>Language</key>\r\n    <value>zh-CHS</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>autologinName</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>autologinPassword</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>rgtries</key>\r\n    <value>1</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>NewSoftwareVersionInfo</key>\r\n    <value>1.0.2</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>RunTimes</key>\r\n    <value></value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>NewSoftwareSpecialVersionInfo</key>\r\n    <value>1.0.2</value>\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>CommCurrent</key>\r\n    <value />\r\n  </appSettings>\r\n  <appSettings>\r\n    <key>RunTimeAt</key>\r\n    <value>0</value>\r\n  </appSettings>\r\n</NewDataSet>";
        public static DateTime dtLast = DateTime.Now;
        private static DataView dv = null;
        public const string gc_EventSourceName = "BioAccess";
        public static bool gRestart = false;
        public static bool IsLogin = false;
        private static string lastPhotoDirectoryName = "";
        public static string LoginTitle = "";
        private static bool m_bCreatePhotoDirectory = false;
        private static bool m_bFindDirectoryNetShare = false;
        private static string m_CultureInfoStr = "";
        private static string m_dbConString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=AccessData;Data Source=(local) ";
        private static string m_dbName = "AccessData";
        private static string m_DirectoryNetShare = "";
        private static bool m_IsAccessDB = false;
        private static string m_PhotoDiriectyName = "";
        private const string n3k_cust = @"\BioAccess.xml";
        private static int photoDirectoryLastFileCount = -1;
        public static DateTime photoDirectoryLastWriteTime = new DateTime();
        public static string ProductTypeOfApp = "AccessControl";
        private static DataTable tb = null;
        private static int tryCreateCnt = 0;

        private wgAppConfig()
        {
        }

        public static void backupBeforeExitByJustCopy()
        {
            try
            {
                if (IsAccessDB)
                {
                    DirectoryInfo info = new DirectoryInfo(BackupDir);
                    if (!info.Exists)
                    {
                        info.Create();
                        info = new DirectoryInfo(BackupDir);
                    }
                    if (info.Exists)
                    {
                        Cursor current = Cursor.Current;
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            string str = accessDbName + "_000.bak";
                            FileInfo info2 = new FileInfo(BackupDir + str);
                            FileInfo info3 = new FileInfo(BackupDir + accessDbName + "_001.bak");
                            FileInfo info4 = new FileInfo(BackupDir + accessDbName + "_LASTDAY0.bak");
                            FileInfo info5 = new FileInfo(BackupDir + accessDbName + "_LASTDAY1.bak");
                            FileInfo info6 = new FileInfo(string.Format(Application.StartupPath + @"\t{0}.bak", accessDbName));
                            try
                            {
                                if (info2.Exists)
                                {
                                    info2.Attributes = FileAttributes.Archive;
                                }
                            }
                            catch (Exception exception)
                            {
                                wgDebugWrite(exception.ToString(), EventLogEntryType.Error);
                            }
                            try
                            {
                                if (info3.Exists)
                                {
                                    info3.Attributes = FileAttributes.Archive;
                                }
                            }
                            catch (Exception exception2)
                            {
                                wgDebugWrite(exception2.ToString(), EventLogEntryType.Error);
                            }
                            try
                            {
                                if (info2.Exists)
                                {
                                    info2.Attributes = FileAttributes.Archive;
                                }
                            }
                            catch (Exception exception3)
                            {
                                wgDebugWrite(exception3.ToString(), EventLogEntryType.Error);
                            }
                            try
                            {
                                if (info4.Exists)
                                {
                                    info4.Attributes = FileAttributes.Archive;
                                }
                            }
                            catch (Exception exception4)
                            {
                                wgDebugWrite(exception4.ToString(), EventLogEntryType.Error);
                            }
                            try
                            {
                                if (info5.Exists)
                                {
                                    info5.Attributes = FileAttributes.Archive;
                                }
                            }
                            catch (Exception exception5)
                            {
                                wgDebugWrite(exception5.ToString(), EventLogEntryType.Error);
                            }
                            try
                            {
                                if (info6.Exists)
                                {
                                    info6.Attributes = FileAttributes.Archive;
                                }
                            }
                            catch (Exception exception6)
                            {
                                wgDebugWrite(exception6.ToString(), EventLogEntryType.Error);
                            }
                            if (info2.Exists)
                            {
                                if (info3.Exists && (info2.LastWriteTime.ToString("yyyyMMdd") != info3.LastWriteTime.ToString("yyyyMMdd")))
                                {
                                    if (info4.Exists)
                                    {
                                        if (info5.Exists)
                                        {
                                            info5.Delete();
                                        }
                                        info4.MoveTo(BackupDir + accessDbName + "_LASTDAY1.bak");
                                    }
                                    info3.MoveTo(BackupDir + accessDbName + "_LASTDAY0.bak");
                                }
                                if ((info3.FullName == (BackupDir + accessDbName + "_001.bak")) && info3.Exists)
                                {
                                    info3.Delete();
                                }
                                info2.MoveTo(BackupDir + accessDbName + "_001.bak");
                            }
                            info2 = new FileInfo(Application.StartupPath + string.Format(@"\{0}.mdb", accessDbName));
                            info2.CopyTo(Application.StartupPath + string.Format(@"\t{0}.bak", accessDbName), true);
                            info2.CopyTo(BackupDir + str, true);
                        }
                        catch (Exception exception7)
                        {
                            wgDebugWrite(exception7.ToString(), EventLogEntryType.Error);
                        }
                    }
                }
            }
            catch (Exception exception8)
            {
                wgDebugWrite(exception8.ToString(), EventLogEntryType.Error);
            }
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
                string defaultCustConfigzhCHS = wgAppConfig.defaultCustConfigzhCHS;
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

        public static bool CreatePhotoDirectory(string strFileName)
        {
            try
            {
                m_PhotoDiriectyName = strFileName;
                m_bCreatePhotoDirectory = false;
                Thread thread = new Thread(new ThreadStart(wgAppConfig.CreatePhotoDirectoryRealize));
                thread.Name = "CreatePhotoDirectoryRealize";
                thread.Start();
                long ticks = DateTime.Now.Ticks;
                while ((DateTime.Now.Ticks - ticks) < 0xf4240L)
                {
                    if (m_bCreatePhotoDirectory)
                    {
                        break;
                    }
                }
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            catch (Exception)
            {
            }
            return m_bCreatePhotoDirectory;
        }

        public static void CreatePhotoDirectoryRealize()
        {
            try
            {
                Directory.CreateDirectory(m_PhotoDiriectyName);
                m_bCreatePhotoDirectory = true;
            }
            catch (Exception)
            {
            }
        }

        public static void CustConfigureInit()
        {
            InsertKeyVal("autologinName", "");
            InsertKeyVal("autologinPassword", "");
            InsertKeyVal("rgtries", "1234");
            InsertKeyVal("CommCurrent", "");
            InsertKeyVal("EMapZoomInfo", "");
            InsertKeyVal("EMapLocInfo", "");
            InsertKeyVal("NewSoftwareVersionInfo", Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")));
            if (GetKeyVal("NewSoftwareVersionInfo") != Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")))
            {
                UpdateKeyVal("NewSoftwareVersionInfo", Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")));
            }
            InsertKeyVal("RunTimes", "0");
            try
            {
                UpdateKeyVal("RunTimes", GetKeyVal("RunTimes") + 1);
            }
            catch
            {
            }
            InsertKeyVal("RunTimeAt", "0");
            InsertKeyVal("NewSoftwareSpecialVersionInfo", "");
            if (GetKeyVal("NewSoftwareSpecialVersionInfo") != Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")))
            {
                UpdateKeyVal("NewSoftwareSpecialVersionInfo", Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf(".")));
            }
            if (int.Parse(GetKeyVal("RunTimeAt")) >= 0)
            {
                DateTime time = DateTime.Parse("2010-5-1");
                if (((DateTime.Now.Date < time.Date) || (int.Parse(GetKeyVal("RunTimeAt")) == 0)) || ((DateTime.Now.Date >= time.AddDays((double) int.Parse(GetKeyVal("RunTimeAt"))).Date) || (DateTime.Now.AddDays(32.0).Date > time.AddDays((double) int.Parse(GetKeyVal("RunTimeAt"))).Date)))
                {
                }
            }
            else
            {
                UpdateKeyVal("RunTimeAt", "0");
            }
            InsertKeyVal("DisplayFormat_DateYMD", "");
            InsertKeyVal("DisplayFormat_DateYMDWeek", "");
            InsertKeyVal("DisplayFormat_DateYMDHMS", "");
            InsertKeyVal("DisplayFormat_DateYMDHMSWeek", "");
        }

        public static void deselectObject(DataGridView dgv)
        {
            try
            {
                int rowIndex;
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
                using (DataTable table = ((DataView) dgv.DataSource).Table)
                {
                    DataRow row;
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void deselectObject(DataGridView dgv, int iSelectedCurrentNoneMax)
        {
            try
            {
                int rowIndex;
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
                using (DataTable table = ((DataView) dgv.DataSource).Table)
                {
                    DataRow row;
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
                                row["f_Selected"] = iSelectedCurrentNoneMax;
                            }
                        }
                    }
                    else
                    {
                        int num6 = (int) dgv.Rows[rowIndex].Cells[0].Value;
                        row = table.Rows.Find(num6);
                        if (row != null)
                        {
                            row["f_Selected"] = iSelectedCurrentNoneMax;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DirectoryIsExisted(string strFileName)
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
                string str = strFileName.Trim();
                if (str.Length > 2)
                {
                    if (str.Substring(0, 2) == @"\\")
                    {
                        return DirectoryIsExistedWithNetShare(str);
                    }
                    if (str.IndexOf(":") <= 0)
                    {
                        str = Application.StartupPath + @"\" + str;
                    }
                }
                if (Directory.Exists(str))
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        private static void DirectoryIsExistedNetShare()
        {
            try
            {
                if (Directory.Exists(m_DirectoryNetShare))
                {
                    m_bFindDirectoryNetShare = true;
                }
            }
            catch (Exception)
            {
            }
        }

        public static bool DirectoryIsExistedWithNetShare(string strFileName)
        {
            bool flag = false;
            try
            {
                string str = strFileName.Trim();
                m_bFindDirectoryNetShare = false;
                m_DirectoryNetShare = str;
                try
                {
                    Thread thread = new Thread(new ThreadStart(wgAppConfig.DirectoryIsExistedNetShare));
                    thread.Name = "DirectoryIsExistedNetShare";
                    thread.Start();
                    long ticks = DateTime.Now.Ticks;
                    while ((DateTime.Now.Ticks - ticks) < 0xf4240L)
                    {
                        if (m_bFindDirectoryNetShare)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        wgTools.WriteLine("DirectoryIsExistedNetShare  Not Found");
                    }
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        public static void DisposeImage(Image img)
        {
            try
            {
                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        public static void DisposeImageRef(ref Image img)
        {
            try
            {
                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        public static bool exportToExcel(DataGridView dgv, string formText)
        {
            string str = "";
            string path = "";
            try
            {
                if (string.IsNullOrEmpty(formText))
                {
                    str = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + ".xls";
                }
                else
                {
                    str = formText + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + ".xls";
                }
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.FileName = str;
                    dialog.Filter = " (*.xls)|*.xls";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        path = dialog.FileName;
                        using (ExcelObject obj2 = new ExcelObject(path))
                        {
                            int num = 0;
                            using (dfrmWait wait = new dfrmWait())
                            {
                                wait.Show();
                                wait.Refresh();
                                obj2.WriteTable(dgv);
                                foreach (DataGridViewRow row in (IEnumerable) dgv.Rows)
                                {
                                    obj2.AddNewRow(row, dgv);
                                    num++;
                                    if (num >= 0xffff)
                                    {
                                        break;
                                    }
                                    if ((num % 0x3e8) == 0)
                                    {
                                        wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num));
                                        Application.DoEvents();
                                    }
                                }
                                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1:d}", CommonStr.strExportRecords, num));
                                wait.Hide();
                            }
                            XMessageBox.Show(CommonStr.strExportRecords + " = " + num.ToString() + "\t" + ((num >= 0xffff) ? CommonStr.strExportRecordsMax : "") + "\r\n\r\n" + CommonStr.strExportToExcel + " " + path);
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine("ExportToExcel" + path + exception.ToString());
            }
            finally
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
            }
            return false;
        }

        public static bool exportToExcelSpecial(ref DataGridView dgv, string formText, bool bLoadedFinished, ref BackgroundWorker bk1, ref int startRecordIndex, int MaxRecord, string dgvSql)
        {
            DataGridView view = dgv;
            if ((view.Rows.Count <= 0xffff) && !bLoadedFinished)
            {
                using (dfrmWait wait = new dfrmWait())
                {
                    wait.Show();
                    wait.Refresh();
                    while (bk1.IsBusy)
                    {
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    do
                    {
                        if (startRecordIndex <= view.Rows.Count)
                        {
                            startRecordIndex += MaxRecord;
                            bk1.RunWorkerAsync(new object[] { (int) startRecordIndex, 0x101d0 - view.Rows.Count, dgvSql });
                            while (bk1.IsBusy)
                            {
                                Thread.Sleep(500);
                                Application.DoEvents();
                            }
                            startRecordIndex = ((startRecordIndex + 0x101d0) - view.Rows.Count) - MaxRecord;
                        }
                        else
                        {
                            wgAppRunInfo.raiseAppRunInfoLoadNums(view.Rows.Count.ToString() + "#");
                            break;
                        }
                    }
                    while (view.Rows.Count <= 0xffff);
                    wait.Hide();
                }
            }
            exportToExcel(view, formText);
            return true;
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
                if (((fileName.Length > 2) && !(fileName.Substring(0, 2) == @"\\")) && (fileName.IndexOf(":") <= 0))
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
            if (IsAccessDB)
            {
                fillDGVData_Acc(ref dgv, strSql);
            }
            else
            {
                tb = new DataTable();
                dv = new DataView(tb);
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    using (SqlCommand command = new SqlCommand(strSql, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(tb);
                        }
                    }
                }
                dgv.AutoGenerateColumns = false;
                dgv.DataSource = dv;
                for (int i = 0; (i < dv.Table.Columns.Count) && (i < dgv.ColumnCount); i++)
                {
                    dgv.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
                }
            }
        }

        public static void fillDGVData_Acc(ref DataGridView dgv, string strSql)
        {
            tb = new DataTable();
            dv = new DataView(tb);
            using (OleDbConnection connection = new OleDbConnection(dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(strSql, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        adapter.Fill(tb);
                    }
                }
            }
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = dv;
            for (int i = 0; (i < dv.Table.Columns.Count) && (i < dgv.ColumnCount); i++)
            {
                dgv.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
            }
        }

        public static void GetAppIcon(ref Icon appicon)
        {
            try
            {
                if (currenAppIcon == null)
                {
                    currenAppIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
                appicon = currenAppIcon;
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
                if (!File.Exists(path))
                {
                    return str;
                }
                using (DataTable table = new DataTable())
                {
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
                    return str;
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
            return str;
        }

        public static bool getParamValBoolByNO(int NO)
        {
            int num;
            string str = getSystemParamByNO(NO);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return (int.TryParse(str, out num) && (num > 0));
        }

        public static string getPhotoFileName(long cardno)
        {
            string str = "";
            try
            {
                if (cardno == 0L)
                {
                    return str;
                }
                if ((arrPhotoFileFullNames.Count <= 0) || (lastPhotoDirectoryName != Path4Photo()))
                {
                    if (!DirectoryIsExisted(Path4Photo()))
                    {
                        return str;
                    }
                    lastPhotoDirectoryName = Path4Photo();
                }
                DirectoryInfo info = new DirectoryInfo(Path4Photo());
                if (((photoDirectoryLastWriteTime != info.LastWriteTime) || (arrPhotoFileFullNames.Count <= 0)) || (info.GetFiles().Length != photoDirectoryLastFileCount))
                {
                    arrPhotoFileFullNames.Clear();
                    foreach (FileInfo info2 in info.GetFiles())
                    {
                        arrPhotoFileFullNames.Add(info2.FullName);
                    }
                    photoDirectoryLastWriteTime = info.LastWriteTime;
                    photoDirectoryLastFileCount = info.GetFiles().Length;
                }
                string str2 = "";
                for (int i = cardno.ToString().Length; i <= 10; i++)
                {
                    str2 = Path4Photo() + cardno.ToString().PadLeft(i, '0') + ".jpg";
                    if (arrPhotoFileFullNames.IndexOf(str2) >= 0)
                    {
                        return str2;
                    }
                    str2 = str2.ToLower(new CultureInfo("en-US", false)).Replace(".jpg", ".bmp");
                    if (arrPhotoFileFullNames.IndexOf(str2) >= 0)
                    {
                        return str2;
                    }
                }
            }
            catch (Exception exception)
            {
                arrPhotoFileFullNames.Clear();
                photoDirectoryLastWriteTime = DateTime.Parse("2012-4-10 09:08:50.531");
                photoDirectoryLastFileCount = -1;
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        public static string getSqlFindNormal(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
        {
            string str = "";
            try
            {
                string str2 = "";
                if (!string.IsNullOrEmpty(strTimeCon))
                {
                    str2 = str2 + string.Format("AND {0}", strTimeCon);
                }
                if (findConsumerID > 0)
                {
                    str2 = str2 + string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
                    return (strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2));
                }
                if (!string.IsNullOrEmpty(findName))
                {
                    str2 = str2 + string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                if (findCard > 0L)
                {
                    str2 = str2 + string.Format(" AND t_b_Consumer.f_CardNO ={0:d} ", findCard);
                }
                if (groupMinNO > 0)
                {
                    if (groupMinNO >= groupMaxNO)
                    {
                        return (strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO)));
                    }
                    return (strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO)));
                }
                str = strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        public static string getSqlFindPrilivege(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
        {
            string str = "";
            try
            {
                string str2 = "";
                if (!string.IsNullOrEmpty(strTimeCon))
                {
                    str2 = str2 + string.Format("AND {0}", strTimeCon);
                }
                if (findConsumerID > 0)
                {
                    str2 = str2 + string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
                    return (strBaseInfo + string.Format(" FROM (t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  INNER JOIN t_b_Door  ON {0}.f_DoorID=t_b_Door.f_DoorID ", fromMainDt, str2));
                }
                if (!string.IsNullOrEmpty(findName))
                {
                    str2 = str2 + string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                if (findCard > 0L)
                {
                    str2 = str2 + string.Format(" AND t_b_Consumer.f_CardNO ={0:d} ", findCard);
                }
                if (groupMinNO > 0)
                {
                    if (groupMinNO >= groupMaxNO)
                    {
                        return (strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Door ON {0}.f_DoorID=t_b_Door.f_DoorID) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO)));
                    }
                    return (strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Door ON {0}.f_DoorID=t_b_Door.f_DoorID) INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO)));
                }
                str = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) INNER JOIN t_b_Door ON {0}.f_DoorID=t_b_Door.f_DoorID ) ", fromMainDt, str2);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        public static string getSqlFindSwipeRecord(string strBaseInfo, string fromMainDt, string strTimeCon, int groupMinNO, int groupIDOfMinNO, int groupMaxNO, string findName, long findCard, int findConsumerID)
        {
            string str = "";
            try
            {
                string str2 = "";
                string str3 = " WHERE (1>0) ";
                if (!string.IsNullOrEmpty(strTimeCon))
                {
                    str3 = str3 + string.Format("AND {0}", strTimeCon);
                }
                if (findConsumerID > 0)
                {
                    str2 = str2 + string.Format("AND   t_b_Consumer.f_ConsumerID ={0:d} ", findConsumerID);
                    str = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1})) LEFT JOIN  t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) ) LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2);
                    return (str + str3);
                }
                if (!string.IsNullOrEmpty(findName))
                {
                    str3 = str3 + string.Format(" AND t_b_Consumer.f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                if (findCard > 0L)
                {
                    str3 = str3 + string.Format(" AND {0}.f_CardNO ={1:d} ", fromMainDt, findCard);
                }
                if (groupMinNO > 0)
                {
                    if (groupMinNO >= groupMaxNO)
                    {
                        str = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO));
                    }
                    else
                    {
                        str = strBaseInfo + string.Format(" FROM ((t_b_Consumer INNER JOIN {0} ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) )  INNER JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID {2} ) ", fromMainDt, str2, string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO));
                    }
                }
                else
                {
                    str = strBaseInfo + string.Format(" FROM (({0} LEFT JOIN t_b_Consumer ON ( t_b_Consumer.f_ConsumerID = {0}.f_ConsumerID {1}))  LEFT JOIN   t_b_Reader on ( t_b_Reader.f_ReaderID = {0}.f_ReaderID) )  LEFT JOIN t_b_Group ON (t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ) ", fromMainDt, str2);
                }
                str = str + str3;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return str;
        }

        private static string getSystemParam(int parNo, string parName)
        {
            if (IsAccessDB)
            {
                return getSystemParam_Acc(parNo, parName);
            }
            try
            {
                string str2;
                if (string.IsNullOrEmpty(parName))
                {
                    str2 = "SELECT f_Value FROM t_a_SystemParam WHERE f_NO=" + parNo.ToString();
                }
                else
                {
                    str2 = "SELECT f_Value FROM t_a_SystemParam WHERE f_Name=" + wgTools.PrepareStr(parName);
                }
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand(str2, connection))
                    {
                        return wgTools.SetObjToStr(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return "";
        }

        private static string getSystemParam_Acc(int parNo, string parName)
        {
            try
            {
                string str2;
                if (string.IsNullOrEmpty(parName))
                {
                    str2 = "SELECT f_Value FROM t_a_SystemParam WHERE f_NO=" + parNo.ToString();
                }
                else
                {
                    str2 = "SELECT f_Value FROM t_a_SystemParam WHERE f_Name=" + wgTools.PrepareStr(parName);
                }
                using (OleDbConnection connection = new OleDbConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand(str2, connection))
                    {
                        return wgTools.SetObjToStr(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return "";
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
            if (IsAccessDB)
            {
                return getSystemParamValue_Acc(NO, out EName, out value, out notes);
            }
            int num = -9;
            EName = null;
            value = null;
            notes = null;
            string cmdText = "SELECT * FROM t_a_SystemParam WHERE f_NO = " + NO.ToString();
            using (SqlConnection connection = new SqlConnection(dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        EName = reader["f_EName"] as string;
                        value = reader["f_Value"] as string;
                        notes = reader["f_Notes"] as string;
                        num = 1;
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public static int getSystemParamValue_Acc(int NO, out string EName, out string value, out string notes)
        {
            int num = -9;
            EName = null;
            value = null;
            notes = null;
            string cmdText = "SELECT * FROM t_a_SystemParam WHERE f_NO = " + NO.ToString();
            using (OleDbConnection connection = new OleDbConnection(dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        EName = reader["f_EName"] as string;
                        value = reader["f_Value"] as string;
                        notes = reader["f_Notes"] as string;
                        num = 1;
                    }
                    reader.Close();
                }
            }
            return num;
        }

        public static int getValBySql(string strSql)
        {
            if (IsAccessDB)
            {
                return getValBySql_Acc(strSql);
            }
            int result = 0;
            if (!string.IsNullOrEmpty(strSql))
            {
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand(strSql, connection))
                    {
                        int.TryParse(command.ExecuteScalar().ToString(), out result);
                    }
                }
            }
            return result;
        }

        public static int getValBySql_Acc(string strSql)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(strSql))
            {
                using (OleDbConnection connection = new OleDbConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        int.TryParse(command.ExecuteScalar().ToString(), out result);
                    }
                }
            }
            return result;
        }

        public static void InsertKeyVal(string key, string value)
        {
            bool flag = false;
            try
            {
                string path = Application.StartupPath + @"\BioAccess.xml";
                if (File.Exists(path))
                {
                    using (DataTable table = new DataTable())
                    {
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
                        using (StringWriter writer = new StringWriter())
                        {
                            using (StreamWriter writer2 = new StreamWriter(path, false))
                            {
                                table.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                                writer2.Write(writer.ToString());
                            }
                        }
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

        public static string Path4Doc()
        {
            string path = @".\DOC\";
            path = Application.StartupPath + @"\DOC\";
            try
            {
                DirectoryInfo info = new DirectoryInfo(path);
                if (!info.Exists)
                {
                    info.Create();
                }
            }
            catch
            {
            }
            return path;
        }

        public static string Path4Photo()
        {
            string str = Application.StartupPath + @"\PHOTO\";
            if (!string.IsNullOrEmpty(getSystemParamByNO(0x29)))
            {
                str = getSystemParamByNO(0x29);
                if (str.Substring(str.Length - 1, 1) != @"\")
                {
                    str = str + @"\";
                }
            }
            return str;
        }

        public static string Path4PhotoDefault()
        {
            return (Application.StartupPath + @"\PHOTO\");
        }

        public static void printdgv(DataGridView dv, string Title)
        {
            using (DGVPrinter printer = new DGVPrinter())
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    printer.Title = Title;
                }
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.PrintDataGridView(dv);
            }
        }

        public static void ReadGVStyle(Form form, DataGridView dgv)
        {
            try
            {
                if ((form != null) && (dgv != null))
                {
                    string str = Application.StartupPath + @"\PHOTO\";
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
                        using (DataTable table = new DataTable())
                        {
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
                }
            }
            catch (Exception exception)
            {
                writeLine(exception.ToString());
            }
        }

        public static string ReplaceFloorRomm(string info)
        {
            string str = info;
            try
            {
                if (bFloorRoomManager)
                {
                    str = info.Replace(CommonStr.strReplaceDepartment, CommonStr.strReplaceFloorRoom);
                    if (str == CommonStr.strReplaceDepartment2)
                    {
                        str = str.Replace(CommonStr.strReplaceDepartment2, CommonStr.strReplaceFloorRoom);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
            return str;
        }

        public static string ReplaceWorkNO(string info)
        {
            string str = info;
            try
            {
                if (bFloorRoomManager)
                {
                    str = info.Replace(CommonStr.strReplaceWorkNO, CommonStr.strReplaceNO);
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
            return str;
        }

        public static void RestoreGVStyle(Form form, DataGridView dgv)
        {
            try
            {
                string str = Application.StartupPath + @"\PHOTO\";
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
                    File.Delete(path);
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
        }

        public static int runUpdateSql(string strSql)
        {
            if (IsAccessDB)
            {
                return runUpdateSql_Acc(strSql);
            }
            int num = -1;
            if (!string.IsNullOrEmpty(strSql))
            {
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlCommand command = new SqlCommand(strSql, connection))
                    {
                        num = command.ExecuteNonQuery();
                    }
                }
            }
            return num;
        }

        public static int runUpdateSql_Acc(string strSql)
        {
            int num = -1;
            if (!string.IsNullOrEmpty(strSql))
            {
                using (OleDbConnection connection = new OleDbConnection(dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        num = command.ExecuteNonQuery();
                    }
                }
            }
            return num;
        }

        public static void SaveDGVStyle(Form form, DataGridView dgv)
        {
            try
            {
                string str2;
                string str = Application.StartupPath + @"\PHOTO\";
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

        public static void SaveNewXmlFile(string key, string value)
        {
            string startupPath = Application.StartupPath;
            string path = startupPath + @"\BioAccess.xmlAA";
            string str3 = startupPath + @"\photo\BioAccess.xmlAA";
            string defaultCustConfigzhCHS = wgAppConfig.defaultCustConfigzhCHS;
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
            if (File.Exists(path))
            {
                UpdateKeyVal(key, value, path);
            }
        }

        public static void selectObject(DataGridView dgv)
        {
            selectObject(dgv, "", "");
        }

        public static void selectObject(DataGridView dgv, int iSelectedCurrentNoneMax)
        {
            selectObject(dgv, "", "", iSelectedCurrentNoneMax);
        }

        public static void selectObject(DataGridView dgv, string secondField, string val)
        {
            try
            {
                int rowIndex;
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
                using (DataTable table = ((DataView) dgv.DataSource).Table)
                {
                    DataRow row;
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void selectObject(DataGridView dgv, string secondField, string val, int iSelectedCurrentNoneMax)
        {
            try
            {
                int rowIndex;
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
                using (DataTable table = ((DataView) dgv.DataSource).Table)
                {
                    DataRow row;
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
                                row["f_Selected"] = iSelectedCurrentNoneMax + 1;
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
                            row["f_Selected"] = iSelectedCurrentNoneMax + 1;
                            if (secondField != "")
                            {
                                row[secondField] = val;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void setDisplayFormatDate(DateTimePicker dtp, string displayformat)
        {
            try
            {
                if (string.IsNullOrEmpty(displayformat))
                {
                    dtp.Format = DateTimePickerFormat.Long;
                }
                else
                {
                    dtp.Format = DateTimePickerFormat.Custom;
                    dtp.CustomFormat = displayformat;
                }
            }
            catch (Exception)
            {
            }
        }

        public static void setDisplayFormatDate(DataGridView dgv, string columnname, string displayformat)
        {
            try
            {
                if (!string.IsNullOrEmpty(displayformat) && !string.IsNullOrEmpty(columnname))
                {
                    dgv.Columns[columnname].DefaultCellStyle.Format = displayformat;
                }
            }
            catch (Exception)
            {
            }
        }

        public static int setSystemParamValue(int NO, string value)
        {
            if (IsAccessDB)
            {
                return setSystemParamValue_Acc(NO, value);
            }
            try
            {
                string cmdText = ("UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStr(value)) + " WHERE f_NO = " + NO.ToString();
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return -9;
        }

        public static int setSystemParamValue(int NO, string EName, string value, string notes)
        {
            if (IsAccessDB)
            {
                return setSystemParamValue_Acc(NO, EName, value, notes);
            }
            try
            {
                string cmdText = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStr(value);
                if (!string.IsNullOrEmpty(EName))
                {
                    cmdText = cmdText + ", [f_EName] = " + wgTools.PrepareStr(EName);
                }
                if (!string.IsNullOrEmpty(notes))
                {
                    cmdText = cmdText + ", [f_Notes] = " + wgTools.PrepareStr(notes);
                }
                cmdText = cmdText + " WHERE f_NO = " + NO.ToString();
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return -9;
        }

        public static int setSystemParamValue_Acc(int NO, string value)
        {
            try
            {
                string cmdText = ("UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStr(value)) + " WHERE f_NO = " + NO.ToString();
                using (OleDbConnection connection = new OleDbConnection(dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return -9;
        }

        public static int setSystemParamValue_Acc(int NO, string EName, string value, string notes)
        {
            if (IsAccessDB)
            {
                try
                {
                    string cmdText = "UPDATE t_a_SystemParam SET [f_Value] = " + wgTools.PrepareStr(value);
                    if (!string.IsNullOrEmpty(EName))
                    {
                        cmdText = cmdText + ", [f_EName] = " + wgTools.PrepareStr(EName);
                    }
                    if (!string.IsNullOrEmpty(notes))
                    {
                        cmdText = cmdText + ", [f_Notes] = " + wgTools.PrepareStr(notes);
                    }
                    cmdText = cmdText + " WHERE f_NO = " + NO.ToString();
                    using (OleDbConnection connection = new OleDbConnection(dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
            return -9;
        }

        public static int setSystemParamValueBool(int NO, bool value)
        {
            if (IsAccessDB)
            {
                return setSystemParamValueBool_Acc(NO, value);
            }
            try
            {
                string cmdText = ("UPDATE t_a_SystemParam SET [f_Value] = " + (value ? "1" : "0")) + " WHERE f_NO = " + NO.ToString();
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return -9;
        }

        public static int setSystemParamValueBool_Acc(int NO, bool value)
        {
            try
            {
                string cmdText = ("UPDATE t_a_SystemParam SET [f_Value] = " + (value ? "1" : "0")) + " WHERE f_NO = " + NO.ToString();
                using (OleDbConnection connection = new OleDbConnection(dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return -9;
        }

        public static void ShowMyImage(string fileToDisplay, ref Image img)
        {
            try
            {
                DisposeImageRef(ref img);
                if (!string.IsNullOrEmpty(fileToDisplay) && FileIsExisted(fileToDisplay))
                {
                    using (FileStream stream = new FileStream(fileToDisplay, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int) stream.Length);
                        using (MemoryStream stream2 = new MemoryStream(buffer))
                        {
                            img = Image.FromStream(stream2);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static void ShowImageStream(byte[] data, ref Image img)
        {
            try
            {
                DisposeImageRef(ref img);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    img = Image.FromStream(stream);
                }
            }
            catch
            {
            }
        }

        public static void UpdateKeyVal(string key, string value)
        {
            UpdateKeyVal(key, value, "");
        }

        public static void UpdateKeyVal(string key, string value, string xmlfileName)
        {
            bool flag = false;
            try
            {
                string path = Application.StartupPath + @"\BioAccess.xml";
                if (!string.IsNullOrEmpty(xmlfileName))
                {
                    path = xmlfileName;
                }
                if (!File.Exists(path))
                {
                    CreateCustXml();
                }
                if (File.Exists(path))
                {
                    using (DataTable table = new DataTable())
                    {
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
                        using (StringWriter writer = new StringWriter())
                        {
                            using (StreamWriter writer2 = new StreamWriter(path, false))
                            {
                                table.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                                writer2.Write(writer.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgDebugWrite(exception.ToString());
            }
        }

        public static string weekdayToChsName(int weekDay)
        {
            string str = "";
            try
            {
                string[] strArray = new string[] { CommonStr.strSunday_Short, CommonStr.strMonday_Short, CommonStr.strTuesday_Short, CommonStr.strWednesday_Short, CommonStr.strThursday_Short, CommonStr.strFriday_Short, CommonStr.strSaturday_Short };
                if ((weekDay >= 0) && (weekDay <= 6))
                {
                    str = strArray[weekDay];
                }
            }
            catch
            {
            }
            return str;
        }

        public static void wgDBLog(string strMsg, EventLogEntryType entryType, byte[] rawData)
        {
            if (IsAccessDB)
            {
                wgDBLog_Acc(strMsg, entryType, rawData);
            }
            else
            {
                string str = string.Concat(new object[] { "INSERT INTO [t_s_wglog]( [f_EventType], [f_EventDesc], [f_UserID], [f_UserName])  VALUES( ", wgTools.PrepareStr(entryType), ",", wgTools.PrepareStr(strMsg), ",", icOperator.OperatorID, ",", wgTools.PrepareStr(icOperator.OperatorName), ")" });
                try
                {
                    if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(dbConString))
                    {
                        using (SqlConnection connection = new SqlConnection(dbConString))
                        {
                            using (SqlCommand command = new SqlCommand(str, connection))
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void wgDBLog_Acc(string strMsg, EventLogEntryType entryType, byte[] rawData)
        {
            if (IsAccessDB)
            {
                string str = string.Concat(new object[] { "INSERT INTO [t_s_wglog]( [f_EventType], [f_EventDesc], [f_UserID], [f_UserName])  VALUES( ", wgTools.PrepareStr(entryType), ",", wgTools.PrepareStr(strMsg), ",", icOperator.OperatorID, ",", wgTools.PrepareStr(icOperator.OperatorName), ")" });
                try
                {
                    if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(dbConString))
                    {
                        using (OleDbConnection connection = new OleDbConnection(dbConString))
                        {
                            using (OleDbCommand command = new OleDbCommand(str, connection))
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void wgDebugWrite(string info)
        {
            wgLogWithoutDB(info, EventLogEntryType.Information, null);
        }

        public static void wgDebugWrite(string strMsg, EventLogEntryType entryType)
        {
            wgLogWithoutDB(strMsg, entryType, null);
        }

        public static void wgLog(string strMsg)
        {
            wgLog(strMsg, EventLogEntryType.Information, null);
        }

        public static void wgLog(string strMsg, EventLogEntryType entryType, byte[] rawData)
        {
            try
            {
                string str = string.Concat(new object[] { icOperator.OperatorID, ".", icOperator.OperatorName, ".", strMsg });
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
            try
            {
                wgDBLog(string.Concat(new object[] { icOperator.OperatorID, ".", icOperator.OperatorName, ".", strMsg }), entryType, rawData);
            }
            catch (Exception)
            {
            }
        }

        public static void wglogRecEventOfController(string strMsg)
        {
            wglogRecEventOfController(strMsg, EventLogEntryType.Information, null);
        }

        public static void wglogRecEventOfController(string strMsg, EventLogEntryType entryType, byte[] rawData)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Application.StartupPath + @"\BioAccessRec.log", true))
                {
                    writer.WriteLine(strMsg);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void wgLogWithoutDB(string strMsg, EventLogEntryType entryType, byte[] rawData)
        {
            try
            {
                string str = string.Concat(new object[] { icOperator.OperatorID, ".", icOperator.OperatorName, ".", strMsg });
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

        public static string accessDbName
        {
            get
            {
                return "AccessDB";
            }
        }

        private static string BackupDir
        {
            get
            {
                return (Application.StartupPath + @"\BACKUP\");
            }
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

        public static string dbName
        {
            get
            {
                return m_dbName;
            }
            set
            {
                m_dbName = value;
            }
        }

        public static string dbWEBUserName
        {
            get
            {
                return "WEBUsers";
            }
        }

        public static bool IsAccessDB
        {
            get
            {
                return m_IsAccessDB;
            }
            set
            {
                m_IsAccessDB = value;
                wgTools.IsSqlServer = !value;
            }
        }

        public static int LogEventMaxCount
        {
            get
            {
                return 0x2710;
            }
        }
    }
}

