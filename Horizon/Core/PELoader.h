#pragma once

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <string>
#include <memory>
#include <optional>

/**
*	NOTE: What to do for each component's settings.
*
*	1. Linker -> Advanced -> BASE ADDRESS : 0x10000000 (32BIT) || 0x180000000 (64BIT)
*							 RANDOMIZED BASE ADDRESS : NO
*							 FIXED BASE ADDRESS: NO
**/



// Defnitions used for better code segmentation
#define store_variable(x) __declspec(allocate(x))
#define store_code(seg) __declspec(code_seg(seg))

// Declare the password's size
#define STORAGE_PASSWORD_SIZE 256
#define STORAGE_EXTRA_MAX_SIZE 2048

// The segments used in code
#define segment_crypt ".crypt"
#define segment_qdata ".qdata"

struct QDATA_STORAGE {
	uint8_t decryption_key[STORAGE_PASSWORD_SIZE];
	uint16_t version;

	// The last TEXT array MUST be followed by a two nulls to mark the end
	char extra[STORAGE_EXTRA_MAX_SIZE] = { 0,0 };
};


typedef struct {
	char* segment_addr;
	uint64_t segment_size;
	uint64_t infile_segment_offset;
	uint64_t infile_segment_size;
	unsigned char architecture;
} SEGMENT_DATA, * PSEGMENT_DATA;


//Returns the string at the <ordinal> position in pointer provided
//The size limit is respected. Strings not null terminated (even when at the end of pointer) will not be returned.
//If reached end of limit will return ""
//Ordinal starts at 1
std::string getNextStringFromMemory(const char* addr, uint64_t size, int ordinal);
std::optional<QDATA_STORAGE> getSegmentStorage(HMODULE component_handle);
std::optional<QDATA_STORAGE> getSegmentStorage(const char* dllpath);

std::optional<SEGMENT_DATA> getSegmentInfo(HMODULE component_handle, const char* segment_name);
std::optional<SEGMENT_DATA> getSegmentInfo(const char* dllpath, const char* segment_name);

// Decrypts the segment from provided dll handle/memory using the already stored password
// The length of the password is taken internally but could be desynchronized from the one that encrypted the segment
bool decryptSegmentInMemory(HMODULE component_handle, const char* segment_name);
bool encryptSegmentInFile(const char* dllpath, const char* segment_name, const uint8_t* key, int pwLength);
bool decryptSegmentInFile(const char* dllpath, const char* segment_name, const uint8_t* key, int pwLength);

bool writeRawInSegmentInFile(const char* dllpath, const char* segment_name, uint64_t offset, const char* data, uint64_t data_size);
bool writeRawInSegmentInFile(const char* dllpath, const char* segment_name, const char* data, uint64_t data_size);
bool writeQDataStorage(const char* dllpath, QDATA_STORAGE storage);

bool revertRelocatorChanges(HMODULE component_handle);
bool rebaseModuleSegment(HMODULE component_handle);
