using System.Collections.Generic;

namespace Apple.CloudKit
{
    [System.Serializable]
    public class DidChangeExternallyResponse
    {
        public int NSUbiquitousKeyValueStoreChangeReasonKey;
        public List<string> NSUbiquitousKeyValueStoreChangedKeysKey;
    }
}