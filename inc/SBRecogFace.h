// Header file for face recognition(matching)
// $Id: SBRecogFace.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_RECOG_FACE_H__
#define __SB_RECOG_FACE_H__

#ifdef S_CPP
extern "C"
{
#endif

typedef void * HSMatcher;
typedef void * HSMatchingDetails;

S_EXPORT_API SResult SBCreateMatcher
(
	HSMatcher* pHMatcher
);
S_EXPORT_API SResult SBIdentifyStartEx
(
	HSMatcher hMatcher, const void * pTemplate, 
	SSizeType templateSize, HSMatchingDetails* pHMatchingDetails
);
S_EXPORT_API SResult SBIdentifyNextEx
(
	HSMatcher hMatcher, const void * pTemplate, SSizeType templateSize, 
	HSMatchingDetails hMatchingDetails, SInt * pScore
);
S_EXPORT_API SResult SBIdentifyEnd
(
	HSMatcher hMatcher
);

#ifdef S_CPP
}
#endif

#endif // !__SB_RECOG_FACE_H__