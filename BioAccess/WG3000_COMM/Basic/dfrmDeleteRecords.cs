namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmDeleteRecords : frmBioAccess
    {
        public dfrmDeleteRecords()
        {
            this.InitializeComponent();
        }

        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            using (dfrmDbCompact compact = new dfrmDbCompact())
            {
                compact.ShowDialog(this);
            }
        }

        private void btnDeleteAllSwipeRecords_Click(object sender, EventArgs e)
        {
            if (XMessageBox.Show((sender as Button).Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                try
                {
                    if (wgAppConfig.IsAccessDB)
                    {
                        wgAppConfig.runUpdateSql("Delete From t_d_SwipeRecord");
                    }
                    else
                    {
                        wgAppConfig.runUpdateSql("TRUNCATE TABLE t_d_SwipeRecord");
                    }
                    wgAppConfig.wgLog((sender as Button).Text);
                    XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
                    base.Close();
                }
                catch (Exception)
                {
                    XMessageBox.Show(this.Text + CommonStr.strFailed);
                }
            }
        }

        private void btnDeleteLog_Click(object sender, EventArgs e)
        {
            if (XMessageBox.Show((sender as Button).Text + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                try
                {
                    if (wgAppConfig.IsAccessDB)
                    {
                        wgAppConfig.runUpdateSql("Delete From t_s_wglog");
                    }
                    else
                    {
                        wgAppConfig.runUpdateSql("TRUNCATE TABLE t_s_wglog");
                    }
                    wgAppConfig.wgLog((sender as Button).Text);
                    XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
                    base.Close();
                }
                catch (Exception)
                {
                    XMessageBox.Show(this.Text + CommonStr.strFailed);
                }
            }
        }

        private void btnDeleteOldSwipeRecords_Click(object sender, EventArgs e)
        {
            string text = "";
            text = (sender as Button).Text + ": " + this.lblIndex.Text + this.nudSwipeRecordIndex.Value.ToString();
            if (this.nudIndexMin.Visible)
            {
                text = text + " ,   " + this.lblIndexMin.Text + this.nudIndexMin.Value.ToString();
            }
            text = text + "? ";
            if (XMessageBox.Show(text, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            {
                return;
            }
            int num = -1;
            Cursor.Current = Cursors.WaitCursor;
            string cmdText = "DELETE FROM t_d_SwipeRecord Where f_RecID <" + this.nudSwipeRecordIndex.Value.ToString();
            if (this.nudIndexMin.Visible)
            {
                cmdText = cmdText + "  AND  f_RecID >= " + this.nudIndexMin.Value.ToString();
            }
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                        num = command.ExecuteNonQuery();
                    }
                    goto Label_0198;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    command2.CommandTimeout = wgAppConfig.dbCommandTimeout;
                    num = command2.ExecuteNonQuery();
                }
            }
        Label_0198:
            Cursor.Current = Cursors.Default;
            if (num >= 0)
            {
                wgAppConfig.wgLog((sender as Button).Text + ": " + cmdText);
                wgAppConfig.wgLogWithoutDB(text + "\r\n" + cmdText, EventLogEntryType.Information, null);
                XMessageBox.Show(this.Text + CommonStr.strSuccessfully);
                base.Close();
            }
            else
            {
                XMessageBox.Show(this.Text + CommonStr.strFailed);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void dfrmDeleteRecords_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.lblIndexMin.Visible = true;
                    this.nudIndexMin.Visible = true;
                }
            }
        }

        private void dfrmDeleteRecords_Load(object sender, EventArgs e)
        {
        }
    }
}

