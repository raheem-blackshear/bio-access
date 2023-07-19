namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmPeripheralControlBoardSuper : frmBioAccess
    {
        private bool bVisibleForce;
        public int ext_warnSignalEnabled2;
        public int extControl;

        public dfrmPeripheralControlBoardSuper()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            this.extControl = 0;
            if (this.radioButton14.Checked)
            {
                this.extControl = 1;
            }
            if (this.radioButton15.Checked)
            {
                this.extControl = 2;
            }
            if (this.radioButton16.Checked)
            {
                this.extControl = 3;
            }
            if (this.radioButton17.Checked)
            {
                this.extControl = this.chkForceOutputTimeRemains.Checked ? 6 : 4;
            }
            if (this.radioButton18.Checked)
            {
                this.extControl = this.chkForceOutputTimeRemains.Checked ? 7 : 5;
            }
            this.ext_warnSignalEnabled2 = 0;
            if (this.checkBox76.Checked)
            {
                this.ext_warnSignalEnabled2 |= 1;
            }
            if (this.checkBox77.Checked)
            {
                this.ext_warnSignalEnabled2 |= 2;
            }
            if (this.checkBox78.Checked)
            {
                this.ext_warnSignalEnabled2 |= 4;
            }
            if (this.checkBox79.Checked)
            {
                this.ext_warnSignalEnabled2 |= 8;
            }
            if (this.checkBox80.Checked)
            {
                this.ext_warnSignalEnabled2 |= 0x10;
            }
            if (this.checkBox81.Checked)
            {
                this.ext_warnSignalEnabled2 |= 0x20;
            }
            if (this.checkBox82.Checked)
            {
                this.ext_warnSignalEnabled2 |= 0x40;
            }
            if (this.checkBox83.Checked)
            {
                this.ext_warnSignalEnabled2 |= 0x80;
            }
            base.Close();
        }

        private void dfrmPeripheralControlBoardSuper_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                this.bVisibleForce = true;
                this.radioButton14_CheckedChanged(null, null);
            }
        }

        private void dfrmPeripheralControlBoardSuper_Load(object sender, EventArgs e)
        {
            this.radioButton14.Checked = (this.extControl == 1) || (this.extControl == 0);
            this.radioButton15.Checked = this.extControl == 2;
            this.radioButton16.Checked = this.extControl == 3;
            this.radioButton17.Checked = (this.extControl == 4) || (this.extControl == 6);
            this.radioButton18.Checked = (this.extControl == 5) || (this.extControl == 7);
            this.bVisibleForce = (this.extControl == 7) || (this.extControl == 6);
            this.chkForceOutputTimeRemains.Visible = this.bVisibleForce;
            this.chkForceOutputTimeRemains.Checked = this.bVisibleForce;
            this.checkBox76.Checked = (this.ext_warnSignalEnabled2 & 1) > 0;
            this.checkBox77.Checked = (this.ext_warnSignalEnabled2 & 2) > 0;
            this.checkBox78.Checked = (this.ext_warnSignalEnabled2 & 4) > 0;
            this.checkBox79.Checked = (this.ext_warnSignalEnabled2 & 8) > 0;
            this.checkBox80.Checked = (this.ext_warnSignalEnabled2 & 0x10) > 0;
            this.checkBox81.Checked = (this.ext_warnSignalEnabled2 & 0x20) > 0;
            this.checkBox82.Checked = (this.ext_warnSignalEnabled2 & 0x40) > 0;
            this.checkBox83.Checked = (this.ext_warnSignalEnabled2 & 0x80) > 0;
            if (this.radioButton17.Checked || this.radioButton18.Checked)
            {
                this.diplayChkbox();
            }
            else
            {
                this.hideChkbox();
            }
        }

        private void diplayChkbox()
        {
            this.checkBox76.Visible = true;
            this.checkBox77.Visible = true;
            this.checkBox78.Visible = true;
            this.checkBox79.Visible = true;
            this.checkBox80.Visible = true;
            this.checkBox81.Visible = true;
            this.checkBox82.Visible = true;
            this.checkBox83.Visible = true;
            this.chkForceOutputTimeRemains.Visible = this.bVisibleForce;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void hideChkbox()
        {
            this.checkBox76.Visible = false;
            this.checkBox77.Visible = false;
            this.checkBox78.Visible = false;
            this.checkBox79.Visible = false;
            this.checkBox80.Visible = false;
            this.checkBox81.Visible = false;
            this.checkBox82.Visible = false;
            this.checkBox83.Visible = false;
            this.chkForceOutputTimeRemains.Visible = false;
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton17.Checked || this.radioButton18.Checked)
            {
                this.diplayChkbox();
            }
            else
            {
                this.hideChkbox();
            }
        }
    }
}

