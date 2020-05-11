#pragma once
#include "node.hpp"

namespace fpl {
	// Internal data structure
#pragma pack(push, 1)
	struct node::data {
		uint32_t const name;
		uint32_t const children;
		uint16_t const num;
		node::type const type;
		union {	
			int64_t const ireal;
			double const dreal;
			uint32_t const string;
			int32_t const vector[2];
			struct {
				uint32_t index;
				uint16_t width;
				uint16_t height;
			} const bitmap;
			struct {
				uint32_t index;
				uint32_t length;
			} const audio;
		};
	};
#pragma pack(pop)
}