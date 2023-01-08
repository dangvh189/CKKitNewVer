namespace Apple.CloudKit
{
    /// <summary>
    /// Provides access to the constants defined for each change
    /// reason as provided by native os.
    /// </summary>
    public enum DidChangeExternallyReason : int
    {
        ServerChange = 0,
        InitialSyncChange = 1,
        QuotaViolationChange = 2,
        AccountChange = 3
    }
}