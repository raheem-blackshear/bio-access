namespace WG3000_COMM.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public class wgMjControllerRunInformation
    {
        private const int DoorCount = 4;

        [CompilerGenerated]
        private uint _CurrentControllerSN;
        [CompilerGenerated]
        private string _driverVersion;
        [CompilerGenerated]
        private DateTime _dtNow;
        [CompilerGenerated]
        private DateTime _refreshTime;
        [CompilerGenerated]
        private byte _weekday;
        private const uint FRamIndex4LastGetRecord = 8;
        private byte m_appError;
        private DateTime m_dtPowerOn;
        private byte m_extendIOStatus;
        private const int m_len = 340;
        private uint[] m_lngFRam = new uint[0x10];
        private byte m_lockStatus;
        private byte m_pbdsStatus;
        private byte m_pbdsStatusHigh;
		
        private uint m_registerCardNumTotal;//S8000
        private uint m_deletedRegisterCardNum;//S8000
        private uint m_registerCardStartAddr;//S8000
		
        private ulong m_ulCardID;
        private uint m_nUserCount;
        private byte m_reserved1;
        private byte[] m_reservedBytes = new byte[0x20];
        private uint m_swipeEndIndex;
        private uint m_swipeStartAddr;
        private uint m_swipeStartIndex;
        private uint m_totalPerson4AntibackShare;
        private byte[] m_warnInfo = new byte[DoorCount];
        private uint m_wgcticks;
        protected internal wgMjControllerSwipeRecord[] newSwipes = new wgMjControllerSwipeRecord[10];
        private byte[] privBytes = new byte[340];
        protected internal const int SWIPE_RECORDS_MAXLEN = 100001;

        private const int MaxUserCount = 3000;

        public wgMjControllerRunInformation()
        {
            this.Reset();
        }

        public void Clear()
        {
            this.Reset();
        }

        public int GetDoorImageIndex(int doorNO)
        {
            int num = 0;
            if ((doorNO > 0) && (doorNO <= DoorCount))
            {
                if (this.IsOpen(doorNO))
                {
                    num = 2;
                }
                else
                {
                    num = 1;
                }
                if (this.appError > 0)
                {
                    return (num + 3);
                }
                if (this.m_warnInfo[doorNO - 1] > 0)
                {
                    num += 3;
                }
            }
            return num;
        }

        public bool IsOpen(int doorNO)
        {
            if ((doorNO <= 0) || (doorNO > DoorCount))
            {
                throw new IndexOutOfRangeException();
            }
            return ((this.m_pbdsStatus & (((int)1) << ((DoorCount + doorNO) - 1))) > 0);
        }

        public uint LngFRam(int index)
        {
            if ((index < 0) || (index >= this.m_lngFRam.Length))
            {
                throw new IndexOutOfRangeException();
            }
            return this.m_lngFRam[index];
        }

        public bool LockRelayActive(int doorNO)
        {
            if ((doorNO <= 0) || (doorNO > DoorCount))
            {
                throw new IndexOutOfRangeException();
            }
            return ((this.m_lockStatus & (((int) 1) << (doorNO - 1))) > 0);
        }

        public bool PushButtonActive(int doorNO)
        {
            if ((doorNO <= 0) || (doorNO > DoorCount))
            {
                throw new IndexOutOfRangeException();
            }
            return ((this.m_pbdsStatus & (((int) 1) << (doorNO - 1))) > 0);
        }

        public uint ReaderValidSwipeGet(int readerNo)
        {
            if ((readerNo < 1) || (readerNo > DoorCount))
            {
                throw new IndexOutOfRangeException();
            }
            return this.m_lngFRam[(readerNo - 1) + 4];
        }

        private void Reset()
        {
            int num;
            this.dtNow = DateTime.Parse("2000-1-1");
            this.weekday = 0;
            this.m_reserved1 = 0;
            this.m_wgcticks = 0;
            this.m_swipeStartIndex = 0;
            this.m_swipeStartAddr = 0;
            this.m_swipeEndIndex = 0;
            this.m_registerCardNumTotal = 0;//S8000
            this.m_deletedRegisterCardNum = 0;//S8000
            this.m_registerCardStartAddr = 0;//S8000
            m_ulCardID = 0;
            m_nUserCount = 0;
            this.m_pbdsStatus = 0;
            this.m_lockStatus = 0;
            this.m_extendIOStatus = 0;
            this.m_appError = 0;
            this.m_warnInfo[0] = 0;
            this.m_warnInfo[1] = 0;
            this.m_warnInfo[2] = 0;
            this.m_warnInfo[3] = 0;
            for (num = 0; num < 0x10; num++)
            {
                this.m_lngFRam[num] = 0;
            }
            for (num = 0; num < 0x20; num++)
            {
                this.m_reservedBytes[num] = 0;
            }
            this.m_pbdsStatusHigh = 0;
            this.driverVersion = "";
            this.m_dtPowerOn = DateTime.Parse("2000-1-1");
            this.refreshTime = DateTime.Parse("2000-1-1");
            this.CurrentControllerSN = uint.MaxValue;
        }

        public void UpdateInfo(byte[] wgpktData, int startIndex, uint ControllerSN)
        {
            this.UpdateInfo_internal(wgpktData, startIndex, ControllerSN);
        }

        internal void UpdateInfo_internal(byte[] wgpktData, int startIndex, uint ControllerSN)
        {
            DateTime time;
            int index = startIndex;
            for (int i = 0; i < 340; i++)
            {
                this.privBytes[i] = wgpktData[startIndex + i];
            }
            string s = string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}", new object[] { wgpktData[index], wgpktData[index + 1], wgpktData[index + 2], wgpktData[index + 4], wgpktData[index + 5], wgpktData[index + 6] });
            this.dtNow = DateTime.Parse("2000-1-1");
            if (DateTime.TryParse(s, out time))
            {
                this.dtNow = time;
            }
            this.weekday = wgpktData[index + 3];
            this.m_reserved1 = wgpktData[index + 7];
            this.m_wgcticks = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x1c);
            this.m_swipeStartIndex = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x20);
            this.m_swipeStartAddr = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x24);
            this.m_swipeEndIndex = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 40);
			
			switch (wgMjController.getDeviceType((int)ControllerSN))
			{
            case wgMjController.DeviceType.A30_AC:
            case wgMjController.DeviceType.A50_AC:
            case wgMjController.DeviceType.F300_AC:
	            m_ulCardID = BitConverter.ToUInt64(wgpktData, (startIndex - 20) + 0x2c);
	            m_nUserCount = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x34);
				break;

            case wgMjController.DeviceType.S8000_DC:
            	this.m_registerCardNumTotal = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x2c);
            	this.m_deletedRegisterCardNum = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x30);
            	this.m_registerCardStartAddr = BitConverter.ToUInt32(wgpktData, (startIndex - 20) + 0x34);
				break;
			}

            this.m_pbdsStatus = wgpktData[(startIndex - 20) + 0x38];
            this.m_lockStatus = wgpktData[(startIndex - 20) + 0x39];
            this.m_extendIOStatus = wgpktData[(startIndex - 20) + 0x3a];
            this.m_appError = wgpktData[(startIndex - 20) + 0x3b];
            this.m_warnInfo[0] = wgpktData[(startIndex - 20) + 60];
            this.m_warnInfo[1] = wgpktData[(startIndex - 20) + 0x3d];
            this.m_warnInfo[2] = wgpktData[(startIndex - 20) + 0x3e];
            this.m_warnInfo[3] = wgpktData[(startIndex - 20) + 0x3f];
            index = (startIndex - 20) + 0x40;
            for (int j = 0; j < 10; j++)
            {
                if (this.newSwipes[j] == null)
                {
                    this.newSwipes[j] = new wgMjControllerSwipeRecord(wgpktData, (uint) (((startIndex - 20) + 0x44) + (j * 20)), ControllerSN, BitConverter.ToUInt32(wgpktData, ((startIndex - 20) + 0x40) + (j * 20)));
                }
                else
                {
                    this.newSwipes[j].Update(wgpktData, (uint) (((startIndex - 20) + 0x44) + (j * 20)), ControllerSN, BitConverter.ToUInt32(wgpktData, ((startIndex - 20) + 0x40) + (j * 20)));
                }
            }
            for (int k = 0; k < 0x10; k++)
            {
                this.m_lngFRam[k] = BitConverter.ToUInt32(wgpktData, ((startIndex - 20) + 0x108) + (4 * k));
            }
            for (int m = 0; m < 0x20; m++)
            {
                this.m_reservedBytes[m] = wgpktData[((startIndex - 20) + 0x148) + m];
            }
            this.m_pbdsStatusHigh = wgpktData[((startIndex - 20) + 0x148) + 0x12];
            this.driverVersion = string.Format("V{0:d}.{1:d}", wgpktData[(startIndex - 20) + 0x11], wgpktData[((startIndex - 20) + 0x148) + 0x13]);
            this.m_dtPowerOn = DateTime.Parse("2000-1-1");
            this.m_dtPowerOn = wgTools.WgDateToMsDate(wgpktData, ((startIndex - 20) + 0x148) + 20);
            this.m_totalPerson4AntibackShare = (uint) ((wgpktData[((startIndex - 20) + 0x148) + 0x19] * 0x100) + wgpktData[((startIndex - 20) + 0x148) + 0x18]);
            this.refreshTime = DateTime.Now;
            this.CurrentControllerSN = ControllerSN;
        }

        public byte WarnInfo(int doorNo)
        {
            if ((doorNo <= 0) || (doorNo > DoorCount))
            {
                throw new IndexOutOfRangeException();
            }
            return this.m_warnInfo[doorNo - 1];
        }

        public byte appError
        {
            get
            {
                return this.m_appError;
            }
        }

        public string BytesDataStr
        {
            get
            {
                return BitConverter.ToString(this.privBytes).Replace("-", "");
            }
        }

        public uint CurrentControllerSN
        {
            [CompilerGenerated]
            get
            {
                return this._CurrentControllerSN;
            }
            [CompilerGenerated]
            private set
            {
                this._CurrentControllerSN = value;
            }
        }
        
        // S8000
        public uint deletedRegisterCardNum
        {
            get
            {
                return this.m_deletedRegisterCardNum;
            }
        }

        public string driverVersion
        {
            [CompilerGenerated]
            get
            {
                return this._driverVersion;
            }
            [CompilerGenerated]
            private set
            {
                this._driverVersion = value;
            }
        }

        public DateTime dtNow
        {
            [CompilerGenerated]
            get
            {
                return this._dtNow;
            }
            [CompilerGenerated]
            private set
            {
                this._dtNow = value;
            }
        }

        public DateTime dtPowerOn
        {
            get
            {
                return this.m_dtPowerOn;
            }
        }

        public byte extendIOStatus
        {
            get
            {
                return this.m_extendIOStatus;
            }
        }

        public bool FireIsActive
        {
            get
            {
                return ((this.m_pbdsStatusHigh & 2) > 0);
            }
        }

        public bool ForceLockIsActive
        {
            get
            {
                return ((this.m_pbdsStatusHigh & 1) > 0);
            }
        }

        public uint lastGetRecordIndex
        {
            get
            {
                if (this.m_lngFRam[8] == 0)
                {
                    return 0;
                }
                if (((this.swipeEndIndex & 0xff000000) > 0) && (((this.swipeEndIndex & 0xff000000) + this.m_lngFRam[8]) > this.swipeEndIndex))
                {
                    return (((this.swipeEndIndex & 0xff000000) - 0x1000000) + this.m_lngFRam[8]);
                }
                return ((this.swipeEndIndex & 0xff000000) + this.m_lngFRam[8]);
            }
        }

        public byte lockStatus
        {
            get
            {
                return this.m_lockStatus;
            }
        }

        public uint getNewRecordsNum()
        {
            int swipeEndIndex = 0;
            int maxRecordCount = (int)wgMjControllerSwipeOperate.getSwipeRecordsMaxLen((int)CurrentControllerSN);
            if (((this.lastGetRecordIndex == 0) || (this.swipeEndIndex >= (((this.lastGetRecordIndex + maxRecordCount) - (0x1000 / wgMjControllerSwipeRecord.SwipeSize)) - 1))) || (this.swipeEndIndex < this.lastGetRecordIndex))
            {
                if (this.swipeEndIndex > 0)
                {
                    if (this.swipeEndIndex >= maxRecordCount)
                    {
                        swipeEndIndex = (maxRecordCount - (0x1000 / wgMjControllerSwipeRecord.SwipeSize)) + ((int)(this.swipeEndIndex % (0x1000 / wgMjControllerSwipeRecord.SwipeSize)));
                    }
                    else
                    {
                        swipeEndIndex = (int) this.swipeEndIndex;
                    }
                }
            }
            else
            {
                swipeEndIndex = (int) (this.swipeEndIndex - this.lastGetRecordIndex);
            }
            if (swipeEndIndex < 0)
            {
                swipeEndIndex = 0;
            }
            return (uint) swipeEndIndex;
        }

        public byte pbdsStatus
        {
            get
            {
                return this.m_pbdsStatus;
            }
        }

        public byte pbdsStatusHigh
        {
            get
            {
                return this.m_pbdsStatusHigh;
            }
        }

        public static int pktlen
        {
            get
            {
                return 360;
            }
        }

        public DateTime refreshTime
        {
            [CompilerGenerated]
            get
            {
                return this._refreshTime;
            }
            [CompilerGenerated]
            private set
            {
                this._refreshTime = value;
            }
        }
		
		// S8000
		public uint registerCardNum
        {
            get
            {
                if (this.m_registerCardNumTotal < this.m_deletedRegisterCardNum)
                {
                    return 0;
                }
                return (this.m_registerCardNumTotal - this.m_deletedRegisterCardNum);
            }
        }

		// S8000
        public uint registerCardNumTotal
        {
            get
            {
                return this.m_registerCardNumTotal;
            }
        }
		
		// S8000
        public uint registerCardStartAddr
        {
            get
            {
                return this.m_registerCardStartAddr;
            }
        }		

        public uint regUserCount
        {
            get { return m_nUserCount; }
        }

        public uint regUserMaxCount
        {
            get { return MaxUserCount; }
        }

        public byte reserved1
        {
            get
            {
                return this.m_reserved1;
            }
        }

        public byte[] reservedBytes
        {
            get
            {
                return this.m_reservedBytes;
            }
        }

        public uint swipeEndIndex
        {
            get
            {
                return this.m_swipeEndIndex;
            }
        }

        public uint swipeStartAddr
        {
            get
            {
                return this.m_swipeStartAddr;
            }
        }

        public uint swipeStartIndex
        {
            get
            {
                return this.m_swipeStartIndex;
            }
        }

        public uint totalPerson4AntibackShare
        {
            get
            {
                return this.m_totalPerson4AntibackShare;
            }
        }

        public byte weekday
        {
            [CompilerGenerated]
            get
            {
                return this._weekday;
            }
            [CompilerGenerated]
            private set
            {
                this._weekday = value;
            }
        }

        public ulong inputCardID
        {
            get
            {
                return this.m_ulCardID;
            }
        }

        public uint wgcticks
        {
            get
            {
                return this.m_wgcticks;
            }
        }
    }
}
