namespace WG3000_COMM.Reports.Shift
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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmHolidayType : frmBioAccess
    {
        private ArrayList arrDefaultList = new ArrayList();
        private DataTable dt;

        public dfrmHolidayType()
        {
            this.InitializeComponent();
        }

        private void _loadData()
        {
            string cmdText = "SELECT * FROM t_a_HolidayType ORDER BY f_NO ";
            this.dt = new DataTable("t_a_HolidayType");
            if (wgAppConfig.IsAccessDB)
            {
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
                comShift_Acc.localizedHolidayType(this.dt);
            }
            else
            {
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                        {
                            adapter2.Fill(this.dt);
                        }
                    }
                }
                comShift.localizedHolidayType(this.dt);
            }
            this.arrDefaultList.Clear();
            this.arrDefaultList.Add("出差");
            this.arrDefaultList.Add("Business Trip");
            this.arrDefaultList.Add("病假");
            this.arrDefaultList.Add("Sick Leave");
            this.arrDefaultList.Add("事假");
            this.arrDefaultList.Add("Private Leave");
            this.arrDefaultList.Add(CommonStr.strAbsence);
            this.arrDefaultList.Add(CommonStr.strLateness);
            this.arrDefaultList.Add(CommonStr.strNotReadCard);
            this.arrDefaultList.Add(CommonStr.strLeaveEarly);
            this.arrDefaultList.Add(CommonStr.strRest);
            this.arrDefaultList.Add(CommonStr.strOvertime);
            this.arrDefaultList.Add(CommonStr.strSignIn);
            this.arrDefaultList.Add(CommonStr.strPrivateLeave);
            this.arrDefaultList.Add(CommonStr.strSickLeave);
            this.arrDefaultList.Add(CommonStr.strBusinessTrip);
            this.arrDefaultList.Add(CommonStr.strPatrolEventAbsence);
            this.arrDefaultList.Add(CommonStr.strPatrolEventEarly);
            this.arrDefaultList.Add(CommonStr.strPatrolEventLate);
            this.arrDefaultList.Add(CommonStr.strPatrolEventNormal);
            this.arrDefaultList.Add(CommonStr.strPatrolEventRest);
            this.arrDefaultList.Add(CommonStr.strPatrolEventLate);
            if (this.dt.Rows.Count > 0)
            {
                this.btnDel.Enabled = true;
                this.btnEdit.Enabled = true;
            }
            else
            {
                this.btnDel.Enabled = false;
                this.btnEdit.Enabled = false;
            }
            if (this.dt.Rows.Count >= 0x20)
            {
                this.btnAdd.Enabled = false;
            }
            else
            {
                this.btnAdd.Enabled = true;
            }
            this.lstHolidayType.DataSource = this.dt;
            this.lstHolidayType.DisplayMember = "f_HolidayType";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    string strNewName = name.strNewName;
                    if (this.arrDefaultList.IndexOf(strNewName) >= 0)
                    {
                        XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        using (DataView view = new DataView(this.lstHolidayType.DataSource as DataTable))
                        {
                            view.RowFilter = " f_HolidayType= " + wgTools.PrepareStr(strNewName);
                            if (view.Count > 0)
                            {
                                XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        wgAppConfig.runUpdateSql(" INSERT INTO  t_a_HolidayType (f_HolidayType) VALUES(" + wgTools.PrepareStr(strNewName.ToString()) + ")");
                        this._loadData();
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.arrDefaultList.IndexOf(this.lstHolidayType.Text) >= 0)
            {
                XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string str = string.Format("{0}", this.btnDel.Text);
                if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    wgAppConfig.runUpdateSql(" DELETE FROM t_a_HolidayType " + " WHERE f_HolidayType = " + wgTools.PrepareStr(this.lstHolidayType.Text));
                    this._loadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.arrDefaultList.IndexOf(this.lstHolidayType.Text) >= 0)
            {
                XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                using (dfrmInputNewName name = new dfrmInputNewName())
                {
                    if (name.ShowDialog(this) == DialogResult.OK)
                    {
                        string strNewName = name.strNewName;
                        if (this.lstHolidayType.Text != strNewName)
                        {
                            if (this.arrDefaultList.IndexOf(strNewName) >= 0)
                            {
                                XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                using (DataView view = new DataView(this.lstHolidayType.DataSource as DataTable))
                                {
                                    view.RowFilter = " f_HolidayType= " + wgTools.PrepareStr(strNewName);
                                    if (view.Count > 0)
                                    {
                                        XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }
                                }
                                wgAppConfig.runUpdateSql((" UPDATE t_a_HolidayType SET f_HolidayType = " + wgTools.PrepareStr(strNewName.ToString())) + " WHERE f_HolidayType = " + wgTools.PrepareStr(this.lstHolidayType.Text));
                                this._loadData();
                            }
                        }
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void dfrmHolidayType_Load(object sender, EventArgs e)
        {
            this._loadData();
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

