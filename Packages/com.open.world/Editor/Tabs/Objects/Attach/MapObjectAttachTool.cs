using OpenWorld.DATA;
using OpenWorldEditor;
using OpenWorldEditor.SceneWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OpenWorldEditor.MapObjectTab.Attach
{
    public static class MapObjectAttachTool
    {
        private static int _selectedLayerIndex = 0; // Индекс выбранного слоя

        static MapObjectAttachTool()
        {
            var settings = MapProjectSettings.GetOrCreateSettings();
            _selectedLayerIndex = PlayerPrefs.GetInt("MapObjectAttachTool.SelectedLayerIndex", 0);
            _selectedLayerIndex = string.IsNullOrEmpty(settings.GetLayer(_selectedLayerIndex)) ? 0 : _selectedLayerIndex;
        }

        public static void Draw()
        {
            GUILayout.Space(15.0f);

            DrawLayerSelector();

            GUILayout.Space(15.0f);

            if (GUILayout.Button("Attach Selected Object"))
            {
                AttachSelectedObjectsToMap(Selection.gameObjects);
            }
            GUILayout.Space(10.0f);
            if (GUILayout.Button("Attach all Objects"))
            {
                List<GameObject> rootObjects = new List<GameObject>();
                Scene scene = SceneManager.GetActiveScene();
                scene.GetRootGameObjects(rootObjects);

                AttachSelectedObjectsToMap(rootObjects.ToArray());
            }
        }

        private static void DrawLayerSelector()
        {
            var settings = MapProjectSettings.GetOrCreateSettings();
            var layerNamesList = new List<string>();

            for (int i = 0; i < MapProjectSettings.MAX_LAYERS; i++)
            {
                string layerName = settings.GetLayer(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    layerNamesList.Add(layerName);
                }
            }

            _selectedLayerIndex = EditorGUILayout.Popup("Layer", _selectedLayerIndex, layerNamesList.ToArray());
        }

        private static void AttachSelectedObjectsToMap(GameObject[] gameObjects)
        {
            List<AttachObject> objectsToAttach = new List<AttachObject>();
            foreach (GameObject obj in gameObjects)
            {
                if (obj == null || obj.transform.parent != null) continue; // Skip if the object is a child

                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(obj);
                if (prefab == null) continue; // Skip if the object has no prefab

                string pathToPrefab = AssetDatabase.GetAssetPath(prefab);
                if (string.IsNullOrEmpty(pathToPrefab)) continue; // Skip if the object has no prefab

                string guid = AssetDatabase.AssetPathToGUID(pathToPrefab);

                // Check if the prefab is attached to the correct Addressable group
                var settings = AddressableAssetSettingsDefaultObject.Settings;
                var entry = settings.FindAssetEntry(guid);
                if (entry == null)
                {
                    Debug.LogError("The added object is not attached to the Addressable group");
                    continue;
                }

                if (string.IsNullOrEmpty(guid))
                {
                    Debug.LogError("Failed to find GUID");
                    continue;
                }

                objectsToAttach.Add(new AttachObject(obj, prefab)); // Add object to the list for attaching to the map
            }

            if (objectsToAttach.Count == 0) return; // Exit if there are no objects to attach

            string objectNames = string.Join(", ", objectsToAttach.Select(a => a.SceneObject.name));

            if (EditorUtility.DisplayDialog("Attach Object", $"Attach {objectNames} to the map?", "Yes", "No"))
            {
                foreach (AttachObject attachObject in objectsToAttach)
                {
                    int xKMBlock = (int)(attachObject.SceneObject.transform.position.x / 1000.0f);
                    int yKMBlock = (int)(attachObject.SceneObject.transform.position.z / 1000.0f);
                    int xTRBlock = (int)((attachObject.SceneObject.transform.position.x % 1000.0f) / TabSetting.Map.TileSize);
                    int yTRBlock = (int)((attachObject.SceneObject.transform.position.z % 1000.0f) / TabSetting.Map.TileSize);

                    Tile mapTile = AssetDatabase.LoadAssetAtPath<Tile>(TabSetting.Map.GetPath(xKMBlock, yKMBlock, xTRBlock, yTRBlock));

                    if (mapTile == null)
                    {
                        Debug.LogError("Failed to load tile");
                        return;
                    }

                    MapObject mapObject = new MapObject(
                        _selectedLayerIndex,
                        attachObject.Prefab,
                        attachObject.SceneObject.transform.position,
                        attachObject.SceneObject.transform.rotation,
                        attachObject.SceneObject.transform.localScale
                    );

                    mapTile.AddObject(mapObject);
                    EditorUtility.SetDirty(mapTile);

                    GameObject.DestroyImmediate(attachObject.SceneObject);
                }
                WorldLoader.Reload();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                PlayerPrefs.SetInt("MapObjectAttachTool.SelectedLayerIndex", _selectedLayerIndex);
            }
        }
    }
}