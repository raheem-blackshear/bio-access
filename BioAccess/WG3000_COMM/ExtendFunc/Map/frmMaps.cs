namespace WG3000_COMM.ExtendFunc.Map
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmMaps : frmBioAccess
    {
        private ArrayList arrZoomScale = new ArrayList();
        private ArrayList arrZoomScaleTabpageName = new ArrayList();
        private bool bEditing;
        private SqlConnection cn;
        private ucMapDoor currentUcMapDoor;
        private SqlDataAdapter da;
        private DataSet dstemp;
        private DataTable dt;
        private DataTable dtReader;
        private DataView dvDoors;
        private DataView dvDoors4Watching;
        private DataView dvMap;
        private DataView dvMapDoor;
        private DataView dvMapDoors;
        private DataView dvSelected;
        private ImageList imgDoor2;
        private int l;
        private Point lastMouseP;
        public ListView lstDoors = new ListView();
        private TabPage mapPage;
        private Panel mapPanel;
        private PictureBox mapPicture;
        private byte[] photoImageData;
        private MemoryStream photoMemoryStream;
        private Dictionary<string, string> ReaderName = new Dictionary<string, string>();
        private string strcmdWatchAllMapsBk = "";
        private string strcmdWatchCurrentMapBk = "";
        private int t;
        private ucMapDoor uc1door;

        public frmMaps()
        {
            this.InitializeComponent();
        }

        private void btnStopOthers_Click(object sender, EventArgs e)
        {
            if (this.btnStop != null)
            {
                this.btnStop.PerformClick();
            }
            this.cmdWatchCurrentMap.Text = this.strcmdWatchCurrentMapBk;
            this.cmdWatchAllMaps.Text = this.strcmdWatchAllMapsBk;
            this.cmdWatchCurrentMap.BackColor = Color.Transparent;
            this.cmdWatchAllMaps.BackColor = Color.Transparent;
            this.btnStopOthers.BackColor = Color.Transparent;
        }

        private void cmdAddDoor_Click(object sender, EventArgs e)
        {
            object lastMouseP = null;
            float result = 1f;
            float.TryParse(wgAppConfig.getSystemParamByNO(0x16), out result);
            try
            {
                if (sender == this.cmdAddDoorByLoc)
                {
                    lastMouseP = this.lastMouseP;
                }
                this.dvMapDoor = new DataView(this.dvDoors.Table);
                if ((this.dvMapDoor.Count > 0) && (this.c1tabMaps.TabPages.Count > 0))
                {
                    using (dfrmSelectMapDoor door = new dfrmSelectMapDoor())
                    {
                        int num3;
                        for (num3 = 0; num3 <= (this.c1tabMaps.TabPages.Count - 1); num3++)
                        {
                            foreach (object obj3 in this.c1tabMaps.TabPages[num3].Controls)
                            {
                                if (obj3 is Panel)
                                {
                                    foreach (object obj4 in ((Panel) obj3).Controls)
                                    {
                                        if (obj4 is PictureBox)
                                        {
                                            foreach (ucMapDoor door2 in ((PictureBox) obj4).Controls)
                                            {
                                                door.lstMappedDoors.Items.Add(door2.doorName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i <= (this.dvMapDoor.Count - 1); i++)
                        {
                            if (door.lstMappedDoors.FindString(this.dvMapDoor[i]["f_DoorName"].ToString()) == -1)
                            {
                                door.lstUnMappedDoors.Items.Add(this.dvMapDoor[i]["f_DoorName"]);
                            }
                        }
                        if (door.ShowDialog(this) == DialogResult.OK)
                        {
                            string doorName = door.doorName;
                            if (!door.bAddDoor)
                            {
                                for (num3 = 0; num3 <= (this.c1tabMaps.TabPages.Count - 1); num3++)
                                {
                                    foreach (object obj5 in this.c1tabMaps.TabPages[num3].Controls)
                                    {
                                        if (obj5 is Panel)
                                        {
                                            foreach (object obj6 in ((Panel) obj5).Controls)
                                            {
                                                if (obj6 is PictureBox)
                                                {
                                                    foreach (ucMapDoor door3 in ((PictureBox) obj6).Controls)
                                                    {
                                                        if (door3.doorName == doorName)
                                                        {
                                                            door3.Dispose();
                                                            ((PictureBox) obj6).Controls.Remove(door3);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            this.uc1door = new ucMapDoor();
                            this.uc1door.doorName = doorName;
                            this.uc1door.doorScale = result;
                            int index = this.arrZoomScaleTabpageName.IndexOf(this.c1tabMaps.SelectedTab.Text);
                            if (index < 0)
                            {
                                this.uc1door.mapScale = 1f;
                            }
                            else
                            {
                                this.uc1door.mapScale = (float) this.arrZoomScale[index];
                            }
                            this.uc1door.MouseDown += new MouseEventHandler(this.UcMapDoor_MouseDown);
                            this.uc1door.MouseMove += new MouseEventHandler(this.UcMapDoor_MouseMove);
                            this.uc1door.MouseUp += new MouseEventHandler(this.UcMapDoor_MouseUp);
                            this.uc1door.Click += new EventHandler(this.ucMapDoor_Click);
                            this.uc1door.imgDoor = this.imgDoor2;
                            this.uc1door.picDoorState.ContextMenuStrip = this.C1CmnuDoor;
                            this.uc1door.ContextMenuStrip = this.contextMenuStrip1Doors;
                            foreach (object obj7 in this.c1tabMaps.SelectedTab.Controls)
                            {
                                if (obj7 is Panel)
                                {
                                    foreach (object obj8 in ((Panel) obj7).Controls)
                                    {
                                        if (obj8 is PictureBox)
                                        {
                                            this.uc1door.bindSource = (PictureBox) obj8;
                                            ((PictureBox) obj8).Controls.Add(this.uc1door);
                                            if (sender == this.cmdAddDoorByLoc)
                                            {
                                                this.uc1door.Location = this.lastMouseP;
                                            }
                                            else
                                            {
                                                this.uc1door.Location = new Point(-((PictureBox) obj8).Location.X, -((PictureBox) obj8).Location.Y);
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdAddMap_Click(object sender, EventArgs e)
        {
            try
            {
                using (dfrmMapInfo info = new dfrmMapInfo())
                {
                    if (info.ShowDialog(this) == DialogResult.OK)
                    {
                        string mapName = info.mapName;
                        string mapFile = info.mapFile;
                        for (int i = 0; i <= (this.c1tabMaps.TabPages.Count - 1); i++)
                        {
                            if (this.c1tabMaps.TabPages[i].Text == mapName)
                            {
                                XMessageBox.Show(CommonStr.strMapNameDuplicated);
                                return;
                            }
                        }
                        this.mapPage = new TabPage();
                        this.mapPage.Text = mapName;
                        this.mapPage.Tag = mapFile;
                        this.c1tabMaps.TabPages.Add(this.mapPage);
                        this.c1tabMaps.SelectedTab = this.mapPage;
                        this.mapPanel = new Panel();
                        this.mapPicture = new PictureBox();
                        this.mapPanel.Dock = DockStyle.Fill;
                        this.mapPanel.BackColor = Color.White;
                        this.mapPanel.AutoScroll = true;
                        this.mapPicture.SizeMode = PictureBoxSizeMode.AutoSize;
                        this.ShowMap(mapFile, this.mapPicture);
                        this.mapPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                        this.mapPicture.ContextMenuStrip = this.C1CmnuMap;
                        this.mapPanel.Controls.Add(this.mapPicture);
                        this.mapPage.Controls.Add(this.mapPanel);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdCancelAndExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.c1tabMaps.Visible = false;
                this.C1ToolBar4MapEdit.Visible = false;
                this.bEditing = false;
                this.C1ToolBar4MapOperate.Visible = true;
                this.cmdAddDoorByLoc.Visible = false;
                this.loadmapFromDB();
                this.Timer2.Enabled = true;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdChangeMapName_Click(object sender, EventArgs e)
        {
            try
            {
                using (dfrmMapInfo info = new dfrmMapInfo())
                {
                    info.txtMapName.Text = this.c1tabMaps.SelectedTab.Text;
                    info.txtMapFileName.Text = this.c1tabMaps.SelectedTab.Tag.ToString();
                    if (info.ShowDialog(this) == DialogResult.OK)
                    {
                        string mapName = info.mapName;
                        string mapFile = info.mapFile;
                        for (int i = 0; i <= (this.c1tabMaps.TabPages.Count - 1); i++)
                        {
                            if ((this.c1tabMaps.TabPages[i].Text == mapName) && (i != this.c1tabMaps.SelectedTab.TabIndex))
                            {
                                XMessageBox.Show(CommonStr.strMapNameDuplicated4Edit);
                                return;
                            }
                        }
                        TabPage selectedTab = this.c1tabMaps.SelectedTab;
                        selectedTab.Text = mapName;
                        selectedTab.Tag = mapFile;
                        foreach (object obj2 in selectedTab.Controls)
                        {
                            if (obj2 is Panel)
                            {
                                foreach (object obj3 in ((Panel) obj2).Controls)
                                {
                                    if (obj3 is PictureBox)
                                    {
                                        ((PictureBox) obj3).SizeMode = PictureBoxSizeMode.AutoSize;
                                        this.ShowMap(mapFile, (PictureBox) obj3);
                                        ((PictureBox) obj3).SizeMode = PictureBoxSizeMode.StretchImage;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdCloseMaps_Click(object sender, EventArgs e)
        {
            try
            {
                this.saveEmapInfoLocation();
                for (int i = 0; i < this.c1tabMaps.TabPages.Count; i++)
                {
                    foreach (object obj2 in this.c1tabMaps.TabPages[i].Controls)
                    {
                        if (obj2 is Panel)
                        {
                            foreach (object obj3 in ((Panel) obj2).Controls)
                            {
                                if (obj3 is PictureBox)
                                {
                                    wgAppConfig.DisposeImage((obj3 as PictureBox).Image);
                                }
                            }
                        }
                    }
                    wgAppConfig.DisposeImage(this.c1tabMaps.TabPages[i].BackgroundImage);
                }
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdDeleteDoor_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                foreach (object obj2 in this.c1tabMaps.SelectedTab.Controls)
                {
                    if (obj2 is Panel)
                    {
                        foreach (object obj3 in ((Panel) obj2).Controls)
                        {
                            if (obj3 is PictureBox)
                            {
                                foreach (ucMapDoor door in ((PictureBox) obj3).Controls)
                                {
                                    if (door.txtDoorName == door.ActiveControl)
                                    {
                                        if (!flag)
                                        {
                                            if (XMessageBox.Show(sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                            {
                                                return;
                                            }
                                            flag = true;
                                        }
                                        door.Dispose();
                                        ((PictureBox) obj3).Controls.Remove(door);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdDeleteMap_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.c1tabMaps.SelectedTab != null) && (XMessageBox.Show(sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel))
                {
                    this.c1tabMaps.TabPages.Remove(this.c1tabMaps.SelectedTab);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdEditMap_Click(object sender, EventArgs e)
        {
            try
            {
                this.C1ToolBar4MapEdit.Visible = true;
                this.bEditing = true;
                this.C1ToolBar4MapOperate.Visible = false;
                this.cmdAddDoorByLoc.Visible = true;
                this.Timer2.Enabled = false;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdSaveMap_Click(object sender, EventArgs e)
        {
            try
            {
                this.dvMapDoor = new DataView(this.dvDoors.Table);
                string strSql = " DELETE FROM t_d_maps ";
                wgAppConfig.runUpdateSql(strSql);
                strSql = " DELETE FROM t_d_mapdoors ";
                wgAppConfig.runUpdateSql(strSql);
                this.cmdZoomIn.Enabled = false;
                this.cmdZoomOut.Enabled = false;
                this.cmdWatchCurrentMap.Enabled = false;
                this.cmdWatchAllMaps.Enabled = false;
                if ((this.dvMapDoor.Count > 0) && (this.c1tabMaps.TabPages.Count > 0))
                {
                    this.cmdZoomIn.Enabled = true;
                    this.cmdZoomOut.Enabled = true;
                    this.cmdWatchCurrentMap.Enabled = true;
                    this.cmdWatchAllMaps.Enabled = true;
                    for (int i = 0; i <= (this.c1tabMaps.TabPages.Count - 1); i++)
                    {
                        foreach (object obj2 in this.c1tabMaps.TabPages[i].Controls)
                        {
                            if (obj2 is Panel)
                            {
                                foreach (object obj3 in ((Panel) obj2).Controls)
                                {
                                    if (obj3 is PictureBox)
                                    {
                                        strSql = " INSERT INTO t_d_maps";
                                        wgAppConfig.runUpdateSql(((((strSql + " (f_MapName, f_MapPageIndex, f_MapFile) ") + " Values(" + wgTools.PrepareStr(this.c1tabMaps.TabPages[i].Text)) + " ," + i) + " ," + wgTools.PrepareStr(this.c1tabMaps.TabPages[i].Tag)) + " )");
                                        strSql = "SELECT f_MapID from t_d_maps where f_MapName = " + wgTools.PrepareStr(this.c1tabMaps.TabPages[i].Text);
                                        long num2 = int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getValBySql(strSql)));
                                        foreach (ucMapDoor door in ((PictureBox) obj3).Controls)
                                        {
                                            this.dvMapDoor.RowFilter = " f_DoorName = " + wgTools.PrepareStr(door.doorName);
                                            wgAppConfig.runUpdateSql((((((" INSERT INTO t_d_mapdoors" + " (f_DoorID, f_MapID, f_DoorLocationX, f_DoorLocationY) ") + " Values(" + this.dvMapDoor[0]["f_DoorID"]) + " ," + num2) + " ," + door.doorLocation.X) + "," + door.doorLocation.Y) + " )");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                this.C1ToolBar4MapEdit.Visible = false;
                this.bEditing = false;
                this.C1ToolBar4MapOperate.Visible = true;
                this.cmdAddDoorByLoc.Visible = false;
                this.Timer2.Enabled = true;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdWatchAllMaps_Click(object sender, EventArgs e)
        {
            try
            {
                this.lstDoors.SelectedItems.Clear();
                for (int i = 0; i <= (this.c1tabMaps.TabPages.Count - 1); i++)
                {
                    foreach (object obj2 in this.c1tabMaps.TabPages[i].Controls)
                    {
                        if (obj2 is Panel)
                        {
                            foreach (object obj3 in ((Panel) obj2).Controls)
                            {
                                if (obj3 is PictureBox)
                                {
                                    foreach (ucMapDoor door in ((PictureBox) obj3).Controls)
                                    {
                                        for (int j = 0; j <= (this.lstDoors.Items.Count - 1); j++)
                                        {
                                            if (this.lstDoors.Items[j].Text == door.doorName)
                                            {
                                                this.lstDoors.Items[j].Selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (this.btnMonitor != null)
                {
                    this.btnMonitor.PerformClick();
                    this.cmdWatchCurrentMap.Text = this.strcmdWatchCurrentMapBk;
                    this.cmdWatchAllMaps.Text = this.strcmdWatchAllMapsBk;
                    this.cmdWatchCurrentMap.BackColor = Color.Transparent;
                    this.cmdWatchAllMaps.BackColor = Color.Transparent;
                    if (this.lstDoors.SelectedItems.Count > 0)
                    {
                        (sender as ToolStripButton).BackColor = Color.Green;
                        this.btnStopOthers.BackColor = Color.Red;
                        (sender as ToolStripButton).Text = CommonStr.strMonitoring;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdWatchCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                this.lstDoors.SelectedItems.Clear();
                int tabIndex = this.c1tabMaps.SelectedTab.TabIndex;
                foreach (object obj2 in this.c1tabMaps.TabPages[tabIndex].Controls)
                {
                    if (obj2 is Panel)
                    {
                        foreach (object obj3 in ((Panel) obj2).Controls)
                        {
                            if (obj3 is PictureBox)
                            {
                                foreach (ucMapDoor door in ((PictureBox) obj3).Controls)
                                {
                                    for (int i = 0; i <= (this.lstDoors.Items.Count - 1); i++)
                                    {
                                        if (this.lstDoors.Items[i].Text == door.doorName)
                                        {
                                            this.lstDoors.Items[i].Selected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (this.btnMonitor != null)
                {
                    this.btnMonitor.PerformClick();
                    this.cmdWatchCurrentMap.Text = this.strcmdWatchCurrentMapBk;
                    this.cmdWatchAllMaps.Text = this.strcmdWatchAllMapsBk;
                    this.cmdWatchCurrentMap.BackColor = Color.Transparent;
                    this.cmdWatchAllMaps.BackColor = Color.Transparent;
                    if (this.lstDoors.SelectedItems.Count > 0)
                    {
                        (sender as ToolStripButton).BackColor = Color.Green;
                        this.btnStopOthers.BackColor = Color.Red;
                        (sender as ToolStripButton).Text = CommonStr.strMonitoring;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void cmdZoomIn_Click(object sender, EventArgs e)
        {
            this.mapZoom(1.25f);
        }

        private void cmdZoomOut_Click(object sender, EventArgs e)
        {
            this.mapZoom(0.8f);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.photoMemoryStream != null))
            {
                this.photoMemoryStream.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMaps_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.c1tabMaps.TabPages.Count; i++)
                {
                    foreach (object obj2 in this.c1tabMaps.TabPages[i].Controls)
                    {
                        if (obj2 is Panel)
                        {
                            foreach (object obj3 in ((Panel) obj2).Controls)
                            {
                                if (obj3 is PictureBox)
                                {
                                    wgAppConfig.DisposeImage((obj3 as PictureBox).Image);
                                }
                            }
                        }
                    }
                    wgAppConfig.DisposeImage(this.c1tabMaps.TabPages[i].BackgroundImage);
                }
            }
            catch
            {
            }
        }

        private void frmMaps_Load(object sender, EventArgs e)
        {
            try
            {
                this.C1ToolBar4MapEdit.Visible = false;
                this.cmdAddDoorByLoc.Visible = false;
                this.bEditing = false;
                this.c1tabMaps.TabPages.Clear();
                this.loadDoorData();
                this.strcmdWatchCurrentMapBk = this.cmdWatchCurrentMap.Text;
                this.strcmdWatchAllMapsBk = this.cmdWatchAllMaps.Text;
                this.loadmapFromDB();
                bool bReadOnly = false;
                string funName = "btnMaps";
                if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
                {
                    this.cmdEditMap.Visible = false;
                }
                this.Timer2.Enabled = true;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        public PictureBox getPicture(TabPage tabpage)
        {
            PictureBox box = null;
            try
            {
                foreach (object obj2 in tabpage.Controls)
                {
                    if (obj2 is Panel)
                    {
                        foreach (object obj3 in ((Panel) obj2).Controls)
                        {
                            if (obj3 is PictureBox)
                            {
                                box = (PictureBox) obj3;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            return box;
        }

        private void loadDoorData()
        {
            icControllerZone zone;
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
            cmdText = cmdText + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
            this.dt = new DataTable();
            this.dvDoors = new DataView(this.dt);
            this.dvDoors4Watching = new DataView(this.dt);
            this.dvSelected = new DataView(this.dt);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dt);
                        }
                    }
                    goto Label_0105;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dt);
                    }
                }
            }
        Label_0105:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dt);
            try
            {
                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
            }
            catch (Exception)
            {
                throw;
            }
            string str2 = "1";
            this.imgDoor2 = new ImageList();
            this.imgDoor2.ImageSize = new Size(0x18, 0x20);
            this.imgDoor2.TransparentColor = SystemColors.Window;
            str2 = wgAppConfig.getSystemParamByNO(0x16);
            if (!string.IsNullOrEmpty(str2))
            {
                decimal num = decimal.Parse(str2, CultureInfo.InvariantCulture);
                if (((num != 1M) && (num > 0M)) && (num < 100M))
                {
                    this.imgDoor2.ImageSize = new Size((int) (24M * num), (int) (32M * num));
                }
            }
            cmdText = " SELECT a.f_ReaderNO, a.f_ReaderName , b.f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Reader a, t_b_Controller b WHERE  b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ";
            this.dtReader = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command3 = new OleDbCommand(cmdText, connection3))
                    {
                        using (OleDbDataAdapter adapter3 = new OleDbDataAdapter(command3))
                        {
                            adapter3.Fill(this.dtReader);
                        }
                    }
                    goto Label_02D5;
                }
            }
            using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command4 = new SqlCommand(cmdText, connection4))
                {
                    using (SqlDataAdapter adapter4 = new SqlDataAdapter(command4))
                    {
                        adapter4.Fill(this.dtReader);
                    }
                }
            }
        Label_02D5:
            if (this.dtReader.Rows.Count > 0)
            {
                for (int i = 0; i < this.dtReader.Rows.Count; i++)
                {
                    this.ReaderName.Add(string.Format("{0}-{1}", this.dtReader.Rows[i]["f_ControllerSN"].ToString(), this.dtReader.Rows[i]["f_ReaderNO"].ToString()), this.dtReader.Rows[i]["f_ReaderName"].ToString());
                }
            }
        }

        public void loadEmapInfoLocation()
        {
            try
            {
                string keyVal = wgAppConfig.GetKeyVal("EMapLocInfo");
                string str2 = wgAppConfig.GetKeyVal("EMapZoomInfo");
                if ((keyVal != "") && (str2 != ""))
                {
                    string[] strArray = keyVal.Split(new char[] { ',' });
                    string[] strArray2 = str2.Split(new char[] { ',' });
                    if ((strArray.Length * 2) == (strArray2.Length * 3))
                    {
                        int num = 0;
                        while (num <= ((strArray.Length / 3) - 1))
                        {
                            if (strArray[num * 3] != strArray2[num * 2])
                            {
                                return;
                            }
                            num++;
                        }
                        foreach (TabPage page in this.c1tabMaps.TabPages)
                        {
                            for (num = 0; num <= ((strArray.Length / 3) - 1); num++)
                            {
                                if (strArray[num * 3] == page.Text)
                                {
                                    this.c1tabMaps.SelectedTab = page;
                                    this.mapZoom(float.Parse(strArray2[(num * 2) + 1]));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void loadmapFromDB()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadmapFromDB_Acc();
            }
            else
            {
                this.c1tabMaps.Visible = false;
                float result = 1f;
                float.TryParse(wgAppConfig.getSystemParamByNO(0x16), out result);
                try
                {
                    this.dstemp = new DataSet("mapInfo");
                    this.cn = new SqlConnection(wgAppConfig.dbConString);
                    string selectCommandText = "SELECT * FROM  t_d_maps ORDER BY f_MapPageIndex";
                    this.da = new SqlDataAdapter(selectCommandText, this.cn);
                    this.da.Fill(this.dstemp, "t_d_maps");
                    this.dvMap = new DataView(this.dstemp.Tables["t_d_maps"]);
                    string str2 = "";
                    selectCommandText = " SELECT t_d_mapdoors.*, t_d_maps.f_MapName, t_d_maps.f_MapPageIndex, t_b_Door.f_DoorName, t_b_Controller.f_ZoneID ";
                    if (str2 == "")
                    {
                        selectCommandText = selectCommandText + " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ";
                    }
                    else
                    {
                        selectCommandText = (selectCommandText + " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ") + " WHERE t_b_door.f_DoorID IN " + str2;
                    }
                    this.da = new SqlDataAdapter(selectCommandText, this.cn);
                    this.da.Fill(this.dstemp, "v_d_mapdoors");
                    this.dt = this.dstemp.Tables["v_d_mapdoors"];
                    new icControllerZone().getAllowedControllers(ref this.dt);
                    this.dvMapDoors = new DataView(this.dstemp.Tables["v_d_mapdoors"]);
                    this.c1tabMaps.TabPages.Clear();
                    if (this.dvMap.Count > 0)
                    {
                        this.cmdZoomIn.Enabled = true;
                        this.cmdZoomOut.Enabled = true;
                        this.cmdWatchCurrentMap.Enabled = true;
                        this.cmdWatchAllMaps.Enabled = true;
                        for (int i = 0; i <= (this.dvMap.Count - 1); i++)
                        {
                            this.mapPage = new TabPage();
                            this.mapPage.Text = this.dvMap[i]["f_MapName"].ToString();
                            this.mapPage.Tag = this.dvMap[i]["f_MapFile"];
                            this.c1tabMaps.TabPages.Add(this.mapPage);
                            this.c1tabMaps.SelectedTab = this.mapPage;
                            this.mapPanel = new Panel();
                            this.mapPicture = new PictureBox();
                            this.mapPanel.Dock = DockStyle.Fill;
                            this.mapPanel.BackColor = Color.White;
                            this.mapPanel.AutoScroll = true;
                            this.mapPicture.SizeMode = PictureBoxSizeMode.AutoSize;
                            this.ShowMap(this.mapPage.Tag.ToString(), this.mapPicture);
                            this.mapPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                            this.mapPicture.ContextMenuStrip = this.C1CmnuMap;
                            this.mapPicture.MouseDown += new MouseEventHandler(this.mapPicture_MouseDown);
                            this.mapPanel.Controls.Add(this.mapPicture);
                            this.mapPage.Controls.Add(this.mapPanel);
                            this.dvMapDoors.RowFilter = " f_MapID= " + this.dvMap[i]["f_MapID"];
                            for (int j = 0; j <= (this.dvMapDoors.Count - 1); j++)
                            {
                                this.uc1door = new ucMapDoor();
                                this.uc1door.doorName = this.dvMapDoors[j]["f_DoorName"].ToString();
                                this.uc1door.doorScale = result;
                                this.uc1door.bindSource = this.mapPicture;
                                this.uc1door.doorLocation = new Point(int.Parse(this.dvMapDoors[j]["f_DoorLocationX"].ToString()), int.Parse(this.dvMapDoors[j]["f_DoorLocationY"].ToString()));
                                this.uc1door.MouseDown += new MouseEventHandler(this.UcMapDoor_MouseDown);
                                this.uc1door.MouseMove += new MouseEventHandler(this.UcMapDoor_MouseMove);
                                this.uc1door.MouseUp += new MouseEventHandler(this.UcMapDoor_MouseUp);
                                this.uc1door.Click += new EventHandler(this.ucMapDoor_Click);
                                this.uc1door.imgDoor = this.imgDoor2;
                                this.uc1door.ContextMenuStrip = this.contextMenuStrip1Doors;
                                this.uc1door.picDoorState.ContextMenuStrip = this.C1CmnuDoor;
                                this.mapPicture.Controls.Add(this.uc1door);
                            }
                        }
                    }
                    else
                    {
                        this.cmdZoomIn.Enabled = false;
                        this.cmdZoomOut.Enabled = false;
                        this.cmdWatchCurrentMap.Enabled = false;
                        this.cmdWatchAllMaps.Enabled = false;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                this.loadEmapInfoLocation();
                this.c1tabMaps.Visible = true;
            }
        }

        private void loadmapFromDB_Acc()
        {
            OleDbConnection selectConnection = null;
            this.c1tabMaps.Visible = false;
            float result = 1f;
            float.TryParse(wgAppConfig.getSystemParamByNO(0x16), out result);
            try
            {
                this.dstemp = new DataSet("mapInfo");
                selectConnection = new OleDbConnection(wgAppConfig.dbConString);
                string selectCommandText = "SELECT * FROM  t_d_maps ORDER BY f_MapPageIndex";
                new OleDbDataAdapter(selectCommandText, selectConnection).Fill(this.dstemp, "t_d_maps");
                this.dvMap = new DataView(this.dstemp.Tables["t_d_maps"]);
                string str2 = "";
                selectCommandText = " SELECT t_d_mapdoors.*, t_d_maps.f_MapName, t_d_maps.f_MapPageIndex, t_b_Door.f_DoorName, t_b_Controller.f_ZoneID ";
                if (str2 == "")
                {
                    selectCommandText = selectCommandText + " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ";
                }
                else
                {
                    selectCommandText = (selectCommandText + " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ") + " WHERE t_b_door.f_DoorID IN " + str2;
                }
                new OleDbDataAdapter(selectCommandText, selectConnection).Fill(this.dstemp, "v_d_mapdoors");
                this.dt = this.dstemp.Tables["v_d_mapdoors"];
                new icControllerZone().getAllowedControllers(ref this.dt);
                this.dvMapDoors = new DataView(this.dstemp.Tables["v_d_mapdoors"]);
                this.c1tabMaps.TabPages.Clear();
                if (this.dvMap.Count > 0)
                {
                    this.cmdZoomIn.Enabled = true;
                    this.cmdZoomOut.Enabled = true;
                    this.cmdWatchCurrentMap.Enabled = true;
                    this.cmdWatchAllMaps.Enabled = true;
                    for (int i = 0; i <= (this.dvMap.Count - 1); i++)
                    {
                        this.mapPage = new TabPage();
                        this.mapPage.Text = this.dvMap[i]["f_MapName"].ToString();
                        this.mapPage.Tag = this.dvMap[i]["f_MapFile"];
                        this.c1tabMaps.TabPages.Add(this.mapPage);
                        this.c1tabMaps.SelectedTab = this.mapPage;
                        this.mapPanel = new Panel();
                        this.mapPicture = new PictureBox();
                        this.mapPanel.Dock = DockStyle.Fill;
                        this.mapPanel.BackColor = Color.White;
                        this.mapPanel.AutoScroll = true;
                        this.mapPicture.SizeMode = PictureBoxSizeMode.AutoSize;
                        this.ShowMap(this.mapPage.Tag.ToString(), this.mapPicture);
                        this.mapPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                        this.mapPicture.ContextMenuStrip = this.C1CmnuMap;
                        this.mapPicture.MouseDown += new MouseEventHandler(this.mapPicture_MouseDown);
                        this.mapPanel.Controls.Add(this.mapPicture);
                        this.mapPage.Controls.Add(this.mapPanel);
                        this.dvMapDoors.RowFilter = " f_MapID= " + this.dvMap[i]["f_MapID"];
                        for (int j = 0; j <= (this.dvMapDoors.Count - 1); j++)
                        {
                            this.uc1door = new ucMapDoor();
                            this.uc1door.doorName = this.dvMapDoors[j]["f_DoorName"].ToString();
                            this.uc1door.doorScale = result;
                            this.uc1door.bindSource = this.mapPicture;
                            this.uc1door.doorLocation = new Point(int.Parse(this.dvMapDoors[j]["f_DoorLocationX"].ToString()), int.Parse(this.dvMapDoors[j]["f_DoorLocationY"].ToString()));
                            this.uc1door.MouseDown += new MouseEventHandler(this.UcMapDoor_MouseDown);
                            this.uc1door.MouseMove += new MouseEventHandler(this.UcMapDoor_MouseMove);
                            this.uc1door.MouseUp += new MouseEventHandler(this.UcMapDoor_MouseUp);
                            this.uc1door.Click += new EventHandler(this.ucMapDoor_Click);
                            this.uc1door.imgDoor = this.imgDoor2;
                            this.uc1door.ContextMenuStrip = this.contextMenuStrip1Doors;
                            this.uc1door.picDoorState.ContextMenuStrip = this.C1CmnuDoor;
                            this.mapPicture.Controls.Add(this.uc1door);
                        }
                    }
                }
                else
                {
                    this.cmdZoomIn.Enabled = false;
                    this.cmdZoomOut.Enabled = false;
                    this.cmdWatchCurrentMap.Enabled = false;
                    this.cmdWatchAllMaps.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            this.loadEmapInfoLocation();
            this.c1tabMaps.Visible = true;
        }

        private void mapPicture_MouseDown(object sender, MouseEventArgs e)
        {
            this.lastMouseP = e.Location;
        }

        private void mapZoom(float zoomScale)
        {
            try
            {
                if (this.c1tabMaps.SelectedTab != null)
                {
                    PictureBox box = this.getPicture(this.c1tabMaps.SelectedTab);
                    if (box != null)
                    {
                        float result = 1f;
                        if (this.arrZoomScaleTabpageName.IndexOf(this.c1tabMaps.SelectedTab.Text) < 0)
                        {
                            this.arrZoomScaleTabpageName.Add(this.c1tabMaps.SelectedTab.Text);
                            this.arrZoomScale.Add(1.0);
                        }
                        int index = this.arrZoomScaleTabpageName.IndexOf(this.c1tabMaps.SelectedTab.Text);
                        if (index >= 0)
                        {
                            float.TryParse(this.arrZoomScale[index].ToString(), out result);
                            if ((((box.Width * zoomScale) >= 10f) && ((box.Width * zoomScale) <= 10000f)) && (((box.Height * zoomScale) >= 10f) && ((box.Height * zoomScale) <= 10000f)))
                            {
                                result *= zoomScale;
                                this.arrZoomScale[index] = result;
                                box.Size = new Size(new Point((int) (box.Width * zoomScale), (int) (box.Height * zoomScale)));
                                foreach (object obj2 in box.Controls)
                                {
                                    if (obj2 is ucMapDoor)
                                    {
                                        ((ucMapDoor) obj2).mapScale *= zoomScale;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        public void saveEmapInfoLocation()
        {
            try
            {
                string str = "";
                string str2 = "";
                foreach (TabPage page in this.c1tabMaps.TabPages)
                {
                    int index = this.arrZoomScaleTabpageName.IndexOf(page.Text);
                    float num2 = 1f;
                    if (index >= 0)
                    {
                        num2 = (float) this.arrZoomScale[index];
                    }
                    if (str2 != "")
                    {
                        str2 = str2 + ",";
                    }
                    object obj2 = str2;
                    str2 = string.Concat(new object[] { obj2, page.Text, ",", num2 });
                    PictureBox box = this.getPicture(page);
                    if (str != "")
                    {
                        str = str + ",";
                    }
                    if (box != null)
                    {
                        object obj3 = str;
                        str = string.Concat(new object[] { obj3, page.Text, ",", box.Location.X, ",", box.Location.Y });
                    }
                    else
                    {
                        str = str + page.Text + ",0,0";
                    }
                }
                wgAppConfig.UpdateKeyVal("EMapZoomInfo", str2);
                wgAppConfig.UpdateKeyVal("EMapLocInfo", str);
                this.arrZoomScaleTabpageName.Clear();
                this.arrZoomScale.Clear();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        public void ShowMap(string fileToDisplay, PictureBox obj)
        {
            try
            {
                obj.Visible = false;
                if (fileToDisplay != null)
                {
                    FileInfo info = new FileInfo(wgAppConfig.Path4PhotoDefault() + fileToDisplay);
                    if (info.Exists)
                    {
                        using (FileStream stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
                        {
                            this.photoImageData = new byte[stream.Length + 1L];
                            stream.Read(this.photoImageData, 0, (int) stream.Length);
                        }
                        if (this.photoMemoryStream != null)
                        {
                            try
                            {
                                this.photoMemoryStream.Close();
                            }
                            catch (Exception)
                            {
                            }
                            this.photoMemoryStream = null;
                        }
                        this.photoMemoryStream = new MemoryStream(this.photoImageData);
                        try
                        {
                            if (obj.Image != null)
                            {
                                obj.Image.Dispose();
                            }
                        }
                        catch (Exception)
                        {
                        }
                        obj.Image = Image.FromStream(this.photoMemoryStream);
                        obj.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (!this.bEditing)
            {
                this.Timer2.Enabled = false;
                try
                {
                    bool flag = false;
                    for (int i = 0; i <= (this.c1tabMaps.TabPages.Count - 1); i++)
                    {
                        foreach (object obj2 in this.c1tabMaps.TabPages[i].Controls)
                        {
                            if (obj2 is Panel)
                            {
                                foreach (object obj3 in ((Panel) obj2).Controls)
                                {
                                    if (obj3 is PictureBox)
                                    {
                                        foreach (ucMapDoor door in ((PictureBox) obj3).Controls)
                                        {
                                            for (int j = 0; j <= (this.lstDoors.Items.Count - 1); j++)
                                            {
                                                if ((this.lstDoors.Items[j].Text == door.doorName) && (this.lstDoors.Items[j].ImageIndex != door.doorStatus))
                                                {
                                                    door.doorStatus = this.lstDoors.Items[j].ImageIndex;
                                                    if (!flag)
                                                    {
                                                        if (door.doorStatus >= 4)
                                                        {
                                                            this.c1tabMaps.SelectedTab = this.c1tabMaps.TabPages[i];
                                                        }
                                                        flag = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                this.Timer2.Enabled = true;
            }
        }

        private void ucMapDoor_Click(object sender, EventArgs e)
        {
            try
            {
                ucMapDoor door = (ucMapDoor) sender;
                for (int i = 0; i <= (this.lstDoors.Items.Count - 1); i++)
                {
                    if (this.lstDoors.Items[i].Text == door.doorName)
                    {
                        this.lstDoors.SelectedItems.Clear();
                        this.lstDoors.Items[i].Selected = true;
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void UcMapDoor_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                PictureBox bindSource = null;
                bindSource = ((ucMapDoor) sender).bindSource;
                this.t = bindSource.PointToClient(Control.MousePosition).Y - (sender as Control).Top;
                this.l = bindSource.PointToClient(Control.MousePosition).X - (sender as Control).Left;
                ucMapDoor door = (ucMapDoor) sender;
                this.currentUcMapDoor = door;
                for (int i = 0; i <= (this.lstDoors.Items.Count - 1); i++)
                {
                    if (this.lstDoors.Items[i].Text == door.doorName)
                    {
                        this.lstDoors.SelectedItems.Clear();
                        this.lstDoors.Items[i].Selected = true;
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void UcMapDoor_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.cmdAddMap.Visible)
            {
                try
                {
                    PictureBox bindSource = ((ucMapDoor) sender).bindSource;
                    int num = bindSource.PointToClient(Control.MousePosition).Y - this.t;
                    int num2 = bindSource.PointToClient(Control.MousePosition).X - this.l;
                    (sender as Control).Top = num;
                    (sender as Control).Left = num2;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        private void UcMapDoor_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.cmdAddMap.Visible)
            {
                try
                {
                    ((ucMapDoor) sender).doorLocation = ((ucMapDoor) sender).Location;
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }
    }
}

