using System;
using System.Runtime.InteropServices;
using Apple.Core.Runtime;

namespace Apple.CloudKit
{
    /// <summary>
    /// An abstract base class for subscriptions.
    /// </summary>
    public abstract class CKSubscription : InteropReference
    {
        /// <summary>
        /// An object that describes the configuration of a subscription’s push notifications.
        /// </summary>
        public class CKNotificationInfo : InteropReference
        {
            #region Init & Dispose
            internal CKNotificationInfo(IntPtr pointer) : base(pointer) {}
            
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_Free(IntPtr pointer);
            protected override void OnDispose(bool isDisposing)
            {
                if (Pointer != IntPtr.Zero)
                {
                    CKNotificationInfo_Free(Pointer);
                    Pointer = IntPtr.Zero;
                }
            }
            #endregion
            
            #region Static Init
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKNotificationInfo_Init();

            public static CKNotificationInfo Init()
            {
                var pointer = CKNotificationInfo_Init();
                
                if(pointer != IntPtr.Zero)
                    return new CKNotificationInfo(pointer);

                return null;
            }
            #endregion
            
            #region Category
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetCategory(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetCategory(IntPtr pointer, string value);

            /// <summary>
            /// The name of the action group that corresponds to this notification.
            /// </summary>
            public string Category
            {
                get => CKNotificationInfo_GetCategory(Pointer);
                set => CKNotificationInfo_SetCategory(Pointer, value);
            }
            #endregion
            
            #region CollapseIdKey
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetCollapseIDKey(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetCollapseIDKey(IntPtr pointer, string value);

            /// <summary>
            /// A value that the system uses to coalesce unseen push notifications.
            /// </summary>
            public string CollapseIdKey
            {
                get => CKNotificationInfo_GetCollapseIDKey(Pointer);
                set => CKNotificationInfo_SetCollapseIDKey(Pointer, value);
            }
            #endregion
            
            #region Should Badge
            [DllImport(InteropUtility.DLLName)]
            private static extern bool CKNotificationInfo_GetShouldBadge(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetShouldBadge(IntPtr pointer, bool value);

            /// <summary>
            /// A Boolean value that determines whether an appâ€™s icon badge increments its value.
            /// </summary>
            public bool ShouldBadge
            {
                get => CKNotificationInfo_GetShouldBadge(Pointer);
                set => CKNotificationInfo_SetShouldBadge(Pointer, value);
            }
            #endregion
            
            #region ShouldSendContentAvailable
            [DllImport(InteropUtility.DLLName)]
            private static extern bool CKNotificationInfo_GetShouldSendContentAvailable(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetShouldSendContentAvailable(IntPtr pointer, bool value);

            /// <summary>
            /// A Boolean value that indicates whether the push notification includes the content available flag.
            /// </summary>
            public bool ShouldSendContentAvailable
            {
                get => CKNotificationInfo_GetShouldSendContentAvailable(Pointer);
                set => CKNotificationInfo_SetShouldSendContentAvailable(Pointer, value);
            }
            #endregion
            
            #region ShouldSendMutableContent
            [DllImport(InteropUtility.DLLName)]
            private static extern bool CKNotificationInfo_GetShouldSendMutableContent(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetShouldSendMutableContent(IntPtr pointer, bool value);

            /// <summary>
            /// A Boolean value that indicates whether the push notification sets the mutable content flag.
            /// </summary>
            public bool ShouldSendMutableContent
            {
                get => CKNotificationInfo_GetShouldSendMutableContent(Pointer);
                set => CKNotificationInfo_SetShouldSendMutableContent(Pointer, value);
            }
            #endregion
            
            #region DesiredKeys
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKNotificationInfo_GetDesiredKeys(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetDesiredKeys(IntPtr pointer, IntPtr value);

            /// <summary>
            /// The names of fields to include in the push notification's payload.
            /// </summary>
            public NSArray<string> DesiredKeys
            {
                get
                {
                    var pointer = CKNotificationInfo_GetDesiredKeys(Pointer);
                    return PointerCast<NSArrayString>(pointer);
                } 
                set => CKNotificationInfo_SetDesiredKeys(Pointer, value?.Pointer ?? IntPtr.Zero);
            }
            #endregion
            
            #if !UNITY_TVOS
            #region Title
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetTitle(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetTitle(IntPtr pointer, string value);

            /// <summary>
            /// The notification's title.
            /// </summary>
            public string Title
            {
                get => CKNotificationInfo_GetTitle(Pointer);
                set => CKNotificationInfo_SetTitle(Pointer, value);
            }
            #endregion
            
            #region TitleLocalizationKey
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetTitleLocalizationKey(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetTitleLocalizationKey(IntPtr pointer, string value);

            /// <summary>
            /// The notification's title.
            /// </summary>
            public string TitleLocalizationKey
            {
                get => CKNotificationInfo_GetTitleLocalizationKey(Pointer);
                set => CKNotificationInfo_SetTitleLocalizationKey(Pointer, value);
            }
            #endregion
            
            #region TitleLocalizationArgs
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKNotificationInfo_GetTitleLocalizationArgs(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetTitleLocalizationArgs(IntPtr pointer, IntPtr value);

            /// <summary>
            /// The fields for building a notification's title.
            /// </summary>
            public NSArray<string> TitleLocalizationArgs
            {
                get
                {
                    var pointer = CKNotificationInfo_GetTitleLocalizationArgs(Pointer);
                    return PointerCast<NSArrayString>(pointer);
                } 
                set => CKNotificationInfo_SetTitleLocalizationArgs(Pointer, value?.Pointer ?? IntPtr.Zero);
            }
            #endregion
            
            #region Subtitle
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetSubtitle(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetSubtitle(IntPtr pointer, string value);

            /// <summary>
            /// The notification's title.
            /// </summary>
            public string Subtitle
            {
                get => CKNotificationInfo_GetSubtitle(Pointer);
                set => CKNotificationInfo_SetSubtitle(Pointer, value);
            }
            #endregion
            
            #region SubtitleLocalizationKey
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetSubtitleLocalizationKey(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetSubtitleLocalizationKey(IntPtr pointer, string value);

            /// <summary>
            /// The notification's title.
            /// </summary>
            public string SubtitleLocalizationKey
            {
                get => CKNotificationInfo_GetSubtitleLocalizationKey(Pointer);
                set => CKNotificationInfo_SetSubtitleLocalizationKey(Pointer, value);
            }
            #endregion
            
            #region SubtitleLocalizationArgs
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKNotificationInfo_GetSubtitleLocalizationArgs(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetSubtitleLocalizationArgs(IntPtr pointer, IntPtr value);

            /// <summary>
            /// The fields for building a notification's title.
            /// </summary>
            public NSArray<string> SubtitleLocalizationArgs
            {
                get
                {
                    var pointer = CKNotificationInfo_GetSubtitleLocalizationArgs(Pointer);
                    return PointerCast<NSArrayString>(pointer);
                } 
                set => CKNotificationInfo_SetSubtitleLocalizationArgs(Pointer, value?.Pointer ?? IntPtr.Zero);
            }
            #endregion
            
            #region AlertBody
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetAlertBody(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetAlertBody(IntPtr pointer, string value);

            /// <summary>
            /// The text for the notification’s alert.
            /// </summary>
            public string AlertBody
            {
                get => CKNotificationInfo_GetAlertBody(Pointer);
                set => CKNotificationInfo_SetAlertBody(Pointer, value);
            }
            #endregion
            
            #region AlertLocalizationKey
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetAlertLocalizationKey(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetAlertLocalizationKey(IntPtr pointer, string value);

            /// <summary>
            /// The key that identifies the localized string for the notification’s alert.
            /// </summary>
            public string AlertLocalizationKey
            {
                get => CKNotificationInfo_GetAlertLocalizationKey(Pointer);
                set => CKNotificationInfo_SetAlertLocalizationKey(Pointer, value);
            }
            #endregion
            
            #region AlertLocalizationArgs
            [DllImport(InteropUtility.DLLName)]
            private static extern IntPtr CKNotificationInfo_GetAlertLocalizationArgs(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetAlertLocalizationArgs(IntPtr pointer, IntPtr value);

            /// <summary>
            /// The fields for building a notification’s alert.
            /// </summary>
            public NSArray<string> AlertLocalizationArgs
            {
                get
                {
                    var pointer = CKNotificationInfo_GetAlertLocalizationArgs(Pointer);
                    return PointerCast<NSArrayString>(pointer);
                } 
                set => CKNotificationInfo_SetAlertLocalizationArgs(Pointer, value?.Pointer ?? IntPtr.Zero);
            }
            #endregion
            
            #region AlertActionLocalizationKey
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetAlertActionLocalizationKey(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetAlertActionLocalizationKey(IntPtr pointer, string value);

            /// <summary>
            /// The key that identifies the localized string for the notification’s action. 
            /// </summary>
            public string AlertActionLocalizationKey
            {
                get => CKNotificationInfo_GetAlertActionLocalizationKey(Pointer);
                set => CKNotificationInfo_SetAlertActionLocalizationKey(Pointer, value);
            }
            #endregion
            
            #region AlertLaunchImage
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetAlertLaunchImage(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetAlertLaunchImage(IntPtr pointer, string value);

            /// <summary>
            /// The filename of an image to use as a launch image.
            /// </summary>
            public string AlertLaunchImage
            {
                get => CKNotificationInfo_GetAlertLaunchImage(Pointer);
                set => CKNotificationInfo_SetAlertLaunchImage(Pointer, value);
            }
            #endregion
            
            #region SoundName
            [DllImport(InteropUtility.DLLName)]
            private static extern string CKNotificationInfo_GetSoundName(IntPtr pointer);
            [DllImport(InteropUtility.DLLName)]
            private static extern void CKNotificationInfo_SetSoundName(IntPtr pointer, string value);

            /// <summary>
            /// The filename of the sound file to play when a notification arrives.
            /// </summary>
            public string SoundName
            {
                get => CKNotificationInfo_GetSoundName(Pointer);
                set => CKNotificationInfo_SetSoundName(Pointer, value);
            }
            #endregion
            #endif
        }
        
        /// <summary>
        /// Constants that identify a subscription's behavior.
        /// </summary>
        public enum CKSubscriptionType : long
        {
            /// <summary>
            /// A constant that indicates the subscription is query-based.
            /// </summary>
            Query = 1,
            /// <summary>
            /// A constant that indicates the subscription is zone-based.
            /// </summary>
            RecordZone = 2,
            /// <summary>
            /// A constant that indicates the subscription is database-based.
            /// </summary>
            Database = 3,
        }
        
        #region Init & Dispose
        protected CKSubscription(IntPtr pointer) : base(pointer) {}
        
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKSubscription_Free(IntPtr pointer);
        
        protected override void OnDispose(bool isDisposing)
        {
            if (Pointer != IntPtr.Zero)
            {
                CKSubscription_Free(Pointer);
                Pointer = IntPtr.Zero;
            }
        }
        #endregion
        
        #region SubscriptionId
        [DllImport(InteropUtility.DLLName)]
        private static extern string CKSubscription_GetSubscriptionID(IntPtr pointer);

        /// <summary>
        /// Constants that identify a subscriptionâ€™s behavior.
        /// </summary>
        public string SubscriptionId
        {
            get => CKSubscription_GetSubscriptionID(Pointer);
        }
        #endregion
        
        #region SubscriptionType
        [DllImport(InteropUtility.DLLName)]
        private static extern CKSubscriptionType CKSubscription_GetSubscriptionType(IntPtr pointer);

        /// <summary>
        /// The behavior that a subscription provides.
        /// </summary>
        public CKSubscriptionType SubscriptionType
        {
            get => CKSubscription_GetSubscriptionType(Pointer);
        }
        #endregion
        
        #region NotificationInfo
        [DllImport(InteropUtility.DLLName)]
        private static extern IntPtr CKSubscription_GetNotificationInfo(IntPtr pointer);
        [DllImport(InteropUtility.DLLName)]
        private static extern void CKSubscription_SetNotificationInfo(IntPtr pointer, IntPtr notificationInfo);

        /// <summary>
        /// The configuration for a subscription’s push notifications.
        /// </summary>
        public CKNotificationInfo NotificationInfo
        {
            get
            {
                var pointer = CKSubscription_GetNotificationInfo(Pointer);
                
                if(pointer != IntPtr.Zero)
                    return new CKNotificationInfo(pointer);

                return null;
            }
            set
            {
                CKSubscription_SetNotificationInfo(Pointer, value?.Pointer ?? IntPtr.Zero);
            }
        }
        #endregion
    }
}