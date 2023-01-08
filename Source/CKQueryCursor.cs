using System;
using System.Collections.Generic;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    public class CKQueryCursor : InteropReference
    {
        public static CKQueryCursor EmptyCursor = new CKQueryCursor(IntPtr.Zero);
        
        #region Init & Dispose
        internal CKQueryCursor(IntPtr pointer) : base(pointer) {}
        #endregion
    }
}