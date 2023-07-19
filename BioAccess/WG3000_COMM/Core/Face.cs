using System;
using System.Runtime.InteropServices;

namespace WG3000_COMM.Core
{
	public class Face
	{
		[DllImportAttribute("SBFaceSDK.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern int SBExtractForEnroll(ref int bits, 
			int width, int height,
			ref int fx, ref int fy, ref int fw, ref int fh, ref int roll,
			ref int ex1, ref int ey1, ref int ex2, ref int ey2,
			ref int templData, ref int templSize);
		[DllImportAttribute("SBFaceSDK.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern void SBFreeTemplate(int templData);
		[DllImportAttribute("SBFaceSDK.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
		public static extern void SBDotNet(int dotNet);

		public static int ExtractTemplate(byte[] bits, int width, int height,
			ref int fx, ref int fy, ref int fw, ref int fh, ref int roll,
			ref int ex1, ref int ey1, ref int ex2, ref int ey2,
			ref int templData, ref int templSize)
		{
			SBDotNet(1);

			GCHandle gh = GCHandle.Alloc(bits, GCHandleType.Pinned);
			int addr = gh.AddrOfPinnedObject().ToInt32();

			return SBExtractForEnroll(ref addr, width, height,
				ref fx, ref fy, ref fw, ref fh, ref roll,
				ref ex1, ref ey1, ref ex2, ref ey2,
				ref templData, ref templSize);
		}

		public static void FreeTemplate(int templData)
		{
			if (templData != 0)
				SBFreeTemplate(templData);
		}

		private static bool IsAccessDB()
		{
			bool isAccessDB = false;
			string str = wgAppConfig.GetKeyVal("dbConnection");
			if (string.IsNullOrEmpty(str))
				isAccessDB = true;
			else if ((str.ToUpper().IndexOf("DATA SOURCE") >= 0) &&
				(str.ToUpper().IndexOf(".OLEDB") < 0))
				isAccessDB = false;
			else
				isAccessDB = true;

			return isAccessDB;
		}
	}
}
