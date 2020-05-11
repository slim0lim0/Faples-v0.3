#pragma once

#include <SDL.h>
#include <SDL_image.h>
#include <map>

namespace fpl
{
	namespace TMX
	{
		SDL_Texture* LoadTexture(std::string path);
		SDL_Texture* LoadTextureFromSurface(std::string path, SDL_Surface* surface);
		SDL_Texture* LoadTextureFromBitmap(std::string path, const void* data, int width, int height);
	}
}