#include "layer.hpp"
#include "map.hpp"
#include "Foothold.hpp"
#include "Window.hpp"
#include "Player.hpp"
#include <FaplesFpx/fpx.hpp>
#include <FaplesFpx/node.hpp>
#include <algorithm>


namespace fpl {
	std::array<Layer, 16> layers;

	void Layer::Load()
	{
		auto allLayers = map::current.GetNode(map::current, "Layers");

		Foothold::Clear();

		int counter = 0;

		int fhgCount = 0;

		for (auto it = allLayers.begin(); it != allLayers.end(); it++)
		{
			layers[counter].z = counter;


			auto scenery = it.GetNode(it, "Scenery");
			auto obj = it.GetNode(it, "Objects");
			auto tile = it.GetNode(it, "Tiles");
			auto hold = it.GetNode(it, "Holds");

			std::vector<node> holds;

			layers[counter].oScenery.emplace_back(new Scenery(scenery));


			for (auto itob = obj.begin(); itob != obj.end(); itob++)
			{
				layers[counter].oObjects.emplace_back(new Obj(itob));

				std::string name = itob.GetNode(itob, "Name");
				std::string sprsheet = itob.GetNode(itob, "SpriteSheet");

				auto root = fpx::fMap.root();
				auto obj = root.GetNode(root, "Objects/" + sprsheet + "/" + name);

				auto objHolds = obj.GetNode(obj, "Holds");

				for (int i = 0; i < objHolds.size(); i++)
				{
				    std::string check = objHolds.name();
					std::string id = "g" + std::to_string(i);
					auto hGroup = objHolds.GetNode(objHolds, id);

					for (auto ithg = hGroup.begin(); ithg != hGroup.end(); ithg++)
					{
						auto hid = std::stoi(ithg.name());
						auto gid = fhgCount;
						auto lid = std::stoi(it.name()) + 8;

						auto trueX = itob.GetNode(itob, "mapX").get_integer();
						auto trueY = itob.GetNode(itob, "mapY").get_integer();

						auto flipped = itob.GetNode(itob, "flipped").get_bool();
						auto flipWidth = obj.GetNode(obj, "width").get_integer();

						auto cannotDrop = itob.GetNode(itob, "cannotDrop").get_bool();

						Foothold::AddFoothold(ithg, hid, gid, lid, trueX, trueY, flipped, flipWidth, hGroup.size(), cannotDrop);
					}

					fhgCount++;
				}
			}

			for (auto itti = tile.begin(); itti != tile.end(); itti++)
			{
				layers[counter].oTiles.emplace_back(new Tile(itti));

				std::string name = itti.GetNode(itti, "Name");
				std::string sprsheet = itti.GetNode(itti, "SpriteSheet");

				auto root = fpx::fMap.root();
				auto tile = root.GetNode(root, "Tiles/" + sprsheet + "/" + name);

				auto tileHolds = tile.GetNode(tile, "Holds");

				for (int i = 0; i < tileHolds.size(); i++)
				{
					std::string check = tileHolds.name();
					std::string id = "g" + std::to_string(i);
					auto hGroup = tileHolds.GetNode(tileHolds, id);

					for (auto ithg = hGroup.begin(); ithg != hGroup.end(); ithg++)
					{
						auto hid = std::stoi(ithg.name());
						auto gid = fhgCount;
						auto lid = std::stoi(it.name()) + 8;

						auto trueX = itti.GetNode(itti, "mapX").get_integer();
						auto trueY = itti.GetNode(itti, "mapY").get_integer();

						auto flipped = itti.GetNode(itti, "flipped").get_bool();
						auto flipWidth = tile.GetNode(tile, "width").get_integer();

						auto cannotDrop = itti.GetNode(itti, "cannotDrop").get_bool();

						Foothold::AddFoothold(ithg, hid, gid, lid, trueX, trueY, flipped, flipWidth, hGroup.size(), cannotDrop);
					}

					fhgCount++;
				}
			}

			for (auto ithg = hold.begin(); ithg != hold.end(); ithg++)
			{
					for (auto ith = ithg.begin(); ith != ithg.end(); ith++)
					{
						auto id = ith.name();
						auto hid = std::stoi(id);
						auto gid = fhgCount;
						auto lid = std::stoi(it.name()) + 8;

 						Foothold::AddFoothold(ith, hid, gid, lid);
					}
				fhgCount++;
			}

			counter++;
		}	

		Foothold::LoadAll();
	}

	void Layer::Clear()
	{
		for (int i = 0; i < layers.size(); i++)
		{
			for (int io = 0; io < layers[i].oObjects.size(); io++)
			{
				delete layers[i].oObjects[io];
				layers[i].oObjects[io] = nullptr;
			}
			layers[i].oObjects.clear();

			for (int it = 0; it < layers[i].oTiles.size(); it++)
			{
				delete layers[i].oTiles[it];
				layers[i].oTiles[it] = nullptr;
			}
			layers[i].oTiles.clear();

			for (int is = 0; is < layers[i].oScenery.size(); is++)
			{
				delete layers[i].oScenery[is];
				layers[i].oScenery[is] = nullptr;
			}
			layers[i].oScenery.clear();
		}
	}

	void Layer::Update()
	{
		for (Layer layer : layers)
		{
			for (Scenery* sc : layer.oScenery)
			{
				sc->Update();
			}

			for (Obj* ob : layer.oObjects)
			{
				ob->Update();
			}

			for (Tile* tile : layer.oTiles)
			{
				tile->Update();
			}
		}
	}

	void Layer::Render()
	{
		for (Layer layer : layers)
		{
			for (Scenery* sc : layer.oScenery)
			{
				sc->Render();
			}

			for (Obj* ob : layer.oObjects)
			{
				ob->Render();
			}

			for (Tile* tile : layer.oTiles)
			{
				tile->Render();
			}

			if (map::GetLocalPlayer()->z == layer.z)
			{
				map::GetLocalPlayer()->Render();
			}
		}

		Foothold::draw_footholds(*window::gRenderer);
	}
}