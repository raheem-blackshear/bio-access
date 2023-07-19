namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftAttReportFindOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmShiftAttReportFindOption));
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnQuery = new System.Windows.Forms.ImageButton();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
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
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            resources.ApplyResources(this.checkedListBox1, "checkedListBox1");
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            // 
            // dfrmShiftAttReportFindOption
            // 
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "dfrmShiftAttReportFindOption";
            this.Load += new System.EventHandler(this.dfrmShiftAttReportFindOption_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageButton btnClose;
        private ImageButton btnQuery;
        private CheckedListBox checkedListBox1;
    }
}

