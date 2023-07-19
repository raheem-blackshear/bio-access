namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmFind : frmBioAccess
    {
        public bool bClose;
        private bool bFound;
        private long cntFound;
        private int curCol;
        private object curfrm;
        private object curObjtofind;
        private int curRow;
        private string curTexttofind = "";
        private object prevObjtofind;
        private string prevTexttofind = "";

        public dfrmFind()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Hide();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.curfrm != null) && ((this.curfrm as Form).ActiveControl != this.curObjtofind))
                {
                    this.setObjtoFind((this.curfrm as Form).ActiveControl, this.curfrm);
                }
                this.curTexttofind = this.txtFind.Text.Trim().ToUpper();
                if (this.curTexttofind == "")
                {
                    return;
                }
                bool flag = false;
                if (((this.prevObjtofind == this.curObjtofind) && (wgTools.SetObjToStr(this.curTexttofind).ToUpper() == wgTools.SetObjToStr(this.prevTexttofind).ToUpper())) && (sender == this.btnFind))
                {
                    flag = true;
                }
                if (!flag)
                {
                    this.curRow = 0;
                    this.curCol = 0;
                    this.cntFound = 0L;
                    this.prevObjtofind = this.curObjtofind;
                    this.prevTexttofind = this.curTexttofind;
                    this.bFound = false;
                }
                if (!(this.curObjtofind is DataGridView))
                {
                    goto Label_02A1;
                }
                DataGridView curObjtofind = (DataGridView) this.curObjtofind;
                int curRow = this.curRow;
                int curCol = this.curCol;
                curObjtofind.ClearSelection();
                goto Label_0208;
            Label_011D:
                if (curObjtofind.Columns[curCol].Visible && (wgTools.SetObjToStr(curObjtofind.Rows[curRow].Cells[curCol].Value).ToUpper().IndexOf(this.curTexttofind) >= 0))
                {
                    curObjtofind.FirstDisplayedScrollingRowIndex = curRow;
                    curObjtofind.Rows[curRow].Selected = true;
                    this.bFound = true;
                    this.curRow = curRow + 1;
                    this.curCol = 0;
                    this.cntFound += 1L;
                    if (sender == this.btnFind)
                    {
                        this.lblCount.Text = this.cntFound.ToString();
                        return;
                    }
                    if (sender == this.btnMarkAll)
                    {
                        this.lblCount.Text = this.cntFound.ToString();
                        goto Label_0202;
                    }
                }
                curCol++;
            Label_01F6:
                if (curCol < curObjtofind.ColumnCount)
                {
                    goto Label_011D;
                }
            Label_0202:
                curCol = 0;
                curRow++;
            Label_0208:
                if (curRow < curObjtofind.Rows.Count)
                {
                    goto Label_01F6;
                }
                this.curRow = 0;
                this.curCol = 0;
                this.lblCount.Text = this.cntFound.ToString();
                if (this.bFound)
                {
                    XMessageBox.Show(CommonStr.strFindComplete);
                }
                else
                {
                    this.cntFound = 0L;
                    this.lblCount.Text = this.cntFound.ToString();
                    XMessageBox.Show(CommonStr.strNotFind);
                }
                this.cntFound = 0L;
                this.lblCount.Text = this.cntFound.ToString();
                this.selectTxtFind();
                return;
            Label_02A1:
                if (this.curObjtofind is ComboBox)
                {
                    ComboBox box = (ComboBox) this.curObjtofind;
                    for (int i = this.curRow; i < box.Items.Count; i++)
                    {
                        object valA = box.Items[i];
                        if (wgTools.SetObjToStr(valA).ToUpper().IndexOf(this.curTexttofind) >= 0)
                        {
                            box.SelectedItem = box.Items[i];
                            box.SelectedIndex = i;
                            this.bFound = true;
                            this.curRow = i + 1;
                            this.curCol = 0;
                            this.cntFound += 1L;
                            this.lblCount.Text = this.cntFound.ToString();
                            return;
                        }
                    }
                    this.curRow = 0;
                    this.curCol = 0;
                    this.lblCount.Text = this.cntFound.ToString();
                    if (this.bFound)
                    {
                        XMessageBox.Show(CommonStr.strFindComplete);
                    }
                    else
                    {
                        this.cntFound = 0L;
                        this.lblCount.Text = this.cntFound.ToString();
                        XMessageBox.Show(CommonStr.strNotFind);
                    }
                    this.cntFound = 0L;
                    this.selectTxtFind();
                    this.lblCount.Text = this.cntFound.ToString();
                }
                else if (this.curObjtofind is ListBox)
                {
                    ListBox box2 = (ListBox) this.curObjtofind;
                    int index = this.curRow;
                    box2.ClearSelected();
                    box2.ClearSelected();
                    while (index < box2.Items.Count)
                    {
                        object obj4;
                        if (box2.DisplayMember == "")
                        {
                            obj4 = box2.Items[index];
                        }
                        else
                        {
                            index++;
                            continue;
                        }
                        if (wgTools.SetObjToStr(obj4).ToUpper().IndexOf(this.curTexttofind) >= 0)
                        {
                            box2.SetSelected(index, true);
                            this.bFound = true;
                            this.curRow = index + 1;
                            this.curCol = 0;
                            this.cntFound += 1L;
                            if (sender == this.btnFind)
                            {
                                this.lblCount.Text = this.cntFound.ToString();
                                return;
                            }
                            ImageButton btnMarkAll = this.btnMarkAll;
                        }
                        index++;
                    }
                    this.curRow = 0;
                    this.curCol = 0;
                    this.lblCount.Text = this.cntFound.ToString();
                    if (this.bFound)
                    {
                        XMessageBox.Show(CommonStr.strFindComplete);
                    }
                    else
                    {
                        this.cntFound = 0L;
                        this.lblCount.Text = this.cntFound.ToString();
                        XMessageBox.Show(CommonStr.strNotFind);
                    }
                    this.cntFound = 0L;
                    this.selectTxtFind();
                    this.lblCount.Text = this.cntFound.ToString();
                }
                else if (this.curObjtofind is CheckedListBox)
                {
                    CheckedListBox box3 = (CheckedListBox) this.curObjtofind;
                    int num5 = this.curRow;
                    box3.ClearSelected();
                    box3.ClearSelected();
                    while (num5 < box3.Items.Count)
                    {
                        object obj5;
                        if (box3.DisplayMember == "")
                        {
                            obj5 = box3.Items[num5];
                        }
                        else
                        {
                            num5++;
                            continue;
                        }
                        if (wgTools.SetObjToStr(obj5).ToUpper().IndexOf(this.curTexttofind) >= 0)
                        {
                            box3.SetSelected(num5, true);
                            this.bFound = true;
                            this.curRow = num5 + 1;
                            this.curCol = 0;
                            this.cntFound += 1L;
                            if (sender == this.btnFind)
                            {
                                this.lblCount.Text = this.cntFound.ToString();
                                return;
                            }
                            ImageButton button2 = this.btnMarkAll;
                        }
                        num5++;
                    }
                    this.curRow = 0;
                    this.curCol = 0;
                    this.lblCount.Text = this.cntFound.ToString();
                    if (this.bFound)
                    {
                        XMessageBox.Show(CommonStr.strFindComplete);
                    }
                    else
                    {
                        this.cntFound = 0L;
                        this.lblCount.Text = this.cntFound.ToString();
                        XMessageBox.Show(CommonStr.strNotFind);
                    }
                    this.cntFound = 0L;
                    this.selectTxtFind();
                    this.lblCount.Text = this.cntFound.ToString();
                }
                else if (this.curObjtofind is ListView)
                {
                    ListView view2 = (ListView) this.curObjtofind;
                    int num6 = this.curRow;
                    view2.SelectedItems.Clear();
                    while (num6 < view2.Items.Count)
                    {
                        object text;
                        if (view2.View == View.Details)
                        {
                            text = "";
                            for (int j = 0; j < (view2.Items[num6].SubItems.Count - 1); j++)
                            {
                                text = text + "    " + view2.Items[num6].SubItems[j].Text;
                            }
                        }
                        else
                        {
                            text = view2.Items[num6].Text;
                        }
                        if (wgTools.SetObjToStr(text).ToUpper().IndexOf(this.curTexttofind) >= 0)
                        {
                            view2.Items[num6].Selected = true;
                            view2.Items[num6].EnsureVisible();
                            view2.Focus();
                            this.bFound = true;
                            this.curRow = num6 + 1;
                            this.curCol = 0;
                            this.cntFound += 1L;
                            if (sender == this.btnFind)
                            {
                                this.lblCount.Text = this.cntFound.ToString();
                                return;
                            }
                        }
                        num6++;
                    }
                    this.curRow = 0;
                    this.curCol = 0;
                    this.lblCount.Text = this.cntFound.ToString();
                    if (this.bFound)
                    {
                        XMessageBox.Show(CommonStr.strFindComplete);
                    }
                    else
                    {
                        this.cntFound = 0L;
                        this.lblCount.Text = this.cntFound.ToString();
                        XMessageBox.Show(CommonStr.strNotFind);
                    }
                    this.cntFound = 0L;
                    this.lblCount.Text = this.cntFound.ToString();
                    this.selectTxtFind();
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void dfrmFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.bClose)
            {
                base.Hide();
                e.Cancel = true;
            }
        }

        private void dfrmFind_Load(object sender, EventArgs e)
        {
        }

        public void ReallyCloseForm()
        {
            this.bClose = true;
            base.Close();
        }

        private void selectTxtFind()
        {
            try
            {
                base.ActiveControl = this.btnFind;
                if (this.txtFind.Text.Length > 0)
                {
                    this.txtFind.SelectionStart = 0;
                    this.txtFind.SelectionLength = this.txtFind.Text.Length;
                }
                base.ActiveControl = this.txtFind;
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        public void setObjtoFind(object obj, object frm)
        {
            try
            {
                bool flag = false;
                object curObjtofind = this.curObjtofind;
                if (this.curObjtofind != null)
                {
                    flag = true;
                }
                this.curObjtofind = obj;
                if (this.curObjtofind is DataGridView)
                {
                    this.btnMarkAll.Visible = true;
                    this.btnMarkAll.Enabled = true;
                    flag = true;
                }
                else if (this.curObjtofind is ListBox)
                {
                    this.btnMarkAll.Visible = true;
                    this.btnMarkAll.Enabled = true;
                    flag = true;
                }
                else if (this.curObjtofind is ComboBox)
                {
                    this.btnMarkAll.Visible = false;
                    this.btnMarkAll.Enabled = false;
                    flag = true;
                }
                else if (this.curObjtofind is ListView)
                {
                    this.btnMarkAll.Visible = false;
                    this.btnMarkAll.Enabled = false;
                    flag = true;
                }
                else if (this.curObjtofind is CheckedListBox)
                {
                    this.btnMarkAll.Visible = false;
                    this.btnMarkAll.Enabled = false;
                    flag = true;
                }
                else
                {
                    this.curObjtofind = curObjtofind;
                }
                this.cntFound = 0L;
                this.lblCount.Text = this.cntFound.ToString();
                this.selectTxtFind();
                if (flag)
                {
                    this.curfrm = frm;
                    base.Show();
                    base.Focus();
                }
                else
                {
                    base.Hide();
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue == 13) && this.btnFind.Enabled)
            {
                this.btnFind.PerformClick();
            }
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            if (this.txtFind.Text.Length == 0)
            {
                this.btnFind.Enabled = false;
            }
            else
            {
                this.btnFind.Enabled = true;
            }
        }
    }
}

