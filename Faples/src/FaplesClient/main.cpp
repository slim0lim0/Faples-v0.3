// Local includes:
#include "Game.hpp"

// Library includes:
#include <WinSock2.h>
#include <Windows.h>
#include <Ws2tcpip.h>
#include <stdio.h>
#include <iostream>
#include <fstream>
#include <thread>
//#include <winsock.h>

namespace fpl
{
	void Start()
	{
		Game& gameInstance = Game::GetInstance();
		if (!gameInstance.Initialise())
		{
			return;
			// terminate
		}

		while (gameInstance.DoGameLoop())
		{
			// No body.
		}


		Game::DestroyInstance();
	}
}

#pragma comment(lib,"ws2_32.lib") //Winsock Library
void RunClient()
{
	std::ofstream clientLog;
	clientLog.open("ntClientLog.txt");
	clientLog << "Writing this to a file.\n\n";

	WSADATA wsaData;

	int iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);


	if (iResult != NO_ERROR)
	{
		clientLog << "Client: Error at WSAStartup().\n";
		clientLog.close();
		return;
	}

	clientLog << "Client: WSAStartup() is OK.\n";

	SOCKET m_socket;

	m_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (m_socket == INVALID_SOCKET)
	{
		clientLog << "Client: socket() - Error at socket(): %ld\n";
		clientLog << WSAGetLastError();
		WSACleanup();
		clientLog.close();
		return;
	}

	clientLog << "Client: socket() is OK.\n";

	sockaddr_in clientService;

	clientService.sin_family = AF_INET;
	InetPton(AF_INET, "127.0.0.1", &clientService.sin_addr.s_addr);
	clientService.sin_port = htons(55555);

	if (connect(m_socket, (SOCKADDR*)&clientService, sizeof(clientService)) == SOCKET_ERROR)
	{
		clientLog << "Client: connect() - Failed to connect.\n";
		WSACleanup();
		clientLog.close();
		return;
	}
	clientLog << "Client: connect() - OK.\n";

	// Send and receive data.
	int bytesSent;
	int bytesRecv = SOCKET_ERROR;

	// Be careful with the array bound, provide some checking mechanism
	char sendbuf[200] = "This is test data from the client!";
	char recvbuf[200] = "";

	bytesSent = send(m_socket, sendbuf, strlen(sendbuf), 0);
	clientLog << "Client: send() - Bytes Sent: ";
	clientLog << bytesSent;
	clientLog << "\n";

	while (bytesRecv == SOCKET_ERROR)
	{
		bytesRecv = recv(m_socket, recvbuf, 200, 0);

		if (bytesRecv == 0 || bytesRecv == WSAECONNRESET)
		{
			clientLog << "Client: Connection Closed.\n";
			break;
		}
		else
		{
			clientLog << "Client: recv() is OK.\n";
		}


		if (bytesRecv < 0)
		{
			return;
		}
		else
		{
			std::string s = recvbuf;

			clientLog << "Client: Received data is: ";
			for (int i = 0; i < s.length(); i++)
			{
				clientLog << s[i];
			}		
			clientLog << "\n";
			clientLog << "Client: Bytes received - ";
			clientLog << bytesRecv;
			clientLog << "\n";
		}
	}

	clientLog.close();
	closesocket(m_socket);
}


int __stdcall WinMain(HINSTANCE, HINSTANCE, char *, int) {
	//std::thread thrdClient(RunClient);
	fpl::Start();
	//thrdClient.join();



	return (0);
}



