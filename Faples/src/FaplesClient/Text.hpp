#pragma once
#include <string>

namespace fpl
{
	class Text
	{
		Text();
		~Text();
		void LoadFont(std::string font);
		void Process();
		void Render();
		void UpdateText();
	private:
		bool Changed;
	};
}