using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class NSPredicate : InteropReference
    {
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr NSPredicate_InitWithFormat(string format);
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr NSPredicate_InitWithValue(bool value);
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSPredicate_Free(IntPtr pointer);
        
        internal NSPredicate(IntPtr pointer) : base(pointer) {}
        
        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                NSPredicate_Free(Pointer);
        }

        public static NSPredicate Init(string format)
        {
            var pointer = NSPredicate_InitWithFormat(format);
            return new NSPredicate(pointer);
        }

        public static NSPredicate Init(bool value)
        {
            var pointer = NSPredicate_InitWithValue(value);
            return new NSPredicate(pointer);
        }

        public static NSPredicate True()
        {
            return Init(true);
        }
        
        public static NSPredicate False()
        {
            return Init(false);
        }
        
        #endregion
        
        #region Get PredicateFormat
        [DllImport(InteropUtility.DLLName)]
        private static extern string NSPredicate_GetPredicateFormat(IntPtr pointer);

        public string PredicateFormat
        {
            get => NSPredicate_GetPredicateFormat(Pointer);
        }
        #endregion
    }
}