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

    public partial class frmShiftOtherTypes : frmBioAccess
    {
        private DataTable dt;
        private DataView dv;

        public frmShiftOtherTypes()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmShiftOtherTypeSet set = new dfrmShiftOtherTypeSet())
            {
                set.operateMode = "New";
                if (set.ShowDialog(this) == DialogResult.OK)
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
            if (XMessageBox.Show(this, str, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            if (wgAppConfig.IsAccessDB)
            {
                using (comShift_Acc acc = new comShift_Acc())
                {
                    acc.shift_delete(int.Parse(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString()));
                    goto Label_015F;
                }
            }
            using (comShift shift = new comShift())
            {
                shift.shift_delete(int.Parse(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString()));
            }
        Label_015F:
            this.loadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
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
            using (dfrmShiftOtherTypeSet set = new dfrmShiftOtherTypeSet())
            {
                set.curShiftID = int.Parse(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString());
                if (set.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadData();
                }
            }
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
            if (wgAppConfig.IsAccessDB)
            {
                cmdText = " SELECT ";
                cmdText = ((((cmdText + " [f_ShiftID], [f_ShiftName], [f_ReadTimes], [f_bOvertimeShift]" + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty1],'Short Time') )   AS [f_OnDuty1t] ") + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty1],'Short Time') )  AS [f_OffDuty1t] " + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty2],'Short Time') )   AS [f_OnDuty2t] ") + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty2],'Short Time') )  AS [f_OffDuty2t] " + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty3],'Short Time') )   AS [f_OnDuty3t] ") + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty3],'Short Time') )  AS [f_OffDuty3t] " + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OnDuty4],'Short Time') )   AS [f_OnDuty4t] ") + " ,  IIF(f_OnDuty1 IS NULL , ' ',  Format([f_OffDuty4],'Short Time') )  AS [f_OffDuty4t] " + "  FROM [t_b_ShiftSet] ORDER BY [f_ShiftID] ASC  ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dt);
                        }
                    }
                    goto Label_01CD;
                }
            }
            cmdText = " SELECT ";
            cmdText = ((((cmdText + " [f_ShiftID], [f_ShiftName], [f_ReadTimes], [f_bOvertimeShift]" + " ,ISNULL(CONVERT(char(5), f_OnDuty1,108) , ' ') AS [f_OnDuty1t] ") + " ,ISNULL(CONVERT(char(5), f_OffDuty1,108) , ' ') AS [f_OffDuty1t] " + " ,ISNULL(CONVERT(char(5), f_OnDuty2,108) , ' ') AS [f_OnDuty2t] ") + " ,ISNULL(CONVERT(char(5), f_OffDuty2,108) , ' ') AS [f_OffDuty2t] " + " ,ISNULL(CONVERT(char(5), f_OnDuty3,108) , ' ') AS [f_OnDuty3t] ") + " ,ISNULL(CONVERT(char(5), f_OffDuty3,108) , ' ') AS [f_OffDuty3t] " + " ,ISNULL(CONVERT(char(5), f_OnDuty4,108) , ' ') AS [f_OnDuty4t] ") + " ,ISNULL(CONVERT(char(5), f_OffDuty4,108) , ' ') AS [f_OffDuty4t] " + "  FROM [t_b_ShiftSet] ORDER BY [f_ShiftID] ASC  ";
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
        Label_01CD:
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
            string funName = "mnuShiftSet";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnEdit.Visible = false;
                this.btnDelete.Visible = false;
            }
        }
    }
}

