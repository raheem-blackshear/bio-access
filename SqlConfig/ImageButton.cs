using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class ImageButton : Button
    {
        #region Hover background color
        private Color m_HoverBkColor = Color.FromArgb(69, 155, 198);

/*
        [Category("Appearance")]
        [Description("Image to show when the button is hovered over.")]
        public Image HoverImage
        {
            get { return m_HoverImage; }
            set { m_HoverImage = value; if (hover) Image = value; }
        }
*/
        #endregion
        #region Down background color
        private Color m_DownBkColor = Color.FromArgb(19, 88, 123);

/*
        [Category("Appearance")]
        [Description("Image to show when the button is depressed.")]
        public Image DownImage
        {
            get { return m_DownImage; }
            set { m_DownImage = value; if (down) Image = value; }
        }
*/
        #endregion
        #region Normal background color
        private Color m_NormalBkColor = Color.FromArgb(43, 124, 170);

/*
        [Category("Appearance")]
        [Description("Image to show when the button is not in any other state.")]
        public Image NormalImage
        {
            get { return m_NormalImage; }
            set { m_NormalImage = value; if (!(hover || down)) Image = value; }
        }
*/
        bool focusable = true;
        [Category("Behavior")]
        [Description("Focusable overrides.")]
        public bool Focusable
        {
            get { return focusable; }
            set { focusable = value; }
        }
        #endregion

        #region Disabled background color
        private Color m_DisabledBkColor = Color.FromArgb(113, 152, 175);

        /*
                [Category("Appearance")]
                [Description("Image to show when the button is not in any other state.")]
                public Image NormalImage
                {
                    get { return m_NormalImage; }
                    set { m_NormalImage = value; if (!(hover || down)) Image = value; }
                }
        */
        #endregion

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private bool hover = false;
        private bool down = false;
        private bool isDefault = false;
        private bool focused = false;

        #region Overrides

        [Category("Appearance")]
        [Description("Background color of button.")]
        public override Color BackColor
        {
            get
            {
                if (base.BackColor == Color.Transparent)
                    return m_NormalBkColor;
                return base.BackColor;
            }
            set
            {
                if (base.DesignMode)
                {
                    if (value != Color.Empty)
                    {
                        PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this)["UseVisualStyleBackColor"];
                        if (descriptor != null)
                        {
                            descriptor.SetValue(this, false);
                        }
                    }
                }
                else
                {
                    this.UseVisualStyleBackColor = false;
                }
                base.BackColor = value;
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                BackColor = m_NormalBkColor;
            else
                BackColor = Color.Transparent;
            base.OnEnabledChanged(e);
        }

        public override Color ForeColor
        {
            get
            {
                return Color.White;
            }

            set
            {
                base.ForeColor = Color.FromArgb(0, 255, 254, 255);
            }
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Hiding

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

        #endregion

        private void refresh_background()
        {
            down = false;
            if (hover)
            {
                if (m_HoverBkColor != null)
                    BackColor = m_HoverBkColor;
            }
            else
                BackColor = m_NormalBkColor;
        }

        #region Events
        protected override void OnMouseMove(MouseEventArgs e)
        {
            hover = true;
            if (down)
            {
                if ((m_DownBkColor != null) && (BackColor != m_DownBkColor))
                    BackColor = m_DownBkColor;
            }
            else
                if (m_HoverBkColor != null)
                    BackColor = m_HoverBkColor;
                else
                    BackColor = m_NormalBkColor;
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hover = false;
            BackColor = m_NormalBkColor;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Focus();
            refresh_background();
            down = true;
            if (m_DownBkColor != null)
                BackColor = m_DownBkColor;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            refresh_background();
            base.OnMouseUp(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            focused = false;
            refresh_background();

            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            focused = true;
            base.OnGotFocus(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (focused || isDefault)
            {
                Pen drawPen = new Pen(System.Drawing.Color.White, 2);
                pevent.Graphics.DrawRectangle(drawPen, 0, 0, base.Width, base.Height);

                if (focusable && focused && base.Width > 6 && base.Height > 6)
                {
                    Pen interPen = new Pen(System.Drawing.Color.White, 1);
                    interPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    pevent.Graphics.DrawRectangle(interPen, 3, 3, base.Width - 7, base.Height - 7);
                }
            }

/*
            if (!Enabled)
            {
                if ((!string.IsNullOrEmpty(Text)) && (pevent != null) && (Font != null))
                {
                    SolidBrush drawBrush = new SolidBrush(Enabled ? Color.White : Color.FromArgb(172, 179, 183));
                    SizeF drawStringSize = pevent.Graphics.MeasureString(Text, Font);
                    PointF drawPoint;
                    drawPoint = new PointF(base.Width / 2 - drawStringSize.Width / 2, base.Height / 2 - drawStringSize.Height / 2);
                    pevent.Graphics.DrawString(Text, Font, drawBrush, drawPoint);
                }
            }
*/
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }
        #endregion

        public ImageButton()
        {
            base.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            base.ForeColor = Color.FromArgb(0, 255, 254, 255);
            base.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(147, 188, 212);
            BackColor = m_NormalBkColor;
        }
    }
}
