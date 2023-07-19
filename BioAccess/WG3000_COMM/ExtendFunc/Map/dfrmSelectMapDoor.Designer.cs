namespace WG3000_COMM.ExtendFunc.Map
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmSelectMapDoor
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
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmSelectMapDoor));
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.lstUnMappedDoors = new System.Windows.Forms.ListBox();
            this.lstMappedDoors = new System.Windows.Forms.ListBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
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
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstUnMappedDoors
            // 
            resources.ApplyResources(this.lstUnMappedDoors, "lstUnMappedDoors");
            this.lstUnMappedDoors.Name = "lstUnMappedDoors";
            this.lstUnMappedDoors.SelectedIndexChanged += new System.EventHandler(this.lstUnMappedDoors_SelectedIndexChanged);
            this.lstUnMappedDoors.DoubleClick += new System.EventHandler(this.lstUnMappedDoors_DoubleClick);
            this.lstUnMappedDoors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstUnMappedDoors_MouseDoubleClick);
            // 
            // lstMappedDoors
            // 
            resources.ApplyResources(this.lstMappedDoors, "lstMappedDoors");
            this.lstMappedDoors.Name = "lstMappedDoors";
            this.lstMappedDoors.SelectedIndexChanged += new System.EventHandler(this.lstMappedDoors_SelectedIndexChanged);
            this.lstMappedDoors.DoubleClick += new System.EventHandler(this.lstMappedDoors_DoubleClick);
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Name = "Label2";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnOK);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmSelectMapDoor
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lstUnMappedDoors);
            this.Controls.Add(this.lstMappedDoors);
            this.Controls.Add(this.Label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmSelectMapDoor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmSelectMapDoor_FormClosing);
            this.Load += new System.EventHandler(this.dfrmSelectMapDoor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmSelectMapDoor_KeyDown);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        
        internal ImageButton btnCancel;
        internal ImageButton btnOK;
        internal Label Label1;
        internal Label Label2;
        internal ListBox lstMappedDoors;
        internal ListBox lstUnMappedDoors;
        private Panel panelBottomBanner;
    }
}

