namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmInputNewName : frmBioAccess
    {
        public bool bNotAllowNull = true;
        public string strNewName = "";

        public dfrmInputNewName()
        {
            this.InitializeComponent();
            this.btnCancel.DialogResult = DialogResult.Cancel;
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.bNotAllowNull && string.IsNullOrEmpty(this.txtNewName.Text))
            {
                this.label2.Visible = true;
            }
            else
            {
                if (string.IsNullOrEmpty(this.txtNewName.Text))
                {
                    this.strNewName = "";
                }
                else
                {
                    this.strNewName = this.txtNewName.Text.Trim();
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void dfrmInputNewName_Load(object sender, EventArgs e)
        {
            this.txtNewName.Text = this.strNewName;
        }

        public void setPasswordChar(char val)
        {
            this.txtNewName.PasswordChar = val;
        }
    }
}

