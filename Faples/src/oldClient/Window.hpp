#pragma once
#include <utility>
#include <SDL.h>

namespace fpl
{
	namespace window
	{
		// Window & Renderer
		extern SDL_Window* gWindow;
		extern SDL_Renderer* gRenderer;	

		void init();
		void recreate(bool fullscreen);
		void check_errors();
		void update();
		void unload();
		std::pair<double, double> mouse_pos();
	}
}

