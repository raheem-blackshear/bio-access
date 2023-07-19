namespace WG3000_COMM.Core
{
    using System;
    using System.Data;
    using System.Net;
    using System.Text;
    using System.Threading;

    public class wgMjControllerPrivilege : IDisposable
    {
        public bool bAllowUploadUserName;
        private static bool m_bStopDownloadPrivilege;
        private static bool m_bStopUploadPrivilege;
        private int m_priWaitMs = 1000;
        private wgUdpComm wgudp;
		
		// S8000 specific
        private readonly int m_uploadUserNameMaxlen = 0xe74;
        private readonly int m_userNamePageHalfStart = 0x1270;
        private readonly int m_userNamePageStart = 0x11f8;

        // A50 specific
        private const uint MaxChunkSize = 0x400; // Maximum size of a chunk
        private const uint OffsetInFlashQuery = 0x1c; // data offset in the packet
        private const uint OffsetInChunk = 0x14; // data offset in chunk(1024)

        private const uint FpEntryCountInChunk = MaxChunkSize / MjFpEntry.Length;
        private const uint FpEntryChunkSize = FpEntryCountInChunk * MjFpEntry.Length;

        private const uint FaceEntryCountInChunk = MaxChunkSize / MjFaceEntry.Length;
        private const uint FaceEntryChunkSize = FaceEntryCountInChunk * MjFaceEntry.Length;
        
        private const uint UserInfoCountInChunk = MaxChunkSize / MjUserInfo.Length;     // User Info count in a chunk
        private const uint UserInfoChunkSize = UserInfoCountInChunk * MjUserInfo.Length;// User Info chunk size

        //private const uint NameInfoCountInChunk = MaxChunkSize / MjNameInfo.Length;     // Name Info count in a chunk
        //private const uint NameInfoChunkSize = NameInfoCountInChunk * MjNameInfo.Length;// Name Info chunk size
        
        //private const uint NameCountInChunk = MaxChunkSize / MjNameInfo.UserNameLength;
        //private const uint NameChunkSize = MjNameInfo.UserNameLength * NameCountInChunk;// Name chunk size

        private bool m_bFpDoubleCheck = true;
        private MjFpTempl _templDoubled = null;

        private bool m_bFaceDoubleCheck = true;
        private MjFaceTempl _faceTemplDoubled = null;

        public int AddPrivilegeOfOneCardIP(int ControllerSN, string IP, int Port, MjRegisterCard mjrc)
        {
            int num = -1;
            WGPacketPrivilegeS8000 privilege = new WGPacketPrivilegeS8000(mjrc);
            privilege.type = 0x23;
            privilege.code = 0x20;
            privilege.iDevSnFrom = 0;
            if (ControllerSN == -1)
            {
                privilege.iDevSnTo = uint.MaxValue;
            }
            else
            {
                privilege.iDevSnTo = (uint)ControllerSN;
            }
            privilege.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = privilege.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                return num;
            }
            int num2 = this.wgudp.udp_get(cmd, this.m_priWaitMs, privilege.xid, IP, Port, ref recv);
            if (recv != null)
            {
                return num2;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return num;
        }

        public void AllowDownload()
        {
            m_bStopDownloadPrivilege = false;
        }

        public void AllowUpload()
        {
            m_bStopUploadPrivilege = false;
        }

        public int ClearAllPrivilegeIP(int ControllerSN, string IP, int Port)
        {
            byte[] recv = null;
            int ret = -1;
            if (wgMjController.isS8000DC(ControllerSN))
            {
                MjRegisterCardsParam param = new MjRegisterCardsParam();
                int num2 = 0;
                uint num3 = 0;
                param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR = num3;
                param.newPrivilegePage4KAddr = num3 + ((uint)(((num2 >> 9) * 4) * 0x400));
                param.freeNewPrivilegePageAddr = (param.newPrivilegePage4KAddr + 0x1000) + ((uint)((num2 % 0x200) * 8));
                param.bOrderInfreePrivilegePage = 1;
                param.totalPrivilegeCount = (uint)num2;
                param.deletedPrivilegeCount = 0;
                WGPacketWith1024 with = new WGPacketWith1024();
                with.type = 0x23;
                with.code = 0x30;
                with.iDevSnFrom = 0;
                with.iDevSnTo = (uint)ControllerSN;
                with.iCallReturn = 0;
                int destinationIndex = 0;
                Array.Copy(BitConverter.GetBytes(param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(param.newPrivilegePage4KAddr), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(param.freeNewPrivilegePageAddr), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(param.bOrderInfreePrivilegePage), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(param.totalPrivilegeCount), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(param.deletedPrivilegeCount), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                if (num2 > 0x16800)
                {
                    Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, with.ucData, destinationIndex, 4);
                    destinationIndex += 4;
                    Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, with.ucData, destinationIndex, 4);
                    destinationIndex += 4;
                }
                else if (param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR == 0)
                {
                    Array.Copy(BitConverter.GetBytes(0xc8000), 0, with.ucData, destinationIndex, 4);
                    destinationIndex += 4;
                    Array.Copy(BitConverter.GetBytes(0xc9000), 0, with.ucData, destinationIndex, 4);
                    destinationIndex += 4;
                }
                else
                {
                    Array.Copy(BitConverter.GetBytes(0), 0, with.ucData, destinationIndex, 4);
                    destinationIndex += 4;
                    Array.Copy(BitConverter.GetBytes(0x1000), 0, with.ucData, destinationIndex, 4);
                    destinationIndex += 4;
                }
                if (this.wgudp == null)
                {
                    this.wgudp = new wgUdpComm();
                    Thread.Sleep(300);
                }
                byte[] cmd = with.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                {
                    wgTools.WriteLine("出错");
                    return -1;
                }
                ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, with.xid, IP, Port, ref recv);
                if (recv == null)
                    return -1;
            }
            else
            {
                WGPacket clearPacket = new WGPacket(0x23, 0x80, (uint)ControllerSN);
                if (this.wgudp == null)
                {
                    this.wgudp = new wgUdpComm();
                    Thread.Sleep(300);
                }
                byte[] cmd = clearPacket.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                {
                    wgTools.WriteLine("出错");
                    return -1;
                }
                /* TODOJ wait time must be reevaluated */
                ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, clearPacket.xid, IP, Port, ref recv);
                if (recv == null)
                    return -1;
            }
            return ret;
        }

        /* Remove a user whose id is UserID from the terminal */
        public int DelPrivilegeOfOneCardIP(int ControllerSN, string IP, int Port, uint userID)
        {
            int ret = -1;
            if (wgMjController.isS8000DC(ControllerSN))
            {
                MjRegisterCard mj = new MjRegisterCard();
                mj.CardID = userID;//S8000: userID contains CardID
                mj.IsDeleted = true;
                WGPacketPrivilegeS8000 privilege = new WGPacketPrivilegeS8000(mj);
                privilege.type = 0x23;
                privilege.code = 0x20;
                privilege.iDevSnFrom = 0;
                privilege.iDevSnTo = (uint)ControllerSN;
                privilege.iCallReturn = 0;
                if (this.wgudp == null)
                {
                    this.wgudp = new wgUdpComm();
                    Thread.Sleep(300);
                }
                byte[] recv = null;
                byte[] cmd = privilege.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                {
                    wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                    return -1;
                }
                ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, privilege.xid, IP, Port, ref recv);
                if (recv != null)
                    return ret;
                
                wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            }
            else
            {
                MjUserInfo user = new MjUserInfo();
                user.userID = userID;
                user.IsDeleted = true;
                WGPacketPrivilege privilege = new WGPacketPrivilege(0x23, 0x20, (uint)ControllerSN, user);
                if (this.wgudp == null)
                {
                    this.wgudp = new wgUdpComm();
                    Thread.Sleep(300);
                }
                byte[] cmd = privilege.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                {
                    wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                    return -1;
                }
                byte[] recv = null;
                ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, privilege.xid, IP, Port, ref recv);
                if (recv == null)
                {
                    wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                    return -1;
                }
            }
            return ret;
        }
        
        protected virtual void DisplayProcessInfo(string info, int infoCode, int specialInfo)
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (this.wgudp != null))
            {
                this.wgudp.Close();
                this.wgudp.Dispose();
            }
        }

        public static void StopDownload()
        {
            m_bStopDownloadPrivilege = true;
        }

        public static void StopUpload()
        {
            m_bStopUploadPrivilege = true;
        }

        public static bool bStopDownloadPrivilege
        {
            get
            {
                return m_bStopDownloadPrivilege;
            }
        }

        public static bool bStopUploadPrivilege
        {
            get
            {
                return m_bStopUploadPrivilege;
            }
        }

        #region S8000 specific region
        public int DownloadIP(int ControllerSN, string IP, int Port, string doorName, ref DataTable dtPrivilege)
        {
            return this.DownloadIP_internal(ControllerSN, IP, Port, doorName, ref dtPrivilege, "");
        }

        public int DownloadIP(int ControllerSN, string IP, int Port, string doorName, ref DataTable dtPrivilege, string PCIPAddr)
        {
            return this.DownloadIP_internal(ControllerSN, IP, Port, doorName, ref dtPrivilege, PCIPAddr);
        }

        internal int DownloadIP_internal(int ControllerSN, string IP, int Port, string doorName, ref DataTable dtPrivilege, string PCIPAddr)
        {
            if (m_bStopDownloadPrivilege)
                return -1;
            
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
                flag = true;
            if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
                flag2 = true;
            if (dtPrivilege.Columns.Contains("f_IsDeleted"))
                flag3 = true;
            else
                dtPrivilege.Columns.Add("f_IsDeleted", Type.GetType("System.UInt32"));
            if (dtPrivilege.Columns.Contains("f_ConsumerName"))
                flag4 = true;
            wgTools.WriteLine("Download Privilege Start");
            WGPacket packet = new WGPacket();
            packet.type = 0x23;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint)ControllerSN;
            packet.iCallReturn = 0;
            if (this.wgudp == null)
            {
                if (PCIPAddr == null)
                {
                    this.wgudp = new wgUdpComm();
                }
                else
                {
                    IPAddress address;
                    if (IPAddress.TryParse(PCIPAddr, out address))
                    {
                        this.wgudp = new wgUdpComm(IPAddress.Parse(PCIPAddr));
                    }
                    else
                    {
                        this.wgudp = new wgUdpComm();
                    }
                }
                Thread.Sleep(300);
            }
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
                return -1;
            
            byte[] recv = null;
            int num = this.wgudp.udp_get(cmd, 1000, packet.xid, IP, Port, ref recv);
            MjRegisterCardsParam param = new MjRegisterCardsParam();
            if (recv == null)
                return -1;
            
            param.updateParam(recv, 20);
            long num3 = 0L;
            WGPacketSSI_FLASH_QUERY_internal _internal = new WGPacketSSI_FLASH_QUERY_internal();
            _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint)ControllerSN, (uint)num3, (uint)((num3 + 0x400L) - 1L));
            long num2 = 0L;
            while (num2 < (param.totalPrivilegeCount - param.deletedPrivilegeCount))
            {
                if (((param.newPrivilegePage4KAddr - param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) / 8) > num2)
                    num3 = param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR + (num2 * 8L);
                else
                    num3 = (long)((param.newPrivilegePage4KAddr + 0x1000) + ((num2 - ((param.newPrivilegePage4KAddr - param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) / 8)) * 8));
                _internal.iStartFlashAddr = (uint)num3;
                _internal.iEndFlashAddr = (uint)((num3 + 0x400L) - 1L);
                byte[] buffer3 = _internal.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                    return -1;
                
                byte[] buffer4 = null;
                if (this.wgudp.udp_get(buffer3, 1000, _internal.xid, IP, Port, ref buffer4) < 0)
                    return -1;
                
                MjRegisterCard card = new MjRegisterCard();
                byte[] destinationArray = new byte[0x18];
                if (buffer4 != null)
                    Array.Copy(buffer4, 0x1c, destinationArray, 0, 8);
                num3 = (num3 * 2L) + 0x194000L;
                _internal.iStartFlashAddr = (uint)num3;
                _internal.iEndFlashAddr = (uint)((num3 + 0x400L) - 1L);
                buffer3 = _internal.ToBytes(this.wgudp.udpPort);
                if (buffer3 == null)
                    return -1;
                
                byte[] buffer6 = null;
                if (this.wgudp.udp_get(buffer3, 1000, _internal.xid, null, 0xea60, ref buffer6) < 0)
                    return -1;
                
                if (buffer6 != null)
                {
                    for (int i = 0; i < 0x40; i++)
                    {
                        Array.Copy(buffer4, 0x1c + (i * 8), destinationArray, 0, 8);
                        Array.Copy(buffer6, 0x1c + (i * 0x10), destinationArray, 8, 0x10);
                        int count = dtPrivilege.Rows.Count;
                        if (card.Update(destinationArray, (uint)(num2 + i)) <= 0)
                            break;
                        
                        if (bStopDownloadPrivilege)
                            return -100002;
                        
                        DataRow row = dtPrivilege.NewRow();
                        row["f_CardNO"] = card.CardID;
                        row["f_ConsumerNO"] = card.userID;
                        row["f_PIN"] = card.Password.ToString();
                        row["f_BeginYMD"] = card.ymdStart;
                        row["f_EndYMD"] = card.ymdEnd;
                        row["f_ControlSegID1"] = card.ControlSegIndexGet(1);
                        row["f_ControlSegID2"] = card.ControlSegIndexGet(2);
                        row["f_ControlSegID3"] = card.ControlSegIndexGet(3);
                        row["f_ControlSegID4"] = card.ControlSegIndexGet(4);
                        if (flag)
                        {
                            row["f_DoorFirstCard_1"] = card.FirstCardGet(1);
                            row["f_DoorFirstCard_2"] = card.FirstCardGet(2);
                            row["f_DoorFirstCard_3"] = card.FirstCardGet(3);
                            row["f_DoorFirstCard_4"] = card.FirstCardGet(4);
                        }
                        if (flag2)
                        {
                            row["f_MoreCards_GrpID_1"] = card.MoreCardGroupIndexGet(1);
                            row["f_MoreCards_GrpID_2"] = card.MoreCardGroupIndexGet(2);
                            row["f_MoreCards_GrpID_3"] = card.MoreCardGroupIndexGet(3);
                            row["f_MoreCards_GrpID_4"] = card.MoreCardGroupIndexGet(4);
                        }
                        row["f_IsDeleted"] = card.IsDeleted ? 1 : 0;
                        dtPrivilege.Rows.Add(row);
                    }
                    dtPrivilege.AcceptChanges();
                }
                num2 += 0x40L;
                if (m_bStopDownloadPrivilege)
                    return -1;
            }
            if (param.totalPrivilegeCount > this.m_uploadUserNameMaxlen)
                flag4 = false;
            if (flag4)
            {
                int userNamePageStart = this.m_userNamePageStart;
                _internal.iStartFlashAddr = (uint)(userNamePageStart * 0x400);
                _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                if (param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR > 0)
                {
                    _internal.iStartFlashAddr = (uint)(this.m_userNamePageHalfStart * 0x400);
                }
                int num6 = 0;
                while (_internal.iStartFlashAddr <= 0x4b9fff)
                {
                    if (this.wgudp.udp_get(_internal.ToBytes(this.wgudp.udpPort), 1000, _internal.xid, IP, Port, ref recv) < 0)
                    {
                        return -1;
                    }
                    if (recv != null)
                    {
                        for (int j = 0; j < 0x400; j += 0x20)
                        {
                            byte[] bytes = new byte[0x21];
                            int num8 = 0;
                            bytes[0x20] = 0;
                            for (int k = 0; k < 0x20; k++)
                            {
                                bytes[k] = recv[(0x1c + j) + k];
                                if (bytes[k] == 0)
                                {
                                    break;
                                }
                                num8++;
                            }
                            if (((bytes[0] != 0xff) || (bytes[1] != 0xff)) && (bytes[0] != 0))
                            {
                                if (num6 >= dtPrivilege.Rows.Count)
                                {
                                    break;
                                }
                                string str = "";
                                str = Encoding.GetEncoding("utf-8").GetString(bytes, 0, num8);
                                dtPrivilege.Rows[num6]["f_ConsumerName"] = str;
                            }
                            num6++;
                        }
                    }
                    else
                    {
                        num6 += 0x20;
                    }
                    if (num6 >= dtPrivilege.Rows.Count)
                    {
                        break;
                    }
                    _internal.iStartFlashAddr += 0x400;
                }
                dtPrivilege.AcceptChanges();
            }
            DataView view = new DataView(dtPrivilege);
            view.RowFilter = "f_IsDeleted = 1";
            if (view.Count > 0)
            {
                for (int m = view.Count - 1; m >= 0; m--)
                {
                    view.Delete(m);
                }
            }
            dtPrivilege.AcceptChanges();
            if (!flag3)
            {
                dtPrivilege.Columns.Remove("f_IsDeleted");
                dtPrivilege.AcceptChanges();
            }
            return 1;
        }

        public int UploadIP(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege)
        {
            return this.UploadIP_internal(ControllerSN, IP, Port, doorName, dtPrivilege, "");
        }

        public int UploadIP(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege, string PCIPAddr)
        {
            return this.UploadIP_internal(ControllerSN, IP, Port, doorName, dtPrivilege, PCIPAddr);
        }

        internal int UploadIP_internal(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege, string PCIPAddr)
        {
            if (bStopUploadPrivilege)
                return -100002;
            
            if (dtPrivilege.Rows.Count >= 2)
            {
                uint num = 0;
                for (int k = 0; k < (dtPrivilege.Rows.Count - 1); k++)
                {
                    if (((uint)dtPrivilege.Rows[k]["f_CardNO"]) <= num)
                        return -200;
                    
                    num = (uint)dtPrivilege.Rows[k]["f_CardNO"];
                }
            }
            int num3 = -201;
            bool flag = false;
            bool flag2 = false;
            if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
                flag = true;
            if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
                flag2 = true;
            bool bAllowUploadUserName = true;
            bAllowUploadUserName = this.bAllowUploadUserName;
            if (dtPrivilege.Rows.Count > this.m_uploadUserNameMaxlen)
                bAllowUploadUserName = false;
            if (!dtPrivilege.Columns.Contains("f_ConsumerName"))
                bAllowUploadUserName = false;
            wgTools.WriteLine("upload Start");
            SIIFlash flash = new SIIFlash();
            WGPacket packet = new WGPacket();
            packet.type = 0x23;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint)ControllerSN;
            packet.iCallReturn = 0;
            int num4 = -1;
            if (this.wgudp == null)
            {
                if (PCIPAddr == null)
                {
                    this.wgudp = new wgUdpComm();
                }
                else
                {
                    IPAddress address;
                    if (IPAddress.TryParse(PCIPAddr, out address))
                    {
                        this.wgudp = new wgUdpComm(IPAddress.Parse(PCIPAddr));
                    }
                    else
                    {
                        this.wgudp = new wgUdpComm();
                    }
                }
                Thread.Sleep(300);
            }
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
                return -202;

            byte[] recv = null;
            int num5 = 3;
            int millisecondsTimeout = 300;
            for (int i = 0; i < num5; i++)
            {
                num4 = this.wgudp.udp_get(cmd, 1000, packet.xid, IP, Port, ref recv);
                if (recv != null)
                    break;
                Thread.Sleep(millisecondsTimeout);
            }
            if (recv == null)
                return -203;
            
            MjRegisterCardsParam param = new MjRegisterCardsParam();
            if (recv == null)
                return -207;
            
            param.updateParam(recv, 20);
            string str = "";
            str = ((str + string.Format("权限起始页 = 0x{0:X8}\r\n", param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) + string.Format("存储新有序权限的4K的页面 = 0x{0:X8}\r\n", param.newPrivilegePage4KAddr)) + string.Format("自由的记录页面(无序的) = 0x{0:X8}\r\n", param.freeNewPrivilegePageAddr) + string.Format("是否有序的(自由页面中) = {0:d}\r\n", param.bOrderInfreePrivilegePage)) + string.Format("总权限数 = {0:d}\r\n", param.totalPrivilegeCount) + string.Format("已删除的权限数 = {0:d}\r\n", param.deletedPrivilegeCount);
            if (bStopUploadPrivilege)
                return -100002;
            
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            int num8 = 0;
            uint num9 = 0;
            if (param.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR == 0)
            {
                num9 = 0xc8000;
            }
            if (param.totalPrivilegeCount == 0)
            {
                num9 = 0;
            }
            int count = dtPrivilege.Rows.Count;
            if (count > param.MaxPrivilegesNum)
            {
                this.DisplayProcessInfo(doorName, -100001, count);
                return -100001;
            }
            if (count >= 0x16800)
            {
                num9 = 0;
                num4 = this.ClearAllPrivilegeIP(ControllerSN, IP, Port);
                if (num4 < 0)
                {
                    wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed clearAllPrivilegeIP ret= " + num4.ToString(), new object[0]);
                    return num4;
                }
            }
            MjRegisterCardsParam param2 = new MjRegisterCardsParam();
            param2.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR = num9;
            param2.newPrivilegePage4KAddr = num9 + ((uint)(((count >> 9) * 4) * 0x400));
            param2.freeNewPrivilegePageAddr = (param2.newPrivilegePage4KAddr + 0x1000) + ((uint)((count % 0x200) * 8));
            param2.bOrderInfreePrivilegePage = 1;
            param2.totalPrivilegeCount = (uint)count;
            param2.deletedPrivilegeCount = 0;
            if (count == 0)
            {
                num3 = 0;
            }
            else
            {
                WGPacketSSI_FLASH_internal _internal = new WGPacketSSI_FLASH_internal();
                _internal.type = 0x21;
                _internal.code = 0x20;
                _internal.iDevSnFrom = 0;
                _internal.iDevSnTo = (uint)ControllerSN;
                _internal.iCallReturn = 0;
                _internal.ucData = new byte[0x400];
                this.DisplayProcessInfo(doorName, 0x186a1, 0);
                _internal.iStartFlashAddr = (uint)num8;
                _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                for (int m = 0; m < 0x400; m++)
                {
                    _internal.ucData[m] = 0xff;
                }
                MjRegisterCard card = new MjRegisterCard();
                card.CardID = 0x6d64;
                long sourceIndex = num9;
                long num13 = sourceIndex;
                long num14 = 0x194000L;
                long num15 = num14 + (num9 * 2);
                long num16 = num15;
                long num17 = (sourceIndex == 0L) ? ((long)(this.m_userNamePageStart * 0x400)) : ((long)(this.m_userNamePageHalfStart * 0x400));
                long num18 = num17;
                int num19 = 0;
                while ((num19 & 0xffffe00) < (count & 0xffffe00))
                {
                    if (bStopUploadPrivilege)
                        return -100002;
                    
                    card.userID = (uint)dtPrivilege.Rows[num19]["f_ConsumerNO"];
                    card.CardID = (uint)dtPrivilege.Rows[num19]["f_CardNO"];
                    card.Password = uint.Parse(dtPrivilege.Rows[num19]["f_PIN"].ToString());
                    card.ymdStart = (DateTime)dtPrivilege.Rows[num19]["f_BeginYMD"];
                    card.ymdEnd = (DateTime)dtPrivilege.Rows[num19]["f_EndYMD"];
                    card.ControlSegIndexSet(1, (byte)dtPrivilege.Rows[num19]["f_ControlSegID1"]);
                    card.ControlSegIndexSet(2, (byte)dtPrivilege.Rows[num19]["f_ControlSegID2"]);
                    card.ControlSegIndexSet(3, (byte)dtPrivilege.Rows[num19]["f_ControlSegID3"]);
                    card.ControlSegIndexSet(4, (byte)dtPrivilege.Rows[num19]["f_ControlSegID4"]);
                    if (flag)
                    {
                        card.FirstCardSet(1, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_1"]) > 0);
                        card.FirstCardSet(2, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_2"]) > 0);
                        card.FirstCardSet(3, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_3"]) > 0);
                        card.FirstCardSet(4, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_4"]) > 0);
                    }
                    if (flag2)
                    {
                        card.MoreCardGroupIndexSet(1, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_1"]);
                        card.MoreCardGroupIndexSet(2, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_2"]);
                        card.MoreCardGroupIndexSet(3, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_3"]);
                        card.MoreCardGroupIndexSet(4, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_4"]);
                    }
                    if (wgMjController.IsElevator_Internal(ControllerSN))
                    {
                        card.AllowFloors_internal = (ulong)dtPrivilege.Rows[num19]["f_AllowFloors"];
                    }
                    Array.Copy(card.ToBytes(), 4L, flash.data, num13, 8L);
                    num13 += 8L;
                    Array.Copy(card.ToBytes(), 12L, flash.data, num16, 0x10L);
                    num16 += 0x10L;
                    if (bAllowUploadUserName)
                    {
                        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dtPrivilege.Rows[num19]["f_ConsumerName"]));
                        for (int n = 0; (n < 0x20) && (n < bytes.Length); n++)
                        {
                            flash.data[(int)((IntPtr)(n + num18))] = bytes[n];
                        }
                        if ((bytes.Length > 0) && (bytes.Length < 0x20))
                        {
                            flash.data[(int)((IntPtr)(bytes.Length + num18))] = 0;
                        }
                        num18 += 0x20L;
                    }
                    num19++;
                }
                long num22 = 0x1000L + num13;
                long num24 = 0x2000L + num16;
                while (num19 < count)
                {
                    if (bStopUploadPrivilege)
                    {
                        return -100002;
                    }
                    card.CardID = (uint)dtPrivilege.Rows[num19]["f_CardNO"];
                    card.userID = (uint)dtPrivilege.Rows[num19]["f_ConsumerNO"];
                    card.Password = uint.Parse(dtPrivilege.Rows[num19]["f_PIN"].ToString());
                    card.ymdStart = (DateTime)dtPrivilege.Rows[num19]["f_BeginYMD"];
                    card.ymdEnd = (DateTime)dtPrivilege.Rows[num19]["f_EndYMD"];
                    card.ControlSegIndexSet(1, (byte)dtPrivilege.Rows[num19]["f_ControlSegID1"]);
                    card.ControlSegIndexSet(2, (byte)dtPrivilege.Rows[num19]["f_ControlSegID2"]);
                    card.ControlSegIndexSet(3, (byte)dtPrivilege.Rows[num19]["f_ControlSegID3"]);
                    card.ControlSegIndexSet(4, (byte)dtPrivilege.Rows[num19]["f_ControlSegID4"]);
                    if (flag)
                    {
                        card.FirstCardSet(1, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_1"]) > 0);
                        card.FirstCardSet(2, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_2"]) > 0);
                        card.FirstCardSet(3, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_3"]) > 0);
                        card.FirstCardSet(4, ((byte)dtPrivilege.Rows[num19]["f_DoorFirstCard_4"]) > 0);
                    }
                    if (flag2)
                    {
                        card.MoreCardGroupIndexSet(1, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_1"]);
                        card.MoreCardGroupIndexSet(2, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_2"]);
                        card.MoreCardGroupIndexSet(3, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_3"]);
                        card.MoreCardGroupIndexSet(4, (byte)dtPrivilege.Rows[num19]["f_MoreCards_GrpID_4"]);
                    }
                    if (wgMjController.IsElevator_Internal(ControllerSN))
                    {
                        card.AllowFloors_internal = (ulong)dtPrivilege.Rows[num19]["f_AllowFloors"];
                    }
                    Array.Copy(card.ToBytes(), 4L, flash.data, num22, 8L);
                    num22 += 8L;
                    Array.Copy(card.ToBytes(), 12L, flash.data, num24, 0x10L);
                    num24 += 0x10L;
                    if (bAllowUploadUserName)
                    {
                        byte[] buffer4 = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dtPrivilege.Rows[num19]["f_ConsumerName"]));
                        for (int num25 = 0; (num25 < 0x20) && (num25 < buffer4.Length); num25++)
                        {
                            flash.data[(int)((IntPtr)(num25 + num18))] = buffer4[num25];
                        }
                        if ((buffer4.Length > 0) && (buffer4.Length < 0x20))
                        {
                            flash.data[(int)((IntPtr)(buffer4.Length + num18))] = 0;
                        }
                        num18 += 0x20L;
                    }
                    num19++;
                }
                wgTools.WriteLine("下传卡号[4K+自由区] Start");
                int specialInfo = 0;
                cmd = null;
                while (sourceIndex < (num22 + 0x1000L))
                {
                    if (bStopUploadPrivilege)
                        return -100002;
                    _internal.iStartFlashAddr = (uint)sourceIndex;
                    _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                    Array.Copy(flash.data, sourceIndex, _internal.ucData, 0L, 0x400L);
                    for (int num27 = 0; num27 < num5; num27++)
                    {
                        if (cmd != null)
                        {
                            _internal.GetNewXid();
                        }
                        cmd = _internal.ToBytes(this.wgudp.udpPort);
                        wgTools.WriteLine(string.Format("{0:d}[{0:X4}]  {1:X8}", sourceIndex, ((cmd[0x1c] + (cmd[0x1d] << 8)) + (cmd[30] << 0x10)) + (cmd[0x1f] << 0x18)));
                        num4 = this.wgudp.udp_get(cmd, this.m_priWaitMs, _internal.xid, IP, Port, ref recv);
                        if (recv != null)
                        {
                            bool flag4 = true;
                            for (int num28 = 0; (num28 < _internal.ucData.Length) && ((0x1c + num28) < recv.Length); num28++)
                            {
                                if (_internal.ucData[num28] != recv[0x1c + num28])
                                {
                                    wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed wgpktWrite.ucData[i]!=recv[28+i] startAdr4KCardID = " + sourceIndex.ToString(), new object[0]);
                                    flag4 = false;
                                    break;
                                }
                            }
                            if (flag4)
                            {
                                break;
                            }
                        }
                        wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg startAdr4KCardID = " + sourceIndex.ToString() + ", tries = " + num27.ToString() + ", m_priWaitMs = " + this.m_priWaitMs.ToString(), new object[0]);
                        Thread.Sleep(millisecondsTimeout);
                    }
                    if (recv == null)
                    {
                        wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed startAdr4KCardID = " + sourceIndex.ToString(), new object[0]);
                        return -203;
                    }
                    sourceIndex += 0x400L;
                    specialInfo += 0x40;
                    if (specialInfo > count)
                    {
                        specialInfo = count;
                    }
                    this.DisplayProcessInfo(doorName, 0x186a2, specialInfo);
                }
                if (bAllowUploadUserName)
                {
                    while (num17 < (num18 + 0x1000L))
                    {
                        if (bStopUploadPrivilege)
                        {
                            return -100002;
                        }
                        _internal.iStartFlashAddr = (uint)num17;
                        _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                        Array.Copy(flash.data, num17, _internal.ucData, 0L, 0x400L);
                        for (int num29 = 0; num29 < num5; num29++)
                        {
                            if (cmd != null)
                            {
                                _internal.GetNewXid();
                            }
                            cmd = _internal.ToBytes(this.wgudp.udpPort);
                            wgTools.WriteLine(string.Format("{0:d}[{0:X4}]  {1:X8}", num17, ((cmd[0x1c] + (cmd[0x1d] << 8)) + (cmd[30] << 0x10)) + (cmd[0x1f] << 0x18)));
                            num4 = this.wgudp.udp_get(cmd, this.m_priWaitMs, _internal.xid, IP, Port, ref recv);
                            if (recv != null)
                            {
                                bool flag5 = true;
                                for (int num30 = 0; (num30 < _internal.ucData.Length) && ((0x1c + num30) < recv.Length); num30++)
                                {
                                    if (_internal.ucData[num30] != recv[0x1c + num30])
                                    {
                                        wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg UserName Failed wgpktWrite.ucData[i]!=recv[28+i] startAdr4KCardIDUserName = " + num17.ToString(), new object[0]);
                                        flag5 = false;
                                        break;
                                    }
                                }
                                if (flag5)
                                {
                                    break;
                                }
                            }
                            wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg UserName startAdr4KCardIDUserName = " + num17.ToString() + ", tries = " + num29.ToString() + ", m_priWaitMs = " + this.m_priWaitMs.ToString(), new object[0]);
                            Thread.Sleep(millisecondsTimeout);
                        }
                        if (recv == null)
                        {
                            wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg UserName Failed startAdr4KCardIDUserName = " + num17.ToString(), new object[0]);
                            return -203;
                        }
                        num17 += 0x400L;
                        specialInfo += 8;
                        if (specialInfo > count)
                        {
                            specialInfo = count;
                        }
                        this.DisplayProcessInfo(doorName, 0x186a2, specialInfo);
                    }
                }
                wgTools.WriteLine("下传卡信息区 Start");
                while (num15 < (num24 + 0x1000L))
                {
                    if (bStopUploadPrivilege)
                    {
                        return -100002;
                    }
                    _internal.iStartFlashAddr = (uint)num15;
                    _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                    Array.Copy(flash.data, num15, _internal.ucData, 0L, 0x400L);
                    for (int num31 = 0; num31 < num5; num31++)
                    {
                        if (cmd != null)
                        {
                            _internal.GetNewXid();
                        }
                        cmd = _internal.ToBytes(this.wgudp.udpPort);
                        wgTools.WriteLine(string.Format("startAdr4KCardIDInfo {0:d}[{0:X4}]", num15));
                        num4 = this.wgudp.udp_get(cmd, this.m_priWaitMs, _internal.xid, IP, Port, ref recv);
                        if (recv != null)
                        {
                            bool flag6 = true;
                            for (int num32 = 0; (num32 < _internal.ucData.Length) && ((0x1c + num32) < recv.Length); num32++)
                            {
                                if (_internal.ucData[num32] != recv[0x1c + num32])
                                {
                                    wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed wgpktWrite.ucData[i]!=recv[28+i] startAdr4KCardID = " + sourceIndex.ToString(), new object[0]);
                                    flag6 = false;
                                    break;
                                }
                            }
                            if (flag6)
                            {
                                break;
                            }
                        }
                        wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg startAdr4KCardIDInfo = " + num15.ToString() + ", tries = " + num31.ToString() + ", m_priWaitMs = " + this.m_priWaitMs.ToString(), new object[0]);
                        Thread.Sleep(millisecondsTimeout);
                    }
                    if (recv == null)
                    {
                        wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed startAdr4KCardIDInfo = " + num15.ToString(), new object[0]);
                        return -203;
                    }
                    num15 += 0x400L;
                    specialInfo += 0x20;
                    if (specialInfo > count)
                    {
                        specialInfo = count;
                    }
                    this.DisplayProcessInfo(doorName, 0x186a2, specialInfo);
                }
            }
            wgTools.WriteLine(string.Format("{0:d}权限上传完成", count));
            WGPacketWith1024 with = new WGPacketWith1024();
            with.type = 0x23;
            with.code = 0x30;
            with.iDevSnFrom = 0;
            with.iDevSnTo = (uint)ControllerSN;
            with.iCallReturn = 0;
            int destinationIndex = 0;
            Array.Copy(BitConverter.GetBytes(param2.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR), 0, with.ucData, destinationIndex, 4);
            destinationIndex += 4;
            Array.Copy(BitConverter.GetBytes(param2.newPrivilegePage4KAddr), 0, with.ucData, destinationIndex, 4);
            destinationIndex += 4;
            Array.Copy(BitConverter.GetBytes(param2.freeNewPrivilegePageAddr), 0, with.ucData, destinationIndex, 4);
            destinationIndex += 4;
            Array.Copy(BitConverter.GetBytes(param2.bOrderInfreePrivilegePage), 0, with.ucData, destinationIndex, 4);
            destinationIndex += 4;
            Array.Copy(BitConverter.GetBytes(param2.totalPrivilegeCount), 0, with.ucData, destinationIndex, 4);
            destinationIndex += 4;
            Array.Copy(BitConverter.GetBytes(param2.deletedPrivilegeCount), 0, with.ucData, destinationIndex, 4);
            destinationIndex += 4;
            if (count > 0x16800)
            {
                Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
            }
            else if (param2.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR == 0)
            {
                Array.Copy(BitConverter.GetBytes(0xc8000), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(0xc9000), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
            }
            else
            {
                Array.Copy(BitConverter.GetBytes(0), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
                Array.Copy(BitConverter.GetBytes(0x1000), 0, with.ucData, destinationIndex, 4);
                destinationIndex += 4;
            }
            cmd = with.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("出错");
                return -206;
            }
            for (int j = 0; j < num5; j++)
            {
                num4 = this.wgudp.udp_get(cmd, this.m_priWaitMs, with.xid, IP, Port, ref recv);
                if (recv != null)
                {
                    break;
                }
                wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg wgparam , tries = " + j.ToString() + ", m_priWaitMs = " + this.m_priWaitMs.ToString(), new object[0]);
                Thread.Sleep(millisecondsTimeout);
            }
            if (recv == null)
            {
                wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed wgparam ", new object[0]);
                return -203;
            }
            wgTools.WriteLine("权限表更新完成");
            this.DisplayProcessInfo(doorName, 0x186a3, count);
            num3 = count;
            wgTools.WriteLine("upload End");
            return num3;
        }
		#endregion

        #region A30, A50 specific region
        public int AddPrivilegeOfOneCardIP(int ControllerSN, string IP, int Port, MjUserInfo user)
        {
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            /* Add a user */
            WGPacketPrivilege userPacket = new WGPacketPrivilege(0x23, 0x20,
                (ControllerSN == -1) ? uint.MaxValue : (uint)ControllerSN, user);
            byte[] cmd = userPacket.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                return -1;
            }
            byte[] recv = null;
            int ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, userPacket.xid, IP, Port, ref recv);
            if (recv == null)
            {
                wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                return -1;
            }

            /* Send the fingerprint templates */
            if (wgMjController.isA30AC(ControllerSN) || wgMjController.isA50AC(ControllerSN))
            {
                WGPacketFpTempl fpPacket = new WGPacketFpTempl(0x23, 0x50,
                    (ControllerSN == -1) ? uint.MaxValue : (uint)ControllerSN, user.userID, 0, 0, false);
                WGPacketNameQuery res = new WGPacketNameQuery();
                int errCode;
                foreach (MjFpTempl templ in user.fpTemplList)
                {
                    fpPacket.index = (ushort)templ.finger;
                    /* 1404 bytes is splitted into 2 parts as 1024+380 */
                    for (byte i = 0; i < 2; i++)
                    {
                        fpPacket.tag = i;
                        fpPacket.GetNewXid();
                        if (i == 0)
                            Array.Copy(templ.data, 0,
                                fpPacket.data, 0, fpPacket.data.Length);
                        else
                            Array.Copy(templ.data, fpPacket.data.Length,
                                fpPacket.data, 0, templ.data.Length - fpPacket.data.Length);
                        if (this.wgudp == null)
                        {
                            this.wgudp = new wgUdpComm();
                            Thread.Sleep(300);
                        }
                        fpPacket.GetNewXid();
                        cmd = fpPacket.ToBytes(this.wgudp.udpPort);
                        if (cmd == null)
                        {
                            wgTools.WriteLine("出错");
                            return -1;
                        }
                        ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, fpPacket.xid, IP, Port, ref recv);
                        if (recv == null || !res.get(recv, 0x14))
                            return -1;

                        errCode = BitConverter.ToInt32(BitConverter.GetBytes(res.tag), 0);
                        switch (errCode)
                        {
                            case wgTools.ErrorCode.ERR_DB_IS_FULL:
                            case wgTools.ErrorCode.ERR_FAIL:
                                return errCode;
                            case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                                _templDoubled = new MjFpTempl(templ);
                                return errCode;
                        }
                    }
                }
            }

            if (wgMjController.isF300AC(ControllerSN))
            {
                WGPacketFaceTempl facePacket = new WGPacketFaceTempl(0x23, 0x90,
                    (ControllerSN == -1) ? uint.MaxValue : (uint)ControllerSN, user.userID, 0, false);
                WGPacketNameQuery res = new WGPacketNameQuery();
                int errCode;
                MjFaceTempl faceTempl = user.faceTempl;
                for (byte i = 0; i <= MjFaceTempl.Length / facePacket.data.Length; i++)
                {
                    facePacket.tag = i;
                    facePacket.GetNewXid();
                    if (i != MjFaceTempl.Length / facePacket.data.Length)
                        Array.Copy(faceTempl.data, i * facePacket.data.Length,
                            facePacket.data, 0, facePacket.data.Length);
                    else
                        Array.Copy(faceTempl.data, i * facePacket.data.Length,
                            facePacket.data, 0, faceTempl.data.Length % facePacket.data.Length);
                    if (this.wgudp == null)
                    {
                        this.wgudp = new wgUdpComm();
                        Thread.Sleep(300);
                    }
                    facePacket.GetNewXid();
                    cmd = facePacket.ToBytes(this.wgudp.udpPort);
                    if (cmd == null)
                    {
                        wgTools.WriteLine("出错");
                        return -1;
                    }
                    ret = this.wgudp.udp_get(cmd, this.m_priWaitMs, facePacket.xid, IP, Port, ref recv);
                    if (recv == null || !res.get(recv, 0x14))
                        return -1;

                    errCode = BitConverter.ToInt32(BitConverter.GetBytes(res.tag), 0);
                    switch (errCode)
                    {
                        case wgTools.ErrorCode.ERR_DB_IS_FULL:
                        case wgTools.ErrorCode.ERR_FAIL:
                            return errCode;
                        case wgTools.ErrorCode.ERR_FACE_DOUBLED:
                            _faceTemplDoubled = new MjFaceTempl(faceTempl);
                            return errCode;
                    }
                }
            }
            return ret;
        }		
		
        public int DownloadIP(int ControllerSN, string IP, int Port,
            string doorName, ref DataTable dtPrivilege, ref DataTable tbFpTempl)
        {
            return this.DownloadIP_internal(ControllerSN, IP, Port,
                doorName, ref dtPrivilege, ref tbFpTempl, "");
        }

        public int DownloadIP(int ControllerSN, string IP, int Port, 
            string doorName, ref DataTable dtPrivilege, ref DataTable tbFpTempl, string PCIPAddr)
        {
            return this.DownloadIP_internal(ControllerSN, IP, Port,
                doorName, ref dtPrivilege, ref tbFpTempl, PCIPAddr);
        }

        internal int DownloadIP_internal(int ControllerSN, string IP, int Port, 
            string doorName, ref DataTable dtPrivilege, ref DataTable tbFpTempl, string PCIPAddr)
        {
            if (m_bStopDownloadPrivilege)
                return -1;

            bool bFirstCard = false, bMoreCard = false, bIsDeleted = false,
                bName = false, bFpTempl = false;
            if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
                bFirstCard = true;
            if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
                bMoreCard = true;
            if (dtPrivilege.Columns.Contains("f_IsDeleted"))
                bIsDeleted = true;
            else
                dtPrivilege.Columns.Add("f_IsDeleted", Type.GetType("System.UInt32"));
            if (dtPrivilege.Columns.Contains("f_ConsumerName"))
                bName = true;
            if (tbFpTempl != null)
                bFpTempl = true;
            wgTools.WriteLine("Download Privilege Start");

            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            
            /************************************************************************/
            /* Load user info and fp entry                                          */
            /************************************************************************/
            WGPacketSSI_FLASH_QUERY_internal flashQuery = 
                new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint) ControllerSN,
                    0, MaxChunkSize - 1);
            byte[] userInfoBuf = new byte[getUserInfoEndAddr(ControllerSN) - getUserInfoStartAddr(ControllerSN) + 1];
            byte[] fpEntryBuf = new byte[getFpEntryEndAddr(ControllerSN) - getFpEntryStartAddr(ControllerSN) + 1];

            /* Load user info data */
            flashQuery.iStartFlashAddr = getUserInfoStartAddr(ControllerSN);
            while (flashQuery.iStartFlashAddr <= getUserInfoEndAddr(ControllerSN))
            {
                flashQuery.iEndFlashAddr = (uint)((flashQuery.iStartFlashAddr + UserInfoChunkSize) - 1);
                flashQuery.GetNewXid();
                byte[] recv = null;
                if (this.wgudp.udp_get(flashQuery.ToBytes(this.wgudp.udpPort), 1000,
                    flashQuery.xid, IP, Port, ref recv) < 0)
                    return -1;

                Array.Copy(recv, OffsetInFlashQuery, userInfoBuf,
                    flashQuery.iStartFlashAddr - getUserInfoStartAddr(ControllerSN), UserInfoChunkSize);
                flashQuery.iStartFlashAddr += UserInfoChunkSize;
            }

            /* Load fp entry data */
            flashQuery.iStartFlashAddr = getFpEntryStartAddr(ControllerSN);
            while (flashQuery.iStartFlashAddr <= getFpEntryEndAddr(ControllerSN))
            {
                flashQuery.iEndFlashAddr = (uint)((flashQuery.iStartFlashAddr + FpEntryChunkSize) - 1);
                flashQuery.GetNewXid();
                byte[] recv = null;
                if (this.wgudp.udp_get(flashQuery.ToBytes(this.wgudp.udpPort), 1000, 
                    flashQuery.xid, IP, Port, ref recv) < 0)
                    return -1;

                Array.Copy(recv, OffsetInFlashQuery, fpEntryBuf,
                    flashQuery.iStartFlashAddr - getFpEntryStartAddr(ControllerSN), FpEntryChunkSize);
                flashQuery.iStartFlashAddr += FpEntryChunkSize;
            }

            /************************************************************************/
            /* Load user name                                                       */
            /************************************************************************/
            /* byte[] userNameBuf = new byte[MaxUserCount * MjNameInfo.UserNameLength];
            if (bName)
            {
                WGPacketNameQuery nameQuery = new WGPacketNameQuery(0x23, 0x60, (uint)ControllerSN, 0);
                for (uint tag = 0; tag <= MaxUserCount / NameCountInChunk; tag++)
                {
                    byte[] recv = null;
                    nameQuery.tag = tag * NameCountInChunk;
                    nameQuery.GetNewXid();
                    if (this.wgudp.udp_get(nameQuery.ToBytes(this.wgudp.udpPort), 1000,
                        nameQuery.xid, IP, Port, ref recv) < 0)
                        return -1;

                    if (recv != null)
                        Array.Copy(recv, OffsetInChunk, userNameBuf, tag * NameChunkSize, NameChunkSize);
                }
            }*/

            MjUserInfo user = new MjUserInfo();
            for (uint i = 0; i < userInfoBuf.Length; i += MjUserInfo.Length)
            {
                user.get(userInfoBuf, (int)i);
                if (m_bStopDownloadPrivilege)
                    return -1;

                /* valid user */
                if (user.isValid())
                {
                    DataRow row = dtPrivilege.NewRow();
                    row["f_ConsumerNO"] = user.userID;
                    row["f_CardNO"] = user.cardID;
                    row["f_PIN"] = user.password.ToString();
                    row["f_BeginYMD"] = user.ymdStart;
                    row["f_EndYMD"] = user.ymdEnd;
                    row["f_ControlSegID1"] = user.ControlSegIndexGet(1);
                    row["f_ControlSegID2"] = user.ControlSegIndexGet(2);
                    row["f_ControlSegID3"] = user.ControlSegIndexGet(3);
                    row["f_ControlSegID4"] = user.ControlSegIndexGet(4);
                    //row["f_ControlSegID5"] = user.ControlSegIndexGet(5);
                    if (bFirstCard)
                    {
                        row["f_DoorFirstCard_1"] = user.FirstCardGet(1);
                        row["f_DoorFirstCard_2"] = user.FirstCardGet(2);
                        row["f_DoorFirstCard_3"] = user.FirstCardGet(3);
                        row["f_DoorFirstCard_4"] = user.FirstCardGet(4);
                        //row["f_DoorFirstCard_5"] = user.FirstCardGet(5);
                    }
                    if (bMoreCard)
                    {
                        row["f_MoreCards_GrpID_1"] = user.MoreCardGroupIndexGet(1);
                        row["f_MoreCards_GrpID_2"] = user.MoreCardGroupIndexGet(2);
                        row["f_MoreCards_GrpID_3"] = user.MoreCardGroupIndexGet(3);
                        row["f_MoreCards_GrpID_4"] = user.MoreCardGroupIndexGet(4);
                        //row["f_MoreCards_GrpID_5"] = user.MoreCardGroupIndexGet(5);
                    }
                    row["f_IsDeleted"] = user.IsDeleted ? 1 : 0;
                    if (bName)
                    {
                        /*
                        string name = MjUserInfo.getUserName(userNameBuf, i / MjUserInfo.Length * MjNameInfo.UserNameLength);
                        if (name != null)
                            row["f_ConsumerName"] = name;*/
                    }
                    dtPrivilege.Rows.Add(row);
                }
            }
            dtPrivilege.AcceptChanges();

            if (m_bStopDownloadPrivilege)
                return -1;
            
            if (bFpTempl)
            {
                MjFpEntry entry = new MjFpEntry();
                MjFpTempl templ = new MjFpTempl();
                WGPacketFpQuery fpQuery = new WGPacketFpQuery(0x23, 0x40, (uint)ControllerSN, 0, 0, 0);
                for (uint i = 0; i < fpEntryBuf.Length; i += MjFpEntry.Length)
                {
                    entry.get(fpEntryBuf, (int)i);
                    if (m_bStopDownloadPrivilege)
                        return -1;

                    /* valid fp entry */
                    if (entry.isValid())
                    {
                        user.get(userInfoBuf, (int)(entry.pos * MjUserInfo.Length));
                        if (user.isValid())
                        {
                            fpQuery.userID = user.userID;
                            fpQuery.index = entry.index;
                            fpQuery.tag = 0;
                            fpQuery.GetNewXid();
                            byte[] recv = null;
                            if (this.wgudp.udp_get(fpQuery.ToBytes(this.wgudp.udpPort), 1000,
                                fpQuery.xid, IP, Port, ref recv) < 0)
                                return -1;

                            Array.Copy(recv, OffsetInChunk, templ.data, 0, MaxChunkSize);

                            fpQuery.tag = 1;
                            fpQuery.GetNewXid();
                            recv = null;
                            if (this.wgudp.udp_get(fpQuery.ToBytes(this.wgudp.udpPort), 1000,
                                fpQuery.xid, IP, Port, ref recv) < 0)
                                return -1;

                            Array.Copy(recv, OffsetInChunk, templ.data, MaxChunkSize, MjFpTempl.Length - MaxChunkSize);

                            DataRow row = tbFpTempl.NewRow();
                            row["f_ConsumerNO"] = user.userID;
                            row["f_Finger"] = entry.index;
                            row["f_Templ"] = templ.data;
                            //TODOJ row["f_Duress"] = duress;
                        }
                    }
                }
                tbFpTempl.AcceptChanges();
            }

            DataView view = new DataView(dtPrivilege);
            view.RowFilter = "f_IsDeleted = 1";
            if (view.Count > 0)
                for (int m = view.Count - 1; m >= 0; m--)
                    view.Delete(m);
            dtPrivilege.AcceptChanges();
            if (!bIsDeleted)
            {
                dtPrivilege.Columns.Remove("f_IsDeleted");
                dtPrivilege.AcceptChanges();
            }
            return 1;
        }

        public int UploadIP(int ControllerSN, string IP, int Port, 
            string doorName, DataTable dtPrivilege, DataTable tbFpTempl, DataTable tbFaceTempl)
        {
            return this.UploadIP_internal(ControllerSN, IP, Port, 
                doorName, dtPrivilege, tbFpTempl, tbFaceTempl, "");
        }

        public int UploadIP(int ControllerSN, string IP, int Port, string 
            doorName, DataTable dtPrivilege, DataTable tbFpTempl, DataTable tbFaceTempl, string PCIPAddr)
        {
            return this.UploadIP_internal(ControllerSN, IP, Port,
                doorName, dtPrivilege, tbFpTempl, tbFaceTempl, PCIPAddr);
        }

        internal int UploadIP_internal(int ControllerSN, string IP, int Port, 
            string doorName, DataTable dtPrivilege, DataTable tbFpTempl, DataTable tbFaceTempl, string PCIPAddr)
        {
            if (m_bStopUploadPrivilege)
                return wgTools.ErrorCode.ERR_FAIL;

            bool bFirstCard = false, bMoreCard = false;//, bFpTempl = false;
            if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
                bFirstCard = true;
            if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
                bMoreCard = true;
            bool bAllowUploadUserName = this.bAllowUploadUserName;
            if (!dtPrivilege.Columns.Contains("f_ConsumerName"))
                bAllowUploadUserName = false;
            wgTools.WriteLine("Upload Start");

            uint maxFpCount = getMaxFpCount(ControllerSN), 
                maxFaceCount = getMaxFaceCount(ControllerSN), 
                maxUserCount = getMaxUserCount(ControllerSN);

            byte[] userBuf = new byte[getUserInfoEndAddr(ControllerSN) - getUserInfoStartAddr(ControllerSN) + 1];
            byte[] entryBuf = new byte[getFpEntryEndAddr(ControllerSN) - getFpEntryStartAddr(ControllerSN) + 1];
            byte[] faceEntryBuf = new byte[getFaceEntryEndAddr(ControllerSN) - getFaceEntryStartAddr(ControllerSN) + 1];
            byte[] templBuf = new byte[maxFpCount * MjFpTempl.Length];
            byte[] faceTemplBuf = new byte[maxFaceCount * MjFaceTempl.Length];
            byte[] nameBuf = new byte[maxUserCount * MjNameInfo.UserNameLength];
            int userCount = dtPrivilege.Rows.Count;
            if (userCount > (int)maxUserCount) userCount = (int)maxUserCount;

            ushort j = 0, k = 0;
            MjUserInfo user = new MjUserInfo();
            MjFpEntry entry = new MjFpEntry();
            MjFpTempl templ = new MjFpTempl();
            MjFaceEntry face_entry = new MjFaceEntry();
            MjFaceTempl face_templ = new MjFaceTempl();
            string val;
            for (int i = 0; i < userCount; i++)
            {
                user.userID = uint.Parse(dtPrivilege.Rows[i]["f_ConsumerNO"].ToString());
                val = dtPrivilege.Rows[i]["f_CardNO"].ToString();
                user.cardID = (val == "") ? 0 : long.Parse(val);
                val = dtPrivilege.Rows[i]["f_PIN"].ToString();
                user.password = (val == "") ? 0 : uint.Parse(val);
                user.ymdStart = (DateTime)dtPrivilege.Rows[i]["f_BeginYMD"];
                user.ymdEnd = (DateTime)dtPrivilege.Rows[i]["f_EndYMD"];
                user.ControlSegIndexSet(1, (byte)dtPrivilege.Rows[i]["f_ControlSegID1"]);
                user.ControlSegIndexSet(2, (byte)dtPrivilege.Rows[i]["f_ControlSegID2"]);
                user.ControlSegIndexSet(3, (byte)dtPrivilege.Rows[i]["f_ControlSegID3"]);
                user.ControlSegIndexSet(4, (byte)dtPrivilege.Rows[i]["f_ControlSegID4"]);
                //user.ControlSegIndexSet(5, (byte)dtPrivilege.Rows[i]["f_ControlSegID5"]);
                if (bFirstCard)
                {
                    user.FirstCardSet(1, ((byte)dtPrivilege.Rows[i]["f_DoorFirstCard_1"]) > 0);
                    user.FirstCardSet(2, ((byte)dtPrivilege.Rows[i]["f_DoorFirstCard_2"]) > 0);
                    user.FirstCardSet(3, ((byte)dtPrivilege.Rows[i]["f_DoorFirstCard_3"]) > 0);
                    user.FirstCardSet(4, ((byte)dtPrivilege.Rows[i]["f_DoorFirstCard_4"]) > 0);
                    //user.FirstCardSet(5, ((byte)dtPrivilege.Rows[i]["f_DoorFirstCard_5"]) > 0);
                }
                if (bMoreCard)
                {
                    user.MoreCardGroupIndexSet(1, (byte)dtPrivilege.Rows[i]["f_MoreCards_GrpID_1"]);
                    user.MoreCardGroupIndexSet(2, (byte)dtPrivilege.Rows[i]["f_MoreCards_GrpID_2"]);
                    user.MoreCardGroupIndexSet(3, (byte)dtPrivilege.Rows[i]["f_MoreCards_GrpID_3"]);
                    user.MoreCardGroupIndexSet(4, (byte)dtPrivilege.Rows[i]["f_MoreCards_GrpID_4"]);
                    //user.MoreCardGroupIndexSet(5, (byte)dtPrivilege.Rows[i]["f_MoreCards_GrpID_5"]);
                }
                if (wgMjController.IsElevator_Internal(ControllerSN))
                    user.AllowFloors_internal = (ulong)dtPrivilege.Rows[i]["f_AllowFloors"];

                if (user.cardID != 0) user.hasCard();
                if (user.password != 0) user.hasPassword();
                user.accessAllowed();

                DataView view = null;
                if (wgMjController.isA30AC(ControllerSN) || wgMjController.isA50AC(ControllerSN))
                {
                    /* fill template buffer */
                    view = new DataView(tbFpTempl);
                    bool duress;
                    view.RowFilter = "f_ConsumerNO = " + user.userID.ToString();
                    entry.pos = (ushort)i;
                    foreach (DataRowView drv in view)
                    {
                        entry.valid = true;
                        entry.index = (byte)(int)drv["f_Finger"];
                        duress = (int)drv["f_Duress"] > 0;
                        user.hasFingerprint(entry.index);
                        if (duress) user.hasDuress(entry.index);
                        Array.Copy(entry.toRaw(), 0,
                            entryBuf, j * MjFpEntry.Length, MjFpEntry.Length);
                        templ.data = (byte[])drv["f_Templ"];
                        Array.Copy(templ.data, 0,
                            templBuf, j * MjFpTempl.Length, MjFpTempl.Length);
                        j++;
                    }
                }

                if (wgMjController.isF300AC(ControllerSN))
                {
                    /* fill face template */
                    view = new DataView(tbFaceTempl);
                    view.RowFilter = "f_ConsumerNO = " + user.userID.ToString();
                    face_entry.pos = (ushort)i;
                    foreach (DataRowView drv in view)
                    {
                        face_entry.valid = true;
                        user.hasFace();
                        Array.Copy(face_entry.toRaw(), 0,
                            faceEntryBuf, k * MjFaceEntry.Length, MjFaceEntry.Length);
                        face_templ.data = (byte[])drv["f_Templ"];
                        Array.Copy(face_templ.data, 0,
                            faceTemplBuf, k * MjFaceTempl.Length, MjFaceTempl.Length);
                        k++;
                    }
                }

                Array.Copy(user.ToBytes(), 0x4,
                    userBuf, i * MjUserInfo.Length, MjUserInfo.Length);
                if (bAllowUploadUserName)
                {
                    byte[] raw = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dtPrivilege.Rows[i]["f_ConsumerName"]));
                    for (int n = 0; (n < MjNameInfo.UserNameLength) && (n < raw.Length); n++)
                        nameBuf[n + i * MjNameInfo.UserNameLength] = raw[n];
                    if ((raw.Length > 0) && (raw.Length < MjNameInfo.UserNameLength))
                        nameBuf[raw.Length + i * MjNameInfo.UserNameLength] = 0;
                }
            }
            int fpCount = (j > maxFpCount) ? (int)maxFpCount : j;
            int faceCount = (k > maxFaceCount) ? (int)maxFaceCount : k;

            /* fill the tail */
            for (int i = userCount * (int)MjUserInfo.Length; i < userBuf.Length; i++)
                userBuf[i] = 0xff;
            for (int i = userCount * (int)MjNameInfo.UserNameLength; i < nameBuf.Length; i++)
                nameBuf[i] = 0xff;
            for (int i = fpCount * (int)MjFpEntry.Length; i < entryBuf.Length; i++)
                entryBuf[i] = 0x00;
            for (int i = faceCount * (int)MjFaceEntry.Length; i < faceEntryBuf.Length; i++)
                faceEntryBuf[i] = 0x00;

            /************************************************************************/
            /* Upload user info, fp template                                        */
            /************************************************************************/
            if (this.wgudp == null)
            {
                if (PCIPAddr == null)
                {
                    this.wgudp = new wgUdpComm();
                }
                else
                {
                    IPAddress address;
                    if (IPAddress.TryParse(PCIPAddr, out address))
                        this.wgudp = new wgUdpComm(IPAddress.Parse(PCIPAddr));
                    else
                        this.wgudp = new wgUdpComm();
                }
                Thread.Sleep(300);
            }

            int ret = 0;

            this.DisplayProcessInfo(doorName, 0x186a1, 0);

            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }

            byte[] cmd = null, recv = null;
            const int retries = 3, timeout = 300;

            /* Upload user info */
            WGPacketSSI_FLASH_internal flashPacket = new WGPacketSSI_FLASH_internal
                (0x21, 0x20, (uint)ControllerSN, 0, MaxChunkSize - 1);
            flashPacket.iStartFlashAddr = getUserInfoStartAddr(ControllerSN);
            while (flashPacket.iStartFlashAddr <= getUserInfoEndAddr(ControllerSN))
            {
                flashPacket.iEndFlashAddr = flashPacket.iStartFlashAddr + UserInfoChunkSize - 1;
                if (flashPacket.iEndFlashAddr > getUserInfoEndAddr(ControllerSN))
                    flashPacket.iEndFlashAddr = getUserInfoEndAddr(ControllerSN);
                for (int dummy = 0; dummy < retries; dummy++)
                {
                    if (cmd != null)
                        flashPacket.GetNewXid();
                    Array.Copy(userBuf, flashPacket.iStartFlashAddr - getUserInfoStartAddr(ControllerSN),
                        flashPacket.ucData, 0, UserInfoChunkSize);
                    cmd = flashPacket.ToBytes(this.wgudp.udpPort);
                    ret = this.wgudp.udp_get(cmd, 1000, flashPacket.xid, IP, Port, ref recv);
                    if (recv != null)
                        break;
                    Thread.Sleep(timeout);
                }
                if (recv == null)
                    return -1;
                
                flashPacket.iStartFlashAddr += UserInfoChunkSize;
            }

            /* Upload fp entry */
            int errCode;
            if (wgMjController.isA30AC(ControllerSN) || wgMjController.isA50AC(ControllerSN))
            {
                flashPacket.iStartFlashAddr = getFpEntryStartAddr(ControllerSN);
                while (flashPacket.iStartFlashAddr <= getFpEntryEndAddr(ControllerSN))
                {
                    flashPacket.iEndFlashAddr = flashPacket.iStartFlashAddr + FpEntryChunkSize - 1;
                    if (flashPacket.iEndFlashAddr > getFpEntryEndAddr(ControllerSN))
                        flashPacket.iEndFlashAddr = getFpEntryEndAddr(ControllerSN);
                    for (int dummy = 0; dummy < retries; dummy++)
                    {
                        if (cmd != null)
                            flashPacket.GetNewXid();
                        Array.Copy(entryBuf, flashPacket.iStartFlashAddr - getFpEntryStartAddr(ControllerSN),
                            flashPacket.ucData, 0, FpEntryChunkSize);
                        cmd = flashPacket.ToBytes(this.wgudp.udpPort);
                        ret = this.wgudp.udp_get(cmd, 1000, flashPacket.xid, IP, Port, ref recv);
                        if (recv != null)
                            break;
                        Thread.Sleep(timeout);
                    }
                    if (recv == null)
                        return -1;

                    flashPacket.iStartFlashAddr += FpEntryChunkSize;
                }

                /* Upload fp template */
                /* It is supposed that if userID is invalid then index is the position of the template. */
                WGPacketFpTempl fpPacket = new WGPacketFpTempl(0x23, 0x50, (uint)ControllerSN,
                    0, 0, 0, true);
                WGPacketNameQuery res = new WGPacketNameQuery();
                errCode = 0;
                for (uint fp = 0; fp < fpCount; fp++)
                {
                    entry.get(entryBuf, (int)(fp * MjFpEntry.Length));
                    user.get(userBuf, (int)(entry.pos * MjUserInfo.Length));
                    fpPacket.userID = user.userID;
                    for (uint tag = 0; tag < 2; tag++)
                    {
                        fpPacket.index = (ushort)entry.index;
                        fpPacket.tag = (byte)tag;
                        if (tag == 0)
                            Array.Copy(templBuf, fp * MjFpTempl.Length,
                                fpPacket.data, 0, MaxChunkSize);
                        else
                            Array.Copy(templBuf, fp * MjFpTempl.Length + MaxChunkSize,
                                fpPacket.data, 0, MjFpTempl.Length - MaxChunkSize);

                        for (int dummy = 0; dummy < retries; dummy++)
                        {
                            if (cmd != null)
                                fpPacket.GetNewXid();
                            cmd = fpPacket.ToBytes(this.wgudp.udpPort);
                            ret = this.wgudp.udp_get(cmd, 1000, fpPacket.xid, IP, Port, ref recv);
                            if (recv != null)
                                break;

                            Thread.Sleep(timeout);
                        }
                        if (recv == null || !res.get(recv, 0x14))
                            return -1;

                        errCode = BitConverter.ToInt32(BitConverter.GetBytes(res.tag), 0);
                        switch (errCode)
                        {
                            case wgTools.ErrorCode.ERR_DB_IS_FULL:
                            case wgTools.ErrorCode.ERR_FAIL:
                                return errCode;
                            case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                                _templDoubled = new MjFpTempl((int)user.userID, (int)fp, null, false);
                                return errCode;
                        }
                    }
                }
            }

            /* Upload face entry */
            if (wgMjController.isF300AC(ControllerSN))
            {
                flashPacket.iStartFlashAddr = getFaceEntryStartAddr(ControllerSN);
                while (flashPacket.iStartFlashAddr <= getFaceEntryEndAddr(ControllerSN))
                {
                    flashPacket.iEndFlashAddr = flashPacket.iStartFlashAddr + FaceEntryChunkSize - 1;
                    if (flashPacket.iEndFlashAddr > getFaceEntryEndAddr(ControllerSN))
                        flashPacket.iEndFlashAddr = getFaceEntryEndAddr(ControllerSN);
                    uint len = flashPacket.iEndFlashAddr - flashPacket.iStartFlashAddr + 1;
                    for (int dummy = 0; dummy < retries; dummy++)
                    {
                        if (cmd != null)
                            flashPacket.GetNewXid();
                        Array.Copy(faceEntryBuf, flashPacket.iStartFlashAddr - getFaceEntryStartAddr(ControllerSN),
                            flashPacket.ucData, 0, len);
                        cmd = flashPacket.ToBytes(this.wgudp.udpPort);
                        ret = this.wgudp.udp_get(cmd, 1000, flashPacket.xid, IP, Port, ref recv);
                        if (recv != null)
                            break;
                        Thread.Sleep(timeout);
                    }
                    if (recv == null)
                        return -1;

                    flashPacket.iStartFlashAddr += len;
                }

                /* Upload face template */
                /* It is supposed that if userID is invalid then index is the position of the template. */
                WGPacketFaceTempl facePacket = new WGPacketFaceTempl(0x23, 0x90, (uint)ControllerSN,
                    0, 0, true);
                WGPacketNameQuery res = new WGPacketNameQuery();
                errCode = 0;
                for (uint face = 0; face < faceCount; face++)
                {
                    face_entry.get(faceEntryBuf, (int)(face * MjFaceEntry.Length));
                    user.get(userBuf, (int)(face_entry.pos * MjUserInfo.Length));
                    facePacket.userID = user.userID;
                    for (uint tag = 0; tag <= MjFaceTempl.Length / MaxChunkSize; tag++)
                    {
                        facePacket.tag = (byte)tag;
                        if (tag != MjFaceTempl.Length / MaxChunkSize)
                            Array.Copy(faceTemplBuf, face * MjFaceTempl.Length + MaxChunkSize * tag,
                                facePacket.data, 0, MaxChunkSize);
                        else
                            Array.Copy(faceTemplBuf, face * MjFaceTempl.Length + MaxChunkSize * tag,
                                facePacket.data, 0, MjFaceTempl.Length % MaxChunkSize);
                        for (int dummy = 0; dummy < retries; dummy++)
                        {
                            if (cmd != null)
                                facePacket.GetNewXid();
                            cmd = facePacket.ToBytes(this.wgudp.udpPort);
                            ret = this.wgudp.udp_get(cmd, 1000, facePacket.xid, IP, Port, ref recv);
                            if (recv != null)
                                break;

                            Thread.Sleep(timeout);
                        }
                        if (recv == null || !res.get(recv, 0x14))
                            return -1;

                        errCode = BitConverter.ToInt32(BitConverter.GetBytes(res.tag), 0);
                        switch (errCode)
                        {
                            case wgTools.ErrorCode.ERR_DB_IS_FULL:
                            case wgTools.ErrorCode.ERR_FAIL:
                                return errCode;
                            case wgTools.ErrorCode.ERR_FACE_DOUBLED:
                                _faceTemplDoubled = new MjFaceTempl((int)user.userID, null);
                                return errCode;
                        }
                    }
                }
            }

            /* Upload user name */
            /*
            WGPacketName namePacket = new WGPacketName(0x23, 0x70, (uint)ControllerSN,
                MjUserInfo.invalidUserID, 0);
            for (int i = 0; i <= userCount / NameCountInChunk; i++)
            {
                namePacket.tag = (uint)i * NameCountInChunk;
                Array.Copy(nameBuf, MjNameInfo.UserNameLength * i, namePacket.data, 0, NameChunkSize);
                for (int dummy = 0; dummy < retries; dummy++)
                {
                    if (cmd != null)
                        namePacket.GetNewXid();
                    cmd = namePacket.ToBytes(this.wgudp.udpPort);
                    ret = this.wgudp.udp_get(cmd, 1000, namePacket.xid, IP, Port, ref recv);
                    if (recv != null)
                        break;
                    Thread.Sleep(timeout);
                }
                if (recv == null)
                    return -203;
            } */

            wgTools.WriteLine("upload End");
            return ret;
        }

        private uint getUserInfoStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800A3000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x801F3000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x801F3000;
                    break;
            }
            return flashAddr;
        }

        private uint getUserInfoEndAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800B2FFF;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x8020AFFF;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x8020AFFF;
                    break;
            }
            return flashAddr;
        }

        private uint getFpEntryStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800B3000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x8020B000;
                    break;
            }
            return flashAddr;
        }

        private uint getFpEntryEndAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    flashAddr = 0x800B4FFF;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    flashAddr = 0x8020DFFF;
                    break;
            }
            return flashAddr;
        }

        private uint getFaceEntryStartAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x800B3000;
                    break;
            }
            return flashAddr;
        }

        private uint getFaceEntryEndAddr(int controllerSN)
        {
            uint flashAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.F300_AC:
                    flashAddr = 0x800B3FFF;
                    break;
            }
            return flashAddr;
        }
		
		public bool fpDoubleCheck
        {
            get { return m_bFpDoubleCheck; }
            set { m_bFpDoubleCheck = value; }
        }

        public MjFpTempl templDoubled
        {
            get { return _templDoubled; }
        }

        public bool faceDoubleCheck
        {
            get { return m_bFaceDoubleCheck; }
            set { m_bFaceDoubleCheck = value; }
        }

        public MjFaceTempl faceTemplDoubled
        {
            get { return _faceTemplDoubled; }
        }

        public uint getMaxFpCount(int sn)
        {
            uint count = 0;
            wgMjController.DeviceType type = wgMjController.getDeviceType(sn);
            switch (type)
            {
                case wgMjController.DeviceType.A30_AC:
                case wgMjController.DeviceType.A50_AC:
                    count = 3000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    count = 0;
                    break;
            }
            return count;
        }
        
        public uint getMaxFaceCount(int sn)
        {
            uint count = 0;
            wgMjController.DeviceType type = wgMjController.getDeviceType(sn);
            switch (type)
            {
                case wgMjController.DeviceType.A30_AC:
                case wgMjController.DeviceType.A50_AC:
                    count = 0;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    count = 1200;
                    break;
            }
            return count;
        }

        public uint getMaxUserCount(int sn)
        {
            uint count = 0;
            wgMjController.DeviceType type = wgMjController.getDeviceType(sn);
            switch (type)
            {
                case wgMjController.DeviceType.A30_AC:
                case wgMjController.DeviceType.A50_AC:
                    count = 3000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    count = 1200;
                    break;
            }
            return count;
        }

		#endregion
    }
}

