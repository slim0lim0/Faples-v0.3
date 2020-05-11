#include "Window.hpp"
#include "View.hpp"
#include "Session.hpp"



namespace fpl 
{
	namespace window 
	{
		const int DEFAULT_WIDTH = 1920;
		const int DEFAULT_HEIGHT = 1080;
		//const int WINDOW_WIDTH = 1440;
		//const int WINDOW_HEIGHT = 1000;

		const int WINDOW_WIDTH = 1920;
		const int WINDOW_HEIGHT = 1080;
		SDL_Window* gWindow;
		SDL_Renderer* gRenderer;

		void init() 
		{
			SDL_Init(SDL_INIT_EVERYTHING);

			//gWindow = SDL_CreateWindow("Faples 0.1", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 1, 1, SDL_WINDOW_SHOWN);
			gWindow = SDL_CreateWindow("Faples 0.3", 1, 1, 1, 1, SDL_WINDOW_SHOWN);
			gRenderer = SDL_CreateRenderer(gWindow, -1, SDL_RENDERER_ACCELERATED);
			SDL_GL_SetSwapInterval(0);

			//Initialize renderer color
			SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);

			SDL_SetRenderDrawBlendMode(gRenderer, SDL_BLENDMODE_BLEND);

			TTF_Init();
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
			View::Resize(WINDOW_WIDTH, WINDOW_HEIGHT);
		}
		float GetWidthScale()
		{
			float defWidth = DEFAULT_WIDTH;
			float winWidth = WINDOW_WIDTH;

			return winWidth / defWidth;
		}
		float GetHeightScale()
		{
			float defHeight = DEFAULT_HEIGHT;
			float winHeight = WINDOW_HEIGHT;

			return winHeight / defHeight;
		}
		std::pair<double, double> MousePos()
		{
			POINT cursorPos;

			GetCursorPos(&cursorPos);
			ScreenToClient(GetActiveWindow(), &cursorPos);
			return { cursorPos.x, cursorPos.y };
		}
	}
}