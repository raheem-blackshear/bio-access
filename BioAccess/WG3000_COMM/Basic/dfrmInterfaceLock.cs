namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmInterfaceLock : frmBioAccess
    {
        public string newPassword;
        public int operatorID;
        private bool unlocked = false;

        public dfrmInterfaceLock()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (icOperator.login(this.txtOperatorName.Text, this.txtPassword.Text))
            {
                unlocked = true;
                base.DialogResult = DialogResult.OK;
                wgAppConfig.IsLogin = true;
                wgAppConfig.wgLog(this.Text, EventLogEntryType.Information, null);
                base.Close();
            }
            else
            {
                SystemSounds.Beep.Play();
                XMessageBox.Show(this, CommonStr.strErrPwdOrName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dfrmSetPassword_Load(object sender, EventArgs e)
        {
            this.txtOperatorName.CharacterCasing = CharacterCasing.Lower;
            this.txtPassword.CharacterCasing = CharacterCasing.Lower;
        }

        private void dfrmInterfaceLock_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !unlocked;
        }
    }
}

