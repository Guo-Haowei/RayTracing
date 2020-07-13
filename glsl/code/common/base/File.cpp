#pragma once
#include "File.h"
#include "Exception.h"
#include <string>
#include <fstream>
#include <streambuf>

namespace pt {
namespace utility {

std::string readAsciiFile(const char* file)
{
    std::ifstream ifs(file);
    if (!ifs.is_open())
    {
        std::string error("Failed to open file ["); error.append(file).append("]");
        THROW_EXCEPTION(error);
    }

    return std::string((std::istreambuf_iterator<char>(ifs)),
                        std::istreambuf_iterator<char>());
}

inline std::string readAsciiFile(const std::string& file)
{
    return readAsciiFile(file.c_str());
}

} // namespace utility
} // namespace pt
