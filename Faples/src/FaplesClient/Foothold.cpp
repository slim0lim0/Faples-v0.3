#include "foothold.hpp"
#include "View.hpp"
#include "Window.hpp"
#include "Map.hpp"

#include <algorithm>
#include <cmath>
#include <string>
#include <GL/glew.h>

namespace fpl
{
	const std::string FOOT = "0";
	const std::string CLIMB = "1";
	const std::string SEAT = "2";

	std::vector<Foothold> footholds;
	Foothold::Foothold(node n, int id, int group, int layer) : id(id), group(group), layer(layer) {
		x1 = n.GetNode(n, "x1").get_integer();
		x2 = n.GetNode(n, "x2").get_integer();
		y1 = n.GetNode(n, "y1").get_integer();
		y2 = n.GetNode(n, "y2").get_integer();
		nextid = n.GetNode(n, "nextid").get_integer();
		previd = n.GetNode(n, "previd").get_integer();
		type = n.GetNode(n, "type");
		force = n.GetNode(n, "force").get_integer();
		cantDrop = n.GetNode(n, "cantDrop").get_bool();
		cantPass = n.GetNode(n, "cantPass").get_bool();
		cantMove = n.GetNode(n, "cantMove").get_bool();
	}

	Foothold::Foothold(node n, int hid, int group, int layer, bool flipped, int flipWidth, int gSize, bool cannotDrop) : id(hid), group(group), layer(layer) {
		x1 = n.GetNode(n, "x1").get_integer();
		x2 = n.GetNode(n, "x2").get_integer();
		y1 = n.GetNode(n, "y1").get_integer();
		y2 = n.GetNode(n, "y2").get_integer();
		nextid = n.GetNode(n, "nextid").get_integer();
		previd = n.GetNode(n, "previd").get_integer();

		if (flipped)
		{
			x1 = flipWidth - x1;
			x2 = flipWidth - x2;
		}

		type = n.GetNode(n, "type");
		force = n.GetNode(n, "force").get_integer();
		cantDrop = cannotDrop;
		cantPass = n.GetNode(n, "cantPass").get_bool();
		cantMove = n.GetNode(n, "cantMove").get_bool();
	}

	void Foothold::Clear()
	{
		footholds.clear();
	}

	void Foothold::AddFoothold(node n, int id, int group, int layer)
	{
		Foothold fh = { n, id, group, layer };

		footholds.emplace_back(fh);
	}

	void Foothold::AddFoothold(node n, int id, int group, int layer, int trueX, int trueY, bool flipped, int flipWidth, int gSize, bool cannotDrop)
	{
		Foothold fh = { n, id, group, layer , flipped, flipWidth, gSize, cannotDrop };

		fh.x1 += trueX;
		fh.y1 += trueY;
		fh.x2 += trueX;
		fh.y2 += trueY;
		

		footholds.emplace_back(fh);
	}

	

	void Foothold::LoadAll()
	{
		int iGroup = -1;

		std::vector<Foothold> alter;
		std::vector<Foothold> finalCol;

		for (auto & fh : footholds) {
			if(fh.group != iGroup)
			{
				if (alter.size() > 0)
				{
					std::sort(alter.begin(), alter.end(), [](Foothold a, Foothold b) {
						return a.x1 < b.x1;
					});

					int iID = 0;

					for (Foothold & fh : alter)
					{
						fh.id = iID;
						
						fh.previd = iID - 1;

						iID++;

						if (iID == alter.size())
							fh.nextid = -1;
						else
							fh.nextid = iID;

						if (fh.nextid != -1)
						{
							fh.x2 = alter[fh.nextid].x1;
							fh.y2 = alter[fh.nextid].y1;
						}
						else
						{
							fh.x2 = 0;
							fh.y2 = 0;
						}
					}
				}

				for (Foothold & fh : alter)
				{
					finalCol.push_back(fh);
				}

				iGroup = fh.group;

				alter.clear();
				alter.push_back(fh);
			}
			else
			{
				alter.push_back(fh);	
			}

			if (&fh == &footholds.back())
			{
				std::sort(alter.begin(), alter.end(), [](Foothold a, Foothold b) {
					return a.x1 < b.x1;
				});

				int iID = 0;

				for (Foothold & fh : alter)
				{
					fh.id = iID;

					fh.previd = iID - 1;

					iID++;

					if (iID == alter.size())
						fh.nextid = -1;
					else
						fh.nextid = iID;

					if (fh.nextid != -1)
					{
						fh.x2 = alter[fh.nextid].x1;
						fh.y2 = alter[fh.nextid].y1;
					}
					else
					{
						fh.x2 = 0;
						fh.y2 = 0;
					}
				}

				for (Foothold & fh : alter)
				{
					finalCol.push_back(fh);
				}

				alter.clear();
			}
		}

		footholds = finalCol;

		for (auto & fh : footholds) {
			fh.next = getById(fh.nextid, fh.group, fh.layer);
			fh.prev = getById(fh.previd, fh.group, fh.layer);
		}

		Reposition();
	}

	void Foothold::Reposition()
	{
		for (auto & fh : footholds) {
			fh.x1 = fh.x1 * window::GetWidthScale();
			fh.y1 = fh.y1 * window::GetHeightScale();
			fh.x2 = fh.x2 * window::GetWidthScale();
			fh.y2 = fh.y2 * window::GetHeightScale();
		}
	}

	Foothold* Foothold::getById(int id, int groupid, int layerid)
	{
		if (id != -1)
		{
			for (auto & fh : footholds) {

				if (fh.id == id && fh.group == groupid && fh.layer == layerid)
				{
					return &fh;
				}
			}
		}

		return nullptr;
	}

	void Foothold::draw_footholds(SDL_Renderer & oRenderer)
	{
		SDL_SetRenderDrawColor(&oRenderer, 0xFF, 0x00, 0x90, 0xFF);

		

		for (auto & fh : footholds) {
			SDL_SetRenderDrawColor(&oRenderer, 0xFF, 0x00, 0x90, 0xFF);
			if (fh.type == FOOT)
			{
				if (map::GetLocalPlayer()->phy->fh != NULL)
				{
					if (fh == *map::GetLocalPlayer()->phy->fh)
						SDL_SetRenderDrawColor(&oRenderer, 0x00, 0xFF, 0xFF, 0xFF);
				}

				if (map::GetLocalPlayer()->phy->nextfh != NULL)
				{
					if (fh == *map::GetLocalPlayer()->phy->nextfh)
						SDL_SetRenderDrawColor(&oRenderer, 0x00, 0xFF, 0x00, 0xFF);
				}

				if (fh.x1 == fh.x2)
				{
					SDL_SetRenderDrawColor(&oRenderer, 0xFF, 0xFF, 0x00, 0xFF);
				}
			}
			else if (fh.type == CLIMB)
			{
				SDL_SetRenderDrawColor(&oRenderer, 0x00, 0x9B, 0x00, 0xFF);

				if (map::GetLocalPlayer()->phy->fh != NULL)
				{
					if (fh == *map::GetLocalPlayer()->phy->fh)
						SDL_SetRenderDrawColor(&oRenderer, 0x00, 0xFF, 0xFF, 0xFF);
				}

				if (map::GetLocalPlayer()->phy->ch != NULL)
				{
					if (fh == *map::GetLocalPlayer()->phy->ch)
						SDL_SetRenderDrawColor(&oRenderer, 0x8B, 0x00, 0x8B, 0xFF);
				}
			}

			SDL_Rect fillRect;

			fillRect.x = (fh.x1 - 3) - View::fx;
			fillRect.y = (fh.y1 - 3) - View::fy;
			fillRect.w = 6;
			fillRect.h = 6;

			SDL_RenderFillRect(&oRenderer, &fillRect);

			if (fh.next != NULL)
				SDL_RenderDrawLine(&oRenderer, fh.x1 - View::fx, fh.y1 - View::fy, fh.x2 - View::fx, fh.y2 - View::fy);
		}
	}

	Foothold* Foothold::collided(int x, int y)
	{
		for (auto & fh : footholds) {

			if (!(x < fh.x1) && !(x > fh.x2))
			{

				float first = (fh.y2 - fh.y1);
				float second = (fh.x2 - fh.x1);

				float m = first / second;
				float b = fh.y1 - (m * fh.x1);

				float total = round((m*x) + b);

				if (y == total)
				{
					return &fh;
				}
			}
		}

		return nullptr;
	}

	bool Foothold::operator==(Foothold const & fh) const
	{
		return id == fh.id && group == fh.group && layer == fh.layer;
	}

	bool Foothold::operator!=(Foothold const &fh) const
	{
		return id != fh.id && group != fh.group && layer != fh.layer;
	}
}


