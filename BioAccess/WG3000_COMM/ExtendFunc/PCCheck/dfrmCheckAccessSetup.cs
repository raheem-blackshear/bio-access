namespace WG3000_COMM.ExtendFunc.PCCheck
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmCheckAccessSetup : frmBioAccess
    {
        public int active;
        public string groupname;
        public int morecards = 1;
        private string newSoundFile = "";
        public string soundfilename;

        public dfrmCheckAccessSetup()
        {
            this.InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + @"\Photo\";
                }
                catch (Exception)
                {
                }
                this.openFileDialog1.Filter = "(*.wav)|*.wav|(*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    this.newSoundFile = this.openFileDialog1.FileName;
                    FileInfo info = new FileInfo(this.newSoundFile);
                    this.txtFileName.Text = info.Name;
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
                if (!string.IsNullOrEmpty(this.newSoundFile))
                {
                    FileInfo info = new FileInfo(this.newSoundFile);
                    FileInfo info2 = new FileInfo(wgAppConfig.Path4PhotoDefault() + this.txtFileName.Text);
                    if (info2.FullName.ToUpper() != this.newSoundFile.ToUpper())
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
                            wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                        }
                        info.CopyTo(wgAppConfig.Path4PhotoDefault() + this.txtFileName.Text, true);
                    }
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            try
            {
                if (this.chkActive.Checked)
                {
                    this.active = 1;
                    this.morecards = (int) this.nudMoreCards.Value;
                    this.soundfilename = this.txtFileName.Text;
                }
                else
                {
                    this.active = 0;
                    this.morecards = 1;
                    this.soundfilename = "";
                }
                base.DialogResult = DialogResult.OK;
            }
            catch (Exception exception3)
            {
                wgTools.WgDebugWrite(exception3.ToString(), new object[0]);
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            this.GroupBox1.Enabled = this.chkActive.Checked;
        }

        private void dfrmCheckAccessSetup_Load(object sender, EventArgs e)
        {
            try
            {
                this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                this.txtGroupName.Text = this.groupname;
                if (this.active > 0)
                {
                    this.chkActive.Checked = true;
                    this.GroupBox1.Enabled = true;
                }
                else
                {
                    this.chkActive.Checked = false;
                    this.GroupBox1.Enabled = false;
                }
                this.nudMoreCards.Value = this.morecards;
                this.txtFileName.Text = this.soundfilename;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
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

