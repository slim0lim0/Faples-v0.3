#pragma once
#include "Packet.hpp"

#include <WinSock2.h>
#include <Windows.h>
#include <Ws2tcpip.h>
#include <thread>

namespace fpl
{
	enum SessionState
	{
		eInit,   //0

		eLogin,   // 1
		eLoginSuccess,  // 2
		eLoginFailure,  // 3

		eWorldSelect,  // 4

		eChannelSelect,  // 5

		eCharacterSelect, // 6  - Character Play, Create, Delete
		eCharacterPlay,
		eCharacterCreateMode,
		eCharacterDeleteMode,

		eCharacterCreate,
		eCharacterCreateSuccess,
		eCharacterCreateFail,

		eCharacterDelete,
		eCharacterDeleteSuccess,
		eCharacterDeleteFail,



		eLoading,
		ePlaying
	};

	class Session
	{
	public:
		static void init();
		static void Process();
		static void Recieve();
		static void Send(const char* data);
		static void SetState(SessionState eState);

		static SessionState State;
	private:
		static void ProcessData(char* data);
		static int ProcessState();		
	};
	extern WSADATA wsaData;
	extern SOCKET gClientSocket;
	extern char recvData[4096];
	extern bool Running;
	extern std::string guid;
}