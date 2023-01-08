using System;
using System.Runtime.InteropServices;

namespace Apple.CloudKit
{
    /// <summary>
    /// A subscription that generates push notifications when CloudKit modifies records in a specific record zone.
    /// </summary>
    public class CKRecordZoneSubscription : CKSubscription
    {
        #region Init & Dispose
        public CKRecordZoneSubscription(IntPtr pointer) : base(pointer) {}
        
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecordZoneSubscription_Init(IntPtr zoneId, string subscriptionId);

        public static CKRecordZoneSubscription Init(CKRecordZone.ID zoneId, string subscriptionId)
        {
            var pointer = CKRecordZoneSubscription_Init(zoneId.Pointer, subscriptionId);

            if (pointer != IntPtr.Zero)
                return new CKRecordZoneSubscription(pointer);

            return null;
        }
        #endregion
        
        #region RecordType
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKRecordZoneSubscription_GetRecordType(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecordZoneSubscription_SetRecordType(IntPtr pointer, string recordType);

        /// <summary>
        /// The type of record that the subscription queries.
        /// </summary>
        public string RecordType
        {
            get => CKRecordZoneSubscription_GetRecordType(Pointer);
            set => CKRecordZoneSubscription_SetRecordType(Pointer, value);
        }
        #endregion
        
        #region ZoneId
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecordZoneSubscription_GetZoneID(IntPtr pointer);

        /// <summary>
        /// The ID of the record zone that the subscription queries.
        /// </summary>
        public CKRecordZone.ID ZoneId
        {
            get
            {
                var pointer = CKRecordZoneSubscription_GetZoneID(Pointer);

                if (pointer != IntPtr.Zero)
                    return new CKRecordZone.ID(pointer);

                return null;
            }
        }
        #endregion
    }
}