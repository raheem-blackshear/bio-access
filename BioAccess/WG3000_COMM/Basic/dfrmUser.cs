using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
    public partial class dfrmUser : frmBioAccess
    {
        private ArrayList arrGroupID = new ArrayList();
        private ArrayList arrGroupName = new ArrayList();
        private ArrayList arrGroupNO = new ArrayList();
        private List<MjFpTempl> fp_templ = new List<MjFpTempl>();
        private MjFaceTempl face_templ = null;
        private bool bContinued;
        private int m_consumerID;
        private string m_curGroup = "";
        private bool m_OperateNew = true;
        private string photoFileName = "";
        private string strStartCaption = "";
        private string strUserAutoAddSet;
        private int userIDlen;

        public dfrmUser()
        {
            InitializeComponent();
        }

        private int AddUser()
        {
            icConsumer consumer = new icConsumer();
            int num = consumer.addNew(
                this.txtf_ConsumerNO.Text,
                this.txtf_ConsumerName.Text,
                int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString()),
                this.chkAttendance.Checked ? ((byte)1) : ((byte)0),
                this.optNormal.Checked ? ((byte)0) : ((byte)1),
                this.chkDoorEnabled.Checked ? ((byte)1) : ((byte)0),
                this.dtpActivate.Value,
                this.dtpDeactivate.Value, 
                this.txtf_PIN.Text.Trim(),
                (this.txtf_CardNO.Text == "") ? 0L : long.Parse(this.txtf_CardNO.Text),
                fp_templ,
                face_templ);
            if (num >= 0)
            {
                consumer.editUserOtherInfo(
                    consumer.gConsumerID,
                    this.txtf_Title.Text,
                    this.txtf_Culture.Text,
                    this.txtf_Hometown.Text,
                    this.txtf_Birthday.Text,
                    this.txtf_Marriage.Text,
                    this.txtf_JoinDate.Text,
                    this.txtf_LeaveDate.Text,
                    this.txtf_CertificateType.Text,
                    this.txtf_CertificateID.Text,
                    this.txtf_SocialInsuranceNo.Text,
                    this.txtf_Addr.Text,
                    this.txtf_Postcode.Text,
                    this.txtf_Sex.Text,
                    this.txtf_Nationality.Text,
                    this.txtf_Religion.Text,
                    this.txtf_EnglishName.Text,
                    this.txtf_Mobile.Text,
                    this.txtf_HomePhone.Text,
                    this.txtf_Telephone.Text,
                    this.txtf_Email.Text,
                    this.txtf_Political.Text,
                    this.txtf_CorporationName.Text,
                    this.txtf_TechGrade.Text,
                    this.txtf_Note.Text);
                wgAppConfig.wgLog(string.Format("{0}:{1} [{2}]", CommonStr.strAddUsers, this.txtf_ConsumerName.Text, this.txtf_CardNO.Text), EventLogEntryType.Information, null);
            }
            consumer = null;
            return num;
        }

        private void btnAddNext_Click(object sender, EventArgs e)
        {
            int errNO = this.AddUser();
            if (errNO < 0)
            {
                XMessageBox.Show(this, icConsumer.getErrInfo(errNO), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.savePhoto();
                icConsumerShare.setUpdateLog();
                long result = 0L;
                long.TryParse(this.txtf_ConsumerNO.Text, out result);
                if (result > 0L)
                {
                    this.txtf_ConsumerNO.Text = (result + 1L).ToString();
                }
                else if (!string.IsNullOrEmpty(this.strStartCaption) && this.txtf_ConsumerNO.Text.StartsWith(this.strStartCaption))
                {
                    long num3;
                    if (long.TryParse(this.txtf_ConsumerNO.Text.Substring(this.txtf_ConsumerNO.Text.IndexOf(this.strStartCaption) + this.strStartCaption.Length), out num3))
                    {
                        string str2 = "";
                        num3 += 1L;
                        if (string.IsNullOrEmpty(this.strStartCaption))
                        {
                            str2 = num3.ToString();
                        }
                        else if ((this.userIDlen - this.strStartCaption.Length) > 0)
                        {
                            str2 = string.Format("{0}{1}", this.strStartCaption, num3.ToString().PadLeft(this.userIDlen - this.strStartCaption.Length, '0'));
                        }
                        else
                        {
                            str2 = string.Format("{0}{1}", this.strStartCaption, num3.ToString());
                        }
                        this.txtf_ConsumerNO.Text = str2;
                    }
                    else
                    {
                        this.txtf_ConsumerNO.Text = "";
                    }
                }
                else
                {
                    this.txtf_ConsumerNO.Text = "";
                }
                this.txtf_ConsumerName.Text = "";
                this.txtf_CardNO.Text = "";
                this.txtf_CardNO.Focus();
                fp_templ.Clear();
                lblRegFpCount.Text = CommonStr.strFpNone;
                lblRegFaceCount.Text = CommonStr.strFaceNone;
                this.bContinued = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.bContinued)
            {
                base.DialogResult = DialogResult.OK;
            }
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int num;
                if (this.m_OperateNew)
                {
                    num = this.AddUser();
                }
                else
                {
                    num = this.EditUser();
                }
                if (num < 0)
                {
                    XMessageBox.Show(this, icConsumer.getErrInfo(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.savePhoto();
                    icConsumerShare.setUpdateLog();
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    this.photoFileName = "";
                }
                catch (Exception exception)
                {
                    wgTools.WriteLine(exception.ToString());
                }
                this.openFileDialog1.Filter = " (*.jpg)|*.jpg|(*.bmp)|*.bmp";
                this.openFileDialog1.FilterIndex = 1;
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    this.photoFileName = this.openFileDialog1.FileName;
                    Image img = this.pictureBox1.Image;
                    wgAppConfig.ShowMyImage(this.photoFileName, ref img);
                    this.pictureBox1.Image = img;
                }
            }
            catch (Exception exception2)
            {
                wgTools.WriteLine(exception2.ToString());
                XMessageBox.Show(exception2.ToString());
            }
            Directory.SetCurrentDirectory(Application.StartupPath);
        }

        private void chkAttendance_CheckedChanged(object sender, EventArgs e)
        {
            if (wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.grpbAttendance.Visible = this.chkAttendance.Checked;
            }
        }

        private void chkDoorEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.grpbAccessControl.Visible = this.chkDoorEnabled.Checked;
        }

        private void dfrmUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            wgAppConfig.DisposeImage(this.pictureBox1.Image);
        }

        private void dfrmUser_Load(object sender, EventArgs e)
        {
            this.txtf_ConsumerNO.Mask = "99999999";
            this.txtf_CardNO.Mask = "9999999999";
            this.txtf_PIN.Mask = "999999";
            this.txtf_PIN.Text = "";
            this.dtpActivate.Value = DateTime.Now.Date;
            this.tabPage1.BackColor = this.BackColor;
            this.tabPage2.BackColor = this.BackColor;
            this.label4.Text = wgAppConfig.ReplaceFloorRomm(this.label4.Text);
            this.label1.Text = wgAppConfig.ReplaceWorkNO(this.label1.Text);
            new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
            int count = this.arrGroupID.Count;
            for (count = 0; count < this.arrGroupID.Count; count++)
            {
                this.cbof_GroupID.Items.Add(this.arrGroupName[count].ToString());
            }
            if (this.cbof_GroupID.Items.Count > 0)
            {
                this.cbof_GroupID.SelectedIndex = 0;
            }
            this.loadUserAutoAdd();
            if (this.m_OperateNew)
            {
                this.loadData4New();
                this.btnAddNext.Visible = true;
            }
            else
            {
                this.loadData4Edit();
                this.btnAddNext.Visible = false;
            }
            this.chkAttendance_CheckedChanged(null, null);
            this.chkDoorEnabled_CheckedChanged(null, null);
            if (chkAttendance.Checked && wgAppConfig.getParamValBoolByNO(0x71))
            {
                this.grpbAttendance.Visible = true;
            }
            else
            {
                this.optNormal.Checked = true;
                this.grpbAttendance.Visible = false;
            }
            //this.label7.Visible = wgAppConfig.getParamValBoolByNO(0x7b);
            //this.txtf_PIN.Visible = wgAppConfig.getParamValBoolByNO(0x7b);
            wgAppConfig.setDisplayFormatDate(this.dtpActivate, wgTools.DisplayFormat_DateYMD);
            wgAppConfig.setDisplayFormatDate(this.dtpDeactivate, wgTools.DisplayFormat_DateYMD);
            lblRegFpCount.Text = (getRegFpCount() == 0) ? CommonStr.strFpNone : 
                getRegFpCount().ToString() + " " + CommonStr.strFpUnit;
            lblRegFaceCount.Text = (getRegFaceCount() == 0) ? CommonStr.strFaceNone : CommonStr.strFaceRegistered;
        }

        private int getRegFpCount()
        {
            int fpCount = 0;
            if (wgAppConfig.IsAccessDB)
            {
                OleDbConnection connection = null;
                OleDbCommand command = null;
                try
                {
                    connection = new OleDbConnection(wgAppConfig.dbConString);
                    command = new OleDbCommand("", connection);
                    try
                    {
                        command.CommandText = command.CommandText = " SELECT COUNT(*) AS f_FpCount FROM t_d_FpTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                        command.Connection = connection;
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                            fpCount = (int)reader["f_FpCount"];
                        reader.Close();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                    finally
                    {
                        if (command != null)
                        {
                            command.Dispose();
                        }
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = new SqlConnection(wgAppConfig.dbConString);
                    command = new SqlCommand("", connection);
                    try
                    {
                        command.CommandText = " SELECT COUNT(*) AS f_FpCount FROM t_d_FpTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                            fpCount = (int)reader["f_FpCount"];
                        reader.Close();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return fpCount;
        }

        private int getRegFaceCount()
        {
            int faceCount = 0;
            if (wgAppConfig.IsAccessDB)
            {
                OleDbConnection connection = null;
                OleDbCommand command = null;
                try
                {
                    connection = new OleDbConnection(wgAppConfig.dbConString);
                    command = new OleDbCommand("", connection);
                    try
                    {
                        command.CommandText = command.CommandText = " SELECT f_ConsumerID FROM t_d_FaceTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                        command.Connection = connection;
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                            faceCount = 1;
                        reader.Close();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                    finally
                    {
                        if (command != null)
                        {
                            command.Dispose();
                        }
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = new SqlConnection(wgAppConfig.dbConString);
                    command = new SqlCommand("", connection);
                    try
                    {
                        command.CommandText = " SELECT f_ConsumerID FROM t_d_FaceTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                            faceCount = 1;
                        reader.Close();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return faceCount;
        }

        private int EditUser()
        {
            icConsumer consumer = new icConsumer();
            int num = consumer.editUser(
                this.m_consumerID,
                this.txtf_ConsumerNO.Text,
                this.txtf_ConsumerName.Text, 
                int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString()),
                this.chkAttendance.Checked ? ((byte)1) : ((byte)0),
                this.optNormal.Checked ? ((byte)0) : ((byte)1),
                this.chkDoorEnabled.Checked ? ((byte)1) : ((byte)0),
                this.dtpActivate.Value, 
                this.dtpDeactivate.Value, 
                this.txtf_PIN.Text.Trim(), 
                (this.txtf_CardNO.Text == "") ? 0L : long.Parse(this.txtf_CardNO.Text),
                fp_templ,
                face_templ);
            if (num >= 0)
            {
                consumer.editUserOtherInfo(
                    this.m_consumerID,
                    this.txtf_Title.Text,
                    this.txtf_Culture.Text,
                    this.txtf_Hometown.Text,
                    this.txtf_Birthday.Text,
                    this.txtf_Marriage.Text,
                    this.txtf_JoinDate.Text,
                    this.txtf_LeaveDate.Text,
                    this.txtf_CertificateType.Text,
                    this.txtf_CertificateID.Text,
                    this.txtf_SocialInsuranceNo.Text,
                    this.txtf_Addr.Text,
                    this.txtf_Postcode.Text,
                    this.txtf_Sex.Text,
                    this.txtf_Nationality.Text,
                    this.txtf_Religion.Text,
                    this.txtf_EnglishName.Text,
                    this.txtf_Mobile.Text,
                    this.txtf_HomePhone.Text,
                    this.txtf_Telephone.Text,
                    this.txtf_Email.Text,
                    this.txtf_Political.Text,
                    this.txtf_CorporationName.Text,
                    this.txtf_TechGrade.Text,
                    this.txtf_Note.Text);
            }
            consumer = null;
            return num;
        }

        private void loadData4Edit()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadData4Edit_Acc();
            }
            else
            {
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = new SqlConnection(wgAppConfig.dbConString);
                    command = new SqlCommand("", connection);
                    try
                    {
                        string str = " SELECT  t_b_Consumer.*,  f_GroupName ";
                        str = (str + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ") + " where [f_ConsumerID]= " + this.m_consumerID.ToString();
                        command.CommandText = str;
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                        {
                            this.chkDoorEnabled.Checked = ((byte) reader["f_DoorEnabled"]) > 0;
                            this.chkAttendance.Checked = ((byte) reader["f_AttendEnabled"]) > 0;
                            this.txtf_ConsumerNO.Text = wgTools.SetObjToStr(reader["f_ConsumerNO"]);
                            this.txtf_ConsumerName.Text = wgTools.SetObjToStr(reader["f_ConsumerName"]);
                            this.txtf_CardNO.Text = wgTools.SetObjToStr(reader["f_CardNO"]);
                            if (this.txtf_CardNO.Text != "")
                            {
                                //this.txtf_CardNO.ReadOnly = true;
                                this.txtf_CardNO.Cursor = Cursors.Arrow;
                            }
                            this.txtf_PIN.Text = wgTools.SetObjToStr(reader["f_PIN"]);
                            this.dtpActivate.Value = (DateTime) reader["f_BeginYMD"];
                            this.dtpDeactivate.Value = (DateTime) reader["f_EndYMD"];
                            this.m_curGroup = wgTools.SetObjToStr(reader["f_GroupName"]);
                            this.cbof_GroupID.Text = this.m_curGroup;
                            this.optNormal.Checked = true;
                            this.optShift.Checked = ((byte) reader["f_ShiftEnabled"]) > 0;
                        }
                        reader.Close();

                        // Load fingerprint templates
                        command.CommandText = " SELECT f_Finger, f_Templ, f_Duress FROM t_d_FpTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                        reader = command.ExecuteReader(CommandBehavior.Default);
                        while (reader.Read())
                        {
                            MjFpTempl e = new MjFpTempl(
                                m_consumerID,
                                (int)reader["f_Finger"],
                                (byte[])reader["f_Templ"],
                                (int)reader["f_Duress"] > 0);
                            fp_templ.Add(e);
                        }
                        reader.Close();

                        // Load face template
                        command.CommandText = " SELECT f_Templ FROM t_d_FaceTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                        reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                        {
                            face_templ = new MjFaceTempl(
                                m_consumerID,
                                (byte[])reader["f_Templ"]);
                        }
                        reader.Close();

                        str = " SELECT  * ";
                        str = (str + " FROM t_b_Consumer_Other  ") + " where [f_ConsumerID]= " + this.m_consumerID.ToString();
                        command.CommandText = str;
                        reader = command.ExecuteReader(CommandBehavior.Default);
                        if (reader.Read())
                        {
                            this.txtf_Title.Text = wgTools.SetObjToStr(reader["f_Title"]);
                            this.txtf_Culture.Text = wgTools.SetObjToStr(reader["f_Culture"]);
                            this.txtf_Hometown.Text = wgTools.SetObjToStr(reader["f_Hometown"]);
                            this.txtf_Birthday.Text = wgTools.SetObjToStr(reader["f_Birthday"]);
                            this.txtf_Marriage.Text = wgTools.SetObjToStr(reader["f_Marriage"]);
                            this.txtf_JoinDate.Text = wgTools.SetObjToStr(reader["f_JoinDate"]);
                            this.txtf_LeaveDate.Text = wgTools.SetObjToStr(reader["f_LeaveDate"]);
                            this.txtf_CertificateType.Text = wgTools.SetObjToStr(reader["f_CertificateType"]);
                            this.txtf_CertificateID.Text = wgTools.SetObjToStr(reader["f_CertificateID"]);
                            this.txtf_SocialInsuranceNo.Text = wgTools.SetObjToStr(reader["f_SocialInsuranceNo"]);
                            this.txtf_Addr.Text = wgTools.SetObjToStr(reader["f_Addr"]);
                            this.txtf_Postcode.Text = wgTools.SetObjToStr(reader["f_Postcode"]);
                            this.txtf_Sex.Text = wgTools.SetObjToStr(reader["f_Sex"]);
                            this.txtf_Nationality.Text = wgTools.SetObjToStr(reader["f_Nationality"]);
                            this.txtf_Religion.Text = wgTools.SetObjToStr(reader["f_Religion"]);
                            this.txtf_EnglishName.Text = wgTools.SetObjToStr(reader["f_EnglishName"]);
                            this.txtf_Mobile.Text = wgTools.SetObjToStr(reader["f_Mobile"]);
                            this.txtf_HomePhone.Text = wgTools.SetObjToStr(reader["f_HomePhone"]);
                            this.txtf_Telephone.Text = wgTools.SetObjToStr(reader["f_Telephone"]);
                            this.txtf_Email.Text = wgTools.SetObjToStr(reader["f_Email"]);
                            this.txtf_Political.Text = wgTools.SetObjToStr(reader["f_Political"]);
                            this.txtf_CorporationName.Text = wgTools.SetObjToStr(reader["f_CorporationName"]);
                            this.txtf_TechGrade.Text = wgTools.SetObjToStr(reader["f_TechGrade"]);
                            this.txtf_Note.Text = wgTools.SetObjToStr(reader["f_Note"]);
                        }
                        reader.Close();
                        this.loadPhoto();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                    finally
                    {
                        if (command != null)
                        {
                            command.Dispose();
                        }
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void loadData4Edit_Acc()
        {
            OleDbConnection connection = null;
            OleDbCommand command = null;
            try
            {
                connection = new OleDbConnection(wgAppConfig.dbConString);
                command = new OleDbCommand("", connection);
                try
                {
                    string str = " SELECT  t_b_Consumer.*,  f_GroupName ";
                    str = (str + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID ") + " where [f_ConsumerID]= " + this.m_consumerID.ToString();
                    command.CommandText = str;
                    command.Connection = connection;
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                    if (reader.Read())
                    {
                        this.chkDoorEnabled.Checked = ((byte) reader["f_DoorEnabled"]) > 0;
                        this.chkAttendance.Checked = ((byte)reader["f_AttendEnabled"]) > 0;
                        this.txtf_ConsumerNO.Text = wgTools.SetObjToStr(reader["f_ConsumerNO"]);
                        this.txtf_ConsumerName.Text = wgTools.SetObjToStr(reader["f_ConsumerName"]);
                        this.txtf_CardNO.Text = wgTools.SetObjToStr(reader["f_CardNO"]);
                        if (this.txtf_CardNO.Text != "")
                        {
                            //this.txtf_CardNO.ReadOnly = true;
                            this.txtf_CardNO.Cursor = Cursors.Arrow;
                        }
                        this.txtf_PIN.Text = wgTools.SetObjToStr(reader["f_PIN"]);
                        this.dtpActivate.Value = (DateTime) reader["f_BeginYMD"];
                        this.dtpDeactivate.Value = (DateTime) reader["f_EndYMD"];
                        this.m_curGroup = wgTools.SetObjToStr(reader["f_GroupName"]);
                        this.cbof_GroupID.Text = this.m_curGroup;
                        this.optNormal.Checked = true;
                        this.optShift.Checked = ((byte) reader["f_ShiftEnabled"]) > 0;
                    }
                    reader.Close();

                    // Load fingerprint templates
                    command.CommandText = " SELECT f_Finger, f_Templ, f_Duress FROM t_d_FpTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                    reader = command.ExecuteReader(CommandBehavior.Default);
                    while (reader.Read())
                    {
                        MjFpTempl e = new MjFpTempl(
                            m_consumerID,
                            (int)reader["f_Finger"],
                            (byte[])reader["f_Templ"],
                            (int)reader["f_Duress"] > 0);
                        fp_templ.Add(e);
                    }
                    reader.Close();

                    // Load face template
                    command.CommandText = " SELECT f_Templ FROM t_d_FaceTempl WHERE f_ConsumerID = " + m_consumerID.ToString();
                    reader = command.ExecuteReader(CommandBehavior.Default);
                    if (reader.Read())
                    {
                        face_templ = new MjFaceTempl(
                            m_consumerID,
                            (byte[])reader["f_Templ"]);
                    }
                    reader.Close();

                    str = " SELECT  * ";
                    str = (str + " FROM t_b_Consumer_Other  ") + " where [f_ConsumerID]= " + this.m_consumerID.ToString();
                    command.CommandText = str;
                    reader = command.ExecuteReader(CommandBehavior.Default);
                    if (reader.Read())
                    {
                        this.txtf_Title.Text = wgTools.SetObjToStr(reader["f_Title"]);
                        this.txtf_Culture.Text = wgTools.SetObjToStr(reader["f_Culture"]);
                        this.txtf_Hometown.Text = wgTools.SetObjToStr(reader["f_Hometown"]);
                        this.txtf_Birthday.Text = wgTools.SetObjToStr(reader["f_Birthday"]);
                        this.txtf_Marriage.Text = wgTools.SetObjToStr(reader["f_Marriage"]);
                        this.txtf_JoinDate.Text = wgTools.SetObjToStr(reader["f_JoinDate"]);
                        this.txtf_LeaveDate.Text = wgTools.SetObjToStr(reader["f_LeaveDate"]);
                        this.txtf_CertificateType.Text = wgTools.SetObjToStr(reader["f_CertificateType"]);
                        this.txtf_CertificateID.Text = wgTools.SetObjToStr(reader["f_CertificateID"]);
                        this.txtf_SocialInsuranceNo.Text = wgTools.SetObjToStr(reader["f_SocialInsuranceNo"]);
                        this.txtf_Addr.Text = wgTools.SetObjToStr(reader["f_Addr"]);
                        this.txtf_Postcode.Text = wgTools.SetObjToStr(reader["f_Postcode"]);
                        this.txtf_Sex.Text = wgTools.SetObjToStr(reader["f_Sex"]);
                        this.txtf_Nationality.Text = wgTools.SetObjToStr(reader["f_Nationality"]);
                        this.txtf_Religion.Text = wgTools.SetObjToStr(reader["f_Religion"]);
                        this.txtf_EnglishName.Text = wgTools.SetObjToStr(reader["f_EnglishName"]);
                        this.txtf_Mobile.Text = wgTools.SetObjToStr(reader["f_Mobile"]);
                        this.txtf_HomePhone.Text = wgTools.SetObjToStr(reader["f_HomePhone"]);
                        this.txtf_Telephone.Text = wgTools.SetObjToStr(reader["f_Telephone"]);
                        this.txtf_Email.Text = wgTools.SetObjToStr(reader["f_Email"]);
                        this.txtf_Political.Text = wgTools.SetObjToStr(reader["f_Political"]);
                        this.txtf_CorporationName.Text = wgTools.SetObjToStr(reader["f_CorporationName"]);
                        this.txtf_TechGrade.Text = wgTools.SetObjToStr(reader["f_TechGrade"]);
                        this.txtf_Note.Text = wgTools.SetObjToStr(reader["f_Note"]);
                    }
                    reader.Close();
                    this.loadPhoto();
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void loadData4New()
        {
            try
            {
                long num = new icConsumer().ConsumerNONext(this.strStartCaption);
                if (num < 0L)
                {
                    num = 1L;
                }
                string str = "";
                if (string.IsNullOrEmpty(this.strStartCaption))
                {
                    str = num.ToString();
                }
                else if ((this.userIDlen - this.strStartCaption.Length) > 0)
                {
                    str = string.Format("{0}{1}", this.strStartCaption, num.ToString().PadLeft(this.userIDlen - this.strStartCaption.Length, '0'));
                }
                else
                {
                    str = string.Format("{0}{1}", this.strStartCaption, num.ToString());
                }
                this.txtf_ConsumerNO.Text = str;
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void loadPhoto()
        {
            string fileToDisplay = "";
            try
            {
                if (this.txtf_CardNO.Text.Trim() == "")
                {
                    fileToDisplay = null;
                }
                else
                {
                    fileToDisplay = wgAppConfig.getPhotoFileName(long.Parse(this.txtf_CardNO.Text));
                }
                Image img = this.pictureBox1.Image;
                wgAppConfig.ShowMyImage(fileToDisplay, ref img);
                this.pictureBox1.Image = img;
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void loadUserAutoAdd()
        {
            try
            {
                this.strUserAutoAddSet = wgAppConfig.GetKeyVal("UserAutoAddSet");
                if (!string.IsNullOrEmpty(this.strUserAutoAddSet) && (this.strUserAutoAddSet.IndexOf(",") > 0))
                {
                    string s = this.strUserAutoAddSet.Substring(0, this.strUserAutoAddSet.IndexOf(","));
                    string valA = this.strUserAutoAddSet.Substring(this.strUserAutoAddSet.IndexOf(",") + 1);
                    if (int.Parse(s) > 0)
                    {
                        this.userIDlen = int.Parse(s);
                    }
                    this.strStartCaption = wgTools.SetObjToStr(valA);
                }
            }
            catch (Exception)
            {
            }
        }

        private void savePhoto()
        {
            if ((this.photoFileName != "") && !string.IsNullOrEmpty(this.txtf_CardNO.Text))
            {
                try
                {
                    wgAppConfig.photoDirectoryLastWriteTime = DateTime.Parse("2012-6-12 18:57:08.531");
                    if (wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo()))
                    {
                        FileInfo info = new FileInfo(this.photoFileName);
                        FileInfo info2 = new FileInfo(wgAppConfig.Path4Photo() + this.txtf_CardNO.Text + info.Extension);
                        if (info2.FullName.ToUpper() != this.photoFileName.ToUpper())
                        {
                            try
                            {
                                if (info2.Exists)
                                {
                                    info2.Delete();
                                }
                            }
                            catch (Exception exception)
                            {
                                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                            }
                            info.CopyTo(wgAppConfig.Path4Photo() + this.txtf_CardNO.Text + ".jpg", true);
                        }
                        this.photoFileName = "";
                        this.pictureBox1.Image = null;
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                if (this.txtf_CardNO.Text.Length >= 8)
                {
                    this.cbof_GroupID.Focus();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void txtf_CardNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtf_CardNO);
        }

        private void txtf_CardNO_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtf_CardNO);
        }

        private void txtf_CardNO_TextChanged(object sender, EventArgs e)
        {
            if (this.txtf_CardNO.Text.Length == 1)
            {
                this.timer1.Interval = 500;
                this.timer1.Enabled = true;
            }
/*
            if (this.txtf_CardNO.Text.Length == 0)
            {
                this.btnSelectPhoto.Enabled = false;
            }
            else
            {
                this.btnSelectPhoto.Enabled = true;
            }
*/
        }

        public int consumerID
        {
            get
            {
                return this.m_consumerID;
            }
            set
            {
                this.m_consumerID = value;
            }
        }

        public string curGroup
        {
            get
            {
                return this.m_curGroup;
            }
            set
            {
                this.m_curGroup = value;
            }
        }

        public bool OperateNew
        {
            get
            {
                return this.m_OperateNew;
            }
            set
            {
                this.m_OperateNew = value;
            }
        }

        private void btnRegFp_Click(object sender, EventArgs e)
        {
            using (dfrmRegisterFingerprint dlg = new dfrmRegisterFingerprint(fp_templ))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Get fingerprint templates
                    fp_templ.Clear();
                    foreach (MjFpTempl t in dlg.fp_templ)
                        fp_templ.Add(new MjFpTempl(t));
                }
            }
            if (fp_templ.Count == 0)
                lblRegFpCount.Text = CommonStr.strFpNone;
            else
                lblRegFpCount.Text = fp_templ.Count + " " + CommonStr.strFpUnit;
        }

        private void txtf_ConsumerNO_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtf_ConsumerNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtf_ConsumerNO);
        }

        private void txtf_ConsumerNO_KeyUp(object sender, KeyEventArgs e)
        {
            wgAppConfig.CardIDInput(ref this.txtf_ConsumerNO);
        }

        private void cmdReadFromReader_Click(object sender, EventArgs e)
        {
            using (dfrmReadCardID dlg = new dfrmReadCardID())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtf_CardNO.Text = dlg.getCardID();
            }
        }

        private void btnRegFace_Click(object sender, EventArgs e)
        {
			int uid = Convert.ToInt32(txtf_ConsumerNO.Text);
			using (dfrmRegisterFace dlg = new dfrmRegisterFace(face_templ, uid))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Get face templates
                    face_templ = dlg.face_templ;
                }
            }
            if (face_templ == null)
                lblRegFaceCount.Text = CommonStr.strFaceNone;
            else
                lblRegFaceCount.Text = CommonStr.strFaceRegistered;
        }
    }
}
