namespace WG3000_COMM.Core
{
    using System;

    internal class wgCRC
    {
        public static ushort CRC_16_IBM(uint len, byte[] data)
        {
            uint num3 = 0;
            byte num4 = 0;
            for (uint i = 0; i < len; i++)
            {
                num4 = data[i];
                switch (i)
                {
                    case 2:
                    case 3:
                        num4 = 0;
                        break;
                }
                num3 ^= num4;
                for (uint j = 0; j < 8; j++)
                {
                    if ((num3 & 1) > 0)
                    {
                        num3 = (num3 >> 1) ^ 0xa001;
                    }
                    else
                    {
                        num3 = num3 >> 1;
                    }
                }
            }
            return (ushort) (num3 & 0xffff);
        }
    }
}

