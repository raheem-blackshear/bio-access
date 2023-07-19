namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ExtendFunc.Elevator;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmExtendedFunctions : frmBioAccess
    {
        private int OneToMoreSelect;

        public dfrmExtendedFunctions()
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
            wgAppConfig.setSystemParamValueBool(0x65, this.chkRecordButtonEvent.Checked);
            wgAppConfig.setSystemParamValueBool(0x66, this.chkRecordDoorStatusEvent.Checked);
            wgAppConfig.setSystemParamValueBool(0x67, this.chkActiveLogQuery.Checked);
            wgAppConfig.setSystemParamValueBool(0x6f, this.chkActivateDontDisplayAccessControl.Checked);
            wgAppConfig.setSystemParamValueBool(0x70, this.chkActivateDontDisplayAttendance.Checked);
            wgAppConfig.setSystemParamValueBool(0x71, this.chkActivateOtherShiftSchedule.Checked);
            wgAppConfig.setSystemParamValueBool(0x72, this.chkActivateMaps.Checked);
            wgAppConfig.setSystemParamValueBool(0x79, this.chkActivateTimeProfile.Checked);
            wgAppConfig.setSystemParamValueBool(0x7a, this.chkActivateRemoteOpenDoor.Checked);
            wgAppConfig.setSystemParamValueBool(0x7b, this.chkActivateAccessKeypad.Checked);
            wgAppConfig.setSystemParamValueBool(0x7c, this.chkActivatePeripheralControl.Checked);
            wgAppConfig.setSystemParamValueBool(0x94, this.chkActivateOperatorManagement.Checked);
            wgAppConfig.setSystemParamValueBool(0x83, this.chkActivateControllerTaskList.Checked);
            wgAppConfig.setSystemParamValueBool(0x84, this.chkActivateAntiPassBack.Checked);
            wgAppConfig.setSystemParamValueBool(0x85, this.chkActivateInterLock.Checked);
            wgAppConfig.setSystemParamValueBool(0x86, this.chkActivateMultiCardAccess.Checked);
            wgAppConfig.setSystemParamValueBool(0x87, this.chkActivateFirstCardOpen.Checked);
            wgAppConfig.setSystemParamValueBool(0x89, this.chkActivatePCCheckAccess.Checked);
            wgAppConfig.setSystemParamValueBool(0x88, this.chkActivateTimeSegLimittedAccess.Checked);
            wgAppConfig.setSystemParamValueBool(0x92, this.chkActivateDoorAsSwitch.Checked);
            wgAppConfig.setSystemParamValueBool(0x8d, this.chkActivateWarnForceWithCard.Checked);
            wgAppConfig.setSystemParamValueBool(0x8e, this.chkActivateDontAutoLoadPrivileges.Checked);
            wgAppConfig.setSystemParamValueBool(0x8f, this.chkActivateDontAutoLoadSwipeRecords.Checked);
            wgAppConfig.setSystemParamValueBool(0x98, this.chkActivateDontDisplayQuickStart.Checked);
            if (!this.chkActivateElevator.Checked)
            {
                wgAppConfig.setSystemParamValueBool(0x90, false);
            }
            else
            {
                if (this.OneToMoreSelect == 0)
                {
                    this.OneToMoreSelect = 1;
                }
                wgAppConfig.setSystemParamValue(0x90, this.OneToMoreSelect.ToString());
            }
            wgAppConfig.setSystemParamValueBool(0x95, this.chkActivateMeeting.Checked);
            wgAppConfig.setSystemParamValueBool(150, this.chkActivateMeal.Checked);
            wgAppConfig.setSystemParamValueBool(0x97, this.chkActivatePatrol.Checked);
            base.DialogResult = DialogResult.Cancel;
            if (XMessageBox.Show(this, CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                base.DialogResult = DialogResult.OK;
            }
            base.Close();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            using (dfrmOneToMoreSetup setup = new dfrmOneToMoreSetup())
            {
                setup.radioButton0.Checked = true;
                setup.radioButton1.Checked = (this.OneToMoreSelect & 0xff) == 2;
                setup.radioButton2.Checked = (this.OneToMoreSelect & 0xff) == 3;
                if (this.OneToMoreSelect > 3)
                {
                    try
                    {
                        setup.numericUpDown21.Value = ((this.OneToMoreSelect >> 8) & 0xff) / 10M;
                        setup.numericUpDown20.Value = ((this.OneToMoreSelect >> 0x10) & 0xff) / 10M;
                        setup.Size = new Size(0x22a, 0x103);
                    }
                    catch (Exception)
                    {
                    }
                }
                if (setup.ShowDialog() == DialogResult.OK)
                {
                    this.OneToMoreSelect = 1;
                    if (setup.radioButton1.Checked)
                    {
                        this.OneToMoreSelect = 2;
                    }
                    if (setup.radioButton2.Checked)
                    {
                        this.OneToMoreSelect = 3;
                    }
                    if ((setup.numericUpDown21.Value != 0.4M) || (setup.numericUpDown20.Value != 5M))
                    {
                        this.OneToMoreSelect += ((int) (setup.numericUpDown21.Value * 10M)) << 8;
                        this.OneToMoreSelect += ((int) (setup.numericUpDown20.Value * 10M)) << 0x10;
                    }
                }
            }
        }

        private void chkActivateElevator_CheckedChanged(object sender, EventArgs e)
        {
            //this.btnSetup.Visible = this.chkActivateElevator.Checked;
        }

        private void dfrmExtendedFunctions_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmExtendedFunctions_Load(object sender, EventArgs e)
        {
            this.chkRecordButtonEvent.Checked = wgAppConfig.getParamValBoolByNO(0x65);
            this.chkRecordDoorStatusEvent.Checked = wgAppConfig.getParamValBoolByNO(0x66);
            this.chkActiveLogQuery.Checked = wgAppConfig.getParamValBoolByNO(0x67);
            this.chkActivateDontDisplayAccessControl.Checked = wgAppConfig.getParamValBoolByNO(0x6f);
            this.chkActivateDontDisplayAttendance.Checked = wgAppConfig.getParamValBoolByNO(0x70);
            this.chkActivateOtherShiftSchedule.Checked = wgAppConfig.getParamValBoolByNO(0x71);
            this.chkActivateMaps.Checked = wgAppConfig.getParamValBoolByNO(0x72);
            this.chkActivateTimeProfile.Checked = wgAppConfig.getParamValBoolByNO(0x79);
            this.chkActivateRemoteOpenDoor.Checked = wgAppConfig.getParamValBoolByNO(0x7a);
            this.chkActivateAccessKeypad.Checked = wgAppConfig.getParamValBoolByNO(0x7b);
            this.chkActivatePeripheralControl.Checked = wgAppConfig.getParamValBoolByNO(0x7c);
            this.chkActivateOperatorManagement.Checked = wgAppConfig.getParamValBoolByNO(0x94);
            this.chkActivateControllerTaskList.Checked = wgAppConfig.getParamValBoolByNO(0x83);
            this.chkActivateAntiPassBack.Checked = wgAppConfig.getParamValBoolByNO(0x84);
            this.chkActivateInterLock.Checked = wgAppConfig.getParamValBoolByNO(0x85);
            this.chkActivateMultiCardAccess.Checked = wgAppConfig.getParamValBoolByNO(0x86);
            this.chkActivateFirstCardOpen.Checked = wgAppConfig.getParamValBoolByNO(0x87);
            this.chkActivatePCCheckAccess.Checked = wgAppConfig.getParamValBoolByNO(0x89);
            this.chkActivateTimeSegLimittedAccess.Checked = wgAppConfig.getParamValBoolByNO(0x88);
            this.chkActivateDoorAsSwitch.Checked = wgAppConfig.getParamValBoolByNO(0x92);
            this.chkActivateWarnForceWithCard.Checked = wgAppConfig.getParamValBoolByNO(0x8d);
            this.chkActivateDontAutoLoadPrivileges.Checked = wgAppConfig.getParamValBoolByNO(0x8e);
            this.chkActivateDontAutoLoadSwipeRecords.Checked = wgAppConfig.getParamValBoolByNO(0x8f);
            this.chkActivateElevator.Checked = wgAppConfig.getParamValBoolByNO(0x90);
            this.OneToMoreSelect = int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90));
            this.chkActivateMeeting.Checked = wgAppConfig.getParamValBoolByNO(0x95);
            this.chkActivateMeal.Checked = wgAppConfig.getParamValBoolByNO(150);
            this.chkActivatePatrol.Checked = wgAppConfig.getParamValBoolByNO(0x97);
            this.chkActivateDontDisplayQuickStart.Checked = wgAppConfig.getParamValBoolByNO(0x98);
            if (!this.chkActivateTimeSegLimittedAccess.Checked)
            {
                this.chkActivateTimeSegLimittedAccess.Visible = false;
            }
            if (!this.chkActivateDoorAsSwitch.Checked)
            {
                this.chkActivateDoorAsSwitch.Visible = false;
            }
        }

        private void funcCtrlShiftQ()
        {
            this.chkActivateTimeSegLimittedAccess.Visible = true;
            this.chkActivateDoorAsSwitch.Visible = true;
        }
    }
}

