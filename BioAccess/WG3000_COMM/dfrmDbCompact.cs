namespace WG3000_COMM
{
    using JRO;
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmDbCompact : frmBioAccess
    {
        public dfrmDbCompact()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public void cmdCompactDatabase_Click(object sender, EventArgs e)
        {
            wgAppConfig.wgLog(this.Text + " ......");
            if (wgAppConfig.IsAccessDB)
            {
                this.cmdCompactDatabase_Click_Acc(sender, e);
            }
            else
            {
                this.sqlBackup2010();
            }
        }

        public void cmdCompactDatabase_Click_Acc(object sender, EventArgs e)
        {
            try
            {
                JetEngine engine;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    engine = new JetEngineClass();
                }
                catch
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "regsvr32";
                        process.StartInfo.Arguments = "/s \"" + Application.StartupPath + "\\msjro.dll\"";
                        process.Start();
                        process.WaitForExit();
                    }
                    engine = new JetEngineClass();
                }
                Thread.Sleep(500);
                string str = "";
                string fileName = "";
                string accessDbName = wgAppConfig.accessDbName;
                if (string.IsNullOrEmpty(accessDbName))
                {
                    str = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + ".mdb";
                }
                else
                {
                    str = accessDbName + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + ".mdb";
                }
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.FileName = str;
                    dialog.Filter = " (*.mdb)|*.mdb";
                    dialog.InitialDirectory = Application.StartupPath + @".\BACKUP";
                    string keyVal = wgAppConfig.GetKeyVal("BackupPathOfAccessDB");
                    if (!string.IsNullOrEmpty(keyVal))
                    {
                        try
                        {
                            dialog.InitialDirectory = keyVal;
                        }
                        catch
                        {
                        }
                    }
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        fileName = dialog.FileName;
                        wgAppConfig.UpdateKeyVal("BackupPathOfAccessDB", fileName);
                        using (dfrmWait wait = new dfrmWait())
                        {
                            wait.Show();
                            wait.Refresh();
                            wgAppConfig.backupBeforeExitByJustCopy();
                            engine.CompactDatabase(wgAppConfig.dbConString, string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User ID=admin;Password=;JET OLEDB:Database Password=passaccess;Jet OLEDB:Engine Type=5", fileName));
                            new FileInfo(fileName).CopyTo(Application.StartupPath + string.Format(@"\{0}.mdb", wgAppConfig.accessDbName), true);
                            wait.Hide();
                        }
                        XMessageBox.Show(this.Text + CommonStr.strSuccessfully + "\r\n\r\n" + fileName);
                        base.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine("Backup Access: " + exception.ToString());
                wgAppConfig.wgLog("Backup Access: " + exception.ToString());
                XMessageBox.Show(this.Text + " " + CommonStr.strFailed + " " + exception.ToString());
            }
            finally
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
            }
        }

        private void dfrmDbCompact_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.txtDirectory.Visible = true;
                }
            }
        }

        private void dfrmDbCompact_Load(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool sqlBackup2010()
        {
            string database = null;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string str4;
                SqlConnection.ClearAllPools();
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    database = connection.Database;
                }
                catch (Exception)
                {
                }
                connection.Close();
                if (database == null)
                {
                    return false;
                }
                if (this.txtDirectory.Visible)
                {
                    if ((this.txtDirectory.Text.Length <= 0) || (this.txtDirectory.Text.Substring(0, 2) != @"\\"))
                    {
                        return false;
                    }
                    if (this.txtDirectory.Text.Substring(this.txtDirectory.Text.Length - 1, 1) == @"\")
                    {
                        str4 = string.Format("{0}{1}_sql_{2}.bak", this.txtDirectory.Text, database, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
                    }
                    else
                    {
                        str4 = string.Format(@"{0}\{1}_sql_{2}.bak", this.txtDirectory.Text, database, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
                    }
                }
                else
                {
                    str4 = string.Format("{0}_sql_{1}.bak", database, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString.Replace(string.Format("initial catalog={0}", database), string.Format("initial catalog={0}", "master"))))
                {
                    try
                    {
                        connection2.Open();
                        string cmdText = "SELECT  SERVERPROPERTY('productversion'), SERVERPROPERTY ('productlevel'), SERVERPROPERTY ('edition')";
                        using (SqlCommand command = new SqlCommand(cmdText, connection2))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            object valA = command.ExecuteScalar();
                            if (valA != null)
                            {
                                if (this.txtDirectory.Visible)
                                {
                                    if ((this.txtDirectory.Text.Length <= 0) || (this.txtDirectory.Text.Substring(0, 2) != @"\\"))
                                    {
                                        return false;
                                    }
                                    if (this.txtDirectory.Text.Substring(this.txtDirectory.Text.Length - 1, 1) == @"\")
                                    {
                                        str4 = string.Format("{0}{1}_sql_{2}_{3}.bak", new object[] { this.txtDirectory.Text, database, wgTools.SetObjToStr(valA), DateAndTime.Now.ToString("yyyyMMdd_HHmmss") });
                                    }
                                    else
                                    {
                                        str4 = string.Format(@"{0}\{1}_sql_{2}_{3}.bak", new object[] { this.txtDirectory.Text, database, wgTools.SetObjToStr(valA), DateAndTime.Now.ToString("yyyyMMdd_HHmmss") });
                                    }
                                }
                                else
                                {
                                    str4 = string.Format("{0}_sql_{1}_{2}.bak", database, wgTools.SetObjToStr(valA), DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
                                }
                            }
                        }
                        using (SqlCommand command2 = new SqlCommand(string.Format("BACKUP DATABASE [{0}] TO DISK = {1}", database, wgTools.PrepareStr(str4)), connection2))
                        {
                            command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            command2.ExecuteNonQuery();
                        }
                        XMessageBox.Show(this.Text + CommonStr.strSuccessfully + "\r\n\r\n" + str4);
                        wgAppConfig.wgLog(this.Text + " OK :  " + str4);
                        base.Close();
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
            }
            catch (Exception exception2)
            {
                string cultureInfoStr = wgAppConfig.CultureInfoStr;
                if (cultureInfoStr == null)
                {
                    goto Label_03CD;
                }
                if (!(cultureInfoStr == "zh-CHS"))
                {
                    if (cultureInfoStr == "zh-CHT")
                    {
                        goto Label_03B4;
                    }
                    goto Label_03CD;
                }
                XMessageBox.Show("失败.\r\n\r\n" + exception2.ToString());
                goto Label_03E4;
            Label_03B4:
                XMessageBox.Show("失敗.\r\n\r\n" + exception2.ToString());
                goto Label_03E4;
            Label_03CD:
                XMessageBox.Show("Failed.  \r\n\r\n" + exception2.ToString());
            Label_03E4:
                wgAppConfig.wgLog(this.Text + "  Failed. :  \r\n\r\n" + exception2.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return true;
        }
    }
}

