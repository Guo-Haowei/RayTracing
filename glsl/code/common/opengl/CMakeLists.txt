add_library(opengl
    RenderSystem.cpp
    GlslProgram.cpp
)

target_link_libraries(opengl PUBLIC
    glad
)

target_include_directories(opengl PUBLIC
    ${PROJECT_SOURCE_DIR}/code/common/
    ${PROJECT_SOURCE_DIR}/external/glad/include
)

add_library(pt::opengl ALIAS opengl)
