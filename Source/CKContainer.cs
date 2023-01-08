using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKContainer : InteropReference
    {
        #region Interop Methods
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKContainer_Default();

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKContainer_Init(string identifier);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKContainer_Free(IntPtr container);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKContainer_GetPublicDatabase(IntPtr container);

        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKContainer_GetPrivateDatabase(IntPtr container);
        #endregion
        
        #region Init & Dispose
        internal CKContainer(IntPtr pointer) : base(pointer) { }

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero) 
                CKContainer_Free(Pointer);
        }

        public static CKContainer Default()
        {
            var pointer = CKContainer_Default();
            return new CKContainer(pointer);
        }

        public static CKContainer Init(string identifier)
        {
            var pointer = CKContainer_Init(identifier);
            return new CKContainer(pointer);
        }
        #endregion

        public CKDatabase PrivateDatabase
        {
            get
            {
                var pointer = CKContainer_GetPrivateDatabase(Pointer);
                return new CKDatabase(pointer);
            }
        }

        public CKDatabase PublicDatabase
        {
            get
            {
                var pointer = CKContainer_GetPublicDatabase(Pointer);
                return new CKDatabase(pointer);
            }
        }

        #region Get Account Status

        private delegate void CKContainer_GetAccountStatusCallback(long taskId, CKAccountStatus accountStatus);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKContainer_GetAccountStatus(IntPtr pointer, long taskId, CKContainer_GetAccountStatusCallback onStatus, NSErrorTaskCallback onError);
        
        public Task<CKAccountStatus> GetAccountStatus()
        {
            var tcs = InteropTasks.Create<CKAccountStatus>(out var taskId);
            //InteropInvocations.Register(new InteropInvocation<CKAccountStatus, CloudKitException>(callback), out var taskId);
            CKContainer_GetAccountStatus(Pointer, taskId, OnGetAccountStatus, OnGetAccountStatusError);
            return tcs.Task;
        }

        [MonoPInvokeCallback(typeof(CKContainer_GetAccountStatusCallback))]
        private static void OnGetAccountStatus(long taskId, CKAccountStatus accountStatus)
        {
            //var invocation = InteropInvocations.GetAndRemove<InteropInvocation<CKAccountStatus, CloudKitException>>(taskId);
            InteropTasks.TrySetResultAndRemove(taskId, accountStatus);
        }

        [MonoPInvokeCallback(typeof(NSErrorTaskCallback))]
        private static void OnGetAccountStatusError(long taskId, IntPtr errorPointer)
        {
            InteropTasks.TrySetExceptionAndRemove<CKAccountStatus>(taskId, new CloudKitException(errorPointer));
        }
        #endregion
        
        #region Get CurrentUserDefaultName
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKContainer_GetCKCurrentUserDefaultName();

        public static string CurrentUserDefaultName
        {
            get => CKContainer_GetCKCurrentUserDefaultName();
        }
        
        #endregion
    }
}