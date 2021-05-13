#include "debug.hpp"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

namespace gfx {
namespace detail {

[[noreturn]] bool dcheck(int ln, const char* file, const char* func, const char* cond) {
    const char* sfile = strrchr(file, '\\');
    sfile = sfile ? sfile + 1 : file;
    fprintf(stderr, "DCHECK(%s) failed in %s(), at [%s:%d]\n", cond, func, sfile, ln);
    exit(1);
}

}  // namespace detail
}  // namespace gfx
