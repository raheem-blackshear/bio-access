namespace WG3000_COMM.ExtendFunc.Elevator
{
    using System;
    using System.Collections;
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
    using WG3000_COMM.ResStrings;

    partial class dfrmFloors
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmFloors));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRemoteControl = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoteControlNC = new System.Windows.Forms.ToolStripMenuItem();
            this.btnChange = new System.Windows.Forms.ImageButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnDel = new System.Windows.Forms.ImageButton();
            this.dgvFloorList = new System.Windows.Forms.DataGridView();
            this.f_floorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_floorFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DoorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_floorNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ZoneID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_floorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new System.Windows.Forms.ImageButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cboFloorNO = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboElevator = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFloorList)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemoteControl,
            this.btnRemoteControlNC});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 48);
            // 
            // btnRemoteControl
            // 
            this.btnRemoteControl.Name = "btnRemoteControl";
            this.btnRemoteControl.Size = new System.Drawing.Size(198, 22);
            this.btnRemoteControl.Text = "Remote Control";
            this.btnRemoteControl.Click += new System.EventHandler(this.btnRemoteControl_Click);
            // 
            // btnRemoteControlNC
            // 
            this.btnRemoteControlNC.Name = "btnRemoteControlNC";
            this.btnRemoteControlNC.Size = new System.Drawing.Size(198, 22);
            this.btnRemoteControlNC.Text = "Remote Control (NC)";
            this.btnRemoteControlNC.Visible = false;
            this.btnRemoteControlNC.Click += new System.EventHandler(this.btnRemoteControlNC_Click);
            // 
            // btnChange
            // 
            this.btnChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChange.Focusable = true;
            this.btnChange.ForeColor = System.Drawing.Color.White;
            this.btnChange.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChange.Location = new System.Drawing.Point(54, 125);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(99, 27);
            this.btnChange.TabIndex = 45;
            this.btnChange.Text = "Change Name";
            this.btnChange.Toggle = false;
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(166, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(175, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(65, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 44;
            this.label3.Text = "*Door Name:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(393, 125);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(99, 27);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDel.Focusable = true;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            this.btnDel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDel.Location = new System.Drawing.Point(280, 125);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(99, 27);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "Delete";
            this.btnDel.Toggle = false;
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // dgvFloorList
            // 
            this.dgvFloorList.AllowUserToAddRows = false;
            this.dgvFloorList.AllowUserToDeleteRows = false;
            this.dgvFloorList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFloorList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(125)))), ((int)(((byte)(156)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFloorList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFloorList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFloorList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_floorID,
            this.f_floorFullName,
            this.f_DoorName,
            this.f_floorNO,
            this.f_ZoneID,
            this.f_floorName});
            this.dgvFloorList.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvFloorList.EnableHeadersVisualStyles = false;
            this.dgvFloorList.Location = new System.Drawing.Point(4, 172);
            this.dgvFloorList.Name = "dgvFloorList";
            this.dgvFloorList.ReadOnly = true;
            this.dgvFloorList.RowHeadersWidth = 20;
            this.dgvFloorList.RowTemplate.Height = 23;
            this.dgvFloorList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFloorList.Size = new System.Drawing.Size(530, 382);
            this.dgvFloorList.TabIndex = 6;
            this.dgvFloorList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmFloors_KeyDown);
            // 
            // f_floorID
            // 
            this.f_floorID.HeaderText = "FloorID";
            this.f_floorID.Name = "f_floorID";
            this.f_floorID.ReadOnly = true;
            this.f_floorID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.f_floorID.Visible = false;
            this.f_floorID.Width = 60;
            // 
            // f_floorFullName
            // 
            this.f_floorFullName.HeaderText = "Door Fullname";
            this.f_floorFullName.Name = "f_floorFullName";
            this.f_floorFullName.ReadOnly = true;
            this.f_floorFullName.Width = 150;
            // 
            // f_DoorName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_DoorName.DefaultCellStyle = dataGridViewCellStyle2;
            this.f_DoorName.HeaderText = "Controller";
            this.f_DoorName.Name = "f_DoorName";
            this.f_DoorName.ReadOnly = true;
            this.f_DoorName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_DoorName.Width = 200;
            // 
            // f_floorNO
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_floorNO.DefaultCellStyle = dataGridViewCellStyle3;
            this.f_floorNO.HeaderText = "Door-Relay No.";
            this.f_floorNO.Name = "f_floorNO";
            this.f_floorNO.ReadOnly = true;
            this.f_floorNO.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_floorNO.Width = 120;
            // 
            // f_ZoneID
            // 
            this.f_ZoneID.HeaderText = "ZoneID";
            this.f_ZoneID.Name = "f_ZoneID";
            this.f_ZoneID.ReadOnly = true;
            this.f_ZoneID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.f_ZoneID.Visible = false;
            // 
            // f_floorName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_floorName.DefaultCellStyle = dataGridViewCellStyle4;
            this.f_floorName.HeaderText = "Floor Name";
            this.f_floorName.Name = "f_floorName";
            this.f_floorName.ReadOnly = true;
            this.f_floorName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_floorName.Visible = false;
            this.f_floorName.Width = 200;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Focusable = true;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdd.Location = new System.Drawing.Point(167, 125);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(99, 27);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.btnAdd.Toggle = false;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(65, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "Controller";
            // 
            // cboFloorNO
            // 
            this.cboFloorNO.DropDownHeight = 300;
            this.cboFloorNO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFloorNO.FormattingEnabled = true;
            this.cboFloorNO.IntegralHeight = false;
            this.cboFloorNO.Location = new System.Drawing.Point(166, 87);
            this.cboFloorNO.Name = "cboFloorNO";
            this.cboFloorNO.Size = new System.Drawing.Size(277, 20);
            this.cboFloorNO.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(65, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "Door-Relay No.:";
            // 
            // cboElevator
            // 
            this.cboElevator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboElevator.FormattingEnabled = true;
            this.cboElevator.Location = new System.Drawing.Point(166, 56);
            this.cboElevator.Name = "cboElevator";
            this.cboElevator.Size = new System.Drawing.Size(277, 20);
            this.cboElevator.TabIndex = 1;
            this.cboElevator.SelectedIndexChanged += new System.EventHandler(this.cboElevator_SelectedIndexChanged);
            // 
            // dfrmFloors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(546, 566);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.dgvFloorList);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboFloorNO);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboElevator);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "dfrmFloors";
            this.Text = "Configure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmFloors_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerTaskList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmFloors_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFloorList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton btnAdd;
        private ImageButton btnChange;
        private ImageButton btnClose;
        private ImageButton btnDel;
        private ToolStripMenuItem btnRemoteControl;
        private ToolStripMenuItem btnRemoteControlNC;
        private ComboBox cboElevator;
        private ComboBox cboFloorNO;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridView dgvFloorList;
        private DataGridViewTextBoxColumn f_DoorName;
        private DataGridViewTextBoxColumn f_floorFullName;
        private DataGridViewTextBoxColumn f_floorID;
        private DataGridViewTextBoxColumn f_floorName;
        private DataGridViewTextBoxColumn f_floorNO;
        private DataGridViewTextBoxColumn f_ZoneID;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
    }
}

