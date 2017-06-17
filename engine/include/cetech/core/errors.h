#ifndef CETECH_ERRORS_H
#define CETECH_ERRORS_H

#ifdef __cplusplus
extern "C" {
#endif

//==============================================================================
// Includes
//==============================================================================

#include <stdlib.h>
#include <stdio.h>

//==============================================================================
// Iterface
//==============================================================================

#ifdef CETECH_DEBUG
#define CETECH_ASSERT(where, condition)                                        \
    do {                                                                       \
        if (!(condition)) {                                                    \
            error_assert(where, #condition, __FILE__, __LINE__);               \
        }                                                                      \
    } while (0)
#else
#define CETECH_ASSERT(where, condition) do {} while (0)
#endif


struct error_api_v0 {
    void (*assert)(const char *where,
                   const char *condition,
                   const char *filename,
                   int line);
};

void error_assert(const char *where,
                  const char *condition,
                  const char *filename,
                  int line);

#ifdef __cplusplus
}
#endif

#endif //CETECH_ERRORS_H