#include "UI.hpp"
#include "Session.hpp"
#include "Window.hpp"
#include "View.hpp"

#include <FaplesFpx/fpx.hpp>
#include <cassert>


namespace fpl
{
	namespace ui
	{
		node current;
		std::vector<UIGroup*> uiGroup;
		std::string currUiType = "";

		void init()
		{
			current = fpx::fUI.root();
		}
		void Process()
		{
			for (UIGroup* group : uiGroup)
			{
				group->Process();
			}
		}
		void Render()
		{
			for (UIGroup* group : uiGroup)
			{
				group->Render();
			}
		}

		void Clear()
		{
			uiGroup.clear();
		}

		void LoadUI(std::string sUIName)
		{
			auto nNode = current.GetNode(current, "UI/" + sUIName);

			currUiType = sUIName;

			uiGroup.emplace_back(new UIGroup(nNode));
		}

		void ReportError(std::string sError, std::string sFallback) {
			LoadUI("error");
			for (UIGroup* group : uiGroup)
			{
				if (group->name == "error")
				{
					group->x = View::width / 2 - group->width / 2;
					group->y = View::height / 2 - group->height / 2;

					group->SetLocation(group->x, group->y);

					for (UILayer* layer : group->GetLayers())
					{
						for (UIControl* control : layer->oControls)
						{
							if (control->name == "txtErrorMessage")
							{
								dynamic_cast<UITextBox*>(control)->UpdateText(sError);
							}
							else if (control->name == "txtErrorFallback")
							{
								dynamic_cast<UITextBox*>(control)->UpdateText(sFallback);
							}
						}
					}
				}
			}
		}

		// Game specific Functions

		void CloseError(int iErrorIndex)
		{
			uiGroup.erase(uiGroup.begin() + iErrorIndex);
		}

		void ExecuteLogin()
		{
			std::string sData = ";$";

			for (UIGroup* group : uiGroup) {
				if (group->name == "login") {
					for (UILayer* layer : group->GetLayers())
					{
						for (UIControl* control : layer->oControls)
						{
							if (control->name == "txtUsername")
							{
								sData += dynamic_cast<UITextBox*>(control)->trueValue + ";$";
							}
							else if (control->name == "txtPassword")
							{
								sData += dynamic_cast<UITextBox*>(control)->trueValue + ";$";
							}
						}
					}

					break;
				}
			}

			const char* data = sData.c_str();

			Session::Send(data);
		}
		void PlayCharacter()
		{
			Session::State = eCharacterPlay;
		}
		void CreateMode()
		{
			Session::State = eCharacterCreateMode;
		}
		void SelectMode()
		{
			Session::State = eLoginSuccess;
		}
		void CreateCharacter()
		{
			//Session::State = eCharacterCreate;

			std::string sData = ";$";
			for (UIGroup* group : uiGroup) {
				if (group->name == "charcreate1") {
					for (UILayer* layer : group->GetLayers())
					{
						for (UIControl* control : layer->oControls)
						{
							if (control->name == "txtCharName")
							{
								sData += dynamic_cast<UITextBox*>(control)->trueValue + ";$";
							}
						}
					}

					break;
				}
			}

			const char* data = sData.c_str();

			Session::Send(data);
		}
	}
}