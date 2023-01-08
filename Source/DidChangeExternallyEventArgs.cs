using System;

namespace Apple.CloudKit
{
    public class DidChangeExternallyEventArgs : EventArgs
    {
        public DidChangeExternallyResponse Response { get; }

        public DidChangeExternallyEventArgs(DidChangeExternallyResponse response)
        {
            Response = response;
        }
    }
}