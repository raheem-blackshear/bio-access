namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;

    public partial class dfrmControlHolidayAdd : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        public bool bHoliday = true;

        public dfrmControlHolidayAdd()
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
            try
            {
                wgAppConfig.runUpdateSql(((((("INSERT INTO [t_b_ControlHolidays] ([f_BeginYMDHMS], [f_EndYMDHMS], [f_Notes], [f_forceWork])" + " Values( ") + " " + wgTools.PrepareStr(DateTime.Parse(this.dtpStartDate.Value.ToString("yyyy-MM-dd ") + this.dateBeginHMS1.Value.ToString("HH:mm")), true, wgTools.YMDHMSFormat)) + ", " + wgTools.PrepareStr(DateTime.Parse(this.dtpEndDate.Value.ToString("yyyy-MM-dd ") + this.dateEndHMS1.Value.ToString("HH:mm:59")), true, wgTools.YMDHMSFormat)) + ", " + wgTools.PrepareStr(this.txtf_Notes.Text)) + ", " + (this.bHoliday ? "0" : "1")) + " )");
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void dfrmLeave_Load(object sender, EventArgs e)
        {
            try
            {
                this.dtpStartDate.Value = DateTime.Now.Date;
                this.dtpEndDate.Value = DateTime.Now.Date;
                this.dateBeginHMS1.CustomFormat = "HH:mm";
                this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
                this.dateBeginHMS1.Value = DateTime.Parse("00:00:00");
                this.dateEndHMS1.CustomFormat = "HH:mm";
                this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
                this.dateEndHMS1.Value = DateTime.Parse("23:59:59");
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
                this.dtpEndDate.MinDate = this.dtpStartDate.Value;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }
    }
}

