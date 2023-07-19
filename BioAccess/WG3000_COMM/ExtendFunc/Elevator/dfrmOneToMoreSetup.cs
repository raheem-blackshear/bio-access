namespace WG3000_COMM.ExtendFunc.Elevator
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmOneToMoreSetup : frmBioAccess
    {
        public dfrmOneToMoreSetup()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void dfrmOneToMoreSetup_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.funcCtrlShiftQ();
                }
            }
        }

        private void dfrmOneToMoreSetup_Load(object sender, EventArgs e)
        {
            this.radioButton0_CheckedChanged(null, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void funcCtrlShiftQ()
        {
            base.Size = new Size(0x22a, 0x103);
        }
        
        private void radioButton0_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox0.Visible = false;
            this.textBox1.Visible = false;
            this.textBox2.Visible = false;
            this.textBox3.Visible = false;
            this.textBox4.Visible = false;
            this.textBox5.Visible = false;
            if (this.radioButton1.Checked)
            {
                this.textBox2.Visible = true;
                this.textBox3.Visible = true;
            }
            else if (this.radioButton2.Checked)
            {
                this.textBox4.Visible = true;
                this.textBox5.Visible = true;
            }
            else
            {
                this.textBox0.Visible = true;
                this.textBox1.Visible = true;
            }
        }
    }
}

