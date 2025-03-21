﻿using AssetPerformanceToolkit.AssetManagement;
using AssetPerformanceToolkit.FrameBalancer;
using OpenWorld.DATA;
using OpenWorld.Helpers;
using OpenWorld.Water;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace OpenWorld
{
    public abstract class BaseTile : MonoBehaviour
    {
        private bool _isDestroyed = false;
        private bool _isAssetLoaded = false;
        private AsyncOperationHandle<MapTile> _tileAssetHandle;
        private List<AsyncOperationHandle<GameObject>> _entityAssetHandlers = new();

        public async void Load(TileLocation location, MapSettings settings)
        {
            _tileAssetHandle = Addressables.LoadAssetAsync<MapTile>(location.Path);
            await _tileAssetHandle.Task;

            _isAssetLoaded = true;
            if (_isDestroyed || _tileAssetHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Release();
                return;
            }

            MapTile tileAsset = _tileAssetHandle.Result;

            OnAssetLoaded(tileAsset);

            FrameTaskScheduler.Execute(() =>
            {
                if (_isDestroyed) return;

                SetupTerrain(location, settings, tileAsset);
            });

            if (tileAsset.WaterTile != null)
            {
                FrameTaskScheduler.Execute(() =>
                {
                    if (_isDestroyed) return;

                    SetupWater(location, settings, tileAsset);
                });
            }

            foreach (MapEntity mapEntity in tileAsset.Entities)
            {
                if (settings.ObjectLayerMask.ContainsLayer(mapEntity.Layer) == false) { continue; }

                if (mapEntity.Prefab == null)
                {
                    Debug.LogError($"Prefab is missing on {mapEntity.ID}");
                    continue;
                }

                AsyncOperationHandle<GameObject> loadEntityHandler = Addressables.LoadAssetAsync<GameObject>(mapEntity.Prefab);
                await loadEntityHandler.Task;
                //loadEntityHandler.Completed += (h) =>
                //{
                    if (_isDestroyed || _isAssetLoaded == false)
                    {
                        Addressables.Release(loadEntityHandler);
                        return;
                    }
                    _entityAssetHandlers.Add(loadEntityHandler);

                    FrameTaskScheduler.Execute(() =>
                    {
                        if (_isDestroyed) return;

                        GameObject obj = SetupEntity(loadEntityHandler.Result, mapEntity);
                        if (obj != null) OnEntityInstantiated(obj, mapEntity);
                    });
              //  };
            }
        }

        protected virtual void OnAssetLoaded(MapTile tileAsset)
        {
        }

        protected virtual void OnEntityInstantiated(GameObject obj, MapEntity mapEntity)
        {
        }

        private GameObject SetupEntity(GameObject prefab, MapEntity mapEntity)
        {
            if (prefab == null) return null;

            GameObject obj = Instantiate(prefab, mapEntity.Position, mapEntity.Rotation);

            if(mapEntity.ID != 0)
            {
                obj.GetComponent<MapEntityIdentifier>()?.Initialize(mapEntity.ID);
            }

            obj.transform.SetParent(transform);
            obj.transform.localScale = mapEntity.Scale;
            foreach (MeshRenderer _m in obj.GetComponentsInChildren<MeshRenderer>()) _m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            obj.isStatic = true;
            foreach (Transform _t in obj.GetComponentsInChildren<Transform>()) _t.gameObject.isStatic = true;

            return obj;
        }

        private void SetupWater(TileLocation location, MapSettings settings, MapTile tileAsset)
        {
            GameObject water = new GameObject("WaterTile");
            water.transform.SetParent(transform);
            MeshFilter meshFilter = water.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = water.AddComponent<MeshRenderer>();
            water.AddComponent<WaterTag>();
            meshFilter.mesh = tileAsset.WaterTile;
            meshRenderer.material = settings.WaterMaterial;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
          
            water.transform.localPosition = new Vector3(0.0f, location.Map.WaterLevel - 1f, 0.0f) + location.Position;
        }

        private void SetupTerrain(TileLocation location, MapSettings settings, MapTile tileAsset)
        {
            GameObject terrain_obj = Terrain.CreateTerrainGameObject(tileAsset.TerrainData);
            terrain_obj.layer = settings.TerrainLayer;

            var terrain = terrain_obj.GetComponent<Terrain>();
            terrain.drawTreesAndFoliage = true;
            terrain.basemapDistance = 0;
            terrain.detailObjectDistance = 0;
            //SETTING TERRAIN <<<
            //   terrain.lightmapIndex = lightmapIndex;
            terrain.materialTemplate = settings.TerrainMaterial;
            terrain.heightmapPixelError = 5;
            terrain.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            terrain.basemapDistance = GraphicsQualitySettings.basemapDistance;
            //   terrain.treeCrossFadeLength = 50.0f;
            //    terrain.treeBillboardDistance = 100.0f;
            terrain.detailObjectDistance = GraphicsQualitySettings.DetailDistance;
            terrain.detailObjectDensity = GraphicsQualitySettings.DetailDensity;

            terrain_obj.transform.position = location.Position;

            gameObject.name = "Tile";
            terrain_obj.transform.SetParent(transform);
        }

        private void Release()
        {
            if (_isAssetLoaded == false) return;

            Addressables.Release(_tileAssetHandle);

            foreach (var handler in _entityAssetHandlers)
            {
                Addressables.Release(handler);
            }
            _isAssetLoaded = false;
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
            Release();
        }

        internal virtual void Destroy()
        {
            _isDestroyed = true;
        }
    }
}
