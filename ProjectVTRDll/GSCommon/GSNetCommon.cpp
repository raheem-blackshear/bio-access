// GSNet.cpp : Defines the class behaviors for the application.
//------------------------------------------------------------------------------
//  NAME : GSNetCommon.cpp 
//  DESC : 통신용 서고파일
// VER  : v1.0 
// PROJ : GS
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  변         경         사         항                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.10.20  로일선  최초프로그람 작성                                     
//------------------------------------------------------------------------------
//  ***  Compile Switch : precompiled header : Not Use  ***
//------------------------------------------------------------------------------

#include "stdafx.h"

#include "GSNetCommon.h"
#include "GSUtilityCls.h"

//=============================================================================
#ifdef GS_PROJECT_PC

#else
GSArm_SockLIst lpSocket;
#endif

#ifdef GS_PROJECT_DLL_INCLUDE
#include "GSPC_Socket.h"
#endif // GS_PROJECT_DLL_INCLUDE

//=============================================================================
//GS_NET 내부처리함수

//변수 정의
//callback 함수렬
GS_NET_CALLBACK* lpgsNetCallbackFirst[GS_DEVICE_MAX];

//송신자료렬
GS_NET_DATA_LIST	gsNetSendData[GS_DEVICE_MAX];
//수신자료렬
GS_NET_DATA_LIST	gsNetRecvData[GS_DEVICE_MAX];
//통보문처리렬
GS_NET_DATA_LIST	gsNetMsgData;

//자료충돌방지용 기발
//static DWORD gsNetSendLock[GS_DEVICE_MAX];
//static DWORD gsNetRecvLock[GS_DEVICE_MAX];

//==========================================================
static CMyMutex gNetFileMutex(GS_Lock_kind_Long);
WCHAR logOutbuf[GS_NET_LOGTEXT_MAX_LENGTH * 2];
void F_LogWrite(LPCTSTR pFormat, ...)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (IsBadReadPtr(utilObj->gLogFP, 4))
		return;

	if (utilObj->m_SystemInfo.nLogEnable == 0)
		return;

	if (strlen(utilObj->m_SystemInfo.czSystemLogPath) < 1)
		return;

	TCHAR chMsg[GS_NET_LOGTEXT_MAX_LENGTH];
	va_list pArg;
	WCHAR  wchDes[GS_NET_LOGTEXT_MAX_LENGTH];

	va_start(pArg, pFormat);
	_vstprintf_s(chMsg, GS_NET_LOGTEXT_MAX_LENGTH, pFormat, pArg);
	va_end(pArg);

	wsprintf(wchDes, L"%s", chMsg);
	
	if (wchDes == NULL) 
		return;
	
	SYSTEMTIME systemTime;

	gNetFileMutex.Lock();

	GetLocalTime(&systemTime);
	wsprintf(logOutbuf, L"%04d-%02d-%02d %02d:%02d:%02d:%03d\t %s\n", systemTime.wYear, systemTime.wMonth, systemTime.wDay,
		systemTime.wHour, systemTime.wMinute, systemTime.wSecond, systemTime.wMilliseconds, wchDes);

	//errno_t err = fopen_s(&(utilObj->gLogFP, CUtilityCls::GetInstance()->m_SystemInfo.czSystemLogPath, "a+, ccs=UTF-8");

	//if (err == 0)
	{
		if (utilObj->gLogFP)
		{
			fwrite(logOutbuf, 2, wcslen(logOutbuf), utilObj->gLogFP);
			//fclose(utilObj->gLogFP);
		}
	} /*else
	  {
	  TRACE(L"File Log Instance is NULL!...logMsg = %s", logOutbuf);
	  }*/

	gNetFileMutex.UnLock();
	//utilObj->gLogFP = NULL;

	return;
}

static int dotNet_ = 0;

GS_NET_API_FN_TYPE void __stdcall dotNet(int net)
{
	dotNet_ = net;
}

GS_NET_API_FN_TYPE void __stdcall initH264()
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	utilObj->Init();
	utilObj->lpGSPCUIObj->Init();
}

GS_NET_API_FN_TYPE void __stdcall deinitH264()
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	utilObj->m_isClose = TRUE;
	utilObj->lpGSPCUIObj->Release();
	utilObj->m_nSystemStatus = GS_NET_SYSTEM_STATUS_SHUTDOWN;
	utilObj->Close();
}

GS_NET_API_FN_TYPE int __stdcall decodeH264(void* in, int size, void* out)
{
	if (dotNet_)
	{
		in = (void*) *(long FAR*)in;
		out = (void*) *(long FAR*)out;
	}
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	return utilObj->lpH264LiveDecoder->Decode(in, size, out);
}

GS_NET_API_FN_TYPE void __stdcall convertGrayImage(
	unsigned char* image,
	int width, int height,
	unsigned char* gray)
{
	if (dotNet_)
	{
		image = (unsigned char*) *(long FAR*)image;
		gray = (unsigned char*) *(long FAR*)gray;
	}

	int x, y, i, j;
	for (y = 0, i = j = 0; y < height; y++)
	{
		for (x = 0; x < width; x++, j += 3)
		{
			gray[i++] = (unsigned char)(
				(float)image[j+2] * 0.299 + //r
				(float)image[j+1] * 0.587 + //g
				(float)image[j+0] * 0.114); //b
		}
	}
}

GS_NET_API_FN_TYPE void GS_NET_CONSTRUCTOR(char* czConfigFileName)
{

	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	utilObj->Init();
	utilObj->lpGSPCUIObj->Init();
    gsNetMsgData.hLock = CreateMutex (NULL, FALSE, NULL);
}

GS_NET_API_FN_TYPE void GS_NET_DESTROY(int nStatus)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	utilObj->m_isClose = TRUE;
	while (utilObj->m_nThreadAliveCount > 0)
		Sleep(1);

	utilObj->AddLogText(GS_NET_LOG_SYSTEM_INFO, L"---------   체계 끄기중...   --------");

	utilObj->lpGSPCUIObj->Release();

	utilObj->m_nSystemStatus = nStatus;
	utilObj->Close();

	GS_NET_DeleteAllDataList(&gsNetMsgData);
	CloseHandle(gsNetMsgData.hLock);
}

GS_NET_API_FN_TYPE void GS_NET_LiveRecordFileName(char* fileName)
{
	//char fileName[128];
	CTime t = CTime::GetCurrentTime();
	sprintf(fileName, "gsVideoRecording(%04d-%02d-%02d_%02d%02d)", t.GetYear(), t.GetMonth(),t.GetDay(), t.GetHour(), t.GetMinute());
}

GS_NET_API_FN_TYPE void GS_NET_SetVideoRecord(BOOL bRecordStatus, char* czFileName)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->m_bVideoRecod = bRecordStatus;
	if (bRecordStatus && czFileName)
	{
		char tmpFileName[100] = {0};
		int  ch = '.';
		if (strchr(czFileName, ch))
			strncpy(tmpFileName, czFileName, (strchr(czFileName, ch) - czFileName));
		else
			sprintf(tmpFileName, "%s", czFileName);
		
		swprintf(utilObj->m_SystemInfo.wzLiveFilePath, L"%S", czFileName);
		utilObj->lpH264LiveDecoder->LiveVideoFileClose();
	}
}

GS_NET_API_FN_TYPE BOOL GS_NET_GetVideoRecord()
{
	return CUtilityCls::GetInstance()->m_bVideoRecod;
}

GS_NET_API_FN_TYPE BOOL GS_NET_GetFilePlayStatus()
{
	return CUtilityCls::GetInstance()->lpH264SearchDecoder->m_bPlayStatus;
}

GS_NET_API_FN_TYPE void GS_NET_RegVideoDataCallback(void* lpFunc, void* lpStabFunc, void* lpTCPData_GetDataCallback)
{
	CUtilityCls::GetInstance()->lpVideoCallbackFunc = (GS_NET_VIDEO_CALLBACK_FUNC)lpFunc;
	CUtilityCls::GetInstance()->lpVideoStabCallbackFunc = (GS_NET_VIDEO_CALLBACK_FUNC)lpStabFunc;
	CUtilityCls::GetInstance()->lpDataCallBackFunc = (GS_NET_DATA_CALLBACK_FUNC)lpTCPData_GetDataCallback;
}

GS_NET_API_FN_TYPE void GS_NET_SetFilePlaySpeed(int nSpeed)
{
	CUtilityCls::GetInstance()->m_nPlaySpeed = nSpeed;
}

GS_NET_API_FN_TYPE int GS_NET_GetFilePlaySpeed()
{
	return CUtilityCls::GetInstance()->m_nPlaySpeed;
}

GS_NET_API_FN_TYPE void GS_NET_INITLOCK(DWORD dwDeviceID)
{
#ifdef GS_PROJECT_PC
	gsNetSendData[dwDeviceID].hLock = CreateMutex (NULL, FALSE, NULL);
	gsNetRecvData[dwDeviceID].hLock = CreateMutex (NULL, FALSE, NULL);
#else
	sem_init( &gsNetSendData[dwDeviceID].hLock, 0, 1 );
	sem_init( &gsNetRecvData[dwDeviceID].hLock, 0, 1 );
#endif
}

GS_NET_API_FN_TYPE void GS_NET_DELETELOCK(DWORD dwDeviceID)
{
#ifdef GS_PROJECT_PC
	if (gsNetSendData[dwDeviceID].hLock)
		CloseHandle(gsNetSendData[dwDeviceID].hLock);
	if (gsNetRecvData[dwDeviceID].hLock)
		CloseHandle(gsNetRecvData[dwDeviceID].hLock);

	gsNetSendData[dwDeviceID].hLock = NULL;
	gsNetRecvData[dwDeviceID].hLock = NULL;
#else
	sem_destroy( &gsNetSendData[dwDeviceID].hLock);
	sem_destroy( &gsNetRecvData[dwDeviceID].hLock);
#endif
}

#ifdef GS_PROJECT_PC
GS_NET_API_FN_TYPE BOOL GS_NET_LOCK(HANDLE* hLock)
{
	if (WAIT_OBJECT_0 != WaitForSingleObject(*hLock, INFINITE ))
	{
		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_LOCK_STATUS, _T("GS_NET_LOCK Lock was not create device or released."));
		return FALSE;
	}
	return TRUE;
}
#else
GS_NET_API_FN_TYPE BOOL GS_NET_LOCK(sem_t* hLock)
{
        if (sem_wait( hLock ) == 0)
                return true;
	return false;
}
#endif

#ifdef GS_PROJECT_PC
GS_NET_API_FN_TYPE void GS_NET_UNLOCK(HANDLE* hLock)
{
	ReleaseMutex(*hLock);
}
#else
GS_NET_API_FN_TYPE void GS_NET_UNLOCK(sem_t* hLock)
{
	sem_post( hLock );
}
#endif

GS_NET_API_FN_TYPE DWORD GS_NET_BUFFER_SIZE(GS_NET_DATA_LIST* lpDataList)
{
	DWORD nCount = 0;
	GS_NET_DATA* lpData = lpDataList->lpFirst;
	while(lpData != NULL)
	{
		nCount++;
		lpData = lpData->lpNext;
	}
	return nCount;
}

GS_NET_API_FN_TYPE void GS_NET_DeleteDataHeader(GS_NET_DATA_LIST* lpDataList)
{
	if (!GS_NET_LOCK(&lpDataList->hLock))
		return;
	
	GS_NET_DATA* lpDataHeader;
	GS_NET_DATA* lpDataNext;

	if(lpDataList->lpFirst != NULL)
	{
		lpDataHeader = lpDataList->lpFirst;

		lpDataNext = lpDataHeader->lpNext;
		if(lpDataNext != NULL)
		{
			lpDataNext->lpPrev = NULL;
		}
		if(lpDataHeader->lpData != NULL)
		{
			delete lpDataHeader->lpData;
			lpDataHeader->lpData = NULL;
		}
		if(lpDataList->lpFirst == lpDataList->lpLast)
		{
			lpDataList->lpLast = lpDataNext;
		}
		delete lpDataList->lpFirst;

		lpDataList->lpFirst = lpDataNext;
	}

	InterlockedDecrement(&lpDataList->nListCount);
	GS_NET_UNLOCK(&lpDataList->hLock);
}

GS_NET_API_FN_TYPE void  GS_NET_AddDataList(GS_NET_DATA_LIST* lpDataList, GS_NET_DATA* lpData)
{
	if (!GS_NET_LOCK(&lpDataList->hLock))
		return;

	GS_NET_DATA* lpDataLast;

	if(lpDataList->lpFirst == NULL)
	{
		lpDataList->lpFirst = lpData;
		lpDataList->lpLast = lpData;
	}
	else
	{
		lpDataLast = lpDataList->lpLast;
		lpDataLast->lpNext = lpData;
		lpData->lpPrev = lpDataLast;
		lpDataList->lpLast = lpData;
	}

	InterlockedIncrement(&lpDataList->nListCount);
	GS_NET_UNLOCK(&lpDataList->hLock);
}
GS_NET_API_FN_TYPE void  GS_NET_DeleteAllDataList(GS_NET_DATA_LIST* lpDataList)
{
	while(lpDataList->lpFirst != NULL)
	{
		GS_NET_DeleteDataHeader(lpDataList);
	}
}

GS_NET_API_FN_TYPE DWORD GS_NET_CheckDeviceID(DWORD dwDeviceID)
{
	if(dwDeviceID >= GS_DEVICE_MAX)
	{
		gsdwNetDeviceErrorCode = GS_NET_ERROR_NODEV;
		return GS_NET_ERROR_RETURN;
	}

	gsdwNetDeviceErrorCode = GS_NET_ERROR_NO;
	return GS_NET_ERROR_NO;
};

GS_NET_API_FN_TYPE BYTE GS_NET_MakeDestOrSrc(BYTE bP1OrCmd, BYTE bRemote, BYTE bDest, BYTE sDest)
{
	BYTE bRs = 0;
	bRs = GS_SetBitVal(bRs, 1, bP1OrCmd);
	bRs = GS_SetBitVal(bRs, 0, bRemote);
	bRs = bRs << 3;
	bRs = bRs | bDest;
	bRs = bRs << 3;
	bRs = bRs | sDest;

	return bRs;
}
GS_NET_API_FN_TYPE void GS_NET_RegularDestOrSrc(BYTE inData, BYTE* bP1OrCmd, BYTE* bRemote, BYTE* bDest, BYTE* sDest)
{
	*bP1OrCmd = GS_GetBitVal(inData, 7);
	*bRemote = GS_GetBitVal(inData, 6);

	inData = inData & 0x3F;//0011 1111
	*bDest = inData >> 3;
	*sDest = inData & 0x07;//0000 0111
}

GS_NET_API_FN_TYPE DWORD GS_NET_MakeDataType(BYTE bDest, BYTE bSrc, BYTE b3, BYTE b4)
{
	return 0x11000000;
	return (bDest<<24) + (bSrc<<16) + (b3<<8) + b4;
}

GS_NET_API_FN_TYPE void GS_NET_RegularDataType(DWORD dwInData, BYTE* bDest, BYTE* bSrc, BYTE* b3, BYTE* b4)
{
	*bDest = dwInData >> 24;
	*bSrc = (BYTE)((dwInData & 0x00FF0000) >> 16);
	*b3 = (dwInData & 0x0000FF00) >> 8;
	*b4 = (dwInData & 0x000000FF);
}

//입력된 값을 가지고 다른 장치에 보낼 값형식을 만들어서 돌려준다.
//dwHeader 에 파케트 련번호가 들어간다.
GS_NET_API_FN_TYPE void GS_NET_GetSendRegularData(BYTE dwHeader, DWORD dwSize, DWORD dwDataType, BYTE* lpData, BYTE* lpResult)
{
	BYTE* lpBuff = &lpResult[0];
	memcpy(lpBuff, GS_NET_RS_HEAD_ST_STRING, GS_NET_RS_HEAD_ST_STRING_LEN);
	lpBuff += GS_NET_RS_HEAD_ST_STRING_LEN;
	memcpy(lpBuff, &dwHeader, 1);
	lpBuff += 1;
	memcpy(lpBuff, &dwSize, 4);
	lpBuff += 4;
	memcpy(lpBuff, &dwDataType, 4);
	lpBuff += 4;
	memcpy(lpBuff, lpData, dwSize);
}

//다른장치에서 들어온 값을 형식에 맞게 분할하여 대입한다.
GS_NET_API_FN_TYPE void GS_NET_GetRecvRegularData(BYTE* lpInData, BYTE* dwHeader, DWORD* dwSize, DWORD* dwDataType, BYTE* lpData)
{
	*dwHeader = 0;
	*dwSize = 0;
	*dwDataType = 0;

	BYTE* lpBuff = &lpInData[GS_NET_RS_HEAD_ST_STRING_LEN];
	memcpy(dwHeader, lpBuff, 1);
	lpBuff += 1;
	memcpy(dwSize, lpBuff, 4);
	lpBuff += 4;
	memcpy(dwDataType, lpBuff, 4);
	lpBuff += 4;
	memcpy(lpData, lpBuff, *dwSize);
}

//========================================================================
//  기본 함수부
GS_NET_API_FN_TYPE DWORD GS_NET_DeviceOpen(DWORD dwDeviceID)
{
	GS_NET_INITLOCK(dwDeviceID);

	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
		return GS_NET_ERROR_RETURN;

	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

	GS_NET_DeleteAllDataList(&gsNetSendData[dwDeviceID]);
	GS_NET_DeleteAllDataList(&gsNetRecvData[dwDeviceID]);

	gsNetSendData[dwDeviceID].nListCount = 0;
	gsNetRecvData[dwDeviceID].nListCount = 0;

	return CUtilityCls::GetInstance()->lpSocket->Create(dwDeviceID);
}

GS_NET_API_FN_TYPE DWORD GS_NET_DeviceClose(DWORD dwDeviceID)
{
	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN || IsBadReadPtr(CUtilityCls::GetInstance()->lpSocket, 4) )
		return GS_NET_ERROR_RETURN;
	
	gsdwNetErrorCode[dwDeviceID] = CUtilityCls::GetInstance()->lpSocket->Close(dwDeviceID);

	//해당 장치와 련결된 모든 송수신자료를 지운다.
	GS_NET_DeleteAllDataList(&gsNetSendData[dwDeviceID]);
	GS_NET_DeleteAllDataList(&gsNetRecvData[dwDeviceID]);
	GS_NET_DeleteAllDataList(&gsNetMsgData);

	gsNetSendData[dwDeviceID].nListCount = 0;
	gsNetRecvData[dwDeviceID].nListCount = 0;

	//해당 장치와 련결된 모든 callback 함수를 지운다.
	GS_NET_CALLBACK* lpCallback;
	while(lpgsNetCallbackFirst[dwDeviceID] != NULL)
	{
		lpCallback = lpgsNetCallbackFirst[dwDeviceID];
		lpgsNetCallbackFirst[dwDeviceID] = lpgsNetCallbackFirst[dwDeviceID]->lpNext;
		delete lpCallback;
	}

	GS_NET_DELETELOCK(dwDeviceID);

	return 1;
}

GS_NET_API_FN_TYPE BYTE GS_NET_Open(DWORD dwDeviceID , BYTE bAppID)
{
	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;
	return bAppID;
}

GS_NET_API_FN_TYPE DWORD GS_NET_Close(DWORD dwDeviceID ,BYTE bAppID)
{
	GS_NET_CALLBACK* lpCallback;
	GS_NET_CALLBACK* lpCallbackCur;
	GS_NET_CALLBACK* lpCallbackPrev;
	GS_NET_CALLBACK* lpCallbackNext;

	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
		return GS_NET_ERROR_RETURN;

	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

	//callback 함수렬에서 해당app의 callback 함수를 삭제한다. 
	lpCallbackCur = lpgsNetCallbackFirst[dwDeviceID];
	while(lpCallbackCur != NULL)
	{
		lpCallback = lpCallbackCur;
		lpCallbackCur = lpCallbackCur->lpNext;
		if(lpCallback->bAppID == bAppID)
		{
			lpCallbackPrev = lpCallback->lpPrev;
			lpCallbackNext = lpCallback->lpNext;
			if(lpCallbackPrev != NULL)
			{
				lpCallbackPrev->lpNext = lpCallbackNext;
			}
			if(lpCallbackNext != NULL)
			{
				lpCallbackNext->lpPrev = lpCallbackPrev;
			}
			if(lpCallback == lpgsNetCallbackFirst[dwDeviceID])
			{
				lpgsNetCallbackFirst[dwDeviceID] = lpCallback->lpNext;
			}
			delete lpCallback;
		}
	}

	return 1;
}

GS_NET_API_FN_TYPE DWORD GS_NET_RegCallback(DWORD dwDeviceID, BYTE bAppID ,DWORD dwCallType,  void* lpFunc, DWORD dwPriority, DWORD dwDataFilter)
{
	DWORD dwRegOK = 1;
	int nCallbackNum = 0;
	GS_NET_CALLBACK* lpCallbackLast;
	GS_NET_CALLBACK* lpCallback;
	
	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
	{
		dwRegOK = 0;
		return dwRegOK;
	}
	
	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

	lpCallback = (GS_NET_CALLBACK*)malloc(sizeof(GS_NET_CALLBACK));
	if(lpCallback == NULL)
	{
		dwRegOK = 0;
		gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NOMEM;
		return dwRegOK;
	}
	lpCallback->dwDeviceID = dwDeviceID;
	lpCallback->bAppID = bAppID;
	lpCallback->dwCallType = dwCallType;
	lpCallback->lpFunc = (GS_NET_CALLBACK_FUNC)lpFunc;
	lpCallback->dwDataFilter = dwDataFilter;
	lpCallback->dwPriority = dwPriority;
	lpCallback->lpPrev = NULL;
	lpCallback->lpNext = NULL;

	//callback 함수렬에 추가한다.
	nCallbackNum = 1;
	if(lpgsNetCallbackFirst[dwDeviceID] == NULL)
	{
		lpgsNetCallbackFirst[dwDeviceID] = lpCallback;
	}
	else
	{
		lpCallbackLast = lpgsNetCallbackFirst[dwDeviceID];
		while(lpCallbackLast->lpNext != NULL)
		{
			nCallbackNum++;
			lpCallbackLast = lpCallbackLast->lpNext;
		}
		nCallbackNum++;
		lpCallbackLast->lpNext = lpCallback;
		lpCallback->lpPrev = lpCallbackLast;
	}

	//callback 함수들을 우선권순위로 정렬한다
	//qsort()
	if(nCallbackNum >= 2)
	{
		GS_NET_CALLBACK* lpCallback1;
		GS_NET_CALLBACK* lpCallback2;
		GS_NET_CALLBACK* lpCallbackMin;
		GS_NET_CALLBACK* lpCallbackTemp;

		lpCallbackTemp = (GS_NET_CALLBACK*)malloc(sizeof(GS_NET_CALLBACK));
	
		lpCallback1 = lpgsNetCallbackFirst[dwDeviceID];
		
		while(lpCallback1->lpNext != NULL)
		{
			lpCallbackMin = lpCallback1;
			lpCallback2 = lpCallback1->lpNext;
			//우선권순위가 가장 작은것을 찾는다.
			do 
			{
				if(lpCallbackMin->dwPriority > lpCallback2->dwPriority )
				{
					lpCallbackMin = lpCallback2;
				}
				lpCallback2 = lpCallback2->lpNext;
			} while (lpCallback2 != NULL);

			memcpy(lpCallbackTemp,lpCallback1,sizeof(GS_NET_CALLBACK));

			lpCallback1->dwDeviceID = lpCallbackMin->dwDeviceID;
			lpCallback1->bAppID = lpCallbackMin->bAppID;
			lpCallback1->dwCallType = lpCallbackMin->dwCallType;
			lpCallback1->dwDataFilter = lpCallbackMin->dwDataFilter;
			lpCallback1->dwPriority = lpCallbackMin->dwPriority;
			lpCallback1->lpFunc = lpCallbackMin->lpFunc;

			lpCallbackMin->dwDeviceID = lpCallbackTemp->dwDeviceID;
			lpCallbackMin->bAppID = lpCallbackTemp->bAppID;
			lpCallbackMin->dwCallType = lpCallbackTemp->dwCallType;
			lpCallbackMin->dwDataFilter = lpCallbackTemp->dwDataFilter;
			lpCallbackMin->dwPriority = lpCallbackTemp->dwPriority;
			lpCallbackMin->lpFunc = lpCallbackTemp->lpFunc;

			lpCallback1 = lpCallback1->lpNext;
		}

		delete lpCallbackTemp;
	}

	return dwRegOK;
}

GS_NET_API_FN_TYPE BOOL GS_NET_CallCallback(DWORD dwDeviceID, DWORD dwDataType, DWORD dwCallType)
{
	//App측의 callback 함수를 호출한다.
	GS_NET_CALLBACK_FUNC CallbackFunc;
	GS_NET_CALLBACK* lpCallback;

	lpCallback = lpgsNetCallbackFirst[dwDeviceID];
	while(lpCallback != NULL)
	{
		if((lpCallback->dwDeviceID == dwDeviceID)&&
			((lpCallback->dwCallType & dwCallType) == dwCallType)&&
			((lpCallback->dwDataFilter & dwDataType) == dwDataType))
		{
			CallbackFunc = lpCallback->lpFunc;
			CallbackFunc(gsdwNetErrorCode[dwDeviceID]);
			return TRUE;
		}
		lpCallback = lpCallback->lpNext;
	}

	return FALSE;
}


GS_NET_API_FN_TYPE DWORD GS_NET_Get_List_Count(DWORD dwDeviceID)
{
	return gsNetSendData[dwDeviceID].nListCount;
}

GS_NET_API_FN_TYPE BOOL GS_NET_Device_isConnected(DWORD dwDeviceID)
{
	return CUtilityCls::GetInstance()->lpSocket->isConnection(dwDeviceID);
}

HResTimer theNetTimer;
GS_NET_API_FN_TYPE DWORD GS_NET_SendP2Data(BYTE bPid, DWORD dwDeviceID, BYTE bAppID,  BYTE* bpData, BYTE dwHeader, DWORD dwSize, DWORD dwDataType)
{
	P::P2 stPacket2;
	stPacket2.wSize = (WORD)dwSize;
	stPacket2.bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Remote, P::DEST::PC, P::DEST::SubDestDefaultVal);
	stPacket2.bSrc = GS_NET_MakeDestOrSrc(P::SRC::Cmd, P::SRC::Local, P::SRC::Camera, P::SRC::SubSrcDefaultVal);
	stPacket2.bPID = bPid;
	stPacket2.lpData = bpData;
	stPacket2.dwStamp = theNetTimer.GetSystemTime();
	
	stPacket2.mktime = 0;
	stPacket2.bStatus = 0;
	stPacket2.bSentCount = 0;

	BYTE* lpSendData = new BYTE[dwSize + P::P2::HeaderSize + 1];

	CGSNetPacket::SendP2RegularData(&stPacket2, lpSendData);

	DWORD dwByteNum = stPacket2.wSize + P::P2::HeaderSize;

	dwByteNum = GS_NET_SendData(dwDeviceID, bAppID, lpSendData, dwHeader, dwByteNum, dwDataType);

	delete lpSendData;

	return dwByteNum;

}
//dwHeader: 2형파케트의 련번호를 입력.
GS_NET_API_FN_TYPE DWORD GS_NET_SendData(DWORD dwDeviceID, BYTE bAppID,  BYTE* bpData, BYTE dwHeader, DWORD dwSize, DWORD dwDataType)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	if (utilObj == NULL)
		return 0;

	/*if (!utilObj->lpSocket->isConnection(dwDeviceID))
	{
		gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO_CONNECTION;
		utilObj->AddLogText(GS_NET_LOG_ERROR, L"Data send failed. because, no connection");
		return 0;
	}*/

	if (gsNetSendData[dwDeviceID].nListCount > GS_NET_LIST_COUNT)
	{
		//GS_NET_DeleteDataHeader(&gsNetSendData[dwDeviceID]);
		//gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NOMEM;
		utilObj->AddLogText(GS_NET_LOG_ERROR, L"Data send failed. because, no connection or buffer overflow.");
		return 0;
	}

	if (!GS_NET_LOCK(&gsNetSendData[dwDeviceID].hLock))
		return 0;

	DWORD dwReqSendOK = 1;
	GS_NET_DATA*  lpSendData;
	
	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
	{
		GS_NET_UNLOCK(&gsNetSendData[dwDeviceID].hLock);
		return 0;
	}

	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

	lpSendData = (GS_NET_DATA*)malloc(sizeof(GS_NET_DATA));
	if(lpSendData == NULL)
	{
		dwReqSendOK = 0;
		gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NOMEM;
		GS_NET_UNLOCK(&gsNetSendData[dwDeviceID].hLock);
		return dwReqSendOK;
	}

	lpSendData->dwDeviceID = dwDeviceID;
	lpSendData->bAppID = bAppID;
	lpSendData->dwHeader = dwHeader;
	lpSendData->dwDataType = dwDataType;
	lpSendData->dwSize = dwSize;
	lpSendData->lpNext = NULL;
	lpSendData->lpPrev = NULL;
	if(dwSize == 0)
	{
		dwReqSendOK = 0;
		gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_INVALID_DATASIZE;
		delete lpSendData;
		GS_NET_UNLOCK(&gsNetSendData[dwDeviceID].hLock);
		return dwReqSendOK;
	}
	lpSendData->lpData = (BYTE*)malloc(dwSize);
	if(lpSendData->lpData == NULL)
	{
		dwReqSendOK = 0;
		gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NOMEM;
		delete lpSendData;
		GS_NET_UNLOCK(&gsNetSendData[dwDeviceID].hLock);
		return dwReqSendOK;
	}
	memcpy(lpSendData->lpData, bpData, dwSize);
	
	GS_NET_UNLOCK(&gsNetSendData[dwDeviceID].hLock);

	GS_NET_AddDataList(&gsNetSendData[dwDeviceID], lpSendData);

	return dwReqSendOK;
}

GS_NET_API_FN_TYPE void GS_NET_AddPacketSend(BYTE nPktNo, WORD dwSize, BYTE* lpData, BOOL bDestLocal)
{
	CUtilityCls::GetInstance()->lpGsNetPacket->AddPacket(nPktNo, dwSize, lpData, bDestLocal);
}

GS_NET_API_FN_TYPE DWORD GS_NET_RecvData(DWORD dwDeviceID, BYTE bAppID, BYTE* bpData, BYTE dwHeader, DWORD dwSize, DWORD dwDataType)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	if (dwDataType == GS_NET_RS_RAW_DATATYPE)
	{
		DWORD dwRet = 1;
		GS_NET_DATA* lpRecvData;

		//장치ID를 검사한다.
		if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
		{
			return 0;
		}

		if (!GS_NET_LOCK(&gsNetRecvData[dwDeviceID].hLock))
			return 0;

		gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

		lpRecvData = (GS_NET_DATA*)malloc(sizeof(GS_NET_DATA));
		if(lpRecvData == NULL)
		{
			dwRet = 0;
			gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NOMEM;
			GS_NET_UNLOCK(&gsNetRecvData[dwDeviceID].hLock);
			return dwRet;
		}

		lpRecvData->dwDeviceID = dwDeviceID;
		lpRecvData->bAppID = bAppID;//의미 없음.
		lpRecvData->dwDataType = dwDataType;
		lpRecvData->dwHeader = dwHeader;
		lpRecvData->dwSize = dwSize;
		lpRecvData->lpNext = NULL;
		lpRecvData->lpPrev = NULL;
		if(dwSize == 0)
		{
			dwRet = 0;
			gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_INVALID_DATASIZE;
			delete lpRecvData;
			GS_NET_UNLOCK(&gsNetRecvData[dwDeviceID].hLock);
			return dwRet;
		}
		lpRecvData->lpData = (BYTE*)malloc(dwSize);
		if(lpRecvData->lpData == NULL)
		{
			dwRet = 0;
			gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NOMEM;
			delete lpRecvData;
			GS_NET_UNLOCK(&gsNetRecvData[dwDeviceID].hLock);
			return dwRet;
		}
		memcpy(lpRecvData->lpData, bpData,dwSize);

		GS_NET_UNLOCK(&gsNetRecvData[dwDeviceID].hLock);

		GS_NET_AddDataList(&gsNetRecvData[dwDeviceID], lpRecvData);

		//App측의 callback 함수를 호출한다.
		if (!GS_NET_CallCallback(dwDeviceID, dwDataType, GS_CALLBACK_READYDATA))
		{
			CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_ERROR, L" - not define Callback function - ");	
		}

		GS_NET_DeleteDataHeader(&gsNetRecvData[dwDeviceID]); 

		return dwRet;
	} else
	{
		if (bpData[0] == P::P2::bHeader)//2형 파케트
		{
			P::P2 stP2;
			if (CGSNetPacket::RecvP2RegularData(bpData, &stP2))
			{
				utilObj->lpGsNetPacket->procP2(&stP2);
				delete stP2.lpData;
			}		
		} else if (bpData[0] == P::P3::bHeader)//3,4형 파케트
		{
			utilObj->lpGsNetPacket->PicP3P4RecvData(bpData, dwSize);
		}
	}

	return 1;
}

GS_NET_API_FN_TYPE DWORD GS_NET_GetDataSize(DWORD dwDeviceID, BYTE bAppID, DWORD dwDataFilter)
{
	DWORD dwSize = 0;
	GS_NET_DATA*  lpRecvDataNext;

	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
	{
		return 0;
	}

	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

	if (!GS_NET_LOCK(&gsNetRecvData[dwDeviceID].hLock))
		return 0;

	lpRecvDataNext = gsNetRecvData[dwDeviceID].lpFirst ;
	if(lpRecvDataNext != NULL)
	{
		do 
		{
			if((lpRecvDataNext->dwDataType & dwDataFilter) == lpRecvDataNext->dwDataType)
			{
				dwSize = lpRecvDataNext->dwSize; 
				break;
			}
			lpRecvDataNext = lpRecvDataNext->lpNext;
		} while (lpRecvDataNext != NULL);
	}

	GS_NET_UNLOCK(&gsNetRecvData[dwDeviceID].hLock);
	return dwSize;
}

GS_NET_API_FN_TYPE DWORD GS_NET_GetData(DWORD dwDeviceID, BYTE bAppID, BYTE* bpData, DWORD dwDataFilter)
{
	GS_NET_DATA* lpRecvDataNext;

	//장치ID를 검사한다.
	if (GS_NET_CheckDeviceID(dwDeviceID) == GS_NET_ERROR_RETURN)
	{
		return 0;
	}

	gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_NO;

	if (!GS_NET_LOCK(&gsNetRecvData[dwDeviceID].hLock))
		return 0;

	lpRecvDataNext = gsNetRecvData[dwDeviceID].lpFirst ;
	if(lpRecvDataNext != NULL)
	{
		do 
		{
			if((lpRecvDataNext->dwDataType & dwDataFilter) == lpRecvDataNext->dwDataType)
			{
// 				if(dwSize != lpRecvDataNext->dwSize) 
// 				{
// 					gsdwNetErrorCode[dwDeviceID] = GS_NET_ERROR_INVALID_DATASIZE;
// 					return 0;
// 				}
				memcpy(bpData, lpRecvDataNext->lpData, lpRecvDataNext->dwSize);
				return lpRecvDataNext->dwSize;
			}
			lpRecvDataNext = lpRecvDataNext->lpNext;
		} while (lpRecvDataNext != NULL);
	}

	GS_NET_UNLOCK(&gsNetRecvData[dwDeviceID].hLock);

	return 0;
}

GS_NET_API_FN_TYPE DWORD GS_NET_GetLastErrorCode(DWORD dwDeviceID)
{
	if (gsdwNetDeviceErrorCode == GS_NET_ERROR_NO)
		return gsdwNetErrorCode[dwDeviceID];
	else
		return gsdwNetDeviceErrorCode;
}

GS_NET_API_FN_TYPE void GS_NET_FILEPLAY(char* czFileName)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (utilObj->lpH264SearchDecoder->m_bPlayStatus || strcmp(czFileName, "stop") == 0)
	{
		utilObj->lpH264SearchDecoder->Close();
		utilObj->lpH264SearchDecoder->Init();
	} else
	{
		utilObj->lpH264SearchDecoder->Close();
		utilObj->lpH264SearchDecoder->Init();
		utilObj->lpH264SearchDecoder->OpenFile(czFileName);
		INT64 xx = utilObj->lpH264SearchDecoder->GetFrameCount();
		//utilObj->lpH264Decoder->SeekTo(xx);
		utilObj->lpH264SearchDecoder->ReadFrame();

		/*utilObj->lpH264Decoder->Close();
		utilObj->lpH264Decoder->SetViewWindowInfo((CWnd*)&utilObj->lpProjectVTRDlg->m_xCamview);
		utilObj->lpH264Decoder->Init();*/
	}
}

GS_NET_API_FN_TYPE BOOL GS_NET_GCSFileOpen(char* czFileName)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (utilObj->lpH264SearchDecoder->m_bPlayStatus || strcmp(czFileName, "stop") == 0)
	{
		utilObj->lpH264SearchDecoder->Close();
		utilObj->lpH264SearchDecoder->Init();
		return TRUE;
	} else
	{
		utilObj->lpH264SearchDecoder->Close();
		utilObj->lpH264SearchDecoder->Init();
		return utilObj->lpH264SearchDecoder->OpenGCSFile(czFileName);
	}
}

GS_NET_API_FN_TYPE void GS_NET_GCSFilePlay()
{
	CUtilityCls::GetInstance()->lpH264SearchDecoder->fileGCSPlay();
}
GS_NET_API_FN_TYPE void GS_NET_GCSFilePause()
{
	CUtilityCls::GetInstance()->lpH264SearchDecoder->fileGCSStop();
}

GS_NET_API_FN_TYPE BOOL	GS_NET_TCPSend(DWORD dwDeviceID, DWORD nSize, BYTE* lpData)
{
	return CUtilityCls::GetInstance()->lpSocket->SendData(dwDeviceID, nSize, lpData);
}

GS_NET_API_FN_TYPE DWORD GS_NET_TCPRecv(DWORD dwDeviceID, DWORD nSize, BYTE* lpData)
{
	return CUtilityCls::GetInstance()->lpSocket->RecvData(dwDeviceID, nSize, lpData);
}

GS_NET_API_FN_TYPE void GS_NET_GetLogTxtList(gs_logType* logList)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	memcpy(logList, utilObj->m_stLogList, sizeof(utilObj->m_stLogList));
}

GS_NET_API_FN_TYPE void GS_NET_AddLogTxt(WCHAR* wzStr)
{
	CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_APP_LAYER_STATUS, wzStr);
}

GS_NET_API_FN_TYPE void GS_NET_SystemInfo(gs_pkt_GCS_Status_Type* m_stPktGCSStatus, gs_pkt_UAV_Status_Type* m_stPktUAVStatus)
{
	*m_stPktGCSStatus = CUtilityCls::GetInstance()->m_stPktGCSStatus;
	*m_stPktUAVStatus = CUtilityCls::GetInstance()->m_stPktUAVStatus;

	//memset(&m_stPktGCSStatus, &CUtilityCls::GetInstance()->m_stPktGCSStatus, sizeof(gs_pkt_GCS_Status_Type));
	//memset(&m_stPktUAVStatus, &CUtilityCls::GetInstance()->m_stPktUAVStatus, sizeof(gs_pkt_UAV_Status_Type));
}

GS_NET_API_FN_TYPE BOOL GS_NET_GetVideoStabEnable(void)
{
	return CUtilityCls::GetInstance()->m_bVStab;
}

GS_NET_API_FN_TYPE void GS_NET_SetVideoStabEnable(BOOL bStatus)
{
	CUtilityCls::GetInstance()->m_bVStab = bStatus;
}

GS_NET_API_FN_TYPE void GS_NET_SetVideoStabParam(int nInFrameRate, int nOutFrameRate, int nImageLevel)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->m_nVStabInFrameCounts = nInFrameRate;
	utilObj->m_nVStabOutFrameCounts = nOutFrameRate;
	utilObj->m_nVStabImageLevel = nImageLevel;
}

GS_NET_API_FN_TYPE void GS_NET_SetVideoStabCrop(int nXpos, int nYpos)
{
	CUtilityCls::GetInstance()->lpH264LiveDecoder->SetCrop(nXpos, nYpos);
}


GS_NET_API_FN_TYPE void GS_NET_SetGCSPlayPosition(int nPos)
{
	CUtilityCls::GetInstance()->lpH264SearchDecoder->fileGCSPosition(nPos);
}

GS_NET_API_FN_TYPE int GS_NET_GetGCSPlayPosition()
{
	return CUtilityCls::GetInstance()->lpH264SearchDecoder->m_nCurFramePosition;
}

GS_NET_API_FN_TYPE DWORD GS_NET_SetGCSPlayTimePosition(DWORD nTickTime)
{
	return CUtilityCls::GetInstance()->lpH264SearchDecoder->SetfileGCSTimePosition(nTickTime);
}

GS_NET_API_FN_TYPE DWORD GS_NET_GetGCSPlayTimePosition()
{
	return CUtilityCls::GetInstance()->lpH264SearchDecoder->GetfileGCSTimePosition();
}

GS_NET_API_FN_TYPE int GS_NET_GetGCSFrameCount()
{
	return CUtilityCls::GetInstance()->lpH264SearchDecoder->GetGCSFrameCount();
}

GS_NET_API_FN_TYPE DWORD GS_NET_GetGCSFrameTotalTime()
{
	return CUtilityCls::GetInstance()->lpH264SearchDecoder->GetGCSFrameTotalTime();
}

GS_NET_API_FN_TYPE void GS_NET_HeaderGenerate(int nInterval)
{
	CFileDialog dlg( TRUE, _T("*.H264"), _T("*.H264"), OFN_HIDEREADONLY|OFN_OVERWRITEPROMPT, _T("Animation (*.h264)|*.AVI|All Files (*.*)|*.*||"));

	if( dlg.DoModal()!=IDOK )
	{
		return;
	}

	CString fileName = dlg.GetPathName();
	CString fileHeaderName;

	fileHeaderName = fileName.Left(fileName.Find(L"."));
	fileHeaderName = fileHeaderName + L".dta";

	CFile fileVideoData, fileHeaderData;

	if (fileVideoData.Open(fileName, CFile::modeNoTruncate | CFile::modeReadWrite | CFile::typeBinary | CFile::shareDenyNone))
	{
		AfxMessageBox(L"Header File을 생성합니다. 원래의 Header File을 갱신합니다.");
		DeleteFile(fileHeaderName);
		fileHeaderData.Open(fileHeaderName, CFile::modeCreate | CFile::modeNoTruncate | CFile::modeReadWrite | CFile::typeBinary | CFile::shareDenyNone);

		BOOL bGenerateStatus = TRUE;
		fileVideoData.SeekToBegin();
		fileHeaderData.SeekToBegin();

		int nSize = (int)fileVideoData.GetLength();
		int nReadBytes;
		if (nSize > 0)
		{
			char tmpVideoBuffer[1024];
			char czData[128];
			DWORD dwTickCount = 0;

			GSFrameBuffer lpFrameObj;
			lpFrameObj.init(GS_NET_VIDEO_BUFFER_SIZE);

			BYTE* m_lpVideoBuffer = new BYTE[GS_NET_VIDEO_BUFFER_SIZE];

			while (TRUE)
			{
				nReadBytes = fileVideoData.Read(tmpVideoBuffer, 1024);
				if (nReadBytes <= 0)
					break;

				if (!lpFrameObj.append(tmpVideoBuffer, nReadBytes))
				{
					bGenerateStatus = FALSE;
					AfxMessageBox(L"Header File생성 실패.");
					break;
				}
				
				while((nSize = lpFrameObj.getFrameBuffer(m_lpVideoBuffer)) > 0)
				{
					sprintf(czData, "Key=%d,Size=%d,Time=%d", 2, nSize, dwTickCount);
					fileHeaderData.Write(czData, strlen(czData));
					czData[0] = 0x0D;
					czData[1] = 0x0A;
					fileHeaderData.Write(czData, 2);

					dwTickCount += nInterval;
				}
			}

			delete m_lpVideoBuffer;
		}

		fileVideoData.Close();
		fileHeaderData.Close();

		if (bGenerateStatus)
			AfxMessageBox(L"Header File이 생성되였습니다.");
	}	
}

GS_NET_API_FN_TYPE void GS_NET_SetLiveEnable(BOOL bEnable)
{
	CUtilityCls::GetInstance()->m_SystemInfo.bLiveEnable = bEnable;
}

GS_NET_API_FN_TYPE BOOL GS_NET_GetLiveEnable()
{
	return CUtilityCls::GetInstance()->m_SystemInfo.bLiveEnable;
}

GS_NET_API_FN_TYPE BOOL GS_NET_VideoImageCapture(BOOL bLive)
{
	if (bLive)
		return CUtilityCls::GetInstance()->lpH264LiveDecoder->VideoImageCapture();
	else
		return CUtilityCls::GetInstance()->lpH264SearchDecoder->VideoImageCapture();
}