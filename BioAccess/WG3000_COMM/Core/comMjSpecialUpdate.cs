namespace WG3000_COMM.Core
{
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public class comMjSpecialUpdate : Component
    {
        private Container components;
        private static string downweb = "http://www.wiegand.com.cn/down/";

        public comMjSpecialUpdate()
        {
            this.InitializeComponent();
        }

        public comMjSpecialUpdate(IContainer Container) : this()
        {
            Container.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static string getSoftwareRar(string newfilename)
        {
            string str = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    string fileName = Application.StartupPath + @"\PHOTO\sp" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".exe";
                    client.DownloadFile(downweb + newfilename, fileName);
                    FileInfo info = new FileInfo(fileName);
                    if (info.Exists)
                    {
                        str = fileName;
                    }
                }
            }
            catch (Exception)
            {
            }
            return str;
        }

        private static string GetVersionFile()
        {
            string str = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    string fileName = Application.StartupPath + @"\PHOTO\ver" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
                    client.DownloadFile(downweb + "ver.txt", fileName);
                    FileInfo info = new FileInfo(fileName);
                    if (info.Exists)
                    {
                        using (StreamReader reader = new StreamReader(fileName))
                        {
                            str = reader.ReadToEnd();
                        }
                        info.Delete();
                    }
                    return str;
                }
            }
            catch (Exception)
            {
            }
            return str;
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public static bool updateMjSpecialSoftware()
        {
            string versionFile = "";
            string pathName = "";
            try
            {
                downweb = "http://www.wiegand.com.cn/down/";
                versionFile = GetVersionFile();
                if (versionFile == "")
                {
                    downweb = "http://www.wgaccess.com/down/";
                    versionFile = GetVersionFile();
                }
                bool flag1 = versionFile == "";
                DateTime time = DateTime.Parse("2011-5-1");
                wgAppConfig.UpdateKeyVal("RunTimeAt", (DateTime.Now.Subtract(time).Days + 0x1f).ToString());
                wgAppConfig.wgLog("GetNewSpecialSoft: " + versionFile, EventLogEntryType.Information, null);
                string[] strArray = versionFile.Split(new char[] { ';' });
                bool flag = false;
                if (wgTools.CmpProductVersion(strArray[1], wgAppConfig.GetKeyVal("NewSoftwareSpecialVersionInfo")) != 0)
                {
                    flag = true;
                }
                if (!flag)
                {
                    return false;
                }
                pathName = getSoftwareRar(strArray[2]);
                if (pathName == "")
                {
                    return false;
                }
                wgAppConfig.UpdateKeyVal("NewSoftwareSpecialVersionInfo", strArray[1]);
                Interaction.Shell(pathName, AppWinStyle.Hide, false, -1);
                Thread.Sleep(0x1388);
                Interaction.Shell(strArray[3], AppWinStyle.Hide, false, -1);
                Thread.Sleep(0x1388);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return false;
        }
    }
}

