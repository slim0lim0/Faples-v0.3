// Local includes:
#include "Game.hpp"

// Library includes:
#include <Windows.h>

namespace fpl
{
	void Start()
	{
		Game& gameInstance = Game::GetInstance();
		if (!gameInstance.Initialise())
		{
			return;
			// terminate
		}

		while (gameInstance.DoGameLoop())
		{
			// No body.
		}


		Game::DestroyInstance();
	}
}

int __stdcall WinMain(HINSTANCE, HINSTANCE, char *, int) {
	fpl::Start();
	return (0);
}




