add_executable(glsl-path-tracer
    GlslPathTracer.cpp
)

target_include_directories(glsl-path-tracer PRIVATE
    ${PROJECT_SOURCE_DIR}/source/common/
)

target_link_libraries(glsl-path-tracer PRIVATE
    pt::base
    pt::opengl
    pt::app
)

target_compile_definitions(glsl-path-tracer PRIVATE
    -DDATA_DIR="${PROJECT_SOURCE_DIR}/data/"
)