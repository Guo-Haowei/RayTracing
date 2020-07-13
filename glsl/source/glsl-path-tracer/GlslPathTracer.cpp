#include "GlslPathTracer.h"
#include "base/GeoMath.h"
#include <vector>
#include <iostream>
using std::vector;

namespace pt {

// TODO: refactor
const int WIDTH = 768;
const int HEIGHT = WIDTH;

struct ConstantBufferCache
{
    vec3 camera_origin  = vec3(0, 0, 2.8);
    int frame           = 0;
};

struct Triangle
{
    vec3 A;
    float _padding0;
    vec3 B;
    float _padding1;
    vec3 C;
    int material;

    Triangle() {}
    Triangle(const vec3& A, const vec3& B, const vec3& C, int material)
        : A(A), B(B), C(C), material(material)
    {}
};

static_assert(sizeof(ConstantBufferCache) % sizeof(vec4) == 0);
static_assert(sizeof(Triangle) % sizeof(vec4) == 0);

static ConstantBufferCache cache {};
static vector<Triangle> g_trianges;

/**
 *        E__________________ H
 *       /|                 /|
 *      / |                / |
 *     /  |               /  |
 *   A/___|______________/D  |
 *    |   |              |   |
 *    |   |              |   |
 *    |   |              |   |
 *    |  F|______________|___|G
 *    |  /               |  /
 *    | /                | /
 *   B|/_________________|C
 *
 */
static void addTriangle(const vec3& center, const vec3& size, int material, float rotateY)
{
    enum { A = 0, B, C, D, E, F, G, H };
    vector<vec3> points =
    {
        vec3(-1, +1, +1) * size,
        vec3(-1, -1, +1) * size,
        vec3(+1, -1, +1) * size,
        vec3(+1, +1, +1) * size,
        vec3(-1, +1, -1) * size,
        vec3(-1, -1, -1) * size,
        vec3(+1, -1, -1) * size,
        vec3(+1, +1, -1) * size
    };

    mat4 rotate4x4 = rotate(mat4(1.0f), rotateY, vec3(0, 1, 0));
    mat3 rotate3x3 = mat3(rotate4x4);

    for (vec3& point : points)
    {
        point = rotate3x3 * point;
        point += center;
    }

    vector<uvec3> faces = {
        { A, B, D }, // ABD
        { D, B, C }, // DBC
        { E, H, F }, // EHF
        { H, G, F }, // HGF

        { D, C, G }, // DCG
        { D, G, H }, // DGH
        { A, F, B }, // AFB
        { A, E, F }, // AEF

        { A, D, H }, // ADH
        { A, H, E }, // AHE
        { B, F, G }, // BFG
        { B, G, C }, // BGC
    };

    for (const uvec3& face : faces)
    {
        g_trianges.push_back(Triangle(points[face.x], points[face.y], points[face.z], material));
    }
}

void GlslPathTracer::QuadBuffer::create()
{
    glCreateVertexArrays(1, &vao);
    glCreateBuffers(1, &vbo);

    float s = 1.0f;
    float vertices[] = {
        -s, s,
        -s, -s,
        s, -s,
        -s, s,
        s, -s,
        s, s,
    };

    glBindVertexArray(vao);
    glBindBuffer(GL_ARRAY_BUFFER, vbo);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
}

void GlslPathTracer::QuadBuffer::destroy()
{
    glDeleteVertexArrays(1, &vao);
    glDeleteBuffers(1, &vbo);
}

void GlslPathTracer::processArguments(int argc, const char** argv)
{

}

void GlslPathTracer::update(float time)
{
    // update uniform
    ++cache.frame;
    // cache.camera_origin.z -= 0.01f;
    glNamedBufferData(m_constantBuffer, sizeof(ConstantBufferCache), &cache, GL_DYNAMIC_DRAW);

    // compute
    m_pathTraceProgram.use();

    glDispatchCompute(WIDTH, HEIGHT, 1);

    glMemoryBarrier(GL_SHADER_IMAGE_ACCESS_BARRIER_BIT);

    glViewport(0, 0, WIDTH, HEIGHT);

    glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);

    m_fullscreenProgram.use();
    glDrawArrays(GL_TRIANGLES, 0, 6);
}

void GlslPathTracer::initialize()
{
    Application::initialize();

    // initialize opengl
    m_renderSystem.initialize();
    // get work group count
    m_renderSystem.getWorkGroupCount();
    // compile shaders
    m_fullscreenProgram.createFromFile(DATA_DIR "shaders/fullscreen.vert",
                                       DATA_DIR "shaders/fullscreen.frag");

    m_pathTraceProgram.createFromFile(DATA_DIR "shaders/pathtracer.comp");

    // create quad buffer
    m_quadBuffer.create();

    // create output image
    // dimensions of the image
    glGenTextures(1, &m_output);
    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, m_output);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA32F, WIDTH, HEIGHT, 0, GL_RGBA, GL_FLOAT, 0);
    glBindImageTexture(0, m_output, 0, GL_FALSE, 0, GL_WRITE_ONLY, GL_RGBA32F);

    glEnable(GL_CULL_FACE);

    // create constant buffer
    glGenBuffers(1, &m_constantBuffer);
    glBindBufferBase(GL_UNIFORM_BUFFER, 0, m_constantBuffer);

    glGenBuffers(1, &m_triangleBuffer);
    glBindBufferBase(GL_UNIFORM_BUFFER, 1, m_triangleBuffer);

    float lightSize = 0.3f;
    g_trianges =
    {
        // light
        Triangle(vec3(-lightSize, +0.99f, +lightSize), vec3(-lightSize, +0.99f, -lightSize), vec3(+lightSize, +0.99f, -lightSize), 0),
        Triangle(vec3(+lightSize, +0.99f, +lightSize), vec3(-lightSize, +0.99f, +lightSize), vec3(+lightSize, +0.99f, -lightSize), 0),
        // left
        Triangle(vec3(-1.0, +1.0, +1.0), vec3(-1.0, -1.0, -1.0), vec3(-1.0, +1.0, -1.0), 1),
        Triangle(vec3(-1.0, +1.0, +1.0), vec3(-1.0, -1.0, +1.0), vec3(-1.0, -1.0, -1.0), 1),
        // right
        Triangle(vec3(+1.0, -1.0, -1.0), vec3(+1.0, +1.0, +1.0), vec3(+1.0, +1.0, -1.0), 2),
        Triangle(vec3(+1.0, -1.0, +1.0), vec3(+1.0, +1.0, +1.0), vec3(+1.0, -1.0, -1.0), 2),
        // up
        Triangle(vec3(-1.0, +1.0, +1.0), vec3(-1.0, +1.0, -1.0), vec3(+1.0, +1.0, -1.0), 3),
        Triangle(vec3(+1.0, +1.0, +1.0), vec3(-1.0, +1.0, +1.0), vec3(+1.0, +1.0, -1.0), 3),
        // down
        Triangle(vec3(-1.0, -1.0, -1.0), vec3(-1.0, -1.0, +1.0), vec3(+1.0, -1.0, -1.0), 3),
        Triangle(vec3(-1.0, -1.0, +1.0), vec3(+1.0, -1.0, +1.0), vec3(+1.0, -1.0, -1.0), 3),
        // back
        Triangle(vec3(-1.0, +1.0, -1.0), vec3(-1.0, -1.0, -1.0), vec3(+1.0, -1.0, -1.0), 3),
        Triangle(vec3(+1.0, +1.0, -1.0), vec3(-1.0, +1.0, -1.0), vec3(+1.0, -1.0, -1.0), 3)
    };

    addTriangle(vec3(-0.4f, -0.6f, -0.4f), vec3(0.3f, 0.8f, 0.3f), 4, glm::radians(30.0f));

    glNamedBufferData(m_triangleBuffer, sizeof(Triangle) * g_trianges.size(), g_trianges.data(), GL_DYNAMIC_DRAW);
}

void GlslPathTracer::finalize()
{
    m_quadBuffer.destroy();
    m_fullscreenProgram.destroy();

    Application::finalize();
}

Application* g_pApp = new GlslPathTracer();

} // namespace pt
