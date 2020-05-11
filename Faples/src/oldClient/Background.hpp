#pragma once

#ifndef __BACKGROUND_H__
#define __BACKGROUND_H__

#include "Entity.hpp"

namespace fpl
{
	class Background : public Entity
	{
	public:
		Background();
		~Background();

		void SetParallaxSettings(float xOffset, float yOffset);
		void SetParallaxMultipliers(float xMult, float yMult);
		void Process(float deltaTime);
	private:
		float m_xOffset = 0.0f;
		float m_yOffset = 0.0f;
		float m_xMultiplier = 0.0f;
		float m_yMultiplier = 0.0f;
	};
}

#endif