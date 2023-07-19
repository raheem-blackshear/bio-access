namespace WG3000_COMM.Basic
{
    partial class dfrmRecordDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmRecordDetail));
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.picPhoto = new System.Windows.Forms.PictureBox();
            this.lblPhoto = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPassage = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblRecID = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtRecID = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.txtDateTime = new System.Windows.Forms.TextBox();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPassage = new System.Windows.Forms.TextBox();
            this.panelBottomBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnClose);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnClose.Focusable = true;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.Toggle = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picPhoto
            // 
            resources.ApplyResources(this.picPhoto, "picPhoto");
            this.picPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPhoto.Name = "picPhoto";
            this.picPhoto.TabStop = false;
            // 
            // lblPhoto
            // 
            resources.ApplyResources(this.lblPhoto, "lblPhoto");
            this.lblPhoto.Name = "lblPhoto";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lblPassage, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblRecID, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblUserID, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtRecID, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtUserID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblUserName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtUserName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDepartment, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDepartment, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDateTime, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblDateTime, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtAddress, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtPassage, 1, 6);
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // lblPassage
            // 
            resources.ApplyResources(this.lblPassage, "lblPassage");
            this.lblPassage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblPassage.Name = "lblPassage";
            // 
            // lblAddress
            // 
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblAddress.Name = "lblAddress";
            // 
            // lblRecID
            // 
            resources.ApplyResources(this.lblRecID, "lblRecID");
            this.lblRecID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblRecID.Name = "lblRecID";
            // 
            // lblUserID
            // 
            resources.ApplyResources(this.lblUserID, "lblUserID");
            this.lblUserID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblUserID.Name = "lblUserID";
            // 
            // txtRecID
            // 
            resources.ApplyResources(this.txtRecID, "txtRecID");
            this.txtRecID.Name = "txtRecID";
            this.txtRecID.ReadOnly = true;
            this.txtRecID.TabStop = false;
            // 
            // txtUserID
            // 
            resources.ApplyResources(this.txtUserID, "txtUserID");
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.TabStop = false;
            // 
            // lblUserName
            // 
            resources.ApplyResources(this.lblUserName, "lblUserName");
            this.lblUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblUserName.Name = "lblUserName";
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            // 
            // lblDepartment
            // 
            resources.ApplyResources(this.lblDepartment, "lblDepartment");
            this.lblDepartment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblDepartment.Name = "lblDepartment";
            // 
            // txtDepartment
            // 
            resources.ApplyResources(this.txtDepartment, "txtDepartment");
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            // 
            // txtDateTime
            // 
            resources.ApplyResources(this.txtDateTime, "txtDateTime");
            this.txtDateTime.Name = "txtDateTime";
            this.txtDateTime.ReadOnly = true;
            // 
            // lblDateTime
            // 
            resources.ApplyResources(this.lblDateTime, "lblDateTime");
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblDateTime.Name = "lblDateTime";
            // 
            // txtAddress
            // 
            resources.ApplyResources(this.txtAddress, "txtAddress");
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            // 
            // txtPassage
            // 
            resources.ApplyResources(this.txtPassage, "txtPassage");
            this.txtPassage.Name = "txtPassage";
            this.txtPassage.ReadOnly = true;
            // 
            // dfrmRecordDetail
            // 
            this.AcceptButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblPhoto);
            this.Controls.Add(this.picPhoto);
            this.Controls.Add(this.panelBottomBanner);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmRecordDetail";
            this.panelBottomBanner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelBottomBanner;
        internal System.Windows.Forms.ImageButton btnClose;
        private System.Windows.Forms.PictureBox picPhoto;
        private System.Windows.Forms.Label lblPhoto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblPassage;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblRecID;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.TextBox txtRecID;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtDateTime;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtPassage;
    }
}