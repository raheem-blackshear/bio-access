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
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmMeetingAdr : frmBioAccess
    {
        private DataSet ds = new DataSet("dsMeetingAdr");
        private DataView dv;

        public dfrmMeetingAdr()
        {
            this.InitializeComponent();
        }

        private void btnAddMeetingAdr_Click(object sender, EventArgs e)
        {
            try
            {
                dfrmMeetingAdrSet set = new dfrmMeetingAdrSet();
                if (set.ShowDialog() == DialogResult.OK)
                {
                    this.loadData();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDeleteMeetingAdr_Click(object sender, EventArgs e)
        {
            try
            {
                if (XMessageBox.Show(this.btnDeleteMeetingAdr.Text + ":" + ((DataRowView) this.lstMeetingAdr.SelectedItems[0]).Row[0].ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingAdr  WHERE t_d_MeetingAdr.f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView) this.lstMeetingAdr.SelectedItems[0]).Row[0]));
                    this.loadData();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnSelectReader_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lstMeetingAdr.SelectedItems.Count > 0)
                {
                    dfrmMeetingAdrSet set = new dfrmMeetingAdrSet();
                    set.curMeetingAdr = ((DataRowView) this.lstMeetingAdr.SelectedItems[0]).Row[0].ToString();
                    if (set.ShowDialog() == DialogResult.OK)
                    {
                        this.loadData();
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmMeetingAdr_Load(object sender, EventArgs e)
        {
            this.loadData();
            bool bReadOnly = false;
            string funName = "mnuMeeting";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAddMeetingAdr.Visible = false;
                this.btnDeleteMeetingAdr.Visible = false;
                this.btnSelectReader.Visible = false;
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
                    this.ds.Clear();
                    SqlCommand selectCommand = new SqlCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(this.ds, "t_d_MeetingAdr");
                    this.lstMeetingAdr.DisplayMember = "f_MeetingAdr";
                    this.lstMeetingAdr.DataSource = this.ds.Tables["t_d_MeetingAdr"];
                    selectCommand = new SqlCommand("Select t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 1 as f_Selected,t_d_MeetingAdr.f_MeetingAdr from t_b_reader,t_d_MeetingAdr  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID ", connection);
                    new SqlDataAdapter(selectCommand).Fill(this.ds, "t_d_MeetingAdrReader");
                    this.dv = new DataView(this.ds.Tables["t_d_MeetingAdrReader"]);
                    this.dv.RowFilter = "1<0";
                    this.dv.Sort = "f_ReaderID ASC ";
                    if (this.lstMeetingAdr.SelectedItems.Count > 0)
                    {
                        this.dv.RowFilter = " f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView) this.lstMeetingAdr.SelectedItems[0]).Row[0]);
                        this.btnSelectReader.Enabled = true;
                        this.btnDeleteMeetingAdr.Enabled = true;
                    }
                    DataTable table = this.ds.Tables["t_d_MeetingAdrReader"];
                    for (int i = 0; i < this.dgvSelected.Columns.Count; i++)
                    {
                        this.dgvSelected.Columns[i].DataPropertyName = table.Columns[i].ColumnName;
                    }
                    this.dgvSelected.AutoGenerateColumns = false;
                    this.dgvSelected.DataSource = this.dv;
                    this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        public void loadData_Acc()
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                this.ds.Clear();
                OleDbCommand selectCommand = new OleDbCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "t_d_MeetingAdr");
                this.lstMeetingAdr.DisplayMember = "f_MeetingAdr";
                this.lstMeetingAdr.DataSource = this.ds.Tables["t_d_MeetingAdr"];
                selectCommand = new OleDbCommand("Select t_b_reader.f_ReaderID, t_b_reader.f_ReaderName, 1 as f_Selected,t_d_MeetingAdr.f_MeetingAdr from t_b_reader,t_d_MeetingAdr  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_MeetingAdr.f_ReaderID ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "t_d_MeetingAdrReader");
                this.dv = new DataView(this.ds.Tables["t_d_MeetingAdrReader"]);
                this.dv.RowFilter = "1<0";
                this.dv.Sort = "f_ReaderID ASC ";
                if (this.lstMeetingAdr.SelectedItems.Count > 0)
                {
                    this.dv.RowFilter = " f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView) this.lstMeetingAdr.SelectedItems[0]).Row[0]);
                    this.btnSelectReader.Enabled = true;
                    this.btnDeleteMeetingAdr.Enabled = true;
                }
                DataTable table = this.ds.Tables["t_d_MeetingAdrReader"];
                for (int i = 0; i < this.dgvSelected.Columns.Count; i++)
                {
                    this.dgvSelected.Columns[i].DataPropertyName = table.Columns[i].ColumnName;
                }
                this.dgvSelected.AutoGenerateColumns = false;
                this.dgvSelected.DataSource = this.dv;
                this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void lstMeetingAdr_DoubleClick(object sender, EventArgs e)
        {
            this.btnSelectReader.PerformClick();
        }

        private void lstMeetingAdr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dv != null)
                {
                    if (this.lstMeetingAdr.SelectedItems.Count > 0)
                    {
                        this.dv.RowFilter = " f_MeetingAdr = " + wgTools.PrepareStr(((DataRowView) this.lstMeetingAdr.SelectedItems[0]).Row[0]);
                        this.btnSelectReader.Enabled = true;
                        this.btnDeleteMeetingAdr.Enabled = true;
                    }
                    else
                    {
                        this.dv.RowFilter = " 1<0 ";
                        this.btnSelectReader.Enabled = false;
                        this.btnDeleteMeetingAdr.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }
    }
}

