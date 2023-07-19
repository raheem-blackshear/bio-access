namespace WG3000_COMM.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using WG3000_COMM.ResStrings;

    internal class wgGlobal
    {
        public const int CONTROLLERID_MAX_PARTITIONNUM = 0x3e7;
        public const string ExtendFunction_Password = "5678";
        public const int MaxPrivilegeCount_Stat = 0x1e8480;
        public const int Param_ActiveAntibackShare = 0x3e;
        public const int Param_ActiveFireSignalShare = 60;
        public const int Param_ActiveInterlockShare = 0x3d;
        public const int Param_ConsumerUpdateLog = 50;
        public const int Param_CreatedPartition = 0x35;
        public const int Param_EarliestTimeAsOndutyForNormalAttendance = 0x38;
        public const int Param_InvalidSwipeNotAsAttend = 0x36;
        public const int Param_NormalWorkTime = 0x3a;
        public const int Param_OffDutyLatestTimeForNormalAttendance = 0x37;
        public const int Param_OnlyOnDutyForNormalAttendance = 0x3b;
        public const int Param_OnlyTwoTimesForNormalAttendance = 0x39;
        public const int Param_PatrolAbsentTimeout = 0x1b;
        public const int Param_PatrolAllowTimeout = 0x1c;
        public const int Param_PrivilegeCountByControllerLog = 0x33;
        public const int Param_PrivilegeTotalLog = 0x34;
        public const int ParamLoc_ActivateAccessKeypad = 0x7b;
        public const int ParamLoc_ActivateAntiPassBack = 0x84;
        public const int ParamLoc_ActivateConstMeal = 150;
        public const int ParamLoc_ActivateControllerTaskList = 0x83;
        public const int ParamLoc_ActivateControllerZone = 0x7d;
        public const int ParamLoc_ActivateDontAutoLoadPrivileges = 0x8e;
        public const int ParamLoc_ActivateDontAutoLoadSwipeRecords = 0x8f;
        public const int ParamLoc_ActivateDontDisplayAccessControl = 0x6f;
        public const int ParamLoc_ActivateDontDisplayAttendance = 0x70;
        public const int ParamLoc_ActivateDoorAsSwitch = 0x92;
        public const int ParamLoc_ActivateElevator = 0x90;
        public const int ParamLoc_ActivateFirstCardOpen = 0x87;
        public const int ParamLoc_ActivateHouse = 0x91;
        public const int ParamLoc_ActivateInterLock = 0x85;
        public const int ParamLoc_ActivateLogQuery = 0x67;
        public const int ParamLoc_ActivateMaps = 0x72;
        public const int ParamLoc_ActivateMeeting = 0x95;
        public const int ParamLoc_ActivateMultiCardAccess = 0x86;
        public const int ParamLoc_ActivateOperatorManagement = 0x94;
        public const int ParamLoc_ActivateOtherShiftSchedule = 0x71;
        public const int ParamLoc_ActivatePatrol = 0x97;
        public const int ParamLoc_ActivatePCCheckAccess = 0x89;
        public const int ParamLoc_ActivatePeripheralControl = 0x7c;
        public const int ParamLoc_ActivateRemoteOpenDoor = 0x7a;
        public const int ParamLoc_ActivateTimeProfile = 0x79;
        public const int ParamLoc_ActivateTimeSegLimittedAccess = 0x88;
        public const int ParamLoc_ActivateValidSwipeGap = 0x93;
        public const int ParamLoc_ActivateWarnForceWithCard = 0x8d;
        public const int ParamLoc_RecordButtonEvent = 0x65;
        public const int ParamLoc_RecordDoorStatusEvent = 0x66;
        public const int TIMEOUT_TWOSWIPE_FOR_CHECK_INSIDE_BY_SWIPE = 20;
        public static int TRIGGER_EVENT_4ARM = 0x3800;
        public static int TRIGGER_SOURCE_4ARM = 0x10;

        private wgGlobal()
        {
        }

        public static int ERR_PRIVILEGES_OVER200K
        {
            get
            {
                return -100001;
            }
        }

        public static int ERR_PRIVILEGES_STOPUPLOAD
        {
            get
            {
                return -100002;
            }
        }

        public static int ERR_SWIPERECORD_STOPGET
        {
            get
            {
                return -200002;
            }
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            [DllImport("iphlpapi.dll", ExactSpelling=true)]
            public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
        }

        public static string getErrorString(int error)
        {
            string ret;
            switch (error)
            {
                case wgTools.ErrorCode.ERR_SUCCESS:
                    ret = ""; break;
                case wgTools.ErrorCode.ERR_FAIL:
                    ret = CommonStr.strFailed; break;
                case wgTools.ErrorCode.ERR_NOT_PRESSED:
                    ret = CommonStr.strNotPressedFp; break;
                case wgTools.ErrorCode.ERR_CAP_TIMEOUT:
                    ret = CommonStr.strCaptureTimeout; break;
                case wgTools.ErrorCode.ERR_IDENTIFY:
                    ret = CommonStr.strFingerNotFound; break;
                case wgTools.ErrorCode.ERR_VERIFY:
                    ret = CommonStr.strFingerNotFound; break;
                case wgTools.ErrorCode.ERR_INVALID_ID:
                    ret = CommonStr.strInvalidID; break;
                case wgTools.ErrorCode.ERR_NOT_ENROLLED_ID:
                    ret = CommonStr.strNotEnrolledID; break;
                case wgTools.ErrorCode.ERR_NTH_ERR:
                    ret = CommonStr.strCannotEnroll; break;
                case wgTools.ErrorCode.ERR_BAD_FINGER:
                    ret = CommonStr.strFeatureExtError; break;
                case wgTools.ErrorCode.ERR_MERGE:
                    ret = CommonStr.strCannotEnroll; break;
                case wgTools.ErrorCode.ERR_FINGER_DOUBLED:
                    ret = CommonStr.strFpDuplicated; break;
                //                 case FP_ERR_ENROLLED_POS:
                //                     ret = "Already enrolled index!"; break;
                //                 case FP_ERR_NOT_ENROLLED_POS:
                //                     ret = "Not enrolled index!"; break;
                case wgTools.ErrorCode.DEV_ERR:
                    ret = CommonStr.strDeviceError; break;
                //                 case FP_ERR_FULL_ID_FP:
                //                     ret = "All fingerprints are enrolled for this user!"; break;
                //                 case FP_ERR_INVALID_POS:
                //                     ret = "Invalid backup number!"; break;
                case wgTools.ErrorCode.ERR_DB_IS_FULL:
                    ret = CommonStr.strDatabaseIsFull;/*"Database is full!"*/; break;
                case wgTools.ErrorCode.ERR_TIMEOUT:
                    ret = CommonStr.strTimeout; break;
                case wgTools.ErrorCode.ERR_INVALID_PARAMETER:
                    ret = CommonStr.strInvalidParam; break;
                //                 case FP_ERR_FW_CANT_OPEN:
                //                     ret = "Cannot open firmware file."; break;
                //                 case FP_ERR_FW_TO0_LARGE:
                //                     ret = "Firmware binary file is too large."; break;
                //                 case FP_ERR_FW_BAD_FILE:
                //                     ret = "Invalid firmware binary file."; break;
                default:
                    ret = CommonStr.strUnknownError; break;
            }
            return ret;
        }
    }
}

