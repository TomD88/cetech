#pragma once

/*******************************************************************************
**** Includes
*******************************************************************************/

#include "celib/string/types.h"
#include "celib/memory/types.h"

#include "cetech/filesystem/file.h"
#include "cetech/resource_compiler/resource_compiler.h"

/*******************************************************************************
**** Interface
*******************************************************************************/
namespace cetech {

    /***************************************************************************
    **** Lua resource
    ***************************************************************************/
    namespace resource_lua {

        /***********************************************************************
        **** Resource structure.
        ***********************************************************************/
        struct Resource {
            uint32_t type;
            uint32_t size;
        };

        /***********************************************************************
        **** Resource type hash.
        ***********************************************************************/
        StringId64_t type_hash();

        /***********************************************************************
        **** Resouce compiler.
        ***********************************************************************/
        void compile(const char* filename,
                     CompilatorAPI& compilator);

        /***********************************************************************
        **** Resouce loader.
        ***********************************************************************/
        char* loader(FSFile& f,
                     Allocator& a);

        /***********************************************************************
        **** Resouce online callback.
        ***********************************************************************/
        void online(void* data);

        /***********************************************************************
        **** Resouce offline callback.
        ***********************************************************************/
        void offline(void* data);

        /***********************************************************************
        **** Resouce unloader.
        ***********************************************************************/
        void unloader(Allocator& a,
                      void* data);

        /***********************************************************************
        **** Get source from resource.
        ***********************************************************************/
        const char* get_source(const Resource* rs);
    }
}