using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKAsset : InteropReference
    {
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKAsset_Init(string fileUrl);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKAsset_Free(IntPtr pointer);
        
        internal CKAsset(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKAsset_Free(Pointer);
        }

        public static CKAsset Init(string fileUrl)
        {
            var pointer = CKAsset_Init(fileUrl);
            return new CKAsset(pointer);
        }
        #endregion

        #region FileUrl
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKAsset_GetFileUrl(IntPtr pointer);

        public string FileUrl
        {
            get => CKAsset_GetFileUrl(Pointer);
        }
        #endregion
    }
}