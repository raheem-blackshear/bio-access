namespace WG3000_COMM.ExtendFunc
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

    public partial class dfrmPeripheralControlBoard : frmBioAccess
    {
        private string chkActiveDefault = "";
        public int ControllerNO;
        public int ControllerSN;
        private int[] ext_active = new int[4];
        private int ext_AlarmControlMode;
        private int[] ext_controlSet = new int[4];
        private int[] ext_doorSet = new int[4];
        private int ext_SetAlarmOffDelay;
        private int ext_SetAlarmOnDelay;
        private decimal[] ext_timeoutSet = new decimal[4];
        private int[] ext_warnSignalEnabled2Set = new int[4];
        private int[] ext_warnSignalEnabledSet = new int[4];
        private int lastTabIndex;

        public dfrmPeripheralControlBoard()
        {
            this.InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.updateParamExt(this.tabControl1.SelectedIndex);
            this.saveParmExt();
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            using (dfrmPeripheralControlBoardSuper super = new dfrmPeripheralControlBoardSuper())
            {
                super.extControl = this.ext_controlSet[this.tabControl1.SelectedIndex];
                super.ext_warnSignalEnabled2 = this.ext_warnSignalEnabled2Set[this.tabControl1.SelectedIndex];
                if (super.ShowDialog(this) == DialogResult.OK)
                {
                    this.ext_controlSet[this.tabControl1.SelectedIndex] = super.extControl;
                    this.ext_warnSignalEnabled2Set[this.tabControl1.SelectedIndex] = super.ext_warnSignalEnabled2;
                }
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            this.grpSet.Visible = this.chkActive.Checked;
        }

        private void dfrmPeripheralControlBoard_Load(object sender, EventArgs e)
        {
            this.txtf_ControllerSN.Text = this.ControllerSN.ToString();
            this.txtf_ControllerNO.Text = this.ControllerNO.ToString();
            this.chkActiveDefault = this.chkActive.Text;
            int controllerID = 0;
            string cmdText = " SELECT b.f_ControllerID, b.f_PeripheralControl ";
            cmdText = cmdText + " FROM t_b_Controller b  WHERE  b.[f_ControllerNO] = " + this.ControllerNO.ToString() + " AND  b.f_Enabled >0 ";
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
                            controllerID = (int) reader[0];
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
                        controllerID = (int) reader2[0];
                    }
                    reader2.Close();
                }
            }
        Label_014D:;
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
            this.ext_timeoutSet[0] = decimal.Parse(strArray[num2++]);
            this.ext_timeoutSet[1] = decimal.Parse(strArray[num2++]);
            this.ext_timeoutSet[2] = decimal.Parse(strArray[num2++]);
            this.ext_timeoutSet[3] = decimal.Parse(strArray[num2++]);
            this.ext_active[0] = int.Parse(strArray[num2++]);
            this.ext_active[1] = int.Parse(strArray[num2++]);
            this.ext_active[2] = int.Parse(strArray[num2++]);
            this.ext_active[3] = int.Parse(strArray[num2++]);
            using (icController controller = new icController())
            {
                controller.GetInfoFromDBByControllerID(controllerID);
                switch (wgMjController.GetControllerType(this.ControllerSN))
                {
                    case 1:
                        this.radioButton10.Text = controller.GetDoorName(1);
                        this.radioButton11.Visible = false;
                        this.radioButton12.Visible = false;
                        this.radioButton13.Visible = false;
                        this.radioButton25.Location = this.radioButton11.Location;
                        goto Label_054E;

                    case 2:
                        this.radioButton10.Text = controller.GetDoorName(1);
                        this.radioButton11.Text = controller.GetDoorName(2);
                        this.radioButton12.Visible = false;
                        this.radioButton13.Visible = false;
                        this.radioButton25.Location = this.radioButton12.Location;
                        goto Label_054E;
                }
                this.radioButton10.Text = controller.GetDoorName(1);
                this.radioButton11.Text = controller.GetDoorName(2);
                this.radioButton12.Text = controller.GetDoorName(3);
                this.radioButton13.Text = controller.GetDoorName(4);
            }
        Label_054E:
            this.tabControl1.SelectedTab = this.tabPage1;
            this.chkActive.Text = this.chkActiveDefault + " " + this.tabControl1.SelectedTab.Text;
            this.updateGrpExt(this.tabControl1.SelectedIndex);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
            this.grpEvent.Visible = !this.radioButton25.Checked;
        }

        private void saveParmExt()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.ext_active[i] == 0)
                {
                    this.ext_doorSet[i] = 0;
                    this.ext_controlSet[i] = 0;
                    this.ext_warnSignalEnabledSet[i] = 0;
                    this.ext_warnSignalEnabled2Set[i] = 0;
                    this.ext_timeoutSet[i] = 0M;
                }
            }
            string str = "";
            str = ((((((((((((((((((((((((((str + this.ext_AlarmControlMode.ToString()) + "," + this.ext_SetAlarmOnDelay.ToString()) + "," + this.ext_SetAlarmOffDelay.ToString()) + "," + this.ext_doorSet[0].ToString()) + "," + this.ext_doorSet[1].ToString()) + "," + this.ext_doorSet[2].ToString()) + "," + this.ext_doorSet[3].ToString()) + "," + this.ext_controlSet[0].ToString()) + "," + this.ext_controlSet[1].ToString()) + "," + this.ext_controlSet[2].ToString()) + "," + this.ext_controlSet[3].ToString()) + "," + this.ext_warnSignalEnabledSet[0].ToString()) + "," + this.ext_warnSignalEnabledSet[1].ToString()) + "," + this.ext_warnSignalEnabledSet[2].ToString()) + "," + this.ext_warnSignalEnabledSet[3].ToString()) + "," + this.ext_warnSignalEnabled2Set[0].ToString()) + "," + this.ext_warnSignalEnabled2Set[1].ToString()) + "," + this.ext_warnSignalEnabled2Set[2].ToString()) + "," + this.ext_warnSignalEnabled2Set[3].ToString()) + "," + this.ext_timeoutSet[0].ToString()) + "," + this.ext_timeoutSet[1].ToString()) + "," + this.ext_timeoutSet[2].ToString()) + "," + this.ext_timeoutSet[3].ToString()) + "," + this.ext_active[0].ToString()) + "," + this.ext_active[1].ToString()) + "," + this.ext_active[2].ToString()) + "," + this.ext_active[3].ToString();
            wgAppConfig.runUpdateSql((" UPDATE t_b_Controller SET f_PeripheralControl =" + wgTools.PrepareStr(str)) + "   WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            for (int j = 0; j < 4; j++)
            {
                if ((this.ext_warnSignalEnabledSet[j] & 4) > 0)
                {
                    num2 = 1;
                }
                if ((this.ext_warnSignalEnabledSet[j] & 2) > 0)
                {
                    num3 = 1;
                }
                if ((this.ext_warnSignalEnabledSet[j] & 1) > 0)
                {
                    num4 = 1;
                }
                if ((this.ext_warnSignalEnabledSet[j] & 0x10) > 0)
                {
                    num5 = 1;
                }
            }
            if (num2 == 1)
            {
                wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_DoorInvalidOpen =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
            }
            if (num3 == 1)
            {
                wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_DoorOpenTooLong =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
            }
            if (num4 == 1)
            {
                wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_ForceWarn =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
            }
            if (num5 == 1)
            {
                wgAppConfig.runUpdateSql(" UPDATE t_b_Controller SET f_InvalidCardWarn =1  WHERE  [f_ControllerNO] = " + this.ControllerNO.ToString());
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lastTabIndex != this.tabControl1.SelectedIndex)
            {
                this.chkActive.Text = this.chkActiveDefault + " " + this.tabControl1.SelectedTab.Text;
                this.updateParamExt(this.lastTabIndex);
                this.updateGrpExt(this.tabControl1.SelectedIndex);
            }
        }

        private void updateGrpExt(int doorNum)
        {
            this.lastTabIndex = doorNum;
            if (this.ext_active[doorNum] <= 0)
            {
                this.chkActive.Checked = false;
                this.grpSet.Visible = false;
                this.radioButton10.Checked = true;
                this.checkBox84.Checked = false;
                this.checkBox85.Checked = false;
                this.checkBox86.Checked = false;
                this.checkBox87.Checked = false;
                this.checkBox88.Checked = false;
                this.checkBox89.Checked = false;
                this.checkBox90.Checked = false;
                this.nudDelay.Value = 10M;
            }
            else
            {
                this.chkActive.Checked = true;
                this.grpSet.Visible = true;
                switch (this.ext_doorSet[doorNum])
                {
                    case 0:
                        this.radioButton10.Checked = true;
                        break;

                    case 1:
                        this.radioButton11.Checked = true;
                        break;

                    case 2:
                        this.radioButton12.Checked = true;
                        break;

                    case 3:
                        this.radioButton13.Checked = true;
                        break;

                    case 0x10:
                        this.radioButton25.Checked = true;
                        break;
                }
                if (!this.radioButton25.Checked)
                {
                    this.grpEvent.Visible = true;
                    int num2 = this.ext_warnSignalEnabledSet[doorNum];
                    this.checkBox84.Checked = (num2 & 1) > 0;
                    this.checkBox85.Checked = (num2 & 2) > 0;
                    this.checkBox86.Checked = (num2 & 4) > 0;
                    this.checkBox87.Checked = (num2 & 8) > 0;
                    this.checkBox88.Checked = (num2 & 0x10) > 0;
                    this.checkBox89.Checked = (num2 & 0x20) > 0;
                    this.checkBox90.Checked = (num2 & 0x40) > 0;
                }
                else
                {
                    this.grpEvent.Visible = false;
                }
                this.nudDelay.Value = this.ext_timeoutSet[doorNum];
            }
        }

        private void updateParamExt(int doorNum)
        {
            if (!this.chkActive.Checked)
            {
                this.ext_active[doorNum] = 0;
            }
            else
            {
                this.ext_active[doorNum] = 1;
                int num = 0;
                if (this.radioButton10.Checked)
                {
                    num = 0;
                }
                if (this.radioButton11.Checked)
                {
                    num = 1;
                }
                if (this.radioButton12.Checked)
                {
                    num = 2;
                }
                if (this.radioButton13.Checked)
                {
                    num = 3;
                }
                if (this.radioButton25.Checked)
                {
                    num = 0x10;
                }
                this.ext_doorSet[doorNum] = num;
                if (this.ext_controlSet[doorNum] == 0)
                {
                    this.ext_controlSet[doorNum] = 1;
                }
                if (!this.radioButton25.Checked)
                {
                    int num2 = 0;
                    if (this.checkBox84.Checked)
                    {
                        num2 |= 1;
                    }
                    if (this.checkBox85.Checked)
                    {
                        num2 |= 2;
                    }
                    if (this.checkBox86.Checked)
                    {
                        num2 |= 4;
                    }
                    if (this.checkBox87.Checked)
                    {
                        num2 |= 8;
                    }
                    if (this.checkBox88.Checked)
                    {
                        num2 |= 0x10;
                    }
                    if (this.checkBox89.Checked)
                    {
                        num2 |= 0x20;
                    }
                    if (this.checkBox90.Checked)
                    {
                        num2 |= 0x40;
                    }
                    this.ext_warnSignalEnabledSet[doorNum] = num2;
                }
                this.ext_timeoutSet[doorNum] = this.nudDelay.Value;
            }
        }
    }
}

