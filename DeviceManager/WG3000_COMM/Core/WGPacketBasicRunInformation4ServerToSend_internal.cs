namespace WG3000_COMM.Core
{
    using System;

    internal class WGPacketBasicRunInformation4ServerToSend_internal : WGPacket
    {
        private const int m_len = 0x1c;
        private uint m_swipeIndex;
        private uint m_timeout;

        public WGPacketBasicRunInformation4ServerToSend_internal()
        {
            this.m_swipeIndex = uint.MaxValue;
            this.m_timeout = 15;
        }

        public WGPacketBasicRunInformation4ServerToSend_internal(uint swipeIndex)
        {
            this.m_swipeIndex = uint.MaxValue;
            this.m_timeout = 15;
            this.m_swipeIndex = swipeIndex;
        }

        public byte[] ToBytes()
        {
            byte[] destinationArray = new byte[0x1c];
            destinationArray[0] = base.type;
            destinationArray[1] = base.code;
            destinationArray[2] = 0;
            destinationArray[3] = 0;
            Array.Copy(BitConverter.GetBytes(base._xid), 0, destinationArray, 4, 4);
            Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, destinationArray, 8, 4);
            Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, destinationArray, 12, 4);
            destinationArray[0x10] = base.iCallReturn;
            destinationArray[0x11] = base.driverVer;
            destinationArray[0x12] = (byte) wgTools.gPTC_internal;
            destinationArray[0x13] = base.reserved19;
            Array.Copy(BitConverter.GetBytes(this.m_swipeIndex), 0, destinationArray, 20, 4);
            Array.Copy(BitConverter.GetBytes(this.m_timeout), 0, destinationArray, 0x18, 4);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x1c, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public uint swipeIndexToRead
        {
            get
            {
                return this.m_swipeIndex;
            }
            set
            {
                this.m_swipeIndex = value;
            }
        }
    }
}

