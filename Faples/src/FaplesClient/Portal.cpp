#include "Portal.hpp"
#include <FaplesFpx/bitmap.hpp>
#include <FaplesFpx/fpx.hpp>
#include "Map.hpp"
#include "Window.hpp"
#include "View.hpp"

#include <cassert>
#include <cmath>

namespace fpl
{
	Portal::Portal(node n)
	{
		auto test = n.name();
		id = std::stoi(n.name());
		type = n.GetNode(n, "type");
		x = n.GetNode(n, "mapX").get_integer();
		y = n.GetNode(n, "mapY").get_integer();
		mapid = n.GetNode(n, "mapID").get_integer();
		regionid = n.GetNode(n, "regionID").get_integer();
		targetid = n.GetNode(n, "targetID").get_integer();

		m_pSprite = new Sprite();

		auto root = fpx::fMap.root();
		auto sprites = root.GetNode(root, "Global/Portal/Sprites");

		if (type == "0")
		{
			auto spr = sprites.GetNode(sprites, "portal.png").get_bitmap();
			m_pSprite->LoadTextureFromBitmap("portal.png", spr.data(), spr.width(), spr.height());
			width = spr.width();
			height = spr.height();
		}
		else if (type == "1")
		{
			auto spr = sprites.GetNode(sprites, "spawnEnter.png").get_bitmap();
			m_pSprite->LoadTextureFromBitmap("spawnEnter.png", spr.data(), spr.width(), spr.height());
			width = spr.width();
			height = spr.height();
		}
		else if (type == "2")
		{
			auto spr = sprites.GetNode(sprites, "spawnExit.png").get_bitmap();
			m_pSprite->LoadTextureFromBitmap("spawnExit.png", spr.data(), spr.width(), spr.height());
			width = spr.width();
			height = spr.height();
		}

		Reposition();
	}

	Portal::~Portal()
	{
		delete m_pSprite;
		m_pSprite = nullptr;
	}

	void Portal::Update()
	{
	}

	void Portal::Render()
	{
		assert(m_pSprite);
		m_pSprite->Draw({ x - View::fx, y - View::fy }, window::gRenderer);
	}

	void Portal::Reposition()
	{
		x = x * window::GetWidthScale();
		y = y * window::GetHeightScale();
		width = width * window::GetWidthScale();
		height = height * window::GetHeightScale();
	}

	vector2i Portal::GetPosition()
	{
		return vector2i(x, y);
	}
}

