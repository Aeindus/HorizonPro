#include "TcpInterface.h"
#include "CoreCommon.h"
#include <stdexcept>
#include <sstream>
#include <string>

#define PACHET_PWS 0x41

//DEBUGGING
#define DISABLE_PACKET_ENCRIPTION 1

//Sanitizes the header for use with lesser safer methods like strlen or strcpy
PACKET_HEADER* sanitizeHeader(PACKET_HEADER* header);
uint64_t calculateChecksum(PACKET_HEADER* packet);

TcpInterface::TcpInterface(SOCKET s) {
	internal_socket = s;

	//Enable keep alive
	int mode = 1;	//<>0 enable
	if (setsockopt(internal_socket, SOL_SOCKET, SO_KEEPALIVE, (char*)&mode, sizeof(int)) == SOCKET_ERROR) {
		//This feature is very important. If not active then no connection can be detected as closed
		closeConnection();
		throw DbgException("Failed to initialize network interface");
	}

	//Set first the keep alive characteristics. Unit: millis
	tcp_keepalive params;
	params.keepalivetime = 5000;
	params.keepaliveinterval = 5000;
	params.onoff = 1;

	DWORD output = 0;
	if (WSAIoctl(internal_socket, SIO_KEEPALIVE_VALS, &params, sizeof(tcp_keepalive), 0, 0, &output, 0, 0) != 0) {
		//This feature is very important. If not active then no connection
		closeConnection();
		throw DbgException("Failed to initialize network interface");
	}

	//Assume connetion is unsecure
	setRecvTimeout(true);
}

bool TcpInterface::sendall(SOCKET s, const char* buffer, uint64_t len) {
	int sent = 0;		//The signed type is very important! recv returns negative. Dimension limited by recv return type(int)

	while (len) {
		if ((sent = send(s, buffer, min(65000, len), 0)) == SOCKET_ERROR) {
			closeConnection();
			return false;
		}

		buffer += sent;
		len -= sent;
	}

	return true;
}
bool TcpInterface::readall(SOCKET s, char* buffer, uint64_t len) {
	int received = 0;	//The signed type is very important! recv returns negative. Dimension limited by recv return type(int)

	if (len == 0) return true;

	while (len) {
		if ((received = recv(s, buffer, min(65000, len), MSG_WAITALL)) < 1) {
			closeConnection();
			return false;
		}

		len -= received;
	}

	return true;
}

bool TcpInterface::isPacketValid(PACKET_HEADER* head) {
	if (head->magic_start != MAGIC_START) return false;
	if (head->checksum != calculateChecksum(head)) return false;
	return true;
}

int TcpInterface::fixSendShift() {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (must_send == 0) return SUCC;

	int empty_buffer_size = 100;
	char* buffer = new char[empty_buffer_size];
	memset(buffer, 0, empty_buffer_size);

	while (must_send) {
		if (!sendall(internal_socket, buffer, min(empty_buffer_size, must_send))) {
			delete[] buffer;
			return ERR_TCP_CLOSED;
		}
		must_send -= min(empty_buffer_size, must_send);
	}
	delete[] buffer;
	return SUCC;
}
int TcpInterface::fixRecvShift() {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (must_recv == 0) return SUCC;

	int empty_buffer_size = 100;
	char* buffer = new char[empty_buffer_size];

	while (must_recv) {
		if (!readall(internal_socket, buffer, min(empty_buffer_size, must_recv))) {
			delete[] buffer;
			return ERR_TCP_CLOSED;
		}
		must_recv -= min(empty_buffer_size, must_recv);
	}

	delete[] buffer;
	return SUCC;
}

int TcpInterface::internalSendPacketHeader(PACKET_HEADER* head) {
	if (isClosed()) return ERR_TCP_CLOSED;
	must_send = head->packet_size;

	std::unique_ptr<PACKET_HEADER> encrypted_head(new PACKET_HEADER);
	memcpy(encrypted_head.get(), head, sizeof(PACKET_HEADER));

#if DISABLE_PACKET_ENCRIPTION==0
	//Directly encrypting pachet
	char* start = (char*)encrypted_head.get();
	for (int i = 0; i < sizeof(PACKET_HEADER); i++) {
		*(start + i) = (*(start + i)) ^ PACHET_PWS;
	}
#endif

	return (sendall(internal_socket, (char*)encrypted_head.get(), sizeof(PACKET_HEADER)) ? SUCC : ERR_TCP_CLOSED);
}

void TcpInterface::generateShiftError(bool send_side, PACKET_HEADER* sample_head, int err_line) {
	std::ostringstream message;

	sanitizeHeader(sample_head);
	std::string processed_args = std::string(sample_head->arguments);
	std::replace(processed_args.begin(), processed_args.end(), '"', '\'');

	message << "-err \"Shift error on " << (send_side ? "send" : "receive") << " side, on line " << err_line << ". Internal status : must_send " << must_send << ", must_recv " << must_recv << ".  Pachet info: size " << sample_head->packet_size << ", type " << sample_head->data_type << ", args | " << processed_args.c_str() << " | \"";

	if (errors.size() < 200) errors.push(message.str());
}
void TcpInterface::generateDataError(bool send_side, uint64_t sample_len, int err_line) {
	std::ostringstream message;

	message << "-err \"Data error on " << (send_side ? "send" : "receive") << " side, on line " << err_line << ". Internal status: must_send " << must_send << ", must_recv " << must_recv << ". Sample len: " << sample_len << "\"";

	if (errors.size() < 200) errors.push(message.str());
}

void TcpInterface::setRecvTimeout(bool set) {
	DWORD milis = (set ? 20000 : 0);
	setsockopt(internal_socket, SOL_SOCKET, SO_RCVTIMEO, (char*)&milis, sizeof(DWORD));
}
void TcpInterface::closeConnection() {
	if (!isClosed()) closesocket(internal_socket);
	internal_socket = 0;
}
bool TcpInterface::isClosed() {
	return (internal_socket == 0);
}

void TcpInterface::generateNotification(std::string arguments) {
	if (notifications.size() < 200) notifications.push(arguments);
}

int TcpInterface::sendPacketHeader(PACKET_HEADER* head, int line) {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (must_send != 0) {
		generateShiftError(true, head, line);
		if (fixSendShift() == ERR_TCP_CLOSED) return ERR_TCP_CLOSED;
	}

	//Send before any packet
	if (sendReports() == ERR_TCP_CLOSED) return ERR_TCP_CLOSED;
	return internalSendPacketHeader(head);
}
int TcpInterface::sendPacketData(const char* data, uint64_t len, int line) {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (len == 0) return SUCC;

	if (must_send == 0) {
		generateDataError(true, len, line);
		return SUCC;	//can't close the connection. It will fix itself
	}
	if (len > must_send) generateDataError(true, len, line); // Log this issue but go on carefully

	// Truncate data if we try send more
	uint64_t send_size = min(len, must_send);
	must_send -= send_size;

	return (sendall(internal_socket, data, send_size) ? SUCC : ERR_TCP_CLOSED);
}

int TcpInterface::recvPacketHeader(PACKET_HEADER* head, int line) {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (must_recv != 0) {
		generateShiftError(false, head, line);
		if (fixRecvShift() == ERR_TCP_CLOSED) return ERR_TCP_CLOSED;
	}

	//Send reports even when receving packets.
	//The send command will not conflict with packets in-transit because I check for shifts
	if (sendReports() == ERR_TCP_CLOSED) return ERR_TCP_CLOSED;

	if (!readall(internal_socket, (char*)head, sizeof(PACKET_HEADER))) {
		return ERR_TCP_CLOSED;
	}

#if DISABLE_PACKET_ENCRIPTION==0
	char* start = (char*)head;
	for (int i = 0; i < sizeof(PACKET_HEADER); i++) {
		*(start + i) = (*(start + i)) ^ PACHET_PWS;
	}
#endif

	must_recv = head->packet_size;

	//Sanitize the received header
	sanitizeHeader(head);

	//Reset connection if packet malformed
	if (!isPacketValid(head)) {
		closeConnection();
		return ERR_TCP_CLOSED;
	}

	return SUCC;
}
int TcpInterface::recvPacketData(char* buffer, uint64_t len, int line) {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (len == 0) return SUCC;

	if (must_recv == 0) {
		generateDataError(false, len, line);
		return SUCC;
	}
	if (len > must_recv) generateDataError(false, len, line); // Log this issue but go on carefully

	// Truncate "buffer" if we try read more
	uint64_t recv_size = min(must_recv, len);
	must_recv -= recv_size;

	return (readall(internal_socket, buffer, recv_size) ? SUCC : ERR_TCP_CLOSED);
}

int TcpInterface::sendReports() {
	if (isClosed()) return ERR_TCP_CLOSED;
	if (must_send != 0) {
		// This is not an critical functionality
		// This will be called on recv packet too. Implement this safety to avoid shifting data half way in transit
		// We can wait for the next sent/received packet to send this information
		return SUCC;
	}

	// Error messages use empty tokens
	TOKEN_KEY errorToken;
	errorToken.fill(0);

	while (!errors.empty()) {
		std::unique_ptr<PACKET_HEADER> report = createHeader(PacketDataTypes::tcp_error_debugging, 0, errors.front(), errorToken);

		if (internalSendPacketHeader(report.get()) == ERR_TCP_CLOSED) {
			return ERR_TCP_CLOSED;
		}
		errors.pop();
	}
	while (!notifications.empty()) {
		auto notificationData = notifications.front();
		std::unique_ptr<PACKET_HEADER> report = createHeader(PacketDataTypes::com_error_debugging, 0, notificationData, errorToken);

		if (internalSendPacketHeader(report.get()) == ERR_TCP_CLOSED) {
			return ERR_TCP_CLOSED;
		}
		notifications.pop();
	}

	return SUCC;
}

bool TcpInterface::dataAvailable(int seconds) {
	struct fd_set read_fs;
	struct timeval timeout;

	if (isClosed()) return false;

	timeout.tv_sec = seconds;
	timeout.tv_usec = (seconds == 0 ? 100000 : 0);  // Timeout in millis

	FD_ZERO(&read_fs);
	FD_SET(internal_socket, &read_fs);

	select(0, &read_fs, NULL, NULL, &timeout);

	return FD_ISSET(internal_socket, &read_fs);
}

std::queue<std::string> TcpInterface::errors;
std::queue<std::string> TcpInterface::notifications;


//Utilities
std::vector<paramStruct> configureParameterSets(char command[1024]) {
	std::vector<paramStruct> data;
	paramStruct temp;
	char* found;
	char* newcmd = command;

	while ((found = strchr(newcmd, '-')) != NULL) {
		newcmd = found + 1;

		char* firstSpace = strchr(newcmd, ' ');
		if (firstSpace == NULL) continue;

		strncpy_s(temp.pName, newcmd, min(sizeof(temp.pName) - 1, firstSpace - newcmd));

		// for value of param
		char* firstQuote = strchr(newcmd, '"');
		if (firstQuote == NULL) continue;

		char* lastQuote = strchr(firstQuote + 1, '"');
		if (lastQuote == NULL) continue;

		strncpy_s(temp.pValue, firstQuote + 1, min(sizeof(temp.pValue) - 1, lastQuote - firstQuote - 1));

		data.push_back(temp);

		//Necessary..this allows us to have the spacial char "-" inside the value area
		if (strchr(newcmd, '-') != NULL) newcmd = lastQuote;
	}

	return data;
}
std::string getPacketParamValue(char data[1024], const char searchParam[20]) {
	data[1023] = 0;
	std::vector<paramStruct> lpInParam = configureParameterSets(data);

	std::string lpOutParamValue = "";

	for (size_t t = 0; t < lpInParam.size(); t++) {
		if (strcmp(lpInParam[t].pName, searchParam) == 0) {
			lpOutParamValue = std::string(lpInParam[t].pValue);
			break;
		}
	}
	return lpOutParamValue;
}


std::unique_ptr<PACKET_HEADER> createHeader(PacketDataTypes data_type, uint64_t size, std::string arguments, const std::array<char, 16>& token) {
	std::unique_ptr<PACKET_HEADER> packet = createHeader(data_type, size, arguments);
	memcpy(packet->token, token.data(), sizeof(packet->token));
	return packet;
}
std::unique_ptr<PACKET_HEADER> createHeader(PacketDataTypes data_type, uint64_t size, std::string arguments) {
	std::unique_ptr<PACKET_HEADER> packet(new PACKET_HEADER);

	packet->magic_start = MAGIC_START;
	packet->packet_size = size;
	packet->data_type = (uint32_t)data_type;

	// Reset block of memory to zero
	memset(packet->arguments, 0,sizeof(packet->arguments));

	// Truncate is undesirable. Using the proper functionality of strcpy_s
	// can discover bugs in the code (aka if the arguments do not fit in the packet then 0 will be placed on first position)
	cstrncpy(packet->arguments, arguments.c_str(), arguments.length());

	packet->checksum = calculateChecksum(packet.get());

	return packet;
}

uint64_t calculateChecksum(PACKET_HEADER* packet) {
	uint64_t sum = 23;
	sum += packet->packet_size;	sum = 1093 * ((sum ^ 0x1a29d31ee240bca3UL) & 0xFFFFFFFFFFFFFUL);
	sum += packet->data_type;	sum = 1093 * ((sum ^ 0x1a29d31ee240bca3UL) & 0xFFFFFFFFFFFFFUL);

	for (int i = 0; i < sizeof(packet->arguments); i++) {
		sum += packet->arguments[i];
		sum = 1093 * ((sum ^ 0x1a29d31ee240bca3UL) & 0xFFFFFFFFFFFFFUL);
	}
	return sum;
}

PACKET_HEADER* sanitizeHeader(PACKET_HEADER* header) {
	header->arguments[sizeof(header->arguments) - 1] = 0;
	return header;
}

