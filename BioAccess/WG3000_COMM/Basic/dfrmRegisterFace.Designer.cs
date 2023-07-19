namespace WG3000_COMM.Basic
{
    partial class dfrmRegisterFace
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmRegisterFace));
			this.lblResult = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.ImageButton();
			this.btnOK = new System.Windows.Forms.ImageButton();
			this.lblController = new System.Windows.Forms.Label();
			this.cmbController = new System.Windows.Forms.ComboBox();
			this.panelBottomBanner = new System.Windows.Forms.Panel();
			this.groupProcessed = new System.Windows.Forms.GroupBox();
			this.picFace = new System.Windows.Forms.PictureBox();
			this.btnDelete = new System.Windows.Forms.ImageButton();
			this.btnScan = new System.Windows.Forms.ImageButton();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.mnuCameraSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.panelBottomBanner.SuspendLayout();
			this.groupProcessed.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFace)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblResult
			// 
			this.lblResult.ForeColor = System.Drawing.Color.Blue;
			resources.ApplyResources(this.lblResult, "lblResult");
			this.lblResult.Name = "lblResult";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
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
			// lblController
			// 
			resources.ApplyResources(this.lblController, "lblController");
			this.lblController.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
			this.lblController.Name = "lblController";
			// 
			// cmbController
			// 
			this.cmbController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cmbController, "cmbController");
			this.cmbController.FormattingEnabled = true;
			this.cmbController.Name = "cmbController";
			// 
			// panelBottomBanner
			// 
			this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
			this.panelBottomBanner.Controls.Add(this.btnCancel);
			this.panelBottomBanner.Controls.Add(this.btnOK);
			resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
			this.panelBottomBanner.Name = "panelBottomBanner";
			// 
			// groupProcessed
			// 
			this.groupProcessed.Controls.Add(this.picFace);
			this.groupProcessed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
			resources.ApplyResources(this.groupProcessed, "groupProcessed");
			this.groupProcessed.Name = "groupProcessed";
			this.groupProcessed.TabStop = false;
			// 
			// picFace
			// 
			this.picFace.BackColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.picFace, "picFace");
			this.picFace.Name = "picFace";
			this.picFace.TabStop = false;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			resources.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.Focusable = true;
			this.btnDelete.ForeColor = System.Drawing.Color.White;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Toggle = false;
			this.btnDelete.UseVisualStyleBackColor = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnScan
			// 
			this.btnScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
			resources.ApplyResources(this.btnScan, "btnScan");
			this.btnScan.Focusable = true;
			this.btnScan.ForeColor = System.Drawing.Color.White;
			this.btnScan.Name = "btnScan";
			this.btnScan.Toggle = false;
			this.btnScan.UseVisualStyleBackColor = false;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCameraSetting});
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Name = "menuStrip1";
			// 
			// mnuCameraSetting
			// 
			this.mnuCameraSetting.Name = "mnuCameraSetting";
			resources.ApplyResources(this.mnuCameraSetting, "mnuCameraSetting");
			this.mnuCameraSetting.Click += new System.EventHandler(this.mnuCameraSetting_Click);
			// 
			// dfrmRegisterFace
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.groupProcessed);
			this.Controls.Add(this.panelBottomBanner);
			this.Controls.Add(this.lblResult);
			this.Controls.Add(this.lblController);
			this.Controls.Add(this.cmbController);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimizeBox = false;
			this.Name = "dfrmRegisterFace";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmRegisterFace_FormClosing);
			this.Load += new System.EventHandler(this.dfrmRegisterFace_Load);
			this.panelBottomBanner.ResumeLayout(false);
			this.groupProcessed.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picFace)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbController;
		private System.Windows.Forms.Label lblController;
        private System.Windows.Forms.ImageButton btnCancel;
		private System.Windows.Forms.ImageButton btnOK;
        private System.Windows.Forms.Label lblResult;
		private System.Windows.Forms.Panel panelBottomBanner;
		private System.Windows.Forms.GroupBox groupProcessed;
		private System.Windows.Forms.PictureBox picFace;
		private System.Windows.Forms.ImageButton btnDelete;
		private System.Windows.Forms.ImageButton btnScan;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem mnuCameraSetting;
    }
}