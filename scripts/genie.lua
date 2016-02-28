--------------------------------------------------------------------------------
ROOT_DIR = path.getabsolute(path.join( ".."))
BUILD_DIR = path.join( ROOT_DIR, "build")

ARCH = os.is64bit() and '64' or '32'

if _OS == 'macosx' then
    _OS = 'darwin'
end

OS_ARCH = _OS .. ARCH

ENGINE_SRC_DIR = path.join(ROOT_DIR, 'sources')

EXTERNALS = path.join(ROOT_DIR, "externals")
EXTERNALS_BUILD = path.join( ROOT_DIR, EXTERNALS, "build")
EXTERNALS_LIB = path.join(EXTERNALS_BUILD, OS_ARCH, "lib")
EXTERNALS_INCLUDE = path.join( EXTERNALS_BUILD, OS_ARCH, "include")

print(OS_ARCH)

--------------------------------------------------------------------------------
newoption {
  trigger = 'runtime',
  value = "Runtime",
  description = "Runtime",
  allowed = {
    {'sdl2', "SDL2"}
  }
}
--------------------------------------------------------------------------------

function nuget_dir(dir_path)
    libdirs {
        path.join(ENGINE_SRC_DIR, "CETech", "packages", dir_path)
    }
end

solution "CETech"
    configurations {"Debug", "Develop", "Release"}
    platforms {"x32", "x64"}
    location(path.join(ENGINE_SRC_DIR,  "CETech"))

--    dofile(path.join(ROOT_DIR, "scripts", "toolchain.lua"))
--    if not toolchain(BUILD_DIR, EXTERNALS) then
--            return -- no action specified
--    end

project "CETech"
    kind "ConsoleApp"
    language "C#"

    location(path.join(ENGINE_SRC_DIR,  "CETech"))

    files {
        path.join(ENGINE_SRC_DIR, "CETech", "**.cs"),
    }

    nuget_dir("MoonSharp.1.5.0.1/lib/portable-net4+sl5+wp8+win8")
    nuget_dir("YamlDotNet.3.8.0/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1")
    nuget_dir("EntityFramework.6.0.0/lib/net45/EntityFramework")
    nuget_dir("MsgPack.Cli.0.6.8/lib/net45")
    nuget_dir("System.Data.SQLite.Core.1.0.99.0/lib/net45")
    nuget_dir("System.Data.SQLite.Linq.1.0.99.0/lib/net45")

    libdirs {
      EXTERNALS_LIB,
    }

    links {
        "System",
        "MoonSharp.Interpreter.dll",
        "YamlDotNet.dll",
        "MsgPack.dll"
    }

    defines {
        'CETECH_SDL2'
    }

    configuration "Debug"
        targetdir "bin/Debug"
        targetsuffix 'Debug'
        defines {
          "DEBUG",
          "CETECH_DEBUG",
          "CETECH_DEVELOP",
        }


    configuration "Develop"
        targetdir "bin/Develop"
        targetsuffix 'Develop'

        defines {
          "CETECH_DEVELOP",
        }

    configuration {}



--function cetech_project(name, test)
--    project(name)
--        kind "ConsoleApp"
--        language "C++"
--
--        files {
--            path.join(ENGINE_SRC_DIR, "cetech", "**.cc"),
--            path.join(ENGINE_SRC_DIR, "cetech", "**.h"),
--            path.join(ENGINE_SRC_DIR, "celib", "**.cc"),
--            path.join(ENGINE_SRC_DIR, "celib", "**.h"),
--
--            path.join(EXTERNALS_INCLUDE, "mpack", "*.c"),
--        }
--
--        if test then
--            files {
--                path.join(ENGINE_SRC_DIR, "tests/tests_main.cc"),
--            }
--
--            excludes {
--                path.join(ENGINE_SRC_DIR, "cetech", "main.cc"),
--            }
--
--            defines {
--                'CETECH_TEST'
--            }
--
--            removeflags {
--                "NoRTTI",
--                "NoExceptions",
--            }
--        end
--
--        links {
--            "luajit",
--            "enet",
--            "SOIL",
--            "sqlite3",
--            "yaml",
--            "nanomsg",
--            "anl"
--        }
--
--        configuration "Release or Develop"
--            links {
--                "bgfxRelease",
--            }
--
--        configuration "Debug"
--            links {
--                --"bgfxDebug",
--                "bgfxRelease",
--            }
--
--        configuration ("linux*")
--            links {
--                'SDL2',
--                'dl',
--                'pthread',
--                "GL",
--                "X11",
--                "rt",
--            }
--
--        configuration ("osx*")
--            linkoptions {
--                "-framework Cocoa",
--                "-framework Metal",
--                "-framework QuartzCore",
--                "-framework OpenGL",
--                "-framework ForceFeedback",
--                "-framework Carbon",
--                "-framework IOKit",
--                "-framework CoreAudio",
--                "-framework AudioToolbox",
--                "-framework AudioUnit",
--                '-pagezero_size 10000',
--                '-image_base 100000000',
--            }
--
--            links {
--                'SDL2',
--                'dl',
--                'pthread',
--                'iconv',
--            }
--
--        strip()
--end
--
--cetech_project('cetech', false)
--cetech_project('cetech_test', true)
