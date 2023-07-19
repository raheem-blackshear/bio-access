namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmOption
    {
        internal ImageButton btnOption;
        private ImageButton btnRefreshDateTime;
        private ImageButton btnRefreshDateTimeWeek;
        private ImageButton btnRefreshDateWeek;
        private ImageButton btnRefreshOnlyDate;
        private ComboBox cboDateTime;
        private ComboBox cboDateTimeWeek;
        private ComboBox cboDateWeek;
        private ComboBox cboLanguage;
        private ComboBox cboOnlyDate;
        private CheckBox chkAutoLoginOnly;
        private CheckBox chkHideLogin;
        private CheckBox chkHouse;
        internal ImageButton cmdCancel;
        internal ImageButton cmdOK;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox txtDateTime;
        private TextBox txtDateTimeWeek;
        private TextBox txtDateWeek;
        private TextBox txtOnlyDate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmOption));
            this.cmdOK = new System.Windows.Forms.ImageButton();
            this.cmdCancel = new System.Windows.Forms.ImageButton();
            this.chkHideLogin = new System.Windows.Forms.CheckBox();
            this.btnOption = new System.Windows.Forms.ImageButton();
            this.chkHouse = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRefreshDateTime = new System.Windows.Forms.ImageButton();
            this.btnRefreshDateTimeWeek = new System.Windows.Forms.ImageButton();
            this.btnRefreshDateWeek = new System.Windows.Forms.ImageButton();
            this.btnRefreshOnlyDate = new System.Windows.Forms.ImageButton();
            this.txtDateTime = new System.Windows.Forms.TextBox();
            this.txtDateTimeWeek = new System.Windows.Forms.TextBox();
            this.cboDateTime = new System.Windows.Forms.ComboBox();
            this.cboDateTimeWeek = new System.Windows.Forms.ComboBox();
            this.txtDateWeek = new System.Windows.Forms.TextBox();
            this.cboDateWeek = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOnlyDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboOnlyDate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkAutoLoginOnly = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            resources.ApplyResources(this.cmdOK, "cmdOK");
            this.cmdOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdOK.Focusable = true;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Toggle = false;
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Focusable = true;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Toggle = false;
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chkHideLogin
            // 
            resources.ApplyResources(this.chkHideLogin, "chkHideLogin");
            this.chkHideLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkHideLogin.Name = "chkHideLogin";
            this.chkHideLogin.UseVisualStyleBackColor = true;
            // 
            // btnOption
            // 
            this.btnOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOption, "btnOption");
            this.btnOption.Focusable = true;
            this.btnOption.ForeColor = System.Drawing.Color.White;
            this.btnOption.Name = "btnOption";
            this.btnOption.Toggle = false;
            this.btnOption.UseVisualStyleBackColor = false;
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // chkHouse
            // 
            resources.ApplyResources(this.chkHouse, "chkHouse");
            this.chkHouse.BackColor = System.Drawing.Color.Transparent;
            this.chkHouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkHouse.Name = "chkHouse";
            this.chkHouse.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.btnRefreshDateTime);
            this.tabPage1.Controls.Add(this.btnRefreshDateTimeWeek);
            this.tabPage1.Controls.Add(this.btnRefreshDateWeek);
            this.tabPage1.Controls.Add(this.btnRefreshOnlyDate);
            this.tabPage1.Controls.Add(this.txtDateTime);
            this.tabPage1.Controls.Add(this.txtDateTimeWeek);
            this.tabPage1.Controls.Add(this.cboDateTime);
            this.tabPage1.Controls.Add(this.cboDateTimeWeek);
            this.tabPage1.Controls.Add(this.txtDateWeek);
            this.tabPage1.Controls.Add(this.cboDateWeek);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtOnlyDate);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cboOnlyDate);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.ForeColor = System.Drawing.Color.White;
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnRefreshDateTime
            // 
            this.btnRefreshDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRefreshDateTime, "btnRefreshDateTime");
            this.btnRefreshDateTime.Focusable = true;
            this.btnRefreshDateTime.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDateTime.Name = "btnRefreshDateTime";
            this.btnRefreshDateTime.Toggle = false;
            this.btnRefreshDateTime.UseVisualStyleBackColor = false;
            this.btnRefreshDateTime.Click += new System.EventHandler(this.btnRefreshDateTime_Click);
            // 
            // btnRefreshDateTimeWeek
            // 
            this.btnRefreshDateTimeWeek.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRefreshDateTimeWeek, "btnRefreshDateTimeWeek");
            this.btnRefreshDateTimeWeek.Focusable = true;
            this.btnRefreshDateTimeWeek.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDateTimeWeek.Name = "btnRefreshDateTimeWeek";
            this.btnRefreshDateTimeWeek.Toggle = false;
            this.btnRefreshDateTimeWeek.UseVisualStyleBackColor = false;
            this.btnRefreshDateTimeWeek.Click += new System.EventHandler(this.btnRefreshDateTimeWeek_Click);
            // 
            // btnRefreshDateWeek
            // 
            this.btnRefreshDateWeek.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRefreshDateWeek, "btnRefreshDateWeek");
            this.btnRefreshDateWeek.Focusable = true;
            this.btnRefreshDateWeek.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDateWeek.Name = "btnRefreshDateWeek";
            this.btnRefreshDateWeek.Toggle = false;
            this.btnRefreshDateWeek.UseVisualStyleBackColor = false;
            this.btnRefreshDateWeek.Click += new System.EventHandler(this.btnRefreshDateWeek_Click);
            // 
            // btnRefreshOnlyDate
            // 
            this.btnRefreshOnlyDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnRefreshOnlyDate, "btnRefreshOnlyDate");
            this.btnRefreshOnlyDate.Focusable = true;
            this.btnRefreshOnlyDate.ForeColor = System.Drawing.Color.White;
            this.btnRefreshOnlyDate.Name = "btnRefreshOnlyDate";
            this.btnRefreshOnlyDate.Toggle = false;
            this.btnRefreshOnlyDate.UseVisualStyleBackColor = false;
            this.btnRefreshOnlyDate.Click += new System.EventHandler(this.btnRefreshOnlyDate_Click);
            // 
            // txtDateTime
            // 
            resources.ApplyResources(this.txtDateTime, "txtDateTime");
            this.txtDateTime.Name = "txtDateTime";
            // 
            // txtDateTimeWeek
            // 
            resources.ApplyResources(this.txtDateTimeWeek, "txtDateTimeWeek");
            this.txtDateTimeWeek.Name = "txtDateTimeWeek";
            // 
            // cboDateTime
            // 
            this.cboDateTime.FormattingEnabled = true;
            resources.ApplyResources(this.cboDateTime, "cboDateTime");
            this.cboDateTime.Name = "cboDateTime";
            this.cboDateTime.SelectedIndexChanged += new System.EventHandler(this.btnRefreshDateTime_Click);
            // 
            // cboDateTimeWeek
            // 
            this.cboDateTimeWeek.FormattingEnabled = true;
            resources.ApplyResources(this.cboDateTimeWeek, "cboDateTimeWeek");
            this.cboDateTimeWeek.Name = "cboDateTimeWeek";
            this.cboDateTimeWeek.SelectedIndexChanged += new System.EventHandler(this.btnRefreshDateTimeWeek_Click);
            // 
            // txtDateWeek
            // 
            resources.ApplyResources(this.txtDateWeek, "txtDateWeek");
            this.txtDateWeek.Name = "txtDateWeek";
            // 
            // cboDateWeek
            // 
            this.cboDateWeek.FormattingEnabled = true;
            resources.ApplyResources(this.cboDateWeek, "cboDateWeek");
            this.cboDateWeek.Name = "cboDateWeek";
            this.cboDateWeek.SelectedIndexChanged += new System.EventHandler(this.btnRefreshDateWeek_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtOnlyDate
            // 
            resources.ApplyResources(this.txtOnlyDate, "txtOnlyDate");
            this.txtOnlyDate.Name = "txtOnlyDate";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cboOnlyDate
            // 
            this.cboOnlyDate.FormattingEnabled = true;
            resources.ApplyResources(this.cboOnlyDate, "cboOnlyDate");
            this.cboOnlyDate.Name = "cboOnlyDate";
            this.cboOnlyDate.SelectedIndexChanged += new System.EventHandler(this.btnRefreshOnlyDate_Click);
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
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkAutoLoginOnly
            // 
            resources.ApplyResources(this.chkAutoLoginOnly, "chkAutoLoginOnly");
            this.chkAutoLoginOnly.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoLoginOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkAutoLoginOnly.Name = "chkAutoLoginOnly";
            this.chkAutoLoginOnly.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboLanguage);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label5.Name = "label5";
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.cboLanguage, "cboLanguage");
            this.cboLanguage.Name = "cboLanguage";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdCancel);
            this.panelBottomBanner.Controls.Add(this.cmdOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmOption
            // 
            this.AcceptButton = this.cmdOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cmdCancel;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkHideLogin);
            this.Controls.Add(this.btnOption);
            this.Controls.Add(this.chkHouse);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.chkAutoLoginOnly);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmOption";
            this.Load += new System.EventHandler(this.dfrmOption_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmOption_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

