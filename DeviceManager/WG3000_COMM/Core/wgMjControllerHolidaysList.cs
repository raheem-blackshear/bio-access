namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;

    public class wgMjControllerHolidaysList
    {
        private ArrayList arrHolidayList;
        private byte[] m_data;

        public wgMjControllerHolidaysList()
        {
            this.m_data = new byte[0x1000];
            this.arrHolidayList = new ArrayList();
            this.Format();
        }

        public wgMjControllerHolidaysList(byte[] byt4K)
        {
            this.m_data = new byte[0x1000];
            this.arrHolidayList = new ArrayList();
            this.Format();
            byte[] destinationArray = new byte[MjControlHolidayTime.byteLen];
            if (byt4K.Length == 0x1000)
            {
                for (int i = 0; (i + MjControlHolidayTime.byteLen) < byt4K.Length; i += MjControlHolidayTime.byteLen)
                {
                    if (wgTools.IsAllFF(byt4K, i, MjControlHolidayTime.byteLen))
                    {
                        return;
                    }
                    Array.Copy(byt4K, i, destinationArray, 0, MjControlHolidayTime.byteLen);
                    MjControlHolidayTime mjCHT = new MjControlHolidayTime(destinationArray);
                    this.AddItem(mjCHT);
                }
            }
        }

        public int AddItem(MjControlHolidayTime mjCHT)
        {
            if (this.arrHolidayList.Count >= MaxHolidayNum)
            {
                return -1;
            }
            if (this.arrHolidayList.Count == 0)
            {
                this.arrHolidayList.Add(mjCHT);
            }
            else
            {
                int index = 0;
                foreach (object obj2 in this.arrHolidayList)
                {
                    if (BitConverter.ToString((obj2 as MjControlHolidayTime).ToBytes()) == BitConverter.ToString(mjCHT.ToBytes()))
                    {
                        return 0;
                    }
                    if ((obj2 as MjControlHolidayTime).dtStart > mjCHT.dtStart)
                    {
                        this.arrHolidayList.Insert(index, mjCHT);
                        return 1;
                    }
                    index++;
                }
                this.arrHolidayList.Add(mjCHT);
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
            this.arrHolidayList.Clear();
        }

        public byte[] ToByte()
        {
            for (int i = 0; i < this.m_data.Length; i++)
            {
                this.m_data[i] = 0xff;
            }
            if (this.arrHolidayList.Count > 0)
            {
                int index = 0;
                foreach (object obj2 in this.arrHolidayList)
                {
                    (obj2 as MjControlHolidayTime).ToBytes().CopyTo(this.m_data, index);
                    index += MjControlHolidayTime.byteLen;
                }
            }
            return this.m_data;
        }

        public int holidayCount
        {
            get
            {
                return this.arrHolidayList.Count;
            }
        }

        public static int MaxHolidayNum
        {
            get
            {
                return 500;
            }
        }
    }
}

