using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKFetchRecordZonesOperation : CKDatabaseOperation<CKRecordZone[]>
    {
        #region Interop Delegates
        private delegate void CompletionCallback(long taskId, IntPtr[] recordZones, int recordZoneCount);
        #endregion
        
        #region Interop Methods
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKFetchRecordZonesOperation_Init();

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKFetchRecordZonesOperation_Free(IntPtr pointer);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKFetchRecordZonesOperation_FetchAllRecordZonesOperation();

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKFetchRecordZonesOperation_SetCompletionCallback(IntPtr operationPtr, long taskId, CompletionCallback onCompletion, NSErrorTaskCallback onError);
        #endregion

        #region Init & Dispose
        internal CKFetchRecordZonesOperation(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKFetchRecordZonesOperation_Free(Pointer);
        }

        public static CKFetchRecordZonesOperation Init()
        {
            var pointer = CKFetchRecordZonesOperation_Init();
            return new CKFetchRecordZonesOperation(pointer);
        }

        public static CKFetchRecordZonesOperation FetchAllRecordZonesOperation()
        {
            var pointer = CKFetchRecordZonesOperation_FetchAllRecordZonesOperation();
            return new CKFetchRecordZonesOperation(pointer);
        }
        #endregion
        
        #region Setup Completion Callback

        internal override Task<CKRecordZone[]> OnSetupCompletionCallback(CKDatabase database)
        {
            var tcs = InteropTasks.Create<CKRecordZone[]>(out var taskId);
            CKFetchRecordZonesOperation_SetCompletionCallback(Pointer, taskId, OnCompletionCallback, OnCompletionError);
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(CompletionCallback))]
        private static void OnCompletionCallback(long taskId, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 2)]
            IntPtr[] recordZones, int recordZoneCount)
        {
            var results = new CKRecordZone[recordZoneCount];

            for (var i = 0; i < recordZoneCount; i++)
            {
                results[i] = new CKRecordZone(recordZones[i]);
            }

            InteropTasks.TrySetResultAndRemove(taskId, results);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnCompletionError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKRecordZone[]>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
    }
}