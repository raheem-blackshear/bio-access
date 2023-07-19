namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmUploadOption : frmBioAccess
    {
        public int checkVal;

        public dfrmUploadOption()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.checkVal = 0;
            if (this.chkBasicConfiguration.Checked)
            {
                this.checkVal++;
            }
            if (this.chkAccessPrivilege.Checked)
            {
                this.checkVal += 2;
            }
            base.Close();
        }

        private void dfrmUploadOption_Load(object sender, EventArgs e)
        {
        }
    }
}

