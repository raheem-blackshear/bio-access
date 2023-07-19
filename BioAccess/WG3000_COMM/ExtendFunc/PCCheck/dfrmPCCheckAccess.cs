namespace WG3000_COMM.ExtendFunc.PCCheck
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Media;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class dfrmPCCheckAccess : frmBioAccess
    {
        public bool bDealing;
        private icController contr4PCCheckAccess = new icController();
        private DataSet ds = new DataSet();
        private DataView dv;
        private string inputCard = "";
        private SoundPlayer player;
        public string strConsumername;
        public string strDoorFullName;
        public string strDoorId;
        public string strGroupname;
        public string strNow;
        private string wavfile;

        public dfrmPCCheckAccess()
        {
            this.InitializeComponent();
            this.player = new SoundPlayer();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            this.player.Stop();
            base.Hide();
            this.bDealing = false;
        }

        private void dfrmPCCheckAccess_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.bDealing = false;
        }

        private void dfrmPCCheckAccess_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (object obj2 in base.Controls)
            {
                try
                {
                    (obj2 as Control).ImeMode = ImeMode.Off;
                }
                catch (Exception)
                {
                }
            }
            if (((!e.Control && !e.Alt) && (!e.Shift && (e.KeyValue >= 0x30))) && (e.KeyValue <= 0x39))
            {
                if (this.inputCard.Length == 0)
                {
                    this.inputCardDate = DateTime.Now;
                    this.timer1.Interval = 500;
                    this.timer1.Enabled = true;
                }
                this.inputCard = this.inputCard + ((e.KeyValue - 0x30)).ToString();
            }
        }

        private void dfrmPCCheckAccess_Load(object sender, EventArgs e)
        {
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            try
            {
                base.ImeMode = ImeMode.Disable;
                this.btnCancel.ImeMode = ImeMode.Disable;
                foreach (object obj2 in base.Controls)
                {
                    try
                    {
                        (obj2 as Control).ImeMode = ImeMode.Off;
                    }
                    catch (Exception)
                    {
                    }
                }
                string cmdText = " SELECT a.f_ConsumerName, a.f_ConsumerID, a.f_CardNO from t_b_consumer a ,t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and b.f_GroupType=1 ";
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                            {
                                adapter.Fill(this.ds, "groups");
                            }
                        }
                        goto Label_0133;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                        {
                            adapter2.Fill(this.ds, "groups");
                        }
                    }
                }
            Label_0133:
                this.dv = new DataView(this.ds.Tables["groups"]);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void dfrmPCCheckAccess_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime time;
                if (!base.Visible || string.IsNullOrEmpty(this.strGroupname))
                {
                    return;
                }
                this.txtA0.Text = this.strGroupname;
                this.txtB0.Text = "";
                if (DateTime.TryParse(this.strNow, out time))
                {
                    this.txtB0.Text = time.ToString("HH:mm:ss");
                }
                this.txtC0.Text = this.strDoorFullName;
                this.txtConsumers.Text = this.strConsumername;
                string cmdText = " SELECT a.f_GroupID,a.f_GroupName,b.f_GroupType,b.f_CheckAccessActive,b.f_MoreCards, b.f_SoundFileName   from t_b_Group a ,t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and a.f_GroupName =" + wgTools.PrepareStr(this.strGroupname);
                if (wgAppConfig.IsAccessDB)
                {
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                        {
                            connection.Open();
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                this.wavfile = reader["f_SoundFileName"].ToString();
                            }
                            reader.Close();
                        }
                        goto Label_016A;
                    }
                }
                using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                    {
                        connection2.Open();
                        SqlDataReader reader2 = command2.ExecuteReader();
                        if (reader2.Read())
                        {
                            this.wavfile = reader2["f_SoundFileName"].ToString();
                        }
                        reader2.Close();
                    }
                }
            Label_016A:
                if (this.wavfile == "")
                {
                    this.wavfile = "DoorBell.wav";
                }
                else if (wgAppConfig.FileIsExisted(wgAppConfig.Path4PhotoDefault() + this.wavfile))
                {
                    this.player.SoundLocation = wgAppConfig.Path4PhotoDefault() + this.wavfile;
                    this.player.PlayLooping();
                }
                this.strGroupname = "";
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.contr4PCCheckAccess != null))
            {
                this.contr4PCCheckAccess.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                if (this.inputCard.Length >= 8)
                {
                    SystemSounds.Beep.Play();
                    this.dv.RowFilter = " f_CardNO = " + this.inputCard.ToString();
                    if (this.dv.Count > 0)
                    {
                        this.contr4PCCheckAccess.GetInfoFromDBByDoorName(this.strDoorFullName);
                        if (this.contr4PCCheckAccess.RemoteOpenDoorIP(this.strDoorFullName, (uint) icOperator.OperatorID, ulong.Parse(this.inputCard)) > 0)
                        {
                            InfoRow newInfo = new InfoRow();
                            newInfo.desc = string.Format("{0}[{1:d}]", this.strDoorFullName, this.contr4PCCheckAccess.ControllerSN);
                            newInfo.information = string.Format("{0} {1}--[{2}]", this.dv[0]["f_ConsumerName"].ToString(), CommonStr.strSendRemoteOpenDoor, this.strConsumername.Replace("\r\n", ","));
                            wgRunInfoLog.addEvent(newInfo);
                        }
                        this.strGroupname = "";
                        base.Hide();
                        this.player.Stop();
                        this.bDealing = false;
                    }
                    else
                    {
                        SystemSounds.Beep.Play();
                    }
                }
            }
            catch (Exception)
            {
            }
            this.inputCard = "";
        }
    }
}

