#ifndef __CHARACTER_H__
#define __CHARACTER_H__

#pragma once

#include "Sprite.hpp"
#include <FaplesFpx/node.hpp>

#include <vector>

namespace fpl
{
	class CharFeature
	{
	public:
		CharFeature(node n);
		~CharFeature();

		std::string name, type, part;
		Sprite* m_pSprite;
		int offsetX, offsetY, width, height;
	};
	class CharPart
	{
	public:
		CharPart(node n);
		~CharPart();
		void Reposition();

		int offsetX, offsetY;
		int zindex = 0;
		std::string startID;
		std::string endID;
		Sprite* m_pSprite;
		std::string name, type;
		std::vector<CharFeature*> features;
	private:
		

		int x1, x2, y1, y2;
	};
	class FramePart
	{
	public:
		FramePart(node n);
		~FramePart();

		std::string startID;
		std::string endID;
		int x1, x2, y1, y2, zindex;
	};

	class AnimFrame
	{
	public:
		AnimFrame(node n);
		~AnimFrame();
		std::vector<FramePart*> parts;
	};

	class CharAnim
	{
	public:
		CharAnim(node n);
		~CharAnim();
		void Update(float delta);
		void DrawPart(CharPart* part, int iX, int iY, bool bFlipped);
		FramePart& GetPartByID(std::string sID);
		int width, height;
		std::string name;
	private:
		double speed;
		double frameTimer;
		int currFrame = 0;
		std::vector<AnimFrame*> frames;	
	};
	class Character
	{
	public:
		Character(node n);
		~Character();
		int GetWidth();
		int GetHeight();
		void Process(float delta);
		void DrawCharacter(int iX, int iY);
		void SetFlip(bool flipped);
		void SetAnimation(std::string sAnimation);	
	private:
		int iCurrAnimation = 0;
		std::vector<CharAnim*> animations;
		std::vector<CharPart*> parts;
		std::vector<CharFeature*> features;
		bool flip = false;
	};	
}

#endif
