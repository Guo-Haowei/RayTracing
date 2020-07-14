cmake_minimum_required(VERSION 3.5.1)
project(cu-ray)
enable_language(CUDA)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
add_subdirectory(code)
