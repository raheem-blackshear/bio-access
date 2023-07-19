namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftOtherParamSet : frmBioAccess
    {
        public dfrmShiftOtherParamSet()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.setShiftOtherAttendanceParam(1, this.nudLateTimeout.Value.ToString());
            this.setShiftOtherAttendanceParam(4, this.nudLeaveTimeout.Value.ToString());
            this.setShiftOtherAttendanceParam(7, this.nudOvertimeTimeout.Value.ToString());
            this.setShiftOtherAttendanceParam(0x11, this.nudAheadMinutes.Value.ToString());
            this.setShiftOtherAttendanceParam(0x12, this.nudAheadMinutes.Value.ToString());
            this.setShiftOtherAttendanceParam(0x13, this.nudAheadMinutes.Value.ToString());
            this.setShiftOtherAttendanceParam(20, this.nudOvertimeMinutes.Value.ToString());
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void dfrmShiftOtherParamSet_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmShiftOtherParamSet_Load(object sender, EventArgs e)
        {
            this.loadOperatorPrivilege();
            this.getShiftOtherAttendanceParam();
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
            this.Label2.Visible = true;
            this.Label14.Visible = true;
            this.nudOvertimeMinutes.Visible = true;
        }

        private void getShiftOtherAttendanceParam()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.getShiftOtherAttendanceParam_Acc();
            }
            else
            {
                string cmdText = "SELECT * FROM t_a_Shift_Attendence";
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
                                case 0x12:
                                    this.nudAheadMinutes.Value = int.Parse((string) reader["f_Value"]);
                                    break;

                                case 20:
                                    this.nudOvertimeMinutes.Value = int.Parse((string) reader["f_Value"]);
                                    break;

                                case 7:
                                    this.nudOvertimeTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    break;

                                case 1:
                                    this.nudLateTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    break;

                                case 4:
                                    this.nudLeaveTimeout.Value = int.Parse((string) reader["f_Value"]);
                                    break;
                            }
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void getShiftOtherAttendanceParam_Acc()
        {
            string cmdText = "SELECT * FROM t_a_Shift_Attendence";
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
                            case 0x12:
                                this.nudAheadMinutes.Value = int.Parse((string) reader["f_Value"]);
                                break;

                            case 20:
                                this.nudOvertimeMinutes.Value = int.Parse((string) reader["f_Value"]);
                                break;

                            case 7:
                                this.nudOvertimeTimeout.Value = int.Parse((string) reader["f_Value"]);
                                break;

                            case 1:
                                this.nudLateTimeout.Value = int.Parse((string) reader["f_Value"]);
                                break;

                            case 4:
                                this.nudLeaveTimeout.Value = int.Parse((string) reader["f_Value"]);
                                break;
                        }
                    }
                    reader.Close();
                }
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuShiftRule";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnOK.Visible = false;
            }
        }

        private void setShiftOtherAttendanceParam(int no, string val)
        {
            wgAppConfig.runUpdateSql(("UPDATE t_a_Shift_Attendence " + " SET [f_value]=" + wgTools.PrepareStr(val)) + " WHERE [f_NO]= " + no.ToString());
        }
    }
}

