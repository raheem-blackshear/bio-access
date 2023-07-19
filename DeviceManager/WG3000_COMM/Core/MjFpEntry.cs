using System;
using System.Collections.Generic;
using System.Text;

namespace WG3000_COMM.Core
{
    class MjFpEntry
    {
	    public bool valid;
        public byte index;
        public ushort pos;
        public const uint Length = 4;

        public MjFpEntry()
        {
            valid = false;
            index = 0;
            pos = 0;
        }

        public void get(byte[] raw, int org)
        {
            valid = (raw[org] != 0);
            index = raw[org + 1];
            pos = BitConverter.ToUInt16(raw, org + 2);
        }

        public bool isValid()
        {
            return (valid && (index >= MjUserInfo.fpMin && index <= MjUserInfo.fpMax));
        }

        public byte[] toRaw()
        {
            byte[] raw = new byte[Length];
            raw[0] = valid ? (byte)1 : (byte)0;
            raw[1] = index;
            Array.Copy(BitConverter.GetBytes(pos), 0, raw, 2, 2);

            return raw;
        }
    }
}
