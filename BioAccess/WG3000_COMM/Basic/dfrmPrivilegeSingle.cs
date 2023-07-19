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

    public partial class dfrmPrivilegeSingle : frmBioAccess
    {
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private bool bEdit;
        private SqlCommand cm;
        private SqlConnection cn;
        private int[] controlSegIDList = new int[0x100];
        private string[] controlSegNameList = new string[0x100];
        private int m_consumerID;
        private string strZoneFilter = "";

        public dfrmPrivilegeSingle()
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
                        table.Rows[i]["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
                        for (int j = 0; j < this.controlSegIDList.Length; j++)
                        {
                            if (this.controlSegIDList[j] == ((int) table.Rows[i]["f_ControlSegID"]))
                            {
                                table.Rows[i]["f_ControlSegName"] = this.controlSegNameList[j];
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                this.dvtmp = new DataView((this.dgvDoors.DataSource as DataView).Table);
                this.dvtmp.RowFilter = string.Format("  {0} ", this.strZoneFilter);
                for (int k = 0; k < this.dvtmp.Count; k++)
                {
                    this.dvtmp[k]["f_ControlSegID"] = this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
                    for (int m = 0; m < this.controlSegIDList.Length; m++)
                    {
                        if (this.controlSegIDList[m] == ((int) table.Rows[k]["f_ControlSegID"]))
                        {
                            this.dvtmp[k]["f_ControlSegName"] = this.controlSegNameList[m];
                            break;
                        }
                    }
                    this.dvtmp[k]["f_Selected"] = 1;
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
            else if (XMessageBox.Show(string.Format("{0} {1} ({2}) ?", CommonStr.strAreYouSureUpdate, this.Text, CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString()), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                if (this.dgvSelectedDoors.Rows.Count > 100)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                try
                {
                    this.bEdit = true;
                    Cursor.Current = Cursors.WaitCursor;
                    wgTools.WriteLine("btnDelete_Click Start");
                    this.cn = new SqlConnection(wgAppConfig.dbConString);
                    this.cm = new SqlCommand("", this.cn);
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
                    this.cm = new SqlCommand("", this.cn);
                    this.cm.CommandTimeout = wgAppConfig.dbCommandTimeout;
                    string info = "DELETE FROM  [t_d_Privilege]  WHERE  ";
                    object obj2 = info;
                    info = string.Concat(new object[] { obj2, " [f_ConsumerID] = (", this.consumerID, " ) " });
                    this.cm.CommandText = info;
                    wgTools.WriteLine(info);
                    this.cm.ExecuteNonQuery();
                    wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
                    for (num = 0; num < this.dgvSelectedDoors.Rows.Count; num++)
                    {
                        info = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
                        object obj3 = ((info + " SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ") + this.dgvSelectedDoors.Rows[num].Cells[4].Value.ToString() + " AS [f_ControlSegID]  ") + " FROM t_b_Consumer, t_b_Door " + " WHERE  ";
                        info = string.Concat(new object[] { obj3, " [f_ConsumerID] = (", this.consumerID, " ) " }) + " AND  ( t_b_Door.f_DoorID= " + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + ")";
                        this.cm.CommandText = info;
                        this.cm.ExecuteNonQuery();
                    }
                    wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
                    string format = "";
                    format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                    for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                    {
                        info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                        this.cm.CommandText = info;
                        this.cm.ExecuteNonQuery();
                    }
                    this.cn.Close();
                    wgTools.WriteLine("btnDelete_Click End");
                    Cursor.Current = Cursors.Default;
                    this.logOperate(this.btnOKAndUpload);
                    ArrayList list3 = new ArrayList();
                    using (icController controller = new icController())
                    {
                        using (icPrivilege privilege = new icPrivilege())
                        {
                            int num4;
                            for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
                            {
                                num4 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"];
                                if (list3.IndexOf(num4) >= 0)
                                {
                                    continue;
                                }
                                controller.GetInfoFromDBByControllerID(num4);
                                if (controller.GetControllerRunInformationIP() > 0)
                                {
                                    if ((controller.runinfo.regUserCount != 0) || (privilege.ClearAllPrivilegeIP(controller.ControllerSN, controller.IP, controller.PORT) >= 0))
                                    {
                                        goto Label_058F;
                                    }
                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                }
                                else
                                {
                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                }
                                return;
                            Label_058F:
                                if (privilege.AddPrivilegeOfOneCardByDB(num4, this.consumerID) < 0)
                                {
                                    format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                    this.cn.Open();
                                    for (int m = 0; m < this.dgvSelectedDoors.Rows.Count; m++)
                                    {
                                        num4 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[m]["f_ControllerID"];
                                        if (list3.IndexOf(num4) < 0)
                                        {
                                            info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, num4);
                                            this.cm.CommandText = info;
                                            this.cm.ExecuteNonQuery();
                                        }
                                    }
                                    this.cn.Close();
                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                                list3.Add(num4);
                            }
                            this.dv = new DataView(this.oldTbPrivilege);
                            for (int k = 0; k < this.dv.Count; k++)
                            {
                                num4 = (int) this.dv[k]["f_ControllerID"];
                                if (list3.IndexOf(num4) < 0)
                                {
                                    if (privilege.DelPrivilegeOfOneCardByDB(num4, this.consumerID) < 0)
                                    {
                                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                        this.cn.Open();
                                        for (int n = 0; n < this.dgvSelectedDoors.Rows.Count; n++)
                                        {
                                            num4 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[n]["f_ControllerID"];
                                            if (list3.IndexOf(num4) < 0)
                                            {
                                                info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, num4);
                                                this.cm.CommandText = info;
                                                this.cm.ExecuteNonQuery();
                                            }
                                        }
                                        this.cn.Close();
                                        XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }
                                    list3.Add(num4);
                                }
                            }
                        }
                    }
                    wgAppConfig.wgLog((sender as Button).Text.Replace("\r\n", "") + " ," + CommonStr.strUsersNum + " = 1," + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "," + CommonStr.strSuccessfully, EventLogEntryType.Information, null);
                    XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            OleDbConnection connection = null;
            OleDbCommand command = null;
            if (XMessageBox.Show(string.Format("{0} {1} ({2}) ?", CommonStr.strAreYouSureUpdate, this.Text, CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString()), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                if (this.dgvSelectedDoors.Rows.Count > 100)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                try
                {
                    this.bEdit = true;
                    Cursor.Current = Cursors.WaitCursor;
                    wgTools.WriteLine("btnDelete_Click Start");
                    connection = new OleDbConnection(wgAppConfig.dbConString);
                    command = new OleDbCommand("", connection);
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
                    string info = "DELETE FROM  [t_d_Privilege]  WHERE  ";
                    object obj2 = info;
                    info = string.Concat(new object[] { obj2, " [f_ConsumerID] = (", this.consumerID, " ) " });
                    command.CommandText = info;
                    wgTools.WriteLine(info);
                    command.ExecuteNonQuery();
                    wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
                    for (num = 0; num < this.dgvSelectedDoors.Rows.Count; num++)
                    {
                        info = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
                        object obj3 = ((info + " SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ") + this.dgvSelectedDoors.Rows[num].Cells[4].Value.ToString() + " AS [f_ControlSegID]  ") + " FROM t_b_Consumer, t_b_Door " + " WHERE  ";
                        info = string.Concat(new object[] { obj3, " [f_ConsumerID] = (", this.consumerID, " ) " }) + " AND  ( t_b_Door.f_DoorID= " + this.dgvSelectedDoors.Rows[num].Cells[0].Value.ToString() + ")";
                        command.CommandText = info;
                        command.ExecuteNonQuery();
                    }
                    wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
                    string format = "";
                    format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                    for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                    {
                        info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                        command.CommandText = info;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    wgTools.WriteLine("btnDelete_Click End");
                    Cursor.Current = Cursors.Default;
                    this.logOperate(this.btnOKAndUpload);
                    ArrayList list3 = new ArrayList();
                    using (icController controller = new icController())
                    {
                        using (icPrivilege privilege = new icPrivilege())
                        {
                            int num4;
                            for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
                            {
                                num4 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"];
                                if (list3.IndexOf(num4) >= 0)
                                {
                                    continue;
                                }
                                controller.GetInfoFromDBByControllerID(num4);
                                if (controller.GetControllerRunInformationIP() > 0)
                                {
                                    if ((controller.runinfo.regUserCount != 0) || (privilege.ClearAllPrivilegeIP(controller.ControllerSN, controller.IP, controller.PORT) >= 0))
                                    {
                                        goto Label_0543;
                                    }
                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                }
                                else
                                {
                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                }
                                return;
                            Label_0543:
                                if (privilege.AddPrivilegeOfOneCardByDB(num4, this.consumerID) < 0)
                                {
                                    format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                    connection.Open();
                                    for (int m = 0; m < this.dgvSelectedDoors.Rows.Count; m++)
                                    {
                                        num4 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[m]["f_ControllerID"];
                                        if (list3.IndexOf(num4) < 0)
                                        {
                                            info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, num4);
                                            command.CommandText = info;
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    connection.Close();
                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                                list3.Add(num4);
                            }
                            this.dv = new DataView(this.oldTbPrivilege);
                            for (int k = 0; k < this.dv.Count; k++)
                            {
                                num4 = (int) this.dv[k]["f_ControllerID"];
                                if (list3.IndexOf(num4) < 0)
                                {
                                    if (privilege.DelPrivilegeOfOneCardByDB(num4, this.consumerID) < 0)
                                    {
                                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                        connection.Open();
                                        for (int n = 0; n < this.dgvSelectedDoors.Rows.Count; n++)
                                        {
                                            num4 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[n]["f_ControllerID"];
                                            if (list3.IndexOf(num4) < 0)
                                            {
                                                info = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), 1, num4);
                                                command.CommandText = info;
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        connection.Close();
                                        XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }
                                    list3.Add(num4);
                                }
                            }
                        }
                    }
                    wgAppConfig.wgLog((sender as Button).Text.Replace("\r\n", "") + " ," + CommonStr.strUsersNum + " = 1," + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "," + CommonStr.strSuccessfully, EventLogEntryType.Information, null);
                    XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

        private void dfrmPrivilegeSingle_Load(object sender, EventArgs e)
        {
            try
            {
                this.label1.Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.dgvSelectedDoors.Columns[4].Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.dgvSelectedDoors.Columns[5].Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.loadControlSegData();
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
                string cmdText = " SELECT f_PrivilegeRecID, f_ControllerID, f_DoorID, f_ControlSegID,' ' as  f_ControlSegName ";
                cmdText = cmdText + " FROM t_d_Privilege  WHERE f_ConsumerID=  " + this.m_consumerID.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            this.tbPrivilege = new DataTable();
                            adapter.Fill(this.tbPrivilege);
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
                                            table.Rows[j]["f_Selected"] = 1;
                                            table.Rows[j]["f_ControlSegID"] = this.dv[i]["f_ControlSegID"];
                                            for (int k = 0; k < this.controlSegIDList.Length; k++)
                                            {
                                                if (this.controlSegIDList[k] == ((int) table.Rows[j]["f_ControlSegID"]))
                                                {
                                                    table.Rows[j]["f_ControlSegName"] = this.controlSegNameList[k];
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    }
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
            wgTools.WriteLine("loadPrivilegeData Start");
            string cmdText = " SELECT f_PrivilegeRecID, f_ControllerID, f_DoorID, f_ControlSegID,' ' as  f_ControlSegName ";
            cmdText = cmdText + " FROM t_d_Privilege  WHERE f_ConsumerID=  " + this.m_consumerID.ToString();
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        this.tbPrivilege = new DataTable();
                        adapter.Fill(this.tbPrivilege);
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
                                        table.Rows[j]["f_Selected"] = 1;
                                        table.Rows[j]["f_ControlSegID"] = this.dv[i]["f_ControlSegID"];
                                        for (int k = 0; k < this.controlSegIDList.Length; k++)
                                        {
                                            if (this.controlSegIDList[k] == ((int) table.Rows[j]["f_ControlSegID"]))
                                            {
                                                table.Rows[j]["f_ControlSegName"] = this.controlSegNameList[k];
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
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

