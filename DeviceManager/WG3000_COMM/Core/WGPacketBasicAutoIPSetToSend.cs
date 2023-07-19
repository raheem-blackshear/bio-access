namespace WG3000_COMM.Core
{
    using System;
    using System.Net;

    internal class WGPacketBasicAutoIPSetToSend : WGPacket
    {
        private uint m_cmdOption;
        private string m_gateway;
        private string m_ip;
        private uint m_ipend;
        private uint m_ipstart;
        private const int m_len = 40;
        private string m_mask;

        public WGPacketBasicAutoIPSetToSend()
        {
            this.m_ip = "0.0.0.0";
            this.m_mask = "255.0.0.0";
            this.m_gateway = "0.0.0.0";
            this.m_ipstart = 0x7b;
            this.m_ipend = 0xfd;
        }

        public WGPacketBasicAutoIPSetToSend(uint cmdOption, string ip, string mask, string gateway, uint ipstart, uint ipend)
        {
            this.m_ip = "0.0.0.0";
            this.m_mask = "255.0.0.0";
            this.m_gateway = "0.0.0.0";
            this.m_ipstart = 0x7b;
            this.m_ipend = 0xfd;
            this.m_cmdOption = cmdOption;
            this.m_ip = ip;
            this.m_mask = mask;
            this.m_gateway = gateway;
            this.m_ipstart = ipstart;
            this.m_ipend = ipend;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[40];
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
            Array.Copy(BitConverter.GetBytes(this.m_cmdOption), 0, destinationArray, 20, 4);
            IPAddress.Parse(this.m_ip).GetAddressBytes().CopyTo(destinationArray, 0x18);
            IPAddress.Parse(this.m_mask).GetAddressBytes().CopyTo(destinationArray, 0x1c);
            IPAddress.Parse(this.m_gateway).GetAddressBytes().CopyTo(destinationArray, 0x20);
            Array.Copy(BitConverter.GetBytes(this.m_ipstart), 0, destinationArray, 0x26, 1);
            Array.Copy(BitConverter.GetBytes(this.m_ipend), 0, destinationArray, 0x27, 1);
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(40, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }
    }
}

