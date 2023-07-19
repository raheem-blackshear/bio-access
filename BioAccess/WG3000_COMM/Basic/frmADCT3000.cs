namespace WG3000_COMM.Basic
{
	using System;
	using System.ComponentModel;
	using System.Data.SqlClient;
	using System.Diagnostics;
	using System.Drawing;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Windows.Forms;
	using WG3000_COMM;
	using WG3000_COMM.Core;
	using WG3000_COMM.DataOper;
	using WG3000_COMM.ExtendFunc;
	using WG3000_COMM.ExtendFunc.Elevator;
	using WG3000_COMM.ExtendFunc.Meal;
	using WG3000_COMM.ExtendFunc.Meeting;
	using WG3000_COMM.ExtendFunc.Patrol;
	using WG3000_COMM.ExtendFunc.PCCheck;
	using WG3000_COMM.Properties;
	using WG3000_COMM.ResStrings;

    public partial class frmADCT3000 : Form
    {
        private bool bConfirmClose;
        private bool bDisplayHideLogin = true;
        private string defaultTitle = "";

        private static readonly Color def_bk_normal = System.Drawing.Color.FromArgb(67, 197, 204);
        private static readonly Color def_bk_focus = System.Drawing.Color.FromArgb(0, 80, 88);

        private static readonly Color bk_purple_normal = System.Drawing.Color.FromArgb(98, 60, 188);
        private static readonly Color bk_purple_focus = System.Drawing.Color.FromArgb(52, 33, 102);

        private static readonly Color bk_orange_normal = System.Drawing.Color.FromArgb(219, 85, 45);
        private static readonly Color bk_orange_focus = System.Drawing.Color.FromArgb(128, 50, 27);

        private FunctionData[] functionNameQuickConfig;

        private FunctionData[] functionNameAttendence = new FunctionData[] 
            { 
                new FunctionData("시간설정", "mnuShiftNormalConfigure", "Reports.Shift.dfrmShiftNormalParamSet", global::Properties.Resources.Single_Shift_Rule, null, bk_purple_normal, bk_purple_focus),
                new FunctionData("휴일", "mnuHolidaySet", "Reports.Shift.dfrmHolidaySet", global::Properties.Resources.Holiday, null, def_bk_normal, def_bk_focus),
                new FunctionData("휴가출장", "mnuLeave", "Reports.Shift.frmLeave", global::Properties.Resources.Business_Trip, null, def_bk_normal, def_bk_focus),
                new FunctionData("출석", "mnuManualCardRecord", "Reports.Shift.frmManualSwipeRecords", global::Properties.Resources.Manual_Sign, null, def_bk_normal, def_bk_focus),
                new FunctionData("출퇴근통계표", "mnuAttendenceData", "Reports.Shift.frmShiftAttReport", global::Properties.Resources.Attendance_Report, null, bk_orange_normal, bk_orange_focus),
            };

        private FunctionData[] functionNameBasicConfigure = new FunctionData[] 
            { 
                new FunctionData( "기대", "mnuControllers", "Basic.frmControllers", global::Properties.Resources.Add_Device, null, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "부문", "mnuGroups", "Basic.frmDepartments", global::Properties.Resources.Department, null, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "사용자", "mnuConsumers", "Basic.frmUsers", global::Properties.Resources.Personnel, null, bk_purple_normal, bk_purple_focus ),
            };

        private FunctionData[] functionNameBasicOperate = new FunctionData[] 
            { 
                new FunctionData( "감시", "mnuTotalControl", "Basic.frmConsole", global::Properties.Resources.Total_Console, null, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "원시기록검색", "mnuCardRecords", "Basic.frmSwipeRecords", global::Properties.Resources.Search_Log, null, bk_purple_normal, bk_purple_focus ),
            };

        private FunctionData[] functionNameTool;

        private FunctionData[] functionNameHelp;

        private string oldTitle = "";

        public frmADCT3000()
        {
            this.InitializeComponent();
            MdiClient client = new MdiClient();
            base.Controls.Add(client);

            functionNameTool = new FunctionData[] 
            { 
                new FunctionData( "암호변경", "cmdChangePasswor", null, global::Properties.Resources.keys, cmdChangePasswor_Click, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "관리자변경", "cmdEditOperator", null, global::Properties.Resources.Change_Account, cmdEditOperator_Click, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "관리자관리", "cmdOperatorManage", "Basic.dfrmOperator", global::Properties.Resources.Operator_Management, null, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "옵션", "mnuOption", null, global::Properties.Resources.Option, mnuOption_Click, def_bk_normal, def_bk_focus ), 
                new FunctionData( "화면잠금", "mnuInterfaceLock", null, global::Properties.Resources.Lock_Screen, mnuInterfaceLock_Click, def_bk_normal, def_bk_focus ), 
            };

            functionNameHelp = new FunctionData[]
            {
                new FunctionData( "제품정보", "mnuAbout", "Basic.dfrmAbout", global::Properties.Resources.About, null, bk_purple_normal, bk_purple_focus ), 
                new FunctionData( "사용설명서", "mnuManual", null, global::Properties.Resources.User_Guide, mnuManual_Click, bk_purple_normal, bk_purple_focus ), 
            };
        }

        private void btnAddController_Click(object sender, EventArgs e)
        {
            this.dispDfrm(null);
            using (dfrmNetControllerConfig config = new dfrmNetControllerConfig())
            {
                config.ShowDialog(this);
            }
        }

        private void btnAddPrivilege_Click(object sender, EventArgs e)
        {
            this.dispDfrm(null);
            using (dfrmPrivilege privilege = new dfrmPrivilege())
            {
                privilege.ShowDialog(this);
            }
        }

        private void btnAutoAddCardBySwiping_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedItem = null;
            if (e is ToolStripItemClickedEventArgs)
            {
                clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
            }

            if (clickedItem == null)
                this.dispDfrm(null);

            try
            {
                using (dfrmUser add = new dfrmUser())
                {
                    //add.bAutoAddBySwiping = true;
                    add.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }

            if (clickedItem != null)
            {
                this.btnBookmarkSelected = null;
                clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
            } 
        }

        private void btnIconBasicConfig_Click(object sender, EventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            if ((this.btnIconSelected == null) || (button != this.btnIconSelected))
            {
                this.btnIconSelected = button;
                foreach (object obj2 in this.panel1.Controls)
                {
                    if (obj2 is ToggleButton)
                    {
                        (obj2 as ToggleButton).BackgroundImage = null;
                        (obj2 as ToggleButton).Toggled = false;
                    }
                }
                button.Toggled = true;
                this.closeChildForm();
                foreach (ToolStripButton button2 in this.toolStrip1BookMark.Items)
                {
                    if (button2.Tag is FunctionData)
                        button2.BackColor = ((FunctionData)button2.Tag).bkcolor_normal;
                    else
                        button2.BackColor = Color.Transparent;
                    
                    button2.Image = null;
                }
                if (icOperator.PCSysInfo(false).IndexOf(": \r\nMicrosoft Windows 7 ") <= 0)
                {
                    foreach (ToolStripButton button3 in this.toolStrip1BookMark.Items)
                    {
                        button3.TextAlign = ContentAlignment.MiddleCenter;
                    }
                }
                this.btnBookmarkSelected = null;
                FunctionData[] functionNameBasicConfigure = null;
                if (wgTools.SetObjToStr(button.Tag) == "QuickConfig")
                {
                    //functionNameBasicConfigure = this.functionNameQuickConfig;
                }                
                if (wgTools.SetObjToStr(button.Tag) == "BasciConfig")
                {
                    functionNameBasicConfigure = this.functionNameBasicConfigure;
                }
                if (wgTools.SetObjToStr(button.Tag) == "BasicOperate")
                {
                    functionNameBasicConfigure = this.functionNameBasicOperate;
                }
                if (button.Tag.ToString() == "Attendance")
                {
                    functionNameBasicConfigure = this.functionNameAttendence;
                }
                if (button.Tag.ToString() == "Tools")
                {
                    functionNameBasicConfigure = this.functionNameTool;
                }
                if (button.Tag.ToString() == "Help")
                {
                    functionNameBasicConfigure = this.functionNameHelp;
                }

                foreach (object obj3 in this.toolStrip1BookMark.Items)
                {
                    (obj3 as ToolStripButton).Visible = false;
                }
                if (functionNameBasicConfigure != null)
                {
                    int num = 0;
                    for (int i = 0; (i < functionNameBasicConfigure.Length) && (i < this.toolStrip1BookMark.Items.Count); i++)
                    {
                        if (!string.IsNullOrEmpty(functionNameBasicConfigure[i].menuId))
                        {
                            this.toolStrip1BookMark.Items[num].Text = CommonStr.ResourceManager.GetString("strFunctionDisplayName_" + functionNameBasicConfigure[i].menuId);
                            this.toolStrip1BookMark.Items[num].Text = wgAppConfig.ReplaceFloorRomm(this.toolStrip1BookMark.Items[num].Text);
                            this.toolStrip1BookMark.Items[num].Tag = functionNameBasicConfigure[i];
                            this.toolStrip1BookMark.Items[num].Image = functionNameBasicConfigure[i].icon;
                            this.toolStrip1BookMark.Items[num].BackColor = functionNameBasicConfigure[i].bkcolor_normal;
                            this.toolStrip1BookMark.Items[num].Visible = true;
                            num++;
                        }
                    }
                }
                if ((wgTools.SetObjToStr(button.Tag) == "BasicOperate") && this.shortcutConsole.Enabled)
                {
                    this.shortcutConsole.PerformClick();
                }
            }
        }

        private void chkHideLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (wgAppConfig.GetKeyVal("HideGettingStartedWhenLogin") != "1")
            {
                if (false && this.bDisplayHideLogin)
                {
                    XMessageBox.Show(CommonStr.strDisplayHideLogin);
                }
                this.bDisplayHideLogin = false;
            }
            wgAppConfig.UpdateKeyVal("HideGettingStartedWhenLogin", "0");
        }

        private void closeChildForm()
        {
            if (this.panel4Form.Controls.Count > 0)
            {
                (this.panel4Form.Controls[0] as Form).Close();
            }
            this.statRunInfo_Num_Update("");
            this.statRunInfo_CommStatus_Update("");
            this.statRunInfo_Monitor_Update("");
        }

        private void cmdChangePasswor_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedItem = null;
            if (e is ToolStripItemClickedEventArgs)
            {
                clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
            }

            if (clickedItem == null)
                this.dispDfrm(null);

            this.dfrmSetPassword1 = new dfrmSetPassword();
//             this.dfrmSetPassword1.Text = this.cmdChangePasswor.Text.Replace('&', ' ');   // TODO KGH
            this.dfrmSetPassword1.operatorID = icOperator.OperatorID;
            dfrmSetPassword1.ShowDialog();

            if (clickedItem != null)
            {
                this.btnBookmarkSelected = null;
                clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
            }
        }

        private void cmdEditOperator_Click(object sender, EventArgs e)
        {
            using (dfrmOperatorUpdate update = new dfrmOperatorUpdate())
            {
                ToolStripButton clickedItem = null;
                if (e is ToolStripItemClickedEventArgs)
                {
                    clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
                }

                if (clickedItem == null)
                    this.dispDfrm(null);

                update.operateMode = 1;
                update.operatorID = icOperator.OperatorID;
                update.operatorName = icOperator.OperatorName;
                update.ShowDialog(this);
                icOperator.OperatorName = update.operatorName;

                if (clickedItem != null)
                {
                    this.btnBookmarkSelected = null;
                    clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
                }
            }
        }

        private void cmdOperatorManage_Click(object sender, EventArgs e)
        {
            this.dfrmOperator1 = new dfrmOperator();
            this.dispDfrm(this.dfrmOperator1);
        }

        private void dispDfrm(Form dfrm)
        {
            this.closeChildForm();
            foreach (object obj2 in this.panel1.Controls)
            {
                if (obj2 is Button)
                {
                    (obj2 as Button).BackColor = Color.FromArgb(43, 124, 170);
                }
            }

            foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
            {
                button.BackColor = ((FunctionData)button.Tag).bkcolor_normal;
                button.Visible = false;
            }
            this.btnIconSelected = null;
            this.btnBookmarkSelected = null;
            wgAppRunInfo.ClearAllDisplayedInfo();
            if (dfrm != null)
            {
                dfrm.ShowDialog(this);
            }
        }

        private void dispInPanel4(Form frm)
        {
            this.closeChildForm();
            foreach (object obj2 in this.panel1.Controls)
            {
                if (obj2 is Button)
                {
                    (obj2 as Button).BackColor = Color.FromArgb(43, 124, 170);
                }
            }

            foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
            {
                button.BackColor = ((FunctionData)button.Tag).bkcolor_normal;
                button.Visible = false;
            }
            this.btnIconSelected = null;
            this.btnBookmarkSelected = this.toolStripButtonBookmark1;
            frm.ShowDialog();
        }

        private void frmADCT3000_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.bConfirmClose)
            {
                if (XMessageBox.Show(CommonStr.strExit + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.closeChildForm();
                    wgAppConfig.wgLog(CommonStr.strExit, EventLogEntryType.Information, null);
                }
            }
        }

        private void frmADCT3000_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && !e.Shift) && (e.KeyValue == 0x54))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.frmTestController1 = new frmTestController();
                this.frmTestController1.Owner = this;
                this.frmTestController1.Show();
            }
            if ((!e.Control && !e.Shift) && (e.KeyValue == 0x70))
            {
                mnuManual_Click(sender, e);
            }
            if ((e.Control && e.Shift) && (e.KeyValue == 0x4e))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.dfrmNetControllerConfig1 = new dfrmNetControllerConfig();
                this.dfrmNetControllerConfig1.Show();
            }
            if ((!e.Control && e.Shift) && (e.KeyValue == 0x76))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.mnuDeleteOldRecords_Click(null, null);
            }
            if ((!e.Control && e.Shift) && (e.KeyValue == 0x77))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.systemParamsToolStripMenuItem_Click(null, null);
            }
            if ((!e.Control && e.Shift) && (e.KeyValue == 0x7b))
            {
                if (icOperator.OperatorID != 1)
                {
                    XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                this.systemParamsCustomTitle();
            }
            if (((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72)) && (this.panel4Form.Controls.Count > 0))
            {
                try
                {
                    Form form = this.panel4Form.Controls[0] as Form;
                    if (this.btnBookmarkSelected != null)
                    {
                        if (this.btnBookmarkSelected.Tag.ToString().IndexOf(".frmControllers") > 0)
                        {
                            (form as frmControllers).frmControllers_KeyDown(form, e);
                        }
                        if (this.btnBookmarkSelected.Tag.ToString().IndexOf(".frmConsole") > 0)
                        {
                            (form as frmConsole).frmConsole_KeyDown(form, e);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void frmADCT3000_Load(object sender, EventArgs e)
        {
            wgAppConfig.bFloorRoomManager = wgAppConfig.getParamValBoolByNO(0x91);
            this.toolStripButtonBookmark2.Text = wgAppConfig.ReplaceFloorRomm(this.toolStripButtonBookmark2.Text);
            Icon appicon = base.Icon;
            wgAppConfig.GetAppIcon(ref appicon);
            base.Icon = appicon;
            base.KeyPreview = true;
            Application.ThreadException += new ThreadExceptionEventHandler(Program.GlobalExceptionHandler);
            UserControlFind.blogin = true;
            UserControlFindSecond.blogin = true;
            Application.ThreadException += new ThreadExceptionEventHandler(Program.GlobalExceptionHandler);

            toolStrip1BookMark.Renderer = new MyCustomRenderer();
            this.hideMenuBySystemConfig();
            this.hideMenuByUserPrivilege();
            bool flag = false;
            foreach (object obj2 in this.panel1.Controls)
            {
                if ((obj2 is Button) && (obj2 as Button).Visible)
                {
                    flag = false;
                    (obj2 as Button).PerformClick();
                    break;
                }
            }

            if (flag)
            {
                XMessageBox.Show(CommonStr.strOperatorHaveNoPrivilege);
                this.bConfirmClose = true;
                ProgramExit();
            }
            else
            {
                UserControlFind.blogin = true;
                UserControlFindSecond.blogin = true;
/*
                if (wgAppConfig.getParamValBoolByNO(0x89) && (icOperator.OperatorID == 1))
                {
                    this.mnuPCCheckAccessConfigure.Visible = true;
                }
                if (!wgAppConfig.getParamValBoolByNO(0x90))
                {
                    this.mnuElevator.Visible = false;
                }
                else
                {
                    using (dfrmOneToMoreSetup setup = new dfrmOneToMoreSetup())
                    {
                        if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 2)
                        {
                            this.mnuElevator.Text = setup.radioButton1.Text;
                        }
                        else if ((int.Parse("0" + wgAppConfig.getSystemParamByNO(0x90)) & 0xff) == 3)
                        {
                            this.mnuElevator.Text = setup.radioButton2.Text;
                        }
                        else
                        {
                            this.mnuElevator.Text = setup.radioButton0.Text;
                        }
                    }
                }
*/
                if (!wgAppConfig.getParamValBoolByNO(0x89) && (icOperator.OperatorID == 1))
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "mnuPCCheckAccessConfigure")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }

                if (!wgAppConfig.getParamValBoolByNO(0x95))
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "mnuMeeting")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(150))
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "mnuConstMeal")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(0x97))
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "mnuPatrolDetailData")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(0x92) || (icOperator.OperatorID != 1))
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "mnuDoorAsSwitch")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
                if (!wgAppConfig.getParamValBoolByNO(0x94))
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "cmdOperatorManage")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
                this.Text = wgAppConfig.LoginTitle;
                this.loadStbRunInfo();
                wgAppRunInfo.evAppRunInfoLoadNum += new wgAppRunInfo.appRunInfoLoadNumHandler(this.statRunInfo_Num_Update);
                wgAppRunInfo.evAppRunInfoCommStatus += new wgAppRunInfo.appRunInfoCommStatusHandler(this.statRunInfo_CommStatus_Update);
                wgAppRunInfo.evAppRunInfoMonitor += new wgAppRunInfo.appRunInfoMonitorHandler(this.statRunInfo_Monitor_Update);
                base.WindowState = FormWindowState.Maximized;
                this.timer1.Enabled = true;
                string strSql = "SELECT COUNT(*) from t_s_Operator ";
                if (int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getValBySql(strSql))) > 1)
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "cmdEditOperator")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (FunctionData func in functionNameTool)
                    {
                        if (func.menuId == "cmdChangePasswor")
                        {
                            func.menuId = null;
                            break;
                        }
                    }
                }
            }
        }

        private void hideFuncItem(ref FunctionData[] func, string funcName, bool bNotHide)
        {
            if (!bNotHide)
            {
                for (int i = 0; i < func.Length; i++)
                {
                    if (!string.IsNullOrEmpty(func[i].menuId) && (func[i].menuId == funcName))
                    {
                        func[i].menuId = null;
                        return;
                    }
                }
            }
        }

        private void hideMenuBySystemConfig()
        {
            if (this.btnIconTools.Visible)
            {
                this.hideFuncItem(ref this.functionNameTool, "mnuLogQuery", wgAppConfig.getParamValBoolByNO(0x67));
            }
            this.btnIconAttendance.Visible = !wgAppConfig.getParamValBoolByNO(0x70);
            if (this.btnIconAttendance.Visible)
            {
                this.hideFuncItem(ref this.functionNameAttendence, "mnuShiftArrange", wgAppConfig.getParamValBoolByNO(0x71));
                this.hideFuncItem(ref this.functionNameAttendence, "mnuShiftRule", wgAppConfig.getParamValBoolByNO(0x71));
                this.hideFuncItem(ref this.functionNameAttendence, "mnuShiftSet", wgAppConfig.getParamValBoolByNO(0x71));
            }
        }

        private void hideMenuByUserPrivilege()
        {
            icOperator.OperatePrivilegeLoad(ref this.functionNameBasicConfigure);
            icOperator.OperatePrivilegeLoad(ref this.functionNameBasicOperate);
            icOperator.OperatePrivilegeLoad(ref this.functionNameAttendence);
            icOperator.OperatePrivilegeLoad(ref this.functionNameTool);
            icOperator.OperatePrivilegeLoad(ref this.functionNameHelp);
            if (!icOperator.OperatePrivilegeVisible("mnu1BasicConfigure"))
            {
                this.btnIconBasicConfig.Visible = false;
            }
            if (!icOperator.OperatePrivilegeVisible("mnu1BasicOperate"))
            {
                this.btnIconBasicOperate.Visible = false;
            }
            if (!icOperator.OperatePrivilegeVisible("mnu1Attendence"))
            {
                this.btnIconAttendance.Visible = false;
            }
            if (!icOperator.OperatePrivilegeVisible("mnu1Tool"))
            {
                this.btnIconTools.Visible = false;
            }
            if (!icOperator.OperatePrivilegeVisible("mnu1Help"))
            {
                this.btnIconHelp.Visible = false;
            }
            Button[] menuFirstButtons = new Button[] {
                this.btnIconBasicConfig,
                this.btnIconBasicOperate,
                this.btnIconAttendance,
                this.btnIconTools,
                this.btnIconHelp
            };
			int left = this.btnIconBasicConfig.Left;
			int top = this.btnIconBasicConfig.Top;
            for (int i = 0; i < menuFirstButtons.Length; ++ i)
            {
                if (menuFirstButtons[i].Visible)
                {
                    menuFirstButtons[i].Left = left;
                    menuFirstButtons[i].Top = top;
                    left += menuFirstButtons[i].Width;
                }
            }

            if (!this.btnIconBasicConfig.Visible)
            {
                this.panel1.Controls.Remove(this.btnIconBasicConfig);
                this.btnIconBasicConfig.Dispose();
                this.shortcutControllers.Visible = false;
                this.shortcutPersonnel.Visible = false;
            }
            if (!this.btnIconBasicOperate.Visible)
            {
                this.panel1.Controls.Remove(this.btnIconBasicOperate);
                this.btnIconBasicOperate.Dispose();
                this.shortcutConsole.Visible = false;
                this.shortcutSwipe.Visible = false;
            }
            if (!this.btnIconAttendance.Visible)
            {
                this.panel1.Controls.Remove(this.btnIconAttendance);
                this.btnIconAttendance.Dispose();
                this.shortcutAttendance.Visible = false;
            }
            if (!this.btnIconTools.Visible)
            {
                this.panel1.Controls.Remove(this.btnIconTools);
                this.btnIconTools.Dispose();
            }
            if (!this.btnIconHelp.Visible)
            {
                this.panel1.Controls.Remove(this.btnIconHelp);
                this.btnIconHelp.Dispose();
            }
            if (this.functionNameBasicConfigure[0].menuId == null)
            {
                this.shortcutControllers.Visible = false;
            }
            if (this.functionNameBasicConfigure[2].menuId == null)
            {
                this.shortcutPersonnel.Visible = false;
            }
            if (this.functionNameBasicOperate[0].menuId == null)
            {
                this.shortcutConsole.Visible = false;
            }
            if (this.functionNameBasicOperate[1].menuId == null)
            {
                this.shortcutSwipe.Visible = false;
            }
            if (this.functionNameAttendence[0].menuId == null)
            {
                this.shortcutAttendance.Visible = false;
            }
        }

        private void loadStbRunInfo()
        {
            if (icOperator.OperatorID == 1)
            {
                this.statOperator.Text = string.Format("{0}:{1}", CommonStr.strSuper, icOperator.OperatorName);
            }
            else
            {
                this.statOperator.Text = string.Format("{0}", icOperator.OperatorName);
            }
            string str = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf("."));
            str = str.Substring(0, str.LastIndexOf("."));
            if (wgAppConfig.IsAccessDB)
            {
                this.statSoftwareVer.Text = string.Format("{0} - Ver: {1}", "Access", str);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                {
                    this.statSoftwareVer.Text = string.Format("SQL: {0} - Ver: {1}", connection.Database, str);
                }
            }
            string str2 = "BLUE";
            string productTypeOfApp = wgAppConfig.ProductTypeOfApp;
            if (productTypeOfApp != null)
            {
                if (!(productTypeOfApp == "Adroitor"))
                {
                    if (productTypeOfApp == "WGACCESS")
                    {
                        str2 = "WG";
                    }
                    else if (productTypeOfApp == "ADCT")
                    {
                        str2 = "ADCT";
                    }
                }
                else
                {
                    str2 = "AT";
                }
            }
            this.statSoftwareVer.Text = this.statSoftwareVer.Text.Replace(" - Ver: ", string.Format(" -{0}- Ver: ", str2));
            string[] strArray = Application.ProductVersion.Split(new char[] { '.' });
            if ((strArray.Length >= 4) && ((int.Parse(strArray[1]) % 2) == 0))
            {
                this.statSoftwareVer.Text = this.statSoftwareVer.Text + "." + strArray[3].ToString();
            }
            wgTools.CommPStr = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommCurrent"));
            if (!string.IsNullOrEmpty(wgTools.CommPStr))
            {
                this.statSoftwareVer.Text = this.statSoftwareVer.Text + ":!s";
            }
            this.statCOM.Text = "";
            this.statRuninfo1.Text = "";
            this.statRuninfo1.Spring = true;
            this.statRuninfo2.Text = "";
            this.statRuninfo3.Text = "";
            this.statRuninfo3.AutoSize = false;
            this.statRuninfo3.Width = 0x30;
            this.statRuninfoLoadedNum.Text = "";
            this.statRuninfoLoadedNum.AutoSize = false;
            this.statRuninfoLoadedNum.Width = 0x89;
            this.statTimeDate.Text = DateTime.Now.ToString(wgTools.YMDHMSFormat);
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            this.dfrmAbout1 = new dfrmAbout();
            this.dfrmAbout1.Owner = this;
            dfrmAbout1.ShowDialog();
        }

        private void mnuDBBackup_Click(object sender, EventArgs e)
        {
            this.dfrmDbCompact1 = new dfrmDbCompact();
            this.dispDfrm(this.dfrmDbCompact1);
        }

        private void mnuDeleteOldRecords_Click(object sender, EventArgs e)
        {
            if (icOperator.OperatorID != 1)
            {
                XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                using (dfrmInputNewName name = new dfrmInputNewName())
                {
                    name.setPasswordChar('*');
                    if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "5678"))
                    {
                        return;
                    }
                }
                using (dfrmDeleteRecords records = new dfrmDeleteRecords())
                {
                    records.ShowDialog(this);
                }
            }
        }

        private void mnuDoorAsSwitch_Click(object sender, EventArgs e)
        {
            this.dispDfrm(null);
            using (dfrmDoorAsSwitch switch2 = new dfrmDoorAsSwitch())
            {
                switch2.ShowDialog();
            }
        }

        private void ProgramExit()
        {
            this.bConfirmClose = true;
            wgAppConfig.wgLog(CommonStr.strExit, EventLogEntryType.Information, null);
            base.Close();
        }

        private void mnuExtendedFunction_Click(object sender, EventArgs e)
        {
            /* TODOJ
            using (dfrmInputNewName name = new dfrmInputNewName())
            {
                name.setPasswordChar('*');
                if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "5678"))
                {
                    return;
                }
            }*/
            this.dispDfrm(null);
            using (dfrmExtendedFunctions functions = new dfrmExtendedFunctions())
            {
                if (functions.ShowDialog(this) == DialogResult.OK)
                {
                    wgAppConfig.gRestart = true;
                    ProgramExit();
                }
            }
        }

        private void mnuInterfaceLock_Click(object sender, EventArgs e)
        {
            using (dfrmOperatorUpdate update = new dfrmOperatorUpdate())
            {
                ToolStripButton clickedItem = null;
                if (e is ToolStripItemClickedEventArgs)
                {
                    clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
                }

                using (dfrmInterfaceLock @lock = new dfrmInterfaceLock())
                {
                    @lock.txtOperatorName.Text = icOperator.OperatorName;
                    @lock.StartPosition = FormStartPosition.CenterScreen;
                    @lock.ShowDialog(this);
                }

                if (clickedItem != null)
                {
                    this.btnBookmarkSelected = null;
                    clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
                }
            }
        }

        private void mnuUpgradeFirmware_Click(object sender, EventArgs e)
        {
            using (dfrmUpgradeFirmware update = new dfrmUpgradeFirmware())
            {
                ToolStripButton clickedItem = null;
                if (e is ToolStripItemClickedEventArgs)
                {
                    clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
                }

                using (dfrmUpgradeFirmware @lock = new dfrmUpgradeFirmware())
                {
                    @lock.StartPosition = FormStartPosition.CenterScreen;
                    @lock.ShowDialog(this);
                }

                if (clickedItem != null)
                {
                    this.btnBookmarkSelected = null;
                    clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
                }
            }
        }

        private void mnuLogQuery_Click(object sender, EventArgs e)
        {
            this.dfrmLogQuery1 = new dfrmLogQuery();
            this.dispDfrm(this.dfrmLogQuery1);
        }

        private void mnuManual_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedItem = null;
            if (e is ToolStripItemClickedEventArgs)
            {
                clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Environment.CurrentDirectory + @"\BioAccess.chm";
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }

            if (clickedItem != null)
            {
                this.btnBookmarkSelected = null;
                clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
            }
        }

        private void mnuMeal_Click(object sender, EventArgs e)
        {
            this.dispInPanel4(new frmMeal());
        }

        private void mnuMeetingSign_Click(object sender, EventArgs e)
        {
            this.dispDfrm(null);
            using (frmMeetings meetings = new frmMeetings())
            {
                meetings.ShowDialog();
            }
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedItem = null;
            if (e is ToolStripItemClickedEventArgs)
            {
                clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
            }

            if (clickedItem == null)
                this.dispDfrm(null);

            using (dfrmOption option = new dfrmOption())
            {
                if (option.ShowDialog(this) == DialogResult.OK)
                {
                    wgAppConfig.gRestart = true;
                    ProgramExit();
                }
            } 

            if (clickedItem != null)
            {
                this.btnBookmarkSelected = null;
                clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
            }
        }

        private void mnuPatrol_Click(object sender, EventArgs e)
        {
            this.dispInPanel4(new frmPatrolReport());
        }

        private void mnuPCCheckAccessConfigure_Click(object sender, EventArgs e)
        {
            this.dfrmCheckAccessConfigure1 = new dfrmCheckAccessConfigure();
            this.dispDfrm(this.dfrmCheckAccessConfigure1);
        }

        private void mnuSystemCharacteristic_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripButton clickedItem = null;
                if (e is ToolStripItemClickedEventArgs)
                {
                    clickedItem = ((ToolStripItemClickedEventArgs)e).ClickedItem as ToolStripButton;
                }
                 
                if (clickedItem == null)
                    this.dispDfrm(null);
                string text = icOperator.PCSysInfo(false);
                XMessageBox.Show(text, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                string eName = "";
                string str3 = "";
                wgAppConfig.setSystemParamValue(0x26, eName, str3, text);

                if (clickedItem != null)
                {
                    this.btnBookmarkSelected = null;
                    clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
            }
        }

        private void mnuTaskList_Click(object sender, EventArgs e)
        {
            this.dfrmControllerTaskList1 = new dfrmControllerTaskList();
            this.dispDfrm(this.dfrmControllerTaskList1);
        }

        private void shortcutAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnIconAttendance.PerformClick();
                foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
                {
                    if (string.Compare((button.Tag as FunctionData).funcId, "Reports.Shift.frmShiftAttReport") == 0)
                    {
                        button.PerformClick();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void shortcutConsole_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnIconBasicOperate.PerformClick();
                foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
                {
                    if (string.Compare((button.Tag as FunctionData).funcId, "Basic.frmConsole") == 0)
                    {
                        button.PerformClick();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void shortcutControllers_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnIconBasicConfig.PerformClick();
                foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
                {
                    if (string.Compare((button.Tag as FunctionData).funcId, "Basic.frmControllers") == 0)
                    {
                        button.PerformClick();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void shortcutPersonnel_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnIconBasicConfig.PerformClick();
                foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
                {
                    if (string.Compare((button.Tag as FunctionData).funcId, "Basic.frmUsers") == 0)
                    {
                        button.PerformClick();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void shortcutPrivilege_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
                {
                    if (string.Compare((button.Tag as FunctionData).funcId, "Basic.frmPrivileges") == 0)
                    {
                        button.PerformClick();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void shortcutSwipe_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnIconBasicOperate.PerformClick();
                foreach (ToolStripButton button in this.toolStrip1BookMark.Items)
                {
                    if (string.Compare((button.Tag as FunctionData).funcId, "Basic.frmSwipeRecords") == 0)
                    {
                        button.PerformClick();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void statRunInfo_CommStatus_Update(string strCommStatus)
        {
            try
            {
                this.statRuninfo1.Text = strCommStatus;
            }
            catch (Exception)
            {
            }
        }

        private void statRunInfo_Monitor_Update(string strMonitor)
        {
            try
            {
                string str = strMonitor;
                if (str == null)
                {
                    goto Label_0074;
                }
                if (!(str == "0"))
                {
                    if (str == "1")
                    {
                        goto Label_0050;
                    }
                    if (str == "2")
                    {
                        goto Label_0062;
                    }
                    goto Label_0074;
                }
                this.statRuninfo2.BackColor = Color.Transparent;
                this.statRuninfo2.Text = CommonStr.strMonitorStop;
                return;
            Label_0050:
                this.statRuninfo2.Text = CommonStr.strMonitoring;
                return;
            Label_0062:
                this.statRuninfo2.Text = CommonStr.strRealtimeGetting;
                return;
            Label_0074:
                this.statRuninfo2.Text = strMonitor;
            }
            catch (Exception)
            {
            }
        }

        private void statRunInfo_Num_Update(string strLoadNum)
        {
            try
            {
                this.statRuninfoLoadedNum.Text = strLoadNum;
            }
            catch (Exception)
            {
            }
        }

        private void systemParamsCustomTitle()
        {
            if (icOperator.OperatorID != 1)
            {
                XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                using (dfrmInputNewName name = new dfrmInputNewName())
                {
                    name.setPasswordChar('*');
                    if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "5678"))
                    {
                        return;
                    }
                }
                using (dfrmInputNewName name2 = new dfrmInputNewName())
                {
                    name2.Text = CommonStr.strNewTitle;
                    name2.bNotAllowNull = false;
                    if ((name2.ShowDialog(this) == DialogResult.OK) && (wgAppConfig.setSystemParamValue(0x11, "", wgTools.SetObjToStr(name2.strNewName).Trim(), "") > 0))
                    {
                        if (wgAppConfig.getSystemParamByName("Custom Title") != "")
                        {
                            this.Text = wgAppConfig.getSystemParamByName("Custom Title");
                            this.oldTitle = this.Text;
                        }
                        else
                        {
                            this.Text = this.defaultTitle;
                        }
                    }
                }
            }
        }

        private void systemParamsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (icOperator.OperatorID != 1)
            {
                XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                using (dfrmInputNewName name = new dfrmInputNewName())
                {
                    name.setPasswordChar('*');
                    if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "5678"))
                    {
                        return;
                    }
                }
                using (dfrmSystemParam param = new dfrmSystemParam())
                {
                    if (param.ShowDialog(this) == DialogResult.OK)
                    {
                        wgAppConfig.gRestart = true;
                        ProgramExit();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timer1.Enabled = false;
                this.statTimeDate.Text = DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
            }
            catch (Exception)
            {
            }
            finally
            {
                this.timer1.Enabled = true;
            }
        }

        private void toolStrip1BookMark_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripButton clickedItem = e.ClickedItem as ToolStripButton;
            if ((this.btnBookmarkSelected == null) || (clickedItem != this.btnBookmarkSelected))
            {
                this.btnBookmarkSelected = clickedItem;
                foreach (ToolStripButton button2 in this.toolStrip1BookMark.Items)
                {
                    if (button2.Tag is FunctionData)
                        button2.BackColor = ((FunctionData)button2.Tag).bkcolor_normal;
                    else
                        button2.BackColor = Color.Transparent;
                }
                clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_focus;
                this.closeChildForm();
                Form form = null;
                FunctionData func = (FunctionData)clickedItem.Tag;
                if (func == null)
                    return;

                string functionName = "WG3000_COMM." + func.funcId;
                if (!string.IsNullOrEmpty(wgTools.SetObjToStr(func.funcId)))
                {
                    form = (Form)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType(functionName));
                }
                if (form == null)
                {
                    if (func.handler != null)
                        func.handler(sender, e);
                }
                else
                {
                    if (functionName.ToString().IndexOf(".dfrm") >= 0)
                    {
                        DialogResult res = form.ShowDialog(this);
                        if (functionName == @"WG3000_COMM.Basic.dfrmExtendedFunctions" &&
                            res == DialogResult.OK)
                        {
                            wgAppConfig.gRestart = true;
                            ProgramExit();
                        }
                        else
                        {
                            this.btnBookmarkSelected = null;
                            clickedItem.BackColor = ((FunctionData)clickedItem.Tag).bkcolor_normal;
                        }
                    }
                    else
                    {
                        form.Location = new Point(-4, -32);
                        form.ControlBox = false;
                        form.WindowState = FormWindowState.Normal;
                        form.MdiParent = this;
                        Cursor.Current = Cursors.WaitCursor;
                        form.FormBorderStyle = FormBorderStyle.None;
                        form.StartPosition = FormStartPosition.Manual;
                        form.Show();
                        form.Dock = DockStyle.Fill;
                        Cursor.Current = Cursors.Default;
                        this.panel4Form.Controls.Add(form);
                        if (this.btnIconSelected != null)
                        {
                            this.btnIconSelected.Select();
                        }
                    }
                }
            }
        }
    }

    public class FunctionData
    {
        public string label;
        public string menuId;
        public string funcId;
        public Bitmap icon;
        public EventHandler handler;
        public Color bkcolor_normal;
        public Color bkcolor_focus;

        public FunctionData(string label, 
                            string menuId, 
                            string funcId, 
                            Bitmap icon, 
                            EventHandler handler,
                            Color bknormal,
                            Color bkfocus)
        {
            this.label  = label;
            this.menuId = menuId;
            this.funcId = funcId;
            this.icon   = icon;
            this.handler= handler;
            this.bkcolor_normal = bknormal;
            this.bkcolor_focus = bkfocus;
        }
    }

    public class MyCustomRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected)
                base.OnRenderButtonBackground(e);
            else
            {
                Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width, e.Item.Size.Height);
                if (e.Item.Tag is FunctionData)
                {
                    FunctionData func = e.Item.Tag as FunctionData;
                    using (SolidBrush brush = new SolidBrush(func.bkcolor_focus))
                    {
                        e.Graphics.FillRectangle(brush, rectangle);
                    }                    
                }
            }
        }
    }

}

