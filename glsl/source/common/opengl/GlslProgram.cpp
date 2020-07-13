#pragma once
#include "GlslProgram.h"
#include "base/Exception.h"
#include "base/File.h"
#include <iostream>

namespace pt {

static constexpr int LOG_SIZE = 512;

////////////////////////////////////////////////////////////////////////////////
/// GlslShader declaration
class GlslShader : public GpuResource
{
public:
    GPU_RESOURCE_DESTRUCTOR( GlslShader );

    void destroy();
    void createFromFile(const char* path, GLenum shaderType);
};

////////////////////////////////////////////////////////////////////////////////
//// GlslShader implementation
void GlslShader::destroy()
{
    glDeleteShader(m_handle);
    m_handle = NULL_HANDLE;
}

void GlslShader::createFromFile(const char* path, GLenum shaderType)
{
    std::string source = utility::readAsciiFile(path);
    m_handle = glCreateShader(shaderType);
    const char* sources[] = { source.c_str() };
    glShaderSource(m_handle, 1, sources, NULL);
    glCompileShader(m_handle);
    int success;
    char log[LOG_SIZE];
    glGetShaderiv(m_handle, GL_COMPILE_STATUS, &success);
    if (!success)
    {
        glGetShaderInfoLog(m_handle, LOG_SIZE, NULL, log);
        log[LOG_SIZE - 1] = '\0'; // prevent overflow
        std::string error("Failed to compile shader [");
        error.append(path).append("]\n").append(std::string(80, '-')).push_back('\n');
        error.append(log).append(std::string(80, '-'));
        THROW_EXCEPTION(error);
    }
}

////////////////////////////////////////////////////////////////////////////////
/// GlslProgram implementation
void GlslProgram::use()
{
    glUseProgram(m_handle);
}

void GlslProgram::stop()
{
    glUseProgram(0);
}

void GlslProgram::destroy()
{
    glDeleteProgram(m_handle);
    m_handle = NULL_HANDLE;
}

void GlslProgram::linkProgram(const char* path)
{
    // link program
    glLinkProgram(m_handle);
    // check link status
    int success;
    char log[LOG_SIZE];
    glGetProgramiv(m_handle, GL_LINK_STATUS, &success);
    if (!success)
    {
        glGetProgramInfoLog(m_handle, LOG_SIZE, NULL, log);
        log[LOG_SIZE - 1] = '\0'; // prevent overflow
        std::string error("Failed to link shader [");
        error.append(path).append("]\n").append(std::string(80, '-')).push_back('\n');
        error.append(log).append(std::string(80, '-'));
        THROW_EXCEPTION(error);
    }
}

void GlslProgram::createFromFile(const char* comp)
{
    m_handle = glCreateProgram();
    GlslShader computeShader;
    computeShader.createFromFile(comp, GL_COMPUTE_SHADER);
    glAttachShader(m_handle, computeShader.getHandle());

    linkProgram(comp);
}


void GlslProgram::createFromFile(const char* vert, const char* frag, const char* geom)
{
    m_handle = glCreateProgram();

    GlslShader vertexShader;
    vertexShader.createFromFile(vert, GL_VERTEX_SHADER);
    glAttachShader(m_handle, vertexShader.getHandle());
    GlslShader fragmentShader;
    fragmentShader.createFromFile(frag, GL_FRAGMENT_SHADER);
    glAttachShader(m_handle, fragmentShader.getHandle());
    GlslShader geometryShader;
    if (geom != nullptr)
    {
        geometryShader.createFromFile(geom, GL_GEOMETRY_SHADER);
        glAttachShader(m_handle, geometryShader.getHandle());
    }

    linkProgram(vert);
}

GLint GlslProgram::getUniformLocation(const char* name)
{
    GLint location = glGetUniformLocation(m_handle, name);
#ifdef _DEBUG
    if (location == -1)
        std::cerr << "[Warnning] uniform [" << name << "] not found" << std::endl;
#endif
    return location;
}

void GlslProgram::setUniform(GLint location, float x, float y, float z)
{
    glUniform3f(location, x, y, z);
}

} // namespace pt
