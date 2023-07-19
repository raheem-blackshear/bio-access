#ifndef _GS_FRAMEBUFFER_HEADER_
#define _GS_FRAMEBUFFER_HEADER_

class GSFrameBuffer
{
public:
	GSFrameBuffer();
	~GSFrameBuffer();
	void	init(int buf_size, int size_opset = 58);
	BOOL	append(void* buf, int size);
	int		getFrameSize();
	int		getFrameBuffer(void* buf);
	BYTE*	m_buffer;
	int		read_end;	// current memory size
private:
	int		findSize();
	
	BYTE*	m_pIdentifier;
	BYTE	m_nIdentifier_size;
	BYTE	m_size_opset;
	int		m_max_memory_size;

	int		m_frame_size;
	int		m_frame_start_pos;

	int		is_frame_continue;
};

#endif