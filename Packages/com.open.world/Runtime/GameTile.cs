using Bundles;
using OpenWorld.DATA;
using OpenWorld.Loader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace OpenWorld
{
    public class GameTile : MonoBehaviour, ITile
    {
        public Tile Tile { get; private set; }
        private Coroutine coroutineLoad = null;
        private List<GameObject> objects = new List<GameObject>();
        private MapLoader mapLoader;
        private Terrain terrain;
        public void Load(TileLocation location, MapLoader mapLoader)
        {
            this.mapLoader = mapLoader;
            coroutineLoad = StartCoroutine(ELoad(location));
        }
        internal IEnumerator ELoad(TileLocation location)
        {
            // Load the Tile asset asynchronously
            var loadHandle = Addressables.LoadAssetAsync<Tile>(location.Path);

            yield return loadHandle;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Tile = loadHandle.Result;
                Debug.Log($"Loaded Tile at path: {location.Path}");
            }
            else
            {
                Debug.LogError($"Failed to load Tile at path: {location.Path}");
                yield break;
            }

            TerrainData terrainData = Tile.TerrainData;



            TaskManager.Instance.Execute(() =>
            {
                GameObject terrain_obj = Terrain.CreateTerrainGameObject(terrainData);
                terrain_obj.layer = LayerMask.NameToLayer("Terrain");

                terrain = terrain_obj.GetComponent<Terrain>();
                terrain.drawTreesAndFoliage = false;
                terrain.basemapDistance = 0;
                terrain.detailObjectDistance = 0;
                //SETTING TERRAIN <<<
                //   terrain.lightmapIndex = lightmapIndex;
                terrain.materialTemplate = mapLoader.TerrainMaterial;
                terrain.heightmapPixelError = 5;
                terrain.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                terrain.basemapDistance = GraphicsQualitySettings.basemapDistance;
                //   terrain.treeCrossFadeLength = 50.0f;
                //    terrain.treeBillboardDistance = 100.0f;
                terrain.detailObjectDistance = GraphicsQualitySettings.DetailDistance;
                terrain.detailObjectDensity = GraphicsQualitySettings.DetailDensity;

                terrain_obj.transform.position = location.Position;
                //  terrain_obj.layer = LayerMask.NameToLayer("Terrain");

                //     if (lightmapIndex >= 0) Debug.Log($"Scale on light:{terrain.realtimeLightmapIndex}");
                //    else Debug.Log($"Scale:{terrain.realtimeLightmapIndex}");

                gameObject.name = "Tile";

                //-------------------------------------------------------------------------------------------
                //------------------IOS-Focus----------------------------------------------------------------
                //-------------------------------------------------------------------------------------------

                TerrainCollider collider = terrain_obj.GetComponent<TerrainCollider>();
                collider.enabled = true;
                TerrainData data = collider.terrainData;
                var layers = terrain.terrainData.terrainLayers;
                //-------------------------------------------------------------------------------------------

                terrain_obj.transform.SetParent(transform);
            });

            if (Tile.WaterTile != null)
            {
                TaskManager.Instance.Execute(() =>
                {
                    GameObject water = new GameObject("WaterTile");
                    MeshFilter meshFilter = water.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = water.AddComponent<MeshRenderer>();
                    meshFilter.mesh = Tile.WaterTile;
                    // meshRenderer.material = SettingsQuality.Instance.WaterMaterial;
                    water.transform.SetParent(transform);
                    water.transform.localPosition = new Vector3(0.0f, mapLoader.Map.WaterLevel, 0.0f) + location.Position;
                    //   water.AddComponent<WaterObject>();
                });
            }

            foreach (MapObject mapObject in Tile.Objects)
            {

                AsyncOperationHandle loadHandler = mapObject.Prefab.LoadAssetAsync<GameObject>();
                loadHandler.Completed += (h) =>
                {
                    GameObject prefab = h.Result as GameObject;
                    if (prefab == null) return;
                    GameObject obj = Instantiate(prefab, mapObject.Position, mapObject.Rotation);

                    obj.transform.SetParent(transform);
                    obj.transform.localScale = mapObject.Scale;
                    foreach (MeshRenderer _m in obj.GetComponentsInChildren<MeshRenderer>()) _m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    obj.isStatic = true;
                    foreach (Transform _t in obj.GetComponentsInChildren<Transform>()) _t.gameObject.isStatic = true;

                    objects.Add(obj);


                    //-----IOS-Focus--------------
                    //foreach (Collider col in obj.GetComponentsInChildren<Collider>()) {
                    //    col.enabled = true;
                    //    Message message = new Message();
                    //    message.Layer = MsgLayer.System;
                    //    message.CharName = "system";
                    //    message.Msg = $"Collider on {col.gameObject.name}:{col.enabled}";

                    //    MessagesManager.Add(message);
                    //}
                    //foreach (Collider col in obj.GetComponents<Collider>()) {
                    //    col.enabled = true;
                    //    Message message = new Message();
                    //    message.Layer = MsgLayer.System;
                    //    message.CharName = "system";
                    //    message.Msg = $"Collider on {col.gameObject.name}:{col.enabled}";

                    //    MessagesManager.Add(message);
                    //}


                };
            }

            if (coroutineLoad != null)
                coroutineLoad = null;
            else DestroyImmediate(gameObject);
        }

        private void Update()
        {
            foreach (GameObject obj in objects)
            {
               //     obj.SetActive(Vector3.Distance(PlayerController.Instance.transform.position, obj.transform.position) < 25.0f);
            }
        }
        public void Dispose()
        {
            if (terrain.lightmapIndex >= 0)
            {
                LightmapData[] data = LightmapSettings.lightmaps.Where((l) => !l.Equals(LightmapSettings.lightmaps[terrain.lightmapIndex])).ToArray();
                LightmapSettings.lightmaps = data;
            }
            if (coroutineLoad == null) { Destroy(gameObject); }
            else coroutineLoad = null;
        }
    }
}
