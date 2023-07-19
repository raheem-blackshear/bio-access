// Header file for image manipulation
// $Id: SBImage.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_IMAGE_H__
#define __SB_IMAGE_H__

#include "SBGeometry.h"
#include "SBTypes.h"
#include "SBCore.h"

#ifdef S_CPP
extern "C"
{
#endif

typedef enum _SPixelFormat_
{
	spfMonochrome = 0x00001001,
	spfGrayscale  = 0x00301001,
	spfRgb        = 0x00303003
} SPixelFormat;

typedef HSObject HSGrayscaleImage;

//////////////////////////////////////////////////////////////////////////
// Export APIs
//////////////////////////////////////////////////////////////////////////
S_EXPORT_API SResult SBLoadGrayImage
(
	const char* filename,
	HSGrayscaleImage* pHImage
);
S_EXPORT_API SResult SBLoadGrayImageFromBits
	(
	const void* pvBits,
	const int width,
	const int height,
	HSGrayscaleImage* pHImage
	);
S_EXPORT_API SResult SBImageCreateWrapperForImagePartEx2
(
	HSGrayscaleImage hSrcImage,
	SUInt left, SUInt top, SUInt width, SUInt height,
	SUInt flags,
	HSGrayscaleImage* pHImage
);
S_EXPORT_API SResult SBImageClone
(
	HSGrayscaleImage hImage,
	HSGrayscaleImage* pHClonedImage
);
S_EXPORT_API SResult SBImageGetWidth
(
	HSGrayscaleImage hImage,
	SUInt* pValue
);
S_EXPORT_API SResult SBImageGetHeight
(
	HSGrayscaleImage hImage,
	SUInt* pValue
);
S_EXPORT_API SBool	SBPixelFormatIsValid
(
	SPixelFormat value
);
S_EXPORT_API SResult SBImageCreateEx
(
	SPixelFormat pixelFormat,
	SUInt width, SUInt height, SSizeType stride,
	SUInt flags,
	HSGrayscaleImage* pHImage
);
S_EXPORT_API SResult SBImageGetStride
(
	HSGrayscaleImage hImage,
	SSizeType* pValue
);
S_EXPORT_API SResult SBImageGetPixels
(
	HSGrayscaleImage hImage,
	void** ppValue
);


#ifdef S_CPP
}
#endif

#endif // !__SB_IMAGE_H__