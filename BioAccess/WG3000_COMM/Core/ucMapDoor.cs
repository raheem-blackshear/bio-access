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
        private const int CONTROL_HEIGHT = 50;
        private const int FONT_HEIGHT = 0x10;
        private const int FONTSIZE = 9;
        public int idoorWarnSource = 0;
        private const int IMG_HEIGHT = 0x20;
        private const int IMG_WIDTH = 0x18;
        private string m_doorName = "门名称";
        private float m_doorScale = 1f;
        private int m_doorStatus;
        private float m_mapScale = 1f;

        public ucMapDoor()
        {
            this.InitializeComponent();
            this.txtDoorName.Text = this.m_doorName;
            this.doorLocation = base.Location;
        }

        private void picDoorState_MouseDown(object sender, MouseEventArgs e)
        {
        }

        public void redraw()
        {
            try
            {
                this.picDoorState.Size = new Size(new Point((int) ((24f * this.m_mapScale) * this.m_doorScale), (int) ((32f * this.m_mapScale) * this.m_doorScale)));
                this.txtDoorName.Text = this.m_doorName;
                this.txtDoorName.Font = new Font("Arial", 9f * this.m_mapScale);
                this.txtDoorName.Location = new Point(0, (this.picDoorState.Location.Y + this.picDoorState.Size.Height) + 2);
                base.Location = new Point((int) (this.m_doorLocation.X * this.m_mapScale), (int) (this.m_doorLocation.Y * this.m_mapScale));
                base.Size = new Size(new Point(Math.Max(this.txtDoorName.Width, this.picDoorState.Size.Width), (int) (((32f * this.doorScale) + 16f) * this.m_mapScale)));
                this.picDoorState.Location = new Point((base.Size.Width - this.picDoorState.Width) / 2, 0);
            }
            catch (Exception)
            {
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.picDoorState.Visible = !this.picDoorState.Visible;
        }

        private void ucMapDoor_Click(object sender, EventArgs e)
        {
            this.txtDoorName.ForeColor = Color.White;
            this.txtDoorName.BackColor = Color.DodgerBlue;
            base.ActiveControl = this.txtDoorName;
        }

        private void ucMapDoor_Leave(object sender, EventArgs e)
        {
            this.txtDoorName.ForeColor = Color.Black;
            this.txtDoorName.BackColor = Color.White;
        }

        private void ucMapDoor_Load(object sender, EventArgs e)
        {
            this.redraw();
        }

        public PictureBox bindSource
        {
            get
            {
                return this.m_bindSource;
            }
            set
            {
                this.m_bindSource = value;
            }
        }

        public Point doorLocation
        {
            get
            {
                return this.m_doorLocation;
            }
            set
            {
                this.m_doorLocation = new Point((int) (((float) value.X) / this.mapScale), (int) (((float) value.Y) / this.mapScale));
            }
        }

        public string doorName
        {
            get
            {
                return this.m_doorName;
            }
            set
            {
                if ((value != null) && (this.m_doorName != value))
                {
                    this.m_doorName = value;
                    this.txtDoorName.Text = this.m_doorName;
                    this.redraw();
                }
            }
        }

        public float doorScale
        {
            get
            {
                return this.m_doorScale;
            }
            set
            {
                if (this.m_doorScale != value)
                {
                    this.m_doorScale = value;
                    this.redraw();
                }
            }
        }

        public int doorStatus
        {
            get
            {
                return this.m_doorStatus;
            }
            set
            {
                if (this.m_doorStatus != value)
                {
                    this.m_doorStatus = 0;
                    this.m_doorStatus = value;
                    switch (this.m_doorStatus)
                    {
                        case 0:
                            this.picDoorState.Image = Resources.pConsole_Door_Unknown;
                            break;

                        case 1:
                            this.picDoorState.Image = Resources.pConsole_Door_NormalClose;
                            break;

                        case 2:
                            this.picDoorState.Image = Resources.pConsole_Door_NormalOpen;
                            break;

                        case 3:
                            this.picDoorState.Image = Resources.pConsole_Door_NotConnected;
                            break;

                        case 4:
                            this.picDoorState.Image = Resources.pConsole_Door_WarnClose;
                            break;

                        case 5:
                            this.picDoorState.Image = Resources.pConsole_Door_WarnOpen;
                            break;

                        case 6:
                            this.picDoorState.Image = Resources.pConsole_Door_Unknown;
                            break;

                        case 7:
                            this.picDoorState.Image = Resources.pConsole_Door_WarnClose;
                            break;

                        case 8:
                            this.picDoorState.Image = Resources.pConsole_Door_WarnOpen;
                            break;

                        case 9:
                            this.picDoorState.Image = Resources.pConsole_Door_NotConnected;
                            break;

                        default:
                            this.picDoorState.Image = Resources.pConsole_Door_Unknown;
                            break;
                    }
                    if ((this.m_doorStatus == 4) || (this.m_doorStatus == 5))
                    {
                        this.Timer1.Enabled = true;
                    }
                    else
                    {
                        this.Timer1.Enabled = false;
                        this.picDoorState.Visible = true;
                    }
                }
            }
        }

        public float mapScale
        {
            get
            {
                return this.m_mapScale;
            }
            set
            {
                if (this.m_mapScale != value)
                {
                    this.m_mapScale = value;
                    this.redraw();
                }
            }
        }
    }
}

