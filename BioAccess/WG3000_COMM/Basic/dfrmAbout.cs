namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmAbout : frmBioAccess
    {
        private bool bDispSpecial;

        public dfrmAbout()
        {
            this.InitializeComponent();
            this.label1.Text = this.AssemblyProduct;
            this.label2.Text = string.Format("Version {0}", this.AssemblyVersion);
            this.label3.Text = this.AssemblyCopyright;
            this.label4.Text = "";
            this.textBoxDescription.Text = this.AssemblyDescription;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (dfrmRegister register = new dfrmRegister())
            {
                if (register.ShowDialog(this) == DialogResult.OK)
                {
                    this.textBoxDescription.Text = "";
                    this.dfrmAbout_Load(null, null);
                }
            }
        }

        private void dfrmAbout_KeyDown(object sender, KeyEventArgs e)
        {
            if ((!this.bDispSpecial && e.Control) && (e.Shift && (e.KeyValue == 0x57)))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.bDispSpecial = true;
                    this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n2010 版权所有(C) 深圳市微耕实业有限公司\r\n保留所有权利";
                    this.label5.Text = this.textBoxDescription.Text.Replace("\r\n", "\r\n\r\n");
                }
            }
        }

        private void dfrmAbout_Load(object sender, EventArgs e)
        {
            this.textBoxDescription.ForeColor = Color.White;
            this.label1.Text = base.Owner.Text;
            this.label2.Text = string.Format("Software Version: {0}", this.AssemblyVersion);
            this.label3.Text = "Database Version: " + wgAppConfig.getSystemParamByNO(9) + (wgAppConfig.IsAccessDB ? "  [Microsoft Access]" : "[MS Sql Server]");
            this.label4.Text = string.Format(".Net Framework {0} ", Environment.Version.ToString());
            this.label5.Text = "";
            try
            {
                if (!string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(12)) && (wgAppConfig.getSystemParamByNO(12) == "200405"))
                {
                    this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + CommonStr.strRegisterAlready;
                    if (!string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(0x24)))
                    {
                        this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + wgAppConfig.getSystemParamByNO(0x24);
                    }
                    this.textBoxDescription.Text = this.textBoxDescription.Text + "\r\n" + CommonStr.strWelcomeToUse;
                    this.label5.Text = this.textBoxDescription.Text.Replace("\r\n", "\r\n\r\n");
                    this.btnRegister.Text = CommonStr.strRegisterAgain;
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string AssemblyCompany
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute) customAttributes[0]).Company;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (customAttributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute) customAttributes[0]).Product;
            }
        }

        public string AssemblyTitle
        {
            get
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (customAttributes.Length > 0)
                {
                    AssemblyTitleAttribute attribute = (AssemblyTitleAttribute) customAttributes[0];
                    if (attribute.Title != "")
                    {
                        return attribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}

