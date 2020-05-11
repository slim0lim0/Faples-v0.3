#pragma once

#ifndef SPRITE_H
#define  SPRITE_H

#include "Window.hpp"
#include <string>

namespace fpl
{
	class Sprite
	{
		SDL_Texture* texture;
	public:

		// Animation Methods
		void EnableAnimation(int numReels, float frSpeed, float rlHeight);
		void DisableAnimation();

		void SetFrameReel(int reelNum, int reelSize, float frWidth);
		void SelectFrame(int frame);

		inline int GetTotalFrames() { return numFrames; };
		inline int GetFrameSpeed() { return frameSpeed; };

		void SetSprite(int x, int y, int w, int h);
		void SetSpriteSize(int w, int h);
		void SetSpriteLocation(int x, int y);

		int GetSpriteWidth();
		int GetSpriteHeight();

		//Load texture from a file
		bool LoadTexture(std::string path);
		bool LoadTextureFromSurface(std::string path, SDL_Surface* surface);
		bool LoadTextureFromBitmap(std::string path, const void* data, int width, int height);
		bool LoadTextureFromText(std::string text, TTF_Font* font, SDL_Color color);

		void SetFlip(bool flip);

		//Process sprite animation
		void Process(float DeltaTime);

		//Draw texture to screen
		void Draw(SDL_Rect pos, SDL_Renderer* gRenderer);
		void RotateAndDraw(SDL_Rect pos, double angle, SDL_Rect offset, SDL_Renderer* gRenderer);

		Sprite();
		virtual ~Sprite();
	private:
		bool animated = false;
		int numFrames = 0;
		int numReels = 0;
		int currFrame = 0;
		int currReel = 0;
		float frameSpeed = 0.0f;
		float frameTimer = 0.0f;
		float reelHeight = 0.0f;
		float frameWidth = 0.0f;
		float frameHeight = 0.0f;

		SDL_RendererFlip spriteFlip = SDL_FLIP_NONE;
		float spriteX;
		float spriteY;
		int spriteW;
		int spriteH;
	};
}

#endif