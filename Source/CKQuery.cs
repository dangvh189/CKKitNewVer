using System;
using System.Linq;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKQuery : InteropReference
    {
        #region Init & Dispose
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQuery_Init(string recordType, IntPtr predicate);
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQuery_Free(IntPtr pointer);
        
        internal CKQuery(IntPtr pointer) : base(pointer) {}

        protected override void OnDispose(bool isDisposing)
        {
            if(Pointer != IntPtr.Zero)
                CKQuery_Free(Pointer);
        }

        public static CKQuery Init(string recordType, NSPredicate predicate)
        {
            var pointer = CKQuery_Init(recordType, predicate.Pointer);
            return new CKQuery(pointer);
        }
        #endregion

        #region Get RecordType
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKQuery_GetRecordType(IntPtr pointer);

        public string RecordType
        {
            get => CKQuery_GetRecordType(Pointer);
        }
        #endregion
        
        #region Get Predicate
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKQuery_GetPredicate(IntPtr pointer);

        public NSPredicate Predicate
        {
            get
            {
                var pointer = CKQuery_GetPredicate(Pointer);
                return new NSPredicate(pointer);
            }
        }
        #endregion
        
        #region SortDescriptors    
        [DllImport(InteropUtility.DLLName)]
        private static extern InteropStructArray CKQuery_GetSortDescriptors(IntPtr pointer);

        [DllImport(InteropUtility.DLLName)]
        private static extern void CKQuery_SetSortDescriptors(IntPtr pointer, InteropStructArray descriptors);

        public NSSortDescriptor[] SortDescriptors
        {
            get
            {
                var pointers = CKQuery_GetSortDescriptors(Pointer).ToArray<IntPtr>();
                var descriptors = pointers.Select(pointer => new NSSortDescriptor(pointer));

                return descriptors.ToArray();
            }
            set
            {
                var pointers = value.Select(descriptor => descriptor.Pointer).ToArray();
                var descriptorsData = InteropStructArray.From(pointers, out var handle);
                CKQuery_SetSortDescriptors(Pointer, descriptorsData);
                handle.Free();
            }
        }
        #endregion
    }
}