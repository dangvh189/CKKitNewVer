using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public abstract class CKOperation : InteropReference
    {
        public class CKOperationConfiguration : InteropReference
        {
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKOperationConfiguration_Free(IntPtr pointer);
            
            internal CKOperationConfiguration(IntPtr pointer) : base(pointer) {}

            protected override void OnDispose(bool isDisposing)
            {
                if(Pointer != IntPtr.Zero)
                    CKOperationConfiguration_Free(Pointer);
                    
            }
            
            #region Interop Get
            [DllImport(InteropUtility.DLLName)]
            private static extern bool CKOperationConfiguration_GetAllowsCellularAccess(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern bool CKOperationConfiguration_GetIsLongLived(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern QualityOfService CKOperationConfiguration_GetQualityOfService(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern double CKOperationConfiguration_GetTimeoutIntervalForRequest(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern double CKOperationConfiguration_GetTimeoutIntervalForResource(IntPtr pointer);
            #endregion

            #region Interop Set
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKOperationConfiguration_SetAllowsCellularAccess(IntPtr pointer, bool value);
            
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKOperationConfiguration_SetIsLongLived(IntPtr pointer, bool value);
            
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKOperationConfiguration_SetQualityOfService(IntPtr pointer, QualityOfService value);

            [DllImport(InteropUtility.DLLName)]
            private static extern void CKOperationConfiguration_SetTimeoutIntervalForRequest(IntPtr pointer, double value);
            
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKOperationConfiguration_SetTimeoutIntervalForResource(IntPtr pointer, double value);
            #endregion

            public bool AllowsCellularAccess
            {
                get => CKOperationConfiguration_GetAllowsCellularAccess(Pointer);
                set => CKOperationConfiguration_SetAllowsCellularAccess(Pointer, value);
            }
            
            public bool IsLongLived
            {
                get => CKOperationConfiguration_GetIsLongLived(Pointer);
                set => CKOperationConfiguration_SetIsLongLived(Pointer, value);
            }
            
            public QualityOfService QualityOfService
            {
                get => CKOperationConfiguration_GetQualityOfService(Pointer);
                set => CKOperationConfiguration_SetQualityOfService(Pointer, value);
            }
            
            public double TimeoutIntervalForRequest
            {
                get => CKOperationConfiguration_GetTimeoutIntervalForRequest(Pointer);
                set => CKOperationConfiguration_SetTimeoutIntervalForRequest(Pointer, value);
            }
            
            public double TimeoutIntervalForResource
            {
                get => CKOperationConfiguration_GetTimeoutIntervalForResource(Pointer);
                set => CKOperationConfiguration_SetTimeoutIntervalForResource(Pointer, value);
            }
        }
        
        internal CKOperation(IntPtr pointer) : base(pointer) {}
        
        #region Configuration
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKOperation_GetConfiguration(IntPtr pointer);

        private CKOperationConfiguration _configuration;
        
        public CKOperationConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var pointer = CKOperation_GetConfiguration(Pointer);
                    
                    if(pointer != IntPtr.Zero)
                        _configuration = new CKOperationConfiguration(pointer);
                }

                return _configuration;
            }
        }
        #endregion
    }
    
}