using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class BorderColorGroupBox : GroupBox
    {
        private Color borderColor;

        [Category("Appearance")]
        [Description("Image to show when the button is hovered over.")]
        public Color BorderColor
        {
            get { return this.borderColor; }
            set { this.borderColor = value; }
        }

        public BorderColorGroupBox()
        {
            this.borderColor = Color.Silver;
        }
 
        protected override void OnPaint(PaintEventArgs e)
        {
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);
 
            Rectangle borderRect = e.ClipRectangle;
            borderRect.X += 1; borderRect.Width -= 1;
            borderRect.Height -= 1;

            borderRect.Y += tSize.Height / 2;
            borderRect.Height -= tSize.Height / 2;
            ControlPaint.DrawBorder(e.Graphics, borderRect, this.borderColor, ButtonBorderStyle.Solid);
 
            Rectangle textRect = e.ClipRectangle;
            textRect.X += 6;
            textRect.Width  = tSize.Width;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);
        }

    }
}
