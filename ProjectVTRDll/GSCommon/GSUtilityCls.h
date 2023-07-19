//------------------------------------------------------------------------------
//  NAME : UtilityCls.h
//  DESC : 프로젝트를 위한 서고함수들을 정의한다.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  변         경         사         항                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.11.06  KM  최초 프로그람 작성                                     
//----------------------------------------------------------------------------


#pragma once

#include "GSNetCommon.h"
#include "GSPC_Socket.h"
#include "GSNetPacket.h"
#include "GSFrameBuffer.h"
#include "GSPC_UI.h"

#include "H264Decoder.h"

#include "ArrayList.h"

void SetRegistryValue(WCHAR *KeyValue, WCHAR* KeyData);
bool GetValueFromRegistry(WCHAR *KeyValue, WCHAR *KeyData, WCHAR* defaultValue);

int gMemFind(char* buf1, int nSizeBuf1, char* buf2, int nSizeBuf2, int nStart=0);

#define UTIL_ADDR_COUNT 500

static const BYTE gbAppID = 0;

class HResTimer
{
public:
	HResTimer() 
	{
		LARGE_INTEGER	liFrequency;
		QueryPerformanceFrequency(&liFrequency);
		m_llFrequency = liFrequency.QuadPart;
	}
	~HResTimer(){};
	DWORD GetSystemTime()
	{
		LARGE_INTEGER liNow;
		QueryPerformanceCounter(&liNow);
		double dNow = (double)liNow.QuadPart/m_llFrequency;
		return (DWORD)(dNow*1000);
	}

private:
	LONGLONG	m_llFrequency;
};

//system information define
struct systemInfo
{
	int nLogEnable;
	int nWirelessEncodeEnable;
	BOOL bLiveEnable;
	char czSystemLogPath[50];
	char czFFMpegLogPah[50];
	WCHAR wzVideoDataSavePath[50];
	char czTestDataSavePath[50];
	WCHAR wzLiveFilePath[50];
	
	int nCmdPktSendCounts;
	int nCmdPktRecvCounts;
	int nCmdPktErrCounts;
};

struct deviceInfo
{
	int deviceId;
	char deviceDesc[50];
	char deviceIp[30];
	int devicePort;
	BOOL isServer;
	BOOL isUDP;
	BOOL isRawData;
	BOOL isEnable;
	BOOL isThreadTrans;
};

struct ut_g_memory_type
{
	int		nKind;
	void*	lpAddr;
};

class CUtilityCls
{
public:
	static void* m_UtilityClsInst;
public:
	
	static CUtilityCls* GetInstance()
	{
		if (m_UtilityClsInst == NULL)
		{
			m_UtilityClsInst = new CUtilityCls();
		}

		return (CUtilityCls*)m_UtilityClsInst;
	}

	void Close();
public:
	CUtilityCls(void);
	~CUtilityCls(void);

public:
	GSPC_SocketList* lpSocket;
	CGSNetPacket* lpGsNetPacket;
	CH264Decoder*  lpH264LiveDecoder;
	CH264Decoder*  lpH264SearchDecoder;
	GSPC_UICls* lpGSPCUIObj;
	GSFrameBuffer* lpFrameObj;

	CArrayList m_listCmdPacket;
	CArrayList m_listDeviceInfo;

	BOOL m_bVideoRecod;
	int m_nSystemStatus;
	BOOL m_isClose;
	BOOL m_isInitPktSendStatus;
	int  m_nThreadAliveCount;

	BOOL m_bVStab;
	int m_nVStabInFrameCounts, m_nVStabOutFrameCounts;
	int m_nVStabImageLevel;
	int m_nPlaySpeed;

	long m_nLogTextCount;
	gs_logType m_stLogList[GS_NET_LOG_MAX_COUNT];
	FILE* gLogFP;

	systemInfo m_SystemInfo;
	DWORD m_nIncomingDataSpeed, m_nIncomingDataSpeedTick;
	
	HResTimer m_tickCount;
	//data define
	gs_pkt_GCS_Status_Type m_stPktGCSStatus;
	gs_pkt_UAV_Status_Type m_stPktUAVStatus;

	GS_NET_VIDEO_CALLBACK_FUNC lpVideoCallbackFunc;
	GS_NET_VIDEO_CALLBACK_FUNC lpVideoStabCallbackFunc;
	GS_NET_DATA_CALLBACK_FUNC  lpDataCallBackFunc;


private:
	ut_g_memory_type m_addr[UTIL_ADDR_COUNT];
	BYTE* m_lpVideoBuffer;
public:
	void Init();
	void InitVariable();
	void gMemoryAddrAdd(void* lpAddr, int nKind = -1);
	void gMemoryAddrRelease(int nKind = -1);

	void AddLogText(int nKind, LPCTSTR pFormat, ...);
	void RemoveLogText();

	void VideoBufferProc(int nSize, BYTE* inBuf, DWORD dwTick);

	unsigned char crc7(unsigned char crc, const unsigned char *buffer, unsigned char len);
	unsigned char check_crc(unsigned char *buf, unsigned char sz);
};


/* Table for CRC-7 (polynomial x^7 + x^3 + 1) */
const unsigned char crc7_syndrome_table[256] = {
	0x00, 0x09, 0x12, 0x1b, 0x24, 0x2d, 0x36, 0x3f,
	0x48, 0x41, 0x5a, 0x53, 0x6c, 0x65, 0x7e, 0x77,
	0x19, 0x10, 0x0b, 0x02, 0x3d, 0x34, 0x2f, 0x26,
	0x51, 0x58, 0x43, 0x4a, 0x75, 0x7c, 0x67, 0x6e,
	0x32, 0x3b, 0x20, 0x29, 0x16, 0x1f, 0x04, 0x0d,
	0x7a, 0x73, 0x68, 0x61, 0x5e, 0x57, 0x4c, 0x45,
	0x2b, 0x22, 0x39, 0x30, 0x0f, 0x06, 0x1d, 0x14,
	0x63, 0x6a, 0x71, 0x78, 0x47, 0x4e, 0x55, 0x5c,
	0x64, 0x6d, 0x76, 0x7f, 0x40, 0x49, 0x52, 0x5b,
	0x2c, 0x25, 0x3e, 0x37, 0x08, 0x01, 0x1a, 0x13,
	0x7d, 0x74, 0x6f, 0x66, 0x59, 0x50, 0x4b, 0x42,
	0x35, 0x3c, 0x27, 0x2e, 0x11, 0x18, 0x03, 0x0a,
	0x56, 0x5f, 0x44, 0x4d, 0x72, 0x7b, 0x60, 0x69,
	0x1e, 0x17, 0x0c, 0x05, 0x3a, 0x33, 0x28, 0x21,
	0x4f, 0x46, 0x5d, 0x54, 0x6b, 0x62, 0x79, 0x70,
	0x07, 0x0e, 0x15, 0x1c, 0x23, 0x2a, 0x31, 0x38,
	0x41, 0x48, 0x53, 0x5a, 0x65, 0x6c, 0x77, 0x7e,
	0x09, 0x00, 0x1b, 0x12, 0x2d, 0x24, 0x3f, 0x36,
	0x58, 0x51, 0x4a, 0x43, 0x7c, 0x75, 0x6e, 0x67,
	0x10, 0x19, 0x02, 0x0b, 0x34, 0x3d, 0x26, 0x2f,
	0x73, 0x7a, 0x61, 0x68, 0x57, 0x5e, 0x45, 0x4c,
	0x3b, 0x32, 0x29, 0x20, 0x1f, 0x16, 0x0d, 0x04,
	0x6a, 0x63, 0x78, 0x71, 0x4e, 0x47, 0x5c, 0x55,
	0x22, 0x2b, 0x30, 0x39, 0x06, 0x0f, 0x14, 0x1d,
	0x25, 0x2c, 0x37, 0x3e, 0x01, 0x08, 0x13, 0x1a,
	0x6d, 0x64, 0x7f, 0x76, 0x49, 0x40, 0x5b, 0x52,
	0x3c, 0x35, 0x2e, 0x27, 0x18, 0x11, 0x0a, 0x03,
	0x74, 0x7d, 0x66, 0x6f, 0x50, 0x59, 0x42, 0x4b,
	0x17, 0x1e, 0x05, 0x0c, 0x33, 0x3a, 0x21, 0x28,
	0x5f, 0x56, 0x4d, 0x44, 0x7b, 0x72, 0x69, 0x60,
	0x0e, 0x07, 0x1c, 0x15, 0x2a, 0x23, 0x38, 0x31,
	0x46, 0x4f, 0x54, 0x5d, 0x62, 0x6b, 0x70, 0x79
};