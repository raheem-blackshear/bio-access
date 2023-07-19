//------------------------------------------------------------------------------
//  NAME : GSPC_Socket.h
//  DESC : Socket 송수신스레드를 만든다.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  변         경         사         항                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.10.17  KM  최초 프로그람 작성                                     
//----------------------------------------------------------------------------

#include "StdAfx.h"
#include "GSPC_Socket.h"
#include <process.h>
#include "GSUtilityCls.h"

class CGSproDlg;

HResTimer theSocketTimer;
//===========================================================

GSPC_SocketList::GSPC_SocketList()
{
	AfxSocketInit();

	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	for (int i=0; i<utilObj->m_listDeviceInfo.Count(); i++)
	{
		deviceInfo* devInfo = (deviceInfo*)utilObj->m_listDeviceInfo.Get(i);
		m_socket[devInfo->deviceId].SetInfo(devInfo->deviceId, devInfo->deviceIp, devInfo->devicePort, devInfo->isServer, devInfo->isUDP, devInfo->isRawData, devInfo->isEnable, devInfo->isThreadTrans);
	}
}

GSPC_SocketList::~GSPC_SocketList()
{

}

DWORD  GSPC_SocketList::Create(DWORD deviceID)
{
	return m_socket[deviceID].Create();
}

BOOL  GSPC_SocketList::isConnection(DWORD deviceID)
{
	if (m_socket)
		return m_socket[deviceID].m_bIsConnection;
	else
		return FALSE;
}

BOOL  GSPC_SocketList::SendData(DWORD deviceID, DWORD nSize, BYTE* lpData)
{
	return m_socket[deviceID].SendData(nSize, lpData);
}

DWORD  GSPC_SocketList::RecvData(DWORD deviceID, DWORD nSize, BYTE* lpData)
{
	return m_socket[deviceID].RecvData(nSize,lpData);
}

DWORD  GSPC_SocketList::Close(DWORD deviceID)
{
	return m_socket[deviceID].Close();
}


//===========================================================
//==========           create         ===========
BOOL procCreateFunc(GSPC_Socket* gsObj)
{
	if (IsBadReadPtr(gsObj, 12))
		goto rFailed;

	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (IsBadReadPtr(utilObj, 4))
		goto rFailed;

	EnterCriticalSection(&gsObj->m_csCreateSection);

	if (gsObj->m_bIsClose)
		return FALSE;

	if (gsObj->m_bIsConnection && gsObj->m_gSocket != INVALID_SOCKET)
		goto rSuccess;

	utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:장치련결중...", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp);

	if (gsObj->m_bIsUDP) //UDP인 경우
	{
		//send용
		gsObj->m_gSocket = WSASocket(AF_INET, SOCK_DGRAM, IPPROTO_UDP, NULL, 0, WSA_FLAG_OVERLAPPED);
		gsObj->m_actSocket = WSASocket(AF_INET, SOCK_DGRAM, IPPROTO_UDP, NULL, 0, WSA_FLAG_OVERLAPPED);

		if(gsObj->m_gSocket == INVALID_SOCKET || gsObj->m_actSocket == INVALID_SOCKET)
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"Socket error %d", WSAGetLastError());
			goto rFailed;
		}

		BOOL optval = TRUE;		
		if (setsockopt(gsObj->m_gSocket, SOL_SOCKET, SO_BROADCAST, (const char*)&optval, sizeof(optval)) == SOCKET_ERROR)
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"setsockopt failed: %d", WSAGetLastError());
			closesocket(gsObj->m_gSocket);
			goto rFailed;
		}else
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"Set SO_BROADCAST: ON");

		if (setsockopt(gsObj->m_actSocket, SOL_SOCKET, SO_BROADCAST, (const char*)&optval, sizeof(optval)) == SOCKET_ERROR)
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"setsockopt failed: %d", WSAGetLastError());
			closesocket(gsObj->m_gSocket);
			goto rFailed;
		}

		gsObj->m_sockInfo.sin_family = AF_INET;
		gsObj->m_sockInfo.sin_port = htons(gsObj->m_nServerPort);
		gsObj->m_sockInfo.sin_addr.s_addr = inet_addr("255.255.255.255");

		//recv 용
		gsObj->m_sockInfoRecv.sin_family = AF_INET;
		gsObj->m_sockInfoRecv.sin_addr.s_addr = htonl(INADDR_ANY);
		gsObj->m_sockInfoRecv.sin_port = htons(gsObj->m_nServerPort);

		if (bind(gsObj->m_actSocket, (SOCKADDR *) &gsObj->m_sockInfoRecv, sizeof(gsObj->m_sockInfoRecv)) == SOCKET_ERROR)
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"bind failed: %d",WSAGetLastError());
			closesocket(gsObj->m_actSocket);
			closesocket(gsObj->m_gSocket);
			goto rFailed;
		}
	}
	else  // TCP 인 경우 
	{
		if (gsObj->m_wRecvOverlapped.hEvent == NULL || gsObj->m_wRecvOverlapped.hEvent == NULL) 
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"WSACreateEvent failed: %d", WSAGetLastError());
			goto rFailed;
		}

		gsObj->m_gSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
		if(gsObj->m_gSocket == INVALID_SOCKET)
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"Socket error %d", WSAGetLastError());
			goto rFailed;
		}

		gsObj->m_sockInfo.sin_family = AF_INET;
		gsObj->m_sockInfo.sin_port = htons(gsObj->m_nServerPort);
		gsObj->m_sockInfo.sin_addr.s_addr = inet_addr(gsObj->m_szServerIP);

		BOOL optval = TRUE;		
		if (setsockopt(gsObj->m_gSocket, SOL_SOCKET, SO_REUSEADDR, (const char*)&optval, sizeof(optval)) == SOCKET_ERROR)
		{
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"setsockopt failed: %d", WSAGetLastError());
			closesocket(gsObj->m_gSocket);
			goto rFailed;
		} 

		if (gsObj->m_bIsServer) // server 인 경우
		{
			if (bind(gsObj->m_gSocket, (SOCKADDR*) &gsObj->m_sockInfo, sizeof(gsObj->m_sockInfo)) == SOCKET_ERROR) 
			{
				utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:bind failed: %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, WSAGetLastError());
				closesocket(gsObj->m_gSocket);
				goto rFailed;
			}

			if (listen(gsObj->m_gSocket, SOMAXCONN) == SOCKET_ERROR)
			{
				utilObj->AddLogText(GS_NET_LOG_DEVICE, L"listen failed: %d", WSAGetLastError());
				closesocket(gsObj->m_gSocket);
				goto rFailed;
			}

			gsObj->m_actSocket = accept( gsObj->m_gSocket, NULL, NULL );
			if (gsObj->m_actSocket == INVALID_SOCKET) 
			{
				utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:accept failed: %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, WSAGetLastError());
				closesocket(gsObj->m_gSocket);
				goto rFailed;
			}			
		} else //client 인 경우
		{
			
			if (connect(gsObj->m_gSocket, (SOCKADDR*) &gsObj->m_sockInfo, sizeof(gsObj->m_sockInfo)) == SOCKET_ERROR) 
			{
				utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:connect failed: %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, WSAGetLastError());
				closesocket(gsObj->m_gSocket);
				
				goto rFailed;
			}
		}
	}

	utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:장치열기 성공.", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp);
	
rSuccess:
	gsObj->m_bIsConnection = TRUE;
	LeaveCriticalSection(&gsObj->m_csCreateSection);
	return TRUE;

rFailed:
	gsObj->m_bIsConnection = FALSE;
	gsObj->m_gSocket = SOCKET_ERROR;
	gsObj->m_actSocket = SOCKET_ERROR;
	LeaveCriticalSection(&gsObj->m_csCreateSection);

	Sleep(1000);

	return FALSE;
}

//==========           send         ===========
void __cdecl procSendThread(void* pArg)
{
	GSPC_Socket *gsObj = (GSPC_Socket *)pArg;
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:Device ID = %d : 송신 스레드 기동", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, gsObj->m_nDeviceID);

	DWORD dwRealDataSize = GS_NET_SEND_BUFFER_SIZE;
	BYTE* lpRealData = new BYTE[dwRealDataSize];

	int iResult = 0, err = 0;
	DWORD nSendBytes = 0;
	DWORD nFlag = 0;
	DWORD dwRealSize;

	SOCKET* tmpSocket;

	while (TRUE)
	{
		if (IsBadReadPtr(gsObj, 12) || IsBadReadPtr(utilObj, 4))
		{
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:procSendThread  스레드오유완료 Error !!!", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp);
			return;
		}

		if (gsObj->m_bIsClose)
		{
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:devid=%d, 송신 스레드 완료 OK!!!", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceId);
			delete lpRealData;
			_endthread();
			return;
		}

		if (!procCreateFunc(gsObj))
		{
			Sleep(500);
			continue;
		}

		GS_NET_DATA* lpSendData = gsNetSendData[gsObj->m_nDeviceID].lpFirst;
		if (lpSendData == NULL)
		{
			Sleep(10);
			continue;
		}

		while (lpSendData != NULL)
		{
			dwRealSize = lpSendData->dwSize + GS_NET_RS_HEAD_DATASIZE;

			if (dwRealSize > dwRealDataSize)
			{
				dwRealDataSize = dwRealSize + 1;
				delete lpRealData;
				lpRealData = new BYTE [dwRealDataSize];
			}
			
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:%d Bytes 전송중", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, dwRealSize);

			if (gsObj->m_bIsRawData) //생자료보내기
			{
				dwRealSize = lpSendData->dwSize;
				memcpy(lpRealData, lpSendData->lpData, lpSendData->dwSize);
			}
			else
				GS_NET_GetSendRegularData(lpSendData->dwHeader, lpSendData->dwSize, lpSendData->dwDataType, lpSendData->lpData, lpRealData);
			
//rReSend:
			while (dwRealSize > 0)
			{
				if (dwRealSize <= GS_NET_SEND_BUFFER_SIZE)
					gsObj->m_wDataSendBuf.len = dwRealSize;
				else
					gsObj->m_wDataSendBuf.len = GS_NET_SEND_BUFFER_SIZE;

				gsObj->m_wDataSendBuf.buf = (char*)lpRealData;

				if (gsObj->m_bIsUDP == TRUE) //UDP
				{
					tmpSocket = &gsObj->m_gSocket;
					iResult = WSASendTo(*tmpSocket, &gsObj->m_wDataSendBuf, 1, &nSendBytes, nFlag, (SOCKADDR*) &gsObj->m_sockInfo, sizeof(gsObj->m_sockInfo), &gsObj->m_wSendOverlapped, NULL);
				}
				else // TCP
				{
					tmpSocket = gsObj->m_bIsServer==TRUE?&gsObj->m_actSocket:&gsObj->m_gSocket;
					iResult = WSASend(*tmpSocket, &gsObj->m_wDataSendBuf, 1, &nSendBytes, 0, &gsObj->m_wSendOverlapped, NULL);
				}

				err = WSAGetLastError();

				if (WSAENOBUFS == err || WSAEFAULT == err)
				{
					utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSASend failed: ( Buffer deadlock 5 second standby) %d", err);
					Sleep(500);
					//goto rReSend;
				}

				if (iResult == SOCKET_ERROR && (WSA_IO_PENDING != err))
				{
					utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSASend failed: %d", err);
					GS_NET_CallCallback(gsObj->m_nDeviceID, lpSendData->dwDataType, GS_CALLBACK_ERROR);
					goto rFailed;
				}

				if (WSAWaitForMultipleEvents(1, &gsObj->m_wSendOverlapped.hEvent, TRUE, INFINITE, TRUE) == WSA_WAIT_FAILED) {
					utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSAWaitForMultipleEvents failed: %d", err);
					goto rFailed;
				}

				if (WSAGetOverlappedResult(*tmpSocket, &gsObj->m_wSendOverlapped, &nSendBytes, FALSE, &nFlag) == FALSE) {
					utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSASend operation failed: %d", err);
					goto rFailed;
				}
				
				WSAResetEvent(gsObj->m_wSendOverlapped.hEvent);
				utilObj->AddLogText(GS_NET_LOG_SOCKET, L"procSendThread  %d Byte sended", nSendBytes);
				
				dwRealSize -= nSendBytes;
				if (dwRealSize > 0)
					memcpy(lpRealData, &lpRealData[nSendBytes], dwRealSize);
			}

			GS_NET_CallCallback(gsObj->m_nDeviceID, lpSendData->dwDataType, GS_CALLBACK_SENT);

			GS_NET_DeleteDataHeader(&gsNetSendData[gsObj->m_nDeviceID]);

			lpSendData = gsNetSendData[gsObj->m_nDeviceID].lpFirst;

			if (gsObj->m_bIsUDP == TRUE) //UDP
				Sleep(1);

			continue;
rFailed:
			utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:connection Failed : %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, WSAGetLastError());
			gsObj->m_bIsConnection = FALSE;
			closesocket(gsObj->m_actSocket);
			closesocket(gsObj->m_gSocket);
			gsObj->m_gSocket = SOCKET_ERROR;
			gsObj->m_actSocket = SOCKET_ERROR;
			GS_NET_DeleteDataHeader(&gsNetSendData[gsObj->m_nDeviceID]);

			break;
		}
	}

	return;
}

static int xxx = 0;
//==========           recv         ===========
void __cdecl procRecvThread(void* pArg)
{
	GSPC_Socket *gsObj = (GSPC_Socket *)pArg;
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S: Device ID = %d : 수신 스레드 기동", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, gsObj->m_nDeviceID);

	BYTE* tempBuffer = new BYTE[GS_NET_RECV_BUFFER_SIZE + 1];
	BYTE* recvBuffer = new BYTE[GS_NET_RECV_BUFFER_SIZE + 1];
	BYTE* lpRealData = new BYTE[GS_NET_RECV_BUFFER_SIZE + 1];

	int nTempBufferLen = 0;
	int err;
	int nFirst;
	SOCKET* tmpSocket;

	int SenderAddrSize = sizeof(gsObj->m_sockInfoRecv);
	DWORD nFlags = 0;
	DWORD nRecvBytes = 0;
	BYTE dwHeader;
	DWORD dwSize = GS_NET_RS_HEAD_DATASIZE;
	DWORD dwDataType;

	BOOL bTcpStatus = TRUE;
	if (gsObj->m_nDeviceID == GS_DEV_PC_UDP_SERVER || gsObj->m_nDeviceID == GS_DEV_PC_UDP_CLIENT || gsObj->m_bIsRawData)
		bTcpStatus = FALSE;

	int nPacketSize = 0;
	int iResult = 0;

	if (gsObj->m_bIsRawData)
		dwSize = 2048;
	else
		dwSize = GS_NET_RS_HEAD_DATASIZE;

	while (TRUE)
	{
		if (IsBadReadPtr(gsObj, 12) || IsBadReadPtr(utilObj, 4))
		{
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:procRecvThread  스레드오유완료 Error !!!", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp);
			return;
		}

		if (gsObj->m_bIsClose)
		{
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:devid=%d, 수신 스레드 완료 OK!!!", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceId);
			
			delete tempBuffer;
		    delete recvBuffer;
		    delete lpRealData;

			_endthread();
			return;
		}

		if (!procCreateFunc(gsObj))
		{
			Sleep(500);
			continue;
		}
		
		if (!bTcpStatus)
			dwSize = 400*1024;

		gsObj->m_wDataRecvBuf.buf = (char*)recvBuffer;

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////  Recv Start  //////////////////////////////////////////////////
		while (dwSize > 0)
		{
			gsObj->m_wDataRecvBuf.len = dwSize;

			nFlags = 0;
			if (gsObj->m_bIsUDP) //UDP
			{
				tmpSocket = &gsObj->m_actSocket;
				iResult = WSARecvFrom(*tmpSocket, &gsObj->m_wDataRecvBuf, 1, &nRecvBytes, &nFlags, (SOCKADDR *) &gsObj->m_sockInfoRecv, &SenderAddrSize, &gsObj->m_wRecvOverlapped, NULL);
			}
			else //TCP
			{
				tmpSocket = gsObj->m_bIsServer==TRUE?&gsObj->m_actSocket:&gsObj->m_gSocket;
				iResult = WSARecv(*tmpSocket, &gsObj->m_wDataRecvBuf, 1, &nRecvBytes, &nFlags, &gsObj->m_wRecvOverlapped, NULL);
			}

			err = WSAGetLastError();

			if (WSAENOBUFS == err || WSAEFAULT == err)
			{
				utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSARecv failed: ( Buffer deadlock 5 second standby) %d", err);
				Sleep(500);
			}

			if (iResult == SOCKET_ERROR && (WSA_IO_PENDING != err))
			{
				utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSARecv failed: %d", err);
				goto rFailed;
			}

			iResult = WSAWaitForMultipleEvents(1, &gsObj->m_wRecvOverlapped.hEvent, TRUE, 5000/*WSA_INFINITE*/, TRUE);
			if (iResult == WSA_WAIT_FAILED) {
				err = WSAGetLastError();
				utilObj->AddLogText(GS_NET_LOG_SOCKET, L"WSAWaitForMultipleEvents failed: %d", err);
				goto rFailed;
			}

			if (WSAGetOverlappedResult(*tmpSocket, &gsObj->m_wRecvOverlapped, &nRecvBytes, FALSE, &nFlags) == FALSE) {
				err = WSAGetLastError();
				utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:WSARecv failed: %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, err);
				if (err == 10040)
					utilObj->AddLogText(GS_NET_LOG_SOCKET, L"Message too long.");
				else
					goto rFailed;
			}

			if (nRecvBytes <= 0)
			{
				utilObj->AddLogText(GS_NET_LOG_SOCKET, L"%S:Received 0byte, will be connection Failed error code: %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, err);
				goto rFailed;
			}

			utilObj->m_nIncomingDataSpeed += nRecvBytes;

			if (!gsObj->m_bIsRawData)
			{
				memcpy(&tempBuffer[nTempBufferLen], recvBuffer, nRecvBytes);
				nTempBufferLen += nRecvBytes;

				dwSize -= nRecvBytes;
				if (dwSize < 0)
				{
					ASSERT(TRUE);
					break;
				}
			}

			//TRACE(L"dwsize=%d, send=%d, recv=%d, time=%d\n", dwSize, gsObj->m_wDataRecvBuf.len, nRecvBytes, theSocketTimer.GetSystemTime() - xxx);
			//xxx = theSocketTimer.GetSystemTime();

			if (!bTcpStatus)
				break;
		}
		/////////////////////////////////////////////////////   Recv End  /////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		utilObj->AddLogText(GS_NET_LOG_SOCKET, L"Recving data %d Bytes", nRecvBytes);
		if (gsObj->m_bIsRawData)
		{
			GS_NET_RecvData(gsObj->m_nDeviceID, 0, recvBuffer, P::P2::bHeader, nRecvBytes, GS_NET_RS_RAW_DATATYPE);
		} else
		{
			while (nTempBufferLen >= GS_NET_RS_HEAD_DATASIZE)
			{
				nFirst = gMemFind((char*)tempBuffer, nTempBufferLen, GS_NET_RS_HEAD_ST_STRING, GS_NET_RS_HEAD_ST_STRING_LEN, 0);
				if (nFirst < 0)
				{
					nTempBufferLen = 0;
					break;
				}

				if (nFirst != 0)
				{
					nTempBufferLen -= nFirst;
					memcpy(tempBuffer, &tempBuffer[nFirst], nTempBufferLen);
				}

				if (nTempBufferLen < GS_NET_RS_HEAD_DATASIZE)
					break;

				GS_NET_GetRecvRegularData(tempBuffer, &dwHeader, &dwSize, &dwDataType, lpRealData);

				nPacketSize = dwSize + GS_NET_RS_HEAD_DATASIZE;

				if (nTempBufferLen >= nPacketSize)
				{
					if (GS_NET_RecvData(gsObj->m_nDeviceID, 0, lpRealData, dwHeader, dwSize, dwDataType) == 0)
					{
						utilObj->AddLogText(GS_NET_LOG_SOCKET, L"Recv Data error : Size 0 or invalid data");
						memcpy(tempBuffer, recvBuffer, nRecvBytes);
						nTempBufferLen = nRecvBytes;
						nRecvBytes = 0;
							continue;
					}
					nTempBufferLen -= nPacketSize;

					if (nTempBufferLen > 0)
						memcpy(tempBuffer, &tempBuffer[nPacketSize], nTempBufferLen);

					dwSize = GS_NET_RS_HEAD_DATASIZE;
				} else
					break;
			}
		}
		
		WSAResetEvent(gsObj->m_wRecvOverlapped.hEvent);

		if (dwSize == 0)
		{
			if (gsObj->m_bIsRawData)
				dwSize = 2048;
			else
				dwSize = GS_NET_RS_HEAD_DATASIZE;
		}
		
		continue;
rFailed:
		if (gsObj->m_bIsRawData)
			dwSize = 2048;//GS_NET_SEND_BUFFER_SIZE;
		else
			dwSize = GS_NET_RS_HEAD_DATASIZE;

		utilObj->AddLogText(GS_NET_LOG_DEVICE, L"%S:connection Failed : %d", ((deviceInfo*)utilObj->m_listDeviceInfo.Get(gsObj->m_nDeviceID))->deviceIp, WSAGetLastError());
		gsObj->m_bIsConnection = FALSE;
		closesocket(gsObj->m_actSocket);
		closesocket(gsObj->m_gSocket);
		gsObj->m_gSocket = SOCKET_ERROR;
		gsObj->m_actSocket = SOCKET_ERROR;
	}

	return;
}


GSPC_Socket::GSPC_Socket()
{
	m_bIsClose = TRUE;
	m_bIsConnection = FALSE;
	
	m_gSocket = INVALID_SOCKET;
	m_actSocket = INVALID_SOCKET;

	m_nResult = WSAStartup(MAKEWORD(2,2), &m_wWsadata);
	if (m_nResult != 0)
		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, L"Unable to load Winsock %d", WSAGetLastError());

}

GSPC_Socket::~GSPC_Socket(void)
{
	m_bIsClose = TRUE;

	WSACleanup();
}

void GSPC_Socket::SetInfo(DWORD dwDeviceID, char* serverIP, DWORD serverPort, BOOL fIsServer, BOOL fIsUDP, BOOL fIsRawData, BOOL fIsEnable, BOOL fThreadTrans)
{
	m_nDeviceID = dwDeviceID;
	if (dwDeviceID == GS_DEV_PC_TCP_SERVER)
	{
		/*hostent *thisHost;
		thisHost = gethostbyname("");
		serverIP = inet_ntoa(*(struct in_addr *) *thisHost->h_addr_list);*/
	}

	sprintf(m_szServerIP, "%s", serverIP);
	m_nServerPort = (u_short)serverPort;
	m_bIsServer = fIsServer;
	m_bIsUDP = fIsUDP;
	m_bIsRawData = fIsRawData;
	m_bIsEnable = fIsEnable;
	m_bIsThreadTrans = fThreadTrans;
}

DWORD GSPC_Socket::Create()
{
	if (!m_bIsEnable)
		return FALSE;

	if (m_bIsConnection)
	{
		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, _T("%S:장치가 이미 열려있습니다."), ((deviceInfo*)CUtilityCls::GetInstance()->m_listDeviceInfo.Get(m_nDeviceID))->deviceIp);
		return GS_NET_ERROR_NO;
	}

	if (m_bIsClose == FALSE)
	{
		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, _T("현재 장치는 열기대기중입니다. "));
		return GS_NET_ERROR_RETURN;
	}

	m_bIsClose = FALSE;
	m_bIsConnection = FALSE;

	InitializeCriticalSection(&m_csCreateSection);

	SecureZeroMemory(&m_wSendOverlapped, sizeof(m_wSendOverlapped));
	SecureZeroMemory(&m_wRecvOverlapped, sizeof(m_wSendOverlapped));

	m_wSendOverlapped.hEvent = WSACreateEvent();
	m_wRecvOverlapped.hEvent = WSACreateEvent();

	if (m_bIsThreadTrans)
	{
		m_hRSThread[0] = (HANDLE)_beginthread(&procSendThread, 0, this);
		m_hRSThread[1] = (HANDLE)_beginthread(&procRecvThread, 0, this);
	}

	//SetThreadPriority(m_hSendThread, THREAD_PRIORITY_NORMAL);

	return GS_NET_ERROR_NO;
}

BOOL GSPC_Socket::SendData(DWORD nSize, BYTE* lpData)
{
	while (!m_bIsConnection)
	{
		Sleep(100);
	}

	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	SOCKET* tmpSocket = m_bIsServer==TRUE?&m_actSocket:&m_gSocket;
	int iResult = send(*tmpSocket, (char*)lpData, nSize, 0 );

	if (iResult == SOCKET_ERROR) {
		utilObj->AddLogText(GS_NET_LOG_SOCKET, L"send failed with error: %d", WSAGetLastError());
		
		m_bIsConnection = FALSE;
		closesocket(m_actSocket);
		closesocket(m_gSocket);
		m_gSocket = SOCKET_ERROR;
		m_actSocket = SOCKET_ERROR;
		
		return FALSE;
	}

	return TRUE;
}

DWORD  GSPC_Socket::RecvData(DWORD nSize, BYTE* lpData)
{
	while (!m_bIsConnection)
	{
		Sleep(100);
	}

	BYTE* lpTempData = lpData;

	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	SOCKET* tmpSocket = m_bIsServer==TRUE?&m_actSocket:&m_gSocket;

	int iResult;
	while (nSize > 0)
	{
		iResult = recv(*tmpSocket, (char*)lpTempData, nSize, 0);

		if (iResult > 0)
		{
			nSize -= iResult;
			lpTempData += iResult;
		} else if (iResult == 0) {
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"Recv failed with error: %d", WSAGetLastError());

			m_bIsConnection = FALSE;
			closesocket(m_actSocket);
			closesocket(m_gSocket);
			m_gSocket = SOCKET_ERROR;
			m_actSocket = SOCKET_ERROR;

			return 0;
		} else
		{
			utilObj->AddLogText(GS_NET_LOG_SOCKET, L"Recv failed with error: %d", WSAGetLastError());
			return 0;
		}
	}
	

	return iResult;
}

DWORD  GSPC_Socket::Close()
{
	if (!m_bIsEnable)
		return TRUE;

	if (m_bIsClose)
	{
		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, _T("%S:이 장치는 이미 완료되였습니다."), ((deviceInfo*)CUtilityCls::GetInstance()->m_listDeviceInfo.Get(m_nDeviceID))->deviceIp);
		return GS_NET_ERROR_NO;
	}

	CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, _T("%S:장치닫기중..."), ((deviceInfo*)CUtilityCls::GetInstance()->m_listDeviceInfo.Get(m_nDeviceID))->deviceIp);

	m_bIsClose = TRUE;
	m_bIsConnection = FALSE;

	LeaveCriticalSection(&m_csCreateSection);
	
	WSACloseEvent(m_wSendOverlapped.hEvent);
	WSACloseEvent(m_wRecvOverlapped.hEvent);

	shutdown(m_actSocket, SD_SEND);
	shutdown(m_actSocket, SD_RECEIVE);
	closesocket(m_actSocket);

	shutdown(m_gSocket, SD_SEND);
	shutdown(m_gSocket, SD_RECEIVE);
	closesocket(m_gSocket);

	m_actSocket = INVALID_SOCKET;
	m_gSocket = INVALID_SOCKET;

	DWORD dwResult = WaitForMultipleObjects(2, m_hRSThread, TRUE, INFINITE);

	if (dwResult != WAIT_OBJECT_0) {
		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, L"error - calling WaitForMultipleObjects(). device ID = %d", m_nDeviceID);
		return GS_NET_ERROR_RETURN;
	}

	//if (m_hRSThread[0] == INVALID_HANDLE_VALUE)
	//CloseHandle(m_hRSThread[0]);
	//CloseHandle(m_hRSThread[1]);

	DeleteCriticalSection(&m_csCreateSection);

	//DWORD dwExitCode;
	//GetExitCodeThread(m_hRecvThread, &dwExitCode);

	//TerminateThread(m_hSendThread, dwExitCode);
	//TerminateThread(m_hRecvThread, dwExitCode);

	CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_DEVICE, _T("%S:장치가 정확히 완료되였습니다."), ((deviceInfo*)CUtilityCls::GetInstance()->m_listDeviceInfo.Get(m_nDeviceID))->deviceIp);

	return GS_NET_ERROR_NO;
}