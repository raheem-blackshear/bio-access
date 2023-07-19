namespace WG3000_COMM.ExtendFunc.Patrol
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPatrolSetup : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private bool bNeedUpdatePatrolReader;
        private bool bNeedUpdatePatrolUsers;
        public int DoorID;
        private DataSet ds = new DataSet("dsPatrol");
        private DataTable dt;
        private static DataTable dtLastLoad;
        private DataTable dtPatrolReader;
        private DataTable dtUser1;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvPatrolReader;
        private DataView dvPatrolReaderSelected;
        private DataView dvSelected;
        private static string lastLoadUsers = "";
        public string retValue = "0";
        private string strGroupFilter = "";

        public dfrmPatrolSetup()
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

        private void btnAddAllReaders_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolReader = true;
            try
            {
                for (int i = 0; i < this.dtPatrolReader.Rows.Count; i++)
                {
                    this.dtPatrolReader.Rows[i]["f_Selected"] = 1;
                }
                this.dtPatrolReader.AcceptChanges();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnAddAllUsers_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolUsers = true;
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

        private void btnAddOneReader_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolReader = true;
            wgAppConfig.selectObject(this.dgvOptional);
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolUsers = true;
            wgAppConfig.selectObject(this.dgvUsers);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDelAllUsers_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolUsers = true;
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

        private void btnDeleteAllReaders_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolReader = true;
            try
            {
                for (int i = 0; i < this.dtPatrolReader.Rows.Count; i++)
                {
                    this.dtPatrolReader.Rows[i]["f_Selected"] = 0;
                }
                this.dtPatrolReader.AcceptChanges();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnDeleteOneReader_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolReader = true;
            wgAppConfig.deselectObject(this.dgvSelected);
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            this.bNeedUpdatePatrolUsers = true;
            wgAppConfig.deselectObject(this.dgvSelectedUsers);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string str;
                wgAppConfig.setSystemParamValue(0x1b, "", this.nudPatrolAbsentTimeout.Value.ToString(), "");
                wgAppConfig.setSystemParamValue(0x1c, "", this.nudPatrolAllowTimeout.Value.ToString(), "");
                if (this.bNeedUpdatePatrolReader)
                {
                    str = " DELETE FROM t_b_Reader4Patrol ";
                    wgAppConfig.runUpdateSql(str);
                    if (this.dvPatrolReaderSelected.Count > 0)
                    {
                        for (int i = 0; i <= (this.dvPatrolReaderSelected.Count - 1); i++)
                        {
                            str = " INSERT INTO t_b_Reader4Patrol";
                            wgAppConfig.runUpdateSql(((str + " (f_ReaderID) ") + " Values(" + this.dvPatrolReaderSelected[i]["f_ReaderID"]) + " )");
                        }
                    }
                }
                if (this.bNeedUpdatePatrolUsers)
                {
                    str = " Delete  FROM t_d_PatrolUsers  ";
                    wgAppConfig.runUpdateSql(str);
                    if (this.dgvSelectedUsers.DataSource != null)
                    {
                        using (DataView view = this.dgvSelectedUsers.DataSource as DataView)
                        {
                            if (view.Count > 0)
                            {
                                for (int j = 0; j <= (view.Count - 1); j++)
                                {
                                    wgAppConfig.runUpdateSql(("INSERT INTO [t_d_PatrolUsers](f_ConsumerID )" + " VALUES( ") + view[j]["f_ConsumerID"].ToString() + ")");
                                }
                            }
                        }
                    }
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
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

        private void dfrmPatrolSetup_Load(object sender, EventArgs e)
        {
            bool bReadOnly = false;
            string funName = "mnuPatrolDetailData";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnOK.Visible = false;
                this.nudPatrolAbsentTimeout.Enabled = false;
                this.nudPatrolAllowTimeout.Enabled = false;
                this.btnDelAllUsers.Enabled = false;
                this.btnDeleteAllReaders.Enabled = false;
                this.btnDeleteOneReader.Enabled = false;
                this.btnDelOneUser.Enabled = false;
                this.btnAddAllReaders.Enabled = false;
                this.btnAddAllUsers.Enabled = false;
                this.btnAddOneReader.Enabled = false;
                this.btnAddOneUser.Enabled = false;
            }
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmPatrolSetup_Load_Acc(sender, e);
            }
            else
            {
                this.nudPatrolAbsentTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(0x1b).ToString());
                this.nudPatrolAllowTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(0x1c).ToString());
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                SqlCommand selectCommand = new SqlCommand();
                try
                {
                    selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(this.ds, "optionalReader");
                    selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", connection);
                    new SqlDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                    this.dvPatrolReader = new DataView(this.ds.Tables["optionalReader"]);
                    this.dvPatrolReader.RowFilter = " f_Selected = 0";
                    this.dvPatrolReaderSelected = new DataView(this.ds.Tables["optionalReader"]);
                    this.dvPatrolReaderSelected.RowFilter = " f_Selected = 1";
                    this.dtPatrolReader = this.ds.Tables["optionalReader"];
                    try
                    {
                        this.dtPatrolReader.PrimaryKey = new DataColumn[] { this.dtPatrolReader.Columns[0] };
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
                    {
                        this.dgvOptional.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
                        this.dgvSelected.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
                    }
                    this.dvPatrolReader.RowFilter = "f_Selected = 0";
                    this.dvPatrolReaderSelected.RowFilter = "f_Selected > 0";
                    this.dgvOptional.AutoGenerateColumns = false;
                    this.dgvOptional.DataSource = this.dvPatrolReader;
                    this.dgvSelected.AutoGenerateColumns = false;
                    this.dgvSelected.DataSource = this.dvPatrolReaderSelected;
                    this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
                    this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                this.loadGroupData();
                this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
                this.UserID2.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID2.HeaderText);
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void dfrmPatrolSetup_Load_Acc(object sender, EventArgs e)
        {
            this.nudPatrolAbsentTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(0x1b).ToString());
            this.nudPatrolAllowTimeout.Value = decimal.Parse(wgAppConfig.getSystemParamByNO(0x1c).ToString());
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            OleDbCommand selectCommand = new OleDbCommand();
            try
            {
                selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_b_Reader4Patrol.f_ReaderID FROM t_b_Reader4Patrol  ) ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                this.dvPatrolReader = new DataView(this.ds.Tables["optionalReader"]);
                this.dvPatrolReader.RowFilter = " f_Selected = 0";
                this.dvPatrolReaderSelected = new DataView(this.ds.Tables["optionalReader"]);
                this.dvPatrolReaderSelected.RowFilter = " f_Selected = 1";
                this.dtPatrolReader = this.ds.Tables["optionalReader"];
                try
                {
                    this.dtPatrolReader.PrimaryKey = new DataColumn[] { this.dtPatrolReader.Columns[0] };
                }
                catch (Exception)
                {
                    throw;
                }
                for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
                {
                    this.dgvOptional.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
                    this.dgvSelected.Columns[i].DataPropertyName = this.dtPatrolReader.Columns[i].ColumnName;
                }
                this.dvPatrolReader.RowFilter = "f_Selected = 0";
                this.dvPatrolReaderSelected.RowFilter = "f_Selected > 0";
                this.dgvOptional.AutoGenerateColumns = false;
                this.dgvOptional.DataSource = this.dvPatrolReader;
                this.dgvSelected.AutoGenerateColumns = false;
                this.dgvSelected.DataSource = this.dvPatrolReaderSelected;
                this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
                this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.loadGroupData();
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            this.UserID2.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID2.HeaderText);
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
                str = (((str + " , f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , IIF ( t_d_PatrolUsers.f_ConsumerID IS NULL , 0 , 1 ) AS f_Selected " + " , f_GroupID ") + " FROM t_b_Consumer " + string.Format(" LEFT OUTER JOIN t_d_PatrolUsers ON ( t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID)", new object[0])) + " WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUser1);
                        }
                    }
                    goto Label_018A;
                }
            }
            str = " SELECT  t_b_Consumer.f_ConsumerID ";
            str = (((str + " , f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , CASE WHEN t_d_PatrolUsers.f_ConsumerID IS NULL THEN 0 ELSE 1 END AS f_Selected " + " , f_GroupID ") + " FROM t_b_Consumer " + " LEFT OUTER JOIN t_d_PatrolUsers ON ( t_b_Consumer.f_ConsumerID = t_d_PatrolUsers.f_ConsumerID ) ") + " WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
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
        Label_018A:
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

        private void btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}

