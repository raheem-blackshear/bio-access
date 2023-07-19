namespace WG3000_COMM.Core
{
    using System;

    public class WGPacketWith1024 : WGPacket
    {
        public byte[] ucData;

        public WGPacketWith1024()
        {
            ucData = new byte[0x400];
        }

        public WGPacketWith1024(byte typePar, byte codePar, uint snToPar)
            : base(typePar, codePar, snToPar)
        {
            ucData = new byte[0x400];
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[0x414];
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
            Array.Copy(this.ucData, 0, destinationArray, 20, this.ucData.Length);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x414, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public bool get(byte[] raw, int index)
        {
            try
            {
                Array.Copy(raw, index, ucData, 0, ucData.Length);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}

