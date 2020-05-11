#pragma once

#ifndef __PLAYER_H__
#define __PLAYER_H__

#include "Sprite.hpp"
#include "Foothold.hpp"
#include "Physics.hpp"
#include "Character.hpp"

namespace fpl
{
	class Player
	{
	public:
		bool MOVE_LEFT, MOVE_RIGHT, MOVE_UP, MOVE_DOWN, FALLING, JUMPING = false;

		Player();
		~Player();

		void Update();
		void Render();
		void SetLayer(int layer);
		void Respawn(int x, int y);
		void Reposition();
		void SetPosition(int x, int y);

		int m_x, m_y, m_width, m_height;
		int z = 0;
		float m_velX, m_velY, m_accX, m_accY;
		Foothold* m_fh;
		Physics* phy;
	private:
		int iSpeed, iPatk, iMatk, iPdef, iMdef;
		Character* character;
	};
}

#endif
