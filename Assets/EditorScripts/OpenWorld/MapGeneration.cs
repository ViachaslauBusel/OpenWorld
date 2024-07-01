﻿#if UNITY_EDITOR
using OpenWorld.DATA;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    //Создание карты
    public class MapGeneration
    {
        private static float maxProgress;
        private static float totalProgress;

        public static void GenerationWorld(Map map, float defaultHeight)
        {
            string path = "Assets/MapData/" + map.Name;
            Directory.CreateDirectory(path);

            maxProgress = map.Size * map.Size;

            string folder;
            for (int x=0; x< map.Size; x++)
            {
                EditorUtility.DisplayProgressBar("OpenWorld", "Map Generation",totalProgress / maxProgress);
                for (int y=0; y< map.Size; y++)
                {

                    totalProgress = (x * map.Size)+y;

                    folder = path + "/KMBlock_" + x + '_' + y;
                    Directory.CreateDirectory(folder);
                    GenerationTerrain(x, y, map, defaultHeight);
                }
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerationTerrain(int xKM, int yKM, Map map, float defaultHeight)
        {

            float height = defaultHeight / map.MaxHeight;
            for (int x = 0; x < map.TilesPerBlock; x++)
            {
                EditorUtility.DisplayProgressBar("OpenWorld", "Map Generation", (totalProgress + (x / (float)map.TilesPerBlock)) / maxProgress);
                for (int y = 0; y < map.TilesPerBlock; y++)
                {
                    TerrainData terrainData = new TerrainData();
                    terrainData.name = "/TRData_" + x + '_' + y;
                   
                    terrainData.heightmapResolution = map.HeightmapResolution;
                    terrainData.alphamapResolution = map.AlphamapResolution;

                   terrainData.size = new Vector3(map.TileSize, map.MaxHeight, map.TileSize);

                    Tile mapElement = ScriptableObject.CreateInstance<Tile>();

                    // AssetDatabase.AddObjectToAsset(mapElement.terrainData, mapElement);
                    string path = map.GetPath(xKM, yKM, x, y);

                    AssetDatabase.CreateAsset(mapElement, path);
                    AssetImporter.GetAtPath(path).assetBundleName = map.Name+"/map";

                    AssetDatabase.AddObjectToAsset(terrainData, mapElement);
                  //  AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(terrainData));
                    mapElement.terrainData = terrainData;
                   
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
       
    }
}
#endif