using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.DATA
{
    [CreateAssetMenu(fileName = "Map", menuName = "Map Data", order = 50), System.Serializable]
    public class Map : ScriptableObject
    {
        public const float SIZE_KMBLOCK = 1000.0f;
        private static readonly int[] RESOLUTIONS = { 16, 32, 64, 128, 256, 512, 1024 };

        [SerializeField] string _mapName = null;
        /// <summary>Map size in kilometers</summary>
        [SerializeField] int _mapSizeKm;
        /// <summary>Number of tiles per kilometer</summary>
        [SerializeField] int _tilesPerKm;
        /// <summary>Tile size in world units</summary>
        [SerializeField] float _tileSize;
        [SerializeField] Vector2 _worldStartPoint;
        [SerializeField] Vector2 _worldEndPoint;
        /// <summary>Maximum world height</summary>
        [SerializeField] float _worldMaxHeight;
        [SerializeField] int _heightmapResolutionIndex = 2;
        [SerializeField] int _alphamapResolutionIndex = 3;
        [SerializeField] int _baseMapResolutionIndex = 3;
        [SerializeField] float _waterLevel = 0;

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------PUBLIC---------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public string MapName => _mapName;
        /// <summary>Map size in kilometers</summary>
        public int MapSizeKilometers => _mapSizeKm;
        /// <summary>Number of tiles per kilometer</summary>
        public int TilesPerKilometer => _tilesPerKm;
        /// <summary>Tile size in world units</summary>
        public float TileSize => _tileSize;
        /// <summary>Maximum world height</summary>
        public float MaximumWorldHeight => _worldMaxHeight;
        /// <summary>Heightmap resolution for terrain (tile)</summary>
        public int HeightmapResolution => RESOLUTIONS[_heightmapResolutionIndex] + 1;
        /// <summary>Texture resolution for terrain (tile)</summary>
        public int AlphamapResolution => RESOLUTIONS[_alphamapResolutionIndex];
        public int BaseMapResolution => RESOLUTIONS[_baseMapResolutionIndex];
        public float WaterLevel { get => _waterLevel; set => _waterLevel = value; }
        public Vector3 WorldStartPoint => new Vector3(_worldStartPoint.x, 0, _worldStartPoint.y);
        public Vector3 WorldEndPoint => new Vector3(_worldEndPoint.x, 0, _worldEndPoint.y);
        public string GetPath(int xKM, int yKM, int xTR, int yTR) => "Assets/MapData/" + _mapName + "/KMBlock_" + xKM + '_' + yKM + "/Tile_" + xTR + '_' + yTR + ".asset";
        public string GetPathToLight(int xKM, int yKM, int xTR, int yTR) => "Assets/MapData/" + _mapName + "(Light)/KMBlock_" + xKM + '_' + yKM + "/TRBlock_" + xTR + '_' + yTR + ".asset";

        /// <summary>
        /// Transform position from world space to map space
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Vector3 WorldToMapPoint(Vector3 position)
        {
            position -= WorldStartPoint;
            position.x = Mathf.Clamp(position.x, 0, MapSizeKilometers * SIZE_KMBLOCK);
            position.z = Mathf.Clamp(position.z, 0, MapSizeKilometers * SIZE_KMBLOCK);
            return position;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public void DisplayMapCreationGUI(ref string inputName, ref float inputHeight)
        {
            EditorGUILayout.LabelField("Map not created");
            inputName = EditorGUILayout.TextField("Name:", inputName);
            _mapSizeKm = EditorGUILayout.IntField("Map size in kilometers:", _mapSizeKm);
            _tilesPerKm = EditorGUILayout.IntSlider("Tiles per kilometer:", _tilesPerKm, 1, 100);
            _worldStartPoint = EditorGUILayout.Vector2Field("World start point:", _worldStartPoint);
            _worldMaxHeight = EditorGUILayout.FloatField("Maximum world height:", _worldMaxHeight);
            inputHeight = EditorGUILayout.FloatField("Specified world height:", inputHeight);

            _heightmapResolutionIndex = EditorGUILayout.Popup("Heightmap Resolution", _heightmapResolutionIndex, RESOLUTIONS.Select(i => $"{i + 1} x {i + 1}").ToArray());
            _alphamapResolutionIndex = EditorGUILayout.Popup("Control Texture Resolution", _alphamapResolutionIndex, RESOLUTIONS.Select(i => $"{i} x {i}").ToArray());
            _baseMapResolutionIndex = EditorGUILayout.Popup("Base Texture Resolution", _baseMapResolutionIndex, RESOLUTIONS.Select(i => $"{i} x {i}").ToArray());
        }

        public void DisplayMapSettingsGUI()
        {
            EditorGUILayout.LabelField($"Имя : {_mapName}");
            EditorGUILayout.LabelField($"Размер карты: {_mapSizeKm}X{_mapSizeKm}км");
            EditorGUILayout.LabelField($"Размер тайла: {_tileSize}X{_tileSize}м");
            EditorGUILayout.LabelField("Начало мира: " + _worldStartPoint.ToString());
            EditorGUILayout.LabelField("Максимальная высота мира: " + _worldMaxHeight.ToString());
            EditorGUILayout.LabelField("Heightmap Resolution: " + HeightmapResolution.ToString());
            _alphamapResolutionIndex = EditorGUILayout.Popup("Control Texture Resolution", _alphamapResolutionIndex, RESOLUTIONS.Select((i) => $"{i} x {i}").ToArray());
            _baseMapResolutionIndex = EditorGUILayout.Popup("Base Texture Resolution", _baseMapResolutionIndex, RESOLUTIONS.Select((i) => $"{i} x {i}").ToArray());
        }

        public void ApplyMapSetting()
        {
            foreach (Tile tile in this)
            {
                if (tile.TerrainData.alphamapResolution != AlphamapResolution)
                { tile.TerrainData.alphamapResolution = AlphamapResolution; }
                if (tile.TerrainData.baseMapResolution != BaseMapResolution)
                { tile.TerrainData.baseMapResolution = BaseMapResolution; }
                //  if (tile.terrainData.alphamapLayers != 0) Debug.Log($"tile.terrainData.alphamapLayers:{tile.terrainData.alphamapLayers}");
                EditorUtility.SetDirty(tile);
            }
        }

        public MapCreationError SetupMapProperties(ref string inputName, ref float inputHeight)
        {
            if (inputName.Length < 3) { return MapCreationError.InvalidName; }
            if (_mapSizeKm <= 0) { return MapCreationError.InvalidSize; }
            if (_worldMaxHeight <= 10.0f) { return MapCreationError.UndefinedHeight; }
            if (inputHeight < 0.0f || inputHeight > _worldMaxHeight) { return MapCreationError.IncorrectSpecifiedHeight; }
            if (Directory.Exists("Assets/MapData/" + inputName)) { return MapCreationError.NameAlreadyExists; }
            if (AssetDatabase.GetAllAssetBundleNames().Any((str) => str.Equals(_mapName))) { return MapCreationError.AssetBundleSettingFailed; }

            _mapName = inputName;
            _tileSize = SIZE_KMBLOCK / _tilesPerKm;
            _worldStartPoint = new Vector2(-(_mapSizeKm * SIZE_KMBLOCK / 2.0f), -(_mapSizeKm * SIZE_KMBLOCK / 2.0f));
            _worldEndPoint = new Vector2(_mapSizeKm * SIZE_KMBLOCK / 2.0f, _mapSizeKm * SIZE_KMBLOCK / 2.0f);

            return MapCreationError.None; // Ensure all code paths return a value
        }

        public IEnumerator GetEnumerator()
        {
            float maxProgress = MapSizeKilometers * MapSizeKilometers + TilesPerKilometer * TilesPerKilometer;
            for (int yKM = 0; yKM < MapSizeKilometers; yKM++)
            {
                for (int xKM = 0; xKM < MapSizeKilometers; xKM++)
                {
                    for (int y = 0; y < TilesPerKilometer; y++)
                    {
                        for (int x = 0; x < TilesPerKilometer; x++)
                        {
                            EditorUtility.DisplayProgressBar("OpenWorld", "processing", ((yKM * MapSizeKilometers + xKM) + (y * TilesPerKilometer + x)) / maxProgress);
                            string path = GetPath(xKM, yKM, x, y);

                            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);

                            yield return tile;
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }

        public void ResetMapProperties()
        {
            _mapName = null;
        }
#endif
    }
}