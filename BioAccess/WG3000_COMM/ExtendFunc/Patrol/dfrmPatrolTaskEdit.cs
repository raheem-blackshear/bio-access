namespace WG3000_COMM.ExtendFunc.Patrol
{
    using System;
    using System.Collections;
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

    public partial class dfrmPatrolTaskEdit : frmBioAccess
    {
        private ArrayList arrConsumerCMIndex = new ArrayList();
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrRouteID = new ArrayList();
        private ArrayList arrSelectedRouteID = new ArrayList();
        private DataSet dsConsumers;
        private DataTable dtOptionalShift;
        public int routeID = -1;

        public dfrmPatrolTaskEdit()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.routeID = int.Parse(this.arrRouteID[this.cbof_route.SelectedIndex].ToString());
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void dfrmShiftEdit_Load(object sender, EventArgs e)
        {
            this.dsConsumers = new DataSet("Users");
            string cmdText = "";
            if (wgAppConfig.IsAccessDB)
            {
                cmdText = (cmdText + " SELECT    IIF( [f_RouteName] IS NULL , CSTR([f_RouteID]) ") + "    , CSTR([f_RouteID]) + '-' + [f_RouteName] " + "    ) AS f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC  ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dsConsumers, "OptionalShift");
                        }
                    }
                    goto Label_0114;
                }
            }
            cmdText = (cmdText + " SELECT    CASE WHEN [f_RouteName] IS NULL THEN CONVERT(nvarchar(50),[f_RouteID]) ") + "    ELSE CONVERT(nvarchar(50),[f_RouteID]) + '-' + [f_RouteName] " + "    END AS f_RouteFullName, [f_RouteID] from t_d_PatrolRouteList order by f_RouteID ASC  ";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dsConsumers, "OptionalShift");
                    }
                }
            }
        Label_0114:
            this.dtOptionalShift = this.dsConsumers.Tables["OptionalShift"];
            this.arrRouteID.Clear();
            this.cbof_route.Items.Clear();
            this.arrRouteID.Add("0");
            this.cbof_route.Items.Add("0*-" + CommonStr.strPatrolEventRest);
            if (this.dtOptionalShift.Rows.Count > 0)
            {
                for (int i = 0; i <= (this.dtOptionalShift.Rows.Count - 1); i++)
                {
                    this.cbof_route.Items.Add(this.dtOptionalShift.Rows[i][0]);
                    this.arrRouteID.Add(this.dtOptionalShift.Rows[i][1]);
                }
            }
            if (this.cbof_route.Items.Count > 0)
            {
                this.cbof_route.SelectedIndex = 0;
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

