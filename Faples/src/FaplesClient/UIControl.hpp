#pragma once
#include <FaplesFpx/node.hpp>
#include "Sprite.hpp"

namespace fpl
{
	class UILayer;

	class UIControl
	{
	public:
		UIControl(node n, UILayer& l);
		~UIControl();
		virtual void Process() = 0;
		virtual void Render() = 0;
		void SetLocation(int x, int y);
		void Reposition();

		int x, y, mapx, mapy, width, height;
		std::string name;
		std::string type;
		std::string uiType;

		UILayer& layer;
	};

	class UITextBox : public UIControl
	{
		public:
			UITextBox(node n, UILayer& l);
			~UITextBox();
			void Process();
			void Render();
			void ReplaceText(std::string sText);
			void UpdateText(std::string sText);
			void RemoveLetter();
			void RemoveAll();
			void RemoveFocus();
			void Prettify(bool bCaret);

			bool multiline, label;
			std::string fontFamily, displayValue, trueValue;
			int fontSize;
			uint8_t colorR, colorG, colorB;
			TTF_Font* font;
			Sprite* m_pMessage;
			bool focused = false;
		private:
			float fFocusBarTick = 0.0;
			bool bFocusBar = false;
	};

	class UIButton : public UIControl
	{
	public:
		UIButton(node n, UILayer& l);
		~UIButton();
		void Process();
		void Render();


		int btnState = 0;
	private:
		Sprite* m_pSprBase;
		Sprite* m_pSprHover;
		Sprite* m_pSprClick;
		Sprite* m_pSprFocus;
	};

	/*class UICursor : public UIControl
	{
	public:
		UICursor(node n);
		~UICursor();
		void Process();
	};*/
}
