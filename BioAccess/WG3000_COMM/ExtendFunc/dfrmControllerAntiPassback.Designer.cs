namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;

    public partial class dfrmControllerAntiPassback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerAntiPassback));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.f_ControllerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_AntiBack = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.f_DoorNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEdit = new System.Windows.Forms.ImageButton();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.chkGrouped = new System.Windows.Forms.CheckBox();
            this.chkActiveAntibackShare = new System.Windows.Forms.CheckBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
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
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ControllerID,
            this.f_ControllerSN,
            this.f_AntiBack,
            this.f_DoorNames});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_DoubleClick);
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
            // f_AntiBack
            // 
            resources.ApplyResources(this.f_AntiBack, "f_AntiBack");
            this.f_AntiBack.Name = "f_AntiBack";
            this.f_AntiBack.ReadOnly = true;
            this.f_AntiBack.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_AntiBack.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // f_DoorNames
            // 
            this.f_DoorNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_DoorNames, "f_DoorNames");
            this.f_DoorNames.Name = "f_DoorNames";
            this.f_DoorNames.ReadOnly = true;
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnEdit.Focusable = true;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Toggle = false;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            // chkActiveAntibackShare
            // 
            resources.ApplyResources(this.chkActiveAntibackShare, "chkActiveAntibackShare");
            this.chkActiveAntibackShare.BackColor = System.Drawing.Color.Transparent;
            this.chkActiveAntibackShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkActiveAntibackShare.Name = "chkActiveAntibackShare";
            this.chkActiveAntibackShare.UseVisualStyleBackColor = false;
            this.chkActiveAntibackShare.CheckedChanged += new System.EventHandler(this.chkActiveAntibackShare_CheckedChanged);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnClose);
            this.panelBottomBanner.Controls.Add(this.btnEdit);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmControllerAntiPassback
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.chkGrouped);
            this.Controls.Add(this.chkActiveAntibackShare);
            this.Controls.Add(this.dataGridView1);
            this.MinimizeBox = false;
            this.Name = "dfrmControllerAntiPassback";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerAntiPassback_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerAntiPassback_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControllerAntiPassback_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton btnClose;
        private ImageButton btnEdit;
        internal CheckBox chkActiveAntibackShare;
        internal CheckBox chkGrouped;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn f_AntiBack;
        private DataGridViewTextBoxColumn f_ControllerID;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_DoorNames;
        private Panel panelBottomBanner;
    }
}

