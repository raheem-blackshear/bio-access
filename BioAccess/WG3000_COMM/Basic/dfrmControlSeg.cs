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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControlSeg : frmBioAccess
    {
        public string operateMode = "";

        public dfrmControlSeg()
        {
            this.InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.cmdOK_Click_Acc(sender, e);
            }
            else
            {
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    string str;
                    if (this.operateMode == "New")
                    {
                        str = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.cbof_ControlSegID.Text;
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(str, connection))
                        {
                            SqlDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                            if (reader.Read())
                            {
                                reader.Close();
                                XMessageBox.Show(this, CommonStr.strIDIsDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                reader.Close();
                                str = " INSERT INTO t_b_ControlSeg([f_ControlSegID], [f_Monday], [f_Tuesday], [f_Wednesday]";
                                decimal num = this.nudf_LimitedTimesOfDay.Value + (((int) this.nudf_LimitedTimesOfMonth.Value) << 8);
                                using (SqlCommand command2 = new SqlCommand((((((((((((((((((((((((((((str + " , [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday] " + " , [f_BeginHMS1],[f_EndHMS1], [f_BeginHMS2], [f_EndHMS2], [f_BeginHMS3], [f_EndHMS3]") + " , [f_BeginYMD],[f_EndYMD], [f_ControlSegName], [f_ControlSegIDLinked]" + " , [f_ReaderCount],[f_LimitedTimesOfDay], [f_LimitedTimesOfHMS1], [f_LimitedTimesOfHMS2], [f_LimitedTimesOfHMS3]") + " , [f_ControlByHoliday] " + ") ") + " VALUES ( " + this.cbof_ControlSegID.Text) + " , " + (this.chkMonday.Checked ? "1" : "0")) + " , " + (this.chkTuesday.Checked ? "1" : "0")) + " , " + (this.chkWednesday.Checked ? "1" : "0")) + " , " + (this.chkThursday.Checked ? "1" : "0")) + " , " + (this.chkFriday.Checked ? "1" : "0")) + " , " + (this.chkSaturday.Checked ? "1" : "0")) + " , " + (this.chkSunday.Checked ? "1" : "0")) + " , " + this.getDateString(this.dateBeginHMS1)) + " , " + this.getDateString(this.dateEndHMS1)) + " , " + this.getDateString(this.dateBeginHMS2)) + " , " + this.getDateString(this.dateEndHMS2)) + " , " + this.getDateString(this.dateBeginHMS3)) + " , " + this.getDateString(this.dateEndHMS3)) + " , " + this.getDateString(this.dtpBegin)) + " , " + this.getDateString(this.dtpEnd)) + " , " + wgTools.PrepareStr(this.txtf_ControlSegName.Text)) + " , " + this.cbof_ControlSegIDLinked.Text) + " , " + (this.optReaderCount.Checked ? "1" : "0")) + " , " + num.ToString()) + " , " + this.nudf_LimitedTimesOfHMS1.Value.ToString()) + " , " + this.nudf_LimitedTimesOfHMS2.Value.ToString()) + " , " + this.nudf_LimitedTimesOfHMS3.Value.ToString()) + " , " + (this.chkNotAllowInHolidays.Checked ? "1" : "0")) + ")", connection))
                                {
                                    command2.ExecuteNonQuery();
                                    base.DialogResult = DialogResult.OK;
                                    base.Close();
                                }
                            }
                            return;
                        }
                    }
                    connection.Open();
                    str = " UPDATE t_b_ControlSeg ";
                    decimal num5 = this.nudf_LimitedTimesOfDay.Value + (((int) this.nudf_LimitedTimesOfMonth.Value) << 8);
                    using (SqlCommand command3 = new SqlCommand(((((((((((((((((((((((((((((((str + " SET  [f_Monday]= " + (this.chkMonday.Checked ? "1" : "0")) + ", [f_Tuesday]=" + (this.chkTuesday.Checked ? "1" : "0")) + ", [f_Wednesday]=" + (this.chkWednesday.Checked ? "1" : "0")) + " , [f_Thursday]= " + (this.chkThursday.Checked ? "1" : "0")) + " ,[f_Friday]=" + (this.chkFriday.Checked ? "1" : "0")) + " , [f_Saturday]=" + (this.chkSaturday.Checked ? "1" : "0")) + " , [f_Sunday] =" + (this.chkSunday.Checked ? "1" : "0")) + " , [f_BeginHMS1]=" + this.getDateString(this.dateBeginHMS1)) + " ,[f_EndHMS1]=" + this.getDateString(this.dateEndHMS1)) + " , [f_BeginHMS2]=" + this.getDateString(this.dateBeginHMS2)) + " ,[f_EndHMS2]=" + this.getDateString(this.dateEndHMS2)) + " , [f_BeginHMS3]=" + this.getDateString(this.dateBeginHMS3)) + " ,[f_EndHMS3]=" + this.getDateString(this.dateEndHMS3)) + " , [f_BeginYMD]=" + this.getDateString(this.dtpBegin)) + " , [f_EndYMD]=" + this.getDateString(this.dtpEnd)) + " , [f_ControlSegName]=") + " " + wgTools.PrepareStr(this.txtf_ControlSegName.Text)) + " , [f_ControlSegIDLinked]=") + " " + this.cbof_ControlSegIDLinked.Text) + " , [f_ReaderCount]=") + "  " + (this.optReaderCount.Checked ? "1" : "0")) + " , [f_LimitedTimesOfDay]=") + "  " + num5.ToString()) + " , [f_LimitedTimesOfHMS1]=") + "  " + this.nudf_LimitedTimesOfHMS1.Value.ToString()) + " , [f_LimitedTimesOfHMS2]=") + "  " + this.nudf_LimitedTimesOfHMS2.Value.ToString()) + " , [f_LimitedTimesOfHMS3]=") + "  " + this.nudf_LimitedTimesOfHMS3.Value.ToString()) + " , [f_ControlByHoliday] =" + (this.chkNotAllowInHolidays.Checked ? "1" : "0")) + " WHERE [f_ControlSegID]= " + this.cbof_ControlSegID.Text, connection))
                    {
                        command3.ExecuteNonQuery();
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }

        private void cmdOK_Click_Acc(object sender, EventArgs e)
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                string str;
                if (this.operateMode == "New")
                {
                    str = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.cbof_ControlSegID.Text;
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        OleDbDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                        {
                            reader.Close();
                            XMessageBox.Show(this, CommonStr.strIDIsDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            reader.Close();
                            str = " INSERT INTO t_b_ControlSeg([f_ControlSegID], [f_Monday], [f_Tuesday], [f_Wednesday]";
                            decimal num = this.nudf_LimitedTimesOfDay.Value + (((int) this.nudf_LimitedTimesOfMonth.Value) << 8);
                            using (OleDbCommand command2 = new OleDbCommand((((((((((((((((((((((((((((str + " , [f_Thursday], [f_Friday], [f_Saturday], [f_Sunday] " + " , [f_BeginHMS1],[f_EndHMS1], [f_BeginHMS2], [f_EndHMS2], [f_BeginHMS3], [f_EndHMS3]") + " , [f_BeginYMD],[f_EndYMD], [f_ControlSegName], [f_ControlSegIDLinked]" + " , [f_ReaderCount],[f_LimitedTimesOfDay], [f_LimitedTimesOfHMS1], [f_LimitedTimesOfHMS2], [f_LimitedTimesOfHMS3]") + " , [f_ControlByHoliday] " + ") ") + " VALUES ( " + this.cbof_ControlSegID.Text) + " , " + (this.chkMonday.Checked ? "1" : "0")) + " , " + (this.chkTuesday.Checked ? "1" : "0")) + " , " + (this.chkWednesday.Checked ? "1" : "0")) + " , " + (this.chkThursday.Checked ? "1" : "0")) + " , " + (this.chkFriday.Checked ? "1" : "0")) + " , " + (this.chkSaturday.Checked ? "1" : "0")) + " , " + (this.chkSunday.Checked ? "1" : "0")) + " , " + this.getDateString(this.dateBeginHMS1)) + " , " + this.getDateString(this.dateEndHMS1)) + " , " + this.getDateString(this.dateBeginHMS2)) + " , " + this.getDateString(this.dateEndHMS2)) + " , " + this.getDateString(this.dateBeginHMS3)) + " , " + this.getDateString(this.dateEndHMS3)) + " , " + this.getDateString(this.dtpBegin)) + " , " + this.getDateString(this.dtpEnd)) + " , " + wgTools.PrepareStr(this.txtf_ControlSegName.Text)) + " , " + this.cbof_ControlSegIDLinked.Text) + " , " + (this.optReaderCount.Checked ? "1" : "0")) + " , " + num.ToString()) + " , " + this.nudf_LimitedTimesOfHMS1.Value.ToString()) + " , " + this.nudf_LimitedTimesOfHMS2.Value.ToString()) + " , " + this.nudf_LimitedTimesOfHMS3.Value.ToString()) + " , " + (this.chkNotAllowInHolidays.Checked ? "1" : "0")) + ")", connection))
                            {
                                command2.ExecuteNonQuery();
                                base.DialogResult = DialogResult.OK;
                                base.Close();
                            }
                        }
                        return;
                    }
                }
                connection.Open();
                str = " UPDATE t_b_ControlSeg ";
                decimal num5 = this.nudf_LimitedTimesOfDay.Value + (((int) this.nudf_LimitedTimesOfMonth.Value) << 8);
                using (OleDbCommand command3 = new OleDbCommand(((((((((((((((((((((((((((((((str + " SET  [f_Monday]= " + (this.chkMonday.Checked ? "1" : "0")) + ", [f_Tuesday]=" + (this.chkTuesday.Checked ? "1" : "0")) + ", [f_Wednesday]=" + (this.chkWednesday.Checked ? "1" : "0")) + " , [f_Thursday]= " + (this.chkThursday.Checked ? "1" : "0")) + " ,[f_Friday]=" + (this.chkFriday.Checked ? "1" : "0")) + " , [f_Saturday]=" + (this.chkSaturday.Checked ? "1" : "0")) + " , [f_Sunday] =" + (this.chkSunday.Checked ? "1" : "0")) + " , [f_BeginHMS1]=" + this.getDateString(this.dateBeginHMS1)) + " ,[f_EndHMS1]=" + this.getDateString(this.dateEndHMS1)) + " , [f_BeginHMS2]=" + this.getDateString(this.dateBeginHMS2)) + " ,[f_EndHMS2]=" + this.getDateString(this.dateEndHMS2)) + " , [f_BeginHMS3]=" + this.getDateString(this.dateBeginHMS3)) + " ,[f_EndHMS3]=" + this.getDateString(this.dateEndHMS3)) + " , [f_BeginYMD]=" + this.getDateString(this.dtpBegin)) + " , [f_EndYMD]=" + this.getDateString(this.dtpEnd)) + " , [f_ControlSegName]=") + " " + wgTools.PrepareStr(this.txtf_ControlSegName.Text)) + " , [f_ControlSegIDLinked]=") + " " + this.cbof_ControlSegIDLinked.Text) + " , [f_ReaderCount]=") + "  " + (this.optReaderCount.Checked ? "1" : "0")) + " , [f_LimitedTimesOfDay]=") + "  " + num5.ToString()) + " , [f_LimitedTimesOfHMS1]=") + "  " + this.nudf_LimitedTimesOfHMS1.Value.ToString()) + " , [f_LimitedTimesOfHMS2]=") + "  " + this.nudf_LimitedTimesOfHMS2.Value.ToString()) + " , [f_LimitedTimesOfHMS3]=") + "  " + this.nudf_LimitedTimesOfHMS3.Value.ToString()) + " , [f_ControlByHoliday] =" + (this.chkNotAllowInHolidays.Checked ? "1" : "0")) + " WHERE [f_ControlSegID]= " + this.cbof_ControlSegID.Text, connection))
                {
                    command3.ExecuteNonQuery();
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                connection.Dispose();
            }
        }

        private void dfrmControlSeg_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                this.chkNotAllowInHolidays.Visible = true;
            }
        }

        private void dfrmControlSeg_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmControlSeg_Load_Acc(sender, e);
            }
            else
            {
                int num;
                if (wgAppConfig.getParamValBoolByNO(0x88))
                {
                    base.Size = new Size(new Point(680, base.Size.Height));
                }
                this.cbof_ControlSegID.Items.Clear();
                this.cbof_ControlSegIDLinked.Items.Clear();
                for (num = 2; num <= 0xff; num++)
                {
                    this.cbof_ControlSegID.Items.Add(num);
                }
                for (num = 0; num <= 0xff; num++)
                {
                    this.cbof_ControlSegIDLinked.Items.Add(num);
                }
                this.cbof_ControlSegIDLinked.Text = "0";
                this.dateBeginHMS1.CustomFormat = "HH:mm";
                this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
                this.dateEndHMS1.CustomFormat = "HH:mm";
                this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
                this.dateBeginHMS2.CustomFormat = "HH:mm";
                this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS2.Value = DateTime.Parse("00:00:00");
                this.dateEndHMS2.CustomFormat = "HH:mm";
                this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS2.Value = DateTime.Parse("00:00:00");
                this.dateBeginHMS3.CustomFormat = "HH:mm";
                this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS3.Value = DateTime.Parse("00:00:00");
                this.dateEndHMS3.CustomFormat = "HH:mm";
                this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS3.Value = DateTime.Parse("00:00:00");
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    if (this.operateMode == "New")
                    {
                        this.cbof_ControlSegID.Enabled = true;
                        string cmdText = " SELECT * FROM t_b_ControlSeg ORDER BY [f_ControlSegID] DESC ";
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        using (SqlCommand command = new SqlCommand(cmdText, connection))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                this.curControlSegID = ((int) reader["f_ControlSegID"]) + 1;
                            }
                            else
                            {
                                this.curControlSegID = 2;
                            }
                            reader.Close();
                        }
                        this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
                    }
                    else
                    {
                        this.cbof_ControlSegID.Enabled = false;
                        this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
                        string str2 = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.curControlSegID.ToString();
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        using (SqlCommand command2 = new SqlCommand(str2, connection))
                        {
                            SqlDataReader reader2 = command2.ExecuteReader();
                            if (reader2.Read())
                            {
                                try
                                {
                                    this.chkMonday.Checked = reader2["f_Monday"].ToString() == "1";
                                    this.chkTuesday.Checked = reader2["f_Tuesday"].ToString() == "1";
                                    this.chkWednesday.Checked = reader2["f_Wednesday"].ToString() == "1";
                                    this.chkThursday.Checked = reader2["f_Thursday"].ToString() == "1";
                                    this.chkFriday.Checked = reader2["f_Friday"].ToString() == "1";
                                    this.chkSaturday.Checked = reader2["f_Saturday"].ToString() == "1";
                                    this.chkSunday.Checked = reader2["f_Sunday"].ToString() == "1";
                                    this.dateBeginHMS1.Value = DateTime.Parse(reader2["f_BeginHMS1"].ToString());
                                    this.dateBeginHMS2.Value = DateTime.Parse(reader2["f_BeginHMS2"].ToString());
                                    this.dateBeginHMS3.Value = DateTime.Parse(reader2["f_BeginHMS3"].ToString());
                                    this.dateEndHMS1.Value = DateTime.Parse(reader2["f_EndHMS1"].ToString());
                                    this.dateEndHMS2.Value = DateTime.Parse(reader2["f_EndHMS2"].ToString());
                                    this.dateEndHMS3.Value = DateTime.Parse(reader2["f_EndHMS3"].ToString());
                                    this.dtpBegin.Value = DateTime.Parse(reader2["f_BeginYMD"].ToString());
                                    this.dtpEnd.Value = DateTime.Parse(reader2["f_EndYMD"].ToString());
                                    this.txtf_ControlSegName.Text = wgTools.SetObjToStr(reader2["f_ControlSegName"]);
                                    this.cbof_ControlSegIDLinked.Text = reader2["f_ControlSegIDLinked"].ToString();
                                    this.chkf_ReaderCount.Checked = (int.Parse(reader2["f_ReaderCount"].ToString()) & 1) > 0;
                                    this.optControllerCount.Checked = (int.Parse(reader2["f_ReaderCount"].ToString()) & 1) == 0;
                                    this.optReaderCount.Checked = (int.Parse(reader2["f_ReaderCount"].ToString()) & 1) > 0;
                                    this.nudf_LimitedTimesOfDay.Value = ((int) reader2["f_LimitedTimesOfDay"]) & 0xff;
                                    this.nudf_LimitedTimesOfMonth.Value = (((int) reader2["f_LimitedTimesOfDay"]) >> 8) & 0xff;
                                    this.nudf_LimitedTimesOfHMS1.Value = (int) reader2["f_LimitedTimesOfHMS1"];
                                    this.nudf_LimitedTimesOfHMS2.Value = (int) reader2["f_LimitedTimesOfHMS2"];
                                    this.nudf_LimitedTimesOfHMS3.Value = (int) reader2["f_LimitedTimesOfHMS3"];
                                    this.chkNotAllowInHolidays.Checked = reader2["f_ControlByHoliday"].ToString() == "1";
                                    if (!this.chkNotAllowInHolidays.Checked)
                                    {
                                        this.chkNotAllowInHolidays.Visible = true;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            reader2.Close();
                        }
                    }
                }
                wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
                wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMD);
            }
        }

        private void dfrmControlSeg_Load_Acc(object sender, EventArgs e)
        {
            int num;
            if (wgAppConfig.getParamValBoolByNO(0x88))
            {
                base.Size = new Size(new Point(680, base.Size.Height));
            }
            this.cbof_ControlSegID.Items.Clear();
            this.cbof_ControlSegIDLinked.Items.Clear();
            for (num = 2; num <= 0xff; num++)
            {
                this.cbof_ControlSegID.Items.Add(num);
            }
            for (num = 0; num <= 0xff; num++)
            {
                this.cbof_ControlSegIDLinked.Items.Add(num);
            }
            this.cbof_ControlSegIDLinked.Text = "0";
            this.dateBeginHMS1.CustomFormat = "HH:mm";
            this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
            this.dateEndHMS1.CustomFormat = "HH:mm";
            this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
            this.dateBeginHMS2.CustomFormat = "HH:mm";
            this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS2.Value = DateTime.Parse("00:00:00");
            this.dateEndHMS2.CustomFormat = "HH:mm";
            this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS2.Value = DateTime.Parse("00:00:00");
            this.dateBeginHMS3.CustomFormat = "HH:mm";
            this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS3.Value = DateTime.Parse("00:00:00");
            this.dateEndHMS3.CustomFormat = "HH:mm";
            this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS3.Value = DateTime.Parse("00:00:00");
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                if (this.operateMode == "New")
                {
                    this.cbof_ControlSegID.Enabled = true;
                    string cmdText = " SELECT * FROM t_b_ControlSeg ORDER BY [f_ControlSegID] DESC ";
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            this.curControlSegID = ((int) reader["f_ControlSegID"]) + 1;
                        }
                        else
                        {
                            this.curControlSegID = 2;
                        }
                        reader.Close();
                    }
                    this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
                }
                else
                {
                    this.cbof_ControlSegID.Enabled = false;
                    this.cbof_ControlSegID.Text = this.curControlSegID.ToString();
                    string str2 = " SELECT * FROM t_b_ControlSeg WHERE [f_ControlSegID]= " + this.curControlSegID.ToString();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand command2 = new OleDbCommand(str2, connection))
                    {
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        if (reader2.Read())
                        {
                            try
                            {
                                this.chkMonday.Checked = reader2["f_Monday"].ToString() == "1";
                                this.chkTuesday.Checked = reader2["f_Tuesday"].ToString() == "1";
                                this.chkWednesday.Checked = reader2["f_Wednesday"].ToString() == "1";
                                this.chkThursday.Checked = reader2["f_Thursday"].ToString() == "1";
                                this.chkFriday.Checked = reader2["f_Friday"].ToString() == "1";
                                this.chkSaturday.Checked = reader2["f_Saturday"].ToString() == "1";
                                this.chkSunday.Checked = reader2["f_Sunday"].ToString() == "1";
                                this.dateBeginHMS1.Value = DateTime.Parse(reader2["f_BeginHMS1"].ToString());
                                this.dateBeginHMS2.Value = DateTime.Parse(reader2["f_BeginHMS2"].ToString());
                                this.dateBeginHMS3.Value = DateTime.Parse(reader2["f_BeginHMS3"].ToString());
                                this.dateEndHMS1.Value = DateTime.Parse(reader2["f_EndHMS1"].ToString());
                                this.dateEndHMS2.Value = DateTime.Parse(reader2["f_EndHMS2"].ToString());
                                this.dateEndHMS3.Value = DateTime.Parse(reader2["f_EndHMS3"].ToString());
                                this.dtpBegin.Value = DateTime.Parse(reader2["f_BeginYMD"].ToString());
                                this.dtpEnd.Value = DateTime.Parse(reader2["f_EndYMD"].ToString());
                                this.txtf_ControlSegName.Text = wgTools.SetObjToStr(reader2["f_ControlSegName"]);
                                this.cbof_ControlSegIDLinked.Text = reader2["f_ControlSegIDLinked"].ToString();
                                this.chkf_ReaderCount.Checked = (int.Parse(reader2["f_ReaderCount"].ToString()) & 1) > 0;
                                this.optControllerCount.Checked = (int.Parse(reader2["f_ReaderCount"].ToString()) & 1) == 0;
                                this.optReaderCount.Checked = (int.Parse(reader2["f_ReaderCount"].ToString()) & 1) > 0;
                                this.nudf_LimitedTimesOfDay.Value = ((int) reader2["f_LimitedTimesOfDay"]) & 0xff;
                                this.nudf_LimitedTimesOfMonth.Value = (((int) reader2["f_LimitedTimesOfDay"]) >> 8) & 0xff;
                                this.nudf_LimitedTimesOfHMS1.Value = (int) reader2["f_LimitedTimesOfHMS1"];
                                this.nudf_LimitedTimesOfHMS2.Value = (int) reader2["f_LimitedTimesOfHMS2"];
                                this.nudf_LimitedTimesOfHMS3.Value = (int) reader2["f_LimitedTimesOfHMS3"];
                                this.chkNotAllowInHolidays.Checked = reader2["f_ControlByHoliday"].ToString() == "1";
                                if (!this.chkNotAllowInHolidays.Checked)
                                {
                                    this.chkNotAllowInHolidays.Visible = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        reader2.Close();
                    }
                }
            }
            wgAppConfig.setDisplayFormatDate(this.dtpBegin, wgTools.DisplayFormat_DateYMD);
            wgAppConfig.setDisplayFormatDate(this.dtpEnd, wgTools.DisplayFormat_DateYMD);
        }

        private string getDateString(DateTimePicker dtp)
        {
            if (dtp == null)
            {
                return wgTools.PrepareStr("");
            }
            return wgTools.PrepareStr(dtp.Value.ToString(wgTools.YMDHMSFormat), true, wgTools.YMDHMSFormat);
        }
    }
}

