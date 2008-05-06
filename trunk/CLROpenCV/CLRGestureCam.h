#pragma once

#include "CLROpenCV.h"
//#include "CLROpticalFlow.h"

namespace CLROpenCVs {





	//public ref class CLRGestureCam
	//{
	//public:

	//	IplImage* _OriginaImage;
	//	CvCapture * _Input_video;
	//	IplImage* _CompareImage;
	//	IplImage* _TemporaryImage;
	//	IplImage* _TemporaryImage2;
	//	
	//	IplImage* _DetectImage;

	//	CvSize*  _size;
	//	


	//	int _intervalTick;
	//	int _curTick;

	//	double _cur_frame;
	//	double _total_frame;
	//	
	//	CLRGestureDetect()
	//	{
	//		_curTick = 0;
	//		_cur_frame	= 0;

	//		_intervalTick = 0;


	//	};

	//	~CLRGestureDetect()
	//	{
	//	}

	//	void SetIntervalTick(int intervalTick)
	//	{
	//		_intervalTick = intervalTick;
	//	}
	//	
	//	// ������ ���Ϻ��� ����
	//	bool SelectCam(int index)
	//	{

	//		_Input_video = cvCaptureFromCAM(0);
	//		
	//		if(!_Input_video)
	//			return false;

	//		_size = new CvSize;
	//		_size->height =
	//			(int) cvGetCaptureProperty( _Input_video, CV_CAP_PROP_FRAME_HEIGHT );
	//		_size->width =
	//			(int) cvGetCaptureProperty( _Input_video, CV_CAP_PROP_FRAME_WIDTH );

	//		_total_frame = cvGetCaptureProperty( _Input_video, CV_CAP_PROP_FRAME_COUNT );

	//		pin_ptr<IplImage*> p = &_CompareImage;
	//		allocateOnDemand(p, *_size, IPL_DEPTH_8U, 1 );
	//		
	//		p = &_OriginaImage;
	//		allocateOnDemand(p, *_size, IPL_DEPTH_8U, 1 );



	//		p = &_TemporaryImage;
	//		allocateOnDemand(p, *_size, IPL_DEPTH_8U, 1 );
	//		p = &_TemporaryImage2;
	//		allocateOnDemand(p, *_size, IPL_DEPTH_8U, 1 );

	//		
	//		p = &_DetectImage;
	//		allocateOnDemand(p, *_size, IPL_DEPTH_8U, 1 );




	//		cvSetCaptureProperty( _Input_video,	CV_CAP_PROP_POS_FRAMES, 0 );

	//		IplImage*frame = cvQueryFrame(_Input_video);

	//		cvConvertImage(frame,_OriginaImage,CV_BGR2GRAY);
	//		


	//		_OpticalFlow->Init(frame,_size);



	//		return _OriginaImage;

	//	}


	//	// ������ ������ ���� ������ ����
	//	IplImage* NextFrame(double frame)
	//	{

	//		if( _total_frame < frame)
	//			return NULL;

	//		cvSetCaptureProperty( _Input_video,	CV_CAP_PROP_POS_FRAMES, frame );

	//		cvConvertImage(cvQueryFrame(_Input_video),_TemporaryImage,CV_BGR2GRAY);
	//		return _TemporaryImage;
	//	}


	//	IplImage * SetOriginalImage(IplImage * pImage)
	//	{
	//		IplImage * temp = _OriginaImage;
	//		_OriginaImage = pImage;


	//		return temp;		
	//	}
	//	
	//	
	//	IplImage* GetOpticalFlowImage()
	//	{
	//		return _OpticalFlow->_frame2_1C;
	//	}

	//	// �Էµ� �������� 
	//	virtual IplImage* NextGestureInfo(double frame)
	//	{
	//		//_TemporaryImage = NextFrame(frame);


	//		
	//		if( _total_frame <= frame)
	//			return NULL;


	//		cvSetCaptureProperty( _Input_video,	CV_CAP_PROP_POS_FRAMES, frame );
	//		IplImage * curFrame = cvQueryFrame(_Input_video);

	//		cvConvertImage(curFrame,_TemporaryImage,CV_BGR2GRAY);

	//		
	//		_OpticalFlow->Process(curFrame);

	//		
	//		// �������Ӱ� ������������ ���� ���Ѵ�
	//		cvAbsDiff(_TemporaryImage,_OriginaImage,_TemporaryImage2);
	//		
	//		// ����
	//		//swap(_OriginaImage,_TemporaryImage);
	//		_TemporaryImage = SetOriginalImage(_TemporaryImage);

	//		cvThreshold(_TemporaryImage2,_TemporaryImage,10,10,CV_THRESH_BINARY);
	//		
	//		if( _intervalTick != 0  && (_curTick + _intervalTick) < System::Environment::TickCount)
	//		{
	//			_curTick = System::Environment::TickCount;
	//			
	//			ClearImage(_DetectImage,0);
	//			//cvThreshold(_DetectImage,_DetectImage,0,0,CV_THRESH_BINARY);
	//		}

	//		cvAdd(_DetectImage,_TemporaryImage,_DetectImage);
	//		
	//		cvSubS( _DetectImage, cvScalarAll(1), _DetectImage);


	//		return _TemporaryImage;		
	//	}
	//	
	//	IplImage* GesDetectedInfo()
	//	{
	//		return _DetectImage; 
	//	}

	//	

	//};
}