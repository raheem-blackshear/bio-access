namespace WG3000_COMM.Reports.Shift
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

    public partial class dfrmShiftEdit : frmBioAccess
    {
        private ArrayList arrConsumerCMIndex = new ArrayList();
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrSelectedShiftID = new ArrayList();
        private ArrayList arrShiftID = new ArrayList();
        private DataSet dsConsumers;
        public int shiftid = -1;
        private DataTable dtOptionalShift;

        public dfrmShiftEdit()
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
            this.shiftid = int.Parse(this.arrShiftID[this.cbof_shift.SelectedIndex].ToString());
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void dfrmShiftEdit_Load(object sender, EventArgs e)
        {
            this.dsConsumers = new DataSet("Users");
            string cmdText = "";
            if (wgAppConfig.IsAccessDB)
            {
                cmdText = (cmdText + " SELECT    IIF( [f_ShiftName] IS NULL , CSTR([f_ShiftID]) ") + "    , CSTR([f_ShiftID]) + '-' + [f_ShiftName] " + "    ) AS f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC  ";
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
            cmdText = (cmdText + " SELECT    CASE WHEN [f_ShiftName] IS NULL THEN CONVERT(nvarchar(50),[f_ShiftID]) ") + "    ELSE CONVERT(nvarchar(50),[f_ShiftID]) + '-' + [f_ShiftName] " + "    END AS f_ShiftFullName, [f_ShiftID] from t_b_ShiftSet order by f_ShiftID ASC  ";
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
            this.arrShiftID.Clear();
            this.cbof_shift.Items.Clear();
            this.arrShiftID.Add("0");
            this.cbof_shift.Items.Add("0*-" + CommonStr.strRest);
            if (this.dtOptionalShift.Rows.Count > 0)
            {
                for (int i = 0; i <= (this.dtOptionalShift.Rows.Count - 1); i++)
                {
                    this.cbof_shift.Items.Add(this.dtOptionalShift.Rows[i][0]);
                    this.arrShiftID.Add(this.dtOptionalShift.Rows[i][1]);
                }
            }
            if (this.cbof_shift.Items.Count > 0)
            {
                this.cbof_shift.SelectedIndex = 0;
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

