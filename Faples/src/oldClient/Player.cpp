#include "Player.hpp"
#include "Foothold.hpp"

namespace fpl
{
	Player::Player()
	{
		FALLING = true;
	}

	Player::~Player()
	{

	}

	void Player::SendCamera(float camX, float camY)
	{
		m_pSprite->UpdateCamera(camX, camY);
	}

	void Player::Process(float deltaTime)
	{
		// Animations

		if (MOVE_RIGHT)
		{
			m_pSprite->SetFlip(true);
			m_pSprite->SetFrameReel(1, 4, 91, 80);
		}


		if (MOVE_LEFT)
		{
			m_pSprite->SetFlip(false);
			m_pSprite->SetFrameReel(1, 4, 91, 80);
		}




		// Physics

		if (m_fh == nullptr)
		{
			if (m_velY >= 0)
			{
				m_fh = Foothold::collided(m_x + 40, m_y + 80);
			}

			if (m_fh != nullptr)
			{
				CAN_JUMP = true;
				FALLING = false;
				m_velY = 0;
				m_accY = 0;
			}
			else
			{
				m_accY = GRAVITY;
			}
		}
		else
		{
			if (MOVE_RIGHT)
			{
				if ((int)m_x + 40 > m_fh->x2)
				{
					if (m_fh->next == NULL)
					{
						m_fh = nullptr;
					}
					else
					{
						m_fh = Foothold::getById(m_fh->nextid, m_fh->group);
					}
				}

				if (m_fh != NULL)
				{
					float vy = (m_fh->y2 - m_fh->y1);
					float vx = (m_fh->x2 - m_fh->x1);
					float m = vy / vx;

					m_velY = 10 * m;

					if (vx != 0)
					{
						if (m_velX < (m_speed / 10))
							m_velX += (m_speed / 10);
						else
							m_velX = (m_speed / 10);
					}
				}
			}

			if (MOVE_LEFT)
			{
				if ((int)m_x + 40 < m_fh->x1)
				{
					if (m_fh->prev == NULL)
					{
						m_fh = nullptr;
						FALLING = true;
					}
					else
					{
						m_fh = Foothold::getById(m_fh->previd, m_fh->group);
					}
				}

				if (m_fh != NULL)
				{
					float vy = (m_fh->y2 - m_fh->y1);
					float vx = (m_fh->x2 - m_fh->x1);
					float m = vy / vx;

					m_velY = 10 * -m;

					if (m_velX > (-m_speed / 10))
						m_velX += (-m_speed / 10);
					else
						m_velX = (-m_speed / 10);
				}
			}
		}

		m_velY = m_velY + (m_accY * deltaTime);
		m_velX = m_velX + (m_accX * deltaTime);

		if (m_velY > MAX_VELOCITY_Y)
			m_velY = MAX_VELOCITY_Y;

		if (JUMPING)
		{
			JUMPING = false;
			m_fh = nullptr;
			m_velY = m_jumpImpulseVel;
		}

		if (!MOVE_RIGHT && !MOVE_LEFT || m_velX == 0)
		{
			if (m_fh)
				m_velY = 0;

			m_pSprite->SetFrameReel(0, 5, 82, 78);
			m_velX = 0;
		}

		m_y += m_velY * deltaTime;
		m_x += m_velX * deltaTime;

		m_pSprite->Process(deltaTime);
	}
}

