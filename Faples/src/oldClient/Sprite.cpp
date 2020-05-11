#include <SDL.h>
#include <SDL_image.h>
#include <stdio.h>
#include <string>
#include "Sprite.hpp"
#include "TextureManager.hpp"

namespace fpl
{
	void Sprite::EnableAnimation(int frReels, float frSpeed, float rlHeight)
	{
		animated = true;
		numReels = frReels;
		frameSpeed = frSpeed;
		reelHeight = rlHeight;
	}

	void Sprite::DisableAnimation()
	{
		animated = false;
		numFrames = 0;
		currFrame = 0;
		currReel = 0;
	}

	void Sprite::SetFrameReel(int reelNum, int reelSize, float frWidth, float frHeight)
	{
		currReel = reelNum;
		numFrames = reelSize;
		frameWidth = frWidth;
		frameHeight = frHeight;
	}


	void Sprite::SelectFrame(int frame)
	{
		currFrame = frame;
	}

	void Sprite::SetSprite(int x, int y, int w, int h)
	{
		spriteX = x;
		spriteY = y;
		spriteW = w;
		spriteH = h;
	}

	bool Sprite::LoadTexture(std::string path)
	{
		texture = TMX::LoadTexture(path);

		//Load image
		SDL_QueryTexture(texture, NULL, NULL, &spriteW, &spriteH);

		return true;
	}

	bool Sprite::LoadTextureFromSurface(std::string path, SDL_Surface* surface)
	{
		texture = TMX::LoadTextureFromSurface(path, surface);

		//Load image
		SDL_QueryTexture(texture, NULL, NULL, &spriteW, &spriteH);

		return true;
	}

	bool Sprite::LoadTextureFromBitmap(std::string path, const void* data, int width, int height)
	{
		texture = TMX::LoadTextureFromBitmap(path, data, width, height);

		//Load image
		SDL_QueryTexture(texture, NULL, NULL, &spriteW, &spriteH);

		return true;
	}

	void Sprite::SetFlip(bool flip)
	{
		if (flip)
			spriteFlip = SDL_FLIP_HORIZONTAL;
		else
			spriteFlip = SDL_FLIP_NONE;
	}

	void Sprite::UpdateCamera(float camX, float camY)
	{
		cameraX = camX;
		cameraY = camY;
	}

	void Sprite::Process(float deltaTime)
	{
		frameTimer += deltaTime;

		if (frameTimer > frameSpeed)
		{
			currFrame += 1;
			frameTimer = 0;
		}

		if (currFrame >= numFrames)
			currFrame = 0;
	}

	void Sprite::Draw(SDL_Rect pos, SDL_Renderer* gRenderer)
	{
		SDL_Rect frame = { 0, 0, 0, 0 };
		SDL_Rect render = pos;

		render.x -= cameraX;
		render.y -= cameraY;

		if (animated)
		{
			frame.x = (frameWidth)* currFrame;
			frame.y = (reelHeight)* currReel;
			frame.w = frameWidth;
			frame.h = frameHeight;

			render.w = frameWidth;
			render.h = frameHeight;
		}
		else
		{
			frame.x = spriteX;
			frame.y = spriteY;
			frame.w = spriteW;
			frame.h = spriteH;

			render.w = this->spriteW;
			render.h = this->spriteH;
		}
		SDL_RenderCopyEx(gRenderer, this->texture, &frame, &render, NULL, NULL, spriteFlip);
	}

	Sprite::Sprite()
	{
	}


	Sprite::~Sprite()
	{

	}
}
