#include "Time.hpp"
#include "Window.hpp"
#include <GL/glew.h>
#include <deque>
#include <chrono>
#include <thread>

namespace fpl
{
	namespace time
	{
		using clock = std::chrono::high_resolution_clock;
		using seconds_double = std::chrono::duration<double>;
		using namespace std::chrono_literals;
		using std::chrono::duration_cast;
		unsigned fps = 0;
		double delta = 0, delta_total = 0;
		clock::time_point first{ clock::now() };
		std::deque<clock::time_point> frames{ first };

		void reset() {
			first = clock::now();
			delta_total = 0;
		}

		void draw() {
			//if (!config::debug) { return; }
			auto const xscale = 8;
			auto const yscale = 8192;

			SDL_SetRenderDrawColor(window::gRenderer, 255, 0, 0, SDL_ALPHA_OPAQUE);
			SDL_RenderDrawLine(window::gRenderer, 0, yscale / 60, static_cast<GLint>(frames.size() * xscale), yscale / 60);
			SDL_SetRenderDrawColor(window::gRenderer, 0, 0, 0, SDL_ALPHA_OPAQUE);
			for (auto i = 1u; i < frames.size(); ++i) {

				if (i + 1 != frames.size())
				{
					auto t1 = duration_cast<seconds_double>(frames[i] - frames[i - 1]);
					auto t2 = duration_cast<seconds_double>(frames[i + 1] - frames[i]);
					SDL_RenderDrawLine(window::gRenderer, 
						static_cast<GLint>(i * xscale), 
						static_cast<GLint>(yscale * t1.count()),
						static_cast<GLint>(i + 1 * xscale),
						static_cast<GLint>(yscale * t2.count()));
				}
			}
			SDL_SetRenderDrawColor(window::gRenderer, 255, 255, 255, SDL_ALPHA_OPAQUE);
			for (auto i = 1u; i < frames.size(); ++i) {

				if (i + 1 != frames.size())
				{
					auto t1 = duration_cast<seconds_double>(frames[i] - frames[i - 1]);
					auto t2 = duration_cast<seconds_double>(frames[i + 1] - frames[i]);
					SDL_RenderDrawLine(window::gRenderer,
						static_cast<GLint>(i * xscale),
						static_cast<GLint>(yscale * t1.count()),
						static_cast<GLint>(i + 1 * xscale),
						static_cast<GLint>(yscale * t2.count()));
				}
			}		
		}

		void update() {
			auto last = frames.back();
			//if (config::target_fps <= 0) { config::target_fps = 1; } // Because some people are idiots
			auto step_size = duration_cast<clock::duration>(1s) / 120;
			//auto step_size = duration_cast<clock::duration>(1s) / config::target_fps;
			auto pre = clock::now();
			/*if (config::limit_fps && pre - last + 1ms < step_size) {
				std::this_thread::sleep_for(step_size - (pre - last + 1ms));
			}*/
			auto now = clock::now();
			delta = duration_cast<seconds_double>(now - last).count();
			if (delta < 0) { delta = 0; }
			if (delta > 0.05) { delta = 0.05; }
			delta_total = duration_cast<seconds_double>(now - first).count();
			while (!frames.empty() && now - frames.front() > 1s) { frames.pop_front(); }
			frames.push_back(now);
			fps = static_cast<unsigned>(frames.size());

			SDL_SetRenderDrawColor(window::gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
			SDL_RenderClear(window::gRenderer);
			//draw();
		}
	}
}