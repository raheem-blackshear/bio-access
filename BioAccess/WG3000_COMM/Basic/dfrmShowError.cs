namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmShowError : frmBioAccess
    {
        public string errInfo = "";

        public dfrmShowError()
        {
            this.InitializeComponent();
        }

        private void btnCopyDetail_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtErrorDetail.Text = this.errInfo;
                Clipboard.SetDataObject(this.txtErrorDetail.Text, false);
                this.btnDetail.Enabled = false;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            base.Height = 320;
            this.btnDetail.Visible = false;
            this.btnCopyDetail.Visible = true;
            try
            {
                this.txtErrorDetail.Visible = true;
                this.txtErrorDetail.Text = this.errInfo;
                Clipboard.SetDataObject(this.txtErrorDetail.Text, false);
                this.btnDetail.Enabled = false;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.Close();
            base.DialogResult = DialogResult.OK;
        }

        private void dfrmShowError_Load(object sender, EventArgs e)
        {
            bool flag1 = this.errInfo != "";
        }
    }
}

