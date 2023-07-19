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
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMultiCards : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private SqlCommand cmd;
        private SqlConnection cn;
        private int controllerID;
        private int controllerSN;
        public int DoorID;
        private int doorNo;
        private DataTable dt;
        private static DataTable dtLastLoad;
        private DataTable dtUser1;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvSelected;
        private static string lastLoadUsers = "";
        private const int MoreCardGroupMaxLen = 9;
        private int moreCards_GoInOut;
        public string retValue = "0";
        private string strGroupFilter = "";

        public dfrmMultiCards()
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

        private void btnAddAllUsers_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("btnAddAllUsers_Click Start");
            DataTable table = ((DataView) this.dgvUsers.DataSource).Table;
            DataView dataSource = (DataView) this.dgvUsers.DataSource;
            DataView view2 = (DataView) this.dgvSelectedUsers.DataSource;
            this.dgvUsers.DataSource = null;
            this.dgvSelectedUsers.DataSource = null;
            if (this.strGroupFilter == "")
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (((int) table.Rows[i]["f_Selected"]) != 1)
                    {
                        table.Rows[i]["f_Selected"] = 1;
                        table.Rows[i]["f_MoreCards_GrpID"] = this.nudGroupToAdd.Value;
                    }
                }
            }
            else
            {
                this.dv = new DataView(table);
                this.dv.RowFilter = this.strGroupFilter;
                for (int j = 0; j < this.dv.Count; j++)
                {
                    if (((int) this.dv[j]["f_Selected"]) != 1)
                    {
                        this.dv[j]["f_Selected"] = 1;
                        this.dv[j]["f_MoreCards_GrpID"] = this.nudGroupToAdd.Value;
                    }
                }
            }
            this.dgvUsers.DataSource = dataSource;
            this.dgvSelectedUsers.DataSource = view2;
            wgTools.WriteLine("btnAddAllUsers_Click End");
            Cursor.Current = Cursors.Default;
        }

        private void btnAddOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvUsers, "f_MoreCards_GrpID", this.nudGroupToAdd.Value.ToString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDelAllUsers_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("btnDelAllUsers_Click Start");
            this.dt = ((DataView) this.dgvUsers.DataSource).Table;
            this.dv1 = (DataView) this.dgvUsers.DataSource;
            this.dv2 = (DataView) this.dgvSelectedUsers.DataSource;
            this.dgvUsers.DataSource = null;
            this.dgvSelectedUsers.DataSource = null;
            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i]["f_Selected"] = 0;
            }
            this.dgvUsers.DataSource = this.dv1;
            this.dgvSelectedUsers.DataSource = this.dv2;
            wgTools.WriteLine("btnDelAllUsers_Click End");
            Cursor.Current = Cursors.Default;
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedUsers);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
            }
            else
            {
                Cursor current = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                if (this.cn.State == ConnectionState.Closed)
                {
                    this.cn.Open();
                }
                if ((wgMjController.GetControllerType(this.controllerSN) == 1) || (wgMjController.GetControllerType(this.controllerSN) == 2))
                {
                    if (this.chkReaderIn.Checked && this.chkActive.Checked)
                    {
                        this.moreCards_GoInOut |= ((int) 1) << ((this.doorNo - 1) * 2);
                    }
                    else
                    {
                        this.moreCards_GoInOut &= 0xff - (((int) 1) << ((this.doorNo - 1) * 2));
                    }
                    if (this.chkReaderOut.Checked && this.chkActive.Checked)
                    {
                        this.moreCards_GoInOut |= ((int) 1) << (((this.doorNo - 1) * 2) + 1);
                    }
                    else
                    {
                        this.moreCards_GoInOut &= 0xff - (((int) 1) << (((this.doorNo - 1) * 2) + 1));
                    }
                }
                else if (wgMjController.GetControllerType(this.controllerSN) == 4)
                {
                    if (this.chkActive.Checked)
                    {
                        this.moreCards_GoInOut |= ((int) 1) << (this.doorNo - 1);
                    }
                    else
                    {
                        this.moreCards_GoInOut &= 0xff - (((int) 1) << (this.doorNo - 1));
                    }
                }
                else if (this.chkActive.Checked)
                {
                    this.moreCards_GoInOut |= ((int) 1) << (this.doorNo - 1);
                }
                else
                {
                    this.moreCards_GoInOut &= 0xff - (((int) 1) << (this.doorNo - 1));
                }
                string cmdText = string.Concat(new object[] { "update t_b_Controller set f_MoreCards_GoInOut =", this.moreCards_GoInOut, " Where f_ControllerID = ", this.controllerID });
                this.cmd = new SqlCommand(cmdText, this.cn);
                this.cmd.ExecuteNonQuery();
                int num = 0;
                if (this.chkReadByOrder.Checked)
                {
                    num += 0x10;
                }
                if (this.chkSingleGroup.Checked)
                {
                    num += 8;
                    num += (int) (this.nudGrpStartOfSingle.Value - 1);
                }
                if (this.chkActive.Checked)
                {
                    cmdText = (((((((((("update t_b_door set f_MoreCards_Total =" + this.nudTotal.Value) + ", f_MoreCards_Grp1=" + this.nudGrp1.Value) + ", f_MoreCards_Grp2=" + this.nudGrp2.Value) + ", f_MoreCards_Grp3=" + this.nudGrp3.Value) + ", f_MoreCards_Grp4=" + this.nudGrp4.Value) + ", f_MoreCards_Grp5=" + this.nudGrp5.Value) + ", f_MoreCards_Grp6=" + this.nudGrp6.Value) + ", f_MoreCards_Grp7=" + this.nudGrp7.Value) + ", f_MoreCards_Grp8=" + this.nudGrp8.Value) + ", f_MoreCards_Option=" + num) + " Where f_DoorID = " + this.DoorID;
                }
                else
                {
                    cmdText = (((((((((("update t_b_door set f_MoreCards_Total =" + 0) + ", f_MoreCards_Grp1=" + 0) + ", f_MoreCards_Grp2=" + 0) + ", f_MoreCards_Grp3=" + 0) + ", f_MoreCards_Grp4=" + 0) + ", f_MoreCards_Grp5=" + 0) + ", f_MoreCards_Grp6=" + 0) + ", f_MoreCards_Grp7=" + 0) + ", f_MoreCards_Grp8=" + 0) + ", f_MoreCards_Option=" + 0) + " Where f_DoorID = " + this.DoorID;
                }
                this.cmd = new SqlCommand(cmdText, this.cn);
                this.cmd.ExecuteNonQuery();
                cmdText = " Delete  FROM t_d_doorMoreCardsUsers  WHERE f_DoorID= " + this.DoorID;
                this.cmd = new SqlCommand(cmdText, this.cn);
                this.cmd.ExecuteNonQuery();
                this.dvSelected = this.dgvSelectedUsers.DataSource as DataView;
                if ((this.chkActive.Checked && (this.nudTotal.Value > 0M)) && (this.dvSelected != null))
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        this.dvSelected.RowFilter = "f_Selected > 0 AND f_MoreCards_GrpID = " + i;
                        if (this.dvSelected.Count > 0)
                        {
                            for (int j = 0; j <= (this.dvSelected.Count - 1); j++)
                            {
                                cmdText = "INSERT INTO [t_d_doorMoreCardsUsers] (f_ConsumerID, f_DoorID ,f_MoreCards_GrpID)";
                                string str2 = cmdText + " VALUES( " + this.dvSelected[j]["f_ConsumerID"].ToString();
                                cmdText = str2 + ", " + this.DoorID.ToString() + "," + i.ToString() + ")";
                                this.cmd.CommandText = cmdText;
                                this.cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                if (this.chkActive.Checked)
                {
                    this.retValue = this.nudTotal.Value.ToString();
                }
                else
                {
                    this.retValue = "0";
                }
                if (this.cn.State == ConnectionState.Open)
                {
                    this.cn.Close();
                }
                base.DialogResult = DialogResult.OK;
                this.Cursor = current;
                base.Close();
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            OleDbConnection connection = null;
            OleDbCommand command = null;
            string str;
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            connection = new OleDbConnection(wgAppConfig.dbConString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            if ((wgMjController.GetControllerType(this.controllerSN) == 1) || (wgMjController.GetControllerType(this.controllerSN) == 2))
            {
                if (this.chkReaderIn.Checked && this.chkActive.Checked)
                {
                    this.moreCards_GoInOut |= ((int) 1) << ((this.doorNo - 1) * 2);
                }
                else
                {
                    this.moreCards_GoInOut &= 0xff - (((int) 1) << ((this.doorNo - 1) * 2));
                }
                if (this.chkReaderOut.Checked && this.chkActive.Checked)
                {
                    this.moreCards_GoInOut |= ((int) 1) << (((this.doorNo - 1) * 2) + 1);
                }
                else
                {
                    this.moreCards_GoInOut &= 0xff - (((int) 1) << (((this.doorNo - 1) * 2) + 1));
                }
            }
            else if (wgMjController.GetControllerType(this.controllerSN) == 4)
            {
                if (this.chkActive.Checked)
                {
                    this.moreCards_GoInOut |= ((int) 1) << (this.doorNo - 1);
                }
                else
                {
                    this.moreCards_GoInOut &= 0xff - (((int) 1) << (this.doorNo - 1));
                }
            }
            else if (this.chkActive.Checked)
            {
                this.moreCards_GoInOut |= ((int) 1) << (this.doorNo - 1);
            }
            else
            {
                this.moreCards_GoInOut &= 0xff - (((int) 1) << (this.doorNo - 1));
            }
            new OleDbCommand(string.Concat(new object[] { "update t_b_Controller set f_MoreCards_GoInOut =", this.moreCards_GoInOut, " Where f_ControllerID = ", this.controllerID }), connection).ExecuteNonQuery();
            int num = 0;
            if (this.chkReadByOrder.Checked)
            {
                num += 0x10;
            }
            if (this.chkSingleGroup.Checked)
            {
                num += 8;
                num += (int) (this.nudGrpStartOfSingle.Value - 1);
            }
            if (this.chkActive.Checked)
            {
                str = (((((((((("update t_b_door set f_MoreCards_Total =" + this.nudTotal.Value) + ", f_MoreCards_Grp1=" + this.nudGrp1.Value) + ", f_MoreCards_Grp2=" + this.nudGrp2.Value) + ", f_MoreCards_Grp3=" + this.nudGrp3.Value) + ", f_MoreCards_Grp4=" + this.nudGrp4.Value) + ", f_MoreCards_Grp5=" + this.nudGrp5.Value) + ", f_MoreCards_Grp6=" + this.nudGrp6.Value) + ", f_MoreCards_Grp7=" + this.nudGrp7.Value) + ", f_MoreCards_Grp8=" + this.nudGrp8.Value) + ", f_MoreCards_Option=" + num) + " Where f_DoorID = " + this.DoorID;
            }
            else
            {
                str = (((((((((("update t_b_door set f_MoreCards_Total =" + 0) + ", f_MoreCards_Grp1=" + 0) + ", f_MoreCards_Grp2=" + 0) + ", f_MoreCards_Grp3=" + 0) + ", f_MoreCards_Grp4=" + 0) + ", f_MoreCards_Grp5=" + 0) + ", f_MoreCards_Grp6=" + 0) + ", f_MoreCards_Grp7=" + 0) + ", f_MoreCards_Grp8=" + 0) + ", f_MoreCards_Option=" + 0) + " Where f_DoorID = " + this.DoorID;
            }
            new OleDbCommand(str, connection).ExecuteNonQuery();
            command = new OleDbCommand(" Delete  FROM t_d_doorMoreCardsUsers  WHERE f_DoorID= " + this.DoorID, connection);
            command.ExecuteNonQuery();
            this.dvSelected = this.dgvSelectedUsers.DataSource as DataView;
            if ((this.chkActive.Checked && (this.nudTotal.Value > 0M)) && (this.dvSelected != null))
            {
                for (int i = 1; i <= 9; i++)
                {
                    this.dvSelected.RowFilter = "f_Selected > 0 AND f_MoreCards_GrpID = " + i;
                    if (this.dvSelected.Count > 0)
                    {
                        for (int j = 0; j <= (this.dvSelected.Count - 1); j++)
                        {
                            str = "INSERT INTO [t_d_doorMoreCardsUsers] (f_ConsumerID, f_DoorID ,f_MoreCards_GrpID)";
                            string str2 = str + " VALUES( " + this.dvSelected[j]["f_ConsumerID"].ToString();
                            str = str2 + ", " + this.DoorID.ToString() + "," + i.ToString() + ")";
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            if (this.chkActive.Checked)
            {
                this.retValue = this.nudTotal.Value.ToString();
            }
            else
            {
                this.retValue = "0";
            }
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            base.DialogResult = DialogResult.OK;
            this.Cursor = current;
            base.Close();
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    dataSource.RowFilter = "f_Selected = 0";
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
                }
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkActive.Checked)
            {
                this.grpNeeded.Visible = true;
                if (this.grpOptInOut.Enabled)
                {
                    this.grpOptInOut.Visible = true;
                }
                this.groupBox1.Visible = true;
                if (this.dgvUsers.DataSource != null)
                {
                    DataView dataSource = (DataView) this.dgvUsers.DataSource;
                    this.dgvUsers.DataSource = null;
                    this.dgvUsers.DataSource = dataSource;
                }
            }
            else
            {
                this.grpNeeded.Visible = false;
                this.grpOptInOut.Visible = false;
                this.groupBox1.Visible = false;
            }
        }

        private void chkSingleGroup_CheckedChanged(object sender, EventArgs e)
        {
            this.nudGrpStartOfSingle.Visible = this.chkSingleGroup.Checked;
        }

        private void dfrmMultiCards_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.grpOption.Visible = true;
                }
            }
        }

        private void dfrmMultiCards_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmMultiCards_Load_Acc(sender, e);
            }
            else
            {
                this.loadGroupData();
                this.cn = new SqlConnection(wgAppConfig.dbConString);
                string cmdText = "SELECT t_b_Door.*,t_b_Controller.f_ControllerSN, t_b_Controller.f_MoreCards_GoInOut  FROM  t_b_Controller,t_b_Door  ";
                cmdText = cmdText + " Where  t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID and t_b_door.f_DoorID = " + this.DoorID;
                if (this.cn.State == ConnectionState.Closed)
                {
                    this.cn.Open();
                }
                this.cmd = new SqlCommand(cmdText, this.cn);
                SqlDataReader reader = this.cmd.ExecuteReader();
                if (reader.Read())
                {
                    this.controllerID = (int) reader["f_ControllerID"];
                    this.controllerSN = (int) reader["f_ControllerSN"];
                    this.moreCards_GoInOut = (int) reader["f_MoreCards_GoInOut"];
                    this.doorNo = int.Parse(reader["f_DoorNo"].ToString());
                    if ((wgMjController.GetControllerType(this.controllerSN) == 1) || (wgMjController.GetControllerType(this.controllerSN) == 2))
                    {
                        this.grpOptInOut.Visible = true;
                        int num = (this.moreCards_GoInOut >> ((this.doorNo - 1) * 2)) & 3;
                        this.chkReaderIn.Checked = (num & 1) > 0;
                        this.chkReaderOut.Checked = (num & 2) > 0;
                    }
                    else if (wgMjController.GetControllerType(this.controllerSN) == 4)
                    {
                        this.grpOptInOut.Visible = false;
                        this.chkReaderIn.Checked = true;
                        this.chkReaderIn.Enabled = false;
                        this.chkReaderOut.Visible = false;
                        this.grpOptInOut.Enabled = false;
                        this.grpOptInOut.Visible = false;
                    }
                    else
                    {
                        this.grpOptInOut.Visible = false;
                        this.chkReaderIn.Checked = true;
                        this.grpOptInOut.Enabled = false;
                        this.grpOptInOut.Visible = false;
                    }
                    if (((int) reader["f_MoreCards_Total"]) > 0)
                    {
                        this.chkActive.Checked = true;
                        this.chkActive_CheckedChanged(null, null);
                    }
                    else
                    {
                        this.chkActive.Checked = false;
                        this.chkActive_CheckedChanged(null, null);
                    }
                    if (((int) reader["f_MoreCards_Total"]) > 1)
                    {
                        this.nudTotal.Value = (int) reader["f_MoreCards_Total"];
                    }
                    this.nudGrp1.Value = (int) reader["f_MoreCards_Grp1"];
                    this.nudGrp2.Value = (int) reader["f_MoreCards_Grp2"];
                    this.nudGrp3.Value = (int) reader["f_MoreCards_Grp3"];
                    this.nudGrp4.Value = (int) reader["f_MoreCards_Grp4"];
                    this.nudGrp5.Value = (int) reader["f_MoreCards_Grp5"];
                    this.nudGrp6.Value = (int) reader["f_MoreCards_Grp6"];
                    this.nudGrp7.Value = (int) reader["f_MoreCards_Grp7"];
                    this.nudGrp8.Value = (int) reader["f_MoreCards_Grp8"];
                    int num2 = (int) reader["f_MoreCards_Option"];
                    if ((num2 & 0x10) > 0)
                    {
                        this.chkReadByOrder.Checked = true;
                    }
                    if ((num2 & 8) > 0)
                    {
                        this.chkSingleGroup.Checked = true;
                        this.nudGrpStartOfSingle.Value = 1 + (num2 & 7);
                        this.nudGrpStartOfSingle.Visible = true;
                    }
                    else
                    {
                        this.chkSingleGroup.Checked = false;
                        this.nudGrpStartOfSingle.Visible = false;
                    }
                    if (this.chkReadByOrder.Checked || this.chkSingleGroup.Checked)
                    {
                        this.grpOption.Visible = true;
                    }
                }
                reader.Close();
                this.cn.Close();
                this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
                this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void dfrmMultiCards_Load_Acc(object sender, EventArgs e)
        {
            this.loadGroupData();
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            string cmdText = "SELECT t_b_Door.*,t_b_Controller.f_ControllerSN, t_b_Controller.f_MoreCards_GoInOut  FROM  t_b_Controller,t_b_Door  ";
            cmdText = cmdText + " Where  t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID and t_b_door.f_DoorID = " + this.DoorID;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            OleDbDataReader reader = new OleDbCommand(cmdText, connection).ExecuteReader();
            if (reader.Read())
            {
                this.controllerID = (int) reader["f_ControllerID"];
                this.controllerSN = (int) reader["f_ControllerSN"];
                this.moreCards_GoInOut = (int) reader["f_MoreCards_GoInOut"];
                this.doorNo = int.Parse(reader["f_DoorNo"].ToString());
                if ((wgMjController.GetControllerType(this.controllerSN) == 1) || (wgMjController.GetControllerType(this.controllerSN) == 2))
                {
                    this.grpOptInOut.Visible = true;
                    int num = (this.moreCards_GoInOut >> ((this.doorNo - 1) * 2)) & 3;
                    this.chkReaderIn.Checked = (num & 1) > 0;
                    this.chkReaderOut.Checked = (num & 2) > 0;
                }
                else if (wgMjController.GetControllerType(this.controllerSN) == 4)
                {
                    this.grpOptInOut.Visible = false;
                    this.chkReaderIn.Checked = true;
                    this.chkReaderIn.Enabled = false;
                    this.chkReaderOut.Visible = false;
                    this.grpOptInOut.Enabled = false;
                    this.grpOptInOut.Visible = false;
                }
                else
                {
                    this.grpOptInOut.Visible = false;
                    this.chkReaderIn.Checked = true;
                    this.grpOptInOut.Enabled = false;
                    this.grpOptInOut.Visible = false;
                }
                if (((int) reader["f_MoreCards_Total"]) > 0)
                {
                    this.chkActive.Checked = true;
                    this.chkActive_CheckedChanged(null, null);
                }
                else
                {
                    this.chkActive.Checked = false;
                    this.chkActive_CheckedChanged(null, null);
                }
                if (((int) reader["f_MoreCards_Total"]) > 1)
                {
                    this.nudTotal.Value = (int) reader["f_MoreCards_Total"];
                }
                this.nudGrp1.Value = (int) reader["f_MoreCards_Grp1"];
                this.nudGrp2.Value = (int) reader["f_MoreCards_Grp2"];
                this.nudGrp3.Value = (int) reader["f_MoreCards_Grp3"];
                this.nudGrp4.Value = (int) reader["f_MoreCards_Grp4"];
                this.nudGrp5.Value = (int) reader["f_MoreCards_Grp5"];
                this.nudGrp6.Value = (int) reader["f_MoreCards_Grp6"];
                this.nudGrp7.Value = (int) reader["f_MoreCards_Grp7"];
                this.nudGrp8.Value = (int) reader["f_MoreCards_Grp8"];
                int num2 = (int) reader["f_MoreCards_Option"];
                if ((num2 & 0x10) > 0)
                {
                    this.chkReadByOrder.Checked = true;
                }
                if ((num2 & 8) > 0)
                {
                    this.chkSingleGroup.Checked = true;
                    this.nudGrpStartOfSingle.Value = 1 + (num2 & 7);
                    this.nudGrpStartOfSingle.Visible = true;
                }
                else
                {
                    this.chkSingleGroup.Checked = false;
                    this.nudGrpStartOfSingle.Visible = false;
                }
                if (this.chkReadByOrder.Checked || this.chkSingleGroup.Checked)
                {
                    this.grpOption.Visible = true;
                }
            }
            reader.Close();
            connection.Close();
            this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
            this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
            this.backgroundWorker1.RunWorkerAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadGroupData()
        {
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
        }

        private DataTable loadUserData4BackWork()
        {
            string str;
            Thread.Sleep(100);
            wgTools.WriteLine("loadUserData Start");
            this.dtUser1 = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT  t_b_Consumer.f_ConsumerID, ";
                str = (((str + " IIF ( f_MoreCards_GrpID IS NULL , 0 , f_MoreCards_GrpID ) AS f_MoreCards_GrpID " + " , f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , IIF (  f_MoreCards_GrpID IS NULL , 0 , 1 ) AS f_Selected " + " , f_GroupID ") + " FROM t_b_Consumer " + string.Format(" LEFT OUTER JOIN t_d_doorMoreCardsUsers ON ( t_b_Consumer.f_ConsumerID=t_d_doorMoreCardsUsers.f_ConsumerID AND f_DoorID= {0} )", this.DoorID.ToString())) + " WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUser1);
                        }
                    }
                    goto Label_01B2;
                }
            }
            str = " SELECT  t_b_Consumer.f_ConsumerID, ";
            str = ((((str + " CASE WHEN f_MoreCards_GrpID IS NULL THEN 0 ELSE f_MoreCards_GrpID END AS f_MoreCards_GrpID ") + " , f_ConsumerNO, f_ConsumerName, f_CardNO " + " , CASE WHEN f_MoreCards_GrpID IS NULL THEN 0 ELSE 1 END AS f_Selected ") + " , f_GroupID " + " FROM t_b_Consumer ") + " LEFT OUTER JOIN t_d_doorMoreCardsUsers ON t_b_Consumer.f_ConsumerID=t_d_doorMoreCardsUsers.f_ConsumerID AND f_DoorID= " + this.DoorID) + " WHERE f_DoorEnabled > 0" + " ORDER BY f_ConsumerNO ASC ";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtUser1);
                    }
                }
            }
        Label_01B2:
            wgTools.WriteLine("da.Fill End");
            try
            {
                this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
            }
            catch (Exception)
            {
                throw;
            }
            lastLoadUsers = icConsumerShare.getUpdateLog();
            dtLastLoad = this.dtUser1;
            return this.dtUser1;
        }

        private void loadUserData4BackWorkComplete(DataTable dtUser)
        {
            this.dv = new DataView(dtUser);
            this.dvSelected = new DataView(dtUser);
            this.dv.RowFilter = "f_Selected = 0";
            this.dvSelected.RowFilter = "f_Selected > 0";
            this.dvSelected.Sort = "f_MoreCards_GrpID ASC, f_ConsumerNo ASC ";
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            this.dgvSelectedUsers.AutoGenerateColumns = false;
            this.dgvSelectedUsers.DataSource = this.dvSelected;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
            }
            this.cbof_GroupID_SelectedIndexChanged(null, null);
            wgTools.WriteLine("loadUserData End");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource == null)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                this.timer1.Enabled = false;
                Cursor.Current = Cursors.Default;
                this.lblWait.Visible = false;
                this.groupBox1.Enabled = true;
                this.btnOK.Enabled = true;
            }
        }

        private decimal getTotalNeeded()
        {
            return nudGrp1.Value + nudGrp2.Value + 
                nudGrp3.Value + nudGrp4.Value +
                nudGrp5.Value + nudGrp6.Value + 
                nudGrp7.Value + nudGrp8.Value; 
        }

        private void nudGrp1_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp1.Value--;
        }

        private void nudGrp2_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp2.Value--;
        }

        private void nudGrp3_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp3.Value--;
        }

        private void nudGrp4_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp4.Value--;
        }

        private void nudGrp5_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp5.Value--;
        }

        private void nudGrp6_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp6.Value--;
        }

        private void nudGrp7_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp7.Value--;
        }

        private void nudGrp8_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
                nudGrp8.Value--;
        }

        private void nudTotal_ValueChanged(object sender, EventArgs e)
        {
            if (getTotalNeeded() > nudTotal.Value)
            {
                nudGrp1.Value = 0;
                nudGrp2.Value = 0;
                nudGrp3.Value = 0;
                nudGrp4.Value = 0;
                nudGrp5.Value = 0;
                nudGrp6.Value = 0;
                nudGrp7.Value = 0;
                nudGrp8.Value = 0;
            }
        }
    }
}

