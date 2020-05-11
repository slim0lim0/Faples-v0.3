#pragma once
#include "Scenery.hpp"
#include "Obj.hpp"
#include "Tile.hpp"

#include <vector>
#include <array>


namespace fpl
{
	class Layer
	{
	public:
		static void Load();
		static void Clear();
		static void Update();
		static void Render();
		std::vector<Scenery*> oScenery;
		std::vector<Obj*> oObjects;
		std::vector<Tile*> oTiles;
		int z = 0;
	private:
	};
	extern std::array<Layer, 16> layers;
}