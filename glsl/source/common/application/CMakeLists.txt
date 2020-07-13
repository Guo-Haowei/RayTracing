add_library(app
    Application.cpp
    main.cpp
    Window.cpp
)

target_include_directories(app PRIVATE
    ${PROJECT_SOURCE_DIR}/source/common/
    ${PROJCET_SOURCE_DIR}/external/glfw/include
)

target_link_libraries(app PRIVATE
    glfw
)

add_library(pt::app ALIAS app)
