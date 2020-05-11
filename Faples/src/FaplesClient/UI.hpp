#pragma once

#include "Window.hpp"
#include "Sprite.hpp"
#include "UIGroup.hpp"

#include <FaplesFpx/node.hpp>
#include <FaplesFpx/bitmap.hpp>
#include <vector>
#include <map>

namespace fpl
{
	namespace ui 
	{
		void init();
		void Process();
		void Render();
		void Clear();
		void LoadUI(std::string sUIName);

		// Game specific Functions
		// Error Handling
		void ReportError(std::string sError, std::string sFallback);
		void CloseError(int iErrorIndex);
		// Login
		void ExecuteLogin();
		// Character Selection
		void PlayCharacter();
		void CreateMode();
		void DeleteCharacter();
		// Character Creation
		void CreateCharacter();
		void SelectMode();

		extern node current;
		extern std::vector<UIGroup*> uiGroup;
		extern std::string currUiType;
	}
}