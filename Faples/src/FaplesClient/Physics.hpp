#pragma once
#include "Foothold.hpp"
#include <cmath>

namespace fpl
{
	class Physics
	{
	public:
		Physics();
		~Physics();

		void Jump();
		void Update();

		void Reset(int nx, int ny, int nwidth, int nheight);
		vector2i GetPosition();

	
		
		int GetWalkSpeed(int entitySpeed);
		int GetFallSpeed();

		bool LEFT = false, RIGHT = false, UP = false, DOWN = false;

		bool FLYING = false;

		int layer, group;
		Foothold *fh, *djump, *nextfh, *ch;
		int fallTimeOut;
		double fallDuration;
	private:
		void CalculateJump();
		void CalculateWalk(double delta);
		void CalculateFall(double delta);
		void CalculateClimb(double delta);
		void CalculateNextFoothold();

		void ClearFoothold(bool refreshTimeout);
		void SetFoothold(Foothold* newfh);

		double laststep;
		double x, y, px, py, tx, ty, r, width, height;
		double vx, vy, vr;
		int miny, minxLeft, minxRight, nextx;
	};
}