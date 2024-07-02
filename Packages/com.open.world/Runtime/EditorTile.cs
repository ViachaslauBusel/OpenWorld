#if UNITY_EDITOR
using Bundles;
using OpenWorld.DATA;
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
        public Terrain Terrain { get; private set; }
        public TileLocation Location { get; private set; }
        /// <summary>Данные для загрузки тайла</summary>
        public Tile Data { get; private set; }
        public List<GameObject> gameObjects = new List<GameObject>();
        public void Load(BundlesMap bundlesMap, TileLocation location, MapLoader mapLoader) 
        {
            Location = location;
            Load(bundlesMap, location, mapLoader.Map);
        }
        public void Load(BundlesMap bundlesMap, TileLocation location, Map map)
        {


            Data = AssetDatabase.LoadAssetAtPath<Tile>(location.Path);


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

            foreach (MapObject mapObject in Data.Objects)
            {
                GameObject prefab = mapObject.Prefab.Asset;
                if (prefab == null) continue;

                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                obj.transform.position = mapObject.Position;
                obj.transform.rotation = mapObject.Rotation;
                obj.transform.localScale = mapObject.Scale;
                obj.transform.SetParent(transform);
                obj.name = mapObject.GetHashCode().ToString();
                obj.isStatic = true;
                foreach (Transform _t in obj.GetComponentsInChildren<Transform>()) _t.gameObject.isStatic = true;
                foreach (MeshRenderer _m in obj.GetComponentsInChildren<MeshRenderer>()) _m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                gameObjects.Add(obj);
            }
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