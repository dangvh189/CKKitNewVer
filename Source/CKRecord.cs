using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKRecord : InteropReference
    {
        public enum ReferenceAction : int
        {
            None = 0,
            DeleteSelf = 1
        }

        public class CKReference : InteropReference
        {
            #region Init & Dispose
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKReference_InitWithRecord(IntPtr record, ReferenceAction action);
            
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKReference_InitWithRecordID(IntPtr record, ReferenceAction action);

            [DllImport(InteropUtility.DLLName)]
            private static extern void CKReference_Free(IntPtr pointer);
            
            internal CKReference(IntPtr pointer) : base(pointer) {}

            protected override void OnDispose(bool isDisposing)
            {
                if(Pointer == IntPtr.Zero)
                    CKReference_Free(Pointer);
            }

            public static CKReference Init(CKRecord record, ReferenceAction action = ReferenceAction.None)
            {
                var pointer = CKReference_InitWithRecord(record.Pointer, action);
                return new CKReference(pointer);
            }
            
            public static CKReference Init(CKRecord.ID recordId, ReferenceAction action = ReferenceAction.None)
            {
                var pointer = CKReference_InitWithRecordID(recordId.Pointer, action);
                return new CKReference(pointer);
            }
            #endregion
            
            #region Get RecordID
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKReference_GetRecordID(IntPtr pointer);

            public CKRecord.ID RecordID
            {
                get
                {
                    var pointer = CKReference_GetRecordID(Pointer);
                    return pointer != IntPtr.Zero ? new CKRecord.ID(pointer) : null;
                }
            }
            
            #endregion
            
            #region Get Action
            [DllImport(InteropUtility.DLLName)]
            private static extern ReferenceAction CKReference_GetAction(IntPtr pointer);

            public ReferenceAction Action
            {
                get => CKReference_GetAction(Pointer);
            }
            #endregion
        }
        
        public class ID : InteropReference
        {
            #region Interop Methods
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKRecordID_InitWithRecordName(string recordName);
            
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKRecordID_InitWithRecordNameAndZone(string recordName, IntPtr zoneId);

            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKRecordID_Free(IntPtr recordId);

            [DllImport(InteropUtility.DLLName)]
            private static extern string CKRecordID_GetRecordName(IntPtr recordId);

            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKRecordID_GetZoneID(IntPtr recordId);
            #endregion
            
            #region Init & Dispose
            internal ID(IntPtr pointer) : base(pointer) {}

            protected override void OnDispose(bool isDisposing)
            {
                if (Pointer != IntPtr.Zero)
                    CKRecordID_Free(Pointer);
            }

            public static ID Init(string recordName)
            {
                var pointer = CKRecordID_InitWithRecordName(recordName);
                return new ID(pointer);
            }
            
            public static ID Init(string recordName, CKRecordZone.ID zoneID)
            {
                var pointer = CKRecordID_InitWithRecordNameAndZone(recordName, zoneID.Pointer);
                return new ID(pointer);
            }
            
            public static ID Init(string recordName, CKRecordZone zone)
            {
                return Init(recordName, zone.ZoneID);
            }
            #endregion

            public string RecordName
            {
                get => CKRecordID_GetRecordName(Pointer);
            }

            public CKRecordZone.ID ZoneID
            {
                get
                {
                    var pointer = CKRecordID_GetZoneID(Pointer);
                    return new CKRecordZone.ID(pointer);
                }
            }
        }
        
        #region Interop Methods
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_InitWithRecordType(string recordType);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_InitWithRecordTypeAndID(string recordType, IntPtr recordId);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_Free(IntPtr record);
        #endregion
        
        #region Init & Dispose
        internal CKRecord(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKRecord_Free(Pointer);
        }

        public static CKRecord Init(string recordType)
        {
            var pointer = CKRecord_InitWithRecordType(recordType);
            return new CKRecord(pointer);
        }
        
        public static CKRecord Init(string recordType, CKRecord.ID recordID)
        {
            var pointer = CKRecord_InitWithRecordTypeAndID(recordType, recordID.Pointer);
            return new CKRecord(pointer);
        }
        #endregion
        
        #region Set Fields
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetInt32(IntPtr record, string key, int value);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetInt64(IntPtr record, string key, long value);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetFloat(IntPtr record, string key, float value);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetBool(IntPtr record, string key, bool value);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetString(IntPtr record, string key, string value);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetBinaryData(IntPtr record, string key, InteropStructArray data);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetCKAsset(IntPtr record, string key, IntPtr asset);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetCKReference(IntPtr record, string key, IntPtr reference);

        public void Set(string key, int value)
        {
            CKRecord_SetInt32(Pointer, key, value);
        }
        
        public void Set(string key, long value)
        {
            CKRecord_SetInt64(Pointer, key, value);
        }
        
        public void Set(string key, float value)
        {
            CKRecord_SetFloat(Pointer, key, value);
        }
        
        public void Set(string key, bool value)
        {
            CKRecord_SetBool(Pointer, key, value);
        }
        
        public void Set(string key, string value)
        {
            CKRecord_SetString(Pointer, key, value);
        }
        
        public void Set(string key, byte[] value)
        {
            var data = InteropStructArray.From(value, out var handle);
            CKRecord_SetBinaryData(Pointer, key, data);
            handle.Free();
        }

        public void Set(string key, CKAsset asset)
        {
            CKRecord_SetCKAsset(Pointer, key, asset.Pointer);
        }

        public void Set(string key, CKReference reference)
        {
            CKRecord_SetCKReference(Pointer, key, reference.Pointer);
        }
        #endregion
        
        #region Get Fields
        [DllImport(InteropUtility.DLLName)]
        private static extern bool CKRecord_HasKey(IntPtr record, string key);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern int CKRecord_GetInt32(IntPtr record, string key);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern long CKRecord_GetInt64(IntPtr record, string key);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern float CKRecord_GetFloat(IntPtr record, string key);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern bool CKRecord_GetBool(IntPtr record, string key);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKRecord_GetString(IntPtr record, string key);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern InteropStructArray CKRecord_GetBinaryData(IntPtr record, string key);

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_GetCKAsset(IntPtr record, string key);

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_GetCKReference(IntPtr record, string key);

        public bool HasKey(string key)
        {
            return CKRecord_HasKey(Pointer, key);
        }
        
        public int GetInt32(string key)
        {
            return CKRecord_GetInt32(Pointer, key);
        }
        
        public long GetInt64(string key)
        {
            return CKRecord_GetInt64(Pointer, key);
        }
        
        public float GetFloat(string key)
        {
            return CKRecord_GetFloat(Pointer, key);
        }
        
        public bool GetBool(string key)
        {
            return CKRecord_GetBool(Pointer, key);
        }
        
        public string GetString(string key)
        {
            return CKRecord_GetString(Pointer, key);
        }
        
        public byte[] GetBinaryData(string key)
        {
            var wrappper = CKRecord.CKRecord_GetBinaryData(Pointer, key);
            return wrappper.ToArray<byte>();
        }

        public CKAsset GetCKAsset(string key)
        {
            var pointer = CKRecord_GetCKAsset(Pointer, key);
            return new CKAsset(pointer);
        }
        
        public CKReference GetCKReference(string key)
        {
            var pointer = CKRecord_GetCKReference(Pointer, key);
            return new CKReference(pointer);
        }
        #endregion
        
        #region Get RecordID
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_GetRecordID(IntPtr record);

        public CKRecord.ID RecordId
        {
            get
            {
                var pointer = CKRecord_GetRecordID(Pointer);
                return new CKRecord.ID(pointer);
            }
        }
        #endregion
        
        #region Get RecordType
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKRecord_GetRecordType(IntPtr pointer);

        public string RecordType
        {
            get => CKRecord_GetRecordType(Pointer);
        }
        #endregion
        
        #region Get CreationDate
        [DllImport(InteropUtility.DLLName)]
        private static extern double CKRecord_GetCreationDate(IntPtr pointer);
        
        public DateTimeOffset CreationDate
        {
            get => DateTimeOffset.FromUnixTimeSeconds((long)CKRecord_GetCreationDate(Pointer));
        } 
        #endregion
        
        #region Get LastModificationDate
        [DllImport(InteropUtility.DLLName)]
        private static extern double CKRecord_GetLastModificationDate(IntPtr pointer);
        
        public DateTimeOffset LastModificationDate
        {
            get => DateTimeOffset.FromUnixTimeSeconds((long)CKRecord_GetLastModificationDate(Pointer));
        } 
        #endregion
        
        #region Get CreatorRecordUserID
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_GetCreatorUserRecordID(IntPtr pointer);

        public CKRecord.ID CreatorUserRecordID
        {
            get
            {
                var pointer = CKRecord_GetCreatorUserRecordID(Pointer);

                if (pointer == IntPtr.Zero)
                    return null;

                return new CKRecord.ID(pointer);
            }
        }
        #endregion
        
        #region Get Parent
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_GetParent(IntPtr pointer);

        public CKReference Parent
        {
            get
            {
                var pointer = CKRecord_GetParent(Pointer);

                if (pointer == IntPtr.Zero)
                    return null;
                
                return new CKReference(pointer);
            }
        }
        #endregion
        
        #region Get RecordChangeTag
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKRecord_GetRecordChangeTag(IntPtr pointer);

        public string RecordChangeTag
        {
            get => CKRecord_GetRecordChangeTag(Pointer);
        }
        #endregion
        
        #region Get LastModifiedUserRecordID
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKRecord_GetLastModifiedUserRecordID(IntPtr pointer);

        public CKRecord.ID LastModifiedUserRecordID
        {
            get
            {
                var pointer = CKRecord_GetLastModifiedUserRecordID(Pointer);

                if (pointer == IntPtr.Zero)
                    return null;

                return new CKRecord.ID(pointer);
            }
        }
        #endregion
        
        #region Set Parent
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetParentWithRecord(IntPtr pointer, IntPtr parent);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKRecord_SetParentWithRecordID(IntPtr pointer, IntPtr parentID);

        public void SetParent(CKRecord parent)
        {
            CKRecord_SetParentWithRecord(Pointer, parent.Pointer);
        }
        
        public void SetParent(CKRecord.ID parentId)
        {
            CKRecord_SetParentWithRecordID(Pointer, parentId.Pointer);
        }
        #endregion
    }
}