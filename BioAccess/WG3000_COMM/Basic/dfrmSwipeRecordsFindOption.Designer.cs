namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmSwipeRecordsFindOption
    {
        private ImageButton btnClose;
        private ImageButton btnQuery;
        private ImageButton btnSelectAll;
        private ImageButton btnSelectNone;
        private ComboBox cboZone;
        private CheckedListBox chkListDoors;
        private IContainer components = null;
        private dfrmFind dfrmFind1;
        private DataTable dt;
        private DataView dvDoors;
        private DataView dvDoors4Watching;
        private Label label25;
        private CheckedListBox listViewNotDisplay = new CheckedListBox();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmSwipeRecordsFindOption));
            this.chkListDoors = new System.Windows.Forms.CheckedListBox();
            this.cboZone = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.ImageButton();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnSelectAll = new System.Windows.Forms.ImageButton();
            this.btnSelectNone = new System.Windows.Forms.ImageButton();
            this.SuspendLayout();
            // 
            // chkListDoors
            // 
            this.chkListDoors.CheckOnClick = true;
            this.chkListDoors.FormattingEnabled = true;
            resources.ApplyResources(this.chkListDoors, "chkListDoors");
            this.chkListDoors.MultiColumn = true;
            this.chkListDoors.Name = "chkListDoors";
            // 
            // cboZone
            // 
            this.cboZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZone.FormattingEnabled = true;
            resources.ApplyResources(this.cboZone, "cboZone");
            this.cboZone.Name = "cboZone";
            this.cboZone.SelectedIndexChanged += new System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label25.Name = "label25";
            // 
            // btnQuery
            // 
            this.btnQuery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Focusable = true;
            this.btnQuery.ForeColor = System.Drawing.Color.White;
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Toggle = false;
            this.btnQuery.UseVisualStyleBackColor = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Focusable = true;
            this.btnSelectAll.ForeColor = System.Drawing.Color.White;
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Toggle = false;
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnSelectNone, "btnSelectNone");
            this.btnSelectNone.Focusable = true;
            this.btnSelectNone.ForeColor = System.Drawing.Color.White;
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Toggle = false;
            this.btnSelectNone.UseVisualStyleBackColor = false;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // dfrmSwipeRecordsFindOption
            // 
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.cboZone);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.chkListDoors);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmSwipeRecordsFindOption";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.dfrmSwipeRecordsFindOption_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmSwipeRecordsFindOption_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

