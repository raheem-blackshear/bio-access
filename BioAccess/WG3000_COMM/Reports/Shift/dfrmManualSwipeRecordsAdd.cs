namespace WG3000_COMM.Reports.Shift
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

    public partial class dfrmManualSwipeRecordsAdd : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private DataTable dt;
        private DataTable dtUser;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvSelected;
        private string strGroupFilter = "";

        public dfrmManualSwipeRecordsAdd()
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
            if (this.strGroupFilter == "")
            {
                icConsumerShare.selectAllUsers();
                ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter());
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter());
            }
            else
            {
                wgTools.WriteLine("btnAddAllUsers_Click Start");
                this.dt = ((DataView) this.dgvUsers.DataSource).Table;
                this.dv1 = (DataView) this.dgvUsers.DataSource;
                this.dv2 = (DataView) this.dgvSelectedUsers.DataSource;
                this.dgvUsers.DataSource = null;
                this.dgvSelectedUsers.DataSource = null;
                if (this.strGroupFilter != "")
                {
                    this.dv = new DataView(this.dt);
                    this.dv.RowFilter = this.strGroupFilter;
                    for (int i = 0; i < this.dv.Count; i++)
                    {
                        this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
                    }
                }
                this.dgvUsers.DataSource = this.dv1;
                this.dgvSelectedUsers.DataSource = this.dv2;
                wgTools.WriteLine("btnAddAllUsers_Click End");
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDelAllUsers_Click(object sender, EventArgs e)
        {
            if (this.dgvSelectedUsers.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("btnDelAllUsers_Click Start");
                icConsumerShare.selectNoneUsers();
                if (string.IsNullOrEmpty(this.strGroupFilter))
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getSelectedRowfilter());
            }
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
                return;
            }
            if (this.dgvSelectedUsers.Rows.Count <= 0)
            {
                return;
            }
            bool flag = false;
            int count = 0;
            string cmdText = "";
            using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    connection.Open();
                    while (count < this.dgvSelectedUsers.Rows.Count)
                    {
                        string str = "";
                        if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                        {
                            while (count < this.dgvSelectedUsers.Rows.Count)
                            {
                                str = str + ((DataView) this.dgvSelectedUsers.DataSource)[count]["f_ConsumerID"] + ",";
                                if (str.Length > 0x7d0)
                                {
                                    break;
                                }
                                count++;
                            }
                            str = str + "0";
                        }
                        else
                        {
                            count = this.dgvSelectedUsers.Rows.Count;
                        }
                        cmdText = "INSERT INTO [t_d_ManualCardRecord] (f_ConsumerID, f_ReadDate,f_Note)";
                        cmdText = (((cmdText + " SELECT t_b_Consumer.f_ConsumerID ") + ", " + wgTools.PrepareStr(this.dtpStartDate.Value.ToString("yyyy-MM-dd") + this.dateEndHMS2.Value.ToString(" HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss") + " AS [f_ReadDate] ") + ", " + wgTools.PrepareStr(this.txtf_Notes.Text) + " AS [f_Note] ") + " FROM t_b_Consumer";
                        if (str != "")
                        {
                            cmdText = cmdText + " WHERE [f_ConsumerID] IN (" + str + " ) ";
                        }
                        command.CommandText = cmdText;
                        if (command.ExecuteNonQuery() > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                            goto Label_0211;
                        }
                        wgTools.WriteLine("INSERT INTO [t_d_Leave] End");
                    }
                }
            }
        Label_0211:
            if (flag)
            {
                XMessageBox.Show((sender as Button).Text + "  " + count.ToString() + "  " + CommonStr.strUnit + "  " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            if (this.dgvSelectedUsers.Rows.Count <= 0)
            {
                return;
            }
            bool flag = false;
            int count = 0;
            string cmdText = "";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    while (count < this.dgvSelectedUsers.Rows.Count)
                    {
                        string str = "";
                        if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                        {
                            while (count < this.dgvSelectedUsers.Rows.Count)
                            {
                                str = str + ((DataView) this.dgvSelectedUsers.DataSource)[count]["f_ConsumerID"] + ",";
                                if (str.Length > 0x7d0)
                                {
                                    break;
                                }
                                count++;
                            }
                            str = str + "0";
                        }
                        else
                        {
                            count = this.dgvSelectedUsers.Rows.Count;
                        }
                        cmdText = "INSERT INTO [t_d_ManualCardRecord] (f_ConsumerID, f_ReadDate,f_Note)";
                        cmdText = (((cmdText + " SELECT t_b_Consumer.f_ConsumerID ") + ", " + wgTools.PrepareStr(this.dtpStartDate.Value.ToString("yyyy-MM-dd") + this.dateEndHMS2.Value.ToString(" HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss") + " AS [f_ReadDate] ") + ", " + wgTools.PrepareStr(this.txtf_Notes.Text) + " AS [f_Note] ") + " FROM t_b_Consumer";
                        if (str != "")
                        {
                            cmdText = cmdText + " WHERE [f_ConsumerID] IN (" + str + " ) ";
                        }
                        command.CommandText = cmdText;
                        if (command.ExecuteNonQuery() > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                            goto Label_0201;
                        }
                        wgTools.WriteLine("INSERT INTO [t_d_ManualCardRecord] End");
                    }
                }
            }
        Label_0201:
            if (flag)
            {
                XMessageBox.Show((sender as Button).Text + "  " + count.ToString() + "  " + CommonStr.strUnit + "  " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    dataSource.RowFilter = icConsumerShare.getOptionalRowfilter();
                    this.strGroupFilter = "";
                }
                else
                {
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
                            this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num2);
                        }
                        else
                        {
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
                            this.strGroupFilter = string.Format("  {0} ", str);
                        }
                    }
                    dataSource.RowFilter = string.Format("{0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                if (string.IsNullOrEmpty(this.strGroupFilter))
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("{0}", icConsumerShare.getOptionalRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format(" {0}", icConsumerShare.getSelectedRowfilter());
            }
        }

        private void dfrmManualSwipeRecordsAdd_Load(object sender, EventArgs e)
        {
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
            this.loadGroupData();
            this.dtpStartDate.Value = DateTime.Today;
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
            this.dateEndHMS2.CustomFormat = "HH:mm";
            this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS2.Value = DateTime.Parse("08:30:00");
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

        private void loadUserData()
        {
            wgTools.WriteLine("loadUserData Start");
            string cmdText = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID ";
            cmdText = cmdText + " FROM t_b_Consumer WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
            this.dtUser = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUser);
                        }
                    }
                    goto Label_00DC;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtUser);
                    }
                }
            }
        Label_00DC:
            wgTools.WriteLine("da.Fill End");
            try
            {
                this.dtUser.PrimaryKey = new DataColumn[] { this.dtUser.Columns[0] };
            }
            catch (Exception)
            {
                throw;
            }
            this.dv = new DataView(this.dtUser);
            this.dvSelected = new DataView(this.dtUser);
            this.dv.RowFilter = "f_Selected = 0";
            this.dvSelected.RowFilter = "f_Selected > 0";
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            this.dgvSelectedUsers.AutoGenerateColumns = false;
            this.dgvSelectedUsers.DataSource = this.dvSelected;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = this.dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers.Columns[i].DataPropertyName = this.dtUser.Columns[i].ColumnName;
            }
            wgTools.WriteLine("loadUserData End");
        }

        private DataTable loadUserData4BackWork()
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadUserData Start");
            icConsumerShare.loadUserData();
            return icConsumerShare.getDt();
        }

        private void loadUserData4BackWorkComplete(DataTable dtUser)
        {
            this.dv = new DataView(dtUser);
            this.dvSelected = new DataView(dtUser);
            this.dv.RowFilter = icConsumerShare.getOptionalRowfilter();
            this.dvSelected.RowFilter = icConsumerShare.getSelectedRowfilter();
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            this.dgvSelectedUsers.AutoGenerateColumns = false;
            this.dgvSelectedUsers.DataSource = this.dvSelected;
            for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dgvUsers.ColumnCount); i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
            }
            this.cbof_GroupID_SelectedIndexChanged(null, null);
            wgTools.WriteLine("loadUserData End");
            Cursor.Current = Cursors.Default;
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
                this.groupBox1.Enabled = true;
                this.btnOK.Enabled = true;
            }
        }
    }
}

