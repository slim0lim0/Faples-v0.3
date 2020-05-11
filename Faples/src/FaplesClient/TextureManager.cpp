#include "TextureManager.hpp"
#include "Window.hpp"

namespace fpl
{
	namespace TMX
	{

		std::map<std::string, SDL_Texture*> textures;
		SDL_Texture *LoadTexture(std::string path)
		{
			if (textures.find(path) != textures.end())
			{
				return textures[path];
			}
			else
			{
				SDL_Texture* newT = nullptr;

				SDL_Surface* loadedSurface = IMG_Load(path.c_str());


				if (loadedSurface == nullptr)
				{
					printf("Failed to load image %s! SDL_image Error: %s\n", path.c_str(), IMG_GetError());
					return false;
				}
				else
				{
					//Create texture from surface pixels
					newT = SDL_CreateTextureFromSurface(window::gRenderer, loadedSurface);
					if (newT == nullptr)
					{
						printf("Unable to create texture from %s! SDL Error: %s\n", path.c_str(), SDL_GetError());
						return false;
					}

					//Free old loaded surface
					SDL_FreeSurface(loadedSurface);
					loadedSurface = NULL;
				}

				textures.emplace(path, newT);

				return newT;
			}
		}

		SDL_Texture *LoadTextureFromSurface(std::string path, SDL_Surface* surface)
		{
			if (textures.find(path) != textures.end())
			{
				return textures[path];
			}
			else
			{
				SDL_Texture* newT = nullptr;

				SDL_Surface* loadedSurface = surface;


				if (loadedSurface == nullptr)
				{
					printf("Failed to load image %s! SDL_image Error: %s\n", path.c_str(), IMG_GetError());
					return false;
				}
				else
				{
					//Create texture from surface pixels
					newT = SDL_CreateTextureFromSurface(window::gRenderer, loadedSurface);
					if (newT == nullptr)
					{
						printf("Unable to create texture from %s! SDL Error: %s\n", path.c_str(), SDL_GetError());
						return false;
					}

					//Free old loaded surface
					SDL_FreeSurface(loadedSurface);
					loadedSurface = NULL;
				}

				textures.emplace(path, newT);

				return newT;
			}
		}

		SDL_Texture * LoadTextureFromBitmap(std::string path, const void * data, int width, int height)
		{
			if (textures.find(path) != textures.end())
			{
				return textures[path];
			}
			else
			{
				SDL_Texture* newT = nullptr;

				auto depth = 32;
				auto pitch = 4 * width;

				Uint32 amask = 0xff000000;
				Uint32 rmask = 0x00ff0000;
				Uint32 gmask = 0x0000ff00;
				Uint32 bmask = 0x000000ff;

				SDL_Surface* loadedSurface = SDL_CreateRGBSurfaceFrom(const_cast<void*>(data), width, height, depth, pitch,
					rmask, gmask, bmask, amask);


				if (loadedSurface == nullptr)
				{
					printf("Failed to load image %s! SDL_image Error: %s\n", path.c_str(), IMG_GetError());
					return false;
				}
				else
				{
					//Create texture from surface pixels
					newT = SDL_CreateTextureFromSurface(window::gRenderer, loadedSurface);
					if (newT == nullptr)
					{
						printf("Unable to create texture from %s! SDL Error: %s\n", path.c_str(), SDL_GetError());
						return false;
					}

					//Free old loaded surface
					SDL_FreeSurface(loadedSurface);
				    loadedSurface = NULL;
				}

				textures.emplace(path, newT);

				return newT;
			}
		}		
	}
}