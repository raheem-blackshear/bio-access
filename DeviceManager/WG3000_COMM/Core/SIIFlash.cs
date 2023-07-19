namespace WG3000_COMM.Core
{
    using System;

    internal class SIIFlash
    {
        public byte[] data = new byte[0x7e8000];

        public SIIFlash()
        {
            for (int i = 0; i < this.data.Length; i++)
            {
                this.data[i] = 0xff;
            }
        }

        public byte getData(long addr)
        {
            if (addr < this.data.Length)
            {
                return this.data[(int) ((IntPtr) addr)];
            }
            return 0xff;
        }
    }
}

