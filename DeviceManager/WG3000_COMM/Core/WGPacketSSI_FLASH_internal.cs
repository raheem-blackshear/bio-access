namespace WG3000_COMM.Core
{
    using System;

    internal class WGPacketSSI_FLASH_internal : WGPacket
    {
        public uint iEndFlashAddr;
        public uint iStartFlashAddr;
        public byte[] ucData;

        public WGPacketSSI_FLASH_internal()
        {
            this.ucData = new byte[0x400];
            base.iCallReturn = 0;
        }

        public WGPacketSSI_FLASH_internal(byte typePar, byte codePar, uint snToPar,
            uint startAddr, uint endAddr) : base(typePar, codePar, snToPar)
        {
            this.ucData = new byte[0x400];
            for (int i = 0; i < ucData.Length; i++)
                ucData[i] = 0xff;
            iStartFlashAddr = startAddr;
            iEndFlashAddr = endAddr;
        }

        public WGPacketSSI_FLASH_internal(byte[] rcvdata) : base(rcvdata)
        {
            this.ucData = new byte[0x400];
            this.iStartFlashAddr = BitConverter.ToUInt32(rcvdata, 20);
            this.iEndFlashAddr = BitConverter.ToUInt32(rcvdata, 0x18);
            Array.Copy(rcvdata, 0x1c, this.ucData, 0, this.ucData.Length);
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[0x41c];
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
            Array.Copy(BitConverter.GetBytes(this.iStartFlashAddr), 0, destinationArray, 20, 4);
            Array.Copy(BitConverter.GetBytes(this.iEndFlashAddr), 0, destinationArray, 0x18, 4);
            Array.Copy(this.ucData, 0, destinationArray, 0x1c, this.ucData.Length);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x41c, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }
    }
}

