#pragma once
#include "fpxfwd.hpp"
#include <cstdint>
#include <cstddef>

namespace fpl {
	class audio {
	public:
		audio() = default;
		audio(audio const &) = default;
		audio & operator=(audio const &) = default;
		// Comparison operators, useful for containers
		bool operator==(audio const &) const;
		bool operator<(audio const &) const;
		// Returns whether the audio is valid or merely null
		explicit operator bool() const;
		// Does not do any sort of decompression
		// Do not free the pointer returned by this method
		// The pointer remains valid until the file this audio is part of is destroyed
		void const * data() const;
		uint32_t length() const;
		// Returns a unique id, useful for keeping track of what audio you loaded
		size_t id() const;

	private:
		audio(void const *, uint32_t);
		void const * m_data = nullptr;
		uint32_t m_length = 0;
		friend node;
	};
}