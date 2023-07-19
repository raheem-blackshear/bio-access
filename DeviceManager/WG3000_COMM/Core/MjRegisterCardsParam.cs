namespace WG3000_COMM.Core
{
    using System;

    public class MjRegisterCardsParam
    {
        public uint bOrderInfreePrivilegePage;
        public uint deletedPrivilegeCount;
        public uint freeNewPrivilegePageAddr;
        public uint iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR;
        public uint[] maxPrivilegeOf4K = new uint[200];
        public uint[] minPrivilegeOf4K = new uint[200];
        public uint newPrivilegePage4KAddr;
        public uint totalPrivilegeCount;

        public void updateParam(byte[] wgpktData, int startIndex)
        {
            int num = startIndex;
            this.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            this.newPrivilegePage4KAddr = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            this.freeNewPrivilegePageAddr = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            this.bOrderInfreePrivilegePage = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            this.totalPrivilegeCount = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            this.deletedPrivilegeCount = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            uint num2 = BitConverter.ToUInt32(wgpktData, num);
            num += 4;
            uint num3 = BitConverter.ToUInt32(wgpktData, num);
            if (num3 >= 0xc8000)
            {
                num3 = 0xc8000;
            }
            for (uint i = num2; i < num3; i += 0x1000)
            {
                num += 4;
                if (num >= wgpktData.Length)
                {
                    return;
                }
                this.minPrivilegeOf4K[i >> 12] = BitConverter.ToUInt32(wgpktData, num);
                num += 4;
                if (num >= wgpktData.Length)
                {
                    return;
                }
                this.maxPrivilegeOf4K[i >> 12] = BitConverter.ToUInt32(wgpktData, num);
            }
        }

        public uint MaxPrivilegesNum
        {
            get
            {
                if (!wgTools.gADCT_internal)
                {
                    return 0xa7f8;
                }
                return 0x30d40;
            }
        }
    }
}

