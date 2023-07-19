#define A1 0.2990                                                
#define A2 0.5870                                              
#define A3 0.1140                                                 
#define A4 0.00
#define A5 (-0.2990)
#define A6 (-0.5870)
#define A7 0.8860
#define A8 0.00
#define A9 0.7010
#define A10 (-0.5870)
#define A11 (-0.1140)
#define A12 0.00

#define DET (A1*(A6*A11-A7*A10)-A2*(A5*A11-A7*A9)+A3*(A5*A10-A6*A9))

#define B11 ((A6*A11-A7*A10)/DET)
#define B12 ((A3*A10-A2*A11)/DET)
#define B13 ((A2*A7-A3*A6)/DET)
#define B21 ((A7*A9-A5*A11)/DET)
#define B22 ((A1*A11-A3*A9)/DET)
#define B23 ((A3*A5-A1*A7)/DET)
#define B31 ((A5*A10-A6*A9)/DET)
#define B32 ((A2*A9-A1*A10)/DET)
#define B33 ((A1*A6-A2*A5)/DET)

//INT64 __GetTickCount64()
//{
//	SYSTEMTIME stu_localtime;
//	FILETIME stu_filetime;
//	GetLocalTime(&stu_localtime);
//	SystemTimeToFileTime(&stu_localtime, &stu_filetime);
//
//	INT64 i64_tickvalue = ((INT64)(*(INT64*)(&stu_filetime))) ;/// 10000;
//	return i64_tickvalue;
//	//return GetTickCount64();
//}
