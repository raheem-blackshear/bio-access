namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmUsersCardLost : frmBioAccess
    {
        public dfrmUsersCardLost()
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
            icConsumer consumer = new icConsumer();
            if (!string.IsNullOrEmpty(this.txtf_CardNONew.Text) && consumer.isExisted(long.Parse(this.txtf_CardNONew.Text)))
            {
                XMessageBox.Show(this, CommonStr.strCardAlreadyUsed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                icConsumerShare.setUpdateLog();
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void dfrmUsersCardLost_Load(object sender, EventArgs e)
        {
            this.txtf_CardNO.Mask = "9999999999";
            this.txtf_CardNONew.Mask = "9999999999";
        }

        private void txtf_CardNONew_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtf_CardNONew);
        }

        private void txtf_CardNONew_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtf_CardNONew);
        }
    }
}

