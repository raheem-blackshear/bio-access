// Header file for errors
// $Id: SBError.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_ERROR_H__
#define __SB_ERROR_H__

#include "SBTypes.h"

#ifdef S_CPP
extern "C"
{
#endif
#ifndef S_NO_ERROR_CODES

#ifndef S_OK
#define		S_OK					0
#endif
#define S_E_FAILED					-1
#define S_E_CORE					-2
#define S_E_ABANDONED_MUTEX			-25
#define S_E_ARGUMENT				-10
#define S_E_ARGUMENT_NULL			-11
#define S_E_ARGUMENT_OUT_OF_RANGE	-12
#define S_E_INVALID_ENUM_ARGUMENT	-16
#define S_E_ARITHMETIC				-17
#define S_E_OVERFLOW				-8
#define S_E_BAD_IMAGE_FORMAT		-26
#define S_E_DLL_NOT_FOUND			-27
#define S_E_ENTRY_POINT_NOT_FOUND	-28
#define S_E_FORMAT					-13
#define S_E_FILE_FORMAT				-29
#define S_E_INDEX_OUT_OF_RANGE		-9
#define S_E_INVALID_CAST			-18
#define S_E_INVALID_OPERATION		-7
#define S_E_IO						-14
#define S_E_DIRECTORY_NOT_FOUND		-19
#define S_E_DRIVE_NOT_FOUND			-20
#define S_E_END_OF_STREAM			-15
#define S_E_FILE_NOT_FOUND			-21
#define S_E_FILE_LOAD				-22
#define S_E_PATH_TOO_LONG			-23
#define S_E_NOT_IMPLEMENTED			-5
#define S_E_NOT_SUPPORTED			-6
#define S_E_NULL_REFERENCE			-3
#define S_E_OUT_OF_MEMORY			-4
#define S_E_SECURITY				-24
#define S_E_TIMEOUT					-30

#define S_E_EXTERNAL				-90
#define S_E_CLR						-93
#define S_E_COM						-92
#define S_E_CPP						-96
#define S_E_JVM						-97
#define S_E_MAC						-95
#define S_E_SYS						-94
#define S_E_WIN32					-91

#define S_E_PARAMETER				-100
#define S_E_PARAMETER_READ_ONLY		-101

#define S_E_NOT_ACTIVATED			-200

/* Face extraction error codes */
#define SFE_OUT_OF_MEMORY			-60
#define SFE_GRAY_NOT_CREATED		-61
#define SFE_FACE_NOT_DETECTED		-62
#define SFE_EYES_NOT_DETECTED		-63
#define SFE_DETAILS_NOT_DETECTED	-64
#define SFE_FEATURES_NOT_EXTRACTED	-65
#define SFE_TEMPLATE_NOT_EXTRACTED	-66
#define SFE_IDENTIFICATION_FAILED	-67

#endif // !S_NO_ERROR_CODES

#define SBFailed(result) ((result) < 0)
#define SBSucceeded(result) ((result) >= 0)

SResult SBErrorReset();
void	SBErrorAppend(const SAChar * szFunction, const SChar * szFile, SInt line);
SResult SBErrorSetLast(SResult code, const SChar* szMessage, 
					   const SChar* szParam, SInt externalError,
					   const SChar* szExternalCallStack, SBool preserveInnerError);

#ifdef S_CPP
}
#endif

#endif // !__SB_ERROR_H__