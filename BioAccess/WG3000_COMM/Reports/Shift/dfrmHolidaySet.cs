namespace WG3000_COMM.Reports.Shift
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
    using WG3000_COMM.ResStrings;

    public partial class dfrmHolidaySet : frmBioAccess
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataAdapter da;
        private DataSet ds;
        private DataTable dt;

        public dfrmHolidaySet()
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
                this.con = new SqlConnection(wgAppConfig.dbConString);
                this.ds = new DataSet("Holiday");
                try
                {
                    this.ds.Clear();
                    string str2 = "";
                    str2 = " SELECT  t_a_Holiday.f_Name,";
                    str2 = ((str2 + " [f_Value] AS f_from, " + " [f_Value1] AS f_from1, ") + " [f_Value2] AS f_to, " + " [f_Value3] AS f_to1, ") + " t_a_Holiday.f_Note, t_a_Holiday.f_No,t_a_Holiday.f_Value,f_EName,f_Value1,f_Value3" + " FROM t_a_Holiday";
                    string cmdText = str2 + " WHERE [f_Type]='1'";
                    this.cmd = new SqlCommand(cmdText, this.con);
                    this.da = new SqlDataAdapter(this.cmd);
                    this.da.Fill(this.ds, "Holiday1");
                    this.dt = this.ds.Tables["Holiday1"];
                    using (comShift shift = new comShift())
                    {
                        shift.localizedHoliday(this.ds.Tables["Holiday1"]);
                    }
                    cmdText = str2 + " WHERE [f_Type]='2' ORDER BY [f_from] ASC ";
                    this.cmd = new SqlCommand(cmdText, this.con);
                    this.da = new SqlDataAdapter(this.cmd);
                    this.da.Fill(this.ds, "Holiday2");
                    using (comShift shift2 = new comShift())
                    {
                        shift2.localizedHoliday(this.ds.Tables["Holiday2"]);
                    }
                    cmdText = str2 + " WHERE [f_Type]='3' ORDER BY [f_from] ASC ";
                    this.cmd = new SqlCommand(cmdText, this.con);
                    this.da = new SqlDataAdapter(this.cmd);
                    this.da.Fill(this.ds, "NeedWork");
                    using (comShift shift3 = new comShift())
                    {
                        shift3.localizedHoliday(this.ds.Tables["NeedWork"]);
                    }
                    this.dgvMain.AutoGenerateColumns = false;
                    this.dgvMain2.AutoGenerateColumns = false;
                    this.dgvMain.DataSource = this.ds.Tables["Holiday2"];
                    this.dgvMain2.DataSource = this.ds.Tables["NeedWork"];
                    int num2 = 0;
                    while (num2 < this.dgvMain.Columns.Count)
                    {
                        this.dgvMain.Columns[num2].DataPropertyName = this.dt.Columns[num2].ColumnName;
                        this.dgvMain2.Columns[num2].DataPropertyName = this.dt.Columns[num2].ColumnName;
                        this.dgvMain.Columns[num2].Name = this.dt.Columns[num2].ColumnName;
                        this.dgvMain2.Columns[num2].Name = this.dt.Columns[num2].ColumnName + "_NeedWork";
                        num2++;
                    }
                    num2 = 0;
                    while (num2 < this.dgvMain.Rows.Count)
                    {
                        this.dgvMain.Rows[num2].Cells["f_from"].Value = DateTime.Parse(this.dgvMain.Rows[num2].Cells["f_from"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                        this.dgvMain.Rows[num2].Cells["f_to"].Value = DateTime.Parse(this.dgvMain.Rows[num2].Cells["f_to"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                        num2++;
                    }
                    for (num2 = 0; num2 < this.dgvMain2.Rows.Count; num2++)
                    {
                        this.dgvMain2.Rows[num2].Cells["f_from_NeedWork"].Value = DateTime.Parse(this.dgvMain2.Rows[num2].Cells["f_from_NeedWork"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                        this.dgvMain2.Rows[num2].Cells["f_to_NeedWork"].Value = DateTime.Parse(this.dgvMain2.Rows[num2].Cells["f_to_NeedWork"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                    }
                    for (int i = 0; i <= (this.dt.Rows.Count - 1); i++)
                    {
                        DataRow row = this.dt.Rows[i];
                        if (Convert.ToInt32(row["f_NO"]) == 1)
                        {
                            if (Convert.ToString(row["f_Value"]) == "0")
                            {
                                this.optSatWork0.Checked = true;
                            }
                            else if (Convert.ToString(row["f_Value"]) == "1")
                            {
                                this.optSatWork1.Checked = true;
                            }
                            else if (Convert.ToString(row["f_Value"]) == "3")
                            {
                                this.optSatWork2.Checked = true;
                            }
                            else
                            {
                                this.optSatWork0.Checked = true;
                            }
                        }
                        else if (Convert.ToInt32(row["f_NO"]) == 2)
                        {
                            if (Convert.ToString(row["f_Value"]) == "0")
                            {
                                this.optSunWork0.Checked = true;
                            }
                            else if (Convert.ToString(row["f_Value"]) == "1")
                            {
                                this.optSunWork1.Checked = true;
                            }
                            else if (Convert.ToString(row["f_Value"]) == "3")
                            {
                                this.optSunWork2.Checked = true;
                            }
                            else
                            {
                                this.optSunWork0.Checked = true;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        private void _dataTableLoad_Acc()
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            this.ds = new DataSet("Holiday");
            try
            {
                int num2;
                this.ds.Clear();
                string str2 = "";
                str2 = " SELECT  t_a_Holiday.f_Name,";
                str2 = ((str2 + " [f_Value] AS f_from, " + " [f_Value1] AS f_from1, ") + " [f_Value2] AS f_to, " + " [f_Value3] AS f_to1, ") + " t_a_Holiday.f_Note, t_a_Holiday.f_No,t_a_Holiday.f_Value,f_EName,f_Value1,f_Value3" + " FROM t_a_Holiday";
                OleDbCommand selectCommand = new OleDbCommand(str2 + " WHERE [f_Type]='1'", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "Holiday1");
                this.dt = this.ds.Tables["Holiday1"];
                using (comShift_Acc acc = new comShift_Acc())
                {
                    acc.localizedHoliday(this.ds.Tables["Holiday1"]);
                }
                selectCommand = new OleDbCommand(str2 + " WHERE [f_Type]='2' ORDER BY [f_Value] ASC ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "Holiday2");
                using (comShift_Acc acc2 = new comShift_Acc())
                {
                    acc2.localizedHoliday(this.ds.Tables["Holiday2"]);
                }
                selectCommand = new OleDbCommand(str2 + " WHERE [f_Type]='3' ORDER BY [f_Value] ASC ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "NeedWork");
                using (comShift_Acc acc3 = new comShift_Acc())
                {
                    acc3.localizedHoliday(this.ds.Tables["NeedWork"]);
                }
                this.dgvMain.AutoGenerateColumns = false;
                this.dgvMain2.AutoGenerateColumns = false;
                this.dgvMain.DataSource = this.ds.Tables["Holiday2"];
                this.dgvMain2.DataSource = this.ds.Tables["NeedWork"];
                for (num2 = 0; num2 < this.dgvMain.Columns.Count; num2++)
                {
                    this.dgvMain.Columns[num2].DataPropertyName = this.dt.Columns[num2].ColumnName;
                    this.dgvMain2.Columns[num2].DataPropertyName = this.dt.Columns[num2].ColumnName;
                    this.dgvMain.Columns[num2].Name = this.dt.Columns[num2].ColumnName;
                    this.dgvMain2.Columns[num2].Name = this.dt.Columns[num2].ColumnName + "_NeedWork";
                }
                for (num2 = 0; num2 < this.dgvMain.Rows.Count; num2++)
                {
                    this.dgvMain.Rows[num2].Cells["f_from"].Value = DateTime.Parse(this.dgvMain.Rows[num2].Cells["f_from"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                    this.dgvMain.Rows[num2].Cells["f_to"].Value = DateTime.Parse(this.dgvMain.Rows[num2].Cells["f_to"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                }
                for (num2 = 0; num2 < this.dgvMain2.Rows.Count; num2++)
                {
                    this.dgvMain2.Rows[num2].Cells["f_from_NeedWork"].Value = DateTime.Parse(this.dgvMain2.Rows[num2].Cells["f_from_NeedWork"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                    this.dgvMain2.Rows[num2].Cells["f_to_NeedWork"].Value = DateTime.Parse(this.dgvMain2.Rows[num2].Cells["f_to_NeedWork"].Value.ToString()).ToString(wgTools.DisplayFormat_DateYMDWeek);
                }
                for (int i = 0; i <= (this.dt.Rows.Count - 1); i++)
                {
                    DataRow row = this.dt.Rows[i];
                    if (Convert.ToInt32(row["f_NO"]) == 1)
                    {
                        if (Convert.ToString(row["f_Value"]) == "0")
                        {
                            this.optSatWork0.Checked = true;
                        }
                        else if (Convert.ToString(row["f_Value"]) == "1")
                        {
                            this.optSatWork1.Checked = true;
                        }
                        else if (Convert.ToString(row["f_Value"]) == "3")
                        {
                            this.optSatWork2.Checked = true;
                        }
                        else
                        {
                            this.optSatWork0.Checked = true;
                        }
                    }
                    else if (Convert.ToInt32(row["f_NO"]) == 2)
                    {
                        if (Convert.ToString(row["f_Value"]) == "0")
                        {
                            this.optSunWork0.Checked = true;
                        }
                        else if (Convert.ToString(row["f_Value"]) == "1")
                        {
                            this.optSunWork1.Checked = true;
                        }
                        else if (Convert.ToString(row["f_Value"]) == "3")
                        {
                            this.optSunWork2.Checked = true;
                        }
                        else
                        {
                            this.optSunWork0.Checked = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnAddHoliday_Click(object sender, EventArgs e)
        {
            using (dfrmHolidayAdd add = new dfrmHolidayAdd())
            {
                add.ShowDialog(this);
            }
            this._dataTableLoad();
        }

        private void btnAddNeedWork_Click(object sender, EventArgs e)
        {
            using (dfrmHolidayAdd add = new dfrmHolidayAdd())
            {
                add.holidayType = "3";
                add.Text = CommonStr.strNeedToWork;
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
                wgAppConfig.runUpdateSql(" DELETE FROM t_a_Holiday WHERE [f_NO]= " + (this.dgvMain.DataSource as DataTable).Rows[index]["f_NO"].ToString());
                this._dataTableLoad();
            }
        }

        private void btnDelNeedWork_Click(object sender, EventArgs e)
        {
            if (this.dgvMain2.Rows.Count > 0)
            {
                int index = this.dgvMain2.SelectedRows[0].Index;
                wgAppConfig.runUpdateSql(" DELETE FROM t_a_Holiday WHERE [f_NO]= " + (this.dgvMain2.DataSource as DataTable).Rows[index]["f_NO"].ToString());
                this._dataTableLoad();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string str = " UPDATE t_a_Holiday ";
            str = str + " SET [f_Value]=";
            if (this.optSatWork0.Checked)
            {
                str = str + wgTools.PrepareStr(0);
            }
            else if (this.optSatWork1.Checked)
            {
                str = str + wgTools.PrepareStr(1);
            }
            else if (this.optSatWork2.Checked)
            {
                str = str + wgTools.PrepareStr(3);
            }
            else
            {
                str = str + wgTools.PrepareStr(0);
            }
            wgAppConfig.runUpdateSql(str + " WHERE [f_NO]=1");
            str = " UPDATE t_a_Holiday ";
            str = str + " SET [f_Value]=";
            if (this.optSunWork0.Checked)
            {
                str = str + wgTools.PrepareStr(0);
            }
            else if (this.optSunWork1.Checked)
            {
                str = str + wgTools.PrepareStr(1);
            }
            else if (this.optSunWork2.Checked)
            {
                str = str + wgTools.PrepareStr(3);
            }
            else
            {
                str = str + wgTools.PrepareStr(0);
            }
            wgAppConfig.runUpdateSql(str + " WHERE [f_NO]=2");
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void dfrmHolidaySet_Load(object sender, EventArgs e)
        {
            this._dataTableLoad();
            bool bReadOnly = false;
            string funName = "mnuHolidaySet";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAddHoliday.Visible = false;
                this.btnAddNeedWork.Visible = false;
                this.btnDelHoliday.Visible = false;
                this.btnDelNeedWork.Visible = false;
                this.btnOK.Visible = false;
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

