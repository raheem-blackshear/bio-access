namespace WG3000_COMM.Core
{
    using System;

    public class MjControlHolidayTime
    {
        public bool bForceWork;
        public DateTime dtEnd;
        public DateTime dtStart;

        public MjControlHolidayTime()
        {
        }

        public MjControlHolidayTime(byte[] holiday)
        {
            if (holiday.Length == byteLen)
            {
                this.bForceWork = false;
                this.dtStart = wgTools.WgDateToMsDate(holiday, 0);
                this.dtEnd = wgTools.WgDateToMsDate(holiday, 4);
                if ((holiday[0] & forceWorkBitLoc) > 0)
                {
                    this.bForceWork = true;
                    this.dtEnd = wgTools.WgDateToMsDate(holiday, 0);
                    this.dtStart = wgTools.WgDateToMsDate(holiday, 4);
                }
            }
        }

        public byte[] ToBytes()
        {
            byte[] destinationArray = new byte[byteLen];
            if (this.bForceWork)
            {
                Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 2, destinationArray, 0, 2);
                Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 0, destinationArray, 2, 2);
                Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 2, destinationArray, 4, 2);
                Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 0, destinationArray, 6, 2);
                destinationArray[0] = (byte) (destinationArray[0] | ((byte) forceWorkBitLoc));
                return destinationArray;
            }
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 2, destinationArray, 0, 2);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtStart)), 0, destinationArray, 2, 2);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 2, destinationArray, 4, 2);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDate(this.dtEnd)), 0, destinationArray, 6, 2);
            destinationArray[0] = (byte) (destinationArray[0] & ~((byte) forceWorkBitLoc));
            return destinationArray;
        }

        internal static int byteLen
        {
            get
            {
                return 8;
            }
        }

        internal static uint getFlashStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;
            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800B8000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x80211000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x80211000;
                    break;
				case wgMjController.DeviceType.S8000_DC:
					flashAddr = 0x4c4000;
					break;
            }
            return flashAddr;
        }

        internal static int forceWorkBitLoc
        {
            get
            {
                return 1;
            }
        }
    }
}
