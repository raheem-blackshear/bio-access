namespace WG3000_COMM.ExtendFunc.Meeting
{
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmManualSign : frmBioAccess
    {
        public string curMeetingNo = "";
        public string curMode = "";
        private dfrmFind dfrmFind1 = new dfrmFind();
        private DataSet ds = new DataSet();
        private DataTable dtUser1;

        public dfrmManualSign()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvSelectedUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvSelectedUsers.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvSelectedUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvSelectedUsers.SelectedRows[0].Index;
                }
                string str = "'";
                str = " UPDATE t_d_MeetingConsumer ";
                if (wgAppConfig.runUpdateSql((((str + "SET f_SignWay = 0 ") + " , f_SignRealTime = NULL " + " , f_RecID = 0 ") + " WHERE  f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo)) + " AND f_ConsumerID = " + this.dgvSelectedUsers.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                {
                    base.Close();
                    base.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex;
                if (this.dgvSelectedUsers.SelectedRows.Count <= 0)
                {
                    if (this.dgvSelectedUsers.SelectedCells.Count <= 0)
                    {
                        return;
                    }
                    rowIndex = this.dgvSelectedUsers.SelectedCells[0].RowIndex;
                }
                else
                {
                    rowIndex = this.dgvSelectedUsers.SelectedRows[0].Index;
                }
                if ((this.curMode == "") || (this.curMode.ToUpper() == "ManualSign".ToUpper()))
                {
                    string str = "'";
                    str = " UPDATE t_d_MeetingConsumer ";
                    if (wgAppConfig.runUpdateSql(((((str + "SET f_SignWay = 1 ") + " , f_SignRealTime = " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , f_RecID = 0 ") + " WHERE  f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo)) + " AND f_ConsumerID = " + this.dgvSelectedUsers.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                    {
                        base.Close();
                        base.DialogResult = DialogResult.OK;
                    }
                }
                if (this.curMode.ToUpper() == "Leave".ToUpper())
                {
                    string str2 = "'";
                    str2 = " UPDATE t_d_MeetingConsumer ";
                    if (wgAppConfig.runUpdateSql(((((str2 + "SET f_SignWay = 2 ") + " , f_SignRealTime = " + wgTools.PrepareStr(this.dtpMeetingDate.Value.ToString("yyyy-MM-dd") + " " + this.dtpMeetingTime.Value.ToString("HH:mm:ss"), true, "yyyy-MM-dd HH:mm:ss")) + " , f_RecID = 0 ") + " WHERE  f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo)) + " AND f_ConsumerID = " + this.dgvSelectedUsers.Rows[rowIndex].Cells[0].Value.ToString()) == 1)
                    {
                        base.Close();
                        base.DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmManualSign_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmManualSign_KeyDown(object sender, KeyEventArgs e)
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

        private void dfrmManualSign_Load(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            base.KeyPreview = true;
            try
            {
                string str;
                int num;
                if (this.curMeetingNo == "")
                {
                    base.Close();
                    return;
                }
                this.dtpMeetingDate.Value = DateTime.Now.Date;
                this.dtpMeetingTime.Value = DateTime.Parse(Strings.Format(DateTime.Now, "yyyy-MM-dd HH:mm:ss"));
                this.dtpMeetingTime.CustomFormat = "HH:mm:ss";
                this.dtpMeetingTime.Format = DateTimePickerFormat.Custom;
                this.dtUser1 = new DataTable();
                if (wgAppConfig.IsAccessDB)
                {
                    str = " SELECT  t_b_Consumer.f_ConsumerID ";
                    str = ((((str + " , f_MeetingIdentity,' ' as  f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , f_Seat " + " ,IIF (t_d_MeetingConsumer.f_MeetingIdentity IS NULL, 0,  IIF (  t_d_MeetingConsumer.f_MeetingIdentity <0 , 0 , 1 )) AS f_Selected ") + " , f_GroupID " + " FROM t_b_Consumer ") + " INNER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo) + ")") + " ORDER BY f_ConsumerNO ASC ";
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(str, connection))
                        {
                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                            {
                                adapter.Fill(this.dtUser1);
                            }
                        }
                        goto Label_021F;
                    }
                }
                str = " SELECT  t_b_Consumer.f_ConsumerID ";
                str = ((((str + " , f_MeetingIdentity,' ' as f_MeetingIdentityStr, f_ConsumerNO, f_ConsumerName, f_CardNO ") + " , f_Seat " + " , CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity IS NULL THEN 0 ELSE CASE WHEN t_d_MeetingConsumer.f_MeetingIdentity < 0 THEN 0 ELSE 1 END END AS f_Selected ") + " , f_GroupID " + " FROM t_b_Consumer ") + " INNER JOIN t_d_MeetingConsumer ON ( t_b_Consumer.f_ConsumerID = t_d_MeetingConsumer.f_ConsumerID AND f_MeetingNO = " + wgTools.PrepareStr(this.curMeetingNo) + ")") + " ORDER BY f_ConsumerNO ASC ";
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
            Label_021F:
                num = 0;
                while (num < this.dtUser1.Rows.Count)
                {
                    DataRow row = this.dtUser1.Rows[num];
                    if (!string.IsNullOrEmpty(row["f_MeetingIdentity"].ToString()) && (((int) row["f_MeetingIdentity"]) >= 0))
                    {
                        row["f_MeetingIdentityStr"] = frmMeetings.getStrMeetingIdentity((long) ((int) row["f_MeetingIdentity"]));
                    }
                    num++;
                }
                this.dtUser1.AcceptChanges();
                DataView view = new DataView(this.dtUser1);
                for (int i = 0; i < view.Table.Columns.Count; i++)
                {
                    this.dgvSelectedUsers.Columns[i].DataPropertyName = this.dtUser1.Columns[i].ColumnName;
                }
                this.dgvSelectedUsers.DataSource = view;
                this.dgvSelectedUsers.DefaultCellStyle.ForeColor = Color.Black;
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
    }
}

