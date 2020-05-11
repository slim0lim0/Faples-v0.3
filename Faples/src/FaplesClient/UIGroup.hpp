#pragma once

#include "UILayer.hpp"
#include <FaplesFpx/node.hpp>
#include <vector>

namespace fpl
{
	class UIGroup
	{
	public:
		UIGroup(node n);
		~UIGroup();
		void Process();
		void Render();
		std::vector<UILayer*> GetLayers();
		void SetLocation(int x, int y);
		void Reposition();

		std::string name;
		int x, y, width, height;
	private:
		bool bStatic;
		std::vector<UILayer*> layers;
	};
}