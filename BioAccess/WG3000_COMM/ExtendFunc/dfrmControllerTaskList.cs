namespace WG3000_COMM.ExtendFunc
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

    public partial class dfrmControllerTaskList : frmBioAccess
    {
        private ArrayList arrDoorID = new ArrayList();
        private dfrmFind dfrmFind1 = new dfrmFind();
        private DataTable dt;
        private DataTable dtDoors;
        private DataView dv;
        private DataView dvDoors;

        public dfrmControllerTaskList()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.Rows.Count >= 400)
            {
                XMessageBox.Show(CommonStr.strTaskListOver, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.dtpBegin.Value.Date > this.dtpEnd.Value.Date)
            {
                XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (((!this.checkBox43.Checked && !this.checkBox44.Checked) && (!this.checkBox45.Checked && !this.checkBox46.Checked)) && ((!this.checkBox47.Checked && !this.checkBox48.Checked) && !this.checkBox49.Checked))
            {
                XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.cboDoors.SelectedIndex >= 0)
            {
                string strSql = " INSERT INTO t_b_ControllerTaskList(f_BeginYMD,[f_EndYMD],  [f_OperateTime] ,";
                strSql = ((((((((((((strSql + "  [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday], [f_DoorID],") + "  [f_DoorControl], [f_Notes]" + ") ") + " VALUES ( " + this.getDateString(this.dtpBegin)) + " , " + this.getDateString(this.dtpEnd)) + " , " + this.getDateString(this.dtpTime)) + " , " + (this.checkBox43.Checked ? "1" : "0")) + " , " + (this.checkBox44.Checked ? "1" : "0")) + " , " + (this.checkBox45.Checked ? "1" : "0")) + " , " + (this.checkBox46.Checked ? "1" : "0")) + " , " + (this.checkBox47.Checked ? "1" : "0")) + " , " + (this.checkBox48.Checked ? "1" : "0")) + " , " + (this.checkBox49.Checked ? "1" : "0")) + " , " + this.arrDoorID[this.cboDoors.SelectedIndex];
                if (this.cboAccessMethod.SelectedIndex < 0)
                    this.cboAccessMethod.SelectedIndex = 0;
                
                // To consider 3 items omitted
                int index = this.cboAccessMethod.SelectedIndex;
                //if (index >= 5) index += 5;

                strSql = ((strSql + " , " + index) + " , " + wgTools.PrepareStr(this.textBox1.Text.Trim())) + " )";
                if (wgAppConfig.runUpdateSql(strSql) > 0)
                {
                    wgAppConfig.wgLog(string.Format("{0} {1} [{2}]", this.Text, this.btnAdd.Text, strSql));
                    this.LoadTaskListData();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int index;
            string str2;
            int rowIndex = 0;
            if (this.dgvTaskList.Rows.Count > 0)
            {
                rowIndex = this.dgvTaskList.CurrentCell.RowIndex;
            }
            if (this.dgvTaskList.SelectedRows.Count <= 0)
            {
                if (this.dgvTaskList.SelectedCells.Count <= 0)
                {
                    return;
                }
                index = this.dgvTaskList.SelectedCells[0].RowIndex;
            }
            else
            {
                index = this.dgvTaskList.SelectedRows[0].Index;
            }
            string str = "";
            if (this.dgvTaskList.SelectedRows.Count > 1)
            {
                int num4 = 0x7fffffff;
                int num5 = 0;
                for (int i = 0; i < this.dgvTaskList.SelectedRows.Count; i++)
                {
                    num4 = 0x7fffffff;
                    for (int j = 0; j < this.dgvTaskList.SelectedRows.Count; j++)
                    {
                        index = this.dgvTaskList.SelectedRows[j].Index;
                        int num3 = int.Parse(this.dgvTaskList.Rows[index].Cells[0].Value.ToString());
                        if ((num3 > num5) && (num3 < num4))
                        {
                            num4 = num3;
                        }
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + ",";
                    }
                    str = str + num4.ToString();
                    num5 = num4;
                }
            }
            if (this.dgvTaskList.SelectedRows.Count <= 1)
            {
                str2 = string.Format("{0}\r\n{1}:  {2}", this.btnDel.Text, this.dgvTaskList.Columns[0].HeaderText, this.dgvTaskList.Rows[index].Cells[0].Value.ToString());
                str2 = string.Format(CommonStr.strAreYouSure + " {0} ?", str2);
            }
            else
            {
                str2 = string.Format("{0}\r\n{1}=  {2}", this.btnDel.Text, CommonStr.strTaskNum, this.dgvTaskList.SelectedRows.Count.ToString());
                str2 = string.Format(CommonStr.strAreYouSure + " {0} ?", str2);
            }
            if (XMessageBox.Show(str2, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string str3;
                if (string.IsNullOrEmpty(str))
                {
                    str3 = " DELETE FROM t_b_ControllerTaskList WHERE [f_Id]= " + this.dgvTaskList.Rows[index].Cells[0].Value.ToString();
                }
                else
                {
                    str3 = string.Format(" DELETE FROM t_b_ControllerTaskList WHERE [f_Id] IN ({0}) ", str);
                }
                wgAppConfig.runUpdateSql(str3);
                wgAppConfig.wgLog(string.Format("{0} {1} [{2}]", this.Text, this.btnDel.Text, str3));
                this.LoadTaskListData();
                if (this.dgvTaskList.RowCount > 0)
                {
                    if (this.dgvTaskList.RowCount > rowIndex)
                    {
                        this.dgvTaskList.CurrentCell = this.dgvTaskList[1, rowIndex];
                    }
                    else
                    {
                        this.dgvTaskList.CurrentCell = this.dgvTaskList[1, this.dgvTaskList.RowCount - 1];
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index;
            int rowIndex = 0;
            if (this.dgvTaskList.Rows.Count > 0)
            {
                rowIndex = this.dgvTaskList.CurrentCell.RowIndex;
            }
            if (this.dgvTaskList.SelectedRows.Count <= 0)
            {
                if (this.dgvTaskList.SelectedCells.Count <= 0)
                {
                    return;
                }
                index = this.dgvTaskList.SelectedCells[0].RowIndex;
            }
            else
            {
                index = this.dgvTaskList.SelectedRows[0].Index;
            }
            string str = "";
            if (this.dgvTaskList.SelectedRows.Count > 1)
            {
                int num4 = 0x7fffffff;
                int num5 = 0;
                for (int i = 0; i < this.dgvTaskList.SelectedRows.Count; i++)
                {
                    num4 = 0x7fffffff;
                    for (int j = 0; j < this.dgvTaskList.SelectedRows.Count; j++)
                    {
                        index = this.dgvTaskList.SelectedRows[j].Index;
                        int num3 = int.Parse(this.dgvTaskList.Rows[index].Cells[0].Value.ToString());
                        if ((num3 > num5) && (num3 < num4))
                        {
                            num4 = num3;
                        }
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str + ",";
                    }
                    str = str + num4.ToString();
                    num5 = num4;
                }
            }
            bool flag = false;
            using (dfrmControllerTask task = new dfrmControllerTask())
            {
                if (this.dgvTaskList.SelectedRows.Count > 1)
                {
                    task.Text = string.Format("{0}: [{1}]", this.btnEdit.Text, this.dgvTaskList.SelectedRows.Count.ToString());
                    task.txtTaskIDs.Text = string.Format("({0})", str);
                }
                else
                {
                    task.txtTaskIDs.Text = this.dgvTaskList.Rows[index].Cells[0].Value.ToString();
                }
                if (task.ShowDialog() == DialogResult.OK)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                this.LoadTaskListData();
            }
            if (this.dgvTaskList.RowCount > 0)
            {
                if (this.dgvTaskList.RowCount > rowIndex)
                {
                    this.dgvTaskList.CurrentCell = this.dgvTaskList[1, rowIndex];
                }
                else
                {
                    this.dgvTaskList.CurrentCell = this.dgvTaskList[1, this.dgvTaskList.RowCount - 1];
                }
            }
        }

        private void dfrmControllerTaskList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmControllerTaskList_KeyDown(object sender, KeyEventArgs e)
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
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmControllerTaskList_Load(object sender, EventArgs e)
        {
            this.dtpBegin.Value = DateTime.Now.Date;
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Format = DateTimePickerFormat.Custom;
            this.dtpTime.Value = DateTime.Parse("00:00:00");
            this.loadDoorData();
            if (this.cboAccessMethod.Items.Count > 0)
                this.cboAccessMethod.SelectedIndex = 0;
            this.LoadTaskListData();
            wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMDWeek);
            this.loadOperatorPrivilege();
        }

        private void dgvTaskList_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.dfrmFind1 != null))
            {
                this.dfrmFind1.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string getDateString(DateTimePicker dtp)
        {
            if (dtp == null)
            {
                return wgTools.PrepareStr("");
            }
            return wgTools.PrepareStr(dtp.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat);
        }

        private void loadDoorData()
        {
            int num;
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
            cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
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
                    goto Label_00E3;
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
        Label_00E3:
            num = this.dtDoors.Rows.Count;
            new icControllerZone().getAllowedControllers(ref this.dtDoors);
            this.cboDoors.Items.Clear();
            if (num == this.dtDoors.Rows.Count)
            {
                this.cboDoors.Items.Add(CommonStr.strAll);
                this.arrDoorID.Add(0);
            }
            if (this.dvDoors.Count > 0)
            {
                for (int i = 0; i < this.dvDoors.Count; i++)
                {
                    this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]));
                    this.arrDoorID.Add(this.dvDoors[i]["f_DoorID"]);
                }
            }
            if (this.cboDoors.Items.Count > 0)
            {
                this.cboDoors.SelectedIndex = 0;
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuTaskList";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnDel.Visible = false;
                this.dgvTaskList.ReadOnly = true;
            }
        }

        private void LoadTaskListData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.LoadTaskListData_Acc();
            }
            else
            {
                int num;
                string cmdText = "  SELECT f_Id,  ";
                cmdText = ((((((cmdText + "   f_BeginYMD, ") + "   f_EndYMD,  " + "  ISNULL(CONVERT(char(5), f_OperateTime,108) , '00:00') AS [f_Time], ") + "  [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday], " + "   [f_Friday], [f_Saturday], [f_Sunday], ") + "  CASE WHEN a.f_DoorID=0 THEN  " + wgTools.PrepareStr(CommonStr.strAll) + " ELSE b.f_DoorName END AS f_AdaptTo, ") + " ' ' AS f_DoorControlDesc, " + " f_Notes, ") + " a.f_DoorID, " + " a.f_DoorControl ") + ", c.f_ZoneID " + " FROM t_b_ControllerTaskList a LEFT JOIN t_b_Door b ON a.f_DoorID = b.f_DoorID  LEFT JOIN  t_b_Controller c on b.f_ControllerID = c.f_ControllerID ";
                this.dt = new DataTable();
                this.dv = new DataView(this.dt);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(this.dt);
                        }
                    }
                }
                new icControllerZone().getAllowedControllers(ref this.dt);
                this.dgvTaskList.AutoGenerateColumns = false;
                this.dgvTaskList.DataSource = this.dv;
                for (num = 0; num < this.dt.Rows.Count; num++)
                {
                    // Consider 3 omitted items
                    int index = (int) this.dt.Rows[num]["f_DoorControl"];
                    //if (index >= 8) index -= 3;

                    if (index < this.cboAccessMethod.Items.Count)
                        this.dt.Rows[num]["f_DoorControlDesc"] = this.cboAccessMethod.Items[index].ToString();
                }
                for (num = 0; (num < this.dv.Table.Columns.Count) && (num < this.dgvTaskList.ColumnCount); num++)
                {
                    this.dgvTaskList.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
                    this.dgvTaskList.Columns[num].Name = this.dv.Table.Columns[num].ColumnName;
                }
                wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_BeginYMD", wgTools.DisplayFormat_DateYMDWeek);
                wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_EndYMD", wgTools.DisplayFormat_DateYMDWeek);
            }
        }

        private void LoadTaskListData_Acc()
        {
            int num;
            string cmdText = "  SELECT f_Id,  ";
            cmdText = ((((((cmdText + "   f_BeginYMD, ") + "   f_EndYMD,  " + "  IIF(f_OperateTime IS NULL , '00:00',  Format([f_OperateTime],'Short Time') ) AS [f_Time], ") + "  [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday], " + "   [f_Friday], [f_Saturday], [f_Sunday], ") + "  IIF ( a.f_DoorID=0 ,  " + wgTools.PrepareStr(CommonStr.strAll) + " , b.f_DoorName ) AS f_AdaptTo, ") + " ' ' AS f_DoorControlDesc, " + " f_Notes, ") + " a.f_DoorID, " + " a.f_DoorControl ") + ", c.f_ZoneID " + " FROM (( t_b_ControllerTaskList a LEFT JOIN t_b_Door b ON a.f_DoorID = b.f_DoorID ) LEFT JOIN  t_b_Controller c on b.f_ControllerID = c.f_ControllerID ) ";
            this.dt = new DataTable();
            this.dv = new DataView(this.dt);
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        adapter.Fill(this.dt);
                    }
                }
            }
            new icControllerZone().getAllowedControllers(ref this.dt);
            this.dgvTaskList.AutoGenerateColumns = false;
            this.dgvTaskList.DataSource = this.dv;
            for (num = 0; num < this.dt.Rows.Count; num++)
            {
                // Consider 3 omitted items
                int index = (int) this.dt.Rows[num]["f_DoorControl"];
                //if (index >= 8) index -= 3;

                if (index < this.cboAccessMethod.Items.Count)
                    this.dt.Rows[num]["f_DoorControlDesc"] = this.cboAccessMethod.Items[index].ToString();
            }
            for (num = 0; (num < this.dv.Table.Columns.Count) && (num < this.dgvTaskList.ColumnCount); num++)
            {
                this.dgvTaskList.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
                this.dgvTaskList.Columns[num].Name = this.dv.Table.Columns[num].ColumnName;
            }
            wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_BeginYMD", wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dgvTaskList, "f_EndYMD", wgTools.DisplayFormat_DateYMDWeek);
        }
    }
}

