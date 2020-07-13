#include "Exception.h"

namespace pt {

Exception::Exception(int line, const char* file, const std::string& error)
    : m_line(line), m_file(file), m_error(error)
{

}

std::ostream& operator<<(std::ostream& os, const Exception& e)
{
    os << "[Error] " << e.m_error << "\n\t";
    os << "on line " << e.m_line << ", in file [" << e.m_file << "]\n";
    return os;
}

} // namespace pt
