namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmPeripheralControlBoardSuper
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
        /// 
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmPeripheralControlBoardSuper));
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.chkForceOutputTimeRemains = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox83 = new System.Windows.Forms.CheckBox();
            this.checkBox82 = new System.Windows.Forms.CheckBox();
            this.checkBox81 = new System.Windows.Forms.CheckBox();
            this.checkBox80 = new System.Windows.Forms.CheckBox();
            this.checkBox79 = new System.Windows.Forms.CheckBox();
            this.checkBox78 = new System.Windows.Forms.CheckBox();
            this.checkBox77 = new System.Windows.Forms.CheckBox();
            this.checkBox76 = new System.Windows.Forms.CheckBox();
            this.radioButton18 = new System.Windows.Forms.RadioButton();
            this.radioButton17 = new System.Windows.Forms.RadioButton();
            this.radioButton16 = new System.Windows.Forms.RadioButton();
            this.radioButton15 = new System.Windows.Forms.RadioButton();
            this.radioButton14 = new System.Windows.Forms.RadioButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkForceOutputTimeRemains
            // 
            resources.ApplyResources(this.chkForceOutputTimeRemains, "chkForceOutputTimeRemains");
            this.chkForceOutputTimeRemains.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkForceOutputTimeRemains.Name = "chkForceOutputTimeRemains";
            this.chkForceOutputTimeRemains.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // checkBox83
            // 
            resources.ApplyResources(this.checkBox83, "checkBox83");
            this.checkBox83.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox83.Name = "checkBox83";
            this.checkBox83.UseVisualStyleBackColor = true;
            // 
            // checkBox82
            // 
            resources.ApplyResources(this.checkBox82, "checkBox82");
            this.checkBox82.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox82.Name = "checkBox82";
            this.checkBox82.UseVisualStyleBackColor = true;
            // 
            // checkBox81
            // 
            resources.ApplyResources(this.checkBox81, "checkBox81");
            this.checkBox81.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox81.Name = "checkBox81";
            this.checkBox81.UseVisualStyleBackColor = true;
            // 
            // checkBox80
            // 
            resources.ApplyResources(this.checkBox80, "checkBox80");
            this.checkBox80.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox80.Name = "checkBox80";
            this.checkBox80.UseVisualStyleBackColor = true;
            // 
            // checkBox79
            // 
            resources.ApplyResources(this.checkBox79, "checkBox79");
            this.checkBox79.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox79.Name = "checkBox79";
            this.checkBox79.UseVisualStyleBackColor = true;
            // 
            // checkBox78
            // 
            resources.ApplyResources(this.checkBox78, "checkBox78");
            this.checkBox78.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox78.Name = "checkBox78";
            this.checkBox78.UseVisualStyleBackColor = true;
            // 
            // checkBox77
            // 
            resources.ApplyResources(this.checkBox77, "checkBox77");
            this.checkBox77.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox77.Name = "checkBox77";
            this.checkBox77.UseVisualStyleBackColor = true;
            // 
            // checkBox76
            // 
            resources.ApplyResources(this.checkBox76, "checkBox76");
            this.checkBox76.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.checkBox76.Name = "checkBox76";
            this.checkBox76.UseVisualStyleBackColor = true;
            // 
            // radioButton18
            // 
            resources.ApplyResources(this.radioButton18, "radioButton18");
            this.radioButton18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton18.Name = "radioButton18";
            this.radioButton18.UseVisualStyleBackColor = true;
            this.radioButton18.CheckedChanged += new System.EventHandler(this.radioButton14_CheckedChanged);
            // 
            // radioButton17
            // 
            resources.ApplyResources(this.radioButton17, "radioButton17");
            this.radioButton17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton17.Name = "radioButton17";
            this.radioButton17.UseVisualStyleBackColor = true;
            this.radioButton17.CheckedChanged += new System.EventHandler(this.radioButton14_CheckedChanged);
            // 
            // radioButton16
            // 
            resources.ApplyResources(this.radioButton16, "radioButton16");
            this.radioButton16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton16.Name = "radioButton16";
            this.radioButton16.UseVisualStyleBackColor = true;
            this.radioButton16.CheckedChanged += new System.EventHandler(this.radioButton14_CheckedChanged);
            // 
            // radioButton15
            // 
            resources.ApplyResources(this.radioButton15, "radioButton15");
            this.radioButton15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton15.Name = "radioButton15";
            this.radioButton15.UseVisualStyleBackColor = true;
            this.radioButton15.CheckedChanged += new System.EventHandler(this.radioButton14_CheckedChanged);
            // 
            // radioButton14
            // 
            resources.ApplyResources(this.radioButton14, "radioButton14");
            this.radioButton14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.radioButton14.Name = "radioButton14";
            this.radioButton14.UseVisualStyleBackColor = true;
            this.radioButton14.CheckedChanged += new System.EventHandler(this.radioButton14_CheckedChanged);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmPeripheralControlBoardSuper
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkForceOutputTimeRemains);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox83);
            this.Controls.Add(this.checkBox82);
            this.Controls.Add(this.checkBox81);
            this.Controls.Add(this.checkBox80);
            this.Controls.Add(this.checkBox79);
            this.Controls.Add(this.checkBox78);
            this.Controls.Add(this.checkBox77);
            this.Controls.Add(this.checkBox76);
            this.Controls.Add(this.radioButton18);
            this.Controls.Add(this.radioButton17);
            this.Controls.Add(this.radioButton16);
            this.Controls.Add(this.radioButton15);
            this.Controls.Add(this.radioButton14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dfrmPeripheralControlBoardSuper";
            this.Load += new System.EventHandler(this.dfrmPeripheralControlBoardSuper_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmPeripheralControlBoardSuper_KeyDown);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        private CheckBox checkBox76;
        private CheckBox checkBox77;
        private CheckBox checkBox78;
        private CheckBox checkBox79;
        private CheckBox checkBox80;
        private CheckBox checkBox81;
        private CheckBox checkBox82;
        private CheckBox checkBox83;
        private CheckBox chkForceOutputTimeRemains;
        private Label label1;
        private RadioButton radioButton14;
        private RadioButton radioButton15;
        private RadioButton radioButton16;
        private RadioButton radioButton17;
        private RadioButton radioButton18;
        private Panel panelBottomBanner;
    }
}

