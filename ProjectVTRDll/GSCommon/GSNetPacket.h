//------------------------------------------------------------------------------
//  NAME : GSNetPacket.h
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
#if !defined(GSNetPacket_h)
#define GSNetPacket_h

#pragma once

#define GS_PIC_BUFFER_SIZE 1024
#define CMD_RESEND_TIME 5000
#define CMD_RESEND_COUNT 3

namespace P //파케트정의
{
	enum Paket
	{
		pak1 = 0,   //1형파케트
		pak2,		//2형파케트
		pak3,		//3형파케트
		pak4		//4형파케트
	};

	enum Report_Value
	{
		Send_Finish = 0,		//송신완료
		Send_Error,				//송신오유
		Recv_Finish,			//수신완료
		Recv_Error				//수신오유
	};

	//1형 파케트 리용하지 않음.
	struct P1 
	{
		static const BYTE bHeader = 0xAA;	//머리부 1 Byte
		BYTE bDest;				//목적지 1 Byte
		BYTE bSrc;				//원천지 1 Byte
		BYTE bSize;				//길이 1 Byte
		BYTE* lpData;			//자료 (0~255)Byte

		static const BYTE HeaderSize = 0x4;//머리부 크기
	};

	//2형 파케트
#pragma pack(1)
	struct P2 
	{
		static const BYTE bHeader = 0xAA;	//머리부 1 Byte
		BYTE bDest;							//목적지 1 Byte
		BYTE bSrc;							//원천지 1 Byte
		BYTE bPID;							//Packet ID
		DWORD dwStamp;						//4Byte
		WORD wSize;							//길이 2 Byte

		BYTE* lpData;						//자료 (0~65535)Byte

		//temp
		static const BYTE HeaderSize = 0xA;//머리부 크기
		BYTE  bCount;			//recv시에 파케트련번호를 계수하기위한 변수
		WORD  wPCount;			//3형파케트에서 넘어온 파케트개수.
		INT64 mktime;			//send 시간
		BYTE  bStatus;			//파케트의 상태 0:초기상태, 1: Send한 상태, 2: recv된 상태
		BYTE  bSentCount;		//보낸 회수
		WORD  wCMemSize;		//창조된 멤모리 크기
	};

	//3형 분 파케트
	struct P3 
	{
		static const BYTE bHeader = 0x55;	//머리부 1 Byte
		BYTE bKind;							//분파케트종류 1 Byte
		BYTE bDest;							//목적지 1 Byte
		BYTE bSrc;							//원천지 1 Byte
		BYTE bP2ID;							//2형 파케트 ID
		DWORD dwStamp;					    //4Byte
		WORD wData;							//3형분파케트 개수 혹은 길이

		BYTE lpData[GS_PIC_BUFFER_SIZE];	//자료 (0~255)Byte

		static const BYTE HeaderSize = 0xB;//머리부 크기
	};

	//4형 분 파케트
	struct P4 
	{
		static const BYTE bHeader = 0x55;	//머리부 1 Byte
		BYTE bKind;							//분파케트종류 1 Byte
		BYTE bSize;							//길이 1 Byte
		BYTE bNo;							//1B:련번호

		BYTE lpData[GS_PIC_BUFFER_SIZE];  //자료 (0~255)Byte 

		//temp
		static const BYTE HeaderSize = 0x4;//머리부 크기
	};

#pragma pack()
	namespace DEST //목적지
	{
		//7bit 
		//const BYTE P1Type		= 0x0; //1 형파케트 사용하지 않음
		const BYTE P2Type		= 0x1; //2형파케트
		//6bit 
		const BYTE Remote		= 0x1; //원격지
		const BYTE Local		= 0x0; //local
		//5,4,3bit
		const BYTE Reserve0		= 0x0; //예약
		const BYTE PC			= 0x01; //PC
		const BYTE Camera		= 0x02; //Camera
		const BYTE SDcard		= 0x03; //SD card
		const BYTE Arm			= 0x04; //ARM
		const BYTE Pic			= 0x05; //PIC
		const BYTE Wireless		= 0x06; //Wireless element
		const BYTE Reserve7		= 0x07; //예약
		//2,1,0 //부분목적지
		const BYTE SubDestDefaultVal	= 0x0; 
		namespace SubDest
		{
			const BYTE PIC1		= 0x1; 
			const BYTE PIC2		= 0x2; 
			const BYTE PIC3		= 0x3; 
		}
	}

	namespace SRC //원천지
	{
		//7bit 
		const BYTE Cmd			= 0x0; //지령자료
		const BYTE Status		= 0x1; //상태자료
		//6bit 
		const BYTE Remote		= 0x1; //원격지
		const BYTE Local		= 0x0; //local
		//5,4,3bit
		const BYTE Reserve0		= 0x0; //예약
		const BYTE PC			= 0x01; //PC
		const BYTE Camera		= 0x02; //Camera
		const BYTE SDcard		= 0x03; //SD card
		const BYTE Arm			= 0x04; //ARM
		const BYTE Pic			= 0x05; //PIC
		const BYTE Wireless		= 0x06; //Wireless element
		const BYTE Reserve7		= 0x07; //예약
		//2,1,0 //부분원천지
		const BYTE SubSrcDefaultVal	= 0x0; 
		namespace SubSRC
		{
			const BYTE PIC1		= 0x1; 
			const BYTE PIC2		= 0x2; 
			const BYTE PIC3		= 0x3; 
		}
	};

	namespace Sub_PKind //분파케트종류
	{
		//3형파케트종륜
		const BYTE Data_Begin		= 0x80;	//자료시작분파케트
		const BYTE Ack_Response		= 0x81;	//Ack 응답  해당 파케트번호가 자료마당에 들어간다.
		const BYTE NAck_Response	= 0x82;	//NAck 응답  해당 파케트번호가 자료마당에 들어간다.
		const BYTE P3_Data			= 0x90;	//3형 자료 파케트
		const BYTE Reserve2			= 0x0;	//0x91~0xFF 예약

		//4형파케트 종류
		const BYTE P1_SubP			= 0x00;	//32바이트 이하의 작은 1형 파케트를 변환한 파케트임 //파케트번호가 붙지않는다.
		const BYTE P2_SubP			= 0x01;	//2형 파케트를 분할한 자료 //파케트번호가 붙는다. 목적지가 붙지않는다.
		const BYTE Reserve1			= 0x0;	//0x02~0x7F 예약
	}
}

class CGSNetPacket
{
public:
	CGSNetPacket(void);
	~CGSNetPacket(void);

	void Init();
	void Close();
private:
	P::P2* m_recvStP2;
	P::P2* m_recvStP2ForP3Data;

protected:
	static void SendThread(LPVOID lpParam);

public:	
	static void SendP2RegularData(P::P2* lpStData, BYTE* lpResult);
	static BOOL RecvP2RegularData(BYTE* lpInData, P::P2* lpStData);

	static void SendP3RegularData(P::P3* lpStData, BYTE* lpResult);
	static BOOL RecvP3RegularData(BYTE* lpInData, P::P3* lpStData);

	static void SendP4RegularData(P::P4* lpStData, BYTE* lpResult);
	static BOOL RecvP4RegularData(BYTE* lpInData, P::P4* lpStData);

	BOOL PicP3P4RecvData(BYTE* lpData, int nLen);
	static void procP2(P::P2* lpStP2);
	void procResponsePkt(int nPktId, int nResponse);
	void AddPacket(BYTE bPid, WORD wSize, BYTE* inData, BOOL bLocal);
};

#endif;