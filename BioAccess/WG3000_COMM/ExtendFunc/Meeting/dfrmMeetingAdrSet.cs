namespace WG3000_COMM.ExtendFunc.Meeting
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMeetingAdrSet : frmBioAccess
    {
        public string curMeetingAdr = "";
        private DataSet ds = new DataSet("dsMeetingAdr");
        private DataTable dt;
        private DataView dv;
        private DataView dvSelected;

        public dfrmMeetingAdrSet()
        {
            this.InitializeComponent();
        }

        private void btnAddAllReaders_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 1;
                }
                this.dt.AcceptChanges();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnAddOneReader_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvOptional);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnDeleteAllReaders_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 0;
                }
                this.dt.AcceptChanges();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnDeleteOneReader_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelected);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
            }
            else
            {
                try
                {
                    this.txtMeetingAdr.Text = this.txtMeetingAdr.Text.Trim();
                    if (this.txtMeetingAdr.Text == "")
                    {
                        XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
                    }
                    else if (this.dvSelected.Count <= 0)
                    {
                        XMessageBox.Show(CommonStr.strMeetingSelectReaderAsSign);
                    }
                    else
                    {
                        if (!this.txtMeetingAdr.ReadOnly)
                        {
                            SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                            SqlCommand command = new SqlCommand();
                            command = new SqlCommand(" SELECT * FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.txtMeetingAdr.Text), connection);
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                reader.Close();
                                XMessageBox.Show(CommonStr.strMeetingNameIsDupliated);
                                return;
                            }
                            reader.Close();
                        }
                        Cursor current = Cursor.Current;
                        wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.txtMeetingAdr.Text));
                        if (this.dvSelected.Count > 0)
                        {
                            for (int i = 0; i <= (this.dvSelected.Count - 1); i++)
                            {
                                string str = " INSERT INTO t_d_MeetingAdr";
                                object obj2 = str + " (f_MeetingAdr, f_ReaderID) ";
                                wgAppConfig.runUpdateSql(string.Concat(new object[] { obj2, " Values(", wgTools.PrepareStr(this.txtMeetingAdr.Text), ",", this.dvSelected[i]["f_ReaderID"] }) + " )");
                            }
                        }
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            try
            {
                this.txtMeetingAdr.Text = this.txtMeetingAdr.Text.Trim();
                if (this.txtMeetingAdr.Text == "")
                {
                    XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
                }
                else if (this.dvSelected.Count <= 0)
                {
                    XMessageBox.Show(CommonStr.strMeetingSelectReaderAsSign);
                }
                else
                {
                    if (!this.txtMeetingAdr.ReadOnly)
                    {
                        OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                        OleDbCommand command = new OleDbCommand();
                        command = new OleDbCommand(" SELECT * FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.txtMeetingAdr.Text), connection);
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            reader.Close();
                            XMessageBox.Show(CommonStr.strMeetingNameIsDupliated);
                            return;
                        }
                        reader.Close();
                    }
                    Cursor current = Cursor.Current;
                    wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.txtMeetingAdr.Text));
                    if (this.dvSelected.Count > 0)
                    {
                        for (int i = 0; i <= (this.dvSelected.Count - 1); i++)
                        {
                            string str = " INSERT INTO t_d_MeetingAdr";
                            object obj2 = str + " (f_MeetingAdr, f_ReaderID) ";
                            wgAppConfig.runUpdateSql(string.Concat(new object[] { obj2, " Values(", wgTools.PrepareStr(this.txtMeetingAdr.Text), ",", this.dvSelected[i]["f_ReaderID"] }) + " )");
                        }
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmMeetingAdr_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmMeetingAdr_Load_Acc(sender, e);
            }
            else
            {
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                SqlCommand selectCommand = new SqlCommand();
                try
                {
                    SqlDataAdapter adapter;
                    if (this.curMeetingAdr == "")
                    {
                        selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )     ", connection);
                        adapter = new SqlDataAdapter(selectCommand);
                        adapter.Fill(this.ds, "optionalReader");
                    }
                    else
                    {
                        this.txtMeetingAdr.Text = this.curMeetingAdr;
                        this.txtMeetingAdr.ReadOnly = true;
                        selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.curMeetingAdr) + ") ", connection);
                        adapter = new SqlDataAdapter(selectCommand);
                        adapter.Fill(this.ds, "optionalReader");
                        selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.curMeetingAdr) + ") ", connection);
                        new SqlDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                    }
                    this.dv = new DataView(this.ds.Tables["optionalReader"]);
                    this.dv.RowFilter = " f_Selected = 0";
                    this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
                    this.dvSelected.RowFilter = " f_Selected = 1";
                    this.dt = this.ds.Tables["optionalReader"];
                    try
                    {
                        this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
                    {
                        this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                        this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                    }
                    this.dv.RowFilter = "f_Selected = 0";
                    this.dvSelected.RowFilter = "f_Selected > 0";
                    this.dgvOptional.AutoGenerateColumns = false;
                    this.dgvOptional.DataSource = this.dv;
                    this.dgvSelected.AutoGenerateColumns = false;
                    this.dgvSelected.DataSource = this.dvSelected;
                    this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
                    this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        private void dfrmMeetingAdr_Load_Acc(object sender, EventArgs e)
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            OleDbCommand selectCommand = new OleDbCommand();
            try
            {
                OleDbDataAdapter adapter;
                if (this.curMeetingAdr == "")
                {
                    selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )   ", connection);
                    adapter = new OleDbDataAdapter(selectCommand);
                    adapter.Fill(this.ds, "optionalReader");
                }
                else
                {
                    this.txtMeetingAdr.Text = this.curMeetingAdr;
                    this.txtMeetingAdr.ReadOnly = true;
                    selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID NOT IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.curMeetingAdr) + ") ", connection);
                    adapter = new OleDbDataAdapter(selectCommand);
                    adapter.Fill(this.ds, "optionalReader");
                    selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  IN (SELECT t_d_MeetingAdr.f_ReaderID FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(this.curMeetingAdr) + ") ", connection);
                    new OleDbDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                }
                this.dv = new DataView(this.ds.Tables["optionalReader"]);
                this.dv.RowFilter = " f_Selected = 0";
                this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
                this.dvSelected.RowFilter = " f_Selected = 1";
                this.dt = this.ds.Tables["optionalReader"];
                try
                {
                    this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                }
                catch (Exception)
                {
                    throw;
                }
                for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
                {
                    this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                    this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                }
                this.dv.RowFilter = "f_Selected = 0";
                this.dvSelected.RowFilter = "f_Selected > 0";
                this.dgvOptional.AutoGenerateColumns = false;
                this.dgvOptional.DataSource = this.dv;
                this.dgvSelected.AutoGenerateColumns = false;
                this.dgvSelected.DataSource = this.dvSelected;
                this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
                this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dgvOptional_DoubleClick(object sender, EventArgs e)
        {
            this.btnAddOneReader.PerformClick();
        }

        private void dgvSelected_DoubleClick(object sender, EventArgs e)
        {
            this.btnDeleteOneReader.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

