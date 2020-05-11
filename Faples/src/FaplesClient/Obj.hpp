#pragma once
#include "Sprite.hpp"
#include <FaplesFpx/node.hpp>

namespace fpl
{
	class Obj
	{
	public:
		Obj(node n);
		~Obj();
		void Update();
		void Render();
		void SetLocation(int x, int y);
		void Reposition();
	private:
		Sprite* m_pSprite;
		int x, y, width, height;
		bool animated, flipped;
		int areelh, areeli, aframew, aframet;
		float aframesp;
	};
}