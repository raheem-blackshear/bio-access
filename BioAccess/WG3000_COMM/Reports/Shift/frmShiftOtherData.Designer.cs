namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmShiftOtherData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShiftOtherData));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.f_RecID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_DateYM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_OnDuty1Short = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ShiftID_31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ConsumerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlFind1 = new WG3000_COMM.Core.UserControlFind();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.toolStrip3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_RecID,
            this.f_DepartmentName,
            this.f_ConsumerNO,
            this.f_ConsumerName,
            this.f_DateYM,
            this.f_ShiftID_01,
            this.f_ShiftID_02,
            this.f_OnDuty1Short,
            this.f_ShiftID_04,
            this.f_ShiftID_05,
            this.f_ShiftID_06,
            this.f_ShiftID_07,
            this.f_ShiftID_08,
            this.f_ShiftID_09,
            this.f_ShiftID_10,
            this.f_ShiftID_11,
            this.f_ShiftID_12,
            this.f_ShiftID_13,
            this.f_ShiftID_14,
            this.f_ShiftID_15,
            this.f_ShiftID_16,
            this.f_ShiftID_17,
            this.f_ShiftID_18,
            this.f_ShiftID_19,
            this.f_ShiftID_20,
            this.f_ShiftID_21,
            this.f_ShiftID_22,
            this.f_ShiftID_23,
            this.f_ShiftID_24,
            this.f_ShiftID_25,
            this.f_ShiftID_26,
            this.f_ShiftID_27,
            this.f_ShiftID_28,
            this.f_ShiftID_29,
            this.f_ShiftID_30,
            this.f_ShiftID_31,
            this.f_ConsumerID});
            resources.ApplyResources(this.dgvMain, "dgvMain");
            this.dgvMain.EnableHeadersVisualStyles = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMain_CellDoubleClick);
            this.dgvMain.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMain_CellFormatting);
            this.dgvMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
            // 
            // f_RecID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_RecID, "f_RecID");
            this.f_RecID.Name = "f_RecID";
            this.f_RecID.ReadOnly = true;
            // 
            // f_DepartmentName
            // 
            resources.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
            this.f_DepartmentName.Name = "f_DepartmentName";
            this.f_DepartmentName.ReadOnly = true;
            // 
            // f_ConsumerNO
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
            this.f_ConsumerNO.Name = "f_ConsumerNO";
            this.f_ConsumerNO.ReadOnly = true;
            // 
            // f_ConsumerName
            // 
            resources.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
            this.f_ConsumerName.Name = "f_ConsumerName";
            this.f_ConsumerName.ReadOnly = true;
            // 
            // f_DateYM
            // 
            resources.ApplyResources(this.f_DateYM, "f_DateYM");
            this.f_DateYM.Name = "f_DateYM";
            this.f_DateYM.ReadOnly = true;
            // 
            // f_ShiftID_01
            // 
            resources.ApplyResources(this.f_ShiftID_01, "f_ShiftID_01");
            this.f_ShiftID_01.Name = "f_ShiftID_01";
            this.f_ShiftID_01.ReadOnly = true;
            // 
            // f_ShiftID_02
            // 
            resources.ApplyResources(this.f_ShiftID_02, "f_ShiftID_02");
            this.f_ShiftID_02.Name = "f_ShiftID_02";
            this.f_ShiftID_02.ReadOnly = true;
            // 
            // f_OnDuty1Short
            // 
            resources.ApplyResources(this.f_OnDuty1Short, "f_OnDuty1Short");
            this.f_OnDuty1Short.Name = "f_OnDuty1Short";
            this.f_OnDuty1Short.ReadOnly = true;
            // 
            // f_ShiftID_04
            // 
            resources.ApplyResources(this.f_ShiftID_04, "f_ShiftID_04");
            this.f_ShiftID_04.Name = "f_ShiftID_04";
            this.f_ShiftID_04.ReadOnly = true;
            // 
            // f_ShiftID_05
            // 
            resources.ApplyResources(this.f_ShiftID_05, "f_ShiftID_05");
            this.f_ShiftID_05.Name = "f_ShiftID_05";
            this.f_ShiftID_05.ReadOnly = true;
            // 
            // f_ShiftID_06
            // 
            resources.ApplyResources(this.f_ShiftID_06, "f_ShiftID_06");
            this.f_ShiftID_06.Name = "f_ShiftID_06";
            this.f_ShiftID_06.ReadOnly = true;
            // 
            // f_ShiftID_07
            // 
            resources.ApplyResources(this.f_ShiftID_07, "f_ShiftID_07");
            this.f_ShiftID_07.Name = "f_ShiftID_07";
            this.f_ShiftID_07.ReadOnly = true;
            // 
            // f_ShiftID_08
            // 
            resources.ApplyResources(this.f_ShiftID_08, "f_ShiftID_08");
            this.f_ShiftID_08.Name = "f_ShiftID_08";
            this.f_ShiftID_08.ReadOnly = true;
            // 
            // f_ShiftID_09
            // 
            resources.ApplyResources(this.f_ShiftID_09, "f_ShiftID_09");
            this.f_ShiftID_09.Name = "f_ShiftID_09";
            this.f_ShiftID_09.ReadOnly = true;
            // 
            // f_ShiftID_10
            // 
            resources.ApplyResources(this.f_ShiftID_10, "f_ShiftID_10");
            this.f_ShiftID_10.Name = "f_ShiftID_10";
            this.f_ShiftID_10.ReadOnly = true;
            // 
            // f_ShiftID_11
            // 
            resources.ApplyResources(this.f_ShiftID_11, "f_ShiftID_11");
            this.f_ShiftID_11.Name = "f_ShiftID_11";
            this.f_ShiftID_11.ReadOnly = true;
            // 
            // f_ShiftID_12
            // 
            resources.ApplyResources(this.f_ShiftID_12, "f_ShiftID_12");
            this.f_ShiftID_12.Name = "f_ShiftID_12";
            this.f_ShiftID_12.ReadOnly = true;
            // 
            // f_ShiftID_13
            // 
            resources.ApplyResources(this.f_ShiftID_13, "f_ShiftID_13");
            this.f_ShiftID_13.Name = "f_ShiftID_13";
            this.f_ShiftID_13.ReadOnly = true;
            // 
            // f_ShiftID_14
            // 
            resources.ApplyResources(this.f_ShiftID_14, "f_ShiftID_14");
            this.f_ShiftID_14.Name = "f_ShiftID_14";
            this.f_ShiftID_14.ReadOnly = true;
            // 
            // f_ShiftID_15
            // 
            resources.ApplyResources(this.f_ShiftID_15, "f_ShiftID_15");
            this.f_ShiftID_15.Name = "f_ShiftID_15";
            this.f_ShiftID_15.ReadOnly = true;
            // 
            // f_ShiftID_16
            // 
            resources.ApplyResources(this.f_ShiftID_16, "f_ShiftID_16");
            this.f_ShiftID_16.Name = "f_ShiftID_16";
            this.f_ShiftID_16.ReadOnly = true;
            // 
            // f_ShiftID_17
            // 
            resources.ApplyResources(this.f_ShiftID_17, "f_ShiftID_17");
            this.f_ShiftID_17.Name = "f_ShiftID_17";
            this.f_ShiftID_17.ReadOnly = true;
            // 
            // f_ShiftID_18
            // 
            resources.ApplyResources(this.f_ShiftID_18, "f_ShiftID_18");
            this.f_ShiftID_18.Name = "f_ShiftID_18";
            this.f_ShiftID_18.ReadOnly = true;
            // 
            // f_ShiftID_19
            // 
            resources.ApplyResources(this.f_ShiftID_19, "f_ShiftID_19");
            this.f_ShiftID_19.Name = "f_ShiftID_19";
            this.f_ShiftID_19.ReadOnly = true;
            // 
            // f_ShiftID_20
            // 
            resources.ApplyResources(this.f_ShiftID_20, "f_ShiftID_20");
            this.f_ShiftID_20.Name = "f_ShiftID_20";
            this.f_ShiftID_20.ReadOnly = true;
            // 
            // f_ShiftID_21
            // 
            resources.ApplyResources(this.f_ShiftID_21, "f_ShiftID_21");
            this.f_ShiftID_21.Name = "f_ShiftID_21";
            this.f_ShiftID_21.ReadOnly = true;
            // 
            // f_ShiftID_22
            // 
            resources.ApplyResources(this.f_ShiftID_22, "f_ShiftID_22");
            this.f_ShiftID_22.Name = "f_ShiftID_22";
            this.f_ShiftID_22.ReadOnly = true;
            // 
            // f_ShiftID_23
            // 
            resources.ApplyResources(this.f_ShiftID_23, "f_ShiftID_23");
            this.f_ShiftID_23.Name = "f_ShiftID_23";
            this.f_ShiftID_23.ReadOnly = true;
            // 
            // f_ShiftID_24
            // 
            resources.ApplyResources(this.f_ShiftID_24, "f_ShiftID_24");
            this.f_ShiftID_24.Name = "f_ShiftID_24";
            this.f_ShiftID_24.ReadOnly = true;
            // 
            // f_ShiftID_25
            // 
            resources.ApplyResources(this.f_ShiftID_25, "f_ShiftID_25");
            this.f_ShiftID_25.Name = "f_ShiftID_25";
            this.f_ShiftID_25.ReadOnly = true;
            // 
            // f_ShiftID_26
            // 
            resources.ApplyResources(this.f_ShiftID_26, "f_ShiftID_26");
            this.f_ShiftID_26.Name = "f_ShiftID_26";
            this.f_ShiftID_26.ReadOnly = true;
            // 
            // f_ShiftID_27
            // 
            resources.ApplyResources(this.f_ShiftID_27, "f_ShiftID_27");
            this.f_ShiftID_27.Name = "f_ShiftID_27";
            this.f_ShiftID_27.ReadOnly = true;
            // 
            // f_ShiftID_28
            // 
            resources.ApplyResources(this.f_ShiftID_28, "f_ShiftID_28");
            this.f_ShiftID_28.Name = "f_ShiftID_28";
            this.f_ShiftID_28.ReadOnly = true;
            // 
            // f_ShiftID_29
            // 
            resources.ApplyResources(this.f_ShiftID_29, "f_ShiftID_29");
            this.f_ShiftID_29.Name = "f_ShiftID_29";
            this.f_ShiftID_29.ReadOnly = true;
            // 
            // f_ShiftID_30
            // 
            resources.ApplyResources(this.f_ShiftID_30, "f_ShiftID_30");
            this.f_ShiftID_30.Name = "f_ShiftID_30";
            this.f_ShiftID_30.ReadOnly = true;
            // 
            // f_ShiftID_31
            // 
            resources.ApplyResources(this.f_ShiftID_31, "f_ShiftID_31");
            this.f_ShiftID_31.Name = "f_ShiftID_31";
            this.f_ShiftID_31.ReadOnly = true;
            // 
            // f_ConsumerID
            // 
            resources.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
            this.f_ConsumerID.Name = "f_ConsumerID";
            this.f_ConsumerID.ReadOnly = true;
            // 
            // userControlFind1
            // 
            resources.ApplyResources(this.userControlFind1, "userControlFind1");
            this.userControlFind1.BackColor = System.Drawing.Color.Transparent;
            this.userControlFind1.BackgroundImage = global::Properties.Resources.pTools_second_title;
            this.userControlFind1.ForeColor = System.Drawing.Color.White;
            this.userControlFind1.Name = "userControlFind1";
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip3.BackgroundImage = global::Properties.Resources.pTools_second_title;
            resources.ApplyResources(this.toolStrip3, "toolStrip3");
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripLabel3});
            this.toolStrip3.Name = "toolStrip3";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.btnClear,
            this.btnPrint,
            this.btnExportToExcel});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_new;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Image = global::Properties.Resources.icon_edit;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = global::Properties.Resources.icon_delete;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Image = global::Properties.Resources.icon_clear;
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::Properties.Resources.icon_print;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportToExcel.Image = global::Properties.Resources.icon_export_excel;
            resources.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // frmShiftOtherData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.userControlFind1);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmShiftOtherData";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
            this.Load += new System.EventHandler(this.frmShiftOtherData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BackgroundWorker backgroundWorker1;
        private ToolStripButton btnAdd;
        private ToolStripButton btnClear;
        private ToolStripButton btnDelete;
        private ToolStripButton btnEdit;
        private ToolStripButton btnExportToExcel;
        private ToolStripButton btnPrint;
        private DataGridView dgvMain;
        private ToolStripDateTime dtpDateFrom;
        private ToolStripDateTime dtpDateTo;
        private DataGridViewTextBoxColumn f_ConsumerID;
        private DataGridViewTextBoxColumn f_ConsumerName;
        private DataGridViewTextBoxColumn f_ConsumerNO;
        private DataGridViewTextBoxColumn f_DateYM;
        private DataGridViewTextBoxColumn f_DepartmentName;
        private DataGridViewTextBoxColumn f_OnDuty1Short;
        private DataGridViewTextBoxColumn f_RecID;
        private DataGridViewTextBoxColumn f_ShiftID_01;
        private DataGridViewTextBoxColumn f_ShiftID_02;
        private DataGridViewTextBoxColumn f_ShiftID_04;
        private DataGridViewTextBoxColumn f_ShiftID_05;
        private DataGridViewTextBoxColumn f_ShiftID_06;
        private DataGridViewTextBoxColumn f_ShiftID_07;
        private DataGridViewTextBoxColumn f_ShiftID_08;
        private DataGridViewTextBoxColumn f_ShiftID_09;
        private DataGridViewTextBoxColumn f_ShiftID_10;
        private DataGridViewTextBoxColumn f_ShiftID_11;
        private DataGridViewTextBoxColumn f_ShiftID_12;
        private DataGridViewTextBoxColumn f_ShiftID_13;
        private DataGridViewTextBoxColumn f_ShiftID_14;
        private DataGridViewTextBoxColumn f_ShiftID_15;
        private DataGridViewTextBoxColumn f_ShiftID_16;
        private DataGridViewTextBoxColumn f_ShiftID_17;
        private DataGridViewTextBoxColumn f_ShiftID_18;
        private DataGridViewTextBoxColumn f_ShiftID_19;
        private DataGridViewTextBoxColumn f_ShiftID_20;
        private DataGridViewTextBoxColumn f_ShiftID_21;
        private DataGridViewTextBoxColumn f_ShiftID_22;
        private DataGridViewTextBoxColumn f_ShiftID_23;
        private DataGridViewTextBoxColumn f_ShiftID_24;
        private DataGridViewTextBoxColumn f_ShiftID_25;
        private DataGridViewTextBoxColumn f_ShiftID_26;
        private DataGridViewTextBoxColumn f_ShiftID_27;
        private DataGridViewTextBoxColumn f_ShiftID_28;
        private DataGridViewTextBoxColumn f_ShiftID_29;
        private DataGridViewTextBoxColumn f_ShiftID_30;
        private DataGridViewTextBoxColumn f_ShiftID_31;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip3;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
    }
}

