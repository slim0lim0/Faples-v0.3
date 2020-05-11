#pragma once
#include <vector>
#include <SDL.h>
#include <FaplesFpx/node.hpp>

namespace fpl
{
	class Foothold {
	public:
		Foothold(node, int, int, int);
		Foothold(node, int, int, int, bool flipped, int flipWidth, int gSize, bool cannotDrop);
		static void Clear();
		static void AddFoothold(node, int, int, int);
		static void AddFoothold(node, int, int, int, int trueX, int trueY, bool flipped, int flipWidth, int gSize, bool cannotDrop);
		static void LoadAll();
		static void Reposition();
		static void draw_footholds(SDL_Renderer & oRenderer);
		Foothold const *next, *prev;
		int x1, y1, x2, y2;
		int force, piece;
		int nextid, previd;
		int id, group, layer;
		std::string type;
		bool cantDrop, cantPass, cantMove;

		static Foothold* getById(int id, int groupid, int layerid);
		static Foothold* collided(int x, int y);

		bool operator==(Foothold const &) const;
		bool operator!=(Foothold const &) const;
	};
	extern std::vector<Foothold> footholds;
}

