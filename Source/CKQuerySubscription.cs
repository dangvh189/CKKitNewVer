using System;
using System.Runtime.InteropServices;

namespace Apple.CloudKit
{
    /// <summary>
    /// A subscription that generates push notifications when CloudKit modifies records that match a predicate.
    /// </summary>
    public class CKQuerySubscription : CKSubscription
    {
        [Flags]
        public enum Options : ulong
        {
            /// <summary>
            /// An object that describes the configuration of a subscription’s push notifications.
            /// </summary>
            FiresOnRecordCreation = 1 << 0,
            /// <summary>
            /// An option that instructs CloudKit to send a push notification when it modifies a record that matches a subscription’s criteria.
            /// </summary>
            FiresOnRecordUpdate = 1 << 1,
            /// <summary>
            /// An option that instructs CloudKit to send a push notification when it deletes a record that matches a subscription’s criteria. 
            /// </summary>
            FiresOnRecordDeletion = 1 << 2,
            /// <summary>
            /// An option that instructs CloudKit to send a push notification only once. 
            /// </summary>
            FiresOnce = 1 << 3
        }

        public CKQuerySubscription(IntPtr pointer) : base(pointer) {}
        
        #region Init
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQuerySubscription_Init(string recordType, IntPtr predicate, string subscriptionId, Options options);

        public static CKQuerySubscription Init(string recordType, NSPredicate predicate, string subscriptionId, Options options)
        {
            var pointer = CKQuerySubscription_Init(recordType, predicate.Pointer, subscriptionId, options);
            
            if(pointer != IntPtr.Zero)
                return new CKQuerySubscription(pointer);

            return null;
        }

        #endregion
        
        #region ZoneId  
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQuerySubscription_GetZoneID(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQuerySubscription_SetZoneID(IntPtr pointer, IntPtr zone);

        /// <summary>
        /// The ID of the record zone that the subscription queries.
        /// </summary>
        public CKRecordZone.ID ZoneId
        {
            get
            {
                var pointer = CKQuerySubscription_GetZoneID(Pointer);

                if (pointer != IntPtr.Zero)
                    return new CKRecordZone.ID(pointer);

                return null;
            }
            set => CKQuerySubscription_SetZoneID(Pointer, value?.Pointer ?? IntPtr.Zero);
        }
        #endregion
        
        #region RecordType
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKQuerySubscription_GetRecordType(IntPtr pointer);

        public string RecordType
        {
            get => CKQuerySubscription_GetRecordType(Pointer);
        }
        #endregion
        
        #region QuerySubscriptionOptions
        [DllImport(InteropUtility.DLLName)]
        private static extern Options CKQuerySubscription_GetQuerySubscriptionOptions(IntPtr pointer);

        public Options QuerySubscriptionOptions
        {
            get => CKQuerySubscription_GetQuerySubscriptionOptions(Pointer);
        }
        #endregion
    }
}