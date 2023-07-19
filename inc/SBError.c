// Implementation file for errors
// $Id: SBError.c 2 2011-04-26 00:55:12Z dragon $

#include <windows.h>
#include "SBError.h"

SResult SBErrorReset()
{
	return 0;
}

void SBErrorAppend(const SAChar * szFunction, const SChar * szFile, SInt line)
{
//	DebugBreak();
}

SResult SBErrorSetLast(SResult code, const SChar* szMessage, const SChar* szParam, SInt externalError, 
						const SChar* szExternalCallStack, SBool preserveInnerError)
{
	DebugBreak();
	return 0;
}
