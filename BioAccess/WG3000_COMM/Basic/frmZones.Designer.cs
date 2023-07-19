namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class frmZones
    {
        private ToolStripButton btnAdd;
        private ToolStripButton btnAddSuper;
        private ToolStripButton btnDeleteDept;
        private ToolStripButton btnEditDept;
        private IContainer components = null;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSplitButton1;
        private TreeView trvDepartments;
        private ToolStripTextBox txtSelectedDept;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmZones));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddSuper = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditDept = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeleteDept = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtSelectedDept = new System.Windows.Forms.ToolStripTextBox();
            this.trvDepartments = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImage = global::Properties.Resources.pChild_title;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddSuper,
            this.toolStripSplitButton1,
            this.btnAdd,
            this.toolStripSeparator1,
            this.btnEditDept,
            this.toolStripSeparator2,
            this.btnDeleteDept});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnAddSuper
            // 
            resources.ApplyResources(this.btnAddSuper, "btnAddSuper");
            this.btnAddSuper.ForeColor = System.Drawing.Color.White;
            this.btnAddSuper.Image = global::Properties.Resources.icon_add_top;
            this.btnAddSuper.Name = "btnAddSuper";
            this.btnAddSuper.Click += new System.EventHandler(this.btnAddSuper_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Image = global::Properties.Resources.icon_add_child;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnEditDept
            // 
            resources.ApplyResources(this.btnEditDept, "btnEditDept");
            this.btnEditDept.ForeColor = System.Drawing.Color.White;
            this.btnEditDept.Image = global::Properties.Resources.icon_edit;
            this.btnEditDept.Name = "btnEditDept";
            this.btnEditDept.Click += new System.EventHandler(this.btnEditDept_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnDeleteDept
            // 
            resources.ApplyResources(this.btnDeleteDept, "btnDeleteDept");
            this.btnDeleteDept.ForeColor = System.Drawing.Color.White;
            this.btnDeleteDept.Image = global::Properties.Resources.icon_delete;
            this.btnDeleteDept.Name = "btnDeleteDept";
            this.btnDeleteDept.Click += new System.EventHandler(this.btnDeleteDept_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.BackgroundImage = global::Properties.Resources.pTools_second_title;
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtSelectedDept});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // txtSelectedDept
            // 
            this.txtSelectedDept.BackColor = System.Drawing.SystemColors.Control;
            this.txtSelectedDept.Name = "txtSelectedDept";
            this.txtSelectedDept.ReadOnly = true;
            resources.ApplyResources(this.txtSelectedDept, "txtSelectedDept");
            this.txtSelectedDept.TextChanged += new System.EventHandler(this.txtSelectedDept_TextChanged);
            // 
            // trvDepartments
            // 
            resources.ApplyResources(this.trvDepartments, "trvDepartments");
            this.trvDepartments.Name = "trvDepartments";
            this.trvDepartments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvDepartments_AfterSelect);
            // 
            // frmZones
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Properties.Resources.pMain_content_bkg;
            this.Controls.Add(this.trvDepartments);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.MinimizeBox = false;
            this.Name = "frmZones";
            this.Load += new System.EventHandler(this.frmDepartments_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

