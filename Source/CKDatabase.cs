using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKDatabase : InteropReference
    {
#region Interop Delegates
        internal delegate void CKDatabaseSavedRecordCallback(long taskId, IntPtr savedRecord);
        internal delegate void CKDatabaseDeletedRecordCallback(long taskId, IntPtr deletedRecordID);
        internal delegate void CKDatabaseFetchRecordCallback(long taskId, IntPtr fetchedRecord);
#endregion
        
#region Interop Methods
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKDatabase_Free(IntPtr pointer);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKDatabase_SaveRecord(IntPtr database, IntPtr record, long taskId, CKDatabaseSavedRecordCallback onSaved, NSErrorTaskCallback onError);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKDatabase_DeleteRecord(IntPtr database, IntPtr record, long taskId, CKDatabaseDeletedRecordCallback onDeleted, NSErrorTaskCallback onError);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKDatabase_FetchRecord(IntPtr database, IntPtr recordId, long taskId, CKDatabaseFetchRecordCallback onFetch, NSErrorTaskCallback onError);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKDatabase_AddOperation(IntPtr database, IntPtr operation);
        #endregion
        
        internal CKDatabase(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKDatabase_Free(Pointer);
        }
        
        #region Add Operation

        public Task<TResult> Add<TResult>(CKDatabaseOperation<TResult> databaseOperation)
        {
            var task = databaseOperation.OnSetupCompletionCallback(this);
            CKDatabase_AddOperation(Pointer, databaseOperation.Pointer);

            return task;
        }
        #endregion

        #region Save
        public Task<CKRecord> Save(CKRecord record)
        {
            var tcs = InteropTasks.Create<CKRecord>(out var taskId);
            CKDatabase_SaveRecord(Pointer, record.Pointer, taskId, OnSaveRecord, OnSaveRecordError);
            return tcs.Task;
        }
        
        [MonoPInvokeCallback(typeof(CKDatabaseSavedRecordCallback))]
        private static void OnSaveRecord(long taskId, IntPtr savedRecord)
        {
            var record = savedRecord != IntPtr.Zero ? new CKRecord(savedRecord) : null;
            InteropTasks.TrySetResultAndRemove(taskId, record);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnSaveRecordError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKRecord>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
        
        #region Delete
        public Task<CKRecord.ID> Delete(CKRecord record)
        {
            var tcs = InteropTasks.Create<CKRecord.ID>(out var taskId);
            CKDatabase_DeleteRecord(Pointer, record.Pointer, taskId, OnDeletedRecord, OnDeletedRecordError);

            return tcs.Task;
        }
        
        [MonoPInvokeCallback(typeof(CKDatabaseDeletedRecordCallback))]
        private static void OnDeletedRecord(long taskId, IntPtr deletedRecordId)
        {
            var recordId = deletedRecordId != IntPtr.Zero ? new CKRecord.ID(deletedRecordId) : null;
            InteropTasks.TrySetResultAndRemove(taskId, recordId);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnDeletedRecordError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKRecord.ID>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
        
        #region Fetch
        public Task<CKRecord> Fetch(CKRecord.ID recordId)
        {
            var tcs = InteropTasks.Create<CKRecord>(out var taskId);
            CKDatabase_FetchRecord(Pointer, recordId.Pointer, taskId, OnFetchRecord, OnFetchRecordError);
            return tcs.Task;
        }
        
        [MonoPInvokeCallback(typeof(CKDatabaseFetchRecordCallback))]
        private static void OnFetchRecord(long taskId, IntPtr fetchedRecord)
        {
            var record = fetchedRecord != IntPtr.Zero ? new CKRecord(fetchedRecord) : null;
            InteropTasks.TrySetResultAndRemove(taskId, record);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnFetchRecordError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKRecord>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
    }
}
