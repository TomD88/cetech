#pragma once

/*******************************************************************************
**** Includes
*******************************************************************************/

#include "celib/memory/memory_types.h"
#include "rapidjson/document.h"

/*******************************************************************************
**** Interface
*******************************************************************************/
namespace cetech {

    /***************************************************************************
    **** Console server is develop comunication layer.
    ***************************************************************************/
    namespace console_server {

        /***********************************************************************
        **** Command callback.
        ***********************************************************************/
        typedef void (* command_clb_t)(const rapidjson::Document&,
                                       rapidjson::Document&);

        /***********************************************************************
        **** Init network layer.
        ***********************************************************************/
        void init();

        /***********************************************************************
        **** Proccess network.
        ***********************************************************************/
        void tick();

        /***********************************************************************
        **** Has console server any clients?
        ***********************************************************************/
        bool has_clients();

        /***********************************************************************
        **** Register command.
        ***********************************************************************/
        void register_command(const char* name,
                              const command_clb_t clb);

        /***********************************************************************
        **** Send json document.
        ***********************************************************************/
        void send_json_document(const rapidjson::Document& document);
    }

    /***************************************************************************
    **** Console server globals function.
    ***************************************************************************/
    namespace console_server_globals {

        /***********************************************************************
        **** Init system.
        ***********************************************************************/
        void init();

        /***********************************************************************
        **** Shutdown system.
        ***********************************************************************/
        void shutdown();
    }
}
