using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
    public partial class dfrmRecordDetail : frmBioAccess
    {
        public dfrmRecordDetail()
        {
            InitializeComponent();
        }

        public dfrmRecordDetail(string recID, string userID, string userName,
            string depart, string recDate, string addr, string pass, byte[] photo)
        {
            InitializeComponent();

            txtRecID.Text = recID;
            txtUserID.Text = userID;
            txtUserName.Text = userName;
            txtDepartment.Text = depart;
            txtDateTime.Text = recDate;
            txtAddress.Text = addr;
            txtPassage.Text = (pass == "1") ? CommonStr.strPassAllowed : CommonStr.strPassDisallowed;
            Image image = picPhoto.Image;
            wgAppConfig.ShowImageStream(photo, ref image);
            if (image != null)
                picPhoto.Image = image;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}
