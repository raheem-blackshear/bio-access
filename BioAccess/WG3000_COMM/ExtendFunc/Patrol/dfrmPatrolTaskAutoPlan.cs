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

    public partial class dfrmPatrolTaskAutoPlan : frmBioAccess
    {
        private ArrayList arrConsumerCMIndex = new ArrayList();
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private ArrayList arrSelectedShiftID = new ArrayList();
        private ArrayList arrShiftID = new ArrayList();
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataAdapter daConsumers;
        private dfrmFind dfrmFind1;
        private DataSet dsConsumers;
        private DataTable dtConsumers;
        private DataTable dtOptionalShift;

        public dfrmPatrolTaskAutoPlan()
        {
            this.InitializeComponent();
        }

        private void _dataTableLoad()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this._dataTableLoad_Acc();
            }
            else
            {
                this.con = new SqlConnection(wgAppConfig.dbConString);
                string cmdText = "";
                this.dsConsumers = new DataSet("Users");
                cmdText = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM ([t_b_Consumer] INNER JOIN t_d_PatrolUsers ON (t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID) )  LEFT OUTER JOIN t_b_Group ON ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID )  ";
                this.cmd = new SqlCommand(cmdText, this.con);
                this.daConsumers = new SqlDataAdapter(this.cmd);
                try
                {
                    this.dsConsumers.Clear();
                    this.daConsumers.Fill(this.dsConsumers, "Consumers");
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
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
                this.loadGroupData();
                try
                {
                    if (wgAppConfig.IsAccessDB)
                    {
                        this.cmd = new SqlCommand("SELECT [f_RouteID] & '-' & [f_RouteName] as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", this.con);
                    }
                    else
                    {
                        this.cmd = new SqlCommand("SELECT CONVERT(nvarchar(50),[f_RouteID]) + case when [f_RouteName] IS NULL Then '' ELSE   '-' + [f_RouteName] end  as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", this.con);
                    }
                    try
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(this.cmd))
                        {
                            adapter.Fill(this.dsConsumers, "OptionalShift");
                        }
                        this.dtOptionalShift = this.dsConsumers.Tables["OptionalShift"];
                        this.arrShiftID.Clear();
                        this.lstOptionalShifts.Items.Clear();
                        if (this.dtOptionalShift.Rows.Count > 0)
                        {
                            this.arrShiftID.Add(0);
                            this.lstOptionalShifts.Items.Add("0*-" + CommonStr.strRest);
                            for (int i = 0; i <= (this.dtOptionalShift.Rows.Count - 1); i++)
                            {
                                this.lstOptionalShifts.Items.Add(this.dtOptionalShift.Rows[i][0]);
                                this.arrShiftID.Add(this.dtOptionalShift.Rows[i][1]);
                            }
                        }
                    }
                    catch (Exception exception3)
                    {
                        wgTools.WgDebugWrite(exception3.ToString(), new object[0]);
                    }
                    finally
                    {
                        this.con.Close();
                    }
                }
                catch (Exception exception4)
                {
                    wgTools.WgDebugWrite(exception4.ToString(), new object[0]);
                }
            }
        }

        private void _dataTableLoad_Acc()
        {
            OleDbConnection connection = null;
            OleDbCommand selectCommand = null;
            connection = new OleDbConnection(wgAppConfig.dbConString);
            string cmdText = "";
            this.dsConsumers = new DataSet("Users");
            cmdText = " SELECT t_b_Group.f_GroupName,t_b_Consumer.f_ConsumerID, t_b_Consumer.f_ConsumerName, LTRIM(([f_ConsumerNo]) +'- '+ [f_ConsumerName]) as [f_UserFullName]  FROM ([t_b_Consumer] INNER JOIN t_d_PatrolUsers ON (t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID) )  LEFT OUTER JOIN t_b_Group ON  ( t_b_Group.f_GroupID = t_b_Consumer.f_GroupID ) ";
            selectCommand = new OleDbCommand(cmdText, connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            try
            {
                this.dsConsumers.Clear();
                adapter.Fill(this.dsConsumers, "Consumers");
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
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
            this.loadGroupData();
            try
            {
                if (wgAppConfig.IsAccessDB)
                {
                    selectCommand = new OleDbCommand("SELECT [f_RouteID] & '-' & [f_RouteName] as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", connection);
                }
                else
                {
                    selectCommand = new OleDbCommand("SELECT CONVERT(nvarchar(50),[f_RouteID]) + case when [f_RouteName] IS NULL Then '' ELSE   '-' + [f_RouteName] end  as f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC", connection);
                }
                try
                {
                    using (OleDbDataAdapter adapter2 = new OleDbDataAdapter(selectCommand))
                    {
                        adapter2.Fill(this.dsConsumers, "OptionalShift");
                    }
                    this.dtOptionalShift = this.dsConsumers.Tables["OptionalShift"];
                    this.arrShiftID.Clear();
                    this.lstOptionalShifts.Items.Clear();
                    if (this.dtOptionalShift.Rows.Count > 0)
                    {
                        this.arrShiftID.Add(0);
                        this.lstOptionalShifts.Items.Add("0*-" + CommonStr.strRest);
                        for (int i = 0; i <= (this.dtOptionalShift.Rows.Count - 1); i++)
                        {
                            this.lstOptionalShifts.Items.Add(this.dtOptionalShift.Rows[i][0]);
                            this.arrShiftID.Add(this.dtOptionalShift.Rows[i][1]);
                        }
                    }
                }
                catch (Exception exception3)
                {
                    wgTools.WgDebugWrite(exception3.ToString(), new object[0]);
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception exception4)
            {
                wgTools.WgDebugWrite(exception4.ToString(), new object[0]);
            }
        }

        private void btnAddOne_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.lstOptionalShifts.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.lstSelectedShifts.Items.Add(this.lstOptionalShifts.Items[selectedIndex]);
                this.arrSelectedShiftID.Add(this.arrShiftID[selectedIndex]);
                if ((this.lstSelectedShifts.Items.Count == 0) || (this.dvConsumers.Count <= 0))
                {
                    this.btnOK.Enabled = false;
                }
                else
                {
                    this.btnOK.Enabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            this.lstSelectedShifts.Items.Clear();
            this.arrSelectedShiftID.Clear();
            if (this.lstSelectedShifts.Items.Count == 0)
            {
                this.btnOK.Enabled = false;
            }
            else
            {
                this.btnOK.Enabled = true;
            }
        }

        private void btnDeleteOne_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.lstSelectedShifts.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.lstSelectedShifts.Items.RemoveAt(selectedIndex);
                this.arrSelectedShiftID.RemoveAt(selectedIndex);
                if (this.lstSelectedShifts.Items.Count == 0)
                {
                    this.btnOK.Enabled = false;
                }
                else
                {
                    this.btnOK.Enabled = true;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (this.arrSelectedShiftID.Count > 0)
                {
                    int num;
                    int num2;
                    int[] numArray2;
                    int[] shiftRule = new int[(this.arrSelectedShiftID.Count - 1) + 1];
                    if (this.cbof_ConsumerName.Text == CommonStr.strAll)
                    {
                        if (this.dvConsumers.Count <= 0)
                        {
                            XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        numArray2 = new int[(this.dvConsumers.Count - 1) + 1];
                        for (num2 = 0; num2 <= (this.dvConsumers.Count - 1); num2++)
                        {
                            numArray2[num2] = (int) this.dvConsumers[num2]["f_ConsumerID"];
                        }
                    }
                    else
                    {
                        numArray2 = new int[] { (int) this.dvConsumers[this.cbof_ConsumerName.SelectedIndex - 1]["f_ConsumerID"] };
                    }
                    DateTime dateStart = this.dtpStartDate.Value;
                    DateTime dateEnd = this.dtpEndDate.Value;
                    for (num2 = 0; num2 <= (this.arrSelectedShiftID.Count - 1); num2++)
                    {
                        shiftRule[num2] = (int) this.arrSelectedShiftID[num2];
                    }
                    if (wgAppConfig.IsAccessDB)
                    {
                        using (comPatrol_Acc acc = new comPatrol_Acc())
                        {
                            num = 0;
                            if (num == 0)
                            {
                                this.ProgressBar1.Maximum = numArray2.Length;
                                for (num2 = 0; num2 <= (numArray2.Length - 1); num2++)
                                {
                                    this.ProgressBar1.Value = num2;
                                    num = acc.shift_arrangeByRule(numArray2[num2], dateStart, dateEnd, shiftRule.Length, shiftRule);
                                    if (num != 0)
                                    {
                                        XMessageBox.Show(this, acc.errDesc(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        break;
                                    }
                                }
                                if (num == 0)
                                {
                                    this.ProgressBar1.Value = this.ProgressBar1.Maximum;
                                    XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                            }
                            goto Label_02CE;
                        }
                    }
                    using (comPatrol patrol = new comPatrol())
                    {
                        num = 0;
                        if (num == 0)
                        {
                            this.ProgressBar1.Maximum = numArray2.Length;
                            for (num2 = 0; num2 <= (numArray2.Length - 1); num2++)
                            {
                                this.ProgressBar1.Value = num2;
                                num = patrol.shift_arrangeByRule(numArray2[num2], dateStart, dateEnd, shiftRule.Length, shiftRule);
                                if (num != 0)
                                {
                                    XMessageBox.Show(this, patrol.errDesc(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            if (num == 0)
                            {
                                this.ProgressBar1.Value = this.ProgressBar1.Maximum;
                                XMessageBox.Show(this, "OK!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        {
                            XMessageBox.Show(this, patrol.errDesc(num) + "\r\n\r\n" + patrol.errInfo, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        Label_02CE:
            this.ProgressBar1.Value = 0;
            Cursor.Current = Cursors.Default;
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
                    this.dvConsumers.RowFilter = " (f_GroupName = '" + this.cbof_Group.Text + "' ) OR (f_GroupName like '" + this.cbof_Group.Text + @"\%')";
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
                if (this.dvConsumers.Count <= 0)
                {
                    this.btnOK.Enabled = false;
                }
                else if (this.lstSelectedShifts.Items.Count > 0)
                {
                    this.btnOK.Enabled = true;
                }
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

        private void dfrmAutoShift_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
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
                this.btnOK.Enabled = false;
                if (this.lstOptionalShifts.Items.Count == 0)
                {
                    XMessageBox.Show(this, CommonStr.strNeedShift, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
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
                int weekDay = weekdayStart;
                if (weekDay >= 7)
                {
                    weekDay = 0;
                }
                this.lstShiftWeekday.Items.Clear();
                for (int i = 1; i <= 14; i++)
                {
                    this.lstShiftWeekday.Items.Add(wgAppConfig.weekdayToChsName(weekDay));
                    weekDay++;
                    if (weekDay >= 7)
                    {
                        weekDay = 0;
                    }
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

        private void lstOptionalShifts_DoubleClick(object sender, EventArgs e)
        {
            this.btnAddOne.PerformClick();
        }

        private void lstSelectedShifts_DoubleClick(object sender, EventArgs e)
        {
            this.btnDeleteOne.PerformClick();
        }
    }
}

