namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControllerExtendFuncPasswordManage : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private ArrayList arrReaderID = new ArrayList();
        private dfrmFind dfrmFind1 = new dfrmFind();
        private dfrmWait dfrmWait1 = new dfrmWait();
        private string dgvSql;
        private DataSet ds;
        private DataTable dtPasswordKeypad;
        private DataTable dtAuthMode;
        private DataTable dtReader;
        private DataTable dtReaderPassword;
        private DataTable dtUserData;
        private DataView dvPasswordKeypad;
        private DataView dvReader;
        private DataView dvReaderPassword;
        private DataView dvUserData;
        private int MaxRecord = 0x3e8;
        private int recNOMax = 0;
        private int startRecordIndex;
        private Dictionary<int, string> dicAuthModes = null;

        public dfrmControllerExtendFuncPasswordManage()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            int startIndex = (int) ((object[]) e.Argument)[0];
            int maxRecords = (int) ((object[]) e.Argument)[1];
            string strSql = (string) ((object[]) e.Argument)[2];
            e.Result = this.loadUserData(startIndex, maxRecords, strSql);
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
                if ((e.Result as DataView).Count < this.MaxRecord)
                {
                    
                }
                this.fillDgv(e.Result as DataView);
                this.cbof_GroupID_SelectedIndexChanged(null, null);
                //wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.txtPasswordNew.Text.Trim() == "")
            {
                this.txtPasswordNew.Text = "";
            }
            else
            {
                long num;
                if (!long.TryParse(this.txtPasswordNew.Text, out num))
                {
                    XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (num <= 0L)
                {
                    XMessageBox.Show(this, CommonStr.strPasswordWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (this.cboReader.Items.Count > 0)
                {
                    long num2 = num;
                    string str = "  Insert INTO t_b_ReaderPassword (f_Password, f_BAll, f_ReaderID) ";
                    str = str + " Values( " + num2;
                    if (this.cboReader.Items[0].ToString() == CommonStr.strAll)
                    {
                        if (this.cboReader.SelectedIndex == 0)
                        {
                            str = (str + " , " + 1) + " , " + 0;
                        }
                        else
                        {
                            str = (str + " , " + 0) + " , " + this.arrReaderID[this.cboReader.SelectedIndex - 1];
                        }
                    }
                    else
                    {
                        str = (str + " , " + 0) + " , " + this.arrReaderID[this.cboReader.SelectedIndex];
                    }
                    if (wgAppConfig.runUpdateSql(str + ")") > 0)
                    {
                        this.fillReaderPasswordGrid();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
/*
            if (this.dgvUsers.SelectedRows.Count <= 0)
            {
                if (this.dgvUsers.SelectedCells.Count <= 0)
                {
                    return;
                }
                int rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
            }
            else
            {
                int index = this.dgvUsers.SelectedRows[0].Index;
            }
            int num = 0;
            DataGridView dgvUsers = this.dgvUsers;
            DataGridViewColumn sortedColumn = dgvUsers.SortedColumn;
            if (dgvUsers.Rows.Count > 0)
            {
                num = dgvUsers.CurrentCell.RowIndex;
            }
            using (dfrmSetPassword password = new dfrmSetPassword())
            {
                password.operatorID = 0;
                password.Text = string.Concat(new object[] { this.btnChangePassword.Text, "  [", this.dgvUsers.Rows[num].Cells[2].Value, "] " });
                if (password.ShowDialog(this) == DialogResult.OK)
                {
                    string str2 = "";
                    string str = "Update t_b_Consumer ";
                    if (wgTools.SetObjToStr(password.newPassword) == "")
                    {
                        str = str + "SET [f_PIN]=" + wgTools.PrepareStr(0);
                        str2 = "0";
                    }
                    else
                    {
                        str = str + "SET [f_PIN]=" + wgTools.PrepareStr(password.newPassword);
                        str2 = wgTools.SetObjToStr(password.newPassword);
                    }
                    if (wgAppConfig.runUpdateSql((str + "  WHERE ") + " [f_ConsumerID]=" + this.dgvUsers.Rows[num].Cells[0].Value) == 1)
                    {
                        if (str2 == "0")
                        {
                            this.dgvUsers.Rows[num].Cells["strPwd"].Value = CommonStr.strPwdNoPassword;
                        }
                        else
                        {
                            int num2 = 0;
                            if (str2 == num2.ToString())
                            {
                                this.dgvUsers.Rows[num].Cells["strPwd"].Value = CommonStr.strPwdUnChanged;
                            }
                            else
                            {
                                this.dgvUsers.Rows[num].Cells["strPwd"].Value = CommonStr.strPwdChanged;
                            }
                        }
                    }
                }
            }*/
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dataGridView3.SelectedRows.Count <= 0)
            {
                if (this.dataGridView3.SelectedCells.Count <= 0)
                {
                    return;
                }
                int rowIndex = this.dataGridView3.SelectedCells[0].RowIndex;
            }
            else
            {
                int index = this.dataGridView3.SelectedRows[0].Index;
            }
            int num = 0;
            DataGridView view = this.dataGridView3;
            if (view.Rows.Count > 0)
            {
                num = view.CurrentCell.RowIndex;
            }
            int num2 = (int) view.Rows[num].Cells[0].Value;
            if (wgAppConfig.runUpdateSql("DELETE FROM t_b_ReaderPassword  WHERE f_Id =" + num2.ToString()) > 0)
            {
                this.fillReaderPasswordGrid();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.dfrmWait1.Show();
            this.dfrmWait1.Refresh();
            this.updatePasswordKeypadEnableGrid();
            this.updateManualPasswordKeypadEnableGrid();
            base.Close();
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
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
                }
                ((DataView) this.dgvUsers.DataSource).RowFilter = this.strGroupFilter;
            }*/
        }

        private void dfrmControllerExtendFuncPasswordManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
            try
            {
                if (this.dfrmWait1 != null)
                {
                    this.dfrmWait1.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void dfrmControllerExtendFuncPasswordManage_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmControllerExtendFuncPasswordManage_Load(object sender, EventArgs e)
        {
            this.dfrmWait1.Show();
            this.dfrmWait1.Refresh();
            this.fillPasswordKeypadEnableGrid();
            this.fillUsersPasswordGrid();
            this.fillReaderPasswordGrid();
            this.fillManualPasswordKeypadEnableGrid();
            //this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dataGridView3.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            //this.dataGridView4.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.tabPage1.BackColor = this.BackColor;
            //this.tabPage2.BackColor = this.BackColor;
            this.tabPage3.BackColor = this.BackColor;
            //this.tabPage4.BackColor = this.BackColor;
            //this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            //this.Deptname.HeaderText = wgAppConfig.ReplaceFloorRomm(this.Deptname.HeaderText);
            //this.ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.ConsumerNO.HeaderText);
            this.loadOperatorPrivilege();
            this.dfrmWait1.Hide();
            try
            {
                base.Owner.Show();
                base.Owner.Activate();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            //this.btnChangePassword.PerformClick();
        }

        private void dgvUsers_Scroll(object sender, ScrollEventArgs e)
        {
            /* if (!this.bLoadedFinished && (e.ScrollOrientation == ScrollOrientation.VerticalScroll))
            {
                wgTools.WriteLine(e.OldValue.ToString());
                wgTools.WriteLine(e.NewValue.ToString());
                DataGridView dgvUsers = this.dgvUsers;
                if ((e.NewValue > e.OldValue) && (((e.NewValue + 100) > dgvUsers.Rows.Count) || ((e.NewValue + (dgvUsers.Rows.Count / 10)) > dgvUsers.Rows.Count)))
                {
                    if (this.startRecordIndex <= dgvUsers.Rows.Count)
                    {
                        if (!this.backgroundWorker1.IsBusy)
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                        }
                    }
                    else
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
                    }
                }
            }*/
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.dfrmWait1 != null))
            {
                this.dfrmWait1.Dispose();
            }
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

        private void fillDgv(DataView dv)
        {
            /* try
            {
                DataGridView dgvUsers = this.dgvUsers;
                if (dgvUsers.DataSource == null)
                {
                    this.dgvUsers.AutoGenerateColumns = false;
                    dgvUsers.DataSource = dv;
                    for (int i = 0; (i < dv.Table.Columns.Count) && (i < dgvUsers.ColumnCount); i++)
                    {
                        dgvUsers.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
                    }
                    wgAppConfig.ReadGVStyle(this, dgvUsers);
                    dgvUsers.Columns[0].Visible = false;
                    if ((this.startRecordIndex == 0) && (dv.Count >= this.MaxRecord))
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                    }
                }
                else if (dv.Count > 0)
                {
                    int firstDisplayedScrollingRowIndex = dgvUsers.FirstDisplayedScrollingRowIndex;
                    DataView dataSource = dgvUsers.DataSource as DataView;
                    dataSource.Table.Merge(dv.Table);
                    if (firstDisplayedScrollingRowIndex >= 0)
                    {
                        dgvUsers.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    }
                    dgvUsers.Columns[0].Visible = false;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;*/
        }

        private void fillManualPasswordKeypadEnableGrid()
        {
            icControllerZone zone;
            string cmdText = " SELECT ";
            cmdText = (((cmdText + " t_b_Reader.f_ReaderID " + ", t_b_Controller.f_ControllerSN ") + ", t_b_Reader.f_ReaderNO " + ", t_b_Reader.f_ReaderName ") + ", t_b_Reader.f_InputCardno_Enabled " + ", t_b_Controller.f_ZoneID ") + "FROM t_b_Reader INNER JOIN t_b_Controller ON t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID " + " ORDER BY [f_ReaderID] ";
            this.dtReader = new DataTable();
            this.dvReader = new DataView(this.dtReader);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtReader);
                        }
                    }
                    goto Label_012B;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtReader);
                    }
                }
            }
        Label_012B:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dtReader);
            /*
            this.dataGridView4.AutoGenerateColumns = false;
            this.dataGridView4.DataSource = this.dvReader;
            for (int i = 0; (i < this.dvReader.Table.Columns.Count) && (i < this.dataGridView4.ColumnCount); i++)
            {
                this.dataGridView4.Columns[i].DataPropertyName = this.dvReader.Table.Columns[i].ColumnName;
            } */
        }

        private void fillPasswordKeypadEnableGrid()
        {
            icControllerZone zone;
            string cmdText = " SELECT ";
            cmdText = (((cmdText + " t_b_Reader.f_ReaderID " + ", t_b_Controller.f_ControllerSN ") + ", t_b_Reader.f_ReaderNO " + ", t_b_Reader.f_ReaderName ") + ", t_b_Reader.f_AuthenticationMode " + ", t_b_Controller.f_ZoneID ") + " FROM t_b_Reader INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) " + " ORDER BY [f_ReaderID] ";
            string cmdAuth = " SELECT * FROM t_d_AuthModes";
            this.dtPasswordKeypad = new DataTable();
            this.dvPasswordKeypad = new DataView(this.dtPasswordKeypad);
            dtAuthMode = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtPasswordKeypad);
                        }
                    }
                    using (OleDbCommand command = new OleDbCommand(cmdAuth, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtAuthMode);
                        }
                    }
                    goto Label_012B;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtPasswordKeypad);
                    }
                }
                using (SqlCommand command2 = new SqlCommand(cmdAuth, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtAuthMode);
                    }
                }
            }
        Label_012B:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dtPasswordKeypad);
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = this.dvPasswordKeypad;
            for (int i = 0; (i < this.dvPasswordKeypad.Table.Columns.Count - 1) && (i < this.dataGridView1.ColumnCount); i++)
            {
                this.dataGridView1.Columns[i].DataPropertyName = this.dvPasswordKeypad.Table.Columns[i].ColumnName;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int mode = (int)dataGridView1.Rows[i].Cells["f_AuthenticationMode"].Value;
                Dictionary<int, string> modes = getAuthModes((int)dataGridView1.Rows[i].Cells["f_ControllerSN"].Value,
                    (int)dataGridView1.Rows[i].Cells["f_ReaderNO"].Value);
                dataGridView1.Rows[i].Cells["f_AuthenticationMode1"].Value = modes[mode];
            }
        }

        private void fillReaderPasswordGrid()
        {
            string str;
            icControllerZone zone;
            int num;
            if (wgAppConfig.IsAccessDB)
            {
                str = string.Format(" SELECT  t_b_ReaderPassword.f_Id , t_b_ReaderPassword.f_Password , IIF ( f_BALL=1 , {0}, tt.f_ReaderName ) AS f_AdaptTo , tt.f_ZoneID  ", wgTools.PrepareStr(CommonStr.strAll)) + string.Format(" FROM    (SELECT t_b_Reader.f_ReaderID,t_b_Reader.f_ReaderName,  t_b_Controller.f_ZoneID  from    t_b_Controller INNER JOIN t_b_Reader ON  t_b_Controller.f_ControllerID = t_b_Reader.f_ControllerID  ) As tt  Right JOIN t_b_ReaderPassword ON ( tt.f_ReaderID = t_b_ReaderPassword.f_ReaderID) ", new object[0]) + " ORDER BY [f_Id] ";
            }
            else
            {
                str = " SELECT ";
                str = ((((str + " t_b_ReaderPassword.f_Id " + ", t_b_ReaderPassword.f_Password ") + ", CASE WHEN f_BALL=1 THEN " + wgTools.PrepareStr(CommonStr.strAll)) + " ELSE t_b_Reader.f_ReaderName ") + " END AS f_AdaptTo " + ", c.f_ZoneID ") + " FROM t_b_ReaderPassword LEFT JOIN (t_b_Reader INNER JOIN t_b_Controller c ON c.f_ControllerID = t_b_Reader.f_ControllerID) ON t_b_Reader.f_ReaderID = t_b_ReaderPassword.f_ReaderID " + " ORDER BY [f_Id] ";
            }
            this.ds = new DataSet();
            this.dtReaderPassword = new DataTable();
            this.dvReaderPassword = new DataView(this.dtReaderPassword);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtReaderPassword);
                        }
                    }
                    goto Label_0181;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtReaderPassword);
                    }
                }
            }
        Label_0181:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dtReaderPassword);
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView3.DataSource = this.dvReaderPassword;
            for (num = 0; (num < this.dvReaderPassword.Table.Columns.Count) && (num < this.dataGridView3.ColumnCount); num++)
            {
                this.dataGridView3.Columns[num].DataPropertyName = this.dvReaderPassword.Table.Columns[num].ColumnName;
            }
            if (this.cboReader.Items.Count != 0)
            {
                return;
            }
            this.cboReader.Items.Clear();
            this.arrReaderID.Clear();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command3 = new OleDbCommand("Select t_b_reader.*,t_b_Controller.f_ZoneID from t_b_reader inner join t_b_Controller on t_b_controller.f_controllerID = t_b_reader.f_ControllerID ", connection3))
                    {
                        using (OleDbDataAdapter adapter3 = new OleDbDataAdapter(command3))
                        {
                            adapter3.Fill(this.ds, "reader");
                        }
                    }
                    goto Label_0310;
                }
            }
            using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command4 = new SqlCommand("Select t_b_reader.*,t_b_Controller.f_ZoneID from t_b_reader inner join t_b_Controller on t_b_controller.f_controllerID = t_b_reader.f_ControllerID ", connection4))
                {
                    using (SqlDataAdapter adapter4 = new SqlDataAdapter(command4))
                    {
                        adapter4.Fill(this.ds, "reader");
                    }
                }
            }
        Label_0310:
            this.dtReader = this.ds.Tables["reader"];
            int count = this.dtReader.Rows.Count;
            zone.getAllowedControllers(ref this.dtReader);
            if (count == this.dtReader.Rows.Count)
            {
                this.cboReader.Items.Add(CommonStr.strAll);
            }
            if (this.ds.Tables["reader"].Rows.Count > 0)
            {
                for (num = 0; num < this.ds.Tables["reader"].Rows.Count; num++)
                {
                    string s = wgTools.SetObjToStr(this.ds.Tables["reader"].Rows[num]["f_ReaderName"]);
                    if (this.cboReader.FindString(s) < 0)
                    {
                        this.cboReader.Items.Add(s);
                        this.arrReaderID.Add(this.ds.Tables["reader"].Rows[num]["f_ReaderID"]);
                    }
                }
            }
            if (this.cboReader.Items.Count > 0)
            {
                this.cboReader.SelectedIndex = 0;
            }
        }

        private void fillUsersPasswordGrid()
        {
            /*
            string str;
            this.loadStyle();
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
            Cursor.Current = Cursors.WaitCursor;
            if (wgAppConfig.IsAccessDB)
            {
                str = string.Format(" SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO,  f_GroupName , IIF ( f_PIN=0, {0}, IIF (f_PIN = 345678, {1},{2}))  AS strPwd, t_b_Consumer.f_GroupID  ", wgTools.PrepareStr(CommonStr.strPwdNoPassword), wgTools.PrepareStr(CommonStr.strPwdUnChanged), wgTools.PrepareStr(CommonStr.strPwdChanged)) + " FROM ( t_b_Consumer LEFT OUTER JOIN t_b_Group ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) ) ";
            }
            else
            {
                str = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO,  f_GroupName ";
                str = (((((str + " ,  CASE WHEN f_PIN=0 THEN " + wgTools.PrepareStr(CommonStr.strPwdNoPassword)) + " ELSE  ") + " CASE WHEN f_PIN=345678 THEN " + wgTools.PrepareStr(CommonStr.strPwdUnChanged)) + " ELSE  " + wgTools.PrepareStr(CommonStr.strPwdChanged)) + " END   ") + " END  AS strPwd, t_b_Consumer.f_GroupID  " + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ";
            }
            this.reloadUserData(str);*/
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuPasswordManagement";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnOK.Visible = false;
                //this.btnChangePassword.Visible = false;
                this.btnAdd.Visible = false;
                this.btnDel.Visible = false;
                this.dataGridView1.ReadOnly = true;
                this.dataGridView3.ReadOnly = true;
                //this.dataGridView4.ReadOnly = true;
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.txtPasswordNew.Visible = false;
                this.cboReader.Visible = false;
            }
        }

        private void loadStyle()
        {
            /*
            this.dgvUsers.AutoGenerateColumns = false;
            wgAppConfig.ReadGVStyle(this, this.dgvUsers); */
        }

        private DataView loadUserData(int startIndex, int maxRecords, string strSql)
        {
            wgTools.WriteLine("loadUserData Start");
            if (strSql.ToUpper().IndexOf("SELECT ") > 0)
                strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            if (startIndex == 0)
                this.recNOMax = 0;
            else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
                strSql = strSql + string.Format(" AND f_ConsumerID > {0}", this.recNOMax.ToString());
            else
                strSql = strSql + string.Format(" WHERE f_ConsumerID > {0}", this.recNOMax.ToString());
            strSql = strSql + " ORDER BY f_ConsumerID ";
            this.dtUserData = new DataTable("users");
            this.dvUserData = new DataView(this.dtUserData);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUserData);
                        }
                    }
                    goto Label_0187;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(strSql, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtUserData);
                    }
                }
            }
        Label_0187:
            if (this.dtUserData.Rows.Count > 0)
            {
                this.recNOMax = (int)this.dtUserData.Rows[this.dtUserData.Rows.Count - 1]["f_ConsumerID"];
            }
            wgTools.WriteLine("loadUserData End");
            return this.dvUserData;
        }

        private void reloadUserData(string strsql)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.startRecordIndex = 0;
                this.MaxRecord = 0x3e8;
                if (!string.IsNullOrEmpty(strsql))
                {
                    this.dgvSql = strsql;
                }
                //this.dgvUsers.DataSource = null;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        private void txtPasswordNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            int num;
            if ((e.KeyChar != '\b') && !int.TryParse(e.KeyChar.ToString(), out num))
            {
                e.Handled = true;
            }
        }

        private void updateManualPasswordKeypadEnableGrid()
        {
            /*
            if (wgAppConfig.IsAccessDB)
            {
                this.updateManualPasswordKeypadEnableGrid_Acc();
            }
            else
            {
                this.dtReader = (this.dataGridView4.DataSource as DataView).Table;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        for (int i = 0; i <= (this.dtReader.Rows.Count - 1); i++)
                        {
                            string str = " UPDATE t_b_Reader SET ";
                            str = (str + " f_InputCardno_Enabled = " + ((this.dtReader.Rows[i]["f_InputCardno_Enabled"].ToString() == "1") ? "1" : "0")) + " WHERE f_ReaderID = " + this.dtReader.Rows[i]["f_ReaderID"];
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            } */
        }

        private void updateManualPasswordKeypadEnableGrid_Acc()
        {
            /*
            this.dtReader = (this.dataGridView4.DataSource as DataView).Table;
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("", connection))
                {
                    connection.Open();
                    for (int i = 0; i <= (this.dtReader.Rows.Count - 1); i++)
                    {
                        string str = " UPDATE t_b_Reader SET ";
                        str = (str + " f_InputCardno_Enabled = " + ((this.dtReader.Rows[i]["f_InputCardno_Enabled"].ToString() == "1") ? "1" : "0")) + " WHERE f_ReaderID = " + this.dtReader.Rows[i]["f_ReaderID"];
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
            }*/
        }

        private void updatePasswordKeypadEnableGrid()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.updatePasswordKeypadEnableGrid_Acc();
            }
            else
            {
                this.dtPasswordKeypad = (this.dataGridView1.DataSource as DataView).Table;
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        for (int i = 0; i <= (this.dtPasswordKeypad.Rows.Count - 1); i++)
                        {
                            string str = " UPDATE t_b_Reader SET ";
                            str = str + " f_AuthenticationMode = " + this.dtPasswordKeypad.Rows[i]["f_AuthenticationMode"].ToString() + " WHERE f_ReaderID = " + this.dtPasswordKeypad.Rows[i]["f_ReaderID"];
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void updatePasswordKeypadEnableGrid_Acc()
        {
            this.dtPasswordKeypad = (this.dataGridView1.DataSource as DataView).Table;
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("", connection))
                {
                    connection.Open();
                    for (int i = 0; i <= (this.dtPasswordKeypad.Rows.Count - 1); i++)
                    {
                        string str = " UPDATE t_b_Reader SET ";
                        str = str + " f_AuthenticationMode = " + this.dtPasswordKeypad.Rows[i]["f_AuthenticationMode"].ToString() + " WHERE f_ReaderID = " + this.dtPasswordKeypad.Rows[i]["f_ReaderID"];
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private Dictionary<int, string> getAuthModes(int sn, int pos)
        {
            wgMjController.DeviceType type = (pos == 1) ? wgMjController.getDeviceType(sn) : 
                wgMjController.DeviceType.A30_AC;
            string devName = "";
            switch (type)
            {
                case wgMjController.DeviceType.A30_AC:
                case wgMjController.DeviceType.A50_AC:
                    devName = "A30";
                    break;
                case wgMjController.DeviceType.F300_AC:
                    devName = "F300AC";
                    break;
            }
            if (devName == "")
                return null;

            Dictionary<int, string> dic = new Dictionary<int, string>();
            string strCommand = " SELECT * FROM t_d_AuthModes WHERE [f_AuthDevType]='" + devName +"' ORDER BY f_AuthMode";
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = strCommand;
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            dic.Add((int)reader["f_AuthMode"], (string)reader[CommonStr.strAuthDesc]);
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        command.CommandText = strCommand;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            dic.Add((int)reader["f_AuthMode"], (string)reader[CommonStr.strAuthDesc]);
                        }
                    }
                }
            }
            return dic;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cmbAuthMode.Items.Clear();
                int mode = (int)dataGridView1.Rows[e.RowIndex].Cells["f_AuthenticationMode"].Value;
                dicAuthModes = getAuthModes((int)dataGridView1.Rows[e.RowIndex].Cells["f_ControllerSN"].Value,
                    (int)dataGridView1.Rows[e.RowIndex].Cells["f_ReaderNO"].Value);
                int selected = 0;
                if (dicAuthModes != null)
                {
                    foreach (int key in dicAuthModes.Keys)
                    {
                        cmbAuthMode.Items.Add(dicAuthModes[key]);
                        if (mode == key)
                            selected = cmbAuthMode.Items.Count - 1;
                    }
                    cmbAuthMode.SelectedIndex = selected;
                }
            }
        }

        private void cmbAuthMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dicAuthModes != null)
            {
                foreach (int key in dicAuthModes.Keys)
                {
                    if (dicAuthModes[key] == cmbAuthMode.SelectedItem.ToString())
                    {
                        if (dataGridView1.SelectedCells.Count > 0)
                        {
                            int row = dataGridView1.SelectedCells[0].RowIndex;
                            dataGridView1.Rows[row].Cells["f_AuthenticationMode"].Value = key;
                            dataGridView1.Rows[row].Cells["f_AuthenticationMode1"].Value = dicAuthModes[key];
                            break;
                        }
                    }
                }
            }
        }
    }
}

