#pragma once
#include "GLPrerequisites.h"

namespace pt {

class RenderSystem
{
public:
    void initialize();
    void finalize();
    void getWorkGroupCount();
public:
    /// TODO: refactor
    int x, y, z;
    int invocation;
};

} // namespace pt
