//------------------------------------------------------------------------------
//  NAME : GSNetPacket.h
//  DESC : �������������Ʈ�� ����� �����Ѵ�.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  ��         ��         ��         ��                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.11.18  KM  ���� ���α׶� �ۼ�                                     
//----------------------------------------------------------------------------
#if !defined(GSNetPacket_h)
#define GSNetPacket_h

#pragma once

#define GS_PIC_BUFFER_SIZE 1024
#define CMD_RESEND_TIME 5000
#define CMD_RESEND_COUNT 3

namespace P //����Ʈ����
{
	enum Paket
	{
		pak1 = 0,   //1������Ʈ
		pak2,		//2������Ʈ
		pak3,		//3������Ʈ
		pak4		//4������Ʈ
	};

	enum Report_Value
	{
		Send_Finish = 0,		//�۽ſϷ�
		Send_Error,				//�۽ſ���
		Recv_Finish,			//���ſϷ�
		Recv_Error				//���ſ���
	};

	//1�� ����Ʈ �������� ����.
	struct P1 
	{
		static const BYTE bHeader = 0xAA;	//�Ӹ��� 1 Byte
		BYTE bDest;				//������ 1 Byte
		BYTE bSrc;				//��õ�� 1 Byte
		BYTE bSize;				//���� 1 Byte
		BYTE* lpData;			//�ڷ� (0~255)Byte

		static const BYTE HeaderSize = 0x4;//�Ӹ��� ũ��
	};

	//2�� ����Ʈ
#pragma pack(1)
	struct P2 
	{
		static const BYTE bHeader = 0xAA;	//�Ӹ��� 1 Byte
		BYTE bDest;							//������ 1 Byte
		BYTE bSrc;							//��õ�� 1 Byte
		BYTE bPID;							//Packet ID
		DWORD dwStamp;						//4Byte
		WORD wSize;							//���� 2 Byte

		BYTE* lpData;						//�ڷ� (0~65535)Byte

		//temp
		static const BYTE HeaderSize = 0xA;//�Ӹ��� ũ��
		BYTE  bCount;			//recv�ÿ� ����Ʈ�ù�ȣ�� ����ϱ����� ����
		WORD  wPCount;			//3������Ʈ���� �Ѿ�� ����Ʈ����.
		INT64 mktime;			//send �ð�
		BYTE  bStatus;			//����Ʈ�� ���� 0:�ʱ����, 1: Send�� ����, 2: recv�� ����
		BYTE  bSentCount;		//���� ȸ��
		WORD  wCMemSize;		//â���� ��� ũ��
	};

	//3�� �� ����Ʈ
	struct P3 
	{
		static const BYTE bHeader = 0x55;	//�Ӹ��� 1 Byte
		BYTE bKind;							//������Ʈ���� 1 Byte
		BYTE bDest;							//������ 1 Byte
		BYTE bSrc;							//��õ�� 1 Byte
		BYTE bP2ID;							//2�� ����Ʈ ID
		DWORD dwStamp;					    //4Byte
		WORD wData;							//3��������Ʈ ���� Ȥ�� ����

		BYTE lpData[GS_PIC_BUFFER_SIZE];	//�ڷ� (0~255)Byte

		static const BYTE HeaderSize = 0xB;//�Ӹ��� ũ��
	};

	//4�� �� ����Ʈ
	struct P4 
	{
		static const BYTE bHeader = 0x55;	//�Ӹ��� 1 Byte
		BYTE bKind;							//������Ʈ���� 1 Byte
		BYTE bSize;							//���� 1 Byte
		BYTE bNo;							//1B:�ù�ȣ

		BYTE lpData[GS_PIC_BUFFER_SIZE];  //�ڷ� (0~255)Byte 

		//temp
		static const BYTE HeaderSize = 0x4;//�Ӹ��� ũ��
	};

#pragma pack()
	namespace DEST //������
	{
		//7bit 
		//const BYTE P1Type		= 0x0; //1 ������Ʈ ������� ����
		const BYTE P2Type		= 0x1; //2������Ʈ
		//6bit 
		const BYTE Remote		= 0x1; //������
		const BYTE Local		= 0x0; //local
		//5,4,3bit
		const BYTE Reserve0		= 0x0; //����
		const BYTE PC			= 0x01; //PC
		const BYTE Camera		= 0x02; //Camera
		const BYTE SDcard		= 0x03; //SD card
		const BYTE Arm			= 0x04; //ARM
		const BYTE Pic			= 0x05; //PIC
		const BYTE Wireless		= 0x06; //Wireless element
		const BYTE Reserve7		= 0x07; //����
		//2,1,0 //�κи�����
		const BYTE SubDestDefaultVal	= 0x0; 
		namespace SubDest
		{
			const BYTE PIC1		= 0x1; 
			const BYTE PIC2		= 0x2; 
			const BYTE PIC3		= 0x3; 
		}
	}

	namespace SRC //��õ��
	{
		//7bit 
		const BYTE Cmd			= 0x0; //�����ڷ�
		const BYTE Status		= 0x1; //�����ڷ�
		//6bit 
		const BYTE Remote		= 0x1; //������
		const BYTE Local		= 0x0; //local
		//5,4,3bit
		const BYTE Reserve0		= 0x0; //����
		const BYTE PC			= 0x01; //PC
		const BYTE Camera		= 0x02; //Camera
		const BYTE SDcard		= 0x03; //SD card
		const BYTE Arm			= 0x04; //ARM
		const BYTE Pic			= 0x05; //PIC
		const BYTE Wireless		= 0x06; //Wireless element
		const BYTE Reserve7		= 0x07; //����
		//2,1,0 //�κп�õ��
		const BYTE SubSrcDefaultVal	= 0x0; 
		namespace SubSRC
		{
			const BYTE PIC1		= 0x1; 
			const BYTE PIC2		= 0x2; 
			const BYTE PIC3		= 0x3; 
		}
	};

	namespace Sub_PKind //������Ʈ����
	{
		//3������Ʈ����
		const BYTE Data_Begin		= 0x80;	//�ڷ���ۺ�����Ʈ
		const BYTE Ack_Response		= 0x81;	//Ack ����  �ش� ����Ʈ��ȣ�� �ڷḶ�翡 ����.
		const BYTE NAck_Response	= 0x82;	//NAck ����  �ش� ����Ʈ��ȣ�� �ڷḶ�翡 ����.
		const BYTE P3_Data			= 0x90;	//3�� �ڷ� ����Ʈ
		const BYTE Reserve2			= 0x0;	//0x91~0xFF ����

		//4������Ʈ ����
		const BYTE P1_SubP			= 0x00;	//32����Ʈ ������ ���� 1�� ����Ʈ�� ��ȯ�� ����Ʈ�� //����Ʈ��ȣ�� �����ʴ´�.
		const BYTE P2_SubP			= 0x01;	//2�� ����Ʈ�� ������ �ڷ� //����Ʈ��ȣ�� �ٴ´�. �������� �����ʴ´�.
		const BYTE Reserve1			= 0x0;	//0x02~0x7F ����
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