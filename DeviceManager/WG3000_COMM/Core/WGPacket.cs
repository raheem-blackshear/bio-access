namespace WG3000_COMM.Core
{
    using AesLib;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    public class WGPacket
    {
        private static uint _Global_xid = 0;
        internal uint _xid;
        [CompilerGenerated]
        private byte _code;
        [CompilerGenerated]
        private byte _iCallReturn;
        [CompilerGenerated]
        private uint _iDevSnFrom;
        [CompilerGenerated]
        private uint _iDevSnTo;
        [CompilerGenerated]
        private byte _type;
        protected internal byte driverVer;
        private static byte[] IV = Encoding.ASCII.GetBytes("wiegand1997csncl");
        private static byte[] Key = new byte[] { 0x57, 0x47, 0x4d, 0x41, 0x53, 0x54, 0x45, 0x52, 0x77, 0x67, 0x6d, 0x61, 0x73, 0x74, 0x65, 0x72 };//WGMASTERwgmaster
        private static bool m_bCommPassword = false;
        private static string m_strCommP = "";
        protected internal byte reserved18;
        protected internal byte reserved19;
        private const int SUCCESS = 1;
        private const int WGP_ALLOC_ERR = -102;
        private const int WGP_CODE_ERR = -104;
        private const int WGP_CRC_ERR = -103;
        private const int WGP_INVALID = -101;
        private const int WGP_INVALID_AES = -108;
        private const int WGP_NOT_ME = -107;
        private const int WGP_SELF_SEND = -106;
        private const int WGP_TYPE_ERR = -105;

        public WGPacket()
        {
            this.reserved18 = (byte) wgTools.gPTC_internal;
            _Global_xid++;
            this._xid = _Global_xid;
            this.GetCommP();
        }

        public WGPacket(byte[] rcvdata)
        {
            this.reserved18 = (byte) wgTools.gPTC_internal;
            this.type = rcvdata[0];
            this.code = rcvdata[1];
            this._xid = BitConverter.ToUInt32(rcvdata, 4);
            this.iDevSnFrom = BitConverter.ToUInt32(rcvdata, 8);
            this.iDevSnTo = BitConverter.ToUInt32(rcvdata, 12);
            this.iCallReturn = rcvdata[0x10];
            this.driverVer = rcvdata[0x11];
            this.reserved18 = rcvdata[0x12];
            this.reserved19 = rcvdata[0x13];
            this.GetCommP();
        }

        public WGPacket(byte typePar, byte codePar, uint snToPar)
        {
            this.reserved18 = (byte) wgTools.gPTC_internal;
            this.type = typePar;
            this.code = codePar;
            this.iDevSnFrom = 0;
            this.iDevSnTo = snToPar;
            this.iCallReturn = 0;
            this.driverVer = 0;
            this.reserved18 = (byte) wgTools.gPTC_internal;
            this.reserved19 = 0;
            _Global_xid++;
            this._xid = _Global_xid;
            this.GetCommP();
        }

        private static bool CrcCheck(byte[] rcvdata, int ud_srcp)
        {
            ushort num = BitConverter.ToUInt16(rcvdata, 2);
            rcvdata[2] = (byte) (ud_srcp & 0xff);
            rcvdata[3] = (byte) ((ud_srcp >> 8) & 0xff);
            return (wgCRC.CRC_16_IBM((uint) rcvdata.Length, rcvdata) == num);
        }

        private static void DecWGPacket(ref byte[] pwgPktBytes, int len)
        {
            if ((bCommP && (len >= 4)) && ((pwgPktBytes[0] & 0x80) != 0))
            {
                pwgPktBytes[0] = (byte) (pwgPktBytes[0] & 0x7f);
                int num = len - 4;
                byte num2 = pwgPktBytes[4];
                byte[] keyBytes = new byte[0x10];
                char[] chArray = Dpt(m_strCommP).PadRight(0x10, '\0').ToCharArray();
                for (int i = 0; i < 0x10; i++)
                {
                    keyBytes[i] = (byte) (chArray[i] & '\x00ff');
                }
                AesLib.Aes aes = new AesLib.Aes(AesLib.Aes.KeySize.Bits128, keyBytes);
                byte[] input = new byte[0x10];
                byte[] output = new byte[0x10];
                for (int j = 0; j < num; j += 0x10)
                {
                    if ((j + 0x10) > num)
                    {
                        while (j < num)
                        {
                            pwgPktBytes[4 + j] = (byte) (pwgPktBytes[j + 4] ^ num2);
                            j++;
                        }
                        return;
                    }
                    for (int k = 0; k < 0x10; k++)
                    {
                        input[k] = pwgPktBytes[(4 + j) + k];
                    }
                    aes.InvCipher(input, ref output);
                    for (int m = 0; m < 0x10; m++)
                    {
                        pwgPktBytes[(4 + j) + m] = output[m];
                    }
                    if (j == 0)
                    {
                        num2 = pwgPktBytes[4];
                    }
                }
            }
        }

        public static string Dpt(string StrInput)
        {
            string str = "";
            byte[] buffer = Convert.FromBase64String(StrInput);
            using (MemoryStream stream = new MemoryStream())
            {
                using (RijndaelManaged managed = new RijndaelManaged())
                {
                    CryptoStream stream2 = new CryptoStream(stream, managed.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.FlushFinalBlock();
                    str = Encoding.Default.GetString(stream.ToArray());
                }
            }
            return str;
        }

        protected internal void EncWGPacket(ref byte[] pwgPktBytes, int len)
        {
            if (bCommP && (len >= 4))
            {
                pwgPktBytes[0] = (byte) (pwgPktBytes[0] | 0x80);
                int num = len - 4;
                byte num2 = pwgPktBytes[4];
                byte[] keyBytes = new byte[0x10];
                char[] chArray = Dpt(m_strCommP).PadRight(0x10, '\0').ToCharArray();
                for (int i = 0; i < 0x10; i++)
                {
                    keyBytes[i] = (byte) (chArray[i] & '\x00ff');
                }
                AesLib.Aes aes = new AesLib.Aes(AesLib.Aes.KeySize.Bits128, keyBytes);
                byte[] input = new byte[0x10];
                byte[] output = new byte[0x10];
                for (int j = 0; j < num; j += 0x10)
                {
                    if ((j + 0x10) > num)
                    {
                        while (j < num)
                        {
                            pwgPktBytes[4 + j] = (byte) (pwgPktBytes[j + 4] ^ num2);
                            j++;
                        }
                        return;
                    }
                    for (int k = 0; k < 0x10; k++)
                    {
                        input[k] = pwgPktBytes[(4 + j) + k];
                    }
                    aes.Cipher(input, ref output);
                    for (int m = 0; m < 0x10; m++)
                    {
                        pwgPktBytes[(4 + j) + m] = output[m];
                    }
                }
            }
        }

        public static string Ept(string StrInput)
        {
            string str = "";
            byte[] bytes = Encoding.Default.GetBytes(StrInput);
            using (MemoryStream stream = new MemoryStream())
            {
                using (RijndaelManaged managed = new RijndaelManaged())
                {
                    CryptoStream stream2 = new CryptoStream(stream, managed.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                    stream2.Write(bytes, 0, bytes.Length);
                    stream2.FlushFinalBlock();
                    str = Convert.ToBase64String(stream.ToArray());
                }
            }
            return str;
        }

        private void GetCommP()
        {
            if (string.IsNullOrEmpty(wgTools.CommPStr))
            {
                m_bCommPassword = false;
                m_strCommP = "";
            }
            else
            {
                m_bCommPassword = true;
                m_strCommP = wgTools.CommPStr;
            }
        }

        public void GetNewXid()
        {
            _Global_xid++;
            this._xid = _Global_xid;
        }

        internal static int Parsing(ref byte[] rcvdata, int srcPort)
        {
            if (rcvdata.Length < MinSize)
            {
                return -101;
            }
            if ((rcvdata[0] & 0x80) > 0)
            {
                DecWGPacket(ref rcvdata, rcvdata.Length);
            }
            if (!CrcCheck(rcvdata, srcPort))
            {
                if ((rcvdata.Length == 44) && (rcvdata[0] == 0x19))
                {
                    return 1;
                }
                return -103;
            }
            switch (((WGPacket_Type_internal) ((byte) (rcvdata[0] & 0x7f))))
            {
                case WGPacket_Type_internal.OP_BASIC:
                case WGPacket_Type_internal.OP_SSI_FLASH:
                case WGPacket_Type_internal.OP_BATCH_SSI_FLASH:
                case WGPacket_Type_internal.OP_PRIVILEGE:
                case WGPacket_Type_internal.OP_CONFIG:
                case WGPacket_Type_internal.OP_CPU_CONFIG:
                case WGPacket_Type_internal.OP_FP_ENROLL:
                case WGPacket_Type_internal.OP_FACE_ENROLL:
                    return 1;
            }
            return -105;
        }

        public byte[] ToBytes(ushort srcPort)
        {
            byte[] destinationArray = new byte[MinSize];
            destinationArray[0] = this.type;
            destinationArray[1] = this.code;
            Array.Copy(BitConverter.GetBytes(srcPort), 0, destinationArray, 2, 2);
            Array.Copy(BitConverter.GetBytes(this.xid), 0, destinationArray, 4, 4);
            Array.Copy(BitConverter.GetBytes(this.iDevSnFrom), 0, destinationArray, 8, 4);
            Array.Copy(BitConverter.GetBytes(this.iDevSnTo), 0, destinationArray, 12, 4);
            destinationArray[0x10] = this.iCallReturn;
            destinationArray[0x11] = this.driverVer;
            destinationArray[0x12] = (byte) wgTools.gPTC_internal;
            destinationArray[0x13] = this.reserved19;
            Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM((uint) MinSize, destinationArray)), 0, destinationArray, 2, 2);
            this.EncWGPacket(ref destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public static bool bCommP
        {
            get
            {
                return m_bCommPassword;
            }
            set
            {
                m_bCommPassword = value;
            }
        }

        public byte code
        {
            [CompilerGenerated]
            get
            {
                return this._code;
            }
            [CompilerGenerated]
            set
            {
                this._code = value;
            }
        }

        public byte iCallReturn
        {
            [CompilerGenerated]
            get
            {
                return this._iCallReturn;
            }
            [CompilerGenerated]
            set
            {
                this._iCallReturn = value;
            }
        }

        public uint iDevSnFrom
        {
            [CompilerGenerated]
            get
            {
                return this._iDevSnFrom;
            }
            [CompilerGenerated]
            set
            {
                this._iDevSnFrom = value;
            }
        }

        public uint iDevSnTo
        {
            [CompilerGenerated]
            get
            {
                return this._iDevSnTo;
            }
            [CompilerGenerated]
            set
            {
                this._iDevSnTo = value;
            }
        }

        public static int MinSize
        {
            get
            {
                return 20;
            }
        }

        public static string strCommP
        {
            set
            {
                m_strCommP = value;
            }
        }

        public byte type
        {
            [CompilerGenerated]
            get
            {
                return this._type;
            }
            [CompilerGenerated]
            set
            {
                this._type = value;
            }
        }

        public uint xid
        {
            get
            {
                return this._xid;
            }
        }
    }
}

