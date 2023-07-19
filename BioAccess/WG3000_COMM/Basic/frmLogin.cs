namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmLogin : frmBioAccess
    {
        private Point mouse_offset;
        private Point mouseDownLocation;

        public frmLogin()
        {
            this.InitializeComponent();
            if (wgAppConfig.GetKeyVal("autologinName") != "")
            {
                this.txtOperatorName.Text = wgAppConfig.GetKeyVal("autologinName");
                this.txtPassword.Text = wgAppConfig.GetKeyVal("autologinPassword");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            /*
            if (icOperator.checkSoftwareRegister() < 0)
            {
                using (dfrmRegister register = new dfrmRegister())
                {
                    register.Text = CommonStr.strLicenseExpired;
                    if (register.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }
            } */

            wgAppConfig.UpdateKeyVal("autologinName", txtOperatorName.Text);
            wgAppConfig.UpdateKeyVal("autologinPassword", txtPassword.Text);

            if (icOperator.login(this.txtOperatorName.Text, this.txtPassword.Text))
            {
                base.DialogResult = DialogResult.OK;
                wgAppConfig.IsLogin = true;
                wgAppConfig.LoginTitle = this.Text;
                string str = "";
                str = str + string.Format("Ver: {0},", Application.ProductVersion);
                wgTools.CommPStr = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommCurrent"));
                if (!string.IsNullOrEmpty(wgTools.CommPStr))
                {
                    str = str + string.Format("Communication With Password,", new object[0]);
                }
                if (wgAppConfig.IsAccessDB)
                {
                    if (icOperator.OperatorID == 1)
                    {
                        str = str + string.Format("{2}:{0}:{1},", icOperator.OperatorName, "MsAccess", CommonStr.strSuper);
                    }
                    else
                    {
                        str = str + string.Format("{0}:{1},", icOperator.OperatorName, "MsAccess");
                    }
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        if (icOperator.OperatorID == 1)
                        {
                            str = str + string.Format("{2}:{0}:{1},", icOperator.OperatorName, connection.Database, CommonStr.strSuper);
                        }
                        else
                        {
                            str = str + string.Format("{0}:{1},", icOperator.OperatorName, connection.Database);
                        }
                    }
                    str = str + wgAppConfig.GetKeyVal("dbConnection");
                }
                wgAppConfig.wgLog(string.Format("{0},{1}", this.Text, str), EventLogEntryType.Information, null);
                base.Close();
            }
            else
            {
                SystemSounds.Beep.Play();
                XMessageBox.Show(this, CommonStr.strErrPwdOrName, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void frmLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == '\r') && ((sender.Equals(this.txtOperatorName) || sender.Equals(this.txtPassword)) || sender.Equals(this.btnOK)))
            {
                this.btnOK_Click(sender, e);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.ProductTypeOfApp != "AccessControl")
            {
                XMessageBox.Show("Wrong login file");
                this.btnExit_Click(null, null);
            }
            else
            {
                wgAppConfig.bFloorRoomManager = wgAppConfig.getParamValBoolByNO(0x91);
                if (wgAppConfig.bFloorRoomManager)
                {
                    this.Text = CommonStr.strTitleHouse;
                }
                if (wgAppConfig.getSystemParamByName("Custom Title") != "")
                {
                    this.Text = wgAppConfig.getSystemParamByName("Custom Title");
                }
                else if (wgAppConfig.GetKeyVal("Custom Title") != "")
                {
                    this.Text = wgAppConfig.GetKeyVal("Custom Title");
                }
                wgAppConfig.IsLogin = false;
                this.timer1.Enabled = true;
            }
        }

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouse_offset = Control.MousePosition;
            mouseDownLocation = base.Location;
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosition = Control.MousePosition;
                Point location = new Point(mouseDownLocation.X + mousePosition.X - mouse_offset.X,
                    mouseDownLocation.Y + mousePosition.Y - mouse_offset.Y);
                base.Location = location;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            if (wgAppConfig.GetKeyVal("autologinName") != "")
            {
                this.btnOK.PerformClick();
            }
        }
    }
}

