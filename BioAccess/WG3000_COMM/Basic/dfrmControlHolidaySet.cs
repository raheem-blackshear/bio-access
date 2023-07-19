namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmControlHolidaySet : frmBioAccess
    {
        private DataTable dt;
        private DataView dvHolidays;
        private DataView dvNeedWork;

        public dfrmControlHolidaySet()
        {
            this.InitializeComponent();
        }

        private void _dataTableLoad()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this._dataTableLoad_Acc();
            }
            else
            {
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    string cmdText = "";
                    cmdText = "SELECT f_Id, f_BeginYMDHMS, f_EndYMDHMS, f_Notes,f_forcework From t_b_ControlHolidays ";
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            this.dt = new DataTable();
                            adapter.Fill(this.dt);
                            this.dvHolidays = new DataView(this.dt);
                            this.dvHolidays.RowFilter = " f_forcework <> 1";
                            this.dgvMain.AutoGenerateColumns = false;
                            this.dgvMain.DataSource = this.dvHolidays;
                            this.dvHolidays.Sort = "f_BeginYMDHMS ASC";
                            for (int i = 0; i < this.dvHolidays.Table.Columns.Count; i++)
                            {
                                this.dgvMain.Columns[i].DataPropertyName = this.dvHolidays.Table.Columns[i].ColumnName;
                                this.dgvMain.Columns[i].Name = this.dvHolidays.Table.Columns[i].ColumnName;
                                if (this.dgvMain.ColumnCount == (i + 1))
                                {
                                    break;
                                }
                            }
                            this.dvNeedWork = new DataView(this.dt);
                            this.dvNeedWork.RowFilter = " f_forcework = 1";
                            this.dgvNeedWork.AutoGenerateColumns = false;
                            this.dgvNeedWork.DataSource = this.dvNeedWork;
                            this.dvNeedWork.Sort = "f_BeginYMDHMS ASC";
                            for (int j = 0; j < this.dvNeedWork.Table.Columns.Count; j++)
                            {
                                this.dgvNeedWork.Columns[j].DataPropertyName = this.dvNeedWork.Table.Columns[j].ColumnName;
                                this.dgvNeedWork.Columns[j].Name = this.dvNeedWork.Table.Columns[j].ColumnName;
                                if (this.dgvNeedWork.ColumnCount == (j + 1))
                                {
                                    goto Label_023C;
                                }
                            }
                        }
                    }
                Label_023C:
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                    wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                    wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                    wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                    this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                    this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                    this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                    this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }

        private void _dataTableLoad_Acc()
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                string cmdText = "";
                cmdText = "SELECT f_Id, f_BeginYMDHMS, f_EndYMDHMS, f_Notes,f_forcework From t_b_ControlHolidays ";
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        this.dt = new DataTable();
                        adapter.Fill(this.dt);
                        this.dvHolidays = new DataView(this.dt);
                        this.dvHolidays.RowFilter = " f_forcework <> 1";
                        this.dgvMain.AutoGenerateColumns = false;
                        this.dgvMain.DataSource = this.dvHolidays;
                        this.dvHolidays.Sort = "f_BeginYMDHMS ASC";
                        for (int i = 0; i < this.dvHolidays.Table.Columns.Count; i++)
                        {
                            this.dgvMain.Columns[i].DataPropertyName = this.dvHolidays.Table.Columns[i].ColumnName;
                            this.dgvMain.Columns[i].Name = this.dvHolidays.Table.Columns[i].ColumnName;
                            if (this.dgvMain.ColumnCount == (i + 1))
                            {
                                break;
                            }
                        }
                        this.dvNeedWork = new DataView(this.dt);
                        this.dvNeedWork.RowFilter = " f_forcework = 1";
                        this.dgvNeedWork.AutoGenerateColumns = false;
                        this.dgvNeedWork.DataSource = this.dvNeedWork;
                        this.dvNeedWork.Sort = "f_BeginYMDHMS ASC";
                        for (int j = 0; j < this.dvNeedWork.Table.Columns.Count; j++)
                        {
                            this.dgvNeedWork.Columns[j].DataPropertyName = this.dvNeedWork.Table.Columns[j].ColumnName;
                            this.dgvNeedWork.Columns[j].Name = this.dvNeedWork.Table.Columns[j].ColumnName;
                            if (this.dgvNeedWork.ColumnCount == (j + 1))
                            {
                                goto Label_022E;
                            }
                        }
                    }
                }
            Label_022E:
                wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                wgAppConfig.setDisplayFormatDate(this.dgvMain, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_BeginYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                wgAppConfig.setDisplayFormatDate(this.dgvNeedWork, "f_EndYMDHMS", wgTools.DisplayFormat_DateYMDHMSWeek);
                this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvMain.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvNeedWork.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                connection.Dispose();
            }
        }

        private void btnAddHoliday_Click(object sender, EventArgs e)
        {
            using (dfrmControlHolidayAdd add = new dfrmControlHolidayAdd())
            {
                add.ShowDialog(this);
            }
            this._dataTableLoad();
        }

        private void btnAddNeedWorkDay_Click(object sender, EventArgs e)
        {
            using (dfrmControlHolidayAdd add = new dfrmControlHolidayAdd())
            {
                add.Text = this.groupBox2.Text;
                add.bHoliday = false;
                add.ShowDialog(this);
            }
            this._dataTableLoad();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnDelHoliday_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.Rows.Count > 0)
            {
                int index = this.dgvMain.SelectedRows[0].Index;
                wgAppConfig.runUpdateSql(" DELETE FROM t_b_ControlHolidays WHERE [f_Id]= " + this.dgvMain.SelectedRows[0].Cells["f_Id"].Value.ToString());
                this._dataTableLoad();
            }
        }

        private void btnDelNeedWorkDay_Click(object sender, EventArgs e)
        {
            if (this.dgvNeedWork.Rows.Count > 0)
            {
                int index = this.dgvNeedWork.SelectedRows[0].Index;
                wgAppConfig.runUpdateSql(" DELETE FROM t_b_ControlHolidays WHERE [f_Id]= " + this.dgvNeedWork.SelectedRows[0].Cells["f_Id"].Value.ToString());
                this._dataTableLoad();
            }
        }

        private void dfrmHolidaySet_Load(object sender, EventArgs e)
        {
            this._dataTableLoad();
            bool bReadOnly = false;
            string funName = "mnuHolidaySet";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAddHoliday.Visible = false;
                this.btnDelHoliday.Visible = false;
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
    }
}

