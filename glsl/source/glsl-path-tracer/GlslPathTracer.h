#include "application/Application.h"
#include "opengl/RenderSystem.h"
#include "opengl/GlslProgram.h"

namespace pt {

class GlslPathTracer : public Application
{
    struct QuadBuffer
    {
        void create();
        void destroy();

        GLuint vao;
        GLuint vbo;
    };
public:
    virtual void processArguments(int argc, const char** argv) override;
protected:
    virtual void update(float time) override;
    virtual void initialize() override;
    virtual void finalize() override;
protected:
    RenderSystem m_renderSystem;
    /// shaders
    GlslProgram m_fullscreenProgram;
    GlslProgram m_pathTraceProgram;
    /// texture
    GLuint m_output;
    /// buffers
    QuadBuffer m_quadBuffer;

    GLuint m_constantBuffer;
    GLuint m_triangleBuffer;
};

} // namespace pt

