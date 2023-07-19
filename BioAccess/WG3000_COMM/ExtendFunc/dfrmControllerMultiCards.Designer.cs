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

    public partial class dfrmControllerMultiCards
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerMultiCards));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnEdit = new System.Windows.Forms.ImageButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.f_DoorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MoreCards_Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
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
            this.f_DoorID,
            this.f_ControllerSN,
            this.f_DoorNo,
            this.f_DoorName,
            this.f_MoreCards_Total});
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // f_DoorID
            // 
            resources.ApplyResources(this.f_DoorID, "f_DoorID");
            this.f_DoorID.Name = "f_DoorID";
            this.f_DoorID.ReadOnly = true;
            // 
            // f_ControllerSN
            // 
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            // 
            // f_DoorNo
            // 
            resources.ApplyResources(this.f_DoorNo, "f_DoorNo");
            this.f_DoorNo.Name = "f_DoorNo";
            this.f_DoorNo.ReadOnly = true;
            // 
            // f_DoorName
            // 
            resources.ApplyResources(this.f_DoorName, "f_DoorName");
            this.f_DoorName.Name = "f_DoorName";
            this.f_DoorName.ReadOnly = true;
            // 
            // f_MoreCards_Total
            // 
            resources.ApplyResources(this.f_MoreCards_Total, "f_MoreCards_Total");
            this.f_MoreCards_Total.Name = "f_MoreCards_Total";
            this.f_MoreCards_Total.ReadOnly = true;
            this.f_MoreCards_Total.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnClose);
            this.panelBottomBanner.Controls.Add(this.btnEdit);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmControllerMultiCards
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.dataGridView1);
            this.MinimizeBox = false;
            this.Name = "dfrmControllerMultiCards";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerMultiCards_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerMultiCards_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControllerMultiCards_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageButton btnClose;
        private ImageButton btnEdit;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_DoorID;
        private DataGridViewTextBoxColumn f_DoorName;
        private DataGridViewTextBoxColumn f_DoorNo;
        private DataGridViewTextBoxColumn f_MoreCards_Total;
        private Panel panelBottomBanner;
    }
}

