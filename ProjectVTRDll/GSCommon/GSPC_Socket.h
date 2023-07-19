//------------------------------------------------------------------------------
//  NAME : GSPC_Socket.h
//  DESC : Socket �ۼ��Ž����带 �����.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  ��         ��         ��         ��                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.10.17  ������  ���� ���α׶� �ۼ�                                     
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

	SOCKET m_gSocket;//udp �� ��� �ڷẸ���� ����
	SOCKET m_actSocket; //udp�� ��� �ڷ� �޴� ����, tcp�ΰ�� accept connection
	sockaddr_in m_sockInfo, m_sockInfoRecv;
	HANDLE m_hRSThread[2];
	BOOL   m_bIsClose;
	BOOL   m_bIsConnection;	//connection ����, �ð�:True �ƴϸ�: False

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
