using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    /// <summary>
    /// An operation for modifying one or more subscriptions.
    /// </summary>
    public class CKModifySubscriptionsOperation : CKDatabaseOperation<CKModifySubscriptionsResult>
    {
        #region Init & Dispose
        public CKModifySubscriptionsOperation(IntPtr pointer) : base(pointer)
        {
        }
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifySubscriptionsOperation_Free(IntPtr pointer);

        protected override void OnDispose(bool isDisposing)
        {
            if (Pointer != IntPtr.Zero)
            {
                CKModifySubscriptionsOperation_Free(Pointer);
                Pointer = IntPtr.Zero;
            }
        }

        #endregion
        
        #region Static Init
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKModifySubscriptionsOperation_Init(IntPtr toSaveArray, IntPtr toDeleteArray);

        public static CKModifySubscriptionsOperation Init(IEnumerable<string> subscriptionIdsToDelete)
        {
            return Init(null, subscriptionIdsToDelete);
        }
        
        public static CKModifySubscriptionsOperation Init(IEnumerable<CKSubscription> subscriptionsToSave, IEnumerable<string> subscriptionIdsToDelete = null)
        {
            subscriptionsToSave ??= new CKSubscription[0];
            subscriptionIdsToDelete ??= new string[0];

            // Build NSArray representations...
            var mutableSubscriptionsToSave = NSMutableArrayFactory.Init<NSMutableArrayCKSubscription, CKSubscription>();
            var mutableSubscriptionIdsToDelete = NSMutableArrayFactory.Init<NSMutableArrayString, string>();

            foreach (var subscription in subscriptionsToSave)
            {
                mutableSubscriptionsToSave.Add(subscription);
            }

            foreach (var id in subscriptionIdsToDelete)
            {
                mutableSubscriptionIdsToDelete.Add(id);
            }

            var pointer = CKModifySubscriptionsOperation_Init(mutableSubscriptionsToSave.Pointer, mutableSubscriptionIdsToDelete.Pointer);

            if (pointer != IntPtr.Zero)
            {
                return new CKModifySubscriptionsOperation(pointer);
            }

            return null;
        }
        #endregion
        
        #region Set Completion Callback
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKModifySubscriptionsOperation_SetCompletionCallback(IntPtr pointer, long taskId, SuccessTaskCallback onSuccess, NSErrorTaskCallback onError);
        
        internal override Task<CKModifySubscriptionsResult> OnSetupCompletionCallback(CKDatabase database)
        {
            var tcs = InteropTasks.Create<CKModifySubscriptionsResult>(out var taskId);
            CKModifySubscriptionsOperation_SetCompletionCallback(Pointer, taskId, OnComplete, OnError);
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(SuccessTaskCallback))]
        private static void OnComplete(long taskId)
        {
            InteropTasks.TrySetResultAndRemove(taskId, new CKModifySubscriptionsResult());
        }
        
        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKModifySubscriptionsResult>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
    }
}