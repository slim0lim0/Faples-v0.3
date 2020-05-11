#pragma once

#ifndef __ENTITY_H__
#define __ENTITY_H__

#include "Sprite.hpp"
#include "Foothold.hpp"

namespace fpl
{
	class Entity
	{

		// Member Methods
	public:
		Entity();
		~Entity();

		void Initialize(Sprite& sprite);

		void SetLevelBounds(float levelWidth, float levelHeight);

		virtual void Process(float deltaTime);
		virtual void Draw(SDL_Renderer& oRenderer);

		inline float GetPositionX() { return m_x; };
		inline float GetPositionY() { return m_y; };
		inline void SetPositionX(float x) { m_x = x; };
		inline void SetPositionY(float y) { m_y = y; };
	private:
		Entity(const Entity& oEntity);
		Entity& operator=(const Entity& oEntity);


	protected:
		Sprite* m_pSprite;

		float m_x;
		float m_y;
		float m_velX;
		float m_velY;
		float m_accX;
		float m_accY;

		float m_levelWidth;
		float m_levelHeight;

		// Stats
		float m_speed = 100;
		float m_jump = 150;

		// Mechanics
		const float MAX_VELOCITY_Y = 100.0f;


		const float GRAVITY = 9.8f;
		const float BASE_VELOCITY = 10;
		const float MAX_AIR_TIME = 1.2f;

		float m_airTime = 0.0f;
		float m_jumpImpulse = 0.2f;
		float m_jumpImpulseVel = -40.0f;
		float m_jumpAccel = -1.0f;

		Foothold* m_fh;
	};
}
#endif