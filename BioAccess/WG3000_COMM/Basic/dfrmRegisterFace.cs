using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Drawing.Imaging;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;
using Microsoft.Win32;

namespace WG3000_COMM.Basic
{
    public partial class dfrmRegisterFace : frmBioAccess
    {
		[DllImport("kernel32")]
		public static extern int GetTickCount();
		[DllImportAttribute("H264Codec.dll", CharSet=CharSet.Unicode, CallingConvention=CallingConvention.StdCall)]
		public static extern void convertGrayImage(ref int image, int width, int height, ref int gray);
		[DllImportAttribute("H264Codec.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern int decodeH264(ref int inputBuff, int buffSize, ref int outputBuff);
		[DllImportAttribute("H264Codec.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern void dotNet(int net);
		[DllImportAttribute("H264Codec.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern void initH264();
		[DllImportAttribute("H264Codec.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern void deinitH264();

        public int ImageWidth = 640;
		public int ImageHeight = 480;
		private const int ExtractionCount = 10;

        private ArrayList cnt_serials = new ArrayList();
        private ArrayList cnt_ips = new ArrayList();
        private ArrayList cnt_ports = new ArrayList();
        //private int serial;
        //private string ip;
        //private int port;
        private static MjFaceTempl _face_templ = null;
		private static int uid;

		static byte[] gImageBits;
		IntPtr addrOfImageBits;

		static Camera camera;
		static Mutex imageMutex;
		static Thread renderThread;
		static ManualResetEvent stopEvent;
		bool capturing;


		static Bitmap portrait, clonned;
		byte[] rgbBuffer;
		IntPtr addrOfBuffer;

		UInt32 ipAddr;
		UInt16 cmdPort, dataPort;

		delegate void delegateReport(int result);
		delegateReport delegateReport_;
		void reportResult(int result)
		{
			string report;

			switch (result)
			{
				case wgTools.ErrorCode.ERR_SUCCESS:
					report = CommonStr.strFaceEnrolledSuccessfully; break;
				case wgTools.ErrorCode.ERR_OUT_OF_MEMORY:
					report = CommonStr.strOutOfMemory; break;
				case wgTools.ErrorCode.ERR_GRAY_NOT_CREATED:
					report = CommonStr.strGrayNotCreated; break;
				case wgTools.ErrorCode.ERR_FACE_NOT_DETECTED:
					report = CommonStr.strFaceNotDetected; break;
				case wgTools.ErrorCode.ERR_EYES_NOT_DETECTED:
					report = CommonStr.strEyesNotDetected; break;
				case wgTools.ErrorCode.ERR_DETAILS_NOT_DETECTED:
					report = CommonStr.strDetailsNotDetected; break;
				case wgTools.ErrorCode.ERR_FEATURES_NOT_EXTRACTED:
					report = CommonStr.strFeaturesNotExtracted; break;
				case wgTools.ErrorCode.ERR_TEMPLATE_NOT_EXTRACTED:
					report = CommonStr.strTemplateNotExtracted; break;
				case wgTools.ErrorCode.ERR_DONT_MOVE_FACE:
					report = CommonStr.strDontMoveFace; break;
				default:
					report = CommonStr.strUnknownError; break;
			}

			lblResult.Text = report;
		}

		delegate void delegateSetButtons(bool scan);
		delegateSetButtons delegateSetButtons_;
		void setButtons(bool scan)
		{
			btnScan.Enabled = scan;
			btnDelete.Enabled = !scan;
			Application.DoEvents();
		}
		
        public dfrmRegisterFace(MjFaceTempl t, int _uid)
        {
            InitializeComponent();

			// Face templates
            _face_templ = t;
			uid = _uid;

			capturing = true;
			imageMutex = new Mutex();
			stopEvent = new ManualResetEvent(false);
        }

        private void fillControllerList()
        {
            DataTable tbControllers = new DataTable();
            DataView view = new DataView(tbControllers);

            string str;
            if (wgAppConfig.IsAccessDB)
            {
                str = "SELECT f_controllerNO, f_ControllerSN, f_IP, f_PORT FROM t_b_Controller";
                using (OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString))
                {
                    using (OleDbCommand command = new OleDbCommand(str, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(tbControllers);
                        }
                    }
                    goto l_fill;
                }
            }
            str = "SELECT f_controllerNO, f_ControllerSN, f_IP, f_PORT FROM t_b_Controller";
            using (SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString))
            {
                using (SqlCommand command2 = new SqlCommand(str, connection2))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                    {
                        adapter2.Fill(tbControllers);
                    }
                }
            }
        l_fill:
            int no, serial_, port_;
            string ip_;
            for (int i = 0; i < view.Count; i++)
            {
                no = (int)view[i]["f_ControllerNO"];
                serial_ = (int)view[i]["f_ControllerSN"];
                ip_ = wgTools.SetObjToStr(view[i]["f_IP"]);
                port_ = (int)view[i]["f_Port"];
                if (wgMjController.isFaceFeasible(serial_))
                {
                    cnt_serials.Add(serial_);
                    cnt_ips.Add(ip_);
                    cnt_ports.Add(port_);

                    cmbController.Items.Add(no.ToString() + " : SN" + serial_.ToString());
                }
            }
            if (cmbController.Items.Count != 0)
                cmbController.SelectedIndex = 0;
        }

        private void dfrmRegisterFace_Load(object sender, EventArgs e)
        {
            fillControllerList();

            // Uncheck duress
			setButtons(true);

            // Render current states
            if (_face_templ != null)
            {
                setButtons(false);
                PictureBox pic = picFace;
                if (pic != null)
                    pic.Image = global::Properties.Resources.face;
            }

			delegateDrawPortrait_ = new delegateDrawPortrait(DrawPortrait);
			delegateReport_ = new delegateReport(reportResult);
			delegateSetButtons_ = new delegateSetButtons(setButtons);

			// for jpeg conversion
			dotNet(1);

			gImageBits = new byte[ImageWidth * ImageHeight];
			addrOfImageBits = GCHandle.Alloc(gImageBits, GCHandleType.Pinned).AddrOfPinnedObject();

			rgbBuffer = new byte[ImageWidth * ImageHeight * 3];
			addrOfBuffer = GCHandle.Alloc(rgbBuffer, GCHandleType.Pinned).AddrOfPinnedObject();

			capturing = false;

			initH264();

			// Camera Setting
			RegistryKey regKey;
			UInt32 defAddr = (100 << 24) + (100 << 16) + (4 << 8) + 193;

			regKey = Registry.CurrentUser.CreateSubKey("Software\\KPT\\BioAccess");
			ipAddr = (UInt32)(Int32)regKey.GetValue("IPAddress", (Int32)defAddr, RegistryValueOptions.None);
			cmdPort = (UInt16)(Int32)regKey.GetValue("CommandPort", 34567, RegistryValueOptions.None);
			dataPort = (UInt16)(Int32)regKey.GetValue("DataPort", 34567, RegistryValueOptions.None);
		}

		private void StartCamera()
		{
			stopEvent.Reset();

			Byte a = (Byte)((ipAddr >> 24) & 255);
			Byte b = (Byte)((ipAddr >> 16) & 255);
			Byte c = (Byte)((ipAddr >> 8) & 255);
			Byte d = (Byte)((ipAddr) & 255);
			string ipString = a.ToString() + "." + b.ToString() + "." + c.ToString() + "." + d.ToString();
			camera = new Camera(this, ipString, cmdPort, dataPort);

			renderThread = new Thread(new ParameterizedThreadStart(VerifyThreadProc));
			renderThread.Name = "RenderThread";
			renderThread.Start(this);
		}

        private void btnOK_Click(object sender, EventArgs e)
        {
			if (portrait != null && _face_templ != null)
				portrait.Save("PHOTO\\" + _face_templ.uid.ToString("D4") + ".jpg",
					ImageFormat.Jpeg);
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        /************************************************************************/
        /* Enroll Face                                                             */
        /************************************************************************/
        private void clearImage()
        {
            PictureBox pic = picFace;
            if (pic != null)
                pic.Image = null;
        }

        private void report_result(int error)
        {
            lblResult.Text = wgGlobal.getErrorString(error);
            Application.DoEvents();
        }

        private void delFace()
        {
            _face_templ = null;

			btnScan.Enabled = true;
			btnDelete.Enabled = false;
			btnScan.Text = CommonStr.strCapture;
			lblResult.Text = CommonStr.strFaceRemovedSuccessfully;
			Application.DoEvents();

			capturing = false;
        }

        public MjFaceTempl face_templ
        {
            get { return _face_templ; }
            set { _face_templ = value; }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
			if (capturing)
			{
				btnScan.Text = CommonStr.strCapture;
				StopCamera();
			}
			else
			{
				btnScan.Text = CommonStr.strPause;
				StartCamera();
			}
			capturing = !capturing;
		}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            delFace();
            clearImage();
        }

		private void dfrmRegisterFace_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopCamera();
			deinitH264();
		}

		private void VerifyThreadProc(Object param)
		{
			int result;
			dfrmRegisterFace form = (dfrmRegisterFace)param;
			int templData, templSize, count = 0;
			int fx, fy, fw, fh, roll;
			int ex1, ey1, ex2, ey2;
			//byte[] frame;
			byte[] buffer = new byte[ImageWidth * ImageHeight];

			fx = fy = fw = fh = roll = ex1 = ey1 = ex2 = ey2 = -1;

			WaitHandle[] handles = new WaitHandle[] {
				Camera.readyEvent,
				stopEvent,
			};

			while (true)
			{
				if (WaitHandle.WaitAny(handles) == 1)
					break;

				imageMutex.WaitOne();
				Marshal.Copy((IntPtr)addrOfImageBits.ToInt32(), buffer, 0, gImageBits.Length);
				imageMutex.ReleaseMutex();

				templData = templSize = 0;
				result = Face.ExtractTemplate(buffer, ImageWidth, ImageHeight,
					ref fx, ref fy, ref fw, ref fh, ref roll,
					ref ex1, ref ey1, ref ex2, ref ey2, 
					ref templData, ref templSize);
				/*
				BeginInvoke(delegateDrawPortrait_,
					new Rectangle(fx, fy, fw, fh),
					new Point(ex1, ey1),
					new Point(ex2, ey2));*/
				if (result == 0) ++count; else count = 0;
				if (count == ExtractionCount)
				{
					count = 0;
					byte[] data = new byte[templSize];
					Marshal.Copy((IntPtr)templData, data, 0, templSize);
					_face_templ = new MjFaceTempl(uid, data);
					Face.FreeTemplate(templData);

					form.BeginInvoke(form.delegateReport_, 0);
					form.BeginInvoke(form.delegateSetButtons_, false);

					StopCamera();
					return;
				}
				if (result == 0)
					result = wgTools.ErrorCode.ERR_DONT_MOVE_FACE;
				form.BeginInvoke(form.delegateReport_, result);
				Face.FreeTemplate(templData);

				camera.GetReady();
			}
		}

		void StopCamera()
		{
			stopEvent.Set();
			if (camera != null)
				camera.StopCapture();
		}

		public bool RenderPortrait(byte[] frame, int len, int count)
		{
			bool ret = false;
			int addr1, addr2;
			try
			{
				// Convert JPEG to Bitmap data
				IntPtr frameHandle = GCHandle.Alloc(frame, 
					System.Runtime.InteropServices.GCHandleType.Pinned).AddrOfPinnedObject();
				addr1 = frameHandle.ToInt32();
				addr2 = addrOfBuffer.ToInt32();
				if (decodeH264(ref addr1, len, ref addr2) < 0)
					return false;
				if (count > 0)
					return false;
				// Convert to gray image
				imageMutex.WaitOne();
				addr1 = addrOfImageBits.ToInt32();
				convertGrayImage(ref addr2, ImageWidth, ImageHeight, ref addr1);
				imageMutex.ReleaseMutex();

				// Render bitmap data
				portrait = new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format24bppRgb);
				BitmapData bmpData = portrait.LockBits(new Rectangle(0, 0, ImageWidth, ImageHeight),
					ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				IntPtr ptr = bmpData.Scan0;
				int bytes = Math.Abs(bmpData.Stride) * ImageHeight;
				Marshal.Copy(rgbBuffer, 0, ptr, bytes);
				// Unlock bitmap data
				portrait.UnlockBits(bmpData);
				clonned = (Bitmap)portrait.Clone();
				BeginInvoke(delegateDrawPortrait_, null, null, null);
				ret = true;
			}
			catch
			{
				portrait = null;
			}

			return ret;
		}

		delegate void delegateDrawPortrait(Rectangle face, Point eye1, Point eye2);
		delegateDrawPortrait delegateDrawPortrait_;
		void DrawPortrait(Rectangle face, Point eye1, Point eye2)
		{
			Graphics graphics = Graphics.FromImage(clonned);
			
			// draw face rectangle
			Pen pen = new Pen(Color.Red, 2);
			const int width = 260;
			const int height = 360;
			
			int left = (ImageWidth - width) / 2;
			int top = (ImageHeight - height) / 2;
			int right = left + width;
			int bottom = top + height;

			Point[] points = {
				new Point(left, top), new Point(left, bottom), 
				new Point(right, bottom), new Point(right, top),
				new Point(left, top)};

			graphics.DrawLines(pen, points);

			/*
			if (face.Left != -1 && face.Top != -1 && face.Width != -1 && face.Width != -1)
				DrawFace(graphics, face);
			if (eye1.X != -1 && eye1.Y != -1)
				DrawEye(graphics, eye1);
			if (eye2.X != -1 && eye2.Y != -1)
				DrawEye(graphics, eye2);*/
			picFace.Image = clonned;
		}

		void DrawFace(Graphics graphics, Rectangle rc)
		{
			// draw face rectangle
			Pen pen = new Pen(Color.Green, 1);

			Point[] points = {
				new Point(rc.Left, rc.Top), new Point(rc.Left, rc.Bottom), 
				new Point(rc.Right, rc.Bottom), new Point(rc.Right, rc.Top),
				new Point(rc.Left, rc.Top)
			};
			graphics.DrawLines(pen, points);
		}

		void DrawEye(Graphics graphics, Point pt)
		{
			// point eyes
			Pen pen = new Pen(Color.Blue, 1);

			graphics.DrawLine(pen, pt.X - 10, pt.Y, pt.X + 10, pt.Y);
			graphics.DrawLine(pen, pt.X, pt.Y - 10, pt.X, pt.Y + 10);
		}

		private void mnuCameraSetting_Click(object sender, EventArgs e)
		{
			using (frmCameraSetting cameraSetting = new frmCameraSetting(ipAddr, cmdPort, dataPort))
			{
				if (cameraSetting.ShowDialog(this) == DialogResult.OK)
				{
					ipAddr = cameraSetting.ipAddr;
					cmdPort = cameraSetting.cmdPort;
					dataPort = cameraSetting.dataPort;
				}
			}
		}
    }

	class t_ipcam_pkt
	{
		public uint hdr;
		public uint streamId;
		public uint unk0;
		public ushort unk1;
		public ushort cmdID;
		public uint len;

		public t_ipcam_pkt(byte[] buffer)
		{
			hdr = BitConverter.ToUInt32(buffer, 0);
			streamId = BitConverter.ToUInt32(buffer, 4);
			unk0 = BitConverter.ToUInt32(buffer, 8);
			unk1 = BitConverter.ToUInt16(buffer, 12);
			cmdID = BitConverter.ToUInt16(buffer, 14);
			len = BitConverter.ToUInt32(buffer, 16);
		}

		public byte[] toBytes()
		{
			byte[] dest = new byte[getSize()];
			Array.Copy(BitConverter.GetBytes(hdr), 0, dest, 0, 4);
			Array.Copy(BitConverter.GetBytes(streamId), 0, dest, 4, 4);
			Array.Copy(BitConverter.GetBytes(unk0), 0, dest, 8, 4);
			Array.Copy(BitConverter.GetBytes(unk1), 0, dest, 12, 2);
			Array.Copy(BitConverter.GetBytes(cmdID), 0, dest, 14, 2);
			Array.Copy(BitConverter.GetBytes(len), 0, dest, 16, 4);
			return dest;
		}

		public static int getSize()
		{
			return 20;
		}
	}

	class Camera
	{
		[DllImport("kernel32")]
		public static extern int GetTickCount();

		enum State {
			None,
			FirstRsp,
			SecondRsp,
			Working,
		};

		static Form parent;
		static String ip;
		static ushort dataPort;
		static ushort cmdPort;

		static TcpClient cmdClient, dataClient;
		static NetworkStream cmdStream, dataStream;
		static Thread thread;
		static bool stopping;
		static ManualResetEvent cmdEvent, dataEvent;
		static ManualResetEvent termEvent, stopEvent;

		public static ManualResetEvent readyEvent;

		public Camera() { }
		public Camera(Form parent_, String ip_, ushort cmdPort_, ushort dataPort_)
		{
			parent = parent_;
			ip = ip_;
			cmdPort = cmdPort_;
			dataPort = dataPort_;

			cmdClient = dataClient = null;
			cmdStream = dataStream = null;

			readyEvent = new ManualResetEvent(false);
			cmdEvent = new ManualResetEvent(false);
			dataEvent = new ManualResetEvent(false);
			termEvent = new ManualResetEvent(false);
			stopEvent = new ManualResetEvent(false);
			
			thread = new Thread(new ParameterizedThreadStart(StartCapture));
			thread.Name = "CaptureThread";
			thread.Start();

			stopping = false;
		}

		private static int send_cam_cmd(NetworkStream stream, byte[] data, uint streamId, byte[] frame)
		{
			t_ipcam_pkt pkt0 = new t_ipcam_pkt(data);
			if (streamId != 0)
			{
				string szStreamID = "0x" + streamId.ToString("X8");
				pkt0.streamId = streamId;
				Array.Copy(pkt0.toBytes(), data, t_ipcam_pkt.getSize());
				char[] idString = szStreamID.ToCharArray();
				for (int i = 0; i < 10; i++)
					data[pkt0.len + 6 + i] = (byte)idString[i];
			}
			stream.Write(data, 0, data.Length);
			int nRecvBytes = stream.Read(frame, 0, frame.Length);
			if (nRecvBytes < t_ipcam_pkt.getSize())
				return -1;
			nRecvBytes -= t_ipcam_pkt.getSize();
			t_ipcam_pkt pkt1 = new t_ipcam_pkt(frame);
			if (pkt1.hdr != 0x000001FF)
				return -1;
			while (pkt1.len > (uint)nRecvBytes)
			{
				pkt1.len -= (uint)nRecvBytes;
				nRecvBytes = stream.Read(frame, 0, frame.Length);
				if (nRecvBytes <= 0)
					return -1;
			}
			if (pkt0.cmdID + 1 != pkt1.cmdID)
				return -1;
			if (streamId != 0 && pkt1.streamId != streamId)
				return -1;
			streamId = pkt1.streamId;
			return (int)streamId;
		}

		static void StartCapture(Object param)
		{
			int i;
			State state = State.None;
			int tAlive = 0;

			int start_time, prev_time;
			int readLen;
			byte[] pkt0 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xe8, 0x03, 
				0x64, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x45, 0x6e, 0x63, 0x72, 0x79, 0x70, 0x74, 0x54, 0x79, 
				0x70, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x4d, 0x44, 0x35, 0x22, 0x2c, 0x20, 0x22, 0x4c, 0x6f, 
				0x67, 0x69, 0x6e, 0x54, 0x79, 0x70, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x44, 0x56, 0x52, 0x49, 
				0x50, 0x2d, 0x57, 0x65, 0x62, 0x22, 0x2c, 0x20, 0x22, 0x50, 0x61, 0x73, 0x73, 0x57, 0x6f, 0x72, 
				0x64, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x74, 0x6c, 0x4a, 0x77, 0x70, 0x62, 0x6f, 0x36, 0x22, 0x2c, 
				0x20, 0x22, 0x55, 0x73, 0x65, 0x72, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x61, 
				0x64, 0x6d, 0x69, 0x6e, 0x22, 0x20, 0x7d, 0x0a
			};
			byte[] pkt1 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xfc, 0x03, 
				0x36, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x49, 0x6e, 0x66, 0x6f, 0x22, 0x2c, 0x20, 0x22, 0x53, 0x65, 
				0x73, 0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x30, 0x78, 0x30, 0x30, 
				0x30, 0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 0x20, 0x7d, 0x0a
			};
			byte[] pkt2 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xdc, 0x05, 
				0x2c, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x22, 0x2c, 0x20, 0x22, 0x53, 0x65, 0x73, 0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 0x22, 0x20, 0x3a, 
				0x20, 0x22, 0x30, 0x78, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 0x20, 0x7d, 0x0a
			};
			byte[] pkt3 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x04, 
				0x38, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x43, 0x68, 0x61, 0x6e, 0x6e, 0x65, 0x6c, 0x54, 0x69, 0x74, 0x6c, 0x65, 0x22, 0x2c, 0x20, 0x22, 
				0x53, 0x65, 0x73, 0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x30, 0x78, 
				0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 0x20, 0x7d, 0x0a
			};

			byte[] pkt4 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xee, 0x03, 
				0x35, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x4b, 0x65, 0x65, 0x70, 0x41, 0x6c, 0x69, 0x76, 0x65, 0x22, 0x2c, 0x20, 0x22, 0x53, 0x65, 0x73, 
				0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x30, 0x78, 0x30, 0x30, 0x30, 
				0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 0x20, 0x7d, 0x0a
			};

			byte[] pkt5 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x85, 0x05, 
				0xbf, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x4f, 0x50, 0x4d, 0x6f, 0x6e, 0x69, 0x74, 0x6f, 0x72, 0x22, 0x2c, 0x20, 0x22, 0x4f, 0x50, 0x4d, 
				0x6f, 0x6e, 0x69, 0x74, 0x6f, 0x72, 0x22, 0x20, 0x3a, 0x20, 0x7b, 0x20, 0x22, 0x41, 0x63, 0x74, 
				0x69, 0x6f, 0x6e, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x43, 0x6c, 0x61, 0x69, 0x6d, 0x22, 0x2c, 0x20, 
				0x22, 0x50, 0x61, 0x72, 0x61, 0x6d, 0x65, 0x74, 0x65, 0x72, 0x22, 0x20, 0x3a, 0x20, 0x7b, 0x20, 
				0x22, 0x43, 0x68, 0x61, 0x6e, 0x6e, 0x65, 0x6c, 0x22, 0x20, 0x3a, 0x20, 0x30, 0x2c, 0x20, 0x22, 
				0x43, 0x6f, 0x6d, 0x62, 0x69, 0x6e, 0x4d, 0x6f, 0x64, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x4e, 
				0x4f, 0x4e, 0x45, 0x22, 0x2c, 0x20, 0x22, 0x53, 0x74, 0x72, 0x65, 0x61, 0x6d, 0x54, 0x79, 0x70, 
				0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x4d, 0x61, 0x69, 0x6e, 0x22, 0x2c, 0x20, 0x22, 0x54, 0x72, 
				0x61, 0x6e, 0x73, 0x4d, 0x6f, 0x64, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x54, 0x43, 0x50, 0x22, 
				0x20, 0x7d, 0x20, 0x7d, 0x2c, 0x20, 0x22, 0x53, 0x65, 0x73, 0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 
				0x22, 0x20, 0x3a, 0x20, 0x22, 0x30, 0x78, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 
				0x20, 0x7d, 0x0a
			};
			byte[] pkt6 = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x82, 0x05, 
				0xbf, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x4f, 0x50, 0x4d, 0x6f, 0x6e, 0x69, 0x74, 0x6f, 0x72, 0x22, 0x2c, 0x20, 0x22, 0x4f, 0x50, 0x4d, 
				0x6f, 0x6e, 0x69, 0x74, 0x6f, 0x72, 0x22, 0x20, 0x3a, 0x20, 0x7b, 0x20, 0x22, 0x41, 0x63, 0x74, 
				0x69, 0x6f, 0x6e, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x53, 0x74, 0x61, 0x72, 0x74, 0x22, 0x2c, 0x20, 
				0x22, 0x50, 0x61, 0x72, 0x61, 0x6d, 0x65, 0x74, 0x65, 0x72, 0x22, 0x20, 0x3a, 0x20, 0x7b, 0x20, 
				0x22, 0x43, 0x68, 0x61, 0x6e, 0x6e, 0x65, 0x6c, 0x22, 0x20, 0x3a, 0x20, 0x30, 0x2c, 0x20, 0x22, 
				0x43, 0x6f, 0x6d, 0x62, 0x69, 0x6e, 0x4d, 0x6f, 0x64, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x4e, 
				0x4f, 0x4e, 0x45, 0x22, 0x2c, 0x20, 0x22, 0x53, 0x74, 0x72, 0x65, 0x61, 0x6d, 0x54, 0x79, 0x70, 
				0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x4d, 0x61, 0x69, 0x6e, 0x22, 0x2c, 0x20, 0x22, 0x54, 0x72, 
				0x61, 0x6e, 0x73, 0x4d, 0x6f, 0x64, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x54, 0x43, 0x50, 0x22, 
				0x20, 0x7d, 0x20, 0x7d, 0x2c, 0x20, 0x22, 0x53, 0x65, 0x73, 0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 
				0x22, 0x20, 0x3a, 0x20, 0x22, 0x30, 0x78, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 
				0x20, 0x7d, 0x0a
			};
			byte[][] pkts = new byte[][] {
				pkt0, pkt1, pkt2, pkt3, pkt4, pkt5, pkt6
			};
			byte[] pkt_alive = new byte[] {
				0xff, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xee, 0x03, 
				0x35, 0x00, 0x00, 0x00, 0x7b, 0x20, 0x22, 0x4e, 0x61, 0x6d, 0x65, 0x22, 0x20, 0x3a, 0x20, 0x22, 
				0x4b, 0x65, 0x65, 0x70, 0x41, 0x6c, 0x69, 0x76, 0x65, 0x22, 0x2c, 0x20, 0x22, 0x53, 0x65, 0x73, 
				0x73, 0x69, 0x6f, 0x6e, 0x49, 0x44, 0x22, 0x20, 0x3a, 0x20, 0x22, 0x30, 0x78, 0x30, 0x30, 0x30, 
				0x30, 0x30, 0x30, 0x30, 0x38, 0x22, 0x20, 0x7d, 0x0a
			};
			const int MAX_FRAME_SIZE = 200*1024;
			const int GS_NET_VIDEO_BUFFER_SIZE = 1024*1024;
			byte[] frame = new byte[MAX_FRAME_SIZE];
			CameraBuffer buffer = new CameraBuffer();
			buffer.init(GS_NET_VIDEO_BUFFER_SIZE * 2, 58);
			start_time = prev_time = GetTickCount();
			int streamId = 0;
			while (!stopping)
			{
				try
				{
					switch (state)
					{
						case State.None:
							{
								DisconnectCmdClient();
								DisconnectDataClient();

								ConnectCmdClient();
								if (cmdClient.Connected)
								{
									cmdStream = cmdClient.GetStream();
									streamId = 0;
									for (i = 0; i < 5; i++)
									{
										streamId = send_cam_cmd(cmdStream, pkts[i], (uint)streamId, frame);
										if (streamId < 0)
											break;
									}
									if (i < 5)
										break;
									state = State.FirstRsp;
								}
								break;
							}
						case State.FirstRsp:
							{
								state = State.None;
								if (!cmdClient.Connected)
									break;
								ConnectDataClient();
								if (!dataClient.Connected)
								{
									state = State.None;
									break;
								}
								dataStream = dataClient.GetStream();
								streamId = send_cam_cmd(dataStream, pkts[5], (uint)streamId, frame);
								if (streamId < 0)
									break;
								streamId = send_cam_cmd(cmdStream, pkts[6], (uint)streamId, frame);
								if (streamId < 0)
									break;
								state = State.Working;
								tAlive = GetTickCount();
								break;
							}
						case State.Working:
							{
								cmdStream.BeginRead(frame, 0, frame.Length,
									new AsyncCallback(OnRead), cmdStream);
								if (GetTickCount() - tAlive > 20000)
								{
									cmdStream.Write(pkt_alive, 0, pkt_alive.Length);
									tAlive = GetTickCount();
								}
								if (!cmdClient.Connected || !dataClient.Connected)
								{
									state = State.None;
									break;
								}
								if ((readLen = dataStream.Read(frame, 0, frame.Length)) == 0)
									break;
								buffer.append(frame, readLen);
								int count = 0;
								while (true)
								{
									readLen = buffer.getFrameBuffer(frame);
									if (readLen <= 1)
										break;
									if (((dfrmRegisterFace)parent).RenderPortrait(frame, readLen, count))
										readyEvent.Set();
									count++;
								}
								break;
							}
						default:
							break;
					}
				}
				catch
				{

				}
			}
			termEvent.Set();
		}
		public void StopCapture()
		{
			stopEvent.Set();

			stopping = true;
			DisconnectCmdClient();
			DisconnectDataClient();
			termEvent.WaitOne();
		}
		static void ConnectCmdClient()
		{
			cmdEvent.Reset();
			OpenCmdPort();
			WaitHandle.WaitAny(new WaitHandle[]{stopEvent, cmdEvent});
		}
		static void ConnectDataClient()
		{
			dataEvent.Reset();
			OpenDataPort();
			WaitHandle.WaitAny(new WaitHandle[]{stopEvent, dataEvent});
		}
		static void OnConnect(IAsyncResult ar)
		{
			TcpClient client = (TcpClient)ar.AsyncState;
			if (client == cmdClient)
				cmdEvent.Set();
			else if (client == dataClient)
				dataEvent.Set();
		}
		static void OnRead(IAsyncResult ar)
		{
			try
			{
				((NetworkStream)ar.AsyncState).EndRead(ar);
			}
			catch
			{
			}
		}
		static void OpenCmdPort()
		{
			cmdClient = new TcpClient(AddressFamily.InterNetwork);
			cmdClient.BeginConnect(ip, cmdPort, new AsyncCallback(OnConnect), cmdClient);
		}
		static void DisconnectCmdClient()
		{
			try
			{
				if (cmdStream != null)
				{
					cmdStream.Close();
					cmdStream = null;
				}
				if (cmdClient != null)
				{
					cmdClient.Close();
					cmdClient = null;
				}
			}
			catch { }
		}
		static void OpenDataPort()
		{
			dataClient = new TcpClient(AddressFamily.InterNetwork);
			dataClient.BeginConnect(ip, dataPort, new AsyncCallback(OnConnect), dataClient);
		}
		static void DisconnectDataClient()
		{
			try
			{
				if (dataStream != null)
				{
					dataStream.Close();
					dataStream = null;
				}
				if (dataClient != null)
				{
					dataClient.Close();
					dataClient = null;
				}
			}
			catch { }
		}
		public void GetReady()
		{
			readyEvent.Reset();
		}
	}

	class CameraBuffer
	{
		Mutex mutex;
		const int gs_video_OneFrame_MaxSize = 300 * 1024;
		byte[] m_buffer = null;
		int read_end;

		int m_nIdentifier_size;
		int m_size_opset;
		int m_max_memory_size;

		int m_frame_size;
		int m_frame_start_pos;
		
		public CameraBuffer()
		{
			m_frame_size = 0;
			mutex = new Mutex();
		}

		public void init(int buf_size, int size_opset)
		{
			m_buffer = new byte[buf_size];
			m_size_opset = size_opset;

			read_end = 0;
			m_max_memory_size = buf_size;
		}

		byte[] ident0 = new byte[] {0x00, 0x00, 0x01, 0xFC};
		byte[] ident1 = new byte[] {0x00, 0x00, 0x01, 0xFD};
		byte[] sig_start = new byte[] {0xFF, 0x01, 0x00, 0x00};

		bool isMatched(byte[] s1, int index, byte[] s2)
		{
			int i;
			for (i = 0; i < s2.Length; i++)
				if (s1[index+i] != s2[i])
					break;
			return (i >= s2.Length);
		}

		int gMemFind(byte[] buf1, int nSizeBuf1, byte[] buf2, int nStart)
		{

			mutex.WaitOne();

			int i = nStart;
			
			while (i < nSizeBuf1)
			{
				if (isMatched(buf1, i, buf2))
				{
					mutex.ReleaseMutex();
					return i;
				}
				i++;
			}
			mutex.ReleaseMutex();
			return -1;
		}

		public int append(byte[] buf, int size)
		{
			if (read_end + size >= m_max_memory_size)
			{
				read_end = 0;
				return 0;
			}

			Array.Copy(buf, 0, m_buffer, read_end, size);
			read_end+= size;
			int sig_pos = 0;
			do 
			{
				sig_pos = gMemFind(m_buffer, read_end, sig_start, sig_pos);
				if (sig_pos >= 0 && sig_pos + 20 <= read_end)
				{
					Array.Copy(m_buffer, sig_pos + 20, m_buffer, sig_pos, read_end - sig_pos - 20);
					read_end -= 20;
				}
				else
				{
					break;
				}
			} while (true);
			return 1;
		}

		int findSize()
		{
			int search_first0, search_first1;
			m_nIdentifier_size = ident0.Length;

			if (m_nIdentifier_size >= read_end)
				return -1;

			search_first0 = gMemFind(m_buffer, read_end, ident0, 0);
			search_first1 = gMemFind(m_buffer, read_end, ident1, 0);

			if (search_first0 < 0 && search_first1 < 0)
				return -1;
			if (search_first0 < 0 || (search_first1 >= 0 && search_first0 > search_first1))
			{
				if (search_first1 + 8 >= read_end)
					return -1;
				m_frame_size = BitConverter.ToInt32(m_buffer, search_first1 + 4) + 8;
				search_first0 = search_first1; // get pkt_size
			}
			else
			{
				if (search_first0 + 16 >= read_end)
					return -1;
				m_frame_size = BitConverter.ToInt32(m_buffer, search_first0 + 12) + 16;
			}
			m_frame_start_pos = search_first0; // get pkt_size

 			if (m_frame_size <= 10 || m_frame_size > gs_video_OneFrame_MaxSize)
 			{
				read_end = 0;
 				return -1;
 			}

			return m_frame_size;
		}

		public int getFrameBuffer(byte[] buf)
		{
			int read_pos;

			if (findSize() < 0)
				return -1;

			read_pos = m_frame_start_pos + m_frame_size;

			if (read_pos > read_end)
				return -1;

			Array.Copy(m_buffer, m_frame_start_pos, buf, 0, m_frame_size);
			//memcpy(buf, &m_buffer[m_frame_start_pos], m_frame_size);
			read_end -= read_pos;
			Array.Copy(m_buffer, read_pos, m_buffer, 0, read_end);

			return m_frame_size;
		}
	};
}
