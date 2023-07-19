namespace WG3000_COMM.ExtendFunc.Patrol
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPatrolReportFindOption : frmBioAccess
    {
        private int[] Event = new int[] { 4, 2, 3, 1 };

        public dfrmPatrolReportFindOption()
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
                (base.Owner as frmPatrolReport).btnQuery_Click(null, null);
            }
        }

        private void dfrmShiftAttReportFindOption_Load(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.Add(CommonStr.strPatrolEventAbsence, false);
            this.checkedListBox1.Items.Add(CommonStr.strPatrolEventEarly, false);
            this.checkedListBox1.Items.Add(CommonStr.strPatrolEventLate, false);
            this.checkedListBox1.Items.Add(CommonStr.strPatrolEventNormal, false);
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
            string str = " (1 > 0) ";
            if ((this.checkedListBox1.CheckedItems.Count != 0) && (this.checkedListBox1.CheckedItems.Count != this.checkedListBox1.Items.Count))
            {
                string str2 = "";
                for (int i = 0; i <= (this.checkedListBox1.CheckedItems.Count - 1); i++)
                {
                    if (str2 == "")
                    {
                        str2 = str2 + "  t_d_PatrolDetailData.f_EventDesc= " + this.Event[this.checkedListBox1.CheckedIndices[i]];
                    }
                    else
                    {
                        str2 = str2 + " OR  t_d_PatrolDetailData.f_EventDesc= " + this.Event[this.checkedListBox1.CheckedIndices[i]];
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    str = " (" + str2 + " )  ";
                }
            }
            return str;
        }
    }
}

