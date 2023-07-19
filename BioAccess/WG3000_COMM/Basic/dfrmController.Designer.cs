namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmController
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
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmController));
            this.grpbDoorReader = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnect4_4 = new System.Windows.Forms.ImageButton();
            this.btnConnect4_3 = new System.Windows.Forms.ImageButton();
            this.btnConnect4_2 = new System.Windows.Forms.ImageButton();
            this.btnConnect4_1 = new System.Windows.Forms.ImageButton();
            this.label21 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.optDutyOff1D = new System.Windows.Forms.RadioButton();
            this.optDutyOn1D = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff1D = new System.Windows.Forms.RadioButton();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.optDutyOff2D = new System.Windows.Forms.RadioButton();
            this.optDutyOn2D = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff2D = new System.Windows.Forms.RadioButton();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.optNC1D = new System.Windows.Forms.RadioButton();
            this.optNO1D = new System.Windows.Forms.RadioButton();
            this.optOnline1D = new System.Windows.Forms.RadioButton();
            this.chkCamera4_4 = new System.Windows.Forms.CheckBox();
            this.chkCamera4_3 = new System.Windows.Forms.CheckBox();
            this.chkCamera4_2 = new System.Windows.Forms.CheckBox();
            this.chkCamera4_1 = new System.Windows.Forms.CheckBox();
            this.nudDoorDelay4D = new System.Windows.Forms.NumericUpDown();
            this.nudDoorDelay3D = new System.Windows.Forms.NumericUpDown();
            this.nudDoorDelay2D = new System.Windows.Forms.NumericUpDown();
            this.nudDoorDelay1D = new System.Windows.Forms.NumericUpDown();
            this.chkDoorActive4D = new System.Windows.Forms.CheckBox();
            this.txtDoorName4D = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkDoorActive3D = new System.Windows.Forms.CheckBox();
            this.txtDoorName3D = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.chkAttend4D = new System.Windows.Forms.CheckBox();
            this.txtReaderName4D = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkAttend3D = new System.Windows.Forms.CheckBox();
            this.txtReaderName3D = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.chkAttend2D = new System.Windows.Forms.CheckBox();
            this.txtReaderName2D = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.chkDoorActive2D = new System.Windows.Forms.CheckBox();
            this.txtDoorName2D = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.optNO2D = new System.Windows.Forms.RadioButton();
            this.optOnline2D = new System.Windows.Forms.RadioButton();
            this.optNC2D = new System.Windows.Forms.RadioButton();
            this.chkAttend1D = new System.Windows.Forms.CheckBox();
            this.txtReaderName1D = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.chkDoorActive1D = new System.Windows.Forms.CheckBox();
            this.txtDoorName1D = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.optNC3D = new System.Windows.Forms.RadioButton();
            this.optNO3D = new System.Windows.Forms.RadioButton();
            this.optOnline3D = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.optNC4D = new System.Windows.Forms.RadioButton();
            this.optNO4D = new System.Windows.Forms.RadioButton();
            this.optOnline4D = new System.Windows.Forms.RadioButton();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.optDutyOff3D = new System.Windows.Forms.RadioButton();
            this.optDutyOn3D = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff3D = new System.Windows.Forms.RadioButton();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.optDutyOff4D = new System.Windows.Forms.RadioButton();
            this.optDutyOn4D = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff4D = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.btnConnect2_4 = new System.Windows.Forms.ImageButton();
            this.btnConnect2_3 = new System.Windows.Forms.ImageButton();
            this.btnConnect2_2 = new System.Windows.Forms.ImageButton();
            this.btnConnect2_1 = new System.Windows.Forms.ImageButton();
            this.chkCamera2_2 = new System.Windows.Forms.CheckBox();
            this.chkCamera2_1 = new System.Windows.Forms.CheckBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.optNC1B = new System.Windows.Forms.RadioButton();
            this.optNO1B = new System.Windows.Forms.RadioButton();
            this.optOnline1B = new System.Windows.Forms.RadioButton();
            this.nudDoorDelay2B = new System.Windows.Forms.NumericUpDown();
            this.nudDoorDelay1B = new System.Windows.Forms.NumericUpDown();
            this.label39 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.optDutyOff4B = new System.Windows.Forms.RadioButton();
            this.optDutyOn4B = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff4B = new System.Windows.Forms.RadioButton();
            this.chkAttend4B = new System.Windows.Forms.CheckBox();
            this.txtReaderName4B = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.optDutyOff3B = new System.Windows.Forms.RadioButton();
            this.optDutyOn3B = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff3B = new System.Windows.Forms.RadioButton();
            this.chkAttend3B = new System.Windows.Forms.CheckBox();
            this.txtReaderName3B = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.optDutyOff2B = new System.Windows.Forms.RadioButton();
            this.optDutyOn2B = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff2B = new System.Windows.Forms.RadioButton();
            this.chkAttend2B = new System.Windows.Forms.CheckBox();
            this.txtReaderName2B = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.chkDoorActive2B = new System.Windows.Forms.CheckBox();
            this.txtDoorName2B = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.optNC2B = new System.Windows.Forms.RadioButton();
            this.optNO2B = new System.Windows.Forms.RadioButton();
            this.optOnline2B = new System.Windows.Forms.RadioButton();
            this.gpbAttend1B = new System.Windows.Forms.GroupBox();
            this.optDutyOff1B = new System.Windows.Forms.RadioButton();
            this.optDutyOn1B = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff1B = new System.Windows.Forms.RadioButton();
            this.chkAttend1B = new System.Windows.Forms.CheckBox();
            this.txtReaderName1B = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.chkDoorActive1B = new System.Windows.Forms.CheckBox();
            this.txtDoorName1B = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.optDutyOff1A = new System.Windows.Forms.RadioButton();
            this.optDutyOn1A = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff1A = new System.Windows.Forms.RadioButton();
            this.btnConnect1_2 = new System.Windows.Forms.ImageButton();
            this.btnConnect1_1 = new System.Windows.Forms.ImageButton();
            this.chkCamera1_1 = new System.Windows.Forms.CheckBox();
            this.nudDoorDelay1A = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.optDutyOff2A = new System.Windows.Forms.RadioButton();
            this.optDutyOn2A = new System.Windows.Forms.RadioButton();
            this.optDutyOnOff2A = new System.Windows.Forms.RadioButton();
            this.chkAttend2A = new System.Windows.Forms.CheckBox();
            this.txtReaderName2A = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.chkAttend1A = new System.Windows.Forms.CheckBox();
            this.txtReaderName1A = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.chkDoorActive1A = new System.Windows.Forms.CheckBox();
            this.txtDoorName1A = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.optNC1A = new System.Windows.Forms.RadioButton();
            this.optNO1A = new System.Windows.Forms.RadioButton();
            this.optOnline1A = new System.Windows.Forms.RadioButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkFaceIdent = new System.Windows.Forms.CheckBox();
            this.chkFpIdent = new System.Windows.Forms.CheckBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.nudVolume = new System.Windows.Forms.NumericUpDown();
            this.chkUseM1Card = new System.Windows.Forms.CheckBox();
            this.chkUseTouchSensor = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.grpbController = new System.Windows.Forms.GroupBox();
            this.chkWiFiComm = new System.Windows.Forms.CheckBox();
            this.grpWiFi = new System.Windows.Forms.GroupBox();
            this.lblWiFiIP = new System.Windows.Forms.Label();
            this.txtWiFiIP = new System.Windows.Forms.TextBox();
            this.txtWiFiKey = new System.Windows.Forms.TextBox();
            this.txtWiFiSSID = new System.Windows.Forms.TextBox();
            this.lblWiFiSSID = new System.Windows.Forms.Label();
            this.lblWiFiKey = new System.Windows.Forms.Label();
            this.chkWiFiDhcp = new System.Windows.Forms.CheckBox();
            this.lblWiFiPort = new System.Windows.Forms.Label();
            this.nudWiFiPort = new System.Windows.Forms.NumericUpDown();
            this.lblWiFiGateway = new System.Windows.Forms.Label();
            this.txtWiFiGateway = new System.Windows.Forms.TextBox();
            this.lblWiFiMask = new System.Windows.Forms.Label();
            this.txtWiFiMask = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnZoneManage = new System.Windows.Forms.ImageButton();
            this.cbof_Zone = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.grpbIP = new System.Windows.Forms.GroupBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.txtControllerIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkControllerActive = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.mtxtbControllerNO = new System.Windows.Forms.MaskedTextBox();
            this.mtxtbControllerSN = new System.Windows.Forms.MaskedTextBox();
            this.optIPLarge = new System.Windows.Forms.RadioButton();
            this.optIPSmall = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.ImageButton();
            this.btnCancel2 = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrev = new System.Windows.Forms.ImageButton();
            this.grpbDoorReader.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay4D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay3D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay2D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay1D)).BeginInit();
            this.groupBox11.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay2B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay1B)).BeginInit();
            this.groupBox16.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.gpbAttend1B.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay1A)).BeginInit();
            this.groupBox14.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).BeginInit();
            this.grpbController.SuspendLayout();
            this.grpWiFi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWiFiPort)).BeginInit();
            this.grpbIP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpbDoorReader
            // 
            this.grpbDoorReader.BackColor = System.Drawing.Color.Transparent;
            this.grpbDoorReader.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.grpbDoorReader, "grpbDoorReader");
            this.grpbDoorReader.Name = "grpbDoorReader";
            this.grpbDoorReader.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.tabPage1.Name = "tabPage1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnect4_4);
            this.groupBox1.Controls.Add(this.btnConnect4_3);
            this.groupBox1.Controls.Add(this.btnConnect4_2);
            this.groupBox1.Controls.Add(this.btnConnect4_1);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.groupBox12);
            this.groupBox1.Controls.Add(this.groupBox10);
            this.groupBox1.Controls.Add(this.groupBox13);
            this.groupBox1.Controls.Add(this.chkCamera4_4);
            this.groupBox1.Controls.Add(this.chkCamera4_3);
            this.groupBox1.Controls.Add(this.chkCamera4_2);
            this.groupBox1.Controls.Add(this.chkCamera4_1);
            this.groupBox1.Controls.Add(this.nudDoorDelay4D);
            this.groupBox1.Controls.Add(this.nudDoorDelay3D);
            this.groupBox1.Controls.Add(this.nudDoorDelay2D);
            this.groupBox1.Controls.Add(this.nudDoorDelay1D);
            this.groupBox1.Controls.Add(this.chkDoorActive4D);
            this.groupBox1.Controls.Add(this.txtDoorName4D);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.chkDoorActive3D);
            this.groupBox1.Controls.Add(this.txtDoorName3D);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.chkAttend4D);
            this.groupBox1.Controls.Add(this.txtReaderName4D);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.chkAttend3D);
            this.groupBox1.Controls.Add(this.txtReaderName3D);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.chkAttend2D);
            this.groupBox1.Controls.Add(this.txtReaderName2D);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.chkDoorActive2D);
            this.groupBox1.Controls.Add(this.txtDoorName2D);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.groupBox11);
            this.groupBox1.Controls.Add(this.chkAttend1D);
            this.groupBox1.Controls.Add(this.txtReaderName1D);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.chkDoorActive1D);
            this.groupBox1.Controls.Add(this.txtDoorName1D);
            this.groupBox1.Controls.Add(this.label40);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox8);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnConnect4_4
            // 
            this.btnConnect4_4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect4_4, "btnConnect4_4");
            this.btnConnect4_4.Focusable = true;
            this.btnConnect4_4.ForeColor = System.Drawing.Color.White;
            this.btnConnect4_4.Name = "btnConnect4_4";
            this.btnConnect4_4.Toggle = false;
            this.btnConnect4_4.UseVisualStyleBackColor = false;
            this.btnConnect4_4.Click += new System.EventHandler(this.btnConnect4_4_Click);
            // 
            // btnConnect4_3
            // 
            this.btnConnect4_3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect4_3, "btnConnect4_3");
            this.btnConnect4_3.Focusable = true;
            this.btnConnect4_3.ForeColor = System.Drawing.Color.White;
            this.btnConnect4_3.Name = "btnConnect4_3";
            this.btnConnect4_3.Toggle = false;
            this.btnConnect4_3.UseVisualStyleBackColor = false;
            this.btnConnect4_3.Click += new System.EventHandler(this.btnConnect4_3_Click);
            // 
            // btnConnect4_2
            // 
            this.btnConnect4_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect4_2, "btnConnect4_2");
            this.btnConnect4_2.Focusable = true;
            this.btnConnect4_2.ForeColor = System.Drawing.Color.White;
            this.btnConnect4_2.Name = "btnConnect4_2";
            this.btnConnect4_2.Toggle = false;
            this.btnConnect4_2.UseVisualStyleBackColor = false;
            this.btnConnect4_2.Click += new System.EventHandler(this.btnConnect4_2_Click);
            // 
            // btnConnect4_1
            // 
            this.btnConnect4_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect4_1, "btnConnect4_1");
            this.btnConnect4_1.Focusable = true;
            this.btnConnect4_1.ForeColor = System.Drawing.Color.White;
            this.btnConnect4_1.Name = "btnConnect4_1";
            this.btnConnect4_1.Toggle = false;
            this.btnConnect4_1.UseVisualStyleBackColor = false;
            this.btnConnect4_1.Click += new System.EventHandler(this.btnConnect4_1_Click);
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.optDutyOff1D);
            this.groupBox12.Controls.Add(this.optDutyOn1D);
            this.groupBox12.Controls.Add(this.optDutyOnOff1D);
            resources.ApplyResources(this.groupBox12, "groupBox12");
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.TabStop = false;
            // 
            // optDutyOff1D
            // 
            resources.ApplyResources(this.optDutyOff1D, "optDutyOff1D");
            this.optDutyOff1D.Name = "optDutyOff1D";
            this.optDutyOff1D.UseVisualStyleBackColor = true;
            // 
            // optDutyOn1D
            // 
            resources.ApplyResources(this.optDutyOn1D, "optDutyOn1D");
            this.optDutyOn1D.Name = "optDutyOn1D";
            this.optDutyOn1D.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff1D
            // 
            resources.ApplyResources(this.optDutyOnOff1D, "optDutyOnOff1D");
            this.optDutyOnOff1D.Checked = true;
            this.optDutyOnOff1D.Name = "optDutyOnOff1D";
            this.optDutyOnOff1D.TabStop = true;
            this.optDutyOnOff1D.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.optDutyOff2D);
            this.groupBox10.Controls.Add(this.optDutyOn2D);
            this.groupBox10.Controls.Add(this.optDutyOnOff2D);
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // optDutyOff2D
            // 
            resources.ApplyResources(this.optDutyOff2D, "optDutyOff2D");
            this.optDutyOff2D.Name = "optDutyOff2D";
            this.optDutyOff2D.UseVisualStyleBackColor = true;
            // 
            // optDutyOn2D
            // 
            resources.ApplyResources(this.optDutyOn2D, "optDutyOn2D");
            this.optDutyOn2D.Name = "optDutyOn2D";
            this.optDutyOn2D.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff2D
            // 
            resources.ApplyResources(this.optDutyOnOff2D, "optDutyOnOff2D");
            this.optDutyOnOff2D.Checked = true;
            this.optDutyOnOff2D.Name = "optDutyOnOff2D";
            this.optDutyOnOff2D.TabStop = true;
            this.optDutyOnOff2D.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.optNC1D);
            this.groupBox13.Controls.Add(this.optNO1D);
            this.groupBox13.Controls.Add(this.optOnline1D);
            resources.ApplyResources(this.groupBox13, "groupBox13");
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.TabStop = false;
            // 
            // optNC1D
            // 
            resources.ApplyResources(this.optNC1D, "optNC1D");
            this.optNC1D.Name = "optNC1D";
            this.optNC1D.UseVisualStyleBackColor = true;
            // 
            // optNO1D
            // 
            resources.ApplyResources(this.optNO1D, "optNO1D");
            this.optNO1D.Name = "optNO1D";
            this.optNO1D.UseVisualStyleBackColor = true;
            // 
            // optOnline1D
            // 
            resources.ApplyResources(this.optOnline1D, "optOnline1D");
            this.optOnline1D.Checked = true;
            this.optOnline1D.Name = "optOnline1D";
            this.optOnline1D.TabStop = true;
            this.optOnline1D.UseVisualStyleBackColor = true;
            // 
            // chkCamera4_4
            // 
            resources.ApplyResources(this.chkCamera4_4, "chkCamera4_4");
            this.chkCamera4_4.Name = "chkCamera4_4";
            this.chkCamera4_4.UseVisualStyleBackColor = true;
            // 
            // chkCamera4_3
            // 
            resources.ApplyResources(this.chkCamera4_3, "chkCamera4_3");
            this.chkCamera4_3.Name = "chkCamera4_3";
            this.chkCamera4_3.UseVisualStyleBackColor = true;
            // 
            // chkCamera4_2
            // 
            resources.ApplyResources(this.chkCamera4_2, "chkCamera4_2");
            this.chkCamera4_2.Name = "chkCamera4_2";
            this.chkCamera4_2.UseVisualStyleBackColor = true;
            // 
            // chkCamera4_1
            // 
            resources.ApplyResources(this.chkCamera4_1, "chkCamera4_1");
            this.chkCamera4_1.Name = "chkCamera4_1";
            this.chkCamera4_1.UseVisualStyleBackColor = true;
            // 
            // nudDoorDelay4D
            // 
            resources.ApplyResources(this.nudDoorDelay4D, "nudDoorDelay4D");
            this.nudDoorDelay4D.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay4D.Name = "nudDoorDelay4D";
            this.nudDoorDelay4D.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudDoorDelay3D
            // 
            resources.ApplyResources(this.nudDoorDelay3D, "nudDoorDelay3D");
            this.nudDoorDelay3D.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay3D.Name = "nudDoorDelay3D";
            this.nudDoorDelay3D.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudDoorDelay2D
            // 
            resources.ApplyResources(this.nudDoorDelay2D, "nudDoorDelay2D");
            this.nudDoorDelay2D.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay2D.Name = "nudDoorDelay2D";
            this.nudDoorDelay2D.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudDoorDelay1D
            // 
            resources.ApplyResources(this.nudDoorDelay1D, "nudDoorDelay1D");
            this.nudDoorDelay1D.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay1D.Name = "nudDoorDelay1D";
            this.nudDoorDelay1D.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // chkDoorActive4D
            // 
            resources.ApplyResources(this.chkDoorActive4D, "chkDoorActive4D");
            this.chkDoorActive4D.Checked = true;
            this.chkDoorActive4D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive4D.Name = "chkDoorActive4D";
            this.chkDoorActive4D.UseVisualStyleBackColor = true;
            // 
            // txtDoorName4D
            // 
            resources.ApplyResources(this.txtDoorName4D, "txtDoorName4D");
            this.txtDoorName4D.Name = "txtDoorName4D";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // chkDoorActive3D
            // 
            resources.ApplyResources(this.chkDoorActive3D, "chkDoorActive3D");
            this.chkDoorActive3D.Checked = true;
            this.chkDoorActive3D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive3D.Name = "chkDoorActive3D";
            this.chkDoorActive3D.UseVisualStyleBackColor = true;
            // 
            // txtDoorName3D
            // 
            resources.ApplyResources(this.txtDoorName3D, "txtDoorName3D");
            this.txtDoorName3D.Name = "txtDoorName3D";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // chkAttend4D
            // 
            resources.ApplyResources(this.chkAttend4D, "chkAttend4D");
            this.chkAttend4D.Checked = true;
            this.chkAttend4D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend4D.Name = "chkAttend4D";
            this.chkAttend4D.UseVisualStyleBackColor = true;
            // 
            // txtReaderName4D
            // 
            resources.ApplyResources(this.txtReaderName4D, "txtReaderName4D");
            this.txtReaderName4D.Name = "txtReaderName4D";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // chkAttend3D
            // 
            resources.ApplyResources(this.chkAttend3D, "chkAttend3D");
            this.chkAttend3D.Checked = true;
            this.chkAttend3D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend3D.Name = "chkAttend3D";
            this.chkAttend3D.UseVisualStyleBackColor = true;
            // 
            // txtReaderName3D
            // 
            resources.ApplyResources(this.txtReaderName3D, "txtReaderName3D");
            this.txtReaderName3D.Name = "txtReaderName3D";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // chkAttend2D
            // 
            resources.ApplyResources(this.chkAttend2D, "chkAttend2D");
            this.chkAttend2D.Checked = true;
            this.chkAttend2D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend2D.Name = "chkAttend2D";
            this.chkAttend2D.UseVisualStyleBackColor = true;
            // 
            // txtReaderName2D
            // 
            resources.ApplyResources(this.txtReaderName2D, "txtReaderName2D");
            this.txtReaderName2D.Name = "txtReaderName2D";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // chkDoorActive2D
            // 
            resources.ApplyResources(this.chkDoorActive2D, "chkDoorActive2D");
            this.chkDoorActive2D.Checked = true;
            this.chkDoorActive2D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive2D.Name = "chkDoorActive2D";
            this.chkDoorActive2D.UseVisualStyleBackColor = true;
            // 
            // txtDoorName2D
            // 
            resources.ApplyResources(this.txtDoorName2D, "txtDoorName2D");
            this.txtDoorName2D.Name = "txtDoorName2D";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.optNO2D);
            this.groupBox11.Controls.Add(this.optOnline2D);
            this.groupBox11.Controls.Add(this.optNC2D);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // optNO2D
            // 
            resources.ApplyResources(this.optNO2D, "optNO2D");
            this.optNO2D.Name = "optNO2D";
            this.optNO2D.UseVisualStyleBackColor = true;
            // 
            // optOnline2D
            // 
            resources.ApplyResources(this.optOnline2D, "optOnline2D");
            this.optOnline2D.Checked = true;
            this.optOnline2D.Name = "optOnline2D";
            this.optOnline2D.TabStop = true;
            this.optOnline2D.UseVisualStyleBackColor = true;
            // 
            // optNC2D
            // 
            resources.ApplyResources(this.optNC2D, "optNC2D");
            this.optNC2D.Name = "optNC2D";
            this.optNC2D.UseVisualStyleBackColor = true;
            // 
            // chkAttend1D
            // 
            resources.ApplyResources(this.chkAttend1D, "chkAttend1D");
            this.chkAttend1D.Checked = true;
            this.chkAttend1D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend1D.Name = "chkAttend1D";
            this.chkAttend1D.UseVisualStyleBackColor = true;
            // 
            // txtReaderName1D
            // 
            resources.ApplyResources(this.txtReaderName1D, "txtReaderName1D");
            this.txtReaderName1D.Name = "txtReaderName1D";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // chkDoorActive1D
            // 
            resources.ApplyResources(this.chkDoorActive1D, "chkDoorActive1D");
            this.chkDoorActive1D.Checked = true;
            this.chkDoorActive1D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive1D.Name = "chkDoorActive1D";
            this.chkDoorActive1D.UseVisualStyleBackColor = true;
            // 
            // txtDoorName1D
            // 
            resources.ApplyResources(this.txtDoorName1D, "txtDoorName1D");
            this.txtDoorName1D.Name = "txtDoorName1D";
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // label41
            // 
            resources.ApplyResources(this.label41, "label41");
            this.label41.Name = "label41";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.optNC3D);
            this.groupBox7.Controls.Add(this.optNO3D);
            this.groupBox7.Controls.Add(this.optOnline3D);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // optNC3D
            // 
            resources.ApplyResources(this.optNC3D, "optNC3D");
            this.optNC3D.Name = "optNC3D";
            this.optNC3D.UseVisualStyleBackColor = true;
            // 
            // optNO3D
            // 
            resources.ApplyResources(this.optNO3D, "optNO3D");
            this.optNO3D.Name = "optNO3D";
            this.optNO3D.UseVisualStyleBackColor = true;
            // 
            // optOnline3D
            // 
            resources.ApplyResources(this.optOnline3D, "optOnline3D");
            this.optOnline3D.Checked = true;
            this.optOnline3D.Name = "optOnline3D";
            this.optOnline3D.TabStop = true;
            this.optOnline3D.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.optNC4D);
            this.groupBox6.Controls.Add(this.optNO4D);
            this.groupBox6.Controls.Add(this.optOnline4D);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // optNC4D
            // 
            resources.ApplyResources(this.optNC4D, "optNC4D");
            this.optNC4D.Name = "optNC4D";
            this.optNC4D.UseVisualStyleBackColor = true;
            // 
            // optNO4D
            // 
            resources.ApplyResources(this.optNO4D, "optNO4D");
            this.optNO4D.Name = "optNO4D";
            this.optNO4D.UseVisualStyleBackColor = true;
            // 
            // optOnline4D
            // 
            resources.ApplyResources(this.optOnline4D, "optOnline4D");
            this.optOnline4D.Checked = true;
            this.optOnline4D.Name = "optOnline4D";
            this.optOnline4D.TabStop = true;
            this.optOnline4D.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.optDutyOff3D);
            this.groupBox9.Controls.Add(this.optDutyOn3D);
            this.groupBox9.Controls.Add(this.optDutyOnOff3D);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // optDutyOff3D
            // 
            resources.ApplyResources(this.optDutyOff3D, "optDutyOff3D");
            this.optDutyOff3D.Name = "optDutyOff3D";
            this.optDutyOff3D.UseVisualStyleBackColor = true;
            // 
            // optDutyOn3D
            // 
            resources.ApplyResources(this.optDutyOn3D, "optDutyOn3D");
            this.optDutyOn3D.Name = "optDutyOn3D";
            this.optDutyOn3D.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff3D
            // 
            resources.ApplyResources(this.optDutyOnOff3D, "optDutyOnOff3D");
            this.optDutyOnOff3D.Checked = true;
            this.optDutyOnOff3D.Name = "optDutyOnOff3D";
            this.optDutyOnOff3D.TabStop = true;
            this.optDutyOnOff3D.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.optDutyOff4D);
            this.groupBox8.Controls.Add(this.optDutyOn4D);
            this.groupBox8.Controls.Add(this.optDutyOnOff4D);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // optDutyOff4D
            // 
            resources.ApplyResources(this.optDutyOff4D, "optDutyOff4D");
            this.optDutyOff4D.Name = "optDutyOff4D";
            this.optDutyOff4D.UseVisualStyleBackColor = true;
            // 
            // optDutyOn4D
            // 
            resources.ApplyResources(this.optDutyOn4D, "optDutyOn4D");
            this.optDutyOn4D.Name = "optDutyOn4D";
            this.optDutyOn4D.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff4D
            // 
            resources.ApplyResources(this.optDutyOnOff4D, "optDutyOnOff4D");
            this.optDutyOnOff4D.Checked = true;
            this.optDutyOnOff4D.Name = "optDutyOnOff4D";
            this.optDutyOnOff4D.TabStop = true;
            this.optDutyOnOff4D.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.groupBox15);
            this.tabPage2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox15
            // 
            this.groupBox15.BackColor = System.Drawing.Color.Transparent;
            this.groupBox15.Controls.Add(this.btnConnect2_4);
            this.groupBox15.Controls.Add(this.btnConnect2_3);
            this.groupBox15.Controls.Add(this.btnConnect2_2);
            this.groupBox15.Controls.Add(this.btnConnect2_1);
            this.groupBox15.Controls.Add(this.chkCamera2_2);
            this.groupBox15.Controls.Add(this.chkCamera2_1);
            this.groupBox15.Controls.Add(this.groupBox23);
            this.groupBox15.Controls.Add(this.nudDoorDelay2B);
            this.groupBox15.Controls.Add(this.nudDoorDelay1B);
            this.groupBox15.Controls.Add(this.label39);
            this.groupBox15.Controls.Add(this.label34);
            this.groupBox15.Controls.Add(this.groupBox16);
            this.groupBox15.Controls.Add(this.chkAttend4B);
            this.groupBox15.Controls.Add(this.txtReaderName4B);
            this.groupBox15.Controls.Add(this.label22);
            this.groupBox15.Controls.Add(this.groupBox17);
            this.groupBox15.Controls.Add(this.chkAttend3B);
            this.groupBox15.Controls.Add(this.txtReaderName3B);
            this.groupBox15.Controls.Add(this.label23);
            this.groupBox15.Controls.Add(this.groupBox18);
            this.groupBox15.Controls.Add(this.chkAttend2B);
            this.groupBox15.Controls.Add(this.txtReaderName2B);
            this.groupBox15.Controls.Add(this.label24);
            this.groupBox15.Controls.Add(this.chkDoorActive2B);
            this.groupBox15.Controls.Add(this.txtDoorName2B);
            this.groupBox15.Controls.Add(this.label27);
            this.groupBox15.Controls.Add(this.groupBox21);
            this.groupBox15.Controls.Add(this.gpbAttend1B);
            this.groupBox15.Controls.Add(this.chkAttend1B);
            this.groupBox15.Controls.Add(this.txtReaderName1B);
            this.groupBox15.Controls.Add(this.label28);
            this.groupBox15.Controls.Add(this.label29);
            this.groupBox15.Controls.Add(this.label30);
            this.groupBox15.Controls.Add(this.chkDoorActive1B);
            this.groupBox15.Controls.Add(this.txtDoorName1B);
            this.groupBox15.Controls.Add(this.label31);
            this.groupBox15.Controls.Add(this.label32);
            resources.ApplyResources(this.groupBox15, "groupBox15");
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.TabStop = false;
            // 
            // btnConnect2_4
            // 
            this.btnConnect2_4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect2_4, "btnConnect2_4");
            this.btnConnect2_4.Focusable = true;
            this.btnConnect2_4.ForeColor = System.Drawing.Color.White;
            this.btnConnect2_4.Name = "btnConnect2_4";
            this.btnConnect2_4.Toggle = false;
            this.btnConnect2_4.UseVisualStyleBackColor = false;
            this.btnConnect2_4.Click += new System.EventHandler(this.btnConnect2_4_Click);
            // 
            // btnConnect2_3
            // 
            this.btnConnect2_3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect2_3, "btnConnect2_3");
            this.btnConnect2_3.Focusable = true;
            this.btnConnect2_3.ForeColor = System.Drawing.Color.White;
            this.btnConnect2_3.Name = "btnConnect2_3";
            this.btnConnect2_3.Toggle = false;
            this.btnConnect2_3.UseVisualStyleBackColor = false;
            this.btnConnect2_3.Click += new System.EventHandler(this.btnConnect2_3_Click);
            // 
            // btnConnect2_2
            // 
            this.btnConnect2_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect2_2, "btnConnect2_2");
            this.btnConnect2_2.Focusable = true;
            this.btnConnect2_2.ForeColor = System.Drawing.Color.White;
            this.btnConnect2_2.Name = "btnConnect2_2";
            this.btnConnect2_2.Toggle = false;
            this.btnConnect2_2.UseVisualStyleBackColor = false;
            this.btnConnect2_2.Click += new System.EventHandler(this.btnConnect2_2_Click);
            // 
            // btnConnect2_1
            // 
            this.btnConnect2_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect2_1, "btnConnect2_1");
            this.btnConnect2_1.Focusable = true;
            this.btnConnect2_1.ForeColor = System.Drawing.Color.White;
            this.btnConnect2_1.Name = "btnConnect2_1";
            this.btnConnect2_1.Toggle = false;
            this.btnConnect2_1.UseVisualStyleBackColor = false;
            this.btnConnect2_1.Click += new System.EventHandler(this.btnConnect2_1_Click);
            // 
            // chkCamera2_2
            // 
            resources.ApplyResources(this.chkCamera2_2, "chkCamera2_2");
            this.chkCamera2_2.Name = "chkCamera2_2";
            this.chkCamera2_2.UseVisualStyleBackColor = true;
            // 
            // chkCamera2_1
            // 
            resources.ApplyResources(this.chkCamera2_1, "chkCamera2_1");
            this.chkCamera2_1.Name = "chkCamera2_1";
            this.chkCamera2_1.UseVisualStyleBackColor = true;
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.optNC1B);
            this.groupBox23.Controls.Add(this.optNO1B);
            this.groupBox23.Controls.Add(this.optOnline1B);
            resources.ApplyResources(this.groupBox23, "groupBox23");
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.TabStop = false;
            // 
            // optNC1B
            // 
            resources.ApplyResources(this.optNC1B, "optNC1B");
            this.optNC1B.Name = "optNC1B";
            this.optNC1B.UseVisualStyleBackColor = true;
            // 
            // optNO1B
            // 
            resources.ApplyResources(this.optNO1B, "optNO1B");
            this.optNO1B.Name = "optNO1B";
            this.optNO1B.UseVisualStyleBackColor = true;
            // 
            // optOnline1B
            // 
            resources.ApplyResources(this.optOnline1B, "optOnline1B");
            this.optOnline1B.Checked = true;
            this.optOnline1B.Name = "optOnline1B";
            this.optOnline1B.TabStop = true;
            this.optOnline1B.UseVisualStyleBackColor = true;
            // 
            // nudDoorDelay2B
            // 
            resources.ApplyResources(this.nudDoorDelay2B, "nudDoorDelay2B");
            this.nudDoorDelay2B.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay2B.Name = "nudDoorDelay2B";
            this.nudDoorDelay2B.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudDoorDelay1B
            // 
            resources.ApplyResources(this.nudDoorDelay1B, "nudDoorDelay1B");
            this.nudDoorDelay1B.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay1B.Name = "nudDoorDelay1B";
            this.nudDoorDelay1B.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label39
            // 
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // label34
            // 
            resources.ApplyResources(this.label34, "label34");
            this.label34.Name = "label34";
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.optDutyOff4B);
            this.groupBox16.Controls.Add(this.optDutyOn4B);
            this.groupBox16.Controls.Add(this.optDutyOnOff4B);
            resources.ApplyResources(this.groupBox16, "groupBox16");
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.TabStop = false;
            // 
            // optDutyOff4B
            // 
            resources.ApplyResources(this.optDutyOff4B, "optDutyOff4B");
            this.optDutyOff4B.Name = "optDutyOff4B";
            this.optDutyOff4B.UseVisualStyleBackColor = true;
            // 
            // optDutyOn4B
            // 
            resources.ApplyResources(this.optDutyOn4B, "optDutyOn4B");
            this.optDutyOn4B.Name = "optDutyOn4B";
            this.optDutyOn4B.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff4B
            // 
            resources.ApplyResources(this.optDutyOnOff4B, "optDutyOnOff4B");
            this.optDutyOnOff4B.Checked = true;
            this.optDutyOnOff4B.Name = "optDutyOnOff4B";
            this.optDutyOnOff4B.TabStop = true;
            this.optDutyOnOff4B.UseVisualStyleBackColor = true;
            // 
            // chkAttend4B
            // 
            resources.ApplyResources(this.chkAttend4B, "chkAttend4B");
            this.chkAttend4B.Checked = true;
            this.chkAttend4B.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend4B.Name = "chkAttend4B";
            this.chkAttend4B.UseVisualStyleBackColor = true;
            // 
            // txtReaderName4B
            // 
            resources.ApplyResources(this.txtReaderName4B, "txtReaderName4B");
            this.txtReaderName4B.Name = "txtReaderName4B";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.optDutyOff3B);
            this.groupBox17.Controls.Add(this.optDutyOn3B);
            this.groupBox17.Controls.Add(this.optDutyOnOff3B);
            resources.ApplyResources(this.groupBox17, "groupBox17");
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.TabStop = false;
            // 
            // optDutyOff3B
            // 
            resources.ApplyResources(this.optDutyOff3B, "optDutyOff3B");
            this.optDutyOff3B.Name = "optDutyOff3B";
            this.optDutyOff3B.UseVisualStyleBackColor = true;
            // 
            // optDutyOn3B
            // 
            resources.ApplyResources(this.optDutyOn3B, "optDutyOn3B");
            this.optDutyOn3B.Name = "optDutyOn3B";
            this.optDutyOn3B.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff3B
            // 
            resources.ApplyResources(this.optDutyOnOff3B, "optDutyOnOff3B");
            this.optDutyOnOff3B.Checked = true;
            this.optDutyOnOff3B.Name = "optDutyOnOff3B";
            this.optDutyOnOff3B.TabStop = true;
            this.optDutyOnOff3B.UseVisualStyleBackColor = true;
            // 
            // chkAttend3B
            // 
            resources.ApplyResources(this.chkAttend3B, "chkAttend3B");
            this.chkAttend3B.Checked = true;
            this.chkAttend3B.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend3B.Name = "chkAttend3B";
            this.chkAttend3B.UseVisualStyleBackColor = true;
            // 
            // txtReaderName3B
            // 
            resources.ApplyResources(this.txtReaderName3B, "txtReaderName3B");
            this.txtReaderName3B.Name = "txtReaderName3B";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.optDutyOff2B);
            this.groupBox18.Controls.Add(this.optDutyOn2B);
            this.groupBox18.Controls.Add(this.optDutyOnOff2B);
            resources.ApplyResources(this.groupBox18, "groupBox18");
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.TabStop = false;
            // 
            // optDutyOff2B
            // 
            resources.ApplyResources(this.optDutyOff2B, "optDutyOff2B");
            this.optDutyOff2B.Name = "optDutyOff2B";
            this.optDutyOff2B.UseVisualStyleBackColor = true;
            // 
            // optDutyOn2B
            // 
            resources.ApplyResources(this.optDutyOn2B, "optDutyOn2B");
            this.optDutyOn2B.Name = "optDutyOn2B";
            this.optDutyOn2B.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff2B
            // 
            resources.ApplyResources(this.optDutyOnOff2B, "optDutyOnOff2B");
            this.optDutyOnOff2B.Checked = true;
            this.optDutyOnOff2B.Name = "optDutyOnOff2B";
            this.optDutyOnOff2B.TabStop = true;
            this.optDutyOnOff2B.UseVisualStyleBackColor = true;
            // 
            // chkAttend2B
            // 
            resources.ApplyResources(this.chkAttend2B, "chkAttend2B");
            this.chkAttend2B.Checked = true;
            this.chkAttend2B.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend2B.Name = "chkAttend2B";
            this.chkAttend2B.UseVisualStyleBackColor = true;
            // 
            // txtReaderName2B
            // 
            resources.ApplyResources(this.txtReaderName2B, "txtReaderName2B");
            this.txtReaderName2B.Name = "txtReaderName2B";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // chkDoorActive2B
            // 
            resources.ApplyResources(this.chkDoorActive2B, "chkDoorActive2B");
            this.chkDoorActive2B.Checked = true;
            this.chkDoorActive2B.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive2B.Name = "chkDoorActive2B";
            this.chkDoorActive2B.UseVisualStyleBackColor = true;
            // 
            // txtDoorName2B
            // 
            resources.ApplyResources(this.txtDoorName2B, "txtDoorName2B");
            this.txtDoorName2B.Name = "txtDoorName2B";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.optNC2B);
            this.groupBox21.Controls.Add(this.optNO2B);
            this.groupBox21.Controls.Add(this.optOnline2B);
            resources.ApplyResources(this.groupBox21, "groupBox21");
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.TabStop = false;
            // 
            // optNC2B
            // 
            resources.ApplyResources(this.optNC2B, "optNC2B");
            this.optNC2B.Name = "optNC2B";
            this.optNC2B.UseVisualStyleBackColor = true;
            // 
            // optNO2B
            // 
            resources.ApplyResources(this.optNO2B, "optNO2B");
            this.optNO2B.Name = "optNO2B";
            this.optNO2B.UseVisualStyleBackColor = true;
            // 
            // optOnline2B
            // 
            resources.ApplyResources(this.optOnline2B, "optOnline2B");
            this.optOnline2B.Checked = true;
            this.optOnline2B.Name = "optOnline2B";
            this.optOnline2B.TabStop = true;
            this.optOnline2B.UseVisualStyleBackColor = true;
            // 
            // gpbAttend1B
            // 
            this.gpbAttend1B.Controls.Add(this.optDutyOff1B);
            this.gpbAttend1B.Controls.Add(this.optDutyOn1B);
            this.gpbAttend1B.Controls.Add(this.optDutyOnOff1B);
            resources.ApplyResources(this.gpbAttend1B, "gpbAttend1B");
            this.gpbAttend1B.Name = "gpbAttend1B";
            this.gpbAttend1B.TabStop = false;
            // 
            // optDutyOff1B
            // 
            resources.ApplyResources(this.optDutyOff1B, "optDutyOff1B");
            this.optDutyOff1B.Name = "optDutyOff1B";
            this.optDutyOff1B.UseVisualStyleBackColor = true;
            // 
            // optDutyOn1B
            // 
            resources.ApplyResources(this.optDutyOn1B, "optDutyOn1B");
            this.optDutyOn1B.Name = "optDutyOn1B";
            this.optDutyOn1B.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff1B
            // 
            resources.ApplyResources(this.optDutyOnOff1B, "optDutyOnOff1B");
            this.optDutyOnOff1B.Checked = true;
            this.optDutyOnOff1B.Name = "optDutyOnOff1B";
            this.optDutyOnOff1B.TabStop = true;
            this.optDutyOnOff1B.UseVisualStyleBackColor = true;
            // 
            // chkAttend1B
            // 
            resources.ApplyResources(this.chkAttend1B, "chkAttend1B");
            this.chkAttend1B.Checked = true;
            this.chkAttend1B.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend1B.Name = "chkAttend1B";
            this.chkAttend1B.UseVisualStyleBackColor = true;
            // 
            // txtReaderName1B
            // 
            resources.ApplyResources(this.txtReaderName1B, "txtReaderName1B");
            this.txtReaderName1B.Name = "txtReaderName1B";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // chkDoorActive1B
            // 
            resources.ApplyResources(this.chkDoorActive1B, "chkDoorActive1B");
            this.chkDoorActive1B.Checked = true;
            this.chkDoorActive1B.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive1B.Name = "chkDoorActive1B";
            this.chkDoorActive1B.UseVisualStyleBackColor = true;
            // 
            // txtDoorName1B
            // 
            resources.ApplyResources(this.txtDoorName1B, "txtDoorName1B");
            this.txtDoorName1B.Name = "txtDoorName1B";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.groupBox19);
            this.tabPage3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.tabPage3.Name = "tabPage3";
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.groupBox20);
            this.groupBox19.Controls.Add(this.btnConnect1_2);
            this.groupBox19.Controls.Add(this.btnConnect1_1);
            this.groupBox19.Controls.Add(this.chkCamera1_1);
            this.groupBox19.Controls.Add(this.nudDoorDelay1A);
            this.groupBox19.Controls.Add(this.label33);
            this.groupBox19.Controls.Add(this.label35);
            this.groupBox19.Controls.Add(this.groupBox14);
            this.groupBox19.Controls.Add(this.chkAttend2A);
            this.groupBox19.Controls.Add(this.txtReaderName2A);
            this.groupBox19.Controls.Add(this.label36);
            this.groupBox19.Controls.Add(this.chkAttend1A);
            this.groupBox19.Controls.Add(this.txtReaderName1A);
            this.groupBox19.Controls.Add(this.label37);
            this.groupBox19.Controls.Add(this.label38);
            this.groupBox19.Controls.Add(this.label42);
            this.groupBox19.Controls.Add(this.chkDoorActive1A);
            this.groupBox19.Controls.Add(this.txtDoorName1A);
            this.groupBox19.Controls.Add(this.label43);
            this.groupBox19.Controls.Add(this.label44);
            this.groupBox19.Controls.Add(this.groupBox22);
            resources.ApplyResources(this.groupBox19, "groupBox19");
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.TabStop = false;
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.optDutyOff1A);
            this.groupBox20.Controls.Add(this.optDutyOn1A);
            this.groupBox20.Controls.Add(this.optDutyOnOff1A);
            resources.ApplyResources(this.groupBox20, "groupBox20");
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.TabStop = false;
            // 
            // optDutyOff1A
            // 
            resources.ApplyResources(this.optDutyOff1A, "optDutyOff1A");
            this.optDutyOff1A.Name = "optDutyOff1A";
            this.optDutyOff1A.UseVisualStyleBackColor = true;
            // 
            // optDutyOn1A
            // 
            resources.ApplyResources(this.optDutyOn1A, "optDutyOn1A");
            this.optDutyOn1A.Name = "optDutyOn1A";
            this.optDutyOn1A.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff1A
            // 
            resources.ApplyResources(this.optDutyOnOff1A, "optDutyOnOff1A");
            this.optDutyOnOff1A.Checked = true;
            this.optDutyOnOff1A.Name = "optDutyOnOff1A";
            this.optDutyOnOff1A.TabStop = true;
            this.optDutyOnOff1A.UseVisualStyleBackColor = true;
            // 
            // btnConnect1_2
            // 
            this.btnConnect1_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect1_2, "btnConnect1_2");
            this.btnConnect1_2.Focusable = true;
            this.btnConnect1_2.ForeColor = System.Drawing.Color.White;
            this.btnConnect1_2.Name = "btnConnect1_2";
            this.btnConnect1_2.Toggle = false;
            this.btnConnect1_2.UseVisualStyleBackColor = false;
            this.btnConnect1_2.Click += new System.EventHandler(this.btnConnect1_2_Click);
            // 
            // btnConnect1_1
            // 
            this.btnConnect1_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConnect1_1, "btnConnect1_1");
            this.btnConnect1_1.Focusable = true;
            this.btnConnect1_1.ForeColor = System.Drawing.Color.White;
            this.btnConnect1_1.Name = "btnConnect1_1";
            this.btnConnect1_1.Toggle = false;
            this.btnConnect1_1.UseVisualStyleBackColor = false;
            this.btnConnect1_1.Click += new System.EventHandler(this.btnConnect1_1_Click);
            // 
            // chkCamera1_1
            // 
            resources.ApplyResources(this.chkCamera1_1, "chkCamera1_1");
            this.chkCamera1_1.Name = "chkCamera1_1";
            this.chkCamera1_1.UseVisualStyleBackColor = true;
            // 
            // nudDoorDelay1A
            // 
            resources.ApplyResources(this.nudDoorDelay1A, "nudDoorDelay1A");
            this.nudDoorDelay1A.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDoorDelay1A.Name = "nudDoorDelay1A";
            this.nudDoorDelay1A.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.optDutyOff2A);
            this.groupBox14.Controls.Add(this.optDutyOn2A);
            this.groupBox14.Controls.Add(this.optDutyOnOff2A);
            resources.ApplyResources(this.groupBox14, "groupBox14");
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.TabStop = false;
            // 
            // optDutyOff2A
            // 
            resources.ApplyResources(this.optDutyOff2A, "optDutyOff2A");
            this.optDutyOff2A.Name = "optDutyOff2A";
            this.optDutyOff2A.UseVisualStyleBackColor = true;
            // 
            // optDutyOn2A
            // 
            resources.ApplyResources(this.optDutyOn2A, "optDutyOn2A");
            this.optDutyOn2A.Name = "optDutyOn2A";
            this.optDutyOn2A.UseVisualStyleBackColor = true;
            // 
            // optDutyOnOff2A
            // 
            resources.ApplyResources(this.optDutyOnOff2A, "optDutyOnOff2A");
            this.optDutyOnOff2A.Checked = true;
            this.optDutyOnOff2A.Name = "optDutyOnOff2A";
            this.optDutyOnOff2A.TabStop = true;
            this.optDutyOnOff2A.UseVisualStyleBackColor = true;
            // 
            // chkAttend2A
            // 
            resources.ApplyResources(this.chkAttend2A, "chkAttend2A");
            this.chkAttend2A.Checked = true;
            this.chkAttend2A.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend2A.Name = "chkAttend2A";
            this.chkAttend2A.UseVisualStyleBackColor = true;
            // 
            // txtReaderName2A
            // 
            resources.ApplyResources(this.txtReaderName2A, "txtReaderName2A");
            this.txtReaderName2A.Name = "txtReaderName2A";
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // chkAttend1A
            // 
            resources.ApplyResources(this.chkAttend1A, "chkAttend1A");
            this.chkAttend1A.Checked = true;
            this.chkAttend1A.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttend1A.Name = "chkAttend1A";
            this.chkAttend1A.UseVisualStyleBackColor = true;
            // 
            // txtReaderName1A
            // 
            resources.ApplyResources(this.txtReaderName1A, "txtReaderName1A");
            this.txtReaderName1A.Name = "txtReaderName1A";
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.Name = "label38";
            // 
            // label42
            // 
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // chkDoorActive1A
            // 
            resources.ApplyResources(this.chkDoorActive1A, "chkDoorActive1A");
            this.chkDoorActive1A.Checked = true;
            this.chkDoorActive1A.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorActive1A.Name = "chkDoorActive1A";
            this.chkDoorActive1A.UseVisualStyleBackColor = true;
            // 
            // txtDoorName1A
            // 
            resources.ApplyResources(this.txtDoorName1A, "txtDoorName1A");
            this.txtDoorName1A.Name = "txtDoorName1A";
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.Name = "label44";
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.optNC1A);
            this.groupBox22.Controls.Add(this.optNO1A);
            this.groupBox22.Controls.Add(this.optOnline1A);
            resources.ApplyResources(this.groupBox22, "groupBox22");
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.TabStop = false;
            // 
            // optNC1A
            // 
            resources.ApplyResources(this.optNC1A, "optNC1A");
            this.optNC1A.Name = "optNC1A";
            this.optNC1A.UseVisualStyleBackColor = true;
            // 
            // optNO1A
            // 
            resources.ApplyResources(this.optNO1A, "optNO1A");
            this.optNO1A.Name = "optNO1A";
            this.optNO1A.UseVisualStyleBackColor = true;
            // 
            // optOnline1A
            // 
            resources.ApplyResources(this.optOnline1A, "optOnline1A");
            this.optOnline1A.Checked = true;
            this.optOnline1A.Name = "optOnline1A";
            this.optOnline1A.TabStop = true;
            this.optOnline1A.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.chkFaceIdent);
            this.groupBox2.Controls.Add(this.chkFpIdent);
            this.groupBox2.Controls.Add(this.lblVolume);
            this.groupBox2.Controls.Add(this.nudVolume);
            this.groupBox2.Controls.Add(this.chkUseM1Card);
            this.groupBox2.Controls.Add(this.chkUseTouchSensor);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkFaceIdent
            // 
            this.chkFaceIdent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chkFaceIdent, "chkFaceIdent");
            this.chkFaceIdent.Name = "chkFaceIdent";
            this.chkFaceIdent.UseVisualStyleBackColor = true;
            // 
            // chkFpIdent
            // 
            this.chkFpIdent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chkFpIdent, "chkFpIdent");
            this.chkFpIdent.Name = "chkFpIdent";
            this.chkFpIdent.UseVisualStyleBackColor = true;
            // 
            // lblVolume
            // 
            resources.ApplyResources(this.lblVolume, "lblVolume");
            this.lblVolume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblVolume.Name = "lblVolume";
            // 
            // nudVolume
            // 
            resources.ApplyResources(this.nudVolume, "nudVolume");
            this.nudVolume.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudVolume.Name = "nudVolume";
            this.nudVolume.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkUseM1Card
            // 
            this.chkUseM1Card.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chkUseM1Card, "chkUseM1Card");
            this.chkUseM1Card.Name = "chkUseM1Card";
            this.chkUseM1Card.UseVisualStyleBackColor = true;
            // 
            // chkUseTouchSensor
            // 
            this.chkUseTouchSensor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.chkUseTouchSensor, "chkUseTouchSensor");
            this.chkUseTouchSensor.Name = "chkUseTouchSensor";
            this.chkUseTouchSensor.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpbController
            // 
            this.grpbController.BackColor = System.Drawing.Color.Transparent;
            this.grpbController.Controls.Add(this.chkWiFiComm);
            this.grpbController.Controls.Add(this.grpWiFi);
            this.grpbController.Controls.Add(this.label8);
            this.grpbController.Controls.Add(this.btnZoneManage);
            this.grpbController.Controls.Add(this.cbof_Zone);
            this.grpbController.Controls.Add(this.label25);
            this.grpbController.Controls.Add(this.grpbIP);
            this.grpbController.Controls.Add(this.chkControllerActive);
            this.grpbController.Controls.Add(this.label26);
            this.grpbController.Controls.Add(this.txtNote);
            this.grpbController.Controls.Add(this.mtxtbControllerNO);
            this.grpbController.Controls.Add(this.mtxtbControllerSN);
            this.grpbController.Controls.Add(this.optIPLarge);
            this.grpbController.Controls.Add(this.optIPSmall);
            this.grpbController.Controls.Add(this.label2);
            this.grpbController.Controls.Add(this.label1);
            this.grpbController.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.grpbController, "grpbController");
            this.grpbController.Name = "grpbController";
            this.grpbController.TabStop = false;
            // 
            // chkWiFiComm
            // 
            resources.ApplyResources(this.chkWiFiComm, "chkWiFiComm");
            this.chkWiFiComm.Name = "chkWiFiComm";
            this.chkWiFiComm.UseVisualStyleBackColor = true;
            this.chkWiFiComm.CheckedChanged += new System.EventHandler(this.chkWiFiComm_CheckedChanged);
            // 
            // grpWiFi
            // 
            this.grpWiFi.Controls.Add(this.lblWiFiIP);
            this.grpWiFi.Controls.Add(this.txtWiFiIP);
            this.grpWiFi.Controls.Add(this.txtWiFiKey);
            this.grpWiFi.Controls.Add(this.txtWiFiSSID);
            this.grpWiFi.Controls.Add(this.lblWiFiSSID);
            this.grpWiFi.Controls.Add(this.lblWiFiKey);
            this.grpWiFi.Controls.Add(this.chkWiFiDhcp);
            this.grpWiFi.Controls.Add(this.lblWiFiPort);
            this.grpWiFi.Controls.Add(this.nudWiFiPort);
            this.grpWiFi.Controls.Add(this.lblWiFiGateway);
            this.grpWiFi.Controls.Add(this.txtWiFiGateway);
            this.grpWiFi.Controls.Add(this.lblWiFiMask);
            this.grpWiFi.Controls.Add(this.txtWiFiMask);
            resources.ApplyResources(this.grpWiFi, "grpWiFi");
            this.grpWiFi.Name = "grpWiFi";
            this.grpWiFi.TabStop = false;
            // 
            // lblWiFiIP
            // 
            resources.ApplyResources(this.lblWiFiIP, "lblWiFiIP");
            this.lblWiFiIP.Name = "lblWiFiIP";
            // 
            // txtWiFiIP
            // 
            resources.ApplyResources(this.txtWiFiIP, "txtWiFiIP");
            this.txtWiFiIP.Name = "txtWiFiIP";
            // 
            // txtWiFiKey
            // 
            resources.ApplyResources(this.txtWiFiKey, "txtWiFiKey");
            this.txtWiFiKey.Name = "txtWiFiKey";
            // 
            // txtWiFiSSID
            // 
            resources.ApplyResources(this.txtWiFiSSID, "txtWiFiSSID");
            this.txtWiFiSSID.Name = "txtWiFiSSID";
            // 
            // lblWiFiSSID
            // 
            resources.ApplyResources(this.lblWiFiSSID, "lblWiFiSSID");
            this.lblWiFiSSID.Name = "lblWiFiSSID";
            // 
            // lblWiFiKey
            // 
            resources.ApplyResources(this.lblWiFiKey, "lblWiFiKey");
            this.lblWiFiKey.Name = "lblWiFiKey";
            // 
            // chkWiFiDhcp
            // 
            resources.ApplyResources(this.chkWiFiDhcp, "chkWiFiDhcp");
            this.chkWiFiDhcp.Name = "chkWiFiDhcp";
            this.chkWiFiDhcp.UseVisualStyleBackColor = true;
            // 
            // lblWiFiPort
            // 
            resources.ApplyResources(this.lblWiFiPort, "lblWiFiPort");
            this.lblWiFiPort.Name = "lblWiFiPort";
            // 
            // nudWiFiPort
            // 
            resources.ApplyResources(this.nudWiFiPort, "nudWiFiPort");
            this.nudWiFiPort.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.nudWiFiPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudWiFiPort.Name = "nudWiFiPort";
            this.nudWiFiPort.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // lblWiFiGateway
            // 
            resources.ApplyResources(this.lblWiFiGateway, "lblWiFiGateway");
            this.lblWiFiGateway.Name = "lblWiFiGateway";
            // 
            // txtWiFiGateway
            // 
            resources.ApplyResources(this.txtWiFiGateway, "txtWiFiGateway");
            this.txtWiFiGateway.Name = "txtWiFiGateway";
            // 
            // lblWiFiMask
            // 
            resources.ApplyResources(this.lblWiFiMask, "lblWiFiMask");
            this.lblWiFiMask.Name = "lblWiFiMask";
            // 
            // txtWiFiMask
            // 
            resources.ApplyResources(this.txtWiFiMask, "txtWiFiMask");
            this.txtWiFiMask.Name = "txtWiFiMask";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnZoneManage
            // 
            this.btnZoneManage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnZoneManage, "btnZoneManage");
            this.btnZoneManage.Focusable = true;
            this.btnZoneManage.ForeColor = System.Drawing.Color.White;
            this.btnZoneManage.Name = "btnZoneManage";
            this.btnZoneManage.Toggle = false;
            this.btnZoneManage.UseVisualStyleBackColor = false;
            this.btnZoneManage.Click += new System.EventHandler(this.btnZoneManage_Click);
            // 
            // cbof_Zone
            // 
            this.cbof_Zone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_Zone.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_Zone, "cbof_Zone");
            this.cbof_Zone.Name = "cbof_Zone";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // grpbIP
            // 
            this.grpbIP.Controls.Add(this.nudPort);
            this.grpbIP.Controls.Add(this.txtControllerIP);
            this.grpbIP.Controls.Add(this.label4);
            this.grpbIP.Controls.Add(this.label3);
            resources.ApplyResources(this.grpbIP, "grpbIP");
            this.grpbIP.Name = "grpbIP";
            this.grpbIP.TabStop = false;
            // 
            // nudPort
            // 
            resources.ApplyResources(this.nudPort, "nudPort");
            this.nudPort.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // txtControllerIP
            // 
            resources.ApplyResources(this.txtControllerIP, "txtControllerIP");
            this.txtControllerIP.Name = "txtControllerIP";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkControllerActive
            // 
            resources.ApplyResources(this.chkControllerActive, "chkControllerActive");
            this.chkControllerActive.Checked = true;
            this.chkControllerActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkControllerActive.Name = "chkControllerActive";
            this.chkControllerActive.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // txtNote
            // 
            resources.ApplyResources(this.txtNote, "txtNote");
            this.txtNote.Name = "txtNote";
            // 
            // mtxtbControllerNO
            // 
            resources.ApplyResources(this.mtxtbControllerNO, "mtxtbControllerNO");
            this.mtxtbControllerNO.Name = "mtxtbControllerNO";
            this.mtxtbControllerNO.ValidatingType = typeof(int);
            this.mtxtbControllerNO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mtxtbControllerNO_KeyPress);
            this.mtxtbControllerNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtbControllerNO_KeyUp);
            // 
            // mtxtbControllerSN
            // 
            resources.ApplyResources(this.mtxtbControllerSN, "mtxtbControllerSN");
            this.mtxtbControllerSN.Name = "mtxtbControllerSN";
            this.mtxtbControllerSN.RejectInputOnFirstFailure = true;
            this.mtxtbControllerSN.ResetOnSpace = false;
            this.mtxtbControllerSN.TextChanged += new System.EventHandler(this.mtxtbControllerSN_TextChanged);
            this.mtxtbControllerSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mtxtbControllerSN_KeyPress);
            this.mtxtbControllerSN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtxtbControllerSN_KeyUp);
            // 
            // optIPLarge
            // 
            resources.ApplyResources(this.optIPLarge, "optIPLarge");
            this.optIPLarge.Name = "optIPLarge";
            this.optIPLarge.UseVisualStyleBackColor = true;
            this.optIPLarge.CheckedChanged += new System.EventHandler(this.optIPLarge_CheckedChanged);
            // 
            // optIPSmall
            // 
            resources.ApplyResources(this.optIPSmall, "optIPSmall");
            this.optIPSmall.Checked = true;
            this.optIPSmall.Name = "optIPSmall";
            this.optIPSmall.TabStop = true;
            this.optIPSmall.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.Focusable = true;
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Name = "btnNext";
            this.btnNext.Toggle = false;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel2
            // 
            this.btnCancel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel2, "btnCancel2");
            this.btnCancel2.Focusable = true;
            this.btnCancel2.ForeColor = System.Drawing.Color.White;
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Toggle = false;
            this.btnCancel2.UseVisualStyleBackColor = false;
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnNext);
            this.panelBottomBanner.Controls.Add(this.btnCancel2);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnPrev, "btnPrev");
            this.btnPrev.Focusable = true;
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Toggle = false;
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // dfrmController
            // 
            this.AcceptButton = this.btnNext;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.grpbDoorReader);
            this.Controls.Add(this.grpbController);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmController";
            this.Load += new System.EventHandler(this.dfrmController_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmController_KeyDown);
            this.grpbDoorReader.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay4D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay3D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay2D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay1D)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay2B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay1B)).EndInit();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.gpbAttend1B.ResumeLayout(false);
            this.gpbAttend1B.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDoorDelay1A)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).EndInit();
            this.grpbController.ResumeLayout(false);
            this.grpbController.PerformLayout();
            this.grpWiFi.ResumeLayout(false);
            this.grpWiFi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWiFiPort)).EndInit();
            this.grpbIP.ResumeLayout(false);
            this.grpbIP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageButton btnCancel;
        private ImageButton btnCancel2;
        public ImageButton btnNext;
        public ImageButton btnOK;
        private ImageButton btnZoneManage;
        private ComboBox cbof_Zone;
        private CheckBox chkAttend1A;
        private CheckBox chkAttend1B;
        private CheckBox chkAttend1D;
        private CheckBox chkAttend2A;
        private CheckBox chkAttend2B;
        private CheckBox chkAttend2D;
        private CheckBox chkAttend3B;
        private CheckBox chkAttend3D;
        private CheckBox chkAttend4B;
        private CheckBox chkAttend4D;
        private CheckBox chkControllerActive;
        private CheckBox chkDoorActive1A;
        private CheckBox chkDoorActive1B;
        private CheckBox chkDoorActive1D;
        private CheckBox chkDoorActive2B;
        private CheckBox chkDoorActive2D;
        private CheckBox chkDoorActive3D;
        private CheckBox chkDoorActive4D;
        private GroupBox gpbAttend1B;
        private GroupBox groupBox1;
        private GroupBox groupBox10;
        private GroupBox groupBox11;
        private GroupBox groupBox12;
        private GroupBox groupBox13;
        private GroupBox groupBox14;
        private GroupBox groupBox15;
        private GroupBox groupBox16;
        private GroupBox groupBox17;
        private GroupBox groupBox18;
        private GroupBox groupBox19;
        private GroupBox groupBox20;
        private GroupBox groupBox21;
        private GroupBox groupBox22;
        private GroupBox groupBox23;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox groupBox8;
        private GroupBox groupBox9;
        private GroupBox grpbController;
        private GroupBox grpbDoorReader;
        private GroupBox grpbIP;
        private Label label1;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label3;
        private Label label30;
        private Label label31;
        private Label label32;
        private Label label33;
        private Label label34;
        private Label label35;
        private Label label36;
        private Label label37;
        private Label label38;
        private Label label39;
        private Label label4;
        private Label label40;
        private Label label41;
        private Label label42;
        private Label label43;
        private Label label44;
        private Label label8;
        public MaskedTextBox mtxtbControllerNO;
        public MaskedTextBox mtxtbControllerSN;
        private NumericUpDown nudDoorDelay1A;
        private NumericUpDown nudDoorDelay1B;
        private NumericUpDown nudDoorDelay1D;
        private NumericUpDown nudDoorDelay2B;
        private NumericUpDown nudDoorDelay2D;
        private NumericUpDown nudDoorDelay3D;
        private NumericUpDown nudDoorDelay4D;
        private NumericUpDown nudPort;
        private RadioButton optDutyOff1A;
        private RadioButton optDutyOff1B;
        private RadioButton optDutyOff1D;
        private RadioButton optDutyOff2A;
        private RadioButton optDutyOff2B;
        private RadioButton optDutyOff2D;
        private RadioButton optDutyOff3B;
        private RadioButton optDutyOff3D;
        private RadioButton optDutyOff4B;
        private RadioButton optDutyOff4D;
        private RadioButton optDutyOn1A;
        private RadioButton optDutyOn1B;
        private RadioButton optDutyOn1D;
        private RadioButton optDutyOn2A;
        private RadioButton optDutyOn2B;
        private RadioButton optDutyOn2D;
        private RadioButton optDutyOn3B;
        private RadioButton optDutyOn3D;
        private RadioButton optDutyOn4B;
        private RadioButton optDutyOn4D;
        private RadioButton optDutyOnOff1A;
        private RadioButton optDutyOnOff1B;
        private RadioButton optDutyOnOff1D;
        private RadioButton optDutyOnOff2A;
        private RadioButton optDutyOnOff2B;
        private RadioButton optDutyOnOff2D;
        private RadioButton optDutyOnOff3B;
        private RadioButton optDutyOnOff3D;
        private RadioButton optDutyOnOff4B;
        private RadioButton optDutyOnOff4D;
        public RadioButton optIPLarge;
        private RadioButton optIPSmall;
        private RadioButton optNC1A;
        private RadioButton optNC1B;
        private RadioButton optNC1D;
        private RadioButton optNC2B;
        private RadioButton optNC2D;
        private RadioButton optNC3D;
        private RadioButton optNC4D;
        private RadioButton optNO1A;
        private RadioButton optNO1B;
        private RadioButton optNO1D;
        private RadioButton optNO2B;
        private RadioButton optNO2D;
        private RadioButton optNO3D;
        private RadioButton optNO4D;
        private RadioButton optOnline1A;
        private RadioButton optOnline1B;
        private RadioButton optOnline1D;
        private RadioButton optOnline2B;
        private RadioButton optOnline2D;
        private RadioButton optOnline3D;
        private RadioButton optOnline4D;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        public TextBox txtControllerIP;
        private TextBox txtDoorName1A;
        private TextBox txtDoorName1B;
        private TextBox txtDoorName1D;
        private TextBox txtDoorName2B;
        private TextBox txtDoorName2D;
        private TextBox txtDoorName3D;
        private TextBox txtDoorName4D;
        private TextBox txtNote;
        private TextBox txtReaderName1A;
        private TextBox txtReaderName1B;
        private TextBox txtReaderName1D;
        private TextBox txtReaderName2A;
        private TextBox txtReaderName2B;
        private TextBox txtReaderName2D;
        private TextBox txtReaderName3B;
        private TextBox txtReaderName3D;
        private TextBox txtReaderName4B;
        private TextBox txtReaderName4D;
        private CheckBox chkWiFiComm;
        private GroupBox grpWiFi;
        private TextBox txtWiFiKey;
        private TextBox txtWiFiSSID;
        private Label lblWiFiSSID;
        private Label lblWiFiKey;
        private CheckBox chkWiFiDhcp;
        private Label lblWiFiPort;
        private NumericUpDown nudWiFiPort;
        private Label lblWiFiGateway;
        private TextBox txtWiFiGateway;
        private Label lblWiFiMask;
        private TextBox txtWiFiMask;
        private Label lblWiFiIP;
        private TextBox txtWiFiIP;
        private CheckBox chkCamera4_4;
        private CheckBox chkCamera4_3;
        private CheckBox chkCamera4_2;
        private CheckBox chkCamera4_1;
        private ImageButton btnConnect4_4;
        private ImageButton btnConnect4_3;
        private ImageButton btnConnect4_2;
        private ImageButton btnConnect4_1;
        private Panel panelBottomBanner;
        private Panel panel1;
        private TabPage tabPage4;
        private GroupBox groupBox2;
        private CheckBox chkUseM1Card;
        private CheckBox chkUseTouchSensor;
        private ImageButton btnConnect1_2;
        private ImageButton btnConnect1_1;
        private CheckBox chkCamera1_1;
        private Label lblVolume;
        private NumericUpDown nudVolume;
        private CheckBox chkFpIdent;
        public ImageButton btnPrev;
        private ImageButton btnConnect2_4;
        private ImageButton btnConnect2_3;
        private ImageButton btnConnect2_2;
        private ImageButton btnConnect2_1;
        private CheckBox chkCamera2_2;
        private CheckBox chkCamera2_1;
        private CheckBox chkFaceIdent;
    }
}

