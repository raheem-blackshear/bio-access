namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmControllerAntiPassback : frmBioAccess
    {
        private bool bLoad = true;
        private dfrmFind dfrmFind1 = new dfrmFind();

        public dfrmControllerAntiPassback()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {
                if (this.dataGridView1.SelectedCells.Count <= 0)
                {
                    return;
                }
                int rowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
            }
            else
            {
                int index = this.dataGridView1.SelectedRows[0].Index;
            }
            int num = 0;
            DataGridView view = this.dataGridView1;
            if (view.Rows.Count > 0)
            {
                num = view.CurrentCell.RowIndex;
            }
            using (dfrmAntiBack back = new dfrmAntiBack())
            {
                back.ControllerSN = view.Rows[num].Cells[1].Value.ToString();
                back.Text = back.Text + "[" + view.Rows[num].Cells[1].Value.ToString() + "]";
                if (back.ShowDialog(this) == DialogResult.OK)
                {
                    int retValue = back.retValue;
                    if (wgAppConfig.runUpdateSql("UPDATE t_b_Controller SET f_AntiBack =" + retValue.ToString() + " Where f_ControllerSN = " + view.Rows[num].Cells[1].Value.ToString()) > 0)
                    {
                        if (retValue == 0)
                        {
                            view.Rows[num].Cells["f_AntiBack"].Value = 0;
                        }
                        else
                        {
                            view.Rows[num].Cells["f_AntiBack"].Value = 1;
                        }
                    }
                }
            }
        }

        private void chkActiveAntibackShare_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.bLoad)
            {
                wgAppConfig.setSystemParamValue(0x3e, this.chkActiveAntibackShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
                if (this.chkActiveAntibackShare.Checked)
                {
                    this.chkGrouped.Enabled = true;
                }
                else
                {
                    this.chkGrouped.Enabled = false;
                    this.chkGrouped.Checked = false;
                }
            }
        }

        private void chkGrouped_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.bLoad)
            {
                wgAppConfig.setSystemParamValue(0x3e, this.chkActiveAntibackShare.Checked ? (this.chkGrouped.Checked ? "2" : "1") : "0");
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void dfrmControllerAntiPassback_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmControllerAntiPassback_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 0x51)) && e.Shift)
                {
                    if (!this.chkGrouped.Visible)
                    {
                        if (this.chkActiveAntibackShare.Visible)
                        {
                            this.chkGrouped.Visible = true;
                            this.dataGridView1.Location = new Point(8, 0x48);
                            this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, (this.dataGridView1.Size.Height + 40) - this.dataGridView1.Location.Y);
                        }
                        else
                        {
                            this.chkActiveAntibackShare.Visible = true;
                            this.dataGridView1.Location = new Point(8, 40);
                            this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, (this.dataGridView1.Size.Height + 8) - this.dataGridView1.Location.Y);
                        }
                    }
                    this.chkActiveAntibackShare_CheckedChanged(null, null);
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

        private void dfrmControllerAntiPassback_Load(object sender, EventArgs e)
        {
            this.chkActiveAntibackShare.Checked = wgAppConfig.getParamValBoolByNO(0x3e);
            this.chkActiveAntibackShare.Visible = this.chkActiveAntibackShare.Checked;
            if (this.chkActiveAntibackShare.Visible)
            {
                this.dataGridView1.Location = new Point(8, 40);
                if (wgAppConfig.getSystemParamByNO(0x3e) == "2")
                {
                    this.chkGrouped.Checked = true;
                    this.chkGrouped.Visible = true;
                    this.dataGridView1.Location = new Point(8, 0x48);
                }
                this.dataGridView1.Size = new Size(this.dataGridView1.Size.Width, (this.dataGridView1.Size.Height + 8) - this.dataGridView1.Location.Y);
            }
            string strSql = " SELECT ";
            strSql = ((strSql + " f_ControllerNO " + ", f_ControllerSN ") + ", f_AntiBack " + ", f_DoorNames ") + ", t_b_Controller.f_ZoneID " + "  from t_b_Controller ORDER BY f_ControllerNO ";
            wgAppConfig.fillDGVData(ref this.dataGridView1, strSql);
            DataTable table = ((DataView) this.dataGridView1.DataSource).Table;
            DataView view = new DataView(table);
            if (view.Count > 0)
            {
                view.RowFilter = " f_AntiBack > 10";
                if (view.Count > 0)
                {
                    dfrmAntiBack.bDisplayIndoorPersonMax = true;
                }
            }
            new icControllerZone().getAllowedControllers(ref table);
            this.loadOperatorPrivilege();
            this.bLoad = false;
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
            string funName = "mnuAntiBack";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnEdit.Visible = false;
            }
        }
    }
}

