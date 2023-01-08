namespace Apple.CloudKit
{
    public enum QualityOfService : int
    {
        Default = -1,
        Background = 9,
        Utility = 17,
        UserInitiated = 25,
        UserInteractive = 33
    }
}