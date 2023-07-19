namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmControllerInterLock : frmBioAccess
    {
        private dfrmFind dfrmFind1 = new dfrmFind();

        public dfrmControllerInterLock()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.btnOK.Enabled = false;
            wgAppConfig.setSystemParamValue(0x3d, this.chkActiveInterlockShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOK_Click_Acc(sender, e);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("", connection))
                    {
                        connection.Open();
                        for (int i = 0; i <= (this.dataGridView1.Rows.Count - 1); i++)
                        {
                            int num2 = 0;
                            for (int j = 2; j < 6; j++)
                            {
                                if (this.dataGridView1.Rows[i].Cells[j].Value.ToString() == "1")
                                {
                                    switch (j)
                                    {
                                        case 2:
                                            num2 = 1;
                                            break;

                                        case 3:
                                            num2 += 2;
                                            break;

                                        case 4:
                                            num2 = 4;
                                            break;

                                        case 5:
                                            num2 = 8;
                                            break;
                                    }
                                }
                            }
                            string str = " UPDATE t_b_Controller SET ";
                            str = (str + " f_InterLock = " + num2.ToString()) + " WHERE f_ControllerNO = " + this.dataGridView1.Rows[i].Cells[0].Value.ToString();
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                base.Close();
            }
        }

        private void btnOK_Click_Acc(object sender, EventArgs e)
        {
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("", connection))
                {
                    connection.Open();
                    for (int i = 0; i <= (this.dataGridView1.Rows.Count - 1); i++)
                    {
                        int num2 = 0;
                        for (int j = 2; j < 6; j++)
                        {
                            if (this.dataGridView1.Rows[i].Cells[j].Value.ToString() == "1")
                            {
                                switch (j)
                                {
                                    case 2:
                                        num2 = 1;
                                        break;

                                    case 3:
                                        num2 += 2;
                                        break;

                                    case 4:
                                        num2 = 4;
                                        break;

                                    case 5:
                                        num2 = 8;
                                        break;
                                }
                            }
                        }
                        string str = " UPDATE t_b_Controller SET ";
                        str = (str + " f_InterLock = " + num2.ToString()) + " WHERE f_ControllerNO = " + this.dataGridView1.Rows[i].Cells[0].Value.ToString();
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
            }
            base.Close();
        }

        private void chkActiveInterlockShare_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkActiveInterlockShare.Checked)
            {
                for (int i = 0; i < this.dataGridView1.RowCount; i++)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[i];
                    if (wgMjController.GetControllerType(int.Parse(row.Cells[1].Value.ToString())) == 2)
                    {
                        row.Cells[3].ReadOnly = true;
                        row.Cells[4].ReadOnly = true;
                        row.Cells[5].ReadOnly = true;
                        row.Cells[3].Style.BackColor = SystemPens.InactiveBorder.Color;
                        row.Cells[4].Style.BackColor = SystemPens.InactiveBorder.Color;
                        row.Cells[5].Style.BackColor = SystemPens.InactiveBorder.Color;
                    }
                    else
                    {
                        row.Cells[2].Value = 0;
                        row.Cells[3].Value = 0;
                        row.Cells[4].Value = 0;
                        row.Cells[2].ReadOnly = true;
                        row.Cells[3].ReadOnly = true;
                        row.Cells[4].ReadOnly = true;
                        row.Cells[2].Style.BackColor = SystemPens.InactiveBorder.Color;
                        row.Cells[3].Style.BackColor = SystemPens.InactiveBorder.Color;
                        row.Cells[4].Style.BackColor = SystemPens.InactiveBorder.Color;
                    }
                }
            }
            if (this.chkActiveInterlockShare.Checked)
            {
                this.chkGrouped.Enabled = true;
            }
            else
            {
                this.chkGrouped.Enabled = false;
                this.chkGrouped.Checked = false;
            }
        }

        private void chkGrouped_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dfrmControllerInterLock_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmControllerInterLock_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 0x51)) && e.Shift)
                {
                    if (!this.chkGrouped.Visible)
                    {
                        if (this.chkActiveInterlockShare.Visible)
                        {
                            this.chkGrouped.Visible = true;
                            this.dataGridView1.Location = new Point(8, 0x48);
                            this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, (this.dataGridView1.Size.Height + 40) - this.dataGridView1.Location.Y);
                        }
                        else
                        {
                            this.chkActiveInterlockShare.Visible = true;
                            this.dataGridView1.Location = new Point(8, 40);
                            this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, (this.dataGridView1.Size.Height + 8) - this.dataGridView1.Location.Y);
                        }
                    }
                    this.chkActiveInterlockShare_CheckedChanged(null, null);
                }
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                    }
                    this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmControllerInterLock_Load(object sender, EventArgs e)
        {
            string str;
            this.chkActiveInterlockShare.Checked = wgAppConfig.getParamValBoolByNO(0x3d);
            this.chkActiveInterlockShare.Visible = this.chkActiveInterlockShare.Checked;
            if (this.chkActiveInterlockShare.Visible)
            {
                this.dataGridView1.Location = new Point(8, 40);
                if (wgAppConfig.getSystemParamByNO(0x3d) == "2")
                {
                    this.chkGrouped.Checked = true;
                    this.chkGrouped.Visible = true;
                    this.dataGridView1.Location = new Point(8, 0x48);
                }
                this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, (this.dataGridView1.Size.Height + 8) - this.dataGridView1.Location.Y);
            }
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT ";
                str = (((((str + " f_ControllerNO ") + ", f_ControllerSN " + ",  IIF ( [f_Interlock]=1 OR [f_Interlock]=3 , 1 , 0) AS f_InterLock12  ") + ",  IIF ( [f_Interlock]=2 OR [f_Interlock]=3 , 1 , 0) AS f_InterLock34  " + ", IIF ( [f_Interlock]=4 , 1 , 0) AS f_InterLock123 ") + ", IIF ( [f_Interlock]=8 , 1 , 0) AS f_InterLock1234 " + ", f_DoorNames ") + ", t_b_Controller.f_ZoneID " + " from t_b_Controller  ") + " WHERE f_ControllerSN > 199999999 " + " ORDER BY f_ControllerNO ";
            }
            else
            {
                str = " SELECT ";
                str = (((((str + " f_ControllerNO ") + ", f_ControllerSN " + ",  CASE WHEN [f_Interlock]=1 OR [f_Interlock]=3  THEN 1 ELSE 0 END AS f_InterLock12  ") + ",  CASE WHEN [f_Interlock]=2 OR [f_Interlock]=3  THEN 1 ELSE 0 END AS f_InterLock34  " + ", CASE WHEN [f_Interlock]=4 THEN 1 ELSE 0 END AS f_InterLock123 ") + ", CASE WHEN [f_Interlock]=8 THEN 1 ELSE 0 END AS f_InterLock1234 " + ", f_DoorNames ") + ", t_b_Controller.f_ZoneID " + " from t_b_Controller  ") + " WHERE f_ControllerSN > 199999999 " + " ORDER BY f_ControllerNO ";
            }
            wgAppConfig.fillDGVData(ref this.dataGridView1, str);
            DataTable dtController = ((DataView) this.dataGridView1.DataSource).Table;
            new icControllerZone().getAllowedControllers(ref dtController);
            for (int i = 0; i < this.dataGridView1.RowCount; i++)
            {
                DataGridViewRow row = this.dataGridView1.Rows[i];
                if (wgMjController.GetControllerType(int.Parse(row.Cells[1].Value.ToString())) == 2)
                {
                    row.Cells[3].ReadOnly = true;
                    row.Cells[4].ReadOnly = true;
                    row.Cells[5].ReadOnly = true;
                    row.Cells[3].Style.BackColor = SystemPens.InactiveBorder.Color;
                    row.Cells[4].Style.BackColor = SystemPens.InactiveBorder.Color;
                    row.Cells[5].Style.BackColor = SystemPens.InactiveBorder.Color;
                }
            }
            this.chkActiveInterlockShare_CheckedChanged(null, null);
            this.loadOperatorPrivilege();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.dfrmFind1 != null))
            {
                this.dfrmFind1.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuInterLock";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnOK.Visible = false;
                this.dataGridView1.ReadOnly = true;
            }
        }
    }
}

