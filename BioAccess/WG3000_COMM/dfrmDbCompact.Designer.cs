namespace WG3000_COMM
{
    using JRO;
    using Microsoft.VisualBasic;
    using System;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmDbCompact
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
        [DebuggerStepThrough]
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmDbCompact));
            this.cmdCompactDatabase = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCompactDatabase
            // 
            resources.ApplyResources(this.cmdCompactDatabase, "cmdCompactDatabase");
            this.cmdCompactDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.cmdCompactDatabase.Focusable = true;
            this.cmdCompactDatabase.ForeColor = System.Drawing.Color.White;
            this.cmdCompactDatabase.Name = "cmdCompactDatabase";
            this.cmdCompactDatabase.Toggle = false;
            this.cmdCompactDatabase.UseVisualStyleBackColor = false;
            this.cmdCompactDatabase.Click += new System.EventHandler(this.cmdCompactDatabase_Click);
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
            // txtDirectory
            // 
            resources.ApplyResources(this.txtDirectory, "txtDirectory");
            this.txtDirectory.Name = "txtDirectory";
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.cmdCompactDatabase);
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmDbCompact
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.panelBottomBanner);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmDbCompact";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.dfrmDbCompact_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmDbCompact_KeyDown);
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal ImageButton btnCancel;
        internal ImageButton cmdCompactDatabase;
        private TextBox txtDirectory;
        private Panel panelBottomBanner;
    }
}

