#pragma once
#include "file.hpp"
#include "node_impl.hpp"

namespace fpl {
#pragma pack(push, 1)
	struct file::header {
		uint32_t const magic;
		uint32_t const node_count;
		uint64_t const node_offset;
		uint32_t const string_count;
		uint64_t const string_offset;
		uint32_t const bitmap_count;
		uint64_t const bitmap_offset;
		uint32_t const audio_count;
		uint64_t const audio_offset;
	};
#pragma pack(pop)
	struct _file_data {
		void const * base = nullptr;
		node::data const * node_table = nullptr;
		uint64_t const * string_table = nullptr;
		uint64_t const * bitmap_table = nullptr;
		uint64_t const * audio_table = nullptr;
		file::header const * header = nullptr;
#ifdef _WIN32
		void * file_handle = nullptr;
		void * map = nullptr;
#else
		int file_handle = 0;
		size_t size = 0;
#endif
	};
}
