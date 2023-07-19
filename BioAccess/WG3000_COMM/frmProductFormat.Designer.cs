namespace WG3000_COMM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Media;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;

    public partial class frmProductFormat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductFormat));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnPing = new System.Windows.Forms.Button();
            this.lblCommLose = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.optNO = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblFloor = new System.Windows.Forms.Label();
            this.label143 = new System.Windows.Forms.Label();
            this.numericUpDown22 = new System.Windows.Forms.NumericUpDown();
            this.button72 = new System.Windows.Forms.Button();
            this.checkBox131 = new System.Windows.Forms.CheckBox();
            this.checkBox130 = new System.Windows.Forms.CheckBox();
            this.button70 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label146 = new System.Windows.Forms.Label();
            this.textBox32 = new System.Windows.Forms.TextBox();
            this.button57 = new System.Windows.Forms.Button();
            this.checkBox117 = new System.Windows.Forms.CheckBox();
            this.checkBox116 = new System.Windows.Forms.CheckBox();
            this.checkBox113 = new System.Windows.Forms.CheckBox();
            this.txtRunInfo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblFailDetail = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnected = new System.Windows.Forms.Button();
            this.chkAutoFormat = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnFormat = new System.Windows.Forms.Button();
            this.btnAdjustTime = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label147 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown22)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // btnPing
            // 
            resources.ApplyResources(this.btnPing, "btnPing");
            this.btnPing.Name = "btnPing";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // lblCommLose
            // 
            resources.ApplyResources(this.lblCommLose, "lblCommLose");
            this.lblCommLose.BackColor = System.Drawing.Color.Red;
            this.lblCommLose.ForeColor = System.Drawing.Color.White;
            this.lblCommLose.Name = "lblCommLose";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.numericUpDown2);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.lblFloor);
            this.tabPage1.Controls.Add(this.label143);
            this.tabPage1.Controls.Add(this.numericUpDown22);
            this.tabPage1.Controls.Add(this.button72);
            this.tabPage1.Controls.Add(this.checkBox131);
            this.tabPage1.Controls.Add(this.checkBox130);
            this.tabPage1.Controls.Add(this.button70);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.optNO);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // optNO
            // 
            resources.ApplyResources(this.optNO, "optNO");
            this.optNO.Checked = true;
            this.optNO.Name = "optNO";
            this.optNO.TabStop = true;
            this.optNO.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // numericUpDown2
            // 
            resources.ApplyResources(this.numericUpDown2, "numericUpDown2");
            this.numericUpDown2.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnStop
            // 
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.Name = "btnStop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblFloor
            // 
            resources.ApplyResources(this.lblFloor, "lblFloor");
            this.lblFloor.Name = "lblFloor";
            // 
            // label143
            // 
            resources.ApplyResources(this.label143, "label143");
            this.label143.Name = "label143";
            // 
            // numericUpDown22
            // 
            resources.ApplyResources(this.numericUpDown22, "numericUpDown22");
            this.numericUpDown22.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDown22.Name = "numericUpDown22";
            this.numericUpDown22.Value = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            // 
            // button72
            // 
            resources.ApplyResources(this.button72, "button72");
            this.button72.Name = "button72";
            this.button72.UseVisualStyleBackColor = true;
            this.button72.Click += new System.EventHandler(this.button70_Click);
            // 
            // checkBox131
            // 
            resources.ApplyResources(this.checkBox131, "checkBox131");
            this.checkBox131.Checked = true;
            this.checkBox131.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox131.Name = "checkBox131";
            this.checkBox131.UseVisualStyleBackColor = true;
            // 
            // checkBox130
            // 
            resources.ApplyResources(this.checkBox130, "checkBox130");
            this.checkBox130.Checked = true;
            this.checkBox130.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox130.Name = "checkBox130";
            this.checkBox130.UseVisualStyleBackColor = true;
            // 
            // button70
            // 
            resources.ApplyResources(this.button70, "button70");
            this.button70.Name = "button70";
            this.button70.UseVisualStyleBackColor = true;
            this.button70.Click += new System.EventHandler(this.button70_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label146);
            this.tabPage2.Controls.Add(this.textBox32);
            this.tabPage2.Controls.Add(this.button57);
            this.tabPage2.Controls.Add(this.checkBox117);
            this.tabPage2.Controls.Add(this.checkBox116);
            this.tabPage2.Controls.Add(this.checkBox113);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label146
            // 
            resources.ApplyResources(this.label146, "label146");
            this.label146.Name = "label146";
            // 
            // textBox32
            // 
            resources.ApplyResources(this.textBox32, "textBox32");
            this.textBox32.Name = "textBox32";
            // 
            // button57
            // 
            resources.ApplyResources(this.button57, "button57");
            this.button57.Name = "button57";
            this.button57.UseVisualStyleBackColor = true;
            this.button57.Click += new System.EventHandler(this.button57_Click);
            // 
            // checkBox117
            // 
            resources.ApplyResources(this.checkBox117, "checkBox117");
            this.checkBox117.Checked = true;
            this.checkBox117.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox117.Name = "checkBox117";
            this.checkBox117.UseVisualStyleBackColor = true;
            // 
            // checkBox116
            // 
            resources.ApplyResources(this.checkBox116, "checkBox116");
            this.checkBox116.Checked = true;
            this.checkBox116.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox116.Name = "checkBox116";
            this.checkBox116.UseVisualStyleBackColor = true;
            // 
            // checkBox113
            // 
            resources.ApplyResources(this.checkBox113, "checkBox113");
            this.checkBox113.Checked = true;
            this.checkBox113.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox113.Name = "checkBox113";
            this.checkBox113.UseVisualStyleBackColor = true;
            // 
            // txtRunInfo
            // 
            resources.ApplyResources(this.txtRunInfo, "txtRunInfo");
            this.txtRunInfo.Name = "txtRunInfo";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Name = "label7";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Name = "label8";
            // 
            // lblFailDetail
            // 
            resources.ApplyResources(this.lblFailDetail, "lblFailDetail");
            this.lblFailDetail.ForeColor = System.Drawing.Color.White;
            this.lblFailDetail.Name = "lblFailDetail";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // btnConnected
            // 
            this.btnConnected.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.btnConnected, "btnConnected");
            this.btnConnected.Name = "btnConnected";
            this.btnConnected.UseVisualStyleBackColor = false;
            // 
            // chkAutoFormat
            // 
            resources.ApplyResources(this.chkAutoFormat, "chkAutoFormat");
            this.chkAutoFormat.ForeColor = System.Drawing.Color.White;
            this.chkAutoFormat.Name = "chkAutoFormat";
            this.chkAutoFormat.UseVisualStyleBackColor = true;
            this.chkAutoFormat.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnFormat
            // 
            resources.ApplyResources(this.btnFormat, "btnFormat");
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.UseVisualStyleBackColor = true;
            this.btnFormat.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnAdjustTime
            // 
            resources.ApplyResources(this.btnAdjustTime, "btnAdjustTime");
            this.btnAdjustTime.Name = "btnAdjustTime";
            this.btnAdjustTime.UseVisualStyleBackColor = true;
            this.btnAdjustTime.Click += new System.EventHandler(this.btnAdjustTime_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // lblTime
            // 
            resources.ApplyResources(this.lblTime, "lblTime");
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Name = "lblTime";
            // 
            // txtTime
            // 
            resources.ApplyResources(this.txtTime, "txtTime");
            this.txtTime.ForeColor = System.Drawing.Color.Black;
            this.txtTime.Name = "txtTime";
            // 
            // txtSN
            // 
            resources.ApplyResources(this.txtSN, "txtSN");
            this.txtSN.ForeColor = System.Drawing.Color.Black;
            this.txtSN.Name = "txtSN";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label147);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Red;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label12.Name = "label12";
            // 
            // label147
            // 
            resources.ApplyResources(this.label147, "label147");
            this.label147.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label147.Name = "label147";
            // 
            // frmProductFormat
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.lblCommLose);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtRunInfo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblFailDetail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnConnected);
            this.Controls.Add(this.chkAutoFormat);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnFormat);
            this.Controls.Add(this.btnAdjustTime);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtSN);
            this.MinimizeBox = false;
            this.Name = "frmProductFormat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProductFormat_FormClosing);
            this.Load += new System.EventHandler(this.frmProductFormat_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown22)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private Button btnAdjustTime;
        private Button btnConnected;
        private Button btnFormat;
        private Button btnPing;
        private Button btnStop;
        private Button button1;
        private Button button57;
        private Button button70;
        private Button button72;
        private CheckBox checkBox1;
        private CheckBox checkBox113;
        private CheckBox checkBox116;
        private CheckBox checkBox117;
        private CheckBox checkBox130;
        private CheckBox checkBox131;
        private CheckBox chkAutoFormat;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label143;
        private Label label146;
        private Label label147;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblCommLose;
        private Label lblFailDetail;
        private Label lblFloor;
        private Label lblTime;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown22;
        private RadioButton optNO;
        private Panel panel1;
        private RadioButton radioButton2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBox32;
        private System.Windows.Forms.Timer timer1;
        private TextBox txtRunInfo;
        private TextBox txtSN;
        private TextBox txtTime;
    }
}

