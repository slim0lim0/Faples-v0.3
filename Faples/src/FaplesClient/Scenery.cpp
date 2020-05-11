#include "Window.hpp"
#include "Scenery.hpp"
#include "View.hpp"
#include "Map.hpp"

#include <FaplesFpx/node.hpp>
#include <FaplesFpx/fpx.hpp>
#include <FaplesFpx/bitmap.hpp>

#include <cassert>


namespace fpl
{
	Scenery::Scenery(node n)
	{
		x = 0;
		y = 0;
		// Load Sprite
		std::string bg = n.GetNode(n, "background");

		parX = n.GetNode(n, "ParallaxX").get_real();
		parY = n.GetNode(n, "ParallaxY").get_real();
		scrX = n.GetNode(n, "ScrollX").get_real();
		scrY = n.GetNode(n, "ScrollY").get_real();
		tileX = n.GetNode(n, "TileX").get_bool();
		tileY = n.GetNode(n, "TileY").get_bool();
		flipped = n.GetNode(n, "flipped").get_bool();

		iTileXCount = 0;
		iTileYCount = 0;

		if (bg != "")
		{
			auto root = fpx::fMap.root();
			auto spr = root.GetNode(root, "SpriteSheets/" + bg).get_bitmap();

			m_pSprite = new Sprite();
			m_pSprite->LoadTextureFromBitmap(bg, spr.data(), spr.width(), spr.height());

			width = spr.width();
			height = spr.height();

			m_pSprite->SetFlip(flipped);
		}
		// Set info

		Reposition();
	}

	Scenery::~Scenery()
	{
		delete m_pSprite;
		m_pSprite = nullptr;
	}

	void Scenery::Update()
	{

	}

	void Scenery::Render()
	{
		if (m_pSprite)
		{
			int viewX = (int)((float)View::fx * (1 - parX));
			int viewY = (int)((float)View::fy * parY);
			int sceneY = (map::mheight - height) * parY;

			m_pSprite->Draw({ x - viewX,  sceneY - viewY }, window::gRenderer);

			if (tileX)
			{
				while (iTileXCount * width < map::mwidth)
				{
					iTileXCount++;

					int drawPos = (x + (width * iTileXCount));

					m_pSprite->Draw({ drawPos - viewX, sceneY - viewY }, window::gRenderer);
				}

				iTileXCount = 0;
			}		
		}
	}
	void Scenery::SetLocation(int iX, int iY)
	{
		x = iX;
		y = iY;

		Reposition();
	}
	void Scenery::Reposition()
	{
		x = x * window::GetWidthScale();
		y = y * window::GetHeightScale();
		width = width * window::GetWidthScale();
		height = height * window::GetHeightScale();
	}
}