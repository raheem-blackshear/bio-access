namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    partial class dfrmUploadOption
    {
        private ImageButton btnCancel;
        private ImageButton btnOK;
        private CheckBox chkAccessPrivilege;
        private CheckBox chkBasicConfiguration;
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmUploadOption));
            this.chkBasicConfiguration = new System.Windows.Forms.CheckBox();
            this.chkAccessPrivilege = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkBasicConfiguration
            // 
            resources.ApplyResources(this.chkBasicConfiguration, "chkBasicConfiguration");
            this.chkBasicConfiguration.BackColor = System.Drawing.Color.Transparent;
            this.chkBasicConfiguration.Checked = true;
            this.chkBasicConfiguration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBasicConfiguration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkBasicConfiguration.Name = "chkBasicConfiguration";
            this.chkBasicConfiguration.UseVisualStyleBackColor = false;
            // 
            // chkAccessPrivilege
            // 
            resources.ApplyResources(this.chkAccessPrivilege, "chkAccessPrivilege");
            this.chkAccessPrivilege.BackColor = System.Drawing.Color.Transparent;
            this.chkAccessPrivilege.Checked = true;
            this.chkAccessPrivilege.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAccessPrivilege.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkAccessPrivilege.Name = "chkAccessPrivilege";
            this.chkAccessPrivilege.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmUploadOption
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkAccessPrivilege);
            this.Controls.Add(this.chkBasicConfiguration);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmUploadOption";
            this.Load += new System.EventHandler(this.dfrmUploadOption_Load);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

