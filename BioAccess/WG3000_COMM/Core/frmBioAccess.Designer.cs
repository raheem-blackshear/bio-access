namespace WG3000_COMM.Core
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class frmBioAccess
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(211, 231, 251);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.ClientSize = new Size(0x318, 0x236);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "frmBioAccess";
            this.Load += new System.EventHandler(this.frmBioAccess_Load);
            this.ResumeLayout(false);

        }
    }
}

