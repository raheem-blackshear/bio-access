namespace WG3000_COMM.Core
{
    using System;

    internal class WGPacketPrivilege : WGPacket
    {
        private MjUserInfo _mj;
        private const int m_len = 0x34;

        public WGPacketPrivilege(byte typePar, byte codePar, uint snToPar, MjUserInfo userInfo)
            : base(typePar, codePar, snToPar)
        {
            this._mj = userInfo;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[m_len];
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
            Array.Copy(this._mj.ToBytes(), 4, destinationArray, 0x14, MjUserInfo.Length);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(m_len, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }
    }
	
	internal class WGPacketPrivilegeS8000 : WGPacket
    {
        private MjRegisterCard _mj;
        private const int m_len = 0x2c;

        public WGPacketPrivilegeS8000(MjRegisterCard mj)
        {
            this._mj = mj;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[0x2c];
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
            Array.Copy(this._mj.ToBytes(), 4, destinationArray, 20, MjRegisterCard.byteLen);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x2c, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }
    }
}

