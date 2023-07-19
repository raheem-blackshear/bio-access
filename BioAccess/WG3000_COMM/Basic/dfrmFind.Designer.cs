namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmFind
    {
        private ImageButton btnClose;
        private ImageButton btnFind;
        public ImageButton btnMarkAll;
        private IContainer components = null;
        private Label Found;
        private Label label1;
        private Label lblCount;
        private TextBox txtFind;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmFind));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.ImageButton();
            this.btnMarkAll = new System.Windows.Forms.ImageButton();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.Found = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // txtFind
            // 
            resources.ApplyResources(this.txtFind, "txtFind");
            this.txtFind.Name = "txtFind";
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFind_KeyDown);
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Transparent;
            this.btnFind.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnFind, "btnFind");
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Name = "btnFind";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnMarkAll
            // 
            this.btnMarkAll.BackColor = System.Drawing.Color.Transparent;
            this.btnMarkAll.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnMarkAll, "btnMarkAll");
            this.btnMarkAll.ForeColor = System.Drawing.Color.White;
            this.btnMarkAll.Name = "btnMarkAll";
            this.btnMarkAll.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Properties.Resources.pMain_button_normal;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Found
            // 
            resources.ApplyResources(this.Found, "Found");
            this.Found.BackColor = System.Drawing.Color.Transparent;
            this.Found.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Found.Name = "Found";
            // 
            // lblCount
            // 
            resources.ApplyResources(this.lblCount, "lblCount");
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.ForeColor = System.Drawing.Color.White;
            this.lblCount.Name = "lblCount";
            // 
            // dfrmFind
            // 
            this.AcceptButton = this.btnFind;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.Found);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnMarkAll);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dfrmFind";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmFind_FormClosing);
            this.Load += new System.EventHandler(this.dfrmFind_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

