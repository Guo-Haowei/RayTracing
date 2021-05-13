#pragma once
#include <cstdio>
#include <string>

#include "debug.hpp"

namespace gfx {

template <typename... Args>
std::string format(const char* format, Args... args) {
    int length = std::snprintf(nullptr, 0, format, args...);
    DCHECK(length >= 0);
    std::string buf(size_t(length + 1), '\0');
    std::snprintf(buf.data(), length + 1, format, args...);
    return buf;
}

}  // namespace gfx
