namespace WG3000_COMM.Basic
{
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

    public partial class dfrmSwipeRecordsFindOption : frmBioAccess
    {
        private int[] arrAddr;
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();

        public dfrmSwipeRecordsFindOption()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Hide();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (base.Owner != null)
            {
                (base.Owner as frmSwipeRecords).btnQuery_Click(null, null);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (this.chkListDoors.Items.Count > 0)
            {
                for (int i = 0; i < this.chkListDoors.Items.Count; i++)
                {
                    this.chkListDoors.SetItemChecked(i, true);
                }
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            if (this.chkListDoors.Items.Count > 0)
            {
                for (int i = 0; i < this.chkListDoors.Items.Count; i++)
                {
                    this.chkListDoors.SetItemChecked(i, false);
                }
            }
        }

        private void cbof_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            if (this.dvDoors != null)
            {
                this.chkListDoors.Items.Clear();
                DataView dvDoors = this.dvDoors;
                if ((this.cboZone.SelectedIndex < 0) || ((this.cboZone.SelectedIndex == 0) && (((int) this.arrZoneID[0]) == 0)))
                {
                    dvDoors.RowFilter = "";
                    str = "";
                }
                else
                {
                    dvDoors.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
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
                            dvDoors.RowFilter = string.Format(" f_ZoneID ={0:d} ", num2);
                            str = string.Format(" f_ZoneID ={0:d} ", num2);
                        }
                        else
                        {
                            dvDoors.RowFilter = "";
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
                            dvDoors.RowFilter = string.Format("  {0} ", str2);
                            str = string.Format("  {0} ", str2);
                        }
                    }
                    dvDoors.RowFilter = string.Format(" {0} ", str);
                }
                this.chkListDoors.Items.Clear();
                if (this.dvDoors.Count > 0)
                {
                    for (int j = 0; j < this.dvDoors.Count; j++)
                    {
                        this.arrAddr[j] = (int) this.dvDoors[j]["f_ReaderID"];
                        this.chkListDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[j]["f_ReaderName"]));
                    }
                }
            }
            else
            {
                this.chkListDoors.Items.Clear();
            }
        }

        private void dfrmSwipeRecordsFindOption_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                    }
                    this.dfrmFind1.setObjtoFind(this.chkListDoors, this);
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void dfrmSwipeRecordsFindOption_Load(object sender, EventArgs e)
        {
            this.loadZoneInfo();
            this.loadDoorData();
        }

        public string getStrSql()
        {
            string str = "-1";
            if (this.chkListDoors.CheckedItems.Count != 0)
            {
                for (int i = 0; i < this.chkListDoors.Items.Count; i++)
                {
                    if (this.chkListDoors.GetItemChecked(i))
                    {
                        str = str + "," + this.arrAddr[i].ToString();
                    }
                }
            }
            return str;
        }

        private void loadDoorData()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadDoorData_Acc();
            }
            else
            {
                string cmdText = " SELECT c.*,b.f_ControllerSN ,  b.f_ZoneID ";
                cmdText = cmdText + " FROM t_b_Controller b, t_b_reader c WHERE c.f_ControllerID = b.f_ControllerID " + " ORDER BY  c.f_ReaderID ";
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            this.dt = new DataTable();
                            this.dvDoors = new DataView(this.dt);
                            this.dvDoors4Watching = new DataView(this.dt);
                            adapter.Fill(this.dt);
                            new icControllerZone().getAllowedControllers(ref this.dt);
                            if (this.dvDoors.Count > 0)
                            {
                                this.arrAddr = new int[this.dvDoors.Count + 1];
                                for (int i = 0; i < this.dvDoors.Count; i++)
                                {
                                    string item = wgTools.SetObjToStr(this.dvDoors[i]["f_ReaderName"]);
                                    this.chkListDoors.Items.Add(item);
                                    this.arrAddr[i] = (int) this.dvDoors[i]["f_ReaderID"];
                                }
                            }
                        }
                    }
                }
            }
        }

        private void loadDoorData_Acc()
        {
            string cmdText = " SELECT c.*,b.f_ControllerSN ,  b.f_ZoneID ";
            cmdText = cmdText + " FROM t_b_Controller b, t_b_reader c WHERE c.f_ControllerID = b.f_ControllerID " + " ORDER BY  c.f_ReaderID ";
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        this.dt = new DataTable();
                        this.dvDoors = new DataView(this.dt);
                        this.dvDoors4Watching = new DataView(this.dt);
                        adapter.Fill(this.dt);
                        new icControllerZone().getAllowedControllers(ref this.dt);
                        if (this.dvDoors.Count > 0)
                        {
                            this.arrAddr = new int[this.dvDoors.Count + 1];
                            for (int i = 0; i < this.dvDoors.Count; i++)
                            {
                                string item = wgTools.SetObjToStr(this.dvDoors[i]["f_ReaderName"]);
                                this.chkListDoors.Items.Add(item);
                                this.arrAddr[i] = (int) this.dvDoors[i]["f_ReaderID"];
                            }
                        }
                    }
                }
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
            bool flag = true;
            this.label25.Visible = flag;
            this.cboZone.Visible = flag;
        }
    }
}

