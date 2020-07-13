#pragma once
#include <string>
#include <ostream>

namespace pt {

class Exception
{
public:
    Exception(int line, const char* file, const std::string& error);

private:
    int         m_line;
    std::string m_file;
    std::string m_error;

    friend std::ostream& operator<<(std::ostream& os, const Exception& e);
};

} // namespace pt

#define THROW_EXCEPTION(ERROR_MESSAGE) \
    throw pt::Exception(__LINE__, __FILE__, ERROR_MESSAGE);
