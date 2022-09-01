#include "../Core/PELoader.h"
#include "../Core/ComponentUtilities.h"

#include <Windows.h>
#include <string>
#include <shlwapi.h>
#include <time.h>
#include <random>
#include <set>
#include <ImageHlp.h>
#include <fstream>
#include <filesystem>
#include <vector>
#include <stdint.h>
#include <iostream>
#include <optional>
#include <vector>

using namespace std;


#define NR_OF_CLSIDS 4
#define CLSID_ENTRY_SIZE 128

unsigned int countFileRows(const char* path) {
	ifstream fi(path);
	unsigned int res = 0;

	if (!fi.good()) return 0;

	string line;
	while (getline(fi, line)) {
		res++;
	}

	return res;
}
string row(const char* path, unsigned int nr) {
	ifstream fi(path);
	string line;

	if (!fi.good()) return string("");

	while (getline(fi, line)) {
		if (nr == 0) {
			return line;
			break;
		}
		nr--;
	}

	return string("");
}
std::vector<int> shuffleArray(int count);

const char* help = "Incorrect nr of arguments"
"Options: dll_path mode[csv_path]"
"mode: loader | dll";

//arg[0] -- current path/filename
//arg[1] -- the program to be modified
//arg[2] -- clsid file
//Returns 0 as a success code; 10 as error
int main(int argc, char** argv) {
	string csv_path;
	string component_path;
	string mode;
	uint8_t password[STORAGE_PASSWORD_SIZE];

	// Init random
	srand(time(NULL));

	// We append a zero after each clsid and an extra one at the end
	if (NR_OF_CLSIDS * CLSID_ENTRY_SIZE + NR_OF_CLSIDS + 1 > STORAGE_EXTRA_MAX_SIZE) {
		cout << "ERROR: too many elements were being stored in qdata";
		return 10;
	}

	// Print help
	if (argc == 1) {
		cout << help << endl;
		return 10;
	}

	// Read arguments
	component_path = string(argv[1]);
	mode = string(argv[2]);

	// Generate password
	for (int i = 0; i < STORAGE_PASSWORD_SIZE; i++) {
		password[i] = rand() % 256;
	}

	optional<QDATA_STORAGE> storage = getSegmentStorage(component_path.c_str());
	if (!storage) {
		cout << "Could not read qdata from file" << endl;
		return 10;
	}
	// Copy generated password
	memcpy(storage->decryption_key, password, sizeof(storage->decryption_key));


	// We will not encrypt loader...yet
	if (mode == "dll") {
		if (!encryptSegmentInFile(component_path.c_str(), segment_crypt, password, STORAGE_PASSWORD_SIZE)) {
			cout << "Error while encrypting file " << component_path << endl;
			return 10;
		}

		if (!writeQDataStorage(component_path.c_str(), storage.value())) {
			cout << "Failed saving QDATA_STORAGE to file" << endl;
			return 10;
		}

		cout << "Success encrypting " << component_path << endl;
		return 0;
	}

	if (mode == "loader") {
		unsigned int rows;

		if (argc != 4) {
			cout << "Not enough params" << endl;
			return 10;
		}

		// Read argument
		csv_path = string(argv[3]);
		rows = countFileRows(csv_path.c_str());

		if (rows < NR_OF_CLSIDS) {
			cout << "Not enough data in CSV file" << endl;
			return 10;
		}

		char* storage_extra = storage->extra;
		std::vector<int> shuffled_indexes = shuffleArray(rows);
		for (int i = 0; i < NR_OF_CLSIDS; i++) {
			unsigned int rand_row = shuffled_indexes[i];
			string line = row(csv_path.c_str(), rand_row);

			if (line.size() == 0) {
				cout << "Randomly fetched line from csv file is empty - file should not have empty lines" << endl;
				return 10;
			}

			// +1 the clsid null terminator
			// +1 the (possible) final double terminator
			if (storage_extra - storage->extra + line.size() + 1 + 1 > STORAGE_EXTRA_MAX_SIZE) {
				cout << "Storage memory was about to be overrun. Could not store/fit some of the clsids." << endl;
				return 10;
			}

			// Safe because the destination is guaranteed to have free space for null
			int remaining_storage_size = STORAGE_EXTRA_MAX_SIZE - (storage_extra - storage->extra);
			strncpy_s(storage_extra, remaining_storage_size, line.c_str(), line.size());
			storage_extra += line.size() + 1;
		}

		// Mark the end of the array plus safety end markup
		*storage_extra = 0;
		storage->extra[STORAGE_EXTRA_MAX_SIZE - 2] = 0;
		storage->extra[STORAGE_EXTRA_MAX_SIZE - 1] = 0;

		if (!writeQDataStorage(component_path.c_str(), storage.value())) {
			cout << "Failed saving QDATA_STORAGE to file" << endl;
			return 10;
		}

		cout << "Success injecting clsid's " << component_path << endl;
		return 0;

	}

	cout << "Unknown mode" << endl;
	return 10;


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


// Create a shuffled array from 0 to count-1
std::vector<int> shuffleArray(int count) {
	std::vector<int> items;
	std::vector<int> randomized;

	for (int i = 0; i < count; i++) {
		items.push_back(i);
	}

	while (items.size() != 0) {
		int offset = rand() % items.size();
		int item = items[offset];
		randomized.push_back(item);
		items.erase(std::next(items.begin(), offset));
	}

	return randomized;
}