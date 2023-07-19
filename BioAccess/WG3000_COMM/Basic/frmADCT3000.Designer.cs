namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ExtendFunc;
    using WG3000_COMM.ExtendFunc.Elevator;
    using WG3000_COMM.ExtendFunc.Meal;
    using WG3000_COMM.ExtendFunc.Meeting;
    using WG3000_COMM.ExtendFunc.Patrol;
    using WG3000_COMM.ExtendFunc.PCCheck;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmADCT3000
    {
		private ToolStripButton btnBookmarkSelected;
        private ToggleButton btnIconAttendance;
        private ToggleButton btnIconBasicConfig;
        private ToggleButton btnIconBasicOperate;
        private ToggleButton btnIconSelected;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip3Normal;
        private dfrmAbout dfrmAbout1;
        private dfrmCheckAccessConfigure dfrmCheckAccessConfigure1;
        private dfrmControllerTaskList dfrmControllerTaskList1;
        private dfrmDbCompact dfrmDbCompact1;
        private dfrmLogQuery dfrmLogQuery1;
        private dfrmNetControllerConfig dfrmNetControllerConfig1;
        private dfrmOperator dfrmOperator1;
        private dfrmSetPassword dfrmSetPassword1;
        private frmTestController frmTestController1;
        private Panel panel2Content;
        private PictureBox panel4Form;
        private ToolStripMenuItem shortcutAttendance;
        private ToolStripMenuItem shortcutConsole;
        private ToolStripMenuItem shortcutControllers;
        private ToolStripMenuItem shortcutPersonnel;
        private ToolStripMenuItem shortcutPrivilege;
        private ToolStripMenuItem shortcutSwipe;
        private ToolStripStatusLabel statCOM;
        private ToolStripStatusLabel statOperator;
        private ToolStripStatusLabel statRuninfo1;
        private ToolStripStatusLabel statRuninfo2;
        private ToolStripStatusLabel statRuninfo3;
        private ToolStripStatusLabel statRuninfoLoadedNum;
        private ToolStripStatusLabel statSoftwareVer;
        private ToolStripStatusLabel statTimeDate;
        private StatusStrip stbRunInfo;
        private ToolStripMenuItem systemParamsToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem toolsFormToolStripMenuItem;
        private ToolStrip toolStrip1BookMark;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private ToolStripButton toolStripButtonBookmark1;
        private ToolStripButton toolStripButtonBookmark2;
        private ToolStripButton toolStripButtonBookmark3;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem12;
        private ToolStripMenuItem toolStripMenuItem13;
        private ToolStripMenuItem toolStripMenuItem14;
        private ToolStripMenuItem toolStripMenuItem15;
        private ToolStripMenuItem toolStripMenuItem16;
        private ToolStripMenuItem toolStripMenuItem17;
        private ToolStripMenuItem toolStripMenuItem18;
        private ToolStripMenuItem toolStripMenuItem19;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem20;
        private ToolStripMenuItem toolStripMenuItem23;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolTip toolTip1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmADCT3000));
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.systemParamsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip3Normal = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.shortcutControllers = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutPersonnel = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutPrivilege = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutConsole = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutSwipe = new System.Windows.Forms.ToolStripMenuItem();
			this.shortcutAttendance = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.toolStrip1BookMark = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonBookmark1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonBookmark2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonBookmark3 = new System.Windows.Forms.ToolStripButton();
			this.panel2Content = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnIconBasicConfig = new System.Windows.Forms.ToggleButton();
			this.btnIconBasicOperate = new System.Windows.Forms.ToggleButton();
			this.btnIconAttendance = new System.Windows.Forms.ToggleButton();
			this.btnIconTools = new System.Windows.Forms.ToggleButton();
			this.btnIconHelp = new System.Windows.Forms.ToggleButton();
			this.stbRunInfo = new System.Windows.Forms.StatusStrip();
			this.statOperator = new System.Windows.Forms.ToolStripStatusLabel();
			this.statSoftwareVer = new System.Windows.Forms.ToolStripStatusLabel();
			this.statCOM = new System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfo1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfo2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfo3 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statRuninfoLoadedNum = new System.Windows.Forms.ToolStripStatusLabel();
			this.statTimeDate = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel4Form = new System.Windows.Forms.PictureBox();
			this.contextMenuStrip3Normal.SuspendLayout();
			this.toolStrip1BookMark.SuspendLayout();
			this.panel2Content.SuspendLayout();
			this.panel1.SuspendLayout();
			this.stbRunInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panel4Form)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			resources.ApplyResources(this.toolStripMenuItem23, "toolStripMenuItem23");
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// systemParamsToolStripMenuItem
			// 
			this.systemParamsToolStripMenuItem.Name = "systemParamsToolStripMenuItem";
			resources.ApplyResources(this.systemParamsToolStripMenuItem, "systemParamsToolStripMenuItem");
			this.systemParamsToolStripMenuItem.Click += new System.EventHandler(this.systemParamsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem20
			// 
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			resources.ApplyResources(this.toolStripMenuItem20, "toolStripMenuItem20");
			// 
			// toolStripMenuItem19
			// 
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			resources.ApplyResources(this.toolStripMenuItem19, "toolStripMenuItem19");
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripSeparator5,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12,
            this.toolStripMenuItem13});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			resources.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			resources.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			resources.ApplyResources(this.toolStripMenuItem10, "toolStripMenuItem10");
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			resources.ApplyResources(this.toolStripMenuItem11, "toolStripMenuItem11");
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			resources.ApplyResources(this.toolStripMenuItem12, "toolStripMenuItem12");
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			resources.ApplyResources(this.toolStripMenuItem13, "toolStripMenuItem13");
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem15,
            this.toolStripMenuItem16,
            this.toolStripMenuItem17,
            this.toolStripMenuItem18,
            this.toolsFormToolStripMenuItem});
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			resources.ApplyResources(this.toolStripMenuItem14, "toolStripMenuItem14");
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			resources.ApplyResources(this.toolStripMenuItem15, "toolStripMenuItem15");
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			resources.ApplyResources(this.toolStripMenuItem16, "toolStripMenuItem16");
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			resources.ApplyResources(this.toolStripMenuItem17, "toolStripMenuItem17");
			// 
			// toolStripMenuItem18
			// 
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			resources.ApplyResources(this.toolStripMenuItem18, "toolStripMenuItem18");
			// 
			// toolsFormToolStripMenuItem
			// 
			this.toolsFormToolStripMenuItem.Name = "toolsFormToolStripMenuItem";
			resources.ApplyResources(this.toolsFormToolStripMenuItem, "toolsFormToolStripMenuItem");
			// 
			// timer1
			// 
			this.timer1.Interval = 300;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// contextMenuStrip3Normal
			// 
			this.contextMenuStrip3Normal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortcutControllers,
            this.shortcutPersonnel,
            this.shortcutPrivilege,
            this.shortcutConsole,
            this.shortcutSwipe,
            this.shortcutAttendance});
			this.contextMenuStrip3Normal.Name = "contextMenuStrip3Normal";
			resources.ApplyResources(this.contextMenuStrip3Normal, "contextMenuStrip3Normal");
			// 
			// shortcutControllers
			// 
			this.shortcutControllers.Name = "shortcutControllers";
			resources.ApplyResources(this.shortcutControllers, "shortcutControllers");
			this.shortcutControllers.Click += new System.EventHandler(this.shortcutControllers_Click);
			// 
			// shortcutPersonnel
			// 
			this.shortcutPersonnel.Name = "shortcutPersonnel";
			resources.ApplyResources(this.shortcutPersonnel, "shortcutPersonnel");
			this.shortcutPersonnel.Click += new System.EventHandler(this.shortcutPersonnel_Click);
			// 
			// shortcutPrivilege
			// 
			this.shortcutPrivilege.Name = "shortcutPrivilege";
			resources.ApplyResources(this.shortcutPrivilege, "shortcutPrivilege");
			this.shortcutPrivilege.Click += new System.EventHandler(this.shortcutPrivilege_Click);
			// 
			// shortcutConsole
			// 
			this.shortcutConsole.Name = "shortcutConsole";
			resources.ApplyResources(this.shortcutConsole, "shortcutConsole");
			this.shortcutConsole.Click += new System.EventHandler(this.shortcutConsole_Click);
			// 
			// shortcutSwipe
			// 
			this.shortcutSwipe.Name = "shortcutSwipe";
			resources.ApplyResources(this.shortcutSwipe, "shortcutSwipe");
			this.shortcutSwipe.Click += new System.EventHandler(this.shortcutSwipe_Click);
			// 
			// shortcutAttendance
			// 
			this.shortcutAttendance.Name = "shortcutAttendance";
			resources.ApplyResources(this.shortcutAttendance, "shortcutAttendance");
			this.shortcutAttendance.Click += new System.EventHandler(this.shortcutAttendance_Click);
			// 
			// toolStrip1BookMark
			// 
			resources.ApplyResources(this.toolStrip1BookMark, "toolStrip1BookMark");
			this.toolStrip1BookMark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			this.toolStrip1BookMark.ContextMenuStrip = this.contextMenuStrip3Normal;
			this.toolStrip1BookMark.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1BookMark.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBookmark1,
            this.toolStripButtonBookmark2,
            this.toolStripButton4,
            this.toolStripButton3,
            this.toolStripButton2,
            this.toolStripButton1,
            this.toolStripButton5,
            this.toolStripButton7,
            this.toolStripButton6,
            this.toolStripButton14,
            this.toolStripButton13,
            this.toolStripButton12,
            this.toolStripButton11,
            this.toolStripButton10,
            this.toolStripButton9,
            this.toolStripButton8,
            this.toolStripButtonBookmark3});
			this.toolStrip1BookMark.Name = "toolStrip1BookMark";
			this.toolStrip1BookMark.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolTip1.SetToolTip(this.toolStrip1BookMark, resources.GetString("toolStrip1BookMark.ToolTip"));
			this.toolStrip1BookMark.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1BookMark_ItemClicked);
			// 
			// toolStripButtonBookmark1
			// 
			this.toolStripButtonBookmark1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButtonBookmark1, "toolStripButtonBookmark1");
			this.toolStripButtonBookmark1.ForeColor = System.Drawing.Color.White;
			this.toolStripButtonBookmark1.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButtonBookmark1.Name = "toolStripButtonBookmark1";
			this.toolStripButtonBookmark1.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButtonBookmark1.Tag = "WG3000_COMM.Basic.frmControllers";
			// 
			// toolStripButtonBookmark2
			// 
			this.toolStripButtonBookmark2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButtonBookmark2, "toolStripButtonBookmark2");
			this.toolStripButtonBookmark2.ForeColor = System.Drawing.Color.White;
			this.toolStripButtonBookmark2.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButtonBookmark2.Name = "toolStripButtonBookmark2";
			this.toolStripButtonBookmark2.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButtonBookmark2.Tag = "WG3000_COMM.Basic.frmDepartments";
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
			this.toolStripButton4.ForeColor = System.Drawing.Color.White;
			this.toolStripButton4.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton4.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
			this.toolStripButton3.ForeColor = System.Drawing.Color.White;
			this.toolStripButton3.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton3.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
			this.toolStripButton2.ForeColor = System.Drawing.Color.White;
			this.toolStripButton2.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton2.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
			this.toolStripButton1.ForeColor = System.Drawing.Color.White;
			this.toolStripButton1.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton1.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
			this.toolStripButton5.ForeColor = System.Drawing.Color.White;
			this.toolStripButton5.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton5.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton7
			// 
			this.toolStripButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton7, "toolStripButton7");
			this.toolStripButton7.ForeColor = System.Drawing.Color.White;
			this.toolStripButton7.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton7.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
			this.toolStripButton6.ForeColor = System.Drawing.Color.White;
			this.toolStripButton6.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton6.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton14
			// 
			this.toolStripButton14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton14, "toolStripButton14");
			this.toolStripButton14.ForeColor = System.Drawing.Color.White;
			this.toolStripButton14.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton14.Name = "toolStripButton14";
			this.toolStripButton14.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton14.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton13
			// 
			this.toolStripButton13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton13, "toolStripButton13");
			this.toolStripButton13.ForeColor = System.Drawing.Color.White;
			this.toolStripButton13.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton13.Name = "toolStripButton13";
			this.toolStripButton13.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton13.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton12
			// 
			this.toolStripButton12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton12, "toolStripButton12");
			this.toolStripButton12.ForeColor = System.Drawing.Color.White;
			this.toolStripButton12.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton12.Name = "toolStripButton12";
			this.toolStripButton12.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton12.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton11
			// 
			this.toolStripButton11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton11, "toolStripButton11");
			this.toolStripButton11.ForeColor = System.Drawing.Color.White;
			this.toolStripButton11.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton11.Name = "toolStripButton11";
			this.toolStripButton11.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton11.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton10
			// 
			this.toolStripButton10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton10, "toolStripButton10");
			this.toolStripButton10.ForeColor = System.Drawing.Color.White;
			this.toolStripButton10.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton10.Name = "toolStripButton10";
			this.toolStripButton10.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton10.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton9
			// 
			this.toolStripButton9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton9, "toolStripButton9");
			this.toolStripButton9.ForeColor = System.Drawing.Color.White;
			this.toolStripButton9.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton9.Name = "toolStripButton9";
			this.toolStripButton9.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton9.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButton8
			// 
			this.toolStripButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButton8, "toolStripButton8");
			this.toolStripButton8.ForeColor = System.Drawing.Color.White;
			this.toolStripButton8.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButton8.Name = "toolStripButton8";
			this.toolStripButton8.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButton8.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// toolStripButtonBookmark3
			// 
			this.toolStripButtonBookmark3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(166)))), ((int)(((byte)(171)))));
			resources.ApplyResources(this.toolStripButtonBookmark3, "toolStripButtonBookmark3");
			this.toolStripButtonBookmark3.ForeColor = System.Drawing.Color.White;
			this.toolStripButtonBookmark3.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripButtonBookmark3.Name = "toolStripButtonBookmark3";
			this.toolStripButtonBookmark3.Padding = new System.Windows.Forms.Padding(5);
			this.toolStripButtonBookmark3.Tag = "WG3000_COMM.Basic.frmUsers";
			// 
			// panel2Content
			// 
			this.panel2Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(92)))), ((int)(((byte)(120)))));
			this.panel2Content.BackgroundImage = global::Properties.Resources.pMain_content;
			resources.ApplyResources(this.panel2Content, "panel2Content");
			this.panel2Content.Controls.Add(this.panel1);
			this.panel2Content.Controls.Add(this.toolStrip1BookMark);
			this.panel2Content.Controls.Add(this.stbRunInfo);
			this.panel2Content.Controls.Add(this.panel4Form);
			this.panel2Content.Name = "panel2Content";
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			this.panel1.Controls.Add(this.btnIconBasicConfig);
			this.panel1.Controls.Add(this.btnIconBasicOperate);
			this.panel1.Controls.Add(this.btnIconAttendance);
			this.panel1.Controls.Add(this.btnIconTools);
			this.panel1.Controls.Add(this.btnIconHelp);
			this.panel1.Name = "panel1";
			// 
			// btnIconBasicConfig
			// 
			this.btnIconBasicConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			this.btnIconBasicConfig.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(this.btnIconBasicConfig, "btnIconBasicConfig");
			this.btnIconBasicConfig.Focusable = false;
			this.btnIconBasicConfig.ForeColor = System.Drawing.Color.White;
			this.btnIconBasicConfig.Name = "btnIconBasicConfig";
			this.btnIconBasicConfig.TabStop = false;
			this.btnIconBasicConfig.Tag = "BasciConfig";
			this.btnIconBasicConfig.Toggled = false;
			this.btnIconBasicConfig.UseVisualStyleBackColor = false;
			this.btnIconBasicConfig.Click += new System.EventHandler(this.btnIconBasicConfig_Click);
			// 
			// btnIconBasicOperate
			// 
			this.btnIconBasicOperate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			this.btnIconBasicOperate.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(this.btnIconBasicOperate, "btnIconBasicOperate");
			this.btnIconBasicOperate.Focusable = false;
			this.btnIconBasicOperate.ForeColor = System.Drawing.Color.White;
			this.btnIconBasicOperate.Name = "btnIconBasicOperate";
			this.btnIconBasicOperate.TabStop = false;
			this.btnIconBasicOperate.Tag = "BasicOperate";
			this.btnIconBasicOperate.Toggled = false;
			this.btnIconBasicOperate.UseVisualStyleBackColor = false;
			this.btnIconBasicOperate.Click += new System.EventHandler(this.btnIconBasicConfig_Click);
			// 
			// btnIconAttendance
			// 
			this.btnIconAttendance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			this.btnIconAttendance.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(this.btnIconAttendance, "btnIconAttendance");
			this.btnIconAttendance.Focusable = false;
			this.btnIconAttendance.ForeColor = System.Drawing.Color.White;
			this.btnIconAttendance.Name = "btnIconAttendance";
			this.btnIconAttendance.TabStop = false;
			this.btnIconAttendance.Tag = "Attendance";
			this.btnIconAttendance.Toggled = false;
			this.btnIconAttendance.UseVisualStyleBackColor = false;
			this.btnIconAttendance.Click += new System.EventHandler(this.btnIconBasicConfig_Click);
			// 
			// btnIconTools
			// 
			this.btnIconTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			this.btnIconTools.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(this.btnIconTools, "btnIconTools");
			this.btnIconTools.Focusable = false;
			this.btnIconTools.ForeColor = System.Drawing.Color.White;
			this.btnIconTools.Name = "btnIconTools";
			this.btnIconTools.TabStop = false;
			this.btnIconTools.Tag = "Tools";
			this.btnIconTools.Toggled = false;
			this.btnIconTools.UseVisualStyleBackColor = false;
			this.btnIconTools.Click += new System.EventHandler(this.btnIconBasicConfig_Click);
			// 
			// btnIconHelp
			// 
			this.btnIconHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			this.btnIconHelp.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(this.btnIconHelp, "btnIconHelp");
			this.btnIconHelp.Focusable = false;
			this.btnIconHelp.ForeColor = System.Drawing.Color.White;
			this.btnIconHelp.Name = "btnIconHelp";
			this.btnIconHelp.TabStop = false;
			this.btnIconHelp.Tag = "Help";
			this.btnIconHelp.Toggled = false;
			this.btnIconHelp.UseVisualStyleBackColor = false;
			this.btnIconHelp.Click += new System.EventHandler(this.btnIconBasicConfig_Click);
			// 
			// stbRunInfo
			// 
			this.stbRunInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
			this.stbRunInfo.BackgroundImage = global::Properties.Resources.pMain_bottom;
			resources.ApplyResources(this.stbRunInfo, "stbRunInfo");
			this.stbRunInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statOperator,
            this.statSoftwareVer,
            this.statCOM,
            this.statRuninfo1,
            this.statRuninfo2,
            this.statRuninfo3,
            this.statRuninfoLoadedNum,
            this.statTimeDate});
			this.stbRunInfo.Name = "stbRunInfo";
			// 
			// statOperator
			// 
			this.statOperator.BackColor = System.Drawing.Color.Transparent;
			this.statOperator.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.statOperator.ForeColor = System.Drawing.Color.White;
			this.statOperator.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
			this.statOperator.Name = "statOperator";
			resources.ApplyResources(this.statOperator, "statOperator");
			// 
			// statSoftwareVer
			// 
			this.statSoftwareVer.BackColor = System.Drawing.Color.Transparent;
			this.statSoftwareVer.ForeColor = System.Drawing.Color.White;
			this.statSoftwareVer.Name = "statSoftwareVer";
			resources.ApplyResources(this.statSoftwareVer, "statSoftwareVer");
			// 
			// statCOM
			// 
			this.statCOM.BackColor = System.Drawing.Color.Transparent;
			this.statCOM.ForeColor = System.Drawing.Color.White;
			this.statCOM.Name = "statCOM";
			resources.ApplyResources(this.statCOM, "statCOM");
			// 
			// statRuninfo1
			// 
			this.statRuninfo1.BackColor = System.Drawing.Color.Transparent;
			this.statRuninfo1.ForeColor = System.Drawing.Color.White;
			this.statRuninfo1.Name = "statRuninfo1";
			resources.ApplyResources(this.statRuninfo1, "statRuninfo1");
			this.statRuninfo1.Spring = true;
			// 
			// statRuninfo2
			// 
			this.statRuninfo2.BackColor = System.Drawing.Color.Transparent;
			this.statRuninfo2.ForeColor = System.Drawing.Color.White;
			this.statRuninfo2.Name = "statRuninfo2";
			resources.ApplyResources(this.statRuninfo2, "statRuninfo2");
			// 
			// statRuninfo3
			// 
			this.statRuninfo3.BackColor = System.Drawing.Color.Transparent;
			this.statRuninfo3.ForeColor = System.Drawing.Color.White;
			this.statRuninfo3.Name = "statRuninfo3";
			resources.ApplyResources(this.statRuninfo3, "statRuninfo3");
			// 
			// statRuninfoLoadedNum
			// 
			this.statRuninfoLoadedNum.BackColor = System.Drawing.Color.Transparent;
			this.statRuninfoLoadedNum.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statRuninfoLoadedNum.ForeColor = System.Drawing.Color.White;
			this.statRuninfoLoadedNum.Name = "statRuninfoLoadedNum";
			resources.ApplyResources(this.statRuninfoLoadedNum, "statRuninfoLoadedNum");
			// 
			// statTimeDate
			// 
			this.statTimeDate.BackColor = System.Drawing.Color.Transparent;
			this.statTimeDate.ForeColor = System.Drawing.Color.White;
			this.statTimeDate.Image = global::Properties.Resources.timequery;
			this.statTimeDate.Name = "statTimeDate";
			resources.ApplyResources(this.statTimeDate, "statTimeDate");
			// 
			// panel4Form
			// 
			resources.ApplyResources(this.panel4Form, "panel4Form");
			this.panel4Form.BackColor = System.Drawing.Color.Transparent;
			this.panel4Form.Name = "panel4Form";
			this.panel4Form.TabStop = false;
			// 
			// frmADCT3000
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(92)))), ((int)(((byte)(120)))));
			this.Controls.Add(this.panel2Content);
			this.Name = "frmADCT3000";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmADCT3000_FormClosing);
			this.Load += new System.EventHandler(this.frmADCT3000_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmADCT3000_KeyDown);
			this.contextMenuStrip3Normal.ResumeLayout(false);
			this.toolStrip1BookMark.ResumeLayout(false);
			this.toolStrip1BookMark.PerformLayout();
			this.panel2Content.ResumeLayout(false);
			this.panel2Content.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.stbRunInfo.ResumeLayout(false);
			this.stbRunInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.panel4Form)).EndInit();
			this.ResumeLayout(false);

		}
        private Panel panel1;
        private ToggleButton btnIconTools;
        private ToggleButton btnIconHelp;
        private ToolStripButton toolStripButton14;
        private ToolStripButton toolStripButton13;
        private ToolStripButton toolStripButton12;
        private ToolStripButton toolStripButton11;
        private ToolStripButton toolStripButton10;
        private ToolStripButton toolStripButton9;
		private ToolStripButton toolStripButton8;
    }
}

