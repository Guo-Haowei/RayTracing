add_executable(cu-ray
    main.cu
)

target_include_directories(cu-ray PRIVATE
    ${PROJECT_SOURCE_DIR}/external/stb/
)
