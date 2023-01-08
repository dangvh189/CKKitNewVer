using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKModifyRecordZonesOperation : CKDatabaseOperation<CKModifyRecordZonesResult>
    {
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKModifyRecordZonesOperation_Init(InteropStructArray zonesToSave, InteropStructArray zonesToDelete);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifyRecordZonesOperation_Free(IntPtr pointer);
        
        public CKModifyRecordZonesOperation(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer == IntPtr.Zero)
                CKModifyRecordZonesOperation_Free(Pointer);
        }

        public static CKModifyRecordZonesOperation Init(IEnumerable<CKRecordZone.ID> recordZoneIdsToDelete)
        {
            return Init(null, recordZoneIdsToDelete);
        }

        public static CKModifyRecordZonesOperation Init(IEnumerable<CKRecordZone> recordZonesToSave, IEnumerable<CKRecordZone.ID> recordZoneIdsToDelete = null)
        {
            // Ensure never null...
            recordZonesToSave ??= new CKRecordZone[0];
            recordZoneIdsToDelete ??= new CKRecordZone.ID[0];
            
            var savesPointers = recordZonesToSave.Select(z => z.Pointer).ToArray();
            var deletesPointers = recordZoneIdsToDelete.Select(z => z.Pointer).ToArray();
            
            var saves = InteropStructArray.From(savesPointers, out var savesHandle);
            var deletes = InteropStructArray.From(deletesPointers, out var deletesHandle);
            
            var pointer = CKModifyRecordZonesOperation_Init(saves, deletes);
            return new CKModifyRecordZonesOperation(pointer);
        }
        #endregion
        
        #region Set Completion Callback
        private delegate void CompletionCallback(long taskId, InteropStructArray savedRecordZones, InteropStructArray deletedRecordZoneIds);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifyRecordZonesOperation_SetCompletionCallback(IntPtr pointer, long taskId, CompletionCallback onComplete, NSErrorTaskCallback onError);

        internal override Task<CKModifyRecordZonesResult> OnSetupCompletionCallback(CKDatabase database)
        {
            var tcs = InteropTasks.Create<CKModifyRecordZonesResult>(out var taskId);

            CKModifyRecordZonesOperation_SetCompletionCallback(Pointer, taskId, OnComplete, OnCompletionError);
            
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(CompletionCallback))]
        private static void OnComplete(long taskId, InteropStructArray savedRecordZones, InteropStructArray deletedRecordZoneIds)
        {
            var result = new CKModifyRecordZonesResult
            {
                SavedRecordZones = savedRecordZones.ToArray<IntPtr>().Select(p => new CKRecordZone(p)).ToArray(),
                DeletedRecordZones = deletedRecordZoneIds.ToArray<IntPtr>().Select(p => new CKRecordZone.ID(p)).ToArray()
            };

            InteropTasks.TrySetResultAndRemove(taskId, result);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnCompletionError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKModifyRecordZonesResult>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
    }
}