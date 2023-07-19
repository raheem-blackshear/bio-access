namespace WG3000_COMM.Core
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Properties;

    partial class ucMapDoor : UserControl
    {
        private IContainer components;
        public ImageList imgDoor;
        private PictureBox m_bindSource;
        private Point m_doorLocation;
        public PictureBox picDoorState;
        internal Timer Timer1;
        internal Label txtDoorName;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMapDoor));
            this.picDoorState = new System.Windows.Forms.PictureBox();
            this.imgDoor = new System.Windows.Forms.ImageList(this.components);
            this.txtDoorName = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picDoorState)).BeginInit();
            this.SuspendLayout();
            // 
            // picDoorState
            // 
            resources.ApplyResources(this.picDoorState, "picDoorState");
            this.picDoorState.Image = global::Properties.Resources.pConsole_Door_Unknown;
            this.picDoorState.Name = "picDoorState";
            this.picDoorState.TabStop = false;
            this.picDoorState.Leave += new System.EventHandler(this.ucMapDoor_Leave);
            this.picDoorState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDoorState_MouseDown);
            // 
            // imgDoor
            // 
            this.imgDoor.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            resources.ApplyResources(this.imgDoor, "imgDoor");
            this.imgDoor.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtDoorName
            // 
            resources.ApplyResources(this.txtDoorName, "txtDoorName");
            this.txtDoorName.BackColor = System.Drawing.Color.White;
            this.txtDoorName.Name = "txtDoorName";
            this.txtDoorName.Click += new System.EventHandler(this.ucMapDoor_Click);
            // 
            // Timer1
            // 
            this.Timer1.Interval = 500;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // ucMapDoor
            // 
            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtDoorName);
            this.Controls.Add(this.picDoorState);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "ucMapDoor";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.ucMapDoor_Load);
            this.Click += new System.EventHandler(this.ucMapDoor_Click);
            this.Leave += new System.EventHandler(this.ucMapDoor_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.picDoorState)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

