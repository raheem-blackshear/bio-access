namespace WG3000_COMM.Basic
{
    using System;
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

    partial class dfrmOperatorZonesConfiguration
    {
        private ImageButton btnAddAllGroups;
        private ImageButton btnAddOneGroup;
        private ImageButton btnDeleteAllGroups;
        private ImageButton btnDeleteOneGroup;
        private ImageButton btnZoneManage;
        internal ImageButton button1;
        internal ImageButton button2;
        private Container components = null;
        internal Label Label10;
        internal Label Label11;
        internal ListBox lstOptionalGroups;
        internal ListBox lstSelectedGroups;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmOperatorZonesConfiguration));
            this.button1 = new System.Windows.Forms.ImageButton();
            this.button2 = new System.Windows.Forms.ImageButton();
            this.btnDeleteAllGroups = new System.Windows.Forms.ImageButton();
            this.btnDeleteOneGroup = new System.Windows.Forms.ImageButton();
            this.btnAddOneGroup = new System.Windows.Forms.ImageButton();
            this.btnAddAllGroups = new System.Windows.Forms.ImageButton();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.lstSelectedGroups = new System.Windows.Forms.ListBox();
            this.lstOptionalGroups = new System.Windows.Forms.ListBox();
            this.btnZoneManage = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.button1.Focusable = true;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.Toggle = false;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.button2.Focusable = true;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Name = "button2";
            this.button2.Toggle = false;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDeleteAllGroups
            // 
            this.btnDeleteAllGroups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteAllGroups, "btnDeleteAllGroups");
            this.btnDeleteAllGroups.Focusable = true;
            this.btnDeleteAllGroups.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAllGroups.Name = "btnDeleteAllGroups";
            this.btnDeleteAllGroups.Toggle = false;
            this.btnDeleteAllGroups.UseVisualStyleBackColor = false;
            this.btnDeleteAllGroups.Click += new System.EventHandler(this.btnDeleteAllGroups_Click);
            // 
            // btnDeleteOneGroup
            // 
            this.btnDeleteOneGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDeleteOneGroup, "btnDeleteOneGroup");
            this.btnDeleteOneGroup.Focusable = true;
            this.btnDeleteOneGroup.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOneGroup.Name = "btnDeleteOneGroup";
            this.btnDeleteOneGroup.Toggle = false;
            this.btnDeleteOneGroup.UseVisualStyleBackColor = false;
            this.btnDeleteOneGroup.Click += new System.EventHandler(this.btnDeleteOneGroup_Click);
            // 
            // btnAddOneGroup
            // 
            this.btnAddOneGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddOneGroup, "btnAddOneGroup");
            this.btnAddOneGroup.Focusable = true;
            this.btnAddOneGroup.ForeColor = System.Drawing.Color.White;
            this.btnAddOneGroup.Name = "btnAddOneGroup";
            this.btnAddOneGroup.Toggle = false;
            this.btnAddOneGroup.UseVisualStyleBackColor = false;
            this.btnAddOneGroup.Click += new System.EventHandler(this.btnAddOneGroup_Click);
            // 
            // btnAddAllGroups
            // 
            this.btnAddAllGroups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddAllGroups, "btnAddAllGroups");
            this.btnAddAllGroups.Focusable = true;
            this.btnAddAllGroups.ForeColor = System.Drawing.Color.White;
            this.btnAddAllGroups.Name = "btnAddAllGroups";
            this.btnAddAllGroups.Toggle = false;
            this.btnAddAllGroups.UseVisualStyleBackColor = false;
            this.btnAddAllGroups.Click += new System.EventHandler(this.btnAddAllGroups_Click);
            // 
            // Label11
            // 
            resources.ApplyResources(this.Label11, "Label11");
            this.Label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label11.Name = "Label11";
            // 
            // Label10
            // 
            resources.ApplyResources(this.Label10, "Label10");
            this.Label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.Label10.Name = "Label10";
            // 
            // lstSelectedGroups
            // 
            resources.ApplyResources(this.lstSelectedGroups, "lstSelectedGroups");
            this.lstSelectedGroups.Name = "lstSelectedGroups";
            this.lstSelectedGroups.DoubleClick += new System.EventHandler(this.btnDeleteOneGroup_Click);
            // 
            // lstOptionalGroups
            // 
            resources.ApplyResources(this.lstOptionalGroups, "lstOptionalGroups");
            this.lstOptionalGroups.Name = "lstOptionalGroups";
            this.lstOptionalGroups.DoubleClick += new System.EventHandler(this.btnAddOneGroup_Click);
            // 
            // btnZoneManage
            // 
            this.btnZoneManage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnZoneManage, "btnZoneManage");
            this.btnZoneManage.Focusable = true;
            this.btnZoneManage.ForeColor = System.Drawing.Color.White;
            this.btnZoneManage.Name = "btnZoneManage";
            this.btnZoneManage.Toggle = false;
            this.btnZoneManage.UseVisualStyleBackColor = false;
            this.btnZoneManage.Click += new System.EventHandler(this.btnZoneManage_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.button2);
            this.panelBottomBanner.Controls.Add(this.button1);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmOperatorZonesConfiguration
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.btnZoneManage);
            this.Controls.Add(this.btnDeleteAllGroups);
            this.Controls.Add(this.btnDeleteOneGroup);
            this.Controls.Add(this.btnAddOneGroup);
            this.Controls.Add(this.btnAddAllGroups);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.lstSelectedGroups);
            this.Controls.Add(this.lstOptionalGroups);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmOperatorZonesConfiguration";
            this.Load += new System.EventHandler(this.dfrmSwitchGroupsConfiguration_Load);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel panelBottomBanner;
    }
}

