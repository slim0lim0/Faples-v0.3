#pragma once

#include "Scenery.hpp"
#include "Obj.hpp"
#include "Tile.hpp"
#include "UIControl.hpp"
#include <FaplesFpx/node.hpp>
#include <vector>

namespace fpl
{
	class UIGroup;

	class UILayer
	{
	public:
		UILayer(node n, UIGroup& g);
		~UILayer();
		void Process();
		void Render();
		
		std::vector<Scenery*> oScenery;
		std::vector<Obj*> oObjects;
		std::vector<Tile*> oTiles;
		std::vector<UIControl*> oControls;

		int z = 0;
		UIGroup& group;
	private:
		double scrollX = 0;
		double scrollY = 0;
	};
}