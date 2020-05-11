#pragma once
namespace fpl {
	namespace time {
		extern unsigned fps;
		extern double delta, delta_total;
		void reset();
		void update();
		void draw();
	}
}