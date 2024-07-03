using OpenWorld;
using System;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.EditorScene
{
    /// <summary>
    /// Manages loading and updating the map in the editor.
    /// </summary>
    public class MapEditorLoader
    {
        private static MapLoader _mapLoader;

        /// <summary>
        /// Gets the MapLoader instance, creating it if necessary.
        /// </summary>
        public static MapLoader CreateOrGetMapLoader()
        {
            if (_mapLoader == null)
            {
                var obj = GameObject.Find("MapEditor");
                _mapLoader = obj?.GetComponent<MapLoader>() ?? CreateMapLoader();
            }
            return _mapLoader;
        }

        /// <summary>
        /// Destroys the current MapLoader and its GameObject.
        /// </summary>
        public static void Destroy()
        {
            if (_mapLoader != null)
            {
                GameObject.DestroyImmediate(_mapLoader.gameObject);
                _mapLoader = null;
            }

            GameObject obj = GameObject.Find("MapEditor");
            while (obj != null)
            {
                GameObject.DestroyImmediate(obj);
                obj = GameObject.Find("MapEditor");
            }
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// Reloads the map by recreating the MapLoader.
        /// </summary>
        public static void Reload()
        {
            if (_mapLoader != null)
            {
                GameObject.DestroyImmediate(_mapLoader.gameObject);
            }

            CreateOrGetMapLoader();
        }

        /// <summary>
        /// Creates and initializes a new MapLoader GameObject.
        /// </summary>
        /// <returns>The created MapLoader component.</returns>
        private static MapLoader CreateMapLoader()
        {
            if (TabSetting.Map == null || TabSetting.MapSettings == null)
            {
                EditorGUILayout.HelpBox("Map or settings not selected", MessageType.Error);
                return null;
            }

            var target = FindTarget();
            if (target == null) return null;

            var obj = new GameObject("MapEditor");
            var newMapLoader = obj.AddComponent<MapLoader>();
            newMapLoader.SetTarget(target);
            newMapLoader.SetMap(TabSetting.Map);
            newMapLoader.SetSetting(TabSetting.MapSettings);
            newMapLoader.LoadMap();
            RenderSettings.fog = false;
            return newMapLoader;
        }

        /// <summary>
        /// Finds the target transform for the map loader based on the active scene view camera.
        /// </summary>
        /// <returns>The camera's transform, or null if not found.</returns>
        private static Transform FindTarget()
        {
            var camera = SceneView.lastActiveSceneView?.camera;
            return camera?.transform;
        }

        /// <summary>
        /// Updates the tracking object's position if needed.
        /// </summary>
        internal static void OnSceneGUI()
        {
            if (_mapLoader == null) return;

            if(_mapLoader.TrackingObject == null)// If the map has lost its tracking object
            {
                Transform target = FindTarget();
                if (target == null) return;
                _mapLoader.SetTarget(target);
            }
            _mapLoader.ShiftTilesIfNeeded();
        }
    }
}