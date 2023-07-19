namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmControllerZoneSelect : frmBioAccess
    {
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        public int selectZoneId = -1;

        public dfrmControllerZoneSelect()
        {
            this.InitializeComponent();
            this.btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cboZone.Items.Count > 0)
            {
                this.selectZoneId = (int) this.arrZoneID[this.cboZone.SelectedIndex];
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }

        private void dfrmControllerZoneSelect_Load(object sender, EventArgs e)
        {
            this.loadZoneInfo();
        }

        private void loadZoneInfo()
        {
            new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
            int count = this.arrZoneID.Count;
            this.cboZone.Items.Clear();
            for (count = 0; count < this.arrZoneID.Count; count++)
            {
                if ((count == 0) && string.IsNullOrEmpty(this.arrZoneName[count].ToString()))
                {
                    this.cboZone.Items.Add("");
                }
                else
                {
                    this.cboZone.Items.Add(this.arrZoneName[count].ToString());
                }
            }
            if (this.cboZone.Items.Count > 0)
            {
                this.cboZone.SelectedIndex = 0;
            }
            this.cboZone.Visible = true;
        }
    }
}

