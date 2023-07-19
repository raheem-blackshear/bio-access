namespace WG3000_COMM.Core
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class XMessageBox : Form
    {
        private Panel bottomInnerPanel;
        private Panel bottomPanel;
        private ImageButton button1;
        private ImageButton button2;
        private ImageButton button3;
        private IContainer components = null;
        private Panel leftPanel;
        private PictureBox pictureBox1;
        private XTextBox TextBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMessageBox));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.bottomInnerPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.ImageButton();
            this.button1 = new System.Windows.Forms.ImageButton();
            this.button3 = new System.Windows.Forms.ImageButton();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TextBox1 = new WG3000_COMM.Core.XTextBox();
            this.bottomPanel.SuspendLayout();
            this.bottomInnerPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.bottomInnerPanel);
            resources.ApplyResources(this.bottomPanel, "bottomPanel");
            this.bottomPanel.Name = "bottomPanel";
            // 
            // bottomInnerPanel
            // 
            this.bottomInnerPanel.Controls.Add(this.button2);
            this.bottomInnerPanel.Controls.Add(this.button1);
            this.bottomInnerPanel.Controls.Add(this.button3);
            resources.ApplyResources(this.bottomInnerPanel, "bottomInnerPanel");
            this.bottomInnerPanel.Name = "bottomInnerPanel";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.TabStop = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.TabStop = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.None;
            this.button3.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.TabStop = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.pictureBox1);
            resources.ApplyResources(this.leftPanel, "leftPanel");
            this.leftPanel.Name = "leftPanel";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // TextBox1
            // 
            this.TextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.TextBox1, "TextBox1");
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ReadOnly = true;
            this.TextBox1.TabStop = false;
            // 
            // XMessageBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ControlBox = false;
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XMessageBox";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.XMessageBox_Load);
            this.BackColorChanged += new System.EventHandler(this.XMessageBox_BackColorChanged);
            this.ForeColorChanged += new System.EventHandler(this.XMessageBox_ForeColorChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.XMessageBox_KeyDown);
            this.Resize += new System.EventHandler(this.XMessageBox_Resize);
            this.bottomPanel.ResumeLayout(false);
            this.bottomInnerPanel.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

