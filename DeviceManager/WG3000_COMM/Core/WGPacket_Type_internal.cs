namespace WG3000_COMM.Core
{
    using System;

    internal enum WGPacket_Type_internal : byte
    {
        OP_BASIC = 0x20,
        OP_SSI_FLASH = 0x21,
        OP_BATCH_SSI_FLASH = 0x22,
        OP_PRIVILEGE = 0x23,
        OP_CONFIG = 0x24,
        OP_CPU_CONFIG = 0x25,
        OP_FP_ENROLL = 0x26,
        OP_FACE_ENROLL = 0x27,
        OP_NONE = 0x28
    }
}

