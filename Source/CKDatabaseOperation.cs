using System;
using System.Threading.Tasks;

namespace Apple.CloudKit
{
    public abstract class CKDatabaseOperation<TResultType> : CKOperation
    {
        internal CKDatabaseOperation(IntPtr pointer) : base(pointer) {}

        internal virtual Task<TResultType> OnSetupCompletionCallback(CKDatabase database)
        {
            return Task.FromResult(default(TResultType));
        }
    }
}