#pragma once
#ifndef __GAME_H__
#define __GAME_H__

#include "InputHandler.hpp"
#include "Sprite.hpp"
#include "Player.hpp"

#include <SDL.h>
#include <vector>

namespace fpl
{
	class Game
	{
		//Methods

	public:
		static Game& GetInstance();
		static void DestroyInstance();

		~Game();

		bool Initialise();
		bool DoGameLoop();
		void Quit();
	protected:
		void Process();
		void Draw();
	private:
		Game(const Game& game);
		Game& operator=(const Game& game);
		Game();


		// Members
	protected:
		static Game* sm_pInstance;
		// Process Loop & members
		bool m_looping;
		InputHandler* gInputHandler;




		Player* m_Player;

	//	std::vector<Background*> m_colBackgrounds;
	private:
	};
}



#endif // __GAME_H__