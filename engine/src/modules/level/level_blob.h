#ifndef CETECH_LEVEL_BLOB_H
#define CETECH_LEVEL_BLOB_H


typedef struct level_blob {
    uint32_t entities_count;
    // uint64_t names[entities_count];
    // uint32_t offset[entities_count];
    // uint8_t data[*];
} level_blob_t;

#define level_blob_names(r) ((uint64_t*) ((r) + 1))
#define level_blob_offset(r) ((uint32_t*) (level_blob_names(r) + ((r)->entities_count)))
#define level_blob_data(r) ((uint8_t*) (level_blob_offset(r) + ((r)->entities_count)))

#endif // CETECH_LEVEL_BLOB_H