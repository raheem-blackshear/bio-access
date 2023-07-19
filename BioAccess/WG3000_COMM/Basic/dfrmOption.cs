namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmOption : frmBioAccess
    {
        public dfrmOption()
        {
            this.InitializeComponent();
            this.cmdCancel.DialogResult = DialogResult.Cancel;
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            using (dfrmOptionAdvanced advanced = new dfrmOptionAdvanced())
            {
                advanced.ShowDialog();
            }
        }

        private void btnRefreshDateTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.cboDateTime.Text))
                {
                    this.txtDateTime.Text = "";
                }
                else
                {
                    DateTime time;
                    this.txtDateTime.Text = DateTime.Now.ToString(this.cboDateTime.Text);
                    if (!DateTime.TryParse(this.txtDateTime.Text, out time))
                    {
                        this.txtDateTime.Text = CommonStr.strDateTimeFormatErr;
                    }
                }
            }
            catch (Exception)
            {
                this.txtDateTime.Text = CommonStr.strDateTimeFormatErr;
            }
        }

        private void btnRefreshDateTimeWeek_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.cboDateTimeWeek.Text))
                {
                    this.txtDateTimeWeek.Text = "";
                }
                else
                {
                    DateTime time;
                    this.txtDateTimeWeek.Text = DateTime.Now.ToString(this.cboDateTimeWeek.Text);
                    if (!DateTime.TryParse(this.txtDateTimeWeek.Text, out time))
                    {
                        this.txtDateTimeWeek.Text = CommonStr.strDateTimeFormatErr;
                    }
                }
            }
            catch (Exception)
            {
                this.txtDateTimeWeek.Text = CommonStr.strDateTimeFormatErr;
            }
        }

        private void btnRefreshDateWeek_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.cboDateWeek.Text))
                {
                    this.txtDateWeek.Text = "";
                }
                else
                {
                    DateTime time;
                    this.txtDateWeek.Text = DateTime.Now.ToString(this.cboDateWeek.Text);
                    if (!DateTime.TryParse(this.txtDateWeek.Text, out time))
                    {
                        this.txtDateWeek.Text = CommonStr.strDateTimeFormatErr;
                    }
                }
            }
            catch (Exception)
            {
                this.txtDateWeek.Text = CommonStr.strDateTimeFormatErr;
            }
        }

        private void btnRefreshOnlyDate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.cboOnlyDate.Text))
                {
                    this.txtOnlyDate.Text = "";
                }
                else
                {
                    DateTime time;
                    this.txtOnlyDate.Text = DateTime.Now.ToString(this.cboOnlyDate.Text);
                    if (!DateTime.TryParse(this.txtOnlyDate.Text, out time))
                    {
                        this.txtOnlyDate.Text = CommonStr.strDateTimeFormatErr;
                    }
                }
            }
            catch (Exception)
            {
                this.txtOnlyDate.Text = CommonStr.strDateTimeFormatErr;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (this.cboLanguage.SelectedIndex == 0)
            {
                wgAppConfig.UpdateKeyVal("Language", "");
            }
            else if (this.cboLanguage.SelectedIndex == 1)
            {
                wgAppConfig.UpdateKeyVal("Language", "zh-CHS");
            }
            else if (this.cboLanguage.SelectedIndex >= 2)
            {
                wgAppConfig.UpdateKeyVal("Language", this.cboLanguage.Items[this.cboLanguage.SelectedIndex].ToString());
            }
            if (this.chkAutoLoginOnly.Checked)
            {
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand("SELECT * FROM t_s_Operator WHERE f_OperatorID= " + icOperator.OperatorID, connection))
                        {
                            connection.Open();
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                wgAppConfig.UpdateKeyVal("autologinName", wgTools.SetObjToStr(reader["f_OperatorName"]));
                                wgAppConfig.UpdateKeyVal("autologinPassword", wgTools.SetObjToStr(reader["f_Password"]));
                            }
                            reader.Close();
                        }
                        goto Label_01D3;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand("SELECT * FROM t_s_Operator WHERE f_OperatorID= " + icOperator.OperatorID, connection2))
                    {
                        connection2.Open();
                        SqlDataReader reader2 = command2.ExecuteReader();
                        if (reader2.Read())
                        {
                            wgAppConfig.UpdateKeyVal("autologinName", wgTools.SetObjToStr(reader2["f_OperatorName"]));
                            wgAppConfig.UpdateKeyVal("autologinPassword", wgTools.SetObjToStr(reader2["f_Password"]));
                        }
                        reader2.Close();
                    }
                    goto Label_01D3;
                }
            }
            wgAppConfig.UpdateKeyVal("autologinName", "");
            wgAppConfig.UpdateKeyVal("autologinPassword", "");
        Label_01D3:
            wgAppConfig.setSystemParamValueBool(0x91, this.chkHouse.Checked);
            wgAppConfig.UpdateKeyVal("HideGettingStartedWhenLogin", this.chkHideLogin.Checked ? "0" : "1");
            if (this.tabControl1.Visible)
            {
                if (wgTools.IsValidDateTimeFormat(this.cboOnlyDate.Text))
                {
                    wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMD", this.cboOnlyDate.Text);
                }
                if (wgTools.IsValidDateTimeFormat(this.cboDateWeek.Text))
                {
                    wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMDWeek", this.cboDateWeek.Text);
                }
                if (wgTools.IsValidDateTimeFormat(this.cboDateTime.Text))
                {
                    wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMDHMS", this.cboDateTime.Text);
                }
                if (wgTools.IsValidDateTimeFormat(this.cboDateTimeWeek.Text))
                {
                    wgAppConfig.UpdateKeyVal("DisplayFormat_DateYMDHMSWeek", this.cboDateTimeWeek.Text);
                }
            }
            if (XMessageBox.Show(CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }

        private void dfrmOption_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmOption_Load(object sender, EventArgs e)
        {
            this.chkHideLogin.Checked = wgAppConfig.GetKeyVal("HideGettingStartedWhenLogin") != "1";
            this.chkAutoLoginOnly.Checked = wgAppConfig.GetKeyVal("autologinName") != "";
            this.software_language_check();
            this.tabPage1.BackColor = this.BackColor;
            this.tabPage2.BackColor = this.BackColor;
            this.chkHouse.Checked = wgAppConfig.bFloorRoomManager;
            if ((icOperator.OperatorID == 1) && (((wgAppConfig.GetKeyVal("AllowUploadUserName") == "1") || !string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(0x29))) || wgAppConfig.getParamValBoolByNO(0x93)))
            {
                this.btnOption.Visible = true;
            }
            this.cboDateTime.Items.Clear();
            this.cboDateTime.Items.AddRange(new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-M-d HH:mm:ss", "yy-M-d HH:mm:ss", "yy-MM-dd HH:mm:ss", "HH:mm:ss dd-MMM-yy", "HH:mm:ss d/M/yyyy", "HH:mm:ss d/M/yy", "HH:mm:ss dd/MM/yy", "yy/M/d HH:mm:ss", "yy/MM/dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "HH:mm:ss M/d/yyyy", "HH:mm:ss M/d/yy", "HH:mm:ss MM/dd/yyyy" });
            this.cboDateTimeWeek.Items.Clear();
            this.cboDateTimeWeek.Items.AddRange(new string[] { "yyyy-MM-dd HH:mm:ss dddd", "yyyy-MM-dd HH:mm:ss ddd", "yyyy-M-d HH:mm:ss ddd", "yy-M-d HH:mm:ss ddd", "yy-MM-dd HH:mm:ss ddd", "HH:mm:ss dd-MMM-yy ddd", "HH:mm:ss d/M/yyyy ddd", "HH:mm:ss d/M/yy ddd", "HH:mm:ss dd/MM/yy ddd", "yy/M/d HH:mm:ss ddd", "yy/MM/dd HH:mm:ss ddd", "yyyy/MM/dd HH:mm:ss ddd", "HH:mm:ss M/d/yyyy ddd", "HH:mm:ss M/d/yy ddd", "HH:mm:ss MM/dd/yyyy ddd" });
            this.cboDateWeek.Items.Clear();
            this.cboDateWeek.Items.AddRange(new string[] { "yyyy-MM-dd dddd", "yyyy-MM-dd ddd", "yyyy-M-d ddd", "yy-M-d ddd", "yy-MM-dd ddd", "dd-MMM-yy ddd", "d/M/yyyy ddd", "d/M/yy ddd", "dd/MM/yy ddd", "yy/M/d ddd", "yy/MM/dd ddd", "yyyy/MM/dd ddd", "M/d/yyyy ddd", "M/d/yy ddd", "MM/dd/yyyy ddd" });
            this.cboOnlyDate.Items.Clear();
            this.cboOnlyDate.Items.AddRange(new string[] { "yyyy-MM-dd", "yyyy-M-d", "yy-M-d", "yy-MM-dd", "dd-MMM-yy", "d/M/yyyy", "d/M/yy", "dd/MM/yy", "yy/M/d", "yy/MM/dd", "yyyy/MM/dd", "M/d/yyyy", "M/d/yy", "MM/dd/yyyy" });
        }

        private void funcCtrlShiftQ()
        {
            if (!this.btnOption.Visible)
            {
                this.btnOption.Visible = true;
            }
            else
            {
                this.tabControl1.Visible = true;
                base.Size = new Size(640, 480);
                try
                {
                    wgAppConfig.GetKeyVal("Language");
                    this.cboOnlyDate.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMD");
                    this.cboDateWeek.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek");
                    this.cboDateTime.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS");
                    this.cboDateTimeWeek.Text = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek");
                }
                catch (Exception)
                {
                }
            }
        }

        private void software_language_check()
        {
            this.cboLanguage.Items.Clear();
            this.cboLanguage.Items.Add("English");
            this.cboLanguage.SelectedIndex = 0;
            this.cboLanguage.Items.Add("조선어");
            if (wgAppConfig.GetKeyVal("Language") == "zh-CHS")
            {
                this.cboLanguage.SelectedIndex = 1;
            }
            DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
            foreach (DirectoryInfo info2 in info.GetDirectories())
            {
                foreach (FileInfo info3 in info2.GetFiles())
                {
                    if (info3.Name == "BioAccess.resources.dll")
                    {
                        wgTools.WriteLine(info3.FullName);
                        if (info2.Name != "zh-CHS")
                        {
                            this.cboLanguage.Items.Add(info2.Name);
                            if (wgAppConfig.GetKeyVal("Language") == info2.Name)
                            {
                                this.cboLanguage.SelectedIndex = this.cboLanguage.Items.Count - 1;
                            }
                        }
                    }
                }
            }
        }
    }
}

