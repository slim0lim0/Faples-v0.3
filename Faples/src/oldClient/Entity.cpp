//This includes:
#include "Entity.hpp"

// Library includes:
#include <cassert>
#include <cmath>

namespace fpl
{
	Entity::Entity()
	{

	}

	Entity::~Entity()
	{

	}

	void Entity::Initialize(Sprite& sprite)
	{
		m_pSprite = &sprite;
	}

	void Entity::SetLevelBounds(float levelWidth, float levelHeight)
	{
		m_levelWidth = levelWidth;
		m_levelHeight = levelHeight;
	}

	void
		Entity::Process(float deltaTime)
	{
		m_pSprite->Process(deltaTime);
	}

	void
		Entity::Draw(SDL_Renderer& oRenderer)
	{
		assert(m_pSprite);
		m_pSprite->Draw({ (int)m_x, (int)m_y }, &oRenderer);
	}
}





