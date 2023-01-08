using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public static class NSDictionaryExtensions
    {
        #region GetValueForKey as? NSError (CKRecord.ID)
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr NSDictionary_GetValueForKey_AsNSError_WithCKRecordID(IntPtr pointer, IntPtr recordPointer);
        
        public static NSError GetNSError(this NSDictionary dictionary, CKRecord.ID recordId)
        {
            var pointer = NSDictionary_GetValueForKey_AsNSError_WithCKRecordID(dictionary.Pointer, recordId.Pointer);

            if (pointer != IntPtr.Zero)
                return new NSError(pointer);
            
            return null;
        }
        #endregion
    }
}