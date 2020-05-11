#pragma once

#ifndef __PLAYER_H__
#define __PLAYER_H__

#include "Entity.hpp"

namespace fpl
{
	class Player : public Entity
	{
	public:
		bool MOVE_LEFT, MOVE_RIGHT, FALLING, JUMPING, CAN_JUMP = false;

		Player();
		~Player();

		void SendCamera(float camX, float camY);
		void Process(float deltaTime);
	private:
	};
}

#endif
