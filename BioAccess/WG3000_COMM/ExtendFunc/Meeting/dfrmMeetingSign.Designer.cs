namespace WG3000_COMM.ExtendFunc.Meeting
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmMeetingSign
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
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmMeetingSign));
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Timer2 = new System.Windows.Forms.Timer(this.components);
            this.TimerStartSlow = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.picSwipe1 = new System.Windows.Forms.PictureBox();
            this.txtSwipeUser1 = new System.Windows.Forms.TextBox();
            this.txtSwipeSeat1 = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.picSwipe2 = new System.Windows.Forms.PictureBox();
            this.txtSwipeUser2 = new System.Windows.Forms.TextBox();
            this.txtSwipeSeat2 = new System.Windows.Forms.TextBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.picSwipe3 = new System.Windows.Forms.PictureBox();
            this.txtSwipeUser3 = new System.Windows.Forms.TextBox();
            this.txtSwipeSeat3 = new System.Windows.Forms.TextBox();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.picSwipe4 = new System.Windows.Forms.PictureBox();
            this.txtSwipeUser4 = new System.Windows.Forms.TextBox();
            this.txtSwipeSeat4 = new System.Windows.Forms.TextBox();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.picSwipe5 = new System.Windows.Forms.PictureBox();
            this.txtSwipeUser5 = new System.Windows.Forms.TextBox();
            this.txtSwipeSeat5 = new System.Windows.Forms.TextBox();
            this.lblMeetingName = new System.Windows.Forms.Label();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.ImageButton();
            this.btnErrConnect = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.Button6 = new System.Windows.Forms.ImageButton();
            this.Button7 = new System.Windows.Forms.ImageButton();
            this.lblTime = new System.Windows.Forms.Label();
            this.Button3 = new System.Windows.Forms.ImageButton();
            this.Button2 = new System.Windows.Forms.ImageButton();
            this.Button1 = new System.Windows.Forms.ImageButton();
            this.Button4 = new System.Windows.Forms.ImageButton();
            this.Button5 = new System.Windows.Forms.ImageButton();
            this.txtA0 = new System.Windows.Forms.TextBox();
            this.txtA1 = new System.Windows.Forms.TextBox();
            this.txtA2 = new System.Windows.Forms.TextBox();
            this.txtA3 = new System.Windows.Forms.TextBox();
            this.txtA4 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtB0 = new System.Windows.Forms.TextBox();
            this.txtB1 = new System.Windows.Forms.TextBox();
            this.txtB2 = new System.Windows.Forms.TextBox();
            this.txtB3 = new System.Windows.Forms.TextBox();
            this.txtB4 = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtC0 = new System.Windows.Forms.TextBox();
            this.txtC1 = new System.Windows.Forms.TextBox();
            this.txtC2 = new System.Windows.Forms.TextBox();
            this.txtC3 = new System.Windows.Forms.TextBox();
            this.txtC4 = new System.Windows.Forms.TextBox();
            this.txtD0 = new System.Windows.Forms.TextBox();
            this.txtD1 = new System.Windows.Forms.TextBox();
            this.txtD2 = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtD3 = new System.Windows.Forms.TextBox();
            this.txtD4 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtE3 = new System.Windows.Forms.TextBox();
            this.txtE4 = new System.Windows.Forms.TextBox();
            this.txtE2 = new System.Windows.Forms.TextBox();
            this.txtE1 = new System.Windows.Forms.TextBox();
            this.txtE0 = new System.Windows.Forms.TextBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox8 = new System.Windows.Forms.GroupBox();
            this.picSwipe6 = new System.Windows.Forms.PictureBox();
            this.txtSwipeUser6 = new System.Windows.Forms.TextBox();
            this.txtSwipeSeat6 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe1)).BeginInit();
            this.GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe2)).BeginInit();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe3)).BeginInit();
            this.GroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe4)).BeginInit();
            this.GroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe5)).BeginInit();
            this.GroupBox5.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.GroupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe6)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 500;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Timer2
            // 
            this.Timer2.Interval = 50;
            this.Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // TimerStartSlow
            // 
            this.TimerStartSlow.Enabled = true;
            this.TimerStartSlow.Interval = 300;
            this.TimerStartSlow.Tick += new System.EventHandler(this.TimerStartSlow_Tick);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.GroupBox2);
            this.flowLayoutPanel1.Controls.Add(this.GroupBox3);
            this.flowLayoutPanel1.Controls.Add(this.GroupBox4);
            this.flowLayoutPanel1.Controls.Add(this.GroupBox6);
            this.flowLayoutPanel1.Controls.Add(this.GroupBox7);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.Controls.Add(this.picSwipe1);
            this.GroupBox2.Controls.Add(this.txtSwipeUser1);
            this.GroupBox2.Controls.Add(this.txtSwipeSeat1);
            this.GroupBox2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox2, "GroupBox2");
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.TabStop = false;
            // 
            // picSwipe1
            // 
            resources.ApplyResources(this.picSwipe1, "picSwipe1");
            this.picSwipe1.Name = "picSwipe1";
            this.picSwipe1.TabStop = false;
            // 
            // txtSwipeUser1
            // 
            resources.ApplyResources(this.txtSwipeUser1, "txtSwipeUser1");
            this.txtSwipeUser1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtSwipeUser1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeUser1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSwipeUser1.Name = "txtSwipeUser1";
            this.txtSwipeUser1.ReadOnly = true;
            // 
            // txtSwipeSeat1
            // 
            resources.ApplyResources(this.txtSwipeSeat1, "txtSwipeSeat1");
            this.txtSwipeSeat1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtSwipeSeat1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeSeat1.Name = "txtSwipeSeat1";
            this.txtSwipeSeat1.ReadOnly = true;
            // 
            // GroupBox3
            // 
            this.GroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox3.Controls.Add(this.picSwipe2);
            this.GroupBox3.Controls.Add(this.txtSwipeUser2);
            this.GroupBox3.Controls.Add(this.txtSwipeSeat2);
            this.GroupBox3.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox3, "GroupBox3");
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.TabStop = false;
            // 
            // picSwipe2
            // 
            resources.ApplyResources(this.picSwipe2, "picSwipe2");
            this.picSwipe2.Name = "picSwipe2";
            this.picSwipe2.TabStop = false;
            // 
            // txtSwipeUser2
            // 
            resources.ApplyResources(this.txtSwipeUser2, "txtSwipeUser2");
            this.txtSwipeUser2.BackColor = System.Drawing.Color.White;
            this.txtSwipeUser2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeUser2.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeUser2.Name = "txtSwipeUser2";
            this.txtSwipeUser2.ReadOnly = true;
            // 
            // txtSwipeSeat2
            // 
            resources.ApplyResources(this.txtSwipeSeat2, "txtSwipeSeat2");
            this.txtSwipeSeat2.BackColor = System.Drawing.Color.White;
            this.txtSwipeSeat2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeSeat2.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeSeat2.Name = "txtSwipeSeat2";
            this.txtSwipeSeat2.ReadOnly = true;
            // 
            // GroupBox4
            // 
            this.GroupBox4.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox4.Controls.Add(this.picSwipe3);
            this.GroupBox4.Controls.Add(this.txtSwipeUser3);
            this.GroupBox4.Controls.Add(this.txtSwipeSeat3);
            this.GroupBox4.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox4, "GroupBox4");
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.TabStop = false;
            // 
            // picSwipe3
            // 
            resources.ApplyResources(this.picSwipe3, "picSwipe3");
            this.picSwipe3.Name = "picSwipe3";
            this.picSwipe3.TabStop = false;
            // 
            // txtSwipeUser3
            // 
            resources.ApplyResources(this.txtSwipeUser3, "txtSwipeUser3");
            this.txtSwipeUser3.BackColor = System.Drawing.Color.White;
            this.txtSwipeUser3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeUser3.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeUser3.Name = "txtSwipeUser3";
            this.txtSwipeUser3.ReadOnly = true;
            // 
            // txtSwipeSeat3
            // 
            resources.ApplyResources(this.txtSwipeSeat3, "txtSwipeSeat3");
            this.txtSwipeSeat3.BackColor = System.Drawing.Color.White;
            this.txtSwipeSeat3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeSeat3.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeSeat3.Name = "txtSwipeSeat3";
            this.txtSwipeSeat3.ReadOnly = true;
            // 
            // GroupBox6
            // 
            this.GroupBox6.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox6.Controls.Add(this.picSwipe4);
            this.GroupBox6.Controls.Add(this.txtSwipeUser4);
            this.GroupBox6.Controls.Add(this.txtSwipeSeat4);
            this.GroupBox6.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox6, "GroupBox6");
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.TabStop = false;
            // 
            // picSwipe4
            // 
            resources.ApplyResources(this.picSwipe4, "picSwipe4");
            this.picSwipe4.Name = "picSwipe4";
            this.picSwipe4.TabStop = false;
            // 
            // txtSwipeUser4
            // 
            resources.ApplyResources(this.txtSwipeUser4, "txtSwipeUser4");
            this.txtSwipeUser4.BackColor = System.Drawing.Color.White;
            this.txtSwipeUser4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeUser4.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeUser4.Name = "txtSwipeUser4";
            this.txtSwipeUser4.ReadOnly = true;
            // 
            // txtSwipeSeat4
            // 
            resources.ApplyResources(this.txtSwipeSeat4, "txtSwipeSeat4");
            this.txtSwipeSeat4.BackColor = System.Drawing.Color.White;
            this.txtSwipeSeat4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeSeat4.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeSeat4.Name = "txtSwipeSeat4";
            this.txtSwipeSeat4.ReadOnly = true;
            // 
            // GroupBox7
            // 
            this.GroupBox7.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox7.Controls.Add(this.picSwipe5);
            this.GroupBox7.Controls.Add(this.txtSwipeUser5);
            this.GroupBox7.Controls.Add(this.txtSwipeSeat5);
            this.GroupBox7.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox7, "GroupBox7");
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.TabStop = false;
            // 
            // picSwipe5
            // 
            resources.ApplyResources(this.picSwipe5, "picSwipe5");
            this.picSwipe5.Name = "picSwipe5";
            this.picSwipe5.TabStop = false;
            // 
            // txtSwipeUser5
            // 
            resources.ApplyResources(this.txtSwipeUser5, "txtSwipeUser5");
            this.txtSwipeUser5.BackColor = System.Drawing.Color.White;
            this.txtSwipeUser5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeUser5.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeUser5.Name = "txtSwipeUser5";
            this.txtSwipeUser5.ReadOnly = true;
            // 
            // txtSwipeSeat5
            // 
            resources.ApplyResources(this.txtSwipeSeat5, "txtSwipeSeat5");
            this.txtSwipeSeat5.BackColor = System.Drawing.Color.White;
            this.txtSwipeSeat5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSwipeSeat5.ForeColor = System.Drawing.Color.Black;
            this.txtSwipeSeat5.Name = "txtSwipeSeat5";
            this.txtSwipeSeat5.ReadOnly = true;
            // 
            // lblMeetingName
            // 
            resources.ApplyResources(this.lblMeetingName, "lblMeetingName");
            this.lblMeetingName.BackColor = System.Drawing.Color.Transparent;
            this.lblMeetingName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblMeetingName.Name = "lblMeetingName";
            // 
            // GroupBox5
            // 
            resources.ApplyResources(this.GroupBox5, "GroupBox5");
            this.GroupBox5.Controls.Add(this.button8);
            this.GroupBox5.Controls.Add(this.btnErrConnect);
            this.GroupBox5.Controls.Add(this.btnCancel);
            this.GroupBox5.Controls.Add(this.Button6);
            this.GroupBox5.Controls.Add(this.Button7);
            this.GroupBox5.Controls.Add(this.lblTime);
            this.GroupBox5.Controls.Add(this.Button3);
            this.GroupBox5.Controls.Add(this.Button2);
            this.GroupBox5.Controls.Add(this.Button1);
            this.GroupBox5.Controls.Add(this.Button4);
            this.GroupBox5.Controls.Add(this.Button5);
            this.GroupBox5.Controls.Add(this.txtA0);
            this.GroupBox5.Controls.Add(this.txtA1);
            this.GroupBox5.Controls.Add(this.txtA2);
            this.GroupBox5.Controls.Add(this.txtA3);
            this.GroupBox5.Controls.Add(this.txtA4);
            this.GroupBox5.Controls.Add(this.Label2);
            this.GroupBox5.Controls.Add(this.Label3);
            this.GroupBox5.Controls.Add(this.txtB0);
            this.GroupBox5.Controls.Add(this.txtB1);
            this.GroupBox5.Controls.Add(this.txtB2);
            this.GroupBox5.Controls.Add(this.txtB3);
            this.GroupBox5.Controls.Add(this.txtB4);
            this.GroupBox5.Controls.Add(this.Label4);
            this.GroupBox5.Controls.Add(this.txtC0);
            this.GroupBox5.Controls.Add(this.txtC1);
            this.GroupBox5.Controls.Add(this.txtC2);
            this.GroupBox5.Controls.Add(this.txtC3);
            this.GroupBox5.Controls.Add(this.txtC4);
            this.GroupBox5.Controls.Add(this.txtD0);
            this.GroupBox5.Controls.Add(this.txtD1);
            this.GroupBox5.Controls.Add(this.txtD2);
            this.GroupBox5.Controls.Add(this.Label5);
            this.GroupBox5.Controls.Add(this.txtD3);
            this.GroupBox5.Controls.Add(this.txtD4);
            this.GroupBox5.Controls.Add(this.Label6);
            this.GroupBox5.Controls.Add(this.txtE3);
            this.GroupBox5.Controls.Add(this.txtE4);
            this.GroupBox5.Controls.Add(this.txtE2);
            this.GroupBox5.Controls.Add(this.txtE1);
            this.GroupBox5.Controls.Add(this.txtE0);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.TabStop = false;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.button8, "button8");
            this.button8.Focusable = true;
            this.button8.ForeColor = System.Drawing.Color.White;
            this.button8.Name = "button8";
            this.button8.Toggle = false;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnErrConnect
            // 
            this.btnErrConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnErrConnect, "btnErrConnect");
            this.btnErrConnect.Focusable = true;
            this.btnErrConnect.ForeColor = System.Drawing.Color.White;
            this.btnErrConnect.Name = "btnErrConnect";
            this.btnErrConnect.Toggle = false;
            this.btnErrConnect.UseVisualStyleBackColor = false;
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
            // Button6
            // 
            this.Button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button6, "Button6");
            this.Button6.Focusable = true;
            this.Button6.ForeColor = System.Drawing.Color.White;
            this.Button6.Name = "Button6";
            this.Button6.Toggle = false;
            this.Button6.UseVisualStyleBackColor = false;
            this.Button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // Button7
            // 
            this.Button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button7, "Button7");
            this.Button7.Focusable = true;
            this.Button7.ForeColor = System.Drawing.Color.White;
            this.Button7.Name = "Button7";
            this.Button7.Toggle = false;
            this.Button7.UseVisualStyleBackColor = false;
            this.Button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTime, "lblTime");
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblTime.Name = "lblTime";
            // 
            // Button3
            // 
            this.Button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button3, "Button3");
            this.Button3.Focusable = true;
            this.Button3.ForeColor = System.Drawing.Color.White;
            this.Button3.Name = "Button3";
            this.Button3.Toggle = false;
            this.Button3.UseVisualStyleBackColor = false;
            this.Button3.Click += new System.EventHandler(this.Button7_Click);
            // 
            // Button2
            // 
            this.Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button2, "Button2");
            this.Button2.Focusable = true;
            this.Button2.ForeColor = System.Drawing.Color.White;
            this.Button2.Name = "Button2";
            this.Button2.Toggle = false;
            this.Button2.UseVisualStyleBackColor = false;
            this.Button2.Click += new System.EventHandler(this.Button7_Click);
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button1, "Button1");
            this.Button1.Focusable = true;
            this.Button1.ForeColor = System.Drawing.Color.White;
            this.Button1.Name = "Button1";
            this.Button1.Toggle = false;
            this.Button1.UseVisualStyleBackColor = false;
            this.Button1.Click += new System.EventHandler(this.Button7_Click);
            // 
            // Button4
            // 
            this.Button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button4, "Button4");
            this.Button4.Focusable = true;
            this.Button4.ForeColor = System.Drawing.Color.White;
            this.Button4.Name = "Button4";
            this.Button4.Toggle = false;
            this.Button4.UseVisualStyleBackColor = false;
            this.Button4.Click += new System.EventHandler(this.Button7_Click);
            // 
            // Button5
            // 
            this.Button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.Button5, "Button5");
            this.Button5.Focusable = true;
            this.Button5.ForeColor = System.Drawing.Color.White;
            this.Button5.Name = "Button5";
            this.Button5.Toggle = false;
            this.Button5.UseVisualStyleBackColor = false;
            this.Button5.Click += new System.EventHandler(this.Button7_Click);
            // 
            // txtA0
            // 
            this.txtA0.BackColor = System.Drawing.Color.White;
            this.txtA0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtA0, "txtA0");
            this.txtA0.ForeColor = System.Drawing.Color.Black;
            this.txtA0.Name = "txtA0";
            this.txtA0.ReadOnly = true;
            // 
            // txtA1
            // 
            this.txtA1.BackColor = System.Drawing.Color.White;
            this.txtA1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtA1, "txtA1");
            this.txtA1.ForeColor = System.Drawing.Color.Black;
            this.txtA1.Name = "txtA1";
            this.txtA1.ReadOnly = true;
            // 
            // txtA2
            // 
            this.txtA2.BackColor = System.Drawing.Color.White;
            this.txtA2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtA2, "txtA2");
            this.txtA2.ForeColor = System.Drawing.Color.Black;
            this.txtA2.Name = "txtA2";
            this.txtA2.ReadOnly = true;
            // 
            // txtA3
            // 
            this.txtA3.BackColor = System.Drawing.Color.White;
            this.txtA3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtA3, "txtA3");
            this.txtA3.ForeColor = System.Drawing.Color.Black;
            this.txtA3.Name = "txtA3";
            this.txtA3.ReadOnly = true;
            // 
            // txtA4
            // 
            this.txtA4.BackColor = System.Drawing.Color.White;
            this.txtA4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtA4, "txtA4");
            this.txtA4.ForeColor = System.Drawing.Color.Black;
            this.txtA4.Name = "txtA4";
            this.txtA4.ReadOnly = true;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label2.Name = "Label2";
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label3.Name = "Label3";
            // 
            // txtB0
            // 
            this.txtB0.BackColor = System.Drawing.Color.White;
            this.txtB0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtB0, "txtB0");
            this.txtB0.ForeColor = System.Drawing.Color.Black;
            this.txtB0.Name = "txtB0";
            this.txtB0.ReadOnly = true;
            // 
            // txtB1
            // 
            this.txtB1.BackColor = System.Drawing.Color.White;
            this.txtB1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtB1, "txtB1");
            this.txtB1.ForeColor = System.Drawing.Color.Black;
            this.txtB1.Name = "txtB1";
            this.txtB1.ReadOnly = true;
            // 
            // txtB2
            // 
            this.txtB2.BackColor = System.Drawing.Color.White;
            this.txtB2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtB2, "txtB2");
            this.txtB2.ForeColor = System.Drawing.Color.Black;
            this.txtB2.Name = "txtB2";
            this.txtB2.ReadOnly = true;
            // 
            // txtB3
            // 
            this.txtB3.BackColor = System.Drawing.Color.White;
            this.txtB3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtB3, "txtB3");
            this.txtB3.ForeColor = System.Drawing.Color.Black;
            this.txtB3.Name = "txtB3";
            this.txtB3.ReadOnly = true;
            // 
            // txtB4
            // 
            this.txtB4.BackColor = System.Drawing.Color.White;
            this.txtB4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtB4, "txtB4");
            this.txtB4.ForeColor = System.Drawing.Color.Black;
            this.txtB4.Name = "txtB4";
            this.txtB4.ReadOnly = true;
            // 
            // Label4
            // 
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label4.Name = "Label4";
            // 
            // txtC0
            // 
            this.txtC0.BackColor = System.Drawing.Color.White;
            this.txtC0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtC0, "txtC0");
            this.txtC0.ForeColor = System.Drawing.Color.Black;
            this.txtC0.Name = "txtC0";
            this.txtC0.ReadOnly = true;
            // 
            // txtC1
            // 
            this.txtC1.BackColor = System.Drawing.Color.White;
            this.txtC1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtC1, "txtC1");
            this.txtC1.ForeColor = System.Drawing.Color.Black;
            this.txtC1.Name = "txtC1";
            this.txtC1.ReadOnly = true;
            // 
            // txtC2
            // 
            this.txtC2.BackColor = System.Drawing.Color.White;
            this.txtC2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtC2, "txtC2");
            this.txtC2.ForeColor = System.Drawing.Color.Black;
            this.txtC2.Name = "txtC2";
            this.txtC2.ReadOnly = true;
            // 
            // txtC3
            // 
            this.txtC3.BackColor = System.Drawing.Color.White;
            this.txtC3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtC3, "txtC3");
            this.txtC3.ForeColor = System.Drawing.Color.Black;
            this.txtC3.Name = "txtC3";
            this.txtC3.ReadOnly = true;
            // 
            // txtC4
            // 
            this.txtC4.BackColor = System.Drawing.Color.White;
            this.txtC4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtC4, "txtC4");
            this.txtC4.ForeColor = System.Drawing.Color.Black;
            this.txtC4.Name = "txtC4";
            this.txtC4.ReadOnly = true;
            // 
            // txtD0
            // 
            this.txtD0.BackColor = System.Drawing.Color.White;
            this.txtD0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtD0, "txtD0");
            this.txtD0.ForeColor = System.Drawing.Color.Black;
            this.txtD0.Name = "txtD0";
            this.txtD0.ReadOnly = true;
            // 
            // txtD1
            // 
            this.txtD1.BackColor = System.Drawing.Color.White;
            this.txtD1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtD1, "txtD1");
            this.txtD1.ForeColor = System.Drawing.Color.Black;
            this.txtD1.Name = "txtD1";
            this.txtD1.ReadOnly = true;
            // 
            // txtD2
            // 
            this.txtD2.BackColor = System.Drawing.Color.White;
            this.txtD2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtD2, "txtD2");
            this.txtD2.ForeColor = System.Drawing.Color.Black;
            this.txtD2.Name = "txtD2";
            this.txtD2.ReadOnly = true;
            // 
            // Label5
            // 
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label5.Name = "Label5";
            // 
            // txtD3
            // 
            this.txtD3.BackColor = System.Drawing.Color.White;
            this.txtD3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtD3, "txtD3");
            this.txtD3.ForeColor = System.Drawing.Color.Black;
            this.txtD3.Name = "txtD3";
            this.txtD3.ReadOnly = true;
            // 
            // txtD4
            // 
            this.txtD4.BackColor = System.Drawing.Color.White;
            this.txtD4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtD4, "txtD4");
            this.txtD4.ForeColor = System.Drawing.Color.Black;
            this.txtD4.Name = "txtD4";
            this.txtD4.ReadOnly = true;
            // 
            // Label6
            // 
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label6.Name = "Label6";
            // 
            // txtE3
            // 
            this.txtE3.BackColor = System.Drawing.Color.White;
            this.txtE3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtE3, "txtE3");
            this.txtE3.ForeColor = System.Drawing.Color.Black;
            this.txtE3.Name = "txtE3";
            this.txtE3.ReadOnly = true;
            // 
            // txtE4
            // 
            this.txtE4.BackColor = System.Drawing.Color.White;
            this.txtE4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtE4, "txtE4");
            this.txtE4.ForeColor = System.Drawing.Color.Black;
            this.txtE4.Name = "txtE4";
            this.txtE4.ReadOnly = true;
            // 
            // txtE2
            // 
            this.txtE2.BackColor = System.Drawing.Color.White;
            this.txtE2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtE2, "txtE2");
            this.txtE2.ForeColor = System.Drawing.Color.Black;
            this.txtE2.Name = "txtE2";
            this.txtE2.ReadOnly = true;
            // 
            // txtE1
            // 
            this.txtE1.BackColor = System.Drawing.Color.White;
            this.txtE1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtE1, "txtE1");
            this.txtE1.ForeColor = System.Drawing.Color.Black;
            this.txtE1.Name = "txtE1";
            this.txtE1.ReadOnly = true;
            // 
            // txtE0
            // 
            this.txtE0.BackColor = System.Drawing.Color.White;
            this.txtE0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtE0, "txtE0");
            this.txtE0.ForeColor = System.Drawing.Color.Black;
            this.txtE0.Name = "txtE0";
            this.txtE0.ReadOnly = true;
            // 
            // GroupBox1
            // 
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Controls.Add(this.GroupBox8);
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            this.GroupBox1.SizeChanged += new System.EventHandler(this.GroupBox1_SizeChanged);
            // 
            // GroupBox8
            // 
            this.GroupBox8.Controls.Add(this.picSwipe6);
            this.GroupBox8.Controls.Add(this.txtSwipeUser6);
            this.GroupBox8.Controls.Add(this.txtSwipeSeat6);
            resources.ApplyResources(this.GroupBox8, "GroupBox8");
            this.GroupBox8.Name = "GroupBox8";
            this.GroupBox8.TabStop = false;
            // 
            // picSwipe6
            // 
            resources.ApplyResources(this.picSwipe6, "picSwipe6");
            this.picSwipe6.Name = "picSwipe6";
            this.picSwipe6.TabStop = false;
            // 
            // txtSwipeUser6
            // 
            this.txtSwipeUser6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtSwipeUser6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtSwipeUser6, "txtSwipeUser6");
            this.txtSwipeUser6.Name = "txtSwipeUser6";
            this.txtSwipeUser6.ReadOnly = true;
            // 
            // txtSwipeSeat6
            // 
            this.txtSwipeSeat6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtSwipeSeat6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtSwipeSeat6, "txtSwipeSeat6");
            this.txtSwipeSeat6.Name = "txtSwipeSeat6";
            this.txtSwipeSeat6.ReadOnly = true;
            // 
            // dfrmMeetingSign
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lblMeetingName);
            this.Controls.Add(this.GroupBox5);
            this.Controls.Add(this.GroupBox1);
            this.MinimizeBox = false;
            this.Name = "dfrmMeetingSign";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.dfrmMeetingSign_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmMeetingSign_FormClosing);
            this.Load += new System.EventHandler(this.dfrmMeetingSign_Load);
            this.SizeChanged += new System.EventHandler(this.dfrmMeetingSign_SizeChanged);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe1)).EndInit();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe2)).EndInit();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe3)).EndInit();
            this.GroupBox6.ResumeLayout(false);
            this.GroupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe4)).EndInit();
            this.GroupBox7.ResumeLayout(false);
            this.GroupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe5)).EndInit();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox5.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox8.ResumeLayout(false);
            this.GroupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSwipe6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton btnErrConnect;
        internal ImageButton Button1;
        internal ImageButton Button2;
        internal ImageButton Button3;
        internal ImageButton Button4;
        internal ImageButton Button5;
        internal ImageButton Button6;
        internal ImageButton Button7;
        internal ImageButton button8;
        private FlowLayoutPanel flowLayoutPanel1;
        internal GroupBox GroupBox1;
        internal GroupBox GroupBox2;
        internal GroupBox GroupBox3;
        internal GroupBox GroupBox4;
        internal GroupBox GroupBox5;
        internal GroupBox GroupBox6;
        internal GroupBox GroupBox7;
        internal GroupBox GroupBox8;
        internal Label Label2;
        internal Label Label3;
        internal Label Label4;
        internal Label Label5;
        internal Label Label6;
        internal Label lblMeetingName;
        internal Label lblTime;
        internal PictureBox picSwipe1;
        internal PictureBox picSwipe2;
        internal PictureBox picSwipe3;
        internal PictureBox picSwipe4;
        internal PictureBox picSwipe5;
        internal PictureBox picSwipe6;
        private DateTime signEndtime;
        private DateTime signStarttime;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.Timer Timer2;
        internal System.Windows.Forms.Timer TimerStartSlow;
        internal TextBox txtA0;
        internal TextBox txtA1;
        internal TextBox txtA2;
        internal TextBox txtA3;
        internal TextBox txtA4;
        internal TextBox txtB0;
        internal TextBox txtB1;
        internal TextBox txtB2;
        internal TextBox txtB3;
        internal TextBox txtB4;
        internal TextBox txtC0;
        internal TextBox txtC1;
        internal TextBox txtC2;
        internal TextBox txtC3;
        internal TextBox txtC4;
        internal TextBox txtD0;
        internal TextBox txtD1;
        internal TextBox txtD2;
        internal TextBox txtD3;
        internal TextBox txtD4;
        internal TextBox txtE0;
        internal TextBox txtE1;
        internal TextBox txtE2;
        internal TextBox txtE3;
        internal TextBox txtE4;
        internal TextBox txtSwipeSeat1;
        internal TextBox txtSwipeSeat2;
        internal TextBox txtSwipeSeat3;
        internal TextBox txtSwipeSeat4;
        internal TextBox txtSwipeSeat5;
        internal TextBox txtSwipeSeat6;
        internal TextBox txtSwipeUser1;
        internal TextBox txtSwipeUser2;
        internal TextBox txtSwipeUser3;
        internal TextBox txtSwipeUser4;
        internal TextBox txtSwipeUser5;
        internal TextBox txtSwipeUser6;
    }
}

