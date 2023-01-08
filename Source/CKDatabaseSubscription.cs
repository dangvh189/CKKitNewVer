using System;
using System.Runtime.InteropServices;

namespace Apple.CloudKit
{
    /// <summary>
    /// A subscription that generates push notifications when CloudKit modifies records in a database.
    /// </summary>
    public class CKDatabaseSubscription : CKSubscription
    {
        #region Init & Dispose
        public CKDatabaseSubscription(IntPtr pointer) : base(pointer) {}

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKDatabaseSubscription_Init(string subscriptionId);
        
        public static CKDatabaseSubscription Init(string subscriptionId)
        {
            var pointer = CKDatabaseSubscription_Init(subscriptionId);
            
            if(pointer != IntPtr.Zero)
                return new CKDatabaseSubscription(pointer);

            return null;
        }
        #endregion
        
        #region RecordType
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKDatabaseSubscription_GetRecordType(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKDatabaseSubscription_SetRecordType(IntPtr pointer, string recordType);

        /// <summary>
        /// The type of record that the subscription queries.
        /// </summary>
        public string RecordType
        {
            get => CKDatabaseSubscription_GetRecordType(Pointer);
            set => CKDatabaseSubscription_SetRecordType(Pointer, value);
        }
        #endregion
    }
}