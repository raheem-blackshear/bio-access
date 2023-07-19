namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmPeripheralControlBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmPeripheralControlBoard));
            this.txtf_ControllerSN = new System.Windows.Forms.TextBox();
            this.txtf_ControllerNO = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.grpExt = new System.Windows.Forms.GroupBox();
            this.grpSet = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.btnOption = new System.Windows.Forms.ImageButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButton25 = new System.Windows.Forms.RadioButton();
            this.radioButton13 = new System.Windows.Forms.RadioButton();
            this.radioButton12 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.grpEvent = new System.Windows.Forms.GroupBox();
            this.checkBox90 = new System.Windows.Forms.CheckBox();
            this.checkBox89 = new System.Windows.Forms.CheckBox();
            this.checkBox88 = new System.Windows.Forms.CheckBox();
            this.checkBox87 = new System.Windows.Forms.CheckBox();
            this.checkBox86 = new System.Windows.Forms.CheckBox();
            this.checkBox85 = new System.Windows.Forms.CheckBox();
            this.checkBox84 = new System.Windows.Forms.CheckBox();
            this.label70 = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.grpExt.SuspendLayout();
            this.grpSet.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtf_ControllerSN
            // 
            this.txtf_ControllerSN.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
            this.txtf_ControllerSN.Name = "txtf_ControllerSN";
            this.txtf_ControllerSN.ReadOnly = true;
            // 
            // txtf_ControllerNO
            // 
            this.txtf_ControllerNO.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtf_ControllerNO, "txtf_ControllerNO");
            this.txtf_ControllerNO.Name = "txtf_ControllerNO";
            this.txtf_ControllerNO.ReadOnly = true;
            // 
            // Label2
            // 
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label2.Name = "Label2";
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label1.Name = "Label1";
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpExt
            // 
            this.grpExt.BackColor = System.Drawing.Color.Transparent;
            this.grpExt.Controls.Add(this.grpSet);
            this.grpExt.Controls.Add(this.chkActive);
            this.grpExt.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grpExt, "grpExt");
            this.grpExt.Name = "grpExt";
            this.grpExt.TabStop = false;
            // 
            // grpSet
            // 
            this.grpSet.Controls.Add(this.label4);
            this.grpSet.Controls.Add(this.label3);
            this.grpSet.Controls.Add(this.label71);
            this.grpSet.Controls.Add(this.btnOption);
            this.grpSet.Controls.Add(this.groupBox5);
            this.grpSet.Controls.Add(this.grpEvent);
            this.grpSet.Controls.Add(this.label70);
            this.grpSet.Controls.Add(this.nudDelay);
            this.grpSet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.grpSet, "grpSet");
            this.grpSet.Name = "grpSet";
            this.grpSet.TabStop = false;
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
            // label71
            // 
            resources.ApplyResources(this.label71, "label71");
            this.label71.Name = "label71";
            // 
            // btnOption
            // 
            this.btnOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOption, "btnOption");
            this.btnOption.ForeColor = System.Drawing.Color.White;
            this.btnOption.Name = "btnOption";
            this.btnOption.UseVisualStyleBackColor = false;
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButton25);
            this.groupBox5.Controls.Add(this.radioButton13);
            this.groupBox5.Controls.Add(this.radioButton12);
            this.groupBox5.Controls.Add(this.radioButton11);
            this.groupBox5.Controls.Add(this.radioButton10);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // radioButton25
            // 
            resources.ApplyResources(this.radioButton25, "radioButton25");
            this.radioButton25.Name = "radioButton25";
            this.radioButton25.UseVisualStyleBackColor = true;
            this.radioButton25.CheckedChanged += new System.EventHandler(this.radioButton25_CheckedChanged);
            // 
            // radioButton13
            // 
            resources.ApplyResources(this.radioButton13, "radioButton13");
            this.radioButton13.Name = "radioButton13";
            this.radioButton13.UseVisualStyleBackColor = true;
            // 
            // radioButton12
            // 
            resources.ApplyResources(this.radioButton12, "radioButton12");
            this.radioButton12.Name = "radioButton12";
            this.radioButton12.UseVisualStyleBackColor = true;
            // 
            // radioButton11
            // 
            resources.ApplyResources(this.radioButton11, "radioButton11");
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            resources.ApplyResources(this.radioButton10, "radioButton10");
            this.radioButton10.Checked = true;
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.TabStop = true;
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // grpEvent
            // 
            this.grpEvent.Controls.Add(this.checkBox90);
            this.grpEvent.Controls.Add(this.checkBox89);
            this.grpEvent.Controls.Add(this.checkBox88);
            this.grpEvent.Controls.Add(this.checkBox87);
            this.grpEvent.Controls.Add(this.checkBox86);
            this.grpEvent.Controls.Add(this.checkBox85);
            this.grpEvent.Controls.Add(this.checkBox84);
            resources.ApplyResources(this.grpEvent, "grpEvent");
            this.grpEvent.Name = "grpEvent";
            this.grpEvent.TabStop = false;
            // 
            // checkBox90
            // 
            resources.ApplyResources(this.checkBox90, "checkBox90");
            this.checkBox90.Name = "checkBox90";
            this.checkBox90.UseVisualStyleBackColor = true;
            // 
            // checkBox89
            // 
            resources.ApplyResources(this.checkBox89, "checkBox89");
            this.checkBox89.Name = "checkBox89";
            this.checkBox89.UseVisualStyleBackColor = true;
            // 
            // checkBox88
            // 
            resources.ApplyResources(this.checkBox88, "checkBox88");
            this.checkBox88.Name = "checkBox88";
            this.checkBox88.UseVisualStyleBackColor = true;
            // 
            // checkBox87
            // 
            resources.ApplyResources(this.checkBox87, "checkBox87");
            this.checkBox87.Name = "checkBox87";
            this.checkBox87.UseVisualStyleBackColor = true;
            // 
            // checkBox86
            // 
            resources.ApplyResources(this.checkBox86, "checkBox86");
            this.checkBox86.Name = "checkBox86";
            this.checkBox86.UseVisualStyleBackColor = true;
            // 
            // checkBox85
            // 
            resources.ApplyResources(this.checkBox85, "checkBox85");
            this.checkBox85.Name = "checkBox85";
            this.checkBox85.UseVisualStyleBackColor = true;
            // 
            // checkBox84
            // 
            resources.ApplyResources(this.checkBox84, "checkBox84");
            this.checkBox84.Name = "checkBox84";
            this.checkBox84.UseVisualStyleBackColor = true;
            // 
            // label70
            // 
            resources.ApplyResources(this.label70, "label70");
            this.label70.Name = "label70";
            // 
            // nudDelay
            // 
            resources.ApplyResources(this.nudDelay, "nudDelay");
            this.nudDelay.Maximum = new decimal(new int[] {
            6553,
            0,
            0,
            0});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // chkActive
            // 
            resources.ApplyResources(this.chkActive, "chkActive");
            this.chkActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkActive.Name = "chkActive";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
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
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnExit);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmPeripheralControlBoard
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.grpExt);
            this.Controls.Add(this.txtf_ControllerSN);
            this.Controls.Add(this.txtf_ControllerNO);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmPeripheralControlBoard";
            this.Load += new System.EventHandler(this.dfrmPeripheralControlBoard_Load);
            this.grpExt.ResumeLayout(false);
            this.grpExt.PerformLayout();
            this.grpSet.ResumeLayout(false);
            this.grpSet.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.grpEvent.ResumeLayout(false);
            this.grpEvent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnExit;
        internal ImageButton btnOK;
        private ImageButton btnOption;
        private CheckBox checkBox84;
        private CheckBox checkBox85;
        private CheckBox checkBox86;
        private CheckBox checkBox87;
        private CheckBox checkBox88;
        private CheckBox checkBox89;
        private CheckBox checkBox90;
        private CheckBox chkActive;
        private GroupBox groupBox5;
        private GroupBox grpEvent;
        private GroupBox grpExt;
        private GroupBox grpSet;
        internal Label Label1;
        internal Label Label2;
        private Label label3;
        private Label label4;
        private Label label70;
        private Label label71;
        private NumericUpDown nudDelay;
        private RadioButton radioButton10;
        private RadioButton radioButton11;
        private RadioButton radioButton12;
        private RadioButton radioButton13;
        private RadioButton radioButton25;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        internal TextBox txtf_ControllerNO;
        internal TextBox txtf_ControllerSN;
        private Panel panelBottomBanner;
    }
}

