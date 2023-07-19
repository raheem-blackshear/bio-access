#include "StdAfx.h"
#include "H264Decoder.h"
#include "changepic.h"
#include "GSUtilityCls.h"
#include <math.h>

#pragma comment(lib, "vfw32.lib")

static CMyMutex m_Lock;
static BOOL bVStabRun = FALSE;
static BOOL bVStabEnable = FALSE;
HResTimer theH264Timer;

#define NO_RECORDING_KEY_VALUE 10
#define NO_RECORDING_STEPTIME 1000

CH264Decoder::CH264Decoder(void)
{
	InitMemberVariables();

	m_bPlayStatus = FALSE;
	m_bLive = FALSE;
}

CH264Decoder::~CH264Decoder(void)
{
	if (CFFMpegInstance::g_ffmpegInstance)
	{
		CFFMpegInstance::GetInstance()->ReleaseLib();
		delete CFFMpegInstance::g_ffmpegInstance;
	}

	CFFMpegInstance::g_ffmpegInstance = NULL;
}

void CH264Decoder::Close()
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	
	m_bPlayStatus = FALSE;
	while (m_bThreadStatus)
	{
		Sleep(1);
	}
	
#ifdef _GS_VSTAB_ENABLE
	IVStabEngine_Stop();
#endif

	m_Lock.Lock();
	Release();
	m_Lock.UnLock();
	
	LiveVideoFileClose();

	if (m_fileSearchVideoData.m_hFile != INVALID_HANDLE_VALUE)
		m_fileSearchVideoData.Close();
	if (m_fileSearchVideoHeaderData.m_hFile != INVALID_HANDLE_VALUE)
		m_fileSearchVideoHeaderData.Close();

	m_listFileHeader.RemoveAll();
}


void CH264Decoder::InitMemberVariables()
{
	m_bThreadStatus = FALSE;

	m_pFormatContext = NULL;
	m_pInputFormat = NULL;
	m_pAVStream = NULL;
	m_pCodecContext = NULL;
	m_pCodec = NULL;
	m_pFrame = NULL;
	m_pFrameRGB = NULL;
	m_lpBuffer = NULL;
	m_pImg_Convert_ctx = NULL;
	m_pDisplayBuffer = NULL;
	m_DisplaySize.cx = 0;
	m_DisplaySize.cy = 0;
	
	m_nFrameCount = 5;
	m_nFrameRate = -1;
	m_nWidth = -1;
	m_nHeight = -1;
	m_nRealWidth = -1;
	m_nFramePos = -1;
	m_nFirstKeyFramePos = -1;
	m_nCurFramePosition = 0;

	zoom = 1;
	yScale = 0.0;
	crScale = 0.0;
	cbScale = 0.0;
	numBytes = 0;

	m_nFrameIndex = -1;
}

BOOL CH264Decoder::fileReadLn()
{
	char czChar[10];
	BOOL rs =FALSE;

	int nPos = 0;
	char rsChar[100];

	while (m_fileSearchVideoHeaderData.Read((void*)czChar, 1) > 0)
	{
		if (czChar[0] == 0x0D)
		{
			rs = TRUE;
			continue;
		}
		
		if (rs && czChar[0] == 0x0A)
			break;
		
		rs =FALSE;
		rsChar[nPos] = czChar[0];
		nPos++;
	}

	if (nPos == 0)
		return FALSE;

	rsChar[nPos] = 0;

	//Key=1,Size=7601,Time=23265578
	frame_struct_type* tmpStFrame = new frame_struct_type;

	tmpStFrame->nNo = 0;
	tmpStFrame->nBeforeSize = 0;
	int nn = sscanf(rsChar, "Key=%d,Size=%d,Time=%d", &tmpStFrame->nKey, &tmpStFrame->nSize, &tmpStFrame->nTime);

	if (nn > 0)
	{
		nn = m_listFileHeader.Count();
		if (nn > 0)
			tmpStFrame->nBeforeSize += ((frame_struct_type*)m_listFileHeader.Get(nn - 1))->nSize + ((frame_struct_type*)m_listFileHeader.Get(nn - 1))->nBeforeSize;

		m_listFileHeader.Add((void*)tmpStFrame);
	}

	return TRUE;
}

BOOL CH264Decoder::fileParser()
{
	char cEndSymbol[2] = {0x0d, 0x0a};
	BOOL rs =FALSE;

	int nPos = 0 , nPos1;
	char rsChar[100];
	int nLength = (int)m_fileSearchVideoHeaderData.GetLength();
	char* czTotalFileContents = new char[nLength];
	m_fileSearchVideoHeaderData.Read((void*)czTotalFileContents, nLength);

	DWORD dwTickTmp = 0;
	DWORD dwTimeBack;
	long dwDiffTick = 0;
	while (TRUE)
	{
		nPos1 = gMemFind(czTotalFileContents, nLength, cEndSymbol, 2, nPos);
		if (nPos1 < 0)
			break;

		for (int i=nPos; i<nPos1; i++)
			rsChar[i-nPos] = czTotalFileContents[i];

		rsChar[nPos1 - nPos] = 0;


		//Key=1,Size=7601,Time=23265578
		frame_struct_type* tmpStFrame = new frame_struct_type;

		tmpStFrame->nNo = 0;
		tmpStFrame->nBeforeSize = 0;
		int nn = sscanf(rsChar, "Key=%d,Size=%d,Time=%d", &tmpStFrame->nKey, &tmpStFrame->nSize, &tmpStFrame->nTime);

		tmpStFrame->nTime += dwDiffTick;
		if (dwTickTmp == 0)
			dwTickTmp = tmpStFrame->nTime - 1;

		if (nn > 0)
		{
			if (tmpStFrame->nTime <= dwTickTmp)
			{
				dwDiffTick += dwTickTmp - tmpStFrame->nTime;
				dwTickTmp += 10;
				tmpStFrame->nTime = dwTickTmp;
			}

			nn = m_listFileHeader.Count();

			if (nn > 0)
				tmpStFrame->nBeforeSize += ((frame_struct_type*)m_listFileHeader.Get(nn - 1))->nSize + ((frame_struct_type*)m_listFileHeader.Get(nn - 1))->nBeforeSize;
			
			dwTimeBack = tmpStFrame->nTime;

			if ((int)(dwTimeBack - dwTickTmp) > NO_RECORDING_STEPTIME)
			{
				while ((int)(dwTimeBack - dwTickTmp) > NO_RECORDING_STEPTIME)
				{
					frame_struct_type* tmpStFrameNext = new frame_struct_type;
					tmpStFrameNext->nNo		= tmpStFrame->nNo;
					tmpStFrameNext->nSize	= tmpStFrame->nSize;
					tmpStFrameNext->nBeforeSize = tmpStFrame->nBeforeSize;

					dwTickTmp += NO_RECORDING_STEPTIME/ 5;
					tmpStFrameNext->nTime = dwTickTmp;
					tmpStFrameNext->nKey = NO_RECORDING_KEY_VALUE;

					m_listFileHeader.Add((void*)tmpStFrameNext);

					CUtilityCls::GetInstance()->AddLogText(1, L"Key=%d,Size=%d,Time=%d", tmpStFrameNext->nKey, tmpStFrameNext->nSize, tmpStFrameNext->nTime);
				}
				
				delete tmpStFrame;
			}else
			{
				CUtilityCls::GetInstance()->AddLogText(1, L"Key=%d,Size=%d,Time=%d", tmpStFrame->nKey, tmpStFrame->nSize, tmpStFrame->nTime);
				dwTickTmp = tmpStFrame->nTime;
				m_listFileHeader.Add((void*)tmpStFrame);
			}
		}

		nPos = nPos1 + 2;
	}
	
	delete czTotalFileContents;
	return TRUE;
}

BOOL CH264Decoder::OpenGCSFile(char *szFileName)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (m_fileSearchVideoData.m_hFile != INVALID_HANDLE_VALUE)
	{
		m_fileSearchVideoData.Close();
		m_fileSearchVideoHeaderData.Close();
	}

	CString fileNameStr;
	fileNameStr.Format(L"%S", szFileName);

	if (m_fileSearchVideoData.Open(fileNameStr.GetBuffer(), CFile::modeNoTruncate | CFile::modeRead | CFile::typeBinary | CFile::shareDenyNone))
	{
		m_fileSearchVideoData.SeekToEnd();

		// parse Header
		fileNameStr = fileNameStr.Left(fileNameStr.Find(L"."));
		fileNameStr = fileNameStr + L".dta";
		m_listFileHeader.RemoveAll();

		if (m_fileSearchVideoHeaderData.Open(fileNameStr, CFile::modeNoTruncate | CFile::modeRead | CFile::typeBinary | CFile::shareDenyNone))
		{
			//while (fileReadLn()){}
			fileParser();

			m_nFrameCount = m_listFileHeader.Count();
			m_nCurFramePosition = 0;
			m_bPlayStatus = FALSE;

			if (m_nFrameCount > 2)
			{
				frame_struct_type* tmpObj = (frame_struct_type*)m_listFileHeader.Get(1);
				if (tmpObj->nKey == 1)
					((frame_struct_type*)m_listFileHeader.Get(0))->nTime = tmpObj->nTime;

				m_nFrameRate = -1;
				int nTmpCount = 1;
				frame_struct_type* tmpObj1;

				for (int i=1; i<m_nFrameCount; i++)
				{
					tmpObj1 = (frame_struct_type*)m_listFileHeader.Get(i - 1);
					tmpObj = (frame_struct_type*)m_listFileHeader.Get(i);
					
					if ( (int)(tmpObj->nTime/1000) == (int)(tmpObj1->nTime/1000) )
					{
						nTmpCount++;
					} else
					{
						if (m_nFrameRate == nTmpCount)
							break;

						m_nFrameRate = nTmpCount;
						nTmpCount = 1;
					}
				}

				m_nFrameRate = m_nFrameRate<1?1:m_nFrameRate;

				m_nAverageTime = 1000 / m_nFrameRate;

				//check time data
				for (int i=2; i<m_nFrameCount; i++)
				{
					tmpObj1 = (frame_struct_type*)m_listFileHeader.Get(i - 1);
					tmpObj = (frame_struct_type*)m_listFileHeader.Get(i);

					if (tmpObj1->nTime > tmpObj->nTime)
					{
						MessageBox(AfxGetMainWnd()->GetSafeHwnd(), L"Video Header화일의 시간정보가 정확히 기록되지 않았습니다.\n Header화일을 다시 창조하십시오.\n", L"File Player", MB_OK);
						return FALSE;
					}
				}

				return TRUE;
			}
		} 
		
		MessageBox(AfxGetMainWnd()->GetSafeHwnd(), L"Video Header화일이 존재하지 않거나 읽기 오유가 발생하였습니다..\r\n 화면재생방식으로 동작합니다.", L"File Player", MB_OK);
		OpenFile(szFileName);
		ReadFrame();
		return FALSE;
	}
	
	CFFMpegInstance::GetInstance()->f_avcodec_flush_buffers(m_pCodecContext);

	return FALSE;
}

void __cdecl procGCSFilePlayThread(void* pArg)
{
	CH264Decoder *gsH264Obj = (CH264Decoder *)pArg;
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"File Play Start");

	frame_struct_type* tmpData = (frame_struct_type*)gsH264Obj->m_listFileHeader.Get(gsH264Obj->m_nCurFramePosition);
	if (tmpData == NULL)
	{
		utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"Header 자료 없음.");
		gsH264Obj->m_bThreadStatus = FALSE;
		return;
	}

	DWORD dwStartTime = tmpData->nTime;
	if (gsH264Obj->m_nCurFramePosition > 0)
		dwStartTime = ((frame_struct_type*)gsH264Obj->m_listFileHeader.Get(gsH264Obj->m_nCurFramePosition - 1))->nTime;

	DWORD dwStartTick = theH264Timer.GetSystemTime();
	DWORD dwDiffTick;
	BOOL bDecodeStatus = FALSE;

	BYTE* lpFrameData = new BYTE[GS_NET_RECV_BUFFER_SIZE];

	int dd;

	utilObj->m_nThreadAliveCount++;
	
	while(TRUE)
    {
		if (IsBadReadPtr(gsH264Obj, 4) || IsBadReadPtr(utilObj, 4))
			break;

		if (utilObj->m_isClose)
			break;

		tmpData = (frame_struct_type*)gsH264Obj->m_listFileHeader.Get(gsH264Obj->m_nCurFramePosition);
		dwDiffTick = (theH264Timer.GetSystemTime() - dwStartTick) * utilObj->m_nPlaySpeed;
		dd = tmpData->nTime - dwStartTime - dwDiffTick;
		
		if (dd<0 || dd > 800) 
		{
			utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"-File Play Delay --err time = %d -------", dd);
			dwStartTime = tmpData->nTime;
			dwStartTick = theH264Timer.GetSystemTime();
			dd = 0;
		}

		if (bDecodeStatus && !gsH264Obj->m_bPlayStatus)
		{
			TRACE(L"break--------------;\r\n");
			break;
		}

		if (dd > 1)
		{
			if (bDecodeStatus)
			{
				Sleep(1);
				continue;
			}
		}

		utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"File pos=%d, offsetTime=%d", gsH264Obj->m_nCurFramePosition, gsH264Obj->GetfileGCSTimePosition());

		if (tmpData->nKey == NO_RECORDING_KEY_VALUE)
		{
			gsH264Obj->DisplayNoRecordingPicture();
			bDecodeStatus = TRUE;
		} else
		{
			gsH264Obj->m_fileSearchVideoData.Seek(tmpData->nBeforeSize, CFile::begin);
			gsH264Obj->m_fileSearchVideoData.Read(lpFrameData, tmpData->nSize);

			/*if (gsH264Obj->Decode(tmpData->nSize, lpFrameData, TRUE, 0) > 0)
			{
				bDecodeStatus = TRUE;
				if (gsH264Obj->m_bPlayStatus == FALSE)
					break;
			} else
			{
				bDecodeStatus = FALSE;
				TRACE(L"Decode failed...\r\n");
			}*/
		}

		if (gsH264Obj->m_nCurFramePosition >= gsH264Obj->m_listFileHeader.Count() - 1)
			break;

		gsH264Obj->m_nCurFramePosition++;
	}

	delete lpFrameData;

	utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"Playback Stop.");
	gsH264Obj->m_bPlayStatus = FALSE;
	utilObj->m_nThreadAliveCount--;
	gsH264Obj->m_bThreadStatus = FALSE;

	/*if (!utilObj->m_isClose)
	MessageBox(AfxGetMainWnd()->GetSafeHwnd(), L"Playback가 완료되였습니다.", L"File Player", MB_OK);*/

	//_endthread();
}
HANDLE hGCSFileHandle;
void CH264Decoder::fileGCSPlay()
{
	if (m_listFileHeader.Count() > 1 && m_fileSearchVideoData.m_hFile != INVALID_HANDLE_VALUE)
	{
		m_bPlayStatus = FALSE;
		while (m_bThreadStatus)
		{
			Sleep(1);
		}
		m_bPlayStatus = TRUE;
		m_bThreadStatus = TRUE;

		if (m_nCurFramePosition > 0)
			m_nCurFramePosition++;

		CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"Video File is playing.");
		hGCSFileHandle = (HANDLE)_beginthread(&procGCSFilePlayThread, 0, this);
	}
}

void CH264Decoder::fileGCSStop()
{
	m_bPlayStatus = FALSE;
}

void CH264Decoder::fileGCSPosition(int nPos)
{
	m_bPlayStatus = FALSE;
	m_nCurFramePosition = nPos;

	procGCSFilePlayThread(this);
}

DWORD CH264Decoder::GetfileGCSTimePosition()
{
	frame_struct_type* tmpObj = (frame_struct_type*)m_listFileHeader.Get(m_nCurFramePosition);
	if (tmpObj)
		return tmpObj->nTime - ((frame_struct_type*)m_listFileHeader.Get(0))->nTime;

	return 0;
}

void CH264Decoder::OnlyDecode()
{
	frame_struct_type* tmpObj;

	tmpObj = (frame_struct_type*)m_listFileHeader.Get(m_nCurFramePosition);

	BYTE* lpFrameData = new BYTE[tmpObj->nSize];

	m_fileSearchVideoData.Seek(tmpObj->nBeforeSize, CFile::begin);
	m_fileSearchVideoData.Read(lpFrameData, tmpObj->nSize);

	//Decode(tmpObj->nSize, lpFrameData, FALSE, 0);

	delete lpFrameData;
}
DWORD CH264Decoder::SetfileGCSTimePosition(DWORD nTickTime)
{
	m_bPlayStatus = FALSE;

	while (m_bThreadStatus)
	{
		Sleep(1);
	}

	frame_struct_type* tmpObj;

	int n = m_listFileHeader.Count();
	if (n > 0)
	{
		tmpObj = ((frame_struct_type*)m_listFileHeader.Get(0));
		DWORD nTimeFirst = tmpObj->nTime;
		DWORD diffTime;
		
		int i;
		int nKeyFrame = -1;
		for (i=0; i<n; i++)
		{
			tmpObj = (frame_struct_type*)m_listFileHeader.Get(i);

			if (tmpObj->nKey >= 1)
				nKeyFrame = i;

			diffTime = tmpObj->nTime - nTimeFirst;
			if (diffTime >= nTickTime)
				break;
		}

		//if (tmpObj->nKey == 2)
			CFFMpegInstance::GetInstance()->f_avcodec_flush_buffers(m_pCodecContext);

		if (nKeyFrame >= 0)
		{
			m_nCurFramePosition = nKeyFrame - 2;

			while (m_nCurFramePosition < i)
			{
				if (m_nCurFramePosition >= 0)
					OnlyDecode();

				m_nCurFramePosition++;
			}

		}

		m_nCurFramePosition = i;
		procGCSFilePlayThread(this);

		return GetfileGCSTimePosition();
	}

	return 0;
}


int CH264Decoder::OpenFile(char *szFileName)
{
	CFFMpegInstance* ffMpeg = CFFMpegInstance::GetInstance();

	const char *szExt = NULL;
	int i = 0, nLen = 0;

	Release();
 
	ffMpeg->f_avcodec_register_all();
    ffMpeg->f_av_register_all();

	if (szFileName == NULL || strlen(szFileName) == 0)
	{
		return ERROR_FILE_NOT_FOUNDED;
	}

	if (ffMpeg->f_avformat_open_input(&m_pFormatContext, szFileName, NULL, NULL) != 0)
	{
		nLen = strlen(szFileName);
		
		while (nLen > 0) 
		{
			if (szFileName[nLen] == '.')
			{
				nLen++;
				break;
			}
			nLen--;
		}

		szExt = &(szFileName[nLen]);
		m_pInputFormat = ffMpeg->f_avformat_find_input_format(szExt);
		
		if (m_pInputFormat == NULL)
		{
			m_pInputFormat = ffMpeg->f_avformat_find_input_format ("image2");
		}
            
		if (m_pInputFormat == NULL)
			return ERROR_NOT_FOUND_INPUT_FORMAT;
    
		if (ffMpeg->f_avformat_open_input(&m_pFormatContext, szFileName, m_pInputFormat, NULL) != 0)
			return ERROR_FILE_OPEN;
	}

	if (m_pFormatContext->nb_streams == 0)
		return ERROR_NOT_FOUND_STREAM;


	if(ffMpeg->f_avformat_find_stream_info(m_pFormatContext, NULL) < 0)
        return ERROR_NOT_FOUND_STREAM;

	m_nFrameIndex = -1;
    for(i = 0; i < (int)m_pFormatContext->nb_streams; i++)
		if(m_pFormatContext->streams[i]->codec->codec_type == AVMEDIA_TYPE_VIDEO)
        {
			m_nFrameIndex = i;
            break;
        }

	if(m_nFrameIndex == -1)
		return ERROR_NOT_FOUND_VIDEO_STREAM;

	m_pAVStream = m_pFormatContext->streams[m_nFrameIndex];
	m_nWidth  = m_pAVStream->codec->width;
    m_nHeight = m_pAVStream->codec->height;
	m_nFrameRate = m_pAVStream->r_frame_rate.num/m_pAVStream->r_frame_rate.den;
	m_nFrameCount = m_pAVStream->nb_index_entries;
	if (m_nFrameCount <= 0)
		m_nFrameCount = (int)m_pFormatContext->duration * m_nFrameRate / AV_TIME_BASE;
	
    // Get a pointer to the codec context for the video stream
    m_pCodecContext = m_pFormatContext->streams[m_nFrameIndex]->codec;

	/*
     * OK, so now we've got a pointer to the so-called codec context for our video
     * stream, but we still have to find the actual codec and open it.
    */
    
	// Find the decoder for the video stream
	m_pCodec = ffMpeg->f_avcodec_find_decoder(m_pCodecContext->codec_id);
    if(m_pCodec == NULL)
	{
		m_pCodecContext = NULL;
		return ERROR_NOT_FOUND_CODEC;
	}

	// Open codec
	if(ffMpeg->f_avcodec_open2(m_pCodecContext, m_pCodec, NULL) < 0)
	{
		m_pCodecContext = NULL;
        return ERROR_OPEN_CODEC;
	}

	// Hack to correct wrong frame rates that seem to be generated by some codecs
    if (m_pCodecContext->time_base.den > 1000 && m_pCodecContext->time_base.num == 1)
	{
		 m_pCodecContext->time_base.num = 1000;
	}

    /*
	* Allocate a video frame to store the decoded images in.
	*/
	m_pFrame = ffMpeg->f_avcodec_alloc_frame();
	
	// Allocate an AVFrame structure
    m_pFrameRGB = ffMpeg->f_avcodec_alloc_frame();
    if(m_pFrameRGB == NULL)
	{
		m_pFrameRGB = NULL;
		return ERROR_ALLOC_FRAME;
	}
	
    // Determine required buffer size and allocate buffer
	numBytes = ffMpeg->f_avpicture_get_size(PIX_FMT_BGR24, m_pCodecContext->width, m_pCodecContext->height);
    m_lpBuffer = (uint8_t *)ffMpeg->f_avutil_malloc(numBytes);
	
	m_nRealWidth = numBytes / m_pCodecContext->height;
	
	// Assign appropriate parts of buffer to image planes in pFrameRGB
    ffMpeg->f_avpicture_fill((AVPicture *)m_pFrameRGB, m_lpBuffer, PIX_FMT_BGR24, m_pCodecContext->width, m_pCodecContext->height);

	m_bPlayStatus = TRUE;

	return OK;
}

void CH264Decoder::Release()
{
	CFFMpegInstance* ffMpeg = CFFMpegInstance::GetInstance();
	
	// Free the RGB image
	if (m_pFrameRGB != NULL)
	{
		ffMpeg->f_avutil_freep(m_pFrameRGB);
		m_lpBuffer = NULL;
	}
	
    // Close the codec
    if (m_pCodecContext != NULL) 
	{
        ffMpeg->f_avcodec_close(m_pCodecContext);
    }
	
    // Close the video file
    if (m_pFormatContext != NULL) 
	{
		ffMpeg->f_avformat_close_input(&m_pFormatContext);
    }

	if (m_pDisplayBuffer)
		delete m_pDisplayBuffer;

	m_pDisplayBuffer = NULL;
	
	InitMemberVariables();
}

BOOL CH264Decoder::Init()
{
	CFFMpegInstance* ffMpeg = CFFMpegInstance::GetInstance();
	
	ffMpeg->f_av_register_all();
	ffMpeg->f_avformat_network_init();

	//m_pCodec = ffMpeg->f_avcodec_find_decoder(AV_CODEC_ID_MJPEG);
	m_pCodec = ffMpeg->f_avcodec_find_decoder(AV_CODEC_ID_H264);

	m_pCodecContext = ffMpeg->f_avcodec_alloc_context3(m_pCodec);
	m_pFrame = ffMpeg->f_avcodec_alloc_frame();
	m_pFrameRGB = ffMpeg->f_avcodec_alloc_frame();

	//if(m_pCodec->capabilities&CODEC_CAP_TRUNCATED)
	//	m_pCodecContext->flags|= CODEC_FLAG_TRUNCATED; /* we do not send complete frames */

	m_pCodec->type = AVMEDIA_TYPE_VIDEO;
	m_pCodecContext->codec_type = AVMEDIA_TYPE_VIDEO;
	m_pCodecContext->codec_id = AV_CODEC_ID_H264;
	m_pCodecContext->thread_count = 2;
	m_pCodecContext->thread_type |= FF_THREAD_SLICE;

	if (ffMpeg->f_avcodec_open2(m_pCodecContext, m_pCodec, NULL) < 0)
	{
		return FALSE;
	}

	m_stVideoFrame.nNo = 0;
	m_stVideoFrame.nKey = 1;

	return TRUE;
}

long g_cnt = 0;

bool CH264Decoder::SeekTo(__int64 pos)
{
	CFFMpegInstance* ffMpeg = CFFMpegInstance::GetInstance();
	
	__int64 dts = m_pFormatContext->duration * (pos - 3 < 0 ? 0 : pos - 3) / m_nFrameCount;

	static AVRational av_r = {1, AV_TIME_BASE};
	//static AVRational av_time_base_q = {1, AV_TIME_BASE};
	//#define AV_TIME_BASE_Q  (av_time_base_q)
	dts = ffMpeg->f_avutil_rescale_q(dts, av_r, m_pFormatContext->streams[m_nFrameIndex]->time_base);
	int hr = ffMpeg->f_avformat_seek_frame(m_pFormatContext, m_nFrameIndex, dts, AVSEEK_FLAG_BACKWARD);
	
	if (hr < 0)
	{
		return FALSE;
	}
	else
	{
		if (m_nFirstKeyFramePos != -1 && pos <= m_nFirstKeyFramePos)
		{
			pos = m_nFirstKeyFramePos;
			if (pos == 0)
			{
				m_nFramePos = m_nFirstKeyFramePos - 1;
				return TRUE;
			}
		}

 		do
 		{
			//ReadFrame(TRUE);
			dts = m_pFormatContext->streams[m_nFrameIndex]->cur_dts - 1;

		} while (m_nFrameCount * dts / m_pFormatContext->streams[m_nFrameIndex]->duration < pos - 1);

		m_nFramePos = pos - 1;
	}

	return TRUE;//파일에서 자료를 읽는다.
}

void __cdecl procFilePlayThread(void* pArg)
{
	CH264Decoder *gsH264Obj = (CH264Decoder *)pArg;
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"File Play start");

	CFFMpegInstance* ffMpeg = CFFMpegInstance::GetInstance();

	AVPacket packet;
	int nFrameFinished = 0;

	//int64_t stream_start_time;
	//int pkt_in_play_range;
	//double duration;
	int64_t start_time = 0;

	/*if (m_nFramePos >= m_nFrameCount - 1)
	{
		gsH264Obj->m_bThreadStatus = FALSE;
		return NULL;
	}*/

	utilObj->m_nThreadAliveCount++;

	DWORD nT0,nT1,nT2 = 1000/6;

	while(TRUE)
    {
		nT0 = theH264Timer.GetSystemTime();
		if (ffMpeg->f_avformat_read_frame(gsH264Obj->m_pFormatContext, &packet) < 0)
			break;

		if (IsBadReadPtr(gsH264Obj, 4) || IsBadReadPtr(utilObj, 4) || IsBadReadPtr(ffMpeg, 4))
			break;

		if (utilObj->m_isClose)
			break;

		if (gsH264Obj->m_bPlayStatus == FALSE)
			break;

		//F_LogWrite(L"packet size - >%d", packet.size);
		//if (packet.stream_index != m_nFrameIndex) //not keyframe
		//	continue;
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		__int64 dts = gsH264Obj->m_pFormatContext->streams[gsH264Obj->m_nFrameIndex]->cur_dts - 1;
		gsH264Obj->m_nFramePos = gsH264Obj->m_nFrameCount * dts / gsH264Obj->m_pFormatContext->streams[gsH264Obj->m_nFrameIndex]->duration;

		//if (packet.stream_index != m_nFrameIndex) //key frame
		//		continue;

		// Decode video frame
		int hr = ffMpeg->f_avcodec_decode_video2(gsH264Obj->m_pCodecContext, gsH264Obj->m_pFrame, &nFrameFinished, &packet);

		if(nFrameFinished > 0)
		{
			// Convert the image from its native format to BGR24
			gsH264Obj->m_pImg_Convert_ctx = ffMpeg->f_sws_getCachedContext(gsH264Obj->m_pImg_Convert_ctx,
				gsH264Obj->m_pCodecContext->width,
				gsH264Obj->m_pCodecContext->height,
				gsH264Obj->m_pCodecContext->pix_fmt,
				gsH264Obj->m_pCodecContext->width,
				gsH264Obj->m_pCodecContext->height,
				PIX_FMT_BGR24,
				SWS_BICUBIC,
				NULL,
				NULL,
				NULL
				);

			if (gsH264Obj->m_pImg_Convert_ctx == NULL)             
				break;

			ffMpeg->f_sws_scale(gsH264Obj->m_pImg_Convert_ctx, gsH264Obj->m_pFrame->data, gsH264Obj->m_pFrame->linesize, 0, 
				gsH264Obj->m_pCodecContext->height, gsH264Obj->m_pFrameRGB->data, gsH264Obj->m_pFrameRGB->linesize);

			if (gsH264Obj->m_nFirstKeyFramePos == -1)
				gsH264Obj->m_nFirstKeyFramePos = gsH264Obj->m_nFramePos;

			/*AVRational r_frame_rate = gsH264Obj->m_pFormatContext->streams[gsH264Obj->m_nFrameIndex]->r_frame_rate;

			int duration = (r_frame_rate.num && r_frame_rate.den ? av_q2d(r_frame_rate) : 0);

			int64_t stream_start_time = gsH264Obj->m_pFormatContext->streams[packet.stream_index]->start_time;
			int pkt_in_play_range = duration == AV_NOPTS_VALUE || (packet.pts - (stream_start_time != AV_NOPTS_VALUE ? stream_start_time : 0)) *
			av_q2d(gsH264Obj->m_pFormatContext->streams[packet.stream_index]->time_base) -
			(double)(start_time != AV_NOPTS_VALUE ? start_time : 0) / 1000000 <= ((double)duration / 1000000);*/

			//gsH264Obj->PostDecode((unsigned char*)gsH264Obj->m_pFrameRGB->data[0], gsH264Obj->m_pCodecContext->width, gsH264Obj->m_pCodecContext->height);
			utilObj->AddLogText(GS_NET_LOG_H264_DECODE, L"File frame %d * %d", gsH264Obj->m_pCodecContext->width, gsH264Obj->m_pCodecContext->height);
			//gDispatchMessage();
			//if (utilObj->m_nVStab == BST_UNCHECKED)
			
			//if (utilObj->m_nPlaySpeed < 10)
			//	Sleep((DWORD)(250 / utilObj->m_nPlaySpeed));

			//Sleep(pkt_in_play_range);
			//return m_pFrameRGB->data[0];

			nT1 = theH264Timer.GetSystemTime();
			SleepEx(max(1,min(nT2,nT2-nT1+nT0)),TRUE);

			TRACE(L"%d\n", max(1,min(nT2,nT2-nT1+nT0)));
		}

		ffMpeg->f_avcodec_free_packet(&packet);
	}

	gsH264Obj->m_bPlayStatus = FALSE;
	gsH264Obj->m_bThreadStatus = FALSE;
	utilObj->m_nThreadAliveCount--;

	utilObj->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"Playback가 완료되였습니다.");

	//if (!utilObj->m_isClose)
	//	MessageBox(AfxGetMainWnd()->GetSafeHwnd(), L"Playback가 완료되였습니다.", L"File Player", MB_OK);

	_endthread();
}

uint8_t* CH264Decoder::ReadFrame()
{
	CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_FILEPLAY_STATUS, L"Video 화일을 Play하고 있습니다.");

	m_bPlayStatus = TRUE;
	m_bThreadStatus = TRUE;

	HANDLE h = (HANDLE)_beginthread(&procFilePlayThread, 0, this);

	return NULL;
}

static INT64 tickCount = 0;
int CH264Decoder::Decode(void* in, int size, void* out)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (utilObj->m_isClose || m_pCodecContext == NULL)
		return -1;

	//m_Lock.Lock();

	CFFMpegInstance* ffMpeg = CFFMpegInstance::GetInstance();

	ffMpeg->f_avcodec_init_packet(&m_packet);
	m_packet.size = size;
	m_packet.data = (uint8_t*)in;

	g_cnt++;

	int int_got_picture = 0;

	int int_decoded_bytes = ffMpeg->f_avcodec_decode_video2(m_pCodecContext, m_pFrame, &int_got_picture, &m_packet);

	if (int_decoded_bytes <= 0 || int_got_picture == 0)
	{
		ffMpeg->f_avcodec_free_packet(&m_packet);
		m_stVideoFrame.nKey = 0;
		//m_Lock.UnLock();
		return -1;
	}

	m_pCodecContext->pkt_timebase;
	m_pCodecContext->ticks_per_frame;
	m_pCodecContext->time_base;
	m_pCodecContext->timecode_frame_start;
	m_nFrameIndex++;

	if(numBytes == 0 || m_DisplaySize.cx != m_pCodecContext->width || m_DisplaySize.cy != m_pCodecContext->height)
	{
		numBytes = ffMpeg->f_avpicture_get_size(PIX_FMT_BGR24, m_pCodecContext->width, m_pCodecContext->height);

		m_lpBuffer = (uint8_t *)ffMpeg->f_avutil_malloc(numBytes);
		ffMpeg->f_avpicture_fill((AVPicture *)m_pFrameRGB, m_lpBuffer, PIX_FMT_BGR24, m_pCodecContext->width, m_pCodecContext->height);
	}

	m_pImg_Convert_ctx = ffMpeg->f_sws_getCachedContext(m_pImg_Convert_ctx,
		m_pCodecContext->width,
		m_pCodecContext->height,
		m_pCodecContext->pix_fmt,
		m_pCodecContext->width,
		m_pCodecContext->height,
		PIX_FMT_BGR24,
		SWS_BICUBIC,
		NULL,
		NULL,
		NULL
		);

	if (m_pImg_Convert_ctx == NULL)
	{
		ffMpeg->f_avcodec_free_packet(&m_packet);
		//m_Lock.UnLock();
		return -1;
	}

	ffMpeg->f_sws_scale(m_pImg_Convert_ctx, m_pFrame->data, m_pFrame->linesize, 0, 
		m_pCodecContext->height, m_pFrameRGB->data, m_pFrameRGB->linesize);

	PostDecode(m_pFrameRGB->data[0], m_pCodecContext->width, m_pCodecContext->height, out);
	ffMpeg->f_avcodec_free_packet(&m_packet);
	
	//m_Lock.UnLock();

	return 1;
}

DWORD dwLastStabTime = 0;
void CH264Decoder::DrawStabilizedFrame(BYTE* pBuffer, int w, int h)
{
#ifdef _GS_VSTAB_ENABLE
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	if (utilObj->m_bVStab)
	{
		int diffTime = theH264Timer.GetSystemTime() - dwLastStabTime;
		utilObj->AddLogText(GS_NET_LOG_H264_DECODE_VSTAB, L"VStab Video Draw 2 time=%d", diffTime);
		dwLastStabTime = theH264Timer.GetSystemTime();

		utilObj->lpVideoStabCallbackFunc(w,h,pBuffer, diffTime);
	}
#endif
}

#define IMAGE_WIDTH		640
#define IMAGE_HEIGHT	480
void CH264Decoder::PostDecode(unsigned char *dis_buf, int width, int height, void* out)
{
	if (m_DisplaySize.cx != width || m_DisplaySize.cy != height)
	{
		m_DisplaySize.cx = width;
		m_DisplaySize.cy = height;
	}

	int x, y, x0, y0;
	int w = (width - IMAGE_WIDTH) / 2, h = (height - IMAGE_HEIGHT) / 2;

	unsigned char* outbuff = (unsigned char*)out;

	for (y = 0, x0 = 0; y < IMAGE_HEIGHT; y++)
	{
		for (x = 0; x < IMAGE_WIDTH; x++)
		{
			y0 = ((height - 1 - y - h) * width + (x + w)) * 3;
			outbuff[x0++] = dis_buf[y0++];
			outbuff[x0++] = dis_buf[y0++];
			outbuff[x0++] = dis_buf[y0++];
		}
	}
}

int CH264Decoder::DisplayNoRecordingPicture()
{	
	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	
	int d = 0;
	int i,j;
	int nColorVal = 0x09;
	for(i=0;i<m_DisplaySize.cy;i++)
	{
		if (i % 100 == 0)
			nColorVal += 17;

		for(j=0;j<m_DisplaySize.cx;j++)
		{
			m_pDisplayBuffer[d++] = nColorVal;
			m_pDisplayBuffer[d++] = nColorVal;
			m_pDisplayBuffer[d++] = nColorVal;
		}
	}

	DWORD offsetTime = 0;
	if (m_listFileHeader.Count() > 0)
	{
		frame_struct_type* tmpData = (frame_struct_type*)m_listFileHeader.Get(0);
		offsetTime = ((frame_struct_type*)m_listFileHeader.Get(m_nCurFramePosition))->nTime - tmpData->nTime;
	}

	if (utilObj->lpVideoCallbackFunc)
		utilObj->lpVideoCallbackFunc(m_DisplaySize.cx, m_DisplaySize.cy, m_pDisplayBuffer, offsetTime);

#ifdef _GS_VSTAB_ENABLE
	if (utilObj->m_bVStab)
	{
		IVStabEngine_SetCamFrame(m_pDisplayBuffer);
	}
#endif

	return 0;
}

void CH264Decoder::SetCrop(int xPos, int yPos)
{
}

void CH264Decoder::BitMapInfoInit(PBITMAPINFO pBitmapInfo, LONG BitmapWidth, LONG BitmapHeight)
{
	pBitmapInfo->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	pBitmapInfo->bmiHeader.biWidth = BitmapWidth / zoom;
	pBitmapInfo->bmiHeader.biHeight = BitmapHeight / zoom;
	pBitmapInfo->bmiHeader.biPlanes =1;
	pBitmapInfo->bmiHeader.biBitCount = 24;
	pBitmapInfo->bmiHeader.biCompression = BI_RGB;
	pBitmapInfo->bmiHeader.biSizeImage = 0;
	pBitmapInfo->bmiHeader.biClrUsed = 0;
	pBitmapInfo->bmiHeader.biClrImportant = 0;
}

void CH264Decoder::LiveVideoFileClose()
{
	m_Lock.Lock();
	if (m_fileLiveVideoData.m_hFile != INVALID_HANDLE_VALUE)
		m_fileLiveVideoData.Close();

	if (m_fileLiveVideoHeaderData.m_hFile != INVALID_HANDLE_VALUE)
		m_fileLiveVideoHeaderData.Close();
	m_Lock.UnLock();
}

void CH264Decoder::SaveVideoData(int int_frame_size, BYTE* ptr_frame_data)
{
	if(m_fileLiveVideoData.m_hFile == INVALID_HANDLE_VALUE)
	{
		CString fileName;
		fileName.Format(L"%s\\%s", CUtilityCls::GetInstance()->m_SystemInfo.wzVideoDataSavePath, CUtilityCls::GetInstance()->m_SystemInfo.wzLiveFilePath);

		m_fileLiveVideoData.Open(fileName + L".h264", CFile::modeCreate | CFile::modeNoTruncate | CFile::modeReadWrite | CFile::typeBinary | CFile::shareDenyNone);
		m_fileLiveVideoHeaderData.Open(fileName + L".dta", CFile::modeCreate | CFile::modeNoTruncate | CFile::modeReadWrite | CFile::typeBinary | CFile::shareDenyNone);

		m_fileLiveVideoData.SeekToEnd();
		m_fileLiveVideoHeaderData.SeekToEnd();
	}

	if (m_fileLiveVideoData.m_hFile != INVALID_HANDLE_VALUE)
	{
		char czData[128];
		sprintf(czData, "Key=%d,Size=%d,Time=%d", m_stVideoFrame.nKey, int_frame_size, m_stVideoFrame.nTime);

		m_fileLiveVideoHeaderData.Write(czData, strlen(czData));

		czData[0] = 0x0D;
		czData[1] = 0x0A;
		m_fileLiveVideoHeaderData.Write(czData, 2);

		m_fileLiveVideoData.Write(ptr_frame_data,int_frame_size);
	}
}

DWORD CH264Decoder::GetGCSFrameTotalTime()
{
	int n = m_listFileHeader.Count();
	if (n > 0)
	{
		return ((frame_struct_type*)m_listFileHeader.Get(n - 1))->nTime - ((frame_struct_type*)m_listFileHeader.Get(0))->nTime;
	}

	return 0;
}

BOOL CH264Decoder::VideoImageCapture()
{
	if (m_pDisplayBuffer == NULL)
		return FALSE;

	CFile m_fileVideoData;
	CString str;

	SYSTEMTIME systemTime;
	GetLocalTime(&systemTime);
	
	str.Format(_T("%sBMP%04d-%02d-%02d %02d%02d%02d_%03d.bmp"), CUtilityCls::GetInstance()->m_SystemInfo.wzVideoDataSavePath, systemTime.wYear, systemTime.wMonth, systemTime.wDay, systemTime.wHour, systemTime.wMinute, systemTime.wSecond, systemTime.wMilliseconds);
	
	if(m_fileVideoData.m_hFile == INVALID_HANDLE_VALUE)
		m_fileVideoData.Open(str, CFile::modeCreate | CFile::modeNoTruncate | CFile::modeReadWrite | CFile::typeBinary | CFile::shareDenyNone);

	if (m_fileVideoData.m_hFile != INVALID_HANDLE_VALUE)
	{
		BITMAPINFO m_pBmp;
		BitMapInfoInit(&m_pBmp, m_DisplaySize.cx, m_DisplaySize.cy);

		BITMAPFILEHEADER hdr;       // bitmap file-header  
		hdr.bfType = 0x4d42;        // 0x42 = "B" 0x4d = "M"  
		// Compute the size of the entire file.  
		hdr.bfSize = (DWORD) (sizeof(BITMAPFILEHEADER) + m_pBmp.bmiHeader.biSize + m_pBmp.bmiHeader.biClrUsed * sizeof(RGBQUAD) + m_pBmp.bmiHeader.biSizeImage); 
		hdr.bfReserved1 = 0; 
		hdr.bfReserved2 = 0; 

		// Compute the offset to the array of color indices.  
		hdr.bfOffBits = (DWORD) sizeof(BITMAPFILEHEADER) + m_pBmp.bmiHeader.biSize + m_pBmp.bmiHeader.biClrUsed * sizeof (RGBQUAD);
		m_fileVideoData.Write((LPVOID) &hdr,sizeof(BITMAPFILEHEADER));
		m_fileVideoData.Write((LPVOID) &m_pBmp.bmiHeader,sizeof(BITMAPINFOHEADER)+ m_pBmp.bmiHeader.biClrUsed * sizeof (RGBQUAD));
		m_fileVideoData.Write(m_pDisplayBuffer, m_DisplaySize.cx * m_DisplaySize.cy * 3);

		m_fileVideoData.Close();
	} else
		return FALSE;

	return TRUE;
}