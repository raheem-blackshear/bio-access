namespace WG3000_COMM.Core
{
    using System;
    using System.Windows.Forms;

    public class XTextBoxAA : TextBox
    {
        public int LinesCount()
        {
            Message m = Message.Create(base.Handle, 0xba, IntPtr.Zero, IntPtr.Zero);
            base.DefWndProc(ref m);
            return m.Result.ToInt32();
        }
    }
}

