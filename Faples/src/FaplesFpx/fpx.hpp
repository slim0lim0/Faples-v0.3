#pragma once
#include "fpxfwd.hpp"

namespace fpl {
	namespace fpx {
		// Pre-defined nodes to access standard MapleStory style data
		// Make sure you called load_all first
		/*extern node base, character, effect, etc, item, map, mob, morph, npc, quest, reactor, skill, sound,
			string, tamingmob, ui;*/
		extern node fMap, fAudio, fUI, fCharacter;
		// Loads the pre-defined nodes from a standard setup of nx files for MapleStory
		// Only call this function once
		void load_all();
	}
}