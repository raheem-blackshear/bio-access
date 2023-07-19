namespace WG3000_COMM.Basic
{
	partial class frmCameraSetting
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
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblDataPort = new System.Windows.Forms.Label();
			this.txtDataPort = new System.Windows.Forms.MaskedTextBox();
			this.txtCmdPort = new System.Windows.Forms.MaskedTextBox();
			this.txtIPAddr = new System.Windows.Forms.MaskedTextBox();
			this.lblCmdPort = new System.Windows.Forms.Label();
			this.lblIPAddr = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(141, 118);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 28);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "취소";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(49, 118);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 28);
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "설정";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblDataPort
			// 
			this.lblDataPort.AutoSize = true;
			this.lblDataPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDataPort.Location = new System.Drawing.Point(21, 78);
			this.lblDataPort.Name = "lblDataPort";
			this.lblDataPort.Size = new System.Drawing.Size(55, 15);
			this.lblDataPort.TabIndex = 13;
			this.lblDataPort.Text = "자료포트";
			// 
			// txtDataPort
			// 
			this.txtDataPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDataPort.Location = new System.Drawing.Point(100, 74);
			this.txtDataPort.Name = "txtDataPort";
			this.txtDataPort.Size = new System.Drawing.Size(136, 21);
			this.txtDataPort.TabIndex = 12;
			this.txtDataPort.Text = "34567";
			// 
			// txtCmdPort
			// 
			this.txtCmdPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCmdPort.Location = new System.Drawing.Point(100, 45);
			this.txtCmdPort.Name = "txtCmdPort";
			this.txtCmdPort.Size = new System.Drawing.Size(136, 21);
			this.txtCmdPort.TabIndex = 11;
			this.txtCmdPort.Text = "34567";
			// 
			// txtIPAddr
			// 
			this.txtIPAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtIPAddr.Location = new System.Drawing.Point(100, 16);
			this.txtIPAddr.Name = "txtIPAddr";
			this.txtIPAddr.Size = new System.Drawing.Size(136, 21);
			this.txtIPAddr.TabIndex = 10;
			this.txtIPAddr.Text = "100.100.4.193";
			// 
			// lblCmdPort
			// 
			this.lblCmdPort.AutoSize = true;
			this.lblCmdPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCmdPort.Location = new System.Drawing.Point(21, 48);
			this.lblCmdPort.Name = "lblCmdPort";
			this.lblCmdPort.Size = new System.Drawing.Size(55, 15);
			this.lblCmdPort.TabIndex = 9;
			this.lblCmdPort.Text = "명령포트";
			// 
			// lblIPAddr
			// 
			this.lblIPAddr.AutoSize = true;
			this.lblIPAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblIPAddr.Location = new System.Drawing.Point(30, 20);
			this.lblIPAddr.Name = "lblIPAddr";
			this.lblIPAddr.Size = new System.Drawing.Size(37, 15);
			this.lblIPAddr.TabIndex = 8;
			this.lblIPAddr.Text = "주  소";
			// 
			// frmCameraSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(255, 164);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblDataPort);
			this.Controls.Add(this.txtDataPort);
			this.Controls.Add(this.txtCmdPort);
			this.Controls.Add(this.txtIPAddr);
			this.Controls.Add(this.lblCmdPort);
			this.Controls.Add(this.lblIPAddr);
			this.Name = "frmCameraSetting";
			this.Text = "카메라설정";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblDataPort;
		private System.Windows.Forms.MaskedTextBox txtDataPort;
		private System.Windows.Forms.MaskedTextBox txtCmdPort;
		private System.Windows.Forms.MaskedTextBox txtIPAddr;
		private System.Windows.Forms.Label lblCmdPort;
		private System.Windows.Forms.Label lblIPAddr;
	}
}