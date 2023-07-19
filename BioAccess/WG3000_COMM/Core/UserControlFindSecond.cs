namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    partial class UserControlFindSecond : UserControl
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        public static bool blogin = false;
        private string dgvSql;
        private static string lastLoadUsers = "";
        private int MaxRecord = 0x4e20;
        private int recNOMax = 0;
        private int SelectedConsumerID;
        private int startRecordIndex;
        private string strGroupFilter = "";

        public UserControlFindSecond()
        {
            this.InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            wgTools.WriteLine("DoWork Starting ...");
            if ((lastLoadUsers == icConsumerShare.getUpdateLog()) && (dvLastLoad != null))
            {
                Thread.Sleep(100);
                dvLastLoad.RowFilter = "";
                e.Result = dvLastLoad;
            }
            else
            {
                lastLoadUsers = icConsumerShare.getUpdateLog();
                int startIndex = (int) ((object[]) e.Argument)[0];
                int maxRecords = (int) ((object[]) e.Argument)[1];
                string strSql = (string) ((object[]) e.Argument)[2];
                BackgroundWorker worker = sender as BackgroundWorker;
                e.Result = this.loadUserData4BackWork(startIndex, maxRecords, strSql);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
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
                this.loadUserData4BackWorkComplete(e.Result as DataView);
                wgTools.WriteLine("backgroundWorker1_RunWorkerCompleted");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtFindCardID.Text = "";
            this.txtFindName.Text = "";
            this.cboUsers.Text = "";
            if (this.cboFindDept.Items.Count > 0)
            {
                this.cboFindDept.SelectedIndex = 0;
            }
            this.cboUsers.Text = "";
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.SelectedConsumerID = 0;
            if (string.IsNullOrEmpty(this.cboUsers.Text))
            {
                this.txtFindName.Text = "";
            }
            else if (this.cboUsers.SelectedIndex < 0)
            {
                this.txtFindName.Text = this.cboUsers.Text;
            }
            else
            {
                this.txtFindName.Text = ((DataRowView) this.cboUsers.SelectedItem).Row["f_ConsumerName"].ToString();
                this.SelectedConsumerID = (int) ((DataRowView) this.cboUsers.SelectedItem).Row["f_ConsumerID"];
            }
        }

        private void cboFindDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cboUsers.DataSource != null)
                {
                    DataView dataSource = (DataView) this.cboUsers.DataSource;
                    if (this.cboFindDept.SelectedIndex < 0)
                    {
                        dataSource.RowFilter = "";
                        this.strGroupFilter = "";
                    }
                    if ((this.cboFindDept.SelectedIndex == 0) && (this.cboFindDept.Text == ""))
                    {
                        dataSource.RowFilter = "";
                        this.strGroupFilter = "";
                    }
                    else
                    {
                        this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cboFindDept.SelectedIndex];
                        int num = 0;
                        int num2 = 0;
                        int num3 = 0;
                        num2 = (int) this.arrGroupID[this.cboFindDept.SelectedIndex];
                        num = (int) this.arrGroupNO[this.cboFindDept.SelectedIndex];
                        num3 = icGroup.getGroupChildMaxNo(this.cboFindDept.Text, this.arrGroupName, this.arrGroupNO);
                        if (num > 0)
                        {
                            if (num >= num3)
                            {
                                this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num2);
                            }
                            else
                            {
                                string str = "";
                                for (int i = 0; i < this.arrGroupNO.Count; i++)
                                {
                                    if ((((int) this.arrGroupNO[i]) <= num3) && (((int) this.arrGroupNO[i]) >= num))
                                    {
                                        if (str == "")
                                        {
                                            str = str + string.Format(" f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                        }
                                        else
                                        {
                                            str = str + string.Format(" OR f_GroupID ={0:d} ", (int) this.arrGroupID[i]);
                                        }
                                    }
                                }
                                this.strGroupFilter = string.Format("  {0} ", str);
                            }
                        }
                        dataSource.RowFilter = string.Format("{0}", this.strGroupFilter);
                    }
                    this.cboUsers.Text = "";
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cboUsers_DropDown(object sender, EventArgs e)
        {
        }

        private void cboUsers_DropDownClosed(object sender, EventArgs e)
        {
        }

        private void cboUsers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.btnQuery.PerformClick();
            }
        }

        public void getSqlInfo(ref int groupMinNO, ref int groupIDOfMinNO, ref int groupMaxNO, ref string findName, ref long findCard, ref int findConsumerID)
        {
            try
            {
                this.btnQuery_Click(null, null);
                findConsumerID = this.SelectedConsumerID;
                if ((this.cboFindDept.SelectedIndex < 0) || ((this.cboFindDept.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    groupMinNO = 0;
                    groupMaxNO = 0;
                }
                else
                {
                    groupIDOfMinNO = (int) this.arrGroupID[this.cboFindDept.SelectedIndex];
                    groupMinNO = (int) this.arrGroupNO[this.cboFindDept.SelectedIndex];
                    groupMaxNO = icGroup.getGroupChildMaxNo(this.cboFindDept.Text, this.arrGroupName, this.arrGroupNO);
                }
                if (string.IsNullOrEmpty(this.txtFindName.Text))
                {
                    findName = "";
                }
                else
                {
                    findName = this.txtFindName.Text.Trim();
                }
                findCard = 0L;
                if (long.TryParse(this.txtFindCardID.Text, out findCard) && (findCard < 0L))
                {
                    findCard = 0L;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        public string getSqlOfGroup(string fieldName)
        {
            string str = "";
            int groupMinNO = 0;
            int groupIDOfMinNO = 0;
            int groupMaxNO = 0;
            string findName = "";
            long findCard = 0L;
            int findConsumerID = 0;
            this.getSqlInfo(ref groupMinNO, ref groupIDOfMinNO, ref groupMaxNO, ref findName, ref findCard, ref findConsumerID);
            if (groupMinNO >= groupMaxNO)
            {
                return (str + string.Format(" {0} ={1:d} ", fieldName, groupIDOfMinNO));
            }
            for (int i = 0; i < this.arrGroupNO.Count; i++)
            {
                if ((((int) this.arrGroupNO[i]) <= groupMaxNO) && (((int) this.arrGroupNO[i]) >= groupMinNO))
                {
                    if (str == "")
                    {
                        str = str + string.Format(" {0} ={1:d} ", fieldName, (int) this.arrGroupID[i]);
                    }
                    else
                    {
                        str = str + string.Format(" OR {0} ={1:d} ", fieldName, (int) this.arrGroupID[i]);
                    }
                }
            }
            return str;
        }

        private DataView loadUserData4BackWork(int startIndex, int maxRecords, string strSql)
        {
            DataView dv;
            wgTools.WriteLine("loadUserData Start");
            if (strSql.ToUpper().IndexOf("SELECT ") > 0)
                strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
            if (startIndex == 0)
                this.recNOMax = 0;
            else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
                strSql = strSql + string.Format(" AND f_ConsumerID > {0}", this.recNOMax.ToString());
            else
                strSql = strSql + string.Format(" WHERE f_ConsumerID > {0}", this.recNOMax.ToString());
            strSql = strSql + " ORDER BY f_ConsumerID ";
            this.tb = new DataTable("users");
            this.dv = new DataView(this.tb);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(strSql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.tb);
                            if (this.tb.Rows.Count > 0)
                            {
                                this.recNOMax = (int)this.tb.Rows[this.tb.Rows.Count - 1]["f_ConsumerID"];
                            }
                            wgTools.WriteLine("loadUserData End");
                            return this.dv;
                        }
                    }
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(strSql, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.tb);
                        if (this.tb.Rows.Count > 0)
                        {
                            this.recNOMax = (int)this.tb.Rows[this.tb.Rows.Count - 1]["f_ConsumerID"];
                        }
                        wgTools.WriteLine("loadUserData End");
                        dv = this.dv;
                    }
                }
            }
            return dv;
        }

        private void loadUserData4BackWorkComplete(DataView dv)
        {
            if (this.cboUsers.DataSource == null)
            {
                this.cboUsers.BeginUpdate();
                this.cboUsers.DisplayMember = "f_ConsumerFull";
                this.cboUsers.ValueMember = "f_ConsumerID";
                this.cboUsers.DataSource = dv;
                dvLastLoad = dv;
                this.cboUsers.EndUpdate();
                this.cboUsers.Text = "";
                this.cboFindDept_SelectedIndexChanged(null, null);
            }
            else if (dv.Count > 0)
            {
                DataView dataSource = this.cboUsers.DataSource as DataView;
                dataSource.Table.Merge(dv.Table);
                if (dv.Count >= this.MaxRecord)
                {
                    this.startRecordIndex += this.MaxRecord;
                    this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            string str = "";
            this.startRecordIndex = 0;
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID, '(' + LTRIM(f_ConsumerNO) + ')-' +  f_ConsumerName + '-' + IIF(ISNULL( f_CardNO),'-',CSTR(f_CardNO))  As f_ConsumerFull ";
                str = str + " FROM t_b_Consumer ";
            }
            else
            {
                str = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID, '(' + LTRIM(f_ConsumerNO) + ')-' +  f_ConsumerName + '-' + CASE WHEN f_CardNO IS NULL THEN '-' ELSE CONVERT(nvarchar(50),f_CardNO) END  As f_ConsumerFull ";
                str = str + " FROM t_b_Consumer ";
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.dgvSql = str;
            }
            this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
        }

        private void txtFindCardID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.btnQuery.PerformClick();
            }
            else
            {
                if (this.txtFindCardID.Text.Length == 0)
                {
                    if (e.KeyChar == '\x0016')
                    {
                        return;
                    }
                }
                else
                {
                    if (e.KeyChar == '\x0003')
                    {
                        return;
                    }
                    if (e.KeyChar == '\x0018')
                    {
                        return;
                    }
                }
                if (e.KeyChar != '\b')
                {
                    int num;
                    if (int.TryParse(e.KeyChar.ToString(), out num))
                    {
                        if (this.txtFindCardID.Text.Length > 10)
                        {
                            e.Handled = true;
                        }
                        else if ((this.txtFindCardID.Text.Length == 10) && (this.txtFindCardID.SelectionLength == 0))
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void UserControlFind_Load(object sender, EventArgs e)
        {
            if (blogin)
            {
                try
                {
                    this.cboFindDept.Items.Clear();
                    new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
                    int count = this.arrGroupID.Count;
                    for (count = 0; count < this.arrGroupID.Count; count++)
                    {
                        this.cboFindDept.Items.Add(this.arrGroupName[count].ToString());
                    }
                    if (this.cboFindDept.Items.Count > 0)
                    {
                        this.cboFindDept.SelectedIndex = 0;
                    }
                    this.toolStripLabel3.Text = wgAppConfig.ReplaceFloorRomm(this.toolStripLabel3.Text);
                    this.timer1.Enabled = true;
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
            }
        }
    }
}

