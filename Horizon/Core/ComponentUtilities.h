#pragma once
#include "HorizonClient.h"
#include "CoreCommon.h"
#include <string>
#include <guiddef.h>


#define EXPORT __declspec( dllexport )

#ifdef _DEBUG 
#define DBG(x) OutputDebugStringA(x)
#define RETURN(msg,val){ DBG(msg);return val;}
#else
#define DBG(x)
#define RETURN(msg,val) return val
#endif

#define STOP MessageBoxA(NULL,"Stop","Stop",0);


//These structures are common in code. Do not move elsewhere.
struct FILE_MUTEX_STRUCT {
	bool locked = false;
	HANDLE loaderHandle = nullptr;		//Used to drive away other loaders trying to load the same component
	std::string file_path;
};

//Used only by components to simply take their lock
FILE_MUTEX_STRUCT getComponentLock(HMODULE dllInstance);

std::string guidToString(GUID guid);

//Windows version
BOOL is64BitWindows();

std::string getRegistryParam(std::string name);
void setRegistryParam(std::string name, std::string value);
//Returns the path where dlls(with backslash) for this compiled dll arhitecture(32/64) are. Otherwise empty string.
std::string getSuitePath();


FILE_MUTEX_STRUCT lockFile(std::string filename);

//Gets the next available dll with corresponding type to load
//Specify ClientTypes::any_load to load the next component
FILE_MUTEX_STRUCT getNextLibComponent(ClientTypes type);
//Loads and manages resource of the provided component
//Releases provided mutex and closes handle
void initiateComponent(FILE_MUTEX_STRUCT lib_file);


//Writes the user descriptor in registry
void updateStoredDescriptor(const USERDESCRIPTOR* user);

//Loads DESCRIPTOR from registry
//Only the component can give the version and component type
std::unique_ptr<USERDESCRIPTOR> loadStoredDescriptor(int version, ClientTypes component_type);


