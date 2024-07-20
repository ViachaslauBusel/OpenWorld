using Bundles;
using OpenWorld.DATA;
using OpenWorld.Loader;
using System.Collections;
using UnityEngine;
using System.Linq;
using OpenWorld.Helpers;
using System;


namespace OpenWorld
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField] GameMap _map;
        [SerializeField] MapSettings _settings;
        private BaseTile[,] _tiles;
        private TileLocation[,] _tilesLocations;
        private Vector4 _border = new Vector4();
        private Transform _trackingObj;
        private Transform _fackeTrackingObj;

        private bool _ready = false;

        public bool Ready => _ready;
        public GameMap Map => _map;
        public MapSettings Settings => _settings;
        public Transform TrackingObject => _trackingObj;


        private void Awake()
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            enabled = false;
        }
     
        public bool isDone
        {
            get
            {
                if (_tiles == null) return false;
                int tileCount = _tiles.GetLength(0) * _tiles.GetLength(1);//Total tiles
                int loadedTiles = GetLoadedTileCount();
                return tileCount == loadedTiles;
            }
        }

        public float progress
        {
            get
            {
                if (_tiles == null) return 0.0f;
                int tileCount = _tiles.GetLength(0) * _tiles.GetLength(1);//Total tiles
                int loadedTiles = GetLoadedTileCount();
                return loadedTiles / (float)tileCount;
            }
        }

        private int GetLoadedTileCount()
        {
            int loadedTiles = 0;
            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    if (_tiles[x, y] != null && !_tiles[x, y].name.Contains("Load"))
                    {
                        loadedTiles++;
                    }
                }
            }
            return loadedTiles;
        }

        public void DestroyMap()
        {
            if (!Ready) return;
            LightmapSettings.lightmaps = null;
            enabled = false;
            _ready = false;
           
            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    _tiles[x, y]?.Destroy();
                }
            }
            _tiles = null;
            _tilesLocations = null;
        }

        public void LoadMapInPoint(Vector3 point)
        {
            if(_trackingObj == null)
            {
                _fackeTrackingObj ??= new GameObject("FakeTrackingObj").transform;
                _trackingObj = _fackeTrackingObj;
            }

            _trackingObj.position = point;
            LoadMap();
        }

        public void LoadMap()
        {
            if (Ready) { Debug.LogError("the map is already loaded"); return; }
            if (_map == null || _map.IsValid() == false) { Debug.LogError("Map is not valid"); return; }

            SetupMap();
        }

        private void SetupMap()
        {
            _tiles = new BaseTile[_settings.AreaVisible * 2 + 1, _settings.AreaVisible * 2 + 1];
            _tilesLocations = new TileLocation[_settings.AreaVisible * 2 + 1, _settings.AreaVisible * 2 + 1];

            CalculateBorder();

            Vector3 startPosition = _map.ClampPosition(_trackingObj.position);
            TileLocation startLocation = _map.CalculateLocation(startPosition);

            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    _tilesLocations[x, y] = startLocation;
                    _tilesLocations[x, y].Xtr -= _settings.AreaVisible - x;
                    _tilesLocations[x, y].Ytr -= _settings.AreaVisible - y;
                    CreateTerrain(x, y, _tilesLocations[x, y]);
                }
            }
            _ready = enabled = true;
        }

        private void CalculateBorder()
        {
            float tileSize = _map.TileSize;
            Vector3 position = _trackingObj.position;

            float baseX = position.x - (position.x % tileSize);
            float baseZ = position.z - (position.z % tileSize);

            _border.w = baseX - tileSize * 0.1f; // Left
            _border.y = baseX + tileSize * 1.1f; // Right
            _border.x = baseZ - tileSize * 0.1f; // Down
            _border.z = baseZ + tileSize * 1.1f; // Up
        }

        private void Update()
        {
            ShiftTilesIfNeeded();
        }

        public void ShiftTilesIfNeeded()
        {
            Vector3 position = _map.ClampPosition(_trackingObj.position);

            if (position.x < _border.w)//left
            {
                _border.w -= _map.TileSize;
                _border.y -= _map.TileSize;

                for (int x = _tiles.GetLength(0)-1; x>=0; x--)
                {
                    for(int y=0; y<_tiles.GetLength(1); y++)
                    {
                        _tilesLocations[x, y].Xtr--;
                        //Dispose tile if it is the last one
                        if (x == _tiles.GetLength(0) - 1) _tiles[x, y]?.Destroy();
                        //Shift tiles
                        if (x != 0) _tiles[x, y] = _tiles[x - 1, y];
                        //Create new tile
                        else CreateTerrain(x, y, _tilesLocations[x,y]);
                    }
                }
              //  TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
            else if(position.x > _border.y)//right
            {
                _border.w += _map.TileSize;
                _border.y += _map.TileSize;

                for (int x = 0; x < _tiles.GetLength(0); x++)
                { 
                    for (int y = 0; y < _tiles.GetLength(1); y++)
                    {
                        _tilesLocations[x, y].Xtr++;
                        if (x == 0) _tiles[x, y]?.Destroy();
                        if (x < _tiles.GetLength(0) - 1) _tiles[x, y] = _tiles[x + 1, y];
                        else CreateTerrain(x, y, _tilesLocations[x, y]);
                    }
                }
              //  TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
            else if(position.z < _border.x)//up
            {
                _border.x -= _map.TileSize;
                _border.z -= _map.TileSize;

                for (int y = _tiles.GetLength(1) - 1; y >= 0; y--)
                {
                    for (int x = 0; x < _tiles.GetLength(0); x++)
                    {
                        _tilesLocations[x, y].Ytr--;
                        if (y == _tiles.GetLength(1) - 1) _tiles[x, y]?.Destroy();
                        if (y != 0) _tiles[x, y] = _tiles[x, y-1];
                        else CreateTerrain(x, y, _tilesLocations[x, y]);
                    }
                }
               // TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
            else if(position.z > _border.z)//down
            {
                _border.x += _map.TileSize;
                _border.z += _map.TileSize;

                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    for (int x = 0; x < _tiles.GetLength(0); x++)
                    {
                        _tilesLocations[x, y].Ytr++;
                        if (y == 0) _tiles[x, y]?.Destroy();
                        if (y < _tiles.GetLength(1) - 1) _tiles[x, y] = _tiles[x , y+1];
                        else CreateTerrain(x, y, _tilesLocations[x, y]);
                    }
                }
              //  TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
        }

        private void CreateTerrain(int x, int y, TileLocation location)
        {
            GameObject obj = new GameObject("LoadingTile");
#if UNITY_EDITOR
            _tiles[x, y] = obj.AddComponent<EditorTile>();
#else
            _tiles[x, y] = obj.AddComponent<GameTile>();
#endif
            obj.transform.SetParent(transform);

            if (_map.IsLocationValid(location) == false) { obj.name = "OutsideTile"; return; }
            
            _tiles[x, y].Load(location, _settings);
        }

        public void SetTarget(Transform target)
        {
            _trackingObj = target;
            if(_fackeTrackingObj != null)
            {
                Destroy(_fackeTrackingObj.gameObject);
            }
        }

        public void SetMap(GameMap map)
        {
            _map = map;
        }

        public void SetSetting(MapSettings mapSettings)
        {
            _settings = mapSettings;
        }
    }
}