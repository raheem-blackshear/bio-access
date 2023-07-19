namespace WG3000_COMM.Core
{
    using System;

    public class MjControlTaskItem
    {
        private ushort m_hms;
        private ushort m_paramLoc;
        private byte m_paramValue;
        private byte m_weekdayControl;
        private ushort m_ymdEnd;
        private ushort m_ymdStart;

        public MjControlTaskItem()
        {
        }

        internal MjControlTaskItem(byte[] bytControlItem)
        {
            if (bytControlItem.Length == byteLen)
            {
                this.m_hms = (ushort) ((bytControlItem[1] << 8) + bytControlItem[0]);
                this.m_ymdStart = (ushort) ((bytControlItem[3] << 8) + bytControlItem[2]);
                this.m_ymdEnd = (ushort) ((bytControlItem[5] << 8) + bytControlItem[4]);
                this.m_weekdayControl = bytControlItem[6];
                this.m_paramValue = bytControlItem[7];
                this.m_paramLoc = (ushort) ((bytControlItem[9] << 8) + bytControlItem[8]);
            }
        }

        public void CopyFrom(MjControlTaskItem mjc)
        {
            byte[] buffer = mjc.ToBytes();
            this.m_hms = (ushort) ((buffer[1] << 8) + buffer[0]);
            this.m_ymdStart = (ushort) ((buffer[3] << 8) + buffer[2]);
            this.m_ymdEnd = (ushort) ((buffer[5] << 8) + buffer[4]);
            this.m_weekdayControl = buffer[6];
            this.m_paramValue = buffer[7];
            this.m_paramLoc = (ushort) ((buffer[9] << 8) + buffer[8]);
        }

        public byte[] ToBytes()
        {
            byte[] destinationArray = new byte[byteLen];
            Array.Copy(BitConverter.GetBytes(this.m_hms), 0, destinationArray, 0, 2);
            Array.Copy(BitConverter.GetBytes(this.m_ymdStart), 0, destinationArray, 2, 2);
            Array.Copy(BitConverter.GetBytes(this.m_ymdEnd), 0, destinationArray, 4, 2);
            Array.Copy(BitConverter.GetBytes((short) this.m_weekdayControl), 0, destinationArray, 6, 1);
            Array.Copy(BitConverter.GetBytes((short) this.m_paramValue), 0, destinationArray, 7, 1);
            Array.Copy(BitConverter.GetBytes(this.m_paramLoc), 0, destinationArray, 8, 2);
            return destinationArray;
        }

        internal static int byteLen
        {
            get
            {
                return 10;
            }
        }

        public static uint getFlashStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800B7000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x80210000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x80210000;
                    break;
				case wgMjController.DeviceType.S8000_DC:
					flashAddr = 0x4bf000;
					break;
            }

            return flashAddr;
        }

        public DateTime hms
        {
            get
            {
                byte[] destinationArray = new byte[4];
                Array.Copy(BitConverter.GetBytes(0x1421), 0, destinationArray, 0, 2);
                Array.Copy(BitConverter.GetBytes(this.m_hms), 0, destinationArray, 2, 2);
                return wgTools.WgDateToMsDate(destinationArray, 0);
            }
            set
            {
                this.m_hms = (ushort) ((wgTools.MsDateToWgDate(value) >> 0x10) & 0xffff);
            }
        }

        public int paramLoc
        {
            get
            {
                return this.m_paramLoc;
            }
            set
            {
                this.m_paramLoc = (ushort) (value & 0xffff);
            }
        }

        public byte paramValue
        {
            get
            {
                return this.m_paramValue;
            }
            set
            {
                this.m_paramValue = value;
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
                Array.Copy(BitConverter.GetBytes(this.m_hms), 0, destinationArray, 2, 2);
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
                Array.Copy(BitConverter.GetBytes(this.m_hms), 0, destinationArray, 2, 2);
                return wgTools.WgDateToMsDate(destinationArray, 0);
            }
            set
            {
                this.m_ymdStart = (ushort) (wgTools.MsDateToWgDate(value) & 0xffff);
            }
        }
    }
}

