#pragma once
#include <FaplesFpx/node.hpp>
#include "Sprite.hpp"
#include <vector>
#include <cmath>

namespace fpl
{
	class Portal
	{
	public:
		Portal(node n);
		~Portal();
		void Update();
		void Render();
		void Reposition();
		vector2i GetPosition();
		int id, x, y, width, height, mapid, regionid, targetid;
		std::string type;
	private:

		Sprite* m_pSprite;
	};
}