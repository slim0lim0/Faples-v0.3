#include "audio.hpp"

namespace fpl {
	audio::audio(void const * d, uint32_t l) : m_data(d), m_length(l) {}
	bool audio::operator<(audio const & o) const { return m_data < o.m_data; }
	bool audio::operator==(audio const & o) const { return m_data == o.m_data; }
	audio::operator bool() const { return m_data ? true : false; }
	void const * audio::data() const { return m_data; }
	uint32_t audio::length() const { return m_length; }
	size_t audio::id() const { return reinterpret_cast<size_t>(m_data); }
}