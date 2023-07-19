using System;
using System.Collections.Generic;
using System.Text;

namespace WG3000_COMM.Core
{
    class MjFaceEntry
    {
	    public bool valid;
        public ushort pos;
        public const uint Length = 3;

        public MjFaceEntry()
        {
            valid = false;
            pos = 0;
        }

        public void get(byte[] raw, int org)
        {
            valid = (raw[org] != 0);
            pos = BitConverter.ToUInt16(raw, org + 1);
        }

        public bool isValid()
        {
            return valid;
        }

        public byte[] toRaw()
        {
            byte[] raw = new byte[Length];
            raw[0] = valid ? (byte)1 : (byte)0;
            Array.Copy(BitConverter.GetBytes(pos), 0, raw, 1, 2);

            return raw;
        }
    }
}
