// This include:
#include "Game.hpp"
#include "Window.hpp"
#include "Time.hpp"
#include "Sound.hpp"
#include "Map.hpp"

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
	const int WINDOW_WIDTH = 1440;
	const int WINDOW_HEIGHT = 1000;

	const int LEVEL_WIDTH = 1400;
	const int LEVEL_HEIGHT = 1050;

	const int CAMERA_WIDTH = WINDOW_WIDTH;
	const int CAMERA_HEIGHT = WINDOW_HEIGHT;

	SDL_Rect camera = { 0, 0, CAMERA_WIDTH, CAMERA_HEIGHT };

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
		window::init();
		time::reset();
		fpx::load_all();
		sound::init();
		map::init();
		window::recreate(false);





	/*	m_currMap = new Map();
		m_currMap->LoadLevel("0", *window::gRenderer);

		Sprite* bg2 = new Sprite();
		bg2->LoadTexture("content\\player\\player.png");
		bg2->EnableAnimation(2, 3, 96);


		m_Player = new Player();
		m_Player->Initialize(*bg2);
		m_Player->SetPositionX(100);
		m_Player->SetPositionY(100);
		m_Player->SetLevelBounds(m_currMap->GetLevelWidth(), m_currMap->GetLevelHeight());*/

		return(true);
	}

	bool Game::DoGameLoop()
	{

		// Check input loop
		assert(gInputHandler);
		gInputHandler->ProcessInput(*m_Player);

		if (m_looping)
		{
			// Draw Objects
			Process();
			Draw();
		}

		SDL_Delay(1);
		return(m_looping);
	}

	void
		Game::Process()
	{

		time::update();

		m_Player->Process(time::delta);

		camera.x = m_Player->GetPositionX() - (CAMERA_WIDTH / 2);
		camera.y = m_Player->GetPositionY() - (CAMERA_HEIGHT / 2);

	/*	if (camera.x < 0)
			camera.x = 0;
		if (camera.y < 0)
			camera.y = 0;
		if (camera.x > (m_currMap->GetLevelWidth() - (CAMERA_WIDTH)))
			camera.x = m_currMap->GetLevelWidth() - (CAMERA_WIDTH);
		if (camera.y > (m_currMap->GetLevelHeight() - (CAMERA_HEIGHT)))
			camera.y = m_currMap->GetLevelHeight() - (CAMERA_HEIGHT);*/

		m_Player->SendCamera(camera.x, camera.y);
		/*m_currMap->SendCamera(camera.x, camera.y);
		m_currMap->Process(time::delta);*/
	}

	void Game::Draw()
	{
		//m_currMap->Draw(*window::gRenderer);
		m_Player->Draw(*window::gRenderer);

		//time::draw();
		SDL_RenderPresent(window::gRenderer);
	}

	void
		Game::Quit()
	{
		m_looping = false;
	}
}

