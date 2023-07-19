namespace WG3000_COMM.Core
{
    using System;

    internal class WGPacketBasicRemoteOpenDoorToSend : WGPacket
    {
        private int m_DoorNO;
        private int m_FloorNO;
        private const int m_len = 0x24;
        private ulong m_OperatorCardNO;
        private uint m_OperatorID;

        public WGPacketBasicRemoteOpenDoorToSend(int DoorNO)
        {
            this.m_DoorNO = 1;
            this.m_OperatorCardNO = ulong.MaxValue;
            if ((DoorNO < 1) || (DoorNO > 4))
            {
                throw new InvalidOperationException();
            }
            this.m_DoorNO = DoorNO;
        }

        public WGPacketBasicRemoteOpenDoorToSend(int DoorNO, int FloorNO)
        {
            this.m_DoorNO = 1;
            this.m_OperatorCardNO = ulong.MaxValue;
            if ((DoorNO < 1) || (DoorNO > 4))
            {
                throw new InvalidOperationException();
            }
            this.m_DoorNO = DoorNO;
            this.m_FloorNO = FloorNO;
        }

        internal void specialDoorNo4ping(int NO)
        {
            this.m_DoorNO = NO;
        }

        public new byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[0x24];
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
            Array.Copy(BitConverter.GetBytes(this.m_OperatorID), 0, destinationArray, 20, 4);
            Array.Copy(BitConverter.GetBytes(this.m_OperatorCardNO), 0, destinationArray, 0x18, 8);
            Array.Copy(BitConverter.GetBytes((int) (this.m_DoorNO - 1)), 0, destinationArray, 0x20, 4);
            destinationArray[0x21] = (byte) this.m_FloorNO;
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM(0x24, destinationArray)), 0, destinationArray, 2, 2);
            base.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public ulong OperatorCardNO
        {
            get
            {
                return this.m_OperatorCardNO;
            }
            set
            {
                this.m_OperatorCardNO = value;
            }
        }

        public uint OperatorID
        {
            get
            {
                return this.m_OperatorID;
            }
            set
            {
                this.m_OperatorID = value;
            }
        }
    }
}

