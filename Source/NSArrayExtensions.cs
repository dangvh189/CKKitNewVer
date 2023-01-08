using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    #region CKSubscription
    public class NSArrayCKSubscription : NSArray<CKSubscription>
    {
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr NSArray_GetCKSubscriptionAt(IntPtr pointer, int index);
        
        public override CKSubscription ElementAtIndex(int index)
        {
            return PointerCast<CKSubscription>(NSArray_GetCKSubscriptionAt(Pointer, index));
        }
    }

    public class NSMutableArrayCKSubscription : NSArrayCKSubscription, INSMutableArray<CKSubscription>
    {
        [DllImport(InteropUtility.DLLName)]
        private static extern void NSMutableArray_AddCKSubscription(IntPtr pointer, IntPtr subscription);
        
        public void Add(CKSubscription value)
        {
            NSMutableArray_AddCKSubscription(Pointer, value.Pointer);
        }
    }
    #endregion
}