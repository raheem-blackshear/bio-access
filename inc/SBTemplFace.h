// Header file for face template
// $Id: SBTemplFace.h 2 2011-04-26 00:55:12Z dragon $

#ifndef __SB_TEMPL_FACE_H__
#define __SB_TEMPL_FACE_H__

#include "SBCore.h"

#ifdef S_CPP
extern "C"
{
#endif

typedef enum _STemplateSize_
{
	stsSmall = 0,
	stsCompact = 1,
	stsMedium = 64,
	stsLarge = 128,
} STemplateSize;

typedef HSObject HSTemplate;

//////////////////////////////////////////////////////////////////////////
// Export APIs
//////////////////////////////////////////////////////////////////////////
S_EXPORT_API SResult SBUnpackTemplate
(
	const void * pBuffer, SSizeType bufferSize,
	SUShort * pVersion, SUInt * pSize, SSByte * pHeaderSize,
	const void * * ppFingersTemplate, SSizeType * pFingersTemplateSize,
	const void * * ppFacesTemplate, SSizeType * pFacesTemplateSize,
	const void * * ppIrisesTemplate, SSizeType * pIrisesTemplateSize,
	const void * * ppPalmsTemplate, SSizeType * pPalmsTemplateSize
);
S_EXPORT_API SResult SBCreateFaceTemplate
(
	HSTemplate* pHTempESP004
);
S_EXPORT_API SResult SBCreateFaceTemplateEx
(
	SUInt flags, HSTemplate*  pHTemplate
);
S_EXPORT_API SResult SBGetFaceTemplateSize
(
	HSTemplate hTemplate, SUInt flags, SSizeType* pSize
);
S_EXPORT_API SResult SBSaveFaceTemplateToMemory
(
	HSTemplate hTemplate, void* pBuffer, SSizeType bufferSize,
	SUInt flags, SSizeType * pSize
);
S_EXPORT_API SResult SBCalculateFaceTemplateSize
(
	SInt recordCount, SSizeType* arRecordSizes, SSizeType* pSize
);
S_EXPORT_API SResult SBPackFaceTemplate
(
	SInt recordCount, const void** arPRecords, SSizeType* arRecordSizes,
	void* pBuffer, SSizeType bufferSize, SSizeType * pSize
);
S_EXPORT_API SResult SBUnpackFaceTemplate
(
	const void * pBuffer, SSizeType bufferSize,
	SUShort * pVersion, SUInt * pSize, SSByte * pHeaderSize,
	SInt * pRecordCount, const void * * arPRecords,
	SSizeType * arRecordSizes
);


#ifdef S_CPP
}
#endif

#endif // !__SB_TEMPL_FACE_H__