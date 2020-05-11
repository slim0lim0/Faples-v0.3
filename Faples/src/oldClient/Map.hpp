#pragma once

#ifndef __MAP_H_
#define __MAP_H_

#include "Background.hpp"
#include "Foothold.hpp"

#include <vector>

#include <FaplesFpx/node.hpp>

namespace fpl
{
	namespace map
	{
		std::vector<std::string> all_maps;
		node map_node;
		node current;
		std::string current_name;

		void load(std::string name);
		void init();
		void update();
		void render();
	}



	//struct Layer
	//{
	//	
	//};

	//class Map
	//{
	//public:
	//	Map();
	//	~Map();
	//	void LoadLevel(std::string sLevel, SDL_Renderer& oRenderer);
	//	int inline GetLevelWidth() { return levelWidth; };
	//	int inline GetLevelHeight() { return levelHeight; };

	//	void SendPlayerPosition(float playerX, float playerY);
	//	void SendCamera(float camX, float camY);

	//	void Process(float deltaTime);
	//	void Draw(SDL_Renderer& oRenderer);
	//private:
	//	std::vector<Background*> m_backgrounds;
	//	int levelWidth = 1920;
	//	int levelHeight = 1080;
	//	int cameraWidth = 0;
	//	int cameraHeight = 0;

	//	float m_playerX = 0;
	//	float m_playerY = 0;
	//};
}
#endif