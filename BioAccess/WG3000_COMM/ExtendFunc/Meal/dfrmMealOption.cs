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

    public partial class dfrmMealOption : frmBioAccess
    {
        private DataSet ds = new DataSet("dsMeal");
        private DataTable dt;
        private DataView dv;
        private DataView dvSelected;
        public int mealNo = -1;
        private string strMealCon = "";

        public dfrmMealOption()
        {
            this.InitializeComponent();
        }

        private void btnAddAllReaders_Click(object sender, EventArgs e)
        {
            this.dgvOptional.SelectAll();
            wgAppConfig.selectObject(this.dgvOptional, "f_cost", this.nudCost.Value.ToString());
            this.dgvOptional.ClearSelection();
        }

        private void btnAddOneReader_Click(object sender, EventArgs e)
        {
            wgAppConfig.selectObject(this.dgvOptional, "f_cost", this.nudCost.Value.ToString());
        }

        private void btnDeleteAllReaders_Click(object sender, EventArgs e)
        {
            this.dgvSelected.SelectAll();
            wgAppConfig.deselectObject(this.dgvSelected);
        }

        private void btnDeleteOneReader_Click(object sender, EventArgs e)
        {
            wgAppConfig.deselectObject(this.dgvSelected);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.cmdOK_Click_Acc(sender, e);
        }

        private void cmdOK_Click_Acc(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Cursor current = Cursor.Current;
                wgAppConfig.runUpdateSql(string.Format(" Update t_d_Reader4Meal SET {0} = -1", this.strMealCon));
                if (this.dvSelected.Count > 0)
                {
                    for (int i = 0; i <= (this.dvSelected.Count - 1); i++)
                    {
                        wgAppConfig.runUpdateSql(string.Format(" Update t_d_Reader4Meal SET {0} = {1} WHERE f_ReaderID ={2}", this.strMealCon, this.dvSelected[i]["f_Cost"].ToString(), this.dvSelected[i]["f_ReaderID"].ToString()));
                    }
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

        private void dfrmMealOption_Load(object sender, EventArgs e)
        {
            switch (this.mealNo)
            {
                case 0:
                    this.strMealCon = "f_CostMorning";
                    break;

                case 1:
                    this.strMealCon = "f_CostLunch";
                    break;

                case 2:
                    this.strMealCon = "f_CostEvening";
                    break;

                case 3:
                    this.strMealCon = "f_CostOther";
                    break;

                default:
                    return;
            }
            this.loadData();
            if ((this.dgvOptional.Rows.Count == 0) && (this.dgvSelected.Rows.Count == 0))
            {
                XMessageBox.Show(CommonStr.strMealPremote);
                base.Close();
            }
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
                    SqlCommand selectCommand = new SqlCommand(string.Format("Select t_d_Reader4Meal.f_ReaderID, f_ReaderName, CASE WHEN {0}  IS NULL  THEN 0 ELSE (CASE WHEN {0} >=0 THEN 1 ELSE 0 END ) END  as f_Selected,{0} as f_Cost from t_b_reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_Reader4Meal.f_ReaderID ", this.strMealCon), connection);
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
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        public void loadData_Acc()
        {
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                OleDbCommand selectCommand = new OleDbCommand(string.Format("Select t_d_Reader4Meal.f_ReaderID, f_ReaderName, IIF(ISNULL({0}),0, IIF({0} >=0,1,0)) as f_Selected,{0} as f_Cost from t_b_reader,t_d_Reader4Meal  , t_b_Controller WHERE ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID ) AND    t_b_reader.f_ReaderID  = t_d_Reader4Meal.f_ReaderID ", this.strMealCon), connection);
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
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }
    }
}

