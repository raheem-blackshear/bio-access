namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmFirstCard : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private int controllerID;
        public int DoorID;
        private int doorNo;
        private DataTable dt;
        private static DataTable dtLastLoad;
        private DataTable dtUser1;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvSelected;
        private static string lastLoadUsers = "";
        private const int MoreCardGroupMaxLen = 9;
        public string retValue = "0";
        private string strGroupFilter = "";

        public dfrmFirstCard()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            e.Result = this.loadUserData4BackWork();
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
            }
            else if (e.Error != null)
            {
                wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
            }
            else
            {
                this.loadUserData4BackWorkComplete(e.Result as DataTable);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
            }
        }

        private void btnAddAllUsers_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("btnAddAllUsers_Click Start");
            this.dt = ((DataView) this.dgvUsers.DataSource).Table;
            this.dv1 = (DataView) this.dgvUsers.DataSource;
            this.dv2 = (DataView) this.dgvSelectedUsers.DataSource;
            this.dgvUsers.DataSource = null;
            this.dgvSelectedUsers.DataSource = null;
            if (this.strGroupFilter == "")
            {
                string rowFilter = this.dv1.RowFilter;
                string str2 = this.dv2.RowFilter;
                this.dv1.Dispose();
                this.dv2.Dispose();
                this.dv1 = null;
                this.dv2 = null;
                this.dt.BeginLoadData();
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 1;
                }
                this.dt.EndLoadData();
                this.dv1 = new DataView(this.dt);
                this.dv1.RowFilter = rowFilter;
                this.dv2 = new DataView(this.dt);
                this.dv2.RowFilter = str2;
            }
            else
            {
                this.dv = new DataView(this.dt);
                this.dv.RowFilter = this.strGroupFilter;
                for (int j = 0; j < this.dv.Count; j++)
                {
                    this.dv[j]["f_Selected"] = 1;
                }
            }
            this.dgvUsers.DataSource = this.dv1;
            this.dgvSelectedUsers.DataSource = this.dv2;
            wgTools.WriteLine("btnAddAllUsers_Click End");
            Cursor.Current = Cursors.Default;
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvUsers);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnDelAllUsers_Click(object sender, EventArgs e)
        {
            if (this.dgvSelectedUsers.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("btnDelAllUsers_Click Start");
                this.dt = ((DataView) this.dgvUsers.DataSource).Table;
                this.dv1 = (DataView) this.dgvUsers.DataSource;
                this.dv2 = (DataView) this.dgvSelectedUsers.DataSource;
                this.dgvUsers.DataSource = null;
                this.dgvSelectedUsers.DataSource = null;
                string rowFilter = this.dv1.RowFilter;
                string str2 = this.dv2.RowFilter;
                this.dv1.Dispose();
                this.dv2.Dispose();
                this.dv1 = null;
                this.dv2 = null;
                this.dt.BeginLoadData();
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 0;
                }
                this.dt.EndLoadData();
                this.dv1 = new DataView(this.dt);
                this.dv1.RowFilter = rowFilter;
                this.dv2 = new DataView(this.dt);
                this.dv2.RowFilter = str2;
                this.dgvUsers.DataSource = this.dv1;
                this.dgvSelectedUsers.DataSource = this.dv2;
                wgTools.WriteLine("btnDelAllUsers_Click End");
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedUsers);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            int num = 0;
            if (this.chkMonday.Checked)
            {
                num |= 1;
            }
            if (this.chkTuesday.Checked)
            {
                num |= 2;
            }
            if (this.chkWednesday.Checked)
            {
                num |= 4;
            }
            if (this.chkThursday.Checked)
            {
                num |= 8;
            }
            if (this.chkFriday.Checked)
            {
                num |= 0x10;
            }
            if (this.chkSaturday.Checked)
            {
                num |= 0x20;
            }
            if (this.chkSunday.Checked)
            {
                num |= 0x40;
            }
            string cmdText = (((((("update t_b_door set f_FirstCard_Enabled =" + (this.chkActive.Checked ? 1 : 0)) + ", f_FirstCard_BeginHMS=" + wgTools.PrepareStr(this.dateBeginHMS1.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat)) + ", f_FirstCard_BeginControl =" + this.cboBeginControlStatus.SelectedIndex) + ", f_FirstCard_EndHMS=" + wgTools.PrepareStr(this.dateEndHMS1.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat)) + ", f_FirstCard_EndControl=" + this.cboEndControlStatus.SelectedIndex) + ", f_FirstCard_Weekday=" + num) + " Where f_DoorID = " + this.DoorID;
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    goto Label_01F2;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    connection2.Open();
                    command2.ExecuteNonQuery();
                }
            }
        Label_01F2:
            cmdText = " Delete  FROM t_d_doorFirstCardUsers  WHERE f_DoorID= " + this.DoorID;
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command3 = new OleDbCommand(cmdText, connection3))
                    {
                        connection3.Open();
                        command3.ExecuteNonQuery();
                    }
                    goto Label_0291;
                }
            }
            using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command4 = new SqlCommand(cmdText, connection4))
                {
                    connection4.Open();
                    command4.ExecuteNonQuery();
                }
            }
        Label_0291:
            if (this.chkActive.Checked)
            {
                if (this.dgvSelectedUsers.DataSource != null)
                {
                    using (DataView view = this.dgvSelectedUsers.DataSource as DataView)
                    {
                        if (view.Count > 0)
                        {
                            for (int i = 0; i <= (view.Count - 1); i++)
                            {
                                cmdText = "INSERT INTO [t_d_doorFirstCardUsers](f_ConsumerID, f_DoorID )";
                                cmdText = (cmdText + " VALUES( " + view[i]["f_ConsumerID"].ToString()) + ", " + this.DoorID.ToString() + ")";
                                if (wgAppConfig.IsAccessDB)
                                {
                                    using (OleDbConnection connection5 = new OleDbConnection(wgAppConfig.dbConString))
                                    {
                                        using (OleDbCommand command5 = new OleDbCommand(cmdText, connection5))
                                        {
                                            connection5.Open();
                                            command5.ExecuteNonQuery();
                                        }
                                        continue;
                                    }
                                }
                                using (SqlConnection connection6 = new SqlConnection(wgAppConfig.dbConString))
                                {
                                    using (SqlCommand command6 = new SqlCommand(cmdText, connection6))
                                    {
                                        connection6.Open();
                                        command6.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
                this.retValue = "1";
            }
            else
            {
                this.retValue = "0";
            }
            base.DialogResult = DialogResult.OK;
            this.Cursor = current;
            base.Close();
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    dataSource.RowFilter = "f_Selected = 0";
                    this.strGroupFilter = "";
                }
                else
                {
                    dataSource.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
                    num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num2);
                            this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num2);
                        }
                        else
                        {
                            dataSource.RowFilter = "f_Selected = 0 ";
                            string str = "";
                            for (int i = 0; i < this.arrGroupNO.Count; i++)
                            {
                                if ((((int) this.arrGroupNO[i]) <= num3) && (((int) this.arrGroupNO[i]) >= num))
                                {
                                    if (str == "")
                                    {
                                        str = str + string.Format(" f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                    }
                                    else
                                    {
                                        str = str + string.Format(" OR f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                    }
                                }
                            }
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", str);
                            this.strGroupFilter = string.Format("  {0} ", str);
                        }
                    }
                }
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkActive.Checked)
            {
                this.grpBegin.Visible = true;
                this.grpEnd.Visible = true;
                this.grpUsers.Visible = true;
                this.grpWeekdayControl.Visible = true;
                if (this.dgvUsers.DataSource != null)
                {
                    DataView dataSource = (DataView) this.dgvUsers.DataSource;
                    this.dgvUsers.DataSource = null;
                    this.dgvUsers.DataSource = dataSource;
                }
            }
            else
            {
                this.grpBegin.Visible = false;
                this.grpEnd.Visible = false;
                this.grpUsers.Visible = false;
                this.grpWeekdayControl.Visible = false;
            }
        }

        private void dfrmFirstCard_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmFirstCard_Load_Acc(sender, e);
            }
            else
            {
                this.dateEndHMS1.CustomFormat = "HH:mm";
                this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS1.Value = DateTime.Parse("08:00:00");
                this.dateBeginHMS1.CustomFormat = "HH:mm";
                this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS1.Value = DateTime.Parse("08:00:00");
                this.loadGroupData();
                string cmdText = "SELECT t_b_Door.*  FROM  t_b_Door  ";
                cmdText = cmdText + " Where  f_DoorID = " + this.DoorID;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.controllerID = (int) reader["f_ControllerID"];
                            this.doorNo = int.Parse(reader["f_DoorNo"].ToString());
                            this.dateBeginHMS1.Value = DateTime.Parse(reader["f_FirstCard_BeginHMS"].ToString());
                            this.dateEndHMS1.Value = DateTime.Parse(reader["f_FirstCard_EndHMS"].ToString());
                            try
                            {
                                this.cboBeginControlStatus.SelectedIndex = ((((int) reader["f_FirstCard_BeginControl"]) >= 0) && (((int) reader["f_FirstCard_BeginControl"]) < 4)) ? ((int) reader["f_FirstCard_BeginControl"]) : 0;
                                this.cboEndControlStatus.SelectedIndex = ((((int) reader["f_FirstCard_EndControl"]) >= 0) && (((int) reader["f_FirstCard_EndControl"]) < 4)) ? ((int) reader["f_FirstCard_EndControl"]) : 0;
                            }
                            catch (Exception)
                            {
                                if (this.cboBeginControlStatus.Items.Count > 0)
                                {
                                    this.cboBeginControlStatus.SelectedIndex = 0;
                                }
                                if (this.cboEndControlStatus.Items.Count > 0)
                                {
                                    this.cboEndControlStatus.SelectedIndex = 0;
                                }
                            }
                            if (wgTools.SetObjToStr(reader["f_FirstCard_Weekday"]) != "")
                            {
                                this.chkMonday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 1) > 0;
                                this.chkTuesday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 2) > 0;
                                this.chkWednesday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 4) > 0;
                                this.chkThursday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 8) > 0;
                                this.chkFriday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 0x10) > 0;
                                this.chkSaturday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 0x20) > 0;
                                this.chkSunday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 0x40) > 0;
                            }
                            if (((int) reader["f_FirstCard_Enabled"]) > 0)
                            {
                                this.chkActive.Checked = true;
                            }
                            else
                            {
                                this.chkActive.Checked = false;
                            }
                            this.chkActive_CheckedChanged(null, null);
                        }
                        reader.Close();
                    }
                }
                this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
                this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void dfrmFirstCard_Load_Acc(object sender, EventArgs e)
        {
            this.dateEndHMS1.CustomFormat = "HH:mm";
            this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS1.Value = DateTime.Parse("08:00:00");
            this.dateBeginHMS1.CustomFormat = "HH:mm";
            this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS1.Value = DateTime.Parse("08:00:00");
            this.loadGroupData();
            string cmdText = "SELECT t_b_Door.*  FROM  t_b_Door  ";
            cmdText = cmdText + " Where  f_DoorID = " + this.DoorID;
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        this.controllerID = (int) reader["f_ControllerID"];
                        this.doorNo = int.Parse(reader["f_DoorNo"].ToString());
                        this.dateBeginHMS1.Value = DateTime.Parse(reader["f_FirstCard_BeginHMS"].ToString());
                        this.dateEndHMS1.Value = DateTime.Parse(reader["f_FirstCard_EndHMS"].ToString());
                        try
                        {
                            this.cboBeginControlStatus.SelectedIndex = ((((int) reader["f_FirstCard_BeginControl"]) >= 0) && (((int) reader["f_FirstCard_BeginControl"]) < 4)) ? ((int) reader["f_FirstCard_BeginControl"]) : 0;
                            this.cboEndControlStatus.SelectedIndex = ((((int) reader["f_FirstCard_EndControl"]) >= 0) && (((int) reader["f_FirstCard_EndControl"]) < 4)) ? ((int) reader["f_FirstCard_EndControl"]) : 0;
                        }
                        catch (Exception)
                        {
                            if (this.cboBeginControlStatus.Items.Count > 0)
                            {
                                this.cboBeginControlStatus.SelectedIndex = 0;
                            }
                            if (this.cboEndControlStatus.Items.Count > 0)
                            {
                                this.cboEndControlStatus.SelectedIndex = 0;
                            }
                        }
                        if (wgTools.SetObjToStr(reader["f_FirstCard_Weekday"]) != "")
                        {
                            this.chkMonday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 1) > 0;
                            this.chkTuesday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 2) > 0;
                            this.chkWednesday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 4) > 0;
                            this.chkThursday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 8) > 0;
                            this.chkFriday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 0x10) > 0;
                            this.chkSaturday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 0x20) > 0;
                            this.chkSunday.Checked = (((int) reader["f_FirstCard_Weekday"]) & 0x40) > 0;
                        }
                        if (((int) reader["f_FirstCard_Enabled"]) > 0)
                        {
                            this.chkActive.Checked = true;
                        }
                        else
                        {
                            this.chkActive.Checked = false;
                        }
                        this.chkActive_CheckedChanged(null, null);
                    }
                    reader.Close();
                }
            }
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
            this.backgroundWorker1.RunWorkerAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadGroupData()
        {
            new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
            for (int i = 0; i < this.arrGroupID.Count; i++)
            {
                if ((i == 0) && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
                {
                    this.cbof_GroupID.Items.Add(CommonStr.strAll);
                }
                else
                {
                    this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
                }
            }
            if (this.cbof_GroupID.Items.Count > 0)
            {
                this.cbof_GroupID.SelectedIndex = 0;
            }
        }

        private DataTable loadUserData4BackWork()
        {
            string str;
            Thread.Sleep(100);
            wgTools.WriteLine("loadUserData Start");
            this.dtUser1 = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT  t_b_Consumer.f_ConsumerID ";
                str = (((str + " , f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , IIF ( f_doorFirstCardUsersId IS NULL , 0 , 1 ) AS f_Selected " + " , f_GroupID ") + " FROM t_b_Consumer " + string.Format(" LEFT OUTER JOIN t_d_doorFirstCardUsers ON ( t_b_Consumer.f_ConsumerID=t_d_doorFirstCardUsers.f_ConsumerID AND f_DoorID= {0})", this.DoorID.ToString())) + " WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUser1);
                        }
                    }
                    goto Label_019A;
                }
            }
            str = " SELECT  t_b_Consumer.f_ConsumerID ";
            str = (((str + " , f_ConsumerNO, f_ConsumerName, f_CardNO " + " , CASE WHEN f_doorFirstCardUsersId IS NULL THEN 0 ELSE 1 END AS f_Selected ") + " , f_GroupID " + " FROM t_b_Consumer ") + " LEFT OUTER JOIN t_d_doorFirstCardUsers ON t_b_Consumer.f_ConsumerID=t_d_doorFirstCardUsers.f_ConsumerID AND f_DoorID= " + this.DoorID) + " WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtUser1);
                    }
                }
            }
        Label_019A:
            wgTools.WriteLine("da.Fill End");
            try
            {
                this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
            }
            catch (Exception)
            {
                throw;
            }
            lastLoadUsers = icConsumerShare.getUpdateLog();
            dtLastLoad = this.dtUser1;
            return this.dtUser1;
        }

        private void loadUserData4BackWorkComplete(DataTable dtUser)
        {
            this.dv = new DataView(dtUser);
            this.dvSelected = new DataView(dtUser);
            this.dv.RowFilter = "f_Selected = 0";
            this.dvSelected.RowFilter = "f_Selected > 0";
            this.dvSelected.Sort = " f_ConsumerNo ASC ";
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            this.dgvSelectedUsers.AutoGenerateColumns = false;
            this.dgvSelectedUsers.DataSource = this.dvSelected;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
            }
            this.cbof_GroupID_SelectedIndexChanged(null, null);
            wgTools.WriteLine("loadUserData End");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource == null)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                this.timer1.Enabled = false;
                Cursor.Current = Cursors.Default;
                this.lblWait.Visible = false;
                this.grpUsers.Enabled = true;
                this.btnOK.Enabled = true;
            }
        }
    }
}

