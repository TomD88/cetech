
#include <engine/entcom/types.h>
#include <engine/world_system/transform.h>
#include <celib/math/mat44f.h>
#include <engine/renderer/mesh_renderer.h>
#include <engine/renderer/material.h>
#include <celib/stringid/stringid.h>
#include "engine/lua_system/lua_system.h"

#define API_NAME "Material"

static int _set_texture(lua_State *l) {
    material_t m = {.h = luasys_to_handler(l, 1)};
    const char *slot_name = luasys_to_string(l, 2);
    const char *texture_name = luasys_to_string(l, 3);

    material_set_texture(m, slot_name, stringid64_from_string(texture_name));
    return 0;
}

void _register_lua_material_api() {
    luasys_add_module_function(API_NAME, "set_texture", _set_texture);
}