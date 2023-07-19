namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmDepartments : frmBioAccess
    {
        public frmDepartments()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as ToolStripButton).Text;
                name.label1.Text = wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartment);
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    this.txtDeptName.Text = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!(this.txtDeptName.Text.Trim() == ""))
            {
                if (this.txtDeptName.Text.LastIndexOf(@"\") >= 0)
                {
                    XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.txtDeptName.Text = this.txtDeptName.Text.Trim();
                    string text = this.txtDeptName.Text;
                    if (sender == this.btnAddSuper)
                    {
                        this.trvDepartments.SelectedNode = null;
                    }
                    if (this.trvDepartments.SelectedNode == null)
                    {
                        foreach (TreeNode node in this.trvDepartments.Nodes)
                        {
                            if (node.Tag.ToString() == text)
                            {
                                XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                    else
                    {
                        text = this.trvDepartments.SelectedNode.Tag + @"\" + text;
                        foreach (TreeNode node2 in this.trvDepartments.SelectedNode.Nodes)
                        {
                            if (node2.Tag.ToString() == text)
                            {
                                XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                    if (text.Length > 255)
                    {
                        XMessageBox.Show(this, CommonStr.strDepartNameTooLong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    icGroup group = new icGroup();
                    if (group.checkExisted(text))
                    {
                        XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        group.addNew(text);
                        TreeNode node3 = new TreeNode();
                        node3.Text = this.txtDeptName.Text;
                        node3.Tag = text;
                        if (this.trvDepartments.SelectedNode == null)
                        {
                            this.trvDepartments.Nodes.Add(node3);
                            this.trvDepartments.ExpandAll();
                        }
                        else
                        {
                            this.trvDepartments.SelectedNode.Nodes.Add(node3);
                            this.trvDepartments.SelectedNode.Expand();
                        }
                    }
                }
            }
            else
            {
                XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAddSuper_Click(object sender, EventArgs e)
        {
            this.trvDepartments.SelectedNode = null;
            this.btnAdd_Click(sender, e);
        }

        private void btnDeleteDept_Click(object sender, EventArgs e)
        {
            if (XMessageBox.Show(this.btnDeleteDept.Text + "\r\n\r\n" + this.txtSelectedDept.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                new icGroup().delete(this.txtSelectedDept.Text);
                this.trvDepartments.Nodes.Remove(this.trvDepartments.SelectedNode);
                this.txtSelectedDept.Text = "";
                this.txtDeptName.Text = "";
                this.trvDepartments.SelectedNode = null;
            }
        }

        private void btnEditDept_Click(object sender, EventArgs e)
        {
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as ToolStripButton).Text;
                name.label1.Text = wgAppConfig.ReplaceFloorRomm(CommonStr.strDepartment);
                if (txtSelectedDept.Text.LastIndexOf(@"\") < 0)
                    name.strNewName = txtSelectedDept.Text;
                else
                    name.strNewName = txtSelectedDept.Text.Substring(txtSelectedDept.Text.LastIndexOf(@"\") + 1);
                if (name.ShowDialog(this) == DialogResult.OK)
                    this.txtDeptName.Text = name.strNewName;
                else
                    return;
            }
            if (!(this.txtDeptName.Text.Trim() == ""))
            {
                if (this.txtDeptName.Text.LastIndexOf(@"\") >= 0)
                {
                    XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string text;
                    icGroup group = new icGroup();
                    if (this.txtSelectedDept.Text.LastIndexOf(@"\") < 0)
                    {
                        text = this.txtDeptName.Text;
                    }
                    else
                    {
                        text = this.txtSelectedDept.Text.Substring(0, this.txtSelectedDept.Text.LastIndexOf(@"\")) + @"\" + this.txtDeptName.Text;
                    }
                    if (group.checkExisted(text))
                    {
                        XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        group.Update(this.txtSelectedDept.Text, text);
                        if (this.trvDepartments.SelectedNode != null)
                        {
                            this.trvDepartments.SelectedNode.Text = this.txtDeptName.Text;
                            this.trvDepartments.SelectedNode.Tag = text;
                            this.txtSelectedDept.Text = text;
                        }
                        else
                        {
                            this.txtSelectedDept.Text = "";
                        }
                        this.loadDepartment();
                    }
                }
            }
            else
            {
                XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void FindRecursive(TreeNode treeNode, string ParentNodeText, out TreeNode foundNode)
        {
            foundNode = null;
            if (treeNode.Tag.ToString() == ParentNodeText)
            {
                foundNode = treeNode;
            }
            else if (foundNode == null)
            {
                foreach (TreeNode node in treeNode.Nodes)
                {
                    if (foundNode != null)
                    {
                        break;
                    }
                    this.FindRecursive(node, ParentNodeText, out foundNode);
                }
            }
        }

        private void frmDepartments_Load(object sender, EventArgs e)
        {
            this.txtDeptName_TextChanged(null, null);
            this.txtSelectedDept_TextChanged(null, null);
            this.loadOperatorPrivilege();
            this.loadDepartment();
            this.txtSelectedDept.Text = "";
            this.txtDeptName.Text = "";
            this.trvDepartments.SelectedNode = null;
            this.Text = wgAppConfig.ReplaceFloorRomm(this.Text);
            this.toolStripLabel1.Text = wgAppConfig.ReplaceFloorRomm(this.toolStripLabel1.Text);
            this.btnAddSuper.Text = wgAppConfig.ReplaceFloorRomm(this.btnAddSuper.Text);
            this.btnAdd.Text = wgAppConfig.ReplaceFloorRomm(this.btnAdd.Text);
            this.btnEditDept.Text = wgAppConfig.ReplaceFloorRomm(this.btnEditDept.Text);
            this.btnDeleteDept.Text = wgAppConfig.ReplaceFloorRomm(this.btnDeleteDept.Text);
            this.toolStripLabel2.Text = wgAppConfig.ReplaceFloorRomm(this.toolStripLabel2.Text);
        }

        private void loadDepartment()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadDepartment_Acc();
            }
            else
            {
                this.trvDepartments.Nodes.Clear();
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
                try
                {
                    using (SqlCommand command = new SqlCommand("SELECT f_GroupName,f_GroupNO FROM t_b_Group ORDER BY f_GroupName ASC", connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                        while (reader.Read())
                        {
                            TreeNode node = new TreeNode();
                            node.Text = wgTools.SetObjToStr(reader[0]);
                            node.Tag = wgTools.SetObjToStr(reader[0]);
                            if (node.Text.LastIndexOf(@"\") > 0)
                            {
                                string parentNodeText = node.Text.Substring(0, node.Text.LastIndexOf(@"\"));
                                node.Text = node.Text.Substring(node.Text.LastIndexOf(@"\") + 1);
                                foreach (TreeNode node3 in this.trvDepartments.Nodes)
                                {
                                    TreeNode node2;
                                    this.FindRecursive(node3, parentNodeText, out node2);
                                    if (node2 != null)
                                    {
                                        node2.Nodes.Add(node);
                                    }
                                }
                            }
                            else
                            {
                                this.trvDepartments.Nodes.Add(node);
                            }
                        }
                        reader.Close();
                        this.trvDepartments.ExpandAll();
                    }
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void loadDepartment_Acc()
        {
            this.trvDepartments.Nodes.Clear();
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                using (OleDbCommand command = new OleDbCommand("SELECT f_GroupName,f_GroupNO FROM t_b_Group ORDER BY f_GroupName ASC", connection))
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                    while (reader.Read())
                    {
                        TreeNode node = new TreeNode();
                        node.Text = wgTools.SetObjToStr(reader[0]);
                        node.Tag = wgTools.SetObjToStr(reader[0]);
                        if (node.Text.LastIndexOf(@"\") > 0)
                        {
                            string parentNodeText = node.Text.Substring(0, node.Text.LastIndexOf(@"\"));
                            node.Text = node.Text.Substring(node.Text.LastIndexOf(@"\") + 1);
                            foreach (TreeNode node3 in this.trvDepartments.Nodes)
                            {
                                TreeNode node2;
                                this.FindRecursive(node3, parentNodeText, out node2);
                                if (node2 != null)
                                {
                                    node2.Nodes.Add(node);
                                }
                            }
                        }
                        else
                        {
                            this.trvDepartments.Nodes.Add(node);
                        }
                    }
                    reader.Close();
                    this.trvDepartments.ExpandAll();
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuGroups";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnAddSuper.Visible = false;
                this.btnDeleteDept.Visible = false;
                this.btnEditDept.Visible = false;
                this.toolStrip1.Visible = false;
            }
        }

        private void trvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.trvDepartments.SelectedNode != null)
            {
                this.txtSelectedDept.Text = this.trvDepartments.SelectedNode.Tag.ToString();
            }
        }

        private void txtDeptName_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtSelectedDept_TextChanged(object sender, EventArgs e)
        {
            if (this.txtSelectedDept.Text.Length > 0)
            {
                this.btnDeleteDept.Enabled = true;
                this.btnAdd.Enabled = true;
                this.btnEditDept.Enabled = true;
            }
            else
            {
                this.btnDeleteDept.Enabled = false;
                this.btnAdd.Enabled = false;
                this.btnEditDept.Enabled = false;
            }
        }
    }
}

