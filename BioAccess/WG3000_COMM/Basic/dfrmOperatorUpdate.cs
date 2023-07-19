namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmOperatorUpdate : frmBioAccess
    {
        public int operateMode;
        public int operatorID = -1;
        public string operatorName = "";

        public dfrmOperatorUpdate()
        {
            this.InitializeComponent();
        }

        private int AddOperator()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.AddOperator_Acc();
            }
            int num = -1;
            try
            {
                string cmdText = "";
                cmdText = " SELECT f_OperatorID FROM [t_s_Operator] ";
                cmdText = (cmdText + "WHERE [f_OperatorName]=" + wgTools.PrepareStr(this.txtName.Text)) + " AND NOT [f_OperatorID]=" + this.operatorID.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (command.ExecuteScalar() != null)
                        {
                            XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return num;
                        }
                        cmdText = "";
                        cmdText = " INSERT INTO [t_s_Operator] ";
                        using (SqlCommand command2 = new SqlCommand((((cmdText + "([f_OperatorName],  [f_Password])") + " Values(" + wgTools.PrepareStr(this.txtName.Text)) + " , " + wgTools.PrepareStr(this.txtPassword.Text.Trim())) + ")", connection))
                        {
                            command2.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
            catch
            {
            }
            return num;
        }

        private int AddOperator_Acc()
        {
            int num = -1;
            try
            {
                string cmdText = "";
                cmdText = " SELECT f_OperatorID FROM [t_s_Operator] ";
                cmdText = (cmdText + "WHERE [f_OperatorName]=" + wgTools.PrepareStr(this.txtName.Text)) + " AND NOT [f_OperatorID]=" + this.operatorID.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (command.ExecuteScalar() != null)
                        {
                            XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return num;
                        }
                        cmdText = "";
                        cmdText = " INSERT INTO [t_s_Operator] ";
                        using (OleDbCommand command2 = new OleDbCommand((((cmdText + "([f_OperatorName],  [f_Password])") + " Values(" + wgTools.PrepareStr(this.txtName.Text)) + " , " + wgTools.PrepareStr(this.txtPassword.Text.Trim())) + ")", connection))
                        {
                            command2.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
            catch
            {
            }
            return num;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                XMessageBox.Show(this, CommonStr.strPersonNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.txtPassword.Text.Trim() != this.txtConfirmedPassword.Text.Trim())
            {
                XMessageBox.Show(this, CommonStr.strPasswordNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if ((this.operateMode == 1) || (this.operateMode == 2))
            {
                if (this.EditOperator() >= 0)
                {
                    operatorName = txtName.Text;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            else if (this.AddOperator() >= 0)
            {
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void dfrmOperatorUpdate_Load(object sender, EventArgs e)
        {
            if (this.operateMode == 2)
            {
                this.txtName.ReadOnly = true;
                this.txtName.TabStop = false;
            }
            this.txtName.Text = this.operatorName;
        }

        private int EditOperator()
        {
            if (wgAppConfig.IsAccessDB)
            {
                return this.EditOperator_Acc();
            }
            int num = -1;
            try
            {
                string cmdText = "";
                cmdText = " SELECT f_OperatorID FROM [t_s_Operator] ";
                cmdText = (cmdText + "WHERE [f_OperatorName]=" + wgTools.PrepareStr(this.txtName.Text)) + " AND  NOT [f_OperatorID]=" + this.operatorID.ToString();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (command.ExecuteScalar() != null)
                        {
                            XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return num;
                        }
                        cmdText = "";
                        cmdText = " UPDATE [t_s_Operator] ";
                        using (SqlCommand command2 = new SqlCommand(((cmdText + "SET [f_OperatorName]=" + wgTools.PrepareStr(this.txtName.Text)) + " , [f_Password]= " + wgTools.PrepareStr(this.txtPassword.Text.Trim())) + " WHERE [f_OperatorID]=" + this.operatorID.ToString(), connection))
                        {
                            command2.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
            catch
            {
            }
            return num;
        }

        private int EditOperator_Acc()
        {
            int num = -1;
            try
            {
                string cmdText = "";
                cmdText = " SELECT f_OperatorID FROM [t_s_Operator] ";
                cmdText = (cmdText + "WHERE [f_OperatorName]=" + wgTools.PrepareStr(this.txtName.Text)) + " AND NOT [f_OperatorID]=" + this.operatorID.ToString();
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        connection.Open();
                        if (command.ExecuteScalar() != null)
                        {
                            XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return num;
                        }
                        cmdText = "";
                        cmdText = " UPDATE [t_s_Operator] ";
                        using (OleDbCommand command2 = new OleDbCommand(((cmdText + "SET [f_OperatorName]=" + wgTools.PrepareStr(this.txtName.Text)) + " , [f_Password]= " + wgTools.PrepareStr(this.txtPassword.Text.Trim())) + " WHERE [f_OperatorID]=" + this.operatorID.ToString(), connection))
                        {
                            command2.ExecuteNonQuery();
                            return 1;
                        }
                    }
                }
            }
            catch
            {
            }
            return num;
        }
    }
}

