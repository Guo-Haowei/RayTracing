#pragma once
#include <string>

namespace pt {
namespace utility {

extern std::string readAsciiFile(const char* file);
extern std::string readAsciiFile(const std::string& file);

} // namespace utility
} // namespace pt
