using System;
using System.Runtime.InteropServices;

namespace Apple.CloudKit.Deprecated
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    [Obsolete("CKRecordZoneID has been Deprecated. Please remove all usages and convert to CKRecordZone.ID implementation.")]
    public struct CKRecordZoneID
    {
        public static CKRecordZoneID Default = new CKRecordZoneID("default", "default");

        public string ZoneName;
        public string OwnerName;

        public CKRecordZoneID(string zoneName, string ownerName)
        {
            ZoneName = zoneName;
            OwnerName = ownerName;
        }
    }
}