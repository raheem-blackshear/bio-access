namespace WG3000_COMM.Core
{
    using System;
    using System.Globalization;

    internal class WGPacketBasicAdjustTimeToSend : WGPacket
    {
        private DateTime _dt;
        private int _door;
        private const int m_len = 0x1c;

        public WGPacketBasicAdjustTimeToSend(DateTime dt, int door)
        {
            this._dt = dt;
            _door = door;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[0x1c];
            destinationArray[0] = base.type;
            destinationArray[1] = base.code;
            Array.Copy(BitConverter.GetBytes(srcPort), 0, destinationArray, 2, 2);
            Array.Copy(BitConverter.GetBytes(base._xid), 0, destinationArray, 4, 4);
            Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, destinationArray, 8, 4);
            Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, destinationArray, 12, 4);
            destinationArray[0x10] = base.iCallReturn;
            destinationArray[0x11] = base.driverVer;
            destinationArray[0x12] = (byte) wgTools.gPTC_internal;
            destinationArray[0x13] = base.reserved19;
            destinationArray[20] = byte.Parse(this._dt.ToString("yy"), NumberStyles.AllowHexSpecifier);
            destinationArray[0x15] = byte.Parse(this._dt.ToString("MM"), NumberStyles.AllowHexSpecifier);
            destinationArray[0x16] = byte.Parse(this._dt.ToString("dd"), NumberStyles.AllowHexSpecifier);
            destinationArray[0x17] = (byte) this._dt.DayOfWeek;
            destinationArray[0x18] = byte.Parse(this._dt.ToString("HH"), NumberStyles.AllowHexSpecifier);
            destinationArray[0x19] = byte.Parse(this._dt.ToString("mm"), NumberStyles.AllowHexSpecifier);
            destinationArray[0x1a] = byte.Parse(this._dt.ToString("ss"), NumberStyles.AllowHexSpecifier);
            destinationArray[0x1b] = (byte)_door;
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x1c, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }
    }
}

