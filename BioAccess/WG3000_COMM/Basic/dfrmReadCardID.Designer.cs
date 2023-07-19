namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmReadCardID : frmBioAccess
    {
        private ImageButton btnCancel;
        private ImageButton btnExit;
        private ImageButton btnNext;
        private ImageButton btnOK;
        private ImageButton button1;
        private ComboBox cboDoors;
        private IContainer components;
        private icController control;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private DataTable dt;
        private DataView dvDoors;
        private DataView dvDoors4Watching;
        public Form frmCall;
        private GroupBox groupBox1;
        private RadioButton optController;
        private RadioButton optUSBReader;
        private Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.dfrmWait1 != null))
            {
                this.dfrmWait1.Dispose();
            }
            if (disposing && (this.control != null))
            {
                this.control.Dispose();
            }
            if ((this.bDisposeWatching && disposing) && (this.watching != null))
            {
                this.watching.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmReadCardID));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCardID = new System.Windows.Forms.TextBox();
            this.lblCardID = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.button1 = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnNext = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboDoors = new System.Windows.Forms.ComboBox();
            this.optController = new System.Windows.Forms.RadioButton();
            this.optUSBReader = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.txtCardID);
            this.groupBox2.Controls.Add(this.lblCardID);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtCardID
            // 
            resources.ApplyResources(this.txtCardID, "txtCardID");
            this.txtCardID.Name = "txtCardID";
            this.txtCardID.ReadOnly = true;
            // 
            // lblCardID
            // 
            resources.ApplyResources(this.lblCardID, "lblCardID");
            this.lblCardID.Name = "lblCardID";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.button1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.Toggle = false;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.button1.Focusable = true;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.Toggle = false;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnNext);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnNext.Focusable = true;
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Name = "btnNext";
            this.btnNext.Toggle = false;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cboDoors);
            this.groupBox1.Controls.Add(this.optController);
            this.groupBox1.Controls.Add(this.optUSBReader);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cboDoors
            // 
            this.cboDoors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboDoors, "cboDoors");
            this.cboDoors.FormattingEnabled = true;
            this.cboDoors.Name = "cboDoors";
            // 
            // optController
            // 
            resources.ApplyResources(this.optController, "optController");
            this.optController.Name = "optController";
            this.optController.UseVisualStyleBackColor = true;
            this.optController.CheckedChanged += new System.EventHandler(this.optController_CheckedChanged);
            // 
            // optUSBReader
            // 
            resources.ApplyResources(this.optUSBReader, "optUSBReader");
            this.optUSBReader.Checked = true;
            this.optUSBReader.Name = "optUSBReader";
            this.optUSBReader.TabStop = true;
            this.optUSBReader.UseVisualStyleBackColor = true;
            // 
            // dfrmReadCardID
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmReadCardID";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmReadCardID_FormClosing);
            this.Load += new System.EventHandler(this.dfrmReadCardID_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmReadCardID_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panelBottomBanner.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private Panel panelBottomBanner;
        private Panel panel1;
        private GroupBox groupBox2;
        private TextBox txtCardID;
        private Label lblCardID;
    }
}

