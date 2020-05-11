#include "Session.hpp"
#include "Map.hpp"

#include <fstream>


namespace fpl
{
	#pragma comment(lib,"ws2_32.lib") //Winsock Library
	WSADATA wsaData;
	SOCKET gClientSocket;
	char recvData[4096] = "";
	bool Running = false;
	SessionState Session::State = eInit;
	const int packetMarker = 2; // ;$ = 2 char size

	const int bufferSize = 4096;

	std::string guid = "";

	void Session::init()
	{
		int iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);

		if (iResult != NO_ERROR)
		{
		}

		gClientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

		if (gClientSocket == INVALID_SOCKET)
		{
		}

		sockaddr_in clientService;

		clientService.sin_family = AF_INET;

		/* Home IP Address */
		InetPton(AF_INET, "192.168.1.24", &clientService.sin_addr.s_addr);

		/* Android Hotspot */
		//InetPton(AF_INET, "192.168.43.87", &clientService.sin_addr.s_addr);
		
		/* LAN base address */
		//InetPton(AF_INET, "127.0.0.1", &clientService.sin_addr.s_addr);
		
		clientService.sin_port = htons(29278);

		if (connect(gClientSocket, (SOCKADDR*)&clientService, sizeof(clientService)) == SOCKET_ERROR)
		{
			return;
		}

		Running = true;
		std::thread thrdSess(Recieve);
		thrdSess.detach();
	}
	void Session::Process()
	{
	}
	void Session::Recieve()
	{
		while(Running)
		{
			recv(gClientSocket, recvData, bufferSize, 0);

			ProcessData(recvData);

			memset(recvData, 0, sizeof(recvData));
			
			//recv(gClientSocket, recvData, 200, 0);
		}
	}
	void Session::Send(const char* data)
	{
		std::string sPacket = guid + ";$" + std::to_string(ProcessState()) + ";$" + data;

		const char *sendBuf = sPacket.c_str();

		int iResult = send(gClientSocket, sendBuf, bufferSize, 0);

		if (iResult == SOCKET_ERROR) {
			printf("send failed: %d\n", WSAGetLastError());
			closesocket(gClientSocket);
			WSACleanup();
			//return 1;
		}
	}
	void Session::SetState(SessionState eState)
	{
		State == eState;
	}

	void Session::ProcessData(char* data)
	{
		std::string read = data;
		Packet recvPacket;

		unsigned int track = 0;

		while (read.size() != 0)
		{
			unsigned int first = read.find_first_of(";$");

			std::string strip = read.substr(0, first);
			read.erase(0, strip.size() + packetMarker);

			switch (track)
			{
			case 0:
				recvPacket.ID = strip;
				break;
			case 1:
				recvPacket.Type = std::stoi(strip);
				break;
			case 2:
				 recvPacket.Data = new char[4096];


				 strcpy_s(recvPacket.Data, 4096, strip.c_str());   //strip.size(),strip.c_str());
				 break;
			default:
				break;
			}
			if(track < 3)
				track++;
		}

		if (recvPacket.Type == 0)
		{
			State = eInit;

			guid = recvPacket.ID;
		}
		else if (recvPacket.Type == 2)
		{
			State = eLoginSuccess;
		}
		else if (recvPacket.Type == 3)
		{
			State = eLoginFailure;
		}
		else if (recvPacket.Type == 9)
		{
			State = eCharacterCreateSuccess;
		}
		else if (recvPacket.Type == 10)
		{
			State = eCharacterCreateFail;
		}
	}

	int Session::ProcessState()
	{
		int iState = 0;

		switch (State)
		{
		case fpl::eInit:
			iState = 0;
			break;
		case fpl::eLogin:
			iState = 1;
			break;
		case fpl::eCharacterSelect:
			iState = 6;
			break;
		case fpl::eCharacterCreate:
			iState = 8;
			break;
		case fpl::eWorldSelect:
			iState = 0;
			break;
		case fpl::eChannelSelect:
			iState = 0;
			break;
		case fpl::eLoading:
			iState = 0;
			break;
		case fpl::ePlaying:
			iState = 0;
			break;
		default:
			break;
		}

		return iState;
	}
}

