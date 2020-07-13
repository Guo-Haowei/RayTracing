#include "Application.h"
#include "base/Exception.h"
#include <iostream>

namespace pt {

int Application::run()
{
    try
    {
        initialize();

        while (!m_window.shouldClose())
        {
            mainloop();
        }

        finalize();
        return 0;
    }
    catch(const Exception& e)
    {
        std::cout << e << std::endl;
        return -1;
    }
}

void Application::initialize()
{
    m_window.initialize(WIDTH, HEIGHT);
}

void Application::mainloop()
{
    m_window.pollEvents();
    update(0.0f);
    m_window.swapBuffers();
}

void Application::finalize()
{
    m_window.finalize();
}

} // namespace pt

