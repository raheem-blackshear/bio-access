namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmEditUserFile : frmBioAccess
    {
        public dfrmEditUserFile()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (this.dfrmFind1 == null)
            {
                this.dfrmFind1 = new dfrmFind();
            }
            this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName;
                this.openFileDialog1.Filter = " (*.xml)|*.xml| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                this.openFileDialog1.Title = (sender as Button).Text;
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                }
                else
                {
                    return;
                }
                string path = fileName;
                if (File.Exists(path))
                {
                    if (this.tb != null)
                    {
                        this.tb.Dispose();
                    }
                    this.tb = new DataTable();
                    this.tb.TableName = wgAppConfig.dbWEBUserName;
                    this.tb.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
                    this.tb.Columns.Add("f_ConsumerName");
                    this.tb.ReadXml(path);
                    this.tb.AcceptChanges();
                    if (this.dv != null)
                    {
                        this.dv.Dispose();
                    }
                    this.dv = new DataView(this.tb);
                    this.dv.Sort = "f_CardNO ASC";
                    this.dataGridView1.AutoGenerateColumns = false;
                    this.dataGridView1.DataSource = this.dv;
                    for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dataGridView1.ColumnCount); i++)
                    {
                        this.dataGridView1.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                    }
                    this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
                    return;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            XMessageBox.Show((sender as Button).Text + " " + CommonStr.strFailed);
        }

        private void btnLoadFromDB_Click(object sender, EventArgs e)
        {
            try
            {
                string cmdText = (string.Format(" SELECT  f_CardNO, f_ConsumerName   ", new object[0]) + " FROM t_b_Consumer ") + " WHERE f_CardNO > 0 " + " ORDER BY f_CardNO ASC ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            command.CommandTimeout = wgAppConfig.dbCommandTimeout;
                            this.tb = new DataTable(wgAppConfig.dbWEBUserName);
                            adapter.Fill(this.tb);
                            this.dv = new DataView(this.tb);
                            this.dv.Sort = "f_CardNO ASC";
                            this.dataGridView1.AutoGenerateColumns = false;
                            this.dataGridView1.DataSource = this.dv;
                            for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dataGridView1.ColumnCount); i++)
                            {
                                this.dataGridView1.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                            }
                        }
                    }
                }
                this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataView view = new DataView((this.dataGridView1.DataSource as DataView).Table))
                {
                    view.Sort = "f_CardNO ASC";
                    if (view.Count > 0)
                    {
                        uint num = 0;
                        string valA = null;
                        for (int i = 0; i < view.Count; i++)
                        {
                            if (view[i]["f_CardNO"] == null)
                            {
                                XMessageBox.Show(CommonStr.strCheckCard);
                                return;
                            }
                            if (string.IsNullOrEmpty(view[i]["f_CardNO"].ToString()))
                            {
                                XMessageBox.Show(CommonStr.strCheckCard);
                                return;
                            }
                            if (uint.Parse(view[i]["f_CardNO"].ToString()) == 0)
                            {
                                XMessageBox.Show(CommonStr.strCheckCard);
                                return;
                            }
                            if (num != uint.Parse(view[i]["f_CardNO"].ToString()))
                            {
                                num = uint.Parse(view[i]["f_CardNO"].ToString());
                                valA = wgTools.SetObjToStr(view[i]["f_ConsumerName"]);
                            }
                            else
                            {
                                XMessageBox.Show(string.Format("{0}:{1}\r\n{2}\r\n{3}", new object[] { CommonStr.strCheckCard, num.ToString(), wgTools.SetObjToStr(valA), wgTools.SetObjToStr(view[i]["f_ConsumerName"]) }));
                                return;
                            }
                        }
                    }
                }
                string path = wgAppConfig.Path4Doc() + wgAppConfig.dbWEBUserName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                using (StringWriter writer = new StringWriter())
                {
                    (this.dataGridView1.DataSource as DataView).Table.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                    using (StreamWriter writer2 = new StreamWriter(path, false))
                    {
                        writer2.Write(writer.ToString());
                    }
                }
                XMessageBox.Show((sender as Button).Text + "\r\n\r\n" + path);
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmEditUserFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        private void dfrmSystemParam_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
                {
                    this.dataGridView1.Columns[i].Visible = true;
                }
                (this.dataGridView1.DataSource as DataView).RowFilter = "";
            }
            if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
            {
                if (this.dfrmFind1 == null)
                {
                    this.dfrmFind1 = new dfrmFind();
                }
                this.dfrmFind1.setObjtoFind(base.ActiveControl, this);
            }
        }

        private void dfrmSystemParam_Load(object sender, EventArgs e)
        {
            this.btnLoadFromDB.Visible = !string.IsNullOrEmpty(wgAppConfig.dbConString);
            try
            {
                if (this.tb != null)
                {
                    this.tb.Dispose();
                }
                this.tb = new DataTable();
                this.tb.TableName = wgAppConfig.dbWEBUserName;
                this.tb.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));
                this.tb.Columns.Add("f_ConsumerName");
                this.tb.AcceptChanges();
                this.dv = new DataView(this.tb);
                this.dv.Sort = "f_CardNO ASC";
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = this.dv;
                for (int i = 0; (i < this.dv.Table.Columns.Count) && (i < this.dataGridView1.ColumnCount); i++)
                {
                    this.dataGridView1.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
        }
    }
}

