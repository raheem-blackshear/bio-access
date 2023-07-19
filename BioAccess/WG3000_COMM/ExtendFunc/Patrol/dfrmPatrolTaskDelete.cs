namespace WG3000_COMM.ExtendFunc.Patrol
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPatrolTaskDelete : frmBioAccess
    {
        private ArrayList arrConsumerCMIndex = new ArrayList();
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private ArrayList arrSelectedShiftID = new ArrayList();
        private ArrayList arrShiftID = new ArrayList();
        private dfrmFind dfrmFind1;
        private DataSet dsConsumers;
        private DataTable dtConsumers;
        private DataView dvConsumers;

        public dfrmPatrolTaskDelete()
        {
            this.InitializeComponent();
        }

        private void _dataTableLoad()
        {
            string cmdText = "";
            this.dsConsumers = new DataSet("Users");
            cmdText = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM ([t_b_Consumer] INNER JOIN t_d_PatrolUsers ON (t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID) )  LEFT OUTER JOIN t_b_Group ON  ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID ) ";
            this.dsConsumers.Clear();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dsConsumers, "Consumers");
                        }
                    }
                    goto Label_00DA;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dsConsumers, "Consumers");
                    }
                }
            }
        Label_00DA:
            this.dtConsumers = this.dsConsumers.Tables["Consumers"];
            this.dvConsumers = new DataView(this.dtConsumers);
            this.dvConsumers.RowFilter = "";
            try
            {
                DataColumn[] columnArray = new DataColumn[2];
                columnArray[0] = this.dtConsumers.Columns["f_UserFullName"];
                this.dtConsumers.PrimaryKey = columnArray;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            this.dtConsumers.AcceptChanges();
            this.loadGroupData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
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
                using (comPatrol patrol = new comPatrol())
                {
                    int errno = 0;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        int num2;
                        int[] numArray;
                        if (this.cbof_ConsumerName.Text == CommonStr.strAll)
                        {
                            if (this.dvConsumers.Count <= 0)
                            {
                                XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            numArray = new int[(this.dvConsumers.Count - 1) + 1];
                            for (num2 = 0; num2 <= (this.dvConsumers.Count - 1); num2++)
                            {
                                numArray[num2] = (int) this.dvConsumers[num2]["f_ConsumerID"];
                            }
                        }
                        else
                        {
                            numArray = new int[] { (int) this.dvConsumers[this.cbof_ConsumerName.SelectedIndex - 1]["f_ConsumerID"] };
                        }
                        DateTime dateStart = this.dtpStartDate.Value;
                        DateTime dateEnd = this.dtpEndDate.Value;
                        if (errno == 0)
                        {
                            this.ProgressBar1.Maximum = numArray.Length;
                            for (num2 = 0; num2 <= (numArray.Length - 1); num2++)
                            {
                                this.ProgressBar1.Value = num2;
                                errno = patrol.shift_arrange_delete(numArray[num2], dateStart, dateEnd);
                                if (errno != 0)
                                {
                                    XMessageBox.Show(this, patrol.errDesc(errno), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            if (errno == 0)
                            {
                                this.ProgressBar1.Value = this.ProgressBar1.Maximum;
                                XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        {
                            XMessageBox.Show(this, patrol.errDesc(errno) + "\r\n\r\n" + patrol.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                    this.ProgressBar1.Value = 0;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            using (comPatrol_Acc acc = new comPatrol_Acc())
            {
                int errno = 0;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    int num2;
                    int[] numArray;
                    if (this.cbof_ConsumerName.Text == CommonStr.strAll)
                    {
                        if (this.dvConsumers.Count <= 0)
                        {
                            XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        numArray = new int[(this.dvConsumers.Count - 1) + 1];
                        for (num2 = 0; num2 <= (this.dvConsumers.Count - 1); num2++)
                        {
                            numArray[num2] = (int) this.dvConsumers[num2]["f_ConsumerID"];
                        }
                    }
                    else
                    {
                        numArray = new int[] { (int) this.dvConsumers[this.cbof_ConsumerName.SelectedIndex - 1]["f_ConsumerID"] };
                    }
                    DateTime dateStart = this.dtpStartDate.Value;
                    DateTime dateEnd = this.dtpEndDate.Value;
                    if (errno == 0)
                    {
                        this.ProgressBar1.Maximum = numArray.Length;
                        for (num2 = 0; num2 <= (numArray.Length - 1); num2++)
                        {
                            this.ProgressBar1.Value = num2;
                            errno = acc.shift_arrange_delete(numArray[num2], dateStart, dateEnd);
                            if (errno != 0)
                            {
                                XMessageBox.Show(this, acc.errDesc(errno), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                        }
                        if (errno == 0)
                        {
                            this.ProgressBar1.Value = this.ProgressBar1.Maximum;
                            XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        XMessageBox.Show(this, acc.errDesc(errno) + "\r\n\r\n" + acc.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                this.ProgressBar1.Value = 0;
                Cursor.Current = Cursors.Default;
            }
        }

        private void cbof_ConsumerName_Leave(object sender, EventArgs e)
        {
            this.checkUserValid(this.cbof_ConsumerName);
        }

        private void cbof_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((this.cbof_Group.SelectedIndex == 0) && (this.arrGroupID[0].ToString() == "0"))
                {
                    this.dvConsumers.RowFilter = "";
                }
                else
                {
                    this.dvConsumers.RowFilter = string.Format(" (f_GroupName = {0} ) OR (f_GroupName like {1})", wgTools.PrepareStr(this.cbof_Group.Text), wgTools.PrepareStr(string.Format(@"{0}\%", this.cbof_Group.Text)));
                }
                this.cbof_ConsumerName.Items.Clear();
                this.cbof_ConsumerName.Items.Add(CommonStr.strAll);
                this.arrConsumerCMIndex.Add("");
                for (int i = 0; i <= (this.dvConsumers.Count - 1); i++)
                {
                    this.cbof_ConsumerName.Items.Add(this.dvConsumers[i]["f_UserFullName"]);
                    this.arrConsumerCMIndex.Add(i);
                }
                if (this.cbof_ConsumerName.Items.Count > 0)
                {
                    this.cbof_ConsumerName.SelectedIndex = 0;
                }
                int count = this.dvConsumers.Count;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        public bool checkUserValid(ComboBox cbo)
        {
            try
            {
                string str = cbo.Text.ToUpper();
                int selectedIndex = cbo.SelectedIndex;
                if ((selectedIndex < 0) || (cbo.Text != cbo.Items[selectedIndex].ToString()))
                {
                    selectedIndex = -1;
                    ComboBox box = cbo;
                    for (int i = 0; i < box.Items.Count; i++)
                    {
                        object valA = box.Items[i];
                        if (Strings.UCase(wgTools.SetObjToStr(valA)).IndexOf(str) >= 0)
                        {
                            box.SelectedItem = box.Items[i];
                            box.SelectedIndex = i;
                            selectedIndex = i;
                            break;
                        }
                    }
                    if (selectedIndex >= 0)
                    {
                        cbo.SelectedIndex = selectedIndex;
                    }
                    else
                    {
                        XMessageBox.Show(this, CommonStr.strUserNonexisted, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            return false;
        }

        private void dfrmAutoShift_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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

        private void dfrmAutoShift_Load(object sender, EventArgs e)
        {
            try
            {
                this.Label3.Text = wgAppConfig.ReplaceFloorRomm(this.Label3.Text);
                base.KeyPreview = true;
                this._dataTableLoad();
                this.dtpStartDate.Value = DateTime.Now.Date;
                this.dtpEndDate.Value = DateTime.Now.Date;
                if (this.cbof_Group.Items.Count > 0)
                {
                    this.cbof_Group.SelectedIndex = 0;
                }
                if (this.cbof_ConsumerName.Items.Count > 0)
                {
                    this.cbof_ConsumerName.SelectedIndex = 0;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
        }

        private void dfrmShiftDelete_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
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

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblEndWeekday.Text = CommonStr.strWeekday + wgAppConfig.weekdayToChsName((int) this.dtpEndDate.Value.DayOfWeek);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.dtpEndDate.MinDate = this.dtpStartDate.Value;
                this.lblStartWeekday.Text = CommonStr.strWeekday + wgAppConfig.weekdayToChsName((int) this.dtpStartDate.Value.DayOfWeek);
                this.lblShiftWeekday_update((int) this.dtpStartDate.Value.DayOfWeek);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void lblShiftWeekday_update(int weekdayStart)
        {
            try
            {
                int num = weekdayStart;
                if (num >= 7)
                {
                    num = 0;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void loadGroupData()
        {
            new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
            for (int i = 0; i < this.arrGroupID.Count; i++)
            {
                if ((i == 0) && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
                {
                    this.cbof_Group.Items.Add(CommonStr.strAll);
                }
                else
                {
                    this.cbof_Group.Items.Add(this.arrGroupName[i].ToString());
                }
            }
            if (this.cbof_Group.Items.Count > 0)
            {
                this.cbof_Group.SelectedIndex = 0;
            }
        }
    }
}

