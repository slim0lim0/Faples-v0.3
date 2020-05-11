// This include
#include "InputHandler.hpp"
#include "Map.hpp"
#include "Session.hpp"
#include "Window.hpp"
#include "UI.hpp"
#include "Map.hpp"

// Library Includes
#include <SDL.h>

namespace fpl
{
	InputHandler::InputHandler()
	{
		PrepareTypeInputs();
	}

	InputHandler::~InputHandler()
	{

	}

	bool jumpPressed = false;
	void InputHandler::ProcessInput()
	{
		SDL_Event e;
		while (SDL_PollEvent(&e)) {
			switch (e.type)
			{
				case SDL_MOUSEMOTION:
				{
					int x, y;
					SDL_GetMouseState(&x, &y);


					for (UIGroup* group : ui::uiGroup) {
						for (UILayer* layer : group->GetLayers())
						{
							for (UIControl* control : layer->oControls)
							{
								if (x > control->mapx && x < (control->mapx + control->width) && y > control->mapy && y < (control->mapy + control->height))
								{
									if (control->type == "Button")
										dynamic_cast<UIButton*>(control)->btnState = 1;
								}
								else
								{
									if (control->type == "Button")
										dynamic_cast<UIButton*>(control)->btnState = 0;
								}
							}
						}
					}					
					
				}
				case SDL_MOUSEBUTTONDOWN:
				{	
					int x, y;
					SDL_GetMouseState(&x, &y);

					if (e.button.button == SDL_BUTTON_LEFT) {
						for (int i = 0; i < ui::uiGroup.size(); i++)
						{
							UIGroup* group = ui::uiGroup[i];

							for (UILayer* layer : group->GetLayers())
							{
								for (UIControl* control : layer->oControls)
								{
									if (control->type == "Textbox")
									{
										dynamic_cast<UITextBox*>(control)->RemoveFocus();
									}

									if (x > control->mapx && x < (control->mapx + control->width) && y > control->mapy && y < (control->mapy + control->height))
									{
										if (control->type == "Textbox" && control)
										{
											if (!dynamic_cast<UITextBox*>(control)->label)
											{
												dynamic_cast<UITextBox*>(control)->focused = true;
												bUIFocused = true;
											}
										}
										else if (control->type == "Button")
										{
											dynamic_cast<UIButton*>(control)->btnState = 2;

											if (group->name == "error" && control->name == "btnOK")
											{
												ui::CloseError(i);
											}
											else if (Session::State == eLogin)
											{
												if (dynamic_cast<UIButton*>(control)->uiType == "login" && dynamic_cast<UIButton*>(control)->name == "btnLogin")		{
													ui::ExecuteLogin();
												}
											}
											else if (Session::State == eCharacterSelect)
											{
												if (dynamic_cast<UIButton*>(control)->uiType == "charselect1" && dynamic_cast<UIButton*>(control)->name == "btnPlay")		{
													ui::PlayCharacter();
												}
												else if (dynamic_cast<UIButton*>(control)->uiType == "charselect1" && dynamic_cast<UIButton*>(control)->name == "btnCreate") {
													ui::CreateMode();
												}
											}
											else if (Session::State == eCharacterCreate)
											{
												if (dynamic_cast<UIButton*>(control)->uiType == "charcreate1" && dynamic_cast<UIButton*>(control)->name == "btnCreate") {
													ui::CreateCharacter();
												}
												else if (dynamic_cast<UIButton*>(control)->uiType == "charcreate1" && dynamic_cast<UIButton*>(control)->name == "btnCancel") {
													ui::SelectMode();
												}
											}
										}
									}
								}
							}
						}
						break;
					}				
				}
				case SDL_MOUSEBUTTONUP:
				{
					int x, y;
					SDL_GetMouseState(&x, &y);

					if (e.button.button == SDL_BUTTON_LEFT) {
						for (UIGroup* group : ui::uiGroup)
						{
							for (UILayer* layer : group->GetLayers())
							{
								for (UIControl* control : layer->oControls)
								{
									if (control->type == "Button")
									{
										if (dynamic_cast<UIButton*>(control)->btnState == 2)
										{
											if (x > control->mapx && x < (control->mapx + control->width) && y > control->mapy && y < (control->mapy + control->height))
											{
												if (control->type == "Button")
													dynamic_cast<UIButton*>(control)->btnState = 1;
											}
											else
											{
												if (control->type == "Button")
													dynamic_cast<UIButton*>(control)->btnState = 0;
											}
										}
									}
								}
							}
						}
						break;
					}
				}
				case SDL_KEYDOWN:
				{
					SDL_Keycode event = e.key.keysym.sym;
					if(bUIFocused)
					{ 
						for (UIGroup* group : ui::uiGroup)
						{
							for (UILayer* layer : group->GetLayers())
							{
								for (UIControl* control : layer->oControls)
								{
									if (control->type == "Textbox" && event >= 0 && event < 322)
									{
										if (dynamic_cast<UITextBox*>(control)->focused)
										{
											if (event == SDLK_BACKSPACE)
											{
												dynamic_cast<UITextBox*>(control)->RemoveLetter();												
											}
											else
											{
												if(std::find(typeInputs.begin(), typeInputs.end(), event) != typeInputs.end())
													dynamic_cast<UITextBox*>(control)->UpdateText(SDL_GetKeyName(event));
											}
										}

									}
								}
							}
						}
						break;
					}
					else if (Session::State == ePlaying)
					{
						auto player = map::GetLocalPlayer();
						if (event == SDLK_UP)
						{
							player->MOVE_UP = true;
							break;
						}
						if (event == SDLK_DOWN)
						{
							player->MOVE_DOWN = true;
							break;
						}
						if (event == SDLK_LEFT)
						{
							player->MOVE_LEFT = true;
							break;
						}
						if (event == SDLK_RIGHT)
						{
							player->MOVE_RIGHT = true;
							break;
						}
						if (event == SDLK_LALT)
						{
							player->JUMPING = true;
							break;
						}
					}		

				}
				case SDL_KEYUP:
				{
					SDL_Keycode event = e.key.keysym.sym;
					if (Session::State == ePlaying)
					{
						auto player = map::GetLocalPlayer();
						if (event == SDLK_UP)
						{
							player->MOVE_UP = false;
							break;
						}
						if (event == SDLK_DOWN)
						{
							player->MOVE_DOWN = false;
							break;
						}
						if (event == SDLK_LEFT)
						{
							player->MOVE_LEFT = false;
							break;
						}
						if (event == SDLK_RIGHT)
						{
							player->MOVE_RIGHT = false;
							break;
						}
						if (event == SDLK_LALT)
						{
							break;
						}
					}
				
				}
				default:
				{
					e.key.keysym.sym = 0;
					break;
				}
			}
		}
	}

	void InputHandler::PrepareTypeInputs()
	{
		// Add in key checks for all relevant keys

		// Add numbers
		typeInputs.push_back(SDLK_0);
		typeInputs.push_back(SDLK_1);
		typeInputs.push_back(SDLK_2);
		typeInputs.push_back(SDLK_3);
		typeInputs.push_back(SDLK_4);
		typeInputs.push_back(SDLK_5);
		typeInputs.push_back(SDLK_6);
		typeInputs.push_back(SDLK_7);
		typeInputs.push_back(SDLK_8);
		typeInputs.push_back(SDLK_9);

		// Add letters
		typeInputs.push_back(SDLK_a);
		typeInputs.push_back(SDLK_b);
		typeInputs.push_back(SDLK_c);
		typeInputs.push_back(SDLK_d);
		typeInputs.push_back(SDLK_e);
		typeInputs.push_back(SDLK_f);
		typeInputs.push_back(SDLK_g);
		typeInputs.push_back(SDLK_h);
		typeInputs.push_back(SDLK_i);
		typeInputs.push_back(SDLK_j);
		typeInputs.push_back(SDLK_k);
		typeInputs.push_back(SDLK_l);
		typeInputs.push_back(SDLK_m);
		typeInputs.push_back(SDLK_n);
		typeInputs.push_back(SDLK_o);
		typeInputs.push_back(SDLK_p);
		typeInputs.push_back(SDLK_q);
		typeInputs.push_back(SDLK_r);
		typeInputs.push_back(SDLK_s);
		typeInputs.push_back(SDLK_t);
		typeInputs.push_back(SDLK_u);
		typeInputs.push_back(SDLK_v);
		typeInputs.push_back(SDLK_w);
		typeInputs.push_back(SDLK_x);
		typeInputs.push_back(SDLK_y);
		typeInputs.push_back(SDLK_z);

		// Add number symbols
		typeInputs.push_back(SDLK_EXCLAIM);
		typeInputs.push_back(SDLK_AT);
		typeInputs.push_back(SDLK_HASH);
		typeInputs.push_back(SDLK_DOLLAR);
		typeInputs.push_back(SDLK_PERCENT);
		typeInputs.push_back(SDLK_CARET);
		typeInputs.push_back(SDLK_AMPERSAND);
		typeInputs.push_back(SDLK_ASTERISK);
		typeInputs.push_back(SDLK_LEFTPAREN);
		typeInputs.push_back(SDLK_RIGHTPAREN);

		// Add Enter region symbol collection
		typeInputs.push_back(SDLK_UNDERSCORE);
		typeInputs.push_back(SDLK_MINUS);
		typeInputs.push_back(SDLK_EQUALS);
		typeInputs.push_back(SDLK_PLUS);
		typeInputs.push_back(SDLK_LEFTBRACKET);
		typeInputs.push_back(SDLK_RIGHTBRACKET);
		typeInputs.push_back(SDLK_KP_LEFTBRACE);
		typeInputs.push_back(SDLK_KP_RIGHTBRACE);
		typeInputs.push_back(SDLK_COLON);
		typeInputs.push_back(SDLK_SEMICOLON);
		typeInputs.push_back(SDLK_SLASH);
		typeInputs.push_back(SDLK_BACKSLASH);
		typeInputs.push_back(SDLK_QUOTE);
		typeInputs.push_back(SDLK_QUOTEDBL);
		typeInputs.push_back(SDLK_PERIOD);
		typeInputs.push_back(SDLK_COMMA);
		typeInputs.push_back(SDLK_QUESTION);
		typeInputs.push_back(SDLK_KP_VERTICALBAR);
	}
}

