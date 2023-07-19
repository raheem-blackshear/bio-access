namespace WG3000_COMM.Core
{
    using System;
    using System.Collections.Generic;

    public class wgMjControllerSwipeRecord
    {
        private bool m_AddressIsReader;
        private byte m_bytRecOption;
        private byte m_bytStatus;
        private int m_currentSwipeTimes;
        private DateTime m_dtRead;
        private int m_eventCategory;
        private int m_floorNo;
        private byte m_iDoorNo;
        private byte m_iReaderNo;
        private bool m_isBiDirection;
        private bool m_isEnterIn;
        private bool m_isPassed;
        private bool m_isRemoteOpen;
        private bool m_isSwipe;
        private bool m_isSwipeLimitted;
        private bool m_isWiegand26;
        private byte m_Reason;
        private SwipeStatusNo m_ReasonNo;
        private SwipeOption m_swipeOption;
        private byte m_swipeStatus;
        private uint m_nUserID;
        private uint m_nPhotoID;
        private byte verifMode_;
        //private ulong m_ulCardID;
        private uint m_ulControllerSN;
        private uint m_ulIndexInDataFlash;
        private byte[] privBytes;
        private const int RecordSizeInDb = 0x30;
        private const int swipeByteSize = 0x10;
        private Dictionary<SwipeStatusNo, string> SwipeStatusDesc;
        private Dictionary<SwipeStatusNo, string> SwipeStatusDesc_Cn;
        
        public wgMjControllerSwipeRecord()
        {
            this.m_isWiegand26 = true;
            this.m_isSwipe = true;
            this.m_floorNo = -1;
            this.m_isPassed = true;
            this.m_iReaderNo = 1;
            this.m_isBiDirection = true;
            this.m_isEnterIn = true;
            this.privBytes = new byte[0x10];
            Dictionary<SwipeStatusNo, string> dictionary = new Dictionary<SwipeStatusNo, string>();
            dictionary.Add(SwipeStatusNo.RecordSwipe, "Verification");
            dictionary.Add(SwipeStatusNo.RecordSwipeClose, "Verification Close");
            dictionary.Add(SwipeStatusNo.RecordSwipeOpen, "Verification Open");
            dictionary.Add(SwipeStatusNo.RecordSwipeWithCount, "Verification Times=");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "Denied Access:PC Control");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "Denied Access:No PRIVILEGE");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "Denied Access: Wrong PASSWORD");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "Denied Access: AntiBack");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "Denied Access: Door Open Group");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "Denied Access: First Card Open");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "Denied Access: Door Set NC");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "Denied Access: InterLock");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "Denied Access: Limited Times");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "Denied Access: Limited Person Indoor");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "Denied Access: Invalid Timezone");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "Denied Access: In Order");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "Denied Access: Verification Gap Limited");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccess, "Denied Access");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "Denied Access: Limited Times With Count");
            dictionary.Add(SwipeStatusNo.RecordPushButton, "Push Button");
            dictionary.Add(SwipeStatusNo.RecordPushButtonOpen, "Push Button Open");
            dictionary.Add(SwipeStatusNo.RecordPushButtonClose, "Push Button Close");
            dictionary.Add(SwipeStatusNo.RecordDoorOpen, "Door Open");
            dictionary.Add(SwipeStatusNo.RecordDoorClosed, "Door Closed");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "Super Password Open Door");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordOpen, "Super Password Open");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordClose, "Super Password Close");
            dictionary.Add(SwipeStatusNo.RecordPowerOn, "Controller Power On");
            dictionary.Add(SwipeStatusNo.RecordReset, "Controller Reset");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "Push Button Invalid: Disable");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "Push Button Invalid: Forced Lock");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "Push Button Invalid: Not On Line");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "Push Button Invalid: InterLock");
            dictionary.Add(SwipeStatusNo.RecordWarnThreat, "Threat");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatOpen, "Threat Open");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatClose, "Threat Close");
            dictionary.Add(SwipeStatusNo.RecordWarnLeftOpen, "Open too long");
            dictionary.Add(SwipeStatusNo.RecordWarnOpenByForce, "Forced Open");
            dictionary.Add(SwipeStatusNo.RecordWarnFire, "Fire");
            dictionary.Add(SwipeStatusNo.RecordWarnCloseByForce, "Forced Close");
            dictionary.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "Guard Against Theft");
            dictionary.Add(SwipeStatusNo.RecordWarn24Hour, "7*24Hour Zone");
            dictionary.Add(SwipeStatusNo.RecordWarnEmergencyCall, "Emergency Call");
            dictionary.Add(SwipeStatusNo.RecordWarnTamper, "Tamper");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor, "Remote Open Door");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "Remote Open Door By USB Reader");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessVM, "Denied Access: Invalid Verification Mode");
            dictionary.Add(SwipeStatusNo.None, "Denied Access");
            this.SwipeStatusDesc = dictionary;
            Dictionary<SwipeStatusNo, string> dictionary2 = new Dictionary<SwipeStatusNo, string>();
            dictionary2.Add(SwipeStatusNo.RecordSwipe, "验证开门");
            dictionary2.Add(SwipeStatusNo.RecordSwipeClose, "验证关");
            dictionary2.Add(SwipeStatusNo.RecordSwipeOpen, "验证开");
            dictionary2.Add(SwipeStatusNo.RecordSwipeWithCount, "验证开门,已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "验证禁止通过: 电脑控制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "验证禁止通过: 没有权限");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "验证禁止通过: 密码不对");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "验证禁止通过: 反潜回");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "验证禁止通过: 多卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "验证禁止通过: 首卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "验证禁止通过: 门为常闭");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "验证禁止通过: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "验证禁止通过: 受验证次数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "验证禁止通过: 门内人数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "验证禁止通过: 卡过期,或不在有效时段,或假期约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "验证禁止通过: 按顺序进出限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "验证禁止通过: 验证间隔约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccess, "验证禁止通过: 原因不明");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "验证禁止通过: 验证次数限制, 已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordPushButton, "按钮开门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonOpen, "按钮开");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonClose, "按钮关");
            dictionary2.Add(SwipeStatusNo.RecordDoorOpen, "门打开[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordDoorClosed, "门关闭[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "超级密码开门");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordOpen, "超级密码开");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordClose, "超级密码关");
            dictionary2.Add(SwipeStatusNo.RecordPowerOn, "控制器上电");
            dictionary2.Add(SwipeStatusNo.RecordReset, "控制器复位");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "按钮不开门: 按钮禁用");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "按钮不开门: 强制关门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "按钮不开门: 门不在线");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "按钮不开门: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreat, "胁迫报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatOpen, "胁迫报警开");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatClose, "胁迫报警关");
            dictionary2.Add(SwipeStatusNo.RecordWarnLeftOpen, "门长时间未关报警[合法开门后]");
            dictionary2.Add(SwipeStatusNo.RecordWarnOpenByForce, "强行闯入报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnFire, "火警");
            dictionary2.Add(SwipeStatusNo.RecordWarnCloseByForce, "强制关门");
            dictionary2.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "防盗报警");
            dictionary2.Add(SwipeStatusNo.RecordWarn24Hour, "烟雾煤气温度报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnEmergencyCall, "紧急呼救报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnTamper, "防拆报警");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor, "操作员远程开门");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "发卡器确定发出的远程开门");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessVM, "验证禁止通过: 验证方式");
            dictionary2.Add(SwipeStatusNo.None, "禁止通过: 原因不明");
            this.SwipeStatusDesc_Cn = dictionary2;
        }

        public wgMjControllerSwipeRecord(string strRecordAll)
        {
            this.m_isWiegand26 = true;
            this.m_isSwipe = true;
            this.m_floorNo = -1;
            this.m_isPassed = true;
            this.m_iReaderNo = 1;
            this.m_isBiDirection = true;
            this.m_isEnterIn = true;
            this.privBytes = new byte[0x10];
            Dictionary<SwipeStatusNo, string> dictionary = new Dictionary<SwipeStatusNo, string>();
            dictionary.Add(SwipeStatusNo.RecordSwipe, "Swipe");
            dictionary.Add(SwipeStatusNo.RecordSwipeClose, "Verification Close");
            dictionary.Add(SwipeStatusNo.RecordSwipeOpen, "Verification Open");
            dictionary.Add(SwipeStatusNo.RecordSwipeWithCount, "Verification Times=");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "Denied Access:PC Control");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "Denied Access:No PRIVILEGE");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "Denied Access: Wrong PASSWORD");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "Denied Access: AntiBack");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "Denied Access: Door Open Group");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "Denied Access: First Card Open");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "Denied Access: Door Set NC");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "Denied Access: InterLock");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "Denied Access: Limited Times");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "Denied Access: Limited Person Indoor");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "Denied Access: Invalid Timezone");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "Denied Access: In Order");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "Denied Access: Verification Gap Limited");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccess, "Denied Access");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "Denied Access: Limited Times With Count");
            dictionary.Add(SwipeStatusNo.RecordPushButton, "Push Button");
            dictionary.Add(SwipeStatusNo.RecordPushButtonOpen, "Push Button Open");
            dictionary.Add(SwipeStatusNo.RecordPushButtonClose, "Push Button Close");
            dictionary.Add(SwipeStatusNo.RecordDoorOpen, "Door Open");
            dictionary.Add(SwipeStatusNo.RecordDoorClosed, "Door Closed");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "Super Password Open Door");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordOpen, "Super Password Open");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordClose, "Super Password Close");
            dictionary.Add(SwipeStatusNo.RecordPowerOn, "Controller Power On");
            dictionary.Add(SwipeStatusNo.RecordReset, "Controller Reset");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "Push Button Invalid: Disable");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "Push Button Invalid: Forced Lock");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "Push Button Invalid: Not On Line");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "Push Button Invalid: InterLock");
            dictionary.Add(SwipeStatusNo.RecordWarnThreat, "Threat");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatOpen, "Threat Open");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatClose, "Threat Close");
            dictionary.Add(SwipeStatusNo.RecordWarnLeftOpen, "Open too long");
            dictionary.Add(SwipeStatusNo.RecordWarnOpenByForce, "Forced Open");
            dictionary.Add(SwipeStatusNo.RecordWarnFire, "Fire");
            dictionary.Add(SwipeStatusNo.RecordWarnCloseByForce, "Forced Close");
            dictionary.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "Guard Against Theft");
            dictionary.Add(SwipeStatusNo.RecordWarn24Hour, "7*24Hour Zone");
            dictionary.Add(SwipeStatusNo.RecordWarnEmergencyCall, "Emergency Call");
            dictionary.Add(SwipeStatusNo.RecordWarnTamper, "Tamper");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor, "Remote Open Door");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "Remote Open Door By USB Reader");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessVM, "Denied Access: Invalid Verification Mode");
            dictionary.Add(SwipeStatusNo.None, "Denied Access");
            this.SwipeStatusDesc = dictionary;
            Dictionary<SwipeStatusNo, string> dictionary2 = new Dictionary<SwipeStatusNo, string>();
            dictionary2.Add(SwipeStatusNo.RecordSwipe, "验证开门");
            dictionary2.Add(SwipeStatusNo.RecordSwipeClose, "验证关");
            dictionary2.Add(SwipeStatusNo.RecordSwipeOpen, "验证开");
            dictionary2.Add(SwipeStatusNo.RecordSwipeWithCount, "验证开门,已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "验证禁止通过: 电脑控制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "验证禁止通过: 没有权限");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "验证禁止通过: 密码不对");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "验证禁止通过: 反潜回");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "验证禁止通过: 多卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "验证禁止通过: 首卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "验证禁止通过: 门为常闭");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "验证禁止通过: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "验证禁止通过: 受验证次数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "验证禁止通过: 门内人数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "验证禁止通过: 卡过期,或不在有效时段,或假期约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "验证禁止通过: 按顺序进出限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "验证禁止通过: 验证间隔约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccess, "验证禁止通过: 原因不明");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "验证禁止通过: 验证次数限制, 已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordPushButton, "按钮开门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonOpen, "按钮开");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonClose, "按钮关");
            dictionary2.Add(SwipeStatusNo.RecordDoorOpen, "门打开[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordDoorClosed, "门关闭[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "超级密码开门");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordOpen, "超级密码开");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordClose, "超级密码关");
            dictionary2.Add(SwipeStatusNo.RecordPowerOn, "控制器上电");
            dictionary2.Add(SwipeStatusNo.RecordReset, "控制器复位");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "按钮不开门: 按钮禁用");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "按钮不开门: 强制关门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "按钮不开门: 门不在线");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "按钮不开门: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreat, "胁迫报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatOpen, "胁迫报警开");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatClose, "胁迫报警关");
            dictionary2.Add(SwipeStatusNo.RecordWarnLeftOpen, "门长时间未关报警[合法开门后]");
            dictionary2.Add(SwipeStatusNo.RecordWarnOpenByForce, "强行闯入报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnFire, "火警");
            dictionary2.Add(SwipeStatusNo.RecordWarnCloseByForce, "强制关门");
            dictionary2.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "防盗报警");
            dictionary2.Add(SwipeStatusNo.RecordWarn24Hour, "烟雾煤气温度报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnEmergencyCall, "紧急呼救报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnTamper, "防拆报警");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor, "操作员远程开门");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "发卡器确定发出的远程开门");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessVM, "验证禁止通过: 验证方式");
            dictionary2.Add(SwipeStatusNo.None, "禁止通过: 原因不明");
            this.SwipeStatusDesc_Cn = dictionary2;
            this.Update(strRecordAll);
        }

        protected internal wgMjControllerSwipeRecord(byte[] rec, uint startIndex)
        {
            this.m_isWiegand26 = true;
            this.m_isSwipe = true;
            this.m_floorNo = -1;
            this.m_isPassed = true;
            this.m_iReaderNo = 1;
            this.m_isBiDirection = true;
            this.m_isEnterIn = true;
            this.privBytes = new byte[0x10];
            Dictionary<SwipeStatusNo, string> dictionary = new Dictionary<SwipeStatusNo, string>();
            dictionary.Add(SwipeStatusNo.RecordSwipe, "Verification");
            dictionary.Add(SwipeStatusNo.RecordSwipeClose, "Verification Close");
            dictionary.Add(SwipeStatusNo.RecordSwipeOpen, "Verification Open");
            dictionary.Add(SwipeStatusNo.RecordSwipeWithCount, "Verification Times=");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "Denied Access:PC Control");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "Denied Access:No PRIVILEGE");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "Denied Access: Wrong PASSWORD");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "Denied Access: AntiBack");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "Denied Access: Door Open Group");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "Denied Access: First Card Open");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "Denied Access: Door Set NC");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "Denied Access: InterLock");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "Denied Access: Limited Times");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "Denied Access: Limited Person Indoor");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "Denied Access: Invalid Timezone");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "Denied Access: In Order");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "Denied Access: Verification Gap Limited");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccess, "Denied Access");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "Denied Access: Limited Times With Count");
            dictionary.Add(SwipeStatusNo.RecordPushButton, "Push Button");
            dictionary.Add(SwipeStatusNo.RecordPushButtonOpen, "Push Button Open");
            dictionary.Add(SwipeStatusNo.RecordPushButtonClose, "Push Button Close");
            dictionary.Add(SwipeStatusNo.RecordDoorOpen, "Door Open");
            dictionary.Add(SwipeStatusNo.RecordDoorClosed, "Door Closed");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "Super Password Open Door");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordOpen, "Super Password Open");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordClose, "Super Password Close");
            dictionary.Add(SwipeStatusNo.RecordPowerOn, "Controller Power On");
            dictionary.Add(SwipeStatusNo.RecordReset, "Controller Reset");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "Push Button Invalid: Disable");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "Push Button Invalid: Forced Lock");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "Push Button Invalid: Not On Line");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "Push Button Invalid: InterLock");
            dictionary.Add(SwipeStatusNo.RecordWarnThreat, "Threat");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatOpen, "Threat Open");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatClose, "Threat Close");
            dictionary.Add(SwipeStatusNo.RecordWarnLeftOpen, "Open too long");
            dictionary.Add(SwipeStatusNo.RecordWarnOpenByForce, "Forced Open");
            dictionary.Add(SwipeStatusNo.RecordWarnFire, "Fire");
            dictionary.Add(SwipeStatusNo.RecordWarnCloseByForce, "Forced Close");
            dictionary.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "Guard Against Theft");
            dictionary.Add(SwipeStatusNo.RecordWarn24Hour, "7*24Hour Zone");
            dictionary.Add(SwipeStatusNo.RecordWarnEmergencyCall, "Emergency Call");
            dictionary.Add(SwipeStatusNo.RecordWarnTamper, "Tamper");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor, "Remote Open Door");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "Remote Open Door By USB Reader");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessVM, "Denied Access: Invalid Verification Mode");
            dictionary.Add(SwipeStatusNo.None, "Denied Access");
            this.SwipeStatusDesc = dictionary;
            Dictionary<SwipeStatusNo, string> dictionary2 = new Dictionary<SwipeStatusNo, string>();
            dictionary2.Add(SwipeStatusNo.RecordSwipe, "验证开门");
            dictionary2.Add(SwipeStatusNo.RecordSwipeClose, "验证关");
            dictionary2.Add(SwipeStatusNo.RecordSwipeOpen, "验证开");
            dictionary2.Add(SwipeStatusNo.RecordSwipeWithCount, "验证开门,已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "验证禁止通过: 电脑控制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "验证禁止通过: 没有权限");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "验证禁止通过: 密码不对");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "验证禁止通过: 反潜回");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "验证禁止通过: 多卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "验证禁止通过: 首卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "验证禁止通过: 门为常闭");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "验证禁止通过: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "验证禁止通过: 受验证次数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "验证禁止通过: 门内人数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "验证禁止通过: 卡过期,或不在有效时段,或假期约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "验证禁止通过: 按顺序进出限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "验证禁止通过: 验证间隔约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccess, "验证禁止通过: 原因不明");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "验证禁止通过: 验证次数限制, 已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordPushButton, "按钮开门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonOpen, "按钮开");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonClose, "按钮关");
            dictionary2.Add(SwipeStatusNo.RecordDoorOpen, "门打开[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordDoorClosed, "门关闭[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "超级密码开门");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordOpen, "超级密码开");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordClose, "超级密码关");
            dictionary2.Add(SwipeStatusNo.RecordPowerOn, "控制器上电");
            dictionary2.Add(SwipeStatusNo.RecordReset, "控制器复位");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "按钮不开门: 按钮禁用");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "按钮不开门: 强制关门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "按钮不开门: 门不在线");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "按钮不开门: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreat, "胁迫报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatOpen, "胁迫报警开");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatClose, "胁迫报警关");
            dictionary2.Add(SwipeStatusNo.RecordWarnLeftOpen, "门长时间未关报警[合法开门后]");
            dictionary2.Add(SwipeStatusNo.RecordWarnOpenByForce, "强行闯入报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnFire, "火警");
            dictionary2.Add(SwipeStatusNo.RecordWarnCloseByForce, "强制关门");
            dictionary2.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "防盗报警");
            dictionary2.Add(SwipeStatusNo.RecordWarn24Hour, "烟雾煤气温度报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnEmergencyCall, "紧急呼救报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnTamper, "防拆报警");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor, "操作员远程开门");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "发卡器确定发出的远程开门");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessVM, "验证禁止通过: 验证方式");
            dictionary2.Add(SwipeStatusNo.None, "禁止通过: 原因不明");
            this.SwipeStatusDesc_Cn = dictionary2;
            this.Update(rec, startIndex, 0, 0);
        }

        protected internal wgMjControllerSwipeRecord(byte[] rec, uint startIndex, uint ControllerSN, uint loc)
        {
            this.m_isWiegand26 = true;
            this.m_isSwipe = true;
            this.m_floorNo = -1;
            this.m_isPassed = true;
            this.m_iReaderNo = 1;
            this.m_isBiDirection = true;
            this.m_isEnterIn = true;
            this.privBytes = new byte[0x10];
            Dictionary<SwipeStatusNo, string> dictionary = new Dictionary<SwipeStatusNo, string>();
            dictionary.Add(SwipeStatusNo.RecordSwipe, "Swipe");
            dictionary.Add(SwipeStatusNo.RecordSwipeClose, "Verification Close");
            dictionary.Add(SwipeStatusNo.RecordSwipeOpen, "Verification Open");
            dictionary.Add(SwipeStatusNo.RecordSwipeWithCount, "Verification Times=");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "Denied Access:PC Control");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "Denied Access:No PRIVILEGE");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "Denied Access: Wrong PASSWORD");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "Denied Access: AntiBack");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "Denied Access: Door Open Group");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "Denied Access: First Card Open");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "Denied Access: Door Set NC");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "Denied Access: InterLock");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "Denied Access: Limited Times");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "Denied Access: Limited Person Indoor");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "Denied Access: Invalid Timezone");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "Denied Access: In Order");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "Denied Access: Verification Gap Limited");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccess, "Denied Access");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "Denied Access: Limited Times With Count");
            dictionary.Add(SwipeStatusNo.RecordPushButton, "Push Button");
            dictionary.Add(SwipeStatusNo.RecordPushButtonOpen, "Push Button Open");
            dictionary.Add(SwipeStatusNo.RecordPushButtonClose, "Push Button Close");
            dictionary.Add(SwipeStatusNo.RecordDoorOpen, "Door Open");
            dictionary.Add(SwipeStatusNo.RecordDoorClosed, "Door Closed");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "Super Password Open Door");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordOpen, "Super Password Open");
            dictionary.Add(SwipeStatusNo.RecordSuperPasswordClose, "Super Password Close");
            dictionary.Add(SwipeStatusNo.RecordPowerOn, "Controller Power On");
            dictionary.Add(SwipeStatusNo.RecordReset, "Controller Reset");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "Push Button Invalid: Disable");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "Push Button Invalid: Forced Lock");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "Push Button Invalid: Not On Line");
            dictionary.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "Push Button Invalid: InterLock");
            dictionary.Add(SwipeStatusNo.RecordWarnThreat, "Threat");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatOpen, "Threat Open");
            dictionary.Add(SwipeStatusNo.RecordWarnThreatClose, "Threat Close");
            dictionary.Add(SwipeStatusNo.RecordWarnLeftOpen, "Open too long");
            dictionary.Add(SwipeStatusNo.RecordWarnOpenByForce, "Forced Open");
            dictionary.Add(SwipeStatusNo.RecordWarnFire, "Fire");
            dictionary.Add(SwipeStatusNo.RecordWarnCloseByForce, "Forced Close");
            dictionary.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "Guard Against Theft");
            dictionary.Add(SwipeStatusNo.RecordWarn24Hour, "7*24Hour Zone");
            dictionary.Add(SwipeStatusNo.RecordWarnEmergencyCall, "Emergency Call");
            dictionary.Add(SwipeStatusNo.RecordWarnTamper, "Tamper");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor, "Remote Open Door");
            dictionary.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "Remote Open Door By USB Reader");
            dictionary.Add(SwipeStatusNo.RecordDeniedAccessVM, "Denied Access: Invalid Verification Mode");
            dictionary.Add(SwipeStatusNo.None, "Denied Access");
            this.SwipeStatusDesc = dictionary;
            Dictionary<SwipeStatusNo, string> dictionary2 = new Dictionary<SwipeStatusNo, string>();
            dictionary2.Add(SwipeStatusNo.RecordSwipe, "验证开门");
            dictionary2.Add(SwipeStatusNo.RecordSwipeClose, "验证关");
            dictionary2.Add(SwipeStatusNo.RecordSwipeOpen, "验证开");
            dictionary2.Add(SwipeStatusNo.RecordSwipeWithCount, "验证开门,已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessPCControl, "验证禁止通过: 电脑控制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE, "验证禁止通过: 没有权限");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessERRPASSWORD, "验证禁止通过: 密码不对");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK, "验证禁止通过: 反潜回");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD, "验证禁止通过: 多卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD, "验证禁止通过: 首卡");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessDOORNC, "验证禁止通过: 门为常闭");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK, "验证禁止通过: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES, "验证禁止通过: 受验证次数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR, "验证禁止通过: 门内人数限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE, "验证禁止通过: 卡过期,或不在有效时段,或假期约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER, "验证禁止通过: 按顺序进出限制");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT, "验证禁止通过: 验证间隔约束");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccess, "验证禁止通过: 原因不明");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT, "验证禁止通过: 验证次数限制, 已刷次数");
            dictionary2.Add(SwipeStatusNo.RecordPushButton, "按钮开门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonOpen, "按钮开");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonClose, "按钮关");
            dictionary2.Add(SwipeStatusNo.RecordDoorOpen, "门打开[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordDoorClosed, "门关闭[门磁信号]");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordDoorOpen, "超级密码开门");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordOpen, "超级密码开");
            dictionary2.Add(SwipeStatusNo.RecordSuperPasswordClose, "超级密码关");
            dictionary2.Add(SwipeStatusNo.RecordPowerOn, "控制器上电");
            dictionary2.Add(SwipeStatusNo.RecordReset, "控制器复位");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_Disable, "按钮不开门: 按钮禁用");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_ForcedLock, "按钮不开门: 强制关门");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_NotOnLine, "按钮不开门: 门不在线");
            dictionary2.Add(SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK, "按钮不开门: 互锁");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreat, "胁迫报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatOpen, "胁迫报警开");
            dictionary2.Add(SwipeStatusNo.RecordWarnThreatClose, "胁迫报警关");
            dictionary2.Add(SwipeStatusNo.RecordWarnLeftOpen, "门长时间未关报警[合法开门后]");
            dictionary2.Add(SwipeStatusNo.RecordWarnOpenByForce, "强行闯入报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnFire, "火警");
            dictionary2.Add(SwipeStatusNo.RecordWarnCloseByForce, "强制关门");
            dictionary2.Add(SwipeStatusNo.RecordWarnGuardAgainstTheft, "防盗报警");
            dictionary2.Add(SwipeStatusNo.RecordWarn24Hour, "烟雾煤气温度报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnEmergencyCall, "紧急呼救报警");
            dictionary2.Add(SwipeStatusNo.RecordWarnTamper, "防拆报警");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor, "操作员远程开门");
            dictionary2.Add(SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader, "发卡器确定发出的远程开门");
            dictionary2.Add(SwipeStatusNo.RecordDeniedAccessVM, "验证禁止通过: 验证方式");
            dictionary2.Add(SwipeStatusNo.None, "禁止通过: 原因不明");
            this.SwipeStatusDesc_Cn = dictionary2;
            this.Update(rec, startIndex, ControllerSN, loc);
        }

        private int categoryGet()
        {
            int num = 0;
            if (this.m_isSwipeLimitted)
            {
                if (this.m_isPassed)
                {
                    num = 2;
                }
                else
                {
                    num = 3;
                }
            }
            else
            {
                num = 0;
                if (!this.m_isSwipe)
                {
                    num += 4;
                }
                if (this.m_isRemoteOpen)
                {
                    num += 2;
                }
                if (!this.m_isPassed)
                {
                    num++;
                }
            }
            this.m_eventCategory = num;
            return num;
        }

        private int reasonGet()
        {
            this.m_ReasonNo = SwipeStatusNo.None;
            if (this.eventCategory == 0)
            {
                if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                {
                    this.m_Reason = 0;
                }
                switch (this.SwipeStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        this.m_ReasonNo = SwipeStatusNo.RecordSwipe;
                        break;

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                        this.m_ReasonNo = SwipeStatusNo.RecordSwipeOpen;
                        break;

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                        this.m_ReasonNo = SwipeStatusNo.RecordSwipeClose;
                        break;
                }
            }
            if (this.eventCategory == 2)
            {
                this.m_ReasonNo = SwipeStatusNo.RecordSwipe;
            }
            if (this.eventCategory == 1)
            {
                switch (this.SwipeStatus)
                {
                    case 0x84:
                    case 0x85:
                    case 0x86:
                    case 0x87:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessPCControl;
                        goto Label_02D8;

                    case 0x90:
                    case 0x91:
                    case 0x92:
                    case 0x93:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessNOPRIVILEGE;
                        goto Label_02D8;

                    case 160:
                    case 0xa1:
                    case 0xa2:
                    case 0xa3:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessERRPASSWORD;
                        goto Label_02D8;

                    case 0xc4:
                    case 0xc5:
                    case 0xc6:
                    case 0xc7:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_ANTIBACK;
                        goto Label_02D8;

                    case 200:
                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_MORECARD;
                        goto Label_02D8;

                    case 0xcc:
                    case 0xcd:
                    case 0xce:
                    case 0xcf:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_FIRSTCARD;
                        goto Label_02D8;

                    case 0xd0:
                    case 0xd1:
                    case 210:
                    case 0xd3:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessDOORNC;
                        goto Label_02D8;

                    case 0xd4:
                    case 0xd5:
                    case 0xd6:
                    case 0xd7:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_INTERLOCK;
                        goto Label_02D8;

                    case 0xd8:
                    case 0xd9:
                    case 0xda:
                    case 0xdb:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES;
                        goto Label_02D8;

                    case 220:
                    case 0xdd:
                    case 0xde:
                    case 0xdf:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR;
                        goto Label_02D8;

                    case 0xe0:
                    case 0xe1:
                    case 0xe2:
                    case 0xe3:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessINVALIDTIMEZONE;
                        goto Label_02D8;

                    case 0xe4:
                    case 0xe5:
                    case 230:
                    case 0xe7:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_INORDER;
                        goto Label_02D8;

                    case 0xe8:
                    case 0xe9:
                    case 0xea:
                    case 0xeb:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT;
                        goto Label_02D8;

                    case 0xec:
                    case 0xed:
                    case 0xee:
                    case 0xef:
                        this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessVM;
                        goto Label_02D8;
                }
                this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccess;
            }
        Label_02D8:
            if (this.eventCategory == 3)
            {
                this.m_ReasonNo = SwipeStatusNo.RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT;
            }
            if (this.eventCategory == 4)
            {
                switch (this.UserID)
                {
                    case 0:
                        this.m_ReasonNo = SwipeStatusNo.RecordPowerOn;
                        break;

                    case 1:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButton;
                        }
                        break;

                    case 2:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButtonOpen;
                        }
                        break;

                    case 3:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButtonClose;
                        }
                        break;

                    case 8:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordDoorOpen;
                        }
                        break;

                    case 9:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordDoorClosed;
                        }
                        break;

                    case 10:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordSuperPasswordDoorOpen;
                        }
                        break;

                    case 11:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordSuperPasswordOpen;
                        }
                        break;

                    case 12:
                        if ((this.SwipeStatus >= 0) && (this.SwipeStatus <= 3))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordSuperPasswordClose;
                        }
                        break;
                }
            }
            if (this.eventCategory == 5)
            {
                switch (this.UserID)
                {
                    case 0:
                        if (this.SwipeStatus != 0)
                        {
                            if ((this.SwipeStatus & 130) == 130)
                            {
                                this.m_ReasonNo = SwipeStatusNo.RecordPowerOn;
                            }
                            else
                            {
                                this.m_ReasonNo = SwipeStatusNo.RecordReset;
                            }
                            break;
                        }
                        this.m_ReasonNo = SwipeStatusNo.RecordPowerOn;
                        break;

                    case 4:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButtonInvalid_Disable;
                        }
                        break;

                    case 5:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButtonInvalid_ForcedLock;
                        }
                        break;

                    case 6:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButtonInvalid_NotOnLine;
                        }
                        break;

                    case 7:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordPushButtonInvalid_INTERLOCK;
                        }
                        break;

                    case 0x51:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnThreat;
                        }
                        break;

                    case 0x52:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnThreatOpen;
                        }
                        break;

                    case 0x53:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnThreatClose;
                        }
                        break;

                    case 0x54:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnLeftOpen;
                        }
                        break;

                    case 0x55:
                        if ((this.SwipeStatus >= 0x80) && (this.SwipeStatus <= 0x83))
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnOpenByForce;
                        }
                        break;

                    case 0x56:
                        if (this.SwipeStatus == 0x80)
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnFire;
                        }
                        break;

                    case 0x57:
                        if (this.SwipeStatus == 0x80)
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnCloseByForce;
                        }
                        break;

                    case 0x58:
                        if (this.SwipeStatus == 0x80)
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnGuardAgainstTheft;
                        }
                        break;

                    case 0x59:
                        if (this.SwipeStatus == 0x80)
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarn24Hour;
                        }
                        break;

                    case 0x60:
                        if (this.SwipeStatus == 0x80)
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnEmergencyCall;
                        }
                        break;
                    case 0x61:
                        if (this.SwipeStatus == 0x80)
                        {
                            this.m_ReasonNo = SwipeStatusNo.RecordWarnTamper;
                        }
                        break;
                }
            }
            if (this.eventCategory == 6)
            {
                switch (this.SwipeStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        this.m_ReasonNo = SwipeStatusNo.RecordRemoteOpenDoor_ByUSBReader;
                        break;

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                        string.Format("DoorNo {0},{1}", this.DoorNo.ToString(), "Remote Open Door By Software ");
                        this.m_ReasonNo = SwipeStatusNo.RecordRemoteOpenDoor;
                        break;
                }
            }
            return 1;
        }

        public string ToDisplaySimpleInfo(bool bChinese)
        {
            string str = "";
            Dictionary<SwipeStatusNo, string> swipeStatusDesc = this.SwipeStatusDesc;
            if (bChinese)
            {
                swipeStatusDesc = this.SwipeStatusDesc_Cn;
            }
            if ((this.m_eventCategory == 0) || (this.m_eventCategory == 2))
            {
                str = string.Format("{0}: \t{1:d}\r\n", "UserID", this.UserID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo);
                if (this.m_isEnterIn)
                {
                    str = str + string.Format(" \t[{0}]\r\n", "In");
                }
                else
                {
                    str = str + string.Format(" \t[{0}]\r\n", "Out");
                }
                str = (str + string.Format("{0}: \t{1:d}\r\n", "ReaderNO", this.m_iReaderNo)) + string.Format("{0}: \t{1}\r\n", "Verification Status", swipeStatusDesc[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if ((this.m_eventCategory == 1) || (this.m_eventCategory == 3))
            {
                str = string.Format("{0}: \t{1:d}\r\n", "UserID", this.UserID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo);
                if (this.m_isEnterIn)
                {
                    str = str + string.Format(" \t[{0}]\r\n", "In");
                }
                else
                {
                    str = str + string.Format(" \t[{0}]\r\n", "Out");
                }
                str = (str + string.Format("{0}: \t{1:d}\r\n", "ReaderNO", this.m_iReaderNo)) + string.Format("{0}: \t{1}\r\n", "Verification Status", swipeStatusDesc[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (this.m_eventCategory == 4)
            {
                str = (string.Format("{0}: \t{1:d}\r\n", "UserID", this.UserID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo)) + string.Format("{0}: \t{1}\r\n", "Verification Status", swipeStatusDesc[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (this.m_eventCategory == 5)
            {
                str = (string.Format("{0}: \t{1:d}\r\n", "UserID", this.UserID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo)) + string.Format("{0}: \t{1}\r\n", "Verification Status", swipeStatusDesc[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (this.m_eventCategory == 6)
            {
                str = (string.Format("{0}: \t{1:d}\r\n", "UserID", this.UserID) + string.Format("{0}: \t{1:d}\r\n", "DoorNO", this.m_iDoorNo)) + string.Format("{0}: \t{1}\r\n", "Verification Status", swipeStatusDesc[this.m_ReasonNo]) + string.Format("{0}: \t{1}\r\n", "Read Date", this.ReadDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return str;
        }

        public string ToStringRaw()
        {
            return string.Format("{0:x8}{1:x8}{2}", this.m_ulControllerSN, this.m_ulIndexInDataFlash, 
                BitConverter.ToString(this.privBytes).Replace("-", ""));
        }

        public void Update(string strRecordAll)
        {
            if (!string.IsNullOrEmpty(strRecordAll) && (strRecordAll.Length == 0x30 || strRecordAll.Length == 0x38))
            {
                byte[] rec = new byte[0x10];
                uint controllerSN = Convert.ToUInt32(strRecordAll.Substring(0, 8), 0x10);
                uint loc = Convert.ToUInt32(strRecordAll.Substring(8, 8), 0x10);
                for (int i = 0; i < 0x10; i++)
                {
                    rec[i] = Convert.ToByte(strRecordAll.Substring(0x10 + (i * 2), 2), 0x10);
                }
                this.Update(rec, 0, controllerSN, loc);
            }
        }

        public void Update(byte[] rec, uint startIndex, uint ControllerSN, uint loc)
        {
            this.m_ulControllerSN = ControllerSN;
            this.m_ulIndexInDataFlash = loc;
            Array.Copy(rec, (long) startIndex, this.privBytes, 0L, 0x10L);
            this.m_nUserID = BitConverter.ToUInt32(this.privBytes, 0);
            this.m_nPhotoID = BitConverter.ToUInt32(this.privBytes, 4);
            //this.m_ulCardID = BitConverter.ToUInt64(this.privBytes, 0);
            this.m_dtRead = wgTools.WgDateToMsDate(this.privBytes, 8);
            SwipeOption option = (SwipeOption) this.privBytes[12];
            this.m_bytRecOption = this.privBytes[12];
            this.m_bytStatus = this.privBytes[13];
            verifMode_ = this.privBytes[14];
            this.m_swipeOption = option;
            this.m_isSwipeLimitted = false;
            this.m_currentSwipeTimes = 0;
            this.m_isSwipe = ((byte) (option & SwipeOption.swipe)) == 0;
            this.m_isWiegand26 = ((byte) (option & SwipeOption.wg26)) == 0;
            this.m_floorNo = -1;
            byte num = this.privBytes[13];
            this.m_swipeStatus = num;
            if (this.m_isSwipe && (((byte) (option & SwipeOption.LimittedAccess)) != 0))
            {
                this.m_isSwipeLimitted = true;
                this.m_isWiegand26 = true;
                this.m_currentSwipeTimes = ((int) this.m_swipeOption) & 0xe0;
                this.m_currentSwipeTimes += (this.m_swipeStatus & 0x7c) >> 2;
                if (((int) (this.m_swipeOption & SwipeOption.wg26)) > 0)
                {
                    this.m_currentSwipeTimes |= 0x100;
                }
                if (wgMjController.IsElevator((int) ControllerSN))
                {
                    if (this.m_currentSwipeTimes >= 0x80)
                    {
                        this.m_floorNo = 0;
                    }
                    else
                    {
                        this.m_floorNo = this.m_currentSwipeTimes;
                    }
                }
            }
            if (((byte) (option & SwipeOption.OneSecond)) > 0)
            {
                this.m_dtRead = this.m_dtRead.AddSeconds(1.0);
            }
            this.m_iReaderNo = (byte) ((num & 3) + 1);
            this.m_isPassed = (num & 0x80) == 0;
            this.m_Reason = (byte) ((num >> 2) & 0x1f);
            this.m_iDoorNo = this.m_iReaderNo;
            this.m_isBiDirection = false;
            if (((byte) (option & SwipeOption.BiDirection)) > 0)
            {
                this.m_isBiDirection = true;
            }
            this.m_isEnterIn = true;
            this.m_AddressIsReader = false;
            this.m_isRemoteOpen = ((byte) (option & (SwipeOption.LimittedAccess | SwipeOption.swipe))) == 6;
            this.categoryGet();
            if (this.m_isSwipe)
            {
                this.m_AddressIsReader = true;
            }
            else if ((this.eventCategory == 4) && (((this.m_nUserID == 10) || (this.m_nUserID == 11)) || (this.m_nUserID == 12)))
            {
                this.m_AddressIsReader = true;
            }
            if (this.m_AddressIsReader)
            {
                if (this.m_isBiDirection)
                {
                    this.m_iDoorNo = (byte) ((this.m_iReaderNo + 1) >> 1);
                    if ((this.m_iReaderNo & 1) == 0)
                    {
                        this.m_isEnterIn = false;
                    }
                }
            }
            else if (this.m_isBiDirection)
            {
                if (this.m_iDoorNo <= 2)
                {
                    this.m_iReaderNo = (byte) ((this.m_iDoorNo << 1) - 1);
                }
                this.m_isEnterIn = true;
            }
            this.reasonGet();
        }

        public bool addressIsReader
        {
            get
            {
                return this.m_AddressIsReader;
            }
        }

        public byte bytRecOption
        {
            get
            {
                return this.privBytes[12];
            }
        }

        internal byte bytRecOption_Internal
        {
            get
            {
                return this.m_bytRecOption;
            }
        }

        public byte bytStatus
        {
            get
            {
                return this.privBytes[13];
            }
        }

        public uint UserID
        {
            get { return this.m_nUserID; }
        }

        public uint PhotoID
        {
            get { return this.m_nPhotoID; }
        }

        public uint ControllerSN
        {
            get
            {
                return this.m_ulControllerSN;
            }
        }

        public int currentSwipeTimes
        {
            get
            {
                return this.m_currentSwipeTimes;
            }
        }

        public byte DoorNo
        {
            get
            {
                return this.m_iDoorNo;
            }
        }

        public int eventCategory
        {
            get
            {
                return this.m_eventCategory;
            }
        }

        public int floorNo
        {
            get
            {
                return this.m_floorNo;
            }
        }

        public uint IndexInDataFlash
        {
            get
            {
                return this.m_ulIndexInDataFlash;
            }
        }

        internal uint indexInDataFlash_Internal
        {
            get
            {
                return this.m_ulIndexInDataFlash;
            }
        }

        public bool IsBiDirection
        {
            get
            {
                return this.m_isBiDirection;
            }
        }

        public bool IsEnterIn
        {
            get
            {
                return this.m_isEnterIn;
            }
        }

        public bool IsPassed
        {
            get
            {
                return this.m_isPassed;
            }
        }

        public bool IsRemoteOpen
        {
            get
            {
                return this.m_isRemoteOpen;
            }
        }

        public bool IsSwipeRecord
        {
            get
            {
                return this.m_isSwipe;
            }
        }

        public bool IsWiegand26
        {
            get
            {
                return this.m_isWiegand26;
            }
        }

        public DateTime ReadDate
        {
            get
            {
                return this.m_dtRead;
            }
        }

        public byte ReaderNo
        {
            get
            {
                return this.m_iReaderNo;
            }
        }

        public int ReasonNo
        {
            get
            {
                return (int) this.m_ReasonNo;
            }
        }

        internal static int SwipeSize
        {
            get
            {
                return 0x10;
            }
        }

        public byte SwipeStatus
        {
            get
            {
                return this.m_swipeStatus;
            }
        }

        public byte verifMode
        {
            get
            {
                return verifMode_;
            }
        }

        public enum EventCategory
        {
            SwipePass,
            SwipeNOPass,
            SwipePassLimitted,
            SwipeNOPassLimitted,
            ValidEvent,
            Warn,
            RemoteOpen
        }

        [Flags]
        private enum SwipeOption : byte
        {
            BiDirection = 0x10,
            LimittedAccess = 4,
            OneSecond = 8,
            remoteOpen = 4,
            swipe = 2,
            wg26 = 1
        }

        private enum SwipeStatusNo
        {
            None,
            RecordSwipe,
            RecordSwipeClose,
            RecordSwipeOpen,
            RecordSwipeWithCount,
            RecordDeniedAccessPCControl,
            RecordDeniedAccessNOPRIVILEGE,
            RecordDeniedAccessERRPASSWORD,
            RecordDeniedAccessSPECIAL_ANTIBACK,
            RecordDeniedAccessSPECIAL_MORECARD,
            RecordDeniedAccessSPECIAL_FIRSTCARD,
            RecordDeniedAccessDOORNC,
            RecordDeniedAccessSPECIAL_INTERLOCK,
            RecordDeniedAccessSPECIAL_LIMITEDTIMES,
            RecordDeniedAccessSPECIAL_LIMITEDPERSONSINDOOR,
            RecordDeniedAccessINVALIDTIMEZONE,
            RecordDeniedAccessSPECIAL_INORDER,
            RecordDeniedAccessSPECIAL_SWIPEGAPLIMIT,
            RecordDeniedAccess,
            RecordDeniedAccessSPECIAL_LIMITEDTIMES_WITHCOUNT,
            RecordPushButton,
            RecordPushButtonOpen,
            RecordPushButtonClose,
            RecordDoorOpen,
            RecordDoorClosed,
            RecordSuperPasswordDoorOpen,
            RecordSuperPasswordOpen,
            RecordSuperPasswordClose,
            RecordPowerOn,
            RecordReset,
            RecordPushButtonInvalid_Disable,
            RecordPushButtonInvalid_ForcedLock,
            RecordPushButtonInvalid_NotOnLine,
            RecordPushButtonInvalid_INTERLOCK,
            RecordWarnThreat,
            RecordWarnThreatOpen,
            RecordWarnThreatClose,
            RecordWarnLeftOpen,
            RecordWarnOpenByForce,
            RecordWarnFire,
            RecordWarnCloseByForce,
            RecordWarnGuardAgainstTheft,
            RecordWarn24Hour,
            RecordWarnEmergencyCall,
            RecordWarnTamper,
            RecordRemoteOpenDoor,
            RecordRemoteOpenDoor_ByUSBReader,
            RecordDeniedAccessVM,
        }
    }
}

