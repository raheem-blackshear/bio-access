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
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPersonsInside : frmBioAccess
    {
        private int[] arrAddr;
        private int[] arrAddrDoorID;
        private string[] arrAddrDoorName;
        private int[] arrAddrOut;
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private dfrmFind dfrmFind1;
        private DataTable dt;
        private DataTable dtReader;
        private DataTable dtUsers;
        private DataView dv;
        private DataView dvDoors;
        private DataView dvDoors4Watching;
        private DataView dvIn;
        private DataView dvOut;
        private DataView dvReader;
        private DataView dvSelected;
        private string strSqlDoorID;
        private string strSqlReaders;

        public dfrmPersonsInside()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            e.Result = this.dealPersonInside();
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
                int result = (int) e.Result;
                if (result == 0)
                {
                    this.dgvEnterIn.DataSource = null;
                    this.dgvOutSide.DataSource = null;
                    this.txtPersons.Text = "0";
                    this.txtPersonsOutSide.Text = "0";
                    XMessageBox.Show(CommonStr.strNoAccessPrivilege4SelectedDoors);
                }
                else if (result > 0)
                {
                    this.dvIn = new DataView(this.dtUsers);
                    this.dvOut = new DataView(this.dtUsers);
                    this.dvIn.RowFilter = "f_bHave =2";
                    this.dvOut.RowFilter = "f_bHave < 2";
                    this.dgvEnterIn.DataSource = this.dvIn;
                    this.dgvOutSide.DataSource = this.dvOut;
                    for (int i = 0; (i < this.dvIn.Table.Columns.Count) && (i < this.dgvEnterIn.ColumnCount); i++)
                    {
                        this.dgvEnterIn.Columns[i].DataPropertyName = this.dvIn.Table.Columns[i].ColumnName;
                    }
                    for (int j = 0; (j < this.dvOut.Table.Columns.Count) && (j < this.dgvOutSide.ColumnCount); j++)
                    {
                        this.dgvOutSide.Columns[j].DataPropertyName = this.dvOut.Table.Columns[j].ColumnName;
                    }
                    if ((this.dvIn.Count == 0) && (this.dvOut.Count > 0))
                    {
                        this.tabControl1.SelectedTab = this.tabPage2;
                    }
                    if ((this.dvIn.Count > 0) && (this.dvOut.Count == 0))
                    {
                        this.tabControl1.SelectedTab = this.tabPage1;
                    }
                    this.txtPersons.Text = this.dvIn.Count.ToString();
                    this.txtPersonsOutSide.Text = this.dvOut.Count.ToString();
                }
                this.btnQuery2.Enabled = true;
                this.timer1.Enabled = true;
                this.NextRefreshTime = DateTime.Now.AddSeconds((double) this.nudCycleSecs.Value);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPage1)
            {
                wgAppConfig.exportToExcel(this.dgvEnterIn, this.tabPage1.Text);
            }
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                wgAppConfig.exportToExcel(this.dgvOutSide, this.tabPage2.Text);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPage1)
            {
                wgAppConfig.printdgv(this.dgvEnterIn, this.tabPage1.Text);
            }
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                wgAppConfig.printdgv(this.dgvOutSide, this.tabPage2.Text);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.strSqlReaders = this.getStrSql();
            if (string.IsNullOrEmpty(this.strSqlReaders))
            {
                XMessageBox.Show(CommonStr.strSelectDoor4Query);
            }
            else
            {
                this.tmStop = DateTime.Now.Date.AddDays(-((double) this.nudDays.Value)).Date;
                if (!this.backgroundWorker1.IsBusy)
                {
                    this.btnQuery2.Enabled = false;
                    this.timer1.Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    this.backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (this.chkListDoors.Items.Count > 0)
            {
                for (int i = 0; i < this.chkListDoors.Items.Count; i++)
                {
                    this.chkListDoors.SetItemChecked(i, true);
                }
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            if (this.chkListDoors.Items.Count > 0)
            {
                for (int i = 0; i < this.chkListDoors.Items.Count; i++)
                {
                    this.chkListDoors.SetItemChecked(i, false);
                }
            }
        }

        private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            if (this.dvDoors != null)
            {
                this.chkListDoors.Items.Clear();
                this.dv = this.dvDoors;
                if ((this.cboZone.SelectedIndex < 0) || ((this.cboZone.SelectedIndex == 0) && (((int) this.arrZoneID[0]) == 0)))
                {
                    this.dv.RowFilter = "";
                    str = "";
                }
                else
                {
                    this.dv.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
                    str = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrZoneID[this.cboZone.SelectedIndex];
                    num = (int) this.arrZoneNO[this.cboZone.SelectedIndex];
                    num3 = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            this.dv.RowFilter = string.Format(" f_ZoneID ={0:d} ", num2);
                            str = string.Format(" f_ZoneID ={0:d} ", num2);
                        }
                        else
                        {
                            this.dv.RowFilter = "";
                            string str2 = "";
                            for (int i = 0; i < this.arrZoneNO.Count; i++)
                            {
                                if ((((int) this.arrZoneNO[i]) <= num3) && (((int) this.arrZoneNO[i]) >= num))
                                {
                                    if (str2 == "")
                                    {
                                        str2 = str2 + string.Format(" f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                    }
                                    else
                                    {
                                        str2 = str2 + string.Format(" OR f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                    }
                                }
                            }
                            this.dv.RowFilter = string.Format("  {0} ", str2);
                            str = string.Format("  {0} ", str2);
                        }
                    }
                    this.dv.RowFilter = string.Format(" {0} ", str);
                }
                this.chkListDoors.Items.Clear();
                if (this.dvDoors.Count > 0)
                {
                    for (int j = 0; j < this.dvDoors.Count; j++)
                    {
                        this.chkListDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[j]["f_DoorName"]));
                        this.arrAddrDoorID[j] = (int) this.dvDoors[j]["f_DoorID"];
                        this.arrAddrDoorName[j] = (string) this.dvDoors[j]["f_DoorName"];
                        this.arrAddr[j] = 0;
                        this.arrAddrOut[j] = 0;
                        if (wgMjController.GetControllerType((int) this.dvDoors[j]["f_ControllerSN"]) == 4)
                        {
                            this.dvReader.RowFilter = "f_ControllerSN = " + this.dvDoors[j]["f_ControllerSN"].ToString() + " AND f_ReaderNO=" + this.dvDoors[j]["f_DoorNO"].ToString();
                            if (this.dvReader.Count > 0)
                            {
                                this.arrAddr[j] = (int) this.dvReader[0]["f_ReaderID"];
                            }
                        }
                        else
                        {
                            string[] strArray = new string[] { "f_ControllerSN = ", this.dvDoors[j]["f_ControllerSN"].ToString(), " AND (f_ReaderNO=", (((((byte) this.dvDoors[j]["f_DoorNO"]) - 1) * 2) + 1).ToString(), " OR f_ReaderNO=", (((((byte) this.dvDoors[j]["f_DoorNO"]) - 1) * 2) + 2).ToString(), " )" };
                            this.dvReader.RowFilter = string.Concat(strArray);
                            if (this.dvReader.Count > 0)
                            {
                                this.arrAddr[j] = (int) this.dvReader[0]["f_ReaderID"];
                                if (this.dvReader.Count > 1)
                                {
                                    this.arrAddrOut[j] = (int) this.dvReader[1]["f_ReaderID"];
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.chkListDoors.Items.Clear();
            }
        }

        private void chkListDoors_KeyDown(object sender, KeyEventArgs e)
        {
            this.dfrmPersonsInside_KeyDown(this.chkListDoors, e);
        }

        private int dealPersonInside()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.dealPersonInside_Acc();
            }
            int count = -1;
            try
            {
                string cmdText = " SELECT  f_ConsumerNO, f_ConsumerName, f_GroupName,  '' as  f_ReadDate, '' as f_DoorName, 0 as f_bHave ";
                cmdText = (cmdText + " , t_b_Consumer.f_GroupID, t_b_Consumer.f_ConsumerID  " + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) ") + " WHERE f_ConsumerID IN (SELECT DISTINCT f_ConsumerID FROM t_d_Privilege WHERE f_DoorID IN (" + this.strSqlDoorID + "))";
                this.dtUsers = new DataTable();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            adapter.Fill(this.dtUsers);
                        }
                    }
                }
                if (this.dtUsers.Rows.Count <= 0)
                {
                    return 0;
                }
                cmdText = " SELECT * FROM t_d_SwipeRecord WHERE 1>0 AND ";
                cmdText = ((cmdText + " f_ReaderID IN (" + this.strSqlReaders + ") ") + 
                    " AND NOT ( f_ConsumerID IS NULL) ") + " AND f_Character >0  " + " ORDER BY f_ReadDate DESC ";
                using (DataView view = new DataView(this.dtUsers))
                {
                    using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                        {
                            command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            connection2.Open();
                            SqlDataReader reader = command2.ExecuteReader();
                            while (reader.Read())
                            {
                                if (((DateTime) reader["f_ReadDate"]) <= DateTime.Now.AddDays(2.0))
                                {
                                    DateTime time2 = (DateTime) reader["f_ReadDate"];
                                    if (time2.Date <= this.tmStop.Date)
                                    {
                                        break;
                                    }
                                    view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"];
                                    if ((view.Count > 0) && (((int) view[0]["f_bHave"]) == 0))
                                    {
                                        view[0]["f_bHave"] = 1;
                                        if (((byte) reader["f_InOut"]) == 1)
                                        {
                                            view[0]["f_bHave"] = 2;
                                            view[0]["f_ReadDate"] = ((DateTime) reader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
                                            for (int i = 0; i < this.arrAddr.Length; i++)
                                            {
                                                if (this.arrAddr[i] == ((int) reader["f_ReaderID"]))
                                                {
                                                    view[0]["f_DoorName"] = this.arrAddrDoorName[i];
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            view[0]["f_ReadDate"] = ((DateTime) reader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
                                            for (int j = 0; j < this.arrAddr.Length; j++)
                                            {
                                                if (this.arrAddrOut[j] == ((int) reader["f_ReaderID"]))
                                                {
                                                    view[0]["f_DoorName"] = this.arrAddrDoorName[j];
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                count = this.dtUsers.Rows.Count;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return count;
        }

        private int dealPersonInside_Acc()
        {
            int count = -1;
            try
            {
                string cmdText = " SELECT  f_ConsumerNO, f_ConsumerName, f_GroupName,  '' as  f_ReadDate, '' as f_DoorName, 0 as f_bHave ";
                cmdText = (cmdText + " , t_b_Consumer.f_GroupID, t_b_Consumer.f_ConsumerID  " + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON ( t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ) ") + " WHERE f_ConsumerID IN (SELECT DISTINCT f_ConsumerID FROM t_d_Privilege WHERE f_DoorID IN (" + this.strSqlDoorID + "))";
                this.dtUsers = new DataTable();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            adapter.Fill(this.dtUsers);
                        }
                    }
                }
                if (this.dtUsers.Rows.Count <= 0)
                {
                    return 0;
                }
                cmdText = " SELECT * FROM t_d_SwipeRecord WHERE 1>0 AND ";
                cmdText = ((cmdText + " f_ReaderID IN (" + this.strSqlReaders + ") ") + 
                    " AND NOT ( f_ConsumerID IS NULL) ") + " AND f_Character >0  " + " ORDER BY f_ReadDate DESC ";
                using (DataView view = new DataView(this.dtUsers))
                {
                    using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command2 = new OleDbCommand(cmdText, connection2))
                        {
                            command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            connection2.Open();
                            OleDbDataReader reader = command2.ExecuteReader();
                            while (reader.Read())
                            {
                                if (((DateTime) reader["f_ReadDate"]) <= DateTime.Now.AddDays(2.0))
                                {
                                    DateTime time2 = (DateTime) reader["f_ReadDate"];
                                    if (time2.Date <= this.tmStop.Date)
                                    {
                                        break;
                                    }
                                    view.RowFilter = "f_ConsumerID = " + reader["f_ConsumerID"];
                                    if ((view.Count > 0) && (((int) view[0]["f_bHave"]) == 0))
                                    {
                                        view[0]["f_bHave"] = 1;
                                        if (((byte) reader["f_InOut"]) == 1)
                                        {
                                            view[0]["f_bHave"] = 2;
                                            view[0]["f_ReadDate"] = ((DateTime) reader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
                                            for (int i = 0; i < this.arrAddr.Length; i++)
                                            {
                                                if (this.arrAddr[i] == ((int) reader["f_ReaderID"]))
                                                {
                                                    view[0]["f_DoorName"] = this.arrAddrDoorName[i];
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            view[0]["f_ReadDate"] = ((DateTime) reader["f_ReadDate"]).ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
                                            for (int j = 0; j < this.arrAddr.Length; j++)
                                            {
                                                if (this.arrAddrOut[j] == ((int) reader["f_ReaderID"]))
                                                {
                                                    view[0]["f_DoorName"] = this.arrAddrDoorName[j];
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            reader.Close();
                        }
                    }
                }
                count = this.dtUsers.Rows.Count;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return count;
        }

        private void dfrmPersonsInside_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmPersonsInside_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmPersonsInside_Load(object sender, EventArgs e)
        {
            this.loadZoneInfo();
            this.tabPage1.BackColor = this.BackColor;
            this.tabPage2.BackColor = this.BackColor;
            this.loadDoorData();
            this.dgvEnterIn.AutoGenerateColumns = false;
            this.dgvOutSide.AutoGenerateColumns = false;
            this.dgvEnterIn.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvOutSide.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.NextRefreshTime = DateTime.Now.AddSeconds((double) this.nudCycleSecs.Value);
            this.dataGridViewTextBoxColumn5.HeaderText = wgAppConfig.ReplaceFloorRomm(this.dataGridViewTextBoxColumn5.HeaderText);
            this.dataGridViewTextBoxColumn14.HeaderText = wgAppConfig.ReplaceFloorRomm(this.dataGridViewTextBoxColumn14.HeaderText);
            this.dataGridViewTextBoxColumn3.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn3.HeaderText);
            this.dataGridViewTextBoxColumn12.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn12.HeaderText);
        }

        private void dgvEnterIn_KeyDown(object sender, KeyEventArgs e)
        {
            this.dfrmPersonsInside_KeyDown(this.dgvEnterIn, e);
        }

        private void dgvOutSide_KeyDown(object sender, KeyEventArgs e)
        {
            this.dfrmPersonsInside_KeyDown(this.dgvOutSide, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string getStrSql()
        {
            string str = "";
            this.strSqlDoorID = "";
            if (this.chkListDoors.CheckedItems.Count != 0)
            {
                for (int i = 0; i < this.chkListDoors.Items.Count; i++)
                {
                    if (this.chkListDoors.GetItemChecked(i))
                    {
                        if (str == "")
                        {
                            this.strSqlDoorID = this.strSqlDoorID + this.arrAddrDoorID[i].ToString();
                            str = str + this.arrAddr[i].ToString();
                        }
                        else
                        {
                            this.strSqlDoorID = this.strSqlDoorID + "," + this.arrAddrDoorID[i].ToString();
                            str = str + "," + this.arrAddr[i].ToString();
                        }
                        if (this.arrAddrOut[i] != 0)
                        {
                            str = str + "," + this.arrAddrOut[i].ToString();
                        }
                    }
                }
            }
            return str;
        }

        private void loadDoorData()
        {
            icControllerZone zone;
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
            this.dt = new DataTable();
            this.dvDoors = new DataView(this.dt);
            this.dvDoors4Watching = new DataView(this.dt);
            this.dvSelected = new DataView(this.dt);
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
                    goto Label_0105;
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
        Label_0105:
            cmdText = " SELECT c.*,b.f_ControllerSN ,  b.f_ZoneID ";
            cmdText = cmdText + " FROM t_b_Controller b, t_b_reader c WHERE c.f_ControllerID = b.f_ControllerID " + " ORDER BY  c.f_ReaderID ";
            this.dtReader = new DataTable();
            this.dvReader = new DataView(this.dtReader);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command3 = new OleDbCommand(cmdText, connection3))
                    {
                        using (OleDbDataAdapter adapter3 = new OleDbDataAdapter(command3))
                        {
                            adapter3.Fill(this.dtReader);
                        }
                    }
                    goto Label_01F4;
                }
            }
            using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command4 = new SqlCommand(cmdText, connection4))
                {
                    using (SqlDataAdapter adapter4 = new SqlDataAdapter(command4))
                    {
                        adapter4.Fill(this.dtReader);
                    }
                }
            }
        Label_01F4:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dt);
            if (this.dvDoors.Count > 0)
            {
                this.arrAddr = new int[this.dvDoors.Count + 1];
                this.arrAddrOut = new int[this.dvDoors.Count + 1];
                this.arrAddrDoorID = new int[this.dvDoors.Count + 1];
                this.arrAddrDoorName = new string[this.dvDoors.Count + 1];
                for (int i = 0; i < this.dvDoors.Count; i++)
                {
                    string item = wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]);
                    this.chkListDoors.Items.Add(item);
                    this.arrAddrDoorID[i] = (int) this.dvDoors[i]["f_DoorID"];
                    this.arrAddrDoorName[i] = (string) this.dvDoors[i]["f_DoorName"];
                    this.arrAddr[i] = 0;
                    this.arrAddrOut[i] = 0;
                    if (wgMjController.GetControllerType((int) this.dvDoors[i]["f_ControllerSN"]) == 4)
                    {
                        this.dvReader.RowFilter = "f_ControllerSN = " + this.dvDoors[i]["f_ControllerSN"].ToString() + " AND f_ReaderNO=" + this.dvDoors[i]["f_DoorNO"].ToString();
                        if (this.dvReader.Count > 0)
                        {
                            this.arrAddr[i] = (int) this.dvReader[0]["f_ReaderID"];
                        }
                    }
                    else
                    {
                        string[] strArray = new string[] { "f_ControllerSN = ", this.dvDoors[i]["f_ControllerSN"].ToString(), " AND (f_ReaderNO=", (((((byte) this.dvDoors[i]["f_DoorNO"]) - 1) * 2) + 1).ToString(), " OR f_ReaderNO=", (((((byte) this.dvDoors[i]["f_DoorNO"]) - 1) * 2) + 2).ToString(), " )" };
                        this.dvReader.RowFilter = string.Concat(strArray);
                        this.dvReader.Sort = "f_ReaderNO ASC";
                        if (this.dvReader.Count > 0)
                        {
                            this.arrAddr[i] = (int) this.dvReader[0]["f_ReaderID"];
                            if (this.dvReader.Count > 1)
                            {
                                this.arrAddrOut[i] = (int) this.dvReader[1]["f_ReaderID"];
                            }
                        }
                    }
                }
            }
        }

        private void loadZoneInfo()
        {
            new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
            int count = this.arrZoneID.Count;
            this.cboZone.Items.Clear();
            for (count = 0; count < this.arrZoneID.Count; count++)
            {
                if ((count == 0) && string.IsNullOrEmpty(this.arrZoneName[count].ToString()))
                {
                    this.cboZone.Items.Add(CommonStr.strAllZones);
                }
                else
                {
                    this.cboZone.Items.Add(this.arrZoneName[count].ToString());
                }
            }
            if (this.cboZone.Items.Count > 0)
            {
                this.cboZone.SelectedIndex = 0;
            }
            bool flag = true;
            this.label25.Visible = flag;
            this.cboZone.Visible = flag;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((this.chkAutoRefresh.Checked && this.btnQuery2.Enabled) && (DateTime.Now > this.NextRefreshTime))
            {
                this.NextRefreshTime = DateTime.Now.AddSeconds((double) this.nudCycleSecs.Value);
                this.btnQuery2.PerformClick();
            }
        }
    }
}

