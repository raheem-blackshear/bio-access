namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmOptionAdvanced
    {
        private ImageButton btnBrowse;
        private CheckBox chkAllowUploadUserName;
        private CheckBox chkValidSwipeGap;
        internal ImageButton cmdCancel;
        internal ImageButton cmdOK;
        private IContainer components = null;
        private FolderBrowserDialog folderBrowserDialog1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label2;
        private NumericUpDown nudValidSwipeGap;
        private TextBox txtPhotoDirectory;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmOptionAdvanced));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAllowUploadUserName = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudValidSwipeGap = new System.Windows.Forms.NumericUpDown();
            this.chkValidSwipeGap = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.ImageButton();
            this.txtPhotoDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.ImageButton();
            this.cmdOK = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValidSwipeGap)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAllowUploadUserName);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkAllowUploadUserName
            // 
            resources.ApplyResources(this.chkAllowUploadUserName, "chkAllowUploadUserName");
            this.chkAllowUploadUserName.BackColor = System.Drawing.Color.Transparent;
            this.chkAllowUploadUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkAllowUploadUserName.Name = "chkAllowUploadUserName";
            this.chkAllowUploadUserName.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.nudValidSwipeGap);
            this.groupBox1.Controls.Add(this.chkValidSwipeGap);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtPhotoDirectory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // nudValidSwipeGap
            // 
            resources.ApplyResources(this.nudValidSwipeGap, "nudValidSwipeGap");
            this.nudValidSwipeGap.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudValidSwipeGap.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.nudValidSwipeGap.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nudValidSwipeGap.Name = "nudValidSwipeGap";
            this.nudValidSwipeGap.ReadOnly = true;
            this.nudValidSwipeGap.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // chkValidSwipeGap
            // 
            resources.ApplyResources(this.chkValidSwipeGap, "chkValidSwipeGap");
            this.chkValidSwipeGap.BackColor = System.Drawing.Color.Transparent;
            this.chkValidSwipeGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkValidSwipeGap.Name = "chkValidSwipeGap";
            this.chkValidSwipeGap.UseVisualStyleBackColor = false;
            this.chkValidSwipeGap.CheckedChanged += new System.EventHandler(this.chkValidSwipeGap_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Focusable = true;
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Toggle = false;
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtPhotoDirectory
            // 
            resources.ApplyResources(this.txtPhotoDirectory, "txtPhotoDirectory");
            this.txtPhotoDirectory.Name = "txtPhotoDirectory";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label2.Name = "label2";
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdCancel.Focusable = true;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Toggle = false;
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
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
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdOK);
            this.panelBottomBanner.Controls.Add(this.cmdCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmOptionAdvanced
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "dfrmOptionAdvanced";
            this.Load += new System.EventHandler(this.dfrmOptionAdvanced_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmOptionAdvanced_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValidSwipeGap)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel panelBottomBanner;
    }
}

