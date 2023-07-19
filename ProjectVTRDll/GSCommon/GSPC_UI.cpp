#include "stdafx.h"
#include "GSPC_UI.h"
#include "GSUtilityCls.h"

/************************************************************************/
/*Timer Callback Function			                                    */
/************************************************************************/
static byte gTimerCount = 0;
void CALLBACK GSSystemTimerProc(HWND hwnd, UINT uMsg, UINT idEvent, DWORD dwTime)
{
	CUtilityCls* utilObj = CUtilityCls::GetInstance();

	if (utilObj->lpGSPCUIObj->m_TimerID_GSSystem == idEvent)
	{
		gTimerCount++;
		
		if (!GS_NET_Device_isConnected(GS_DEV_PC_TCP_SERVER))
		{
			//utilObj->InitVariable();
		}

		if (gTimerCount % 10 == 0)
		{
			float speed = (float)((utilObj->m_nIncomingDataSpeed * 8 / (double)(dwTime - utilObj->m_nIncomingDataSpeedTick) * 1000) / 1024.0);
			utilObj->m_nIncomingDataSpeedTick = dwTime;
			utilObj->m_nIncomingDataSpeed = 0;

			utilObj->AddLogText(GS_NET_LOG_RECV_AMOUNT, L"수신 자료량 %0.01f Kbps", speed);
		}

	}

	return;
}

GSPC_UICls::GSPC_UICls()
{
	srand( (unsigned)time( NULL ) );
}

GSPC_UICls::~GSPC_UICls()
{
}

void GSPC_UICls::Init()
{
	CUtilityCls::GetInstance()->m_isClose = FALSE;

	CUtilityCls* utilObj = CUtilityCls::GetInstance();
	utilObj->lpH264LiveDecoder->Init();
}

void GSPC_UICls::Release()
{
	if (!IsBadReadPtr(CUtilityCls::GetInstance()->lpH264LiveDecoder, 4))
		CUtilityCls::GetInstance()->lpH264LiveDecoder->Close();
}


	/************************************************************************/
	/* IP Camera Send                                                       */
	/************************************************************************/
	////INIT
	////
	//DWORD dwDevID = GS_DEV_PC_IPCAM_TCP_CLIENT_DATA;
	//DWORD dwBuffSize = 16*1024;
	//BYTE lpBuff[10000] = {0};

	//CString strHTTPheaders = _T("GET /videostream.cgi?user=admin&pwd=111&resolution=32&rate=0 HTTP/1.1");

	//strHTTPheaders += "User-Agent: Mozilla/5.0\r\n";
	////strHeaders += "User-Agent: HttpCall\r\n";
	////strHeaders += "Accept-Language: en-us\r\n";
	//strHTTPheaders += "Accept: image/png,image/*;q=0.8,*/*;q=0.5\r\n";
	//strHTTPheaders += "Accept-Language: en-us,en;q=0.5\r\n";
	//strHTTPheaders += "Accept-Encoding: gzip,deflate\r\n";
	//strHTTPheaders += "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\r\n";
	//strHTTPheaders += "Keep-Alive: 300\r\n";
	//strHTTPheaders += "Connection: keep-alive\r\n";
	//strHTTPheaders += "Referer: http://100.100.100.5/monitor2.htm\r\n";
	//strHTTPheaders += "Cookie: noshow=0; browser=1\r\n";
	//strHTTPheaders += "Authorization: Basic YWRtaW46MQ==\r\n";
	//strHTTPheaders += "\r\n";
	
	//sprintf_s((char*)lpBuff, strHTTPheaders.GetLength() + 1, "%S", strHTTPheaders.GetBuffer());
	//DWORD dwErr = GS_NET_SendData(dwDevID, gbAppID, lpBuff, 0xAA, strHTTPheaders.GetLength());
	//if(dwErr == 1)
	//{

	//}
	
	