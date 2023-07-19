#pragma once

#include "FFMpegInstance.h"
#include "vfw.h"
#include "GSLockMng.h"
#include "ArrayList.h"

#define OK							  0

#define ERROR_FILE_NOT_FOUNDED		 -1
#define ERROR_FILE_OPEN				 -2
#define ERROR_NOT_FOUND_INPUT_FORMAT -3
#define ERROR_NOT_FOUND_STREAM		 -4
#define ERROR_NOT_FOUND_VIDEO_STREAM -5
#define ERROR_NOT_FOUND_CODEC		 -6
#define ERROR_OPEN_CODEC			 -7
#define ERROR_ALLOC_FRAME			 -8
#define ERROR_CONVERT_RGB24			 -9

struct frame_struct_type
{
	int		nNo;
	int		nKey;
	int		nSize;
	DWORD	nTime;
	DWORD   nBeforeSize;
};
class CH264Decoder
{
public:
	CH264Decoder(void);
	~CH264Decoder(void);
	void Close();
public:
    AVCodec*			m_pCodec;
    AVCodecContext*		m_pCodecContext;
	AVFormatContext*	m_pFormatContext;
	AVInputFormat*		m_pInputFormat;
	AVStream*			m_pAVStream;
	AVPacket			m_packet;
    AVFrame*			m_pFrame;
    AVFrame*			m_pFrameRGB;
	uint8_t*			m_lpBuffer;
	struct SwsContext*  m_pImg_Convert_ctx;
	int					m_nFrameIndex;
	int					m_nFrameCount;
	int					m_nFrameRate;
	int				    m_nAverageTime;
	int					m_nWidth;
	int					m_nRealWidth;
	int					m_nHeight;
	__int64				m_nFramePos;
	__int64				m_nFirstKeyFramePos;
	BOOL				m_bLive;
	
public:
	void InitMemberVariables();
	int OpenFile(char *szFileName);

	void LiveVideoFileClose();
	BOOL OpenGCSFile(char *szFileName);
	BOOL fileParser();
	BOOL fileReadLn();
	int GetGCSFrameCount() { return m_nFrameCount; }
	DWORD GetGCSFrameTotalTime();
	void fileGCSPlay();
	void fileGCSStop();
	void fileGCSPosition(int nPos);

	DWORD GetfileGCSTimePosition();
	DWORD SetfileGCSTimePosition(DWORD nTickTime);

	int m_nCurFramePosition;
	BOOL m_bPlayStatus;
	BOOL m_bThreadStatus;

	void Release();
	bool SeekTo(__int64 pos);
	uint8_t* ReadFrame();
	int GetFrameCount() { return m_nFrameCount; }

	BOOL Init();
	int Decode(void* in, int size, void* out);
	void OnlyDecode();
	void PostDecode(unsigned char *dis_buf, int dis_width, int dis_height, void* out);
	int DisplayNoRecordingPicture();
	void SetCrop(int xPos, int yPos);
	BOOL VideoImageCapture();

	static void __stdcall DrawStabilizedFrame(BYTE* pBuffer, int w, int h);

public:
	void BitMapInfoInit(PBITMAPINFO pBitmapInfo,LONG BitmapWidth, LONG BitmapHeight);
	void SaveVideoData(int int_frame_size, BYTE* ptr_frame_data);

	unsigned char* m_pDisplayBuffer;
	SIZE m_DisplaySize;
	int zoom;
 	double yScale;
	double crScale;
	double cbScale;
	int numBytes;

public:
	CFile m_fileLiveVideoData;
	CFile m_fileLiveVideoHeaderData;

	CFile m_fileSearchVideoData;
	CFile m_fileSearchVideoHeaderData;

	frame_struct_type m_stVideoFrame;

	CArrayList m_listFileHeader;
};
