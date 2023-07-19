namespace WG3000_COMM.Basic
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmControllers : frmBioAccess
    {
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();

        public frmControllers()
        {
            this.InitializeComponent();
        }

        private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvControllers.SelectedRows.Count > 0)
            {
                using (dfrmControllerZoneSelect select = new dfrmControllerZoneSelect())
                {
                    string str = "";
                    for (int i = 0; i < this.dgvControllers.SelectedRows.Count; i++)
                    {
                        int index = this.dgvControllers.SelectedRows[i].Index;
                        int num2 = int.Parse(this.dgvControllers.Rows[index].Cells[0].Value.ToString());
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str + ",";
                        }
                        str = str + num2.ToString();
                    }
                    select.Text = string.Format("{0}: [{1}]", sender.ToString(), this.dgvControllers.SelectedRows.Count.ToString());
                    if (select.ShowDialog(this) == DialogResult.OK)
                    {
                        wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_Controller SET f_ZoneID= {0} WHERE  f_ControllerID IN ({1}) ", select.selectZoneId, str));
                        this.loadControllerData();
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmController controller = new dfrmController())
            {
                int count = this.dv.Table.Rows.Count;
                int num2 = this.dv.Count;
                controller.ShowDialog(this);
                this.loadControllerData();
                this.cboZone_SelectedIndexChanged(null, null);
                if ((count != this.dv.Table.Rows.Count) && ((num2 + 1) != this.dv.Count))
                {
                    this.loadZoneInfo();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index;
            if (this.dgvControllers.SelectedRows.Count <= 1)
            {
                index = this.dgvControllers.SelectedRows[0].Index;
                if (XMessageBox.Show(this, CommonStr.strDelete + " " + this.dgvControllers[1, index].Value.ToString() + ":" + this.dgvControllers[2, index].Value.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                {
                    return;
                }
            }
            else if (XMessageBox.Show(this, CommonStr.strDeleteSelected + this.dgvControllers.SelectedRows.Count.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
            {
                return;
            }
            int firstDisplayedScrollingRowIndex = this.dgvControllers.FirstDisplayedScrollingRowIndex;
            string str = "";
            if (this.dgvControllers.SelectedRows.Count <= 1)
            {
                index = this.dgvControllers.SelectedRows[0].Index;
                icController.DeleteControllerFromDB(int.Parse(this.dgvControllers[0, index].Value.ToString()));
                str = str + "(" + this.dgvControllers[1, index].Value.ToString() + ")" + this.dgvControllers[2, index].Value.ToString();
            }
            else
            {
                foreach (DataGridViewRow row in this.dgvControllers.SelectedRows)
                {
                    str = str + "(" + row.Cells[1].Value.ToString() + ")" + row.Cells[2].Value.ToString() + ",";
                    icController.DeleteControllerFromDB(int.Parse(row.Cells[0].Value.ToString()));
                }
            }
            wgAppConfig.wgLog(CommonStr.strDelete + CommonStr.strController + ":" + str);
            this.loadControllerData();
            this.cboZone_SelectedIndexChanged(null, null);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvControllers.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (this.dgvControllers.Rows.Count > 0)
                {
                    rowIndex = this.dgvControllers.CurrentCell.RowIndex;
                }
                int rowCount = this.dgvControllers.RowCount;
                using (dfrmController controller = new dfrmController())
                {
                    controller.OperateNew = false;
                    controller.ControllerID = int.Parse(this.dgvControllers.Rows[rowIndex].Cells[0].Value.ToString());
                    controller.ShowDialog(this);
                    this.loadControllerData();
                    if (controller.bEditZone)
                    {
                        this.loadZoneInfo();
                    }
                    this.cboZone_SelectedIndexChanged(null, null);
                }
                if ((this.dgvControllers.RowCount == 0) || (rowCount != this.dgvControllers.RowCount))
                {
                    this.loadZoneInfo();
                }
                else if (this.dgvControllers.RowCount > 0)
                {
                    if (this.dgvControllers.RowCount > rowIndex)
                    {
                        this.dgvControllers.CurrentCell = this.dgvControllers[1, rowIndex];
                    }
                    else
                    {
                        this.dgvControllers.CurrentCell = this.dgvControllers[1, this.dgvControllers.RowCount - 1];
                    }
                }
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            wgAppConfig.exportToExcel(this.dgvControllers, this.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            wgAppConfig.printdgv(this.dgvControllers, this.Text);
        }

        private void btnSearchController_Click(object sender, EventArgs e)
        {
            using (dfrmNetControllerConfig config = new dfrmNetControllerConfig())
            {
                int count = this.dv.Table.Rows.Count;
                int num1 = this.dv.Count;
                config.ShowDialog(this);
                this.loadControllerData();
                this.cboZone_SelectedIndexChanged(null, null);
                if (count != this.dv.Table.Rows.Count)
                {
                    this.loadZoneInfo();
                }
            }
        }

        private void cboZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            try
            {
                if (this.dv != null)
                {
                    if ((this.cboZone.SelectedIndex < 0) || ((this.cboZone.SelectedIndex == 0) && (((int) this.arrZoneID[0]) == 0)))
                    {
                        this.dv.RowFilter = "";
                        str = "";
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dv.Count.ToString());
                    }
                    else
                    {
                        this.dv.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
                        str = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
                        int num = 0;
                        int num2 = 0;
                        int num3 = 0;
                        num2 = (int) this.arrZoneID[this.cboZone.SelectedIndex];
                        num = (int) this.arrZoneNO[this.cboZone.SelectedIndex];
                        num3 = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
                        if (num > 0)
                        {
                            if (num >= num3)
                            {
                                this.dv.RowFilter = string.Format(" f_ZoneID ={0:d} ", num2);
                                str = string.Format(" f_ZoneID ={0:d} ", num2);
                            }
                            else
                            {
                                this.dv.RowFilter = "";
                                string str2 = "";
                                for (int i = 0; i < this.arrZoneNO.Count; i++)
                                {
                                    if ((((int) this.arrZoneNO[i]) <= num3) && (((int) this.arrZoneNO[i]) >= num))
                                    {
                                        if (str2 == "")
                                        {
                                            str2 = str2 + string.Format(" f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                        }
                                        else
                                        {
                                            str2 = str2 + string.Format(" OR f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                        }
                                    }
                                }
                                this.dv.RowFilter = string.Format("  {0} ", str2);
                                str = string.Format("  {0} ", str2);
                            }
                        }
                        this.dv.RowFilter = string.Format(" {0} ", str);
                        wgTools.WriteLine("foreach (ListViewItem itm in listViewNotDisplay.Items)");
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.dv.Count.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void dgvControllers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.btnEdit.Enabled)
            {
                this.btnEdit.PerformClick();
            }
        }

        private void frmControllers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.dfrmFind1 != null)
            {
                this.dfrmFind1.ReallyCloseForm();
            }
        }

        public void frmControllers_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.funcCtrlShiftQ();
            }
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
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void frmControllers_Load(object sender, EventArgs e)
        {
            this.loadZoneInfo();
            this.loadOperatorPrivilege();
            this.loadControllerData();
            this.dgvControllers.ContextMenuStrip = this.contextMenuStrip1;
        }

        private void funcCtrlShiftQ()
        {
            try
            {
                string strNewName;
                string str2;
                uint num;
                using (dfrmInputNewName name = new dfrmInputNewName())
                {
                    name.Text = CommonStr.strControllerBeginNo;
                    name.label1.Text = CommonStr.strControllerSN;
                    name.strNewName = "";
                    if ((name.ShowDialog(this) != DialogResult.OK) || !uint.TryParse(name.strNewName, out num))
                    {
                        return;
                    }
                    strNewName = name.strNewName;
                }
                using (dfrmInputNewName name2 = new dfrmInputNewName())
                {
                    name2.Text = CommonStr.strControllerEndNo;
                    name2.label1.Text = CommonStr.strControllerSN;
                    name2.strNewName = "";
                    if (name2.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!uint.TryParse(name2.strNewName, out num))
                        {
                            str2 = strNewName;
                        }
                        else
                        {
                            str2 = name2.strNewName;
                        }
                    }
                    else
                    {
                        str2 = strNewName;
                    }
                }
                if (Information.IsNumeric(strNewName) && Information.IsNumeric(str2))
                {
                    if (((int.Parse(strNewName) <= int.Parse(str2)) && (wgMjController.GetControllerType(int.Parse(strNewName)) >= 0)) && (wgMjController.GetControllerType(int.Parse(str2)) >= 0))
                    {
                        using (dfrmController controller = new dfrmController())
                        {
                            controller.Show();
                            for (long i = int.Parse(strNewName); i <= int.Parse(str2); i += 1L)
                            {
                                controller.Text = i.ToString();
                                controller.mtxtbControllerSN.Text = i.ToString();
                                controller.mtxtbControllerNO.Text = ((((int) (i / 0x5f5e100L)) * 0x2710) + (i % 0x2710L)).ToString();
                                controller.btnNext.PerformClick();
                                controller.btnOK_Click(null, null);
                                Application.DoEvents();
                            }
                            goto Label_01E4;
                        }
                    }
                    XMessageBox.Show(CommonStr.strSNWrong);
                }
                else
                {
                    XMessageBox.Show(CommonStr.strSNWrong);
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        Label_01E4:
            this.loadControllerData();
        }

        private void loadControllerData()
        {
            string str;
            icControllerZone zone;
            this.dtController = new DataTable();
            this.dv = new DataView(this.dtController);
            if (wgAppConfig.IsAccessDB)
            {
                str = " SELECT f_ControllerID, f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_ZoneName, f_Note, f_DoorNames,  t_b_Controller.f_ZoneID ";
                str = str + " FROM t_b_Controller LEFT OUTER JOIN t_b_Controller_Zone ON t_b_Controller.f_ZoneID = t_b_Controller_Zone.f_ZoneID" + "  ORDER BY [f_ControllerNO]";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dtController);
                        }
                    }
                    goto Label_0101;
                }
            }
            str = " SELECT f_ControllerID, f_ControllerNO, f_ControllerSN, f_Enabled, f_IP, f_PORT, f_ZoneName, f_Note, f_DoorNames,  t_b_Controller.f_ZoneID ";
            str = str + " FROM t_b_Controller LEFT OUTER JOIN t_b_Controller_Zone ON t_b_Controller.f_ZoneID = t_b_Controller_Zone.f_ZoneID" + "  ORDER BY f_ControllerNO ";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dtController);
                    }
                }
            }
        Label_0101:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dtController);
            this.dgvControllers.AutoGenerateColumns = false;
            this.dgvControllers.DataSource = this.dv;
            for (int i = 0; i < (this.dv.Table.Columns.Count - 1); i++)
            {
                this.dgvControllers.Columns[i].DataPropertyName = this.dv.Table.Columns[i].ColumnName;
            }
            this.dgvControllers.Columns[6].Visible = true;
            if (this.dv.Count > 0)
            {
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
            }
            else
            {
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
            }
            wgAppRunInfo.raiseAppRunInfoLoadNums(this.dv.Count.ToString());
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuControllers";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnEdit.Visible = false;
                this.btnDelete.Visible = false;
                this.btnSearchController.Visible = false;
            }
        }

        private void loadZoneInfo()
        {
            new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
            int count = this.arrZoneID.Count;
            this.cboZone.Items.Clear();
            for (count = 0; count < this.arrZoneID.Count; count++)
            {
                if ((count == 0) && string.IsNullOrEmpty(this.arrZoneName[count].ToString()))
                {
                    this.cboZone.Items.Add(CommonStr.strAllZones);
                }
                else
                {
                    this.cboZone.Items.Add(this.arrZoneName[count].ToString());
                }
            }
            if (this.cboZone.Items.Count > 0)
            {
                this.cboZone.SelectedIndex = 0;
            }
            this.cboZone.Visible = true;
        }
    }
}

