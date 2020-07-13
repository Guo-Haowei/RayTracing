add_library(base
    Exception.cpp
    File.cpp
)

add_library(pt::base ALIAS base)
