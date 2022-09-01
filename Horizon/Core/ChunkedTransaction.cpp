#include "ChunkedTransaction.h"

uint64_t ChunkedTransaction::getChunksCount() {
	return chunk_elements;
}
bool ChunkedTransaction::isProcessing() {
	return processing_started;
}

bool ChunkedTransaction::startChunk() {
	if (processing_started) {
		sendError("Transaction still processing");
		return false;
	}

	if (!recvData((char*)&chunk_elements, 4)) {
		return false;
	}
	if (!recvData(end_chunk_token.data(), end_chunk_token.size())) {
		return false;
	}

	if (chunk_elements < 10) {
		sendError("Transaction limit too small");
		return false;
	}

	processing_started = true;

	return true;
}

bool ChunkedTransaction::endChunk() {
	if (!processing_started) {
		sendError("Transaction wasn't started");
		return false;
	}

	// End the chunk
	client.setToken(end_chunk_token);
	if (!sendHeader(PacketDataTypes::stop_chunked_operation, 0, "")) {
		return false;
	}

	processing_started = false;
	return true;
}