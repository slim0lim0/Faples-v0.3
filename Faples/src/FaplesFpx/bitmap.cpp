#include "bitmap.hpp"
#include <lz4.h>
#include <vector>

namespace fpl {
	bitmap::bitmap(void const * d, uint16_t w, uint16_t h) : m_data(d), m_width(w), m_height(h) {}
	bool bitmap::operator<(bitmap const & o) const { return m_data < o.m_data; }
	bool bitmap::operator==(bitmap const & o) const { return m_data == o.m_data; }
	bitmap::operator bool() const { return m_data ? true : false; }
	std::vector<char> bitmap_buf;
	void const * bitmap::data() const {
		if (!m_data) return nullptr;
		auto const l = length();
		if (l + 0x20 > bitmap_buf.size()) bitmap_buf.resize(l + 0x20);
		::LZ4_decompress_safe(4 + reinterpret_cast<char const *>(m_data), bitmap_buf.data(),
			static_cast<int>(l), static_cast<int>(l));
		return bitmap_buf.data();
	}
	uint16_t bitmap::width() const { return m_width; }
	uint16_t bitmap::height() const { return m_height; }
	uint32_t bitmap::length() const { return 4u * m_width * m_height; }
	size_t bitmap::id() const { return reinterpret_cast<size_t>(m_data); }
}
