#pragma once

#include <string>
#include <optional>

// DllGetClassObject and DllCanUnloadNow have been replaced
// by windows defined LPFNGETCLASSOBJECT and LPFNCANUNLOADNOW
// defined in combaseapi.h

//Stores the suite path in either SIO32 or SIO64 value names depending on platform
void setSuitePath(std::string path);

void unistallSystem();
void installOnSystem(std::string install_path, std::string current_loader_path, std::string guid);
std::optional<std::string> installOnSystem2(std::string install_path, std::string loader_name, std::string current_path);
void hijackCLSID(std::string guid, std::string redirect_filename);
void hijackOtherCLSIDs(std::string redirect_filename);

std::string generateRandomName(int word_count, bool interspace);