#include "stdafx.h"
#include "GSLockMng.h"
#include "GSUtilityCls.h"

//Mutex
CMyMutex::CMyMutex(int nKind)
{
	m_nLockKind = nKind;
	m_nLock = 0;
	InitializeCriticalSection(&m_CriticalSection);
}

CMyMutex::~CMyMutex()
{
	m_nLockKind = 0;
	m_nLock = 0;

	LeaveCriticalSection(&m_CriticalSection);
	DeleteCriticalSection(&m_CriticalSection);
}

void CMyMutex::SetLockKine(int nKind)
{
	m_nLockKind = nKind;
}
static int nLockCounts = 0;
void CMyMutex::Lock()
{
	if (m_nLockKind == GS_Lock_kind_Long)
	{
		nLockCounts = 0;
		while (m_nLock != 0)
		{
			Sleep(1);
			nLockCounts ++;
			if (nLockCounts > 3000)
				CUtilityCls::GetInstance()->AddLogText(GS_NET_LOG_LOCK_STATUS, L"The Lock is not release.!!!!!");
		}
		InterlockedIncrement(&m_nLock);
	} else if (m_nLockKind == GS_Lock_kind_Long)
	{
		EnterCriticalSection(&m_CriticalSection);
	}
}

void CMyMutex::UnLock()
{
	if (m_nLockKind == 0)
		return;

	if (m_nLockKind == GS_Lock_kind_Long)
	{
		InterlockedDecrement(&m_nLock);
	} else if (m_nLockKind == GS_Lock_kind_Long)
	{
		LeaveCriticalSection(&m_CriticalSection);
	}
}
