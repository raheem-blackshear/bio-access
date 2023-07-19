namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControllerWarnSet : frmBioAccess
    {
        private dfrmFind dfrmFind1 = new dfrmFind();
        private int[] ext_active = new int[4];
        private int ext_AlarmControlMode;
        private int[] ext_controlSet = new int[4];
        private int[] ext_doorSet = new int[4];
        private int ext_SetAlarmOffDelay;
        private int ext_SetAlarmOnDelay;
        private decimal[] ext_timeoutSet = new decimal[4];
        private int[] ext_warnSignalEnabled2Set = new int[4];
        private int[] ext_warnSignalEnabledSet = new int[4];

        public dfrmControllerWarnSet()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnChangeThreatPassword_Click(object sender, EventArgs e)
        {
            using (dfrmSetPassword password = new dfrmSetPassword())
            {
                password.operatorID = 0;
                password.Text = this.btnChangeThreatPassword.Text;
                if (password.ShowDialog(this) == DialogResult.OK)
                {
                    if ((int.Parse(password.newPassword) >= 0xf423f) || (int.Parse(password.newPassword) <= 0))
                    {
                        XMessageBox.Show(this, CommonStr.strFailedNumeric999999, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (wgAppConfig.setSystemParamValue(0x18, "Threat Password", password.newPassword, "") == 1)
                    {
                        this.lblThreatPassword.Text = password.newPassword;
                        XMessageBox.Show(this, "OK", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        XMessageBox.Show(this, CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnExtension_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {
                if (this.dataGridView1.SelectedCells.Count <= 0)
                {
                    return;
                }
                int rowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
            }
            else
            {
                int index = this.dataGridView1.SelectedRows[0].Index;
            }
            int num = 0;
            DataGridView view = this.dataGridView1;
            if (view.Rows.Count > 0)
            {
                num = view.CurrentCell.RowIndex;
            }
            using (dfrmPeripheralControlBoard board = new dfrmPeripheralControlBoard())
            {
                board.ControllerNO = int.Parse(view.Rows[num].Cells[0].Value.ToString());
                board.ControllerSN = int.Parse(view.Rows[num].Cells[1].Value.ToString());
                if (board.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadData();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.btnOK.Enabled = false;
            wgAppConfig.setSystemParamValue(60, this.chkActiveFireSignalShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
            }
            else
            {
                DataTable table = (this.dataGridView1.DataSource as DataView).Table;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        for (int i = 0; i <= (table.Rows.Count - 1); i++)
                        {
                            string str = " UPDATE t_b_Controller SET ";
                            str = str + " f_DoorInvalidOpen = 0" + table.Rows[i]["f_DoorInvalidOpen"].ToString() + 
                                ", f_DoorOpenTooLong = 0" + table.Rows[i]["f_DoorOpenTooLong"].ToString() + 
                                ", f_ForceWarn  = 0" + table.Rows[i]["f_ForceWarn"].ToString() +
                                ", f_InvalidCardWarn = 0" + table.Rows[i]["f_InvalidCardWarn"].ToString() +
                                ", f_DoorTamperWarn = 0" + table.Rows[i]["f_DoorTamperWarn"].ToString() + 
                                " WHERE f_ControllerNo = " + table.Rows[i]["f_ControllerNo"].ToString();
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                wgAppConfig.setSystemParamValue(40, "", this.nudOpenDoorTimeout.Value.ToString(), "");
                base.Close();
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            DataTable table = (this.dataGridView1.DataSource as DataView).Table;
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("", connection))
                {
                    connection.Open();
                    for (int i = 0; i <= (table.Rows.Count - 1); i++)
                    {
                        string str = " UPDATE t_b_Controller SET ";
                        str = str + " f_DoorInvalidOpen = 0" + table.Rows[i]["f_DoorInvalidOpen"].ToString() + 
                            ", f_DoorOpenTooLong = 0" + table.Rows[i]["f_DoorOpenTooLong"].ToString() + 
                            ", f_ForceWarn  = 0" + table.Rows[i]["f_ForceWarn"].ToString() +
                            ", f_InvalidCardWarn = 0" + table.Rows[i]["f_InvalidCardWarn"].ToString() +
                            ", f_DoorTamperWarn = 0" + table.Rows[i]["f_DoorTamperWarn"].ToString() + 
                            " WHERE f_ControllerNo = " + table.Rows[i]["f_ControllerNo"].ToString();
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
            }
            wgAppConfig.setSystemParamValue(40, "", this.nudOpenDoorTimeout.Value.ToString(), "");
            base.Close();
        }

        private void chkActiveFireSignalShare_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkActiveFireSignalShare.Checked)
            {
                this.chkGrouped.Enabled = true;
            }
            else
            {
                this.chkGrouped.Enabled = false;
                this.chkGrouped.Checked = false;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.btnExtension.PerformClick();
        }

        private void saveAlarmSettings()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            if (index < 0)
                return;

            ext_timeoutSet[0] = nudDelay.Value;

            int ControllerNO = int.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
            string str = "";
            str = ((((((((((((((((((((((((((str + this.ext_AlarmControlMode.ToString()) + "," + this.ext_SetAlarmOnDelay.ToString()) + "," + this.ext_SetAlarmOffDelay.ToString()) + "," + this.ext_doorSet[0].ToString()) + "," + this.ext_doorSet[1].ToString()) + "," + this.ext_doorSet[2].ToString()) + "," + this.ext_doorSet[3].ToString()) + "," + this.ext_controlSet[0].ToString()) + "," + this.ext_controlSet[1].ToString()) + "," + this.ext_controlSet[2].ToString()) + "," + this.ext_controlSet[3].ToString()) + "," + this.ext_warnSignalEnabledSet[0].ToString()) + "," + this.ext_warnSignalEnabledSet[1].ToString()) + "," + this.ext_warnSignalEnabledSet[2].ToString()) + "," + this.ext_warnSignalEnabledSet[3].ToString()) + "," + this.ext_warnSignalEnabled2Set[0].ToString()) + "," + this.ext_warnSignalEnabled2Set[1].ToString()) + "," + this.ext_warnSignalEnabled2Set[2].ToString()) + "," + this.ext_warnSignalEnabled2Set[3].ToString()) + "," + this.ext_timeoutSet[0].ToString()) + "," + this.ext_timeoutSet[1].ToString()) + "," + this.ext_timeoutSet[2].ToString()) + "," + this.ext_timeoutSet[3].ToString()) + "," + this.ext_active[0].ToString()) + "," + this.ext_active[1].ToString()) + "," + this.ext_active[2].ToString()) + "," + this.ext_active[3].ToString();
            wgAppConfig.runUpdateSql((" UPDATE t_b_Controller SET f_PeripheralControl =" + wgTools.PrepareStr(str)) + "   WHERE  [f_ControllerNO] = " + ControllerNO.ToString());
        }

        private void loadAlarmSettings()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            if (index < 0)
                return;

            int ControllerNO = int.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
            int controllerID = 0;
            string cmdText = " SELECT b.f_ControllerID, b.f_PeripheralControl ";
            cmdText = cmdText + " FROM t_b_Controller b  WHERE  b.[f_ControllerNO] = " + ControllerNO.ToString();
            string str = "0";
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            str = wgTools.SetObjToStr(reader["f_PeripheralControl"]);
                            controllerID = (int)reader[0];
                        }
                        reader.Close();
                    }
                    goto Label_014D;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.Read())
                    {
                        str = wgTools.SetObjToStr(reader2["f_PeripheralControl"]);
                        controllerID = (int)reader2[0];
                    }
                    reader2.Close();
                }
            }
        Label_014D: ;
            string[] strArray = str.Split(new char[] { ',' });
            if (strArray.Length != 0x1b)
            {
                strArray = "126,30,30,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,10,10,0,0,0,0".Split(new char[] { ',' });
            }
            int num2 = 0;
            this.ext_AlarmControlMode = int.Parse(strArray[num2++]);
            this.ext_SetAlarmOnDelay = int.Parse(strArray[num2++]);
            this.ext_SetAlarmOffDelay = int.Parse(strArray[num2++]);
            this.ext_doorSet[0] = int.Parse(strArray[num2++]);
            this.ext_doorSet[1] = int.Parse(strArray[num2++]);
            this.ext_doorSet[2] = int.Parse(strArray[num2++]);
            this.ext_doorSet[3] = int.Parse(strArray[num2++]);
            this.ext_controlSet[0] = int.Parse(strArray[num2++]);
            this.ext_controlSet[1] = int.Parse(strArray[num2++]);
            this.ext_controlSet[2] = int.Parse(strArray[num2++]);
            this.ext_controlSet[3] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabledSet[0] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabledSet[1] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabledSet[2] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabledSet[3] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabled2Set[0] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabled2Set[1] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabled2Set[2] = int.Parse(strArray[num2++]);
            this.ext_warnSignalEnabled2Set[3] = int.Parse(strArray[num2++]);
            nudDelay.Value = this.ext_timeoutSet[0] = decimal.Parse(strArray[num2++]);
            this.ext_timeoutSet[1] = decimal.Parse(strArray[num2++]);
            this.ext_timeoutSet[2] = decimal.Parse(strArray[num2++]);
            this.ext_timeoutSet[3] = decimal.Parse(strArray[num2++]);
            this.ext_active[0] = int.Parse(strArray[num2++]);
            this.ext_active[1] = int.Parse(strArray[num2++]);
            this.ext_active[2] = int.Parse(strArray[num2++]);
            this.ext_active[3] = int.Parse(strArray[num2++]);
        }

        private void dfrmControllerInterLock_Load(object sender, EventArgs e)
        {
            this.lblThreatPassword.Text = "889988";
            this.loadData();
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Select();
                loadAlarmSettings();
            }

            this.lblThreatPassword.Text = wgAppConfig.getSystemParamByNO(0x18);
            this.chkActiveFireSignalShare.Checked = wgAppConfig.getParamValBoolByNO(60);
            this.chkActiveFireSignalShare.Visible = this.chkActiveFireSignalShare.Checked;
            if (this.chkActiveFireSignalShare.Visible && (wgAppConfig.getSystemParamByNO(60) == "2"))
            {
                this.chkGrouped.Checked = true;
                this.chkGrouped.Visible = true;
            }
            this.nudOpenDoorTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(40));
            this.loadOperatorPrivilege();
        }

        private void dfrmControllerWarnSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmControllerWarnSet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 0x51)) && e.Shift)
                {
                    if (this.chkActiveFireSignalShare.Visible)
                    {
                        this.chkGrouped.Visible = true;
                    }
                    this.chkActiveFireSignalShare.Visible = true;
                    this.chkActiveFireSignalShare_CheckedChanged(null, null);
                }
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                    }
                    this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.dfrmFind1 != null))
            {
                this.dfrmFind1.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadData()
        {
            string strSql = " SELECT ";
            strSql = strSql + " f_ControllerNO " + ", f_ControllerSN " + ", f_ForceWarn " + ", f_DoorOpenTooLong " + ", f_DoorInvalidOpen " + ", f_InvalidCardWarn " + ", f_DoorTamperWarn" + ", f_DoorNames " + ", f_ZoneID " + " from t_b_Controller  " + " ORDER BY f_ControllerNO ";
            wgAppConfig.fillDGVData(ref this.dataGridView1, strSql);
            DataTable dtController = ((DataView) this.dataGridView1.DataSource).Table;
            new icControllerZone().getAllowedControllers(ref dtController);
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuExtendedFunction";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnExtension.Visible = false;
                this.btnChangeThreatPassword.Enabled = false;
                this.btnOK.Visible = false;
                this.nudOpenDoorTimeout.ReadOnly = true;
                this.nudOpenDoorTimeout.Enabled = false;
                this.dataGridView1.ReadOnly = true;
            }
        }

        private void nudDelay_Leave(object sender, EventArgs e)
        {
            saveAlarmSettings();
        }

        private void nudDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                saveAlarmSettings();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            loadAlarmSettings();
        }
    }
}

