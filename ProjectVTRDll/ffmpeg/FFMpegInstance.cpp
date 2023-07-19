#include "StdAfx.h"
#include "FFMpegInstance.h"
#include "GSUtilityCls.h"

void* CFFMpegInstance::g_ffmpegInstance = NULL;

static CMyMutex g_cs_file(GS_Lock_kind_CSection);
void ff_log_func(void* ptr, int level, const char* fmt, va_list vl)
{
	if (strlen(CUtilityCls::GetInstance()->m_SystemInfo.czFFMpegLogPah) < 1)
		return;

	g_cs_file.Lock();
	char str_logs[GS_NET_LOGTEXT_MAX_LENGTH] = {0};
	vsprintf(str_logs, fmt, vl);
	FILE* fp=fopen(CUtilityCls::GetInstance()->m_SystemInfo.czFFMpegLogPah, "a+");
	if (fp != NULL)
	{
		fwrite(str_logs, 1, strlen(str_logs), fp);
		fclose(fp);
	}
	g_cs_file.UnLock();
}

CFFMpegInstance::CFFMpegInstance(void)
{
	m_hAvCodec = LoadLibraryA("avcodec-56.dll");
	if (m_hAvCodec != NULL)
	{
		f_avcodec_register_all = (g_avcodec_register_all)GetProcAddress(m_hAvCodec, "avcodec_register_all");

		f_avcodec_alloc_frame = (g_avcodec_alloc_frame)GetProcAddress(m_hAvCodec, "avcodec_alloc_frame");
		f_avcodec_free_frame = (g_avcodec_free_frame)GetProcAddress(m_hAvCodec, "avcodec_free_frame");
		f_avcodec_get_frame_defaults = (g_avcodec_get_frame_defaults)GetProcAddress(m_hAvCodec, "avcodec_get_frame_defaults");
		
		f_avcodec_find_decoder = (g_avcodec_find_decoder)GetProcAddress(m_hAvCodec, "avcodec_find_decoder");
		f_avcodec_alloc_context3 = (g_avcodec_alloc_context3)GetProcAddress(m_hAvCodec, "avcodec_alloc_context3");
		f_avcodec_open2 = (g_avcodec_open2)GetProcAddress(m_hAvCodec, "avcodec_open2");
		f_avcodec_decode_video2 = (g_avcodec_decode_video2)GetProcAddress(m_hAvCodec, "avcodec_decode_video2");
		f_avpicture_get_size = (g_avpicture_get_size)GetProcAddress(m_hAvCodec, "avpicture_get_size");
		f_avpicture_fill = (g_avpicture_fill)GetProcAddress(m_hAvCodec, "avpicture_fill");
		f_avcodec_init_packet = (g_avcodec_init_packet)GetProcAddress(m_hAvCodec, "av_init_packet");
		f_avcodec_free_packet = (g_avcodec_free_packet)GetProcAddress(m_hAvCodec, "av_free_packet");
		f_avcodec_close = (g_avcodec_close)GetProcAddress(m_hAvCodec, "avcodec_close");
		f_avcodec_flush_buffers = (g_avcodec_flush_buffers)GetProcAddress(m_hAvCodec, "avcodec_flush_buffers");	
	} else
		AfxMessageBox(L"error Load Dll");


	m_hAvFormat = LoadLibraryA("avformat-56.dll");
	if (m_hAvFormat != NULL)
	{
		f_av_register_all = (g_av_register_all)GetProcAddress(m_hAvFormat, "av_register_all");
		f_avformat_network_init = (g_avformat_network_init)GetProcAddress(m_hAvFormat, "avformat_network_init");
		f_avformat_open_input = (g_avformat_open_input)GetProcAddress(m_hAvFormat, "avformat_open_input");
		f_avformat_close_input = (g_avformat_close_input)GetProcAddress(m_hAvFormat, "avformat_close_input");
		f_avformat_read_frame = (g_avformat_read_frame)GetProcAddress(m_hAvFormat, "av_read_frame");
		f_avformat_find_input_format = (g_avformat_find_input_format)GetProcAddress(m_hAvFormat, "av_find_input_format");
		f_avformat_find_stream_info = (g_avformat_find_stream_info)GetProcAddress(m_hAvFormat, "avformat_find_stream_info");
		f_avformat_seek_frame = (g_avformat_seek_frame)GetProcAddress(m_hAvFormat, "av_seek_frame");
	} else
		AfxMessageBox(L"error Load Dll");

	m_hAvUtil = LoadLibraryA("avutil-54.dll");
	if (m_hAvUtil != NULL)
	{
		f_avutil_malloc = (g_avutil_malloc)GetProcAddress(m_hAvUtil, "av_malloc");
		f_avutil_freep = (g_avutil_freep)GetProcAddress(m_hAvUtil, "av_freep");
		
		f_avutil_rescale_q = (g_avutil_rescale_q)GetProcAddress(m_hAvUtil, "av_rescale_q");

		f_av_log_set_level = (g_av_log_set_level)GetProcAddress(m_hAvUtil, "av_log_set_level");
		f_av_log_set_callback = (g_av_log_set_callback)GetProcAddress(m_hAvUtil, "av_log_set_callback");
		
		//f_av_log_set_level(48);
		f_av_log_set_callback(ff_log_func);
	} else
		AfxMessageBox(L"error Load Dll");

	m_hSwscale = LoadLibraryA("swscale-3.dll");
	if (m_hSwscale != NULL)
	{
		f_sws_getContext = (g_sws_getContext)GetProcAddress(m_hSwscale, "sws_getContext");
		f_sws_scale = (g_sws_scale)GetProcAddress(m_hSwscale, "sws_scale");
		f_sws_getCachedContext = (g_sws_getCachedContext)GetProcAddress(m_hSwscale, "sws_getCachedContext");
	} else
		AfxMessageBox(L"error Load Dll");

}

CFFMpegInstance::~CFFMpegInstance(void)
{

}

void CFFMpegInstance::ReleaseLib()
{
	FreeLibrary(m_hAvCodec);
	FreeLibrary(m_hAvFormat);
	FreeLibrary(m_hAvUtil);
	FreeLibrary(m_hSwscale);
}
