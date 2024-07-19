﻿#if UNITY_EDITOR
using OpenWorld.DATA;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{

    public class TerrainExport
    {
        public static void Export(GameMap map)
        {
           
            float totalProgress = 0; 

          
            using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"Export/terrain.dat", FileMode.Create)))
            { 
                int maxTile = map.MapSizeKilometers * map.TilesPerKilometer;
                float maxProgress = maxTile * maxTile;
                stream_out.Write(map.MapSizeKilometers);
                stream_out.Write(map.TilesPerKilometer);//Тайлов на километр
                stream_out.Write(map.HeightmapResolution);

                for (int y = 0; y < maxTile; y++)
                {
                  
                    for (int x = 0; x < maxTile; x++)
                    {
                        totalProgress = (y * maxTile) + x;
                        EditorUtility.DisplayProgressBar("OpenWorld", "Export terrain.dat", totalProgress / maxProgress);


                        int xKM = x / map.TilesPerKilometer;
                        int yKM = y / map.TilesPerKilometer;
                        int xTR = x % map.TilesPerKilometer;
                        int yTR = y % map.TilesPerKilometer;
                       
                        Write(xKM,yKM, xTR, yTR, map, stream_out); 
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
      
        private static void Write(int xKM, int yKM, int xTR, int yTR, GameMap map, BinaryWriter stream_out)
        {


            string path = map.GetPath(xKM, yKM, xTR, yTR);
                    MapTile mapElement = AssetDatabase.LoadAssetAtPath<MapTile>(path);
           
            if (mapElement != null)
            {
              //  Debug.Log(mapElement.terrainData.heightmapResolution);
                int size = (mapElement.TerrainData.heightmapResolution * mapElement.TerrainData.heightmapResolution) * sizeof(float);
                stream_out.Write(size);//Размер массива высот
                float[,] heights = mapElement.TerrainData.GetHeights(0, 0, mapElement.TerrainData.heightmapResolution, mapElement.TerrainData.heightmapResolution);
                for (int i = 0; i < mapElement.TerrainData.heightmapResolution; i++)
                {
                    for (int j = 0; j < mapElement.TerrainData.heightmapResolution; j++)
                    {
                        stream_out.Write(heights[i, j] * mapElement.TerrainData.size.y);
                    }
                }
            }
           
        }
    }
}
#endif