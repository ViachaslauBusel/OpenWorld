using OpenWorld;
using OpenWorld.DATA;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.MapObjectTab.Display
{
    internal static class MapObjectDisplayTool
    {
        private static GameObject _lastActiveObject;
        private static OpenWorldProjectSettings _settings;
        private static Vector2 _scrollPosition = Vector2.zero;
        /// <summary>Selected object on the scene</summary>
        private static EditorTile _selectedTile = null;
        /// <summary>List of objects attached to the selected tile and displayed in the UI</summary>
        private static List<DisplayObject> _displayedAttachedObjects = new List<DisplayObject>();
        /// <summary>Selected object from the list of objects attached to the tile</summary>
        private static DisplayObject _selectedDisplayObject;
        /// <summary>List of selected tiles</summary>
        private static List<EditorTile> _selectedTiles = new List<EditorTile>();


        static MapObjectDisplayTool()
        {
            _settings = OpenWorldProjectSettings.GetOrCreateSettings();
        }

        public static bool IsActiveObjectHasChanged()
        {
            if(_lastActiveObject != Selection.activeObject)
            {
                _lastActiveObject = Selection.activeObject as GameObject;
                return true;
            }
            return false;
        }

        public static void Draw()
        {
            if (Selection.activeObject == null) return;

            GameObject selectedGameObject = Selection.activeObject as GameObject;
            if (selectedGameObject == null) return;

            // If a different object was selected on the scene
            if (IsActiveObjectHasChanged())
            {
                var editorTile = _lastActiveObject.GetComponentInParent<EditorTile>();

                if (editorTile != _selectedTile)
                {
                    _selectedTile = editorTile;
                    _displayedAttachedObjects.Clear();
                    _selectedTiles.Clear();
                    foreach (GameObject selectedObj in Selection.gameObjects)
                    {
                        EditorTile tile = selectedObj.GetComponentInParent<EditorTile>();
                        if (tile != null) _selectedTiles.Add(tile);
                    }

                    foreach (EditorTile tile in _selectedTiles)
                    {
                        foreach (MapEntity mapObject in tile.Data.Objects)
                        {
                            DisplayObject displayObject = DisplayObject.Create(
                            tile.Data,
                            mapObject,
                            mapObject.Prefab?.editorAsset as GameObject,
                            tile.GetSceneObjectByMapObject(mapObject),
                            _settings.GetLayer(mapObject.Layer)
                            );
                            if (displayObject != null) _displayedAttachedObjects.Add(displayObject);
                        }
                    }
                }
            }

            if (GUILayout.Button("Detach All"))
            {
                if (EditorUtility.DisplayDialog("Detach Objects", $"Are you sure you want to detach {_displayedAttachedObjects.Count} objects from the map?", "Yes", "No"))
                {
                    _displayedAttachedObjects.ForEach((d) => d.Detach());
                    _displayedAttachedObjects.Clear();
                }
            }

            if (_displayedAttachedObjects.Count == 0) return;

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                for (int i = _displayedAttachedObjects.Count - 1; i >= 0; i--)
                {
                    GUILayout.Space(10.0f);
                    GUILayout.BeginHorizontal();

                    GUILayout.Label(_displayedAttachedObjects[i]?.Prefab.name ?? "Prefab not found");

                    // Display the object's layer
                    GUILayout.Label(_displayedAttachedObjects[i].LayerName);

                    GUI.enabled = _selectedDisplayObject != _displayedAttachedObjects[i];
                    if (GUILayout.Button("Select"))
                    {
                        _selectedDisplayObject = _displayedAttachedObjects[i];
                        Selection.activeObject = _displayedAttachedObjects[i].ObjectOnScene;
                    }
                    GUI.enabled = true;

                    if (GUILayout.Button("Detach"))
                    {
                        _displayedAttachedObjects[i].Detach();
                        // Remove object from the UI list
                        _displayedAttachedObjects.RemoveAt(i);
                    }

                    if (GUILayout.Button("Delete"))
                    {
                        _displayedAttachedObjects[i].Delete();
                        // Remove object from the UI list
                        _displayedAttachedObjects.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
    }
}