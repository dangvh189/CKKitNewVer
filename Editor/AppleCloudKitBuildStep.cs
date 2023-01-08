using System.Collections.Generic;
using Apple.Core;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif

namespace Apple.CloudKit.Editor
{
    public class AppleCloudKitBuildStep : AppleBuildStep
    {
        public override string DisplayName => "CloudKit";
        public override string DisplayIcon => "cloud.fill";

        const string _iosFrameworkGuid = "70a7dd15b96a743fab073e0abb6a46c7";
        const string _tvOSFrameworkGuid = "b7648eff34115496b8e60e928995dbd3";
        const string _macOSFrameworkGuid = "bd51646559abb4a9ebdc2213e7e064d3";

        public bool UseiCloudContainers = false;
        public List<string> iCloudContainerIdentifiers = new List<string>();
        public CloudKitContainerEnvironment iCloudContainerEnvironment = CloudKitContainerEnvironment.Auto;
        public bool UseUbiquityKeyValueStore = false;
        public string UbiquityKeyValueStoreIdentifier = "$(TeamIdentifierPrefix)$(CFBundleIdentifier)";
        
        [Tooltip("Adds the aps-environment entitlement, which may be required on some platforms when publishing to TestFlight.")]
        public bool UseAPS = false;
        public APSEnvironment APSEnvironment = APSEnvironment.Auto;

        #if UNITY_EDITOR
        public AppleBuildProfile GetAppleBuildProfile()
        {
            var path = AssetDatabase.GetAssetPath(this);
            return AssetDatabase.LoadAssetAtPath<AppleBuildProfile>(path);
        }
        #endif
        
#if UNITY_EDITOR_OSX
        public override void OnProcessEntitlements(AppleBuildProfile appleBuildProfile, BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument entitlements)
        {
            if(UseiCloudContainers || UseUbiquityKeyValueStore)
            {
                var services = entitlements.root.CreateArray("com.apple.developer.icloud-services");
                services.AddString("CloudKit");
            }

            // Containers...
            if(UseiCloudContainers)
            {
                var containers = entitlements.root.CreateArray("com.apple.developer.icloud-container-identifiers");

                foreach (var container in iCloudContainerIdentifiers)
                {
                    var safeContainerName = container;

                    // Replace the $(CFBundleIdentifier) for non-xcode generated mac builds...
                    safeContainerName = safeContainerName.Replace("$(CFBundleIdentifier)", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone));

                    containers.AddString(safeContainerName);
                }

                switch(iCloudContainerEnvironment)
                {
                    case CloudKitContainerEnvironment.Auto:
                        entitlements.root.SetString("com.apple.developer.icloud-container-environment", EditorUserBuildSettings.iOSBuildConfigType == iOSBuildType.Debug ? "Development" : "Production");
                        break;
                    default:
                        entitlements.root.SetString("com.apple.developer.icloud-container-environment", iCloudContainerEnvironment.ToString());
                        break;
                }
            }

            // KeyStore...
            if(UseUbiquityKeyValueStore)
            {
                var safeIdentifier = UbiquityKeyValueStoreIdentifier;

                // Replace $(TeamIdentifierPrefix) and $(CFBundleIdentifier)...
                safeIdentifier = safeIdentifier.Replace("$(TeamIdentifierPrefix)", PlayerSettings.iOS.appleDeveloperTeamID);
                safeIdentifier = safeIdentifier.Replace("$(CFBundleIdentifier)", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone));

                entitlements.root.SetString("com.apple.developer.ubiquity-kvstore-identifier", safeIdentifier);
            }

            // APS...
            if(UseAPS && buildTarget != BuildTarget.StandaloneOSX)
            {
                switch (APSEnvironment)
                {
                    case APSEnvironment.Auto:
                        entitlements.root.SetString("aps-environment", EditorUserBuildSettings.iOSBuildConfigType == iOSBuildType.Debug ? "Development" : "Production");
                        break;
                    default:
                        entitlements.root.SetString("aps-environment", APSEnvironment.ToString());
                        break;
                }
            }
        }

        public override void OnProcessFrameworks(AppleBuildProfile appleBuildProfile, BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            var frameworkGuid = string.Empty;

            switch(buildTarget)
            {
                case BuildTarget.iOS:
                    frameworkGuid = _iosFrameworkGuid;
                    break;
                case BuildTarget.StandaloneOSX:
                    frameworkGuid = _macOSFrameworkGuid;
                    break;
                case BuildTarget.tvOS:
                    frameworkGuid = _tvOSFrameworkGuid;
                    break;
            }

            // Prepare paths...
            var localBinaryPath = AssetDatabase.GUIDToAssetPath(frameworkGuid);

            // Delete and copy...
            AppleFrameworkUtility.CopyAndEmbed(localBinaryPath, buildTarget, pathToBuiltTarget, pBXProject);
            AppleFrameworkUtility.AddFrameworkToProject("CloudKit.framework", false, buildTarget, pBXProject);
        }

        public override void OnProcessExportPlistOptions(AppleBuildProfile appleBuildProfile, BuildTarget buildTarget, string pathToBuiltProject, PlistDocument exportPlistOptions)
        {
            // Set iCloudContainerEnvironment...
            if (UseiCloudContainers)
            {
                switch (iCloudContainerEnvironment)
                {
                    case CloudKitContainerEnvironment.Auto:
                        exportPlistOptions.root.SetString("iCloudContainerEnvironment", EditorUserBuildSettings.iOSBuildConfigType == iOSBuildType.Debug ? "Development" : "Production");
                        break;
                    default:
                        exportPlistOptions.root.SetString("iCloudContainerEnvironment", iCloudContainerEnvironment.ToString());
                        break;
                }
            }
        }
#endif
    }
}