#include "Window.h"
#include "base/Exception.h"
#include <GLFW/glfw3.h>

namespace pt {

void Window::initialize(int width, int height)
{
    glfwSetErrorCallback([](int, const char* error)
    {
        THROW_EXCEPTION(error);
    });

    glfwInit();

    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 6);

    m_pWindow = glfwCreateWindow(width, height, "GLSL path tracer", 0, 0);

    glfwMakeContextCurrent(m_pWindow);
}

void Window::finalize()
{
    glfwDestroyWindow(m_pWindow);
    glfwTerminate();
}

bool Window::shouldClose()
{
    return glfwWindowShouldClose(m_pWindow);
}

void Window::pollEvents()
{
    glfwPollEvents();
}

void Window::swapBuffers()
{
    glfwSwapBuffers(m_pWindow);
}

} // namespace pt
