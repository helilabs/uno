cmake_minimum_required(VERSION 3.4.1)
project(@(Project.Name:QuoteSpace))

include(AndroidNdkModules)
include_directories(include@(IncludeDirectory:QuoteSpace:Join('', ' ')))
link_directories(@(LinkDirectory:QuoteSpace:Join(' ')))
add_definitions(@(PreprocessorDefinition:Join(' ', '-D')))

set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -std=@(CStandard)")
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=@(CppStandard) -Wno-unused-value -Wno-dangling-else -Wno-switch -Wno-invalid-offsetof -fexceptions -frtti")

# Strip release builds to reduce file size.
if (CMAKE_BUILD_TYPE STREQUAL "Release")
    set(CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} -s")
endif()

add_library(@(Activity.Name) SHARED
    @(HeaderFile:Join('\n    ', '"include/', '"'))
    @(SourceFile:Join('\n    ', '"jni/', '"')))

target_link_libraries(@(Activity.Name) @(LinkLibrary:Join(' ','"','"')))
target_link_libraries(@(Activity.Name) @(JNI.StaticLibraries:Join(' ','"','"')))
