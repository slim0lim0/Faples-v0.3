#include "foothold.hpp"
#include <algorithm>
#include <string>
#include <GL/glew.h>

namespace fpl
{
	std::vector<Foothold> footholds;
	float Foothold::camX = 0.0f;
	float Foothold::camY = 0.0f;
	Foothold::Foothold(int id, int group, int ox1, int oy1, int ox2, int oy2) : id(id), group(group) {
		//x1 = n["x1"];
		//x1 = 100 + (100 * id);
		//x2 = n["x2"];
		//x2 = 100 + (100 * (id+1));
		//y1 = n["y1"];

		//y1 = 800;
		//y1 = 300 + (75 * id);
		//y1 = 1000 - (25 * id);
		//y2 = n["y2"];
		//y2 = 800;
		//y2 = 300 + (75 * (id + 1));
		//y2 = 1000 - (25 * (id + 1));

		x1 = ox1;
		x2 = ox2;
		y1 = oy1;
		y2 = oy2;


		//force = n["force"];
		force = 0;
		//piece = n["piece"];
		piece = 0;
		//nextid = n["next"];
		nextid = id + 1;
		//previd = n["prev"];
		previd = id - 1;
		//cant_through = n["cantThrough"].get_bool();
		cant_through = false;
		//forbid_fall_down = n["forbidFallDown"].get_bool();
		forbid_fall_down = true;
	}
	void Foothold::load() {
		footholds.clear();

		// Create 3 points
		footholds.emplace_back(footholds.size(), 0, 100, 1000, 200, 1000);
		footholds.emplace_back(footholds.size(), 0, 200, 1000, 300, 1000);
		footholds.emplace_back(footholds.size(), 0, 300, 1000, 400, 950);
		footholds.emplace_back(footholds.size(), 0, 400, 950, 500, 900);
		footholds.emplace_back(footholds.size(), 0, 500, 900, 600, 900);
		footholds.emplace_back(footholds.size(), 0, 600, 900, 700, 900);
		footholds.emplace_back(footholds.size(), 0, 700, 900, 800, 950);
		footholds.emplace_back(footholds.size(), 0, 800, 950, 900, 1000);
		footholds.emplace_back(footholds.size(), 0, 900, 1000, 1000, 1000);
		footholds.emplace_back(footholds.size(), 0, 1000, 1000, 1100, 1000);
		footholds.emplace_back(footholds.size(), 0, 1100, 1000, 1100, 500);

		footholds.emplace_back(footholds.size(), 1, 450, 850, 550, 850);
		footholds.emplace_back(footholds.size(), 1, 550, 850, 650, 850);
		footholds.emplace_back(footholds.size(), 1, 650, 850, 750, 900);
		footholds.emplace_back(footholds.size(), 1, 750, 900, 800, 950);

		footholds.emplace_back(footholds.size(), 2, 350, 800, 400, 800);
		footholds.emplace_back(footholds.size(), 2, 400, 800, 450, 800);
		footholds.emplace_back(footholds.size(), 2, 450, 800, 500, 800);


		footholds.emplace_back(footholds.size(), 3, 350, 750, 360, 745);
		footholds.emplace_back(footholds.size(), 3, 360, 745, 370, 745);
		footholds.emplace_back(footholds.size(), 3, 370, 745, 380, 750);
		footholds.emplace_back(footholds.size(), 3, 380, 750, 380, 750);

		//footholds.emplace_back(6, 0, 725, 875, 735, 865);


		std::sort(footholds.begin(), footholds.end(), [](Foothold const & p_1, Foothold const & p_2) { return p_1.id < p_2.id; });


		for (auto & fh : footholds) {
			auto pred = [](Foothold const & p_fh, int p_id) { return p_fh.id < p_id; };
			auto nextit = std::lower_bound(footholds.cbegin(), footholds.cend(), fh.nextid, pred);
			fh.next = nextit != footholds.cend() && nextit->id == fh.nextid ? &*nextit : nullptr;
			auto previt = std::lower_bound(footholds.cbegin(), footholds.cend(), fh.previd, pred);
			fh.prev = previt != footholds.cend() && previt->id == fh.previd ? &*previt : nullptr;

			if (fh.next == NULL)
			{
				fh.x2 = 0;
				fh.y2 = 0;
			}
		}
	}
	void Foothold::draw_footholds(SDL_Renderer & oRenderer)
	{
		SDL_SetRenderDrawColor(&oRenderer, 0xFF, 0x00, 0x90, 0xFF);

		for (auto & fh : footholds) {
			SDL_Rect fillRect;

			fillRect.x = (fh.x1 - 3) - camX;
			fillRect.y = (fh.y1 - 3) - camY;
			fillRect.w = 6;
			fillRect.h = 6;

			SDL_RenderFillRect(&oRenderer, &fillRect);

			if (fh.next != NULL)
				SDL_RenderDrawLine(&oRenderer, fh.x1 - camX, fh.y1 - camY, fh.x2 - camX, fh.y2 - camY);
		}
	}

	void Foothold::updateCamera(float cX, float cY)
	{
		camX = cX;
		camY = cY;
	}

	Foothold * Foothold::getById(int id, int groupid)
	{
		for (auto & fh : footholds) {

			if (fh.id == id && fh.group == groupid)
			{
				return &fh;
			}
		}

		return nullptr;
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
}


