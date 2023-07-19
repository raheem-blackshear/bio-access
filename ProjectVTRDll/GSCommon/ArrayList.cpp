//------------------------------------------------------------------------------
//  NAME : ArrayList.cpp
//  DESC : Array�� ��ü�� �����.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  ��         ��         ��         ��                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.11.17  KM  ���� ���α׶� �ۼ�                                     
//----------------------------------------------------------------------------

#include "stdafx.h"

#include "ArrayList.h"
#include "GSNetPacket.h"

CArrayList::CArrayList(void)
{
	m_nArraySize = 0;
	m_nLastIndex = 0;
	m_nArrayType = 0;

	m_lpList = NULL;

	m_Lock.SetLockKine(GS_Lock_kind_Long);
}


CArrayList::~CArrayList(void)
{
	RemoveAll();
}

void CArrayList::SetType(int nType)
{
	m_nArrayType = nType;
}

void CArrayList::Add(void* lpData, BOOL isDelete)
{
	if (m_nArraySize <= m_nLastIndex)
	{
		m_nArraySize += Add_Buffer_size;
		array_list_type* m_lpListTmp = new array_list_type[m_nArraySize];		
		memset(m_lpListTmp, 0, sizeof(array_list_type) * m_nArraySize);

		for (int i=0; i<m_nLastIndex; i++)
			m_lpListTmp[i] = m_lpList[i];

		array_list_type* tmp = m_lpList;
		Lock();
		m_lpList = m_lpListTmp;
		UnLock();

		if (tmp)
			delete []tmp;
	}

	Lock();
	m_lpList[m_nLastIndex].isDelete = isDelete;
	m_lpList[m_nLastIndex].lpData = lpData;
	UnLock();
	m_nLastIndex++;
}

//n��° �ڷḦ �����ش�. �ڷᰡ ������ NULL �� ������.
//�ڷḦ ������ �ܺο��� Lock�� �����ؾ� �Ѵ�.
void* CArrayList::Get(int n)
{
	if (m_nLastIndex > n && n>=0)
		return m_lpList[n].lpData;
	else
		return NULL;
}

//header ����
void CArrayList::Remove(int n)
{
	if (m_nLastIndex <= n)
		return;

	Lock();

	if (m_lpList[n].isDelete)
	{
		if (m_nArrayType == GS_Array_removeType_P2)
			delete ((P::P2*)m_lpList[n].lpData)->lpData;

		delete m_lpList[n].lpData;
	}

	for (int i=n; i<m_nLastIndex-1; i++)
		m_lpList[i] = m_lpList[i+1];
	
	m_nLastIndex--;
	ASSERT(m_nLastIndex >= 0);

	m_lpList[m_nLastIndex].isDelete = FALSE;
	m_lpList[m_nLastIndex].lpData = NULL;

	UnLock();
}

void CArrayList::RemoveAll()
{
	while (m_nLastIndex > 0)
		Remove(m_nLastIndex - 1);

	delete []m_lpList;
	
	m_lpList = NULL;
	m_nArraySize = 0;
	m_nLastIndex = 0;
}

int	CArrayList::Count()
{
	return m_nLastIndex;
}

void CArrayList::Lock()
{
	m_Lock.Lock();
}

void CArrayList::UnLock()
{
	m_Lock.UnLock();
}