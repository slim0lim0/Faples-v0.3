#pragma once
#include "Sprite.hpp"
#include <FaplesFpx/node.hpp>

namespace fpl
{
	class Scenery
	{
	public:
		Scenery(node n);
		~Scenery();
		void Update();
		void Render();
		void SetLocation(int x, int y);
		void Reposition();
	private:
		Sprite* m_pSprite;
		int id, width, height;
		float parX, parY, scrX, scrY;
		bool tileX, tileY, flipped;
		int x, y;
		int iTileXCount, iTileYCount;
	};
}