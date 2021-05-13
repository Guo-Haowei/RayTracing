#pragma once
#include <cassert>

namespace gfx {

/// TODO: assertmsg
#define DCHECK(expr, ...) (void)(!!(expr) || gfx::detail::dcheck(__LINE__, __FILE__, __FUNCTION__, #expr))

namespace detail {

[[noreturn]] bool dcheck(int ln, const char* file, const char* func, const char* cond);

}  // namespace detail

}  // namespace gfx
