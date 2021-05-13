#include <iostream>

#include "base/format.hpp"

int main(int argc, const char** argv) {
    std::cout << gfx::format("Hello, %s", "world") << std::endl;
    return 0;
}
