#define _CRT_SECURE_NO_WARNINGS

#include "PELoader.h"
#include "ComponentUtilities.h"
#include <fstream>
#include <optional>

#ifdef  _WIN64 
#define DLL_FIXED_ADDRESS 0x180000000
#define MY_ARCH 64
#else
#define DLL_FIXED_ADDRESS 0x10000000
#define MY_ARCH 32
#endif 

// Better code visualization
#define callback CALLBACK
#define PUCHAR UCHAR*

typedef struct BASE_RELOCATION_BLOCK {
	DWORD PageAddress;
	DWORD BlockSize;
} BASE_RELOCATION_BLOCK, * PBASE_RELOCATION_BLOCK;

typedef struct BASE_RELOCATION_ENTRY {
	USHORT Offset : 12;
	USHORT Type : 4;
} BASE_RELOCATION_ENTRY, * PBASE_RELOCATION_ENTRY;

void applyKeyAlgorithm(char* memory, uint64_t memory_size, const uint8_t* key, int key_length);




BOOL getNTHeader(HMODULE component_handle, PIMAGE_NT_HEADERS& nt_header) {
	PIMAGE_DOS_HEADER pDOSHeader = (PIMAGE_DOS_HEADER)component_handle;

	if (pDOSHeader->e_magic != IMAGE_DOS_SIGNATURE) {
		nt_header = NULL;
		return false;
	}

	nt_header = (PIMAGE_NT_HEADERS)((PBYTE)component_handle + pDOSHeader->e_lfanew);

	if (nt_header->Signature != IMAGE_NT_SIGNATURE) {
		nt_header = NULL;
		return false;
	}

	return true;
}
BOOL getNTHeader(const char* path, IMAGE_NT_HEADERS& pNTHeaders) {
	IMAGE_DOS_HEADER pDOSHeader;

	std::fstream fs(path, std::ios::binary | std::ios::in);
	fs.read((char*)&pDOSHeader, sizeof(IMAGE_DOS_HEADER));

	if (pDOSHeader.e_magic != IMAGE_DOS_SIGNATURE) {
		memset(&pNTHeaders, 0, sizeof(pNTHeaders));
		return false;
	}

	fs.seekg(pDOSHeader.e_lfanew, std::ios::beg);
	fs.read((char*)&pNTHeaders, sizeof(pNTHeaders));
	fs.close();

	if (pNTHeaders.Signature != IMAGE_NT_SIGNATURE) {
		memset(&pNTHeaders, 0, sizeof(pNTHeaders));
		return false;
	}

	return true;
}

//Returns the string at the <ordinal> position in pointer provided
//The size limit is respected. Strings not null terminated (even when at the end of pointer) will not be returned.
//If reached end of limit will return ""
//Ordinal starts at 0
std::string getNextStringFromMemory(const char* addr, uint64_t size, int ordinal) {
	unsigned int offset = 0;
	const char* start = addr;

	while (offset < size - 1) {
		//Maximum values:  on size-1 position to be a 0  => offset can be at maximum on pos. size-2 (e.g {....'a',0})

		const char* current_pointer = start + offset;

		// Stop at the double null terminators aka end of the data
		if (*current_pointer == 0) {
			return std::string("");
		}

		// If no null terminator was found return empty
		if (strnlen_s(current_pointer, size - offset) == size - offset) return std::string("");

		if (ordinal == 0) {
			return std::string(current_pointer);
		}

		// Account for this string
		ordinal--;

		// Jump over to the next string passing over the null terminator
		offset += strlen(current_pointer) + 1;
	}
	return std::string("");
}
std::optional<QDATA_STORAGE> getSegmentStorage(HMODULE component_handle) {
	QDATA_STORAGE res;
	std::optional<SEGMENT_DATA> segQDataInfo = getSegmentInfo(component_handle, segment_qdata);

	if (!segQDataInfo) return {};
	res = *(QDATA_STORAGE*)(segQDataInfo->segment_addr);
	return res;
}
std::optional<QDATA_STORAGE> getSegmentStorage(const char* dllpath) {
	std::optional<SEGMENT_DATA> segQDataInfo;
	QDATA_STORAGE qdataStorage;
	
	segQDataInfo = getSegmentInfo(dllpath, segment_qdata);

	// If it couldn't find the desired segments inside the file return
	if (!segQDataInfo) return {};

	// Open the library to write/read on
	std::fstream bn(dllpath, std::ios::binary | std::ios::in | std::ios::out);
	if (!bn.good()) return {};
	
	// Read the segment into buffer
	bn.seekg(segQDataInfo->infile_segment_offset, std::ios_base::beg);
	bn.read((char*)&qdataStorage, sizeof(QDATA_STORAGE));

	return qdataStorage;
}

std::optional<SEGMENT_DATA> getSegmentInfo(HMODULE component_handle, const char* segment_name) {
	PIMAGE_SECTION_HEADER sectionHeader;
	PIMAGE_NT_HEADERS nth;
	SEGMENT_DATA segInfo;

	if (getNTHeader(component_handle, nth) == false) return {};
	sectionHeader = IMAGE_FIRST_SECTION(nth);

	bool found = false;
	for (WORD i = 0; i < nth->FileHeader.NumberOfSections; i++, sectionHeader++) {
		if (strcmp((char*)sectionHeader->Name, segment_name) == 0) {
			segInfo.segment_addr = (char*)component_handle + sectionHeader->VirtualAddress;
			segInfo.segment_size = sectionHeader->Misc.VirtualSize;
			segInfo.infile_segment_offset = sectionHeader->PointerToRawData;
			segInfo.infile_segment_size = sectionHeader->SizeOfRawData;

			found = true;
			break;
		}
	}

	if (!found) return {};
	if (nth->OptionalHeader.Magic == 0x020B) segInfo.architecture = 64;
	else if (nth->OptionalHeader.Magic == 0x010B) segInfo.architecture = 32;

	return segInfo;
}
std::optional<SEGMENT_DATA> getSegmentInfo(const char* dllpath, const char* segment_name) {
	std::optional<SEGMENT_DATA> segInfo;
	char* completeFileBuffer;
	std::fstream fs(dllpath, std::ios::binary | std::ios::in);

	if (!fs.good()) return {};

	// Get file size
	fs.seekg(0, std::ios::end);
	if (fs.tellg() == -1) return {};

	__int64 cbSize = fs.tellg();

	// Read entire dll in this buffer
	fs.seekg(0, std::ios::beg);
	completeFileBuffer = new char[cbSize];
	fs.read(completeFileBuffer, cbSize);
	fs.close();

	segInfo = getSegmentInfo((HMODULE)completeFileBuffer, segment_name);
	if (!segInfo) {
		delete[] completeFileBuffer;
		return {};
	}

	delete[] completeFileBuffer;
	return segInfo;
}

bool decryptSegmentInMemory(HMODULE component_handle, const char* segment_name) {
	std::optional < SEGMENT_DATA> segInfo = getSegmentInfo(component_handle, segment_name);

	if (!segInfo) return false;

	std::optional<QDATA_STORAGE> qdataStorage = getSegmentStorage(component_handle);
	if (!qdataStorage) return false;

	uint8_t* lDecryptionKey = qdataStorage->decryption_key;	// read decryption key from segment qdata
	DWORD mOlProtection;
	DWORD mOlProtection_1;

	if (segInfo->architecture != MY_ARCH) return false;

	if (VirtualProtect(segInfo->segment_addr, segInfo->segment_size, PAGE_EXECUTE_READWRITE, &mOlProtection) == 0) return false;

	applyKeyAlgorithm(segInfo->segment_addr, segInfo->segment_size, lDecryptionKey, STORAGE_PASSWORD_SIZE);

	if (VirtualProtect(segInfo->segment_addr, segInfo->segment_size, mOlProtection, &mOlProtection_1) == 0) return false;

	// check for some useless 'integrity-protocol' test
	// though not so useless as it allows the loader to verify if decryption worked on the desired segment page
	//TODO: implement this integrity protocol
	//UINT64 thDecryptionMagicKey = *(UINT64*)segInfo->segment_addr;

	return true;
}
bool encryptSegmentInFile(const char* dllpath, const char* segment_name, const uint8_t* key, int pw_length) {
	std::optional < SEGMENT_DATA> segInfo;
	char* readSegmentBuffer;

	// The logic here is that getSegmentInfo() already opens the entire file in memory
	// So to simplify things we will read the entire segment in memory, apply the same enc/decryption algorithm and then write it back
	// Note: seekg and seekp are identical - they modify the position of the same pointer internally

	// Retrieve segment info from file
	segInfo = getSegmentInfo(dllpath, segment_name);

	// If it couldn't find the desired segments inside the file return
	if (!segInfo) return false;

	// Open the library to write/read on
	std::fstream bn(dllpath, std::ios::binary | std::ios::in | std::ios::out);

	if (!bn.good()) return false;
	readSegmentBuffer = new char[segInfo->infile_segment_size];

	// Read the segment into buffer
	bn.seekg(segInfo->infile_segment_offset, std::ios_base::beg);
	bn.read(readSegmentBuffer, segInfo->infile_segment_size);

	// Encrypt buffer
	applyKeyAlgorithm(readSegmentBuffer, segInfo->infile_segment_size, key, pw_length);

	// Write the modified data back to file
	bn.seekg(segInfo->infile_segment_offset, std::ios_base::beg);
	bn.write(readSegmentBuffer, segInfo->infile_segment_size);

	// Flush the data-buffer 
	bn.flush();
	bn.close();
	delete[] readSegmentBuffer;

	return true;
}
bool decryptSegmentInFile(const char* dllpath, const char* segment_name, const uint8_t* key, int pwLength) {
	return encryptSegmentInFile(dllpath, segment_name, key, pwLength);
}


bool writeRawInSegmentInFile(const char* dllpath, const char* segment_name, uint64_t offset, const char* data, uint64_t data_size) {
	std::optional<SEGMENT_DATA> segInfo;
	segInfo = getSegmentInfo(dllpath, segment_name);

	// if it couldn't find the segment
	if (!segInfo) return false;

	// open the library to write/read on
	std::fstream bn(dllpath, std::ios::binary | std::ios::in | std::ios::out);

	// check for availabilty
	if (!bn.is_open()) return false;

	// write the raw data in the desired segment
	bn.seekg(segInfo->infile_segment_offset + offset, std::ios::beg);
	bn.write(data, data_size);

	// flush data-buffer
	bn.flush();
	bn.close();

	return true;
}
bool writeRawInSegmentInFile(const char* dllpath, const char* segment_name, const char* data, uint64_t data_size) {
	return writeRawInSegmentInFile(dllpath, segment_name, 0, data, data_size);
}
bool writeQDataStorage(const char* dllpath, QDATA_STORAGE storage) {
	return writeRawInSegmentInFile(dllpath, segment_qdata, (char*)&storage, sizeof(QDATA_STORAGE));
}

bool revertRelocatorChanges(HMODULE component_handle) {
	PBASE_RELOCATION_BLOCK	pBlockheader;
	PBASE_RELOCATION_ENTRY	pBlocks;
	std::optional < SEGMENT_DATA> segRelocInfo = getSegmentInfo(component_handle, ".reloc");
	std::optional < SEGMENT_DATA> segCryptInfo = getSegmentInfo(component_handle, segment_crypt);

	size_t dwRelocOffset = (size_t)component_handle - DLL_FIXED_ADDRESS;
	size_t dwItterationalOffset = 0;
	size_t dwEntryCount;

	if (!segRelocInfo || !segCryptInfo) return false;

	if (segRelocInfo->architecture != MY_ARCH) return false;

	while (dwItterationalOffset < segRelocInfo->segment_size) {
		pBlockheader = (PBASE_RELOCATION_BLOCK)(segRelocInfo->segment_addr + dwItterationalOffset);

		if (pBlockheader->BlockSize == 0) break;

		dwItterationalOffset += sizeof(BASE_RELOCATION_BLOCK);
		dwEntryCount = (pBlockheader->BlockSize - sizeof(BASE_RELOCATION_BLOCK)) / sizeof(BASE_RELOCATION_ENTRY);
		pBlocks = (PBASE_RELOCATION_ENTRY)(segRelocInfo->segment_addr + dwItterationalOffset);

		if (pBlockheader->PageAddress + (uint64_t)component_handle < (uint64_t)segCryptInfo->segment_addr || pBlockheader->PageAddress + (uint64_t)component_handle >(uint64_t)segCryptInfo->segment_addr + segCryptInfo->segment_size) {
			// then don't apply any reallocation because this page address is not targetted
			dwItterationalOffset += dwEntryCount * sizeof(BASE_RELOCATION_ENTRY);
			continue;
		}

		for (size_t y = 0; y < dwEntryCount; y++) {
			dwItterationalOffset += sizeof(BASE_RELOCATION_ENTRY);

			if (pBlocks[y].Offset == 0 || pBlocks[y].Type == 0) break;

			size_t dwFieldAddress = (uint64_t)component_handle + pBlockheader->PageAddress + pBlocks[y].Offset;
			DWORD pProtection_1;
			DWORD pProtection_2;

			if (VirtualProtect((LPVOID)dwFieldAddress, sizeof(size_t), PAGE_EXECUTE_READWRITE, &pProtection_1) == 0) return false;

			switch (pBlocks[y].Type) {
				case IMAGE_REL_BASED_DIR64:
					*((UINT_PTR*)(dwFieldAddress)) -= dwRelocOffset;
					break;
				case IMAGE_REL_BASED_HIGHLOW:
					*((size_t*)(dwFieldAddress)) -= (size_t)dwRelocOffset;
					break;
				case IMAGE_REL_BASED_HIGH:
					*((WORD*)(dwFieldAddress)) -= HIWORD(dwRelocOffset);
					break;
				case IMAGE_REL_BASED_LOW:
					*((WORD*)(dwFieldAddress)) -= LOWORD(dwRelocOffset);
					break;
			}

			if (VirtualProtect((LPVOID)dwFieldAddress, sizeof(size_t), pProtection_1, &pProtection_2) == 0) return false;
		}
	}

	return true;
}
bool rebaseModuleSegment(HMODULE component_handle) {
	PBASE_RELOCATION_BLOCK	pBlockheader;
	PBASE_RELOCATION_ENTRY	pBlocks;
	std::optional < SEGMENT_DATA> segRelocInfo = getSegmentInfo(component_handle, ".reloc");
	std::optional < SEGMENT_DATA> segCryptInfo = getSegmentInfo(component_handle, segment_crypt);

	size_t dwRelocOffset = (size_t)component_handle - DLL_FIXED_ADDRESS;
	size_t dwItterationalOffset = 0;
	size_t dwEntryCount;

	if (!segRelocInfo || !segCryptInfo) return false;


	if (segRelocInfo->architecture != MY_ARCH) return false;

	while (dwItterationalOffset < segRelocInfo->segment_size) {
		pBlockheader = (PBASE_RELOCATION_BLOCK)(segRelocInfo->segment_addr + dwItterationalOffset);

		if (pBlockheader->BlockSize == 0) break;

		dwItterationalOffset += sizeof(BASE_RELOCATION_BLOCK);
		dwEntryCount = (pBlockheader->BlockSize - sizeof(BASE_RELOCATION_BLOCK)) / sizeof(BASE_RELOCATION_ENTRY);
		pBlocks = (PBASE_RELOCATION_ENTRY)(segRelocInfo->segment_addr + dwItterationalOffset);

		if (pBlockheader->PageAddress + (uint64_t)component_handle < (uint64_t)segCryptInfo->segment_addr || pBlockheader->PageAddress + (uint64_t)component_handle >(uint64_t)segCryptInfo->segment_addr + segCryptInfo->segment_size) {
			// then don't apply any reallocation because this page address is not targetted
			dwItterationalOffset += dwEntryCount * sizeof(BASE_RELOCATION_ENTRY);
			continue;
		}

		for (uint64_t y = 0; y < dwEntryCount; y++) {
			dwItterationalOffset += sizeof(BASE_RELOCATION_ENTRY);

			if (pBlocks[y].Offset == 0 || pBlocks[y].Type == 0) break;

			uint64_t dwFieldAddress = (uint64_t)component_handle + pBlockheader->PageAddress + pBlocks[y].Offset;
			DWORD pProtection_1;
			DWORD pProtection_2;

			if (VirtualProtect((LPVOID)dwFieldAddress, sizeof(size_t), PAGE_EXECUTE_READWRITE, &pProtection_1) == 0) return false;

			switch (pBlocks[y].Type) {
				case IMAGE_REL_BASED_DIR64:
					*((UINT_PTR*)(dwFieldAddress)) += dwRelocOffset;
					break;
				case IMAGE_REL_BASED_HIGHLOW:
					*((size_t*)(dwFieldAddress)) += (size_t)dwRelocOffset;
					break;
				case IMAGE_REL_BASED_HIGH:
					*((WORD*)(dwFieldAddress)) += HIWORD(dwRelocOffset);
					break;
				case IMAGE_REL_BASED_LOW:
					*((WORD*)(dwFieldAddress)) += LOWORD(dwRelocOffset);
					break;
			}

			if (VirtualProtect((LPVOID)dwFieldAddress, sizeof(size_t), pProtection_1, &pProtection_2) == 0) return false;
		}
	}

	return true;
}



size_t convertRvaToOffset(PIMAGE_NT_HEADERS nth, size_t RVA) {
	PIMAGE_SECTION_HEADER sectionHeader = IMAGE_FIRST_SECTION(nth);
	int sections = nth->FileHeader.NumberOfSections;

	for (int i = 0; i < sections; i++, sectionHeader++)
		if (sectionHeader->VirtualAddress <= RVA && (sectionHeader->VirtualAddress + sectionHeader->Misc.VirtualSize) > RVA) {
			RVA -= sectionHeader->VirtualAddress;
			RVA += sectionHeader->PointerToRawData;
			return RVA;
		}

	return 0;
}
size_t getFunctionOffsetInFile(const char* dllpath, const char* function) {
	HMODULE mHandle = LoadLibraryA(dllpath);
	size_t retOffset = 0;

	if (mHandle == NULL) return false;

	PVOID funcAddr = GetProcAddress(mHandle, function);

	if (funcAddr == NULL) {
		FreeLibrary(mHandle);
		return false;
	}

	PIMAGE_NT_HEADERS nth;
	if (getNTHeader(mHandle, nth) == false) {
		FreeLibrary(mHandle);
		return false;
	}

	if (nth == NULL) {
		FreeLibrary(mHandle);
		return false;
	}

	size_t delta = (size_t)funcAddr - (size_t)mHandle;
	size_t jmpAddr;
	BYTE ff[5];
	FILE* fl;

	retOffset = convertRvaToOffset(nth, delta);

	fl = fopen(dllpath, "rb");
	fseek(fl, retOffset, SEEK_CUR);
	fread(ff, 1, 5, fl);
	fclose(fl);

	jmpAddr = *(UINT*)(ff + 1);

	retOffset = delta + jmpAddr + 5;

	retOffset = convertRvaToOffset(nth, retOffset);

	FreeLibrary(mHandle);
	return retOffset;
}
void applyKeyAlgorithm(char* memory, uint64_t memory_size, const uint8_t* key, int key_length) {
	for (uint64_t i = 0; i < memory_size; i++) {
		char* lpByteAddr = (memory + i);
		char temp = *lpByteAddr;
		*lpByteAddr = temp ^ key[i % key_length];
	}
}