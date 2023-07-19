namespace WG3000_COMM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public sealed class ExcelObject : IDisposable
    {
        private OleDbConnection con;
        private DataSet ds;
        private string excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES\"";
        private string filepath = string.Empty;

        private event EventHandler connectionStringChange;

        public event EventHandler ConnectionStringChanged
        {
            add
            {
                this.connectionStringChange += value;
            }
            remove
            {
                this.connectionStringChange -= value;
            }
        }

        private event ProgressWork Reading;

        public event ProgressWork ReadProgress
        {
            add
            {
                this.Reading += value;
            }
            remove
            {
                this.Reading -= value;
            }
        }

        public event ProgressWork WriteProgress
        {
            add
            {
                this.Writing += value;
            }
            remove
            {
                this.Writing -= value;
            }
        }

        private event ProgressWork Writing;

        public ExcelObject(string path)
        {
            this.filepath = path;
            this.onConnectionStringChanged();
        }

        public bool AddNewRow(DataRow dr)
        {
            using (OleDbCommand command = new OleDbCommand(this.GenerateInsertStatement(dr), this.Connection))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }

        public bool AddNewRow(DataGridViewRow dgvdr, DataGridView dgv)
        {
            using (OleDbCommand command = new OleDbCommand(this.GenerateInsertStatement(dgvdr, dgv), this.Connection))
            {
                command.ExecuteNonQuery();
            }
            return true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((this.con != null) && (this.con.State == ConnectionState.Open))
                {
                    this.con.Close();
                }
                if (this.con != null)
                {
                    this.con.Dispose();
                    this.con = null;
                }
                if (this.filepath != null)
                {
                    this.filepath = string.Empty;
                }
                if (this.ds != null)
                {
                    this.ds.Dispose();
                }
            }
        }

        public bool DropTable(string tablename)
        {
            try
            {
                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                    this.onWriteProgress(10f);
                }
                string format = "Drop Table [{0}]";
                using (OleDbCommand command = new OleDbCommand(string.Format(format, tablename), this.Connection))
                {
                    this.onWriteProgress(30f);
                    command.ExecuteNonQuery();
                    this.onWriteProgress(80f);
                }
                this.Connection.Close();
                this.onWriteProgress(100f);
                return true;
            }
            catch (Exception exception)
            {
                this.onWriteProgress(0f);
                XMessageBox.Show(exception.Message);
                return false;
            }
        }

        private string GenerateCreateTable(DataView dv)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            builder.AppendFormat("CREATE TABLE [{0}](", dv.Table.TableName);
            flag = true;
            foreach (DataColumn column in dv.Table.Columns)
            {
                if (!flag)
                {
                    builder.Append(",");
                }
                flag = false;
                if (column.DataType.ToString().IndexOf("System.Int") >= 0)
                {
                    builder.AppendFormat("{0} {1}", column.ColumnName.ToString(), "Int");
                }
                else
                {
                    builder.AppendFormat("{0} {1}", column.ColumnName.ToString(), column.DataType.ToString().Replace("System.", ""));
                }
            }
            builder.Append(")");
            return builder.ToString().Replace("\r\n", " ");
        }

        private string GenerateCreateTable(DataGridView dgv)
        {
            StringBuilder builder = new StringBuilder();
            string str = "";
            bool flag = true;
            builder.AppendFormat("CREATE TABLE [{0}](", "ExcelData");
            flag = true;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    str = column.HeaderText.ToString().Replace("[", "(").Replace("]", ")").Replace(".", " ").Replace("\r\n", " ");
                    if (!flag)
                    {
                        builder.Append(",");
                    }
                    flag = false;
                    if (column.ValueType.Name.ToString().IndexOf("Int") >= 0)
                    {
                        if (builder.ToString().IndexOf(string.Format("[{0}]", str)) >= 0)
                        {
                            builder.AppendFormat("[{0}] {1}", str + column.Index.ToString(), "Int");
                        }
                        else
                        {
                            builder.AppendFormat("[{0}] {1}", str, "Int");
                        }
                    }
                    else if (column.ValueType.Name.ToString().IndexOf("DateTime") >= 0)
                    {
                        if (builder.ToString().IndexOf(string.Format("[{0}]", str)) >= 0)
                        {
                            builder.AppendFormat("[{0}] {1}", str + column.Index.ToString(), "String");
                        }
                        else
                        {
                            builder.AppendFormat("[{0}] {1}", str, "String");
                        }
                    }
                    else if (builder.ToString().IndexOf(string.Format("[{0}]", str)) >= 0)
                    {
                        builder.AppendFormat("[{0}] {1}", str + column.Index.ToString(), column.ValueType.Name.ToString());
                    }
                    else
                    {
                        builder.AppendFormat("[{0}] {1}", str, column.ValueType.Name.ToString());
                    }
                }
            }
            builder.Append(")");
            return builder.ToString().Replace("\r\n", " ").Replace(".", " ");
        }

        private string GenerateCreateTable(string tableName, Dictionary<string, string> tableDefination)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            builder.AppendFormat("CREATE TABLE [{0}](", tableName);
            flag = true;
            foreach (KeyValuePair<string, string> pair in tableDefination)
            {
                if (!flag)
                {
                    builder.Append(",");
                }
                flag = false;
                builder.AppendFormat("{0} {1}", pair.Key, pair.Value);
            }
            builder.Append(")");
            return builder.ToString().Replace("\r\n", " ");
        }

        private string GenerateInsertStatement(DataRow dr)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            builder.AppendFormat("INSERT INTO [{0}](", dr.Table.TableName);
            foreach (DataColumn column in dr.Table.Columns)
            {
                if (!flag)
                {
                    builder.Append(",");
                }
                flag = false;
                builder.Append(column.Caption);
            }
            builder.Append(") VALUES(");
            flag = true;
            for (int i = 0; i <= (dr.Table.Columns.Count - 1); i++)
            {
                if (!object.ReferenceEquals(dr.Table.Columns[i].DataType, typeof(int)))
                {
                    builder.Append("'");
                    builder.Append(dr[i].ToString().Replace("'", "''"));
                    builder.Append("'");
                }
                else
                {
                    builder.Append(dr[i].ToString().Replace("'", "''"));
                }
                if (i != (dr.Table.Columns.Count - 1))
                {
                    builder.Append(",");
                }
            }
            builder.Append(")");
            return builder.ToString().Replace("\r\n", " ");
        }

        private string GenerateInsertStatement(DataGridViewRow dgvdr, DataGridView dgv)
        {
            StringBuilder builder = new StringBuilder();
            string str = "";
            bool flag = true;
            builder.AppendFormat("INSERT INTO [{0}](", "ExcelData");
            flag = true;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    str = column.HeaderText.ToString().Replace("[", "(").Replace("]", ")").Replace(".", " ").Replace("\r\n", " ");
                    if (!flag)
                    {
                        builder.Append(",");
                    }
                    flag = false;
                    if (builder.ToString().IndexOf(string.Format("[{0}]", str)) >= 0)
                    {
                        builder.AppendFormat(string.Format("[{0}]", str + column.Index.ToString()), new object[0]);
                    }
                    else
                    {
                        builder.AppendFormat(string.Format("[{0}]", str), new object[0]);
                    }
                }
            }
            builder.Append(") VALUES(");
            string str2 = builder.ToString().Replace("\r\n", " ").Replace(".", " ");
            builder = null;
            builder = new StringBuilder();
            builder.Append(str2);
            flag = true;
            for (int i = 0; i <= (dgv.Columns.Count - 1); i++)
            {
                if (dgv.Columns[i].Visible)
                {
                    if (!flag)
                    {
                        builder.Append(",");
                    }
                    flag = false;
                    if (dgvdr.Cells[i].Value == null)
                    {
                        builder.Append("NULL");
                    }
                    else if (dgvdr.Cells[i].Value == DBNull.Value)
                    {
                        builder.Append("NULL");
                    }
                    else if (dgvdr.Cells[i].Value.ToString().Trim() == "")
                    {
                        builder.Append("NULL");
                    }
                    else if (dgv.Columns[i].ValueType.Name.ToString().IndexOf("Int") < 0)
                    {
                        if (dgv.Columns[i].ValueType.Name.ToString().IndexOf("DateTime") >= 0)
                        {
                            builder.Append("'");
                            if (string.IsNullOrEmpty(dgv.Columns[i].DefaultCellStyle.Format))
                            {
                                builder.Append(dgvdr.Cells[i].Value.ToString().Replace("'", "''"));
                            }
                            else
                            {
                                DateTime time = (DateTime) dgvdr.Cells[i].Value;
                                builder.Append(time.ToString(dgv.Columns[i].DefaultCellStyle.Format).Replace("'", "''"));
                            }
                            builder.Append("'");
                        }
                        else
                        {
                            builder.Append("'");
                            builder.Append(dgvdr.Cells[i].Value.ToString().Replace("'", "''"));
                            builder.Append("'");
                        }
                    }
                    else
                    {
                        builder.Append(dgvdr.Cells[i].Value.ToString().Replace("'", "''"));
                    }
                }
            }
            builder.Append(")");
            return builder.ToString().Replace("\r\n", " ");
        }

        public DataTable GetSchema()
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }
            object[] restrictions = new object[4];
            restrictions[3] = "TABLE";
            return this.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
        }

        public void onConnectionStringChanged()
        {
            if ((this.Connection != null) && !this.Connection.ConnectionString.Equals(this.ConnectionString))
            {
                if (this.Connection.State == ConnectionState.Open)
                {
                    this.Connection.Close();
                }
                this.Connection.Dispose();
                this.con = null;
            }
            if (this.connectionStringChange != null)
            {
                this.connectionStringChange(this, new EventArgs());
            }
        }

        public void onReadProgress(float percentage)
        {
            if (this.Reading != null)
            {
                this.Reading(percentage);
            }
        }

        public void onWriteProgress(float percentage)
        {
            if (this.Writing != null)
            {
                this.Writing(percentage);
            }
        }

        public DataTable ReadTable(string tableName)
        {
            return this.ReadTable(tableName, "");
        }

        public DataTable ReadTable(string tableName, string criteria)
        {
            DataTable table;
            try
            {
                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                    this.onReadProgress(10f);
                }
                string format = "Select * from [{0}]";
                if (!string.IsNullOrEmpty(criteria))
                {
                    format = format + " Where " + criteria;
                }
                using (OleDbCommand command = new OleDbCommand(string.Format(format, tableName)))
                {
                    command.Connection = this.Connection;
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        this.onReadProgress(30f);
                        this.ds = new DataSet();
                        this.onReadProgress(50f);
                        adapter.Fill(this.ds, tableName);
                        this.onReadProgress(100f);
                        if (this.ds.Tables.Count == 1)
                        {
                            return this.ds.Tables[0];
                        }
                        table = null;
                    }
                }
            }
            catch
            {
                XMessageBox.Show("Table Cannot be read");
                table = null;
            }
            return table;
        }

        public bool test_WriteTable()
        {
            bool flag;
            try
            {
                string cmdText = "CREATE TABLE [users](f_ConsumerID Int,编号 CHAR(50),姓名 CHAR(100),卡号 Int,部门 String,考勤 Byte,倒班 Byte,门禁 Byte,起始日期 DateTime,截止日期 DateTime)";
                using (OleDbCommand command = new OleDbCommand(cmdText, this.Connection))
                {
                    if (this.Connection.State != ConnectionState.Open)
                    {
                        this.Connection.Open();
                    }
                    command.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
                flag = false;
            }
            return flag;
        }

        public bool WriteTable(DataView dv)
        {
            bool flag;
            try
            {
                using (OleDbCommand command = new OleDbCommand(this.GenerateCreateTable(dv), this.Connection))
                {
                    if (this.Connection.State != ConnectionState.Open)
                    {
                        this.Connection.Open();
                    }
                    command.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
                flag = false;
            }
            return flag;
        }

        public bool WriteTable(DataGridView dgv)
        {
            bool flag;
            try
            {
                using (OleDbCommand command = new OleDbCommand(this.GenerateCreateTable(dgv), this.Connection))
                {
                    if (this.Connection.State != ConnectionState.Open)
                    {
                        this.Connection.Open();
                    }
                    command.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
                flag = false;
            }
            return flag;
        }

        public bool WriteTable(string tableName, Dictionary<string, string> tableDefination)
        {
            bool flag;
            try
            {
                using (OleDbCommand command = new OleDbCommand(this.GenerateCreateTable(tableName, tableDefination), this.Connection))
                {
                    if (this.Connection.State != ConnectionState.Open)
                    {
                        this.Connection.Open();
                    }
                    command.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public OleDbConnection Connection
        {
            get
            {
                if (this.con == null)
                {
                    OleDbConnection connection = new OleDbConnection(this.ConnectionString);
                    this.con = connection;
                }
                return this.con;
            }
        }

        public string ConnectionString
        {
            get
            {
                if (this.filepath == string.Empty)
                {
                    return string.Empty;
                }
                FileInfo info = new FileInfo(this.filepath);
                if (!info.Extension.Equals(".xls") && info.Extension.Equals(".xlsx"))
                {
                    return string.Format(this.excelObject, new object[] { "Ace", "12.0", this.filepath, "12.0" });
                }
                return string.Format(this.excelObject, new object[] { "Jet", "4.0", this.filepath, "8.0" });
            }
        }

        public delegate void ProgressWork(float percentage);
    }
}

