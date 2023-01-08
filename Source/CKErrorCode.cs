using Apple.Core.Runtime;
using UnityEngine.PlayerLoop;

namespace Apple.CloudKit
{
    public enum CKErrorCode : int
    {
        InternalError = 1,
        PartialFailure = 2,
        NetworkUnavailable = 3,
        NetworkFailure = 4,
        BadContainer = 5,
        ServiceUnavailable = 6,
        RequestRateLimited = 7,
        MissingEntitlement = 8,
        NotAuthenticated = 9,
        UnknownItem = 11,
        InvalidArguments = 12,
        ServerRecordChanged = 14,
        ServerRejectedRequest = 15,
        AssetFileNotFound = 16,
        AssetFileModified = 17,
        IncompatibleVersion = 18,
        ConstraintViolation = 19,
        OperationCancelled = 20,
        ChangeTokenExpiration = 21,
        BatchRequestFailed = 22,
        ZoneBusy = 23,
        BadDatabase = 24,
        QuotaExceeded = 25,
        ZoneNotFound = 26,
        LimitExceeded = 27,
        UserDeletedZone = 28,
        TooManyParticipants = 29,
        AlreadyShared = 30,
        ReferenceViolation = 31,
        ManagedAccountRestriction = 32,
        ServerResponseLost = 34,
    }
}