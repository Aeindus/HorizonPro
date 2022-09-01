#pragma once

#include <string>
#include <stdexcept>

#ifdef _DEBUG
#define DbgException(x) std::runtime_error(x); 
#else
#define DbgException(x) std::runtime_error("Except"); 
#endif


// A custom and proper implementation of safe strcpy which appends null terminator at the end and padds with zeroes.
template<size_t size_dest>
constexpr void cstrncpy(char(&dest)[size_dest], const char* src, size_t count) {
	// We immitate the behaviour of strncpy_s which sets the error if truncation would occur
	// By substracting 1 we create the truncate efect
	if (count >= size_dest) count = size_dest;	//count = size_dest - 1;

	size_t src_len = strnlen(src, count + 1);
	// size of the final string in destination cropped for the destination size and the actual provided count of chars
	size_t in_dest_size = min(count, src_len);

	memset(dest, 0, size_dest);

	// Error mode - no truncation allowed
	if (in_dest_size >= size_dest) {
		dest[0] = 0;
		return;
	}

	memcpy(dest, src, in_dest_size);
	dest[in_dest_size] = 0;	// Enforce null terminator
}