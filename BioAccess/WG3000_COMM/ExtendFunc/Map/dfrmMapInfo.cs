namespace WG3000_COMM.ExtendFunc.Map
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMapInfo : frmBioAccess
    {
        public string mapFile;
        public string mapName;
        private ResourceManager resStr;

        public dfrmMapInfo()
        {
            this.InitializeComponent();
            this.resStr = new ResourceManager("WgiCCard." + base.Name + "Str", Assembly.GetExecutingAssembly());
            this.resStr.IgnoreCase = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (this.txtMapFileName.Text != "")
                    {
                        this.OpenFileDialog1.InitialDirectory = this.txtMapFileName.Text;
                    }
                }
                catch (Exception)
                {
                }
                this.OpenFileDialog1.FilterIndex = 1;
                if (this.OpenFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    this.txtMapFileName.Text = this.OpenFileDialog1.FileName;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtMapName.Text = this.txtMapName.Text.Trim();
                this.txtMapFileName.Text = this.txtMapFileName.Text.Trim();
                if (this.txtMapName.Text == "")
                {
                    XMessageBox.Show(CommonStr.strMapNameNull);
                }
                else if (this.txtMapFileName.Text == "")
                {
                    XMessageBox.Show(CommonStr.strMapFileNull);
                }
                else
                {
                    FileInfo info = new FileInfo(this.txtMapFileName.Text);
                    if (!info.Exists)
                    {
                        info = new FileInfo(wgAppConfig.Path4PhotoDefault() + info.Name);
                        if (!info.Exists)
                        {
                            XMessageBox.Show(CommonStr.strMapFileNotExist);
                            return;
                        }
                    }
                    FileInfo info2 = new FileInfo(wgAppConfig.Path4PhotoDefault() + info.Name);
                    if (info2.FullName.ToUpper() != info.FullName.ToUpper())
                    {
                        try
                        {
                            if (info2.Exists)
                            {
                                info2.Delete();
                            }
                        }
                        catch (Exception exception)
                        {
                            wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                        }
                        info.CopyTo(wgAppConfig.Path4PhotoDefault() + info.Name, true);
                    }
                    this.mapName = this.txtMapName.Text;
                    this.mapFile = info.Name;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

