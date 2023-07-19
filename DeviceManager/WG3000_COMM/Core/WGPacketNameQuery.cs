﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WG3000_COMM.Core
{
    public class WGPacketNameQuery : WGPacket
    {
        public uint tag;

        public WGPacketNameQuery()
        {
        }

        public WGPacketNameQuery(byte typePar, byte codePar, uint snToPar, uint tag)
            : base(typePar, codePar, snToPar)
        {
            this.tag = tag;
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
            destinationArray[0x12] = (byte)wgTools.gPTC_internal;
            destinationArray[0x13] = base.reserved19;
            Array.Copy(BitConverter.GetBytes(tag), 0, destinationArray, 0x14, 4);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x18, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public bool get(byte[] raw, int org)
        {
            try
            {
                tag = BitConverter.ToUInt32(raw, org);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
