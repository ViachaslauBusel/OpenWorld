#if UNITY_EDITOR
using OpenWorld;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.SceneWindow
{
    public class WorldLoader
    {
        private static MapLoader mapLoader;

        public static MapLoader MapEditor
        {
            get
            {
                if (mapLoader == null)
                {
                    GameObject obj = GameObject.Find("MapEditor");
                    if (obj != null)
                        mapLoader = obj.GetComponent<MapLoader>();

                    if (mapLoader == null)
                        mapLoader = CreateMap();
                }
                return mapLoader;
            }
        }


        public static void Destroy()
        {
            if (mapLoader != null) GameObject.DestroyImmediate(mapLoader.gameObject);
            GameObject obj = GameObject.Find("MapEditor");
            while (obj != null)
            {
                GameObject.DestroyImmediate(obj);
                obj = GameObject.Find("MapEditor");
            }
            Resources.UnloadUnusedAssets();
        }

        public static void Reload()
        {
            if (mapLoader != null)
            {
                Transform tracking = mapLoader.TrackingObject;
                GameObject.DestroyImmediate(mapLoader.gameObject);
                mapLoader = MapEditor; ;
            }
        }
    

        private static MapLoader CreateMap()
        {
            if (TabSetting.Map == null)
            {
                EditorGUILayout.HelpBox("Карта не выбрана", MessageType.Error);
                return null;
            }
            Transform target = FindTarget();
            if (target == null) return null;

            GameObject obj = new GameObject();// GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Map.prefab"));
            obj.name = "MapEditor";
            MapLoader mapLoader = obj.AddComponent<MapLoader>();
            mapLoader.SetTarget(target);
            mapLoader.SetMap(TabSetting.Map);
            GraphicsQualitySettings.AreaVisible = TabSetting.areaVisible;
            mapLoader.LoadMap();
            RenderSettings.fog = false;
            return mapLoader;

        }

        internal static MapLoader LoadMap()
        {
            return MapEditor;
        }

        private static Transform FindTarget()
        {
            Camera _camera = SceneView.lastActiveSceneView.camera;
            if (_camera == null) return null;
            return _camera.transform;
        }
        /// <summary>
        /// Обновление позиции обьекта вокруг которой отрисовается карта
        /// </summary>
        internal static void OnSceneGUI()
        {
            if (mapLoader == null) return;
            if(mapLoader.TrackingObject == null)//Если карта потеряла обьект слежения
            {
                Transform target = FindTarget();
                if (target == null) return;
                mapLoader.SetTarget(target);
            }
            mapLoader.ShiftTilesIfNeeded();
        }
    }
}
#endif