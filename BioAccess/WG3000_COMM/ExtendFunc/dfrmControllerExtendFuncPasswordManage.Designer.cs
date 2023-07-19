namespace WG3000_COMM.ExtendFunc
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmControllerExtendFuncPasswordManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmControllerExtendFuncPasswordManage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cmbAuthMode = new System.Windows.Forms.ComboBox();
            this.lblSelectAuthMode = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.f_ReaderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReaderNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ReaderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_AuthenticationMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_AuthenticationMode1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtPasswordNew = new System.Windows.Forms.TextBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.f_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_AdaptTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDel = new System.Windows.Forms.ImageButton();
            this.btnAdd = new System.Windows.Forms.ImageButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cboReader = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBottomBanner.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
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
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(231)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.cmbAuthMode);
            this.tabPage1.Controls.Add(this.lblSelectAuthMode);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.ForeColor = System.Drawing.Color.White;
            this.tabPage1.Name = "tabPage1";
            // 
            // cmbAuthMode
            // 
            this.cmbAuthMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbAuthMode, "cmbAuthMode");
            this.cmbAuthMode.Name = "cmbAuthMode";
            this.cmbAuthMode.SelectedIndexChanged += new System.EventHandler(this.cmbAuthMode_SelectedIndexChanged);
            // 
            // lblSelectAuthMode
            // 
            resources.ApplyResources(this.lblSelectAuthMode, "lblSelectAuthMode");
            this.lblSelectAuthMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblSelectAuthMode.Name = "lblSelectAuthMode";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
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
            this.f_ReaderID,
            this.f_ControllerSN,
            this.f_ReaderNO,
            this.f_ReaderName,
            this.f_AuthenticationMode,
            this.f_AuthenticationMode1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // f_ReaderID
            // 
            resources.ApplyResources(this.f_ReaderID, "f_ReaderID");
            this.f_ReaderID.Name = "f_ReaderID";
            this.f_ReaderID.ReadOnly = true;
            this.f_ReaderID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_ControllerSN
            // 
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            this.f_ControllerSN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_ReaderNO
            // 
            resources.ApplyResources(this.f_ReaderNO, "f_ReaderNO");
            this.f_ReaderNO.Name = "f_ReaderNO";
            this.f_ReaderNO.ReadOnly = true;
            this.f_ReaderNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_ReaderName
            // 
            resources.ApplyResources(this.f_ReaderName, "f_ReaderName");
            this.f_ReaderName.Name = "f_ReaderName";
            this.f_ReaderName.ReadOnly = true;
            this.f_ReaderName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_AuthenticationMode
            // 
            resources.ApplyResources(this.f_AuthenticationMode, "f_AuthenticationMode");
            this.f_AuthenticationMode.Name = "f_AuthenticationMode";
            this.f_AuthenticationMode.ReadOnly = true;
            this.f_AuthenticationMode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_AuthenticationMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // f_AuthenticationMode1
            // 
            resources.ApplyResources(this.f_AuthenticationMode1, "f_AuthenticationMode1");
            this.f_AuthenticationMode1.Name = "f_AuthenticationMode1";
            this.f_AuthenticationMode1.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(231)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.txtPasswordNew);
            this.tabPage3.Controls.Add(this.dataGridView3);
            this.tabPage3.Controls.Add(this.btnDel);
            this.tabPage3.Controls.Add(this.btnAdd);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.cboReader);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.ForeColor = System.Drawing.Color.Black;
            this.tabPage3.Name = "tabPage3";
            // 
            // txtPasswordNew
            // 
            resources.ApplyResources(this.txtPasswordNew, "txtPasswordNew");
            this.txtPasswordNew.Name = "txtPasswordNew";
            this.txtPasswordNew.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPasswordNew_KeyPress);
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridView3, "dataGridView3");
            this.dataGridView3.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_Id,
            this.f_Password,
            this.f_AdaptTo});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView3.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView3.EnableHeadersVisualStyles = false;
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // f_Id
            // 
            resources.ApplyResources(this.f_Id, "f_Id");
            this.f_Id.Name = "f_Id";
            this.f_Id.ReadOnly = true;
            // 
            // f_Password
            // 
            resources.ApplyResources(this.f_Password, "f_Password");
            this.f_Password.Name = "f_Password";
            this.f_Password.ReadOnly = true;
            // 
            // f_AdaptTo
            // 
            this.f_AdaptTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.f_AdaptTo, "f_AdaptTo");
            this.f_AdaptTo.Name = "f_AdaptTo";
            this.f_AdaptTo.ReadOnly = true;
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDel, "btnDel");
            this.btnDel.Focusable = true;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            this.btnDel.Name = "btnDel";
            this.btnDel.Toggle = false;
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Focusable = true;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Toggle = false;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // cboReader
            // 
            this.cboReader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReader.FormattingEnabled = true;
            resources.ApplyResources(this.cboReader, "cboReader");
            this.cboReader.Name = "cboReader";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // dfrmControllerExtendFuncPasswordManage
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.tabControl1);
            this.MinimizeBox = false;
            this.Name = "dfrmControllerExtendFuncPasswordManage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerExtendFuncPasswordManage_FormClosing);
            this.Load += new System.EventHandler(this.dfrmControllerExtendFuncPasswordManage_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmControllerExtendFuncPasswordManage_KeyDown);
            this.panelBottomBanner.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private BackgroundWorker backgroundWorker1;
        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        private TabPage tabPage3;
        internal TextBox txtPasswordNew;
        private DataGridView dataGridView3;
        private DataGridViewTextBoxColumn f_Id;
        private DataGridViewTextBoxColumn f_Password;
        private DataGridViewTextBoxColumn f_AdaptTo;
        private ImageButton btnDel;
        private ImageButton btnAdd;
        private Label label3;
        private ComboBox cboReader;
        private Label label2;
        private Label label1;
        private TabPage tabPage1;
        private DataGridView dataGridView1;
        private TabControl tabControl1;
        private Panel panelBottomBanner;
        private ComboBox cmbAuthMode;
        private Label lblSelectAuthMode;
        private DataGridViewTextBoxColumn f_ReaderID;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_ReaderNO;
        private DataGridViewTextBoxColumn f_ReaderName;
        private DataGridViewTextBoxColumn f_AuthenticationMode;
        private DataGridViewTextBoxColumn f_AuthenticationMode1;
    }
}

