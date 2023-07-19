namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;

    public class wgMjControllerTaskList
    {
        private ArrayList arrTaskList;
        private byte[] m_data;

        public wgMjControllerTaskList()
        {
            this.m_data = new byte[0x1000];
            this.arrTaskList = new ArrayList();
            this.Format();
        }

        public wgMjControllerTaskList(byte[] byt4K)
        {
            this.m_data = new byte[0x1000];
            this.arrTaskList = new ArrayList();
            this.Format();
            byte[] destinationArray = new byte[MjControlTaskItem.byteLen];
            if (byt4K.Length == 0x1000)
            {
                for (int i = 0; (i + MjControlTaskItem.byteLen) < byt4K.Length; i += MjControlTaskItem.byteLen)
                {
                    if (wgTools.IsAllFF(byt4K, i, MjControlTaskItem.byteLen))
                    {
                        return;
                    }
                    Array.Copy(byt4K, i, destinationArray, 0, MjControlTaskItem.byteLen);
                    MjControlTaskItem mjCI = new MjControlTaskItem(destinationArray);
                    this.AddItem(mjCI);
                }
            }
        }

        public int AddItem(MjControlTaskItem mjCI)
        {
            if (this.arrTaskList.Count >= MaxTasksNum)
            {
                return -1;
            }
            if (this.arrTaskList.Count == 0)
            {
                this.arrTaskList.Add(mjCI);
            }
            else
            {
                int index = 0;
                foreach (object obj2 in this.arrTaskList)
                {
                    if (BitConverter.ToString((obj2 as MjControlTaskItem).ToBytes()) == BitConverter.ToString(mjCI.ToBytes()))
                    {
                        return 0;
                    }
                    if ((obj2 as MjControlTaskItem).hms > mjCI.hms)
                    {
                        this.arrTaskList.Insert(index, mjCI);
                        return 1;
                    }
                    index++;
                }
                this.arrTaskList.Add(mjCI);
            }
            return 1;
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
                int index = 0;
                foreach (object obj2 in this.arrTaskList)
                {
                    (obj2 as MjControlTaskItem).ToBytes().CopyTo(this.m_data, index);
                    index += MjControlTaskItem.byteLen;
                }
            }
            return this.m_data;
        }

        public static int MaxTasksNum
        {
            get
            {
                return 400;
            }
        }

        public int taskCount
        {
            get
            {
                return this.arrTaskList.Count;
            }
        }
    }
}

