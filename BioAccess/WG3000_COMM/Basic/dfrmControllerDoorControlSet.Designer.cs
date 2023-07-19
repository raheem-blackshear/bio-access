namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    partial class dfrmControllerDoorControlSet
    {
        public ImageButton btnCancel;
        public ImageButton btnNormalClose;
        private ImageButton btnNormalOpen;
        private ImageButton btnOnline;
        private IContainer components = null;
        private Label label1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerDoorControlSet));
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOnline = new System.Windows.Forms.ImageButton();
            this.btnNormalClose = new System.Windows.Forms.ImageButton();
            this.btnNormalOpen = new System.Windows.Forms.ImageButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Name = "label1";
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
            // btnOnline
            // 
            this.btnOnline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOnline, "btnOnline");
            this.btnOnline.Focusable = true;
            this.btnOnline.ForeColor = System.Drawing.Color.White;
            this.btnOnline.Name = "btnOnline";
            this.btnOnline.Toggle = false;
            this.btnOnline.UseVisualStyleBackColor = false;
            this.btnOnline.Click += new System.EventHandler(this.btnOnline_Click);
            // 
            // btnNormalClose
            // 
            this.btnNormalClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnNormalClose, "btnNormalClose");
            this.btnNormalClose.Focusable = true;
            this.btnNormalClose.ForeColor = System.Drawing.Color.White;
            this.btnNormalClose.Name = "btnNormalClose";
            this.btnNormalClose.Toggle = false;
            this.btnNormalClose.UseVisualStyleBackColor = false;
            this.btnNormalClose.Click += new System.EventHandler(this.btnOnline_Click);
            // 
            // btnNormalOpen
            // 
            this.btnNormalOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnNormalOpen, "btnNormalOpen");
            this.btnNormalOpen.Focusable = true;
            this.btnNormalOpen.ForeColor = System.Drawing.Color.White;
            this.btnNormalOpen.Name = "btnNormalOpen";
            this.btnNormalOpen.Toggle = false;
            this.btnNormalOpen.UseVisualStyleBackColor = false;
            this.btnNormalOpen.Click += new System.EventHandler(this.btnOnline_Click);
            // 
            // dfrmControllerDoorControlSet
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOnline);
            this.Controls.Add(this.btnNormalClose);
            this.Controls.Add(this.btnNormalOpen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmControllerDoorControlSet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

