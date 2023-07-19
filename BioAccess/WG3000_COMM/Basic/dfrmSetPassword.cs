namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmSetPassword : frmBioAccess
    {
        public string newPassword;
        public int operatorID;

        public dfrmSetPassword()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.txtPasswordNew.Text != this.txtPasswordNewConfirm.Text)
            {
                XMessageBox.Show(this, CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.operatorID == 0)
            {
                if (this.txtPasswordNew.Text == "")
                {
                    this.txtPasswordNew.Text = "0";
                    this.txtPasswordNewConfirm.Text = "0";
                }
                long result = -1L;
                if (!long.TryParse(this.txtPasswordNew.Text, out result) || (this.txtPasswordNew.Text.Length > 6))
                {
                    XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (result <= 0L)
                {
                    XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.newPassword = this.txtPasswordNew.Text;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            else
            {
                try
                {
                    this.newPassword = this.txtPasswordNew.Text;
                    string str = "";
                    str = " UPDATE [t_s_Operator] ";
                    if (wgAppConfig.runUpdateSql((str + "SET [f_Password]=" + wgTools.PrepareStr(this.txtPasswordNew.Text)) + " WHERE [f_OperatorID]=" + this.operatorID) >= 0)
                    {
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                    XMessageBox.Show(exception.Message);
                }
            }
        }

        private void dfrmSetPassword_Load(object sender, EventArgs e)
        {
            this.txtPasswordNew.CharacterCasing = CharacterCasing.Lower;
            this.txtPasswordNewConfirm.CharacterCasing = CharacterCasing.Lower;
        }
    }
}

