namespace WG3000_COMM.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public class MjControlTimeSeg
    {
        private byte m_ControlByHoliday = 1;
        private ushort m_hmsEnd1;
        private ushort m_hmsEnd2;
        private ushort m_hmsEnd3;
        private ushort m_hmsStart1;
        private ushort m_hmsStart2;
        private ushort m_hmsStart3;
        private int m_LimittedAccess1 = 0;
        private int m_LimittedAccess2 = 0;
        private int m_LimittedAccess3 = 0;
        private int m_LimittedMode = 0;
        private byte m_nextSeg = 0;
        private byte[] m_reserved1 = new byte[2];
        private byte[] m_reserved2 = new byte[10];
        private int m_TimeSegLimittedAccess = 0;
        private int m_TimeSegMonthLimittedAccess = 0;
        private byte m_weekdayControl = 0xff;
        private ushort m_ymdEnd;
        private ushort m_ymdStart;

        private DateTime GetHms(ushort curhms)
        {
            byte[] destinationArray = new byte[4];
            Array.Copy(BitConverter.GetBytes(0x1421), 0, destinationArray, 0, 2);
            Array.Copy(BitConverter.GetBytes(curhms), 0, destinationArray, 2, 2);
            return wgTools.WgDateToMsDate(destinationArray, 0);
        }

        private ushort SetHms(DateTime tm)
        {
            return (ushort) ((wgTools.MsDateToWgDate(tm) >> 0x10) & 0xffe0);
        }

        public byte[] ToBytes()
        {
            byte[] destinationArray = new byte[byteLen];
            for (int i = 0; i < byteLen; i++)
            {
                destinationArray[i] = 0xff;
            }
            destinationArray[0] = this.m_weekdayControl;
            destinationArray[1] = this.m_nextSeg;
            destinationArray[2] = (byte) (this.m_LimittedMode & 0xff);
            destinationArray[3] = (byte) (this.m_TimeSegLimittedAccess & 0xff);
            ushort num2 = (ushort) (this.m_hmsStart1 & 0xffe0);
            Array.Copy(BitConverter.GetBytes(num2), 0, destinationArray, 4, 2);
            num2 = (ushort) (this.m_hmsEnd1 & 0xffe0);
            Array.Copy(BitConverter.GetBytes(num2), 0, destinationArray, 6, 2);
            num2 = (ushort) (this.m_hmsStart2 & 0xffe0);
            Array.Copy(BitConverter.GetBytes(num2), 0, destinationArray, 8, 2);
            num2 = (ushort) (this.m_hmsEnd2 & 0xffe0);
            Array.Copy(BitConverter.GetBytes(num2), 0, destinationArray, 10, 2);
            num2 = (ushort) (this.m_hmsStart3 & 0xffe0);
            Array.Copy(BitConverter.GetBytes(num2), 0, destinationArray, 12, 2);
            num2 = (ushort) (this.m_hmsEnd3 & 0xffe0);
            Array.Copy(BitConverter.GetBytes(num2), 0, destinationArray, 14, 2);
            Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, destinationArray, 0x10, 2);
            Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, destinationArray, 0x12, 2);
            destinationArray[20] = (byte) (this.m_LimittedAccess1 & 0xff);
            destinationArray[0x15] = (byte) (this.m_LimittedAccess2 & 0xff);
            destinationArray[0x16] = (byte) (this.m_LimittedAccess3 & 0xff);
            destinationArray[0x17] = this.m_ControlByHoliday;
            destinationArray[0x18] = (byte) (this.m_TimeSegMonthLimittedAccess & 0xff);
            return destinationArray;
        }

        internal static int byteLen
        {
            get
            {
                return 0x20;
            }
        }

        public byte ControlByHoliday
        {
            get
            {
                return this.m_ControlByHoliday;
            }
            set
            {
                this.m_ControlByHoliday = value;
            }
        }

        internal static uint getFlashStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800B5000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x8020E000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x8020E000;
                    break;
                case wgMjController.DeviceType.S8000_DC:
					flashAddr = 0x4bd000;
					break;
            }

            return flashAddr;
        }

        public DateTime hmsEnd1
        {
            get
            {
                return this.GetHms(this.m_hmsEnd1);
            }
            set
            {
                this.m_hmsEnd1 = this.SetHms(value);
            }
        }

        public DateTime hmsEnd2
        {
            get
            {
                return this.GetHms(this.m_hmsEnd2);
            }
            set
            {
                this.m_hmsEnd2 = this.SetHms(value);
            }
        }

        public DateTime hmsEnd3
        {
            get
            {
                return this.GetHms(this.m_hmsEnd3);
            }
            set
            {
                this.m_hmsEnd3 = this.SetHms(value);
            }
        }

        public DateTime hmsStart1
        {
            get
            {
                return this.GetHms(this.m_hmsStart1);
            }
            set
            {
                this.m_hmsStart1 = this.SetHms(value);
            }
        }

        public DateTime hmsStart2
        {
            get
            {
                return this.GetHms(this.m_hmsStart2);
            }
            set
            {
                this.m_hmsStart2 = this.SetHms(value);
            }
        }

        public DateTime hmsStart3
        {
            get
            {
                return this.GetHms(this.m_hmsStart3);
            }
            set
            {
                this.m_hmsStart3 = this.SetHms(value);
            }
        }

        public int LimittedAccess1
        {
            get
            {
                return this.m_LimittedAccess1;
            }
            set
            {
                if ((value >= 0) & (value <= 0x1f))
                {
                    this.m_LimittedAccess1 = value;
                }
            }
        }

        public int LimittedAccess2
        {
            get
            {
                return this.m_LimittedAccess2;
            }
            set
            {
                if ((value >= 0) & (value <= 0x1f))
                {
                    this.m_LimittedAccess2 = value;
                }
            }
        }

        public int LimittedAccess3
        {
            get
            {
                return this.m_LimittedAccess3;
            }
            set
            {
                if ((value >= 0) & (value <= 0x1f))
                {
                    this.m_LimittedAccess3 = value;
                }
            }
        }

        public int LimittedMode
        {
            get
            {
                return this.m_LimittedMode;
            }
            set
            {
                if ((value == 0) || (value == 1))
                {
                    this.m_LimittedMode = value;
                }
            }
        }

        public int MonthLimittedAccess
        {
            get
            {
                return this.m_TimeSegMonthLimittedAccess;
            }
            set
            {
                if ((value >= 0) & (value <= 0xfe))
                {
                    this.m_TimeSegMonthLimittedAccess = value;
                }
            }
        }

        public byte nextSeg
        {
            get
            {
                return this.m_nextSeg;
            }
            set
            {
                this.m_nextSeg = value;
            }
        }

        public byte SegIndex { get; set; }

        public int TotalLimittedAccess
        {
            get
            {
                return this.m_TimeSegLimittedAccess;
            }
            set
            {
                if ((value >= 0) & (value <= 0xfe))
                {
                    this.m_TimeSegLimittedAccess = value;
                }
            }
        }

        public byte weekdayControl
        {
            get
            {
                return this.m_weekdayControl;
            }
            set
            {
                this.m_weekdayControl = value;
            }
        }

        public DateTime ymdEnd
        {
            get
            {
                byte[] destinationArray = new byte[4];
                Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, destinationArray, 0, 2);
                destinationArray[2] = 0;
                destinationArray[3] = 0;
                return wgTools.WgDateToMsDate(destinationArray, 0);
            }
            set
            {
                this.m_ymdEnd = (ushort) (wgTools.MsDateToWgDate(value) & 0xffff);
            }
        }

        public DateTime ymdStart
        {
            get
            {
                byte[] destinationArray = new byte[4];
                Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, destinationArray, 0, 2);
                destinationArray[2] = 0;
                destinationArray[3] = 0;
                return wgTools.WgDateToMsDate(destinationArray, 0);
            }
            set
            {
                this.m_ymdStart = (ushort) (wgTools.MsDateToWgDate(value) & 0xffff);
            }
        }
    }
}

