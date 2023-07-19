using System;
using System.Collections.Generic;
using System.Text;

namespace WG3000_COMM.Core
{
    public class MjFaceTempl
    {
        private const int TemplSize = 0x8c9a;//27668;

        private byte[] _data;
        private int _uid;

        public MjFaceTempl()
        {
            _data = new byte[TemplSize];
            _uid = -1;
        }

        public MjFaceTempl(int uid, byte[] data)
        {
            _uid = uid;
            if (data != null)
            {
                _data = new byte[Length];
                Array.Copy(data, _data, Length);
            }
            else
            {
                _data = null;
            }
        }

        public MjFaceTempl(MjFaceTempl t)
        {
            _uid = t.uid;
            _data = new byte[Length];
            Array.Copy(t.data, _data, Length);
        }

        public byte[] data
        {
            get { return _data; }
            set { Array.Copy(value, _data, Length); }
        }

        public int uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public static int Length
        {
            get { return TemplSize; }
        }
    }
}
