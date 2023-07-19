namespace WG3000_COMM.Core
{
    using System;

    public class MjRegisterCard
    {
        private ulong m_AllowFloors;
        private uint uid = 0;
        private uint m_CardId = 0;
        private byte[] m_controlIndex = new byte[4];
        private int m_extfunction;
        private uint m_flashAddr = 0;
        private bool m_FloorControl;
        private DateTime m_hmsEnd = DateTime.Parse("2029-12-31 23:59:59");
        private int m_maxSwipe;
        private byte[] m_moreCardGroup = new byte[4];
        private RegisterCardOption m_option = (RegisterCardOption.NotDeleted | RegisterCardOption.Reserved5);
        private uint m_password = 0x5464e;
        private DateTime m_ymdEnd = new DateTime(0x7e4, 12, 0x1f);
        private DateTime m_ymdStart = new DateTime(0x7d9, 1, 1);

        public MjRegisterCard()
        {
            this.m_password = 0x5464e;
            this.m_ymdStart = new DateTime(0x7da, 1, 1);
            this.m_ymdEnd = new DateTime(0x7ed, 12, 0x1f);
            this.m_controlIndex = new byte[4];
            this.m_moreCardGroup = new byte[4];
            this.m_extfunction = 0;
            this.m_maxSwipe = 0;
            this.m_hmsEnd = DateTime.Parse("2029-12-31 23:59:59");
        }

        public byte ControlSegIndexGet(byte DoorNo)
        {
            if ((DoorNo < 1) || (DoorNo > 4))
            {
                throw new IndexOutOfRangeException();
            }
            return this.m_controlIndex[DoorNo - 1];
        }

        public void ControlSegIndexSet(byte DoorNo, byte ControlSegIndex)
        {
            if ((DoorNo < 1) || (DoorNo > 4))
            {
                throw new IndexOutOfRangeException();
            }
            this.m_controlIndex[DoorNo - 1] = ControlSegIndex;
        }

        public bool FirstCardGet(byte DoorNo)
        {
            if ((DoorNo < 1) || (DoorNo > 4))
            {
                throw new IndexOutOfRangeException();
            }
            return ((((byte)this.m_option) & (1 << (DoorNo - 1))) > 0);
        }

        public void FirstCardSet(byte DoorNo, bool val)
        {
            if ((DoorNo < 1) || (DoorNo > 4))
            {
                throw new IndexOutOfRangeException();
            }
            if (val)
            {
                this.m_option = (RegisterCardOption) ((byte)(this.m_option) | (1 << (DoorNo - 1)));
            }
            else
            {
                this.m_option = (RegisterCardOption) ((byte)(this.m_option) & ~(1 << (DoorNo - 1)));
            }
        }

        public byte MoreCardGroupIndexGet(byte DoorNo)
        {
            if ((DoorNo < 1) || (DoorNo > 4))
            {
                throw new IndexOutOfRangeException();
            }
            return this.m_moreCardGroup[DoorNo - 1];
        }

        public void MoreCardGroupIndexSet(byte DoorNo, byte moreCardGroupIndex)
        {
            if (((DoorNo < 1) || (DoorNo > 4)) || (moreCardGroupIndex >= 0x10))
            {
                throw new IndexOutOfRangeException();
            }
            this.m_moreCardGroup[DoorNo - 1] = moreCardGroupIndex;
        }

        public byte[] ToBytes()
        {
            byte[] destinationArray = new byte[4 + byteLen];
            Array.Copy(BitConverter.GetBytes(this.m_flashAddr), 0, destinationArray, 0, 4);
            Array.Copy(BitConverter.GetBytes(this.m_CardId), 0, destinationArray, 4, 4);
            Array.Copy(BitConverter.GetBytes(this.uid), 0, destinationArray, 8, 4);
            destinationArray[12] = (byte) this.m_option;
            Array.Copy(BitConverter.GetBytes(this.m_password), 0, destinationArray, 13, 3);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDateYMD(this.m_ymdStart)), 0, destinationArray, 0x10, 2);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDateYMD(this.m_ymdEnd)), 0, destinationArray, 0x12, 2);
            Array.Copy(this.m_controlIndex, 0, destinationArray, 20, 4);
            int num = this.m_moreCardGroup[0] & 15;
            num += (this.m_moreCardGroup[1] & 15) << 4;
            num += (this.m_moreCardGroup[2] & 15) << 8;
            num += (this.m_moreCardGroup[3] & 15) << 12;
            Array.Copy(BitConverter.GetBytes(num), 0, destinationArray, 0x18, 2);
            if (this.m_extfunction == 1)
            {
                int num2 = (int) (wgTools.MsDateToWgDateHMS(this.m_hmsEnd) >> 5);
                destinationArray[0x1a] = (byte) (num2 & 0xff);
                destinationArray[0x1b] = (byte) (((num2 & 0x3f00) >> 8) | (this.m_extfunction << 6));
            }
            else if (this.m_extfunction == 0)
            {
                destinationArray[0x1a] = (byte) (this.m_maxSwipe & 0xff);
                destinationArray[0x1b] = (byte) (((this.m_maxSwipe & 0x100) >> 8) | (this.m_extfunction << 6));
            }
            if (this.m_FloorControl)
            {
                Array.Copy(BitConverter.GetBytes(this.m_AllowFloors), 0, destinationArray, 0x15, 3);
                destinationArray[0x19] = (byte) ((this.m_AllowFloors >> 0x18) & ((ulong) 0xffL));
                destinationArray[0x18] = (byte) ((destinationArray[0x18] & 15) + ((byte) (((this.m_AllowFloors >> 0x20) << 4) & ((ulong) 240L))));
                destinationArray[12] = (byte) (((byte) (destinationArray[12] & 0xf1)) + ((byte) (((this.m_AllowFloors >> 0x24) & 7L) << 1)));
                destinationArray[15] = (byte) ((destinationArray[15] & 0x3f) + ((byte) (((this.m_AllowFloors >> 0x27) & 3L) << 6)));
            }
            return destinationArray;
        }

        public int Update(string strRegisterCardAll)
        {
            if (string.IsNullOrEmpty(strRegisterCardAll) || (strRegisterCardAll.Length != (byteLen * 2)))
            {
                return -1;
            }
            byte[] bytRegisterCard = new byte[byteLen];
            for (int i = 0; i < byteLen; i++)
            {
                bytRegisterCard[i] = Convert.ToByte(strRegisterCardAll.Substring(i * 2, 2), 0x10);
            }
            return this.Update(bytRegisterCard, 0);
        }

        public int Update(byte[] bytRegisterCard, uint flashAddr)
        {
            try
            {
                if ((BitConverter.ToUInt64(bytRegisterCard, 0) == 0L) || (BitConverter.ToUInt64(bytRegisterCard, 0) == ulong.MaxValue))
                {
                    return -1;
                }
                this.m_flashAddr = flashAddr;
                this.m_CardId = BitConverter.ToUInt32(bytRegisterCard, 0);
                uid = (uint)BitConverter.ToUInt32(bytRegisterCard, 4);
                this.m_option = (RegisterCardOption) bytRegisterCard[8];
                this.m_password = (BitConverter.ToUInt32(bytRegisterCard, 8) >> 8) & 0x3fffff;
                byte[] destinationArray = new byte[4];
                destinationArray[2] = 0;
                destinationArray[3] = 0;
                Array.Copy(bytRegisterCard, 12, destinationArray, 0, 2);
                this.m_ymdStart = wgTools.WgDateToMsDate(destinationArray, 0);
                Array.Copy(bytRegisterCard, 14, destinationArray, 0, 2);
                this.m_ymdEnd = wgTools.WgDateToMsDate(destinationArray, 0);
                Array.Copy(bytRegisterCard, 0x10, this.m_controlIndex, 0, 4);
                this.m_moreCardGroup[0] = (byte) (bytRegisterCard[20] & 15);
                this.m_moreCardGroup[1] = (byte) (bytRegisterCard[20] >> 4);
                this.m_moreCardGroup[2] = (byte) (bytRegisterCard[0x15] & 15);
                this.m_moreCardGroup[3] = (byte) (bytRegisterCard[0x15] >> 4);
                this.m_extfunction = 0;
                if ((bytRegisterCard[0x17] & 0x40) > 0)
                {
                    this.m_extfunction = 1;
                    byte[] dts = new byte[] { bytRegisterCard[14], bytRegisterCard[15], (byte) (((bytRegisterCard[0x16] + ((bytRegisterCard[0x17] & 0x3f) << 8)) << 5) & 0xff), (byte) ((((bytRegisterCard[0x16] + ((bytRegisterCard[0x17] & 0x3f) << 8)) << 5) >> 8) & 0xff) };
                    this.m_hmsEnd = wgTools.WgDateToMsDate(dts, 0);
                    this.m_hmsEnd = DateTime.Parse(this.m_hmsEnd.ToString("yyyy-MM-dd HH:mm:59"));
                    this.m_ymdEnd = DateTime.Parse(this.m_ymdEnd.ToString("yyyy-MM-dd ") + this.m_hmsEnd.ToString("HH:mm:ss"));
                }
                else
                {
                    this.m_maxSwipe = bytRegisterCard[0x16] + (bytRegisterCard[0x17] << 8);
                }
                this.m_AllowFloors = 0L;
                this.m_AllowFloors += bytRegisterCard[0x11];
                this.m_AllowFloors += (ulong)bytRegisterCard[0x12] << 8;
                this.m_AllowFloors += (ulong)bytRegisterCard[0x13] << 0x10;
                this.m_AllowFloors += (ulong)bytRegisterCard[0x15] << 0x18;
                this.m_AllowFloors += ((ulong)bytRegisterCard[20] >> 4) << 0x20;
                this.m_AllowFloors += ((((ulong)bytRegisterCard[8] & 14L) >> 1) << 0x24);
                this.m_AllowFloors += ((ulong)bytRegisterCard[11] >> 6) << 0x27;
                return 1;
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public ulong AllowFloors
        {
            get
            {
                return this.AllowFloors_internal;
            }
            set
            {
                this.AllowFloors_internal = value;
            }
        }

        internal ulong AllowFloors_internal
        {
            get
            {
                return this.m_AllowFloors;
            }
            set
            {
                if (value < 0x20000000000L)
                {
                    this.m_AllowFloors = value;
                    this.m_FloorControl = true;
                }
            }
        }

        internal static int byteLen
        {
            get
            {
                return 0x18;
            }
        }

        public uint CardID
        {
            get
            {
                return (uint) (this.m_CardId & 0x7fffffffffffffffL);
            }
            set
            {
                this.m_CardId = value;
            }
        }

        public uint userID
        {
            get { return uid; }
            set { uid = value; }
        }

        public uint FlashAddr
        {
            get
            {
                return this.m_flashAddr;
            }
        }

        public DateTime hmsEnd
        {
            get
            {
                return this.m_hmsEnd;
            }
            set
            {
                this.m_extfunction = 1;
                this.m_hmsEnd = value;
            }
        }

        public bool IsActivated
        {
            get
            {
                return (((byte) (this.m_option & RegisterCardOption.Activate)) == 0);
            }
            set
            {
                if (value)
                {
                    this.m_option = (RegisterCardOption) ((byte) (this.m_option & (RegisterCardOption.firstCardOfDoor1 | RegisterCardOption.firstCardOfDoor2 | RegisterCardOption.firstCardOfDoor3 | RegisterCardOption.firstCardOfDoor4 | RegisterCardOption.NotDeleted | RegisterCardOption.Reserved5 | RegisterCardOption.SuperCard)));
                }
                else
                {
                    this.m_option = (RegisterCardOption) ((byte) (this.m_option | RegisterCardOption.Activate));
                }
            }
        }

        public bool IsDeleted
        {
            get
            {
                return (((byte) (this.m_option & RegisterCardOption.NotDeleted)) == 0);
            }
            set
            {
                if (value)
                {
                    this.m_option = (RegisterCardOption) ((byte) (this.m_option & (RegisterCardOption.Activate | RegisterCardOption.firstCardOfDoor1 | RegisterCardOption.firstCardOfDoor2 | RegisterCardOption.firstCardOfDoor3 | RegisterCardOption.firstCardOfDoor4 | RegisterCardOption.Reserved5 | RegisterCardOption.SuperCard)));
                }
                else
                {
                    this.m_option = (RegisterCardOption) ((byte) (this.m_option | RegisterCardOption.NotDeleted));
                }
            }
        }

        public bool IsSuperCard
        {
            get
            {
                return (((byte) (this.m_option & RegisterCardOption.SuperCard)) > 0);
            }
            set
            {
                if (value)
                {
                    this.m_option = (RegisterCardOption) ((byte) (this.m_option | RegisterCardOption.SuperCard));
                }
                else
                {
                    this.m_option = (RegisterCardOption) ((byte) (this.m_option & (RegisterCardOption.Activate | RegisterCardOption.firstCardOfDoor1 | RegisterCardOption.firstCardOfDoor2 | RegisterCardOption.firstCardOfDoor3 | RegisterCardOption.firstCardOfDoor4 | RegisterCardOption.NotDeleted | RegisterCardOption.Reserved5)));
                }
            }
        }

        public int maxSwipe
        {
            get
            {
                return this.m_maxSwipe;
            }
            set
            {
                if (value > 0)
                {
                    this.m_extfunction = 0;
                    this.m_maxSwipe = value & 0x1ff;
                }
            }
        }

        public uint Password
        {
            get
            {
                return this.m_password;
            }
            set
            {
                if (value <= 0xf423f)
                {
                    this.m_password = value;
                }
            }
        }

        public DateTime ymdEnd
        {
            get
            {
                return this.m_ymdEnd;
            }
            set
            {
                this.m_ymdEnd = value;
            }
        }

        public DateTime ymdStart
        {
            get
            {
                return this.m_ymdStart;
            }
            set
            {
                this.m_ymdStart = value;
            }
        }

        [Flags]
        private enum RegisterCardOption : byte
        {
            Activate = 0x40,
            firstCardOfDoor1 = 1,
            firstCardOfDoor2 = 2,
            firstCardOfDoor3 = 4,
            firstCardOfDoor4 = 8,
            NotDeleted = 0x80,
            Reserved5 = 0x20,
            SuperCard = 0x10
        }
    }
}

