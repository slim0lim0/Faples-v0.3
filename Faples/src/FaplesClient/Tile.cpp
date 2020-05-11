#include "Tile.hpp"
#include "Window.hpp"
#include <FaplesFpx/node.hpp>
#include <FaplesFpx/bitmap.hpp>
#include <FaplesFpx/fpx.hpp>
#include "Time.hpp"
#include "View.hpp"

#include <cassert>

namespace fpl
{
	Tile::Tile(node n)
	{

		std::string name = n.GetNode(n, "Name");
		std::string sprsheet = n.GetNode(n, "SpriteSheet");

		auto sprName = name.substr(0, name.find_last_of("/"));

		auto root = fpx::fMap.root();
		auto spr = root.GetNode(root, "SpriteSheets/" + sprsheet).get_bitmap();
		auto tile = root.GetNode(root, "Tiles/" + sprsheet + "/" + name);

		m_pSprite = new Sprite();
		m_pSprite->LoadTextureFromBitmap(sprsheet, spr.data(), spr.width(), spr.height());

		flipped = n.GetNode(n, "flipped").get_bool();
		x = n.GetNode(n, "mapX").get_integer();
		y = n.GetNode(n, "mapY").get_integer();
		width = tile.GetNode(tile, "width").get_integer();
		height = tile.GetNode(tile, "height").get_integer();

		
		m_pSprite->DisableAnimation();
		int sprX = tile.GetNode(tile, "x").get_integer();
		int sprY = tile.GetNode(tile, "y").get_integer();
		m_pSprite->SetSprite(sprX, sprY, width, height);
		

		m_pSprite->SetFlip(flipped);

		Reposition();
	}

	Tile::~Tile()
	{
		delete m_pSprite;
		m_pSprite = nullptr;
	}

	void Tile::Update()
	{
		m_pSprite->Process(time::delta);
	}

	void Tile::Render()
	{
		if (m_pSprite)
			m_pSprite->Draw({ x - View::fx, y - View::fy }, window::gRenderer);
	}
	void Tile::Reposition()
	{
		x = x * window::GetWidthScale();
		y = y * window::GetHeightScale();
		width = width * window::GetWidthScale();
		height = height * window::GetHeightScale();
	}
	void Tile::SetLocation(int iX, int iY)
	{
		x = iX;
		y = iY;

		Reposition();
	}
}
