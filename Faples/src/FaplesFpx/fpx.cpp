#include "fpx.hpp"
#include "file.hpp"
#include "node.hpp"
#include <fstream>
#include <vector>
#include <memory>
#include <stdexcept>
namespace fpl {
	namespace fpx {
		std::vector<std::unique_ptr<file>> files;
		bool exists(std::string name) { return std::ifstream(name).is_open(); }
		node add_file(std::string name) {
			if (!exists(name)) return {};
			files.emplace_back(new file(name));
			return *files.back();
		}
		node fBase, fMap, fAudio, fUI, fCharacter;

		void load_all() {
			if (exists("Map.fpx")) {
				fMap = add_file("Map.fpx");
				fAudio = add_file("Audio.fpx");
				fUI = add_file("UI.fpx");
				fCharacter = add_file("Character.fpx");
			}
			else if (exists("Data.fpx")) {
				fBase = add_file("Data.fpx");
				fMap = fBase["Map"];
				fAudio = fBase["Audio"];
				fUI = fBase["UI"];
				fCharacter = fBase["Character"];
			}
			else { throw std::runtime_error("Failed to locate fpx files."); }
		}
	}
}
