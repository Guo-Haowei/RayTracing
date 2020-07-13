#pragma once
#include "Window.h"

namespace pt {

extern const int WIDTH;
extern const int HEIGHT;

class Application
{
public:
    int run();
    virtual void processArguments(int argc, const char** argv) = 0;
protected:
    virtual void update(float time) = 0;
    virtual void initialize();
    virtual void finalize();
    void mainloop();
protected:
    Window m_window;
};

extern Application* g_pApp;

} // namespace pt
