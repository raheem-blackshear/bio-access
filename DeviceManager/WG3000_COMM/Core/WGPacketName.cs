using System;
using System.Collections.Generic;
using System.Text;

namespace WG3000_COMM.Core
{
    internal class WGPacketName : WGPacket
    {
        public uint userID;
        public uint tag;
        public byte[] data;

        public WGPacketName(byte typePar, byte codePar, uint snToPar,
            uint userID, uint tag) : base(typePar, codePar, snToPar)
        {
            data = new byte[0x400];
            this.userID = userID;
            this.tag = tag;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[Length];
            destinationArray[0] = base.type;
            destinationArray[1] = base.code;
            Array.Copy(BitConverter.GetBytes(srcPort), 0, destinationArray, 2, 2);
            Array.Copy(BitConverter.GetBytes(base._xid), 0, destinationArray, 4, 4);
            Array.Copy(BitConverter.GetBytes(base.iDevSnFrom), 0, destinationArray, 8, 4);
            Array.Copy(BitConverter.GetBytes(base.iDevSnTo), 0, destinationArray, 12, 4);
            destinationArray[0x10] = base.iCallReturn;
            destinationArray[0x11] = base.driverVer;
            destinationArray[0x12] = (byte)wgTools.gPTC_internal;
            destinationArray[0x13] = base.reserved19;
            Array.Copy(BitConverter.GetBytes(this.userID), 0, destinationArray, 0x14, 4);
            Array.Copy(BitConverter.GetBytes(this.tag), 0, destinationArray, 0x18, 4);
            Array.Copy(this.data, 0, destinationArray, 0x1c, this.data.Length);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(Length, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public static uint Length
        {
            get { return 0x41c; }
        }
    }
}
