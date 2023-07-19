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

    public partial class frmZones : frmBioAccess
    {
        public frmZones()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string strNewName;
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as ToolStripButton).Text;
                name.label1.Text = CommonStr.strZone;
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    strNewName = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                if (strNewName.Trim() == "")
                {
                    XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (strNewName.LastIndexOf(@"\") >= 0)
                {
                    XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    strNewName = strNewName.Trim();
                    string zoneNewName = strNewName;
                    if (sender == this.btnAddSuper)
                    {
                        this.trvDepartments.SelectedNode = null;
                    }
                    if (this.trvDepartments.SelectedNode == null)
                    {
                        foreach (TreeNode node in this.trvDepartments.Nodes)
                        {
                            if (node.Tag.ToString() == zoneNewName)
                            {
                                XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                    else
                    {
                        zoneNewName = this.trvDepartments.SelectedNode.Tag + @"\" + zoneNewName;
                        foreach (TreeNode node2 in this.trvDepartments.SelectedNode.Nodes)
                        {
                            if (node2.Tag.ToString() == zoneNewName)
                            {
                                XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
                    icControllerZone zone = new icControllerZone();
                    if (zone.checkExisted(zoneNewName))
                    {
                        XMessageBox.Show(this, zoneNewName + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        zone.addNew(zoneNewName);
                        TreeNode node3 = new TreeNode();
                        node3.Text = strNewName;
                        node3.Tag = zoneNewName;
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
                new icControllerZone().delete(this.txtSelectedDept.Text);
                this.trvDepartments.Nodes.Remove(this.trvDepartments.SelectedNode);
                this.txtSelectedDept.Text = "";
                this.trvDepartments.SelectedNode = null;
            }
        }

        private void btnEditDept_Click(object sender, EventArgs e)
        {
            string strNewName;
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.Text = (sender as ToolStripButton).Text;
                name.label1.Text = CommonStr.strZone;
                if (trvDepartments.SelectedNode != null)
                    name.strNewName = trvDepartments.SelectedNode.Text;
                if (name.ShowDialog(this) == DialogResult.OK)
                {
                    strNewName = name.strNewName;
                }
                else
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                if (strNewName.Trim() == "")
                {
                    XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (strNewName.LastIndexOf(@"\") >= 0)
                {
                    XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string str2;
                    icControllerZone zone = new icControllerZone();
                    if (this.txtSelectedDept.Text.LastIndexOf(@"\") < 0)
                    {
                        str2 = strNewName;
                    }
                    else
                    {
                        str2 = this.txtSelectedDept.Text.Substring(0, this.txtSelectedDept.Text.LastIndexOf(@"\")) + @"\" + strNewName;
                    }
                    if (zone.checkExisted(str2))
                    {
                        XMessageBox.Show(this, str2 + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        zone.Update(this.txtSelectedDept.Text, str2);
                        if (this.trvDepartments.SelectedNode != null)
                        {
                            this.trvDepartments.SelectedNode.Text = strNewName;
                            this.trvDepartments.SelectedNode.Tag = str2;
                            this.txtSelectedDept.Text = str2;
                        }
                        else
                        {
                            this.txtSelectedDept.Text = "";
                        }
                        this.loadZone();
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
            this.txtSelectedDept_TextChanged(null, null);
            this.loadOperatorPrivilege();
            this.loadZone();
            this.txtSelectedDept.Text = "";
            this.trvDepartments.SelectedNode = null;
        }

        private void loadOperatorPrivilege()
        {
            bool bReadOnly = false;
            string funName = "mnuZones";
            if (icOperator.OperatePrivilegeVisible(funName, ref bReadOnly) && bReadOnly)
            {
                this.btnAdd.Visible = false;
                this.btnAddSuper.Visible = false;
                this.btnDeleteDept.Visible = false;
                this.btnEditDept.Visible = false;
                this.toolStrip1.Visible = false;
            }
        }

        private void loadZone()
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.loadZone_Acc();
            }
            else
            {
                this.trvDepartments.Nodes.Clear();
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    using (SqlCommand command = new SqlCommand("SELECT f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName ASC", connection))
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
            }
        }

        private void loadZone_Acc()
        {
            this.trvDepartments.Nodes.Clear();
            using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
            {
                using (OleDbCommand command = new OleDbCommand("SELECT f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName ASC", connection))
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
        }

        private void trvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.trvDepartments.SelectedNode != null)
            {
                this.txtSelectedDept.Text = this.trvDepartments.SelectedNode.Tag.ToString();
            }
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

