// This include:
#include "Game.hpp"
#include "Window.hpp"
#include "Time.hpp"
#include "Sound.hpp"
#include "Map.hpp"
#include "View.hpp"
#include "Session.hpp"
#include "UI.hpp"

// Local includes:
// Library Includes:
#include <cassert>
#include <SDL.h>
#include <SDL_image.h>
#include <string>
#include <FaplesFpx/fpx.hpp>

namespace fpl
{
	Game* Game::sm_pInstance = 0;

	Game&
		Game::GetInstance()
	{

		if (sm_pInstance == 0)
		{
			sm_pInstance = new Game();
		}

		assert(sm_pInstance);

		return(*sm_pInstance);
	}

	void
		Game::DestroyInstance()
	{
		delete sm_pInstance;
		sm_pInstance = 0;
	}

	Game::Game()
		: m_looping(true)
	{

	}

	Game::~Game()
	{

	}

	bool
	Game::Initialise()
	{
		gInputHandler = new InputHandler();
		Session::init();
		window::init();
		time::reset();
		fpx::load_all();
		ui::init();
		sound::init();
		map::init();
		window::recreate(false);

		//Session::State = ePlaying;

		return(true);
	}

	bool Game::DoGameLoop()
	{
		// Check input loop
		assert(gInputHandler);
		gInputHandler->ProcessInput();

		if (m_looping)
		{
			// Draw Objects
			Process();
			Draw();
			time::update();
		}

		SDL_Delay(1);
		return(m_looping);
	}

	void Game::Process()
	{
		Session::Process();

		if (Session::State == eInit)
		{
			ui::LoadUI("login");
			Session::State = eLogin;
		}
		else if (Session::State == eLogin)
		{
			ui::Process();
		}
		else if (Session::State == eLoginSuccess)
		{
			ui::Clear();
			gInputHandler->bUIFocused = false;

			ui::LoadUI("charselect1");		
			Session::State = eCharacterSelect;
		}
		else if (Session::State == eLoginFailure)
		{
			ui::ReportError("Login failed!", "Please try again.");
			Session::State = eLogin;
		}
		else if (Session::State == eCharacterSelect)
		{
			ui::Process();
		}
		else if (Session::State == eCharacterCreateMode)
		{
			ui::Clear();
			gInputHandler->bUIFocused = false;

			ui::LoadUI("charcreate1");
			Session::State = eCharacterCreate;
		}
		else if (Session::State == eCharacterCreateSuccess)
		{
			ui::Clear();
			gInputHandler->bUIFocused = false;

			ui::LoadUI("charselect1");
			Session::State = eCharacterSelect;
		}
		else if (Session::State == eCharacterCreateFail)
		{
			ui::ReportError("Character creation failed!", "Please try again.");
			Session::State = eCharacterSelect;
		}
		else if (Session::State == eCharacterPlay)
		{
			ui::Clear();
			gInputHandler->bUIFocused = false;

			map::Load("0100");
			map::SpawnPlayer(nullptr);

			Session::State = ePlaying;
		}
		else if (Session::State == ePlaying)
		{
			View::Update();
			map::Update();
		}
	}

	void Game::Draw()
	{
		if (Session::State == eLogin || Session::State == eCharacterSelect || Session::State == eCharacterCreate)
		{
			ui::Render();
		}
		else if (Session::State == ePlaying)
		{
			map::Render();
		}
		

		SDL_RenderPresent(window::gRenderer);
	}

	void
		Game::Quit()
	{
		m_looping = false;
	}
}

