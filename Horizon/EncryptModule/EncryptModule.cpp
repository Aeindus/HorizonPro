#include "..\Core\MicrosoftPELoader.h"

#ifdef  _WIN64 
	#define pth "C:\\Users\\rezni\\Desktop\\HorizonDovaExpress\\Horizon\\Debug\\FtpComponent.dll"
#else
	#define pth "C:\\Users\\rezni\\Desktop\\HorizonDovaExpress\\Horizon\\Debug\\FtpComponent.dll"
#endif 


int main(int argc, char **argv) {
	const char *path = argv[1];
	char password[PASSWORD_SIZE] = "zxcvbnm,.lkjhgd";

	encryptSegmentInFile(path, segment_crypt, password, PASSWORD_SIZE);
	return 0;

	/*
	HMODULE lib = LoadLibraryA(path);

	typedef DWORD(CALLBACK *MainEntryPoint)(LPVOID);
	typedef bool(CALLBACK *test)();

	MainEntryPoint mainEntry = (MainEntryPoint)GetProcAddress(lib, "intro");
	mainEntry(NULL);

	
	system("pause");
	return 0;
	*/
}
 