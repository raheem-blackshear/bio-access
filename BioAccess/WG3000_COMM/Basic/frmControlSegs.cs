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

    public partial class frmControlSegs : frmBioAccess
    {
        public frmControlSegs()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmControlSeg seg = new dfrmControlSeg())
            {
                seg.operateMode = "New";
                if (seg.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadControlSegData();
                    this.showUpload();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (this.dgvControlSegs.SelectedRows.Count <= 0)
            {
                if (this.dgvControlSegs.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = this.dgvControlSegs.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = this.dgvControlSegs.SelectedRows[0].Index;
            }
            string str = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvControlSegs.Columns[1].HeaderText, this.dgvControlSegs.Rows[rowIndex].Cells[0].Value.ToString());
            if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                wgAppConfig.runUpdateSql(" DELETE FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.dgvControlSegs.Rows[rowIndex].Cells[0].Value.ToString());
                this.loadControlSegData();
                this.showUpload();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int rowIndex;
            if (this.dgvControlSegs.SelectedRows.Count <= 0)
            {
                if (this.dgvControlSegs.SelectedCells.Count <= 0)
                {
                    return;
                }
                rowIndex = this.dgvControlSegs.SelectedCells[0].RowIndex;
            }
            else
            {
                rowIndex = this.dgvControlSegs.SelectedRows[0].Index;
            }
            using (dfrmControlSeg seg = new dfrmControlSeg())
            {
                seg.curControlSegID = int.Parse(this.dgvControlSegs.Rows[rowIndex].Cells[0].Value.ToString());
                if (seg.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadControlSegData();
                    this.showUpload();
                }
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcel(this.dgvControlSegs, this.Text);
        }

        private void btnHolidayControl_Click(object sender, EventArgs e)
        {
            using (dfrmControlHolidaySet set = new dfrmControlHolidaySet())
            {
                set.ShowDialog();
            }
            this.showUpload();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvControlSegs, this.Text);
        }

        private void dgvControlSegs_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void frmControlSegs_Load(object sender, EventArgs e)
        {
            this.loadOperatorPrivilege();
            this.loadControlSegData();
        }

        private void loadControlSegData()
        {
            DataGridView view;
            string cmdText = " SELECT ";
            cmdText = cmdText + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak, ";
            if (wgAppConfig.IsAccessDB)
            {
                cmdText = ((((((cmdText + "  IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID, ") + " [f_Monday], [f_Tuesday], [f_Wednesday]" + " , [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday] ") + " , Format([f_BeginHMS1],'Short Time')  as [f_BeginHMS1A]" + ",Format([f_EndHMS1],'Short Time')  as [f_EndHMS1A]") + ", Format([f_BeginHMS2],'Short Time')  as [f_BeginHMS2A]" + ",Format([f_EndHMS2],'Short Time')  as [f_EndHMS2A]") + ", Format([f_BeginHMS3],'Short Time')  as [f_BeginHMS3A]" + ",Format([f_EndHMS3],'Short Time')  as [f_EndHMS3A]") + "  ,f_ControlSegIDLinked,f_BeginYMD, f_EndYMD  " + ",   f_ControlByHoliday  ") + " " + "  FROM [t_b_ControlSeg] ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
            }
            else
            {
                cmdText = (((((((cmdText + "   CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),  " + "     ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50), ") + "     ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']') " + "    END AS f_ControlSegID, [f_Monday], [f_Tuesday], [f_Wednesday], [f_Thursday], ") + "   [f_Friday], [f_Saturday], [f_Sunday], " + " ISNULL(CONVERT(char(5), f_BeginHMS1,108) , '00:00') AS [f_BeginHMS1A], ") + " ISNULL(CONVERT(char(5), f_EndHMS1,108) , '00:00')  AS [f_EndHMS1A], " + " ISNULL(CONVERT(char(5), f_BeginHMS2,108) , '00:00')  AS [f_BeginHMS2A], ") + " ISNULL(CONVERT(char(5), f_EndHMS2,108) , '00:00')  AS [f_EndHMS2A], " + " ISNULL(CONVERT(char(5), f_BeginHMS3,108) , '00:00')  AS [f_BeginHMS3A], ") + " ISNULL(CONVERT(char(5), f_EndHMS3,108) , '00:00')  AS [f_EndHMS3A] " + "  ,f_ControlSegIDLinked, ") + "   f_BeginYMD, " + "   f_EndYMD  ") + ",   f_ControlByHoliday  " + "  FROM [t_b_ControlSeg] ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ";
            }
            this.dt = new DataTable();
            this.dv = new DataView(this.dt);
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
                    goto Label_0242;
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
        Label_0242:
            view = this.dgvControlSegs;
            view.AutoGenerateColumns = false;
            view.DataSource = this.dv;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                view.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                view.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
            }
            wgAppConfig.setDisplayFormatDate(view, "f_BeginYMD", wgTools.DisplayFormat_DateYMD);
            wgAppConfig.setDisplayFormatDate(view, "f_EndYMD", wgTools.DisplayFormat_DateYMD);
            using (DataView view2 = new DataView(this.dt))
            {
                view2.RowFilter = "f_ControlByHoliday = 0";
                if (view2.Count == 0)
                {
                    view.Columns["f_ControlByHoliday"].Visible = false;
                }
                else
                {
                    view.Columns["f_ControlByHoliday"].Visible = true;
                }
            }
            if (this.dv.Count > 0)
            {
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
                }
            }
            else
            {
                base.Close();
            }
        }

        private void showUpload()
        {
            if (this.firstShow)
            {
                this.firstShow = true;// false;
                XMessageBox.Show(CommonStr.strNeedUploadControlTimeSeg);
            }
        }
    }
}

