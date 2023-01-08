using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKModifyRecordsOperation : CKDatabaseOperation<CKModifyRecordsResult>
    {
        public enum RecordSavePolicy : int
        {
            /// <summary>
            /// A policy that instructs CloudKit to save only the fields of a record that contain changes.
            /// If the server record's change tag is more recent, CloudKit discards the save and returns CKError.Code.serverRecordChanged error. 
            /// </summary>
            IfServerRecordUnchanged = 0,
            /// <summary>
            /// A policy that instructs CloudKit to save only the fields of a record that contain changes.
            /// The server doesn't compare record change tags when using this policy. 
            /// </summary>
            ChangedKeys = 1,
            /// <summary>
            /// A policy that instructs CloudKit to save all keys of a record, even those without changes.
            /// The server doesn't compare record change tags when using this policy. 
            /// </summary>
            AllKeys = 2
        }
        
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKModifyRecordsOperation_Init(InteropStructArray recordsToSave, InteropStructArray recordIdsToDelete);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifyRecordsOperation_Free(IntPtr pointer);
        
        internal CKModifyRecordsOperation(IntPtr pointer) : base(pointer) {}

        public static CKModifyRecordsOperation Init(IEnumerable<CKRecord.ID> recordIdsToDelete)
        {
            return Init(null, recordIdsToDelete);
        }
        
        public static CKModifyRecordsOperation Init(IEnumerable<CKRecord> recordsToSave, IEnumerable<CKRecord.ID> recordIdsToDelete = null)
        {
            // Ensure never null...
            recordsToSave ??= new CKRecord[0];
            recordIdsToDelete ??= new CKRecord.ID[0];

            var recordPointersToSave = recordsToSave.Select(r => r.Pointer).ToArray();
            var recordIdPointersToDelete = recordIdsToDelete.Select(r => r.Pointer).ToArray();

            var recordPointersToSaveArray = InteropStructArray.From(recordPointersToSave, out var savesHandle);
            var recordIdPointersToDeleteArray = InteropStructArray.From(recordIdPointersToDelete, out var deletesHandle);

            var pointer = CKModifyRecordsOperation_Init(recordPointersToSaveArray, recordIdPointersToDeleteArray);
            
            savesHandle.Free();
            deletesHandle.Free();
            
            return new CKModifyRecordsOperation(pointer);
        }
        #endregion

        #region Set Completion Callback
        private delegate void CompletionCallback(long taskId, InteropStructArray savedRecords, InteropStructArray deletedRecordIds);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifyRecordsOperation_SetCompletionCallback(IntPtr pointer, long taskId, CompletionCallback onComplete, NSErrorTaskCallback onError);
        
        internal override Task<CKModifyRecordsResult> OnSetupCompletionCallback(CKDatabase database)
        {
            var tcs = InteropTasks.Create<CKModifyRecordsResult>(out var taskId);
            CKModifyRecordsOperation_SetCompletionCallback(Pointer, taskId, OnComplete, OnCompletionError);
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(CompletionCallback))]
        private static void OnComplete(long taskId, InteropStructArray savedRecords, InteropStructArray deletedRecordIds)
        {
            var result = new CKModifyRecordsResult
            {
                SavedRecords = savedRecords.ToArray<IntPtr>().Select(p => new CKRecord(p)).ToArray(),
                DeletedRecords = deletedRecordIds.ToArray<IntPtr>().Select(p => new CKRecord.ID(p)).ToArray()
            };

            InteropTasks.TrySetResultAndRemove(taskId, result);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnCompletionError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKModifyRecordsResult>(taskId, new CloudKitException(errorPointer));
        }

        #endregion

        #region IsAtomic
        [DllImport(InteropUtility.DLLName)]
        private static extern bool CKModifyRecordsOperation_GetIsAtomic(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifyRecordsOperation_SetIsAtomic(IntPtr pointer, bool value);

        /// <summary>
        /// A Boolean value that indicates whether the entire operation fails when CloudKit can't update one or more records in a record zone. 
        /// </summary>
        public bool IsAtomic
        {
            get => CKModifyRecordsOperation_GetIsAtomic(Pointer);
            set => CKModifyRecordsOperation_SetIsAtomic(Pointer, value);
        }
        #endregion
        
        #region SavePolicy
        [DllImport(InteropUtility.DLLName)]
        private static extern RecordSavePolicy CKModifyRecordsOperation_GetSavePolicy(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifyRecordsOperation_SetSavePolicy(IntPtr pointer, RecordSavePolicy savePolicy);

        /// <summary>
        /// The policy to use when saving changes to records. 
        /// </summary>
        public RecordSavePolicy SavePolicy
        {
            get => CKModifyRecordsOperation_GetSavePolicy(Pointer);
            set => CKModifyRecordsOperation_SetSavePolicy(Pointer, value);
        }
        #endregion
    }
}