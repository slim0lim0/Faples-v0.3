#include "Character.hpp"
#include "Time.hpp"
#include "Window.hpp"

#include <FaplesFpx/bitmap.hpp>
#include <FaplesFpx/fpx.hpp>

#include <algorithm>
#include <cmath>

namespace fpl
{

#pragma region Character
	// Character
	Character::Character(node n)
	{
		auto skele = n.GetNode(n, "Skeleton");
		auto anim = n.GetNode(n, "Animations");
		auto feat = n.GetNode(n, "Features");

		for (auto itpa = skele.begin(); itpa != skele.end(); itpa++)
		{
			parts.emplace_back(new CharPart(itpa));
		}

		std::sort(parts.begin(), parts.end(), [](const CharPart* lpa, const CharPart* rpa)
		{
			return lpa->zindex < rpa->zindex;
		});

		for (auto itan = anim.begin(); itan != anim.end(); itan++)
		{
			animations.emplace_back(new CharAnim(itan));			
		}

		for (auto itfe = feat.begin(); itfe != feat.end(); itfe++)
		{
			features.emplace_back(new CharFeature(itfe));

			auto oFeature = features[features.size() - 1];

			for (int i = 0; i < parts.size(); i++)
			{
				auto oPart = parts[i];

				if (oFeature->part == oPart->name)
				{
					int iFeature = features.size() - 1;

					oPart->features.push_back(features[iFeature]);
				}
			}
		}
	}
	Character::~Character()
	{
	}
	int Character::GetWidth()
	{
		if (animations.size() > 0)
			return animations[iCurrAnimation]->width;
	}
	int Character::GetHeight()
	{
		if (animations.size() > 0)
			return animations[iCurrAnimation]->height;
	}
	void Character::Process(float delta)
	{
		if (animations.size() > 0)
			animations[iCurrAnimation]->Update(delta);
	}
	void Character::DrawCharacter(int iX, int iY)
	{
		auto anim = animations[iCurrAnimation];

		for (CharPart* part : parts)
		{
			part->zindex = anim->GetPartByID(part->startID).zindex;
		}

		std::sort(parts.begin(), parts.end(), [](const CharPart* lpa, const CharPart* rpa)
		{
			return lpa->zindex < rpa->zindex;
		});

		for (CharPart* part : parts)
		{
			anim->DrawPart(part, iX, iY, flip);
		}
	}
	void Character::SetFlip(bool flipped)
	{
		flip = flipped;

		for (int i = 0; i < parts.size(); i++)
		{
			if (parts[i]->m_pSprite)
				parts[i]->m_pSprite->SetFlip(flipped);

			for (int f = 0; f < parts[i]->features.size(); f++)
			{
				if(parts[i]->features[f]->m_pSprite)
					parts[i]->features[f]->m_pSprite->SetFlip(flipped);
			}
		}
	}
	void Character::SetAnimation(std::string sAnimation)
	{
		for (int i = 0; i < animations.size(); i++)
		{
			if (animations[i]->name == sAnimation)
				iCurrAnimation = i;
		}
	}
#pragma endregion

#pragma region Character Part
	CharPart::CharPart(node n)
	{
		name = n.name();
		type = n.GetNode(n, "Type").get_string();
		offsetX = n.GetNode(n, "OffsetX").get_integer();
		offsetY = n.GetNode(n, "OffsetY").get_integer();
		zindex = n.GetNode(n, "ZIndex").get_integer();

		startID = n.GetNode(n, "StartID").get_string();
		x1 = n.GetNode(n, "x1").get_integer();
		y1 = n.GetNode(n, "y1").get_integer();
		endID = n.GetNode(n, "EndID").get_string();
		x2 = n.GetNode(n, "x2").get_integer();
		y2 = n.GetNode(n, "y2").get_integer();

		std::string sSprite = n.GetNode(n, "Sprite").get_string();
		
		if (sSprite != "")
		{
			auto root = fpx::fCharacter.root();
			auto spr = root.GetNode(root, "Sprites/" + sSprite).get_bitmap();

			m_pSprite = new Sprite();
			m_pSprite->LoadTextureFromBitmap(sSprite, spr.data(), spr.width(), spr.height());
		}

		Reposition();
	}
	CharPart::~CharPart()
	{
	}
	void CharPart::Reposition()
	{
		x1 = x1 * window::GetWidthScale();
		y1 = y1 * window::GetHeightScale();
		x2 = x2 * window::GetWidthScale();
		y2 = y2 * window::GetHeightScale();
	}
#pragma endregion

#pragma region Character Animation
	// Animation
	CharAnim::CharAnim(node n)
	{
		name = n.name();
		width = n.GetNode(n, "Width").get_integer();
		height = n.GetNode(n, "Height").get_integer();
		speed = n.GetNode(n, "Speed").get_real();

		auto fra = n.GetNode(n, "Frames");

		for (auto itfr = fra.begin(); itfr != fra.end(); itfr++)
		{
			frames.emplace_back(new AnimFrame(itfr));
		}
	}
	CharAnim::~CharAnim()
	{
	}

	void CharAnim::Update(float delta)
	{
		frameTimer += delta;

		if (frameTimer > speed)
		{
			currFrame += 1;
			frameTimer = 0;
		}

		if (currFrame >= frames.size())
			currFrame = 0;
	}



	void CharAnim::DrawPart(CharPart * part, int iX, int iY, bool bFlipped)
	{
		bool bContainsPart = false;

		if (frames.size() > 0)
		{
			auto frame = frames[currFrame];

			for (FramePart* oPart : frame->parts)
			{
				if (oPart->startID == part->startID && oPart->endID == part->endID)
				{
					if (part->m_pSprite)
					{
						int x1 = oPart->x1;
						int x2 = oPart->x2;
						int y1 = oPart->y1;
						int y2 = oPart->y2;

						if (bFlipped)
						{
							x1 = width - oPart->x1;
							x2 = width - oPart->x2;
						}

						float xDiff = x2 - x1;
						float yDiff = y2 - y1;

						double angle = atan2(yDiff, xDiff) * 180.0 / M_PI;

						if (part->type == "Head")
						{
							int trueDiameter = (int)sqrt((pow(oPart->x1 - oPart->x2, 2)) + (pow(y1 - y2, 2)));

							int diameter = (int)sqrt((pow(x1 - x2, 2)) + (pow(y1 - y2, 2)));
							int radius = diameter / 2;

							int centerX = (x1 + x2) / 2;
							int centerY = (y1 + y2) / 2;

							if (bFlipped)
							{

								int iFinalOffsetX = part->offsetX;
								int iFinalOffsetY = part->offsetY;
								int iFinalX = iX + (centerX - radius) - iFinalOffsetX;
								int iFinalY = iY + (centerY - radius) - iFinalOffsetY;

								part->m_pSprite->RotateAndDraw({ iFinalX, iFinalY, diameter, diameter }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);

								for (int i = 0; i < part->features.size(); i++)
								{
									auto feature = part->features[i];

									iFinalOffsetX = feature->offsetX;
									iFinalOffsetY = feature->offsetY;
									iFinalX = iX + (width - iFinalOffsetX) - (feature->width) - (centerX - radius);
									iFinalY = iY + oPart->y1 + iFinalOffsetY;

									feature->m_pSprite->RotateAndDraw({ iFinalX,  iFinalY, feature->width, feature->height }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);
								}
							}
							else
							{
								int iFinalOffsetX = part->offsetX;
								int iFinalOffsetY = part->offsetY;
								int iFinalX = iX + (centerX - radius) - iFinalOffsetX;
								int iFinalY = iY + (centerY - radius) - iFinalOffsetY;

								part->m_pSprite->RotateAndDraw({ iFinalX, iFinalY, diameter, diameter }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);

								for (int i = 0; i < part->features.size(); i++)
								{
									auto feature = part->features[i];

									iFinalOffsetX = feature->offsetX;
									iFinalOffsetY = feature->offsetY;
									iFinalX = iX + (oPart->x1 - radius) + iFinalOffsetX;
									iFinalY = iY + oPart->y1 + iFinalOffsetY;

									feature->m_pSprite->RotateAndDraw({ iFinalX,  iFinalY, feature->width, feature->height }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);
								}
							}
						}
						else if (part->type == "Limb")
						{		
							if (bFlipped)
							{
								int iFinalOffsetX = part->m_pSprite->GetSpriteWidth() - part->offsetX;
								int iFinalOffsetY = part->offsetY;
								int iFinalX = iX + (width - oPart->x1) - (iFinalOffsetX);
								int iFinalY = iY + oPart->y1 + iFinalOffsetY;

								part->m_pSprite->RotateAndDraw({ iFinalX,  iFinalY }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);

								for (int i = 0; i < part->features.size(); i++)
								{
									auto feature = part->features[i];

									iFinalOffsetX = feature->m_pSprite->GetSpriteWidth() - feature->offsetX;
									iFinalOffsetY = feature->offsetY;
									iFinalX = iX + (width - oPart->x1) - (iFinalOffsetX);
									iFinalY = iY + oPart->y1 + iFinalOffsetY;

									feature->m_pSprite->RotateAndDraw({ iFinalX,  iFinalY}, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);
								}
							}
							else
							{
								int iFinalOffsetX = part->offsetX;
								int iFinalOffsetY = part->offsetY;
								int iFinalX = iX + oPart->x1 - iFinalOffsetX;
								int iFinalY = iY + oPart->y1 + iFinalOffsetY;

								part->m_pSprite->RotateAndDraw({ iFinalX,  iFinalY }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);

								for (int i = 0; i < part->features.size(); i++)
								{
									auto feature = part->features[i];

									iFinalOffsetX = feature->offsetX;
									iFinalOffsetY = feature->offsetY;
									iFinalX = iX + oPart->x1 - iFinalOffsetX;
									iFinalY = iY + oPart->y1 + iFinalOffsetY;

									feature->m_pSprite->RotateAndDraw({ iFinalX,  iFinalY }, angle - 90, { iFinalOffsetX, iFinalOffsetY }, window::gRenderer);
								}
							}
						}
					}
					break;
				}
			}
		}
	}
	FramePart & CharAnim::GetPartByID(std::string sID)
	{
		if (frames.size() > 0)
		{
			auto frame = frames[currFrame];

			for (FramePart* oPart : frame->parts)
			{
				if (oPart->startID == sID)
					return *oPart;
			}
		}
	}
#pragma endregion

#pragma region Animation Frame
	// Frame
	AnimFrame::AnimFrame(node n)
	{
		for (auto itpa = n.begin(); itpa != n.end(); itpa++)
		{
			parts.emplace_back(new FramePart(itpa));
		}
	}

	AnimFrame::~AnimFrame()
	{
	}
#pragma endregion

#pragma region Frame Part
	// Parts
	FramePart::FramePart(node n)
	{
		startID = n.GetNode(n, "StartID").get_string();
		x1 = n.GetNode(n, "x1").get_integer();
		y1 = n.GetNode(n, "y1").get_integer();
		endID = n.GetNode(n, "EndID").get_string();
		x2 = n.GetNode(n, "x2").get_integer();
		y2 = n.GetNode(n, "y2").get_integer();
		zindex = n.GetNode(n, "ZIndex").get_integer();
	}

	FramePart::~FramePart()
	{
	}
#pragma endregion

#pragma region Character Feature
	CharFeature::CharFeature(node n)
	{
		name = n.name();
		type = n.GetNode(n, "Type").get_string();
		part = n.GetNode(n, "Part").get_string();
		offsetX = n.GetNode(n, "OffsetX").get_integer();
		offsetY = n.GetNode(n, "OffsetY").get_integer();
		width = n.GetNode(n, "Width").get_integer();
		height = n.GetNode(n, "Height").get_integer();

		std::string sSprite = n.GetNode(n, "Sprite").get_string();

		if (sSprite != "")
		{
			auto root = fpx::fCharacter.root();
			auto spr = root.GetNode(root, "Sprites/" + sSprite).get_bitmap();

			m_pSprite = new Sprite();
			m_pSprite->LoadTextureFromBitmap(sSprite, spr.data(), spr.width(), spr.height());
		}
	}
	CharFeature::~CharFeature()
	{
	}
#pragma endregion
}