namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmShiftAttReportFindOption : frmBioAccess
    {
        public dfrmShiftAttReportFindOption()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Hide();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (base.Owner != null)
            {
                (base.Owner as frmShiftAttReport).btnQuery_Click(null, null);
            }
        }

        private void dfrmShiftAttReportFindOption_Load(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.Add(CommonStr.strLateness, false);
            this.checkedListBox1.Items.Add(CommonStr.strLeaveEarly, false);
            this.checkedListBox1.Items.Add(CommonStr.strAbsence, false);
            this.checkedListBox1.Items.Add(CommonStr.strSignIn, false);
            this.checkedListBox1.Items.Add(CommonStr.strNotReadCard, false);
            this.checkedListBox1.Items.Add(CommonStr.strOvertime, false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string getStrSql()
        {
            string str = " (1 < 0) ";
            if (this.checkedListBox1.CheckedItems.Count == 0)
            {
                return str;
            }
            string str2 = "";
            for (int i = 0; i <= (this.checkedListBox1.CheckedItems.Count - 1); i++)
            {
                if (str2 == "")
                {
                    str2 = str2 + " t_d_shift_AttReport.[f_OnDuty1AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i]);
                }
                else
                {
                    str2 = str2 + " OR t_d_shift_AttReport.[f_OnDuty1AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i]);
                }
                str2 = ((((((((((((((str2 + " OR t_d_shift_AttReport.[f_OnDuty1CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty1AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty1CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OnDuty2AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OnDuty2CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty2AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty2CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OnDuty3AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OnDuty3CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty3AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty3CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OnDuty4AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OnDuty4CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty4AttDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i])) + " OR t_d_shift_AttReport.[f_OffDuty4CardRecordDesc]= " + wgTools.PrepareStr(this.checkedListBox1.CheckedItems[i]);
            }
            return (" (" + str2 + " )  ");
        }
    }
}

