namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmShiftOtherTypeSet : frmBioAccess
    {
        public int curShiftID;
        public string operateMode = "";

        public dfrmShiftOtherTypeSet()
        {
            this.InitializeComponent();
        }

        private void cbof_Readtimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBox box = this.chkBOvertimeShift1;
            this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
            this.groupBox3.Visible = false;
            this.groupBox4.Visible = false;
            this.chkBOvertimeShift1.Visible = false;
            this.chkBOvertimeShift2.Visible = false;
            this.chkBOvertimeShift3.Visible = false;
            this.chkBOvertimeShift4.Visible = false;
            if (!string.IsNullOrEmpty(this.cbof_Readtimes.Text))
            {
                if (int.Parse(this.cbof_Readtimes.Text) >= 2)
                {
                    this.groupBox1.Visible = true;
                    box = this.chkBOvertimeShift1;
                }
                if (int.Parse(this.cbof_Readtimes.Text) >= 4)
                {
                    this.groupBox2.Visible = true;
                    box = this.chkBOvertimeShift2;
                }
                if (int.Parse(this.cbof_Readtimes.Text) >= 6)
                {
                    this.groupBox3.Visible = true;
                    box = this.chkBOvertimeShift3;
                }
                if (int.Parse(this.cbof_Readtimes.Text) >= 8)
                {
                    this.groupBox4.Visible = true;
                    box = this.chkBOvertimeShift4;
                }
            }
            box.Visible = true;
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
                int num2;
                int bOvertimeShift = 0;
                if (this.chkBOvertimeShift.Checked)
                {
                    bOvertimeShift = 1;
                }
                else if (this.chkBOvertimeShift1.Visible)
                {
                    bOvertimeShift = this.chkBOvertimeShift1.Checked ? 2 : 0;
                }
                else if (this.chkBOvertimeShift2.Visible)
                {
                    bOvertimeShift = this.chkBOvertimeShift2.Checked ? 2 : 0;
                }
                else if (this.chkBOvertimeShift3.Visible)
                {
                    bOvertimeShift = this.chkBOvertimeShift3.Checked ? 2 : 0;
                }
                else if (this.chkBOvertimeShift4.Visible)
                {
                    bOvertimeShift = this.chkBOvertimeShift4.Checked ? 2 : 0;
                }
                if (this.operateMode == "New")
                {
                    using (comShift shift = new comShift())
                    {
                        num2 = shift.shift_add(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, bOvertimeShift);
                        if (num2 != 0)
                        {
                            XMessageBox.Show(this, shift.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            base.DialogResult = DialogResult.OK;
                            base.Close();
                        }
                        return;
                    }
                }
                using (comShift shift2 = new comShift())
                {
                    num2 = shift2.shift_update(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, bOvertimeShift);
                    if (num2 != 0)
                    {
                        XMessageBox.Show(this, shift2.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
            }
        }

        private void cmdOK_Click_Acc(object sender, EventArgs e)
        {
            int num2;
            int bOvertimeShift = 0;
            if (this.chkBOvertimeShift.Checked)
            {
                bOvertimeShift = 1;
            }
            else if (this.chkBOvertimeShift1.Visible)
            {
                bOvertimeShift = this.chkBOvertimeShift1.Checked ? 2 : 0;
            }
            else if (this.chkBOvertimeShift2.Visible)
            {
                bOvertimeShift = this.chkBOvertimeShift2.Checked ? 2 : 0;
            }
            else if (this.chkBOvertimeShift3.Visible)
            {
                bOvertimeShift = this.chkBOvertimeShift3.Checked ? 2 : 0;
            }
            else if (this.chkBOvertimeShift4.Visible)
            {
                bOvertimeShift = this.chkBOvertimeShift4.Checked ? 2 : 0;
            }
            if (this.operateMode == "New")
            {
                using (comShift_Acc acc = new comShift_Acc())
                {
                    num2 = acc.shift_add(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, bOvertimeShift);
                    if (num2 != 0)
                    {
                        XMessageBox.Show(this, acc.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                    return;
                }
            }
            using (comShift_Acc acc2 = new comShift_Acc())
            {
                num2 = acc2.shift_update(int.Parse(this.cbof_ShiftID.Text), this.txtName.Text.Trim(), int.Parse(this.cbof_Readtimes.Text), this.dateBeginHMS1.Value, this.dateEndHMS1.Value, this.dateBeginHMS2.Value, this.dateEndHMS2.Value, this.dateBeginHMS3.Value, this.dateEndHMS3.Value, this.dateBeginHMS4.Value, this.dateEndHMS4.Value, bOvertimeShift);
                if (num2 != 0)
                {
                    XMessageBox.Show(this, acc2.errDesc(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
        }

        private void dfrmShiftOtherTypeSet_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.dfrmShiftOtherTypeSet_Load_Acc(sender, e);
            }
            else
            {
                string str;
                this.dateBeginHMS1.CustomFormat = "HH:mm";
                this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
                this.dateEndHMS1.CustomFormat = "HH:mm";
                this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS1.Value = DateTime.Parse("00:00:00");
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
                this.dateBeginHMS4.CustomFormat = "HH:mm";
                this.dateBeginHMS4.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS4.Value = DateTime.Parse("00:00:00");
                this.dateEndHMS4.CustomFormat = "HH:mm";
                this.dateEndHMS4.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS4.Value = DateTime.Parse("00:00:00");
                this.cbof_ShiftID.Items.Clear();
                for (int i = 1; i <= 0x63; i++)
                {
                    this.cbof_ShiftID.Items.Add(i);
                }
                this.cbof_Readtimes.Items.Clear();
                this.cbof_Readtimes.Items.Add(2);
                this.cbof_Readtimes.Items.Add(4);
                this.cbof_Readtimes.Items.Add(6);
                this.cbof_Readtimes.Items.Add(8);
                if (this.operateMode == "New")
                {
                    this.cbof_ShiftID.Enabled = true;
                    str = "SELECT f_ShiftID FROM t_b_ShiftSet  ORDER BY [f_ShiftID] ASC ";
                    using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command = new SqlCommand(str, connection))
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                int index = this.cbof_ShiftID.Items.IndexOf((int) reader[0]);
                                if (index >= 0)
                                {
                                    this.cbof_ShiftID.Items.RemoveAt(index);
                                }
                            }
                            reader.Close();
                        }
                    }
                    if (this.cbof_ShiftID.Items.Count == 0)
                    {
                        base.Close();
                    }
                    this.cbof_ShiftID.Text = this.cbof_ShiftID.Items[0].ToString();
                    this.curShiftID = int.Parse(this.cbof_ShiftID.Text);
                    this.cbof_Readtimes.Text = this.cbof_Readtimes.Items[0].ToString();
                }
                else
                {
                    this.cbof_ShiftID.Enabled = false;
                    this.cbof_ShiftID.Text = this.curShiftID.ToString();
                    str = " SELECT * FROM t_b_ShiftSet WHERE [f_ShiftID]= " + this.curShiftID.ToString();
                    using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                    {
                        using (SqlCommand command2 = new SqlCommand(str, connection2))
                        {
                            connection2.Open();
                            SqlDataReader reader2 = command2.ExecuteReader();
                            if (reader2.Read())
                            {
                                DateTime result = DateTime.Parse("2010-3-10 00:00:00");
                                if (DateTime.TryParse(reader2["f_OnDuty1"].ToString(), out result))
                                {
                                    this.dateBeginHMS1.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OnDuty2"].ToString(), out result))
                                {
                                    this.dateBeginHMS2.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OnDuty3"].ToString(), out result))
                                {
                                    this.dateBeginHMS3.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OnDuty4"].ToString(), out result))
                                {
                                    this.dateBeginHMS4.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OffDuty1"].ToString(), out result))
                                {
                                    this.dateEndHMS1.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OffDuty2"].ToString(), out result))
                                {
                                    this.dateEndHMS2.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OffDuty3"].ToString(), out result))
                                {
                                    this.dateEndHMS3.Value = result;
                                }
                                if (DateTime.TryParse(reader2["f_OffDuty4"].ToString(), out result))
                                {
                                    this.dateEndHMS4.Value = result;
                                }
                                this.cbof_Readtimes.Text = reader2["f_Readtimes"].ToString();
                                this.txtName.Text = wgTools.SetObjToStr(reader2["f_ShiftName"].ToString());
                                this.chkBOvertimeShift.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 1;
                                this.chkBOvertimeShift1.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                                this.chkBOvertimeShift2.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                                this.chkBOvertimeShift3.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                                this.chkBOvertimeShift4.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                            }
                            reader2.Close();
                        }
                    }
                }
                this.cbof_Readtimes_SelectedIndexChanged(null, null);
            }
        }

        private void dfrmShiftOtherTypeSet_Load_Acc(object sender, EventArgs e)
        {
            string str;
            this.dateBeginHMS1.CustomFormat = "HH:mm";
            this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
            this.dateEndHMS1.CustomFormat = "HH:mm";
            this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS1.Value = DateTime.Parse("00:00:00");
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
            this.dateBeginHMS4.CustomFormat = "HH:mm";
            this.dateBeginHMS4.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS4.Value = DateTime.Parse("00:00:00");
            this.dateEndHMS4.CustomFormat = "HH:mm";
            this.dateEndHMS4.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS4.Value = DateTime.Parse("00:00:00");
            this.cbof_ShiftID.Items.Clear();
            for (int i = 1; i <= 0x63; i++)
            {
                this.cbof_ShiftID.Items.Add(i);
            }
            this.cbof_Readtimes.Items.Clear();
            this.cbof_Readtimes.Items.Add(2);
            this.cbof_Readtimes.Items.Add(4);
            this.cbof_Readtimes.Items.Add(6);
            this.cbof_Readtimes.Items.Add(8);
            if (this.operateMode == "New")
            {
                this.cbof_ShiftID.Enabled = true;
                str = "SELECT f_ShiftID FROM t_b_ShiftSet  ORDER BY [f_ShiftID] ASC ";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int index = this.cbof_ShiftID.Items.IndexOf((int) reader[0]);
                            if (index >= 0)
                            {
                                this.cbof_ShiftID.Items.RemoveAt(index);
                            }
                        }
                        reader.Close();
                    }
                }
                if (this.cbof_ShiftID.Items.Count == 0)
                {
                    base.Close();
                }
                this.cbof_ShiftID.Text = this.cbof_ShiftID.Items[0].ToString();
                this.curShiftID = int.Parse(this.cbof_ShiftID.Text);
                this.cbof_Readtimes.Text = this.cbof_Readtimes.Items[0].ToString();
            }
            else
            {
                this.cbof_ShiftID.Enabled = false;
                this.cbof_ShiftID.Text = this.curShiftID.ToString();
                str = " SELECT * FROM t_b_ShiftSet WHERE [f_ShiftID]= " + this.curShiftID.ToString();
                using (OleDbConnection connection2 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command2 = new OleDbCommand(str, connection2))
                    {
                        connection2.Open();
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        if (reader2.Read())
                        {
                            DateTime result = DateTime.Parse("2010-3-10 00:00:00");
                            if (DateTime.TryParse(reader2["f_OnDuty1"].ToString(), out result))
                            {
                                this.dateBeginHMS1.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OnDuty2"].ToString(), out result))
                            {
                                this.dateBeginHMS2.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OnDuty3"].ToString(), out result))
                            {
                                this.dateBeginHMS3.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OnDuty4"].ToString(), out result))
                            {
                                this.dateBeginHMS4.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OffDuty1"].ToString(), out result))
                            {
                                this.dateEndHMS1.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OffDuty2"].ToString(), out result))
                            {
                                this.dateEndHMS2.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OffDuty3"].ToString(), out result))
                            {
                                this.dateEndHMS3.Value = result;
                            }
                            if (DateTime.TryParse(reader2["f_OffDuty4"].ToString(), out result))
                            {
                                this.dateEndHMS4.Value = result;
                            }
                            this.cbof_Readtimes.Text = reader2["f_Readtimes"].ToString();
                            this.txtName.Text = wgTools.SetObjToStr(reader2["f_ShiftName"].ToString());
                            this.chkBOvertimeShift.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 1;
                            this.chkBOvertimeShift1.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                            this.chkBOvertimeShift2.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                            this.chkBOvertimeShift3.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                            this.chkBOvertimeShift4.Checked = int.Parse(reader2["f_bOvertimeShift"].ToString()) == 2;
                        }
                        reader2.Close();
                    }
                }
            }
            this.cbof_Readtimes_SelectedIndexChanged(null, null);
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

