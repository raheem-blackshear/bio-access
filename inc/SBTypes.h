// Header file for general types
// $Id: SBTypes.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_TYPES_H__
#define __SB_TYPES_H__

#ifdef S_DOCUMENTATION
	#define S_64
	#define S_ANSI_C
	#define S_BIG_ENDIAN
	#define S_CPP
	#define S_DEBUG
	#define S_FAST_FLOAT
	#define S_GCC
	#define S_LIB
	#define S_LINUX
	#define S_MAC
	#define S_MSVC
	#define S_NO_ANSI_FUNC
	#define S_NO_INT_64
	#define S_UNICODE
	#define S_WINDOWS
#endif

#ifdef __cplusplus
	#define S_CPP
#endif

#ifdef S_CPP
extern "C"
{
#endif

#ifdef _DEBUG
	#define S_DEBUG
#endif

#ifdef _LIB
	#define S_LIB
#endif

#if defined(S_LIB) && defined(S_EXE)
	#error S_LIB and S_EXE defined simultaneously
#endif

#if defined(_WIN32) || defined(WIN32) || defined(_WIN64) || defined(WIN64)
	#define S_WINDOWS
#endif

#ifdef WINCE
	#define S_WINDOWS_CE
#endif

#ifdef __linux__
	#define S_LINUX
#endif

#ifdef __APPLE__
	#define S_MAC
#endif

#ifdef _AIX
	#define S_AIX
#endif

#if defined(_UNICODE) || defined(UNICODE)
	#define S_UNICODE
#endif


#ifdef _MSC_VER
	#if (_MSC_VER >= 1300)
		#define S_MSVC
	#else
		#define S_NO_INT_64
	#endif
#endif

#ifdef __GNUC__
	#define S_GCC
	#define S_GCC_VERSION (__GNUC__ * 10000 + __GNUC_MINOR__ * 100 + __GNUC_PATCHLEVEL__)
#endif

#if defined(__STDC__) && !defined(S_GCC)
	#define S_ANSI_C
#endif

#if defined(S_MSVC)
	#define S_DEPRECATED(message) __declspec(deprecated(message))
	#define S_NO_RETURN __declspec(noreturn)
	#define S_PACKED
#elif defined(S_GCC)
	#define S_DEPRECATED(message) __attribute__ ((deprecated))
	#define S_NO_RETURN __attribute__ ((noreturn))
	#define S_PACKED __attribute__((__packed__))
#else
	#define S_DEPRECATED(message)
	#define S_NO_RETURN
	#define S_PACKED
#endif

#ifdef S_CPP
	#if defined(S_MSVC)
		#define S_NO_THROW __declspec(nothrow)
	#elif defined(S_GCC)
		#define S_NO_THROW __attribute__ ((nothrow))
	#else
		#define S_NO_THROW
	#endif
#else
	#define S_NO_THROW
#endif

#if defined(_M_IX86) || defined(__i386__)
	#define S_X86
#endif

#if defined(_M_X64) || defined(__x86_64__)
	#define S_X64
#endif

#if defined(S_X86) || defined(S_X64)
	#define S_X86_FAMILY
#endif

#if defined(__POWERPC__) || defined(_POWER) || defined(_ARCH_PPC)
	#define S_POWER_PC
#endif

#if defined(S_POWER_PC)
	#define S_POWER_PC_FAMILY
#endif

#if defined(S_POWER_PC_FAMILY)
	#define S_BIG_ENDIAN
#endif

#if defined(S_X64)
	#define S_64
#endif

#if defined(S_64) && defined(S_NO_INT_64)
	#error S_64 and S_NO_INT_64 defined simultaneously
#endif

#if defined(S_X86_FAMILY) || defined(S_POWER_PC_FAMILY)
	#define S_FAST_FLOAT
#endif

#ifdef S_MSVC
	typedef unsigned __int8  SUInt8;
	typedef signed   __int8  SInt8;
	typedef unsigned __int16 SUInt16;
	typedef signed   __int16 SInt16;
	typedef unsigned __int32 SUInt32;
	typedef signed   __int32 SInt32;

	#define S_UINT8_MIN 0x00ui8
	#define S_UINT8_MAX 0xFFui8
	#define S_INT8_MIN 0x80i8
	#define S_INT8_MAX 0x7Fi8
	#define S_UINT16_MIN 0x0000ui16
	#define S_UINT16_MAX 0xFFFFui16
	#define S_INT16_MIN 0x8000i16
	#define S_INT16_MAX 0x7FFFi16
	#define S_UINT32_MIN 0x00000000ui32
	#define S_UINT32_MAX 0xFFFFFFFFui32
	#define S_INT32_MIN 0x80000000i32
	#define S_INT32_MAX 0x7FFFFFFFi32
#else
	typedef unsigned char  SUInt8;
	typedef signed   char  SInt8;
	typedef unsigned short SUInt16;
	typedef signed   short SInt16;
	typedef unsigned int   SUInt32;
	typedef signed   int   SInt32;

	#define S_UINT8_MIN ((SUInt8)0x00u)
	#define S_UINT8_MAX ((SUInt8)0xFFu)
	#define S_INT8_MIN ((SInt8)0x80)
	#define S_INT8_MAX ((SInt8)0x7F)
	#define S_UINT16_MIN ((SUInt16)0x0000u)
	#define S_UINT16_MAX ((SUInt16)0xFFFFu)
	#define S_INT16_MIN ((SInt16)0x8000)
	#define S_INT16_MAX ((SInt16)0x7FFF)
	#define S_UINT32_MIN 0x00000000u
	#define S_UINT32_MAX 0xFFFFFFFFu
	#define S_INT32_MIN 0x80000000
	#define S_INT32_MAX 0x7FFFFFFF
#endif

#ifndef S_NO_INT_64
	#ifdef S_MSVC
		typedef unsigned __int64 SUInt64;
		typedef signed   __int64 SInt64;

		#define S_UINT64_MIN 0x0000000000000000ui64
		#define S_UINT64_MAX 0xFFFFFFFFFFFFFFFFui64
		#define S_INT64_MIN 0x8000000000000000i64
		#define S_INT64_MAX 0x7FFFFFFFFFFFFFFFi64
	#else
		typedef unsigned long long SUInt64;
		typedef signed   long long SInt64;

		#define S_UINT64_MIN 0x0000000000000000ull
		#define S_UINT64_MAX 0xFFFFFFFFFFFFFFFFull
		#define S_INT64_MIN 0x8000000000000000ll
		#define S_INT64_MAX 0x7FFFFFFFFFFFFFFFll
	#endif
#endif


typedef SUInt8 SUByte;
typedef SInt8 SSByte;
typedef SUInt16 SUShort;
typedef SInt16 SShort;
typedef SUInt32 SUInt;
typedef SInt32 SInt;

#ifndef S_NO_INT_64
	typedef SUInt64 SULong;
	typedef SInt64 SLong;
#endif

#define S_BYTE_MIN S_UINT8_MIN
#define S_BYTE_MAX S_UINT8_MAX
#define S_SBYTE_MIN S_INT8_MIN
#define S_SBYTE_MAX S_INT8_MAX
#define S_USHORT_MIN S_UINT16_MIN
#define S_USHORT_MAX S_UINT16_MAX
#define S_SHORT_MIN S_INT16_MIN
#define S_SHORT_MAX S_INT16_MAX
#define S_UINT_MIN S_UINT32_MIN
#define S_UINT_MAX S_UINT32_MAX
#define S_INT_MIN S_INT32_MIN
#define S_INT_MAX S_INT32_MAX

#ifndef S_NO_INT_64
	#define S_ULONG_MIN S_UINT64_MIN
	#define S_ULONG_MAX S_UINT64_MAX
	#define S_LONG_MIN S_INT64_MIN
	#define S_LONG_MAX S_INT64_MAX
#endif

#ifndef S_NO_FLOAT
	typedef float SSingle;
	typedef double SDouble;

	#define S_SINGLE_MIN 1.175494351e-38F
	#define S_SINGLE_MAX 3.402823466e+38F
	#define S_SINGLE_EPSILON 1.192092896e-07F
	#define S_DOUBLE_MIN 2.2250738585072014e-308
	#define S_DOUBLE_MAX 1.7976931348623158e+308
	#define S_DOUBLE_EPSILON 2.2204460492503131e-016

	typedef SSingle SFloat;

	#define S_FLOAT_MIN S_SINGLE_MIN
	#define S_FLOAT_MAX S_SINGLE_MAX
	#define S_FLOAT_EPSILON S_SINGLE_EPSILON
#endif

typedef int SBoolean;

#define STrue 1
#define SFalse 0

typedef SBoolean SBool;


#if defined(S_NO_UNICODE) && defined(S_UNICODE)
	#error "S_NO_UNICODE and S_UNICODE defined simultaneously"
#endif

typedef char SAChar;

#ifndef S_NO_UNICODE
	#if defined(S_WINDOWS) || (defined(__SIZEOF_WCHAR_T__) && __SIZEOF_WCHAR_T__ == 2)
		#define S_WCHAR_SIZE 2

		#ifndef _WCHAR_T_DEFINED
			typedef SUShort SWChar;
		#endif
	#else // !defined(S_WINDOWS) && (!defined(__SIZEOF_WCHAR_T__) || __SIZEOF_WCHAR_T__ != 2)
		#define S_WCHAR_SIZE 4

		#if !defined(_WCHAR_T_DEFINED) && !defined(_WCHAR_T)
			#ifdef S_CPP
				typedef wchar_t SWChar;
			#else
				#ifdef __WCHAR_TYPE__
					typedef __WCHAR_TYPE__ SWChar;
				#else
					typedef int SWChar;
				#endif
			#endif
		#endif
	#endif // !defined(S_WINDOWS) && (!defined(__SIZEOF_WCHAR_T__) || __SIZEOF_WCHAR_T__ != 2)

	#if defined(_WCHAR_T_DEFINED) || defined(_WCHAR_T)
		typedef wchar_t SWChar;
	#endif
#endif // !S_NO_UNICODE

#define S_JOIS_SYMBOLS_(name1, name2) name1##name2
#define S_JOIS_SYMBOLS(name1, name2) S_JOIS_SYMBOLS_(name1, name2)

#define S_JOIS_SYMBOLS2_(name1, name2) name1##name2
#define S_JOIS_SYMBOLS2(name1, name2) S_JOIS_SYMBOLS2_(name1, name2)

#ifndef S_NO_UNICODE
	#ifdef S_NO_ANSI_FUNC
		#define S_DECL_AW(name) S_JOIS_SYMBOLS2(name, W)
	#else // !S_NO_ANSI_FUNC
		#define S_DECL_AW(name) S_JOIS_SYMBOLS2(name, A), S_JOIS_SYMBOLS2(name, W)
	#endif // !S_NO_ANSI_FUNC
#else // S_NO_UNICODE
	#define S_DECL_AW(name) S_JOIS_SYMBOLS2(name, A)
#endif // S_NO_UNICODE

#define S_SYMBOL_AW(name) S_JOIS_SYMBOLS2(name, S_AW)

#define S_MACRO_AW(name) S_SYMBOL_AW(name)
#define S_FUNC_AW(name) S_SYMBOL_AW(name)
#define S_CALLBACK_AW(name) S_SYMBOL_AW(name)
#define S_STRUCT_AW(name) S_SYMBOL_AW(name)
#define S_VAR_AW(name) S_SYMBOL_AW(name)
#define S_FIELD_AW(name) S_DECL_AW(name)
#define S_ARG_AW(name) S_DECL_AW(name)
#define S_NULL_AW S_DECL_AW(S_NULL_ARG)

#define S_NULL_ARGA NULL
#define S_EMPTY_STRINGA ""
#define S_T_A(text) text
#define S_TA(text) S_T_A(text)
#define S_STRINGIZE_A(value) S_TA(#value)
#define S_STRINGIZEA(value) S_STRINGIZE_A(value)

#ifndef S_NO_UNICODE
#define S_NULL_ARGW NULL
#define S_EMPTY_STRINGW L""
#define S_T_W(text) L##text
#define S_TW(text) S_T_W(text)
#define S_STRINGIZE_W(value) S_TW(#value)
#define S_STRINGIZEW(value) S_STRINGIZE_W(value)
#endif

#ifdef S_UNICODE
	typedef SWChar SChar;
	#define S_AW W
	#define S_T_ S_T_W
	#define S_T S_TW
	#define S_EMPTY_STRING S_EMPTY_STRINGW
	#define S_STRINGIZE_ S_STRINGIZE_W
	#define S_STRINGIZE S_STRINGIZEW
#else
	typedef SAChar SChar;
	#define S_AW A
	#define S_T_ S_T_A
	#define S_T S_TA
	#define S_EMPTY_STRING S_EMPTY_STRINGA
	#define S_STRINGIZE_ S_STRINGIZE_A
	#define S_STRINGIZE S_STRINGIZEA
#endif

#ifdef S_WINDOWS_CE
	#define S_NO_ANSI_FUNC
	#ifndef S_UNICODE
		#error "S_UNICODE must be defined under Windows CE"
	#endif
#endif

#if defined(S_NO_ANSI_FUNC) && !defined(S_UNICODE)
	#error "S_NO_ANSI_FUNC defined when S_UNICODE is not defined"
#endif

#ifdef S_64
	typedef SUInt64 SSizeType;

	#define S_SIZE_TYPE_MIN S_UINT64_MIN
	#define S_SIZE_TYPE_MAX S_UINT64_MAX
#else
	#ifdef S_MSVC
		typedef __w64 SUInt32 SSizeType;
	#else
		typedef SUInt32 SSizeType;
	#endif

	#define S_SIZE_TYPE_MIN S_UINT32_MIN
	#define S_SIZE_TYPE_MAX S_UINT32_MAX
#endif

#ifndef S_NO_INT_64
	typedef SInt64 SPosType;

	#define S_POS_TYPE_MIN S_INT64_MIN
	#define S_POS_TYPE_MAX S_INT64_MAX
#else
	#ifdef S_MSVC
		typedef __w64 SInt32 SPosType;
	#else
		typedef SInt32 SPosType;
	#endif

	#define S_POS_TYPE_MIN S_INT32_MIN
	#define S_POS_TYPE_MAX S_INT32_MAX
#endif

#define S_PTR_SIZE sizeof(void *)

#ifndef NULL
	#define NULL 0
#endif

typedef SInt SResult;

#ifndef S_CALL_CONV
	#ifdef S_WINDOWS
		#define S_CALL_CONV __stdcall
	#else
		#define S_CALL_CONV
	#endif
#endif

#define S_API S_NO_THROW S_CALL_CONV
#define S_EXPORT_API __declspec(dllexport)
#ifdef S_MSVC
	#define S_API_PTR_RET S_CALL_CONV
#else
	#define S_API_PTR_RET S_NO_THROW S_CALL_CONV
#endif
#define S_CALLBACK S_CALL_CONV *

#ifdef S_MSVC
#define S_UNREFERENCED_PARAMETER(parameter) (parameter)
#else
#define S_UNREFERENCED_PARAMETER(parameter)
#endif

typedef void * NHandle;

#define S_DECLARE_HANDLE(name) typedef struct name##_ { int unused; } * name;

typedef struct _SURational_
{
	SUInt Numerator;
	SUInt Denominator;
} SURational;

typedef struct _SRational_
{
	SInt Numerator;
	SInt Denominator;
} SRational;

typedef struct _SIndexPair_
{
	SInt Index1;
	SInt Index2;
} SIndexPair;

typedef struct _SGuid_
{
	SUInt Data1;
	SUShort Data2;
	SUShort Data3;
	SUByte Data4[8];
} SGuid;

typedef struct _SRange_
{
	SInt From;
	SInt To;
} SRange;

#ifndef S_NO_UNICODE
	SSizeType SBWCharGetSize(void);
#endif

#define SBVersionMake(major, minor) ((SUShort)(((SByte)(minor)) | ((major) << 8)))
#define SBVersionGetMajor(value) ((SByte)((value) >> 8))
#define SBVersionGetMinor(value) ((SByte)((value) & 0xFF))

#define SBVersionRangeMake(from, to) ((SUInt)(((SUShort)(to)) | ((from) << 16)))
#define SBVersionRangeGetFrom(value) ((SUShort)((value) >> 16))
#define SBVersionRangeGetTo(value) ((SUShort)((value) & 0xFFFF))
#define SBVersionRangeContains(value, version) ((version) >= SBVersionRangeGetFrom(value) && (version) <= SBVersionRangeGetTo(value))
#define SBVersionRangeContainsRange(value, versionRange) (SBVersionRangeGetFrom(versionRange) >= SBVersionRangeGetFrom(value) && SBVersionRangeGetTo(versionRange) <= SBVersionRangeGetTo(value))
#define SBVersionRangeIntersectsWith(value, otherValue) (SBVersionRangeGetFrom(otherValue) <= SBVersionRangeGetTo(value) && SBVersionRangeGetFrom(value) <= SBVersionRangeGetTo(otherValue))
SUInt S_API SBVersionRangeIntersect(SUInt value1, SUInt value2);

#ifdef S_CPP
}
#endif

#endif // !__SB_TYPES_H__
