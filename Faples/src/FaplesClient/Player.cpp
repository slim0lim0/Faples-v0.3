#include "Player.hpp"
#include "Window.hpp"
#include "Time.hpp"
#include "Portal.hpp"
#include "Map.hpp"
#include "View.hpp"

#include <FaplesFpx/bitmap.hpp>
#include <FaplesFpx/fpx.hpp>
#include <cmath>

namespace fpl
{
	double const longFallTime = 100;

	Player::Player()
	{
		auto root2 = fpx::fCharacter.root();
		std::string sCharRobot = "player1";
		auto charRobot = root2.GetNode(root2, "Characters/" + sCharRobot);

		character = new Character(charRobot);
		FALLING = true;

		m_x = 0;
		m_y = 0;
		m_width = character->GetWidth();
		m_height = character->GetHeight();
		m_velX = 0;
		m_velY = 0;
		iSpeed = 0;

		phy = new Physics();

		Reposition();
	}

	Player::~Player()
	{
		delete phy;
		phy = nullptr;
	}

	void Player::Update()
	{
		phy->RIGHT = MOVE_RIGHT;
		phy->LEFT = MOVE_LEFT;
		phy->UP = MOVE_UP;
		phy->DOWN = MOVE_DOWN;

		if (MOVE_UP)
		{
			for (Portal* p : map::portals)
			{
				if (p->type != "1" && p->type != "2")
				{
					int xmin = p->GetPosition().first - p->width/2;
					int xmax = p->GetPosition().first + p->width/2;
					int ymin = p->GetPosition().second - p->height;
					int ymax = p->GetPosition().second + p->height;

					if (m_x >= xmin && m_x <= xmax && m_y >= ymin && m_y <= ymax)
					{
						map::Teleport(p);
						return;
					}
				}
			}
		}

		if (JUMPING)
		{
			phy->Jump();
			JUMPING = false;
		}
		phy->Update();
		// Animations
		if (MOVE_RIGHT)
		{
			if (phy->fh != NULL)
			{
				if (phy->fh->type == "0")
				{
					character->SetFlip(true);
					character->SetAnimation("Walk1");
				}
			}
			else if (phy->fallTimeOut == 0)
			{
				character->SetFlip(true);
				if(phy->fallDuration > longFallTime)
					character->SetAnimation("Fall2");
				else
					character->SetAnimation("Fall1");
			}
		}

		if (MOVE_LEFT)
		{
			if (phy->fh != NULL)
			{
				if (phy->fh->type == "0")
				{
					character->SetFlip(false);
					character->SetAnimation("Walk1");
				}
			}
			else if (phy->fallTimeOut == 0)
			{
				character->SetFlip(false);
				if (phy->fallDuration > longFallTime)
					character->SetAnimation("Fall2");
				else
					character->SetAnimation("Fall1");
			}
		}

		if (!MOVE_RIGHT && !MOVE_LEFT || MOVE_RIGHT && MOVE_LEFT)
		{
			if (phy->fh != NULL)
			{
				character->SetAnimation("Idle1");
			}
			else if(phy->fallTimeOut == 0)
			{
				if (phy->fallDuration > longFallTime)
					character->SetAnimation("Fall2");
				else
					character->SetAnimation("Fall1");
			}
		}
		vector2i pos = phy->GetPosition();
		m_x = pos.first;
		m_y = pos.second;
		z = phy->layer;

		character->Process(time::delta);
	}

	void Player::Render()
	{
		if (character)
		{
			character->DrawCharacter(m_x - View::fx, m_y - View::fy);
			/*SDL_SetRenderDrawColor(window::gRenderer, 0xFF, 0x00, 0x00, 0xFF);
			SDL_Rect fillRect;

			fillRect.x = ((m_x - View::fx) + m_width/2) - 3;
			fillRect.y = ((m_y - View::fy) + m_height) - 3;
			fillRect.w = 6;
			fillRect.h = 6;

			SDL_RenderFillRect(window::gRenderer, &fillRect);*/
		}		
	}

	void Player::SetLayer(int layer)
	{
		z = layer;
	}

	void Player::Respawn(int x, int y)
	{
		m_x = x;
		m_y = y;
		m_width = character->GetWidth();
		m_height = character->GetHeight();

		phy->Reset(m_x, m_y, m_width, m_height);
	}
	void Player::Reposition()
	{
		m_x = m_x * window::GetWidthScale();
		m_y = m_y * window::GetHeightScale();
		m_width = m_width * window::GetWidthScale();
		m_height = m_height * window::GetHeightScale();

		phy->Reset(m_x, m_y, m_width, m_height);
	}
	void Player::SetPosition(int x, int y)
	{
		m_x = x;
		m_y = y;

		Reposition();
	}
}

