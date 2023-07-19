#pragma once

extern "C"
{
#include <libavcodec/avcodec.h>
#include <libavformat/avformat.h>
#include <libswscale/swscale.h>
#include <libavutil/avutil.h>
};

typedef void (__cdecl *g_avcodec_register_all)(void); 
typedef void (__cdecl *g_av_register_all)(void); 
typedef AVFrame* (__cdecl *g_avcodec_alloc_frame)(void); 
typedef AVFrame* (__cdecl *g_avcodec_free_frame)(AVFrame **frame); 
typedef void (__cdecl *g_avcodec_get_frame_defaults)(AVFrame *frame);
typedef int (__cdecl *g_avcodec_close)(AVCodecContext *avctx);
typedef int (__cdecl *g_avcodec_flush_buffers)(AVCodecContext *avctx);
typedef AVCodec* (__cdecl *g_avcodec_find_decoder)(enum AVCodecID id); 
typedef AVCodecContext* (__cdecl *g_avcodec_alloc_context3)(AVCodec *codec); 
typedef int (__cdecl *g_avcodec_open2)(AVCodecContext *avctx, AVCodec *codec, AVDictionary **options); 
typedef int (__cdecl *g_avcodec_decode_video2)(AVCodecContext *avctx, AVFrame *picture, int *got_picture_ptr, AVPacket *avpkt); 
typedef int (__cdecl *g_avpicture_get_size)(enum PixelFormat pix_fmt, int width, int height); 
typedef int (__cdecl *g_avcodec_init_packet)(AVPacket* avpkt);
typedef void (__cdecl *g_avcodec_free_packet)(AVPacket* avpkt);
typedef int (__cdecl *g_avpicture_fill)(AVPicture *picture, uint8_t *ptr, enum PixelFormat pix_fmt, int width, int height); 
typedef struct SwsContext * (__cdecl *g_sws_getContext)(int srcW, int srcH, enum PixelFormat srcFormat,
                                  int dstW, int dstH, enum PixelFormat dstFormat,
                                  int flags, SwsFilter *srcFilter,
                                  SwsFilter *dstFilter, const double *param); 

typedef AVInputFormat* (__cdecl *g_avformat_find_input_format)(const char *short_name);
typedef int (__cdecl *g_avformat_network_init)(void);
typedef int (__cdecl *g_avformat_open_input)(AVFormatContext **ps, const char *filename, AVInputFormat *fmt, AVDictionary **options);
typedef int (__cdecl *g_avformat_read_frame)(AVFormatContext *s, AVPacket *pkt);
typedef void (__cdecl *g_avformat_close_input)(AVFormatContext **s);
typedef int (__cdecl *g_avformat_find_stream_info)(AVFormatContext *ic, AVDictionary **options);
typedef int (__cdecl *g_avformat_seek_frame)(AVFormatContext *s, int stream_index, int64_t timestamp, int flags);

typedef void (__cdecl *g_av_log_set_level)(int);
typedef void (__cdecl *g_avutil_freep)(void *ptr);
typedef void* (__cdecl *g_avutil_malloc)(size_t size) av_malloc_attrib av_alloc_size(1);

typedef int (__cdecl *g_av_log_set_callback)(void (*)(void*, int, const char*, va_list));
typedef int (__cdecl *g_sws_scale)(struct SwsContext *c, const uint8_t *const srcSlice[],
              const int srcStride[], int srcSliceY, int srcSliceH,
              uint8_t *const dst[], const int dstStride[]);

typedef int64_t (__cdecl *g_avutil_rescale_q)(int64_t a, AVRational bq, AVRational cq) av_const;
typedef struct SwsContext* (__cdecl *g_sws_getCachedContext)(struct SwsContext *context,
                                        int srcW, int srcH, enum AVPixelFormat srcFormat,
                                        int dstW, int dstH, enum AVPixelFormat dstFormat,
                                        int flags, SwsFilter *srcFilter,
                                        SwsFilter *dstFilter, const double *param);


class CFFMpegInstance
{
public:
	static void* g_ffmpegInstance;
	CFFMpegInstance(void);
	~CFFMpegInstance(void);

	void ReleaseLib();
	static CFFMpegInstance* GetInstance()
	{
		if (g_ffmpegInstance == NULL)
		{
			g_ffmpegInstance = new CFFMpegInstance();
		}
		return (CFFMpegInstance*)g_ffmpegInstance;
	}

public:
	HMODULE m_hAvCodec;
	HMODULE m_hAvFormat;
	HMODULE m_hAvUtil;
	HMODULE m_hSwscale;

	g_avcodec_register_all f_avcodec_register_all;
	g_av_register_all	f_av_register_all;	
	g_avcodec_alloc_frame	f_avcodec_alloc_frame;
	g_avcodec_free_frame	f_avcodec_free_frame;
	g_avcodec_get_frame_defaults f_avcodec_get_frame_defaults;
	g_avcodec_close f_avcodec_close;
	g_avcodec_flush_buffers f_avcodec_flush_buffers;
	g_avcodec_find_decoder	f_avcodec_find_decoder;
	g_avcodec_alloc_context3	f_avcodec_alloc_context3;
	g_avcodec_open2		f_avcodec_open2;
	g_avcodec_decode_video2		f_avcodec_decode_video2;
	g_avpicture_get_size		f_avpicture_get_size;
	g_avcodec_init_packet    f_avcodec_init_packet;
	g_avcodec_free_packet    f_avcodec_free_packet;
	g_avpicture_fill	f_avpicture_fill;
	g_sws_getContext	f_sws_getContext;

	g_avformat_find_input_format f_avformat_find_input_format;
	g_avformat_network_init f_avformat_network_init;
	g_avformat_open_input f_avformat_open_input;
	g_avformat_close_input f_avformat_close_input;
	g_avformat_find_stream_info f_avformat_find_stream_info;
	g_avformat_seek_frame f_avformat_seek_frame;

	g_avformat_read_frame f_avformat_read_frame;
	g_av_log_set_level		f_av_log_set_level;
	g_avutil_malloc				f_avutil_malloc;
	g_avutil_freep			f_avutil_freep;
	g_av_log_set_callback	f_av_log_set_callback;
	g_sws_scale				f_sws_scale;
	g_avutil_rescale_q f_avutil_rescale_q;
	g_sws_getCachedContext f_sws_getCachedContext;

};
