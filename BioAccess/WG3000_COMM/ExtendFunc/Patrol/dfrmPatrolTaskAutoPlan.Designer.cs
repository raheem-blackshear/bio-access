namespace WG3000_COMM.ExtendFunc.Patrol
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPatrolTaskAutoPlan
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
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmPatrolTaskAutoPlan));
            this.cbof_Group = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.cbof_ConsumerName = new System.Windows.Forms.ComboBox();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartWeekday = new System.Windows.Forms.Label();
            this.lblEndWeekday = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnAddOne = new System.Windows.Forms.ImageButton();
            this.btnDeleteOne = new System.Windows.Forms.ImageButton();
            this.btnDeleteAll = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.lstOptionalShifts = new System.Windows.Forms.ListBox();
            this.lstSelectedShifts = new System.Windows.Forms.ListBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.lstShiftWeekday = new System.Windows.Forms.ListBox();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.GroupBox1.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbof_Group
            // 
            this.cbof_Group.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_Group, "cbof_Group");
            this.cbof_Group.Name = "cbof_Group";
            this.cbof_Group.SelectedIndexChanged += new System.EventHandler(this.cbof_Group_SelectedIndexChanged);
            this.cbof_Group.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
            // 
            // Label3
            // 
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.Name = "Label3";
            // 
            // Label4
            // 
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.Name = "Label4";
            // 
            // cbof_ConsumerName
            // 
            this.cbof_ConsumerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbof_ConsumerName, "cbof_ConsumerName");
            this.cbof_ConsumerName.Name = "cbof_ConsumerName";
            this.cbof_ConsumerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
            this.cbof_ConsumerName.Leave += new System.EventHandler(this.cbof_ConsumerName_Leave);
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Value = new System.DateTime(2004, 7, 19, 0, 0, 0, 0);
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // Label6
            // 
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.Name = "Label6";
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Value = new System.DateTime(2004, 7, 19, 0, 0, 0, 0);
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // lblStartWeekday
            // 
            resources.ApplyResources(this.lblStartWeekday, "lblStartWeekday");
            this.lblStartWeekday.Name = "lblStartWeekday";
            // 
            // lblEndWeekday
            // 
            resources.ApplyResources(this.lblEndWeekday, "lblEndWeekday");
            this.lblEndWeekday.Name = "lblEndWeekday";
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.cbof_Group);
            this.GroupBox1.Controls.Add(this.dtpEndDate);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.dtpStartDate);
            this.GroupBox1.Controls.Add(this.cbof_ConsumerName);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.lblEndWeekday);
            this.GroupBox1.Controls.Add(this.lblStartWeekday);
            this.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Name = "Label2";
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
            // btnAddOne
            // 
            this.btnAddOne.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOne, "btnAddOne");
            this.btnAddOne.Focusable = true;
            this.btnAddOne.ForeColor = System.Drawing.Color.White;
            this.btnAddOne.Name = "btnAddOne";
            this.btnAddOne.Toggle = false;
            this.btnAddOne.UseVisualStyleBackColor = false;
            this.btnAddOne.Click += new System.EventHandler(this.btnAddOne_Click);
            // 
            // btnDeleteOne
            // 
            this.btnDeleteOne.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteOne, "btnDeleteOne");
            this.btnDeleteOne.Focusable = true;
            this.btnDeleteOne.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOne.Name = "btnDeleteOne";
            this.btnDeleteOne.Toggle = false;
            this.btnDeleteOne.UseVisualStyleBackColor = false;
            this.btnDeleteOne.Click += new System.EventHandler(this.btnDeleteOne_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteAll, "btnDeleteAll");
            this.btnDeleteAll.Focusable = true;
            this.btnDeleteAll.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Toggle = false;
            this.btnDeleteAll.UseVisualStyleBackColor = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
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
            // lstOptionalShifts
            // 
            resources.ApplyResources(this.lstOptionalShifts, "lstOptionalShifts");
            this.lstOptionalShifts.Name = "lstOptionalShifts";
            this.lstOptionalShifts.DoubleClick += new System.EventHandler(this.lstOptionalShifts_DoubleClick);
            // 
            // lstSelectedShifts
            // 
            resources.ApplyResources(this.lstSelectedShifts, "lstSelectedShifts");
            this.lstSelectedShifts.Name = "lstSelectedShifts";
            this.lstSelectedShifts.DoubleClick += new System.EventHandler(this.lstSelectedShifts_DoubleClick);
            // 
            // Label7
            // 
            this.Label7.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.Name = "Label7";
            // 
            // lstShiftWeekday
            // 
            resources.ApplyResources(this.lstShiftWeekday, "lstShiftWeekday");
            this.lstShiftWeekday.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lstShiftWeekday.Name = "lstShiftWeekday";
            this.lstShiftWeekday.TabStop = false;
            // 
            // ProgressBar1
            // 
            resources.ApplyResources(this.ProgressBar1, "ProgressBar1");
            this.ProgressBar1.Name = "ProgressBar1";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnOK);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmPatrolTaskAutoPlan
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.lstSelectedShifts);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.lstOptionalShifts);
            this.Controls.Add(this.btnAddOne);
            this.Controls.Add(this.btnDeleteOne);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.lstShiftWeekday);
            this.MinimizeBox = false;
            this.Name = "dfrmPatrolTaskAutoPlan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmAutoShift_FormClosing);
            this.Load += new System.EventHandler(this.dfrmAutoShift_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
            this.GroupBox1.ResumeLayout(false);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal ImageButton btnAddOne;
        internal ImageButton btnCancel;
        internal ImageButton btnDeleteAll;
        internal ImageButton btnDeleteOne;
        internal ImageButton btnOK;
        internal ComboBox cbof_ConsumerName;
        internal ComboBox cbof_Group;
        internal DateTimePicker dtpEndDate;
        internal DateTimePicker dtpStartDate;
        private DataView dvConsumers;
        internal GroupBox GroupBox1;
        internal Label Label1;
        internal Label Label2;
        internal Label Label3;
        internal Label Label4;
        internal Label Label5;
        internal Label Label6;
        internal Label Label7;
        internal Label label8;
        internal Label lblEndWeekday;
        internal Label lblStartWeekday;
        internal ListBox lstOptionalShifts;
        internal ListBox lstSelectedShifts;
        internal ListBox lstShiftWeekday;
        internal ProgressBar ProgressBar1;
        private Panel panelBottomBanner;
    }
}

