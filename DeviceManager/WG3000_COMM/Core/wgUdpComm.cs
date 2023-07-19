namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class wgUdpComm : IDisposable
    {
        private bool bUDPListenStop;
        public static long CommTimeoutMsMin = 300L;
        private IPAddress m_localIP;
        public static int timeoutMsInternet = 500;
        public static long triesTotal = 0L;
        private Thread UDPListenThread;
        private System.Collections.Queue UDPQueue;
        private Socket UdpSocket;

        public wgUdpComm()
        {
            this.UDPQueue = new System.Collections.Queue();
            try
            {
                this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this.UdpSocket.EnableBroadcast = true;
                this.UdpSocket.ReceiveBufferSize = 0x1000000;
                this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
                this.UDPListenThread.IsBackground = true;
                this.UDPListenThread.Start();
                Thread.Sleep(10);
            }
            catch (Exception)
            {
            }
        }

        public wgUdpComm(IPAddress localIP)
        {
            this.UDPQueue = new System.Collections.Queue();
            try
            {
                this.m_localIP = localIP;
                this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this.UdpSocket.EnableBroadcast = true;
                this.UdpSocket.ReceiveBufferSize = 0x1000000;
                this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
                this.UDPListenThread.IsBackground = true;
                this.UDPListenThread.Start();
                Thread.Sleep(10);
            }
            catch (Exception)
            {
            }
        }

        public bool ClearAllPacket()
        {
            lock (this.UDPQueue.SyncRoot)
            {
                this.UDPQueue.Clear();
            }
            return true;
        }

        public bool Close()
        {
            try
            {
                this.bUDPListenStop = true;
                if (this.UDPListenThread != null)
                {
                    this.UDPListenThread.Abort();
                }
                if (this.UDPQueue != null)
                {
                    this.UDPQueue.Clear();
                    this.UDPQueue = null;
                }
                if (this.UdpSocket != null)
                {
                    this.UdpSocket.Close();
                }
            }
            catch
            {
                throw;
            }
            return true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (this.UdpSocket != null))
            {
                this.UdpSocket.Close();
            }
        }

        public static int GetIPEndByIPAddr(string ipAddr, int ipPort, ref IPEndPoint endp)
        {
            int num = -1;
            if (ipAddr == null)
            {
                endp = new IPEndPoint(IPAddress.Broadcast, 0xea60);
                return 0;
            }
            if (ipAddr.Length == 0)
            {
                endp = new IPEndPoint(IPAddress.Broadcast, 0xea60);
                return 0;
            }
            try
            {
                int num2;
                if (int.TryParse(ipAddr.Substring(0, 1), out num2))
                {
                    IPAddress.Parse(ipAddr);
                    if (((ipAddr == "192.0.0.0") || (ipAddr == "192.168.0.0")) || (ipAddr == "192.168.168.0"))
                    {
                        endp = new IPEndPoint(IPAddress.Broadcast, ipPort);
                        num = 0;
                    }
                    else
                    {
                        endp = new IPEndPoint(IPAddress.Parse(ipAddr), ipPort);
                        num = 1;
                    }
                }
            }
            catch
            {
            }
            if (num < 0)
            {
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(ipAddr);
                    if (hostEntry.AddressList.Length > 0)
                    {
                        endp = new IPEndPoint(IPAddress.Parse(hostEntry.AddressList[0].ToString()), ipPort);
                        num = 2;
                    }
                }
                catch (Exception)
                {
                }
            }
            return num;
        }

        public byte[] GetPacket()
        {
            byte[] buffer = null;
            if (this.UDPQueue.Count <= 0)
            {
                return buffer;
            }
            lock (this.UDPQueue.SyncRoot)
            {
                return (byte[]) this.UDPQueue.Dequeue();
            }
        }

        public long getXidOfCommand(byte[] cmd)
        {
            long num = -1L;
            try
            {
                if (cmd.Length >= WGPacket.MinSize)
                {
                    num = cmd[7];
                    num = num << 0x18;
                    num += (cmd[4] | (cmd[5] << 8)) | (cmd[6] << 0x10);
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public int udp_get(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
        {
            int num = -13;
            recv = null;
            try
            {
                IPEndPoint endp = null;
                int num2 = GetIPEndByIPAddr(ipAddr, ipPort, ref endp);
                if (num2 < 0)
                {
                    return num;
                }
                if (num2 == 2)
                {
                    parWaitMs += timeoutMsInternet;
                }
                lock (this.UDPQueue.SyncRoot)
                {
                    this.UDPQueue.Clear();
                }
                if (endp == null)
                {
                    return num;
                }
                EndPoint remoteEP = endp;
                int num3 = 3;
                while (num3-- > 0)
                {
                    this.UdpSocket.SendTo(cmd, remoteEP);
                    long ticks = DateTime.Now.Ticks;
                    long num5 = ticks + ((CommTimeoutMsMin * 1000) * 10);
                    if (((CommTimeoutMsMin < parWaitMs) && (parWaitMs > 300)) && (parWaitMs < 10000))
                    {
                        num5 = ticks + ((parWaitMs * 1000) * 10);
                    }
                    if (ticks > num5)
                    {
                        return num;
                    }
                    long num6 = 0L;
                    while (num5 > DateTime.Now.Ticks)
                    {
                        if (this.UDPQueue.Count > 0)
                        {
                            byte[] buffer;
                            lock (this.UDPQueue.SyncRoot)
                            {
                                buffer = (byte[]) this.UDPQueue.Dequeue();
                            }
                            if (((xid == 0) || (xid == this.getXidOfCommand(buffer))) || (xid == uint.MaxValue))
                            {
                                recv = buffer;
                                return 1;
                            }
                        }
                        else if ((ticks + 10000) <= DateTime.Now.Ticks)
                        {
                            if (num6 > 10)
                            {
                                Thread.Sleep(30);
                                continue;
                            }
                            num6 += 1L;
                            Thread.Sleep(1);
                        }
                    }
                    wgTools.WriteLine(string.Format("tries = {0:d} cmd={1}", 3 - num3, BitConverter.ToString(cmd)));
                    if (xid == uint.MaxValue)
                    {
                        return 1;
                    }
                    triesTotal += 1L;
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        public int udp_get_notries(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
        {
            int num = -13;
            recv = null;
            try
            {
                IPEndPoint endp = null;
                int num2 = GetIPEndByIPAddr(ipAddr, ipPort, ref endp);
                if (num2 < 0)
                {
                    return num;
                }
                if (num2 == 2)
                {
                    parWaitMs += timeoutMsInternet;
                }
                lock (this.UDPQueue.SyncRoot)
                {
                    this.UDPQueue.Clear();
                }
                if (endp == null)
                {
                    return num;
                }
                EndPoint remoteEP = endp;
                int num3 = 3;
                while (num3-- > 0)
                {
                    this.UdpSocket.SendTo(cmd, remoteEP);
                    long ticks = DateTime.Now.Ticks;
                    long num5 = ticks + ((CommTimeoutMsMin * 0x3e8L) * 10L);
                    if (((CommTimeoutMsMin < parWaitMs) && (parWaitMs > 300)) && (parWaitMs < 0x2710))
                    {
                        num5 = ticks + ((parWaitMs * 0x3e8) * 10);
                    }
                    if (ticks > num5)
                    {
                        return num;
                    }
                    long num6 = 0L;
                    while (num5 > DateTime.Now.Ticks)
                    {
                        if (this.UDPQueue.Count > 0)
                        {
                            byte[] buffer;
                            lock (this.UDPQueue.SyncRoot)
                            {
                                buffer = (byte[]) this.UDPQueue.Dequeue();
                            }
                            if (((xid == 0) || (xid == this.getXidOfCommand(buffer))) || (xid == uint.MaxValue))
                            {
                                recv = buffer;
                                return 1;
                            }
                        }
                        else if ((ticks + 0x2710L) <= DateTime.Now.Ticks)
                        {
                            if (num6 > 10L)
                            {
                                Thread.Sleep(30);
                                continue;
                            }
                            num6 += 1L;
                            Thread.Sleep(1);
                        }
                    }
                    wgTools.WriteLine(string.Format("tries = {0:d} cmd={1}", 3 - num3, BitConverter.ToString(cmd)));
                    if (xid != 0x7fffffff)
                    {
                        return num;
                    }
                    return 1;
                }
            }
            catch (Exception)
            {
            }
            return num;
        }

        private void UDPListenProc()
        {
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
                if (this.m_localIP != null)
                {
                    point = new IPEndPoint(this.m_localIP, 0);
                }
                IPEndPoint point2 = new IPEndPoint(IPAddress.Broadcast, 0xea60);
                byte[] buffer4 = new byte[0x1c];
                buffer4[0] = 13;
                buffer4[1] = 13;
                byte[] buffer = buffer4;
                EndPoint localEP = point;
                EndPoint remoteEP = point2;
                this.UdpSocket.Bind(localEP);
                this.UdpSocket.SendTo(buffer, remoteEP);
                do
                {
                    byte[] buffer2 = new byte[1500];
                    int length = this.UdpSocket.ReceiveFrom(buffer2, ref localEP);
                    byte[] destinationArray = new byte[length];
                    Array.Copy(buffer2, 0, destinationArray, 0, length);
                    if (WGPacket.Parsing(ref destinationArray, ((IPEndPoint) localEP).Port) >= 0)
                    {
                        lock (this.UDPQueue.SyncRoot)
                        {
                            this.UDPQueue.Enqueue(destinationArray);
                        }
                    }
                }
                while (!this.bUDPListenStop);
            }
            catch
            {

            }
        }

        public IPAddress localIP
        {
            get
            {
                return this.m_localIP;
            }
        }

        public int PacketCount
        {
            get
            {
                return this.UDPQueue.Count;
            }
        }

        public ushort udpPort
        {
            get
            {
                try
                {
                    if ((this.UdpSocket != null) && (this.UdpSocket.LocalEndPoint != null))
                    {
                        return (ushort) ((IPEndPoint) this.UdpSocket.LocalEndPoint).Port;
                    }
                }
                catch
                {
                }
                return 0;
            }
        }
    }
}

