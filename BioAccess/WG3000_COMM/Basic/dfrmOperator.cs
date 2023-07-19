namespace WG3000_COMM.Basic
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

    public partial class dfrmOperator : frmBioAccess
    {
        public dfrmOperator()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmOperatorUpdate update = new dfrmOperatorUpdate())
            {
                update.operateMode = 0;
                update.Text = this.btnAdd.Text + " " + update.Text;
                if (update.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadOperatorData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvOperators.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvOperators.Rows.Count > 0)
                {
                    rowIndex = this.dgvOperators.CurrentCell.RowIndex;
                }
                if (int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                {
                    XMessageBox.Show(this, CommonStr.strDeleteForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (XMessageBox.Show(this, CommonStr.strDelete + " " + this.dgvOperators[1, rowIndex].Value.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
                {
                    wgAppConfig.runUpdateSql(" DELETE FROM [t_s_Operator] " + "WHERE  [f_OperatorID]=" + this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString());
                    this.loadOperatorData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvOperators.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvOperators.Rows.Count > 0)
                {
                    rowIndex = this.dgvOperators.CurrentCell.RowIndex;
                }
                using (dfrmOperatorUpdate update = new dfrmOperatorUpdate())
                {
                    update.Text = this.btnEdit.Text + " " + update.Text;
                    update.operateMode = 1;
                    update.operatorID = int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString());
                    update.operatorName = this.dgvOperators.Rows[rowIndex].Cells[1].Value.ToString();
                    if (update.ShowDialog(this) == DialogResult.OK)
                    {
                        this.loadOperatorData();
                    }
                }
            }
        }

        private void btnEditDepartment_Click(object sender, EventArgs e)
        {
            if (this.dgvOperators.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvOperators.Rows.Count > 0)
                {
                    rowIndex = this.dgvOperators.CurrentCell.RowIndex;
                }
                if (int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                {
                    XMessageBox.Show(this, CommonStr.strEditOperatePrivilegeForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    using (dfrmOperatorDepartmentsConfiguration configuration = new dfrmOperatorDepartmentsConfiguration())
                    {
                        configuration.operatorId = int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString());
                        configuration.ShowDialog(this);
                    }
                }
            }
        }

        private void btnEditPrivilege_Click(object sender, EventArgs e)
        {
            if (this.dgvOperators.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvOperators.Rows.Count > 0)
                {
                    rowIndex = this.dgvOperators.CurrentCell.RowIndex;
                }
                if (int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                {
                    XMessageBox.Show(this, CommonStr.strEditOperatePrivilegeForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    using (dfrmOperatePrivilege privilege = new dfrmOperatePrivilege())
                    {
                        privilege.Text = this.dgvOperators.Rows[rowIndex].Cells[1].Value.ToString() + "--" + privilege.Text;
                        privilege.operatorID = int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString());
                        privilege.ShowDialog(this);
                    }
                }
            }
        }

        private void btnEditZones_Click(object sender, EventArgs e)
        {
            if (this.dgvOperators.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvOperators.Rows.Count > 0)
                {
                    rowIndex = this.dgvOperators.CurrentCell.RowIndex;
                }
                if (int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                {
                    XMessageBox.Show(this, CommonStr.strEditOperatePrivilegeForbidden, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    using (dfrmOperatorZonesConfiguration configuration = new dfrmOperatorZonesConfiguration())
                    {
                        configuration.operatorId = int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString());
                        configuration.ShowDialog(this);
                    }
                }
            }
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            if (this.dgvOperators.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvOperators.Rows.Count > 0)
                {
                    rowIndex = this.dgvOperators.CurrentCell.RowIndex;
                }
                using (dfrmOperatorUpdate update = new dfrmOperatorUpdate())
                {
                    update.Text = this.btnSetPassword.Text;
                    update.operateMode = 2;
                    update.operatorID = int.Parse(this.dgvOperators.Rows[rowIndex].Cells[0].Value.ToString());
                    update.operatorName = this.dgvOperators.Rows[rowIndex].Cells[1].Value.ToString();
                    update.ShowDialog(this);
                }
            }
        }

        private void dfrmOperator_Load(object sender, EventArgs e)
        {
            this.btnEditDepartment.Text = wgAppConfig.ReplaceFloorRomm(this.btnEditDepartment.Text);
            this.loadOperatorPrivilege();
            this.loadOperatorData();
        }

        private void loadOperatorData()
        {
            string cmdText = " SELECT f_OperatorID, f_OperatorName";
            cmdText = cmdText + " FROM t_s_Operator " + "  ORDER BY f_OperatorID ";
            this.table = new DataTable();
            this.dv = new DataView(this.table);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.table);
                        }
                    }
                    goto Label_00E3;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.table);
                    }
                }
            }
        Label_00E3:
            this.dgvOperators.AutoGenerateColumns = false;
            this.dgvOperators.DataSource = this.dv;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                this.dgvOperators.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
            }
            if (this.dv.Count > 0)
            {
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
            }
            else
            {
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
            }
        }

        private void loadOperatorPrivilege()
        {
            bool flag;
            bool flag2;
            icOperator.getFrmOperatorPrivilege(base.Name.ToString(), out flag, out flag2);
            if (flag || flag2)
            {
                if (!flag2 && flag)
                {
                    this.btnAdd.Visible = false;
                    this.btnEdit.Visible = false;
                    this.btnDelete.Visible = false;
                    this.btnSetPassword.Visible = false;
                    this.btnEditPrivilege.Visible = false;
                    this.toolStrip1.Visible = false;
                }
            }
            else
            {
                base.Close();
            }
        }
    }
}

