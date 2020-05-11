#pragma once
#include <utility>
#include <SDL.h>
#include <SDL_image.h>
#include <SDL_ttf.h>

namespace fpl
{
	namespace window
	{
		// Window & Renderer
		extern SDL_Window* gWindow;
		extern SDL_Renderer* gRenderer;	

		void init();
		void recreate(bool fullscreen);
		float GetWidthScale();
		float GetHeightScale();
		void check_errors();
		void update();
		void unload();
		std::pair<double, double> MousePos();
	}
}

