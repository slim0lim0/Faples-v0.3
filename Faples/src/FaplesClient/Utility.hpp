#pragma once
namespace fpl
{
	namespace utility
	{
		typedef struct _GUID {
			uint32_t Data1;
			uint16_t Data2;
			uint16_t Data3;
			uint8_t Data4[8];
		} GUID;

		GUID StringToGuid(const std::string& str)
		{
			GUID guid;
			sscanf_s(str.c_str(),
				"{%8x-%4hx-%4hx-%2hhx%2hhx-%2hhx%2hhx%2hhx%2hhx%2hhx%2hhx}",
				&guid.Data1, &guid.Data2, &guid.Data3,
				&guid.Data4[0], &guid.Data4[1], &guid.Data4[2], &guid.Data4[3],
				&guid.Data4[4], &guid.Data4[5], &guid.Data4[6], &guid.Data4[7]);

			return guid;
		}

		std::string GuidToString(GUID guid)
		{
			char guid_cstr[39];
			snprintf(guid_cstr, sizeof(guid_cstr),
				"{%08x-%04x-%04x-%02x%02x-%02x%02x%02x%02x%02x%02x}",
				guid.Data1, guid.Data2, guid.Data3,
				guid.Data4[0], guid.Data4[1], guid.Data4[2], guid.Data4[3],
				guid.Data4[4], guid.Data4[5], guid.Data4[6], guid.Data4[7]);

			return std::string(guid_cstr);
		}
	}
	
}