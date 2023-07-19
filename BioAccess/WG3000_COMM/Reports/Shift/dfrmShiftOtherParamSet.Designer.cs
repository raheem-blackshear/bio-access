namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftOtherParamSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmShiftOtherParamSet));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.nudAheadMinutes = new System.Windows.Forms.NumericUpDown();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.nudOvertimeTimeout = new System.Windows.Forms.NumericUpDown();
            this.nudLeaveTimeout = new System.Windows.Forms.NumericUpDown();
            this.nudLateTimeout = new System.Windows.Forms.NumericUpDown();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.Label2 = new System.Windows.Forms.Label();
            this.nudOvertimeMinutes = new System.Windows.Forms.NumericUpDown();
            this.Label14 = new System.Windows.Forms.Label();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAheadMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOvertimeTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeaveTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLateTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOvertimeMinutes)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.nudAheadMinutes);
            this.GroupBox1.Controls.Add(this.Label16);
            this.GroupBox1.Controls.Add(this.Label15);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.Label12);
            this.GroupBox1.Controls.Add(this.nudOvertimeTimeout);
            this.GroupBox1.Controls.Add(this.nudLeaveTimeout);
            this.GroupBox1.Controls.Add(this.nudLateTimeout);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label17);
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // nudAheadMinutes
            // 
            this.nudAheadMinutes.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudAheadMinutes, "nudAheadMinutes");
            this.nudAheadMinutes.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudAheadMinutes.Name = "nudAheadMinutes";
            this.nudAheadMinutes.ReadOnly = true;
            this.nudAheadMinutes.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // Label16
            // 
            resources.ApplyResources(this.Label16, "Label16");
            this.Label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label16.Name = "Label16";
            // 
            // Label15
            // 
            resources.ApplyResources(this.Label15, "Label15");
            this.Label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label15.Name = "Label15";
            // 
            // Label13
            // 
            resources.ApplyResources(this.Label13, "Label13");
            this.Label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label13.Name = "Label13";
            // 
            // Label12
            // 
            resources.ApplyResources(this.Label12, "Label12");
            this.Label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label12.Name = "Label12";
            // 
            // nudOvertimeTimeout
            // 
            this.nudOvertimeTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudOvertimeTimeout, "nudOvertimeTimeout");
            this.nudOvertimeTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudOvertimeTimeout.Name = "nudOvertimeTimeout";
            this.nudOvertimeTimeout.ReadOnly = true;
            this.nudOvertimeTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // nudLeaveTimeout
            // 
            this.nudLeaveTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudLeaveTimeout, "nudLeaveTimeout");
            this.nudLeaveTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudLeaveTimeout.Name = "nudLeaveTimeout";
            this.nudLeaveTimeout.ReadOnly = true;
            this.nudLeaveTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudLateTimeout
            // 
            this.nudLateTimeout.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.nudLateTimeout, "nudLateTimeout");
            this.nudLateTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudLateTimeout.Name = "nudLateTimeout";
            this.nudLateTimeout.ReadOnly = true;
            this.nudLateTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label1.Name = "Label1";
            // 
            // Label3
            // 
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label3.Name = "Label3";
            // 
            // Label4
            // 
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label4.Name = "Label4";
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label5.Name = "Label5";
            // 
            // Label17
            // 
            resources.ApplyResources(this.Label17, "Label17");
            this.Label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label17.Name = "Label17";
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
            // Label2
            // 
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label2.Name = "Label2";
            // 
            // nudOvertimeMinutes
            // 
            resources.ApplyResources(this.nudOvertimeMinutes, "nudOvertimeMinutes");
            this.nudOvertimeMinutes.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudOvertimeMinutes.Name = "nudOvertimeMinutes";
            this.nudOvertimeMinutes.Value = new decimal(new int[] {
            360,
            0,
            0,
            0});
            // 
            // Label14
            // 
            resources.ApplyResources(this.Label14, "Label14");
            this.Label14.BackColor = System.Drawing.Color.Transparent;
            this.Label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label14.Name = "Label14";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmShiftOtherParamSet
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.nudOvertimeMinutes);
            this.Controls.Add(this.Label14);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmShiftOtherParamSet";
            this.Load += new System.EventHandler(this.dfrmShiftOtherParamSet_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmShiftOtherParamSet_KeyDown);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAheadMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOvertimeTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeaveTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLateTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOvertimeMinutes)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        internal GroupBox GroupBox1;
        internal Label Label1;
        internal Label Label12;
        internal Label Label13;
        internal Label Label14;
        internal Label Label15;
        internal Label Label16;
        internal Label Label17;
        internal Label Label2;
        internal Label Label3;
        internal Label Label4;
        internal Label Label5;
        internal NumericUpDown nudAheadMinutes;
        internal NumericUpDown nudLateTimeout;
        internal NumericUpDown nudLeaveTimeout;
        internal NumericUpDown nudOvertimeMinutes;
        internal NumericUpDown nudOvertimeTimeout;
        private Panel panelBottomBanner;
    }
}

