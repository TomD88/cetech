

#include <cetech/kernel/blob.h>
#include <cetech/kernel/module.h>
#include <cetech/kernel/api_system.h>

#include "celib/array.inl"


using namespace celib;

typedef celib::Array<uint8_t> blob_array;

namespace blob {
    uint8_t *data(ct_blob_instance_t *inst) {
        blob_array *array = (blob_array *) inst;
        return array::begin(*array);
    }

    uint64_t size(ct_blob_instance_t *inst) {
        blob_array *array = (blob_array *) inst;
        return array::size(*array);
    }

    void push(ct_blob_instance_t *inst,
              void *data,
              uint64_t size) {
        blob_array *array = (blob_array *) inst;
        return array::push(*array, (const uint8_t *) data, size);
    }

    ct_blob *create(cel_alloc *allocator) {
        blob_array *inst = CEL_NEW(allocator,
                                   blob_array,
                                   allocator);

        ct_blob *blob = CEL_NEW(allocator,
                                ct_blob);
        blob->inst = inst;
        blob->push = push;
        blob->size = size;
        blob->data = data;

        return blob;
    }

    void destroy(ct_blob *blob, bool free) {
        blob_array *inst = (blob_array *) blob->inst;
        cel_alloc *a = inst->_allocator;

        if(!free) {
            inst->_data = NULL;
        }

        CEL_DELETE(a, blob_array, inst);
        CEL_DELETE(a, ct_blob, blob);
    }

    static ct_blob_a0 api{
            .create = blob::create,
            .destroy = blob::destroy,
    };
}

CETECH_MODULE_DEF(
        blob,
        {
            CEL_UNUSED(api);
        },
        {
            CEL_UNUSED(reload);

            api->register_api("ct_blob_a0", &blob::api);
        },
        {
            CEL_UNUSED(api, reload);
        }
)