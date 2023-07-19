namespace WG3000_COMM.ExtendFunc.Patrol
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmRouteEdit : frmBioAccess
    {
        private int colf_ReaderID = 3;
        private int colf_Sn = 2;
        public int currentRouteID;
        private string datetimeFirstPatrol;
        private DataSet ds = new DataSet("dsMeal");
        private DataTable dt;
        private DataView dv;
        private DataView dvSelected;
        private int routeSn = -1;

        public dfrmRouteEdit()
        {
            this.InitializeComponent();
        }

        private void btnAddAllReaders_Click(object sender, EventArgs e)
        {
            this.dgvOptional.SelectAll();
            this.btnAddOneReader_Click(sender, e);
            this.dgvOptional.ClearSelection();
        }

        private void btnAddOneReader_Click(object sender, EventArgs e)
        {
            int rowIndex;
            this.saveDefaultValue();
            int count = this.dgvSelected.Rows.Count;
            if ((this.routeSn < 0) && (this.dgvSelected.Rows.Count > 0))
            {
                foreach (DataRowView view in this.dgvSelected.DataSource as DataView)
                {
                    if (this.routeSn < ((int) view["f_Sn"]))
                    {
                        this.routeSn = (int) view["f_Sn"];
                    }
                }
                this.routeSn++;
            }
            if (this.routeSn <= 0)
            {
                this.routeSn = 1;
            }
            if (this.dgvSelected.Rows.Count <= 0)
            {
                this.datetimeFirstPatrol = this.dtpTime.Value.ToString("HH:mm");
                this.routeSn = 1;
                this.radioButton2.Checked = false;
                this.radioButton1.Checked = true;
            }
            else
            {
                this.datetimeFirstPatrol = (this.dgvSelected.DataSource as DataView)[0]["f_patroltime"].ToString();
                if ((this.dgvSelected.DataSource as DataView)[0]["f_NextDay"].ToString() == "1")
                {
                    XMessageBox.Show(CommonStr.strPatrolPointFirstTimeNextDay);
                    return;
                }
                if (string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.ToString("HH:mm")) > 0)
                {
                    this.radioButton2.Checked = true;
                }
                else
                {
                    if ((string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.ToString("HH:mm")) == 0) && (this.dtpTime.Value.ToString("HH:mm") == "00:00"))
                    {
                        XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
                        return;
                    }
                    this.radioButton2.Checked = false;
                }
            }
            DataGridView dgvOptional = this.dgvOptional;
            if (dgvOptional.SelectedRows.Count <= 0)
            {
                if (dgvOptional.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = dgvOptional.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = dgvOptional.SelectedRows[0].Index;
            }
            DataTable table = ((DataView) this.dgvSelected.DataSource).Table;
            using (DataTable table2 = ((DataView) dgvOptional.DataSource).Table)
            {
                DataRow row;
                if (dgvOptional.SelectedRows.Count > 0)
                {
                    int num2 = dgvOptional.SelectedRows.Count;
                    int[] numArray = new int[num2];
                    int index = 0;
                    for (int i = 0; i < dgvOptional.Rows.Count; i++)
                    {
                        if (dgvOptional.Rows[i].Selected)
                        {
                            numArray[index] = (int) dgvOptional.Rows[i].Cells[this.colf_ReaderID].Value;
                            index++;
                        }
                    }
                    for (int j = 0; j < num2; j++)
                    {
                        int key = numArray[j];
                        row = table2.Rows.Find(key);
                        if (row != null)
                        {
                            row["f_NextDay"] = this.radioButton2.Checked ? 1 : 0;
                            row["f_patroltime"] = this.dtpTime.Value.ToString("HH:mm");
                            DataRow row2 = table.NewRow();
                            for (int k = 0; k < table2.Columns.Count; k++)
                            {
                                row2[k] = row[k];
                            }
                            row2["f_Sn"] = this.routeSn;
                            this.routeSn++;
                            table.Rows.Add(row2);
                            if (this.chkAutoAdd.Checked)
                            {
                                if (this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value).Date == this.dtpTime.Value.Date)
                                {
                                    if (this.radioButton2.Checked && (string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value).ToString("HH:mm")) <= 0))
                                    {
                                        XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
                                        goto Label_078C;
                                    }
                                    this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value);
                                }
                                else if (!this.radioButton2.Checked)
                                {
                                    this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value);
                                    this.radioButton2.Checked = true;
                                }
                                else
                                {
                                    XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
                                    goto Label_078C;
                                }
                            }
                        }
                    }
                }
                else
                {
                    int num8 = (int) dgvOptional.Rows[rowIndex].Cells[this.colf_ReaderID].Value;
                    row = table2.Rows.Find(num8);
                    if (row != null)
                    {
                        row["f_NextDay"] = this.radioButton2.Checked ? 1 : 0;
                        row["f_patroltime"] = this.dtpTime.Value.ToString("HH:mm");
                        DataRow row3 = table.NewRow();
                        for (int m = 0; m < table2.Columns.Count; m++)
                        {
                            row3[m] = row[m];
                        }
                        row3["f_Sn"] = this.routeSn;
                        this.routeSn++;
                        table.Rows.Add(row3);
                        if (this.chkAutoAdd.Checked)
                        {
                            if (this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value).Date == this.dtpTime.Value.Date)
                            {
                                if (this.radioButton2.Checked && (string.Compare(this.datetimeFirstPatrol, this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value).ToString("HH:mm")) <= 0))
                                {
                                    XMessageBox.Show(CommonStr.strPatrolPointAddFailed);
                                    table.AcceptChanges();
                                    return;
                                }
                                this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value);
                            }
                            else if (!this.radioButton2.Checked)
                            {
                                this.dtpTime.Value = this.dtpTime.Value.AddMinutes((double) this.nudMinute.Value);
                                this.radioButton2.Checked = true;
                            }
                        }
                    }
                }
            }
        Label_078C:
            table.AcceptChanges();
        }

        private void btnCopyFromOtherRoute_Click(object sender, EventArgs e)
        {
            using (dfrmPatrolTaskEdit edit = new dfrmPatrolTaskEdit())
            {
                if (edit.ShowDialog() == DialogResult.OK)
                {
                    int routeID = edit.routeID;
                    if (routeID != 0)
                    {
                        this.ds.Tables["selectedReader"].Clear();
                        string cmdText = "Select   f_NextDay, f_patroltime, f_Sn, t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 0 as f_Selected from t_d_PatrolRouteDetail,t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_PatrolRouteDetail.f_ReaderID and t_d_PatrolRouteDetail.f_RouteID = " + routeID.ToString();
                        if (wgAppConfig.IsAccessDB)
                        {
                            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                            {
                                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                                {
                                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                                    {
                                        adapter.Fill(this.ds, "selectedReader");
                                    }
                                }
                                return;
                            }
                        }
                        using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                        {
                            using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                            {
                                using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                                {
                                    adapter2.Fill(this.ds, "selectedReader");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnDeleteAllReaders_Click(object sender, EventArgs e)
        {
            this.dgvSelected.SelectAll();
            this.btnDeleteOneReader_Click(sender, e);
        }

        private void btnDeleteOneReader_Click(object sender, EventArgs e)
        {
            DataGridView dgvSelected = this.dgvSelected;
            try
            {
                int rowIndex;
                if (dgvSelected.SelectedRows.Count <= 0)
                {
                    if (dgvSelected.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgvSelected.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgvSelected.SelectedRows[0].Index;
                }
                using (DataTable table = ((DataView) dgvSelected.DataSource).Table)
                {
                    DataRow row;
                    if (dgvSelected.SelectedRows.Count > 0)
                    {
                        int count = dgvSelected.SelectedRows.Count;
                        int[] numArray = new int[count];
                        for (int i = 0; i < dgvSelected.SelectedRows.Count; i++)
                        {
                            numArray[i] = (int) dgvSelected.SelectedRows[i].Cells[this.colf_Sn].Value;
                        }
                        for (int j = 0; j < count; j++)
                        {
                            int key = numArray[j];
                            row = table.Rows.Find(key);
                            if (row != null)
                            {
                                row.Delete();
                            }
                        }
                        table.AcceptChanges();
                    }
                    else
                    {
                        int num6 = (int) dgvSelected.Rows[rowIndex].Cells[this.colf_Sn].Value;
                        row = table.Rows.Find(num6);
                        if (row != null)
                        {
                            row["f_Selected"] = 0;
                            row.Delete();
                        }
                        table.AcceptChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnStartTimeUpdate_Click(object sender, EventArgs e)
        {
            DataTable table = ((DataView) this.dgvSelected.DataSource).Table;
            DataView dataSource = (DataView) this.dgvSelected.DataSource;
            if (dataSource.Count > 0)
            {
                if ((this.dgvSelected.DataSource as DataView)[0]["f_NextDay"].ToString() == "1")
                {
                    XMessageBox.Show(CommonStr.strPatrolPointFirstTimeNextDay);
                }
                else
                {
                    DateTime time = DateTime.Parse(this.dtpTime.Value.ToString("yyyy-MM-dd ") + dataSource[0]["f_patroltime"].ToString() + ":00");
                    TimeSpan span = this.dtpTime.Value.Subtract(time);
                    dataSource[0]["f_patroltime"] = this.dtpTime.Value.ToString("HH:mm");
                    time = DateTime.Parse(this.dtpTime.Value.ToString("yyyy-MM-dd HH:mm:00"));
                    DateTime time2 = time.AddDays(1.0);
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        DateTime time3 = DateTime.Parse(this.dtpTime.Value.AddDays((double) ((int) table.Rows[i]["f_NextDay"])).ToString("yyyy-MM-dd ") + table.Rows[i]["f_patroltime"].ToString() + ":00").AddMinutes(span.TotalMinutes);
                        if (time3 >= time2)
                        {
                            XMessageBox.Show(CommonStr.strPatrolErrPatrolTime);
                            table.AcceptChanges();
                            return;
                        }
                        table.Rows[i]["f_patroltime"] = time3.ToString("HH:mm");
                        table.Rows[i]["f_NextDay"] = (time3.Date != time.Date) ? 1 : 0;
                    }
                    table.AcceptChanges();
                }
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                XMessageBox.Show(CommonStr.strNameNotEmpty);
            }
            else if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                XMessageBox.Show(CommonStr.strNameNotEmpty);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    Cursor current = Cursor.Current;
                    if (this.currentRouteID <= 0)
                    {
                        this.currentRouteID = int.Parse(this.cbof_RouteID.Text);
                        wgAppConfig.runUpdateSql(string.Format(" INSERT INTO t_d_PatrolRouteList(f_RouteID, f_RouteName) VALUES({0},{1})", this.currentRouteID.ToString(), wgTools.PrepareStr(this.txtName.Text)));
                    }
                    wgAppConfig.runUpdateSql(" DELETE FROM  t_d_PatrolRouteDetail WHERE f_RouteID = " + this.currentRouteID.ToString());
                    if (this.dvSelected.Count > 0)
                    {
                        for (int i = 0; i <= (this.dvSelected.Count - 1); i++)
                        {
                            wgAppConfig.runUpdateSql(((((("INSERT INTO t_d_PatrolRouteDetail (f_RouteID, f_Sn, f_ReaderID, f_patroltime, f_NextDay) VALUES( " + this.currentRouteID.ToString()) + "," + ((i + 1)).ToString()) + "," + this.dvSelected[i]["f_ReaderID"].ToString()) + "," + wgTools.PrepareStr(this.dvSelected[i]["f_patroltime"].ToString())) + "," + this.dvSelected[i]["f_NextDay"].ToString()) + ") ");
                        }
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void dfrmMealOption_Load(object sender, EventArgs e)
        {
            int index;
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Format = DateTimePickerFormat.Custom;
            this.dtpTime.Value = DateTime.Parse("00:00:00");
            this.loadData();
            this.dgvOptional.AutoGenerateColumns = false;
            this.dgvOptional.DataSource = this.dv;
            this.dgvSelected.AutoGenerateColumns = false;
            this.dgvSelected.DataSource = this.dvSelected;
            this.dvSelected.Sort = "f_NextDay ASC, f_patroltime asc, f_Sn asc";
            this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
            this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
            this.dt = this.ds.Tables["optionalReader"];
            try
            {
                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[this.colf_ReaderID] };
            }
            catch (Exception)
            {
                throw;
            }
            this.dt = this.ds.Tables["selectedReader"];
            try
            {
                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[this.colf_Sn] };
            }
            catch (Exception)
            {
                throw;
            }
            for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
            {
                this.dgvOptional.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                this.dgvSelected.Columns[i].DataPropertyName = this.dvSelected.Table.Columns[i].ColumnName;
            }
            this.cbof_RouteID.Items.Clear();
            for (int j = 1; j <= 0x63; j++)
            {
                this.cbof_RouteID.Items.Add(j);
            }
            string str = "";
            if (this.currentRouteID <= 0)
            {
                str = "New";
            }
            if (!(str == "New"))
            {
                this.cbof_RouteID.Enabled = false;
                this.cbof_RouteID.Text = this.currentRouteID.ToString();
                string str3 = " SELECT * FROM t_d_PatrolRouteList WHERE [f_RouteID]= " + this.currentRouteID.ToString();
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command3 = new OleDbCommand(str3, connection3))
                        {
                            connection3.Open();
                            OleDbDataReader reader3 = command3.ExecuteReader();
                            if (reader3.Read())
                            {
                                this.txtName.Text = wgTools.SetObjToStr(reader3["f_RouteName"].ToString());
                            }
                            reader3.Close();
                        }
                        goto Label_04DF;
                    }
                }
                using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command4 = new SqlCommand(str3, connection4))
                    {
                        connection4.Open();
                        SqlDataReader reader4 = command4.ExecuteReader();
                        if (reader4.Read())
                        {
                            this.txtName.Text = wgTools.SetObjToStr(reader4["f_RouteName"].ToString());
                        }
                        reader4.Close();
                    }
                }
                goto Label_04DF;
            }
            this.ds.Tables["selectedReader"].Clear();
            this.cbof_RouteID.Enabled = true;
            string cmdText = "SELECT f_RouteID FROM t_d_PatrolRouteList  ORDER BY [f_RouteID] ASC ";
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            index = this.cbof_RouteID.Items.IndexOf((int) reader[0]);
                            if (index >= 0)
                            {
                                this.cbof_RouteID.Items.RemoveAt(index);
                            }
                        }
                        reader.Close();
                    }
                    goto Label_0376;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        index = this.cbof_RouteID.Items.IndexOf((int) reader2[0]);
                        if (index >= 0)
                        {
                            this.cbof_RouteID.Items.RemoveAt(index);
                        }
                    }
                    reader2.Close();
                }
            }
        Label_0376:
            if (this.cbof_RouteID.Items.Count == 0)
            {
                base.Close();
            }
            this.cbof_RouteID.Text = this.cbof_RouteID.Items[0].ToString();
        Label_04DF:
            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("RouteEditAutoIncrease")))
            {
                if (wgAppConfig.GetKeyVal("RouteEditAutoIncrease") == "0")
                {
                    this.chkAutoAdd.Checked = false;
                }
                else
                {
                    this.chkAutoAdd.Checked = true;
                    try
                    {
                        this.nudMinute.Value = decimal.Parse(wgAppConfig.GetKeyVal("RouteEditAutoIncrease"));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("RouteEditStartTime")))
            {
                try
                {
                    this.dtpTime.Value = DateTime.Parse(wgAppConfig.GetKeyVal("RouteEditStartTime"));
                }
                catch (Exception)
                {
                }
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

        public void loadData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadData_Acc();
            }
            else
            {
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    SqlCommand selectCommand = new SqlCommand("Select  0 as f_NextDay,'' as f_patroltime, -1 as f_Sn, t_b_reader.f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader, t_b_Reader4Patrol  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader4Patrol.f_ReaderID = t_b_Reader.f_ReaderID  ", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(this.ds, "optionalReader");
                    selectCommand = new SqlCommand("Select   f_NextDay, f_patroltime, f_Sn, t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 0 as f_Selected from t_d_PatrolRouteDetail,t_b_reader,t_b_Reader4Patrol   , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_PatrolRouteDetail.f_ReaderID  AND t_b_Reader.f_ReaderID = t_b_Reader4Patrol.f_ReaderID  and t_d_PatrolRouteDetail.f_RouteID = " + this.currentRouteID.ToString(), connection);
                    new SqlDataAdapter(selectCommand).Fill(this.ds, "selectedReader");
                    this.dv = new DataView(this.ds.Tables["optionalReader"]);
                    this.dvSelected = new DataView(this.ds.Tables["selectedReader"]);
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        public void loadData_Acc()
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                OleDbCommand selectCommand = new OleDbCommand("Select  0 as f_NextDay,'' as f_patroltime, -1 as f_Sn, t_b_reader.f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader, t_b_Reader4Patrol  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND  t_b_Reader4Patrol.f_ReaderID = t_b_Reader.f_ReaderID  ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                selectCommand = new OleDbCommand("Select   f_NextDay, f_patroltime, f_Sn, t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 0 as f_Selected from t_d_PatrolRouteDetail,t_b_reader, t_b_Reader4Patrol  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_PatrolRouteDetail.f_ReaderID  AND t_b_Reader.f_ReaderID = t_b_Reader4Patrol.f_ReaderID  and t_d_PatrolRouteDetail.f_RouteID = " + this.currentRouteID.ToString(), connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "selectedReader");
                this.dv = new DataView(this.ds.Tables["optionalReader"]);
                this.dvSelected = new DataView(this.ds.Tables["selectedReader"]);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void saveDefaultValue()
        {
            if (this.dgvSelected.Rows.Count == 0)
            {
                wgAppConfig.UpdateKeyVal("RouteEditStartTime", this.dtpTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                if (this.chkAutoAdd.Checked)
                {
                    wgAppConfig.UpdateKeyVal("RouteEditAutoIncrease", this.nudMinute.Value.ToString());
                }
                else
                {
                    wgAppConfig.UpdateKeyVal("RouteEditAutoIncrease", "0");
                }
            }
        }
    }
}

