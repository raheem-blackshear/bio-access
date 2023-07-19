namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    partial class dfrmAntiBack : frmBioAccess
    {
        public static bool bDisplayIndoorPersonMax;
        public string ControllerSN = "";
        public int retValue;

        public dfrmAntiBack()
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
            this.retValue = 0;
            if (this.radioButton1.Checked)
            {
                this.retValue = 1;
            }
            if (this.radioButton2.Checked)
            {
                this.retValue = 2;
            }
            if (this.radioButton3.Checked)
            {
                this.retValue = 3;
            }
            if (this.radioButton4.Checked)
            {
                this.retValue = 4;
            }
            if (this.chkActiveAntibackShare.Checked)
            {
                this.retValue += ((int) this.nudTotal.Value) * 10;
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void chkActiveAntibackShare_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkActiveAntibackShare.Checked)
            {
                if (!this.radioButton4.Visible)
                {
                    if (this.radioButton2.Visible)
                    {
                        this.radioButton0.Enabled = false;
                        this.radioButton1.Checked = false;
                        this.radioButton1.Enabled = false;
                        this.radioButton2.Checked = true;
                    }
                    else if (this.radioButton1.Visible)
                    {
                        this.radioButton0.Enabled = false;
                        this.radioButton1.Checked = true;
                    }
                }
                else
                {
                    this.radioButton0.Enabled = false;
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Enabled = true;
                    this.radioButton3.Enabled = false;
                    if (!this.radioButton4.Checked)
                    {
                        this.radioButton2.Checked = true;
                    }
                }
            }
            else if (wgAppConfig.getParamValBoolByNO(0x3e))
            {
                if (this.radioButton0.Visible)
                {
                    this.radioButton0.Enabled = true;
                }
                bool visible = this.radioButton1.Visible;
                if (this.radioButton2.Visible)
                {
                    this.radioButton1.Checked = false;
                    this.radioButton1.Enabled = false;
                }
                if (this.radioButton3.Visible)
                {
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Enabled = false;
                }
                if (this.radioButton4.Visible)
                {
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Enabled = true;
                    this.radioButton3.Enabled = false;
                }
            }
            else
            {
                this.radioButton0.Enabled = true;
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
                this.radioButton3.Enabled = true;
            }
        }

        private void dfrmAntiBack_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && (e.KeyValue == 0x51)) && e.Shift)
            {
                if (this.chkActiveAntibackShare.Visible)
                {
                    this.nudTotal.ReadOnly = false;
                    this.nudTotal.Maximum = 4000M;
                }
                this.chkActiveAntibackShare.Visible = true;
                this.nudTotal.Visible = true;
            }
        }

        private void dfrmAntiBack_Load(object sender, EventArgs e)
        {
            DbConnection connection;
            DbCommand command;
            string cmdText = "SELECT * FROM t_b_Controller Where f_ControllerSN = " + this.ControllerSN;
            if (wgAppConfig.IsAccessDB)
            {
                connection = new OleDbConnection(wgAppConfig.dbConString);
                command = new OleDbCommand(cmdText, connection as OleDbConnection);
            }
            else
            {
                connection = new SqlConnection(wgAppConfig.dbConString);
                command = new SqlCommand(cmdText, connection as SqlConnection);
            }
            connection.Open();
            DbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                switch (wgMjController.GetControllerType(int.Parse(this.ControllerSN)))
                {
                    case 1:
                        this.radioButton1.Text = this.checkBox11.Text;
                        this.radioButton2.Visible = false;
                        this.radioButton3.Visible = false;
                        this.radioButton4.Visible = false;
                        break;

                    case 2:
                        this.radioButton1.Text = this.checkBox21.Text;
                        this.radioButton2.Text = this.checkBox22.Text;
                        this.radioButton3.Visible = false;
                        this.radioButton4.Visible = false;
                        break;
                }
                switch ((((int) reader["f_AntiBack"]) % 10))
                {
                    case 1:
                        this.radioButton1.Checked = true;
                        break;

                    case 2:
                        this.radioButton2.Checked = true;
                        break;

                    case 3:
                        this.radioButton3.Checked = true;
                        break;

                    case 4:
                        this.radioButton4.Checked = true;
                        break;

                    default:
                        this.radioButton0.Checked = true;
                        break;
                }
                if (((int) reader["f_AntiBack"]) > 10)
                {
                    this.nudTotal.Visible = true;
                    this.chkActiveAntibackShare.Visible = true;
                    if (((((int) reader["f_AntiBack"]) - (((int) reader["f_AntiBack"]) % 10)) / 10) > 0x3e8)
                    {
                        this.nudTotal.Maximum = 4000M;
                    }
                    this.nudTotal.Value = (((int) reader["f_AntiBack"]) - (((int) reader["f_AntiBack"]) % 10)) / 10;
                    this.chkActiveAntibackShare.Checked = true;
                }
            }
            reader.Close();
            connection.Close();
            if (bDisplayIndoorPersonMax)
            {
                this.nudTotal.Visible = true;
                this.chkActiveAntibackShare.Visible = true;
            }
            if (wgAppConfig.getParamValBoolByNO(0x3e))
            {
                bool visible = this.radioButton0.Visible;
                bool flag2 = this.radioButton1.Visible;
                if (this.radioButton2.Visible)
                {
                    this.radioButton1.Checked = false;
                    this.radioButton1.Enabled = false;
                }
                if (this.radioButton3.Visible)
                {
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Enabled = false;
                }
                if (this.radioButton4.Visible)
                {
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Enabled = true;
                    this.radioButton3.Enabled = false;
                }
            }
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

