namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmUserBatchUpdate : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNameWithSpace = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private bool bInsertNullDepartment;
        public string strSqlSelected = "";

        public dfrmUserBatchUpdate()
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
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
            }
            else
            {
                string str;
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                string str2 = "  ";
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    num = 0;
                    num3 = 0;
                }
                else if (this.bInsertNullDepartment && (this.cbof_GroupID.Text == wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartmentIsEmpty)))
                {
                    num = 0;
                    num3 = 0;
                    str2 = " WHERE f_GroupID = 0   ";
                }
                else
                {
                    if (this.bInsertNullDepartment)
                    {
                        num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex - 1];
                        num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex - 1];
                        num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                    }
                    else
                    {
                        num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                        num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
                        num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                    }
                    if (!this.chkIncludeAllBranch.Checked)
                    {
                        num3 = num;
                    }
                }
                if (num > 0)
                {
                    if (num >= num3)
                    {
                        str2 = " FROM   t_b_Consumer,t_b_Group WHERE  t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
                        str2 = str2 + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", num2);
                    }
                    else
                    {
                        str2 = " FROM   t_b_Consumer,t_b_Group   WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
                        str2 = str2 + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", num3);
                    }
                }
                if (!string.IsNullOrEmpty(this.strSqlSelected))
                {
                    str2 = string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
                }
                if (this.chk1.Checked)
                {
                    str = "UPDATE t_b_Consumer   SET ";
                    wgAppConfig.runUpdateSql((str + "  t_b_Consumer.[f_DoorEnabled]=" + (this.opt1a.Checked ? "1" : "0")) + str2);
                }
                if (this.chk3.Checked)
                {
                    str = "UPDATE t_b_Consumer SET ";
                    wgAppConfig.runUpdateSql((str + "  t_b_Consumer.[f_ShiftEnabled]=" + (this.opt3b.Checked ? "1" : "0")) + str2);
                }
                if (this.chk2.Checked)
                {
                    str = "UPDATE t_b_Consumer SET ";
                    wgAppConfig.runUpdateSql((str + "  t_b_Consumer.[f_AttendEnabled]=" + (this.opt2a.Checked ? "1" : "0")) + str2);
                    if (!this.opt2a.Checked)
                    {
                        str = "UPDATE t_b_Consumer SET ";
                        wgAppConfig.runUpdateSql(str + "  t_b_Consumer.[f_ShiftEnabled]=0" + str2);
                    }
                }
                if (this.chk5.Checked)
                {
                    str = "UPDATE t_b_Consumer SET ";
                    wgAppConfig.runUpdateSql(((str + " t_b_Consumer.[f_BeginYMD]=" + wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd")) + "  ,t_b_Consumer.[f_EndYMD]=" + wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd")) + str2);
                }
                if (this.chk6.Checked)
                {
                    str = "UPDATE t_b_Consumer SET ";
                    wgAppConfig.runUpdateSql((str + " t_b_Consumer.[f_PIN] = " + ((this.txtf_PIN.Text == "") ? "0" : this.txtf_PIN.Text.Trim())) + str2);
                }
                if (this.chk4.Checked)
                {
                    str = "UPDATE t_b_Consumer SET ";
                    if (this.cbof_GroupNew.SelectedIndex == -1)
                    {
                        str = str + "  t_b_Consumer.[f_GroupID]=0";
                    }
                    else
                    {
                        str = str + "  t_b_Consumer.[f_GroupID]=" + wgTools.PrepareStr(this.arrGroupID[this.cbof_GroupNew.SelectedIndex]);
                    }
                    wgAppConfig.runUpdateSql(str + str2);
                }
                base.DialogResult = DialogResult.OK;
                icConsumerShare.setUpdateLog();
                base.Close();
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            string str;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            string str3 = "";
            string str2 = "  ";
            if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
            {
                num = 0;
                num3 = 0;
            }
            else if (this.bInsertNullDepartment && (this.cbof_GroupID.Text == wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartmentIsEmpty)))
            {
                num = 0;
                num3 = 0;
                str2 = " WHERE f_GroupID = 0   ";
            }
            else
            {
                if (this.bInsertNullDepartment)
                {
                    num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex - 1];
                    num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex - 1];
                    num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                }
                else
                {
                    num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
                    num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                }
                if (!this.chkIncludeAllBranch.Checked)
                {
                    num3 = num;
                }
            }
            if (num > 0)
            {
                if (num >= num3)
                {
                    str3 = "    INNER JOIN t_b_Group ON (  t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
                    str3 = str3 + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ) ", num2);
                }
                else
                {
                    str3 = "    INNER JOIN t_b_Group ON  ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ";
                    str3 = str3 + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", num) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ) ", num3);
                }
            }
            if (!string.IsNullOrEmpty(this.strSqlSelected))
            {
                str2 = string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
                str3 = " ";
            }
            if (this.chk1.Checked)
            {
                str = "UPDATE t_b_Consumer  ";
                wgAppConfig.runUpdateSql(((str + str3) + "  SET  t_b_Consumer.[f_DoorEnabled]=" + (this.opt1a.Checked ? "1" : "0")) + str2);
            }
            if (this.chk3.Checked)
            {
                str = "UPDATE t_b_Consumer ";
                wgAppConfig.runUpdateSql(((str + str3) + " SET  t_b_Consumer.[f_ShiftEnabled]=" + (this.opt3b.Checked ? "1" : "0")) + str2);
            }
            if (this.chk2.Checked)
            {
                str = "UPDATE t_b_Consumer ";
                wgAppConfig.runUpdateSql(((str + str3) + " SET  t_b_Consumer.[f_AttendEnabled]=" + (this.opt2a.Checked ? "1" : "0")) + str2);
                if (!this.opt2a.Checked)
                {
                    str = "UPDATE t_b_Consumer  ";
                    wgAppConfig.runUpdateSql((str + str3) + " SET t_b_Consumer.[f_ShiftEnabled]=0" + str2);
                }
            }
            if (this.chk5.Checked)
            {
                str = "UPDATE t_b_Consumer ";
                wgAppConfig.runUpdateSql((((str + str3) + " SET t_b_Consumer.[f_BeginYMD]=" + wgTools.PrepareStr(this.dtpBegin.Value, true, "yyyy-MM-dd")) + "  ,t_b_Consumer.[f_EndYMD]=" + wgTools.PrepareStr(this.dtpEnd.Value, true, "yyyy-MM-dd")) + str2);
            }
            if (this.chk6.Checked)
            {
                str = "UPDATE t_b_Consumer ";
                wgAppConfig.runUpdateSql(((str + str3) + " SET  t_b_Consumer.[f_PIN] = " + ((this.txtf_PIN.Text == "") ? "0" : this.txtf_PIN.Text.Trim())) + str2);
            }
            if (this.chk4.Checked)
            {
                str = "UPDATE t_b_Consumer ";
                str = str + str3;
                if (this.cbof_GroupNew.SelectedIndex == -1)
                {
                    str = str + " SET  t_b_Consumer.[f_GroupID]=0";
                }
                else
                {
                    str = str + " SET  t_b_Consumer.[f_GroupID]=" + wgTools.PrepareStr(this.arrGroupID[this.cbof_GroupNew.SelectedIndex]);
                }
                wgAppConfig.runUpdateSql(str + str2);
            }
            base.DialogResult = DialogResult.OK;
            icConsumerShare.setUpdateLog();
            base.Close();
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            this.GroupBox1.Enabled = this.chk1.Checked;
        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            this.GroupBox2.Enabled = this.chk2.Checked;
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {
            this.GroupBox3.Enabled = this.chk3.Checked;
        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {
            this.cbof_GroupNew.Enabled = this.chk4.Checked;
        }

        private void chk5_CheckedChanged(object sender, EventArgs e)
        {
            this.GroupBox4.Enabled = this.chk5.Checked;
        }

        private void dfrmUserBatchUpdate_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmUserBatchUpdate_Load(object sender, EventArgs e)
        {
            this.txtf_PIN.Mask = "999999";
            this.txtf_PIN.Text = "";
            this.Label3.Text = wgAppConfig.ReplaceFloorRomm(this.Label3.Text);
            this.chk4.Text = wgAppConfig.ReplaceFloorRomm(this.chk4.Text);
            this.chkIncludeAllBranch.Text = wgAppConfig.ReplaceFloorRomm(this.chkIncludeAllBranch.Text);
            try
            {
                new icGroup().getGroup(ref this.arrGroupNameWithSpace, ref this.arrGroupID, ref this.arrGroupNO);
                int count = this.arrGroupID.Count;
                for (count = 0; count < this.arrGroupID.Count; count++)
                {
                    if ((count == 0) && string.IsNullOrEmpty(this.arrGroupNameWithSpace[count].ToString()))
                    {
                        this.arrGroupName.Add(CommonStr.strAll);
                    }
                    else
                    {
                        this.arrGroupName.Add(this.arrGroupNameWithSpace[count].ToString());
                    }
                    this.cbof_GroupID.Items.Add(this.arrGroupName[count].ToString());
                    this.cbof_GroupNew.Items.Add(this.arrGroupNameWithSpace[count].ToString());
                }
                if (((int) this.arrGroupID[0]) == 0)
                {
                    this.cbof_GroupID.Items.Insert(1, wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartmentIsEmpty));
                    this.bInsertNullDepartment = true;
                }
                if (this.cbof_GroupID.Items.Count > 0)
                {
                    this.cbof_GroupID.SelectedIndex = 0;
                }
                if (this.cbof_GroupNew.Items.Count > 0)
                {
                    this.cbof_GroupNew.SelectedIndex = 0;
                }
                if (!string.IsNullOrEmpty(this.strSqlSelected))
                {
                    this.cbof_GroupID.Visible = false;
                    this.Label3.Visible = false;
                    this.chkIncludeAllBranch.Visible = false;
                }
                if (wgAppConfig.getParamValBoolByNO(0x71))
                {
                    this.chk3.Visible = true;
                    this.GroupBox3.Visible = true;
                }
                else
                {
                    this.chk3.Visible = false;
                    this.GroupBox3.Visible = false;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
            wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMD);
        }

        private void funcCtrlShiftQ()
        {
            this.chk6.Visible = true;
            this.txtf_PIN.Visible = true;
        }
    }
}

