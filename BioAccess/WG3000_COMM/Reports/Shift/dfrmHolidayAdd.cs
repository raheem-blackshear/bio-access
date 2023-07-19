namespace WG3000_COMM.Reports.Shift
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmHolidayAdd : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        public string holidayType = "2";

        public dfrmHolidayAdd()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.txtHolidayName.Text = this.txtHolidayName.Text.Trim();
            if (this.txtHolidayName.Text == "")
            {
                XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    wgAppConfig.runUpdateSql(((((((("INSERT INTO [t_a_Holiday] ([f_Name], [f_Value], [f_Value1], [f_Value2],[f_Value3], [f_Type], [f_Note])" + " Values( " + wgTools.PrepareStr(this.txtHolidayName.Text)) + ", " + wgTools.PrepareStr(this.dtpStartDate.Value.ToString("yyyy-MM-dd"))) + ", " + wgTools.PrepareStr(this.cboStart.Text)) + ", " + wgTools.PrepareStr(this.dtpEndDate.Value.ToString("yyyy-MM-dd"))) + ", " + wgTools.PrepareStr(this.cboEnd.Text)) + ", " + wgTools.PrepareStr(this.holidayType)) + ", " + wgTools.PrepareStr(this.txtf_Notes.Text)) + " )");
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        private void dfrmLeave_Load(object sender, EventArgs e)
        {
            try
            {
                this.dtpStartDate.Value = DateTime.Now.Date;
                this.dtpEndDate.Value = DateTime.Now.Date;
                this.cboStart.Items.Clear();
                this.cboStart.Items.AddRange(new string[] { CommonStr.strAM, CommonStr.strPM });
                this.cboEnd.Items.Clear();
                this.cboEnd.Items.AddRange(new string[] { CommonStr.strAM, CommonStr.strPM });
                if (this.cboStart.Items.Count > 0)
                {
                    this.cboStart.SelectedIndex = 0;
                }
                if (this.cboEnd.Items.Count > 1)
                {
                    this.cboEnd.SelectedIndex = 1;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            wgAppConfig.setDisplayFormatDate(this.dtpStartDate, wgTools.DisplayFormat_DateYMDWeek);
            wgAppConfig.setDisplayFormatDate(this.dtpEndDate, wgTools.DisplayFormat_DateYMDWeek);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dtpStartDate.Value > this.dtpEndDate.Value)
                {
                    this.cboEnd.Text = CommonStr.strPM;
                }
                this.dtpEndDate.MinDate = this.dtpStartDate.Value;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }
    }
}

