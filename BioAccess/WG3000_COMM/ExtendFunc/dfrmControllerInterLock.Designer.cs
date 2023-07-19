namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmControllerInterLock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerInterLock));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkGrouped = new System.Windows.Forms.CheckBox();
            this.chkActiveInterlockShare = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.f_ControllerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_InterLock12 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_InterLock34 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_InterLock123 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_InterLock1234 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_DoorNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkGrouped
            // 
            resources.ApplyResources(this.chkGrouped, "chkGrouped");
            this.chkGrouped.BackColor = System.Drawing.Color.Transparent;
            this.chkGrouped.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkGrouped.Name = "chkGrouped";
            this.chkGrouped.UseVisualStyleBackColor = false;
            this.chkGrouped.CheckedChanged += new System.EventHandler(this.chkGrouped_CheckedChanged);
            // 
            // chkActiveInterlockShare
            // 
            resources.ApplyResources(this.chkActiveInterlockShare, "chkActiveInterlockShare");
            this.chkActiveInterlockShare.BackColor = System.Drawing.Color.Transparent;
            this.chkActiveInterlockShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkActiveInterlockShare.Name = "chkActiveInterlockShare";
            this.chkActiveInterlockShare.UseVisualStyleBackColor = false;
            this.chkActiveInterlockShare.CheckedChanged += new System.EventHandler(this.chkActiveInterlockShare_CheckedChanged);
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
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ControllerID,
            this.f_ControllerSN,
            this.f_InterLock12,
            this.f_InterLock34,
            this.f_InterLock123,
            this.f_InterLock1234,
            this.f_DoorNames});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // f_ControllerID
            // 
            resources.ApplyResources(this.f_ControllerID, "f_ControllerID");
            this.f_ControllerID.Name = "f_ControllerID";
            this.f_ControllerID.ReadOnly = true;
            // 
            // f_ControllerSN
            // 
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            // 
            // f_InterLock12
            // 
            resources.ApplyResources(this.f_InterLock12, "f_InterLock12");
            this.f_InterLock12.Name = "f_InterLock12";
            this.f_InterLock12.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_InterLock12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_InterLock34
            // 
            resources.ApplyResources(this.f_InterLock34, "f_InterLock34");
            this.f_InterLock34.Name = "f_InterLock34";
            // 
            // f_InterLock123
            // 
            resources.ApplyResources(this.f_InterLock123, "f_InterLock123");
            this.f_InterLock123.Name = "f_InterLock123";
            // 
            // f_InterLock1234
            // 
            resources.ApplyResources(this.f_InterLock1234, "f_InterLock1234");
            this.f_InterLock1234.Name = "f_InterLock1234";
            // 
            // f_DoorNames
            // 
            this.f_DoorNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_DoorNames, "f_DoorNames");
            this.f_DoorNames.Name = "f_DoorNames";
            this.f_DoorNames.ReadOnly = true;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmControllerInterLock
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkGrouped);
            this.Controls.Add(this.chkActiveInterlockShare);
            this.Controls.Add(this.dataGridView1);
            this.MinimizeBox = false;
            this.Name = "dfrmControllerInterLock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerInterLock_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerInterLock_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControllerInterLock_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ImageButton btnCancel;
        private ImageButton btnOK;
        internal CheckBox chkActiveInterlockShare;
        internal CheckBox chkGrouped;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn f_ControllerID;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_DoorNames;
        private DataGridViewCheckBoxColumn f_InterLock12;
        private DataGridViewCheckBoxColumn f_InterLock123;
        private DataGridViewCheckBoxColumn f_InterLock1234;
        private DataGridViewCheckBoxColumn f_InterLock34;
        private Panel panelBottomBanner;
    }
}

