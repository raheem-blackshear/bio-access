namespace WG3000_COMM.Core
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;

    /* User Registration Information */
    public class MjUserInfo
    {
        private const int DoorCount = 4;
        private const int FingersNum = 3;

        private uint m_uiUserId;
        private uint m_uiRegFlag;               // all flags & options
        private ulong m_AllowFloors;
        private long m_CardId = 0L;
        private byte[] m_controlIndex = new byte[DoorCount];
        private int m_extfunction;
        private uint m_flashAddr = 0;
        private bool m_FloorControl;
        private DateTime m_hmsEnd = DateTime.Parse("2029-12-31 23:59:59");
        private int m_maxSwipe;
        private byte[] m_moreCardGroup = new byte[DoorCount];
        private RegisterCardOption m_option = RegisterCardOption.NotDeleted;
        private uint m_password = 0;
        private DateTime m_ymdEnd = new DateTime(0x7e4, 12, 0x1f);
        private DateTime m_ymdStart = new DateTime(0x7d9, 1, 1);
        private List<MjFpTempl> m_fpTemplList = new List<MjFpTempl>();

        public const uint Length = 0x20;
        private const int UserNameLength = 0x1c;

        public MjUserInfo()
        {
            this.m_uiUserId = 0;
            this.m_uiRegFlag = 0;
            this.m_password = 0;
            this.m_ymdStart = new DateTime(0x7da, 1, 1);
            this.m_ymdEnd = new DateTime(0x7ed, 12, 0x1f);
            this.m_controlIndex = new byte[DoorCount];
            this.m_moreCardGroup = new byte[DoorCount];
            this.m_extfunction = 0;
            this.m_maxSwipe = 0;
            this.m_hmsEnd = DateTime.Parse("2029-12-31 23:59:59");
        }

        public byte ControlSegIndexGet(byte DoorNo)
        {
            if ((DoorNo < 1) || (DoorNo > DoorCount))
                throw new IndexOutOfRangeException();
            return this.m_controlIndex[DoorNo - 1];
        }

        public void ControlSegIndexSet(byte DoorNo, byte ControlSegIndex)
        {
            if ((DoorNo < 1) || (DoorNo > DoorCount))
                throw new IndexOutOfRangeException();
            this.m_controlIndex[DoorNo - 1] = ControlSegIndex;
        }

        public bool FirstCardGet(byte DoorNo)
        {
            if ((DoorNo < 1) || (DoorNo > DoorCount))
                throw new IndexOutOfRangeException();
            return ((((byte)this.m_option) & (1 << (DoorNo - 1))) > 0);
        }

        public void FirstCardSet(byte DoorNo, bool val)
        {
            if ((DoorNo < 1) || (DoorNo > DoorCount))
                throw new IndexOutOfRangeException();
            if (val)
                this.m_option = (RegisterCardOption) ((byte)(this.m_option) | (1 << (DoorNo - 1)));
            else
                this.m_option = (RegisterCardOption) ((byte)(this.m_option) & ~(1 << (DoorNo - 1)));
        }

        public byte MoreCardGroupIndexGet(byte DoorNo)
        {
            if ((DoorNo < 1) || (DoorNo > DoorCount))
                throw new IndexOutOfRangeException();
            return this.m_moreCardGroup[DoorNo - 1];
        }

        public void MoreCardGroupIndexSet(byte DoorNo, byte moreCardGroupIndex)
        {
            if (((DoorNo < 1) || (DoorNo > DoorCount)) || (moreCardGroupIndex >= 0x10))
                throw new IndexOutOfRangeException();
            this.m_moreCardGroup[DoorNo - 1] = moreCardGroupIndex;
        }

        public byte[] ToBytes()
        {
            byte[] destinationArray = new byte[4 + Length];
            Array.Copy(BitConverter.GetBytes(this.m_flashAddr), 0, destinationArray, 0, 4);
            Array.Copy(BitConverter.GetBytes(this.m_uiUserId), 0, destinationArray, 4, 4);
            Array.Copy(BitConverter.GetBytes(this.m_uiRegFlag), 0, destinationArray, 8, 4);
            Array.Copy(BitConverter.GetBytes(this.m_CardId), 0, destinationArray, 0x0c, 8);
            destinationArray[0x14] = (byte) this.m_option;
            Array.Copy(BitConverter.GetBytes(this.m_password), 0, destinationArray, 0x15, 3);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDateYMD(this.m_ymdStart)), 0, destinationArray, 0x18, 2);
            Array.Copy(BitConverter.GetBytes(wgTools.MsDateToWgDateYMD(this.m_ymdEnd)), 0, destinationArray, 0x1A, 2);
            Array.Copy(this.m_controlIndex, 0, destinationArray, 0x1c, DoorCount);
            int num = this.m_moreCardGroup[0] & 15;
            num += (this.m_moreCardGroup[1] & 15) << 4;
            num += (this.m_moreCardGroup[2] & 15) << 8;
            num += (this.m_moreCardGroup[3] & 15) << 12;
            //num += (this.m_moreCardGroup[4] & 15) << 16;
            Array.Copy(BitConverter.GetBytes(num), 0, destinationArray, 0x20, 2);
            if (this.m_extfunction == 1)
            {
                int num2 = (int) (wgTools.MsDateToWgDateHMS(this.m_hmsEnd) >> 5);
                destinationArray[0x22] = (byte) (num2 & 0xff);
                destinationArray[0x23] = (byte) (((num2 & 0x3f00) >> 8) | (this.m_extfunction << 6));
            }
            else if (this.m_extfunction == 0)
            {
                destinationArray[0x22] = (byte) (this.m_maxSwipe & 0xff);
                destinationArray[0x23] = (byte) (((this.m_maxSwipe & 0x100) >> 8) | (this.m_extfunction << 6));
            }
            if (this.m_FloorControl)
            {
                Array.Copy(BitConverter.GetBytes(this.m_AllowFloors), 0, destinationArray, 0x1d, 3);
                destinationArray[0x21] = (byte) ((this.m_AllowFloors >> 0x18) & ((ulong) 0xffL));
                destinationArray[0x20] = (byte) ((destinationArray[0x20] & 15) + ((byte) (((this.m_AllowFloors >> 0x20) << 4) & ((ulong) 240L))));
                destinationArray[0x14] = (byte) (((byte) (destinationArray[0x14] & 0xf1)) + ((byte) (((this.m_AllowFloors >> 0x24) & 7L) << 1)));
                destinationArray[0x17] = (byte) ((destinationArray[0x17] & 0x3f) + ((byte) (((this.m_AllowFloors >> 0x27) & 3L) << 6)));
            }
            return destinationArray;
        }

        public int get(string strRegisterCardAll)
        {
            if (string.IsNullOrEmpty(strRegisterCardAll) || (strRegisterCardAll.Length != (Length * 2)))
                return -1;

            byte[] bytRegisterCard = new byte[Length];
            for (int i = 0; i < Length; i++)
                bytRegisterCard[i] = Convert.ToByte(strRegisterCardAll.Substring(i * 2, 2), 0x10);
            return this.get(bytRegisterCard, 0);
        }

        public int get(byte[] raw, int index)
        {
            try
            {
                if ((BitConverter.ToUInt32(raw, index) == 0L) ||
                    (BitConverter.ToUInt32(raw, index) == uint.MaxValue))
                    return -1;

                //this.m_flashAddr = flashAddr;
                this.m_uiUserId = (uint)BitConverter.ToUInt32(raw, index);
                this.m_uiRegFlag = (uint)BitConverter.ToUInt32(raw, index + 0x4);
                this.m_CardId = (long)BitConverter.ToUInt64(raw, index + 0x8);
                this.m_option = (RegisterCardOption)raw[index + 0x10];
                this.m_password = (BitConverter.ToUInt32(raw, index + 0x11) >> 8) & 0x3fffff;
                
                byte[] destinationArray = new byte[4];
                destinationArray[2] = 0;
                destinationArray[3] = 0;
                Array.Copy(raw, index + 0x14, destinationArray, 0, 2);
                this.m_ymdStart = wgTools.WgDateToMsDate(destinationArray, 0);

                Array.Copy(raw, index + 0x16, destinationArray, 0, 2);
                this.m_ymdEnd = wgTools.WgDateToMsDate(destinationArray, 0);

                Array.Copy(raw, index + 0x18, this.m_controlIndex, 0, DoorCount);
                this.m_moreCardGroup[0] = (byte)(raw[index + 0x1c] & 15);
                this.m_moreCardGroup[1] = (byte)(raw[index + 0x1c] >> 4);
                this.m_moreCardGroup[2] = (byte)(raw[index + 0x1d] & 15);
                this.m_moreCardGroup[3] = (byte)(raw[index + 0x1d] >> 4);
                //this.m_moreCardGroup[4] = (byte)(raw[index + 0x1f] >> 4);

                this.m_extfunction = 0;
                if ((raw[index + 0x1f] & 0x40) > 0)
                {
                    this.m_extfunction = 1;
                    byte[] dts = new byte[] { raw[index + 0x16], raw[index + 0x17],
                        (byte) (((raw[index + 0x1e] + ((raw[index + 0x1f] & 0x3f) << 8)) << 5) & 0xff),
                        (byte) ((((raw[index + 0x1e] + ((raw[index + 0x1f] & 0x3f) << 8)) << 5) >> 8) & 0xff) };
                    this.m_hmsEnd = wgTools.WgDateToMsDate(dts, 0);
                    this.m_hmsEnd = DateTime.Parse(this.m_hmsEnd.ToString("yyyy-MM-dd HH:mm:59"));
                    this.m_ymdEnd = DateTime.Parse(this.m_ymdEnd.ToString("yyyy-MM-dd ") + this.m_hmsEnd.ToString("HH:mm:ss"));
                }
                else
                {
                    this.m_maxSwipe = raw[index + 0x1e] + (raw[index + 0x1f] << 8);
                }
                this.m_AllowFloors = 0L;
                this.m_AllowFloors += raw[index + 0x19];
                this.m_AllowFloors += (ulong)raw[index + 0x1a] << 8;
                this.m_AllowFloors += (ulong)raw[index + 0x1b] << 0x10;
                this.m_AllowFloors += (ulong)raw[index + 0x1d] << 0x18;
                this.m_AllowFloors += ((ulong)raw[index + 0x1c] >> 4) << 0x20;
                this.m_AllowFloors += ((((ulong)raw[index + 0x10] & 14L) >> 1) << 0x24);
                this.m_AllowFloors += ((ulong)raw[index + 0x13] >> 6) << 0x27;
                return 1;
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public ulong AllowFloors
        {
            get { return this.AllowFloors_internal; }
            set { this.AllowFloors_internal = value; }
        }

        internal ulong AllowFloors_internal
        {
            get { return this.m_AllowFloors; }
            set
            {
                if (value < 0x20000000000L)
                {
                    this.m_AllowFloors = value;
                    this.m_FloorControl = true;
                }
            }
        }

        public uint userID
        {
            get { return m_uiUserId; }
            set { m_uiUserId = value; }
        }

        public long cardID
        {
            get { return (long)(this.m_CardId & 0x7fffffffffffffffL); }
            set { this.m_CardId = value; }
        }

        public uint FlashAddr
        {
            get { return this.m_flashAddr; }
        }

        public DateTime hmsEnd
        {
            get { return this.m_hmsEnd; }
            set { this.m_extfunction = 1; this.m_hmsEnd = value; }
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
            get { return this.m_maxSwipe; }
            set
            {
                if (value > 0)
                {
                    this.m_extfunction = 0;
                    this.m_maxSwipe = value & 0x1ff;
                }
            }
        }

        public uint password
        {
            get { return this.m_password; }
            set
            {
                if (value <= 0xf423f)
                    this.m_password = value;
            }
        }

        public DateTime ymdEnd
        {
            get { return this.m_ymdEnd; }
            set { this.m_ymdEnd = value; }
        }

        public DateTime ymdStart
        {
            get { return this.m_ymdStart; }
            set { this.m_ymdStart = value; }
        }

        public List<MjFpTempl> fpTemplList
        {
            get { return m_fpTemplList; }
        }

        public MjFaceTempl faceTempl
        {
            get;
            set;
        }

        public static uint invalidUserID
        {
            get { return 0xffffffff; }
        }

        public bool isValid()
        {
            return (userID != invalidUserID);
        }

        public uint flag
        {
            get { return m_uiRegFlag; }
            set { m_uiRegFlag = value; }
        }

        public static uint fpMin
        {
            get { return 0; }
        }

        public static uint fpMax
        {
            get { return FingersNum - 1; }
        }

        public static string getUserName(byte[] raw, uint index)
        {
            string str = null;
            byte[] name = new byte[UserNameLength];
            int length = 0;
            name[UserNameLength - 1] = 0;
            for (int k = 0; k < UserNameLength; k++)
            {
                name[k] = raw[index + k];
                if (name[k] == 0)
                    break;
                length++;
            }
            if ((name[0] != 0xff || name[1] != 0xff) && (name[0] != 0))
                str = Encoding.GetEncoding("utf-8").GetString(name, 0, length);
            return str;
        }

        public void hasFingerprint(byte index)
        {
            flag |= (uint)(1 << (index * 2));
        }

        public void hasFace()
        {
            flag |= (1 << 21);
        }

        public void hasDuress(byte index)
        {
            flag |= (uint)(1 << (index * 2 + 1));
        }

        public void hasCard()
        {
            flag |= (1 << 22);
        }

        public void hasPassword()
        {
            flag |= (1 << 20);
        }

        public void accessAllowed()
        {
            flag |= (1 << 27);
        }

        [Flags]
        private enum RegisterCardOption : byte
        {
            Activate = 0x40,
            firstCardOfDoor1 = 0x1,
            firstCardOfDoor2 = 0x2,
            firstCardOfDoor3 = 0x4,
            firstCardOfDoor4 = 0x8,
            //firstCardOfDoor5 = 0x10,
            NotDeleted = 0x80,
            Reserved5 = 0x20,
            SuperCard = 0x10
        }
    }
}

