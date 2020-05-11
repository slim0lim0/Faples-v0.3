#pragma once
#include "fpxfwd.hpp"
#include <cstdint>
#include <string>

namespace fpl {
	struct _file_data;
	class file {
	public:
		typedef _file_data data;
		struct header;
		// Creates a null file object
		// Nothing can really be done until you call open()
		file() = default;
		// Used to construct an nx file from a filename
		// Multiple file objects can be created from the same filename
		// and the resulting nodes are interchangeable
		file(std::string name);
		// Destructor calls close()
		~file();
		// Files cannot be copied
		file(file const &) = delete;
		// Files cannot be copied
		file & operator=(file const &) = delete;
		// Transfers ownership of the file handle to another file
		file(file &&);
		// Transfers ownership of the file handle to another file
		file & operator=(file &&);
		// Opens the file with the given name
		void open(std::string name);
		// Closes the given file
		// Any nodes derived from this file become invalid after closing it
		// Any attempts to use invalid nodes will result in undefined behavior
		void close();
		// Obtains the root node from which all other nodes may be accessed
		// If the file is not open, a null node is returned
		node root() const;
		// Effectivelly calls root()
		operator node() const;
		// Returns the number of strings in the file
		uint32_t string_count() const;
		// Returns the number of bitmaps in the file
		uint32_t bitmap_count() const;
		// Returns the number of audios in the file
		uint32_t audio_count() const;
		// Returns the number of nodes in the file
		uint32_t node_count() const;
		// Returns the string with a given id number
		std::string get_string(uint32_t) const;

	private:
		data * m_data = nullptr;
		friend node;
		friend bitmap;
		friend audio;
	};
}