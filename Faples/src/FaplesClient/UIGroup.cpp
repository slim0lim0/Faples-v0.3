#include "UIGroup.hpp"

namespace fpl
{
	UIGroup::UIGroup(node n)
	{
		name = n.name();
		bStatic = n.GetNode(n, "static").get_bool();
		x = n.GetNode(n, "x").get_integer();
		y = n.GetNode(n, "y").get_integer();
		width = n.GetNode(n, "width").get_integer();
		height = n.GetNode(n, "height").get_integer();

		auto allLayers = n.GetNode(n, "Layers");

		for (auto it = allLayers.begin(); it != allLayers.end(); it++)
		{
			UILayer* layer = new UILayer(it, *this);
			layer->z = layers.size();

			auto counter = layers.size();

			layers.emplace_back(layer);
			layers.at(counter)->z = counter;
		}

		Reposition();
	}

	UIGroup::~UIGroup()
	{
	}

	void UIGroup::Process()
	{
		for (UILayer* layer : layers)
		{
			layer->Process();
		}
	}

	void UIGroup::Render()
	{
		for (UILayer* layer : layers)
		{
			layer->Render();
		}
	}

	std::vector<UILayer*> UIGroup::GetLayers()
	{
		return layers;
	}

	void UIGroup::SetLocation(int x, int y)
	{
		for (UILayer* layer : layers)
		{
			for (Scenery* sc : layer->oScenery)
			{
				sc->SetLocation(x, y);
			}

			for (Obj* ob : layer->oObjects)
			{
				ob->SetLocation(x, y);
			}

			for (Tile* tile : layer->oTiles)
			{
				tile->SetLocation(x, y);
			}

			for (UIControl* control : layer->oControls)
			{
				control->SetLocation(x, y);
			}
		}
	}

	void UIGroup::Reposition()
	{
		x = x * window::GetWidthScale();
		y = y * window::GetHeightScale();
		width = width * window::GetWidthScale();
		height = height * window::GetHeightScale();
	}
}


