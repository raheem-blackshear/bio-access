namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmCommPSet : frmBioAccess
    {
        public bool bChangedPwd;
        public string CurrentPwd = "";

        public dfrmCommPSet()
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
            else if (this.txtPasswordPrev.Text != this.txtPasswordPrevConfirm.Text)
            {
                XMessageBox.Show(this, this.label1.Text + "\r\n\r\n" + CommonStr.strPwdNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.CurrentPwd = this.txtPasswordNew.Text.Trim();
                if (this.bChangedPwd)
                {
                    if (string.IsNullOrEmpty(this.txtPasswordPrev.Text.Trim()))
                    {
                        wgTools.CommPStr = "";
                    }
                    else
                    {
                        wgTools.CommPStr = WGPacket.Ept(this.txtPasswordPrev.Text.Trim());
                    }
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void dfrmCommPSet_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && !e.Shift) && ((e.KeyValue == 120) && !this.bChangedPwd))
            {
                base.Size = new Size(base.Size.Width, 310);
                this.groupBox1.Visible = true;
                this.bChangedPwd = true;
            }
        }

        private void dfrmCommPSet_Load(object sender, EventArgs e)
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
    }
}

