namespace WG3000_COMM.Basic
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

    public partial class dfrmOperatorZonesConfiguration : frmBioAccess
    {
        private SqlDataAdapter daOptionalGroup;
        private SqlDataAdapter daSelectedGroup;
        private DataSet ds;
        private DataTable dtOptionalGroups;
        private DataTable dtSelectedGroups;
        public int operatorId;

        public dfrmOperatorZonesConfiguration()
        {
            this.InitializeComponent();
            this.button2.DialogResult = DialogResult.Cancel;
        }

        private void _bindGroup()
        {
            try
            {
                this.lstOptionalGroups.DisplayMember = "f_ZoneName";
                this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
                this.lstSelectedGroups.DisplayMember = "f_ZoneName";
                this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void _dataTableLoad()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this._dataTableLoad_Acc();
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("Select * from t_b_Controller_Zone where f_ZoneID IN (SELECT f_ZoneID FROM t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", connection))
                    {
                        using (SqlCommand command2 = new SqlCommand("Select * from t_b_Controller_Zone where f_ZoneID NOT IN (SELECT f_ZoneID FROM  t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", connection))
                        {
                            this.ds = new DataSet("Users-Doors");
                            this.daSelectedGroup = new SqlDataAdapter(command);
                            this.daOptionalGroup = new SqlDataAdapter(command2);
                            try
                            {
                                this.ds.Clear();
                                this.daOptionalGroup.Fill(this.ds, "OptionalGroups");
                                this.daSelectedGroup.Fill(this.ds, "SelectedGroups");
                                this.dtOptionalGroups = new DataTable();
                                this.dtOptionalGroups = this.ds.Tables["OptionalGroups"].Copy();
                                this.dtSelectedGroups = new DataTable();
                                this.dtSelectedGroups = this.ds.Tables["SelectedGroups"].Copy();
                                this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
                                this.lstOptionalGroups.DisplayMember = "f_ZoneName";
                                this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
                                this.lstSelectedGroups.DisplayMember = "f_ZoneName";
                                this.dtSelectedGroups.AcceptChanges();
                                this.dtOptionalGroups.AcceptChanges();
                            }
                            catch (Exception exception)
                            {
                                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                            }
                        }
                    }
                }
            }
        }

        private void _dataTableLoad_Acc()
        {
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("Select * from t_b_Controller_Zone where f_ZoneID IN (SELECT f_ZoneID FROM t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", connection))
                {
                    using (OleDbCommand command2 = new OleDbCommand("Select * from t_b_Controller_Zone where f_ZoneID NOT IN (SELECT f_ZoneID FROM  t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId.ToString() + " ) ", connection))
                    {
                        this.ds = new DataSet("Users-Doors");
                        OleDbDataAdapter adapter2 = new OleDbDataAdapter(command);
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command2);
                        try
                        {
                            this.ds.Clear();
                            adapter.Fill(this.ds, "OptionalGroups");
                            adapter2.Fill(this.ds, "SelectedGroups");
                            this.dtOptionalGroups = new DataTable();
                            this.dtOptionalGroups = this.ds.Tables["OptionalGroups"].Copy();
                            this.dtSelectedGroups = new DataTable();
                            this.dtSelectedGroups = this.ds.Tables["SelectedGroups"].Copy();
                            this.lstOptionalGroups.DataSource = this.dtOptionalGroups;
                            this.lstOptionalGroups.DisplayMember = "f_ZoneName";
                            this.lstSelectedGroups.DataSource = this.dtSelectedGroups;
                            this.lstSelectedGroups.DisplayMember = "f_ZoneName";
                            this.dtSelectedGroups.AcceptChanges();
                            this.dtOptionalGroups.AcceptChanges();
                        }
                        catch (Exception exception)
                        {
                            wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                        }
                    }
                }
            }
        }

        private void _unbindGroup()
        {
            try
            {
                this.lstOptionalGroups.DataSource = null;
                this.lstOptionalGroups.DisplayMember = null;
                this.lstSelectedGroups.DataSource = null;
                this.lstSelectedGroups.DisplayMember = null;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnAddAllGroups_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor current = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                this._unbindGroup();
                DataTable dtOptionalGroups = this.dtOptionalGroups;
                DataTable dtSelectedGroups = this.dtSelectedGroups;
                for (int i = 0; i <= (dtOptionalGroups.Rows.Count - 1); i++)
                {
                    dtSelectedGroups.ImportRow(dtOptionalGroups.Rows[i]);
                }
                dtOptionalGroups.Clear();
                dtSelectedGroups.AcceptChanges();
                dtOptionalGroups.AcceptChanges();
                this.lstSelectedGroups.Refresh();
                this.lstOptionalGroups.Refresh();
                this._bindGroup();
                Cursor.Current = current;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnAddOneGroup_Click(object sender, EventArgs e)
        {
            lst_UpdateOne(this.dtOptionalGroups, this.dtSelectedGroups, this.lstOptionalGroups, this.lstSelectedGroups);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDeleteAllGroups_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor current = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                this._unbindGroup();
                DataTable dtSelectedGroups = this.dtSelectedGroups;
                DataTable dtOptionalGroups = this.dtOptionalGroups;
                for (int i = 0; i <= (dtSelectedGroups.Rows.Count - 1); i++)
                {
                    dtOptionalGroups.ImportRow(dtSelectedGroups.Rows[i]);
                }
                dtSelectedGroups.Clear();
                dtSelectedGroups.AcceptChanges();
                dtOptionalGroups.AcceptChanges();
                this.lstSelectedGroups.Refresh();
                this.lstOptionalGroups.Refresh();
                this._bindGroup();
                Cursor.Current = current;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnDeleteOneGroup_Click(object sender, EventArgs e)
        {
            lst_UpdateOne(this.dtSelectedGroups, this.dtOptionalGroups, this.lstSelectedGroups, this.lstOptionalGroups);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Cursor current = Cursor.Current;
                DataTable dtSelectedGroups = this.dtSelectedGroups;
                wgAppConfig.runUpdateSql(" DELETE FROM t_b_Controller_Zone4Operator Where f_OperatorId = " + this.operatorId);
                if (dtSelectedGroups.Rows.Count > 0)
                {
                    for (int i = 0; i <= (dtSelectedGroups.Rows.Count - 1); i++)
                    {
                        wgAppConfig.runUpdateSql((((" INSERT INTO t_b_Controller_Zone4Operator" + " (f_ZoneID, f_OperatorID) ") + " Values(" + dtSelectedGroups.Rows[i]["f_ZoneID"]) + " ," + this.operatorId) + " )");
                    }
                }
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.Cursor = Cursors.Default;
        }

        private void btnZoneManage_Click(object sender, EventArgs e)
        {
            using (frmZones zones = new frmZones())
            {
                zones.ShowDialog(this);
            }
            this._dataTableLoad();
        }

        private void dfrmSwitchGroupsConfiguration_Load(object sender, EventArgs e)
        {
            this._dataTableLoad();
            this.btnZoneManage.Visible = false;
            bool bReadOnly = false;
            string funName = "btnZoneManage";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && !bReadOnly)
            {
                this.btnZoneManage.Visible = true;
            }
        }

        public static void lst_UpdateOne(DataTable dtSource, DataTable dtDestine, ListBox lstSrc, ListBox lstDest)
        {
            try
            {
                object dataSource = lstDest.DataSource;
                string displayMember = lstDest.DisplayMember;
                lstDest.DisplayMember = null;
                lstDest.DataSource = null;
                Cursor current = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    if (lstSrc.SelectedIndices.Count > 0)
                    {
                        int num2;
                        DataTable table = dtDestine.Copy();
                        table.Rows.Clear();
                        int num3 = lstSrc.SelectedIndices.Count - 1;
                        int[] numArray = new int[num3 + 1];
                        for (num2 = 0; num2 <= num3; num2++)
                        {
                            numArray[num2] = lstSrc.SelectedIndices[num3 - num2];
                        }
                        for (num2 = 0; num2 <= num3; num2++)
                        {
                            int num = numArray[num2];
                            if (num >= 0)
                            {
                                DataRow row = dtSource.Rows[num];
                                table.ImportRow(row);
                                dtSource.Rows.Remove(row);
                                dtSource.AcceptChanges();
                            }
                        }
                        table.AcceptChanges();
                        for (num2 = 0; num2 <= num3; num2++)
                        {
                            dtDestine.ImportRow(table.Rows[num3 - num2]);
                        }
                        dtSource.AcceptChanges();
                        dtDestine.AcceptChanges();
                        lstSrc.Refresh();
                        lstDest.Refresh();
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                lstDest.DisplayMember = displayMember;
                lstDest.DataSource = dataSource;
                Cursor.Current = current;
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[] { EventLogEntryType.Error });
            }
        }
    }
}

