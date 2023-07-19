// Header file for core
// $Id: SBCore.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_CORE_H__
#define __SB_CORE_H__

#include "SBTypes.h"

#ifdef S_CPP
extern "C"
{
#endif

typedef void * HSObject;

//////////////////////////////////////////////////////////////////////////
//	Export APIs
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//	Export APIs
//////////////////////////////////////////////////////////////////////////
S_EXPORT_API void SBFree
(
	void* pvESP004
);
S_EXPORT_API void SBAlignedFree
(
	void* pBlock
);
S_EXPORT_API SResult SBAlloc
(
	SUInt unESP004,
	void** ppvESP008
);
S_EXPORT_API SResult SBCAlloc
(
	SUInt unESP004,
	void** ppvESP008
);
S_EXPORT_API void SBObjectFree
(
	void* obj
);

S_EXPORT_API void SBDotNet
(
	int nDotNet
);

#ifdef S_CPP
}
#endif

#endif // !__SB_CORE_H__