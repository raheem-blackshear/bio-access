//------------------------------------------------------------------------------
//  NAME : UtilityCls.cpp
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
#include "stdafx.h"

#include "GSUtilityCls.h"
#include "GSXMLite.h"

//Util

/* ===========================================================================
* 첫번째 메모리와 두번째메모리를 비교하여 처음으로 같은 지적자의 위치를 돌려준다.
*===========================================================================*/

static CMyMutex gMemMutex(GS_Lock_kind_Long);
int gMemFind(char* buf1, int nSizeBuf1, char* buf2, int nSizeBuf2, int nStart)
{
	gMemMutex.Lock();

	int i = nStart;
	while (i < nSizeBuf1)
	{
		if (memcmp(&buf1[i], buf2, nSizeBuf2) == 0)
		{
			gMemMutex.UnLock();
			return i;
		}
		i++;
	}

	gMemMutex.UnLock();
	return -1;
}


void* CUtilityCls::m_UtilityClsInst = NULL;

CUtilityCls::CUtilityCls(void)
{
	m_nSystemStatus = GS_NET_SYSTEM_STATUS_INIT;

	m_nLogTextCount = 0;

	WCHAR wchStr[10] = {0};

	m_nVStabInFrameCounts = 6;
	m_nVStabOutFrameCounts = 25;
	m_nVStabImageLevel =10;

	m_nThreadAliveCount = 0;

	lpH264LiveDecoder = NULL;
	lpGSPCUIObj = NULL;

	m_lpVideoBuffer = new BYTE[GS_NET_VIDEO_BUFFER_SIZE];

	gLogFP = NULL;
}


CUtilityCls::~CUtilityCls(void)
{
	if (m_UtilityClsInst == NULL)
	{
		return;
	}

	delete m_UtilityClsInst;
	m_UtilityClsInst = NULL;
}

void CUtilityCls::Init()
{
	m_isClose = FALSE;
	/******************class create ********************/
	if (m_nSystemStatus == GS_NET_SYSTEM_STATUS_INIT)
	{
		lpGSPCUIObj = new GSPC_UICls();
	}

	lpH264LiveDecoder = new CH264Decoder();
	lpH264LiveDecoder->m_bLive = FALSE;

	m_nSystemStatus = GS_NET_SYSTEM_STATUS_ACTIVE;
}

void CUtilityCls::InitVariable()
{
	memset(&m_stPktGCSStatus, 0, sizeof(gs_pkt_GCS_Status_Type));
	memset(&m_stPktUAVStatus, 0, sizeof(gs_pkt_UAV_Status_Type));
}

void CUtilityCls::Close()
{
	m_isClose = TRUE;

	lpVideoCallbackFunc = NULL;
	lpVideoStabCallbackFunc = NULL;
	lpDataCallBackFunc = NULL;

	if (m_nSystemStatus == GS_NET_SYSTEM_STATUS_SHUTDOWN)
		delete lpGSPCUIObj;

	delete lpH264LiveDecoder;
	
	if (m_nSystemStatus == GS_NET_SYSTEM_STATUS_SHUTDOWN)
	{
		delete m_lpVideoBuffer;
		if (m_UtilityClsInst)
			delete m_UtilityClsInst;

		m_UtilityClsInst = NULL;
	}
}

#define GS_LOG_VIEW_LENGTH 1000
#define GS_LOG_VIEW_COUNT 15
void CUtilityCls::AddLogText(int nKind, LPCTSTR pFormat, ...)
{
	TCHAR    logText[GS_NET_LOGTEXT_MAX_LENGTH];
	va_list pArg;

	va_start(pArg, pFormat);
	_vstprintf_s(logText, GS_NET_LOGTEXT_MAX_LENGTH, pFormat, pArg);
	va_end(pArg);

	F_LogWrite(logText);

	wsprintf(m_stLogList[nKind].wzLogText, L"%s", logText);
	m_stLogList[nKind].tickVal = m_tickCount.GetSystemTime();

	/*if (nLevel >= GS_NET_MSG_HIGH)
	{
		wcscat(m_sLogText, logText);
		wcscat(m_sLogText, _T("\r\n"));

		InterlockedIncrement(&m_nLogTextCount);
		{
			if (wcslen(m_sLogText) > GS_LOG_VIEW_LENGTH)
			if (m_nLogTextCount > GS_LOG_VIEW_COUNT)
			{
				RemoveLogText();
			}
		}
	}

	if (nLevel >= GS_NET_MSG_MID)
	{
		F_LogWrite(logText);
	}*/
	
	return;
}

void CUtilityCls::RemoveLogText()
{
//rRemove:
	/*int nSize = wcslen(m_sLogText);
	int nFirst = (int)(wcschr(m_sLogText, '\n') - m_sLogText + 1);
	if (nFirst < 5)
	{
	int xx = 0;
	xx++;
	}
	if (nFirst > 0)
	{
	int n = nSize - nFirst;
	memcpy(m_sLogText, &m_sLogText[nFirst], n * 2);

	m_sLogText[n] = '\0';

	InterlockedDecrement(&m_nLogTextCount);

	if (m_nLogTextCount<0)
	{
	m_nLogTextCount = 0;
	}

	if (m_nLogTextCount > GS_LOG_VIEW_COUNT)
	goto rRemove;
	}*/
}

DWORD dwTickBack = 0;
void CUtilityCls::VideoBufferProc(int nSize, BYTE* inBuf, DWORD dwTick)
{
#if (GS_NET_H264_APPLY_CURRENT_TIME)
	dwTick = m_tickCount.GetSystemTime();
	if (dwTick - dwTickBack < 10)
		dwTick += 10;
#else
	if (dwTick <= 0 || dwTick >= 0x8FFFFFFF)
		dwTick = m_tickCount.GetSystemTime();
#endif

	dwTickBack = dwTick;

	lpFrameObj->append(inBuf, nSize);
	
	while(TRUE)
	{
		nSize = lpFrameObj->getFrameBuffer(m_lpVideoBuffer);
		if (nSize <= 1)
		{
			//AddLogText(GS_NET_LOG_H264_DECODE, L"No get GS Frame Size");
			break;
		}


		//char fileName[128];
		//GS_NET_LiveRecordFileName(fileName);
		//GS_NET_SetVideoRecord(TRUE, fileName);

		//lpH264LiveDecoder->Decode(nSize, m_lpVideoBuffer, TRUE, dwTick);
	}
}


/* ===========================================================================
* 창조된 메모리의 주소값을 저장한다.
*===========================================================================*/
void CUtilityCls::gMemoryAddrAdd(void* lpAddr, int nKind)
{
	int i;
	for (i=0; i<UTIL_ADDR_COUNT; i++)
	{
		if (m_addr[i].lpAddr == NULL)
			break;
	}

	m_addr[i].lpAddr = lpAddr;
	m_addr[i].nKind = nKind;
}

/* ===========================================================================
* 메모리령역을 해방한다.
*===========================================================================*/
void CUtilityCls::gMemoryAddrRelease(int nKind)
{
	for (int i=0; i<UTIL_ADDR_COUNT; i++)
	{
		if (m_addr[i].nKind == nKind || nKind < 0)
		{
			void *lpVoid = m_addr[i].lpAddr;
			if (!IsBadReadPtr(lpVoid, 4))
				delete m_addr[i].lpAddr;

			m_addr[i].lpAddr = NULL;
			m_addr[i].nKind = -1;
		}
	}
}

/* ===========================================================================
*  CRC7 
*===========================================================================*/

/**
 * crc7 - update the CRC7 for the data buffer
 * @crc:     previous CRC7 value
 * @buffer:  data pointer
 * @len:     number of bytes in the buffer
 * Context: any
 *
 * Returns the updated CRC7 value.
 */
unsigned char CUtilityCls::crc7(unsigned char crc, const unsigned char *buffer, unsigned char len)
{
	unsigned char data;

	while (len--){
		data = *buffer++;
		crc = crc7_syndrome_table[(crc << 1) ^ data];
	}
	return crc;
}

//temp[0] = 0xEB;temp[1] = 0x90;temp[2] = 0xBA;
//temp[3] = crc7(0, temp, 3);

unsigned char CUtilityCls::check_crc(unsigned char *buf, unsigned char len)
{
	unsigned char crc = crc7(0, buf, len-1);
	if (crc == buf[len-1])
		return 1;
	else
		return 0;	
}
