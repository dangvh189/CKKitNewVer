using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CloudKitException : NSError
    {
        public CloudKitException(IntPtr pointer) : base(pointer) { }
        
        #region UserInfo Keyss
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKError_GetCKErrorRetryAfterKey();
        
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKError_GetCKRecordChangedErrorAncestorRecordKey();
        
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKError_GetCKRecordChangedErrorClientRecordKey();
        
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKError_GetCKRecordChangedErrorServerRecordKey();
        
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKError_GetCKPartialErrorsByItemIDKey();

        public static string CKErrorRetryAfterKey
        {
            get => CKError_GetCKErrorRetryAfterKey();
        }
        public static string CKPartialErrorsByItemIDKey
        {
            get => CKError_GetCKPartialErrorsByItemIDKey();
        }
        
        public static string CKRecordChangedErrorAncestorRecordKey
        {
            get => CKError_GetCKRecordChangedErrorAncestorRecordKey();
        }
        
        public static string CKRecordChangedErrorClientRecordKey
        {
            get => CKError_GetCKRecordChangedErrorClientRecordKey();
        }

         public static string CKRecordChangedErrorServerRecordKey
        {
            get => CKError_GetCKRecordChangedErrorClientRecordKey();
        }
        #endregion
    }
}