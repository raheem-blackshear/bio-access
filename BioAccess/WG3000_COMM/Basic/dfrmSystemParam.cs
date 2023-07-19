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

    public partial class dfrmSystemParam : frmBioAccess
    {
        public dfrmSystemParam()
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
            DataTable table = (this.dataGridView1.DataSource as DataView).Table;
            for (int i = 0; i <= (table.Rows.Count - 1); i++)
            {
                if (wgTools.SetObjToStr(table.Rows[i]["f_Value"]) != wgTools.SetObjToStr(table.Rows[i]["f_OldValue"]))
                {
                    wgAppConfig.runUpdateSql(((" UPDATE t_a_SystemParam SET " + " f_Value = " + wgTools.PrepareStr(table.Rows[i]["f_Value"].ToString())) + " , f_Modified = " + wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))) + " WHERE f_NO = " + table.Rows[i]["f_NO"].ToString());
                }
            }
            if (XMessageBox.Show(CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }

        private void dfrmSystemParam_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
                    {
                        this.dataGridView1.Columns[i].Visible = true;
                    }
                }
            }
        }

        private void dfrmSystemParam_Load(object sender, EventArgs e)
        {
            this.fillSystemParam();
        }

        private void fillSystemParam()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.fillSystemParam_Acc();
            }
            else
            {
                string cmdText = " SELECT ";
                cmdText = (cmdText + " f_NO, f_Name, f_Value, f_EName, f_Notes, f_Modified, f_Value as f_OldValue ") + " FROM t_a_SystemParam " + " ORDER BY [f_NO] ";
                this.dt = new DataTable();
                this.dv = new DataView(this.dt);
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(this.dt);
                        }
                    }
                }
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = this.dv;
                for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dataGridView1.ColumnCount); i++)
                {
                    this.dataGridView1.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                }
                this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            }
        }

        private void fillSystemParam_Acc()
        {
            string cmdText = " SELECT ";
            cmdText = (cmdText + " f_NO, f_Name, f_Value, f_EName, f_Notes, f_Modified, f_Value as f_OldValue ") + " FROM t_a_SystemParam " + " ORDER BY [f_NO] ";
            this.dt = new DataTable();
            this.dv = new DataView(this.dt);
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
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = this.dv;
            for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dataGridView1.ColumnCount); i++)
            {
                this.dataGridView1.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
            }
            this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
        }
    }
}

