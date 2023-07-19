//------------------------------------------------------------------------------
//  NAME : GSNetCommon.h 
//  DESC : 통신용 서고의 정의파일
// VER  : v1.0 
// PROJ : 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  변         경         사         항                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.10.20  로일선  최초 프로그람 작성                                     
//------------------------------------------------------------------------------
#include "stdafx.h"

#ifndef GSNETCOMMON_H
#define GSNETCOMMON_H

/////   constant

//#define  _GS_VSTAB_ENABLE
//////////////////////////////////////////////////////
#define GS_NET_H264_APPLY_CURRENT_TIME 1
#define GS_NET_VIDEO_BUFFER_SIZE 1024 * 1024
#define GS_NET_RECV_BUFFER_SIZE 500 * 1024
#define GS_NET_SEND_BUFFER_SIZE 16*1024 + GS_NET_RS_HEAD_DATASIZE

#define GS_NET_LOGTEXT_MAX_LENGTH 1024 * 2
//////////////////////////////////////////////////////
#define GS_PKT_GCS_INIT 1
#define GS_PKT_GCS_CHN_DI_SET 2
#define GS_PKT_GCS_CHN_DO_SET 2
#define GS_PKT_GCS_CHN_VI_SET 2
#define GS_PKT_UAV_INIT 3
#define GS_PKT_UAV_CHN_DO_SET 4
#define GS_PKT_UAV_CHN_DI_SET 4
#define GS_PKT_UAV_CHN_VO_SET 4
#define GS_PKT_GCS_STATUS 5
#define GS_PKT_UAV_GET_STATUS 6
#define GS_PKT_UAV_STATUS 7
#define GS_PKT_GCS_RFIC1_SET 30
#define GS_PKT_GCS_RFIC1_GET 31
#define GS_PKT_GCS_RFIC1_PROP 32
#define GS_PKT_GCS_RFIC2_SET 30
#define GS_PKT_GCS_RFIC2_GET 31
#define GS_PKT_GCS_RFIC2_PROP 32
#define GS_PKT_GCS_RFIC3_SET 30
#define GS_PKT_GCS_RFIC3_GET 31
#define GS_PKT_GCS_RFIC3_PROP 32
#define GS_PKT_UAV_RFIC1_SET 60
#define GS_PKT_UAV_RFIC1_GET 61
#define GS_PKT_UAV_RFIC1_PROP 62
#define GS_PKT_UAV_RFIC2_SET 60
#define GS_PKT_UAV_RFIC2_GET 61
#define GS_PKT_UAV_RFIC2_PROP 62
#define GS_PKT_UAV_RFIC3_SET 60
#define GS_PKT_UAV_RFIC3_GET 61
#define GS_PKT_UAV_RFIC3_PROP 62
#define GS_PKT_CAM_INIT 90
#define GS_PKT_CAM_SET 91
#define GS_PKT_CAM_FRAME_DATA 92
#define GS_PKT_CAM_DATA_READ 93
#define GS_PKT_CAM_DATA_WRITE 94
#define GS_PKT_SD_FORMAT 100
#define GS_PKT_SD_GET_INFO 101
#define GS_PKT_SD_INFO 102
#define GS_PKT_FLIGHT_PLAN 110
#define GS_PKT_FLIGHT_CONTROL_CMD 111
#define GS_PKT_FLIGHT_STATUS 112
#define GS_PKT_TEST_DATA 150

struct VIDEO_PARAM
{
	int		isAudioEnable;
	int		nBitRate;
	int		nBitRateControl;
	int		nCompression;
	int		nFps;
	int		nGop;
	int		nQuality;
	int		nResolution;
	int		isVideoEnable;
};

struct CAMERA_PARAM
{
	int		nExposureMode;
	DWORD	dwExposureLeastTime;
	DWORD	dwExposureMostTime;
	int		nDayNightMode;
	int		nBLC;
	int		nAutoIris;
	int		nProfile;
	int		nAEReference;
	int		nDncThreshold;
	int		nAESensitivity;
	int		nAGC;
	int		nAGCLimit;
	int		nSlowShutterMode;
	int		nIRCUT;
	int		nDayNTLevel;
	int		nNightNTLevel;
	int		isMirror;
	int		isFlip;
	int		isAntiFlicker;
	int		isIRSwap;
};

//////  com function

#define CALLBACK    __stdcall
#define GS_NET_API_FN_TYPE extern "C"  __declspec(dllexport)

typedef void (*GS_NET_CALLBACK_FUNC)(DWORD dwErrorCode);
typedef void (*GS_NET_VIDEO_CALLBACK_FUNC)(DWORD dwWidth, DWORD dwHeight, BYTE* lpVideoData, DWORD dwOffsetTime);
typedef void (*GS_NET_DATA_CALLBACK_FUNC)(BYTE bPid, DWORD dwSize, BYTE* lpData);

#ifdef GS_PROJECT_PC
void F_LogWrite(LPCTSTR pFormat, ...);
#endif

#define GS_NET_SYSTEM_STATUS_INIT 0x01
#define GS_NET_SYSTEM_STATUS_ACTIVE 0x02
#define GS_NET_SYSTEM_STATUS_RESTART 0x03
#define GS_NET_SYSTEM_STATUS_SHUTDOWN 0xFFFF

struct gs_pkt_GCS_Status_Type
{
	DWORD dwDataRecvFreq;
	DWORD dwDataRecvSpeed;
	DWORD dwDataRecvSenitive;
	DWORD dwDataSendFreq;
	DWORD dwDataSendSpeed;
	DWORD dwDataSendSenitive;
	DWORD dwVideoRecvFreq;
	DWORD dwVideoRecvSpeed;
	DWORD dwVideoRecvSenitive;
};

struct gs_pkt_UAV_Status_Type
{
	DWORD dwDataRecvFreq;
	DWORD dwDataRecvSpeed;
	DWORD dwDataRecvSenitive;
	DWORD dwDataSendFreq;
	DWORD dwDataSendSpeed;
	DWORD dwDataSendSenitive;
	DWORD dwVideoSendFreq;
	DWORD dwVideoSendSpeed;
	DWORD dwVideoSendOutput;
};

struct gs_logType
{
	WCHAR wzLogText[GS_NET_LOGTEXT_MAX_LENGTH];
	DWORD tickVal;
};

//=============================================================================
static inline BYTE GS_Get_Low_Bits_Mask(BYTE n)
{
  return (1 << n) - 1;
}

static inline BYTE GS_Get_Bits(BYTE x, BYTE first, BYTE len)
{
  return (x >> first) & GS_Get_Low_Bits_Mask(len);
}

static inline void GS_Set_Bits(BYTE* x, BYTE first, BYTE len, BYTE val)
{
  *x &= ~(GS_Get_Low_Bits_Mask(len) << first);
  *x |= (GS_Get_Low_Bits_Mask(len) & val) << first;
}

//n번째 Bit의 값을 얻는다.
#define GS_GetBitVal(nVal, nBit) (nVal & (1 << nBit)) >> nBit
#define GS_SetBitVal(nVal, nBit, d) (nVal &  (~(1 << nBit))  ) | (d << nBit)

//===============================================================
//GS_NET 내부처리자료형 함수
typedef struct gs_NET_CALLBACK
{
	DWORD	dwDeviceID;
	BYTE	bAppID;
	DWORD	dwCallType;
	DWORD	dwDataFilter;//dwDataType 와 매칭
	DWORD	dwPriority;
	GS_NET_CALLBACK_FUNC	lpFunc;

	struct gs_NET_CALLBACK* lpPrev; 
	struct gs_NET_CALLBACK* lpNext; 
} GS_NET_CALLBACK;

//송수신자료형정의
typedef struct gs_NET_DATA 
{
	DWORD	dwDeviceID;
	BYTE	bAppID;
	BYTE    dwHeader;
	DWORD	dwDataType;//1B:목적지 2B:원천지	3,4B:미정
	DWORD	dwSize;
	BYTE*	lpData;

	struct gs_NET_DATA* lpPrev; 
	struct gs_NET_DATA* lpNext; 
} GS_NET_DATA;

typedef struct
{
#ifdef GS_PROJECT_PC
	HANDLE	hLock;
#else
	sem_t hLock;
#endif
	long nListCount;
	GS_NET_DATA* lpFirst;
	GS_NET_DATA* lpLast;
} GS_NET_DATA_LIST;

//장치ID 정의
enum GS_DEVICE
{
#ifdef GS_PROJECT_PC
	GS_DEV_PC_TCP_SERVER = 0,
	GS_DEV_PC_TCP_CLIENT,
	GS_DEV_PC_IPCAM_TCP_CLIENT_CMD,
	GS_DEV_PC_IPCAM_TCP_CLIENT_DATA,
	GS_DEV_PC_UDP_SERVER,
	GS_DEV_PC_UDP_CLIENT,
#endif

#ifdef GS_PROJECT_ARM_LAND
	GS_DEV_ARM2RADIO_VIDEO = 0,		//ARM-RADIO: 영상자료용 장치ID
	GS_DEV_ARM2RADIO_STATUS,		//ARM-RADIO: 상태자료용 장치ID
	GS_DEV_ARM2RADIO_CMD,			//ARM-RADIO: 지령용 장치ID	    

	GS_DEV_ARM2PC_VIDEO,			//ARM-PC: 영상자료용 장치ID, UDP_SVR
	GS_DEV_ARM2PC_STATUS,			//ARM-PC: 상태자료용 장치ID, UDP_SVR
	GS_DEV_PC2ARM_COM,				//ARM-PC: 지령용 장치ID,	 TCP_CLI		    

	GS_DEV_ARM_TEST,				//검사용 장치ID		    
#endif

#ifdef GS_PROJECT_ARM_AUV
//	GS_DEV_ARM2RADIO_VIDEO = 0,		//ARM-RADIO: 영상자료용 장치ID
//	GS_DEV_ARM2RADIO_STATUS,		//ARM-RADIO: 상태자료용 장치ID
//	GS_DEV_ARM2RADIO_CMD,			//ARM-RADIO: 지령용 장치ID	    

//	GS_DEV_CAM2ARM_VIDEO,			//ARM-CAM: 영상자료용 장치ID
//	GS_DEV_CAM2ARM_STATUS,			//ARM-CAM: 상태자료용 장치ID
//	GS_DEV_ARM2CAM_CMD,				//ARM-CAM: 지령자료용 장치ID
//  GS_DEV_ARM2PC_VIDEO,			//ARM-PC: 영상자료용 장치ID, UDP_SVR
//  GS_DEV_ARM2PC_STATUS,			//ARM-PC: 상태자료용 장치ID, UDP_SVR

//  GS_DEV_PC2ARM_COM,			//ARM-PC: 지령용 장치ID,	 TCP_CLI

#endif
	GS_DEVICE_MAX 					//장치의 총개수
};

//List 의 최대개수
#define  GS_NET_LIST_COUNT				100

//data부의 머리부 크기
#define  GS_NET_RS_HEAD_ST_STRING_LEN	14
#define  GS_NET_RS_HEAD_ST_STRING		"Content-Start:"
#define  GS_NET_RS_HEAD_DATASIZE		9 + GS_NET_RS_HEAD_ST_STRING_LEN

#define  GS_NET_RS_RAW_DATATYPE			0x0000FFFF

//APP ID 정의
#define  GS_APP_PC_VIDEO	0x10
#define  GS_APP_PC_STATUS	0x20
#define  GS_APP_PC_COMMAND	0x30
#define  GS_APP_ARM_PC		0x40
#define  GS_APP_ARM_RADIO	0x50
#define  GS_APP_ARM_CAMERA	0x60

//GS_NET_RegCallback용 callback 함수 호출형정의
#define  GS_CALLBACK_SENT		0x01 //자료송신완료
#define  GS_CALLBACK_READYDATA	0x02 //상태측으로 부터 받은 수신자료가 있을때 
#define  GS_CALLBACK_ERROR		0x04 //자료송신시 오유가 발생했을때

//callback 함수의 우선권순위정의
#define  GS_CALL_PRIORITY_HIGH			0x00
#define  GS_CALL_PRIORITY_NORMAL		0x10
#define  GS_CALL_PRIORITY_LOW			0x20

//오유코드정의
#define  GS_NET_ERROR_NO				0x00 //오유없음
#define  GS_NET_ERROR_NOMEM				0x02 //기억기모자람
#define  GS_NET_ERROR_NODEV				0x03 //지원하지 않는 장치
#define  GS_NET_ERROR_INVALID_DATASIZE	0x04 //자료의 크기가 정확치 않음
#define  GS_NET_ERROR_NO_CONNECTION		0x05 //콘넥션 없음.
#define  GS_NET_ERROR_INVALID_SOCKET	0x10 //소켓장치열기에러
#define  GS_NET_ERROR_SOCKET_ERROR		0x11 //소켓  에러
#define  GS_NET_ERROR_RETURN			0x0ffffffff //함수가 실패한경우 돌려주는 값.
//callback  함수형 정의

//통보문코드정의
enum GS_LOG
{
	GS_NET_LOG_SYSTEM_INFO = 0,
	GS_NET_LOG_DEVICE,
	GS_NET_LOG_SOCKET,
	GS_NET_LOG_RECV_AMOUNT,
	GS_NET_LOG_H264_DECODE,
	GS_NET_LOG_H264_DECODE_VSTAB,
	GS_NET_LOG_GCS_INFO,
	GS_NET_LOG_UAV_INFO,
	GS_NET_LOG_FILEPLAY_STATUS,
	GS_NET_LOG_PACKET_STATUS,
	GS_NET_LOG_ERROR,
	GS_NET_LOG_APP_LAYER_STATUS,
	GS_NET_LOG_LOCK_STATUS,

	GS_NET_LOG_MAX_COUNT
};

//callback  함수형 정의
//오유코드
static DWORD gsdwNetDeviceErrorCode;
static DWORD gsdwNetErrorCode[GS_DEVICE_MAX];

//공통함수 초기화함수
GS_NET_API_FN_TYPE void __stdcall dotNet(int net);
GS_NET_API_FN_TYPE void __stdcall initH264();
GS_NET_API_FN_TYPE void __stdcall deinitH264();
GS_NET_API_FN_TYPE int __stdcall decodeH264(void* in, int size, void* out);
GS_NET_API_FN_TYPE void __stdcall convertGrayImage
(
	unsigned char* image,
	int width, int height,
	unsigned char* gray
);
GS_NET_API_FN_TYPE void GS_NET_CONSTRUCTOR(char* czConfigFileName);
GS_NET_API_FN_TYPE void GS_NET_DESTROY(int nStatus = GS_NET_SYSTEM_STATUS_SHUTDOWN);

GS_NET_API_FN_TYPE void GS_NET_RegVideoDataCallback(void* lpFunc, void* lpStabFunc, void* lpTCPData_GetDataCallback = NULL);

GS_NET_API_FN_TYPE void GS_NET_FILEPLAY(char* czFileName);
GS_NET_API_FN_TYPE BOOL GS_NET_GCSFileOpen(char* czFileName);

GS_NET_API_FN_TYPE void GS_NET_GCSFilePlay();
GS_NET_API_FN_TYPE void GS_NET_GCSFilePause();

//장치초기화함수
GS_NET_API_FN_TYPE DWORD	GS_NET_DeviceOpen(DWORD dwDeviceID);
GS_NET_API_FN_TYPE DWORD	GS_NET_DeviceClose(DWORD dwDeviceID);

GS_NET_API_FN_TYPE BYTE	GS_NET_Open(DWORD dwDeviceID , BYTE bAppID);
GS_NET_API_FN_TYPE DWORD	GS_NET_Close(DWORD dwDeviceID ,BYTE bAppID);
GS_NET_API_FN_TYPE BOOL	GS_NET_Device_isConnected(DWORD dwDeviceID);
//Callback함수를 
GS_NET_API_FN_TYPE DWORD GS_NET_RegCallback (DWORD dwDeviceID, BYTE bAppID ,DWORD dwCallType,
							  void* lpFunc, DWORD dwPriority, DWORD dwDataFilter);

GS_NET_API_FN_TYPE BOOL GS_NET_CallCallback(DWORD dwDeviceID, DWORD dwDataType, DWORD dwCallType);

//자료송신을 위한 함수
GS_NET_API_FN_TYPE DWORD GS_NET_SendP2Data(BYTE bPid, DWORD dwDeviceID, BYTE bAppID,  BYTE* bpData, BYTE dwHeader, DWORD dwSize, DWORD dwDataType);
GS_NET_API_FN_TYPE DWORD GS_NET_SendData(DWORD dwDeviceID, BYTE bAppID,  BYTE* bpData, BYTE dwHeader, DWORD dwSize, DWORD dwDataType);

GS_NET_API_FN_TYPE void GS_NET_AddPacketSend(BYTE nPktNo, WORD dwSize, BYTE* lpData, BOOL bDestLocal);

//자료가 수신되였을때 물리층에서 호출한다.
GS_NET_API_FN_TYPE DWORD	GS_NET_RecvData(DWORD dwDeviceID, BYTE bAppID, BYTE* bpData, BYTE dwHeader, DWORD dwSize, DWORD dwDataType);

GS_NET_API_FN_TYPE BOOL	GS_NET_TCPSend(DWORD dwDeviceID, DWORD nSize, BYTE* lpData);
GS_NET_API_FN_TYPE DWORD GS_NET_TCPRecv(DWORD dwDeviceID, DWORD nSize, BYTE* lpData);

//자료수신을 위한 함수
GS_NET_API_FN_TYPE DWORD	GS_NET_GetDataSize(DWORD dwDeviceID, BYTE bAppID, DWORD dwDataFilter);
GS_NET_API_FN_TYPE DWORD	GS_NET_GetData(DWORD dwDeviceID, BYTE bAppID, BYTE* bpData, DWORD dwDataFilter);
GS_NET_API_FN_TYPE DWORD	GS_NET_Get_List_Count(DWORD dwDeviceID);

//자료충돌을 피하기 위한 함수
//DWORD	GS_NET_AccessStart(DWORD dwDeviceID, BYTE bAppID, DWORD dwIsSend);
//DWORD GS_NET_AccessEnd(DWORD dwDeviceID, BYTE bAppID, DWORD dwIsSend);

//함수호출에서의 오유처리를 위한 함수
GS_NET_API_FN_TYPE DWORD GS_NET_GetLastErrorCode(DWORD dwDeviceID);

//GS_NET 내부처리함수
/*
bP1OrCmd 목적지에서  0:1형  1: 2형
		 원천지에서  0:지령 1:상태 
bRemote   1:원격지, 0:local
bdest:목적지 sDest : subDest 
*/
GS_NET_API_FN_TYPE BYTE GS_NET_MakeDestOrSrc(BYTE bP1OrCmd, BYTE bRemote, BYTE bDest, BYTE sDest);
GS_NET_API_FN_TYPE void GS_NET_RegularDestOrSrc(BYTE inData, BYTE* bP1OrCmd, BYTE* bRemote, BYTE* bDest, BYTE* sDest);

GS_NET_API_FN_TYPE DWORD  GS_NET_MakeDataType(BYTE bDest, BYTE bSrc, BYTE b3 = 0, BYTE b4 = 0);
GS_NET_API_FN_TYPE void GS_NET_RegularDataType(DWORD dwInData, BYTE* bDest, BYTE* bSrc, BYTE* b3, BYTE* b4);

GS_NET_API_FN_TYPE void  GS_NET_DeleteDataHeader(GS_NET_DATA_LIST* lpDataList);
GS_NET_API_FN_TYPE void  GS_NET_AddDataList(GS_NET_DATA_LIST* lpDataList, GS_NET_DATA* lpData);
GS_NET_API_FN_TYPE void  GS_NET_DeleteAllDataList(GS_NET_DATA_LIST* lpDataList);
GS_NET_API_FN_TYPE DWORD GS_NET_BUFFER_SIZE(GS_NET_DATA_LIST* lpDataList);

GS_NET_API_FN_TYPE void GS_NET_GetSendRegularData(BYTE dwHeader, DWORD dwSize, DWORD dwDataType, BYTE* lpData, BYTE* lpResult);
GS_NET_API_FN_TYPE void GS_NET_GetRecvRegularData(BYTE* lpInData, BYTE* dwHeader, DWORD* dwSize, DWORD* dwDataType, BYTE* lpData);

/*
	DLL 상태얻기
*/

GS_NET_API_FN_TYPE void GS_NET_LiveRecordFileName(char* fileName);
GS_NET_API_FN_TYPE void	GS_NET_SetVideoRecord(BOOL bRecordStatus, char* czFileName = NULL);
GS_NET_API_FN_TYPE BOOL GS_NET_GetVideoRecord();

GS_NET_API_FN_TYPE BOOL GS_NET_GetFilePlayStatus();
GS_NET_API_FN_TYPE void GS_NET_SetFilePlaySpeed(int nSpeed);
GS_NET_API_FN_TYPE int GS_NET_GetFilePlaySpeed();

GS_NET_API_FN_TYPE void GS_NET_GetLogTxtList(gs_logType* logList);
GS_NET_API_FN_TYPE void GS_NET_AddLogTxt(WCHAR* wzStr);

GS_NET_API_FN_TYPE void GS_NET_SystemInfo(gs_pkt_GCS_Status_Type* m_stPktGCSStatus, gs_pkt_UAV_Status_Type* m_stPktUAVStatus);

GS_NET_API_FN_TYPE BOOL GS_NET_GetVideoStabEnable(void);
GS_NET_API_FN_TYPE void GS_NET_SetVideoStabEnable(BOOL bStatus);

GS_NET_API_FN_TYPE void GS_NET_SetVideoStabParam(int nInFrameRate, int nOutFrameRate=25, int nImageLevel=10);
GS_NET_API_FN_TYPE void GS_NET_SetVideoStabCrop(int nXpos, int nYpos);

GS_NET_API_FN_TYPE void GS_NET_SetGCSPlayPosition(int nPos);
GS_NET_API_FN_TYPE int GS_NET_GetGCSPlayPosition();

GS_NET_API_FN_TYPE DWORD GS_NET_SetGCSPlayTimePosition(DWORD nTickTime);
GS_NET_API_FN_TYPE DWORD GS_NET_GetGCSPlayTimePosition();

GS_NET_API_FN_TYPE int GS_NET_GetGCSFrameCount();
GS_NET_API_FN_TYPE DWORD GS_NET_GetGCSFrameTotalTime();

GS_NET_API_FN_TYPE void GS_NET_HeaderGenerate(int nInterval);

GS_NET_API_FN_TYPE void GS_NET_SetLiveEnable(BOOL bEnable);
GS_NET_API_FN_TYPE BOOL GS_NET_GetLiveEnable();

GS_NET_API_FN_TYPE BOOL GS_NET_VideoImageCapture(BOOL bLive);

//송수신 완충기변수정의부
extern GS_NET_CALLBACK* lpgsNetCallbackFirst[GS_DEVICE_MAX];
extern GS_NET_DATA_LIST	gsNetSendData[GS_DEVICE_MAX];
extern GS_NET_DATA_LIST	gsNetRecvData[GS_DEVICE_MAX];
extern GS_NET_DATA_LIST	gsNetMsgData;

#endif
