//------------------------------------------------------------------------------
//  NAME : ArrayList.h
//  DESC : Array형 객체를 만든다.
// VER  : v1.0 
// PROJ : GS Project Net System 
// Copyright 2014  KPT  All rights reserved
//------------------------------------------------------------------------------
//                  변         경         사         항                       
//------------------------------------------------------------------------------
//    DATE     AUTHOR                      DESCRIPTION                        
// ----------  ------  --------------------------------------------------------- 
// 2014.11.17  KM  최초 프로그람 작성                                     
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
	int m_nArraySize;//현재배렬 Buffer의 크기
	int m_nLastIndex;//현재 배렬의 마지막첨수번호
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

