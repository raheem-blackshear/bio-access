namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmDoorAsSwitch
    {
        private ImageButton btnAddAllDoors;
        private ImageButton btnAddOneDoor;
        private ImageButton btnDelAllDoors;
        private ImageButton btnDelOneDoor;
        private ImageButton btnExit;
        private ImageButton btnOKAndUpload;
        private ComboBox cbof_ControlSegID;
        private ComboBox cbof_ZoneID;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private IContainer components = null;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private dfrmWait dfrmWait1 = new dfrmWait();
        private DataGridView dgvDoors;
        private DataGridView dgvSelectedDoors;
        private DataTable dt;
        private DataTable dtDoorTmpSelected;
        private DataView dv;
        private DataView dvDoorTmpSelected;
        private DataView dvSelected;
        private DataView dvtmp;
        private DataGridViewTextBoxColumn f_ControlSegName;
        private DataGridViewTextBoxColumn f_ControlSegName1;
        private DataGridViewTextBoxColumn f_Selected;
        private DataGridViewTextBoxColumn f_Selected2;
        private DataGridViewTextBoxColumn f_ZoneID;
        private Label label1;
        private Label label25;
        private Label lblOptional;
        private Label lblSeleted;
        private DataGridViewTextBoxColumn TimeProfile;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.dfrmWait1 != null))
            {
                this.dfrmWait1.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmDoorAsSwitch));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.btnOKAndUpload = new System.Windows.Forms.ImageButton();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.lblSeleted = new System.Windows.Forms.Label();
            this.lblOptional = new System.Windows.Forms.Label();
            this.cbof_ControlSegID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbof_ZoneID = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.dgvSelectedDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeProfile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDoors = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Selected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ZoneID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControlSegName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddAllDoors = new System.Windows.Forms.ImageButton();
            this.btnDelAllDoors = new System.Windows.Forms.ImageButton();
            this.btnAddOneDoor = new System.Windows.Forms.ImageButton();
            this.btnDelOneDoor = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnOKAndUpload);
            this.panelBottomBanner.Controls.Add(this.btnExit);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // btnOKAndUpload
            // 
            resources.ApplyResources(this.btnOKAndUpload, "btnOKAndUpload");
            this.btnOKAndUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOKAndUpload.Focusable = true;
            this.btnOKAndUpload.ForeColor = System.Drawing.Color.White;
            this.btnOKAndUpload.Name = "btnOKAndUpload";
            this.btnOKAndUpload.Toggle = false;
            this.btnOKAndUpload.UseVisualStyleBackColor = false;
            this.btnOKAndUpload.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.Toggle = false;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblSeleted
            // 
            resources.ApplyResources(this.lblSeleted, "lblSeleted");
            this.lblSeleted.BackColor = System.Drawing.Color.Transparent;
            this.lblSeleted.ForeColor = System.Drawing.Color.White;
            this.lblSeleted.Name = "lblSeleted";
            // 
            // lblOptional
            // 
            resources.ApplyResources(this.lblOptional, "lblOptional");
            this.lblOptional.BackColor = System.Drawing.Color.Transparent;
            this.lblOptional.ForeColor = System.Drawing.Color.White;
            this.lblOptional.Name = "lblOptional";
            // 
            // cbof_ControlSegID
            // 
            this.cbof_ControlSegID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ControlSegID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
            this.cbof_ControlSegID.Name = "cbof_ControlSegID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // cbof_ZoneID
            // 
            this.cbof_ZoneID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbof_ZoneID.FormattingEnabled = true;
            resources.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
            this.cbof_ZoneID.Name = "cbof_ZoneID";
            this.cbof_ZoneID.SelectedIndexChanged += new System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label25.Name = "label25";
            // 
            // dgvSelectedDoors
            // 
            this.dgvSelectedDoors.AllowUserToAddRows = false;
            this.dgvSelectedDoors.AllowUserToDeleteRows = false;
            this.dgvSelectedDoors.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
            this.dgvSelectedDoors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectedDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.f_Selected2,
            this.Column1,
            this.TimeProfile,
            this.f_ControlSegName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
            this.dgvSelectedDoors.Name = "dgvSelectedDoors";
            this.dgvSelectedDoors.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSelectedDoors.RowTemplate.Height = 23;
            this.dgvSelectedDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedDoors.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSelectedDoors_CellFormatting);
            this.dgvSelectedDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn8
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // f_Selected2
            // 
            resources.ApplyResources(this.f_Selected2, "f_Selected2");
            this.f_Selected2.Name = "f_Selected2";
            this.f_Selected2.ReadOnly = true;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // TimeProfile
            // 
            resources.ApplyResources(this.TimeProfile, "TimeProfile");
            this.TimeProfile.Name = "TimeProfile";
            this.TimeProfile.ReadOnly = true;
            // 
            // f_ControlSegName
            // 
            this.f_ControlSegName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_ControlSegName, "f_ControlSegName");
            this.f_ControlSegName.Name = "f_ControlSegName";
            this.f_ControlSegName.ReadOnly = true;
            // 
            // dgvDoors
            // 
            this.dgvDoors.AllowUserToAddRows = false;
            this.dgvDoors.AllowUserToDeleteRows = false;
            this.dgvDoors.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgvDoors, "dgvDoors");
            this.dgvDoors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDoors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDoors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.f_Selected,
            this.f_ZoneID,
            this.Column2,
            this.f_ControlSegName1});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDoors.EnableHeadersVisualStyles = false;
            this.dgvDoors.Name = "dgvDoors";
            this.dgvDoors.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDoors.RowTemplate.Height = 23;
            this.dgvDoors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // f_Selected
            // 
            resources.ApplyResources(this.f_Selected, "f_Selected");
            this.f_Selected.Name = "f_Selected";
            this.f_Selected.ReadOnly = true;
            // 
            // f_ZoneID
            // 
            resources.ApplyResources(this.f_ZoneID, "f_ZoneID");
            this.f_ZoneID.Name = "f_ZoneID";
            this.f_ZoneID.ReadOnly = true;
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // f_ControlSegName1
            // 
            resources.ApplyResources(this.f_ControlSegName1, "f_ControlSegName1");
            this.f_ControlSegName1.Name = "f_ControlSegName1";
            this.f_ControlSegName1.ReadOnly = true;
            // 
            // btnAddAllDoors
            // 
            this.btnAddAllDoors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
            this.btnAddAllDoors.Focusable = true;
            this.btnAddAllDoors.ForeColor = System.Drawing.Color.White;
            this.btnAddAllDoors.Name = "btnAddAllDoors";
            this.btnAddAllDoors.Toggle = false;
            this.btnAddAllDoors.UseVisualStyleBackColor = false;
            this.btnAddAllDoors.Click += new System.EventHandler(this.btnAddAllDoors_Click);
            // 
            // btnDelAllDoors
            // 
            this.btnDelAllDoors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
            this.btnDelAllDoors.Focusable = true;
            this.btnDelAllDoors.ForeColor = System.Drawing.Color.White;
            this.btnDelAllDoors.Name = "btnDelAllDoors";
            this.btnDelAllDoors.Toggle = false;
            this.btnDelAllDoors.UseVisualStyleBackColor = false;
            this.btnDelAllDoors.Click += new System.EventHandler(this.btnDelAllDoors_Click);
            // 
            // btnAddOneDoor
            // 
            this.btnAddOneDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
            this.btnAddOneDoor.Focusable = true;
            this.btnAddOneDoor.ForeColor = System.Drawing.Color.White;
            this.btnAddOneDoor.Name = "btnAddOneDoor";
            this.btnAddOneDoor.Toggle = false;
            this.btnAddOneDoor.UseVisualStyleBackColor = false;
            this.btnAddOneDoor.Click += new System.EventHandler(this.btnAddOneDoor_Click);
            // 
            // btnDelOneDoor
            // 
            this.btnDelOneDoor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
            this.btnDelOneDoor.Focusable = true;
            this.btnDelOneDoor.ForeColor = System.Drawing.Color.White;
            this.btnDelOneDoor.Name = "btnDelOneDoor";
            this.btnDelOneDoor.Toggle = false;
            this.btnDelOneDoor.UseVisualStyleBackColor = false;
            this.btnDelOneDoor.Click += new System.EventHandler(this.btnDelOneDoor_Click);
            // 
            // dfrmDoorAsSwitch
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.lblSeleted);
            this.Controls.Add(this.lblOptional);
            this.Controls.Add(this.cbof_ControlSegID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbof_ZoneID);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.dgvSelectedDoors);
            this.Controls.Add(this.dgvDoors);
            this.Controls.Add(this.btnAddAllDoors);
            this.Controls.Add(this.btnDelAllDoors);
            this.Controls.Add(this.btnAddOneDoor);
            this.Controls.Add(this.btnDelOneDoor);
            this.MinimizeBox = false;
            this.Name = "dfrmDoorAsSwitch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilegeSingle_FormClosing);
            this.Load += new System.EventHandler(this.dfrmDoorAsSwitch_Load);
            this.panelBottomBanner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedDoors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelBottomBanner;
    }
}

