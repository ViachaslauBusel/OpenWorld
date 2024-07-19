#if UNITY_EDITOR
using OpenWorld.DATA;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace OpenWorldEditor
{
    //Создание карты
    public class MapGeneration
    {
        private static float maxProgress;
        private static float totalProgress;

        public static void GenerationWorld(GameMap map, float defaultHeight)
        {
            string path = "Assets/MapData/" + map.MapName;
            Directory.CreateDirectory(path);

            maxProgress = map.MapSizeKilometers * map.MapSizeKilometers;

            string folder;
            for (int x=0; x< map.MapSizeKilometers; x++)
            {
                EditorUtility.DisplayProgressBar("OpenWorld", "Map Generation",totalProgress / maxProgress);
                for (int y=0; y< map.MapSizeKilometers; y++)
                {

                    totalProgress = (x * map.MapSizeKilometers)+y;

                    folder = path + "/KMBlock_" + x + '_' + y;
                    Directory.CreateDirectory(folder);
                    GenerationTerrain(x, y, map, defaultHeight);
                }
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerationTerrain(int xKM, int yKM, GameMap map, float defaultHeight)
        {

            float height = defaultHeight / map.MaximumWorldHeight;
            for (int x = 0; x < map.TilesPerKilometer; x++)
            {
                EditorUtility.DisplayProgressBar("OpenWorld", "Map Generation", (totalProgress + (x / (float)map.TilesPerKilometer)) / maxProgress);
                for (int y = 0; y < map.TilesPerKilometer; y++)
                {
                    TerrainData terrainData = new TerrainData();
                    terrainData.name = "/TRData_" + x + '_' + y;
                   
                    terrainData.heightmapResolution = map.HeightmapResolution;
                    terrainData.alphamapResolution = map.AlphamapResolution;

                   terrainData.size = new Vector3(map.TileSize, map.MaximumWorldHeight, map.TileSize);

                    MapTile mapElement = ScriptableObject.CreateInstance<MapTile>();

                    // AssetDatabase.AddObjectToAsset(mapElement.terrainData, mapElement);
                    string path = map.GetPath(xKM, yKM, x, y);

                    AssetDatabase.CreateAsset(mapElement, path);
                    // AssetImporter.GetAtPath(path).assetBundleName = map.MapName+"/map";

                    AssetDatabase.AddObjectToAsset(terrainData, mapElement);
                  //  AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(terrainData));
                    mapElement.SetTerrainData(terrainData);

                    AddToAddressableGroup(mapElement, map.MapName);

                    /*   TerrainLayer[] terrainLayers = new TerrainLayer[1];
                       for (int k = 0; k < 1; k++)
                       {
                        terrainLayers[k] = Resources.Load("Gras1") as TerrainLayer;
                       }

                    terrainData.terrainLayers = terrainLayers;
                       terrainData.SetAlphamaps(0, 0, GetMapTexture(terrainData.alphamapHeight, terrainData.alphamapWidth, terrainLayers.Length));*/

                    //Задать высоту точек высот >>>>
                    float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

                    for (int yh = 0; yh < heights.GetLength(1); yh++)
                    {
                        for (int xh = 0; xh < heights.GetLength(0); xh++)
                        {
                            heights[xh, yh] = height;
                        }
                    }
                    terrainData.SetHeights(0, 0, heights);
                    //Задать высоту точек высот <<<<
                }

            }
        }

        private static void AddToAddressableGroup(UnityEngine.Object asset, string groupName)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup group = settings.FindGroup(groupName);

            if (group == null)
            {
                // Optionally create the group if it doesn't exist
                group = settings.CreateGroup(groupName, false, false, true, null);
            }

            string assetPath = AssetDatabase.GetAssetPath(asset);
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            settings.CreateOrMoveEntry(guid, group);
        }
    }
}
#endif