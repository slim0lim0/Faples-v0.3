#pragma once
#include "Sprite.hpp"
#include <FaplesFpx/node.hpp>
namespace fpl
{
	class Tile
	{
	public:
		Tile(node n);
		~Tile();
		void Update();
		void Render();
		void Reposition();
		void SetLocation(int x, int y);
	private:
		Sprite* m_pSprite;
		int x, y, width, height;
		bool flipped;
	};
}

