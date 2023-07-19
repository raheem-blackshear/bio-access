namespace WG3000_COMM.Core
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class frmBioAccess : Form
    {
        public frmBioAccess()
        {
            this.InitializeComponent();
        }

        private void frmBioAccess_Load(object sender, EventArgs e)
        {
            Icon appicon = base.Icon;
            wgAppConfig.GetAppIcon(ref appicon);
            base.Icon = appicon;
            if (!base.IsMdiContainer)
            {
                wgAppRunInfo.ClearAllDisplayedInfo();
            }
        }
    }
}

