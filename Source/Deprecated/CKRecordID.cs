using System;
using System.Runtime.InteropServices;

namespace Apple.CloudKit.Deprecated
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    [Obsolete("CKRecordID has been Deprecated. Please remove all usages and convert to CKRecord.ID implementation.")]
    public struct CKRecordID
    {
        public string RecordName;
        public CKRecordZoneID ZoneID;
    }
}