#ifndef CETECH_BLOB_H
#define CETECH_BLOB_H

#ifdef __cplusplus
extern "C" {
#endif

//==============================================================================
// Includes
//==============================================================================

#include <stdint.h>

struct cel_alloc;

//==============================================================================
// Typedefs
//==============================================================================

typedef void ct_blob_instance_t;

//==============================================================================
// Api
//==============================================================================

struct ct_blob {
    ct_blob_instance_t *inst;

    uint8_t *(*data)(ct_blob_instance_t *inst);

    uint64_t (*size)(ct_blob_instance_t *inst);

    void (*push)(ct_blob_instance_t *inst,
                 void *data,
                 uint64_t size);
};

struct ct_blob_a0 {
    struct ct_blob *(*create)(struct cel_alloc *allocator);

    void (*destroy)(struct ct_blob *blob, bool free);
};

#ifdef __cplusplus
}
#endif

#endif //CETECH_BLOB_H
