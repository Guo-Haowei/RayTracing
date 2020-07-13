#pragma once
#include <string>

struct GLFWwindow;

namespace pt {

class Window
{
public:
    void initialize(int width, int height);
    void finalize();
    bool shouldClose();
    void pollEvents();
    void swapBuffers();
private:
    GLFWwindow* m_pWindow;
};

} // namespace pt

