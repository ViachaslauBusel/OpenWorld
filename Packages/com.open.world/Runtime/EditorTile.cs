#if UNITY_EDITOR
using Bundles;
using OpenWorld.DATA;
using OpenWorld.Helpers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorld
{
    /// <summary>
    /// Скрипт для загрузки данных тайла в режиме редактора
    /// </summary>
    public class EditorTile : MonoBehaviour, ITile
    {
        private Dictionary<MapEntity, GameObject> _mapObjectToSceneObject = new Dictionary<MapEntity, GameObject>();
        public Terrain Terrain { get; private set; }
        public TileLocation Location { get; private set; }

        private MapLoader _mapLoader;

        /// <summary>Данные для загрузки тайла</summary>
        public MapTile Data { get; private set; }
        public List<GameObject> gameObjects = new List<GameObject>();


        public void Load(TileLocation location, MapLoader mapLoader) 
        {
            Location = location;
            _mapLoader = mapLoader;
            Load(location, mapLoader.Map);
        }
        public void Load(TileLocation location, GameMap map)
        {


            Data = AssetDatabase.LoadAssetAtPath<MapTile>(location.Path);


            TerrainData terrainData = Data.TerrainData;


                GameObject terrain_obj = Terrain.CreateTerrainGameObject(terrainData);
              //  terrain_obj.layer = LayerMask.NameToLayer("Terrain");


            Terrain = terrain_obj.GetComponent<Terrain>();

           // Terrain.materialTemplate = TabSetting.TerrainMaterial;
            Terrain.heightmapPixelError = 1;
            Terrain.basemapDistance = GraphicsQualitySettings.basemapDistance;
            Terrain.treeCrossFadeLength = 50.0f;
            Terrain.treeBillboardDistance = 100.0f;
            Terrain.detailObjectDistance = GraphicsQualitySettings.DetailDistance;
            Terrain.detailObjectDensity = GraphicsQualitySettings.DetailDensity;

                terrain_obj.transform.position = location.Position;
                //  terrain_obj.layer = LayerMask.NameToLayer("Terrain");


                gameObject.name = "Tile";

                terrain_obj.transform.SetParent(transform);
          //  if(lightmapIndex >= 0) Debug.Log($"Scale on light:{Terrain.realtimeLightmapIndex}");
         //   else Debug.Log($"Scale:{Terrain.realtimeLightmapIndex}");

            if (Data.WaterTile != null)
            {
                    GameObject water = new GameObject("WaterTile");
                    MeshFilter meshFilter = water.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = water.AddComponent<MeshRenderer>();
                    meshFilter.mesh = Data.WaterTile;
              //  meshFilter.mesh.name = "water";
               // meshRenderer.material = TabSetting.WaterMaterial;
                water.transform.SetParent(transform);
                    water.transform.localPosition = new Vector3(0.0f, map.WaterLevel, 0.0f) + location.Position;
              //  water.AddComponent<WaterObject>();
            }

            foreach (MapEntity mapObject in Data.Objects)
            {
                if (_mapLoader.Settings.ObjectLayerMask.ContainsLayer(mapObject.Layer) == false) { continue; }

                GameObject prefab = mapObject.Prefab.editorAsset as GameObject;
                if (prefab == null) continue;

                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                obj.transform.position = mapObject.Position;
                obj.transform.rotation = mapObject.Rotation;
                obj.transform.localScale = mapObject.Scale;
                obj.transform.SetParent(transform);
                //foreach (MeshRenderer _m in obj.GetComponentsInChildren<MeshRenderer>()) _m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                gameObjects.Add(obj);
                _mapObjectToSceneObject.Add(mapObject, obj);
            }
        }

        public GameObject GetSceneObjectByMapObject(MapEntity mapObject)
        {
            if (_mapObjectToSceneObject.ContainsKey(mapObject))
            {
                return _mapObjectToSceneObject[mapObject];
            }
            return null;
        }

        public void Dispose()
        {
          /*  if (Terrain.lightmapIndex >= 0)
            {
                LightmapData[] data = LightmapSettings.lightmaps.Where((l) => !l.Equals(LightmapSettings.lightmaps[Terrain.lightmapIndex])).ToArray();
                LightmapSettings.lightmaps = data;
            }*/
           
           DestroyImmediate(gameObject); 
        }
    }
}
#endif