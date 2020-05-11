#pragma once

#ifndef __MAP_H_
#define __MAP_H_

#include "Player.hpp"
#include "Portal.hpp"

#include <vector>

#include <FaplesFpx/node.hpp>

namespace fpl
{
	namespace map
	{
		extern node current;
		extern std::string current_name;
		extern int mwidth, mheight;

		void Load(std::string name);
		void init();
		void Update();
		void Render();
		void Teleport(Portal* p);
		void SpawnPlayer(Portal* ptarget);
		void Clear();


		void GetWorldPlayers();
		Player* GetLocalPlayer();

		extern Player* playerLoc;
		extern std::vector<Portal*> portals;
	}
}
#endif