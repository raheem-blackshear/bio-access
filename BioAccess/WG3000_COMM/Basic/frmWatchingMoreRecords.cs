namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;

    public partial class frmWatchingMoreRecords : frmBioAccess
    {
        public int groupMax = 3;
        public float InfoFontSize = 12f;
        private int lastCnt = -1;

        public frmWatchingMoreRecords()
        {
            this.InitializeComponent();
        }

        private void enlargeFontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.InfoFontSize++;
                RichTextBox[] boxArray = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
                for (int i = 0; i < 5; i++)
                {
                    boxArray[i].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, boxArray[i].Font.Unit);
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void enlargeInfoDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RichTextBox[] boxArray = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
                PictureBox[] boxArray2 = new PictureBox[] { this.pictureBox1, this.pictureBox2, this.pictureBox3, this.pictureBox4, this.pictureBox5 };
                for (int i = 0; i < 5; i++)
                {
                    if (boxArray2[i].Height > 0x1a)
                    {
                        boxArray[i].Size = new Size(boxArray[i].Width, boxArray[i].Height + 0x1a);
                        boxArray2[i].Location = new Point(boxArray2[i].Location.X, boxArray2[i].Location.Y + 0x1a);
                        boxArray2[i].Size = new Size(boxArray2[i].Width, boxArray2[i].Height - 0x1a);
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void frmWatchingMoreRecords_FormClosing(object sender, FormClosingEventArgs e)
        {
            wgAppConfig.DisposeImage(this.pictureBox1.Image);
            wgAppConfig.DisposeImage(this.pictureBox2.Image);
            wgAppConfig.DisposeImage(this.pictureBox3.Image);
            wgAppConfig.DisposeImage(this.pictureBox4.Image);
            wgAppConfig.DisposeImage(this.pictureBox5.Image);
        }

        private void frmWatchingMoreRecords_Load(object sender, EventArgs e)
        {
            if (this.tbRunInfoLog != null)
            {
                string keyVal = wgAppConfig.GetKeyVal("WatchingMoreRecords_Display");
                if (!string.IsNullOrEmpty(keyVal))
                {
                    try
                    {
                        string[] strArray = keyVal.Split(new char[] { ',' });
                        RichTextBox[] boxArray = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
                        base.Size = new Size(int.Parse(strArray[0]), int.Parse(strArray[1]));
                        base.Location = new Point(int.Parse(strArray[2]), int.Parse(strArray[3]));
                        float.TryParse(strArray[6], out this.InfoFontSize);
                        for (int j = 0; j < 5; j++)
                        {
                            boxArray[j].Size = new Size(boxArray[0].Size.Width, int.Parse(strArray[4]));
                            boxArray[j].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, boxArray[j].Font.Unit);
                        }
                        this.groupMax = int.Parse(strArray[5]);
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                }
                GroupBox[] boxArray2 = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
                for (int i = 0; i < 5; i++)
                {
                    if (i >= this.groupMax)
                    {
                        boxArray2[i].Visible = false;
                    }
                    else
                    {
                        boxArray2[i].Visible = true;
                    }
                }
                this.lstSwipes_RowsAdded(null, null);
                this.frmWatchingMoreRecords_SizeChanged(null, null);
            }
            this.timer1.Enabled = true;
        }

        private void frmWatchingMoreRecords_SizeChanged(object sender, EventArgs e)
        {
            GroupBox[] boxArray = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
            for (int i = 0; i < this.groupMax; i++)
            {
                boxArray[i].Size = new Size((this.flowLayoutPanel1.Width / this.groupMax) - 6, this.flowLayoutPanel1.Height - 0x12);
            }
        }

        private void loadPhoto(MjRec rec, string detail, ref PictureBox box)
        {
            try
            {
                box.Visible = false;
                int rec_id = Convert.ToInt32(detail.Substring(0x30, 8), 0x10);
                rec.loadPhotoFromDB(rec_id);
                Image img = box.Image;
                wgAppConfig.ShowImageStream(rec.PhotoData, ref img);
                if (img != null)
                {
                    box.Image = img;
                    box.Visible = true;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void lstSwipes_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.tbRunInfoLog != null)
            {
                this.lastCnt = this.tbRunInfoLog.Rows.Count;
                if (this.tbRunInfoLog.Rows.Count == 0)
                {
                    this.lstSwipes_RowsRemoved(null, null);
                }
                else
                {
                    base.SuspendLayout();
                    if (this.tbRunInfoLog.Rows.Count > 0)
                    {
                        int index = 0;
                        if (this.grp == null)
                        {
                            this.grp = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
                            this.txtB = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
                            this.picBox = new PictureBox[] { this.pictureBox1, this.pictureBox2, this.pictureBox3, this.pictureBox4, this.pictureBox5 };
                        }
                        for (int i = this.tbRunInfoLog.Rows.Count - 1; i >= 0; i--)
                        {
                            string str = this.tbRunInfoLog.Rows[i]["f_Detail"] as string;
                            string str2 = this.tbRunInfoLog.Rows[i]["f_MjRecStr"] as string;
                            if (!string.IsNullOrEmpty(str2))
                            {
                                MjRec rec = new MjRec(this.tbRunInfoLog.Rows[i]["f_MjRecStr"] as string, false);
                                if (rec.IsSwipeRecord)
                                {
                                    this.loadPhoto(rec, this.tbRunInfoLog.Rows[i]["f_MjRecStr"] as string, ref this.picBox[index]);
                                    this.txtB[index].Text = str;
                                    this.txtB[index].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, this.txtB[index].Font.Unit);
                                    this.grp[index].Text = this.tbRunInfoLog.Rows[i]["f_RecID"].ToString();
                                    this.grp[index].Visible = true;
                                    if (rec.IsPassed)
                                    {
                                        this.txtB[index].BackColor = Color.FromArgb(211, 231, 251);
                                        this.txtB[index].ForeColor = Color.FromArgb(20, 66, 81);
                                    }
                                    else
                                    {
                                        this.txtB[index].BackColor = Color.Orange;
                                        this.txtB[index].ForeColor = Color.FromArgb(20, 66, 81);
                                    }
                                    index++;
                                    if (index >= this.groupMax)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        this.richTextBox1.Text = this.txtB[0].Text;
                    }
                    base.ResumeLayout();
                }
            }
        }

        private void lstSwipes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.richTextBox1.Text = "";
            this.richTextBox2.Text = "";
            this.richTextBox3.Text = "";
            this.richTextBox4.Text = "";
            this.richTextBox5.Text = "";
            this.richTextBox1.BackColor = Color.FromArgb(211, 231, 251);
            this.richTextBox2.BackColor = Color.FromArgb(211, 231, 251);
            this.richTextBox3.BackColor = Color.FromArgb(211, 231, 251);
            this.richTextBox4.BackColor = Color.FromArgb(211, 231, 251);
            this.richTextBox5.BackColor = Color.FromArgb(211, 231, 251);
            this.groupBox1.Text = "";
            this.groupBox2.Text = "";
            this.groupBox3.Text = "";
            this.groupBox4.Text = "";
            this.groupBox5.Text = "";
            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;
            this.pictureBox3.Image = null;
            this.pictureBox4.Image = null;
            this.pictureBox5.Image = null;
            this.lastCnt = 0;
        }

        public void ReallyCloseForm()
        {
            wgAppConfig.DisposeImage(this.pictureBox1.Image);
            wgAppConfig.DisposeImage(this.pictureBox2.Image);
            wgAppConfig.DisposeImage(this.pictureBox3.Image);
            wgAppConfig.DisposeImage(this.pictureBox4.Image);
            wgAppConfig.DisposeImage(this.pictureBox5.Image);
            base.Close();
        }

        private void ReduceFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.InfoFontSize > 9f)
                {
                    this.InfoFontSize--;
                }
                else
                {
                    return;
                }
                RichTextBox[] boxArray = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
                for (int i = 0; i < 5; i++)
                {
                    boxArray[i].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, boxArray[i].Font.Unit);
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void ReduceInfoDisplaytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.richTextBox1.Height >= 0x1a)
                {
                    RichTextBox[] boxArray = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
                    PictureBox[] boxArray2 = new PictureBox[] { this.pictureBox1, this.pictureBox2, this.pictureBox3, this.pictureBox4, this.pictureBox5 };
                    for (int i = 0; i < 5; i++)
                    {
                        boxArray[i].Size = new Size(boxArray[i].Width, boxArray[i].Height - 0x1a);
                        boxArray2[i].Location = new Point(boxArray2[i].Location.X, boxArray2[i].Location.Y - 0x1a);
                        boxArray2[i].Size = new Size(boxArray2[i].Width, boxArray2[i].Height + 0x1a);
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wgAppConfig.UpdateKeyVal("WatchingMoreRecords_Display", "");
            base.Close();
        }

        private void saveDisplayStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "";
            string str2 = base.Size.Width.ToString() + "," + base.Size.Height.ToString() + ",";
            str = ((str2 + base.Location.X.ToString() + "," + base.Location.Y.ToString() + ",") + this.richTextBox1.Height.ToString() + "," + this.groupMax.ToString()) + "," + this.InfoFontSize.ToString();
            wgAppConfig.UpdateKeyVal("WatchingMoreRecords_Display", str);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                if ((this.tbRunInfoLog != null) && (this.lastCnt != this.tbRunInfoLog.Rows.Count))
                {
                    this.lstSwipes_RowsAdded(null, null);
                }
            }
            catch (Exception)
            {
            }
            this.timer1.Enabled = true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int num;
            if (sender == this.toolStripMenuItem2)
            {
                num = 5;
            }
            else if (sender == this.toolStripMenuItem3)
            {
                num = 4;
            }
            else if (sender == this.toolStripMenuItem4)
            {
                num = 3;
            }
            else if (sender == this.toolStripMenuItem5)
            {
                num = 2;
            }
            else if (sender == this.toolStripMenuItem6)
            {
                num = 1;
            }
            else
            {
                return;
            }
            GroupBox[] boxArray = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
            for (int i = 0; i < 5; i++)
            {
                if (i >= num)
                {
                    boxArray[i].Visible = false;
                }
                else
                {
                    boxArray[i].Visible = true;
                }
            }
            this.groupMax = num;
            this.frmWatchingMoreRecords_SizeChanged(null, null);
        }
    }
}

