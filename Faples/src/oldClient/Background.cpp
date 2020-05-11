#include "Background.hpp"

namespace fpl
{
	Background::Background()
	{

	}

	Background::~Background()
	{

	}

	void Background::SetParallaxSettings(float xOffset, float yOffset)
	{
		m_xOffset = xOffset * m_xMultiplier;
		m_yOffset = yOffset * m_yMultiplier;
	}

	void Background::SetParallaxMultipliers(float xMult, float yMult)
	{
		m_xMultiplier = xMult;
		m_yMultiplier = yMult;
	}

	void Background::Process(float deltaTime)
	{
		m_pSprite->UpdateCamera(m_xOffset, m_yOffset);
		m_pSprite->Process(deltaTime);
	}
}

