namespace WG3000_COMM.ExtendFunc.Elevator
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

    partial class dfrmFloors : frmBioAccess
    {
        private ArrayList arrControllerID = new ArrayList();
        private ArrayList arrDoorID = new ArrayList();
        private dfrmFind dfrmFind1;
        private DataTable dt;
        private DataTable dtDoors;
        private DataView dv;
        private DataView dvDoors;
        private DataView dvFloorList;
        private string newFloorNameShort = "";

        public dfrmFloors()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (((this.cboElevator.SelectedIndex >= 0) && (this.cboFloorNO.SelectedIndex >= 0)) && !string.IsNullOrEmpty(this.textBox1.Text))
            {
                this.textBox1.Text = this.textBox1.Text.Trim();
                this.dvFloorList.RowFilter = "f_floorName = " + wgTools.PrepareStr(this.textBox1.Text) + " AND f_DoorName = " + wgTools.PrepareStr(this.cboElevator.Text);
                if (this.dvFloorList.Count > 0)
                {
                    XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string str = " INSERT INTO t_b_floor(";
                    if (wgAppConfig.runUpdateSql((((((str + "  f_floorName, f_DoorID, f_ControllerID, f_floorNO ") + ") " + " VALUES ( ") + wgTools.PrepareStr(this.textBox1.Text.Trim()) + " , ") + this.arrDoorID[this.cboElevator.SelectedIndex] + " , ") + this.arrControllerID[this.cboElevator.SelectedIndex] + " , ") + this.cboFloorNO.Text + ") ") > 0)
                    {
                        this.LoadFloorData();
                        try
                        {
                            this.textBox1.Text = ((this.cboFloorNO.Text.Length == 1) ? "_" : "") + this.cboFloorNO.Text + this.newFloorNameShort;
                        }
                        catch (Exception exception)
                        {
                            wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                        }
                        this.textBox1.Focus();
                        this.textBox1.SelectAll();
                    }
                }
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (this.dgvFloorList.SelectedRows.Count <= 0)
            {
                if (this.dgvFloorList.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = this.dgvFloorList.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = this.dgvFloorList.SelectedRows[0].Index;
            }
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as Button).Text + ":  " + this.dgvFloorList.Rows[rowIndex].Cells[1].Value.ToString();
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    string str = name.strNewName.Trim();
                    if (!string.IsNullOrEmpty(str) && (str != this.dgvFloorList.Rows[rowIndex].Cells[1].Value.ToString()))
                    {
                        this.dvFloorList.RowFilter = "f_floorName = " + wgTools.PrepareStr(str);
                        if (this.dvFloorList.Count > 0)
                        {
                            XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_floor SET f_floorName={0}  WHERE [f_floorId]={1} ", wgTools.PrepareStr(str), this.dgvFloorList.Rows[rowIndex].Cells[0].Value.ToString()));
                            this.LoadFloorData();
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (this.dgvFloorList.SelectedRows.Count <= 0)
            {
                if (this.dgvFloorList.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = this.dgvFloorList.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = this.dgvFloorList.SelectedRows[0].Index;
            }
            string str = string.Format("{0}\r\n{1}:  {2}", this.btnDel.Text, this.dgvFloorList.Columns[1].HeaderText, this.dgvFloorList.Rows[rowIndex].Cells[1].Value.ToString());
            if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                wgAppConfig.runUpdateSql(" DELETE FROM t_b_floor WHERE [f_floorId]= " + this.dgvFloorList.Rows[rowIndex].Cells[0].Value.ToString());
                this.LoadFloorData();
            }
        }

        private void btnRemoteControl_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvFloorList.SelectedRows.Count <= 0)
                {
                    if (this.dgvFloorList.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvFloorList.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvFloorList.SelectedRows[0].Index;
                }
                using (icController controller = new icController())
                {
                    string str;
                    controller.GetInfoFromDBByDoorName(this.dgvFloorList.Rows[rowIndex].Cells["f_DoorName"].Value.ToString());
                    if (controller.RemoteOpenFoorIP(int.Parse(this.dgvFloorList.Rows[rowIndex].Cells["f_floorNO"].Value.ToString()), (uint) icOperator.OperatorID, ulong.MaxValue) > 0)
                    {
                        str = this.btnRemoteControl.Text + " " + this.dgvFloorList.Rows[rowIndex].Cells["f_floorName"].Value.ToString() + " " + CommonStr.strSuccessfully;
                        wgAppConfig.wgLog(str);
                        XMessageBox.Show(str);
                    }
                    else
                    {
                        str = this.btnRemoteControl.Text + "  " + this.dgvFloorList.Rows[rowIndex].Cells["f_floorName"].Value.ToString() + " " + CommonStr.strFailed;
                        wgAppConfig.wgLog(str);
                        XMessageBox.Show(this, str, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnRemoteControlNC_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvFloorList.SelectedRows.Count <= 0)
                {
                    if (this.dgvFloorList.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvFloorList.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvFloorList.SelectedRows[0].Index;
                }
                using (icController controller = new icController())
                {
                    string str;
                    controller.GetInfoFromDBByDoorName(this.dgvFloorList.Rows[rowIndex].Cells["f_DoorName"].Value.ToString());
                    if (controller.RemoteOpenFoorIP(int.Parse(this.dgvFloorList.Rows[rowIndex].Cells["f_floorNO"].Value.ToString()) + 40, (uint) icOperator.OperatorID, ulong.MaxValue) > 0)
                    {
                        str = this.btnRemoteControlNC.Text + " " + this.dgvFloorList.Rows[rowIndex].Cells["f_floorName"].Value.ToString() + " " + CommonStr.strSuccessfully;
                        wgAppConfig.wgLog(str);
                        XMessageBox.Show(str);
                    }
                    else
                    {
                        str = this.btnRemoteControlNC.Text + "  " + this.dgvFloorList.Rows[rowIndex].Cells["f_floorName"].Value.ToString() + " " + CommonStr.strFailed;
                        wgAppConfig.wgLog(str);
                        XMessageBox.Show(this, str, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void cboElevator_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.updateOptionFloors();
        }

        private void dfrmControllerTaskList_Load(object sender, EventArgs e)
        {
            string newValue = "";
            newValue = CommonStr.strFloor;
            this.newFloorNameShort = CommonStr.strFloorShort;
            string str2 = "";
            if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 2)
            {
                newValue = CommonStr.strFloor2;
                str2 = CommonStr.strFloorController2;
                this.newFloorNameShort = CommonStr.strFloorShort2;
            }
            else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 3)
            {
                newValue = CommonStr.strFloor3;
                str2 = CommonStr.strFloorController3;
                this.newFloorNameShort = CommonStr.strFloorShort3;
            }
            if (!string.IsNullOrEmpty(str2))
            {
                this.label1.Text = this.label1.Text.Replace(CommonStr.strFloor, newValue);
                this.label3.Text = this.label3.Text.Replace(CommonStr.strFloor, newValue);
                this.btnChange.Text = this.btnChange.Text.Replace(CommonStr.strFloor, newValue);
                this.f_floorFullName.HeaderText = this.f_floorFullName.HeaderText.Replace(CommonStr.strFloor, newValue);
                this.f_floorNO.HeaderText = this.f_floorNO.HeaderText.Replace(CommonStr.strFloor, newValue);
                this.label2.Text = this.label2.Text.Replace(CommonStr.strFloorController, str2);
                this.f_DoorName.HeaderText = this.f_DoorName.HeaderText.Replace(CommonStr.strFloorController, str2);
            }
            this.loadDoorData();
            this.LoadFloorData();
            if (this.cboFloorNO.Items.Count > 0)
            {
                this.cboFloorNO.SelectedIndex = 0;
            }
            try
            {
                this.textBox1.Text = ((this.cboFloorNO.Text.Length == 1) ? "_" : "") + this.cboFloorNO.Text + this.newFloorNameShort;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            bool bReadOnly = false;
            string funName = "mnuElevator";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnChange.Visible = false;
                this.btnDel.Visible = false;
                this.cboElevator.Enabled = false;
                this.cboFloorNO.Enabled = false;
                this.textBox1.Enabled = false;
            }
        }

        private void dfrmFloors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmFloors_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.KeyValue == 0x51) && e.Control)
                {
                    this.btnRemoteControlNC.Visible = true;
                }
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
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID,a.f_ControllerID ";
            cmdText = (cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ") + " AND b.f_ControllerSN >= 170000000 AND b.f_ControllerSN <= 179999999 " + " ORDER BY  a.f_DoorName ";
            this.dtDoors = new DataTable();
            this.dvDoors = new DataView(this.dtDoors);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtDoors);
                        }
                    }
                    goto Label_00EF;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtDoors);
                    }
                }
            }
        Label_00EF:
            int num1 = this.dtDoors.Rows.Count;
            new icControllerZone().getAllowedControllers(ref this.dtDoors);
            this.cboElevator.Items.Clear();
            if (this.dvDoors.Count > 0)
            {
                for (int i = 0; i < this.dvDoors.Count; i++)
                {
                    this.cboElevator.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
                    this.arrDoorID.Add(this.dvDoors[i]["f_DoorID"]);
                    this.arrControllerID.Add(this.dvDoors[i]["f_ControllerID"]);
                }
            }
            if (this.cboElevator.Items.Count > 0)
            {
                this.cboElevator.SelectedIndex = 0;
                this.textBox1.Focus();
            }
        }

        private void LoadFloorData()
        {
            icControllerZone zone;
            string cmdText = "  SELECT t_b_Floor.f_floorID, t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName as f_floorFullName,  ";
            cmdText = (cmdText + "   t_b_Door.f_DoorName, " + "   t_b_Floor.f_floorNO, t_b_Controller.f_ZoneID, t_b_Floor.f_floorName  ") + "FROM (t_b_Floor LEFT JOIN t_b_Door ON t_b_Floor.f_DoorID = t_b_Door.f_DoorID) LEFT JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID" + " ORDER BY  (  t_b_Door.f_DoorName + '.' + t_b_Floor.f_floorName ) ";
            this.dt = new DataTable();
            this.dv = new DataView(this.dt);
            this.dvFloorList = new DataView(this.dt);
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
                    goto Label_010C;
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
        Label_010C:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dt);
            this.dgvFloorList.AutoGenerateColumns = false;
            this.dgvFloorList.DataSource = this.dv;
            this.updateOptionFloors();
            for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dgvFloorList.ColumnCount); i++)
            {
                this.dgvFloorList.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                this.dgvFloorList.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                this.btnAdd.Enabled = false;
            }
            else
            {
                this.btnAdd.Enabled = true;
            }
        }

        private void updateOptionFloors()
        {
            if (this.dvFloorList != null)
            {
                if (this.cboElevator.SelectedIndex < 0)
                {
                    this.cboFloorNO.Items.Clear();
                }
                else
                {
                    this.dvFloorList.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboElevator.Text);
                    this.cboFloorNO.Items.Clear();
                    for (int i = 1; i <= 40; i++)
                    {
                        this.dvFloorList.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.cboElevator.Text) + "AND f_floorNO = " + i.ToString();
                        if (this.dvFloorList.Count == 0)
                        {
                            this.cboFloorNO.Items.Add(i.ToString());
                        }
                    }
                    if (this.cboFloorNO.Items.Count > 0)
                    {
                        this.cboFloorNO.SelectedIndex = 0;
                    }
                }
            }
        }
    }
}

