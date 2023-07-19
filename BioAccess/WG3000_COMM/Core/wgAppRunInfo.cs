namespace WG3000_COMM.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class wgAppRunInfo
    {
        private static  event appRunInfoCommStatusHandler appRunInfoCommStatus;

        private static  event appRunInfoLoadNumHandler appRunInfoLoadNums;

        private static  event appRunInfoMonitorHandler appRunInfoMonitors;

        public static  event appRunInfoCommStatusHandler evAppRunInfoCommStatus
        {
            add
            {
                appRunInfoCommStatus += value;
            }
            remove
            {
                appRunInfoCommStatus -= value;
            }
        }

        public static  event appRunInfoLoadNumHandler evAppRunInfoLoadNum
        {
            add
            {
                appRunInfoLoadNums += value;
            }
            remove
            {
                appRunInfoLoadNums -= value;
            }
        }

        public static  event appRunInfoMonitorHandler evAppRunInfoMonitor
        {
            add
            {
                appRunInfoMonitors += value;
            }
            remove
            {
                appRunInfoMonitors -= value;
            }
        }

        public static void ClearAllDisplayedInfo()
        {
            raiseAppRunInfoLoadNums("");
            raiseAppRunInfoCommStatus("");
        }

        public static void raiseAppRunInfoCommStatus(string info)
        {
            if (appRunInfoCommStatus != null)
            {
                appRunInfoCommStatus(info);
            }
        }

        public static void raiseAppRunInfoLoadNums(string info)
        {
            if (appRunInfoLoadNums != null)
            {
                appRunInfoLoadNums(info);
            }
        }

        public static void raiseAppRunInfoMonitors(string info)
        {
            if (appRunInfoMonitors != null)
            {
                appRunInfoMonitors(info);
            }
        }

        public delegate void appRunInfoCommStatusHandler(string strCommStatus);

        public delegate void appRunInfoLoadNumHandler(string strNum);

        public delegate void appRunInfoMonitorHandler(string strNum);
    }
}

