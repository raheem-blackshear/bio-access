// Header file for face detection
// $Id: SBDetectFace.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_DETECT_FACE_H__
#define __SB_DETECT_FACE_H__

#include "SBGeometry.h"
#include "SBImage.h"
#include "SBTypes.h"
#include "SBCore.h"

#ifdef S_CPP
extern "C"
{
#endif

typedef struct _SFaceRotation_
{
	SShort	Yaw;		//0
	SShort	Pitch;	//2
	SShort	Roll;	//4
} SFaceRotation;

typedef struct _SFace_
{
	SRect	Rectangle;	//0
	SFaceRotation Rotation;//10
	SDouble	Confidence;	//18
} SFace;

typedef struct _SEyes_
{
	SPoint	First;			//000
	SDouble FirstConfidence;//008
	SPoint	Second;			//010
	SDouble SecondConfidence;//018
} SEyes;

typedef struct _SFaceDetectionDetails_
{
	SBool	FaceAvailable;//0
	SFace	Face;		//8
	SBool	EyesAvailable;//28
	SEyes	Eyes;		//30
} SFaceDetectionDetails;

typedef void * HSFaceExtractor;
typedef void * HSGrayscaleImage;

//////////////////////////////////////////////////////////////////////////
//	Export APIs
//////////////////////////////////////////////////////////////////////////
S_EXPORT_API SResult SBDetectFaces
(
	HSFaceExtractor hExtractor,
	HSGrayscaleImage hImage,
	SInt* pFaceCount,
	SFace** pArFaces
);
S_EXPORT_API SResult SBDetectFacialFeatures
(
	HSFaceExtractor hExtESP004,
	HSGrayscaleImage hImgESP008,
	const SFace * pFaceESP00C,
	SFaceDetectionDetails * pDetectESP010
);

#ifdef S_CPP
}
#endif


#endif // !__SB_DETECT_FACE_H__