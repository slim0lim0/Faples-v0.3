#include "Map.hpp"
#include "Foothold.hpp"
#include "Time.hpp"
#include "Sound.hpp"

#include <FaplesFpx/fpx.hpp>
#include <SDL.h>
#include <SDL_image.h>

namespace fpl
{
	namespace map
	{
		int LEVEL_WIDTH = 1920;
		int LEVEL_HEIGHT = 1080;

		void init() {
			map_node = fpx::fMap["Maps"];
			if (!map_node) { throw std::runtime_error{ "Theres no maps!" }; }
			load("0");
		}

		void load(std::string id)
		{
			current = map_node.GetNode(map_node.root(), id);
			current_name = current.name();
			//config::map = current_name;
		
			time::reset();
			sound::play();
			//layer::load();
			//background::load();
			//foothold::load();
			//portal::load();
			//player::respawn(next_portal);
			//view::reset();
		}
	}
	

	//node m_Maps;

	//Map::Map()
	//{

	//}

	//Map::~Map()
	//{

	//}

	//void Map::LoadLevel(std::string sLevel, SDL_Renderer& oRenderer)
	//{
	//	m_Maps = fpx::fMap.root();

	//	auto wnode = m_Maps.GetNode(m_Maps, "Maps/0/Layers/-8/Scenery/background").get_string();
	//	auto img = m_Maps.GetNode(m_Maps, "SpriteSheets/" + wnode).get_bitmap();
	//	
	//	levelWidth = 2400;
	//	levelHeight = 1100;

	//	Sprite* bg = new Sprite();
	//	bg->LoadTextureFromBitmap(wnode, img.data(), img.width(), img.height());

	//	bg->SetSprite(0, 0, img.width(), img.height());
	//	Background* e = new Background();
	//	e->Initialize(*bg);
	//	e->SetParallaxMultipliers(1, 1);

	//	m_backgrounds.push_back(e);

	//	auto wnode2 = m_Maps.GetNode(m_Maps, "Maps/0/Layers/-6/Scenery/background").get_string();
	//	img = m_Maps.GetNode(m_Maps, "SpriteSheets/" + wnode2).get_bitmap();

	//	Sprite* bg2 = new Sprite();

	//	bg2->LoadTextureFromBitmap(wnode2, img.data(), img.width(), img.height());
	//	bg2->SetSprite(0, 0, img.width(), img.height());
	//	Background* e2 = new Background();
	//	e2->Initialize(*bg2);
	//	e2->SetParallaxMultipliers(1, 1);
	//	m_backgrounds.push_back(e2);

	//	Foothold::load();
	//}

	//void Map::SendPlayerPosition(float playerX, float playerY)
	//{
	//	m_playerX = playerX;
	//	m_playerY = playerY;
	//}

	//void Map::SendCamera(float camX, float camY)
	//{
	//	for (int i = 0; i < m_backgrounds.size(); i++)
	//	{
	//		Background& e = *m_backgrounds[i];
	//		e.SetParallaxSettings(camX, camY);
	//	}

	//	Foothold::updateCamera(camX, camY);
	//}

	//void Map::Process(float deltaTime)
	//{
	//	for (int i = 0; i < m_backgrounds.size(); i++)
	//	{
	//		Background& e = *m_backgrounds[i];
	//		e.Process(deltaTime);
	//	}
	//}

	//void Map::Draw(SDL_Renderer & oRenderer)
	//{
	//	for (int i = 0; i < m_backgrounds.size(); i++)
	//	{
	//		Background& e = *m_backgrounds[i];
	//		e.Draw(oRenderer);
	//	}
	//	Foothold::draw_footholds(oRenderer);
	//}

}

