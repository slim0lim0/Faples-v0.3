#pragma once
#ifndef __INPUTHANDLER_H__
#define __INPUTHANDLER_H__

#include "Player.hpp"

namespace fpl
{
	class InputHandler
	{
		//Member Methods:
	public:
		InputHandler();
		~InputHandler();

		void ProcessInput(Player& player);
	private:
		InputHandler(const InputHandler& inputHandler);
		InputHandler& operator=(const InputHandler& inputHandler);
	};
}

#endif