using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKRecordZone : InteropReference
    {
        public enum Capabilities : int
        {
            FetchChanges = 1,
            Atomic = 2,
            Sharing = 4
        }
        
        public class ID : InteropReference
        {
#region Interop Methods
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKRecordZoneID_Init(string zoneName, string ownerName);

            [DllImport(InteropUtility.DLLName)]
            private static extern void CKRecordZoneID_Free(IntPtr recordZoneID);

            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKRecordZoneID_Default();

            [DllImport(InteropUtility.DLLName)]
            private static extern string CKRecordZoneID_GetZoneName(IntPtr pointer);
            
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKRecordZoneID_GetOwnerName(IntPtr pointer);
#endregion

            public string ZoneName
            {
                get => CKRecordZoneID_GetZoneName(Pointer);
            }

            public string OwnerName
            {
                get => CKRecordZoneID_GetOwnerName(Pointer);
            }

            internal ID(IntPtr pointer) : base(pointer) {}

            protected override void OnDispose(bool isDisposing)
            {
                if (Pointer != IntPtr.Zero)
                    CKRecordZoneID_Free(Pointer);
            }

            public static ID Init(string zoneName, string ownerName)
            {
                var pointer = CKRecordZoneID_Init(zoneName, ownerName);
                return new ID(pointer);
            }

            public static ID Default()
            {
                var pointer = CKRecordZoneID_Default();
                return new ID(pointer);
            }
        }
        
        #region Interop Methods
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecordZone_InitWithZoneName(string zoneName);

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecordZone_InitWithZoneID(IntPtr zoneID);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecordZone_Free(IntPtr recordZone);

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecordZone_Default();

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecordZone_GetZoneID(IntPtr zone);
        #endregion
        
        #region Init & Dispose
        internal CKRecordZone(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKRecordZone_Free(Pointer);
        }

        public static CKRecordZone Init(string zoneName)
        {
            var pointer = CKRecordZone_InitWithZoneName(zoneName);
            return new CKRecordZone(pointer);
        }
        
        public static CKRecordZone Init(CKRecordZone.ID zoneID)
        {
            var pointer = CKRecordZone_InitWithZoneID(zoneID.Pointer);
            return new CKRecordZone(pointer);
        }

        public static CKRecordZone Default()
        {
            var pointer = CKRecordZone_Default();
            return new CKRecordZone(pointer);
        }
        #endregion
        
        #region Get ZoneID
        public CKRecordZone.ID ZoneID
        {
            get
            {
                var pointer = CKRecordZone_GetZoneID(Pointer);
                return new ID(pointer);
            }
        }
        #endregion

        #region Get Capabilities
        [DllImport(InteropUtility.DLLName)]
        private static extern Capabilities CKRecordZone_GetCapabilities(IntPtr pointer);
            
        public Capabilities ZoneCapabilities
        {
            get => CKRecordZone_GetCapabilities(Pointer);
        }
        #endregion
    }
}