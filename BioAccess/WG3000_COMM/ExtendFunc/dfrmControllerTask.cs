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
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControllerTask : frmBioAccess
    {
        private ArrayList arrDoorID = new ArrayList();
        private DataView dvDoors;

        public dfrmControllerTask()
        {
            this.InitializeComponent();
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = !this.chk1.Visible || this.chk1.Checked;
        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox2.Enabled = !this.chk2.Visible || this.chk2.Checked;
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox3.Enabled = !this.chk3.Visible || this.chk3.Checked;
        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox4.Enabled = !this.chk4.Visible || this.chk4.Checked;
        }

        private void chk5_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox5.Enabled = !this.chk5.Visible || this.chk5.Checked;
        }

        private void chk6_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox6.Enabled = !this.chk6.Visible || this.chk6.Checked;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.groupBox1.Enabled && (this.dtpBegin.Value.Date > this.dtpEnd.Value.Date))
                {
                    XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (((this.groupBox3.Enabled && !this.checkBox43.Checked) && (!this.checkBox44.Checked && !this.checkBox45.Checked)) && ((!this.checkBox46.Checked && !this.checkBox47.Checked) && (!this.checkBox48.Checked && !this.checkBox49.Checked)))
                {
                    XMessageBox.Show(CommonStr.strTimeInvalidParm, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (this.groupBox4.Enabled && (this.cboDoors.SelectedIndex < 0))
                {
                    XMessageBox.Show(this.label2.Text + "...?", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (this.txtTaskIDs.Text.IndexOf("(") < 0)
                    {
                        string strSql = " UPDATE t_b_ControllerTaskList SET ";
                        strSql = ((((((((((strSql + " f_BeginYMD =" + this.getDateString(this.dtpBegin)) + " , f_EndYMD =" + this.getDateString(this.dtpEnd)) + " , f_OperateTime = " + this.getDateString(this.dtpTime)) + " , f_Monday =" + (this.checkBox43.Checked ? "1" : "0")) + " , f_Tuesday = " + (this.checkBox44.Checked ? "1" : "0")) + " , f_Wednesday = " + (this.checkBox45.Checked ? "1" : "0")) + " , f_Thursday = " + (this.checkBox46.Checked ? "1" : "0")) + " , f_Friday = " + (this.checkBox47.Checked ? "1" : "0")) + " , f_Saturday = " + (this.checkBox48.Checked ? "1" : "0")) + " , f_Sunday = " + (this.checkBox49.Checked ? "1" : "0")) + " , f_DoorID = " + this.arrDoorID[this.cboDoors.SelectedIndex];
                        if (this.cboAccessMethod.SelectedIndex < 0)
                            this.cboAccessMethod.SelectedIndex = 0;
                        
                        // Consider 3 omitted items
                        int index = this.cboAccessMethod.SelectedIndex;
                        //if (index >= 5) index += 3;

                        strSql = ((strSql + " , f_DoorControl = " + index) + " , f_Notes = " + wgTools.PrepareStr(this.txtNote.Text.Trim())) + " WHERE [f_Id]= " + this.txtTaskIDs.Text;
                        if (wgAppConfig.runUpdateSql(strSql) <= 0)
                        {
                            return;
                        }
                        wgAppConfig.wgLog(string.Format("{0} {1}:{2} [{3}]", new object[] { this.Text, this.lblTaskID.Text, this.txtTaskIDs.Text, strSql }));
                    }
                    else
                    {
                        string str5 = "";
                        string str4 = " UPDATE t_b_ControllerTaskList SET ";
                        str5 = str5 + (!this.chk1.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_BeginYMD =" + this.getDateString(this.dtpBegin)));
                        str5 = str5 + (!this.chk1.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_EndYMD =" + this.getDateString(this.dtpEnd)));
                        str5 = str5 + (!this.chk2.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_OperateTime = " + this.getDateString(this.dtpTime)));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Monday =" + (this.checkBox43.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Tuesday = " + (this.checkBox44.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Wednesday = " + (this.checkBox45.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Thursday = " + (this.checkBox46.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Friday = " + (this.checkBox47.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Saturday = " + (this.checkBox48.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk3.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_Sunday = " + (this.checkBox49.Checked ? "1" : "0")));
                        str5 = str5 + (!this.chk4.Checked ? "" : (((str5 == "") ? " " : " , ") + " f_DoorID = " + this.arrDoorID[this.cboDoors.SelectedIndex]));
                        if (this.cboAccessMethod.SelectedIndex < 0)
                            this.cboAccessMethod.SelectedIndex = 0;

                        // Consider 3 omitted items
                        int index = this.cboAccessMethod.SelectedIndex;
                        //if (index >= 5) index += 3;

                        str5 = str5 + (!this.chk5.Checked ? "" : (((str5 == "") ? " " : " , ") + "  f_DoorControl = " + index));
                        str5 = str5 + (!this.chk6.Checked ? "" : (((str5 == "") ? " " : " , ") + "  f_Notes = " + wgTools.PrepareStr(this.txtNote.Text.Trim())));
                        if (str5 != "")
                        {
                            str4 = (str4 + str5) + " WHERE [f_Id] IN " + this.txtTaskIDs.Text;
                            if (wgAppConfig.runUpdateSql(str4) > 0)
                            {
                                wgAppConfig.wgLog(string.Format("{0} {1}:{2} [{3}]", new object[] { this.Text, this.lblTaskID.Text, this.txtTaskIDs.Text, str4 }));
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void dfrmControllerTask_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmControllerTask_Load_Acc(sender, e);
            }
            else
            {
                this.dtpBegin.Value = DateTime.Now.Date;
                this.dtpTime.CustomFormat = "HH:mm";
                this.dtpTime.Format = DateTimePickerFormat.Custom;
                this.dtpTime.Value = DateTime.Parse("00:00:00");
                this.loadDoorData();
                if (this.cboAccessMethod.Items.Count > 0)
                    this.cboAccessMethod.SelectedIndex = 0;
                
                if (!string.IsNullOrEmpty(this.txtTaskIDs.Text))
                {
                    if (this.txtTaskIDs.Text.IndexOf("(") < 0)
                    {
                        this.groupBox6.Visible = true;
                        string cmdText = " SELECT * FROM t_b_ControllerTaskList WHERE [f_Id]= " + this.txtTaskIDs.Text;
                        using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                        {
                            using (SqlCommand command = new SqlCommand(cmdText, connection))
                            {
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();
                                if (reader.Read())
                                {
                                    this.dtpBegin.Value = DateTime.Parse(wgTools.SetObjToStr(reader["f_BeginYMD"]));
                                    this.dtpEnd.Value = DateTime.Parse(wgTools.SetObjToStr(reader["f_EndYMD"]));
                                    this.dtpTime.Value = DateTime.Parse(wgTools.SetObjToStr(reader["f_OperateTime"]));
                                    this.checkBox43.Checked = wgTools.SetObjToStr(reader["f_Monday"]) == "1";
                                    this.checkBox44.Checked = wgTools.SetObjToStr(reader["f_Tuesday"]) == "1";
                                    this.checkBox45.Checked = wgTools.SetObjToStr(reader["f_Wednesday"]) == "1";
                                    this.checkBox46.Checked = wgTools.SetObjToStr(reader["f_Thursday"]) == "1";
                                    this.checkBox47.Checked = wgTools.SetObjToStr(reader["f_Friday"]) == "1";
                                    this.checkBox48.Checked = wgTools.SetObjToStr(reader["f_Saturday"]) == "1";
                                    this.checkBox49.Checked = wgTools.SetObjToStr(reader["f_Sunday"]) == "1";
                                    this.cboDoors.SelectedIndex = this.arrDoorID.IndexOf(int.Parse(wgTools.SetObjToStr(reader["f_DoorID"])));

                                    // Consider 3 omitted items
                                    int index = int.Parse(wgTools.SetObjToStr(reader["f_DoorControl"]));
                                    //if (index >= 10) index -= 5;
                                    this.cboAccessMethod.SelectedIndex = index;
                                    this.txtNote.Text = wgTools.SetObjToStr(reader["f_Notes"]);
                                }
                                reader.Close();
                            }
                            return;
                        }
                    }
                    this.chk1.Visible = true;
                    this.chk2.Visible = true;
                    this.chk3.Visible = true;
                    this.chk4.Visible = true;
                    this.chk5.Visible = true;
                    this.chk6.Visible = true;
                    this.groupBox1.Enabled = false;
                    this.groupBox2.Enabled = false;
                    this.groupBox3.Enabled = false;
                    this.groupBox4.Enabled = false;
                    this.groupBox5.Enabled = false;
                    this.groupBox6.Enabled = false;
                }
            }
        }

        private void dfrmControllerTask_Load_Acc(object sender, EventArgs e)
        {
            bool isAccessDB = wgAppConfig.IsAccessDB;
            this.dtpBegin.Value = DateTime.Now.Date;
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Format = DateTimePickerFormat.Custom;
            this.dtpTime.Value = DateTime.Parse("00:00:00");
            this.loadDoorData();
            if (this.cboAccessMethod.Items.Count > 0)
                this.cboAccessMethod.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(this.txtTaskIDs.Text))
            {
                if (this.txtTaskIDs.Text.IndexOf("(") < 0)
                {
                    this.groupBox6.Visible = true;
                    string cmdText = " SELECT * FROM t_b_ControllerTaskList WHERE [f_Id]= " + this.txtTaskIDs.Text;
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            connection.Open();
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                this.dtpBegin.Value = DateTime.Parse(wgTools.SetObjToStr(reader["f_BeginYMD"]));
                                this.dtpEnd.Value = DateTime.Parse(wgTools.SetObjToStr(reader["f_EndYMD"]));
                                this.dtpTime.Value = DateTime.Parse(wgTools.SetObjToStr(reader["f_OperateTime"]));
                                this.checkBox43.Checked = wgTools.SetObjToStr(reader["f_Monday"]) == "1";
                                this.checkBox44.Checked = wgTools.SetObjToStr(reader["f_Tuesday"]) == "1";
                                this.checkBox45.Checked = wgTools.SetObjToStr(reader["f_Wednesday"]) == "1";
                                this.checkBox46.Checked = wgTools.SetObjToStr(reader["f_Thursday"]) == "1";
                                this.checkBox47.Checked = wgTools.SetObjToStr(reader["f_Friday"]) == "1";
                                this.checkBox48.Checked = wgTools.SetObjToStr(reader["f_Saturday"]) == "1";
                                this.checkBox49.Checked = wgTools.SetObjToStr(reader["f_Sunday"]) == "1";
                                this.cboDoors.SelectedIndex = this.arrDoorID.IndexOf(int.Parse(wgTools.SetObjToStr(reader["f_DoorID"])));

                                // Consider 3 omitted items
                                int index = int.Parse(wgTools.SetObjToStr(reader["f_DoorControl"]));
                                //if (index >= 8) index -= 3;
                                this.cboAccessMethod.SelectedIndex = index;
                                this.txtNote.Text = wgTools.SetObjToStr(reader["f_Notes"]);
                            }
                            reader.Close();
                        }
                        return;
                    }
                }
                this.chk1.Visible = true;
                this.chk2.Visible = true;
                this.chk3.Visible = true;
                this.chk4.Visible = true;
                this.chk5.Visible = true;
                this.chk6.Visible = true;
                this.groupBox1.Enabled = false;
                this.groupBox2.Enabled = false;
                this.groupBox3.Enabled = false;
                this.groupBox4.Enabled = false;
                this.groupBox5.Enabled = false;
                this.groupBox6.Enabled = false;
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
    }
}

