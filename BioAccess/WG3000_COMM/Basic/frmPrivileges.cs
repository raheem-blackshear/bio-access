namespace WG3000_COMM.Basic
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

    public partial class frmPrivileges : Form
    {
        private ArrayList arrController;
        private ArrayList arrControllerAllEnabled;
        private bool bLoadedFinished;
        private SqlCommand cmd;
        private OleDbCommand cmd_Acc;
        private SqlConnection cn;
        private OleDbConnection cn_Acc;
        private int[] controlSegIDList = new int[0x100];
        private string[] controlSegNameList = new string[0x100];
        private SqlDataAdapter da;
        private OleDbDataAdapter da_Acc;
        private string dgvSql;
        private int iAllPrivilegsNum;
        private int iSelectedControllerIndex = -1;
        private int iSelectedControllerIndexLast = -1;
        private int MaxRecord = 0x3e8;
        private int recIdMax;
        private int startRecordIndex;
        private string strAllPrivilegsNum = "";
        private string strSqlAllPrivileg = " SELECT  t_d_Privilege.f_PrivilegeRecID,d.f_DoorName, c.f_ConsumerNO, c.f_ConsumerName, c.f_CardNO,  t_d_Privilege.f_ControlSegID,' ' as  f_ControlSegName, t_d_Privilege.f_ConsumerID  FROM ((t_d_Privilege  INNER JOIN t_b_Consumer c ON t_d_Privilege.f_ConsumerID=c.f_ConsumerID)   INNER JOIN t_b_Door d ON t_d_Privilege.f_DoorID=d.f_DoorID) ";

        public frmPrivileges()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            int startIndex = (int) ((object[]) e.Argument)[0];
            int maxRecords = (int) ((object[]) e.Argument)[1];
            string strSqlpar = (string) ((object[]) e.Argument)[2];
            e.Result = this.loadData(startIndex, maxRecords, strSqlpar);
            if (worker.CancellationPending)
            {
                wgTools.WriteLine("bw.CancellationPending");
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
                    if ((this.iSelectedControllerIndex + 1) < this.arrController.Count)
                    {
                        this.iSelectedControllerIndex++;
                        this.startRecordIndex = 0;
                    }
                    else
                    {
                        this.bLoadedFinished = true;
                    }
                }
                this.fillDgv(e.Result as DataView);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvPrivileges.Rows.Count.ToString() + this.strAllPrivilegsNum + (this.bLoadedFinished ? "#" : "..."));
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.cboDoor.SelectedIndex = -1;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (this.backgroundWorker1.IsBusy)
            {
                flag = true;
                this.backgroundWorker1.CancelAsync();
            }
            using (dfrmPrivilege privilege = new dfrmPrivilege())
            {
                if (privilege.ShowDialog(this) != DialogResult.OK)
                {
                    if (flag)
                    {
                        this.userControlFind1.btnQuery.PerformClick();
                    }
                    return;
                }
            }
            icPrivilegeShare.setNeedRefresh();
            this.userControlFind1.btnQuery.PerformClick();
        }

        private void btnEditSinglePrivilege_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                DataGridView dgvPrivileges = this.dgvPrivileges;
                if (dgvPrivileges.SelectedRows.Count <= 0)
                {
                    if (dgvPrivileges.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgvPrivileges.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgvPrivileges.SelectedRows[0].Index;
                }
                bool flag = false;
                if (this.backgroundWorker1.IsBusy)
                {
                    flag = true;
                    this.backgroundWorker1.CancelAsync();
                }
                using (dfrmPrivilegeSingle num2 = new dfrmPrivilegeSingle())
                {
                    num2.consumerID = int.Parse(dgvPrivileges.Rows[rowIndex].Cells[7].Value.ToString());
                    num2.Text = dgvPrivileges.Rows[rowIndex].Cells[2].Value.ToString().Trim() + "." + dgvPrivileges.Rows[rowIndex].Cells[3].Value.ToString().Trim() + " -- " + num2.Text;
                    if (num2.ShowDialog(this) != DialogResult.OK)
                    {
                        if (flag)
                        {
                            this.userControlFind1.btnQuery.PerformClick();
                        }
                        return;
                    }
                }
                icPrivilegeShare.setNeedRefresh();
                this.userControlFind1.btnQuery.PerformClick();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridView dgvPrivileges = this.dgvPrivileges;
            if ((dgvPrivileges.Rows.Count <= 0xffff) && !this.bLoadedFinished)
            {
                using (dfrmWait wait = new dfrmWait())
                {
                    wait.Show();
                    wait.Refresh();
                    while (this.backgroundWorker1.IsBusy)
                    {
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    do
                    {
                        if (((this.iSelectedControllerIndex + 1) < this.arrController.Count) || (this.startRecordIndex <= dgvPrivileges.Rows.Count))
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 0x101d0 - dgvPrivileges.Rows.Count, this.dgvSql });
                            while (this.backgroundWorker1.IsBusy)
                            {
                                Thread.Sleep(500);
                                Application.DoEvents();
                            }
                            this.startRecordIndex = ((this.startRecordIndex + 0x101d0) - dgvPrivileges.Rows.Count) - this.MaxRecord;
                        }
                        else
                        {
                            wgAppRunInfo.raiseAppRunInfoLoadNums(dgvPrivileges.Rows.Count.ToString() + "#");
                            break;
                        }
                    }
                    while (dgvPrivileges.Rows.Count <= 0xffff);
                    wait.Hide();
                }
            }
            wgAppConfig.exportToExcel(dgvPrivileges, this.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvPrivileges, this.Text);
        }

        private void btnPrivilegeCopy_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                if (this.backgroundWorker1.IsBusy)
                {
                    flag = true;
                    this.backgroundWorker1.CancelAsync();
                }
                using (dfrmPrivilegeCopy copy = new dfrmPrivilegeCopy())
                {
                    if (copy.ShowDialog(this) != DialogResult.OK)
                    {
                        if (flag)
                        {
                            this.userControlFind1.btnQuery.PerformClick();
                        }
                        return;
                    }
                }
                icPrivilegeShare.setNeedRefresh();
                this.userControlFind1.btnQuery.PerformClick();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string strBaseInfo = " SELECT  t_d_Privilege.f_PrivilegeRecID,t_b_Door.f_DoorName, t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, t_b_Consumer.f_CardNO,  t_d_Privilege.f_ControlSegID,' ' as  f_ControlSegName, t_b_Consumer.f_ConsumerID ";
            string strsql = wgAppConfig.getSqlFindPrilivege(strBaseInfo, "t_d_Privilege", "", groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
            if (this.cboDoor.Text != "")
            {
                this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoor.Text);
                if (strsql.ToUpper().IndexOf(" WHERE ") > 0)
                {
                    strsql = strsql + " AND t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
                }
                else
                {
                    strsql = strsql + " WHERE t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
                }
                this.arrController.Clear();
                this.arrController.Add(this.dvDoors4Watching[0]["f_ControllerID"]);
            }
            else
            {
                this.arrController.Clear();
                for (int i = 0; i < this.arrControllerAllEnabled.Count; i++)
                {
                    this.arrController.Add(this.arrControllerAllEnabled[i]);
                }
            }
            this.reloadData(strsql);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void cardNOFuzzyQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strNewName = "";
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = sender.ToString();
                name.label1.Text = CommonStr.strCardID;
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    strNewName = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                this.userControlFind1.txtFindCardID.Text = "";
                this.userControlFind1.txtFindName.Text = "";
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string strBaseInfo = " SELECT  t_d_Privilege.f_PrivilegeRecID,t_b_Door.f_DoorName, t_b_Consumer.f_ConsumerNO, t_b_Consumer.f_ConsumerName, t_b_Consumer.f_CardNO,  t_d_Privilege.f_ControlSegID,' ' as  f_ControlSegName, t_b_Consumer.f_ConsumerID ";
                string strsql = wgAppConfig.getSqlFindPrilivege(strBaseInfo, "t_d_Privilege", "", groupMinNO, groupIDOfMinNO, groupMaxNO, findName, findCard, findConsumerID);
                if (this.cboDoor.Text != "")
                {
                    this.dvDoors4Watching.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboDoor.Text);
                    if (strsql.ToUpper().IndexOf(" WHERE ") > 0)
                    {
                        strsql = strsql + " AND t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
                    }
                    else
                    {
                        strsql = strsql + " WHERE t_d_Privilege.f_DoorID =" + this.dvDoors4Watching[0]["f_DoorID"].ToString();
                    }
                    this.arrController.Clear();
                    this.arrController.Add(this.dvDoors4Watching[0]["f_ControllerID"]);
                }
                else
                {
                    this.arrController.Clear();
                    for (int i = 0; i < this.arrControllerAllEnabled.Count; i++)
                    {
                        this.arrController.Add(this.arrControllerAllEnabled[i]);
                    }
                }
                string str5 = " ( 1>0 ) ";
                if (strNewName.IndexOf("%") < 0)
                {
                    strNewName = string.Format("%{0}%", strNewName);
                }
                if (wgAppConfig.IsAccessDB)
                {
                    str5 = str5 + string.Format(" AND CSTR(f_CardNO) like {0} ", wgTools.PrepareStr(strNewName));
                }
                else
                {
                    str5 = str5 + string.Format(" AND f_CardNO like {0} ", wgTools.PrepareStr(strNewName));
                }
                if (strsql.ToUpper().IndexOf("WHERE") > 0)
                {
                    strsql = strsql + string.Format(" AND {0} ", str5);
                }
                else
                {
                    strsql = strsql + string.Format(" WHERE {0} ", str5);
                }
                this.reloadData(strsql);
            }
        }

        private void cboDoor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.userControlFind1.btnQuery.PerformClick();
            }
        }

        private void dgvPrivileges_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (((e.ColumnIndex >= 0) && (e.ColumnIndex < this.dgvPrivileges.Columns.Count)) && this.dgvPrivileges.Columns[e.ColumnIndex].Name.Equals("f_ControlSegName"))
            {
                switch ((e.Value as string))
                {
                    case null:
                    case " ":
                    {
                        DataGridViewCell cell = this.dgvPrivileges[e.ColumnIndex, e.RowIndex];
                        int num = (int) this.dgvPrivileges[e.ColumnIndex - 1, e.RowIndex].Value;
                        for (int i = 0; i < this.controlSegIDList.Length; i++)
                        {
                            if (this.controlSegIDList[i] == num)
                            {
                                e.Value = this.controlSegNameList[i].ToString();
                                cell.Value = e.Value;
                                return;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void dgvPrivileges_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.bLoadedFinished && (e.ScrollOrientation == ScrollOrientation.VerticalScroll))
            {
                wgTools.WriteLine(e.OldValue.ToString());
                wgTools.WriteLine(e.NewValue.ToString());
                DataGridView dgvPrivileges = this.dgvPrivileges;
                if ((e.NewValue > e.OldValue) && (((e.NewValue + 100) > dgvPrivileges.Rows.Count) || ((e.NewValue + (dgvPrivileges.Rows.Count / 10)) > dgvPrivileges.Rows.Count)))
                {
                    if (((this.iSelectedControllerIndex + 1) < this.arrController.Count) || (this.startRecordIndex <= dgvPrivileges.Rows.Count))
                    {
                        if (!this.backgroundWorker1.IsBusy)
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                        }
                    }
                    else
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvPrivileges.Rows.Count.ToString() + "/" + this.dgvPrivileges.Rows.Count.ToString() + "#");
                    }
                }
            }
        }

        private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvSql == null)
                return;

            while (!this.bLoadedFinished)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.startRecordIndex <= this.dgvPrivileges.Rows.Count)
                {
                    if (!this.backgroundWorker1.IsBusy)
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 100000000, this.dgvSql });
                    }
                }
                else
                {
                    wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvPrivileges.Rows.Count.ToString() + "#");
                }
                Thread.Sleep(0x7d0);
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
            }
            Cursor.Current = Cursors.Default;
        }

        private void fillDgv(DataView dv)
        {
            try
            {
                DataGridView dgvPrivileges = this.dgvPrivileges;
                if (dgvPrivileges.DataSource == null)
                {
                    dgvPrivileges.DataSource = dv;
                    for (int i = 0; i < dv.Table.Columns.Count; i++)
                    {
                        dgvPrivileges.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
                    }
                    wgAppConfig.ReadGVStyle(this, dgvPrivileges);
                    if ((this.startRecordIndex == 0) && (dv.Count >= this.MaxRecord))
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                    }
                }
                else if (dv.Count > 0)
                {
                    int firstDisplayedScrollingRowIndex = dgvPrivileges.FirstDisplayedScrollingRowIndex;
                    DataView dataSource = dgvPrivileges.DataSource as DataView;
                    dataSource.Table.Merge(dv.Table);
                    if (firstDisplayedScrollingRowIndex >= 0)
                    {
                        dgvPrivileges.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmPrivileges_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
        }

        private void frmPrivileges_Load(object sender, EventArgs e)
        {
            this.f_ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.f_ConsumerNO.HeaderText);
            this.loadOperatorPrivilege();
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.userControlFind1.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.loadStyle();
            this.loadControlSegData();
            this.loadDoorData();
            this.loadControllerData();
            icControllerZone zone = new icControllerZone();
            ArrayList arrZoneName = new ArrayList();
            ArrayList arrZoneID = new ArrayList();
            ArrayList arrZoneNO = new ArrayList();
            zone.getZone(ref arrZoneName, ref arrZoneID, ref arrZoneNO);
            if ((arrZoneID.Count > 0) && (((int) arrZoneID[0]) != 0))
            {
                this.btnEditSinglePrivilege.Enabled = false;
            }
            if (!wgAppConfig.getParamValBoolByNO(0x8e))
            {
                this.userControlFind1.btnQuery.PerformClick();
            }
        }

        private void loadControllerData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadControllerData_Acc();
            }
            else
            {
                string cmdText = " SELECT f_ControllerID   FROM [t_b_Controller] WHERE f_Enabled > 0";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        this.arrControllerAllEnabled = new ArrayList();
                        this.arrController = new ArrayList();
                        while (reader.Read())
                        {
                            this.arrControllerAllEnabled.Add(reader[0]);
                            this.arrController.Add(reader[0]);
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void loadControllerData_Acc()
        {
            string cmdText = " SELECT f_ControllerID   FROM [t_b_Controller] WHERE f_Enabled > 0";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    this.arrControllerAllEnabled = new ArrayList();
                    this.arrController = new ArrayList();
                    while (reader.Read())
                    {
                        this.arrControllerAllEnabled.Add(reader[0]);
                        this.arrController.Add(reader[0]);
                    }
                    reader.Close();
                }
            }
        }

        private void loadControlSegData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadControlSegData_Acc();
            }
            else
            {
                this.controlSegNameList[0] = CommonStr.strFreeTime;
                this.controlSegIDList[0] = 1;
                string cmdText = " SELECT ";
                cmdText = ((cmdText + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, " + "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),  ") + "     ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50), " + "     ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']') ") + "    END AS f_ControlSegID  " + "  FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        for (int i = 1; reader.Read(); i++)
                        {
                            this.controlSegNameList[i] = (string) reader["f_ControlSegID"];
                            this.controlSegIDList[i] = (int) reader["f_ControlSegIDBak"];
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void loadControlSegData_Acc()
        {
            this.controlSegNameList[0] = CommonStr.strFreeTime;
            this.controlSegIDList[0] = 1;
            string cmdText = " SELECT ";
            cmdText = (cmdText + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ") + "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID " + "  FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    for (int i = 1; reader.Read(); i++)
                    {
                        this.controlSegNameList[i] = (string) reader["f_ControlSegID"];
                        this.controlSegIDList[i] = (int) reader["f_ControlSegIDBak"];
                    }
                    reader.Close();
                }
            }
        }

        private DataView loadData(int startIndex, int maxRecords, string strSqlpar)
        {
            string str;
            if (wgAppConfig.IsAccessDB)
            {
                return this.loadData_Acc(startIndex, maxRecords, strSqlpar);
            }
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("load Privileges Data Start");
            int iSelectedControllerIndex = this.iSelectedControllerIndex;
            if (this.cn != null)
            {
                this.cn.Dispose();
            }
            this.cn = new SqlConnection(wgAppConfig.dbConString);
            if (this.cmd != null)
            {
                this.cmd.Dispose();
            }
            this.cmd = new SqlCommand("", this.cn);
            if (this.da != null)
            {
                this.da.Dispose();
            }
            this.da = new SqlDataAdapter();
            this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
            this.dtData = new DataTable();
        Label_00B1:
            if ((this.iSelectedControllerIndex == 0) && (this.arrController.Count > 1))
            {
                str = "SELECT TOP 1 f_ControllerID FROM t_d_Privilege order by f_ControllerID";
                this.cmd.CommandText = str;
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                object obj2 = this.cmd.ExecuteScalar();
                if (obj2 == null)
                {
                    this.iSelectedControllerIndex = this.arrController.Count - 1;
                    return new DataView(this.dtData);
                }
                this.iSelectedControllerIndex = this.arrController.IndexOf((int) obj2);
                if (this.iSelectedControllerIndex < 0)
                {
                    this.iSelectedControllerIndex = 0;
                }
            }
            str = strSqlpar;
            if (str.ToUpper().IndexOf("SELECT ") > 0)
            {
                str = string.Format("SELECT TOP {0:d} ", maxRecords) + str.Substring(str.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            }
            if (this.iSelectedControllerIndexLast != this.iSelectedControllerIndex)
            {
                this.recIdMax = -2147483648;
                this.iSelectedControllerIndexLast = this.iSelectedControllerIndex;
            }
            else if (startIndex == 0)
            {
                this.recIdMax = -2147483648;
            }
            else if (str.ToUpper().IndexOf(" WHERE ") > 0)
            {
                str = str + string.Format(" AND f_PrivilegeRecID > {0:d}", this.recIdMax);
            }
            else
            {
                str = str + string.Format(" WHERE f_PrivilegeRecID > {0:d}", this.recIdMax);
            }
            if (str.ToUpper().IndexOf(" WHERE ") > 0)
            {
                if (this.iSelectedControllerIndex >= 0)
                    str = str + string.Format(" AND t_d_Privilege.f_ControllerID = {0:d}", (int) this.arrController[this.iSelectedControllerIndex]);
            }
            else
            {
                if (this.iSelectedControllerIndex >= 0)
                    str = str + string.Format(" WHERE t_d_Privilege.f_ControllerID = {0:d}", (int) this.arrController[this.iSelectedControllerIndex]);
            }
            str = str + " ORDER BY f_PrivilegeRecID ";
            this.cmd.CommandText = str;
            this.da.SelectCommand = this.cmd;
            wgTools.WriteLine("da.Fill start");
            this.da.Fill(this.dtData);
            if ((this.dtData.Rows.Count > 0) && ((iSelectedControllerIndex != 0) || (this.dtData.Rows.Count >= 100)))
            {
                this.recIdMax = int.Parse(this.dtData.Rows[this.dtData.Rows.Count - 1][0].ToString());
                wgTools.WriteLine(string.Format("recIdMax = {0:d}", this.recIdMax));
            }
            else
            {
                this.iSelectedControllerIndex++;
                if (this.iSelectedControllerIndex < this.arrController.Count)
                {
                    goto Label_00B1;
                }
            }
            wgTools.WriteLine("da.Fill End " + startIndex.ToString());
            Cursor.Current = Cursors.Default;
            wgTools.WriteLine("load Privileges Data End");
            return new DataView(this.dtData);
        }

        private DataView loadData_Acc(int startIndex, int maxRecords, string strSqlpar)
        {
            string str;
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("load Privileges Data Start");
            int iSelectedControllerIndex = this.iSelectedControllerIndex;
            if (this.cn_Acc != null)
            {
                this.cn_Acc.Dispose();
            }
            this.cn_Acc = new OleDbConnection(wgAppConfig.dbConString);
            if (this.cmd_Acc != null)
            {
                this.cmd_Acc.Dispose();
            }
            this.cmd_Acc = new OleDbCommand("", this.cn_Acc);
            if (this.da_Acc != null)
            {
                this.da_Acc.Dispose();
            }
            this.da_Acc = new OleDbDataAdapter();
            this.cmd_Acc.CommandTimeout = wgAppConfig.dbCommandTimeout;
            this.dtData = new DataTable();
        Label_00A0:
            if ((this.iSelectedControllerIndex == 0) && (this.arrController.Count > 1))
            {
                str = "SELECT TOP 1 f_ControllerID FROM t_d_Privilege order by f_ControllerID";
                this.cmd_Acc.CommandText = str;
                if (this.cn_Acc.State != ConnectionState.Open)
                {
                    this.cn_Acc.Open();
                }
                object obj2 = this.cmd_Acc.ExecuteScalar();
                if (obj2 == null)
                {
                    this.iSelectedControllerIndex = this.arrController.Count - 1;
                    return new DataView(this.dtData);
                }
                this.iSelectedControllerIndex = this.arrController.IndexOf((int) obj2);
                if (this.iSelectedControllerIndex < 0)
                {
                    this.iSelectedControllerIndex = 0;
                }
            }
            str = strSqlpar;
            if (str.ToUpper().IndexOf("SELECT ") > 0)
            {
                str = string.Format("SELECT TOP {0:d} ", maxRecords) + str.Substring(str.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            }
            if (this.iSelectedControllerIndexLast != this.iSelectedControllerIndex)
            {
                this.recIdMax = -2147483648;
                this.iSelectedControllerIndexLast = this.iSelectedControllerIndex;
            }
            else if (startIndex == 0)
            {
                this.recIdMax = -2147483648;
            }
            else if (str.ToUpper().IndexOf(" WHERE ") > 0)
            {
                str = str + string.Format(" AND f_PrivilegeRecID > {0:d}", this.recIdMax);
            }
            else
            {
                str = str + string.Format(" WHERE f_PrivilegeRecID > {0:d}", this.recIdMax);
            }
            if (str.ToUpper().IndexOf(" WHERE ") > 0)
            {
                str = str + string.Format(" AND t_d_Privilege.f_ControllerID = {0:d}", (int) this.arrController[this.iSelectedControllerIndex]);
            }
            else
            {
                str = str + string.Format(" WHERE t_d_Privilege.f_ControllerID = {0:d}", (int) this.arrController[this.iSelectedControllerIndex]);
            }
            str = str + " ORDER BY f_PrivilegeRecID ";
            this.cmd_Acc.CommandText = str;
            this.da_Acc.SelectCommand = this.cmd_Acc;
            wgTools.WriteLine("da_Acc.Fill start");
            this.da_Acc.Fill(this.dtData);
            if ((this.dtData.Rows.Count > 0) && ((iSelectedControllerIndex != 0) || (this.dtData.Rows.Count >= 100)))
            {
                this.recIdMax = int.Parse(this.dtData.Rows[this.dtData.Rows.Count - 1][0].ToString());
                wgTools.WriteLine(string.Format("recIdMax = {0:d}", this.recIdMax));
            }
            else
            {
                this.iSelectedControllerIndex++;
                if (this.iSelectedControllerIndex < this.arrController.Count)
                {
                    goto Label_00A0;
                }
            }
            wgTools.WriteLine("da_Acc.Fill End " + startIndex.ToString());
            Cursor.Current = Cursors.Default;
            wgTools.WriteLine("load Privileges Data End");
            return new DataView(this.dtData);
        }

        private void loadDoorData()
        {
            icControllerZone zone;
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, a.f_ControllerID , b.f_ZoneID ";
            cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
            this.dt = new DataTable();
            this.dvDoors = new DataView(this.dt);
            this.dvDoors4Watching = new DataView(this.dt);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dt);
                        }
                    }
                    goto Label_00F4;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dt);
                    }
                }
            }
        Label_00F4:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dt);
            try
            {
                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            this.cboDoor.Items.Clear();
            this.cboDoor.Items.Add("");
            if (this.dvDoors.Count > 0)
            {
                for (int i = 0; i < this.dvDoors.Count; i++)
                {
                    this.cboDoor.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
                }
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuPrivilege";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnEditSinglePrivilege.Visible = false;
                this.btnEdit.Visible = false;
                this.btnPrivilegeCopy.Visible = false;
            }
        }

        private void loadStyle()
        {
            this.dgvPrivileges.AutoGenerateColumns = false;
            this.dgvPrivileges.Columns[5].Visible = wgAppConfig.getParamValBoolByNO(0x79);
            this.dgvPrivileges.Columns[6].Visible = wgAppConfig.getParamValBoolByNO(0x79);
            wgAppConfig.ReadGVStyle(this, this.dgvPrivileges);
        }

        private void reloadData(string strsql)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.reloadData_Acc(strsql);
            }
            else if (!this.backgroundWorker1.IsBusy && (this.arrController.Count > 0))
            {
                this.bLoadedFinished = false;
                this.iAllPrivilegsNum = 0;
                this.iSelectedControllerIndex = 0;
                this.strAllPrivilegsNum = "";
                if (this.strSqlAllPrivileg == strsql)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            SqlCommand command = null;
                            {
                                try
                                {
                                    if (wgAppConfig.getSystemParamByNO(0x35) == "1")
                                    {
                                        command = new SqlCommand("SELECT SUM(row_count)  FROM sys.dm_db_partition_stats WHERE object_id = OBJECT_ID('t_d_privilege') AND index_id =1 ", connection);
                                        this.iAllPrivilegsNum = int.Parse(command.ExecuteScalar().ToString());
                                        this.strAllPrivilegsNum = "/" + this.iAllPrivilegsNum;
                                    }
                                    else
                                    {
                                        command = new SqlCommand("select rowcnt from sysindexes where id=object_id(N't_d_Privilege') and name = N'PK_t_d_Privilege'", connection);
                                        this.iAllPrivilegsNum = int.Parse(command.ExecuteScalar().ToString());
                                        if (this.iAllPrivilegsNum <= 0x1e8480)
                                        {
                                            using (SqlCommand command2 = new SqlCommand("select count(1) from t_d_Privilege", connection))
                                            {
                                                this.iAllPrivilegsNum = int.Parse(command2.ExecuteScalar().ToString());
                                                this.strAllPrivilegsNum = "/" + this.iAllPrivilegsNum;
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.strAllPrivilegsNum);
                this.startRecordIndex = 0;
                this.MaxRecord = 0x3e8;
                if (!string.IsNullOrEmpty(strsql))
                {
                    this.dgvSql = strsql;
                }
                this.dgvPrivileges.DataSource = null;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        private void reloadData_Acc(string strsql)
        {
            if (!this.backgroundWorker1.IsBusy && (this.arrController.Count > 0))
            {
                this.bLoadedFinished = false;
                this.iAllPrivilegsNum = 0;
                this.iSelectedControllerIndex = 0;
                this.strAllPrivilegsNum = "";
                if (this.strSqlAllPrivileg == strsql)
                {
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            using (null)
                            {
                                try
                                {
                                    using (OleDbCommand command2 = new OleDbCommand("select count(1) from t_d_Privilege", connection))
                                    {
                                        this.iAllPrivilegsNum = int.Parse(command2.ExecuteScalar().ToString());
                                        this.strAllPrivilegsNum = "/" + this.iAllPrivilegsNum;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                }
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.strAllPrivilegsNum);
                this.startRecordIndex = 0;
                this.MaxRecord = 0x3e8;
                if (!string.IsNullOrEmpty(strsql))
                {
                    this.dgvSql = strsql;
                }
                this.dgvPrivileges.DataSource = null;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                if (this.backgroundWorker1.IsBusy)
                {
                    flag = true;
                    this.backgroundWorker1.CancelAsync();
                }
                using (dfrmPrivilegeDoorCopy copy = new dfrmPrivilegeDoorCopy())
                {
                    if (copy.ShowDialog(this) != DialogResult.OK)
                    {
                        if (flag)
                        {
                            this.userControlFind1.btnQuery.PerformClick();
                        }
                        return;
                    }
                }
                this.userControlFind1.btnQuery.PerformClick();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }
    }
}

