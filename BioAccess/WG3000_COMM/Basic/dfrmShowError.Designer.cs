namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    partial class dfrmShowError
    {
        private ImageButton btnCopyDetail;
        private ImageButton btnDetail;
        private ImageButton btnOK;
        private IContainer components = null;
        private Label label1;
        private TextBox txtErrorDetail;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmShowError));
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.txtErrorDetail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopyDetail = new System.Windows.Forms.ImageButton();
            this.btnDetail = new System.Windows.Forms.ImageButton();
            this.SuspendLayout();
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
            // txtErrorDetail
            // 
            resources.ApplyResources(this.txtErrorDetail, "txtErrorDetail");
            this.txtErrorDetail.Name = "txtErrorDetail";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // btnCopyDetail
            // 
            this.btnCopyDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnCopyDetail, "btnCopyDetail");
            this.btnCopyDetail.Focusable = true;
            this.btnCopyDetail.ForeColor = System.Drawing.Color.White;
            this.btnCopyDetail.Name = "btnCopyDetail";
            this.btnCopyDetail.Toggle = false;
            this.btnCopyDetail.UseVisualStyleBackColor = false;
            this.btnCopyDetail.Click += new System.EventHandler(this.btnCopyDetail_Click);
            // 
            // btnDetail
            // 
            this.btnDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDetail, "btnDetail");
            this.btnDetail.Focusable = true;
            this.btnDetail.ForeColor = System.Drawing.Color.White;
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Toggle = false;
            this.btnDetail.UseVisualStyleBackColor = false;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // dfrmShowError
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.txtErrorDetail);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDetail);
            this.Controls.Add(this.btnCopyDetail);
            this.MinimizeBox = false;
            this.Name = "dfrmShowError";
            this.Load += new System.EventHandler(this.dfrmShowError_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

