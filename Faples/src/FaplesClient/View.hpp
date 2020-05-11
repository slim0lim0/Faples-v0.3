#pragma once

namespace fpl
{
	namespace View
	{
		extern int x, y;
		extern double tx, ty;
		extern int fx, fy;
		extern double shiftx, shifty;
		extern int width, height;
		extern int left, right, top, bottom;
		extern int xmin, xmax, ymin, ymax;
		extern float vx, vy;
		extern float r, g, b;
		void Resize(int w, int h);
		void Reset();
		void Update();
	}
}