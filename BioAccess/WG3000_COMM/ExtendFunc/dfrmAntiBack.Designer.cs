namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmAntiBack
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmAntiBack));
            this.nudTotal = new System.Windows.Forms.NumericUpDown();
            this.chkActiveAntibackShare = new System.Windows.Forms.CheckBox();
            this.radioButton0 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.checkBox22 = new System.Windows.Forms.CheckBox();
            this.checkBox21 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudTotal
            // 
            this.nudTotal.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudTotal, "nudTotal");
            this.nudTotal.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTotal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTotal.Name = "nudTotal";
            this.nudTotal.ReadOnly = true;
            this.nudTotal.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // chkActiveAntibackShare
            // 
            resources.ApplyResources(this.chkActiveAntibackShare, "chkActiveAntibackShare");
            this.chkActiveAntibackShare.BackColor = System.Drawing.Color.Transparent;
            this.chkActiveAntibackShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkActiveAntibackShare.Name = "chkActiveAntibackShare";
            this.chkActiveAntibackShare.UseVisualStyleBackColor = false;
            this.chkActiveAntibackShare.CheckedChanged += new System.EventHandler(this.chkActiveAntibackShare_CheckedChanged);
            // 
            // radioButton0
            // 
            resources.ApplyResources(this.radioButton0, "radioButton0");
            this.radioButton0.BackColor = System.Drawing.Color.Transparent;
            this.radioButton0.Checked = true;
            this.radioButton0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton0.Name = "radioButton0";
            this.radioButton0.TabStop = true;
            this.radioButton0.UseVisualStyleBackColor = false;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.BackColor = System.Drawing.Color.Transparent;
            this.radioButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = false;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.BackColor = System.Drawing.Color.Transparent;
            this.radioButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = false;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
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
            // checkBox22
            // 
            resources.ApplyResources(this.checkBox22, "checkBox22");
            this.checkBox22.BackColor = System.Drawing.Color.Transparent;
            this.checkBox22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox22.Name = "checkBox22";
            this.checkBox22.UseVisualStyleBackColor = false;
            // 
            // checkBox21
            // 
            resources.ApplyResources(this.checkBox21, "checkBox21");
            this.checkBox21.BackColor = System.Drawing.Color.Transparent;
            this.checkBox21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox21.Name = "checkBox21";
            this.checkBox21.UseVisualStyleBackColor = false;
            // 
            // checkBox11
            // 
            resources.ApplyResources(this.checkBox11, "checkBox11");
            this.checkBox11.BackColor = System.Drawing.Color.Transparent;
            this.checkBox11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.UseVisualStyleBackColor = false;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmAntiBack
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.nudTotal);
            this.Controls.Add(this.chkActiveAntibackShare);
            this.Controls.Add(this.radioButton0);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.checkBox22);
            this.Controls.Add(this.checkBox21);
            this.Controls.Add(this.checkBox11);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmAntiBack";
            this.Load += new System.EventHandler(this.dfrmAntiBack_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmAntiBack_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ImageButton btnCancel;
        private ImageButton btnOK;
        private CheckBox checkBox11;
        private CheckBox checkBox21;
        private CheckBox checkBox22;
        internal CheckBox chkActiveAntibackShare;
        internal NumericUpDown nudTotal;
        private RadioButton radioButton0;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private Panel panelBottomBanner;
    }
}

