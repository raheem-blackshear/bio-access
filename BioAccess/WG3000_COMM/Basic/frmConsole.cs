namespace WG3000_COMM.Basic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Media;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ExtendFunc;
    using WG3000_COMM.ExtendFunc.Map;
    using WG3000_COMM.ExtendFunc.PCCheck;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    public partial class frmConsole : Form
    {
        private Dictionary<int, int> arrDealtController = new Dictionary<int, int>();
        public ArrayList arrSelectDoors4Sign = new ArrayList();
        private ArrayList arrSelectedDoors = new ArrayList();
        private ArrayList arrSelectedDoorsItem = new ArrayList();
        private ArrayList arrZoneID = new ArrayList();
        private ArrayList arrZoneName = new ArrayList();
        private ArrayList arrZoneNO = new ArrayList();
        private bool bDirectToRealtimeGet;
        private BackgroundWorker bkDispDoorStatus;
        private BackgroundWorker bkRealtimeGetRecords;
        private BackgroundWorker bkUploadAndGetRecords;
        private byte[] blankImage = new byte[] { 
            0x42, 0x4d, 0xc6, 0, 0, 0, 0, 0, 0, 0, 0x76, 0, 0, 0, 40, 0, 
            0, 0, 11, 0, 0, 0, 10, 0, 0, 0, 1, 0, 4, 0, 0, 0, 
            0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x10, 0, 
            0, 0, 0x10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x80, 0, 0, 0x80, 
            0, 0, 0, 0x80, 0x80, 0, 0x80, 0, 0, 0, 0x80, 0, 0x80, 0, 0x80, 0x80, 
            0, 0, 0xc0, 0xc0, 0xc0, 0, 0x80, 0x80, 0x80, 0, 0, 0, 0xff, 0, 0, 0xff, 
            0, 0, 0, 0xff, 0xff, 0, 0xff, 0, 0, 0, 0xff, 0, 0xff, 0, 0xff, 0xff, 
            0, 0, 0xff, 0xff, 0xff, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 
            0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 
            0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 
            0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 
            0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 240, 0, 0, 0xff, 0xff, 
            0xff, 0xff, 0xff, 240, 0, 0
         };
        public bool bMainWindowDisplay = true;
        private bool bNeedCheckLosePacket;
        private bool bPCCheckAccess;
        private bool bStopComm;
        private ArrayList checkAccess_arrCardId = new ArrayList();
        private ArrayList checkAccess_arrCheckStartTime = new ArrayList();
        private ArrayList checkAccess_arrCheckTime = new ArrayList();
        private ArrayList checkAccess_arrConsumerName = new ArrayList();
        private ArrayList checkAccess_arrCount = new ArrayList();
        private ArrayList checkAccess_arrDB_GroupName = new ArrayList();
        private ArrayList checkAccess_arrDB_MoreCards = new ArrayList();
        private ArrayList checkAccess_arrDoor = new ArrayList();
        private ArrayList checkAccess_arrDoorName = new ArrayList();
        private ArrayList checkAccess_arrGroupName = new ArrayList();
        private ArrayList checkAccess_arrReaderNo = new ArrayList();
        private SqlCommand cm4ParamPrivilege;
        private string CommOperate = "";
        private int CommOperateOption;
        private icController control4btnServer;
        private icController control4Check;
        private icController control4getRecordsFromController;
        private icController control4Realtime;
        private icController control4uploadPrivilege = new icController();
        private wgMjControllerConfigure controlConfigure4uploadPrivilege = new wgMjControllerConfigure();
        private wgMjControllerHolidaysList controlHolidayList4uploadPrivilege = new wgMjControllerHolidaysList();
        private wgMjControllerTaskList controlTaskList4uploadPrivilege = new wgMjControllerTaskList();
        private static int dealingTxt;
        private int dealtDoorIndex;
        private int dealtIndexOfDoorsNeedToGetRecords = -1;
        private ArrayList doorsNeedToGetRecords = new ArrayList();
        private static int infoRowsCount;
        public const int MODE_Check = 1;
        public const int MODE_GetRecords = 5;
        public const int MODE_RemoteOpen = 6;
        public const int MODE_Server = 4;
        public const int MODE_SetTime = 2;
        public const int MODE_Upload = 3;
        private Dictionary<int, int> needDelSwipeControllers;
        private byte[] oImage = new byte[] { 
            0x42, 0x4d, 0xc6, 0, 0, 0, 0, 0, 0, 0, 0x76, 0, 0, 0, 40, 0, 
            0, 0, 11, 0, 0, 0, 10, 0, 0, 0, 1, 0, 4, 0, 0, 0, 
            0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x10, 0, 
            0, 0, 0x10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x80, 0, 0, 0x80, 
            0, 0, 0, 0x80, 0x80, 0, 0x80, 0, 0, 0, 0x80, 0, 0x80, 0, 0x80, 0x80, 
            0, 0, 0xc0, 0xc0, 0xc0, 0, 0x80, 0x80, 0x80, 0, 0, 0, 0xff, 0, 0, 0xff, 
            0, 0, 0, 0xff, 0xff, 0, 0xff, 0, 0, 0, 0xff, 0, 0xff, 0, 0xff, 0xff, 
            0, 0, 0xff, 0xff, 0xff, 0, 0xff, 0xff, 0, 15, 0xff, 240, 0, 0, 0xff, 0, 
            0xff, 240, 15, 240, 0, 0, 240, 0xff, 0xff, 0xff, 240, 240, 0, 0, 240, 0xff, 
            0xff, 0xff, 240, 240, 0, 0, 15, 0xff, 0xff, 0xff, 0xff, 0, 0, 0, 15, 0xff, 
            0xff, 0xff, 0xff, 0, 0, 0, 240, 0xff, 0xff, 0xff, 240, 240, 0, 0, 240, 0xff, 
            0xff, 0xff, 240, 240, 0, 0, 0xff, 0, 0xff, 240, 15, 240, 0, 0, 0xff, 0xff, 
            0, 15, 0xff, 240, 0, 0
         };
        private string oldInfoTitleString;
        private icPrivilege pr4uploadPrivilege = new icPrivilege();
        private Queue QueRecText = new Queue();
        private Dictionary<string, string> ReaderName = new Dictionary<string, string>();
        private Dictionary<int, int> realtimeGetRecordsSwipeIndexGot = new Dictionary<int, int>();
        private static int receivedPktCount;
        private Dictionary<int, icController> selectedControllersOfRealtimeGetRecords;
        private ArrayList selectedControllersSNOfRealtimeGetRecords = new ArrayList();
        private string strAllProductsDriversInfo;
        private string strRealMonitor = "";
        private icSwipeRecord swipe4GetRecords = new icSwipeRecord();
        public int totalConsoleMode;
        private WatchingService watching;
        private int watchingDealtDoorIndex;
        private DateTime watchingStartTime;
        private Queue wgCommQueRecText = new Queue();
        private static int wgCommReceivedPktCount;
        private wgCommService wgCommService1;
        private byte[] xImage = new byte[] { 
            0x42, 0x4d, 0xc6, 0, 0, 0, 0, 0, 0, 0, 0x76, 0, 0, 0, 40, 0, 
            0, 0, 11, 0, 0, 0, 10, 0, 0, 0, 1, 0, 4, 0, 0, 0, 
            0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x10, 0, 
            0, 0, 0x10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x80, 0, 0, 0x80, 
            0, 0, 0, 0x80, 0x80, 0, 0x80, 0, 0, 0, 0x80, 0, 0x80, 0, 0x80, 0x80, 
            0, 0, 0xc0, 0xc0, 0xc0, 0, 0x80, 0x80, 0x80, 0, 0, 0, 0xff, 0, 0, 0xff, 
            0, 0, 0, 0xff, 0xff, 0, 0xff, 0, 0, 0, 0xff, 0, 0xff, 0, 0xff, 0xff, 
            0, 0, 0xff, 0xff, 0xff, 0, 240, 0xff, 0xff, 0xff, 240, 240, 0, 0, 0xff, 15, 
            0xff, 0xff, 15, 240, 0, 0, 0xff, 240, 0xff, 240, 0xff, 240, 0, 0, 0xff, 0xff, 
            15, 15, 0xff, 240, 0, 0, 0xff, 0xff, 15, 15, 0xff, 240, 0, 0, 0xff, 0xff, 
            15, 15, 0xff, 240, 0, 0, 0xff, 240, 0xff, 240, 0xff, 240, 0, 0, 0xff, 15, 
            0xff, 0xff, 15, 240, 0, 0, 240, 0xff, 0xff, 0xff, 240, 240, 0, 0, 0xff, 0xff, 
            0xff, 0xff, 0xff, 240, 0, 0
        };

        public frmConsole()
        {
            this.InitializeComponent();
        }

        private void bkDispDoorStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            using (icController controller = new icController())
            {
                this.watchingDealtDoorIndex = 0;
                Thread.Sleep(100);
                while (((this.watchingDealtDoorIndex > -1) && (this.watchingDealtDoorIndex < this.arrSelectedDoors.Count)) && !this.bStopComm)
                {
                    if (this.watchingDealtDoorIndex <= this.dealtDoorIndex)
                    {
                        controller.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.watchingDealtDoorIndex].ToString());
                        this.dispDoorStatusByIPComm(controller, (ListViewItem) this.arrSelectedDoorsItem[this.watchingDealtDoorIndex]);
                        this.watchingDealtDoorIndex++;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void bkRealtimeGetRecords_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
                if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.GetRecordFirst)
                {
                    this.swipe4GetRecords.Clear();
                    int swipeRecordsByDoorName = -1;
                    swipeRecordsByDoorName = this.swipe4GetRecords.GetSwipeRecordsByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
                    if ((swipeRecordsByDoorName >= 0) && this.realtimeGetRecordsSwipeIndexGot.ContainsKey(this.swipe4GetRecords.ControllerSN))
                    {
                        this.realtimeGetRecordsSwipeIndexGot[this.swipe4GetRecords.ControllerSN] = this.swipe4GetRecords.lastRecordFlashIndex;
                    }
                    e.Result = swipeRecordsByDoorName;
                }
                else if ((this.stepOfRealtimeGetRecords != StepOfRealtimeGetReocrds.GetFinished) && (this.stepOfRealtimeGetRecords != StepOfRealtimeGetReocrds.StartMonitoring))
                {
                    if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.WaitGetRecord)
                    {
                        while (!this.bStopComm)
                        {
                            if (this.doorsNeedToGetRecords.Count > 0)
                            {
                                if ((this.dealtIndexOfDoorsNeedToGetRecords + 1) < this.doorsNeedToGetRecords.Count)
                                {
                                    int controllerSN;
                                    using (icController controller = new icController())
                                    {
                                        controller.GetInfoFromDBByDoorName(this.doorsNeedToGetRecords[this.dealtIndexOfDoorsNeedToGetRecords + 1].ToString());
                                        controllerSN = controller.ControllerSN;
                                    }
                                    if (((this.realtimeGetRecordsSwipeIndexGot.ContainsKey(controllerSN) && (this.realtimeGetRecordsSwipeIndexGot[controllerSN] > 0)) && (this.selectedControllersOfRealtimeGetRecords.ContainsKey(controllerSN) && (this.selectedControllersOfRealtimeGetRecords[controllerSN].GetControllerRunInformationIP() > 0))) && ((this.selectedControllersOfRealtimeGetRecords[controllerSN].runinfo.lastGetRecordIndex + this.selectedControllersOfRealtimeGetRecords[controllerSN].runinfo.getNewRecordsNum()) >= ((long)this.realtimeGetRecordsSwipeIndexGot[controllerSN])))
                                    {
                                        this.selectedControllersOfRealtimeGetRecords[controllerSN].UpdateLastGetRecordLocationIP((uint)this.realtimeGetRecordsSwipeIndexGot[controllerSN]);
                                        this.needDelSwipeControllers[controllerSN] = 0;
                                    }
                                    this.swipe4GetRecords.Clear();
                                    int num3 = -1;
                                    num3 = this.swipe4GetRecords.GetSwipeRecordsByDoorName(this.doorsNeedToGetRecords[this.dealtIndexOfDoorsNeedToGetRecords + 1].ToString());
                                    e.Result = num3;
                                    if ((num3 >= 0) && this.realtimeGetRecordsSwipeIndexGot.ContainsKey(this.swipe4GetRecords.ControllerSN))
                                    {
                                        this.realtimeGetRecordsSwipeIndexGot[this.swipe4GetRecords.ControllerSN] = this.swipe4GetRecords.lastRecordFlashIndex;
                                    }
                                    break;
                                }
                                if (this.doorsNeedToGetRecords.Count > 0x3e8)
                                {
                                    this.doorsNeedToGetRecords.Clear();
                                    this.dealtIndexOfDoorsNeedToGetRecords = -1;
                                }
                            }
                            else
                            {
                                Thread.Sleep(0x3e8);
                            }
                        }
                    }
                    else if ((this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.DelSwipe) && (this.realtimeGetRecordsSwipeIndexGot.Count > 0))
                    {
                        foreach (object obj2 in this.selectedControllersSNOfRealtimeGetRecords)
                        {
                            int key = (int) obj2;
                            if (((this.realtimeGetRecordsSwipeIndexGot.ContainsKey(key) && (this.realtimeGetRecordsSwipeIndexGot[key] > 0)) && (this.selectedControllersOfRealtimeGetRecords.ContainsKey(key) && (this.needDelSwipeControllers[key] == 1))) && ((this.selectedControllersOfRealtimeGetRecords[key].GetControllerRunInformationIP() > 0) && ((this.selectedControllersOfRealtimeGetRecords[key].runinfo.lastGetRecordIndex + this.selectedControllersOfRealtimeGetRecords[key].runinfo.getNewRecordsNum()) >= ((long) this.realtimeGetRecordsSwipeIndexGot[key]))))
                            {
                                this.selectedControllersOfRealtimeGetRecords[key].UpdateLastGetRecordLocationIP((uint)this.realtimeGetRecordsSwipeIndexGot[key]);
                            }
                        }
                    }
                }
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void bkRealtimeGetRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    wgAppConfig.wgLog(CommonStr.strOperationCanceled);
                }
                else if (e.Error != null)
                {
                    wgAppConfig.wgLog(string.Format("An error occurred: {0}", e.Error.Message));
                }
                else if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.GetRecordFirst)
                {
                    this.getRecordsFromController(int.Parse(e.Result.ToString()));
                    if (!this.bStopComm)
                    {
                        if (this.dealtDoorIndex == this.arrSelectedDoors.Count)
                        {
                            this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.GetFinished;
                        }
                        this.bkRealtimeGetRecords.RunWorkerAsync();
                    }
                    else
                    {
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = CommonStr.strStopComm;
                        newInfo.information = CommonStr.strStopComm;
                        wgRunInfoLog.addEvent(newInfo);
                        this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.Stop;
                        this.btnRealtimeGetRecords.BackColor = Color.Transparent;
                        this.btnRealtimeGetRecords.Text = CommonStr.strRealtimeGetRecords;
                        wgAppRunInfo.raiseAppRunInfoCommStatus(CommonStr.strStopComm);
                    }
                }
                else if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.GetFinished)
                {
                    wgAppRunInfo.raiseAppRunInfoCommStatus("");
                    if (this.watching == null)
                    {
                        this.watching = new WatchingService();
                        this.watching.EventHandler += new OnEventHandler(this.evtNewInfoCallBack);
                    }
                    this.timerUpdateDoorInfo.Enabled = false;
                    this.watchingStartTime = DateTime.Now;
                    wgTools.WriteLine("selectedControllers.Count=" + this.selectedControllersOfRealtimeGetRecords.Count.ToString());
                    this.watching.WatchingController = this.selectedControllersOfRealtimeGetRecords;
                    this.timerUpdateDoorInfo.Interval = 300;
                    this.timerUpdateDoorInfo.Enabled = true;
                    wgAppRunInfo.raiseAppRunInfoMonitors("2");
                    this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.StartMonitoring;
                    this.bkRealtimeGetRecords.RunWorkerAsync();
                }
                else if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.StartMonitoring)
                {
                    this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.WaitGetRecord;
                    this.bkRealtimeGetRecords.RunWorkerAsync();
                }
                else if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.WaitGetRecord)
                {
                    if (this.bStopComm)
                    {
                        this.dealtDoorIndex = 0;
                        this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.DelSwipe;
                        this.bkRealtimeGetRecords.RunWorkerAsync();
                    }
                    else
                    {
                        InfoRow row2 = new InfoRow();
                        row2.desc = string.Format("{0}", this.doorsNeedToGetRecords[this.dealtIndexOfDoorsNeedToGetRecords + 1].ToString());
                        row2.information = string.Format("{0}", CommonStr.strAlreadyGotSwipeRecord);
                        wgRunInfoLog.addEvent(row2);
                        if (this.dgvRunInfo.Rows.Count > 0)
                        {
                            this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.dgvRunInfo.Rows.Count - 1;
                            this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = true;
                            Application.DoEvents();
                        }
                        this.dealtIndexOfDoorsNeedToGetRecords++;
                        this.bkRealtimeGetRecords.RunWorkerAsync();
                    }
                }
                else if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.DelSwipe)
                {
                    this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.Stop;
                    this.btnRealtimeGetRecords.BackColor = Color.Transparent;
                    this.btnRealtimeGetRecords.Text = CommonStr.strRealtimeGetRecords;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void bkUploadAndGetRecords_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            if (this.CommOperate == "UPLOAD")
            {
                e.Result = this.uploadPrivilegeNow(this.CommOperateOption);
            }
            else if (this.CommOperate == "GETRECORDS")
            {
                e.Result = this.getRecordsNow();
            }
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void bkUploadAndGetRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                XMessageBox.Show(CommonStr.strOperationCanceled);
                wgAppConfig.wgLog(CommonStr.strOperationCanceled);
            }
            else if (e.Error != null)
            {
                XMessageBox.Show(string.Format("An error occurred: {0}", e.Error.Message));
            }
            else if (this.CommOperate == "UPLOAD")
            {
                this.uploadPrivilegeToController(int.Parse(e.Result.ToString()));
                if (this.dealtDoorIndex < this.arrSelectedDoors.Count)
                {
                    this.bkUploadAndGetRecords.RunWorkerAsync();
                }
            }
            else if (this.CommOperate == "GETRECORDS")
            {
                this.getRecordsFromController(int.Parse(e.Result.ToString()));
                if (this.dealtDoorIndex < this.arrSelectedDoors.Count)
                {
                    this.bkUploadAndGetRecords.RunWorkerAsync();
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (this.lstDoors.SelectedItems.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectDoor);
            }
            else
            {
                using (this.control4Check = new icController())
                {
                    this.bStopComm = false;
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        if (this.bStopComm)
                        {
                            return;
                        }
                        this.control4Check.GetInfoFromDBByDoorName(item.Text);
                        if (this.control4Check.GetControllerRunInformationIP() <= 0)
                        {
                            wgRunInfoLog.addEventNotConnect(this.control4Check.ControllerSN, this.control4Check.IP, item);
                        }
                        else
                        {
                            wgTools.WriteLine("Start");
                            wgMjControllerConfigure controlConfigure = new wgMjControllerConfigure();
                            wgMjControllerConfigure configure2 = new wgMjControllerConfigure();
                            wgMjControllerTaskList controlTaskList = new wgMjControllerTaskList();
                            wgMjControllerHolidaysList controlHolidayList = new wgMjControllerHolidaysList();
                            icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(this.control4Check.ControllerID, ref configure2, ref controlTaskList, ref controlHolidayList);
                            wgTools.WriteLine("getControllerConfigureFromDBByControllerID");
                            if (this.control4Check.GetConfigureIP(ref controlConfigure) <= 0)
                            {
                                wgRunInfoLog.addEventNotConnect(this.control4Check.ControllerSN, this.control4Check.IP, item);
                                continue;
                            }
                            wgTools.WriteLine("getConfigureIP");
                            wgMjControllerTaskList list3 = new wgMjControllerTaskList();
                            if (((this.control4Check.runinfo.appError & 2) == 0) && (this.control4Check.GetControlTaskListIP(ref list3) <= 0))
                            {
                                wgRunInfoLog.addEventNotConnect(this.control4Check.ControllerSN, this.control4Check.IP, item);
                                continue;
                            }
                            wgTools.WriteLine("getControlTaskListIP");
                            new wgMjControllerHolidaysList();
                            if ((this.control4Check.runinfo.appError & 2) == 0)
                            {
                                byte[] holidayListData = null;
                                if (this.control4Check.GetHolidayListIP(ref holidayListData) <= 0)
                                {
                                    wgRunInfoLog.addEventNotConnect(this.control4Check.ControllerSN, this.control4Check.IP, item);
                                    continue;
                                }
                                new wgMjControllerHolidaysList(holidayListData);
                            }
                            wgTools.WriteLine("GetHolidayListIP");
                            InfoRow newInfo = new InfoRow();
                            newInfo.desc = string.Format("{0}[{1:d}]", item.Text, this.control4Check.ControllerSN);
                            newInfo.information = "";
                            newInfo.detail = item.Text;
                            newInfo.detail = newInfo.detail + string.Format("\r\n{0}:\t{1}", CommonStr.strDoorStatus, this.control4Check.runinfo.IsOpen(this.control4Check.GetDoorNO(item.Text)) ? CommonStr.strDoorStatus_Open : CommonStr.strDoorStatus_Closed);
                            newInfo.information = newInfo.information + string.Format("{0};", this.control4Check.runinfo.IsOpen(this.control4Check.GetDoorNO(item.Text)) ? CommonStr.strDoorStatus_Open : CommonStr.strDoorStatus_Closed);
                            newInfo.detail = newInfo.detail + string.Format("\r\n{0}:\t{1}", CommonStr.strDoorControl, icDesc.doorControlDesc(controlConfigure.DoorControlGet(this.control4Check.GetDoorNO(item.Text))));
                            newInfo.information = newInfo.information + string.Format("{0};", icDesc.doorControlDesc(controlConfigure.DoorControlGet(this.control4Check.GetDoorNO(item.Text))));
                            newInfo.detail = newInfo.detail + string.Format("\r\n{0}:\t{1:d}", CommonStr.strDoorDelay, controlConfigure.DoorDelayGet(this.control4Check.GetDoorNO(item.Text)).ToString());
                            newInfo.information = newInfo.information + string.Format("{0}:{1:d};", CommonStr.strDoorDelay, controlConfigure.DoorDelayGet(this.control4Check.GetDoorNO(item.Text)).ToString());
                            newInfo.detail = newInfo.detail + string.Format("\r\n{0}:\t{1:d}", CommonStr.strControllerSN, this.control4Check.ControllerSN);
                            newInfo.detail = newInfo.detail + string.Format("\r\nIP:\t{0}", this.control4Check.IP);
                            newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strSwipes, this.control4Check.runinfo.getNewRecordsNum());
                            newInfo.information = newInfo.information + string.Format("{0}:{1};", CommonStr.strSwipes, this.control4Check.runinfo.getNewRecordsNum());
                            newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strPrivileges, this.control4Check.runinfo.regUserCount);
                            newInfo.information = newInfo.information + string.Format("{0}:{1};", CommonStr.strPrivileges, this.control4Check.runinfo.regUserCount);
                            newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strRealClock, this.control4Check.runinfo.dtNow.ToString(wgTools.DisplayFormat_DateYMDHMSWeek));
                            newInfo.information = newInfo.information + string.Format("{0};", this.control4Check.runinfo.dtNow.ToString(wgTools.DisplayFormat_DateYMDHMSWeek));
                            if (this.control4Check.runinfo.appError > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strErr, icDesc.ErrorDetail(this.control4Check.runinfo.appError));
                                newInfo.information = newInfo.information + string.Format("{0};", icDesc.ErrorDetail(this.control4Check.runinfo.appError));
                            }
                            if (this.control4Check.runinfo.WarnInfo(this.control4Check.GetDoorNO(item.Text)) > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strWarnDesc, icDesc.WarnDetail(this.control4Check.runinfo.WarnInfo(this.control4Check.GetDoorNO(item.Text))));
                                newInfo.information = newInfo.information + string.Format("{0};", icDesc.WarnDetail(this.control4Check.runinfo.WarnInfo(this.control4Check.GetDoorNO(item.Text))));
                            }
                            if (this.control4Check.runinfo.FireIsActive)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strFire);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strFire);
                            }
                            if (this.control4Check.runinfo.ForceLockIsActive)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strCloseByForce);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strCloseByForce);
                            }
                            newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strFirmware, this.control4Check.runinfo.driverVersion);
                            newInfo.information = newInfo.information + string.Format("{0};", this.control4Check.runinfo.driverVersion);
                            try
                            {
                                string compactInfo = "";
                                string desc = "";
                                if (!string.IsNullOrEmpty(this.control4Check.GetProductInfoIP(ref compactInfo, ref desc)))
                                {
                                    newInfo.detail = newInfo.detail + string.Format(" [{0}]", compactInfo.Substring(compactInfo.IndexOf("DATE=") + 5, 10));
                                }
                            }
                            catch (Exception)
                            {
                            }
                            newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", "MAC", controlConfigure.MACAddr);
                            newInfo.information = newInfo.information + string.Format("{0};", controlConfigure.MACAddr);
                            newInfo.detail = newInfo.detail + "\r\n---- " + CommonStr.strEnabled + " ----";
                            item.ImageIndex = this.control4Check.runinfo.GetDoorImageIndex(this.control4Check.GetDoorNO(item.Text));
                            if ((DateTime.Now.AddMinutes(-30.0) > this.control4Check.runinfo.dtNow) || (DateTime.Now.AddMinutes(30.0) < this.control4Check.runinfo.dtNow))
                            {
                                this.checkParam(DateTime.Now.ToString(wgTools.YMDHMSFormat), this.control4Check.runinfo.dtNow.ToString(wgTools.YMDHMSFormat), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRealClock, item.Text + " " + CommonStr.strNeedAdjustTime, false);
                            }
                            wgTools.WriteLine("icPrivilege.getPrivilegeNumInDBByID");
                            //this.checkParamPrivileges(item.Text, this.control4Check, (int)this.control4Check.runinfo.regUserCount);
                            if (controlConfigure.controlTaskList_enabled > 0)
                            {
                                if (list3.taskCount > 0)
                                {
                                    newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strControlTaskList);
                                    newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strControlTaskList);
                                }
                            }
                            else
                            {
                                list3.Clear();
                            }
                            this.checkParam(controlTaskList.taskCount.ToString(), list3.taskCount.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strControlTaskList, item.Text, false);
                            if (list3.taskCount == 0)
                            {
                                this.checkParam(icDesc.doorControlDesc(configure2.DoorControlGet(this.control4Check.GetDoorNO(item.Text))), icDesc.doorControlDesc(controlConfigure.DoorControlGet(this.control4Check.GetDoorNO(item.Text))), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strDoorControl, item.Text, false);
                            }
                            this.checkParam(configure2.DoorDelayGet(this.control4Check.GetDoorNO(item.Text)).ToString(), controlConfigure.DoorDelayGet(this.control4Check.GetDoorNO(item.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strDoorDelay, item.Text, false);
                            if (controlConfigure.DoorInterlockGet(this.control4Check.GetDoorNO(item.Text)) > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strInterLock);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strInterLock);
                            }
                            this.checkParam(configure2.DoorInterlockGet(this.control4Check.GetDoorNO(item.Text)).ToString(), controlConfigure.DoorInterlockGet(this.control4Check.GetDoorNO(item.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strInterLock, item.Text, false);
                            if (wgMjController.GetControllerType(this.control4Check.ControllerSN) == 4)
                            {
                                if (controlConfigure.ReaderPasswordGet(this.control4Check.GetDoorNO(item.Text)) > 0)
                                {
                                    newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strPasswordKeypad);
                                    newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strPasswordKeypad);
                                }
                                this.checkParam(configure2.ReaderPasswordGet(this.control4Check.GetDoorNO(item.Text)).ToString(), controlConfigure.ReaderPasswordGet(this.control4Check.GetDoorNO(item.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strPasswordKeypad, item.Text, false);
                            }
                            else
                            {
                                if (controlConfigure.ReaderPasswordGet(((this.control4Check.GetDoorNO(item.Text) - 1) * 2) + 1) > 0)
                                {
                                    newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strInDoor, CommonStr.strPasswordKeypad);
                                    newInfo.information = newInfo.information + string.Format("{0}:{1};", CommonStr.strInDoor, CommonStr.strPasswordKeypad);
                                }
                                if (controlConfigure.ReaderPasswordGet(((this.control4Check.GetDoorNO(item.Text) - 1) * 2) + 2) > 0)
                                {
                                    newInfo.detail = newInfo.detail + string.Format("\r\n--{0}:\t{1}", CommonStr.strExitDoor, CommonStr.strPasswordKeypad);
                                    newInfo.information = newInfo.information + string.Format("{0}:{1};", CommonStr.strExitDoor, CommonStr.strPasswordKeypad);
                                }
                                this.checkParam(configure2.ReaderPasswordGet(((this.control4Check.GetDoorNO(item.Text) - 1) * 2) + 1).ToString(), controlConfigure.ReaderPasswordGet(((this.control4Check.GetDoorNO(item.Text) - 1) * 2) + 1).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strInDoor + " " + CommonStr.strPasswordKeypad, item.Text, false);
                                this.checkParam(configure2.ReaderPasswordGet(((this.control4Check.GetDoorNO(item.Text) - 1) * 2) + 2).ToString(), controlConfigure.ReaderPasswordGet(((this.control4Check.GetDoorNO(item.Text) - 1) * 2) + 2).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strExitDoor + " " + CommonStr.strPasswordKeypad, item.Text, false);
                            }
                            if (controlConfigure.receventPB > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strRecordButtonEvent);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strRecordButtonEvent);
                            }
                            this.checkParam(configure2.receventPB.ToString(), controlConfigure.receventPB.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRecordButtonEvent, item.Text, true);
                            if (controlConfigure.receventDS > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strRecordDoorStatusEvent);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strRecordDoorStatusEvent);
                            }
                            this.checkParam(configure2.receventDS.ToString(), controlConfigure.receventDS.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRecordDoorStatusEvent, item.Text, true);
                            if (controlConfigure.receventWarn > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strRecordWarnEvent);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strRecordWarnEvent);
                            }
                            this.checkParam(configure2.receventWarn.ToString(), controlConfigure.receventWarn.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strRecordWarnEvent, item.Text, true);
                            if (controlConfigure.antiback > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strAntiBack);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strAntiBack);
                            }
                            this.checkParam(configure2.antiback.ToString(), controlConfigure.antiback.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strAntiBack, item.Text, true);
                            if (controlConfigure.DoorDisableTimesegMinGet(this.control4Check.GetDoorNO(item.Text)) > 1)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strDisableControlSeg);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strDisableControlSeg);
                            }
                            this.checkParam(configure2.DoorDisableTimesegMinGet(this.control4Check.GetDoorNO(item.Text)).ToString(), controlConfigure.DoorDisableTimesegMinGet(this.control4Check.GetDoorNO(item.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strDisableControlSeg, item.Text, true);
                            if (controlConfigure.indoorPersonsMax > 0)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strIndoorPersonsMax);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strIndoorPersonsMax);
                            }
                            this.checkParam(configure2.indoorPersonsMax.ToString(), controlConfigure.indoorPersonsMax.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strIndoorPersonsMax, item.Text, true);
                            if ((controlConfigure.warnSetup & -41) > 1)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", icDesc.WarnDetail(controlConfigure.warnSetup & -41));
                                newInfo.information = newInfo.information + string.Format("{0};", icDesc.WarnDetail(controlConfigure.warnSetup & -41));
                            }
                            this.checkParam(configure2.warnSetup.ToString(), controlConfigure.warnSetup.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strWarn, item.Text, true);
                            if (controlConfigure.MorecardNeedCardsGet(this.control4Check.GetDoorNO(item.Text)) > 1)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strMoreCards);
                                newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strMoreCards);
                            }
                            this.checkParam(configure2.MorecardNeedCardsGet(this.control4Check.GetDoorNO(item.Text)).ToString(), controlConfigure.MorecardNeedCardsGet(this.control4Check.GetDoorNO(item.Text)).ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strMoreCards, item.Text, true);
                            if (controlConfigure.lockSwitchOption >= 1)
                            {
                                string str4 = "";
                                for (int i = 0; i < 4; i++)
                                {
                                    if ((controlConfigure.lockSwitchOption & (((int) 1) << i)) > 0)
                                    {
                                        if (str4 != "")
                                        {
                                            str4 = str4 + ",";
                                        }
                                        str4 = str4 + "#" + ((i + 1)).ToString();
                                    }
                                }
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}({1})", CommonStr.strLockSwitch, str4);
                                newInfo.information = newInfo.information + string.Format("{0}({1});", CommonStr.strLockSwitch, str4);
                            }
                            this.checkParam(configure2.lockSwitchOption.ToString(), controlConfigure.lockSwitchOption.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strLockSwitch, item.Text, true);
                            if (controlConfigure.swipeGap >= 1)
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}({1}s)", CommonStr.strSwipeGap, controlConfigure.swipeGap);
                                newInfo.information = newInfo.information + string.Format("{0}({1}s);", CommonStr.strSwipeGap, controlConfigure.swipeGap);
                            }
                            this.checkParam(configure2.swipeGap.ToString(), controlConfigure.swipeGap.ToString(), "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strSwipeGap, item.Text, true);
                            if ((controlConfigure.webPort != 0) && (controlConfigure.webPort != 0xffff))
                            {
                                string str5 = "";
                                str5 = controlConfigure.webDeviceName + (string.IsNullOrEmpty(controlConfigure.webDeviceName) ? "" : ",") + string.Format("{0},{1},{2}", controlConfigure.webLanguage, (wgAppConfig.CultureInfoStr == "zh-CHS") ? controlConfigure.webDateDisplayFormatCHS : controlConfigure.webDateDisplayFormat, controlConfigure.webPort.ToString());
                                newInfo.detail = newInfo.detail + string.Format("\r\n--{0}({1})", CommonStr.strWEBEnabled, str5);
                                newInfo.information = newInfo.information + string.Format("{0}({1});", CommonStr.strWEBEnabled, str5);
                            }
                            if ((controlConfigure.SpecialCard_Mother1 != 0L) && (controlConfigure.SpecialCard_Mother1 != 0xffffffffL))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}1({1})", CommonStr.strSpecialCardMother, controlConfigure.SpecialCard_Mother1.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}1({1});", CommonStr.strSpecialCardMother, controlConfigure.SpecialCard_Mother1.ToString());
                            }
                            if ((controlConfigure.SpecialCard_Mother2 != 0L) && (controlConfigure.SpecialCard_Mother2 != 0xffffffffL))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}2({1})", CommonStr.strSpecialCardMother, controlConfigure.SpecialCard_Mother2.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}2({1});", CommonStr.strSpecialCardMother, controlConfigure.SpecialCard_Mother2.ToString());
                            }
                            if ((controlConfigure.SpecialCard_OnlyOpen1 != 0L) && (controlConfigure.SpecialCard_OnlyOpen1 != 0xffffffffL))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}1({1})", CommonStr.strSpecialCardSuper, controlConfigure.SpecialCard_OnlyOpen1.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}1({1});", CommonStr.strSpecialCardSuper, controlConfigure.SpecialCard_OnlyOpen1.ToString());
                            }
                            if ((controlConfigure.SpecialCard_OnlyOpen2 != 0L) && (controlConfigure.SpecialCard_OnlyOpen2 != 0xffffffffL))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}2({1})", CommonStr.strSpecialCardSuper, controlConfigure.SpecialCard_OnlyOpen2.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}2({1});", CommonStr.strSpecialCardSuper, controlConfigure.SpecialCard_OnlyOpen2.ToString());
                            }
                            if (((controlConfigure.fire_broadcast_receive != 0) && (controlConfigure.fire_broadcast_receive != 0xffL)) || ((controlConfigure.fire_broadcast_send != 0) && (controlConfigure.fire_broadcast_send != 0xffL)))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}({1}s,#{2})", CommonStr.strFireSignalShare, controlConfigure.fire_broadcast_receive.ToString(), controlConfigure.fire_broadcast_send.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}({1}s,#{2});", CommonStr.strFireSignalShare, controlConfigure.fire_broadcast_receive.ToString(), controlConfigure.fire_broadcast_send.ToString());
                            }
                            this.checkParam((string.Format("({0}s,{1})", configure2.fire_broadcast_receive.ToString(), configure2.fire_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", (string.Format("({0}s,{1})", controlConfigure.fire_broadcast_receive.ToString(), controlConfigure.fire_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strFireSignalShare, item.Text, true);
                            if (((controlConfigure.interlock_broadcast_receive != 0) && (controlConfigure.interlock_broadcast_receive != 0xffL)) || ((controlConfigure.interlock_broadcast_send != 0) && (controlConfigure.interlock_broadcast_send != 0xffL)))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}({1}s,#{2})", CommonStr.strInterLockShare, controlConfigure.interlock_broadcast_receive.ToString(), controlConfigure.interlock_broadcast_send.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}({1}s,#{2});", CommonStr.strInterLockShare, controlConfigure.interlock_broadcast_receive.ToString(), controlConfigure.interlock_broadcast_send.ToString());
                            }
                            this.checkParam((string.Format("({0}s,{1})", configure2.interlock_broadcast_receive.ToString(), configure2.interlock_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", (string.Format("({0}s,{1})", controlConfigure.interlock_broadcast_receive.ToString(), controlConfigure.interlock_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strInterLockShare, item.Text, true);
                            if ((controlConfigure.antiback_broadcast_send != 0) && (controlConfigure.antiback_broadcast_send != 0xffL))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}(#{1})", CommonStr.strAntibackShare, controlConfigure.antiback_broadcast_send.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}(#{1});", CommonStr.strAntibackShare, controlConfigure.antiback_broadcast_send.ToString());
                            }
                            this.checkParam((string.Format("({0}s,{1})", configure2.antiback_broadcast_send.ToString(), configure2.antiback_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", (string.Format("({0}s,{1})", controlConfigure.antiback_broadcast_send.ToString(), controlConfigure.antiback_broadcast_send.ToString()) != "(0s,0)") ? "1" : "0", "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strAntibackShare, item.Text, true);
                            if ((controlConfigure.indoorPersonsMax > 0) || ((controlConfigure.antiback_broadcast_send != 0) && (controlConfigure.antiback_broadcast_send != 0xffL)))
                            {
                                newInfo.detail = newInfo.detail + string.Format("\r\n--***{0}({1})", CommonStr.strtotalPerson4AntibackShare, this.control4Check.runinfo.totalPerson4AntibackShare.ToString());
                                newInfo.information = newInfo.information + string.Format("***{0}({1});", CommonStr.strtotalPerson4AntibackShare, this.control4Check.runinfo.totalPerson4AntibackShare.ToString());
                            }
                            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DISPLAY_NEWEST_SWIPE")) && (this.control4Check.runinfo.getNewRecordsNum() > 0))
                            {
                                MjRec rec = this.control4Check.runinfo.newSwipes[0];
                                if (rec.addressIsReader)
                                {
                                    if (this.ReaderName.ContainsKey(string.Format("{0}-{1}", rec.ControllerSN.ToString(), rec.ReaderNo.ToString())))
                                    {
                                        wgTools.WriteLine("ReaderName.ContainsKey(string.Format(");
                                        rec.address = this.ReaderName[string.Format("{0}-{1}", rec.ControllerSN.ToString(), rec.ReaderNo.ToString())];
                                    }
                                }
                                else
                                {
                                    this.dvDoors4Check.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", rec.ControllerSN.ToString(), rec.DoorNo.ToString());
                                    if (this.dvDoors4Check.Count > 0)
                                    {
                                        newInfo.desc = this.dvDoors4Check[0]["f_DoorName"].ToString();
                                        rec.address = this.dvDoors4Check[0]["f_DoorName"] as string;
                                    }
                                }
                                string str6 = rec.ToDisplayInfo();
                                int length = str6.LastIndexOf("-");
                                str6 = str6.Substring(0, length) + "\r\n  " + str6.Substring(length);
                                newInfo.detail = newInfo.detail + string.Format("\r\n\r\n--{0}", str6);
                            }
                            if (this.bNeedCheckLosePacket)
                            {
                                int num3 = 0;
                                int num4 = 0;
                                int num5 = 0;
                                for (int j = 0; j < 200; j++)
                                {
                                    num3++;
                                    if (this.control4Check.SpecialPingIP() == 1)
                                    {
                                        num4++;
                                    }
                                    else
                                    {
                                        num5++;
                                    }
                                }
                                if (num5 == 0)
                                {
                                    wgUdpComm.triesTotal = 0L;
                                    wgTools.WriteLine("control.Test1024 Start");
                                    int page = 0;
                                    string str7 = "";
                                    if (this.control4Check.test1024Write() < 0)
                                    {
                                        str7 = str7 + CommonStr.strCommLargePacketWriteFailed + "\r\n";
                                    }
                                    int num7 = this.control4Check.test1024Read(100, ref page);
                                    if (num7 < 0)
                                    {
                                        str7 = str7 + CommonStr.strCommLargePacketReadFailed + num7.ToString() + "\r\n";
                                    }
                                    if (wgUdpComm.triesTotal > 0L)
                                    {
                                        string str10 = str7;
                                        str7 = str10 + CommonStr.strCommLargePacketTryTimes + " = " + wgUdpComm.triesTotal.ToString() + "\r\n";
                                    }
                                    wgTools.WriteLine("control.Test1024 End");
                                    if (str7 != "")
                                    {
                                        string str8 = str7;
                                        newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strCommLose);
                                        newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strCommLose);
                                        newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", str8);
                                        newInfo.information = newInfo.information + string.Format("{0};", str8);
                                        InfoRow row2 = new InfoRow();
                                        row2.desc = "[" + item.Text + "]" + CommonStr.strCommLose;
                                        row2.information = "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strCommLose + ": " + str8;
                                        row2.category = 0x1f5;
                                        wgRunInfoLog.addEvent(row2);
                                    }
                                    else
                                    {
                                        newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strCommOK);
                                        newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strCommOK);
                                    }
                                }
                                else
                                {
                                    string str9 = string.Format(" {0}: {1}={2}, {3}={4}, {5} = {6}", new object[] { CommonStr.strCommPacket, CommonStr.strCommPacketSent, num3, CommonStr.strCommPacketReceived, num4, CommonStr.strCommPacketLost, num5 }) + "\r\n";
                                    newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", CommonStr.strCommLose);
                                    newInfo.information = newInfo.information + string.Format("{0};", CommonStr.strCommLose);
                                    newInfo.detail = newInfo.detail + string.Format("\r\n--{0}", str9);
                                    newInfo.information = newInfo.information + string.Format("{0};", str9);
                                    InfoRow row3 = new InfoRow();
                                    row3.desc = "[" + item.Text + "]" + CommonStr.strCommLose;
                                    row3.information = "[" + this.control4Check.ControllerSN.ToString() + "]" + CommonStr.strCommLose + ": " + str9;
                                    row3.category = 0x1f5;
                                    wgRunInfoLog.addEvent(row3);
                                }
                            }
                            wgRunInfoLog.addEvent(newInfo);
                        }
                        this.displayNewestLog();
                        wgTools.WriteLine("displayNewestLog");
                    }
                }
            }
        }

        private void btnClearRunInfo_Click(object sender, EventArgs e)
        {
            (this.dgvRunInfo.DataSource as DataView).Table.Clear();
            this.txtInfo.Text = "";
            this.richTxtInfo.Text = "";
            this.pictureBox1.Visible = false;
            if (!string.IsNullOrEmpty(this.oldInfoTitleString))
            {
                this.dataGridView2.Columns[0].HeaderText = this.oldInfoTitleString;
            }
        }

        private void btnDirectSetDoorControl()
        {
            if (this.lstDoors.SelectedItems.Count <= 0)
            {
                this.btnSelectAll_Click(null, null);
            }
            if (this.lstDoors.SelectedItems.Count > 0)
            {
                int doorControl = -1;
                using (dfrmControllerDoorControlSet set = new dfrmControllerDoorControlSet())
                {
                    if (this.lstDoors.SelectedItems.Count == 1)
                    {
                        set.Text = set.Text + "--" + this.lstDoors.SelectedItems[0].Text;
                    }
                    else
                    {
                        set.Text = set.Text + "--" + CommonStr.strDoorsNum + " = " + this.lstDoors.SelectedItems.Count.ToString();
                        if (this.lstDoors.Items.Count == this.lstDoors.SelectedItems.Count)
                        {
                            set.Text = set.Text + CommonStr.strAll;
                        }
                    }
                    if (set.ShowDialog(this) == DialogResult.OK)
                    {
                        doorControl = set.doorControl;
                    }
                    if (doorControl < 0)
                    {
                        return;
                    }
                }
                using (icController controller = new icController())
                {
                    this.bStopComm = false;
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        if (this.bStopComm)
                        {
                            return;
                        }
                        controller.GetInfoFromDBByDoorName(item.Text);
                        if (controller.DirectSetDoorControlIP(item.Text, doorControl) <= 0)
                        {
                            wgRunInfoLog.addEventNotConnect(controller.ControllerSN, controller.IP, item);
                        }
                        else
                        {
                            InfoRow newInfo = new InfoRow();
                            newInfo.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                            newInfo.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(doorControl));
                            wgRunInfoLog.addEvent(newInfo);
                            this.dispDoorStatusByIPComm(controller, item);
                        }
                        this.displayNewestLog();
                    }
                }
            }
        }

        private void btnGetRecords_Click(object sender, EventArgs e)
        {
            if (((this.lstDoors.SelectedItems.Count <= 0) || (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)) && !this.bkUploadAndGetRecords.IsBusy)
            {
                if (this.lstDoors.SelectedItems.Count <= 0)
                {
                    XMessageBox.Show(CommonStr.strSelectDoor);
                }
                else
                {
                    this.btnRealtimeGetRecords.Enabled = false;
                    this.btnStopOthers.BackColor = Color.Red;
                    this.btnStopMonitor.BackColor = Color.Red;
                    this.btnGetRecords.Enabled = false;
                    this.btnUpload.Enabled = false;
                    this.arrSelectedDoors.Clear();
                    this.arrSelectedDoorsItem.Clear();
                    this.dealtDoorIndex = 0;
                    this.arrDealtController.Clear();
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        this.arrSelectedDoors.Add(item.Text);
                        this.arrSelectedDoorsItem.Add(item);
                    }
                    using (icController controller = new icController())
                    {
                        controller.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), controller.ControllerSN);
                        newInfo.information = string.Format("{0}", CommonStr.strGetSwipeRecordStart);
                        wgRunInfoLog.addEvent(newInfo);
                    }
                    this.displayNewestLog();
                    this.CommOperate = "GETRECORDS";
                    this.bkUploadAndGetRecords.RunWorkerAsync();
                    if (!this.bkDispDoorStatus.IsBusy)
                    {
                        this.bkDispDoorStatus.RunWorkerAsync();
                    }
                }
            }
        }

        private void btnHideTools_Click(object sender, EventArgs e)
        {
            wgAppConfig.UpdateKeyVal("DISPLAY_NEWEST_SWIPE", this.chkDisplayNewestSwipe.Checked ? "1" : "");
            this.bNeedCheckLosePacket = this.chkNeedCheckLosePacket.Checked;
            this.grpTool.Visible = false;
        }

        private void btnMaps_Click(object sender, EventArgs e)
        {
            if (this.frmMaps1 != null)
            {
                try
                {
                    this.frmMaps1.Dispose();
                    this.frmMaps1 = null;
                }
                catch (Exception exception)
                {
                    wgAppConfig.wgLog(exception.ToString());
                }
            }
            this.frmMaps1 = new frmMaps();
            this.frmMaps1.lstDoors = this.lstDoors;
            this.frmMaps1.btnMonitor = this.btnServer;
            this.frmMaps1.contextMenuStrip1Doors = this.contextMenuStrip1Doors;
            this.frmMaps1.TopMost = true;
            this.frmMaps1.btnStop = this.btnStopOthers;
            this.frmMaps1.Show();
        }

        private void btnRealtimeGetRecords_Click(object sender, EventArgs e)
        {
            if (!this.bkRealtimeGetRecords.IsBusy && this.btnRealtimeGetRecords.Enabled)
            {
                if (this.lstDoors.SelectedItems.Count <= 0)
                {
                    if (!this.bDirectToRealtimeGet)
                    {
                        XMessageBox.Show(CommonStr.strSelectDoor);
                    }
                }
                else if (((this.lstDoors.SelectedItems.Count <= 0) || this.bDirectToRealtimeGet) || (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
                {
                    this.bDirectToRealtimeGet = false;
                    this.btnStopOthers_Click(null, null);
                    this.btnRealtimeGetRecords.Enabled = false;
                    this.btnGetRecords.Enabled = false;
                    this.btnUpload.Enabled = false;
                    this.btnServer.Enabled = false;
                    this.Refresh();
                    this.arrSelectedDoors.Clear();
                    this.arrSelectedDoorsItem.Clear();
                    this.dealtDoorIndex = 0;
                    this.arrDealtController.Clear();
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        this.arrSelectedDoors.Add(item.Text);
                        this.arrSelectedDoorsItem.Add(item);
                    }
                    this.timerUpdateDoorInfo.Enabled = false;
                    Dictionary<int, icController> dictionary = new Dictionary<int, icController>();
                    this.needDelSwipeControllers = new Dictionary<int, int>();
                    Cursor.Current = Cursors.WaitCursor;
                    foreach (ListViewItem item2 in this.lstDoors.Items)
                    {
                        (item2.Tag as DoorSetInfo).Selected = 0;
                    }
                    this.realtimeGetRecordsSwipeIndexGot.Clear();
                    this.selectedControllersSNOfRealtimeGetRecords.Clear();
                    foreach (ListViewItem item3 in this.lstDoors.SelectedItems)
                    {
                        (item3.Tag as DoorSetInfo).Selected = 1;
                        if (!dictionary.ContainsKey((item3.Tag as DoorSetInfo).ControllerSN))
                        {
                            wgTools.WriteLine("!selectedControllers.ContainsKey(control.ControllerSN)");
                            this.control4Realtime = new icController();
                            this.control4Realtime.GetInfoFromDBByDoorName(item3.Text);
                            dictionary.Add(this.control4Realtime.ControllerSN, this.control4Realtime);
                            this.realtimeGetRecordsSwipeIndexGot.Add(this.control4Realtime.ControllerSN, -1);
                            this.selectedControllersSNOfRealtimeGetRecords.Add(this.control4Realtime.ControllerSN);
                            this.needDelSwipeControllers.Add(this.control4Realtime.ControllerSN, 0);
                        }
                    }
                    this.selectedControllersOfRealtimeGetRecords = dictionary;
                    using (icController controller = new icController())
                    {
                        controller.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = "";
                        newInfo.information = string.Format("{0}", CommonStr.strRealtimeGetSwipeRecordStart);
                        wgRunInfoLog.addEvent(newInfo);
                        newInfo = new InfoRow();
                        newInfo.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), controller.ControllerSN);
                        newInfo.information = string.Format("{0}", CommonStr.strGetSwipeRecordStart);
                        wgRunInfoLog.addEvent(newInfo);
                    }
                    this.displayNewestLog();
                    this.bStopComm = false;
                    this.stepOfRealtimeGetRecords = StepOfRealtimeGetReocrds.GetRecordFirst;
                    this.dealtIndexOfDoorsNeedToGetRecords = -1;
                    this.doorsNeedToGetRecords.Clear();
                    this.bkRealtimeGetRecords.RunWorkerAsync();
                    if (!this.bkDispDoorStatus.IsBusy)
                    {
                        this.bkDispDoorStatus.RunWorkerAsync();
                    }
                    (sender as ToolStripButton).BackColor = Color.LightGreen;
                    (sender as ToolStripButton).Text = CommonStr.strRealtimeGetting;
                    this.btnStopOthers.BackColor = Color.Red;
                    this.btnStopMonitor.BackColor = Color.Red;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void btnRemoteOpen_Click(object sender, EventArgs e)
        {
            if (this.lstDoors.SelectedItems.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectDoor);
            }
            else if ((this.lstDoors.SelectedItems.Count <= 0) || (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
            {
                using (icController controller = new icController())
                {
                    this.bStopComm = false;
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        if (this.bStopComm)
                        {
                            return;
                        }
                        controller.GetInfoFromDBByDoorName(item.Text);
                        if (controller.RemoteOpenDoorIP(item.Text) <= 0)
                        {
                            wgRunInfoLog.addEventNotConnect(controller.ControllerSN, controller.IP, item);
                        }
                        else
                        {
                            InfoRow newInfo = new InfoRow();
                            newInfo.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                            newInfo.information = string.Format("{0}", CommonStr.strRemoteOpenDoorOK);
                            wgRunInfoLog.addEvent(newInfo);
                            this.dispDoorStatusByIPComm(controller, item);
                        }
                        this.displayNewestLog();
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lstDoors.Items)
            {
                item.Selected = true;
            }
            this.lstDoors.Focus();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            if (this.btnServer.Enabled)
            {
                if (this.lstDoors.SelectedItems.Count <= 0)
                {
                    XMessageBox.Show(CommonStr.strSelectDoor);
                }
                else
                {
                    if (this.watching == null)
                    {
                        this.watching = new WatchingService();
                        this.watching.EventHandler += new OnEventHandler(this.evtNewInfoCallBack);
                    }
                    this.timerUpdateDoorInfo.Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    this.watchingStartTime = DateTime.Now;
                    Dictionary<int, icController> dictionary = new Dictionary<int, icController>();
                    foreach (ListViewItem item in this.lstDoors.Items)
                    {
                        (item.Tag as DoorSetInfo).Selected = 0;
                    }
                    arrSelectedDoors.Clear();
                    arrSelectedDoorsItem.Clear();
                    foreach (ListViewItem item2 in this.lstDoors.SelectedItems)
                    {
                        arrSelectedDoors.Add(item2.Text);
                        arrSelectedDoorsItem.Add(item2);

                        (item2.Tag as DoorSetInfo).Selected = 1;
                        if (!dictionary.ContainsKey((item2.Tag as DoorSetInfo).ControllerSN))
                        {
                            wgTools.WriteLine("!selectedControllers.ContainsKey(control.ControllerSN)");
                            this.control4btnServer = new icController();
                            this.control4btnServer.GetInfoFromDBByDoorName(item2.Text);
                            dictionary.Add(this.control4btnServer.ControllerSN, this.control4btnServer);
                        }
                    }
                    if (dictionary.Count > 0)
                    {
                        wgTools.WriteLine("selectedControllers.Count=" + dictionary.Count.ToString());
                        this.watching.WatchingController = dictionary;
                        this.timerUpdateDoorInfo.Interval = 300;
                        this.timerUpdateDoorInfo.Enabled = true;
                        wgAppRunInfo.raiseAppRunInfoMonitors("1");
                    }
                    else
                    {
                        wgTools.WriteLine("selectedControllers.Count=" + dictionary.Count.ToString());
                        this.watching.WatchingController = null;
                        this.timerUpdateDoorInfo.Enabled = false;
                        wgAppRunInfo.raiseAppRunInfoMonitors("0");
                    }
                    (sender as ToolStripButton).BackColor = Color.Green;
                    this.btnStopMonitor.BackColor = Color.Red;
                    this.btnStopOthers.BackColor = Color.Red;
                    (sender as ToolStripButton).Text = CommonStr.strMonitoring;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            if (this.lstDoors.SelectedItems.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectDoor);
                return;
            }
            if ((this.lstDoors.SelectedItems.Count > 0) && (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK))
            {
                return;
            }
            this.btnStopOthers.BackColor = Color.Red;
            this.btnStopMonitor.BackColor = Color.Red;
            using (icController controller = new icController())
            {
                this.bStopComm = false;
                foreach (ListViewItem item in this.lstDoors.SelectedItems)
                {
                    if (this.bStopComm)
                    {
                        goto Label_016B;
                    }
                    controller.GetInfoFromDBByDoorName(item.Text);
                    DateTime now = DateTime.Now;
                    int door = controller.GetDoorNO(item.Text) - 1;
                    if (controller.AdjustTimeIP(now, door) <= 0)
                    {
                        wgRunInfoLog.addEventNotConnect(controller.ControllerSN, controller.IP, item);
                    }
                    else
                    {
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                        newInfo.information = string.Format("{0}:{1}", CommonStr.strAdjustTimeOK, now.ToString("yyyy-MM-dd HH:mm:ss"));
                        wgRunInfoLog.addEvent(newInfo);
                        this.dispDoorStatusByIPComm(controller, item);
                    }
                    this.displayNewestLog();
                }
            }
        Label_016B:
            if ((this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting) && (this.btnServer.Text != CommonStr.strMonitoring))
            {
                this.btnStopOthers.BackColor = Color.Transparent;
                this.btnStopMonitor.BackColor = Color.Transparent;
            }
        }

        private void btnStopMonitor_Click(object sender, EventArgs e)
        {
            if (this.watching != null)
            {
                this.watching.WatchingController = null;
                this.timerUpdateDoorInfo.Enabled = false;
                wgAppRunInfo.raiseAppRunInfoMonitors("0");
            }
        }

        private void btnStopOthers_Click(object sender, EventArgs e)
        {
            if (this.watching != null)
            {
                this.watching.WatchingController = null;
                this.timerUpdateDoorInfo.Enabled = false;
                wgAppRunInfo.raiseAppRunInfoMonitors("0");
            }
            this.bStopComm = true;
            wgMjControllerPrivilege.StopUpload();
            wgMjControllerSwipeOperate.StopGetRecord();
            if (this.bkUploadAndGetRecords.IsBusy)
            {
                this.bkUploadAndGetRecords.CancelAsync();
            }
            lock (this.QueRecText.SyncRoot)
            {
                this.QueRecText.Clear();
            }
            this.btnServer.BackColor = Color.Transparent;
            this.btnServer.Text = this.strRealMonitor;
            Interlocked.Exchange(ref dealingTxt, 0);
            this.btnRealtimeGetRecords.Enabled = true;
            this.btnStopOthers.BackColor = Color.Transparent;
            this.btnStopMonitor.BackColor = Color.Transparent;
            this.btnGetRecords.Enabled = true;
            this.btnUpload.Enabled = true;
            this.btnServer.Enabled = true;
            this.btnStopOthers.BackColor = Color.Transparent;
            this.btnStopMonitor.BackColor = Color.Transparent;
            Cursor.Current = Cursors.Default;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (!this.bkUploadAndGetRecords.IsBusy)
            {
                if (this.lstDoors.SelectedItems.Count <= 0)
                {
                    XMessageBox.Show(CommonStr.strSelectDoor);
                }
                else
                {
                    using (dfrmUploadOption option = new dfrmUploadOption())
                    {
                        option.ShowDialog(this);
                        if (option.checkVal == 0)
                        {
                            return;
                        }
                        this.CommOperateOption = option.checkVal;
                    }
                    this.btnRealtimeGetRecords.Enabled = false;
                    this.btnStopOthers.BackColor = Color.Red;
                    this.btnStopMonitor.BackColor = Color.Red;
                    this.btnGetRecords.Enabled = false;
                    this.btnUpload.Enabled = false;
                    this.arrSelectedDoors.Clear();
                    this.arrSelectedDoorsItem.Clear();
                    this.dealtDoorIndex = 0;
                    this.arrDealtController.Clear();
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        this.arrSelectedDoors.Add(item.Text);
                        this.arrSelectedDoorsItem.Add(item);
                    }
                    using (icController controller = new icController())
                    {
                        controller.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), controller.ControllerSN);
                        newInfo.information = string.Format("{0}", CommonStr.strUploadStart);
                        wgRunInfoLog.addEvent(newInfo);
                    }
                    this.displayNewestLog();
                    this.CommOperate = "UPLOAD";
                    this.bkUploadAndGetRecords.RunWorkerAsync();
                    if (!this.bkDispDoorStatus.IsBusy)
                    {
                        this.bkDispDoorStatus.RunWorkerAsync();
                    }
                }
            }
        }

        private void btnWarnExisted_Click(object sender, EventArgs e)
        {
            this.btnWarnExisted.Visible = false;
        }

        private void cboZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            if (this.dvDoors != null)
            {
                DataView dvDoors = this.dvDoors;
                if ((this.cboZone.SelectedIndex < 0) || ((this.cboZone.SelectedIndex == 0) && (((int) this.arrZoneID[0]) == 0)))
                {
                    dvDoors.RowFilter = "";
                    str = "";
                    if (this.listViewNotDisplay.Items.Count == 0)
                    {
                        wgAppRunInfo.raiseAppRunInfoLoadNums(this.lstDoors.Items.Count.ToString());
                        return;
                    }
                }
                else
                {
                    if ((this.lstDoors.Items.Count + this.listViewNotDisplay.Items.Count) > 100)
                    {
                        this.dfrmWait1.Show();
                        this.dfrmWait1.Refresh();
                    }
                    this.lstDoors.BeginUpdate();
                    this.listViewNotDisplay.BeginUpdate();
                    foreach (ListViewItem item in this.lstDoors.Items)
                    {
                        this.lstDoors.Items.Remove(item);
                        this.listViewNotDisplay.Items.Add(item);
                    }
                    this.listViewNotDisplay.EndUpdate();
                    this.lstDoors.EndUpdate();
                    dvDoors.RowFilter = "f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
                    str = " f_ZoneID =" + this.arrZoneID[this.cboZone.SelectedIndex];
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    num2 = (int) this.arrZoneID[this.cboZone.SelectedIndex];
                    num = (int) this.arrZoneNO[this.cboZone.SelectedIndex];
                    num3 = icControllerZone.getZoneChildMaxNo(this.cboZone.Text, this.arrZoneName, this.arrZoneNO);
                    if (num > 0)
                    {
                        if (num >= num3)
                        {
                            dvDoors.RowFilter = string.Format(" f_ZoneID ={0:d} ", num2);
                            str = string.Format(" f_ZoneID ={0:d} ", num2);
                        }
                        else
                        {
                            dvDoors.RowFilter = "";
                            string str2 = "";
                            for (int i = 0; i < this.arrZoneNO.Count; i++)
                            {
                                if ((((int) this.arrZoneNO[i]) <= num3) && (((int) this.arrZoneNO[i]) >= num))
                                {
                                    if (str2 == "")
                                    {
                                        str2 = str2 + string.Format(" f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                    }
                                    else
                                    {
                                        str2 = str2 + string.Format(" OR f_ZoneID ={0:d} ", (int) this.arrZoneID[i]);
                                    }
                                }
                            }
                            dvDoors.RowFilter = string.Format("  {0} ", str2);
                            str = string.Format("  {0} ", str2);
                        }
                    }
                    dvDoors.RowFilter = string.Format(" {0} ", str);
                }
                if ((this.lstDoors.Items.Count + this.listViewNotDisplay.Items.Count) > 100)
                {
                    this.dfrmWait1.Show();
                    this.dfrmWait1.Refresh();
                }
                this.lstDoors.BeginUpdate();
                this.listViewNotDisplay.BeginUpdate();
                foreach (ListViewItem item2 in this.listViewNotDisplay.Items)
                {
                    if (str != "")
                    {
                        this.dvDoors.RowFilter = string.Format("({0}) AND (f_DoorName = {1})", str, wgTools.PrepareStr(item2.Text));
                    }
                    else
                    {
                        this.dvDoors.RowFilter = string.Format("f_DoorName = {0}", wgTools.PrepareStr(item2.Text));
                    }
                    if (this.dvDoors.Count > 0)
                    {
                        this.listViewNotDisplay.Items.Remove(item2);
                        this.lstDoors.Items.Add(item2);
                    }
                }
                this.listViewNotDisplay.EndUpdate();
                this.lstDoors.EndUpdate();
                this.dfrmWait1.Hide();
                wgTools.WriteLine("foreach (ListViewItem itm in listViewNotDisplay.Items)");
                wgAppRunInfo.raiseAppRunInfoLoadNums(this.lstDoors.Items.Count.ToString());
            }
            else
            {
                wgAppRunInfo.raiseAppRunInfoLoadNums("0");
            }
        }

        private void checkParam(string shouldBe, string inFact, string title, string desc, bool bEnable)
        {
            wgTools.WriteLine(title);
            if (shouldBe != inFact)
            {
                InfoRow newInfo = new InfoRow();
                newInfo.desc = "[" + desc + "]" + CommonStr.strNeedUpload;
                newInfo.information = title + ": " + CommonStr.strShouldBe + shouldBe + CommonStr.strInfact + inFact;
                newInfo.category = 0x1f5;
                wgRunInfoLog.addEvent(newInfo);
            }
        }

        private void checkParamPrivileges(string doorName, icController controller, int infactPrivileges)
        {
            if (wgAppConfig.IsAccessDB)
            {
                this.checkParamPrivileges_Acc(doorName, controller, infactPrivileges);
            }
            else
            {
                try
                {
                    if ((controller.ControllerID <= 0x3e7) && !wgMjController.IsElevator(controller.ControllerSN))
                    {
                        bool flag = false;
                        string cmdText = "SELECT * FROM t_b_Controller WHERE f_ControllerID = " + controller.ControllerID.ToString();
                        using (SqlConnection connection = new SqlConnection(wgAppConfig.dbConString))
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            if (wgAppConfig.getSystemParamByNO(0x35) == "1")
                            {
                                this.cm4ParamPrivilege = new SqlCommand(cmdText, connection);
                                this.cm4ParamPrivilege.CommandText = " SELECT row_count  FROM sys.dm_db_partition_stats WHERE object_id = OBJECT_ID('t_d_privilege') AND partition_number = " + controller.ControllerID.ToString();
                                string str2 = wgTools.SetObjToStr(this.cm4ParamPrivilege.ExecuteScalar());
                                if (!string.IsNullOrEmpty(str2))
                                {
                                    int num = int.Parse(str2);
                                    this.cm4ParamPrivilege.CommandText = cmdText;
                                    SqlDataReader reader = this.cm4ParamPrivilege.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        if ((!flag && (((int) reader["f_lastConsoleUploadPrivilege"]) != num)) && ((wgTools.SetObjToStr(reader["f_lastDelAddDateTime"]) == wgTools.SetObjToStr(reader["f_lastConsoleUploadDateTime"])) && (string.Compare(wgTools.SetObjToStr(reader["f_lastDelAddDateTime"]), wgTools.SetObjToStr(reader["f_lastDelAddAndUploadDateTime"])) > 0)))
                                        {
                                            flag = true;
                                        }
                                        if (!flag && !string.IsNullOrEmpty(wgTools.SetObjToStr(reader["f_lastDelAddDateTime"])))
                                        {
                                            if (wgTools.SetObjToStr(reader["f_lastDelAddDateTime"]) != wgTools.SetObjToStr(reader["f_lastConsoleUploadDateTime"]))
                                            {
                                                flag = true;
                                            }
                                            else if ((string.Compare(wgTools.SetObjToStr(reader["f_lastDelAddDateTime"]), wgTools.SetObjToStr(reader["f_lastDelAddAndUploadDateTime"])) > 0) && (wgTools.SetObjToStr(reader["f_lastConsoleUploadConsuemrsTotal"]) != wgTools.SetObjToStr(infactPrivileges)))
                                            {
                                                flag = true;
                                            }
                                        }
                                        if ((!flag && ((num == 0) || (infactPrivileges == 0))) && (num != infactPrivileges))
                                        {
                                            flag = true;
                                        }
                                        if (flag)
                                        {
                                            InfoRow newInfo = new InfoRow();
                                            newInfo.desc = "[" + doorName + "]" + CommonStr.strPrivileges + CommonStr.strNeedUpload;
                                            newInfo.information = string.Format(("[" + controller.ControllerSN.ToString() + "]" + CommonStr.strPrivileges + CommonStr.strNeedUpload) + " [{0:d}-{1:d}],[{2:d}-{3:d}-{4:d}]", new object[] { infactPrivileges, num, (int) reader["f_lastConsoleUploadConsuemrsTotal"], (int) reader["f_lastConsoleUploadPrivilege"], (int) reader["f_lastConsoleUploadValidPrivilege"] });
                                            newInfo.category = 0x1f5;
                                            wgRunInfoLog.addEvent(newInfo);
                                        }
                                    }
                                    reader.Close();
                                }
                            }
                            else
                            {
                                this.cm4ParamPrivilege = new SqlCommand("select rowcnt from sysindexes where id=object_id(N't_d_Privilege') and name = N'PK_t_d_Privilege'", connection);
                                if (int.Parse(this.cm4ParamPrivilege.ExecuteScalar().ToString()) <= 0x1e8480)
                                {
                                    this.cm4ParamPrivilege = new SqlCommand(cmdText, connection);
                                    this.cm4ParamPrivilege.CommandText = " SELECT COUNT( DISTINCT t_b_Consumer.f_ConsumerNO) FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID and f_ControllerID =" + controller.ControllerID.ToString();
                                    string str4 = wgTools.SetObjToStr(this.cm4ParamPrivilege.ExecuteScalar());
                                    if (!string.IsNullOrEmpty(str4))
                                    {
                                        int num3 = int.Parse(str4);
                                        if (num3 != infactPrivileges)
                                        {
                                            InfoRow row2 = new InfoRow();
                                            row2.desc = "[" + doorName + "]" + CommonStr.strPrivileges + CommonStr.strNeedUpload;
                                            row2.information = string.Format(("[" + controller.ControllerSN.ToString() + "]" + CommonStr.strPrivileges + CommonStr.strNeedUpload) + " [{0:d}-{1:d}],[{2:d}-{3:d}-{4:d}]", new object[] { infactPrivileges, num3, 9, 9, 9 });
                                            row2.category = 0x1f5;
                                            wgRunInfoLog.addEvent(row2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        private void checkParamPrivileges_Acc(string doorName, icController controller, int infactPrivileges)
        {
            OleDbCommand command = null;
            try
            {
                if ((controller.ControllerID <= 0x3e7) && !wgMjController.IsElevator(controller.ControllerSN))
                {
                    string cmdText = "SELECT * FROM t_b_Controller WHERE f_ControllerID = " + controller.ControllerID.ToString();
                    using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        command = new OleDbCommand(cmdText, connection);
                        command.CommandText = " SELECT COUNT(*) FROM (SELECT DISTINCT t_b_Consumer.f_ConsumerNO FROM t_b_Consumer, t_d_Privilege WHERE t_b_Consumer.f_DoorEnabled=1 AND f_ConsumerNO IS NOT NULL AND t_b_Consumer.f_ConsumerID = t_d_Privilege.f_ConsumerID  and f_ControllerID =" + controller.ControllerID.ToString() + ")";
                        string str2 = wgTools.SetObjToStr(command.ExecuteScalar());
                        if (!string.IsNullOrEmpty(str2))
                        {
                            int num = int.Parse(str2);
                            if (num != infactPrivileges)
                            {
                                InfoRow newInfo = new InfoRow();
                                newInfo.desc = "[" + doorName + "]" + CommonStr.strPrivileges + CommonStr.strNeedUpload;
                                newInfo.information = string.Format(("[" + controller.ControllerSN.ToString() + "]" + CommonStr.strPrivileges + CommonStr.strNeedUpload) + " [{0:d}-{1:d}],[{2:d}-{3:d}-{4:d}]", new object[] { infactPrivileges, num, 9, 9, 9 });
                                newInfo.category = 0x1f5;
                                wgRunInfoLog.addEvent(newInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void clearRunInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnClearRunInfo.PerformClick();
        }

        private void dgvRunInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvRunInfo.Columns[e.ColumnIndex].Name.Equals("f_Category"))
                {
                    string stringValue = e.Value as string;
                    if (stringValue != null)
                    {
                        DataGridViewCell cell = this.dgvRunInfo[e.ColumnIndex, e.RowIndex];
                        cell.ToolTipText = stringValue;
                        DataGridViewRow dgvr = this.dgvRunInfo.Rows[e.RowIndex];
                        e.Value = InfoRow.getImage(stringValue, ref dgvr);
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void dgvRunInfo_KeyDown(object sender, KeyEventArgs e)
        {
            this.frmConsole_KeyDown(sender, e);
        }

        private void dgvRunInfo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvRunInfo.SelectedRows.Count > 0)
                {
                    if (string.IsNullOrEmpty(this.oldInfoTitleString))
                    {
                        this.oldInfoTitleString = this.dataGridView2.Columns[0].HeaderText;
                    }
                    this.pictureBox1.Visible = false;
                    this.txtInfo.Text = this.dgvRunInfo.SelectedRows[0].Cells["f_Detail"].Value as string;
                    this.richTxtInfo.Text = this.dgvRunInfo.SelectedRows[0].Cells["f_Detail"].Value as string;
                    this.dataGridView2.Columns[0].HeaderText = this.oldInfoTitleString + "  [" + this.dgvRunInfo.SelectedRows[0].Cells["f_RecID"].Value.ToString() + "/" + this.dgvRunInfo.RowCount.ToString() + "]";
                    if (!string.IsNullOrEmpty(this.dgvRunInfo.SelectedRows[0].Cells["f_MjRecStr"].Value as string))
                    {
                        string recStr = (string)this.dgvRunInfo.SelectedRows[0].Cells["f_MjRecStr"].Value;
                        MjRec rec = new MjRec(recStr, false);
                        if (rec.IsSwipeRecord)
                        {
                            int rec_id = Convert.ToInt32(recStr.Substring(0x30, 8), 0x10);
                            loadPhoto(rec, rec_id);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        public void directToRealtimeGet()
        {
            Cursor.Current = Cursors.WaitCursor;
            Thread.Sleep(0x3e8);
            if (this.arrSelectDoors4Sign.Count == 0)
            {
                this.btnSelectAll.PerformClick();
            }
            else
            {
                try
                {
                    for (int i = 0; i <= (this.lstDoors.Items.Count - 1); i++)
                    {
                        if (this.arrSelectDoors4Sign.IndexOf(this.dvDoors[i]["f_ControllerID"]) >= 0)
                        {
                            this.lstDoors.Items[i].Selected = true;
                        }
                        else
                        {
                            this.lstDoors.Items[i].Selected = false;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            this.bMainWindowDisplay = false;
            this.bDirectToRealtimeGet = true;
            this.btnRealtimeGetRecords.PerformClick();
        }

        private void dispDoorStatusByIPComm(icController control, ListViewItem itm)
        {
            try
            {
                if (control.GetControllerRunInformationIP() <= 0)
                {
                    base.Invoke(new itmDisplayStatus(this.itmDisplayStatusEntry), new object[] { itm, 3 });
                }
                else
                {
                    base.Invoke(new itmDisplayStatus(this.itmDisplayStatusEntry), new object[] { itm, control.runinfo.GetDoorImageIndex(control.GetDoorNO(itm.Text)) });
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void displayMoreSwipesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.frmMoreRecords != null) && !this.frmMoreRecords.Visible)
                {
                    this.frmMoreRecords.Close();
                    this.frmMoreRecords = null;
                }
                if (this.frmMoreRecords == null)
                {
                    this.frmMoreRecords = new frmWatchingMoreRecords();
                    this.frmMoreRecords.tbRunInfoLog = this.tbRunInfoLog;
                }
                this.frmMoreRecords.Show(this);
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void displayNewestLog()
        {
            if (this.dgvRunInfo.Rows.Count > 0)
            {
                this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.dgvRunInfo.Rows.Count - 1;
                this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = true;
                this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = false;
                Application.DoEvents();
            }
        }

        private void displayTools()
        {
            if (!this.grpTool.Visible)
            {
                this.chkNeedCheckLosePacket.Checked = this.bNeedCheckLosePacket;
                this.chkDisplayNewestSwipe.Checked = !string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DISPLAY_NEWEST_SWIPE"));
                this.grpTool.Visible = true;
                this.grpTool.Size = new Size(310, 0xdd);
            }
        }

        private void evtCommCallBack(string text)
        {
            wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
            wgCommReceivedPktCount++;
            lock (this.wgCommQueRecText.SyncRoot)
            {
                this.wgCommQueRecText.Enqueue(text);
            }
        }

        private void evtNewInfoCallBack(string text)
        {
            wgTools.WgDebugWrite("Got text through callback! {0}", new object[] { text });
            receivedPktCount++;
            lock (this.QueRecText.SyncRoot)
            {
                this.QueRecText.Enqueue(text);
            }
        }

        private void frmConsole_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this.watching != null)
                {
                    this.watching.StopWatch();
                }
                if (this.dfrmFind1 != null)
                {
                    this.dfrmFind1.ReallyCloseForm();
                }
                if (this.dfrmWait1 != null)
                {
                    this.dfrmWait1.Close();
                }
                if (this.frmMoreRecords != null)
                {
                    this.frmMoreRecords.ReallyCloseForm();
                }
                if (this.frmMaps1 != null)
                {
                    try
                    {
                        this.frmMaps1.Dispose();
                        this.frmMaps1 = null;
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                }
                if (this.frm4ShowLocate != null)
                {
                    try
                    {
                        this.frm4ShowLocate.Dispose();
                        this.frm4ShowLocate = null;
                    }
                    catch (Exception exception2)
                    {
                        wgAppConfig.wgLog(exception2.ToString());
                    }
                }
                if (this.frm4ShowPersonsInside != null)
                {
                    try
                    {
                        this.frm4ShowPersonsInside.Dispose();
                        this.frm4ShowPersonsInside = null;
                    }
                    catch (Exception exception3)
                    {
                        wgAppConfig.wgLog(exception3.ToString());
                    }
                }
                if (this.frm4PCCheckAccess != null)
                {
                    try
                    {
                        this.frm4PCCheckAccess.Dispose();
                        this.frm4PCCheckAccess = null;
                    }
                    catch (Exception exception4)
                    {
                        wgAppConfig.wgLog(exception4.ToString());
                    }
                }
                this.control4uploadPrivilege = null;
                this.controlConfigure4uploadPrivilege = null;
                this.controlTaskList4uploadPrivilege = null;
                this.swipe4GetRecords = null;
                wgAppConfig.DisposeImage(this.pictureBox1.Image);
                wgAppRunInfo.raiseAppRunInfoMonitors("");
            }
            catch (Exception exception5)
            {
                wgAppConfig.wgLog(exception5.ToString());
            }
        }

        private void frmConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.stepOfRealtimeGetRecords != StepOfRealtimeGetReocrds.Stop)
                {
                    this.btnStopOthers.PerformClick();
                }
                long num2 = DateTime.Now.Ticks + 0x8f0d180L;
                while (DateTime.Now.Ticks < num2)
                {
                    if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.Stop)
                    {
                        break;
                    }
                    Application.DoEvents();
                }
                this.btnStopOthers.PerformClick();
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        public void frmConsole_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((this.frm4PCCheckAccess != null) && this.frm4PCCheckAccess.bDealing)
                {
                    this.frm4PCCheckAccess.Focus();
                }
                if ((e.Control && (e.KeyValue == 70)) || (e.KeyValue == 0x72))
                {
                    if (this.dfrmFind1 == null)
                    {
                        this.dfrmFind1 = new dfrmFind();
                        this.dfrmFind1.StartPosition = FormStartPosition.Manual;
                        this.dfrmFind1.Location = new Point(600, 8);
                    }
                    this.dfrmFind1.setObjtoFind(this.lstDoors, null);
                }
                if (e.Control && (e.KeyValue == 0x41))
                {
                    this.btnSelectAll.PerformClick();
                }
                if (e.Control && (e.KeyValue == 0x30))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.btnRemoteOpen.Visible)
                    {
                        this.btnDirectSetDoorControl();
                    }
                }
                if ((e.Control && !e.Shift) && (e.KeyValue == 0x79))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    using (dfrmInputNewName name = new dfrmInputNewName())
                    {
                        name.setPasswordChar('*');
                        if ((name.ShowDialog(this) != DialogResult.OK) || (name.strNewName != "668899"))
                        {
                            return;
                        }
                    }
                    using (dfrmCommPSet set = new dfrmCommPSet())
                    {
                        set.Text = CommonStr.strSaveAsConfigureFile;
                        if (set.ShowDialog(this) == DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(set.CurrentPwd))
                            {
                                wgAppConfig.UpdateKeyVal("CommCurrent", "");
                            }
                            else
                            {
                                wgAppConfig.UpdateKeyVal("CommCurrent", WGPacket.Ept(set.CurrentPwd));
                                wgAppConfig.SaveNewXmlFile("CommCurrent", WGPacket.Ept(set.CurrentPwd));
                            }
                            wgTools.CommPStr = wgAppConfig.GetKeyVal("CommCurrent");
                            wgAppConfig.wgLog(".pCurr_" + wgAppConfig.GetKeyVal("CommCurrent"));
                        }
                    }
                }
                if ((e.Control && !e.Shift) && (e.KeyValue == 120))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    using (dfrmCommPSet set2 = new dfrmCommPSet())
                    {
                        set2.Text = CommonStr.strSetCommPassword;
                        bool flag = false;
                        if (set2.ShowDialog(this) == DialogResult.OK)
                        {
                            string keyVal = wgAppConfig.GetKeyVal("CommCurrent");
                            if (string.IsNullOrEmpty(set2.CurrentPwd))
                            {
                                if (string.IsNullOrEmpty(keyVal))
                                {
                                    flag = true;
                                }
                            }
                            else if (string.Compare(WGPacket.Ept(set2.CurrentPwd), keyVal) == 0)
                            {
                                flag = true;
                            }
                            if (!flag)
                            {
                                XMessageBox.Show(this, CommonStr.strNewPwdNotAsSameInSystem, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            if (flag)
                            {
                                this.UploadCommPassword(set2.CurrentPwd);
                                wgTools.CommPStr = wgAppConfig.GetKeyVal("CommCurrent");
                            }
                        }
                    }
                }
                if ((e.Control && e.Alt) && (e.KeyValue == 0x31))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    using (dfrmInputNewName name2 = new dfrmInputNewName())
                    {
                        name2.Text = CommonStr.strRestorAllSwipeRecords;
                        name2.setPasswordChar('*');
                        if ((name2.ShowDialog(this) != DialogResult.OK) || (name2.strNewName != "5678"))
                        {
                            return;
                        }
                        this.RestoreAllSwipeInTheControllers();
                    }
                }
                if ((e.Control && e.Shift) && (e.KeyValue == 80))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    this.displayTools();
                }
                if ((e.Control && e.Shift) && (e.KeyValue == 0x4c))
                {
                    if (icOperator.OperatorID != 1)
                    {
                        XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (DateTime.Now > this.dtlstDoorViewChange.AddSeconds(3.0))
                    {
                        this.dtlstDoorViewChange = DateTime.Now;
                        if (this.lstDoors.View == View.Details)
                        {
                            this.lstDoors.View = View.LargeIcon;
                        }
                        else if (this.lstDoors.View == View.LargeIcon)
                        {
                            this.lstDoors.View = View.List;
                        }
                        else if (this.lstDoors.View == View.List)
                        {
                            this.lstDoors.View = View.SmallIcon;
                        }
                        else if (this.lstDoors.View == View.SmallIcon)
                        {
                            this.lstDoors.View = View.Tile;
                        }
                        else if (this.lstDoors.View == View.Tile)
                        {
                            this.lstDoors.View = View.LargeIcon;
                        }
                        else
                        {
                            this.lstDoors.View = View.LargeIcon;
                        }
                        wgTools.WgDebugWrite(this.lstDoors.View.ToString(), new object[0]);
                        wgAppConfig.UpdateKeyVal("CONSOLE_DOORVIEW", this.lstDoors.View.ToString());
                    }
                }
                if (e.Control && (e.KeyValue == 0x43))
                {
                    string data = "";
                    for (int i = 0; i < this.dgvRunInfo.Rows.Count; i++)
                    {
                        for (int j = 0; j < this.dgvRunInfo.ColumnCount; j++)
                        {
                            data = data + this.dgvRunInfo.Rows[i].Cells[j].Value.ToString().Replace("\r\n", ",") + "\t";
                        }
                        data = data + "\r\n";
                    }
                    Clipboard.SetDataObject(data, false);
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void frmConsole_Load(object sender, EventArgs e)
        {
            if (wgAppConfig.FileIsExisted(wgAppConfig.Path4PhotoDefault() + "invalidCard.WAV"))
            {
                this.player = new SoundPlayer();
                this.player.SoundLocation = wgAppConfig.Path4PhotoDefault() + "invalidCard.WAV";
            }
            wgTools.WriteLine("frmConsole_Load Start");
            this.btnWarnExisted.Visible = false;
            if (this.totalConsoleMode == 0)
            {
                this.loadOperatorPrivilege();
            }
            else
            {
                this.btnCheck.Visible = false;
                this.btnSetTime.Visible = false;
                this.btnUpload.Visible = false;
                this.btnServer.Visible = false;
                this.btnGetRecords.Visible = false;
                this.btnRemoteOpen.Visible = false;
                this.mnuCheck.Visible = false;
                switch (this.totalConsoleMode)
                {
                    case 1:
                        this.btnCheck.Visible = true;
                        this.mnuCheck.Visible = true;
                        break;

                    case 2:
                        this.btnSetTime.Visible = true;
                        break;

                    case 3:
                        this.btnUpload.Visible = true;
                        break;

                    case 4:
                        this.btnServer.Visible = true;
                        break;

                    case 5:
                        this.btnGetRecords.Visible = true;
                        break;

                    case 6:
                        this.btnRemoteOpen.Visible = true;
                        break;
                }
            }
            this.bPCCheckAccess = wgAppConfig.getParamValBoolByNO(0x89);
            this.loadDoorData();
            this.txtInfo.Text = "";
            this.richTxtInfo.Text = "";
            wgRunInfoLog.init(out this.tbRunInfoLog);
            this.dv = new DataView(this.tbRunInfoLog);
            this.dgvRunInfo.AutoGenerateColumns = false;
            this.dgvRunInfo.DataSource = this.dv;
            this.dgvRunInfo.Columns[0].DataPropertyName = "f_Category";
            this.dgvRunInfo.Columns[1].DataPropertyName = "f_RecID";
            this.dgvRunInfo.Columns[2].DataPropertyName = "f_Time";
            this.dgvRunInfo.Columns[3].DataPropertyName = "f_Desc";
            this.dgvRunInfo.Columns[4].DataPropertyName = "f_Info";
            this.dgvRunInfo.Columns[5].DataPropertyName = "f_Detail";
            this.dgvRunInfo.Columns[6].DataPropertyName = "f_MjRecStr";
            for (int i = 0; i < this.dgvRunInfo.ColumnCount; i++)
            {
                this.dgvRunInfo.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.loadZoneInfo();
            this.btnRemoteOpen.Visible = this.btnRemoteOpen.Visible && wgAppConfig.getParamValBoolByNO(0x7a);
            this.btnMaps.Visible = this.btnMaps.Visible && wgAppConfig.getParamValBoolByNO(0x72);
            this.mnuWarnOutputReset.Visible = wgAppConfig.getParamValBoolByNO(0x7c);
            this.resetPersonInsideToolStripMenuItem.Visible = wgAppConfig.getParamValBoolByNO(0x84);
            infoRowsCount = 0;
            this.strRealMonitor = this.btnServer.Text;
            this.oldInfoTitleString = this.dataGridView2.Columns[0].HeaderText;
            wgTools.WriteLine("frmConsole_Load End");
        }

        private void frmConsole_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.toolTip1.Active)
            {
                this.toolTip1.Active = false;
            }
        }

        public int getAllInfoRowsCount()
        {
            return infoRowsCount;
        }

        private void getRecordsFromController(int result)
        {
            this.control4getRecordsFromController = new icController();
            this.control4getRecordsFromController.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
            this.arrDealtController.Add(this.control4getRecordsFromController.ControllerSN, result);
            int dealtDoorIndex = this.dealtDoorIndex;
            while (dealtDoorIndex < this.arrSelectedDoors.Count)
            {
                this.control4getRecordsFromController.GetInfoFromDBByDoorName(this.arrSelectedDoors[dealtDoorIndex].ToString());
                if (!this.arrDealtController.ContainsKey(this.control4getRecordsFromController.ControllerSN))
                {
                    break;
                }
                if (this.arrDealtController[this.control4getRecordsFromController.ControllerSN] >= 0)
                {
                    InfoRow newInfo = new InfoRow();
                    newInfo.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), this.control4getRecordsFromController.ControllerSN);
                    if (dealtDoorIndex == this.dealtDoorIndex)
                    {
                        newInfo.information = string.Format("{0}--[{1:d}]", CommonStr.strGetSwipeRecordOK, this.arrDealtController[this.control4getRecordsFromController.ControllerSN]);
                        wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), CommonStr.strGetSwipeRecordOK, this.arrDealtController[this.control4getRecordsFromController.ControllerSN]));
                    }
                    else
                    {
                        newInfo.information = string.Format("{0}", CommonStr.strAlreadyGotSwipeRecord);
                    }
                    wgRunInfoLog.addEvent(newInfo);
                }
                else
                {
                    foreach (ListViewItem item in this.lstDoors.Items)
                    {
                        if (item.Text == this.arrSelectedDoors[dealtDoorIndex].ToString())
                        {
                            wgRunInfoLog.addEventNotConnect(this.control4getRecordsFromController.ControllerSN, 
                                this.control4getRecordsFromController.IP, item);
                            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", item.Text, CommonStr.strCommFail));
                            break;
                        }
                    }
                }
                dealtDoorIndex++;
            }
            if (dealtDoorIndex < this.arrSelectedDoors.Count)
            {
                this.dealtDoorIndex = dealtDoorIndex;
                InfoRow row2 = new InfoRow();
                row2.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.control4getRecordsFromController.ControllerSN);
                row2.information = string.Format("{0}", CommonStr.strGetSwipeRecordStart);
                wgRunInfoLog.addEvent(row2);
                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), CommonStr.strGettingSwipeRecord));
            }
            else
            {
                this.dealtDoorIndex = dealtDoorIndex;
                if (this.stepOfRealtimeGetRecords == StepOfRealtimeGetReocrds.Stop)
                {
                    this.btnRealtimeGetRecords.Enabled = true;
                    if ((this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting) && (this.btnServer.Text != CommonStr.strMonitoring))
                    {
                        this.btnStopOthers.BackColor = Color.Transparent;
                        this.btnStopMonitor.BackColor = Color.Transparent;
                    }
                    this.btnGetRecords.Enabled = true;
                    this.btnUpload.Enabled = true;
                }
            }
            this.displayNewestLog();
        }

        private int getRecordsNow()
        {
            this.swipe4GetRecords.Clear();
            return this.swipe4GetRecords.GetSwipeRecordsByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
        }

        private void itmDisplayStatusEntry(ListViewItem itm, int status)
        {
            try
            {
                if (itm != null)
                {
                    itm.ImageIndex = status;
                }
            }
            catch (Exception exception)
            {
                wgTools.WriteLine(exception.ToString());
            }
        }

        private void loadDoorData()
        {
            icControllerZone zone;
            string cmdText = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
            cmdText = (cmdText + " , a.f_ControllerID ") + " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID " + " ORDER BY  a.f_DoorName ";
            this.dt = new DataTable();
            this.dvDoors = new DataView(this.dt);
            this.dvDoors4Watching = new DataView(this.dt);
            this.dvDoors4Check = new DataView(this.dt);
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(this.dt);
                        }
                    }
                    goto Label_0111;
                }
            }
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(cmdText, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(this.dt);
                    }
                }
            }
        Label_0111:
            zone = new icControllerZone();
            zone.getAllowedControllers(ref this.dt);
            try
            {
                this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
            string str2 = "1";
            this.imgDoor2 = new ImageList();
            this.imgDoor2.ImageSize = new Size(0x18, 0x20);
            this.imgDoor2.TransparentColor = SystemColors.Window;
            str2 = wgAppConfig.getSystemParamByNO(0x16);
            if (!string.IsNullOrEmpty(str2))
            {
                decimal num = decimal.Parse(str2, CultureInfo.InvariantCulture);
                if (((num != 1M) && (num > 0M)) && (num < 100M))
                {
                    this.imgDoor2.ImageSize = new Size((int) (24M * num), (int) (32M * num));
                }
            }
            this.imgDoor2.Images.Add(Resources.pConsole_Door_Unknown);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_NormalClose);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_NormalOpen);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_NotConnected);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnClose);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnOpen);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_Unknown);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnClose);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_WarnOpen);
            this.imgDoor2.Images.Add(Resources.pConsole_Door_NotConnected);
            this.lstDoors.LargeImageList = this.imgDoor2;
            this.lstDoors.SmallImageList = this.imgDoor2;
            try
            {
                if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("CONSOLE_DOORVIEW")))
                {
                    string keyVal = wgAppConfig.GetKeyVal("CONSOLE_DOORVIEW");
                    if (keyVal == View.Details.ToString())
                    {
                        this.lstDoors.View = View.LargeIcon;
                    }
                    else if (keyVal == View.LargeIcon.ToString())
                    {
                        this.lstDoors.View = View.LargeIcon;
                    }
                    else if (keyVal == View.List.ToString())
                    {
                        this.lstDoors.View = View.List;
                    }
                    else if (keyVal == View.SmallIcon.ToString())
                    {
                        this.lstDoors.View = View.SmallIcon;
                    }
                    else if (keyVal == View.Tile.ToString())
                    {
                        this.lstDoors.View = View.Tile;
                    }
                    else
                    {
                        this.lstDoors.View = View.LargeIcon;
                    }
                }
            }
            catch (Exception exception2)
            {
                wgAppConfig.wgLog(exception2.ToString());
            }
            this.lstDoors.Items.Clear();
            if (this.dvDoors.Count > 0)
            {
                wgTools.WriteLine("this.lstDoors.Items.Add(itm); Start");
                this.lstDoors.BeginUpdate();
                for (int i = 0; i < this.dvDoors.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = wgTools.SetObjToStr(this.dvDoors[i]["f_DoorName"]);
                    item.ImageIndex = 0;
                    DoorSetInfo info = new DoorSetInfo((int) this.dvDoors[i]["f_DoorID"], (string) this.dvDoors[i]["f_DoorName"], (byte) this.dvDoors[i]["f_DoorNO"], (int) this.dvDoors[i]["f_ControllerSN"], this.dvDoors[i]["f_IP"].ToString(), (int) this.dvDoors[i]["f_PORT"], (int) this.dvDoors[i]["f_ConnectState"], (int) this.dvDoors[i]["f_ZoneID"]);
                    info.Selected = 0;
                    item.Tag = info;
                    this.lstDoors.Items.Add(item);
                }
                this.lstDoors.EndUpdate();
                wgTools.WriteLine("this.lstDoors.Items.Add(itm); End");
            }
            cmdText = " SELECT a.f_ReaderNO, a.f_ReaderName , b.f_ControllerSN ";
            cmdText = cmdText + " FROM t_b_Reader a, t_b_Controller b WHERE  b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ";
            this.dtReader = new DataTable();
            if (wgAppConfig.IsAccessDB)
            {
                using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command3 = new OleDbCommand(cmdText, connection3))
                    {
                        using (OleDbDataAdapter adapter3 = new OleDbDataAdapter(command3))
                        {
                            adapter3.Fill(this.dtReader);
                        }
                    }
                    goto Label_0675;
                }
            }
            using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command4 = new SqlCommand(cmdText, connection4))
                {
                    using (SqlDataAdapter adapter4 = new SqlDataAdapter(command4))
                    {
                        adapter4.Fill(this.dtReader);
                    }
                }
            }
        Label_0675:
            if (this.dtReader.Rows.Count > 0)
            {
                for (int j = 0; j < this.dtReader.Rows.Count; j++)
                {
                    this.ReaderName.Add(string.Format("{0}-{1}", this.dtReader.Rows[j]["f_ControllerSN"].ToString(), this.dtReader.Rows[j]["f_ReaderNO"].ToString()), this.dtReader.Rows[j]["f_ReaderName"].ToString());
                }
            }
            this.pcCheckAccess_Init();
        }

        private void loadOperatorPrivilege()
        {
            bool flag;
            bool flag2;
            icOperator.getFrmOperatorPrivilege(base.Name.ToString(), out flag, out flag2);
            if (flag2)
            {
                icOperator.getFrmOperatorPrivilege("btnCheckController", out flag, out flag2);
                this.btnCheck.Visible = flag || flag2;
                this.mnuCheck.Visible = this.btnCheck.Visible;
                icOperator.getFrmOperatorPrivilege("btnAdjustTime", out flag, out flag2);
                this.btnSetTime.Visible = flag2;
                icOperator.getFrmOperatorPrivilege("btnUpload", out flag, out flag2);
                this.btnUpload.Visible = flag2;
                icOperator.getFrmOperatorPrivilege("btnMonitor", out flag, out flag2);
                this.btnServer.Visible = flag || flag2;
                icOperator.getFrmOperatorPrivilege("btnGetRecords", out flag, out flag2);
                this.btnGetRecords.Visible = flag2;
                icOperator.getFrmOperatorPrivilege("btnRemoteOpen", out flag, out flag2);
                this.btnRemoteOpen.Visible = flag2;
                icOperator.getFrmOperatorPrivilege("btnRealtimeGetRecords", out flag, out flag2);
                this.btnRealtimeGetRecords.Visible = flag2;
                this.btnMaps.Visible = icOperator.OperatePrivilegeVisible("btnMaps");
            }
            else if (flag)
            {
                icOperator.getFrmOperatorPrivilege("btnCheckController", out flag, out flag2);
                this.btnCheck.Visible = flag || flag2;
                this.mnuCheck.Visible = this.btnCheck.Visible;
                this.btnSetTime.Visible = false;
                this.btnUpload.Visible = false;
                icOperator.getFrmOperatorPrivilege("btnMonitor", out flag, out flag2);
                this.btnServer.Visible = flag || flag2;
                icOperator.getFrmOperatorPrivilege("btnMaps", out flag, out flag2);
                this.btnMaps.Visible = flag2 || flag;
                this.btnGetRecords.Visible = false;
                this.btnRemoteOpen.Visible = false;
                this.btnRealtimeGetRecords.Visible = false;
            }
            else
            {
                base.Close();
            }
        }

        private void loadPhoto(MjRec rec, int rec_id)
        {
            if (this.bMainWindowDisplay && rec_id > 0)
            {
                this.pictureBox1.Visible = false;
                try
                {
                    rec.loadPhotoFromDB(rec_id);
                    Image img = this.pictureBox1.Image;
                    wgAppConfig.ShowImageStream(rec.PhotoData, ref img);
                    if (img != null)
                    {
                        this.pictureBox1.Image = img;
                        this.pictureBox1.Visible = true;
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
            }
        }

        private void loadZoneInfo()
        {
            new icControllerZone().getZone(ref this.arrZoneName, ref this.arrZoneID, ref this.arrZoneNO);
            int count = this.arrZoneID.Count;
            this.cboZone.Items.Clear();
            for (count = 0; count < this.arrZoneID.Count; count++)
            {
                if ((count == 0) && string.IsNullOrEmpty(this.arrZoneName[count].ToString()))
                {
                    this.cboZone.Items.Add(CommonStr.strAllZones);
                }
                else
                {
                    this.cboZone.Items.Add(this.arrZoneName[count].ToString());
                }
            }
            if (this.cboZone.Items.Count > 0)
            {
                this.cboZone.SelectedIndex = 0;
            }
            this.cboZone.Visible = true;
        }

        private void locateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.frm4ShowLocate == null)
                {
                    this.frm4ShowLocate = new dfrmLocate();
                    this.frm4ShowLocate.TopMost = true;
                    this.frm4ShowLocate.Show();
                }
                else
                {
                    try
                    {
                        if (this.frm4ShowLocate.WindowState == FormWindowState.Minimized)
                        {
                            this.frm4ShowLocate.WindowState = FormWindowState.Normal;
                        }
                        this.frm4ShowLocate.Show();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                        this.frm4ShowLocate = null;
                        this.frm4ShowLocate = new dfrmLocate();
                        this.frm4ShowLocate.TopMost = true;
                        this.frm4ShowLocate.Show();
                    }
                }
            }
            catch (Exception exception2)
            {
                wgAppConfig.wgLog(exception2.ToString());
            }
        }

        private void lstDoors_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void mnuCheck_Click(object sender, EventArgs e)
        {
            this.btnCheck.PerformClick();
        }

        private void mnuWarnReset_Click(object sender, EventArgs e)
        {
            using (icController controller = new icController())
            {
                foreach (ListViewItem item in this.lstDoors.SelectedItems)
                {
                    controller.GetInfoFromDBByDoorName(item.Text);
                    if (controller.WarnResetIP() <= 0)
                    {
                        wgRunInfoLog.addEventNotConnect(controller.ControllerSN, controller.IP, item);
                    }
                    else
                    {
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                        newInfo.information = string.Format("{0}", sender.ToString());
                        wgRunInfoLog.addEvent(newInfo);
                    }
                    this.displayNewestLog();
                }
            }
        }

        private void pcCheckAccess_DealNewRecord(MjRec mjrec)
        {
            if (this.bPCCheckAccess)
            {
                mjrec.GetUserInfoFromDB();
                if (!string.IsNullOrEmpty(mjrec.groupname) && mjrec.eventCategory != 4)
                {
                    try
                    {
                        if ((mjrec.ReadDate.Date <= mjrec.endYMD.Date) && (mjrec.ReadDate.Date >= mjrec.beginYMD.Date))
                        {
                            int index = -1;
                            this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjrec.ControllerSN.ToString(), mjrec.DoorNo.ToString());
                            if (this.dvDoors4Watching.Count > 0)
                            {
                                index = this.checkAccess_arrDoorName.IndexOf(this.dvDoors4Watching[0]["f_DoorName"].ToString());
                            }
                            if (index >= 0)
                            {
                                DateTime time5 = (DateTime) this.checkAccess_arrCheckStartTime[index];
                                if ((mjrec.ReadDate > time5.AddSeconds(20.0)) || (mjrec.ReadDate.AddSeconds(20.0) < ((DateTime) this.checkAccess_arrCheckStartTime[index])))
                                {
                                    this.checkAccess_arrCount[index] = 0;
                                }
                                if (((((int) this.checkAccess_arrCount[index]) > 0) && (((byte) this.checkAccess_arrReaderNo[index]) == mjrec.ReaderNo)) && (((string) this.checkAccess_arrGroupName[index]) == mjrec.groupname))
                                {
                                    if (this.checkAccess_arrCardId[index].ToString().IndexOf(mjrec.UserID.ToString().PadLeft(10, '0')) < 0)// TODOJ
                                    {
                                        ArrayList list;
                                        int num5;
                                        ArrayList list2;
                                        int num7;
                                        (list = this.checkAccess_arrCardId)[num5 = index] = list[num5] + "," + mjrec.UserID.ToString().PadLeft(10, '0');//TODOJ
                                        (list2 = this.checkAccess_arrConsumerName)[num7 = index] = list2[num7] + "\r\n" + mjrec.consumerName;
                                        this.checkAccess_arrCheckStartTime[index] = mjrec.ReadDate;
                                        this.checkAccess_arrCount[index] = ((int) this.checkAccess_arrCount[index]) + 1;
                                    }
                                }
                                else
                                {
                                    this.checkAccess_arrReaderNo[index] = mjrec.ReaderNo;
                                    this.checkAccess_arrGroupName[index] = mjrec.groupname;
                                    this.checkAccess_arrCardId[index] = mjrec.UserID.ToString().PadLeft(10, '0');
                                    this.checkAccess_arrConsumerName[index] = mjrec.consumerName;
                                    this.checkAccess_arrCheckStartTime[index] = mjrec.ReadDate;
                                    this.checkAccess_arrCount[index] = 1;
                                }
                                MethodInvoker method = new MethodInvoker(this.pcCheckAccess_DealOpen);
                                base.BeginInvoke(method);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                    }
                }
            }
        }

        private void pcCheckAccess_DealOpen()
        {
            if (this.bPCCheckAccess)
            {
                try
                {
                    if (this.frm4PCCheckAccess == null)
                    {
                        this.frm4PCCheckAccess = new dfrmPCCheckAccess();
                        this.frm4PCCheckAccess.TopMost = true;
                    }
                    if (!this.frm4PCCheckAccess.bDealing)
                    {
                        for (int i = 0; i <= (this.checkAccess_arrDoor.Count - 1); i++)
                        {
                            if (((int) this.checkAccess_arrCount[i]) > 0)
                            {
                                int index = this.checkAccess_arrDB_GroupName.IndexOf(this.checkAccess_arrGroupName[i]);
                                if ((index >= 0) && (((int) this.checkAccess_arrCount[i]) >= ((int) this.checkAccess_arrDB_MoreCards[index])))
                                {
                                    this.checkAccess_arrCount[i] = 0;
                                    this.frm4PCCheckAccess.bDealing = true;
                                    if (this.frm4PCCheckAccess.WindowState == FormWindowState.Minimized)
                                    {
                                        this.frm4PCCheckAccess.WindowState = FormWindowState.Normal;
                                    }
                                    this.frm4PCCheckAccess.strDoorId = this.checkAccess_arrDoor[i].ToString();
                                    this.frm4PCCheckAccess.strDoorFullName = this.checkAccess_arrDoorName[i].ToString();
                                    this.frm4PCCheckAccess.strGroupname = this.checkAccess_arrGroupName[i].ToString();
                                    this.frm4PCCheckAccess.strConsumername = this.checkAccess_arrConsumerName[i].ToString();
                                    this.frm4PCCheckAccess.strNow = ((DateTime) this.checkAccess_arrCheckStartTime[i]).ToString(wgTools.YMDHMSFormat);
                                    this.frm4PCCheckAccess.Show();
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        private void pcCheckAccess_Init()
        {
            if (this.bPCCheckAccess)
            {
                try
                {
                    string str = "";
                    try
                    {
                        string cmdText = " SELECT a.f_GroupID,a.f_GroupName,b.f_GroupType,b.f_MoreCards,b.f_SoundFileName  from t_b_Group a, t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and b.f_GroupType=1 order by f_GroupName ASC";
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
                                        str = wgTools.SetObjToStr(reader["f_SoundFileName"]);
                                    }
                                    reader.Close();
                                }
                                goto Label_0101;
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
                                    str = wgTools.SetObjToStr(reader2["f_SoundFileName"]);
                                }
                                reader2.Close();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                    }
                Label_0101:
                    using (DataView view = new DataView(this.dvDoors.Table))
                    {
                        if (string.IsNullOrEmpty(str))
                        {
                            for (int i = 0; i <= (view.Count - 1); i++)
                            {
                                this.checkAccess_arrDoor.Add(view[i]["f_DoorID"]);
                                this.checkAccess_arrDoorName.Add(view[i]["f_DoorName"]);
                                this.checkAccess_arrReaderNo.Add("");
                                this.checkAccess_arrGroupName.Add("");
                                this.checkAccess_arrCardId.Add("");
                                this.checkAccess_arrConsumerName.Add("");
                                this.checkAccess_arrCheckTime.Add("");
                                this.checkAccess_arrCheckStartTime.Add(DateTime.Now);
                                this.checkAccess_arrCount.Add(-1);
                            }
                        }
                        else
                        {
                            string[] strArray = str.Split(new char[] { ',' });
                            for (int j = 0; j <= (view.Count - 1); j++)
                            {
                                for (int k = 0; k < strArray.Length; k++)
                                {
                                    if (int.Parse(strArray[k]) == ((int) view[j]["f_DoorID"]))
                                    {
                                        this.checkAccess_arrDoor.Add(view[j]["f_DoorID"]);
                                        this.checkAccess_arrDoorName.Add(view[j]["f_DoorName"]);
                                        this.checkAccess_arrReaderNo.Add("");
                                        this.checkAccess_arrGroupName.Add("");
                                        this.checkAccess_arrCardId.Add("");
                                        this.checkAccess_arrConsumerName.Add("");
                                        this.checkAccess_arrCheckTime.Add("");
                                        this.checkAccess_arrCheckStartTime.Add(DateTime.Now);
                                        this.checkAccess_arrCount.Add(-1);
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        string str3 = " SELECT a.f_GroupID,a.f_GroupName,b.f_GroupType,b.f_MoreCards  from t_b_Group a, t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and b.f_CheckAccessActive=1 order by f_GroupName ASC";
                        if (wgAppConfig.IsAccessDB)
                        {
                            using (OleDbConnection connection3 = new OleDbConnection(wgAppConfig.dbConString))
                            {
                                using (OleDbCommand command3 = new OleDbCommand(str3, connection3))
                                {
                                    connection3.Open();
                                    OleDbDataReader reader3 = command3.ExecuteReader();
                                    while (reader3.Read())
                                    {
                                        this.checkAccess_arrDB_GroupName.Add(reader3["f_GroupName"]);
                                        this.checkAccess_arrDB_MoreCards.Add(reader3["f_MoreCards"]);
                                    }
                                    reader3.Close();
                                }
                                return;
                            }
                        }
                        using (SqlConnection connection4 = new SqlConnection(wgAppConfig.dbConString))
                        {
                            using (SqlCommand command4 = new SqlCommand(str3, connection4))
                            {
                                connection4.Open();
                                SqlDataReader reader4 = command4.ExecuteReader();
                                while (reader4.Read())
                                {
                                    this.checkAccess_arrDB_GroupName.Add(reader4["f_GroupName"]);
                                    this.checkAccess_arrDB_MoreCards.Add(reader4["f_MoreCards"]);
                                }
                                reader4.Close();
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        wgTools.WgDebugWrite(exception2.ToString(), new object[] { EventLogEntryType.Error });
                    }
                }
                catch (Exception exception3)
                {
                    wgTools.WgDebugWrite(exception3.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        private void personInsideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.frm4ShowPersonsInside == null)
                {
                    this.frm4ShowPersonsInside = new dfrmPersonsInside();
                    this.frm4ShowPersonsInside.TopMost = true;
                    this.frm4ShowPersonsInside.Show();
                }
                else
                {
                    try
                    {
                        if (this.frm4ShowPersonsInside.WindowState == FormWindowState.Minimized)
                        {
                            this.frm4ShowPersonsInside.WindowState = FormWindowState.Normal;
                        }
                        this.frm4ShowPersonsInside.Show();
                    }
                    catch (Exception exception)
                    {
                        wgAppConfig.wgLog(exception.ToString());
                        this.frm4ShowPersonsInside = null;
                        this.frm4ShowPersonsInside = new dfrmPersonsInside();
                        this.frm4ShowPersonsInside.TopMost = true;
                        this.frm4ShowPersonsInside.Show();
                    }
                }
            }
            catch (Exception exception2)
            {
                wgAppConfig.wgLog(exception2.ToString());
            }
        }

        private void resetPersonInsideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.lstDoors.SelectedItems.Count <= 0)
            {
                XMessageBox.Show(CommonStr.strSelectDoor);
            }
            else if ((this.lstDoors.SelectedItems.Count <= 0) || (XMessageBox.Show(sender.ToString() + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
            {
                using (icController controller = new icController())
                {
                    ArrayList list = new ArrayList();
                    foreach (ListViewItem item in this.lstDoors.SelectedItems)
                    {
                        controller.GetInfoFromDBByDoorName(item.Text);
                        if (list.IndexOf(controller.ControllerSN) < 0)
                        {
                            if (controller.UpdateFRamIP(0x10000002, 0) <= 0)
                            {
                                wgRunInfoLog.addEventNotConnect(controller.ControllerSN, controller.IP, item);
                            }
                            else
                            {
                                InfoRow newInfo = new InfoRow();
                                newInfo.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                                newInfo.information = string.Format("{0}", sender.ToString());
                                wgRunInfoLog.addEvent(newInfo);
                                list.Add(controller.ControllerSN);
                            }
                        }
                        else
                        {
                            InfoRow row2 = new InfoRow();
                            row2.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                            row2.information = string.Format("{0}", sender.ToString());
                            wgRunInfoLog.addEvent(row2);
                        }
                        this.displayNewestLog();
                    }
                }
            }
        }

        private void RestoreAllSwipeInTheControllers()
        {
            using (icController controller = new icController())
            {
                foreach (ListViewItem item in this.lstDoors.SelectedItems)
                {
                    controller.GetInfoFromDBByDoorName(item.Text);
                    if (controller.RestoreAllSwipeInTheControllersIP() <= 0)
                    {
                        wgRunInfoLog.addEventNotConnect(controller.ControllerSN, controller.IP, item);
                    }
                    else
                    {
                        InfoRow newInfo = new InfoRow();
                        newInfo.desc = string.Format("{0}[{1:d}]", item.Text, controller.ControllerSN);
                        newInfo.information = string.Format("{0}", CommonStr.strRestorAllSwipeRecords);
                        wgRunInfoLog.addEvent(newInfo);
                    }
                    this.displayNewestLog();
                }
            }
        }

        private void timerUpdateDoorInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerUpdateDoorInfo.Enabled = false;
                if (this.watching != null)
                {
                    this.updateSelectedDoorsStatus();
                    if (this.QueRecText.Count > 0)
                    {
                        base.Invoke(new txtInfoHaveNewInfo(this.txtInfoHaveNewInfoEntry));
                    }
                    wgAppRunInfo.raiseAppRunInfoLoadNums(infoRowsCount.ToString());
                    Application.DoEvents();
                    this.pcCheckAccess_DealOpen();
                    this.timerUpdateDoorInfo.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                wgAppConfig.wgLog(exception.ToString());
            }
        }

        private void timerWarn_Tick(object sender, EventArgs e)
        {
            this.timerWarn.Enabled = false;
            if (this.btnWarnExisted.Visible)
            {
                if (this.btnWarnExisted.BackColor == Color.Red)
                {
                    this.btnWarnExisted.BackColor = Color.Transparent;
                }
                else
                {
                    this.btnWarnExisted.BackColor = Color.Red;
                }
                SystemSounds.Beep.Play();
                this.timerWarn.Enabled = true;
            }
        }

        private void txtInfoHaveNewInfoEntry()
        {
            if ((dealingTxt <= 0) && (this.watching.WatchingController != null))
            {
                Interlocked.Exchange(ref dealingTxt, 1);
                int num = 0;
                long ticks = DateTime.Now.Ticks;
                long num3 = 0x1312d00L;
                long num4 = ticks + num3;
                bool isSelected = false;
                while (this.QueRecText.Count > 0)
                {
                    object obj2;
                    lock (this.QueRecText.SyncRoot)
                    {
                        obj2 = this.QueRecText.Dequeue();
                    }
                    if (this.txtInfoUpdateEntry(obj2))
                    {
                        if (!isSelected) isSelected = true;
                        infoRowsCount++;
                        num++;
                        if (DateTime.Now.Ticks > num4)
                        {
                            num4 = DateTime.Now.Ticks + num3;
                            if (this.dgvRunInfo.Rows.Count > 0)
                            {
                                this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.dgvRunInfo.Rows.Count - 1;
                                this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = true;
                                Application.DoEvents();
                                wgRunInfoLog.addEventSpecial2();
                            }
                            if (this.watching.WatchingController == null)
                            {
                                break;
                            }
                        }
                    }
                }
                if (isSelected)
                {
                    wgRunInfoLog.addEventSpecial2();
                    this.displayNewestLog();
                    Application.DoEvents();
                }
                Interlocked.Exchange(ref dealingTxt, 0);
            }
        }

        private bool txtInfoUpdateEntry(object info)
        {
            string info_ = info as string;
            if (info_.Length < 0x30)
                return false;

            bool isSelected = false;
            MjRec mjrec = new MjRec(info as string, true);
            if (mjrec.ControllerSN > 0)
            {
                try
                {
                    if (!this.watching.WatchingController.ContainsKey((int) mjrec.ControllerSN))
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                InfoRow newInfo = new InfoRow();
                wgTools.WriteLine("new InfoRow");
                newInfo.category = mjrec.eventCategory;
                newInfo.desc = "";
                if (mjrec.addressIsReader)
                {
                    if (this.ReaderName.ContainsKey(string.Format("{0}-{1}", mjrec.ControllerSN.ToString(), mjrec.ReaderNo.ToString())))
                    {
                        wgTools.WriteLine("ReaderName.ContainsKey(string.Format(");
                        newInfo.desc = this.ReaderName[string.Format("{0}-{1}", mjrec.ControllerSN.ToString(), mjrec.ReaderNo.ToString())];
                        foreach (string item in arrSelectedDoors)
                        {
                            if (newInfo.desc.IndexOf(item) >= 0)
                            {
                                isSelected = true;
                                break;
                            }
                        }
                        mjrec.address = this.ReaderName[string.Format("{0}-{1}", mjrec.ControllerSN.ToString(), mjrec.ReaderNo.ToString())];
                    }
                    else
                    {
                        newInfo.desc = "";
                    }
                }
                else
                {
                    this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjrec.ControllerSN.ToString(), mjrec.DoorNo.ToString());
                    if (this.dvDoors4Watching.Count > 0)
                    {
                        newInfo.desc = this.dvDoors4Watching[0]["f_DoorName"].ToString();
                        foreach (string item in arrSelectedDoors)
                        {
                            if (newInfo.desc.IndexOf(item) >= 0)
                            {
                                isSelected = true;
                                break;
                            }
                        }
                        mjrec.address = this.dvDoors4Watching[0]["f_DoorName"] as string;
                    }
                }
                if (this.player != null)
                {
                    if (mjrec.IsPassed)
                    {
                        SystemSounds.Beep.Play();
                    }
                    else
                    {
                        this.player.Play();
                    }
                }
                this.txtInfoUpdateEntry4RealtimeGetRecords(ref mjrec);
                if (isSelected)
                {
                    newInfo.information = mjrec.ToDisplayInfo();
                    newInfo.detail = mjrec.ToDisplayDetail();
                    newInfo.MjRecStr = (info as string) + string.Format("{0:x8}", mjrec.RecID);
                    wgRunInfoLog.addEventSpecial1(newInfo);
                    if (wgRunInfoLog.logRecEventMode == 1)
                    {
                        wgAppConfig.wglogRecEventOfController(string.Format("Rec: {0}\r\n{1}", newInfo.MjRecStr, newInfo.detail).Replace("\t", ""));
                    }
                    else if (wgRunInfoLog.logRecEventMode == 2)
                    {
                        if (mjrec.addressIsReader)
                        {
                            wgAppConfig.wglogRecEventOfController(string.Format("Rec: {0}\r\n{1}", newInfo.MjRecStr, newInfo.detail).Replace("\t", ""));
                        }
                    }
                    else if ((wgRunInfoLog.logRecEventMode == 3) && !mjrec.addressIsReader)
                    {
                        wgAppConfig.wglogRecEventOfController(string.Format("Rec: {0}\r\n{1}", newInfo.MjRecStr, newInfo.detail).Replace("\t", ""));
                    }
                }
                this.pcCheckAccess_DealNewRecord(mjrec);
            }
            return isSelected;
        }

        private void txtInfoUpdateEntry4pcCheckAccess(MjRec mjrec)
        {
            if (this.bPCCheckAccess)
            {
                try
                {
                    this.pcCheckAccess_DealNewRecord(mjrec);
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
        }

        private void txtInfoUpdateEntry4RealtimeGetRecords(ref MjRec mjrec)
        {
            if ((this.stepOfRealtimeGetRecords != StepOfRealtimeGetReocrds.Stop) && this.realtimeGetRecordsSwipeIndexGot.ContainsKey((int) mjrec.ControllerSN))
            {
                if (mjrec.IndexInDataFlash == ((long) this.realtimeGetRecordsSwipeIndexGot[(int) mjrec.ControllerSN]))
                {
                    if (icSwipeRecord.AddNewSwipe_SynConsumerID(ref mjrec) >= 0)
                    {
                        this.realtimeGetRecordsSwipeIndexGot[(int) mjrec.ControllerSN] = ((int) mjrec.IndexInDataFlash) + 1;
                        this.needDelSwipeControllers[(int) mjrec.ControllerSN] = 1;
                    }
                }
                else if (mjrec.IndexInDataFlash > ((long) this.realtimeGetRecordsSwipeIndexGot[(int) mjrec.ControllerSN]))
                {
                    this.dvDoors4Watching.RowFilter = string.Format("f_ControllerSN={0}  AND f_DoorNO={1}", mjrec.ControllerSN.ToString(), mjrec.DoorNo.ToString());
                    if ((this.dvDoors4Watching.Count > 0) && (this.doorsNeedToGetRecords.IndexOf(this.dvDoors4Watching[0]["f_DoorName"].ToString(), Math.Max(0, this.dealtIndexOfDoorsNeedToGetRecords + 1)) < 0))
                    {
                        this.doorsNeedToGetRecords.Add(this.dvDoors4Watching[0]["f_DoorName"].ToString());
                    }
                }
            }
        }

        private void updateSelectedDoorsStatus()
        {
            if (this.watching != null)
            {
                foreach (ListViewItem item in this.lstDoors.Items)
                {
                    if ((((item.Tag as DoorSetInfo).Selected > 0) && (this.watching.WatchingController != null)) && this.watching.WatchingController.ContainsKey((item.Tag as DoorSetInfo).ControllerSN))
                    {
                        ControllerRunInformation runInfo = this.watching.GetRunInfo((item.Tag as DoorSetInfo).ControllerSN);
                        if (runInfo == null)
                        {
                            if ((DateTime.Now > this.watchingStartTime.AddSeconds(3.0)) && (item.ImageIndex != 3))
                            {
                                item.ImageIndex = 3;
                            }
                        }
                        else if (DateTime.Now > runInfo.refreshTime.AddSeconds((double) WatchingService.unconnect_timeout_sec))
                        {
                            if ((this.watching.lastGetInfoDateTime.AddMilliseconds((double) WatchingService.Watching_Cycle_ms) > DateTime.Now) && (item.ImageIndex != 3))
                            {
                                item.ImageIndex = 3;
                            }
                        }
                        else
                        {
                            int imageIndex = item.ImageIndex;
                            item.ImageIndex = runInfo.GetDoorImageIndex((item.Tag as DoorSetInfo).DoorNO);
                            if ((item.ImageIndex > 2) && (imageIndex != item.ImageIndex))
                            {
                                this.btnWarnExisted.Visible = true;
                                this.btnWarnExisted.BackColor = Color.Red;
                                this.timerWarn.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void UploadCommPassword(string pwd)
        {
            using (icController controller = new icController())
            {
                this.bStopComm = false;
                ArrayList list = new ArrayList();
                byte[] data = new byte[0x480];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }
                string str = "";
                if (!string.IsNullOrEmpty(pwd))
                {
                    str = pwd.Substring(0, Math.Min(0x10, pwd.Length));
                }
                char[] chArray = str.PadRight(0x10, '\0').ToCharArray();
                int index = 0x10;
                for (int j = 0; (j < 0x10) && (j < chArray.Length); j++)
                {
                    data[index] = (byte) (chArray[j] & '\x00ff');
                    data[0x400 + (index >> 3)] = (byte) (data[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                    index++;
                }
                foreach (ListViewItem item in this.lstDoors.SelectedItems)
                {
                    if (this.bStopComm)
                    {
                        return;
                    }
                    controller.GetInfoFromDBByDoorName(item.Text);
                    if (list.IndexOf(controller.ControllerSN) < 0)
                    {
                        list.Add(controller.ControllerSN);
                        if (string.IsNullOrEmpty(wgTools.CommPStr))
                        {
                            controller.UpdateConfigureCPUSuperIP(data, "");
                        }
                        else
                        {
                            controller.UpdateConfigureCPUSuperIP(data, WGPacket.Dpt(wgTools.CommPStr));
                        }
                        wgAppConfig.wgLog(".setComm_" + controller.ControllerSN.ToString());
                    }
                }
            }
        }

        private int uploadPrivilegeNow(int Option)
        {
            int num = -1;
            try
            {
                this.controlConfigure4uploadPrivilege.Clear();
                this.controlTaskList4uploadPrivilege.Clear();
                this.controlHolidayList4uploadPrivilege.Clear();
                this.pr4uploadPrivilege.AllowUpload();
                string doorName = this.arrSelectedDoors[this.dealtDoorIndex].ToString();
                this.control4uploadPrivilege.GetInfoFromDBByDoorName(doorName);
                string compactInfo = "";
                string desc = "";
                string productInfoIP = null;
                int num2 = 3;
                int millisecondsTimeout = 300;
                for (int i = 0; i < num2; i++)
                {
                    productInfoIP = this.control4uploadPrivilege.GetProductInfoIP(ref compactInfo, ref desc);
                    if (!string.IsNullOrEmpty(productInfoIP))
                    {
                        break;
                    }
                    Thread.Sleep(millisecondsTimeout);
                }
                if (!string.IsNullOrEmpty(productInfoIP))
                {
                    if (string.IsNullOrEmpty(this.strAllProductsDriversInfo))
                    {
                        string str5;
                        wgAppConfig.getSystemParamValue(0x30, out str5, out str5, out this.strAllProductsDriversInfo);
                    }
                    if (!string.IsNullOrEmpty(this.strAllProductsDriversInfo))
                    {
                        if (this.strAllProductsDriversInfo.IndexOf(compactInfo) < 0)
                        {
                            if (this.strAllProductsDriversInfo.IndexOf("SN") < 0)
                            {
                                this.strAllProductsDriversInfo = this.strAllProductsDriversInfo + "\r\n";
                            }
                            this.strAllProductsDriversInfo = this.strAllProductsDriversInfo + compactInfo;
                            wgAppConfig.setSystemParamValue(0x30, "ConInfo", "", this.strAllProductsDriversInfo);
                        }
                    }
                    else
                    {
                        this.strAllProductsDriversInfo = compactInfo;
                        wgAppConfig.setSystemParamValue(0x30, "ConInfo", "", this.strAllProductsDriversInfo);
                    }
                }
                else
                {
                    wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.GetProductInfoIP Failed num =" + num.ToString(), new object[0]);
                    return -13;
                }
                if ((Option & 1) > 0)
                {
                    icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(this.control4uploadPrivilege.ControllerID, ref this.controlConfigure4uploadPrivilege, ref this.controlTaskList4uploadPrivilege, ref this.controlHolidayList4uploadPrivilege);
                    num = this.control4uploadPrivilege.UpdateConfigureIP(this.controlConfigure4uploadPrivilege);
                    if (num <= 0)
                    {
                        wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " updateConfigureIP Failed num =" + num.ToString(), new object[0]);
                        return -13;
                    }
                    if ((this.controlConfigure4uploadPrivilege.controlTaskList_enabled > 0) && ((num = this.control4uploadPrivilege.UpdateControlTaskListIP(this.controlTaskList4uploadPrivilege)) <= 0))
                    {
                        wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " updateControlTaskListIP Failed num =" + num.ToString(), new object[0]);
                        return -13;
                    }
                    if (wgAppConfig.getParamValBoolByNO(0x79))
                    {
                        icControllerTimeSegList controlTimeSegList = new icControllerTimeSegList();
                        if (wgAppConfig.getParamValBoolByNO(0x79))
                        {
                            controlTimeSegList.fillByDB();
                        }
                        num = this.control4uploadPrivilege.UpdateControlTimeSegListIP(controlTimeSegList);
                        if (num <= 0)
                        {
                            wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " updateControlTimeSegListIP Failed num =" + num.ToString(), new object[0]);
                            return -13;
                        }
                        num = this.control4uploadPrivilege.UpdateHolidayListIP(this.controlHolidayList4uploadPrivilege.ToByte());
                        if (num <= 0)
                        {
                            wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " UpdateHolidayListIP Failed num =" + num.ToString(), new object[0]);
                            return -13;
                        }
                    }
                }
                if ((Option & 2) > 0)
                {
                    int controllerID = this.pr4uploadPrivilege.getControllerIDByDoorName(doorName);
                    if (controllerID > 0)
                    {
                        num = this.pr4uploadPrivilege.getPrivilegeByID(controllerID);
                        if (num < 0)
                        {
                            wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.getPrivilegeByID Failed num =" + num.ToString(), new object[0]);
                            return num;
                        }
                        num = this.pr4uploadPrivilege.upload(this.control4uploadPrivilege.ControllerSN, this.control4uploadPrivilege.IP, this.control4uploadPrivilege.PORT, doorName);
                        if (num < 0)
                        {
                            switch (num)
                            {
                                case wgTools.ErrorCode.ERR_DB_IS_FULL:
                                case wgTools.ErrorCode.ERR_FAIL:
                                    break;
                                case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                                    num = wgTools.ErrorCode.ERR_UNKNOWN - (pr4uploadPrivilege.templDoubled.uid + (pr4uploadPrivilege.templDoubled.finger << 28));
                                    break;
                                default:
                                    num = -1;
                                    break;
                            }
                            wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.upload Failed num =" + num.ToString(), new object[0]);
                            return num;
                        }
                        string format = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
                        wgAppConfig.runUpdateSql(string.Format(format, new object[] { wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), this.pr4uploadPrivilege.ConsumersTotal, this.pr4uploadPrivilege.PrivilegTotal, this.pr4uploadPrivilege.ValidPrivilege, controllerID }));
                    }
                }
                if (((Option & 1) > 0) && (this.controlTaskList4uploadPrivilege.taskCount > 0))
                {
                    num = this.control4uploadPrivilege.RenewControlTaskListIP();
                    if (num < 0)
                    {
                        wgTools.WgDebugWrite(this.control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.renewControlTaskListIP Failed num =" + num.ToString(), new object[0]);
                    }
                }
            }
            catch (Exception exception)
            {
                num = -1;
                wgAppConfig.wgLog(exception.ToString());
            }
            return num;
        }

        private void uploadPrivilegeToController(int result)
        {
            this.control4uploadPrivilege.GetInfoFromDBByDoorName(this.arrSelectedDoors[this.dealtDoorIndex].ToString());
            this.arrDealtController.Add(this.control4uploadPrivilege.ControllerSN, result);
            int dealtDoorIndex = this.dealtDoorIndex;
            while (dealtDoorIndex < this.arrSelectedDoors.Count)
            {
                this.control4uploadPrivilege.GetInfoFromDBByDoorName(this.arrSelectedDoors[dealtDoorIndex].ToString());
                if (!this.arrDealtController.ContainsKey(this.control4uploadPrivilege.ControllerSN))
                {
                    break;
                }
                if (this.arrDealtController[this.control4uploadPrivilege.ControllerSN] >= 0)
                {
                    InfoRow newInfo = new InfoRow();
                    newInfo.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), this.control4uploadPrivilege.ControllerSN);
                    if (dealtDoorIndex == this.dealtDoorIndex)
                    {
                        if ((this.CommOperateOption & 3) == 3)
                        {
                            newInfo.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadAllOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]);
                            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), CommonStr.strUploadAllOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]));
                        }
                        else if ((this.CommOperateOption & 1) > 0)
                        {
                            newInfo.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadBasicConfigureOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]);
                            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), CommonStr.strUploadBasicConfigureOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]));
                        }
                        else if ((this.CommOperateOption & 2) > 0)
                        {
                            newInfo.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadPrivilegesOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]);
                            wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), CommonStr.strUploadPrivilegesOK, this.arrDealtController[this.control4uploadPrivilege.ControllerSN]));
                        }
                    }
                    else
                    {
                        newInfo.information = string.Format("{0}", CommonStr.strAlreadyUploadPrivileges);
                    }
                    wgRunInfoLog.addEvent(newInfo);
                }
                else
                {
                    foreach (ListViewItem item in this.lstDoors.Items)
                    {
                        if (item.Text == this.arrSelectedDoors[dealtDoorIndex].ToString())
                        {
                            if (this.arrDealtController[this.control4uploadPrivilege.ControllerSN] == wgGlobal.ERR_PRIVILEGES_OVER200K)
                            {
                                InfoRow row2 = new InfoRow();
                                row2.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[dealtDoorIndex].ToString(), this.control4uploadPrivilege.ControllerSN);
                                row2.information = string.Format("{0}--[{1:d}]", wgTools.gADCT ? CommonStr.strUploadFail_200K : CommonStr.strUploadFail_40K, item.Text);
                                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[dealtDoorIndex].ToString(), wgTools.gADCT ? CommonStr.strUploadFail_200K : CommonStr.strUploadFail_40K));
                                wgRunInfoLog.addEvent(row2);
                            }
                            else
                            {
                                wgRunInfoLog.addEventNotConnect(this.control4uploadPrivilege.ControllerSN,
                                    this.control4uploadPrivilege.IP, item,
                                    arrDealtController[this.control4uploadPrivilege.ControllerSN]);
                                string info;
                                switch (arrDealtController[this.control4uploadPrivilege.ControllerSN])
                                {
                                    case wgTools.ErrorCode.ERR_DB_IS_FULL:
                                    case wgTools.ErrorCode.ERR_FAIL:
                                    case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                                        info = wgGlobal.getErrorString(arrDealtController[this.control4uploadPrivilege.ControllerSN]);
                                        break;
                                    default:
                                        if (arrDealtController[this.control4uploadPrivilege.ControllerSN] <= wgTools.ErrorCode.ERR_UNKNOWN)
                                        {
                                            int err = wgTools.ErrorCode.ERR_UNKNOWN - arrDealtController[this.control4uploadPrivilege.ControllerSN];
                                            int finger = err >> 28;
                                            int uid = err & 0xFFFFFFF;
                                            info = wgGlobal.getErrorString(wgTools.ErrorCode.ERR_FINGER_DOUBLED) + " " +
                                                CommonStr.strUserID + ":" + uid.ToString() + " " +
                                                CommonStr.strVerifFinger + ":" + (finger + 1).ToString();
                                        }
                                        else
                                        {
                                            info = CommonStr.strCommFail;
                                        }
                                        break;
                                }
                                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", item.Text, info));
                            }
                            break;
                        }
                    }
                }
                dealtDoorIndex++;
            }
            if (dealtDoorIndex < this.arrSelectedDoors.Count)
            {
                this.dealtDoorIndex = dealtDoorIndex;
                InfoRow row3 = new InfoRow();
                row3.desc = string.Format("{0}[{1:d}]", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), this.control4uploadPrivilege.ControllerSN);
                row3.information = string.Format("{0}", CommonStr.strUploadStart);
                wgRunInfoLog.addEvent(row3);
                wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}", this.arrSelectedDoors[this.dealtDoorIndex].ToString(), CommonStr.strUploadingPrivileges));
            }
            else
            {
                this.dealtDoorIndex = dealtDoorIndex;
                this.btnRealtimeGetRecords.Enabled = true;
                if ((this.btnRealtimeGetRecords.Text != CommonStr.strRealtimeGetting) && (this.btnServer.Text != CommonStr.strMonitoring))
                {
                    this.btnStopOthers.BackColor = Color.Transparent;
                    this.btnStopMonitor.BackColor = Color.Transparent;
                }
                this.btnGetRecords.Enabled = true;
                this.btnUpload.Enabled = true;
            }
            this.displayNewestLog();
        }

        public void wgCommServiceStart()
        {
            if (this.wgCommService1 == null)
            {
                this.wgCommService1 = new wgCommService();
                this.wgCommService1.EventHandler += new OnCommEventHandler(this.evtCommCallBack);
            }
        }

        private class DoorSetInfo
        {
            public int ConnectState;
            public int ControllerSN;
            public int DoorId;
            public string DoorName = "";
            public int DoorNO;
            public string IP = "";
            public int PORT = 0xea60;
            public int Selected;
            public int ZoneID;

            public DoorSetInfo(int id, string name, int no, int sn, string ip, int port, int state, int zoneid)
            {
                this.DoorId = id;
                this.DoorName = name;
                this.DoorNO = no;
                this.ControllerSN = sn;
                this.IP = ip;
                this.PORT = port;
                this.ConnectState = state;
                this.ZoneID = zoneid;
            }
        }

        private delegate void itmDisplayStatus(ListViewItem itm, int status);

        private enum StepOfRealtimeGetReocrds
        {
            Stop,
            GetRecordFirst,
            GetFinished,
            StartMonitoring,
            WaitGetRecord,
            DelSwipe,
            EndStep
        }

        private delegate void txtInfoHaveNewInfo();
    }
}

