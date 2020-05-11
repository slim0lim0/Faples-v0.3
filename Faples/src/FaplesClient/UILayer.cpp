#include "UILayer.hpp"
#include <FaplesFpx/node.hpp>
#include <FaplesFpx/fpx.hpp>

class UIGroup;

namespace fpl
{
	UILayer::UILayer(node n, UIGroup& g) : group(g) {
		//scrollX = n.GetNode(n, "ScrollX").get_real();
		//scrollY = n.GetNode(n, "ScrollY").get_real();

		auto scenery = n.GetNode(n, "Scenery");
		auto obj = n.GetNode(n, "Objects");
		auto tile = n.GetNode(n, "Tiles");
		auto control = n.GetNode(n, "Controls");

		oScenery.emplace_back(new Scenery(scenery));

		for (auto itob = obj.begin(); itob != obj.end(); itob++)
		{
			oObjects.emplace_back(new Obj(itob));

			std::string name = itob.GetNode(itob, "Name");
			std::string sprsheet = itob.GetNode(itob, "SpriteSheet");

			auto root = fpx::fMap.root();
			auto obj = root.GetNode(root, "Objects/" + sprsheet + "/" + name);
		}

		for (auto itti = tile.begin(); itti != tile.end(); itti++)
		{
			oTiles.emplace_back(new Tile(itti));

			std::string name = itti.GetNode(itti, "Name");
			std::string sprsheet = itti.GetNode(itti, "SpriteSheet");

			auto root = fpx::fMap.root();
			auto tile = root.GetNode(root, "Tiles/" + sprsheet + "/" + name);
		}

		for (auto itcon = control.begin(); itcon != control.end(); itcon++)
		{
			std::string type = itcon.GetNode(itcon, "Type").get_string();

			if (type == "Textbox")
			{
				oControls.emplace_back(new UITextBox(itcon, *this));
			}
			else if (type == "Button")
			{
				oControls.emplace_back(new UIButton(itcon, *this));
			}
		}
	}

	UILayer::~UILayer() {

	}
	void UILayer::Process()
	{
		for (Scenery* sc : oScenery)
		{
			sc->Update();
		}

		for (Obj* ob : oObjects)
		{
			ob->Update();
		}

		for (Tile* tile : oTiles)
		{
			tile->Update();
		}

		for (UIControl* control : oControls)
		{
			control->Process();
		}
	}
	void UILayer::Render()
	{
		for (Scenery* sc : oScenery)
		{
			sc->Render();
		}

		for (Obj* ob : oObjects)
		{
			ob->Render();
		}

		for (Tile* tile : oTiles)
		{
			tile->Render();
		}

		for (UIControl* control : oControls)
		{
			control->Render();
		}
	}
}