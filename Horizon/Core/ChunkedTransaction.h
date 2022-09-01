#pragma once
#include "HorizonClient.h"
#include "TcpShortDef.h"

class ChunkedTransaction {
	TOKEN_KEY end_chunk_token = {};
	uint64_t chunk_elements = 0;
	bool processing_started = false;
	HorizonClient& client;

public:
	ChunkedTransaction(HorizonClient& client) :client(client) {}

	uint64_t getChunksCount();
	bool isProcessing();

	//Read starting transaction packet and store ending token, item count etc.
	//Sends error in case of error.
	//Returns false if an error was detected and automatically calls dispose
	bool startChunk();

	bool endChunk();
};

