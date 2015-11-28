#pragma once

/*******************************************************************************
**** Includes
*******************************************************************************/

#include "celib/defines.h"

/*******************************************************************************
**** Interface
*******************************************************************************/
namespace cetech {

    /***************************************************************************
    **** Keyboard input manager.
    ***************************************************************************/
    namespace keyboard {

        /***********************************************************************
        **** Process keyboard for curent frame.
        ***********************************************************************/
        void process_keyboard();

        /***********************************************************************
        **** Get button_name index.
        ***********************************************************************/
        uint32_t button_index(const char* button_name);

        /***********************************************************************
        **** Get button_index name.
        ***********************************************************************/
        const char* button_name(const uint32_t button_index);

        /***********************************************************************
        **** Button state in curent frame.
        ***********************************************************************/
        bool button_state(const uint32_t button_index);

        /***********************************************************************
        **** Is button pressed in curent frame. Last frame: 0, Curent: 1
        ***********************************************************************/
        bool button_pressed(const uint32_t button_index);

        /***********************************************************************
        **** Is button released in curent frame. Last frame: 1, Curent: 0
        ***********************************************************************/
        bool button_released(const uint32_t button_index);
    }
}
