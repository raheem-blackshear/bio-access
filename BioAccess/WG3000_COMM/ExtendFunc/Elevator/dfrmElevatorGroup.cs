namespace WG3000_COMM.ExtendFunc.Elevator
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

    partial class dfrmElevatorGroup : frmBioAccess
    {
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private bool bEdit;
        private SqlCommand cmd;
        private SqlConnection cn;
        private int[] controlSegIDList = new int[0x100];
        private DataTable dt;
        private DataView dv;
        private DataView dvSelected;
        private int m_consumerID;
        private DataTable oldTbPrivilege;
        private string strZoneFilter = "";
        private DataTable tbPrivilege;

        public dfrmElevatorGroup()
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
                        table.Rows[i]["f_Selected"] = this.nudGroupToAdd.Value;
                    }
                }
            }
            else
            {
                using (DataView view = new DataView((this.dgvDoors.DataSource as DataView).Table))
                {
                    view.RowFilter = string.Format("  {0} ", this.strZoneFilter);
                    for (int j = 0; j < view.Count; j++)
                    {
                        view[j]["f_Selected"] = this.nudGroupToAdd.Value;
                    }
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
            }
            else
            {
                this.bEdit = true;
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("btnDelete_Click Start");
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                int num = 0;
                this.cn.Open();
                this.cmd = new SqlCommand("", this.cn);
                this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
                string info = "DELETE FROM  [t_b_ElevatorGroup]    ";
                this.cmd.CommandText = info;
                wgTools.WriteLine(info);
                this.cmd.ExecuteNonQuery();
                wgTools.WriteLine("DELETE FROM  [t_b_ElevatorGroup] End");
                for (num = 0; num < this.dgvSelectedDoors.Rows.Count; num++)
                {
                    info = "INSERT INTO [t_b_ElevatorGroup] ([f_DoorID], [f_ControllerID], [f_ElevatorGroupNO])";
                    info = (((info + " VALUES(  ") + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + " , ") + this.dgvSelectedDoors.Rows[num].Cells[4].Value.ToString() + " , ") + this.dgvSelectedDoors.Rows[num].Cells[1].Value.ToString() + " ) ";
                    this.cmd.CommandText = info;
                    this.cmd.ExecuteNonQuery();
                }
                wgTools.WriteLine("INSERT INTO [t_b_ElevatorGroup] End");
                string format = "";
                format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                {
                    info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                    this.cmd.CommandText = info;
                    this.cmd.ExecuteNonQuery();
                }
                this.cn.Close();
                Cursor.Current = Cursors.Default;
                this.logOperate(this.btnOK);
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            OleDbCommand command = null;
            OleDbConnection connection = null;
            this.bEdit = true;
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("btnDelete_Click Start");
            connection = new OleDbConnection(wgAppConfig.dbConString);
            int num = 0;
            connection.Open();
            command = new OleDbCommand("", connection);
            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
            string info = "DELETE FROM  [t_b_ElevatorGroup]    ";
            command.CommandText = info;
            wgTools.WriteLine(info);
            command.ExecuteNonQuery();
            wgTools.WriteLine("DELETE FROM  [t_b_ElevatorGroup] End");
            for (num = 0; num < this.dgvSelectedDoors.Rows.Count; num++)
            {
                info = "INSERT INTO [t_b_ElevatorGroup] ([f_DoorID], [f_ControllerID], [f_ElevatorGroupNO])";
                info = (((info + " VALUES(  ") + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + " , ") + this.dgvSelectedDoors.Rows[num].Cells[4].Value.ToString() + " , ") + this.dgvSelectedDoors.Rows[num].Cells[1].Value.ToString() + " ) ";
                command.CommandText = info;
                command.ExecuteNonQuery();
            }
            wgTools.WriteLine("INSERT INTO [t_b_ElevatorGroup] End");
            string format = "";
            format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
            for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
            {
                info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                command.CommandText = info;
                command.ExecuteNonQuery();
            }
            connection.Close();
            Cursor.Current = Cursors.Default;
            this.logOperate(this.btnOK);
            base.DialogResult = DialogResult.OK;
            base.Close();
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

        private void dfrmPrivilegeSingle_Load(object sender, EventArgs e)
        {
            try
            {
                this.loadZoneInfo();
                this.loadDoorData();
                this.loadElevatorGroupData();
                this.updateCount();
                bool bReadOnly = false;
                string funName = "mnuElevator";
                if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
                {
                    this.btnAddAllDoors.Visible = false;
                    this.btnAddOneDoor.Visible = false;
                    this.btnDelAllDoors.Visible = false;
                    this.btnDelOneDoor.Visible = false;
                    this.btnOK.Visible = false;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            Cursor.Current = Cursors.WaitCursor;
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
            string cmdText = " SELECT a.f_DoorID,  0 as f_Selected, a.f_DoorName , b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
            cmdText = (cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ") + " AND b.f_ControllerSN >= 170000000 AND b.f_ControllerSN <= 179999999 " + " ORDER BY f_Selected, a.f_DoorName ";
            this.dt = new DataTable();
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
                    goto Label_00DE;
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
        Label_00DE:
            this.dv = new DataView(this.dt);
            this.dv.Sort = "f_Selected, f_DoorName";
            this.dvSelected = new DataView(this.dt);
            this.dvSelected.Sort = "f_Selected, f_DoorName";
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

        private void loadElevatorGroupData()
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadPrivilegeData Start");
            string cmdText = " SELECT [f_DoorID], [f_ControllerID], [f_ElevatorGroupNO] ";
            cmdText = cmdText + " FROM t_b_ElevatorGroup  ";
            this.tbPrivilege = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            adapter.Fill(this.tbPrivilege);
                        }
                    }
                    goto Label_00F1;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        adapter2.Fill(this.tbPrivilege);
                    }
                }
            }
        Label_00F1:
            wgTools.WriteLine("da.Fill End");
            this.dv = new DataView(this.tbPrivilege);
            this.oldTbPrivilege = this.tbPrivilege;
            if (this.dv.Count > 0)
            {
                DataTable table = ((DataView) this.dgvDoors.DataSource).Table;
                for (int i = 0; i < this.dv.Count; i++)
                {
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        if (((int) this.dv[i]["f_DoorID"]) == ((int) table.Rows[j]["f_DoorID"]))
                        {
                            table.Rows[j]["f_Selected"] = this.dv[i]["f_ElevatorGroupNO"];
                            break;
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
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
                            row["f_Selected"] = this.nudGroupToAdd.Value;
                        }
                    }
                }
                else
                {
                    int num6 = (int) dgv.Rows[rowIndex].Cells[0].Value;
                    row = table.Rows.Find(num6);
                    if (row != null)
                    {
                        row["f_Selected"] = this.nudGroupToAdd.Value;
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

