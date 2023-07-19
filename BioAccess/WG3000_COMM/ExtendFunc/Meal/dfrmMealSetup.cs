namespace WG3000_COMM.ExtendFunc.Meal
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

    public partial class dfrmMealSetup : frmBioAccess
    {
        private DataSet ds = new DataSet("dsMeal");
        private DataTable dt;
        private DataView dv;
        private DataView dvSelected;

        public dfrmMealSetup()
        {
            this.InitializeComponent();
        }

        private void btnAddAllReaders_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 1;
                }
                this.dt.AcceptChanges();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnAddOneReader_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvOptional);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnDeleteAllReaders_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dt.Rows.Count; i++)
                {
                    this.dt.Rows[i]["f_Selected"] = 0;
                }
                this.dt.AcceptChanges();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnDeleteOneReader_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelected);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string str5;
                string strB = "";
                string str2 = "";
                string str3 = "";
                string str4 = "";
                if (this.chkMorningMeal.Checked)
                {
                    strB = this.dateBeginHMS1.Value.ToString("HH:mm");
                    str2 = this.dateEndHMS1.Value.ToString("HH:mm");
                    str3 = this.dateBeginHMS1.Value.ToString("HH:mm");
                    str4 = this.dateEndHMS1.Value.ToString("HH:mm");
                    if (this.dateBeginHMS1.Value > this.dateEndHMS1.Value)
                    {
                        XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS1.Value.ToString("HH:mm") + "\r\n\r\n" + this.dateEndHMS1.Value.ToString("HH:mm"));
                        return;
                    }
                }
                if (this.chkLunchMeal.Checked)
                {
                    if (this.dateBeginHMS2.Value > this.dateEndHMS2.Value)
                    {
                        XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS2.Value.ToString("HH:mm") + "\r\n\r\n" + this.dateEndHMS2.Value.ToString("HH:mm"));
                        return;
                    }
                    if ((str4 != "") && (string.Compare(this.dateBeginHMS2.Value.ToString("HH:mm"), str4) < 0))
                    {
                        XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS2.Value.ToString("HH:mm") + "\r\n\r\n" + str4);
                        return;
                    }
                    if (strB == "")
                    {
                        strB = this.dateBeginHMS2.Value.ToString("HH:mm");
                    }
                    if (str2 == "")
                    {
                        str2 = this.dateEndHMS2.Value.ToString("HH:mm");
                    }
                    str3 = this.dateBeginHMS2.Value.ToString("HH:mm");
                    str4 = this.dateEndHMS2.Value.ToString("HH:mm");
                }
                if (this.chkEveningMeal.Checked)
                {
                    if (this.dateBeginHMS3.Value > this.dateEndHMS3.Value)
                    {
                        XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS3.Value.ToString("HH:mm") + "\r\n\r\n" + this.dateEndHMS3.Value.ToString("HH:mm"));
                        return;
                    }
                    if ((str4 != "") && (string.Compare(this.dateBeginHMS3.Value.ToString("HH:mm"), str4) < 0))
                    {
                        XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS3.Value.ToString("HH:mm") + "\r\n\r\n" + str4);
                        return;
                    }
                    if (strB == "")
                    {
                        strB = this.dateBeginHMS3.Value.ToString("HH:mm");
                    }
                    if (str2 == "")
                    {
                        str2 = this.dateEndHMS3.Value.ToString("HH:mm");
                    }
                    str3 = this.dateBeginHMS3.Value.ToString("HH:mm");
                    str4 = this.dateEndHMS3.Value.ToString("HH:mm");
                }
                if (this.chkOtherMeal.Checked && (strB != ""))
                {
                    if (string.Compare(this.dateBeginHMS4.Value.ToString("HH:mm"), str4) > 0)
                    {
                        if ((string.Compare(this.dateEndHMS4.Value.ToString("HH:mm"), strB) >= 0) && (this.dateEndHMS4.Value < this.dateBeginHMS4.Value))
                        {
                            XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateEndHMS4.Value.ToString("HH:mm") + "\r\n\r\n" + strB);
                            return;
                        }
                    }
                    else
                    {
                        if (string.Compare(this.dateBeginHMS4.Value.ToString("HH:mm"), strB) >= 0)
                        {
                            if (string.Compare(this.dateBeginHMS4.Value.ToString("HH:mm"), str3) >= 0)
                            {
                                XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS4.Value.ToString("HH:mm") + "\r\n\r\n" + str4);
                            }
                            else
                            {
                                XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateBeginHMS4.Value.ToString("HH:mm") + "\r\n\r\n" + strB);
                            }
                            return;
                        }
                        if (string.Compare(this.dateEndHMS4.Value.ToString("HH:mm"), strB) >= 0)
                        {
                            XMessageBox.Show(CommonStr.strWrongTimeSegment + "\r\n\r\n" + this.dateEndHMS4.Value.ToString("HH:mm") + "\r\n\r\n" + strB);
                            return;
                        }
                    }
                    str3 = this.dateBeginHMS4.Value.ToString("HH:mm");
                    str4 = this.dateEndHMS4.Value.ToString("HH:mm");
                }
                Cursor current = Cursor.Current;
                if (this.dvSelected.Count > 0)
                {
                    string str6 = "";
                    for (int i = 0; i <= (this.dvSelected.Count - 1); i++)
                    {
                        if (str6 == "")
                        {
                            str6 = str6 + this.dvSelected[i]["f_ReaderID"];
                        }
                        else
                        {
                            str6 = str6 + "," + this.dvSelected[i]["f_ReaderID"];
                        }
                    }
                    wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_d_Reader4Meal WHERE f_ReaderID NOT IN ({0})", str6));
                    wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_d_Reader4Meal (f_ReaderID) SELECT f_ReaderID from t_b_Reader WHERE f_ReaderID  IN ({0}) AND f_ReaderID NOT IN (SELECT f_ReaderID From t_d_Reader4Meal)  ", str6));
                }
                else
                {
                    str5 = " DELETE FROM t_d_Reader4Meal ";
                    wgAppConfig.runUpdateSql(str5);
                }
                int num3 = 60;
                int num2 = 0;
                if (this.radioButton1.Checked)
                {
                    num2 = 0;
                }
                if (this.radioButton2.Checked)
                {
                    num2 = 1;
                }
                if (this.radioButton3.Checked)
                {
                    num2 = 2;
                    num3 = (int) this.nudRuleSeconds.Value;
                }
                wgAppConfig.runUpdateSql(string.Concat(new object[] { "UPDATE t_b_MealSetup SET f_Value = ", num2, ", f_ParamVal=", num3, " WHERE f_ID=1 " }));
                str5 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=2 ";
                if (this.chkMorningMeal.Checked)
                {
                    str5 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
                    str5 = (((str5 + ",f_BeginHMS =" + wgTools.PrepareStr(this.dateBeginHMS1.Value, true, "HH:mm")) + ",f_EndHMS =" + wgTools.PrepareStr(this.dateEndHMS1.Value, true, "HH:mm")) + ", f_ParamVal= " + this.nudMorning.Value) + " WHERE f_ID=2 ";
                }
                wgAppConfig.runUpdateSql(str5);
                str5 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=3 ";
                if (this.chkLunchMeal.Checked)
                {
                    str5 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
                    str5 = (((str5 + ",f_BeginHMS =" + wgTools.PrepareStr(this.dateBeginHMS2.Value, true, "HH:mm")) + ",f_EndHMS =" + wgTools.PrepareStr(this.dateEndHMS2.Value, true, "HH:mm")) + ", f_ParamVal= " + this.nudLunch.Value) + " WHERE f_ID=3 ";
                }
                wgAppConfig.runUpdateSql(str5);
                str5 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=4 ";
                if (this.chkEveningMeal.Checked)
                {
                    str5 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
                    str5 = (((str5 + ",f_BeginHMS =" + wgTools.PrepareStr(this.dateBeginHMS3.Value, true, "HH:mm")) + ",f_EndHMS =" + wgTools.PrepareStr(this.dateEndHMS3.Value, true, "HH:mm")) + ", f_ParamVal= " + this.nudEvening.Value) + " WHERE f_ID=4 ";
                }
                wgAppConfig.runUpdateSql(str5);
                str5 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=5 ";
                if (this.chkOtherMeal.Checked)
                {
                    str5 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
                    str5 = (((str5 + ",f_BeginHMS =" + wgTools.PrepareStr(this.dateBeginHMS4.Value, true, "HH:mm")) + ",f_EndHMS =" + wgTools.PrepareStr(this.dateEndHMS4.Value, true, "HH:mm")) + ", f_ParamVal= " + this.nudOther.Value) + " WHERE f_ID=5 ";
                }
                wgAppConfig.runUpdateSql(str5);
                if (wgAppConfig.IsAccessDB)
                {
                    OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                    OleDbCommand command = new OleDbCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=6 ", connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                    bool flag = true;
                    if (reader.HasRows)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                    reader.Close();
                    connection.Close();
                    if (flag)
                    {
                        str5 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(6,0,NULL,NULL,0) ";
                        wgAppConfig.runUpdateSql(str5);
                    }
                    str5 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=6 ";
                    if (this.chkAllowableSwipe.Checked)
                    {
                        str5 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=6 ";
                    }
                    wgAppConfig.runUpdateSql(str5);
                }
                else
                {
                    SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString);
                    SqlCommand command2 = new SqlCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=6 ", connection2);
                    connection2.Open();
                    SqlDataReader reader2 = command2.ExecuteReader(CommandBehavior.Default);
                    bool flag2 = true;
                    if (reader2.HasRows)
                    {
                        flag2 = false;
                    }
                    else
                    {
                        flag2 = true;
                    }
                    reader2.Close();
                    connection2.Close();
                    if (flag2)
                    {
                        str5 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(6,0,NULL,NULL,0) ";
                        wgAppConfig.runUpdateSql(str5);
                    }
                    str5 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=6 ";
                    if (this.chkAllowableSwipe.Checked)
                    {
                        str5 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=6 ";
                    }
                    wgAppConfig.runUpdateSql(str5);
                }
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void btnOption0_Click(object sender, EventArgs e)
        {
            using (dfrmMealOption option = new dfrmMealOption())
            {
                option.mealNo = 0;
                option.Text = option.Text + "--" + this.chkMorningMeal.Text;
                option.ShowDialog();
            }
        }

        private void btnOption1_Click(object sender, EventArgs e)
        {
            using (dfrmMealOption option = new dfrmMealOption())
            {
                option.mealNo = 1;
                option.Text = option.Text + "--" + this.chkLunchMeal.Text;
                option.ShowDialog();
            }
        }

        private void btnOption2_Click(object sender, EventArgs e)
        {
            using (dfrmMealOption option = new dfrmMealOption())
            {
                option.mealNo = 2;
                option.Text = option.Text + "--" + this.chkEveningMeal.Text;
                option.ShowDialog();
            }
        }

        private void btnOption3_Click(object sender, EventArgs e)
        {
            using (dfrmMealOption option = new dfrmMealOption())
            {
                option.mealNo = 3;
                option.Text = option.Text + "--" + this.chkOtherMeal.Text;
                option.ShowDialog();
            }
        }

        private void chkMeal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.dateBeginHMS1.Visible = this.chkMorningMeal.Checked;
                this.dateEndHMS1.Visible = this.chkMorningMeal.Checked;
                this.nudMorning.Visible = this.chkMorningMeal.Checked;
                this.lblMorning.Visible = this.chkMorningMeal.Checked;
                this.btnOption0.Visible = this.chkMorningMeal.Checked;
                this.dateBeginHMS2.Visible = this.chkLunchMeal.Checked;
                this.dateEndHMS2.Visible = this.chkLunchMeal.Checked;
                this.nudLunch.Visible = this.chkLunchMeal.Checked;
                this.lblLunch.Visible = this.chkLunchMeal.Checked;
                this.btnOption1.Visible = this.chkLunchMeal.Checked;
                this.dateBeginHMS3.Visible = this.chkEveningMeal.Checked;
                this.dateEndHMS3.Visible = this.chkEveningMeal.Checked;
                this.nudEvening.Visible = this.chkEveningMeal.Checked;
                this.lblEvening.Visible = this.chkEveningMeal.Checked;
                this.btnOption2.Visible = this.chkEveningMeal.Checked;
                this.dateBeginHMS4.Visible = this.chkOtherMeal.Checked;
                this.dateEndHMS4.Visible = this.chkOtherMeal.Checked;
                this.nudOther.Visible = this.chkOtherMeal.Checked;
                this.lblOther.Visible = this.chkOtherMeal.Checked;
                this.btnOption3.Visible = this.chkOtherMeal.Checked;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmMealSetup_Load(object sender, EventArgs e)
        {
            this.dateBeginHMS1.CustomFormat = "HH:mm";
            this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS1.Value = DateTime.Parse("04:00:00");
            this.dateEndHMS1.CustomFormat = "HH:mm";
            this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS1.Value = DateTime.Parse("9:59:59");
            this.dateBeginHMS2.CustomFormat = "HH:mm";
            this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS2.Value = DateTime.Parse("10:00:00");
            this.dateEndHMS2.CustomFormat = "HH:mm";
            this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS2.Value = DateTime.Parse("15:59:59");
            this.dateBeginHMS3.CustomFormat = "HH:mm";
            this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS3.Value = DateTime.Parse("16:00:00");
            this.dateEndHMS3.CustomFormat = "HH:mm";
            this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS3.Value = DateTime.Parse("21:59:59");
            this.dateBeginHMS4.CustomFormat = "HH:mm";
            this.dateBeginHMS4.Format = DateTimePickerFormat.Custom;
            this.dateBeginHMS4.Value = DateTime.Parse("22:00:00");
            this.dateEndHMS4.CustomFormat = "HH:mm";
            this.dateEndHMS4.Format = DateTimePickerFormat.Custom;
            this.dateEndHMS4.Value = DateTime.Parse("23:59:59");
            this.loadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void loadData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadData_Acc();
            }
            else
            {
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    SqlCommand selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID NOT IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(this.ds, "optionalReader");
                    selectCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID  IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", connection);
                    new SqlDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                    this.dv = new DataView(this.ds.Tables["optionalReader"]);
                    this.dv.RowFilter = " f_Selected = 0";
                    this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
                    this.dvSelected.RowFilter = " f_Selected = 1";
                    this.dt = this.ds.Tables["optionalReader"];
                    try
                    {
                        this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
                    {
                        this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                        this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                    }
                    this.dv.RowFilter = "f_Selected = 0";
                    this.dvSelected.RowFilter = "f_Selected > 0";
                    this.dgvOptional.AutoGenerateColumns = false;
                    this.dgvOptional.DataSource = this.dv;
                    this.dgvSelected.AutoGenerateColumns = false;
                    this.dgvSelected.DataSource = this.dvSelected;
                    this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
                    this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
                    selectCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID=1 ";
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        if (int.Parse(reader["f_Value"].ToString()) == 1)
                        {
                            this.radioButton2.Checked = true;
                        }
                        else
                        {
                            if (int.Parse(reader["f_Value"].ToString()) == 2)
                            {
                                this.radioButton3.Checked = true;
                                try
                                {
                                    this.nudRuleSeconds.Value = (int) decimal.Parse(reader["f_ParamVal"].ToString());
                                    goto Label_02D0;
                                }
                                catch (Exception exception)
                                {
                                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                                    goto Label_02D0;
                                }
                            }
                            this.radioButton1.Checked = true;
                        }
                    }
                Label_02D0:
                    reader.Close();
                    selectCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    reader = selectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        if (int.Parse(reader["f_ID"].ToString()) == 2)
                        {
                            if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                            {
                                this.chkMorningMeal.Checked = true;
                                this.dateBeginHMS1.Value = (DateTime) reader["f_BeginHMS"];
                                this.dateEndHMS1.Value = (DateTime) reader["f_EndHMS"];
                                this.nudMorning.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                            }
                            else
                            {
                                this.chkMorningMeal.Checked = false;
                            }
                            this.dateBeginHMS1.Visible = this.chkMorningMeal.Checked;
                            this.dateEndHMS1.Visible = this.chkMorningMeal.Checked;
                            this.nudMorning.Visible = this.chkMorningMeal.Checked;
                        }
                        else
                        {
                            if (int.Parse(reader["f_ID"].ToString()) == 3)
                            {
                                if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                                {
                                    this.chkLunchMeal.Checked = true;
                                    this.dateBeginHMS2.Value = (DateTime) reader["f_BeginHMS"];
                                    this.dateEndHMS2.Value = (DateTime) reader["f_EndHMS"];
                                    this.nudLunch.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                                }
                                else
                                {
                                    this.chkLunchMeal.Checked = false;
                                }
                                this.dateBeginHMS2.Visible = this.chkLunchMeal.Checked;
                                this.dateEndHMS2.Visible = this.chkLunchMeal.Checked;
                                this.nudLunch.Visible = this.chkLunchMeal.Checked;
                                continue;
                            }
                            if (int.Parse(reader["f_ID"].ToString()) == 4)
                            {
                                if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                                {
                                    this.chkEveningMeal.Checked = true;
                                    this.dateBeginHMS3.Value = (DateTime) reader["f_BeginHMS"];
                                    this.dateEndHMS3.Value = (DateTime) reader["f_EndHMS"];
                                    this.nudEvening.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                                }
                                else
                                {
                                    this.chkEveningMeal.Checked = false;
                                }
                                this.dateBeginHMS3.Visible = this.chkEveningMeal.Checked;
                                this.dateEndHMS3.Visible = this.chkEveningMeal.Checked;
                                this.nudEvening.Visible = this.chkEveningMeal.Checked;
                                continue;
                            }
                            if (int.Parse(reader["f_ID"].ToString()) == 5)
                            {
                                if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                                {
                                    this.chkOtherMeal.Checked = true;
                                    this.dateBeginHMS4.Value = (DateTime) reader["f_BeginHMS"];
                                    this.dateEndHMS4.Value = (DateTime) reader["f_EndHMS"];
                                    this.nudOther.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                                }
                                else
                                {
                                    this.chkOtherMeal.Checked = false;
                                }
                                this.dateBeginHMS4.Visible = this.chkOtherMeal.Checked;
                                this.dateEndHMS4.Visible = this.chkOtherMeal.Checked;
                                this.nudOther.Visible = this.chkOtherMeal.Checked;
                                continue;
                            }
                            if (int.Parse(reader["f_ID"].ToString()) == 6)
                            {
                                if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                                {
                                    this.chkAllowableSwipe.Checked = true;
                                    continue;
                                }
                                this.chkAllowableSwipe.Checked = false;
                            }
                        }
                    }
                    reader.Close();
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
            }
        }

        public void loadData_Acc()
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                OleDbCommand selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID NOT IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                selectCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID  IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", connection);
                new OleDbDataAdapter(selectCommand).Fill(this.ds, "optionalReader");
                this.dv = new DataView(this.ds.Tables["optionalReader"]);
                this.dv.RowFilter = " f_Selected = 0";
                this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
                this.dvSelected.RowFilter = " f_Selected = 1";
                this.dt = this.ds.Tables["optionalReader"];
                try
                {
                    this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
                }
                catch (Exception)
                {
                    throw;
                }
                for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
                {
                    this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                    this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
                }
                this.dv.RowFilter = "f_Selected = 0";
                this.dvSelected.RowFilter = "f_Selected > 0";
                this.dgvOptional.AutoGenerateColumns = false;
                this.dgvOptional.DataSource = this.dv;
                this.dgvSelected.AutoGenerateColumns = false;
                this.dgvSelected.DataSource = this.dvSelected;
                this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
                this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
                selectCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID=1 ";
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                OleDbDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    if (int.Parse(reader["f_Value"].ToString()) == 1)
                    {
                        this.radioButton2.Checked = true;
                    }
                    else
                    {
                        if (int.Parse(reader["f_Value"].ToString()) == 2)
                        {
                            this.radioButton3.Checked = true;
                            try
                            {
                                this.nudRuleSeconds.Value = (int) decimal.Parse(reader["f_ParamVal"].ToString());
                                goto Label_02C2;
                            }
                            catch (Exception exception)
                            {
                                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                                goto Label_02C2;
                            }
                        }
                        this.radioButton1.Checked = true;
                    }
                }
            Label_02C2:
                reader.Close();
                selectCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (int.Parse(reader["f_ID"].ToString()) == 2)
                    {
                        if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                        {
                            this.chkMorningMeal.Checked = true;
                            this.dateBeginHMS1.Value = (DateTime) reader["f_BeginHMS"];
                            this.dateEndHMS1.Value = (DateTime) reader["f_EndHMS"];
                            this.nudMorning.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                        }
                        else
                        {
                            this.chkMorningMeal.Checked = false;
                        }
                        this.dateBeginHMS1.Visible = this.chkMorningMeal.Checked;
                        this.dateEndHMS1.Visible = this.chkMorningMeal.Checked;
                        this.nudMorning.Visible = this.chkMorningMeal.Checked;
                    }
                    else
                    {
                        if (int.Parse(reader["f_ID"].ToString()) == 3)
                        {
                            if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                            {
                                this.chkLunchMeal.Checked = true;
                                this.dateBeginHMS2.Value = (DateTime) reader["f_BeginHMS"];
                                this.dateEndHMS2.Value = (DateTime) reader["f_EndHMS"];
                                this.nudLunch.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                            }
                            else
                            {
                                this.chkLunchMeal.Checked = false;
                            }
                            this.dateBeginHMS2.Visible = this.chkLunchMeal.Checked;
                            this.dateEndHMS2.Visible = this.chkLunchMeal.Checked;
                            this.nudLunch.Visible = this.chkLunchMeal.Checked;
                            continue;
                        }
                        if (int.Parse(reader["f_ID"].ToString()) == 4)
                        {
                            if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                            {
                                this.chkEveningMeal.Checked = true;
                                this.dateBeginHMS3.Value = (DateTime) reader["f_BeginHMS"];
                                this.dateEndHMS3.Value = (DateTime) reader["f_EndHMS"];
                                this.nudEvening.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                            }
                            else
                            {
                                this.chkEveningMeal.Checked = false;
                            }
                            this.dateBeginHMS3.Visible = this.chkEveningMeal.Checked;
                            this.dateEndHMS3.Visible = this.chkEveningMeal.Checked;
                            this.nudEvening.Visible = this.chkEveningMeal.Checked;
                            continue;
                        }
                        if (int.Parse(reader["f_ID"].ToString()) == 5)
                        {
                            if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                            {
                                this.chkOtherMeal.Checked = true;
                                this.dateBeginHMS4.Value = (DateTime) reader["f_BeginHMS"];
                                this.dateEndHMS4.Value = (DateTime) reader["f_EndHMS"];
                                this.nudOther.Value = decimal.Parse(wgTools.SetObjToStr(reader["f_ParamVal"]));
                            }
                            else
                            {
                                this.chkOtherMeal.Checked = false;
                            }
                            this.dateBeginHMS4.Visible = this.chkOtherMeal.Checked;
                            this.dateEndHMS4.Visible = this.chkOtherMeal.Checked;
                            this.nudOther.Visible = this.chkOtherMeal.Checked;
                            continue;
                        }
                        if (int.Parse(reader["f_ID"].ToString()) == 6)
                        {
                            if (int.Parse(wgTools.SetObjToStr(reader["f_Value"])) > 0)
                            {
                                this.chkAllowableSwipe.Checked = true;
                                continue;
                            }
                            this.chkAllowableSwipe.Checked = false;
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception exception2)
            {
                wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
            }
        }
    }
}

