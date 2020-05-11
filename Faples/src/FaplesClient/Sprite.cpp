#include "Sprite.hpp"
#include "TextureManager.hpp"
#include <stdio.h>
#include <string>


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

	void Sprite::SetFrameReel(int reelNum, int reelSize, float frWidth)
	{
		currReel = reelNum;
		numFrames = reelSize;
		frameWidth = frWidth;
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

	void Sprite::SetSpriteSize(int w, int h)
	{
		spriteW = w;
		spriteH = h;
	}

	void Sprite::SetSpriteLocation(int x, int y)
	{
		spriteX = x;
		spriteY = y;
	}

	int Sprite::GetSpriteWidth()
	{
		return spriteW;
	}

	int Sprite::GetSpriteHeight()
	{
		return spriteH;
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

	bool Sprite::LoadTextureFromText(std::string text, TTF_Font* font, SDL_Color color)
	{
		if (texture != NULL)
		{
			SDL_DestroyTexture(texture);
			texture = NULL;
		}

		SDL_Surface* message = TTF_RenderText_Solid(font, text.c_str(), color);

		texture = SDL_CreateTextureFromSurface(window::gRenderer, message);

		SDL_FreeSurface(message);
		message = NULL;

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
		if (texture != NULL)
		{
			SDL_Rect frame = { 0, 0, 0, 0 };
			SDL_Rect render = pos;

			if (animated)
			{
				frame.x = (frameWidth)* currFrame;
				frame.y = (reelHeight)* currReel;
				frame.w = frameWidth;
				frame.h = reelHeight;

				render.w = frameWidth;
				render.h = reelHeight;
			}
			else
			{
				frame.x = spriteX;
				frame.y = spriteY;
				frame.w = spriteW;
				frame.h = spriteH;

				if(render.w == 0)
					render.w = this->spriteW;
				if (render.h == 0)
					render.h = this->spriteH;

				float wMult = window::GetWidthScale();
				float hMult = window::GetHeightScale();

				render.w = render.w * wMult;
				render.h = render.h * hMult;
			}
			SDL_RenderCopyEx(gRenderer, this->texture, &frame, &render, NULL, NULL, spriteFlip);
		}	
	}

	void Sprite::RotateAndDraw(SDL_Rect pos, double angle, SDL_Rect offset, SDL_Renderer* gRenderer)
	{
		if (texture != NULL)
		{
			SDL_Rect frame = { 0, 0, 0, 0 };
			SDL_Rect render = pos;

			frame.x = spriteX;
			frame.y = spriteY;
			frame.w = spriteW;
			frame.h = spriteH;

			if (render.w == 0)
				render.w = this->spriteW;
			if (render.h == 0)
				render.h = this->spriteH;

			SDL_Point* pOffset = new SDL_Point();
			pOffset->x = offset.x;
			pOffset->y = offset.y;
			
			SDL_RenderCopyEx(gRenderer, this->texture, &frame, &render, angle, pOffset, spriteFlip);
		}
	}

	Sprite::Sprite()
	{
	}


	Sprite::~Sprite()
	{
		//SDL_DestroyTexture(texture);
		//texture = NULL;
	}
}
