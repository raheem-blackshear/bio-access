namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;

    public class wgMjControllerTimeSegList
    {
        private ArrayList arrTaskList = new ArrayList();
        private byte[] m_data = new byte[0x2000];

        public wgMjControllerTimeSegList()
        {
            this.Format();
        }

        public int AddItem(MjControlTimeSeg mjControlTimeSeg)
        {
            if (this.arrTaskList.Count <= 0xfc)
            {
                this.arrTaskList.Add(mjControlTimeSeg);
                return 1;
            }
            return -1;
        }

        public void Clear()
        {
            this.Format();
        }

        private void Format()
        {
            for (int i = 0; i < this.m_data.Length; i++)
            {
                this.m_data[i] = 0xff;
            }
            this.arrTaskList.Clear();
        }

        public byte[] ToByte()
        {
            for (int i = 0; i < this.m_data.Length; i++)
            {
                this.m_data[i] = 0xff;
            }
            if (this.arrTaskList.Count > 0)
            {
                foreach (object obj2 in this.arrTaskList)
                {
                    if ((obj2 as MjControlTimeSeg).SegIndex < 0xff)
                    {
                        (obj2 as MjControlTimeSeg).ToBytes().CopyTo(this.m_data, (int) ((obj2 as MjControlTimeSeg).SegIndex * MjControlTimeSeg.byteLen));
                    }
                }
            }
            return this.m_data;
        }
    }
}

