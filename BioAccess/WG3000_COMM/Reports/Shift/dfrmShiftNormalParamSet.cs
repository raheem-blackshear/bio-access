namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftNormalParamSet : frmBioAccess
    {
        public dfrmShiftNormalParamSet()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.nudLateTimeout.Value >= this.nudLateAbsenceTimeout.Value)
            {
                XMessageBox.Show(this, CommonStr.strShiftNormalParamSet1, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.optReadCardTwoTimes.Checked && (this.dtpOffduty0.Value <= this.dtpOnduty0.Value))
            {
                XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (this.optReadCardFourTimes.Checked)
                {
                    if (this.dtpOffduty1.Value <= this.dtpOnduty1.Value)
                    {
                        XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.dtpOffduty2.Value <= this.dtpOnduty2.Value)
                    {
                        XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.dtpOffduty2.Value <= this.dtpOnduty1.Value)
                    {
                        XMessageBox.Show(this, CommonStr.strShiftNormalParamSet2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                for (int i = 1; i <= 14; i++)
                {
                    switch (i)
                    {
                        case 1:
                        {
                            this.setAttendanceParam(i, this.nudLateTimeout.Value.ToString());
                            continue;
                        }
                        case 2:
                        {
                            this.setAttendanceParam(i, this.nudLateAbsenceTimeout.Value.ToString());
                            continue;
                        }
                        case 3:
                        {
                            this.setAttendanceParam(i, (this.cboLateAbsenceDay.SelectedIndex / 2M).ToString(CultureInfo.InvariantCulture));
                            continue;
                        }
                        case 4:
                        {
                            this.setAttendanceParam(i, this.nudLeaveTimeout.Value.ToString());
                            continue;
                        }
                        case 5:
                        {
                            this.setAttendanceParam(i, this.nudLeaveAbsenceTimeout.Value.ToString());
                            continue;
                        }
                        case 6:
                        {
                            this.setAttendanceParam(i, (this.cboLeaveAbsenceDay.SelectedIndex / 2M).ToString(CultureInfo.InvariantCulture));
                            continue;
                        }
                        case 7:
                        {
                            this.setAttendanceParam(i, this.nudOvertimeTimeout.Value.ToString());
                            continue;
                        }
                        case 8:
                        {
                            this.setAttendanceParam(i, this.dtpOnduty0.Value.ToString(wgTools.YMDHMSFormat));
                            continue;
                        }
                        case 9:
                        {
                            this.setAttendanceParam(i, this.dtpOffduty0.Value.ToString(wgTools.YMDHMSFormat));
                            continue;
                        }
                        case 10:
                        {
                            this.setAttendanceParam(i, this.dtpOnduty1.Value.ToString(wgTools.YMDHMSFormat));
                            continue;
                        }
                        case 11:
                        {
                            this.setAttendanceParam(i, this.dtpOffduty1.Value.ToString(wgTools.YMDHMSFormat));
                            continue;
                        }
                        case 12:
                        {
                            this.setAttendanceParam(i, this.dtpOnduty2.Value.ToString(wgTools.YMDHMSFormat));
                            continue;
                        }
                        case 13:
                        {
                            this.setAttendanceParam(i, this.dtpOffduty2.Value.ToString(wgTools.YMDHMSFormat));
                            continue;
                        }
                        case 14:
                        {
                            if (!this.optReadCardTwoTimes.Checked)
                            {
                                break;
                            }
                            this.setAttendanceParam(i, "2");
                            continue;
                        }
                        default:
                        {
                            continue;
                        }
                    }
                    this.setAttendanceParam(i, "4");
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            using (dfrmShiftNormalOption option = new dfrmShiftNormalOption())
            {
                option.ShowDialog();
            }
        }

        private void dfrmShiftNormalParamSet_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmShiftNormalParamSet_Load(object sender, EventArgs e)
        {
            this.dtpOnduty0.CustomFormat = "HH:mm";
            this.dtpOnduty0.Format = DateTimePickerFormat.Custom;
            this.dtpOnduty0.Value = DateTime.Parse("08:30:00");
            this.dtpOffduty0.CustomFormat = "HH:mm";
            this.dtpOffduty0.Format = DateTimePickerFormat.Custom;
            this.dtpOffduty0.Value = DateTime.Parse("17:30:00");
            this.dtpOnduty1.CustomFormat = "HH:mm";
            this.dtpOnduty1.Format = DateTimePickerFormat.Custom;
            this.dtpOnduty1.Value = DateTime.Parse("08:30:00");
            this.dtpOffduty1.CustomFormat = "HH:mm";
            this.dtpOffduty1.Format = DateTimePickerFormat.Custom;
            this.dtpOffduty1.Value = DateTime.Parse("12:00:00");
            this.dtpOnduty2.CustomFormat = "HH:mm";
            this.dtpOnduty2.Format = DateTimePickerFormat.Custom;
            this.dtpOnduty2.Value = DateTime.Parse("13:30:00");
            this.dtpOffduty2.CustomFormat = "HH:mm";
            this.dtpOffduty2.Format = DateTimePickerFormat.Custom;
            this.dtpOffduty2.Value = DateTime.Parse("17:30:00");
            this.loadOperatorPrivilege();
            this.getAttendanceParam();
            try
            {
                if (!(wgAppConfig.getSystemParamByNO(0x37).ToString() == "00:00") && !(wgAppConfig.getSystemParamByNO(0x37).ToString() == "00:00:00"))
                {
                    this.btnOption.Visible = true;
                }
                if (wgAppConfig.getSystemParamByNO(0x38).ToString() == "1")
                {
                    this.btnOption.Visible = true;
                }
                if (wgAppConfig.getSystemParamByNO(0x39).ToString() == "1")
                {
                    this.btnOption.Visible = true;
                }
                if (wgAppConfig.getSystemParamByNO(0x36).ToString() == "1")
                {
                    this.btnOption.Visible = true;
                }
                if (wgAppConfig.getSystemParamByNO(0x3b).ToString() == "1")
                {
                    this.btnOption.Visible = true;
                }
            }
            catch
            {
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

        private void funcCtrlShiftQ()
        {
            this.btnOption.Visible = true;
        }

        private void getAttendanceParam()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.getAttendanceParam_Acc();
            }
            else
            {
                string cmdText = "SELECT * FROM t_a_Attendence";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (((int) reader["f_No"]))
                            {
                                case 1:
                                {
                                    this.nudLateTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 2:
                                {
                                    this.nudLateAbsenceTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 3:
                                {
                                    this.cboLateAbsenceDay.SelectedIndex = (int) (decimal.Parse(reader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2M);
                                    continue;
                                }
                                case 4:
                                {
                                    this.nudLeaveTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 5:
                                {
                                    this.nudLeaveAbsenceTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 6:
                                {
                                    this.cboLeaveAbsenceDay.SelectedIndex = (int) (decimal.Parse(reader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2M);
                                    continue;
                                }
                                case 7:
                                {
                                    this.nudOvertimeTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 8:
                                {
                                    this.dtpOnduty0.Value = DateTime.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 9:
                                {
                                    this.dtpOffduty0.Value = DateTime.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 10:
                                {
                                    this.dtpOnduty1.Value = DateTime.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 11:
                                {
                                    this.dtpOffduty1.Value = DateTime.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 12:
                                {
                                    this.dtpOnduty2.Value = DateTime.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 13:
                                {
                                    this.dtpOffduty2.Value = DateTime.Parse((string) reader["f_Value"]);
                                    continue;
                                }
                                case 14:
                                {
                                    if (int.Parse((string) reader["f_Value"]) != 4)
                                    {
                                        break;
                                    }
                                    this.optReadCardFourTimes.Checked = true;
                                    this.grpbFourtimes.Visible = true;
                                    this.grpbTwoTimes.Visible = false;
                                    continue;
                                }
                                default:
                                {
                                    continue;
                                }
                            }
                            this.optReadCardTwoTimes.Checked = true;
                            this.grpbFourtimes.Visible = false;
                            this.grpbTwoTimes.Visible = true;
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void getAttendanceParam_Acc()
        {
            string cmdText = "SELECT * FROM t_a_Attendence";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        switch (((int) reader["f_No"]))
                        {
                            case 1:
                            {
                                this.nudLateTimeout.Value = int.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 2:
                            {
                                this.nudLateAbsenceTimeout.Value = int.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 3:
                            {
                                this.cboLateAbsenceDay.SelectedIndex = (int) (decimal.Parse(reader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2M);
                                continue;
                            }
                            case 4:
                            {
                                this.nudLeaveTimeout.Value = int.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 5:
                            {
                                this.nudLeaveAbsenceTimeout.Value = int.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 6:
                            {
                                this.cboLeaveAbsenceDay.SelectedIndex = (int) (decimal.Parse(reader["f_Value"].ToString(), CultureInfo.InvariantCulture) * 2M);
                                continue;
                            }
                            case 7:
                            {
                                this.nudOvertimeTimeout.Value = int.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 8:
                            {
                                this.dtpOnduty0.Value = DateTime.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 9:
                            {
                                this.dtpOffduty0.Value = DateTime.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 10:
                            {
                                this.dtpOnduty1.Value = DateTime.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 11:
                            {
                                this.dtpOffduty1.Value = DateTime.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 12:
                            {
                                this.dtpOnduty2.Value = DateTime.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 13:
                            {
                                this.dtpOffduty2.Value = DateTime.Parse((string) reader["f_Value"]);
                                continue;
                            }
                            case 14:
                            {
                                if (int.Parse((string) reader["f_Value"]) != 4)
                                {
                                    break;
                                }
                                this.optReadCardFourTimes.Checked = true;
                                this.grpbFourtimes.Visible = true;
                                this.grpbTwoTimes.Visible = false;
                                continue;
                            }
                            default:
                            {
                                continue;
                            }
                        }
                        this.optReadCardTwoTimes.Checked = true;
                        this.grpbFourtimes.Visible = false;
                        this.grpbTwoTimes.Visible = true;
                    }
                    reader.Close();
                }
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuShiftNormalConfigure";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnOK.Visible = false;
            }
        }

        private void optReadCardFourTimes_CheckedChanged(object sender, EventArgs e)
        {
            this.grpbTwoTimes.Visible = this.optReadCardTwoTimes.Checked;
            this.grpbFourtimes.Visible = this.optReadCardFourTimes.Checked;
        }

        private void optReadCardTwoTimes_CheckedChanged(object sender, EventArgs e)
        {
            this.grpbTwoTimes.Visible = this.optReadCardTwoTimes.Checked;
            this.grpbFourtimes.Visible = this.optReadCardFourTimes.Checked;
        }

        private void setAttendanceParam(int no, string val)
        {
            wgAppConfig.runUpdateSql(("UPDATE t_a_Attendence " + " SET [f_value]=" + wgTools.PrepareStr(val)) + " WHERE [f_NO]= " + no.ToString());
        }
    }
}

