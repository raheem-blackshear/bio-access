namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmDeleteRecords
    {
        private ImageButton btnBackupDatabase;
        private ImageButton btnDeleteAllSwipeRecords;
        private ImageButton btnDeleteLog;
        private ImageButton btnExit;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmDeleteRecords));
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnDeleteAllSwipeRecords = new System.Windows.Forms.ImageButton();
            this.btnDeleteLog = new System.Windows.Forms.ImageButton();
            this.btnBackupDatabase = new System.Windows.Forms.ImageButton();
            this.btnDeleteOldSwipeRecords = new System.Windows.Forms.ImageButton();
            this.lblIndex = new System.Windows.Forms.Label();
            this.nudSwipeRecordIndex = new System.Windows.Forms.NumericUpDown();
            this.lblIndexMin = new System.Windows.Forms.Label();
            this.nudIndexMin = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwipeRecordIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIndexMin)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Name = "label1";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.Toggle = false;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDeleteAllSwipeRecords
            // 
            this.btnDeleteAllSwipeRecords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteAllSwipeRecords, "btnDeleteAllSwipeRecords");
            this.btnDeleteAllSwipeRecords.Focusable = true;
            this.btnDeleteAllSwipeRecords.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAllSwipeRecords.Name = "btnDeleteAllSwipeRecords";
            this.btnDeleteAllSwipeRecords.Toggle = false;
            this.btnDeleteAllSwipeRecords.UseVisualStyleBackColor = false;
            this.btnDeleteAllSwipeRecords.Click += new System.EventHandler(this.btnDeleteAllSwipeRecords_Click);
            // 
            // btnDeleteLog
            // 
            this.btnDeleteLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteLog, "btnDeleteLog");
            this.btnDeleteLog.Focusable = true;
            this.btnDeleteLog.ForeColor = System.Drawing.Color.White;
            this.btnDeleteLog.Name = "btnDeleteLog";
            this.btnDeleteLog.Toggle = false;
            this.btnDeleteLog.UseVisualStyleBackColor = false;
            this.btnDeleteLog.Click += new System.EventHandler(this.btnDeleteLog_Click);
            // 
            // btnBackupDatabase
            // 
            this.btnBackupDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnBackupDatabase, "btnBackupDatabase");
            this.btnBackupDatabase.Focusable = true;
            this.btnBackupDatabase.ForeColor = System.Drawing.Color.White;
            this.btnBackupDatabase.Name = "btnBackupDatabase";
            this.btnBackupDatabase.Toggle = false;
            this.btnBackupDatabase.UseVisualStyleBackColor = false;
            this.btnBackupDatabase.Click += new System.EventHandler(this.btnBackupDatabase_Click);
            // 
            // btnDeleteOldSwipeRecords
            // 
            this.btnDeleteOldSwipeRecords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteOldSwipeRecords, "btnDeleteOldSwipeRecords");
            this.btnDeleteOldSwipeRecords.Focusable = true;
            this.btnDeleteOldSwipeRecords.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOldSwipeRecords.Name = "btnDeleteOldSwipeRecords";
            this.btnDeleteOldSwipeRecords.Toggle = false;
            this.btnDeleteOldSwipeRecords.UseVisualStyleBackColor = false;
            this.btnDeleteOldSwipeRecords.Click += new System.EventHandler(this.btnDeleteOldSwipeRecords_Click);
            // 
            // lblIndex
            // 
            resources.ApplyResources(this.lblIndex, "lblIndex");
            this.lblIndex.Name = "lblIndex";
            // 
            // nudSwipeRecordIndex
            // 
            this.nudSwipeRecordIndex.BackColor = System.Drawing.Color.White;
            this.nudSwipeRecordIndex.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            resources.ApplyResources(this.nudSwipeRecordIndex, "nudSwipeRecordIndex");
            this.nudSwipeRecordIndex.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.nudSwipeRecordIndex.Name = "nudSwipeRecordIndex";
            // 
            // lblIndexMin
            // 
            resources.ApplyResources(this.lblIndexMin, "lblIndexMin");
            this.lblIndexMin.Name = "lblIndexMin";
            // 
            // nudIndexMin
            // 
            this.nudIndexMin.BackColor = System.Drawing.Color.White;
            this.nudIndexMin.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            resources.ApplyResources(this.nudIndexMin, "nudIndexMin");
            this.nudIndexMin.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.nudIndexMin.Name = "nudIndexMin";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.nudIndexMin);
            this.groupBox2.Controls.Add(this.lblIndexMin);
            this.groupBox2.Controls.Add(this.nudSwipeRecordIndex);
            this.groupBox2.Controls.Add(this.lblIndex);
            this.groupBox2.Controls.Add(this.btnDeleteOldSwipeRecords);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // dfrmDeleteRecords
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDeleteLog);
            this.Controls.Add(this.btnBackupDatabase);
            this.Controls.Add(this.btnDeleteAllSwipeRecords);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmDeleteRecords";
            this.Load += new System.EventHandler(this.dfrmDeleteRecords_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmDeleteRecords_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudSwipeRecordIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIndexMin)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ImageButton btnDeleteOldSwipeRecords;
        private Label lblIndex;
        internal NumericUpDown nudSwipeRecordIndex;
        private Label lblIndexMin;
        internal NumericUpDown nudIndexMin;
        private GroupBox groupBox2;
    }
}

