using System;
using System.Collections.Generic;
using System.Text;

namespace WG3000_COMM.Core
{
    public class MjFpTempl
    {
        private const int TemplSize = 1404;
        private const int CompactTemplSize = 1000;

        private byte[] _data;
        private int _finger;
        private bool _duress;
        private int _uid;

        public MjFpTempl()
        {
            _data = new byte[TemplSize];
            _finger = 0;
            _duress = false;
            _uid = -1;
        }

        public MjFpTempl(int uid, int finger, byte[] data, bool duress)
        {
            _uid = uid;
            _finger = finger;
            if (data != null)
            {
                _data = new byte[Length];
                Array.Copy(data, _data, Length);
            }
            else
            {
                _data = null;
            }
            _duress = duress;
        }

        public MjFpTempl(MjFpTempl t)
        {
            _uid = t.uid;
            _finger = t.finger;
            _data = new byte[Length];
            Array.Copy(t.data, _data, Length);
            _duress = t.duress;
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

        public int finger
        {
            get { return _finger; }
            set { _finger = value; }
        }

        public bool duress
        {
            get { return _duress; }
            set { _duress = value; }
        }

        public static int Length
        {
            get { return TemplSize; }
        }

        public static int CompactLength
        {
            get { return CompactTemplSize; }
        }
    }
}
