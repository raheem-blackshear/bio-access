namespace WG3000_COMM.Reports.Shift
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
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmHolidayType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmHolidayType));
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnDel = new System.Windows.Forms.ImageButton();
            this.btnEdit = new System.Windows.Forms.ImageButton();
            this.btnAdd = new System.Windows.Forms.ImageButton();
            this.lstHolidayType = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.TabStop = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnDel, "btnDel");
            this.btnDel.Name = "btnDel";
            this.btnDel.TabStop = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.TabStop = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.TabStop = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstHolidayType
            // 
            this.lstHolidayType.FormattingEnabled = true;
            resources.ApplyResources(this.lstHolidayType, "lstHolidayType");
            this.lstHolidayType.Name = "lstHolidayType";
            // 
            // dfrmHolidayType
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lstHolidayType);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmHolidayType";
            this.Load += new System.EventHandler(this.dfrmHolidayType_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageButton btnAdd;
        private ImageButton btnDel;
        private ImageButton btnEdit;
        private ImageButton btnExit;
        private ListBox lstHolidayType;
    }
}

