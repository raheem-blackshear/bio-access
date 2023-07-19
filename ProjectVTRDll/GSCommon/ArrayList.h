//------------------------------------------------------------------------------
//  NAME : ArrayList.h
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

#pragma once

#include "GSLockMng.h"

#define GS_Array_removeType_P2 1

struct array_list_type
{
	BOOL isDelete;
	void* lpData;
};

class CArrayList
{
public:
	CArrayList(void);
	~CArrayList(void);

private:
	static const int Add_Buffer_size = 150;
	CMyMutex m_Lock;
	
	int m_nArrayType;
	int m_nArraySize;//������ Buffer�� ũ��
	int m_nLastIndex;//���� ����� ������÷����ȣ
	array_list_type* m_lpList;

public:
	void    SetType(int nType);
	void	Add(void* lpData, BOOL isDelete = TRUE);
	void*	Get(int n);
	void	Remove(int n);
	void	RemoveAll();
	int		Count();
	void	Lock();
	void    UnLock();
};

