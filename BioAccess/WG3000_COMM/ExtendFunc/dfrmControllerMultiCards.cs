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

    public partial class dfrmControllerMultiCards : frmBioAccess
    {
        private dfrmFind dfrmFind1 = new dfrmFind();

        public dfrmControllerMultiCards()
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
            using (dfrmMultiCards cards = new dfrmMultiCards())
            {
                cards.DoorID = int.Parse(view.Rows[num].Cells[0].Value.ToString());
                cards.Text = cards.Text + "[" + view.Rows[num].Cells[2].Value.ToString() + "   " + view.Rows[num].Cells[3].Value.ToString() + "]";
                if (cards.ShowDialog(this) == DialogResult.OK)
                {
                    view.Rows[num].Cells["f_MoreCards_Total"].Value = cards.retValue;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void dfrmControllerMultiCards_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmControllerMultiCards_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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

        private void dfrmControllerMultiCards_Load(object sender, EventArgs e)
        {
            string strSql = " SELECT ";
            strSql = (((strSql + " t_b_Door.f_DoorID " + ", t_b_Controller.f_ControllerSN ") + ", t_b_Door.f_DoorNO " + ", t_b_Door.f_DoorName ") + ", t_b_Door.f_MoreCards_Total " + ", t_b_Controller.f_ZoneID ") + " from t_b_Controller,t_b_Door WHERE t_b_Controller.f_ControllerID=t_b_Door.f_ControllerID " + " ORDER BY t_b_Door.f_DoorID ";
            wgAppConfig.fillDGVData(ref this.dataGridView1, strSql);
            DataTable dtController = ((DataView) this.dataGridView1.DataSource).Table;
            new icControllerZone().getAllowedControllers(ref dtController);
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
            string funName = "mnuMoreCards";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnEdit.Visible = false;
                this.dataGridView1.ReadOnly = true;
            }
        }
    }
}

