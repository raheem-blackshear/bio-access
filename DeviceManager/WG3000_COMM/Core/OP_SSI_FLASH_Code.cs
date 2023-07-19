namespace WG3000_COMM.Core
{
    using System;

    public enum OP_SSI_FLASH_Code : byte
    {
        OP_END = 0xff,
        OP_READ = 0x10,
        OP_READ_FAIL = 0x12,
        OP_READ_OK = 0x11,
        OP_WRITE = 0x20,
        OP_WRITE_FAIL = 0x22,
        OP_WRITE_OK = 0x21,
        OP_WRITE_PARAM = 0x30,
        OP_WRITE_PARAM_FAIL = 50,
        OP_WRITE_PARAM_OK = 0x31
    }
}

