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

    public partial class frmMaps
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMaps));
            this.c1tabMaps = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.C1CmnuMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdAddDoorByLoc = new System.Windows.Forms.ToolStripMenuItem();
            this.C1CmnuDoor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDoorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer2 = new System.Windows.Forms.Timer(this.components);
            this.C1ToolBar4MapEdit = new System.Windows.Forms.ToolStrip();
            this.cmdAddMap = new System.Windows.Forms.ToolStripButton();
            this.cmdDeleteMap = new System.Windows.Forms.ToolStripButton();
            this.cmdChangeMapName = new System.Windows.Forms.ToolStripButton();
            this.cmdAddDoor = new System.Windows.Forms.ToolStripButton();
            this.cmdDeleteDoor = new System.Windows.Forms.ToolStripButton();
            this.cmdSaveMap = new System.Windows.Forms.ToolStripButton();
            this.cmdCancelAndExit = new System.Windows.Forms.ToolStripButton();
            this.C1ToolBar4MapOperate = new System.Windows.Forms.ToolStrip();
            this.cmdCloseMaps = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomIn = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomOut = new System.Windows.Forms.ToolStripButton();
            this.cmdEditMap = new System.Windows.Forms.ToolStripButton();
            this.cmdWatchCurrentMap = new System.Windows.Forms.ToolStripButton();
            this.cmdWatchAllMaps = new System.Windows.Forms.ToolStripButton();
            this.btnStopOthers = new System.Windows.Forms.ToolStripButton();
            this.c1tabMaps.SuspendLayout();
            this.C1CmnuMap.SuspendLayout();
            this.C1CmnuDoor.SuspendLayout();
            this.C1ToolBar4MapEdit.SuspendLayout();
            this.C1ToolBar4MapOperate.SuspendLayout();
            this.SuspendLayout();
            // 
            // c1tabMaps
            // 
            this.c1tabMaps.Controls.Add(this.tabPage2);
            this.c1tabMaps.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.c1tabMaps, "c1tabMaps");
            this.c1tabMaps.Name = "c1tabMaps";
            this.c1tabMaps.SelectedIndex = 0;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // C1CmnuMap
            // 
            this.C1CmnuMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdAddDoorByLoc});
            this.C1CmnuMap.Name = "C1CmnuMap";
            resources.ApplyResources(this.C1CmnuMap, "C1CmnuMap");
            // 
            // cmdAddDoorByLoc
            // 
            this.cmdAddDoorByLoc.Name = "cmdAddDoorByLoc";
            resources.ApplyResources(this.cmdAddDoorByLoc, "cmdAddDoorByLoc");
            this.cmdAddDoorByLoc.Click += new System.EventHandler(this.cmdAddDoor_Click);
            // 
            // C1CmnuDoor
            // 
            this.C1CmnuDoor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDoorToolStripMenuItem});
            this.C1CmnuDoor.Name = "C1CmnuMap";
            resources.ApplyResources(this.C1CmnuDoor, "C1CmnuDoor");
            // 
            // openDoorToolStripMenuItem
            // 
            this.openDoorToolStripMenuItem.Name = "openDoorToolStripMenuItem";
            resources.ApplyResources(this.openDoorToolStripMenuItem, "openDoorToolStripMenuItem");
            // 
            // Timer2
            // 
            this.Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // C1ToolBar4MapEdit
            // 
            this.C1ToolBar4MapEdit.BackColor = System.Drawing.Color.Transparent;
            this.C1ToolBar4MapEdit.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.C1ToolBar4MapEdit, "C1ToolBar4MapEdit");
            this.C1ToolBar4MapEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdAddMap,
            this.cmdDeleteMap,
            this.cmdChangeMapName,
            this.cmdAddDoor,
            this.cmdDeleteDoor,
            this.cmdSaveMap,
            this.cmdCancelAndExit});
            this.C1ToolBar4MapEdit.Name = "C1ToolBar4MapEdit";
            // 
            // cmdAddMap
            // 
            this.cmdAddMap.ForeColor = System.Drawing.Color.White;
            this.cmdAddMap.Image = global::Properties.Resources.icon_add_auto;
            resources.ApplyResources(this.cmdAddMap, "cmdAddMap");
            this.cmdAddMap.Name = "cmdAddMap";
            this.cmdAddMap.Click += new System.EventHandler(this.cmdAddMap_Click);
            // 
            // cmdDeleteMap
            // 
            this.cmdDeleteMap.ForeColor = System.Drawing.Color.White;
            this.cmdDeleteMap.Image = global::Properties.Resources.icon_delete;
            resources.ApplyResources(this.cmdDeleteMap, "cmdDeleteMap");
            this.cmdDeleteMap.Name = "cmdDeleteMap";
            this.cmdDeleteMap.Click += new System.EventHandler(this.cmdDeleteMap_Click);
            // 
            // cmdChangeMapName
            // 
            this.cmdChangeMapName.ForeColor = System.Drawing.Color.White;
            this.cmdChangeMapName.Image = global::Properties.Resources.icon_edit_batch;
            resources.ApplyResources(this.cmdChangeMapName, "cmdChangeMapName");
            this.cmdChangeMapName.Name = "cmdChangeMapName";
            this.cmdChangeMapName.Click += new System.EventHandler(this.cmdChangeMapName_Click);
            // 
            // cmdAddDoor
            // 
            this.cmdAddDoor.ForeColor = System.Drawing.Color.White;
            this.cmdAddDoor.Image = global::Properties.Resources.icon_new;
            resources.ApplyResources(this.cmdAddDoor, "cmdAddDoor");
            this.cmdAddDoor.Name = "cmdAddDoor";
            this.cmdAddDoor.Click += new System.EventHandler(this.cmdAddDoor_Click);
            // 
            // cmdDeleteDoor
            // 
            this.cmdDeleteDoor.ForeColor = System.Drawing.Color.White;
            this.cmdDeleteDoor.Image = global::Properties.Resources.icon_delete;
            resources.ApplyResources(this.cmdDeleteDoor, "cmdDeleteDoor");
            this.cmdDeleteDoor.Name = "cmdDeleteDoor";
            this.cmdDeleteDoor.Click += new System.EventHandler(this.cmdDeleteDoor_Click);
            // 
            // cmdSaveMap
            // 
            this.cmdSaveMap.ForeColor = System.Drawing.Color.White;
            this.cmdSaveMap.Image = global::Properties.Resources.icon_maps_save;
            resources.ApplyResources(this.cmdSaveMap, "cmdSaveMap");
            this.cmdSaveMap.Name = "cmdSaveMap";
            this.cmdSaveMap.Click += new System.EventHandler(this.cmdSaveMap_Click);
            // 
            // cmdCancelAndExit
            // 
            this.cmdCancelAndExit.ForeColor = System.Drawing.Color.White;
            this.cmdCancelAndExit.Image = global::Properties.Resources.icon_close;
            resources.ApplyResources(this.cmdCancelAndExit, "cmdCancelAndExit");
            this.cmdCancelAndExit.Name = "cmdCancelAndExit";
            this.cmdCancelAndExit.Click += new System.EventHandler(this.cmdCancelAndExit_Click);
            // 
            // C1ToolBar4MapOperate
            // 
            this.C1ToolBar4MapOperate.BackColor = System.Drawing.Color.Transparent;
            this.C1ToolBar4MapOperate.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.C1ToolBar4MapOperate, "C1ToolBar4MapOperate");
            this.C1ToolBar4MapOperate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdCloseMaps,
            this.cmdZoomIn,
            this.cmdZoomOut,
            this.cmdEditMap,
            this.cmdWatchCurrentMap,
            this.cmdWatchAllMaps,
            this.btnStopOthers});
            this.C1ToolBar4MapOperate.Name = "C1ToolBar4MapOperate";
            // 
            // cmdCloseMaps
            // 
            this.cmdCloseMaps.ForeColor = System.Drawing.Color.White;
            this.cmdCloseMaps.Image = global::Properties.Resources.icon_close;
            resources.ApplyResources(this.cmdCloseMaps, "cmdCloseMaps");
            this.cmdCloseMaps.Name = "cmdCloseMaps";
            this.cmdCloseMaps.Click += new System.EventHandler(this.cmdCloseMaps_Click);
            // 
            // cmdZoomIn
            // 
            this.cmdZoomIn.ForeColor = System.Drawing.Color.White;
            this.cmdZoomIn.Image = global::Properties.Resources.icon_maps_zoomin;
            resources.ApplyResources(this.cmdZoomIn, "cmdZoomIn");
            this.cmdZoomIn.Name = "cmdZoomIn";
            this.cmdZoomIn.Click += new System.EventHandler(this.cmdZoomIn_Click);
            // 
            // cmdZoomOut
            // 
            this.cmdZoomOut.ForeColor = System.Drawing.Color.White;
            this.cmdZoomOut.Image = global::Properties.Resources.icon_maps_zoomout;
            resources.ApplyResources(this.cmdZoomOut, "cmdZoomOut");
            this.cmdZoomOut.Name = "cmdZoomOut";
            this.cmdZoomOut.Click += new System.EventHandler(this.cmdZoomOut_Click);
            // 
            // cmdEditMap
            // 
            this.cmdEditMap.ForeColor = System.Drawing.Color.White;
            this.cmdEditMap.Image = global::Properties.Resources.icon_edit;
            resources.ApplyResources(this.cmdEditMap, "cmdEditMap");
            this.cmdEditMap.Name = "cmdEditMap";
            this.cmdEditMap.Click += new System.EventHandler(this.cmdEditMap_Click);
            // 
            // cmdWatchCurrentMap
            // 
            this.cmdWatchCurrentMap.ForeColor = System.Drawing.Color.White;
            this.cmdWatchCurrentMap.Image = global::Properties.Resources.icon_monitor;
            resources.ApplyResources(this.cmdWatchCurrentMap, "cmdWatchCurrentMap");
            this.cmdWatchCurrentMap.Name = "cmdWatchCurrentMap";
            this.cmdWatchCurrentMap.Click += new System.EventHandler(this.cmdWatchCurrentMap_Click);
            // 
            // cmdWatchAllMaps
            // 
            this.cmdWatchAllMaps.ForeColor = System.Drawing.Color.White;
            this.cmdWatchAllMaps.Image = global::Properties.Resources.icon_select_all;
            resources.ApplyResources(this.cmdWatchAllMaps, "cmdWatchAllMaps");
            this.cmdWatchAllMaps.Name = "cmdWatchAllMaps";
            this.cmdWatchAllMaps.Click += new System.EventHandler(this.cmdWatchAllMaps_Click);
            // 
            // btnStopOthers
            // 
            this.btnStopOthers.ForeColor = System.Drawing.Color.White;
            this.btnStopOthers.Image = global::Properties.Resources.icon_stop;
            resources.ApplyResources(this.btnStopOthers, "btnStopOthers");
            this.btnStopOthers.Name = "btnStopOthers";
            this.btnStopOthers.Click += new System.EventHandler(this.btnStopOthers_Click);
            // 
            // frmMaps
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.c1tabMaps);
            this.Controls.Add(this.C1ToolBar4MapEdit);
            this.Controls.Add(this.C1ToolBar4MapOperate);
            this.MinimizeBox = false;
            this.Name = "frmMaps";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMaps_FormClosing);
            this.Load += new System.EventHandler(this.frmMaps_Load);
            this.c1tabMaps.ResumeLayout(false);
            this.C1CmnuMap.ResumeLayout(false);
            this.C1CmnuDoor.ResumeLayout(false);
            this.C1ToolBar4MapEdit.ResumeLayout(false);
            this.C1ToolBar4MapEdit.PerformLayout();
            this.C1ToolBar4MapOperate.ResumeLayout(false);
            this.C1ToolBar4MapOperate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ToolStripButton btnMonitor;
        public ToolStripButton btnStop;
        private ToolStripButton btnStopOthers;
        private ContextMenuStrip C1CmnuDoor;
        private ContextMenuStrip C1CmnuMap;
        private TabControl c1tabMaps;
        private ToolStrip C1ToolBar4MapEdit;
        private ToolStrip C1ToolBar4MapOperate;
        private ToolStripButton cmdAddDoor;
        private ToolStripMenuItem cmdAddDoorByLoc;
        private ToolStripButton cmdAddMap;
        private ToolStripButton cmdCancelAndExit;
        private ToolStripButton cmdChangeMapName;
        private ToolStripButton cmdCloseMaps;
        private ToolStripButton cmdDeleteDoor;
        private ToolStripButton cmdDeleteMap;
        private ToolStripButton cmdEditMap;
        private ToolStripButton cmdSaveMap;
        private ToolStripButton cmdWatchAllMaps;
        private ToolStripButton cmdWatchCurrentMap;
        private ToolStripButton cmdZoomIn;
        private ToolStripButton cmdZoomOut;
        public ContextMenuStrip contextMenuStrip1Doors;
        private ToolStripMenuItem openDoorToolStripMenuItem;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Timer Timer2;
    }
}

