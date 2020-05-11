#pragma once
#include <vector>
#include <SDL.h>

namespace fpl
{
	class Foothold {
	public:
		Foothold(int, int, int, int, int, int);
		static void load();
		static void draw_footholds(SDL_Renderer & oRenderer);
		Foothold const *next, *prev;
		int x1, y1, x2, y2;
		int force, piece;
		int nextid, previd;
		int id, group, layer;
		bool cant_through, forbid_fall_down;
		static float camX, camY;
		static void updateCamera(float cX, float cY);

		static Foothold* getById(int id, int groupid);
		static Foothold* collided(int x, int y);
	};
	extern std::vector<Foothold> footholds;
}

