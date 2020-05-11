// This include
#include "InputHandler.hpp"

// Library Includes
#include <SDL.h>

namespace fpl
{
	InputHandler::InputHandler()
	{

	}

	InputHandler::~InputHandler()
	{

	}

	void
		InputHandler::ProcessInput(Player& player)
	{
		SDL_Event e;
		while (SDL_PollEvent(&e)) {
			switch (e.type)
			{
			case SDL_KEYDOWN:
			{
				SDL_Keycode event = e.key.keysym.sym;
				if (event == SDLK_w)
				{
					if (player.CAN_JUMP)
					{
						player.JUMPING = true;
						player.CAN_JUMP = false;
					}
					break;
				}
				if (event == SDLK_s)
				{

					break;
				}
				if (event == SDLK_a)
				{
					player.MOVE_LEFT = true;
					break;
				}
				if (event == SDLK_d)
				{
					player.MOVE_RIGHT = true;
					break;
				}
			}
			case SDL_KEYUP:
			{

				SDL_Keycode event = e.key.keysym.sym;
				if (event == SDLK_w)
				{
					break;
				}
				if (event == SDLK_s)
				{
					break;
				}
				if (event == SDLK_a)
				{
					player.MOVE_LEFT = false;
					break;
				}
				if (event == SDLK_d)
				{
					player.MOVE_RIGHT = false;
					break;
				}
			}
			default:
			{
				e.key.keysym.sym = 0;
				break;
			}
			}
		}
	}
}

