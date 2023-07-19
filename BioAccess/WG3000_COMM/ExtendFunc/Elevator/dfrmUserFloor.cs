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
    using System.Text;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmUserFloor : frmBioAccess
    {
        private ArrayList arrRecIDOfUserFloor = new ArrayList();
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private bool bEdit;
        private SqlCommand cmd;
        private SqlConnection cn;
        private int[] controlSegIDList = new int[0x100];
        private string[] controlSegNameList = new string[0x100];
        private dfrmFind dfrmFind1;
        private DataTable dt;
        private DataTable dtDoorTmpSelected;
        private DataView dv;
        private DataView dvDoorTmpSelected;
        private DataView dvSelected;
        private DataView dvSelectedControllerID;
        private int lastRecordCurrentCnt;
        private int lastRecordTotalCnt;
        private int m_consumerID;
        private DataTable oldTbPrivilege;
        public string strSqlSelected = "";
        private string strZoneFilter = "";
        private DataTable tbPrivilege;

        public dfrmUserFloor()
        {
            this.InitializeComponent();
        }

        private void btnAddAllDoors_Click(object sender, EventArgs e)
        {
            DataTable table = ((DataView) this.dgvFloors.DataSource).Table;
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
                using (DataView view = new DataView((this.dgvFloors.DataSource as DataView).Table))
                {
                    view.RowFilter = string.Format("  {0} ", this.strZoneFilter);
                    for (int j = 0; j < view.Count; j++)
                    {
                        view[j]["f_Selected"] = 1;
                    }
                }
            }
            this.updateCount();
        }

        private void btnAddOneDoor_Click(object sender, EventArgs e)
        {
            this.selectObject(this.dgvFloors);
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
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                this.dtDoorTmpSelected = ((DataView) this.dgvSelectedDoors.DataSource).Table.Copy();
                this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
                this.dvSelectedControllerID = new DataView(this.dtDoorTmpSelected);
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
                this.dvSelectedControllerID.RowFilter = "f_Selected = 2";
                foreach (DataRowView view in this.dvDoorTmpSelected)
                {
                    this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", view["f_ControllerID"].ToString());
                    if (this.dvSelectedControllerID.Count == wgMjController.GetControllerType(int.Parse(view["f_ControllerSN"].ToString())))
                    {
                        if (list.IndexOf(int.Parse(view["f_ControllerID"].ToString())) < 0)
                        {
                            list.Add(int.Parse(view["f_ControllerID"].ToString()));
                            list2.Add(int.Parse(view["f_ControllerSN"].ToString()));
                        }
                    }
                    else
                    {
                        view["f_Selected"] = 2;
                    }
                }
                this.dvDoorTmpSelected.RowFilter = "f_Selected = 2";
                int num = 0;
                this.cn.Open();
                this.cmd = new SqlCommand("", this.cn);
                this.cmd.CommandTimeout = wgAppConfig.dbCommandTimeout;
                string info = "DELETE FROM  [t_b_UserFloor]    ";
                if (string.IsNullOrEmpty(this.strSqlSelected))
                {
                    object obj2 = info;
                    info = string.Concat(new object[] { obj2, "WHERE [f_ConsumerID] = (", this.consumerID, " ) " });
                }
                else
                {
                    info = info + string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
                }
                if ((this.arrRecIDOfUserFloor.Count > 0) && (this.arrZoneID.Count > 0))
                {
                    StringBuilder builder = new StringBuilder();
                    for (int j = 0; j < this.arrZoneID.Count; j++)
                    {
                        if (builder.Length == 0)
                        {
                            builder.Append(this.arrZoneID[j].ToString());
                        }
                        else
                        {
                            builder.Append("," + this.arrZoneID[j].ToString());
                        }
                    }
                    if (builder.Length > 0)
                    {
                        info = (info + " AND f_FloorID IN ") + " (SELECT f_floorID FROM t_b_Floor, t_b_Controller WHERE t_b_Floor.f_ControllerID = t_b_Controller.f_ControllerID AND t_b_Controller.f_ZoneID IN ( " + builder.ToString() + "))";
                    }
                }
                this.cmd.CommandText = info;
                wgTools.WriteLine(info);
                this.cmd.ExecuteNonQuery();
                wgTools.WriteLine("DELETE FROM  [t_b_UserFloor] End");
                num = 0;
                if (!string.IsNullOrEmpty(this.strSqlSelected))
                {
                    while (num < this.dgvSelectedDoors.Rows.Count)
                    {
                        info = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
                        info = ((((info + " SELECT  f_ConsumerID,  ") + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + " AS f_floorID,") + this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString() + " AS f_ControlSegID,") + this.dgvSelectedDoors.Rows.Count.ToString() + " AS f_MoreFloorNum ") + " from t_b_consumer " + string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
                        this.cmd.CommandText = info;
                        this.cmd.ExecuteNonQuery();
                        num++;
                    }
                }
                else
                {
                    while (num < this.dgvSelectedDoors.Rows.Count)
                    {
                        info = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
                        info = ((((info + " VALUES ( ") + this.consumerID.ToString() + ",") + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + ",") + this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString() + ",") + this.dgvSelectedDoors.Rows.Count.ToString() + ")";
                        this.cmd.CommandText = info;
                        this.cmd.ExecuteNonQuery();
                        num++;
                    }
                }
                wgTools.WriteLine("INSERT INTO [t_b_UserFloor] End");
                if (string.IsNullOrEmpty(this.strSqlSelected))
                {
                    info = ("update [t_b_UserFloor] set f_MoreFloorNum = " + (((this.lastRecordTotalCnt + this.dgvSelectedDoors.RowCount) - this.lastRecordCurrentCnt)).ToString()) + " where  f_ConsumerID =  " + this.consumerID.ToString();
                    this.cmd.CommandText = info;
                    this.cmd.ExecuteNonQuery();
                }
                string format = "";
                if (sender.Equals(this.btnOK))
                {
                    format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                }
                else
                {
                    format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                }
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
            connection = new OleDbConnection(wgAppConfig.dbConString);
            this.dtDoorTmpSelected = ((DataView) this.dgvSelectedDoors.DataSource).Table.Copy();
            this.dvDoorTmpSelected = new DataView(this.dtDoorTmpSelected);
            this.dvSelectedControllerID = new DataView(this.dtDoorTmpSelected);
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            this.dvDoorTmpSelected.RowFilter = "f_Selected > 0";
            this.dvSelectedControllerID.RowFilter = "f_Selected = 2";
            foreach (DataRowView view in this.dvDoorTmpSelected)
            {
                this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", view["f_ControllerID"].ToString());
                if (this.dvSelectedControllerID.Count == wgMjController.GetControllerType(int.Parse(view["f_ControllerSN"].ToString())))
                {
                    if (list.IndexOf(int.Parse(view["f_ControllerID"].ToString())) < 0)
                    {
                        list.Add(int.Parse(view["f_ControllerID"].ToString()));
                        list2.Add(int.Parse(view["f_ControllerSN"].ToString()));
                    }
                }
                else
                {
                    view["f_Selected"] = 2;
                }
            }
            this.dvDoorTmpSelected.RowFilter = "f_Selected = 2";
            int num = 0;
            connection.Open();
            command = new OleDbCommand("", connection);
            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
            string info = "DELETE FROM  [t_b_UserFloor]    ";
            if (string.IsNullOrEmpty(this.strSqlSelected))
            {
                object obj2 = info;
                info = string.Concat(new object[] { obj2, "WHERE [f_ConsumerID] = (", this.consumerID, " ) " });
            }
            else
            {
                info = info + string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
            }
            if ((this.arrRecIDOfUserFloor.Count > 0) && (this.arrZoneID.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                for (int j = 0; j < this.arrZoneID.Count; j++)
                {
                    if (builder.Length == 0)
                    {
                        builder.Append(this.arrZoneID[j].ToString());
                    }
                    else
                    {
                        builder.Append("," + this.arrZoneID[j].ToString());
                    }
                }
                if (builder.Length > 0)
                {
                    info = (info + " AND f_FloorID IN ") + " (SELECT f_floorID FROM t_b_Floor, t_b_Controller WHERE t_b_Floor.f_ControllerID = t_b_Controller.f_ControllerID AND t_b_Controller.f_ZoneID IN ( " + builder.ToString() + "))";
                }
            }
            command.CommandText = info;
            wgTools.WriteLine(info);
            command.ExecuteNonQuery();
            wgTools.WriteLine("DELETE FROM  [t_b_UserFloor] End");
            num = 0;
            if (!string.IsNullOrEmpty(this.strSqlSelected))
            {
                while (num < this.dgvSelectedDoors.Rows.Count)
                {
                    info = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
                    info = ((((info + " SELECT  f_ConsumerID,  ") + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + " AS f_floorID,") + this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString() + " AS f_ControlSegID,") + this.dgvSelectedDoors.Rows.Count.ToString() + " AS f_MoreFloorNum ") + " from t_b_consumer " + string.Format(" WHERE f_ConsumerID IN ({0}) ", this.strSqlSelected);
                    command.CommandText = info;
                    command.ExecuteNonQuery();
                    num++;
                }
            }
            else
            {
                while (num < this.dgvSelectedDoors.Rows.Count)
                {
                    info = "INSERT INTO [t_b_UserFloor] (f_ConsumerID, f_floorID , f_ControlSegID, f_MoreFloorNum)";
                    info = ((((info + " VALUES ( ") + this.consumerID.ToString() + ",") + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + ",") + this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex].ToString() + ",") + this.dgvSelectedDoors.Rows.Count.ToString() + ")";
                    command.CommandText = info;
                    command.ExecuteNonQuery();
                    num++;
                }
            }
            wgTools.WriteLine("INSERT INTO [t_b_UserFloor] End");
            if (string.IsNullOrEmpty(this.strSqlSelected))
            {
                info = ("update [t_b_UserFloor] set f_MoreFloorNum = " + (((this.lastRecordTotalCnt + this.dgvSelectedDoors.RowCount) - this.lastRecordCurrentCnt)).ToString()) + " where  f_ConsumerID =  " + this.consumerID.ToString();
                command.CommandText = info;
                command.ExecuteNonQuery();
            }
            string format = "";
            if (sender.Equals(this.btnOK))
            {
                format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
            }
            else
            {
                format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
            }
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

        private void cbof_ControlSegID_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvFloors.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvFloors.DataSource;
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
                this.label1.Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.loadControlSegData();
                this.loadZoneInfo();
                this.loadDoorData();
                this.loadPrivilegeData();
                this.lastRecordTotalCnt = this.dgvSelectedDoors.RowCount;
                if (this.dgvFloors.DataSource != null)
                {
                    DataView dataSource = (DataView) this.dgvFloors.DataSource;
                    if (((int) this.arrZoneID[0]) != 0)
                    {
                        int num = 0;
                        for (int i = 0; i < this.arrZoneID.Count; i++)
                        {
                            dataSource.RowFilter = " f_ZoneID =" + this.arrZoneID[i];
                            this.strZoneFilter = " f_ZoneID =" + this.arrZoneID[i];
                            num = (int) this.arrZoneID[i];
                            int num1 = (int) this.arrZoneNO[i];
                            int num5 = (int) this.arrZoneNO[i];
                            dataSource.RowFilter = string.Format(" f_ZoneID ={0:d} ", num);
                            this.strZoneFilter = string.Format(" f_ZoneID ={0:d} ", num);
                            if (dataSource.Count > 0)
                            {
                                for (int k = 0; k < dataSource.Count; k++)
                                {
                                    this.arrRecIDOfUserFloor.Add((int) dataSource[k]["f_floorID"]);
                                }
                            }
                        }
                        DataTable table = dataSource.Table;
                        for (int j = table.Rows.Count - 1; j >= 0; j--)
                        {
                            DataRow row = table.Rows[j];
                            if (this.arrRecIDOfUserFloor.IndexOf((int) row["f_floorID"]) < 0)
                            {
                                table.Rows.Remove(row);
                            }
                        }
                        table.AcceptChanges();
                    }
                }
                this.cbof_Zone_SelectedIndexChanged(null, null);
                this.updateCount();
                this.lastRecordCurrentCnt = this.dgvSelectedDoors.RowCount;
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

        private void dfrmUserFloor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmUserFloor_KeyDown(object sender, KeyEventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void loadControlSegData()
        {
            string str;
            this.cbof_ControlSegID.Items.Clear();
            this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
            this.controlSegNameList[0] = CommonStr.strFreeTime;
            this.controlSegIDList[0] = 1;
            int index = 1;
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT ";
                str = (str + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ") + "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID " + "  FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            this.cbof_ControlSegID.Items.Add(reader["f_ControlSegID"]);
                            this.controlSegNameList[index] = (string) reader["f_ControlSegID"];
                            this.controlSegIDList[index] = (int) reader["f_ControlSegIDBak"];
                            index++;
                        }
                        reader.Close();
                    }
                    goto Label_0208;
                }
            }
            str = " SELECT ";
            str = ((str + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, " + "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),  ") + "     ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50), " + "     ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']') ") + "    END AS f_ControlSegID  " + "  FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        this.cbof_ControlSegID.Items.Add(reader2["f_ControlSegID"]);
                        this.controlSegNameList[index] = (string) reader2["f_ControlSegID"];
                        this.controlSegIDList[index] = (int) reader2["f_ControlSegIDBak"];
                        index++;
                    }
                    reader2.Close();
                }
            }
        Label_0208:
            if (this.cbof_ControlSegID.Items.Count > 0)
            {
                this.cbof_ControlSegID.SelectedIndex = 0;
            }
        }

        private void loadDoorData()
        {
            string cmdText = " SELECT a.f_floorID,  c.f_DoorName + '.' + a.f_floorName as f_floorFullName , 0 as f_Selected, b.f_ZoneID, 0 as f_TimeProfile, b.f_ControllerID, b.f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_floor a, t_b_Controller b,t_b_Door c WHERE c.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID and a.f_DoorID = c.f_DoorID " + " ORDER BY  (  c.f_DoorName + '.' + a.f_floorName ) ";
            this.dt = new DataTable();
            this.dv = new DataView(this.dt);
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
            this.dgvFloors.AutoGenerateColumns = false;
            this.dgvFloors.DataSource = this.dv;
            this.dgvSelectedDoors.AutoGenerateColumns = false;
            this.dgvSelectedDoors.DataSource = this.dvSelected;
            for (int i = 0; i < this.dgvFloors.Columns.Count; i++)
            {
                this.dgvFloors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                this.dgvSelectedDoors.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
            }
        }

        private void loadPrivilegeData()
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadPrivilegeData Start");
            string cmdText = " SELECT f_RecID, f_FloorId, f_ControlSegID ";
            cmdText = cmdText + " FROM t_b_UserFloor  WHERE f_ConsumerID=  " + this.m_consumerID.ToString();
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
                    goto Label_00FC;
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
        Label_00FC:
            wgTools.WriteLine("da.Fill End");
            this.dv = new DataView(this.tbPrivilege);
            this.oldTbPrivilege = this.tbPrivilege;
            int num = 1;
            if (this.dv.Count > 0)
            {
                DataTable table = ((DataView) this.dgvFloors.DataSource).Table;
                for (int j = 0; j < this.dv.Count; j++)
                {
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        if (((int) this.dv[j]["f_floorID"]) == ((int) table.Rows[k]["f_floorID"]))
                        {
                            table.Rows[k]["f_Selected"] = 1;
                            num = int.Parse(this.dv[j]["f_ControlSegID"].ToString());
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < this.controlSegIDList.Length; i++)
            {
                if (this.controlSegIDList[i] == num)
                {
                    this.cbof_ControlSegID.SelectedIndex = i;
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
                str2 = str2 + ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_floorFullName"] + ",";
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
            this.lblOptional.Text = this.dgvFloors.RowCount.ToString();
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

