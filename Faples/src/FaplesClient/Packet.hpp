#pragma once

#include <string>
namespace fpl
{
	class Packet
	{
	public:
		std::string ID;
		int Type;
		char* Data;
	};	
}