//------------------------------------------------------------------------------
//  NAME : GSNetPacket.cpp
//  DESC : 무선층통신파케트와 기능을 정의한다.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  변         경         사         항                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.11.18  KM  최초 프로그람 작성                                     
//----------------------------------------------------------------------------

#include "stdafx.h"

#include "GSNetPacket.h"
#include "GSUtilityCls.h"

HResTimer thePacketTimer;

//P3,P4  Thread
void CGSNetPacket::SendThread(LPVOID lpParam)
{
	CGSNetPacket *pdlg = (CGSNetPacket *)lpParam;
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	BYTE* lpSendTempData = new BYTE[GS_NET_SEND_BUFFER_SIZE];
	DWORD dwDataType = GS_NET_MakeDataType(P::DEST::Remote,P::DEST::PC, 0, 0);

	utilObj->m_nThreadAliveCount ++;
	P::P2* stPacket2;
	int nCount;

	while (TRUE)
	{
		if (IsBadReadPtr(pdlg, 4))
			break;

		if (IsBadReadPtr(utilObj, 4))
			break;

		if (utilObj->m_isClose)
			break;

		if (GS_NET_Device_isConnected(GS_DEV_PC_TCP_SERVER))
		{
			//지령 파케트
			nCount = utilObj->m_listCmdPacket.Count();
			if (nCount > 0)
			{
				utilObj->m_listCmdPacket.Lock();
				
				stPacket2 = (P::P2*)utilObj->m_listCmdPacket.Get(0);
				if (stPacket2 != NULL)
				{
					switch (stPacket2->bStatus)//파케트의 상태 0:초기상태, 1: Send한 상태, 2: 응답된 상태
					{
					case 0:
						stPacket2->bStatus = 1;
						stPacket2->dwStamp = thePacketTimer.GetSystemTime();
						stPacket2->mktime = thePacketTimer.GetSystemTime();
						stPacket2->bSentCount++;
						
						CGSNetPacket::SendP2RegularData(stPacket2, lpSendTempData);

						if (GS_NET_SendData(GS_DEV_PC_TCP_SERVER, gbAppID , lpSendTempData, P::P2::bHeader, stPacket2->wSize + P::P2::HeaderSize, dwDataType) != 1)
						{
							utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("Error [%4d] : connect failed"), GS_NET_GetLastErrorCode(GS_DEV_PC_TCP_SERVER));
						} else
							utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("지령 파케트전송 pkt_id=%d"), stPacket2->bPID);

						//Sleep(150);
						break;
					case 1:
						//if (stPacket2->bSentCount > CMD_RESEND_COUNT)//목록에서 삭제
						if ((thePacketTimer.GetSystemTime() - stPacket2->mktime) > CMD_RESEND_TIME)
						{
							utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("지령 파케트 응답없음. pkt_id=%d"), stPacket2->bPID);
							utilObj->m_listCmdPacket.UnLock();
							utilObj->m_listCmdPacket.Remove(0);
							utilObj->m_listCmdPacket.Lock();
							
							break;
						}

						/*if ((thePacketTimer.GetSystemTime()() - stPacket2->mktime) > CMD_RESEND_TIME)
						stPacket2->bStatus = 0;*/

						break;
					case 2:
						utilObj->m_listCmdPacket.UnLock();
						utilObj->m_listCmdPacket.Remove(0);
						utilObj->m_listCmdPacket.Lock();
						break;
					}
				}
				
				utilObj->m_listCmdPacket.UnLock();
			} 
		}
		
		Sleep(1);
	}

	utilObj->m_nThreadAliveCount --;
	delete lpSendTempData;
}


CGSNetPacket::CGSNetPacket(void)
{
	Init();
}

void CGSNetPacket::Init(void)
{
	m_recvStP2 = NULL;

	HANDLE m_hSendThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)SendThread, (LPVOID)this, 0, NULL);
	if ( m_hSendThread == NULL /*|| m_hRecvThread == NULL*/)
	{
		AfxMessageBox( _T("Can not create the thread. Restart System.") );
		return;
	}

	CloseHandle( m_hSendThread );
}

CGSNetPacket::~CGSNetPacket(void)
{
	if (m_recvStP2)
	{
		delete m_recvStP2->lpData;
		delete m_recvStP2;
		m_recvStP2 = NULL;
	}
}

void CGSNetPacket::SendP2RegularData(P::P2* lpStData, BYTE* lpResult)
{
	BYTE* lpBuff = &lpResult[0];

	memset(lpBuff, P::P2::bHeader, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bDest, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bSrc, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bPID, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->dwStamp, 4);
	lpBuff += 4;
	memcpy(lpBuff, &lpStData->wSize, 2);
	lpBuff += 2;
	memcpy(lpBuff, lpStData->lpData, lpStData->wSize);
}
BOOL CGSNetPacket::RecvP2RegularData(BYTE* lpInData, P::P2* lpStData)
{
	BYTE* lpBuff = &lpInData[0];

	if (lpBuff[0] != P::P2::bHeader)
		return FALSE;

	lpBuff += 1;
	memcpy(&lpStData->bDest, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bSrc, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bPID, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->dwStamp, lpBuff, 4);
	lpBuff += 4;
	memcpy(&lpStData->wSize, lpBuff, 2);
	lpBuff += 2;

	lpStData->lpData = new BYTE[lpStData->wSize];
	memcpy(lpStData->lpData, lpBuff, lpStData->wSize);

	return TRUE;
}

void CGSNetPacket::SendP3RegularData(P::P3* lpStData, BYTE* lpResult)
{
	BYTE* lpBuff = &lpResult[0];

	memset(lpBuff, P::P3::bHeader, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bKind, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bDest, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bSrc, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bP2ID, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->dwStamp, 4);
	lpBuff += 4;
	memcpy(lpBuff, &lpStData->wData, 2);

	if (lpStData->bKind == P::Sub_PKind::P3_Data)//3형 자료파케트인경우
	{
		lpBuff += 2;
		memcpy(lpBuff, &lpStData->lpData, lpStData->wData);
	}
}
BOOL CGSNetPacket::RecvP3RegularData(BYTE* lpInData, P::P3* lpStData)
{
	BYTE* lpBuff = &lpInData[0];

	if (lpBuff[0] != P::P3::bHeader)
		return FALSE;

	lpBuff += 1;
	memcpy(&lpStData->bKind, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bDest, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bSrc, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bP2ID, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->dwStamp, lpBuff, 4);
	lpBuff += 4;
	memcpy(&lpStData->wData, lpBuff, 2);

	if (lpStData->bKind == P::Sub_PKind::P3_Data)//3형 자료파케트인경우
	{
		lpBuff += 2;
		memcpy(&lpStData->lpData, lpBuff, lpStData->wData);
	}

	return TRUE;
}

void CGSNetPacket::SendP4RegularData(P::P4* lpStData, BYTE* lpResult)
{
	BYTE* lpBuff = &lpResult[0];

	memset(lpBuff, P::P4::bHeader, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bKind, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bSize, 1);
	lpBuff += 1;
	memcpy(lpBuff, &lpStData->bNo, 1);
	lpBuff += 1;
	memcpy(lpBuff, lpStData->lpData, lpStData->bSize);
}
BOOL CGSNetPacket::RecvP4RegularData(BYTE* lpInData, P::P4* lpStData)
{
	BYTE* lpBuff = &lpInData[0];

	if (lpBuff[0] != P::P4::bHeader)
		return FALSE;

	lpBuff += 1;
	memcpy(&lpStData->bKind, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bSize, lpBuff, 1);
	lpBuff += 1;
	memcpy(&lpStData->bNo, lpBuff, 1);
	lpBuff += 1;
	memcpy(lpStData->lpData, lpBuff, lpStData->bSize);

	return TRUE;
}

BOOL CGSNetPacket::PicP3P4RecvData(BYTE* lpData, int nLen)
{
	BYTE* lpRecvData = lpData;
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	/*if (utilObj->check_crc(lpRecvData, nLen) == 0)
			return FALSE;*/

	if (lpRecvData[0] != P::P3::bHeader && lpRecvData[0] != P::P4::bHeader)
	{
		utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"파케트 머리부가 0x55, 0xAA가 아닌 불량 파케트가 들어왔습니다.");
		return FALSE;
	}

	int n;

	P::P3 stP3;
	P::P4 stP4;

	switch (lpRecvData[1])//3,4형
	{
	case P::Sub_PKind::Data_Begin://3형파케트
		if (!RecvP3RegularData(lpRecvData, &stP3))
			return FALSE;

		if (m_recvStP2 != NULL)
		{
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"P4 Count error");
			procP2(m_recvStP2);//림시

			delete m_recvStP2->lpData;
			delete m_recvStP2;
			m_recvStP2 = NULL;
		}

		m_recvStP2 = new P::P2;
		m_recvStP2->bDest = stP3.bDest;
		m_recvStP2->bSrc = stP3.bSrc;
		m_recvStP2->bPID = stP3.bP2ID;

		m_recvStP2->wSize = 0;
		m_recvStP2->wCMemSize = stP3.wData * GS_PIC_BUFFER_SIZE;
		ASSERT(stP3.wData * GS_PIC_BUFFER_SIZE <= 0xFFFF);

		m_recvStP2->lpData = new BYTE[m_recvStP2->wCMemSize];

		m_recvStP2->bCount = 0;
		m_recvStP2->wPCount = stP3.wData;

		break;
	case P::Sub_PKind::P3_Data://3형 자료인 경우
		if (!RecvP3RegularData(lpRecvData, &stP3))
			return FALSE;

		m_recvStP2ForP3Data = new P::P2;
		m_recvStP2ForP3Data->bDest = stP3.bDest;
		m_recvStP2ForP3Data->bSrc = stP3.bSrc;
		m_recvStP2ForP3Data->bPID = stP3.bP2ID;

		m_recvStP2ForP3Data->wSize = stP3.wData;
		m_recvStP2ForP3Data->lpData = new BYTE[stP3.wData];
		memcpy(m_recvStP2ForP3Data->lpData, stP3.lpData, stP3.wData);

		procP2(m_recvStP2ForP3Data);

		delete m_recvStP2ForP3Data->lpData;
		delete m_recvStP2ForP3Data;
		m_recvStP2ForP3Data = NULL;

		break;
	case P::Sub_PKind::Ack_Response://응답신호
		if (!CGSNetPacket::RecvP3RegularData(lpRecvData, &stP3))
			return FALSE;

		n = utilObj->m_listCmdPacket.Count();
		if (n > 0)
		{
			P::P2* lpStP2 = (P::P2*)utilObj->m_listCmdPacket.Get(0);
			if (lpStP2->bStatus == 1 && lpStP2->bPID == stP3.bP2ID)
			{
				utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("지령 응답 pkt_id=%d time=%d"), lpStP2->bPID, thePacketTimer.GetSystemTime() - lpStP2->dwStamp);		
				lpStP2->bStatus = 2;
				procResponsePkt(lpStP2->bPID,  P::Sub_PKind::Ack_Response);
			} else
				utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("불량 지령 응답 pkt_id=%d time=%d"), lpStP2->bPID, thePacketTimer.GetSystemTime() - lpStP2->dwStamp);
		} else
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("불량 지령 응답 pkt_id = no "));
		break;
	case P::Sub_PKind::NAck_Response://Nack 응답신호
		if (!CGSNetPacket::RecvP3RegularData(lpRecvData, &stP3))
			return FALSE;

		n = utilObj->m_listCmdPacket.Count();
		if (n > 0)
		{
			P::P2* lpStP2 = (P::P2*)utilObj->m_listCmdPacket.Get(0);
			if (lpStP2->bStatus == 1 && lpStP2->bPID == stP3.bP2ID)
			{
				utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("지령 Nack 응답 pkt_id=%d time=%d"), lpStP2->bPID, thePacketTimer.GetSystemTime() - lpStP2->dwStamp);
				lpStP2->bStatus = 2;
				procResponsePkt(lpStP2->bPID,  P::Sub_PKind::NAck_Response);
			} else
				utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("불량 지령 응답 pkt_id=%d time=%d"), lpStP2->bPID, thePacketTimer.GetSystemTime() - lpStP2->dwStamp);
		} else
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, _T("불량  지령 Nack 응답 pkt_id = no "));
		break;
	case P::Sub_PKind::P2_SubP://4형파케트
		if (m_recvStP2 == NULL)
			return FALSE;

		if (!RecvP4RegularData(lpRecvData, &stP4))//머리부가 0x55(4형)가 아닌경우 탈퇴.
		{
			delete m_recvStP2->lpData;
			delete m_recvStP2;
			m_recvStP2 = NULL;
			return FALSE;
		}

		if (stP4.bKind != P::Sub_PKind::P2_SubP) //2형분파케트가 아닌경우 탈퇴.
		{
			delete m_recvStP2->lpData;
			delete m_recvStP2;
			m_recvStP2 = NULL;
			return FALSE;
		}

		if (stP4.bNo != m_recvStP2->bCount)//련번호가 틀리는경우 탈퇴.
		{
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"packet num error %d,%d", stP4.bNo, m_recvStP2->bCount);

			int nDiff = m_recvStP2->bCount - stP4.bNo;//림시

			if (nDiff <10 && nDiff > -10)
			{
				m_recvStP2->bCount = stP4.bNo;
				m_recvStP2->wPCount -= abs(nDiff);
			} else
			{
				delete m_recvStP2->lpData;
				delete m_recvStP2;
				m_recvStP2 = NULL;
				return FALSE;
			}
		}

		if ((m_recvStP2->wSize + stP4.bSize) > m_recvStP2->wCMemSize)
		{
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"2type packet memory over full");
			return FALSE;
		} else
		{
			memcpy(&m_recvStP2->lpData[m_recvStP2->wSize], stP4.lpData, stP4.bSize);
			m_recvStP2->wSize += stP4.bSize;
		}

		m_recvStP2->bCount++;
		if (m_recvStP2->bCount > 0xFF)
			m_recvStP2->bCount = 0;

		m_recvStP2->wPCount--;

		if (m_recvStP2->wPCount == 0) 
		{
			procP2(m_recvStP2);

			delete m_recvStP2->lpData;
			delete m_recvStP2;
			m_recvStP2 = NULL;
		}
		break;
	default:
		return FALSE;
	}

	return TRUE;
}

void CGSNetPacket::procP2(P::P2* lpStP2)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (GS_Get_Bits(lpStP2->bDest, 3, 3) != P::DEST::PC)
	{
		utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"목적지가 다른 2형파케트가 존재합니다.");
		return;
	}

	if (GS_Get_Bits(lpStP2->bSrc, 7, 1) == P::SRC::Cmd)//지령자료인경우 응답파케트를 날리고 다음처리로 넘어간다.
	{
		//4형파케트 접수완료 3형으로 응답신호 보내기로직추가.
		P::P3 stP3;
		stP3.bKind = P::Sub_PKind::Ack_Response;
		stP3.bDest = lpStP2->bSrc;//목적지, 원천지를 바꾼다.
		stP3.bSrc = lpStP2->bDest;
		stP3.bP2ID = lpStP2->bPID;
		stP3.wData = 0x0;
		
		//utilObj->lpGSproDlg->m_sCmdRecv = (char*)lpStP2->lpData;
		//PicP3SendData(&stP3); //응답신호전송
	} else //상태
	{

	}
	
	BYTE* inData = lpStP2->lpData;

	switch (lpStP2->bPID)
	{
	case GS_PKT_GCS_INIT:
	case GS_PKT_GCS_CHN_DI_SET:// GCS_CHN_DO_SET: GCS_CHN_VI_SET:
	case GS_PKT_UAV_INIT:
	case GS_PKT_UAV_CHN_DO_SET:// UAV_CHN_DI_SET: UAV_CHN_VO_SET:
		if (GS_Get_Bits(lpStP2->bDest, 3, 3) == P::SRC::Pic)
		{
			if (GS_Get_Bits(lpStP2->bDest, 6, 3) == P::SRC::SubSRC::PIC1)
			{
			} else if (GS_Get_Bits(lpStP2->bDest, 6, 3) == P::SRC::SubSRC::PIC2)
			{
			} else if (GS_Get_Bits(lpStP2->bDest, 6, 3) == P::SRC::SubSRC::PIC3)
			{
			}
		}
	case GS_PKT_GCS_STATUS:
		utilObj->m_stPktGCSStatus = *((gs_pkt_GCS_Status_Type*)inData);

		utilObj->AddLogText(GS_NET_LOG_GCS_INFO, L"GCS INfo- %d, %d, %d, %d, %d, %d, %d, %d, %d", 
			utilObj->m_stPktGCSStatus.dwDataRecvFreq, //915, 0, 0, 915, 0, 0, 433, 180, 240640
			utilObj->m_stPktGCSStatus.dwDataRecvSenitive,
			utilObj->m_stPktGCSStatus.dwDataRecvSpeed,
			utilObj->m_stPktGCSStatus.dwDataSendFreq,
			utilObj->m_stPktUAVStatus.dwDataSendSenitive,
			utilObj->m_stPktGCSStatus.dwDataSendSpeed,
			utilObj->m_stPktGCSStatus.dwVideoRecvFreq,
			utilObj->m_stPktGCSStatus.dwVideoRecvSenitive,
			utilObj->m_stPktGCSStatus.dwVideoRecvSpeed
			);
		break;
	case GS_PKT_UAV_GET_STATUS:
		break;
	case GS_PKT_UAV_STATUS:
		utilObj->m_stPktUAVStatus = *((gs_pkt_UAV_Status_Type*)inData);
		utilObj->AddLogText(GS_NET_LOG_UAV_INFO, L"UAV Info- %d, %d, %d, %d, %d, %d, %d, %d, %d", 
			utilObj->m_stPktUAVStatus.dwDataRecvFreq,
			utilObj->m_stPktUAVStatus.dwDataRecvSenitive,
			utilObj->m_stPktUAVStatus.dwDataRecvSpeed,
			utilObj->m_stPktUAVStatus.dwDataSendFreq,
			utilObj->m_stPktUAVStatus.dwDataSendSenitive,
			utilObj->m_stPktUAVStatus.dwDataSendSpeed,
			utilObj->m_stPktUAVStatus.dwVideoSendFreq,
			utilObj->m_stPktUAVStatus.dwVideoSendOutput,
			utilObj->m_stPktUAVStatus.dwVideoSendSpeed
			);
		break;
	case GS_PKT_GCS_RFIC1_SET: // GCS_RFIC2_SET: GCS_RFIC3_SET:
	case GS_PKT_GCS_RFIC1_GET: // GCS_RFIC2_GET: GCS_RFIC3_GET:
	case GS_PKT_GCS_RFIC1_PROP: // GCS_RFIC2_PROP: GCS_RFIC3_PROP:
	case GS_PKT_UAV_RFIC1_SET: // UAV_RFIC2_SET: UAV_RFIC3_SET
	case GS_PKT_UAV_RFIC1_GET: // UAV_RFIC2_GET: UAV_RFIC3_GET
	case GS_PKT_UAV_RFIC1_PROP: // UAV_RFIC2_PROP: UAV_RFIC3_PROP:
	case GS_PKT_CAM_INIT:
	case GS_PKT_CAM_SET:
	case GS_PKT_CAM_FRAME_DATA:
		if (GS_Get_Bits(lpStP2->bSrc, 3, 3) == P::SRC::Camera)
		{
			utilObj->VideoBufferProc(lpStP2->wSize, inData, lpStP2->dwStamp);
		}
		break;
	case GS_PKT_CAM_DATA_READ:
		{
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"Camera Read Packet . size=%d", lpStP2->wSize);
			if (utilObj->lpDataCallBackFunc)
				utilObj->lpDataCallBackFunc(GS_PKT_CAM_DATA_READ, lpStP2->wSize, lpStP2->lpData);
		}
		
		break;
	case GS_PKT_CAM_DATA_WRITE:
		{
			utilObj->AddLogText(GS_NET_LOG_PACKET_STATUS, L"Camera Write Packet . size=%d", lpStP2->wSize);
			int xx= 0;
			xx++;
		}
		break;
	case GS_PKT_SD_FORMAT:
	case GS_PKT_SD_GET_INFO:
	case GS_PKT_SD_INFO:
	case GS_PKT_FLIGHT_PLAN:
	case GS_PKT_FLIGHT_CONTROL_CMD:
	case GS_PKT_FLIGHT_STATUS:
		break;
	case GS_PKT_TEST_DATA:
		
		break;
	default:
		break;
	}
}

void CGSNetPacket::procResponsePkt(int nPktId, int nResponse)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	switch (nPktId)
	{
	case GS_PKT_GCS_INIT:
		break;
	case GS_PKT_GCS_CHN_DI_SET:// GCS_CHN_DO_SET: GCS_CHN_VI_SET:
		
		break;
	case GS_PKT_UAV_INIT:
	case GS_PKT_UAV_CHN_DO_SET:// UAV_CHN_DI_SET: UAV_CHN_VO_SET:
	case GS_PKT_GCS_STATUS:
	case GS_PKT_UAV_GET_STATUS:
	case GS_PKT_UAV_STATUS:
	case GS_PKT_GCS_RFIC1_SET: // GCS_RFIC2_SET: GCS_RFIC3_SET:
	case GS_PKT_GCS_RFIC1_GET: // GCS_RFIC2_GET: GCS_RFIC3_GET:
	case GS_PKT_GCS_RFIC1_PROP: // GCS_RFIC2_PROP: GCS_RFIC3_PROP:
	case GS_PKT_UAV_RFIC1_SET: // UAV_RFIC2_SET: UAV_RFIC3_SET
	case GS_PKT_UAV_RFIC1_GET: // UAV_RFIC2_GET: UAV_RFIC3_GET
	case GS_PKT_UAV_RFIC1_PROP: // UAV_RFIC2_PROP: UAV_RFIC3_PROP:
	case GS_PKT_CAM_INIT:
	case GS_PKT_CAM_SET: //영상 스트림 시작, 영상록화 시작
	case GS_PKT_CAM_FRAME_DATA:
	case GS_PKT_SD_FORMAT:
	case GS_PKT_SD_GET_INFO:
	case GS_PKT_SD_INFO:
	case GS_PKT_FLIGHT_PLAN:
	case GS_PKT_FLIGHT_CONTROL_CMD:
	case GS_PKT_FLIGHT_STATUS:
	case GS_PKT_TEST_DATA:
	default:
		break;
	}
}

/************************************************************************/
/* inData: New로 창조된 변수이며 내부에서 해제된다.                       */
/************************************************************************/
void CGSNetPacket::AddPacket(BYTE bPid, WORD wSize, BYTE* inDataR, BOOL bLocal)
{
	BYTE* inData = NULL;
	if (inDataR)
	{
		inData = new BYTE[wSize];
		memcpy(inData, inDataR, wSize);
	}

	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	DWORD dwTmp = 0;

	P::P2* stPacket2 = new P::P2;
	memset(stPacket2, 0x0, sizeof(P::P2));

	stPacket2->bPID = bPid;
	stPacket2->wSize = wSize;
	stPacket2->bSrc  = GS_NET_MakeDestOrSrc(P::SRC::Cmd, P::SRC::Local, P::SRC::PC, P::SRC::SubSrcDefaultVal);

	switch (bPid)
	{
	case GS_PKT_GCS_INIT:
		//if (utilObj->m_isInitPktSendStatus)
		//	return;

		utilObj->m_isInitPktSendStatus = TRUE;
		if (bLocal)
			stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Local, P::DEST::Camera, P::DEST::SubDestDefaultVal);
		else
			stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Remote, P::DEST::Camera, P::DEST::SubDestDefaultVal);

		if (utilObj->m_SystemInfo.nWirelessEncodeEnable == 0)
		{
			stPacket2->wSize = 0;
			stPacket2->lpData = NULL;
			break;
		}
		if (inData == NULL)
		{
			stPacket2->lpData = new BYTE[stPacket2->wSize];
			// data set(encoding key)
			for (int i=0; i<wSize; i++)
				stPacket2->lpData[i] = rand() % 0xFF + 1;
		}
		break;
		
	case GS_PKT_GCS_CHN_DI_SET:// GCS_CHN_DO_SET: GCS_CHN_VI_SET:
		stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Local, P::DEST::Arm, P::DEST::SubDestDefaultVal);
		
		stPacket2->lpData = inData;
		break;
	case GS_PKT_UAV_INIT:
	case GS_PKT_UAV_CHN_DO_SET:// UAV_CHN_DI_SET: UAV_CHN_VO_SET:
	case GS_PKT_GCS_STATUS:
	case GS_PKT_UAV_GET_STATUS:
	case GS_PKT_UAV_STATUS:
	case GS_PKT_GCS_RFIC1_SET: // GCS_RFIC2_SET: GCS_RFIC3_SET:
	case GS_PKT_GCS_RFIC1_GET: // GCS_RFIC2_GET: GCS_RFIC3_GET:
	case GS_PKT_GCS_RFIC1_PROP: // GCS_RFIC2_PROP: GCS_RFIC3_PROP:
	case GS_PKT_UAV_RFIC1_SET: // UAV_RFIC2_SET: UAV_RFIC3_SET
	case GS_PKT_UAV_RFIC1_GET: // UAV_RFIC2_GET: UAV_RFIC3_GET
	case GS_PKT_UAV_RFIC1_PROP: // UAV_RFIC2_PROP: UAV_RFIC3_PROP:
	case GS_PKT_CAM_INIT:
	case GS_PKT_CAM_SET: //영상 스트림 시작, 영상록화 시작
		stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Remote, P::DEST::Arm, P::DEST::SubDestDefaultVal);
		
		stPacket2->lpData = new BYTE[stPacket2->wSize];
		//data set
		dwTmp = 1 | 2; //방식 전송 & 록화
		memcpy(&stPacket2->lpData[0], &dwTmp, 4);
		dwTmp = 1024 * 2; //Bitrate
		memcpy(&stPacket2->lpData[4], &dwTmp, 4);
		dwTmp = 5; // Framerate
		memcpy(&stPacket2->lpData[8], &dwTmp, 4);
		dwTmp = 704 * 576;//resolution
		memcpy(&stPacket2->lpData[12], &dwTmp, 4);
		dwTmp = 10; //GOV length
		memcpy(&stPacket2->lpData[16], &dwTmp, 4);
		break;
	case GS_PKT_CAM_FRAME_DATA:
		break;
	case GS_PKT_CAM_DATA_READ:
		if (bLocal)
			stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Local, P::DEST::Camera, P::DEST::SubDestDefaultVal);
		else
			stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Remote, P::DEST::Camera, P::DEST::SubDestDefaultVal);

		stPacket2->lpData = inData;
		break;
	case GS_PKT_CAM_DATA_WRITE:
		if (bLocal)
			stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Local, P::DEST::Camera, P::DEST::SubDestDefaultVal);
		else
			stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Remote, P::DEST::Camera, P::DEST::SubDestDefaultVal);

		stPacket2->lpData = inData;
		break;
	case GS_PKT_SD_FORMAT:
	case GS_PKT_SD_GET_INFO:
	case GS_PKT_SD_INFO:
	case GS_PKT_FLIGHT_PLAN:
	case GS_PKT_FLIGHT_CONTROL_CMD:
	case GS_PKT_FLIGHT_STATUS:
	case GS_PKT_TEST_DATA:
		stPacket2->bDest = GS_NET_MakeDestOrSrc(P::DEST::P2Type, P::DEST::Remote, P::DEST::Arm, P::DEST::SubDestDefaultVal);

		stPacket2->lpData = inData;
		break;
	default:
		break;
	}

	utilObj->m_listCmdPacket.Add((void*)stPacket2);
}