using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Collections;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
    public partial class frmUsers : Form
    {
        public WatchingService watching;
        private bool bLoadedFinished;
        private int deletedUserCnt;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private string dgvSql;
        private DataSet DS;
        private DataView dv;
        private DataView dv4loadUserData;
        private int MaxRecord = 0x3e8;
        private OleDbDataAdapter MyCommand;
        private OleDbConnection MyConnection;
        private int recNOMax = 0;
        private int startRecordIndex;
        private DataTable tb4loadUserData;

        public frmUsers()
        {
            InitializeComponent();
        }

        private bool _addConsumer4Import(string no, string name, string strCard, int password, string dept)
        {
            long result = 0L;
            icConsumer consumer = new icConsumer();
            if (!string.IsNullOrEmpty(strCard))
            {
                long.TryParse(strCard, out result);
            }
            if (string.IsNullOrEmpty(dept))
            {
                return (consumer.addNew(no, name, result, password, 0) >= 0);
            }
            return (consumer.addNew(no, name, result, password, this.getDeptId(dept)) >= 0);
        }

        public int addConsumerNew(string no, string name, string strCard, int password, string dept)
        {
            icConsumer consumer = new icConsumer();
            if (strCard != "")
            {
                long num2;
                bool flag = false;
                if (long.TryParse(strCard, out num2))
                {
                    if (strCard.ToUpper().IndexOf("E") >= 0)
                        return -1;

                    long num3 = long.Parse(strCard);
                    strCard = num3.ToString();
                    if (num3 <= 0L)
                        return -1;

                    if (consumer.isExisted(long.Parse(strCard)))
                        return -1;

                    flag = true;
                }
                if (!flag)
                    return -1;
            }
            if (password != 0 && consumer.isExistPassword(password))
                return -1;
            
            if (this._addConsumer4Import(no, name, strCard, password, dept))
                return 1;

            return -1;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            int startIndex = (int) ((object[]) e.Argument)[0];
            int maxRecords = (int) ((object[]) e.Argument)[1];
            string strSql = (string) ((object[]) e.Argument)[2];
            e.Result = this.loadUserData(startIndex, maxRecords, strSql);
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
            }
            else if (e.Error != null)
            {
                wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
            }
            else
            {
                if ((e.Result as DataView).Count < this.MaxRecord)
                {
                    this.bLoadedFinished = true;
                }
                this.fillDgv(e.Result as DataView);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + (this.bLoadedFinished ? "#" : "..."));
            }
        }

        private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvUsers.SelectedRows.Count > 0)
            {
                using (dfrmUserBatchUpdate update = new dfrmUserBatchUpdate())
                {
                    string str = "";
                    for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i++)
                    {
                        int index = this.dgvUsers.SelectedRows[i].Index;
                        int num2 = int.Parse(this.dgvUsers.Rows[index].Cells[0].Value.ToString());
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str + ",";
                        }
                        str = str + num2.ToString();
                    }
                    update.strSqlSelected = str;
                    update.Text = string.Format("{0}: [{1}]", this.batchUpdateSelectToolStripMenuItem.Text, this.dgvUsers.SelectedRows.Count.ToString());
                    if (update.ShowDialog(this) == DialogResult.OK)
                    {
                        this.reloadUserData("");
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            DataGridView dgvUsers = this.dgvUsers;
            DataGridViewColumn sortedColumn = dgvUsers.SortedColumn;
            ListSortDirection ascending = ListSortDirection.Ascending;
            if ((sortedColumn != null) && (dgvUsers.SortOrder == System.Windows.Forms.SortOrder.Descending))
            {
                ascending = ListSortDirection.Descending;
            }
            if (dgvUsers.Rows.Count > 0)
            {
                rowIndex = dgvUsers.CurrentCell.RowIndex;
            }
            using (dfrmUser user = new dfrmUser())
            {
                if (user.ShowDialog(this) == DialogResult.OK)
                {
                    this.reloadUserData("");
                    if (dgvUsers.RowCount > 0)
                    {
                        if (dgvUsers.RowCount > rowIndex)
                        {
                            dgvUsers.CurrentCell = dgvUsers[1, rowIndex];
                        }
                        else
                        {
                            dgvUsers.CurrentCell = dgvUsers[1, dgvUsers.RowCount - 1];
                        }
                    }
                    if (sortedColumn != null)
                    {
                        dgvUsers.Sort(sortedColumn, ascending);
                    }
                }
            }
        }

        private void btnAutoAdd_Click(object sender, EventArgs e)
        {
            using (dfrmUserAutoAdd add = new dfrmUserAutoAdd())
            {
                add.watching = this.watching;
                add.frmCall = this;
                if (add.ShowDialog(this) == DialogResult.OK)
                {
                    this.reloadUserData("");
                }
            }
        }

        private void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            using (dfrmUserBatchUpdate update = new dfrmUserBatchUpdate())
            {
                if (update.ShowDialog(this) == DialogResult.OK)
                {
                    this.reloadUserData("");
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvUsers.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvUsers.SelectedRows[0].Index;
                }
                int consumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells[0].Value.ToString());
                if (consumerID > 0)
                {
                    string str;
                    if (this.dgvUsers.SelectedRows.Count == 1)
                    {
                        str = string.Format("{0}\r\n\r\n{1}:  {2}", this.btnDelete.Text, this.dgvUsers.Columns[2].HeaderText, this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString());
                    }
                    else
                    {
                        str = string.Format("{0}\r\n\r\n{1}=  {2}", this.btnDelete.Text, CommonStr.strUsersNum, this.dgvUsers.SelectedRows.Count);
                    }
                    if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", str), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        icConsumer consumer = new icConsumer();
                        if (this.dgvUsers.SelectedRows.Count == 1)
                        {
                            str = string.Format("{0} {1}:  {2}", this.btnDelete.Text, this.dgvUsers.Columns[2].HeaderText, this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString());
                        }
                        else
                        {
                            str = string.Format("{0} {1}=  {2}", this.btnDelete.Text, CommonStr.strUsersNum, this.dgvUsers.SelectedRows.Count) + string.Format("From {0}...", this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString());
                        }
                        int count = this.dgvUsers.SelectedRows.Count;
                        if (this.dgvUsers.SelectedRows.Count == 1)
                        {
                            consumer.deleteUser(consumerID);
                        }
                        else
                        {
                            for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i++)
                            {
                                rowIndex = this.dgvUsers.SelectedRows[i].Index;
                                consumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells[0].Value.ToString());
                                consumer.deleteUser(consumerID);
                            }
                        }
                        foreach (DataGridViewRow row in this.dgvUsers.SelectedRows)
                        {
                            this.dgvUsers.Rows.Remove(row);
                        }
                        this.deletedUserCnt += count;
                        wgAppConfig.wgLog(str, EventLogEntryType.Information, null);
                        if (this.bLoadedFinished)
                        {
                            wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
                        }
                    }
                    icConsumerShare.setUpdateLog();
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvUsers.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvUsers.SelectedRows[0].Index;
                }
                using (dfrmUser user = new dfrmUser())
                {
                    user.consumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells[0].Value.ToString());
                    user.OperateNew = false;
                    if (user.ShowDialog(this) == DialogResult.OK)
                    {
                        DataGridViewRow row = this.dgvUsers.Rows[rowIndex];
                        string cmdText;
                        if (wgAppConfig.IsAccessDB)
                        {
                            cmdText = " SELECT a.f_ConsumerID, a.f_ConsumerNO, a.f_ConsumerName, " +
                                "c.f_FpCount AS f_HasFp, " +
                                "d.f_FaceCount AS f_HasFace, " +
                                "a.f_CardNO, a.f_PIN, a.f_AttendEnabled, a.f_ShiftEnabled, a.f_DoorEnabled, a.f_BeginYMD, a.f_EndYMD, t_b_Group.f_GroupName " +
                                "FROM (((t_b_Consumer a LEFT OUTER JOIN t_b_Group ON a.f_GroupID = t_b_Group.f_GroupID) " +
                                "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FpCount, f_ConsumerID FROM t_d_FpTempl GROUP BY f_ConsumerID) c ON a.f_ConsumerID = c.f_ConsumerID) " +
                                "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FaceCount, f_ConsumerID FROM t_d_FaceTempl GROUP BY f_ConsumerID) d ON a.f_ConsumerID = d.f_ConsumerID) " +
                                "WHERE a.f_ConsumerID = " + user.consumerID.ToString();
                            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                            {
                                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                                {
                                    connection.Open();
                                    OleDbDataReader reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        for (int i = 1; i < this.dgvUsers.Columns.Count; i++)
                                            row.Cells[i].Value = reader[i];
                                    }
                                    reader.Close();
                                }
                                return;
                            }
                        }
                        using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                        {
                            cmdText = " SELECT a.f_ConsumerID, a.f_ConsumerNO, a.f_ConsumerName, " +
                                "c.f_FpCount AS f_HasFp, " +
                                "d.f_FaceCount AS f_HasFace, " +
                                "a.f_CardNO, a.f_PIN, a.f_AttendEnabled, a.f_ShiftEnabled, a.f_DoorEnabled, a.f_BeginYMD, a.f_EndYMD, t_b_Group.f_GroupName " +
                                "FROM (((t_b_Consumer a LEFT OUTER JOIN t_b_Group ON a.f_GroupID = t_b_Group.f_GroupID) " +
                                "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FpCount, f_ConsumerID FROM t_d_FpTempl GROUP BY f_ConsumerID) c ON a.f_ConsumerID = c.f_ConsumerID) " +
                                "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FaceCount, f_ConsumerID FROM t_d_FaceTempl GROUP BY f_ConsumerID) d ON a.f_ConsumerID = d.f_ConsumerID) " +
                                "WHERE a.f_ConsumerID = " + user.consumerID.ToString();
                            using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                            {
                                connection2.Open();
                                SqlDataReader reader2 = command2.ExecuteReader();
                                if (reader2.Read())
                                {
                                    for (int j = 1; j < this.dgvUsers.Columns.Count; j++)
                                        row.Cells[j].Value = reader2[j];
                                }
                                reader2.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnEditPrivilege_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvUsers.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvUsers.SelectedRows[0].Index;
                }
                using (dfrmPrivilegeSingle num2 = new dfrmPrivilegeSingle())
                {
                    num2.consumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells[0].Value.ToString());
                    num2.Text = this.dgvUsers.Rows[rowIndex].Cells[1].Value.ToString().Trim() + "." + this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString().Trim() + " -- " + num2.Text;
                    num2.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcelSpecial(ref this.dgvUsers, this.Text, this.bLoadedFinished, ref this.backgroundWorker1, ref this.startRecordIndex, this.MaxRecord, this.dgvSql);
        }

        private void btnImportFromExcel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string fileName;
                XMessageBox.Show(wgAppConfig.ReplaceWorkNO(wgAppConfig.ReplaceFloorRomm(CommonStr.strImportInformation)));
                this.openFileDialog1.Filter = " (*.xls)|*.xls| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                try
                {
                    this.openFileDialog1.InitialDirectory = @".\REPORT";
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
                this.openFileDialog1.Title = this.btnImportFromExcel.Text;
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                }
                else
                {
                    return;
                }
                this.dfrmWait1.Show();
                this.dfrmWait1.Refresh();
                wgTools.WriteLine("start");

                /* Clear user list */
                if (dgvUsers.Rows.Count > 0)
                {
                    icConsumer consumer = new icConsumer();
                    for (int i = 0; i < dgvUsers.Rows.Count; i++)
                        consumer.deleteUser(int.Parse(this.dgvUsers.Rows[i].Cells[0].Value.ToString()));
                    foreach (DataGridViewRow row in this.dgvUsers.Rows)
                        this.dgvUsers.Rows.Remove(row);
                }

                int num = 0;
                int num2 = 1;
                int num3 = 2;
                int num4 = 3;
                int num5 = -1;
                this.MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
                string format = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE\"";
                if (fileName != string.Empty)
                {
                    FileInfo info = new FileInfo(fileName);
                    if (info.Extension.Equals(".xls"))
                    {
                        this.MyConnection = new OleDbConnection(string.Format(format, new object[] { "Jet", "4.0", fileName, "8.0" }));
                    }
                    else if (info.Extension.Equals(".xlsx"))
                    {
                        this.MyConnection = new OleDbConnection(string.Format(format, new object[] { "Ace", "12.0", fileName, "12.0" }));
                    }
                    else
                    {
                        this.MyConnection = new OleDbConnection(string.Format(format, new object[] { "Jet", "4.0", fileName, "8.0" }));
                    }
                }
                this.DS = new DataSet();
                this.MyConnection.Open();
                DataTable oleDbSchemaTable = this.MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                this.MyConnection.Close();
                string str3 = "";
                if (oleDbSchemaTable.Rows.Count <= 0)
                {
                    XMessageBox.Show(this.btnImportFromExcel.Text + ": " + 0);
                }
                else
                {
                    str3 = wgTools.SetObjToStr(oleDbSchemaTable.Rows[0][2]);
                    for (int i = 0; i <= (oleDbSchemaTable.Rows.Count - 1); i++)
                    {
                        if (((wgTools.SetObjToStr(oleDbSchemaTable.Rows[i][2]) == "用户") || (wgTools.SetObjToStr(oleDbSchemaTable.Rows[i][2]) == "用戶")) || (wgTools.SetObjToStr(oleDbSchemaTable.Rows[i][2]) == "Users"))
                        {
                            str3 = wgTools.SetObjToStr(oleDbSchemaTable.Rows[i][2]);
                            break;
                        }
                    }
                    num = -1;
                    num2 = -1;
                    num3 = -1;
                    num4 = -1;
                    num5 = -1;
                    if (str3.IndexOf("$") <= 0)
                    {
                        str3 = str3 + "$";
                    }
                    try
                    {
                        this.MyCommand = new OleDbDataAdapter("select * from [" + str3 + "A1:Z1]", this.MyConnection);
                        this.MyCommand.Fill(this.DS, "userInfoTitle");
                        string columnName = this.DS.Tables["userInfoTitle"].Columns[0].ColumnName;
                        for (int j = 0; j <= (this.DS.Tables["userInfoTitle"].Columns.Count - 1); j++)
                        {
                            object valA = this.DS.Tables["userInfoTitle"].Columns[j].ColumnName;
                            if (wgTools.SetObjToStr(valA) != "")
                            {
                                if (wgTools.SetObjToStr(valA).ToUpper() == "User ID".ToUpper())
                                {
                                    num = j;
                                }
                                else if (wgTools.SetObjToStr(valA).ToUpper() == "User Name".ToUpper())
                                {
                                    num2 = j;
                                }
                                else if (wgTools.SetObjToStr(valA).ToUpper() == "Card NO".ToUpper())
                                {
                                    num3 = j;
                                }
                                else if ((wgTools.SetObjToStr(valA).ToUpper() == "Department".ToUpper()) || (wgTools.SetObjToStr(valA).ToUpper() == CommonStr.strReplaceFloorRoom.ToUpper()))
                                {
                                    num4 = j;
                                }
                                else
                                {
                                    switch (wgTools.SetObjToStr(valA).ToUpper().Substring(0, 2))
                                    {
                                        case "NO":
                                        case "用户":
                                        case "用戶":
                                        case "编号":
                                        case "編號":
                                        case "WO":
                                        case "工号":
                                        case "工號":
                                            num = j;
                                            break;

                                        case "NA":
                                        case "姓名":
                                            num2 = j;
                                            break;

                                        case "CA":
                                        case "卡号":
                                        case "卡號":
                                            num3 = j;
                                            break;

                                        case "DE":
                                        case "部门":
                                        case "部門":
                                            num4 = j;
                                            break;

                                        case "PA":
                                        case "PI":
                                        case "密码":
                                            num5 = j;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                        wgAppConfig.wgLog(exception2.ToString());
                    }
                    if (num2 < 0)
                    {
                        XMessageBox.Show(CommonStr.strWrongUsersFile);
                    }
                    else
                    {
                        string str4 = "";
                        int num10 = 0;
                        try
                        {
                            int startIndex = 0;
                            startIndex = Math.Max(Math.Max(Math.Max(num, num2), num3), num4);
                            string str5 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            if (startIndex < str5.Length)
                            {
                                this.MyCommand = new OleDbDataAdapter("select * from [" + str3 + "A1:" + str5.Substring(startIndex, 1) + "65535]", this.MyConnection);
                                this.MyCommand.Fill(this.DS, "userInfo");
                            }
                        }
                        catch (Exception exception3)
                        {
                            wgTools.WgDebugWrite(exception3.ToString(), new object[0]);
                            wgAppConfig.wgLog(exception3.ToString());
                        }
                        this.dv = new DataView(this.DS.Tables["userInfo"]);
                        int num9 = 0;
                        long num13 = new icConsumer().ConsumerNONext();
                        long num12 = num13;
                        if (num13 < 0L)
                        {
                            num12 = 1L;
                        }
                        string strCard = "";
                        string s = "";
                        string strPassword = "";
                        string dept = "";
                        string name = "";
                        int password = 0;
                        new icConsumer();
                        for (int k = 0; k <= (this.dv.Count - 1); k++)
                        {
                            int num15;
                            if (((wgTools.SetObjToStr(this.dv[k][num2]).Trim() != "") && !(s == "")) && int.TryParse(s, out num15))
                            {
                                num12 = Math.Max(num12, (long)(int.Parse(s) + 1));
                            }
                        }
                        for (int m = 0; m <= (this.dv.Count - 1); m++)
                        {
                            strCard = "";
                            s = "";
                            strPassword = "";
                            dept = "";
                            name = wgTools.SetObjToStr(this.dv[m][num2]).Trim();
                            if (num >= 0)
                            {
                                s = wgTools.SetObjToStr(this.dv[m][num]).Trim();
                            }
                            if (num5 >= 0)
                            {
                                strPassword = wgTools.SetObjToStr(this.dv[m][num5]).Trim();
                                password = (strPassword == "") ? 0 : Convert.ToInt32(strPassword);
                            }
                            if (num3 >= 0)
                            {
                                strCard = wgTools.SetObjToStr(this.dv[m][num3]).Trim();
                            }
                            if (num4 >= 0)
                            {
                                dept = wgTools.SetObjToStr(this.dv[m][num4]).Trim();
                            }
                            if (name != "")
                            {
                                if (s == "")
                                {
                                    s = num12.ToString();
                                }
                                else
                                {
                                    int num16;
                                    if (int.TryParse(s, out num16))
                                    {
                                        num12 = Math.Max(num12, (long)int.Parse(s));
                                    }
                                }
                                if (this.addConsumerNew(s, name, strCard, password, dept) > 0)
                                {
                                    num9++;
                                    num12 += 1L;
                                }
                                else
                                {
                                    str4 = str4 + name + ",";
                                    num10++;
                                }
                            }
                            else
                            {
                                if (((s != "") || (name != "")) || (((strCard != "") || (dept != "")) || (strPassword != "")))
                                {
                                    str4 = string.Concat(new object[] { str4, "L", m + 1, "," });
                                    num10++;
                                }
                                if (str4.Length > 500)
                                {
                                    break;
                                }
                            }
                            if (m >= 0xffff)
                            {
                                break;
                            }
                            wgAppRunInfo.raiseAppRunInfoLoadNums(((num10 + num9)).ToString() + " / " + this.dv.Count.ToString());
                            this.dfrmWait1.Text = ((num10 + num9)).ToString() + " / " + this.dv.Count.ToString();
                            Application.DoEvents();
                        }
                        wgAppRunInfo.raiseAppRunInfoLoadNums(((num10 + num9)).ToString() + " / " + this.dv.Count.ToString());
                        this.dfrmWait1.Text = ((num10 + num9)).ToString() + " / " + this.dv.Count.ToString();
                        new icGroup().updateGroupNO();
                        wgTools.WriteLine("Import end");
                        if (!(str4 == ""))
                        {
                            this.dfrmWait1.Hide();
                            wgTools.WgDebugWrite(CommonStr.strNotImportedUsers + num10.ToString() + "\r\n" + str4, new object[0]);
                            XMessageBox.Show(CommonStr.strNotImportedUsers + num10.ToString() + "\r\n\r\n" + str4);
                            wgAppConfig.wgLog(CommonStr.strNotImportedUsers + num10.ToString() + "\r\n" + str4);
                        }
                        wgAppConfig.wgLog(this.btnImportFromExcel.Text + ": " + num9);
                        icConsumerShare.setUpdateLog();
                        XMessageBox.Show(this.btnImportFromExcel.Text + ": " + num9);
                        this.reloadUserData("");
                    }
                }
            }
            catch (Exception exception4)
            {
                wgTools.WgDebugWrite(exception4.ToString(), new object[0]);
                wgAppConfig.wgLog(exception4.ToString());
            }
            finally
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
                Cursor.Current = Cursors.Default;
                this.dfrmWait1.Hide();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvUsers, this.Text);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.deletedUserCnt = 0;
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            string strsql;
            //if (wgAppConfig.IsAccessDB)
            strsql = " SELECT a.f_ConsumerID, a.f_ConsumerNO, a.f_ConsumerName, " +
                "c.f_FpCount as f_HasFp, " +
                "d.f_FaceCount as f_HasFace, " +
                "a.f_CardNO, a.f_PIN, a.f_AttendEnabled, a.f_ShiftEnabled, a.f_DoorEnabled, a.f_BeginYMD, a.f_EndYMD, t_b_Group.f_GroupName " +
                "FROM (((t_b_Consumer a LEFT OUTER JOIN t_b_Group ON a.f_GroupID = t_b_Group.f_GroupID) " +
                "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FpCount, f_ConsumerID FROM t_d_FpTempl GROUP BY f_ConsumerID) c ON a.f_ConsumerID = c.f_ConsumerID) " +
                "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FaceCount, f_ConsumerID FROM t_d_FaceTempl GROUP BY f_ConsumerID) d ON a.f_ConsumerID = d.f_ConsumerID) ";
            if (findConsumerID > 0)
            {
                strsql += "WHERE a.f_ConsumerID = " + findConsumerID.ToString();
            }
            else if (groupMinNO > 0)
            {
                if (groupMinNO >= groupMaxNO)
                    strsql += "WHERE " + string.Format("t_b_Group.f_GroupID = {0:d}", groupIDOfMinNO);
                else
                    strsql += "WHERE " + string.Format("t_b_Group.f_GroupNO >= {0:d} ", groupMinNO) +
                            string.Format(" AND t_b_Group.f_GroupNO <= {0:d} ", groupMaxNO);
                if (findName != "")
                    strsql += string.Format(" AND a.f_ConsumerName LIKE {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                else if (findCard > 0L)
                    strsql += string.Format(" AND a.f_CardNO = {0:d} ", findCard);
            }
            else if (findName != "")
            {
                strsql += string.Format("WHERE a.f_ConsumerName LIKE {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
            }
            else if (findCard > 0L)
            {
                strsql += string.Format("WHERE a.f_CardNO = {0:d} ", findCard);
            }
            this.reloadUserData(strsql);
        }

        private void btnRegisterLostCard_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvUsers.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvUsers.SelectedRows[0].Index;
                }
                int consumerID = int.Parse(this.dgvUsers.Rows[rowIndex].Cells[0].Value.ToString());
                if (consumerID > 0)
                {
                    using (dfrmUsersCardLost lost = new dfrmUsersCardLost())
                    {
                        lost.txtf_ConsumerName.Text = this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
                        lost.txtf_CardNO.Text = this.dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
                        string str = this.dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();//ConsumerID
                        string text = "";
                        if (lost.ShowDialog(this) == DialogResult.OK)
                        {
                            icConsumer consumer = new icConsumer();
                            text = lost.txtf_CardNONew.Text;
                            if (string.IsNullOrEmpty(lost.txtf_CardNONew.Text))
                            {
                                consumer.registerLostCard(consumerID, 0L);
                            }
                            else
                            {
                                consumer.registerLostCard(consumerID, long.Parse(lost.txtf_CardNONew.Text.Trim()));
                            }
                            icConsumerShare.setUpdateLog();
                            wgAppConfig.wgLog(string.Format("{0}:{1} [{2} => {3}]", new object[] { sender.ToString(), this.dgvUsers.Rows[rowIndex].Cells[2].Value.ToString(), this.dgvUsers.Rows[rowIndex].Cells[3].Value.ToString(), lost.txtf_CardNONew.Text }), EventLogEntryType.Information, null);
                            DataGridViewRow row = this.dgvUsers.Rows[rowIndex];
                            string cmdText = " SELECT    f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_PIN, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName ";
                            cmdText = (cmdText + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ") + " WHERE f_ConsumerID= " + consumerID.ToString();
                            if (wgAppConfig.IsAccessDB)
                            {
                                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                                {
                                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                                    {
                                        connection.Open();
                                        OleDbDataReader reader = command.ExecuteReader();
                                        bool flag = false;
                                        if (reader.Read())
                                        {
                                            for (int i = 1; i < this.dgvUsers.Columns.Count; i++)
                                            {
                                                row.Cells[i].Value = reader[i];
                                            }
                                            if (int.Parse(wgTools.SetObjToStr(reader["f_DoorEnabled"])) > 0)
                                            {
                                                flag = true;
                                            }
                                        }
                                        reader.Close();
                                        if (flag)
                                        {
                                            cmdText = " SELECT a.* ";
                                            cmdText = cmdText + " FROM t_b_Controller a, t_d_Privilege b";
                                            cmdText = cmdText + " WHERE b.f_ConsumerID= " + consumerID.ToString();
                                            cmdText = cmdText + " AND  a.f_ControllerID = b.f_ControllerID  ";
                                            using (icPrivilege privilege = new icPrivilege())
                                            {
                                                try
                                                {
                                                    using (OleDbCommand command2 = new OleDbCommand(cmdText, connection))
                                                    {
                                                        reader = command2.ExecuteReader();
                                                        ArrayList list = new ArrayList();
                                                        while (reader.Read())
                                                        {
                                                            if (list.IndexOf((int)reader["f_ControllerID"]) >= 0)
                                                            {
                                                                continue;
                                                            }
                                                            list.Add((int)reader["f_ControllerID"]);
                                                            if (wgMjController.IsElevator((int)reader["f_ControllerSN"]))
                                                            {
                                                                continue;
                                                            }
                                                            if (!string.IsNullOrEmpty(str) && (privilege.DelPrivilegeOfOneCardIP((int)reader["f_ControllerSN"], wgTools.SetObjToStr(reader["f_IP"]), (int)reader["f_PORT"], uint.Parse(str)) < 0))
                                                            {
                                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                break;
                                                            }
                                                            if (string.IsNullOrEmpty(text))
                                                            {
                                                                continue;
                                                            }
                                                            using (icController controller = new icController())
                                                            {
                                                                controller.GetInfoFromDBByControllerID((int)reader["f_ControllerID"]);
                                                                if (controller.GetControllerRunInformationIP() > 0)
                                                                {
                                                                    if ((controller.runinfo.regUserCount != 0) || (privilege.ClearAllPrivilegeIP(controller.ControllerSN, controller.IP, controller.PORT) >= 0))
                                                                    {
                                                                        goto Label_04B4;
                                                                    }
                                                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                }
                                                                else
                                                                {
                                                                    XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                }
                                                                return;
                                                            }
                                                        Label_04B4:
                                                            if (privilege.AddPrivilegeOfOneCardByDB((int)reader["f_ControllerID"], consumerID) < 0)
                                                            {
                                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                break;
                                                            }
                                                        }
                                                        reader.Close();
                                                    }
                                                }
                                                catch (Exception exception)
                                                {
                                                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                                                }
                                            }
                                        }
                                    }
                                    return;
                                }
                            }
                            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                            {
                                using (SqlCommand command3 = new SqlCommand(cmdText, connection2))
                                {
                                    connection2.Open();
                                    SqlDataReader reader2 = command3.ExecuteReader();
                                    bool flag2 = false;
                                    if (reader2.Read())
                                    {
                                        for (int j = 1; j < this.dgvUsers.Columns.Count; j++)
                                        {
                                            row.Cells[j].Value = reader2[j];
                                        }
                                        if (int.Parse(wgTools.SetObjToStr(reader2["f_DoorEnabled"])) > 0)
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    reader2.Close();
                                    if (flag2)
                                    {
                                        cmdText = " SELECT a.* ";
                                        cmdText = ((cmdText + " FROM t_b_Controller a, t_d_Privilege b") + " WHERE b.f_ConsumerID= " + consumerID.ToString()) + " AND  a.f_ControllerID = b.f_ControllerID  ";
                                        using (icPrivilege privilege2 = new icPrivilege())
                                        {
                                            try
                                            {
                                                using (SqlCommand command4 = new SqlCommand(cmdText, connection2))
                                                {
                                                    reader2 = command4.ExecuteReader();
                                                    ArrayList list2 = new ArrayList();
                                                    while (reader2.Read())
                                                    {
                                                        if (list2.IndexOf((int)reader2["f_ControllerID"]) >= 0)
                                                        {
                                                            continue;
                                                        }
                                                        list2.Add((int)reader2["f_ControllerID"]);
                                                        if (wgMjController.IsElevator((int)reader2["f_ControllerSN"]))
                                                        {
                                                            continue;
                                                        }
                                                        if (!string.IsNullOrEmpty(str) && (privilege2.DelPrivilegeOfOneCardIP((int)reader2["f_ControllerSN"], wgTools.SetObjToStr(reader2["f_IP"]), (int)reader2["f_PORT"], uint.Parse(str)) < 0))
                                                        {
                                                            XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            break;
                                                        }
                                                        if (string.IsNullOrEmpty(text))
                                                        {
                                                            continue;
                                                        }
                                                        using (icController controller2 = new icController())
                                                        {
                                                            controller2.GetInfoFromDBByControllerID((int)reader2["f_ControllerID"]);
                                                            if (controller2.GetControllerRunInformationIP() > 0)
                                                            {
                                                                if ((controller2.runinfo.regUserCount != 0) || (privilege2.ClearAllPrivilegeIP(controller2.ControllerSN, controller2.IP, controller2.PORT) >= 0))
                                                                {
                                                                    goto Label_07A5;
                                                                }
                                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            }
                                                            else
                                                            {
                                                                XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            }
                                                            return;
                                                        }
                                                    Label_07A5:
                                                        if (privilege2.AddPrivilegeOfOneCardByDB((int)reader2["f_ControllerID"], consumerID) < 0)
                                                        {
                                                            XMessageBox.Show(CommonStr.strDelAddAndUploadFail, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                            break;
                                                        }
                                                    }
                                                    reader2.Close();
                                                }
                                            }
                                            catch (Exception exception2)
                                            {
                                                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                wgAppConfig.wgLog(exception3.ToString());
            }
        }

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void dgvUsers_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.bLoadedFinished && (e.ScrollOrientation == ScrollOrientation.VerticalScroll))
            {
                wgTools.WriteLine(e.OldValue.ToString());
                wgTools.WriteLine(e.NewValue.ToString());
                DataGridView dgvUsers = this.dgvUsers;
                if ((e.NewValue > e.OldValue) && ((((e.NewValue + 100) + this.deletedUserCnt) > (dgvUsers.Rows.Count + this.deletedUserCnt)) || (((e.NewValue + this.deletedUserCnt) + ((dgvUsers.Rows.Count + this.deletedUserCnt) / 10)) > (dgvUsers.Rows.Count + this.deletedUserCnt))))
                {
                    if (this.startRecordIndex <= (dgvUsers.Rows.Count + this.deletedUserCnt))
                    {
                        if (!this.backgroundWorker1.IsBusy)
                        {
                            this.startRecordIndex += this.MaxRecord;
                            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                        }
                    }
                    else
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
                    }
                }
            }
        }

        private void displayAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.bLoadedFinished)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.startRecordIndex <= this.dgvUsers.Rows.Count)
                {
                    if (!this.backgroundWorker1.IsBusy)
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.bLoadedFinished = true;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, 0x5f5e100, this.dgvSql });
                    }
                }
                else
                {
                    wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString() + "#");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.dv != null)
                {
                    this.dv.Dispose();
                }
                if (this.dv4loadUserData != null)
                {
                    this.dv4loadUserData.Dispose();
                }
                if (this.tb4loadUserData != null)
                {
                    this.tb4loadUserData.Dispose();
                }
                if (this.userControlFind1 != null)
                {
                    this.userControlFind1.Dispose();
                }
                if (disposing && (this.dfrmWait1 != null))
                {
                    this.dfrmWait1.Dispose();
                }
            }
            if (disposing && (this.watching != null))
            {
                this.watching.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void fillDgv(DataView dv)
        {
            try
            {
                DataGridView dgvUsers = this.dgvUsers;
                if (dgvUsers.DataSource == null)
                {
                    dgvUsers.DataSource = dv;
                    for (int i = 0; i < dv.Table.Columns.Count; i++)
                    {
                        dgvUsers.Columns[i].DataPropertyName = dv.Table.Columns[i].ColumnName;
                        dgvUsers.Columns[i].Name = dv.Table.Columns[i].ColumnName;
                    }
                    wgAppConfig.setDisplayFormatDate(dgvUsers, "f_BeginYMD", wgTools.DisplayFormat_DateYMD);
                    wgAppConfig.setDisplayFormatDate(dgvUsers, "f_EndYMD", wgTools.DisplayFormat_DateYMD);
                    wgAppConfig.ReadGVStyle(this, dgvUsers);
                    if ((this.startRecordIndex == 0) && (dv.Count >= this.MaxRecord))
                    {
                        this.startRecordIndex += this.MaxRecord;
                        this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                    }
                }
                else if (dv.Count > 0)
                {
                    int firstDisplayedScrollingRowIndex = dgvUsers.FirstDisplayedScrollingRowIndex;
                    DataView dataSource = dgvUsers.DataSource as DataView;
                    dataSource.Table.Merge(dv.Table);
                    if (firstDisplayedScrollingRowIndex >= 0)
                    {
                        dgvUsers.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void frmUsers_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.stopWatch();
        }

        private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.watching != null)
            {
                this.watching.StopWatch();
            }
        }

        private void frmUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.funcCtrlShiftQ();
                }
            }
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            bool flag;
            bool flag2;
            this.Deptname.HeaderText = wgAppConfig.ReplaceFloorRomm(this.Deptname.HeaderText);
            this.ConsumerNO.HeaderText = wgAppConfig.ReplaceWorkNO(this.ConsumerNO.HeaderText);
            this.loadOperatorPrivilege();
            this.userControlFind1.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.loadStyle();
            Cursor.Current = Cursors.WaitCursor;
            string str = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, f_PIN, f_AttendEnabled, f_ShiftEnabled, f_DoorEnabled, f_BeginYMD, f_EndYMD, f_GroupName ";
            str = str + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ";
            this.userControlFind1.btnQuery.PerformClick();
            icOperator.getFrmOperatorPrivilege("mnuCardLost", out flag, out flag2);
            this.btnRegisterLostCard.Visible = flag2;
            bool bReadOnly = false;
            string funName = "mnuCardLost";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly))
            {
                this.btnRegisterLostCard.Visible = false; //!bReadOnly;
            }
            else
            {
                this.btnRegisterLostCard.Visible = false;
            }
            this.btnEditPrivilege.Visible = false;
            if (!wgAppConfig.getParamValBoolByNO(0x6f))
            {
                bReadOnly = false;
                funName = "mnu1DoorControl";
                if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && !bReadOnly)
                {
                    funName = "mnuPrivilege";
                    if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && !bReadOnly)
                    {
                        this.btnEditPrivilege.Visible = true;
                    }
                }
            }
            icControllerZone zone = new icControllerZone();
            ArrayList arrZoneName = new ArrayList();
            ArrayList arrZoneID = new ArrayList();
            ArrayList arrZoneNO = new ArrayList();
            zone.getZone(ref arrZoneName, ref arrZoneID, ref arrZoneNO);
            if ((arrZoneID.Count > 0) && (((int)arrZoneID[0]) != 0))
            {
                this.btnEditPrivilege.Enabled = false;
            }
            this.dgvUsers.ContextMenuStrip = this.contextMenuStrip1;
        }

        private void funcCtrlShiftQ()
        {
            this.btnImportFromExcel.Visible = true;
        }

        private int getDeptId(string deptName)
        {
            int num2 = -1;
            icGroup group = new icGroup();
            num2 = group.getGroupID(deptName);
            if (num2 > 0)
            {
                return num2;
            }
            string[] strArray = deptName.Split(new char[] { '\\' });
            string groupNewName = "";
            bool flag = false;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (groupNewName == "")
                {
                    groupNewName = strArray[i];
                }
                else
                {
                    groupNewName = groupNewName + @"\" + strArray[i];
                }
                if (flag || !group.checkExisted(groupNewName))
                {
                    flag = true;
                    group.addNew4BatchExcel(groupNewName);
                }
            }
            return group.getGroupID(deptName);
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuConsumers";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly))
            {
                if (bReadOnly)
                {
                    this.btnAutoAdd.Visible = false;
                    this.btnAdd.Visible = false;
                    this.btnEdit.Visible = false;
                    this.btnDelete.Visible = false;
                    this.btnImportFromExcel.Visible = false;
                    this.btnBatchUpdate.Visible = false;
                }
            }
            else
            {
                base.Close();
            }
        }

        private void loadStyle()
        {
            this.dgvUsers.AutoGenerateColumns = false;
            bool flag = wgAppConfig.getParamValBoolByNO(0x71);
            this.dgvUsers.Columns["Shift"].Visible = flag;
            wgAppConfig.ReadGVStyle(this, this.dgvUsers);
        }

        private DataView loadUserData(int startIndex, int maxRecords, string strSql)
        {
            wgTools.WriteLine("loadUserData Start");
            if (strSql.ToUpper().IndexOf("SELECT ") > 0)
                strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            if (startIndex == 0)
                this.recNOMax = 0;
            else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
            {
                if (strSql.IndexOf("a.f_ConsumerID") == -1)
                    strSql += string.Format(" AND f_ConsumerID > {0}", this.recNOMax.ToString());
                else
                    strSql += string.Format(" AND a.f_ConsumerID > {0}", this.recNOMax.ToString());
            }
            else
            {
                if (strSql.IndexOf("a.f_ConsumerID") == -1)
                    strSql += string.Format(" WHERE f_ConsumerID > {0}", this.recNOMax.ToString());
                else
                    strSql += string.Format(" WHERE a.f_ConsumerID > {0}", this.recNOMax.ToString());
            }
            if (strSql.IndexOf("a.f_ConsumerID") == -1)
                strSql += " ORDER BY f_ConsumerID ";
            else
                strSql += " ORDER BY a.f_ConsumerID ";
            this.tb4loadUserData = new DataTable("users");
            this.dv4loadUserData = new DataView(this.tb4loadUserData);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.tb4loadUserData);
                        }
                    }
                    goto Label_0187;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(strSql, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.tb4loadUserData);
                    }
                }
            }
        Label_0187:
            if (this.tb4loadUserData.Rows.Count > 0)
            {
                this.recNOMax = (int)this.tb4loadUserData.Rows[this.tb4loadUserData.Rows.Count - 1]["f_ConsumerID"];
            }
            wgTools.WriteLine("loadUserData End");
            return this.dv4loadUserData;
        }

        private void reloadUserData(string strsql)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.bLoadedFinished = false;
                this.startRecordIndex = 0;
                this.MaxRecord = 0x3e8;
                if (!string.IsNullOrEmpty(strsql))
                {
                    this.dgvSql = strsql;
                }
                this.dgvUsers.DataSource = null;
                this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
            }
        }

        public void startWatch()
        {
            if (this.watching == null)
            {
                this.watching = new WatchingService();
            }
        }

        public void stopWatch()
        {
            if (this.watching != null)
            {
                this.watching.StopWatch();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string strNewName = "";
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = sender.ToString();
                name.label1.Text = CommonStr.strCardID;
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    strNewName = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                this.deletedUserCnt = 0;
                int groupMinNO = 0;
                int groupIDOfMinNO = 0;
                int groupMaxNO = 0;
                string findName = "";
                long findCard = 0L;
                int findConsumerID = 0;
                this.userControlFind1.txtFindCardID.Text = "";
                this.userControlFind1.txtFindName.Text = "";
                this.userControlFind1.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
                string strsql = " SELECT a.f_ConsumerID, a.f_ConsumerNO, a.f_ConsumerName, " +
                    "c.f_FpCount as f_HasFp, " +
                    "d.f_FaceCount as f_HasFace, " +
                    "a.f_CardNO, a.f_PIN, a.f_AttendEnabled, a.f_ShiftEnabled, a.f_DoorEnabled, a.f_BeginYMD, a.f_EndYMD, t_b_Group.f_GroupName " +
                    "FROM (((t_b_Consumer a LEFT OUTER JOIN t_b_Group ON a.f_GroupID = t_b_Group.f_GroupID) " +
                    "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FpCount, f_ConsumerID FROM t_d_FpTempl GROUP BY f_ConsumerID) c ON a.f_ConsumerID = c.f_ConsumerID) " +
                    "LEFT OUTER JOIN (SELECT COUNT(f_ConsumerID) AS f_FaceCount, f_ConsumerID FROM t_d_FaceTempl GROUP BY f_ConsumerID) d ON a.f_ConsumerID = d.f_ConsumerID) ";
                if (findConsumerID > 0)
                {
                    strsql += "WHERE a.f_ConsumerID = " + findConsumerID.ToString();
                }
                else if (groupMinNO > 0)
                {
                    if (groupMinNO >= groupMaxNO)
                        strsql += "WHERE " + string.Format("t_b_Group.f_GroupID = {0:d} ", groupIDOfMinNO);
                    else
                        strsql += "WHERE " + string.Format("t_b_Group.f_GroupNO >= {0:d} ", groupMinNO) + 
                            string.Format(" AND  t_b_Group.f_GroupNO <= {0:d} ", groupMaxNO);
                    if (findName != "")
                        strsql += string.Format(" AND a.f_ConsumerName like {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                    else if (findCard > 0L)
                        strsql += string.Format(" AND a.f_CardNO = {0:d} ", findCard);
                }
                else if (findName != "")
                {
                    strsql += string.Format("WHERE a.f_ConsumerName LIKE {0} ", wgTools.PrepareStr(string.Format("%{0}%", findName)));
                }
                else if (findCard > 0L)
                {
                    strsql += string.Format(" WHERE a.f_CardNO ={0:d} ", findCard);
                }
                string str4 = " ( 1>0 ) ";
                if (strNewName.IndexOf("%") < 0)
                {
                    strNewName = string.Format("%{0}%", strNewName);
                }
                if (wgAppConfig.IsAccessDB)
                {
                    str4 = str4 + string.Format(" AND CSTR(f_CardNO) like {0} ", wgTools.PrepareStr(strNewName));
                }
                else
                {
                    str4 = str4 + string.Format(" AND f_CardNO like {0} ", wgTools.PrepareStr(strNewName));
                }
                if (strsql.ToUpper().IndexOf("WHERE") > 0)
                {
                    strsql = strsql + string.Format(" AND {0} ", str4);
                }
                else
                {
                    strsql = strsql + string.Format(" WHERE {0} ", str4);
                }
                this.reloadUserData(strsql);
            }
        }
    }
}
