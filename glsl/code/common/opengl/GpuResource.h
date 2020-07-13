#pragma once
#include "GLPrerequisites.h"

namespace pt {

class GpuResource
{
public:
    enum : GLuint { NULL_HANDLE = 0 };
public:
    GpuResource() = default;
    ~GpuResource() = default;

    inline GLuint getHandle() const { return m_handle; }
protected:
    GLuint m_handle = NULL_HANDLE;
};

#define GPU_RESOURCE_DESTRUCTOR( CLASS ) \
    ~CLASS() \
    { \
        if (m_handle != NULL_HANDLE) destroy(); \
        m_handle = NULL_HANDLE; \
    }

} // namespace pt
