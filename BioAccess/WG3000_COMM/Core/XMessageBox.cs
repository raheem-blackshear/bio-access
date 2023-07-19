namespace WG3000_COMM.Core
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class XMessageBox : Form
    {
        private const int maxWidth = 600;
        private const int minWidth = 180;

        private XMessageBox()
        {
            this.InitializeComponent();
            this.TextBox1.Cursor = Cursors.Default;
        }

        private void SetButtons(XMessageBoxButtons buttons)
        {
            if (buttons == XMessageBoxButtons.OK)
            {
                this.button2.Visible = true;
                this.button2.Text = CommonStr.strMsgOK;
                this.button2.DialogResult = DialogResult.OK;
                base.ControlBox = true;
            }
            else if (buttons == XMessageBoxButtons.AbortRetryIgnore)
            {
                this.button1.Visible = true;
                this.button1.Text = CommonStr.strMsgAbort;
                this.button1.DialogResult = DialogResult.Abort;
                this.button2.Visible = true;
                this.button2.Text = CommonStr.strMsgRetry;
                this.button2.DialogResult = DialogResult.Retry;
                this.button3.Visible = true;
                this.button3.Text = CommonStr.strMsgIgnore;
                this.button3.DialogResult = DialogResult.Ignore;
                if (base.Width < 180)
                {
                    base.Width = 180;
                }
            }
            else if (buttons == XMessageBoxButtons.OKCancel)
            {
                this.button1.Visible = true;
                this.button1.Text = CommonStr.strMsgOK;
                this.button1.DialogResult = DialogResult.OK;
                this.button1.Location = new Point(0x29, 3);
                this.button2.Visible = true;
                this.button2.Text = CommonStr.strMsgCancel;
                this.button2.DialogResult = DialogResult.Cancel;
                this.button2.Location = new Point(0x7a, 3);
                base.ControlBox = true;
                if (base.Width < 180)
                {
                    base.Width = 180;
                }
            }
            else if (buttons == XMessageBoxButtons.YesNo)
            {
                this.button1.Visible = true;
                this.button1.Text = CommonStr.strMsgYes;
                this.button1.DialogResult = DialogResult.Yes;
                this.button1.Location = new Point(0x29, 3);
                this.button2.Visible = true;
                this.button2.Text = CommonStr.strMsgNo;
                this.button2.DialogResult = DialogResult.No;
                this.button2.Location = new Point(0x7a, 3);
                if (base.Width < 180)
                {
                    base.Width = 180;
                }
            }
            else if (buttons == XMessageBoxButtons.YesNoCancel)
            {
                this.button1.Visible = true;
                this.button1.DialogResult = DialogResult.Yes;
                this.button1.Text = CommonStr.strMsgYes;
                this.button2.Visible = true;
                this.button2.DialogResult = DialogResult.No;
                this.button2.Text = CommonStr.strMsgNo;
                this.button3.Visible = true;
                this.button3.DialogResult = DialogResult.Cancel;
                this.button3.Text = CommonStr.strMsgCancel;
                if (base.Width < 180)
                {
                    base.Width = 180;
                }
            }
        }

        private void SetIcon(XMessageBoxIcon icon)
        {
            if (base.Height < 0x68)
            {
                base.Height = 0x68;
            }
            if (base.Width <= 600)
            {
                base.Width += 0x37;
            }
            this.leftPanel.Visible = true;
            if (icon == XMessageBoxIcon.Information)
            {
                this.pictureBox1.BackgroundImage = SystemIcons.Information.ToBitmap();
            }
            else if (icon == XMessageBoxIcon.Warning)
            {
                this.pictureBox1.BackgroundImage = SystemIcons.Warning.ToBitmap();
            }
            else if (icon == XMessageBoxIcon.Error)
            {
                this.pictureBox1.BackgroundImage = SystemIcons.Error.ToBitmap();
            }
            else if (icon == XMessageBoxIcon.Question)
            {
                this.pictureBox1.BackgroundImage = SystemIcons.Question.ToBitmap();
            }
            else if (icon == XMessageBoxIcon.Exclamation)
            {
                this.pictureBox1.BackgroundImage = SystemIcons.Exclamation.ToBitmap();
            }
        }

        private static DialogResult show(string text)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, "", box.Size, box.Font, box.BackColor, box.ForeColor, false);
                box.SetButtons(XMessageBoxButtons.OK);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, box.Size, box.Font, box.BackColor, box.ForeColor, false);
                box.SetButtons(XMessageBoxButtons.OK);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, XMessageBoxButtons buttons)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, box.Size, box.Font, box.BackColor, box.ForeColor, false);
                box.SetButtons(buttons);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, Color backColor, Color foreColor)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, box.Size, box.Font, backColor, foreColor, false);
                box.SetButtons(XMessageBoxButtons.OK);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, XMessageBoxButtons buttons, Size size)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, size, box.Font, box.BackColor, box.ForeColor, true);
                box.SetButtons(buttons);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, XMessageBoxButtons buttons, XMessageBoxIcon icon)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, box.Size, box.Font, box.BackColor, box.ForeColor, false);
                box.SetButtons(buttons);
                box.SetIcon(icon);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, box.Size, box.Font, backColor, foreColor, false);
                box.SetButtons(buttons);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, XMessageBoxButtons buttons, Size size, Font font)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, size, font, box.BackColor, box.ForeColor, true);
                box.SetButtons(buttons);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, XMessageBoxButtons buttons, XMessageBoxIcon icon, Size size)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, size, box.Font, box.BackColor, box.ForeColor, true);
                box.SetButtons(buttons);
                box.SetIcon(icon);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons, XMessageBoxIcon icon)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, box.Size, box.Font, backColor, foreColor, false);
                box.SetButtons(buttons);
                box.SetIcon(icon);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, XMessageBoxButtons buttons, XMessageBoxIcon icon, Size size, Font font)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, size, font, box.BackColor, box.ForeColor, true);
                box.SetButtons(buttons);
                box.SetIcon(icon);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons, Size size, Font font)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, size, font, backColor, foreColor, true);
                box.SetButtons(buttons);
                return box.ShowDialog();
            }
        }

        private static DialogResult show(string text, string title, Color backColor, Color foreColor, XMessageBoxButtons buttons, XMessageBoxIcon icon, Size size, Font font)
        {
            using (XMessageBox box = new XMessageBox())
            {
                box.XmessageBox(text, title, size, font, backColor, foreColor, true);
                box.SetButtons(buttons);
                box.SetIcon(icon);
                return box.ShowDialog();
            }
        }

        public static DialogResult Show(string text)
        {
            return Show(null, text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static DialogResult Show(string text, string title, MessageBoxButtons buttons)
        {
            return Show(null, text, title, buttons, MessageBoxIcon.Asterisk);
        }

        public static DialogResult Show(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, title, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string title, MessageBoxButtons buttons)
        {
            return Show(owner, text, title, buttons, MessageBoxIcon.Asterisk);
        }

        public static DialogResult Show(IWin32Window owner, string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            using (XMessageBox box = new XMessageBox())
            {
                Color backColor = Color.FromArgb(211, 231, 251);
                Color fore_color = Color.FromArgb(20, 66, 81);
                box.XmessageBox(text, title, box.Size, box.Font, backColor, fore_color, false);
                if (buttons == MessageBoxButtons.OK)
                {
                    box.SetButtons(XMessageBoxButtons.OK);
                }
                if (buttons == MessageBoxButtons.OKCancel)
                {
                    box.SetButtons(XMessageBoxButtons.OKCancel);
                }
                if (icon == MessageBoxIcon.Exclamation)
                {
                    box.SetIcon(XMessageBoxIcon.Exclamation);
                }
                if (icon == MessageBoxIcon.Asterisk)
                {
                    box.SetIcon(XMessageBoxIcon.Information);
                }
                if (icon == MessageBoxIcon.Hand)
                {
                    box.SetIcon(XMessageBoxIcon.Error);
                }
                if (icon == MessageBoxIcon.Exclamation)
                {
                    box.SetIcon(XMessageBoxIcon.Warning);
                }
                return box.ShowDialog();
            }
        }

        private void XmessageBox(string text, string title, Size size, Font font, Color backColor, Color foreColor, bool customSize)
        {
            int width = 0;
            int height = 0;
            if (!string.IsNullOrEmpty(title))
            {
                this.Text = title;
            }
            this.TextBox1.Text = text;
            this.Font = font;
            this.BackColor = backColor;
            this.BackColor = Color.FromArgb(211, 231, 251);
            this.ForeColor = foreColor;
            Graphics graphics = base.CreateGraphics();
            SizeF ef = graphics.MeasureString(text, font);
            graphics.Dispose();
            if (ef.Width <= 600f)
            {
                width = ((int) ef.Width) + 15;
            }
            else
            {
                width = 600;
            }
            height = ((this.TextBox1.LinesCount() * this.TextBox1.Font.Height) + 40) + 70;
            if (customSize)
            {
                base.Size = size;
            }
            else
            {
                base.Size = new Size(width, height);
            }
        }

        private void XMessageBox_BackColorChanged(object sender, EventArgs e)
        {
            this.TextBox1.BackColor = this.BackColor;
        }

        private void XMessageBox_ForeColorChanged(object sender, EventArgs e)
        {
            this.TextBox1.ForeColor = this.ForeColor;
        }

        private void XMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 0x1b)
            {
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private void XMessageBox_Load(object sender, EventArgs e)
        {
            Icon appicon = base.Icon;
            wgAppConfig.GetAppIcon(ref appicon);
            base.Icon = appicon;
        }

        private void XMessageBox_Resize(object sender, EventArgs e)
        {
            int x = (this.bottomPanel.Width - this.bottomInnerPanel.Width) / 2;
            this.bottomInnerPanel.Location = new Point(x, this.bottomInnerPanel.Location.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.DialogResult = button1.DialogResult;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.DialogResult = button2.DialogResult;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            base.DialogResult = button3.DialogResult;
            Close();
        }
    }
}

