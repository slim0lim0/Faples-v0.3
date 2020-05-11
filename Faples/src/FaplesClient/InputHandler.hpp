#pragma once
#ifndef __INPUTHANDLER_H__
#define __INPUTHANDLER_H__

#include <vector>

//#include "Player.hpp"

namespace fpl
{
	class InputHandler
	{
		//Member Methods:
	public:
		InputHandler();
		~InputHandler();

		void ProcessInput();

		bool bUIFocused = false;
	//	void ProcessInput(Player& player);
	private:
		InputHandler(const InputHandler& inputHandler);
		InputHandler& operator=(const InputHandler& inputHandler);

		void PrepareTypeInputs();

		std::vector<int> typeInputs;
	};
}

#endif