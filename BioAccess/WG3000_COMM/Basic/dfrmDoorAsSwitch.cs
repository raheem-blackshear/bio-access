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
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmDoorAsSwitch : frmBioAccess
    {
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private int[] controlSegIDList = new int[0x100];
        private string[] controlSegNameList = new string[0x100];
        private int m_consumerID;
        private string strZoneFilter = "";

        public dfrmDoorAsSwitch()
        {
            this.InitializeComponent();
        }

        private void btnAddAllDoors_Click(object sender, EventArgs e)
        {
            DataTable table = ((DataView) this.dgvDoors.DataSource).Table;
            if ((this.cbof_ZoneID.SelectedIndex <= 0) && (this.cbof_ZoneID.Text == CommonStr.strAllZones))
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (((int) table.Rows[i]["f_Selected"]) != 1)
                    {
                        table.Rows[i]["f_Selected"] = 1;
                    }
                }
            }
            else
            {
                this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
                this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
                for (int j = 0; j < this.dvtmp.Count; j++)
                {
                    this.dvtmp[j]["f_Selected"] = 1;
                }
            }
            this.updateCount();
        }

        private void btnAddOneDoor_Click(object sender, EventArgs e)
        {
            this.selectObject(this.dgvDoors);
            this.updateCount();
        }

        private void btnDelAllDoors_Click(object sender, EventArgs e)
        {
            DataTable table = ((DataView) this.dgvSelectedDoors.DataSource).Table;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i]["f_Selected"] = 0;
            }
            this.updateCount();
        }

        private void btnDelOneDoor_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedDoors);
            this.updateCount();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtDoorTmpSelected = ((DataView) this.dgvSelectedDoors.DataSource).Table.Copy();
                this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
                this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
                string str = "";
                foreach (DataRowView view in this.dvSelected)
                {
                    if (str != "")
                    {
                        str = str + ",";
                    }
                    str = str + view["f_DoorID"].ToString();
                }
                wgAppConfig.runUpdateSql(string.Format(" UPDATE t_a_SystemParam SET f_Notes ={0} Where f_NO = {1}", wgTools.PrepareStr(str), 0x92));
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            finally
            {
                this.dfrmWait1.Hide();
            }
        }

        private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
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
                this.updateCount();
            }
        }

        private void dfrmDoorAsSwitch_Load(object sender, EventArgs e)
        {
            try
            {
                this.loadZoneInfo();
                this.loadDoorData();
                this.loadPrivilegeData();
                this.updateCount();
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        private void dfrmPrivilegeSingle_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnAddOneDoor.PerformClick();
        }

        private void dgvSelectedDoors_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneDoor.PerformClick();
        }

        private void loadControlSegData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadControlSegData_Acc();
            }
            else
            {
                this.cbof_ControlSegID.Items.Clear();
                this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
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
                            this.cbof_ControlSegID.Items.Add(reader["f_ControlSegID"]);
                            this.controlSegNameList[i] = (string) reader["f_ControlSegID"];
                            this.controlSegIDList[i] = (int) reader["f_ControlSegIDBak"];
                        }
                        reader.Close();
                    }
                }
                if (this.cbof_ControlSegID.Items.Count > 0)
                {
                    this.cbof_ControlSegID.SelectedIndex = 0;
                }
            }
        }

        private void loadControlSegData_Acc()
        {
            this.cbof_ControlSegID.Items.Clear();
            this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
            this.controlSegNameList[0] = CommonStr.strFreeTime;
            this.controlSegIDList[0] = 1;
            string cmdText = " SELECT ";
            cmdText = cmdText + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ";
            if (wgAppConfig.IsAccessDB)
            {
                cmdText = cmdText + "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID " + "  FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
            }
            else
            {
                cmdText = ((cmdText + "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),  ") + "     ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50), " + "     ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']') ") + "    END AS f_ControlSegID  " + "  FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
            }
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    for (int i = 1; reader.Read(); i++)
                    {
                        this.cbof_ControlSegID.Items.Add(reader["f_ControlSegID"]);
                        this.controlSegNameList[i] = (string) reader["f_ControlSegID"];
                        this.controlSegIDList[i] = (int) reader["f_ControlSegIDBak"];
                    }
                    reader.Close();
                }
            }
            if (this.cbof_ControlSegID.Items.Count > 0)
            {
                this.cbof_ControlSegID.SelectedIndex = 0;
            }
        }

        private void loadDoorData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadDoorData_Acc();
            }
            else
            {
                string cmdText = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, 1 as f_ControlSegID,' ' as f_ControlSegName, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
                cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            this.dt = new DataTable();
                            this.dv = new DataView(this.dt);
                            this.dvSelected = new DataView(this.dt);
                            adapter.Fill(this.dt);
                            try
                            {
                                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                            }
                            catch (Exception exception)
                            {
                                wgAppConfig.wgLog(exception.ToString());
                            }
                            this.dv.RowFilter = "f_Selected = 0";
                            this.dvSelected.RowFilter = "f_Selected > 0";
                            this.dgvDoors.AutoGenerateColumns = false;
                            this.dgvDoors.DataSource = this.dv;
                            this.dgvSelectedDoors.AutoGenerateColumns = false;
                            this.dgvSelectedDoors.DataSource = this.dvSelected;
                            for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
                            {
                                this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                                this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                            }
                        }
                    }
                }
            }
        }

        private void loadDoorData_Acc()
        {
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, 1 as f_ControlSegID,' ' as f_ControlSegName, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        this.dt = new DataTable();
                        this.dv = new DataView(this.dt);
                        this.dvSelected = new DataView(this.dt);
                        adapter.Fill(this.dt);
                        try
                        {
                            this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                        }
                        catch (Exception exception)
                        {
                            wgAppConfig.wgLog(exception.ToString());
                        }
                        this.dv.RowFilter = "f_Selected = 0";
                        this.dvSelected.RowFilter = "f_Selected > 0";
                        this.dgvDoors.AutoGenerateColumns = false;
                        this.dgvDoors.DataSource = this.dv;
                        this.dgvSelectedDoors.AutoGenerateColumns = false;
                        this.dgvSelectedDoors.DataSource = this.dvSelected;
                        for (int i = 0; i < this.dgvDoors.Columns.Count; i++)
                        {
                            this.dgvDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                            this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                        }
                    }
                }
            }
        }

        private void loadPrivilegeData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadPrivilegeData_Acc();
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("loadPrivilegeData Start");
                string cmdText = string.Format(" SELECT f_Notes FROM t_a_SystemParam Where f_NO = {0}", 0x92);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        string str2 = wgTools.SetObjToStr(command.ExecuteScalar());
                        if (!string.IsNullOrEmpty(str2))
                        {
                            using (DataView view = new DataView(this.dt))
                            {
                                view.RowFilter = string.Format("f_DoorID IN ({0})", str2);
                                for (int i = 0; i < view.Count; i++)
                                {
                                    view[i]["f_Selected"] = 1;
                                }
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void loadPrivilegeData_Acc()
        {
            Cursor.Current = Cursors.WaitCursor;
            string cmdText = string.Format(" SELECT f_Notes FROM t_a_SystemParam Where f_NO = {0}", 0x92);
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    connection.Open();
                    string str2 = wgTools.SetObjToStr(command.ExecuteScalar());
                    if (!string.IsNullOrEmpty(str2))
                    {
                        using (DataView view = new DataView(this.dt))
                        {
                            view.RowFilter = string.Format("f_DoorID IN ({0})", str2);
                            for (int i = 0; i < view.Count; i++)
                            {
                                view[i]["f_Selected"] = 1;
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
            string text = this.Text;
            string str2 = "";
            for (int i = 0; i <= (Math.Min(10, this.dgvSelectedDoors.RowCount) - 1); i++)
            {
                str2 = str2 + ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_DoorName"] + ",";
            }
            if (this.dgvSelectedDoors.RowCount > 10)
            {
                object obj2 = str2;
                str2 = string.Concat(new object[] { obj2, "......(", this.dgvSelectedDoors.RowCount, ")" });
            }
            else
            {
                object obj3 = str2;
                str2 = string.Concat(new object[] { obj3, "(", this.dgvSelectedDoors.RowCount, ")" });
            }
            wgAppConfig.wgLog(string.Format("{0}:[{1} => {2}]:{3} => {4}", new object[] { (sender as Button).Text.Replace("\r\n", ""), 1, this.dgvSelectedDoors.RowCount.ToString(), text, str2 }), EventLogEntryType.Information, null);
        }

        private void selectObject(DataGridView dgv)
        {
            try
            {
                int rowIndex;
                DataRow row;
                if (dgv.SelectedRows.Count <= 0)
                {
                    if (dgv.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgv.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgv.SelectedRows[0].Index;
                }
                DataTable table = ((DataView) dgv.DataSource).Table;
                if (dgv.SelectedRows.Count > 0)
                {
                    int count = dgv.SelectedRows.Count;
                    int[] numArray = new int[count];
                    for (int i = 0; i < dgv.SelectedRows.Count; i++)
                    {
                        numArray[i] = (int) dgv.SelectedRows[i].Cells[0].Value;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        int key = numArray[j];
                        row = table.Rows.Find(key);
                        if (row != null)
                        {
                            row["f_Selected"] = 1;
                            row["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
                            row["f_ControlSegName"] = this.controlSegNameList[this.cbof_ControlSegID.SelectedIndex];
                        }
                    }
                }
                else
                {
                    int num6 = (int) dgv.Rows[rowIndex].Cells[0].Value;
                    row = table.Rows.Find(num6);
                    if (row != null)
                    {
                        row["f_Selected"] = 1;
                        row["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
                        row["f_ControlSegName"] = this.controlSegNameList[this.cbof_ControlSegID.SelectedIndex];
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void updateCount()
        {
            this.lblOptional.Text = this.dgvDoors.RowCount.ToString();
            this.lblSeleted.Text = this.dgvSelectedDoors.RowCount.ToString();
        }

        public int consumerID
        {
            get
            {
                return this.m_consumerID;
            }
            set
            {
                this.m_consumerID = value;
            }
        }
    }
}

