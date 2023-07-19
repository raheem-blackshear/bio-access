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
// 2014.10.17  전광명  최초 프로그람 작성                                     
//----------------------------------------------------------------------------

#pragma once

#include "GSNetCommon.h"

class GSPC_Socket
{
public:

	GSPC_Socket();
	~GSPC_Socket(void);

public:
	DWORD m_nDeviceID;
	char m_szServerIP[100];
	u_short m_nServerPort;
	BOOL  m_bIsServer;
	BOOL  m_bIsUDP;
	BOOL  m_bIsRawData;
	BOOL  m_bIsEnable;
	BOOL  m_bIsThreadTrans;
	CRITICAL_SECTION m_csCreateSection;

	WSADATA m_wWsadata;
	WSAOVERLAPPED m_wSendOverlapped, m_wRecvOverlapped;
	WSABUF m_wDataSendBuf, m_wDataRecvBuf;
	int m_nResult;

	SOCKET m_gSocket;//udp 인 경우 자료보내는 소켓
	SOCKET m_actSocket; //udp인 경우 자료 받는 소켓, tcp인경우 accept connection
	sockaddr_in m_sockInfo, m_sockInfoRecv;
	HANDLE m_hRSThread[2];
	BOOL   m_bIsClose;
	BOOL   m_bIsConnection;	//connection 상태, 련결:True 아니면: False

public:
	void   SetInfo(DWORD dwDeviceID, char* serverIP, DWORD serverPort, BOOL fIsServer, BOOL fIsUDP, BOOL fIsRawData, BOOL fIsEnable, BOOL fThreadTrans);
	DWORD  Create();
	BOOL  SendData(DWORD nSize, BYTE* lpData);
	DWORD  RecvData(DWORD nSize, BYTE* lpData);
	DWORD  Close();

};

class GSPC_SocketList
{
public:
	GSPC_SocketList();
	~GSPC_SocketList();

	GSPC_Socket m_socket[GS_DEVICE_MAX];

	DWORD  Create(DWORD deviceID);
	BOOL  isConnection(DWORD deviceID);
	BOOL  SendData(DWORD deviceID, DWORD nSize, BYTE* lpData);
	DWORD  RecvData(DWORD deviceID, DWORD nSize, BYTE* lpData);
	DWORD  Close(DWORD deviceID);

};
