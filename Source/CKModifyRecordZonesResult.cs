namespace Apple.CloudKit
{
    public class CKModifyRecordZonesResult
    {
        public CKRecordZone[] SavedRecordZones { get; internal set; }
        public CKRecordZone.ID[] DeletedRecordZones { get; internal set; }
    }
}