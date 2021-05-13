add_executable(renderer
    base/debug.cpp
    main.cpp
)

target_include_directories(renderer PRIVATE
    ${CMAKE_SOURCE_DIR}/
)
