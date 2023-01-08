using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKQueryOperation : CKDatabaseOperation<CKQueryCursor?>
    {
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQueryOperation_InitWithQuery(IntPtr queryPtr);
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQueryOperation_InitWithCursor(IntPtr cursorPtr);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQueryOperation_Free(IntPtr pointer);
        
        internal CKQueryOperation(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKQueryOperation_Free(Pointer);
        }

        public static CKQueryOperation Init(CKQuery query)
        {
            var pointer = CKQueryOperation_InitWithQuery(query.Pointer);
            return new CKQueryOperation(pointer);
        }

        public static CKQueryOperation Init(CKQueryCursor cursor)
        {
            var pointer = CKQueryOperation_InitWithCursor(cursor.Pointer);
            return new CKQueryOperation(pointer);
        }
        #endregion
        
        #region Set Record Fetched Block
        public List<CKRecord> Results { get; private set; }
        
        private static readonly Dictionary<long, CKQueryOperation> _operationReferences = new Dictionary<long, CKQueryOperation>();
        private delegate void RecordFetchedBlockCallback(long taskId, IntPtr recordPtr);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQueryOperation_SetRecordFetchedBlock(IntPtr pointer, long taskId, RecordFetchedBlockCallback onFetched);

        internal void SetRecordFetchedBlock(long taskId)
        {
            Results = new List<CKRecord>();
            
            if(!_operationReferences.ContainsKey(taskId))
                _operationReferences.Add(taskId, this);
            
            CKQueryOperation_SetRecordFetchedBlock(Pointer, taskId, OnRecordFetched);
        }

        [MonoPInvokeCallback(typeof(RecordFetchedBlockCallback))]
        private static void OnRecordFetched(long taskId, IntPtr recordPtr)
        {
            if (_operationReferences.ContainsKey(taskId))
            {
                var operation = _operationReferences[taskId];
                
                // We buffer the results, and when complete pass them to the cursor...
                if (recordPtr != IntPtr.Zero)
                {
                    operation.Results.Add(new CKRecord(recordPtr));
                }
            }
        }
        #endregion

        #region SetCompletionCallback
        private delegate void CompletionCallback(long taskId, IntPtr cursorPointer);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQueryOperation_SetCompletionCallback(IntPtr pointer, long taskId, CompletionCallback onComplete, NSErrorTaskCallback onError);
        
        internal override Task<CKQueryCursor> OnSetupCompletionCallback(CKDatabase database)
        {
            var tcs = InteropTasks.Create<CKQueryCursor>(out var taskId);
            SetRecordFetchedBlock(taskId);
            CKQueryOperation_SetCompletionCallback(Pointer, taskId, OnComplete, OnCompletionError);
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(CompletionCallback))]
        private static void OnComplete(long taskId, IntPtr cursorPtr)
        {
            _operationReferences.Remove(taskId);
            
            var cursor = cursorPtr != IntPtr.Zero ? new CKQueryCursor(cursorPtr) : CKQueryCursor.EmptyCursor;
            InteropTasks.TrySetResultAndRemove(taskId, cursor);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnCompletionError(long taskId, IntPtr errorPointer)
        {
            _operationReferences.Remove(taskId);
            InteropTasks.TrySetExceptionAndRemove<CKQueryCursor>(taskId, new CloudKitException(errorPointer));
        }

        #endregion
        
        #region Results Limit
        [DllImport(InteropUtility.DLLName)]
        private static extern int CKQueryOperation_GetResultsLimit(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQueryOperation_SetResultsLimit(IntPtr pointer, int value);

        public int ResultsLimit
        {
            get => CKQueryOperation_GetResultsLimit(Pointer);
            set => CKQueryOperation_SetResultsLimit(Pointer, value);
        }
        #endregion
        
        #region ZoneID
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQueryOperation_GetZoneID(IntPtr pointer);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQueryOperation_SetZoneID(IntPtr pointer, IntPtr zoneID);

        public CKRecordZone.ID ZoneId
        {
            get
            {
                var pointer = CKQueryOperation_GetZoneID(Pointer);
                return pointer != IntPtr.Zero ? new CKRecordZone.ID(pointer) : null;
            }
            set => CKQueryOperation_SetZoneID(Pointer, value.Pointer);
        }
        #endregion
        
        #region Desired Keys
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKQueryOperation_GetDesiredKeys(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQueryOperation_SetDesiredKeys(IntPtr pointer, string keys);

        public string[] DesiredKeys
        {
            get
            {
                var keys = CKQueryOperation_GetDesiredKeys(Pointer);
                return keys.Split(';');
            }
            set
            {
                var keys = string.Join(";", value);
                CKQueryOperation_SetDesiredKeys(Pointer, keys);
            }
        }
        #endregion
    }
}