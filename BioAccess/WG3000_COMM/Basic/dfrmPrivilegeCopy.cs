namespace WG3000_COMM.Basic
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

    public partial class dfrmPrivilegeCopy : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private BackgroundWorker backgroundWorker1;
        private bool bEdit;
        private bool bStarting = true;
        private SqlCommand cm;
        private SqlConnection cn;
        private string strGroupFilter = "";
        private System.Windows.Forms.Timer timer1;

        public dfrmPrivilegeCopy()
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
                ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                int rowCount = this.dgvSelectedUsers.RowCount;
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
                    string rowFilter = this.dv1.RowFilter;
                    string str2 = this.dv2.RowFilter;
                    this.dv1.Dispose();
                    this.dv2.Dispose();
                    this.dv1 = null;
                    this.dv2 = null;
                    this.dt.BeginLoadData();
                    this.dv = new DataView(this.dt);
                    this.dv.RowFilter = this.strGroupFilter;
                    for (int i = 0; i < this.dv.Count; i++)
                    {
                        this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
                    }
                    this.dt.EndLoadData();
                    this.dv1 = new DataView(this.dt);
                    this.dv1.RowFilter = rowFilter;
                    this.dv2 = new DataView(this.dt);
                    this.dv2.RowFilter = str2;
                    this.dgvUsers.DataSource = this.dv1;
                    this.dgvSelectedUsers.DataSource = this.dv2;
                    wgTools.WriteLine("btnAddAllUsers_Click End");
                    int count = this.dv2.Count;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
            int rowCount = this.dgvSelectedUsers.RowCount;
        }

        private void btnAddOneUser4Copy_Click(object sender, EventArgs e)
        {
            if (this.dt4copy.Rows.Count <= 0)
            {
                try
                {
                    int rowIndex;
                    DataGridView dgvUsers = this.dgvUsers;
                    if (dgvUsers.SelectedRows.Count <= 0)
                    {
                        if (dgvUsers.SelectedCells.Count <= 0)
                        {
                            return;
                        }
                        rowIndex = dgvUsers.SelectedCells[0].RowIndex;
                    }
                    else
                    {
                        rowIndex = dgvUsers.SelectedRows[0].Index;
                    }
                    DataTable table = ((DataView) dgvUsers.DataSource).Table;
                    int key = (int) dgvUsers.Rows[rowIndex].Cells[0].Value;
                    DataRow row = table.Rows.Find(key);
                    if (row != null)
                    {
                        DataRow row2 = this.dt4copy.NewRow();
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            row2[i] = row[i];
                        }
                        table.Rows.Remove(row);
                        table.AcceptChanges();
                        this.dt4copy.Rows.Add(row2);
                        this.dt4copy.AcceptChanges();
                    }
                    icConsumerShare.setUpdateLog();
                }
                catch (Exception exception)
                {
                    wgTools.WriteLine(exception.ToString());
                }
            }
        }

        private void btnAddPass_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnAddPass_Click_Acc(sender, e);
            }
            else if (((this.dgvSelectedUsers.RowCount > 0) && (this.dgvSelectedUsers4Copy.RowCount > 0)) && (XMessageBox.Show(this.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel))
            {
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                this.cm = new SqlCommand("", this.cn);
                bool flag = false;
                this.timer1.Enabled = true;
                if (this.dgvSelectedUsers.Rows.Count > 0x3e8)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                try
                {
                    string str2 = "";
                    DataView dataSource = (DataView) this.dgvSelectedUsers.DataSource;
                    DataView view = (DataView) this.dgvSelectedUsers4Copy.DataSource;
                    if (this.cn.State != ConnectionState.Open)
                    {
                        this.cn.Open();
                    }
                    try
                    {
                        string str;
                        int num = 0x7d0;
                        int num2 = 0;
                        this.progressBar1.Maximum = 2 * this.dgvSelectedUsers.RowCount;
                        while (num2 < this.dgvSelectedUsers.Rows.Count)
                        {
                            str2 = "";
                            while (num2 < this.dgvSelectedUsers.Rows.Count)
                            {
                                str2 = str2 + ((DataView) this.dgvSelectedUsers.DataSource)[num2]["f_ConsumerID"] + ",";
                                num2++;
                                if (str2.Length > num)
                                {
                                    break;
                                }
                            }
                            str2 = str2 + "0";
                            str = "DELETE FROM  [t_d_Privilege] ";
                            str = (str + " WHERE [f_ConsumerID] IN (") + str2 + " ) ";
                            this.cm.CommandText = str;
                            this.cm.ExecuteNonQuery();
                            int count = this.dgvSelectedUsers.Rows.Count;
                            this.progressBar1.Value = num2;
                            Application.DoEvents();
                        }
                        num2 = 0;
                        while (num2 < this.dgvSelectedUsers.Rows.Count)
                        {
                            str2 = "";
                            while (num2 < this.dgvSelectedUsers.Rows.Count)
                            {
                                str2 = str2 + ((DataView) this.dgvSelectedUsers.DataSource)[num2]["f_ConsumerID"] + ",";
                                num2++;
                                if (str2.Length > num)
                                {
                                    break;
                                }
                            }
                            str2 = str2 + "0";
                            str = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
                            str = (((str + " SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege.f_DoorID,t_d_Privilege.[f_ControlSegID] , t_d_Privilege.[f_ControllerID], t_d_Privilege.[f_DoorNO] ") + " FROM t_d_Privilege, t_b_Consumer " + " WHERE [t_b_Consumer].[f_ConsumerID] IN (") + str2 + " ) ") + " AND (t_d_Privilege.f_ConsumerID)= " + view[0]["f_ConsumerID"];
                            this.cm.CommandText = str;
                            this.cm.ExecuteNonQuery();
                            this.progressBar1.Value = num2 + this.dgvSelectedUsers.Rows.Count;
                            Application.DoEvents();
                        }
                        flag = true;
                        string format = "";
                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount);
                        this.cm.CommandText = str;
                        this.cm.ExecuteNonQuery();
                        this.logOperate(null);
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                    XMessageBox.Show(exception2.Message);
                }
                finally
                {
                    if (this.cm != null)
                    {
                        this.cm.Dispose();
                    }
                    if (this.cn.State != ConnectionState.Closed)
                    {
                        this.cn.Close();
                    }
                    this.dfrmWait1.Hide();
                }
                this.progressBar1.Value = this.progressBar1.Maximum;
                Cursor.Current = Cursors.Default;
                if (flag)
                {
                    wgAppConfig.wgLog(this.Text + CommonStr.strSuccessfully);
                    XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
                    base.DialogResult = DialogResult.OK;
                    this.bEdit = true;
                    base.Close();
                }
                else
                {
                    this.progressBar1.Value = 0;
                    this.bEdit = true;
                    wgAppConfig.wgLog(this.Text + CommonStr.strOperateFailed);
                    XMessageBox.Show(this.Text + CommonStr.strOperateFailed);
                }
            }
        }

        private void btnAddPass_Click_Acc(object sender, EventArgs e)
        {
            OleDbConnection connection = null;
            OleDbCommand command = null;
            if (((this.dgvSelectedUsers.RowCount > 0) && (this.dgvSelectedUsers4Copy.RowCount > 0)) && (XMessageBox.Show(this.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel))
            {
                connection = new OleDbConnection(wgAppConfig.dbConString);
                command = new OleDbCommand("", connection);
                bool flag = false;
                this.timer1.Enabled = true;
                if (this.dgvSelectedUsers.Rows.Count > 0x3e8)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                try
                {
                    string str2 = "";
                    DataView dataSource = (DataView) this.dgvSelectedUsers.DataSource;
                    DataView view = (DataView) this.dgvSelectedUsers4Copy.DataSource;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    try
                    {
                        string str;
                        int num = 0x7d0;
                        int num2 = 0;
                        this.progressBar1.Maximum = 2 * this.dgvSelectedUsers.RowCount;
                        while (num2 < this.dgvSelectedUsers.Rows.Count)
                        {
                            str2 = "";
                            while (num2 < this.dgvSelectedUsers.Rows.Count)
                            {
                                str2 = str2 + ((DataView) this.dgvSelectedUsers.DataSource)[num2]["f_ConsumerID"] + ",";
                                num2++;
                                if (str2.Length > num)
                                {
                                    break;
                                }
                            }
                            str2 = str2 + "0";
                            str = "DELETE FROM  [t_d_Privilege] ";
                            str = (str + " WHERE [f_ConsumerID] IN (") + str2 + " ) ";
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                            int count = this.dgvSelectedUsers.Rows.Count;
                            this.progressBar1.Value = num2;
                            Application.DoEvents();
                        }
                        num2 = 0;
                        while (num2 < this.dgvSelectedUsers.Rows.Count)
                        {
                            str2 = "";
                            while (num2 < this.dgvSelectedUsers.Rows.Count)
                            {
                                str2 = str2 + ((DataView) this.dgvSelectedUsers.DataSource)[num2]["f_ConsumerID"] + ",";
                                num2++;
                                if (str2.Length > num)
                                {
                                    break;
                                }
                            }
                            str2 = str2 + "0";
                            str = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
                            str = (((str + " SELECT t_b_Consumer.f_ConsumerID, t_d_Privilege.f_DoorID,t_d_Privilege.[f_ControlSegID] , t_d_Privilege.[f_ControllerID], t_d_Privilege.[f_DoorNO] ") + " FROM t_d_Privilege, t_b_Consumer " + " WHERE [t_b_Consumer].[f_ConsumerID] IN (") + str2 + " ) ") + " AND (t_d_Privilege.f_ConsumerID)= " + view[0]["f_ConsumerID"];
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                            this.progressBar1.Value = num2 + this.dgvSelectedUsers.Rows.Count;
                            Application.DoEvents();
                        }
                        flag = true;
                        string format = "";
                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount);
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        this.logOperate(null);
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                    XMessageBox.Show(exception2.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    this.dfrmWait1.Hide();
                }
                this.progressBar1.Value = this.progressBar1.Maximum;
                Cursor.Current = Cursors.Default;
                if (flag)
                {
                    wgAppConfig.wgLog(this.Text + CommonStr.strSuccessfully);
                    XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
                    base.DialogResult = DialogResult.OK;
                    this.bEdit = true;
                    base.Close();
                }
                else
                {
                    this.progressBar1.Value = 0;
                    this.bEdit = true;
                    wgAppConfig.wgLog(this.Text + CommonStr.strOperateFailed);
                    XMessageBox.Show(this.Text + CommonStr.strOperateFailed);
                }
            }
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
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                    ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                    ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                }
            }
        }

        private void btnDeleteOneUser4Copy_Click(object sender, EventArgs e)
        {
            if (this.dt4copy.Rows.Count != 0)
            {
                try
                {
                    DataTable table = ((DataView) this.dgvUsers.DataSource).Table;
                    DataRow row = table.NewRow();
                    if (row != null)
                    {
                        DataRow row2 = this.dt4copy.Rows[0];
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            row[i] = row2[i];
                        }
                        row["f_Selected"] = icConsumerShare.iSelectedCurrentNoneMax;
                        table.Rows.Add(row);
                        table.AcceptChanges();
                        this.dt4copy.Rows.Remove(row2);
                        this.dt4copy.AcceptChanges();
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WriteLine(exception.ToString());
                }
            }
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (this.bEdit)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
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
                    dataSource.RowFilter = string.Format("(f_DoorEnabled > 0) AND {0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                if (string.IsNullOrEmpty(this.strGroupFilter))
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
            }
        }

        private void dfrmPrivilegeCopy_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dfrmPrivilegeCopy_KeyDown(object sender, KeyEventArgs e)
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
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void dfrmPrivilegeCopy_Load(object sender, EventArgs e)
        {
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.dataGridViewTextBoxColumn6.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn6.HeaderText);
            this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
            this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            try
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
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers4Copy.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneUser.PerformClick();
        }

        private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
        {
            this.dfrmPrivilegeCopy_KeyDown(this.dgvUsers, e);
        }

        private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.dgvSelectedUsers4Copy.Rows.Count == 0)
            {
                this.btnAddOneUser4Copy.PerformClick();
            }
            else
            {
                this.btnAddOneUser.PerformClick();
            }
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
            this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
            this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            this.dgvSelectedUsers.AutoGenerateColumns = false;
            this.dgvSelectedUsers.DataSource = this.dvSelected;
            this.dt4copy = dtUser.Clone();
            this.dgvSelectedUsers4Copy.AutoGenerateColumns = false;
            this.dgvSelectedUsers4Copy.DataSource = new DataView(this.dt4copy);
            for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dgvUsers.ColumnCount); i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers4Copy.Columns[i].DataPropertyName = this.dt4copy.Columns[i].ColumnName;
            }
            this.cbof_GroupID_SelectedIndexChanged(null, null);
            wgTools.WriteLine("loadUserData End");
            Cursor.Current = Cursors.Default;
        }

        private void logOperate(object sender)
        {
            string str = "";
            for (int i = 0; i <= (Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedUsers.RowCount) - 1); i++)
            {
                str = str + ((DataView) this.dgvSelectedUsers.DataSource)[i]["f_ConsumerName"] + ",";
            }
            if (this.dgvSelectedUsers.RowCount > wgAppConfig.LogEventMaxCount)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "......(", this.dgvSelectedUsers.RowCount, ")" });
            }
            else
            {
                object obj3 = str;
                str = string.Concat(new object[] { obj3, "(", this.dgvSelectedUsers.RowCount, ")" });
            }
            wgAppConfig.wgLog(string.Format("{0}: {1} => {2}", this.Text.Replace("\r\n", ""), ((DataView) this.dgvSelectedUsers4Copy.DataSource)[0]["f_ConsumerName"].ToString(), str), EventLogEntryType.Information, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.bStarting)
                {
                    if ((this.progressBar1.Value != 0) && (this.progressBar1.Value != this.progressBar1.Maximum))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }
                else if (this.dgvUsers.DataSource == null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
                else
                {
                    this.timer1.Enabled = false;
                    Cursor.Current = Cursors.Default;
                    this.lblWait.Visible = false;
                    this.btnAddAllUsers.Enabled = true;
                    this.btnAddOneUser.Enabled = true;
                    this.btnAddOneUser4Copy.Enabled = true;
                    this.btnAddPass.Enabled = true;
                    this.btnDelAllUsers.Enabled = true;
                    this.btnDeleteOneUser4Copy.Enabled = true;
                    this.btnDelOneUser.Enabled = true;
                    this.cbof_GroupID.Enabled = true;
                    this.bStarting = false;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }
    }
}

