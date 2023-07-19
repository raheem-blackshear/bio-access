namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmUsersCardLost
    {
        private ImageButton btnCancel;
        private ImageButton btnOK;
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        public MaskedTextBox txtf_CardNO;
        public MaskedTextBox txtf_CardNONew;
        public TextBox txtf_ConsumerName;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmUsersCardLost));
            this.txtf_CardNO = new System.Windows.Forms.MaskedTextBox();
            this.txtf_ConsumerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtf_CardNONew = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtf_CardNO
            // 
            resources.ApplyResources(this.txtf_CardNO, "txtf_CardNO");
            this.txtf_CardNO.Name = "txtf_CardNO";
            this.txtf_CardNO.ReadOnly = true;
            // 
            // txtf_ConsumerName
            // 
            resources.ApplyResources(this.txtf_ConsumerName, "txtf_ConsumerName");
            this.txtf_ConsumerName.Name = "txtf_ConsumerName";
            this.txtf_ConsumerName.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label3.Name = "label3";
            // 
            // txtf_CardNONew
            // 
            resources.ApplyResources(this.txtf_CardNONew, "txtf_CardNONew");
            this.txtf_CardNONew.Name = "txtf_CardNONew";
            this.txtf_CardNONew.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtf_CardNONew_KeyPress);
            this.txtf_CardNONew.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtf_CardNONew_KeyUp);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
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
            // dfrmUsersCardLost
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.txtf_CardNONew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtf_CardNO);
            this.Controls.Add(this.txtf_ConsumerName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmUsersCardLost";
            this.Load += new System.EventHandler(this.dfrmUsersCardLost_Load);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

