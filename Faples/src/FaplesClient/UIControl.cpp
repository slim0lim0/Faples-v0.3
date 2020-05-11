#include "UIControl.hpp"
#include "Window.hpp"
#include "View.hpp"
#include "Time.hpp"
#include "UI.hpp"
#include <FaplesFpx/bitmap.hpp>
#include <FaplesFpx/fpx.hpp>

namespace fpl
{
	//class UI;
	// Main Control Skeleton
	UIControl::UIControl(node n, UILayer& l) : layer(l)
	{
		name = n.name();
		type = n.GetNode(n, "Type").get_string();
		x = n.GetNode(n, "LocX").get_integer();
		y = n.GetNode(n, "LocY").get_integer();
		mapx = x;
		mapy = y;
		width = n.GetNode(n, "Width").get_integer();
		height = n.GetNode(n, "Height").get_integer();

		uiType = ui::currUiType;

		Reposition();
	}

	UIControl::~UIControl()
	{

	}

	void UIControl::SetLocation(int iX, int iY)
	{
		x = iX + x;
		y = iY + y;

		Reposition();
	}

	void UIControl::Reposition()
	{
		mapx = x * window::GetWidthScale();
		mapy = y * window::GetHeightScale();
		width = width * window::GetWidthScale();
		height = height * window::GetHeightScale();
	}

	// UI Textbox
	const std::string BASE = "Lato-Black";
	UITextBox::UITextBox(node n, UILayer& l) : UIControl(n, l)
	{
		multiline = n.GetNode(n, "Multiline").get_bool();

		fontFamily = n.GetNode(n, "Font").get_string();
		fontSize = n.GetNode(n, "Size").get_integer();
		colorR = n.GetNode(n, "ColorR").get_integer();
		colorG = n.GetNode(n, "ColorG").get_integer();
		colorB = n.GetNode(n, "ColorB").get_integer();

		label = n.GetNode(n, "Label").get_bool();
		trueValue = n.GetNode(n, "LabelValue").get_string();
		
		for (std::string::iterator it = fontFamily.begin(); it != fontFamily.end(); ++it) {
			if (*it == ' ') {
				*it = '-';
			}
		}

		if (fontFamily == "Lato")
		{
			fontFamily = "Lato-Regular";
		}

		std::string oFontPath = "Fonts/" + fontFamily + ".ttf";
		std::string oFontBase = "Fonts/" + BASE + ".ttf";

		if(fontFamily != "")
			font = TTF_OpenFont(oFontPath.c_str(), fontSize + 6);
		else
			font = TTF_OpenFont(oFontBase.c_str(), fontSize + 6);
			

		m_pMessage = new Sprite();

		if (label)
		{
			ReplaceText(trueValue);
		}
	}

	UITextBox::~UITextBox()
	{

	}

	void UITextBox::Process()
	{
		if (focused)
		{
			fFocusBarTick += time::delta;

			if (fFocusBarTick > 0.5)
			{
				bFocusBar = !bFocusBar;

				Prettify(bFocusBar);

				fFocusBarTick = 0.0;
			}
		}
	}

	void UITextBox::Render()
	{
		m_pMessage->Draw({ mapx - View::fx, mapy - View::fy }, window::gRenderer);
	}

	void UITextBox::ReplaceText(std::string sText)
	{
		trueValue = sText;

		if (name == "txtPassword")
		{
			std::string sMask = "";

			for (int i = 0; i < trueValue.size(); i++)
			{
				sMask += '*';
			}

			displayValue = sMask;
		}
		else
		{
			displayValue = trueValue;
		}


		m_pMessage->LoadTextureFromText(displayValue, font, { colorR, colorG, colorB });
	}

	void UITextBox::UpdateText(std::string sText)
	{
		if (trueValue.size())
		{
			if (trueValue.at(trueValue.size() - 1) == '|')
				trueValue = trueValue.substr(0, trueValue.size() - 1);
		}

		trueValue += sText;

		if (name == "txtPassword")
		{
			std::string sMask = "";

			for (int i = 0; i < trueValue.size(); i++)
			{
				sMask += '*';
			}

			displayValue = sMask;
		}
		else
		{
			displayValue = trueValue;
		}

		m_pMessage->LoadTextureFromText(displayValue, font, { colorR, colorG, colorB });
	}

	void UITextBox::RemoveLetter()
	{
		trueValue = trueValue.substr(0, trueValue.size() - 1);

		if (name == "txtPassword")
		{
			std::string sMask = "";

			for (int i = 0; i < trueValue.size(); i++)
			{

				if (trueValue.size() - 1 == i && trueValue.at(trueValue.size() - 1) == '|')
					break;

				sMask += '*';
			}

			displayValue = sMask;
		}
		else
		{
			displayValue = trueValue;
		}

		m_pMessage->LoadTextureFromText(displayValue, font, { colorR, colorG, colorB });
	}

	void UITextBox::RemoveAll()
	{
		trueValue = "";
		displayValue = trueValue;

		m_pMessage->LoadTextureFromText(displayValue, font, { colorR, colorG, colorB });
	}

	void UITextBox::RemoveFocus()
	{
		if (focused)
		{
			focused = false;

			if (trueValue.size() > 0)
			{
				Prettify(false);
			}	
		}
	}

	void UITextBox::Prettify(bool bCaret)
	{
		if (!bCaret)
		{
			if (displayValue.size() > 0)
			{
				if (displayValue.at(displayValue.size() - 1) == '|')
					displayValue = displayValue.substr(0, displayValue.size() - 1);
			}
		}
		else
		{
			displayValue = displayValue + '|';
		}

		m_pMessage->LoadTextureFromText(displayValue, font, { colorR, colorG, colorB });

	}

	// UI Button

	UIButton::UIButton(node n, UILayer& l) : UIControl(n, l)
	{
		auto sBtnBase = n.GetNode(n, "ButtonBase").get_string();
		auto sBtnHover = n.GetNode(n, "ButtonHover").get_string();
		auto sBtnClick = n.GetNode(n, "ButtonClick").get_string();
		auto sBtnFocus = n.GetNode(n, "ButtonFocus").get_string();
		
		auto root = fpx::fUI.root();

		if (sBtnBase != "")
		{
			auto spr = root.GetNode(root, "SpriteSheets/" + sBtnBase).get_bitmap();

			m_pSprBase = new Sprite();
			m_pSprBase->LoadTextureFromBitmap(sBtnBase, spr.data(), spr.width(), spr.height());
			//m_pSprBase->SetSpriteSize(width, height);
		}
		
		if (sBtnHover != "")
		{
			auto spr = root.GetNode(root, "SpriteSheets/" + sBtnHover).get_bitmap();

			m_pSprHover = new Sprite();
			m_pSprHover->LoadTextureFromBitmap(sBtnHover, spr.data(), spr.width(), spr.height());
		}
		
		if (sBtnClick != "")
		{
			auto spr = root.GetNode(root, "SpriteSheets/" + sBtnClick).get_bitmap();

			m_pSprClick = new Sprite();
			m_pSprClick->LoadTextureFromBitmap(sBtnClick, spr.data(), spr.width(), spr.height());
		}

		if (sBtnFocus != "")
		{
			auto spr = root.GetNode(root, "SpriteSheets/" + sBtnFocus).get_bitmap();

			m_pSprFocus = new Sprite();
			m_pSprFocus->LoadTextureFromBitmap(sBtnFocus, spr.data(), spr.width(), spr.height());
		}
	}

	UIButton::~UIButton()
	{
	}

	void UIButton::Process()
	{
	}

	void UIButton::Render()
	{
		// Render based on eState
		// 0 = Default base
		// 1 = Hovering
		// 2 = Clicked/Active
		// 3 = Focused

		switch (btnState)
		{
			case 0:
				if (m_pSprBase != NULL)
					m_pSprBase->Draw({ mapx - View::fx,  mapy - View::fy, width, height }, window::gRenderer);
				break;
			case 1:
				if (m_pSprHover != NULL)
					m_pSprHover->Draw({ mapx - View::fx,  mapy - View::fy, width, height }, window::gRenderer);
				break;
			case 2:
				if (m_pSprClick != NULL)
					m_pSprClick->Draw({ mapx - View::fx,  mapy - View::fy, width, height }, window::gRenderer);
				break;
			case 3:
				if (m_pSprFocus != NULL)
					m_pSprFocus->Draw({ mapx - View::fx, mapy - View::fy, width, height }, window::gRenderer);
				break;
			default:
				if (m_pSprBase != NULL)
					m_pSprBase->Draw({ mapx - View::fx, mapy - View::fy, width, height }, window::gRenderer);
				break;

		}
	}
}