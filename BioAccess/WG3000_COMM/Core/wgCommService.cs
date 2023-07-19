namespace WG3000_COMM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using WG3000_COMM.DataOper;

    public class wgCommService : MarshalByRefObject, IDisposable
    {
        private wgCommServer commServer;
        private int m_bHaveServer;
        private int m_bStopWatch;
        private DateTime m_ControllerUpdateTime = DateTime.Now;
        private DateTime m_lastGetInfoDateTime = DateTime.Now;
        private Dictionary<int, icController> m_NowWatching;
        private static int m_unconnect_timeout_sec = 6;
        private Dictionary<int, icController> m_WantWatching;
        private static int m_watching_cycle_ms = 400;
        private int updateCnt;

        public event OnCommEventHandler EventHandler;

        public wgCommService()
        {
            Thread thread = new Thread(new ThreadStart(this.WatchController));
            thread.Name = "Comm Service";
            thread.Start();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (this.commServer != null))
            {
                this.commServer.Close();
            }
        }

        public ControllerRunInformation GetRunInfo(int ControllerSN)
        {
            return this.commServer.GetRunInfo(ControllerSN);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void PublishEvent(string message)
        {
            wgTools.WgDebugWrite("Publishing \"{0}\"...", new object[] { message });
            if (this.EventHandler != null)
            {
                this.EventHandler(message);
            }
        }

        public void StopWatch()
        {
            Interlocked.Exchange(ref this.m_bStopWatch, 1);
        }

        private void udpserver_evNewRecord(string info)
        {
            if (this.EventHandler != null)
            {
                OnCommEventHandler handler = null;
                int num = 1;
                foreach (Delegate delegate2 in this.EventHandler.GetInvocationList())
                {
                    try
                    {
                        handler = (OnCommEventHandler) delegate2;
                        handler(info);
                    }
                    catch (Exception exception)
                    {
                        wgTools.WriteLine(exception.ToString());
                        wgTools.WgDebugWrite("事件订阅者" + num.ToString() + "发生错误,系统将取消事件订阅!", new object[0]);
                        this.EventHandler -= handler;
                    }
                    num++;
                }
            }
        }

        private void WatchController()
        {
            wgTools.WgDebugWrite("watchController= {0:d}", new object[] { 0x69f6bc7 });
            if (this.m_bHaveServer <= 0)
            {
                Interlocked.Increment(ref this.m_bHaveServer);
                WGPacketBasicRunInformation4ServerToSend send = new WGPacketBasicRunInformation4ServerToSend();
                send.type = 0x20;
                send.code = 0x20;
                send.iDevSnFrom = 0;
                send.iDevSnTo = 0;
                send.iCallReturn = 0;
                this.commServer = new wgCommServer();
                this.commServer.evNewRecord += new wgCommServer.newRecordHandler(this.udpserver_evNewRecord);
                byte[] cmd = null;
                wgTools.WgDebugWrite("m_bStopWatch= {0:d}", new object[] { this.m_bStopWatch });
                DateTime now = DateTime.Now;
                int num = -1;
                int num4 = 0;
                while (this.m_bStopWatch < 1)
                {
                    if (num != this.updateCnt)
                    {
                        this.m_NowWatching = null;
                        if (this.m_WantWatching != null)
                        {
                            Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_NowWatching, this.m_WantWatching);
                        }
                        Interlocked.Exchange(ref num, this.updateCnt);
                        num4 = 3;
                    }
                    else if (this.m_NowWatching == null)
                    {
                        Thread.Sleep(100);
                    }
                    else if (num4 >= 0)
                    {
                        num4--;
                        long ticks = DateTime.Now.Ticks;
                        foreach (KeyValuePair<int, icController> pair in this.m_NowWatching)
                        {
                            send.iDevSnTo = (uint) pair.Value.ControllerSN;
                            if (cmd != null)
                            {
                                send.GetNewXid();
                            }
                            cmd = send.ToBytes();
                            this.commServer.UDP_OnlySend(cmd, 300, pair.Value.IP, pair.Value.PORT);
                            Thread.Sleep(1);
                        }
                        long num3 = DateTime.Now.Ticks;
                        this.m_lastGetInfoDateTime = DateTime.Now;
                        if ((num3 > ticks) && ((num3 - ticks) < ((m_watching_cycle_ms * 0x3e8) * 10)))
                        {
                            Thread.Sleep((int) (m_watching_cycle_ms - (((int) (num3 - ticks)) / 0x2710)));
                        }
                    }
                }
                this.commServer.evNewRecord -= new wgCommServer.newRecordHandler(this.udpserver_evNewRecord);
                this.commServer.Dispose();
            }
        }

        public DateTime lastGetInfoDateTime
        {
            get
            {
                return this.m_lastGetInfoDateTime;
            }
        }

        public static int unconnect_timeout_sec
        {
            get
            {
                if (Watching_Cycle_ms > (m_unconnect_timeout_sec * 0x3e8))
                {
                    return ((Watching_Cycle_ms / 0x3e8) + 1);
                }
                return m_unconnect_timeout_sec;
            }
            set
            {
                if ((value > 0) && (value < 0xe10))
                {
                    m_unconnect_timeout_sec = value;
                }
            }
        }

        public static int Watching_Cycle_ms
        {
            get
            {
                return m_watching_cycle_ms;
            }
            set
            {
                if ((value > 0) && (value < 0x36ee80))
                {
                    m_watching_cycle_ms = value;
                }
            }
        }

        public Dictionary<int, icController> WatchingController
        {
            get
            {
                return this.m_NowWatching;
            }
            set
            {
                if (this.m_WantWatching != null)
                {
                    this.m_WantWatching = null;
                }
                if (value != null)
                {
                    Dictionary<int, icController> dictionary = new Dictionary<int, icController>(value);
                    Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_WantWatching, dictionary);
                }
                this.m_ControllerUpdateTime = DateTime.Now;
                if (this.updateCnt == 0x7fffffff)
                {
                    Interlocked.Exchange(ref this.updateCnt, 0);
                }
                Interlocked.Increment(ref this.updateCnt);
            }
        }
    }
}

