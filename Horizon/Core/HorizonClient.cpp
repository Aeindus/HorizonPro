#include "HorizonClient.h"
#include "ComponentUtilities.h"
#include "../Config.h"
#include <string>
#include <stdexcept>
#include <process.h>
#include <sstream>

//Used for the async getaddrinfo
struct GETADRRINFOTHREAD {
	const char* host;
	const char* port;
	addrinfo hints, * addr_res;
};


// Asynchrnous function to get DNS results
unsigned int WINAPI asyncGetAddrInfo(void* data);
// Used to connect to a specified server
SOCKET tryConnect(TRY_CONNECT_INFO details);
SOCKET tryConnect(std::string host, std::string port, bool isIp);



bool HorizonClient::wsa_initiated = false;

HorizonClient::HorizonClient(TRY_CONNECT_INFO server_addr, const USERDESCRIPTOR* set_user, HMODULE dll, std::function<void(const USERDESCRIPTOR*)> userupdated) {
	SOCKET connection_socket;

	// Initialize Winsock
	if (!wsa_initiated) {
		WSADATA wsaData;
		int result = result = WSAStartup(MAKEWORD(2, 2), &wsaData);
		if (result == 0) wsa_initiated = true;
		else throw DbgException("Could not initialize WSA");
	}


	if ((connection_socket = tryConnect(server_addr)) == -1) {
		throw DbgException("Could not connect to the server");
	}

	user = new USERDESCRIPTOR;
	memcpy(user, set_user, sizeof(USERDESCRIPTOR));
	sanitizeUserDescriptor(user);

	this_dll = dll;
	callback_userupdated = userupdated;
	protocol = TcpInterface(connection_socket);

	ZeroMemory(last_packet_token.data(), 0);
}
HorizonClient::~HorizonClient() {
	// In case the object was destroyed because of an exception or for other reasons
	// do not forget to close the socket
	closeClient();

	if (user != nullptr) delete user;
	user = nullptr;
}

const USERDESCRIPTOR* HorizonClient::getCurrentDescriptor() {
	return user;
}

bool HorizonClient::authentificate(int line) {
	if (isClosed()) return false;
	if (authentificated) return true;

	std::unique_ptr<PACKET_HEADER> temp(new PACKET_HEADER);

	if (protocol.recvPacketHeader(temp.get(), line) == ERR_TCP_CLOSED) {
		closeClient();
		return false;
	}

	//Not standard protocol
	if ((PacketDataTypes)temp->data_type != PacketDataTypes::authentificate) {
		closeClient();
		return false;
	}

	setTokenFromHeader(temp.get());

	std::unique_ptr<USERDESCRIPTOR> data(new USERDESCRIPTOR);
	if (protocol.recvPacketData((char*)data.get(), sizeof(USERDESCRIPTOR), line) == ERR_TCP_CLOSED) {
		closeClient();
		return false;
	}

	//Sanitize the received user descriptor
	sanitizeUserDescriptor(data.get());

	if (user->unique_id[0] == 0) {
		strncpy_s(user->unique_id, data->unique_id, _TRUNCATE);

		//Notify the caller the descriptor changed
		if (callback_userupdated) callback_userupdated(user);
	}

	//Send back the curent userdescriptor
	std::unique_ptr<PACKET_HEADER> auth_head = createHeader(PacketDataTypes::authentificate, sizeof(USERDESCRIPTOR), "", last_packet_token);

	if (protocol.sendPacketHeader(auth_head.get(), line) == ERR_TCP_CLOSED) {
		closeClient();
		return false;
	}

	//Make the timming as accurate as possible
	GetSystemTime(&(user->os_time));

	if (protocol.sendPacketData((char*)user, sizeof(USERDESCRIPTOR), line) == ERR_TCP_CLOSED) {
		closeClient();
		return false;
	}

	//Received the packets as described in standard model...assume is secured connection
	protocol.setRecvTimeout(false);
	authentificated = true;

	return true;
}

void HorizonClient::closeClient() {
	protocol.closeConnection();

	if (user != nullptr) delete user;
	user = nullptr;
}
bool HorizonClient::isClosed() {
	return protocol.isClosed();
}
bool HorizonClient::isAuthentificated() {
	return authentificated;
}
bool HorizonClient::canUse() {
	return !isClosed() && isAuthentificated();
}

bool HorizonClient::dataAvailable(int seconds) {
	if (isClosed()) return false;
	return protocol.dataAvailable(seconds);
}
void HorizonClient::applyTokenToHeader(PACKET_HEADER* header) {
	memcpy(header->token, last_packet_token.data(), sizeof(header->token));
}
void HorizonClient::setTokenFromHeader(PACKET_HEADER* header) {
	memcpy(last_packet_token.data(), header->token, sizeof(last_packet_token));
}
void HorizonClient::setToken(TOKEN_KEY& token) {
	memcpy(last_packet_token.data(), token.data(), sizeof(last_packet_token));
}
void HorizonClient::getToken(TOKEN_KEY& token) {
	memcpy(token.data(), last_packet_token.data(), sizeof(last_packet_token));
}


bool HorizonClient::sendPacketHeader(PacketDataTypes data_type, uint64_t size, std::string arguments, int line) {
	std::unique_ptr<PACKET_HEADER> head = createHeader(data_type, size, arguments, last_packet_token);
	return sendPacketHeader(head.get(), line);
}
bool HorizonClient::sendPacketHeader(PACKET_HEADER* head, int line) {
	if (isClosed()) return false;
	applyTokenToHeader(head);
	return protocol.sendPacketHeader(head, line) == SUCC;
}
bool HorizonClient::sendData(const char* buffer, uint64_t len, int line) {
	if (isClosed()) return false;
	return protocol.sendPacketData(buffer, len, line) == SUCC;
}

bool HorizonClient::sendNotification(std::string msg, int line) {
	TOKEN_KEY notificationToken;
	notificationToken.fill(0);

	std::unique_ptr<PACKET_HEADER> head = createHeader(PacketDataTypes::com_notification, 0, msg, notificationToken);
	return sendPacketHeader(head.get(), line);
}
bool HorizonClient::sendError(std::string msg, int line) {
	std::ostringstream message;

	std::replace(msg.begin(), msg.end(), '"', '\'');
	message << "-err \"" << msg << "\"";

	protocol.generateNotification(message.str());

	// Success does not mean notification was sent
	return protocol.sendReports() == SUCC;
}

bool HorizonClient::recvPacketHeader(PACKET_HEADER* head, int line) {
	if (isClosed()) return false;

	while (true) {
		std::unique_ptr<PACKET_HEADER> temp(new PACKET_HEADER);

		if (protocol.recvPacketHeader(temp.get(), line) == ERR_TCP_CLOSED) return false;

		setTokenFromHeader(temp.get());

		switch ((PacketDataTypes)temp->data_type) {
		case PacketDataTypes::authentificate:
		{
			std::unique_ptr<USERDESCRIPTOR> data(new USERDESCRIPTOR);

			if (protocol.recvPacketData((char*)data.get(), sizeof(USERDESCRIPTOR), line) == ERR_TCP_CLOSED) return false;
			sanitizeUserDescriptor(data.get());

			//Received another request after authetifications
			//This means this is an update
			strncpy_s(user->custom_name, data->custom_name, _TRUNCATE);
			strncpy_s(user->pc_description, data->pc_description, _TRUNCATE);
			strncpy_s(user->extra_data, data->extra_data, _TRUNCATE);

			//Notify the caller the descriptor changed
			if (callback_userupdated) callback_userupdated(user);

			break;
		}
		case PacketDataTypes::load_component:
		{
			ClientTypes type = ClientTypes::no_load;	//default
			if (protocol.recvPacketData((char*)&type, 4, line) == ERR_TCP_CLOSED) return false;

			FILE_MUTEX_STRUCT lib_file = getNextLibComponent(type);

			initiateComponent(lib_file);
			break;
		}
		case PacketDataTypes::ping_acknowledged:
		{
			break;
		}
		case PacketDataTypes::ping_send:
		{
			// Do not enable or this will fill the buffer of the server uselesly
			// The connection is already "aware" on both directions:
			//		here because of the constant receiving and the keep alive
			//		on the server because we just sent this package
			//protocol.generateNotification(PacketDataTypes::ping_acknowledged,last_packet_token, "");
			break;
		}
		case PacketDataTypes::close_client:
		{
			closeClient();
			return false;
			break;
		}
		case PacketDataTypes::abort:
		{
			//What to do?
			//Better close client
			closeClient();
			return false;
			break;
		}
		case PacketDataTypes::free_component:
		{
			FreeLibraryAndExitThread(this_dll, 0);
			break;
		}
		default:
		{
			*head = *temp;
			return true;
		}
		}
	}
}
bool HorizonClient::recvData(char* buffer, uint64_t len, int line) {
	if (isClosed()) return false;
	return protocol.recvPacketData(buffer, len, line) == SUCC;
}


//Returns an dns accordingly to debug/release and random dns
//Increments and rewrites dns_index when passes threshold automatically
TRY_CONNECT_INFO getNextDNS() {
	TRY_CONNECT_INFO res;
	static int dns_index = 0;

#if RANDOM_DNS
	std::string dns[] = { TCPDNSES,	generateRandomDNS() };
#else 
	std::string dns[] = { TCPDNSES };
#endif

	dns_index = dns_index % 3;

	res.is_ip = false;
	res.server_addres = dns[dns_index];
	res.port = TCPPORT;

	dns_index++;
	return res;
}
//Returns a random dns for each week in year
std::string generateRandomDNS() {
	SYSTEMTIME time;
	char dns[1][30] = { "example.com" };

	GetLocalTime(&time);
	int week = ((time.wMonth - 1) * 4 + (time.wDay - 1) / 7) % (sizeof(dns) / sizeof(dns[0]));

	return std::string(dns[week]);
}

std::unique_ptr<USERDESCRIPTOR> createQuickUser(std::string custom_name, std::string pc_description, char version, ClientTypes client_type) {
	std::unique_ptr<USERDESCRIPTOR> res(new USERDESCRIPTOR);

	strncpy_s(res->custom_name, custom_name.c_str(), _TRUNCATE);
	strncpy_s(res->pc_description, pc_description.c_str(), _TRUNCATE);
	res->version = version;
	res->client_type = (uint32_t)client_type;

	return res;
}


unsigned int WINAPI asyncGetAddrInfo(void* data) {
	GETADRRINFOTHREAD* info = (GETADRRINFOTHREAD*)data;
	if (getaddrinfo((char*)(info->host), (char*)(info->port), &(info->hints), &(info->addr_res)) == 0) return 2;
	return 1;
}
SOCKET tryConnect(TRY_CONNECT_INFO details) {
	return tryConnect(details.server_addres.c_str(), details.port.c_str(), details.is_ip);
}
SOCKET tryConnect(std::string host, std::string port, bool isIp) {
	SOCKET conn_sock;
	struct fd_set write_fs;
	struct timeval timeout;
	uint32_t iMode = 1;		//0-blocking;<>0 non-blocking

	// Timeout for DNS querying
	timeout.tv_sec = 5;		//5 seconds
	timeout.tv_usec = 0;

	if (isIp) {
		sockaddr_in dest;

		if ((conn_sock = socket(AF_INET, SOCK_STREAM, 0)) == INVALID_SOCKET) return -1;

		FD_ZERO(&write_fs);
		FD_SET(conn_sock, &write_fs);
		ioctlsocket(conn_sock, FIONBIO, (unsigned long*)&iMode);

		dest.sin_family = AF_INET;
		dest.sin_port = htons(atoi(port.c_str()));
		InetPtonA(AF_INET, host.c_str(), &dest.sin_addr.S_un.S_addr);

		if (connect(conn_sock, (sockaddr*)&dest, sizeof(dest)) == SOCKET_ERROR) {
			//Non-blocking so first do some checks...

			if (select(0, NULL, &write_fs, NULL, &timeout) == 1) {
				//Disable non-blocking
				iMode = 0;
				ioctlsocket(conn_sock, FIONBIO, (unsigned long*)&iMode);
				return conn_sock;		//Connection succesfull
			}
			closesocket(conn_sock);		//Timeout or error so free socket
		}

	} else {
		DWORD thread_exit_code = 0;
		GETADRRINFOTHREAD info;


		memset(&info.hints, 0, sizeof info.hints);
		info.host = host.c_str();
		info.port = port.c_str();
		info.hints.ai_family = AF_UNSPEC;
		info.hints.ai_socktype = SOCK_STREAM;

		HANDLE thread = (HANDLE)_beginthreadex(NULL, 0, &asyncGetAddrInfo, &info, 0, NULL);
		//HANDLE thread = CreateThread(NULL, 0, asyncGetAddrInfo, &info, 0, 0);

		if (thread == NULL) return -1;
		WaitForSingleObject(thread, timeout.tv_usec / 1000 + timeout.tv_sec * 1000);
		GetExitCodeThread(thread, &thread_exit_code);
		TerminateThread(thread, 0);
		CloseHandle(thread);

		if (thread_exit_code != 2) return -1;

		// loop through all the results and connect to the first we can
		addrinfo* res;
		for (res = info.addr_res; res != NULL; res = res->ai_next) {
			if ((conn_sock = socket(res->ai_family, res->ai_socktype, res->ai_protocol)) == -1) {
				continue;
			}
			FD_ZERO(&write_fs);
			FD_SET(conn_sock, &write_fs);
			ioctlsocket(conn_sock, FIONBIO, (unsigned long*)&iMode);

			if (connect(conn_sock, res->ai_addr, res->ai_addrlen) == SOCKET_ERROR) {
				//Non-blocking so first do some checks...

				if (select(0, NULL, &write_fs, NULL, &timeout) == 1) break;			//Connection succesfull

				closesocket(conn_sock);												//Timeout or error so free socket
				continue;
			}
		}

		freeaddrinfo(info.addr_res); // all done with this structure

		if (res != NULL) {
			//Disable non-blocking
			iMode = 0;
			ioctlsocket(conn_sock, FIONBIO, (unsigned long*)&iMode);
			return conn_sock;		//Connection succesfull
		}
	}

	return -1;
}

USERDESCRIPTOR* sanitizeUserDescriptor(USERDESCRIPTOR* user) {
	user->unique_id[sizeof(user->unique_id) - 1] = 0;
	user->user_name[sizeof(user->user_name) - 1] = 0;
	user->pc_description[sizeof(user->pc_description) - 1] = 0;
	user->custom_name[sizeof(user->custom_name) - 1] = 0;
	user->extra_data[sizeof(user->extra_data) - 1] = 0;

	return user;
}