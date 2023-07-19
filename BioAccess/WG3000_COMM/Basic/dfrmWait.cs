namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;

    public partial class dfrmWait : Form
    {
        public dfrmWait()
        {
            this.InitializeComponent();
        }

        private void dfrmWait_Load(object sender, EventArgs e)
        {
            Icon appicon = base.Icon;
            wgAppConfig.GetAppIcon(ref appicon);
            base.Icon = appicon;
            this.Refresh();
            Cursor.Current = Cursors.WaitCursor;
        }
    }
}

