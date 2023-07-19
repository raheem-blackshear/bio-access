// Header file for geometry
// $Id: SBGeometry.h 2 2011-04-26 00:55:12Z dragon $

#ifndef S_GEOMETRY_H_INCLUDED
#define S_GEOMETRY_H_INCLUDED

#include "../inc/SBTypes.h"

#ifdef S_CPP
extern "C"
{
#endif

typedef struct _SPoint_
{
	SInt X;
	SInt Y;
} SPoint;

#ifndef S_NO_FLOAT

typedef struct _SPointF_
{
	SFloat X;
	SFloat Y;
} SPointF;

typedef struct _SPointD_
{
	SDouble X;
	SDouble Y;
} SPointD;

#endif

typedef struct _SSize_
{
	SInt Width;
	SInt Height;
} SSize;

#ifndef S_NO_FLOAT

typedef struct _SSizeF_
{
	SFloat Width;
	SFloat Height;
} SSizeF;

typedef struct _SSizeD_
{
	SDouble Width;
	SDouble Height;
} SSizeD;

#endif

typedef struct _SRect_
{
	SInt X;
	SInt Y;
	SInt Width;
	SInt Height;
} SRect;

typedef struct _SRectUS_
{
	SUShort X;
	SUShort Y;
	SUShort Width;
	SUShort Height;
} SRectUS;

#ifndef S_NO_FLOAT

typedef struct _SRectF_
{
	SFloat X;
	SFloat Y;
	SFloat Width;
	SFloat Height;
} SRectF;

typedef struct _SRectD_
{
	SDouble X;
	SDouble Y;
	SDouble Width;
	SDouble Height;
} SRectD;

#endif

typedef struct _SPolygon_
{
	SPointF p[4];
	SRectUS box;
} SPolygon;

#ifdef S_CPP
}
#endif

#endif // !S_GEOMETRY_H_INCLUDED
