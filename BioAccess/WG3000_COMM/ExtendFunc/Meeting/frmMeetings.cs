namespace WG3000_COMM.ExtendFunc.Meeting
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
    using WG3000_COMM.ResStrings;

    public partial class frmMeetings : frmBioAccess
    {
        private dfrmFind dfrmFind1 = new dfrmFind();
        private DataTable dt;
        private DataView dv;

        public frmMeetings()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmMeetingSet set = new dfrmMeetingSet())
            {
                set.ShowDialog(this);
                this.loadMeetingData();
            }
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            using (dfrmMeetingAdr adr = new dfrmMeetingAdr())
            {
                adr.ShowDialog();
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
            string str = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvMain.Columns[0].HeaderText, this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString());
            if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                wgAppConfig.runUpdateSql(" DELETE FROM t_d_Meeting WHERE [f_MeetingNO]= " + wgTools.PrepareStr(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString()));
                wgAppConfig.runUpdateSql(" DELETE FROM t_d_MeetingConsumer WHERE [f_MeetingNO]= " + wgTools.PrepareStr(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString()));
                this.loadMeetingData();
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
            using (dfrmMeetingSet set = new dfrmMeetingSet())
            {
                set.curMeetingNo = this.dgvMain.Rows[index].Cells[0].Value.ToString();
                if (set.ShowDialog(this) == DialogResult.OK)
                {
                    this.loadMeetingData();
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcel(this.dgvMain, this.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvMain, this.Text);
        }

        private void btnRealtimeSign_Click(object sender, EventArgs e)
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
            try
            {
                DataView view = new DataView(this.dv.Table);
                view.RowFilter = "f_MeetingNO = " + wgTools.PrepareStr(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString());
                if (view.Count > 0)
                {
                    DateTime time = (DateTime) view[0]["f_MeetingDateTime"];
                    if (!(time.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        DateTime time3 = (DateTime) view[0]["f_MeetingDateTime"];
                        if (XMessageBox.Show(((((view[0]["f_MeetingName"].ToString() + "\r\n\r\n" + string.Format(CommonStr.strMeetingDate + ": ", new object[0])) + time3.ToString("yyyy-MM-dd")) + ", " + CommonStr.strMeetingSystemDate + ": ") + DateTime.Now.ToString("yyyy-MM-dd")) + " , " + CommonStr.strMeetingMismatch + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                    dfrmMeetingSign sign = new dfrmMeetingSign();
                    sign.TopMost = true;
                    sign.curMeetingNo = this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString();
                    sign.ShowDialog(this);
                }
            }
            catch
            {
            }
        }

        private void btnStat_Click(object sender, EventArgs e)
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
            try
            {
                DataView view = new DataView(this.dv.Table);
                view.RowFilter = "f_MeetingNO = " + wgTools.PrepareStr(this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString());
                if (view.Count > 0)
                {
                    dfrmMeetingStatDetail detail = new dfrmMeetingStatDetail();
                    detail.TopMost = true;
                    detail.curMeetingNo = this.dgvMain.Rows[rowIndex].Cells[0].Value.ToString();
                    detail.ShowDialog(this);
                }
            }
            catch
            {
            }
        }

        private void dgvMain_DoubleClick(object sender, EventArgs e)
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

        private void frmMeetings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void frmMeetings_KeyDown(object sender, KeyEventArgs e)
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

        private void frmMeetings_Load(object sender, EventArgs e)
        {
            this.loadOperatorPrivilege();
            this.loadMeetingData();
        }

        public static string getStrMeetingIdentity(long id)
        {
            string str = "";
            try
            {
                long num = id;
                if ((num <= 5L) && (num >= 0L))
                {
                    switch (((int) num))
                    {
                        case 0:
                            return CommonStr.strMeetingIdentity0;

                        case 1:
                            return CommonStr.strMeetingIdentity1;

                        case 2:
                            return CommonStr.strMeetingIdentity2;

                        case 3:
                            return CommonStr.strMeetingIdentity3;

                        case 4:
                            return CommonStr.strMeetingIdentity4;

                        case 5:
                            return CommonStr.strMeetingIdentity5;
                    }
                }
                str = id.ToString();
            }
            catch
            {
            }
            return str;
        }

        public static string getStrSignWay(long id)
        {
            string str = "";
            try
            {
                long num = id;
                if ((num <= 2L) && (num >= 0L))
                {
                    switch (((int) num))
                    {
                        case 0:
                            return CommonStr.strSignWay0;

                        case 1:
                            return CommonStr.strSignWay1;

                        case 2:
                            return CommonStr.strSignWay2;
                    }
                }
                str = id.ToString();
            }
            catch
            {
            }
            return str;
        }

        private void loadMeetingData()
        {
            DataGridView view;
            string cmdText = "SELECT [f_MeetingNO], [f_MeetingName], [f_MeetingDateTime], [f_MeetingAdr], [f_Content], [f_Notes] FROM t_d_Meeting ";
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
                    goto Label_00CB;
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
        Label_00CB:
            view = this.dgvMain;
            view.AutoGenerateColumns = false;
            view.DataSource = this.dv;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                view.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                view.Columns[i].Name = this.dv.Table.Columns[i].ColumnName;
            }
            wgAppConfig.setDisplayFormatDate(view, "f_MeetingDateTime", wgTools.DisplayFormat_DateYMDHMSWeek);
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
            bool bReadOnly = false;
            string funName = "mnuMeeting";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnEdit.Visible = false;
                this.btnDelete.Visible = false;
                this.btnRealtimeSign.Visible = false;
            }
        }
    }
}

