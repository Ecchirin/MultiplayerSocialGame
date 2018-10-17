#include <winsock2.h>
#include <stdio.h>



int main(void)
{
	WSADATA     wsaData;
	SOCKET      RecvSocket;
	SOCKADDR_IN RecvAddr;
	SOCKADDR_IN SenderAddr;

	int         SenderAddrSize = sizeof(SenderAddr);
	char        RecvBuf[BUFSIZ];
	int         MsgSize;

	// 1. Initialize Winsock
	if (NO_ERROR != WSAStartup(MAKEWORD(2, 2), &wsaData)) return 1;

	// 2. Create a receiver socket to receive datagrams
	RecvSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (INVALID_SOCKET == RecvSocket) return 1;

	// 3. Bind the socket to any address and the specified port.
	RecvAddr.sin_family = AF_INET;
	RecvAddr.sin_port = htons(7890);
	RecvAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	if (SOCKET_ERROR == bind(RecvSocket, (SOCKADDR *)& RecvAddr, sizeof(RecvAddr)))
	{
		closesocket(RecvSocket);
		WSACleanup();
		return 1;
	}

	while (1)
	{
		// 4. Call the recvfrom function to receive datagrams
		printf("Receiving datagrams...\n");
		if (SOCKET_ERROR == (MsgSize = recvfrom(RecvSocket, RecvBuf, BUFSIZ, 0, (SOCKADDR *)&SenderAddr, &SenderAddrSize))) break;
		else if (0 == MsgSize) break;

		// Send a datagram to the receiver
		printf("Sending a datagram to the receiver...\n");
		if (SOCKET_ERROR == sendto(RecvSocket, RecvBuf, MsgSize, 0, (SOCKADDR *)&SenderAddr, sizeof(SenderAddr))) break;
	}

		// 5. Close & Clean up
		closesocket(RecvSocket);
		WSACleanup();

		return 0;
}