namespace WG3000_COMM
{
    using Microsoft.VisualBasic;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.CoreAA;

    internal static class Program
    {
        private static string appName = "BioAccess";

        public static void localize()
        {
            wgToolsAA.CultureInfoStr = wgToolsAA.GetKeyVal("Language");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgToolsAA.CultureInfoStr, false);
        }

        private static void logByFile(string info)
        {
            wgToolsAA.wgLogWithoutDB(info, EventLogEntryType.Information, null);
        }

        [STAThread]
        public static void Main(string[] cmdArgs)
        {
            Directory.SetCurrentDirectory(Application.StartupPath);
            try
            {
                if (cmdArgs.Length == 0)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    localize();
                    Application.Run(new frmSqlServerSetup());
                }
                else if ((cmdArgs.Length == 1) && (cmdArgs[0].ToUpper() == "UPDATE"))
                {
                    wgToolsAA.wgLogWithoutDB("starting UPDATE", EventLogEntryType.Information, null);
                    Process[] processesByName = null;
                    for (int i = 0; i <= 3; i++)
                    {
                        processesByName = Process.GetProcessesByName(appName);
                        if (processesByName.Length <= 0)
                        {
                            break;
                        }
                        Thread.Sleep(0x3e8);
                        if (processesByName.Length > 0)
                        {
                            foreach (Process process in processesByName)
                            {
                                try
                                {
                                    process.Kill();
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    if (processesByName.Length > 0)
                    {
                        logByFile(string.Format("More {0} apps Running", appName));
                    }
                    else
                    {
                        Directory.SetCurrentDirectory(Application.StartupPath);
                        FileInfo info = new FileInfo(Application.StartupPath + @"\" + appName + ".exe");
                        if (!info.Exists)
                        {
                            logByFile(appName + " Nonexist and exit");
                        }
                        else
                        {
                            DateTime lastWriteTime = info.LastWriteTime;
                            string fileName = Application.StartupPath + @"\update\mjupdate.exe";
                            info = new FileInfo(fileName);
                            if (info.Exists)
                            {
                                Interaction.Shell(fileName, AppWinStyle.Hide, true, 0x2710);
                                fileName = Application.StartupPath + @"\" + appName + ".exe";
                                info = new FileInfo(fileName);
                                if (info.Exists)
                                {
                                    logByFile("updated Success. " + appName + " start");
                                    Interaction.Shell(fileName, AppWinStyle.NormalFocus, false, -1);
                                }
                                else
                                {
                                    logByFile(appName + " Nonexist and exit");
                                }
                            }
                            else
                            {
                                logByFile(@"\update\mjupdate.exe Nonexist and exit");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logByFile(exception.ToString());
            }
        }
    }
}

