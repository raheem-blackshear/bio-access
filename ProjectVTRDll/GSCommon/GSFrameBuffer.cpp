
#include "stdafx.h"

#include "GSFrameBuffer.h"
#include "GSUtilityCls.h"
#ifndef GS_PROJECT_PC
#include <stdlib.h>
#include <errno.h>
#endif

#define gs_video_OneFrame_MaxSize 300 * 1024

GSFrameBuffer::GSFrameBuffer()
{
	m_frame_size = 0;	
}

GSFrameBuffer::~GSFrameBuffer()
{
	delete m_buffer;
}

void GSFrameBuffer::init(int buf_size, int size_opset)
{
	m_buffer = new BYTE[buf_size];
	m_size_opset = size_opset;

	read_end = 0;
	is_frame_continue = 0;
	m_max_memory_size = buf_size;
}

const unsigned char ident0[] = {0x00, 0x00, 0x01, 0xFC};
const unsigned char ident1[] = {0x00, 0x00, 0x01, 0xFD};
const unsigned char sig_start[] = {0xFF, 0x01, 0x00, 0x00};

BOOL GSFrameBuffer::append(void* buf, int size)
{
	if (read_end + size >= m_max_memory_size)
	{
		read_end = 0;
		return FALSE;
	}

	memcpy(&m_buffer[read_end], buf, size);
	read_end+= size;
	int sig_pos;
	do 
	{
		sig_pos = gMemFind((char*)m_buffer, read_end, (char*)sig_start, sizeof(sig_start));
		if (sig_pos >= 0 && sig_pos + 20 <= read_end)
		{
			memmove(&m_buffer[sig_pos], &m_buffer[sig_pos + 20], read_end - sig_pos - 20);
			read_end -= 20;
		}
		else
		{
			break;
		}
	} while (1);
	return TRUE;
}

int GSFrameBuffer::getFrameSize()
{
	return m_frame_size;
}

int GSFrameBuffer::findSize()
{
	int search_first0, search_first1;
	m_nIdentifier_size = sizeof(ident0);

	if (m_nIdentifier_size >= read_end)
		return -1;

	search_first0 = gMemFind((char*)m_buffer, read_end, (char*)ident0, sizeof(ident0));
	search_first1 = gMemFind((char*)m_buffer, read_end, (char*)ident1, sizeof(ident1));

	if (search_first0 < 0 && search_first1 < 0)
		return -1;
	if (search_first0 < 0 || (search_first1 >= 0 && search_first0 > search_first1))
	{
		if (search_first1 + 8 >= read_end)
			return -1;
		memcpy(&m_frame_size, &m_buffer[search_first1 + 4], 4);
		m_frame_size += 8;
		search_first0 = search_first1; // get pkt_size
	}
	else
	{
		if (search_first0 + 16 >= read_end)
			return -1;
		memcpy(&m_frame_size, &m_buffer[search_first0 + 12], 4);
		m_frame_size += 16;
	}
	m_frame_start_pos = search_first0; // get pkt_size

 	if (m_frame_size <= 10 || m_frame_size > gs_video_OneFrame_MaxSize)
 	{
		read_end = 0;
 		return -1;
 	}

	return m_frame_size;
}

int GSFrameBuffer::getFrameBuffer(void* buf)
{
	int read_pos;
	if (findSize() < 0)
		return -EINVAL;

	read_pos = m_frame_start_pos + m_frame_size;

	if (read_pos > read_end)
	{
		return -ERANGE;
	}

	memcpy(buf, &m_buffer[m_frame_start_pos], m_frame_size);
	read_end -= read_pos;
	memmove(m_buffer, &m_buffer[read_pos], read_end);

	return m_frame_size;
}

