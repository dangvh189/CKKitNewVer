using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Apple.CloudKit.Editor
{
    [CustomEditor(typeof(AppleCloudKitBuildStep))]
    public class AppleCloudKitBuildStepEditor : UnityEditor.Editor
    {
        private SerializedObject _serializedSettings;
        private ReorderableList _list;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var settings = target as AppleCloudKitBuildStep;

            // Use Containers...
            GUILayout.BeginHorizontal();
            settings.UseiCloudContainers = EditorGUILayout.Toggle("Use Containers", settings.UseiCloudContainers);
            if (GUILayout.Button("More Info", EditorStyles.toolbarButton, GUILayout.Width(75)))
            {
                Application.OpenURL("https://developer.apple.com/documentation/cloudkit/ckcontainer");
            }
            GUILayout.EndHorizontal();

            if(settings.UseiCloudContainers)
            {
                settings.iCloudContainerEnvironment = (CloudKitContainerEnvironment)EditorGUILayout.EnumPopup("Container Environment", settings.iCloudContainerEnvironment);

                _serializedSettings = _serializedSettings ?? new SerializedObject(settings);

                // Ensure default container identifiers...
                if (settings.iCloudContainerIdentifiers.Count == 0)
                {
                    settings.iCloudContainerIdentifiers.Add($"iCloud.$(CFBundleIdentifier)");
                    EditorUtility.SetDirty(settings);
                }

                // Initialize container identifier list...
                if (_list == null)
                {
                    _list = new ReorderableList(_serializedSettings, _serializedSettings.FindProperty("iCloudContainerIdentifiers"), true, true, true, true);
                    _list.drawHeaderCallback += OnDrawContainerIdentifiersHeader;
                    _list.drawElementCallback += OnDrawContainerIdentifier;
                }

                // Render container identifiers...
                _serializedSettings.Update();
                _list.DoLayoutList();
                _serializedSettings.ApplyModifiedProperties();
            }

            // Key/Value store...
            GUILayout.BeginHorizontal();
            settings.UseUbiquityKeyValueStore = EditorGUILayout.Toggle("Use KeyValue Store", settings.UseUbiquityKeyValueStore);
            if (GUILayout.Button("More Info", EditorStyles.toolbarButton, GUILayout.Width(75)))
            {
                Application.OpenURL("https://developer.apple.com/documentation/foundation/nsubiquitouskeyvaluestore");
            }
            GUILayout.EndHorizontal();

            if(settings.UseUbiquityKeyValueStore)
            {
                settings.UbiquityKeyValueStoreIdentifier = EditorGUILayout.TextField("KeyValue Store Identifier", settings.UbiquityKeyValueStoreIdentifier);
            }

            // APS...
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneOSX)
            {
                GUILayout.BeginHorizontal();
                settings.UseAPS = EditorGUILayout.Toggle("Use APS", settings.UseAPS);
                if (GUILayout.Button("More Info", EditorStyles.toolbarButton, GUILayout.Width(75)))
                {
                    Application.OpenURL("https://developer.apple.com/documentation/bundleresources/entitlements/aps-environment");
                }

                GUILayout.EndHorizontal();

                if (settings.UseAPS)
                {
                    settings.APSEnvironment = (APSEnvironment) EditorGUILayout.EnumPopup("APS Environment", settings.APSEnvironment);
                }
            }


            if(GUI.changed)
            {
                EditorUtility.SetDirty(settings);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDrawContainerIdentifier(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            var newValue = EditorGUI.TextField(rect, element.stringValue);

            if(element.stringValue != newValue)
            {
                element.stringValue = newValue;
            }
        }

        private void OnDrawContainerIdentifiersHeader(Rect rect)
        {
            GUI.Label(rect, "Container Identifiers");
        }
    }
}