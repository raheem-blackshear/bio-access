// MyMutex

#pragma once

#define GS_Lock_kind_Long 1
#define GS_Lock_kind_CSection 2

class CMyMutex 
{
public:
	CMyMutex(const int nKind = 1);
	~CMyMutex();

	void SetLockKine(int nKind);
	void Lock();
	void UnLock();

protected:
	CRITICAL_SECTION	m_CriticalSection;
	LONG	m_nLock;
	int m_nLockKind;
};