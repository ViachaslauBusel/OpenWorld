
using OpenWorld.DATA;
using OpenWorldEditor;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace OpenWorld.DATA
{

    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {
        private string _inputName = "";
        private float _inputHeight;
        private MapCreationError _error;

        public override void OnInspectorGUI()
        {
            Map map = target as Map;

            ShowError();

            if (string.IsNullOrEmpty(map.MapName)) // Map not created
            {
                map.DisplayMapCreationGUI(ref _inputName, ref _inputHeight);

                if (GUILayout.Button("Create Map"))
                {
                    _error = map.SetupMapProperties(ref _inputName, ref _inputHeight);

                    if (_error != MapCreationError.None) return;

                    try
                    {
                        MapGeneration.GenerationWorld(map, _inputHeight);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        map.ResetMapProperties();
                        _error = MapCreationError.MapCreationFailed;
                        return;
                    }
                    // else {   return; }
                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), map.MapName);
                    _error = MapCreationError.None;
                }
            }
            else
            {
                map.DisplayMapSettingsGUI();

                if (GUILayout.Button("Apply"))
                {
                    if (EditorUtility.DisplayDialog("Applying Settings", "Setting a lower Control Texture Resolution will irreversibly lose quality", "Continue", "Cancel"))
                    {
                        map.ApplyMapSetting();
                    }

                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Deleting Map", "Delete the map: " + map.MapName + "?", "Yes", "No"))
                    {
                        FileUtil.DeleteFileOrDirectory($"Assets/MapData/{map.MapName}/");

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(target));
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

        private void ShowError()
        {
            switch (_error)
            {
                case MapCreationError.None:
                    break;
                case MapCreationError.InvalidName:
                    EditorGUILayout.HelpBox("Invalid name", MessageType.Error);
                    break;
                case MapCreationError.InvalidSize:
                    EditorGUILayout.HelpBox("Invalid size", MessageType.Error);
                    break;
                case MapCreationError.NameAlreadyExists:
                    EditorGUILayout.HelpBox("Name already exists", MessageType.Error);
                    break;
                case MapCreationError.MapCreationFailed:
                    EditorGUILayout.HelpBox("Error creating map", MessageType.Error);
                    break;
                case MapCreationError.UndefinedHeight:
                    EditorGUILayout.HelpBox("Height not set", MessageType.Error);
                    break;
                case MapCreationError.IncorrectSpecifiedHeight:
                    EditorGUILayout.HelpBox("Incorrect specified height", MessageType.Error);
                    break;
                case MapCreationError.AssetBundleSettingFailed:
                    EditorGUILayout.HelpBox("Failed to set Asset Bundle", MessageType.Error);
                    break;
                default:
                    EditorGUILayout.HelpBox("Unknown error", MessageType.Error);
                    break;
            }
        }
    }
}
