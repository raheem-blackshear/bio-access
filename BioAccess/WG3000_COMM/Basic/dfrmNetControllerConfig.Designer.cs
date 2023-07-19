namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmNetControllerConfig
    {
        private ToolStripMenuItem addSelectedToSystemToolStripMenuItem;
        public ImageButton btnAddToSystem;
        private ImageButton btnConfigure;
        private ImageButton btnDefault;
        private ImageButton btnExit;
        public ImageButton btnIPAndWebConfigure;
        private ImageButton btnSearch;
        private CheckBox chkSearchAgain;
        private ToolStripMenuItem clearSwipesToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem communicationTestToolStripMenuItem;
        private IContainer components = null;
        private ToolStripMenuItem configureToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private dfrmFind dfrmFind1;
        private frmTestController dfrmTest;
        private DataGridView dgvFoundControllers;
        private DataTable dtPrivilege;
        private DataTable tbFpTempl;
        private DataTable tbFaceTempl;
        private DataTable dtWebStringAdvanced_IPWEB;
        private DataView dv;
        private ToolStripMenuItem findF3ToolStripMenuItem;
        private ToolStripMenuItem formatToolStripMenuItem;
        private frmProductFormat frmProductFormat1;
        private Label label1;
        private Label lblCount;
        private Label lblSearchNow;
        private ToolStripMenuItem restoreAllSwipesToolStripMenuItem;
        private ToolStripMenuItem restoreDefaultIPToolStripMenuItem;
        private ToolStripMenuItem restoreDefaultParamToolStripMenuItem;
        private ToolStripMenuItem search100FromTheSpecialSNToolStripMenuItem;
        private ToolStripMenuItem searchAdvancedToolStripMenuItem;
        private ToolStripMenuItem searchSpecialSNToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private StatusStrip statusStrip1;
        private DataTable tb;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private wgUdpComm wgudp;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.wgudp != null))
            {
                this.wgudp.Dispose();
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmNetControllerConfig));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findF3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchAdvancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchSpecialSNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.search100FromTheSpecialSNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.communicationTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultParamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreAllSwipesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSelectedToSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSwipesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnIPAndWebConfigure = new System.Windows.Forms.ImageButton();
            this.lblCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSearchNow = new System.Windows.Forms.Label();
            this.chkSearchAgain = new System.Windows.Forms.CheckBox();
            this.btnAddToSystem = new System.Windows.Forms.ImageButton();
            this.dgvFoundControllers = new System.Windows.Forms.DataGridView();
            this.f_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Mask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Gateway = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MACAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_PCIPAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_WiFiChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExit = new System.Windows.Forms.ImageButton();
            this.btnDefault = new System.Windows.Forms.ImageButton();
            this.btnConfigure = new System.Windows.Forms.ImageButton();
            this.btnSearch = new System.Windows.Forms.ImageButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoundControllers)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.findF3ToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.searchAdvancedToolStripMenuItem,
            this.communicationTestToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.restoreDefaultIPToolStripMenuItem,
            this.restoreDefaultParamToolStripMenuItem,
            this.restoreAllSwipesToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.addSelectedToSystemToolStripMenuItem,
            this.clearSwipesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            resources.ApplyResources(this.configureToolStripMenuItem, "configureToolStripMenuItem");
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // findF3ToolStripMenuItem
            // 
            this.findF3ToolStripMenuItem.Name = "findF3ToolStripMenuItem";
            resources.ApplyResources(this.findF3ToolStripMenuItem, "findF3ToolStripMenuItem");
            this.findF3ToolStripMenuItem.Click += new System.EventHandler(this.findF3ToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            resources.ApplyResources(this.searchToolStripMenuItem, "searchToolStripMenuItem");
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // searchAdvancedToolStripMenuItem
            // 
            this.searchAdvancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchSpecialSNToolStripMenuItem,
            this.search100FromTheSpecialSNToolStripMenuItem});
            this.searchAdvancedToolStripMenuItem.Name = "searchAdvancedToolStripMenuItem";
            resources.ApplyResources(this.searchAdvancedToolStripMenuItem, "searchAdvancedToolStripMenuItem");
            // 
            // searchSpecialSNToolStripMenuItem
            // 
            this.searchSpecialSNToolStripMenuItem.Name = "searchSpecialSNToolStripMenuItem";
            resources.ApplyResources(this.searchSpecialSNToolStripMenuItem, "searchSpecialSNToolStripMenuItem");
            this.searchSpecialSNToolStripMenuItem.Click += new System.EventHandler(this.search100FromTheSpecialSNToolStripMenuItem_Click);
            // 
            // search100FromTheSpecialSNToolStripMenuItem
            // 
            this.search100FromTheSpecialSNToolStripMenuItem.Name = "search100FromTheSpecialSNToolStripMenuItem";
            resources.ApplyResources(this.search100FromTheSpecialSNToolStripMenuItem, "search100FromTheSpecialSNToolStripMenuItem");
            this.search100FromTheSpecialSNToolStripMenuItem.Click += new System.EventHandler(this.search100FromTheSpecialSNToolStripMenuItem_Click);
            // 
            // communicationTestToolStripMenuItem
            // 
            this.communicationTestToolStripMenuItem.Name = "communicationTestToolStripMenuItem";
            resources.ApplyResources(this.communicationTestToolStripMenuItem, "communicationTestToolStripMenuItem");
            this.communicationTestToolStripMenuItem.Click += new System.EventHandler(this.communicationTestToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // restoreDefaultIPToolStripMenuItem
            // 
            this.restoreDefaultIPToolStripMenuItem.Name = "restoreDefaultIPToolStripMenuItem";
            resources.ApplyResources(this.restoreDefaultIPToolStripMenuItem, "restoreDefaultIPToolStripMenuItem");
            this.restoreDefaultIPToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultIPToolStripMenuItem_Click);
            // 
            // restoreDefaultParamToolStripMenuItem
            // 
            this.restoreDefaultParamToolStripMenuItem.Name = "restoreDefaultParamToolStripMenuItem";
            resources.ApplyResources(this.restoreDefaultParamToolStripMenuItem, "restoreDefaultParamToolStripMenuItem");
            this.restoreDefaultParamToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultParamToolStripMenuItem_Click);
            // 
            // restoreAllSwipesToolStripMenuItem
            // 
            this.restoreAllSwipesToolStripMenuItem.Name = "restoreAllSwipesToolStripMenuItem";
            resources.ApplyResources(this.restoreAllSwipesToolStripMenuItem, "restoreAllSwipesToolStripMenuItem");
            this.restoreAllSwipesToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSwipesToolStripMenuItem_Click);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            resources.ApplyResources(this.formatToolStripMenuItem, "formatToolStripMenuItem");
            this.formatToolStripMenuItem.Click += new System.EventHandler(this.formatToolStripMenuItem_Click);
            // 
            // addSelectedToSystemToolStripMenuItem
            // 
            this.addSelectedToSystemToolStripMenuItem.Name = "addSelectedToSystemToolStripMenuItem";
            resources.ApplyResources(this.addSelectedToSystemToolStripMenuItem, "addSelectedToSystemToolStripMenuItem");
            this.addSelectedToSystemToolStripMenuItem.Click += new System.EventHandler(this.addSelectedToSystemToolStripMenuItem_Click);
            // 
            // clearSwipesToolStripMenuItem
            // 
            this.clearSwipesToolStripMenuItem.Name = "clearSwipesToolStripMenuItem";
            resources.ApplyResources(this.clearSwipesToolStripMenuItem, "clearSwipesToolStripMenuItem");
            this.clearSwipesToolStripMenuItem.Click += new System.EventHandler(this.clearSwipesToolStripMenuItem_Click);
            // 
            // btnIPAndWebConfigure
            // 
            this.btnIPAndWebConfigure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnIPAndWebConfigure, "btnIPAndWebConfigure");
            this.btnIPAndWebConfigure.Focusable = true;
            this.btnIPAndWebConfigure.ForeColor = System.Drawing.Color.White;
            this.btnIPAndWebConfigure.Name = "btnIPAndWebConfigure";
            this.btnIPAndWebConfigure.Toggle = false;
            this.btnIPAndWebConfigure.UseVisualStyleBackColor = false;
            this.btnIPAndWebConfigure.Click += new System.EventHandler(this.btnIPAndWebConfigure_Click);
            // 
            // lblCount
            // 
            resources.ApplyResources(this.lblCount, "lblCount");
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblCount.Name = "lblCount";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.label1.Name = "label1";
            // 
            // lblSearchNow
            // 
            resources.ApplyResources(this.lblSearchNow, "lblSearchNow");
            this.lblSearchNow.BackColor = System.Drawing.Color.Transparent;
            this.lblSearchNow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.lblSearchNow.Name = "lblSearchNow";
            // 
            // chkSearchAgain
            // 
            resources.ApplyResources(this.chkSearchAgain, "chkSearchAgain");
            this.chkSearchAgain.BackColor = System.Drawing.Color.Transparent;
            this.chkSearchAgain.Checked = true;
            this.chkSearchAgain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchAgain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            this.chkSearchAgain.Name = "chkSearchAgain";
            this.chkSearchAgain.UseVisualStyleBackColor = false;
            // 
            // btnAddToSystem
            // 
            this.btnAddToSystem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnAddToSystem, "btnAddToSystem");
            this.btnAddToSystem.Focusable = true;
            this.btnAddToSystem.ForeColor = System.Drawing.Color.White;
            this.btnAddToSystem.Name = "btnAddToSystem";
            this.btnAddToSystem.Toggle = false;
            this.btnAddToSystem.UseVisualStyleBackColor = false;
            this.btnAddToSystem.Click += new System.EventHandler(this.btnAddToSystem_Click);
            // 
            // dgvFoundControllers
            // 
            this.dgvFoundControllers.AllowUserToAddRows = false;
            this.dgvFoundControllers.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgvFoundControllers, "dgvFoundControllers");
            this.dgvFoundControllers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFoundControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFoundControllers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFoundControllers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ID,
            this.f_ControllerSN,
            this.f_IP,
            this.f_Mask,
            this.f_Gateway,
            this.f_PORT,
            this.f_MACAddr,
            this.f_PCIPAddr,
            this.f_Note,
            this.f_WiFiChannel});
            this.dgvFoundControllers.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvFoundControllers.EnableHeadersVisualStyles = false;
            this.dgvFoundControllers.Name = "dgvFoundControllers";
            this.dgvFoundControllers.ReadOnly = true;
            this.dgvFoundControllers.RowHeadersVisible = false;
            this.dgvFoundControllers.RowTemplate.Height = 23;
            this.dgvFoundControllers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFoundControllers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvFoundControllers_MouseDoubleClick);
            // 
            // f_ID
            // 
            resources.ApplyResources(this.f_ID, "f_ID");
            this.f_ID.Name = "f_ID";
            this.f_ID.ReadOnly = true;
            // 
            // f_ControllerSN
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            // 
            // f_IP
            // 
            resources.ApplyResources(this.f_IP, "f_IP");
            this.f_IP.Name = "f_IP";
            this.f_IP.ReadOnly = true;
            // 
            // f_Mask
            // 
            resources.ApplyResources(this.f_Mask, "f_Mask");
            this.f_Mask.Name = "f_Mask";
            this.f_Mask.ReadOnly = true;
            // 
            // f_Gateway
            // 
            resources.ApplyResources(this.f_Gateway, "f_Gateway");
            this.f_Gateway.Name = "f_Gateway";
            this.f_Gateway.ReadOnly = true;
            // 
            // f_PORT
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_PORT.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.f_PORT, "f_PORT");
            this.f_PORT.Name = "f_PORT";
            this.f_PORT.ReadOnly = true;
            // 
            // f_MACAddr
            // 
            resources.ApplyResources(this.f_MACAddr, "f_MACAddr");
            this.f_MACAddr.Name = "f_MACAddr";
            this.f_MACAddr.ReadOnly = true;
            // 
            // f_PCIPAddr
            // 
            resources.ApplyResources(this.f_PCIPAddr, "f_PCIPAddr");
            this.f_PCIPAddr.Name = "f_PCIPAddr";
            this.f_PCIPAddr.ReadOnly = true;
            // 
            // f_Note
            // 
            resources.ApplyResources(this.f_Note, "f_Note");
            this.f_Note.Name = "f_Note";
            this.f_Note.ReadOnly = true;
            // 
            // f_WiFiChannel
            // 
            resources.ApplyResources(this.f_WiFiChannel, "f_WiFiChannel");
            this.f_WiFiChannel.Name = "f_WiFiChannel";
            this.f_WiFiChannel.ReadOnly = true;
            this.f_WiFiChannel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.f_WiFiChannel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Focusable = true;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.Toggle = false;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnDefault, "btnDefault");
            this.btnDefault.Focusable = true;
            this.btnDefault.ForeColor = System.Drawing.Color.White;
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Toggle = false;
            this.btnDefault.UseVisualStyleBackColor = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnConfigure
            // 
            this.btnConfigure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnConfigure, "btnConfigure");
            this.btnConfigure.Focusable = true;
            this.btnConfigure.ForeColor = System.Drawing.Color.White;
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Toggle = false;
            this.btnConfigure.UseVisualStyleBackColor = false;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Focusable = true;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Toggle = false;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(92)))), ((int)(((byte)(120)))));
            this.statusStrip1.BackgroundImage = global::Properties.Resources.pMain_bottom;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Spring = true;
            // 
            // dfrmNetControllerConfig
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnIPAndWebConfigure);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSearchNow);
            this.Controls.Add(this.chkSearchAgain);
            this.Controls.Add(this.btnAddToSystem);
            this.Controls.Add(this.dgvFoundControllers);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnConfigure);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnSearch);
            this.MinimizeBox = false;
            this.Name = "dfrmNetControllerConfig";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dfrmNetControllerConfig_FormClosing);
            this.Load += new System.EventHandler(this.dfrmNetControllerConfig_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmNetControllerConfig_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoundControllers)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private DataGridViewTextBoxColumn f_ID;
        private DataGridViewTextBoxColumn f_ControllerSN;
        private DataGridViewTextBoxColumn f_IP;
        private DataGridViewTextBoxColumn f_Mask;
        private DataGridViewTextBoxColumn f_Gateway;
        private DataGridViewTextBoxColumn f_PORT;
        private DataGridViewTextBoxColumn f_MACAddr;
        private DataGridViewTextBoxColumn f_PCIPAddr;
        private DataGridViewTextBoxColumn f_Note;
        private DataGridViewTextBoxColumn f_WiFiChannel;
    }
}

