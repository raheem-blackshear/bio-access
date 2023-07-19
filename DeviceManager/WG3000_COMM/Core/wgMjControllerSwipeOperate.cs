namespace WG3000_COMM.Core
{
    using System;
    using System.Data;
    using System.Threading;

    public class wgMjControllerSwipeOperate : IDisposable
    {
        private wgMjController control = new wgMjController();
        private static bool m_bStopGetRecord;
        private int m_ControllerSN;
        private int m_lastRecordFlashIndex;
        private wgUdpComm m_wgudp;
        private const int SSI_FLASH_PAGE_SIZE = 0x100;
        private const int SSI_FLASH_SWIPE_PAGES = 0xc80;
        protected internal const int SWIPE_SIZE = 0x10;
        private const int SWIPE_RECORDS_PER_PAGE = 0x10;

        public wgMjControllerSwipeOperate()
        {
            this.Clear();
        }

        public void Clear()
        {
            m_bStopGetRecord = false;
            this.m_ControllerSN = -1;
            this.m_lastRecordFlashIndex = -1;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_wgudp != null)
                {
                    this.m_wgudp.Close();
                    this.m_wgudp.Dispose();
                }
                if (this.control != null)
                {
                    this.control.Dispose();
                }
            }
        }

        protected internal int GetSwipeIndex(int controllerSN, uint iSwipeLoc)
        {
            return (int)((iSwipeLoc - getSwipeStartAddr(controllerSN)) / 0x10);
        }

        protected internal uint GetSwipeLoc(int controllerSN, uint iSwipeIndex)
        {
            uint num = iSwipeIndex - (iSwipeIndex % getSwipeRecordsMaxLen(controllerSN));
            if (iSwipeIndex >= num)
                return ((getSwipeStartAddr(controllerSN) + (0x100 * ((iSwipeIndex - num) / 0x10))) + (((iSwipeIndex - num) % 0x10) * 0x10));

            return ((getSwipeEndAddr(controllerSN) - (0x100 * ((num - iSwipeIndex) / 0x10))) + (((num - iSwipeIndex) % 0x10) * 0x10));
        }

        public int GetSwipeRecords(int ControllerSN, string IP, int Port, ref DataTable dtSwipe)
        {
            wgTools.WriteLine("getSwipeRecords Start");
            this.control.ControllerSN = ControllerSN;
            this.control.IP = IP;
            this.control.PORT = Port;
            this.m_ControllerSN = ControllerSN;
            byte[] recvInfo = null;
            if (this.control.GetMjControllerRunInformationIP(ref recvInfo) < 0)
            {
                return -13;
            }
            wgMjControllerRunInformation information = new wgMjControllerRunInformation();
            if (recvInfo == null)
            {
                return -1;
            }
            if (ControllerSN != -1)
            {
                information.UpdateInfo_internal(recvInfo, 20, (uint) ControllerSN);
            }
            else
            {
                uint controllerSN = (uint) (((recvInfo[8] + (recvInfo[9] << 8)) + (recvInfo[10] << 0x10)) + (recvInfo[11] << 0x18));
                information.UpdateInfo_internal(recvInfo, 20, controllerSN);
            }
            if (information.getNewRecordsNum() == 0)
            {
                this.m_lastRecordFlashIndex = (int) information.lastGetRecordIndex;
                return 0;
            }
            if (this.m_wgudp == null)
            {
                this.m_wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            WGPacketSSI_FLASH_QUERY_internal _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint)ControllerSN, getSwipeStartAddr(m_ControllerSN), getSwipeStartAddr(m_ControllerSN) + 0x3ff);
            byte[] cmd = _internal.ToBytes(this.m_wgudp.udpPort);
            if (cmd == null)
            {
                return -12;
            }
            recv = null;
            if (this.m_wgudp.udp_get(cmd, 1000, _internal.xid, IP, Port, ref recv) < 0)
            {
                return -13;
            }
            wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
            int num3 = 0x1000;
            int num4 = 0;
            uint num6 = 0;
            uint lastGetRecordIndex = information.lastGetRecordIndex;
            lastGetRecordIndex = information.swipeEndIndex - information.getNewRecordsNum();
            if (lastGetRecordIndex > 0)
            {
                uint swipeLoc = this.GetSwipeLoc(ControllerSN, lastGetRecordIndex);
                _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint) ControllerSN, (uint) (swipeLoc - (swipeLoc % 0x400)), (uint) (((swipeLoc - (swipeLoc % 0x400)) + 0x400) - 1));
                cmd = _internal.ToBytes(this.m_wgudp.udpPort);
                if (cmd == null)
                {
                    return -12;
                }
                recv = null;
                if (this.m_wgudp.udp_get(cmd, 1000, _internal.xid, IP, Port, ref recv) < 0)
                {
                    return -13;
                }
            }
            uint iStartFlashAddr = _internal.iStartFlashAddr;
            bool flag = false;
            uint num9 = 0;
            wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 0x400));
            uint num10 = lastGetRecordIndex - (lastGetRecordIndex % getSwipeRecordsMaxLen(ControllerSN));
        Label_0235:
            if (!m_bStopGetRecord && (recv != null))
            {
                WGPacketSSI_FLASH_internal _internal2 = new WGPacketSSI_FLASH_internal(recv);
                wgMjControllerSwipeRecord record = null;
                uint loc = 0;
                loc = (uint) (this.GetSwipeIndex(ControllerSN, _internal2.iStartFlashAddr) + num10);
                for (uint i = 0; i < 0x400; i += 0x10)
                {
                    record = new wgMjControllerSwipeRecord(_internal2.ucData, i, _internal2.iDevSnFrom, loc);
                    if ((record.UserID == uint.MaxValue) || (((record.bytRecOption_Internal == 0) || (record.bytRecOption_Internal == 0xff)) && (record.UserID == 0)))
                    {
                        if (information.swipeEndIndex <= loc)
                        {
                            break;
                        }
                        num9++;
                        loc++;
                    }
                    else
                    {
                        if ((num4 > 0) || (record.indexInDataFlash_Internal >= lastGetRecordIndex))
                        {
                            DataRow row = dtSwipe.NewRow();
                            row["f_Index"] = num4 + 1;
                            row["f_ConsumerNO"] = record.UserID;
                            row["f_DoorNO"] = record.DoorNo;
                            row["f_InOut"] = record.IsEnterIn ? 1 : 0;
                            row["f_ReaderNO"] = record.ReaderNo;
                            row["f_ReadDate"] = record.ReadDate;
                            row["f_EventCategory"] = record.eventCategory;
                            row["f_ReasonNo"] = record.ReasonNo;
                            row["f_ControllerSN"] = record.ControllerSN;
                            if (dtSwipe.Columns.Contains("f_RecordAll"))
                            {
                                row["f_RecordAll"] = record.ToStringRaw();
                            }
                            dtSwipe.Rows.Add(row);
                            num4++;
                            num6 = record.indexInDataFlash_Internal;
                        }
                        loc++;
                    }
                }
                dtSwipe.AcceptChanges();
                if (information.swipeEndIndex <= loc)
                {
                    flag = true;
                }
                else if (!flag)
                {
                    _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint) ControllerSN, _internal.iStartFlashAddr + 0x400, ((_internal.iStartFlashAddr + 0x400) + 0x400) - 1);
                    if (_internal.iStartFlashAddr > getSwipeEndAddr(ControllerSN))
                    {
                        _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint)ControllerSN, getSwipeStartAddr(m_ControllerSN), getSwipeStartAddr(m_ControllerSN) + 0x3ff);
                        num10 += getSwipeRecordsMaxLen(ControllerSN);
                    }
                    if (_internal.iStartFlashAddr != iStartFlashAddr)
                    {
                        _internal.GetNewXid();
                        cmd = _internal.ToBytes(this.m_wgudp.udpPort);
                        if (((cmd != null) && (this.m_wgudp.udp_get(cmd, 1000, _internal.xid, IP, Port, ref recv) >= 0)) && (--num3 > 0))
                        {
                            goto Label_0235;
                        }
                    }
                }
            }
            if (m_bStopGetRecord)
            {
                return -1;
            }
            wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", _internal.iStartFlashAddr / 0x400));
            wgTools.WriteLine("Syn Data Info");
            if (num4 > 0)
            {
                if (this.control.GetMjControllerRunInformationIP(ref recvInfo) < 0)
                {
                    return -13;
                }
                if (recvInfo != null)
                {
                    if (ControllerSN != -1)
                    {
                        information.UpdateInfo_internal(recvInfo, 20, (uint) ControllerSN);
                    }
                    else
                    {
                        uint num13 = (uint) (((recvInfo[8] + (recvInfo[9] << 8)) + (recvInfo[10] << 0x10)) + (recvInfo[11] << 0x18));
                        information.UpdateInfo_internal(recvInfo, 20, num13);
                    }
                }
                if ((num6 % getSwipeRecordsMaxLen(ControllerSN)) >= (information.swipeEndIndex % getSwipeRecordsMaxLen(ControllerSN)))
                {
                    if (information.swipeEndIndex > getSwipeRecordsMaxLen(ControllerSN))
                    {
                        lastGetRecordIndex = (((uint)(information.swipeEndIndex - (information.swipeEndIndex % getSwipeRecordsMaxLen(ControllerSN)))) - getSwipeRecordsMaxLen(ControllerSN)) + (num6 % getSwipeRecordsMaxLen(ControllerSN));
                    }
                    else
                    {
                        lastGetRecordIndex = 0;
                    }
                }
                else
                {
                    lastGetRecordIndex = ((uint)(information.swipeEndIndex - (information.swipeEndIndex % getSwipeRecordsMaxLen(ControllerSN)))) + (num6 % getSwipeRecordsMaxLen(ControllerSN));
                }
                this.control.UpdateLastGetRecordLocationIP((uint) (lastGetRecordIndex + 1));
                this.m_lastRecordFlashIndex = (int)(lastGetRecordIndex + 1);
            }
            return num4;
        }

        public static void StopGetRecord()
        {
            m_bStopGetRecord = true;
        }

        public static uint getSwipeRecordsMaxLen(int controllerSN)
        {
            uint maxCount = 0;
            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
					maxCount = 100001/*80001*/;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    maxCount = 100001;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    maxCount = 100001;//TODOJ_140418_12
                    break;
                case wgMjController.DeviceType.S8000_DC:
                    maxCount = 0x32000;
                    break;
            }
            return maxCount;
        }

        protected internal uint getSwipeStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x0/*0x802A2000*/;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x804EF000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x804EF000;
                    break;
                case wgMjController.DeviceType.S8000_DC:
                    flashAddr = 0x4c9000;
                    break;
            }

            return flashAddr;
        }

        protected internal uint getSwipeEndAddr(int controllerSN)
        {
            return getSwipeStartAddr(controllerSN) + getSwipeRecordsMaxLen(controllerSN) * SWIPE_SIZE - 1;
        }

        protected internal static bool bStopGetRecord
        {
            get
            {
                return m_bStopGetRecord;
            }
        }

        public int ControllerSN
        {
            get
            {
                return this.m_ControllerSN;
            }
            protected internal set
            {
                this.m_ControllerSN = value;
            }
        }

        public int lastRecordFlashIndex
        {
            get
            {
                return this.m_lastRecordFlashIndex;
            }
            protected internal set
            {
                this.m_lastRecordFlashIndex = value;
            }
        }

        protected internal wgUdpComm wgudp
        {
            get
            {
                return this.m_wgudp;
            }
            set
            {
                this.m_wgudp = value;
            }
        }
    }
}

