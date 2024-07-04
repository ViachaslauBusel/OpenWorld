using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    public class OpenWorldProjectSettings : ScriptableObject
    {
        public const int MAX_LAYERS = 32;
        // The path where the settings will be saved
        private const string settingsPath = "Assets/Editor/OpenWorldProjectSettings.asset";

        [SerializeField, HideInInspector]
        private string[] _objectsLayers = new string[MAX_LAYERS];

        public void SetLayer(int index, string name)
        {
            if (index < 0 || index >= MAX_LAYERS) return;
            _objectsLayers[index] = name;
        }

        public string GetLayer(int index)
        {
            if(index < 0 || index >= MAX_LAYERS) return null;
            
            if (index == 0 && string.IsNullOrEmpty(_objectsLayers[index]))
            {
                _objectsLayers[index] = "Default";
            }
            return _objectsLayers[index];
        }

        // Loading or creating the settings object
        internal static OpenWorldProjectSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<OpenWorldProjectSettings>(settingsPath);
            if (settings == null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(settingsPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
                }
                settings = ScriptableObject.CreateInstance<OpenWorldProjectSettings>();
                AssetDatabase.CreateAsset(settings, settingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        // Registering the settings tab
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Project/OpenWorldSettings", SettingsScope.Project)
            {
                label = "Open World",
                guiHandler = (searchContext) =>
                {
                    var settings = OpenWorldProjectSettings.GetOrCreateSettings();

                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label("Object Layers", EditorStyles.boldLabel);

                    // Display each layer with a TextField for renaming
                    for (int i = 0; i < OpenWorldProjectSettings.MAX_LAYERS; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label($"Layer {i}", GUILayout.Width(150));
                        string newLayerName = EditorGUILayout.TextField(settings.GetLayer(i));
                        if (newLayerName != settings.GetLayer(i))
                        {
                            // Update layer name if it has been changed
                            settings.SetLayer(i, newLayerName);
                            EditorUtility.SetDirty(settings);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                },

                // Сохранение настроек при изменении
                keywords = new HashSet<string>(new[] { "OpenWorld", "Setting" })
            };
        }
    }
}
