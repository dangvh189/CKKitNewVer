//
//  CloudSavesWrapper.h
//  CloudSavesWrapper
//
//  Copyright © 2021 Apple, Inc. All rights reserved.
//

#import <Foundation/Foundation.h>

//! Project version number for CloudSavesWrapper.
FOUNDATION_EXPORT double CloudSavesWrapperVersionNumber;

//! Project version string for CloudSavesWrapper.
FOUNDATION_EXPORT const unsigned char CloudSavesWrapperVersionString[];

//! iOS & tvOS Frameworks do not support bridging headers...
#if TARGET_OS_IOS || TARGET_OS_TV
    #include <stdbool.h>

#include "AppleCoreRuntimeShared.h"

#endif
