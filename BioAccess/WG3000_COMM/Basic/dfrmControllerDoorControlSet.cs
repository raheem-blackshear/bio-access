namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmControllerDoorControlSet : frmBioAccess
    {
        public int doorControl = -1;

        public dfrmControllerDoorControlSet()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOnline_Click(object sender, EventArgs e)
        {
            if (sender == this.btnOnline)
            {
                this.doorControl = 3;
            }
            else if (sender == this.btnNormalClose)
            {
                this.doorControl = 2;
            }
            else if (sender == this.btnNormalOpen)
            {
                this.doorControl = 1;
            }
            else
            {
                this.btnCancel.PerformClick();
                return;
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
    }
}

