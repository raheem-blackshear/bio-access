// Header file for face extraction
// $Id: SBExtractFace.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_FACE_EXTRACT_H__
#define __SB_FACE_EXTRACT_H__

#include "SBCore.h"
#include "SBDetectFace.h"
#include "SBTemplFace.h"

#ifdef S_CPP
extern "C"
{
#endif

typedef enum _SFaceExtractionStatus_
{
	sfeNone = 0,
	sfeTemplateCreated = 1,
	sfeFaceNotDetected = 2,
	sfeEyesNotDetected = 3,
	sfeFaceTooCloseToImageBorder = 4,
	sfeQualityCheckGrayscaleDensityFailed = 100,
	sfeQualityCheckExposureFailed = 101,
	sfeQualityCheckSharpnessFailed = 102,
	sfeLivenessCheckFailed = 200,
	sfeGeneralizationFailed = 300,
} SFaceExtractionStatus;

typedef HSObject HSFaceExtractor;

//////////////////////////////////////////////////////////////////////////
// Export APIs
//////////////////////////////////////////////////////////////////////////
S_EXPORT_API SResult SBCreateFaceExtractor
(
	HSFaceExtractor* pHExtractor
);

S_EXPORT_API SResult SBExtractForEnroll(
	void* pvBits, 
	const SInt width,
	const SInt height,
	SInt* fx, SInt* fy, SInt* fw, SInt* fh, SInt* roll,
	SInt* ex1, SInt* ey1,
	SInt* ex2, SInt* ey2,
	void** templData,
	SInt* templSize
);

S_EXPORT_API SResult SBDetectFacesFromRawImage(
	void* pvBits, 
	const SInt width,
	const SInt height,
	SInt* faceCount,
	SInt** fx, SInt** fy, SInt** fw, SInt** fh, SInt** roll,
	SInt** ex1, SInt** ey1, SInt** ex2, SInt** ey2
	);

S_EXPORT_API SResult SBExtractFace(
	SInt* faceCount,
	void*** templData,
	SInt** templSize
	);

S_EXPORT_API void SBFreeTemplate
(
	void* templData
);

S_EXPORT_API SResult SBExtractUsingDetails
(
	HSFaceExtractor hExtractor,
	HSGrayscaleImage hImage,
	SFaceDetectionDetails* pDetectionDetails,
	SFaceExtractionStatus* pStatus,
	HSTemplate* pHTemplate
);

#ifdef S_CPP
}
#endif


#endif // !__SB_FACE_EXTRACT_H__