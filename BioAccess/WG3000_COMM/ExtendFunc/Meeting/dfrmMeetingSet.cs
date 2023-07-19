namespace WG3000_COMM.ExtendFunc.Meeting
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmMeetingSet : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private bool bNeedUpdateMeetingConsumer;
        public string curMeetingNo = "";
        private dfrmFind dfrmFind1 = new dfrmFind();
        private DataSet ds = new DataSet();
        private DataTable dt;
        private static DataTable dtLastLoad;
        private DataTable dtUser1;
        private DataView dv;
        private DataView dv1;
        private DataView dv2;
        private DataView dvGroupedConsumers;
        private DataView dvSelected;
        private static string lastLoadUsers = "";
        private ResourceManager resStr;
        private string strGroupFilter = "";

        public dfrmMeetingSet()
        {
            this.InitializeComponent();
            this.resStr = new ResourceManager("WgiCCard." + base.Name + "Str", Assembly.GetExecutingAssembly());
            this.resStr.IgnoreCase = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            e.Result = this.loadUserData4BackWork();
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
                this.loadUserData4BackWorkComplete(e.Result as DataTable);
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.dgvUsers.Rows.Count.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.cboIdentity.SelectedIndex;
            selectObject(this.dgvUsers, "f_MeetingIdentity", selectedIndex, "f_Seat", this.txtSeat.Text.ToString(), "f_MeetingIdentityStr", this.cboIdentity.Items[selectedIndex].ToString());
            this.bNeedUpdateMeetingConsumer = true;
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            wgTools.WriteLine("btnAddAllUsers_Click Start");
            DataTable table = ((DataView) this.dgvUsers.DataSource).Table;
            DataView dataSource = (DataView) this.dgvUsers.DataSource;
            DataView view2 = (DataView) this.dgvSelectedUsers.DataSource;
            this.dgvUsers.DataSource = null;
            this.dgvSelectedUsers.DataSource = null;
            int selectedIndex = this.cboIdentity.SelectedIndex;
            if (this.strGroupFilter == "")
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (((int) table.Rows[i]["f_Selected"]) != 1)
                    {
                        table.Rows[i]["f_Selected"] = 1;
                        table.Rows[i]["f_Seat"] = this.txtSeat.Text;
                        table.Rows[i]["f_MeetingIdentity"] = selectedIndex;
                        table.Rows[i]["f_MeetingIdentityStr"] = this.cboIdentity.Items[selectedIndex];
                    }
                }
            }
            else
            {
                this.dv = new DataView(table);
                this.dv.RowFilter = this.strGroupFilter;
                for (int j = 0; j < this.dv.Count; j++)
                {
                    if (((int) this.dv[j]["f_Selected"]) != 1)
                    {
                        this.dv[j]["f_Selected"] = 1;
                        this.dv[j]["f_Seat"] = this.txtSeat.Text;
                        this.dv[j]["f_MeetingIdentity"] = selectedIndex;
                        this.dv[j]["f_MeetingIdentityStr"] = this.cboIdentity.Items[selectedIndex];
                    }
                }
            }
            this.dgvUsers.DataSource = dataSource;
            this.dgvSelectedUsers.DataSource = view2;
            this.bNeedUpdateMeetingConsumer = true;
            Cursor.Current = Cursors.Default;
        }

        private void btnAddMeetingAdr_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.cbof_MeetingAdr.Text;
                using (dfrmMeetingAdr adr = new dfrmMeetingAdr())
                {
                    adr.ShowDialog();
                }
                this.loadMeetingAdr();
                if (string.IsNullOrEmpty(text))
                {
                    this.cbof_MeetingAdr.Text = text;
                }
                else
                {
                    foreach (object obj2 in this.cbof_MeetingAdr.Items)
                    {
                        if (obj2.ToString() == text)
                        {
                            this.cbof_MeetingAdr.Text = text;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnCreateInfo_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string str = "";
                string str4 = str;
                string str5 = str4 + this.lblMeetingName.Text + "\t" + this.txtf_MeetingName.Text + "\r\n";
                string str6 = str5 + this.lblMeetingAddr.Text + "\t" + this.cbof_MeetingAdr.Text + "\r\n";
                string str7 = str6 + this.lblMeetingDateTime.Text + "\t" + this.dtpMeetingDate.Text + " " + this.dtpMeetingTime.Text + "\r\n";
                string str8 = str7 + this.lblSignBegin.Text + "\t" + this.dtpStartTime.Text + "\r\n";
                str = str8 + this.lblSignEnd.Text + "\t" + this.dtpEndTime.Text + "\r\n";
                if (this.txtf_Content.Text.Length > 0)
                {
                    str = (str + this.lblContent.Text + "\r\n") + this.txtf_Content.Text + "\r\n";
                }
                if (this.txtf_Notes.Text.Length > 0)
                {
                    str = (str + this.lblNotes.Text + "\r\n") + this.txtf_Notes.Text + "\r\n";
                }
                str = (str + "\r\n") + "\r\n" + Strings.Format(DateTime.Now, "yyyy-MM-dd");
                string str2 = this.txtf_MeetingNo.Text + "-" + Strings.Format(DateTime.Now, "yyyy-MM-dd_HHmmss_ff") + ".txt";
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.FileName = str2;
                    dialog.Filter = " (*.txt)|*.txt";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = dialog.FileName;
                        using (StreamWriter writer = new StreamWriter(fileName, true))
                        {
                            writer.WriteLine(str);
                        }
                        str2 = fileName;
                    }
                }
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = str2;
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            this.Cursor = Cursors.Default;
        }

        private void btnDelAllUsers_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.dt = ((DataView) this.dgvUsers.DataSource).Table;
            this.dv1 = (DataView) this.dgvUsers.DataSource;
            this.dv2 = (DataView) this.dgvSelectedUsers.DataSource;
            this.dgvUsers.DataSource = null;
            this.dgvSelectedUsers.DataSource = null;
            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i]["f_Selected"] = 0;
            }
            this.dgvUsers.DataSource = this.dv1;
            this.dgvSelectedUsers.DataSource = this.dv2;
            this.bNeedUpdateMeetingConsumer = true;
            Cursor.Current = Cursors.Default;
        }

        private void btnDelOneUser_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelectedUsers);
            this.bNeedUpdateMeetingConsumer = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.btnOk_Click_Acc(sender, e);
            }
            else
            {
                Cursor current = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    SqlCommand command;
                    string str;
                    if (this.txtf_MeetingName.Text.Trim() == "")
                    {
                        XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
                        return;
                    }
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    if (this.curMeetingNo == "")
                    {
                        command = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text), connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.curMeetingNo = this.txtf_MeetingNo.Text;
                        }
                        reader.Close();
                    }
                    if (this.curMeetingNo == "")
                    {
                        str = "INSERT INTO t_d_Meeting ([f_MeetingNO], [f_MeetingName], [f_MeetingAdr], [f_MeetingDateTime], [f_SignStartTime], [f_SignEndTime], [f_Content], [f_Notes]) ";
                        command = new SqlCommand(((((((((str + "VALUES(" + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + " , " + wgTools.PrepareStr(this.txtf_MeetingName.Text)) + " , " + wgTools.PrepareStr(this.cbof_MeetingAdr.Text)) + " , " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , " + wgTools.PrepareStr(this.txtf_Content.Text)) + " , " + wgTools.PrepareStr(this.txtf_Notes.Text)) + ")", connection);
                        if (command.ExecuteNonQuery() <= 0)
                        {
                        }
                    }
                    else
                    {
                        str = "   Update [t_d_Meeting] ";
                        command = new SqlCommand((((((((str + " SET [f_MeetingName]=" + wgTools.PrepareStr(this.txtf_MeetingName.Text)) + " , [f_MeetingAdr]=" + wgTools.PrepareStr(this.cbof_MeetingAdr.Text)) + " , [f_MeetingDateTime]=" + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , [f_SignStartTime]=" + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , [f_SignEndTime]=" + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , [f_Content]=" + wgTools.PrepareStr(this.txtf_Content.Text)) + " , [f_Notes]=" + wgTools.PrepareStr(this.txtf_Notes.Text)) + " WHERE  f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text), connection);
                        command.ExecuteNonQuery();
                    }
                    if (this.bNeedUpdateMeetingConsumer)
                    {
                        string str2 = "";
                        this.dvGroupedConsumers = (DataView) this.dgvSelectedUsers.DataSource;
                        if (this.dvGroupedConsumers.Count > 0)
                        {
                            int num2;
                            for (num2 = 0; num2 <= (this.dvGroupedConsumers.Count - 1); num2++)
                            {
                                str2 = str2 + this.dvGroupedConsumers[num2]["f_ConsumerID"] + ",";
                            }
                            str2 = str2 + 0;
                            str = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text);
                            if (str2 != "")
                            {
                                str = str + " AND f_ConsumerID NOT IN (" + str2 + ")";
                            }
                            command = new SqlCommand(str, connection);
                            command.ExecuteNonQuery();
                            command = new SqlCommand(str, connection);
                            str = " SELECT COUNT(*) FROM t_d_MeetingConsumer ";
                            str = str + " WHERE  f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text);
                            command.CommandText = str;
                            int num3 = (int) command.ExecuteScalar();
                            int num = 0;
                            for (num2 = 0; num2 <= (this.dvGroupedConsumers.Count - 1); num2++)
                            {
                                if (num3 > 0)
                                {
                                    str = " Update t_d_MeetingConsumer ";
                                    str = ((((str + " SET f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + ", f_MeetingIdentity = " + this.dvGroupedConsumers[num2]["f_MeetingIdentity"]) + ", f_Seat = " + wgTools.PrepareStr(this.dvGroupedConsumers[num2]["f_Seat"])) + " WHERE  f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + " AND f_ConsumerID = " + this.dvGroupedConsumers[num2]["f_ConsumerID"];
                                    command.CommandText = str;
                                    num = command.ExecuteNonQuery();
                                }
                                if (num <= 0)
                                {
                                    str = " INSERT INTO t_d_MeetingConsumer ( [f_MeetingNO], [f_ConsumerID], [f_MeetingIdentity], [f_Seat])";
                                    str = ((((str + " VALUES( " + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + ",  " + this.dvGroupedConsumers[num2]["f_ConsumerID"]) + ", " + this.dvGroupedConsumers[num2]["f_MeetingIdentity"]) + ", " + wgTools.PrepareStr(this.dvGroupedConsumers[num2]["f_Seat"])) + " ) ";
                                    command.CommandText = str;
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            str = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text);
                            if (str2 != "")
                            {
                                str = str + " AND f_ConsumerID NOT IN (" + str2 + ")";
                            }
                            new SqlCommand(str, connection).ExecuteNonQuery();
                        }
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                Cursor.Current = current;
            }
        }

        private void btnOk_Click_Acc(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                OleDbCommand command;
                string str;
                if (this.txtf_MeetingName.Text.Trim() == "")
                {
                    XMessageBox.Show(CommonStr.strMeetingNameIsEmpty);
                    return;
                }
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (this.curMeetingNo == "")
                {
                    command = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text), connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        this.curMeetingNo = this.txtf_MeetingNo.Text;
                    }
                    reader.Close();
                }
                if (this.curMeetingNo == "")
                {
                    str = "INSERT INTO t_d_Meeting ([f_MeetingNO], [f_MeetingName], [f_MeetingAdr], [f_MeetingDateTime], [f_SignStartTime], [f_SignEndTime], [f_Content], [f_Notes]) ";
                    command = new OleDbCommand(((((((((str + "VALUES(" + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + " , " + wgTools.PrepareStr(this.txtf_MeetingName.Text)) + " , " + wgTools.PrepareStr(this.cbof_MeetingAdr.Text)) + " , " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , " + wgTools.PrepareStr(this.txtf_Content.Text)) + " , " + wgTools.PrepareStr(this.txtf_Notes.Text)) + ")", connection);
                    if (command.ExecuteNonQuery() <= 0)
                    {
                    }
                }
                else
                {
                    str = "   Update [t_d_Meeting] ";
                    command = new OleDbCommand((((((((str + " SET [f_MeetingName]=" + wgTools.PrepareStr(this.txtf_MeetingName.Text)) + " , [f_MeetingAdr]=" + wgTools.PrepareStr(this.cbof_MeetingAdr.Text)) + " , [f_MeetingDateTime]=" + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , [f_SignStartTime]=" + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpStartTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , [f_SignEndTime]=" + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpEndTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , [f_Content]=" + wgTools.PrepareStr(this.txtf_Content.Text)) + " , [f_Notes]=" + wgTools.PrepareStr(this.txtf_Notes.Text)) + " WHERE  f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text), connection);
                    command.ExecuteNonQuery();
                }
                if (this.bNeedUpdateMeetingConsumer)
                {
                    string str2 = "";
                    this.dvGroupedConsumers = (DataView) this.dgvSelectedUsers.DataSource;
                    if (this.dvGroupedConsumers.Count > 0)
                    {
                        int num2;
                        for (num2 = 0; num2 <= (this.dvGroupedConsumers.Count - 1); num2++)
                        {
                            str2 = str2 + this.dvGroupedConsumers[num2]["f_ConsumerID"] + ",";
                        }
                        str2 = str2 + 0;
                        str = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text);
                        if (str2 != "")
                        {
                            str = str + " AND f_ConsumerID NOT IN (" + str2 + ")";
                        }
                        command = new OleDbCommand(str, connection);
                        command.ExecuteNonQuery();
                        command = new OleDbCommand(str, connection);
                        str = " SELECT COUNT(*) FROM t_d_MeetingConsumer ";
                        str = str + " WHERE  f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text);
                        command.CommandText = str;
                        int num3 = (int) command.ExecuteScalar();
                        int num = 0;
                        for (num2 = 0; num2 <= (this.dvGroupedConsumers.Count - 1); num2++)
                        {
                            if (num3 > 0)
                            {
                                str = " Update t_d_MeetingConsumer ";
                                str = ((((str + " SET f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + ", f_MeetingIdentity = " + this.dvGroupedConsumers[num2]["f_MeetingIdentity"]) + ", f_Seat = " + wgTools.PrepareStr(this.dvGroupedConsumers[num2]["f_Seat"])) + " WHERE  f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + " AND f_ConsumerID = " + this.dvGroupedConsumers[num2]["f_ConsumerID"];
                                command.CommandText = str;
                                num = command.ExecuteNonQuery();
                            }
                            if (num <= 0)
                            {
                                str = " INSERT INTO t_d_MeetingConsumer ( [f_MeetingNO], [f_ConsumerID], [f_MeetingIdentity], [f_Seat])";
                                str = ((((str + " VALUES( " + wgTools.PrepareStr(this.txtf_MeetingNo.Text)) + ",  " + this.dvGroupedConsumers[num2]["f_ConsumerID"]) + ", " + this.dvGroupedConsumers[num2]["f_MeetingIdentity"]) + ", " + wgTools.PrepareStr(this.dvGroupedConsumers[num2]["f_Seat"])) + " ) ";
                                command.CommandText = str;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        str = " Delete  FROM t_d_MeetingConsumer  WHERE f_MeetingNO= " + wgTools.PrepareStr(this.txtf_MeetingNo.Text);
                        if (str2 != "")
                        {
                            str = str + " AND f_ConsumerID NOT IN (" + str2 + ")";
                        }
                        new OleDbCommand(str, connection).ExecuteNonQuery();
                    }
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            Cursor.Current = current;
        }

        private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource != null)
            {
                DataView dataSource = (DataView) this.dgvUsers.DataSource;
                if ((this.cbof_GroupID.SelectedIndex < 0) || ((this.cbof_GroupID.SelectedIndex == 0) && (((int) this.arrGroupID[0]) == 0)))
                {
                    dataSource.RowFilter = "f_Selected = 0";
                    this.strGroupFilter = "";
                }
                else
                {
                    dataSource.RowFilter = "f_Selected = 0 AND f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrGroupID[this.cbof_GroupID.SelectedIndex];
                    num = (int) this.arrGroupNO[this.cbof_GroupID.SelectedIndex];
                    num3 = icGroup.getGroupChildMaxNo(this.cbof_GroupID.Text, this.arrGroupName, this.arrGroupNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND f_GroupID ={0:d} ", num2);
                            this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num2);
                        }
                        else
                        {
                            dataSource.RowFilter = "f_Selected = 0 ";
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
                            dataSource.RowFilter = string.Format("f_Selected = 0 AND ( {0} )", str);
                            this.strGroupFilter = string.Format("  {0} ", str);
                        }
                    }
                }
            }
        }

        private void dfrmMeetingSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmMeetingSet_KeyDown(object sender, KeyEventArgs e)
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
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmMeetingSet_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmMeetingSet_Load_Acc(sender, e);
            }
            else
            {
                Cursor current = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                base.KeyPreview = true;
                try
                {
                    this.cboIdentity.SelectedIndex = 0;
                    this.loadGroupData();
                    this.loadMeetingAdr();
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    if (this.curMeetingNo == "")
                    {
                        this.txtf_MeetingNo.Text = Strings.Format(DateTime.Now, "yyyyMMdd_HHmmss");
                        this.dtpMeetingDate.Value = DateTime.Now.Date;
                        this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 9:00:00"));
                        this.dtpStartTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 8:00:00"));
                        this.dtpEndTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 17:30:00"));
                    }
                    else
                    {
                        this.txtf_MeetingNo.Text = this.curMeetingNo;
                        SqlDataReader reader = new SqlCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text), connection).ExecuteReader();
                        if (reader.Read())
                        {
                            this.txtf_MeetingName.Text = wgTools.SetObjToStr(reader["f_MeetingName"]);
                            this.cbof_MeetingAdr.Text = wgTools.SetObjToStr(reader["f_MeetingAdr"]);
                            this.dtpMeetingDate.Value = DateTime.Parse(Strings.Format(reader["f_MeetingDateTime"], "yyyy-MM-dd"));
                            this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(reader["f_MeetingDateTime"], "yyyy-MM-dd HH:mm:ss"));
                            this.dtpStartTime.Value = DateTime.Parse(Strings.Format(reader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
                            this.dtpEndTime.Value = DateTime.Parse(Strings.Format(reader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
                            this.txtf_Content.Text = wgTools.SetObjToStr(reader["f_Content"]);
                            this.txtf_Notes.Text = wgTools.SetObjToStr(reader["f_Notes"]);
                        }
                        reader.Close();
                    }
                    this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                    this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                    this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                    this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
                    this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
                    this.dtpMeetingTime.CustomFormat = "HH:mm";
                    this.dtpMeetingTime.Format = DateTimePickerFormat.Custom;
                    this.dtpStartTime.CustomFormat = "HH:mm";
                    this.dtpStartTime.Format = DateTimePickerFormat.Custom;
                    this.dtpEndTime.CustomFormat = "HH:mm";
                    this.dtpEndTime.Format = DateTimePickerFormat.Custom;
                    wgAppConfig.setDisplayFormatDate(this.dtpMeetingDate, wgTools.DisplayFormat_DateYMDWeek);
                    this.backgroundWorker1.RunWorkerAsync();
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
                Cursor.Current = current;
            }
        }

        private void dfrmMeetingSet_Load_Acc(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            base.KeyPreview = true;
            try
            {
                this.cboIdentity.SelectedIndex = 0;
                this.loadGroupData();
                this.loadMeetingAdr();
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (this.curMeetingNo == "")
                {
                    this.txtf_MeetingNo.Text = Strings.Format(DateTime.Now, "yyyyMMdd_HHmmss");
                    this.dtpMeetingDate.Value = DateTime.Now.Date;
                    this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 9:00:00"));
                    this.dtpStartTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 8:00:00"));
                    this.dtpEndTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd 17:30:00"));
                }
                else
                {
                    this.txtf_MeetingNo.Text = this.curMeetingNo;
                    OleDbDataReader reader = new OleDbCommand("SELECT * FROM t_d_Meeting WHERE f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text), connection).ExecuteReader();
                    if (reader.Read())
                    {
                        this.txtf_MeetingName.Text = wgTools.SetObjToStr(reader["f_MeetingName"]);
                        this.cbof_MeetingAdr.Text = wgTools.SetObjToStr(reader["f_MeetingAdr"]);
                        this.dtpMeetingDate.Value = DateTime.Parse(Strings.Format(reader["f_MeetingDateTime"], "yyyy-MM-dd"));
                        this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(reader["f_MeetingDateTime"], "yyyy-MM-dd HH:mm:ss"));
                        this.dtpStartTime.Value = DateTime.Parse(Strings.Format(reader["f_SignStartTime"], "yyyy-MM-dd HH:mm:ss"));
                        this.dtpEndTime.Value = DateTime.Parse(Strings.Format(reader["f_SignEndTime"], "yyyy-MM-dd HH:mm:ss"));
                        this.txtf_Content.Text = wgTools.SetObjToStr(reader["f_Content"]);
                        this.txtf_Notes.Text = wgTools.SetObjToStr(reader["f_Notes"]);
                    }
                    reader.Close();
                }
                this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
                this.UserID.HeaderText = wgAppConfig.ReplaceWorkNO(this.UserID.HeaderText);
                this.dataGridViewTextBoxColumn2.HeaderText = wgAppConfig.ReplaceWorkNO(this.dataGridViewTextBoxColumn2.HeaderText);
                this.dtpMeetingTime.CustomFormat = "HH:mm";
                this.dtpMeetingTime.Format = DateTimePickerFormat.Custom;
                this.dtpStartTime.CustomFormat = "HH:mm";
                this.dtpStartTime.Format = DateTimePickerFormat.Custom;
                this.dtpEndTime.CustomFormat = "HH:mm";
                this.dtpEndTime.Format = DateTimePickerFormat.Custom;
                wgAppConfig.setDisplayFormatDate(this.dtpMeetingDate, wgTools.DisplayFormat_DateYMDWeek);
                this.backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
            Cursor.Current = current;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadGroupData()
        {
            new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
            for (int i = 0; i < this.arrGroupID.Count; i++)
            {
                if ((i == 0) && string.IsNullOrEmpty(this.arrGroupName[i].ToString()))
                {
                    this.cbof_GroupID.Items.Add(CommonStr.strAll);
                }
                else
                {
                    this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
                }
            }
            if (this.cbof_GroupID.Items.Count > 0)
            {
                this.cbof_GroupID.SelectedIndex = 0;
            }
        }

        public void loadMeetingAdr()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadMeetingAdr_Acc();
            }
            else
            {
                try
                {
                    this.cbof_MeetingAdr.Items.Clear();
                    DataSet dataSet = new DataSet();
                    SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                    try
                    {
                        dataSet.Clear();
                        SqlCommand selectCommand = new SqlCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", connection);
                        new SqlDataAdapter(selectCommand).Fill(dataSet, "t_d_MeetingAdr");
                        if (dataSet.Tables["t_d_MeetingAdr"].Rows.Count > 0)
                        {
                            for (int i = 0; i <= (dataSet.Tables["t_d_MeetingAdr"].Rows.Count - 1); i++)
                            {
                                this.cbof_MeetingAdr.Items.Add(dataSet.Tables["t_d_MeetingAdr"].Rows[i][0]);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        public void loadMeetingAdr_Acc()
        {
            try
            {
                this.cbof_MeetingAdr.Items.Clear();
                DataSet dataSet = new DataSet();
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                try
                {
                    dataSet.Clear();
                    OleDbCommand selectCommand = new OleDbCommand("Select DISTINCT f_MeetingAdr  from t_d_MeetingAdr ", connection);
                    new OleDbDataAdapter(selectCommand).Fill(dataSet, "t_d_MeetingAdr");
                    if (dataSet.Tables["t_d_MeetingAdr"].Rows.Count > 0)
                    {
                        for (int i = 0; i <= (dataSet.Tables["t_d_MeetingAdr"].Rows.Count - 1); i++)
                        {
                            this.cbof_MeetingAdr.Items.Add(dataSet.Tables["t_d_MeetingAdr"].Rows[i][0]);
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private DataTable loadUserData4BackWork()
        {
            string str;
            int num;
            Thread.Sleep(100);
            wgTools.WriteLine("loadUserData Start");
            this.dtUser1 = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT  t_b_Consumer.f_ConsumerID ";
                str = ((((str + " , f_MeetingIdentity,' ' as  f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , f_Seat " + " ,IIF (t_d_MeetingConsumer.f_MeetingIdentity IS NULL, 0,  IIF (  t_d_MeetingConsumer.f_MeetingIdentity <0 , 0 , 1 )) AS f_Selected ") + " , f_GroupID " + " FROM t_b_Consumer ") + " LEFT OUTER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text) + ")") + " ORDER BY f_ConsumerNO ASC ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtUser1);
                        }
                    }
                    goto Label_01A9;
                }
            }
            str = " SELECT  t_b_Consumer.f_ConsumerID ";
            str = ((((str + " , f_MeetingIdentity,' ' as f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , f_Seat " + " , CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity IS NULL THEN 0 ELSE CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity < 0 THEN 0 ELSE 1 END END AS f_Selected ") + " , f_GroupID " + " FROM t_b_Consumer ") + " LEFT OUTER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStr(this.txtf_MeetingNo.Text) + ")") + " ORDER BY f_ConsumerNO ASC ";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtUser1);
                    }
                }
            }
        Label_01A9:
            num = 0;
            while (num < this.dtUser1.Rows.Count)
            {
                DataRow row = this.dtUser1.Rows[num];
                if (!string.IsNullOrEmpty(row["f_MeetingIdentity"].ToString()) && (((int) row["f_MeetingIdentity"]) >= 0))
                {
                    row["f_MeetingIdentityStr"] = this.cboIdentity.Items[(int) row["f_MeetingIdentity"]].ToString();
                }
                num++;
            }
            this.dtUser1.AcceptChanges();
            wgTools.WriteLine("da.Fill End");
            try
            {
                this.dtUser1.PrimaryKey = new DataColumn[] { this.dtUser1.Columns[0] };
            }
            catch (Exception)
            {
                throw;
            }
            lastLoadUsers = icConsumerShare.getUpdateLog();
            dtLastLoad = this.dtUser1;
            return this.dtUser1;
        }

        private void loadUserData4BackWorkComplete(DataTable dtUser)
        {
            this.dv = new DataView(dtUser);
            this.dvSelected = new DataView(dtUser);
            this.dv.RowFilter = "f_Selected = 0";
            this.dvSelected.RowFilter = "f_Selected > 0";
            this.dvSelected.Sort = "f_MeetingIdentity ASC, f_ConsumerNo ASC ";
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.DataSource = this.dv;
            this.dgvSelectedUsers.AutoGenerateColumns = false;
            this.dgvSelectedUsers.DataSource = this.dvSelected;
            for (int i = 0; i < this.dv.Table.Columns.Count; i++)
            {
                this.dgvUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
                this.dgvSelectedUsers.Columns[i].DataPropertyName = dtUser.Columns[i].ColumnName;
            }
            this.cbof_GroupID_SelectedIndexChanged(null, null);
            wgTools.WriteLine("loadUserData End");
        }

        public static void selectObject(DataGridView dgv, string secondField, int val, string secondField2, string val2, string secondField3, string val3)
        {
            try
            {
                int rowIndex;
                if (dgv.SelectedRows.Count <= 0)
                {
                    if (dgv.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = dgv.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = dgv.SelectedRows[0].Index;
                }
                using (DataTable table = ((DataView) dgv.DataSource).Table)
                {
                    DataRow row;
                    if (dgv.SelectedRows.Count > 0)
                    {
                        int count = dgv.SelectedRows.Count;
                        int[] numArray = new int[count];
                        for (int i = 0; i < dgv.SelectedRows.Count; i++)
                        {
                            numArray[i] = (int) dgv.SelectedRows[i].Cells[0].Value;
                        }
                        for (int j = 0; j < count; j++)
                        {
                            int key = numArray[j];
                            row = table.Rows.Find(key);
                            if (row != null)
                            {
                                row["f_Selected"] = 1;
                                if (secondField != "")
                                {
                                    row[secondField] = val;
                                }
                                if (secondField2 != "")
                                {
                                    row[secondField2] = val2;
                                }
                                if (secondField3 != "")
                                {
                                    row[secondField3] = val3;
                                }
                            }
                        }
                    }
                    else
                    {
                        int num6 = (int) dgv.Rows[rowIndex].Cells[0].Value;
                        row = table.Rows.Find(num6);
                        if (row != null)
                        {
                            row["f_Selected"] = 1;
                            if (secondField != "")
                            {
                                row[secondField] = val;
                            }
                            if (secondField2 != "")
                            {
                                row[secondField2] = val2;
                            }
                            if (secondField3 != "")
                            {
                                row[secondField3] = val3;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.dgvUsers.DataSource == null)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                this.timer1.Enabled = false;
                Cursor.Current = Cursors.Default;
                this.lblWait.Visible = false;
                this.groupBox1.Enabled = true;
                this.btnOK.Enabled = true;
            }
        }
    }
}

