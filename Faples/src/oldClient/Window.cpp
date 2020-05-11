#include "Window.hpp"

namespace fpl 
{
	namespace window 
	{
		const int WINDOW_WIDTH = 1440;
		const int WINDOW_HEIGHT = 1000;
		SDL_Window* gWindow;
		SDL_Renderer* gRenderer;

		void init() 
		{
			SDL_Init(SDL_INIT_EVERYTHING);

			gWindow = SDL_CreateWindow("Faples 0.1", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 1, 1, SDL_WINDOW_SHOWN);
			gRenderer = SDL_CreateRenderer(gWindow, -1, SDL_RENDERER_ACCELERATED);

			//Initialize renderer color
			SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);

			SDL_SetRenderDrawBlendMode(gRenderer, SDL_BLENDMODE_BLEND);
		}

		void recreate(bool fullscreen)
		{
			if (fullscreen)
				SDL_SetWindowFullscreen(gWindow, SDL_WINDOW_FULLSCREEN);
			else
			{
				SDL_SetWindowFullscreen(gWindow, 0);
				SDL_SetWindowSize(gWindow, WINDOW_WIDTH, WINDOW_HEIGHT);
			}
		}
	}
}