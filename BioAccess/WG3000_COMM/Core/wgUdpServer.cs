namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class wgUdpServer : IDisposable
    {
        private ArrayList arrlstController = new ArrayList();
        private ArrayList arrlstLastRecordIndex = new ArrayList();
        private ArrayList arrlstSwipeRecord = new ArrayList();
        private bool bUDPListenStop;
        private byte[] cmdtemp = new byte[0x41c];
        private Thread DealRuninfoPacketThread;
        private IPEndPoint endp4broadcst;
        private long iNewRecordsCnt;
        private Thread UDPListenThread;
        private System.Collections.Queue UDPQueue = new System.Collections.Queue();
        private Socket UdpSocket;
        private Dictionary<uint, udpController> watchedControllers = new Dictionary<uint, udpController>();
        private ulong prevCardId = 0;

        public event newRecordHandler evNewRecord
        {
            add
            {
                this.evNewRecords += value;
            }
            remove
            {
                this.evNewRecords -= value;
            }
        }

        private event newRecordHandler evNewRecords;

        public wgUdpServer()
        {
            try
            {
                this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this.UdpSocket.EnableBroadcast = true;
                this.UdpSocket.ReceiveBufferSize = 0x1000000;
                this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
                this.UDPListenThread.Name = "wgUdpServer";
                this.UDPListenThread.IsBackground = true;
                this.UDPListenThread.Start();
                this.DealRuninfoPacketThread = new Thread(new ThreadStart(this.DealRuninfoPacketProc));
                this.DealRuninfoPacketThread.Name = "Deal Run InfoPacket";
                this.DealRuninfoPacketThread.IsBackground = true;
                this.DealRuninfoPacketThread.Start();
                Thread.Sleep(10);
            }
            catch (Exception)
            {
            }
        }

        public bool Close()
        {
            try
            {
                this.bUDPListenStop = true;
                Thread.Sleep(20);
                if (this.UDPListenThread != null)
                {
                    this.UDPListenThread.Abort();
                }
                if (this.UDPQueue != null)
                {
                    this.UDPQueue.Clear();
                    this.UDPQueue = null;
                }
                if (this.DealRuninfoPacketThread != null)
                {
                    this.DealRuninfoPacketThread.Abort();
                }
                if (this.UdpSocket != null)
                {
                    this.UdpSocket.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        private void DealRuninfoPacketProc() //Run Information Routine with new swipes
        {
            try
            {
                ControllerRunInformation runinfo = null;
                do
                {
                    if (this.UDPQueue.Count > 0)
                    {
                        byte[] buffer;
                        lock (this.UDPQueue.SyncRoot)
                        {
                            buffer = (byte[]) this.UDPQueue.Dequeue();
                        }
                        if (((buffer.Length == wgMjControllerRunInformation.pktlen) && (buffer[0] == 0x20)) && (buffer[1] == 0x21))
                        {
                            uint key = BitConverter.ToUInt32(buffer, 8);
                            if (!this.watchedControllers.ContainsKey(key))
                            {
                                this.watchedControllers.Add(key, new udpController(key));
                            }
                            runinfo = this.watchedControllers[key].runinfo;
                            runinfo.update(buffer, 20, key);

                            if (runinfo.inputCardID != 0 && runinfo.inputCardID != prevCardId)
                                this.RaiseEvNewRecord(string.Format("{0:x8}{1:x16}", key, runinfo.inputCardID));
                            prevCardId = runinfo.inputCardID;

                            if ((runinfo.newSwipes[0].IndexInDataFlash != uint.MaxValue) && ((runinfo.newSwipes[0].IndexInDataFlash > this.watchedControllers[key].lastRecordIndex) || (this.watchedControllers[key].lastRecordIndex == uint.MaxValue)))
                            {
                                for (int i = 9; i >= 0; i--)
                                {
                                    if ((runinfo.newSwipes[i].IndexInDataFlash != uint.MaxValue) && ((runinfo.newSwipes[i].IndexInDataFlash > this.watchedControllers[key].lastRecordIndex) || (this.watchedControllers[key].lastRecordIndex == uint.MaxValue)))
                                    {
                                        this.watchedControllers[key].lastRecordIndex = runinfo.newSwipes[i].IndexInDataFlash;
                                        this.arrlstSwipeRecord.Add(runinfo.newSwipes[i]);
                                        if (!this.watchedControllers[key].isFirstComm)
                                        {
                                            this.RaiseEvNewRecord(runinfo.newSwipes[i].ToStringRaw());
                                            this.iNewRecordsCnt += 1L;
                                        }
                                    }
                                }
                            }
                            if (this.watchedControllers[key].isFirstComm)
                            {
                                this.watchedControllers[key].isFirstComm = false;
                            }
                        }
                        Thread.Sleep(1);
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                while (!this.bUDPListenStop);
            }
            catch (Exception)
            {
            }
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
                this.Close();
            }
        }

        public ControllerRunInformation GetRunInfo(int controllerSN)
        {
            if (this.watchedControllers.ContainsKey((uint) controllerSN))
            {
                return this.watchedControllers[(uint) controllerSN].runinfo;
            }
            return null;
        }

        private void RaiseEvNewRecord(string info)
        {
            if (this.evNewRecords != null)
            {
                this.evNewRecords(info);
            }
        }

        public int UDP_OnlySend(byte[] cmd, int parWaitMs, string ipAddr, int ipPort)
        {
            int num = -13;
            try
            {
                IPEndPoint endp = null;
                if (this.endp4broadcst == null)
                {
                    this.endp4broadcst = new IPEndPoint(IPAddress.Broadcast, 0xea60);
                }
                if (string.IsNullOrEmpty(ipAddr))
                {
                    endp = this.endp4broadcst;
                }
                else
                {
                    int num2 = wgUdpComm.GetIPEndByIPAddr(ipAddr, ipPort, ref endp);
                    if (num2 < 0)
                    {
                        return num;
                    }
                    if (num2 == 2)
                    {
                        parWaitMs += wgUdpComm.timeoutMsInternet;
                    }
                }
                if (endp != null)
                {
                    this.cmdtemp = new byte[cmd.Length];
                    Array.Copy(cmd, this.cmdtemp, cmd.Length);
                    EndPoint remoteEP = endp;
                    this.UdpSocket.SendTo(cmd, remoteEP);
                    num = 1;
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
                    byte[] buffer2 = new byte[0x5dc];
                    int length = this.UdpSocket.ReceiveFrom(buffer2, ref localEP);
                    byte[] destinationArray = new byte[length];
                    Array.Copy(buffer2, 0, destinationArray, 0, length);
                    lock (this.UDPQueue.SyncRoot)
                    {
                        this.UDPQueue.Enqueue(destinationArray);
                    }
                }
                while (!this.bUDPListenStop);
            }
            catch (Exception)
            {
            }
        }

        public delegate void newRecordHandler(string info);

        private class udpController
        {
            public uint ControllerSN;
            public bool isFirstComm = true;
            public uint lastRecordIndex = uint.MaxValue;
            public ControllerRunInformation runinfo;

            public udpController(uint SN)
            {
                this.ControllerSN = SN;
                this.runinfo = new ControllerRunInformation();
            }
        }
    }
}

