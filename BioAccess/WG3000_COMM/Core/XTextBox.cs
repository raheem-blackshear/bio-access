namespace WG3000_COMM.Core
{
    using System;
    using System.Security.Permissions;
    using System.Windows.Forms;

    public class XTextBox : TextBox
    {
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        public int LinesCount()
        {
            Message m = Message.Create(base.Handle, 0xba, IntPtr.Zero, IntPtr.Zero);
            base.DefWndProc(ref m);
            return m.Result.ToInt32();
        }
    }
}

