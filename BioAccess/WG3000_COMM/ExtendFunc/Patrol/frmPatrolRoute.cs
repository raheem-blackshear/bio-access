namespace WG3000_COMM.ExtendFunc.Patrol
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

    public partial class frmPatrolRoute
    {
        private DataTable dt;
        private DataView dv;

        public frmPatrolRoute()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmRouteEdit edit = new dfrmRouteEdit())
            {
                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (this.dgvMain.SelectedRows.Count <= 0)
            {
                if (this.dgvMain.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = this.dgvMain.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = this.dgvMain.SelectedRows[0].Index;
            }
            string str = string.Format("{0}\r\n{1}:  {2}", this.btnDelete.Text, this.dgvMain.Columns[0].HeaderText, this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString());
            str = string.Format(CommonStr.strAreYouSure + " {0} ?", str);
            if (XMessageBox.Show(this, str, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int num2 = int.Parse(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString());
                wgAppConfig.runUpdateSql(" DELETE FROM t_d_PatrolRouteList WHERE f_RouteID = " + num2.ToString());
                wgAppConfig.runUpdateSql(" DELETE FROM t_d_PatrolRouteDetail WHERE f_RouteID = " + num2.ToString());
                this.loadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index;
            int rowIndex = 0;
            if (this.dgvMain.Rows.Count > 0)
            {
                rowIndex = this.dgvMain.CurrentCell.RowIndex;
            }
            if (this.dgvMain.SelectedRows.Count <= 0)
            {
                if (this.dgvMain.SelectedCells.Count <= 0)
                {
                    return;
                }
                index = this.dgvMain.SelectedCells[0].RowIndex;
            }
            else
            {
                index = this.dgvMain.SelectedRows[0].Index;
            }
            using (dfrmRouteEdit edit = new dfrmRouteEdit())
            {
                edit.currentRouteID = int.Parse(this.dgvMain.Rows[index].Cells[0].Value.ToString());
                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadData();
                }
            }
            if (this.dgvMain.RowCount > 0)
            {
                if (this.dgvMain.RowCount > rowIndex)
                {
                    this.dgvMain.CurrentCell = this.dgvMain[1, rowIndex];
                }
                else
                {
                    this.dgvMain.CurrentCell = this.dgvMain[1, this.dgvMain.RowCount - 1];
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcel(this.dgvMain, this.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvMain, this.Text);
        }

        private void dgvControlSegs_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmShiftOtherTypes_Load(object sender, EventArgs e)
        {
            this.Refresh();
            this.loadOperatorPrivilege();
            this.loadData();
        }

        private void loadData()
        {
            DataGridView view;
            int num;
            string cmdText = "";
            this.dt = new DataTable();
            this.dv = new DataView(this.dt);
            cmdText = " SELECT ";
            cmdText = cmdText + " [f_RouteID], [f_RouteName] " + "  FROM [t_d_PatrolRouteList] ORDER BY [f_RouteID] ASC  ";
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
                    goto Label_00E9;
                }
            }
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
        Label_00E9:
            view = this.dgvMain;
            view.AutoGenerateColumns = false;
            view.DataSource = this.dv;
            for (num = 0; num < this.dv.Table.Columns.Count; num++)
            {
                view.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
            }
            if (this.dv.Count > 0)
            {
                for (num = 0; num < this.dv.Count; num++)
                {
                }
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
            }
            else
            {
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuPatrolDetailData";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnEdit.Visible = false;
                this.btnDelete.Visible = false;
            }
        }
    }
}

