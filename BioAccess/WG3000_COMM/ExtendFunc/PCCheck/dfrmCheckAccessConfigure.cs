namespace WG3000_COMM.ExtendFunc.PCCheck
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmCheckAccessConfigure : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private dfrmFind dfrmFind1;
        private DataSet dsCheckAccess = new DataSet();
        private DataTable dt;
        private DataView dv;
        private DataView dvCheckAccess;
        private DataView dvSelected;
        private DataView dvtmp;
        private string strSelectedDoors = "";
        private string strZoneFilter = "";

        public dfrmCheckAccessConfigure()
        {
            this.InitializeComponent();
        }

        private void btnAddAllDoors_Click(object sender, EventArgs e)
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
        }

        private void btnAddOneDoor_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvDoors);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDelAllDoors_Click(object sender, EventArgs e)
        {
            this.dt = ((DataView) this.dgvSelectedDoors.DataSource).Table;
            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i]["f_Selected"] = 0;
            }
        }

        private void btnDelOneDoor_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedDoors);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                DataGridView dgvGroups = this.dgvGroups;
                if (dgvGroups.SelectedRows.Count <= 0)
                {
                    if (dgvGroups.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgvGroups.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgvGroups.SelectedRows[0].Index;
                }
                DataTable table = ((DataView) dgvGroups.DataSource).Table;
                int key = (int) dgvGroups.Rows[rowIndex].Cells[0].Value;
                DataRow row = table.Rows.Find(key);
                if (row != null)
                {
                    using (dfrmCheckAccessSetup setup = new dfrmCheckAccessSetup())
                    {
                        setup.groupname = row["f_GroupName"].ToString();
                        setup.soundfilename = row["f_SoundFileName"].ToString();
                        setup.active = (int) row["f_CheckAccessActive"];
                        setup.morecards = (int) row["f_MoreCards"];
                        if (setup.ShowDialog(this) == DialogResult.OK)
                        {
                            row["f_SoundFileName"] = setup.soundfilename;
                            row["f_CheckAccessActive"] = setup.active;
                            row["f_MoreCards"] = setup.morecards;
                            this.dsCheckAccess.Tables["groups"].AcceptChanges();
                            this.Refresh();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                if (this.dvSelected.Count > 0)
                {
                    for (int i = 0; i < this.dvSelected.Count; i++)
                    {
                        if (i != 0)
                        {
                            str = str + ",";
                        }
                        str = str + string.Format("{0:d}", this.dvSelected[i]["f_DoorID"]);
                    }
                }
                string cmdText = "DELETE from t_b_group4PCCheckAccess";
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            for (int j = 0; j <= (this.dvCheckAccess.Count - 1); j++)
                            {
                                cmdText = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
                                cmdText = cmdText + " VALUES( " + this.dvCheckAccess[j]["f_GroupID"].ToString();
                                cmdText = cmdText + " ," + 0;
                                cmdText = cmdText + " ," + this.dvCheckAccess[j]["f_CheckAccessActive"].ToString();
                                cmdText = cmdText + " ," + this.dvCheckAccess[j]["f_MoreCards"].ToString();
                                cmdText = cmdText + " ," + wgTools.PrepareStr(this.dvCheckAccess[j]["f_SoundFileName"].ToString());
                                cmdText = cmdText + ")";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                            }
                            if (!string.IsNullOrEmpty(this.cbof_GroupID.Text))
                            {
                                cmdText = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
                                cmdText = cmdText + " VALUES( " + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                                cmdText = cmdText + " ," + 1;
                                cmdText = cmdText + " ," + 0;
                                cmdText = cmdText + " ," + 1;
                                cmdText = cmdText + " ," + wgTools.PrepareStr(str);
                                cmdText = cmdText + ")";
                                command.CommandText = cmdText;
                                command.ExecuteNonQuery();
                            }
                        }
                        goto Label_0428;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        command2.ExecuteNonQuery();
                        for (int k = 0; k <= (this.dvCheckAccess.Count - 1); k++)
                        {
                            cmdText = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
                            cmdText = (((((cmdText + " VALUES( " + this.dvCheckAccess[k]["f_GroupID"].ToString()) + " ," + 0) + " ," + this.dvCheckAccess[k]["f_CheckAccessActive"].ToString()) + " ," + this.dvCheckAccess[k]["f_MoreCards"].ToString()) + " ," + wgTools.PrepareStr(this.dvCheckAccess[k]["f_SoundFileName"].ToString())) + ")";
                            command2.CommandText = cmdText;
                            command2.ExecuteNonQuery();
                        }
                        if (!string.IsNullOrEmpty(this.cbof_GroupID.Text))
                        {
                            cmdText = "INSERT INTO t_b_group4PCCheckAccess (f_GroupID,f_GroupType,f_CheckAccessActive,f_MoreCards,f_SoundFileName)";
                            cmdText = (((((cmdText + " VALUES( " + this.arrGroupID[this.cbof_GroupID.SelectedIndex]) + " ," + 1) + " ," + 0) + " ," + 1) + " ," + wgTools.PrepareStr(str)) + ")";
                            command2.CommandText = cmdText;
                            command2.ExecuteNonQuery();
                        }
                    }
                }
            Label_0428:
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            base.Size = new Size(0x328, 620);
            this.btnOption.Enabled = false;
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dvCheckAccess != null)
                {
                    if (string.IsNullOrEmpty(this.cbof_GroupID.Text))
                    {
                        this.dvCheckAccess.RowFilter = "";
                    }
                    else
                    {
                        this.dvCheckAccess.RowFilter = string.Format("f_GroupName <> '{0}'", this.cbof_GroupID.Text);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
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
            }
        }

        private void dfrmCheckAccessConfigure_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmCheckAccessConfigure_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmCheckAccessSetup_Load(object sender, EventArgs e)
        {
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.GroupName.HeaderText = wgAppConfig.ReplaceFloorRomm(this.GroupName.HeaderText);
            this.label1.Text = wgAppConfig.ReplaceFloorRomm(this.label1.Text);
            try
            {
                new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
                for (int i = 0; i < this.arrGroupID.Count; i++)
                {
                    if ((i == 0) && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
                    {
                        this.cbof_GroupID.Items.Add("");
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
                string cmdText = " SELECT a.f_GroupID,a.f_GroupName,b.f_CheckAccessActive,b.f_MoreCards, b.f_SoundFileName,b.f_GroupType from t_b_Group a LEFT JOIN t_b_group4PCCheckAccess b ON a.f_GroupID = b.f_GroupID order by f_GroupName ASC";
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                            {
                                adapter.Fill(this.dsCheckAccess, "groups");
                            }
                        }
                        goto Label_01BB;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                        {
                            adapter2.Fill(this.dsCheckAccess, "groups");
                        }
                    }
                }
            Label_01BB:
                this.dvCheckAccess = new DataView(this.dsCheckAccess.Tables["groups"]);
                for (int j = 0; j <= (this.dvCheckAccess.Count - 1); j++)
                {
                    if (string.IsNullOrEmpty(this.dvCheckAccess[j]["f_GroupType"].ToString()))
                    {
                        this.dvCheckAccess[j]["f_GroupType"] = 0;
                        this.dvCheckAccess[j]["f_CheckAccessActive"] = 0;
                        this.dvCheckAccess[j]["f_MoreCards"] = 1;
                        this.dvCheckAccess[j]["f_SoundFileName"] = "";
                    }
                }
                this.dvCheckAccess.RowFilter = "f_GroupType = 1";
                if ((this.dvCheckAccess.Count > 0) && (this.cbof_GroupID.Items.IndexOf(this.dvCheckAccess[0]["f_GroupName"].ToString()) > 0))
                {
                    this.strSelectedDoors = wgTools.SetObjToStr(this.dvCheckAccess[0]["f_SoundFileName"]);
                    this.dvCheckAccess[0]["f_SoundFileName"] = "";
                    this.cbof_GroupID.Text = this.dvCheckAccess[0]["f_GroupName"].ToString();
                    if (!string.IsNullOrEmpty(this.strSelectedDoors))
                    {
                        this.btnOption.Enabled = false;
                        base.Size = new Size(0x328, 0x25c);
                    }
                }
                try
                {
                    this.dsCheckAccess.Tables["groups"].PrimaryKey = new DataColumn[] { this.dsCheckAccess.Tables["groups"].Columns[0] };
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
                this.cbof_GroupID_SelectedIndexChanged(null, null);
                this.dgvGroups.AutoGenerateColumns = false;
                this.dgvGroups.DataSource = this.dvCheckAccess;
                for (int k = 0; (k < this.dvCheckAccess.Table.Columns.Count) && (k < this.dgvGroups.ColumnCount); k++)
                {
                    this.dgvGroups.Columns[k].DataPropertyName = this.dvCheckAccess.Table.Columns[k].ColumnName;
                }
            }
            catch (Exception exception2)
            {
                wgAppConfig.wgLog(exception2.ToString());
            }
            this.dgvGroups.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.loadZoneInfo();
            this.loadDoorData();
            this.loadPrivilegeData();
            this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
        }

        private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnAddOneDoor.PerformClick();
        }

        private void dgvGroups_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneDoor.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
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
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadPrivilegeData Start");
            if (!string.IsNullOrEmpty(this.strSelectedDoors))
            {
                string[] strArray = this.strSelectedDoors.Split(new char[] { ',' });
                if (strArray.Length > 0)
                {
                    DataTable table = ((DataView) this.dgvDoors.DataSource).Table;
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            if (int.Parse(strArray[i]) == ((int) table.Rows[j]["f_DoorID"]))
                            {
                                table.Rows[j]["f_Selected"] = 1;
                                break;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
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
    }
}

