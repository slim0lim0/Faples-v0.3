#include "Map.hpp"
#include "Time.hpp"
#include "Sound.hpp"
#include "Layer.hpp"
#include "Portal.hpp"
#include "Utility.hpp"
#include "View.hpp"

#include <FaplesFpx/fpx.hpp>
#include <SDL.h>
#include <SDL_image.h>

#include <algorithm>
#include <iostream>
#include <random>
#include <vector>

namespace fpl
{
	namespace map
	{
		int mwidth = 1920;
		int mheight = 1080;
		std::vector<std::string> all_maps;
		node map_node;
		node current;
		std::string current_name;
		Player* playerLoc;
		std::vector<Portal*> portals;

		void init() {

			auto root = fpx::fMap.root();

			map_node = root.GetNode(root, "Maps");
			if (!map_node) { throw std::runtime_error{ "Theres no maps!" }; }
			//Load("0100");
			//SpawnPlayer(nullptr);
		}

		void Load(std::string id)
		{
			Clear();


			std::string sregid = id.substr(0, 2);
			int regid = std::stoi(sregid);
			sregid = std::to_string(regid);

			std::string smapid = id.substr(2, id.size() - 2);
			int mapid = std::stoi(smapid);
			smapid = std::to_string(mapid);

			std::string trueid = "" + sregid + "/" + smapid;

			current = map_node.GetNode(map_node, trueid);
			current_name = current.name();
		
			mwidth = current.GetNode(current, "width");
			mheight = current.GetNode(current, "height");


			Layer::Load();

			auto ports = current.GetNode(map::current, "Portals");

			for (auto it = ports.begin(); it != ports.end(); it++)
			{
				portals.emplace_back(new Portal(it));
			}

			time::reset();
			sound::play();
			window::recreate(false);

			GetLocalPlayer();
			View::Reset();
		}

		void Update()
		{
			Layer::Update();
			playerLoc->Update();

			if ((playerLoc->m_y + playerLoc->m_height)+1 >= View::bottom)
			{
				SpawnPlayer(nullptr);
			}
		}

		void Render()
		{
			Layer::Render();
			for(Portal* p : portals)
			{
				p->Render();
			}
		}
		void Teleport(Portal* p)
		{
			std::string moveto = std::to_string(p->regionid);

			while (moveto.size() < 2)
				moveto = "0" + moveto;

			moveto += std::to_string(p->mapid);

			Load(moveto);
			SpawnPlayer(portals[p->targetid]);
		}
		void SpawnPlayer(Portal* ptarget)
		{
			if (ptarget != nullptr)
			{
				playerLoc->Respawn(ptarget->x, ptarget->y - ptarget->height);
			}
			else
			{
				std::vector<Portal*> filtered_items;

				for (auto * portal : portals) {
					if (portal->type == "1" || portal->type == "2") {
						filtered_items.emplace_back(portal);
					}
				}

				if (filtered_items.size() != 0)
				{
					int randomIndex = rand() % filtered_items.size();
					Portal* p = filtered_items[randomIndex];

					if (p->type == "1")
					{
						playerLoc->Respawn(p->x, p->y - p->height);
					}
					else if (p->type == "2")
					{
						Teleport(p);
					}
				}
			}
			View::Reset();
		}
		void Clear()
		{
			Layer::Clear();
			portals.clear();
			delete playerLoc;
			playerLoc = nullptr;
		}

		void GetWorldPlayers()
		{
		}
		
		Player* GetLocalPlayer()
		{
			if (playerLoc == nullptr)
				playerLoc = new Player();

			return playerLoc;
		}
	}
}

