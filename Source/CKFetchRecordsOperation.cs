using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKFetchRecordsOperation : CKDatabaseOperation<CKRecord[]>
    {
        #region Interop Methods
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKFetchRecordsOperation_Init();
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKFetchRecordsOperation_InitWithRecordIDs(InteropStructArray recordIds);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKFetchRecordsOperation_Free(IntPtr pointer);
        #endregion

        #region Init & Dispose
        internal CKFetchRecordsOperation(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if (Pointer != IntPtr.Zero)
                CKFetchRecordsOperation_Free(Pointer);
        }
        
        public static CKFetchRecordsOperation Init()
        {
            var pointer = CKFetchRecordsOperation_Init();
            return new CKFetchRecordsOperation(pointer);
        }
        
        public static CKFetchRecordsOperation Init(IEnumerable<CKRecord.ID> recordIds)
        {
            var pointers = recordIds.Select((r) => r.Pointer).ToArray();
            var recordIdPointers = InteropStructArray.From(pointers, out var handle);
            var pointer = CKFetchRecordsOperation_InitWithRecordIDs(recordIdPointers);
            handle.Free();
            return new CKFetchRecordsOperation(pointer);
        }
        #endregion
        
        #region RecordIds
        [DllImport(InteropUtility.DLLName)]
        private static extern InteropStructArray CKFetchRecordsOperation_GetRecordIDs(IntPtr pointer);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern InteropStructArray CKFetchRecordsOperation_SetRecordIDs(IntPtr pointer, InteropStructArray recordIds);

        public CKRecord.ID[] RecordIds
        {
            get
            {
                var data = CKFetchRecordsOperation_GetRecordIDs(Pointer);
                var pointers = data.ToArray<IntPtr>();
                
                return pointers.Select(pointer => new CKRecord.ID(pointer)).ToArray();
            }
            set
            {
                var pointers = value.Select(recordId => recordId.Pointer).ToArray();
                var data = InteropStructArray.From(pointers, out var handle);
                CKFetchRecordsOperation_SetRecordIDs(Pointer, data);
                handle.Free();
            }
        }
        #endregion
        
        #region Setup Completion Callback

        private delegate void CompletionCallback(long taskId, InteropStructArray data);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKFetchRecordsOperation_SetCompletionCallback(IntPtr pointer, long taskId, CompletionCallback onComplete, NSErrorTaskCallback onError);

        internal override Task<CKRecord[]> OnSetupCompletionCallback(CKDatabase database)
        {
            var tcs = InteropTasks.Create<CKRecord[]>(out var taskId);
            CKFetchRecordsOperation_SetCompletionCallback(Pointer, taskId, OnComplete, OnCompletionError);
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(CompletionCallback))]
        private static void OnComplete(long taskId, InteropStructArray data)
        {
            var pointers = data.ToArray<IntPtr>();
            var records = pointers.Select(pointer => new CKRecord(pointer)).ToArray();

            InteropTasks.TrySetResultAndRemove(taskId, records);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnCompletionError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKRecord[]>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
    }
} 