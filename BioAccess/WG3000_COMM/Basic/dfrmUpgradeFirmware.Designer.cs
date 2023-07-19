namespace WG3000_COMM.Basic
{
    partial class dfrmUpgradeFirmware
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmUpgradeFirmware));
            this.lblFirmware = new System.Windows.Forms.Label();
            this.lblController = new System.Windows.Forms.Label();
            this.cmbController = new System.Windows.Forms.ComboBox();
            this.btnSelectFirmware = new System.Windows.Forms.ImageButton();
            this.btnUpgrade = new System.Windows.Forms.ImageButton();
            this.progUpgrading = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProg = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtFirmware = new System.Windows.Forms.TextBox();
            this.comdlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFirmware
            // 
            this.lblFirmware.BackColor = System.Drawing.Color.Transparent;
            this.lblFirmware.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblFirmware, "lblFirmware");
            this.lblFirmware.Name = "lblFirmware";
            // 
            // lblController
            // 
            this.lblController.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblController, "lblController");
            this.lblController.Name = "lblController";
            // 
            // cmbController
            // 
            this.cmbController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbController, "cmbController");
            this.cmbController.FormattingEnabled = true;
            this.cmbController.Name = "cmbController";
            // 
            // btnSelectFirmware
            // 
            this.btnSelectFirmware.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnSelectFirmware, "btnSelectFirmware");
            this.btnSelectFirmware.Focusable = true;
            this.btnSelectFirmware.ForeColor = System.Drawing.Color.White;
            this.btnSelectFirmware.Name = "btnSelectFirmware";
            this.btnSelectFirmware.Toggle = false;
            this.btnSelectFirmware.UseVisualStyleBackColor = true;
            this.btnSelectFirmware.Click += new System.EventHandler(this.btnSelectFirmware_Click);
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnUpgrade, "btnUpgrade");
            this.btnUpgrade.Focusable = true;
            this.btnUpgrade.ForeColor = System.Drawing.Color.White;
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Toggle = false;
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // progUpgrading
            // 
            resources.ApplyResources(this.progUpgrading, "progUpgrading");
            this.progUpgrading.Name = "progUpgrading";
            this.progUpgrading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblProg);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.progUpgrading);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnUpgrade);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblProg
            // 
            this.lblProg.BackColor = System.Drawing.Color.Transparent;
            this.lblProg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblProg, "lblProg");
            this.lblProg.Name = "lblProg";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // txtFirmware
            // 
            resources.ApplyResources(this.txtFirmware, "txtFirmware");
            this.txtFirmware.Name = "txtFirmware";
            // 
            // dfrmUpgradeFirmware
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtFirmware);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSelectFirmware);
            this.Controls.Add(this.lblController);
            this.Controls.Add(this.cmbController);
            this.Controls.Add(this.lblFirmware);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmUpgradeFirmware";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmUpgradeFirmware_FormClosing);
            this.Load += new System.EventHandler(this.dfrmUpgradeFirmware_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblFirmware;
        private System.Windows.Forms.Label lblController;
        private System.Windows.Forms.ComboBox cmbController;
        private System.Windows.Forms.ImageButton btnSelectFirmware;
        private System.Windows.Forms.ImageButton btnUpgrade;
        private System.Windows.Forms.ProgressBar progUpgrading;
        private System.Windows.Forms.ImageButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtFirmware;
        private System.Windows.Forms.OpenFileDialog comdlgOpen;
        internal System.Windows.Forms.Label lblProg;
    }
}