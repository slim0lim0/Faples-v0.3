#include "Obj.hpp"
#include "Window.hpp"
#include <FaplesFpx/node.hpp>
#include <FaplesFpx/bitmap.hpp>
#include <FaplesFpx/fpx.hpp>
#include "Time.hpp"
#include "View.hpp"

#include <cassert>

namespace fpl
{
	Obj::Obj(node n)
	{
	    std::string name = n.GetNode(n, "Name");
		std::string  sprsheet = n.GetNode(n, "SpriteSheet");

		auto root = fpx::fMap.root();
		auto spr = root.GetNode(root, "SpriteSheets/" + sprsheet).get_bitmap();
		auto obj = root.GetNode(root, "Objects/" + sprsheet + "/" + name);

		m_pSprite = new Sprite();
		m_pSprite->LoadTextureFromBitmap(sprsheet, spr.data(), spr.width(), spr.height());

		flipped = n.GetNode(n, "flipped").get_bool();
		x = n.GetNode(n, "mapX").get_integer();
		y = n.GetNode(n, "mapY").get_integer();
		width = obj.GetNode(obj, "width").get_integer();
		height = obj.GetNode(obj, "height").get_integer();

		auto anim = obj.GetNode(obj, "Anim");

		animated = anim.GetNode(anim, "Animated").get_bool();

		if (animated)
		{
			auto aindex = anim.GetNode(anim, "Index").get_integer();
			auto animation = anim.GetNode(anim, "Animation");

			for (auto itanm = animation.begin(); itanm != animation.end(); itanm++)
			{
				auto cindex = itanm.GetNode(itanm, "ReelIndex").get_integer();
				if (cindex == aindex)
				{
					areelh = itanm.GetNode(itanm, "ReelHeight").get_integer();
					areeli = itanm.GetNode(itanm, "ReelIndex").get_integer() - 1;
					aframew = itanm.GetNode(itanm, "FrameWidth").get_integer();
					aframet = itanm.GetNode(itanm, "TotalFrames").get_integer();
					aframesp = itanm.GetNode(itanm, "FrameSpeed").get_real();

					break;
				}
			}

			m_pSprite->EnableAnimation(animation.size(), aframesp, areelh);
			m_pSprite->SetFrameReel(areeli, aframet, aframew);
		}
		else
		{
			m_pSprite->DisableAnimation();
			int sprX = obj.GetNode(obj, "x").get_integer();
			int sprY = obj.GetNode(obj, "y").get_integer();
			m_pSprite->SetSprite(sprX, sprY, width, height);
		}

		m_pSprite->SetFlip(flipped);
		

		Reposition();
	}

	Obj::~Obj()
	{
		delete m_pSprite;
		m_pSprite = nullptr;
	}

	void Obj::Update()
	{
		m_pSprite->Process(time::delta);
	}

	void Obj::Render()
	{
		//assert(m_pSprite);
		if (m_pSprite)
			m_pSprite->Draw({ x - View::fx, y - View::fy }, window::gRenderer);
	}
	void Obj::SetLocation(int iX, int iY)
	{
		x = iX;
		y = iY;

		Reposition();
	}
	void Obj::Reposition()
	{
		x = x * window::GetWidthScale();
		y = y * window::GetHeightScale();
		//width = width * window::GetWidthScale();
		//height = height * window::GetHeightScale();
	}
}