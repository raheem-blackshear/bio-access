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
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPrivilege : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private BackgroundWorker backgroundWorker1;
        private bool bEdit;
        private bool bStarting = true;
        private SqlCommand cmd;
        private SqlConnection cn;
        private int[] controlSegIDList = new int[0x100];
        private string strGroupFilter = "";
        private string strZoneFilter = "";
        private System.Windows.Forms.Timer timer1;

        public dfrmPrivilege()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            e.Result = this.loadUserData4BackWork();
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
                this.loadUserData4BackWorkComplete(e.Result as DataTable);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
            }
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

        private void btnAddAllUsers_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (this.strGroupFilter == "")
            {
                icConsumerShare.selectAllUsers();
                ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                if (this.dgvSelectedUsers.RowCount > 0x3e8)
                {
                    this.btnAddPassAndUpload.Enabled = false;
                    this.btnDeletePassAndUpload.Enabled = false;
                }
            }
            else
            {
                wgTools.WriteLine("btnAddAllUsers_Click Start");
                this.dt = ((DataView) this.dgvUsers.DataSource).Table;
                this.dv1 = (DataView) this.dgvUsers.DataSource;
                this.dv2 = (DataView) this.dgvSelectedUsers.DataSource;
                this.dgvUsers.DataSource = null;
                this.dgvSelectedUsers.DataSource = null;
                if (this.strGroupFilter != "")
                {
                    string rowFilter = this.dv1.RowFilter;
                    string str2 = this.dv2.RowFilter;
                    this.dv1.Dispose();
                    this.dv2.Dispose();
                    this.dv1 = null;
                    this.dv2 = null;
                    this.dt.BeginLoadData();
                    this.dv = new DataView(this.dt);
                    this.dv.RowFilter = this.strGroupFilter;
                    for (int i = 0; i < this.dv.Count; i++)
                    {
                        this.dv[i]["f_Selected"] = icConsumerShare.getSelectedValue();
                    }
                    this.dt.EndLoadData();
                    this.dv1 = new DataView(this.dt);
                    this.dv1.RowFilter = rowFilter;
                    this.dv2 = new DataView(this.dt);
                    this.dv2.RowFilter = str2;
                    this.dgvUsers.DataSource = this.dv1;
                    this.dgvSelectedUsers.DataSource = this.dv2;
                    wgTools.WriteLine("btnAddAllUsers_Click End");
                    if (this.dv2.Count > 0x3e8)
                    {
                        this.btnAddPassAndUpload.Enabled = false;
                        this.btnDeletePassAndUpload.Enabled = false;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void btnAddOneDoor_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvDoors);
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvUsers, icConsumerShare.iSelectedCurrentNoneMax);
            if (this.dgvSelectedUsers.RowCount > 0x3e8)
            {
                this.btnAddPassAndUpload.Enabled = false;
                this.btnDeletePassAndUpload.Enabled = false;
            }
        }

        private void btnAddPass_Click(object sender, EventArgs e)
        {
            this.btnAddPassAndUpload_Click(sender, e);
            this.bEdit = true;
        }

        private void btnAddPassAndUpload_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnAddPassAndUpload_Click_Acc(sender, e);
            }
            else if (((XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + 
                this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + 
                CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() +
                "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) && 
                (this.dgvSelectedDoors.Rows.Count > 0)) && (this.dgvSelectedUsers.Rows.Count > 0))
            {
                this.bEdit = true;
                if ((this.dgvSelectedUsers.Rows.Count > 0x3e8) || (this.dgvSelectedDoors.Rows.Count > 100))
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("btnAddPass_Click Start");
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    this.cn.Open();
                    this.cmd = new SqlCommand("", this.cn);
                    try
                    {
                        string str;
                        string str2 = "";
                        int num = 0;
                        num = 0;
                        this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
                        bool flag = true;
                        flag = true;
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
                        int num2 = 0;
                        int num3 = 0;
                        while (num < this.dgvSelectedDoors.Rows.Count)
                        {
                            str2 = "";
                            int count = 0;
                            if ((list.Count > 0) && (num2 < list.Count))
                            {
                                str2 = str2 + " [f_ControllerID] = ( " + list[num2].ToString() + ")";
                                this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", list[num2].ToString());
                                num2++;
                                count = this.dvSelectedControllerID.Count;
                                num += this.dvSelectedControllerID.Count;
                            }
                            else
                            {
                                if (this.dvDoorTmpSelected.Count <= num3)
                                {
                                    break;
                                }
                                str2 = str2 + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num3]["f_DoorID"].ToString() + ")";
                                num3++;
                                count = 1;
                                num++;
                            }
                            int num5 = 0x7d0;
                            int num6 = 0;
                            while (num6 < this.dgvSelectedUsers.Rows.Count)
                            {
                                string str3 = "";
                                if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                                {
                                    while (num6 < this.dgvSelectedUsers.Rows.Count)
                                    {
                                        str3 = str3 + ((DataView) this.dgvSelectedUsers.DataSource)[num6]["f_ConsumerID"] + ",";
                                        num6++;
                                        if (str3.Length > num5)
                                        {
                                            break;
                                        }
                                    }
                                    str3 = str3 + "0";
                                }
                                else
                                {
                                    num6 = this.dgvSelectedUsers.Rows.Count;
                                }
                                if (flag)
                                {
                                    str = "DELETE FROM  [t_d_Privilege]  WHERE  ";
                                    str = str + "  ( " + str2 + ")";
                                    if (str3 != "")
                                    {
                                        str = str + " AND [f_ConsumerID] IN (" + str3 + " ) ";
                                    }
                                    this.cmd.CommandText = str;
                                    wgTools.WriteLine(str);
                                    this.cmd.ExecuteNonQuery();
                                    wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
                                }
                                this.progressBar1.Value = (num6 * count) + (this.dgvSelectedUsers.Rows.Count * (num - count));
                                Application.DoEvents();
                            }
                        }
                        flag = true;
                        num = 0;
                        num2 = 0;
                        num3 = 0;
                        while (num < this.dgvSelectedDoors.Rows.Count)
                        {
                            str2 = "";
                            int num7 = 0;
                            if ((list.Count > 0) && (num2 < list.Count))
                            {
                                str2 = str2 + " [f_ControllerID] = ( " + list[num2].ToString() + ")";
                                this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", list[num2].ToString());
                                num2++;
                                num7 = this.dvSelectedControllerID.Count;
                                num += this.dvSelectedControllerID.Count;
                            }
                            else
                            {
                                if (this.dvDoorTmpSelected.Count <= num3)
                                {
                                    break;
                                }
                                str2 = str2 + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num3]["f_DoorID"].ToString() + ")";
                                num3++;
                                num7 = 1;
                                num++;
                            }
                            int num8 = 0x7d0;
                            int num9 = 0;
                            while (num9 < this.dgvSelectedUsers.Rows.Count)
                            {
                                string str4 = "";
                                if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                                {
                                    while (num9 < this.dgvSelectedUsers.Rows.Count)
                                    {
                                        str4 = str4 + ((DataView) this.dgvSelectedUsers.DataSource)[num9]["f_ConsumerID"] + ",";
                                        num9++;
                                        if (str4.Length > num8)
                                        {
                                            break;
                                        }
                                    }
                                    str4 = str4 + "0";
                                }
                                else
                                {
                                    num9 = this.dgvSelectedUsers.Rows.Count;
                                }
                                str = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, " SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ", this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex], " AS [f_ControlSegID]  " }) + " FROM t_b_Consumer, t_b_Door " + " WHERE  ((t_b_Consumer.f_DoorEnabled)=1) ";
                                if (str4 != "")
                                {
                                    str = str + " AND [f_ConsumerID] IN (" + str4 + " ) ";
                                }
                                str = str + " AND  ( " + str2 + ")";
                                this.cmd.CommandText = str;
                                wgTools.WriteLine(str);
                                this.cmd.ExecuteNonQuery();
                                wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
                                this.progressBar1.Value = (num9 * num7) + (this.dgvSelectedUsers.Rows.Count * (num - num7));
                                Application.DoEvents();
                            }
                        }
                        string format = "";
                        if (sender.Equals(this.btnAddPass))
                        {
                            format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                        }
                        else
                        {
                            format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                        }
                        for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                        {
                            str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                            this.cmd.CommandText = str;
                            this.cmd.ExecuteNonQuery();
                        }
                        wgTools.WriteLine("btnAddPass_Click End");
                        Cursor.Current = Cursors.Default;
                        this.progressBar1.Value = this.progressBar1.Maximum;
                        if (sender.Equals(this.btnAddPass))
                        {
                            this.logOperate(this.btnAddPass);
                            XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "\r\n\r\n" + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.progressBar1.Value = 0;
                        }
                        else
                        {
                            this.logOperate(this.btnAddPassAndUpload);
                            this.progressBar1.Value = 0;
                            ArrayList list3 = new ArrayList();
                            this.progressBar1.Maximum = this.dgvSelectedDoors.Rows.Count;
                            if (this.dgvSelectedUsers.Rows.Count > 0)
                            {
                                using (icPrivilege privilege = new icPrivilege())
                                {
                                    using (icController controller = new icController())
                                    {
                                        for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
                                        {
                                            int num14;
                                            int num11 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"];
                                            if (list3.IndexOf(num11) >= 0)
                                            {
                                                goto Label_0C73;
                                            }
                                            controller.GetInfoFromDBByControllerID(num11);
                                            if (controller.GetControllerRunInformationIP() > 0)
                                            {
                                                goto Label_0B19;
                                                /*
                                                if ((controller.runinfo.regUserCount != 0) || (privilege.ClearAllPrivilegeIP(controller.ControllerSN, controller.IP, controller.PORT) >= 0))
                                                {
                                                    goto Label_0B19;
                                                }
                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                this.progressBar1.Value = 0;*/
                                            }
                                            else
                                            {
                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                this.progressBar1.Value = 0;
                                            }
                                            return;
                                        Label_0B19:
                                            num14 = 0;
                                            int ret;
                                            while (num14 < this.dgvSelectedUsers.Rows.Count)
                                            {
                                                if ((ret = privilege.AddPrivilegeOfOneCardByDB(num11, (int) ((DataView) this.dgvSelectedUsers.DataSource)[num14]["f_ConsumerID"])) < 0)
                                                {
                                                    format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                                    if (this.cn.State != ConnectionState.Open)
                                                    {
                                                        this.cn.Open();
                                                    }
                                                    for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
                                                    {
                                                        num11 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"];
                                                        if (list3.IndexOf(num11) < 0)
                                                        {
                                                            str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num11);
                                                            this.cmd.CommandText = str;
                                                            this.cmd.ExecuteNonQuery();
                                                        }
                                                    }
                                                    string msg;
                                                    switch (ret)
                                                    {
                                                        case wgTools.ErrorCode.ERR_DB_IS_FULL:
                                                        case wgTools.ErrorCode.ERR_FAIL:
                                                            XMessageBox.Show(wgGlobal.getErrorString(ret), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            break;
                                                        case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                                                            msg = wgGlobal.getErrorString(ret) + " " + 
                                                                CommonStr.strUserID + ":" + privilege.templDoubled.uid.ToString() + " " + 
                                                                CommonStr.strVerifFinger + ":" + (privilege.templDoubled.finger + 1).ToString();
                                                            XMessageBox.Show(msg, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            break;
                                                        case wgTools.ErrorCode.ERR_FACE_DOUBLED:
                                                            msg = wgGlobal.getErrorString(ret) + " " + CommonStr.strUserID + ":" + 
                                                                privilege.faceTemplDoubled.uid.ToString();
                                                            XMessageBox.Show(msg, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            break;
                                                        default:
                                                            XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            this.progressBar1.Value = 0;
                                                            return;
                                                    }
                                                }
                                                num14++;
                                            }
                                            list3.Add(num11);
                                        Label_0C73:
                                            this.progressBar1.Value = j + 1;
                                        }
                                    }
                                }
                            }
                            wgAppConfig.wgLog((sender as Button).Text.Replace("\r\n", "") + " ," + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "," + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "," + CommonStr.strSuccessfully, EventLogEntryType.Information, null);
                            Cursor.Current = Cursors.Default;
                            this.progressBar1.Value = this.progressBar1.Maximum;
                            XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "\r\n\r\n" + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.progressBar1.Value = 0;
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                    }
                    finally
                    {
                        if (this.cmd != null)
                        {
                            this.cmd.Dispose();
                        }
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
                finally
                {
                    if (this.cn != null)
                    {
                        this.cn.Dispose();
                    }
                    this.dfrmWait1.Hide();
                }
            }
        }

        private void btnAddPassAndUpload_Click_Acc(object sender, EventArgs e)
        {
            OleDbCommand command = null;
            OleDbConnection connection = null;
            if (((XMessageBox.Show((sender as ImageButton).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + 
                this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + 
                CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + 
                "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) && 
                (this.dgvSelectedDoors.Rows.Count > 0)) && (this.dgvSelectedUsers.Rows.Count > 0))
            {
                this.bEdit = true;
                if ((this.dgvSelectedUsers.Rows.Count > 0x3e8) || (this.dgvSelectedDoors.Rows.Count > 100))
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("btnAddPass_Click Start");
                connection = new OleDbConnection(wgAppConfig.dbConString);
                try
                {
                    connection.Open();
                    using (command = new OleDbCommand("", connection))
                    {
                        try
                        {
                            string str;
                            string str2 = "";
                            int num = 0;
                            num = 0;
                            this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
                            bool flag = true;
                            flag = true;
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
                            int num2 = 0;
                            int num3 = 0;
                            while (num < this.dgvSelectedDoors.Rows.Count)
                            {
                                str2 = "";
                                int count = 0;
                                if ((list.Count > 0) && (num2 < list.Count))
                                {
                                    str2 = str2 + " [f_ControllerID] = ( " + list[num2].ToString() + ")";
                                    this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", list[num2].ToString());
                                    num2++;
                                    count = this.dvSelectedControllerID.Count;
                                    num += this.dvSelectedControllerID.Count;
                                }
                                else
                                {
                                    if (this.dvDoorTmpSelected.Count <= num3)
                                    {
                                        break;
                                    }
                                    str2 = str2 + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num3]["f_DoorID"].ToString() + ")";
                                    num3++;
                                    count = 1;
                                    num++;
                                }
                                int num5 = 0x7d0;
                                int num6 = 0;
                                while (num6 < this.dgvSelectedUsers.Rows.Count)
                                {
                                    string str3 = "";
                                    if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                                    {
                                        while (num6 < this.dgvSelectedUsers.Rows.Count)
                                        {
                                            str3 = str3 + ((DataView) this.dgvSelectedUsers.DataSource)[num6]["f_ConsumerID"] + ",";
                                            num6++;
                                            if (str3.Length > num5)
                                            {
                                                break;
                                            }
                                        }
                                        str3 = str3 + "0";
                                    }
                                    else
                                    {
                                        num6 = this.dgvSelectedUsers.Rows.Count;
                                    }
                                    if (flag)
                                    {
                                        str = "DELETE FROM  [t_d_Privilege]  WHERE  ";
                                        str = str + "  ( " + str2 + ")";
                                        if (str3 != "")
                                        {
                                            str = str + " AND [f_ConsumerID] IN (" + str3 + " ) ";
                                        }
                                        command.CommandText = str;
                                        wgTools.WriteLine(str);
                                        command.ExecuteNonQuery();
                                        wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
                                    }
                                    this.progressBar1.Value = (num6 * count) + (this.dgvSelectedUsers.Rows.Count * (num - count));
                                    Application.DoEvents();
                                }
                            }
                            flag = true;
                            num = 0;
                            num2 = 0;
                            num3 = 0;
                            while (num < this.dgvSelectedDoors.Rows.Count)
                            {
                                str2 = "";
                                int num7 = 0;
                                if ((list.Count > 0) && (num2 < list.Count))
                                {
                                    str2 = str2 + " [f_ControllerID] = ( " + list[num2].ToString() + ")";
                                    this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", list[num2].ToString());
                                    num2++;
                                    num7 = this.dvSelectedControllerID.Count;
                                    num += this.dvSelectedControllerID.Count;
                                }
                                else
                                {
                                    if (this.dvDoorTmpSelected.Count <= num3)
                                    {
                                        break;
                                    }
                                    str2 = str2 + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num3]["f_DoorID"].ToString() + ")";
                                    num3++;
                                    num7 = 1;
                                    num++;
                                }
                                int num8 = 0x7d0;
                                int num9 = 0;
                                while (num9 < this.dgvSelectedUsers.Rows.Count)
                                {
                                    string str4 = "";
                                    if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                                    {
                                        while (num9 < this.dgvSelectedUsers.Rows.Count)
                                        {
                                            str4 = str4 + ((DataView) this.dgvSelectedUsers.DataSource)[num9]["f_ConsumerID"] + ",";
                                            num9++;
                                            if (str4.Length > num8)
                                            {
                                                break;
                                            }
                                        }
                                        str4 = str4 + "0";
                                    }
                                    else
                                    {
                                        num9 = this.dgvSelectedUsers.Rows.Count;
                                    }
                                    str = "INSERT INTO [t_d_Privilege] (f_ConsumerID, f_DoorID ,f_ControllerID, f_DoorNO, f_ControlSegID)";
                                    object obj2 = str;
                                    str = string.Concat(new object[] { obj2, " SELECT t_b_Consumer.f_ConsumerID, t_b_Door.f_DoorID, t_b_Door.f_ControllerID ,t_b_Door.f_DoorNO, ", this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex], " AS [f_ControlSegID]  " }) + " FROM t_b_Consumer, t_b_Door " + " WHERE  ((t_b_Consumer.f_DoorEnabled)=1) ";
                                    if (str4 != "")
                                    {
                                        str = str + " AND [f_ConsumerID] IN (" + str4 + " ) ";
                                    }
                                    str = str + " AND  ( " + str2 + ")";
                                    command.CommandText = str;
                                    wgTools.WriteLine(str);
                                    command.ExecuteNonQuery();
                                    wgTools.WriteLine("INSERT INTO [t_d_Privilege] End");
                                    this.progressBar1.Value = (num9 * num7) + (this.dgvSelectedUsers.Rows.Count * (num - num7));
                                    Application.DoEvents();
                                }
                            }
                            string format = "";
                            if (sender.Equals(this.btnAddPass))
                            {
                                format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                            }
                            else
                            {
                                format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                            }
                            for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                            {
                                str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                                command.CommandText = str;
                                command.ExecuteNonQuery();
                            }
                            wgTools.WriteLine("btnAddPass_Click End");
                            Cursor.Current = Cursors.Default;
                            this.progressBar1.Value = this.progressBar1.Maximum;
                            if (sender.Equals(this.btnAddPass))
                            {
                                this.logOperate(this.btnAddPass);
                                XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "\r\n\r\n" + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.progressBar1.Value = 0;
                            }
                            else
                            {
                                this.logOperate(this.btnAddPassAndUpload);
                                this.progressBar1.Value = 0;
                                ArrayList list3 = new ArrayList();
                                this.progressBar1.Maximum = this.dgvSelectedDoors.Rows.Count;
                                if (this.dgvSelectedUsers.Rows.Count > 0)
                                {
                                    using (icPrivilege privilege = new icPrivilege())
                                    {
                                        using (icController controller = new icController())
                                        {
                                            for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
                                            {
                                                int num14;
                                                int num11 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"];
                                                if (list3.IndexOf(num11) >= 0)
                                                {
                                                    goto Label_0C34;
                                                }
                                                controller.GetInfoFromDBByControllerID(num11);
                                                if (controller.GetControllerRunInformationIP() > 0)
                                                {
                                                    goto Label_0AEE;
                                                    /*
                                                    if ((controller.runinfo.regUserCount != 0) || (privilege.ClearAllPrivilegeIP(controller.ControllerSN, controller.IP, controller.PORT) >= 0))
                                                    {
                                                        goto Label_0AEE;
                                                    }
                                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                    this.progressBar1.Value = 0;*/
                                                }
                                                else
                                                {
                                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                    this.progressBar1.Value = 0;
                                                }
                                                return;
                                            Label_0AEE:
                                                num14 = 0;
                                                int ret;
                                                while (num14 < this.dgvSelectedUsers.Rows.Count)
                                                {
                                                    if ((ret = privilege.AddPrivilegeOfOneCardByDB(num11, (int)((DataView)this.dgvSelectedUsers.DataSource)[num14]["f_ConsumerID"])) < 0)
                                                    {
                                                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                                        if (connection.State != ConnectionState.Open)
                                                        {
                                                            connection.Open();
                                                        }
                                                        for (int k = 0; k < this.dgvSelectedDoors.Rows.Count; k++)
                                                        {
                                                            num11 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[k]["f_ControllerID"];
                                                            if (list3.IndexOf(num11) < 0)
                                                            {
                                                                str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num11);
                                                                command.CommandText = str;
                                                                command.ExecuteNonQuery();
                                                            }
                                                        }
                                                        string msg;
                                                        switch (ret)
                                                        {
                                                            case wgTools.ErrorCode.ERR_DB_IS_FULL:
                                                            case wgTools.ErrorCode.ERR_FAIL:
                                                                XMessageBox.Show(wgGlobal.getErrorString(ret), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                break;
                                                            case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                                                                msg = wgGlobal.getErrorString(ret) + " " + 
                                                                    CommonStr.strUserID + ":" + privilege.templDoubled.uid.ToString() + " " + 
                                                                    CommonStr.strVerifFinger + ":" + (privilege.templDoubled.finger + 1).ToString();
                                                                XMessageBox.Show(msg, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                break;
                                                            case wgTools.ErrorCode.ERR_FACE_DOUBLED:
                                                                msg = wgGlobal.getErrorString(ret) + " " + CommonStr.strUserID + ":" + 
                                                                    privilege.faceTemplDoubled.uid.ToString();
                                                                XMessageBox.Show(msg, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                break;
                                                            default:
                                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                this.progressBar1.Value = 0;
                                                                return;
                                                        }
                                                    }
                                                    num14++;
                                                }
                                                list3.Add(num11);
                                            Label_0C34:
                                                this.progressBar1.Value = j + 1;
                                            }
                                        }
                                    }
                                }
                                wgAppConfig.wgLog((sender as Button).Text.Replace("\r\n", "") + " ," + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "," + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "," + CommonStr.strSuccessfully, EventLogEntryType.Information, null);
                                Cursor.Current = Cursors.Default;
                                this.progressBar1.Value = this.progressBar1.Maximum;
                                XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "\r\n\r\n" + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.progressBar1.Value = 0;
                            }
                        }
                        catch (Exception exception)
                        {
                            wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                        }
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                    this.dfrmWait1.Hide();
                }
            }
        }

        private void btnDelAllDoors_Click(object sender, EventArgs e)
        {
            this.dt = ((DataView) this.dgvSelectedDoors.DataSource).Table;
            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i]["f_Selected"] = 0;
            }
        }

        private void btnDelAllUsers_Click(object sender, EventArgs e)
        {
            if (this.dgvSelectedUsers.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                wgTools.WriteLine("btnDelAllUsers_Click Start");
                icConsumerShare.selectNoneUsers();
                if (string.IsNullOrEmpty(this.strGroupFilter))
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                    ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                    ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                }
            }
        }

        private void btnDeletePass_Click(object sender, EventArgs e)
        {
            this.btnDeletePassAndUpload_Click(sender, e);
            this.bEdit = true;
        }

        private void btnDeletePassAndUpload_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnDeletePassAndUpload_Click_Acc(sender, e);
            }
            else if (((XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) && (this.dgvSelectedDoors.Rows.Count > 0)) && (this.dgvSelectedUsers.Rows.Count > 0))
            {
                this.bEdit = true;
                Cursor.Current = Cursors.WaitCursor;
                if (this.dgvSelectedUsers.Rows.Count > 0x3e8)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                wgTools.WriteLine("btnDelete_Click Start");
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                this.cmd = new SqlCommand("", this.cn);
                try
                {
                    string str;
                    this.cn.Open();
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
                    string str2 = "";
                    int num = 0;
                    this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
                    int num2 = 0;
                    int num3 = 0;
                    while (num < this.dgvSelectedDoors.Rows.Count)
                    {
                        str2 = "";
                        int count = 0;
                        if ((list.Count > 0) && (num2 < list.Count))
                        {
                            str2 = str2 + " [f_ControllerID] = ( " + list[num2].ToString() + ")";
                            this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", list[num2].ToString());
                            num2++;
                            count = this.dvSelectedControllerID.Count;
                            num += this.dvSelectedControllerID.Count;
                        }
                        else
                        {
                            if (this.dvDoorTmpSelected.Count <= num3)
                            {
                                break;
                            }
                            str2 = str2 + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num3]["f_DoorID"].ToString() + ")";
                            num3++;
                            count = 1;
                            num++;
                        }
                        int num5 = 0x7d0;
                        int num6 = 0;
                        while (num6 < this.dgvSelectedUsers.Rows.Count)
                        {
                            string str3 = "";
                            if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                            {
                                while (num6 < this.dgvSelectedUsers.Rows.Count)
                                {
                                    str3 = str3 + ((DataView) this.dgvSelectedUsers.DataSource)[num6]["f_ConsumerID"] + ",";
                                    if (str3.Length > num5)
                                    {
                                        break;
                                    }
                                    num6++;
                                }
                                str3 = str3 + "0";
                            }
                            else
                            {
                                num6 = this.dgvSelectedUsers.Rows.Count;
                            }
                            str = "DELETE FROM  [t_d_Privilege]  WHERE  ";
                            str = str + "  ( " + str2 + ")";
                            if (str3 != "")
                            {
                                str = str + " AND [f_ConsumerID] IN (" + str3 + " ) ";
                            }
                            this.cmd.CommandText = str;
                            wgTools.WriteLine(str);
                            this.cmd.ExecuteNonQuery();
                            wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
                            this.progressBar1.Value = (num6 * count) + (this.dgvSelectedUsers.Rows.Count * (num - count));
                            Application.DoEvents();
                        }
                    }
                    string format = "";
                    if (sender.Equals(this.btnDeletePass))
                    {
                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                    }
                    else
                    {
                        format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                    }
                    for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                    {
                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                        this.cmd.CommandText = str;
                        this.cmd.ExecuteNonQuery();
                    }
                    wgTools.WriteLine("btnDelete_Click End");
                    this.progressBar1.Value = this.progressBar1.Maximum;
                    Cursor.Current = Cursors.Default;
                    if (sender.Equals(this.btnDeletePass))
                    {
                        this.logOperate(this.btnDeletePass);
                        XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "\r\n\r\n" + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.progressBar1.Value = 0;
                    }
                    else
                    {
                        this.logOperate(this.btnDeletePassAndUpload);
                        ArrayList list3 = new ArrayList();
                        if (this.dgvSelectedUsers.Rows.Count > 0)
                        {
                            using (icPrivilege privilege = new icPrivilege())
                            {
                                for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
                                {
                                    int num8 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"];
                                    if (list3.IndexOf(num8) < 0)
                                    {
                                        for (int k = 0; k < this.dgvSelectedUsers.Rows.Count; k++)
                                        {
                                            if (privilege.DelPrivilegeOfOneCardByDB(num8, (int) ((DataView) this.dgvSelectedUsers.DataSource)[k]["f_ConsumerID"]) < 0)
                                            {
                                                format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                                for (int m = 0; m < this.dgvSelectedDoors.Rows.Count; m++)
                                                {
                                                    num8 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[m]["f_ControllerID"];
                                                    if (list3.IndexOf(num8) < 0)
                                                    {
                                                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num8);
                                                        this.cmd.CommandText = str;
                                                        this.cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                this.progressBar1.Value = 0;
                                                return;
                                            }
                                        }
                                        list3.Add(num8);
                                    }
                                }
                            }
                        }
                        wgAppConfig.wgLog((sender as Button).Text.Replace("\r\n", "") + " ," + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "," + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "," + CommonStr.strSuccessfully, EventLogEntryType.Information, null);
                        this.progressBar1.Value = this.progressBar1.Maximum;
                        Cursor.Current = Cursors.Default;
                        XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.progressBar1.Value = 0;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    if (this.cmd != null)
                    {
                        this.cmd.Dispose();
                    }
                    if (this.cn != null)
                    {
                        this.cn.Dispose();
                    }
                    this.dfrmWait1.Hide();
                }
            }
        }

        private void btnDeletePassAndUpload_Click_Acc(object sender, EventArgs e)
        {
            OleDbCommand command = null;
            OleDbConnection connection = null;
            if (((XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) && (this.dgvSelectedDoors.Rows.Count > 0)) && (this.dgvSelectedUsers.Rows.Count > 0))
            {
                this.bEdit = true;
                Cursor.Current = Cursors.WaitCursor;
                if (this.dgvSelectedUsers.Rows.Count > 0x3e8)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                wgTools.WriteLine("btnDelete_Click Start");
                connection = new OleDbConnection(wgAppConfig.dbConString);
                command = new OleDbCommand("", connection);
                try
                {
                    string str;
                    connection.Open();
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
                    string str2 = "";
                    int num = 0;
                    this.progressBar1.Maximum = this.dgvSelectedDoors.RowCount * this.dgvSelectedUsers.RowCount;
                    int num2 = 0;
                    int num3 = 0;
                    while (num < this.dgvSelectedDoors.Rows.Count)
                    {
                        str2 = "";
                        int count = 0;
                        if ((list.Count > 0) && (num2 < list.Count))
                        {
                            str2 = str2 + " [f_ControllerID] = ( " + list[num2].ToString() + ")";
                            this.dvSelectedControllerID.RowFilter = string.Format("f_Selected > 0  AND f_ControllerID ={0} ", list[num2].ToString());
                            num2++;
                            count = this.dvSelectedControllerID.Count;
                            num += this.dvSelectedControllerID.Count;
                        }
                        else
                        {
                            if (this.dvDoorTmpSelected.Count <= num3)
                            {
                                break;
                            }
                            str2 = str2 + " [f_DoorID] = ( " + this.dvDoorTmpSelected[num3]["f_DoorID"].ToString() + ")";
                            num3++;
                            count = 1;
                            num++;
                        }
                        int num5 = 0x7d0;
                        int num6 = 0;
                        while (num6 < this.dgvSelectedUsers.Rows.Count)
                        {
                            string str3 = "";
                            if (((DataView) this.dgvSelectedUsers.DataSource).Table.Rows.Count > this.dgvSelectedUsers.Rows.Count)
                            {
                                while (num6 < this.dgvSelectedUsers.Rows.Count)
                                {
                                    str3 = str3 + ((DataView) this.dgvSelectedUsers.DataSource)[num6]["f_ConsumerID"] + ",";
                                    if (str3.Length > num5)
                                    {
                                        break;
                                    }
                                    num6++;
                                }
                                str3 = str3 + "0";
                            }
                            else
                            {
                                num6 = this.dgvSelectedUsers.Rows.Count;
                            }
                            str = "DELETE FROM  [t_d_Privilege]  WHERE  ";
                            str = str + "  ( " + str2 + ")";
                            if (str3 != "")
                            {
                                str = str + " AND [f_ConsumerID] IN (" + str3 + " ) ";
                            }
                            command.CommandText = str;
                            wgTools.WriteLine(str);
                            command.ExecuteNonQuery();
                            wgTools.WriteLine("DELETE FROM  [t_d_Privilege] End");
                            this.progressBar1.Value = (num6 * count) + (this.dgvSelectedUsers.Rows.Count * (num - count));
                            Application.DoEvents();
                        }
                    }
                    string format = "";
                    if (sender.Equals(this.btnDeletePass))
                    {
                        format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                    }
                    else
                    {
                        format = "UPDATE t_b_Controller SET f_lastDelAddAndUploadDateTime ={0}, f_lastDelAddAndUploadConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                    }
                    for (int i = 0; i < this.dgvSelectedDoors.Rows.Count; i++)
                    {
                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, (int) ((DataView) this.dgvSelectedDoors.DataSource)[i]["f_ControllerID"]);
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                    wgTools.WriteLine("btnDelete_Click End");
                    this.progressBar1.Value = this.progressBar1.Maximum;
                    Cursor.Current = Cursors.Default;
                    if (sender.Equals(this.btnDeletePass))
                    {
                        this.logOperate(this.btnDeletePass);
                        XMessageBox.Show((sender as Button).Text + " \r\n\r\n" + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "\r\n\r\n" + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "\r\n\r\n" + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.progressBar1.Value = 0;
                    }
                    else
                    {
                        this.logOperate(this.btnDeletePassAndUpload);
                        ArrayList list3 = new ArrayList();
                        if (this.dgvSelectedUsers.Rows.Count > 0)
                        {
                            using (icPrivilege privilege = new icPrivilege())
                            {
                                for (int j = 0; j < this.dgvSelectedDoors.Rows.Count; j++)
                                {
                                    int num8 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[j]["f_ControllerID"];
                                    if (list3.IndexOf(num8) < 0)
                                    {
                                        for (int k = 0; k < this.dgvSelectedUsers.Rows.Count; k++)
                                        {
                                            if (privilege.DelPrivilegeOfOneCardByDB(num8, (int) ((DataView) this.dgvSelectedUsers.DataSource)[k]["f_ConsumerID"]) < 0)
                                            {
                                                format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal ={1:d} WHERE f_ControllerID ={2:d}";
                                                for (int m = 0; m < this.dgvSelectedDoors.Rows.Count; m++)
                                                {
                                                    num8 = (int) ((DataView) this.dgvSelectedDoors.DataSource)[m]["f_ControllerID"];
                                                    if (list3.IndexOf(num8) < 0)
                                                    {
                                                        str = string.Format(format, wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.dgvSelectedUsers.RowCount, num8);
                                                        command.CommandText = str;
                                                        command.ExecuteNonQuery();
                                                    }
                                                }
                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                this.progressBar1.Value = 0;
                                                return;
                                            }
                                        }
                                        list3.Add(num8);
                                    }
                                }
                            }
                        }
                        wgAppConfig.wgLog((sender as Button).Text.Replace("\r\n", "") + " ," + CommonStr.strUsersNum + " = " + this.dgvSelectedUsers.RowCount.ToString() + "," + CommonStr.strDoorsNum + " = " + this.dgvSelectedDoors.RowCount.ToString() + "," + CommonStr.strSuccessfully, EventLogEntryType.Information, null);
                        this.progressBar1.Value = this.progressBar1.Maximum;
                        Cursor.Current = Cursors.Default;
                        XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.progressBar1.Value = 0;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                    this.dfrmWait1.Hide();
                }
            }
        }

        private void btnDelOneDoor_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedDoors);
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedUsers, icConsumerShare.iSelectedCurrentNoneMax);
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

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    dataSource.RowFilter = icConsumerShare.getOptionalRowfilter();
                    this.strGroupFilter = "";
                }
                else
                {
                    dataSource.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
                    num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num2);
                            this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num2);
                        }
                        else
                        {
                            dataSource.RowFilter = "f_Selected = 0 ";
                            string str = "";
                            for (int i = 0; i < this.arrGroupNO.Count; i++)
                            {
                                if ((((int) this.arrGroupNO[i]) <= num3) && (((int) this.arrGroupNO[i]) >= num))
                                {
                                    if (str == "")
                                    {
                                        str = str + string.Format(" f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                    }
                                    else
                                    {
                                        str = str + string.Format(" OR f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                    }
                                }
                            }
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", str);
                            this.strGroupFilter = string.Format("  {0} ", str);
                        }
                    }
                    dataSource.RowFilter = string.Format("(f_DoorEnabled > 0) AND {0} AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                if (string.IsNullOrEmpty(this.strGroupFilter))
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                }
                else
                {
                    ((DataView) this.dgvUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND  ({0}) AND ({1})", icConsumerShare.getOptionalRowfilter(), this.strGroupFilter);
                }
                ((DataView) this.dgvSelectedUsers.DataSource).RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
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

        private void dfrmPrivilege_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
            try
            {
                if (this.dfrmWait1 != null)
                {
                    this.dfrmWait1.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        private void dfrmPrivilege_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmPrivilege_Load(object sender, EventArgs e)
        {
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
            this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            try
            {
                this.label1.Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.cbof_ControlSegID.Visible = wgAppConfig.getParamValBoolByNO(0x79);
                this.loadControlSegData();
                new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
                for (int i = 0; i < this.arrGroupID.Count; i++)
                {
                    if ((i == 0) && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
                    {
                        this.cbof_GroupID.Items.Add(CommonStr.strAll);
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
                this.loadZoneInfo();
                this.loadDoorData();
                this.cbof_Zone_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedDoors.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void dgvDoors_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void dgvDoors_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void dgvDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnAddOneDoor.PerformClick();
        }

        private void dgvSelectedDoors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneDoor.PerformClick();
        }

        private void dgvSelectedUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnDelOneUser.PerformClick();
        }

        private void dgvUsers_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnAddOneUser.PerformClick();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
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
                this.controlSegIDList[0] = 1;
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    string str = " SELECT ";
                    using (SqlCommand command = new SqlCommand(((str + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, " + "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),  ") + "     ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50), " + "     ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']') ") + "    END AS f_ControlSegID  " + "  FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", this.cn))
                    {
                        this.cn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        for (int i = 1; reader.Read(); i++)
                        {
                            this.cbof_ControlSegID.Items.Add(reader["f_ControlSegID"]);
                            this.controlSegIDList[i] = (int) reader["f_ControlSegIDBak"];
                        }
                        reader.Close();
                        if (this.cbof_ControlSegID.Items.Count > 0)
                        {
                            this.cbof_ControlSegID.SelectedIndex = 0;
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    if (this.cn != null)
                    {
                        this.cn.Dispose();
                    }
                }
            }
        }

        private void loadControlSegData_Acc()
        {
            this.cbof_ControlSegID.Items.Clear();
            this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
            this.controlSegIDList[0] = 1;
            OleDbConnection connection = null;
            connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                string str = " SELECT ";
                using (OleDbCommand command = new OleDbCommand((str + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ") + "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID " + "  FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    for (int i = 1; reader.Read(); i++)
                    {
                        this.cbof_ControlSegID.Items.Add(reader["f_ControlSegID"]);
                        this.controlSegIDList[i] = (int) reader["f_ControlSegIDBak"];
                    }
                    reader.Close();
                    if (this.cbof_ControlSegID.Items.Count > 0)
                    {
                        this.cbof_ControlSegID.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
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
                string cmdText = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
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
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , 0 as f_Selected, b.f_ZoneID, a.f_ControllerID, a.f_DoorNO,b.f_ControllerSN ";
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

        private DataTable loadUserData4BackWork()
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("loadUserData Start");
            icConsumerShare.loadUserData();
            return icConsumerShare.getDt();
        }

        private void loadUserData4BackWorkComplete(DataTable dtUser)
        {
            this.dv = new DataView(dtUser);
            this.dvSelected = new DataView(dtUser);
            try
            {
                this.dv.RowFilter = string.Format("f_DoorEnabled > 0 AND  {0}", icConsumerShare.getOptionalRowfilter());
                this.dvSelected.RowFilter = string.Format("f_DoorEnabled > 0 AND {0}", icConsumerShare.getSelectedRowfilter());
                this.dgvUsers.AutoGenerateColumns = false;
                this.dgvUsers.DataSource = this.dv;
                this.dgvSelectedUsers.AutoGenerateColumns = false;
                this.dgvSelectedUsers.DataSource = this.dvSelected;
                for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dgvUsers.ColumnCount); i++)
                {
                    this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                    this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            wgTools.WriteLine("loadUserData End");
            this.cbof_GroupID_SelectedIndexChanged(null, null);
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
            int num;
            string str = "";
            for (num = 0; num <= (Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedUsers.RowCount) - 1); num++)
            {
                str = str + ((DataView) this.dgvSelectedUsers.DataSource)[num]["f_ConsumerName"] + ",";
            }
            if (this.dgvSelectedUsers.RowCount > wgAppConfig.LogEventMaxCount)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "......(", this.dgvSelectedUsers.RowCount, ")" });
            }
            else
            {
                object obj3 = str;
                str = string.Concat(new object[] { obj3, "(", this.dgvSelectedUsers.RowCount, ")" });
            }
            string str2 = "";
            for (num = 0; num <= (Math.Min(wgAppConfig.LogEventMaxCount, this.dgvSelectedDoors.RowCount) - 1); num++)
            {
                str2 = str2 + ((DataView) this.dgvSelectedDoors.DataSource)[num]["f_DoorName"] + ",";
            }
            if (this.dgvSelectedDoors.RowCount > wgAppConfig.LogEventMaxCount)
            {
                object obj4 = str2;
                str2 = string.Concat(new object[] { obj4, "......(", this.dgvSelectedDoors.RowCount, ")" });
            }
            else
            {
                object obj5 = str2;
                str2 = string.Concat(new object[] { obj5, "(", this.dgvSelectedDoors.RowCount, ")" });
            }
            wgAppConfig.wgLog(string.Format("{0}:[{1} => {2}]:{3} => {4}", new object[] { (sender as Button).Text.Replace("\r\n", ""), this.dgvSelectedUsers.RowCount.ToString(), this.dgvSelectedDoors.RowCount.ToString(), str, str2 }), EventLogEntryType.Information, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.bStarting)
                {
                    if ((this.progressBar1.Value != 0) && (this.progressBar1.Value != this.progressBar1.Maximum))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }
                else if (this.dgvUsers.DataSource == null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    this.lblWait.Visible = false;
                    this.groupBox1.Enabled = true;
                    this.bStarting = false;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }
    }
}

