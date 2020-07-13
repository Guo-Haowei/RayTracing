#include "Application.h"
#include <Windows.h>

// force NV card selection
extern "C" {
    _declspec(dllexport) DWORD NvOptimusEnablement = 0x00000001;
}

int main(const int argc, const char** argv)
{
    pt::g_pApp->processArguments(argc, argv);
    return pt::g_pApp->run();
}
