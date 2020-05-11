#pragma once
#include "fpxfwd.hpp"
#include <cstdint>
#include <cstddef>

namespace fpl {
	class bitmap {
	public:
		bitmap() = default;
		bitmap(bitmap const &) = default;
		bitmap & operator=(bitmap const &) = default;
		// Comparison operators, useful for containers
		bool operator==(bitmap const &) const;
		bool operator<(bitmap const &) const;
		// Returns whether the bitmap is valid or merely null
		explicit operator bool() const;
		// This function decompresses the data on the fly
		// Do not free the pointer returned by this method
		// Every time this function is called
		// any previous pointers returned by this method become invalid
		void const * data() const;
		uint16_t width() const;
		uint16_t height() const;
		uint32_t length() const;
		// Returns a unique id, useful for keeping track of what bitmaps you loaded
		size_t id() const;

	private:
		bitmap(void const *, uint16_t, uint16_t);
		void const * m_data = nullptr;
		uint16_t m_width = 0;
		uint16_t m_height = 0;
		friend node;
	};
}