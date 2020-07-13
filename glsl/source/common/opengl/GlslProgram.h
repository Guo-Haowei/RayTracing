#pragma once
#include "GpuResource.h"

namespace pt {

class GlslProgram : public GpuResource
{
public:
    GPU_RESOURCE_DESTRUCTOR( GlslProgram );

    void use();
    void stop();
    void destroy();
    void createFromFile(const char* vert, const char* frag, const char* geom = nullptr);
    void createFromFile(const char* comp);
    GLint getUniformLocation(const char* name);
    void setUniform(GLint location, float x, float y, float z);
private:
    void linkProgram(const char* path);
};

} // namespace pt
