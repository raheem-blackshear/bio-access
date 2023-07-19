namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading;

    public class wgMjController : IDisposable
    {
        private const int ControllerDriverVer_LOC = 0x11;
        private const int ControllerProductTypeCode_LOC = 0x12;
        public const int Door_NC = 2;
        public const int Door_NO = 1;
        public const int Door_Offline = 0;
        public const int Door_Online = 3;
        public const int Door_OnlyAllowFirstCard = 4;
        private int m_ControllerDriverVer;
        private int m_ControllerProductTypeCode;
        private int m_ControllerSN;
        private string m_IP = "";
        private int m_PORT = 0xea60;
        private wgMjControllerRunInformation m_runinfo = new wgMjControllerRunInformation();
        private int priMaxTries = 3;
        private int sleep4Tries = 300;
        private TcpClient tcp;
        private wgUdpComm wgudp;

        private string _ssid_wifi = "";
        private string _key_wifi = "";
        private bool _enable_wifi = false;
        private string _ip_wifi = "";
        private string _mask_wifi = "";
        private string _gateway_wifi = "";
        private int _port_wifi = 60000;

        /* WiFi Communication Parameters */
        public int AdjustTimeIP(DateTime dateTimeNew, int door)
        {
            return this.AdjustTimeIP_internal(dateTimeNew, door, "");
        }

        public int AdjustTimeIP(DateTime dateTimeNew, int door, string PCIPAddr)
        {
            return this.AdjustTimeIP_internal(dateTimeNew, door, PCIPAddr);
        }

        internal int AdjustTimeIP_internal(DateTime dateTimeNew, int door, string PCIPAddr)
        {
            WGPacketBasicAdjustTimeToSend send = new WGPacketBasicAdjustTimeToSend(dateTimeNew, door);
            send.type = 0x20;
            send.code = 0x30;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
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
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int AdjustTimeIP_TCP(DateTime dateTimeNew)
        {
            WGPacketBasicAdjustTimeToSend send = new WGPacketBasicAdjustTimeToSend(dateTimeNew, 0);
            send.type = 0x20;
            send.code = 0x30;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            byte[] buffer = send.ToBytes((ushort) (this.tcp.Client.LocalEndPoint as IPEndPoint).Port);
            if (buffer == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            NetworkStream stream = this.tcp.GetStream();
            if (stream.CanWrite)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            DateTime time = DateTime.Now.AddSeconds(2.0);
            byte[] buffer2 = new byte[0x7d0];
            int num = 0;
            while (time > DateTime.Now)
            {
                if (stream.CanRead && stream.DataAvailable)
                {
                    num = stream.Read(buffer2, 0, buffer2.Length);
                    break;
                }
            }
            if (num > 0)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int AutoIPSetIP(int cmdOption, string strIP, string strMask, string strGateway, int ipstart, int ipend)
        {
            WGPacketBasicAutoIPSetToSend send = new WGPacketBasicAutoIPSetToSend((uint) cmdOption, strIP, strMask, strGateway, (uint) ipstart, (uint) ipend);
            send.type = 0x20;
            send.code = 0xc0;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.wgudp != null)
                {
                    this.wgudp.Close();
                    this.wgudp.Dispose();
                }
                if (this.tcp != null)
                {
                    this.tcp.Close();
                }
            }
        }

        public int FormatIP()
        {
            byte[] data = new byte[0x480];
            data[0x3fc] = 0x8a;
            data[0x3fe] = 0x9a;
            data[0x400] = 0x8a;
            data[0x402] = 0x9a;
            data[0x403] = 0xa5;
            data[0x402] = 0xa5;
            data[0x401] = 0xa5;
            data[0x400] = 0xa5;
            data[0x404] = 0x89;
            data[0x405] = 0x98;
            data[0x406] = 0x89;
            data[0x407] = 0x98;
            if (GetControllerType(this.ControllerSN) > 0)
            {
                this.UpdateConfigureSuperIP_internal(data);
                return 1;
            }
            return -1;
        }

        public int GetConfigureIP(ref byte[] controlConfigureData)
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.m_ControllerSN;
            packet.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, packet.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                this.m_ControllerDriverVer = recv[0x11];
                this.m_ControllerProductTypeCode = recv[0x12];
                BitConverter.ToString(recv);
                controlConfigureData = recv;
                return 1;
            }
            return -1;
        }

        public int GetConfigureIP(ref wgMjControllerConfigure controlConfigure)
        {
            try
            {
                byte[] controlConfigureData = null;
                if ((this.GetConfigureIP(ref controlConfigureData) == 1) && (controlConfigureData != null))
                {
                    controlConfigure = new wgMjControllerConfigure(controlConfigureData, 20);
                    return 1;
                }
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public int GetControlDataIP(int start, int len, ref byte[] data)
        {
            return this.GetControlDataIP_internal(start, len, ref data);
        }

        private int GetControlDataIP_internal(int start, int len, ref byte[] data)
        {
            WGPacketCPU_CONFIG_READToSend send = new WGPacketCPU_CONFIG_READToSend((uint) start, (uint) len);
            send.type = 0x25;
            send.code = 0x10;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv == null)
            {
                return -1;
            }
            this.m_ControllerDriverVer = recv[0x11];
            this.m_ControllerProductTypeCode = recv[0x12];
            for (int i = 0; (i < recv.Length) && (i < data.Length); i++)
            {
                data[i] = recv[i];
            }
            return 1;
        }

        public static int GetControllerReaderNum(int controllerSN)
        {
            if (controllerSN >= 100000000)
            {
                if (controllerSN < 200000000)
                    return 2;
                if (controllerSN < 300000000)
                    return 4;

				// A30
                if (controllerSN < 310000000)
                {
                    int count = 2, doors = (controllerSN - 300000000) / 1000000;
                    switch (doors)
                    {
                        case 1: count = 2; break;
                        case 2:
                        case 4: count = 4; break;
                    }
                    return count;
                }
					
				// A50
                if (controllerSN < 500000000)
                    return 0;
                if (controllerSN < 510000000)
                {
                    int count = 2, doors = (controllerSN - 500000000) / 1000000;
                    switch (doors)
                    {
                        case 1: count = 2; break;
                        case 2:
                        case 4: count = 4; break;
                    }
                    return count;
                }

                // F300AC
                if (controllerSN < 700000000)
                    return 0;
                if (controllerSN < 710000000)
                {
                    int count = 2, doors = (controllerSN - 700000000) / 1000000;
                    switch (doors)
                    {
                        case 1: count = 2; break;
                        case 2:
                        case 4: count = 4; break;
                    }
                    return count;
                }
				
				// S8000
				if (controllerSN < 800000000)
					return 0;
				if (controllerSN < 810000000)
				{
					int count = 2, doors = (controllerSN - 800000000) / 1000000;
					switch (doors)
					{
						case 1: count = 2; break;
						case 2:
						case 4: count = 4; break;
					}
                    return count;
				}
            }
            return 0;
        }

        /* changed by dragon :
         * 
         *      Get the number of doors for a controller which have specific serial number.
         *      A50 and A30 supports 5 doors at most.
         * 
         */
        public static int GetControllerType(int controllerSN)
        {
            if (controllerSN > 100000000)
            {
                if (controllerSN < 200000000)
                    return 0;

                /*
                 * A30 Online defines
                 * SN: "30XYYYYYY"
                 *        X: Door count
                 */
                if (controllerSN < 300000000)
                    return 0;
                if (controllerSN < 310000000)
                    return (controllerSN - 300000000) / 1000000;

                /* A50T defines
                 * SN: "50XYYYYYY"
                 *        X: Door count
                 */
                if (controllerSN < 500000000)
                    return 0;

                if (controllerSN < 510000000)
                    return (controllerSN - 500000000) / 1000000;

                /* F300AC defines
                 * SN: "70XYYYYYY"
                 *        X: Door count
                 */
                if (controllerSN < 700000000)
                    return 0;

                if (controllerSN < 710000000)
                    return (controllerSN - 700000000) / 1000000;

                /* S8000 defines
                 * SN: "80XYYYYYY"
                 *        X: Door count
                 */
                if (controllerSN < 800000000)
                    return 0;

                if (controllerSN < 810000000)
                    return (controllerSN - 800000000) / 1000000;
            }
            return 0;
        }

        /*
         * Get controller genre
         * 
         */
        public static DeviceType getDeviceType(int controllerSN)
        {
			// A30
            if (controllerSN < 300000000)
                return DeviceType.UNKNOWN_TYPE;

            if (controllerSN < 310000000)
                return DeviceType.A30_AC;

			// A50
            if (controllerSN < 500000000)
                return DeviceType.UNKNOWN_TYPE;

            if (controllerSN < 510000000)
                return DeviceType.A50_AC;

            // F300AC
            if (controllerSN < 700000000)
                return DeviceType.UNKNOWN_TYPE;

            if (controllerSN < 710000000)
                return DeviceType.F300_AC;

			// S8000
            if (controllerSN < 800000000)
                return DeviceType.UNKNOWN_TYPE;

            if (controllerSN < 810000000)
                return DeviceType.S8000_DC;

            return DeviceType.UNKNOWN_TYPE;
        }

        /*
         * Is fingerprint feasible?
         * 
         */
        public static bool isFingerprintFeasible(int controllerSN)
        {
            bool feasible = false;
            switch (getDeviceType(controllerSN))
            {
                case DeviceType.A30_AC:
                case DeviceType.A50_AC:
                    feasible = true;
                    break;
            }
            return feasible;
        }

        /*
         * Is face feasible?
         * 
         */
        public static bool isFaceFeasible(int controllerSN)
        {
            DeviceType type = getDeviceType(controllerSN);
            bool feasible = false;
            switch (type)
            {
                case DeviceType.F300_AC:
                    feasible = true;
                    break;
            }
            return feasible;
        }

        public int GetControlTaskListIP(ref byte[] controlTaskListData)
        {
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] destinationArray = new byte[0x1000];
            for (int i = 0; i < 0x1000; i += 0x400)
            {
                WGPacketSSI_FLASH_QUERY_internal _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint) this.m_ControllerSN,
                    (uint)(MjControlTaskItem.getFlashStartAddr(m_ControllerSN) + i),
                    (uint)(((MjControlTaskItem.getFlashStartAddr(m_ControllerSN) + i) + 0x400) - 1));
                byte[] cmd = _internal.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                {
                    wgTools.WriteLine("\r\nError 1");
                    return -1;
                }
                this.wgudp.udp_get(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
                if (recv != null)
                {
                    this.m_ControllerDriverVer = recv[0x11];
                    this.m_ControllerProductTypeCode = recv[0x12];
                    Array.Copy(recv, 0x1c, destinationArray, i, 0x400);
                }
                else
                {
                    return -1;
                }
            }
            controlTaskListData = destinationArray;
            return 1;
        }

        public int GetHolidayListIP(ref byte[] holidayListData)
        {
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] destinationArray = new byte[0x1000];
            for (int i = 0; i < 0x1000; i += 0x400)
            {
                WGPacketSSI_FLASH_QUERY_internal _internal = new WGPacketSSI_FLASH_QUERY_internal(0x21, 0x10, (uint)this.m_ControllerSN,
                    (uint)(MjControlHolidayTime.getFlashStartAddr(m_ControllerSN) + i),
                    (uint)(((MjControlHolidayTime.getFlashStartAddr(m_ControllerSN) + i) + 0x400) - 1));
                byte[] cmd = _internal.ToBytes(this.wgudp.udpPort);
                if (cmd == null)
                {
                    wgTools.WriteLine("\r\nError 1");
                    return -1;
                }
                this.wgudp.udp_get(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
                if (recv != null)
                {
                    this.m_ControllerDriverVer = recv[0x11];
                    this.m_ControllerProductTypeCode = recv[0x12];
                    Array.Copy(recv, 0x1c, destinationArray, i, 0x400);
                }
                else
                {
                    return -1;
                }
            }
            holidayListData = destinationArray;
            return 1;
        }

        public int GetMjControllerRunInformationIP()
        {
            byte[] recvInfo = null;
            return this.GetMjControllerRunInformationIP(ref recvInfo);
        }

        public int GetMjControllerRunInformationIP(ref byte[] recvInfo)
        {
            return this.GetMjControllerRunInformationIP_internal(ref recvInfo, "");
        }

        public int GetMjControllerRunInformationIP(ref byte[] recvInfo, string PCIPAddr)
        {
            return this.GetMjControllerRunInformationIP_internal(ref recvInfo, PCIPAddr);
        }

        internal int GetMjControllerRunInformationIP_internal(ref byte[] recvInfo, string PCIPAddr)
        {
            WGPacketBasicRunInformationToSend send = new WGPacketBasicRunInformationToSend();
            send.type = 0x20;
            send.code = 0x10;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
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
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv == null)
            {
                wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                return -1;
            }
            this.m_ControllerDriverVer = recv[0x11];
            this.m_ControllerProductTypeCode = recv[0x12];
            recvInfo = recv;
            if (this.m_ControllerSN != -1)
            {
                this.m_runinfo.UpdateInfo_internal(recv, 20, (uint) this.m_ControllerSN);
            }
            else
            {
                uint controllerSN = (uint) (((recv[8] + (recv[9] << 8)) + (recv[10] << 0x10)) + (recv[11] << 0x18));
                this.m_runinfo.UpdateInfo_internal(recv, 20, controllerSN);
            }
            for (int i = 0; i < 10; i++)
            {
                if (this.m_runinfo.newSwipes[i].indexInDataFlash_Internal == uint.MaxValue)
                {
                    break;
                }
            }
            return 1;
        }

        public int GetMjControllerRunInformationIP_TCP(string strIP, ref byte[] recvInfo)
        {
            if (this.tcp == null)
            {
                this.tcp = new TcpClient();
                this.tcp.Connect(IPAddress.Parse(strIP), this.PORT);
            }
            WGPacketBasicRunInformationToSend send = new WGPacketBasicRunInformationToSend();
            send.type = 0x20;
            send.code = 0x10;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            byte[] array = null;
            this.tcp.ReceiveBufferSize = 0x7d0;
            this.tcp.SendBufferSize = 0x7d0;
            byte[] buffer = send.ToBytes((ushort) (this.tcp.Client.LocalEndPoint as IPEndPoint).Port);
            if (buffer == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            NetworkStream stream = this.tcp.GetStream();
            if (stream.CanWrite)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            DateTime time = DateTime.Now.AddSeconds(2.0);
            byte[] buffer3 = new byte[0x7d0];
            int num = 0;
            while (time > DateTime.Now)
            {
                if (stream.CanRead && stream.DataAvailable)
                {
                    num = stream.Read(buffer3, 0, buffer3.Length);
                    break;
                }
            }
            if (num > 0)
            {
                array = new byte[buffer3.Length];
                buffer3.CopyTo(array, 0);
            }
            if (array != null)
            {
                this.m_ControllerDriverVer = array[0x11];
                this.m_ControllerProductTypeCode = array[0x12];
                recvInfo = array;
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int GetMjControllerRunInformationIPNoTries(ref byte[] recvInfo)
        {
            WGPacketBasicRunInformationToSend send = new WGPacketBasicRunInformationToSend();
            send.type = 0x20;
            send.code = 0x10;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get_notries(cmd, 100, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                this.m_ControllerDriverVer = recv[0x11];
                this.m_ControllerProductTypeCode = recv[0x12];
                recvInfo = recv;
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        private ushort GetPort()
        {
            return (ushort) (this.tcp.Client.LocalEndPoint as IPEndPoint).Port;
        }

        public string GetProductInfoIP(ref string compactInfo, ref string Desc)
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x20;
            packet.code = 80;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.m_ControllerSN;
            packet.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return "";
            }
            this.wgudp.udp_get(cmd, 1000, packet.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                this.m_ControllerDriverVer = recv[0x11];
                this.m_ControllerProductTypeCode = recv[0x12];
                string str = "";
                uint num = (uint) (((recv[8] + (recv[9] << 8)) + (recv[10] << 0x10)) + (recv[11] << 0x18));
                str = (str + string.Format("控制器SN: {0}\r\n", num)) + string.Format("驱动版本: {0}.{1}\r\n", recv[0x1f], recv[30]) + string.Format("驱动日期: {0:X2}{1:X2}-{2:X2}-{3:X2}\r\n", new object[] { recv[0x20], recv[0x21], recv[0x22], recv[0x23] });
                Desc = str;
                str = str + "获取控制器信息:\r\n" + BitConverter.ToString(recv);
                string str2 = "";
                str2 = (((str2 + string.Format("SN={0:d},", num)) + string.Format("VER={0}.{1},", recv[0x1f], recv[30]) + string.Format("DATE={0:X2}{1:X2}-{2:X2}-{3:X2},", new object[] { recv[0x20], recv[0x21], recv[0x22], recv[0x23] })) + "DATA=" + BitConverter.ToString(recv).Replace("-", "").Substring(40, 80)) + BitConverter.ToString(recv).Replace("-", "").Substring(0x780, 0x100) + ";";
                compactInfo = str2;
                return str;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return "";
        }

        public string GetProductInfoIP_TCP()
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x20;
            packet.code = 80;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.m_ControllerSN;
            packet.iCallReturn = 0;
            byte[] bytSend = packet.ToBytes(this.GetPort());
            byte[] buffer2 = this.TCP_Send(bytSend);
            if (buffer2 != null)
            {
                this.m_ControllerDriverVer = buffer2[0x11];
                this.m_ControllerProductTypeCode = buffer2[0x12];
                return ("获取控制器信息: " + BitConverter.ToString(buffer2));
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return "";
        }

        public int GetSingleSwipeRecord(int swipeIndex, ref wgMjControllerSwipeRecord Swipe)
        {
            WGPacketBasicGetSingleSwipeRecordToSend send = new WGPacketBasicGetSingleSwipeRecordToSend((uint) swipeIndex);
            send.type = 0x20;
            send.code = 0xb0;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            Swipe = null;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                uint loc = (uint) (((recv[40] + (recv[0x29] << 8)) + (recv[0x2a] << 0x10)) + (recv[0x2b] << 0x18));
                if (loc < uint.MaxValue)
                {
                    Swipe = new wgMjControllerSwipeRecord(recv, 0x18, (uint) this.m_ControllerSN, loc);
                }
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public static bool IsElevator(int controllerSN)
        {
            return IsElevator_Internal(controllerSN);
        }

        internal static bool IsElevator_Internal(int controllerSN)
        {
            return ((controllerSN >= 0xa21fe80) && (controllerSN <= 0xaba94ff));
        }

        public void NetIPConfigure(string strSN, string strMac, string strIP, string strMask, string strGateway, string strPort, string PCIPAddr)
        {
            if (this.wgudp != null)
            {
                this.wgudp = null;
            }
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
            WGPacketWith1152_internal _internal = new WGPacketWith1152_internal();
            _internal.type = 0x25;
            _internal.code = 0x20;
            _internal.iDevSnFrom = 0;
            _internal.iCallReturn = 0;
            if (int.Parse(strSN) == -1)
            {
                _internal.iDevSnTo = uint.MaxValue;
            }
            else
            {
                _internal.iDevSnTo = uint.Parse(strSN);
            }
            byte[] buffer = new byte[0x480];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
            WGPacketWith1152_internal _internal2 = new WGPacketWith1152_internal();
            int index = 0x74;
            IPAddress.Parse(strIP).GetAddressBytes().CopyTo(_internal2.ucData, index);
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 120;
            IPAddress.Parse(strMask).GetAddressBytes().CopyTo(_internal2.ucData, index);
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 0x7c;
            if (string.IsNullOrEmpty(strGateway))
            {
                _internal2.ucData[index] = 0;
                _internal2.ucData[index + 1] = 0;
                _internal2.ucData[index + 2] = 0;
                _internal2.ucData[index + 3] = 0;
            }
            else
            {
                IPAddress.Parse(strGateway).GetAddressBytes().CopyTo(_internal2.ucData, index);
            }
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 0x80;
            _internal2.ucData[index] = (byte) (int.Parse(strPort) & 0xff);
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index++;
            _internal2.ucData[index] = (byte) ((int.Parse(strPort) >> 8) & 0xff);
            _internal2.ucData[0x400 + (index >> 3)] = (byte) (_internal2.ucData[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
            index = 0;
            for (int j = 0; j < 0x10; j++)
            {
                buffer[index] = (byte) (_internal2.ucData[0x74 + j] & 0xff);
                buffer[0x400 + (index >> 3)] = (byte) (buffer[0x400 + (index >> 3)] | ((byte) (((int) 1) << (index & 7))));
                index++;
            }
            buffer.CopyTo(_internal.ucData, 0);
            byte[] cmd = _internal.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WgDebugWrite("Err: IP Configure", new object[0]);
            }
            else
            {
                byte[] recv = null;
                this.wgudp.udp_get(cmd, 1000, 0x7fffffff, null, 0xea60, ref recv);
            }
        }

        public int RebootControllerIP()
        {
            return this.RebootControllerIP(null);
        }

        public int RebootControllerIP(string PCIPAddr)
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x20;
            packet.code = 0x90;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.m_ControllerSN;
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
            byte[] recv = null;
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, 0, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("重启控制器 没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int RemoteOpenDoorIP(int doorNO)
        {
            return this.RemoteOpenDoorIP_internal(doorNO, 1, ulong.MaxValue);
        }

        public int RemoteOpenDoorIP(int doorNO, uint operatorId, ulong operatorCardNO)
        {
            WGPacketBasicRemoteOpenDoorToSend send = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
            send.type = 0x20;
            send.code = 0x40;
            send.iDevSnFrom = 0;
            send.OperatorID = Math.Max(operatorId, 1);
            send.OperatorCardNO = operatorCardNO;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        internal int RemoteOpenDoorIP_internal(int doorNO, uint operatorId, ulong operatorCardNO)
        {
            WGPacketBasicRemoteOpenDoorToSend send = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
            send.type = 0x20;
            send.code = 0x40;
            send.iDevSnFrom = 0;
            send.OperatorID = Math.Max(operatorId, 1);
            send.OperatorCardNO = operatorCardNO;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int RemoteOpenDoorIP_TCP(int doorNO, uint operatorId, uint operatorCardNO)
        {
            WGPacketBasicRemoteOpenDoorToSend send = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
            send.type = 0x20;
            send.code = 0x40;
            send.iDevSnFrom = 0;
            send.OperatorID = operatorId;
            send.OperatorCardNO = operatorCardNO;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            byte[] bytSend = send.ToBytes(this.GetPort());
            if (this.TCP_Send(bytSend) != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int RemoteOpenFoorIP(int floorNO, uint operatorId, ulong operatorCardNO)
        {
            WGPacketBasicRemoteOpenDoorToSend send = new WGPacketBasicRemoteOpenDoorToSend(1, floorNO);
            send.type = 0x20;
            send.code = 0x40;
            send.iDevSnFrom = 0;
            send.OperatorID = Math.Max(operatorId, 1);
            send.OperatorCardNO = operatorCardNO;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int RenewControlTaskListIP()
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x20;
            packet.code = 0x70;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.m_ControllerSN;
            packet.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, packet.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int RestoreAllSwipeInTheControllersIP()
        {
            return this.UpdateLastGetRecordLocationIP(0);
        }

        public void SearchControlers(ref ArrayList arrController)
        {
            uint maxValue = uint.MaxValue;
            WGPacket packet = new WGPacket();
            packet.type = 0x24;
            packet.code = 0x10;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = maxValue;
            packet.iCallReturn = 0;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            foreach (NetworkInterface interface2 in allNetworkInterfaces)
            {
                UnicastIPAddressInformationCollection unicastAddresses = interface2.GetIPProperties().UnicastAddresses;
                if (unicastAddresses.Count > 0)
                {
                    foreach (UnicastIPAddressInformation information in unicastAddresses)
                    {
                        if (!information.Address.IsIPv6LinkLocal && (information.Address.ToString() != "127.0.0.1"))
                        {
                            this.wgudp = new wgUdpComm(information.Address);
                            Thread.Sleep(300);
                            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
                            if (cmd == null)
                            {
                                return;
                            }
                            byte[] recv = null;
                            this.wgudp.udp_get(cmd, 1000, uint.MaxValue, null, 0xea60, ref recv);
                            if (recv != null)
                            {
                                long num3 = DateTime.Now.Ticks + 0x3d0900L;
                                wgMjControllerConfigure configure = new wgMjControllerConfigure(recv, 20);
                                if (list.IndexOf(configure.controllerSN) < 0)
                                {
                                    list.Add(configure.controllerSN);
                                    configure.pcIPAddr = information.Address.ToString();
                                    list2.Add(configure);
                                }
                                while (DateTime.Now.Ticks < num3)
                                {
                                    if (this.wgudp.PacketCount > 0)
                                    {
                                        while (this.wgudp.PacketCount > 0)
                                        {
                                            configure = new wgMjControllerConfigure(this.wgudp.GetPacket(), 20);
                                            if (list.IndexOf(configure.controllerSN) < 0)
                                            {
                                                list.Add(configure.controllerSN);
                                                configure.pcIPAddr = information.Address.ToString();
                                                list2.Add(configure);
                                            }
                                        }
                                        num3 = DateTime.Now.Ticks + 0x3d0900L;
                                    }
                                    else
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                            }
                        }
                    }
                    Console.WriteLine();
                }
            }
            arrController = list2;
        }

        public int SpecialPingIP()
        {
            WGPacketBasicRemoteOpenDoorToSend send = new WGPacketBasicRemoteOpenDoorToSend(1);
            send.specialDoorNo4ping(5);
            send.type = 0x20;
            send.code = 0x40;
            send.iDevSnFrom = 0;
            send.OperatorID = 1;
            send.OperatorCardNO = 0L;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get_notries(cmd, 100, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                this.m_ControllerSN = (((recv[11] << 0x18) + (recv[10] << 0x10)) + (recv[9] << 8)) + recv[8];
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public void TCP_Close()
        {
            if (this.tcp != null)
            {
                this.tcp.Close();
            }
        }

        public void TCP_Open(string strIP)
        {
            if (this.tcp == null)
            {
                this.tcp = new TcpClient();
                this.tcp.Connect(IPAddress.Parse(strIP), 0xea60);
                this.tcp.ReceiveBufferSize = 0x7d0;
                this.tcp.SendBufferSize = 0x7d0;
            }
        }

        public byte[] TCP_Send(byte[] bytSend)
        {
            if (bytSend == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return null;
            }
            NetworkStream stream = this.tcp.GetStream();
            if (stream.CanWrite)
            {
                stream.Write(bytSend, 0, bytSend.Length);
            }
            DateTime time = DateTime.Now.AddSeconds(2.0);
            byte[] buffer = new byte[0x7d0];
            byte[] destinationArray = null;
            int length = 0;
            while (time > DateTime.Now)
            {
                if (stream.CanRead && stream.DataAvailable)
                {
                    length = stream.Read(buffer, 0, buffer.Length);
                    destinationArray = new byte[length];
                    Array.Copy(buffer, destinationArray, length);
                    return destinationArray;
                }
            }
            return destinationArray;
        }

        public int test1024Read(uint times, ref int page)
        {
            WGPacketSSI_FLASH_QUERY tssi_flash_query = new WGPacketSSI_FLASH_QUERY();
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            int num2 = 0;
            try
            {
                int num3 = (int) times;
                int num4 = 0x12f0;
                while (++num2 <= num3)
                {
                    tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.m_ControllerSN, (uint) (num4 * 0x400), (uint) (((num4 * 0x400) + 0x400) - 1));
                    byte[] cmd = tssi_flash_query.ToBytes(this.wgudp.udpPort);
                    byte[] recv = null;
                    int num = this.wgudp.udp_get(cmd, 1000, tssi_flash_query.xid, this.m_IP, this.m_PORT, ref recv);
                    page = num4;
                    if (num < 0)
                    {
                        return -num2;
                    }
                    tssi_flash_query = new WGPacketSSI_FLASH_QUERY(0x21, 0x10, (uint) this.m_ControllerSN, (uint) ((num4 + 1) * 0x400), (uint) ((((num4 + 1) * 0x400) + 0x400) - 1));
                    cmd = tssi_flash_query.ToBytes(this.wgudp.udpPort);
                    recv = null;
                    num = this.wgudp.udp_get(cmd, 1000, tssi_flash_query.xid, this.m_IP, this.m_PORT, ref recv);
                    page = num4 + 1;
                    if (num < 0)
                    {
                        return -num2;
                    }
                }
            }
            catch (Exception)
            {
            }
            return num2;
        }

        public int test1024Write()
        {
            WGPacketSSI_FLASH tssi_flash = new WGPacketSSI_FLASH();
            tssi_flash.type = 0x21;
            tssi_flash.code = 0x30;
            tssi_flash.iDevSnFrom = 0;
            tssi_flash.iDevSnTo = (uint) this.m_ControllerSN;
            tssi_flash.iCallReturn = 0;
            tssi_flash.ucData = new byte[0x400];
            int num = -1;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            try
            {
                int num2 = 0x12f0;
                int num3 = 0x400;
                tssi_flash.iStartFlashAddr = (uint) (num2 * 0x400);
                tssi_flash.iEndFlashAddr = (tssi_flash.iStartFlashAddr + 0x400) - 1;
                for (int i = 0; i < 0x400; i++)
                {
                    tssi_flash.ucData[i] = 0xff;
                }
                for (int j = 0; j < num3; j++)
                {
                    tssi_flash.ucData[j] = 0;
                }
                byte[] recv = null;
                num = this.wgudp.udp_get(tssi_flash.ToBytes(this.wgudp.udpPort), 1000, tssi_flash.xid, this.m_IP, this.m_PORT, ref recv);
                if (num < 0)
                {
                    return -1;
                }
                if ((recv != null) && (recv.Length == 0x41c))
                {
                    for (int n = 0; n < num3; n++)
                    {
                        if (recv[n + 0x1c] != 0)
                        {
                            return -2;
                        }
                    }
                }
                tssi_flash.iStartFlashAddr += 0x400;
                tssi_flash.iEndFlashAddr = (tssi_flash.iStartFlashAddr + 0x400) - 1;
                for (int k = 0; k < 0x400; k++)
                {
                    tssi_flash.ucData[k] = 0xff;
                }
                num = this.wgudp.udp_get(tssi_flash.ToBytes(this.wgudp.udpPort), 1000, tssi_flash.xid, this.m_IP, this.m_PORT, ref recv);
                if (num < 0)
                {
                    return -1;
                }
                if ((recv == null) || (recv.Length != 0x41c))
                {
                    return num;
                }
                for (int m = 0; m < num3; m++)
                {
                    if (recv[m + 0x1c] != 0xff)
                    {
                        return -3;
                    }
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public int UpdateConfigureCPUSuperIP(byte[] data, string oldPassword)
        {
            return this.UpdateConfigureCPUSuperIP(data, oldPassword, "");
        }

        public int UpdateConfigureCPUSuperIP(byte[] data, string oldPassword, string PCIPAddr)
        {
            WGPacketWith1152_internal _internal = new WGPacketWith1152_internal();
            _internal.type = 0x25;
            _internal.code = 0x20;
            _internal.iDevSnFrom = 0;
            _internal.iDevSnTo = (uint) this.m_ControllerSN;
            _internal.iCallReturn = 0;
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
            data.CopyTo(_internal.ucData, 0);
            byte[] recv = null;
            WGPacket.bCommP = !string.IsNullOrEmpty(oldPassword);
            if (WGPacket.bCommP)
            {
                WGPacket.strCommP = WGPacket.Ept(oldPassword);
            }
            byte[] cmd = _internal.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                return -1;
            }
            this.wgudp.udp_get_notries(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int UpdateConfigureIP(wgMjControllerConfigure mjconf)
        {
            return this.UpdateConfigureIP(mjconf.paramData, mjconf.needUpdate);
        }

        public int UpdateConfigureIP(byte[] dataParam, byte[] dataNeedUpdate)
        {
            WGPacketWith1152_internal _internal = new WGPacketWith1152_internal();
            _internal.type = 0x24;
            _internal.code = 0x20;
            _internal.iDevSnFrom = 0;
            _internal.iDevSnTo = (uint) this.m_ControllerSN;
            _internal.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            dataParam.CopyTo(_internal.ucData, 0);
            dataNeedUpdate.CopyTo(_internal.ucData, 0x400);
            byte[] recv = null;
            byte[] cmd = _internal.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int UpdateConfigureSuperIP(byte[] data)
        {
            return this.UpdateConfigureSuperIP_internal(data);
        }

        internal int UpdateConfigureSuperIP_internal(byte[] data)
        {
            WGPacketWith1152_internal _internal = new WGPacketWith1152_internal();
            _internal.type = 0x24;
            _internal.code = 0x30;
            _internal.iDevSnFrom = 0;
            _internal.iDevSnTo = (uint) this.m_ControllerSN;
            _internal.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            data.CopyTo(_internal.ucData, 0);
            byte[] recv = null;
            byte[] cmd = _internal.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
                return -1;
            }
            this.wgudp.udp_get_notries(cmd, 1000, uint.MaxValue, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int UpdateControlTaskListIP(byte[] controlTaskListData)
        {
            WGPacketSSI_FLASH_internal _internal = new WGPacketSSI_FLASH_internal();
            _internal.type = 0x21;
            _internal.code = 0x20;
            _internal.iDevSnFrom = 0;
            _internal.iDevSnTo = (uint) this.m_ControllerSN;
            _internal.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] sourceArray = controlTaskListData;
            byte[] cmd = null;
            byte[] recv = null;
            for (int i = 0; i < 0x1000; i += 0x400)
            {
                Array.Copy(sourceArray, i, _internal.ucData, 0, 0x400);
                _internal.iStartFlashAddr = (uint)(MjControlTaskItem.getFlashStartAddr(m_ControllerSN) + i);
                _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                for (int j = 0; j < this.priMaxTries; j++)
                {
                    if (cmd != null)
                    {
                        _internal.GetNewXid();
                    }
                    cmd = _internal.ToBytes(this.wgudp.udpPort);
                    if (cmd == null)
                    {
                        wgTools.WriteLine("\r\nError 1");
                        return -1;
                    }
                    this.wgudp.udp_get(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
                    if (recv != null)
                    {
                        bool flag = true;
                        for (int k = 0; (k < _internal.ucData.Length) && ((0x1c + k) < recv.Length); k++)
                        {
                            if (_internal.ucData[k] != recv[0x1c + k])
                            {
                                wgTools.WgDebugWrite("Upload updateControlTimeSegListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
                                flag = false;
                                recv = null;
                                break;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(this.sleep4Tries);
                }
                if (recv == null)
                {
                    wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                    break;
                }
            }
            if (recv != null)
            {
                return 1;
            }
            return -1;
        }

        public int UpdateControlTimeSegListIP(byte[] controlTimeSegListData)
        {
            WGPacketSSI_FLASH_internal _internal = new WGPacketSSI_FLASH_internal();
            _internal.type = 0x21;
            _internal.code = 0x20;
            _internal.iDevSnFrom = 0;
            _internal.iDevSnTo = (uint) this.m_ControllerSN;
            _internal.iCallReturn = 0;
            int num = -1;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] sourceArray = controlTimeSegListData;
            byte[] cmd = null;
            byte[] recv = null;
            for (int i = 0; i < 0x2000; i += 0x400)
            {
                Array.Copy(sourceArray, i, _internal.ucData, 0, 0x400);
                _internal.iStartFlashAddr = (uint)(MjControlTimeSeg.getFlashStartAddr(m_ControllerSN) + i);
                _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                for (int j = 0; j < this.priMaxTries; j++)
                {
                    if (cmd != null)
                    {
                        _internal.GetNewXid();
                    }
                    cmd = _internal.ToBytes(this.wgudp.udpPort);
                    if (cmd == null)
                    {
                        wgTools.WriteLine("\r\nError 1");
                        return -1;
                    }
                    num = this.wgudp.udp_get(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
                    if (recv != null)
                    {
                        bool flag = true;
                        for (int k = 0; (k < _internal.ucData.Length) && ((0x1c + k) < recv.Length); k++)
                        {
                            if (_internal.ucData[k] != recv[0x1c + k])
                            {
                                wgTools.WgDebugWrite("Upload updateControlTimeSegListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
                                flag = false;
                                recv = null;
                                break;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(this.sleep4Tries);
                }
                if ((num < 0) || (recv == null))
                {
                    wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                    break;
                }
            }
            if (recv != null)
            {
                return 1;
            }
            return -1;
        }

        public int UpdateFRamIP(uint FRamIndex, uint newValue)
        {
            return this.UpdateFRamIP_internal(FRamIndex, newValue);
        }

        private int UpdateFRamIP_internal(uint FRamIndex, uint newValue)
        {
            WGPacketBasicFRamSetToSend send = new WGPacketBasicFRamSetToSend(FRamIndex, newValue);
            send.type = 0x20;
            send.code = 0x80;
            send.iDevSnFrom = 0;
            send.iDevSnTo = (uint) this.m_ControllerSN;
            send.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = send.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, send.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int UpdateHolidayListIP(byte[] holidayListData)
        {
            WGPacketSSI_FLASH_internal _internal = new WGPacketSSI_FLASH_internal();
            _internal.type = 0x21;
            _internal.code = 0x20;
            _internal.iDevSnFrom = 0;
            _internal.iDevSnTo = (uint) this.m_ControllerSN;
            _internal.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] sourceArray = holidayListData;
            byte[] cmd = null;
            byte[] recv = null;
            for (int i = 0; i < 0x1000; i += 0x400)
            {
                Array.Copy(sourceArray, i, _internal.ucData, 0, 0x400);
                _internal.iStartFlashAddr = (uint)(MjControlHolidayTime.getFlashStartAddr(m_ControllerSN) + i);
                _internal.iEndFlashAddr = (_internal.iStartFlashAddr + 0x400) - 1;
                for (int j = 0; j < this.priMaxTries; j++)
                {
                    if (cmd != null)
                    {
                        _internal.GetNewXid();
                    }
                    cmd = _internal.ToBytes(this.wgudp.udpPort);
                    if (cmd == null)
                    {
                        wgTools.WriteLine("\r\nError 1");
                        return -1;
                    }
                    this.wgudp.udp_get(cmd, 1000, _internal.xid, this.m_IP, this.m_PORT, ref recv);
                    if (recv != null)
                    {
                        bool flag = true;
                        for (int k = 0; (k < _internal.ucData.Length) && ((0x1c + k) < recv.Length); k++)
                        {
                            if (_internal.ucData[k] != recv[0x1c + k])
                            {
                                wgTools.WgDebugWrite("Upload UpdateHolidayListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
                                flag = false;
                                recv = null;
                                break;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(this.sleep4Tries);
                }
                if (recv == null)
                {
                    wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
                    break;
                }
            }
            if (recv != null)
            {
                return 1;
            }
            return -1;
        }

        public int UpdateLastGetRecordLocationIP(uint newValue)
        {
            byte[] data = new byte[0x2c];
            string str = "深履薄夙兴温凊似兰斯馨如松之盛川流不息渊澄取映容止若思言辞安定笃初诚美慎终宜令荣业所基籍甚无竟学优登仕摄职从政存以甘棠去而益咏乐殊贵贱礼别尊卑上和下睦夫唱妇随外受傅训入奉母仪诸姑伯叔犹子比儿孔怀兄弟同气连枝交友投分切磨箴规仁慈隐恻造次弗离节义廉退颠沛匪亏性静情逸心动神疲守真志满逐物意移坚持雅操好爵自縻都邑华夏东西二京背邙面洛浮渭据泾宫殿盘郁楼观飞惊图写禽兽画彩仙灵丙舍傍启甲帐对楹肆筵设席鼓瑟吹笙升阶纳陛弁转疑星右通广内左达承明既集坟典亦聚群英杜稿钟隶漆书壁经府罗将相路侠槐卿户封八县家给千兵高冠陪辇驱毂振缨世禄侈富车驾肥轻策功茂实勒碑刻铭磻溪伊尹佐时阿衡奄宅曲阜微旦孰营桓公匡合济弱扶倾绮回汉惠说感武丁俊乂密勿多士寔宁晋楚更霸赵魏困横假途灭虢践土会盟何遵约法韩弊烦刑起翦颇牧用军最精宣";
            if (this.GetControlDataIP_internal(0x30380, 0x551c0010, ref data) > 0)
            {
                if ((((((data[0x1c] + (data[0x1d] * 0x100)) + ((data[30] * 0x100) * 0x100)) + (((data[0x1f] * 0x100) * 0x100) * 0x100)) == this.ControllerSN) && (data[0x24] == ((byte) (str[data[0x1c]] & '\x00ff')))) && (data[0x25] == ((byte) ((str[data[0x1c]] >> 8) & '\x00ff'))))
                {
                    return this.UpdateFRamIP_internal(8, newValue);
                }
                return 1;
            }
            return -1;
        }

        public int WarnResetIP()
        {
            WGPacket packet = new WGPacket();
            packet.type = 0x20;
            packet.code = 0xa0;
            packet.iDevSnFrom = 0;
            packet.iDevSnTo = (uint) this.m_ControllerSN;
            packet.iCallReturn = 0;
            if (this.wgudp == null)
            {
                this.wgudp = new wgUdpComm();
                Thread.Sleep(300);
            }
            byte[] recv = null;
            byte[] cmd = packet.ToBytes(this.wgudp.udpPort);
            if (cmd == null)
            {
                wgTools.WriteLine("\r\nError 1");
                return -1;
            }
            this.wgudp.udp_get(cmd, 1000, packet.xid, this.m_IP, this.m_PORT, ref recv);
            if (recv != null)
            {
                return 1;
            }
            wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
            return -1;
        }

        public int ControllerDriverMainVer
        {
            get
            {
                return this.m_ControllerDriverVer;
            }
        }

        public int ControllerProductTypeCode
        {
            get
            {
                return this.m_ControllerProductTypeCode;
            }
        }

        public int ControllerSN
        {
            get
            {
                return this.m_ControllerSN;
            }
            set
            {
                if ((value == -1) || (value == 999999999))
                {
                    this.m_ControllerSN = -1;
                }
                else if (GetControllerType(value) > 0)
                {
                    this.m_ControllerSN = value;
                }
            }
        }

        public string IP
        {
            get
            {
                return this.m_IP;
            }
            set
            {
                this.m_IP = value;
            }
        }

        public int PORT
        {
            get
            {
                return this.m_PORT;
            }
            set
            {
                if ((value >= 0) && (value < 0xffff))
                {
                    this.m_PORT = value;
                }
            }
        }

        public string ssid_wifi
        {
            get { return _ssid_wifi; }
            set { _ssid_wifi = value; }
        }

        public string key_wifi
        {
            get { return _key_wifi; }
            set { _key_wifi = value; }
        }

        public bool enable_wifi
        {
            get { return _enable_wifi; }
            set { _enable_wifi = value; }
        }

        public string ip_wifi
        {
            get { return _ip_wifi; }
            set { _ip_wifi = value; }
        }

        public string mask_wifi
        {
            get { return _mask_wifi; }
            set { _mask_wifi = value; }
        }

        public string gateway_wifi
        {
            get { return _gateway_wifi; }
            set { _gateway_wifi = value; }
        }

        public int port_wifi
        {
            get
            {
                return _port_wifi;
            }
            set
            {
                if (value >= 0 && value < 0xffff)
                    _port_wifi = value;
            }
        }

        public wgMjControllerRunInformation RunInfo
        {
            get
            {
                return this.m_runinfo;
            }
        }

        public static uint getFirmwareStartAddr(int controllerSN)
        {
            uint startAddr = uint.MaxValue;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    startAddr = 0x802A2000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    startAddr = 0x804EF000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    startAddr = 0x804EF000;
                    break;
				case wgMjController.DeviceType.S8000_DC:
					startAddr = 0x804C9000;
					break;
            }
            return startAddr;
        }

        public static uint getFirmwareMaxSize(int controllerSN)
        {
            uint maxSize = 0;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    maxSize = 0xA0000;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    maxSize = 0x280000;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    maxSize = 0x1900000;
                    break;
				case wgMjController.DeviceType.S8000_DC:
					maxSize = 0xA0000;
					break;
            }
            
            return maxSize;
        }

        public static uint getFirmwareMagic(int controllerSN)
        {
            uint magic = 0;

            switch (wgMjController.getDeviceType(controllerSN))
            {
                case wgMjController.DeviceType.A30_AC:
                    magic = 0x30332041;
                    break;
                case wgMjController.DeviceType.A50_AC:
                    magic = 0x0A504657;
                    break;
                case wgMjController.DeviceType.F300_AC:
                    magic = 0x46574d44;
                    break;
				case wgMjController.DeviceType.S8000_DC:
					magic = 0x80004657;
					break;
            }
            
            return magic;
        }

        public enum DeviceType
        {
            UNKNOWN_TYPE,
            A30_AC,
            A50_AC,
            S8000_DC,
            F300_AC,
        }

        public static bool isS8000DC(int controllerSN)
        {
            return (getDeviceType(controllerSN) == DeviceType.S8000_DC);
        }

        public static bool isA50AC(int controllerSN)
        {
            return (getDeviceType(controllerSN) == DeviceType.A50_AC);
        }

        public static bool isA30AC(int controllerSN)
        {
            return (getDeviceType(controllerSN) == DeviceType.A30_AC);
        }

        public static bool isF300AC(int controllerSN)
        {
            return (getDeviceType(controllerSN) == DeviceType.F300_AC);
        }
    }
}

