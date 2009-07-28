#pragma once

#include "NxPhysics.h"
//#include "DebugRenderer.h"
#include "DrawObjects.h"
#include "Timing.h"
#include "Actors.h"
#include "Joints.h"
//#include "Utilities.h"
#include "Timing.h"
//#include "ErrorStream.h"
//#include "SamplePulleyJoint.h"
// Limit globals



extern NxScene*          gScene;

class Creature
{
public:
	Creature(void);
	~Creature(void);


  NxActor* _capsuleList[5] ;
  void Init()
  {
	
		int z = 15;	
		NxD6Joint* d6Joint;

	  for(int i = 0;i< 5;i++)
	  {
			_capsuleList[ i]  = CreateBox(NxVec3(0,z,0),NxVec3(0.5,1,0.5),20);

			//_capsuleList[ i]  = CreateCapsule(NxVec3(0,z,0), 1, 0.5, 10);
			

			if( i != 0)
			{
				d6Joint = CreateD6Joint(_capsuleList[ i-1], _capsuleList[ i], NxVec3(0,z+2,0), NxVec3(0,-1,0));
			}
			z -= 2;
	  }
	}

	void Update()
	{
		for(int i =0;i< 5;i++)
		{
			int x = rand() % 50;
			int y = rand() % 5;
			int z = rand() % 50;


			NxVec3 forceDir = NxVec3(x,0,z);
			//_capsuleList[ i]->addLocalForce(forceDir);
			//_capsuleList[ i]->addLocalTorque(forceDir);
			_capsuleList[ i]->addTorque(forceDir);
		}
	}
  
	NxD6Joint* CreateD6Joint(NxActor* a0, NxActor* a1, const NxVec3& globalAnchor, const NxVec3& globalAxis);

};
