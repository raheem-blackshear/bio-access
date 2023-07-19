namespace WG3000_COMM.ExtendFunc.Map
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmSelectMapDoor : frmBioAccess
    {
        public bool bAddDoor = true;
        private dfrmFind dfrmFind1 = new dfrmFind();
        public string doorName;

        public dfrmSelectMapDoor()
        {
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.lstUnMappedDoors.SelectedItems.Count > 0)
            {
                this.doorName = this.lstUnMappedDoors.SelectedItem.ToString();
                base.DialogResult = DialogResult.OK;
            }
            else if (this.lstMappedDoors.SelectedItems.Count > 0)
            {
                this.doorName = this.lstMappedDoors.SelectedItem.ToString();
                this.bAddDoor = false;
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
        }

        private void dfrmSelectMapDoor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmSelectMapDoor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                    }
                    this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
                    this.dfrmFind1.Focus();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmSelectMapDoor_Load(object sender, EventArgs e)
        {
            base.KeyPreview = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void lstMappedDoors_DoubleClick(object sender, EventArgs e)
        {
            this.btnOK.PerformClick();
        }

        private void lstMappedDoors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstMappedDoors.SelectedItems.Count > 0)
            {
                this.lstUnMappedDoors.SelectedIndex = -1;
            }
        }

        private void lstUnMappedDoors_DoubleClick(object sender, EventArgs e)
        {
            this.btnOK.PerformClick();
        }

        private void lstUnMappedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnOK.PerformClick();
        }

        private void lstUnMappedDoors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstUnMappedDoors.SelectedItems.Count > 0)
            {
                this.lstMappedDoors.SelectedIndex = -1;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = btnCancel.DialogResult;
            Close();
        }
    }
}

