namespace WG3000_COMM.DataOper
{
    using System;
    using WG3000_COMM.ResStrings;

    public class icDesc
    {
        private static string[] allPinDesc = new string[] { 
            "1", "读头的4号指示灯", "2", "读头的3号指示灯", "5", "读头的2号指示灯", "6", "读头的1号指示灯", "10", "1号读头的D0", "11", "1号读头的D1", "12", "2号读头的D0", "13", "2号读头的D1", 
            "18", "DF的写保护控制", "19", "DF的复位控制", "22", "2号门磁", "23", "刷卡或按键信号指示灯", "24", "故障指示灯", "25", "运行指示灯", "26", "强制锁门(紧急双闭)", "27", "作为3.3电源使能控制 2010-8-24 13:45:23 ", 
            "28", "DF的sck", "29", "DF的片选", "30", "DF的miso", "31", "DF的mosi", "34", "SD2405API的scl", "35", "SD2405API的sda", "47", "3号开门按钮", "59", "网络连接指示灯绿色(由网络控制)", 
            "60", "网络通信指示灯(由网络控制)", "61", "4号开门按钮", "66", "1号开门按钮", "67", "2号开门按钮", "70", "扩展板的scl", "71", "扩展板的sda", "72", "3号锁", "73", "4号锁", 
            "74", "3号门磁", "75", "4号门磁", "77", "JTAG 5", "78", "JTAG 1", "79", "JTAG 3", "80", "JTAG 4", "89", "JTAG 只接了上拉3.3V", "90", "2号锁", 
            "91", "1号门磁", "92", "1号锁", "95", "3号读头的D0", "96", "3号读头的D1", "99", "4号读头的D0", "100", "4号读头的D1"
         };
        public const int ERRBIT_DATAFLASH = 1;
        public const int ERRBIT_PARAM = 0;
        public const int ERRBIT_REALCLOCK = 2;
        public const int WARNBIT_ALARM = 6;
        public const int WARNBIT_DOORINVALIDOPEN = 2;
        public const int WARNBIT_DOORINVALIDREAD = 4;
        public const int WARNBIT_DOOROPENTOOLONG = 1;
        public const int WARNBIT_FIRELINK = 5;
        public const int WARNBIT_FORCE = 0;
        public const int WARNBIT_FORCE_WITHCARD = 7;
        public const int WARNBIT_FORCECLOSE = 3;

        public static string doorControlDesc(int doorControl)
        {
            switch (doorControl)
            {
                case 1:
                    return CommonStr.strDoorControl_NO;

                case 2:
                    return CommonStr.strDoorControl_NC;

                case 3:
                    return CommonStr.strDoorControl_OnLine;
            }
            return doorControl.ToString();
        }

        public static string ErrorDetail(int errNo)
        {
            string str = "";
            if ((errNo & 1) > 0)
            {
                str = str + CommonStr.strErrParam;
            }
            if ((errNo & 2) > 0)
            {
                str = str + " " + CommonStr.strErrDataFlash;
            }
            if ((errNo & 4) > 0)
            {
                str = str + " " + CommonStr.strErrRealClock;
            }
            return str;
        }

        public static string failedPinDesc(int failedPin)
        {
            string str = "";
            int num = -1;
            if (failedPin == 0x68)
            {
                str = "时钟问题";
            }
            else if (failedPin == 0x67)
            {
                num = 100;
                str = "时钟问题, ";
            }
            else if (failedPin > 100)
            {
                num = failedPin - 100;
                str = "时钟问题, ";
            }
            else if (failedPin > 0)
            {
                num = failedPin;
            }
            if (num > 0)
            {
                for (int i = 0; i < allPinDesc.Length; i += 2)
                {
                    if (string.Compare(num.ToString(), allPinDesc[i]) == 0)
                    {
                        str = str + allPinDesc[i + 1];
                    }
                }
            }
            return str;
        }

        public static string WarnDetail(int warnNo)
        {
            string str = "";
            if ((warnNo & 1) > 0)
            {
                str = str + "-" + CommonStr.strWarnThreateCode;
            }
            if ((warnNo & 2) > 0)
            {
                str = str + "-" + CommonStr.strWarnOpenTooLong;
            }
            if ((warnNo & 4) > 0)
            {
                str = str + "-" + CommonStr.strWarnForcedOpen;
            }
            if ((warnNo & 8) > 0)
            {
                str = str + "-" + CommonStr.strWarnForcedLock;
            }
            if ((warnNo & 0x10) > 0)
            {
                str = str + "-" + CommonStr.strWarnInvalidCardSwiping;
            }
            if ((warnNo & 0x20) > 0)
            {
                str = str + "-" + CommonStr.strWarnFireAlarm;
            }
            if ((warnNo & 0x40) > 0)
            {
                str = str + "-" + CommonStr.strWarnARM;
            }
            return str;
        }
    }
}

