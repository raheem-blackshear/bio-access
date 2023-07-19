namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmOperatePrivilege : frmBioAccess
    {
        public int operatorID = -1;
        private static DataGridViewCellStyle styleYellow = new DataGridViewCellStyle();

        public dfrmOperatePrivilege()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnFullControlAllOn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
            {
                if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_FullControl"))
                {
                    for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
                    {
                        this.dgvOperatePrivilege[i, j].Value = true;
                    }
                }
            }
        }

        private void btnFullControlOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
            {
                if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_FullControl"))
                {
                    for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
                    {
                        this.dgvOperatePrivilege[i, j].Value = false;
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (icOperator.setOperatorPrivilege(this.operatorID, (this.dgvOperatePrivilege.DataSource as DataView).Table) > 0)
            {
                base.Close();
            }
        }

        private void btnReadAllOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
            {
                if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_ReadOnly"))
                {
                    for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
                    {
                        this.dgvOperatePrivilege[i, j].Value = false;
                    }
                }
            }
        }

        private void btnReadAllOn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvOperatePrivilege.Columns.Count; i++)
            {
                if (this.dgvOperatePrivilege.Columns[i].Name.Equals("f_ReadOnly"))
                {
                    for (int j = 0; j < this.dgvOperatePrivilege.Rows.Count; j++)
                    {
                        this.dgvOperatePrivilege[i, j].Value = true;
                    }
                }
            }
        }

        private void dfrmOperatePrivilege_Load(object sender, EventArgs e)
        {
            this.dgvOperatePrivilege.AutoGenerateColumns = false;
            this.tb = new DataTable();
            this.tb.TableName = "OperatePrivilege";
            this.tb.Columns.Add("f_FunctionID");
            this.tb.Columns.Add("f_FunctionName");
            this.tb.Columns.Add("f_FunctionDisplayName");
            this.tb.Columns.Add("f_ReadOnly");
            this.tb.Columns.Add("f_FullControl");
            this.tb.Columns.Add("f_DisplayID");
            DataRow dr = this.tb.NewRow();
            this.updateDr(ref dr, 1, "frmControllers", "Controllers", false, true);
            this.tb.Rows.Add(dr);
            this.tb.AcceptChanges();
            this.dgvOperatePrivilege.DataSource = this.tb;
            this.dgvOperatePrivilege.Columns[0].DataPropertyName = "f_FunctionID";
            this.dgvOperatePrivilege.Columns[1].DataPropertyName = "f_FunctionName";
            this.dgvOperatePrivilege.Columns[2].DataPropertyName = "f_FunctionDisplayName";
            this.dgvOperatePrivilege.Columns[3].DataPropertyName = "f_ReadOnly";
            this.dgvOperatePrivilege.Columns[4].DataPropertyName = "f_FullControl";
            this.dgvOperatePrivilege.Columns[5].DataPropertyName = "f_DisplayID";
            string info = "";
            DataTable table = icOperator.getOperatorPrivilege(this.operatorID);
            if (table != null)
            {
                table.Columns.Add("f_DisplayID");
                string[] strArray = new string[] { 
                    "100", "mnu1BasicConfigure", "110", "mnuControllers", "120", "mnuGroups", "130", "mnuConsumers", "131", /*"mnuCardLost", "200",*/ "mnu1DoorControl", "210", "mnuControlSeg", "220", "mnuPrivilege", 
                    "230", "mnuPeripheral", "240", "mnuPasswordManagement", "250", "mnuAntiBack", "260", "mnuInterLock", "270", "mnuMoreCards", "280", "mnuFirstCard", "300", "mnu1BasicOperate", "310", "mnuTotalControl", 
                    "311", "mnuCheckController", "312", "mnuAdjustTime", "313", "mnuUpload", "317", "mnuMonitor", "314", "mnuGetCardRecords", "316", "TotalControl_RemoteOpen", "320", "mnuCardRecords", "400", "mnu1Attendence", 
                    "410", "mnuShiftNormalConfigure", "420", "mnuShiftRule", "430", "mnuShiftSet", "440", "mnuShiftArrange", "450", "mnuHolidaySet", "460", "mnuLeave", "470", "mnuManualCardRecord", "480", "mnuAttendenceData", 
                    "500", "mnu1Tool", "510", "cmdChangePasswor", "520", "cmdOperatorManage", "530", "mnuDBBackup", "540", "mnuExtendedFunction", "550", "mnuOption", "560", "mnuTaskList", "570", "mnuLogQuery", 
                    "600", "mnu1Help", "610", "mnuAbout", "620", "mnuManual", "630", "mnuSystemCharacteristic", "318", "btnMaps", "580", "btnZoneManage", "315", "mnuRealtimeGetRecords", "581", "mnuPatrolDetailData", 
                    "582", "mnuConstMeal", "583", "mnuMeeting", "584", "mnuElevator", "", ""
                 };
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i]["f_FunctionDisplayName"] = CommonStr.ResourceManager.GetString("strFunctionDisplayName_" + table.Rows[i]["f_FunctionName"].ToString());
                    table.Rows[i]["f_FunctionDisplayName"] = wgAppConfig.ReplaceFloorRomm(table.Rows[i]["f_FunctionDisplayName"] as string);
                    for (int j = 0; j < strArray.Length; j += 2)
                    {
                        if (strArray[j + 1] == table.Rows[i]["f_FunctionName"].ToString())
                        {
                            table.Rows[i]["f_DisplayID"] = strArray[j];
                        }
                    }
                }
                this.dgvOperatePrivilege.DataSource = table;
                this.dv = new DataView(table);
                this.dv.Sort = "f_DisplayID";
                this.dgvOperatePrivilege.DataSource = this.dv;
            }
            wgTools.WgDebugWrite(info, new object[0]);
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = Color.Yellow;
            style.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 0x86);
            style.ForeColor = Color.Blue;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            styleYellow = style;
        }

        private void dgvOperatePrivilege_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewCell cell = this.dgvOperatePrivilege[1, e.RowIndex];
            if (e.Value != null)
            {
                ArrayList list2 = new ArrayList();
                list2.Add("mnu1BasicConfigure");
                list2.Add("mnu1DoorControl");
                list2.Add("mnu1BasicOperate");
                list2.Add("mnu1Attendence");
                list2.Add("mnu1Tool");
                list2.Add("mnu1Help");
                ArrayList list = list2;
                if (list.IndexOf(cell.Value.ToString()) >= 0)
                {
                    DataGridViewRow row = this.dgvOperatePrivilege.Rows[e.RowIndex];
                    row.DefaultCellStyle = styleYellow;
                }
            }
        }

        private void updateDr(ref DataRow dr, int id, string name, string display, bool read, bool fullControl)
        {
            dr[0] = id;
            dr[1] = name;
            dr[2] = display;
            dr[3] = read;
            dr[4] = fullControl;
        }
    }
}

