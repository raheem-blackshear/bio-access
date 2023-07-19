namespace WG3000_COMM.DataOper
{
    using System;
    using System.Data;
    using WG3000_COMM.Core;

    internal class icPrivilegeShare
    {
        public static bool bNeedRefresh = true;
        private static DataView m_dvPrivilegeCount = null;
        private static int m_privilegeTotal = -1;

        public static void setNeedRefresh()
        {
            m_privilegeTotal = -1;
            m_dvPrivilegeCount = null;
            wgAppConfig.setSystemParamValue(0x34, null, null, null);
            wgAppConfig.setSystemParamValue(0x33, null, null, null);
            bNeedRefresh = true;
        }

        public static DataView dvPrivilegeCount
        {
            get
            {
                return m_dvPrivilegeCount;
            }
        }

        public static int privilegeTotal
        {
            get
            {
                return m_privilegeTotal;
            }
        }
    }
}

