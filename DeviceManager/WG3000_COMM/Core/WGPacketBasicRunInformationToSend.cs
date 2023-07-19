﻿namespace WG3000_COMM.Core
{
    using System;

    internal class WGPacketBasicRunInformationToSend : WGPacket
    {
        private const int m_len = 0x18;
        private uint m_swipeIndex;

        public WGPacketBasicRunInformationToSend()
        {
            this.m_swipeIndex = uint.MaxValue;
        }

        public WGPacketBasicRunInformationToSend(uint swipeIndex)
        {
            this.m_swipeIndex = uint.MaxValue;
            this.m_swipeIndex = swipeIndex;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[0x18];
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
            Array.Copy(BitConverter.GetBytes(this.m_swipeIndex), 0, destinationArray, 20, 4);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x18, destinationArray)), 0, destinationArray, 2, 2);
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

