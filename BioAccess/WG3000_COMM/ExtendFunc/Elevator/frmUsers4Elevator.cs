namespace WG3000_COMM.ExtendFunc.Elevator
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
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmUsers4Elevator : Form
    {
        private bool bLoadedFinished;
        private int currentRowIndex;
        private dfrmFind dfrmFind1;
        private string dgvSql;
        private DataTable dtUserFloor;
        private DataView dv;
        private DataView dvUserFloor;
        private bool firstShow = true;
        private int MaxRecord = 0x3e8;
        private int recNOMax = 0;
        private int startRecordIndex;
        private DataTable tb;
        private UserControlFind userControlFind1;

        public frmUsers4Elevator()
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
                    this.bLoadedFinished = true;
                }
                this.fillDgv(e.Result as DataView);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
            }
        }

        private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvUsers.SelectedRows.Count > 0)
            {
                using (dfrmUserFloor floor = new dfrmUserFloor())
                {
                    string str = "";
                    for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i++)
                    {
                        int index = this.dgvUsers.SelectedRows[i].Index;
                        int num2 = int.Parse(this.dgvUsers.Rows[index].Cells[0].Value.ToString());
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str + ",";
                        }
                        str = str + num2.ToString();
                    }
                    floor.strSqlSelected = str;
                    floor.Text = string.Format("{0}: [{1}]", sender.ToString(), this.dgvUsers.SelectedRows.Count.ToString());
                    if (floor.ShowDialog(this) == DialogResult.OK)
                    {
                        this.btnQuery_Click(null, null);
                    }
                }
            }
        }

        private void btnAutoAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (dfrmElevatorGroup group = new dfrmElevatorGroup())
                {
                    group.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            using (dfrmFloors floors = new dfrmFloors())
            {
                floors.Text = this.btnBatchUpdate.Text;
                if (floors.ShowDialog(this) == DialogResult.OK)
                {
                    this.reloadUserData("");
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (dfrmElevatorGroup group = new dfrmElevatorGroup())
                {
                    group.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnEditPrivilege_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvUsers.SelectedCells.Count <= 0)
                    {
                        XMessageBox.Show(this, CommonStr.strSelectUser, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvUsers.SelectedRows[0].Index;
                }
                if (this.dgvUsers.Rows.Count > 0)
                {
                    this.currentRowIndex = this.dgvUsers.CurrentCell.RowIndex;
                }
                using (dfrmUserFloor floor = new dfrmUserFloor())
                {
                    floor.consumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells[0].Value.ToString());
                    floor.Text = this.dgvUsers.Rows[rowIndex].Cells[1].Value.ToString().Trim() + "." + this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString().Trim() + " -- " + floor.Text;
                    if (floor.ShowDialog(this) == DialogResult.OK)
                    {
                        this.showUpload();
                        this.btnQuery_Click(null, null);
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcelSpecial(ref this.dgvUsers, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvUsers, this.Text);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.loadUserFloor();
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string strsql = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
            strsql = strsql + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ";
            if (findConsumerID > 0)
            {
                strsql = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName ";
                strsql = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
                strsql = (strsql + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ") + " WHERE  t_b_Consumer.f_ConsumerID = " + findConsumerID.ToString();
            }
            else if (groupMinNO > 0)
            {
                strsql = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName ";
                strsql = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
                if (groupMinNO >= groupMaxNO)
                {
                    strsql = strsql + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  " + string.Format(" AND  t_b_Group.f_GroupID ={0:d} ", groupIDOfMinNO);
                }
                else
                {
                    strsql = (strsql + " FROM t_b_Consumer,t_b_Group  WHERE t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  ") + string.Format(" AND  t_b_Group.f_GroupNO >={0:d} ", groupMinNO) + string.Format(" AND  t_b_Group.f_GroupNO <={0:d} ", groupMaxNO);
                }
                if (findName != "")
                {
                    strsql = strsql + string.Format(" AND f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                else if (findCard > 0L)
                {
                    strsql = strsql + string.Format(" AND f_CardNO ={0:d} ", findCard);
                }
            }
            else if (findName != "")
            {
                strsql = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName ";
                strsql = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
                strsql = strsql + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
            }
            else if (findCard > 0L)
            {
                strsql = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName ";
                strsql = " SELECT    t_b_Consumer.f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName, ' ' as f_FloorNameDesc,' ' as f_ControlSegID,' ' as f_MoreFloorNum, ' ' as f_FloorID  ";
                strsql = strsql + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID " + string.Format(" WHERE f_CardNO ={0:d} ", findCard);
            }
            this.reloadUserData(strsql);
        }

        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (((e.ColumnIndex >= 0) && (e.ColumnIndex < this.dgvUsers.Columns.Count)) && this.dgvUsers.Columns[e.ColumnIndex].Name.Equals("f_FloorNameDesc"))
            {
                switch ((e.Value as string))
                {
                    case null:
                    case " ":
                    {
                        DataGridViewCell cell = this.dgvUsers[e.ColumnIndex, e.RowIndex];
                        string str2 = this.dgvUsers[0, e.RowIndex].Value.ToString();
                        if ((this.dvUserFloor == null) || string.IsNullOrEmpty(str2))
                        {
                            e.Value = "";
                            cell.Value = "";
                            return;
                        }
                        this.dvUserFloor.RowFilter = "f_ConsumerID = " + str2;
                        if (this.dvUserFloor.Count == 0)
                        {
                            e.Value = "";
                            cell.Value = "";
                            return;
                        }
                        if (this.dvUserFloor.Count == 1)
                        {
                            e.Value = this.dvUserFloor[0]["f_floorName"];
                            cell.Value = this.dvUserFloor[0]["f_floorName"];
                            this.dgvUsers[e.ColumnIndex + 1, e.RowIndex].Value = this.dvUserFloor[0]["f_ControlSegID"];
                        }
                        else if (this.dvUserFloor.Count > 1)
                        {
                            if (this.batchUpdateSelectToolStripMenuItem.Enabled)
                            {
                                e.Value = CommonStr.strElevatorMoreFloors + string.Format("({0})", this.dvUserFloor.Count.ToString());
                                cell.Value = CommonStr.strElevatorMoreFloors + string.Format("({0})", this.dvUserFloor.Count.ToString());
                                this.dgvUsers[e.ColumnIndex + 1, e.RowIndex].Value = this.dvUserFloor[0]["f_ControlSegID"];
                            }
                            else
                            {
                                e.Value = CommonStr.strElevatorMoreFloors;
                                cell.Value = CommonStr.strElevatorMoreFloors;
                                this.dgvUsers[e.ColumnIndex + 1, e.RowIndex].Value = this.dvUserFloor[0]["f_ControlSegID"];
                            }
                        }
                        else
                        {
                            e.Value = "";
                            cell.Value = "";
                        }
                        break;
                    }
                }
            }
        }

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            this.btnEditPrivilege.PerformClick();
        }

        private void dgvUsers_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.bLoadedFinished && (e.ScrollOrientation == ScrollOrientation.VerticalScroll))
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

        private void fillDgv(DataView dvUser4Elevator)
        {
            try
            {
                DataGridView dgvUsers = this.dgvUsers;
                if (dgvUsers.DataSource == null)
                {
                    dgvUsers.DataSource = dvUser4Elevator;
                    for (int i = 0; i < dvUser4Elevator.Table.Columns.Count; i++)
                    {
                        dgvUsers.Columns[i].DataPropertyName = dvUser4Elevator.Table.Columns[i].ColumnName;
                        dgvUsers.Columns[i].Name = dvUser4Elevator.Table.Columns[i].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(dgvUsers, "f_BeginYMD", wgTools.DisplayFormat_DateYMD);
                    wgAppConfig.setDisplayFormatDate(dgvUsers, "f_EndYMD", wgTools.DisplayFormat_DateYMD);
                    wgAppConfig.ReadGVStyle(this, dgvUsers);
                    if ((this.startRecordIndex == 0) && (dvUser4Elevator.Count >= this.MaxRecord))
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                    }
                }
                else if (dvUser4Elevator.Count > 0)
                {
                    int firstDisplayedScrollingRowIndex = dgvUsers.FirstDisplayedScrollingRowIndex;
                    DataView dataSource = dgvUsers.DataSource as DataView;
                    dataSource.Table.Merge(dvUser4Elevator.Table);
                    if (firstDisplayedScrollingRowIndex >= 0)
                    {
                        dgvUsers.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    }
                }
                if ((this.dgvUsers.RowCount > 0) && (this.dgvUsers.RowCount > this.currentRowIndex))
                {
                    this.dgvUsers.CurrentCell = this.dgvUsers[1, this.currentRowIndex];
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmUsers_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void frmUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                using (dfrmInputNewName name = new dfrmInputNewName())
                {
                    name.setPasswordChar('*');
                    if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "5678"))
                    {
                        return;
                    }
                    this.funcCtrlShiftQ();
                }
            }
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

        private void frmUsers_Load(object sender, EventArgs e)
        {
            if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 2)
            {
                this.btnEditPrivilege.Text = CommonStr.strFloorPrivilege2;
                this.btnBatchUpdate.Text = CommonStr.strFloorConfigure2;
            }
            else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 3)
            {
                this.btnEditPrivilege.Text = CommonStr.strFloorPrivilege3;
                this.btnBatchUpdate.Text = CommonStr.strFloorConfigure3;
            }
            this.ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.ConsumerNO.HeaderText);
            Icon appicon = base.Icon;
            wgAppConfig.GetAppIcon(ref appicon);
            base.Icon = appicon;
            this.Deptname.HeaderText = wgAppConfig.ReplaceFloorRomm(this.Deptname.HeaderText);
            this.loadOperatorPrivilege();
            this.loadUserFloor();
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.loadStyle();
            Cursor.Current = Cursors.WaitCursor;
            this.btnAutoAdd.Visible = wgAppConfig.GetKeyVal("ElevatorGroupVisible") == "1";
            this.dgvUsers.ContextMenuStrip = this.contextMenuStrip1;
            this.userControlFind1.btnQuery.PerformClick();
            icControllerZone zone = new icControllerZone();
            ArrayList arrZoneName = new ArrayList();
            ArrayList arrZoneID = new ArrayList();
            ArrayList arrZoneNO = new ArrayList();
            zone.getZone(ref arrZoneName, ref arrZoneID, ref arrZoneNO);
            if ((arrZoneID.Count > 0) && (((int) arrZoneID[0]) != 0))
            {
                this.batchUpdateSelectToolStripMenuItem.Enabled = false;
            }
        }

        private void frmUsers4Elevator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void funcCtrlShiftQ()
        {
            this.btnAutoAdd.Visible = true;
            wgAppConfig.UpdateKeyVal("ElevatorGroupVisible", "1");
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuConsumers";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly))
            {
                if (bReadOnly)
                {
                    this.btnAutoAdd.Visible = false;
                    this.btnBatchUpdate.Visible = false;
                }
            }
            else
            {
                base.Close();
            }
        }

        private void loadStyle()
        {
            this.dgvUsers.AutoGenerateColumns = false;
            bool flag = wgAppConfig.getParamValBoolByNO(0x79);
            this.dgvUsers.Columns[11].Visible = flag;
            wgAppConfig.ReadGVStyle(this, this.dgvUsers);
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
            this.tb = new DataTable("users");
            this.dv = new DataView(this.tb);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.tb);
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
                        adapter2.Fill(this.tb);
                    }
                }
            }
        Label_0187:
            if (this.tb.Rows.Count > 0)
            {
                this.recNOMax = (int)this.tb.Rows[this.tb.Rows.Count - 1]["f_ConsumerID"];
            }
            wgTools.WriteLine("loadUserData End");
            return this.dv;
        }

        private void loadUserFloor()
        {
            string cmdText = " SELECT  t_b_UserFloor .*,  t_b_Door.f_DoorName + '.' +  t_b_floor.f_floorName as f_floorName  FROM (t_b_UserFloor INNER JOIN t_b_Floor ON t_b_UserFloor.f_floorID = t_b_Floor.f_floorID) LEFT JOIN (t_b_Controller RIGHT JOIN t_b_Door ON t_b_Controller.f_ControllerID = t_b_Door.f_ControllerID) ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID ";
            this.dtUserFloor = new DataTable();
            this.dvUserFloor = new DataView(this.dtUserFloor);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUserFloor);
                        }
                    }
                    return;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtUserFloor);
                    }
                }
            }
        }

        private void reloadUserData(string strsql)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.bLoadedFinished = false;
                this.startRecordIndex = 0;
                this.MaxRecord = 0x3e8;
                if (!string.IsNullOrEmpty(strsql))
                {
                    this.dgvSql = strsql;
                }
                this.dgvUsers.DataSource = null;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        private void showUpload()
        {
            if (this.firstShow)
            {
                this.firstShow = false;
                XMessageBox.Show(CommonStr.strNeedUploadFloor);
            }
        }
    }
}

