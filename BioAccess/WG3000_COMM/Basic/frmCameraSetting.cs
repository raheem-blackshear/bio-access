using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WG3000_COMM.Basic
{
	using WG3000_COMM.Core;
	using Microsoft.Win32;
	public partial class frmCameraSetting : Form
	{
		public UInt32 ipAddr;
		public UInt16 cmdPort, dataPort;

		public frmCameraSetting(UInt32 ipAddr, UInt16 cmdPort, UInt16 dataPort)
		{
			InitializeComponent();

			Byte a = (Byte)((ipAddr >> 24) & 255);
			Byte b = (Byte)((ipAddr >> 16) & 255);
			Byte c = (Byte)((ipAddr >> 8) & 255);
			Byte d = (Byte)((ipAddr) & 255);
			txtIPAddr.Text = a.ToString() + "." + b.ToString() + "." + c.ToString() + "." + d.ToString();
			txtCmdPort.Text = cmdPort.ToString();
			txtDataPort.Text = dataPort.ToString();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			string ipString = txtIPAddr.Text;
			string[] split = ipString.Split(new Char[] {'.'});
			int a = 0, b = 0, c = 0, d = 0;
			if (split.Length != 4 ||
				!Int32.TryParse(split[0], out a) ||
				!Int32.TryParse(split[1], out b) ||
				!Int32.TryParse(split[2], out c) ||
				!Int32.TryParse(split[3], out d) ||
				a < 0 || a > 255 ||
				b < 0 || b > 255 ||
				c < 0 || c > 255 ||
				d < 0 || d > 255)
				XMessageBox.Show("주소를 다시 입력하여주십시오.");
			else if (!UInt16.TryParse(txtCmdPort.Text, out cmdPort))
				XMessageBox.Show("명령포트를 다시 입력하여주십시오.");
			else if (!UInt16.TryParse(txtDataPort.Text, out dataPort))
				XMessageBox.Show("자료포트를 다시 입력하여주십시오.");
			else
			{
				RegistryKey regKey;
				regKey = Registry.CurrentUser.CreateSubKey("Software\\KPT\\BioAccess");
				ipAddr = (UInt32)(a << 24) + (UInt32)(b << 16) + (UInt32)(c << 8) + (UInt32)d;
				regKey.SetValue("IPAddress", ipAddr, Microsoft.Win32.RegistryValueKind.DWord);
				regKey.SetValue("CommandPort", cmdPort, Microsoft.Win32.RegistryValueKind.DWord);
				regKey.SetValue("DataPort", dataPort, Microsoft.Win32.RegistryValueKind.DWord);
				Close();
			}
			base.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
