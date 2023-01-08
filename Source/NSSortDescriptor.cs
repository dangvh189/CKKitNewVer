using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class NSSortDescriptor : InteropReference
    {
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr NSSortDescriptor_Init(string key, bool isAscending);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSSortDescriptor_Free(IntPtr pointer);
        
        internal NSSortDescriptor(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                NSSortDescriptor_Free(Pointer);
        }
        #endregion

        #region Get Key
        [DllImport(InteropUtility.DLLName)]
        private static extern string NSSortDescriptor_GetKey(IntPtr pointer);

        public string Key
        {
            get => NSSortDescriptor_GetKey(Pointer);
        }
        #endregion
        
        #region Get Ascending
        [DllImport(InteropUtility.DLLName)]
        private static extern bool NSSortDescriptor_GetAscending(IntPtr pointer);

        public bool IsAscending
        {
            get => NSSortDescriptor_GetAscending(Pointer);
        }
        #endregion
    }
}