namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
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

    public partial class dfrmPrivilegeDoorCopy : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private bool bEdit;
        private bool bStarting = true;
        private string strZoneFilter = "";
        private System.Windows.Forms.Timer timer1;

        public dfrmPrivilegeDoorCopy()
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

        private void btnAddAllUsers_Click(object sender, EventArgs e)
        {
            this.dt = ((DataView) this.dgvDoors.DataSource).Table;
            if ((this.cbof_ZoneID.SelectedIndex <= 0) && (this.cbof_ZoneID.Text == CommonStr.strAllZones))
            {
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 1;
                }
            }
            else if (this.cbof_ZoneID.SelectedIndex >= 0)
            {
                this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
                this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
                for (int j = 0; j < this.dvtmp.Count; j++)
                {
                    this.dvtmp[j]["f_Selected"] = 1;
                }
            }
            this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvDoors);
            this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
        }

        private void btnAddOneUser4Copy_Click(object sender, EventArgs e)
        {
            if (this.dt4copy.Rows.Count <= 0)
            {
                try
                {
                    int rowIndex;
                    DataGridView dgvDoors = this.dgvDoors;
                    if (dgvDoors.SelectedRows.Count <= 0)
                    {
                        if (dgvDoors.SelectedCells.Count <= 0)
                        {
                            return;
                        }
                        rowIndex = dgvDoors.SelectedCells[0].RowIndex;
                    }
                    else
                    {
                        rowIndex = dgvDoors.SelectedRows[0].Index;
                    }
                    DataTable table = ((DataView) dgvDoors.DataSource).Table;
                    int key = (int) dgvDoors.Rows[rowIndex].Cells[0].Value;
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
                }
                catch (Exception exception)
                {
                    wgTools.WriteLine(exception.ToString());
                }
            }
        }

        private void btnAddPass_Click(object sender, EventArgs e)
        {
            if (((this.dgvSelectedDoors.RowCount > 0) && (this.dgvSelectedDoors4Copy.RowCount > 0)) && (XMessageBox.Show(this.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel))
            {
                DbConnection connection;
                DbCommand command;
                if (wgAppConfig.IsAccessDB)
                {
                    connection = new OleDbConnection(wgAppConfig.dbConString);
                    command = new OleDbCommand("", connection as OleDbConnection);
                }
                else
                {
                    connection = new SqlConnection(wgAppConfig.dbConString);
                    command = new SqlCommand("", connection as SqlConnection);
                }
                command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                string str = "";
                bool flag = false;
                this.timer1.Enabled = true;
                if (this.dgvSelectedDoors.Rows.Count > 0x3e8)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                try
                {
                    DataView dataSource = (DataView) this.dgvSelectedDoors.DataSource;
                    DataView view = (DataView) this.dgvSelectedDoors4Copy.DataSource;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    try
                    {
                        int num = 0;
                        this.progressBar1.Maximum = 2 * this.dgvSelectedDoors.RowCount;
                        while (num < this.dgvSelectedDoors.Rows.Count)
                        {
                            str = "DELETE FROM  [t_d_Privilege] ";
                            str = str + " WHERE f_DoorID = " + ((DataView) this.dgvSelectedDoors.DataSource)[num]["f_DoorID"];
                            command.CommandText = str;
                            num++;
                            command.ExecuteNonQuery();
                            int count = this.dgvSelectedDoors.Rows.Count;
                            this.progressBar1.Value = num;
                            this.toolStripStatusLabel2.Text = "(x)" + num.ToString() + "." + ((DataView) this.dgvSelectedDoors.DataSource)[num - 1]["f_DoorName"].ToString();
                            Application.DoEvents();
                        }
                        connection.Close();
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        num = 0;
                        int num2 = 0;
                        str = " SELECT count(t_d_Privilege.f_ConsumerID) ";
                        str = str + " FROM t_d_Privilege ";
                        str = str + " WHERE (t_d_Privilege.f_DoorID)= " + view[0]["f_DoorID"];
                        command.CommandText = str;
                        num2 = int.Parse(wgTools.SetObjToStr(command.ExecuteScalar()));
                        while (num < this.dgvSelectedDoors.Rows.Count)
                        {
                            str = "INSERT INTO [t_d_Privilege] ( [f_ConsumerID],[f_DoorID], [f_ControlSegID], [f_ControllerID], [f_DoorNO])";
                            str = str + " SELECT t_d_Privilege.f_ConsumerID, t_b_Door.f_DoorID,t_d_Privilege.[f_ControlSegID] , t_b_Controller.[f_ControllerID], t_b_Door.[f_DoorNO] ";
                            str = str + " FROM t_d_Privilege, t_b_Door, t_b_Controller ";
                            str = str + " WHERE t_b_Door.f_DoorID = " + ((DataView) this.dgvSelectedDoors.DataSource)[num]["f_DoorID"];
                            str = str + " AND (t_d_Privilege.f_DoorID)= " + view[0]["f_DoorID"];
                            str = str + " AND (t_b_Controller.f_ControllerID)= (t_b_Door.f_ControllerID ) ";
                            num++;
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                            this.progressBar1.Value = num + this.dgvSelectedDoors.Rows.Count;
                            this.toolStripStatusLabel2.Text = "(+)" + num.ToString() + "." + ((DataView) this.dgvSelectedDoors.DataSource)[num - 1]["f_DoorName"].ToString();
                            Application.DoEvents();
                        }
                        connection.Close();
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        flag = true;
                        string format = "";
                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} ";
                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), num2);
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        this.logOperate(null);
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                        wgAppConfig.wgLog(exception.ToString() + "\r\nstrSql = " + str);
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                    wgAppConfig.wgLog(exception2.ToString() + "\r\nstrSql2 = " + str);
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
            this.dt = ((DataView) this.dgvSelectedDoors.DataSource).Table;
            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i]["f_Selected"] = 0;
            }
            this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
        }

        private void btnDeleteOneUser4Copy_Click(object sender, EventArgs e)
        {
            if (this.dt4copy.Rows.Count != 0)
            {
                try
                {
                    DataTable table = ((DataView) this.dgvDoors.DataSource).Table;
                    DataRow row = table.NewRow();
                    if (row != null)
                    {
                        DataRow row2 = this.dt4copy.Rows[0];
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            row[i] = row2[i];
                        }
                        row["f_Selected"] = 0;
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
            wgAppConfig.deselectObject(this.dgvSelectedDoors);
            this.toolStripStatusLabel1.Text = this.dgvSelectedDoors.Rows.Count.ToString();
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

        private void cbof_ZoneID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvDoors.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvDoors.DataSource;
                if ((this.cbof_ZoneID.SelectedIndex < 0) || ((this.cbof_ZoneID.SelectedIndex == 0) && (((int) this.arrZoneID[0]) == 0)))
                {
                    dataSource.RowFilter = "f_Selected = 0";
                    this.strZoneFilter = "";
                }
                else
                {
                    dataSource.RowFilter = "f_Selected = 0 AND f_ZoneID =" + this.arrZoneID[this.cbof_ZoneID.SelectedIndex];
                    this.strZoneFilter = " f_ZoneID =" + this.arrZoneID[this.cbof_ZoneID.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrZoneID[this.cbof_ZoneID.SelectedIndex];
                    num = (int) this.arrZoneNO[this.cbof_ZoneID.SelectedIndex];
                    num3 = icControllerZone.getZoneChildMaxNo(this.cbof_ZoneID.Text, this.arrZoneName, this.arrZoneNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND f_ZoneID ={0:d} ", num2);
                            this.strZoneFilter = string.Format(" f_ZoneID ={0:d} ", num2);
                        }
                        else
                        {
                            dataSource.RowFilter = "f_Selected = 0 ";
                            string str = "";
                            for (int i = 0; i < this.arrZoneNO.Count; i++)
                            {
                                if ((((int) this.arrZoneNO[i]) <= num3) && (((int) this.arrZoneNO[i]) >= num))
                                {
                                    if (str == "")
                                    {
                                        str = str + string.Format(" f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                    }
                                    else
                                    {
                                        str = str + string.Format(" OR f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                    }
                                }
                            }
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", str);
                            this.strZoneFilter = string.Format("  {0} ", str);
                        }
                    }
                    dataSource.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", this.strZoneFilter);
                }
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
            this.loadZoneInfo();
            string strSql = " SELECT ";
            strSql = (((strSql + " t_b_Door.f_DoorID ") + ", t_b_Controller.f_ControllerSN " + ", t_b_Door.f_DoorNO ") + ", t_b_Door.f_DoorName " + ", t_b_Controller.f_ZoneID  , 0 as f_Selected") + " FROM t_b_Door , t_b_Controller  WHERE t_b_Door.f_DoorEnabled > 0 and t_b_Controller.f_Enabled >0 and t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID " + " ORDER BY t_b_Door.f_DoorName ";
            wgAppConfig.fillDGVData(ref this.dgvDoors, strSql);
            this.dt = (this.dgvDoors.DataSource as DataView).Table;
            try
            {
                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            this.dv = new DataView(this.dt);
            this.dvSelected = new DataView(this.dt);
            this.dt4copy = this.dt.Clone();
            this.dvSelectedNone = new DataView(this.dt4copy);
            this.dv.RowFilter = "f_Selected = 0";
            this.dv.Sort = "f_DoorName";
            this.dvSelected.RowFilter = "f_Selected > 0";
            this.dvSelected.Sort = "f_DoorName";
            this.dgvDoors.AutoGenerateColumns = false;
            this.dgvDoors.DataSource = this.dv;
            this.dgvSelectedDoors.AutoGenerateColumns = false;
            this.dgvSelectedDoors.DataSource = this.dvSelected;
            this.dgvSelectedDoors4Copy.AutoGenerateColumns = false;
            this.dgvSelectedDoors4Copy.DataSource = this.dvSelectedNone;
            for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
            {
                this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                this.dgvSelectedDoors4Copy.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
            }
            this.cbof_ZoneID_SelectedIndexChanged(null, null);
            this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedDoors4Copy.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void dgvDoors_KeyDown(object sender, KeyEventArgs e)
        {
            this.dfrmPrivilegeCopy_KeyDown(this.dgvDoors, e);
        }

        private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.dgvSelectedDoors4Copy.Rows.Count == 0)
            {
                this.btnAddOneUser4Copy.PerformClick();
            }
            else
            {
                this.btnAddOneUser.PerformClick();
            }
        }

        private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneUser.PerformClick();
        }

        private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneUser.PerformClick();
        }

        private DataTable loadUserData4BackWork()
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadUserData Start");
            icConsumerShare.loadUserData();
            return icConsumerShare.getDt();
        }

        private void loadZoneInfo()
        {
            new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
            int count = this.arrZoneID.Count;
            this.cbof_ZoneID.Items.Clear();
            for (count = 0; count < this.arrZoneID.Count; count++)
            {
                if ((count == 0) && string.IsNullOrEmpty(this.arrZoneName[count].ToString()))
                {
                    this.cbof_ZoneID.Items.Add(CommonStr.strAllZones);
                }
                else
                {
                    this.cbof_ZoneID.Items.Add(this.arrZoneName[count].ToString());
                }
            }
            if (this.cbof_ZoneID.Items.Count > 0)
            {
                this.cbof_ZoneID.SelectedIndex = 0;
            }
            bool flag = true;
            this.label25.Visible = flag;
            this.cbof_ZoneID.Visible = flag;
        }

        private void logOperate(object sender)
        {
            string str = "";
            for (int i = 0; i <= (Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedDoors.RowCount) - 1); i++)
            {
                str = str + ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_DoorName"] + ",";
            }
            if (this.dgvSelectedDoors.RowCount > wgAppConfig.LogEventMaxCount)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "......(", this.dgvSelectedDoors.RowCount, ")" });
            }
            else
            {
                object obj3 = str;
                str = string.Concat(new object[] { obj3, "(", this.dgvSelectedDoors.RowCount, ")" });
            }
            wgAppConfig.wgLog(string.Format("{0}: {1} => {2}", this.Text.Replace("\r\n", ""), ((DataView) this.dgvSelectedDoors4Copy.DataSource)[0]["f_DoorName"].ToString(), str), EventLogEntryType.Information, null);
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
                else if (this.dgvDoors.DataSource == null)
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

