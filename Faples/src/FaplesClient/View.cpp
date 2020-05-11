#include "View.hpp"
#include "Map.hpp"
#include "Time.hpp"
#include "Session.hpp"

#include <algorithm>
#include <random>
#include <chrono>
#include <cmath>
#include <iostream>

namespace fpl
{
	namespace View
	{
		double const tau{ 6.28318530717958647692528676655900576839433879875021 };
		int x{ 0 }, y{ 0 };
		int width{ 0 }, height{ 0 };
		int fx{ 0 }, fy{ 0 };
		double shiftx{ 0 }, shifty{ 0 };
		float vx{ 0 }, vy{ 0 };
		double rx{ 0 }, ry{ 0 };
		int left{ 0 }, right{ 0 }, top{ 0 }, bottom{ 0 };
		int leftin{ 0 }, rightin{ 0 }, topin{ 0 }, bottomin{ 0 };
		int cleft{ 0 }, cright{ 0 }, ctop{ 0 }, cbottom{ 0 };
		bool doside{ false }, dotop{ false }, dobottom{ false };
		int xmin{ 0 }, xmax{ 0 }, ymin{ 0 }, ymax{ 0 };
		double tx{ 0 }, ty{ 0 };
		float r{ 1 }, g{ 1 }, b{ 1 };
		float laststep{ 0 };
		int bottomoffset{ 250 };
		bool innerScroll{ false };

		template <typename T>
		void restrict(T & p_x, T & p_y){
			p_x = std::max<T>(std::min<T>(p_x, rightin), leftin);
			p_y = std::max<T>(std::min<T>(p_y, bottomin), topin);
		}
			
		void Resize(int p_width, int p_height) {
			width = p_width;
			height = p_height;
		}
		void Reset() {
			auto mapn = map::current;

			top = mapn.GetNode(mapn, "boundT").get_integer();
			bottom = map::mheight - mapn.GetNode(mapn, "boundB").get_integer();
			left = mapn.GetNode(mapn, "boundL").get_integer();
			right = map::mwidth - mapn.GetNode(mapn, "boundR").get_integer();

			tx = map::GetLocalPlayer()->m_x;
			ty = map::GetLocalPlayer()->m_y;
			shiftx = tx;
			shifty = ty;
			Update();
		}

		void Update() {
			tx = map::GetLocalPlayer()->m_x;
			ty = map::GetLocalPlayer()->m_y;
			
			if (fx > (tx - width / 2) + 100)
			{
				vx = -(fx - (tx - width / 2));
			}				
			else if (fx < (tx - width / 2) - 100)
			{
				vx = -(fx - (tx - width / 2));
			}

			shiftx += vx * time::delta;

			if (vx < 0 && shiftx < (tx-width/2))
			{
				shiftx = tx - (width / 2);
				vx = 0;
			}
			if (vx > 0 && shiftx > tx - (width / 2))
			{
				shiftx = tx - (width / 2);
				vx = 0;
			}



			if (fy > (ty - height / 2) + 50)
			{
				vy = -(fy - (ty - height / 2));
			}
			else if (fy < (ty - height / 2) - 50)
			{
				vy = -(fy - (ty - height / 2));
			}

			shifty += vy* time::delta;

			if (vy < 0 && shifty < (ty - height / 2))
			{
				shifty = ty - (height / 2);
				vy = 0;
			}

			if (vy > 0 && shifty > ty - (height / 2))
			{
				shifty = ty - (height / 2);
				vy = 0;
			}


			if (shiftx < left)
				shiftx = left;
			if (shifty < top)
				shifty = top;
			if (shiftx > (right - width))
				shiftx = right - width;
			if (shifty > (bottom - height))
				shifty = bottom - height;
			
			fx = shiftx;
			fy = shifty;
		}
	}
}